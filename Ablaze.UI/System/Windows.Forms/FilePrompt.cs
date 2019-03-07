using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Platforms.Windows;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Dispatch;

namespace System.Windows.Forms {
	/// <summary>
	/// Asks the user to select a file/folder.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxItemFilter("System.Windows.Forms")]
	[DesignTimeVisible(true)]
	[Description("Asks the user to select a file/folder.")]
	[DisplayName(nameof(FilePrompt))]
	[DefaultEvent(nameof(Load))]
	public class FilePrompt : StyledForm {
		/// <summary>
		/// The last valid path that was browsed.
		/// </summary>
		public static string LastPath;
		private static char[] InvalidFileChars = Path.GetInvalidFileNameChars();
		private static char[] Trim = new char[] { ' ', '*' };
		private static char[] SplitSemicolon = new char[] { ';' };
		private static char[] SplitPipe = new char[] { '|' };
		private static char[] TrimDot = new char[] { ' ', '.' };
		private static char[] SplitDir = new char[] { Path.DirectorySeparatorChar };
		private static Action<TreeNodeCollection, List<TreeNode>> addNode = AddNode;
		private DispatcherSlim rebuildDispatch;
		private Func<object, object> rebuild;
		private IContainer components;
		private StyledButton SelectButton;
		private TextBox FileTextBox;
		private ImageList imageList;
		private BufferedTreeView treeView;
		private StyledComboBox comboBox;
		private SyncedList<string> CurrentExtensions = new SyncedList<string>();
		private object treeSyncRoot = new object();
		private bool selectDirectory, open, warnOverwrite, appendExtension = true;
		private string filters = "", allFilesString = "All Files";
		private StyledLabel label1, label2;

		/// <summary>
		/// Gets or sets whether to append file extension when saving file if it is missing.
		/// </summary>
		[Description("Gets or sets whether to append file extension when saving file if it is missing.")]
		[DefaultValue(true)]
		public bool AppendExtension {
			get {
				return appendExtension;
			}
			set {
				appendExtension = value;
			}
		}

		/// <summary>
		/// Gets or sets whether to allow selection of multiple files.
		/// </summary>
		[Description("Gets or sets whether to allow selection of multiple files.")]
		[DefaultValue(false)]
		public bool Multiselect {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the All Files string (default: "All Files (*.*)").
		/// </summary>
		[Description("Gets or sets the All Files string (default: \"All Files(*.*)\").")]
		[DefaultValue("All Files (*.*)")]
		public string AllFilesString {
			get {
				return allFilesString;
			}
			set {
				if (value == null || value.Length == 0)
					value = "All Files (*.*)";
				allFilesString = value;
				comboBox.Items[comboBox.Items.Count - 1] = value;
			}
		}

		/// <summary>
		/// Gets or sets the text that appears on the selection button.
		/// </summary>
		[Description("Gets or sets the text that appears on the selection button.")]
		public string ButtonText {
			get {
				return SelectButton.Text;
			}
			set {
				if (value == null || value.Length == 0)
					value = open ? "Open" : "Save";
				SelectButton.Text = value;
			}
		}

		/// <summary>
		/// Set to true to select an existing file, false for saving or for suppressing file existence checks.
		/// </summary>
		[Description("Set to true to select an existing file, false for saving or for suppressing file existence checks.")]
		[DefaultValue(true)]
		public bool Open {
			get {
				return open;
			}
			set {
				open = value;
			}
		}

		/// <summary>
		/// Gets or sets whether to show a warning before attempting to overwrite file.
		/// </summary>
		[Description("Gets or sets whether to show a warning before attempting to overwrite file.")]
		public bool WarnOverwrite {
			get {
				return warnOverwrite;
			}
			set {
				warnOverwrite = value;
			}
		}

		/// <summary>
		/// Gets or sets whether to select a directory or a file.
		/// </summary>
		[Description("Gets or sets whether to select a directory or a file.")]
		public bool SelectDirectory {
			get {
				return selectDirectory;
			}
			set {
				if (value == selectDirectory)
					return;
				selectDirectory = value;
				if (rebuildDispatch != null)
					rebuildDispatch.BeginInvoke(new InvocationData(rebuild));
			}
		}

		/// <summary>
		/// Gets or sets the FilePrompt text. You can use Text instead.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public string Title {
			get {
				return Text;
			}
			set {
				Text = value;
			}
		}

