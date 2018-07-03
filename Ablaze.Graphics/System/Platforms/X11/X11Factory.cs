using System.Collections.Generic;
using System.Drawing;
using System.Graphics;
using System.Graphics.GLX;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Platforms.X11 {
	/// <summary>
	/// Handles the creation of native X11 resources.
	/// </summary>
	public class X11Factory : NativeFactory {
		private static List<Dictionary<Resolution, int>> screenResolutionToIndex = new List<Dictionary<Resolution, int>>();
		private static Dictionary<Display, int> deviceToDefaultResolution = new Dictionary<Display, int>();
		private static Dictionary<Display, int> deviceToScreen = new Dictionary<Display, int>();
		private static bool xrandr_supported;
		private static List<Display> availableDevices = new List<Display>();
		private static FieldInfo DisplayHandle, RootWindow, ScreenNo, CustomVisual, CustomColormap;
		private static Display primary;

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

		static X11Factory() {
			IntPtr display = NativeApi.DefaultDisplay;
			NativeApi.XLockDisplay(display);
			bool xinerama_supported = false;
			try {
				int event_base, error_base;
				if (NativeApi.XineramaQueryExtension(display, out event_base, out error_base) &&
					NativeApi.XineramaIsActive(display)) {
					IList<XineramaScreenInfo> screens = NativeApi.XineramaQueryScreens(display);
					bool first = true;
					Display dev;
					foreach (XineramaScreenInfo screen in screens) {
						dev = new Display();
						dev.Bounds = new Rectangle(screen.X, screen.Y, screen.Width, screen.Height);
						if (first) {
							dev.IsPrimary = true;
							first = false;
						}
						availableDevices.Add(dev);
						deviceToScreen.Add(dev, screen.ScreenNumber);
					}
				}
				xinerama_supported = availableDevices.Count != 0;
			} catch {
			}
			if (!xinerama_supported) {
				Display dev;
				int screenCount = NativeApi.ScreenCount;
				for (int i = 0; i < screenCount; i++) {
					dev = new Display();
					dev.IsPrimary = i == NativeApi.XDefaultScreen(display);
					availableDevices.Add(dev);
					deviceToScreen.Add(dev, i);
				}
			}
			try {
				int screen;
				List<Resolution> available_res;
				int[] depths;
				short[] rates;
				Resolution res;
				foreach (Display dev in availableDevices) {
					screen = deviceToScreen[dev];
					available_res = new List<Resolution>();
					screenResolutionToIndex.Add(new Dictionary<Resolution, int>());
					depths = NativeApi.XListDepths(display, screen);
					int resolution_count = 0;
					foreach (XRRScreenSize size in FindAvailableResolutions(screen)) {
						if (size.Width == 0 || size.Height == 0)
							continue;
						rates = NativeApi.XRRRates(display, screen, resolution_count);
						foreach (short rate in rates) {
							if (rate != 0 || rates.Length == 1)
								foreach (int depth in depths)
									available_res.Add(new Resolution(0, 0, size.Width, size.Height, depth, rate));
						}
						foreach (int depth in depths) {
							res = new Resolution(0, 0, size.Width, size.Height, depth, 0);
							if (!screenResolutionToIndex[screen].ContainsKey(res))
								screenResolutionToIndex[screen].Add(res, resolution_count);
						}
						++resolution_count;
					}
					IntPtr screen_config = NativeApi.XRRGetScreenInfo(display, NativeApi.XRootWindow(display, screen));
					ushort current_rotation;
					int current_resolution_index = NativeApi.XRRConfigCurrentConfiguration(screen_config, out current_rotation);
					if (dev.Bounds == Rectangle.Empty)
						dev.Bounds = new Rectangle(0, 0, available_res[current_resolution_index].Width, available_res[current_resolution_index].Height);
					dev.BitsPerPixel = (int) NativeApi.XDefaultDepth(display, screen);
					dev.RefreshRate = NativeApi.XRRConfigCurrentRate(screen_config);
					NativeApi.XRRFreeScreenConfigInfo(screen_config);
					dev.availableResolutions = available_res;
					deviceToDefaultResolution.Add(dev, current_resolution_index);
				}
				xrandr_supported = true;
			} catch {
			}
			if (!xrandr_supported) {
				try {
					int major;
					int minor;
					if (NativeApi.XF86VidModeQueryVersion(display, out major, out minor)) {
						int x, y, pixelClock, count, currentScreen = 0;
						IntPtr srcArray;
						IntPtr[] array;
						NativeApi.XF86VidModeModeInfo Mode;
						NativeApi.XF86VidModeModeLine currentMode;
						List<Resolution> resolutions;
						Type ModeInfo = typeof(NativeApi.XF86VidModeModeInfo);
						foreach (Display dev in availableDevices) {
							NativeApi.XF86VidModeGetAllModeLines(display, currentScreen, out count, out srcArray);
							array = new IntPtr[count];
							Marshal.Copy(srcArray, array, 0, count);
							Mode = new NativeApi.XF86VidModeModeInfo();
							NativeApi.XF86VidModeGetViewPort(display, currentScreen, out x, out y);
							resolutions = new List<Resolution>();
							for (int i = 0; i < count; i++) {
								Mode = (NativeApi.XF86VidModeModeInfo) Marshal.PtrToStructure(array[i], ModeInfo);
								resolutions.Add(new Resolution(x, y, Mode.hdisplay, Mode.vdisplay, 24, (Mode.dotclock * 1000D) / (Mode.vtotal * Mode.htotal)));
							}
							dev.availableResolutions = resolutions;
							NativeApi.XF86VidModeGetModeLine(display, currentScreen, out pixelClock, out currentMode);
							dev.Bounds = new Rectangle(x, y, currentMode.hdisplay, (currentMode.vdisplay == 0) ? currentMode.vsyncstart : currentMode.vdisplay);
							dev.BitsPerPixel = (int) NativeApi.XDefaultDepth(display, currentScreen);
							dev.RefreshRate = (pixelClock * 1000D) / (currentMode.vtotal * currentMode.htotal);
							currentScreen++;
						}
					}
				} catch {
				}
			}
			foreach (Display dev in availableDevices) {
				if (dev.IsPrimary) {
					primary = dev;
					break;
				}
			}
			if (primary == null)
				primary = availableDevices[0];
			NativeApi.XUnlockDisplay(display);
		}

		/// <summary>
		/// Does nothing.
		/// </summary>
		internal X11Factory() {
		}

		/// <summary>
		/// Initializes a new graphics context.
		/// </summary>
		/// <param name="mode">The graphics configuration to use.</param>
		/// <param name="window">The window to render onto.</param>
		/// <param name="version">The requested OpenGL version.</param>
		/// <param name="sharedContext">A context with which to share resources (textures, buffers...).</param>
		public override GraphicsContext CreateGLContext(GraphicsMode mode, Window window, MajorMinorVersion version, GraphicsContext sharedContext = null) {
			return new X11GraphicsContext(mode, window, version, sharedContext);
		}

		/// <summary>
		/// Initializes a graphics context wrapper.
		/// </summary>
		/// <param name="handle">A handle that points to a native context.</param>
		public override GraphicsContext CreateGLContext(IntPtr handle) {
			return new X11GraphicsContext(handle);
		}

		/// <summary>
		/// Gets the related info of the window that has the specified handle.
		/// </summary>
		/// <param name="handle">The handle of the window.</param>
		/// <param name="mode">The GraphicsMode of the window.</param>
		public override Window GetWindowInfo(IntPtr handle, GraphicsMode mode) {
			if (DisplayHandle == null) {
				Type xplatui = Type.GetType("System.Windows.Forms.XplatUIX11, System.Windows.Forms");
				DisplayHandle = xplatui.GetField(nameof(DisplayHandle), BindingFlags.Static | BindingFlags.NonPublic);
				RootWindow = xplatui.GetField(nameof(RootWindow), BindingFlags.Static | BindingFlags.NonPublic);
				ScreenNo = xplatui.GetField(nameof(ScreenNo), BindingFlags.Static | BindingFlags.NonPublic);
				CustomVisual = xplatui.GetField(nameof(CustomVisual), BindingFlags.Static | BindingFlags.NonPublic);
				CustomColormap = xplatui.GetField(nameof(CustomColormap), BindingFlags.Static | BindingFlags.NonPublic);
			}
			IntPtr display = (IntPtr) DisplayHandle.GetValue(null);
			IntPtr rootWindow = (IntPtr) RootWindow.GetValue(null);
			int screen = (int) ScreenNo.GetValue(null);
			XVisualInfo info = new XVisualInfo();
			info.VisualID = mode.Index.Value;
			int dummy;
			IntPtr infoPtr = NativeApi.XGetVisualInfo(display, new IntPtr(1), ref info, out dummy);
			info = (XVisualInfo) Marshal.PtrToStructure(infoPtr, typeof(XVisualInfo));
			CustomVisual.SetValue(null, info.Visual);
			CustomColormap.SetValue(null, NativeApi.XCreateColormap(display, rootWindow, info.Visual, 0));
			return new X11Window(handle, display, screen, (XVisualInfo) Marshal.PtrToStructure(infoPtr, typeof(XVisualInfo)));
		}

		/// <summary>
		/// Gets a handle that points to the current graphics context.
		/// </summary>
		public override IntPtr GetCurrentContext() {
			return Glx.GetCurrentContext();
		}

		/// <summary>
		/// Selects the graphics mode that is the most similar to the specified mode.
		/// </summary>
		/// <param name="mode">The mode to search similarity with.</param>
		/// <param name="window">The window to use for querying.</param>
		public override GraphicsMode SelectClosestGraphicsMode(GraphicsMode mode, IntPtr window) {
			if (mode.Index != null)
				return mode;
			GraphicsMode gfx;
			IntPtr visual;
			IntPtr display = NativeApi.DefaultDisplay;
			visual = SelectVisualUsingFBConfig(ref mode);
			if (visual == IntPtr.Zero)
				visual = SelectVisualUsingChooseVisual(ref mode);
			if (visual == IntPtr.Zero)
				throw new ArgumentException("Requested GraphicsMode not available.");
			XVisualInfo info = (XVisualInfo) Marshal.PtrToStructure(visual, typeof(XVisualInfo));
			int r, g, b, a;
			Glx.GetConfig(display, ref info, PixelFormatAttribute.ALPHA_SIZE, out a);
			Glx.GetConfig(display, ref info, PixelFormatAttribute.RED_SIZE, out r);
			Glx.GetConfig(display, ref info, PixelFormatAttribute.GREEN_SIZE, out g);
			Glx.GetConfig(display, ref info, PixelFormatAttribute.BLUE_SIZE, out b);
			int ar, ag, ab, aa;
			Glx.GetConfig(display, ref info, PixelFormatAttribute.ACCUM_ALPHA_SIZE, out aa);
			Glx.GetConfig(display, ref info, PixelFormatAttribute.ACCUM_RED_SIZE, out ar);
			Glx.GetConfig(display, ref info, PixelFormatAttribute.ACCUM_GREEN_SIZE, out ag);
			Glx.GetConfig(display, ref info, PixelFormatAttribute.ACCUM_BLUE_SIZE, out ab);
			int depth, stencil, samples, buffers;
			Glx.GetConfig(display, ref info, PixelFormatAttribute.DEPTH_SIZE, out depth);
			Glx.GetConfig(display, ref info, PixelFormatAttribute.STENCIL_SIZE, out stencil);
			Glx.GetConfig(display, ref info, PixelFormatAttribute.SAMPLES, out samples);
			Glx.GetConfig(display, ref info, PixelFormatAttribute.DOUBLEBUFFER, out buffers);
			++buffers;
			int st;
			Glx.GetConfig(display, ref info, PixelFormatAttribute.STEREO, out st);
			bool stereo = st != 0;
			gfx = new GraphicsMode(info.VisualID, new ColorFormat(r, g, b, a), depth, stencil, samples, new ColorFormat(ar, ag, ab, aa), buffers > 1, stereo);
			NativeApi.XLockDisplay(display);
			NativeApi.XFree(visual);
			NativeApi.XUnlockDisplay(display);
			return gfx;
		}

		/// <summary>
		/// Gets a pointer that points to the address of the specified native function.
		/// </summary>
		/// <param name="function">The function to find.</param>
		public override IntPtr GetAddress(string function) {
			IntPtr display = NativeApi.DefaultDisplay;
			NativeApi.XLockDisplay(display);
			IntPtr address = Glx.GetProcAddress(function);
			NativeApi.XUnlockDisplay(display);
			return address;
		}

		/// <summary>
		/// Tries to change the resolution of the specified display to the given parameters.
		/// </summary>
		/// <param name="device">The display device whose resolution to change.</param>
		/// <param name="resolution">The target resolution of the display.</param>
		public override bool TryChangeResolution(Display device, Resolution resolution) {
			if (xrandr_supported) {
				IntPtr display = NativeApi.DefaultDisplay;
				NativeApi.XLockDisplay(display);
				int screen = deviceToScreen[device];
				IntPtr root = NativeApi.XRootWindow(display, screen);
				IntPtr screen_config = NativeApi.XRRGetScreenInfo(display, root);
				ushort current_rotation;
				NativeApi.XRRConfigCurrentConfiguration(screen_config, out current_rotation);
				int new_resolution_index = resolution == Resolution.Empty ? deviceToDefaultResolution[device] : screenResolutionToIndex[screen][new Resolution(0, 0, resolution.Width, resolution.Height, resolution.BitsPerPixel, 0)];
				int ret = 0;
				short refresh_rate = (short) (resolution.RefreshRate);
				if (refresh_rate > 0) {
					ret = NativeApi.XRRSetScreenConfigAndRate(display,
					screen_config, root, new_resolution_index, current_rotation,
					refresh_rate, IntPtr.Zero);
				} else {
					ret = NativeApi.XRRSetScreenConfig(display,
					screen_config, root, new_resolution_index, current_rotation,
					IntPtr.Zero);
				}
				NativeApi.XUnlockDisplay(display);
				return ret == 0;
			} else
				return false;
		}

		/// <summary>
		/// Tries to restore the resolution of the specified display to the default resolution.
		/// </summary>
		/// <param name="device">The display device.</param>
		public override bool TryRestoreResolution(Display device) {
			return TryChangeResolution(device, Resolution.Empty);
		}

		private static IntPtr SelectVisualUsingFBConfig(ref GraphicsMode mode) {
			List<int> visualAttributes = new List<int>();
			if (mode.ColorFormat.BitsPerPixel > 0) {
				if (!mode.ColorFormat.IsIndexed) {
					visualAttributes.Add((int) PixelFormatAttribute.RGBA);
					visualAttributes.Add((int) PixelFormatAttribute.USE_GL);
				}
				visualAttributes.Add((int) PixelFormatAttribute.RED_SIZE);
				visualAttributes.Add(mode.ColorFormat.Red);
				visualAttributes.Add((int) PixelFormatAttribute.GREEN_SIZE);
				visualAttributes.Add(mode.ColorFormat.Green);
				visualAttributes.Add((int) PixelFormatAttribute.BLUE_SIZE);
				visualAttributes.Add(mode.ColorFormat.Blue);
				visualAttributes.Add((int) PixelFormatAttribute.ALPHA_SIZE);
				visualAttributes.Add(mode.ColorFormat.Alpha);
			}
			if (mode.Depth > 0) {
				visualAttributes.Add((int) PixelFormatAttribute.DEPTH_SIZE);
				visualAttributes.Add(mode.Depth);
			}

			if (mode.DoubleBuffering) {
				visualAttributes.Add((int) PixelFormatAttribute.DOUBLEBUFFER);
				visualAttributes.Add((int) PixelFormatAttribute.USE_GL);
			}

			if (mode.Stencil > 1) {
				visualAttributes.Add((int) PixelFormatAttribute.STENCIL_SIZE);
				visualAttributes.Add(mode.Stencil);
			}

			if (mode.AccumFormat.BitsPerPixel > 0) {
				visualAttributes.Add((int) PixelFormatAttribute.ACCUM_ALPHA_SIZE);
				visualAttributes.Add(mode.AccumFormat.Alpha);
				visualAttributes.Add((int) PixelFormatAttribute.ACCUM_BLUE_SIZE);
				visualAttributes.Add(mode.AccumFormat.Blue);
				visualAttributes.Add((int) PixelFormatAttribute.ACCUM_GREEN_SIZE);
				visualAttributes.Add(mode.AccumFormat.Green);
				visualAttributes.Add((int) PixelFormatAttribute.ACCUM_RED_SIZE);
				visualAttributes.Add(mode.AccumFormat.Red);
			}

			if (mode.Samples > 0) {
				visualAttributes.Add((int) PixelFormatAttribute.SAMPLE_BUFFERS);
				visualAttributes.Add((int) PixelFormatAttribute.USE_GL);
				visualAttributes.Add((int) PixelFormatAttribute.SAMPLES);
				visualAttributes.Add(mode.Samples);
			}

			if (mode.Stereo) {
				visualAttributes.Add((int) PixelFormatAttribute.STEREO);
				visualAttributes.Add((int) PixelFormatAttribute.USE_GL);
			}

			visualAttributes.Add((int) PixelFormatAttribute.EMPTY);
			IntPtr display = NativeApi.DefaultDisplay;
			IntPtr visual = IntPtr.Zero;
			NativeApi.XLockDisplay(display);
			try {
				int screen = NativeApi.XDefaultScreen(display);
				unsafe
				{
					int fbcount;
					IntPtr* fbconfigs = Glx.ChooseFBConfig(display, screen, visualAttributes.ToArray(), out fbcount);
					if (fbcount > 0 && fbconfigs != null) {
						visual = Glx.GetVisualFromFBConfig(display, *fbconfigs);
						NativeApi.XFree((IntPtr) fbconfigs);
					}
				}
			} catch {
				visual = IntPtr.Zero;
			}
			NativeApi.XUnlockDisplay(display);
			return visual;
		}

		private static IntPtr SelectVisualUsingChooseVisual(ref GraphicsMode mode) {
			List<int> visualAttributes = new List<int>();
			if (mode.ColorFormat.BitsPerPixel > 0) {
				if (!mode.ColorFormat.IsIndexed)
					visualAttributes.Add((int) PixelFormatAttribute.RGBA);
				visualAttributes.Add((int) PixelFormatAttribute.RED_SIZE);
				visualAttributes.Add(mode.ColorFormat.Red);
				visualAttributes.Add((int) PixelFormatAttribute.GREEN_SIZE);
				visualAttributes.Add(mode.ColorFormat.Green);
				visualAttributes.Add((int) PixelFormatAttribute.BLUE_SIZE);
				visualAttributes.Add(mode.ColorFormat.Blue);
				visualAttributes.Add((int) PixelFormatAttribute.ALPHA_SIZE);
				visualAttributes.Add(mode.ColorFormat.Alpha);
			}
			if (mode.Depth > 0) {
				visualAttributes.Add((int) PixelFormatAttribute.DEPTH_SIZE);
				visualAttributes.Add(mode.Depth);
			}

			if (mode.DoubleBuffering)
				visualAttributes.Add((int) PixelFormatAttribute.DOUBLEBUFFER);

			if (mode.Stencil > 1) {
				visualAttributes.Add((int) PixelFormatAttribute.STENCIL_SIZE);
				visualAttributes.Add(mode.Stencil);
			}

			if (mode.AccumFormat.BitsPerPixel > 0) {
				visualAttributes.Add((int) PixelFormatAttribute.ACCUM_ALPHA_SIZE);
				visualAttributes.Add(mode.AccumFormat.Alpha);
				visualAttributes.Add((int) PixelFormatAttribute.ACCUM_BLUE_SIZE);
				visualAttributes.Add(mode.AccumFormat.Blue);
				visualAttributes.Add((int) PixelFormatAttribute.ACCUM_GREEN_SIZE);
				visualAttributes.Add(mode.AccumFormat.Green);
				visualAttributes.Add((int) PixelFormatAttribute.ACCUM_RED_SIZE);
				visualAttributes.Add(mode.AccumFormat.Red);
			}

			if (mode.Samples > 0) {
				visualAttributes.Add((int) PixelFormatAttribute.SAMPLE_BUFFERS);
				visualAttributes.Add(1);
				visualAttributes.Add((int) PixelFormatAttribute.SAMPLES);
				visualAttributes.Add(mode.Samples);
			}

			if (mode.Stereo)
				visualAttributes.Add((int) PixelFormatAttribute.STEREO);

			visualAttributes.Add((int) PixelFormatAttribute.EMPTY);
			IntPtr display = NativeApi.DefaultDisplay;
			NativeApi.XLockDisplay(display);
			IntPtr visual = Glx.ChooseVisual(display, NativeApi.XDefaultScreen(display), visualAttributes.ToArray());
			NativeApi.XUnlockDisplay(display);
			return visual;
		}

		private static XRRScreenSize[] FindAvailableResolutions(int screen) {
			XRRScreenSize[] resolutions = NativeApi.XRRSizes(NativeApi.DefaultDisplay, screen);
			if (resolutions == null)
				throw new NotSupportedException("XRandR extensions not available.");
			return resolutions;
		}
	}
}