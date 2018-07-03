using System.Windows.Forms;

namespace Designer {
	partial class MainWindow {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">Specifies how the form is to be disposed.</param>
		protected override void OnDisposing(DisposeFormEventArgs e) {
			if (e.DisposeMode == DisposeOptions.FullDisposal && (components != null)) {
				components.Dispose();
				components = null;
			}
			base.OnDisposing(e);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.viewCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.reloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.referencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.errorLog = new System.Windows.Forms.ListBox();
			this.splitContainer1 = new System.Windows.Forms.BufferedSplitContainer();
			this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
			this.menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.viewCodeToolStripMenuItem,
            this.reloadToolStripMenuItem,
            this.referencesToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(1150, 24);
			this.menuStrip1.TabIndex = 3;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
			this.openToolStripMenuItem.Text = "Open";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
			this.saveToolStripMenuItem.Text = "Save";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
			// 
			// viewCodeToolStripMenuItem
			// 
			this.viewCodeToolStripMenuItem.Name = "viewCodeToolStripMenuItem";
			this.viewCodeToolStripMenuItem.Size = new System.Drawing.Size(75, 20);
			this.viewCodeToolStripMenuItem.Text = "View Code";
			this.viewCodeToolStripMenuItem.Click += new System.EventHandler(this.viewCodeToolStripMenuItem_Click);
			// 
			// reloadToolStripMenuItem
			// 
			this.reloadToolStripMenuItem.Name = "reloadToolStripMenuItem";
			this.reloadToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
			this.reloadToolStripMenuItem.Text = "Reload";
			this.reloadToolStripMenuItem.Click += new System.EventHandler(this.reloadToolStripMenuItem_Click);
			// 
			// referencesToolStripMenuItem
			// 
			this.referencesToolStripMenuItem.Name = "referencesToolStripMenuItem";
			this.referencesToolStripMenuItem.Size = new System.Drawing.Size(108, 20);
			this.referencesToolStripMenuItem.Text = "Show References";
			this.referencesToolStripMenuItem.Click += new System.EventHandler(this.referencesToolStripMenuItem_Click);
			// 
			// errorLog
			// 
			this.errorLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.errorLog.FormattingEnabled = true;
			this.errorLog.Location = new System.Drawing.Point(0, 0);
			this.errorLog.Name = "errorLog";
			this.errorLog.Size = new System.Drawing.Size(1150, 192);
			this.errorLog.TabIndex = 4;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.propertyGrid1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.errorLog);
			this.splitContainer1.Size = new System.Drawing.Size(1150, 694);
			this.splitContainer1.SplitterDistance = 498;
			this.splitContainer1.TabIndex = 5;
			// 
			// propertyGrid1
			// 
			this.propertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.propertyGrid1.LineColor = System.Drawing.SystemColors.ControlDark;
			this.propertyGrid1.Location = new System.Drawing.Point(798, 27);
			this.propertyGrid1.Name = "propertyGrid1";
			this.propertyGrid1.Size = new System.Drawing.Size(349, 468);
			this.propertyGrid1.TabIndex = 0;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(1150, 694);
			this.Controls.Add(this.menuStrip1);
			this.Controls.Add(this.splitContainer1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "Form1";
			this.Text = "Form1";
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem viewCodeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem reloadToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem referencesToolStripMenuItem;
		private System.Windows.Forms.ListBox errorLog;
		private System.Windows.Forms.BufferedSplitContainer splitContainer1;
		private System.Windows.Forms.PropertyGrid propertyGrid1;
	}
}