﻿using System.Graphics.OGL;
using System.Runtime.CompilerServices;

namespace System.Graphics.Models {
	/// <summary>
	/// Manages an OpenGL RenderBuffer object.
	/// This is similar to a FrameBuffer but the resources won't be accessable.
	/// </summary>
	public sealed class RenderBuffer : IDisposable {
		private int name;

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
		/// Gets whether the buffer is disposed.
		/// </summary>
		public bool IsDisposed {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return name == 0;
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
			if (name == 0)
				GL.GenRenderbuffers(1, out name);
			GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, name);
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
			GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, name);
		}

		/// <summary>
		/// Returns the buffer name.
		/// </summary>
		/// <returns>The buffer name.</returns>
		public override int GetHashCode() {
			return name;
		}

		/// <summary>
		/// Creates a System.String that describes this RenderBuffer.
		/// </summary>
		/// <returns>A System.String that describes this RenderBuffer.</returns>
		public override string ToString() {
			return "Render buffer (handle " + name + ")";
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
			return other == null ? false : (name == other.name);
		}

		/// <summary>
		/// Disposes of the buffer and its resources.
		/// </summary>
		~RenderBuffer() {
			if (name == 0)
				return;
			GraphicsContext.RaiseResourceLeakedEvent(this, LeakedWhile.Finalizing, new IntPtr(name));
			name = 0;
		}

		/// <summary>
		/// Disposes of the buffer and its resources.
		/// </summary>
		public void Dispose() {
			if (name == 0)
				return;
			try {
				GL.DeleteRenderbuffers(1, ref name);
			} catch {
				GraphicsContext.RaiseResourceLeakedEvent(this, LeakedWhile.Disposing, new IntPtr(name));
			}
			name = 0;
			GC.SuppressFinalize(this);
		}
	}
}