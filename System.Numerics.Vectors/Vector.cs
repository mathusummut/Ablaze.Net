using System.Runtime.CompilerServices;

namespace System.Numerics {
	/// <summary>
	/// Contains various methods useful for creating, manipulating, combining, and converting generic vectors with one another.
	/// </summary>
	public static class Vector {
		/// <summary>
		/// Returns whether or not vector operations are subject to hardware acceleration through JIT intrinsic support.
		/// </summary>
		public static bool IsHardwareAccelerated {
			get {
				return false;
			}
		}

		/// <summary>
		/// Creates a new vector with elements selected between the two given source vectors, and based on a mask vector.
		/// </summary>
		/// <param name="condition">The integral mask vector used to drive selection.</param>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The new vector with elements selected based on the mask.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<float> ConditionalSelect(Vector<int> condition, Vector<float> left, Vector<float> right) {
			return Vector<float>.ConditionalSelect((Vector<float>) condition, left, right);
		}

		/// <summary>
		/// Creates a new vector with elements selected between the two given source vectors, and based on a mask vector.
		/// </summary>
		/// <param name="condition">The integral mask vector used to drive selection.</param>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The new vector with elements selected based on the mask.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<double> ConditionalSelect(Vector<long> condition, Vector<double> left, Vector<double> right) {
			return Vector<double>.ConditionalSelect((Vector<double>) condition, left, right);
		}

		/// <summary>
		/// Creates a new vector with elements selected between the two given source vectors, and based on a mask vector.
		/// </summary>
		/// <param name="condition">The mask vector used to drive selection.</param>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The new vector with elements selected based on the mask.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<T> ConditionalSelect<T>(Vector<T> condition, Vector<T> left, Vector<T> right) where T : struct {
			return Vector<T>.ConditionalSelect(condition, left, right);
		}

		/// <summary>
		/// Returns a new vector whose elements signal whether the elements in left and right were equal.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>The resultant vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<T> Equals<T>(Vector<T> left, Vector<T> right) where T : struct {
			return Vector<T>.Equals(left, right);
		}

		/// <summary>
		/// Returns an integral vector whose elements signal whether elements in the left and right floating point vectors were equal.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>The resultant vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<int> Equals(Vector<float> left, Vector<float> right) {
			return (Vector<int>) Vector<float>.Equals(left, right);
		}

		/// <summary>
		/// Returns a new vector whose elements signal whether the elements in left and right were equal.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>The resultant vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<int> Equals(Vector<int> left, Vector<int> right) {
			return Vector<int>.Equals(left, right);
		}

		/// <summary>
		/// Returns an integral vector whose elements signal whether elements in the left and right floating point vectors were equal.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>The resultant vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<long> Equals(Vector<double> left, Vector<double> right) {
			return (Vector<long>) Vector<double>.Equals(left, right);
		}

		/// <summary>
		/// Returns a new vector whose elements signal whether the elements in left and right were equal.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>The resultant vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<long> Equals(Vector<long> left, Vector<long> right) {
			return Vector<long>.Equals(left, right);
		}

		/// <summary>
		/// Returns a boolean indicating whether each pair of elements in the given vectors are equal.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The first vector to compare.</param>
		/// <returns>True if all elements are equal; False otherwise.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool EqualsAll<T>(Vector<T> left, Vector<T> right) where T : struct {
			return left == right;
		}

		/// <summary>
		/// Returns a boolean indicating whether any single pair of elements in the given vectors are equal.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>True if any element pairs are equal; False if no element pairs are equal.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool EqualsAny<T>(Vector<T> left, Vector<T> right) where T : struct {
			return !Vector<T>.Equals(left, right).Equals(Vector<T>.Zero);
		}

		/// <summary>
		/// Returns a new vector whose elements signal whether the elements in left were less than their
		/// corresponding elements in right.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>The resultant vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<T> LessThan<T>(Vector<T> left, Vector<T> right) where T : struct {
			return Vector<T>.LessThan(left, right);
		}

		/// <summary>
		/// Returns an integral vector whose elements signal whether the elements in left were less than their
		/// corresponding elements in right.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>The resultant integral vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<int> LessThan(Vector<float> left, Vector<float> right) {
			return (Vector<int>) Vector<float>.LessThan(left, right);
		}

		/// <summary>
		/// Returns a new vector whose elements signal whether the elements in left were less than their
		/// corresponding elements in right.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>The resultant vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<int> LessThan(Vector<int> left, Vector<int> right) {
			return Vector<int>.LessThan(left, right);
		}

		/// <summary>
		/// Returns an integral vector whose elements signal whether the elements in left were less than their
		/// corresponding elements in right.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>The resultant integral vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<long> LessThan(Vector<double> left, Vector<double> right) {
			return (Vector<long>) Vector<double>.LessThan(left, right);
		}

		/// <summary>
		/// Returns a new vector whose elements signal whether the elements in left were less than their
		/// corresponding elements in right.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>The resultant vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<long> LessThan(Vector<long> left, Vector<long> right) {
			return Vector<long>.LessThan(left, right);
		}

		/// <summary>
		/// Returns a boolean indicating whether all of the elements in left are less than their corresponding elements in right.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>True if all elements in left are less than their corresponding elements in right; False otherwise.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool LessThanAll<T>(Vector<T> left, Vector<T> right) where T : struct {
			return ((Vector<int>) Vector<T>.LessThan(left, right)).Equals(Vector<int>.AllOnes);
		}

		/// <summary>
		/// Returns a boolean indicating whether any element in left is less than its corresponding element in right.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>True if any elements in left are less than their corresponding elements in right; False otherwise.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool LessThanAny<T>(Vector<T> left, Vector<T> right) where T : struct {
			return !((Vector<int>) Vector<T>.LessThan(left, right)).Equals(Vector<int>.Zero);
		}

		/// <summary>
		/// Returns a new vector whose elements signal whether the elements in left were less than or equal to their
		/// corresponding elements in right.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>The resultant vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<T> LessThanOrEqual<T>(Vector<T> left, Vector<T> right) where T : struct {
			return Vector<T>.LessThanOrEqual(left, right);
		}

		/// <summary>
		/// Returns an integral vector whose elements signal whether the elements in left were less than or equal to their
		/// corresponding elements in right.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>The resultant integral vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<int> LessThanOrEqual(Vector<float> left, Vector<float> right) {
			return (Vector<int>) Vector<float>.LessThanOrEqual(left, right);
		}

		/// <summary>
		/// Returns a new vector whose elements signal whether the elements in left were less than or equal to their
		/// corresponding elements in right.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>The resultant vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<int> LessThanOrEqual(Vector<int> left, Vector<int> right) {
			return Vector<int>.LessThanOrEqual(left, right);
		}

		/// <summary>
		/// Returns a new vector whose elements signal whether the elements in left were less than or equal to their
		/// corresponding elements in right.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>The resultant vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<long> LessThanOrEqual(Vector<long> left, Vector<long> right) {
			return Vector<long>.LessThanOrEqual(left, right);
		}

		/// <summary>
		/// Returns an integral vector whose elements signal whether the elements in left were less than or equal to their
		/// corresponding elements in right.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>The resultant integral vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<long> LessThanOrEqual(Vector<double> left, Vector<double> right) {
			return (Vector<long>) Vector<double>.LessThanOrEqual(left, right);
		}

		/// <summary>
		/// Returns a boolean indicating whether all elements in left are less than or equal to their corresponding elements in right.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>True if all elements in left are less than or equal to their corresponding elements in right; False otherwise.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool LessThanOrEqualAll<T>(Vector<T> left, Vector<T> right) where T : struct {
			return ((Vector<int>) Vector<T>.LessThanOrEqual(left, right)).Equals(Vector<int>.AllOnes);
		}

		/// <summary>
		/// Returns a boolean indicating whether any element in left is less than or equal to its corresponding element in right.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>True if any elements in left are less than their corresponding elements in right; False otherwise.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool LessThanOrEqualAny<T>(Vector<T> left, Vector<T> right) where T : struct {
			return !((Vector<int>) Vector<T>.LessThanOrEqual(left, right)).Equals(Vector<int>.Zero);
		}

		/// <summary>
		/// Returns a new vector whose elements signal whether the elements in left were greater than their
		/// corresponding elements in right.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>The resultant vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<T> GreaterThan<T>(Vector<T> left, Vector<T> right) where T : struct {
			return Vector<T>.GreaterThan(left, right);
		}

		/// <summary>
		/// Returns an integral vector whose elements signal whether the elements in left were greater than their
		/// corresponding elements in right.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>The resultant integral vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<int> GreaterThan(Vector<float> left, Vector<float> right) {
			return (Vector<int>) Vector<float>.GreaterThan(left, right);
		}

		/// <summary>
		/// Returns a new vector whose elements signal whether the elements in left were greater than their
		/// corresponding elements in right.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>The resultant vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<int> GreaterThan(Vector<int> left, Vector<int> right) {
			return Vector<int>.GreaterThan(left, right);
		}

		/// <summary>
		/// Returns an integral vector whose elements signal whether the elements in left were greater than their
		/// corresponding elements in right.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>The resultant integral vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<long> GreaterThan(Vector<double> left, Vector<double> right) {
			return (Vector<long>) Vector<double>.GreaterThan(left, right);
		}

		/// <summary>
		/// Returns a new vector whose elements signal whether the elements in left were greater than their
		/// corresponding elements in right.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>The resultant vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<long> GreaterThan(Vector<long> left, Vector<long> right) {
			return Vector<long>.GreaterThan(left, right);
		}

		/// <summary>
		/// Returns a boolean indicating whether all elements in left are greater than the corresponding elements in right.
		/// elements in right.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>True if all elements in left are greater than their corresponding elements in right; False otherwise.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool GreaterThanAll<T>(Vector<T> left, Vector<T> right) where T : struct {
			return ((Vector<int>) Vector<T>.GreaterThan(left, right)).Equals(Vector<int>.AllOnes);
		}

		/// <summary>
		/// Returns a boolean indicating whether any element in left is greater than its corresponding element in right.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>True if any elements in left are greater than their corresponding elements in right; False otherwise.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool GreaterThanAny<T>(Vector<T> left, Vector<T> right) where T : struct {
			return !((Vector<int>) Vector<T>.GreaterThan(left, right)).Equals(Vector<int>.Zero);
		}

		/// <summary>
		/// Returns a new vector whose elements signal whether the elements in left were greater than or equal to their
		/// corresponding elements in right.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>The resultant vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<T> GreaterThanOrEqual<T>(Vector<T> left, Vector<T> right) where T : struct {
			return Vector<T>.GreaterThanOrEqual(left, right);
		}

		/// <summary>
		/// Returns an integral vector whose elements signal whether the elements in left were greater than or equal to their
		/// corresponding elements in right.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>The resultant integral vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<int> GreaterThanOrEqual(Vector<float> left, Vector<float> right) {
			return (Vector<int>) Vector<float>.GreaterThanOrEqual(left, right);
		}

		/// <summary>
		/// Returns a new vector whose elements signal whether the elements in left were greater than or equal to their
		/// corresponding elements in right.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>The resultant vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<int> GreaterThanOrEqual(Vector<int> left, Vector<int> right) {
			return Vector<int>.GreaterThanOrEqual(left, right);
		}

		/// <summary>
		/// Returns a new vector whose elements signal whether the elements in left were greater than or equal to their
		/// corresponding elements in right.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>The resultant vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<long> GreaterThanOrEqual(Vector<long> left, Vector<long> right) {
			return Vector<long>.GreaterThanOrEqual(left, right);
		}

		/// <summary>
		/// Returns an integral vector whose elements signal whether the elements in left were greater than or equal to
		/// their corresponding elements in right.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>The resultant integral vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<long> GreaterThanOrEqual(Vector<double> left, Vector<double> right) {
			return (Vector<long>) Vector<double>.GreaterThanOrEqual(left, right);
		}

		/// <summary>
		/// Returns a boolean indicating whether all of the elements in left are greater than or equal to
		/// their corresponding elements in right.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>True if all elements in left are greater than or equal to their corresponding elements in right; False otherwise.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool GreaterThanOrEqualAll<T>(Vector<T> left, Vector<T> right) where T : struct {
			return ((Vector<int>) Vector<T>.GreaterThanOrEqual(left, right)).Equals(Vector<int>.AllOnes);
		}

		/// <summary>
		/// Returns a boolean indicating whether any element in left is greater than or equal to its corresponding element in right.
		/// </summary>
		/// <param name="left">The first vector to compare.</param>
		/// <param name="right">The second vector to compare.</param>
		/// <returns>True if any elements in left are greater than or equal to their corresponding elements in right; False otherwise.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool GreaterThanOrEqualAny<T>(Vector<T> left, Vector<T> right) where T : struct {
			return !((Vector<int>) Vector<T>.GreaterThanOrEqual(left, right)).Equals(Vector<int>.Zero);
		}

		/// <summary>
		/// Returns a new vector whose elements are the absolute values of the given vector's elements.
		/// </summary>
		/// <param name="value">The source vector.</param>
		/// <returns>The absolute value vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<T> Abs<T>(Vector<T> value) where T : struct {
			return Vector<T>.Abs(value);
		}

		/// <summary>
		/// Returns a new vector whose elements are the minimum of each pair of elements in the two given vectors.
		/// </summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The minimum vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<T> Min<T>(Vector<T> left, Vector<T> right) where T : struct {
			return Vector<T>.Min(left, right);
		}

		/// <summary>
		/// Returns a new vector whose elements are the maximum of each pair of elements in the two given vectors.
		/// </summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The maximum vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<T> Max<T>(Vector<T> left, Vector<T> right) where T : struct {
			return Vector<T>.Max(left, right);
		}

		/// <summary>Returns the dot product of two vectors.</summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The dot product.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static T Dot<T>(Vector<T> left, Vector<T> right) where T : struct {
			return Vector<T>.DotProduct(left, right);
		}

		/// <summary>
		/// Returns a new vector whose elements are the square roots of the given vector's elements.
		/// </summary>
		/// <param name="value">The source vector.</param>
		/// <returns>The square root vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<T> SquareRoot<T>(Vector<T> value) where T : struct {
			return Vector<T>.SquareRoot(value);
		}

		/// <summary>
		/// Creates a new vector whose values are the sum of each pair of elements from the two given vectors.
		/// </summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The summed vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<T> Add<T>(Vector<T> left, Vector<T> right) where T : struct {
			return (Vector<T>) (left + right);
		}

		/// <summary>
		/// Creates a new vector whose values are the difference between each pairs of elements in the given vectors.
		/// </summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The difference vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<T> Subtract<T>(Vector<T> left, Vector<T> right) where T : struct {
			return (Vector<T>) (left - right);
		}

		/// <summary>
		/// Creates a new vector whose values are the product of each pair of elements from the two given vectors.
		/// </summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The summed vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<T> Multiply<T>(Vector<T> left, Vector<T> right) where T : struct {
			return (Vector<T>) (left * right);
		}

		/// <summary>
		/// Returns a new vector whose values are the values of the given vector each multiplied by a scalar value.
		/// </summary>
		/// <param name="left">The source vector.</param>
		/// <param name="right">The scalar factor.</param>
		/// <returns>The scaled vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<T> Multiply<T>(Vector<T> left, T right) where T : struct {
			return (Vector<T>) (left * right);
		}

		/// <summary>
		/// Returns a new vector whose values are the values of the given vector each multiplied by a scalar value.
		/// </summary>
		/// <param name="left">The scalar factor.</param>
		/// <param name="right">The source vector.</param>
		/// <returns>The scaled vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<T> Multiply<T>(T left, Vector<T> right) where T : struct {
			return (Vector<T>) (left * right);
		}

		/// <summary>
		/// Returns a new vector whose values are the result of dividing the first vector's elements
		/// by the corresponding elements in the second vector.
		/// </summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The divided vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<T> Divide<T>(Vector<T> left, Vector<T> right) where T : struct {
			return (Vector<T>) (left / right);
		}

		/// <summary>
		/// Returns a new vector whose elements are the given vector's elements negated.
		/// </summary>
		/// <param name="value">The source vector.</param>
		/// <returns>The negated vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<T> Negate<T>(Vector<T> value) where T : struct {
			return (Vector<T>) (-value);
		}

		/// <summary>
		/// Returns a new vector by performing a bitwise-and operation on each of the elements in the given vectors.
		/// </summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The resultant vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<T> BitwiseAnd<T>(Vector<T> left, Vector<T> right) where T : struct {
			return (Vector<T>) (left & right);
		}

		/// <summary>
		/// Returns a new vector by performing a bitwise-or operation on each of the elements in the given vectors.
		/// </summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The resultant vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<T> BitwiseOr<T>(Vector<T> left, Vector<T> right) where T : struct {
			return (Vector<T>) (left | right);
		}

		/// <summary>
		/// Returns a new vector whose elements are obtained by taking the one's complement of the given vector's elements.
		/// </summary>
		/// <param name="value">The source vector.</param>
		/// <returns>The one's complement vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<T> OnesComplement<T>(Vector<T> value) where T : struct {
			return (Vector<T>) ~value;
		}

		/// <summary>
		/// Returns a new vector by performing a bitwise-exclusive-or operation on each of the elements in the given vectors.
		/// </summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The resultant vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<T> Xor<T>(Vector<T> left, Vector<T> right) where T : struct {
			return (Vector<T>) (left ^ right);
		}

		/// <summary>
		/// Returns a new vector by performing a bitwise-and-not operation on each of the elements in the given vectors.
		/// </summary>
		/// <param name="left">The first source vector.</param>
		/// <param name="right">The second source vector.</param>
		/// <returns>The resultant vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<T> AndNot<T>(Vector<T> left, Vector<T> right) where T : struct {
			return (Vector<T>) (left & (Vector<T>) ~right);
		}

		/// <summary>
		/// Reinterprets the bits of the given vector into those of a vector of unsigned bytes.
		/// </summary>
		/// <param name="value">The source vector</param>
		/// <returns>The reinterpreted vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<byte> AsVectorByte<T>(Vector<T> value) where T : struct {
			return (Vector<byte>) value;
		}

		/// <summary>
		/// Reinterprets the bits of the given vector into those of a vector of signed bytes.
		/// </summary>
		/// <param name="value">The source vector</param>
		/// <returns>The reinterpreted vector.</returns>
		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<sbyte> AsVectorSByte<T>(Vector<T> value) where T : struct {
			return (Vector<sbyte>) value;
		}

		/// <summary>
		/// Reinterprets the bits of the given vector into those of a vector of 16-bit integers.
		/// </summary>
		/// <param name="value">The source vector</param>
		/// <returns>The reinterpreted vector.</returns>
		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<ushort> AsVectorUInt16<T>(Vector<T> value) where T : struct {
			return (Vector<ushort>) value;
		}

		/// <summary>
		/// Reinterprets the bits of the given vector into those of a vector of signed 16-bit integers.
		/// </summary>
		/// <param name="value">The source vector</param>
		/// <returns>The reinterpreted vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<short> AsVectorInt16<T>(Vector<T> value) where T : struct {
			return (Vector<short>) value;
		}

		/// <summary>
		/// Reinterprets the bits of the given vector into those of a vector of unsigned 32-bit integers.
		/// </summary>
		/// <param name="value">The source vector</param>
		/// <returns>The reinterpreted vector.</returns>
		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<uint> AsVectorUInt32<T>(Vector<T> value) where T : struct {
			return (Vector<uint>) value;
		}

		/// <summary>
		/// Reinterprets the bits of the given vector into those of a vector of signed 32-bit integers.
		/// </summary>
		/// <param name="value">The source vector</param>
		/// <returns>The reinterpreted vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<int> AsVectorInt32<T>(Vector<T> value) where T : struct {
			return (Vector<int>) value;
		}

		/// <summary>
		/// Reinterprets the bits of the given vector into those of a vector of unsigned 64-bit integers.
		/// </summary>
		/// <param name="value">The source vector</param>
		/// <returns>The reinterpreted vector.</returns>
		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<ulong> AsVectorUInt64<T>(Vector<T> value) where T : struct {
			return (Vector<ulong>) value;
		}

		/// <summary>
		/// Reinterprets the bits of the given vector into those of a vector of signed 64-bit integers.
		/// </summary>
		/// <param name="value">The source vector</param>
		/// <returns>The reinterpreted vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<long> AsVectorInt64<T>(Vector<T> value) where T : struct {
			return (Vector<long>) value;
		}

		/// <summary>
		/// Reinterprets the bits of the given vector into those of a vector of 32-bit floating point numbers.
		/// </summary>
		/// <param name="value">The source vector</param>
		/// <returns>The reinterpreted vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<float> AsVectorSingle<T>(Vector<T> value) where T : struct {
			return (Vector<float>) value;
		}

		/// <summary>
		/// Reinterprets the bits of the given vector into those of a vector of 64-bit floating point numbers.
		/// </summary>
		/// <param name="value">The source vector</param>
		/// <returns>The reinterpreted vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector<double> AsVectorDouble<T>(Vector<T> value) where T : struct {
			return (Vector<double>) value;
		}
	}
}