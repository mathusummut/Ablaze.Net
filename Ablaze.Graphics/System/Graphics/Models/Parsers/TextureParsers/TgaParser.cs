using System.Drawing;
using System.IO;

namespace System.Graphics.Models.Parsers.TextureParsers {
	/// <summary>
	/// Contains methods for parsing image files supported by the Bitmap class.
	/// </summary>
	[TextureParser("tga,Parse")]
	public static class TgaParser {
		/// <summary>
		/// Parses textures.
		/// </summary>
		/// <param name="source">The location of the file to parse the texture from.</param>
		/// <returns>A list of the textures parsed.</returns>
		public static TextureCollection Parse(Stream source) {
			return new TextureCollection(new Texture2D(new TargaImage(source).Image, NPotTextureScaleMode.ScaleUp, ImageParameterAction.Dispose));
		}
	}
}