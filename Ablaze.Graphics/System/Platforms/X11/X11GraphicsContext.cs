using System.Graphics;
using System.Graphics.GLX;
using System.Runtime.InteropServices;

namespace System.Platforms.X11 {
	/// <summary>
	/// Provides methods to manipulate an X11 graphics context.
	/// </summary>
	public class X11GraphicsContext : GraphicsContext {
		private IntPtr display;
		private IntPtr WindowHandle;
		private int swap_interval = 1;

		/// <summary>
		/// Gets or sets a the number of display refreshes between consecutive <see cref="SwapBuffers()"/> calls.
		/// </summary>
		public override int SwapInterval {
			get {
				return swap_interval;
			}
			set {
				try {
					NativeApi.XLockDisplay(display);
					ErrorCode error_code = Glx.Sgi.SwapInterval(value);
					NativeApi.XUnlockDisplay(display);
					if (error_code == ErrorCode.NO_ERROR)
						swap_interval = value;
				} catch {
				}
			}
		}

		/// <summary>
		/// Initializes a new graphics context wrapper.
		/// </summary>
		/// <param name="handle">The handle of the context to wrap.</param>
		public X11GraphicsContext(IntPtr handle) : base(handle) {
		}

		/// <summary>
		/// Initializes a new X11 graphics context.
		/// </summary>
		/// <param name="mode">The graphics mode to use.</param>
		/// <param name="window">The window on which to initialize the context.</param>
		/// <param name="version">The requested OpenGL version.</param>
		/// <param name="sharedContext">A context with which to share resources (textures, buffers...).</param>
		public X11GraphicsContext(GraphicsMode mode, Window window, MajorMinorVersion version, GraphicsContext sharedContext = null) : base(sharedContext) {
			GraphicsMode = mode;
			X11Window currentWindow = (X11Window) window;
			display = currentWindow.Display;
			WindowHandle = window.Handle;
			currentWindow.VisualInfo = SelectVisual(mode, currentWindow, display);
			IntPtr shareHandle = sharedContext == null ? IntPtr.Zero : sharedContext.Handle;
			NativeApi.XLockDisplay(display);
			if (Glx.Delegates.glXCreateContextAttribsARB != null) {
				unsafe
				{
					int count;
					IntPtr* fbconfigs = Glx.ChooseFBConfig(display, currentWindow.Screen,
						new int[] {
						(int)PixelFormatAttribute.VISUAL_ID,
						(int)mode.Index,
						0
					}, out count);
					if (count > 0) {
						if (version.Major < 1)
							version.Major = 1;
						if (version.Minor < 0)
							version.Minor = 0;
						int[] attributes = new int[] { (int) ArbCreateContext.MajorVersion, version.Major, (int) ArbCreateContext.MinorVersion, version.Minor };
						Handle = Glx.Arb.CreateContextAttribs(display, *fbconfigs, shareHandle, true, attributes);
						if (Handle == IntPtr.Zero)
							Handle = Glx.Arb.CreateContextAttribs(display, *fbconfigs, shareHandle, false, attributes);
						NativeApi.XFree((IntPtr) fbconfigs);
					}
				}
			}
			if (Handle == IntPtr.Zero) {
				XVisualInfo info = currentWindow.VisualInfo;
				Handle = Glx.CreateContext(display, ref info, shareHandle, true);
				if (Handle == IntPtr.Zero)
					Handle = Glx.CreateContext(display, ref info, IntPtr.Zero, false);
			}
			NativeApi.XUnlockDisplay(display);
			if (Handle == IntPtr.Zero)
				throw new InvalidOperationException("Failed to create GL context.");
		}

		private static XVisualInfo SelectVisual(GraphicsMode mode, X11Window currentWindow, IntPtr display) {
			XVisualInfo info = new XVisualInfo() {
				VisualID = (IntPtr) mode.Index,
				Screen = currentWindow.Screen
			};
			lock (NativeApi.SyncRoot) {
                int items;
                IntPtr vs = NativeApi.XGetVisualInfo(display, XVisualInfoMask.ID | XVisualInfoMask.Screen, ref info, out items);
				if (items == 0)
					throw new ArgumentException(string.Format("Invalid GraphicsMode specified ({0}).", mode));
				info = (XVisualInfo) Marshal.PtrToStructure(vs, typeof(XVisualInfo));
				NativeApi.XFree(vs);
			}
			return info;
		}

		/// <summary>
		/// Swaps buffers on a context. This presents the rendered scene to the user.
		/// </summary>
		public override void SwapBuffers() {
			NativeApi.XLockDisplay(display);
			Glx.SwapBuffers(display, WindowHandle);
			NativeApi.XUnlockDisplay(display);
			base.SwapBuffers();
		}

		/// <summary>
		/// Makes the GraphicsContext the rendering target on the current thread (can be null).
		/// A context can only be current on a single thread. To transfer a context to another thread,
		/// MakeCurrent(null) must first be called on the thread that currently owns it.
		/// </summary>
		/// <param name="window">The window in which the context is initialized.</param>
		public override void MakeCurrent(Window window) {
			base.MakeCurrent(window);
			if (window.Handle == WindowHandle && IsCurrent)
				return;
			X11Window w = window as X11Window;
			if (w != null && w.Display != display)
				throw new InvalidOperationException("MakeCurrent() may only be called on windows originating from the same display on which the graphics context was created.");
			bool result;
			if (window == null) {
				NativeApi.XLockDisplay(display);
				result = Glx.MakeCurrent(display, IntPtr.Zero, IntPtr.Zero);
				NativeApi.XUnlockDisplay(display);
			} else {
				NativeApi.XLockDisplay(display);
				result = Glx.MakeCurrent(display, w.Handle, Handle);
				NativeApi.XUnlockDisplay(display);
			}
			if (!result)
				throw new InvalidOperationException("Failed to make context current.");
		}

		/// <summary>
		/// Disposes of the resources used by the GraphicsContext.
		/// </summary>
		protected override void OnDisposed() {
			NativeApi.XLockDisplay(display);
			Glx.DestroyContext(display, Handle);
			NativeApi.XUnlockDisplay(display);
		}
	}
}