using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace System.Windows.Forms {
	/// <summary>
	/// A notepad application for C#.
	/// </summary>
	public class Notepad : StyledForm {
		private FilePrompt fileDialog;
		private FontDialog fontDialog;
		private ColorDialog colorDialog;
		private StyledMenuStrip mainMenu;
		private FindAndReplace toolWindow;
		private StyledItem fileToolStripMenuItem, newToolStripMenuItem, openToolStripMenuItem, saveToolStripMenuItem, editToolStripMenuItem,
			cutToolStripMenuItem, copyToolStripMenuItem, pasteToolStripMenuItem, selectAllToolStripMenuItem, formatToolStripMenuItem,
			wordWrapToolStripMenuItem, backgroundToolStripMenuItem, fontToolStripMenuItem, saveAsToolStripMenuItem, findToolStripMenuItem,
			goToLineToolStripMenuItem, goToCharacterToolStripMenuItem, rightAlignedToolStripMenuItem, cutToolStripMenuItem1, copyToolStripMenuItem1,
			pasteToolStripMenuItem1, selectAllToolStripMenuItem1;
		private string titleString = nameof(Notepad), filename = string.Empty, lastSave = string.Empty;
		private StyledContextMenu rightClickContextMenu;
		private IContainer components;
		private NewRichTextBox textBox;

		/// <summary>
		/// The main text box of the Notepad window.
		/// </summary>
		public NewRichTextBox TextBox {
			get {
				return textBox;
			}
		}

		/// <summary>
		/// Initializes a new blank notepad instance.
		/// </summary>
		public Notepad() {
			ConstructObject();
		}

		/// <summary>
		/// Initializes a new slider dialog.
		/// </summary>
		/// <param name="container">The container to add this dialog to.</param>
		public Notepad(IContainer container) : base(container) {
			ConstructObject();
		}

		/// <summary>
		/// Initializes a notepad instance from the specified file/text.
		/// </summary>
		/// <param name="parameter">The text/file to show in the text box.</param>
		public Notepad(string parameter) {
			ConstructObject();
			if (string.IsNullOrEmpty(parameter))
				return;
			if (FileUtils.FileExists(parameter)) {
				fileDialog.FileName = parameter;
				if (!Open()) {
					textBox.Text = parameter;
					titleString = "*Notepad";
					Text = titleString;
				}
			} else {
				textBox.Text = parameter;
				titleString = "*Notepad";
				Text = titleString;
			}
		}

		/// <summary>
		/// Consutructs the window.
		/// </summary>
		private void ConstructObject() {
			InitializeComponent();
			textBox.Font = new Font(new FontFamily("Consolas"), 12f);
			toolWindow = new FindAndReplace(this);
			toolWindow.Icon = Icon;
			toolWindow.FindNextDownClick += ToolWindow_FindNextDownClick;
			toolWindow.FindNextUpClick += ToolWindow_FindNextUpClick;
			toolWindow.ReplaceNextDownClick += ToolWindow_ReplaceNextDownClick;
			toolWindow.ReplaceNextUpClick += ToolWindow_ReplaceNextUpClick;
			toolWindow.ReplaceAllClick += ToolWindow_ReplaceAllClick;
			rightClickContextMenu.Renderer = mainMenu.Renderer;
			StyleRenderer renderer = (StyleRenderer) mainMenu.Renderer;
			renderer.NormalBackgroundTop = ImageLib.ChangeLightness(Color.LightSteelBlue, -10);
			renderer.NormalBackgroundBottom = Color.SteelBlue;
			renderer.HoverBackgroundTop = ImageLib.ChangeLightness(Color.LightSteelBlue, 20);
			renderer.HoverBackgroundBottom = ImageLib.ChangeLightness(Color.SteelBlue, 20);
			renderer.PressedBackgroundTop = ImageLib.ChangeLightness(Color.LightSteelBlue, -20);
			renderer.PressedBackgroundBottom = ImageLib.ChangeLightness(Color.SteelBlue, -20);
			mainMenu.ItemRenderer.CopyConfigFrom((StyleRenderer) mainMenu.Renderer);
			mainMenu.UpdateRenderer();
			mainMenu.MouseUp += mainMenu_MouseUp;
		}

		/// <summary>
		/// Sets whether the prompt is visible.
		/// </summary>
		/// <param name="value">The visibility flag.</param>
		protected override void SetVisibleCore(bool value) {
			base.SetVisibleCore(DesignMode ? false : value);
		}

		private void ToolWindow_FindNextDownClick(FindAndReplace obj) {
			RichTextBoxFinds options = RichTextBoxFinds.NoHighlight;
			if (obj.MatchCase)
				options |= RichTextBoxFinds.MatchCase;
			if (obj.MatchWord)
				options |= RichTextBoxFinds.WholeWord;
			int index = textBox.Find(obj.FindTextBoxContent, Math.Min(textBox.SelectionStart + 1, textBox.TextLength), options);
			if (index == -1) {
				index = textBox.Find(obj.FindTextBoxContent, 0, options);
				if (index == -1)
					return;
			}
			textBox.SelectionStart = index;
			textBox.SelectionLength = obj.FindTextBoxContent.Length;
		}

		private void ToolWindow_FindNextUpClick(FindAndReplace obj) {
			RichTextBoxFinds options = RichTextBoxFinds.Reverse | RichTextBoxFinds.NoHighlight;
			if (obj.MatchCase)
				options |= RichTextBoxFinds.MatchCase;
			if (obj.MatchWord)
				options |= RichTextBoxFinds.WholeWord;
			int index = textBox.Find(obj.FindTextBoxContent, 0, Math.Max(0, textBox.SelectionStart), options);
			if (index == -1) {
				index = textBox.Find(obj.FindTextBoxContent, 0, -1, options);
				if (index == -1)
					return;
			}
			textBox.SelectionStart = index;
			textBox.SelectionLength = obj.FindTextBoxContent.Length;
		}

		private void ToolWindow_ReplaceNextDownClick(FindAndReplace obj) {
			if (textBox.SelectionStart + obj.FindTextBoxContent.Length >= textBox.TextLength)
				return;
			RichTextBoxFinds options = RichTextBoxFinds.NoHighlight;
			if (obj.MatchCase)
				options |= RichTextBoxFinds.MatchCase;
			if (obj.MatchWord)
				options |= RichTextBoxFinds.WholeWord;
			int index = textBox.Find(obj.FindTextBoxContent, Math.Min(textBox.SelectionStart + 1, textBox.TextLength), options);
			if (index == -1) {
				index = textBox.Find(obj.FindTextBoxContent, 0, options);
				if (index == -1)
					return;
			}
			textBox.Text = string.Concat(textBox.Text.Substring(0, index), obj.ReplaceTextBoxContent, textBox.Text.Substring(index + obj.FindTextBoxContent.Length));
			textBox.SelectionStart = index;
			textBox.SelectionLength = obj.ReplaceTextBoxContent.Length;
		}

		private void ToolWindow_ReplaceNextUpClick(FindAndReplace obj) {
			if (textBox.SelectionStart < obj.FindTextBoxContent.Length)
				return;
			RichTextBoxFinds options = RichTextBoxFinds.Reverse | RichTextBoxFinds.NoHighlight;
			if (obj.MatchCase)
				options |= RichTextBoxFinds.MatchCase;
			if (obj.MatchWord)
				options |= RichTextBoxFinds.WholeWord;
			int index = textBox.Find(obj.FindTextBoxContent, 0, Math.Max(0, textBox.SelectionStart), options);
			if (index == -1) {
				index = textBox.Find(obj.FindTextBoxContent, 0, -1, options);
				if (index == -1)
					return;
			}
			textBox.Text = string.Concat(textBox.Text.Substring(0, index), obj.ReplaceTextBoxContent, textBox.Text.Substring(index + obj.FindTextBoxContent.Length));
			textBox.SelectionStart = index;
			textBox.SelectionLength = obj.ReplaceTextBoxContent.Length;
		}

		private void ToolWindow_ReplaceAllClick(FindAndReplace obj) {
			RichTextBoxFinds options = RichTextBoxFinds.NoHighlight;
			if (obj.MatchCase)
				options |= RichTextBoxFinds.MatchCase;
			if (obj.MatchWord)
				options |= RichTextBoxFinds.WholeWord;
			int index = -1, oldIndex = 0;
			while (index + obj.FindTextBoxContent.Length < textBox.TextLength) {
				index = textBox.Find(obj.FindTextBoxContent, index + 1, options);
				if (index == -1)
					break;
				else {
					oldIndex = index;
					textBox.Text = string.Concat(textBox.Text.Substring(0, index), obj.ReplaceTextBoxContent, textBox.Text.Substring(index + obj.FindTextBoxContent.Length));
				}
			}
			if (oldIndex == 0 || oldIndex + obj.ReplaceTextBoxContent.Length >= textBox.TextLength) {
				textBox.SelectionStart = 0;
				textBox.SelectionLength = 0;
			} else {
				textBox.SelectionStart = oldIndex;
				textBox.SelectionLength = obj.ReplaceTextBoxContent.Length;
			}
		}

		private void mainMenu_MouseUp(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Left && mainMenu.ClientRectangle.Contains(e.Location) && mainMenu.GetItemAt(e.X) == -1)
				StyledMessageBox.Show("This application is an enhanced C# Windows Forms Notepad application with syntax highlighting.", "About", true, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void newToolStripMenuItem_Click(object sender, EventArgs e) {
			if (textBox.Text != lastSave && StyledMessageBox.Show("Do you want to save the current text?", "Confirm", true, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				saveToolStripMenuItem_Click(null, null);
			filename = null;
			lastSave = string.Empty;
			textBox.Text = string.Empty;
			titleString = nameof(Notepad);
			Text = nameof(Notepad);
		}

		/// <summary>
		/// Called when a key is about to be processed.
		/// </summary>
		/// <param name="msg">The related message.</param>
		/// <param name="keyData">The keys that were pressed.</param>
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
			if (mainMenu.ProcessKeys(keyData))
				return true;
			else
				return base.ProcessCmdKey(ref msg, keyData);
		}

		private void rightAlignedToolStripMenuItem_Click(object sender, EventArgs e) {
			textBox.RightToLeft = rightAlignedToolStripMenuItem.Checked ? RightToLeft.Yes : RightToLeft.No;
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e) {
			if (textBox.Text != lastSave && StyledMessageBox.Show("Do you want to save the current text?", "Confirm", true, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				saveToolStripMenuItem_Click(null, null);
			fileDialog.Open = true;
			fileDialog.Title = "Open...";
			while (MessageLoop.ShowDialog(fileDialog, false) == DialogResult.OK) {
				if (Open())
					break;
			}
		}

		private bool Open() {
			try {
				string newText;
				TextUtils.DetectTextEncoding(File.ReadAllBytes(fileDialog.FileName), out newText);
				filename = fileDialog.FileName;
				if (filename.EndsWith(".rtf", StringComparison.InvariantCultureIgnoreCase)) {
					textBox.Rtf = newText;
					lastSave = textBox.Text;
				} else {
					textBox.Text = newText;
					lastSave = newText;
				}
				titleString = fileDialog.FileName + " - Notepad";
				Text = titleString;
				return true;
			} catch (Exception ex) {
				StyledMessageBox.Show("An error occurred while trying to open to the specified file:\n" + ex.Message, "Error", true, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
			if (string.IsNullOrEmpty(filename))
				saveAsToolStripMenuItem_Click(null, null);
			else if (textBox.Text != lastSave) {
				lastSave = textBox.Text;
				try {
					using (StreamWriter writer = new StreamWriter(filename, false))
						writer.Write(filename.EndsWith(".rtf", StringComparison.InvariantCultureIgnoreCase) ? textBox.Rtf : lastSave);
					Text = titleString;
				} catch (Exception ex) {
					StyledMessageBox.Show("An error occurred while trying to save:\n" + ex.Message, "Error", true, MessageBoxButtons.OK, MessageBoxIcon.Error);
					saveAsToolStripMenuItem_Click(null, null);
				}
			}
		}

		private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
			do {
				try {
					if (filename.EndsWith(".rtf", StringComparison.InvariantCultureIgnoreCase))
						fileDialog.Filter = "Rich Text Files|*.rtf|Text Files|*.txt";
					else
						fileDialog.Filter = "Text Files|*.txt|Rich Text Files|*.rtf";
					fileDialog.Open = false;
					fileDialog.Title = "Save As...";
					if (MessageLoop.ShowDialog(fileDialog, false) == DialogResult.OK) {
						filename = fileDialog.FileName;
						lastSave = textBox.Text;
						using (StreamWriter writer = new StreamWriter(filename, false))
							writer.Write(filename.EndsWith(".rtf", StringComparison.InvariantCultureIgnoreCase) ? textBox.Rtf : lastSave);
					}
					break;
				} catch (Exception ex) {
					StyledMessageBox.Show("An error occurred while trying to save to the specified file:\n" + ex.Message, "Error", true, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			} while (true);
			Text = titleString;
		}

		private void cutToolStripMenuItem_Click(object sender, EventArgs e) {
			textBox.Cut();
		}

		private void copyToolStripMenuItem_Click(object sender, EventArgs e) {
			textBox.Copy();
		}

		private void pasteToolStripMenuItem_Click(object sender, EventArgs e) {
			textBox.Paste();
		}

		private void selectAllToolStripMenuItem_Click(object sender, EventArgs e) {
			textBox.SelectAll();
		}

		private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e) {
			textBox.WordWrap = wordWrapToolStripMenuItem.Checked;
		}

		private void fontToolStripMenuItem_Click(object sender, EventArgs e) {
			if (fontDialog.ShowDialog() == DialogResult.OK) {
				textBox.Font = fontDialog.Font;
				textBox.ForeColor = fontDialog.Color;
			}
		}

		private void backgroundToolStripMenuItem_Click(object sender, EventArgs e) {
			if (colorDialog.ShowDialog() == DialogResult.OK)
				textBox.BackColor = colorDialog.Color;
		}

		private void findToolStripMenuItem_Click(object sender, EventArgs e) {
			if (textBox.TextLength == 0)
				StyledMessageBox.Show("The textbox is empty.", "No Need", true, MessageBoxButtons.OK, MessageBoxIcon.Information);
			else
				toolWindow.Show(this);
		}

		private void goToLineToolStripMenuItem_Click(object sender, EventArgs e) {
			if (textBox.Text.Contains("\n")) {
				GoToDialog dialog = new GoToDialog();
				if (MessageLoop.ShowDialog(dialog) == DialogResult.OK) {
					textBox.SelectionStart = textBox.GetFirstCharIndexFromLine(Math.Min(((int) dialog.NumericUpDown.Value) - 1, Regex.Split(textBox.Text, "\r\n|\n").Length - 1));
					textBox.ScrollToCaret();
				}
			} else
				StyledMessageBox.Show("There is only 1 line.", "No Need", true, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void goToCharacterToolStripMenuItem_Click(object sender, EventArgs e) {
			if (textBox.TextLength == 0)
				StyledMessageBox.Show("The textbox is empty.", "No Need", true, MessageBoxButtons.OK, MessageBoxIcon.Information);
			else {
				GoToDialog dialog = new GoToDialog();
				dialog.Label.Text = "Index:";
				if (MessageLoop.ShowDialog(dialog) == DialogResult.OK) {
					textBox.SelectionStart = Math.Min(((int) dialog.NumericUpDown.Value) - 1, Regex.Split(textBox.Text, "\r\n|\n").Length - 1);
					textBox.ScrollToCaret();
				}
			}
		}

		/// <summary>
		/// Called when the window is being closed.
		/// </summary>
		protected override bool OnQueryClose(CloseReason reason) {
			if (textBox.Text != lastSave) {
				switch (StyledMessageBox.Show("Do you want to save the current text?", "Confirm", true, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)) {
					case DialogResult.Yes:
						saveToolStripMenuItem_Click(null, null);
						break;
					case DialogResult.Cancel:
						return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Initializes the window and its controls.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Notepad));
			this.fileDialog = new System.Windows.Forms.FilePrompt();
			this.fontDialog = new System.Windows.Forms.FontDialog();
			this.colorDialog = new System.Windows.Forms.ColorDialog();
			this.textBox = new System.Windows.Forms.NewRichTextBox();
			this.rightClickContextMenu = new System.Windows.Forms.StyledContextMenu(this.components);
			this.cutToolStripMenuItem1 = new System.Windows.Forms.StyledItem();
			this.copyToolStripMenuItem1 = new System.Windows.Forms.StyledItem();
			this.pasteToolStripMenuItem1 = new System.Windows.Forms.StyledItem();
			this.selectAllToolStripMenuItem1 = new System.Windows.Forms.StyledItem();
			this.mainMenu = new System.Windows.Forms.StyledMenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.StyledItem();
			this.newToolStripMenuItem = new System.Windows.Forms.StyledItem();
			this.openToolStripMenuItem = new System.Windows.Forms.StyledItem();
			this.saveToolStripMenuItem = new System.Windows.Forms.StyledItem();
			this.saveAsToolStripMenuItem = new System.Windows.Forms.StyledItem();
			this.editToolStripMenuItem = new System.Windows.Forms.StyledItem();
			this.cutToolStripMenuItem = new System.Windows.Forms.StyledItem();
			this.copyToolStripMenuItem = new System.Windows.Forms.StyledItem();
			this.pasteToolStripMenuItem = new System.Windows.Forms.StyledItem();
			this.selectAllToolStripMenuItem = new System.Windows.Forms.StyledItem();
			this.findToolStripMenuItem = new System.Windows.Forms.StyledItem();
			this.goToLineToolStripMenuItem = new System.Windows.Forms.StyledItem();
			this.goToCharacterToolStripMenuItem = new System.Windows.Forms.StyledItem();
			this.formatToolStripMenuItem = new System.Windows.Forms.StyledItem();
			this.wordWrapToolStripMenuItem = new System.Windows.Forms.StyledItem();
			this.fontToolStripMenuItem = new System.Windows.Forms.StyledItem();
			this.backgroundToolStripMenuItem = new System.Windows.Forms.StyledItem();
			this.rightAlignedToolStripMenuItem = new System.Windows.Forms.StyledItem();
			this.rightClickContextMenu.SuspendLayout();
			this.mainMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// fileDialog
			// 
			this.fileDialog.ActiveBorderOpacity = 0.75F;
			this.fileDialog.AllFilesString = "All Files";
			this.fileDialog.AllowDrop = true;
			this.fileDialog.BackColorOpacity = ((byte)(255));
			this.fileDialog.BorderCursor = System.Windows.Forms.Cursors.Default;
			this.fileDialog.BorderWidth = 4;
			this.fileDialog.ButtonText = "Open";
			this.fileDialog.CausesValidation = false;
			this.fileDialog.ClientSize = new System.Drawing.Size(539, 518);
			this.fileDialog.EnableFullscreenOnAltEnter = false;
			this.fileDialog.FileName = "Text.txt";
			this.fileDialog.FileNames = new string[] {
        "Text.txt"};
			this.fileDialog.Filter = "Text Files|*.txt|Rich Text Files|*.rtf";
			this.fileDialog.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.fileDialog.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.fileDialog.InactiveBorderOpacity = 0.5F;
			this.fileDialog.InlineColor = System.Drawing.Color.Black;
			this.fileDialog.KeyPreview = true;
			this.fileDialog.Location = new System.Drawing.Point(0, 0);
			this.fileDialog.MinimizeBox = false;
			this.fileDialog.MinimizeEnabled = false;
			this.fileDialog.MinimumSize = new System.Drawing.Size(200, 50);
			this.fileDialog.Name = "fileDialog";
			this.fileDialog.OutlineColor = System.Drawing.Color.Black;
			this.fileDialog.Padding = new System.Windows.Forms.Padding(4, 29, 4, 4);
			this.fileDialog.SelectDirectory = false;
			this.fileDialog.ShowIcon = false;
			this.fileDialog.ShowInTaskbar = false;
			this.fileDialog.ShowShadow = true;
			this.fileDialog.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.fileDialog.SystemMenu = null;
			this.fileDialog.SystemMenuStrip = null;
			this.fileDialog.Text = "Save As...";
			this.fileDialog.TitleBarBadding = new System.Drawing.Size(0, 1);
			this.fileDialog.TitleBarHeight = 29;
			this.fileDialog.Visible = false;
			this.fileDialog.WarnOverwrite = false;
			this.fileDialog.WindowCursor = System.Windows.Forms.Cursors.Default;
			// 
			// fontDialog
			// 
			this.fontDialog.ShowColor = true;
			// 
			// colorDialog
			// 
			this.colorDialog.AnyColor = true;
			// 
			// textBox
			// 
			this.textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBox.ContextMenuStrip = this.rightClickContextMenu;
			this.textBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox.HideSelection = false;
			this.textBox.Location = new System.Drawing.Point(4, 54);
			this.textBox.MinimumSize = new System.Drawing.Size(20, 20);
			this.textBox.Name = "textBox";
			this.textBox.Size = new System.Drawing.Size(650, 443);
			this.textBox.TabIndex = 1;
			this.textBox.Text = "";
			// 
			// rightClickContextMenu
			// 
			this.rightClickContextMenu.Alignment = System.Drawing.ContentAlignment.MiddleLeft;
			this.rightClickContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cutToolStripMenuItem1,
            this.copyToolStripMenuItem1,
            this.pasteToolStripMenuItem1,
            this.selectAllToolStripMenuItem1});
			this.rightClickContextMenu.Name = "rightClickContextMenu";
			this.rightClickContextMenu.Size = new System.Drawing.Size(67, 100);
			// 
			// cutToolStripMenuItem1
			// 
			this.cutToolStripMenuItem1.Font = new System.Drawing.Font("Segoe UI", 9.25F);
			this.cutToolStripMenuItem1.MaximumSize = new System.Drawing.Size(0, 0);
			this.cutToolStripMenuItem1.Name = "StyledItem";
			this.cutToolStripMenuItem1.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.cutToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
			this.cutToolStripMenuItem1.Size = new System.Drawing.Size(35, 25);
			this.cutToolStripMenuItem1.Text = "Cut";
			this.cutToolStripMenuItem1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.cutToolStripMenuItem1.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.cutToolStripMenuItem1.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
			// 
			// copyToolStripMenuItem1
			// 
			this.copyToolStripMenuItem1.Font = new System.Drawing.Font("Segoe UI", 9.25F);
			this.copyToolStripMenuItem1.MaximumSize = new System.Drawing.Size(0, 0);
			this.copyToolStripMenuItem1.Name = "StyledItem";
			this.copyToolStripMenuItem1.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.copyToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.copyToolStripMenuItem1.Size = new System.Drawing.Size(45, 25);
			this.copyToolStripMenuItem1.Text = "Copy";
			this.copyToolStripMenuItem1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.copyToolStripMenuItem1.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.copyToolStripMenuItem1.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
			// 
			// pasteToolStripMenuItem1
			// 
			this.pasteToolStripMenuItem1.Font = new System.Drawing.Font("Segoe UI", 9.25F);
			this.pasteToolStripMenuItem1.MaximumSize = new System.Drawing.Size(0, 0);
			this.pasteToolStripMenuItem1.Name = "StyledItem";
			this.pasteToolStripMenuItem1.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.pasteToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
			this.pasteToolStripMenuItem1.Size = new System.Drawing.Size(46, 25);
			this.pasteToolStripMenuItem1.Text = "Paste";
			this.pasteToolStripMenuItem1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.pasteToolStripMenuItem1.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.pasteToolStripMenuItem1.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
			// 
			// selectAllToolStripMenuItem1
			// 
			this.selectAllToolStripMenuItem1.Font = new System.Drawing.Font("Segoe UI", 9.25F);
			this.selectAllToolStripMenuItem1.MaximumSize = new System.Drawing.Size(0, 0);
			this.selectAllToolStripMenuItem1.Name = "StyledItem";
			this.selectAllToolStripMenuItem1.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.selectAllToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
			this.selectAllToolStripMenuItem1.Size = new System.Drawing.Size(67, 25);
			this.selectAllToolStripMenuItem1.Text = "Select All";
			this.selectAllToolStripMenuItem1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.selectAllToolStripMenuItem1.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.selectAllToolStripMenuItem1.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
			// 
			// mainMenu
			// 
			this.mainMenu.AlignGradientWorkaround = true;
			this.mainMenu.AutoSize = false;
			this.mainMenu.BackColor = System.Drawing.Color.SteelBlue;
			this.mainMenu.ForeColor = System.Drawing.Color.Black;
			this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.formatToolStripMenuItem});
			this.mainMenu.Location = new System.Drawing.Point(4, 29);
			this.mainMenu.Name = "mainMenu";
			this.mainMenu.Size = new System.Drawing.Size(650, 25);
			this.mainMenu.TabIndex = 0;
			this.mainMenu.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripMenuItem[] {
				this.newToolStripMenuItem,
				this.openToolStripMenuItem,
				this.saveToolStripMenuItem,
				this.saveAsToolStripMenuItem,
			});
			this.fileToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9.25F);
			this.fileToolStripMenuItem.ForeColor = System.Drawing.Color.White;
			this.fileToolStripMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.fileToolStripMenuItem.Name = "StyledItem";
			this.fileToolStripMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 25);
			this.fileToolStripMenuItem.Text = "File";
			this.fileToolStripMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// newToolStripMenuItem
			// 
			this.newToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9.25F);
			this.newToolStripMenuItem.ForeColor = System.Drawing.Color.White;
			this.newToolStripMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.newToolStripMenuItem.Name = "StyledItem";
			this.newToolStripMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.newToolStripMenuItem.ShortcutKeyDisplayString = "";
			this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.newToolStripMenuItem.Size = new System.Drawing.Size(41, 25);
			this.newToolStripMenuItem.Text = "New";
			this.newToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.newToolStripMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9.25F);
			this.openToolStripMenuItem.ForeColor = System.Drawing.Color.White;
			this.openToolStripMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.openToolStripMenuItem.Name = "StyledItem";
			this.openToolStripMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.openToolStripMenuItem.ShortcutKeyDisplayString = "";
			this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.openToolStripMenuItem.Size = new System.Drawing.Size(55, 25);
			this.openToolStripMenuItem.Text = "Open...";
			this.openToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.openToolStripMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9.25F);
			this.saveToolStripMenuItem.ForeColor = System.Drawing.Color.White;
			this.saveToolStripMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.saveToolStripMenuItem.Name = "StyledItem";
			this.saveToolStripMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.saveToolStripMenuItem.ShortcutKeyDisplayString = "";
			this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(42, 25);
			this.saveToolStripMenuItem.Text = "Save";
			this.saveToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.saveToolStripMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
			// 
			// saveAsToolStripMenuItem
			// 
			this.saveAsToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9.25F);
			this.saveAsToolStripMenuItem.ForeColor = System.Drawing.Color.White;
			this.saveAsToolStripMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.saveAsToolStripMenuItem.Name = "StyledItem";
			this.saveAsToolStripMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
			this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(67, 25);
			this.saveAsToolStripMenuItem.Text = "Save As...";
			this.saveAsToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.saveAsToolStripMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripMenuItem[] {
				this.cutToolStripMenuItem,
				this.copyToolStripMenuItem,
				this.pasteToolStripMenuItem,
				this.selectAllToolStripMenuItem,
				this.findToolStripMenuItem,
				this.goToLineToolStripMenuItem,
				this.goToCharacterToolStripMenuItem,
			});
			this.editToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9.25F);
			this.editToolStripMenuItem.ForeColor = System.Drawing.Color.White;
			this.editToolStripMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.editToolStripMenuItem.Name = "StyledItem";
			this.editToolStripMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.editToolStripMenuItem.Size = new System.Drawing.Size(37, 25);
			this.editToolStripMenuItem.Text = "Edit";
			this.editToolStripMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// cutToolStripMenuItem
			// 
			this.cutToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9.25F);
			this.cutToolStripMenuItem.ForeColor = System.Drawing.Color.White;
			this.cutToolStripMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.cutToolStripMenuItem.Name = "StyledItem";
			this.cutToolStripMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.cutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
			this.cutToolStripMenuItem.Size = new System.Drawing.Size(35, 25);
			this.cutToolStripMenuItem.Text = "Cut";
			this.cutToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.cutToolStripMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
			// 
			// copyToolStripMenuItem
			// 
			this.copyToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9.25F);
			this.copyToolStripMenuItem.ForeColor = System.Drawing.Color.White;
			this.copyToolStripMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.copyToolStripMenuItem.Name = "StyledItem";
			this.copyToolStripMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.copyToolStripMenuItem.Size = new System.Drawing.Size(45, 25);
			this.copyToolStripMenuItem.Text = "Copy";
			this.copyToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.copyToolStripMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
			// 
			// pasteToolStripMenuItem
			// 
			this.pasteToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9.25F);
			this.pasteToolStripMenuItem.ForeColor = System.Drawing.Color.White;
			this.pasteToolStripMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.pasteToolStripMenuItem.Name = "StyledItem";
			this.pasteToolStripMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
			this.pasteToolStripMenuItem.Size = new System.Drawing.Size(46, 25);
			this.pasteToolStripMenuItem.Text = "Paste";
			this.pasteToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.pasteToolStripMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
			// 
			// selectAllToolStripMenuItem
			// 
			this.selectAllToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9.25F);
			this.selectAllToolStripMenuItem.ForeColor = System.Drawing.Color.White;
			this.selectAllToolStripMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.selectAllToolStripMenuItem.Name = "StyledItem";
			this.selectAllToolStripMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.selectAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
			this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(67, 25);
			this.selectAllToolStripMenuItem.Text = "Select All";
			this.selectAllToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.selectAllToolStripMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
			// 
			// findToolStripMenuItem
			// 
			this.findToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9.25F);
			this.findToolStripMenuItem.ForeColor = System.Drawing.Color.White;
			this.findToolStripMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.findToolStripMenuItem.Name = "StyledItem";
			this.findToolStripMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.findToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
			this.findToolStripMenuItem.Size = new System.Drawing.Size(109, 25);
			this.findToolStripMenuItem.Text = "Find & Replace...";
			this.findToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.findToolStripMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.findToolStripMenuItem.Click += new System.EventHandler(this.findToolStripMenuItem_Click);
			// 
			// goToLineToolStripMenuItem
			// 
			this.goToLineToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9.25F);
			this.goToLineToolStripMenuItem.ForeColor = System.Drawing.Color.White;
			this.goToLineToolStripMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.goToLineToolStripMenuItem.Name = "StyledItem";
			this.goToLineToolStripMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.goToLineToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
			this.goToLineToolStripMenuItem.Size = new System.Drawing.Size(84, 25);
			this.goToLineToolStripMenuItem.Text = "Go To Line...";
			this.goToLineToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.goToLineToolStripMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.goToLineToolStripMenuItem.Click += new System.EventHandler(this.goToLineToolStripMenuItem_Click);
			// 
			// goToCharacterToolStripMenuItem
			// 
			this.goToCharacterToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9.25F);
			this.goToCharacterToolStripMenuItem.ForeColor = System.Drawing.Color.White;
			this.goToCharacterToolStripMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.goToCharacterToolStripMenuItem.Name = "StyledItem";
			this.goToCharacterToolStripMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.goToCharacterToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
			this.goToCharacterToolStripMenuItem.Size = new System.Drawing.Size(115, 25);
			this.goToCharacterToolStripMenuItem.Text = "Go To Character...";
			this.goToCharacterToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.goToCharacterToolStripMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.goToCharacterToolStripMenuItem.Click += new System.EventHandler(this.goToCharacterToolStripMenuItem_Click);
			// 
			// formatToolStripMenuItem
			// 
			this.formatToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripMenuItem[] {
				this.wordWrapToolStripMenuItem,
				this.fontToolStripMenuItem,
				this.backgroundToolStripMenuItem,
				this.rightAlignedToolStripMenuItem,
			});
			this.formatToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9.25F);
			this.formatToolStripMenuItem.ForeColor = System.Drawing.Color.White;
			this.formatToolStripMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.formatToolStripMenuItem.Name = "StyledItem";
			this.formatToolStripMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.formatToolStripMenuItem.Size = new System.Drawing.Size(55, 25);
			this.formatToolStripMenuItem.Text = "Format";
			this.formatToolStripMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// wordWrapToolStripMenuItem
			// 
			this.wordWrapToolStripMenuItem.Checked = true;
			this.wordWrapToolStripMenuItem.CheckOnClick = true;
			this.wordWrapToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.wordWrapToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9.25F);
			this.wordWrapToolStripMenuItem.ForeColor = System.Drawing.Color.White;
			this.wordWrapToolStripMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.wordWrapToolStripMenuItem.Name = "StyledItem";
			this.wordWrapToolStripMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.wordWrapToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
			this.wordWrapToolStripMenuItem.ShowCheckBox = true;
			this.wordWrapToolStripMenuItem.Size = new System.Drawing.Size(102, 25);
			this.wordWrapToolStripMenuItem.Text = "Word Wrap";
			this.wordWrapToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.wordWrapToolStripMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.wordWrapToolStripMenuItem.Click += new System.EventHandler(this.wordWrapToolStripMenuItem_Click);
			// 
			// fontToolStripMenuItem
			// 
			this.fontToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9.25F);
			this.fontToolStripMenuItem.ForeColor = System.Drawing.Color.White;
			this.fontToolStripMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.fontToolStripMenuItem.Name = "StyledItem";
			this.fontToolStripMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.fontToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
			this.fontToolStripMenuItem.Size = new System.Drawing.Size(71, 25);
			this.fontToolStripMenuItem.Text = "Font...";
			this.fontToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.fontToolStripMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.fontToolStripMenuItem.Click += new System.EventHandler(this.fontToolStripMenuItem_Click);
			// 
			// backgroundToolStripMenuItem
			// 
			this.backgroundToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9.25F);
			this.backgroundToolStripMenuItem.ForeColor = System.Drawing.Color.White;
			this.backgroundToolStripMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.backgroundToolStripMenuItem.Name = "StyledItem";
			this.backgroundToolStripMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.backgroundToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
			this.backgroundToolStripMenuItem.Size = new System.Drawing.Size(113, 25);
			this.backgroundToolStripMenuItem.Text = "Background...";
			this.backgroundToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.backgroundToolStripMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.backgroundToolStripMenuItem.Click += new System.EventHandler(this.backgroundToolStripMenuItem_Click);
			// 
			// rightAlignedToolStripMenuItem
			// 
			this.rightAlignedToolStripMenuItem.CheckOnClick = true;
			this.rightAlignedToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9.25F);
			this.rightAlignedToolStripMenuItem.ForeColor = System.Drawing.Color.White;
			this.rightAlignedToolStripMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.rightAlignedToolStripMenuItem.Name = "StyledItem";
			this.rightAlignedToolStripMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.rightAlignedToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
			this.rightAlignedToolStripMenuItem.ShowCheckBox = true;
			this.rightAlignedToolStripMenuItem.Size = new System.Drawing.Size(115, 25);
			this.rightAlignedToolStripMenuItem.Text = "Right-Aligned";
			this.rightAlignedToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.rightAlignedToolStripMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.rightAlignedToolStripMenuItem.Click += new System.EventHandler(this.rightAlignedToolStripMenuItem_Click);
			// 
			// Notepad
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
			this.BackColor = System.Drawing.Color.Black;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.ClientSize = new System.Drawing.Size(658, 501);
			this.Controls.Add(this.textBox);
			this.Controls.Add(this.mainMenu);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.mainMenu;
			this.Name = "Notepad";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Notepad";
			this.rightClickContextMenu.ResumeLayout(false);
			this.mainMenu.ResumeLayout(false);
			this.mainMenu.PerformLayout();
			this.ResumeLayout(false);

		}

		/// <summary>
		/// Called when the window is being disposed.
		/// </summary>
		/// <param name="e">Whether managed resources are about to be disposed.</param>
		protected override void OnDisposing(DisposeFormEventArgs e) {
			if (e.DisposeMode == DisposeOptions.FullDisposal) {
				fileDialog.Dispose();
				fontDialog.Dispose();
				colorDialog.Dispose();
				if (toolWindow != null) {
					toolWindow.Dispose();
					toolWindow = null;
				}
			}
		}
	}
}