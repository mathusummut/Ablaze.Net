using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Runtime.Serialization;

namespace System {
	/// <summary>
	/// Represents a half-precision floating-point number. It occupies only 16 bits, which are split into 1 Sign bit, 5 Exponent bits and 10 Mantissa bits.
	/// </summary>
	[Serializable, StructLayout(LayoutKind.Sequential)]
	public struct Half : ISerializable, IComparable<Half>, IFormattable, IEquatable<Half> {
		/// <summary>
		/// The size in bytes for an instance of the Half struct.
		/// </summary>
		public const int SizeInBytes = 2;
		/// <summary>
		/// The type of Half.
		/// </summary>
		public static readonly Type Type = typeof(Half);
		/// <summary>
		/// A value that represents 0.
		/// </summary>
		public static readonly Half Zero = new Half();
		/// <summary>
		/// A value that represents 1.
		/// </summary>
		public static readonly Half One = new Half(1f);
		/// <summary>
		/// A value that represents -1.
		/// </summary>
		public static readonly Half MinusOne = new Half(-1f);
		/// <summary>Smallest positive half</summary>
		public const float MinValue = 5.96046448e-08f;
		/// <summary>Smallest positive normalized half</summary>
		public const float MinNormalizedValue = 6.10351562e-05f;
		/// <summary>Largest positive half</summary>
		public const float MaxValue = 65504f;
		/// <summary>Smallest positive e for which half (1.0 + e) != half (1.0)</summary>
		public const float Epsilon = 0.00097656f;
		/// <summary>
		/// Gets or sets the bits used to represent this instance.
		/// </summary>
		[CLSCompliant(false)]
		public ushort Bits;

		/// <summary>Returns true if the Half is zero.</summary>
		public bool IsZero {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return (Bits == 0) || (Bits == 0x8000);
			}
		}

