namespace System.Windows.Forms {
	/// <summary>
	/// Represents an item renderer.
	/// </summary>
	public interface IItemRenderer : ISmartControl {
		/// <summary>
		/// Gets the renderer used for styling items.
		/// </summary>
		StyleRenderer ItemRenderer {
			get;
		}
	}
}