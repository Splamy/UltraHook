using System;
using System.Collections.Generic;
using System.Text;

namespace UltraHook
{
	public abstract class ID3DControl
	{
		public abstract string Name { get; protected set; }

		public static Connection connection { get; set; }

		public static bool closed
		{
			get
			{
				if (closed_intern) return true;
				if (connection != null)
				{
					if (connection.closing)
					{
						closed_intern = true;
						connection.closing = false;
						return true;
					}
					return false;
				}
				return false;
			}
			protected set { }
		}

		private static bool closed_intern = false;

		public abstract void Hook();
	}
}
