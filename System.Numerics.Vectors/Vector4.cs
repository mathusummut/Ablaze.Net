using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Numerics {
	/// <summary>
	/// A structure encapsulating four single precision floating point values and provides hardware accelerated methods.
	/// </summary>
	[Serializable]
	public struct Vector4 : IEquatable<Vector4>, IFormattable {
		/// <summary>The X component of the vector.</summary>
		public float X;
		/// <summary>The Y component of the vector.</summary>
		public float Y;
		/// <summary>The Z component of the vector.</summary>
		public float Z;
		/// <summary>The W component of the vector.</summary>
		public float W;

		/// <summary>Returns the vector (0,0,0,0).</summary>
		public static Vector4 Zero {
			get {
				return new Vector4();
			}
		}

		/// <summary>Returns the vector (1,1,1,1).</summary>
		public static Vector4 One {
			get {
				return new Vector4(1f, 1f, 1f, 1f);
			}
		}

		/// <summary>Returns the vector (1,0,0,0).</summary>
		public static Vector4 UnitX {
			get {
				return new Vector4(1f, 0.0f, 0.0f, 0.0f);
			}
		}

		/// <summary>Returns the vector (0,1,0,0).</summary>
		public static Vector4 UnitY {
			get {
				return new Vector4(0.0f, 1f, 0.0f, 0.0f);
			}
		}

		/// <summary>Returns the vector (0,0,1,0).</summary>
		public static Vector4 UnitZ {
			get {
				return new Vector4(0.0f, 0.0f, 1f, 0.0f);
			}
		}

		/// <summary>Returns the vector (0,0,0,1).</summary>
		public static Vector4 UnitW {
			get {
				return new Vector4(0.0f, 0.0f, 0.0f, 1f);
			}
		}

		/// <summary>
		/// Constructs a vector whose elements are all the single specified value.
		/// </summary>
		/// <param name="value">The element to fill the vector with.</param>

		public Vector4(float value) {
			this = new Vector4(value, value, value, value);
		}

		/// <summary>
		/// Constructs a vector with the given individual elements.
		/// </summary>
		/// <param name="w">W component.</param>
		/// <param name="x">X component.</param>
		/// <param name="y">Y component.</param>
		/// <param name="z">Z component.</param>

		public Vector4(float x, float y, float z, float w) {
			this.W = w;
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		/// <summary>
		/// Constructs a Vector4 from the given Vector2 and a Z and W component.
		/// </summary>
		/// <param name="value">The vector to use as the X and Y components.</param>
		/// <param name="z">The Z component.</param>
		/// <param name="w">The W component.</param>
		public Vector4(Vector2 value, float z, float w) {
			this.X = value.X;
			this.Y = value.Y;
			this.Z = z;
			this.W = w;
		}

		/// <summary>
		/// Constructs a Vector4 from the given Vector3 and a W component.
		/// </summary>
		/// <param name="value">The vector to use as the X, Y, and Z components.</param>
		/// <param name="w">The W component.</param>
		public Vector4(Vector3 value, float w) {
			this.X = value.X;
			this.Y = value.Y;
			this.Z = value.Z;
			this.W = w;
		}

		/// <summary>Adds two vectors together.</summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The summed vector.</returns>

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector4 operator +(Vector4 left, Vector4 right) {
			return new Vector4(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
		}

		/// <summary>Subtracts the second vector from the first.</summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The difference vector.</returns>

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector4 operator -(Vector4 left, Vector4 right) {
			return new Vector4(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
		}

		/// <summary>Multiplies two vectors together.</summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The product vector.</returns>

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector4 operator *(Vector4 left, Vector4 right) {
			return new Vector4(left.X * right.X, left.Y * right.Y, left.Z * right.Z, left.W * right.W);
		}

		/// <summary>Multiplies a vector by the given scalar.</summary>
		/// <param name="left">The source vector.</param>
		/// <param name="right">The scalar value.</param>
		/// <returns>The scaled vector.</returns>

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector4 operator *(Vector4 left, float right) {
			return left * new Vector4(right);
		}

		/// <summary>Multiplies a vector by the given scalar.</summary>
		/// <param name="left">The scalar value.</param>
		/// <param name="right">The source vector.</param>
		/// <returns>The scaled vector.</returns>

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector4 operator *(float left, Vector4 right) {
			return new Vector4(left) * right;
		}

		/// <summary>Divides the first vector by the second.</summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The vector resulting from the division.</returns>

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector4 operator /(Vector4 left, Vector4 right) {
			return new Vector4(left.X / right.X, left.Y / right.Y, left.Z / right.Z, left.W / right.W);
		}

		/// <summary>Divides the vector by the given scalar.</summary>
		/// <param name="value1">The source vector.</param>
		/// <param name="value2">The scalar value.</param>
		/// <returns>The result of the division.</returns>

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector4 operator /(Vector4 value1, float value2) {
			float num = 1f / value2;
			return new Vector4(value1.X * num, value1.Y * num, value1.Z * num, value1.W * num);
		}

		/// <summary>Negates a given vector.</summary>
		/// <param name="value">The source vector.</param>
		/// <returns>The negated vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector4 operator -(Vector4 value) {
			return Vector4.Zero - value;
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
		public static bool operator ==(Vector4 left, Vector4 right) {
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
		public static bool operator !=(Vector4 left, Vector4 right) {
			return !(left == right);
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>The hash code.</returns>
		public override int GetHashCode() {
			return Helper.CombineHashCodes(Helper.CombineHashCodes(Helper.CombineHashCodes(this.X.GetHashCode(), this.Y.GetHashCode()), this.Z.GetHashCode()), this.W.GetHashCode());
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Object is equal to this Vector4 instance.
		/// </summary>
		/// <param name="obj">The Object to compare against.</param>
		/// <returns>True if the Object is equal to this Vector4; False otherwise.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public override bool Equals(object obj) {
			if (!(obj is Vector4))
				return false;
			return this.Equals((Vector4) obj);
		}

		/// <summary>Returns a String representing this Vector4 instance.</summary>
		/// <returns>The string representation.</returns>
		public override string ToString() {
			return this.ToString("G", (IFormatProvider) CultureInfo.CurrentCulture);
		}

		/// <summary>
		/// Returns a String representing this Vector4 instance, using the specified format to format individual elements.
		/// </summary>
		/// <param name="format">The format of individual elements.</param>
		/// <returns>The string representation.</returns>
		public string ToString(string format) {
			return this.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture);
		}

		/// <summary>
		/// Returns a String representing this Vector4 instance, using the specified format to format individual elements
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
			stringBuilder.Append(numberGroupSeparator);
			stringBuilder.Append(' ');
			stringBuilder.Append(this.Z.ToString(format, formatProvider));
			stringBuilder.Append(numberGroupSeparator);
			stringBuilder.Append(' ');
			stringBuilder.Append(this.W.ToString(format, formatProvider));
			stringBuilder.Append('>');
			return stringBuilder.ToString();
		}

		/// <summary>
		/// Returns the length of the vector. This operation is cheaper than Length().
		/// </summary>
		/// <returns>The vector's length.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public float Length() {
			if (Vector.IsHardwareAccelerated)
				return (float) Math.Sqrt((double) Vector4.Dot(this, this));
			return (float) Math.Sqrt((double) this.X * (double) this.X + (double) this.Y * (double) this.Y + (double) this.Z * (double) this.Z + (double) this.W * (double) this.W);
		}

		/// <summary>Returns the length of the vector squared.</summary>
		/// <returns>The vector's length squared.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public float LengthSquared() {
			if (Vector.IsHardwareAccelerated)
				return Vector4.Dot(this, this);
			return (float) ((double) this.X * (double) this.X + (double) this.Y * (double) this.Y + (double) this.Z * (double) this.Z + (double) this.W * (double) this.W);
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
		public static float Distance(Vector4 value1, Vector4 value2) {
			if (Vector.IsHardwareAccelerated) {
				Vector4 vector4 = value1 - value2;
				return (float) Math.Sqrt((double) Vector4.Dot(vector4, vector4));
			}
			float num1 = value1.X - value2.X;
			float num2 = value1.Y - value2.Y;
			float num3 = value1.Z - value2.Z;
			float num4 = value1.W - value2.W;
			return (float) Math.Sqrt((double) num1 * (double) num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3 + (double) num4 * (double) num4);
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
		public static float DistanceSquared(Vector4 value1, Vector4 value2) {
			if (Vector.IsHardwareAccelerated) {
				Vector4 vector4 = value1 - value2;
				return Vector4.Dot(vector4, vector4);
			}
			float num1 = value1.X - value2.X;
			float num2 = value1.Y - value2.Y;
			float num3 = value1.Z - value2.Z;
			float num4 = value1.W - value2.W;
			return (float) ((double) num1 * (double) num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3 + (double) num4 * (double) num4);
		}

		/// <summary>
		/// Returns a vector with the same direction as the given vector, but with a length of 1.
		/// </summary>
		/// <param name="vector">The vector to normalize.</param>
		/// <returns>The normalized vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector4 Normalize(Vector4 vector) {
			if (Vector.IsHardwareAccelerated) {
				float num = vector.Length();
				return vector / num;
			}
			float num1 = 1f / (float) Math.Sqrt((double) vector.X * (double) vector.X + (double) vector.Y * (double) vector.Y + (double) vector.Z * (double) vector.Z + (double) vector.W * (double) vector.W);
			return new Vector4(vector.X * num1, vector.Y * num1, vector.Z * num1, vector.W * num1);
		}

		/// <summary>Restricts a vector between a min and max value.</summary>
		/// <param name="value1">The source vector.</param>
		/// <param name="min">The minimum value.</param>
		/// <param name="max">The maximum value.</param>
		/// <returns>The restricted vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector4 Clamp(Vector4 value1, Vector4 min, Vector4 max) {
			float x1 = value1.X;
			float num1 = (double) x1 > (double) max.X ? max.X : x1;
			float x2 = (double) num1 < (double) min.X ? min.X : num1;
			float y1 = value1.Y;
			float num2 = (double) y1 > (double) max.Y ? max.Y : y1;
			float y2 = (double) num2 < (double) min.Y ? min.Y : num2;
			float z1 = value1.Z;
			float num3 = (double) z1 > (double) max.Z ? max.Z : z1;
			float z2 = (double) num3 < (double) min.Z ? min.Z : num3;
			float w1 = value1.W;
			float num4 = (double) w1 > (double) max.W ? max.W : w1;
			float w2 = (double) num4 < (double) min.W ? min.W : num4;
			return new Vector4(x2, y2, z2, w2);
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
		public static Vector4 Lerp(Vector4 value1, Vector4 value2, float amount) {
			return new Vector4(value1.X + (value2.X - value1.X) * amount, value1.Y + (value2.Y - value1.Y) * amount, value1.Z + (value2.Z - value1.Z) * amount, value1.W + (value2.W - value1.W) * amount);
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
		public static Vector4 Transform(Vector2 value, Quaternion rotation) {
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
			return new Vector4((float) ((double) value.X * (1.0 - (double) num10 - (double) num12) + (double) value.Y * ((double) num8 - (double) num6)), (float) ((double) value.X * ((double) num8 + (double) num6) + (double) value.Y * (1.0 - (double) num7 - (double) num12)), (float) ((double) value.X * ((double) num9 - (double) num5) + (double) value.Y * ((double) num11 + (double) num4)), 1f);
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
		public static Vector4 Transform(Vector3 value, Quaternion rotation) {
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
			return new Vector4((float) ((double) value.X * (1.0 - (double) num10 - (double) num12) + (double) value.Y * ((double) num8 - (double) num6) + (double) value.Z * ((double) num9 + (double) num5)), (float) ((double) value.X * ((double) num8 + (double) num6) + (double) value.Y * (1.0 - (double) num7 - (double) num12) + (double) value.Z * ((double) num11 - (double) num4)), (float) ((double) value.X * ((double) num9 - (double) num5) + (double) value.Y * ((double) num11 + (double) num4) + (double) value.Z * (1.0 - (double) num7 - (double) num10)), 1f);
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
		public static Vector4 Transform(Vector4 value, Quaternion rotation) {
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
			return new Vector4((float) ((double) value.X * (1.0 - (double) num10 - (double) num12) + (double) value.Y * ((double) num8 - (double) num6) + (double) value.Z * ((double) num9 + (double) num5)), (float) ((double) value.X * ((double) num8 + (double) num6) + (double) value.Y * (1.0 - (double) num7 - (double) num12) + (double) value.Z * ((double) num11 - (double) num4)), (float) ((double) value.X * ((double) num9 - (double) num5) + (double) value.Y * ((double) num11 + (double) num4) + (double) value.Z * (1.0 - (double) num7 - (double) num10)), value.W);
		}

		/// <summary>Adds two vectors together.</summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The summed vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector4 Add(Vector4 left, Vector4 right) {
			return left + right;
		}

		/// <summary>Subtracts the second vector from the first.</summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The difference vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector4 Subtract(Vector4 left, Vector4 right) {
			return left - right;
		}

		/// <summary>Multiplies two vectors together.</summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The product vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector4 Multiply(Vector4 left, Vector4 right) {
			return left * right;
		}

		/// <summary>Multiplies a vector by the given scalar.</summary>
		/// <param name="left">The source vector.</param>
		/// <param name="right">The scalar value.</param>
		/// <returns>The scaled vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector4 Multiply(Vector4 left, float right) {
			return left * new Vector4(right, right, right, right);
		}

		/// <summary>Multiplies a vector by the given scalar.</summary>
		/// <param name="left">The scalar value.</param>
		/// <param name="right">The source vector.</param>
		/// <returns>The scaled vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector4 Multiply(float left, Vector4 right) {
			return new Vector4(left, left, left, left) * right;
		}

		/// <summary>Divides the first vector by the second.</summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The vector resulting from the division.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector4 Divide(Vector4 left, Vector4 right) {
			return left / right;
		}

		/// <summary>Divides the vector by the given scalar.</summary>
		/// <param name="left">The source vector.</param>
		/// <param name="divisor">The scalar value.</param>
		/// <returns>The result of the division.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector4 Divide(Vector4 left, float divisor) {
			return left / divisor;
		}

		/// <summary>Negates a given vector.</summary>
		/// <param name="value">The source vector.</param>
		/// <returns>The negated vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector4 Negate(Vector4 value) {
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
			if (array.Length - index < 4)
				throw new ArgumentException("Number of elements in source vector is greater than the destination array.");
			array[index] = this.X;
			array[index + 1] = this.Y;
			array[index + 2] = this.Z;
			array[index + 3] = this.W;
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Vector4 is equal to this Vector4 instance.
		/// </summary>
		/// <param name="other">The Vector4 to compare this instance to.</param>
		/// <returns>True if the other Vector4 is equal to this instance; False otherwise.</returns>

		public bool Equals(Vector4 other) {
			if ((double) this.X == (double) other.X && (double) this.Y == (double) other.Y && (double) this.Z == (double) other.Z)
				return (double) this.W == (double) other.W;
			return false;
		}

		/// <summary>Returns the dot product of two vectors.</summary>
		/// <param name="vector1">The first vector.</param>
		/// <param name="vector2">The second vector.</param>
		/// <returns>The dot product.</returns>

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static float Dot(Vector4 vector1, Vector4 vector2) {
			return (float) ((double) vector1.X * (double) vector2.X + (double) vector1.Y * (double) vector2.Y + (double) vector1.Z * (double) vector2.Z + (double) vector1.W * (double) vector2.W);
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
		public static Vector4 Min(Vector4 value1, Vector4 value2) {
			return new Vector4((double) value1.X < (double) value2.X ? value1.X : value2.X, (double) value1.Y < (double) value2.Y ? value1.Y : value2.Y, (double) value1.Z < (double) value2.Z ? value1.Z : value2.Z, (double) value1.W < (double) value2.W ? value1.W : value2.W);
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
		public static Vector4 Max(Vector4 value1, Vector4 value2) {
			return new Vector4((double) value1.X > (double) value2.X ? value1.X : value2.X, (double) value1.Y > (double) value2.Y ? value1.Y : value2.Y, (double) value1.Z > (double) value2.Z ? value1.Z : value2.Z, (double) value1.W > (double) value2.W ? value1.W : value2.W);
		}

		/// <summary>
		/// Returns a vector whose elements are the absolute values of each of the source vector's elements.
		/// </summary>
		/// <param name="value">The source vector.</param>
		/// <returns>The absolute value vector.</returns>

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector4 Abs(Vector4 value) {
			return new Vector4(Math.Abs(value.X), Math.Abs(value.Y), Math.Abs(value.Z), Math.Abs(value.W));
		}

		/// <summary>
		/// Returns a vector whose elements are the square root of each of the source vector's elements.
		/// </summary>
		/// <param name="value">The source vector.</param>
		/// <returns>The square root vector.</returns>

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector4 SquareRoot(Vector4 value) {
			return new Vector4((float) Math.Sqrt((double) value.X), (float) Math.Sqrt((double) value.Y), (float) Math.Sqrt((double) value.Z), (float) Math.Sqrt((double) value.W));
		}
	}
}