using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Numerics;

namespace System.Graphics.Models.Parsers.MeshParsers {
	/// <summary>
	/// Contains methods for parsing Msh models.
	/// </summary>
	[ModelParser("msh,Parse,Save")]
	public static class MshParser {
		/// <summary>
		/// Parses an Msh model.
		/// </summary>
		/// <param name="mesh">The stream containing the Msh data.</param>
		/// <param name="textures">The textures for the model to use (can be null or empty).</param>
		/// <returns>A list of models with all the parsed components.</returns>
		public static Model Parse(Stream mesh, ITexture[] textures) {
			using (MemoryStream stream = CompressionUtils.DecompressToStream(mesh)) {
				Vertex[][] bufferData;
				Array[] indices;
				using (BinaryReader reader = new BinaryReader(stream)) {
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
		}

		/// <summary>
		/// Serializes the Msh model into the specified stream.
		/// </summary>
		/// <param name="structure">The model to save.</param>
		/// <param name="stream">The stream to serialize to.</param>
		public static void Save(IModel structure, Stream stream) {
			using (MemoryStream memStream = new MemoryStream()) {
				List<Vertex[]> vertices = new List<Vertex[]>();
				List<Array> indices = new List<Array>();
				FromModelStructure(structure, vertices, indices);
				Vertex[][] bufferData = vertices.ToArray();
				Array[] indexArray = indices.ToArray();
				using (BinaryWriter writer = new BinaryWriter(memStream)) {
					writer.Write(bufferData.Length);
					Vertex[] temp;
					int i, j;
					for (i = 0; i < bufferData.Length; i++) {
						temp = bufferData[i];
						writer.Write(temp.Length);
						for (j = 0; j < temp.Length; j++) {
							writer.Write(temp[j].Pos.X);
							writer.Write(temp[j].Pos.Y);
							writer.Write(temp[j].Pos.Z);
							writer.Write(temp[j].TexPos.X);
							writer.Write(temp[j].TexPos.Y);
							writer.Write(temp[j].Normal.X);
							writer.Write(temp[j].Normal.Y);
							writer.Write(temp[j].Normal.Z);
						}
					}
					writer.Write(indexArray.Length);
					Array array;
					byte[] byteArray;
					ushort[] ushortArray;
					for (i = 0; i < indexArray.Length; i++) {
						array = indexArray[i];
						writer.Write(array.Length);
						byteArray = array as byte[];
						if (byteArray == null) {
							ushortArray = array as ushort[];
							if (ushortArray == null) {
								writer.Write((byte) 2);
								uint[] uintArray = (uint[]) array;
								for (j = 0; j < uintArray.Length; j++)
									writer.Write(uintArray[j]);
							} else {
								writer.Write((byte) 1);
								for (j = 0; j < ushortArray.Length; j++)
									writer.Write(ushortArray[j]);
							}
						} else {
							writer.Write((byte) 0);
							for (j = 0; j < byteArray.Length; j++)
								writer.Write(byteArray[j]);
						}
					}
				}
				using (MemoryStream saveStream = CompressionUtils.CompressToStream(memStream.ToArray()))
					saveStream.CopyTo(stream);
			}
		}

		private static void FromModelStructure(IModel structure, List<Vertex[]> vertices, List<Array> indices) {
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