using System.Drawing;
using System.IO;

namespace System.Graphics.Models.Parsers.TextureParsers {
	/// <summary>
	/// Contains methods for parsing image files supported by the Bitmap class.
	/// </summary>
	[TextureParser("bmp,Parse,jpg,Parse,png,Parse,jpeg,Parse,jpe,Parse,ico,Parse,icon,Parse,emf,Parse,exif,Parse,gif,Parse,wmf,Parse,tif,Parse,tiff,Parse")]
	public static class ImgParser {
		/// <summary>
		/// Parses textures.
		/// </summary>
		/// <param name="source">The location of the file to parse the texture from.</param>
		/// <returns>A list of the textures parsed.</returns>
		public static TextureCollection Parse(Stream source) {
			return new TextureCollection(new Texture2D(new Bitmap(source), NPotTextureScaleMode.ScaleUp, ImageParameterAction.Dispose));
		}
	}
}