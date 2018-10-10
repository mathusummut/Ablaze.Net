using System.Collections.Concurrent;
using System.Graphics.OGL;
using System.Runtime.CompilerServices;

namespace System.Graphics.Models {
	/// <summary>
	/// A managed wrapper for an index buffer
	/// </summary>
	public sealed class IndexBuffer : IEquatable<IndexBuffer>, IDisposable {
		/// <summary>
		/// An empty index buffer
		/// </summary>
		public static readonly IndexBuffer Empty = new IndexBuffer(0, 0, DrawElementsType.UnsignedByte);
		/// <summary>
		/// Whether to keep a copy of the indices in memory even after loading the indices into GPU memory (allows faster and multithreaded index retrieval)
		/// </summary>
		public bool KeepCopyInMemory;
		private static ConcurrentDictionary<Array, IndexBuffer> buffers = new ConcurrentDictionary<Array, IndexBuffer>();
		private int id, count, references = 1;
		private Array indices, copy;
		private DrawElementsType format;

		/// <summary>
		/// Gets the native OpenGL name of the buffer
		/// </summary>
		public int ID {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return id;
			}
		}

		/// <summary>
		/// Gets the number of indices in the buffer
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
		/// Whether the index buffer sports a byte, ushort or uint array
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
		/// Gets the indices that are represented by this buffer. The returned value can be byte[], ushort[] or uint[]
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
		/// Gets whether the index buffer is disposed
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
		/// Creates an index buffer wrapper for an already existing buffer
		/// </summary>
		/// <param name="name">The index buffer unique name</param>
		/// <param name="count">The number of indices</param>
		/// <param name="format">The format of the index buffer</param>
		public IndexBuffer(int name, int count, DrawElementsType format) {
			this.count = count;
			this.id = name;
			if (name == 0)
				this.format = DrawElementsType.UnsignedByte;
			else
				this.format = format;
		}

		/// <summary>
		/// Generates an index buffer from an array of indices
		/// </summary>
		/// <param name="array">The array to generate an index buffer from. Has to be byte[], ushort[] or uint[]</param>
		private IndexBuffer(Array array) {
			if (array == null || array.Length == 0)
				return;
			indices = array;
			buffers.TryAdd(array, this);
			copy = array;
			count = array.Length;
			if (array is byte[])
				format = DrawElementsType.UnsignedByte;
			else if (array is ushort[])
				format = DrawElementsType.UnsignedShort;
			else if (array is uint[])
				format = DrawElementsType.UnsignedInt;
			else
				throw new ArgumentException("The index array must be byte[], ushort[] or uint[]", nameof(array));
		}

		/// <summary>
		/// Generates an index buffer from an array of indices
		/// </summary>
		/// <param name="array">The array to generate an index buffer from. Has to be byte[], ushort[] or uint[]</param>
		public static IndexBuffer FromArray(Array array) {
			if (!(array == null || array.Length == 0)) {
				IndexBuffer buffer;
				if (buffers.TryGetValue(array, out buffer)) {
					buffer.AddReference();
					return buffer;
				}
			}
			return new IndexBuffer(array);
		}

		/// <summary>
		/// Binds the index buffer for use with OpenGL operations
		/// </summary>
		public void Bind() {
			if (indices == null)
				GL.BindBuffer(BufferTarget.ElementArrayBuffer, id);
			else {
				if (id == 0 && indices.Length != 0)
					GL.GenBuffers(1, out id);
				GL.BindBuffer(BufferTarget.ElementArrayBuffer, id);
				if (format == DrawElementsType.UnsignedByte)
					GL.BufferData<byte>(BufferTarget.ElementArrayBuffer, new IntPtr(indices.Length * sizeof(byte)), (byte[]) indices, BufferUsageHint.StaticDraw);
				else if (format == DrawElementsType.UnsignedShort)
					GL.BufferData<ushort>(BufferTarget.ElementArrayBuffer, new IntPtr(indices.Length * sizeof(ushort)), (ushort[]) indices, BufferUsageHint.StaticDraw);
				else
					GL.BufferData<uint>(BufferTarget.ElementArrayBuffer, new IntPtr(indices.Length * sizeof(uint)), (uint[]) indices, BufferUsageHint.StaticDraw);
				if (!KeepCopyInMemory) {
					IndexBuffer temp;
					buffers.TryRemove(copy, out temp);
					copy = null;
				}
				indices = null;
			}
		}

