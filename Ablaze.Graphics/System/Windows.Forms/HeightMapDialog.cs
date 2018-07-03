using System.ComponentModel;
using System.Drawing;
using System.Graphics.Models;
using System.IO;
using System.Numerics;
using System.Threading;

namespace System.Windows.Forms {
	/// <summary>
	/// A dialog to configure height map creation parameters.
	/// </summary>
	public class HeightMapDialog : StyledForm {
		/// <summary>
		/// Fired when a new mesh component has loaded.
		/// </summary>
		public event Action<MeshComponent> ComponentLoaded;
		private StyledLabel smoothnessLabel, fileNameLabel, scaleXLabel, scaleYLabel, greenMultiplierLabel, redMultiplierLabel, blueMultiplierLabel, sizeLabel;
		private NumericUpDown smoothnessSelector, scaleXSelector, scaleYSelector, redMultiplierSelector, greenMultiplierSelector, blueMultiplierSelector;
		private FilePrompt openFileDialog;
		private StyledButton loadButton, fileButton;
		private WaitCallback generate;
		private Bitmap image;

		/// <summary>
		/// Initializes a new height map dialog.
		/// </summary>
		public HeightMapDialog() {
			InitializeComponent();
			BackColorOpacity = 190;
			try {
				Icon = Properties.Resources.Ablaze;
			} catch {
			}
			generate = GenerateHeightMap;
			DragEnter += HeightMapDialog_DragEnter;
			DragDrop += HeightMapDialog_DragDrop;
		}

		/// <summary>
		/// Initializes the new height map dialog.
		/// </summary>
		/// <param name="container">The container to add this dialog to.</param>
		public HeightMapDialog(IContainer container) : this() {
			if (container != null)
				container.Add(this);
			Font = new Font("Calibri", 12F);
		}

		private static void HeightMapDialog_DragEnter(object sender, DragEventArgs e) {
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
				e.Effect = DragDropEffects.Link;
		}

		private void HeightMapDialog_DragDrop(object sender, DragEventArgs e) {
			string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
			if (!(files == null || files.Length == 0)) {
				openFileDialog.FileName = files[0];
				try {
					fileNameLabel.Text = Path.GetFileName(files[0]);
				} catch {
					fileNameLabel.Text = "Invalid filename";
				}
				Thread thread = new Thread(LoadFile);
				thread.Name = "LoadFileThread";
				thread.IsBackground = true;
				thread.Start(files[0]);
			}
		}

		/// <summary>
		/// Called when the window is shown.
		/// </summary>
		/// <param name="e">Ignored.</param>
		protected override void OnShown(EventArgs e) {
			base.OnShown(e);
			UIScaler.AddToScaler(this);
		}

		private void fileButton_Click(object sender, EventArgs e) {
			if (MessageLoop.ShowDialog(openFileDialog, false) == DialogResult.OK) {
				try {
					fileNameLabel.Text = Path.GetFileName(openFileDialog.FileName);
				} catch {
					fileNameLabel.Text = "Invalid filename";
				}
				Thread thread = new Thread(LoadFile);
				thread.Name = "LoadFileThread";
				thread.IsBackground = true;
				thread.Start(openFileDialog.FileName);
			}
		}

		private void LoadFile(object file) {
			string fileName = (string) file;
			try {
				using (BufferedStream stream = FileUtils.LoadFileBuffered(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, true)) {
					Bitmap image = Path.GetExtension(fileName).Trim().ToLower() == ".tga" ? new TargaImage(stream, true).Image : new Bitmap(stream);
					sizeLabel.Text = "Size: " + image.Width + "x" + image.Height;
					this.image = image;
				}
			} catch {
				sizeLabel.Text = "An error occurred";
			}
		}

		private void loadButton_Click(object sender, EventArgs e) {
			Bitmap image = this.image;
			if (image == null)
				StyledMessageBox.Show("No height map is selected yet.", "Info", true, MessageBoxButtons.OK, MessageBoxIcon.Information);
			else {
				loadButton.Enabled = false;
				loadButton.Text = "Loading...";
				ThreadPool.UnsafeQueueUserWorkItem(generate, new HeightMapParams(image, new Vector2((float) scaleXSelector.Value, (float) scaleYSelector.Value), new Vector3((float) redMultiplierSelector.Value, (float) greenMultiplierSelector.Value, (float) blueMultiplierSelector.Value), (float) smoothnessSelector.Value, false, false));
			}
		}

