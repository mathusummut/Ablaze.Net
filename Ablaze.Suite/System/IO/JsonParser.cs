using System.Collections.Generic;
#if !NET35
using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
#endif
using System.Reflection;
using System.Text.JScript;

namespace System.IO {
	/// <summary>
	/// Provides methods for working with data in JavaScript Object Notation (JSON) format.
	/// </summary>
	public static class Json {
		private static JavaScriptSerializer serializer = new JavaScriptSerializer();
#if NET35
		private static MethodInfo deserialize;
#endif

		static Json() {
			Assembly[] assemblies;
			try {
				assemblies = AppDomain.CurrentDomain.GetAssemblies();
			} catch {
				return;
			}
#if NET35
			MethodInfo[] methods = typeof(JavaScriptSerializer).GetMethods(BindingFlags.Public);
			foreach (MethodInfo method in methods) {
				if (method.IsGenericMethod && method.Name == "Deserialize") {
					deserialize = method;
					break;
				}
			}
			List<JavaScriptConverter> converters = new List<JavaScriptConverter>();
			foreach (Assembly assembly in assemblies) {
#else
			SyncedList<JavaScriptConverter> converters = new SyncedList<JavaScriptConverter>();
			Parallel.ForEach(assemblies, assembly => {
#endif
				try {
#if NET35
					foreach (Type type in assembly.GetTypes()) {
#else
					Parallel.ForEach(assembly.GetTypes(), type => {
#endif
						try {
							if (type.IsSubclassOf(typeof(JavaScriptConverter))) {
								ConstructorInfo info = type.GetConstructor(Type.EmptyTypes);
								if (info != null) {
									JavaScriptConverter instance = info.Invoke(null) as JavaScriptConverter;
									if (instance != null)
										converters.Add(instance);
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
#if !NET35
			);
#endif
			serializer.RegisterConverters(converters);
		}

		/// <summary>
		/// Converts data in JavaScript Object Notation (JSON) format into a data object.
		/// </summary>
		/// <param name="value">The JSON object to deserialize.</param>
		/// <param name="caseInsensitiveDictonary">Whether the returned dictionary is case insensitive.
		/// Note that sub-dictionaries would still be case-sensitive.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Dictionary<string, object> Decode(string value, bool caseInsensitiveDictonary = true) {
			Dictionary<string, object> returnValues = serializer.DeserializeObject(value) as Dictionary<string, object>;
			if (caseInsensitiveDictonary)
				returnValues = new Dictionary<string, object>(returnValues, StringComparer.OrdinalIgnoreCase);
			return returnValues;
		}

/*#if NET35
		/// <summary>
		/// Converts data in JavaScript Object Notation (JSON) format into a data object.
		/// </summary>
		/// <param name="value">The JSON object to deserialize.</param>
		/// <param name="targetType">The type to deserialize to.</param>
		public static object Decode(string value, Type targetType) {
			MethodInfo generic = deserialize.MakeGenericMethod(targetType);
			return generic.Invoke(serializer, new object[] { value });
		}
#else*/
		/// <summary>
		/// Converts data in JavaScript Object Notation (JSON) format into a data object.
		/// </summary>
		/// <param name="value">The JSON object to deserialize.</param>
		/// <param name="targetType">The type to deserialize to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static object Decode(string value, Type targetType) {
			return serializer.Deserialize(value, targetType);
		}
//#endif

		/// <summary>
		/// Converts data in JavaScript Object Notation (JSON) format into a data object.
		/// </summary>
		/// <param name="value">The JSON object to deserialize.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static T Decode<T>(string value) {
			return serializer.Deserialize<T>(value);
		}

		/// <summary>
		/// Converts a data object to a string that is in the JavaScript Object Notation (JSON) format.
		/// </summary>
		/// <param name="value">The JSON object to serialize.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static string Encode(object value) {
			return serializer.Serialize(value);
		}

		/// <summary>
		/// Converts a data object to a string that is in the JavaScript Object Notation (JSON) format.
		/// </summary>
		/// <param name="pair">The JSON object to serialize.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static string Encode<TKey, TValue>(KeyValuePair<TKey, TValue> pair) {
			return serializer.Serialize(new Dictionary<TKey, TValue> { { pair.Key, pair.Value } });
		}

		/*/// <summary>
		/// Converts a Dictionary to a string that is in the JavaScript Object Notation (JSON) format.
		/// </summary>
		/// <param name="dict">The dictionary to serialize.</param>
		public static string Encode<T>(Dictionary<string, T> dict) {
			ExpandoObject eo = new ExpandoObject();
			ICollection<KeyValuePair<string, object>> eoColl = eo;
			foreach (KeyValuePair<string, T> kvp in dict)
				eoColl.Add(new KeyValuePair<string, object>(kvp.Key, kvp.Value));
			dynamic e = eo;
			return serializer.Serialize(e);
		}*/

#if !NET35
		private sealed class ExpandoConverter : JavaScriptConverter {
			private static Type Expando = typeof(ExpandoObject);

			public override IEnumerable<Type> SupportedTypes {
				get {
					return new Type[] { Expando };
				}
			}

			public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer) {
				return null;
			}

			public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer) {
				var result = new Dictionary<string, object>();
				var dictionary = obj as IDictionary<string, object>;
				foreach (var item in dictionary)
					result.Add(item.Key, item.Value);
				return result;
			}
		}
#endif
	}
}