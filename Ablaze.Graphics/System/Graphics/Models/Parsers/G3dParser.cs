using System.Drawing;
using System.IO;
using System.Numerics;

namespace System.Graphics.Models.Parsers {
	/// <summary>
	/// Contains methods for parsing and saving G3D models.
	/// </summary>
	[ModelParser("g3d,Parse,Save")]
	public static class G3dParser {
		/// <summary>
		/// Parses an Msh model
		/// </summary>
		/// <param name="mesh">The stream containing the G3D data.</param>
		/// <param name="textures">The textures for the model to use (can be null or empty)</param>
		/// <returns>A list of models with all the parsed components</returns>
		public static Model Parse(Stream mesh, ITexture[] textures) {
			Vertex[][] bufferData;
			Array[] indices;
			using (BinaryReader reader = new BinaryReader(mesh)) {
				bufferData = new Vertex[reader.ReadInt32()][];
				int i, j, length;
				for (i = 0; i < bufferData.Length; i++) {
					length = reader.ReadInt32();
					bufferData[i] = new Vertex[length];
					for (j = 0; j < length; j++)
						bufferData[i][j] = new Vertex(new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()), new Vector2(reader.ReadSingle(), reader.ReadSingle()), new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()));
				}
				length = reader.ReadInt32();
				indices = new Array[length];
				int tempLength;
				byte[] byteArray;
				ushort[] ushortArray;
				uint[] uintArray;
				for (i = 0; i < indices.Length; i++) {
					tempLength = reader.ReadInt32();
					switch (reader.ReadByte()) {
						case 0:
							byteArray = new byte[tempLength];
							for (j = 0; j < tempLength; j++)
								byteArray[j] = reader.ReadByte();
							indices[i] = byteArray;
							break;
						case 1:
							ushortArray = new ushort[tempLength];
							for (j = 0; j < tempLength; j++)
								ushortArray[j] = reader.ReadUInt16();
							indices[i] = ushortArray;
							break;
						default:
							uintArray = new uint[tempLength];
							for (j = 0; j < tempLength; j++)
								uintArray[j] = reader.ReadUInt32();
							indices[i] = uintArray;
							break;
					}
				}
			}
			MeshComponent[] temp = new MeshComponent[indices.Length];
			if (textures == null || textures.Length == 0)
				textures = Texture2D.EmptyTexture;
			int index = 0;
			for (int i = 0; i < temp.Length; i++) {
				temp[i] = new MeshComponent(textures[index], bufferData[i], indices[i]);
				if (index < textures.Length - 1)
					index++;
			}
			return new Model(temp);
		}

		/// <summary>
		/// Serializes the model structure into the specified stream
		/// </summary>
		/// <param name="structure">The structure to serialize</param>
		/// <param name="stream">The stream to serialize into</param>
		public static void Save(IModel structure, Stream stream) {
			MeshComponent component = structure as MeshComponent;
			if (component == null) {
				foreach (IModel comp in ((Model) structure))
					FromModelStructure(comp, vertices, indices);
			} else {
				indices.Add(component.IndexBuffer.Indices);
				vertices.Add(component.BufferData);
			}
		}
	}
}