		/// <summary>
		/// Gets or sets the selected file name.
		/// </summary>
		[Description("Gets or sets the selected file name.")]
		public string FileName {
			get {
				return FileTextBox.Text;
			}
			set {
				FileTextBox.Text = value;
				if (FileTextBox.TextLength != 0) {
					FileTextBox.SelectionStart = FileTextBox.TextLength;
					FileTextBox.SelectionLength = 0;
				}
			}
		}

		/// <summary>
		/// Gets or sets the selected file names.
		/// </summary>
		[Description("Gets or sets the selected file names.")]
		public string[] FileNames {
			get {
				return new string[] { FileTextBox.Text };
			}
			set {
				FileTextBox.Text = value == null || value.Length == 0 ? string.Empty : value[0];
				if (FileTextBox.TextLength != 0) {
					FileTextBox.SelectionStart = FileTextBox.TextLength;
					FileTextBox.SelectionLength = 0;
				}
			}
		}

		/// <summary>
		/// Only shows the specified file extensions. For example: "Jpeg Images|jpg|Text Files|txt;doc".
		/// "All Files" is added automatically.
		/// </summary>
		[Description("Only shows the specified file extensions. For example: \"Jpeg Images|jpg|Text Files|txt;doc\".")]
		public string Filter {
			get {
				return filters;
			}
			set {
				if (value == filters)
					return;
				filters = value;
				comboBox.Items.Clear();
				if (!(value == null || value.Length == 0)) {
					string[] items = value.Split(SplitPipe);
					string[] extensions;
					string description, extension;
					StringBuilder builder = new StringBuilder();
					int j;
					for (int i = 0; i < items.Length; i++) {
						builder.Length = 0;
						description = items[i].Trim();
						i++;
						extensions = items[i].Trim(Trim).Split(SplitSemicolon);
						for (j = 0; j < extensions.Length; j++) {
							extension = extensions[j].Trim(Trim);
							if (extension.Length != 0) {
								if (extension[0] != '.')
									extension = '.' + extension;
								builder.Append(extension);
								builder.Append(';');
							}
						}
						if (builder.Length != 0)
							builder.Remove(builder.Length - 1, 1);
						builder.Append(')');
						comboBox.Items.Add(description + " (" + builder);
					}
				}
				comboBox.Items.Add(allFilesString);
				comboBox.SelectedIndex = 0;
				ReloadExtensions();
			}
		}

		/// <summary>
		/// Initializes a new open file prompt using default values.
		/// </summary>
		public FilePrompt() : this(true) {
		}

		/// <summary>
		/// Initializes a new file prompt using the specified file extension filter.
		/// </summary>
		/// <param name="open">True to select an existing file, false for saving or for suppressing file existence checks.</param>
		/// <param name="filter">The filter to use (can be null).</param>
		/// <param name="selectDirectory">Whether to select a directory instead of a file.</param>
		public FilePrompt(bool open, string filter = "", bool selectDirectory = false) {
			rebuild = RebuildTree;
			InitializeComponent();
			IntPtr handle = treeView.Handle;
			MinimizeEnabled = false;
			ShowInTaskbar = false;
			comboBox.AddItem(allFilesString);
			comboBox.SelectedIndex = 0;
			this.open = open;
			if (!open) {
				Text = "Choose where to save...";
				ButtonText = "Save";
			}
			if (selectDirectory)
				Text = "Choose a folder...";
			this.selectDirectory = selectDirectory;
			this.filters = filter;
			DialogResult = DialogResult.Cancel;
			DragEnter += FilePrompt_DragEnter;
			DragDrop += FilePrompt_DragDrop;
		}

		private static void FilePrompt_DragEnter(object sender, DragEventArgs e) {
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
				e.Effect = DragDropEffects.Link;
		}

		private void FilePrompt_DragDrop(object sender, DragEventArgs e) {
			FileNames = e.Data.GetData(DataFormats.FileDrop) as string[];
		}

