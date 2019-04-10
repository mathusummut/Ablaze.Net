using System.Threading;

namespace System {
	/// <summary>
	/// Contains extension methods for the System.Console class
	/// </summary>
	public static class ConsoleExt {
		private static Thread inputThread;
		private static AutoResetEvent getInput, gotInput;
		private static string input;

		static ConsoleExt() {
			getInput = new AutoResetEvent(false);
			gotInput = new AutoResetEvent(false);
			inputThread = new Thread(StartReader);
			inputThread.IsBackground = true;
			inputThread.Start();
		}

		private static void StartReader() {
			while (true) {
				getInput.WaitOne();
				input = Console.ReadLine();
				gotInput.Set();
			}
		}

		/// <summary>
		/// Reads the next line of characters from the standard input stream, and returns the default string if timeout occurs
		/// </summary>
		/// <param name="millisecondsTimeout">The input timeout in milliseconds</param>
		/// <param name="timeoutDefault">The default string to return if ReadLine times out</param>
		public static string ReadLine(int millisecondsTimeout = Timeout.Infinite, string timeoutDefault = "") {
			getInput.Set();
			if (gotInput.WaitOne(millisecondsTimeout))
				return input;
			else
				return timeoutDefault;
		}

		/// <summary>
		/// Reads the next line of characters from the standard input stream, and throws TimeoutException if timeout occurs
		/// </summary>
		/// <param name="millisecondsTimeout">The input timeout in milliseconds</param>
		public static string ReadLineThrow(int millisecondsTimeout = Timeout.Infinite) {
			getInput.Set();
			if (gotInput.WaitOne(millisecondsTimeout))
				return input;
			else
				throw new TimeoutException("ReadLineThrow timeout exceeded");
		}
	}
}