using System.Graphics;
using System.Graphics.WGL;
using System.Runtime.InteropServices;

namespace System.Platforms.Windows {
	/// <summary>
	/// Provides methods to manipulate an Windows graphics context.
	/// </summary>
	public class WinGraphicsContext : GraphicsContext {
		private static object SyncRoot = new object();

		/// <summary>
		/// Gets or sets a the number of display refreshes between consecutive <see cref="SwapBuffers()"/> calls.
		/// </summary>
		public override int SwapInterval {
			get {
				lock (SyncRoot) {
					try {
						return Wgl.Ext.GetSwapInterval();
					} catch {
						return 0;
					}
				}
			}
			set {
				lock (SyncRoot) {
					try {
						Wgl.Ext.SwapInterval(value);
					} catch {
					}
				}
			}
		}

		/// <summary>
		/// Initializes a new graphics context wrapper.
		/// </summary>
		/// <param name="handle">The handle of the context to wrap.</param>
		public WinGraphicsContext(IntPtr handle) : base(handle) {
		}

		/// <summary>
		/// Initializes a new Windows graphics context.
		/// </summary>
		/// <param name="mode">The graphics mode to use.</param>
		/// <param name="window">The window on which to initialize the context.</param>
		/// <param name="version">The requested OpenGL version.</param>
		/// <param name="sharedContext">A context with which to share resources (textures, buffers...).</param>
		public WinGraphicsContext(GraphicsMode mode, WinWindow window, MajorMinorVersion version, GraphicsContext sharedContext = null) : base(sharedContext) {
			lock (SyncRoot) {
				if (!mode.Index.HasValue)
					mode = GraphicsPlatform.Factory.SelectClosestGraphicsMode(mode, window.Handle);
				GraphicsMode = mode;
				PixelFormatDescriptor pfd = new PixelFormatDescriptor();
				int maxIndex = NativeApi.DescribePixelFormat(window.DeviceContext, (int) mode.Index.Value, (uint) PixelFormatDescriptor.StructSize, ref pfd);
				if (maxIndex == 0) {
					int errorCode = Marshal.GetLastWin32Error();
					throw new InvalidOperationException(string.Format("Requested GraphicsMode not available. DescribePixelFormat error {0}: {1}", errorCode, errorCode.Win32ErrorToString()));
				} else if ((int) mode.Index.Value > maxIndex)
					NativeApi.DescribePixelFormat(window.DeviceContext, maxIndex, (uint) PixelFormatDescriptor.StructSize, ref pfd);
				if (!NativeApi.SetPixelFormat(window.DeviceContext, (int) mode.Index.Value, ref pfd)) {
					int errorCode = Marshal.GetLastWin32Error();
					throw new InvalidOperationException(string.Format("Requested GraphicsMode not available. SetPixelFormat error {0}: {1}", errorCode, errorCode.Win32ErrorToString()));
				}
				if (Wgl.Delegates.wglCreateContextAttribsARB != null) {
					try {
						if (version.Major < 1)
							version.Major = 1;
						if (version.Minor < 0)
							version.Minor = 0;
						unsafe
						{
							int* attribs = stackalloc int[6];
							attribs[0] = (int) ArbCreateContext.MajorVersion;
							attribs[1] = version.Major;
							attribs[2] = (int) ArbCreateContext.MinorVersion;
							attribs[3] = version.Minor;
							attribs[4] = 0;
							attribs[5] = 0;
							Handle = Wgl.Arb.CreateContextAttribs(window.DeviceContext, sharedContext == null ? IntPtr.Zero : sharedContext.Handle, attribs);
						}
					} catch {
					}
				}
				if (Handle == IntPtr.Zero) {
					Handle = Wgl.CreateContext(window.DeviceContext);
					if (Handle == IntPtr.Zero) {
						int errorCode = Marshal.GetLastWin32Error();
						throw new InvalidOperationException(string.Format("Failed to create graphics context. Error {0}: {1}", errorCode, errorCode.Win32ErrorToString()));
					}
				}
				if (sharedContext != null)
					Wgl.ShareLists(sharedContext.Handle, Handle);
			}
		}

		/// <summary>
		/// Swaps buffers on a context. This presents the rendered scene to the user.
		/// </summary>
		public override void SwapBuffers() {
			NativeApi.SwapBuffers(Wgl.GetCurrentDC());
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
			if (Handle == IntPtr.Zero)
				return;
			lock (SyncRoot) {
				if (!(window == null ? Wgl.MakeCurrent(IntPtr.Zero, IntPtr.Zero) : Wgl.MakeCurrent(((WinWindow) window).DeviceContext, Handle)))
					throw new InvalidOperationException("Failed to make context current.");
			}
		}

		/// <summary>
		/// Disposes of the resources used by the GraphicsContext.
		/// </summary>
		protected override void OnDisposed() {
			Wgl.DeleteContext(Handle);
		}
	}
}