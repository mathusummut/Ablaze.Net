#if !NET35
using System.Collections.Concurrent;
#endif
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace System.Graphics.Models.Parsers {
	/// <summary>
	/// Contains methods for parsing textures from files.
	/// </summary>
	public static class TextureParser {
		private static Dictionary<string, Func<Stream, ITexture[]>> ParserDictionary;

		static TextureParser() {
			Assembly[] assemblies;
			try {
				assemblies = AppDomain.CurrentDomain.GetAssemblies();
			} catch {
				ParserDictionary = new Dictionary<string, Func<Stream, ITexture[]>>(StringComparer.Ordinal);
				return;
			}
			Type typeOfParser = typeof(Func<Stream, ITexture[]>);
			Type[] parserSignature = new Type[] { typeof(Stream) };
#if NET35
			Dictionary<string, Func<Stream, ITexture[]>> collection = new Dictionary<string, Func<Stream, ITexture[]>>(StringComparer.Ordinal);
			foreach (Assembly assembly in assemblies) {
#else
			ConcurrentDictionary<string, Func<Stream, ITexture[]>> collection = new ConcurrentDictionary<string, Func<Stream, ITexture[]>>(StringComparer.Ordinal);
			Parallel.ForEach(assemblies, assembly => {
#endif
				try {
#if NET35
					foreach (Type type in assembly.GetTypes()) {
#else
					Parallel.ForEach(assembly.GetTypes(), type => {
#endif
						try {
							object[] attributes = type.GetCustomAttributes(TextureParserAttribute.Type, false);
							if (!(attributes == null || attributes.Length == 0)) {
								TextureParserAttribute attr = attributes[0] as TextureParserAttribute;
								if (attr != null) {
									Func<Stream, ITexture[]> parse;
									MethodInfo method;
									foreach (KeyValuePair<string, string> pair in attr.Extensions) {
										parse = null;
										if (pair.Value.Length != 0) {
											try {
												method = type.GetMethod(pair.Value, BindingFlags.Public | BindingFlags.Static, null, parserSignature, null);
												if (method != null)
													parse = (Func<Stream, ITexture[]>) Delegate.CreateDelegate(typeOfParser, null, method);
											} catch {
											}
										}
#if NET35
										collection.Add(pair.Key, parse);
#else
										collection.TryAdd(pair.Key, parse);
#endif
									}
								}
							}
						} catch {
						}
					}
#if !NET35
					);
#endif
				} catch {
				}
			}
#if NET35
			ParserDictionary = collection;
#else
			);
			ParserDictionary = new Dictionary<string, Func<Stream, ITexture[]>>(collection, StringComparer.Ordinal);
#endif
		}

		/// <summary>
		/// Parses textures.
		/// </summary>
		/// <param name="source">The location of the file to parse the texture from.</param>
		/// <returns>A list of the textures parsed.</returns>
		public static ITexture[] Parse(string source) {
			if (source == null)
				return null;
			using (BufferedStream file = FileUtils.LoadFileBuffered(source, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				return Parse(file, Path.GetExtension(source));
		}

		/// <summary>
		/// Parses textures.
		/// </summary>
		/// <param name="source">The stream to parse the texture from.</param>
		/// <param name="encoding">The file extension representing the texture encoding.</param>
		/// <returns>A list of the textures parsed.</returns>
		public static ITexture[] Parse(Stream source, string encoding) {
			if (source == null || encoding == null)
				return null;
			encoding = encoding.Trim().ToLower();
			if (encoding.Length != 0 && encoding[0] == '.')
				encoding = encoding.Substring(1);
			Func<Stream, ITexture[]> func;
			if (!ParserDictionary.TryGetValue(encoding, out func) || func == null)
				throw new NotImplementedException("No " + encoding + " parser files has been found (the class must be marked with TextureParserAttribute).");
			ITexture[] textures = func(source);
			FileStream str = source as FileStream;
			string name = string.Empty;
			if (str != null)
				name = Path.GetFileName(str.Name);
			for (int i = 0; i < textures.Length; i++) {
				if (textures[i].ID == null)
					textures[i].ID = name + i;
			}
			return textures;
		}
	}
}