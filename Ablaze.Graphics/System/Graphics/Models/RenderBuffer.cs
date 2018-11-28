using System.Graphics.OGL;
using System.Runtime.CompilerServices;

namespace System.Graphics.Models {
	/// <summary>
	/// Manages an OpenGL RenderBuffer object. Similar to a FrameBuffer but the resources won't be accessable.
	/// Remember to dispose the object on the OpenGL context thread after use
	/// </summary>
	public sealed class RenderBuffer : IDisposable {
		private int id;

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
		/// Initializes a new render buffer.
		/// </summary>
		public RenderBuffer() {
		}

		/// <summary>
		/// Binds the render buffer.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Bind() {
			if (id == 0)
				GL.GenRenderbuffers(1, out id);
			GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, id);
		}

		/// <summary>
		/// Unbinds any currently active render buffer.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Unbind() {
			GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);
		}

		/// <summary>
		/// Sets the size of the current renderbuffer
		/// </summary>
		/// <param name="width">The width in pixels of the renderbuffer</param>
		/// <param name="height">The height in pixels of the renderbuffer</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void SetSize(int width, int height) {
			GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent24, width, height);
			GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, id);
		}

		/// <summary>
		/// Returns the buffer name.
		/// </summary>
		/// <returns>The buffer name.</returns>
		public override int GetHashCode() {
			return id;
		}

		/// <summary>
		/// Creates a System.String that describes this RenderBuffer.
		/// </summary>
		/// <returns>A System.String that describes this RenderBuffer.</returns>
		public override string ToString() {
			return "Render buffer (handle " + id + ")";
		}

		/// <summary>
		/// Compares whether this RenderBuffer is equal to the specified object.
		/// </summary>
		/// <param name="obj">An object to compare to.</param>
		public override bool Equals(object obj) {
			return Equals(obj as RenderBuffer);
		}

		/// <summary>
		/// Compares whether this RenderBuffer is equal to the specified object.
		/// </summary>
		/// <param name="other">The object to compare to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool Equals(RenderBuffer other) {
			return other == null ? false : (id == other.id);
		}

		/// <summary>
		/// Disposes of the buffer and its resources.
		/// </summary>
		~RenderBuffer() {
			if (id != 0) {
				GraphicsContext.RaiseResourceLeakedEvent(this, LeakedWhile.Finalizing, new IntPtr(id));
				id = 0;
			}
		}

		/// <summary>
		/// Disposes of the buffer and its resources.
		/// </summary>
		public void Dispose() {
			if (id == 0)
				return;
			try {
				GL.DeleteRenderbuffers(1, ref id);
			} catch {
				GraphicsContext.RaiseResourceLeakedEvent(this, LeakedWhile.Disposing, new IntPtr(id));
			}
			id = 0;
			GC.SuppressFinalize(this);
		}
	}
}