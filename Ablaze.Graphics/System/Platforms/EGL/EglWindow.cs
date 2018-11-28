namespace System.Platforms.EGL {
	/// <summary>
	/// Manages an Egl window.
	/// </summary>
	public class EglWindow : Window {
		/// <summary>
		/// The handle of the display that contains the window.
		/// </summary>
		public readonly IntPtr Display;

		/// <summary>
		/// A handle that identifies a surface.
		/// </summary>
		public IntPtr Surface {
			get;
			private set;
		}

		/// <summary>
		/// Initializes a new window wrapper.
		/// </summary>
		/// <param name="handle">The handle of the window.</param>
		/// <param name="display">The handle of the display that contains the window.</param>
		public EglWindow(IntPtr handle, IntPtr display)
			: base(handle) {
			Display = display;
		}

		/// <summary>
		/// Initializes a new window surface.
		/// </summary>
		/// <param name="config">The configuration to use.</param>
		public void CreateWindowSurface(IntPtr config) {
			Surface = System.Graphics.EGL.Egl.CreateWindowSurface(Display, config, Handle, null);
			if (Surface == IntPtr.Zero)
				throw new InvalidOperationException(String.Format("Failed to create EGL window surface, error {0}.", System.Graphics.EGL.Egl.GetError()));
		}

		/// <summary>
		/// Called when the window is disposed.
		/// </summary>
		protected override void OnDisposed() {
			if (Surface != IntPtr.Zero)
				System.Graphics.EGL.Egl.DestroySurface(Display, Surface);
		}
	}
}