using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;


using EasyHook;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace UltraHook
{
	class HookD3D_11 : ID3DControl
	{
		public override string Name { get { return "Direct3D 11"; } protected set { } }

		List<IntPtr> _d3d11VTblAddresses = null;
		List<IntPtr> _dxgiSwapChainVTblAddresses = null;

		LocalHook DXGISwapChain_PresentHook = null;
		//LocalHook DXGISwapChain_ResizeTargetHook = null;

		DX11GUI dx11gui = null;

		public HookD3D_11()
		{

		}

		public override void Hook()
		{
			_d3d11VTblAddresses = new List<IntPtr>();
			_dxgiSwapChainVTblAddresses = new List<IntPtr>();
			SharpDX.Direct3D11.Device device;
			SwapChain swapChain;

			using (SharpDX.Windows.RenderForm renderForm = new SharpDX.Windows.RenderForm())
			{
				SharpDX.Direct3D11.Device.CreateWithSwapChain(
					DriverType.Hardware,
					DeviceCreationFlags.None,
					CreateSwapChainDescription(renderForm.Handle),
					out device,
					out swapChain);

				if (device != null && swapChain != null)
				{
					using (device)
					{
						_d3d11VTblAddresses.AddRange(DXTools.GetVTblAddresses(device.NativePointer, 43));
						using (swapChain)
						{
							_dxgiSwapChainVTblAddresses.AddRange(DXTools.GetVTblAddresses(swapChain.NativePointer, 18));
						}
					}
				}
				else
				{
					Core.Log("Hook: Device creation failed");
				}
			}

			DXGISwapChain_PresentHook = LocalHook.Create(
				_dxgiSwapChainVTblAddresses[(int)DXGISwapChainVTbl.Present],
				new DXGISwapChain_PresentDelegate(PresentHook),
				this);

			/*DXGISwapChain_ResizeTargetHook = LocalHook.Create(
			_dxgiSwapChainVTblAddresses[(int)DXGI.DXGISwapChainVTbl.ResizeTarget],
			new DXGISwapChain_ResizeTargetDelegate(ResizeTargetHook),
			this);*/


			DXGISwapChain_PresentHook.ThreadACL.SetExclusiveACL(new Int32[1]);
			//DXGISwapChain_ResizeTargetHook.ThreadACL.SetExclusiveACL(new Int32[1]);

			while (!closed)
				System.Threading.Thread.Sleep(10);
		}

		public static SwapChainDescription CreateSwapChainDescription(IntPtr windowHandle)
		{
			return new SharpDX.DXGI.SwapChainDescription
			{
				BufferCount = 1,
				Flags = SharpDX.DXGI.SwapChainFlags.None,
				IsWindowed = true,
				ModeDescription = new ModeDescription(100, 100, new Rational(60, 1), SharpDX.DXGI.Format.R8G8B8A8_UNorm),
				OutputHandle = windowHandle,
				SampleDescription = new SampleDescription(1, 0),
				SwapEffect = SharpDX.DXGI.SwapEffect.Discard,
				Usage = SharpDX.DXGI.Usage.RenderTargetOutput
			};
		}

		int PresentHook(IntPtr swapChainPtr, int syncInterval, SharpDX.DXGI.PresentFlags flags)
		{
			SwapChain swapChain = (SharpDX.DXGI.SwapChain)swapChainPtr;
			//SharpDX.Direct3D11.Device device = swapChain.GetDevice<SharpDX.Direct3D11.Device>();

			if (!closed)
			{
				if (dx11gui == null)
					dx11gui = new DX11GUI(swapChain);

				dx11gui.RenderCrosshair();
			}

			swapChain.Present(syncInterval, flags);
			return SharpDX.Result.Ok.Code;
		}


		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
		delegate int DXGISwapChain_PresentDelegate(IntPtr swapChainPtr, int syncInterval, /* int */ SharpDX.DXGI.PresentFlags flags);

		//[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
		//delegate int DXGISwapChain_ResizeTargetDelegate(IntPtr swapChainPtr, ref ModeDescription newTargetParameters);
	}
}
