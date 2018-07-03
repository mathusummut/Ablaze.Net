using System.IO;

namespace System.Text {
	/// <summary>Contains functions to load and save in the Hjson format.</summary>
	public static class HjsonValue {
		/// <summary>Loads Hjson/JSON from a file.</summary>
		public static JsonObject Load(string path) {
			return load(path, null, null);
		}

		/// <summary>Loads Hjson/JSON from a file, optionally preserving whitespace and comments.</summary>
		public static JsonObject Load(string path, HjsonOptions options) {
			return load(path, null, options);
		}

		/// <summary>Loads Hjson/JSON from a stream.</summary>
		public static JsonObject Load(Stream stream) {
			return load(stream, null, null);
		}

		/// <summary>Loads Hjson/JSON from a stream, optionally preserving whitespace and comments.</summary>
		public static JsonObject Load(Stream stream, HjsonOptions options) {
			return load(stream, null, options);
		}

		/// <summary>Loads Hjson/JSON from a TextReader.</summary>
		public static JsonObject Load(TextReader textReader, IJsonReader jsonReader = null) {
			return load(textReader, jsonReader, null);
		}

		/// <summary>Loads Hjson/JSON from a TextReader, optionally preserving whitespace and comments.</summary>
		public static JsonObject Load(TextReader textReader, HjsonOptions options, IJsonReader jsonReader = null) {
			return load(textReader, jsonReader, options);
		}

		static JsonObject load(string path, IJsonReader jsonReader, HjsonOptions options) {
			if (Path.GetExtension(path).ToLower() == ".json")
				return JsonObject.Load(path);
			try {
				using (FileStream s = File.OpenRead(path))
					return load(s, jsonReader, options);
			} catch (Exception e) {
				throw new Exception(e.Message + " (in " + path + ")", e);
			}
		}

		static JsonObject load(Stream stream, IJsonReader jsonReader, HjsonOptions options) {
			return load(new StreamReader(stream, true), jsonReader, options);
		}

		static JsonObject load(TextReader textReader, IJsonReader jsonReader, HjsonOptions options) {
			return new HjsonReader(textReader, jsonReader, options).Read();
		}

		/// <summary>Parses the specified Hjson/JSON string.</summary>
		public static JsonObject Parse(string hjsonString) {
			return Load(new StringReader(hjsonString));
		}

		/// <summary>Parses the specified Hjson/JSON string, optionally preserving whitespace and comments.</summary>
		public static JsonObject Parse(string hjsonString, HjsonOptions options) {
			return Load(new StringReader(hjsonString), options);
		}

		/// <summary>Saves Hjson to a file.</summary>
		public static void Save(JsonObject json, string path, HjsonOptions options = null) {
			if (Path.GetExtension(path).ToLower() == ".json")
				json.Save(path, Stringify.Formatted);
			else {
				using (StreamWriter s = File.CreateText(path))
					Save(json, s, options);
			}
		}

		/// <summary>Saves Hjson to a stream.</summary>
		public static void Save(JsonObject json, Stream stream, HjsonOptions options = null) {
			Save(json, new StreamWriter(stream), options);
		}

		/// <summary>Saves Hjson to a TextWriter.</summary>
		public static void Save(JsonObject json, TextWriter textWriter, HjsonOptions options = null) {
			new HjsonWriter(options).Save(json, textWriter, 0, false, string.Empty, true, true);
			textWriter.Flush();
		}

		internal static bool IsPunctuatorChar(char ch) {
			return ch == '{' || ch == '}' || ch == '[' || ch == ']' || ch == ',' || ch == ':' || ch == '=';
		}
	}
}