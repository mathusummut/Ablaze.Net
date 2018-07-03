using System;
using System.Drawing;
using System.Graphics.Models;
using System.Graphics.OGL;
using System.Windows.Forms;

namespace GDITest {
	public class GdiTest : GraphicsForm {
		private CheckBox checkBox1;
		private CheckBox checkBox2;
		private StyledLabel statusLabel;
		
		public GdiTest() {
			InitializeComponent();
			InitializeGL(true);
			IsGdiEnabled = true;
			statusLabel = new StyledLabel() {
				BackColor = Color.Transparent,
				Blur = 4,
				Font = new Font("Cambria", 13f, FontStyle.Regular, GraphicsUnit.Point, 0),
				ForeColor = Color.White,
				LineSpacingMultiplier = 0.25f,
				Name = "Status",
				Outline = Color.Transparent,
				ReduceCaching = true,
				RenderShadow = true,
				ShadowOpacity = 1.35f,
				Bounds = new Rectangle(10, 10, 50, 50),
				Text = "OK"
			};
			GdiControls.Add(statusLabel);
			StyledButton button = new StyledButton();
			button.Click += Button_Click;
			//button.Location = new Point(100, 100);
			statusLabel.Controls.Add(button);
			//button = new StyledButton();
			button.Location = new Point(100, 200);
			//GdiControls.Add(button);
			StyledCheckBox checkBox = new StyledCheckBox();
			checkBox.Location = new Point(110, 250);
			checkBox.Text = "checkBox1";
			checkBox.Size = new Size(100, 20);
			GdiControls.Add(checkBox);
			Form form = new Form() {
				Location = new Point(100, 100),
				Size = new Size(200, 100)
			};
			GdiControls.Add(form);
			form.Controls.Add(button);
			Label label = new Label();
			label.Text = "ayyy";
			label.Location = new Point(2, 2);
			form.Controls.Add(label);
			Button button2 = new Button();
			button.Text = "oh";
			button2.Location = new Point(20, 5);
			button2.Click += Button2_Click;
			form.Controls.Add(button2);
			EnableFullscreenOnAltEnter = true;
		}

		private void Button2_Click(object sender, EventArgs e) {
			Console.WriteLine("Inner button clicked");
		}

		private void Button_Click(object sender, EventArgs e) {
			Console.WriteLine("Button clicked");
		}

		/// <summary>
		/// Called when the OpenGL context is initialized.
		/// </summary>
		protected override void OnGLInitialized() {
			base.OnGLInitialized();
			MeshComponent.SetupGLEnvironment();
			GL.Enable(EnableCap.Light0);
			GL.Light(LightName.Light0, LightParameter.Ambient, Color4.White);
		}

		protected override void OnPaintGL() {
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			base.OnPaintGL();
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		public static void Main() {
			MessageLoop.Run(new GdiTest());
		}

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.checkBox2 = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Checked = true;
			this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox1.Location = new System.Drawing.Point(44, 260);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(88, 17);
			this.checkBox1.TabIndex = 1;
			this.checkBox1.Text = "Use OpenGL";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
			// 
			// checkBox2
			// 
			this.checkBox2.AutoSize = true;
			this.checkBox2.Location = new System.Drawing.Point(162, 261);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new System.Drawing.Size(119, 17);
			this.checkBox2.TabIndex = 2;
			this.checkBox2.Text = "Border on GDI layer";
			this.checkBox2.UseVisualStyleBackColor = true;
			this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
			// 
			// GdiTest
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.ClientSize = new System.Drawing.Size(416, 336);
			this.Controls.Add(this.checkBox2);
			this.Controls.Add(this.checkBox1);
			this.Name = "GdiTest";
			this.Text = "GDI Test";
			this.Controls.SetChildIndex(this.checkBox1, 0);
			this.Controls.SetChildIndex(this.checkBox2, 0);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e) {
			if (checkBox1.Checked)
				InitializeGL(true);
			else
				DisposeGLContext(true);
		}

		private void checkBox2_CheckedChanged(object sender, EventArgs e) {
			//BorderOnGdiLayer = checkBox2.Checked;
		}
	}
}
