using System.Runtime.CompilerServices;

namespace System.Collections.Generic {
	/// <summary>
	/// Enumerates through all elements of a List.
	/// </summary>
	[Serializable]
	public struct FastEnumerator<T> : IEnumerator<T>, IEnumerator, IDisposable {
		private List<T> list;
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
		/// Initializes a FastEnumarator to enumerate a List.
		/// </summary>
		/// <param name="list">The list to enumerate.</param>
		public FastEnumerator(List<T> list) {
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
			if (Index < list.Count) {
				current = list[Index];
				return true;
			} else
				return false;
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