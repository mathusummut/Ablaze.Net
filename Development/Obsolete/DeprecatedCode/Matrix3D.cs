using System.Runtime.InteropServices;

namespace System.Drawing {
	/// <summary>
	/// Represents a 3x3 matrix containing 3D rotation and scale with double-precision components.
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct Matrix3D : IEquatable<Matrix3D> {
		#region Fields

		/// <summary>
		/// First row of the matrix.
		/// </summary>
		public Vector3D Row0;

		/// <summary>
		/// Second row of the matrix.
		/// </summary>
		public Vector3D Row1;

		/// <summary>
		/// Third row of the matrix.
		/// </summary>
		public Vector3D Row2;

		/// <summary>
		/// The identity matrix.
		/// </summary>
		public static readonly Matrix3D Identity = new Matrix3D(Vector3D.UnitX, Vector3D.UnitY, Vector3D.UnitZ);

		#endregion

		#region Constructors

		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		/// <param name="row0">Top row of the matrix</param>
		/// <param name="row1">Second row of the matrix</param>
		/// <param name="row2">Bottom row of the matrix</param>
		public Matrix3D(Vector3D row0, Vector3D row1, Vector3D row2) {
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
		public Matrix3D(
			double m00, double m01, double m02,
			double m10, double m11, double m12,
			double m20, double m21, double m22) {
			Row0 = new Vector3D(m00, m01, m02);
			Row1 = new Vector3D(m10, m11, m12);
			Row2 = new Vector3D(m20, m21, m22);
		}

		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		/// <param name="matrix">A Matrix4d to take the upper-left 3x3 from.</param>
		public Matrix3D(Matrix4D matrix) {
			Row0 = matrix.Row0.Xyz;
			Row1 = matrix.Row1.Xyz;
			Row2 = matrix.Row2.Xyz;
		}

		#endregion

		#region Public Members

		#region Properties

		/// <summary>
		/// Gets the determinant of this matrix.
		/// </summary>
		public double Determinant {
			get {
				double m11 = Row0.X, m12 = Row0.Y, m13 = Row0.Z,
				m21 = Row1.X, m22 = Row1.Y, m23 = Row1.Z,
				m31 = Row2.X, m32 = Row2.Y, m33 = Row2.Z;

				return
					m11 * m22 * m33 + m12 * m23 * m31 + m13 * m21 * m32
						- m13 * m22 * m31 - m11 * m23 * m32 - m12 * m21 * m33;
			}
		}

		/// <summary>
		/// Gets the first column of this matrix.
		/// </summary>
		public Vector3D Column0 {
			get {
				return new Vector3D(Row0.X, Row1.X, Row2.X);
			}
		}

		/// <summary>
		/// Gets the second column of this matrix.
		/// </summary>
		public Vector3D Column1 {
			get {
				return new Vector3D(Row0.Y, Row1.Y, Row2.Y);
			}
		}

		/// <summary>
		/// Gets the third column of this matrix.
		/// </summary>
		public Vector3D Column2 {
			get {
				return new Vector3D(Row0.Z, Row1.Z, Row2.Z);
			}
		}

		/// <summary>
		/// Gets or sets the value at row 1, column 1 of this instance.
		/// </summary>
		public double M11 {
			get {
				return Row0.X;
			}
			set {
				Row0.X = value;
			}
		}

		/// <summary>
		/// Gets or sets the value at row 1, column 2 of this instance.
		/// </summary>
		public double M12 {
			get {
				return Row0.Y;
			}
			set {
				Row0.Y = value;
			}
		}

		/// <summary>
		/// Gets or sets the value at row 1, column 3 of this instance.
		/// </summary>
		public double M13 {
			get {
				return Row0.Z;
			}
			set {
				Row0.Z = value;
			}
		}

		/// <summary>
		/// Gets or sets the value at row 2, column 1 of this instance.
		/// </summary>
		public double M21 {
			get {
				return Row1.X;
			}
			set {
				Row1.X = value;
			}
		}

		/// <summary>
		/// Gets or sets the value at row 2, column 2 of this instance.
		/// </summary>
		public double M22 {
			get {
				return Row1.Y;
			}
			set {
				Row1.Y = value;
			}
		}

		/// <summary>
		/// Gets or sets the value at row 2, column 3 of this instance.
		/// </summary>
		public double M23 {
			get {
				return Row1.Z;
			}
			set {
				Row1.Z = value;
			}
		}

		/// <summary>
		/// Gets or sets the value at row 3, column 1 of this instance.
		/// </summary>
		public double M31 {
			get {
				return Row2.X;
			}
			set {
				Row2.X = value;
			}
		}

		/// <summary>
		/// Gets or sets the value at row 3, column 2 of this instance.
		/// </summary>
		public double M32 {
			get {
				return Row2.Y;
			}
			set {
				Row2.Y = value;
			}
		}

		/// <summary>
		/// Gets or sets the value at row 3, column 3 of this instance.
		/// </summary>
		public double M33 {
			get {
				return Row2.Z;
			}
			set {
				Row2.Z = value;
			}
		}

		/// <summary>
		/// Gets or sets the values along the main diagonal of the matrix.
		/// </summary>
		public Vector3D Diagonal {
			get {
				return new Vector3D(Row0.X, Row1.Y, Row2.Z);
			}
			set {
				Row0.X = value.X;
				Row1.Y = value.Y;
				Row2.Z = value.Z;
			}
		}

		/// <summary>
		/// Gets the trace of the matrix, the sum of the values along the diagonal.
		/// </summary>
		public double Trace {
			get {
				return Row0.X + Row1.Y + Row2.Z;
			}
		}

		#endregion

		#region Instance

		#region public void Invert()

		/// <summary>
		/// Converts this instance into its inverse.
		/// </summary>
		public void Invert() {
			this = Matrix3D.Invert(this);
		}

		#endregion

		#region public void Transpose()

		/// <summary>
		/// Converts this instance into its transpose.
		/// </summary>
		public void Transpose() {
			this = Matrix3D.Transpose(this);
		}

		#endregion

		/// <summary>
		/// Returns a normalised copy of this instance.
		/// </summary>
		public Matrix3D Normalized() {
			Matrix3D m = this;
			m.Normalize();
			return m;
		}

		/// <summary>
		/// Divides each element in the Matrix by the <see cref="Determinant"/>.
		/// </summary>
		public void Normalize() {
			var determinant = this.Determinant;
			Row0 /= determinant;
			Row1 /= determinant;
			Row2 /= determinant;
		}

		/// <summary>
		/// Returns an inverted copy of this instance.
		/// </summary>
		public Matrix3D Inverted() {
			Matrix3D m = this;
			if (m.Determinant != 0)
				m.Invert();
			return m;
		}


		/// <summary>
		/// Returns a copy of this Matrix3 without scale.
		/// </summary>
		public Matrix3D ClearScale() {
			Matrix3D m = this;
			m.Row0.Normalize();
			m.Row1.Normalize();
			m.Row2.Normalize();
			return m;
		}
		/// <summary>
		/// Returns a copy of this Matrix3 without rotation.
		/// </summary>
		public Matrix3D ClearRotation() {
			Matrix3D m = this;
			m.Row0 = new Vector3D(m.Row0.Length, 0, 0);
			m.Row1 = new Vector3D(0, m.Row1.Length, 0);
			m.Row2 = new Vector3D(0, 0, m.Row2.Length);
			return m;
		}

		/// <summary>
		/// Returns the scale component of this instance.
		/// </summary>
		public Vector3D ExtractScale() {
			return new Vector3D(Row0.Length, Row1.Length, Row2.Length);
		}

		/// <summary>
		/// Returns the rotation component of this instance. Quite slow.
		/// </summary>
		/// <param name="row_normalise">Whether the method should row-normalise (i.e. remove scale from) the Matrix. Pass false if you know it's already normalised.</param>
		public QuaternionD ExtractRotation(bool row_normalise = true) {
			var row0 = Row0;
			var row1 = Row1;
			var row2 = Row2;

			if (row_normalise) {
				row0.Normalize();
				row1.Normalize();
				row2.Normalize();
			}

			// code below adapted from Blender

			QuaternionD q = new QuaternionD();
			double trace = 0.25 * (row0.X + row1.Y + row2.Z + 1.0);

			if (trace > 0) {
				double sq = Math.Sqrt(trace);

				q.W = sq;
				sq = 1.0 / (4.0 * sq);
				q.X = (row1.Z - row2.Y) * sq;
				q.Y = (row2.X - row0.Z) * sq;
				q.Z = (row0.Y - row1.X) * sq;
			} else if (row0.X > row1.Y && row0.X > row2.Z) {
				double sq = 2.0 * Math.Sqrt(1.0 + row0.X - row1.Y - row2.Z);

				q.X = 0.25 * sq;
				sq = 1.0 / sq;
				q.W = (row2.Y - row1.Z) * sq;
				q.Y = (row1.X + row0.Y) * sq;
				q.Z = (row2.X + row0.Z) * sq;
			} else if (row1.Y > row2.Z) {
				double sq = 2.0 * Math.Sqrt(1.0 + row1.Y - row0.X - row2.Z);

				q.Y = 0.25 * sq;
				sq = 1.0 / sq;
				q.W = (row2.X - row0.Z) * sq;
				q.X = (row1.X + row0.Y) * sq;
				q.Z = (row2.Y + row1.Z) * sq;
			} else {
				double sq = 2.0 * Math.Sqrt(1.0 + row2.Z - row0.X - row1.Y);

				q.Z = 0.25 * sq;
				sq = 1.0 / sq;
				q.W = (row1.X - row0.Y) * sq;
				q.X = (row2.X + row0.Z) * sq;
				q.Y = (row2.Y + row1.Z) * sq;
			}

			q.Normalize();
			return q;
		}

		#endregion

		#region Static

		#region CreateFromAxisAngle

		/// <summary>
		/// Build a rotation matrix from the specified axis/angle rotation.
		/// </summary>
		/// <param name="axis">The axis to rotate about.</param>
		/// <param name="angle">Angle in radians to rotate counter-clockwise (looking in the direction of the given axis).</param>
		/// <param name="result">A matrix instance.</param>
		public static void CreateFromAxisAngle(Vector3D axis, double angle, out Matrix3D result) {
			//normalize and create a local copy of the vector.
			axis.Normalize();
			double axisX = axis.X, axisY = axis.Y, axisZ = axis.Z;

			//calculate angles
			double cos = System.Math.Cos(-angle);
			double sin = System.Math.Sin(-angle);
			double t = 1f - cos;

			//do the conversion math once
			double tXX = t * axisX * axisX,
			tXY = t * axisX * axisY,
			tXZ = t * axisX * axisZ,
			tYY = t * axisY * axisY,
			tYZ = t * axisY * axisZ,
			tZZ = t * axisZ * axisZ;

			double sinX = sin * axisX,
			sinY = sin * axisY,
			sinZ = sin * axisZ;

			result.Row0.X = tXX + cos;
			result.Row0.Y = tXY - sinZ;
			result.Row0.Z = tXZ + sinY;
			result.Row1.X = tXY + sinZ;
			result.Row1.Y = tYY + cos;
			result.Row1.Z = tYZ - sinX;
			result.Row2.X = tXZ - sinY;
			result.Row2.Y = tYZ + sinX;
			result.Row2.Z = tZZ + cos;
		}

		/// <summary>
		/// Build a rotation matrix from the specified axis/angle rotation.
		/// </summary>
		/// <param name="axis">The axis to rotate about.</param>
		/// <param name="angle">Angle in radians to rotate counter-clockwise (looking in the direction of the given axis).</param>
		/// <returns>A matrix instance.</returns>
		public static Matrix3D CreateFromAxisAngle(Vector3D axis, double angle) {
			Matrix3D result;
			CreateFromAxisAngle(axis, angle, out result);
			return result;
		}

		#endregion

		#region CreateFromQuaternion

		/// <summary>
		/// Build a rotation matrix from the specified quaternion.
		/// </summary>
		/// <param name="q">Quaternion to translate.</param>
		/// <param name="result">Matrix result.</param>
		public static void CreateFromQuaternion(ref QuaternionD q, out Matrix3D result) {
			Vector3D axis;
			double angle;
			q.ToAxisAngle(out axis, out angle);
			CreateFromAxisAngle(axis, angle, out result);
		}

		/// <summary>
		/// Build a rotation matrix from the specified quaternion.
		/// </summary>
		/// <param name="q">Quaternion to translate.</param>
		/// <returns>A matrix instance.</returns>
		public static Matrix3D CreateFromQuaternion(QuaternionD q) {
			Matrix3D result;
			CreateFromQuaternion(ref q, out result);
			return result;
		}

		#endregion

		#region CreateRotation[XYZ]

		/// <summary>
		/// Builds a rotation matrix for a rotation around the x-axis.
		/// </summary>
		/// <param name="angle">The counter-clockwise angle in radians.</param>
		/// <param name="result">The resulting Matrix3D instance.</param>
		public static void CreateRotationX(double angle, out Matrix3D result) {
			double cos = System.Math.Cos(angle);
			double sin = System.Math.Sin(angle);

			result = Identity;
			result.Row1.Y = cos;
			result.Row1.Z = sin;
			result.Row2.Y = -sin;
			result.Row2.Z = cos;
		}

		/// <summary>
		/// Builds a rotation matrix for a rotation around the x-axis.
		/// </summary>
		/// <param name="angle">The counter-clockwise angle in radians.</param>
		/// <returns>The resulting Matrix3D instance.</returns>
		public static Matrix3D CreateRotationX(double angle) {
			Matrix3D result;
			CreateRotationX(angle, out result);
			return result;
		}

		/// <summary>
		/// Builds a rotation matrix for a rotation around the y-axis.
		/// </summary>
		/// <param name="angle">The counter-clockwise angle in radians.</param>
		/// <param name="result">The resulting Matrix3D instance.</param>
		public static void CreateRotationY(double angle, out Matrix3D result) {
			double cos = System.Math.Cos(angle);
			double sin = System.Math.Sin(angle);

			result = Identity;
			result.Row0.X = cos;
			result.Row0.Z = -sin;
			result.Row2.X = sin;
			result.Row2.Z = cos;
		}

		/// <summary>
		/// Builds a rotation matrix for a rotation around the y-axis.
		/// </summary>
		/// <param name="angle">The counter-clockwise angle in radians.</param>
		/// <returns>The resulting Matrix3D instance.</returns>
		public static Matrix3D CreateRotationY(double angle) {
			Matrix3D result;
			CreateRotationY(angle, out result);
			return result;
		}

		/// <summary>
		/// Builds a rotation matrix for a rotation around the z-axis.
		/// </summary>
		/// <param name="angle">The counter-clockwise angle in radians.</param>
		/// <param name="result">The resulting Matrix3D instance.</param>
		public static void CreateRotationZ(double angle, out Matrix3D result) {
			double cos = System.Math.Cos(angle);
			double sin = System.Math.Sin(angle);

			result = Identity;
			result.Row0.X = cos;
			result.Row0.Y = sin;
			result.Row1.X = -sin;
			result.Row1.Y = cos;
		}

		/// <summary>
		/// Builds a rotation matrix for a rotation around the z-axis.
		/// </summary>
		/// <param name="angle">The counter-clockwise angle in radians.</param>
		/// <returns>The resulting Matrix3D instance.</returns>
		public static Matrix3D CreateRotationZ(double angle) {
			Matrix3D result;
			CreateRotationZ(angle, out result);
			return result;
		}

		#endregion

		#region CreateScale

		/// <summary>
		/// Creates a scale matrix.
		/// </summary>
		/// <param name="scale">Single scale factor for the x, y, and z axes.</param>
		/// <returns>A scale matrix.</returns>
		public static Matrix3D CreateScale(double scale) {
			Matrix3D result;
			CreateScale(scale, out result);
			return result;
		}

		/// <summary>
		/// Creates a scale matrix.
		/// </summary>
		/// <param name="scale">Scale factors for the x, y, and z axes.</param>
		/// <returns>A scale matrix.</returns>
		public static Matrix3D CreateScale(Vector3D scale) {
			Matrix3D result;
			CreateScale(ref scale, out result);
			return result;
		}

		/// <summary>
		/// Creates a scale matrix.
		/// </summary>
		/// <param name="x">Scale factor for the x axis.</param>
		/// <param name="y">Scale factor for the y axis.</param>
		/// <param name="z">Scale factor for the z axis.</param>
		/// <returns>A scale matrix.</returns>
		public static Matrix3D CreateScale(double x, double y, double z) {
			Matrix3D result;
			CreateScale(x, y, z, out result);
			return result;
		}

		/// <summary>
		/// Creates a scale matrix.
		/// </summary>
		/// <param name="scale">Single scale factor for the x, y, and z axes.</param>
		/// <param name="result">A scale matrix.</param>
		public static void CreateScale(double scale, out Matrix3D result) {
			result = Identity;
			result.Row0.X = scale;
			result.Row1.Y = scale;
			result.Row2.Z = scale;
		}

		/// <summary>
		/// Creates a scale matrix.
		/// </summary>
		/// <param name="scale">Scale factors for the x, y, and z axes.</param>
		/// <param name="result">A scale matrix.</param>
		public static void CreateScale(ref Vector3D scale, out Matrix3D result) {
			result = Identity;
			result.Row0.X = scale.X;
			result.Row1.Y = scale.Y;
			result.Row2.Z = scale.Z;
		}

		/// <summary>
		/// Creates a scale matrix.
		/// </summary>
		/// <param name="x">Scale factor for the x axis.</param>
		/// <param name="y">Scale factor for the y axis.</param>
		/// <param name="z">Scale factor for the z axis.</param>
		/// <param name="result">A scale matrix.</param>
		public static void CreateScale(double x, double y, double z, out Matrix3D result) {
			result = Identity;
			result.Row0.X = x;
			result.Row1.Y = y;
			result.Row2.Z = z;
		}

		#endregion

		#region Add Functions

		/// <summary>
		/// Adds two instances.
		/// </summary>
		/// <param name="left">The left operand of the addition.</param>
		/// <param name="right">The right operand of the addition.</param>
		/// <returns>A new instance that is the result of the addition.</returns>
		public static Matrix3D Add(Matrix3D left, Matrix3D right) {
			Matrix3D result;
			Add(ref left, ref right, out result);
			return result;
		}

		/// <summary>
		/// Adds two instances.
		/// </summary>
		/// <param name="left">The left operand of the addition.</param>
		/// <param name="right">The right operand of the addition.</param>
		/// <param name="result">A new instance that is the result of the addition.</param>
		public static void Add(ref Matrix3D left, ref Matrix3D right, out Matrix3D result) {
			Vector3D.Add(ref left.Row0, ref right.Row0, out result.Row0);
			Vector3D.Add(ref left.Row1, ref right.Row1, out result.Row1);
			Vector3D.Add(ref left.Row2, ref right.Row2, out result.Row2);
		}

		#endregion

		#region Multiply Functions

		/// <summary>
		/// Multiplies two instances.
		/// </summary>
		/// <param name="left">The left operand of the multiplication.</param>
		/// <param name="right">The right operand of the multiplication.</param>
		/// <returns>A new instance that is the result of the multiplication</returns>
		public static Matrix3D Mult(Matrix3D left, Matrix3D right) {
			Matrix3D result;
			Mult(ref left, ref right, out result);
			return result;
		}

		/// <summary>
		/// Multiplies two instances.
		/// </summary>
		/// <param name="left">The left operand of the multiplication.</param>
		/// <param name="right">The right operand of the multiplication.</param>
		/// <param name="result">A new instance that is the result of the multiplication</param>
		public static void Mult(ref Matrix3D left, ref Matrix3D right, out Matrix3D result) {
			double lM11 = left.Row0.X, lM12 = left.Row0.Y, lM13 = left.Row0.Z,
			lM21 = left.Row1.X, lM22 = left.Row1.Y, lM23 = left.Row1.Z,
			lM31 = left.Row2.X, lM32 = left.Row2.Y, lM33 = left.Row2.Z,
			rM11 = right.Row0.X, rM12 = right.Row0.Y, rM13 = right.Row0.Z,
			rM21 = right.Row1.X, rM22 = right.Row1.Y, rM23 = right.Row1.Z,
			rM31 = right.Row2.X, rM32 = right.Row2.Y, rM33 = right.Row2.Z;

			result.Row0.X = ((lM11 * rM11) + (lM12 * rM21)) + (lM13 * rM31);
			result.Row0.Y = ((lM11 * rM12) + (lM12 * rM22)) + (lM13 * rM32);
			result.Row0.Z = ((lM11 * rM13) + (lM12 * rM23)) + (lM13 * rM33);
			result.Row1.X = ((lM21 * rM11) + (lM22 * rM21)) + (lM23 * rM31);
			result.Row1.Y = ((lM21 * rM12) + (lM22 * rM22)) + (lM23 * rM32);
			result.Row1.Z = ((lM21 * rM13) + (lM22 * rM23)) + (lM23 * rM33);
			result.Row2.X = ((lM31 * rM11) + (lM32 * rM21)) + (lM33 * rM31);
			result.Row2.Y = ((lM31 * rM12) + (lM32 * rM22)) + (lM33 * rM32);
			result.Row2.Z = ((lM31 * rM13) + (lM32 * rM23)) + (lM33 * rM33);
		}

		#endregion

		#region Invert Functions

		/// <summary>
		/// Calculate the inverse of the given matrix
		/// </summary>
		/// <param name="mat">The matrix to invert</param>
		/// <param name="result">The inverse of the given matrix if it has one, or the input if it is singular</param>
		/// <exception cref="InvalidOperationException">Thrown if the Matrix3D is singular.</exception>
		public static void Invert(ref Matrix3D mat, out Matrix3D result) {
			int[] colIdx = { 0, 0, 0 };
			int[] rowIdx = { 0, 0, 0 };
			int[] pivotIdx = { -1, -1, -1 };

			double[][] inverse = new double[3][];
			inverse[0] = new double[] { mat.Row0.X, mat.Row0.Y, mat.Row0.Z };
			inverse[1] = new double[] { mat.Row1.X, mat.Row1.Y, mat.Row1.Z };
			inverse[2] = new double[] { mat.Row2.X, mat.Row2.Y, mat.Row2.Z };

			int icol = 0;
			int irow = 0;
			for (int i = 0; i < 3; i++) {
				double maxPivot = 0.0;
				for (int j = 0; j < 3; j++) {
					if (pivotIdx[j] != 0) {
						for (int k = 0; k < 3; ++k) {
							if (pivotIdx[k] == -1) {
								double absVal = System.Math.Abs(inverse[j][k]);
								if (absVal > maxPivot) {
									maxPivot = absVal;
									irow = j;
									icol = k;
								}
							} else if (pivotIdx[k] > 0) {
								result = mat;
								return;
							}
						}
					}
				}

				++(pivotIdx[icol]);

				if (irow != icol) {
					for (int k = 0; k < 3; ++k) {
						double f = inverse[irow][k];
						inverse[irow][k] = inverse[icol][k];
						inverse[icol][k] = f;
					}
				}

				rowIdx[i] = irow;
				colIdx[i] = icol;

				double pivot = inverse[icol][icol];

				if (pivot == 0.0) {
					throw new InvalidOperationException("Matrix is singular and cannot be inverted.");
				}

				double oneOverPivot = 1.0 / pivot;
				inverse[icol][icol] = 1.0;
				for (int k = 0; k < 3; ++k)
					inverse[icol][k] *= oneOverPivot;

				for (int j = 0; j < 3; ++j) {
					if (icol != j) {
						double f = inverse[j][icol];
						inverse[j][icol] = 0.0;
						for (int k = 0; k < 3; ++k)
							inverse[j][k] -= inverse[icol][k] * f;
					}
				}
			}

			for (int j = 2; j >= 0; --j) {
				int ir = rowIdx[j];
				int ic = colIdx[j];
				for (int k = 0; k < 3; ++k) {
					double f = inverse[k][ir];
					inverse[k][ir] = inverse[k][ic];
					inverse[k][ic] = f;
				}
			}

			result.Row0.X = inverse[0][0];
			result.Row0.Y = inverse[0][1];
			result.Row0.Z = inverse[0][2];
			result.Row1.X = inverse[1][0];
			result.Row1.Y = inverse[1][1];
			result.Row1.Z = inverse[1][2];
			result.Row2.X = inverse[2][0];
			result.Row2.Y = inverse[2][1];
			result.Row2.Z = inverse[2][2];
		}

		/// <summary>
		/// Calculate the inverse of the given matrix
		/// </summary>
		/// <param name="mat">The matrix to invert</param>
		/// <returns>The inverse of the given matrix if it has one, or the input if it is singular</returns>
		/// <exception cref="InvalidOperationException">Thrown if the Matrix4 is singular.</exception>
		public static Matrix3D Invert(Matrix3D mat) {
			Matrix3D result;
			Invert(ref mat, out result);
			return result;
		}

		#endregion

		#region Transpose

		/// <summary>
		/// Calculate the transpose of the given matrix
		/// </summary>
		/// <param name="mat">The matrix to transpose</param>
		/// <returns>The transpose of the given matrix</returns>
		public static Matrix3D Transpose(Matrix3D mat) {
			return new Matrix3D(mat.Column0, mat.Column1, mat.Column2);
		}

		/// <summary>
		/// Calculate the transpose of the given matrix
		/// </summary>
		/// <param name="mat">The matrix to transpose</param>
		/// <param name="result">The result of the calculation</param>
		public static void Transpose(ref Matrix3D mat, out Matrix3D result) {
			result.Row0 = mat.Column0;
			result.Row1 = mat.Column1;
			result.Row2 = mat.Column2;
		}

		#endregion

		#endregion

		#region Operators

		/// <summary>
		/// Matrix multiplication
		/// </summary>
		/// <param name="left">left-hand operand</param>
		/// <param name="right">right-hand operand</param>
		/// <returns>A new Matrix3D which holds the result of the multiplication</returns>
		public static Matrix3D operator *(Matrix3D left, Matrix3D right) {
			return Matrix3D.Mult(left, right);
		}

		/// <summary>
		/// Compares two instances for equality.
		/// </summary>
		/// <param name="left">The first instance.</param>
		/// <param name="right">The second instance.</param>
		/// <returns>True, if left equals right; false otherwise.</returns>
		public static bool operator ==(Matrix3D left, Matrix3D right) {
			return left.Equals(right);
		}

		/// <summary>
		/// Compares two instances for inequality.
		/// </summary>
		/// <param name="left">The first instance.</param>
		/// <param name="right">The second instance.</param>
		/// <returns>True, if left does not equal right; false otherwise.</returns>
		public static bool operator !=(Matrix3D left, Matrix3D right) {
			return !left.Equals(right);
		}

		#endregion

		#region Overrides

		#region public override string ToString()

		/// <summary>
		/// Returns a System.String that represents the current Matrix3D.
		/// </summary>
		/// <returns>The string representation of the matrix.</returns>
		public override string ToString() {
			return String.Format("{0}\n{1}\n{2}", Row0, Row1, Row2);
		}

		#endregion

		#region public override int GetHashCode()

		/// <summary>
		/// Returns the hashcode for this instance.
		/// </summary>
		/// <returns>A System.Int32 containing the unique hashcode for this instance.</returns>
		public override int GetHashCode() {
			return Row0.GetHashCode() ^ Row1.GetHashCode() ^ Row2.GetHashCode();
		}

		#endregion

		#region public override bool Equals(object obj)

		/// <summary>
		/// Indicates whether this instance and a specified object are equal.
		/// </summary>
		/// <param name="obj">The object to compare to.</param>
		/// <returns>True if the instances are equal; false otherwise.</returns>
		public override bool Equals(object obj) {
			if (!(obj is Matrix3D))
				return false;

			return this.Equals((Matrix3D) obj);
		}

		#endregion

		#endregion

		#endregion

		#region IEquatable<Matrix3D> Members

		/// <summary>Indicates whether the current matrix is equal to another matrix.</summary>
		/// <param name="other">A matrix to compare with this matrix.</param>
		/// <returns>true if the current matrix is equal to the matrix parameter; otherwise, false.</returns>
		public bool Equals(Matrix3D other) {
			return
				Row0 == other.Row0 &&
					Row1 == other.Row1 &&
					Row2 == other.Row2;
		}

		#endregion
	}
}