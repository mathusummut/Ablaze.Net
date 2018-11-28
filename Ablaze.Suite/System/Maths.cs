using System.Runtime.CompilerServices;

namespace System {
	/// <summary>
	/// Contains common mathematical functions and constants.
	/// </summary>
	public static partial class Maths {
		/// <summary>
		/// Defines the value of 1/2Pi as a <see cref="System.Single"/>.
		/// </summary>
		public const float TwoPIInverseF = 0.1591549430918953357689f;
		/// <summary>
		/// Defines the value of 1/2Pi as a <see cref="System.Double"/>.
		/// </summary>
		public const double TwoPIInverseD = 0.1591549430918953357689;
		/// <summary>
		/// Defines the value of 1/Pi as a <see cref="System.Single"/>.
		/// </summary>
		public const float PIInverseF = 0.3183098861837906715378f;
		/// <summary>
		/// Defines the value of 1/Pi as a <see cref="System.Double"/>.
		/// </summary>
		public const double PIInverseD = 0.3183098861837906715378;
		/// <summary>
		/// Defines the value of Pi as a <see cref="System.Single"/>.
		/// </summary>
		public const float PiF = 3.1415926535897932384626433832795F;
		/// <summary>
		/// Defines the value of Pi as a <see cref="System.Double"/>.
		/// </summary>
		public const double PiD = Math.PI;
		/// <summary>
		/// Defines the value of Pi/2 as a <see cref="System.Single"/>.
		/// </summary>
		public const float PiOver2F = PiF * 0.5F;
		/// <summary>
		/// Defines the value of Pi/2 divided by two as a <see cref="System.Double"/>.
		/// </summary>
		public const double PiOver2D = Math.PI * 0.5;
		/// <summary>
		/// Defines the value of Pi/3 as a <see cref="System.Single"/>.
		/// </summary>
		public const float PiOver3F = PiF * 0.333333333333333333333f;
		/// <summary>
		/// Defines the value of Pi/3 as a <see cref="System.Double"/>.
		/// </summary>
		public const double PiOver3D = Math.PI * 0.333333333333333333333;
		/// <summary>
		/// Defines the value of Pi/4 as a <see cref="System.Single"/>.
		/// </summary>
		public const float PiOver4F = PiF * 0.25F;
		/// <summary>
		/// Defines the value of Pi/4 as a <see cref="System.Double"/>.
		/// </summary>
		public const double PiOver4D = Math.PI * 0.25;
		/// <summary>
		/// Defines the value of Pi/5 as a <see cref="System.Single"/>.
		/// </summary>
		public const float PiOver5F = PiF * 0.2f;
		/// <summary>
		/// Defines the value of Pi/5 as a <see cref="System.Double"/>.
		/// </summary>
		public const double PiOver5D = Math.PI * 0.2;
		/// <summary>
		/// Defines the value of Pi/6 as a <see cref="System.Single"/>.
		/// </summary>
		public const float PiOver6F = PiF * 0.166666666666666666666666f;
		/// <summary>
		/// Defines the value of Pi/6 as a <see cref="System.Double"/>.
		/// </summary>
		public const double PiOver6D = Math.PI * 0.166666666666666666666666;
		/// <summary>
		/// Defines the value of Pi*2 as a <see cref="System.Single"/>.
		/// </summary>
		public const float TwoPiF = 2F * PiF;
		/// <summary>
		/// Defines the value of Pi*2 as a <see cref="System.Double"/>.
		/// </summary>
		public const double TwoPiD = 2.0 * Math.PI;
		/// <summary>
		/// Defines the value of Pi*3 as a <see cref="System.Single"/>.
		/// </summary>
		public const float ThreePiF = 3F * PiF;
		/// <summary>
		/// Defines the value of Pi*3 as a <see cref="System.Double"/>.
		/// </summary>
		public const double ThreePiD = 3.0 * Math.PI;
		/// <summary>
		/// Defines the value of Pi*3/2 as a <see cref="System.Single"/>.
		/// </summary>
		public const float ThreePiOver2F = PiF * 1.5F;
		/// <summary>
		/// Defines the value of Pi*3/2 as a <see cref="System.Double"/>.
		/// </summary>
		public const double ThreePiOver2D = Math.PI * 1.5;
		/// <summary>
		/// Defines the base-10 logarithm of E as a <see cref="System.Single"/>.
		/// </summary>
		public const float Log10eF = 0.4342944819032518276511F;
		/// <summary>
		/// Defines the base-10 logarithm of E as a <see cref="System.Double"/>.
		/// </summary>
		public const double Log10eD = 0.4342944819032518276511;
		/// <summary>
		/// Defines a constant that when multiplied to a degrees value converts it to radians (PI/180). 
		/// </summary>
		public const float DegToRadF = PiF * 0.0055555555555555555555f;
		/// <summary>
		/// Defines a constant that when multiplied to a degrees value converts it to radians (PI/180). 
		/// </summary>
		public const double DegToRadD = Math.PI * 0.0055555555555555555555;
		/// <summary>
		/// Defines a constant that when multiplied to a radian value converts it to degrees (180/PI). 
		/// </summary>
		public const float RadToDegF = 180f / PiF;
		/// <summary>
		/// Defines a constant that when multiplied to a radian value converts it to degrees (180/PI). 
		/// </summary>
		public const double RadToDegD = 180.0 / Math.PI;
		/// <summary>
		/// Defines the square root of 2. 
		/// </summary>
		public const float SqrtOf2F = 1.414213562373095048802f;
		/// <summary>
		/// Defines the square root of 2. 
		/// </summary>
		public const double SqrtOf2D = 1.414213562373095048802;

