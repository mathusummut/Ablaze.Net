using System.Collections.Generic;
using System.Text;

namespace System.Graphics.Models.Parsers {
	/// <summary>
	/// Marks a class as a texture parser.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class TextureParserAttribute : Attribute {
		/// <summary>
		/// The type of TextureParserAttribute. 
		/// </summary>
		public static readonly Type Type = typeof(TextureParserAttribute);
		internal Dictionary<string, string> Extensions = new Dictionary<string, string>(StringComparer.Ordinal);

		/// <summary>
		/// Initializes the texture parser.
		/// </summary>
		/// <param name="parameters">A string representing the encoding file extension and the static method name that handles the parsing in the class.
		/// Examples: "obj, ParseObj", or "obj,ParseObj, mtl,ParseMtl"</param>
		public TextureParserAttribute(string parameters) {
			if (parameters == null)
				return;
			string[] strs = parameters.Split(',');
			if (parameters.Length != 0 && (strs.Length & 1) == 0) {
				string key, value;
				for (int i = 0; i < strs.Length; i += 2) {
					key = strs[i];
					value = strs[i + 1].Trim();
					if (!(key.Length == 0 || value.Length == 0)) {
						key = key.Trim().ToLower();
						if (key[0] == '.')
							key = key.Substring(1);
						Extensions.Add(key, value);
					}
				}
			} else
				throw new ArgumentException("The parameter string is not in the proper format.");
		}

		/// <summary>
		/// Gets whether the number of counted extensions is 0.
		/// </summary>
		public override bool IsDefaultAttribute() {
			return Extensions.Count == 0;
		}

		/// <summary>
		/// Gets a string representing the encoding methods for this attribute instance.
		/// </summary>
		public override string ToString() {
			StringBuilder builder = new StringBuilder("{");
			string[] array = new List<string>(Extensions.Keys).ToArray();
			for (int i = 0; i < array.Length; i++) {
				if (i == array.Length - 1)
					builder.Append(array[i]);
				else
					builder.Append(array[i] + ", ");
			}
			builder.Append("}");
			return builder.ToString();
		}
	}
}