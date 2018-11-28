using System.Drawing;
using System.IO;
using System.Numerics;
using System.Text;

namespace System.Graphics.Models.Parsers {
	/// <summary>
	/// Contains methods for parsing and saving G3D models
	/// </summary>
	[ModelParser("g3d,Parse,")]
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
				if (Encoding.ASCII.GetString(reader.ReadBytes(3)) != "G3D")
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
					name = Encoding.ASCII.GetString(reader.ReadBytes(64)).TruncateAtNull();
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
						if ((associatedTextures & MeshTexture.Diffuse) == MeshTexture.Diffuse) //has texture
							diffuseTexture = TextureParser.Parse(Encoding.ASCII.GetString(reader.ReadBytes(64)).TruncateAtNull());
						else
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
						animatedComponent.Add(new MeshComponent(diffuseTexture, bufferData[frame], indices) {
							Cull = (properties & MeshPropertyFlag.TwoSided) != MeshPropertyFlag.TwoSided,
							Alpha = opacity,
							MaterialHue = diffuseColor,
							ShineHue = specularColor,
							Shininess = specularPower,
							Name = name
						});
					}
					model.CombineWith(animatedComponent);
				}
			}
			return model;
		}

		/*/// <summary>
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
		}*/
	}
}