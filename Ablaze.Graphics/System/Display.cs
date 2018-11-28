using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace System {
	/// <summary>
	/// Provides methods to access and change display parameters.
	/// </summary>
	public sealed class Display {
		private bool primary;
		private Resolution currentResolution, originalResolution;
		internal List<Resolution> availableResolutions = new List<Resolution>();
		internal object Id; // A platform-specific id for this monitor
		private static object displayLock = new object();
		private static Display primaryDisplay;

		/// <summary>
		/// Gets the resolution info of the current display.
		/// </summary>
		public Resolution CurrentResolution {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return currentResolution;
			}
		}

		/// <summary>
		/// Gets the bounds of this instance in pixel coordinates.
		/// </summary>
		public Rectangle Bounds {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return currentResolution.Bounds;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			internal set {
				currentResolution.Bounds = value;
			}
		}

		/// <summary>Gets a System.Int32 that contains the width of this display in pixels.</summary>
		public int Width {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return currentResolution.Width;
			}
		}

		/// <summary>Gets a System.Int32 that contains the height of this display in pixels.</summary>
		public int Height {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return currentResolution.Height;
			}
		}

		/// <summary>Gets a System.Int32 that contains number of bits per pixel of this display. Typical values include 8, 16, 24 and 32.</summary>
		public int BitsPerPixel {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return currentResolution.BitsPerPixel;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			internal set {
				currentResolution.BitsPerPixel = value;
			}
		}

		/// <summary>
		/// Gets a System.Double representing the vertical refresh rate of this display.
		/// </summary>
		public double RefreshRate {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return currentResolution.RefreshRate;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			internal set {
				currentResolution.RefreshRate = value;
			}
		}

		/// <summary>Gets a System.Boolean that indicates whether this Display is the primary Display in systems with multiple Displays.</summary>
		public bool IsPrimary {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return primary;
			}
			internal set {
				if (value && primaryDisplay != null && primaryDisplay != this)
					primaryDisplay.IsPrimary = false;
				lock (displayLock) {
					primary = value;
					if (value)
						primaryDisplay = this;
				}
			}
		}

		/// <summary>Gets the default (primary) display of this system.</summary>
		public static Display Default {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return GraphicsPlatform.Factory.GetDisplay(0);
			}
		}

		/// <summary>
		/// Gets the list of <see cref="Resolution"/> objects available on this device.
		/// </summary>
		public Collections.ObjectModel.ReadOnlyCollection<Resolution> AvailableResolutions {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return availableResolutions.AsReadOnly();
			}
		}

		internal Display() {
		}

		internal Display(Resolution currentResolution, bool primary, List<Resolution> availableResolutions, object id) {
			this.currentResolution = currentResolution;
			IsPrimary = primary;
			this.availableResolutions = availableResolutions;
			Id = id;
		}

		internal void CopyFrom(Display temp) {
			lock (displayLock) {
				currentResolution = temp.currentResolution;
				originalResolution = temp.originalResolution;
				IsPrimary = temp.primary;
				availableResolutions = temp.availableResolutions;
				Id = temp.Id;
			}
		}

		/// <summary>
		/// Selects an available resolution that matches the specified parameters.
		/// </summary>
		/// <param name="width">The width of the requested resolution in pixels.</param>
		/// <param name="height">The height of the requested resolution in pixels.</param>
		/// <param name="bitsPerPixel">The bits per pixel of the requested resolution.</param>
		/// <param name="refreshRate">The refresh rate of the requested resolution in hertz.</param>
		/// <returns>The requested Display or null if the parameters cannot be met.</returns>
		/// <remarks>
		/// <para>If a matching resolution is not found, this function will retry ignoring the specified refresh rate,
		/// bits per pixel and resolution, in this order. If a matching resolution still doesn't exist, this function will
		/// return the current resolution.</para>
		/// <para>A parameter set to 0 or negative numbers will not be used in the search (e.g. if refreshRate is 0,
		/// any refresh rate will be considered valid).</para>
		/// <para>This function allocates memory.</para>
		/// </remarks>
		public Resolution SelectResolution(int width, int height, int bitsPerPixel, double refreshRate) {
			Resolution resolution = SelectClosestResolution(width, height, bitsPerPixel, refreshRate);
			if (resolution == Resolution.Empty)
				resolution = SelectClosestResolution(width, height, bitsPerPixel, 0);
			if (resolution == Resolution.Empty)
				resolution = SelectClosestResolution(width, height, 0, 0);
			if (resolution == Resolution.Empty)
				return currentResolution;
			return resolution;
		}

		/// <summary>Changes the resolution of the Displays.</summary>
		/// <param name="resolution">The resolution to set. <see cref="Display.SelectResolution"/></param>
		/// <exception cref="ArgumentException">Thrown if the requested resolution could not be set.</exception>
		/// <remarks>If the specified resolution is null, this function will restore the original Display.</remarks>
		public void ChangeResolution(Resolution resolution) {
			if (resolution == Resolution.Empty)
				RestoreResolution();

			if (resolution == currentResolution)
				return;
			if (GraphicsPlatform.Factory.TryChangeResolution(this, resolution)) {
				if (originalResolution == Resolution.Empty)
					originalResolution = currentResolution;
				currentResolution = resolution;
			} else
				throw new ArgumentException(string.Format("Device {0}: Failed to change resolution to {1}.", this, resolution));
		}

		/// <summary>Changes the resolution of the Displays.</summary>
		/// <param name="width">The new width of the Displays.</param>
		/// <param name="height">The new height of the Displays.</param>
		/// <param name="bitsPerPixel">The new bits per pixel of the Displays.</param>
		/// <param name="refreshRate">The new refresh rate of the Displays.</param>
		/// <exception cref="ArgumentException">Thrown if the requested resolution could not be set.</exception>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void ChangeResolution(int width, int height, int bitsPerPixel, double refreshRate) {
			ChangeResolution(SelectResolution(width, height, bitsPerPixel, refreshRate));
		}

		/// <summary>Restores the original resolution of the Displays.</summary>
		/// <exception cref="ArgumentException">Thrown if the original resolution could not be restored.</exception>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void RestoreResolution() {
			if (originalResolution != Resolution.Empty) {
				if (GraphicsPlatform.Factory.TryRestoreResolution(this))
					currentResolution = originalResolution;
				else
					throw new ArgumentException(string.Format("Device {0}: Failed to restore resolution.", this));
			}
		}

		/// <summary>
		/// Gets the <see cref="Display"/> for the specified index.
		/// </summary>
		/// <param name="index">The index that defines the desired display.</param>
		/// <returns>A <see cref="Display"/> or null, if no device corresponds to the specified index.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Display GetDisplay(int index) {
			return GraphicsPlatform.Factory.GetDisplay(index);
		}

		/// <summary>
		/// Gets the display that contains the specified point.
		/// </summary>
		/// <param name="point">The coordinate of the point.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Display GetDisplay(Point point) {
			return GraphicsPlatform.Factory.GetDisplay(point);
		}

		/// <summary>
		/// Gets the display that contains the specified control.
		/// </summary>
		/// <param name="control">The control.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Display GetDisplay(Windows.Forms.Control control) {
			return GraphicsPlatform.Factory.GetDisplay(control.Location);
		}

		/// <summary>
		/// Selects the closest available resolution.
		/// </summary>
		/// <param name="width">The width of the display in pixels.</param>
		/// <param name="height">The height of the display in pixels.</param>
		/// <param name="bitsPerPixel">The bit-depth of each pixel.</param>
		/// <param name="refreshRate">The number of frames displayes each second.</param>
		public Resolution SelectClosestResolution(int width, int height, int bitsPerPixel, double refreshRate) {
			return availableResolutions.Find(delegate (Resolution test) {
				return
					((width > 0 && width == test.Width) || width == 0) &&
					((height > 0 && height == test.Height) || height == 0) &&
					((bitsPerPixel > 0 && bitsPerPixel == test.BitsPerPixel) || bitsPerPixel == 0) &&
					((refreshRate > 0 && Math.Abs(refreshRate - test.RefreshRate) < 1.0) || refreshRate == 0);
			});
		}

		/// <summary>
		/// Returns a System.String representing this Displays.
		/// </summary>
		/// <returns>A System.String representing this Displays.</returns>
		public override string ToString() {
			return string.Format("{0}: {1} ({2} modes available)", IsPrimary ? "Primary" : "Secondary", currentResolution.ToString(), availableResolutions.Count);
		}

		/// <summary>Determines whether the specified DisplayDevices are equal.</summary>
		/// <param name="obj">The System.Object to check against.</param>
		/// <returns>True if the System.Object is an equal Displays; false otherwise.</returns>
		public override bool Equals(object obj) {
			Display dev = obj as Display;
			return dev == null ? false : IsPrimary == dev.IsPrimary && currentResolution == dev.currentResolution && availableResolutions.Count == dev.availableResolutions.Count;
		}

		/// <summary>Returns a unique hash representing this Display.</summary>
		/// <returns>A System.Int32 that may serve as a hash code for this Display.</returns>
		public override int GetHashCode() {
			return currentResolution.GetHashCode() ^ (IsPrimary ? 1 : 0);
		}
	}
}