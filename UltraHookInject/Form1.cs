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

		int[] cchd = null; // custom crosshair data
		Rectangle[][] buildrect;
		Color[] buildcol;

		/*ResizeStage rsStage = ResizeStage.Sleeping;
		int rsSpeed = 0;
		int rsTarget = 0;
		int rsOverlap = 0;
		bool rsMaximizing = true;*/
		ResizeTool rtWidth = new ResizeTool();
		ResizeTool rtHeight = new ResizeTool();
		Size rsMinSize = new Size(350, 135);

		bool getOnce = false;

		// init/deinit ////////////////////////////////////////////////////////

		public Form1()
		{
			InitializeComponent();

			// todo load/save all stuff
			conMain = new Connection();
			if (File.Exists("config.ini"))
			{
				config = ReadFile("config.ini");
				try
				{
					conMain.X = Convert.ToInt32(config["X"]);
					conMain.Y = Convert.ToInt32(config["Y"]);
					conMain.chType = (CHType)Convert.ToInt32(config["crosshair"]);
					cobCHT.SelectedIndex = (int)conMain.chType;
					conMain.drawDot = Convert.ToBoolean(config["drawDot"]);
					targetproc = config["game"];
					// custom ch path
					// auto inject
					// auto reinject
				}
				catch
				{
					Core.Log("Could not read config.ini");
				}
			}
		}

		public void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			WriteFile(config, "config.ini");

			try
			{
				int cnt = 0;
				conMain.closing = true;
				if (conServer != null)
				{
					conServer.StopListening(null);
					lblInfo.Text = "Waiting for target to close (200ms)";
					while (conMain.closing && cnt < 200)
					{
						System.Threading.Thread.Sleep(1);
						cnt++;
						Application.DoEvents();
					}
				}
			}
			catch { }
		}

		// injecting and detecting ////////////////////////////////////////////

		public void timer1_Tick(object sender, EventArgs e)
		{
			// todo logical injecting

			Process[] res = Process.GetProcessesByName(targetproc);
			if (res.Length == 0)
			{
				lblInfo.Text = "No process found";
				return;
			}

			ProcessModuleCollection pmc = res[0].Modules;

			conServer = RemoteHooking.IpcCreateServer<Connection>(
				ref conName,
				WellKnownObjectMode.SingleCall,
				conMain,
				new WellKnownSidType[] { WellKnownSidType.LocalSid });

			try
			{
				getOnce = false;

				RemoteHooking.Inject(res[0].Id,
					InjectionOptions.Default,
				   typeof(Connection).Assembly.Location,
				   typeof(Connection).Assembly.Location,
				   conName);
			}
			catch (Exception ex)
			{
				lblInfo.Text = "Not enough permissions";
				Core.Log("Injection Error: " + ex.Message);
				return;
			}

			lblInfo.Text = "UltraHook - Injection OK";
			timer1.Stop();
			resizer.Start();
		}

		// Interface interaction //////////////////////////////////////////////

		public void cobCHT_SelectedIndexChanged(object sender, EventArgs e)
		{
			conMain.chType = (CHType)cobCHT.SelectedIndex;
			if (conMain.chType == CHType.custom)
			{
				openImage.Enabled = true;
				conMain.customCHBD = cchd;
			}
			else openImage.Enabled = false;
		}

		public void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			conMain.drawDot = checkUseDot.Checked;
		}

		public void openImage_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			if (ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

			switch (conMain.dxVersion)
			{
			case DXVersion.DX09:
				using (var bmp = new Bitmap(ofd.FileName))
				{
					setImageRasterized(bmp);
				}
				break;
			case DXVersion.DX10:
				break;
			case DXVersion.DX10_1:
				break;
			case DXVersion.DX11:
				conMain.customCHBD = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read);
				break;
			case DXVersion.DX11_1:
				break;
			case DXVersion.unknown:
				break;
			default:
				break;
			}

			conMain.refreshData = true;
			preview.Invalidate();
		}

		public void SetFPSLimit_Changed(object sender, EventArgs e)
		{
			if (checkFPSLimit.Checked) conMain.limitFPS = (int)(1000 / numFPSLimit.Value);
			else conMain.limitFPS = -1;
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

		// DX09 Helptools /////////////////////////////////////////////////////

		public void setImageRasterized(Bitmap bmp)
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
			}

			if (conMain.dxVersion == DXVersion.unknown) return;

			int locGetFPS = conMain.getFPS;
			lblFPS.Text = locGetFPS.ToString();

			if (getOnce) return; // from here only one time initializations per injection

			DXVersion locDxVersion = conMain.dxVersion;
			lblInfo.Text = "UltraHook - Successfully hooked " + locDxVersion.ToString();

			// todo: remove this when drawdot is implemented for other dx versions
			checkUseDot.Enabled = conMain.dxVersion == DXVersion.DX09;

			getOnce = true;
		}

		public void DoResize(int tWidth, int tHeigth)
		{
			rtWidth.Set(this.Width, tWidth);
			rtHeight.Set(this.Height, tHeigth);
			resizer.Interval = 1;
			resizer.Start();
		}

		private class ResizeTool
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
						rsStage++;
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

		Random r = new Random();
		private void lblInfo_Click(object sender, EventArgs e)
		{
			DoResize(r.Next(100, 800), r.Next(100, 800)); // wobbel debug demo
		}
	}
}
