using System.Runtime.CompilerServices;

namespace System.Graphics {
	/// <summary>Defines the format of color representation.</summary>
	/// <remarks>
	/// <para>A ColorFormat contains Red, Green, Blue and Alpha components that descibe
	/// the allocated bits per pixel for the corresponding color.</para>
	/// </remarks>
	[Serializable]
	public struct ColorFormat : IEquatable<ColorFormat>, IComparable<ColorFormat> {
		/// <summary>
		/// Gets an empty color format.
		/// </summary>
		public static readonly ColorFormat Empty = new ColorFormat();
		/// <summary>
		/// Gets an 8-bit color format.
		/// </summary>
		public static readonly ColorFormat Bit8 = new ColorFormat(8);
		/// <summary>
		/// Gets a 16-bit color format.
		/// </summary>
		public static readonly ColorFormat Bit16 = new ColorFormat(16);
		/// <summary>
		/// Gets a 24-bit color format.
		/// </summary>
		public static readonly ColorFormat Bit24 = new ColorFormat(24);
		/// <summary>
		/// Gets a 32-bit color format.
		/// </summary>
		public static readonly ColorFormat Bit32 = new ColorFormat(32);

		/// <summary>
		/// The amount of bits assigned to the red channel per pixel.
		/// </summary>
		public readonly byte Red;
		/// <summary>
		/// The amount of bits assigned to the green channel per pixel.
		/// </summary>
		public readonly byte Green;
		/// <summary>
		/// The amount of bits assigned to the blue channel per pixel.
		/// </summary>
		public readonly byte Blue;
		/// <summary>
		/// The amount of bits assigned to the alpha channel per pixel.
		/// </summary>
		public readonly byte Alpha;
		/// <summary>
		/// The total bits assigned per pixel.
		/// </summary>
		public readonly int BitsPerPixel;
		/// <summary>
		/// Whether the ColorFormat represents an indexed format.
		/// </summary>
		public readonly bool IsIndexed;

		/// <summary>
		/// Constructs a new ColorFormat with the specified aggregate bits per pixel.
		/// </summary>
		/// <param name="bpp">The bits per pixel sum for the Red, Green, Blue and Alpha color channels.</param>
		public ColorFormat(int bpp) {
			if (bpp < 0)
				throw new ArgumentOutOfRangeException(nameof(bpp), "Must be greater or equal to zero.");
			IsIndexed = false;
			BitsPerPixel = bpp;
			switch (bpp) {
				case 32:
					Red = Green = Blue = Alpha = 8;
					break;
				case 24:
					Red = Green = Blue = 8;
					Alpha = 0;
					break;
				case 16:
					Red = Blue = 5;
					Green = 6;
					Alpha = 0;
					break;
				case 15:
					Red = Green = Blue = 5;
					Alpha = 0;
					break;
				case 8:
					Red = Green = 3;
					Blue = 2;
					Alpha = 0;
					IsIndexed = true;
					break;
				case 4:
					Red = Green = 2;
					Blue = 1;
					Alpha = 0;
					IsIndexed = true;
					break;
				case 1:
					IsIndexed = true;
					Red = Green = Blue = Alpha = 0;
					break;
				default:
					Red = Blue = Alpha = (byte) (bpp / 4);
					Green = (byte) ((bpp / 4) + (bpp % 4));
					break;
			}
		}

		/// <summary>
		/// Constructs a new ColorFormat with the specified bits per pixel for 
		/// the Red, Green, Blue and Alpha color channels.
		/// </summary>
		/// <param name="red">Bits per pixel for the Red color channel.</param>
		/// <param name="green">Bits per pixel for the Green color channel.</param>
		/// <param name="blue">Bits per pixel for the Blue color channel.</param>
		/// <param name="alpha">Bits per pixel for the Alpha color channel.</param>
		public ColorFormat(int red, int green, int blue, int alpha) {
			if (red < 0 || green < 0 || blue < 0 || alpha < 0)
				throw new ArgumentOutOfRangeException("Arguments must be greater or equal to zero.", null as Exception);
			Red = (byte) red;
			Green = (byte) green;
			Blue = (byte) blue;
			Alpha = (byte) alpha;
			BitsPerPixel = red + green + blue + alpha;
			IsIndexed = BitsPerPixel < 15 && BitsPerPixel != 0;
		}

