using System.ComponentModel;
using System.Drawing;
using System.Platforms.Windows;

namespace System.Windows.Forms {
	/// <summary>
	/// A transparent control for background overlay.
	/// </summary>
	[ToolboxItem(true)]
	[DesignTimeVisible(true)]
	[DesignerCategory("CommonControls")]
	[ToolboxItemFilter("System.Windows.Forms")]
	[Description("A transparent control for background overlay.")]
	[DisplayName(nameof(TransparentControl))]
	public class TransparentControl : Control {
		/// <summary>
		/// Signals that the control is click-through (new IntPtr(-1)).
		/// </summary>
		public static readonly IntPtr HTTRANSPARENT = new IntPtr((int) NCHITTEST.HTTRANSPARENT);

		/// <summary>
		/// Initializes the control as transparent.
		/// </summary>
		protected override CreateParams CreateParams {
			get {
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= (int) ExtendedWindowStyle.Transparent;
				return cp;
			}
		}

		/// <summary>
		/// Gets or sets whether the control is click-through or not.
		/// </summary>
		[Description("Gets or sets whether the control is click-through or not.")]
		[DefaultValue(false)]
		public virtual bool ClickThrough {
			get;
			set;
		}

		/// <summary>
		/// Initializes a new transparent control.
		/// </summary>
		public TransparentControl() {
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer | ControlStyles.CacheText, true);
			BackColor = Color.Transparent;
		}

		/// <summary>
		/// Overrides some Windows messages of the underlying control.
		/// </summary>
		/// <param name="m">The message received.</param>
		protected override void WndProc(ref Message m) {
			if (ClickThrough && m.Msg == (int) WindowMessage.NCHITTEST)
				m.Result = HTTRANSPARENT;
			else
				base.WndProc(ref m);
		}
	}
}