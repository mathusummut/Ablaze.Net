namespace System.Threading.Dispatch {
	/// <summary>
	/// The state of the invocation.
	/// </summary>
	public enum InvokeState {
		/// <summary>
		/// The invocation has not been used.
		/// </summary>
		Idle,
		/// <summary>
		/// The invocation has been queued.
		/// </summary>
		Queued,
		/// <summary>
		/// The invocation has started.
		/// </summary>
		Started,
		/// <summary>
		/// The invocation has completed.
		/// </summary>
		Completed,
		/// <summary>
		/// The invocation has thrown an error.
		/// </summary>
		Error
	}
}