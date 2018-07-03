namespace System.Collections.Generic {
	/// <summary>
	/// Comparer to daisy-chain two existing comparers and apply in sequence (i.e. sort by x then y)
	/// </summary>
	/// <typeparam name="T">The type of items to compare.</typeparam>
	public sealed class LinkedComparer<T> : IComparer<T> {
		private IComparer<T> primary, secondary;

		/// <summary>
		/// Gets or sets the first comparer to apply (null is replaced by Comparer.Default).
		/// </summary>
		public IComparer<T> Primary {
			get {
				return primary;
			}
			set {
				if (value == null)
					value = Comparer<T>.Default;
				primary = value;
			}
		}

		/// <summary>
		/// Gets or sets the secondary comparer to apply (null is replaced by Comparer.Default).
		/// </summary>
		public IComparer<T> Secondary {
			get {
				return secondary;
			}
			set {
				if (value == null)
					value = Comparer<T>.Default;
				secondary = value;
			}
		}

		/// <summary>
		/// Create a new LinkedComparer
		/// </summary>
		/// <param name="primary">The first comparison to use</param>
		/// <param name="secondary">The next level of comparison if the primary returns 0 (equivalent)</param>
		public LinkedComparer(IComparer<T> primary, IComparer<T> secondary) {
			Primary = primary;
			Secondary = secondary;
		}

		/// <summary>
		/// Compares the specified items using the specified chained comparers.
		/// </summary>
		/// <param name="x">The item to compare.</param>
		/// <param name="y">The item to compare with.</param>
		public int Compare(T x, T y) {
			int result = primary.Compare(x, y);
			return result == 0 ? secondary.Compare(x, y) : result;
		}
	}
}