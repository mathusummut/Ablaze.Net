using System.Globalization;

namespace System.Numerics {
	/// <summary>
	/// A structure encapsulating a four-dimensional vector (x,y,z,w),
	/// which is used to efficiently rotate an object about the (x,y,z) vector by the angle theta, where w = cos(theta/2).
	/// </summary>
	[Serializable]
	public struct Quaternion : IEquatable<Quaternion> {
		/// <summary>
		/// Specifies the X-value of the vector component of the Quaternion.
		/// </summary>
		public float X;
		/// <summary>
		/// Specifies the Y-value of the vector component of the Quaternion.
		/// </summary>
		public float Y;
		/// <summary>
		/// Specifies the Z-value of the vector component of the Quaternion.
		/// </summary>
		public float Z;
		/// <summary>Specifies the rotation component of the Quaternion.</summary>
		public float W;

		/// <summary>Returns a Quaternion representing no rotation.</summary>
		public static Quaternion Identity {
			get {
				return new Quaternion(0.0f, 0.0f, 0.0f, 1f);
			}
		}

		/// <summary>
		/// Returns whether the Quaternion is the identity Quaternion.
		/// </summary>
		public bool IsIdentity {
			get {
				if ((double) this.X == 0.0 && (double) this.Y == 0.0 && (double) this.Z == 0.0)
					return (double) this.W == 1.0;
				return false;
			}
		}

		/// <summary>Constructs a Quaternion from the given components.</summary>
		/// <param name="x">The X component of the Quaternion.</param>
		/// <param name="y">The Y component of the Quaternion.</param>
		/// <param name="z">The Z component of the Quaternion.</param>
		/// <param name="w">The W component of the Quaternion.</param>
		public Quaternion(float x, float y, float z, float w) {
			this.X = x;
			this.Y = y;
			this.Z = z;
			this.W = w;
		}

		/// <summary>
		/// Constructs a Quaternion from the given vector and rotation parts.
		/// </summary>
		/// <param name="vectorPart">The vector part of the Quaternion.</param>
		/// <param name="scalarPart">The rotation part of the Quaternion.</param>
		public Quaternion(Vector3 vectorPart, float scalarPart) {
			this.X = vectorPart.X;
			this.Y = vectorPart.Y;
			this.Z = vectorPart.Z;
			this.W = scalarPart;
		}

		/// <summary>Flips the sign of each component of the quaternion.</summary>
		/// <param name="value">The source Quaternion.</param>
		/// <returns>The negated Quaternion.</returns>
		public static Quaternion operator -(Quaternion value) {
			Quaternion quaternion;
			quaternion.X = -value.X;
			quaternion.Y = -value.Y;
			quaternion.Z = -value.Z;
			quaternion.W = -value.W;
			return quaternion;
		}

		/// <summary>Adds two Quaternions element-by-element.</summary>
		/// <param name="value1">The first source Quaternion.</param>
		/// <param name="value2">The second source Quaternion.</param>
		/// <returns>The result of adding the Quaternions.</returns>
		public static Quaternion operator +(Quaternion value1, Quaternion value2) {
			Quaternion quaternion;
			quaternion.X = value1.X + value2.X;
			quaternion.Y = value1.Y + value2.Y;
			quaternion.Z = value1.Z + value2.Z;
			quaternion.W = value1.W + value2.W;
			return quaternion;
		}

		/// <summary>Subtracts one Quaternion from another.</summary>
		/// <param name="value1">The first source Quaternion.</param>
		/// <param name="value2">The second Quaternion, to be subtracted from the first.</param>
		/// <returns>The result of the subtraction.</returns>
		public static Quaternion operator -(Quaternion value1, Quaternion value2) {
			Quaternion quaternion;
			quaternion.X = value1.X - value2.X;
			quaternion.Y = value1.Y - value2.Y;
			quaternion.Z = value1.Z - value2.Z;
			quaternion.W = value1.W - value2.W;
			return quaternion;
		}

