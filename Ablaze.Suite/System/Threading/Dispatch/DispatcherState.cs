namespace System.Threading.Dispatch {
	/// <summary>
	/// Provides information about the dispatcher's state.
	/// </summary>
	public enum DispatcherState {
		/// <summary>
		/// The dispatcher is starting.
		/// </summary>
		Starting,
		/// <summary>
		/// The dispatcher is running and it is waiting for an invocation.
		/// </summary>
		Idle,
		/// <summary>
		/// The dispatcher is running and it is busy invoking methods.
		/// </summary>
		Busy,
		/// <summary>
		/// The dispatcher is being disposed.
		/// </summary>
		Disposing,
		/// <summary>
		/// The dispatcher is disposed.
		/// </summary>
		Disposed,
	}
}