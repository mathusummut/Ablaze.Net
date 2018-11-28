using System.Collections.Generic;
using System.Drawing;
using System.Graphics.AGL;

namespace System.Platforms.MacOS {
	using Graphics;

	/// <summary>
	/// Handles the creation of native Mac resources.
	/// </summary>
	public class MacOSFactory : NativeFactory {
		internal static Dictionary<IntPtr, IntPtr> storedModes = new Dictionary<IntPtr, IntPtr>();
		internal static List<IntPtr> displaysCaptured = new List<IntPtr>();
		internal static List<Display> availableDevices = new List<Display>();
		internal static Dictionary<IntPtr, Window> windows = new Dictionary<IntPtr, Window>();
		internal static Display primary;
		private const int maxDisplayCount = 20;

		/// <summary>
		/// Gets a list of the display devices that are available.
		/// </summary>
		public override Collections.ObjectModel.ReadOnlyCollection<Display> DisplayDevices {
			get {
				return availableDevices.AsReadOnly();
			}
		}

		/// <summary>
		/// Gets the primary display.
		/// </summary>
		public override Display Primary {
			get {
				return primary;
			}
		}

		static MacOSFactory() {
			IntPtr[] displays = new IntPtr[maxDisplayCount];
			int displayCount;
			unsafe
			{
				fixed (IntPtr* displayPtr = displays)
					CG.GetActiveDisplayList(maxDisplayCount, displayPtr, out displayCount);
			}
			IntPtr currentDisplay;
			bool isPrimary;
			CFArray displayModes;
			Resolution gamegl_dev_current_res, thisRes;
			List<Resolution> gamegl_dev_available_res;
			CFDictionary currentMode, dict;
			HIRect bounds;
			Display gamegl_dev;
			for (int i = 0; i < displayCount; i++) {
				currentDisplay = displays[i];
				isPrimary = (i == 0);
				displayModes = new CFArray(CG.DisplayAvailableModes(currentDisplay));
				gamegl_dev_current_res = Resolution.Empty;
				gamegl_dev_available_res = new List<Resolution>();
				currentMode = new CFDictionary(CG.DisplayCurrentMode(currentDisplay));
				for (int j = 0; j < displayModes.Count; j++) {
					dict = new CFDictionary(displayModes[j]);
					thisRes = new Resolution(0, 0, (int) dict.GetNumberValue("Width"), (int) dict.GetNumberValue("Height"), (int) dict.GetNumberValue("BitsPerPixel"), dict.GetNumberValue("RefreshRate"));
					gamegl_dev_available_res.Add(thisRes);
					if (currentMode.Ref == dict.Ref)
						gamegl_dev_current_res = thisRes;
				}
				bounds = CG.DisplayBounds(currentDisplay);
				gamegl_dev_current_res.Bounds = new Rectangle((int) bounds.Origin.X, (int) bounds.Origin.Y, (int) bounds.Size.Width, (int) bounds.Size.Height);
				gamegl_dev = new Display(gamegl_dev_current_res, isPrimary, gamegl_dev_available_res, currentDisplay);
				availableDevices.Add(gamegl_dev);
				if (isPrimary)
					primary = gamegl_dev;
			}
		}

		internal MacOSFactory() {
		}

		/// <summary>
		/// Initializes a new graphics context.
		/// </summary>
		/// <param name="mode">The graphics configuration to use.</param>
		/// <param name="window">The window to render onto.</param>
		/// <param name="version">The requested OpenGL version.</param>
		/// <param name="sharedContext">A context with which to share resources (textures, buffers...).</param>
		public override GraphicsContext CreateGLContext(GraphicsMode mode, Window window, MajorMinorVersion version, GraphicsContext sharedContext = null) {
			return new MacOSGraphicsContext(mode, window, version, sharedContext);
		}

