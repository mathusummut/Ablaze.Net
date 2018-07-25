using System.ComponentModel;

namespace System.Windows.Forms {
	/// <summary>
	/// A standard simple find-and-replace dialog.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxItemFilter("System.Windows.Forms")]
	[DesignTimeVisible(true)]
	[Description("A standard simple find-and-replace dialog.")]
	[DisplayName(nameof(FindAndReplace))]
	[DefaultEvent(nameof(FindNextDownClick))]
	public class FindAndReplace : StyledForm {
		private StyledLabel label1, label2;
		private TextBox textBox1, textBox2;
		private StyledButton findNextDown, findNextUp, replaceNextUp, replaceNextDown, replaceAll;
		private StyledCheckBox matchCaseCheckBox, matchWordCheckBox;
		/// <summary>
		/// Fired when the "Find Next ↓" button is clicked.
		/// </summary>
		public event Action<FindAndReplace> FindNextDownClick;
		/// <summary>
		/// Fired when the "Find Next ↑" button is clicked.
		/// </summary>
		public event Action<FindAndReplace> FindNextUpClick;
		/// <summary>
		/// Fired when the "Replace Next ↓" button is clicked.
		/// </summary>
		public event Action<FindAndReplace> ReplaceNextDownClick;
		/// <summary>
		/// Fired when the "Replace Next ↑" button is clicked.
		/// </summary>
		public event Action<FindAndReplace> ReplaceNextUpClick;
		/// <summary>
		/// Fired when the "Replace All" button is clicked.
		/// </summary>
		public event Action<FindAndReplace> ReplaceAllClick;

		/// <summary>
		/// Gets whether the "Match Case" option is ticked.
		/// </summary>
		[Browsable(false)]
		public bool MatchCase {
			get {
				return matchCaseCheckBox.Checked;
			}
		}

		/// <summary>
		/// Gets whether the "Match Word" option is ticked.
		/// </summary>
		[Browsable(false)]
		public bool MatchWord {
			get {
				return matchWordCheckBox.Checked;
			}
		}

		/// <summary>
		/// Gets or sets the content of the 'find' text field.
		/// </summary>
		[Description("Gets or sets the content of the 'find' text field.")]
		public string FindTextBoxContent {
			get {
				return textBox1.Text;
			}
			set {
				textBox1.Text = value;
			}
		}

		/// <summary>
		/// Gets or sets the content of the 'replace' text field.
		/// </summary>
		[Description("Gets or sets the content of the 'replace' text field.")]
		public string ReplaceTextBoxContent {
			get {
				return textBox2.Text;
			}
			set {
				textBox2.Text = value;
			}
		}

		/// <summary>
		/// Initializes a new find-and-replace dialog.
		/// </summary>
		public FindAndReplace() {
			InitializeComponent();
			StartPosition = FormStartPosition.CenterScreen;
			TopMost = true;
			findNextDown.Click += FindNextDown_Click;
			findNextUp.Click += FindNextUp_Click;
			replaceNextDown.Click += ReplaceNextDown_Click;
			replaceNextUp.Click += ReplaceNextUp_Click;
			replaceAll.Click += ReplaceAll_Click;
		}

		/// <summary>
		/// Initializes a new find-and-replace dialog.
		/// </summary>
		/// <param name="owner">The owner of the dialog.</param>
		public FindAndReplace(Form owner) : this() {
			if (owner != null)
				Owner = owner;
		}

		/// <summary>
		/// Initializes a new find-and-replace dialog
		/// </summary>
		/// <param name="container">The container to add this dialog to.</param>
		public FindAndReplace(IContainer container) : this() {
			if (container != null)
				container.Add(this);
		}

		private void FindNextDown_Click(object sender, EventArgs e) {
			Action<FindAndReplace> findNextDown = FindNextDownClick;
			if (findNextDown != null)
				findNextDown(this);
			Form owner = Owner;
			if (!(owner == null || owner.IsDisposed))
				owner.Activate();
		}

