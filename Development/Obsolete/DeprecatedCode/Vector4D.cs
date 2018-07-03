using System.Runtime.InteropServices;

namespace System.Drawing {
	/// <summary>Represents a 4D vector using four double-precision floating-point numbers.</summary>
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector4D : IEquatable<Vector4D> {
		#region Fields

		/// <summary>
		/// The X component of the Vector4d.
		/// </summary>
		public double X;

		/// <summary>
		/// The Y component of the Vector4d.
		/// </summary>
		public double Y;

		/// <summary>
		/// The Z component of the Vector4d.
		/// </summary>
		public double Z;

		/// <summary>
		/// The W component of the Vector4d.
		/// </summary>
		public double W;

		/// <summary>
		/// Defines a unit-length Vector4d that points towards the X-axis.
		/// </summary>
		public static readonly Vector4D UnitX = new Vector4D(1D, 0D, 0D, 0D);

		/// <summary>
		/// Defines a unit-length Vector4d that points towards the Y-axis.
		/// </summary>
		public static readonly Vector4D UnitY = new Vector4D(0D, 1D, 0D, 0D);

		/// <summary>
		/// Defines a unit-length Vector4d that points towards the Z-axis.
		/// </summary>
		public static readonly Vector4D UnitZ = new Vector4D(0D, 0D, 1D, 0D);

		/// <summary>
		/// Defines a unit-length Vector4d that points towards the W-axis.
		/// </summary>
		public static readonly Vector4D UnitW = new Vector4D(0D, 0D, 0D, 1D);

		/// <summary>
		/// Defines a zero-length Vector4d.
		/// </summary>
		public static readonly Vector4D Zero = new Vector4D();

		/// <summary>
		/// Defines an instance with all components set to 1.
		/// </summary>
		public static readonly Vector4D One = new Vector4D(1D);

		/// <summary>
		/// Defines the size of the Vector4d struct in bytes.
		/// </summary>
		public static readonly int SizeInBytes = Marshal.SizeOf(new Vector4D());

		#endregion

		#region Constructors

		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		/// <param name="value">The value that will initialize this instance.</param>
		public Vector4D(double value) {
			X = value;
			Y = value;
			Z = value;
			W = value;
		}

		/// <summary>
		/// Constructs a new Vector4d.
		/// </summary>
		/// <param name="x">The x component of the Vector4d.</param>
		/// <param name="y">The y component of the Vector4d.</param>
		/// <param name="z">The z component of the Vector4d.</param>
		/// <param name="w">The w component of the Vector4d.</param>
		public Vector4D(double x, double y, double z, double w) {
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		/// <summary>
		/// Constructs a new Vector4d from the given Vector3d.
		/// The w component is initialized to 0.
		/// </summary>
		/// <param name="v">The Vector3d to copy components from.</param>
		/// <remarks><seealso cref="Vector4D(Vector3D, double)"/></remarks>
		public Vector4D(Vector3D v) {
			X = v.X;
			Y = v.Y;
			Z = v.Z;
			W = 0f;
		}

		/// <summary>
		/// Constructs a new Vector4d from the specified Vector3d and w component.
		/// </summary>
		/// <param name="v">The Vector3d to copy components from.</param>
		/// <param name="w">The w component of the new Vector4.</param>
		public Vector4D(Vector3D v, double w) {
			X = v.X;
			Y = v.Y;
			Z = v.Z;
			W = w;
		}

		/// <summary>
		/// Constructs a new Vector4d from the given Vector4d.
		/// </summary>
		/// <param name="v">The Vector4d to copy components from.</param>
		public Vector4D(Vector4D v) {
			X = v.X;
			Y = v.Y;
			Z = v.Z;
			W = v.W;
		}

		#endregion

		#region Public Members

		#region Instance

		#region public Vector3d Xyz

		/// <summary>
		/// Gets or sets an System.Vector3d with the X, Y and Z components of this instance.
		/// </summary>
		public Vector3D Xyz {
			get {
				return new Vector3D(X, Y, Z);
			}
			set {
				X = value.X;
				Y = value.Y;
				Z = value.Z;
			}
		}

		#endregion

		#region public double Length

		/// <summary>
		/// Gets the length (magnitude) of the vector.
		/// </summary>
		/// <see cref="LengthFast"/>
		/// <seealso cref="LengthSquared"/>
		public double Length {
			get {
				return System.Math.Sqrt(X * X + Y * Y + Z * Z + W * W);
			}
		}

		#endregion

		#region public double LengthFast

		/// <summary>
		/// Gets an approximation of the vector length (magnitude).
		/// </summary>
		/// <remarks>
		/// This property uses an approximation of the square root function to calculate vector magnitude, with
		/// an upper error bound of 0.001.
		/// </remarks>
		/// <see cref="Length"/>
		/// <seealso cref="LengthSquared"/>
		public double LengthFast {
			get {
				return 1.0 / MathHelper.InverseSqrtFast((float) (X * X + Y * Y + Z * Z + W * W));
			}
		}

		#endregion

		#region public double LengthSquared

		/// <summary>
		/// Gets the square of the vector length (magnitude).
		/// </summary>
		/// <remarks>
		/// This property avoids the costly square root operation required by the Length property. This makes it more suitable
		/// for comparisons.
		/// </remarks>
		/// <see cref="Length"/>
		public double LengthSquared {
			get {
				return X * X + Y * Y + Z * Z + W * W;
			}
		}

		#endregion

		#region public void Normalize()

		/// <summary>
		/// Scales the Vector4d to unit length.
		/// </summary>
		public void Normalize() {
			double scale = 1.0 / this.Length;
			X *= scale;
			Y *= scale;
			Z *= scale;
			W *= scale;
		}

		#endregion

		#region public void NormalizeFast()

		/// <summary>
		/// Scales the Vector4d to approximately unit length.
		/// </summary>
		public void NormalizeFast() {
			double scale = MathHelper.InverseSqrtFast((float) (X * X + Y * Y + Z * Z + W * W));
			X *= scale;
			Y *= scale;
			Z *= scale;
			W *= scale;
		}

		#endregion

		#region public void Conjugate()

		/// <summary>
		/// Convert this quaternion to its conjugate
		/// </summary>
		public void Conjugate() {
			Xyz = -Xyz;
		}

		#endregion

		#region public void ToAxisAngle()

		/// <summary>
		/// Convert the current quaternion to axis angle representation
		/// </summary>
		/// <param name="axis">The resultant axis</param>
		/// <param name="angle">The resultant angle</param>
		public void ToAxisAngle(out Vector3D axis, out double angle) {
			Vector4D result = ToAxisAngle();
			axis = result.Xyz;
			angle = result.W;
		}

		/// <summary>
		/// Convert this instance to an axis-angle representation.
		/// </summary>
		/// <returns>A Vector4d that is the axis-angle representation of this quaternion.</returns>
		public Vector4D ToAxisAngle() {
			Vector4D q = this;
			if (Math.Abs(q.W) > 1.0)
				q.Normalize();

			Vector4D result = new Vector4D();

			result.W = 2.0 * System.Math.Acos(q.W); // angle
			double den = System.Math.Sqrt(1.0 - q.W * q.W);
			if (den > 0.0001)
				result.Xyz = q.Xyz / den;
			else
				result.Xyz = Vector3D.UnitX;

			return result;
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
		public static Vector4D Add(Vector4D a, Vector4D b) {
			Add(ref a, ref b, out a);
			return a;
		}

		/// <summary>
		/// Adds two vectors.
		/// </summary>
		/// <param name="a">Left operand.</param>
		/// <param name="b">Right operand.</param>
		/// <param name="result">Result of operation.</param>
		public static void Add(ref Vector4D a, ref Vector4D b, out Vector4D result) {
			result = new Vector4D(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
		}

		#endregion

		#region Subtract

		/// <summary>
		/// Subtract one Vector from another
		/// </summary>
		/// <param name="a">First operand</param>
		/// <param name="b">Second operand</param>
		/// <returns>Result of subtraction</returns>
		public static Vector4D Subtract(Vector4D a, Vector4D b) {
			Subtract(ref a, ref b, out a);
			return a;
		}

		/// <summary>
		/// Subtract one Vector from another
		/// </summary>
		/// <param name="a">First operand</param>
		/// <param name="b">Second operand</param>
		/// <param name="result">Result of subtraction</param>
		public static void Subtract(ref Vector4D a, ref Vector4D b, out Vector4D result) {
			result = new Vector4D(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);
		}

		#endregion

		#region Multiply

		/// <summary>
		/// Multiplies a vector by a scalar.
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <returns>Result of the operation.</returns>
		public static Vector4D Multiply(Vector4D vector, double scale) {
			Multiply(ref vector, scale, out vector);
			return vector;
		}

		/// <summary>
		/// Multiplies a vector by a scalar.
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <param name="result">Result of the operation.</param>
		public static void Multiply(ref Vector4D vector, double scale, out Vector4D result) {
			result = new Vector4D(vector.X * scale, vector.Y * scale, vector.Z * scale, vector.W * scale);
		}

		/// <summary>
		/// Multiplies a vector by the components a vector (scale).
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <returns>Result of the operation.</returns>
		public static Vector4D Multiply(Vector4D vector, Vector4D scale) {
			Multiply(ref vector, ref scale, out vector);
			return vector;
		}

		/// <summary>
		/// Multiplies a vector by the components of a vector (scale).
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <param name="result">Result of the operation.</param>
		public static void Multiply(ref Vector4D vector, ref Vector4D scale, out Vector4D result) {
			result = new Vector4D(vector.X * scale.X, vector.Y * scale.Y, vector.Z * scale.Z, vector.W * scale.W);
		}

		#endregion

		#region Divide

		/// <summary>
		/// Divides a vector by a scalar.
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <returns>Result of the operation.</returns>
		public static Vector4D Divide(Vector4D vector, double scale) {
			Divide(ref vector, scale, out vector);
			return vector;
		}

		/// <summary>
		/// Divides a vector by a scalar.
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <param name="result">Result of the operation.</param>
		public static void Divide(ref Vector4D vector, double scale, out Vector4D result) {
			Multiply(ref vector, 1 / scale, out result);
		}

		/// <summary>
		/// Divides a vector by the components of a vector (scale).
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <returns>Result of the operation.</returns>
		public static Vector4D Divide(Vector4D vector, Vector4D scale) {
			Divide(ref vector, ref scale, out vector);
			return vector;
		}

		/// <summary>
		/// Divide a vector by the components of a vector (scale).
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <param name="result">Result of the operation.</param>
		public static void Divide(ref Vector4D vector, ref Vector4D scale, out Vector4D result) {
			result = new Vector4D(vector.X / scale.X, vector.Y / scale.Y, vector.Z / scale.Z, vector.W / scale.W);
		}

		#endregion

		#region Min

		/// <summary>
		/// Calculate the component-wise minimum of two vectors
		/// </summary>
		/// <param name="a">First operand</param>
		/// <param name="b">Second operand</param>
		/// <returns>The component-wise minimum</returns>
		public static Vector4D Min(Vector4D a, Vector4D b) {
			a.X = a.X < b.X ? a.X : b.X;
			a.Y = a.Y < b.Y ? a.Y : b.Y;
			a.Z = a.Z < b.Z ? a.Z : b.Z;
			a.W = a.W < b.W ? a.W : b.W;
			return a;
		}

		/// <summary>
		/// Calculate the component-wise minimum of two vectors
		/// </summary>
		/// <param name="a">First operand</param>
		/// <param name="b">Second operand</param>
		/// <param name="result">The component-wise minimum</param>
		public static void Min(ref Vector4D a, ref Vector4D b, out Vector4D result) {
			result.X = a.X < b.X ? a.X : b.X;
			result.Y = a.Y < b.Y ? a.Y : b.Y;
			result.Z = a.Z < b.Z ? a.Z : b.Z;
			result.W = a.W < b.W ? a.W : b.W;
		}

		#endregion

		#region Max

		/// <summary>
		/// Calculate the component-wise maximum of two vectors
		/// </summary>
		/// <param name="a">First operand</param>
		/// <param name="b">Second operand</param>
		/// <returns>The component-wise maximum</returns>
		public static Vector4D Max(Vector4D a, Vector4D b) {
			a.X = a.X > b.X ? a.X : b.X;
			a.Y = a.Y > b.Y ? a.Y : b.Y;
			a.Z = a.Z > b.Z ? a.Z : b.Z;
			a.W = a.W > b.W ? a.W : b.W;
			return a;
		}

		/// <summary>
		/// Calculate the component-wise maximum of two vectors
		/// </summary>
		/// <param name="a">First operand</param>
		/// <param name="b">Second operand</param>
		/// <param name="result">The component-wise maximum</param>
		public static void Max(ref Vector4D a, ref Vector4D b, out Vector4D result) {
			result.X = a.X > b.X ? a.X : b.X;
			result.Y = a.Y > b.Y ? a.Y : b.Y;
			result.Z = a.Z > b.Z ? a.Z : b.Z;
			result.W = a.W > b.W ? a.W : b.W;
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
		public static Vector4D Clamp(Vector4D vec, Vector4D min, Vector4D max) {
			vec.X = vec.X < min.X ? min.X : vec.X > max.X ? max.X : vec.X;
			vec.Y = vec.Y < min.Y ? min.Y : vec.Y > max.Y ? max.Y : vec.Y;
			vec.Z = vec.X < min.Z ? min.Z : vec.Z > max.Z ? max.Z : vec.Z;
			vec.W = vec.Y < min.W ? min.W : vec.W > max.W ? max.W : vec.W;
			return vec;
		}

		/// <summary>
		/// Clamp a vector to the given minimum and maximum vectors
		/// </summary>
		/// <param name="vec">Input vector</param>
		/// <param name="min">Minimum vector</param>
		/// <param name="max">Maximum vector</param>
		/// <param name="result">The clamped vector</param>
		public static void Clamp(ref Vector4D vec, ref Vector4D min, ref Vector4D max, out Vector4D result) {
			result.X = vec.X < min.X ? min.X : vec.X > max.X ? max.X : vec.X;
			result.Y = vec.Y < min.Y ? min.Y : vec.Y > max.Y ? max.Y : vec.Y;
			result.Z = vec.X < min.Z ? min.Z : vec.Z > max.Z ? max.Z : vec.Z;
			result.W = vec.Y < min.W ? min.W : vec.W > max.W ? max.W : vec.W;
		}

		#endregion

		#region Normalize

		/// <summary>
		/// Scale a vector to unit length
		/// </summary>
		/// <param name="vec">The input vector</param>
		/// <returns>The normalized vector</returns>
		public static Vector4D Normalize(Vector4D vec) {
			double scale = 1.0 / vec.Length;
			vec.X *= scale;
			vec.Y *= scale;
			vec.Z *= scale;
			vec.W *= scale;
			return vec;
		}

		/// <summary>
		/// Scale a vector to unit length
		/// </summary>
		/// <param name="vec">The input vector</param>
		/// <param name="result">The normalized vector</param>
		public static void Normalize(ref Vector4D vec, out Vector4D result) {
			double scale = 1.0 / vec.Length;
			result.X = vec.X * scale;
			result.Y = vec.Y * scale;
			result.Z = vec.Z * scale;
			result.W = vec.W * scale;
		}

		#endregion

		#region NormalizeFast

		/// <summary>
		/// Scale a vector to approximately unit length
		/// </summary>
		/// <param name="vec">The input vector</param>
		/// <returns>The normalized vector</returns>
		public static Vector4D NormalizeFast(Vector4D vec) {
			double scale = MathHelper.InverseSqrtFast((float) (vec.X * vec.X + vec.Y * vec.Y + vec.Z * vec.Z + vec.W * vec.W));
			vec.X *= scale;
			vec.Y *= scale;
			vec.Z *= scale;
			vec.W *= scale;
			return vec;
		}

		/// <summary>
		/// Scale a vector to approximately unit length
		/// </summary>
		/// <param name="vec">The input vector</param>
		/// <param name="result">The normalized vector</param>
		public static void NormalizeFast(ref Vector4D vec, out Vector4D result) {
			double scale = MathHelper.InverseSqrtFast((float) (vec.X * vec.X + vec.Y * vec.Y + vec.Z * vec.Z + vec.W * vec.W));
			result.X = vec.X * scale;
			result.Y = vec.Y * scale;
			result.Z = vec.Z * scale;
			result.W = vec.W * scale;
		}

		#endregion

		#region Dot

		/// <summary>
		/// Calculate the dot product of two vectors
		/// </summary>
		/// <param name="left">First operand</param>
		/// <param name="right">Second operand</param>
		/// <returns>The dot product of the two inputs</returns>
		public static double Dot(Vector4D left, Vector4D right) {
			return left.X * right.X + left.Y * right.Y + left.Z * right.Z + left.W * right.W;
		}

		/// <summary>
		/// Calculate the dot product of two vectors
		/// </summary>
		/// <param name="left">First operand</param>
		/// <param name="right">Second operand</param>
		/// <param name="result">The dot product of the two inputs</param>
		public static void Dot(ref Vector4D left, ref Vector4D right, out double result) {
			result = left.X * right.X + left.Y * right.Y + left.Z * right.Z + left.W * right.W;
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
		public static Vector4D Lerp(Vector4D a, Vector4D b, double blend) {
			a.X = blend * (b.X - a.X) + a.X;
			a.Y = blend * (b.Y - a.Y) + a.Y;
			a.Z = blend * (b.Z - a.Z) + a.Z;
			a.W = blend * (b.W - a.W) + a.W;
			return a;
		}

		/// <summary>
		/// Returns a new Vector that is the linear blend of the 2 given Vectors
		/// </summary>
		/// <param name="a">First input vector</param>
		/// <param name="b">Second input vector</param>
		/// <param name="blend">The blend factor. a when blend=0, b when blend=1.</param>
		/// <param name="result">a when blend=0, b when blend=1, and a linear combination otherwise</param>
		public static void Lerp(ref Vector4D a, ref Vector4D b, double blend, out Vector4D result) {
			result.X = blend * (b.X - a.X) + a.X;
			result.Y = blend * (b.Y - a.Y) + a.Y;
			result.Z = blend * (b.Z - a.Z) + a.Z;
			result.W = blend * (b.W - a.W) + a.W;
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
		public static Vector4D BaryCentric(Vector4D a, Vector4D b, Vector4D c, double u, double v) {
			return a + u * (b - a) + v * (c - a);
		}

		/// <summary>Interpolate 3 Vectors using Barycentric coordinates</summary>
		/// <param name="a">First input Vector.</param>
		/// <param name="b">Second input Vector.</param>
		/// <param name="c">Third input Vector.</param>
		/// <param name="u">First Barycentric Coordinate.</param>
		/// <param name="v">Second Barycentric Coordinate.</param>
		/// <param name="result">Output Vector. a when u=v=0, b when u=1,v=0, c when u=0,v=1, and a linear combination of a,b,c otherwise</param>
		public static void BaryCentric(ref Vector4D a, ref Vector4D b, ref Vector4D c, double u, double v, out Vector4D result) {
			result = a; // copy

			Vector4D temp = b; // copy
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

		/// <summary>Transform a Vector by the given Matrix</summary>
		/// <param name="vec">The vector to transform</param>
		/// <param name="mat">The desired transformation</param>
		/// <returns>The transformed vector</returns>
		public static Vector4D Transform(Vector4D vec, Matrix4D mat) {
			Vector4D result;
			Transform(ref vec, ref mat, out result);
			return result;
		}

		/// <summary>Transform a Vector by the given Matrix</summary>
		/// <param name="vec">The vector to transform</param>
		/// <param name="mat">The desired transformation</param>
		/// <param name="result">The transformed vector</param>
		public static void Transform(ref Vector4D vec, ref Matrix4D mat, out Vector4D result) {
			result = new Vector4D(
				vec.X * mat.Row0.X + vec.Y * mat.Row1.X + vec.Z * mat.Row2.X + vec.W * mat.Row3.X,
				vec.X * mat.Row0.Y + vec.Y * mat.Row1.Y + vec.Z * mat.Row2.Y + vec.W * mat.Row3.Y,
				vec.X * mat.Row0.Z + vec.Y * mat.Row1.Z + vec.Z * mat.Row2.Z + vec.W * mat.Row3.Z,
				vec.X * mat.Row0.W + vec.Y * mat.Row1.W + vec.Z * mat.Row2.W + vec.W * mat.Row3.W);
		}

		/// <summary>
		/// Transforms a vector by a quaternion rotation.
		/// </summary>
		/// <param name="vec">The vector to transform.</param>
		/// <param name="quat">The quaternion to rotate the vector by.</param>
		/// <returns>The result of the operation.</returns>
		public static Vector4D Transform(Vector4D vec, Vector4D quat) {
			Vector4D result;
			Transform(ref vec, ref quat, out result);
			return result;
		}

		/// <summary>
		/// Transforms a vector by a quaternion rotation.
		/// </summary>
		/// <param name="vec">The vector to transform.</param>
		/// <param name="quat">The quaternion to rotate the vector by.</param>
		/// <param name="result">The result of the operation.</param>
		public static void Transform(ref Vector4D vec, ref Vector4D quat, out Vector4D result) {
			Vector4D v = new Vector4D(vec.X, vec.Y, vec.Z, vec.W), i, t;
			Vector4D.Invert(ref quat, out i);
			Vector4D.Multiply(ref quat, ref v, out t);
			Vector4D.Multiply(ref t, ref i, out v);

			result = new Vector4D(v.X, v.Y, v.Z, v.W);
		}

		#endregion

		#region Conjugate

		/// <summary>
		/// Get the conjugate of the given quaternion
		/// </summary>
		/// <param name="q">The quaternion</param>
		/// <returns>The conjugate of the given quaternion</returns>
		public static Vector4D Conjugate(Vector4D q) {
			return new Vector4D(-q.Xyz, q.W);
		}

		/// <summary>
		/// Get the conjugate of the given quaternion
		/// </summary>
		/// <param name="q">The quaternion</param>
		/// <param name="result">The conjugate of the given quaternion</param>
		public static void Conjugate(ref Vector4D q, out Vector4D result) {
			result = new Vector4D(-q.Xyz, q.W);
		}

		#endregion

		#region Invert

		/// <summary>
		/// Get the inverse of the given quaternion
		/// </summary>
		/// <param name="q">The quaternion to invert</param>
		/// <returns>The inverse of the given quaternion</returns>
		public static Vector4D Invert(Vector4D q) {
			Vector4D result;
			Invert(ref q, out result);
			return result;
		}

		/// <summary>
		/// Get the inverse of the given quaternion
		/// </summary>
		/// <param name="q">The quaternion to invert</param>
		/// <param name="result">The inverse of the given quaternion</param>
		public static void Invert(ref Vector4D q, out Vector4D result) {
			double lengthSq = q.LengthSquared;
			if (lengthSq != 0.0) {
				double i = 1.0 / lengthSq;
				result = new Vector4D(q.Xyz * -i, q.W * i);
			} else {
				result = q;
			}
		}

		#endregion

		#region FromAxisAngle

		/// <summary>
		/// Build a quaternion from the given axis and angle
		/// </summary>
		/// <param name="axis">The axis to rotate about</param>
		/// <param name="angle">The rotation angle in radians</param>
		/// <returns></returns>
		public static Vector4D FromAxisAngle(Vector3D axis, double angle) {
			if (axis.LengthSquared == 0.0)
				return UnitW;

			Vector4D result = new Vector4D();

			angle *= 0.5;
			axis.Normalize();
			result.Xyz = axis * System.Math.Sin(angle);
			result.W = System.Math.Cos(angle);

			return Normalize(result);
		}

		#endregion

		#region Slerp

		/// <summary>
		/// Do Spherical linear interpolation between two quaternions 
		/// </summary>
		/// <param name="q1">The first quaternion</param>
		/// <param name="q2">The second quaternion</param>
		/// <param name="blend">The blend factor</param>
		/// <returns>A smooth blend between the given quaternions</returns>
		public static Vector4D Slerp(Vector4D q1, Vector4D q2, double blend) {
			// if either input is zero, return the other.
			if (q1.LengthSquared == 0.0) {
				if (q2.LengthSquared == 0.0) {
					return UnitW;
				}
				return q2;
			} else if (q2.LengthSquared == 0.0) {
				return q1;
			}


			double cosHalfAngle = q1.W * q2.W + Vector3D.Dot(q1.Xyz, q2.Xyz);

			if (cosHalfAngle >= 1.0 || cosHalfAngle <= -1.0)
				return q1;
			else if (cosHalfAngle < 0.0) {
				q2.Xyz = -q2.Xyz;
				q2.W = -q2.W;
				cosHalfAngle = -cosHalfAngle;
			}

			double blendA;
			double blendB;
			if (cosHalfAngle < 0.99) {
				// do proper slerp for big angles
				double halfAngle = System.Math.Acos(cosHalfAngle);
				double sinHalfAngle = System.Math.Sin(halfAngle);
				double oneOverSinHalfAngle = 1.0 / sinHalfAngle;
				blendA = System.Math.Sin(halfAngle * (1.0 - blend)) * oneOverSinHalfAngle;
				blendB = System.Math.Sin(halfAngle * blend) * oneOverSinHalfAngle;
			} else {
				// do lerp if angle is really small.
				blendA = 1.0 - blend;
				blendB = blend;
			}

			Vector4D result = new Vector4D(blendA * q1.Xyz + blendB * q2.Xyz, blendA * q1.W + blendB * q2.W);
			if (result.LengthSquared > 0.0)
				return Normalize(result);
			else
				return UnitW;
		}

		#endregion

		#endregion

		#region Operators

		/// <summary>
		/// Adds two instances.
		/// </summary>
		/// <param name="left">The first instance.</param>
		/// <param name="right">The second instance.</param>
		/// <returns>The result of the calculation.</returns>
		public static Vector4D operator +(Vector4D left, Vector4D right) {
			left.X += right.X;
			left.Y += right.Y;
			left.Z += right.Z;
			left.W += right.W;
			return left;
		}

		/// <summary>
		/// Subtracts two instances.
		/// </summary>
		/// <param name="left">The first instance.</param>
		/// <param name="right">The second instance.</param>
		/// <returns>The result of the calculation.</returns>
		public static Vector4D operator -(Vector4D left, Vector4D right) {
			left.X -= right.X;
			left.Y -= right.Y;
			left.Z -= right.Z;
			left.W -= right.W;
			return left;
		}

		/// <summary>
		/// Negates an instance.
		/// </summary>
		/// <param name="vec">The instance.</param>
		/// <returns>The result of the calculation.</returns>
		public static Vector4D operator -(Vector4D vec) {
			vec.X = -vec.X;
			vec.Y = -vec.Y;
			vec.Z = -vec.Z;
			vec.W = -vec.W;
			return vec;
		}

		/// <summary>
		/// Multiplies an instance by a scalar.
		/// </summary>
		/// <param name="vec">The instance.</param>
		/// <param name="scale">The scalar.</param>
		/// <returns>The result of the calculation.</returns>
		public static Vector4D operator *(Vector4D vec, double scale) {
			vec.X *= scale;
			vec.Y *= scale;
			vec.Z *= scale;
			vec.W *= scale;
			return vec;
		}

		/// <summary>
		/// Multiplies an instance by a scalar.
		/// </summary>
		/// <param name="scale">The scalar.</param>
		/// <param name="vec">The instance.</param>
		/// <returns>The result of the calculation.</returns>
		public static Vector4D operator *(double scale, Vector4D vec) {
			vec.X *= scale;
			vec.Y *= scale;
			vec.Z *= scale;
			vec.W *= scale;
			return vec;
		}

		/// <summary>
		/// Divides an instance by a scalar.
		/// </summary>
		/// <param name="vec">The instance.</param>
		/// <param name="scale">The scalar.</param>
		/// <returns>The result of the calculation.</returns>
		public static Vector4D operator /(Vector4D vec, double scale) {
			double mult = 1 / scale;
			vec.X *= mult;
			vec.Y *= mult;
			vec.Z *= mult;
			vec.W *= mult;
			return vec;
		}

		/// <summary>
		/// Compares two instances for equality.
		/// </summary>
		/// <param name="left">The first instance.</param>
		/// <param name="right">The second instance.</param>
		/// <returns>True, if left equals right; false otherwise.</returns>
		public static bool operator ==(Vector4D left, Vector4D right) {
			return left.Equals(right);
		}

		/// <summary>
		/// Compares two instances for inequality.
		/// </summary>
		/// <param name="left">The first instance.</param>
		/// <param name="right">The second instance.</param>
		/// <returns>True, if left does not equa lright; false otherwise.</returns>
		public static bool operator !=(Vector4D left, Vector4D right) {
			return !left.Equals(right);
		}

		/// <summary>
		/// Returns a pointer to the first element of the specified instance.
		/// </summary>
		/// <param name="v">The instance.</param>
		/// <returns>A pointer to the first element of v.</returns>
		[CLSCompliant(false)]
		unsafe public static explicit operator double*(Vector4D v) {
			return &v.X;
		}

		/// <summary>
		/// Returns a pointer to the first element of the specified instance.
		/// </summary>
		/// <param name="v">The instance.</param>
		/// <returns>A pointer to the first element of v.</returns>
		public static explicit operator IntPtr(Vector4D v) {
			unsafe {
				return (IntPtr) (&v.X);
			}
		}

		/// <summary>Converts System.Vector4 to System.Vector4d.</summary>
		/// <param name="v4">The Vector4 to convert.</param>
		/// <returns>The resulting Vector4d.</returns>
		public static explicit operator Vector4D(Vector4 v4) {
			return new Vector4D(v4.X, v4.Y, v4.Z, v4.W);
		}

		/// <summary>Converts System.Vector4d to System.Vector4.</summary>
		/// <param name="v4d">The Vector4d to convert.</param>
		/// <returns>The resulting Vector4.</returns>
		public static explicit operator Vector4(Vector4D v4d) {
			return new Vector4((float) v4d.X, (float) v4d.Y, (float) v4d.Z, (float) v4d.W);
		}

		#endregion

		#region Overrides

		#region public override string ToString()

		/// <summary>
		/// Returns a System.String that represents the current Vector4d.
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return String.Format("({0}, {1}, {2}, {3})", X, Y, Z, W);
		}

		#endregion

		#region public override int GetHashCode()

		/// <summary>
		/// Returns the hashcode for this instance.
		/// </summary>
		/// <returns>A System.Int32 containing the unique hashcode for this instance.</returns>
		public override int GetHashCode() {
			return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode() ^ W.GetHashCode();
		}

		#endregion

		#region public override bool Equals(object obj)

		/// <summary>
		/// Indicates whether this instance and a specified object are equal.
		/// </summary>
		/// <param name="obj">The object to compare to.</param>
		/// <returns>True if the instances are equal; false otherwise.</returns>
		public override bool Equals(object obj) {
			if (!(obj is Vector4D))
				return false;

			return this.Equals((Vector4D) obj);
		}

		#endregion

		#endregion

		#endregion

		#region IEquatable<Vector4d> Members

		/// <summary>Indicates whether the current vector is equal to another vector.</summary>
		/// <param name="other">A vector to compare with this vector.</param>
		/// <returns>true if the current vector is equal to the vector parameter; otherwise, false.</returns>
		public bool Equals(Vector4D other) {
			return
				X == other.X &&
				Y == other.Y &&
				Z == other.Z &&
				W == other.W;
		}

		#endregion
	}
}