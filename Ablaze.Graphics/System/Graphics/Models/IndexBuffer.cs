using System.Runtime.CompilerServices;

namespace System.Graphics.Models {
	using OGL;

	/// <summary>
	/// A managed wrapper for an index buffer.
	/// </summary>
	public sealed class IndexBuffer : IEquatable<IndexBuffer>, IDisposable {
		/// <summary>
		/// Whether to keep a managed copy of the indices in the buffer for quick retrieval.
		/// </summary>
		public static bool KeepBuffer;
		private int name, count, references = 1;
		private Array indices, copy;
		private DrawElementsType format;
		/// <summary>
		/// An empty index buffer.
		/// </summary>
		public static readonly IndexBuffer Empty = new IndexBuffer(0, 0, DrawElementsType.UnsignedByte);

		/// <summary>
		/// Gets the native OpenGL name of the buffer.
		/// </summary>
		public int Name {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return name;
			}
		}

		/// <summary>
		/// Gets the number of indices in the buffer.
		/// </summary>
		public int Count {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return count;
			}
		}

		/// <summary>
		/// Whether the index buffer sports a byte, ushort or uint array.
		/// </summary>
		public DrawElementsType Format {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return format;
			}
		}

		/// <summary>
		/// Gets the indices that are represented by this buffer. The returned value can be byte[], ushort[] or uint[].
		/// </summary>
		public Array Indices {
			get {
				if (indices != null)
					return indices;
				else if (copy != null)
					return copy;
				if (count == 0)
					return new byte[0];
				Bind();
				unsafe
				{
					if (format == DrawElementsType.UnsignedByte) {
						byte[] array = new byte[count];
						fixed (byte* ptr = array)
							GL.GetBufferSubData(BufferTarget.ElementArrayBuffer, IntPtr.Zero, (IntPtr) (sizeof(byte) * count), new IntPtr(ptr));
						return array;
					} else if (format == DrawElementsType.UnsignedShort) {
						ushort[] array = new ushort[count];
						fixed (ushort* ptr = array)
							GL.GetBufferSubData(BufferTarget.ElementArrayBuffer, IntPtr.Zero, (IntPtr) (sizeof(ushort) * count), new IntPtr(ptr));
						return array;
					} else {
						uint[] array = new uint[count];
						fixed (uint* ptr = array)
							GL.GetBufferSubData(BufferTarget.ElementArrayBuffer, IntPtr.Zero, (IntPtr) (sizeof(uint) * count), new IntPtr(ptr));
						return array;
					}
				}
			}
		}

		/// <summary>
		/// Gets whether the index buffer is disposed.
		/// </summary>
		public bool IsDisposed {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return count == 0;
			}
		}

		/// <summary>
		/// Creates an index buffer wrapper for an already existing buffer.
		/// </summary>
		/// <param name="name">The index buffer unique name.</param>
		/// <param name="count">The number of indices.</param>
		/// <param name="format">The format of the index buffer.</param>
		public IndexBuffer(int name, int count, DrawElementsType format) {
			this.count = count;
			this.name = name;
			if (name == 0)
				this.format = DrawElementsType.UnsignedByte;
			else
				this.format = format;
		}

		/// <summary>
		/// Generates an index buffer from an array of indices.
		/// </summary>
		/// <param name="array">The array to generate an index buffer from. Has to be byte[], ushort[] or uint[].</param>
		public IndexBuffer(Array array) {
			if (array == null || array.Length == 0)
				return;
			indices = array;
			if (KeepBuffer)
				copy = array;
			count = array.Length;
			if (indices is byte[])
				format = DrawElementsType.UnsignedByte;
			else if (indices is ushort[])
				format = DrawElementsType.UnsignedShort;
			else
				format = DrawElementsType.UnsignedInt;
		}

		/// <summary>
		/// Binds the index buffer for use with OpenGL operations.
		/// </summary>
		public void Bind() {
			if (indices == null)
				GL.BindBuffer(BufferTarget.ElementArrayBuffer, name);
			else {
				if (name == 0 && indices.Length != 0)
					GL.GenBuffers(1, out name);
				GL.BindBuffer(BufferTarget.ElementArrayBuffer, name);
				if (format == DrawElementsType.UnsignedByte)
					GL.BufferData<byte>(BufferTarget.ElementArrayBuffer, new IntPtr(indices.Length * sizeof(byte)), (byte[]) indices, BufferUsageHint.StaticDraw);
				else if (format == DrawElementsType.UnsignedShort)
					GL.BufferData<ushort>(BufferTarget.ElementArrayBuffer, new IntPtr(indices.Length * sizeof(ushort)), (ushort[]) indices, BufferUsageHint.StaticDraw);
				else
					GL.BufferData<uint>(BufferTarget.ElementArrayBuffer, new IntPtr(indices.Length * sizeof(uint)), (uint[]) indices, BufferUsageHint.StaticDraw);
				indices = null;
			}
		}

		/// <summary>
		/// Unbinds the current index buffer.
		/// </summary>
		public static void Unbind() {
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
		}

		/// <summary>
		/// Replaces the indices with the specified array.
		/// </summary>
		/// <param name="indices">The indices to use. Has to be byte[], ushort[] or uint[].</param>
		public void ReplaceIndicesWith(Array indices) {
			if (indices == null)
				indices = new byte[0];
			count = indices.Length;
			this.indices = indices;
			if (indices is byte[])
				format = DrawElementsType.UnsignedByte;
			else if (indices is ushort[])
				format = DrawElementsType.UnsignedShort;
			else
				format = DrawElementsType.UnsignedInt;
		}

		/// <summary>
		/// Adds a reference to this index buffer.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void AddReference() {
			references++;
		}

		/// <summary>
		/// Returns the buffer name.
		/// </summary>
		/// <returns>The buffer name.</returns>
		public override int GetHashCode() {
			return name;
		}

		/// <summary>
		/// Creates a System.String that describes this IndexBuffer.
		/// </summary>
		/// <returns>A System.String that describes this IndexBuffer.</returns>
		public override string ToString() {
			return "Index buffer (handle " + name + ")";
		}

		/// <summary>
		/// Compares whether this IndexBuffer is equal to the specified object.
		/// </summary>
		/// <param name="obj">An object to compare to.</param>
		public override bool Equals(object obj) {
			return Equals(obj as IndexBuffer);
		}

		/// <summary>
		/// Compares whether this IndexBuffer is equal to the specified object.
		/// </summary>
		/// <param name="other">The object to compare to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool Equals(IndexBuffer other) {
			return other == null ? false : (name == other.name);
		}

		/// <summary>
		/// Disposes of the buffer and the resources consumed by it.
		/// </summary>
		~IndexBuffer() {
			if (name == 0 || count == 0)
				return;
			GraphicsContext.RaiseResourceLeakedEvent(this, LeakedWhile.Finalizing, new IntPtr(name));
			name = 0;
			count = 0;
		}

		/// <summary>
		/// Disposes of the buffer and the resources consumed by it.
		/// </summary>
		public void Dispose() {
			Dispose(false);
		}

		/// <summary>
		/// Disposes of the buffer and the resources consumed by it.
		/// </summary>
		/// <param name="forceDispose">If true, the reference count is ignored, forcing the buffer to be disposed, unless it is already disposed.</param>
		public void Dispose(bool forceDispose) {
			if (name == 0 || count == 0)
				return;
			if (references > 0)
				references--;
			if (references <= 0 || forceDispose) {
				try {
					GL.DeleteBuffers(1, ref name);
				} catch {
					GraphicsContext.RaiseResourceLeakedEvent(this, LeakedWhile.Disposing, new IntPtr(name));
				}
				name = 0;
				count = 0;
				indices = null;
				GC.SuppressFinalize(this);
			}
		}
	}
}