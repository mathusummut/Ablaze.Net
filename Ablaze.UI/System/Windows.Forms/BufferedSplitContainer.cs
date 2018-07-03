using System.ComponentModel;
using System.Reflection;

namespace System.Windows.Forms {
	/// <summary>
	/// A double-buffered split control container.
	/// </summary>
	[ToolboxItem(true)]
	[DesignTimeVisible(true)]
	[ToolboxItemFilter("System.Windows.Forms")]
	[DesignerCategory("CommonControls")]
	[Description("A double-buffered split container.")]
	[DisplayName(nameof(BufferedSplitContainer))]
	public class BufferedSplitContainer : SplitContainer {
		private static Action<Control, ControlStyles, bool> EnableDoubleBuffer = (Action<Control, ControlStyles, bool>) Delegate.CreateDelegate(typeof(Action<Control, ControlStyles, bool>), typeof(Control).GetMethod(nameof(SetStyle), BindingFlags.NonPublic | BindingFlags.Instance));

		/// <summary>
		/// Gets whether events can be raised on the control.
		/// </summary>
		protected override bool CanRaiseEvents {
			get {
				return !StyledForm.DesignMode;
			}
		}

		/// <summary>
		/// Initializes a double-buffered split control container using default parameters.
		/// </summary>
		public BufferedSplitContainer() : this(true, true) {
		}

		/// <summary>
		/// Initializes a double-buffered split control container using the specfied parameters.
		/// </summary>
		/// <param name="panel1">Whether to use double-buffering in the first panel.</param>
		/// <param name="panel2">Whether to use double-buffering in the second panel.</param>
		public BufferedSplitContainer(bool panel1, bool panel2) {
			const ControlStyles parameters = ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint;
            SetStyle(parameters, true);
			if (panel1)
				EnableDoubleBuffer(Panel1, parameters, true);
			if (panel2)
				EnableDoubleBuffer(Panel2, parameters, true);
		}
	}
}