		/// <summary>Returns true if the Half represents Not A Number (NaN)</summary>
		public bool IsNaN {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return (((Bits & 0x7C00) == 0x7C00) && (Bits & 0x03FF) != 0x0000);
			}
		}

		/// <summary>Returns true if the Half represents positive infinity.</summary>
		public bool IsPositiveInfinity {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return (Bits == 31744);
			}
		}

		/// <summary>Returns true if the Half represents negative infinity.</summary>
		public bool IsNegativeInfinity {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return (Bits == 64512);
			}
		}

		/// <summary>
		/// The new Half instance will convert the parameter into 16-bit half-precision floating-point.
		/// </summary>
		/// <param name="f">32-bit single-precision floating-point number.</param>
		public Half(float f) {
			unsafe
			{
				int si32 = *(int*) &f;
				// Our floating point number, F, is represented by the bit pattern in integer i.
				// Disassemble that bit pattern into the sign, S, the exponent, E, and the significand, M.
				// Shift S into the position where it will go in in the resulting half number.
				// Adjust E, accounting for the different exponent bias of float and half (127 versus 15).
				int sign = (si32 >> 16) & 0x00008000;
				int exponent = ((si32 >> 23) & 0x000000ff) - (127 - 15);
				int mantissa = si32 & 0x007fffff;
				// Now reassemble S, E and M into a half:
				if (exponent <= 0) {
					if (exponent < -10) {
						// E is less than -10. The absolute value of F is less than Half.MinValue
						// (F may be a small normalized float, a denormalized float or a zero).
						// We convert F to a half zero with the same sign as F.
						Bits = (ushort) sign;
					} else {
						// E is between -10 and 0. F is a normalized float whose magnitude is less than Half.MinNormalizedValue.
						// We convert F to a denormalized half.
						// Add an explicit leading 1 to the significand.
						mantissa = mantissa | 0x00800000;
						// Round to M to the nearest (10+E)-bit value (with E between -10 and 0); in case of a tie, round to the nearest even value.
						//
						// Rounding may cause the significand to overflow and make our number normalized. Because of the way a half's bits
						// are laid out, we don't have to treat this case separately; the code below will handle it correctly.
						int t = 14 - exponent;
						int a = (1 << (t - 1)) - 1;
						int b = (mantissa >> t) & 1;
						mantissa = (mantissa + a + b) >> t;
						// Assemble the half from S, E (==zero) and M.
						Bits = (ushort) (sign | mantissa);
					}
				} else if (exponent == 0xff - (127 - 15)) {
					if (mantissa == 0) {
						// F is an infinity; convert F to a half infinity with the same sign as F.
						Bits = (ushort) (sign | 0x7c00);
					} else {
						// F is a NAN; we produce a half NAN that preserves the sign bit and the 10 leftmost bits of the
						// significand of F, with one exception: If the 10 leftmost bits are all zero, the NAN would turn 
						// into an infinity, so we have to set at least one bit in the significand.
						mantissa >>= 13;
						Bits = (ushort) (sign | 0x7c00 | mantissa | ((mantissa == 0) ? 1 : 0));
					}
				} else {
					// E is greater than zero.  F is a normalized float. We try to convert F to a normalized half.
					// Round to M to the nearest 10-bit value. In case of a tie, round to the nearest even value.
					mantissa = mantissa + 0x00000fff + ((mantissa >> 13) & 1);
					if ((mantissa & 0x00800000) == 1) {
						mantissa = 0;        // overflow in significand,
						exponent += 1;       // adjust exponent
					}
					// exponent overflow
					//if (exponent > 30)
					//	throw new ArithmeticException("Half: Hardware floating-point overflow.");
					// Assemble the half from S, E and M.
					Bits = (ushort) (sign | (exponent << 10) | (mantissa >> 13));
				}
			}
		}

		/// <summary>
		/// The new Half instance will convert the parameter into 16-bit half-precision floating-point.
		/// </summary>
		/// <param name="d">64-bit double-precision floating-point number.</param>
		public Half(double d) : this((float) d) {
		}

		/// <summary>Converts the 16-bit half to 32-bit floating-point.</summary>
		/// <returns>A single-precision floating-point number.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public float ToSingle() {
			int i = HalfToFloat(Bits);
			unsafe
			{
				return *(float*) &i;
			}
		}

		/// <summary>Ported from OpenEXR's IlmBase 1.0.1</summary>
		private static int HalfToFloat(ushort ui16) {
			int sign = (ui16 >> 15) & 0x00000001;
			int exponent = (ui16 >> 10) & 0x0000001f;
			int mantissa = ui16 & 0x000003ff;
			if (exponent == 0) {
				if (mantissa == 0) // Plus or minus zero
					return sign << 31;
				else {
					// Denormalized number -- renormalize it
					while ((mantissa & 0x00000400) == 0) {
						mantissa <<= 1;
						exponent -= 1;
					}
					exponent += 1;
					mantissa &= ~0x00000400;
				}
			} else if (exponent == 31) {
				if (mantissa == 0) // Positive or negative infinity
					return (sign << 31) | 0x7f800000;
				else // Nan -- preserve sign and significand bits
					return (sign << 31) | 0x7f800000 | (mantissa << 13);
			}
			// Normalized number
			exponent = exponent + (127 - 15);
			mantissa = mantissa << 13;
			// Assemble S, E and M.
			return (sign << 31) | (exponent << 23) | mantissa;
		}

		/// <summary>
		/// Converts a System.Single to a System.Half.
		/// </summary>
		/// <param name="f">The value to convert.
		/// A <see cref="System.Single"/>
		/// </param>
		/// <returns>The result of the conversion.
		/// A <see cref="Half"/>
		/// </returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static explicit operator Half(float f) {
			return new Half(f);
		}

		/// <summary>
		/// Converts a System.Double to a System.Half.
		/// </summary>
		/// <param name="d">The value to convert.
		/// A <see cref="System.Double"/>
		/// </param>
		/// <returns>The result of the conversion.
		/// A <see cref="Half"/>
		/// </returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static explicit operator Half(double d) {
			return new Half(d);
		}

		/// <summary>
		/// Converts a System.Half to a System.Single.
		/// </summary>
		/// <param name="h">The value to convert.
		/// A <see cref="Half"/>
		/// </param>
		/// <returns>The result of the conversion.
		/// A <see cref="System.Single"/>
		/// </returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static implicit operator float(Half h) {
			return h.ToSingle();
		}

		/// <summary>
		/// Converts a System.Half to a System.Double.
		/// </summary>
		/// <param name="h">The value to convert.
		/// A <see cref="Half"/>
		/// </param>
		/// <returns>The result of the conversion.
		/// A <see cref="System.Double"/>
		/// </returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static implicit operator double(Half h) {
			return h.ToSingle();
		}

		/// <summary>
		/// Compares the two values and return true if equal.
		/// </summary>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator ==(Half left, Half right) {
			return left.Equals(right);
		}

		/// <summary>
		/// Compares the two values and return true if not equal.
		/// </summary>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator !=(Half left, Half right) {
			return !left.Equals(right);
		}

		/// <summary>
		/// Compares the two values and return true if left is smaller.
		/// </summary>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator <(Half left, float right) {
			return ((float) left) < ((float) right);
		}

		/// <summary>
		/// Compares the two values and return true if left is smaller.
		/// </summary>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator >(Half left, float right) {
			return ((float) left) > ((float) right);
		}

		/// <summary>
		/// Compares the two values and return true if left is smaller.
		/// </summary>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator <(float left, Half right) {
			return ((float) left) < ((float) right);
		}

		/// <summary>
		/// Compares the two values and return true if left is smaller.
		/// </summary>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator >(float left, Half right) {
			return ((float) left) > ((float) right);
		}

		/// <summary>
		/// Compares the two values and return true if left is smaller.
		/// </summary>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator <(Half left, Half right) {
			return ((float) left) < ((float) right);
		}

		/// <summary>
		/// Compares the two values and return true if left is smaller.
		/// </summary>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator >(Half left, Half right) {
			return ((float) left) > ((float) right);
		}

		/// <summary>
		/// Compares the two values and return true if left is smaller or equal.
		/// </summary>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator <=(Half left, float right) {
			return ((float) left) <= ((float) right);
		}

		/// <summary>
		/// Compares the two value and return true if left is greater or equal.
		/// </summary>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator >=(Half left, float right) {
			return ((float) left) >= ((float) right);
		}

		/// <summary>
		/// Compares the two values and return true if left is smaller or equal.
		/// </summary>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator <=(float left, Half right) {
			return ((float) left) <= ((float) right);
		}

		/// <summary>
		/// Compares the two value and return true if left is greater or equal.
		/// </summary>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator >=(float left, Half right) {
			return ((float) left) >= ((float) right);
		}

		/// <summary>
		/// Compares the two values and return true if left is smaller or equal.
		/// </summary>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator <=(Half left, Half right) {
			return ((float) left) <= ((float) right);
		}

		/// <summary>
		/// Compares the two value and return true if left is greater or equal.
		/// </summary>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator >=(Half left, Half right) {
			return ((float) left) >= ((float) right);
		}

		/// <summary>
		/// Does nothing lol
		/// </summary>
		/// <param name="value">lol why</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Half operator +(Half value) {
			return value;
		}

		/// <summary>
		/// Negates the specified value.
		/// </summary>
		/// <param name="value">The value to negate.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Half operator -(Half value) {
			return new Half(-(float) value);
		}

		/// <summary>
		/// Adds the two values.
		/// </summary>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Half operator +(Half left, float right) {
			return (Half) (((float) left) + ((float) right));
		}

		/// <summary>
		/// Adds the two values.
		/// </summary>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Half operator +(float left, Half right) {
			return (Half) (((float) left) + ((float) right));
		}

		/// <summary>
		/// Adds the two values.
		/// </summary>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Half operator +(Half left, Half right) {
			return (Half) (((float) left) + ((float) right));
		}

		/// <summary>
		/// Subtracts the second value from the first value.
		/// </summary>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Half operator -(float left, Half right) {
			return (Half) (((float) left) - ((float) right));
		}

		/// <summary>
		/// Subtracts the second value from the first value.
		/// </summary>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Half operator -(Half left, float right) {
			return (Half) (((float) left) - ((float) right));
		}

		/// <summary>
		/// Subtracts the second value from the first value.
		/// </summary>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Half operator -(Half left, Half right) {
			return (Half) (((float) left) - ((float) right));
		}

		/// <summary>
		/// Multiplies the two value.
		/// </summary>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Half operator *(Half left, Half right) {
			return (Half) (((float) left) * ((float) right));
		}

		/// <summary>
		/// Multiplies the two value.
		/// </summary>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Half operator *(Half left, float right) {
			return (Half) (((float) left) * ((float) right));
		}

		/// <summary>
		/// Multiplies the two value.
		/// </summary>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Half operator *(float left, Half right) {
			return (Half) (((float) left) * ((float) right));
		}

		/// <summary>
		/// Divides the second value from the first half.
		/// </summary>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Half operator /(Half left, Half right) {
			return (Half) (((float) left) / ((float) right));
		}

		/// <summary>
		/// Divides the second value from the first half.
		/// </summary>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Half operator /(Half left, float right) {
			return (Half) (((float) left) / ((float) right));
		}

		/// <summary>
		/// Divides the second value from the first half.
		/// </summary>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Half operator /(float left, Half right) {
			return (Half) (((float) left) / ((float) right));
		}

		/// <summary>Constructor used by ISerializable to deserialize the object.</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		private Half(SerializationInfo info, StreamingContext context) {
			Bits = (ushort) info.GetValue(nameof(Bits), typeof(ushort));
		}

		/// <summary>Used by ISerialize to serialize the object.</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public void GetObjectData(SerializationInfo info, StreamingContext context) {
			info.AddValue(nameof(Bits), Bits);
		}

		/// <summary>Updates the Half by reading from a Stream.</summary>
		/// <param name="bin">A BinaryReader instance associated with an open Stream.</param>
		public void FromBinaryStream(BinaryReader bin) {
			Bits = bin.ReadUInt16();
		}

		/// <summary>Writes the Half into a Stream.</summary>
		/// <param name="bin">A BinaryWriter instance associated with an open Stream.</param>
		public void ToBinaryStream(BinaryWriter bin) {
			bin.Write(Bits);
		}

		/// <summary>
		/// Returns a value indicating whether this instance is equal to a specified System.Half value.
		/// </summary>
		/// <param name="other">System.Half object to compare to this instance..</param>
		/// <returns>True, if other is equal to this instance; false otherwise.</returns>
		public bool Equals(Half other) {
			short aInt, bInt;
			unchecked {
				aInt = (short) other.Bits;
				bInt = (short) Bits;
			}
			// Make aInt lexicographically ordered as a twos-complement int
			if (aInt < 0)
				aInt = (short) (0x8000 - aInt);
			// Make bInt lexicographically ordered as a twos-complement int
			if (bInt < 0)
				bInt = (short) (0x8000 - bInt);
			return Math.Abs((short) (aInt - bInt)) <= 1;
		}

		/// <summary>
		/// Returns whether the specified object is equal to this one.
		/// </summary>
		/// <param name="obj">The object to compare against.</param>
		public override bool Equals(object obj) {
			return obj is Half && Equals((Half) obj);
		}

		/// <summary>
		/// Gets the hash code of this instance.
		/// </summary>
		public override int GetHashCode() {
			return Bits;
		}

		/// <summary>
		/// Compares this instance to a specified half-precision floating-point number
		/// and returns an integer that indicates whether the value of this instance
		/// is less than, equal to, or greater than the value of the specified half-precision
		/// floating-point number. 
		/// </summary>
		/// <param name="other">A half-precision floating-point number to compare.</param>
		/// <returns>
		/// A signed number indicating the relative values of this instance and value. If the number is:
		/// <para>Less than zero, then this instance is less than other, or this instance is not a number
		/// (System.Half.NaN) and other is a number.</para>
		/// <para>Zero: this instance is equal to value, or both this instance and other
		/// are not a number (System.Half.NaN), System.Half.PositiveInfinity, or
		/// System.Half.NegativeInfinity.</para>
		/// <para>Greater than zero: this instance is greater than othrs, or this instance is a number
		/// and other is not a number (System.Half.NaN).</para>
		/// </returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public int CompareTo(float other) {
			return ((float) this).CompareTo(other);
		}

		/// <summary>
		/// Compares this instance to a specified half-precision floating-point number
		/// and returns an integer that indicates whether the value of this instance
		/// is less than, equal to, or greater than the value of the specified half-precision
		/// floating-point number. 
		/// </summary>
		/// <param name="other">A half-precision floating-point number to compare.</param>
		/// <returns>
		/// A signed number indicating the relative values of this instance and value. If the number is:
		/// <para>Less than zero, then this instance is less than other, or this instance is not a number
		/// (System.Half.NaN) and other is a number.</para>
		/// <para>Zero: this instance is equal to value, or both this instance and other
		/// are not a number (System.Half.NaN), System.Half.PositiveInfinity, or
		/// System.Half.NegativeInfinity.</para>
		/// <para>Greater than zero: this instance is greater than othrs, or this instance is a number
		/// and other is not a number (System.Half.NaN).</para>
		/// </returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public int CompareTo(Half other) {
			return ((float) this).CompareTo(other);
		}

		/// <summary>Converts this Half into a human-legible string representation.</summary>
		/// <returns>The string representation of this instance.</returns>
		public override string ToString() {
			return ToSingle().ToString();
		}

		/// <summary>Converts this Half into a human-legible string representation.</summary>
		/// <param name="format">Formatting for the output string.</param>
		/// <param name="formatProvider">Culture-specific formatting information.</param>
		/// <returns>The string representation of this instance.</returns>
		public string ToString(string format, IFormatProvider formatProvider) {
			return ToSingle().ToString(format, formatProvider);
		}

		/// <summary>Converts the string representation of a number to a half-precision floating-point equivalent.</summary>
		/// <param name="s">String representation of the number to convert.</param>
		/// <returns>A new Half instance.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Half Parse(string s) {
			return (Half) float.Parse(s);
		}

		/// <summary>Converts the string representation of a number to a half-precision floating-point equivalent.</summary>
		/// <param name="s">String representation of the number to convert.</param>
		/// <param name="style">Specifies the format of s.</param>
		/// <param name="provider">Culture-specific formatting information.</param>
		/// <returns>A new Half instance.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Half Parse(string s, NumberStyles style, IFormatProvider provider) {
			return (Half) float.Parse(s, style, provider);
		}

		/// <summary>Converts the string representation of a number to a half-precision floating-point equivalent. Returns success.</summary>
		/// <param name="s">String representation of the number to convert.</param>
		/// <param name="result">The Half instance to write to.</param>
		/// <returns>Success.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool TryParse(string s, out Half result) {
			float f;
			bool b = float.TryParse(s, out f);
			result = (Half) f;
			return b;
		}

		/// <summary>Converts the string representation of a number to a half-precision floating-point equivalent. Returns success.</summary>
		/// <param name="s">String representation of the number to convert.</param>
		/// <param name="style">Specifies the format of s.</param>
		/// <param name="provider">Culture-specific formatting information.</param>
		/// <param name="result">The Half instance to write to.</param>
		/// <returns>Success.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out Half result) {
			float f;
			bool b = float.TryParse(s, style, provider, out f);
			result = (Half) f;
			return b;
		}

		/// <summary>Returns the Half as an array of bytes.</summary>
		/// <param name="h">The Half to convert.</param>
		/// <returns>The input as byte array.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static byte[] GetBytes(Half h) {
			return BitConverter.GetBytes(h.Bits);
		}

		/// <summary>Converts an array of bytes into Half.</summary>
		/// <param name="value">A Half in it's byte[] representation.</param>
		/// <param name="startIndex">The starting position within value.</param>
		/// <returns>A new Half instance.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Half FromBytes(byte[] value, int startIndex) {
			Half h;
			h.Bits = BitConverter.ToUInt16(value, startIndex);
			return h;
		}
	}
}