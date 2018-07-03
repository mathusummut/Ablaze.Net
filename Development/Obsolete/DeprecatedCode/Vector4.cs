using System.Runtime.InteropServices;
using System.Numerics;

namespace System.Drawing {
	/// <summary>Represents a 4D vector using four single-precision floating-point numbers.</summary>
	/// <remarks>
	/// The Vector4 structure is suitable for interoperation with unmanaged code requiring four consecutive floats.
	/// </remarks>
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector4 : IEquatable<Vector4> {
		#region Fields

		/// <summary>
		/// The Xyzw component of the vector.
		/// </summary>
		public Numerics.Vector4 Xyzw;

		/// <summary>
		/// Defines a unit-length Vector4 that points towards the X-axis.
		/// </summary>
		public static readonly Vector4 UnitX = new Vector4(1F, 0F, 0F, 0F);

		/// <summary>
		/// Defines a unit-length Vector4 that points towards the Y-axis.
		/// </summary>
		public static readonly Vector4 UnitY = new Vector4(0F, 1F, 0F, 0F);

		/// <summary>
		/// Defines a unit-length Vector4 that points towards the Z-axis.
		/// </summary>
		public static readonly Vector4 UnitZ = new Vector4(0F, 0F, 1F, 0F);

		/// <summary>
		/// Defines a unit-length Vector4 that points towards the W-axis.
		/// </summary>
		public static readonly Vector4 UnitW = new Vector4(0F, 0F, 0F, 1F);

		/// <summary>
		/// Defines a zero-length Vector4.
		/// </summary>
		public static readonly Vector4 Zero = new Vector4();

		/// <summary>
		/// Defines an instance with all components set to 1.
		/// </summary>
		public static readonly Vector4 One = new Vector4(1F);

		/// <summary>
		/// Defines the size of the Vector4 struct in bytes.
		/// </summary>
		public static readonly int SizeInBytes = Marshal.SizeOf(new Vector4());

		#endregion

		#region Constructors

		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		/// <param name="value">The value that will initialize this instance.</param>
		public Vector4(float value) {
			Xyzw = new Numerics.Vector4() {
				X = value,
				Y = value,
				Z = value,
				W = value
			};
		}

		/// <summary>
		/// Constructs a new Vector4.
		/// </summary>
		/// <param name="x">The x component of the Vector4.</param>
		/// <param name="y">The y component of the Vector4.</param>
		/// <param name="z">The z component of the Vector4.</param>
		/// <param name="w">The w component of the Vector4.</param>
		public Vector4(float x, float y, float z, float w) {
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		/// <summary>
		/// Constructs a new Vector4 from the given Vector3.
		/// The w component is initialized to 0.
		/// </summary>
		/// <param name="v">The Vector3 to copy components from.</param>
		/// <remarks><seealso cref="Vector4(Vector3, float)"/></remarks>
		public Vector4(Vector3 v) {
			X = v.X;
			Y = v.Y;
			Z = v.Z;
			W = 0f;
		}

		/// <summary>
		/// Constructs a new Vector4 from the specified Vector3 and w component.
		/// </summary>
		/// <param name="v">The Vector3 to copy components from.</param>
		/// <param name="w">The w component of the new Vector4.</param>
		public Vector4(Vector3 v, float w) {
			X = v.X;
			Y = v.Y;
			Z = v.Z;
			W = w;
		}

		/// <summary>
		/// Constructs a new Vector4 from the given Vector4.
		/// </summary>
		/// <param name="v">The Vector4 to copy components from.</param>
		public Vector4(Vector4 v) {
			X = v.X;
			Y = v.Y;
			Z = v.Z;
			W = v.W;
		}

		/// <summary>
		/// Construct a new Quaternion from given Euler angles
		/// </summary>
		/// <param name="pitch">The pitch (attitude), rotation around X axis</param>
		/// <param name="yaw">The yaw (heading), rotation around Y axis</param>
		/// <param name="roll">The roll (bank), rotation around Z axis</param>
		public Vector4(float pitch, float yaw, float roll) {
			yaw *= 0.5f;
			pitch *= 0.5f;
			roll *= 0.5f;
			float c1 = (float) Math.Cos(yaw);
			float c2 = (float) Math.Cos(pitch);
			float c3 = (float) Math.Cos(roll);
			float s1 = (float) Math.Sin(yaw);
			float s2 = (float) Math.Sin(pitch);
			float s3 = (float) Math.Sin(roll);

			W = c1 * c2 * c3 - s1 * s2 * s3;
			X = s1 * s2 * c3 + c1 * c2 * s3;
			Y = s1 * c2 * c3 + c1 * s2 * s3;
			Z = c1 * s2 * c3 - s1 * c2 * s3;
		}

		#endregion

		#region Public Members

		#region Instance

		#region public Vector3 Xyz

		/// <summary>
		/// Gets or sets an System.Vector3 with the X, Y and Z components of this instance.
		/// </summary>
		public Vector3 Xyz {
			get {
				return new Vector3(X, Y, Z);
			}
			set {
				X = value.X;
				Y = value.Y;
				Z = value.Z;
			}
		}

		#endregion

		#region public float Length

		/// <summary>
		/// Gets the length (magnitude) of the vector.
		/// </summary>
		/// <see cref="LengthFast"/>
		/// <seealso cref="LengthSquared"/>
		public float Length {
			get {
				return (float) Math.Sqrt(X * X + Y * Y + Z * Z + W * W);
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
				return 1f / MathHelper.InverseSqrtFast(X * X + Y * Y + Z * Z + W * W);
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
				return X * X + Y * Y + Z * Z + W * W;
			}
		}

		#endregion

		#region public void Normalize()

		/// <summary>
		/// Scales the Vector4 to unit length.
		/// </summary>
		public void Normalize() {
			float scale = 1f / this.Length;
			X *= scale;
			Y *= scale;
			Z *= scale;
			W *= scale;
		}

		#endregion

		#region public void NormalizeFast()

		/// <summary>
		/// Scales the Vector4 to approximately unit length.
		/// </summary>
		public void NormalizeFast() {
			float scale = MathHelper.InverseSqrtFast(X * X + Y * Y + Z * Z + W * W);
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
		public void ToAxisAngle(out Vector3 axis, out float angle) {
			Vector4 result;
			ToAxisAngle(out result);
			axis = result.Xyz;
			angle = result.W;
		}

		/// <summary>
		/// Convert this instance to an axis-angle representation.
		/// </summary>
		/// <returns>A Vector4 that is the axis-angle representation of this quaternion.</returns>
		public void ToAxisAngle(out Vector4 vec) {
			Vector4 q = this;
			if (Math.Abs(q.W) > 1f)
				q.Normalize();

			float w = 2f * (float) Math.Acos(q.W); // angle
			float den = (float) Math.Sqrt(1.0 - q.W * q.W);
			if (den > 0.0001f)
				vec = new Vector4(q.Xyz / den, w);
			else
				vec = new Vector4(1f, 0f, 0f, w);

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
		/// <param name="result">Result of operation.</param>
		public static void Add(ref Vector4 a, ref Vector4 b, out Vector4 result) {
			result = new Vector4() {
				X = a.X + b.X,
				Y = a.Y + b.Y,
				Z = a.Z + b.Z,
				W = a.W + b.W
			};
		}

		#endregion

		#region Subtract

		/// <summary>
		/// Subtract one Vector from another
		/// </summary>
		/// <param name="a">First operand</param>
		/// <param name="b">Second operand</param>
		/// <param name="result">Result of subtraction</param>
		public static void Subtract(ref Vector4 a, ref Vector4 b, out Vector4 result) {
			result = new Vector4() {
				X = a.X - b.X,
				Y = a.Y - b.Y,
				Z = a.Z - b.Z,
				W = a.W - b.W
			};
		}

		#endregion

		#region Multiply

		/// <summary>
		/// Multiplies a vector by a scalar.
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <param name="result">Result of the operation.</param>
		public static void Multiply(ref Vector4 vector, float scale, out Vector4 result) {
			result = new Vector4() {
				X = vector.X * scale,
				Y = vector.Y * scale,
				Z = vector.Z * scale,
				W = vector.W * scale
			};
		}

		/// <summary>
		/// Multiplies a vector by the components of a vector (scale).
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <param name="result">Result of the operation.</param>
		public static void Multiply(ref Vector4 vector, ref Vector4 scale, out Vector4 result) {
			result = new Vector4(vector.X * scale.X, vector.Y * scale.Y, vector.Z * scale.Z, vector.W * scale.W);
		}

		#endregion

		#region Divide

		/// <summary>
		/// Divides a vector by a scalar.
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <param name="result">Result of the operation.</param>
		public static void Divide(ref Vector4 vector, float scale, out Vector4 result) {
			Multiply(ref vector, 1f / scale, out result);
		}

		#endregion

		#region Min

		/// <summary>
		/// Calculate the component-wise minimum of two vectors
		/// </summary>
		/// <param name="a">First operand</param>
		/// <param name="b">Second operand</param>
		/// <param name="result">The component-wise minimum</param>
		public static void Min(ref Vector4 a, ref Vector4 b, out Vector4 result) {
			result = new Vector4() {
				X = a.X < b.X ? a.X : b.X,
				Y = a.Y < b.Y ? a.Y : b.Y,
				Z = a.Z < b.Z ? a.Z : b.Z,
				W = a.W < b.W ? a.W : b.W
			};
		}

		#endregion

		#region Max

		/// <summary>
		/// Calculate the component-wise maximum of two vectors
		/// </summary>
		/// <param name="a">First operand</param>
		/// <param name="b">Second operand</param>
		/// <param name="result">The component-wise maximum</param>
		public static void Max(ref Vector4 a, ref Vector4 b, out Vector4 result) {
			result = new Vector4() {
				X = a.X > b.X ? a.X : b.X,
				Y = a.Y > b.Y ? a.Y : b.Y,
				Z = a.Z > b.Z ? a.Z : b.Z,
				W = a.W > b.W ? a.W : b.W
			};
		}

		#endregion

		#region Clamp

		/// <summary>
		/// Clamp a vector to the given minimum and maximum vectors
		/// </summary>
		/// <param name="vec">Input vector</param>
		/// <param name="min">Minimum vector</param>
		/// <param name="max">Maximum vector</param>
		/// <param name="result">The clamped vector</param>
		public static void Clamp(ref Vector4 vec, ref Vector4 min, ref Vector4 max, out Vector4 result) {
			result = new Vector4() {
				X = vec.X < min.X ? min.X : vec.X > max.X ? max.X : vec.X,
				Y = vec.Y < min.Y ? min.Y : vec.Y > max.Y ? max.Y : vec.Y,
				Z = vec.X < min.Z ? min.Z : vec.Z > max.Z ? max.Z : vec.Z,
				W = vec.Y < min.W ? min.W : vec.W > max.W ? max.W : vec.W
			};
		}

		#endregion

		#region Normalize

		/// <summary>
		/// Scale a vector to unit length
		/// </summary>
		/// <param name="vec">The input vector</param>
		/// <param name="result">The normalized vector</param>
		public static void Normalize(ref Vector4 vec, out Vector4 result) {
			float scale = 1f / vec.Length;
			result = new Vector4() {
				X = vec.X * scale,
				Y = vec.Y * scale,
				Z = vec.Z * scale,
				W = vec.W * scale
			};
		}

		#endregion

		#region NormalizeFast

		/// <summary>
		/// Scale a vector to approximately unit length
		/// </summary>
		/// <param name="vec">The input vector</param>
		/// <param name="result">The normalized vector</param>
		public static void NormalizeFast(ref Vector4 vec, out Vector4 result) {
			float scale = MathHelper.InverseSqrtFast(vec.X * vec.X + vec.Y * vec.Y + vec.Z * vec.Z + vec.W * vec.W);
			result = new Vector4() {
				X = vec.X * scale,
				Y = vec.Y * scale,
				Z = vec.Z * scale,
				W = vec.W * scale
			};
		}

		#endregion

		#region Dot

		/// <summary>
		/// Calculate the dot product of two vectors
		/// </summary>
		/// <param name="left">First operand</param>
		/// <param name="right">Second operand</param>
		public static float Dot(ref Vector4 left, ref Vector4 right) {
			return left.X * right.X + left.Y * right.Y + left.Z * right.Z + left.W * right.W;
		}

		#endregion

		#region Lerp

		/// <summary>
		/// Returns a new Vector that is the linear blend of the 2 given Vectors
		/// </summary>
		/// <param name="a">First input vector</param>
		/// <param name="b">Second input vector</param>
		/// <param name="blend">The blend factor. a when blend=0, b when blend=1.</param>
		/// <param name="result">a when blend=0, b when blend=1, and a linear combination otherwise</param>
		public static void Lerp(ref Vector4 a, ref Vector4 b, float blend, out Vector4 result) {
			result.X = blend * (b.X - a.X) + a.X;
			result.Y = blend * (b.Y - a.Y) + a.Y;
			result.Z = blend * (b.Z - a.Z) + a.Z;
			result.W = blend * (b.W - a.W) + a.W;
		}

		#endregion

		#region Barycentric

		/// <summary>Interpolate 3 Vectors using Barycentric coordinates</summary>
		/// <param name="a">First input Vector.</param>
		/// <param name="b">Second input Vector.</param>
		/// <param name="c">Third input Vector.</param>
		/// <param name="u">First Barycentric Coordinate.</param>
		/// <param name="v">Second Barycentric Coordinate.</param>
		/// <param name="result">Output Vector. a when u=v=0, b when u=1,v=0, c when u=0,v=1, and a linear combination of a,b,c otherwise</param>
		public static void BaryCentric(ref Vector4 a, ref Vector4 b, ref Vector4 c, float u, float v, out Vector4 result) {
			result = a; // copy

			Vector4 temp = b; // copy
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
		/// <param name="result">The transformed vector</param>
		public static void Transform(ref Vector4 vec, ref Matrix4 mat, out Vector4 result) {
			result = new Vector4(
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
		/// <param name="result">The result of the operation.</param>
		public static void Transform(ref Vector4 vec, ref Vector4 quat, out Vector4 result) {
			Vector4 v = new Vector4(vec.X, vec.Y, vec.Z, vec.W), i, t;
			Invert(ref quat, out i);
			Multiply(ref quat, ref v, out t);
			Multiply(ref t, ref i, out v);

			result = new Vector4(v.X, v.Y, v.Z, v.W);
		}

		#endregion

		#region Conjugate

		/// <summary>
		/// Get the conjugate of the given quaternion
		/// </summary>
		/// <param name="q">The quaternion</param>
		/// <returns>The conjugate of the given quaternion</returns>
		public static Vector4 Conjugate(Vector4 q) {
			return new Vector4(-q.Xyz, q.W);
		}

		/// <summary>
		/// Get the conjugate of the given quaternion
		/// </summary>
		/// <param name="q">The quaternion</param>
		/// <param name="result">The conjugate of the given quaternion</param>
		public static void Conjugate(ref Vector4 q, out Vector4 result) {
			result = new Vector4(-q.Xyz, q.W);
		}

		#endregion

		#region Invert

		/// <summary>
		/// Get the inverse of the given quaternion
		/// </summary>
		/// <param name="q">The quaternion to invert</param>
		/// <returns>The inverse of the given quaternion</returns>
		public static Vector4 Invert(Vector4 q) {
			Vector4 result;
			Invert(ref q, out result);
			return result;
		}

		/// <summary>
		/// Get the inverse of the given quaternion
		/// </summary>
		/// <param name="q">The quaternion to invert</param>
		/// <param name="result">The inverse of the given quaternion</param>
		public static void Invert(ref Vector4 q, out Vector4 result) {
			float lengthSq = q.LengthSquared;
			if (lengthSq != 0.0) {
				float i = 1f / lengthSq;
				result = new Vector4(q.Xyz * -i, q.W * i);
			} else {
				result = q;
			}
		}

		/// <summary>
		/// Reverses the rotation angle of this quaternion.
		/// </summary>
		public void InvertQuaternion() {
			W = -W;
		}

		#endregion

		#region FromAxisAngle

		/// <summary>
		/// Build a quaternion from the given axis and angle
		/// </summary>
		/// <param name="axis">The axis to rotate about</param>
		/// <param name="angle">The rotation angle in radians</param>
		/// <returns></returns>
		public static Vector4 FromAxisAngle(Vector3 axis, float angle) {
			if (axis.LengthSquared == 0f)
				return UnitW;

			Vector4 result = new Vector4();

			angle *= 0.5f;
			axis.Normalize();
			result.Xyz = axis * (float) System.Math.Sin(angle);
			result.W = (float) System.Math.Cos(angle);

			return Normalize(result);
		}

		#endregion

		/// <summary>
		/// Builds a Quaternion from the given euler angles
		/// </summary>
		/// <param name="pitch">The pitch (attitude), rotation around X axis</param>
		/// <param name="yaw">The yaw (heading), rotation around Y axis</param>
		/// <param name="roll">The roll (bank), rotation around Z axis</param>
		/// <returns></returns>
		public static Quaternion FromEulerAngles(float pitch, float yaw, float roll) {
			return new Quaternion(pitch, yaw, roll);
		}

		/// <summary>
		/// Builds a Quaternion from the given euler angles
		/// </summary>
		/// <param name="eulerAngles">The euler angles as a vector</param>
		/// <returns>The equivalent Quaternion</returns>
		public static Quaternion FromEulerAngles(Vector3 eulerAngles) {
			return new Quaternion(eulerAngles);
		}

		/// <summary>
		/// Builds a Quaternion from the given euler angles
		/// </summary>
		/// <param name="eulerAngles">The euler angles a vector</param>
		/// <param name="result">The equivalent Quaternion</param>
		public static void FromEulerAngles(ref Vector3 eulerAngles, out Quaternion result) {
			float c1 = (float) Math.Cos(eulerAngles.Y * 0.5f);
			float c2 = (float) Math.Cos(eulerAngles.X * 0.5f);
			float c3 = (float) Math.Cos(eulerAngles.Z * 0.5f);
			float s1 = (float) Math.Sin(eulerAngles.Y * 0.5f);
			float s2 = (float) Math.Sin(eulerAngles.X * 0.5f);
			float s3 = (float) Math.Sin(eulerAngles.Z * 0.5f);

			result.w = c1 * c2 * c3 - s1 * s2 * s3;
			result.xyz.X = s1 * s2 * c3 + c1 * c2 * s3;
			result.xyz.Y = s1 * c2 * c3 + c1 * s2 * s3;
			result.xyz.Z = c1 * s2 * c3 - s1 * c2 * s3;
		}

		#region Slerp

		/// <summary>
		/// Do Spherical linear interpolation between two quaternions 
		/// </summary>
		/// <param name="q1">The first quaternion</param>
		/// <param name="q2">The second quaternion</param>
		/// <param name="blend">The blend factor</param>
		/// <returns>A smooth blend between the given quaternions</returns>
		public static Vector4 Slerp(Vector4 q1, Vector4 q2, float blend) {
			// if either input is zero, return the other.
			if (q1.LengthSquared == 0f) {
				if (q2.LengthSquared == 0f) {
					return UnitW;
				}
				return q2;
			} else if (q2.LengthSquared == 0f) {
				return q1;
			}


			float cosHalfAngle = q1.W * q2.W + Vector3.Dot(q1.Xyz, q2.Xyz);

			if (cosHalfAngle >= 1f || cosHalfAngle <= -1f) {
				// angle = 0f, so just return one input.
				return q1;
			} else if (cosHalfAngle < 0f) {
				q2.Xyz = -q2.Xyz;
				q2.W = -q2.W;
				cosHalfAngle = -cosHalfAngle;
			}

			float blendA;
			float blendB;
			if (cosHalfAngle < 0.99f) {
				// do proper slerp for big angles
				float halfAngle = (float) System.Math.Acos(cosHalfAngle);
				float sinHalfAngle = (float) System.Math.Sin(halfAngle);
				float oneOverSinHalfAngle = 1f / sinHalfAngle;
				blendA = (float) System.Math.Sin(halfAngle * (1f - blend)) * oneOverSinHalfAngle;
				blendB = (float) System.Math.Sin(halfAngle * blend) * oneOverSinHalfAngle;
			} else {
				// do lerp if angle is really small.
				blendA = 1f - blend;
				blendB = blend;
			}

			Vector4 result = new Vector4(blendA * q1.Xyz + blendB * q2.Xyz, blendA * q1.W + blendB * q2.W);
			if (result.LengthSquared > 0f)
				return Normalize(result);
			else
				return UnitW;
		}

		/// <summary>
		/// Builds a quaternion from the given rotation matrix
		/// </summary>
		/// <param name="matrix">A rotation matrix</param>
		/// <param name="result">The equivalent quaternion</param>
		public static void FromMatrix(ref Matrix3 matrix, out Quaternion result) {
			float trace = matrix.Trace;

			if (trace > 0) {
				float s = (float) Math.Sqrt(trace + 1) * 2;
				float invS = 1f / s;

				result.w = s * 0.25f;
				result.xyz.X = (matrix.Row2.Y - matrix.Row1.Z) * invS;
				result.xyz.Y = (matrix.Row0.Z - matrix.Row2.X) * invS;
				result.xyz.Z = (matrix.Row1.X - matrix.Row0.Y) * invS;
			} else {
				float m00 = matrix.Row0.X, m11 = matrix.Row1.Y, m22 = matrix.Row2.Z;

				if (m00 > m11 && m00 > m22) {
					float s = (float) Math.Sqrt(1 + m00 - m11 - m22) * 2;
					float invS = 1f / s;

					result.w = (matrix.Row2.Y - matrix.Row1.Z) * invS;
					result.xyz.X = s * 0.25f;
					result.xyz.Y = (matrix.Row0.Y + matrix.Row1.X) * invS;
					result.xyz.Z = (matrix.Row0.Z + matrix.Row2.X) * invS;
				} else if (m11 > m22) {
					float s = (float) Math.Sqrt(1 + m11 - m00 - m22) * 2;
					float invS = 1f / s;

					result.w = (matrix.Row0.Z - matrix.Row2.X) * invS;
					result.xyz.X = (matrix.Row0.Y + matrix.Row1.X) * invS;
					result.xyz.Y = s * 0.25f;
					result.xyz.Z = (matrix.Row1.Z + matrix.Row2.Y) * invS;
				} else {
					float s = (float) Math.Sqrt(1 + m22 - m00 - m11) * 2;
					float invS = 1f / s;

					result.w = (matrix.Row1.X - matrix.Row0.Y) * invS;
					result.xyz.X = (matrix.Row0.Z + matrix.Row2.X) * invS;
					result.xyz.Y = (matrix.Row1.Z + matrix.Row2.Y) * invS;
					result.xyz.Z = s * 0.25f;
				}
			}
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
		public static Vector4 operator +(Vector4 left, Vector4 right) {
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
		public static Vector4 operator -(Vector4 left, Vector4 right) {
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
		public static Vector4 operator -(Vector4 vec) {
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
		public static Vector4 operator *(Vector4 vec, float scale) {
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
		public static Vector4 operator *(float scale, Vector4 vec) {
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
		public static Vector4 operator /(Vector4 vec, float scale) {
			float mult = 1f / scale;
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
		public static bool operator ==(Vector4 left, Vector4 right) {
			return left.Equals(right);
		}

		/// <summary>
		/// Compares two instances for inequality.
		/// </summary>
		/// <param name="left">The first instance.</param>
		/// <param name="right">The second instance.</param>
		/// <returns>True, if left does not equa lright; false otherwise.</returns>
		public static bool operator !=(Vector4 left, Vector4 right) {
			return !left.Equals(right);
		}

		/// <summary>
		/// Returns a pointer to the first element of the specified instance.
		/// </summary>
		/// <param name="v">The instance.</param>
		/// <returns>A pointer to the first element of v.</returns>
		[CLSCompliant(false)]
		unsafe public static explicit operator float*(Vector4 v) {
			return &v.X;
		}

		/// <summary>
		/// Returns a pointer to the first element of the specified instance.
		/// </summary>
		/// <param name="v">The instance.</param>
		/// <returns>A pointer to the first element of v.</returns>
		public static explicit operator IntPtr(Vector4 v) {
			unsafe {
				return (IntPtr) (&v.X);
			}
		}

		#endregion

		#region Overrides

		#region public override string ToString()

		/// <summary>
		/// Returns a System.String that represents the current Vector4.
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
			if (!(obj is Vector4))
				return false;

			return this.Equals((Vector4) obj);
		}

		#endregion

		#endregion

		#endregion

		#region IEquatable<Vector4> Members

		/// <summary>Indicates whether the current vector is equal to another vector.</summary>
		/// <param name="other">A vector to compare with this vector.</param>
		/// <returns>true if the current vector is equal to the vector parameter; otherwise, false.</returns>
		public bool Equals(Vector4 other) {
			return
				X == other.X &&
				Y == other.Y &&
				Z == other.Z &&
				W == other.W;
		}

		#endregion
	}
}