using System.IO;

namespace System.Diagnostics {
	/// <summary>
	/// Represents an error logger. To use your own, implement IErrorLogger and set System.Diagnostics.ErrorHandler.Logger to a new instance of your logger.
	/// </summary>
	public interface IErrorLogger {
		/// <summary>
		/// Handles an exception that was thrown.
		/// </summary>
		/// <param name="ex">The first exception that was thrown.</param>
		/// <param name="errorLog">A full description of the error that occurred (pre-formatted for logging).</param>
		void LogException(Exception ex, string errorLog);
	}

	/// <summary>
	/// The default error logger. To use your own, inherit this class or implement IErrorLogger and override LogException(),
	/// and set System.Diagnostics.ErrorHandler.Logger to a new instance of your logger.
	/// </summary>
	public class ErrorLogger : IErrorLogger {
		private int maxLogSize = 1048576;

		/// <summary>
		/// Gets or sets the maximum log file size in kibibytes.
		/// </summary>
		public int MaxLogSizeKiB {
			get {
				return maxLogSize / 1024;
			}
			set {
				if (value < 1)
					value = 1;
				maxLogSize = value * 1024;
			}
		}

		/// <summary>
		/// Handles an exception that was thrown.
		/// </summary>
		/// <param name="ex">The first exception that was thrown.</param>
		/// <param name="errorLog">A full description of the error that occurred (pre-formatted for logging).</param>
		public virtual void LogException(Exception ex, string errorLog) {
			if (errorLog == null)
				return;
			errorLog = errorLog.Replace("\r", string.Empty);
			if (errorLog.Length == 0)
				return;
			errorLog = errorLog[0] == '\n' ? errorLog : "\n" + errorLog;
			Console.WriteLine(errorLog.RemoveConsecutiveDuplicates('\n'));
			try {
				string filename;
				try {
					filename = FileUtils.GetFileNameWithoutExtension(Reflection.Assembly.GetEntryAssembly().Location) + "_errors.log";
				} catch {
					filename = "Exceptions.log";
				}
				File.AppendAllText(filename, errorLog);
				if (new FileInfo(filename).Length > maxLogSize) {
					int halfSize = maxLogSize / 2;
					string allText = File.ReadAllText(filename);
					File.WriteAllText(filename, allText.Substring(halfSize));
				}
			} catch {
			}
		}
	}
}