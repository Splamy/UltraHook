using System;
using System.Collections.Generic;
using System.Text;

namespace UltraHook
{
	public class Connection : MarshalByRefObject
	{
		public int X;
		public int Y;
		public int chType;
		public bool drawDot;
		public bool closing;
		public bool refreshData;
		public int getFPS;
		public int limitFPS;
		public int dxVersion;
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

	public class CHType // yes we need this shit, no enums allowed !!!
	{
		public const int none = 0;
		public const int lines = 1;
		public const int thinlines = 2;
		public const int square = 3;
		public const int custom = 4;
		public const int DOT = 5;
		public const int END = 6;
	}

	public class DXVersion // same here
	{
		public const int unknown = 0;
		public const int DX09 = 1;
		public const int DX10 = 2;
		public const int DX10_1 = 3;
		public const int DX11 = 4;
		public const int DX11_1 = 5;
	}
}
