namespace System.Platforms {
	/// <summary>
	/// Manages a platform-specific window.
	/// </summary>
	public abstract class Window : IDisposable {
		/// <summary>
		/// The handle of the window.
		/// </summary>
		public IntPtr Handle {
			get;
			protected set;
		}

		/// <summary>
		/// Gets whether the Handle is zeroed.
		/// </summary>
		public bool IsDisposed {
			get {
				return Handle == IntPtr.Zero;
			}
		}

		/// <summary>
		/// Initializes a new window wrapper.
		/// </summary>
		/// <param name="handle">The handle of the window.</param>
		protected Window(IntPtr handle) {
			Handle = handle;
		}

		/// <summary>
		/// Checks whether the object has the same handle as this one.
		/// </summary>
		/// <param name="obj">The object to compare equality with.</param>
		public override bool Equals(object obj) {
			Window window = obj as Window;
			return window != null && window.Handle == Handle;
		}

		/// <summary>
		/// Gets a string that describes this instance.
		/// </summary>
		public override string ToString() {
			return string.Format("Handle: {0}", Handle);
		}

		/// <summary>
		/// Gets the hash code of this instance.
		/// </summary>
		public override int GetHashCode() {
			return Handle.GetHashCode();
		}

		/// <summary>
		/// Disposes of the window.
		/// </summary>
		~Window() {
			Dispose();
		}

		/// <summary>
		/// Called when the window is disposed.
		/// </summary>
		protected abstract void OnDisposed();

		/// <summary>
		/// Disposes of the window.
		/// </summary>
		public void Dispose() {
			if (Handle == IntPtr.Zero)
				return;
			OnDisposed();
			Handle = IntPtr.Zero;
			GC.SuppressFinalize(this);
		}
	}
}