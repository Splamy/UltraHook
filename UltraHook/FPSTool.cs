using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltraHook
{
	internal class FPSTool
	{
		int Count = 0;
		int LastFPS = 0;
		System.Diagnostics.Stopwatch sw;
		System.Diagnostics.Stopwatch limitsw;

		public FPSTool()
		{
			sw = new System.Diagnostics.Stopwatch();
			limitsw = new System.Diagnostics.Stopwatch();
		}

		public int getFPS()
		{
			if (!sw.IsRunning)
				sw.Start();
			else if (sw.ElapsedMilliseconds < 1000)
				Count++;
			else
			{
				LastFPS = Count;
				Count = 0;
				sw.Restart();
			}
			return LastFPS;
		}

		public void limitFPS()
		{
			//System.Threading.Thread.SpinWait() // do some thets with that
			//http://www.codeproject.com/Articles/98346/Microsecond-and-Millisecond-NET-Timer
			if (D3DHook.connection.limitFPS < 0) return;
			if (!limitsw.IsRunning) { limitsw.Start(); return; }
			while (limitsw.ElapsedMilliseconds < D3DHook.connection.limitFPS)
				System.Threading.Thread.Sleep(1);
			limitsw.Restart();
		}
	}
}
