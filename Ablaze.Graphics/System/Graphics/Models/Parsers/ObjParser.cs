using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Graphics.Models.Parsers.TextureParsers;
using System.IO;
using System.Numerics;

namespace System.Graphics.Models.Parsers {
	/// <summary>
	/// Contains methods for parsing WaveFront Obj 3D models
	/// </summary>
	[ModelParser("obj,Parse,Save")]
	public static class ObjParser {
		/// <summary>
		/// Whether to optimize the obj vertices on load. The obj will takes less memory space, but optimization can take very long
		/// </summary>
		public static bool OptimizeVertexDuplicates;
		internal static char[] SplitChar = new char[] { ' ' };
		private static char[] FaceSplitChar = new char[] { '/' };

		/// <summary>
		/// Parses a WaveFront Obj model
		/// </summary>
		/// <param name="mesh">The stream containing the obj data</param>
		/// <param name="textures">The textures for the model to use (can be null or empty)</param>
		/// <returns>A list of models with all the parsed components</returns>
		public static Model Parse(Stream mesh, ITexture[] textures) {
			if (textures == null || textures.Length == 0)
				textures = new Texture2D[0];
			using (StreamReader reader = new StreamReader(mesh))
				return Parse(reader, new List<ITexture>(), new List<ITexture>(textures));
		}

