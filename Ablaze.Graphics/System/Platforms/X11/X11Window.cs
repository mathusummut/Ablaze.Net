namespace System.Platforms.X11 {
	/// <summary>
	/// Manages an X11 window.
	/// </summary>
	public class X11Window : Window {
		/// <summary>
		/// The handle of the display that contains the window.
		/// </summary>
		public readonly IntPtr Display;
		/// <summary>
		/// The screen index.
		/// </summary>
		public readonly int Screen;
		/// <summary>
		/// Additional window info.
		/// </summary>
		public XVisualInfo VisualInfo;

		/// <summary>
		/// Initializes a new window wrapper.
		/// </summary>
		/// <param name="handle">The handle of the window.</param>
		/// <param name="display">The handle of the display that contains the window.</param>
		/// <param name="screen">The screen index.</param>
		/// <param name="visualInfo">Additional window info.</param>
		public X11Window(IntPtr handle, IntPtr display, int screen, XVisualInfo visualInfo)
			: base(handle) {
			Display = display;
			Screen = screen;
			VisualInfo = visualInfo;
		}

		/// <summary>
		/// Called when the window is disposed.
		/// </summary>
		protected override void OnDisposed() {
		}

		/// <summary>
		/// Gets a string that describes this instance.
		/// </summary>
		public override string ToString() {
			return String.Format("X11.WindowInfo: Display {0}, Screen {1}, Handle {2}", Display, Screen, Handle);
		}

		/// <summary>
		/// Checks whether the object has the same handle as this one.
		/// </summary>
		/// <param name="obj">The object to compare equality with.</param>
		public override bool Equals(object obj) {
			X11Window window = obj as X11Window;
			return window != null && window.Display == Display && window.Handle == Handle;
		}

		/// <summary>
		/// Gets the hash code of this instance.
		/// </summary>
		public override int GetHashCode() {
			return unchecked((Handle.GetHashCode() << 16) ^ Display.GetHashCode());
		}
	}
}