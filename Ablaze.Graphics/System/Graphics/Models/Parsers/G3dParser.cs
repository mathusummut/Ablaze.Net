using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Text;

namespace System.Graphics.Models.Parsers {
	/// <summary>
	/// Contains methods for parsing and saving G3D models
	/// </summary>
	[ModelParser("g3d,Parse,Save")]
	public static class G3dParser {
		/// <summary>
		/// Describes rendering properties for the mesh
		/// </summary>
		[Flags]
		private enum MeshPropertyFlag {
			/// <summary>
			/// No property is specified
			/// </summary>
			None = 0,
			/// <summary>
			/// Whether the alpha in this model is replaced by a custom color, usually the player color
			/// </summary>
			CustomColor = 1,
			/// <summary>
			/// Whether culling is disabled
			/// </summary>
			TwoSided = 2,
			/// <summary>
			/// Whether the model is selectable
			/// </summary>
			NoSelect = 4,
			/// <summary>
			/// Whether the model has a glow effect
			/// </summary>
			Glow = 8
		}

		/// <summary>
		/// Lists the types of textures associated with the model
		/// </summary>
		[Flags]
		private enum MeshTexture {
			/// <summary>
			/// No texture is specified
			/// </summary>
			None = 0,
			/// <summary>
			/// Whether a UV mapping texture is defined
			/// </summary>
			Diffuse = 1,
			/// <summary>
			/// Whether a specular mapping texture is defined
			/// </summary>
			Specular = 2,
			/// <summary>
			/// Whether a normal map texture is defined
			/// </summary>
			Normal = 4
		}

