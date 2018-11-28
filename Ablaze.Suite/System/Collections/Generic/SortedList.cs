using System.Runtime.CompilerServices;

namespace System.Collections.Generic {
	/// <summary>
	/// A list that automatically keeps items sorted. Internally, it only uses a regular list, unlike SortedList&lt;Key, Value&gt;.
	/// It is only suitable for a small number of elements.
	/// </summary>
	/// <typeparam name="T">The type of object contained as items in the collection.</typeparam>
	[Serializable]
	public sealed class SortedList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IList, ICollection
#if NET45
		, IReadOnlyList<T>, IReadOnlyCollection<T>
#endif
	{
		/// <summary>
		/// The list of elements contained in the collection. DO NOT modify the content, or you may unbalance the sorting.
		/// </summary>
		public readonly List<T> Items;
		/// <summary>
		/// The comparer to use.
		/// </summary>
		private IComparer<T> comparer;

		/// <summary>
		/// Gets the comparer that is used by this instance.
		/// </summary>
		public IComparer<T> Comparer {
			get {
				return comparer;
			}
		}

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
		/// Gets an element from the collection with a specified index. When an element is changed, the new value may not be inserted at the specified index.
		/// The new value is insterted in a sorted position.
		/// </summary>
		/// <param name="index">The zero-based index of the element to be retrieved from the collection.</param>
		public T this[int index] {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return Items[index];
			}
			set {
				if (object.Equals(value, Items[index]))
					return;
				int newIndex = FindTargetIndex(value);
				if (newIndex == index)
					return;
				Items.RemoveAt(index);
				if (newIndex > index)
					newIndex--;
				Items.Insert(newIndex, value);
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
		/// Initializes a new list using the default comparer.
		/// </summary>
		public SortedList() {
			Items = new List<T>();
			comparer = Comparer<T>.Default;
		}

		/// <summary>
		/// Copies the specified sorted list.
		/// </summary>
		/// <param name="list">The list whose elemenst and comparer to copy.</param>
		public SortedList(SortedList<T> list) {
			Items = new List<T>(list.Items);
			comparer = list.comparer;
		}

		/// <summary>
		/// Initializes a new list using the specified comparer.
		/// </summary>
		/// <param name="comparer">The comparer to use.</param>
		public SortedList(Comparison<T> comparer) : this(Extensions.Create<T>(comparer)) {
		}

		/// <summary>
		/// Initializes a new list using the specified comparer.
		/// </summary>
		/// <param name="comparer">The comparer to use.</param>
		public SortedList(IComparer<T> comparer) {
			Items = new List<T>();
			this.comparer = comparer;
		}

		/// <summary>
		/// Initializes a new list from items from the specified list, but sorts the elements using the default comparer before doing so.
		/// </summary>
		/// <param name="list">The list of elements to sort.</param>
		public SortedList(List<T> list) : this(list, Comparer<T>.Default) {
		}

		/// <summary>
		/// Initializes a new list from items from the specified list, but sorts the elements using the default comparer before doing so.
		/// </summary>
		/// <param name="items">The list of elements to sort.</param>
		public SortedList(IEnumerable<T> items) : this(items, Comparer<T>.Default) {
		}

		/// <summary>
		/// Initializes a new list from items from the specified array of elements, but sorts the elements using the default comparer before doing so.
		/// </summary>
		/// <param name="items">The array of elements to sort.</param>
		public SortedList(params T[] items) : this(items, Comparer<T>.Default) {
		}

		/// <summary>
		/// Initializes a new list from items from the specified list, but sorts the elements using the specified comparer before doing so.
		/// </summary>
		/// <param name="list">The list of elements to sort.</param>
		/// <param name="comparer">The comparer to use for sorting.</param>
		public SortedList(List<T> list, Comparison<T> comparer) : this(list, Extensions.Create<T>(comparer)) {
		}

		/// <summary>
		/// Initializes a new list from items from the specified list, but sorts the elements using the specified comparer before doing so.
		/// </summary>
		/// <param name="items">The list of elements to sort.</param>
		/// <param name="comparer">The comparer to use for sorting.</param>
		public SortedList(IEnumerable<T> items, Comparison<T> comparer) : this(items, Extensions.Create<T>(comparer)) {
		}

		/// <summary>
		/// Initializes a new list from items from the specified array of elements, but sorts the elements using the specified comparer before doing so.
		/// </summary>
		/// <param name="items">The array of elements to sort.</param>
		/// <param name="comparer">The comparer to use for sorting.</param>
		public SortedList(Comparison<T> comparer, params T[] items) : this(items, Extensions.Create<T>(comparer)) {
		}

		/// <summary>
		/// Initializes a new list from items from the specified list, but sorts the elements using the specified comparer before doing so.
		/// </summary>
		/// <param name="list">The list of elements to sort.</param>
		/// <param name="comparer">The comparer to use for sorting.</param>
		public SortedList(List<T> list, IComparer<T> comparer) {
			Items = new List<T>(list);
			this.comparer = comparer;
			Items.Sort(comparer);
		}

		/// <summary>
		/// Initializes a new list from items from the specified list, but sorts the elements using the specified comparer before doing so.
		/// </summary>
		/// <param name="items">The list of elements to sort.</param>
		/// <param name="comparer">The comparer to use for sorting.</param>
		public SortedList(IEnumerable<T> items, IComparer<T> comparer) {
			Items = new List<T>(items);
			this.comparer = comparer;
			Items.Sort(comparer);
		}

		/// <summary>
		/// Initializes a new list from items from the specified array of elements, but sorts the elements using the specified comparer before doing so.
		/// </summary>
		/// <param name="items">The array of elements to sort.</param>
		/// <param name="comparer">The comparer to use for sorting.</param>
		public SortedList(IComparer<T> comparer, params T[] items) {
			Items = new List<T>(items);
			this.comparer = comparer;
			Items.Sort(comparer);
		}

		/// <summary>
		/// Adds an item to the collection.
		/// </summary>
		/// <param name="item">The element to be added to the collection.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Add(T item) {
			Items.Insert(FindTargetIndex(item), item);
		}

		/// <summary>
		/// Adds a range of items to the collection.
		/// </summary>
		/// <param name="items">The elements to be added to the collection.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void AddRange(IEnumerable<T> items) {
			foreach (T item in items)
				Add(item);
		}

