using System.Drawing;

namespace System.Windows.Forms {
	/// <summary>
	/// The event argument for a touch event.
	/// </summary>
	public class TouchEventArgs : EventArgs {
		/// <summary>
		/// Identifies the ID of the event. It is consistent across the corresponding TouchDown, TouchMove and TouchUp events.
		/// </summary>
		public readonly int ID;
		/// <summary>
		/// The bounds of the touch event
		/// </summary>
		public readonly Rectangle Bounds;
		/// <summary>
		/// Whether the touch event corresponds to a primary/initial/first contact point
		/// </summary>
		public readonly bool Primary;
		/// <summary>
		/// Whether the touch event was caused by palm contact to the screen
		/// </summary>
		public readonly bool IsPalm;

		/// <summary>
		/// Initializes a touch event description.
		/// </summary>
		/// <param name="id">Identifies the ID of the event. It is consistent across the corresponding TouchDown, TouchMove and TouchUp events.</param>
		/// <param name="bounds">The bounds of the touch event</param>
		/// <param name="primary">Whether the touch event corresponds to a primary/initial/first contact point</param>
		/// <param name="isPalm">Whether the touch event was caused by palm contact to the screen</param>
		public TouchEventArgs(int id, Rectangle bounds, bool primary, bool isPalm) {
			ID = id;
			Bounds = bounds;
			Primary = primary;
			IsPalm = isPalm;
		}
	}
}