		/// <summary>Multiplies two Quaternions together.</summary>
		/// <param name="value1">The Quaternion on the left side of the multiplication.</param>
		/// <param name="value2">The Quaternion on the right side of the multiplication.</param>
		/// <returns>The result of the multiplication.</returns>
		public static Quaternion operator *(Quaternion value1, Quaternion value2) {
			float x1 = value1.X;
			float y1 = value1.Y;
			float z1 = value1.Z;
			float w1 = value1.W;
			float x2 = value2.X;
			float y2 = value2.Y;
			float z2 = value2.Z;
			float w2 = value2.W;
			float num1 = (float) ((double) y1 * (double) z2 - (double) z1 * (double) y2);
			float num2 = (float) ((double) z1 * (double) x2 - (double) x1 * (double) z2);
			float num3 = (float) ((double) x1 * (double) y2 - (double) y1 * (double) x2);
			float num4 = (float) ((double) x1 * (double) x2 + (double) y1 * (double) y2 + (double) z1 * (double) z2);
			Quaternion quaternion;
			quaternion.X = (float) ((double) x1 * (double) w2 + (double) x2 * (double) w1) + num1;
			quaternion.Y = (float) ((double) y1 * (double) w2 + (double) y2 * (double) w1) + num2;
			quaternion.Z = (float) ((double) z1 * (double) w2 + (double) z2 * (double) w1) + num3;
			quaternion.W = w1 * w2 - num4;
			return quaternion;
		}

		/// <summary>Multiplies a Quaternion by a scalar value.</summary>
		/// <param name="value1">The source Quaternion.</param>
		/// <param name="value2">The scalar value.</param>
		/// <returns>The result of the multiplication.</returns>
		public static Quaternion operator *(Quaternion value1, float value2) {
			Quaternion quaternion;
			quaternion.X = value1.X * value2;
			quaternion.Y = value1.Y * value2;
			quaternion.Z = value1.Z * value2;
			quaternion.W = value1.W * value2;
			return quaternion;
		}

		/// <summary>Divides a Quaternion by another Quaternion.</summary>
		/// <param name="value1">The source Quaternion.</param>
		/// <param name="value2">The divisor.</param>
		/// <returns>The result of the division.</returns>
		public static Quaternion operator /(Quaternion value1, Quaternion value2) {
			float x = value1.X;
			float y = value1.Y;
			float z = value1.Z;
			float w = value1.W;
			float num1 = 1f / (float) ((double) value2.X * (double) value2.X + (double) value2.Y * (double) value2.Y + (double) value2.Z * (double) value2.Z + (double) value2.W * (double) value2.W);
			float num2 = -value2.X * num1;
			float num3 = -value2.Y * num1;
			float num4 = -value2.Z * num1;
			float num5 = value2.W * num1;
			float num6 = (float) ((double) y * (double) num4 - (double) z * (double) num3);
			float num7 = (float) ((double) z * (double) num2 - (double) x * (double) num4);
			float num8 = (float) ((double) x * (double) num3 - (double) y * (double) num2);
			float num9 = (float) ((double) x * (double) num2 + (double) y * (double) num3 + (double) z * (double) num4);
			Quaternion quaternion;
			quaternion.X = (float) ((double) x * (double) num5 + (double) num2 * (double) w) + num6;
			quaternion.Y = (float) ((double) y * (double) num5 + (double) num3 * (double) w) + num7;
			quaternion.Z = (float) ((double) z * (double) num5 + (double) num4 * (double) w) + num8;
			quaternion.W = w * num5 - num9;
			return quaternion;
		}

		/// <summary>
		/// Returns a boolean indicating whether the two given Quaternions are equal.
		/// </summary>
		/// <param name="value1">The first Quaternion to compare.</param>
		/// <param name="value2">The second Quaternion to compare.</param>
		/// <returns>True if the Quaternions are equal; False otherwise.</returns>
		public static bool operator ==(Quaternion value1, Quaternion value2) {
			if ((double) value1.X == (double) value2.X && (double) value1.Y == (double) value2.Y && (double) value1.Z == (double) value2.Z)
				return (double) value1.W == (double) value2.W;
			return false;
		}

		/// <summary>
		/// Returns a boolean indicating whether the two given Quaternions are not equal.
		/// </summary>
		/// <param name="value1">The first Quaternion to compare.</param>
		/// <param name="value2">The second Quaternion to compare.</param>
		/// <returns>True if the Quaternions are not equal; False if they are equal.</returns>
		public static bool operator !=(Quaternion value1, Quaternion value2) {
			if ((double) value1.X == (double) value2.X && (double) value1.Y == (double) value2.Y && (double) value1.Z == (double) value2.Z)
				return (double) value1.W != (double) value2.W;
			return true;
		}

		/// <summary>Calculates the length of the Quaternion.</summary>
		/// <returns>The computed length of the Quaternion.</returns>
		public float Length() {
			return (float) Math.Sqrt((double) this.X * (double) this.X + (double) this.Y * (double) this.Y + (double) this.Z * (double) this.Z + (double) this.W * (double) this.W);
		}

