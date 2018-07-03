using System.Runtime.CompilerServices;

namespace System.Numerics {
	/// <summary>
	/// Represents a complex number.
	/// </summary>
	[Serializable]
	public struct ComplexF : IEquatable<ComplexF>, IFormattable {
		/// <summary>
		/// The real component.
		/// </summary>
		public float Real;
		/// <summary>
		/// The imaginary component.
		/// </summary>
		public float Imaginary;

		/// <summary>
		/// Returns a new ComplexF instance with a real number equal to zero and an imaginary number equal to zero.
		/// </summary>
		public static readonly ComplexF Zero = new ComplexF();
		/// <summary>
		/// Returns a new ComplexF instance with a real number equal to one and an imaginary number equal to zero.
		/// </summary>
		public static readonly ComplexF One = new ComplexF(1f, 0f);
		/// <summary>
		/// Returns a new ComplexF instance with a real number equal to two and an imaginary number equal to zero.
		/// </summary>
		public static readonly ComplexF Two = new ComplexF(2f, 0f);
		/// <summary>
		/// Returns a new ComplexF instance with a real number equal to zero and an imaginary number equal to one.
		/// </summary>
		public static readonly ComplexF ImaginaryOne = new ComplexF(0f, 1f);
		/// <summary>
		/// Returns a new ComplexF instance with a real number equal to zero and an imaginary number equal to one.
		/// </summary>
		public static readonly ComplexF OnePlusi = new ComplexF(1f, 1f);

		/// <summary>
		/// Gets the magnitude (or absolute value) of a complex number in decibels.
		/// </summary>
		public float MagnitudeDB {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return (float) (20.0 * Math.Log10(Math.Sqrt(Real * Real + Imaginary * Imaginary)));
			}
		}

