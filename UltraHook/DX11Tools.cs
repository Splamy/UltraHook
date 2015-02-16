using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.Toolkit.Graphics;

namespace UltraHook
{
	public class DX11GUI
	{
		//SharpDX.DXGI.SwapChain swapChain;
		//Device device;
		GraphicsDevice graphics;
		Texture image;
		SpriteBatch sprb;
		SharpDX.Toolkit.Graphics.BlendState blState = null;
		bool initok = false;

		public DX11GUI(SharpDX.DXGI.SwapChain _swapChain)
		{
			//swapChain = _swapChain;
			Device device = _swapChain.GetDevice<Device>();

			try
			{
				graphics = GraphicsDevice.New(device);
				image = Texture.Load(graphics, "crosshair.png");
				sprb = new SpriteBatch(graphics);

				blState = SharpDX.Toolkit.Graphics.BlendState.New(
					graphics,
					BlendOption.SourceAlpha,
					BlendOption.InverseSourceAlpha,
					BlendOperation.Add,
					BlendOption.One,
					BlendOption.Zero,
					BlendOperation.Add);

				initok = true;
			}
			catch (Exception ex)
			{
				Core.Log("DX11 GUI Failed: " + ex.Message);
			}
		}

		public void RenderCrosshair()
		{
			if (!initok) return;

			sprb.Begin(SpriteSortMode.Deferred, blState);
			sprb.Draw(image, new RectangleF(0, 0, 100, 100), Color.White);
			sprb.End();
		}
	}

	public enum DXGISwapChainVTbl : short
	{
		// IUnknown
		QueryInterface = 0,
		AddRef = 1,
		Release = 2,

		// IDXGIObject
		SetPrivateData = 3,
		SetPrivateDataInterface = 4,
		GetPrivateData = 5,
		GetParent = 6,

		// IDXGIDeviceSubObject
		GetDevice = 7,

		// IDXGISwapChain
		Present = 8,
		GetBuffer = 9,
		SetFullscreenState = 10,
		GetFullscreenState = 11,
		GetDesc = 12,
		ResizeBuffers = 13,
		ResizeTarget = 14,
		GetContainingOutput = 15,
		GetFrameStatistics = 16,
		GetLastPresentCount = 17,
	}
}
