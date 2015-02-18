using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using SharpDX;

namespace UltraHook
{
	internal class ColorPalette
	{
		public static readonly ColorBGRA ColorWhite = new ColorBGRA(255, 255, 255, 255);
		public static readonly ColorBGRA ColorBlack = new ColorBGRA(0, 0, 0, 255);

		public static readonly ColorBGRA ColorRed = new ColorBGRA(255, 0, 0, 255);
		public static readonly ColorBGRA ColorGreen = new ColorBGRA(0, 255, 0, 255);
		public static readonly ColorBGRA ColorBlue = new ColorBGRA(0, 0, 255, 255);

		public static readonly ColorBGRA ColorPink = new ColorBGRA(255, 0, 255, 255);
		public static readonly ColorBGRA ColorCyan = new ColorBGRA(0, 255, 255, 255);
		public static readonly ColorBGRA ColorYellow = new ColorBGRA(255, 255, 0, 255);

		public static readonly ColorBGRA ColorLime = new ColorBGRA(127, 255, 0, 255);
		public static readonly ColorBGRA ColorOrange = new ColorBGRA(255, 127, 0, 255);
		public static readonly ColorBGRA ColorPurple = new ColorBGRA(127, 0, 255, 255);
		public static readonly ColorBGRA ColorSpringGreen = new ColorBGRA(0, 255, 127, 255);
		public static readonly ColorBGRA ColorLightBlue = new ColorBGRA(0, 255, 127, 255);
	}

	internal class CHBuild
	{
		public Rectangle[][] rects;
		public ColorBGRA[] colors;
		public int size;

		public CHBuild()
		{
			rects = new Rectangle[0][];
			colors = new ColorBGRA[0];
			size = 0;
		}

		public CHBuild(Rectangle[][] _rects, ColorBGRA[] _colors, int _size)
		{
			rects = _rects;
			colors = _colors;
			size = _size;
		}

		public CHBuild(int[] data)
		{
			if (data == null) throw new ArgumentNullException("data must not be null");

			size = data[0];
			colors = new ColorBGRA[data[1]];

			int pos = 2;
			for (int i = 0; i < colors.Length; i++)
			{
				colors[i] = ShiftInt(data[pos]);
				pos++;
			}

			rects = new Rectangle[data[pos]][];
			pos++;
			for (int i = 0; i < rects.Length; i++)
			{
				rects[i] = new Rectangle[data[pos]];
				pos++;
				for (int j = 0; j < rects[i].Length; j++)
				{
					rects[i][j] = new Rectangle(data[pos], data[pos + 1], data[pos + 2], data[pos + 3]);
					pos += 4;
				}
			}
		}

		public CHBuild genOffset()
		{
			CHBuild ret = new CHBuild();
			ret.rects = new SharpDX.Rectangle[rects.Length][];
			ret.colors = (SharpDX.ColorBGRA[])colors.Clone();
			ret.size = size;
			for (int i = 0; i < rects.Length; i++)
			{
				ret.rects[i] = new SharpDX.Rectangle[rects[i].Length];
				for (int j = 0; j < ret.rects[i].Length; j++)
				{
					SharpDX.Rectangle cpy = rects[i][j];
					ret.rects[i][j] = new SharpDX.Rectangle(
						D3DHook.connection.X / 2 - ret.size / 2 + cpy.X,
						D3DHook.connection.Y / 2 - ret.size / 2 + cpy.Y,
						cpy.Width,
						cpy.Height);
				}
			}
			return ret;
		}

		private static ColorBGRA ShiftInt(int i)
		{
			byte B = (byte)(i & 0xFF); i >>= 8;
			byte G = (byte)(i & 0xFF); i >>= 8;
			byte R = (byte)(i & 0xFF); i >>= 8;
			byte A = (byte)(i & 0xFF);
			return new ColorBGRA(R, G, B, A);
		}

		#region DefaultNone
		public static readonly CHBuild DefaultNone = new CHBuild();
		#endregion

		#region DefaultLines
		public static readonly CHBuild DefaultLines = new CHBuild()
		{
			rects = new[] {
				new[] { 
					new Rectangle(0, 14, 10, 2) , //  -
					new Rectangle(20, 14, 10, 2) , //   -
					new Rectangle(14, 0, 2, 10) , //   |
					new Rectangle(14, 20, 2, 10) , //  |
				} 
			},
			colors = new[] { ColorPalette.ColorYellow },
			size = 30
		};
		#endregion

		#region DefaultThinLines
		public static readonly CHBuild DefaultThinLines = new CHBuild()
		{
			rects = new[] {
				new[] { 
					new Rectangle(0, 14, 10, 1) , //  -
					new Rectangle(20, 14, 10, 1) , //   -
					new Rectangle(13, 11, 1, 2) , //   |
					new Rectangle(16, 11, 1, 2) , //   |
					new Rectangle(13, 16, 1, 2) , //  |
					new Rectangle(16, 16, 1, 2) , //  |
				} 
			},
			colors = new[] { ColorPalette.ColorYellow },
			size = 30
		};
		#endregion

		#region DefaultSquare
		public static readonly CHBuild DefaultSquare = new CHBuild()
		{
			rects = new[] {
				new[] { 
					new Rectangle(0, 0, 40, 1) ,
					new Rectangle(0, 0, 1, 40) ,
					new Rectangle(40, 0, 1, 40) ,
					new Rectangle(0, 40, 40, 1) ,
				} 
			},
			colors = new[] { ColorPalette.ColorCyan },
			size = 40
		};
		#endregion

		#region DefaultDot
		public static readonly CHBuild DefaultDot = new CHBuild()
		{
			rects = new[] { new[] { new Rectangle(0, 0, 2, 2) } },
			colors = new[] { ColorPalette.ColorWhite },
			size = 2
		};
		#endregion
	}

	public class CHBuilder
	{
		public static int[] FromExtern(System.Drawing.Rectangle[][] rec, System.Drawing.Color[] col, int size)
		{
			if (rec == null || col == null) return null;

			// Data Syntax:
			// 1 int element <size>
			// 1 int color array length +=> int:rgba
			// 1 int Rect[] array length <rLen>
			// foreach <rLen>: 1 int array length +=> 4int:rectangle
			int[] tmpret = new int[3 + rec.Sum<System.Drawing.Rectangle[]>(a => a.Length * 4 + 1) + col.Length];
			tmpret[0] = size;
			tmpret[1] = col.Length;

			int pos = 2;
			Array.ForEach(col, c => { tmpret[pos] = c.ToArgb(); pos++; });

			tmpret[pos] = rec.Length; pos++;
			Array.ForEach(rec, r =>
			{
				tmpret[pos] = r.Length; pos++;
				Array.ForEach(r, sr =>
				{
					tmpret[pos] = sr.X; pos++;
					tmpret[pos] = sr.Y; pos++;
					tmpret[pos] = sr.Width; pos++;
					tmpret[pos] = sr.Height; pos++;
				});
			});
			return tmpret;
		}
	}
}
