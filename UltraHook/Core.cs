using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Reflection;

using EasyHook;

/*
 * Based on spazzarama's DirectX Recorder from here:
 * https://github.com/spazzarama/Direct3DHook/
 */

namespace UltraHook
{
	public class Core : IEntryPoint
	{
		ID3DControl loadedControl;

		public Core(RemoteHooking.IContext param, string pChannelName)
		{
			ID3DControl.connection = RemoteHooking.IpcConnectClient<Connection>(pChannelName);
		}

		public void Run(RemoteHooking.IContext param, string pChannelName)
		{
			startHook();
		}

		public void startHook()
		{
			IntPtr d3D9Loaded = IntPtr.Zero;
			IntPtr d3D10Loaded = IntPtr.Zero;
			//IntPtr d3D10_1Loaded = IntPtr.Zero;
			IntPtr d3D11Loaded = IntPtr.Zero;
			//IntPtr d3D11_1Loaded = IntPtr.Zero;

			d3D9Loaded = DXTools.GetModuleHandle("d3d9.dll");
			d3D10Loaded = DXTools.GetModuleHandle("d3d10.dll");
			//d3D10_1Loaded = Tools.GetModuleHandle("d3d10_1.dll");
			d3D11Loaded = DXTools.GetModuleHandle("d3d11.dll");
			//d3D11_1Loaded = Tools.GetModuleHandle("d3d11_1.dll");

			if (d3D9Loaded != IntPtr.Zero)
				loadedControl = new HookD3D_09();
			else if (d3D10Loaded != IntPtr.Zero)
				loadedControl = new HookD3D_10();
			else if (d3D11Loaded != IntPtr.Zero)
				loadedControl = new HookD3D_11();

			if (loadedControl != null)
			{
				Core.Log("Found: " + loadedControl.Name);

				try
				{
					loadedControl.Hook();
					Core.Log("Hook started!");

					while (!ID3DControl.closed)
						System.Threading.Thread.Sleep(10);
				}
				catch (Exception e)
				{
					Core.Log("ERROR:" + e.Message);
				}
			}
		}

		public static void Log(string s)
		{
			System.IO.File.AppendAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "output.txt"), s + "\r\n");
		}
	}
}
