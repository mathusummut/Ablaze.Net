using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace System.Windows.Forms {
	/// <summary>
	/// Shows an image message box.
	/// </summary>
	public static class ImageMessageBox {
		private const string DefaultTitle = "Image Viewer";

		/// <summary>
		/// Shows an image message box using the specified parameters.
		/// </summary>
		/// <param name="image">The image to show.</param>
		/// <param name="text">The text in the caption of the image.</param>
		/// <param name="title">The text in the title bar of the message box.</param>
		/// <param name="layout">The layout to use to represent the image.</param>
		/// <param name="interpolation">The interpolation method to use.</param>
		public static void Show(Image image, string text = "", string title = DefaultTitle, ImageLayout layout = ImageLayout.Zoom, InterpolationMode interpolation = InterpolationMode.HighQualityBicubic) {
			MessageLoop.ShowDialog(new StyledImgMsgBox(image, null, text, title, layout, interpolation, false));
		}

		/// <summary>
		/// Shows an image message box using the specified parameters.
		/// </summary>
		/// <param name="image">The image to show.</param>
		/// <param name="text">The text in the caption of the image.</param>
		/// <param name="title">The text in the title bar of the message box.</param>
		/// <param name="layout">The layout to use to represent the image.</param>
		/// <param name="interpolation">The interpolation method to use.</param>
		public static void ShowAsync(Image image, string text = "", string title = DefaultTitle, ImageLayout layout = ImageLayout.Zoom, InterpolationMode interpolation = InterpolationMode.HighQualityBicubic) {
			StyledImgMsgBox msgbox = new StyledImgMsgBox(image, null, text, title, layout, interpolation, false);
			msgbox.TopMost = true;
			msgbox.Show();
		}

		/// <summary>
		/// Shows an image message box using the specified parameters.
		/// </summary>
		/// <param name="imageLeft">The image to show on the left.</param>
		/// <param name="imageRight">The image to show on the right.</param>
		/// <param name="text">The text in the caption of the image.</param>
		/// <param name="title">The text in the title bar of the message box.</param>
		/// <param name="layout">The layout to use to represent the image.</param>
		/// <param name="interpolation">The interpolation method to use.</param>
		public static void Show(Image imageLeft, Image imageRight, string text = "", string title = DefaultTitle, ImageLayout layout = ImageLayout.Zoom, InterpolationMode interpolation = InterpolationMode.HighQualityBicubic) {
			MessageLoop.ShowDialog(new StyledImgMsgBox(imageLeft, imageRight, text, title, layout, interpolation, false));
		}

		/// <summary>
		/// Shows an image message box using the specified parameters.
		/// </summary>
		/// <param name="imageLeft">The image to show on the left.</param>
		/// <param name="imageRight">The image to show on the right.</param>
		/// <param name="text">The text in the caption of the image.</param>
		/// <param name="title">The text in the title bar of the message box.</param>
		/// <param name="layout">The layout to use to represent the image.</param>
		/// <param name="interpolation">The interpolation method to use.</param>
		public static void ShowAsync(Image imageLeft, Image imageRight, string text = "", string title = DefaultTitle, ImageLayout layout = ImageLayout.Zoom, InterpolationMode interpolation = InterpolationMode.HighQualityBicubic) {
			StyledImgMsgBox msgbox = new StyledImgMsgBox(imageLeft, imageRight, text, title, layout, interpolation, false);
			msgbox.TopMost = true;
			msgbox.Show();
		}

		/// <summary>
		/// Shows an image message box using the specified parameters.
		/// </summary>
		/// <param name="image">The image to show.</param>
		/// <param name="text">The text in the caption of the image.</param>
		/// <param name="title">The text in the title bar of the message box.</param>
		/// <param name="layout">The layout to use to represent the image.</param>
		/// <param name="interpolation">The interpolation method to use.</param>
		public static void Show(PixelWorker image, string text = "", string title = DefaultTitle, ImageLayout layout = ImageLayout.Zoom, InterpolationMode interpolation = InterpolationMode.HighQualityBicubic) {
			MessageLoop.ShowDialog(new StyledImgMsgBox(image == null ? null : image.ToBitmap(), null, text, title, layout, interpolation, false));
		}

		/// <summary>
		/// Shows an image message box using the specified parameters.
		/// </summary>
		/// <param name="image">The image to show.</param>
		/// <param name="text">The text in the caption of the image.</param>
		/// <param name="title">The text in the title bar of the message box.</param>
		/// <param name="layout">The layout to use to represent the image.</param>
		/// <param name="interpolation">The interpolation method to use.</param>
		public static void ShowAsync(PixelWorker image, string text = "", string title = DefaultTitle, ImageLayout layout = ImageLayout.Zoom, InterpolationMode interpolation = InterpolationMode.HighQualityBicubic) {
			StyledImgMsgBox msgbox = new StyledImgMsgBox(image == null ? null : image.ToBitmap(), null, text, title, layout, interpolation, false);
			msgbox.TopMost = true;
			msgbox.Show();
		}

		/// <summary>
		/// Shows an image message box using the specified parameters.
		/// </summary>
		/// <param name="imageLeft">The image to show on the left.</param>
		/// <param name="imageRight">The image to show on the right.</param>
		/// <param name="text">The text in the caption of the image.</param>
		/// <param name="title">The text in the title bar of the message box.</param>
		/// <param name="layout">The layout to use to represent the image.</param>
		/// <param name="interpolation">The interpolation method to use.</param>
		public static void Show(PixelWorker imageLeft, PixelWorker imageRight, string text = "", string title = DefaultTitle, ImageLayout layout = ImageLayout.Zoom, InterpolationMode interpolation = InterpolationMode.HighQualityBicubic) {
			MessageLoop.ShowDialog(new StyledImgMsgBox(imageLeft == null ? null : imageLeft.ToBitmap(), imageRight == null ? null : imageRight.ToBitmap(), text, title, layout, interpolation, false));
		}

		/// <summary>
		/// Shows an image message box using the specified parameters.
		/// </summary>
		/// <param name="imageLeft">The image to show on the left.</param>
		/// <param name="imageRight">The image to show on the right.</param>
		/// <param name="text">The text in the caption of the image.</param>
		/// <param name="title">The text in the title bar of the message box.</param>
		/// <param name="layout">The layout to use to represent the image.</param>
		/// <param name="interpolation">The interpolation method to use.</param>
		public static void ShowAsync(PixelWorker imageLeft, PixelWorker imageRight, string text = "", string title = DefaultTitle, ImageLayout layout = ImageLayout.Zoom, InterpolationMode interpolation = InterpolationMode.HighQualityBicubic) {
			StyledImgMsgBox msgbox = new StyledImgMsgBox(imageLeft == null ? null : imageLeft.ToBitmap(), imageRight == null ? null : imageRight.ToBitmap(), text, title, layout, interpolation, false);
			msgbox.TopMost = true;
			msgbox.Show();
		}

		/// <summary>
		/// Shows an image message box using the specified parameters.
		/// </summary>
		/// <param name="imagePath">The path to the image to show.</param>
		/// <param name="text">The text in the caption of the image.</param>
		/// <param name="title">The text in the title bar of the message box.</param>
		/// <param name="layout">The layout to use to represent the image.</param>
		/// <param name="interpolation">The interpolation method to use.</param>
		public static void Show(string imagePath, string text = "", string title = DefaultTitle, ImageLayout layout = ImageLayout.Zoom, InterpolationMode interpolation = InterpolationMode.HighQualityBicubic) {
			Bitmap image = null;
			if (!(imagePath == null || imagePath.Length == 0)) {
				try {
					using (FileStream stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
						using (Bitmap temp = new Bitmap(stream))
							image = ImageLib.ConvertPixelFormat(temp, PixelFormat.Format32bppPArgb);
					}
				} catch {
				}
			}
			MessageLoop.ShowDialog(new StyledImgMsgBox(image, null, text, title, layout, interpolation, true));
		}

		/// <summary>
		/// Shows an image message box using the specified parameters.
		/// </summary>
		/// <param name="imagePath">The path to the image to show.</param>
		/// <param name="text">The text in the caption of the image.</param>
		/// <param name="title">The text in the title bar of the message box.</param>
		/// <param name="layout">The layout to use to represent the image.</param>
		/// <param name="interpolation">The interpolation method to use.</param>
		public static void ShowAsync(string imagePath, string text = "", string title = DefaultTitle, ImageLayout layout = ImageLayout.Zoom, InterpolationMode interpolation = InterpolationMode.HighQualityBicubic) {
			Bitmap image = null;
			if (!(imagePath == null || imagePath.Length == 0)) {
				try {
					using (FileStream stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
						using (Bitmap temp = new Bitmap(stream))
							image = ImageLib.ConvertPixelFormat(temp, PixelFormat.Format32bppPArgb);
					}
				} catch {
				}
			}
			StyledImgMsgBox msgbox = new StyledImgMsgBox(image, null, text, title, layout, interpolation, true);
			msgbox.TopMost = true;
			msgbox.Show();
		}

		/// <summary>
		/// Shows an image message box using the specified parameters.
		/// </summary>
		/// <param name="leftImagePath">The path to the image to show on the left.</param>
		/// <param name="rightImagePath">The path to the image to show on the right.</param>
		/// <param name="text">The text in the caption of the image.</param>
		/// <param name="title">The text in the title bar of the message box.</param>
		/// <param name="layout">The layout to use to represent the image.</param>
		/// <param name="interpolation">The interpolation method to use.</param>
		public static void Show(string leftImagePath, string rightImagePath, string text = "", string title = DefaultTitle, ImageLayout layout = ImageLayout.Zoom, InterpolationMode interpolation = InterpolationMode.HighQualityBicubic) {
			Bitmap imageLeft = null;
			if (!(leftImagePath == null || leftImagePath.Length == 0)) {
				try {
					using (FileStream stream = new FileStream(leftImagePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
						using (Bitmap temp = new Bitmap(stream))
							imageLeft = ImageLib.ConvertPixelFormat(temp, PixelFormat.Format32bppPArgb);
					}
				} catch {
				}
			}
			Bitmap imageRight = null;
			if (!(rightImagePath == null || rightImagePath.Length == 0)) {
				try {
					using (FileStream stream = new FileStream(rightImagePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
						using (Bitmap temp = new Bitmap(stream))
							imageRight = ImageLib.ConvertPixelFormat(temp, PixelFormat.Format32bppPArgb);
					}
				} catch {
				}
			}
			MessageLoop.ShowDialog(new StyledImgMsgBox(imageLeft, imageRight, text, title, layout, interpolation, true));
		}

		/// <summary>
		/// Shows an image message box using the specified parameters.
		/// </summary>
		/// <param name="leftImagePath">The path to the image to show on the left.</param>
		/// <param name="rightImagePath">The path to the image to show on the right.</param>
		/// <param name="text">The text in the caption of the image.</param>
		/// <param name="title">The text in the title bar of the message box.</param>
		/// <param name="layout">The layout to use to represent the image.</param>
		/// <param name="interpolation">The interpolation method to use.</param>
		public static void ShowAsync(string leftImagePath, string rightImagePath, string text = "", string title = DefaultTitle, ImageLayout layout = ImageLayout.Zoom, InterpolationMode interpolation = InterpolationMode.HighQualityBicubic) {
			Bitmap imageLeft = null;
			if (!(leftImagePath == null || leftImagePath.Length == 0)) {
				try {
					using (FileStream stream = new FileStream(leftImagePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
						using (Bitmap temp = new Bitmap(stream))
							imageLeft = ImageLib.ConvertPixelFormat(temp, PixelFormat.Format32bppPArgb);
					}
				} catch {
				}
			}
			Bitmap imageRight = null;
			if (!(rightImagePath == null || rightImagePath.Length == 0)) {
				try {
					using (FileStream stream = new FileStream(rightImagePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
						using (Bitmap temp = new Bitmap(stream))
							imageRight = ImageLib.ConvertPixelFormat(temp, PixelFormat.Format32bppPArgb);
					}
				} catch {
				}
			}
			StyledImgMsgBox msgbox = new StyledImgMsgBox(imageLeft, imageRight, text, title, layout, interpolation, true);
			msgbox.TopMost = true;
			msgbox.Show();
		}

		private sealed class StyledImgMsgBox : StyledForm {
			private static Pen redPen = new Pen(Color.Red, 3f);
			public ImageLayout layout;
			private Color backColor = Color.Black;
			private Image imageLeft, imageRight;
			private StyledContextMenu RightClickContextMenu;
			private StyledItem colorItem, saveItem;
			private ColorDialog colorDialog;
			private FilePrompt saveDialog;
			private IContainer components;
			private InterpolationMode mode;
			private bool isMouseDown, dispose;
			private float zoom = 1f;
			private Point oldMouseLoc;
			private PointF offset, cumulativeDiff;

			public StyledImgMsgBox(Image imageLeft, Image imageRight, string text, string title, ImageLayout layout, InterpolationMode mode, bool dispose) {
				SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.CacheText | ControlStyles.Opaque | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
				InitializeComponent();
				this.dispose = dispose;
				Text = title;
				if (!(text == null || text.Length == 0)) {
					StyledLabel label = new StyledLabel() {
						BackColor = Color.Transparent,
						Blur = 4,
						Dock = DockStyle.Fill,
						Font = new Font("Calibri", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0))),
						ForeColor = Color.White,
						Name = "MessageLabel",
						RenderShadow = true,
						ShadowOpacity = 1.35F,
						ShadowColor = Color.Black,
						TextAlign = ContentAlignment.BottomCenter,
						Text = text + "\v"
					};
					Controls.Add(label);
				}
				this.layout = layout;
				this.imageLeft = imageLeft;
				this.imageRight = imageRight;
				if (imageLeft == null && imageRight == null)
					RightClickContextMenu.Dispose();
				else {
					if (imageRight == null || imageLeft == null) {
						Image image = imageRight == null ? imageLeft : imageRight;
						int offset = Math.Max(1, (int) (400f * (((float) image.Width) / image.Height)));
						if (offset <= 1000)
							ViewSize = new Size(offset, ViewSize.Height);
						else
							ViewSize = new Size(1000, (int) (ViewSize.Height * (1000f / offset)));
					} else
						ViewSize = new Size(750, ViewSize.Height);
				}
				this.mode = mode;
				WindowCursor = Cursors.Hand;
				MouseWheel += StyledImgMsgBox_MouseWheel;
				MouseMove += StyledImgMsgBox_MouseMove;
				MouseDown += StyledImgMsgBox_MouseDown;
				MouseUp += StyledImgMsgBox_MouseUp;
			}

			private void StyledImgMsgBox_MouseDown(object sender, MouseEventArgs e) {
				if (e.Button == MouseButtons.Left) {
					isMouseDown = true;
					oldMouseLoc = e.Location;
				}
			}

			private void StyledImgMsgBox_MouseMove(object sender, MouseEventArgs e) {
				if (isMouseDown) {
					float diffX = e.X - oldMouseLoc.X, diffY = e.Y - oldMouseLoc.Y;
					oldMouseLoc = e.Location;
					offset = new PointF(offset.X + diffX, offset.Y + diffY);
					cumulativeDiff = new PointF(cumulativeDiff.X - diffX * (zoom - 1f), cumulativeDiff.Y - diffY * (zoom - 1f));
					ClampOffset();
					Invalidate(false);
				}
			}

			private void StyledImgMsgBox_MouseUp(object sender, MouseEventArgs e) {
				if (e.Button == MouseButtons.Left)
					isMouseDown = false;
			}

			private void StyledImgMsgBox_MouseWheel(object sender, MouseEventArgs e) {
				bool zoomIn = e.Delta >= 0;
				for (int max = Math.Abs(e.Delta / SystemInformation.MouseWheelScrollDelta); max > 0; max--)
					ProcessMouseZoom(zoomIn, e.Location);
				Invalidate(false);
			}

			private void ProcessMouseZoom(bool isZoomIn, Point cursorPosition) {
				//to image coordinate
				float oldZoom = zoom;
				if (isZoomIn)
					zoom += 0.1f;
				else
					zoom = Math.Max(zoom - 0.1f, 1f);
				Size viewSize = ViewSize;
				float mult = zoom / oldZoom;
				if (isZoomIn)
					cumulativeDiff.X += (cursorPosition.X - viewSize.Width * 0.5f) / zoom;
				else
					cumulativeDiff.X *= mult;
				if (isZoomIn)
					cumulativeDiff.Y += (cursorPosition.Y - viewSize.Height * 0.5f) / zoom;
				else
					cumulativeDiff.Y *= mult;
				offset = new PointF((viewSize.Width - (viewSize.Width * zoom)) * 0.5f - cumulativeDiff.X, (viewSize.Height - viewSize.Height * zoom) * 0.5f - cumulativeDiff.Y);
				ClampOffset();
			}

			private void ClampOffset() {
				Size area = ViewSize;
				if (offset.X > 0f) {
					offset.X = 0f;
					cumulativeDiff.X = (area.Width - (area.Width * zoom)) * 0.5f;
				}
				if (offset.Y > 0f) {
					offset.Y = 0f;
					cumulativeDiff.Y = (area.Height - area.Height * zoom) * 0.5f;
				}
				Size size = ViewSize;
				float minX = size.Width - (int) (size.Width * zoom);
				if (offset.X < minX) {
					cumulativeDiff.X = 0f;
					offset.X = minX;
				}
				float minY = size.Height - (int) (size.Height * zoom);
				if (offset.Y < minY) {
					cumulativeDiff.Y = 0f;
					offset.Y = minY;
				}
			}

			protected override void OnPaint(Graphics g, Rectangle clippingRect) {
				g.CompositingQuality = CompositingQuality.HighQuality;
				g.PixelOffsetMode = PixelOffsetMode.HighQuality;
				g.SmoothingMode = SmoothingMode.HighQuality;
				g.CompositingMode = CompositingMode.SourceOver;
				g.Clear(backColor);
				Size size = ViewSize;
				if (imageLeft == null && imageRight == null) {
					g.DrawLine(redPen, Point.Empty, new Point(size.Width, size.Height));
					g.DrawLine(redPen, new Point(size.Width, 0), new Point(0, size.Height));
				} else {
					size.Width = (int) (size.Width * zoom);
					size.Height = (int) (size.Height * zoom);
					g.InterpolationMode = mode;
					if (imageLeft == null || imageRight == null)
						ImageLib.DrawImageWithLayout(g, imageRight == null ? imageLeft : imageRight, new RectangleF(offset, size), layout);
					else {
						float halfWidth = size.Width / 2;
						ImageLib.DrawImageWithLayout(g, imageLeft, new RectangleF(offset.X, offset.Y, halfWidth, size.Height), layout);
						ImageLib.DrawImageWithLayout(g, imageRight, new RectangleF(offset.X + halfWidth, offset.Y, (int) Math.Ceiling(halfWidth), size.Height), layout);
					}
				}
				base.OnPaint(g, clippingRect);
			}

			private void ColorItem_Click(object sender, EventArgs e) {
				if (colorDialog == null)
					colorDialog = new ColorDialog();
				colorDialog.Color = backColor;
				if (colorDialog.ShowDialog() == DialogResult.OK) {
					backColor = colorDialog.Color;
					Invalidate(false);
				}
			}

			private void SaveToolStripMenuItem_Click(object sender, EventArgs e) {
				Point point = PointToClient(Cursor.Position);
				if (saveDialog == null) {
					saveDialog = new FilePrompt();
					saveDialog.Open = false;
					saveDialog.Title = "Save image as...";
					saveDialog.FileName = "Picture.png";
					saveDialog.Filter = "PNG files (*.png)|*.png|JPG files (*.jpg)|*.jpg|Bitmap files (*.bmp)|*.bmp|GIF files (*.gif)|*.gif|TIF files (*.tif)|*.tif";
				}
				if (MessageLoop.ShowDialog(saveDialog, false) == DialogResult.OK) {
					string fileName = saveDialog.FileName;
					if (imageRight == null || imageLeft == null)
						(imageRight == null ? imageLeft : imageRight).Save(fileName, ToImageFormat(Path.GetExtension(fileName)));
					else if (point.X <= Width / 2)
						imageLeft.Save(fileName, ToImageFormat(Path.GetExtension(fileName)));
					else
						imageRight.Save(fileName, ToImageFormat(Path.GetExtension(fileName)));
				}
			}

			private static ImageFormat ToImageFormat(string extension) {
				switch (extension) {
					case ".gif":
						return ImageFormat.Gif;
					case ".ico":
					case ".icon":
						return ImageFormat.Icon;
					case ".jpg":
					case ".jpeg":
						return ImageFormat.Jpeg;
					case ".png":
						return ImageFormat.Png;
					case ".tif":
					case ".tiff":
						return ImageFormat.Tiff;
					case ".wmf":
						return ImageFormat.Wmf;
					default:
						return ImageFormat.Bmp;
				}
			}

			/// <summary>
			/// Called when the window is being disposed.
			/// </summary>
			/// <param name="e">Whether managed resources are about to be disposed.</param>
			protected override void OnDisposing(DisposeFormEventArgs e) {
				if (e.DisposeMode == DisposeOptions.FullDisposal) {
					if (colorDialog != null) {
						colorDialog.Dispose();
						colorDialog = null;
					}
					if (saveDialog != null) {
						saveDialog.Dispose();
						saveDialog = null;
					}
					if (dispose) {
						if (imageLeft != null) {
							imageLeft.Dispose();
							imageLeft = null;
						}
						if (imageRight != null) {
							imageRight.Dispose();
							imageRight = null;
						}
					}
				}
			}

			/// <summary>
			/// Initializes the window and its controls.
			/// </summary>
			private void InitializeComponent() {
				this.components = new System.ComponentModel.Container();
				this.RightClickContextMenu = new System.Windows.Forms.StyledContextMenu(this.components);
				this.colorItem = new System.Windows.Forms.StyledItem();
				this.saveItem = new System.Windows.Forms.StyledItem();
				this.RightClickContextMenu.SuspendLayout();
				this.SuspendLayout();
				// 
				// RightClickContextMenu
				// 
				this.RightClickContextMenu.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
				this.RightClickContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
					this.colorItem,
					this.saveItem });
				this.RightClickContextMenu.Name = "rightClickContextMenu";
				// 
				// colorItem
				// 
				this.colorItem.Font = new System.Drawing.Font("Segoe UI", 9.25F);
				this.colorItem.Name = "colorItem";
				this.colorItem.Size = new System.Drawing.Size(31, 25);
				this.colorItem.Text = "Change background (for images with transparency)";
				this.colorItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
				this.colorItem.Click += ColorItem_Click;
				// 
				// saveItem
				// 
				this.saveItem.Font = new System.Drawing.Font("Segoe UI", 9.25F);
				this.saveItem.Name = "saveItem";
				this.saveItem.Size = new System.Drawing.Size(31, 25);
				this.saveItem.Text = "Save this image";
				this.saveItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
				this.saveItem.Click += SaveToolStripMenuItem_Click;
				// 
				// ImageMessageBox
				// 
				this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
				this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
				this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
				this.ViewSize = new System.Drawing.Size(650, 400);
				this.ContextMenuStrip = RightClickContextMenu;
				this.MaximizeBox = true;
				this.Name = "ImageMessageBox";
				this.ShowIcon = false;
				this.ShowInTaskbar = false;
				this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
				this.Text = "Message";
				this.RightClickContextMenu.ResumeLayout(false);
				this.ResumeLayout(false);
			}
		}
	}
}