		/// <summary>
		/// Adds a range of items to the collection.
		/// </summary>
		/// <param name="items">The elements to be added to the collection.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void AddRange(T[] items) {
			AddRange((IEnumerable<T>) items);
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
			return IndexOf(item) != -1;
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
			int insertIndex = FindTargetIndex(item);
			if (insertIndex == Items.Count)
				return -1;
			if (comparer.Compare(item, Items[insertIndex]) == 0) {
				int index = insertIndex;
				while (index > 0 && comparer.Compare(item, Items[index - 1]) == 0)
					index--;
				return index;
			}
			return -1;
		}

		/// <summary>
		/// Adds an element to the list. The index is ignored, the item is inserted at the appropriate sorted position.
		/// </summary>
		/// <param name="index">Ignored.</param>
		/// <param name="item">The object to be inserted into the collection as an element.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Insert(int index, T item) {
			Add(item);
		}

		/// <summary>
		/// Inserts items into the collection at a specified index. The index is ignored, the items are inserted at the appropriate sorted position.
		/// </summary>
		/// <param name="index">Ignored.</param>
		/// <param name="items">The objects to be inserted into the collection as an element.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void InsertRange(int index, IEnumerable<T> items) {
			AddRange(items);
		}

		/// <summary>
		/// Removes the first occurrence of a specified item from the collection. Returns true if removal was successful, else false.
		/// </summary>
		/// <param name="item">The object to remove from the collection.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool Remove(T item) {
			int index = IndexOf(item);
			if (index >= 0) {
				Items.RemoveAt(index);
				return true;
			} else
				return false;
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
		/// Reverses the list. The comparer is also reversed.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Reverse() {
			Extensions.Reverse(comparer);
			Items.Reverse();
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

		private int FindTargetIndex(T item) {
			if (Items.Count == 0)
				return 0;
			int lowerIndex = 0;
			int upperIndex = Items.Count - 1;
			int comparisonResult;
			while (lowerIndex < upperIndex) {
				int middleIndex = (lowerIndex + upperIndex) / 2;
				T middle = Items[middleIndex];
				comparisonResult = comparer.Compare(middle, item);
				if (comparisonResult == 0)
					return middleIndex;
				else if (comparisonResult > 0) // middle > item
					upperIndex = middleIndex - 1;
				else // middle < item
					lowerIndex = middleIndex + 1;
			}
			// At this point any entry following 'middle' is greater than 'item',
			// and any entry preceding 'middle' is lesser than 'item'.
			// So we either put 'item' before or after 'middle'.
			comparisonResult = comparer.Compare(Items[lowerIndex], item);
			return comparisonResult < 0 ? lowerIndex + 1 : lowerIndex;
		}

		/// <summary>
		/// Gets the items from a SortedList. Please do not modify the returned list. If you need to, copy the elements to a new list.
		/// </summary>
		/// <param name="list">The list whose items to get.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static explicit operator List<T>(SortedList<T> list) {
			return list.Items;
		}

		/// <summary>
		/// Initializes a new SortedList from a list.
		/// </summary>
		/// <param name="list">The list of items to start with.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static explicit operator SortedList<T>(List<T> list) {
			return new SortedList<T>(list);
		}
	}
}