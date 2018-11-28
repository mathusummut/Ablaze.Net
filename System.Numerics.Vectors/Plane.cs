using System.Globalization;
using System.Runtime.CompilerServices;

namespace System.Numerics {
	/// <summary>A structure encapsulating a 3D Plane</summary>
	[Serializable]
	public struct Plane : IEquatable<Plane> {
		/// <summary>The normal vector of the Plane.</summary>
		public Vector3 Normal;
		/// <summary>
		/// The distance of the Plane along its normal from the origin.
		/// </summary>
		public float D;

		/// <summary>
		/// Constructs a Plane from the X, Y, and Z components of its normal, and its distance from the origin on that normal.
		/// </summary>
		/// <param name="x">The X-component of the normal.</param>
		/// <param name="y">The Y-component of the normal.</param>
		/// <param name="z">The Z-component of the normal.</param>
		/// <param name="d">The distance of the Plane along its normal from the origin.</param>
		public Plane(float x, float y, float z, float d) {
			this.Normal = new Vector3(x, y, z);
			this.D = d;
		}

		/// <summary>
		/// Constructs a Plane from the given normal and distance along the normal from the origin.
		/// </summary>
		/// <param name="normal">The Plane's normal vector.</param>
		/// <param name="d">The Plane's distance from the origin along its normal vector.</param>
		public Plane(Vector3 normal, float d) {
			this.Normal = normal;
			this.D = d;
		}

		/// <summary>Constructs a Plane from the given Vector4.</summary>
		/// <param name="value">A vector whose first 3 elements describe the normal vector,
		/// and whose W component defines the distance along that normal from the origin.</param>
		public Plane(Vector4 value) {
			this.Normal = new Vector3(value.X, value.Y, value.Z);
			this.D = value.W;
		}

		/// <summary>
		/// Returns a boolean indicating whether the two given Planes are equal.
		/// </summary>
		/// <param name="value1">The first Plane to compare.</param>
		/// <param name="value2">The second Plane to compare.</param>
		/// <returns>True if the Planes are equal; False otherwise.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator ==(Plane value1, Plane value2) {
			if ((double) value1.Normal.X == (double) value2.Normal.X && (double) value1.Normal.Y == (double) value2.Normal.Y && (double) value1.Normal.Z == (double) value2.Normal.Z)
				return (double) value1.D == (double) value2.D;
			return false;
		}

