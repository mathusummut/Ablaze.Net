using System.Drawing;
using System.Runtime.CompilerServices;

namespace System {
	/// <summary>Contains information regarding a monitor's display resolution.</summary>
	[Serializable]
	public struct Resolution : IEquatable<Resolution> {
		/// <summary>
		/// An empty resolution.
		/// </summary>
		public static readonly Resolution Empty = new Resolution();
		/// <summary>
		/// Gets the System.Drawing.Rectangle containing the bounds of the display in pixels.
		/// </summary>
		public Rectangle Bounds;
		/// <summary>
		/// Gets the colour depth of the display.
		/// </summary>
		public int BitsPerPixel;
		/// <summary>
		/// Gets the refresh rate of the display in Hertz.
		/// </summary>
		public double RefreshRate;

		/// <summary>
		/// Gets or sets the X-coordinate of the origin of the display in pixels.
		/// </summary>
		public int X {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return Bounds.X;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				Bounds.X = value;
			}
		}

		/// <summary>
		/// Gets or sets the Y-coordinate of the origin of the display in pixels.
		/// </summary>
		public int Y {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return Bounds.Y;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				Bounds.Y = value;
			}
		}

		/// <summary>
		/// Gets or sets the width of the display in pixels.
		/// </summary>
		public int Width {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return Bounds.Width;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				Bounds.Width = value;
			}
		}

		/// <summary>
		/// Gets or sets the height of the display in pixels.
		/// </summary>
		public int Height {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return Bounds.Height;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				Bounds.Height = value;
			}
		}

		/// <summary>
		/// Initializes a new resolution structure.
		/// </summary>
		/// <param name="x">The relative X-coordinate of the origin of the display.</param>
		/// <param name="y">The relative Y-coordinate of the origin of the display.</param>
		/// <param name="width">The width of the display in pixels.</param>
		/// <param name="height">The height of the display in pixels.</param>
		/// <param name="bitsPerPixel">The bit-depth of each pixel.</param>
		/// <param name="refreshRate">The number of frames displayes each second.</param>
		public Resolution(int x, int y, int width, int height, int bitsPerPixel, double refreshRate) : this() {
			BitsPerPixel = bitsPerPixel;
			RefreshRate = refreshRate;
			Bounds = new Rectangle(x, y, width, height);
		}

		/// <summary>
		/// Returns a System.String representing this Display.
		/// </summary>
		/// <returns>A System.String representing this Display.</returns>
		public override string ToString() {
			return string.Format("{0}x{1}b@{2}Hz", Bounds, BitsPerPixel, RefreshRate);
		}

		/// <summary>Determines whether the specified resolutions are equal.</summary>
		/// <param name="res">The resolution to check against.</param>
		public bool Equals(Resolution res) {
			return Bounds.Width == res.Bounds.Width && res.Bounds.Height == res.Height && BitsPerPixel == res.BitsPerPixel && RefreshRate == res.RefreshRate;
		}

		/// <summary>Determines whether the specified resolutions are equal.</summary>
		/// <param name="obj">The System.Object to check against.</param>
		/// <returns>True if the System.Object is an equal Display; otherwise false.</returns>
		public override bool Equals(object obj) {
			return obj is Resolution ? Equals(obj) : false;
		}

		/// <summary>Returns a unique hash representing this resolution.</summary>
		/// <returns>A System.Int32 that may serve as a hash code for this resolution.</returns>
		public override int GetHashCode() {
			return unchecked((Bounds.GetHashCode() << 17) ^ (BitsPerPixel << 13) ^ RefreshRate.GetHashCode());
		}

		/// <summary>
		/// Compares two instances for equality.
		/// </summary>
		/// <param name="left">The first instance.</param>
		/// <param name="right">The second instance.</param>
		/// <returns>True, if left equals right; false otherwise.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator ==(Resolution left, Resolution right) {
			return left.Equals(right);
		}

		/// <summary>
		/// Compares two instances for inequality.
		/// </summary>
		/// <param name="left">The first instance.</param>
		/// <param name="right">The second instance.</param>
		/// <returns>True, if left does not equal right; false otherwise.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator !=(Resolution left, Resolution right) {
			return !left.Equals(right);
		}
	}
}