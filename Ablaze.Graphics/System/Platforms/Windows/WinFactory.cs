using System.Collections.Generic;
using System.Graphics.WGL;

namespace System.Platforms.Windows {
	using Graphics;

	/// <summary>
	/// Handles the creation of native resources.
	/// </summary>
	public class WinFactory : NativeFactory {
		private static object displayLock = new object();
		private static List<Display> availableDevices = new List<Display>();
		private static Display primary = new Display();
        internal static Dictionary<IntPtr, Window> windows = new Dictionary<IntPtr, Window>();

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

		static WinFactory() {
			RefreshDisplayDevices();
        }

		internal WinFactory() {
		}

		/// <summary>
		/// Initializes a new graphics context.
		/// </summary>
		/// <param name="mode">The graphics configuration to use.</param>
		/// <param name="window">The window to render onto.</param>
		/// <param name="version">The requested OpenGL version.</param>
		/// <param name="sharedContext">A context with which to share resources (textures, buffers...).</param>
		public override GraphicsContext CreateGLContext(GraphicsMode mode, Window window, MajorMinorVersion version, GraphicsContext sharedContext = null) {
			return new WinGraphicsContext(mode, (WinWindow) window, version, sharedContext);
		}

		/// <summary>
		/// Initializes a graphics context wrapper.
		/// </summary>
		/// <param name="handle">A handle that points to a native context.</param>
		public override GraphicsContext CreateGLContext(IntPtr handle) {
			return new WinGraphicsContext(handle);
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
				Window window = new WinWindow(handle);
				windows.Add(handle, window);
				return window;
			}
		}

		/// <summary>
		/// Gets a handle that points to the current graphics context.
		/// </summary>
		public override IntPtr GetCurrentContext() {
			return Wgl.GetCurrentContext();
		}

		/// <summary>
		/// Selects the graphics mode that is the most similar to the specified mode.
		/// </summary>
		/// <param name="mode">The mode to search similarity with.</param>
		/// <param name="window">The window to use for querying.</param>
		public override GraphicsMode SelectClosestGraphicsMode(GraphicsMode mode, IntPtr window) {
			IntPtr dc = NativeApi.GetDC(window);
			try {
				GraphicsMode resultant = GraphicsMode.Empty;
				GraphicsMode oldMode = mode;
				if (!(Wgl.Delegates.wglChoosePixelFormatARB == null || Wgl.Delegates.wglGetPixelFormatAttribivARB == null)) {
					do {
						resultant = TryFindGraphicsMode(ref mode, dc);
					} while (resultant == GraphicsMode.Empty && RelaxParameters(ref mode));
					mode = oldMode;
				}
				if (resultant == GraphicsMode.Empty) {
					bool supportComposition = Platform.IsWindowsVistaOrNewer;
					do {
						do {
							PixelFormatDescriptor pfd = new PixelFormatDescriptor() {
								Size = PixelFormatDescriptor.StructSize,
								Version = 1,
								Flags = PixelFormatDescriptorFlags.SUPPORT_OPENGL | PixelFormatDescriptorFlags.DRAW_TO_WINDOW
							};
							if (mode.DoubleBuffering)
								pfd.Flags |= PixelFormatDescriptorFlags.DOUBLEBUFFER;
							if (supportComposition)
								pfd.Flags |= PixelFormatDescriptorFlags.SUPPORT_COMPOSITION;
							pfd.AlphaBits = mode.ColorFormat.Alpha;
							pfd.AccumBits = (byte) mode.AccumFormat.BitsPerPixel;
							pfd.DepthBits = (byte) mode.Depth;
							pfd.StencilBits = (byte) mode.Stencil;
							int index = NativeApi.ChoosePixelFormat(dc, ref pfd);
							if (!(index == 0 || NativeApi.DescribePixelFormat(dc, index, (uint) PixelFormatDescriptor.StructSize, ref pfd) == 0)) {
								resultant = new GraphicsMode((IntPtr) index, new ColorFormat(pfd.RedBits, pfd.GreenBits, pfd.BlueBits, pfd.AlphaBits),
											pfd.DepthBits, pfd.StencilBits, 0, new ColorFormat(pfd.AccumBits),
											(pfd.Flags & PixelFormatDescriptorFlags.DOUBLEBUFFER) != 0,
											(pfd.Flags & PixelFormatDescriptorFlags.STEREO) != 0);
							}
						} while (resultant == GraphicsMode.Empty && RelaxParameters(ref mode));
						if (supportComposition) {
							supportComposition = false;
							mode = oldMode;
						} else
							break;
					} while (true);
				}
				return resultant;
			} finally {
				if (dc != IntPtr.Zero)
					NativeApi.ReleaseDC(window, dc);
			}
		}

