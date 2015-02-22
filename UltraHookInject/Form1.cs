using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Ipc;
using System.Diagnostics;
using System.Security.Principal;
using System.IO;

using EasyHook;
using UltraHook;

namespace UltraHookInject
{
	public partial class Form1 : Form
	{
		Dictionary<string, string> config;

		IpcServerChannel conServer;
		Connection conMain;
		string conName = "uh_data";
		string targetproc = "KillingFloor";
		string customImage = "";

		int[] cchd = null; // custom crosshair data
		Rectangle[][] buildrect;
		Color[] buildcol;

		ResizeTool rtWidth = new ResizeTool();
		ResizeTool rtHeight = new ResizeTool();
		Size rsMaxSize = new Size(335, 215);
		Size rsMinSize = new Size(335, 55);

		Dictionary<string, ProcInfo> processDict = new Dictionary<string, ProcInfo>();
		Process ptarget = null;
		bool isInjected = false;
		bool getOnce = false;

		// init/deinit ////////////////////////////////////////////////////////

		public Form1()
		{
			InitializeComponent();

			mTitlebar1.Icon = Icon.ToBitmap();
			mTitlebar1.DragElement = this;
			mTitlebar1.ClickClose += AppExit;
			mTitlebar1.ClickMinimize += Minimize;

			// todo load/save all stuff
			conMain = new Connection();
			if (File.Exists("config.ini"))
			{
				config = ReadFile("config.ini");
				try
				{
					numResX.Value = conMain.X = Convert.ToInt32(config["X"]);
					numResY.Value = conMain.Y = Convert.ToInt32(config["Y"]);
					conMain.chType = cobCHT.SelectedIndex = Math.Max(0, Math.Min(CHType.custom, Convert.ToInt32(config["crosshair"])));
					conMain.drawDot = checkUseDot.Checked = Convert.ToBoolean(config["drawDot"]);
					cbxProcessList.Text = targetproc = config["game"];
					customImage = config["customimage"];
					checkAutoInject.Checked = Convert.ToBoolean(config["autoinject"]);
					checkAutoReInject.Checked = Convert.ToBoolean(config["autoreinject"]);

					timer1.Enabled = checkAutoInject.Checked;
				}
				catch (Exception ex)
				{
					Core.Log("Could not read config.ini -> " + ex.Message);
				}
			}

			try
			{
				conServer = RemoteHooking.IpcCreateServer<Connection>(
					ref conName,
					WellKnownObjectMode.SingleCall,
					conMain,
					new WellKnownSidType[] { WellKnownSidType.LocalSid });
			}
			catch
			{
				MessageBox.Show("IPC Startup failed");
			}

			this.Size = rsMinSize;
		}

		public void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			config["X"] = numResX.Value.ToString();
			config["Y"] = numResY.Value.ToString();
			config["crosshair"] = cobCHT.SelectedIndex.ToString();
			config["drawDot"] = checkUseDot.Checked.ToString();
			config["game"] = targetproc;
			config["customimage"] = customImage;
			config["autoinject"] = checkAutoInject.Checked.ToString();
			config["autoreinject"] = checkAutoReInject.Checked.ToString();

			WriteFile(config, "config.ini");

			try
			{
				int cnt = 0;
				conMain.closing = true;
				if (conServer != null)
				{
					lblInfo.Text = "Waiting for target to close (200ms)";
					while (conMain.closing && cnt < 200)
					{
						System.Threading.Thread.Sleep(1);
						cnt++;
						Application.DoEvents();
					}
					conServer.StopListening(null);
				}
			}
			catch { }
		}

		// injecting and detecting ////////////////////////////////////////////

