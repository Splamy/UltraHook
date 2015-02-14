using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing.Imaging;

using EasyHook;
using SharpDX.Direct3D;
using SharpDX.Direct3D9;

namespace UltraHook
{
	internal class HookD3D_09 : ID3DControl
	{
		public string Name { get; set; }

		LocalHook Direct3DDevice_EndSceneHook = null;
		LocalHook Direct3DDevice_ResetHook = null;

		FontDescription fdesc;
		Font dxFont = null;

		Connection con;
		bool closed = false;

		CHBuild[] prebakedCH = new CHBuild[Connection.CH_END];

		public HookD3D_09()
		{
			Name = "Direct3D 9";
		}

		public void Hook()
		{
			List<IntPtr> id3dDeviceFunctionAddresses = new List<IntPtr>();
			Device device;

			using (Direct3D d3d = new Direct3D())
			{
				using (device = new Device(d3d, 0, DeviceType.NullReference, IntPtr.Zero, CreateFlags.HardwareVertexProcessing, new PresentParameters() { BackBufferWidth = 1, BackBufferHeight = 1 }))
				{
					id3dDeviceFunctionAddresses.AddRange(Tools.GetVTblAddresses(device.NativePointer, 119));
				}
			}

			con = RemoteHooking.IpcConnectClient<Connection>("uh_data");

			//System.Windows.Forms.MessageBox.Show(id3dDeviceFunctionAddresses[42].ToString());
			Direct3DDevice_EndSceneHook = LocalHook.Create(
			   id3dDeviceFunctionAddresses[42],
			   new Direct3D9Device_EndSceneDelegate(EndSceneHook),
			   this);

			Direct3DDevice_ResetHook = LocalHook.Create(
			   id3dDeviceFunctionAddresses[16],
			   new Direct3D9Device_ResetDelegate(ResetHook),
			   this);

			Direct3DDevice_EndSceneHook.ThreadACL.SetExclusiveACL(new Int32[1]);
			Direct3DDevice_ResetHook.ThreadACL.SetExclusiveACL(new Int32[1]);

			#region fontsinit
			fdesc = new FontDescription()
			{
				Height = 25,
				FaceName = "Arial",
				Italic = false,
				Width = 12,
				MipLevels = 1,
				CharacterSet = FontCharacterSet.Default,
				OutputPrecision = FontPrecision.Default,
				Quality = FontQuality.Antialiased,
				PitchAndFamily = FontPitchAndFamily.Default | FontPitchAndFamily.DontCare,
				Weight = FontWeight.Normal,
			};
			#endregion

			while (!closed)
				System.Threading.Thread.Sleep(10);
		}

		int ResetHook(IntPtr devicePtr, ref PresentParameters presentParameters)
		{
			Device device = (Device)devicePtr;
			try
			{
				dxFont = null;
				device.Reset(presentParameters);
				return SharpDX.Result.Ok.Code;
			}
			catch (SharpDX.SharpDXException sde)
			{
				return sde.ResultCode.Code;
			}
			catch
			{
				return SharpDX.Result.Ok.Code;
			}
		}

		int EndSceneHook(IntPtr devicePtr)
		{
			Device device = (Device)devicePtr;

			if (!closed)
			{
				if (con.closing)
				{
					closed = true;
					con.closing = false;
				}

				//initialize font
				//if (dxFont == null)
				//	dxFont = new Font(device, fdesc);

				if (prebakedCH[con.CHT] == null)
				{
					switch (con.CHT)
					{
					case Connection.CH_none: prebakedCH[con.CHT] = CHBuild.DefaultNone; break;
					case Connection.CH_lines: prebakedCH[con.CHT] = genOffset(CHBuild.DefaultLines); break;
					case Connection.CH_thinlines: prebakedCH[con.CHT] = genOffset(CHBuild.DefaultThinLines); break;
					case Connection.CH_square: prebakedCH[con.CHT] = genOffset(CHBuild.DefaultSquare); break;
					case Connection.CH_custom: prebakedCH[con.CHT] = genOffset(CHBuilder.ToCHBuild(con.customCHBD)); break; //
					default: break; //
					}
					prebakedCH[Connection.CH_DOT] = genOffset(CHBuild.DefaultDot);
				}

				if (con.drawDot)
					device.Clear(ClearFlags.Target, prebakedCH[Connection.CH_DOT].colors[0], 0, 0, prebakedCH[Connection.CH_DOT].rects[0]);

				CHBuild selected = prebakedCH[con.CHT];
				if (selected != null)
				{
					for (int i = 0; i < selected.colors.Length; i++)
						device.Clear(ClearFlags.Target, selected.colors[i], 0, 0, selected.rects[i]);
					//dxFont.DrawText(null, "bluub", con.X / 2 - 3, con.Y / 2 - 19, SharpDX.Color.White);
				}
			}

			device.EndScene();
			return SharpDX.Result.Ok.Code;
		}

		public CHBuild genOffset(CHBuild input)
		{
			if (input == null) return null;

			CHBuild ret = new CHBuild();
			ret.rects = new SharpDX.Rectangle[input.rects.Length][];
			ret.colors = (SharpDX.ColorBGRA[])input.colors.Clone();
			ret.size = input.size;
			for (int i = 0; i < input.rects.Length; i++)
			{
				ret.rects[i] = new SharpDX.Rectangle[input.rects[i].Length];
				for (int j = 0; j < ret.rects[i].Length; j++)
				{
					SharpDX.Rectangle cpy = input.rects[i][j];
					ret.rects[i][j] = new SharpDX.Rectangle(
						con.X / 2 - ret.size / 2 + cpy.X,
						con.Y / 2 - ret.size / 2 + cpy.Y,
						cpy.Width,
						cpy.Height);
				}
			}
			return ret;
		}

		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
		delegate int Direct3D9Device_EndSceneDelegate(IntPtr device);
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
		delegate int Direct3D9Device_ResetDelegate(IntPtr device, ref PresentParameters presentParameters);
	}
}