		/// <summary>
		/// Sets whether the prompt is visible.
		/// </summary>
		/// <param name="value">The visibility flag.</param>
		protected override void SetVisibleCore(bool value) {
			base.SetVisibleCore(DesignMode ? false : value);
		}

		/// <summary>
		/// Called when the file prompt is shown.
		/// </summary>
		/// <param name="e">Ignored.</param>
		protected override void OnShown(EventArgs e) {
			base.OnShown(e);
			IntPtr handle = treeView.Handle;
			if (!StyledForm.DesignMode)
				rebuildDispatch = new DispatcherSlim(nameof(RebuildTree), true, ExceptionMode.Throw, ThreadPriority.Normal, 2);
			ReloadExtensions();
			UIScaler.ExcludeFontScaling(treeView);
			UIScaler.AddToScaler(this);
		}

		private void ReloadExtensions() {
			CurrentExtensions.Clear();
			if (comboBox.SelectedIndex == comboBox.Items.Count - 1)
				CurrentExtensions.Add("*");
			else {
				string selected = (string) comboBox.SelectedItem;
				for (int i = selected.Length - 2; i >= 0; i--) {
					if (selected[i] == '(') {
						i++;
						foreach (string str in selected.Substring(i, (selected.Length - 1) - i).Split(SplitSemicolon))
							CurrentExtensions.Add(str);
						break;
					}
				}
			}
			if (rebuildDispatch != null)
				rebuildDispatch.BeginInvoke(new InvocationData(rebuild));
		}

		private int GetIcon(string path) {
			try {
				SHFILEINFO shinfo = new SHFILEINFO();
				Shell.GetFileInfo(path, 0, ref shinfo, SHFILEINFO.Size, ShGetFileIconFlags.Icon | ShGetFileIconFlags.LargeIcon);
				int index = imageList.Images.Count;
				lock (treeSyncRoot) {
					if (shinfo.hIcon == IntPtr.Zero || treeView.IsDisposed || treeView.Disposing || !treeView.IsHandleCreated)
						return -1;
					Icon icon = Icon.FromHandle(shinfo.hIcon);
					if (icon == null)
						return -1;
					imageList.Images.Add(icon);
					return index;
				}
			} catch {
				return -1;
			}
		}

		private object RebuildTree(object param) {
			if (rebuildDispatch == null)
				return null;
			treeView.Nodes.Clear();
			imageList.Images.Clear();
			TreeNode currentNode;
			string str;
			if (IsClosing || IsDisposed || rebuildDispatch.QueueCount != 0)
				return null;
			List<TreeNode> queuedNodes = new List<TreeNode>(10);
			int iconIndex;
			for (int i = 0; i < 3; i++) {
				str = Shell.GetPath((Shell.KnownFolder) i);
				if (str != null) {
					currentNode = new TreeNode(((Shell.KnownFolder) i).ToString());
					currentNode.Tag = str;
					iconIndex = GetIcon(str);
					if (iconIndex == -1)
						return null;
					currentNode.ImageIndex = iconIndex;
					currentNode.SelectedImageIndex = currentNode.ImageIndex;
					currentNode.Nodes.Add(" "); //Placeholder to enable expanding (+)
					queuedNodes.Add(currentNode);
				}
			}
			foreach (DriveInfo drive in DriveInfo.GetDrives()) {
				currentNode = new TreeNode(drive.Name);
				currentNode.Tag = drive.Name;
				iconIndex = GetIcon(drive.Name);
				if (iconIndex == -1)
					return null;
				currentNode.ImageIndex = iconIndex;
				currentNode.SelectedImageIndex = currentNode.ImageIndex;
				currentNode.Nodes.Add(" "); //Placeholder to enable expanding (+)
				queuedNodes.Add(currentNode);
			}
			if (IsClosing || IsDisposed || rebuildDispatch.QueueCount != 0)
				return null;
			treeView.Invoke(addNode, treeView.Nodes, queuedNodes);
			SelectPath(LastPath);
			return null;
		}

		private static void AddNode(TreeNodeCollection parent, List<TreeNode> nodes) {
			parent.AddRange(nodes);
		}

		private void treeView_BeforeExpand(object sender, TreeViewCancelEventArgs e) {
			if (e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Text == " ")
				PopulateChildren(e.Node, (string) e.Node.Tag);
		}

