namespace System.Windows.Forms {
	/// <summary>
	/// Specifies on which thread the GDI layer rendering is to be performed.
	/// </summary>
	public enum GdiRenderMode {
		/// <summary>
		/// Renders on a dedicated GDI thread (this is the default)
		/// </summary>
		GdiAsync,
		/// <summary>
		/// Renders on the current calling thread
		/// </summary>
		CurrentSync,
		/// <summary>
		/// Renders on the thread on which the form is resident
		/// </summary>
		MainAsync,
		/// <summary>
		/// Not much use to this.
		/// </summary>
		MainSync,
		/// <summary>
		/// There is no reason to use this, seriously.
		/// </summary>
		GdiSync
	}
}