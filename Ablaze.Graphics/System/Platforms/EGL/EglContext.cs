using System.Graphics;

namespace System.Platforms.EGL {
	/// <summary>
	/// Provides methods to manipulate an EGL graphics context.
	/// </summary>
	public class EglContext : GraphicsContext {
		private EglWindow WindowInfo;
		private int swap_interval = 1;

		/// <summary>
		/// Gets or sets a the number of display refreshes between consecutive <see cref="SwapBuffers()"/> calls.
		/// </summary>
		public override int SwapInterval {
			get {
				return swap_interval;
			}
			set {
				if (WindowInfo != null && Graphics.EGL.Egl.SwapInterval(WindowInfo.Display, value))
					swap_interval = value;
			}
		}

		/// <summary>
		/// Initializes a new graphics context wrapper.
		/// </summary>
		/// <param name="handle">The handle of the context to wrap.</param>
		public EglContext(IntPtr handle) : base(handle) {
		}

		/// <summary>
		/// Initializes a new Windows graphics context.
		/// </summary>
		/// <param name="mode">The graphics mode to use.</param>
		/// <param name="window">The window on which to initialize the context.</param>
		/// <param name="version">The requested OpenGL version.</param>
		/// <param name="sharedContext">A context with which to share resources (textures, buffers...).</param>
		public EglContext(GraphicsMode mode, EglWindow window, MajorMinorVersion version, GraphicsContext sharedContext = null) : base(sharedContext) {
			EglContext shared = sharedContext as EglContext;
			int dummy_major, dummy_minor;
			if (!Graphics.EGL.Egl.Initialize(window.Display, out dummy_major, out dummy_minor))
				throw new InvalidOperationException(string.Format("Failed to initialize EGL, error {0}.", Graphics.EGL.Egl.GetError()));
			WindowInfo = window;
            GraphicsMode modeCopy = mode;
            Graphics.EGL.RenderableFlags flags = Graphics.EGL.RenderableFlags.OpenGLES2Bit;
			if (version.Major < 1)
				version.Major = 1;
			do {
                try {
                    GraphicsMode = SelectGraphicsMode(ref mode, flags);
                    if (GraphicsMode.Index.HasValue) {
                        IntPtr config = GraphicsMode.Index.Value;
                        if (window.Surface == IntPtr.Zero)
                            window.CreateWindowSurface(config);
                        int[] attrib_list = new int[] { 12440 /*EGL_CONTEXT_CLIENT_VERSION*/, version.Major, (int) Graphics.EGL.All.None };
                        Handle = Graphics.EGL.Egl.CreateContext(window.Display, config, shared == null ? IntPtr.Zero : shared.Handle, attrib_list);
                    }
                } catch {
                }
                if (Handle == IntPtr.Zero) {
                    if (flags == Graphics.EGL.RenderableFlags.OpenGLESBit)
                        break;
                    mode = modeCopy;
                    flags = Graphics.EGL.RenderableFlags.OpenGLESBit;
                } else
                    break;
            } while (true);
            if (Handle == IntPtr.Zero)
                throw new InvalidOperationException("Failed to create graphics context.");
		}

		/// <summary>
		/// Swaps buffers on a context. This presents the rendered scene to the user.
		/// </summary>
		public override void SwapBuffers() {
			if (WindowInfo != null)
				Graphics.EGL.Egl.SwapBuffers(WindowInfo.Display, WindowInfo.Surface);
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
			if (!(WindowInfo == null || Graphics.EGL.Egl.MakeCurrent(WindowInfo.Display, WindowInfo.Surface, WindowInfo.Surface, Handle)))
				throw new InvalidOperationException("Failed to make context current.");
		}

		internal static GraphicsMode SelectGraphicsMode(ref GraphicsMode mode, Graphics.EGL.RenderableFlags flags) {
			IntPtr[] configs = new IntPtr[1];
			int[] attribList = new int[]
			{
				(int) System.Graphics.EGL.All.RenderableType, (int)flags,

				(int) System.Graphics.EGL.All.RedSize, mode.ColorFormat.Red,
				(int) System.Graphics.EGL.All.GreenSize,  mode.ColorFormat.Green,
				(int) System.Graphics.EGL.All.BlueSize,  mode.ColorFormat.Blue,
				(int) System.Graphics.EGL.All.AlphaSize,  mode.ColorFormat.Alpha,

				(int) System.Graphics.EGL.All.DepthSize, mode.Depth > 0 ? mode.Depth : 0,
				(int) System.Graphics.EGL.All.StencilSize, mode.Stencil > 0 ? mode.Stencil : 0,
				(int) System.Graphics.EGL.All.Samples, mode.Samples > 0 ? mode.Samples : 0,

				(int) System.Graphics.EGL.All.None,
			};
			IntPtr display = System.Graphics.EGL.Egl.GetDisplay(IntPtr.Zero);
			int major, minor;
			if (!System.Graphics.EGL.Egl.Initialize(display, out major, out minor))
				throw new ArgumentException(String.Format("Failed to initialize display connection, error {0}", System.Graphics.EGL.Egl.GetError()));

			int num_configs;
			if (!System.Graphics.EGL.Egl.ChooseConfig(display, attribList, configs, configs.Length, out num_configs) || num_configs == 0)
				throw new ArgumentException(String.Format("Failed to retrieve GraphicsMode, error {0}", System.Graphics.EGL.Egl.GetError()));
			IntPtr active_config = configs[0];
			int r, g, b, a;
			System.Graphics.EGL.Egl.GetConfigAttrib(display, active_config, (int) System.Graphics.EGL.All.RedSize, out r);
			System.Graphics.EGL.Egl.GetConfigAttrib(display, active_config, (int) System.Graphics.EGL.All.GreenSize, out g);
			System.Graphics.EGL.Egl.GetConfigAttrib(display, active_config, (int) System.Graphics.EGL.All.BlueSize, out b);
			System.Graphics.EGL.Egl.GetConfigAttrib(display, active_config, (int) System.Graphics.EGL.All.AlphaSize, out a);
			int d, s;
			System.Graphics.EGL.Egl.GetConfigAttrib(display, active_config, (int) System.Graphics.EGL.All.DepthSize, out d);
			System.Graphics.EGL.Egl.GetConfigAttrib(display, active_config, (int) System.Graphics.EGL.All.StencilSize, out s);
			int sample_buffers, samples;
			System.Graphics.EGL.Egl.GetConfigAttrib(display, active_config, (int) System.Graphics.EGL.All.Samples, out sample_buffers);
			System.Graphics.EGL.Egl.GetConfigAttrib(display, active_config, (int) System.Graphics.EGL.All.Samples, out samples);

			return new GraphicsMode(active_config, new ColorFormat(r, g, b, a), d, s, sample_buffers > 0 ? samples : 0, ColorFormat.Empty, true, false);
		}

		/// <summary>
		/// Disposes of the resources used by the GraphicsContext.
		/// </summary>
		protected override void OnDisposed() {
			if (WindowInfo == null)
				return;
			if (IsCurrent)
				Graphics.EGL.Egl.MakeCurrent(WindowInfo.Display, WindowInfo.Surface, WindowInfo.Surface, IntPtr.Zero);
			Graphics.EGL.Egl.DestroyContext(WindowInfo.Display, Handle);
		}
	}
}