using System.Graphics.OGL;
using System.Runtime.CompilerServices;

namespace System.Graphics.Models {
	/// <summary>
	/// Manages an OpenGL data buffer.
	/// </summary>
	public sealed class DataBuffer : IEquatable<DataBuffer>, IDisposable {
		private int id, references = 1;
		private BufferTarget target;

		/// <summary>
		/// Gets the native OpenGL name of the buffer.
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
		/// Gets whether the buffer is disposed.
		/// </summary>
		public bool IsDisposed {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return id == 0;
			}
		}

		/// <summary>
		/// Initializes a new GL data buffer.
		/// </summary>
		public DataBuffer() {
		}

		/// <summary>
		/// Binds the data buffer object.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Bind(BufferTarget target) {
			if (id == 0)
				GL.GenBuffers(1, out id);
			this.target = target;
			GL.BindBuffer(target, id);
		}

		/// <summary>
		/// Unbinds the current data buffer object.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Unbind() {
			GL.BindBuffer(target, 0);
		}

		/// <summary>
		/// Calculates the hash code for this DataBuffer.
		/// </summary>
		/// <returns>A System.Int32 containing the hashcode of this DataBuffer.</returns>
		public override int GetHashCode() {
			return id;
		}

		/// <summary>
		/// Creates a System.String that describes this DataBuffer.
		/// </summary>
		/// <returns>A System.String that describes this DataBuffer.</returns>
		public override string ToString() {
			return "Data buffer (handle " + id + ")";
		}

		/// <summary>
		/// Adds a reference to this data buffer.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void AddReference() {
			references++;
		}

		/// <summary>
		/// Compares whether this DataBuffer is equal to the specified object.
		/// </summary>
		/// <param name="obj">An object to compare to.</param>
		public override bool Equals(object obj) {
			return Equals(obj as DataBuffer);
		}

		/// <summary>
		/// Compares whether this DataBuffer is equal to the specified object.
		/// </summary>
		/// <param name="other">The object to compare to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool Equals(DataBuffer other) {
			return other == null ? false : (id == other.id);
		}

		/// <summary>
		/// Disposes of the data buffer.
		/// </summary>
		~DataBuffer() {
			GraphicsContext.IsFinalizer = true;
			try {
				Dispose(true);
			} finally {
				GraphicsContext.IsFinalizer = false;
			}
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
		/// 
		public void Dispose(bool forceDispose) {
			if (id == 0)
				return;
			else if (GraphicsContext.IsFinalizer) {
				GraphicsContext.RaiseResourceLeakedEvent(this, LeakedWhile.Finalizing, new IntPtr(id));
				id = 0;
				return;
			} else if (references > 0)
				references--;
			if (references <= 0 || forceDispose) {
				try {
					GL.DeleteBuffers(1, ref id);
				} catch {
					GraphicsContext.RaiseResourceLeakedEvent(this, LeakedWhile.Disposing, new IntPtr(id));
				}
				id = 0;
				GC.SuppressFinalize(this);
			}
		}
	}
}