		private static Model Parse(StreamReader reader, List<ITexture> textures, List<ITexture> loadedTextures) {
			List<Model> models = new List<Model>();
			List<MeshComponent> components = new List<MeshComponent>();
			List<Vector3> vertices = new List<Vector3>();
			List<Vector2> texCoords = new List<Vector2>();
			List<Vector3> normals = new List<Vector3>();
			List<VertexIndex> points = new List<VertexIndex>();
			int index = 0;
			string line;
			string name;
			string[] indices;
			Queue<string> names = new Queue<string>();
			ITexture[] parsedTextures;
			MeshComponent component;
			ITexture currText;
			MtlParser.Material material = null;
			ITexture[] current = null;
			List<KeyValuePair<VertexIndex, Vertex>> vertexIndices;
			VertexIndex temp1, temp2;
			Vertex currentVertex;
			int commentIndex, i;
			Dictionary<string, ITexture[]> textureLookup = new Dictionary<string, ITexture[]>(StringComparer.OrdinalIgnoreCase);
			while ((line = reader.ReadLine()) != null) {
				commentIndex = line.IndexOf('#');
				if (commentIndex != -1)
					line = line.Substring(0, commentIndex);
				indices = line.Trim().Split(SplitChar, StringSplitOptions.RemoveEmptyEntries);
				if (indices.Length != 0) {
					switch (indices[0].ToLower()) {
						case "v":
							if (indices.Length >= 4)
								vertices.Add(new Vector3(float.Parse(indices[1], NumberStyles.Float), float.Parse(indices[2], NumberStyles.Float), float.Parse(indices[3], NumberStyles.Float)));
							else if (indices.Length == 3)
								vertices.Add(new Vector3(float.Parse(indices[1], NumberStyles.Float), float.Parse(indices[2], NumberStyles.Float), 0f));
							else if (indices.Length == 2)
								vertices.Add(new Vector3(float.Parse(indices[1], NumberStyles.Float), 0f, 0f));
							else
								vertices.Add(new Vector3());
							break;
						case "vt":
							if (indices.Length >= 3)
								texCoords.Add(new Vector2(float.Parse(indices[1], NumberStyles.Float), 1f - float.Parse(indices[2], NumberStyles.Float)));
							else if (indices.Length == 2)
								texCoords.Add(new Vector2(float.Parse(indices[1], NumberStyles.Float), 1f));
							else
								texCoords.Add(new Vector2(0f, 1f));
							break;
						case "vn":
							if (indices.Length >= 4)
								normals.Add(new Vector3(float.Parse(indices[1], NumberStyles.Float), float.Parse(indices[2], NumberStyles.Float), float.Parse(indices[3], NumberStyles.Float)));
							else if (indices.Length == 3)
								normals.Add(new Vector3(float.Parse(indices[1], NumberStyles.Float), float.Parse(indices[2], NumberStyles.Float), 0f));
							else if (indices.Length == 2)
								normals.Add(new Vector3(float.Parse(indices[1], NumberStyles.Float), 0f, 0f));
							else
								normals.Add(new Vector3());
							break;
						case "f":
							if (indices.Length == 4) {
								points.Add(LoadFace(indices[1].Split(FaceSplitChar, StringSplitOptions.None), vertices.Count));
								points.Add(LoadFace(indices[2].Split(FaceSplitChar, StringSplitOptions.None), vertices.Count));
								points.Add(LoadFace(indices[3].Split(FaceSplitChar, StringSplitOptions.None), vertices.Count));
							} else if (indices.Length == 5) {
								temp1 = LoadFace(indices[1].Split(FaceSplitChar, StringSplitOptions.None), vertices.Count);
								points.Add(temp1);
								points.Add(LoadFace(indices[2].Split(FaceSplitChar, StringSplitOptions.None), vertices.Count));
								temp2 = LoadFace(indices[3].Split(FaceSplitChar, StringSplitOptions.None), vertices.Count);
								points.Add(temp2);
								points.Add(temp1);
								points.Add(temp2);
								points.Add(LoadFace(indices[4].Split(FaceSplitChar, StringSplitOptions.None), vertices.Count));
							} else {
								vertexIndices = new List<KeyValuePair<VertexIndex, Vertex>>(indices.Length - 1);
								for (i = 1; i < indices.Length; i++) {
									temp1 = LoadFace(indices[i].Split(FaceSplitChar, StringSplitOptions.None), vertices.Count);
									currentVertex = new Vertex();
									if (temp1.Pos < vertices.Count)
										currentVertex.Pos = vertices[temp1.Pos];
									if (temp1.TexPos < texCoords.Count)
										currentVertex.TexPos = texCoords[temp1.TexPos];
									if (temp1.Normal < normals.Count)
										currentVertex.Normal = normals[temp1.Normal];
									else
										currentVertex.Normal = Vector3.UnitY;
									vertexIndices.Add(new KeyValuePair<VertexIndex, Vertex>(temp1, currentVertex));
								}
								points.AddRange(MeshExtensions.TriangulatePolygon(vertexIndices));
							}
							break;
						case "o":
						case "g":
						case "usemtl":
							if (indices[0] == "usemtl")
								textures.Add(FindByMaterial(loadedTextures, indices[1]));
							else
								names.Enqueue(indices.Length == 1 ? string.Empty : line.Substring(line.IndexOf(indices[0], StringComparison.InvariantCultureIgnoreCase) + 2).Trim());
							if (points.Count != 0) {
								currText = textures == null || textures.Count == 0 ? null : (index < textures.Count ? textures[index] : textures[textures.Count - 1]);
								component = new MeshComponent(currText, MeshExtensions.ToVertexArray(vertices.ToArray(), texCoords.ToArray(), normals.ToArray(), points.ToArray()), OptimizeVertexDuplicates);
								if (names.Count != 0)
									component.Name = names.Dequeue();
								if (currText != null) {
									material = currText.Info as MtlParser.Material;
									if (material != null) {
										component.AmbientHue = material.AmbientHue;
										component.MaterialHue = material.MaterialHue;
										component.ShineHue = material.ShineHue;
										component.Shininess = material.Shininess;
									}
								}
								components.Add(component);
								index++;
								points = new List<VertexIndex>();
							}
							break;
						case "mtllib":
							parsedTextures = TextureParser.Parse(line.Substring(line.IndexOf("mtllib", StringComparison.InvariantCultureIgnoreCase) + 7).Trim());
							if (parsedTextures != null)
								loadedTextures.AddRange(parsedTextures);
							break;
						case "new":
							models.AddRange(Parse(reader, textures, loadedTextures));
							break;
						case "newmtl": //mtl
							if (current == null && material != null) {
								ITexture empty = new Texture2D() {
									Info = material
								};
								loadedTextures.Add(empty);
							} else
								current = null;
							material = new MtlParser.Material() {
								Name = line.Substring(line.IndexOf("newmtl", StringComparison.InvariantCultureIgnoreCase) + 7).Trim()
							};
							break;
						case "ns":
							if (material != null && indices.Length >= 2)
								material.Shininess = float.Parse(indices[1], NumberStyles.Float);
							break;
						case "ka":
							if (material != null) {
								if (indices.Length == 4)
									material.AmbientHue = new Color4(1f, float.Parse(indices[1], NumberStyles.Float), float.Parse(indices[2], NumberStyles.Float), float.Parse(indices[3], NumberStyles.Float));
								else if (indices.Length >= 5)
									material.AmbientHue = new Color4(float.Parse(indices[4], NumberStyles.Float), float.Parse(indices[1], NumberStyles.Float), float.Parse(indices[2], NumberStyles.Float), float.Parse(indices[3], NumberStyles.Float));
								if (material.AmbientHue.R == 0f && material.AmbientHue.G == 0f && material.AmbientHue.B == 0f) {
									material.AmbientHue.R = 0.15f;
									material.AmbientHue.G = 0.15f;
									material.AmbientHue.B = 0.15f;
								}
							}
							break;
						case "kd":
							if (material != null) {
								if (indices.Length == 4) {
									material.MaterialHue.R = float.Parse(indices[1], NumberStyles.Float);
									material.MaterialHue.G = float.Parse(indices[2], NumberStyles.Float);
									material.MaterialHue.B = float.Parse(indices[3], NumberStyles.Float);
								} else if (indices.Length >= 5)
									material.MaterialHue = new Color4(float.Parse(indices[4], NumberStyles.Float), float.Parse(indices[1], NumberStyles.Float), float.Parse(indices[2], NumberStyles.Float), float.Parse(indices[3], NumberStyles.Float));
								if (material.MaterialHue.R == 0f && material.MaterialHue.G == 0f && material.MaterialHue.B == 0f) {
									material.MaterialHue.R = 1f;
									material.MaterialHue.G = 1f;
									material.MaterialHue.B = 1f;
								}
							}
							break;
						case "ks":
							if (material != null) {
								if (indices.Length == 4)
									material.ShineHue = new Color4(1f, float.Parse(indices[1], NumberStyles.Float), float.Parse(indices[2], NumberStyles.Float), float.Parse(indices[3], NumberStyles.Float));
								else if (indices.Length >= 5)
									material.ShineHue = new Color4(float.Parse(indices[4], NumberStyles.Float), float.Parse(indices[1], NumberStyles.Float), float.Parse(indices[2], NumberStyles.Float), float.Parse(indices[3], NumberStyles.Float));
							}
							break;
						case "d":
							if (material != null && indices.Length >= 2) {
								material.MaterialHue.A = float.Parse(indices[1], NumberStyles.Float);
								if (material.MaterialHue.A == 0f)
									material.MaterialHue.A = 1f;
							}
							break;
						case "tr":
							if (material != null && indices.Length >= 2) {
								material.MaterialHue.A = 1f - float.Parse(indices[1], NumberStyles.Float);
								if (material.MaterialHue.A == 0f)
									material.MaterialHue.A = 1f;
							}
							break;
						case "map_kd":
							name = line.Substring(line.IndexOf("map_kd", StringComparison.InvariantCultureIgnoreCase) + 7).Trim();
							if (textureLookup.TryGetValue(name, out current)) {
								foreach (ITexture tex in current)
									tex.AddReference();
							} else {
								current = TextureParser.Parse(name);
								textureLookup.Add(name, current);
								if (current == null || current.Length == 0)
									current = null;
								else {
									try {
										name = Path.GetFileName(name);
									} catch {
										name = string.Empty;
									}
									foreach (ITexture tex in current) {
										tex.ID = name;
										tex.Info = material;
									}
								}
							}
							textures.AddRange(current);
							break;
					}
				}
			}
			if (current == null && material != null) {
				ITexture empty = new Texture2D() {
					Info = material
				};
				textures.Add(empty);
			}
			if (points.Count != 0) {
				currText = textures == null || textures.Count == 0 ? null : (index < textures.Count ? textures[index] : textures[textures.Count - 1]);
				component = new MeshComponent(currText, MeshExtensions.ToVertexArray(vertices.ToArray(), texCoords.ToArray(), normals.ToArray(), points.ToArray()), OptimizeVertexDuplicates);
				if (names.Count != 0)
					component.Name = names.Dequeue();
				if (currText != null) {
					material = currText.Info as MtlParser.Material;
					if (material != null) {
						component.AmbientHue = material.AmbientHue;
						component.MaterialHue = material.MaterialHue;
						component.ShineHue = material.ShineHue;
						component.Shininess = material.Shininess;
					}
				}
				components.Add(component);
			}
			if (components.Count != 0)
				models.Add(new Model(MeshExtensions.ToModelStructureArray(components)));
			return new Model(models);
		}