		/// <summary>
		/// Quantile function (Inverse CDF) for the normal distribution (based on the R function QNorm()).
		/// </summary>
		/// <param name="p">Probability.</param>
		/// <param name="mean">The mean of normal distribution.</param>
		/// <param name="sigma">Standard deviation of normal distribution.</param>
		public static double NormInv(double p, double mean = 0, double sigma = 1) {
			if (double.IsNaN(p) || double.IsNaN(mean) || double.IsNaN(sigma))
				return double.NaN;
			if (p < 0.0)
				return double.NegativeInfinity;
			else if (p > 1.0)
				return double.PositiveInfinity;
			else if (sigma < 0.0)
				sigma = -sigma;
			else if (sigma == 0.0)
				return mean;

			double q = p - 0.5;
			double r, val;

			if (Math.Abs(q) <= 0.425)  // 0.075 <= p <= 0.925
			{
				r = .180625 - q * q;
				val = q * (((((((r * 2509.0809287301226727 +
								 33430.575583588128105) * r + 67265.770927008700853) * r +
							   45921.953931549871457) * r + 13731.693765509461125) * r +
							 1971.5909503065514427) * r + 133.14166789178437745) * r +
						   3.387132872796366608)
					/ (((((((r * 5226.495278852854561 +
							 28729.085735721942674) * r + 39307.89580009271061) * r +
						   21213.794301586595867) * r + 5394.1960214247511077) * r +
						 687.1870074920579083) * r + 42.313330701600911252) * r + 1.0);
			} else {
				r = Math.Sqrt(-Math.Log(p));

				if (r <= 5.0)              // <==> min(p,1-p) >= exp(-25) ~= 1.3888e-11
				{
					r -= 1.6;
					val = (((((((r * 7.7454501427834140764e-4 +
								 .0227238449892691845833) * r + .24178072517745061177) *
							   r + 1.27045825245236838258) * r +
							  3.64784832476320460504) * r + 5.7694972214606914055) *
							r + 4.6303378461565452959) * r +
						   1.42343711074968357734)
						/ (((((((r *
								 1.05075007164441684324e-9 + 5.475938084995344946e-4) *
								r + .0151986665636164571966) * r +
							   .14810397642748007459) * r + .68976733498510000455) *
							 r + 1.6763848301838038494) * r +
							2.05319162663775882187) * r + 1.0);
				} else                     // very close to  0 or 1 
				{
					r -= 5.0;
					val = (((((((r * 2.01033439929228813265e-7 +
								 2.71155556874348757815e-5) * r +
								.0012426609473880784386) * r + .026532189526576123093) *
							  r + .29656057182850489123) * r +
							 1.7848265399172913358) * r + 5.4637849111641143699) *
						   r + 6.6579046435011037772)
						/ (((((((r *
								 2.04426310338993978564e-15 + 1.4215117583164458887e-7) *
								r + 1.8463183175100546818e-5) * r +
							   7.868691311456132591e-4) * r + .0148753612908506148525)
							 * r + .13692988092273580531) * r +
							.59983220655588793769) * r + 1.0);
				}
				if (q < 0.0)
					val = -val;
			}
			return mean + sigma * val;
		}

