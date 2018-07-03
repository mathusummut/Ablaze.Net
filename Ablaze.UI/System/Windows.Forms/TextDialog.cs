using System.ComponentModel;

namespace System.Windows.Forms {
	/// <summary>
	/// A simple text prompt dialog.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxItemFilter("System.Windows.Forms")]
	[DesignTimeVisible(true)]
	[Description("A simple text prompt dialog.")]
	[DisplayName(nameof(TextDialog))]
	[DefaultEvent(nameof(Load))]
	public class TextDialog : StyledForm {
		/// <summary>
		/// The prompt label.
		/// </summary>
		public StyledLabel Label;
		/// <summary>
		/// The text box that contains the user input.
		/// </summary>
		public TextBox TextBox;
		/// <summary>
		/// The OK button.
		/// </summary>
		public StyledButton Button;
		/// <summary>
		/// The resultant user input.
		/// </summary>
		public string Input;

		/// <summary>
		/// Initializes a new prompt dialog with the default parameters.
		/// </summary>
		public TextDialog() {
			InitializeComponent();
			MaximizeEnabled = false;
			MinimizeEnabled = false;
		}

		/// <summary>
		/// Initializes a new prompt dialog with the default parameters.
		/// </summary>
		/// <param name="container">The container to add this dialog to.</param>
		public TextDialog(IContainer container) : this() {
			if (container != null)
				container.Add(this);
		}

		/// <summary>
		/// Initializes a new prompt dialog.
		/// </summary>
		/// <param name="text">The prompt text.</param>
		/// <param name="title">The title caption of the dialog.</param>
		/// <param name="button">The button text.</param>
		public TextDialog(string text, string title = "Prompt", string button = "OK") : this() {
			Text = title;
			Label.Text = text;
			Button.Text = button;
			OnResize(EventArgs.Empty);
		}

		private void button1_Click(object sender, EventArgs e) {
			Input = TextBox.Text;
			DialogResult = DialogResult.OK;
			CloseAsync();
		}

		/// <summary>
		/// Called when the dialog is resized.
		/// </summary>
		protected override void OnResize(EventArgs e) {
			base.OnResize(e);
			if (Height != 147)
				Height = 147;
			if (Label != null) {
				Label.Width = ViewSize.Width - 15;
				TextBox.Width = Label.Width;
				Button.Width = TextBox.Width;
			}
		}

		/// <summary>
		/// Initializes the window and its controls.
		/// </summary>
		private void InitializeComponent() {
			this.Label = new System.Windows.Forms.StyledLabel();
			this.TextBox = new System.Windows.Forms.TextBox();
			this.Button = new System.Windows.Forms.StyledButton();
			this.SuspendLayout();
			// 
			// Label
			// 
			this.Label.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Tile;
			this.Label.Font = new System.Drawing.Font("Calibri Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.Label.Location = new System.Drawing.Point(15, 45);
			this.Label.Name = "Label";
			this.Label.Outline = System.Drawing.Color.Transparent;
			this.Label.OutlinePen = null;
			this.Label.Size = new System.Drawing.Size(313, 19);
			this.Label.TabIndex = 0;
			this.Label.Text = "Add Entry:";
			this.Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.Label.TextBrush = null;
			this.Label.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// TextBox
			// 
			this.TextBox.Location = new System.Drawing.Point(15, 68);
			this.TextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.TextBox.Name = "TextBox";
			this.TextBox.Size = new System.Drawing.Size(313, 26);
			this.TextBox.TabIndex = 1;
			// 
			// Button
			// 
			this.Button.Font = new System.Drawing.Font("Calibri Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.Button.ForeColor = System.Drawing.Color.Black;
			this.Button.Location = new System.Drawing.Point(15, 102);
			this.Button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Button.Name = "Button";
			this.Button.Size = new System.Drawing.Size(313, 30);
			this.Button.TabIndex = 2;
			this.Button.Text = "OK";
			this.Button.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.Button.UseVisualStyleBackColor = true;
			this.Button.Click += new System.EventHandler(this.button1_Click);
			// 
			// TextDialog
			// 
			this.AcceptButton = this.Button;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 18F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(344, 147);
			this.Controls.Add(this.Button);
			this.Controls.Add(this.TextBox);
			this.Controls.Add(this.Label);
			this.Font = new System.Drawing.Font("Calibri Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(1024, 147);
			this.MinimizeBox = false;
			this.MinimizeEnabled = false;
			this.MinimumSize = new System.Drawing.Size(342, 147);
			this.Name = "TextDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Prompt";
			this.Controls.SetChildIndex(this.Label, 0);
			this.Controls.SetChildIndex(this.TextBox, 0);
			this.Controls.SetChildIndex(this.Button, 0);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}