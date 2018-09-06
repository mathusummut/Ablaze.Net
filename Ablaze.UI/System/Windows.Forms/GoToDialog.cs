using System.ComponentModel;

namespace System.Windows.Forms {
	/// <summary>
	/// A simple go-to-line prompt dialog.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxItemFilter("System.Windows.Forms")]
	[DesignTimeVisible(true)]
	[Description("A simple go-to-line prompt dialog.")]
	[DisplayName(nameof(GoToDialog))]
	[DefaultEvent(nameof(Load))]
	public class GoToDialog : StyledForm {
		/// <summary>
		/// The label showing: "Line:".
		/// </summary>
		public StyledLabel Label;
		/// <summary>
		/// The line number.
		/// </summary>
		public NumericUpDown NumericUpDown;
		/// <summary>
		/// The OK button.
		/// </summary>
		public StyledButton Button;

		/// <summary>
		/// Initializes the go to dialog.
		/// </summary>
		public GoToDialog() {
			InitializeComponent();
			MaximizeEnabled = false;
			MinimizeEnabled = false;
		}

		/// <summary>
		/// Initializes the go to dialog.
		/// </summary>
		/// <param name="container">The container to add this dialog to.</param>
		public GoToDialog(IContainer container) : this() {
			if (container != null)
				container.Add(this);
		}

		/// <summary>
		/// Called when the dialog is shown.
		/// </summary>
		/// <param name="e">Ignored.</param>
		protected override void OnShown(EventArgs e) {
			base.OnShown(e);
			UIScaler.AddToScaler(this);
		}

		/// <summary>
		/// Sets whether the prompt is visible.
		/// </summary>
		/// <param name="value">The visibility flag.</param>
		protected override void SetVisibleCore(bool value) {
			base.SetVisibleCore(DesignMode ? false : value);
		}

		private void button_Click(object sender, EventArgs e) {
			DialogResult = DialogResult.OK;
			CloseAsync();
		}

		/// <summary>
		/// Initializes the window and its controls.
		/// </summary>
		private void InitializeComponent() {
			this.Label = new System.Windows.Forms.StyledLabel();
			this.NumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.Button = new System.Windows.Forms.StyledButton();
			((System.ComponentModel.ISupportInitialize) (this.NumericUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// Label
			// 
			this.Label.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Tile;
			this.Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.Label.Location = new System.Drawing.Point(14, 39);
			this.Label.Name = "Label";
			this.Label.Outline = System.Drawing.Color.Transparent;
			this.Label.OutlinePen = null;
			this.Label.Size = new System.Drawing.Size(60, 26);
			this.Label.TabIndex = 0;
			this.Label.Text = "Line:";
			this.Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.Label.TextBrush = null;
			this.Label.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// NumericUpDown
			// 
			this.NumericUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.NumericUpDown.Location = new System.Drawing.Point(80, 40);
			this.NumericUpDown.Maximum = new decimal(new int[] {
			2147483647,
			0,
			0,
			0});
			this.NumericUpDown.Minimum = new decimal(new int[] {
			1,
			0,
			0,
			0});
			this.NumericUpDown.Name = "NumericUpDown";
			this.NumericUpDown.Size = new System.Drawing.Size(133, 26);
			this.NumericUpDown.TabIndex = 1;
			this.NumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.NumericUpDown.Value = new decimal(new int[] {
			1,
			0,
			0,
			0});
			// 
			// Button
			// 
			this.Button.ForeColor = System.Drawing.Color.Black;
			this.Button.Location = new System.Drawing.Point(14, 72);
			this.Button.Name = "Button";
			this.Button.Size = new System.Drawing.Size(199, 27);
			this.Button.TabIndex = 2;
			this.Button.Text = "OK";
			this.Button.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.Button.UseVisualStyleBackColor = true;
			this.Button.Click += new System.EventHandler(this.button_Click);
			// 
			// GoToDialog
			// 
			this.AcceptButton = this.Button;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(228, 114);
			this.Controls.Add(this.Button);
			this.Controls.Add(this.NumericUpDown);
			this.Controls.Add(this.Label);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimizeEnabled = false;
			this.Name = "GoToDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Go To...";
			this.Controls.SetChildIndex(this.Label, 0);
			this.Controls.SetChildIndex(this.NumericUpDown, 0);
			this.Controls.SetChildIndex(this.Button, 0);
			((System.ComponentModel.ISupportInitialize) (this.NumericUpDown)).EndInit();
			this.ResumeLayout(false);

		}
	}
}