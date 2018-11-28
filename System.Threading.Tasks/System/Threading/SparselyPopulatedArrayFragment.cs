namespace System.Threading {
	/// <summary>
	/// A fragment of a sparsely populated array, doubly linked.
	/// </summary>
	/// <typeparam name="T">The kind of elements contained within.</typeparam>
	internal class SparselyPopulatedArrayFragment<T> where T : class {
		internal readonly T[] m_elements;
		internal volatile int m_freeCount;
		internal volatile SparselyPopulatedArrayFragment<T> m_next;
		internal volatile SparselyPopulatedArrayFragment<T> m_prev;

		internal T this[int index] {
			get {
				return this.m_elements[index];
			}
		}

		internal int Length {
			get {
				return this.m_elements.Length;
			}
		}

		internal SparselyPopulatedArrayFragment<T> Next {
			get {
				return this.m_next;
			}
		}

		internal SparselyPopulatedArrayFragment<T> Prev {
			get {
				return this.m_prev;
			}
		}

		internal SparselyPopulatedArrayFragment(int size)
		  : this(size, (SparselyPopulatedArrayFragment<T>) null) {
		}

		internal SparselyPopulatedArrayFragment(int size, SparselyPopulatedArrayFragment<T> prev) {
			this.m_elements = new T[size];
			this.m_freeCount = size;
			this.m_prev = prev;
		}

		internal T SafeAtomicRemove(int index, T expectedElement) {
			T obj = Interlocked.CompareExchange<T>(ref this.m_elements[index], default(T), expectedElement);
			if ((object) obj != null)
				++this.m_freeCount;
			return obj;
		}
	}
}