		private void GenerateHeightMap(object parameters) {
			HeightMapParams param = (HeightMapParams) parameters;
			if (param.Image == null)
				return;
			try {
				MeshComponent component = HeightMapGenerator.GenerateHeightmap(ref param);
				component.Textures = new ITexture[] { new Texture2D(param.Image, NPotTextureScaleMode.ScaleUp, false) };
				component.Location -= component.Bounds * 0.5f;
				Action<MeshComponent> componentLoaded = ComponentLoaded;
				if (componentLoaded != null)
					componentLoaded(component);
			} catch {
				sizeLabel.Text = "An error occurred";
			}
			loadButton.Text = "Load";
			loadButton.Enabled = true;
		}

		/// <summary>
		/// Called when the height map dialog is about to close.
		/// </summary>
		/// <param name="reason">The reason the form will be closed.</param>
		protected override bool OnQueryClose(CloseReason reason) {
			if (image != null) {
				image.DisposeSafe();
				image = null;
			}
			return true;
		}

		/// <summary>
		/// Called when the window is being disposed.
		/// </summary>
		/// <param name="e">Whether managed resources are about to be disposed.</param>
		protected override void OnDisposing(DisposeFormEventArgs e) {
			if (e.DisposeMode == DisposeOptions.FullDisposal)
				openFileDialog.Dispose();
		}

