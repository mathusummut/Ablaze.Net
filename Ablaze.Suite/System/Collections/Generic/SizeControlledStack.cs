using System.Runtime.CompilerServices;

namespace System.Collections.Generic {
	/// <summary>
	/// Stack with a specified maximum capacity, bottom items beyond the capacity are overrwritten when full.
	/// </summary>
	/// <typeparam name="T">The type of the elements in the stack.</typeparam>
	[Serializable]
	public class SizeControlledStack<T> : IEnumerable<T>, ICloneable {
		private LazyList<T> items;
		private int bottom, count, maximumCapacity;

		/// <summary>
		/// Gets or sets the element specified by the index in the stack.
		/// </summary>
		/// <param name="index">The index of the element. 0 refers to the first element that was pushed in the stack but not overwritten, while Count - 1 refers to the last element that was pushed.</param>
		public T this[int index] {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				if (index < 0 || index >= count)
					throw new IndexOutOfRangeException("Index " + index + " is outside the boundaries of the stack.");
				index += bottom;
				if (index >= maximumCapacity)
					index -= maximumCapacity;
				return items[index];
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				if (index < 0 || index >= count)
					throw new IndexOutOfRangeException("Index " + index + " is outside the boundaries of the stack.");
				index += bottom;
				if (index >= maximumCapacity)
					index -= maximumCapacity;
				items[index] = value;
			}
		}

		/// <summary>
		/// Gets or sets the maximum capacity of the stack.
		/// </summary>
		public int MaximumCapacity {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return maximumCapacity;
			}
			set {
				if (value < 0)
					value = 0;
				if (value == maximumCapacity)
					return;
				else if (value < maximumCapacity) {
					SizeControlledStack<T> temp = Clone(value);
					items = temp.items;
					bottom = temp.bottom;
					count = temp.count;
				}
				maximumCapacity = value;
			}
		}

		/// <summary>
		/// Gets if the stack is empty.
		/// </summary>
		public bool IsEmpty {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return count == 0;
			}
		}

		/// <summary>
		/// Gets if the stack is full (and remember, when the stack is full, the next pushed element will overwrite the oldest pushed element in the stack).
		/// </summary>
		public bool IsFull {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return count == maximumCapacity;
			}
		}

		/// <summary>
		/// Gets the number of items currently in the stack.
		/// </summary>
		public int Count {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return count;
			}
		}

		/// <summary>
		/// Initializes a new stack with the specified maximum capacity.
		/// </summary>
		/// <param name="maximumCapacity">The maximum capacity of the stack.</param>
		public SizeControlledStack(int maximumCapacity) {
			this.maximumCapacity = maximumCapacity < 0 ? 0 : maximumCapacity;
			items = new LazyList<T>();
		}

		/// <summary>
		/// Initializes a new stack with the specified maximum capacity.
		/// </summary>
		/// <param name="initialCapacity">The initial capacity of the stack.</param>
		/// <param name="maximumCapacity">The maximum capacity of the stack.</param>
		public SizeControlledStack(int initialCapacity, int maximumCapacity) {
			if (maximumCapacity < 0)
				maximumCapacity = 0;
			if (maximumCapacity < initialCapacity)
				initialCapacity = maximumCapacity;
			this.maximumCapacity = maximumCapacity;
			items = new LazyList<T>(initialCapacity);
		}

		/// <summary>
		/// Creates an enumerator for the stack.
		/// </summary>
		IEnumerator<T> IEnumerable<T>.GetEnumerator() {
			return new StackEnumerator(this);
		}

		/// <summary>
		/// Creates an enumerator for the stack.
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator() {
			return new StackEnumerator(this);
		}

		/// <summary>
		/// Inserts an item at the top of the stack.
		/// </summary>
		/// <param name="item">The item to push.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Push(T item) {
			if (IsFull)
				bottom++;
			else
				count++;
			this[count - 1] = item;
		}

		/// <summary>
		/// Returns the object at the top of the stack without removing it.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public T Peek() {
			return count == 0 ? default(T) : this[count - 1];
		}

		/// <summary>
		/// Returns the object at the top of the stack and removes it.
		/// </summary>
		public T Pop() {
			if (count == 0)
				throw new InvalidOperationException("Cannot pop from empty stack.");
			int index = bottom + count - 1;
			if (index >= maximumCapacity)
				index -= maximumCapacity;
			T removed = items[index];
			items[index] = default(T);
			count--;
			return removed;
		}

		/// <summary>
		/// Replaces the object at the top of the stack with the specified value.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Replace(T item) {
			this[count - 1] = item;
		}

		/// <summary>
		/// Replaces the object at the bottom of the stack with the specified value.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void ReplaceBottom(T item) {
			if (count == 0)
				throw new InvalidOperationException("Cannot replace bottom of empty stack.");
			items[bottom] = item;
		}

		/// <summary>
		/// Returns a shallow copy of the current instance of SizeControlledStack.
		/// </summary>
		public object Clone() {
			SizeControlledStack<T> clone = new SizeControlledStack<T>(maximumCapacity);
			for (int i = 0; i < count; i++)
				clone.Push(this[i]);
			return clone;
		}

		/// <summary>
		/// Returns a shallow copy of the current instance of SizeControlledStack with the specified maximum capacity.
		/// </summary>
		/// <param name="maxCapacity">The maximum capacity of the stack to return.</param>
		public SizeControlledStack<T> Clone(int maxCapacity) {
			SizeControlledStack<T> clone = new SizeControlledStack<T>(maxCapacity);
			for (int i = 0; i < count; i++)
				clone.Push(this[i]);
			return clone;
		}

		/// <summary>
		/// Removes all the objects from the stack.
		/// </summary>
		public void Clear() {
			bottom = 0;
			if (count != 0) {
				count = 0;
				for (int i = 0; i < count; i++)
					items[i] = default(T);
			}
		}

		/// <summary>
		/// Like Clear(), but does not take the time to delete all items from the stack, instead just sets count to 0.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void FastClear() {
			count = 0;
			bottom = 0;
		}

		[Serializable]
		private sealed class StackEnumerator : IEnumerator<T> {
			private SizeControlledStack<T> Stack;
			private int index;
			private T current;

			public T Current {
#if NET45
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
				get {
					return current;
				}
			}

			object IEnumerator.Current {
#if NET45
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
				get {
					return Current;
				}
			}

			public StackEnumerator(SizeControlledStack<T> stack) {
				Stack = stack;
			}

#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public bool MoveNext() {
				if (index == Stack.count - 1)
					return false;
				current = Stack[index];
				index++;
				return true;
			}

#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			void IEnumerator.Reset() {
				index = 0;
				current = default(T);
			}

#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public void Dispose() {
			}
		}
	}
}