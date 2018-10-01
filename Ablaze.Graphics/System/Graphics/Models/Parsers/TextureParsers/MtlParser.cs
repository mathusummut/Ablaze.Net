using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;

namespace System.Graphics.Models.Parsers.TextureParsers {
	/// <summary>
	/// Contains methods for parsing Mtl files.
	/// </summary>
	[TextureParser("mtl,Parse")]
	public static class MtlParser {
		/// <summary>
		/// Parses textures.
		/// </summary>
		/// <param name="source">The location of the file to parse the textures from.</param>
		/// <returns>A list of the textures parsed.</returns>
		public static ITexture[] Parse(Stream source) {
			List<ITexture> textures = new List<ITexture>();
			Material material = null;
			ITexture[] current = null;
			string[] indices;
			string line;
			string name;
			int commentIndex;
			Dictionary<string, ITexture[]> textureLookup = new Dictionary<string, ITexture[]>(StringComparer.OrdinalIgnoreCase);
			using (StreamReader reader = new StreamReader(source)) {
				while ((line = reader.ReadLine()) != null) {
					commentIndex = line.IndexOf('#');
					if (commentIndex != -1)
						line = line.Substring(0, commentIndex);
					indices = line.Trim().Split(ObjParser.SplitChar, StringSplitOptions.RemoveEmptyEntries);
					if (indices.Length != 0) {
						switch (indices[0].ToLower()) {
							case "newmtl":
								if (current == null && material != null) {
									ITexture empty = new Texture2D() {
										Info = material
									};
									textures.Add(empty);
								} else
									current = null;
								material = new Material() {
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
										material.AmbientHue = new ColorF(1f, float.Parse(indices[1], NumberStyles.Float), float.Parse(indices[2], NumberStyles.Float), float.Parse(indices[3], NumberStyles.Float));
									else if (indices.Length >= 5)
										material.AmbientHue = new ColorF(float.Parse(indices[4], NumberStyles.Float), float.Parse(indices[1], NumberStyles.Float), float.Parse(indices[2], NumberStyles.Float), float.Parse(indices[3], NumberStyles.Float));
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
										material.MaterialHue = new ColorF(float.Parse(indices[4], NumberStyles.Float), float.Parse(indices[1], NumberStyles.Float), float.Parse(indices[2], NumberStyles.Float), float.Parse(indices[3], NumberStyles.Float));
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
										material.ShineHue = new ColorF(1f, float.Parse(indices[1], NumberStyles.Float), float.Parse(indices[2], NumberStyles.Float), float.Parse(indices[3], NumberStyles.Float));
									else if (indices.Length >= 5)
										material.ShineHue = new ColorF(float.Parse(indices[4], NumberStyles.Float), float.Parse(indices[1], NumberStyles.Float), float.Parse(indices[2], NumberStyles.Float), float.Parse(indices[3], NumberStyles.Float));
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
			}
			if (current == null && material != null) {
				ITexture empty = new Texture2D() {
					Info = material
				};
				textures.Add(empty);
			}
			return textures.ToArray();
		}

		internal sealed class Material {
			/// <summary>
			/// The name of the material.
			/// </summary>
			public string Name;
			/// <summary>
			/// The component's hue and opacity.
			/// </summary>
			public ColorF MaterialHue = Light.DefaultMaterialHue;
			/// <summary>
			/// The hue of the ambient light that hits the object.
			/// </summary>
			public ColorF AmbientHue = Light.DefaultAmbientHue;
			/// <summary>
			/// The hue of the reflective shine of the object.
			/// </summary>
			public ColorF ShineHue = Light.DefaultShineHue;
			/// <summary>
			/// The shininess exponent of the material.
			/// </summary>
			public float Shininess = Light.DefaultShininess;

			public override string ToString() {
				return string.Format("{0} => {{ material hue: {1}, ambient hue: {2}, shine hue: {3}, shininess: {4} }}", Name, MaterialHue, AmbientHue, ShineHue, Shininess);
			}
		}
	}
}