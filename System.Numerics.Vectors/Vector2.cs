using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Numerics {
	/// <summary>
	/// A structure encapsulating two single precision floating point values and provides hardware accelerated methods.
	/// </summary>
	[Serializable]
	public struct Vector2 : IEquatable<Vector2>, IFormattable {
		/// <summary>The X component of the vector.</summary>
		public float X;
		/// <summary>The Y component of the vector.</summary>
		public float Y;

		/// <summary>Returns the vector (0,0).</summary>
		public static Vector2 Zero {
			get {
				return new Vector2();
			}
		}

		/// <summary>Returns the vector (1,1).</summary>
		public static Vector2 One {
			get {
				return new Vector2(1f, 1f);
			}
		}

		/// <summary>Returns the vector (1,0).</summary>
		public static Vector2 UnitX {
			get {
				return new Vector2(1f, 0.0f);
			}
		}

		/// <summary>Returns the vector (0,1).</summary>
		public static Vector2 UnitY {
			get {
				return new Vector2(0.0f, 1f);
			}
		}

		/// <summary>
		/// Constructs a vector whose elements are all the single specified value.
		/// </summary>
		/// <param name="value">The element to fill the vector with.</param>

		public Vector2(float value) {
			this = new Vector2(value, value);
		}

		/// <summary>
		/// Constructs a vector with the given individual elements.
		/// </summary>
		/// <param name="x">The X component.</param>
		/// <param name="y">The Y component.</param>

		public Vector2(float x, float y) {
			this.X = x;
			this.Y = y;
		}

		/// <summary>Adds two vectors together.</summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The summed vector.</returns>

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2 operator +(Vector2 left, Vector2 right) {
			return new Vector2(left.X + right.X, left.Y + right.Y);
		}

		/// <summary>Subtracts the second vector from the first.</summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The difference vector.</returns>

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2 operator -(Vector2 left, Vector2 right) {
			return new Vector2(left.X - right.X, left.Y - right.Y);
		}

		/// <summary>Multiplies two vectors together.</summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The product vector.</returns>

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2 operator *(Vector2 left, Vector2 right) {
			return new Vector2(left.X * right.X, left.Y * right.Y);
		}

		/// <summary>Multiplies a vector by the given scalar.</summary>
		/// <param name="left">The scalar value.</param>
		/// <param name="right">The source vector.</param>
		/// <returns>The scaled vector.</returns>

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2 operator *(float left, Vector2 right) {
			return new Vector2(left, left) * right;
		}

		/// <summary>Multiplies a vector by the given scalar.</summary>
		/// <param name="left">The source vector.</param>
		/// <param name="right">The scalar value.</param>
		/// <returns>The scaled vector.</returns>

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2 operator *(Vector2 left, float right) {
			return left * new Vector2(right, right);
		}

		/// <summary>Divides the first vector by the second.</summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The vector resulting from the division.</returns>

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2 operator /(Vector2 left, Vector2 right) {
			return new Vector2(left.X / right.X, left.Y / right.Y);
		}

		/// <summary>Divides the vector by the given scalar.</summary>
		/// <param name="value1">The source vector.</param>
		/// <param name="value2">The scalar value.</param>
		/// <returns>The result of the division.</returns>

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2 operator /(Vector2 value1, float value2) {
			float num = 1f / value2;
			return new Vector2(value1.X * num, value1.Y * num);
		}

		/// <summary>Negates a given vector.</summary>
		/// <param name="value">The source vector.</param>
		/// <returns>The negated vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2 operator -(Vector2 value) {
			return Vector2.Zero - value;
		}

		/// <summary>
		/// Returns a boolean indicating whether the two given vectors are equal.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>True if the vectors are equal; False otherwise.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator ==(Vector2 left, Vector2 right) {
			return left.Equals(right);
		}

		/// <summary>
		/// Returns a boolean indicating whether the two given vectors are not equal.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>True if the vectors are not equal; False if they are equal.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator !=(Vector2 left, Vector2 right) {
			return !(left == right);
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>The hash code.</returns>
		public override int GetHashCode() {
			return Helper.CombineHashCodes(this.X.GetHashCode(), this.Y.GetHashCode());
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Object is equal to this Vector2 instance.
		/// </summary>
		/// <param name="obj">The Object to compare against.</param>
		/// <returns>True if the Object is equal to this Vector2; False otherwise.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public override bool Equals(object obj) {
			if (!(obj is Vector2))
				return false;
			return this.Equals((Vector2) obj);
		}

		/// <summary>Returns a String representing this Vector2 instance.</summary>
		/// <returns>The string representation.</returns>
		public override string ToString() {
			return this.ToString("G", (IFormatProvider) CultureInfo.CurrentCulture);
		}

		/// <summary>
		/// Returns a String representing this Vector2 instance, using the specified format to format individual elements.
		/// </summary>
		/// <param name="format">The format of individual elements.</param>
		/// <returns>The string representation.</returns>
		public string ToString(string format) {
			return this.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture);
		}

		/// <summary>
		/// Returns a String representing this Vector2 instance, using the specified format to format individual elements
		/// and the given IFormatProvider.
		/// </summary>
		/// <param name="format">The format of individual elements.</param>
		/// <param name="formatProvider">The format provider to use when formatting elements.</param>
		/// <returns>The string representation.</returns>
		public string ToString(string format, IFormatProvider formatProvider) {
			StringBuilder stringBuilder = new StringBuilder();
			string numberGroupSeparator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
			stringBuilder.Append('<');
			stringBuilder.Append(this.X.ToString(format, formatProvider));
			stringBuilder.Append(numberGroupSeparator);
			stringBuilder.Append(' ');
			stringBuilder.Append(this.Y.ToString(format, formatProvider));
			stringBuilder.Append('>');
			return stringBuilder.ToString();
		}

		/// <summary>Returns the length of the vector.</summary>
		/// <returns>The vector's length.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public float Length() {
			if (Vector.IsHardwareAccelerated)
				return (float) Math.Sqrt((double) Vector2.Dot(this, this));
			return (float) Math.Sqrt((double) this.X * (double) this.X + (double) this.Y * (double) this.Y);
		}

		/// <summary>
		/// Returns the length of the vector squared. This operation is cheaper than Length().
		/// </summary>
		/// <returns>The vector's length squared.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public float LengthSquared() {
			if (Vector.IsHardwareAccelerated)
				return Vector2.Dot(this, this);
			return (float) ((double) this.X * (double) this.X + (double) this.Y * (double) this.Y);
		}

		/// <summary>
		/// Returns the Euclidean distance between the two given points.
		/// </summary>
		/// <param name="value1">The first point.</param>
		/// <param name="value2">The second point.</param>
		/// <returns>The distance.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static float Distance(Vector2 value1, Vector2 value2) {
			if (Vector.IsHardwareAccelerated) {
				Vector2 vector2 = value1 - value2;
				return (float) Math.Sqrt((double) Vector2.Dot(vector2, vector2));
			}
			float num1 = value1.X - value2.X;
			float num2 = value1.Y - value2.Y;
			return (float) Math.Sqrt((double) num1 * (double) num1 + (double) num2 * (double) num2);
		}

		/// <summary>
		/// Returns the Euclidean distance squared between the two given points.
		/// </summary>
		/// <param name="value1">The first point.</param>
		/// <param name="value2">The second point.</param>
		/// <returns>The distance squared.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static float DistanceSquared(Vector2 value1, Vector2 value2) {
			if (Vector.IsHardwareAccelerated) {
				Vector2 vector2 = value1 - value2;
				return Vector2.Dot(vector2, vector2);
			}
			float num1 = value1.X - value2.X;
			float num2 = value1.Y - value2.Y;
			return (float) ((double) num1 * (double) num1 + (double) num2 * (double) num2);
		}

		/// <summary>
		/// Returns a vector with the same direction as the given vector, but with a length of 1.
		/// </summary>
		/// <param name="value">The vector to normalize.</param>
		/// <returns>The normalized vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2 Normalize(Vector2 value) {
			if (Vector.IsHardwareAccelerated) {
				float num = value.Length();
				return value / num;
			}
			float num1 = 1f / (float) Math.Sqrt((double) value.X * (double) value.X + (double) value.Y * (double) value.Y);
			return new Vector2(value.X * num1, value.Y * num1);
		}

		/// <summary>
		/// Returns the reflection of a vector off a surface that has the specified normal.
		/// </summary>
		/// <param name="vector">The source vector.</param>
		/// <param name="normal">The normal of the surface being reflected off.</param>
		/// <returns>The reflected vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2 Reflect(Vector2 vector, Vector2 normal) {
			if (Vector.IsHardwareAccelerated) {
				float num = Vector2.Dot(vector, normal);
				return vector - 2f * num * normal;
			}
			float num1 = (float) ((double) vector.X * (double) normal.X + (double) vector.Y * (double) normal.Y);
			return new Vector2(vector.X - 2f * num1 * normal.X, vector.Y - 2f * num1 * normal.Y);
		}

		/// <summary>Restricts a vector between a min and max value.</summary>
		/// <param name="value1">The source vector.</param>
		/// <param name="min">The minimum value.</param>
		/// <param name="max">The maximum value.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2 Clamp(Vector2 value1, Vector2 min, Vector2 max) {
			float x1 = value1.X;
			float num1 = (double) x1 > (double) max.X ? max.X : x1;
			float x2 = (double) num1 < (double) min.X ? min.X : num1;
			float y1 = value1.Y;
			float num2 = (double) y1 > (double) max.Y ? max.Y : y1;
			float y2 = (double) num2 < (double) min.Y ? min.Y : num2;
			return new Vector2(x2, y2);
		}

		/// <summary>
		/// Linearly interpolates between two vectors based on the given weighting.
		/// </summary>
		/// <param name="value1">The first source vector.</param>
		/// <param name="value2">The second source vector.</param>
		/// <param name="amount">Value between 0 and 1 indicating the weight of the second source vector.</param>
		/// <returns>The interpolated vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2 Lerp(Vector2 value1, Vector2 value2, float amount) {
			return new Vector2(value1.X + (value2.X - value1.X) * amount, value1.Y + (value2.Y - value1.Y) * amount);
		}

		/// <summary>
		/// Transforms a vector by the given Quaternion rotation value.
		/// </summary>
		/// <param name="value">The source vector to be rotated.</param>
		/// <param name="rotation">The rotation to apply.</param>
		/// <returns>The transformed vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2 Transform(Vector2 value, Quaternion rotation) {
			float num1 = rotation.X + rotation.X;
			float num2 = rotation.Y + rotation.Y;
			float num3 = rotation.Z + rotation.Z;
			float num4 = rotation.W * num3;
			float num5 = rotation.X * num1;
			float num6 = rotation.X * num2;
			float num7 = rotation.Y * num2;
			float num8 = rotation.Z * num3;
			return new Vector2((float) ((double) value.X * (1.0 - (double) num7 - (double) num8) + (double) value.Y * ((double) num6 - (double) num4)), (float) ((double) value.X * ((double) num6 + (double) num4) + (double) value.Y * (1.0 - (double) num5 - (double) num8)));
		}

		/// <summary>Adds two vectors together.</summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The summed vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2 Add(Vector2 left, Vector2 right) {
			return left + right;
		}

		/// <summary>Subtracts the second vector from the first.</summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The difference vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2 Subtract(Vector2 left, Vector2 right) {
			return left - right;
		}

		/// <summary>Multiplies two vectors together.</summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The product vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2 Multiply(Vector2 left, Vector2 right) {
			return left * right;
		}

		/// <summary>Multiplies a vector by the given scalar.</summary>
		/// <param name="left">The source vector.</param>
		/// <param name="right">The scalar value.</param>
		/// <returns>The scaled vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2 Multiply(Vector2 left, float right) {
			return left * right;
		}

		/// <summary>Multiplies a vector by the given scalar.</summary>
		/// <param name="left">The scalar value.</param>
		/// <param name="right">The source vector.</param>
		/// <returns>The scaled vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2 Multiply(float left, Vector2 right) {
			return left * right;
		}

		/// <summary>Divides the first vector by the second.</summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The vector resulting from the division.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2 Divide(Vector2 left, Vector2 right) {
			return left / right;
		}

		/// <summary>Divides the vector by the given scalar.</summary>
		/// <param name="left">The source vector.</param>
		/// <param name="divisor">The scalar value.</param>
		/// <returns>The result of the division.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2 Divide(Vector2 left, float divisor) {
			return left / divisor;
		}

		/// <summary>Negates a given vector.</summary>
		/// <param name="value">The source vector.</param>
		/// <returns>The negated vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2 Negate(Vector2 value) {
			return -value;
		}

		/// <summary>
		/// Copies the contents of the vector into the given array.
		/// </summary>
		/// <param name="array">The destination array.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void CopyTo(float[] array) {
			this.CopyTo(array, 0);
		}

		/// <summary>
		/// Copies the contents of the vector into the given array, starting from the given index.
		/// </summary>
		/// <exception cref="T:System.ArgumentNullException">If array is null.</exception>
		/// <exception cref="T:System.RankException">If array is multidimensional.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">If index is greater than end of the array or index is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">If number of elements in source vector is greater than those available in destination array
		/// or if there are not enough elements to copy.</exception>
		public void CopyTo(float[] array, int index) {
			if (index < 0 || index >= array.Length)
				throw new ArgumentOutOfRangeException("Index was out of bounds: " + index);
			if (array.Length - index < 2)
				throw new ArgumentException("Number of elements in source vector is greater than the destination array.");
			array[index] = this.X;
			array[index + 1] = this.Y;
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Vector2 is equal to this Vector2 instance.
		/// </summary>
		/// <param name="other">The Vector2 to compare this instance to.</param>
		/// <returns>True if the other Vector2 is equal to this instance; False otherwise.</returns>

		public bool Equals(Vector2 other) {
			if ((double) this.X == (double) other.X)
				return (double) this.Y == (double) other.Y;
			return false;
		}

		/// <summary>Returns the dot product of two vectors.</summary>
		/// <param name="value1">The first vector.</param>
		/// <param name="value2">The second vector.</param>
		/// <returns>The dot product.</returns>

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static float Dot(Vector2 value1, Vector2 value2) {
			return (float) ((double) value1.X * (double) value2.X + (double) value1.Y * (double) value2.Y);
		}

		/// <summary>
		/// Returns a vector whose elements are the minimum of each of the pairs of elements in the two source vectors.
		/// </summary>
		/// <param name="value1">The first source vector.</param>
		/// <param name="value2">The second source vector.</param>
		/// <returns>The minimized vector.</returns>

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2 Min(Vector2 value1, Vector2 value2) {
			return new Vector2((double) value1.X < (double) value2.X ? value1.X : value2.X, (double) value1.Y < (double) value2.Y ? value1.Y : value2.Y);
		}

		/// <summary>
		/// Returns a vector whose elements are the maximum of each of the pairs of elements in the two source vectors
		/// </summary>
		/// <param name="value1">The first source vector</param>
		/// <param name="value2">The second source vector</param>
		/// <returns>The maximized vector</returns>

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2 Max(Vector2 value1, Vector2 value2) {
			return new Vector2((double) value1.X > (double) value2.X ? value1.X : value2.X, (double) value1.Y > (double) value2.Y ? value1.Y : value2.Y);
		}

		/// <summary>
		/// Returns a vector whose elements are the absolute values of each of the source vector's elements.
		/// </summary>
		/// <param name="value">The source vector.</param>
		/// <returns>The absolute value vector.</returns>

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2 Abs(Vector2 value) {
			return new Vector2(Math.Abs(value.X), Math.Abs(value.Y));
		}

		/// <summary>
		/// Returns a vector whose elements are the square root of each of the source vector's elements.
		/// </summary>
		/// <param name="value">The source vector.</param>
		/// <returns>The square root vector.</returns>

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2 SquareRoot(Vector2 value) {
			return new Vector2((float) Math.Sqrt((double) value.X), (float) Math.Sqrt((double) value.Y));
		}
	}
}