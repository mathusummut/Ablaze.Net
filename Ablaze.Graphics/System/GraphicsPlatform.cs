using System.Platforms.Unix;
using System.Runtime.CompilerServices;

namespace System {
	/// <summary>
	/// The type of graphics context that is created.
	/// </summary>
	[Flags]
	public enum GraphicsContextType {
		/// <summary>
		/// The graphics context type is not supported.
		/// </summary>
		None,
		/// <summary>
		/// Whether the graphics context makes use of AGL on MacOS.
		/// </summary>
		Agl,
		/// <summary>
		/// Whether the graphics context makes use of EGL (Embedded GL).
		/// </summary>
		Embedded,
		/// <summary>
		/// Whether the graphics context makes use of GLX on X11.
		/// </summary>
		Glx,
		/// <summary>
		/// Whether the graphics context makes use of WGL on Windows.
		/// </summary>
		Wgl
	}

	/// <summary>
	/// Provides information about the graphics context types supported by the current platform.
	/// </summary>
	public static class GraphicsPlatform {
		/// <summary>
		/// Gets the type of graphics contexts that are available on this platform.
		/// </summary>
		public static readonly GraphicsContextType ContextType;
		/// <summary>
		/// The factory that handles native resources.
		/// </summary>
		private static Platforms.NativeFactory factory;

		/// <summary>
		/// Gets the factory used to handle platform-specific resources.
		/// </summary>
		public static Platforms.NativeFactory Factory {
			get {
				if (factory == null)
					throw new PlatformNotSupportedException("Could not initialize a graphics loader on the current platform.");
				else
					return factory;
			}
		}

		static GraphicsPlatform() {
			if (Platform.IsOnSpecifiedPlatform(PlatformType.Windows)) {
				factory = new Platforms.Windows.WinFactory();
				try {
					Graphics.WGL.Wgl.LoadOpenGL();
					Graphics.WGL.Wgl.Delegates.ReloadDelegates();
					ContextType = GraphicsContextType.Wgl;
				} catch {
					if (Graphics.EGL.Egl.IsSupported) {
						factory = new Platforms.EGL.EglWinFactory();
						ContextType = GraphicsContextType.Embedded;
					} else
						factory = null;
				}
			} else if (Platform.IsOnSpecifiedPlatform(PlatformType.MacOSX)) {
				factory = new Platforms.MacOS.MacOSFactory();
				try {
					factory.GetCurrentContext();
					ContextType = GraphicsContextType.Agl;
				} catch {
					if (Graphics.EGL.Egl.IsSupported) {
						factory = new Platforms.EGL.EglMacOSFactory();
						ContextType = GraphicsContextType.Embedded;
					} else
						factory = null;
				}
			} else if (Platform.IsOnSpecifiedPlatform(PlatformType.X11)) {
				factory = new Platforms.X11.X11Factory();
				try {
					factory.GetCurrentContext();
					ContextType = GraphicsContextType.Glx;
				} catch {
					if (Graphics.EGL.Egl.IsSupported) {
						factory = new Platforms.EGL.EglX11Factory();
						ContextType = GraphicsContextType.Embedded;
					} else
						factory = null;
				}
			}
		}

		/// <summary>
		/// Gets whether the specified context type is supported.
		/// </summary>
		/// <param name="type">The type of graphics context to test if supported.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool SupportsContextType(GraphicsContextType type) {
			return (ContextType & type) == type;
		}
	}
}