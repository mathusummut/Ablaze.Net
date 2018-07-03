namespace System.Collections.Generic {
	/// <summary>
	/// Returns whether the specified items are considered equal.
	/// </summary>
	/// <typeparam name="T">The type of element to compare.</typeparam>
	/// <param name="item1">The item to compare.</param>
	/// <param name="item2">The item to compare with.</param>
	public delegate bool EqualityComparison<T>(T item1, T item2);

	/// <summary>
	/// Wraps a comparer for equality testing.
	/// </summary>
	/// <typeparam name="T">The type of element this instance compares.</typeparam>
	public sealed class EqualityComparisonComparer<T> : IEqualityComparer<T> {
		private EqualityComparison<T> comparison;
		private Func<T, int> hashCode;

		/// <summary>
		/// Initializes a new EqualityComparisonComparer wrapper.
		/// </summary>
		/// <param name="comparison">The delegate that performs the actual equality comparison.</param>
		/// <param name="hashCode">The hash code generator to use. The rule is: equal items MUST have the save hash-code,
		/// and for the same item, the gerenated hash-code must always be the same.</param>
		public EqualityComparisonComparer(EqualityComparison<T> comparison, Func<T, int> hashCode) {
			this.comparison = comparison;
			this.hashCode = hashCode;
		}

		/// <summary>
		/// Returns whether the specified items are considered equal.
		/// </summary>
		/// <param name="item1">The item to compare.</param>
		/// <param name="item2">The item to compare with.</param>
		public bool Equals(T item1, T item2) {
			return comparison(item1, item2);
		}

		/// <summary>
		/// Generates a hash-code for the specified item.
		/// </summary>
		/// <param name="item">The item whose hash-code to generate.</param>
		public int GetHashCode(T item) {
			return hashCode(item);
		}
	}
}