		/// <summary>
		/// Unbinds the current index buffer
		/// </summary>
		public static void Unbind() {
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
		}

		/// <summary>
		/// Replaces the indices with the specified array
		/// </summary>
		/// <param name="array">The indices to use. Has to be byte[], ushort[] or uint[]</param>
		public void ReplaceIndicesWith(Array array) {
			if (array == null)
				array = new byte[0];
			count = array.Length;
			if (copy != null) {
				IndexBuffer temp;
				buffers.TryRemove(copy, out temp);
				copy = null;
			}
			indices = array;
			buffers.TryAdd(array, this);
			if (KeepCopyInMemory)
				copy = array;
			if (array is byte[])
				format = DrawElementsType.UnsignedByte;
			else if (array is ushort[])
				format = DrawElementsType.UnsignedShort;
			else if (array is uint[])
				format = DrawElementsType.UnsignedInt;
			else
				throw new ArgumentException("The index array must be byte[], ushort[] or uint[]", nameof(array));
		}

		/// <summary>
		/// Adds a reference to this index buffer
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void AddReference() {
			references++;
		}

		/// <summary>
		/// Returns the buffer name
		/// </summary>
		/// <returns>The buffer name</returns>
		public override int GetHashCode() {
			return id;
		}

		/// <summary>
		/// Creates a System.String that describes this IndexBuffer
		/// </summary>
		/// <returns>A System.String that describes this IndexBuffer</returns>
		public override string ToString() {
			return "Index buffer (handle " + id + ")";
		}

		/// <summary>
		/// Compares whether this IndexBuffer is equal to the specified object
		/// </summary>
		/// <param name="obj">An object to compare to</param>
		public override bool Equals(object obj) {
			return Equals(obj as IndexBuffer);
		}

		/// <summary>
		/// Compares whether this IndexBuffer is equal to the specified object
		/// </summary>
		/// <param name="other">The object to compare to</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool Equals(IndexBuffer other) {
			return other == null ? false : (id == other.id);
		}

		/// <summary>
		/// Disposes of the buffer and the resources consumed by it
		/// </summary>
		~IndexBuffer() {
			GraphicsContext.IsFinalizer = true;
			try {
				Dispose(true);
			} finally {
				GraphicsContext.IsFinalizer = false;
			}
		}

		/// <summary>
		/// Disposes of the buffer and the resources consumed by it
		/// </summary>
		public void Dispose() {
			Dispose(false);
		}

		/// <summary>
		/// Disposes of the buffer and the resources consumed by it
		/// </summary>
		/// <param name="forceDispose">If true, the reference count is ignored, forcing the buffer to be disposed, unless it is already disposed</param>
		public void Dispose(bool forceDispose) {
			if (id == 0 || count == 0)
				return;
			else if (GraphicsContext.IsFinalizer) {
				GraphicsContext.RaiseResourceLeakedEvent(this, LeakedWhile.Finalizing, new IntPtr(id));
				id = 0;
				count = 0;
				return;
			}
			if (references > 0)
				references--;
			if (references <= 0 || forceDispose) {
				try {
					GL.DeleteBuffers(1, ref id);
				} catch {
					GraphicsContext.RaiseResourceLeakedEvent(this, LeakedWhile.Disposing, new IntPtr(id));
				}
				id = 0;
				count = 0;
				IndexBuffer temp;
				if (copy != null) {
					buffers.TryRemove(copy, out temp);
					copy = null;
				} else if (indices != null)
					buffers.TryRemove(indices, out temp);
				indices = null;
				GC.SuppressFinalize(this);
			}
		}
	}
}