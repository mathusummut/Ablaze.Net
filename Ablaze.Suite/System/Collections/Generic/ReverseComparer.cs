namespace System.Collections.Generic {
	/// <summary>
	/// Implementation of IComparer&lt;T&gt; based on another one;
	/// this simply reverses the original comparison.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public sealed class ReverseComparer<T> : IComparer<T> {
		private IComparer<T> originalComparer;

		/// <summary>
		/// Gets the original comparer.
		/// </summary>
		public IComparer<T> OriginalComparer {
			get {
				return originalComparer;
			}
		}

		/// <summary>
		/// Creates a new reversing comparer.
		/// </summary>
		/// <param name="original">The original comparer to use for comparisons.</param>
		public ReverseComparer(IComparer<T> original) {
			originalComparer = original;
		}

		/// <summary>
		/// Returns the result of comparing the specified values using the original
		/// comparer, but reversing the order of comparison.
		/// </summary>
		public int Compare(T x, T y) {
			return originalComparer.Compare(y, x);
		}
	}
}