		/// <summary>
		/// Calculates the length squared of the Quaternion. This operation is cheaper than Length().
		/// </summary>
		/// <returns>The length squared of the Quaternion.</returns>
		public float LengthSquared() {
			return (float) ((double) this.X * (double) this.X + (double) this.Y * (double) this.Y + (double) this.Z * (double) this.Z + (double) this.W * (double) this.W);
		}

		/// <summary>
		/// Divides each component of the Quaternion by the length of the Quaternion.
		/// </summary>
		/// <param name="value">The source Quaternion.</param>
		/// <returns>The normalized Quaternion.</returns>
		public static Quaternion Normalize(Quaternion value) {
			float num = 1f / (float) Math.Sqrt((double) value.X * (double) value.X + (double) value.Y * (double) value.Y + (double) value.Z * (double) value.Z + (double) value.W * (double) value.W);
			Quaternion quaternion;
			quaternion.X = value.X * num;
			quaternion.Y = value.Y * num;
			quaternion.Z = value.Z * num;
			quaternion.W = value.W * num;
			return quaternion;
		}

		/// <summary>Creates the conjugate of a specified Quaternion.</summary>
		/// <param name="value">The Quaternion of which to return the conjugate.</param>
		/// <returns>A new Quaternion that is the conjugate of the specified one.</returns>
		public static Quaternion Conjugate(Quaternion value) {
			Quaternion quaternion;
			quaternion.X = -value.X;
			quaternion.Y = -value.Y;
			quaternion.Z = -value.Z;
			quaternion.W = value.W;
			return quaternion;
		}

		/// <summary>Returns the inverse of a Quaternion.</summary>
		/// <param name="value">The source Quaternion.</param>
		/// <returns>The inverted Quaternion.</returns>
		public static Quaternion Inverse(Quaternion value) {
			float num = 1f / (float) ((double) value.X * (double) value.X + (double) value.Y * (double) value.Y + (double) value.Z * (double) value.Z + (double) value.W * (double) value.W);
			Quaternion quaternion;
			quaternion.X = -value.X * num;
			quaternion.Y = -value.Y * num;
			quaternion.Z = -value.Z * num;
			quaternion.W = value.W * num;
			return quaternion;
		}

		/// <summary>
		/// Creates a Quaternion from a normalized vector axis and an angle to rotate about the vector.
		/// </summary>
		/// <param name="axis">The unit vector to rotate around.
		/// This vector must be normalized before calling this function or the resulting Quaternion will be incorrect.</param>
		/// <param name="angle">The angle, in radians, to rotate around the vector.</param>
		/// <returns>The created Quaternion.</returns>
		public static Quaternion CreateFromAxisAngle(Vector3 axis, float angle) {
			float num1 = angle * 0.5f;
			float num2 = (float) Math.Sin((double) num1);
			float num3 = (float) Math.Cos((double) num1);
			Quaternion quaternion;
			quaternion.X = axis.X * num2;
			quaternion.Y = axis.Y * num2;
			quaternion.Z = axis.Z * num2;
			quaternion.W = num3;
			return quaternion;
		}

		/// <summary>
		/// Creates a new Quaternion from the given yaw, pitch, and roll, in radians.
		/// </summary>
		/// <param name="yaw">The yaw angle, in radians, around the Y-axis.</param>
		/// <param name="pitch">The pitch angle, in radians, around the X-axis.</param>
		/// <param name="roll">The roll angle, in radians, around the Z-axis.</param>
		/// <returns></returns>
		public static Quaternion CreateFromYawPitchRoll(float yaw, float pitch, float roll) {
			float num1 = roll * 0.5f;
			float num2 = (float) Math.Sin((double) num1);
			float num3 = (float) Math.Cos((double) num1);
			float num4 = pitch * 0.5f;
			float num5 = (float) Math.Sin((double) num4);
			float num6 = (float) Math.Cos((double) num4);
			float num7 = yaw * 0.5f;
			float num8 = (float) Math.Sin((double) num7);
			float num9 = (float) Math.Cos((double) num7);
			Quaternion quaternion;
			quaternion.X = (float) ((double) num9 * (double) num5 * (double) num3 + (double) num8 * (double) num6 * (double) num2);
			quaternion.Y = (float) ((double) num8 * (double) num6 * (double) num3 - (double) num9 * (double) num5 * (double) num2);
			quaternion.Z = (float) ((double) num9 * (double) num6 * (double) num2 - (double) num8 * (double) num5 * (double) num3);
			quaternion.W = (float) ((double) num9 * (double) num6 * (double) num3 + (double) num8 * (double) num5 * (double) num2);
			return quaternion;
		}

