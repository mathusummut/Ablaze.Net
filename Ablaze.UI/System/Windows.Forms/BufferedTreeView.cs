using System.ComponentModel;
using System.Platforms.Windows;

namespace System.Windows.Forms {
	/// <summary>
	/// A double-buffered tree view control.
	/// </summary>
	[ToolboxItem(true)]
	[DesignTimeVisible(true)]
	[ToolboxItemFilter("System.Windows.Forms")]
	[DesignerCategory("CommonControls")]
	[Description("A double-buffered tree view control.")]
	[DisplayName(nameof(BufferedTreeView))]
	public class BufferedTreeView : TreeView {
		private const int TVS_EX_DOUBLEBUFFER = 0x0004;

		/// <summary>
		/// Called when the tree view handle is created.
		/// </summary>
		/// <param name="e">Empty.</param>
		protected override void OnHandleCreated(EventArgs e) {
			NativeApi.PostMessage(Handle, WindowMessage.TVM_SETEXTENDEDSTYLE, (IntPtr) TVS_EX_DOUBLEBUFFER, (IntPtr) TVS_EX_DOUBLEBUFFER);
			base.OnHandleCreated(e);
		}
	}
}