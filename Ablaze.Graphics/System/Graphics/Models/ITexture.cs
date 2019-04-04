using System.Drawing;
using System.Graphics.OGL;

namespace System.Graphics.Models {
	/// <summary>
	/// Represents a bindable texture
	/// </summary>
	public interface ITexture : ICloneable, IDisposable {
		/// <summary>
		/// Gets or sets whether the texture alpha components are premultiplied
		/// </summary>
		bool Premultiplied {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets additional info to be stored with the texture
		/// </summary>
		object Tag {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the texture name
		/// </summary>
		string Name {
			get;
			set;
		}

		/// <summary>
		/// Gets the current texture that will be bound upon calling Bind()
		/// </summary>
		ITexture Current {
			get;
		}

		/// <summary>
		/// Gets or sets what to do with the passed image after binding the texture into GPU memory
		/// </summary>
		ImageParameterAction BindAction {
			get;
			set;
		}
		
		/// <summary>
		/// Gets whether the texture is empty
		/// </summary>
		bool IsEmpty {
			get;
		}

		/// <summary>
		/// Gets whether the texture is disposed
		/// </summary>
		bool IsDisposed {
			get;
		}

		/// <summary>
		/// Gets the number of textures in the collection (or if a single texture, returns 1 if not empty else 0)
		/// </summary>
		int Count {
			get;
		}

		/// <summary>
		/// Binds the texture for use with OpenGL operations
		/// </summary>
		void Bind();

		/// <summary>
		/// Binds the texture for use with OpenGL operations
		/// </summary>
		/// <param name="mode">The texture wrap mode to use</param>
		void Bind(TextureWrapMode mode);

		/// <summary>
		/// Unbinds the texture
		/// </summary>
		void Unbind();

		/// <summary>
		/// Creates a copy of the texture
		/// </summary>
		/// <param name="components">Whether to clone the internal components too</param>
		ITexture Clone(bool components);

		/// <summary>
		/// Disposes of the resources used by the texture
		/// </summary>
		/// <param name="disposeChildren">Whether to dispose of the child components of the texture</param>
		void Dispose(bool disposeChildren);
	}
}