		/// <summary>Calculates the dot product of two Quaternions.</summary>
		/// <param name="quaternion1">The first source Quaternion.</param>
		/// <param name="quaternion2">The second source Quaternion.</param>
		/// <returns>The dot product of the Quaternions.</returns>
		public static float Dot(Quaternion quaternion1, Quaternion quaternion2) {
			return (float) ((double) quaternion1.X * (double) quaternion2.X + (double) quaternion1.Y * (double) quaternion2.Y + (double) quaternion1.Z * (double) quaternion2.Z + (double) quaternion1.W * (double) quaternion2.W);
		}

		/// <summary>
		/// Interpolates between two quaternions, using spherical linear interpolation.
		/// </summary>
		/// <param name="quaternion1">The first source Quaternion.</param>
		/// <param name="quaternion2">The second source Quaternion.</param>
		/// <param name="amount">The relative weight of the second source Quaternion in the interpolation.</param>
		/// <returns>The interpolated Quaternion.</returns>
		public static Quaternion Slerp(Quaternion quaternion1, Quaternion quaternion2, float amount) {
			float num1 = amount;
			float num2 = (float) ((double) quaternion1.X * (double) quaternion2.X + (double) quaternion1.Y * (double) quaternion2.Y + (double) quaternion1.Z * (double) quaternion2.Z + (double) quaternion1.W * (double) quaternion2.W);
			bool flag = false;
			if ((double) num2 < 0.0) {
				flag = true;
				num2 = -num2;
			}
			float num3;
			float num4;
			if ((double) num2 > 0.999998986721039) {
				num3 = 1f - num1;
				num4 = flag ? -num1 : num1;
			} else {
				float num5 = (float) Math.Acos((double) num2);
				float num6 = (float) (1.0 / Math.Sin((double) num5));
				num3 = (float) Math.Sin((1.0 - (double) num1) * (double) num5) * num6;
				num4 = flag ? (float) -Math.Sin((double) num1 * (double) num5) * num6 : (float) Math.Sin((double) num1 * (double) num5) * num6;
			}
			Quaternion quaternion;
			quaternion.X = (float) ((double) num3 * (double) quaternion1.X + (double) num4 * (double) quaternion2.X);
			quaternion.Y = (float) ((double) num3 * (double) quaternion1.Y + (double) num4 * (double) quaternion2.Y);
			quaternion.Z = (float) ((double) num3 * (double) quaternion1.Z + (double) num4 * (double) quaternion2.Z);
			quaternion.W = (float) ((double) num3 * (double) quaternion1.W + (double) num4 * (double) quaternion2.W);
			return quaternion;
		}

		/// <summary>Linearly interpolates between two quaternions.</summary>
		/// <param name="quaternion1">The first source Quaternion.</param>
		/// <param name="quaternion2">The second source Quaternion.</param>
		/// <param name="amount">The relative weight of the second source Quaternion in the interpolation.</param>
		/// <returns>The interpolated Quaternion.</returns>
		public static Quaternion Lerp(Quaternion quaternion1, Quaternion quaternion2, float amount) {
			float num1 = amount;
			float num2 = 1f - num1;
			Quaternion quaternion = new Quaternion();
			if ((double) quaternion1.X * (double) quaternion2.X + (double) quaternion1.Y * (double) quaternion2.Y + (double) quaternion1.Z * (double) quaternion2.Z + (double) quaternion1.W * (double) quaternion2.W >= 0.0) {
				quaternion.X = (float) ((double) num2 * (double) quaternion1.X + (double) num1 * (double) quaternion2.X);
				quaternion.Y = (float) ((double) num2 * (double) quaternion1.Y + (double) num1 * (double) quaternion2.Y);
				quaternion.Z = (float) ((double) num2 * (double) quaternion1.Z + (double) num1 * (double) quaternion2.Z);
				quaternion.W = (float) ((double) num2 * (double) quaternion1.W + (double) num1 * (double) quaternion2.W);
			} else {
				quaternion.X = (float) ((double) num2 * (double) quaternion1.X - (double) num1 * (double) quaternion2.X);
				quaternion.Y = (float) ((double) num2 * (double) quaternion1.Y - (double) num1 * (double) quaternion2.Y);
				quaternion.Z = (float) ((double) num2 * (double) quaternion1.Z - (double) num1 * (double) quaternion2.Z);
				quaternion.W = (float) ((double) num2 * (double) quaternion1.W - (double) num1 * (double) quaternion2.W);
			}
			float num3 = 1f / (float) Math.Sqrt((double) quaternion.X * (double) quaternion.X + (double) quaternion.Y * (double) quaternion.Y + (double) quaternion.Z * (double) quaternion.Z + (double) quaternion.W * (double) quaternion.W);
			quaternion.X *= num3;
			quaternion.Y *= num3;
			quaternion.Z *= num3;
			quaternion.W *= num3;
			return quaternion;
		}

