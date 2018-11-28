using System;
using System.IO;
using System.Windows.Forms;

namespace Notepad {
	/// <summary>
	/// A notepad application for C#
	/// </summary>
	public static class NotepadWindow {
		/// <summary>
		/// The entry point of the application
		/// </summary>
		/// <param name="args">The console arguments passed to the application</param>
		[STAThread]
		public static void Main(string[] args) {
#if NET45
			try {
				System.Runtime.ProfileOptimization.SetProfileRoot(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location));
				System.Runtime.ProfileOptimization.StartProfile("LoadCache.prf");
			} catch {
			}
#endif
			string file = null;
			if (args.Length != 0)
				file = args[0].Trim();
			MessageLoop.Run(new System.Windows.Forms.Notepad(file));
		}
	}
}