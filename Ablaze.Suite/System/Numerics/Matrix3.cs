using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Numerics {
	/// <summary>
	/// Represents a hardware-accelerated 3x3 matrix containing 3D rotation and scale.
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct Matrix3 : IEquatable<Matrix3> {
		/// <summary>
		/// First row of the matrix.
		/// </summary>
		public Vector3 Row0;
		/// <summary>
		/// Second row of the matrix.
		/// </summary>
		public Vector3 Row1;
		/// <summary>
		/// Third row of the matrix.
		/// </summary>
		public Vector3 Row2;
		/// <summary>
		/// The identity matrix.
		/// </summary>
		public static readonly Matrix3 Identity = new Matrix3(Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ);
		/// <summary>
		/// The zero matrix.
		/// </summary>
		public static readonly Matrix3 Zero = new Matrix3(Vector3.Zero, Vector3.Zero, Vector3.Zero);

		/// <summary>
		/// Gets the determinant of this matrix.
		/// </summary>
		public float Determinant {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return Row0.X * Row1.Y * Row2.Z + Row0.Y * Row1.Z * Row2.X + Row0.Z * Row1.X * Row2.Y
					 - Row0.Z * Row1.Y * Row2.X - Row0.X * Row1.Z * Row2.Y - Row0.Y * Row1.X * Row2.Z;
			}
		}

		/// <summary>
		/// Gets the first column of this matrix.
		/// </summary>
		public Vector3 Column0 {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return new Vector3(Row0.X, Row1.X, Row2.X);
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				Row0.X = value.X;
				Row1.X = value.Y;
				Row2.X = value.Z;
			}
		}

		/// <summary>
		/// Gets the second column of this matrix.
		/// </summary>
		public Vector3 Column1 {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return new Vector3(Row0.Y, Row1.Y, Row2.Y);
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				Row0.Y = value.X;
				Row1.Y = value.Y;
				Row2.Y = value.Z;
			}
		}

		/// <summary>
		/// Gets the third column of this matrix.
		/// </summary>
		public Vector3 Column2 {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return new Vector3(Row0.Z, Row1.Z, Row2.Z);
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				Row0.Z = value.X;
				Row1.Z = value.Y;
				Row2.Z = value.Z;
			}
		}

		/// <summary>
		/// Gets or sets the value at row 1, column 1 of this instance.
		/// </summary>
		public float M11 {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return Row0.X;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				Row0.X = value;
			}
		}

		/// <summary>
		/// Gets or sets the value at row 1, column 2 of this instance.
		/// </summary>
		public float M12 {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return Row0.Y;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				Row0.Y = value;
			}
		}

		/// <summary>
		/// Gets or sets the value at row 1, column 3 of this instance.
		/// </summary>
		public float M13 {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return Row0.Z;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				Row0.Z = value;
			}
		}

		/// <summary>
		/// Gets or sets the value at row 2, column 1 of this instance.
		/// </summary>
		public float M21 {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return Row1.X;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				Row1.X = value;
			}
		}

		/// <summary>
		/// Gets or sets the value at row 2, column 2 of this instance.
		/// </summary>
		public float M22 {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return Row1.Y;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				Row1.Y = value;
			}
		}

		/// <summary>
		/// Gets or sets the value at row 2, column 3 of this instance.
		/// </summary>
		public float M23 {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return Row1.Z;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				Row1.Z = value;
			}
		}

		/// <summary>
		/// Gets or sets the value at row 3, column 1 of this instance.
		/// </summary>
		public float M31 {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return Row2.X;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				Row2.X = value;
			}
		}

		/// <summary>
		/// Gets or sets the value at row 3, column 2 of this instance.
		/// </summary>
		public float M32 {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return Row2.Y;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				Row2.Y = value;
			}
		}

		/// <summary>
		/// Gets or sets the value at row 3, column 3 of this instance.
		/// </summary>
		public float M33 {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return Row2.Z;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				Row2.Z = value;
			}
		}

		/// <summary>
		/// Gets or sets the values along the main diagonal of the matrix.
		/// </summary>
		public Vector3 Diagonal {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return new Vector3(Row0.X, Row1.Y, Row2.Z);
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				Row0.X = value.X;
				Row1.Y = value.Y;
				Row2.Z = value.Z;
			}
		}

		/// <summary>
		/// Gets the trace of the matrix, the sum of the values along the diagonal.
		/// </summary>
		public float Trace {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return Row0.X + Row1.Y + Row2.Z;
			}
		}

		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		/// <param name="row0">Top row of the matrix</param>
		/// <param name="row1">Second row of the matrix</param>
		/// <param name="row2">Bottom row of the matrix</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public Matrix3(Vector3 row0, Vector3 row1, Vector3 row2) {
			Row0 = row0;
			Row1 = row1;
			Row2 = row2;
		}

		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		/// <param name="row0">Top row of the matrix</param>
		/// <param name="row1">Second row of the matrix</param>
		/// <param name="row2">Bottom row of the matrix</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public Matrix3(ref Vector3 row0, ref Vector3 row1, ref Vector3 row2) {
			Row0 = row0;
			Row1 = row1;
			Row2 = row2;
		}

		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		/// <param name="m00">First item of the first row of the matrix.</param>
		/// <param name="m01">Second item of the first row of the matrix.</param>
		/// <param name="m02">Third item of the first row of the matrix.</param>
		/// <param name="m10">First item of the second row of the matrix.</param>
		/// <param name="m11">Second item of the second row of the matrix.</param>
		/// <param name="m12">Third item of the second row of the matrix.</param>
		/// <param name="m20">First item of the third row of the matrix.</param>
		/// <param name="m21">Second item of the third row of the matrix.</param>
		/// <param name="m22">Third item of the third row of the matrix.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public Matrix3(float m00, float m01, float m02, float m10, float m11, float m12, float m20, float m21, float m22) {
			Row0 = new Vector3(m00, m01, m02);
			Row1 = new Vector3(m10, m11, m12);
			Row2 = new Vector3(m20, m21, m22);
		}

		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		/// <param name="matrix">A Matrix4 to take the upper-left 3x3 from.</param>
		public Matrix3(ref Matrix4 matrix) {
			Row0 = new Vector3(matrix.Row0.X, matrix.Row0.Y, matrix.Row0.Z);
			Row1 = new Vector3(matrix.Row1.X, matrix.Row1.Y, matrix.Row1.Z);
			Row2 = new Vector3(matrix.Row2.X, matrix.Row2.Y, matrix.Row2.Z);
		}

		/// <summary>
		/// Divides each element in the Matrix by the <see cref="Determinant"/>.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public Matrix3 Normalized() {
			float determinant = 1f / Determinant;
			return new Matrix3(Row0 * determinant, Row1 * determinant, Row2 * determinant);
		}

		/// <summary>
		/// Outs an inverted copy of this instance. Returns whether the inversion was successful.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool Invert(out Matrix3 matrix) {
			return Invert(ref this, out matrix);
		}

		/// <summary>
		/// Returns a copy of this Matrix3 without scale.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public Matrix3 ClearScale() {
			return new Matrix3(Vector3.Normalize(Row0), Vector3.Normalize(Row1), Vector3.Normalize(Row2));
		}

		/// <summary>
		/// Returns a copy of this Matrix3 without rotation.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public Matrix3 ClearRotation() {
			return new Matrix3(new Vector3(Row0.Length(), 0f, 0f), new Vector3(0f, Row1.Length(), 0f), new Vector3(0f, 0f, Row2.Length()));
		}

		/// <summary>
		/// Returns the scale component of this instance.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public Vector3 ExtractScale() {
			return new Vector3(Row0.Length(), Row1.Length(), Row2.Length());
		}

		/// <summary>
		/// Returns the rotation component of this instance. Quite slow.
		/// </summary>
		/// <param name="rowNormalise">Whether the method should row-normalise (i.e. remove scale from) the Matrix. Pass false if you know it's already normalised.</param>
		public Quaternion ExtractRotation(bool rowNormalise = true) {
			Vector3 row0;
			Vector3 row1;
			Vector3 row2;
			if (rowNormalise) {
				row0 = Vector3.Normalize(Row0);
				row1 = Vector3.Normalize(Row1);
				row2 = Vector3.Normalize(Row2);
			} else {
				row0 = Row0;
				row1 = Row1;
				row2 = Row2;
			}
			Quaternion q = new Quaternion();
			float trace = 0.25f * (row0.X + row1.Y + row2.Z + 1f);
			if (trace > 0f) {
				float sq = (float) Math.Sqrt(trace);
				q.W = sq;
				sq = 1f / (4f * sq);
				q.X = (row1.Z - row2.Y) * sq;
				q.Y = (row2.X - row0.Z) * sq;
				q.Z = (row0.Y - row1.X) * sq;
			} else if (row0.X > row1.Y && row0.X > row2.Z) {
				float sq = 2f * (float) Math.Sqrt(1f + row0.X - row1.Y - row2.Z);
				q.X = 0.25f * sq;
				sq = 1f / sq;
				q.W = (row2.Y - row1.Z) * sq;
				q.Y = (row1.X + row0.Y) * sq;
				q.Z = (row2.X + row0.Z) * sq;
			} else if (row1.Y > row2.Z) {
				float sq = 2f * (float) Math.Sqrt(1f + row1.Y - row0.X - row2.Z);
				q.Y = 0.25f * sq;
				sq = 1f / sq;
				q.W = (row2.X - row0.Z) * sq;
				q.X = (row1.X + row0.Y) * sq;
				q.Z = (row2.Y + row1.Z) * sq;
			} else {
				float sq = 2f * (float) Math.Sqrt(1f + row2.Z - row0.X - row1.Y);
				q.Z = 0.25f * sq;
				sq = 1f / sq;
				q.W = (row1.X - row0.Y) * sq;
				q.X = (row2.X + row0.Z) * sq;
				q.Y = (row2.Y + row1.Z) * sq;
			}
			return Quaternion.Normalize(q);
		}

		/// <summary>
		/// Returns the cross product of a vector.
		/// </summary>
		/// <param name="v">The vector to use as input.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Matrix3 CrossProductMatrix(ref Vector3 v) {
			return new Matrix3() {
				M12 = -v.Z,
				M13 = v.Y,
				M21 = v.Z,
				M23 = -v.X,
				M31 = -v.Y,
				M32 = v.X
			};
		}

		/// <summary>
		/// Build a rotation matrix from the specified axis/angle rotation.
		/// </summary>
		/// <param name="axis">The axis to rotate about.</param>
		/// <param name="angle">Angle in radians to rotate counter-clockwise (looking in the direction of the given axis).</param>
		/// <param name="result">A matrix instance.</param>
		public static void CreateFromAxisAngle(ref Vector3 axis, float angle, out Matrix3 result) {
			//normalize and create a local copy of the vector.
			Vector3 norm = Vector3.Normalize(axis);

			//calculate angles
			float cos = (float) Math.Cos(-angle);
			float sin = (float) Math.Sin(-angle);
			float t = 1f - cos;

			//do the conversion math once
			float tXX = t * norm.X * norm.X,
			tXY = t * norm.X * norm.Y,
			tXZ = t * norm.X * norm.Z,
			tYY = t * norm.Y * norm.Y,
			tYZ = t * norm.Y * norm.Z,
			tZZ = t * norm.Z * norm.Z;

			float sinX = sin * norm.X,
			sinY = sin * norm.Y,
			sinZ = sin * norm.Z;
			result = new Matrix3(tXX + cos, tXY - sinZ, tXZ + sinY, tXY + sinZ, tYY + cos, tYZ - sinX, tXZ - sinY, tYZ + sinX, tZZ + cos);
		}

		/// <summary>
		/// Build a rotation matrix from the specified quaternion.
		/// </summary>
		/// <param name="q">Quaternion to translate.</param>
		/// <param name="result">Matrix result.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void CreateFromQuaternion(ref Quaternion q, out Matrix3 result) {
			Quaternion quat = q.ToAxisAngle();
			Vector3 axis = new Vector3(quat.X, quat.Y, quat.Z);
			CreateFromAxisAngle(ref axis, quat.W, out result);
		}

		/// <summary>
		/// Builds a rotation matrix for a rotation around the x-axis.
		/// </summary>
		/// <param name="angle">The counter-clockwise angle in radians.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Matrix3 CreateRotationX(float angle) {
			float cos = (float) Math.Cos(angle);
			float sin = (float) Math.Sin(angle);
			return new Matrix3(Vector3.UnitX, new Vector3(0f, cos, sin), new Vector3(0f, -sin, cos));
		}

		/// <summary>
		/// Builds a rotation matrix for a rotation around the y-axis.
		/// </summary>
		/// <param name="angle">The counter-clockwise angle in radians.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Matrix3 CreateRotationY(float angle) {
			float cos = (float) Math.Cos(angle);
			float sin = (float) Math.Sin(angle);
			return new Matrix3(new Vector3(cos, 0f, -sin), Vector3.UnitY, new Vector3(sin, 0f, cos));
		}

		/// <summary>
		/// Builds a rotation matrix for a rotation around the z-axis.
		/// </summary>
		/// <param name="angle">The counter-clockwise angle in radians.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Matrix3 CreateRotationZ(float angle) {
			float cos = (float) Math.Cos(angle);
			float sin = (float) Math.Sin(angle);
			return new Matrix3(new Vector3(cos, sin, 0f), new Vector3(-sin, cos, 0f), Vector3.UnitZ);
		}

		/// <summary>
		/// Creates a scale matrix.
		/// </summary>
		/// <param name="scale">Single scale factor for the x, y, and z axes.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Matrix3 CreateScale(float scale) {
			Matrix3 result = new Matrix3();
			result.Row0.X = scale;
			result.Row1.Y = scale;
			result.Row2.Z = scale;
			return result;
		}

		/// <summary>
		/// Creates a scale matrix.
		/// </summary>
		/// <param name="scale">Scale factors for the x, y, and z axes.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Matrix3 CreateScale(ref Vector3 scale) {
			Matrix3 result = new Matrix3();
			result.Row0.X = scale.X;
			result.Row1.Y = scale.Y;
			result.Row2.Z = scale.Z;
			return result;
		}

		/// <summary>
		/// Creates a scale matrix.
		/// </summary>
		/// <param name="x">Scale factor for the x axis.</param>
		/// <param name="y">Scale factor for the y axis.</param>
		/// <param name="z">Scale factor for the z axis.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Matrix3 CreateScale(float x, float y, float z) {
			Matrix3 result = new Matrix3();
			result.Row0.X = x;
			result.Row1.Y = y;
			result.Row2.Z = z;
			return result;
		}

		/// <summary>
		/// Adds two instances.
		/// </summary>
		/// <param name="left">The left operand of the addition.</param>
		/// <param name="right">The right operand of the addition.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Matrix3 Add(ref Matrix3 left, ref Matrix3 right) {
			return new Matrix3(left.Row0 + right.Row0, left.Row1 + right.Row1, left.Row2 + right.Row2);
		}

		/// <summary>
		/// Subtracts the second matrix from the first.
		/// </summary>
		/// <param name="left">The left operand of the subtraction.</param>
		/// <param name="right">The right operand of the subtraction.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Matrix3 Subtract(ref Matrix3 left, ref Matrix3 right) {
			return new Matrix3(left.Row0 - right.Row0, left.Row1 - right.Row1, left.Row2 - right.Row2);
		}

		/// <summary>
		/// The matrix to negate (not invert, ie. 0 - matrix)
		/// </summary>
		/// <param name="matrix">The matrix to negate.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Matrix3 Negate(ref Matrix3 matrix) {
			return new Matrix3(-matrix.Row0, -matrix.Row1, -matrix.Row2);
		}

		/// <summary>
		/// Multiplies two instances.
		/// </summary>
		/// <param name="left">The left operand of the multiplication.</param>
		/// <param name="right">The right operand of the multiplication.</param>
		/// <returns>A new instance that is the result of the multiplication</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Matrix3 Mult(ref Matrix3 left, float right) {
			return new Matrix3(left.Row0 * right, left.Row1 * right, left.Row2 * right);
		}

		/// <summary>
		/// Multiplies two instances.
		/// </summary>
		/// <param name="matrix">The left operand of the multiplication.</param>
		/// <param name="vertex">The right operand of the multiplication.</param>
		/// <returns>A new instance that is the result of the multiplication</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector3 Mult(ref Matrix3 matrix, ref Vector3 vertex) {
			return vertex * matrix.Row0 + vertex * matrix.Row1 + vertex * matrix.Row2;
		}

		/// <summary>
		/// Multiplies two instances.
		/// </summary>
		/// <param name="left">The left operand of the multiplication.</param>
		/// <param name="right">The right operand of the multiplication.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Matrix3 Mult(ref Matrix3 left, ref Matrix3 right) {
			float lM11 = left.Row0.X, lM12 = left.Row0.Y, lM13 = left.Row0.Z,
			lM21 = left.Row1.X, lM22 = left.Row1.Y, lM23 = left.Row1.Z,
			lM31 = left.Row2.X, lM32 = left.Row2.Y, lM33 = left.Row2.Z,
			rM11 = right.Row0.X, rM12 = right.Row0.Y, rM13 = right.Row0.Z,
			rM21 = right.Row1.X, rM22 = right.Row1.Y, rM23 = right.Row1.Z,
			rM31 = right.Row2.X, rM32 = right.Row2.Y, rM33 = right.Row2.Z;

			return new Matrix3(lM11 * rM11 + lM12 * rM21 + lM13 * rM31, lM11 * rM12 + lM12 * rM22 + lM13 * rM32, lM11 * rM13 + lM12 * rM23 + lM13 * rM33,
				lM21 * rM11 + lM22 * rM21 + lM23 * rM31, lM21 * rM12 + lM22 * rM22 + lM23 * rM32, lM21 * rM13 + lM22 * rM23 + lM23 * rM33,
				lM31 * rM11 + lM32 * rM21 + lM33 * rM31, lM31 * rM12 + lM32 * rM22 + lM33 * rM32, lM31 * rM13 + lM32 * rM23 + lM33 * rM33);
		}

		/// <summary>
		/// Calculate the inverse of the given matrix, and returns whether the inversion was successful.
		/// </summary>
		/// <param name="mat">The matrix to invert</param>
		/// <param name="result">The inverse of the given matrix if it has one, or the input if it is singular</param>
		public static bool Invert(ref Matrix3 mat, out Matrix3 result) {
			int[] colIdx = { 0, 0, 0 };
			int[] rowIdx = { 0, 0, 0 };
			int[] pivotIdx = { -1, -1, -1 };

			float[][] inverse = new float[3][];
			inverse[0] = new float[] { mat.Row0.X, mat.Row0.Y, mat.Row0.Z };
			inverse[1] = new float[] { mat.Row1.X, mat.Row1.Y, mat.Row1.Z };
			inverse[2] = new float[] { mat.Row2.X, mat.Row2.Y, mat.Row2.Z };
			int icol = 0;
			int irow = 0;
			for (int i = 0; i < 3; i++) {
				float maxPivot = 0f;
				for (int j = 0; j < 3; j++) {
					if (pivotIdx[j] != 0) {
						for (int k = 0; k < 3; ++k) {
							if (pivotIdx[k] == -1) {
								float absVal = Math.Abs(inverse[j][k]);
								if (absVal > maxPivot) {
									maxPivot = absVal;
									irow = j;
									icol = k;
								}
							} else if (pivotIdx[k] > 0) {
								result = mat;
								return false;
							}
						}
					}
				}

				++(pivotIdx[icol]);

				if (irow != icol) {
					for (int k = 0; k < 3; ++k) {
						float f = inverse[irow][k];
						inverse[irow][k] = inverse[icol][k];
						inverse[icol][k] = f;
					}
				}

				rowIdx[i] = irow;
				colIdx[i] = icol;

				float pivot = inverse[icol][icol];

				if (pivot == 0f) {
					result = mat;
					return false;
				}

				float oneOverPivot = 1f / pivot;
				inverse[icol][icol] = 1f;
				for (int k = 0; k < 3; ++k)
					inverse[icol][k] *= oneOverPivot;

				for (int j = 0; j < 3; ++j) {
					if (icol != j) {
						float f = inverse[j][icol];
						inverse[j][icol] = 0f;
						for (int k = 0; k < 3; ++k)
							inverse[j][k] -= inverse[icol][k] * f;
					}
				}
			}

			for (int j = 2; j >= 0; --j) {
				int ir = rowIdx[j];
				int ic = colIdx[j];
				for (int k = 0; k < 3; ++k) {
					float f = inverse[k][ir];
					inverse[k][ir] = inverse[k][ic];
					inverse[k][ic] = f;
				}
			}

			result = new Matrix3(inverse[0][0], inverse[0][1], inverse[0][2], inverse[1][0], inverse[1][1], inverse[1][2], inverse[2][0], inverse[2][1], inverse[2][2]);
			return true;
		}

		/// <summary>
		/// Calculate the transpose of the given matrix
		/// </summary>
		/// <param name="mat">The matrix to transpose</param>
		/// <returns>The transpose of the given matrix</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Matrix3 Transpose(ref Matrix3 mat) {
			return new Matrix3(mat.Column0, mat.Column1, mat.Column2);
		}

		/// <summary>
		/// Calculate the transpose of the given matrix
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public Matrix3 Transpose() {
			return new Matrix3(Column0, Column1, Column2);
		}

		/// <summary>
		/// Matrix addition
		/// </summary>
		/// <param name="left">left-hand operand</param>
		/// <param name="right">right-hand operand</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Matrix3 operator +(Matrix3 left, Matrix3 right) {
			return Add(ref left, ref right);
		}

		/// <summary>
		/// Does nothing
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Matrix3 operator +(Matrix3 matrix) {
			return matrix;
		}

		/// <summary>
		/// Negates the matrix
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Matrix3 operator -(Matrix3 matrix) {
			return Negate(ref matrix);
		}

		/// <summary>
		/// Matrix subtraction
		/// </summary>
		/// <param name="left">left-hand operand</param>
		/// <param name="right">right-hand operand</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Matrix3 operator -(Matrix3 left, Matrix3 right) {
			return Subtract(ref left, ref right);
		}

		/// <summary>
		/// Matrix multiplication
		/// </summary>
		/// <param name="left">left-hand operand</param>
		/// <param name="right">right-hand operand</param>
		/// <returns>A new Matrix3 which holds the result of the multiplication</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Matrix3 operator *(Matrix3 left, Matrix3 right) {
			return Mult(ref left, ref right);
		}

		/// <summary>
		/// Multiplies the matrix with the vector
		/// </summary>
		/// <param name="left">The matrix</param>
		/// <param name="right">The vector</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector3 operator *(Matrix3 left, Vector3 right) {
			return Mult(ref left, ref right);
		}

		/// <summary>
		/// Multiplies the matrix with the scalar
		/// </summary>
		/// <param name="left">The matrix</param>
		/// <param name="right">The scalar</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Matrix3 operator *(Matrix3 left, float right) {
			return Mult(ref left, right);
		}

		/// <summary>
		/// Multiplies the matrix with the scalar
		/// </summary>
		/// <param name="left">The scalar</param>
		/// <param name="right">The matrix</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Matrix3 operator *(float left, Matrix3 right) {
			return Mult(ref right, left);
		}

		/// <summary>
		/// Compares two instances for equality.
		/// </summary>
		/// <param name="left">The first instance.</param>
		/// <param name="right">The second instance.</param>
		/// <returns>True, if left equals right; false otherwise.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator ==(Matrix3 left, Matrix3 right) {
			return left.Equals(ref right);
		}

		/// <summary>
		/// Compares two instances for inequality.
		/// </summary>
		/// <param name="left">The first instance.</param>
		/// <param name="right">The second instance.</param>
		/// <returns>True, if left does not equal right; false otherwise.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator !=(Matrix3 left, Matrix3 right) {
			return !left.Equals(ref right);
		}

		/// <summary>
		/// Returns a System.String that represents the current Matrix3d.
		/// </summary>
		/// <returns>The string representation of the matrix.</returns>
		public override string ToString() {
			return "{" + Row0.ToString() + "\n" + Row1.ToString() + "\n" + Row2.ToString() + "}";
		}

		/// <summary>
		/// Returns the hashcode for this instance.
		/// </summary>
		/// <returns>A System.Int32 containing the unique hashcode for this instance.</returns>
		public override int GetHashCode() {
			return unchecked((Row0.GetHashCode() << 17) ^ (Row1.GetHashCode() << 7) ^ Row2.GetHashCode());
		}

		/// <summary>
		/// Indicates whether this instance and a specified object are equal.
		/// </summary>
		/// <param name="obj">The object to compare to.</param>
		/// <returns>True if the instances are equal; false otherwise.</returns>
		public override bool Equals(object obj) {
			return obj is Matrix3 ? Equals((Matrix3) obj) : false;
		}

		/// <summary>Indicates whether the current matrix is equal to another matrix.</summary>
		/// <param name="other">A matrix to compare with this matrix.</param>
		/// <returns>true if the current matrix is equal to the matrix parameter; otherwise, false.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool Equals(Matrix3 other) {
			return Row0 == other.Row0 && Row1 == other.Row1 && Row2 == other.Row2;
		}

		/// <summary>Indicates whether the current matrix is equal to another matrix.</summary>
		/// <param name="other">A matrix to compare with this matrix.</param>
		/// <returns>true if the current matrix is equal to the matrix parameter; otherwise, false.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public bool Equals(ref Matrix3 other) {
			return Row0 == other.Row0 && Row1 == other.Row1 && Row2 == other.Row2;
		}
	}
}