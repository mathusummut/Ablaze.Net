namespace Designer {
	partial class ReferencesWindow {
		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.addReferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.removeReferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// listBox1
			// 
			this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listBox1.FormattingEnabled = true;
			this.listBox1.Location = new System.Drawing.Point(4, 54);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(833, 372);
			this.listBox1.TabIndex = 0;
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addReferencesToolStripMenuItem,
            this.removeReferencesToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(4, 30);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(833, 24);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// addReferencesToolStripMenuItem
			// 
			this.addReferencesToolStripMenuItem.Name = "addReferencesToolStripMenuItem";
			this.addReferencesToolStripMenuItem.Size = new System.Drawing.Size(101, 20);
			this.addReferencesToolStripMenuItem.Text = "Add References";
			this.addReferencesToolStripMenuItem.Click += new System.EventHandler(this.addReferencesToolStripMenuItem_Click);
			// 
			// removeReferencesToolStripMenuItem
			// 
			this.removeReferencesToolStripMenuItem.Name = "removeReferencesToolStripMenuItem";
			this.removeReferencesToolStripMenuItem.Size = new System.Drawing.Size(122, 20);
			this.removeReferencesToolStripMenuItem.Text = "Remove References";
			this.removeReferencesToolStripMenuItem.Click += new System.EventHandler(this.removeReferencesToolStripMenuItem_Click);
			// 
			// ReferencesWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.ClientSize = new System.Drawing.Size(841, 430);
			this.Controls.Add(this.listBox1);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "ReferencesWindow";
			this.Text = "Manage References";
			this.Controls.SetChildIndex(this.menuStrip1, 0);
			this.Controls.SetChildIndex(this.listBox1, 0);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem addReferencesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem removeReferencesToolStripMenuItem;
	}
}