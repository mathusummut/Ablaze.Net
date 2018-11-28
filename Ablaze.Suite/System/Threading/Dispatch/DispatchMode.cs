namespace System.Threading.Dispatch {
	/// <summary>
	/// Specifies the thread on which the dispatcher will reside.
	/// </summary>
	public enum DispatchMode {
		/// <summary>
		/// The dispatcher will reside on the current thread (causing the thread to block until the dispatcher is disposed).
		/// </summary>
		OnCurrentThread,
		/// <summary>
		/// The dispatcher will reside on a seperate thread.
		/// </summary>
		OnSeperateThread
	}
}