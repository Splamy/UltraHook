using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

using EasyHook;
using SharpDX.Direct3D;
using SharpDX.Direct3D9;

namespace UltraHook
{
	public class HookD3D_09 : D3DHook
	{
		public override string Name { get { return "Direct3D 9"; } protected set { } }

		LocalHook Direct3DDevice_EndSceneHook = null;
		LocalHook Direct3DDevice_ResetHook = null;

		FontDescription fdesc;
		//Font dxFont = null;

		CHBuild[] prebakedCH = new CHBuild[(int)CHType.END];
		FPSTool fpsTool = new FPSTool();

		public HookD3D_09()
		{
			connection.dxVersion = DXVersion.DX09;
		}

		public override void Hook()
		{
			List<IntPtr> id3dDeviceFunctionAddresses = new List<IntPtr>();
			Device device;

			using (Direct3D d3d = new Direct3D())
			{
				using (device = new Device(d3d, 0, DeviceType.NullReference, IntPtr.Zero, CreateFlags.HardwareVertexProcessing, new PresentParameters() { BackBufferWidth = 1, BackBufferHeight = 1 }))
				{
					id3dDeviceFunctionAddresses.AddRange(DXTools.GetVTblAddresses(device.NativePointer, 119));
				}
			}

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
		}

		int ResetHook(IntPtr devicePtr, ref PresentParameters presentParameters)
		{
			Device device = (Device)devicePtr;
			try
			{
				//dxFont = null;
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

			if (!closed && true)
			{
				//don't initialize font, it MAY crash
				//if (dxFont == null)
				//	dxFont = new Font(device, fdesc);

				int chTypei = 0;
				chTypei = (int)connection.chType;

				if (prebakedCH[chTypei] == null || connection.refreshData)
				{
					switch (connection.chType)
					{
					case CHType.none: prebakedCH[chTypei] = CHBuild.DefaultNone; break;
					case CHType.lines: prebakedCH[chTypei] = CHBuild.DefaultLines.genOffset(); break;
					case CHType.thinlines: prebakedCH[chTypei] = CHBuild.DefaultThinLines.genOffset(); break;
					case CHType.square: prebakedCH[chTypei] = CHBuild.DefaultSquare.genOffset(); break;
					case CHType.custom:
						int[] ibuff = connection.customCHBD as int[];
						if (ibuff != null) prebakedCH[chTypei] = new CHBuild(ibuff).genOffset();
						break;
					}
					prebakedCH[(int)CHType.DOT] = CHBuild.DefaultDot.genOffset();
				}

				if (connection.drawDot)
					device.Clear(ClearFlags.Target, prebakedCH[(int)CHType.DOT].colors[0], 0, 0, prebakedCH[(int)CHType.DOT].rects[0]);

				CHBuild selected = prebakedCH[chTypei];
				if (selected != null)
				{
					for (int i = 0; i < selected.colors.Length; i++)
						device.Clear(ClearFlags.Target, selected.colors[i], 0, 0, selected.rects[i]);
					//dxFont.DrawText(null, "bluub", con.X / 2 - 3, con.Y / 2 - 19, SharpDX.Color.White);
				}


				//http://www.unknowncheats.me/forum/d3d-tutorials-and-source/58821-simple-2d-circle-using-drawprimitiveup.html
				//device.DrawUserPrimitives(PrimitiveType.LineStrip)
			}


			connection.getFPS = fpsTool.getFPS();
			fpsTool.limitFPS();

			device.EndScene();
			return SharpDX.Result.Ok.Code;
		}

		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
		delegate int Direct3D9Device_EndSceneDelegate(IntPtr device);
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
		delegate int Direct3D9Device_ResetDelegate(IntPtr device, ref PresentParameters presentParameters);
	}
}