		/// <summary>
		/// Concatenates two Quaternions; the result represents the value1 rotation followed by the value2 rotation.
		/// </summary>
		/// <param name="value1">The first Quaternion rotation in the series.</param>
		/// <param name="value2">The second Quaternion rotation in the series.</param>
		/// <returns>A new Quaternion representing the concatenation of the value1 rotation followed by the value2 rotation.</returns>
		public static Quaternion Concatenate(Quaternion value1, Quaternion value2) {
			float x1 = value2.X;
			float y1 = value2.Y;
			float z1 = value2.Z;
			float w1 = value2.W;
			float x2 = value1.X;
			float y2 = value1.Y;
			float z2 = value1.Z;
			float w2 = value1.W;
			float num1 = (float) ((double) y1 * (double) z2 - (double) z1 * (double) y2);
			float num2 = (float) ((double) z1 * (double) x2 - (double) x1 * (double) z2);
			float num3 = (float) ((double) x1 * (double) y2 - (double) y1 * (double) x2);
			float num4 = (float) ((double) x1 * (double) x2 + (double) y1 * (double) y2 + (double) z1 * (double) z2);
			Quaternion quaternion;
			quaternion.X = (float) ((double) x1 * (double) w2 + (double) x2 * (double) w1) + num1;
			quaternion.Y = (float) ((double) y1 * (double) w2 + (double) y2 * (double) w1) + num2;
			quaternion.Z = (float) ((double) z1 * (double) w2 + (double) z2 * (double) w1) + num3;
			quaternion.W = w1 * w2 - num4;
			return quaternion;
		}

		/// <summary>Flips the sign of each component of the quaternion.</summary>
		/// <param name="value">The source Quaternion.</param>
		/// <returns>The negated Quaternion.</returns>
		public static Quaternion Negate(Quaternion value) {
			Quaternion quaternion;
			quaternion.X = -value.X;
			quaternion.Y = -value.Y;
			quaternion.Z = -value.Z;
			quaternion.W = -value.W;
			return quaternion;
		}

		/// <summary>Adds two Quaternions element-by-element.</summary>
		/// <param name="value1">The first source Quaternion.</param>
		/// <param name="value2">The second source Quaternion.</param>
		/// <returns>The result of adding the Quaternions.</returns>
		public static Quaternion Add(Quaternion value1, Quaternion value2) {
			Quaternion quaternion;
			quaternion.X = value1.X + value2.X;
			quaternion.Y = value1.Y + value2.Y;
			quaternion.Z = value1.Z + value2.Z;
			quaternion.W = value1.W + value2.W;
			return quaternion;
		}

		/// <summary>Subtracts one Quaternion from another.</summary>
		/// <param name="value1">The first source Quaternion.</param>
		/// <param name="value2">The second Quaternion, to be subtracted from the first.</param>
		/// <returns>The result of the subtraction.</returns>
		public static Quaternion Subtract(Quaternion value1, Quaternion value2) {
			Quaternion quaternion;
			quaternion.X = value1.X - value2.X;
			quaternion.Y = value1.Y - value2.Y;
			quaternion.Z = value1.Z - value2.Z;
			quaternion.W = value1.W - value2.W;
			return quaternion;
		}

