using System.Graphics;
using System.Platforms.MacOS;

namespace System.Platforms.EGL {
	/// <summary>
	/// Handles the creation of native Mac EGL resources.
	/// </summary>
	public class EglMacOSFactory : MacOSFactory {
		/// <summary>
		/// Initializes a new graphics context.
		/// </summary>
		/// <param name="mode">The graphics configuration to use.</param>
		/// <param name="window">The window to render onto.</param>
		/// <param name="version">The requested OpenGL version.</param>
		/// <param name="sharedContext">A context with which to share resources (textures, buffers...).</param>
		public override GraphicsContext CreateGLContext(GraphicsMode mode, Window window, MajorMinorVersion version, GraphicsContext sharedContext = null) {
			return new EglContext(mode, new EglWindow(window.Handle, System.Graphics.EGL.Egl.GetDisplay(IntPtr.Zero)), version, sharedContext);
		}

		/// <summary>
		/// Initializes a graphics context wrapper.
		/// </summary>
		/// <param name="handle">A handle that points to a native context.</param>
		public override GraphicsContext CreateGLContext(IntPtr handle) {
			return new EglContext(handle);
		}

		/// <summary>
		/// Gets a handle that points to the current graphics context.
		/// </summary>
		public override IntPtr GetCurrentContext() {
			return System.Graphics.EGL.Egl.GetCurrentContext();
		}

		/// <summary>
		/// Gets a pointer that points to the address of the specified native function.
		/// </summary>
		/// <param name="function">The function to find.</param>
		public override IntPtr GetAddress(string function) {
			return System.Graphics.EGL.Egl.GetProcAddress(function);
		}

		/// <summary>
		/// Selects the graphics mode that is the most similar to the specified mode.
		/// </summary>
		/// <param name="mode">The mode to search similarity with.</param>
		/// <param name="window">The window to use for querying.</param>
		public override GraphicsMode SelectClosestGraphicsMode(GraphicsMode mode, IntPtr window) {
			return EglContext.SelectGraphicsMode(ref mode, Graphics.EGL.RenderableFlags.OpenGLESBit);
		}
	}
}