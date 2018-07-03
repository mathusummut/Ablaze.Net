using System;
using System.Windows.Forms;

namespace MDITest {
	public class Form1 : StyledForm {
		public Form1() {
			InitializeComponent();
			IsMdiContainer = true;
		}

		protected override void OnShown(EventArgs e) {
			base.OnShown(e);
			Form2 form1 = new Form2();
			form1.Name = "First Form";
			form1.Text = "First Form";
			form1.MdiParent = this;
			form1.Show();
			Form2 form2 = new Form2();
			form2.Name = "Second Form";
			form2.Text = "Second Form";
			form2.MdiParent = this;
			form2.Show();
			Form3 form3 = new Form3();
			form3.Name = "Third Form";
			form3.Text = "Third Form";
			form3.MdiParent = this;
			form3.Show();
		}

		public static void Main() {
			MessageLoop.Run(new Form1());
		}

		private void InitializeComponent() {
			this.SuspendLayout();
			// 
			// Form1
			// 
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.ClientSize = new System.Drawing.Size(639, 441);
			this.Name = "Form1";
			this.ResumeLayout(false);

		}
	}
}