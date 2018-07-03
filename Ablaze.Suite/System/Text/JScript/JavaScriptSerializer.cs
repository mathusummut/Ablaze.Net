//
// JavaScriptSerializer.cs
//
// Authors:
//   Konstantin Triger <kostat@mainsoft.com>
//   Marek Safar <marek.safar@gmail.com>
//
// (C) 2007 Mainsoft, Inc.  http://www.mainsoft.com
// Copyright 2012 Xamarin Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Reflection;
using System.ComponentModel;

namespace System.Text.JScript {
	/// <summary>
	/// Provides serialization and deserialization functionality for Javascript-enabled applications.
	/// </summary>
	public class JavaScriptSerializer {
		internal static readonly JavaScriptSerializer DefaultSerializer = new JavaScriptSerializer(null);
		internal const string SerializedTypeNameKey = "__type";
		List<IEnumerable<JavaScriptConverter>> _converterList;
		int _recursionLimit;
		JavaScriptTypeResolver _typeResolver;

		/// <summary>
		/// Initializes a new instance of the JavaScriptSerializer class that has no type resolver
		/// .</summary>
		public JavaScriptSerializer() : this(null) {
		}

		/// <summary>
		/// Initializes a new instance of the JavaScriptSerializer that has a custom type resolver.
		/// </summary>
		/// <param name="resolver">The custom type-resolver object.</param>
		public JavaScriptSerializer(JavaScriptTypeResolver resolver) {
			_typeResolver = resolver;
			_recursionLimit = 100;
		}

		/// <summary>
		/// Gets or sets the limit for constraining the number of object levels to process.
		/// </summary>
		/// <returns>The number of object levels. The default is 100.</returns>
		public int RecursionLimit {
			get {
				return _recursionLimit;
			}
			set {
				_recursionLimit = value;
			}
		}

		internal JavaScriptTypeResolver TypeResolver {
			get {
				return _typeResolver;
			}
		}

		/// <summary>Converts the given object to the specified type.</summary>
		/// <returns>The object that has been converted to the target type.</returns>
		/// <param name="obj">The object to convert.</param>
		/// <typeparam name="T">The type to which <paramref name="obj" /> will be converted.</typeparam>
		public T ConvertToType<T>(object obj) {
			if (obj == null)
				return default(T);

			return (T) ConvertToType(obj, typeof(T));
		}

		/// <summary>Converts the specified object to the specified type.</summary>
		/// <returns>The serialized JSON string.</returns>
		/// <param name="obj">The object to convert.</param>
		/// <param name="targetType">The type to convert the object to.</param>
		public object ConvertToType(object obj, Type targetType) {
			if (obj == null)
				return null;

			if (obj is IDictionary<string, object>) {
				if (targetType == null)
					obj = EvaluateDictionary((IDictionary<string, object>) obj);
				else {
					JavaScriptConverter converter = GetConverter(targetType);
					if (converter != null)
						return converter.Deserialize(
							EvaluateDictionary((IDictionary<string, object>) obj),
							targetType, this);
				}

				return ConvertToObject((IDictionary<string, object>) obj, targetType);
			}
			if (obj is ArrayList)
				return ConvertToList((ArrayList) obj, targetType);

			if (targetType == null)
				return obj;

			Type sourceType = obj.GetType();
			if (targetType.IsAssignableFrom(sourceType))
				return obj;

			if (targetType.IsEnum)
				if (obj is string)
					return Enum.Parse(targetType, (string) obj, true);
				else
					return Enum.ToObject(targetType, obj);

			TypeConverter c = TypeDescriptor.GetConverter(targetType);
			if (c.CanConvertFrom(sourceType)) {
				if (obj is string)
					return c.ConvertFromInvariantString((string) obj);

				return c.ConvertFrom(obj);
			}

			if ((targetType.IsGenericType) && (targetType.GetGenericTypeDefinition() == typeof(Nullable<>))) {
				if (obj is String) {
					/*
					 * Take care of the special case whereas in JSON an empty string ("") really means 
					 * an empty value 
					 * (see: https://bugzilla.novell.com/show_bug.cgi?id=328836)
					 */
					if (String.IsNullOrEmpty((String) obj))
						return null;
				} else if (c.CanConvertFrom(typeof(string))) {
					TypeConverter objConverter = TypeDescriptor.GetConverter(obj);
					string s = objConverter.ConvertToInvariantString(obj);
					return c.ConvertFromInvariantString(s);
				}
			}

			return Convert.ChangeType(obj, targetType);
		}

		/// <summary>Converts the specified JSON string to an object of type T.</summary>
		/// <returns>The deserialized object.</returns>
		/// <param name="input">The JSON string to be deserialized.</param>
		/// <typeparam name="T">The type of the resulting object.</typeparam>
		public T Deserialize<T>(string input) {
			return ConvertToType<T>(DeserializeObjectInternal(input));
		}

