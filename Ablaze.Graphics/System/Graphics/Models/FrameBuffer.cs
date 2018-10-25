using System.Graphics.OGL;
using System.Runtime.CompilerServices;

namespace System.Graphics.Models {
	/// <summary>
	/// Manages an OpenGL frame buffer object. Remember to dispose the object on the OpenGL context thread after use
	/// </summary>
	public sealed class FrameBuffer : IEquatable<FrameBuffer>, IDisposable {
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
		/// Checks whether the buffer is disposed
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
		/// Initializes a new frame buffer
		/// </summary>
		public FrameBuffer() {
		}

		/// <summary>
		/// Binds the framebuffer object. All fragment output that follows will be captured by it
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Bind() {
			if (id == 0) {
				if (GL.Delegates.glGenFramebuffers == null)
					GL.Ext.GenFramebuffers(1, out id);
				else
					GL.GenFramebuffers(1, out id);
			}
			if (GL.Delegates.glBindFramebuffer == null)
				GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, id);
			else
				GL.BindFramebuffer(FramebufferTarget.Framebuffer, id);
		}

		/// <summary>
		/// Removes any current framebuffer object
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Unbind() {
			if (GL.Delegates.glBindFramebuffer == null)
				GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0);
			else
				GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
		}

		/// <summary>
		/// Associates a Texture with the current framebuffer object
		/// </summary>
		/// <param name="texture">The texture to associate</param>
		/// <param name="index">The channel to asscociate</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void SetTexture(Texture2D texture, int index = 0) {
			if (GL.Delegates.glFramebufferTexture2D == null)
				GL.Ext.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.ColorAttachment0 + index, TextureTarget.Texture2D, texture.GetHashCode(), 0);
			else
				GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0 + index, TextureTarget.Texture2D, texture.GetHashCode(), 0);
		}

		/// <summary>
		/// Calculates the hash code for this FrameBuffer
		/// </summary>
		public override int GetHashCode() {
			return id;
		}

		/// <summary>
		/// Creates a System.String that describes this FrameBuffer
		/// </summary>
		public override string ToString() {
			return "Frame buffer (handle " + id + ")";
		}

		/// <summary>
		/// Compares whether this FrameBuffer is equal to the specified object
		/// </summary>
		/// <param name="obj">An object to compare to</param>
		public override bool Equals(object obj) {
			return Equals(obj as FrameBuffer);
		}

		/// <summary>
		/// Compares whether this FrameBuffer is equal to the specified object
		/// </summary>
		/// <param name="other">The object to compare to</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool Equals(FrameBuffer other) {
			return other == null ? false : (id == other.id);
		}

		/// <summary>
		/// Disposes of the frame buffer
		/// </summary>
		~FrameBuffer() {
			if (id != 0) {
				GraphicsContext.RaiseResourceLeakedEvent(this, LeakedWhile.Finalizing, new IntPtr(id));
				id = 0;
			}
		}

		/// <summary>
		/// Disposes of the frame buffer
		/// </summary>
		public void Dispose() {
			if (id == 0)
				return;
			try {
				if (GL.Delegates.glDeleteFramebuffers == null)
					GL.Ext.DeleteFramebuffers(1, ref id);
				else
					GL.DeleteFramebuffers(1, ref id);
			} catch {
				GraphicsContext.RaiseResourceLeakedEvent(this, LeakedWhile.Disposing, new IntPtr(id));
			}
			id = 0;
			GC.SuppressFinalize(this);
		}
	}
}