		/// <summary>
		/// Gets the magnitude (or absolute value) of a complex number.
		/// </summary>
		public float Magnitude {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return (float) Math.Sqrt(Real * Real + Imaginary * Imaginary);
			}
		}

		/// <summary>
		/// Gets the squared magnitude (or absolute value) of a complex number.
		/// </summary>
		public float MagnitudeSquared {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return Real * Real + Imaginary * Imaginary;
			}
		}

		/// <summary>
		/// Gets the phase of a complex number in radians.
		/// </summary>
		public float Phase {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return (float) Math.Atan2(Imaginary, Real);
			}
		}

		/// <summary>
		/// Initializes a new instance of the ComplexF structure using the specified real value.
		/// </summary>
		/// <param name="real">The real part of the complex number.</param>
		public ComplexF(float real) {
			Real = real;
			Imaginary = 0f;
		}

		/// <summary>
		/// Initializes a new instance of the ComplexF structure using the specified real and imaginary values.
		/// </summary>
		/// <param name="real">The real part of the complex number.</param>
		/// <param name="imaginary">The imaginary part of the complex number.</param>
		public ComplexF(float real, float imaginary) {
			Real = real;
			Imaginary = imaginary;
		}

		/// <summary>
		/// Returns the angle that is the arc cosine of the specified complex number.
		/// </summary>
		/// <param name="value">A complex number.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static ComplexF Acos(ComplexF value) {
			return -ImaginaryOne * Log(value + (ImaginaryOne * Sqrt(One - (value * value))));
		}

		/// <summary>
		/// Adds two complex numbers and returns the result.
		/// </summary>
		/// <param name="left">The first complex number to add.</param>
		/// <param name="right">The second complex number to add.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static ComplexF Add(ComplexF left, ComplexF right) {
			return left + right;
		}

		/// <summary>
		/// Returns the angle that is the arc sine of the specified complex number.
		/// </summary>
		/// <param name="value">A complex number.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static ComplexF Asin(ComplexF value) {
			return -ImaginaryOne * Log((ImaginaryOne * value) + Sqrt(One - (value * value)));
		}

		/// <summary>
		/// Returns the angle that is the arc tangent of the specified complex number.
		/// </summary>
		/// <param name="value">A complex number.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static ComplexF Atan(ComplexF value) {
			return (ImaginaryOne / Two) * (Log(One - (ImaginaryOne * value)) - Log(One + (ImaginaryOne * value)));
		}

		/// <summary>
		/// Computes the conjugate of a complex number and returns the result.
		/// </summary>
		/// <param name="value">A complex number.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static ComplexF Conjugate(ComplexF value) {
			return new ComplexF(value.Real, -value.Imaginary);
		}

		/// <summary>
		/// Returns the cosine of the specified complex number.
		/// </summary>
		/// <param name="value">A complex number.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static ComplexF Cos(ComplexF value) {
			double mReal = value.Real;
			double mImaginary = value.Imaginary;
			return new ComplexF((float) (Math.Cos(mReal) * Math.Cosh(mImaginary)), (float) -(Math.Sin(mReal) * Math.Sinh(mImaginary)));
		}

		/// <summary>
		/// Returns the hyperbolic cosine of the specified complex number.
		/// </summary>
		/// <param name="value">A complex number.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static ComplexF Cosh(ComplexF value) {
			double mReal = value.Real;
			double mImaginary = value.Imaginary;
			return new ComplexF((float) (Math.Cosh(mReal) * Math.Cos(mImaginary)), (float) (Math.Sinh(mReal) * Math.Sin(mImaginary)));
		}

		/// <summary>
		/// Divides one complex number by another and returns the result.
		/// </summary>
		/// <param name="dividend">The complex number to be divided.</param>
		/// <param name="divisor">The complex number to divide by.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static ComplexF Divide(ComplexF dividend, ComplexF divisor) {
			return dividend / divisor;
		}

		/// <summary>
		/// Returns a value that indicates whether the current instance and a specified object have the same value.
		/// </summary>
		/// <param name="obj">The object to compare.</param>
		public override bool Equals(object obj) {
			return obj is ComplexF ? Equals((ComplexF) obj) : false;
		}

		/// <summary>
		/// Returns a value that indicates whether the current instance and a specified complex number have the same value.
		/// </summary>
		/// <param name="value">The complex number to compare.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool Equals(ComplexF value) {
			return Real == value.Real && Imaginary == value.Imaginary;
		}

		/// <summary>
		/// Returns e raised to the power specified by a complex number.
		/// </summary>
		/// <param name="value">A complex number that specifies a power.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static ComplexF Exp(ComplexF value) {
			double num = Math.Exp(value.Real);
			double imag = value.Imaginary;
			return new ComplexF((float) (num * Math.Cos(imag)), (float) (num * Math.Sin(imag)));
		}

		/// <summary
		/// >Creates a complex number from a point's polar coordinates.
		/// </summary>
		/// <param name="magnitude">The magnitude, which is the distance from the origin (the intersection of the x-axis and the y-axis) to the number.</param>
		/// <param name="phase">The phase, which is the angle from the line to the horizontal axis, measured in radians.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static ComplexF FromPolarCoordinates(float magnitude, float phase) {
			return new ComplexF(magnitude * (float) Math.Cos(phase), magnitude * (float) Math.Sin(phase));
		}

		/// <summary>
		/// Returns the hash code for the current ComplexF object.
		/// </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode() {
			return unchecked(Imaginary.GetHashCode() << 16 + Real.GetHashCode());
		}

		/// <summary>
		/// Returns the natural (base e) logarithm of a specified complex number.
		/// </summary>
		/// <param name="value">A complex number.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static ComplexF Log(ComplexF value) {
			return new ComplexF((float) Math.Log(value.Magnitude), (float) Math.Atan2(value.Imaginary, value.Real));
		}

		/// <summary>
		/// Returns the logarithm of a specified complex number in a specified base.
		/// </summary>
		/// <param name="value">A complex number.</param>
		/// <param name="baseValue">The base of the logarithm.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static ComplexF Log(ComplexF value, ComplexF baseValue) {
			return Log(value) / Log(baseValue);
		}

		/// <summary>
		/// Returns the base-10 logarithm of a specified complex number.
		/// </summary>
		/// <param name="value">A complex number.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static ComplexF Log10(ComplexF value) {
			value = Log(value);
			return new ComplexF(value.Real * 0.43429448190325f, value.Imaginary * 0.43429448190325f);
		}

		/// <summary>
		/// Returns the product of two complex numbers.
		/// </summary>
		/// <param name="left">The first complex number to multiply.</param>
		/// <param name="right">The second complex number to multiply.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static ComplexF Multiply(ComplexF left, ComplexF right) {
			return left * right;
		}

		/// <summary>
		/// Returns the additive inverse of a specified complex number.
		/// </summary>
		/// <param name="value">A complex number.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static ComplexF Negate(ComplexF value) {
			return -value;
		}

		/// <summary>
		/// Casts the specified value to a Complex number.
		/// </summary>
		/// <param name="value">The value to cast to ComplexF.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static explicit operator ComplexF(decimal value) {
			return new ComplexF((float) value, 0f);
		}

		/// <summary>
		/// Casts the specified value to a Complex number.
		/// </summary>
		/// <param name="value">The value to cast to ComplexF.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static implicit operator ComplexF(short value) {
			return new ComplexF(value, 0f);
		}

		/// <summary>
		/// Casts the specified value to a Complex number.
		/// </summary>
		/// <param name="value">The value to cast to ComplexF.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static implicit operator ComplexF(int value) {
			return new ComplexF(value, 0f);
		}

		/// <summary>
		/// Casts the specified value to a Complex number.
		/// </summary>
		/// <param name="value">The value to cast to ComplexF.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static implicit operator ComplexF(long value) {
			return new ComplexF(value, 0f);
		}

		/// <summary>
		/// Casts the specified value to a Complex number.
		/// </summary>
		/// <param name="value">The value to cast to ComplexF.</param>
		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static implicit operator ComplexF(ushort value) {
			return new ComplexF(value, 0f);
		}

		/// <summary>
		/// Casts the specified value to a Complex number.
		/// </summary>
		/// <param name="value">The value to cast to ComplexF.</param>
		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static implicit operator ComplexF(uint value) {
			return new ComplexF(value, 0f);
		}

		/// <summary>
		/// Casts the specified value to a Complex number.
		/// </summary>
		/// <param name="value">The value to cast to ComplexF.</param>
		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static implicit operator ComplexF(ulong value) {
			return new ComplexF(value, 0f);
		}

		/// <summary>
		/// Casts the specified value to a Complex number.
		/// </summary>
		/// <param name="value">The value to cast to ComplexF.</param>
		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static implicit operator ComplexF(sbyte value) {
			return new ComplexF(value, 0f);
		}

		/// <summary>
		/// Casts the specified value to a Complex number.
		/// </summary>
		/// <param name="value">The value to cast to ComplexF.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static implicit operator ComplexF(byte value) {
			return new ComplexF(value, 0f);
		}

		/// <summary>
		/// Casts the specified value to a Complex number.
		/// </summary>
		/// <param name="value">The value to cast to ComplexF.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static implicit operator ComplexF(float value) {
			return new ComplexF(value, 0f);
		}

		/// <summary>
		/// Casts the specified value to a Complex number.
		/// </summary>
		/// <param name="value">The value to cast to ComplexF.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static implicit operator ComplexF(double value) {
			return new ComplexF((float) value, 0f);
		}

		/// <summary>
		/// Returns a value that indicates whether two complex numbers are equal.
		/// </summary>
		/// <param name="left">The first complex number to compare.</param>
		/// <param name="right">The second complex number to compare.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator ==(ComplexF left, ComplexF right) {
			return left.Real == right.Real && left.Imaginary == right.Imaginary;
		}

		/// <summary>
		/// Returns a value that indicates whether two complex numbers are not equal.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator !=(ComplexF left, ComplexF right) {
			return !(left.Real == right.Real && left.Imaginary == right.Imaginary);
		}

		/// <summary>
		/// Adds two complex numbers.
		/// </summary>
		/// <param name="left">The first value to add.</param>
		/// <param name="right">The second value to add.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static ComplexF operator +(ComplexF left, ComplexF right) {
			return new ComplexF(left.Real + right.Real, left.Imaginary + right.Imaginary);
		}

		/// <summary>
		/// Adds two complex numbers.
		/// </summary>
		/// <param name="left">The first value to add.</param>
		/// <param name="right">The second value to add.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static ComplexF operator +(ComplexF left, float right) {
			return new ComplexF(left.Real + right, left.Imaginary);
		}

		/// <summary>
		/// Adds two complex numbers.
		/// </summary>
		/// <param name="left">The first value to add.</param>
		/// <param name="right">The second value to add.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static ComplexF operator +(float right, ComplexF left) {
			return new ComplexF(left.Real + right, left.Imaginary);
		}

		/// <summary>
		/// Multiplies the specified complex number by the specified real number.
		/// </summary>
		/// <param name="left">The first value to multiply.</param>
		/// <param name="right">The second value to multiply.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static ComplexF operator *(ComplexF left, float right) {
			return new ComplexF(left.Real * right, left.Imaginary * right);
		}

		/// <summary>
		/// Multiplies the specified complex number by the specified real number.
		/// </summary>
		/// <param name="left">The first value to multiply.</param>
		/// <param name="right">The second value to multiply.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static ComplexF operator *(float right, ComplexF left) {
			return new ComplexF(left.Real * right, left.Imaginary * right);
		}

		/// <summary>
		/// Multiplies two specified complex numbers.
		/// </summary>
		/// <param name="left">The first value to multiply.</param>
		/// <param name="right">The second value to multiply.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static ComplexF operator *(ComplexF left, ComplexF right) {
			return new ComplexF(left.Real * right.Real - left.Imaginary * right.Imaginary, left.Imaginary * right.Real + left.Real * right.Imaginary);
		}

		/// <summary>
		/// Divides the specified complex number by the specified real number.
		/// </summary>
		/// <param name="left">The value to be divided.</param>
		/// <param name="right">The value to divide by.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static ComplexF operator /(ComplexF left, float right) {
			return new ComplexF(left.Real / right, left.Imaginary / right);
		}

		/// <summary>
		/// Divides a specified complex number by another specified complex number.
		/// </summary>
		/// <param name="left">The value to be divided.</param>
		/// <param name="right">The value to divide by.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static ComplexF operator /(ComplexF left, ComplexF right) {
			float temp;
			if (Math.Abs(right.Imaginary) < Math.Abs(right.Real)) {
				temp = right.Imaginary / right.Real;
				return new ComplexF((left.Real + left.Imaginary * temp) / (right.Real + right.Imaginary * temp), (left.Imaginary - left.Real * temp) / (right.Real + right.Imaginary * temp));
			} else {
				temp = right.Real / right.Imaginary;
				return new ComplexF((left.Imaginary + left.Real * temp) / (right.Imaginary + right.Real * temp), (-left.Real + left.Imaginary * temp) / (right.Imaginary + right.Real * temp));
			}
		}

		/// <summary>
		/// Does nothing.
		/// </summary>
		/// <param name="value">The value to waste time on.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static ComplexF operator +(ComplexF value) {
			return value;
		}

		/// <summary>
		/// Subtracts a complex number from another complex number.
		/// </summary>
		/// <param name="left">The value to subtract from (the minuend).</param>
		/// <param name="right">The value to subtract (the subtrahend).</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static ComplexF operator -(ComplexF left, ComplexF right) {
			return new ComplexF(left.Real - right.Real, left.Imaginary - right.Imaginary);
		}

		/// <summary>
		/// Returns the additive inverse of a specified complex number.
		/// </summary>
		/// <param name="value">The value to negate.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static ComplexF operator -(ComplexF value) {
			return new ComplexF(-value.Real, -value.Imaginary);
		}

		/// <summary>
		/// Returns a specified complex number raised to a power specified by a complex number.
		/// </summary>
		/// <param name="value">A complex number to be raised to a power.</param>
		/// <param name="power">A complex number that specifies a power.</param>
		public static ComplexF Pow(ComplexF value, ComplexF power) {
			if (power == Zero)
				return One;
			else if (value == Zero)
				return Zero;
			double mag = value.Magnitude;
			double phase = Math.Atan2(value.Imaginary, value.Real);
			double num3 = power.Real * phase + power.Imaginary * Math.Log(mag);
			double num4 = Math.Pow(mag, power.Real) * Math.Pow(2.71828182845905, -power.Imaginary * phase);
			return new ComplexF((float) (num4 * Math.Cos(num3)), (float) (num4 * Math.Sin(num3)));
		}

		/// <summary>
		/// Returns the multiplicative inverse of a complex number.
		/// </summary>
		/// <param name="value">A complex number.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static ComplexF Reciprocal(ComplexF value) {
			return value.Real == 0f && value.Imaginary == 0f ? Zero : One / value;
		}

		/// <summary>
		/// Returns the sine of the specified complex number.
		/// </summary>
		/// <param name="value">A complex number.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static ComplexF Sin(ComplexF value) {
			double mReal = value.Real;
			double mImaginary = value.Imaginary;
			return new ComplexF((float) (Math.Sin(mReal) * Math.Cosh(mImaginary)), (float) (Math.Cos(mReal) * Math.Sinh(mImaginary)));
		}

		/// <summary>
		/// Returns the hyperbolic sine of the specified complex number.
		/// </summary>
		/// <param name="value">A complex number.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static ComplexF Sinh(ComplexF value) {
			double mReal = value.Real;
			double mImaginary = value.Imaginary;
			return new ComplexF((float) (Math.Sinh(mReal) * Math.Cos(mImaginary)), (float) (Math.Cosh(mReal) * Math.Sin(mImaginary)));
		}

		/// <summary>
		/// Returns the square root of a specified complex number.
		/// </summary>
		/// <param name="value">A complex number.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static ComplexF Sqrt(ComplexF value) {
			return FromPolarCoordinates((float) Math.Sqrt(value.Magnitude), value.Phase * 0.5f);
		}

		/// <summary>
		/// Subtracts one complex number from another and returns the result.
		/// </summary>
		/// <param name="left">The value to subtract from (the minuend).</param>
		/// <param name="right">The value to subtract (the subtrahend).</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static ComplexF Subtract(ComplexF left, ComplexF right) {
			return left - right;
		}

		/// <summary>
		/// Returns the tangent of the specified complex number.
		/// </summary>
		/// <param name="value">A complex number.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static ComplexF Tan(ComplexF value) {
			return Sin(value) / Cos(value);
		}

		/// <summary>
		/// Returns the hyperbolic tangent of the specified complex number.
		/// </summary>
		/// <param name="value">A complex number.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static ComplexF Tanh(ComplexF value) {
			return Sinh(value) / Cosh(value);
		}

		/// <summary>
		/// Converts the value of the current complex number to its equivalent string representation in Cartesian form.
		/// </summary>
		/// <returns>The string representation of the current instance in Cartesian form.</returns>
		public override string ToString() {
			return string.Format("(" + Real + ", " + Imaginary + ")");
		}

		/// <summary>
		/// Converts the value of the current complex number to its equivalent string representation in Cartesian form by using the specified culture-specific formatting information.
		/// </summary>
		/// <param name="provider">An object that supplies culture-specific formatting information.</param>
		public string ToString(IFormatProvider provider) {
			return string.Format(provider, "(" + Real + ", " + Imaginary + ")");
		}

		/// <summary>
		/// Converts the value of the current complex number to its equivalent string representation in Cartesian form by using the specified format and culture-specific format information for its real and imaginary parts.
		/// </summary>
		/// <param name="format">A standard or custom numeric format string.</param>
		/// <param name="provider">An object that supplies culture-specific formatting information.</param>
		public string ToString(string format, IFormatProvider provider) {
			return string.Format(provider, "(" + Real.ToString(format, provider) + ", " + Imaginary.ToString(format, provider) + ")");
		}
	}
}