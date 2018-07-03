namespace System.Collections.Generic {
	/// <summary>
	/// Wraps a comparer, generally used for sorting.
	/// </summary>
	/// <typeparam name="T">The type of element this instance compares.</typeparam>
	public sealed class ComparisonComparer<T> : IComparer<T> {
		private Comparison<T> comparison;

		/// <summary>
		/// Initializes a new comparison comparer wrapper.
		/// </summary>
		/// <param name="comparison">The delegate that performs the actual comparison.</param>
		public ComparisonComparer(Comparison<T> comparison) {
			this.comparison = comparison;
		}

		/// <summary>
		/// Returns -1 if x is smaller than y, 0 if x equals y, or 1 if x is larger than y.
		/// </summary>
		/// <param name="x">The reference value.</param>
		/// <param name="y">The value to compare with the reference value.</param>
		public int Compare(T x, T y) {
			return comparison(x, y);
		}
	}
}