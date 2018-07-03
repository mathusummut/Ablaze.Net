using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace System.Collections.Generic {
	/// <summary>
	/// Provides a thread-safe collection that represents a queue, but prioritized elements are dequeued (popped) first.
	/// Small values have greater priority than larger values.
	/// </summary>
	public class SyncedPriorityQueue<T> : ICloneable {
		/// <summary>
		/// The object used to synchronize the priority queue.
		/// </summary>
		public readonly object SyncRoot = new object();
		/// <summary>
		/// The priority queue that holds the elements in this thread-safe collection.
		/// </summary>
		public readonly PriorityQueue<T> Queue;

		/// <summary>
		/// Gets a list of the items in the array (not in order).
		/// </summary>
		public ReadOnlyCollection<T> Items {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				lock (SyncRoot)
					return Queue.Items;
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
				return Queue.Count;
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
				lock (SyncRoot)
					return Queue.First;
			}
		}

		/// <summary>
		///  Initializes a new priority queue using the default comparer.
		/// </summary>
		public SyncedPriorityQueue() : this(null as IComparer<T>) {
		}

		/// <summary>
		///  Initializes a new priority queue using the specified comparer.
		/// </summary>
		/// <param name="comparer">The comparer to use. Smaller values are moved to the beginning of the queue.</param>
		public SyncedPriorityQueue(Comparison<T> comparer) : this(0, comparer) {
		}

		/// <summary>
		/// Initializes a new priority queue using the specified comparer.
		/// </summary>
		/// <param name="capacity">The initial capacity of the queue.</param>
		/// <param name="comparer">The comparer to use. Smaller values are moved to the beginning of the queue.</param>
		public SyncedPriorityQueue(int capacity, Comparison<T> comparer) : this(capacity, comparer == null ? null : Extensions.Create(comparer)) {
		}

		/// <summary>
		///  Initializes a new priority queue using the specified comparer.
		/// </summary>
		/// <param name="comparer">The comparer to use. Smaller values are moved to the beginning of the queue.</param>
		public SyncedPriorityQueue(IComparer<T> comparer) : this(0, comparer) {
		}

		/// <summary>
		/// Initializes a new priority queue using the specified comparer.
		/// </summary>
		/// <param name="capacity">The initial capacity of the queue.</param>
		/// <param name="comparer">The comparer to use. Smaller values are moved to the beginning of the queue.</param>
		public SyncedPriorityQueue(int capacity, IComparer<T> comparer) {
			Queue = new PriorityQueue<T>(capacity, comparer);
		}

		/// <summary>
		/// Initializes a SyncedPriorityQueue wrapper from the specified queue.
		/// </summary>
		/// <param name="queue">The queue to wrap.</param>
		public SyncedPriorityQueue(PriorityQueue<T> queue) {
			Queue = queue;
		}

		/// <summary>
		/// Returns whether the specified item is in the queue.
		/// </summary>
		/// <param name="item">The item to locate in the queue.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool Contains(T item) {
			lock (SyncRoot)
				return Queue.Contains(item);
		}

		/// <summary>
		/// Adds the specified item to the priority queue.
		/// </summary>
		/// <param name="item">The item to enqueue.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Enqueue(T item) {
			lock (SyncRoot)
				Queue.Enqueue(item);
		}

		/// <summary>
		/// Removes the first node (smallest) from the heap.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public T Dequeue() {
			lock (SyncRoot)
				return Queue.Dequeue();
		}

		/// <summary>
		/// Returns the queue as a sorted array.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public T[] ToArray() {
			lock (SyncRoot)
				return Queue.ToArray();
		}

		/// <summary>
		/// Retrieves the nth queue item sorted by priority.
		/// </summary>
		/// <param name="index">The queue index of the item to find (0 is the lowest priority item, 1 is second lowest...etc).</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public T GetItem(int index) {
			lock (SyncRoot)
				return Queue.GetItem(index);
		}

		/// <summary>
		/// Removes a specific item from the queue specified by its queue index (very slow).
		/// </summary>
		/// <param name="index">The queue index of the item to remove.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void RemoveAt(int index) {
			lock (SyncRoot)
				Queue.RemoveAt(index);
		}

		/// <summary>
		/// Removes a specific item from the queue, and returns whether the item was found.
		/// </summary>
		/// <param name="item">The item to remove.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool Remove(T item) {
			lock (SyncRoot)
				return Queue.Remove(item);
		}

		/// <summary>
		/// Clears the priority queue.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Clear() {
			lock (SyncRoot)
				Queue.Clear();
		}

		/// <summary>
		/// Creates a shallow copy of the queue.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public SyncedPriorityQueue<T> Copy() {
			return new SyncedPriorityQueue<T>(Queue.Copy());
		}

		/// <summary>
		/// Creates a shallow copy of the queue.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public object Clone() {
			return new SyncedPriorityQueue<T>(Queue.Copy());
		}
	}
}