		private void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e) {
			string candidate = (string) e.Node.Tag;
			LastPath = candidate;
			if (open && IsPathToFile(candidate) != selectDirectory)
				FileName = candidate;
		}

		private void treeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e) {
			SelectButton_Click(sender, e);
		}

		private static bool IsPathToFile(string Path) {
			return (File.GetAttributes(Path) & FileAttributes.Directory) != FileAttributes.Directory;
		}

		private void PopulateChildren(TreeNode node, string path) {
			if (rebuildDispatch == null || IsPathToFile(path))
				return;
			node.Nodes.Clear();
			string[] dirs;
			try {
				dirs = Directory.GetDirectories(path);
			} catch {
				return;
			}
			if (IsClosing || IsDisposed || rebuildDispatch.QueueCount != 0)
				return;
			List<TreeNode> queuedNodes = new List<TreeNode>(10);
			TreeNode currentNode = null;
			int iconIndex;
			foreach (string dir in dirs) {
				try {
					currentNode = new TreeNode(new DirectoryInfo(dir).Name);
					currentNode.Tag = dir;
					iconIndex = GetIcon(dir);
					if (iconIndex == -1)
						return;
					currentNode.ImageIndex = iconIndex;
					currentNode.SelectedImageIndex = currentNode.ImageIndex;
					currentNode.Nodes.Add(" ");
					queuedNodes.Add(currentNode);
				} catch {
				}
			}
			if (IsClosing || IsDisposed || rebuildDispatch.QueueCount != 0)
				return;
			treeView.Invoke(addNode, node.Nodes, queuedNodes);
			queuedNodes.Clear();
			bool visible = false;
			if (currentNode != null) {
				visible = true;
				currentNode.EnsureVisible();
			}
			if (selectDirectory)
				return;
			string ext;
			foreach (string file in Directory.GetFiles(path)) {
				if (IsAllowed(file, CurrentExtensions, out ext)) {
					try {
						currentNode = new TreeNode(Path.GetFileName(file));
						if (!visible) {
							visible = true;
							currentNode.EnsureVisible();
						}
						currentNode.Tag = file;
						iconIndex = GetIcon(file);
						if (iconIndex == -1)
							return;
						currentNode.ImageIndex = iconIndex;
						currentNode.SelectedImageIndex = currentNode.ImageIndex;
						queuedNodes.Add(currentNode);
					} catch {
					}
				}
			}
			if (!(IsClosing || IsDisposed || rebuildDispatch.QueueCount != 0))
				treeView.Invoke(addNode, node.Nodes, queuedNodes);
		}

		private static bool IsValid(string path, bool selectDirectory, SyncedList<string> extensions) {
			if (string.IsNullOrEmpty(path))
				return false;
			else if (!selectDirectory && IsPathToFile(path)) {
				string ext;
				return FileUtils.FileExists(path) && IsAllowed(path, extensions, out ext);
			} else if (selectDirectory)
				return FileUtils.FolderExists(path);
			else
				return false;
		}

		private static bool IsAllowed(string file, SyncedList<string> extensions, out string extension) {
			foreach (string ex in extensions) {
				if (ex == "*" || file.EndsWith(ex, StringComparison.OrdinalIgnoreCase)) {
					extension = ex;
					return true;
				}
			}
			extension = string.Empty;
			return false;
		}

		/// <summary>
		/// Selects the specified path in the tree-view.
		/// </summary>
		/// <param name="path">The path to try to select.</param>
		public void SelectPath(string path) {
			if (path == null || path.Length == 0)
				path = Environment.CurrentDirectory;
			path = FileUtils.ResolvePath(path);
			if (path == null)
				return;
			bool directory = selectDirectory;
			TreeNode node;
			do {
				if (IsValid(path, directory, CurrentExtensions)) {
					string[] dirs = path.Split(SplitDir);
					node = null;
					foreach (string str in dirs) {
						node = GetSubNode(str, node);
						if (node == null)
							break;
						else {
							PopulateChildren(node, (string) node.Tag);
							lock (treeSyncRoot) {
								if (treeView.IsDisposed || treeView.Disposing || !treeView.IsHandleCreated)
									return;
								node.Expand();
							}
						}
					}
					if (node != null)
						treeView.SelectedNode = node;
					return;
				} else
					path = Path.GetDirectoryName(path);
				directory = true;
			} while (path.Length != 0);
		}

		private TreeNode GetSubNode(string name, TreeNode node) {
			StringComparison comparer = StringComparison.CurrentCultureIgnoreCase;
			switch (Environment.OSVersion.Platform) {
				case PlatformID.MacOSX:
				case PlatformID.Unix:
					comparer = StringComparison.CurrentCulture;
					break;
			}
			if (node == null) {
				foreach (TreeNode child in treeView.Nodes) {
					if (name.Equals(child.Text.TrimEnd(SplitDir), comparer))
						return child;
				}
			} else {
				foreach (TreeNode child in node.Nodes) {
					if (name.Equals(child.Text, comparer))
						return child;
				}
			}
			return null;
		}

		private void FileTextBox_TextChanged(object sender, EventArgs e) {
			if (open)
				SelectButton.Enabled = (selectDirectory && FileUtils.FolderExists(FileName)) || (!selectDirectory && FileUtils.FileExists(FileName));
			else
				SelectButton.Enabled = FileName.IndexOfAny(InvalidFileChars) == -1;
		}

		private void comboBox_SelectedIndexChanged(object sender, EventArgs e) {
			string ext, target = FileName;
			ReloadExtensions();
			if (FileTextBox.TextLength != 0) {
				FileTextBox.SelectionStart = FileTextBox.TextLength;
				FileTextBox.SelectionLength = 0;
			}
			if (!IsAllowed(target, CurrentExtensions, out ext)) {
				if (target.Length == 0 || (CurrentExtensions.Count == 1 && CurrentExtensions[0] == "*"))
					return;
				if (ext != "*")
					target = Path.ChangeExtension(target, CurrentExtensions[0]);
				FileName = target;
			}
		}

		private void SelectButton_Click(object sender, EventArgs e) {
			if (open) {
				if (selectDirectory) {
					if (!FileUtils.FolderExists(FileName)) {
						StyledMessageBox.Show("The specified directory does not exist.", "Error", true, MessageBoxButtons.OK, MessageBoxIcon.Warning);
						return;
					}
				} else {
					if (!FileUtils.FileExists(FileName)) {
						StyledMessageBox.Show("The specified file does not exist.", "Error", true, MessageBoxButtons.OK, MessageBoxIcon.Warning);
						return;
					}
				}
			} else if (warnOverwrite) {
				if (FileUtils.FileExists(FileName)) {
					if (StyledMessageBox.Show("The specified file may be overwritten. Are you sure you want to continue?", "Question", true, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
						return;
				}
			}
			string ext;
			if (appendExtension && !(open || CurrentExtensions.Count == 0 || IsAllowed(FileName, CurrentExtensions, out ext)))
				FileName = FileName.TrimEnd(TrimDot) + CurrentExtensions[0];
			DialogResult = DialogResult.OK;
			LastPath = FileName;
			CloseAsync();
		}

		/// <summary>
		/// Called when the window is being disposed.
		/// </summary>
		/// <param name="e">Whether managed resources are about to be disposed.</param>
		protected override void OnDisposing(DisposeFormEventArgs e) {
			if (e.DisposeMode == DisposeOptions.FullDisposal) {
				if (rebuildDispatch != null) {
					rebuildDispatch.Dispose(false);
					rebuildDispatch = null;
				}
				lock (treeSyncRoot) {
					if (components != null) {
						components.Dispose();
						components = null;
					}
				}
			}
			base.OnDisposing(e);
		}

		/// <summary>
		/// Initializes the window and its controls.
		/// </summary>
		protected void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.SelectButton = new System.Windows.Forms.StyledButton();
			this.FileTextBox = new System.Windows.Forms.TextBox();
			this.treeView = new System.Windows.Forms.BufferedTreeView();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.comboBox = new System.Windows.Forms.StyledComboBox();
			this.label1 = new System.Windows.Forms.StyledLabel();
			this.label2 = new System.Windows.Forms.StyledLabel();
			this.SuspendLayout();
			// 
			// SelectButton
			// 
			this.SelectButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.SelectButton.Enabled = false;
			this.SelectButton.ForeColor = System.Drawing.Color.Black;
			this.SelectButton.Image = null;
			this.SelectButton.Location = new System.Drawing.Point(15, 483);
			this.SelectButton.Name = "SelectButton";
			this.SelectButton.Size = new System.Drawing.Size(509, 23);
			this.SelectButton.TabIndex = 0;
			this.SelectButton.Text = "Open";
			this.SelectButton.UseVisualStyleBackColor = true;
			this.SelectButton.Click += new System.EventHandler(this.SelectButton_Click);
			// 
			// FileTextBox
			// 
			this.FileTextBox.Location = new System.Drawing.Point(94, 427);
			this.FileTextBox.Name = "FileTextBox";
			this.FileTextBox.Size = new System.Drawing.Size(430, 20);
			this.FileTextBox.TabIndex = 1;
			this.FileTextBox.TextChanged += new System.EventHandler(this.FileTextBox_TextChanged);
			// 
			// treeView
			// 
			this.treeView.HideSelection = false;
			this.treeView.ImageIndex = 0;
			this.treeView.ImageList = this.imageList;
			this.treeView.Location = new System.Drawing.Point(15, 39);
			this.treeView.Margin = new System.Windows.Forms.Padding(6);
			this.treeView.Name = "treeView";
			this.treeView.SelectedImageIndex = 0;
			this.treeView.Size = new System.Drawing.Size(509, 378);
			this.treeView.TabIndex = 2;
			this.treeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView_BeforeExpand);
			this.treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_NodeMouseClick);
			this.treeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_NodeMouseDoubleClick);
			// 
			// imageList
			// 
			this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.imageList.ImageSize = new System.Drawing.Size(32, 32);
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// comboBox
			// 
			this.comboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.comboBox.DropDownHeight = 1;
			this.comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox.ForeColor = System.Drawing.Color.Black;
			this.comboBox.FormattingEnabled = true;
			this.comboBox.IntegralHeight = false;
			this.comboBox.Location = new System.Drawing.Point(94, 453);
			this.comboBox.Name = "comboBox";
			this.comboBox.Size = new System.Drawing.Size(430, 24);
			this.comboBox.TabIndex = 3;
			this.comboBox.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.comboBox.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Tile;
			this.label1.LineSpacingMultiplier = 0.15F;
			this.label1.Location = new System.Drawing.Point(17, 427);
			this.label1.Margin = new System.Windows.Forms.Padding(0);
			this.label1.Name = "label1";
			this.label1.Outline = System.Drawing.Color.Transparent;
			this.label1.OutlinePen = null;
			this.label1.PixelAlignment = System.Drawing.Drawing2D.PixelOffsetMode.Default;
			this.label1.Size = new System.Drawing.Size(74, 20);
			this.label1.TabIndex = 4;
			this.label1.Text = "Selected:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label1.TextBrush = null;
			this.label1.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// label2
			// 
			this.label2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Tile;
			this.label2.Location = new System.Drawing.Point(14, 453);
			this.label2.Margin = new System.Windows.Forms.Padding(0);
			this.label2.Name = "label2";
			this.label2.Outline = System.Drawing.Color.Transparent;
			this.label2.OutlinePen = null;
			this.label2.Size = new System.Drawing.Size(77, 21);
			this.label2.TabIndex = 5;
			this.label2.Text = "File Types:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label2.TextBrush = null;
			this.label2.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// FilePrompt
			// 
			this.AcceptButton = this.SelectButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.ClientSize = new System.Drawing.Size(539, 518);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.comboBox);
			this.Controls.Add(this.FileTextBox);
			this.Controls.Add(this.SelectButton);
			this.Controls.Add(this.treeView);
			this.MinimizeBox = false;
			this.Name = "FilePrompt";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Choose a file...";
			this.Controls.SetChildIndex(this.treeView, 0);
			this.Controls.SetChildIndex(this.SelectButton, 0);
			this.Controls.SetChildIndex(this.FileTextBox, 0);
			this.Controls.SetChildIndex(this.comboBox, 0);
			this.Controls.SetChildIndex(this.label1, 0);
			this.Controls.SetChildIndex(this.label2, 0);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}