namespace System.Windows.Forms.Layout {
	/// <summary>
	/// Represents a dummy layout engine that does not do anything.
	/// </summary>
	public sealed class DummyLayoutEngine : LayoutEngine {
		/// <summary>
		/// A shared instance of the dummy layout engine.
		/// </summary>
		public static readonly DummyLayoutEngine Instance = new DummyLayoutEngine();

		/// <summary>
		/// Does absolutely nothing.
		/// </summary>
		public override void InitLayout(object child, BoundsSpecified specified) {
		}

		/// <summary>
		/// Does absolutely nothing.
		/// </summary>
		public override bool Layout(object container, LayoutEventArgs layoutEventArgs) {
			return false;
		}
	}
}