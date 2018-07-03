using System.Collections.Generic;
using System.Graphics.AGL;

namespace System.Platforms.MacOS {
	using Graphics;

	/// <summary>
	/// Provides methods to manipulate an Mac graphics context.
	/// </summary>
	public class MacOSGraphicsContext : GraphicsContext {
		/// <summary>
		/// Gets or sets a the number of display refreshes between consecutive <see cref="SwapBuffers()"/> calls.
		/// </summary>
		public override int SwapInterval {
			get {
				int swap_interval = 0;
				if (Agl.GetInteger(Handle, ParameterNames.SWAP_INTERVAL, out swap_interval))
					return swap_interval;
				else
					return 0;
			}
			set {
				Agl.SetInteger(Handle, ParameterNames.SWAP_INTERVAL, ref value);
			}
		}

		/// <summary>
		/// Initializes a new graphics context wrapper.
		/// </summary>
		/// <param name="handle">The handle of the context to wrap.</param>
		public MacOSGraphicsContext(IntPtr handle) : base(handle) {
		}

		/// <summary>
		/// Initializes a new Mac graphics context.
		/// </summary>
		/// <param name="mode">The graphics mode to use.</param>
		/// <param name="window">The window on which to initialize the context.</param>
		/// <param name="version">The requested OpenGL version.</param>
		/// <param name="sharedContext">A context with which to share resources (textures, buffers...).</param>
		public MacOSGraphicsContext(GraphicsMode mode, Window window, MajorMinorVersion version, GraphicsContext sharedContext = null) {
            GraphicsMode = mode;
            IntPtr shareContextRef = sharedContext == null ? IntPtr.Zero : sharedContext.Handle;
            bool fullscreen = true;
            CreateContext:
            List<int> aglAttributes = new List<int>();
            aglAttributes.Add((int) PixelFormatAttribute.RGBA);
            aglAttributes.Add((int) PixelFormatAttribute.DOUBLEBUFFER);
            aglAttributes.Add((int) PixelFormatAttribute.RED_SIZE);
            aglAttributes.Add(GraphicsMode.ColorFormat.Red);
            aglAttributes.Add((int) PixelFormatAttribute.GREEN_SIZE);
            aglAttributes.Add(GraphicsMode.ColorFormat.Green);
            aglAttributes.Add((int) PixelFormatAttribute.BLUE_SIZE);
            aglAttributes.Add(GraphicsMode.ColorFormat.Blue);
            aglAttributes.Add((int) PixelFormatAttribute.ALPHA_SIZE);
            aglAttributes.Add(GraphicsMode.ColorFormat.Alpha);
            if (GraphicsMode.Depth > 0) {
                aglAttributes.Add((int) PixelFormatAttribute.DEPTH_SIZE);
                aglAttributes.Add(GraphicsMode.Depth);
            }
            if (GraphicsMode.Stencil > 0) {
                aglAttributes.Add((int) PixelFormatAttribute.STENCIL_SIZE);
                aglAttributes.Add(GraphicsMode.Stencil);
            }
            if (GraphicsMode.AccumFormat.BitsPerPixel > 0) {
                aglAttributes.Add((int) PixelFormatAttribute.ACCUM_RED_SIZE);
                aglAttributes.Add(GraphicsMode.AccumFormat.Red);
                aglAttributes.Add((int) PixelFormatAttribute.ACCUM_GREEN_SIZE);
                aglAttributes.Add(GraphicsMode.AccumFormat.Green);
                aglAttributes.Add((int) PixelFormatAttribute.ACCUM_BLUE_SIZE);
                aglAttributes.Add(GraphicsMode.AccumFormat.Blue);
                aglAttributes.Add((int) PixelFormatAttribute.ACCUM_ALPHA_SIZE);
                aglAttributes.Add(GraphicsMode.AccumFormat.Alpha);
            }
            if (GraphicsMode.Samples > 1) {
                aglAttributes.Add((int) PixelFormatAttribute.SAMPLE_BUFFERS_ARB);
                aglAttributes.Add(1);
                aglAttributes.Add((int) PixelFormatAttribute.SAMPLES_ARB);
                aglAttributes.Add(GraphicsMode.Samples);
            }
            if (fullscreen)
                aglAttributes.Add((int) PixelFormatAttribute.FULLSCREEN);
            aglAttributes.Add(0);
            IntPtr myAGLPixelFormat;
            if (fullscreen) {
                IntPtr gdevice;
                IntPtr cgdevice = (IntPtr) Display.Default.Id;
                OSStatus status = NativeApi.DMGetGDeviceByDisplayID(cgdevice, out gdevice, false);
                if (status != OSStatus.NoError)
                    throw new InvalidOperationException("DMGetGDeviceByDisplayID failed.");
                myAGLPixelFormat = Agl.ChoosePixelFormat(ref gdevice, 1, aglAttributes.ToArray());
                if (Agl.GetError() == AglError.BadPixelFormat) {
                    fullscreen = false;
                    goto CreateContext;
                }
            } else {
                IntPtr temp = IntPtr.Zero;
                myAGLPixelFormat = Agl.ChoosePixelFormat(ref temp, 0, aglAttributes.ToArray());
            }
            Handle = Agl.CreateContext(myAGLPixelFormat, shareContextRef);
            if (Handle == IntPtr.Zero)
                throw new InvalidOperationException("Failed to create graphics context.");
            Agl.DestroyPixelFormat(myAGLPixelFormat);
            IntPtr windowPort = NativeApi.GetWindowPort(NativeApi.GetControlOwner(window.Handle));
            Agl.SetDrawable(Handle, windowPort);
        }

		/// <summary>
		/// Swaps buffers on a context. This presents the rendered scene to the user.
		/// </summary>
		public override void SwapBuffers() {
			Agl.SwapBuffers(Handle);
		}

		/// <summary>
		/// Makes the GraphicsContext the rendering target on the current thread (can be null).
		/// A context can only be current on a single thread. To transfer a context to another thread,
		/// MakeCurrent(null) must first be called on the thread that currently owns it.
		/// </summary>
		/// <param name="window">The window in which the context is initialized.</param>
		public override void MakeCurrent(Window window) {
			base.MakeCurrent(window);
			if (!Agl.SetCurrentContext(Handle))
				throw new InvalidOperationException("Failed to make context current.");
		}

		/// <summary>
		/// Disposes of the resources used by the GraphicsContext.
		/// </summary>
		protected override void OnDisposed() {
			Agl.DestroyContext(Handle);
		}
	}
}