		/// <summary>
		/// Initializes the window and its controls.
		/// </summary>
		private void InitializeComponent() {
			this.openFileDialog = new System.Windows.Forms.FilePrompt();
			this.fileButton = new System.Windows.Forms.StyledButton();
			this.fileNameLabel = new System.Windows.Forms.StyledLabel();
			this.smoothnessLabel = new System.Windows.Forms.StyledLabel();
			this.scaleXLabel = new System.Windows.Forms.StyledLabel();
			this.scaleYLabel = new System.Windows.Forms.StyledLabel();
			this.redMultiplierLabel = new System.Windows.Forms.StyledLabel();
			this.greenMultiplierLabel = new System.Windows.Forms.StyledLabel();
			this.blueMultiplierLabel = new System.Windows.Forms.StyledLabel();
			this.smoothnessSelector = new System.Windows.Forms.NumericUpDown();
			this.scaleXSelector = new System.Windows.Forms.NumericUpDown();
			this.scaleYSelector = new System.Windows.Forms.NumericUpDown();
			this.redMultiplierSelector = new System.Windows.Forms.NumericUpDown();
			this.greenMultiplierSelector = new System.Windows.Forms.NumericUpDown();
			this.blueMultiplierSelector = new System.Windows.Forms.NumericUpDown();
			this.loadButton = new System.Windows.Forms.StyledButton();
			this.sizeLabel = new System.Windows.Forms.StyledLabel();
			((System.ComponentModel.ISupportInitialize)(this.smoothnessSelector)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.scaleXSelector)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.scaleYSelector)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.redMultiplierSelector)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.greenMultiplierSelector)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.blueMultiplierSelector)).BeginInit();
			this.SuspendLayout();
			// 
			// openFileDialog
			// 
			this.openFileDialog.ActiveBorderOpacity = 0.75F;
			this.openFileDialog.AllFilesString = "All Files";
			this.openFileDialog.AllowDrop = true;
			this.openFileDialog.BackColorOpacity = ((byte)(255));
			this.openFileDialog.BorderCursor = System.Windows.Forms.Cursors.Default;
			this.openFileDialog.BorderWidth = 4;
			this.openFileDialog.ButtonText = "Open";
			this.openFileDialog.CausesValidation = false;
			this.openFileDialog.ClientSize = new System.Drawing.Size(539, 518);
			this.openFileDialog.EnableFullscreenOnAltEnter = false;
			this.openFileDialog.FileName = "";
			this.openFileDialog.FileNames = new string[] {
        ""};
			this.openFileDialog.Filter = "";
			this.openFileDialog.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.openFileDialog.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.openFileDialog.InactiveBorderOpacity = 0.5F;
			this.openFileDialog.InlineColor = System.Drawing.Color.Black;
			this.openFileDialog.KeyPreview = true;
			this.openFileDialog.Location = new System.Drawing.Point(0, 0);
			this.openFileDialog.MinimizeBox = false;
			this.openFileDialog.MinimizeEnabled = false;
			this.openFileDialog.MinimumSize = new System.Drawing.Size(200, 50);
			this.openFileDialog.Name = "openFileDialog";
			this.openFileDialog.OutlineColor = System.Drawing.Color.Black;
			this.openFileDialog.Padding = new System.Windows.Forms.Padding(4, 29, 4, 4);
			this.openFileDialog.SelectDirectory = false;
			this.openFileDialog.ShowIcon = false;
			this.openFileDialog.ShowInTaskbar = false;
			this.openFileDialog.ShowShadow = true;
			this.openFileDialog.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.openFileDialog.SystemMenu = null;
			this.openFileDialog.SystemMenuStrip = null;
			this.openFileDialog.Text = "Choose a file...";
			this.openFileDialog.TitleBarBadding = new System.Drawing.Size(0, 1);
			this.openFileDialog.TitleBarHeight = 29;
			this.openFileDialog.Visible = false;
			this.openFileDialog.WarnOverwrite = false;
			this.openFileDialog.WindowCursor = System.Windows.Forms.Cursors.Default;
			// 
			// fileButton
			// 
			this.fileButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.fileButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
			this.fileButton.Image = null;
			this.fileButton.Location = new System.Drawing.Point(14, 43);
			this.fileButton.Name = "fileButton";
			this.fileButton.Size = new System.Drawing.Size(277, 30);
			this.fileButton.TabIndex = 0;
			this.fileButton.Text = "Select File";
			this.fileButton.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
			this.fileButton.UseVisualStyleBackColor = true;
			this.fileButton.Click += new System.EventHandler(this.fileButton_Click);
			// 
			// fileNameLabel
			// 
			this.fileNameLabel.BackColor = System.Drawing.Color.Transparent;
			this.fileNameLabel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Tile;
			this.fileNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.fileNameLabel.ForeColor = System.Drawing.Color.White;
			this.fileNameLabel.Location = new System.Drawing.Point(14, 76);
			this.fileNameLabel.Name = "fileNameLabel";
			this.fileNameLabel.OutlinePen = null;
			this.fileNameLabel.Size = new System.Drawing.Size(277, 19);
			this.fileNameLabel.TabIndex = 1;
			this.fileNameLabel.Text = "Selected file name";
			this.fileNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.fileNameLabel.TextBrush = null;
			// 
			// smoothnessLabel
			// 
			this.smoothnessLabel.BackColor = System.Drawing.Color.Transparent;
			this.smoothnessLabel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Tile;
			this.smoothnessLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.smoothnessLabel.ForeColor = System.Drawing.Color.Aqua;
			this.smoothnessLabel.Location = new System.Drawing.Point(14, 104);
			this.smoothnessLabel.Name = "smoothnessLabel";
			this.smoothnessLabel.OutlinePen = null;
			this.smoothnessLabel.Size = new System.Drawing.Size(84, 23);
			this.smoothnessLabel.TabIndex = 2;
			this.smoothnessLabel.Text = "Smoothing:";
			this.smoothnessLabel.TextBrush = null;
			// 
			// scaleXLabel
			// 
			this.scaleXLabel.BackColor = System.Drawing.Color.Transparent;
			this.scaleXLabel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Tile;
			this.scaleXLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
			this.scaleXLabel.ForeColor = System.Drawing.Color.White;
			this.scaleXLabel.Location = new System.Drawing.Point(14, 138);
			this.scaleXLabel.Name = "scaleXLabel";
			this.scaleXLabel.OutlinePen = null;
			this.scaleXLabel.Size = new System.Drawing.Size(71, 23);
			this.scaleXLabel.TabIndex = 3;
			this.scaleXLabel.Text = "Scale X:";
			this.scaleXLabel.TextBrush = null;
			// 
			// scaleYLabel
			// 
			this.scaleYLabel.BackColor = System.Drawing.Color.Transparent;
			this.scaleYLabel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Tile;
			this.scaleYLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
			this.scaleYLabel.ForeColor = System.Drawing.Color.White;
			this.scaleYLabel.Location = new System.Drawing.Point(14, 171);
			this.scaleYLabel.Name = "scaleYLabel";
			this.scaleYLabel.OutlinePen = null;
			this.scaleYLabel.Size = new System.Drawing.Size(71, 23);
			this.scaleYLabel.TabIndex = 4;
			this.scaleYLabel.Text = "Scale Y:";
			this.scaleYLabel.TextBrush = null;
			// 
			// redMultiplierLabel
			// 
			this.redMultiplierLabel.BackColor = System.Drawing.Color.Transparent;
			this.redMultiplierLabel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Tile;
			this.redMultiplierLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
			this.redMultiplierLabel.ForeColor = System.Drawing.Color.Red;
			this.redMultiplierLabel.Location = new System.Drawing.Point(14, 202);
			this.redMultiplierLabel.Name = "redMultiplierLabel";
			this.redMultiplierLabel.OutlinePen = null;
			this.redMultiplierLabel.Size = new System.Drawing.Size(107, 23);
			this.redMultiplierLabel.TabIndex = 5;
			this.redMultiplierLabel.Text = "Red Multiplier:";
			this.redMultiplierLabel.TextBrush = null;
			// 
			// greenMultiplierLabel
			// 
			this.greenMultiplierLabel.BackColor = System.Drawing.Color.Transparent;
			this.greenMultiplierLabel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Tile;
			this.greenMultiplierLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
			this.greenMultiplierLabel.ForeColor = System.Drawing.Color.Green;
			this.greenMultiplierLabel.Location = new System.Drawing.Point(14, 235);
			this.greenMultiplierLabel.Name = "greenMultiplierLabel";
			this.greenMultiplierLabel.OutlinePen = null;
			this.greenMultiplierLabel.Size = new System.Drawing.Size(122, 23);
			this.greenMultiplierLabel.TabIndex = 6;
			this.greenMultiplierLabel.Text = "Green Multiplier:";
			this.greenMultiplierLabel.TextBrush = null;
			// 
			// blueMultiplierLabel
			// 
			this.blueMultiplierLabel.BackColor = System.Drawing.Color.Transparent;
			this.blueMultiplierLabel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Tile;
			this.blueMultiplierLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
			this.blueMultiplierLabel.ForeColor = System.Drawing.Color.DodgerBlue;
			this.blueMultiplierLabel.Location = new System.Drawing.Point(14, 271);
			this.blueMultiplierLabel.Name = "blueMultiplierLabel";
			this.blueMultiplierLabel.OutlinePen = null;
			this.blueMultiplierLabel.Size = new System.Drawing.Size(111, 23);
			this.blueMultiplierLabel.TabIndex = 7;
			this.blueMultiplierLabel.Text = "Blue Multiplier:";
			this.blueMultiplierLabel.TextBrush = null;
			// 
			// smoothnessSelector
			// 
			this.smoothnessSelector.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
			this.smoothnessSelector.Location = new System.Drawing.Point(140, 102);
			this.smoothnessSelector.Maximum = new decimal(new int[] {
            32768,
            0,
            0,
            0});
			this.smoothnessSelector.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
			this.smoothnessSelector.Name = "smoothnessSelector";
			this.smoothnessSelector.Size = new System.Drawing.Size(151, 24);
			this.smoothnessSelector.TabIndex = 9;
			this.smoothnessSelector.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.smoothnessSelector.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
			// 
			// scaleXSelector
			// 
			this.scaleXSelector.DecimalPlaces = 6;
			this.scaleXSelector.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
			this.scaleXSelector.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.scaleXSelector.Location = new System.Drawing.Point(140, 134);
			this.scaleXSelector.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
			this.scaleXSelector.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            393216});
			this.scaleXSelector.Name = "scaleXSelector";
			this.scaleXSelector.Size = new System.Drawing.Size(151, 24);
			this.scaleXSelector.TabIndex = 10;
			this.scaleXSelector.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.scaleXSelector.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// scaleYSelector
			// 
			this.scaleYSelector.DecimalPlaces = 6;
			this.scaleYSelector.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
			this.scaleYSelector.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.scaleYSelector.Location = new System.Drawing.Point(140, 167);
			this.scaleYSelector.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
			this.scaleYSelector.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            393216});
			this.scaleYSelector.Name = "scaleYSelector";
			this.scaleYSelector.Size = new System.Drawing.Size(151, 24);
			this.scaleYSelector.TabIndex = 11;
			this.scaleYSelector.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.scaleYSelector.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// redMultiplierSelector
			// 
			this.redMultiplierSelector.DecimalPlaces = 6;
			this.redMultiplierSelector.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
			this.redMultiplierSelector.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
			this.redMultiplierSelector.Location = new System.Drawing.Point(140, 200);
			this.redMultiplierSelector.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
			this.redMultiplierSelector.Name = "redMultiplierSelector";
			this.redMultiplierSelector.Size = new System.Drawing.Size(151, 24);
			this.redMultiplierSelector.TabIndex = 12;
			this.redMultiplierSelector.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.redMultiplierSelector.Value = new decimal(new int[] {
            299,
            0,
            0,
            196608});
			// 
			// greenMultiplierSelector
			// 
			this.greenMultiplierSelector.DecimalPlaces = 6;
			this.greenMultiplierSelector.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
			this.greenMultiplierSelector.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
			this.greenMultiplierSelector.Location = new System.Drawing.Point(140, 233);
			this.greenMultiplierSelector.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
			this.greenMultiplierSelector.Name = "greenMultiplierSelector";
			this.greenMultiplierSelector.Size = new System.Drawing.Size(151, 24);
			this.greenMultiplierSelector.TabIndex = 13;
			this.greenMultiplierSelector.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.greenMultiplierSelector.Value = new decimal(new int[] {
            587,
            0,
            0,
            196608});
			// 
			// blueMultiplierSelector
			// 
			this.blueMultiplierSelector.DecimalPlaces = 6;
			this.blueMultiplierSelector.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
			this.blueMultiplierSelector.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
			this.blueMultiplierSelector.Location = new System.Drawing.Point(140, 269);
			this.blueMultiplierSelector.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
			this.blueMultiplierSelector.Name = "blueMultiplierSelector";
			this.blueMultiplierSelector.Size = new System.Drawing.Size(151, 24);
			this.blueMultiplierSelector.TabIndex = 14;
			this.blueMultiplierSelector.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.blueMultiplierSelector.Value = new decimal(new int[] {
            114,
            0,
            0,
            196608});
			// 
			// loadButton
			// 
			this.loadButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.loadButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
			this.loadButton.Image = null;
			this.loadButton.Location = new System.Drawing.Point(14, 311);
			this.loadButton.Name = "loadButton";
			this.loadButton.Size = new System.Drawing.Size(277, 34);
			this.loadButton.TabIndex = 17;
			this.loadButton.Text = "Load";
			this.loadButton.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
			this.loadButton.UseVisualStyleBackColor = true;
			this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
			// 
			// sizeLabel
			// 
			this.sizeLabel.BackColor = System.Drawing.Color.Transparent;
			this.sizeLabel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Tile;
			this.sizeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.sizeLabel.ForeColor = System.Drawing.Color.White;
			this.sizeLabel.Location = new System.Drawing.Point(10, 348);
			this.sizeLabel.Margin = new System.Windows.Forms.Padding(0);
			this.sizeLabel.Name = "sizeLabel";
			this.sizeLabel.OutlinePen = null;
			this.sizeLabel.Size = new System.Drawing.Size(286, 25);
			this.sizeLabel.TabIndex = 18;
			this.sizeLabel.Text = "Size: 0x0";
			this.sizeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.sizeLabel.TextBrush = null;
			// 
			// HeightMapDialog
			// 
			this.AcceptButton = this.fileButton;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.MidnightBlue;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.ClientSize = new System.Drawing.Size(304, 377);
			this.Controls.Add(this.loadButton);
			this.Controls.Add(this.blueMultiplierSelector);
			this.Controls.Add(this.greenMultiplierSelector);
			this.Controls.Add(this.redMultiplierSelector);
			this.Controls.Add(this.scaleYSelector);
			this.Controls.Add(this.scaleXSelector);
			this.Controls.Add(this.smoothnessSelector);
			this.Controls.Add(this.blueMultiplierLabel);
			this.Controls.Add(this.greenMultiplierLabel);
			this.Controls.Add(this.redMultiplierLabel);
			this.Controls.Add(this.scaleYLabel);
			this.Controls.Add(this.scaleXLabel);
			this.Controls.Add(this.fileNameLabel);
			this.Controls.Add(this.smoothnessLabel);
			this.Controls.Add(this.fileButton);
			this.Controls.Add(this.sizeLabel);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MaximizeBox = false;
			this.MaximizeEnabled = false;
			this.MinimizeBox = false;
			this.MinimizeEnabled = false;
			this.Name = "HeightMapDialog";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Height Map Generator";
			((System.ComponentModel.ISupportInitialize)(this.smoothnessSelector)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.scaleXSelector)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.scaleYSelector)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.redMultiplierSelector)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.greenMultiplierSelector)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.blueMultiplierSelector)).EndInit();
			this.ResumeLayout(false);

		}
	}
}