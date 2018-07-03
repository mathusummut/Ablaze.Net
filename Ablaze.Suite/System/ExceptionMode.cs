namespace System {
	/// <summary>
	/// Represents the exception handling mode.
	/// </summary>
	public enum ExceptionMode {
		/// <summary>
		/// Logs any exception that may arise but does not throw.
		/// </summary>
		Log = 0,
		/// <summary>
		/// Throws the exception that was encountered.
		/// </summary>
		Throw,
		/// <summary>
		/// Silently ignores any exceptions.
		/// </summary>
		Ignore
	}
}
