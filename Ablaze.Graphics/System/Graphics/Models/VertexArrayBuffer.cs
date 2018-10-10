using System.Graphics.OGL;
using System.Runtime.CompilerServices;

namespace System.Graphics.Models {
	/// <summary>
	/// Manages an OpenGL vertex array buffer.
	/// </summary>
	public sealed class VertexArrayBuffer : IEquatable<VertexArrayBuffer>, IDisposable {
		private int id, references = 1;

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
		/// Initializes a new GL vertex array buffer.
		/// </summary>
		public VertexArrayBuffer() {
		}

		/// <summary>
		/// Binds the vertex array buffer object.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Bind() {
			if (id == 0)
				GL.GenVertexArrays(1, out id);
			GL.BindVertexArray(id);
		}

		/// <summary>
		/// Unbinds the current vertex array buffer object.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Unbind() {
			GL.BindVertexArray(0);
		}

		/// <summary>
		/// Calculates the hash code for this VertexArrayBuffer.
		/// </summary>
		/// <returns>A System.Int32 containing the hashcode of this VertexArrayBuffer.</returns>
		public override int GetHashCode() {
			return id;
		}

		/// <summary>
		/// Creates a System.String that describes this VertexArrayBuffer.
		/// </summary>
		/// <returns>A System.String that describes this VertexArrayBuffer.</returns>
		public override string ToString() {
			return "Vertex array buffer (handle " + id + ")";
		}

		/// <summary>
		/// Adds a reference to this buffer.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void AddReference() {
			references++;
		}

		/// <summary>
		/// Compares whether this VertexArrayBuffer is equal to the specified object.
		/// </summary>
		/// <param name="obj">An object to compare to.</param>
		public override bool Equals(object obj) {
			return Equals(obj as VertexArrayBuffer);
		}

		/// <summary>
		/// Compares whether this VertexArrayBuffer is equal to the specified object.
		/// </summary>
		/// <param name="other">The object to compare to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool Equals(VertexArrayBuffer other) {
			return other == null ? false : (id == other.id);
		}

		/// <summary>
		/// Disposes of the vertex array buffer.
		/// </summary>
		~VertexArrayBuffer() {
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
					GL.DeleteVertexArrays(1, ref id);
				} catch {
					GraphicsContext.RaiseResourceLeakedEvent(this, LeakedWhile.Disposing, new IntPtr(id));
				}
				id = 0;
				GC.SuppressFinalize(this);
			}
		}
	}
}