using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace System.Collections.Generic {
	/// <summary>
	/// Represents a queue, but prioritized elements are dequeued (popped) first. Small values have greater priority than larger values.
	/// </summary>
	[Serializable]
	public class PriorityQueue<T> : ICloneable {
		private IComparer<T> comparer;
		private List<T> heap;

		/// <summary>
		/// Gets a list of the items in the array (not in order).
		/// </summary>
		public ReadOnlyCollection<T> Items {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return heap.AsReadOnly();
			}
		}

		/// <summary>
		/// Gets the number of items in the queue.
		/// </summary>
		public int Count {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return heap.Count;
			}
		}

		/// <summary>
		/// Gets the first or topmost item in the priority queue, which is the item with the minimum value.
		/// If the queue is empty, an ArgumentOutOfRange exception is thrown.
		/// </summary>
		public T First {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return heap[0];
			}
		}

		/// <summary>
		///  Initializes a new priority queue using the default comparer.
		/// </summary>
		public PriorityQueue() : this(0, null as IComparer<T>) {
		}

		/// <summary>
		///  Initializes a new priority queue using the default comparer.
		/// </summary>
		/// <param name="capacity">The initial capacity of the queue.</param>
		public PriorityQueue(int capacity) : this(capacity, null as IComparer<T>) {
		}

		/// <summary>
		///  Initializes a new priority queue using the specified comparer.
		/// </summary>
		/// <param name="comparer">The comparer to use. Smaller values are moved to the beginning of the queue.</param>
		public PriorityQueue(Comparison<T> comparer) : this(0, comparer) {
		}

		/// <summary>
		/// Initializes a new priority queue using the specified comparer.
		/// </summary>
		/// <param name="capacity">The initial capacity of the queue.</param>
		/// <param name="comparer">The comparer to use. Smaller values are moved to the beginning of the queue.</param>
		public PriorityQueue(int capacity, Comparison<T> comparer) : this(capacity, comparer == null ? null : Extensions.Create(comparer)) {
		}

		/// <summary>
		///  Initializes a new priority queue using the specified comparer.
		/// </summary>
		/// <param name="comparer">The comparer to use. Smaller values are moved to the beginning of the queue.</param>
		public PriorityQueue(IComparer<T> comparer) : this(0, comparer) {
		}

		/// <summary>
		/// Initializes a new priority queue using the specified comparer.
		/// </summary>
		/// <param name="capacity">The initial capacity of the queue.</param>
		/// <param name="comparer">The comparer to use. Smaller values are moved to the beginning of the queue.</param>
		public PriorityQueue(int capacity, IComparer<T> comparer) {
			heap = new List<T>(capacity > 0 ? capacity : 6);
			this.comparer = comparer == null ? Comparer<T>.Default : comparer;
		}

		private PriorityQueue(List<T> heap, IComparer<T> comparer) {
			this.heap = new List<T>(heap);
			this.comparer = comparer;
		}

		/// <summary>
		/// Returns whether the specified item is in the queue.
		/// </summary>
		/// <param name="item">The item to locate in the queue.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool Contains(T item) {
			return heap.Contains(item);
		}

		/// <summary>
		/// Adds the specified item to the priority queue.
		/// </summary>
		/// <param name="item">The item to enqueue.</param>
		public void Enqueue(T item) {
			int index = heap.Count;
			heap.Add(default(T));
			int parentIndex;
			while (index > 0) {
				parentIndex = (index - 1) >> 1;
				if (comparer.Compare(item, heap[parentIndex]) < 0) {
					heap[index] = heap[parentIndex];
					index = parentIndex;
				} else
					break;
			}
			heap[index] = item;
		}

		/// <summary>
		/// Removes the first node (smallest) from the heap.
		/// </summary>
		public T Dequeue() {
			T returnValue = heap[0];
			int parent = 0;
			int leftChild = (parent * 2) + 1;
			int rightChild, bestChild;
			int itemCount = heap.Count;
			while (leftChild < itemCount) {
				rightChild = leftChild + 1;
				bestChild = (rightChild < itemCount && comparer.Compare(heap[rightChild], heap[leftChild]) < 0) ? rightChild : leftChild;
				heap[parent] = heap[bestChild]; //promote bestChild to fill the gap left by parent.
				parent = bestChild;
				leftChild = (parent * 2) + 1;
			}
			itemCount--;
			heap[parent] = heap[itemCount]; //fill the last gap by moving the last (i.e., bottom-rightmost) node
			T pivot, value = heap[parent];
			while (parent > 0) { //Rebalance the heap
				int parentIndex = (parent - 1) >> 1;
				if (comparer.Compare(value, heap[parentIndex]) < 0) {
					// value is a better match than the parent node so exchange
					// places to preserve the "heap" property.
					pivot = heap[parent];
					heap[parent] = heap[parentIndex];
					heap[parentIndex] = pivot;
					parent = parentIndex;
				} else
					break; // Heap is balanced
			}
			heap.RemoveAt(itemCount);
			return returnValue;
		}

		/// <summary>
		/// Returns the queue as a sorted array.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public T[] ToArray() {
			T[] copy = heap.ToArray();
			Array.Sort(copy, comparer);
			return copy;
		}

		/// <summary>
		/// Retrieves the nth queue item sorted by priority.
		/// </summary>
		/// <param name="index">The queue index of the item to find (0 is the lowest priority item, 1 is second lowest...etc).</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public T GetItem(int index) {
			return ToArray()[index];
		}

		/// <summary>
		/// Removes a specific item from the queue specified by its queue index (very slow).
		/// </summary>
		/// <param name="index">The queue index of the item to remove.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void RemoveAt(int index) {
			Remove(GetItem(index));
		}

		/// <summary>
		/// Removes a specific item from the queue, and returns whether the item was found.
		/// </summary>
		/// <param name="item">The item to remove.</param>
		public bool Remove(T item) {
			int index = heap.IndexOf(item);
			if (index == -1)
				return false;
			else if (index == 0) {
				Dequeue();
				return true;
			}
			int parent;
			int last = heap.Count - 1;
			T lastItem = heap[last];
			if (last != 0 && index != last) {
				int i = index;
				parent = (i - 1) >> 1;
				while (comparer.Compare(heap[index], heap[parent]) < 0) {
					heap[i] = heap[parent];
					i = parent;
					parent = (i - 1) >> 1;
				}
				if (i == index) {
					int halfLast = last >> 1;
					int lastMinusOne = last - 1;
					int j;
					while (i < halfLast) {
						j = (2 * i) + 1;
						if ((j < lastMinusOne) && (comparer.Compare(heap[j], heap[j + 1]) > 0))
							j++;
						if (comparer.Compare(heap[j], lastItem) >= 0)
							break;
						heap[i] = heap[j];
						i = j;
					}
				}
				heap[i] = lastItem;
			}
			heap.RemoveAt(last);
			return true;
		}

		/// <summary>
		/// Clears the priority queue.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Clear() {
			heap.Clear();
		}

		/// <summary>
		/// Creates a shallow copy of the queue.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public PriorityQueue<T> Copy() {
			return new PriorityQueue<T>(heap, comparer);
		}

		/// <summary>
		/// Creates a shallow copy of the queue.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public object Clone() {
			return new PriorityQueue<T>(heap, comparer);
		}
	}
}