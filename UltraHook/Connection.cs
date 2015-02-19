using System;
using System.Collections.Generic;
using System.Text;

namespace UltraHook
{
	public class Connection : MarshalByRefObject
	{
		public int X;
		public int Y;
		public CHType chType;
		public bool drawDot;
		public bool closing;
		public bool refreshData;
		public int getFPS;
		public int limitFPS;
		public DXVersion dxVersion;
		public object customCHBD; // 
		/*
		 * custom CrossHair Build Data Explanation, use:
		 * - int[] for DX9 with the CHBuild Constructor.
		 * - System.IO.Stream for DX11 with an Image.
		 */

		public Connection()
		{
			X = Y = 100;
			chType = CHType.none;
			drawDot = false;
			closing = false;
			refreshData = false;
			limitFPS = -1;
			dxVersion = DXVersion.unknown;
		}
	}

	public enum CHType : int
	{
		none,
		lines,
		thinlines,
		square,
		custom,
		DOT,
		END
	}

	public enum DXVersion : byte
	{
		unknown,
		DX09,
		DX10,
		DX10_1,
		DX11,
		DX11_1
	}
}
