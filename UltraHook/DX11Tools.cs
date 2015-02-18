using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.Toolkit.Graphics;

namespace UltraHook
{
	internal class DX11GUI
	{
		GraphicsDevice graphics;
		Texture image;
		SpriteBatch sprb;
		SharpDX.Toolkit.Graphics.BlendState blState = null;
		bool initok = false;
		bool initcrash = false;

		public DX11GUI(SharpDX.DXGI.SwapChain _swapChain)
		{
			try
			{
				Device device = _swapChain.GetDevice<Device>();
				graphics = GraphicsDevice.New(device);
				sprb = new SpriteBatch(graphics);

				blState = SharpDX.Toolkit.Graphics.BlendState.New(
					graphics,
					BlendOption.SourceAlpha,
					BlendOption.InverseSourceAlpha,
					BlendOperation.Add,
					BlendOption.One,
					BlendOption.Zero,
					BlendOperation.Add);
			}
			catch (Exception ex)
			{
				initcrash = true;
				Core.Log("DX11 GUI initcrash: " + ex.Message);
			}
		}

		private bool loadData()
		{
			if (D3DHook.connection.refreshData || !initok)
			{
				initok = false;
				try
				{
					System.IO.Stream imgInput = D3DHook.connection.customCHBD as System.IO.Stream;
					if (imgInput == null) return false;
					image = Texture.Load(graphics, imgInput); // "crosshair.png"

					initok = true;
					D3DHook.connection.refreshData = false;
				}
				catch (Exception ex)
				{
					initcrash = true;
					Core.Log("DX11 GUI loadData failed: " + ex.Message);
					return false;
				}
			}
			return true;
		}

		public void RenderCrosshair()
		{
			if (initcrash) return;
			if (!loadData()) return;

			sprb.Begin(SpriteSortMode.Deferred, blState);
			sprb.Draw(image, new RectangleF(0, 0, 100, 100), Color.White);
			sprb.End();
		}
	}

	internal enum DXGISwapChainVTbl : short
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
