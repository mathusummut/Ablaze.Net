using System.Collections;
using System.Collections.Generic;

namespace System.Text {
	/// <summary>Implements an array value.</summary>
	public class JsonArray : JsonObject, IList<JsonObject> {
		List<JsonObject> list;

		/// <summary>Initializes a new instance of this class.</summary>
		public JsonArray(params JsonObject[] items) {
			list = new List<JsonObject>();
			AddRange(items);
		}

		/// <summary>Initializes a new instance of this class.</summary>
		public JsonArray(IEnumerable<JsonObject> items) {
			if (items == null)
				throw new ArgumentNullException(nameof(items));
			list = new List<JsonObject>(items);
		}

		/// <summary>Gets the count of the contained items.</summary>
		public override int Count {
			get {
				return list.Count;
			}
		}

		bool ICollection<JsonObject>.IsReadOnly {
			get {
				return false;
			}
		}

		/// <summary>Gets or sets the value for the specified index.</summary>
		public override sealed JsonObject this[int index] {
			get {
				return list[index];
			}
			set {
				list[index] = value;
			}
		}

		/// <summary>The type of this value.</summary>
		public override JsonType JsonType {
			get {
				return JsonType.Array;
			}
		}

		/// <summary>Adds a new item.</summary>
		public void Add(JsonObject item) {
			list.Add(item);
		}

		/// <summary>Adds a range of items.</summary>
		public void AddRange(IEnumerable<JsonObject> items) {
			if (items == null)
				throw new ArgumentNullException(nameof(items));
			list.AddRange(items);
		}

		/// <summary>Clears the array.</summary>
		public override void Clear() {
			list.Clear();
		}

		/// <summary>Determines whether the array contains a specific value.</summary>
		public bool Contains(JsonObject item) {
			return list.Contains(item);
		}

		/// <summary>Copies the elements to an System.Array, starting at a particular System.Array index.</summary>
		public void CopyTo(JsonObject[] array, int arrayIndex) {
			list.CopyTo(array, arrayIndex);
		}

		/// <summary>Determines the index of a specific item.</summary>
		public int IndexOf(JsonObject item) {
			return list.IndexOf(item);
		}

		/// <summary>Inserts an item.</summary>
		public void Insert(int index, JsonObject item) {
			list.Insert(index, item);
		}

		/// <summary>Removes the specified item.</summary>
		public bool Remove(JsonObject item) {
			return list.Remove(item);
		}

		/// <summary>Removes the item with the specified index.</summary>
		public void RemoveAt(int index) {
			list.RemoveAt(index);
		}

		IEnumerator<JsonObject> IEnumerable<JsonObject>.GetEnumerator() {
			return list.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return list.GetEnumerator();
		}
	}
}