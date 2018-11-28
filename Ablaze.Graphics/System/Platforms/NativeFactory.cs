using System.Drawing;
using System.Graphics;

namespace System.Platforms {
	/// <summary>
	/// Handles the creation of native resources.
	/// </summary>
	public abstract class NativeFactory {
		/// <summary>
		/// Gets a list of the display devices that are available.
		/// </summary>
		public abstract Collections.ObjectModel.ReadOnlyCollection<Display> DisplayDevices {
			get;
		}

		/// <summary>
		/// Gets the primary display.
		/// </summary>
		public abstract Display Primary {
			get;
		}

		/// <summary>
		/// Initializes a new graphics context.
		/// </summary>
		/// <param name="mode">The graphics configuration to use.</param>
		/// <param name="window">The window to render onto.</param>
		/// <param name="version">The requested OpenGL version.</param>
		/// <param name="sharedContext">A context with which to share resources (textures, buffers...).</param>
		public abstract GraphicsContext CreateGLContext(GraphicsMode mode, Window window, MajorMinorVersion version, GraphicsContext sharedContext = null);

		/// <summary>
		/// Initializes a graphics context wrapper.
		/// </summary>
		/// <param name="handle">A handle that points to a native context.</param>
		public abstract GraphicsContext CreateGLContext(IntPtr handle);

		/// <summary>
		/// Gets the related info of the window that has the specified handle.
		/// </summary>
		/// <param name="handle">The handle of the window.</param>
		/// <param name="mode">The GraphicsMode of the window.</param>
		public abstract Window GetWindowInfo(IntPtr handle, GraphicsMode mode);

		/// <summary>
		/// Gets a handle that points to the current graphics context.
		/// </summary>
		public abstract IntPtr GetCurrentContext();

		/// <summary>
		/// Selects the graphics mode that is the most similar to the specified mode.
		/// </summary>
		/// <param name="mode">The mode to search similarity with.</param>
		/// <param name="window">The window to use for querying.</param>
		public abstract GraphicsMode SelectClosestGraphicsMode(GraphicsMode mode, IntPtr window);

		/// <summary>
		/// Gets a pointer that points to the address of the specified native function.
		/// </summary>
		/// <param name="function">The function to find.</param>
		public abstract IntPtr GetAddress(string function);

		/// <summary>
		/// Tries to change the resolution of the specified display to the given parameters.
		/// </summary>
		/// <param name="device">The display device whose resolution to change.</param>
		/// <param name="resolution">The target resolution of the display.</param>
		public abstract bool TryChangeResolution(Display device, Resolution resolution);

		/// <summary>
		/// Tries to restore the resolution of the specified display to the default resolution.
		/// </summary>
		/// <param name="device">The display device.</param>
		public abstract bool TryRestoreResolution(Display device);

		/// <summary>
		/// Gets the display device that is at the specified index. If the index is invalid, the primary display is returned.
		/// </summary>
		/// <param name="index">The index of the display.</param>
		public Display GetDisplay(int index) {
			if (index <= 0 || index >= DisplayDevices.Count)
				return Primary;
			else
				return DisplayDevices[index];
		}

		/// <summary>
		/// Gets the display device that contains the specified point.
		/// </summary>
		/// <param name="point">The coordinates of the point.</param>
		public Display GetDisplay(Point point) {
			foreach (Display display in GraphicsPlatform.Factory.DisplayDevices) {
				if (display.Bounds.Contains(point))
					return display;
			}
			return Primary;
		}
	}
}