		/// <summary>
		/// Parses a G3D model
		/// </summary>
		/// <param name="mesh">The stream containing the G3D data</param>
		/// <param name="textures">The textures for the model to use (can be null or empty)</param>
		/// <returns>An animated model with all the parsed components</returns>
		public static Model Parse(Stream mesh, TextureCollection textures) {
			const float AnimationSpeed = 75f;
			AnimatedModel model = new AnimatedModel(AnimationSpeed);
			using (BinaryReader reader = new BinaryReader(mesh)) {
				if (Encoding.UTF8.GetString(reader.ReadBytes(3)) != "G3D")
					throw new FormatException("Unrecognized model format (expected \"G3D\" at start of file)");
				byte version = reader.ReadByte();
				if (version == 0)
					return null;
				else if (version != 4)
					throw new FormatException("Unsupported version number (expected version 4, instead found version " + version + ")");
				int meshCount = reader.ReadUInt16(); //the number of meshes in the model
				reader.ReadByte(); //discard type

				string name; //the name of the model
				uint frameCount, vertexCount, indexCount;
				ColorF diffuseColor; //the hue of the model
				ColorF specularColor; //the specular hue of the model
				float specularPower; //the strength of the specular highlights
				float opacity; //the model opacity
				MeshPropertyFlag properties; //specifies property flags for the mesh
				MeshTexture associatedTextures; //specifies which textures are used by the mesh
				TextureCollection diffuseTexture; //the texture used by the model
				int vertex, frame, index;
				Vertex[][] bufferData;
				Vertex[] frameBufferData, coords;
				uint[] indices;
				AnimatedModel animatedComponent;

				for (int i = 0; i < meshCount; i++) {
					name = Encoding.UTF8.GetString(reader.ReadBytes(64)).TruncateAtNull();
					frameCount = reader.ReadUInt32();
					vertexCount = reader.ReadUInt32();
					indexCount = reader.ReadUInt32();
					diffuseColor = new ColorF(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
					specularColor = new ColorF(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
					specularPower = reader.ReadSingle();
					opacity = reader.ReadSingle();
					properties = (MeshPropertyFlag) reader.ReadUInt32();
					associatedTextures = (MeshTexture) reader.ReadUInt32();
					if (textures == null || textures.Count == 0) {
						if ((associatedTextures & MeshTexture.Diffuse) == MeshTexture.Diffuse) { //has texture
							try {
								diffuseTexture = TextureParser.Parse(Encoding.UTF8.GetString(reader.ReadBytes(64)).TruncateAtNull());
							} catch {
								diffuseTexture = null;
							}
						} else
							diffuseTexture = null;
					} else
						diffuseTexture = textures;
					bufferData = new Vertex[frameCount][];
					if (frameCount != 0) {
						for (frame = 0; frame < frameCount; frame++) {
							frameBufferData = new Vertex[vertexCount];
							bufferData[frame] = frameBufferData;
							for (vertex = 0; vertex < vertexCount; vertex++)
								frameBufferData[vertex].Pos = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
						}
						for (frame = 0; frame < frameCount; frame++) {
							frameBufferData = bufferData[frame];
							for (vertex = 0; vertex < vertexCount; vertex++)
								frameBufferData[vertex].Normal = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
						}
						if (associatedTextures != MeshTexture.None) {
							coords = bufferData[0];
							for (vertex = 0; vertex < vertexCount; vertex++)
								coords[vertex].TexPos = new Vector2(reader.ReadSingle(), 1f - reader.ReadSingle());
							for (frame = 1; frame < frameCount; frame++) {
								frameBufferData = bufferData[frame];
								for (vertex = 0; vertex < vertexCount; vertex++)
									frameBufferData[vertex].TexPos = coords[vertex].TexPos;
							}
						}
					}
					indices = new uint[indexCount];
					for (index = 0; index < indexCount; index++)
						indices[index] = reader.ReadUInt32();
					animatedComponent = new AnimatedModel(AnimationSpeed);
					for (frame = 0; frame < frameCount; frame++) {
						animatedComponent.Add(new MeshComponent(name, diffuseTexture, bufferData[frame], indices) {
							Cull = (properties & MeshPropertyFlag.TwoSided) != MeshPropertyFlag.TwoSided,
							Alpha = opacity,
							MaterialHue = diffuseColor,
							ShineHue = specularColor,
							Shininess = specularPower
						});
					}
					model.CombineWith(animatedComponent, true);
				}
			}
			return model;
		}

		/// <summary>
		/// Serializes the model structure into the specified stream (must be an AnimatedModel instance)
		/// </summary>
		/// <param name="structure">The structure to serialize</param>
		/// <param name="stream">The stream to serialize into</param>
		public static void Save(IModel structure, Stream stream) {
			Model temp;
			while (structure.Count == 1) {
				temp = structure as Model;
				if (temp == null)
					break;
				else
					structure = temp[0];
			}
			using (BinaryWriter writer = new BinaryWriter(stream)) {
				//model header
				writer.Write((byte) 'G');
				writer.Write((byte) '3');
				writer.Write((byte) 'D');
				writer.Write((byte) 4);
				AnimatedModel model = structure as AnimatedModel;
				int frameCount = model.Count;
				int meshCount = MeshExtensions.IsNullOrEmpty(model) ? 0 : model[0].Count;
				writer.Write((ushort) meshCount);
				writer.Write((byte) 0);
				if (meshCount == 0)
					return;

				MeshComponent mesh;
				MeshComponent currentMesh = model[0] as MeshComponent;
				bool hasMultipleModels = currentMesh == null;
					
				List<byte> name; //the name of the model
				Vertex[] bufferData;
				int frame, vertex, index;
				bool hasTexture;
				for (int i = 0; i < meshCount; i++) {
					currentMesh = hasMultipleModels ? ((MeshComponent) ((Model) model[0])[i]) : (MeshComponent) model[i];
					name = new List<byte>(Encoding.UTF8.GetBytes(currentMesh.Name == null ? string.Empty : (currentMesh.Name.Length > 63 ? currentMesh.Name.Substring(0, 63) : currentMesh.Name)));
					while (name.Count < 64)
						name.Add(0);
					writer.Write(name.ToArray()); //the name of the model
					writer.Write((uint) frameCount); //number of frames of the mesh
					writer.Write((uint) currentMesh.Vertices); //number of vertices in the mesh
					writer.Write((uint) currentMesh.IndexCount); //number of indices in the mesh
					writer.Write(currentMesh.MaterialHue.R);
					writer.Write(currentMesh.MaterialHue.G);
					writer.Write(currentMesh.MaterialHue.B);
					writer.Write(currentMesh.ShineHue.R);
					writer.Write(currentMesh.ShineHue.G);
					writer.Write(currentMesh.ShineHue.B);
					writer.Write(currentMesh.Shininess);
					writer.Write(currentMesh.MaterialHue.A);
					writer.Write((uint) (currentMesh.Cull ? MeshPropertyFlag.None : MeshPropertyFlag.TwoSided));
					hasTexture = MeshExtensions.IsNullOrEmpty(currentMesh.Textures);
					if (hasTexture)
						writer.Write((uint) MeshTexture.None);
					else {
						writer.Write((uint) MeshTexture.Diffuse);
						name = new List<byte>(Encoding.UTF8.GetBytes(currentMesh.Textures.Name == null ? string.Empty : (currentMesh.Textures.Name.Length > 63 ? currentMesh.Textures.Name.Substring(0, 63) : currentMesh.Textures.Name)));
						while (name.Count < 64)
							name.Add(0);
						writer.Write(name.ToArray());
					}
					for (frame = 0; frame < frameCount; frame++) {
						mesh = hasMultipleModels ? ((MeshComponent) ((Model) model[frame])[i]) : (MeshComponent) model[frame];
						bufferData = mesh.BufferData;
						for (vertex = 0; vertex < mesh.Vertices; vertex++) {
							writer.Write(bufferData[vertex].Pos.X);
							writer.Write(bufferData[vertex].Pos.Y);
							writer.Write(bufferData[vertex].Pos.Z);
						}
					}
					for (frame = 0; frame < frameCount; frame++) {
						mesh = hasMultipleModels ? ((MeshComponent) ((Model) model[frame])[i]) : (MeshComponent) model[frame];
						bufferData = mesh.BufferData;
						for (vertex = 0; vertex < mesh.Vertices; vertex++) {
							writer.Write(bufferData[vertex].Normal.X);
							writer.Write(bufferData[vertex].Normal.Y);
							writer.Write(bufferData[vertex].Normal.Z);
						}
					}
					if (hasTexture) {
						mesh = hasMultipleModels ? ((MeshComponent) ((Model) model[frame])[i]) : (MeshComponent) model[frame];
						bufferData = mesh.BufferData;
						for (vertex = 0; vertex < mesh.Vertices; vertex++) {
							writer.Write(bufferData[vertex].TexPos.X);
							writer.Write(bufferData[vertex].TexPos.Y);
						}
					}
					Array buffer = currentMesh.IndexBuffer.Indices;
					switch (currentMesh.IndexBuffer.Format) {
						case OGL.DrawElementsType.UnsignedByte: {
								byte[] indices = (byte[]) buffer;
								for (index = 0; index < indices.Length; index++)
									writer.Write((uint) indices[index]);
								break;
							}
						case OGL.DrawElementsType.UnsignedShort: {
								ushort[] indices = (ushort[]) buffer;
								for (index = 0; index < indices.Length; index++)
									writer.Write((uint) indices[index]);
								break;
							}
						default: {
								uint[] indices = (uint[]) buffer;
								for (index = 0; index < indices.Length; index++)
									writer.Write(indices[index]);
								break;
							}
					}
				}
			}
		}
	}
}