		public void timer1_Tick(object sender, EventArgs e)
		{
			if (isInjected && ptarget != null && !ptarget.HasExited)
				return;
			else
				isInjected = false;

			ProcInfo pitarget = cbxProcessList.SelectedItem as ProcInfo;
			string starget = cbxProcessList.Text;

			if (pitarget != null && !cbxProcessList.DroppedDown)
				targetproc = pitarget.procname;
			else if (pitarget == null && starget != null)
				targetproc = starget;

			ptarget = null;
			Process[] res = Process.GetProcessesByName(targetproc);
			if (res.Length > 0)
				ptarget = res[0];

			if (ptarget == null)
			{
				lblInfo.Text = "No process found";
				return;
			}

			try
			{
				ProcessModuleCollection pmc = ptarget.Modules;
				foreach (ProcessModule pm in pmc)
				{
					if (pm.ModuleName.CompareTo("EasyHook32.dll") == 0 || pm.ModuleName.CompareTo("EasyHook64.dll") == 0)
					{
						isInjected = true;
						return;
					}
				}

				RemoteHooking.Inject(ptarget.Id,
					InjectionOptions.Default, // DoNotRequireStrongName if everything else fails
					typeof(Core).Assembly.Location,
					typeof(Core).Assembly.Location,
					conName);
				getOnce = false;
				isInjected = true;
			}
			catch (Exception ex)
			{
				lblInfo.Text = "Err: " + ex.Message;
				Core.Log("Injection Error: " + ex.Message);
				return;
			}

			lblInfo.Text = "Injection OK (waiting for response)";
			if (!checkAutoReInject.Checked)
				timer1.Stop();
			resizer.Start();
		}

		public void RefreshDDList()
		{
			cbxProcessList.Items.Clear();

			foreach (Process p in Process.GetProcesses())
			{
				if (processDict.ContainsKey(p.ProcessName))
				{
					ProcInfo pidic = processDict[p.ProcessName];
					if (pidic != null)
					{
						if (checkFilerList.Checked && pidic.usedDX == DXVersion.unknown) continue;
						cbxProcessList.Items.Add(pidic);
					}
					continue;
				}

				ProcInfo pi = new ProcInfo();
				pi.usedDX = DXVersion.unknown;

				bool founddx = false;
				try
				{

					foreach (ProcessModule pm in p.Modules)
					{
						if (pm.ModuleName == "d3d9.dll")
						{
							pi.usedDX = DXVersion.DX09;
							founddx = true;
						}
						else if (pm.ModuleName == "d3d10.dll")
						{
							pi.usedDX = DXVersion.DX10;
							founddx = true;
						}
						else if (pm.ModuleName == "d3d11.dll")
						{
							pi.usedDX = DXVersion.DX11;
							founddx = true;
						}
					}
				}
				catch (Exception) { processDict.Add(p.ProcessName, null); continue; }

				if (checkFilerList.Checked && !founddx) continue;

				pi.procname = p.ProcessName;

				processDict.Add(p.ProcessName, pi);

				cbxProcessList.Items.Add(pi);
			}
		}

		// Interface interaction //////////////////////////////////////////////