		private static ITexture FindByMaterial(List<ITexture> textures, string name) {
			if (name == null)
				return null;
			name = name.ToLower();
			MtlParser.Material material;
			for (int i = 0; i < textures.Count; i++) {
				material = textures[i].Info as MtlParser.Material;
				if (material != null && material.Name.ToLower() == name)
					return textures[i];
			}
			return null;
		}

		private static VertexIndex LoadFace(string[] faces, int currentVertexIndex) {
			VertexIndex vertex;
			switch (faces.Length) {
				case 1:
					vertex = new VertexIndex(faces[0].Length == 0 ? 0 : int.Parse(faces[0], NumberStyles.Integer) - 1);
					break;
				case 2:
					vertex = new VertexIndex(faces[0].Length == 0 ? 0 : int.Parse(faces[0], NumberStyles.Integer) - 1, faces[1].Length == 0 ? 0 : int.Parse(faces[1], NumberStyles.Integer) - 1);
					break;
				case 3:
					vertex = new VertexIndex(faces[0].Length == 0 ? 0 : int.Parse(faces[0], NumberStyles.Integer) - 1, faces[1].Length == 0 ? 0 : int.Parse(faces[1], NumberStyles.Integer) - 1, faces[2].Length == 0 ? 0 : int.Parse(faces[2], NumberStyles.Integer) - 1);
					break;
				default:
					return new VertexIndex();
			}
			if (vertex.Pos < 0) //relative indexing
				vertex.Pos += currentVertexIndex;
			if (vertex.TexPos < 0)
				vertex.TexPos += currentVertexIndex;
			if (vertex.Normal < 0)
				vertex.Normal += currentVertexIndex;
			return vertex;
		}


