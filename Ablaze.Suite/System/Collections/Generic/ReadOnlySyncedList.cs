using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace System.Collections.ObjectModel {
	/// <summary>
	/// Serves as a thread-safe read-only wrapper for a synced list.</summary>
	/// <typeparam name="T">The type of elements in the collection.</typeparam>
	[ComVisible(false)]
	[Serializable]
	public class ReadOnlySyncedList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IList
#if NET45
		, IReadOnlyList<T>, IReadOnlyCollection<T>
#endif
		{
		private SyncedList<T> list;

		/// <summary>
		/// Gets the number of elements contained in the wrapped list.
		/// </summary>
		public int Count {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return list.Count;
			}
		}

		/// <summary>
		/// Gets the element at the specified index.
		/// </summary>
		public T this[int index] {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return list[index];
			}
		}

		/// <summary>
		/// Returns the synced list that this wraps.
		/// </summary>
		protected SyncedList<T> Items {
			get {
				return list;
			}
		}

		/// <summary>
		/// Gets the object that is used by the SyncedList to synchronize access to the collection.
		/// </summary>
		public object SyncRoot {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return list.SyncRoot;
			}
		}

		/// <summary>
		/// Gets a value (true) that indicates whether the collection is read only.
		/// </summary>
		bool ICollection<T>.IsReadOnly {
			get {
				return true;
			}
		}

		/// <summary>
		/// Gets the element at the specified index.
		/// </summary>
		T IList<T>.this[int index] {
			get {
				return list[index];
			}
			set {
			}
		}

		/// <summary>
		/// Gets a value (true) that indicates whether the collection is fixed in size.
		/// </summary>
		bool IList.IsFixedSize {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return true;
			}
		}

		/// <summary>
		/// Gets a value (true) that indicates whether the collection is read only.
		/// </summary>
		bool IList.IsReadOnly {
			get {
				return true;
			}
		}

		/// <summary>
		/// Gets or sets the element at the specified index.
		/// </summary>
		object IList.this[int index] {
			get {
				return list[index];
			}
			set {
			}
		}

		/// <summary>
		/// Gets a value (true) that indicates whether the collection is thread safe.
		/// </summary>
		bool ICollection.IsSynchronized {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return true;
			}
		}

		/// <summary>
		/// Initializes a new read-only wrapper around the specified list.</summary>
		/// <param name="list">The list to wrap.</param>
		public ReadOnlySyncedList(SyncedList<T> list) {
			this.list = list;
		}

		/// <summary>
		/// Determines whether the collection contains an element with a specific value.
		/// </summary>
		/// <param name="item">The object to locate in the collection.</param>
		public bool Contains(T item) {
			return list.Contains(item);
		}

		/// <summary>
		/// Copies the elements of the collection to a specified array, starting at index 0.
		/// </summary>
		/// <param name="array">The destination array for the elements of type T copied from the collection.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void CopyTo(T[] array) {
			list.CopyTo(array);
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
			list.CopyTo(array, index);
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
			list.CopyTo(index, array, arrayIndex, count);
		}

		/// <summary>
		/// Copies the elements of the collection to a specified array, starting at a particular index.
		/// </summary>
		/// <param name="array">The destination array for the elements of type T copied from the collection.</param>
		/// <param name="index">The zero-based index in the array at which copying begins.</param>
		void ICollection.CopyTo(Array array, int index) {
			lock (SyncRoot)
				((ICollection) list).CopyTo(array, index);
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public IEnumerator<T> GetEnumerator() {
			return list.GetEnumerator();
		}

		/// <summary>
		/// Returns the index of the first occurrence of a value in the collection.
		/// </summary>
		/// <param name="item">The item to locate.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public int IndexOf(T item) {
			return list.IndexOf(item);
		}


		void ICollection<T>.Add(T value) {
		}

		void ICollection<T>.Clear() {
		}

		bool ICollection<T>.Remove(T value) {
			return false;
		}

		void IList<T>.Insert(int index, T value) {
		}

		void IList<T>.RemoveAt(int index) {
		}

		/// <summary>
		/// Returns the index of the first occurrence of a value in the collection.
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator() {
			return list.GetEnumerator();
		}

		int IList.Add(object value) {
			return -1;
		}

		void IList.Clear() {
		}

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

		void IList.Insert(int index, object value) {
		}

		void IList.Remove(object value) {
		}

		void IList.RemoveAt(int index) {
		}
	}
}