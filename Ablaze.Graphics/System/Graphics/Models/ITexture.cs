using System.Graphics.OGL;

namespace System.Graphics.Models {
	/// <summary>
	/// Represents a bindable texture.
	/// </summary>
	public interface ITexture : IEquatable<ITexture>, IDisposable {
		/// <summary>
		/// Gets or sets whether the texture alpha components are premultiplied.
		/// </summary>
		bool Premultiplied {
			get;
			set;
		}

		/// <summary>
		/// Gets the currently assigned OpenGL name (0 if not bound once yet).
		/// </summary>
		int Name {
			get;
		}

		/// <summary>
		/// Gets or sets additional info to be stored with the texture.
		/// </summary>
		object Info {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the texture name.
		/// </summary>
		string ID {
			get;
			set;
		}

		/// <summary>
		/// Adds a reference to this texture.
		/// </summary>
		void AddReference();

		/// <summary>
		/// Binds the texture for use with OpenGL operations.
		/// </summary>
		void Bind();

		/// <summary>
		/// Binds the texture for use with OpenGL operations.
		/// </summary>
		/// <param name="mode">The texture wrap mode to use.</param>
		void Bind(TextureWrapMode mode);

		/// <summary>
		/// Unbinds the texture.
		/// </summary>
		void Unbind();
	}
}