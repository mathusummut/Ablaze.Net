using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Timers;
using System.Windows.Forms;

namespace PixelArtEffector {
	using Timer = System.Timers.Timer;

	/// <summary>
	/// The main application window.
	/// </summary>
	public class PixelArtEffectorForm : StyledForm {
		private IContainer components;
		private StyledMenuStrip menuStrip1;
		private FilePrompt fileDialog;
		private StyledContextMenu contextMenuStrip1;
		private Bitmap background, background2, background3, background4, savedBackground;
		private SliderDialog sliderDialog;
		private StyledLabel aboutLabel;
		private bool isMouseDown, isAboutShown, recess;
		private float zoom = 1f;
		private string loadedPath;
		private Point oldMouseLoc;
		private PointF offset, cumulativeDiff;
		private Timer aboutTimer;
		private StyledItem editToolStripMenuItem, undoMenuItem, styledItem1, toolStripMenuItem1, toolStripMenuItem2, toolStripMenuItem3,
			toolStripMenuItem4, toolStripMenuItem5, toolStripMenuItem6, toolStripMenuItem7, toolStripMenuItem8, medianToolStripMenuItem,
			sharpenToolStripMenuItem, unsharpMaskToolStripMenuItem, ditheredThresholdToolStripMenuItem, brightnessToolStripMenuItem,
			contrastToolStripMenuItem, gammaToolStripMenuItem, lightnessToolStripMenuItem, dilateToolStripMenuItem, erodeToolStripMenuItem,
			openToolStripMenuItem, closeMorphologicalToolStripMenuItem, normalizeToolStripMenuItem, saltAndPepperNoiseToolStripMenuItem,
			grayscaleToolStripMenuItem, sobelEdgeDetectionToolStripMenuItem, prewittEdgeDetectionToolStripMenuItem, premultiplyAlpha,
			xbrMenuItem, saveMenuItem, saveAsMenuItem, toolStripMenuItem9, antialiasItem, medianEnhance, smoothStretching, ungrayscale,
			signedDistanceField, scaleItem, highPassFilter, lowPassFilter, equalizeMenuItem;

		/// <summary>
		/// The entry point of the application.
		/// </summary>
		/// <param name="args">The console arguments passed to the application.</param>
		[STAThread]
		public static void Main(string[] args) {
#if NET45
			try {
				System.Runtime.ProfileOptimization.SetProfileRoot(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location));
				System.Runtime.ProfileOptimization.StartProfile("LoadCache.prf");
			} catch {
			}
#endif
			string file = null;
			if (args.Length != 0)
				file = args[0].Trim();
			MessageLoop.Run(new PixelArtEffectorForm(file));
		}

		/// <summary>
		/// Initializes a new window.
		/// </summary>
		public PixelArtEffectorForm() {
			InitializeComponent();
			BackColorOpacity = 150;
			BackColor = Color.Black;
			fileDialog.FileName = "Image.jpg";
			fileDialog.Filter = "Image Files|" + Extensions.ImageFileExtensions;
			WindowCursor = Cursors.Hand;
			menuStrip1.Cursor = Cursors.Arrow;
			MouseWheel += StyledImgMsgBox_MouseWheel;
			MouseMove += StyledImgMsgBox_MouseMove;
			MouseDown += StyledImgMsgBox_MouseDown;
			MouseUp += StyledImgMsgBox_MouseUp;
			menuStrip1.MouseUp += MenuStrip1_MouseUp;
			aboutLabel = new StyledLabel() {
				BackColor = Color.Transparent,
				Blur = 4,
				Font = new Font("Calibri Light", 14.25F),
				ForeColor = Color.White,
				Location = new Point(0, -230),
				Name = nameof(aboutLabel),
				Outline = Color.Transparent,
				OutlinePen = null,
				ShadowOpacity = 1.35F,
				RenderShadow = true,
				Size = new Size(ViewSize.Width, 230),
				Text = "Enhance images using a vast selection of filters and effects.\n\nCopyright 2015 by MathuSum Mut\n\nEmail: mathusum.mut @gmail.com",
				TextAlign = ContentAlignment.BottomCenter
			};
			Controls.Add(aboutLabel);
			aboutTimer = new Timer(10.0);
			aboutTimer.Elapsed += AboutTimer_Elapsed;
			DragEnter += PixelArtEffector_DragEnter;
			DragDrop += PixelArtEffector_DragDrop;
		}

		/// <summary>
		/// Initializes a new window and opens the specified image.
		/// </summary>
		/// <param name="filename">The path to the image to open.</param>
		public PixelArtEffectorForm(string filename) : this() {
			if (filename == null || filename.Length == 0)
				return;
			else if (FileUtils.FileExists(filename))
				Open(filename);
		}

		private static void PixelArtEffector_DragEnter(object sender, DragEventArgs e) {
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
				e.Effect = DragDropEffects.Link;
		}

		private void PixelArtEffector_DragDrop(object sender, DragEventArgs e) {
			string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
			if (!(files == null || files.Length == 0))
				Open(files[0]);
		}

		private void AboutTimer_Elapsed(object sender, ElapsedEventArgs e) {
			int newTop;
			if (recess) {
				newTop = aboutLabel.Top - 30;
				if (newTop <= -aboutLabel.Height) {
					newTop = -aboutLabel.Height;
					aboutLabel.Top = newTop;
					aboutTimer.Stop();
				} else
					aboutLabel.Top = newTop;
			} else {
				newTop = aboutLabel.Top + 30;
				if (newTop >= 0) {
					newTop = 0;
					aboutLabel.Top = newTop;
					aboutTimer.Stop();
				} else
					aboutLabel.Top = newTop;
			}
		}

