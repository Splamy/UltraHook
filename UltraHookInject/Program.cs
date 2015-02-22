using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;

namespace UltraHookInject
{
	static class Program
	{
		/// <summary>
		/// Der Haupteinstiegspunkt für die Anwendung.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
			Application.Run(new Form1());
		}

		static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			string dllname = "UltraHookInject." + args.Name.Substring(0, args.Name.IndexOf(',')).Trim() + ".dll";
			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(dllname))
			{
				byte[] assData = new byte[stream.Length];
				stream.Read(assData, 0, assData.Length);
				return Assembly.Load(assData);
			}
		}
	}
}