		/// <summary>
		/// Initializes a graphics context wrapper.
		/// </summary>
		/// <param name="handle">A handle that points to a native context.</param>
		public override GraphicsContext CreateGLContext(IntPtr handle) {
			return new MacOSGraphicsContext(handle);
		}

		/// <summary>
		/// Gets the related info of the window that has the specified handle.
		/// </summary>
		/// <param name="handle">The handle of the window.</param>
		/// <param name="mode">The GraphicsMode of the window.</param>
		public override Window GetWindowInfo(IntPtr handle, GraphicsMode mode) {
			if (windows.ContainsKey(handle))
				return windows[handle];
			else {
				Window window = new MacOSWindow(handle);
				windows.Add(handle, window);
				return window;
			}
		}

		/// <summary>
		/// Gets a handle that points to the current graphics context.
		/// </summary>
		public override IntPtr GetCurrentContext() {
			return Agl.GetCurrentContext();
		}

		/// <summary>
		/// Selects the graphics mode that is the most similar to the specified mode.
		/// </summary>
		/// <param name="mode">The mode to search similarity with.</param>
		/// <param name="window">The window to use for querying.</param>
		public override GraphicsMode SelectClosestGraphicsMode(GraphicsMode mode, IntPtr window) {
			List<int> attribs = new List<int>();

			if (mode.ColorFormat.BitsPerPixel > 0) {
				if (!mode.ColorFormat.IsIndexed) {
					attribs.Add((int) PixelFormatAttribute.RGBA);
				}
				attribs.Add((int) PixelFormatAttribute.RED_SIZE);
				attribs.Add(mode.ColorFormat.Red);
				attribs.Add((int) PixelFormatAttribute.GREEN_SIZE);
				attribs.Add(mode.ColorFormat.Green);
				attribs.Add((int) PixelFormatAttribute.BLUE_SIZE);
				attribs.Add(mode.ColorFormat.Blue);
				attribs.Add((int) PixelFormatAttribute.ALPHA_SIZE);
				attribs.Add(mode.ColorFormat.Alpha);
			}

			if (mode.Depth > 0) {
				attribs.Add((int) PixelFormatAttribute.DEPTH_SIZE);
				attribs.Add(mode.Depth);
			}

			if (mode.DoubleBuffering)
				attribs.Add((int) PixelFormatAttribute.DOUBLEBUFFER);

			if (mode.Stencil > 1) {
				attribs.Add((int) PixelFormatAttribute.STENCIL_SIZE);
				attribs.Add(mode.Stencil);
			}

			if (mode.AccumFormat.BitsPerPixel > 0) {
				attribs.Add((int) PixelFormatAttribute.ACCUM_ALPHA_SIZE);
				attribs.Add(mode.AccumFormat.Alpha);
				attribs.Add((int) PixelFormatAttribute.ACCUM_BLUE_SIZE);
				attribs.Add(mode.AccumFormat.Blue);
				attribs.Add((int) PixelFormatAttribute.ACCUM_GREEN_SIZE);
				attribs.Add(mode.AccumFormat.Green);
				attribs.Add((int) PixelFormatAttribute.ACCUM_RED_SIZE);
				attribs.Add(mode.AccumFormat.Red);
			}

			if (mode.Samples > 0) {
				attribs.Add((int) PixelFormatAttribute.SAMPLE_BUFFERS_ARB);
				attribs.Add(1);
				attribs.Add((int) PixelFormatAttribute.SAMPLES_ARB);
				attribs.Add(mode.Samples);
			}

			if (mode.Stereo) {
				attribs.Add((int) PixelFormatAttribute.STEREO);
			}

			attribs.Add(0);
			attribs.Add(0);
			IntPtr temp = IntPtr.Zero;
			IntPtr pixelformat = Agl.ChoosePixelFormat(ref temp, 0, attribs.ToArray());
			if (pixelformat == IntPtr.Zero) {
				throw new ArgumentException(string.Format(
					"[Error] Failed to select GraphicsMode, error {0}.", Agl.GetError()));
			}
			int r, g, b, a, ar, ag, ab, aa, stereoInt;
			Agl.DescribePixelFormat(pixelformat, PixelFormatAttribute.RED_SIZE, out r);
			Agl.DescribePixelFormat(pixelformat, PixelFormatAttribute.GREEN_SIZE, out g);
			Agl.DescribePixelFormat(pixelformat, PixelFormatAttribute.BLUE_SIZE, out b);
			Agl.DescribePixelFormat(pixelformat, PixelFormatAttribute.ALPHA_SIZE, out a);
			Agl.DescribePixelFormat(pixelformat, PixelFormatAttribute.ACCUM_ALPHA_SIZE, out aa);
			Agl.DescribePixelFormat(pixelformat, PixelFormatAttribute.ACCUM_RED_SIZE, out ar);
			Agl.DescribePixelFormat(pixelformat, PixelFormatAttribute.ACCUM_GREEN_SIZE, out ag);
			Agl.DescribePixelFormat(pixelformat, PixelFormatAttribute.ACCUM_BLUE_SIZE, out ab);
			int depth, stencil, samples, buffers;
			Agl.DescribePixelFormat(pixelformat, PixelFormatAttribute.DEPTH_SIZE, out depth);
			Agl.DescribePixelFormat(pixelformat, PixelFormatAttribute.STENCIL_SIZE, out stencil);
			Agl.DescribePixelFormat(pixelformat, PixelFormatAttribute.SAMPLES_ARB, out samples);
			Agl.DescribePixelFormat(pixelformat, PixelFormatAttribute.DOUBLEBUFFER, out buffers);
			Agl.DescribePixelFormat(pixelformat, PixelFormatAttribute.STEREO, out stereoInt);
			return new GraphicsMode(pixelformat, new ColorFormat(r, g, b, a), depth, stencil, samples, new ColorFormat(ar, ag, ab, aa), buffers + 1 > 1, stereoInt != 0);
		}

