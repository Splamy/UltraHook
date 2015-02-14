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
		IpcServerChannel conServer;
		Connection conMain;
		string conName = "uh_data";
		string targetproc = "KillingFloor";

		int[] cchd = null; // custom crosshair data

		public Form1()
		{
			InitializeComponent();

			conMain = new Connection();
			if (File.Exists("config.ini"))
			{
				Dictionary<string, string> config = ReadFile("config.ini");
				try
				{
					conMain.X = Convert.ToInt32(config["X"]);
					conMain.Y = Convert.ToInt32(config["Y"]);
					conMain.CHT = Convert.ToInt32(config["crosshair"]);
					cobCHT.SelectedIndex = conMain.CHT;
					conMain.drawDot = Convert.ToBoolean(config["drawDot"]);
					targetproc = config["game"];
				}
				catch { }
			}


		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			Process[] res = Process.GetProcessesByName(targetproc); // KillingFloor
			if (res.Length == 0)
			{
				label1.Text = "No process found";
				return;
			}

			conServer = RemoteHooking.IpcCreateServer<Connection>(
				ref conName,
				WellKnownObjectMode.SingleCall,
				conMain,
				new WellKnownSidType[] { WellKnownSidType.LocalSid });

			try
			{
				RemoteHooking.Inject(res[0].Id,
					InjectionOptions.Default,
				   typeof(Connection).Assembly.Location,
				   typeof(Connection).Assembly.Location,
				   conName);
			}
			catch
			{
				label1.Text = "Not enough permissions";
				return;
			}

			label1.Text = "UltraHook - Injection OK";
			timer1.Stop();
		}

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

		private void cobCHT_SelectedIndexChanged(object sender, EventArgs e)
		{
			conMain.CHT = cobCHT.SelectedIndex;
			if (conMain.CHT == Connection.CH_custom)
			{
				openFromImg.Enabled = true;
				conMain.customCHBD = cchd;
			}
			//else openFromImg.Enabled = false;
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			conMain.drawDot = checkBox1.Checked;
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				int cnt = 0;
				conMain.closing = true;
				if (conServer != null)
				{
					conServer.StopListening(null);
					label1.Text = "Waiting for target to close (200ms)";
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

		//fuck comments, fuck this method, fuck everything
		private void openFromImg_CheckedChanged(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			//if (ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

			Bitmap bmp = new Bitmap("crosshair.png"); // ofd.FileName
			if (bmp.Width != bmp.Height)
			{
				MessageBox.Show("The image is not quadratical");
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
			panel1.Invalidate();
		}

		Rectangle[][] buildrect;
		Color[] buildcol;

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

		private void panel1_Paint(object sender, PaintEventArgs e)
		{
			if (buildrect == null || buildcol == null) return;

			for (int i = 0; i < buildrect.Length; i++)
			{
				e.Graphics.DrawRectangles(new Pen(buildcol[i]), buildrect[i]);
			}
		}
	}
}
