using System.Runtime.InteropServices;

namespace System.Drawing {
	/// <summary>
	/// Represents a 3D vector using three double-precision floating-point numbers.
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector3D : IEquatable<Vector3D> {
		#region Fields

		/// <summary>
		/// The X component of the Vector3.
		/// </summary>
		public double X;

		/// <summary>
		/// The Y component of the Vector3.
		/// </summary>
		public double Y;

		/// <summary>
		/// The Z component of the Vector3.
		/// </summary>
		public double Z;

		/// <summary>
		/// Defines a unit-length Vector3d that points towards the X-axis.
		/// </summary>
		public static readonly Vector3D UnitX = new Vector3D(1D, 0D, 0D);

		/// <summary>
		/// Defines a unit-length Vector3d that points towards the Y-axis.
		/// </summary>
		public static readonly Vector3D UnitY = new Vector3D(0D, 1D, 0D);

		/// <summary>
		/// /// Defines a unit-length Vector3d that points towards the Z-axis.
		/// </summary>
		public static readonly Vector3D UnitZ = new Vector3D(0D, 0D, 1D);

		/// <summary>
		/// Defines a zero-length Vector3.
		/// </summary>
		public static readonly Vector3D Zero = new Vector3D();

		/// <summary>
		/// Defines an instance with all components set to 1.
		/// </summary>
		public static readonly Vector3D One = new Vector3D(1D);

		/// <summary>
		/// Defines the size of the Vector3d struct in bytes.
		/// </summary>
		public static readonly int SizeInBytes = Marshal.SizeOf(new Vector3D());

		#endregion

		#region Constructors

		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		/// <param name="value">The value that will initialize this instance.</param>
		public Vector3D(double value) {
			X = value;
			Y = value;
			Z = value;
		}

		/// <summary>
		/// Constructs a new Vector3.
		/// </summary>
		/// <param name="x">The x component of the Vector3.</param>
		/// <param name="y">The y component of the Vector3.</param>
		/// <param name="z">The z component of the Vector3.</param>
		public Vector3D(double x, double y, double z) {
			X = x;
			Y = y;
			Z = z;
		}

		/// <summary>
		/// Constructs a new instance from the given Vector2d.
		/// </summary>
		/// <param name="v">The Vector2d to copy components from.</param>
		public Vector3D(Vector2D v) {
			X = v.X;
			Y = v.Y;
			Z = 0f;
		}

		/// <summary>
		/// Constructs a new instance from the given Vector3d.
		/// </summary>
		/// <param name="v">The Vector3d to copy components from.</param>
		public Vector3D(Vector3D v) {
			X = v.X;
			Y = v.Y;
			Z = v.Z;
		}

		/// <summary>
		/// Constructs a new instance from the given Vector4d.
		/// </summary>
		/// <param name="v">The Vector4d to copy components from.</param>
		public Vector3D(Vector4D v) {
			X = v.X;
			Y = v.Y;
			Z = v.Z;
		}


		#endregion

		#region Public Members

		#region Instance

		#region public double Length

		/// <summary>
		/// Gets the length (magnitude) of the vector.
		/// </summary>
		/// <see cref="LengthFast"/>
		/// <seealso cref="LengthSquared"/>
		public double Length {
			get {
				return System.Math.Sqrt(X * X + Y * Y + Z * Z);
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
				return 1.0 / MathHelper.InverseSqrtFast((float) (X * X + Y * Y + Z * Z));
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
		/// <seealso cref="LengthFast"/>
		public double LengthSquared {
			get {
				return X * X + Y * Y + Z * Z;
			}
		}

		#endregion

		#region public void Normalize()

		/// <summary>
		/// Scales the Vector3d to unit length.
		/// </summary>
		public void Normalize() {
			double scale = 1.0 / this.Length;
			X *= scale;
			Y *= scale;
			Z *= scale;
		}

		#endregion

		#region public void NormalizeFast()

		/// <summary>
		/// Scales the Vector3d to approximately unit length.
		/// </summary>
		public void NormalizeFast() {
			double scale = MathHelper.InverseSqrtFast((float) (X * X + Y * Y + Z * Z));
			X *= scale;
			Y *= scale;
			Z *= scale;
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
		public static Vector3D Add(Vector3D a, Vector3D b) {
			Add(ref a, ref b, out a);
			return a;
		}

		/// <summary>
		/// Adds two vectors.
		/// </summary>
		/// <param name="a">Left operand.</param>
		/// <param name="b">Right operand.</param>
		/// <param name="result">Result of operation.</param>
		public static void Add(ref Vector3D a, ref Vector3D b, out Vector3D result) {
			result = new Vector3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
		}

		#endregion

		#region Subtract

		/// <summary>
		/// Subtract one Vector from another
		/// </summary>
		/// <param name="a">First operand</param>
		/// <param name="b">Second operand</param>
		/// <returns>Result of subtraction</returns>
		public static Vector3D Subtract(Vector3D a, Vector3D b) {
			Subtract(ref a, ref b, out a);
			return a;
		}

		/// <summary>
		/// Subtract one Vector from another
		/// </summary>
		/// <param name="a">First operand</param>
		/// <param name="b">Second operand</param>
		/// <param name="result">Result of subtraction</param>
		public static void Subtract(ref Vector3D a, ref Vector3D b, out Vector3D result) {
			result = new Vector3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
		}

		#endregion

		#region Multiply

		/// <summary>
		/// Multiplies a vector by a scalar.
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <returns>Result of the operation.</returns>
		public static Vector3D Multiply(Vector3D vector, double scale) {
			Multiply(ref vector, scale, out vector);
			return vector;
		}

		/// <summary>
		/// Multiplies a vector by a scalar.
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <param name="result">Result of the operation.</param>
		public static void Multiply(ref Vector3D vector, double scale, out Vector3D result) {
			result = new Vector3D(vector.X * scale, vector.Y * scale, vector.Z * scale);
		}

		/// <summary>
		/// Multiplies a vector by the components a vector (scale).
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <returns>Result of the operation.</returns>
		public static Vector3D Multiply(Vector3D vector, Vector3D scale) {
			Multiply(ref vector, ref scale, out vector);
			return vector;
		}

		/// <summary>
		/// Multiplies a vector by the components of a vector (scale).
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <param name="result">Result of the operation.</param>
		public static void Multiply(ref Vector3D vector, ref Vector3D scale, out Vector3D result) {
			result = new Vector3D(vector.X * scale.X, vector.Y * scale.Y, vector.Z * scale.Z);
		}

		#endregion

		#region Divide

		/// <summary>
		/// Divides a vector by a scalar.
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <returns>Result of the operation.</returns>
		public static Vector3D Divide(Vector3D vector, double scale) {
			Divide(ref vector, scale, out vector);
			return vector;
		}

		/// <summary>
		/// Divides a vector by a scalar.
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <param name="result">Result of the operation.</param>
		public static void Divide(ref Vector3D vector, double scale, out Vector3D result) {
			Multiply(ref vector, 1 / scale, out result);
		}

		/// <summary>
		/// Divides a vector by the components of a vector (scale).
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <returns>Result of the operation.</returns>
		public static Vector3D Divide(Vector3D vector, Vector3D scale) {
			Divide(ref vector, ref scale, out vector);
			return vector;
		}

		/// <summary>
		/// Divide a vector by the components of a vector (scale).
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <param name="result">Result of the operation.</param>
		public static void Divide(ref Vector3D vector, ref Vector3D scale, out Vector3D result) {
			result = new Vector3D(vector.X / scale.X, vector.Y / scale.Y, vector.Z / scale.Z);
		}

		#endregion

		#region ComponentMin

		/// <summary>
		/// Calculate the component-wise minimum of two vectors
		/// </summary>
		/// <param name="a">First operand</param>
		/// <param name="b">Second operand</param>
		/// <returns>The component-wise minimum</returns>
		public static Vector3D ComponentMin(Vector3D a, Vector3D b) {
			a.X = a.X < b.X ? a.X : b.X;
			a.Y = a.Y < b.Y ? a.Y : b.Y;
			a.Z = a.Z < b.Z ? a.Z : b.Z;
			return a;
		}

		/// <summary>
		/// Calculate the component-wise minimum of two vectors
		/// </summary>
		/// <param name="a">First operand</param>
		/// <param name="b">Second operand</param>
		/// <param name="result">The component-wise minimum</param>
		public static void ComponentMin(ref Vector3D a, ref Vector3D b, out Vector3D result) {
			result.X = a.X < b.X ? a.X : b.X;
			result.Y = a.Y < b.Y ? a.Y : b.Y;
			result.Z = a.Z < b.Z ? a.Z : b.Z;
		}

		#endregion

		#region ComponentMax

		/// <summary>
		/// Calculate the component-wise maximum of two vectors
		/// </summary>
		/// <param name="a">First operand</param>
		/// <param name="b">Second operand</param>
		/// <returns>The component-wise maximum</returns>
		public static Vector3D ComponentMax(Vector3D a, Vector3D b) {
			a.X = a.X > b.X ? a.X : b.X;
			a.Y = a.Y > b.Y ? a.Y : b.Y;
			a.Z = a.Z > b.Z ? a.Z : b.Z;
			return a;
		}

		/// <summary>
		/// Calculate the component-wise maximum of two vectors
		/// </summary>
		/// <param name="a">First operand</param>
		/// <param name="b">Second operand</param>
		/// <param name="result">The component-wise maximum</param>
		public static void ComponentMax(ref Vector3D a, ref Vector3D b, out Vector3D result) {
			result.X = a.X > b.X ? a.X : b.X;
			result.Y = a.Y > b.Y ? a.Y : b.Y;
			result.Z = a.Z > b.Z ? a.Z : b.Z;
		}

		#endregion

		#region Min

		/// <summary>
		/// Returns the Vector3d with the minimum magnitude
		/// </summary>
		/// <param name="left">Left operand</param>
		/// <param name="right">Right operand</param>
		/// <returns>The minimum Vector3</returns>
		public static Vector3D Min(Vector3D left, Vector3D right) {
			return left.LengthSquared < right.LengthSquared ? left : right;
		}

		#endregion

		#region Max

		/// <summary>
		/// Returns the Vector3d with the minimum magnitude
		/// </summary>
		/// <param name="left">Left operand</param>
		/// <param name="right">Right operand</param>
		/// <returns>The minimum Vector3</returns>
		public static Vector3D Max(Vector3D left, Vector3D right) {
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
		public static Vector3D Clamp(Vector3D vec, Vector3D min, Vector3D max) {
			vec.X = vec.X < min.X ? min.X : vec.X > max.X ? max.X : vec.X;
			vec.Y = vec.Y < min.Y ? min.Y : vec.Y > max.Y ? max.Y : vec.Y;
			vec.Z = vec.Z < min.Z ? min.Z : vec.Z > max.Z ? max.Z : vec.Z;
			return vec;
		}

		/// <summary>
		/// Clamp a vector to the given minimum and maximum vectors
		/// </summary>
		/// <param name="vec">Input vector</param>
		/// <param name="min">Minimum vector</param>
		/// <param name="max">Maximum vector</param>
		/// <param name="result">The clamped vector</param>
		public static void Clamp(ref Vector3D vec, ref Vector3D min, ref Vector3D max, out Vector3D result) {
			result.X = vec.X < min.X ? min.X : vec.X > max.X ? max.X : vec.X;
			result.Y = vec.Y < min.Y ? min.Y : vec.Y > max.Y ? max.Y : vec.Y;
			result.Z = vec.Z < min.Z ? min.Z : vec.Z > max.Z ? max.Z : vec.Z;
		}

		#endregion

		#region Normalize

		/// <summary>
		/// Scale a vector to unit length
		/// </summary>
		/// <param name="vec">The input vector</param>
		/// <returns>The normalized vector</returns>
		public static Vector3D Normalize(Vector3D vec) {
			double scale = 1.0 / vec.Length;
			vec.X *= scale;
			vec.Y *= scale;
			vec.Z *= scale;
			return vec;
		}

		/// <summary>
		/// Scale a vector to unit length
		/// </summary>
		/// <param name="vec">The input vector</param>
		/// <param name="result">The normalized vector</param>
		public static void Normalize(ref Vector3D vec, out Vector3D result) {
			double scale = 1.0 / vec.Length;
			result.X = vec.X * scale;
			result.Y = vec.Y * scale;
			result.Z = vec.Z * scale;
		}

		#endregion

		#region NormalizeFast

		/// <summary>
		/// Scale a vector to approximately unit length
		/// </summary>
		/// <param name="vec">The input vector</param>
		/// <returns>The normalized vector</returns>
		public static Vector3D NormalizeFast(Vector3D vec) {
			double scale = MathHelper.InverseSqrtFast((float) (vec.X * vec.X + vec.Y * vec.Y + vec.Z * vec.Z));
			vec.X *= scale;
			vec.Y *= scale;
			vec.Z *= scale;
			return vec;
		}

		/// <summary>
		/// Scale a vector to approximately unit length
		/// </summary>
		/// <param name="vec">The input vector</param>
		/// <param name="result">The normalized vector</param>
		public static void NormalizeFast(ref Vector3D vec, out Vector3D result) {
			double scale = MathHelper.InverseSqrtFast((float) (vec.X * vec.X + vec.Y * vec.Y + vec.Z * vec.Z));
			result.X = vec.X * scale;
			result.Y = vec.Y * scale;
			result.Z = vec.Z * scale;
		}

		#endregion

		#region Dot

		/// <summary>
		/// Calculate the dot (scalar) product of two vectors
		/// </summary>
		/// <param name="left">First operand</param>
		/// <param name="right">Second operand</param>
		/// <returns>The dot product of the two inputs</returns>
		public static double Dot(Vector3D left, Vector3D right) {
			return left.X * right.X + left.Y * right.Y + left.Z * right.Z;
		}

		/// <summary>
		/// Calculate the dot (scalar) product of two vectors
		/// </summary>
		/// <param name="left">First operand</param>
		/// <param name="right">Second operand</param>
		/// <param name="result">The dot product of the two inputs</param>
		public static void Dot(ref Vector3D left, ref Vector3D right, out double result) {
			result = left.X * right.X + left.Y * right.Y + left.Z * right.Z;
		}

		#endregion

		#region Cross

		/// <summary>
		/// Caclulate the cross (vector) product of two vectors
		/// </summary>
		/// <param name="left">First operand</param>
		/// <param name="right">Second operand</param>
		/// <returns>The cross product of the two inputs</returns>
		public static Vector3D Cross(Vector3D left, Vector3D right) {
			Vector3D result;
			Cross(ref left, ref right, out result);
			return result;
		}

		/// <summary>
		/// Caclulate the cross (vector) product of two vectors
		/// </summary>
		/// <param name="left">First operand</param>
		/// <param name="right">Second operand</param>
		/// <returns>The cross product of the two inputs</returns>
		/// <param name="result">The cross product of the two inputs</param>
		public static void Cross(ref Vector3D left, ref Vector3D right, out Vector3D result) {
			result = new Vector3D(left.Y * right.Z - left.Z * right.Y,
				left.Z * right.X - left.X * right.Z,
				left.X * right.Y - left.Y * right.X);
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
		public static Vector3D Lerp(Vector3D a, Vector3D b, double blend) {
			a.X = blend * (b.X - a.X) + a.X;
			a.Y = blend * (b.Y - a.Y) + a.Y;
			a.Z = blend * (b.Z - a.Z) + a.Z;
			return a;
		}

		/// <summary>
		/// Returns a new Vector that is the linear blend of the 2 given Vectors
		/// </summary>
		/// <param name="a">First input vector</param>
		/// <param name="b">Second input vector</param>
		/// <param name="blend">The blend factor. a when blend=0, b when blend=1.</param>
		/// <param name="result">a when blend=0, b when blend=1, and a linear combination otherwise</param>
		public static void Lerp(ref Vector3D a, ref Vector3D b, double blend, out Vector3D result) {
			result.X = blend * (b.X - a.X) + a.X;
			result.Y = blend * (b.Y - a.Y) + a.Y;
			result.Z = blend * (b.Z - a.Z) + a.Z;
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
		public static Vector3D BaryCentric(Vector3D a, Vector3D b, Vector3D c, double u, double v) {
			return a + u * (b - a) + v * (c - a);
		}

		/// <summary>Interpolate 3 Vectors using Barycentric coordinates</summary>
		/// <param name="a">First input Vector.</param>
		/// <param name="b">Second input Vector.</param>
		/// <param name="c">Third input Vector.</param>
		/// <param name="u">First Barycentric Coordinate.</param>
		/// <param name="v">Second Barycentric Coordinate.</param>
		/// <param name="result">Output Vector. a when u=v=0, b when u=1,v=0, c when u=0,v=1, and a linear combination of a,b,c otherwise</param>
		public static void BaryCentric(ref Vector3D a, ref Vector3D b, ref Vector3D c, double u, double v, out Vector3D result) {
			result = a; // copy

			Vector3D temp = b; // copy
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

		/// <summary>Transform a direction vector by the given Matrix
		/// Assumes the matrix has a bottom row of (0,0,0,1), that is the translation part is ignored.
		/// </summary>
		/// <param name="vec">The vector to transform</param>
		/// <param name="mat">The desired transformation</param>
		/// <returns>The transformed vector</returns>
		public static Vector3D TransformVector(Vector3D vec, Matrix4D mat) {
			return new Vector3D(
				Vector3D.Dot(vec, new Vector3D(mat.Column0)),
				Vector3D.Dot(vec, new Vector3D(mat.Column1)),
				Vector3D.Dot(vec, new Vector3D(mat.Column2)));
		}

		/// <summary>Transform a direction vector by the given Matrix
		/// Assumes the matrix has a bottom row of (0,0,0,1), that is the translation part is ignored.
		/// </summary>
		/// <param name="vec">The vector to transform</param>
		/// <param name="mat">The desired transformation</param>
		/// <param name="result">The transformed vector</param>
		public static void TransformVector(ref Vector3D vec, ref Matrix4D mat, out Vector3D result) {
			result.X = vec.X * mat.Row0.X +
					   vec.Y * mat.Row1.X +
					   vec.Z * mat.Row2.X;

			result.Y = vec.X * mat.Row0.Y +
					   vec.Y * mat.Row1.Y +
					   vec.Z * mat.Row2.Y;

			result.Z = vec.X * mat.Row0.Z +
					   vec.Y * mat.Row1.Z +
					   vec.Z * mat.Row2.Z;
		}

		/// <summary>Transform a Normal by the given Matrix</summary>
		/// <remarks>
		/// This calculates the inverse of the given matrix, use TransformNormalInverse if you
		/// already have the inverse to avoid this extra calculation
		/// </remarks>
		/// <param name="norm">The normal to transform</param>
		/// <param name="mat">The desired transformation</param>
		/// <returns>The transformed normal</returns>
		public static Vector3D TransformNormal(Vector3D norm, Matrix4D mat) {
			mat.Invert();
			return TransformNormalInverse(norm, mat);
		}

		/// <summary>Transform a Normal by the given Matrix</summary>
		/// <remarks>
		/// This calculates the inverse of the given matrix, use TransformNormalInverse if you
		/// already have the inverse to avoid this extra calculation
		/// </remarks>
		/// <param name="norm">The normal to transform</param>
		/// <param name="mat">The desired transformation</param>
		/// <param name="result">The transformed normal</param>
		public static void TransformNormal(ref Vector3D norm, ref Matrix4D mat, out Vector3D result) {
			Matrix4D Inverse = Matrix4D.Invert(mat);
			Vector3D.TransformNormalInverse(ref norm, ref Inverse, out result);
		}

		/// <summary>Transform a Normal by the (transpose of the) given Matrix</summary>
		/// <remarks>
		/// This version doesn't calculate the inverse matrix.
		/// Use this version if you already have the inverse of the desired transform to hand
		/// </remarks>
		/// <param name="norm">The normal to transform</param>
		/// <param name="invMat">The inverse of the desired transformation</param>
		/// <returns>The transformed normal</returns>
		public static Vector3D TransformNormalInverse(Vector3D norm, Matrix4D invMat) {
			return new Vector3D(
				Vector3D.Dot(norm, new Vector3D(invMat.Row0)),
				Vector3D.Dot(norm, new Vector3D(invMat.Row1)),
				Vector3D.Dot(norm, new Vector3D(invMat.Row2)));
		}

		/// <summary>Transform a Normal by the (transpose of the) given Matrix</summary>
		/// <remarks>
		/// This version doesn't calculate the inverse matrix.
		/// Use this version if you already have the inverse of the desired transform to hand
		/// </remarks>
		/// <param name="norm">The normal to transform</param>
		/// <param name="invMat">The inverse of the desired transformation</param>
		/// <param name="result">The transformed normal</param>
		public static void TransformNormalInverse(ref Vector3D norm, ref Matrix4D invMat, out Vector3D result) {
			result.X = norm.X * invMat.Row0.X +
					   norm.Y * invMat.Row0.Y +
					   norm.Z * invMat.Row0.Z;

			result.Y = norm.X * invMat.Row1.X +
					   norm.Y * invMat.Row1.Y +
					   norm.Z * invMat.Row1.Z;

			result.Z = norm.X * invMat.Row2.X +
					   norm.Y * invMat.Row2.Y +
					   norm.Z * invMat.Row2.Z;
		}

		/// <summary>Transform a Position by the given Matrix</summary>
		/// <param name="pos">The position to transform</param>
		/// <param name="mat">The desired transformation</param>
		/// <returns>The transformed position</returns>
		public static Vector3D TransformPosition(Vector3D pos, Matrix4D mat) {
			return new Vector3D(
				Vector3D.Dot(pos, new Vector3D(mat.Column0)) + mat.Row3.X,
				Vector3D.Dot(pos, new Vector3D(mat.Column1)) + mat.Row3.Y,
				Vector3D.Dot(pos, new Vector3D(mat.Column2)) + mat.Row3.Z);
		}

		/// <summary>Transform a Position by the given Matrix</summary>
		/// <param name="pos">The position to transform</param>
		/// <param name="mat">The desired transformation</param>
		/// <param name="result">The transformed position</param>
		public static void TransformPosition(ref Vector3D pos, ref Matrix4D mat, out Vector3D result) {
			result.X = pos.X * mat.Row0.X +
					   pos.Y * mat.Row1.X +
					   pos.Z * mat.Row2.X +
					   mat.Row3.X;

			result.Y = pos.X * mat.Row0.Y +
					   pos.Y * mat.Row1.Y +
					   pos.Z * mat.Row2.Y +
					   mat.Row3.Y;

			result.Z = pos.X * mat.Row0.Z +
					   pos.Y * mat.Row1.Z +
					   pos.Z * mat.Row2.Z +
					   mat.Row3.Z;
		}

		/// <summary>Transform a Vector by the given Matrix</summary>
		/// <param name="vec">The vector to transform</param>
		/// <param name="mat">The desired transformation</param>
		/// <returns>The transformed vector</returns>
		public static Vector3D Transform(Vector3D vec, Matrix4D mat) {
			Vector3D result;
			Transform(ref vec, ref mat, out result);
			return result;
		}

		/// <summary>Transform a Vector by the given Matrix</summary>
		/// <param name="vec">The vector to transform</param>
		/// <param name="mat">The desired transformation</param>
		/// <param name="result">The transformed vector</param>
		public static void Transform(ref Vector3D vec, ref Matrix4D mat, out Vector3D result) {
			Vector4D v4 = new Vector4D(vec.X, vec.Y, vec.Z, 1.0);
			Vector4D.Transform(ref v4, ref mat, out v4);
			result = v4.Xyz;
		}

		/// <summary>
		/// Transforms a vector by a quaternion rotation.
		/// </summary>
		/// <param name="vec">The vector to transform.</param>
		/// <param name="quat">The quaternion to rotate the vector by.</param>
		/// <returns>The result of the operation.</returns>
		public static Vector3D Transform(Vector3D vec, Vector4D quat) {
			Vector3D result;
			Transform(ref vec, ref quat, out result);
			return result;
		}

		/// <summary>
		/// Transforms a vector by a quaternion rotation.
		/// </summary>
		/// <param name="vec">The vector to transform.</param>
		/// <param name="quat">The quaternion to rotate the vector by.</param>
		/// <param name="result">The result of the operation.</param>
		public static void Transform(ref Vector3D vec, ref Vector4D quat, out Vector3D result) {
			// Since vec.W == 0, we can optimize quat * vec * quat^-1 as follows:
			// vec + 2.0 * cross(quat.xyz, cross(quat.xyz, vec) + quat.w * vec)
			Vector3D xyz = quat.Xyz, temp, temp2;
			Vector3D.Cross(ref xyz, ref vec, out temp);
			Vector3D.Multiply(ref vec, quat.W, out temp2);
			Vector3D.Add(ref temp, ref temp2, out temp);
			Vector3D.Cross(ref xyz, ref temp, out temp);
			Vector3D.Multiply(ref temp, 2, out temp);
			Vector3D.Add(ref vec, ref temp, out result);
		}

		/// <summary>
		/// Transform a Vector3d by the given Matrix, and project the resulting Vector4 back to a Vector3
		/// </summary>
		/// <param name="vec">The vector to transform</param>
		/// <param name="mat">The desired transformation</param>
		/// <returns>The transformed vector</returns>
		public static Vector3D TransformPerspective(Vector3D vec, Matrix4D mat) {
			Vector3D result;
			TransformPerspective(ref vec, ref mat, out result);
			return result;
		}

		/// <summary>Transform a Vector3d by the given Matrix, and project the resulting Vector4d back to a Vector3d</summary>
		/// <param name="vec">The vector to transform</param>
		/// <param name="mat">The desired transformation</param>
		/// <param name="result">The transformed vector</param>
		public static void TransformPerspective(ref Vector3D vec, ref Matrix4D mat, out Vector3D result) {
			Vector4D v = new Vector4D(vec, 1);
			Vector4D.Transform(ref v, ref mat, out v);
			result.X = v.X / v.W;
			result.Y = v.Y / v.W;
			result.Z = v.Z / v.W;
		}

		#endregion

		#region CalculateAngle

		/// <summary>
		/// Calculates the angle (in radians) between two vectors.
		/// </summary>
		/// <param name="first">The first vector.</param>
		/// <param name="second">The second vector.</param>
		/// <returns>Angle (in radians) between the vectors.</returns>
		/// <remarks>Note that the returned angle is never bigger than the constant Pi.</remarks>
		public static double CalculateAngle(Vector3D first, Vector3D second) {
			return System.Math.Acos((Vector3D.Dot(first, second)) / (first.Length * second.Length));
		}

		/// <summary>Calculates the angle (in radians) between two vectors.</summary>
		/// <param name="first">The first vector.</param>
		/// <param name="second">The second vector.</param>
		/// <param name="result">Angle (in radians) between the vectors.</param>
		/// <remarks>Note that the returned angle is never bigger than the constant Pi.</remarks>
		public static void CalculateAngle(ref Vector3D first, ref Vector3D second, out double result) {
			double temp;
			Vector3D.Dot(ref first, ref second, out temp);
			result = System.Math.Acos(temp / (first.Length * second.Length));
		}

		#endregion

		#endregion

		#region Swizzle

		/// <summary>
		/// Gets or sets an System.Vector2d with the X and Y components of this instance.
		/// </summary>
		public Vector2D Xy {
			get {
				return new Vector2D(X, Y);
			}
			set {
				X = value.X;
				Y = value.Y;
			}
		}

		#endregion

		#region Operators

		/// <summary>
		/// Adds two instances.
		/// </summary>
		/// <param name="left">The first instance.</param>
		/// <param name="right">The second instance.</param>
		/// <returns>The result of the calculation.</returns>
		public static Vector3D operator +(Vector3D left, Vector3D right) {
			left.X += right.X;
			left.Y += right.Y;
			left.Z += right.Z;
			return left;
		}

		/// <summary>
		/// Subtracts two instances.
		/// </summary>
		/// <param name="left">The first instance.</param>
		/// <param name="right">The second instance.</param>
		/// <returns>The result of the calculation.</returns>
		public static Vector3D operator -(Vector3D left, Vector3D right) {
			left.X -= right.X;
			left.Y -= right.Y;
			left.Z -= right.Z;
			return left;
		}

		/// <summary>
		/// Negates an instance.
		/// </summary>
		/// <param name="vec">The instance.</param>
		/// <returns>The result of the calculation.</returns>
		public static Vector3D operator -(Vector3D vec) {
			vec.X = -vec.X;
			vec.Y = -vec.Y;
			vec.Z = -vec.Z;
			return vec;
		}

		/// <summary>
		/// Multiplies an instance by a scalar.
		/// </summary>
		/// <param name="vec">The instance.</param>
		/// <param name="scale">The scalar.</param>
		/// <returns>The result of the calculation.</returns>
		public static Vector3D operator *(Vector3D vec, double scale) {
			vec.X *= scale;
			vec.Y *= scale;
			vec.Z *= scale;
			return vec;
		}

		/// <summary>
		/// Multiplies an instance by a scalar.
		/// </summary>
		/// <param name="scale">The scalar.</param>
		/// <param name="vec">The instance.</param>
		/// <returns>The result of the calculation.</returns>
		public static Vector3D operator *(double scale, Vector3D vec) {
			vec.X *= scale;
			vec.Y *= scale;
			vec.Z *= scale;
			return vec;
		}

		/// <summary>
		/// Multiplies an instance by a Vector3d.
		/// </summary>
		/// <param name="left">The scalar.</param>
		/// <param name="right">The instance.</param>
		/// <returns>The result of the calculation.</returns>
		public static Vector3D operator *(Vector3D left, Vector3D right) {
			left.X *= right.X;
			left.Y *= right.Y;
			left.Z *= right.Z;
			return left;
		}

		/// <summary>
		/// Divides an instance by a scalar.
		/// </summary>
		/// <param name="vec">The instance.</param>
		/// <param name="scale">The scalar.</param>
		/// <returns>The result of the calculation.</returns>
		public static Vector3D operator /(Vector3D vec, double scale) {
			double mult = 1 / scale;
			vec.X *= mult;
			vec.Y *= mult;
			vec.Z *= mult;
			return vec;
		}

		/// <summary>
		/// Compares two instances for equality.
		/// </summary>
		/// <param name="left">The first instance.</param>
		/// <param name="right">The second instance.</param>
		/// <returns>True, if left equals right; false otherwise.</returns>
		public static bool operator ==(Vector3D left, Vector3D right) {
			return left.Equals(right);
		}

		/// <summary>
		/// Compares two instances for inequality.
		/// </summary>
		/// <param name="left">The first instance.</param>
		/// <param name="right">The second instance.</param>
		/// <returns>True, if left does not equa lright; false otherwise.</returns>
		public static bool operator !=(Vector3D left, Vector3D right) {
			return !left.Equals(right);
		}

		/// <summary>Converts System.Vector3 to System.Vector3d.</summary>
		/// <param name="v3">The Vector3 to convert.</param>
		/// <returns>The resulting Vector3d.</returns>
		public static explicit operator Vector3D(Vector3 v3) {
			return new Vector3D(v3.X, v3.Y, v3.Z);
		}

		/// <summary>Converts System.Vector3d to System.Vector3.</summary>
		/// <param name="v3d">The Vector3d to convert.</param>
		/// <returns>The resulting Vector3.</returns>
		public static explicit operator Vector3(Vector3D v3d) {
			return new Vector3((float) v3d.X, (float) v3d.Y, (float) v3d.Z);
		}

		#endregion

		#region Overrides

		#region public override string ToString()

		/// <summary>
		/// Returns a System.String that represents the current Vector3.
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return String.Format("({0}, {1}, {2})", X, Y, Z);
		}

		#endregion

		#region public override int GetHashCode()

		/// <summary>
		/// Returns the hashcode for this instance.
		/// </summary>
		/// <returns>A System.Int32 containing the unique hashcode for this instance.</returns>
		public override int GetHashCode() {
			return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
		}

		#endregion

		#region public override bool Equals(object obj)

		/// <summary>
		/// Indicates whether this instance and a specified object are equal.
		/// </summary>
		/// <param name="obj">The object to compare to.</param>
		/// <returns>True if the instances are equal; false otherwise.</returns>
		public override bool Equals(object obj) {
			if (!(obj is Vector3D))
				return false;

			return this.Equals((Vector3D) obj);
		}

		#endregion

		#endregion

		#endregion

		#region IEquatable<Vector3> Members

		/// <summary>Indicates whether the current vector is equal to another vector.</summary>
		/// <param name="other">A vector to compare with this vector.</param>
		/// <returns>true if the current vector is equal to the vector parameter; otherwise, false.</returns>
		public bool Equals(Vector3D other) {
			return
				X == other.X &&
				Y == other.Y &&
				Z == other.Z;
		}

		#endregion
	}
}