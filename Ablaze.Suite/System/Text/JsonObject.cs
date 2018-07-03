using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace System.Text {
	using JsonPair = KeyValuePair<string, JsonObject>;

	/// <summary>The ToString format.</summary>
	public enum Stringify {
		/// <summary>JSON (no whitespace).</summary>
		Plain,
		/// <summary>Formatted JSON.</summary>
		Formatted,
		/// <summary>Hjson.</summary>
		Hjson,
	}

	/// <summary>Implements an object value.</summary>
	public class JsonObject : IDictionary<string, JsonObject>, ICollection<JsonPair> {
		/// <summary>
		/// The default key comparer used by the key dictionary.
		/// </summary>
		public static StringComparer KeyComparer = StringComparer.OrdinalIgnoreCase;
		internal static string eol = Environment.NewLine;
		Dictionary<string, JsonObject> map;

		/// <summary>Gets or sets the newline charater(s).</summary>
		/// <remarks>Defaults to Environment.NewLine.</remarks>
		public static string Eol {
			get {
				return eol;
			}
			set {
				if (value == "\r\n" || value == "\n")
					eol = value;
			}
		}

		/// <summary>
		/// Use this if you don't want to make use of the underlying dictionary and manage the items yourself.
		/// </summary>
		protected JsonObject() {
		}

		/// <summary>Initializes a new instance of this class.</summary>
		/// <remarks>You can also initialize an object using the C# add syntax: new JsonObject { { "key", "value" }, ... }</remarks>
		public JsonObject(params JsonPair[] items) {
			map = new Dictionary<string, JsonObject>(KeyComparer);
			if (items != null)
				AddRange(items);
		}

		/// <summary>Initializes a new instance of this class.</summary>
		/// <remarks>You can also initialize an object using the C# add syntax: new JsonObject { { "key", "value" }, ... }</remarks>
		public JsonObject(IEnumerable<JsonPair> items) {
			map = new Dictionary<string, JsonObject>(KeyComparer);
			AddRange(items);
		}

		/// <summary>Gets the count of the contained items.</summary>
		public virtual int Count {
			get {
				return map.Count;
			}
		}

		IEnumerator<JsonPair> IEnumerable<JsonPair>.GetEnumerator() {
			return map.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return map.GetEnumerator();
		}

		/// <summary>Gets or sets the value for the specified key.</summary>
		public virtual JsonObject this[string key] {
			get {
				return map[key];
			}
			set {
				map[key] = value;
			}
		}

		/// <summary>Gets or sets the value for the specified key.</summary>
		public virtual JsonObject this[int index] {
			get {
				return map[index.ToString()];
			}
			set {
				map[index.ToString()] = value;
			}
		}

		/// <summary>The type of this value.</summary>
		public virtual JsonType JsonType {
			get {
				return JsonType.Object;
			}
		}

		/// <summary>Gets the keys of this object.</summary>
		public ICollection<string> Keys {
			get {
				return map.Keys;
			}
		}

		/// <summary>Gets the values of this object.</summary>
		public ICollection<JsonObject> Values {
			get {
				return map.Values;
			}
		}

		/// <summary>Adds a new item.</summary>
		/// <remarks>You can also initialize an object using the C# add syntax: new JsonObject { { "key", "value" }, ... }</remarks>
		public void Add(string key, JsonObject value) {
			map[key] = value; // json allows duplicate keys
		}

		/// <summary>Adds a new item.</summary>
		public void Add(JsonPair pair) {
			Add(pair.Key, pair.Value);
		}

		/// <summary>Adds a range of items.</summary>
		public void AddRange(IEnumerable<JsonPair> items) {
			foreach (var pair in items)
				Add(pair);
		}

		/// <summary>Clears the object.</summary>
		public virtual void Clear() {
			map.Clear();
		}

		bool ICollection<JsonPair>.Contains(JsonPair item) {
			return (map as ICollection<JsonPair>).Contains(item);
		}

		bool ICollection<JsonPair>.Remove(JsonPair item) {
			return (map as ICollection<JsonPair>).Remove(item);
		}

		/// <summary>Determines whether the array contains a specific key.</summary>
		public virtual bool ContainsKey(string key) {
			return map.ContainsKey(key);
		}

		/// <summary>Copies the elements to an System.Array, starting at a particular System.Array index.</summary>
		public void CopyTo(JsonPair[] array, int arrayIndex) {
			(map as ICollection<JsonPair>).CopyTo(array, arrayIndex);
		}

		/// <summary>Removes the item with the specified key.</summary>
		/// <param name="key">The key of the element to remove.</param>
		/// <returns>true if the element is successfully found and removed; otherwise, false.</returns>
		public bool Remove(string key) {
			return map.Remove(key);
		}

		bool ICollection<JsonPair>.IsReadOnly {
			get {
				return false;
			}
		}

		/// <summary>Gets the value associated with the specified key.</summary>
		public bool TryGetValue(string key, out JsonObject value) {
			return map.TryGetValue(key, out value);
		}

		void ICollection<JsonPair>.Add(JsonPair item) {
			this.Add(item);
		}

		void ICollection<JsonPair>.Clear() {
			this.Clear();
		}

		void ICollection<JsonPair>.CopyTo(JsonPair[] array, int arrayIndex) {
			this.CopyTo(array, arrayIndex);
		}

		int ICollection<JsonPair>.Count {
			get {
				return this.Count;
			}
		}

		/// <summary>Saves the JSON to a file.</summary>
		public void Save(string path, Stringify format = Stringify.Plain) {
			using (var s = File.CreateText(path))
				Save(s, format);
		}

		/// <summary>Saves the JSON to a stream.</summary>
		public void Save(Stream stream, Stringify format = Stringify.Plain) {
			Save(new StreamWriter(stream), format);
		}

		/// <summary>Saves the JSON to a TextWriter.</summary>
		public void Save(TextWriter textWriter, Stringify format = Stringify.Plain) {
			if (textWriter == null)
				throw new ArgumentNullException(nameof(textWriter));
			if (format == Stringify.Hjson)
				HjsonValue.Save(this, textWriter);
			else
				new JsonWriter(format == Stringify.Formatted).Save(this, textWriter, 0);
			textWriter.Flush();
		}

		/// <summary>Saves as Hjson to a string.</summary>
		public string ToString(HjsonOptions options) {
			var sw = new StringWriter();
			HjsonValue.Save(this, sw, options);
			return sw.ToString();
		}

		/// <summary>Saves the JSON to a string.</summary>
		public string ToString(Stringify format) {
			var sw = new StringWriter();
			Save(sw, format);
			return sw.ToString();
		}

		/// <summary>Saves the JSON to a string.</summary>
		public override string ToString() {
			return ToString(Stringify.Plain);
		}

		/// <summary>Returns the contained primitive value.</summary>
		public object ToValue() {
			return ((JsonPrimitive) this).Value;
		}

		/// <summary>Wraps an unknown object into a JSON value (to be used with DSF).</summary>
		public static JsonObject FromObject(object value) {
			return JsonPrimitive.FromObject(value);
		}

		/// <summary>Loads JSON from a file.</summary>
		public static JsonObject Load(string path) {
			using (FileStream s = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				return Load(s);
		}

		/// <summary>Loads JSON from a stream.</summary>
		public static JsonObject Load(Stream stream, bool disposeStream = true) {
			return Load(new StreamReader(stream, true), disposeStream);
		}

		/// <summary>Loads JSON from a TextReader.</summary>
		public static JsonObject Load(TextReader textReader, bool disposeStream = true, IJsonReader jsonReader = null) {
			var ret = new JsonReader(textReader, jsonReader, disposeStream).Read();
			return ret;
		}

		/// <summary>Parses the specified JSON string.</summary>
		public static JsonObject Parse(string jsonString, bool disposeStream = true) {
			return Load(new StringReader(jsonString), disposeStream);
		}

		// CLI -> JsonObject

		/// <summary>Converts from bool.</summary>
		public static implicit operator JsonObject(bool value) {
			return new JsonPrimitive(value);
		}
		/// <summary>Converts from byte.</summary>
		public static implicit operator JsonObject(byte value) {
			return new JsonPrimitive(value);
		}
		/// <summary>Converts from char.</summary>
		public static implicit operator JsonObject(char value) {
			return new JsonPrimitive(value);
		}
		/// <summary>Converts from decimal.</summary>
		public static implicit operator JsonObject(decimal value) {
			return new JsonPrimitive(value);
		}
		/// <summary>Converts from double.</summary>
		public static implicit operator JsonObject(double value) {
			return new JsonPrimitive(value);
		}
		/// <summary>Converts from float.</summary>
		public static implicit operator JsonObject(float value) {
			return new JsonPrimitive(value);
		}
		/// <summary>Converts from int.</summary>
		public static implicit operator JsonObject(int value) {
			return new JsonPrimitive(value);
		}
		/// <summary>Converts from long.</summary>
		public static implicit operator JsonObject(long value) {
			return new JsonPrimitive(value);
		}
		/// <summary>Converts from short.</summary>
		public static implicit operator JsonObject(short value) {
			return new JsonPrimitive(value);
		}
		/// <summary>Converts from string.</summary>
		public static implicit operator JsonObject(string value) {
			return new JsonPrimitive(value);
		}

		// JsonObject -> CLI

		/// <summary>Converts to bool. Also see <see cref="JsonUtil"/>.</summary>
		public static explicit operator bool(JsonObject value) {
			if (value == null)
				throw new ArgumentNullException("value");
			return Convert.ToBoolean(((JsonPrimitive) value).Value);
		}

		/// <summary>Converts to byte. Also see <see cref="JsonUtil"/>.</summary>
		public static explicit operator byte(JsonObject value) {
			if (value == null)
				throw new ArgumentNullException("value");
			return Convert.ToByte(((JsonPrimitive) value).Value);
		}

		/// <summary>Converts to char. Also see <see cref="JsonUtil"/>.</summary>
		public static explicit operator char(JsonObject value) {
			if (value == null)
				throw new ArgumentNullException("value");
			return Convert.ToChar(((JsonPrimitive) value).Value);
		}

		/// <summary>Converts to decimal. Also see <see cref="JsonUtil"/>.</summary>
		public static explicit operator decimal(JsonObject value) {
			if (value == null)
				throw new ArgumentNullException("value");
			return Convert.ToDecimal(((JsonPrimitive) value).Value);
		}

		/// <summary>Converts to double. Also see <see cref="JsonUtil"/>.</summary>
		public static explicit operator double(JsonObject value) {
			if (value == null)
				throw new ArgumentNullException("value");
			return Convert.ToDouble(((JsonPrimitive) value).Value);
		}

		/// <summary>Converts to float. Also see <see cref="JsonUtil"/>.</summary>
		public static explicit operator float(JsonObject value) {
			if (value == null)
				throw new ArgumentNullException("value");
			return Convert.ToSingle(((JsonPrimitive) value).Value);
		}

		/// <summary>Converts to int. Also see <see cref="JsonUtil"/>.</summary>
		public static explicit operator int(JsonObject value) {
			if (value == null)
				throw new ArgumentNullException("value");
			return Convert.ToInt32(((JsonPrimitive) value).Value);
		}

		/// <summary>Converts to long. Also see <see cref="JsonUtil"/>.</summary>
		public static explicit operator long(JsonObject value) {
			if (value == null)
				throw new ArgumentNullException("value");
			return Convert.ToInt64(((JsonPrimitive) value).Value);
		}

		/// <summary>Converts to short. Also see <see cref="JsonUtil"/>.</summary>
		public static explicit operator short(JsonObject value) {
			if (value == null)
				throw new ArgumentNullException("value");
			return Convert.ToInt16(((JsonPrimitive) value).Value);
		}

		/// <summary>Converts to string. Also see <see cref="JsonUtil"/>.</summary>
		public static explicit operator string(JsonObject value) {
			if (value == null)
				return null;
			return (string) ((JsonPrimitive) value).Value;
		}
	}
}