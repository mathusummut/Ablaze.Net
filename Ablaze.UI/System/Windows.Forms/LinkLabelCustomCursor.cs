using System.ComponentModel;

namespace System.Windows.Forms {
	/// <summary>
	/// A link label that exposes the ability to override cursor when hovering over link.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxItemFilter("System.Windows.Forms")]
	[DesignTimeVisible(true)]
	[DesignerCategory("CommonControls")]
	[Description("A link label that exposes the ability to override cursor when hovering over link.")]
	[DisplayName(nameof(LinkLabelCustomCursor))]
	[DefaultEvent(nameof(LinkClicked))]
	public class LinkLabelCustomCursor : LinkLabel {
		/// <summary>
		/// Gets or sets the cursor to use when hovering over a link.
		/// </summary>
		[Description("Gets or sets the cursor to use when hovering over a link.")]
		public Cursor LinkCursor {
			get;
			set;
		}

		/// <summary>
		/// Initializes the link label.
		/// </summary>
		public LinkLabelCustomCursor() {
		}

		/// <summary>
		/// Initializes the link label using the specified text.
		/// </summary>
		/// <param name="text">The text to initialize the label with.</param>
		public LinkLabelCustomCursor(string text) {
			if (text != null)
				Text = text;
		}

		/// <summary>
		/// Overrides the event when the mouse moves.
		/// </summary>
		protected override void OnMouseMove(MouseEventArgs e) {
			base.OnMouseMove(e);
			if (LinkCursor == null)
				return;
			OverrideCursor = LinkCursor;
			Cursor = OverrideCursor;
		}
	}
}