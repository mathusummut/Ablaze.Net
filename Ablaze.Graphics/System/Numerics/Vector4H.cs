using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Numerics {
	/// <summary>
	/// 4-component Vector of the Half type. Occupies 8 Byte total.
	/// </summary>
	[Serializable, StructLayout(LayoutKind.Sequential)]
	public struct Vector4H : ISerializable, IEquatable<Vector4H> {
		/// <summary>
		/// The size in bytes for an instance of the Vector4H struct is 8.
		/// </summary>
		public const int SizeInBytes = 8;

		/// <summary>
		/// A vector with X, Y, Z and W all 0.
		/// </summary>
		public static readonly Vector4H Zero = new Vector4H();
		/// <summary>
		/// A vector with X=1, Y=0, Z=0 and W=0.
		/// </summary>
		public static readonly Vector4H UnitX = new Vector4H(Half.One, Half.Zero, Half.Zero, Half.Zero);
		/// <summary>
		/// A vector with X=0, Y=1, Z=0 and W=0.
		/// </summary>
		public static readonly Vector4H UnitY = new Vector4H(Half.Zero, Half.One, Half.Zero, Half.Zero);
		/// <summary>
		/// A vector with X=0, Y=0, Z=1 and W=0.
		/// </summary>
		public static readonly Vector4H UnitZ = new Vector4H(Half.Zero, Half.Zero, Half.One, Half.Zero);
		/// <summary>
		/// A vector with X=0, Y=0, Z=0 and W=1.
		/// </summary>
		public static readonly Vector4H UnitW = new Vector4H(Half.Zero, Half.Zero, Half.Zero, Half.One);
		/// <summary>
		/// A vector with X, Y, Z and W all 1.
		/// </summary>
		public static readonly Vector4H One = new Vector4H(Half.One, Half.One, Half.One, Half.One);

		/// <summary>The X component of the Vector4H.</summary>
		public Half X;

		/// <summary>The Y component of the Vector4H.</summary>
		public Half Y;

		/// <summary>The Z component of the Vector4H.</summary>
		public Half Z;

		/// <summary>The W component of the Vector4H.</summary>
		public Half W;

		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		/// <param name="value">The value that will initialize this instance.</param>
		public Vector4H(Half value) {
			X = value;
			Y = value;
			Z = value;
			W = value;
		}

		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		/// <param name="value">The value that will initialize this instance.</param>
		public Vector4H(float value) {
			X = new Half(value);
			Y = new Half(value);
			Z = new Half(value);
			W = new Half(value);
		}

		/// <summary>
		/// The new Vector4H instance will avoid conversion and copy directly from the Half parameters.
		/// </summary>
		/// <param name="x">An Half instance of a 16-bit half-precision floating-point number.</param>
		/// <param name="y">An Half instance of a 16-bit half-precision floating-point number.</param>
		/// <param name="z">An Half instance of a 16-bit half-precision floating-point number.</param>
		/// <param name="w">An Half instance of a 16-bit half-precision floating-point number.</param>
		public Vector4H(Half x, Half y, Half z, Half w) {
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		/// <summary>
		/// The new Vector4H instance will convert the 4 parameters into 16-bit half-precision floating-point.
		/// </summary>
		/// <param name="x">32-bit single-precision floating-point number.</param>
		/// <param name="y">32-bit single-precision floating-point number.</param>
		/// <param name="z">32-bit single-precision floating-point number.</param>
		/// <param name="w">32-bit single-precision floating-point number.</param>
		public Vector4H(float x, float y, float z, float w) {
			X = new Half(x);
			Y = new Half(y);
			Z = new Half(z);
			W = new Half(w);
		}

		/// <summary>
		/// The new Vector4H instance will convert the Vector4 into 16-bit half-precision floating-point.
		/// </summary>
		/// <param name="v">System.Vector4</param>
		[CLSCompliant(false)]
		public Vector4H(Vector4 v) {
			X = new Half(v.X);
			Y = new Half(v.Y);
			Z = new Half(v.Z);
			W = new Half(v.W);
		}

		/// <summary>
		/// The new Vector4H instance will convert the Vector4 into 16-bit half-precision floating-point.
		/// This is the fastest constructor.
		/// </summary>
		/// <param name="v">System.Vector4</param>
		public Vector4H(ref Vector4 v) {
			X = new Half(v.X);
			Y = new Half(v.Y);
			Z = new Half(v.Z);
			W = new Half(v.W);
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

		/// <summary>
		/// Gets or sets an System.Vector3h with the X, Y and Z components of this instance.
		/// </summary>
		public Vector3H Xyz {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return new Vector3H(X, Y, Z);
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				X = value.X;
				Y = value.Y;
				Z = value.Z;
			}
		}

		/// <summary>Converts System.Vector4 to Vector4H.</summary>
		/// <param name="v4f">The Vector4 to convert.</param>
		/// <returns>The resulting Half vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static explicit operator Vector4H(Vector4 v4f) {
			return new Vector4H(v4f);
		}

		/// <summary>Converts Vector4H to System.Vector4.</summary>
		/// <param name="h4">The Vector4H to convert.</param>
		/// <returns>The resulting Vector4.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static explicit operator Vector4(Vector4H h4) {
			return new Vector4(h4.X.ToSingle(), h4.Y.ToSingle(), h4.Z.ToSingle(), h4.W.ToSingle());
		}

		/// <summary>Constructor used by ISerializable to deserialize the object.</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		private Vector4H(SerializationInfo info, StreamingContext context) {
			X = (Half) info.GetValue(nameof(X), Half.Type);
			Y = (Half) info.GetValue(nameof(Y), Half.Type);
			Z = (Half) info.GetValue(nameof(Z), Half.Type);
			W = (Half) info.GetValue(nameof(W), Half.Type);
		}

		/// <summary>Used by ISerialize to serialize the object.</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public void GetObjectData(SerializationInfo info, StreamingContext context) {
			info.AddValue(nameof(X), X);
			info.AddValue(nameof(Y), Y);
			info.AddValue(nameof(Z), Z);
			info.AddValue(nameof(W), W);
		}

		/// <summary>Updates the X,Y,Z and W components of this instance by reading from a Stream.</summary>
		/// <param name="bin">A BinaryReader instance associated with an open Stream.</param>
		public void FromBinaryStream(BinaryReader bin) {
			X.FromBinaryStream(bin);
			Y.FromBinaryStream(bin);
			Z.FromBinaryStream(bin);
			W.FromBinaryStream(bin);
		}

		/// <summary>Writes the X,Y,Z and W components of this instance into a Stream.</summary>
		/// <param name="bin">A BinaryWriter instance associated with an open Stream.</param>
		public void ToBinaryStream(BinaryWriter bin) {
			X.ToBinaryStream(bin);
			Y.ToBinaryStream(bin);
			Z.ToBinaryStream(bin);
			W.ToBinaryStream(bin);
		}

		/// <summary>
		/// Compares the two vectors for equality.
		/// </summary>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator ==(Vector4H left, Vector4H right) {
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
		public static bool operator !=(Vector4H left, Vector4H right) {
			return !left.Equals(right);
		}

		/// <summary>
		/// Does nothing lol
		/// </summary>
		/// <param name="value">wanna hear a joke?</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector4H operator +(Vector4H value) {
			return value;
		}

		/// <summary>
		/// Negates the components of the specified vector.
		/// </summary>
		/// <param name="value">The vector to negate.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector4H operator -(Vector4H value) {
			return new Vector4H(-value.X, -value.Y, -value.Z, -value.W);
		}

		/// <summary>
		/// Adds the two vectors.
		/// </summary>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector4H operator +(Vector4H left, Vector4H right) {
			return new Vector4H(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
		}

		/// <summary>
		/// Subtracts the second vector from the first vector.
		/// </summary>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector4H operator -(Vector4H left, Vector4H right) {
			return new Vector4H(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
		}

		/// <summary>
		/// Component-wise multiplies the two vectors.
		/// </summary>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector4H operator *(Vector4H left, Vector4H right) {
			return new Vector4H(left.X * right.X, left.Y * right.Y, left.Z * right.Z, left.W * right.W);
		}

		/// <summary>
		/// Component-wise divides the first vector by the second vector.
		/// </summary>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector4H operator /(Vector4H left, Vector4H right) {
			return new Vector4H(left.X / right.X, left.Y / right.Y, left.Z / right.Z, left.W / right.W);
		}

		/// <summary>
		/// Returns whether the specified object is equal to this vector.
		/// </summary>
		/// <param name="obj">The instance to compare to this instance.</param>
		public override bool Equals(object obj) {
			return obj is Vector4H ? Equals((Vector4H) obj) : false;
		}

		/// <summary>
		/// Gets the hash code for this instance.
		/// </summary>
		public override int GetHashCode() {
			return unchecked((X.GetHashCode() << 17) ^ (Y.GetHashCode() << 13) ^ (Z.GetHashCode() << 9) ^ W.GetHashCode());
		}

		/// <summary>Returns a value indicating whether this instance is equal to a specified Vector4H vector.</summary>
		/// <param name="other">Vector4H to compare to this instance..</param>
		/// <returns>True, if other is equal to this instance; false otherwise.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool Equals(Vector4H other) {
			return (X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z) && W.Equals(other.W));
		}

		/// <summary>Returns a string that contains this Vector4H's numbers in human-legible form.</summary>
		public override string ToString() {
			return string.Format("({0}, {1}, {2}, {3})", X.ToString(), Y.ToString(), Z.ToString(), W.ToString());
		}

		/// <summary>Returns the Vector4H as an array of bytes.</summary>
		/// <param name="h">The Vector4H to convert.</param>
		/// <returns>The input as byte array.</returns>
		public static byte[] GetBytes(Vector4H h) {
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
			temp = Half.GetBytes(h.W);
			result[6] = temp[0];
			result[7] = temp[1];

			return result;
		}

		/// <summary>Converts an array of bytes into Vector4H.</summary>
		/// <param name="value">A Vector4H in it's byte[] representation.</param>
		/// <param name="startIndex">The starting position within value.</param>
		/// <returns>A new Vector4H instance.</returns>
		public static Vector4H FromBytes(byte[] value, int startIndex) {
			return new Vector4H(Half.FromBytes(value, startIndex), Half.FromBytes(value, startIndex + 2), Half.FromBytes(value, startIndex + 4), Half.FromBytes(value, startIndex + 6));
		}
	}
}