		/// <summary>
		/// Computes the sigmoid of the specified parameter.
		/// </summary>
		/// <param name="value">The value to use as parameter.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static double Sigmoid(double value) {
			double k = Math.Exp(value);
			return k / (1.0 + k);
		}

		/// <summary>
		/// Computes the normalized sinc (sin πx)/πx.
		/// </summary>
		/// <param name="value">The value to use as parameter.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static double Sinc(double value) {
			value *= Math.PI;
			if (value < 0.01 && value > -0.01) {
				value *= value;
				return value * (value * 0.00833333333333333333333333 + -0.1666666666666666666666667) + 1.0;
			} else
				return Math.Sin(value) / value;
		}

		/// <summary>
		/// Skews the specified uniform values into a normal frequency distribution approximation (no randomization).
		/// </summary>
		/// <param name="min">The minimum value of the distribution graph.</param>
		/// <param name="value">The value to convert into the corresponding normally-distributed value.</param>
		/// <param name="max">The maximum value of the distribution graph.</param>
		/// <param name="inverted">Whether to flip the distribution upside down (favoring min and max instead of the center).</param>
		public static double ApplyNormativeSkew(double min, double value, double max, bool inverted = false) {
			if (min > max) {
				double temp = min;
				min = max;
				max = temp;
			}
			if (value <= min)
				return min;
			else if (value >= max)
				return max;
			value -= min;
			max -= min;
			double center = max * 0.5;
			if (value == center)
				return center;
			double p = value / max;
			double normalDist = Sigmoid(Math.Abs(1 / NormInv(p, 0.0, 0.2)));
			if (double.IsNaN(normalDist))
				return center;
			double multiplier = normalDist;
			normalDist *= multiplier;
			if (p < 0.175 || p > 0.825)
				normalDist *= multiplier;
			if (p < 0.09 || p > 0.91)
				normalDist *= multiplier;
			if (p < 0.025 || p > 0.975)
				normalDist *= multiplier;
			if (p < 0.01 || p > 0.99)
				normalDist *= multiplier;
			normalDist *= 0.6366197723675813430755;
			if (!inverted)
				normalDist = 1.0 - normalDist;
			return (value - center) * normalDist + center + min;
		}

		/// <summary>
		/// Calculates the standard deviation of the specified numerical values.
		/// </summary>
		/// <param name="values">The values to calculate S.D. from.</param>
		public static double StandardDeviation(this double[] values) {
			if (values == null || values.Length == 0)
				return 0.0;
			double avg = Mean(values);
			double totalVariance = 0.0;
			double temp;
			for (int i = 0; i < values.Length; i++) {
				temp = values[i] - avg;
				totalVariance += temp * temp;
			}
			return Math.Sqrt(totalVariance / values.Length);
		}

		/// <summary>
		/// Calculates the average of the specified values.
		/// </summary>
		/// <param name="values">The values whose average to return.</param>
		public static double Mean(this double[] values) {
			if (values == null || values.Length == 0)
				return 0.0;
			double total = 0.0;
			for (int i = 0; i < values.Length; i++)
				total += values[i];
			return total / values.Length;
		}

		/// <summary>
		/// Calculates the standard deviation of the specified numerical values.
		/// </summary>
		/// <param name="values">The values to calculate S.D. from.</param>
		public static float StandardDeviation(this float[] values) {
			if (values == null || values.Length == 0)
				return 0f;
			float avg = Mean(values);
			float totalVariance = 0f;
			float temp;
			for (int i = 0; i < values.Length; i++) {
				temp = values[i] - avg;
				totalVariance += temp * temp;
			}
			return (float) Math.Sqrt(totalVariance / values.Length);
		}

		/// <summary>
		/// Calculates the average of the specified values.
		/// </summary>
		/// <param name="values">The values whose average to return.</param>
		public static float Mean(this float[] values) {
			if (values == null || values.Length == 0)
				return 0f;
			float total = 0f;
			for (int i = 0; i < values.Length; i++)
				total += values[i];
			return total / values.Length;
		}

		/// <summary>
		/// Gets whether the value is a power of two.
		/// </summary>
		/// <param name="x">The value to check.</param>
		public static bool IsPowerOf2(this int x) {
			return (x > 0) ? ((x & (x - 1)) == 0) : false;
		}

		/// <summary>
		/// Squares the given value.
		/// </summary>
		/// <param name="x">The value to square.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static int Square(this int x) {
			return x * x;
		}

		/// <summary>
		/// Squares the given value.
		/// </summary>
		/// <param name="x">The value to square.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static float Square(this float x) {
			return x * x;
		}

		/// <summary>
		/// Squares the given value.
		/// </summary>
		/// <param name="x">The value to square.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static double Square(this double x) {
			return x * x;
		}

		/// <summary>
		/// Cubes the given value.
		/// </summary>
		/// <param name="x">The value to cube.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static int Cube(this int x) {
			return x * x * x;
		}

		/// <summary>
		/// Cubes the given value.
		/// </summary>
		/// <param name="x">The value to cube.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static float Cube(this float x) {
			return x * x * x;
		}

		/// <summary>
		/// Cubes the given value.
		/// </summary>
		/// <param name="x">The value to cube.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static double Cube(this double x) {
			return x * x * x;
		}

		/// <summary>
		/// Returns the next power of two that is equal or larger than the specified number.
		/// </summary>
		/// <param name="n">The specified number.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public static uint CeilingPowerOfTwo(this uint n) {
			n--;
			n |= n >> 1;
			n |= n >> 2;
			n |= n >> 4;
			n |= n >> 8;
			n |= n >> 16;
			return n + 1u;
		}

		/// <summary>
		/// Returns the next power of two that is equal or larger than the specified number.
		/// </summary>
		/// <param name="n">The specified number.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public static ulong CeilingPowerOfTwo(this ulong n) {
			n--;
			n |= n >> 1;
			n |= n >> 2;
			n |= n >> 4;
			n |= n >> 8;
			n |= n >> 16;
			n |= n >> 32;
			return n + 1u;
		}

		/// <summary>
		/// Returns the last power of two that is equal or smaller than the specified number.
		/// </summary>
		/// <param name="n">The specified number.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public static uint FloorPowerOfTwo(this uint n) {
			n |= n >> 1;
			n |= n >> 2;
			n |= n >> 4;
			n |= n >> 8;
			n |= n >> 16;
			return n - (n >> 1);
		}

		/// <summary>
		/// Returns the last power of two that is equal or smaller than the specified number.
		/// </summary>
		/// <param name="n">The specified number.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public static ulong FloorPowerOfTwo(this ulong n) {
			n |= n >> 1;
			n |= n >> 2;
			n |= n >> 4;
			n |= n >> 8;
			n |= n >> 16;
			n |= n >> 32;
			return n - (n >> 1);
		}

		/// <summary>
		/// Calculates the truncated log base 2 of a natural number.
		/// </summary>
		/// <param name="n">The number.</param>
		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static int Log2(this uint n) {
			int log = 0;
			while ((n >>= 1) != 0)
				log++;
			return log;
		}

		/// <summary>
		/// Calculates the truncated log base 2 of a natural number. 0 is returned for 0.
		/// </summary>
		/// <param name="n">The number.</param>
		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static int Log2(this ulong n) {
			int log = 0;
			while ((n >>= 1) != 0)
				log++;
			return log;
		}

		/// <summary>Calculates the factorial of a given natural number.
		/// </summary>
		/// <param name="n">The number.</param>
		/// <returns>n!</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static long Factorial(this int n) {
			long result = 1;
			for (; n > 1; n--)
				result *= n;
			return result;
		}

		/// <summary>
		/// Finds the integer square root of the specified value.
		/// </summary>
		/// <param name="num">The value whose square root to return.</param>
		[CLSCompliant(false)]
		public static uint Sqrt(this uint num) {
			if (num == 0)
				return 0;
			uint n = (num >> 1) + 1;
			uint n1 = (n + (num / n)) >> 1;
			while (n1 < n) {
				n = n1;
				n1 = (n + (num / n)) >> 1;
			}
			return n;
		}

		/// <summary>
		/// Calculates the binomial coefficient <paramref name="n"/> above <paramref name="k"/>.
		/// </summary>
		/// <param name="n">The n.</param>
		/// <param name="k">The k.</param>
		/// <returns>n! / (k! * (n - k)!)</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static long BinomialCoefficient(int n, int k) {
			return Factorial(n) / (Factorial(k) * Factorial(n - k));
		}

		/// <summary>
		/// Gets the number of bits set in the specified value.
		/// </summary>
		/// <param name="val">The value whose set bits to count.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static int NumberOfSetBits(this int val) {
			return NumberOfSetBits(unchecked((uint) val));
		}

		/// <summary>
		/// Gets the number of bits set in the specified value.
		/// </summary>
		/// <param name="val">The value whose set bits to count.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public static int NumberOfSetBits(this uint val) {
			val = val - ((val >> 1) & 0x55555555);
			val = (val & 0x33333333) + ((val >> 2) & 0x33333333);
			return (int) unchecked((((val + (val >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24);
		}

		/// <summary>
		/// Gets the number of bits set in the specified value.
		/// </summary>
		/// <param name="val">The value whose set bits to count.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static int NumberOfSetBits(this long val) {
			return NumberOfSetBits(unchecked((ulong) val));
		}

		/// <summary>
		/// Gets the number of bits set in the specified value.
		/// </summary>
		/// <param name="val">The value whose set bits to count.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public static int NumberOfSetBits(this ulong val) {
			val = val - ((val >> 1) & 0x5555555555555555UL);
			val = (val & 0x3333333333333333UL) + ((val >> 2) & 0x3333333333333333UL);
			return (int) unchecked(((val + (val >> 4)) & 0xF0F0F0F0F0F0F0FUL) * 0x101010101010101UL >> 56);
		}

		/// <summary>
		/// Gets the zero-based index of the highest bit set in the specified value (or -1 if no bit is set).
		/// </summary>
		/// <param name="val">The value to check.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static int HighestSetBit(this int val) {
			return HighestSetBit(unchecked((uint) val));
		}

		/// <summary>
		/// Gets the zero-based index of the highest bit set in the specified value (or -1 if no bit is set).
		/// </summary>
		/// <param name="val">The value to check.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public static int HighestSetBit(this uint val) {
			return val == 0u ? -1 : Log2(FloorPowerOfTwo(val));
		}

		/// <summary>
		/// Gets the zero-based index of the highest bit set in the specified value (or -1 if no bit is set).
		/// </summary>
		/// <param name="val">The value to check.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static int HighestSetBit(this long val) {
			return HighestSetBit(unchecked((ulong) val));
		}

		/// <summary>
		/// Gets the zero-based index of the highest bit set in the specified value (or -1 if no bit is set).
		/// </summary>
		/// <param name="val">The value to check.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public static int HighestSetBit(this ulong val) {
			return val == 0ul ? -1 : Log2(FloorPowerOfTwo(val));
		}

		/// <summary>
		/// Returns whether the specified value can be represented as an integer.
		/// </summary>
		/// <param name="value">The value to check.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool RepresentsInteger(this float value) {
			return Math.Abs(value - (int) value) <= float.Epsilon;
		}

		/// <summary>
		/// Returns whether the specified value can be represented as a long data type.
		/// </summary>
		/// <param name="value">The value to check.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool RepresentsLong(this double value) {
			return Math.Abs(value - (long) value) <= double.Epsilon;
		}

		/// <summary>
		/// Clamps the value to the specified range. If min is larger than max, the return value is either min or max (unspecified for speed).
		/// </summary>
		/// <param name="value">The value to clamp to the specified range.</param>
		/// <param name="min">The minimum value (inclusive).</param>
		/// <param name="max">The maximum value (inclusive).</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public static sbyte Clamp(this sbyte value, sbyte min, sbyte max) {
			return (value < min) ? min : ((value > max) ? max : value);
		}

		/// <summary>
		/// Clamps the value to the specified range. If min is larger than max, the return value is either min or max (unspecified for speed).
		/// </summary>
		/// <param name="value">The value to clamp to the specified range.</param>
		/// <param name="min">The minimum value (inclusive).</param>
		/// <param name="max">The maximum value (inclusive).</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static short Clamp(this short value, short min, short max) {
			return (value < min) ? min : ((value > max) ? max : value);
		}

		/// <summary>
		/// Clamps the value to the specified range. If min is larger than max, the return value is either min or max (unspecified for speed).
		/// </summary>
		/// <param name="value">The value to clamp to the specified range.</param>
		/// <param name="min">The minimum value (inclusive).</param>
		/// <param name="max">The maximum value (inclusive).</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static int Clamp(this int value, int min, int max) {
			return (value < min) ? min : ((value > max) ? max : value);
		}

		/// <summary>
		/// Clamps the value to the specified range. If min is larger than max, the return value is either min or max (unspecified for speed).
		/// </summary>
		/// <param name="value">The value to clamp to the specified range.</param>
		/// <param name="min">The minimum value (inclusive).</param>
		/// <param name="max">The maximum value (inclusive).</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static long Clamp(this long value, long min, long max) {
			return (value < min) ? min : ((value > max) ? max : value);
		}

		/// <summary>
		/// Clamps the value to the specified range. If min is larger than max, the return value is either min or max (unspecified for speed).
		/// </summary>
		/// <param name="value">The value to clamp to the specified range.</param>
		/// <param name="min">The minimum value (inclusive).</param>
		/// <param name="max">The maximum value (inclusive).</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static float Clamp(this float value, float min, float max) {
			return (value < min) ? min : ((value > max) ? max : value);
		}

		/// <summary>
		/// Clamps the value to the specified range. If min is larger than max, the return value is either min or max (unspecified for speed).
		/// </summary>
		/// <param name="value">The value to clamp to the specified range.</param>
		/// <param name="min">The minimum value (inclusive).</param>
		/// <param name="max">The maximum value (inclusive).</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static double Clamp(this double value, double min, double max) {
			return (value < min) ? min : ((value > max) ? max : value);
		}

		/// <summary>
		/// Clamps the value to the specified range. If min is larger than max, the return value is either min or max (unspecified for speed).
		/// </summary>
		/// <param name="value">The value to clamp to the specified range.</param>
		/// <param name="min">The minimum value (inclusive).</param>
		/// <param name="max">The maximum value (inclusive).</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static decimal Clamp(this decimal value, decimal min, decimal max) {
			return (value < min) ? min : ((value > max) ? max : value);
		}

		/// <summary>
		/// Clamps the value to the specified range. If min is larger than max, the return value is either min or max (unspecified for speed).
		/// </summary>
		/// <param name="value">The value to clamp to the specified range.</param>
		/// <param name="min">The minimum value (inclusive).</param>
		/// <param name="max">The maximum value (inclusive).</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static byte Clamp(this byte value, byte min, byte max) {
			return (value < min) ? min : ((value > max) ? max : value);
		}

		/// <summary>
		/// Clamps the value to the specified range. If min is larger than max, the return value is either min or max (unspecified for speed).
		/// </summary>
		/// <param name="value">The value to clamp to the specified range.</param>
		/// <param name="min">The minimum value (inclusive).</param>
		/// <param name="max">The maximum value (inclusive).</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public static ushort Clamp(this ushort value, ushort min, ushort max) {
			return (value < min) ? min : ((value > max) ? max : value);
		}

		/// <summary>
		/// Clamps the value to the specified range. If min is larger than max, the return value is either min or max (unspecified for speed).
		/// </summary>
		/// <param name="value">The value to clamp to the specified range.</param>
		/// <param name="min">The minimum value (inclusive).</param>
		/// <param name="max">The maximum value (inclusive).</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public static uint Clamp(this uint value, uint min, uint max) {
			return (value < min) ? min : ((value > max) ? max : value);
		}

		/// <summary>
		/// Clamps the value to the specified range. If min is larger than max, the return value is either min or max (unspecified for speed).
		/// </summary>
		/// <param name="value">The value to clamp to the specified range.</param>
		/// <param name="min">The minimum value (inclusive).</param>
		/// <param name="max">The maximum value (inclusive).</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public static ulong Clamp(this ulong value, ulong min, ulong max) {
			return (value < min) ? min : ((value > max) ? max : value);
		}
	}
}