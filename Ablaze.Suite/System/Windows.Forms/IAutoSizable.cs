using System.Drawing;

namespace System.Windows.Forms {
	/// <summary>
	/// Marks controls that support auto-size.
	/// </summary>
	public interface IAutoSizable {
		/// <summary>
		/// Gets or sets whether the control should be auto-sized.
		/// </summary>
		bool AutoSize {
			get;
			set;
		}

		/// <summary>
		/// Gets the resultant size of the control if it were to be auto-sized with both grow and shrink enabled.
		/// </summary>
		Size GetAutoSize();

		/// <summary>
		/// Gets the resultant size of the control if it were to be auto-sized with both grow and shrink enabled.
		/// </summary>
		/// <param name="maxBounds">The maximum size to allow (0 means unlimited).</param>
		/// <param name="includePadding">If true, padding is included within the size.</param>
		Size GetAutoSize(Size maxBounds, bool includePadding);

		/// <summary>
		/// Sets the size of the control to the autosize result.
		/// </summary>
		void FitToContent();
	}
}