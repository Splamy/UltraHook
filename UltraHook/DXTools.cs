using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;

namespace UltraHook
{
	class DXTools
	{
		[DllImport("kernel32.dll")]
		public static extern IntPtr GetModuleHandle(string lpModuleName);

		public static IntPtr[] GetVTblAddresses(IntPtr pointer, int numberOfMethods)
		{
			return GetVTblAddresses(pointer, 0, numberOfMethods);
		}

		public static IntPtr[] GetVTblAddresses(IntPtr pointer, int startIndex, int numberOfMethods)
		{
			List<IntPtr> vtblAddresses = new List<IntPtr>();

			IntPtr vTable = Marshal.ReadIntPtr(pointer);
			for (int i = startIndex; i < startIndex + numberOfMethods; i++)
				vtblAddresses.Add(Marshal.ReadIntPtr(vTable, i * IntPtr.Size)); // using IntPtr.Size allows us to support both 32 and 64-bit processes

			return vtblAddresses.ToArray();
		}
	}
}
