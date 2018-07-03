using System.Runtime.CompilerServices;

namespace System.Collections.Generic {
	/// <summary>
	/// A list that automatically expands when an index that is out of range is accessed.
	/// </summary>
	/// <typeparam name="T">The type of object contained as items in the collection.</typeparam>
	[Serializable]
	public sealed class LazyList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IList, ICollection
#if NET45
		, IReadOnlyList<T>, IReadOnlyCollection<T>
#endif
	{
		/// <summary>
		/// The list of elements contained in the collection.
		/// </summary>
		public readonly List<T> Items;

		/// <summary>
		/// Gets the current number of elements contained in the collection.
		/// </summary>
		public int Count {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return Items.Count;
			}
		}

		/// <summary>
		/// Gets an element from the collection with a specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the element to be retrieved from the collection.</param>
		public T this[int index] {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				EnsureCapacity(index + 1);
				return Items[index];
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				EnsureCapacity(index + 1);
				Items[index] = value;
			}
		}

		bool ICollection<T>.IsReadOnly {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return false;
			}
		}

		/// <summary>
		/// Gets a value (false) that indicates whether the collection is thread safe.
		/// </summary>
		bool ICollection.IsSynchronized {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return false;
			}
		}

		/// <summary>
		/// Gets the object used to synchronize access to the collection.
		/// </summary>
		object ICollection.SyncRoot {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return this;
			}
		}

		/// <summary>
		/// Gets a value (false) that indicates whether the collection is fixed in size.
		/// </summary>
		bool IList.IsFixedSize {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return false;
			}
		}

		/// <summary>
		/// Gets a value (false) that indicates whether the collection is read only.
		/// </summary>
		bool IList.IsReadOnly {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return false;
			}
		}

		/// <summary>
		/// Gets or sets the item at a specified zero-based index.
		/// </summary>
		/// <param name="index">The zero-based index of the element to be retrieved from the collection.</param>
		object IList.this[int index] {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return this[index];
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				this[index] = (T) value;
			}
		}

		/// <summary>
		/// Initializes a new list.
		/// </summary>
		public LazyList() {
			Items = new List<T>();
		}

		/// <summary>
		/// Initializes a new list.
		/// </summary>
		/// <param name="initialCapacity">The initial capacity of the list.</param>
		public LazyList(int initialCapacity) {
			Items = new List<T>(initialCapacity < 0 ? 6 : initialCapacity);
		}

		/// <summary>
		/// Initializes a LazyList wrapper from the specified list.
		/// </summary>
		/// <param name="list">The list of elements to wrap.</param>
		public LazyList(List<T> list) {
			Items = list;
		}

		/// <summary>
		/// Initializes a new list from a specified enumerable list of elements .
		/// </summary>
		/// <param name="list">The <see cref="T:System.Collections.Generic.IEnumerable`1" /> collection of elements used to initialize the collection.</param>
		public LazyList(IEnumerable<T> list) {
			Items = new List<T>(list);
		}

		/// <summary>
		/// Initializes a new list from a specified array of elements.
		/// </summary>
		/// <param name="list">The <see cref="T:System.Array"/> of type T elements used to initialize the collection.</param>
		public LazyList(params T[] list) : this((IEnumerable<T>) list) {
		}

		private void EnsureCapacity(int targetCount) {
			for (int i = Count; i < targetCount; i++)
				Add(default(T));
		}

		/// <summary>
		/// Adds an item to the collection.
		/// </summary>
		/// <param name="item">The element to be added to the collection.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Add(T item) {
			Items.Add(item);
		}

		/// <summary>
		/// Adds a range of items to the collection.
		/// </summary>
		/// <param name="items">The elements to be added to the collection.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void AddRange(IEnumerable<T> items) {
			Items.AddRange(items);
		}

		/// <summary>
		/// Adds a range of items to the collection.
		/// </summary>
		/// <param name="items">The elements to be added to the collection.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void AddRange(T[] items) {
			Items.AddRange(items);
		}

		/// <summary>
		/// Removes all items from the collection.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Clear() {
			Items.Clear();
		}

		/// <summary>
		/// Determines whether the collection contains an element with a specific value.
		/// </summary>
		/// <param name="item">The object to locate in the collection.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool Contains(T item) {
			return Items.Contains(item);
		}

		/// <summary>
		/// Copies the elements of the collection to a specified array, starting at index 0.
		/// </summary>
		/// <param name="array">The destination <see cref="T:System.Array"/> for the elements of type T copied from the collection.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void CopyTo(T[] array) {
			Items.CopyTo(array);
		}

		/// <summary>
		/// Copies the elements of the collection to a specified array, starting at a particular index.
		/// </summary>
		/// <param name="array">The destination <see cref="T:System.Array"/> for the elements of type T copied from the collection.</param>
		/// <param name="index">The zero-based index in the array at which copying begins.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void CopyTo(T[] array, int index) {
			Items.CopyTo(array, index);
		}

		/// <summary>
		/// Copies the elements of the collection to a specified array using the specified parameters.
		/// </summary>
		/// <param name="index">The zero-based index in the source list at which copying begins.</param>
		/// <param name="array">The one-dimensional array that is the destination of the elements copied from the list. The array must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in the array at which copying begins.</param>
		/// <param name="count">The number of elements to copy.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void CopyTo(int index, T[] array, int arrayIndex, int count) {
			Items.CopyTo(index, array, arrayIndex, count);
		}

		/// <summary>
		/// Returns the index of the first occurrence of a value in the collection. Returns -1 if the item is not found.
		/// </summary>
		/// <param name="item">The item to locate.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public int IndexOf(T item) {
			return Items.IndexOf(item);
		}

		/// <summary>
		/// Inserts an item into the collection at a specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the element to be retrieved from the collection.</param>
		/// <param name="item">The object to be inserted into the collection as an element.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Insert(int index, T item) {
			EnsureCapacity(index);
			Items.Insert(index, item);
		}

		/// <summary>
		/// Inserts items into the collection at a specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the element to be retrieved from the collection.</param>
		/// <param name="items">The objects to be inserted into the collection as an element.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void InsertRange(int index, IEnumerable<T> items) {
			EnsureCapacity(index);
			Items.InsertRange(index, items);
		}

		/// <summary>
		/// Removes the first occurrence of a specified item from the collection. Returns true if removal was successful, else false.
		/// </summary>
		/// <param name="item">The object to remove from the collection.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool Remove(T item) {
			return Items.Remove(item);
		}

		/// <summary>
		/// Removes an item at a specified index from the collection.
		/// </summary>
		/// <param name="index">The zero-based index of the element to be retrieved from the collection.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void RemoveAt(int index) {
			if (index < Items.Count)
				Items.RemoveAt(index);
		}

		/// <summary>
		/// Removes a range of items at a specified index from the collection.
		/// </summary>
		/// <param name="index">The zero-based index of the element to be retrieved from the collection.</param>
		/// <param name="count">The number of items to remove.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void RemoveRange(int index, int count) {
			int currentCount = Count;
			if (index + count > currentCount)
				count = currentCount - index;
			if (count > 0)
				Items.RemoveRange(index, count);
		}

		/// <summary>
		/// Reverses the list.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Reverse() {
			Items.Reverse();
		}

		/// <summary>
		/// Reverses the specified elements in the list.
		/// </summary>
		/// <param name="index">The starting index.</param>
		/// <param name="count">The number of indices to reverse.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Reverse(int index, int count) {
			Items.Reverse(index, count);
		}

		/// <summary>
		/// Copies the elements of the collection to a specified array, starting at a particular index.
		/// </summary>
		/// <param name="array">The destination array for the elements of type T copied from the collection.</param>
		/// <param name="index">The zero-based index in the array at which copying begins.</param>
		void ICollection.CopyTo(Array array, int index) {
			((ICollection) Items).CopyTo(array, index);
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public IEnumerator<T> GetEnumerator() {
			return new FastEnumerator<T>(Items);
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator() {
			return new FastEnumerator<T>(Items);
		}

		/// <summary>
		/// Adds an element to the collection and return the position into which the new element was inserted.
		/// </summary>
		/// <param name="value">The object to add to the collection.</param>
		int IList.Add(object value) {
			int count = Items.Count;
			Add((T) value);
			return count;
		}

		/// <summary>
		/// Determines whether the collection contains an element with a specific value.
		/// </summary>
		/// <param name="value">The object to locate in the collection.</param>
		bool IList.Contains(object value) {
			return value is T ? Contains((T) value) : false;
		}

		/// <summary>
		/// Determines the zero-based index of an element in the collection, or -1 if not found.
		/// </summary>
		/// <param name="value">The element in the collection whose index is being determined.</param>
		int IList.IndexOf(object value) {
			return value is T ? IndexOf((T) value) : -1;
		}

		/// <summary>
		/// Inserts an object into the collection at a specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which <paramref name="value"/> is to be inserted.</param>
		/// <param name="value">The object to insert into the collection.</param>
		void IList.Insert(int index, object value) {
			Insert(index, (T) value);
		}

		/// <summary>
		/// Removes the first occurrence of a specified object as an element from the collection.
		/// </summary>
		/// <param name="value">The object to be removed from the collection.</param>
		void IList.Remove(object value) {
			Remove((T) value);
		}

		/// <summary>
		/// Gets the items from a LazyList.
		/// </summary>
		/// <param name="list">The list whose items to get.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static explicit operator List<T>(LazyList<T> list) {
			return list.Items;
		}

		/// <summary>
		/// Initializes a LazyList wrapper from a list.
		/// </summary>
		/// <param name="list">The list to wrap.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static explicit operator LazyList<T>(List<T> list) {
			return new LazyList<T>(list);
		}
	}
}