		/// <summary>
		/// Compares two instances.
		/// </summary>
		/// <param name="other">The other instance.</param>
		/// <returns>
		/// Zero if this instance is equal to other;
		/// a positive value  if this instance is greater than other;
		/// a negative value otherwise.
		/// </returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public int CompareTo(ColorFormat other) {
			int result = BitsPerPixel.CompareTo(other.BitsPerPixel);
			if (result != 0)
				return result;
			result = IsIndexed.CompareTo(other.IsIndexed);
			return result;
		}

		/// <summary>
		/// Indicates whether this instance and a specified object are equal.
		/// </summary>
		/// <param name="obj">Another object to compare to.</param>
		/// <returns>True if this instance is equal to obj; false otherwise.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool Equals(ColorFormat obj) {
			return Red == obj.Red && Green == obj.Green && Blue == obj.Blue && Alpha == obj.Alpha;
		}

		/// <summary>
		/// Indicates whether this instance and a specified object are equal.
		/// </summary>
		/// <param name="obj">Another object to compare to.</param>
		/// <returns>True if this instance is equal to obj; false otherwise.</returns>
		public override bool Equals(object obj) {
			return obj is ColorFormat ? Equals((ColorFormat) obj) : false;
		}

		/// <summary>
		/// Compares two instances for equality.
		/// </summary>
		/// <param name="left">The left operand.</param>
		/// <param name="right">The right operand.</param>
		/// <returns>True if both instances are equal; false otherwise.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator ==(ColorFormat left, ColorFormat right) {
			return left.Equals(right);
		}

		/// <summary>
		/// Compares two instances for inequality.
		/// </summary>
		/// <param name="left">The left operand.</param>
		/// <param name="right">The right operand.</param>
		/// <returns>True if both instances are not equal; false otherwise.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator !=(ColorFormat left, ColorFormat right) {
			return !left.Equals(right);
		}

		/// <summary>
		/// Compares two instances for inequality.
		/// </summary>
		/// <param name="left">The left operand.</param>
		/// <param name="right">The right operand.</param>
		/// <returns>True if left is greater than right; false otherwise.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator >(ColorFormat left, ColorFormat right) {
			return left.CompareTo(right) > 0;
		}

		/// <summary>
		/// Compares two instances for inequality.
		/// </summary>
		/// <param name="left">The left operand.</param>
		/// <param name="right">The right operand.</param>
		/// <returns>True if left is greater than or equal to right; false otherwise.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator >=(ColorFormat left, ColorFormat right) {
			return left.CompareTo(right) >= 0;
		}

		/// <summary>
		/// Compares two instances for inequality.
		/// </summary>
		/// <param name="left">The left operand.</param>
		/// <param name="right">The right operand.</param>
		/// <returns>True if left is less than right; false otherwise.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator <(ColorFormat left, ColorFormat right) {
			return left.CompareTo(right) < 0;
		}

		/// <summary>
		/// Compares two instances for inequality.
		/// </summary>
		/// <param name="left">The left operand.</param>
		/// <param name="right">The right operand.</param>
		/// <returns>True if left is less than or equal to right; false otherwise.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator <=(ColorFormat left, ColorFormat right) {
			return left.CompareTo(right) <= 0;
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>A System.Int32 with the hash code of this instance.</returns>
		public override int GetHashCode() {
			return unchecked((Red << 24) ^ (Green << 16) ^ (Blue << 8) ^ Alpha);
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that describes this instance.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that describes this instance.</returns>
		public override string ToString() {
			return string.Format("{0} ({1})", BitsPerPixel, (IsIndexed ? " indexed" : Red.ToStringLookup() + Green.ToStringLookup() + Blue.ToStringLookup() + Alpha.ToStringLookup()));
		}
	}
}