		/// <summary>
		/// Returns a boolean indicating whether the two given Planes are not equal.
		/// </summary>
		/// <param name="value1">The first Plane to compare.</param>
		/// <param name="value2">The second Plane to compare.</param>
		/// <returns>True if the Planes are not equal; False if they are equal.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator !=(Plane value1, Plane value2) {
			if ((double) value1.Normal.X == (double) value2.Normal.X && (double) value1.Normal.Y == (double) value2.Normal.Y && (double) value1.Normal.Z == (double) value2.Normal.Z)
				return (double) value1.D != (double) value2.D;
			return true;
		}

		/// <summary>Creates a Plane that contains the three given points.</summary>
		/// <param name="point1">The first point defining the Plane.</param>
		/// <param name="point2">The second point defining the Plane.</param>
		/// <param name="point3">The third point defining the Plane.</param>
		/// <returns>The Plane containing the three points.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Plane CreateFromVertices(Vector3 point1, Vector3 point2, Vector3 point3) {
			if (Vector.IsHardwareAccelerated) {
				Vector3 vector3 = Vector3.Normalize(Vector3.Cross(point2 - point1, point3 - point1));
				float d = -Vector3.Dot(vector3, point1);
				return new Plane(vector3, d);
			}
			float num1 = point2.X - point1.X;
			float num2 = point2.Y - point1.Y;
			float num3 = point2.Z - point1.Z;
			float num4 = point3.X - point1.X;
			float num5 = point3.Y - point1.Y;
			float num6 = point3.Z - point1.Z;
			float num7 = (float) ((double) num2 * (double) num6 - (double) num3 * (double) num5);
			float num8 = (float) ((double) num3 * (double) num4 - (double) num1 * (double) num6);
			float num9 = (float) ((double) num1 * (double) num5 - (double) num2 * (double) num4);
			float num10 = 1f / (float) Math.Sqrt((double) num7 * (double) num7 + (double) num8 * (double) num8 + (double) num9 * (double) num9);
			Vector3 normal = new Vector3(num7 * num10, num8 * num10, num9 * num10);
			return new Plane(normal, (float) -((double) normal.X * (double) point1.X + (double) normal.Y * (double) point1.Y + (double) normal.Z * (double) point1.Z));
		}

		/// <summary>
		/// Creates a new Plane whose normal vector is the source Plane's normal vector normalized.
		/// </summary>
		/// <param name="value">The source Plane.</param>
		/// <returns>The normalized Plane.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Plane Normalize(Plane value) {
			if (Vector.IsHardwareAccelerated) {
				float num1 = value.Normal.LengthSquared();
				if ((double) Math.Abs(num1 - 1f) < 1.19209289550781E-07)
					return value;
				float num2 = (float) Math.Sqrt((double) num1);
				return new Plane(value.Normal / num2, value.D / num2);
			}
			float num3 = (float) ((double) value.Normal.X * (double) value.Normal.X + (double) value.Normal.Y * (double) value.Normal.Y + (double) value.Normal.Z * (double) value.Normal.Z);
			if ((double) Math.Abs(num3 - 1f) < 1.19209289550781E-07)
				return value;
			float num4 = 1f / (float) Math.Sqrt((double) num3);
			return new Plane(value.Normal.X * num4, value.Normal.Y * num4, value.Normal.Z * num4, value.D * num4);
		}

		/// <summary>
		///  Transforms a normalized Plane by a Quaternion rotation.
		/// </summary>
		/// <param name="plane"> The normalized Plane to transform.
		/// This Plane must already be normalized, so that its Normal vector is of unit length, before this method is called.</param>
		/// <param name="rotation">The Quaternion rotation to apply to the Plane.</param>
		/// <returns>A new Plane that results from applying the rotation.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Plane Transform(Plane plane, Quaternion rotation) {
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
			float num13 = 1f - num10 - num12;
			float num14 = num8 - num6;
			float num15 = num9 + num5;
			float num16 = num8 + num6;
			float num17 = 1f - num7 - num12;
			float num18 = num11 - num4;
			float num19 = num9 - num5;
			float num20 = num11 + num4;
			float num21 = 1f - num7 - num10;
			float x = plane.Normal.X;
			float y = plane.Normal.Y;
			float z = plane.Normal.Z;
			return new Plane((float) ((double) x * (double) num13 + (double) y * (double) num14 + (double) z * (double) num15), (float) ((double) x * (double) num16 + (double) y * (double) num17 + (double) z * (double) num18), (float) ((double) x * (double) num19 + (double) y * (double) num20 + (double) z * (double) num21), plane.D);
		}

		/// <summary>Calculates the dot product of a Plane and Vector4.</summary>
		/// <param name="plane">The Plane.</param>
		/// <param name="value">The Vector4.</param>
		/// <returns>The dot product.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static float Dot(Plane plane, Vector4 value) {
			return (float) ((double) plane.Normal.X * (double) value.X + (double) plane.Normal.Y * (double) value.Y + (double) plane.Normal.Z * (double) value.Z + (double) plane.D * (double) value.W);
		}

		/// <summary>
		/// Returns the dot product of a specified Vector3 and the normal vector of this Plane plus the distance (D) value of the Plane.
		/// </summary>
		/// <param name="plane">The plane.</param>
		/// <param name="value">The Vector3.</param>
		/// <returns>The resulting value.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static float DotCoordinate(Plane plane, Vector3 value) {
			if (Vector.IsHardwareAccelerated)
				return Vector3.Dot(plane.Normal, value) + plane.D;
			return (float) ((double) plane.Normal.X * (double) value.X + (double) plane.Normal.Y * (double) value.Y + (double) plane.Normal.Z * (double) value.Z) + plane.D;
		}

		/// <summary>
		/// Returns the dot product of a specified Vector3 and the Normal vector of this Plane.
		/// </summary>
		/// <param name="plane">The plane.</param>
		/// <param name="value">The Vector3.</param>
		/// <returns>The resulting dot product.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static float DotNormal(Plane plane, Vector3 value) {
			if (Vector.IsHardwareAccelerated)
				return Vector3.Dot(plane.Normal, value);
			return (float) ((double) plane.Normal.X * (double) value.X + (double) plane.Normal.Y * (double) value.Y + (double) plane.Normal.Z * (double) value.Z);
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Plane is equal to this Plane instance.
		/// </summary>
		/// <param name="other">The Plane to compare this instance to.</param>
		/// <returns>True if the other Plane is equal to this instance; False otherwise.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool Equals(Plane other) {
			if (Vector.IsHardwareAccelerated) {
				if (this.Normal.Equals(other.Normal))
					return (double) this.D == (double) other.D;
				return false;
			}
			if ((double) this.Normal.X == (double) other.Normal.X && (double) this.Normal.Y == (double) other.Normal.Y && (double) this.Normal.Z == (double) other.Normal.Z)
				return (double) this.D == (double) other.D;
			return false;
		}

		/// <summary>
		/// Returns a boolean indicating whether the given Object is equal to this Plane instance.
		/// </summary>
		/// <param name="obj">The Object to compare against.</param>
		/// <returns>True if the Object is equal to this Plane; False otherwise.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public override bool Equals(object obj) {
			if (obj is Plane)
				return this.Equals((Plane) obj);
			return false;
		}

		/// <summary>Returns a String representing this Plane instance.</summary>
		/// <returns>The string representation.</returns>
		public override string ToString() {
			CultureInfo currentCulture = CultureInfo.CurrentCulture;
			return string.Format((IFormatProvider) currentCulture, "{{Normal:{0} D:{1}}}", new object[2]
			{
		(object) this.Normal.ToString(),
		(object) this.D.ToString((IFormatProvider) currentCulture)
			});
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>The hash code.</returns>
		public override int GetHashCode() {
			return this.Normal.GetHashCode() + this.D.GetHashCode();
		}
	}
}