		/// <summary>Converts a JSON-formatted string to an object of the specified type.</summary>
		/// <returns>The deserialized object.</returns>
		/// <param name="input">The JSON string to deserialize.</param>
		/// <param name="targetType">The type of the resulting object.</param>
		public object Deserialize(string input, Type targetType) {
			object obj = DeserializeObjectInternal(input);

			if (obj == null)
				return null;

			return ConvertToType(obj, targetType);
		}

		static object Evaluate(object value) {
			return Evaluate(value, false);
		}

		static object Evaluate(object value, bool convertListToArray) {
			if (value is IDictionary<string, object>)
				value = EvaluateDictionary((IDictionary<string, object>) value, convertListToArray);
			else if (value is ArrayList)
				value = EvaluateList((ArrayList) value, convertListToArray);
			return value;
		}

		static object EvaluateList(ArrayList e) {
			return EvaluateList(e, false);
		}

		static object EvaluateList(ArrayList e, bool convertListToArray) {
			ArrayList list = new ArrayList();
			foreach (object value in e)
				list.Add(Evaluate(value, convertListToArray));

			return convertListToArray ? (object) list.ToArray() : list;
		}

		static IDictionary<string, object> EvaluateDictionary(IDictionary<string, object> dict) {
			return EvaluateDictionary(dict, false);
		}

		static IDictionary<string, object> EvaluateDictionary(IDictionary<string, object> dict, bool convertListToArray) {
			Dictionary<string, object> d = new Dictionary<string, object>(StringComparer.Ordinal);
			foreach (KeyValuePair<string, object> entry in dict) {
				d.Add(entry.Key, Evaluate(entry.Value, convertListToArray));
			}

			return d;
		}

		static readonly Type typeofObject = typeof(object);
		static readonly Type typeofGenList = typeof(List<>);

		object ConvertToList(ArrayList col, Type type) {
			Type elementType = null;
			if (type != null && type.HasElementType)
				elementType = type.GetElementType();

			IList list;
			if (type == null || type.IsArray || typeofObject == type || typeof(ArrayList).IsAssignableFrom(type))
				list = new ArrayList();
			else if (ReflectionUtils.IsInstantiatableType(type))
				// non-generic typed list
				list = (IList) Activator.CreateInstance(type, true);
			else if (ReflectionUtils.IsAssignable(type, typeofGenList)) {
				if (type.IsGenericType) {
					Type[] genArgs = type.GetGenericArguments();
					elementType = genArgs[0];
					// generic list
					list = (IList) Activator.CreateInstance(typeofGenList.MakeGenericType(genArgs));
				} else
					list = new ArrayList();
			} else
				throw new InvalidOperationException(String.Format("Deserializing list type '{0}' not supported.", type.GetType().Name));

			if (list.IsReadOnly) {
				EvaluateList(col);
				return list;
			}

			if (elementType == null)
				elementType = typeof(object);

			foreach (object value in col)
				list.Add(ConvertToType(value, elementType));

			if (type != null && type.IsArray)
				list = ((ArrayList) list).ToArray(elementType);

			return list;
		}

		object ConvertToObject(IDictionary<string, object> dict, Type type) {
			if (_typeResolver != null) {
				if (dict.Keys.Contains(SerializedTypeNameKey)) {
					// already Evaluated
					type = _typeResolver.ResolveType((string) dict[SerializedTypeNameKey]);
				}
			}

			if (type.IsGenericType) {
				if (type.GetGenericTypeDefinition().IsAssignableFrom(typeof(IDictionary<,>))) {
					Type[] arguments = type.GetGenericArguments();
					if (arguments == null || arguments.Length != 2 || (arguments[0] != typeof(object) && arguments[0] != typeof(string)))
						throw new InvalidOperationException(
							"Type '" + type + "' is not not supported for serialization/deserialization of a dictionary, keys must be strings or objects.");
					if (type.IsAbstract) {
						Type dictType = typeof(Dictionary<,>);
						type = dictType.MakeGenericType(arguments[0], arguments[1]);
					}
				}
			} else if (type.IsAssignableFrom(typeof(IDictionary)))
				type = typeof(Dictionary<string, object>);

			object target = Activator.CreateInstance(type, true);

