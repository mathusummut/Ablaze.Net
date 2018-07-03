using System.Collections.Generic;
using System.Text;

namespace System.Graphics.Models.Parsers {
	/// <summary>
	/// Marks a class as a model parser.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class ModelParserAttribute : Attribute {
		/// <summary>
		/// The type of ModelParserAttribute. 
		/// </summary>
		public static readonly Type Type = typeof(ModelParserAttribute);
		internal Dictionary<string, ValueTuple<string, string>> Extensions = new Dictionary<string, ValueTuple<string, string>>(StringComparer.Ordinal);

		/// <summary>
		/// Initializes the model parser.
		/// </summary>
		/// <param name="parameters">A string representing the encoding file extension and the static method name that handles the parsing in the class.
		/// The save method can be empty. Examples: "obj, ParseObj, SaveObj", or "obj,ParseObj, , mtl,ParseMtl,SaveMtl"</param>
		public ModelParserAttribute(string parameters) {
			if (parameters == null)
				return;
			string[] strs = parameters.Split(',');
			if (parameters.Length != 0 && strs.Length % 3 == 0) {
				string key;
				ValueTuple<string, string> value;
				for (int i = 0; i < strs.Length; i += 3) {
					key = strs[i];
					value = new ValueTuple<string, string>(strs[i + 1].Trim(), strs[i + 2].Trim());
					if (!(key.Length == 0 || (value.Item1.Length == 0 && value.Item2.Length == 0))) {
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
			builder.Append('}');
			return builder.ToString();
		}
	}
}