		private void FindNextUp_Click(object sender, EventArgs e) {
			Action<FindAndReplace> findNextUp = FindNextUpClick;
			if (findNextUp != null)
				findNextUp(this);
			Form owner = Owner;
			if (!(owner == null || owner.IsDisposed))
				owner.Activate();
		}

		private void ReplaceNextDown_Click(object sender, EventArgs e) {
			Action<FindAndReplace> replaceNextDown = ReplaceNextDownClick;
			if (replaceNextDown != null)
				replaceNextDown(this);
			Form owner = Owner;
			if (!(owner == null || owner.IsDisposed))
				owner.Activate();
		}

		private void ReplaceNextUp_Click(object sender, EventArgs e) {
			Action<FindAndReplace> replaceNextUp = ReplaceNextUpClick;
			if (replaceNextUp != null)
				replaceNextUp(this);
			Form owner = Owner;
			if (!(owner == null || owner.IsDisposed))
				owner.Activate();
		}

		private void ReplaceAll_Click(object sender, EventArgs e) {
			Action<FindAndReplace> replaceAll = ReplaceAllClick;
			if (replaceAll != null)
				replaceAll(this);
			Form owner = Owner;
			if (!(owner == null || owner.IsDisposed))
				owner.Activate();
		}

		/// <summary>
		/// Called when the window is shown
		/// </summary>
		/// <param name="e">Ignored.</param>
		protected override void OnShown(EventArgs e) {
			base.OnShown(e);
			UIScaler.AddToScaler(this);
		}

