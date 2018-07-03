namespace System.Windows.Forms {
	internal class DrawableForm : Form {
		protected override CreateParams CreateParams {
			get {
				CreateParams cp = base.CreateParams;
				cp.ClassStyle |= (int) Platforms.Windows.ClassStyle.OwnDC;
				return cp;
			}
		}
	}
}