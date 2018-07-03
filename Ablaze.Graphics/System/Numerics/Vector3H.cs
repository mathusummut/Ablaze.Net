using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Numerics {
	/// <summary>
	/// 3-component Vector of the Half type. Occupies 6 Byte total.
	/// </summary>
	[Serializable, StructLayout(LayoutKind.Sequential)]
	public struct Vector3H : ISerializable, IEquatable<Vector3H> {
		/// <summary>
		/// The size in bytes for an instance of the Vector3H struct is 6.
		/// </summary>
		public const int SizeInBytes = 6;

		/// <summary>
		/// A vector with X, Y and Z all 0.
		/// </summary>
		public static readonly Vector3H Zero = new Vector3H();
		/// <summary>
		/// A vector with X=1, Y=0 and Z=0.
		/// </summary>
		public static readonly Vector3H UnitX = new Vector3H(Half.One, Half.Zero, Half.Zero);
		/// <summary>
		/// A vector with X=0, Y=1 and Z=0.
		/// </summary>
		public static readonly Vector3H UnitY = new Vector3H(Half.Zero, Half.One, Half.Zero);
		/// <summary>
		/// A vector with X=0, Y=0 and Z=1.
		/// </summary>
		public static readonly Vector3H UnitZ = new Vector3H(Half.Zero, Half.Zero, Half.One);
		/// <summary>
		/// A vector with X, Y and Z all 1.
		/// </summary>
		public static readonly Vector3H One = new Vector3H(Half.One, Half.One, Half.One);

		/// <summary>The X component of the Vector3H.</summary>
		public Half X;

		/// <summary>The Y component of the Vector3H.</summary>
		public Half Y;

		/// <summary>The Z component of the Vector3H.</summary>
		public Half Z;

		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		/// <param name="value">The value that will initialize this instance.</param>
		public Vector3H(Half value) {
			X = value;
			Y = value;
			Z = value;
		}

		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		/// <param name="value">The value that will initialize this instance.</param>
		public Vector3H(float value) {
			X = new Half(value);
			Y = new Half(value);
			Z = new Half(value);
		}

		/// <summary>
		/// The new Vector3H instance will avoid conversion and copy directly from the Half parameters.
		/// </summary>
		/// <param name="x">An Half instance of a 16-bit half-precision floating-point number.</param>
		/// <param name="y">An Half instance of a 16-bit half-precision floating-point number.</param>
		/// <param name="z">An Half instance of a 16-bit half-precision floating-point number.</param>
		public Vector3H(Half x, Half y, Half z) {
			X = x;
			Y = y;
			Z = z;
		}

		/// <summary>
		/// The new Vector3H instance will convert the 3 parameters into 16-bit half-precision floating-point.
		/// </summary>
		/// <param name="x">32-bit single-precision floating-point number.</param>
		/// <param name="y">32-bit single-precision floating-point number.</param>
		/// <param name="z">32-bit single-precision floating-point number.</param>
		public Vector3H(float x, float y, float z) {
			X = new Half(x);
			Y = new Half(y);
			Z = new Half(z);
		}

		/// <summary>
		/// The new Vector3H instance will convert the Vector3 into 16-bit half-precision floating-point.
		/// </summary>
		/// <param name="v">System.Vector3</param>
		[CLSCompliant(false)]
		public Vector3H(Vector3 v) {
			X = new Half(v.X);
			Y = new Half(v.Y);
			Z = new Half(v.Z);
		}

		/// <summary>
		/// The new Vector3H instance will convert the Vector3 into 16-bit half-precision floating-point.
		/// This is the fastest constructor.
		/// </summary>
		/// <param name="v">System.Vector3</param>
		public Vector3H(ref Vector3 v) {
			X = new Half(v.X);
			Y = new Half(v.Y);
			Z = new Half(v.Z);
		}

		/// <summary>
		/// Gets or sets an System.Vector2h with the X and Y components of this instance.
		/// </summary>
		public Vector2H Xy {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return new Vector2H(X, Y);
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				X = value.X;
				Y = value.Y;
			}
		}

		/// <summary>Converts System.Vector3 to Vector3H.</summary>
		/// <param name="v3f">The Vector3 to convert.</param>
		/// <returns>The resulting Half vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static explicit operator Vector3H(Vector3 v3f) {
			return new Vector3H(v3f);
		}

		/// <summary>Converts Vector3H to System.Vector3.</summary>
		/// <param name="h3">The Vector3H to convert.</param>
		/// <returns>The resulting Vector3.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static explicit operator Vector3(Vector3H h3) {
			return new Vector3(h3.X.ToSingle(), h3.Y.ToSingle(), h3.Z.ToSingle());
		}

		/// <summary>Constructor used by ISerializable to deserialize the object.</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		private Vector3H(SerializationInfo info, StreamingContext context) {
			X = (Half) info.GetValue(nameof(X), Half.Type);
			Y = (Half) info.GetValue(nameof(Y), Half.Type);
			Z = (Half) info.GetValue(nameof(Z), Half.Type);
		}

		/// <summary>Used by ISerialize to serialize the object.</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public void GetObjectData(SerializationInfo info, StreamingContext context) {
			info.AddValue(nameof(X), X);
			info.AddValue(nameof(Y), Y);
			info.AddValue(nameof(Z), Z);
		}

		/// <summary>Updates the X,Y and Z components of this instance by reading from a Stream.</summary>
		/// <param name="bin">A BinaryReader instance associated with an open Stream.</param>
		public void FromBinaryStream(BinaryReader bin) {
			X.FromBinaryStream(bin);
			Y.FromBinaryStream(bin);
			Z.FromBinaryStream(bin);
		}

		/// <summary>Writes the X,Y and Z components of this instance into a Stream.</summary>
		/// <param name="bin">A BinaryWriter instance associated with an open Stream.</param>
		public void ToBinaryStream(BinaryWriter bin) {
			X.ToBinaryStream(bin);
			Y.ToBinaryStream(bin);
			Z.ToBinaryStream(bin);
		}

		/// <summary>
		/// Compares the two vectors for equality.
		/// </summary>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator ==(Vector3H left, Vector3H right) {
			return left.Equals(right);
		}

		/// <summary>
		/// Compares the two vectors for inequality.
		/// </summary>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator !=(Vector3H left, Vector3H right) {
			return !left.Equals(right);
		}

		/// <summary>
		/// Does nothing lol
		/// </summary>
		/// <param name="value">wanna hear a joke?</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector3H operator +(Vector3H value) {
			return value;
		}

		/// <summary>
		/// Negates the components of the specified vector.
		/// </summary>
		/// <param name="value">The vector to negate.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector3H operator -(Vector3H value) {
			return new Vector3H(-value.X, -value.Y, -value.Z);
		}

		/// <summary>
		/// Adds the two vectors.
		/// </summary>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector3H operator +(Vector3H left, Vector3H right) {
			return new Vector3H(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
		}

		/// <summary>
		/// Subtracts the second vector from the first vector.
		/// </summary>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector3H operator -(Vector3H left, Vector3H right) {
			return new Vector3H(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
		}

		/// <summary>
		/// Component-wise multiplies the two vectors.
		/// </summary>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector3H operator *(Vector3H left, Vector3H right) {
			return new Vector3H(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
		}

		/// <summary>
		/// Component-wise divides the first vector by the second vector.
		/// </summary>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector3H operator /(Vector3H left, Vector3H right) {
			return new Vector3H(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
		}

		/// <summary>Returns a value indicating whether this instance is equal to a specified Vector3H vector.</summary>
		/// <param name="other">Vector3H to compare to this instance..</param>
		/// <returns>True, if other is equal to this instance; false otherwise.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool Equals(Vector3H other) {
			return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
		}

		/// <summary>
		/// Returns whether the specified object is equal to this vector.
		/// </summary>
		/// <param name="obj">The instance to compare to this instance.</param>
		public override bool Equals(object obj) {
			return obj is Vector3H ? Equals((Vector3H) obj) : false;
		}

		/// <summary>
		/// Gets the hash code for this instance.
		/// </summary>
		public override int GetHashCode() {
			return unchecked((X.GetHashCode() << 17) ^ (Y.GetHashCode() << 11) ^ Z.GetHashCode());
		}

		/// <summary>Returns a string that contains this Vector3H's numbers in human-legible form.</summary>
		public override string ToString() {
			return string.Format("({0}, {1}, {2})", X.ToString(), Y.ToString(), Z.ToString());
		}

		/// <summary>Returns the Vector3H as an array of bytes.</summary>
		/// <param name="h">The Vector3H to convert.</param>
		/// <returns>The input as byte array.</returns>
		public static byte[] GetBytes(Vector3H h) {
			byte[] result = new byte[SizeInBytes];

			byte[] temp = Half.GetBytes(h.X);
			result[0] = temp[0];
			result[1] = temp[1];
			temp = Half.GetBytes(h.Y);
			result[2] = temp[0];
			result[3] = temp[1];
			temp = Half.GetBytes(h.Z);
			result[4] = temp[0];
			result[5] = temp[1];

			return result;
		}

		/// <summary>Converts an array of bytes into Vector3H.</summary>
		/// <param name="value">A Vector3H in it's byte[] representation.</param>
		/// <param name="startIndex">The starting position within value.</param>
		/// <returns>A new Vector3H instance.</returns>
		public static Vector3H FromBytes(byte[] value, int startIndex) {
			return new Vector3H(Half.FromBytes(value, startIndex), Half.FromBytes(value, startIndex + 2), Half.FromBytes(value, startIndex + 4));
		}
	}
}