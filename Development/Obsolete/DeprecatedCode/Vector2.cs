using System.Runtime.InteropServices;

namespace System.Drawing {
	/// <summary>Represents a 2D vector using two single-precision floating-point numbers.</summary>
	/// <remarks>
	/// The Vector2 structure is suitable for interoperation with unmanaged code requiring two consecutive floats.
	/// </remarks>
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector2 : IEquatable<Vector2> {
		#region Fields

		/// <summary>
		/// The X component of the Vector2.
		/// </summary>
		public float X;

		/// <summary>
		/// The Y component of the Vector2.
		/// </summary>
		public float Y;

		/// <summary>
		/// Defines a unit-length Vector2 that points towards the X-axis.
		/// </summary>
		public static readonly Vector2 UnitX = new Vector2(1F, 0F);

		/// <summary>
		/// Defines a unit-length Vector2 that points towards the Y-axis.
		/// </summary>
		public static readonly Vector2 UnitY = new Vector2(0F, 1F);

		/// <summary>
		/// Defines a zero-length Vector2.
		/// </summary>
		public static readonly Vector2 Zero = new Vector2();

		/// <summary>
		/// Defines an instance with all components set to 1.
		/// </summary>
		public static readonly Vector2 One = new Vector2(1F);

		/// <summary>
		/// Defines the size of the Vector2 struct in bytes.
		/// </summary>
		public static readonly int SizeInBytes = Marshal.SizeOf(new Vector2());

		#endregion

		#region Constructors

		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		/// <param name="value">The value that will initialize this instance.</param>
		public Vector2(float value) {
			X = value;
			Y = value;
		}

		/// <summary>
		/// Constructs a new Vector2.
		/// </summary>
		/// <param name="x">The x coordinate of the net Vector2.</param>
		/// <param name="y">The y coordinate of the net Vector2.</param>
		public Vector2(float x, float y) {
			X = x;
			Y = y;
		}

		#endregion

		#region Public Members

		#region Instance

		#region public float Length

		/// <summary>
		/// Gets the length (magnitude) of the vector.
		/// </summary>
		/// <see cref="LengthFast"/>
		/// <seealso cref="LengthSquared"/>
		public float Length {
			get {
				return (float) System.Math.Sqrt(X * X + Y * Y);
			}
		}

		#endregion

		#region public float LengthFast

		/// <summary>
		/// Gets an approximation of the vector length (magnitude).
		/// </summary>
		/// <remarks>
		/// This property uses an approximation of the square root function to calculate vector magnitude, with
		/// an upper error bound of 0.001.
		/// </remarks>
		/// <see cref="Length"/>
		/// <seealso cref="LengthSquared"/>
		public float LengthFast {
			get {
				return 1f / MathHelper.InverseSqrtFast(X * X + Y * Y);
			}
		}

		#endregion

		#region public float LengthSquared

		/// <summary>
		/// Gets the square of the vector length (magnitude).
		/// </summary>
		/// <remarks>
		/// This property avoids the costly square root operation required by the Length property. This makes it more suitable
		/// for comparisons.
		/// </remarks>
		/// <see cref="Length"/>
		/// <seealso cref="LengthFast"/>
		public float LengthSquared {
			get {
				return X * X + Y * Y;
			}
		}

		#endregion

		#region public Vector2 PerpendicularRight

		/// <summary>
		/// Gets the perpendicular vector on the right side of this vector.
		/// </summary>
		public Vector2 PerpendicularRight {
			get {
				return new Vector2(Y, -X);
			}
		}

		#endregion

		#region public Vector2 PerpendicularLeft

		/// <summary>
		/// Gets the perpendicular vector on the left side of this vector.
		/// </summary>
		public Vector2 PerpendicularLeft {
			get {
				return new Vector2(-Y, X);
			}
		}

		#endregion

		/// <summary>
		/// Gets a Vector2 like this one but with X and Y swapped.
		/// </summary>
		public Vector2 SwappedComponents {
			get {
				return new Vector2(Y, X);
			}
		}

		#region public void Normalize()

		/// <summary>
		/// Scales the Vector2 to unit length.
		/// </summary>
		public void Normalize() {
			if (X == 0f && Y == 0f)
				return;
			float scale = 1f / this.Length;
			X *= scale;
			Y *= scale;
		}

		#endregion

		#region public void NormalizeFast()

		/// <summary>
		/// Scales the Vector2 to approximately unit length.
		/// </summary>
		public void NormalizeFast() {
			float scale = MathHelper.InverseSqrtFast(X * X + Y * Y);
			X *= scale;
			Y *= scale;
		}

		#endregion

		#endregion

		#region Static

		#region Add

		/// <summary>
		/// Adds two vectors.
		/// </summary>
		/// <param name="a">Left operand.</param>
		/// <param name="b">Right operand.</param>
		/// <returns>Result of operation.</returns>
		public static Vector2 Add(Vector2 a, Vector2 b) {
			Add(ref a, ref b, out a);
			return a;
		}

		/// <summary>
		/// Adds two vectors.
		/// </summary>
		/// <param name="a">Left operand.</param>
		/// <param name="b">Right operand.</param>
		/// <param name="result">Result of operation.</param>
		public static void Add(ref Vector2 a, ref Vector2 b, out Vector2 result) {
			result = new Vector2(a.X + b.X, a.Y + b.Y);
		}

		/// <summary>
		/// Adds a scalar to a vector.
		/// </summary>
		/// <param name="a">Left operand.</param>
		/// <param name="b">Right operand.</param>
		/// <returns>Result of operation.</returns>
		public static Vector2 AddScalar(Vector2 a, float b) {
			a.X += b;
			a.Y += b;
			return a;
		}

		/// <summary>
		/// Adds a scalar to a vector.
		/// </summary>
		/// <param name="a">Left operand.</param>
		/// <param name="b">Right operand.</param>
		/// <returns>Result of operation.</returns>
		public static Vector2 AddScalar(float b, Vector2 a) {
			a.X += b;
			a.Y += b;
			return a;
		}

		/// <summary>
		/// Multiplies a scalar to a vector.
		/// </summary>
		/// <param name="a">Left operand.</param>
		/// <param name="b">Right operand.</param>
		/// <returns>Result of operation.</returns>
		public static Vector2 MultScalar(Vector2 a, float b) {
			a.X *= b;
			a.Y *= b;
			return a;
		}

		/// <summary>
		/// Multiplies a scalar to a vector.
		/// </summary>
		/// <param name="a">Left operand.</param>
		/// <param name="b">Right operand.</param>
		/// <returns>Result of operation.</returns>
		public static Vector2 MultScalar(float b, Vector2 a) {
			a.X *= b;
			a.Y *= b;
			return a;
		}

		/// <summary>
		/// Divides a vector by a scalar.
		/// </summary>
		/// <param name="a">Left operand.</param>
		/// <param name="b">Right operand.</param>
		/// <returns>Result of operation.</returns>
		public static Vector2 DivScalar(Vector2 a, float b) {
			a.X /= b;
			a.Y /= b;
			return a;
		}

		#endregion

		#region Subtract

		/// <summary>
		/// Subtract one Vector from another
		/// </summary>
		/// <param name="a">First operand</param>
		/// <param name="b">Second operand</param>
		/// <returns>Result of subtraction</returns>
		public static Vector2 Subtract(Vector2 a, Vector2 b) {
			Subtract(ref a, ref b, out a);
			return a;
		}

		/// <summary>
		/// Subtract one Vector from another
		/// </summary>
		/// <param name="a">First operand</param>
		/// <param name="b">Second operand</param>
		/// <param name="result">Result of subtraction</param>
		public static void Subtract(ref Vector2 a, ref Vector2 b, out Vector2 result) {
			result = new Vector2(a.X - b.X, a.Y - b.Y);
		}

		#endregion

		#region Multiply

		/// <summary>
		/// Multiplies a vector by a scalar.
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <returns>Result of the operation.</returns>
		public static Vector2 Multiply(Vector2 vector, float scale) {
			Multiply(ref vector, scale, out vector);
			return vector;
		}

		/// <summary>
		/// Multiplies a vector by a scalar.
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <param name="result">Result of the operation.</param>
		public static void Multiply(ref Vector2 vector, float scale, out Vector2 result) {
			result = new Vector2(vector.X * scale, vector.Y * scale);
		}

		/// <summary>
		/// Multiplies a vector by the components a vector (scale).
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <returns>Result of the operation.</returns>
		public static Vector2 Multiply(Vector2 vector, Vector2 scale) {
			Multiply(ref vector, ref scale, out vector);
			return vector;
		}

		/// <summary>
		/// Multiplies a vector by the components of a vector (scale).
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <param name="result">Result of the operation.</param>
		public static void Multiply(ref Vector2 vector, ref Vector2 scale, out Vector2 result) {
			result = new Vector2(vector.X * scale.X, vector.Y * scale.Y);
		}

		#endregion

		#region Divide

		/// <summary>
		/// Divides a vector by a scalar.
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <returns>Result of the operation.</returns>
		public static Vector2 Divide(Vector2 vector, float scale) {
			Divide(ref vector, scale, out vector);
			return vector;
		}

		/// <summary>
		/// Divides a vector by a scalar.
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <param name="result">Result of the operation.</param>
		public static void Divide(ref Vector2 vector, float scale, out Vector2 result) {
			Multiply(ref vector, 1 / scale, out result);
		}

		/// <summary>
		/// Divides a vector by the components of a vector (scale).
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <returns>Result of the operation.</returns>
		public static Vector2 Divide(Vector2 vector, Vector2 scale) {
			Divide(ref vector, ref scale, out vector);
			return vector;
		}

		/// <summary>
		/// Divide a vector by the components of a vector (scale).
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <param name="result">Result of the operation.</param>
		public static void Divide(ref Vector2 vector, ref Vector2 scale, out Vector2 result) {
			result = new Vector2(vector.X / scale.X, vector.Y / scale.Y);
		}

		#endregion

		#region ComponentMin

		/// <summary>
		/// Calculate the component-wise minimum of two vectors
		/// </summary>
		/// <param name="a">First operand</param>
		/// <param name="b">Second operand</param>
		/// <returns>The component-wise minimum</returns>
		public static Vector2 ComponentMin(Vector2 a, Vector2 b) {
			a.X = a.X < b.X ? a.X : b.X;
			a.Y = a.Y < b.Y ? a.Y : b.Y;
			return a;
		}

		/// <summary>
		/// Calculate the component-wise minimum of two vectors
		/// </summary>
		/// <param name="a">First operand</param>
		/// <param name="b">Second operand</param>
		/// <param name="result">The component-wise minimum</param>
		public static void ComponentMin(ref Vector2 a, ref Vector2 b, out Vector2 result) {
			result.X = a.X < b.X ? a.X : b.X;
			result.Y = a.Y < b.Y ? a.Y : b.Y;
		}

		#endregion

		#region ComponentMax

		/// <summary>
		/// Calculate the component-wise maximum of two vectors
		/// </summary>
		/// <param name="a">First operand</param>
		/// <param name="b">Second operand</param>
		/// <returns>The component-wise maximum</returns>
		public static Vector2 ComponentMax(Vector2 a, Vector2 b) {
			a.X = a.X > b.X ? a.X : b.X;
			a.Y = a.Y > b.Y ? a.Y : b.Y;
			return a;
		}

		/// <summary>
		/// Calculate the component-wise maximum of two vectors
		/// </summary>
		/// <param name="a">First operand</param>
		/// <param name="b">Second operand</param>
		/// <param name="result">The component-wise maximum</param>
		public static void ComponentMax(ref Vector2 a, ref Vector2 b, out Vector2 result) {
			result.X = a.X > b.X ? a.X : b.X;
			result.Y = a.Y > b.Y ? a.Y : b.Y;
		}

		#endregion

		#region Min

		/// <summary>
		/// Returns the Vector3 with the minimum magnitude
		/// </summary>
		/// <param name="left">Left operand</param>
		/// <param name="right">Right operand</param>
		/// <returns>The minimum Vector3</returns>
		public static Vector2 Min(Vector2 left, Vector2 right) {
			return left.LengthSquared < right.LengthSquared ? left : right;
		}

		#endregion

		#region Max

		/// <summary>
		/// Returns the Vector3 with the minimum magnitude
		/// </summary>
		/// <param name="left">Left operand</param>
		/// <param name="right">Right operand</param>
		/// <returns>The minimum Vector3</returns>
		public static Vector2 Max(Vector2 left, Vector2 right) {
			return left.LengthSquared >= right.LengthSquared ? left : right;
		}

		#endregion

		#region Clamp

		/// <summary>
		/// Clamp a vector to the given minimum and maximum vectors
		/// </summary>
		/// <param name="vec">Input vector</param>
		/// <param name="min">Minimum vector</param>
		/// <param name="max">Maximum vector</param>
		/// <returns>The clamped vector</returns>
		public static Vector2 Clamp(Vector2 vec, Vector2 min, Vector2 max) {
			vec.X = vec.X < min.X ? min.X : vec.X > max.X ? max.X : vec.X;
			vec.Y = vec.Y < min.Y ? min.Y : vec.Y > max.Y ? max.Y : vec.Y;
			return vec;
		}

		/// <summary>
		/// Clamp a vector to the given minimum and maximum vectors
		/// </summary>
		/// <param name="vec">Input vector</param>
		/// <param name="min">Minimum vector</param>
		/// <param name="max">Maximum vector</param>
		/// <param name="result">The clamped vector</param>
		public static void Clamp(ref Vector2 vec, ref Vector2 min, ref Vector2 max, out Vector2 result) {
			result.X = vec.X < min.X ? min.X : vec.X > max.X ? max.X : vec.X;
			result.Y = vec.Y < min.Y ? min.Y : vec.Y > max.Y ? max.Y : vec.Y;
		}

		#endregion

		#region Normalize

		/// <summary>
		/// Scale a vector to unit length
		/// </summary>
		/// <param name="vec">The input vector</param>
		/// <returns>The normalized vector</returns>
		public static Vector2 Normalize(Vector2 vec) {
			if (vec == Zero)
				return Zero;
			float scale = 1f / vec.Length;
			vec.X *= scale;
			vec.Y *= scale;
			return vec;
		}

		#endregion

		#region NormalizeFast

		/// <summary>
		/// Scale a vector to approximately unit length
		/// </summary>
		/// <param name="vec">The input vector</param>
		/// <returns>The normalized vector</returns>
		public static Vector2 NormalizeFast(Vector2 vec) {
			float scale = MathHelper.InverseSqrtFast(vec.X * vec.X + vec.Y * vec.Y);
			vec.X *= scale;
			vec.Y *= scale;
			return vec;
		}

		/// <summary>
		/// Scale a vector to approximately unit length
		/// </summary>
		/// <param name="vec">The input vector</param>
		/// <param name="result">The normalized vector</param>
		public static void NormalizeFast(ref Vector2 vec, out Vector2 result) {
			float scale = MathHelper.InverseSqrtFast(vec.X * vec.X + vec.Y * vec.Y);
			result.X = vec.X * scale;
			result.Y = vec.Y * scale;
		}

		#endregion

		#region Dot

		/// <summary>
		/// Calculate the dot (scalar) product of two vectors
		/// </summary>
		/// <param name="left">First operand</param>
		/// <param name="right">Second operand</param>
		/// <returns>The dot product of the two inputs</returns>
		public static float Dot(Vector2 left, Vector2 right) {
			return left.X * right.X + left.Y * right.Y;
		}

		/// <summary>
		/// Calculate the dot (scalar) product of two vectors
		/// </summary>
		/// <param name="left">First operand</param>
		/// <param name="right">Second operand</param>
		/// <param name="result">The dot product of the two inputs</param>
		public static void Dot(ref Vector2 left, ref Vector2 right, out float result) {
			result = left.X * right.X + left.Y * right.Y;
		}

		#endregion

		#region Lerp

		/// <summary>
		/// Returns a new Vector that is the linear blend of the 2 given Vectors
		/// </summary>
		/// <param name="a">First input vector</param>
		/// <param name="b">Second input vector</param>
		/// <param name="blend">The blend factor. a when blend=0, b when blend=1.</param>
		/// <returns>a when blend=0, b when blend=1, and a linear combination otherwise</returns>
		public static Vector2 Lerp(Vector2 a, Vector2 b, float blend) {
			a.X = blend * (b.X - a.X) + a.X;
			a.Y = blend * (b.Y - a.Y) + a.Y;
			return a;
		}

		/// <summary>
		/// Returns a new Vector that is the linear blend of the 2 given Vectors
		/// </summary>
		/// <param name="a">First input vector</param>
		/// <param name="b">Second input vector</param>
		/// <param name="blend">The blend factor. a when blend=0, b when blend=1.</param>
		/// <param name="result">a when blend=0, b when blend=1, and a linear combination otherwise</param>
		public static void Lerp(ref Vector2 a, ref Vector2 b, float blend, out Vector2 result) {
			result.X = blend * (b.X - a.X) + a.X;
			result.Y = blend * (b.Y - a.Y) + a.Y;
		}

		#endregion

		#region Barycentric

		/// <summary>
		/// Interpolate 3 Vectors using Barycentric coordinates
		/// </summary>
		/// <param name="a">First input Vector</param>
		/// <param name="b">Second input Vector</param>
		/// <param name="c">Third input Vector</param>
		/// <param name="u">First Barycentric Coordinate</param>
		/// <param name="v">Second Barycentric Coordinate</param>
		/// <returns>a when u=v=0, b when u=1,v=0, c when u=0,v=1, and a linear combination of a,b,c otherwise</returns>
		public static Vector2 BaryCentric(Vector2 a, Vector2 b, Vector2 c, float u, float v) {
			return a + MultScalar(u, (b - a)) + MultScalar(v, (c - a));
		}

		/// <summary>Interpolate 3 Vectors using Barycentric coordinates</summary>
		/// <param name="a">First input Vector.</param>
		/// <param name="b">Second input Vector.</param>
		/// <param name="c">Third input Vector.</param>
		/// <param name="u">First Barycentric Coordinate.</param>
		/// <param name="v">Second Barycentric Coordinate.</param>
		/// <param name="result">Output Vector. a when u=v=0, b when u=1,v=0, c when u=0,v=1, and a linear combination of a,b,c otherwise</param>
		public static void BaryCentric(ref Vector2 a, ref Vector2 b, ref Vector2 c, float u, float v, out Vector2 result) {
			result = a; // copy

			Vector2 temp = b; // copy
			Subtract(ref temp, ref a, out temp);
			Multiply(ref temp, u, out temp);
			Add(ref result, ref temp, out result);

			temp = c; // copy
			Subtract(ref temp, ref a, out temp);
			Multiply(ref temp, v, out temp);
			Add(ref result, ref temp, out result);
		}

		#endregion

		#region Transform

		/// <summary>
		/// Transforms a vector by a quaternion rotation.
		/// </summary>
		/// <param name="vec">The vector to transform.</param>
		/// <param name="quat">The quaternion to rotate the vector by.</param>
		/// <returns>The result of the operation.</returns>
		public static Vector2 Transform(Vector2 vec, Vector4 quat) {
			Vector2 result;
			Transform(ref vec, ref quat, out result);
			return result;
		}

		/// <summary>
		/// Transforms a vector by a quaternion rotation.
		/// </summary>
		/// <param name="vec">The vector to transform.</param>
		/// <param name="quat">The quaternion to rotate the vector by.</param>
		/// <param name="result">The result of the operation.</param>
		public static void Transform(ref Vector2 vec, ref Vector4 quat, out Vector2 result) {
			Vector4 v = new Vector4(vec.X, vec.Y, 0, 0), i, t;
			Vector4.Invert(ref quat, out i);
			Vector4.Multiply(ref quat, ref v, out t);
			Vector4.Multiply(ref t, ref i, out v);

			result = new Vector2(v.X, v.Y);
		}

		/// <summary>
		/// Returns the cross product of the specified vector or point.
		/// </summary>
		/// <param name="v">The vector to calculate the cross-product with.</param>
		public float CrossProduct(Vector2 v) {
			return (X * v.Y) - (Y * v.X);
		}

		/// <summary>
		/// Returns the distance between two points.
		/// </summary>
		/// <param name="v">The vector to calculate the distance from.</param>
		public float DistanceFrom(Vector2 v) {
			return (this - v).Length;
		}

		/// <summary>
		/// Returns the cross product of the specified vector or point.
		/// </summary>
		/// <param name="vector">The vector to calculate the cross-product with.</param>
		/// <param name="v">The vector to calculate the cross-product with.</param>
		public static float CrossProduct(Vector2 vector, Vector2 v) {
			return (vector.X * v.Y) - (vector.Y * v.X);
		}

		/// <summary>
		/// Returns the distance between two points.
		/// </summary>
		/// <param name="a">The starting point.</param>
		/// <param name="b">The vector to calculate the distance from.</param>
		public static float DistanceBetween(Vector2 a, Vector2 b) {
			return (a - b).Length;
		}

		/// <summary>
		/// Returns the distance between two points.
		/// </summary>
		/// <param name="a">The starting point.</param>
		/// <param name="b">The vector to calculate the distance from.</param>
		public static float DistanceBetweenFast(Vector2 a, Vector2 b) {
			return (a - b).LengthFast;
		}

		/// <summary>
		/// Returns the distance between two points faster that DistanceFrom() but less accurately.
		/// </summary>
		/// <param name="v">The vector to calculate the distance from.</param>
		public float DistanceFromFast(Vector2 v) {
			return (this - v).LengthFast;
		}

		/// <summary>
		/// Places the point about the specified axis at the specified angle.
		/// </summary>
		/// <param name="vector">The vector to set the angle with respect to it.</param>
		/// <param name="axis">The center point to use as origin.</param>
		/// <param name="angle">The angle to set in relation to the vector (not cumulative).</param>
		public static Vector2 SetAngleFrom(Vector2 vector, Vector2 axis, float angle) {
			return axis + ToMoveAt(vector.DistanceFrom(axis), angle);
		}

		/// <summary>
		/// Rotates the point about the specified axis at the specified angle.
		/// </summary>
		/// <param name="vector">The vector to rotate.</param>
		/// <param name="axis">The center point of rotation.</param>
		/// <param name="angle">The cumulative angle of rotation.</param>
		public static Vector2 RotateAbout(Vector2 vector, Vector2 axis, float angle) {
			return axis + ToMoveAt(vector.DistanceFrom(axis), axis.GetAngleFrom(vector) + angle);
		}

		/// <summary>
		/// Gets the current angle from another point in Radians.
		/// </summary>
		/// <param name="v">The point to get angle with respect to it.</param>
		public float GetAngleFrom(Vector2 v) {
			return (float) Math.Atan2(v.Y - Y, v.X - X);
		}

		/// <summary>
		/// Gets the current angle from another point in Radians.
		/// </summary>
		/// <param name="v">A point.</param>
		/// <param name="axis">The axis point to get rotation from.</param>
		public static float GetAngleFrom(Vector2 v, Vector2 axis) {
			return (float) Math.Atan2(v.Y - axis.Y, v.X - axis.X);
		}

		/// <summary>
		/// Moves point at the specified angle (measured in Radians) at the specified distance.
		/// </summary>
		/// <param name="distance">The distance to move.</param>
		/// <param name="angle">The angle to move.</param>
		public void MoveAt(float distance, float angle) {
			Vector2 v = ToMoveAt(distance, angle);
			X += v.X;
			Y += v.Y;
		}

		/// <summary>
		/// Moves point at the specified angle (measured in Radians) at the specified distance.
		/// </summary>
		/// <param name="vector">The point to move.</param>
		/// <param name="distance">The distance to move.</param>
		/// <param name="angle">The angle to move.</param>
		public static Vector2 MoveAt(Vector2 vector, float distance, float angle) {
			Vector2 v = ToMoveAt(distance, angle);
			return new Vector2(vector.X + v.X, vector.Y + v.Y);
		}

		/// <summary>
		/// Returns the movement required for the point to move at the specified distance and angle.
		/// </summary>
		/// <param name="distance">The distance to move at.</param>
		/// <param name="angle">The angle to move at</param>
		public static Vector2 ToMoveAt(float distance, float angle) {
			return new Vector2(((float) Math.Cos(angle)) * distance, ((float) Math.Sin(angle)) * distance);
		}

		/// <summary>
		/// Moves the point towards the specified point for the specified distance.
		/// </summary>
		/// <param name="v">The point to move towards.</param>
		/// <param name="distance">The distance to move.</param>
		public void MoveTowards(Vector2 v, float distance) {
			MoveAt(distance, GetAngleFrom(v));
		}

		/// <summary>
		/// Moves the point towards the specified point for the specified distance.
		/// </summary>
		/// <param name="vector">The vector to move.</param>
		/// <param name="point">The point to move towards.</param>
		/// <param name="distance">The distance to move.</param>
		public static Vector2 MoveTowards(Vector2 vector, Vector2 point, float distance) {
			vector.MoveAt(distance, vector.GetAngleFrom(point));
			return vector;
		}

		/// <summary>
		/// Returns the movement required to move the point the specified distance towards the specified point.
		/// </summary>
		/// <param name="v">The point to move towards.</param>
		/// <param name="distance">The distance to move at.</param>
		public Vector2 DirectionTowards(Vector2 v, float distance) {
			return ToMoveAt(distance, GetAngleFrom(v));
		}

		/// <summary>
		/// Moves the point away from the specified point at the specified distance.
		/// </summary>
		/// <param name="v">The point to move away from.</param>
		/// <param name="distance">The distance to move.</param>
		public void MoveAwayFrom(Vector2 v, float distance) {
			MoveAt(distance, v.GetAngleFrom(this));
		}

		/// <summary>
		/// Moves the point away from the specified point at the specified distance.
		/// </summary>
		/// <param name="vector">The vector to move.</param>
		/// <param name="point">The point to move away from.</param>
		/// <param name="distance">The distance to move.</param>
		public static Vector2 MoveAway(Vector2 vector, Vector2 point, float distance) {
			vector.MoveAt(distance, point.GetAngleFrom(vector));
			return vector;
		}

		/// <summary>
		/// Returns the movement required to move the point the specified distance away from the specified point.
		/// </summary>
		/// <param name="v">The point to move towards.</param>
		/// <param name="distance">The distance to move at.</param>
		public Vector2 DirectionAwayFrom(Vector2 v, float distance) {
			return ToMoveAt(distance, v.GetAngleFrom(this));
		}

		/// <summary>
		/// Gets the point on the circumference of an ellipse that is located at the specified angle from the center.
		/// </summary>
		/// <param name="center">The center point of the ellipse.</param>
		/// <param name="radii">The radii of the ellipse (width and height).</param>
		/// <param name="ellipseRotation">The angle of rotation of the ellipse itself.</param>
		/// <param name="angle">The angle the point to obtain on the circumference makes with the center.</param>
		public static Vector2 GetPointOnEllipseFromAngle(Vector2 center, Vector2 radii, float ellipseRotation, float angle) {
			if (Math.Abs(radii.X - radii.Y) <= float.Epsilon)
				return center + SetAngleFrom(radii, Zero, angle);
			else {
				angle = angle % MathHelper.TwoPiF;
				if (angle >= MathHelper.PiF)
					angle -= MathHelper.TwoPiF;
				double t = Math.Atan(radii.X * Math.Tan(angle) / radii.Y);
				if (angle >= MathHelper.PiOver2F && angle < MathHelper.PiF)
					t += Math.PI;
				else if (angle >= -MathHelper.PiF && angle < -MathHelper.PiOver2F)
					t -= Math.PI;
				Vector2 resultant = new Vector2(radii.X * (float) Math.Cos(t), radii.Y * (float) Math.Sin(t));
				return center + (ellipseRotation % MathHelper.TwoPiF == 0f ? resultant : RotateAbout(resultant, Zero, ellipseRotation));
			}
		}

		#endregion

		#endregion

		#region Operators

		/// <summary>
		/// Adds the specified instances.
		/// </summary>
		/// <param name="left">Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>Result of addition.</returns>
		public static Vector2 operator +(Vector2 left, Vector2 right) {
			left.X += right.X;
			left.Y += right.Y;
			return left;
		}

		/// <summary>
		/// Subtracts the specified instances.
		/// </summary>
		/// <param name="left">Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>Result of subtraction.</returns>
		public static Vector2 operator -(Vector2 left, Vector2 right) {
			left.X -= right.X;
			left.Y -= right.Y;
			return left;
		}

		/// <summary>
		/// Negates the specified instance.
		/// </summary>
		/// <param name="vec">Operand.</param>
		/// <returns>Result of negation.</returns>
		public static Vector2 operator -(Vector2 vec) {
			vec.X = -vec.X;
			vec.Y = -vec.Y;
			return vec;
		}

		/// <summary>
		/// Compares the specified instances for equality.
		/// </summary>
		/// <param name="left">Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True if both instances are equal; false otherwise.</returns>
		public static bool operator ==(Vector2 left, Vector2 right) {
			return left.Equals(right);
		}

		/// <summary>
		/// Compares the specified instances for inequality.
		/// </summary>
		/// <param name="left">Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True if both instances are not equal; false otherwise.</returns>
		public static bool operator !=(Vector2 left, Vector2 right) {
			return !left.Equals(right);
		}

		/// <summary>
		/// Casts a Vector2 to PointF
		/// </summary>
		/// <param name="v">The vector to cast</param>
		public static explicit operator PointF(Vector2 v) {
			return new PointF(v.X, v.Y);
		}

		/// <summary>
		/// Casts a PointF to Vector2
		/// </summary>
		/// <param name="v">The point to cast</param>
		public static explicit operator Vector2(PointF v) {
			return new Vector2(v.X, v.Y);
		}

		/// <summary>
		/// Casts a Vector2 to Point
		/// </summary>
		/// <param name="v">The vector to cast</param>
		public static explicit operator Point(Vector2 v) {
			return new Point((int) v.X, (int) v.Y);
		}

		/// <summary>
		/// Casts a Point to Vector2
		/// </summary>
		/// <param name="v">The point to cast</param>
		public static explicit operator Vector2(Point v) {
			return new Vector2(v.X, v.Y);
		}

		/// <summary>
		/// Casts a Vector2 to SizeF
		/// </summary>
		/// <param name="v">The vector to cast</param>
		public static explicit operator SizeF(Vector2 v) {
			return new SizeF(v.X, v.Y);
		}

		/// <summary>
		/// Casts a SizeF to Vector2
		/// </summary>
		/// <param name="v">The size to cast</param>
		public static explicit operator Vector2(SizeF v) {
			return new Vector2(v.Width, v.Height);
		}

		/// <summary>
		/// Casts a Vector2 to Size
		/// </summary>
		/// <param name="v">The vector to cast</param>
		public static explicit operator Size(Vector2 v) {
			return new Size((int) v.X, (int) v.Y);
		}

		/// <summary>
		/// Casts a Size to Vector2
		/// </summary>
		/// <param name="v">The size to cast</param>
		public static explicit operator Vector2(Size v) {
			return new Vector2(v.Width, v.Height);
		}

		#endregion

		#region Overrides

		#region public override string ToString()

		/// <summary>
		/// Returns a System.String that represents the current Vector2.
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return String.Format("({0}, {1})", X, Y);
		}

		#endregion

		#region public override int GetHashCode()

		/// <summary>
		/// Returns the hashcode for this instance.
		/// </summary>
		/// <returns>A System.Int32 containing the unique hashcode for this instance.</returns>
		public override int GetHashCode() {
			return X.GetHashCode() ^ Y.GetHashCode();
		}

		#endregion

		#region public override bool Equals(object obj)

		/// <summary>
		/// Indicates whether this instance and a specified object are equal.
		/// </summary>
		/// <param name="obj">The object to compare to.</param>
		/// <returns>True if the instances are equal; false otherwise.</returns>
		public override bool Equals(object obj) {
			if (!(obj is Vector2))
				return false;

			return Equals((Vector2) obj);
		}

		#endregion

		#endregion

		#endregion

		#region IEquatable<Vector2> Members

		/// <summary>Indicates whether the current vector is equal to another vector.</summary>
		/// <param name="other">A vector to compare with this vector.</param>
		/// <returns>true if the current vector is equal to the vector parameter; otherwise, false.</returns>
		public bool Equals(Vector2 other) {
			return
				X == other.X &&
				Y == other.Y;
		}

		#endregion
	}
}