		public void cobCHT_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cobCHT.SelectedIndex > CHType.custom)
			{
				conMain.chType = CHType.custom;
				customImage = cobCHT.Text;
				TryLoadImage(customImage);
			}
			else
				conMain.chType = cobCHT.SelectedIndex;

			openImage.Enabled = conMain.chType == CHType.custom;
		}

		private void cobCHT_DropDown(object sender, EventArgs e)
		{
			cobCHT.Items.Clear();
			cobCHT.Items.AddRange(new[] { "none", "lines", "thin lines", "square", "custom" });

			foreach (string fname in Directory.EnumerateFiles(Application.StartupPath))
			{
				if (fname.EndsWith(".png") ||
					fname.EndsWith(".bmp") ||
					fname.EndsWith(".gif") ||
					fname.EndsWith(".tga"))
				{
					cobCHT.Items.Add(fname.Substring(fname.LastIndexOf('\\') + 1));
				}
			}
		}

		public void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			conMain.drawDot = checkUseDot.Checked;
		}

		public void openImage_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			if (ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

			TryLoadImage(ofd.FileName);

			preview.Invalidate();
		}

		public void SetFPSLimit_Changed(object sender, EventArgs e)
		{
			if (checkFPSLimit.Checked) conMain.limitFPS = (int)(1000 / numFPSLimit.Value);
			else conMain.limitFPS = -1;
		}

		private void comboBox1_DropDown(object sender, EventArgs e)
		{
			RefreshDDList();
			isInjected = false;
		}

		private void checkSettings_CheckedChanged(object sender, EventArgs e)
		{
			if (checkSettings.Checked)
			{
				DoResize(rsMaxSize.Width, rsMaxSize.Height);
			}
			else
			{
				DoResize(rsMinSize.Width, rsMinSize.Height);
			}
		}

		private void btnStartSearch_Click(object sender, EventArgs e)
		{
			timer1.Enabled = true;
		}

		private void checkAutoInject_CheckedChanged(object sender, EventArgs e)
		{
			timer1.Enabled = checkAutoInject.Checked;
		}

		private void numResX_ValueChanged(object sender, EventArgs e)
		{
			conMain.X = (int)numResX.Value;
			conMain.refreshData = true;
		}

		private void numResY_ValueChanged(object sender, EventArgs e)
		{
			conMain.Y = (int)numResY.Value;
			conMain.refreshData = true;
		}

		// Settings Helptools /////////////////////////////////////////////////

		public static Dictionary<string, string> ReadFile(string filename)
		{
			FileStream fs = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
			StreamReader input = new StreamReader(fs);
			Dictionary<string, string> result = new Dictionary<string, string>();
			string s;
			int index;
			while (!input.EndOfStream)
			{
				s = input.ReadLine();
				if (!s.StartsWith(";") && !s.StartsWith("//") && !s.StartsWith("#"))
				{
					index = s.IndexOf('=');
					result.Add(s.Substring(0, index).Trim(), s.Substring(index + 1).Trim());
				}
			}
			fs.Close();
			return result;
		}

		public static void WriteFile(Dictionary<string, string> values, string filename)
		{
			FileStream fs = File.Open(filename, FileMode.Create, FileAccess.Write);
			StreamWriter output = new StreamWriter(fs);
			foreach (string key in values.Keys)
			{
				output.Write(key);
				output.Write('=');
				output.WriteLine(values[key]);
			}
			output.Flush();
			fs.Close();
		}

		public void TryLoadImage(string imgname)
		{
			if (!File.Exists(imgname)) return;

			switch (conMain.dxVersion)
			{
			case DXVersion.DX09:
				using (var bmp = new Bitmap(imgname))
				{
					SetImageRasterized(bmp);
				}
				break;
			case DXVersion.DX10:
				break;
			case DXVersion.DX10_1:
				break;
			case DXVersion.DX11:
				Stream str = conMain.customCHBD as Stream;
				if (str != null) str.Close();
				conMain.customCHBD = File.Open(imgname, FileMode.Open, FileAccess.Read);
				break;
			case DXVersion.DX11_1:
				break;
			case DXVersion.unknown:
				break;
			default:
				break;
			}

			conMain.refreshData = true;
		}

		private void AppExit()
		{
			Application.Exit();
		}

		private void Minimize()
		{
			this.WindowState = FormWindowState.Minimized;
		}

		// DX09 Helptools /////////////////////////////////////////////////////

		public void SetImageRasterized(Bitmap bmp)
		{
			if (bmp.Width != bmp.Height)
			{
				MessageBox.Show("The image is not quadratical (not supported yet)");
				return;
			}

			#region Check if image has alpha
			bool hasAlpha = false;
			Dictionary<int, int> index = new Dictionary<int, int>();
			for (int i = 0; i < bmp.Width; i++)
			{
				for (int j = 0; j < bmp.Height; j++)
				{
					var pixel = bmp.GetPixel(i, j);
					if (pixel.A != 255) hasAlpha = true;
					if (!hasAlpha)
					{
						int colorkey = pixel.ToArgb();
						if (index.ContainsKey(colorkey))
							index[colorkey] = index[colorkey] + 1;
						else
							index.Add(colorkey, 1);
					}
				}
			}
			#endregion

			#region TransparenceyRequest
			Color bestselect = Color.Transparent;
			if (!hasAlpha)
			{
				int max = -1;
				int best = 0;
				foreach (var v in index)
					if (v.Value > max) { best = v.Key; max = v.Value; }
				bestselect = Color.FromArgb(best);

				StringBuilder strb = new StringBuilder();
				strb.Append("The image doesn't seem to use alpha, the autodetect suggests:\n");
				string colorname;
				if (GetColorName(bestselect, out colorname))
					strb.Append(colorname);
				strb.Append(" [R=");
				strb.Append(bestselect.R);
				strb.Append(" G=");
				strb.Append(bestselect.G);
				strb.Append(" B=");
				strb.Append(bestselect.B);
				strb.Append("]\nDo you want to use this color as transparency key?");
				if (MessageBox.Show(this, strb.ToString(), "Color", MessageBoxButtons.YesNo) != DialogResult.Yes)
					bestselect = Color.Transparent;
			}
			#endregion

			Dictionary<Color, List<Rectangle>> rGen = new Dictionary<Color, List<Rectangle>>();

			int imagesize = bmp.Width; // = bmp.Height

			#region Calculate rectangles
			bool[,] check = new bool[imagesize, imagesize];
			for (int i = 0; i < bmp.Width; i++)
			{
				for (int j = 0; j < bmp.Height; j++)
				{
					if (check[i, j]) continue;
					var pixel = bmp.GetPixel(i, j);
					if (pixel.A != 255 || (pixel == bestselect && !hasAlpha)) { check[i, j] = true; continue; }

					bool rowok;

					int maxb = j;
					do
					{
						rowok = true;
						maxb++;
						if (maxb == imagesize) break;
						if (bmp.GetPixel(i, maxb) != pixel) rowok = false;
						if (check[i, maxb]) rowok = false;
					}
					while (rowok);
					maxb--;

					int maxr = i;
					do
					{
						rowok = true;
						maxr++;
						if (maxr == imagesize) break;
						for (int k = j; k <= maxb; k++)
						{
							if (bmp.GetPixel(maxr, k) != pixel) { rowok = false; break; }
							if (check[maxr, k]) { rowok = false; break; }
						}
					} while (rowok);
					maxb++;

					for (int m = i; m < maxr; m++)
						for (int k = j; k < maxb; k++)
							check[m, k] = true;

					if (rGen.ContainsKey(pixel))
						rGen[pixel].Add(new Rectangle(i, j, maxr - i, maxb - j));
					else
					{
						var list = new List<Rectangle>();
						list.Add(new Rectangle(i, j, maxr - i, maxb - j));
						rGen.Add(pixel, list);
					}

				}
			}
			#endregion

			buildrect = new Rectangle[rGen.Count][];
			buildcol = new Color[rGen.Count];

			int cnt = 0;
			foreach (var v in rGen)
			{
				buildrect[cnt] = v.Value.ToArray();
				buildcol[cnt] = v.Key;
				cnt++;
			}

			cchd = CHBuilder.FromExtern(buildrect, buildcol, imagesize);

			conMain.customCHBD = cchd;
		}

		public bool GetColorName(Color colorToCheck, out string name)
		{
			int checkcol = colorToCheck.ToArgb();
			foreach (KnownColor kc in Enum.GetValues(typeof(KnownColor)))
			{
				Color known = Color.FromKnownColor(kc);
				if (checkcol == known.ToArgb())
				{
					name = known.Name;
					return true;
				}
			}
			name = string.Empty;
			return false;
		}

		// Visualizer /////////////////////////////////////////////////////////

		public void preview_Paint(object sender, PaintEventArgs e)
		{
			if (buildrect == null || buildcol == null) return;

			for (int i = 0; i < buildrect.Length; i++)
			{
				e.Graphics.DrawRectangles(new Pen(buildcol[i]), buildrect[i]);
			}
		}

		public void resizer_Tick(object sender, EventArgs e)
		{
			if (!rtHeight.IsActive && !rtWidth.IsActive)
				resizer.Interval = 100;
			else
			{
				this.Height = rtHeight.DoResizeStep(this.Height);
				this.Width = rtWidth.DoResizeStep(this.Width);
				this.Invalidate();
			}

			if (conMain.dxVersion == DXVersion.unknown) return;

			int locGetFPS = conMain.getFPS;
			lblFPS.Text = locGetFPS.ToString();

			if (getOnce) return; // from here only one time initializations per injection

			int locDxVersion = conMain.dxVersion;
			lblInfo.Text = "Successfully hooked " + Global.DXNames[locDxVersion];

			// todo: remove this when drawdot is implemented for other dx versions
			checkUseDot.Enabled = conMain.dxVersion == DXVersion.DX09;

			if (cobCHT.SelectedIndex >= CHType.custom) TryLoadImage(customImage);

			getOnce = true;
		}

		public void DoResize(int tWidth, int tHeigth)
		{
			rtWidth.Set(this.Width, tWidth);
			rtHeight.Set(this.Height, tHeigth);
			resizer.Interval = 1;
			resizer.Start();
		}

		private void Form1_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.DrawRectangle(Pens.Silver, 0, 0, this.Width - 1, this.Height - 1);
		}
	}

	internal class ResizeTool
	{
		private ResizeStage rsStage;
		private int rsSpeed;
		private int rsTarget;
		private int rsOverlap;
		private bool rsMaximizing;
		public bool IsActive { get { return rsStage != ResizeStage.Sleeping; } protected set { } }

		public ResizeTool()
		{
			rsStage = ResizeStage.Sleeping;
		}

		public void Set(int current, int target)
		{
			if (current == target)
			{
				rsStage = ResizeStage.Sleeping;
				return;
			}
			rsTarget = target;
			rsSpeed = 0;
			rsMaximizing = current < rsTarget;
			if (rsMaximizing) rsOverlap = rsTarget + ((rsTarget - current) / 3);
			else rsOverlap = rsTarget - ((current - rsTarget) / 3);
			rsStage = ResizeStage.Accelerating;
		}

		public int DoResizeStep(int currentVal)
		{
			switch (rsStage)
			{
			case ResizeStage.Sleeping: return currentVal; // do nothing
			case ResizeStage.Accelerating:
				if (rsMaximizing) rsSpeed++;
				else rsSpeed--;
				if ((rsMaximizing && currentVal > rsTarget) || (!rsMaximizing && currentVal < rsTarget)) rsStage++;
				return currentVal + rsSpeed;
			case ResizeStage.Breaking:
				rsSpeed /= 2;
				if (rsSpeed <= 10) rsStage++;
				return currentVal + rsSpeed;
			case ResizeStage.Reversing:
				if (rsMaximizing) rsSpeed--;
				else rsSpeed++;
				if ((rsMaximizing && currentVal + rsSpeed < rsTarget) || (!rsMaximizing && currentVal + rsSpeed > rsTarget))
				{
					rsStage = ResizeStage.Sleeping;
					return rsTarget;
				}
				return currentVal + rsSpeed;
			}
			return currentVal;
		}

		private enum ResizeStage
		{
			Sleeping,
			Accelerating,
			Breaking,
			Reversing
		}
	}

	internal class ProcInfo
	{
		public string procname;
		public int usedDX;
		public override string ToString()
		{
			if (usedDX == DXVersion.unknown)
				return procname;
			else
				return procname + " [" + Global.DXNames[usedDX] + "]";
		}
	}

	public class Global
	{
		private static string[] _DXNames;
		public static string[] DXNames
		{
			get
			{
				if (_DXNames == null)
				{
					_DXNames = new string[6];
					_DXNames[0] = "unknown";
					_DXNames[1] = "DX09";
					_DXNames[2] = "DX10";
					_DXNames[3] = "DX10.1";
					_DXNames[4] = "DX11";
					_DXNames[5] = "DX11.1";
				}
				return _DXNames;
			}
			protected set { }
		}
	}
}