		/// <summary>Multiplies two Quaternions together.</summary>
		/// <param name="value1">The Quaternion on the left side of the multiplication.</param>
		/// <param name="value2">The Quaternion on the right side of the multiplication.</param>
		/// <returns>The result of the multiplication.</returns>
		public static Quaternion Multiply(Quaternion value1, Quaternion value2) {
			float x1 = value1.X;
			float y1 = value1.Y;
			float z1 = value1.Z;
			float w1 = value1.W;
			float x2 = value2.X;
			float y2 = value2.Y;
			float z2 = value2.Z;
			float w2 = value2.W;
			float num1 = (float) ((double) y1 * (double) z2 - (double) z1 * (double) y2);
			float num2 = (float) ((double) z1 * (double) x2 - (double) x1 * (double) z2);
			float num3 = (float) ((double) x1 * (double) y2 - (double) y1 * (double) x2);
			float num4 = (float) ((double) x1 * (double) x2 + (double) y1 * (double) y2 + (double) z1 * (double) z2);
			Quaternion quaternion;
			quaternion.X = (float) ((double) x1 * (double) w2 + (double) x2 * (double) w1) + num1;
			quaternion.Y = (float) ((double) y1 * (double) w2 + (double) y2 * (double) w1) + num2;
			quaternion.Z = (float) ((double) z1 * (double) w2 + (double) z2 * (double) w1) + num3;
			quaternion.W = w1 * w2 - num4;
			return quaternion;
		}

		/// <summary>Multiplies a Quaternion by a scalar value.</summary>
		/// <param name="value1">The source Quaternion.</param>
		/// <param name="value2">The scalar value.</param>
		/// <returns>The result of the multiplication.</returns>
		public static Quaternion Multiply(Quaternion value1, float value2) {
			Quaternion quaternion;
			quaternion.X = value1.X * value2;
			quaternion.Y = value1.Y * value2;
			quaternion.Z = value1.Z * value2;
			quaternion.W = value1.W * value2;
			return quaternion;
		}

		/// <summary>Divides a Quaternion by another Quaternion.</summary>
		/// <param name="value1">The source Quaternion.</param>
		/// <param name="value2">The divisor.</param>
		/// <returns>The result of the division.</returns>
		public static Quaternion Divide(Quaternion value1, Quaternion value2) {
			float x = value1.X;
			float y = value1.Y;
			float z = value1.Z;
			float w = value1.W;
			float num1 = 1f / (float) ((double) value2.X * (double) value2.X + (double) value2.Y * (double) value2.Y + (double) value2.Z * (double) value2.Z + (double) value2.W * (double) value2.W);
			float num2 = -value2.X * num1;
			float num3 = -value2.Y * num1;
			float num4 = -value2.Z * num1;
			float num5 = value2.W * num1;
			float num6 = (float) ((double) y * (double) num4 - (double) z * (double) num3);
			float num7 = (float) ((double) z * (double) num2 - (double) x * (double) num4);
			float num8 = (float) ((double) x * (double) num3 - (double) y * (double) num2);
			float num9 = (float) ((double) x * (double) num2 + (double) y * (double) num3 + (double) z * (double) num4);
			Quaternion quaternion;
			quaternion.X = (float) ((double) x * (double) num5 + (double) num2 * (double) w) + num6;
			quaternion.Y = (float) ((double) y * (double) num5 + (double) num3 * (double) w) + num7;
			quaternion.Z = (float) ((double) z * (double) num5 + (double) num4 * (double) w) + num8;
			quaternion.W = w * num5 - num9;
			return quaternion;
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Quaternion is equal to this Quaternion instance.
		/// </summary>
		/// <param name="other">The Quaternion to compare this instance to.</param>
		/// <returns>True if the other Quaternion is equal to this instance; False otherwise.</returns>
		public bool Equals(Quaternion other) {
			if ((double) this.X == (double) other.X && (double) this.Y == (double) other.Y && (double) this.Z == (double) other.Z)
				return (double) this.W == (double) other.W;
			return false;
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Object is equal to this Quaternion instance.
		/// </summary>
		/// <param name="obj">The Object to compare against.</param>
		/// <returns>True if the Object is equal to this Quaternion; False otherwise.</returns>
		public override bool Equals(object obj) {
			if (obj is Quaternion)
				return this.Equals((Quaternion) obj);
			return false;
		}

		/// <summary>
		/// Returns a String representing this Quaternion instance.
		/// </summary>
		/// <returns>The string representation.</returns>
		public override string ToString() {
			CultureInfo currentCulture = CultureInfo.CurrentCulture;
			return string.Format((IFormatProvider) currentCulture, "{{X:{0} Y:{1} Z:{2} W:{3}}}", (object) this.X.ToString((IFormatProvider) currentCulture), (object) this.Y.ToString((IFormatProvider) currentCulture), (object) this.Z.ToString((IFormatProvider) currentCulture), (object) this.W.ToString((IFormatProvider) currentCulture));
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>The hash code.</returns>
		public override int GetHashCode() {
			return this.X.GetHashCode() + this.Y.GetHashCode() + this.Z.GetHashCode() + this.W.GetHashCode();
		}
	}
}