		/// <summary>
		/// Gets a pointer that points to the address of the specified native function.
		/// </summary>
		/// <param name="function">The function to find.</param>
		public override IntPtr GetAddress(string function) {
			return Agl.GetAddress(function);
		}

		/// <summary>
		/// Tries to change the resolution of the specified display to the given parameters.
		/// </summary>
		/// <param name="device">The display device whose resolution to change.</param>
		/// <param name="resolution">The target resolution of the display.</param>
		public override bool TryChangeResolution(Display device, Resolution resolution) {
			IntPtr display = (IntPtr) device.Id;
			if (!storedModes.ContainsKey(display))
				storedModes.Add(display, CG.DisplayCurrentMode(display));
			CFArray displayModes = new CFArray(CG.DisplayAvailableModes(display));
			CFDictionary dict;
			for (int j = 0; j < displayModes.Count; j++) {
				dict = new CFDictionary(displayModes[j]);
				if ((int) dict.GetNumberValue("Width") == resolution.Width && (int) dict.GetNumberValue("Height") == resolution.Height && (int) dict.GetNumberValue("BitsPerPixel") == resolution.BitsPerPixel && System.Math.Abs(dict.GetNumberValue("RefreshRate") - resolution.RefreshRate) < 1e-6) {
					if (!displaysCaptured.Contains(display))
						CG.DisplayCapture(display);
					CG.DisplaySwitchToMode(display, displayModes[j]);
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Tries to restore the resolution of the specified display to the default resolution.
		/// </summary>
		/// <param name="device">The display device.</param>
		public override bool TryRestoreResolution(Display device) {
			IntPtr display = (IntPtr) device.Id;
			if (storedModes.ContainsKey(display)) {
				CG.DisplaySwitchToMode(display, storedModes[display]);
				CG.DisplayRelease(display);
				displaysCaptured.Remove(display);
				return true;
			} else
				return false;
		}
	}
}