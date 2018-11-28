using System.Graphics.OGL;

namespace System.Graphics.Models {
	/// <summary>
	/// Renders the buffer output into the specified texture(s). Typical usage is within a using statement
	/// </summary>
	public sealed class RenderToTexture : IDisposable {
		private FrameBuffer fbo;
		private RenderBuffer rbo;

		/// <summary>
		/// Renders the next scene into the specified textures
		/// </summary>
		/// <param name="width">The width of the buffer</param>
		/// <param name="height">The height of the buffer</param>
		/// <param name="depth">The depth of the buffer</param>
		/// <param name="textures">The textures to write to</param>
		public RenderToTexture(int width, int height, bool depth, Texture2D[] textures) {
			int buffers = textures.Length;
			fbo = new FrameBuffer();
			DrawBuffersEnum[] bufs = new DrawBuffersEnum[buffers];
			for (int i = 0; i < buffers; i++)
				bufs[i] = DrawBuffersEnum.ColorAttachment0 + i;
			fbo.Bind();
			GL.DrawBuffers(buffers, bufs);
			GL.DrawBuffer(DrawBufferMode.Back);
			for (int i = 0; i < buffers; i++)
				FrameBuffer.SetTexture(textures[i], i);
			if (depth) {
				rbo = new RenderBuffer();
				rbo.SetSize(width, height);
			}
			//GL.PushAttrib(AttribMask.ViewportBit);
			GL.Viewport(0, 0, textures[0].TextureSize.Width, textures[0].TextureSize.Height);
		}

		/// <summary>
		/// Signifies a resource leak
		/// </summary>
		~RenderToTexture() {
			if (fbo != null) {
				GraphicsContext.RaiseResourceLeakedEvent(this, LeakedWhile.Finalizing, new IntPtr(fbo.GetHashCode()));
				fbo = null;
			}
			if (rbo != null) {
				GraphicsContext.RaiseResourceLeakedEvent(this, LeakedWhile.Finalizing, new IntPtr(rbo.GetHashCode()));
				rbo = null;
			}
		}

		/// <summary>
		/// Finishes capturing.
		/// </summary>
		public void Dispose() {
			FrameBuffer.Unbind();
			//GL.PopAttrib();
			if (fbo != null) {
				fbo.Dispose();
				fbo = null;
			}
			if (rbo != null) {
				rbo.Dispose();
				rbo = null;
			}
			GC.SuppressFinalize(this);
		}
	}
}