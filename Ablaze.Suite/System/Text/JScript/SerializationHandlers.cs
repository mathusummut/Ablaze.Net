using System.Collections.Generic;

namespace System.Text.JScript {
	/// <summary>
	/// Provides the abstract base class for implementing a custom type resolver.
	/// </summary>
	public abstract class JavaScriptTypeResolver {
		/// <summary>
		/// Initializes a new instance of the JavaScriptTypeResolver class.
		/// </summary>
		public JavaScriptTypeResolver() {
		}

		/// <summary>When overridden in a derived class, returns the <see cref="T:System.Type" /> object that is associated with the specified type name.</summary>
		/// <returns>The <see cref="T:System.Type" /> object that is associated with the specified type name.</returns>
		/// <param name="id">The name of the managed type.</param>
		public abstract Type ResolveType(string id);

		/// <summary>When overridden in a derived class, returns the type name for the specified <see cref="T:System.Type" /> object.</summary>
		/// <returns>The name of the specified managed type.</returns>
		/// <param name="type">The managed type to be resolved.</param>
		public abstract string ResolveTypeId(Type type);
	}

	/// <summary>
	/// Provides an abstract base class for a custom type converter. Simply inheriting this class is enough for an instance of the converter
	/// to be registered for JSON serialization.
	/// </summary>
	public abstract class JavaScriptConverter {
		/// <summary>
		/// When overridden in a derived class, gets a collection of the supported types.
		/// </summary>
		public abstract IEnumerable<Type> SupportedTypes {
			get;
		}

		/// <summary>
		/// Initializes a new instance of the JavaScriptConverter class.
		/// </summary>
		public JavaScriptConverter() {
		}

		/// <summary>When overridden in a derived class, converts the provided dictionary into an object of the specified type.</summary>
		/// <returns>The deserialized object.</returns>
		/// <param name="dictionary">An IDictionary instance of property data stored as name/value pairs.</param>
		/// <param name="type">The type of the resulting object.</param>
		/// <param name="serializer">The JavaScriptSerializer instance.</param>
		public abstract object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer);

		/// <summary>When overridden in a derived class, builds a dictionary of name/value pairs.</summary>
		/// <returns>An object that contains key/value pairs that represent the object’s data.</returns>
		/// <param name="obj">The object to serialize.</param>
		/// <param name="serializer">The object that is responsible for the serialization.</param>
		public abstract IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer);
	}
}