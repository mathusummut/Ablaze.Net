#if !NET35
using System.Collections.Concurrent;
#endif
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace System.Graphics.Models.Parsers {
	/// <summary>
	/// Contains methods for parsing 3D models from files.
	/// </summary>
	public static class ModelParser {
		private static Dictionary<string, ValueTuple<Func<Stream, ITexture[], Model>, Action<IModel, Stream>>> ParserDictionary;

		static ModelParser() {
			Assembly[] assemblies;
			try {
				assemblies = AppDomain.CurrentDomain.GetAssemblies();
			} catch {
				ParserDictionary = new Dictionary<string, ValueTuple<Func<Stream, ITexture[], Model>, Action<IModel, Stream>>>(StringComparer.Ordinal);
				return;
			}
			Type typeOfParser = typeof(Func<Stream, ITexture[], Model>);
			Type typeOfSave = typeof(Action<IModel, Stream>);
			Type[] parserSignature = new Type[] { typeof(Stream), typeof(ITexture[]) };
			Type[] saveSignature = new Type[] { typeof(IModel), typeof(Stream) };
#if NET35
			Dictionary<string, ValueTuple<Func<Stream, ITexture[], Model>, Action<IModel, Stream>>> collection = new Dictionary<string, ValueTuple<Func<Stream, ITexture[], Model>, Action<IModel, Stream>>>(StringComparer.Ordinal);
			foreach (Assembly assembly in assemblies) {
#else
			ConcurrentDictionary<string, ValueTuple<Func<Stream, ITexture[], Model>, Action<IModel, Stream>>> collection = new ConcurrentDictionary<string, ValueTuple<Func<Stream, ITexture[], Model>, Action<IModel, Stream>>>(StringComparer.Ordinal);
			Parallel.ForEach(assemblies, assembly => {
#endif
				try {
#if NET35
					foreach (Type type in assembly.GetTypes()) {
#else
					Parallel.ForEach(assembly.GetTypes(), type => {
#endif
						try {
							object[] attributes = type.GetCustomAttributes(ModelParserAttribute.Type, false);
							if (!(attributes == null || attributes.Length == 0)) {
								ModelParserAttribute attr = attributes[0] as ModelParserAttribute;
								if (attr != null) {
									MethodInfo method;
									Func<Stream, ITexture[], Model> parse;
									Action<IModel, Stream> save;
									foreach (KeyValuePair<string, ValueTuple<string, string>> pair in attr.Extensions) {
										parse = null;
										if (pair.Value.Item1.Length != 0) {
											try {
												method = type.GetMethod(pair.Value.Item1, BindingFlags.Public | BindingFlags.Static, null, parserSignature, null);
												if (method != null)
													parse = (Func<Stream, ITexture[], Model>) Delegate.CreateDelegate(typeOfParser, null, method);
											} catch {
											}
										}
										save = null;
										if (pair.Value.Item2.Length != 0) {
											try {
												method = type.GetMethod(pair.Value.Item2, BindingFlags.Public | BindingFlags.Static, null, saveSignature, null);
												if (method != null)
													save = (Action<IModel, Stream>) Delegate.CreateDelegate(typeOfSave, null, method);
											} catch {
											}
										}
#if NET35
										collection.Add(pair.Key, new ValueTuple<Func<Stream, ITexture[], Model>, Action<IModel, Stream>>(parse, save));
#else
										collection.TryAdd(pair.Key, new ValueTuple<Func<Stream, ITexture[], Model>, Action<IModel, Stream>>(parse, save));
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
			ParserDictionary = new Dictionary<string, ValueTuple<Func<Stream, ITexture[], Model>, Action<IModel, Stream>>>(collection, StringComparer.Ordinal);
#endif
		}

		/// <summary>
		/// Parses a 3D model.
		/// </summary>
		/// <param name="mesh">The stream containing the mesh data.</param>
		/// <param name="encoding">The file extension representing the mesh encoding.</param>
		/// <returns>A list of models with all the parsed components.</returns>
		public static Model Parse(Stream mesh, string encoding) {
			return Parse(mesh, encoding, null as ITexture[]);
		}

		/// <summary>
		/// Parses a 3D model.
		/// </summary>
		/// <param name="mesh">The location of the file to parse the mesh from.</param>
		/// <returns>A list of models with all the parsed components.</returns>
		public static Model Parse(string mesh) {
			if (mesh == null)
				return null;
			using (BufferedStream file = FileUtils.LoadFileBuffered(mesh, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				return Parse(file, Path.GetExtension(mesh), null as ITexture[]);
		}

		/// <summary>
		/// Parses a 3D model.
		/// </summary>
		/// <param name="mesh">The location of the file to parse the mesh from.</param>
		/// <param name="textures">The locations of the textures involved (can be null or empty).</param>
		/// <returns>A list of models with all the parsed components.</returns>
		public static Model Parse(string mesh, params string[] textures) {
			if (mesh == null)
				return null;
			using (BufferedStream file = FileUtils.LoadFileBuffered(mesh, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				return Parse(file, Path.GetExtension(mesh), Texture2D.ToTextures(textures));
		}

		/// <summary>
		/// Parses a 3D model
		/// </summary>
		/// <param name="mesh">The location of the file to parse the mesh from</param>
		/// <param name="bindAction">Determines what to do with the image after binding into GPU memory</param>
		/// <param name="textures">The images to use as textures (can be null or empty)</param>
		/// <returns>A list of models with all the parsed components</returns>
		public static Model Parse(string mesh, ImageParameterAction bindAction, params Bitmap[] textures) {
			if (mesh == null)
				return null;
			using (BufferedStream file = FileUtils.LoadFileBuffered(mesh, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				return Parse(file, Path.GetExtension(mesh), Texture2D.ToTextures(bindAction, textures));
		}

		/// <summary>
		/// Parses a 3D model.
		/// </summary>
		/// <param name="mesh">The location of the file to parse the mesh from.</param>
		/// <param name="textures">The textures to use (can be null or empty).</param>
		/// <returns>A list of models with all the parsed components.</returns>
		public static Model Parse(string mesh, params ITexture[] textures) {
			if (mesh == null)
				return null;
			using (BufferedStream file = FileUtils.LoadFileBuffered(mesh, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				return Parse(file, Path.GetExtension(mesh), textures);
		}

		/// <summary>
		/// Parses a 3D model.
		/// </summary>
		/// <param name="mesh">The stream containing the mesh data.</param>
		/// <param name="encoding">The file extension representing the mesh encoding.</param>
		/// <param name="textures">The locations of the textures involved (can be null or empty).</param>
		/// <returns>A list of models with all the parsed components.</returns>
		public static Model Parse(Stream mesh, string encoding, params string[] textures) {
			return Parse(mesh, encoding, Texture2D.ToTextures(textures));
		}

		/// <summary>
		/// Parses a 3D model
		/// </summary>
		/// <param name="mesh">The stream containing the mesh data</param>
		/// <param name="encoding">The file extension representing the mesh encoding</param>
		/// <param name="bindAction">Determines what to do with the image after binding into GPU memory</param>
		/// <param name="textures">The bitmap images to use as textures (can be null)</param>
		/// <returns>A list of models with all the parsed components</returns>
		public static Model Parse(Stream mesh, string encoding, ImageParameterAction bindAction, params Bitmap[] textures) {
			return Parse(mesh, encoding, Texture2D.ToTextures(bindAction, textures));
		}

		/// <summary>
		/// Parses a 3D model.
		/// </summary>
		/// <param name="mesh">The stream containing the mesh data.</param>
		/// <param name="encoding">The file extension representing the mesh encoding.</param>
		/// <param name="textures">The textures for the model to use (can be null or empty).</param>
		/// <returns>A list of models with all the parsed components.</returns>
		public static Model Parse(Stream mesh, string encoding, params ITexture[] textures) {
			if (mesh == null || encoding == null || mesh.Position == mesh.Length)
				return null;
			encoding = encoding.Trim().ToLower();
			if (encoding.Length != 0 && encoding[0] == '.')
				encoding = encoding.Substring(1);
			ValueTuple<Func<Stream, ITexture[], Model>, Action<IModel, Stream>> pair;
			if (!ParserDictionary.TryGetValue(encoding, out pair) || pair.Item1 == null)
				throw new NotImplementedException("No " + encoding + " parser has been found (the class must be marked with ModelParserAttribute).");
			Model models = pair.Item1(mesh, textures);
			FileStream str = mesh as FileStream;
			string name = string.Empty;
			if (str != null)
				name = str.Name;
			if (models.Name == null)
				models.Name = name;
			return models;
		}

		/// <summary>
		/// Saves the specified component into the specified stream, using the specified encoding.
		/// </summary>
		/// <param name="component">The component to save.</param>
		/// <param name="saveStream">The stream to save in.</param>
		/// <param name="encoding">The encoding to use.</param>
		public static void Save(IModel component, Stream saveStream, string encoding) {
			if (component == null || saveStream == null || encoding == null)
				return;
			encoding = encoding.Trim().ToLower();
			if (encoding.Length != 0 && encoding[0] == '.')
				encoding = encoding.Substring(1);
			ValueTuple<Func<Stream, ITexture[], Model>, Action<IModel, Stream>> pair;
			if (!ParserDictionary.TryGetValue(encoding, out pair) || pair.Item2 == null)
				throw new NotImplementedException("No " + encoding + " serializer handler has been found (the class must be marked with ModelParserAttribute).");
			pair.Item2(component, saveStream);
		}
	}
}