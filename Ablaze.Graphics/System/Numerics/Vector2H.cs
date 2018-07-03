using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Numerics {

	/// <summary>2-component Vector of the Half type. Occupies 4 Byte total.</summary>
	[Serializable, StructLayout(LayoutKind.Sequential)]
	public struct Vector2H : ISerializable, IEquatable<Vector2H> {
		/// <summary>The size in bytes for an instance of the Vector2H struct is 4.</summary>
		public const int SizeInBytes = 4;
		/// <summary>
		/// A vector with X and Y both 0.
		/// </summary>
		public static readonly Vector2H Zero = new Vector2H();
		/// <summary>
		/// A vector with X=1 and Y=0.
		/// </summary>
		public static readonly Vector2H UnitX = new Vector2H(Half.One, Half.Zero);
		/// <summary>
		/// A vector with X=0 and Y=1.
		/// </summary>
		public static readonly Vector2H UnitY = new Vector2H(Half.Zero, Half.One);
		/// <summary>
		/// A vector with X and Y both 1.
		/// </summary>
		public static readonly Vector2H One = new Vector2H(Half.One, Half.One);
		/// <summary>
		/// The X component of the Vector2H.
		/// </summary>
		public Half X;
		/// <summary>
		/// The Y component of the Vector2H.
		/// </summary>
		public Half Y;

		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		/// <param name="value">The value that will initialize this instance.</param>
		public Vector2H(Half value) {
			X = value;
			Y = value;
		}

		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		/// <param name="value">The value that will initialize this instance.</param>
		public Vector2H(float value) {
			X = new Half(value);
			Y = new Half(value);
		}

		/// <summary>
		/// The new Vector2H instance will avoid conversion and copy directly from the Half parameters.
		/// </summary>
		/// <param name="x">An Half instance of a 16-bit half-precision floating-point number.</param>
		/// <param name="y">An Half instance of a 16-bit half-precision floating-point number.</param>
		public Vector2H(Half x, Half y) {
			X = x;
			Y = y;
		}

		/// <summary>
		/// The new Vector2H instance will convert the 2 parameters into 16-bit half-precision floating-point.
		/// </summary>
		/// <param name="x">32-bit single-precision floating-point number.</param>
		/// <param name="y">32-bit single-precision floating-point number.</param>
		public Vector2H(float x, float y) {
			X = new Half(x);
			Y = new Half(y);
		}

		/// <summary>
		/// The new Vector2H instance will convert the Vector2 into 16-bit half-precision floating-point.
		/// </summary>
		/// <param name="v">System.Vector2</param>
		public Vector2H(Vector2 v) {
			X = new Half(v.X);
			Y = new Half(v.Y);
		}

		/// <summary>
		/// Does nothing lol
		/// </summary>
		/// <param name="value">wanna hear a joke?</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2H operator +(Vector2H value) {
			return value;
		}

		/// <summary>
		/// Negates the components of the specified vector.
		/// </summary>
		/// <param name="value">The vector to negate.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2H operator -(Vector2H value) {
			return new Vector2H(-value.X, -value.Y);
		}

		/// <summary>
		/// Compares the two vectors for equality.
		/// </summary>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator ==(Vector2H left, Vector2H right) {
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
		public static bool operator !=(Vector2H left, Vector2H right) {
			return !left.Equals(right);
		}

		/// <summary>
		/// Adds the two vectors.
		/// </summary>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2H operator +(Vector2H left, Vector2H right) {
			return new Vector2H(left.X + right.X, left.Y + right.Y);
		}

		/// <summary>
		/// Subtracts the second vector from the first vector.
		/// </summary>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2H operator -(Vector2H left, Vector2H right) {
			return new Vector2H(left.X - right.X, left.Y - right.Y);
		}

		/// <summary>
		/// Component-wise multiplies the two vectors.
		/// </summary>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2H operator *(Vector2H left, Vector2H right) {
			return new Vector2H(left.X * right.X, left.Y * right.Y);
		}

		/// <summary>
		/// Component-wise divides the first vector by the second vector.
		/// </summary>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2H operator /(Vector2H left, Vector2H right) {
			return new Vector2H(left.X / right.X, left.Y / right.Y);
		}

		/// <summary>Converts System.Vector2 to System.Vector2H.</summary>
		/// <param name="v">The Vector2 to convert.</param>
		/// <returns>The resulting Half vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static explicit operator Vector2H(Vector2 v) {
			return new Vector2H(v);
		}

		/// <summary>Converts System.Vector2H to System.Vector2.</summary>
		/// <param name="h">The Vector2H to convert.</param>
		/// <returns>The resulting Vector2.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static explicit operator Vector2(Vector2H h) {
			return new Vector2(h.X, h.Y);
		}

		/// <summary>Constructor used by ISerializable to deserialize the object.</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		private Vector2H(SerializationInfo info, StreamingContext context) {
			X = (Half) info.GetValue(nameof(X), Half.Type);
			Y = (Half) info.GetValue(nameof(Y), Half.Type);
		}

		/// <summary>Used by ISerialize to serialize the object.</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public void GetObjectData(SerializationInfo info, StreamingContext context) {
			info.AddValue(nameof(X), X);
			info.AddValue(nameof(Y), Y);
		}

		/// <summary>Updates the X and Y components of this instance by reading from a Stream.</summary>
		/// <param name="bin">A BinaryReader instance associated with an open Stream.</param>
		public void FromBinaryStream(BinaryReader bin) {
			X.FromBinaryStream(bin);
			Y.FromBinaryStream(bin);
		}

		/// <summary>Writes the X and Y components of this instance into a Stream.</summary>
		/// <param name="bin">A BinaryWriter instance associated with an open Stream.</param>
		public void ToBinaryStream(BinaryWriter bin) {
			X.ToBinaryStream(bin);
			Y.ToBinaryStream(bin);
		}

		/// <summary>Returns a value indicating whether this instance is equal to a specified Vector2H vector.</summary>
		/// <param name="other">Vector2H to compare to this instance.</param>
		/// <returns>True, if other is equal to this instance; false otherwise.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool Equals(Vector2H other) {
			return (X.Equals(other.X) && Y.Equals(other.Y));
		}

		/// <summary>
		/// Returns whether the specified object is equal to this vector.
		/// </summary>
		/// <param name="obj">The instance to compare to this instance.</param>
		public override bool Equals(object obj) {
			return obj is Vector2H ? Equals((Vector2H) obj) : false;
		}

		/// <summary>
		/// Gets the hash code for this instance.
		/// </summary>
		public override int GetHashCode() {
			return unchecked((X.GetHashCode() << 16) ^ Y.GetHashCode());
		}

		/// <summary>Returns a string that contains this Vector2H's numbers in human-legible form.</summary>
		public override string ToString() {
			return string.Format("({0}, {1})", X.ToString(), Y.ToString());
		}

		/// <summary>Returns the Vector2H as an array of bytes.</summary>
		/// <param name="h">The Vector2H to convert.</param>
		/// <returns>The input as byte array.</returns>
		public static byte[] GetBytes(Vector2H h) {
			byte[] result = new byte[SizeInBytes];
			byte[] temp = Half.GetBytes(h.X);
			result[0] = temp[0];
			result[1] = temp[1];
			temp = Half.GetBytes(h.Y);
			result[2] = temp[0];
			result[3] = temp[1];
			return result;
		}

		/// <summary>Converts an array of bytes into Vector2H.</summary>
		/// <param name="value">A Vector2H in it's byte[] representation.</param>
		/// <param name="startIndex">The starting position within value.</param>
		/// <returns>A new Vector2H instance.</returns>
		public static Vector2H FromBytes(byte[] value, int startIndex) {
			return new Vector2H(Half.FromBytes(value, startIndex), Half.FromBytes(value, startIndex + 2));
		}
	}
}