		/// <summary>
		/// Serializes the model structure into the specified stream
		/// </summary>
		/// <param name="structure">The structure to serialize</param>
		/// <param name="stream">The stream to serialize into</param>
		public static void Save(IModel structure, Stream stream) {
			List<object> vertices = new List<object>();
			Save(structure, vertices, new int[] { 0 });
			int[] texCoordsIndex;
			int[] normalsIndex;
			int i;
			int j;
			bool found = false;
			int length = vertices.Count;
			int actualLength = 0;
			Vector3 temp;
			object[] vertexIndex = new object[length];
			Vector3[] optimizedVertices = new Vector3[length];
			for (i = 0; i < length; i++) {
				if (vertices[i] is Vertex) {
					temp = ((Vertex) vertices[i]).Pos;
					for (j = 0; j < actualLength; j++) {
						if (temp == optimizedVertices[j]) {
							vertexIndex[i] = j + 1;
							found = true;
							break;
						}
					}
					if (found)
						found = false;
					else {
						optimizedVertices[actualLength] = temp;
						vertexIndex[i] = actualLength + 1;
						actualLength++;
					}
				} else
					vertexIndex[i] = vertices[i];
			}
			Vector3[] vertexArray = new Vector3[actualLength];
			Array.Copy(optimizedVertices, vertexArray, actualLength);
			found = false;
			actualLength = 0;
			Vector2 temp2;
			texCoordsIndex = new int[length];
			Vector2[] optimizedTexCoords = new Vector2[length];
			for (i = 0; i < length; i++) {
				if (vertices[i] is Vertex) {
					temp2 = ((Vertex) vertices[i]).TexPos;
					for (j = 0; j < actualLength; j++) {
						if (temp2 == optimizedTexCoords[j]) {
							texCoordsIndex[i] = j + 1;
							found = true;
							break;
						}
					}
					if (found)
						found = false;
					else {
						optimizedTexCoords[actualLength] = temp2;
						texCoordsIndex[i] = actualLength + 1;
						actualLength++;
					}
				} else
					texCoordsIndex[i] = -1;
			}
			Vector2[] texArray = new Vector2[actualLength];
			Array.Copy(optimizedTexCoords, texArray, actualLength);
			found = false;
			actualLength = 0;
			normalsIndex = new int[length];
			Vector3[] optimizedNormals = new Vector3[length];
			for (i = 0; i < length; i++) {
				if (vertices[i] is Vertex) {
					temp = ((Vertex) vertices[i]).Normal;
					for (j = 0; j < actualLength; j++) {
						if (temp == optimizedNormals[j]) {
							normalsIndex[i] = j + 1;
							found = true;
							break;
						}
					}
					if (found)
						found = false;
					else {
						optimizedNormals[actualLength] = temp;
						normalsIndex[i] = actualLength + 1;
						actualLength++;
					}
				} else
					normalsIndex[i] = -1;
			}
			Vector3[] normalArray = new Vector3[actualLength];
			Array.Copy(optimizedNormals, normalArray, actualLength);
			using (StreamWriter writer = new StreamWriter(stream)) {
				writer.Write("#Exported from Ablaze.Net\n\n");
				for (i = 0; i < vertexArray.Length; i++) {
					writer.Write("v ");
					writer.Write((decimal) vertexArray[i].X);
					writer.Write(' ');
					writer.Write((decimal) vertexArray[i].Y);
					writer.Write(' ');
					writer.Write((decimal) vertexArray[i].Z);
					writer.Write('\n');
				}
				for (i = 0; i < texArray.Length; i++) {
					writer.Write("vt ");
					writer.Write((decimal) texArray[i].X);
					writer.Write(' ');
					writer.Write((decimal) (1f - texArray[i].Y));
					writer.Write('\n');
				}
				for (i = 0; i < normalArray.Length; i++) {
					writer.Write("vn ");
					writer.Write((decimal) normalArray[i].X);
					writer.Write(' ');
					writer.Write((decimal) normalArray[i].Y);
					writer.Write(' ');
					writer.Write((decimal) normalArray[i].Z);
					writer.Write('\n');
				}
				i = 0;
				while (i < vertexIndex.Length - 2) {
					if (vertexIndex[i] is int) {
						writer.Write("f ");
						writer.Write((int) vertexIndex[i]);
						writer.Write('/');
						writer.Write(texCoordsIndex[i]);
						writer.Write('/');
						writer.Write(normalsIndex[i]);
						i++;
						writer.Write(' ');
						writer.Write((int) vertexIndex[i]);
						writer.Write('/');
						writer.Write(texCoordsIndex[i]);
						writer.Write('/');
						writer.Write(normalsIndex[i]);
						i++;
						writer.Write(' ');
						writer.Write((int) vertexIndex[i]);
						writer.Write('/');
						writer.Write(texCoordsIndex[i]);
						writer.Write('/');
						writer.Write(normalsIndex[i]);
						i++;
						writer.Write('\n');
					} else {
						writer.Write((string) vertexIndex[i]);
						writer.Write('\n');
						i++;
					}
				}
			}
		}

		private static void Save(IModel structure, List<object> vertices, int[] counter) {
			MeshComponent component = structure as MeshComponent;
			if (component == null) {
				foreach (IModel comp in ((Model) structure))
					Save(comp, vertices, counter);
			} else {
				if (component.IndexBuffer.Count == 0)
					return;
				counter[0]++;
				string name;
				if (component.Name == null || (name = component.Name.Trim()).Length == 0)
					vertices.Add("g Component " + counter[0]);
				else
					vertices.Add("g " + name);
				foreach (Vertex vertex in MeshExtensions.ToVertexArray(component.BufferData, component.IndexBuffer.Indices))
					vertices.Add(vertex);
			}
		}
	}
}