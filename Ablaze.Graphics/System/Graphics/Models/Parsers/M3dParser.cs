using System.IO;
using System.Text;
using System.Graphics.Models.Parsers.MeshParsers;

namespace System.Graphics.Models.Parsers {
	/// <summary>
	/// Contains methods for parsing M3d models.
	/// </summary>
	[ModelParser("m3d,Parse,Save")]
	public static class M3dParser {
		/// <summary>
		/// Parses an Msh model.
		/// </summary>
		/// <param name="mesh">The stream containing the M3d data.</param>
		/// <param name="textures">Ignored.</param>
		/// <returns>A list of models with all the parsed components.</returns>
		public static Model Parse(Stream mesh, ITexture[] textures) {
			using (BinaryReader reader = new BinaryReader(mesh)) {
				StringBuilder length = new StringBuilder();
				do {
					length.Append(reader.ReadChar());
				} while (char.IsNumber((char) reader.PeekChar()));
				int characters = int.Parse(length.ToString());
				if (characters <= 0)
					throw new FormatException("The specified (if at all) character length is invalid.");
				length.Length = 0;
				for (int i = 0; i < characters; i++)
					length.Append(reader.ReadChar());
				textures = TextureParser.Parse(length.ToString());
				return MshParser.Parse(mesh, textures);
			}
		}

		/// <summary>
		/// Serializes the model structure into the specified stream.
		/// </summary>
		/// <param name="structure">The structure to serialize.</param>
		/// <param name="stream">The stream to serialize into.</param>
		public static void Save(IModel structure, Stream stream) {
			MshParser.Save(structure, stream);
		}
	}
}