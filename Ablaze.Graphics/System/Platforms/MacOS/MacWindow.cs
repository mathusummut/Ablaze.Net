namespace System.Platforms.MacOS {
	/// <summary>
	/// Manages an Mac window.
	/// </summary>
	public class MacOSWindow : Window {
		/// <summary>
		/// Initializes a new window wrapper.
		/// </summary>
		/// <param name="handle">The handle of the window.</param>
		public MacOSWindow(IntPtr handle)
			: base(handle) {
		}

		/// <summary>
		/// Called when the window is disposed.
		/// </summary>
		protected override void OnDisposed() {
			NativeApi.DisposeWindow(Handle);
			MacOSFactory.windows.Remove(Handle);
		}
	}
}