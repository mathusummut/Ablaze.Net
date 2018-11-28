using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Numerics {
	/// <summary>
	/// Represents a hardware-accelerated 4x4 Matrix.
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct Matrix4 : IEquatable<Matrix4> {
		/// <summary>
		/// Top row of the matrix
		/// </summary>
		public Vector4 Row0;
		/// <summary>
		/// 2nd row of the matrix
		/// </summary>
		public Vector4 Row1;
		/// <summary>
		/// 3rd row of the matrix
		/// </summary>
		public Vector4 Row2;
		/// <summary>
		/// Bottom row of the matrix
		/// </summary>
		public Vector4 Row3;
		/// <summary>
		/// The identity matrix
		/// </summary>
		public static readonly Matrix4 Identity = new Matrix4(Vector4.UnitX, Vector4.UnitY, Vector4.UnitZ, Vector4.UnitW);

		/// <summary>
		/// The determinant of this matrix
		/// </summary>
		public float Determinant {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return
					Row0.X * Row1.Y * Row2.Z * Row3.W - Row0.X * Row1.Y * Row2.W * Row3.Z + Row0.X * Row1.Z * Row2.W * Row3.Y - Row0.X * Row1.Z * Row2.Y * Row3.W
				  + Row0.X * Row1.W * Row2.Y * Row3.Z - Row0.X * Row1.W * Row2.Z * Row3.Y - Row0.Y * Row1.Z * Row2.W * Row3.X + Row0.Y * Row1.Z * Row2.X * Row3.W
				  - Row0.Y * Row1.W * Row2.X * Row3.Z + Row0.Y * Row1.W * Row2.Z * Row3.X - Row0.Y * Row1.X * Row2.Z * Row3.W + Row0.Y * Row1.X * Row2.W * Row3.Z
				  + Row0.Z * Row1.W * Row2.X * Row3.Y - Row0.Z * Row1.W * Row2.Y * Row3.X + Row0.Z * Row1.X * Row2.Y * Row3.W - Row0.Z * Row1.X * Row2.W * Row3.Y
				  + Row0.Z * Row1.Y * Row2.W * Row3.X - Row0.Z * Row1.Y * Row2.X * Row3.W - Row0.W * Row1.X * Row2.Y * Row3.Z + Row0.W * Row1.X * Row2.Z * Row3.Y
				  - Row0.W * Row1.Y * Row2.Z * Row3.X + Row0.W * Row1.Y * Row2.X * Row3.Z - Row0.W * Row1.Z * Row2.X * Row3.Y + Row0.W * Row1.Z * Row2.Y * Row3.X;
			}
		}

		/// <summary>
		/// The first column of this matrix
		/// </summary>
		public Vector4 Column0 {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return new Vector4(Row0.X, Row1.X, Row2.X, Row3.X);
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				Row0.X = value.X;
				Row1.X = value.Y;
				Row2.X = value.Z;
				Row3.X = value.W;
			}
		}

		/// <summary>
		/// The second column of this matrix
		/// </summary>
		public Vector4 Column1 {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return new Vector4(Row0.Y, Row1.Y, Row2.Y, Row3.Y);
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				Row0.Y = value.X;
				Row1.Y = value.Y;
				Row2.Y = value.Z;
				Row3.Y = value.W;
			}
		}

		/// <summary>
		/// The third column of this matrix
		/// </summary>
		public Vector4 Column2 {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return new Vector4(Row0.Z, Row1.Z, Row2.Z, Row3.Z);
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				Row0.Z = value.X;
				Row1.Z = value.Y;
				Row2.Z = value.Z;
				Row3.Z = value.W;
			}
		}

		/// <summary>
		/// The fourth column of this matrix
		/// </summary>
		public Vector4 Column3 {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return new Vector4(Row0.W, Row1.W, Row2.W, Row3.W);
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				Row0.W = value.X;
				Row1.W = value.Y;
				Row2.W = value.Z;
				Row3.W = value.W;
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
		/// Gets or sets the value at row 1, column 4 of this instance.
		/// </summary>
		public float M14 {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return Row0.W;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				Row0.W = value;
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
		/// Gets or sets the value at row 2, column 4 of this instance.
		/// </summary>
		public float M24 {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return Row1.W;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				Row1.W = value;
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
		/// Gets or sets the value at row 3, column 4 of this instance.
		/// </summary>
		public float M34 {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return Row2.W;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				Row2.W = value;
			}
		}

		/// <summary>
		/// Gets or sets the value at row 4, column 1 of this instance.
		/// </summary>
		public float M41 {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return Row3.X;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				Row3.X = value;
			}
		}

		/// <summary>
		/// Gets or sets the value at row 4, column 2 of this instance.
		/// </summary>
		public float M42 {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return Row3.Y;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				Row3.Y = value;
			}
		}

		/// <summary>
		/// Gets or sets the value at row 4, column 3 of this instance.
		/// </summary>
		public float M43 {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return Row3.Z;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				Row3.Z = value;
			}
		}

		/// <summary>
		/// Gets or sets the value at row 4, column 4 of this instance.
		/// </summary>
		public float M44 {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return Row3.W;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				Row3.W = value;
			}
		}

		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		/// <param name="row0">Top row of the matrix</param>
		/// <param name="row1">Second row of the matrix</param>
		/// <param name="row2">Third row of the matrix</param>
		/// <param name="row3">Bottom row of the matrix</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public Matrix4(Vector4 row0, Vector4 row1, Vector4 row2, Vector4 row3) {
			Row0 = row0;
			Row1 = row1;
			Row2 = row2;
			Row3 = row3;
		}

		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		/// <param name="row0">Top row of the matrix</param>
		/// <param name="row1">Second row of the matrix</param>
		/// <param name="row2">Third row of the matrix</param>
		/// <param name="row3">Bottom row of the matrix</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public Matrix4(ref Vector4 row0, ref Vector4 row1, ref Vector4 row2, ref Vector4 row3) {
			Row0 = row0;
			Row1 = row1;
			Row2 = row2;
			Row3 = row3;
		}

		/// <summary>
		/// Converts a 3x3 matrix to a 4x4 matrix.
		/// </summary>
		/// <param name="matrix">The matrix to convert.</param>
		public Matrix4(ref Matrix3 matrix) : this(new Vector4(matrix.Row0, 0f), new Vector4(matrix.Row1, 0f), new Vector4(matrix.Row2, 0f), Vector4.UnitW) {
		}

		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		/// <param name="m00">First item of the first row of the matrix.</param>
		/// <param name="m01">Second item of the first row of the matrix.</param>
		/// <param name="m02">Third item of the first row of the matrix.</param>
		/// <param name="m03">Fourth item of the first row of the matrix.</param>
		/// <param name="m10">First item of the second row of the matrix.</param>
		/// <param name="m11">Second item of the second row of the matrix.</param>
		/// <param name="m12">Third item of the second row of the matrix.</param>
		/// <param name="m13">Fourth item of the second row of the matrix.</param>
		/// <param name="m20">First item of the third row of the matrix.</param>
		/// <param name="m21">Second item of the third row of the matrix.</param>
		/// <param name="m22">Third item of the third row of the matrix.</param>
		/// <param name="m23">First item of the third row of the matrix.</param>
		/// <param name="m30">Fourth item of the fourth row of the matrix.</param>
		/// <param name="m31">Second item of the fourth row of the matrix.</param>
		/// <param name="m32">Third item of the fourth row of the matrix.</param>
		/// <param name="m33">Fourth item of the fourth row of the matrix.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public Matrix4(float m00, float m01, float m02, float m03,
					float m10, float m11, float m12, float m13,
					float m20, float m21, float m22, float m23,
					float m30, float m31, float m32, float m33) {
			Row0 = new Vector4(m00, m01, m02, m03);
			Row1 = new Vector4(m10, m11, m12, m13);
			Row2 = new Vector4(m20, m21, m22, m23);
			Row3 = new Vector4(m30, m31, m32, m33);
		}

		/// <summary>
		/// Gets the Matrix3 subset of this matrix.
		/// </summary>
		public Matrix3 ToMatrix3() {
			Matrix3 matrix;
			ToMatrix3(out matrix);
			return matrix;
		}

		/// <summary>
		/// Gets the Matrix3 subset of this matrix.
		/// </summary>
		/// <param name="matrix">The resultant matrix.</param>
		public void ToMatrix3(out Matrix3 matrix) {
			matrix.Row0 = Row0.ToVector3();
			matrix.Row1 = Row1.ToVector3();
			matrix.Row2 = Row2.ToVector3();
		}

		/// <summary>
		/// Divides each element in the Matrix by the <see cref="Determinant"/>.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public Matrix4 Normalized() {
			float determinant = 1f / Determinant;
			return new Matrix4(Row0 * determinant, Row1 * determinant, Row2 * determinant, Row3 * determinant);
		}

		/// <summary>
		/// Outs an inverted copy of this instance, returns whether the inversion was successful.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool Invert(out Matrix4 matrix) {
			return Invert(ref this, out matrix);
		}

		/// <summary>
		/// Returns a copy of this matrix without scale.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public Matrix4 ClearScale() {
			return new Matrix4(Vector4.Normalize(Row0), Vector4.Normalize(Row1), Vector4.Normalize(Row2), Vector4.Normalize(Row3));
		}

		/// <summary>
		/// Returns a copy of this matrix without rotation.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public Matrix4 ClearRotation() {
			return new Matrix4(new Vector4(Row0.Length(), 0f, 0f, 0f), new Vector4(0f, Row1.Length(), 0f, 0f), new Vector4(0f, 0f, Row2.Length(), 0f), new Vector4(0f, 0f, 0f, Row2.Length()));
		}

		/// <summary>
		/// Returns the scale component of this instance.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public Vector4 ExtractScale() {
			return new Vector4(Row0.Length(), Row1.Length(), Row2.Length(), Row3.Length());
		}

		/// <summary>
		/// Build a rotation matrix from the specified axis/angle rotation.
		/// </summary>
		/// <param name="axis">The axis to rotate about.</param>
		/// <param name="angle">Angle in radians to rotate counter-clockwise (looking in the direction of the given axis).</param>
		/// <param name="result">A matrix instance.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void CreateFromAxisAngle(ref Vector3 axis, float angle, out Matrix4 result) {
			Matrix3 temp;
			Matrix3.CreateFromAxisAngle(ref axis, angle, out temp);
			result = new Matrix4(ref temp);
		}

		/// <summary>
		/// Builds a rotation matrix for a rotation around the x-axis.
		/// </summary>
		/// <param name="angle">The counter-clockwise angle in radians.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Matrix4 CreateRotationX(float angle) {
			float cos = (float) Math.Cos(angle);
			float sin = (float) Math.Sin(angle);
			return new Matrix4(Vector4.UnitX, new Vector4(0f, cos, sin, 0f), new Vector4(0f, -sin, cos, 0f), Vector4.UnitW);
		}

		/// <summary>
		/// Builds a rotation matrix for a rotation around the y-axis.
		/// </summary>
		/// <param name="angle">The counter-clockwise angle in radians.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Matrix4 CreateRotationY(float angle) {
			float cos = (float) Math.Cos(angle);
			float sin = (float) Math.Sin(angle);
			return new Matrix4(new Vector4(cos, 0f, -sin, 0f), Vector4.UnitY, new Vector4(sin, 0f, cos, 0f), Vector4.UnitW);
		}

		/// <summary>
		/// Builds a rotation matrix for a rotation around the z-axis.
		/// </summary>
		/// <param name="angle">The counter-clockwise angle in radians.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Matrix4 CreateRotationZ(float angle) {
			float cos = (float) Math.Cos(angle);
			float sin = (float) Math.Sin(angle);
			return new Matrix4(new Vector4(cos, sin, 0f, 0f), new Vector4(-sin, cos, 0f, 0f), Vector4.UnitZ, Vector4.UnitW);
		}

		/// <summary>
		/// Creates a translation matrix.
		/// </summary>
		/// <param name="x">X translation.</param>
		/// <param name="y">Y translation.</param>
		/// <param name="z">Z translation.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Matrix4 CreateTranslation(float x, float y, float z) {
			Matrix4 result = Identity;
			result.Row3 = new Vector4(x, y, z, 1f);
			return result;
		}

		/// <summary>
		/// Creates a translation matrix.
		/// </summary>
		/// <param name="vector">The translation vector.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Matrix4 CreateTranslation(ref Vector3 vector) {
			Matrix4 result = Identity;
			result.Row3 = new Vector4(vector.X, vector.Y, vector.Z, 1f);
			return result;
		}

		/// <summary>
		/// Creates an orthographic projection matrix.
		/// </summary>
		/// <param name="width">The width of the projection volume.</param>
		/// <param name="height">The height of the projection volume.</param>
		/// <param name="zNear">The near edge of the projection volume.</param>
		/// <param name="zFar">The far edge of the projection volume.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Matrix4 CreateOrthographic(float width, float height, float zNear, float zFar) {
			return CreateOrthographicOffCenter(-width * 0.5f, width * 0.5f, -height * 0.5f, height * 0.5f, zNear, zFar);
		}

		/// <summary>
		/// Creates an orthographic projection matrix.
		/// </summary>
		/// <param name="left">The left edge of the projection volume.</param>
		/// <param name="right">The right edge of the projection volume.</param>
		/// <param name="bottom">The bottom edge of the projection volume.</param>
		/// <param name="top">The top edge of the projection volume.</param>
		/// <param name="zNear">The near edge of the projection volume.</param>
		/// <param name="zFar">The far edge of the projection volume.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Matrix4 CreateOrthographicOffCenter(float left, float right, float bottom, float top, float zNear, float zFar) {
			float invRL = 1f / (right - left);
			float invTB = 1f / (top - bottom);
			float invFN = 1f / (zFar - zNear);

			Matrix4 result = new Matrix4() {
				M11 = 2f * invRL,
				M22 = 2f * invTB,
				M33 = -2f * invFN,

				M41 = -(right + left) * invRL,
				M42 = -(top + bottom) * invTB,
				M43 = -(zFar + zNear) * invFN,
				M44 = 1f
			};
			return result;
		}

		/// <summary>
		/// Creates a perspective projection matrix.
		/// </summary>
		/// <param name="fovy">Angle of the field of view in the y direction in radians, greater than 0 but smaller than pi (pi/4 recommended).</param>
		/// <param name="aspect">Aspect ratio of the view (width / height).</param>
		/// <param name="zNear">Distance to the near clip plane (greater than 0).</param>
		/// <param name="zFar">Distance to the far clip plane (greater than 0).</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Matrix4 CreatePerspectiveFieldOfView(float fovy, float aspect, float zNear, float zFar) {
			float yMax = zNear * (float) Math.Tan(0.5f * fovy);
			float yMin = -yMax;
			return CreatePerspectiveOffCenter(yMin * aspect, yMax * aspect, yMin, yMax, zNear, zFar);
		}

		/// <summary>
		/// Creates a perspective projection matrix.
		/// </summary>
		/// <param name="left">Left edge of the view frustum</param>
		/// <param name="right">Right edge of the view frustum</param>
		/// <param name="bottom">Bottom edge of the view frustum</param>
		/// <param name="top">Top edge of the view frustum</param>
		/// <param name="zNear">Distance to the near clip plane</param>
		/// <param name="zFar">Distance to the far clip plane (must be greater than zNear)</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Matrix4 CreatePerspectiveOffCenter(float left, float right, float bottom, float top, float zNear, float zFar) {
			float width = right - left;
			float height = top - bottom;
			float zDist = zFar - zNear;
			return new Matrix4((2f * zNear) / width, 0f, 0f, 0f, 0f, (2f * zNear) / height, 0f, 0f, (right + left) / width, (top + bottom) / height, -(zFar + zNear) / zDist, -1f, 0f, 0f, -(2f * zFar * zNear) / zDist, 0f);
		}

		/// <summary>
		/// Build a scaling matrix
		/// </summary>
		/// <param name="scale">Single scale factor for x,y and z axes</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Matrix4 CreateScale(float scale) {
			Matrix4 result = new Matrix4();
			result.Row0.X = scale;
			result.Row1.Y = scale;
			result.Row2.Z = scale;
			result.Row3.W = 1f;
			return result;
		}

		/// <summary>
		/// Build a scaling matrix
		/// </summary>
		/// <param name="scale">Scale factors for x,y and z axes</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Matrix4 CreateScale(ref Vector3 scale) {
			Matrix4 result = new Matrix4();
			result.Row0.X = scale.X;
			result.Row1.Y = scale.Y;
			result.Row2.Z = scale.Z;
			result.Row3.W = 1f;
			return result;
		}

		/// <summary>
		/// Build a scaling matrix
		/// </summary>
		/// <param name="x">Scale factor for x-axis</param>
		/// <param name="y">Scale factor for y-axis</param>
		/// <param name="z">Scale factor for z-axis</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Matrix4 CreateScale(float x, float y, float z) {
			Matrix4 result = new Matrix4();
			result.Row0.X = x;
			result.Row1.Y = y;
			result.Row2.Z = z;
			result.Row3.W = 1f;
			return result;
		}

		/// <summary>
		/// Build a rotation matrix from the specified quaternion.
		/// </summary>
		/// <param name="q">Quaternion to translate.</param>
		/// <param name="result">Matrix result.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void CreateFromQuaternion(ref Quaternion q, out Matrix4 result) {
			Quaternion quat = q.ToAxisAngle();
			Vector3 axis = new Vector3(quat.X, quat.Y, quat.Z);
			CreateFromAxisAngle(ref axis, quat.W, out result);
		}

		/// <summary>
		/// USE THE ref OVERLOAD OF THIS FUNCTION! Builds a world space to camera space matrix.
		/// </summary>
		/// <param name="eye">Eye (camera) position in world space</param>
		/// <param name="target">Target position in world space</param>
		/// <param name="up">Up vector in world space (should not be parallel to the camera direction, that is target - eye)</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Matrix4 LookAt(Vector3 eye, Vector3 target, Vector3 up) {
			return LookAt(ref eye, ref target, ref up);
		}

		/// <summary>
		/// Builds a world space to camera space matrix
		/// </summary>
		/// <param name="eye">Eye (camera) position in world space</param>
		/// <param name="target">Target position in world space</param>
		/// <param name="up">Up vector in world space (should not be parallel to the camera direction, that is target - eye)</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public static Matrix4 LookAt(ref Vector3 eye, ref Vector3 target, ref Vector3 up) {
			Vector3 z = Vector3.Normalize(eye - target);
			Vector3 x = Vector3.Normalize(Vector3.Cross(up, z));
			Vector3 y = Vector3.Normalize(Vector3.Cross(z, x));
			Vector3 negEye = -eye;
			//trans * rot
			return CreateTranslation(ref negEye) * new Matrix4(x.X, y.X, z.X, 0f, x.Y, y.Y, z.Y, 0f, x.Z, y.Z, z.Z, 0f, 0f, 0f, 0f, 1f);
		}

		/// <summary>
		/// Subtracts the second matrix from the first.
		/// </summary>
		/// <param name="left">The left operand of the subtraction.</param>
		/// <param name="right">The right operand of the subtraction.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Matrix4 Subtract(ref Matrix4 left, ref Matrix4 right) {
			return new Matrix4(left.Row0 - right.Row0, left.Row1 - right.Row1, left.Row2 - right.Row2, left.Row3 - right.Row3);
		}

		/// <summary>
		/// The matrix to negate (not invert, ie. 0 - matrix)
		/// </summary>
		/// <param name="matrix">The matrix to negate.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Matrix4 Negate(ref Matrix4 matrix) {
			return new Matrix4(-matrix.Row0, -matrix.Row1, -matrix.Row2, -matrix.Row3);
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
		public static Matrix4 Mult(ref Matrix4 left, float right) {
			return new Matrix4(left.Row0 * right, left.Row1 * right, left.Row2 * right, left.Row3);
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
		public static Vector4 Mult(ref Matrix4 matrix, ref Vector4 vertex) {
			return vertex * matrix.Row0 + vertex * matrix.Row1 + vertex * matrix.Row2 + vertex * matrix.Row3;
		}

		/// <summary>
		/// Multiplies two instances.
		/// </summary>
		/// <param name="left">The left operand of the multiplication.</param>
		/// <param name="right">The right operand of the multiplication.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Matrix4 Mult(ref Matrix4 left, ref Matrix4 right) {
			float lM11 = left.Row0.X, lM12 = left.Row0.Y, lM13 = left.Row0.Z, lM14 = left.Row0.W,
				lM21 = left.Row1.X, lM22 = left.Row1.Y, lM23 = left.Row1.Z, lM24 = left.Row1.W,
				lM31 = left.Row2.X, lM32 = left.Row2.Y, lM33 = left.Row2.Z, lM34 = left.Row2.W,
				lM41 = left.Row3.X, lM42 = left.Row3.Y, lM43 = left.Row3.Z, lM44 = left.Row3.W,
				rM11 = right.Row0.X, rM12 = right.Row0.Y, rM13 = right.Row0.Z, rM14 = right.Row0.W,
				rM21 = right.Row1.X, rM22 = right.Row1.Y, rM23 = right.Row1.Z, rM24 = right.Row1.W,
				rM31 = right.Row2.X, rM32 = right.Row2.Y, rM33 = right.Row2.Z, rM34 = right.Row2.W,
				rM41 = right.Row3.X, rM42 = right.Row3.Y, rM43 = right.Row3.Z, rM44 = right.Row3.W;

			return new Matrix4((((lM11 * rM11) + (lM12 * rM21)) + (lM13 * rM31)) + (lM14 * rM41), (((lM11 * rM12) + (lM12 * rM22)) + (lM13 * rM32)) + (lM14 * rM42), (((lM11 * rM13) + (lM12 * rM23)) + (lM13 * rM33)) + (lM14 * rM43), (((lM11 * rM14) + (lM12 * rM24)) + (lM13 * rM34)) + (lM14 * rM44),
				(((lM21 * rM11) + (lM22 * rM21)) + (lM23 * rM31)) + (lM24 * rM41), (((lM21 * rM12) + (lM22 * rM22)) + (lM23 * rM32)) + (lM24 * rM42), (((lM21 * rM13) + (lM22 * rM23)) + (lM23 * rM33)) + (lM24 * rM43), (((lM21 * rM14) + (lM22 * rM24)) + (lM23 * rM34)) + (lM24 * rM44),
				(((lM31 * rM11) + (lM32 * rM21)) + (lM33 * rM31)) + (lM34 * rM41), (((lM31 * rM12) + (lM32 * rM22)) + (lM33 * rM32)) + (lM34 * rM42), (((lM31 * rM13) + (lM32 * rM23)) + (lM33 * rM33)) + (lM34 * rM43), (((lM31 * rM14) + (lM32 * rM24)) + (lM33 * rM34)) + (lM34 * rM44),
				(((lM41 * rM11) + (lM42 * rM21)) + (lM43 * rM31)) + (lM44 * rM41), (((lM41 * rM12) + (lM42 * rM22)) + (lM43 * rM32)) + (lM44 * rM42), (((lM41 * rM13) + (lM42 * rM23)) + (lM43 * rM33)) + (lM44 * rM43), (((lM41 * rM14) + (lM42 * rM24)) + (lM43 * rM34)) + (lM44 * rM44));
		}

		/// <summary>
		/// Calculate the inverse of the given matrix, and returns whether the inversion was successful.
		/// </summary>
		/// <param name="mat">The matrix to invert</param>
		/// <param name="result">The resultant inverse of the given matrix if it has one, or the input if it is singular</param>
		public static bool Invert(ref Matrix4 mat, out Matrix4 result) {
			int[] colIdx = { 0, 0, 0, 0 };
			int[] rowIdx = { 0, 0, 0, 0 };
			int[] pivotIdx = { -1, -1, -1, -1 };

			// convert the matrix to an array for easy looping
			float[][] inverse = new float[4][];
			inverse[0] = new float[] { mat.Row0.X, mat.Row0.Y, mat.Row0.Z, mat.Row0.W };
			inverse[1] = new float[] { mat.Row1.X, mat.Row1.Y, mat.Row1.Z, mat.Row1.W };
			inverse[2] = new float[] { mat.Row2.X, mat.Row2.Y, mat.Row2.Z, mat.Row2.W };
			inverse[3] = new float[] { mat.Row3.X, mat.Row3.Y, mat.Row3.Z, mat.Row3.W };
			int icol = 0;
			int irow = 0;
			for (int i = 0; i < 4; i++) {
				// Find the largest pivot value
				float maxPivot = 0f;
				for (int j = 0; j < 4; j++) {
					if (pivotIdx[j] != 0) {
						for (int k = 0; k < 4; ++k) {
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

				// Swap rows over so pivot is on diagonal
				if (irow != icol) {
					for (int k = 0; k < 4; ++k) {
						float f = inverse[irow][k];
						inverse[irow][k] = inverse[icol][k];
						inverse[icol][k] = f;
					}
				}

				rowIdx[i] = irow;
				colIdx[i] = icol;

				float pivot = inverse[icol][icol];
				// check for singular matrix
				if (pivot == 0f) {
					result = mat;
					return false;
				}

				// Scale row so it has a unit diagonal
				float oneOverPivot = 1f / pivot;
				inverse[icol][icol] = 1f;
				for (int k = 0; k < 4; ++k)
					inverse[icol][k] *= oneOverPivot;

				// Do elimination of non-diagonal elements
				for (int j = 0; j < 4; ++j) {
					// check this isn't on the diagonal
					if (icol != j) {
						float f = inverse[j][icol];
						inverse[j][icol] = 0f;
						for (int k = 0; k < 4; ++k)
							inverse[j][k] -= inverse[icol][k] * f;
					}
				}
			}

			for (int j = 3; j >= 0; --j) {
				int ir = rowIdx[j];
				int ic = colIdx[j];
				for (int k = 0; k < 4; ++k) {
					float f = inverse[k][ir];
					inverse[k][ir] = inverse[k][ic];
					inverse[k][ic] = f;
				}
			}

			result = new Matrix4(inverse[0][0], inverse[0][1], inverse[0][2], inverse[0][3],
				inverse[1][0], inverse[1][1], inverse[1][2], inverse[1][3],
				inverse[2][0], inverse[2][1], inverse[2][2], inverse[2][3],
				inverse[3][0], inverse[3][1], inverse[3][2], inverse[3][3]);
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
		public static Matrix4 Transpose(ref Matrix4 mat) {
			return new Matrix4(mat.Column0, mat.Column1, mat.Column2, mat.Column3);
		}

		/// <summary>
		/// Calculate the transpose of the given matrix
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public Matrix4 Transpose() {
			return new Matrix4(Column0, Column1, Column2, Column3);
		}

		/// <summary>
		/// Adds two instances.
		/// </summary>
		/// <param name="left">The left operand of the addition.</param>
		/// <param name="right">The right operand of the addition.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Matrix4 Add(ref Matrix4 left, ref Matrix4 right) {
			return new Matrix4(left.Row0 + right.Row0, left.Row1 + right.Row1, left.Row2 + right.Row2, left.Row3 + right.Row3);
		}

		/// <summary>
		/// Matrix multiplication
		/// </summary>
		/// <param name="left">left-hand operand</param>
		/// <param name="right">right-hand operand</param>
		/// <returns>A new Matrix4 which holds the result of the multiplication</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Matrix4 operator *(Matrix4 left, Matrix4 right) {
			return Mult(ref left, ref right);
		}

		/// <summary>
		/// Matrix addition
		/// </summary>
		/// <param name="left">left-hand operand</param>
		/// <param name="right">right-hand operand</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Matrix4 operator +(Matrix4 left, Matrix4 right) {
			return Add(ref left, ref right);
		}

		/// <summary>
		/// Does nothing
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Matrix4 operator +(Matrix4 matrix) {
			return matrix;
		}

		/// <summary>
		/// Negates the matrix
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Matrix4 operator -(Matrix4 matrix) {
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
		public static Matrix4 operator -(Matrix4 left, Matrix4 right) {
			return Subtract(ref left, ref right);
		}

		/// <summary>
		/// Multiplies the matrix with the vector
		/// </summary>
		/// <param name="left">The matrix</param>
		/// <param name="right">The vector</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector4 operator *(Matrix4 left, Vector4 right) {
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
		public static Matrix4 operator *(Matrix4 left, float right) {
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
		public static Matrix4 operator *(float left, Matrix4 right) {
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
		public static bool operator ==(Matrix4 left, Matrix4 right) {
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
		public static bool operator !=(Matrix4 left, Matrix4 right) {
			return !left.Equals(ref right);
		}

		/// <summary>
		/// Returns a System.String that represents the current Matrix44.
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return "{" + Row0.ToString() + "\n" + Row1.ToString() + "\n" + Row2.ToString() + "\n" + Row3.ToString() + "}";
		}

		/// <summary>
		/// Returns the hashcode for this instance.
		/// </summary>
		/// <returns>A System.Int32 containing the unique hashcode for this instance.</returns>
		public override int GetHashCode() {
			return unchecked((Row0.GetHashCode() << 17) ^ (Row1.GetHashCode() << 13) & (Row2.GetHashCode() << 7) ^ Row3.GetHashCode());
		}

		/// <summary>
		/// Indicates whether this instance and a specified object are equal.
		/// </summary>
		/// <param name="obj">The object to compare tresult.</param>
		/// <returns>True if the instances are equal; false otherwise.</returns>
		public override bool Equals(object obj) {
			return obj is Matrix4 ? Equals((Matrix4) obj) : false;
		}

		/// <summary>Indicates whether the current matrix is equal to another matrix.</summary>
		/// <param name="other">An matrix to compare with this matrix.</param>
		/// <returns>true if the current matrix is equal to the matrix parameter; otherwise, false.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool Equals(Matrix4 other) {
			return Row0 == other.Row0 && Row1 == other.Row1 && Row2 == other.Row2 && Row3 == other.Row3;
		}

		/// <summary>Indicates whether the current matrix is equal to another matrix.</summary>
		/// <param name="other">An matrix to compare with this matrix.</param>
		/// <returns>true if the current matrix is equal to the matrix parameter; otherwise, false.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public bool Equals(ref Matrix4 other) {
			return Row0 == other.Row0 && Row1 == other.Row1 && Row2 == other.Row2 && Row3 == other.Row3;
		}
	}
}