using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Graphics.Models {
	/// <summary>
	/// Holds the indices of the data for a 3D vertex
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct VertexIndex : IEquatable<VertexIndex> {
		/// <summary>
		/// The index of the coordinates of the vector
		/// </summary>
		public int Pos;
		/// <summary>
		/// The index of the coordinates of the corresponding texture location (for UV wrapping)
		/// </summary>
		public int TexPos;
		/// <summary>
		/// The index of the attributes of the normal
		/// </summary>
		public int Normal;

		/// <summary>
		/// Creates a new vertex data holder
		/// </summary>
		/// <param name="position">The index of the coordinates of the vector</param>
		public VertexIndex(int position) {
			Pos = position;
			TexPos = 0;
			Normal = 0;
		}

		/// <summary>
		/// Creates a new vertex data holder
		/// </summary>
		/// <param name="position">The index of the coordinates of the vector</param>
		/// <param name="texPos">The index of the coordinates of the corresponding texture location (for UV wrapping)</param>
		public VertexIndex(int position, int texPos) {
			Pos = position;
			TexPos = texPos;
			Normal = 0;
		}

		/// <summary>
		/// Creates a new vertex data holder
		/// </summary>
		/// <param name="position">The index of the coordinates of the vector</param>
		/// <param name="texPos">The index of the coordinates of the corresponding texture location (for UV wrapping)</param>
		/// <param name="normal">The index of the attributes of the normal</param>
		public VertexIndex(int position, int texPos, int normal) {
			Pos = position;
			TexPos = texPos;
			Normal = normal;
		}

		/// <summary>
		/// Returns whether two vertices are equal
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator ==(VertexIndex left, VertexIndex right) {
			return left.Equals(right);
		}

		/// <summary>
		/// Returns whether two vertices are not equal
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator !=(VertexIndex left, VertexIndex right) {
			return !left.Equals(right);
		}

		/// <summary>
		/// Gets a hash code for this instance
		/// </summary>
		public override int GetHashCode() {
			return unchecked((Pos.GetHashCode() << 17) ^ (TexPos.GetHashCode() << 11) ^ Normal.GetHashCode());
		}

		/// <summary>
		/// Returns a string that represents a vertex index
		/// </summary>
		public override string ToString() {
			return "{" + Pos + ", " + TexPos + ", " + Normal + "}";
		}

		/// <summary>
		/// Returns whether the vertices are equal to another
		/// </summary>
		/// <param name="obj">The vertex to check for equality to</param>
		/// <returns>Whether the vertex is equal to another</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool Equals(VertexIndex obj) {
			return Pos == obj.Pos && TexPos == obj.TexPos && Normal == obj.Normal;
		}

		/// <summary>
		/// Returns whether the vertex is equal to another
		/// </summary>
		/// <param name="obj">The vertex to check for equality to</param>
		/// <returns>Whether the vertex is equal to another</returns>
		public override bool Equals(object obj) {
			return obj is VertexIndex ? Equals((VertexIndex) obj) : false;
		}
	}

	/// <summary>
	/// Holds the data for a 3D vertex
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct Vertex : IEquatable<Vertex> {
		/// <summary>
		/// The size of a vertex instance
		/// </summary>
		public static readonly int SizeOfVertex = Marshal.SizeOf(typeof(Vertex));
		/// <summary>
		/// The index of the coordinates of the vertex
		/// </summary>
		public Vector3 Pos;
		/// <summary>
		/// The index of the coordinates of the corresponding texture location (for UV wrapping)
		/// </summary>
		public Vector2 TexPos;
		/// <summary>
		/// The index of the attributes of the normal
		/// </summary>
		public Vector3 Normal;

		/// <summary>
		/// Creates a new vertex data holder
		/// </summary>
		/// <param name="location">The index of the coordinates of the vector</param>
		/// <param name="texPos">The index of the coordinates of the corresponding texture location (for UV wrapping)</param>
		public Vertex(Vector3 location, Vector2 texPos) {
			Pos = location;
			TexPos = texPos;
			Normal = Vector3.UnitY;
		}

		/// <summary>
		/// Creates a new vertex data holder
		/// </summary>
		/// <param name="location">The index of the coordinates of the vector</param>
		/// <param name="texPos">The index of the coordinates of the corresponding texture location (for UV wrapping)</param>
		/// <param name="normal">The index of the attributes of the normal</param>
		public Vertex(Vector3 location, Vector2 texPos, Vector3 normal) {
			Pos = location;
			TexPos = texPos;
			Normal = normal;
		}

		/// <summary>
		/// Returns whether two vertices are equal
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator ==(Vertex left, Vertex right) {
			return left.Equals(right);
		}

		/// <summary>
		/// Returns whether two vertices are not equal
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator !=(Vertex left, Vertex right) {
			return !left.Equals(right);
		}

		/// <summary>
		/// Returns whether the vertices are equal to another
		/// </summary>
		/// <param name="obj">The vertex to check for equality to</param>
		/// <returns>Whether the vertex is equal to another</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool Equals(Vertex obj) {
			return Pos == obj.Pos && TexPos == obj.TexPos && Normal == obj.Normal;
		}

		/// <summary>
		/// Returns whether the vertex is equal to another
		/// </summary>
		/// <param name="obj">The vertex to check for equality to</param>
		/// <returns>Whether the vertex is equal to another</returns>
		public override bool Equals(object obj) {
			return obj is Vertex ? Equals((Vertex) obj) : false;
		}

		/// <summary>
		/// Gets a hash code for this instance
		/// </summary>
		public override int GetHashCode() {
			return unchecked((Pos.GetHashCode() << 17) ^ (TexPos.GetHashCode() << 11) ^ Normal.GetHashCode());
		}

		/// <summary>
		/// Gets a string that represents this vertex
		/// </summary>
		public override string ToString() {
			return "{" + Pos.ToString() + ", " + TexPos.ToString() + ", " + Normal.ToString() + "}";
		}
	}

	/// <summary>
	/// Holds the data for a 3D vertex
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct VertexH : IEquatable<VertexH> {
		/// <summary>
		/// The size of a vertex instance
		/// </summary>
		public static readonly int SizeOfVertex = Marshal.SizeOf(typeof(VertexH));
		/// <summary>
		/// The index of the coordinates of the vertex
		/// </summary>
		public Vector3H Pos;
		/// <summary>
		/// The index of the coordinates of the corresponding texture location (for UV wrapping)
		/// </summary>
		public Vector2H TexPos;
		/// <summary>
		/// The index of the attributes of the normal
		/// </summary>
		public Vector3H Normal;

		/// <summary>
		/// Creates a new vertex data holder
		/// </summary>
		/// <param name="location">The index of the coordinates of the vector</param>
		/// <param name="texPos">The index of the coordinates of the corresponding texture location (for UV wrapping)</param>
		public VertexH(Vector3H location, Vector2H texPos) {
			Pos = location;
			TexPos = texPos;
			Normal = Vector3H.UnitY;
		}

		/// <summary>
		/// Creates a new vertex data holder
		/// </summary>
		/// <param name="location">The index of the coordinates of the vector</param>
		/// <param name="texPos">The index of the coordinates of the corresponding texture location (for UV wrapping)</param>
		/// <param name="normal">The index of the attributes of the normal</param>
		public VertexH(Vector3H location, Vector2H texPos, Vector3H normal) {
			Pos = location;
			TexPos = texPos;
			Normal = normal;
		}

		/// <summary>
		/// Returns whether two vertices are equal
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator ==(VertexH left, VertexH right) {
			return left.Equals(right);
		}

		/// <summary>
		/// Returns whether two vertices are not equal
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator !=(VertexH left, VertexH right) {
			return !left.Equals(right);
		}

		/// <summary>
		/// Returns whether the vertices are equal to another
		/// </summary>
		/// <param name="obj">The vertex to check for equality to</param>
		/// <returns>Whether the vertex is equal to another</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool Equals(VertexH obj) {
			return Pos == obj.Pos && TexPos == obj.TexPos && Normal == obj.Normal;
		}

		/// <summary>
		/// Returns whether the vertex is equal to another
		/// </summary>
		/// <param name="obj">The vertex to check for equality to</param>
		/// <returns>Whether the vertex is equal to another</returns>
		public override bool Equals(object obj) {
			return obj is VertexH ? Equals((VertexH) obj) : false;
		}

		/// <summary>
		/// Gets a hash code for this instance
		/// </summary>
		public override int GetHashCode() {
			return unchecked((Pos.GetHashCode() << 17) ^ (TexPos.GetHashCode() << 11) ^ Normal.GetHashCode());
		}

		/// <summary>
		/// Gets a string that represents this vertex
		/// </summary>
		public override string ToString() {
			return "{" + Pos.ToString() + ", " + TexPos.ToString() + ", " + Normal.ToString() + "}";
		}
	}
}