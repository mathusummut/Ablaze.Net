using System.Runtime.CompilerServices;

namespace System.Reflection {
	/// <summary>
	/// Delegates to a field or property.
	/// </summary>
	public struct FieldOrProperty : IEquatable<FieldOrProperty> {
		/// <summary>
		/// An empty field or property.
		/// </summary>
		public static readonly FieldOrProperty Empty = new FieldOrProperty();
		/// <summary>
		/// The field represented by this instance. If null, then this instance represents a property.
		/// </summary>
		public readonly FieldInfo Field;
		/// <summary>
		/// The property represented by this instance. If null then this instance represents a field.
		/// </summary>
		public readonly PropertyInfo Property;
		/// <summary>
		/// The class that contains the property or field.
		/// </summary>
		public readonly Type ContaningType;
		/// <summary>
		/// The type signature of the property or field.
		/// </summary>
		public readonly Type TargetType;
		/// <summary>
		/// The instance on which the field or property resides. If null, then the field/property is static.
		/// </summary>
		public readonly object Instance;
		/// <summary>
		/// The name of the field or property.
		/// </summary>
		public readonly string Name;

		/// <summary>
		/// Gets the value of the field/property. To set value, use SetValue().
		/// </summary>
		public object Value {
			get {
				if (Property == null) {
					if (Field == null)
						return null;
					return Field.GetValue(Instance);
				} else if (Property.CanWrite)
					return Property.GetValue(Instance, null);
				else
					return null;
			}
		}

		/// <summary>
		/// Initializes a new FieldOrProperty instance.
		/// </summary>
		/// <param name="field">The field to represent.</param>
		/// <param name="instance">The instance on which the specified field resides (null if static).
		/// For equality comparison, == is used.</param>
		public FieldOrProperty(FieldInfo field, object instance) {
			Field = field;
			Instance = instance;
			Property = null;
			if (field == null) {
				Name = null;
				ContaningType = null;
				TargetType = null;
			} else {
				Name = field.Name;
				TargetType = field.FieldType;
				ContaningType = field.DeclaringType;
			}
		}

		/// <summary>
		/// Initializes a new FieldOrProperty instance.
		/// </summary>
		/// <param name="property">The property to represent.</param>
		/// <param name="instance">The instance on which the specified property resides (null if static).
		/// For equality comparison, == is used.</param>
		public FieldOrProperty(PropertyInfo property, object instance) {
			Field = null;
			Property = property;
			Instance = instance;
			if (property == null) {
				Name = null;
				ContaningType = null;
				TargetType = null;
			} else {
				Name = property.Name;
				TargetType = property.PropertyType;
				ContaningType = property.DeclaringType;
			}
		}

		/// <summary>
		/// Gets the corresponding field/property instance for the class member query.
		/// </summary>
		/// <param name="instance">The instance to get the field of (or type of class if static).</param>
		/// <param name="name">The name of the field/property to obtain.</param>
		/// <param name="throwIfNotFound">If true, an error will be thrown if the specified property/field is not found.</param>
		public FieldOrProperty(string name, object instance, bool throwIfNotFound = true) : this() {
			if (instance != null) {
				ContaningType = instance as Type;
				bool isInstance = ContaningType == null;
				if (isInstance) {
					Instance = instance;
					ContaningType = instance.GetType();
				}
			}
			if (name != null) {
				name = name.Trim();
				Name = name;
			}
			if (string.IsNullOrEmpty(name)) {
				if (throwIfNotFound)
					throw new FieldAccessException("Empty field name.");
				return;
			}
			do {
				Property = ContaningType.GetProperty(name, BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
				if (Property == null) {
					Field = ContaningType.GetField(name, BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
					if (Field != null) {
						TargetType = Field.FieldType;
						break;
					}
				} else {
					TargetType = Property.PropertyType;
					break;
				}
				ContaningType = ContaningType.BaseType;
			} while (Property == null && Field == null && ContaningType != null);
			if (throwIfNotFound && Property == null && Field == null)
				throw new FieldAccessException("The " + name + " field or property was not found.");
		}

		/// <summary>
		/// Sets the field/property to the specified value.
		/// </summary>
		/// <param name="newValue">The new value to assign to the field/property.</param>
		public void SetValue(object newValue) {
			if (Property == null) {
				if (Field != null)
					Field.SetValue(Instance, newValue);
			} else
				Property.SetValue(Instance, newValue, null);
		}

		/// <summary>
		/// Compares the FieldOrProperty for equality.
		/// </summary>
		/// <param name="left">The left-hand side of the comparison.</param>
		/// <param name="right">The right-hand side of the comparison.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator ==(FieldOrProperty left, FieldOrProperty right) {
			return left.Equals(right);
		}

		/// <summary>
		/// Compares the FieldOrProperty for inequality.
		/// </summary>
		/// <param name="left">The left-hand side of the comparison.</param>
		/// <param name="right">The right-hand side of the comparison.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator !=(FieldOrProperty left, FieldOrProperty right) {
			return !left.Equals(right);
		}

		/// <summary>
		/// Calculates the hash code for this FieldOrProperty.
		/// </summary>
		/// <returns>A System.Int32 containing the hashcode of this FieldOrProperty.</returns>
		public override int GetHashCode() {
			return unchecked((Property == null ? (Field == null ? 0 : Field.MetadataToken) : Property.MetadataToken) << 16 ^ (Instance == null ? 0 : Instance.GetHashCode()));
		}

		/// <summary>
		/// Creates a System.String that describes this FieldOrProperty.
		/// </summary>
		/// <returns>A System.String that describes this FieldOrProperty.</returns>
		public override string ToString() {
			return ContaningType == null ? "null" : (ContaningType + "." + Name);
		}

		/// <summary>
		/// Compares whether this FieldOrProperty is equal to the specified object.
		/// </summary>
		/// <param name="obj">An object to compare to.</param>
		public override bool Equals(object obj) {
			if (obj is FieldOrProperty)
				return Equals((FieldOrProperty) obj);
			else
				return false;
		}

		/// <summary>
		/// Compares whether this FieldOrProperty is equal to the specified object.
		/// </summary>
		/// <param name="other">The object to compare to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool Equals(FieldOrProperty other) {
			if (ContaningType == null)
				return other.ContaningType == null;
			else if (ContaningType.Equals(other.ContaningType) && Instance == other.Instance) {
				if (Field == null) {
					if (Property == null)
						return other.Field == null && other.Property == null;
					else if (other.Property == null)
						return false;
					else
						return Property.MetadataToken == other.Property.MetadataToken && Property.Module.Equals(other.Property.Module);
				} else {
					if (other.Field == null)
						return false;
					else
						return Field.MetadataToken == other.Field.MetadataToken && Field.Module.Equals(other.Field.Module);
				}
			} else
				return false;
		}
	}
}