		private static GraphicsMode TryFindGraphicsMode(ref GraphicsMode mode, IntPtr dc) {
			List<int> modeAttribs = new List<int>(26);
			modeAttribs.Add((int) ArbPixelFormat.AccelerationArb);
			modeAttribs.Add((int) ArbPixelFormat.FullAccelerationArb);
			modeAttribs.Add((int) ArbPixelFormat.SupportOpenglArb);
			modeAttribs.Add(1);
			modeAttribs.Add((int) ArbPixelFormat.DrawToWindowArb);
			modeAttribs.Add(1);
			modeAttribs.Add((int) ArbPixelFormat.AlphaBitsArb);
			modeAttribs.Add(mode.ColorFormat.Alpha);
			modeAttribs.Add((int) ArbPixelFormat.AccumBitsArb);
			modeAttribs.Add(mode.AccumFormat.BitsPerPixel);
			modeAttribs.Add((int) ArbPixelFormat.DepthBitsArb);
			modeAttribs.Add(mode.Depth);
			modeAttribs.Add((int) ArbPixelFormat.DoubleBufferArb);
			modeAttribs.Add(mode.DoubleBuffering ? 1 : 0);
			modeAttribs.Add((int) ArbPixelFormat.StencilBitsArb);
			modeAttribs.Add(mode.Stencil);
			if (mode.Stereo) {
				modeAttribs.Add((int) ArbPixelFormat.StereoArb);
				modeAttribs.Add(mode.Stereo ? 1 : 0);
			}
			if (mode.Samples > 1) {
				modeAttribs.Add((int) ArbMultiSample.SampleBuffersArb);
				modeAttribs.Add(1);
				modeAttribs.Add((int) ArbMultiSample.SamplesArb);
				modeAttribs.Add(mode.Samples);
			}
			modeAttribs.Add(0);
			modeAttribs.Add(0);
			int[] num_formats = new int[1];
			int[] pixels = new int[1];
			if (Wgl.Arb.ChoosePixelFormat(dc, modeAttribs.ToArray(), null, 1, pixels, num_formats) && num_formats[0] != 0) {
				unsafe
				{
					int* attribs = stackalloc int[18];
					attribs[0] = (int) ArbPixelFormat.AccelerationArb;
					attribs[1] = (int) ArbPixelFormat.RedBitsArb;
					attribs[2] = (int) ArbPixelFormat.GreenBitsArb;
					attribs[3] = (int) ArbPixelFormat.BlueBitsArb;
					attribs[4] = (int) ArbPixelFormat.AlphaBitsArb;
					attribs[5] = (int) ArbPixelFormat.ColorBitsArb;
					attribs[6] = (int) ArbPixelFormat.DepthBitsArb;
					attribs[7] = (int) ArbPixelFormat.StencilBitsArb;
					attribs[8] = (int) ArbMultiSample.SampleBuffersArb;
					attribs[9] = (int) ArbMultiSample.SamplesArb;
					attribs[10] = (int) ArbPixelFormat.AccumRedBitsArb;
					attribs[11] = (int) ArbPixelFormat.AccumGreenBitsArb;
					attribs[12] = (int) ArbPixelFormat.AccumBlueBitsArb;
					attribs[13] = (int) ArbPixelFormat.AccumAlphaBitsArb;
					attribs[14] = (int) ArbPixelFormat.AccumBitsArb;
					attribs[15] = (int) ArbPixelFormat.DoubleBufferArb;
					attribs[16] = (int) ArbPixelFormat.StereoArb;
					attribs[17] = 0;
					int* values = stackalloc int[18];
					if (Wgl.Arb.GetPixelFormatAttrib(dc, pixels[0], 0, 17, attribs, values)) {
						return new GraphicsMode(new IntPtr(pixels[0]), new ColorFormat(values[1], values[2], values[3], values[4]),
							values[6], values[7], values[8] == 0 ? 0 : values[9],
							new ColorFormat(values[10], values[11], values[12], values[13]), values[15] == 1, values[16] == 1);
					}
				}
			}
			return GraphicsMode.Empty;
		}

		/*private static GraphicsMode ToGraphicsMode(PixelFormatDescriptor pfd) {
			return new GraphicsMode(new ColorFormat(pfd.RedBits, pfd.GreenBits, pfd.BlueBits, pfd.AlphaBits),
								pfd.DepthBits,
								pfd.StencilBits,
								0,
								new ColorFormat(pfd.AccumBits),
								(pfd.Flags & PixelFormatDescriptorFlags.DOUBLEBUFFER) == 0 ? 1 : 2,
								(pfd.Flags & PixelFormatDescriptorFlags.STEREO) != 0);
		}*/

		/// <summary>
		/// Gets a pointer that points to the address of the specified native function.
		/// </summary>
		/// <param name="function">The function to find.</param>
		public override IntPtr GetAddress(string function) {
			return Wgl.Core.GetProcAddress(function);
		}

