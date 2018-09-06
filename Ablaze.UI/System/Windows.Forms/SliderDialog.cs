using System.ComponentModel;

namespace System.Windows.Forms {
	/// <summary>
	/// A simple slider dialog prompt dialog.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxItemFilter("System.Windows.Forms")]
	[DesignTimeVisible(true)]
	[Description("A simple slider dialog prompt dialog.")]
	[DisplayName(nameof(SliderDialog))]
	[DefaultEvent(nameof(Load))]
	public class SliderDialog : StyledForm {
		/// <summary>
		/// The silder used with this instance.
		/// </summary>
		public StyledSlider StyledSlider;
		private TextBox TextBox;

		/// <summary>
		/// The OK button.
		/// </summary>
		public StyledButton Button;

		/// <summary>
		/// Gets or sets whether to remove the restriction on the value that can be written in the text box.
		/// </summary>
		[Description("Gets or sets whether to remove the restriction on the value that can be written in the text box.")]
		[DefaultValue(false)]
		public bool AllowValuesOutsideRange {
			get;
			set;
		}

		/// <summary>
		/// Gets the current value of the slider.
		/// </summary>
		[Browsable(false)]
		public float Value {
			get {
				return StyledSlider.Value;
			}
		}

		/// <summary>
		/// Initializes a new slider dialog.
		/// </summary>
		public SliderDialog() : this("Set New Value", 0f, 100f, 50f) {
		}

		/// <summary>
		/// Initializes a new slider dialog.
		/// </summary>
		/// <param name="container">The container to add this dialog to.</param>
		public SliderDialog(IContainer container) : this() {
			if (container != null)
				container.Add(this);
		}

		/// <summary>
		/// Initializes a new slider dialog.
		/// </summary>
		/// <param name="min">The minimum value.</param>
		/// <param name="max">The maximum value.</param>
		/// <param name="value">The current value.</param>
		public SliderDialog(float min, float max, float value) : this("Set New Value", min, max, value) {
		}

		/// <summary>
		/// Initializes a new slider dialog.
		/// </summary>
		/// <param name="text">The text to show next to the slider.</param>
		/// <param name="min">The minimum value.</param>
		/// <param name="max">The maximum value.</param>
		/// <param name="value">The current value.</param>
		/// <param name="increment">The increments supported by the slider (ex. 0 for no limit, 1 for whole numbers, 2 for even numbers...).</param>
		/// <param name="allowValuesOutsideRange">Whether to remove the restriction on the value that can be written in the text box.</param>
		public SliderDialog(string text, float min, float max, float value, float increment = 0f, bool allowValuesOutsideRange = false) {
			InitializeComponent();
			AllowValuesOutsideRange = allowValuesOutsideRange;
			StyledSlider.Reset(min, max, value, increment);
			Text = text == null ? string.Empty : text.Trim();
			MaximizeEnabled = false;
			MinimizeEnabled = false;
		}

		/// <summary>
		/// Sets whether the prompt is visible.
		/// </summary>
		/// <param name="value">The visibility flag.</param>
		protected override void SetVisibleCore(bool value) {
			base.SetVisibleCore(DesignMode ? false : value);
		}

		/// <summary>
		/// Called when the dialog is shown.
		/// </summary>
		/// <param name="e">Ignored.</param>
		protected override void OnShown(EventArgs e) {
			base.OnShown(e);
			TextBox.Text = StyledSlider.Value.ToString();
			UIScaler.AddToScaler(this);
		}

		private void button_Click(object sender, EventArgs e) {
			DialogResult = DialogResult.OK;
			CloseAsync();
		}

		private void textBox1_TextChanged(object sender, EventArgs e) {
			float value;
			if (float.TryParse(TextBox.Text, out value)) {
				if (AllowValuesOutsideRange) {
					if (value > StyledSlider.Maximum)
						StyledSlider.Maximum = value;
					else if (value < StyledSlider.Minimum)
						StyledSlider.Minimum = value;
				}
				StyledSlider.Value = value;
			}
		}

		private void StyledSlider_ValueChanged(object sender, EventArgs e) {
			TextBox.Text = StyledSlider.Value.ToString();
		}

		/// <summary>
		/// Initializes the window and its controls.
		/// </summary>
		private void InitializeComponent() {
			this.Button = new System.Windows.Forms.StyledButton();
			this.StyledSlider = new System.Windows.Forms.StyledSlider();
			this.TextBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// Button
			// 
			this.Button.ForeColor = System.Drawing.Color.Black;
			this.Button.Location = new System.Drawing.Point(14, 72);
			this.Button.Name = "Button";
			this.Button.Size = new System.Drawing.Size(307, 27);
			this.Button.TabIndex = 2;
			this.Button.Text = "OK";
			this.Button.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.Button.UseVisualStyleBackColor = true;
			this.Button.Click += new System.EventHandler(this.button_Click);
			// 
			// StyledSlider
			// 
			this.StyledSlider.BackColor = System.Drawing.Color.Transparent;
			this.StyledSlider.LabelPadding = 2;
			this.StyledSlider.Location = new System.Drawing.Point(14, 39);
			this.StyledSlider.Name = "StyledSlider";
			this.StyledSlider.Size = new System.Drawing.Size(245, 23);
			this.StyledSlider.TabIndex = 4;
			this.StyledSlider.Text = "Value";
			this.StyledSlider.ValueChanged += new System.EventHandler(this.StyledSlider_ValueChanged);
			// 
			// TextBox
			// 
			this.TextBox.Location = new System.Drawing.Point(265, 41);
			this.TextBox.Name = "TextBox";
			this.TextBox.Size = new System.Drawing.Size(56, 20);
			this.TextBox.TabIndex = 5;
			this.TextBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			// 
			// SliderDialog
			// 
			this.AcceptButton = this.Button;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(342, 118);
			this.Controls.Add(this.TextBox);
			this.Controls.Add(this.StyledSlider);
			this.Controls.Add(this.Button);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimizeEnabled = false;
			this.Name = "SliderDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Set new value";
			this.Controls.SetChildIndex(this.Button, 0);
			this.Controls.SetChildIndex(this.StyledSlider, 0);
			this.Controls.SetChildIndex(this.TextBox, 0);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}