namespace System.Platforms.Windows {
	/// <summary>
	/// Manages an Windows window.
	/// </summary>
	public class WinWindow : Window {
		private IntPtr dc;

		/// <summary>
		/// Gets the device context of the window.
		/// </summary>
		public IntPtr DeviceContext {
			get {
				if (dc == IntPtr.Zero)
					dc = NativeApi.GetDC(Handle);
				return dc;
			}
		}

		/// <summary>
		/// Initializes a new window wrapper.
		/// </summary>
		/// <param name="handle">The handle of the window.</param>
		public WinWindow(IntPtr handle)
			: base(handle) {
		}

		/// <summary>
		/// Called when the window is disposed.
		/// </summary>
		protected override void OnDisposed() {
			if (dc != IntPtr.Zero) {
				try {
					NativeApi.ReleaseDC(Handle, dc);
				} catch {
				}
			}
			try {
				NativeApi.DestroyWindow(Handle);
			} catch {
			}
			WinFactory.windows.Remove(Handle);
		}
	}
}