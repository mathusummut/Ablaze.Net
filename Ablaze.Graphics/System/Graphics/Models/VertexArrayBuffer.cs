using System.Graphics.OGL;
using System.Runtime.CompilerServices;

namespace System.Graphics.Models {
	/// <summary>
	/// Manages an OpenGL vertex array buffer. Remember to dispose the object on the OpenGL context thread after use
	/// </summary>
	public sealed class VertexArrayBuffer : IEquatable<VertexArrayBuffer>, IDisposable {
		private GraphicsContext parentContext;
		private int id;

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
		/// Gets whether the buffer is disposed
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
		/// Initializes a new GL vertex array buffer
		/// </summary>
		public VertexArrayBuffer() {
		}

		/// <summary>
		/// Binds the vertex array buffer object
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Bind() {
			if (id == 0) {
				GL.GenVertexArrays(1, out id);
				parentContext = GraphicsContext.CurrentContext;
			}
			GL.BindVertexArray(id);
		}

		/// <summary>
		/// Unbinds the current vertex array buffer object
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Unbind() {
			GL.BindVertexArray(0);
		}

		/// <summary>
		/// Calculates the hash code for this VertexArrayBuffer
		/// </summary>
		public override int GetHashCode() {
			return id;
		}

		/// <summary>
		/// Creates a System.String that describes this VertexArrayBuffer
		/// </summary>
		public override string ToString() {
			return "Vertex array buffer (handle " + id + ")";
		}

		/// <summary>
		/// Compares whether this VertexArrayBuffer is equal to the specified object
		/// </summary>
		/// <param name="obj">An object to compare to</param>
		public override bool Equals(object obj) {
			return Equals(obj as VertexArrayBuffer);
		}

		/// <summary>
		/// Compares whether this VertexArrayBuffer is equal to the specified object
		/// </summary>
		/// <param name="other">The object to compare to</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool Equals(VertexArrayBuffer other) {
			return other == null ? false : (id == other.id);
		}

		/// <summary>
		/// Disposes of the buffer and the resources consumed by it
		/// </summary>
		~VertexArrayBuffer() {
			if (parentContext == null) {
				if (id != 0) {
					GraphicsContext.RaiseResourceLeakedEvent(this, LeakedWhile.Finalizing, new IntPtr(id));
					id = 0;
				}
			} else {
				int currentId = id;
				if (currentId != 0) {
					parentContext.InvokeOnGLThreadAsync(context => {
						try {
							GL.DeleteVertexArrays(1, ref currentId);
						} catch {
							GraphicsContext.RaiseResourceLeakedEvent(this, LeakedWhile.Disposing, new IntPtr(currentId));
						}
					});
					id = 0;
				}
				parentContext = null;
			}
		}

		/// <summary>
		/// Disposes of the buffer and the resources consumed by it
		/// </summary>
		public void Dispose() {
			if (id != 0 && (parentContext == null || parentContext.IsCurrent)) {
				try {
					GL.DeleteVertexArrays(1, ref id);
				} catch {
					GraphicsContext.RaiseResourceLeakedEvent(this, LeakedWhile.Disposing, new IntPtr(id));
				}
				id = 0;
				parentContext = null;
			}
			if (id == 0)
				GC.SuppressFinalize(this);
		}
	}
}