			foreach (KeyValuePair<string, object> entry in dict) {
				object value = entry.Value;
				if (target is IDictionary) {
					Type valueType = ReflectionUtils.GetTypedDictionaryValueType(type);
					if (value != null && valueType == typeof(System.Object))
						valueType = value.GetType();

					((IDictionary) target).Add(entry.Key, ConvertToType(value, valueType));
					continue;
				}
				MemberInfo[] memberCollection = type.GetMember(entry.Key, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
				if (memberCollection == null || memberCollection.Length == 0) {
					//must evaluate value
					Evaluate(value);
					continue;
				}

				MemberInfo member = memberCollection[0];

				if (!ReflectionUtils.CanSetMemberValue(member)) {
					//must evaluate value
					Evaluate(value);
					continue;
				}

				Type memberType = ReflectionUtils.GetMemberUnderlyingType(member);

				if (memberType.IsInterface) {
					if (memberType.IsGenericType)
						memberType = ResolveGenericInterfaceToType(memberType);
					else
						memberType = ResolveInterfaceToType(memberType);

					if (memberType == null)
						throw new InvalidOperationException("Unable to deserialize a member, as its type is an unknown interface.");
				}

				ReflectionUtils.SetMemberValue(member, target, ConvertToType(value, memberType));
			}

			return target;
		}

		static Type ResolveGenericInterfaceToType(Type type) {
			Type[] genericArgs = type.GetGenericArguments();

			if (ReflectionUtils.IsSubClass(type, typeof(IDictionary<,>)))
				return typeof(Dictionary<,>).MakeGenericType(genericArgs);

			if (ReflectionUtils.IsSubClass(type, typeof(IList<>)) ||
				ReflectionUtils.IsSubClass(type, typeof(ICollection<>)) ||
				ReflectionUtils.IsSubClass(type, typeof(IEnumerable<>))
			)
				return typeof(List<>).MakeGenericType(genericArgs);

			if (ReflectionUtils.IsSubClass(type, typeof(IComparer<>)))
				return typeof(Comparer<>).MakeGenericType(genericArgs);

			if (ReflectionUtils.IsSubClass(type, typeof(IEqualityComparer<>)))
				return typeof(EqualityComparer<>).MakeGenericType(genericArgs);

			return null;
		}

		static Type ResolveInterfaceToType(Type type) {
			if (typeof(IDictionary).IsAssignableFrom(type))
				return typeof(Hashtable);

			if (typeof(IList).IsAssignableFrom(type) ||
				typeof(ICollection).IsAssignableFrom(type) ||
				typeof(IEnumerable).IsAssignableFrom(type))
				return typeof(ArrayList);

			if (typeof(IComparer).IsAssignableFrom(type))
				return typeof(Comparer);

			return null;
		}

		/// <summary>Converts the specified JSON string to an object graph.</summary>
		/// <returns>The deserialized object.</returns>
		/// <param name="input">The JSON string to be deserialized.</param>
		public object DeserializeObject(string input) {
			object obj = Evaluate(DeserializeObjectInternal(input), true);
			IDictionary dictObj = obj as IDictionary;
			if (dictObj != null && dictObj.Contains(SerializedTypeNameKey)) {
				if (_typeResolver == null) {
					throw new ArgumentNullException("resolver", "Must have a type resolver to deserialize an object that has an '__type' member");
				}

				obj = ConvertToType(obj, null);
			}
			return obj;
		}

		internal object DeserializeObjectInternal(string input) {
			return Json.Deserialize(input, this);
		}

		internal object DeserializeObjectInternal(TextReader input) {
			return Json.Deserialize(input, this);
		}

		/// <summary>Registers a custom converter with the JavaScriptSerializer instance. </summary>
		/// <param name="converters">An array that contains the custom converters to be registered.</param>
		internal void RegisterConverters(IEnumerable<JavaScriptConverter> converters) {
			if (converters == null)
				return;
			if (_converterList == null)
				_converterList = new List<IEnumerable<JavaScriptConverter>>();
			_converterList.Add(converters);
		}

		internal JavaScriptConverter GetConverter(Type type) {
			if (_converterList != null)
				for (int i = 0; i < _converterList.Count; i++) {
					foreach (JavaScriptConverter converter in _converterList[i])
						foreach (Type supportedType in converter.SupportedTypes)
							if (supportedType.IsAssignableFrom(type))
								return converter;
				}

			return null;
		}

		/// <summary>Converts an object to a JSON string.</summary>
		/// <returns>The serialized JSON string.</returns>
		/// <param name="obj">The object to serialize.</param>
		public string Serialize(object obj) {
			StringBuilder b = new StringBuilder();
			Serialize(obj, b);
			return b.ToString();
		}

		/// <summary>Serializes an object and writes the resulting JSON string to the specified StringBuilder object.</summary>
		/// <param name="obj">The object to serialize.</param>
		/// <param name="output">The StringBuilder object that is used to write the JSON string.</param>
		public void Serialize(object obj, StringBuilder output) {
			Json.Serialize(obj, this, output);
		}

		internal void Serialize(object obj, TextWriter output) {
			Json.Serialize(obj, this, output);
		}
	}
}