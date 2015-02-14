using System;
using System.Collections.Generic;
using System.Text;

namespace UltraHook
{
	public class Connection : MarshalByRefObject
	{
		public int X;
		public int Y;
		public int CHT;
		public bool drawDot;
		public string drawString;
		public bool closing;
		public int[] customCHBD; // custom CrossHair Build Data

		public Connection()
		{
			X = Y = 100;
			CHT = CH_none;
			drawDot = false;
			drawString = string.Empty;
			closing = false;
		}

		public Connection(int _X, int _Y, int _CHT, bool _dd, string _ds, int[] _cchbd)
		{
			X = _X; Y = _Y; CHT = _CHT; drawDot = _dd; drawString = _ds; customCHBD = _cchbd;
			closing = false;
		}

		public Connection Clone()
		{
			return new Connection(X, Y, CHT, drawDot, drawString, (int[])customCHBD.Clone());
		}

		public const int CH_none = 0;
		public const int CH_lines = 1;
		public const int CH_thinlines = 2;
		public const int CH_square = 3;
		public const int CH_custom = 4;
		public const int CH_DOT = 5;
		public const int CH_END = 6;
	}
}
