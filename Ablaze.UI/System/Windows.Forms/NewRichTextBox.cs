using System.ComponentModel;
using System.Platforms.Windows;

namespace System.Windows.Forms {
	/// <summary>
	/// A RichTextBox instance that uses newer API.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxItemFilter("System.Windows.Forms")]
	[DesignTimeVisible(true)]
	[DesignerCategory("CommonControls")]
	[Description("A RichTextBox instance that uses newer API.")]
	[DisplayName(nameof(NewRichTextBox))]
	public class NewRichTextBox : RichTextBox {
		/// <summary>
		/// Specifies that the RichTextBox instance should use RichEdit50W API instead of the old RichEdit20W.
		/// </summary>
		protected override CreateParams CreateParams {
			get {
				CreateParams parameters = base.CreateParams;
				try {
					NativeApi.LoadLibrary("MsftEdit.dll");
					parameters.ClassName = "RichEdit50W";
				} catch {
				}
				return parameters;
			}
		}

		/// <summary>
		/// Initializes a RichTextBox instance that uses RichEdit50W API instead of the old RichEdit20W.
		/// </summary>
		public NewRichTextBox() {
			SetStyle(ControlStyles.CacheText, true);
		}

		/// <summary>
		/// Initializes a NewRichTextBox with the specified text.
		/// </summary>
		/// <param name="text">The text to initialize the RichTextBox with.</param>
		public NewRichTextBox(string text) : this() {
			Text = text;
		}

		/// <summary>
		/// Gets a string that describes this instance.
		/// </summary>
		public override string ToString() {
			return nameof(NewRichTextBox) + ": " + Text;
		}
	}
}