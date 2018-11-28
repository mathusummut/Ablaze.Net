using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Numerics {
	/// <summary>
	/// A structure encapsulating three single precision floating point values and provides hardware accelerated methods.
	/// </summary>
	[Serializable]
	public struct Vector3 : IEquatable<Vector3>, IFormattable {
		/// <summary>The X component of the vector.</summary>
		public float X;
		/// <summary>The Y component of the vector.</summary>
		public float Y;
		/// <summary>The Z component of the vector.</summary>
		public float Z;

		/// <summary>Returns the vector (0,0,0).</summary>
		public static Vector3 Zero {
			get {
				return new Vector3();
			}
		}

		/// <summary>Returns the vector (1,1,1).</summary>
		public static Vector3 One {
			get {
				return new Vector3(1f, 1f, 1f);
			}
		}

		/// <summary>Returns the vector (1,0,0).</summary>
		public static Vector3 UnitX {
			get {
				return new Vector3(1f, 0.0f, 0.0f);
			}
		}

		/// <summary>Returns the vector (0,1,0).</summary>
		public static Vector3 UnitY {
			get {
				return new Vector3(0.0f, 1f, 0.0f);
			}
		}

		/// <summary>Returns the vector (0,0,1).</summary>
		public static Vector3 UnitZ {
			get {
				return new Vector3(0.0f, 0.0f, 1f);
			}
		}

		/// <summary>
		/// Constructs a vector whose elements are all the single specified value.
		/// </summary>
		/// <param name="value">The element to fill the vector with.</param>

		public Vector3(float value) {
			this = new Vector3(value, value, value);
		}

		/// <summary>
		/// Constructs a Vector3 from the given Vector2 and a third value.
		/// </summary>
		/// <param name="value">The Vector to extract X and Y components from.</param>
		/// <param name="z">The Z component.</param>
		public Vector3(Vector2 value, float z) {
			this = new Vector3(value.X, value.Y, z);
		}

		/// <summary>
		/// Constructs a vector with the given individual elements.
		/// </summary>
		/// <param name="x">The X component.</param>
		/// <param name="y">The Y component.</param>
		/// <param name="z">The Z component.</param>

		public Vector3(float x, float y, float z) {
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		/// <summary>Adds two vectors together.</summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The summed vector.</returns>

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector3 operator +(Vector3 left, Vector3 right) {
			return new Vector3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
		}

		/// <summary>Subtracts the second vector from the first.</summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The difference vector.</returns>

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector3 operator -(Vector3 left, Vector3 right) {
			return new Vector3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
		}

		/// <summary>Multiplies two vectors together.</summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The product vector.</returns>

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector3 operator *(Vector3 left, Vector3 right) {
			return new Vector3(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
		}

		/// <summary>Multiplies a vector by the given scalar.</summary>
		/// <param name="left">The source vector.</param>
		/// <param name="right">The scalar value.</param>
		/// <returns>The scaled vector.</returns>

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector3 operator *(Vector3 left, float right) {
			return left * new Vector3(right);
		}

		/// <summary>Multiplies a vector by the given scalar.</summary>
		/// <param name="left">The scalar value.</param>
		/// <param name="right">The source vector.</param>
		/// <returns>The scaled vector.</returns>

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector3 operator *(float left, Vector3 right) {
			return new Vector3(left) * right;
		}

		/// <summary>Divides the first vector by the second.</summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The vector resulting from the division.</returns>

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector3 operator /(Vector3 left, Vector3 right) {
			return new Vector3(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
		}

		/// <summary>Divides the vector by the given scalar.</summary>
		/// <param name="value1">The source vector.</param>
		/// <param name="value2">The scalar value.</param>
		/// <returns>The result of the division.</returns>

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector3 operator /(Vector3 value1, float value2) {
			float num = 1f / value2;
			return new Vector3(value1.X * num, value1.Y * num, value1.Z * num);
		}

		/// <summary>Negates a given vector.</summary>
		/// <param name="value">The source vector.</param>
		/// <returns>The negated vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector3 operator -(Vector3 value) {
			return Vector3.Zero - value;
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
		public static bool operator ==(Vector3 left, Vector3 right) {
			if ((double) left.X == (double) right.X && (double) left.Y == (double) right.Y)
				return (double) left.Z == (double) right.Z;
			return false;
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
		public static bool operator !=(Vector3 left, Vector3 right) {
			if ((double) left.X == (double) right.X && (double) left.Y == (double) right.Y)
				return (double) left.Z != (double) right.Z;
			return true;
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>The hash code.</returns>
		public override int GetHashCode() {
			return Helper.CombineHashCodes(Helper.CombineHashCodes(this.X.GetHashCode(), this.Y.GetHashCode()), this.Z.GetHashCode());
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Object is equal to this Vector3 instance.
		/// </summary>
		/// <param name="obj">The Object to compare against.</param>
		/// <returns>True if the Object is equal to this Vector3; False otherwise.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public override bool Equals(object obj) {
			if (!(obj is Vector3))
				return false;
			return this.Equals((Vector3) obj);
		}

		/// <summary>Returns a String representing this Vector3 instance.</summary>
		/// <returns>The string representation.</returns>
		public override string ToString() {
			return this.ToString("G", (IFormatProvider) CultureInfo.CurrentCulture);
		}

		/// <summary>
		/// Returns a String representing this Vector3 instance, using the specified format to format individual elements.
		/// </summary>
		/// <param name="format">The format of individual elements.</param>
		/// <returns>The string representation.</returns>
		public string ToString(string format) {
			return this.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture);
		}

		/// <summary>
		/// Returns a String representing this Vector3 instance, using the specified format to format individual elements
		/// and the given IFormatProvider.
		/// </summary>
		/// <param name="format">The format of individual elements.</param>
		/// <param name="formatProvider">The format provider to use when formatting elements.</param>
		/// <returns>The string representation.</returns>
		public string ToString(string format, IFormatProvider formatProvider) {
			StringBuilder stringBuilder = new StringBuilder();
			string numberGroupSeparator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
			stringBuilder.Append('<');
			stringBuilder.Append(((IFormattable) this.X).ToString(format, formatProvider));
			stringBuilder.Append(numberGroupSeparator);
			stringBuilder.Append(' ');
			stringBuilder.Append(((IFormattable) this.Y).ToString(format, formatProvider));
			stringBuilder.Append(numberGroupSeparator);
			stringBuilder.Append(' ');
			stringBuilder.Append(((IFormattable) this.Z).ToString(format, formatProvider));
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
				return (float) Math.Sqrt((double) Vector3.Dot(this, this));
			return (float) Math.Sqrt((double) this.X * (double) this.X + (double) this.Y * (double) this.Y + (double) this.Z * (double) this.Z);
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
				return Vector3.Dot(this, this);
			return (float) ((double) this.X * (double) this.X + (double) this.Y * (double) this.Y + (double) this.Z * (double) this.Z);
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
		public static float Distance(Vector3 value1, Vector3 value2) {
			if (Vector.IsHardwareAccelerated) {
				Vector3 vector3 = value1 - value2;
				return (float) Math.Sqrt((double) Vector3.Dot(vector3, vector3));
			}
			float num1 = value1.X - value2.X;
			float num2 = value1.Y - value2.Y;
			float num3 = value1.Z - value2.Z;
			return (float) Math.Sqrt((double) num1 * (double) num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3);
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
		public static float DistanceSquared(Vector3 value1, Vector3 value2) {
			if (Vector.IsHardwareAccelerated) {
				Vector3 vector3 = value1 - value2;
				return Vector3.Dot(vector3, vector3);
			}
			float num1 = value1.X - value2.X;
			float num2 = value1.Y - value2.Y;
			float num3 = value1.Z - value2.Z;
			return (float) ((double) num1 * (double) num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3);
		}

		/// <summary>
		/// Returns a vector with the same direction as the given vector, but with a length of 1.
		/// </summary>
		/// <param name="value">The vector to normalize.</param>
		/// <returns>The normalized vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector3 Normalize(Vector3 value) {
			if (Vector.IsHardwareAccelerated) {
				float num = value.Length();
				return value / num;
			}
			float num1 = (float) Math.Sqrt((double) value.X * (double) value.X + (double) value.Y * (double) value.Y + (double) value.Z * (double) value.Z);
			return new Vector3(value.X / num1, value.Y / num1, value.Z / num1);
		}

		/// <summary>Computes the cross product of two vectors.</summary>
		/// <param name="vector1">The first vector.</param>
		/// <param name="vector2">The second vector.</param>
		/// <returns>The cross product.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector3 Cross(Vector3 vector1, Vector3 vector2) {
			return new Vector3((float) ((double) vector1.Y * (double) vector2.Z - (double) vector1.Z * (double) vector2.Y), (float) ((double) vector1.Z * (double) vector2.X - (double) vector1.X * (double) vector2.Z), (float) ((double) vector1.X * (double) vector2.Y - (double) vector1.Y * (double) vector2.X));
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
		public static Vector3 Reflect(Vector3 vector, Vector3 normal) {
			if (Vector.IsHardwareAccelerated) {
				float num = Vector3.Dot(vector, normal);
				Vector3 vector3 = normal * num * 2f;
				return vector - vector3;
			}
			float num1 = (float) ((double) vector.X * (double) normal.X + (double) vector.Y * (double) normal.Y + (double) vector.Z * (double) normal.Z);
			float num2 = (float) ((double) normal.X * (double) num1 * 2.0);
			float num3 = (float) ((double) normal.Y * (double) num1 * 2.0);
			float num4 = (float) ((double) normal.Z * (double) num1 * 2.0);
			return new Vector3(vector.X - num2, vector.Y - num3, vector.Z - num4);
		}

		/// <summary>Restricts a vector between a min and max value.</summary>
		/// <param name="value1">The source vector.</param>
		/// <param name="min">The minimum value.</param>
		/// <param name="max">The maximum value.</param>
		/// <returns>The restricted vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector3 Clamp(Vector3 value1, Vector3 min, Vector3 max) {
			float x1 = value1.X;
			float num1 = (double) x1 > (double) max.X ? max.X : x1;
			float x2 = (double) num1 < (double) min.X ? min.X : num1;
			float y1 = value1.Y;
			float num2 = (double) y1 > (double) max.Y ? max.Y : y1;
			float y2 = (double) num2 < (double) min.Y ? min.Y : num2;
			float z1 = value1.Z;
			float num3 = (double) z1 > (double) max.Z ? max.Z : z1;
			float z2 = (double) num3 < (double) min.Z ? min.Z : num3;
			return new Vector3(x2, y2, z2);
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
		public static Vector3 Lerp(Vector3 value1, Vector3 value2, float amount) {
			if (Vector.IsHardwareAccelerated)
				return value1 * (1f - amount) + value2 * amount;
			return new Vector3(value1.X + (value2.X - value1.X) * amount, value1.Y + (value2.Y - value1.Y) * amount, value1.Z + (value2.Z - value1.Z) * amount);
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
		public static Vector3 Transform(Vector3 value, Quaternion rotation) {
			float num1 = rotation.X + rotation.X;
			float num2 = rotation.Y + rotation.Y;
			float num3 = rotation.Z + rotation.Z;
			float num4 = rotation.W * num1;
			float num5 = rotation.W * num2;
			float num6 = rotation.W * num3;
			float num7 = rotation.X * num1;
			float num8 = rotation.X * num2;
			float num9 = rotation.X * num3;
			float num10 = rotation.Y * num2;
			float num11 = rotation.Y * num3;
			float num12 = rotation.Z * num3;
			return new Vector3((float) ((double) value.X * (1.0 - (double) num10 - (double) num12) + (double) value.Y * ((double) num8 - (double) num6) + (double) value.Z * ((double) num9 + (double) num5)), (float) ((double) value.X * ((double) num8 + (double) num6) + (double) value.Y * (1.0 - (double) num7 - (double) num12) + (double) value.Z * ((double) num11 - (double) num4)), (float) ((double) value.X * ((double) num9 - (double) num5) + (double) value.Y * ((double) num11 + (double) num4) + (double) value.Z * (1.0 - (double) num7 - (double) num10)));
		}

		/// <summary>Adds two vectors together.</summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The summed vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector3 Add(Vector3 left, Vector3 right) {
			return left + right;
		}

		/// <summary>Subtracts the second vector from the first.</summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The difference vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector3 Subtract(Vector3 left, Vector3 right) {
			return left - right;
		}

		/// <summary>Multiplies two vectors together.</summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The product vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector3 Multiply(Vector3 left, Vector3 right) {
			return left * right;
		}

		/// <summary>Multiplies a vector by the given scalar.</summary>
		/// <param name="left">The source vector.</param>
		/// <param name="right">The scalar value.</param>
		/// <returns>The scaled vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector3 Multiply(Vector3 left, float right) {
			return left * right;
		}

		/// <summary>Multiplies a vector by the given scalar.</summary>
		/// <param name="left">The scalar value.</param>
		/// <param name="right">The source vector.</param>
		/// <returns>The scaled vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector3 Multiply(float left, Vector3 right) {
			return left * right;
		}

		/// <summary>Divides the first vector by the second.</summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The vector resulting from the division.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector3 Divide(Vector3 left, Vector3 right) {
			return left / right;
		}

		/// <summary>Divides the vector by the given scalar.</summary>
		/// <param name="left">The source vector.</param>
		/// <param name="divisor">The scalar value.</param>
		/// <returns>The result of the division.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector3 Divide(Vector3 left, float divisor) {
			return left / divisor;
		}

		/// <summary>Negates a given vector.</summary>
		/// <param name="value">The source vector.</param>
		/// <returns>The negated vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector3 Negate(Vector3 value) {
			return -value;
		}

		/// <summary>
		/// Copies the contents of the vector into the given array.
		/// </summary>
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

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void CopyTo(float[] array, int index) {
			if (index < 0 || index >= array.Length)
				throw new ArgumentOutOfRangeException("Index was out of bounds: " + index);
			if (array.Length - index < 3)
				throw new ArgumentException("Number of elements in source vector is greater than the destination array.");
			array[index] = this.X;
			array[index + 1] = this.Y;
			array[index + 2] = this.Z;
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Vector3 is equal to this Vector3 instance.
		/// </summary>
		/// <param name="other">The Vector3 to compare this instance to.</param>
		/// <returns>True if the other Vector3 is equal to this instance; False otherwise.</returns>

		public bool Equals(Vector3 other) {
			if ((double) this.X == (double) other.X && (double) this.Y == (double) other.Y)
				return (double) this.Z == (double) other.Z;
			return false;
		}

		/// <summary>Returns the dot product of two vectors.</summary>
		/// <param name="vector1">The first vector.</param>
		/// <param name="vector2">The second vector.</param>
		/// <returns>The dot product.</returns>

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static float Dot(Vector3 vector1, Vector3 vector2) {
			return (float) ((double) vector1.X * (double) vector2.X + (double) vector1.Y * (double) vector2.Y + (double) vector1.Z * (double) vector2.Z);
		}

		/// <summary>
		/// Returns a vector whose elements are the minimum of each of the pairs of elements in the two source vectors.
		/// </summary>
		/// <param name="value1">The first source vector.</param>
		/// <param name="value2">The second source vector.</param>
		/// <returns>The minimized vector.</returns>

		public static Vector3 Min(Vector3 value1, Vector3 value2) {
			return new Vector3((double) value1.X < (double) value2.X ? value1.X : value2.X, (double) value1.Y < (double) value2.Y ? value1.Y : value2.Y, (double) value1.Z < (double) value2.Z ? value1.Z : value2.Z);
		}

		/// <summary>
		/// Returns a vector whose elements are the maximum of each of the pairs of elements in the two source vectors.
		/// </summary>
		/// <param name="value1">The first source vector.</param>
		/// <param name="value2">The second source vector.</param>
		/// <returns>The maximized vector.</returns>

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector3 Max(Vector3 value1, Vector3 value2) {
			return new Vector3((double) value1.X > (double) value2.X ? value1.X : value2.X, (double) value1.Y > (double) value2.Y ? value1.Y : value2.Y, (double) value1.Z > (double) value2.Z ? value1.Z : value2.Z);
		}

		/// <summary>
		/// Returns a vector whose elements are the absolute values of each of the source vector's elements.
		/// </summary>
		/// <param name="value">The source vector.</param>
		/// <returns>The absolute value vector.</returns>

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector3 Abs(Vector3 value) {
			return new Vector3(Math.Abs(value.X), Math.Abs(value.Y), Math.Abs(value.Z));
		}

		/// <summary>
		/// Returns a vector whose elements are the square root of each of the source vector's elements.
		/// </summary>
		/// <param name="value">The source vector.</param>
		/// <returns>The square root vector.</returns>

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector3 SquareRoot(Vector3 value) {
			return new Vector3((float) Math.Sqrt((double) value.X), (float) Math.Sqrt((double) value.Y), (float) Math.Sqrt((double) value.Z));
		}
	}
}