		private void MenuStrip1_MouseUp(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Left && menuStrip1.ClientRectangle.Contains(e.Location) && menuStrip1.GetItemAt(e.X) == -1) {
				if (isAboutShown) {
					recess = true;
					aboutTimer.Start();
					isAboutShown = false;
				} else {
					recess = false;
					isAboutShown = true;
					aboutTimer.Start();
				}
			}
		}

		private void StyledImgMsgBox_MouseDown(object sender, MouseEventArgs e) {
			if (isAboutShown) {
				recess = true;
				aboutTimer.Start();
				isAboutShown = false;
			}
			if (background == null)
				return;
			if (e.Button == MouseButtons.Left) {
				isMouseDown = true;
				oldMouseLoc = e.Location;
			}
		}

		private void StyledImgMsgBox_MouseMove(object sender, MouseEventArgs e) {
			if (background == null)
				return;
			else if (isMouseDown) {
				float diffX = e.X - oldMouseLoc.X, diffY = e.Y - oldMouseLoc.Y;
				oldMouseLoc = e.Location;
				offset = new PointF(offset.X + diffX, offset.Y + diffY);
				cumulativeDiff = new PointF(cumulativeDiff.X - diffX * (zoom - 1f), cumulativeDiff.Y - diffY * (zoom - 1f));
				ClampOffset();
				Invalidate(false);
			}
		}

		private void StyledImgMsgBox_MouseUp(object sender, MouseEventArgs e) {
			if (background == null)
				return;
			else if (e.Button == MouseButtons.Left)
				isMouseDown = false;
		}

		private void StyledImgMsgBox_MouseWheel(object sender, MouseEventArgs e) {
			if (background == null)
				return;
			bool zoomIn = e.Delta >= 0;
			Point cursorPosition = e.Location;
			cursorPosition.Y -= menuStrip1.Height;
			for (int max = Math.Abs(e.Delta / SystemInformation.MouseWheelScrollDelta); max > 0; max--)
				ProcessMouseZoom(zoomIn, cursorPosition);
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
			viewSize.Height -= menuStrip1.Height;
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
			if (background == null)
				return;
			Size area = ViewSize;
			area.Height -= menuStrip1.Height;
			Size imageSize = background.Size;
			float nPercentW = area.Width * zoom / imageSize.Width;
			float nPercentH = area.Height * zoom / imageSize.Height;
			float maxX, maxY;
			if (nPercentH < nPercentW) {
				maxX = -(area.Width - (imageSize.Width * nPercentH)) * 0.5f;
				maxY = 0f;
			} else {
				maxX = 0f;
				maxY = -(area.Height - (imageSize.Height * nPercentW)) * 0.5f;
			}
			if (offset.X > maxX) {
				offset.X = maxX;
				cumulativeDiff.X = (area.Width - (area.Width * zoom)) * 0.5f - maxX;
			}
			if (offset.Y > maxY) {
				offset.Y = maxY;
				cumulativeDiff.Y = (area.Height - area.Height * zoom) * 0.5f - maxX;
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

		/// <summary>
		/// Called when the window is resized.
		/// </summary>
		/// <param name="e">Ignored.</param>
		protected override void OnResize(EventArgs e) {
			base.OnResize(e);
			if (aboutLabel != null)
				aboutLabel.Width = ViewSize.Width;
			ClampOffset();
		}

		/// <summary>
		/// Draws the window.
		/// </summary>
		/// <param name="g">The Graphics object to use to paint on.</param>
		/// <param name="clippingRect">The clipping rectangle that was invalidated.</param>
		protected override void OnPaint(Graphics g, Rectangle clippingRect) {
			if (background == null)
				return;
			g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
			g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			g.InterpolationMode = smoothStretching.Checked ? System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic : System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
			Size size = ViewSize;
			size.Width = (int) (size.Width * zoom);
			size.Height = (int) (size.Height * zoom) - menuStrip1.Height;
			PointF offset = this.offset;
			offset.Y += menuStrip1.Height;
			ImageLib.DrawImageWithLayout(g, background, new RectangleF(offset, size), ImageLayout.Zoom);
			base.OnPaint(g, clippingRect);
		}

		private void Open(string filename) {
			zoom = 1f;
			offset = PointF.Empty;
			cumulativeDiff = PointF.Empty;
			try {
				fileDialog.FileName = Path.GetFileName(filename);
				UpdateBackground(Extensions.ImageFromFile(filename));
				savedBackground = background;
				loadedPath = filename;
				Invalidate(false);
			} catch (Exception ex) {
				StyledMessageBox.Show("An error occurred while trying to open to the specified image:\n" + ex.Message, "Error", true, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void styledItem1_Click(object sender, EventArgs e) {
			fileDialog.Open = true;
			fileDialog.Title = "Choose an image file...";
			if (MessageLoop.ShowDialog(fileDialog, false) == DialogResult.OK)
				Open(fileDialog.FileName);
		}

		private void ScaleItem_Click(object sender, EventArgs e) {
			if (background == null)
				return;
			sliderDialog.Text = "Scale";
			sliderDialog.AllowValuesOutsideRange = true;
			sliderDialog.StyledSlider.Reset(0.1f, 4f, 1f, 0f);
			if (MessageLoop.ShowDialog(sliderDialog, false) == DialogResult.OK) {
				try {
					UpdateBackground(background.ConvertPixelFormat((int) (background.Width * sliderDialog.Value), (int) (background.Height * sliderDialog.Value), background.PixelFormat));
				} catch {
				}
			}
			sliderDialog.AllowValuesOutsideRange = false;
		}

		private void toolStripMenuItem1_Click(object sender, EventArgs e) {
			if (background == null)
				return;
			Bitmap newBitmap = background.FastCopy();
			newBitmap.Invert();
			UpdateBackground(newBitmap);
		}

		private void toolStripMenuItem2_Click(object sender, EventArgs e) {
			if (background == null)
				return;
			sliderDialog.Text = "Binarization";
			sliderDialog.StyledSlider.Reset(0, 255, 1, 1);
			if (MessageLoop.ShowDialog(sliderDialog, false) == DialogResult.OK) {
				Bitmap newBitmap = background.FastCopy();
				newBitmap.BitwiseAnd((byte) sliderDialog.Value);
				UpdateBackground(newBitmap);
			}
		}

		private void toolStripMenuItem3_Click(object sender, EventArgs e) {
			if (background == null)
				return;
			sliderDialog.Text = "Threshold";
			sliderDialog.StyledSlider.Reset(0, 255, 1, 1);
			if (MessageLoop.ShowDialog(sliderDialog, false) == DialogResult.OK) {
				Bitmap newBitmap = background.FastCopy();
				newBitmap.Threshold((byte) sliderDialog.Value);
				UpdateBackground(newBitmap);
			}
		}

		private void toolStripMenuItem4_Click(object sender, EventArgs e) {
			if (background == null)
				return;
			Bitmap newBitmap = background.FastCopy();
			newBitmap.Logarithm();
			UpdateBackground(newBitmap);
		}

		private void normalizeToolStripMenuItem_Click(object sender, EventArgs e) {
			if (background == null)
				return;
			Bitmap newBitmap = background.FastCopy();
			newBitmap.Normalize();
			UpdateBackground(newBitmap);
		}

		private void grayscaleToolStripMenuItem_Click(object sender, EventArgs e) {
			if (background != null)
				UpdateBackground(background.ToGrayscale());
		}

		private void Ungrayscale_Click(object sender, EventArgs e) {
			if (background != null)
				UpdateBackground(background.To32Bit());
		}

		private void equalizeMenuItem_Click(object sender, EventArgs e) {
			if (background == null)
				return;
			Bitmap newBitmap = background.FastCopy();
			newBitmap.EqualizeHistogram();
			UpdateBackground(newBitmap);
		}

		private void ditheredThresholdToolStripMenuItem_Click(object sender, EventArgs e) {
			if (background == null)
				return;
			sliderDialog.Text = "Threshold + Dither";
			sliderDialog.StyledSlider.Reset(0, 255, 1, 1);
			if (MessageLoop.ShowDialog(sliderDialog, false) == DialogResult.OK) {
				Bitmap newBitmap = background.FastCopy();
				newBitmap.ThresholdDither((byte) sliderDialog.Value);
				UpdateBackground(newBitmap);
			}
		}

		private void toolStripMenuItem5_Click(object sender, EventArgs e) {
			if (background == null)
				return;
			sliderDialog.Text = "Power";
			sliderDialog.AllowValuesOutsideRange = true;
			sliderDialog.StyledSlider.Reset(-2, 2, 0.7f, 0f);
			if (MessageLoop.ShowDialog(sliderDialog, false) == DialogResult.OK) {
				Bitmap newBitmap = background.FastCopy();
				newBitmap.RaiseBy(sliderDialog.Value);
				UpdateBackground(newBitmap);
			}
			sliderDialog.AllowValuesOutsideRange = false;
		}

		private void toolStripMenuItem6_Click(object sender, EventArgs e) {
			if (background == null)
				return;
			sliderDialog.Text = "Box Blur";
			sliderDialog.AllowValuesOutsideRange = true;
			sliderDialog.StyledSlider.Reset(0f, 10f, 0.7f, 0f);
			if (MessageLoop.ShowDialog(sliderDialog, false) == DialogResult.OK) {
				Bitmap newBitmap = background.FastCopy();
				newBitmap.BoxBlur(sliderDialog.Value);
				UpdateBackground(newBitmap);
			}
			sliderDialog.AllowValuesOutsideRange = false;
		}

		private void medianToolStripMenuItem_Click(object sender, EventArgs e) {
			if (background == null)
				return;
			sliderDialog.Text = "Median Filter";
			sliderDialog.AllowValuesOutsideRange = true;
			sliderDialog.StyledSlider.Reset(0f, 10f, 1f, 0f);
			if (MessageLoop.ShowDialog(sliderDialog, false) == DialogResult.OK)
				UpdateBackground(background.MedianFilter(sliderDialog.Value));
			sliderDialog.AllowValuesOutsideRange = false;
		}

		private void toolStripMenuItem7_Click(object sender, EventArgs e) {
			if (background == null)
				return;
			sliderDialog.Text = "Gaussian Blur Spatial";
			sliderDialog.AllowValuesOutsideRange = true;
			sliderDialog.StyledSlider.Reset(0f, 10f, 1f, 0f);
			if (MessageLoop.ShowDialog(sliderDialog, false) == DialogResult.OK) {
				Bitmap newBitmap = background.FastCopy();
				newBitmap.ApplyFilter(Filter.GaussianBlur, sliderDialog.Value);
				UpdateBackground(newBitmap);
			}
			sliderDialog.AllowValuesOutsideRange = false;
		}

		private void toolStripMenuItem8_Click(object sender, EventArgs e) {
			if (background == null)
				return;
			sliderDialog.Text = "Gaussian Blur Fourier";
			sliderDialog.AllowValuesOutsideRange = true;
			sliderDialog.StyledSlider.Reset(0f, 10f, 1f, 0f);
			if (MessageLoop.ShowDialog(sliderDialog, false) == DialogResult.OK)
				UpdateBackground(background.GaussianBlurFFT(sliderDialog.Value).ToBitmap());
			sliderDialog.AllowValuesOutsideRange = false;
		}

		private void sharpenToolStripMenuItem_Click(object sender, EventArgs e) {
			if (background == null)
				return;
			sliderDialog.Text = "Sharpen";
			sliderDialog.AllowValuesOutsideRange = true;
			sliderDialog.StyledSlider.Reset(0f, 10f, 1f, 0f);
			if (MessageLoop.ShowDialog(sliderDialog, false) == DialogResult.OK) {
				Bitmap newBitmap = background.FastCopy();
				newBitmap.ApplyFilter(Filter.Sharpen, sliderDialog.Value);
				UpdateBackground(newBitmap);
			}
			sliderDialog.AllowValuesOutsideRange = false;
		}

		private void unsharpMaskToolStripMenuItem_Click(object sender, EventArgs e) {
			if (background == null)
				return;
			sliderDialog.Text = "Unsharp Mask";
			sliderDialog.AllowValuesOutsideRange = true;
			sliderDialog.StyledSlider.Reset(0f, 10f, 1f, 0f);
			if (MessageLoop.ShowDialog(sliderDialog, false) == DialogResult.OK) {
				Bitmap newBitmap = background.FastCopy();
				newBitmap.ApplyFilter(Filter.UnsharpMask, sliderDialog.Value);
				UpdateBackground(newBitmap);
			}
			sliderDialog.AllowValuesOutsideRange = false;
		}

		private void brightnessToolStripMenuItem_Click(object sender, EventArgs e) {
			if (background == null)
				return;
			sliderDialog.Text = "Brightness";
			sliderDialog.AllowValuesOutsideRange = true;
			sliderDialog.StyledSlider.Reset(0f, 4f, 1f, 0f);
			if (MessageLoop.ShowDialog(sliderDialog, false) == DialogResult.OK) {
				Bitmap newBitmap = background.FastCopy();
				newBitmap.ChangeBrightness(sliderDialog.Value);
				UpdateBackground(newBitmap);
			}
			sliderDialog.AllowValuesOutsideRange = false;
		}

		private void contrastToolStripMenuItem_Click(object sender, EventArgs e) {
			if (background == null)
				return;
			sliderDialog.Text = "Contrast";
			sliderDialog.AllowValuesOutsideRange = true;
			sliderDialog.StyledSlider.Reset(0f, 1.5f, 0.5f, 0f);
			if (MessageLoop.ShowDialog(sliderDialog, false) == DialogResult.OK) {
				Bitmap newBitmap = background.FastCopy();
				newBitmap.ChangeContrast(sliderDialog.Value);
				UpdateBackground(newBitmap);
			}
			sliderDialog.AllowValuesOutsideRange = false;
		}

		private void gammaToolStripMenuItem_Click(object sender, EventArgs e) {
			if (background == null)
				return;
			sliderDialog.Text = "Gamma";
			sliderDialog.AllowValuesOutsideRange = true;
			sliderDialog.StyledSlider.Reset(0f, 10f, 1f, 0f);
			if (MessageLoop.ShowDialog(sliderDialog, false) == DialogResult.OK) {
				Bitmap newBitmap = background.FastCopy();
				newBitmap.ChangeGamma(sliderDialog.Value);
				UpdateBackground(newBitmap);
			}
			sliderDialog.AllowValuesOutsideRange = false;
		}

		private void lightnessToolStripMenuItem_Click(object sender, EventArgs e) {
			if (background == null)
				return;
			sliderDialog.Text = "Lightness";
			sliderDialog.AllowValuesOutsideRange = true;
			sliderDialog.StyledSlider.Reset(-255f, 255f, 0f, 1f);
			if (MessageLoop.ShowDialog(sliderDialog, false) == DialogResult.OK) {
				Bitmap newBitmap = background.FastCopy();
				newBitmap.ChangeLightness((int) sliderDialog.Value);
				UpdateBackground(newBitmap);
			}
			sliderDialog.AllowValuesOutsideRange = false;
		}

		private void dilateToolStripMenuItem_Click(object sender, EventArgs e) {
			if (background == null)
				return;
			sliderDialog.Text = "Dilate";
			sliderDialog.AllowValuesOutsideRange = true;
			sliderDialog.StyledSlider.Reset(0f, 20f, 0f, 0f);
			if (MessageLoop.ShowDialog(sliderDialog, false) == DialogResult.OK)
				UpdateBackground(background.Dilate((int) sliderDialog.Value));
			sliderDialog.AllowValuesOutsideRange = false;
		}

		private void erodeToolStripMenuItem_Click(object sender, EventArgs e) {
			if (background == null)
				return;
			sliderDialog.Text = "Erode";
			sliderDialog.AllowValuesOutsideRange = true;
			sliderDialog.StyledSlider.Reset(0f, 20f, 0f, 0f);
			if (MessageLoop.ShowDialog(sliderDialog, false) == DialogResult.OK)
				UpdateBackground(background.Erode((int) sliderDialog.Value));
			sliderDialog.AllowValuesOutsideRange = false;
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e) {
			if (background == null)
				return;
			sliderDialog.Text = "Open (Morphological)";
			sliderDialog.AllowValuesOutsideRange = true;
			sliderDialog.StyledSlider.Reset(0f, 20f, 0f, 0f);
			if (MessageLoop.ShowDialog(sliderDialog, false) == DialogResult.OK) {
				Bitmap newBitmap = background.FastCopy();
				newBitmap.OpenFilter((int) sliderDialog.Value);
				UpdateBackground(newBitmap);
			}
			sliderDialog.AllowValuesOutsideRange = false;
		}

		private void closeMorphologicalToolStripMenuItem_Click(object sender, EventArgs e) {
			if (background == null)
				return;
			sliderDialog.Text = "Close (Morphological)";
			sliderDialog.AllowValuesOutsideRange = true;
			sliderDialog.StyledSlider.Reset(0f, 20f, 0f, 0f);
			if (MessageLoop.ShowDialog(sliderDialog, false) == DialogResult.OK) {
				Bitmap newBitmap = background.FastCopy();
				newBitmap.CloseFilter((int) sliderDialog.Value);
				UpdateBackground(newBitmap);
			}
			sliderDialog.AllowValuesOutsideRange = false;
		}

		private void SDF_Click(object sender, EventArgs e) {
			if (background == null)
				return;
			sliderDialog.Text = "Signed Distance Field";
			sliderDialog.AllowValuesOutsideRange = true;
			sliderDialog.StyledSlider.Reset(0f, 20f, 0f, 0f);
			if (MessageLoop.ShowDialog(sliderDialog, false) == DialogResult.OK)
				UpdateBackground(background.SignedDistanceField((int) sliderDialog.Value));
			sliderDialog.AllowValuesOutsideRange = false;
		}

		private void saltAndPepperNoiseToolStripMenuItem_Click(object sender, EventArgs e) {
			if (background == null)
				return;
			sliderDialog.Text = "Salt and Pepper";
			sliderDialog.StyledSlider.Reset(0f, 1f, 0f, 0f);
			if (MessageLoop.ShowDialog(sliderDialog, false) == DialogResult.OK) {
				Bitmap newBitmap = background.FastCopy();
				newBitmap.SaltAndPepperNoise(sliderDialog.Value);
				UpdateBackground(newBitmap);
			}
		}

		private void sobelEdgeDetectionToolStripMenuItem_Click(object sender, EventArgs e) {
			if (background == null)
				return;
			Bitmap newBitmap = background.FastCopy();
			newBitmap.SobelEdgeFilter();
			UpdateBackground(newBitmap);
		}

		private void prewittEdgeDetectionToolStripMenuItem_Click(object sender, EventArgs e) {
			if (background == null)
				return;
			Bitmap newBitmap = background.FastCopy();
			newBitmap.PrewittEdgeFilter();
			UpdateBackground(newBitmap);
		}

		private void PremultiplyAlpha_Click(object sender, EventArgs e) {
			Bitmap newBitmap = background.FastCopy();
			newBitmap.PremultiplyAlpha();
			UpdateBackground(newBitmap);
		}

		private void xbr_Click(object sender, EventArgs e) {
			if (background == null)
				return;
			sliderDialog.Text = "Xbr Scale";
			sliderDialog.StyledSlider.Reset(1f, 4f, 1f, 1f);
			if (MessageLoop.ShowDialog(sliderDialog, false) == DialogResult.OK) {
				switch ((int) sliderDialog.Value) {
					case 2:
						UpdateBackground(background.ApplyXbr2());
						break;
					case 3:
						UpdateBackground(background.ApplyXbr3());
						break;
					case 4:
						UpdateBackground(background.ApplyXbr4());
						break;
				}
			}
		}

		private void UpdateBackground(Bitmap bitmap) {
			background4 = background3;
			background3 = background2;
			background2 = background;
			background = bitmap;
			Invalidate(false);
		}

		private void undoMenuItem_Click(object sender, EventArgs e) {
			if (background == null || background2 == null)
				return;
			background = background2;
			background2 = background3;
			background3 = background4;
			Invalidate(false);
		}

		private void MedianEnhance_Click(object sender, EventArgs e) {
			if (background == null)
				return;
			sliderDialog.Text = "Median Enhance";
			sliderDialog.AllowValuesOutsideRange = true;
			sliderDialog.StyledSlider.Reset(0f, 10f, 1f, 0f);
			if (MessageLoop.ShowDialog(sliderDialog, false) == DialogResult.OK)
				UpdateBackground(background.MedianEnhance(sliderDialog.Value));
			sliderDialog.AllowValuesOutsideRange = false;
		}

		private void HighPassFilter_Click(object sender, EventArgs e) {
			if (background == null)
				return;
			sliderDialog.Text = "High-Pass Filter";
			sliderDialog.AllowValuesOutsideRange = true;
			sliderDialog.StyledSlider.Reset(0f, 1f, 0.5f, 0f);
			string originalText = sliderDialog.Text;
			sliderDialog.Text = "Cutoff (Hz):";
			if (MessageLoop.ShowDialog(sliderDialog, false) == DialogResult.OK) {
				float freqSqr = sliderDialog.Value;
				using (FourierWorker fourier = new FourierWorker(background, false, true)) {
					fourier.ApplyFunctionToAllValues(value => {
						if (value.MagnitudeSquared >= freqSqr)
							return System.Numerics.ComplexF.Zero;
						else
							return value;
					});
					using (PixelWorker worker = fourier.ConvertToBitmapInPlace())
						UpdateBackground(worker.ToBitmap());
				}
			}
			sliderDialog.Text = originalText;
			sliderDialog.AllowValuesOutsideRange = false;
		}

		private void LowPassFilter_Click(object sender, EventArgs e) {
			if (background == null)
				return;
			sliderDialog.Text = "Low-Pass Filter";
			sliderDialog.AllowValuesOutsideRange = true;
			sliderDialog.StyledSlider.Reset(0f, 1f, 0.5f, 0f);
			string originalText = sliderDialog.Text;
			sliderDialog.Text = "Cutoff:";
			if (MessageLoop.ShowDialog(sliderDialog, false) == DialogResult.OK) {
				float freqSqr = sliderDialog.Value;
				using (FourierWorker fourier = new FourierWorker(background, false, true)) {
					fourier.ApplyFunctionToAllValues(value => {
						if (value.MagnitudeSquared < freqSqr)
							return System.Numerics.ComplexF.Zero;
						else
							return value;
					});
					using (PixelWorker worker = fourier.ConvertToBitmapInPlace())
						UpdateBackground(worker.ToBitmap());
				}
			}
			sliderDialog.Text = originalText;
			sliderDialog.AllowValuesOutsideRange = false;
		}

		private void AntialiasItem_Click(object sender, EventArgs e) {
			if (background == null)
				return;
			sliderDialog.Text = "Antialias";
			sliderDialog.StyledSlider.Reset(0f, 1f, 0.7f, 0f);
			if (MessageLoop.ShowDialog(sliderDialog, false) == DialogResult.OK) {
				Bitmap newBitmap = background.FastCopy();
				newBitmap.AntiAlias(sliderDialog.Value);
				UpdateBackground(newBitmap);
			}
		}

		private void SaveMenuItem_Click(object sender, EventArgs e) {
			if (background == null)
				return;
			else if (!Extensions.IsNullOrWhiteSpace(loadedPath)) {
				try {
					background.Save(loadedPath);
					savedBackground = background;
					return;
				} catch (Exception) {
				}
			}
			SaveAsMenuItem_Click(this, EventArgs.Empty);
		}

		private void SaveAsMenuItem_Click(object sender, EventArgs e) {
			if (background == null)
				return;
			fileDialog.Open = false;
			fileDialog.Title = "Choose where to save image file...";
			if (MessageLoop.ShowDialog(fileDialog, false) == DialogResult.OK) {
				try {
					background.Save(fileDialog.FileName);
					savedBackground = background;
				} catch (Exception ex) {
					StyledMessageBox.Show("An error occurred while trying to save the specified image:\n" + ex.Message, "Error", true, MessageBoxButtons.OK, MessageBoxIcon.Error);
					SaveAsMenuItem_Click(this, EventArgs.Empty);
				}
			}
		}

		private void SmoothStretching_CheckedChanged(object sender, EventArgs e) {
			Invalidate(false);
		}

		/// <summary>
		/// Called when the window is being closed.
		/// </summary>
		protected override bool OnQueryClose(CloseReason reason) {
			if (background != null && savedBackground != background) {
				switch (StyledMessageBox.Show("Do you want to save the current image?", "Confirm", true, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)) {
					case DialogResult.Yes:
						SaveMenuItem_Click(this, EventArgs.Empty);
						break;
					case DialogResult.Cancel:
						return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Called when the window is being disposed.
		/// </summary>
		/// <param name="e">Whether managed resources are about to be disposed.</param>
		protected override void OnDisposing(DisposeFormEventArgs e) {
			if (e.DisposeMode == DisposeOptions.FullDisposal)
				aboutTimer.Dispose();
		}

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PixelArtEffectorForm));
			this.menuStrip1 = new System.Windows.Forms.StyledMenuStrip();
			this.editToolStripMenuItem = new System.Windows.Forms.StyledItem();
			this.styledItem1 = new System.Windows.Forms.StyledItem();
			this.undoMenuItem = new System.Windows.Forms.StyledItem();
			this.scaleItem = new System.Windows.Forms.StyledItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.StyledItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.StyledItem();
			this.medianToolStripMenuItem = new System.Windows.Forms.StyledItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.StyledItem();
			this.equalizeMenuItem = new System.Windows.Forms.StyledItem();
			this.ditheredThresholdToolStripMenuItem = new System.Windows.Forms.StyledItem();
			this.toolStripMenuItem4 = new System.Windows.Forms.StyledItem();
			this.toolStripMenuItem5 = new System.Windows.Forms.StyledItem();
			this.toolStripMenuItem6 = new System.Windows.Forms.StyledItem();
			this.toolStripMenuItem7 = new System.Windows.Forms.StyledItem();
			this.toolStripMenuItem8 = new System.Windows.Forms.StyledItem();
			this.sharpenToolStripMenuItem = new System.Windows.Forms.StyledItem();
			this.unsharpMaskToolStripMenuItem = new System.Windows.Forms.StyledItem();
			this.brightnessToolStripMenuItem = new System.Windows.Forms.StyledItem();
			this.contrastToolStripMenuItem = new System.Windows.Forms.StyledItem();
			this.gammaToolStripMenuItem = new System.Windows.Forms.StyledItem();
			this.lightnessToolStripMenuItem = new System.Windows.Forms.StyledItem();
			this.dilateToolStripMenuItem = new System.Windows.Forms.StyledItem();
			this.erodeToolStripMenuItem = new System.Windows.Forms.StyledItem();
			this.openToolStripMenuItem = new System.Windows.Forms.StyledItem();
			this.closeMorphologicalToolStripMenuItem = new System.Windows.Forms.StyledItem();
			this.normalizeToolStripMenuItem = new System.Windows.Forms.StyledItem();
			this.saltAndPepperNoiseToolStripMenuItem = new System.Windows.Forms.StyledItem();
			this.grayscaleToolStripMenuItem = new System.Windows.Forms.StyledItem();
			this.ungrayscale = new System.Windows.Forms.StyledItem();
			this.signedDistanceField = new System.Windows.Forms.StyledItem();
			this.sobelEdgeDetectionToolStripMenuItem = new System.Windows.Forms.StyledItem();
			this.prewittEdgeDetectionToolStripMenuItem = new System.Windows.Forms.StyledItem();
			this.premultiplyAlpha = new System.Windows.Forms.StyledItem();
			this.antialiasItem = new System.Windows.Forms.StyledItem();
			this.medianEnhance = new System.Windows.Forms.StyledItem();
			this.xbrMenuItem = new System.Windows.Forms.StyledItem();
			this.saveMenuItem = new System.Windows.Forms.StyledItem();
			this.saveAsMenuItem = new System.Windows.Forms.StyledItem();
			this.smoothStretching = new System.Windows.Forms.StyledItem();
			this.fileDialog = new System.Windows.Forms.FilePrompt();
			this.contextMenuStrip1 = new System.Windows.Forms.StyledContextMenu(this.components);
			this.toolStripMenuItem9 = new System.Windows.Forms.StyledItem();
			this.sliderDialog = new System.Windows.Forms.SliderDialog(this.components);
			this.highPassFilter = new System.Windows.Forms.StyledItem();
			this.lowPassFilter = new System.Windows.Forms.StyledItem();
			this.menuStrip1.SuspendLayout();
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.AutoSize = false;
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.editToolStripMenuItem,
			this.smoothStretching});
			this.menuStrip1.Location = new System.Drawing.Point(8, 33);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(585, 25);
			this.menuStrip1.TabIndex = 4;
			this.menuStrip1.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.styledItem1,
			this.undoMenuItem,
			this.scaleItem,
			this.toolStripMenuItem1,
			this.toolStripMenuItem2,
			this.medianToolStripMenuItem,
			this.toolStripMenuItem3,
			this.equalizeMenuItem,
			this.ditheredThresholdToolStripMenuItem,
			this.toolStripMenuItem4,
			this.toolStripMenuItem5,
			this.highPassFilter,
			this.lowPassFilter,
			this.toolStripMenuItem6,
			this.toolStripMenuItem7,
			this.toolStripMenuItem8,
			this.sharpenToolStripMenuItem,
			this.unsharpMaskToolStripMenuItem,
			this.brightnessToolStripMenuItem,
			this.contrastToolStripMenuItem,
			this.gammaToolStripMenuItem,
			this.lightnessToolStripMenuItem,
			this.dilateToolStripMenuItem,
			this.erodeToolStripMenuItem,
			this.openToolStripMenuItem,
			this.closeMorphologicalToolStripMenuItem,
			this.normalizeToolStripMenuItem,
			this.saltAndPepperNoiseToolStripMenuItem,
			this.grayscaleToolStripMenuItem,
			this.ungrayscale,
			this.signedDistanceField,
			this.sobelEdgeDetectionToolStripMenuItem,
			this.prewittEdgeDetectionToolStripMenuItem,
			this.premultiplyAlpha,
			this.antialiasItem,
			this.medianEnhance,
			this.xbrMenuItem,
			this.saveMenuItem,
			this.saveAsMenuItem});
			this.editToolStripMenuItem.Icon = null;
			this.editToolStripMenuItem.Image = null;
			this.editToolStripMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.editToolStripMenuItem.Size = new System.Drawing.Size(36, 25);
			this.editToolStripMenuItem.Text = "Edit";
			this.editToolStripMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// styledItem1
			// 
			this.styledItem1.Icon = null;
			this.styledItem1.Image = null;
			this.styledItem1.MaximumSize = new System.Drawing.Size(0, 0);
			this.styledItem1.Name = "styledItem1";
			this.styledItem1.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.styledItem1.Size = new System.Drawing.Size(46, 25);
			this.styledItem1.Text = "Open";
			this.styledItem1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.styledItem1.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.styledItem1.Click += new System.EventHandler(this.styledItem1_Click);
			// 
			// undoMenuItem
			// 
			this.undoMenuItem.Icon = null;
			this.undoMenuItem.Image = null;
			this.undoMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.undoMenuItem.Name = "undoMenuItem";
			this.undoMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.undoMenuItem.Size = new System.Drawing.Size(122, 25);
			this.undoMenuItem.Text = "Undo (max 3 times)";
			this.undoMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.undoMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.undoMenuItem.Click += new System.EventHandler(this.undoMenuItem_Click);
			// 
			// scaleItem
			// 
			this.scaleItem.Icon = null;
			this.scaleItem.Image = null;
			this.scaleItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.scaleItem.Name = "scaleItem";
			this.scaleItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.scaleItem.Size = new System.Drawing.Size(44, 25);
			this.scaleItem.Text = "Scale";
			this.scaleItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.scaleItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.scaleItem.Click += new System.EventHandler(this.ScaleItem_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Icon = null;
			this.toolStripMenuItem1.Image = null;
			this.toolStripMenuItem1.MaximumSize = new System.Drawing.Size(0, 0);
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.toolStripMenuItem1.Size = new System.Drawing.Size(55, 25);
			this.toolStripMenuItem1.Text = "Negate";
			this.toolStripMenuItem1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.toolStripMenuItem1.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Icon = null;
			this.toolStripMenuItem2.Image = null;
			this.toolStripMenuItem2.MaximumSize = new System.Drawing.Size(0, 0);
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.toolStripMenuItem2.Size = new System.Drawing.Size(58, 25);
			this.toolStripMenuItem2.Text = "Binarize";
			this.toolStripMenuItem2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.toolStripMenuItem2.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
			// 
			// medianToolStripMenuItem
			// 
			this.medianToolStripMenuItem.Icon = null;
			this.medianToolStripMenuItem.Image = null;
			this.medianToolStripMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.medianToolStripMenuItem.Name = "medianToolStripMenuItem";
			this.medianToolStripMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.medianToolStripMenuItem.Size = new System.Drawing.Size(87, 25);
			this.medianToolStripMenuItem.Text = "Median Filter";
			this.medianToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.medianToolStripMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.medianToolStripMenuItem.Click += new System.EventHandler(this.medianToolStripMenuItem_Click);
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Icon = null;
			this.toolStripMenuItem3.Image = null;
			this.toolStripMenuItem3.MaximumSize = new System.Drawing.Size(0, 0);
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.toolStripMenuItem3.Size = new System.Drawing.Size(69, 25);
			this.toolStripMenuItem3.Text = "Threshold";
			this.toolStripMenuItem3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.toolStripMenuItem3.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
			// 
			// equalizeMenuItem
			// 
			this.equalizeMenuItem.Icon = null;
			this.equalizeMenuItem.Image = null;
			this.equalizeMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.equalizeMenuItem.Name = "equalizeMenuItem";
			this.equalizeMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.equalizeMenuItem.Size = new System.Drawing.Size(69, 25);
			this.equalizeMenuItem.Text = "Equalize";
			this.equalizeMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.equalizeMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.equalizeMenuItem.Click += new System.EventHandler(this.equalizeMenuItem_Click);
			// 
			// ditheredThresholdToolStripMenuItem
			// 
			this.ditheredThresholdToolStripMenuItem.Icon = null;
			this.ditheredThresholdToolStripMenuItem.Image = null;
			this.ditheredThresholdToolStripMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.ditheredThresholdToolStripMenuItem.Name = "ditheredThresholdToolStripMenuItem";
			this.ditheredThresholdToolStripMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.ditheredThresholdToolStripMenuItem.Size = new System.Drawing.Size(120, 25);
			this.ditheredThresholdToolStripMenuItem.Text = "Dithered Threshold";
			this.ditheredThresholdToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.ditheredThresholdToolStripMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.ditheredThresholdToolStripMenuItem.Click += new System.EventHandler(this.ditheredThresholdToolStripMenuItem_Click);
			// 
			// toolStripMenuItem4
			// 
			this.toolStripMenuItem4.Icon = null;
			this.toolStripMenuItem4.Image = null;
			this.toolStripMenuItem4.MaximumSize = new System.Drawing.Size(0, 0);
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			this.toolStripMenuItem4.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.toolStripMenuItem4.Size = new System.Drawing.Size(106, 25);
			this.toolStripMenuItem4.Text = "Apply Logarithm";
			this.toolStripMenuItem4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.toolStripMenuItem4.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
			// 
			// toolStripMenuItem5
			// 
			this.toolStripMenuItem5.Icon = null;
			this.toolStripMenuItem5.Image = null;
			this.toolStripMenuItem5.MaximumSize = new System.Drawing.Size(0, 0);
			this.toolStripMenuItem5.Name = "toolStripMenuItem5";
			this.toolStripMenuItem5.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.toolStripMenuItem5.Size = new System.Drawing.Size(98, 25);
			this.toolStripMenuItem5.Text = "Raise by Power";
			this.toolStripMenuItem5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.toolStripMenuItem5.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.toolStripMenuItem5.Click += new System.EventHandler(this.toolStripMenuItem5_Click);
			// 
			// toolStripMenuItem6
			// 
			this.toolStripMenuItem6.Icon = null;
			this.toolStripMenuItem6.Image = null;
			this.toolStripMenuItem6.MaximumSize = new System.Drawing.Size(0, 0);
			this.toolStripMenuItem6.Name = "toolStripMenuItem6";
			this.toolStripMenuItem6.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.toolStripMenuItem6.Size = new System.Drawing.Size(60, 25);
			this.toolStripMenuItem6.Text = "Box Blur";
			this.toolStripMenuItem6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.toolStripMenuItem6.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.toolStripMenuItem6.Click += new System.EventHandler(this.toolStripMenuItem6_Click);
			// 
			// toolStripMenuItem7
			// 
			this.toolStripMenuItem7.Icon = null;
			this.toolStripMenuItem7.Image = null;
			this.toolStripMenuItem7.MaximumSize = new System.Drawing.Size(0, 0);
			this.toolStripMenuItem7.Name = "toolStripMenuItem7";
			this.toolStripMenuItem7.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.toolStripMenuItem7.Size = new System.Drawing.Size(129, 25);
			this.toolStripMenuItem7.Text = "Gaussian Blur Spatial";
			this.toolStripMenuItem7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.toolStripMenuItem7.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.toolStripMenuItem7.Click += new System.EventHandler(this.toolStripMenuItem7_Click);
			// 
			// toolStripMenuItem8
			// 
			this.toolStripMenuItem8.Icon = null;
			this.toolStripMenuItem8.Image = null;
			this.toolStripMenuItem8.MaximumSize = new System.Drawing.Size(0, 0);
			this.toolStripMenuItem8.Name = "toolStripMenuItem8";
			this.toolStripMenuItem8.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.toolStripMenuItem8.Size = new System.Drawing.Size(130, 25);
			this.toolStripMenuItem8.Text = "Gaussian Blur Fourier";
			this.toolStripMenuItem8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.toolStripMenuItem8.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.toolStripMenuItem8.Click += new System.EventHandler(this.toolStripMenuItem8_Click);
			// 
			// sharpenToolStripMenuItem
			// 
			this.sharpenToolStripMenuItem.Icon = null;
			this.sharpenToolStripMenuItem.Image = null;
			this.sharpenToolStripMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.sharpenToolStripMenuItem.Name = "sharpenToolStripMenuItem";
			this.sharpenToolStripMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.sharpenToolStripMenuItem.Size = new System.Drawing.Size(60, 25);
			this.sharpenToolStripMenuItem.Text = "Sharpen";
			this.sharpenToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.sharpenToolStripMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.sharpenToolStripMenuItem.Click += new System.EventHandler(this.sharpenToolStripMenuItem_Click);
			// 
			// unsharpMaskToolStripMenuItem
			// 
			this.unsharpMaskToolStripMenuItem.Icon = null;
			this.unsharpMaskToolStripMenuItem.Image = null;
			this.unsharpMaskToolStripMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.unsharpMaskToolStripMenuItem.Name = "unsharpMaskToolStripMenuItem";
			this.unsharpMaskToolStripMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.unsharpMaskToolStripMenuItem.Size = new System.Drawing.Size(93, 25);
			this.unsharpMaskToolStripMenuItem.Text = "Unsharp Mask";
			this.unsharpMaskToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.unsharpMaskToolStripMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.unsharpMaskToolStripMenuItem.Click += new System.EventHandler(this.unsharpMaskToolStripMenuItem_Click);
			// 
			// brightnessToolStripMenuItem
			// 
			this.brightnessToolStripMenuItem.Icon = null;
			this.brightnessToolStripMenuItem.Image = null;
			this.brightnessToolStripMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.brightnessToolStripMenuItem.Name = "brightnessToolStripMenuItem";
			this.brightnessToolStripMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.brightnessToolStripMenuItem.Size = new System.Drawing.Size(72, 25);
			this.brightnessToolStripMenuItem.Text = "Brightness";
			this.brightnessToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.brightnessToolStripMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.brightnessToolStripMenuItem.Click += new System.EventHandler(this.brightnessToolStripMenuItem_Click);
			// 
			// contrastToolStripMenuItem
			// 
			this.contrastToolStripMenuItem.Icon = null;
			this.contrastToolStripMenuItem.Image = null;
			this.contrastToolStripMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.contrastToolStripMenuItem.Name = "contrastToolStripMenuItem";
			this.contrastToolStripMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.contrastToolStripMenuItem.Size = new System.Drawing.Size(62, 25);
			this.contrastToolStripMenuItem.Text = "Contrast";
			this.contrastToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.contrastToolStripMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.contrastToolStripMenuItem.Click += new System.EventHandler(this.contrastToolStripMenuItem_Click);
			// 
			// gammaToolStripMenuItem
			// 
			this.gammaToolStripMenuItem.Icon = null;
			this.gammaToolStripMenuItem.Image = null;
			this.gammaToolStripMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.gammaToolStripMenuItem.Name = "gammaToolStripMenuItem";
			this.gammaToolStripMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.gammaToolStripMenuItem.Size = new System.Drawing.Size(58, 25);
			this.gammaToolStripMenuItem.Text = "Gamma";
			this.gammaToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.gammaToolStripMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.gammaToolStripMenuItem.Click += new System.EventHandler(this.gammaToolStripMenuItem_Click);
			// 
			// lightnessToolStripMenuItem
			// 
			this.lightnessToolStripMenuItem.Icon = null;
			this.lightnessToolStripMenuItem.Image = null;
			this.lightnessToolStripMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.lightnessToolStripMenuItem.Name = "lightnessToolStripMenuItem";
			this.lightnessToolStripMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.lightnessToolStripMenuItem.Size = new System.Drawing.Size(67, 25);
			this.lightnessToolStripMenuItem.Text = "Lightness";
			this.lightnessToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lightnessToolStripMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.lightnessToolStripMenuItem.Click += new System.EventHandler(this.lightnessToolStripMenuItem_Click);
			// 
			// dilateToolStripMenuItem
			// 
			this.dilateToolStripMenuItem.Icon = null;
			this.dilateToolStripMenuItem.Image = null;
			this.dilateToolStripMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.dilateToolStripMenuItem.Name = "dilateToolStripMenuItem";
			this.dilateToolStripMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.dilateToolStripMenuItem.Size = new System.Drawing.Size(47, 25);
			this.dilateToolStripMenuItem.Text = "Dilate";
			this.dilateToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.dilateToolStripMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.dilateToolStripMenuItem.Click += new System.EventHandler(this.dilateToolStripMenuItem_Click);
			// 
			// erodeToolStripMenuItem
			// 
			this.erodeToolStripMenuItem.Icon = null;
			this.erodeToolStripMenuItem.Image = null;
			this.erodeToolStripMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.erodeToolStripMenuItem.Name = "erodeToolStripMenuItem";
			this.erodeToolStripMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.erodeToolStripMenuItem.Size = new System.Drawing.Size(47, 25);
			this.erodeToolStripMenuItem.Text = "Erode";
			this.erodeToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.erodeToolStripMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.erodeToolStripMenuItem.Click += new System.EventHandler(this.erodeToolStripMenuItem_Click);
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Icon = null;
			this.openToolStripMenuItem.Image = null;
			this.openToolStripMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.openToolStripMenuItem.Size = new System.Drawing.Size(136, 25);
			this.openToolStripMenuItem.Text = "Open (Morphological)";
			this.openToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.openToolStripMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// closeMorphologicalToolStripMenuItem
			// 
			this.closeMorphologicalToolStripMenuItem.Icon = null;
			this.closeMorphologicalToolStripMenuItem.Image = null;
			this.closeMorphologicalToolStripMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.closeMorphologicalToolStripMenuItem.Name = "closeMorphologicalToolStripMenuItem";
			this.closeMorphologicalToolStripMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.closeMorphologicalToolStripMenuItem.Size = new System.Drawing.Size(136, 25);
			this.closeMorphologicalToolStripMenuItem.Text = "Close (Morphological)";
			this.closeMorphologicalToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.closeMorphologicalToolStripMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.closeMorphologicalToolStripMenuItem.Click += new System.EventHandler(this.closeMorphologicalToolStripMenuItem_Click);
			// 
			// normalizeToolStripMenuItem
			// 
			this.normalizeToolStripMenuItem.Icon = null;
			this.normalizeToolStripMenuItem.Image = null;
			this.normalizeToolStripMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.normalizeToolStripMenuItem.Name = "normalizeToolStripMenuItem";
			this.normalizeToolStripMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.normalizeToolStripMenuItem.Size = new System.Drawing.Size(71, 25);
			this.normalizeToolStripMenuItem.Text = "Normalize";
			this.normalizeToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.normalizeToolStripMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.normalizeToolStripMenuItem.Click += new System.EventHandler(this.normalizeToolStripMenuItem_Click);
			// 
			// saltAndPepperNoiseToolStripMenuItem
			// 
			this.saltAndPepperNoiseToolStripMenuItem.Icon = null;
			this.saltAndPepperNoiseToolStripMenuItem.Image = null;
			this.saltAndPepperNoiseToolStripMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.saltAndPepperNoiseToolStripMenuItem.Name = "saltAndPepperNoiseToolStripMenuItem";
			this.saltAndPepperNoiseToolStripMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.saltAndPepperNoiseToolStripMenuItem.Size = new System.Drawing.Size(136, 25);
			this.saltAndPepperNoiseToolStripMenuItem.Text = "Salt and Pepper Noise";
			this.saltAndPepperNoiseToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.saltAndPepperNoiseToolStripMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.saltAndPepperNoiseToolStripMenuItem.Click += new System.EventHandler(this.saltAndPepperNoiseToolStripMenuItem_Click);
			// 
			// grayscaleToolStripMenuItem
			// 
			this.grayscaleToolStripMenuItem.Icon = null;
			this.grayscaleToolStripMenuItem.Image = null;
			this.grayscaleToolStripMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.grayscaleToolStripMenuItem.Name = "grayscaleToolStripMenuItem";
			this.grayscaleToolStripMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.grayscaleToolStripMenuItem.Size = new System.Drawing.Size(67, 25);
			this.grayscaleToolStripMenuItem.Text = "Grayscale";
			this.grayscaleToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.grayscaleToolStripMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.grayscaleToolStripMenuItem.Click += new System.EventHandler(this.grayscaleToolStripMenuItem_Click);
			// 
			// ungrayscale
			// 
			this.ungrayscale.Icon = null;
			this.ungrayscale.Image = null;
			this.ungrayscale.MaximumSize = new System.Drawing.Size(0, 0);
			this.ungrayscale.Name = "ungrayscale";
			this.ungrayscale.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.ungrayscale.Size = new System.Drawing.Size(176, 25);
			this.ungrayscale.Text = "Ungrayscale (will still be gray)";
			this.ungrayscale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.ungrayscale.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.ungrayscale.Click += new System.EventHandler(this.Ungrayscale_Click);
			// 
			// signedDistanceField
			// 
			this.signedDistanceField.Icon = null;
			this.signedDistanceField.Image = null;
			this.signedDistanceField.MaximumSize = new System.Drawing.Size(0, 0);
			this.signedDistanceField.Name = "signedDistanceField";
			this.signedDistanceField.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.signedDistanceField.Size = new System.Drawing.Size(132, 25);
			this.signedDistanceField.Text = "Signed Distance Field";
			this.signedDistanceField.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.signedDistanceField.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.signedDistanceField.Click += new System.EventHandler(this.SDF_Click);
			// 
			// sobelEdgeDetectionToolStripMenuItem
			// 
			this.sobelEdgeDetectionToolStripMenuItem.Icon = null;
			this.sobelEdgeDetectionToolStripMenuItem.Image = null;
			this.sobelEdgeDetectionToolStripMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.sobelEdgeDetectionToolStripMenuItem.Name = "sobelEdgeDetectionToolStripMenuItem";
			this.sobelEdgeDetectionToolStripMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.sobelEdgeDetectionToolStripMenuItem.Size = new System.Drawing.Size(133, 25);
			this.sobelEdgeDetectionToolStripMenuItem.Text = "Sobel Edge Detection";
			this.sobelEdgeDetectionToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.sobelEdgeDetectionToolStripMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.sobelEdgeDetectionToolStripMenuItem.Click += new System.EventHandler(this.sobelEdgeDetectionToolStripMenuItem_Click);
			// 
			// prewittEdgeDetectionToolStripMenuItem
			// 
			this.prewittEdgeDetectionToolStripMenuItem.Icon = null;
			this.prewittEdgeDetectionToolStripMenuItem.Image = null;
			this.prewittEdgeDetectionToolStripMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.prewittEdgeDetectionToolStripMenuItem.Name = "prewittEdgeDetectionToolStripMenuItem";
			this.prewittEdgeDetectionToolStripMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.prewittEdgeDetectionToolStripMenuItem.Size = new System.Drawing.Size(140, 25);
			this.prewittEdgeDetectionToolStripMenuItem.Text = "Prewitt Edge Detection";
			this.prewittEdgeDetectionToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.prewittEdgeDetectionToolStripMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.prewittEdgeDetectionToolStripMenuItem.Click += new System.EventHandler(this.prewittEdgeDetectionToolStripMenuItem_Click);
			// 
			// premultiplyAlpha
			// 
			this.premultiplyAlpha.Icon = null;
			this.premultiplyAlpha.Image = null;
			this.premultiplyAlpha.MaximumSize = new System.Drawing.Size(0, 0);
			this.premultiplyAlpha.Name = "premultiplyAlpha";
			this.premultiplyAlpha.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.premultiplyAlpha.Size = new System.Drawing.Size(112, 25);
			this.premultiplyAlpha.Text = "Premultiply Alpha";
			this.premultiplyAlpha.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.premultiplyAlpha.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.premultiplyAlpha.Click += new System.EventHandler(this.PremultiplyAlpha_Click);
			// 
			// antialiasItem
			// 
			this.antialiasItem.Icon = null;
			this.antialiasItem.Image = null;
			this.antialiasItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.antialiasItem.Name = "antialiasItem";
			this.antialiasItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.antialiasItem.Size = new System.Drawing.Size(61, 25);
			this.antialiasItem.Text = "Antialias";
			this.antialiasItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.antialiasItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.antialiasItem.Click += new System.EventHandler(this.AntialiasItem_Click);
			// 
			// medianEnhance
			// 
			this.medianEnhance.Icon = null;
			this.medianEnhance.Image = null;
			this.medianEnhance.MaximumSize = new System.Drawing.Size(0, 0);
			this.medianEnhance.Name = "medianEnhance";
			this.medianEnhance.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.medianEnhance.Size = new System.Drawing.Size(106, 25);
			this.medianEnhance.Text = "Median Enhance";
			this.medianEnhance.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.medianEnhance.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.medianEnhance.Click += new System.EventHandler(this.MedianEnhance_Click);
			// 
			// xbrMenuItem
			// 
			this.xbrMenuItem.Icon = null;
			this.xbrMenuItem.Image = null;
			this.xbrMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.xbrMenuItem.Name = "xbrMenuItem";
			this.xbrMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.xbrMenuItem.Size = new System.Drawing.Size(66, 25);
			this.xbrMenuItem.Text = "Xbr Scale";
			this.xbrMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.xbrMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.xbrMenuItem.Click += new System.EventHandler(this.xbr_Click);
			// 
			// saveMenuItem
			// 
			this.saveMenuItem.Icon = null;
			this.saveMenuItem.Image = null;
			this.saveMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.saveMenuItem.Name = "saveMenuItem";
			this.saveMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.saveMenuItem.Size = new System.Drawing.Size(57, 25);
			this.saveMenuItem.Text = "Save";
			this.saveMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.saveMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.saveMenuItem.Click += new System.EventHandler(this.SaveMenuItem_Click);
			// 
			// saveAsMenuItem
			// 
			this.saveAsMenuItem.Icon = null;
			this.saveAsMenuItem.Image = null;
			this.saveAsMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.saveAsMenuItem.Name = "saveAsMenuItem";
			this.saveAsMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.saveAsMenuItem.Size = new System.Drawing.Size(57, 25);
			this.saveAsMenuItem.Text = "Save As";
			this.saveAsMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.saveAsMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.saveAsMenuItem.Click += new System.EventHandler(this.SaveAsMenuItem_Click);
			// 
			// smoothStretching
			// 
			this.smoothStretching.Checked = true;
			this.smoothStretching.CheckOnClick = true;
			this.smoothStretching.CheckState = System.Windows.Forms.CheckState.Checked;
			this.smoothStretching.Icon = null;
			this.smoothStretching.Image = null;
			this.smoothStretching.MaximumSize = new System.Drawing.Size(0, 0);
			this.smoothStretching.Name = "smoothStretching";
			this.smoothStretching.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.smoothStretching.ShowCheckBox = true;
			this.smoothStretching.Size = new System.Drawing.Size(139, 25);
			this.smoothStretching.Text = "Smooth Stretching";
			this.smoothStretching.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.smoothStretching.CheckedChanged += new System.EventHandler(this.SmoothStretching_CheckedChanged);
			// 
			// fileDialog
			// 
			this.fileDialog.FileName = "Image.jpg";
			this.fileDialog.Title = "Choose an image...";
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.toolStripMenuItem9});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(153, 48);
			// 
			// toolStripMenuItem9
			// 
			this.toolStripMenuItem9.Icon = null;
			this.toolStripMenuItem9.Image = null;
			this.toolStripMenuItem9.MaximumSize = new System.Drawing.Size(0, 0);
			this.toolStripMenuItem9.Name = "toolStripMenuItem9";
			this.toolStripMenuItem9.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.toolStripMenuItem9.Size = new System.Drawing.Size(57, 25);
			this.toolStripMenuItem9.Text = "Save As";
			this.toolStripMenuItem9.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.toolStripMenuItem9.Click += new System.EventHandler(this.SaveAsMenuItem_Click);
			// 
			// sliderDialog
			// 
			this.sliderDialog.CausesValidation = false;
			this.sliderDialog.ClientSize = new System.Drawing.Size(342, 118);
			this.sliderDialog.Cursor = System.Windows.Forms.Cursors.Default;
			this.sliderDialog.EnableFullscreenOnAltEnter = false;
			this.sliderDialog.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.sliderDialog.InlineColor = System.Drawing.Color.FromArgb(((int) (((byte) (180)))), ((int) (((byte) (0)))), ((int) (((byte) (0)))), ((int) (((byte) (0)))));
			this.sliderDialog.KeyPreview = true;
			this.sliderDialog.Location = new System.Drawing.Point(62, 0);
			this.sliderDialog.MaximizeBox = false;
			this.sliderDialog.MaximizeEnabled = false;
			this.sliderDialog.MinimizeBox = false;
			this.sliderDialog.MinimizeEnabled = false;
			this.sliderDialog.MinimumSize = new System.Drawing.Size(200, 50);
			this.sliderDialog.Name = "sliderDialog";
			this.sliderDialog.OutlineColor = System.Drawing.Color.FromArgb(((int) (((byte) (180)))), ((int) (((byte) (0)))), ((int) (((byte) (0)))), ((int) (((byte) (139)))));
			this.sliderDialog.ShowIcon = false;
			this.sliderDialog.ShowInTaskbar = false;
			this.sliderDialog.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.sliderDialog.Text = "sliderDialog1";
			this.sliderDialog.Visible = false;
			this.sliderDialog.WindowCursor = System.Windows.Forms.Cursors.Default;
			// 
			// highPassFilter
			// 
			this.highPassFilter.Icon = null;
			this.highPassFilter.Image = null;
			this.highPassFilter.MaximumSize = new System.Drawing.Size(0, 0);
			this.highPassFilter.Name = "highPassFilter";
			this.highPassFilter.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.highPassFilter.Size = new System.Drawing.Size(101, 25);
			this.highPassFilter.Text = "High-Pass Filter";
			this.highPassFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.highPassFilter.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.highPassFilter.Click += new System.EventHandler(this.HighPassFilter_Click);
			// 
			// lowPassFilter
			// 
			this.lowPassFilter.Icon = null;
			this.lowPassFilter.Image = null;
			this.lowPassFilter.MaximumSize = new System.Drawing.Size(0, 0);
			this.lowPassFilter.Name = "lowPassFilter";
			this.lowPassFilter.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.lowPassFilter.Size = new System.Drawing.Size(96, 25);
			this.lowPassFilter.Text = "Low-Pass Filter";
			this.lowPassFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lowPassFilter.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.lowPassFilter.Click += new System.EventHandler(this.LowPassFilter_Click);
			// 
			// PixelArtEffectorForm
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.ClientSize = new System.Drawing.Size(601, 493);
			this.ContextMenuStrip = this.contextMenuStrip1;
			this.Controls.Add(this.menuStrip1);
			this.Font = new System.Drawing.Font("Calibri Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "PixelArtEffectorForm";
			this.Text = "Pixel Art Effector";
			this.TransparencyKey = System.Drawing.Color.Transparent;
			this.Controls.SetChildIndex(this.menuStrip1, 0);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
	}
}