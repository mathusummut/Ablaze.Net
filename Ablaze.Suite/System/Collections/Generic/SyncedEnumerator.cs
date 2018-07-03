using System.Runtime.CompilerServices;

namespace System.Collections.Generic {
	/// <summary>
	/// Enumerates through all elements of a SyncedList.
	/// </summary>
	[Serializable]
	public struct SyncedEnumerator<T> : IEnumerator<T>, IEnumerator, IDisposable {
		private SyncedList<T> list;
		private T current;
		/// <summary>
		/// The current position of the enumerator (can be -1 if uninitialized).
		/// </summary>
		public int Index;

		/// <summary>
		/// Gets current element cached by the enumerator.
		/// </summary>
		public T Current {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return current;
			}
		}

		/// <summary>
		/// Gets the element at the current position of the enumerator.
		/// </summary>
		object IEnumerator.Current {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return current;
			}
		}

		/// <summary>
		/// Initializes a SyncedEnumarator to enumerate a SyncedList.
		/// </summary>
		/// <param name="list">The list to enumerate.</param>
		public SyncedEnumerator(SyncedList<T> list) {
			this.list = list;
			Index = -1;
			current = default(T);
		}

		/// <summary>
		/// At the current index, updates Current accordingly.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool RefreshCurrent() {
			lock (list.SyncRoot) {
				if (Index < list.Items.Count)
					current = list.Items[Index];
				else
					return false;
			}
			return true;
		}

		/// <summary>
		/// Advances the enumerator to the next element of the list, and returns false if the entire list has been traversed, else false.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool MoveNext() {
			Index++;
			return RefreshCurrent();
		}

		/// <summary>
		/// Sets the enumerator to its initial position, which is before the first element in the collection.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Reset() {
			Index = -1;
			current = default(T);
		}

		/// <summary>
		/// Empty lol
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Dispose() {
		}
	}
}