		/// <summary>
		/// Initializes the window and its controls.
		/// </summary>
		private void InitializeComponent() {
			this.label1 = new System.Windows.Forms.StyledLabel();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.StyledLabel();
			this.findNextDown = new System.Windows.Forms.StyledButton();
			this.findNextUp = new System.Windows.Forms.StyledButton();
			this.replaceNextUp = new System.Windows.Forms.StyledButton();
			this.replaceNextDown = new System.Windows.Forms.StyledButton();
			this.replaceAll = new System.Windows.Forms.StyledButton();
			this.matchCaseCheckBox = new System.Windows.Forms.StyledCheckBox();
			this.matchWordCheckBox = new System.Windows.Forms.StyledCheckBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Tile;
			this.label1.ForeColor = System.Drawing.Color.Black;
			this.label1.Location = new System.Drawing.Point(17, 38);
			this.label1.Name = "label1";
			this.label1.OutlinePen = null;
			this.label1.Size = new System.Drawing.Size(45, 17);
			this.label1.TabIndex = 0;
			this.label1.Text = "Find:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label1.TextBrush = null;
			this.label1.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(18, 58);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBox1.Size = new System.Drawing.Size(262, 44);
			this.textBox1.TabIndex = 1;
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(18, 162);
			this.textBox2.Multiline = true;
			this.textBox2.Name = "textBox2";
			this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBox2.Size = new System.Drawing.Size(262, 42);
			this.textBox2.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Tile;
			this.label2.ForeColor = System.Drawing.Color.Black;
			this.label2.Location = new System.Drawing.Point(17, 146);
			this.label2.Name = "label2";
			this.label2.OutlinePen = null;
			this.label2.Size = new System.Drawing.Size(96, 17);
			this.label2.TabIndex = 2;
			this.label2.Text = "Replace With:";
			this.label2.TextBrush = null;
			this.label2.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// findNextDown
			// 
			this.findNextDown.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.findNextDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.findNextDown.Image = null;
			this.findNextDown.Location = new System.Drawing.Point(18, 108);
			this.findNextDown.Name = "findNextDown";
			this.findNextDown.Size = new System.Drawing.Size(90, 30);
			this.findNextDown.TabIndex = 4;
			this.findNextDown.Text = "Find Next ↓";
			this.findNextDown.UseVisualStyleBackColor = true;
			// 
			// findNextUp
			// 
			this.findNextUp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.findNextUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.findNextUp.Image = null;
			this.findNextUp.Location = new System.Drawing.Point(190, 108);
			this.findNextUp.Name = "findNextUp";
			this.findNextUp.Size = new System.Drawing.Size(90, 30);
			this.findNextUp.TabIndex = 5;
			this.findNextUp.Text = "Find Next ↑";
			this.findNextUp.UseVisualStyleBackColor = true;
			// 
			// replaceNextUp
			// 
			this.replaceNextUp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.replaceNextUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.replaceNextUp.Image = null;
			this.replaceNextUp.Location = new System.Drawing.Point(190, 210);
			this.replaceNextUp.Name = "replaceNextUp";
			this.replaceNextUp.Size = new System.Drawing.Size(90, 30);
			this.replaceNextUp.TabIndex = 7;
			this.replaceNextUp.Text = "Replace Next ↑";
			this.replaceNextUp.UseVisualStyleBackColor = true;
			// 
			// replaceNextDown
			// 
			this.replaceNextDown.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.replaceNextDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.replaceNextDown.Image = null;
			this.replaceNextDown.Location = new System.Drawing.Point(18, 210);
			this.replaceNextDown.Name = "replaceNextDown";
			this.replaceNextDown.Size = new System.Drawing.Size(90, 30);
			this.replaceNextDown.TabIndex = 6;
			this.replaceNextDown.Text = "Replace Next ↓";
			this.replaceNextDown.UseVisualStyleBackColor = true;
			// 
			// replaceAll
			// 
			this.replaceAll.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.replaceAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.replaceAll.Image = null;
			this.replaceAll.Location = new System.Drawing.Point(114, 210);
			this.replaceAll.Name = "replaceAll";
			this.replaceAll.Size = new System.Drawing.Size(75, 30);
			this.replaceAll.TabIndex = 8;
			this.replaceAll.Text = "Replace All";
			this.replaceAll.UseVisualStyleBackColor = true;
			// 
			// matchCaseCheckBox
			// 
			this.matchCaseCheckBox.BackColor = System.Drawing.Color.Transparent;
			this.matchCaseCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.matchCaseCheckBox.Location = new System.Drawing.Point(101, 38);
			this.matchCaseCheckBox.Name = "matchCaseCheckBox";
			this.matchCaseCheckBox.Size = new System.Drawing.Size(83, 17);
			this.matchCaseCheckBox.TabIndex = 9;
			this.matchCaseCheckBox.Text = "Match Case";
			this.matchCaseCheckBox.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// matchWordCheckBox
			// 
			this.matchWordCheckBox.BackColor = System.Drawing.Color.Transparent;
			this.matchWordCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.matchWordCheckBox.Location = new System.Drawing.Point(190, 38);
			this.matchWordCheckBox.Name = "matchWordCheckBox";
			this.matchWordCheckBox.Size = new System.Drawing.Size(85, 17);
			this.matchWordCheckBox.TabIndex = 10;
			this.matchWordCheckBox.Text = "Match Word";
			this.matchWordCheckBox.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// FindAndReplace
			// 
			this.AcceptButton = this.findNextDown;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.ClientSize = new System.Drawing.Size(299, 256);
			this.Controls.Add(this.matchWordCheckBox);
			this.Controls.Add(this.matchCaseCheckBox);
			this.Controls.Add(this.replaceAll);
			this.Controls.Add(this.replaceNextUp);
			this.Controls.Add(this.replaceNextDown);
			this.Controls.Add(this.findNextUp);
			this.Controls.Add(this.findNextDown);
			this.Controls.Add(this.textBox2);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.label1);
			this.MaximizeEnabled = false;
			this.MinimizeEnabled = false;
			this.Name = "FindAndReplace";
			this.ShowInTaskbar = false;
			this.Text = "Find and Replace";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}