		/// <summary>
		/// Tries to change the resolution of the specified display to the given parameters.
		/// </summary>
		/// <param name="device">The display device whose resolution to change.</param>
		/// <param name="resolution">The target resolution of the display.</param>
		public override bool TryChangeResolution(Display device, Resolution resolution) {
			DeviceMode mode = null;
			if (resolution != Resolution.Empty) {
				mode = new DeviceMode() {
					PelsWidth = resolution.Width,
					PelsHeight = resolution.Height,
					BitsPerPel = resolution.BitsPerPixel,
					DisplayFrequency = (int) resolution.RefreshRate,
					Fields = Constants.DM_BITSPERPEL | Constants.DM_PELSWIDTH | Constants.DM_PELSHEIGHT | Constants.DM_DISPLAYFREQUENCY
				};
			}
			return Constants.DISP_CHANGE_SUCCESSFUL ==
				NativeApi.ChangeDisplaySettingsEx((string) device.Id, mode, IntPtr.Zero, ChangeDisplaySettingsEnum.Fullscreen, IntPtr.Zero);
		}

		/// <summary>
		/// Tries to restore the resolution of the specified display to the default resolution.
		/// </summary>
		/// <param name="device">The display device.</param>
		public override bool TryRestoreResolution(Display device) {
			return TryChangeResolution(device, Resolution.Empty);
		}

        /// <summary>
        /// Call this when a screen's resolution has been changed.
        /// </summary>
		public static void RefreshDisplayDevices() {
			lock (displayLock) {
				availableDevices = new List<Display>();
				Display gamegl_dev;
				Resolution gamegl_dev_current_res;
				List<Resolution> gamegl_dev_available_res = new List<Resolution>();
				bool gamegl_dev_primary;
				int mode_count, device_count = 0;
				WindowsDisplayDevice dev1 = new WindowsDisplayDevice();
				DeviceMode monitor_mode;
				while (NativeApi.EnumDisplayDevices(null, device_count++, dev1, 0)) {
					if ((dev1.StateFlags & DisplayDeviceStateFlags.AttachedToDesktop) == DisplayDeviceStateFlags.None)
						continue;
					gamegl_dev_current_res = Resolution.Empty;
					gamegl_dev_primary = false;
					monitor_mode = new DeviceMode();
					if (NativeApi.EnumDisplaySettingsEx(dev1.DeviceName, DisplayModeSettingsEnum.CurrentSettings, monitor_mode, 0) ||
						NativeApi.EnumDisplaySettingsEx(dev1.DeviceName, DisplayModeSettingsEnum.RegistrySettings, monitor_mode, 0)) {
						gamegl_dev_current_res = new Resolution(monitor_mode.Position.X, monitor_mode.Position.Y,
							monitor_mode.PelsWidth, monitor_mode.PelsHeight, monitor_mode.BitsPerPel, monitor_mode.DisplayFrequency);
						gamegl_dev_primary = (dev1.StateFlags & DisplayDeviceStateFlags.PrimaryDevice) != DisplayDeviceStateFlags.None;
					}
					mode_count = 0;
					while (NativeApi.EnumDisplaySettings(dev1.DeviceName, mode_count++, monitor_mode)) {
						gamegl_dev_available_res.Add(new Resolution(monitor_mode.Position.X, monitor_mode.Position.Y,
							monitor_mode.PelsWidth, monitor_mode.PelsHeight, monitor_mode.BitsPerPel, monitor_mode.DisplayFrequency));
					}
					gamegl_dev = new Display(gamegl_dev_current_res, gamegl_dev_primary, gamegl_dev_available_res, dev1.DeviceName);
					availableDevices.Add(gamegl_dev);
					if (gamegl_dev_primary)
						primary.CopyFrom(gamegl_dev);
				}
				if (primary == null)
					primary.CopyFrom(availableDevices[0]);
			}
		}

		private static bool RelaxParameters(ref GraphicsMode mode) {
			ColorFormat color = mode.ColorFormat;
			int depth = mode.Depth;
			int stencil = mode.Stencil;
			int samples = mode.Samples;
			ColorFormat accum = mode.AccumFormat;
			bool buffering = mode.DoubleBuffering;
			bool stereo = mode.Stereo;
			if (stereo)
				stereo = false;
			else if (!buffering)
				buffering = true;
			else if (accum != ColorFormat.Empty)
				accum = ColorFormat.Empty;
			else if (samples != 0)
				samples >>= 1;
			else if (depth < 16)
				depth = 16;
			else if (depth != 24)
				depth = 24;
			else if (stencil > 0 && stencil != 8)
				stencil = 8;
			else if (stencil == 8)
				stencil = 0;
			else if (color.BitsPerPixel < 8)
				color = ColorFormat.Bit8;
			else if (color.BitsPerPixel < 16)
				color = ColorFormat.Bit16;
			else if (color.BitsPerPixel < 24)
				color = ColorFormat.Bit24;
			else if (color.BitsPerPixel < 32 || color.BitsPerPixel > 32)
				color = ColorFormat.Bit32;
			else
				return false;
			mode = new GraphicsMode(mode.Index, color, depth, stencil, samples, accum, buffering, stereo);
			return true;
		}
    }
}