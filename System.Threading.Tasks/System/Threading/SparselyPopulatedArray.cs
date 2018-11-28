namespace System.Threading {
	/// <summary>
	/// A sparsely populated array.  Elements can be sparse and some null, but this allows for
	/// lock-free additions and growth, and also for constant time removal (by nulling out).
	/// </summary>
	/// <typeparam name="T">The kind of elements contained within.</typeparam>
	internal class SparselyPopulatedArray<T> where T : class {
		private readonly SparselyPopulatedArrayFragment<T> m_head;
		private volatile SparselyPopulatedArrayFragment<T> m_tail;

		/// <summary>The head of the doubly linked list.</summary>
		internal SparselyPopulatedArrayFragment<T> Head {
			get {
				return this.m_head;
			}
		}

		/// <summary>The tail of the doubly linked list.</summary>
		internal SparselyPopulatedArrayFragment<T> Tail {
			get {
				return this.m_tail;
			}
		}

		/// <summary>Allocates a new array with the given initial size.</summary>
		/// <param name="initialSize">How many array slots to pre-allocate.</param>
		internal SparselyPopulatedArray(int initialSize) {
			this.m_head = this.m_tail = new SparselyPopulatedArrayFragment<T>(initialSize);
		}

		/// <summary>
		/// Adds an element in the first available slot, beginning the search from the tail-to-head.
		/// If no slots are available, the array is grown.  The method doesn't return until successful.
		/// </summary>
		/// <param name="element">The element to add.</param>
		/// <returns>Information about where the add happened, to enable O(1) deregistration.</returns>
		internal SparselyPopulatedArrayAddInfo<T> Add(T element) {
			while (true) {
				SparselyPopulatedArrayFragment<T> prev;
				SparselyPopulatedArrayFragment<T> populatedArrayFragment;
				do {
					prev = this.m_tail;
					while (prev.m_next != null)
						this.m_tail = prev = prev.m_next;
					for (SparselyPopulatedArrayFragment<T> source = prev; source != null; source = source.m_prev) {
						if (source.m_freeCount < 1)
							--source.m_freeCount;
						if (source.m_freeCount > 0 || source.m_freeCount < -10) {
							int length = source.Length;
							int num1 = (length - source.m_freeCount) % length;
							if (num1 < 0) {
								num1 = 0;
								--source.m_freeCount;
							}
							for (int index1 = 0; index1 < length; ++index1) {
								int index2 = (num1 + index1) % length;
								if ((object) source.m_elements[index2] == null && (object) Interlocked.CompareExchange<T>(ref source.m_elements[index2], element, default(T)) == null) {
									int num2 = source.m_freeCount - 1;
									source.m_freeCount = num2 > 0 ? num2 : 0;
									return new SparselyPopulatedArrayAddInfo<T>(source, index2);
								}
							}
						}
					}
					populatedArrayFragment = new SparselyPopulatedArrayFragment<T>(prev.m_elements.Length == 4096 ? 4096 : prev.m_elements.Length * 2, prev);
				}
				while (Interlocked.CompareExchange<SparselyPopulatedArrayFragment<T>>(ref prev.m_next, populatedArrayFragment, (SparselyPopulatedArrayFragment<T>) null) != null);
				this.m_tail = populatedArrayFragment;
			}
		}
	}
}