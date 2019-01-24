using System.Graphics.OGL;
using System.Runtime.CompilerServices;

namespace System.Graphics.Models {
	/// <summary>
	/// Manages an OpenGL data buffer
	/// </summary>
	public sealed class DataBuffer : IEquatable<DataBuffer>, IDisposable {
		private GraphicsContext parentContext;
		private BufferTarget target;
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
		/// Initializes a new GL data buffer
		/// </summary>
		public DataBuffer() {
		}

		/// <summary>
		/// Binds the data buffer object
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Bind(BufferTarget target) {
			if (id == 0) {
				GL.GenBuffers(1, out id);
				parentContext = GraphicsContext.CurrentContext;
			}
			this.target = target;
			GL.BindBuffer(target, id);
		}

		/// <summary>
		/// Unbinds the current data buffer object
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Unbind() {
			GL.BindBuffer(target, 0);
		}

		/// <summary>
		/// Calculates the hash code for this DataBuffer
		/// </summary>
		public override int GetHashCode() {
			return id;
		}

		/// <summary>
		/// Creates a System.String that describes this DataBuffer
		/// </summary>
		public override string ToString() {
			return "Data buffer (handle " + id + ")";
		}

		/// <summary>
		/// Compares whether this DataBuffer is equal to the specified object
		/// </summary>
		/// <param name="obj">An object to compare to</param>
		public override bool Equals(object obj) {
			return Equals(obj as DataBuffer);
		}

		/// <summary>
		/// Compares whether this DataBuffer is equal to the specified object
		/// </summary>
		/// <param name="other">The object to compare to</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool Equals(DataBuffer other) {
			return other == null ? false : (id == other.id);
		}

		/// <summary>
		/// Disposes of the buffer and the resources consumed by it
		/// </summary>
		~DataBuffer() {
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
							GL.DeleteBuffers(1, ref currentId);
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
		/// Disposes of the buffer and the resources consumed by it. Do not dispose if still in use by other components
		/// </summary>
		public void Dispose() {
			if (id != 0 && (parentContext == null || parentContext.IsCurrent)) {
				try {
					GL.DeleteBuffers(1, ref id);
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