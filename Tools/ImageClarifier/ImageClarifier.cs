using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Timers;
using System.Threading.Dispatch;
using System.Windows.Forms;

namespace ImageClarifier {
	using Timer = System.Timers.Timer;

	public class ImageClarifier : StyledForm {
		private readonly object loadedImageSyncRoot = new object(), resultantImageSyncRoot = new object();
		private DispatcherSlim dispatcher;
		private static string[] Args;
		private bool suppressRedraw, stopProcessing, stopSharpening, stopContrast, isProcessing, isSharpening, isContrast, isLeftDown, isRightDown, isLDOnLPicBox, isLDOnRPicBox, isRDOnLPicBox, isRDOnRPicBox, isLeftBoxFront, isAboutShown, recess, smoothStretching = true;
		private Bitmap loadedImage, blurredImage, clarifiedImage, resultantImage;
		private Point oldLocation;
		private InvocationData reprocess, sharpen, applycontrast;
		private StyledMenuStrip menuStrip;
		private FilePrompt filePrompt;
		private Rectangle leftPictureBox, rightPictureBox;
		private StyledItem openToolStripMenuItem, smoothStretchingToolStripMenuItem, restoreDefaultValuesToolStripMenuItem, saveAsToolStripMenuItem, modeToolStripMenuItem;
		private StyledSlider scaleSlider, smoothnessSlider, claritySlider, filterSlider, contrastSlider;
		private BufferedSplitContainer splitContainer;
		private StyledLabel aboutLabel, beforeLabel, afterLabel;
		private ToolStripMenuItem toolStripMenuItem1, toolStripMenuItem2, toolStripMenuItem3, toolStripMenuItem4, toolStripMenuItem5;
		private Timer aboutTimer;

		[STAThread]
		public static void Main(string[] args) {
#if NET45
			try {
				System.Runtime.ProfileOptimization.SetProfileRoot(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location));
				System.Runtime.ProfileOptimization.StartProfile("LoadCache.prf");
			} catch {
			}
#endif
			Args = args;
			MessageLoop.Run(new ImageClarifier());
		}
	
		public ImageClarifier() {
			CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            filePrompt.Filter = "Image Files|" + Extensions.ImageFileExtensions;
            beforeLabel = new StyledLabel() {
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Blur = 4,
                Font = new Font("Calibri Light", 12F),
                ShadowOpacity = 5f,
                RenderShadow = true,
                Text = "Before"
            };
            afterLabel = new StyledLabel() {
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Blur = 4,
                Font = new Font("Calibri Light", 12F),
                ShadowOpacity = 1.35f,
                RenderShadow = true,
                Text = "After"
            };
            reprocess = new InvocationData(ReProcess);
            sharpen = new InvocationData(Sharpen);
            applycontrast = new InvocationData(ApplyContrast);
            splitContainer.Panel1.Paint += Panel1_Paint;
            splitContainer.Resize += SplitContainer_Resize;
            splitContainer.Panel1.MouseDown += Panel1_MouseDown;
            splitContainer.Panel1.MouseMove += Panel1_MouseMove;
            splitContainer.Panel1.MouseUp += Panel1_MouseUp;
            scaleSlider.MouseDown += ScaleSlider_MouseDown;
            filterSlider.MouseDown += FilterSlider_MouseDown;
            smoothnessSlider.MouseDown += SmoothnessSlider_MouseDown;
            claritySlider.MouseDown += ClaritySlider_MouseDown;
            contrastSlider.MouseDown += ContrastSlider_MouseDown;
            menuStrip.MouseUp += MenuStrip_MouseUp;
            modeToolStripMenuItem.TextChanged += ModeToolStripMenuItem_TextChanged;
            dispatcher = new DispatcherSlim("ImageProcessor", true) {
                QueueCap = 2
            };
            scaleSlider.Label.Blur = 4;
            scaleSlider.Label.ForeColor = Color.White;
            scaleSlider.Label.Font = Font;
            scaleSlider.Label.RenderShadow = true;
            scaleSlider.Label.ShadowOpacity = 1.35F;
            scaleSlider.LabelAutoSize = false;
            smoothnessSlider.Label.Blur = 4;
            smoothnessSlider.Label.ForeColor = Color.White;
            smoothnessSlider.Label.Font = Font;
            smoothnessSlider.Label.RenderShadow = true;
            smoothnessSlider.Label.ShadowOpacity = 1.35F;
            smoothnessSlider.LabelAutoSize = true;
            claritySlider.Label.Blur = 4;
            claritySlider.Label.ForeColor = Color.White;
            claritySlider.Label.Font = Font;
            claritySlider.Label.RenderShadow = true;
            claritySlider.Label.ShadowOpacity = 1.35F;
            claritySlider.LabelAutoSize = false;
            filterSlider.Label.Blur = 4;
            filterSlider.Label.ForeColor = Color.White;
            filterSlider.Label.Font = Font;
            filterSlider.Label.RenderShadow = true;
            filterSlider.Label.ShadowOpacity = 1.35F;
            filterSlider.LabelAutoSize = false;
            contrastSlider.Label.Blur = 4;
            contrastSlider.Label.ForeColor = Color.White;
            contrastSlider.Label.Font = Font;
            contrastSlider.Label.RenderShadow = true;
            contrastSlider.Label.ShadowOpacity = 1.35F;
            contrastSlider.LabelAutoSize = false;
            scaleSlider.Label.Width = smoothnessSlider.Label.Width;
            claritySlider.Label.Width = smoothnessSlider.Label.Width;
            filterSlider.Label.Width = smoothnessSlider.Label.Width;
            contrastSlider.Label.Width = smoothnessSlider.Label.Width;
            splitContainer.SplitterMoved += SplitContainer_SplitterMoved;
			EnableResizeAnimation = false;
        }

		private void SplitContainer_SplitterMoved(object sender, SplitterEventArgs e) {
			int dist = splitContainer.SplitterDistance;
			int min = ViewSize.Height - (scaleSlider.Height * 5 + menuStrip.Height + splitContainer.SplitterWidth);
			if (dist < min)
				splitContainer.SplitterDistance = min;
		}

		private void ModeToolStripMenuItem_TextChanged(object sender, EventArgs e) {
			splitContainer.Panel1.Invalidate();
		}

		private void MenuStrip_MouseUp(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Left && menuStrip.ClientRectangle.Contains(e.Location) && menuStrip.GetItemAt(e.X) == -1) {
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

		protected override void OnShown(EventArgs e) {
			base.OnShown(e);
			SplitContainer_Resize(null, null);
			aboutTimer = new Timer(10.0);
			aboutTimer.Elapsed += AboutTimer_Elapsed;
			if (Args == null || Args.Length == 0)
				return;
			string filename = Args[0].Trim();
			Args = null;
			if (filename == string.Empty)
				return;
            LoadImage(filename);
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

		private void Panel1_MouseDown(object sender, MouseEventArgs e) {
			if (isAboutShown) {
				recess = true;
				aboutTimer.Start();
				isAboutShown = false;
			}
			if (loadedImage == null)
				return;
			if (e.Button == MouseButtons.Left) {
				if (isRightDown)
					return;
				isLeftDown = true;
				if (leftPictureBox.Contains(e.Location) && rightPictureBox.Contains(e.Location)) {
					Cursor = Cursors.SizeNWSE;
					if (isLeftBoxFront) {
						isLDOnLPicBox = true;
						isLeftBoxFront = true;
					} else {
						isLDOnRPicBox = true;
						isLeftBoxFront = false;
					}
				} else if (leftPictureBox.Contains(e.Location)) {
					Cursor = Cursors.SizeNWSE;
					isLDOnLPicBox = true;
					isLeftBoxFront = true;
				} else if (rightPictureBox.Contains(e.Location)) {
					Cursor = Cursors.SizeNWSE;
					isLDOnRPicBox = true;
					isLeftBoxFront = false;
				}
			} else if (e.Button == MouseButtons.Right) {
				isRightDown = true;
				if (leftPictureBox.Contains(e.Location) && rightPictureBox.Contains(e.Location)) {
					if (isLeftBoxFront) {
						Cursor = Cursors.SizeAll;
						isRDOnLPicBox = true;
						isLeftBoxFront = true;
					} else {
						Cursor = Cursors.SizeAll;
						isRDOnRPicBox = true;
						isLeftBoxFront = false;
					}
				} else if (leftPictureBox.Contains(e.Location)) {
					Cursor = Cursors.SizeAll;
					isRDOnLPicBox = true;
					isLeftBoxFront = true;
				} else if (rightPictureBox.Contains(e.Location)) {
					Cursor = Cursors.SizeAll;
					isRDOnRPicBox = true;
					isLeftBoxFront = false;
				}
			}
			oldLocation = Cursor.Position;
			splitContainer.Panel1.Invalidate();
		}

		private void Panel1_MouseMove(object sender, MouseEventArgs e) {
			if (isRDOnLPicBox) {
				leftPictureBox.X += Cursor.Position.X - oldLocation.X;
				leftPictureBox.Y += Cursor.Position.Y - oldLocation.Y;
				beforeLabel.Location = new Point(leftPictureBox.X + 10, leftPictureBox.Y + 10);
				splitContainer.Panel1.Invalidate();
			} else if (isLDOnLPicBox) {
				if (oldLocation.X < Cursor.Position.X || leftPictureBox.Width > 5)
					leftPictureBox.Width += Cursor.Position.X - oldLocation.X;
				if (oldLocation.Y < Cursor.Position.Y || leftPictureBox.Height > 5)
					leftPictureBox.Height += Cursor.Position.Y - oldLocation.Y;
				splitContainer.Panel1.Invalidate();
			} else if (isRDOnRPicBox) {
				rightPictureBox.X += Cursor.Position.X - oldLocation.X;
				rightPictureBox.Y += Cursor.Position.Y - oldLocation.Y;
				afterLabel.Location = new Point(rightPictureBox.X + 10, rightPictureBox.Y + 10);
				splitContainer.Panel1.Invalidate();
			} else if (isLDOnRPicBox) {
				if (oldLocation.X < Cursor.Position.X || rightPictureBox.Width > 5)
					rightPictureBox.Width += Cursor.Position.X - oldLocation.X;
				if (oldLocation.Y < Cursor.Position.Y || rightPictureBox.Height > 5)
					rightPictureBox.Height += Cursor.Position.Y - oldLocation.Y;
				splitContainer.Panel1.Invalidate();
			}
			oldLocation = Cursor.Position;
		}

		private void Panel1_MouseUp(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Left) {
				isLeftDown = false;
				isLDOnLPicBox = false;
				isLDOnRPicBox = false;
				if (!isRightDown)
					Cursor = Cursors.Default;
			} else if (e.Button == MouseButtons.Right) {
				if (isLeftDown) {
					if (isRDOnLPicBox || isRDOnRPicBox)
						Cursor = Cursors.SizeNWSE;
					else
						Cursor = Cursors.Default;
				} else
					Cursor = Cursors.Default;
				isRightDown = false;
				isRDOnLPicBox = false;
				isRDOnRPicBox = false;
			}
		}

		private void SplitContainer_Resize(object sender, EventArgs e) {
			int distance = splitContainer.Height - (scaleSlider.Height * 5 + 1);
			if (distance < 0)
				return;
			try {
				splitContainer.SplitterDistance = splitContainer.Height - (scaleSlider.Height * 5 + splitContainer.SplitterWidth);
				leftPictureBox = new Rectangle(0, 0, splitContainer.Width / 2, splitContainer.Panel1.Height);
				beforeLabel.Location = new Point(leftPictureBox.X + 10, leftPictureBox.Y + 10);
				rightPictureBox = new Rectangle(leftPictureBox.Width + 1, 0, splitContainer.Width + 1 - leftPictureBox.Width, leftPictureBox.Height);
				afterLabel.Location = new Point(rightPictureBox.X + 10, rightPictureBox.Y + 10);
				scaleSlider.Width = splitContainer.Width;
				filterSlider.Width = splitContainer.Width;
				smoothnessSlider.Width = splitContainer.Width;
				claritySlider.Width = splitContainer.Width;
				contrastSlider.Width = splitContainer.Width;
				aboutLabel.Width = splitContainer.Width;
				splitContainer.Panel1.Invalidate();
			} catch {
			}
		}

		private void Panel1_Paint(object sender, PaintEventArgs e) {
			e.Graphics.Clear(Color.Black);
			e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
			e.Graphics.InterpolationMode = smoothStretching ? InterpolationMode.HighQualityBicubic : InterpolationMode.NearestNeighbor;
			e.Graphics.SmoothingMode = SmoothingMode.None;
			e.Graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
			if (isLeftBoxFront) {
				RenderRightImage(e.Graphics);
				RenderLeftImage(e.Graphics);
			} else {
				RenderLeftImage(e.Graphics);
				RenderRightImage(e.Graphics);
			}
		}

		private void RenderLeftImage(Graphics g) {
			if (loadedImage == null)
				beforeLabel.DrawGdi(g);
			else {
				lock (loadedImageSyncRoot) {
					switch (modeToolStripMenuItem.Text) {
						case "Crop if smaller stretch if larger":
							ImageLib.DrawImageWithLayout(g, loadedImage, leftPictureBox, ImageLayout.None);
							break;
						case "Stretch":
							ImageLib.DrawImageWithLayout(g, loadedImage, leftPictureBox, ImageLayout.Stretch);
							break;
						case "Keep Aspect Ratio":
							ImageLib.DrawImageWithLayout(g, loadedImage, leftPictureBox, ImageLayout.Zoom);
							break;
						case "Tile":
							ImageLib.DrawImageWithLayout(g, loadedImage, leftPictureBox, ImageLayout.Tile);
							break;
						default:
							ImageLib.DrawImageWithLayout(g, loadedImage, leftPictureBox, ImageLayout.Center);
							break;
					}
				}
			}
			g.DrawRectangle(Pens.Blue, new Rectangle(leftPictureBox.X - 1, leftPictureBox.Y - 1, leftPictureBox.Width, leftPictureBox.Height));
		}

		private void RenderRightImage(Graphics g) {
			if (resultantImage == null)
				afterLabel.DrawGdi(g);
			else {
				lock (resultantImageSyncRoot) {
					switch (modeToolStripMenuItem.Text) {
						case "Crop if smaller stretch if larger":
							ImageLib.DrawImageWithLayout(g, resultantImage, rightPictureBox, ImageLayout.None);
							break;
						case "Stretch":
							ImageLib.DrawImageWithLayout(g, resultantImage, rightPictureBox, ImageLayout.Stretch);
							break;
						case "Keep Aspect Ratio":
							ImageLib.DrawImageWithLayout(g, resultantImage, rightPictureBox, ImageLayout.Zoom);
							break;
						case "Tile":
							ImageLib.DrawImageWithLayout(g, resultantImage, rightPictureBox, ImageLayout.Tile);
							break;
						default:
							ImageLib.DrawImageWithLayout(g, resultantImage, rightPictureBox, ImageLayout.Center);
							break;
					}
				}
			}
			g.DrawRectangle(Pens.Red, rightPictureBox.X - 1, rightPictureBox.Y - 1, rightPictureBox.Width, rightPictureBox.Height);
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e) {
			filePrompt.Open = true;
			filePrompt.Title = "Select an image to stretch and clarify...";
			if (MessageLoop.ShowDialog(filePrompt, false) == DialogResult.OK)
				LoadImage(filePrompt.FileName);
		}

		private void LoadImage(string filename) {
			if (isProcessing)
				stopProcessing = true;
			else
				Text = "Image Clarifier - Processing...";
			try {
				filePrompt.FileName = Path.GetFileName(filename);
				lock (loadedImageSyncRoot) {
					if (loadedImage != null)
						loadedImage.Dispose();
					loadedImage = Extensions.ImageFromFile(filename);
				}
			} catch {
				StyledMessageBox.Show("An error occured while trying to load the specified image.");
				Text = "Image Clarifier";
				return;
			}
			dispatcher.BeginInvoke(reprocess);
		}

		private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
			if (resultantImage == null)
				StyledMessageBox.Show("No image has been loaded yet.", "Error", true, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			else {
				filePrompt.Open = false;
				filePrompt.Title = "Save As...";
				if (MessageLoop.ShowDialog(filePrompt, false) == DialogResult.OK) {
					if (isProcessing)
						StyledMessageBox.Show("The resultant image is still processing. Please wait before saving.", "Information", true, MessageBoxButtons.OK, MessageBoxIcon.Warning);
					else {
						lock (resultantImageSyncRoot)
							resultantImage.Save(filePrompt.FileName);
						StyledMessageBox.Show("Saving was successful!", "Info", true, MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
				}
			}
		}

		private object ReProcess(object param) {
			if (loadedImage == null || suppressRedraw)
				return null;
			isProcessing = true;
			Bitmap temp, image;
			Size newSize;
			try {
				lock (loadedImageSyncRoot) {
					newSize = new Size((int) Math.Ceiling(loadedImage.Width * scaleSlider.Value), (int) Math.Ceiling(loadedImage.Height * scaleSlider.Value));
					if (blurredImage == null)
						blurredImage = ImageLib.Stretch(loadedImage, newSize);
					else {
						if (blurredImage.Size == newSize) {
							using (Graphics g = Graphics.FromImage(blurredImage)) {
								g.CompositingMode = CompositingMode.SourceCopy;
								g.InterpolationMode = InterpolationMode.HighQualityBicubic;
								g.PixelOffsetMode = PixelOffsetMode.HighQuality;
								ImageLib.DrawStretched(g, loadedImage, newSize);
							}
						} else {
							blurredImage.Dispose();
							blurredImage = ImageLib.Stretch(loadedImage, newSize);
						}
					}
					ImageLib.ApplyFilter(blurredImage, Filter.GaussianBlur, smoothnessSlider.Value, 1f);
					if (stopProcessing) {
						stopProcessing = false;
						return null;
					}
					image = ImageLib.ApplyXbr4(loadedImage);
				}
				if (stopProcessing) {
					image.Dispose();
					stopProcessing = false;
					return null;
				}
				ImageLib.ApplyFilter(image, Filter.GaussianBlur, smoothnessSlider.Value, 1f);
				if (stopProcessing) {
					image.Dispose();
					stopProcessing = false;
					return null;
				}
				float blend = (float) Math.Log(scaleSlider.Value, 4d);
				int iterations = (int) Math.Ceiling(blend);
				blend %= 1f;
				int i;
				for (i = 1; i < iterations - 1; i++) {
					temp = image;
					image = ImageLib.ApplyXbr4(temp);
					temp.Dispose();
					if (stopProcessing) {
						image.Dispose();
						stopProcessing = false;
						return null;
					}
					ImageLib.ApplyFilter(image, Filter.GaussianBlur, smoothnessSlider.Value, 1f);
					if (stopProcessing) {
						image.Dispose();
						stopProcessing = false;
						return null;
					}
				}
				if (i + 1 == iterations) {
					temp = ImageLib.ApplyXbr4(image);
					if (stopProcessing) {
						image.Dispose();
						temp.Dispose();
						stopProcessing = false;
						return null;
					}
					Bitmap currentImage = image;
					image = ImageLib.Stretch(currentImage, newSize);
					currentImage.Dispose();
					if (stopProcessing) {
						image.Dispose();
						temp.Dispose();
						stopProcessing = false;
						return null;
					}
					ImageLib.Transition(image, temp, blend);
					temp.Dispose();
					if (stopProcessing) {
						image.Dispose();
						stopProcessing = false;
						return null;
					}
					ImageLib.ApplyFilter(image, Filter.GaussianBlur, smoothnessSlider.Value, 1f);
					if (stopProcessing) {
						image.Dispose();
						stopProcessing = false;
						return null;
					}
				}
				Bitmap processedImage;
				if (image.Size == newSize)
					processedImage = image;
				else {
					processedImage = new Bitmap(newSize.Width, newSize.Height);
					using (Graphics canvas = Graphics.FromImage(processedImage)) {
						canvas.CompositingMode = CompositingMode.SourceCopy;
						canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
						canvas.PixelOffsetMode = PixelOffsetMode.HighQuality;
						canvas.DrawImage(image, ImageLib.ToDestPoints(Point.Empty, newSize), new Rectangle(Point.Empty, image.Size), GraphicsUnit.Pixel, ImageLib.TilingAttributes);
						image.Dispose();
					}
				}
				if (stopProcessing) {
					processedImage.Dispose();
					stopProcessing = false;
					return null;
				}
				ImageLib.Transition(blurredImage, processedImage, filterSlider.Value);
				processedImage.Dispose();
				if (stopProcessing || stopSharpening) {
					stopProcessing = false;
					stopSharpening = false;
					return null;
				}
			} catch {
			}
			return Sharpen(null);
		}

		private object Sharpen(object param) {
			if (blurredImage == null || suppressRedraw)
				return null;
			try {
				isProcessing = true;
				isSharpening = true;
				if (clarifiedImage == null)
					clarifiedImage = blurredImage.FastCopy();
				else {
					if (clarifiedImage.Size == blurredImage.Size) {
						using (Graphics g = Graphics.FromImage(clarifiedImage)) {
							g.CompositingMode = CompositingMode.SourceCopy;
							g.DrawImageUnscaled(blurredImage, 0, 0);
						}
					} else {
						clarifiedImage.Dispose();
						clarifiedImage = blurredImage.FastCopy();
					}
				}
				if (stopProcessing || stopSharpening) {
					stopProcessing = false;
					stopSharpening = false;
					return null;
				}
				ImageLib.ApplyFilter(clarifiedImage, Filter.UnsharpMask, claritySlider.Value, 1f);
				if (stopProcessing || stopSharpening || stopContrast) {
					stopProcessing = false;
					stopSharpening = false;
					stopContrast = false;
					return null;
				}
			} catch {
			}
			return ApplyContrast(null);
		}

		private object ApplyContrast(object param) {
			if (clarifiedImage == null || suppressRedraw)
				return null;
			isProcessing = true;
			try {
				lock (resultantImageSyncRoot) {
					isSharpening = true;
					isContrast = true;
					if (resultantImage == null)
						resultantImage = clarifiedImage.FastCopy();
					else {
						if (resultantImage.Size == clarifiedImage.Size) {
							using (Graphics g = Graphics.FromImage(resultantImage)) {
								g.CompositingMode = CompositingMode.SourceCopy;
								g.DrawImageUnscaled(clarifiedImage, 0, 0);
							}
						} else {
							resultantImage.Dispose();
							resultantImage = clarifiedImage.FastCopy();
						}
					}
					if (stopProcessing || stopSharpening || stopContrast) {
						stopProcessing = false;
						stopSharpening = false;
						stopContrast = false;
						return null;
					}
					ImageLib.ChangeContrast(resultantImage, contrastSlider.Value);
				}
			} catch {
			}
			isContrast = false;
			isSharpening = false;
			isProcessing = false;
			splitContainer.Panel1.Invalidate();
			Text = "Image Clarifier";
			return null;
		}

		private void processOnValueChange(object sender, EventArgs e) {
			if (isProcessing)
				stopProcessing = true;
			else if (loadedImage == null)
				return;
			else
				Text = "Image Clarifier - Processing...";
			dispatcher.BeginInvoke(reprocess);
		}

		private void sharpenOnValueChange(object sender, EventArgs e) {
			if (!isProcessing) {
				if (loadedImage == null)
					return;
				Text = "Image Clarifier - Processing...";
			} else if (isSharpening)
				stopSharpening = true;
			dispatcher.BeginInvoke(sharpen);
		}

		private void adjustContrastOnValueChange(object sender, EventArgs e) {
			if (!isProcessing) {
				if (loadedImage == null)
					return;
				Text = "Image Clarifier - Processing...";
			} else if (isContrast)
				stopContrast = true;
			dispatcher.BeginInvoke(applycontrast);
		}

		private void smoothStretchingToolStripMenuItem_Click(object sender, EventArgs e) {
			smoothStretching = !smoothStretching;
			splitContainer.Panel1.Invalidate();
		}

		private void restoreDefaultValuesToolStripMenuItem_Click(object sender, EventArgs e) {
			suppressRedraw = true;
			if (isProcessing)
				stopProcessing = true;
			else if (loadedImage != null)
				Text = "Image Clarifier - Processing...";
			scaleSlider.Value = 4f;
			filterSlider.Value = 0.95f;
			smoothnessSlider.Value = 0.25f;
			claritySlider.Value = 0.4f;
			contrastSlider.Value = 1f;
			suppressRedraw = false;
			dispatcher.BeginInvoke(reprocess);
		}

		private void ScaleSlider_MouseDown(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Right)
				scaleSlider.Value = 4f;
		}

		private void FilterSlider_MouseDown(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Right)
				filterSlider.Value = 0.95f;
		}

		private void SmoothnessSlider_MouseDown(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Right)
				smoothnessSlider.Value = 0.25f;
		}

		private void ClaritySlider_MouseDown(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Right)
				claritySlider.Value = 0.4f;
		}

		private void ContrastSlider_MouseDown(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Right)
				contrastSlider.Value = 1f;
		}

		protected override bool OnQueryClose(CloseReason reason) {
			stopProcessing = true;
			dispatcher.Dispose(false);
			return true;
		}

		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageClarifier));
			this.filePrompt = new System.Windows.Forms.FilePrompt();
			this.menuStrip = new System.Windows.Forms.StyledMenuStrip();
			this.openToolStripMenuItem = new System.Windows.Forms.StyledItem();
			this.saveAsToolStripMenuItem = new System.Windows.Forms.StyledItem();
			this.smoothStretchingToolStripMenuItem = new System.Windows.Forms.StyledItem();
			this.restoreDefaultValuesToolStripMenuItem = new System.Windows.Forms.StyledItem();
			this.modeToolStripMenuItem = new System.Windows.Forms.StyledItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
			this.splitContainer = new System.Windows.Forms.BufferedSplitContainer();
			this.aboutLabel = new System.Windows.Forms.StyledLabel();
			this.contrastSlider = new System.Windows.Forms.StyledSlider();
			this.claritySlider = new System.Windows.Forms.StyledSlider();
			this.scaleSlider = new System.Windows.Forms.StyledSlider();
			this.filterSlider = new System.Windows.Forms.StyledSlider();
			this.smoothnessSlider = new System.Windows.Forms.StyledSlider();
			this.menuStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
			this.splitContainer.Panel1.SuspendLayout();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			this.SuspendLayout();
			// 
			// filePrompt
			// 
			this.filePrompt.ActiveBorderOpacity = 0.75F;
			this.filePrompt.AllFilesString = "All Standard Images";
			this.filePrompt.AllowDrop = true;
			this.filePrompt.BackColorOpacity = ((byte)(255));
			this.filePrompt.BorderCursor = System.Windows.Forms.Cursors.Default;
			this.filePrompt.BorderWidth = 4;
			this.filePrompt.ButtonText = "Open";
			this.filePrompt.CausesValidation = false;
			this.filePrompt.ClientSize = new System.Drawing.Size(539, 518);
			this.filePrompt.EnableFullscreenOnAltEnter = false;
			this.filePrompt.FileName = "Image.png";
			this.filePrompt.FileNames = new string[] {
        "Image.png"};
			this.filePrompt.Filter = "";
			this.filePrompt.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.filePrompt.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.filePrompt.InactiveBorderOpacity = 0.5F;
			this.filePrompt.InlineColor = System.Drawing.Color.Black;
			this.filePrompt.KeyPreview = true;
			this.filePrompt.Location = new System.Drawing.Point(0, 0);
			this.filePrompt.MinimizeBox = false;
			this.filePrompt.MinimizeEnabled = false;
			this.filePrompt.MinimumSize = new System.Drawing.Size(200, 50);
			this.filePrompt.Name = "filePrompt";
			this.filePrompt.OutlineColor = System.Drawing.Color.Black;
			this.filePrompt.Padding = new System.Windows.Forms.Padding(4, 29, 4, 4);
			this.filePrompt.SelectDirectory = false;
			this.filePrompt.ShowIcon = false;
			this.filePrompt.ShowInTaskbar = false;
			this.filePrompt.ShowShadow = true;
			this.filePrompt.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.filePrompt.SystemMenu = null;
			this.filePrompt.SystemMenuStrip = null;
			this.filePrompt.Text = "Select an image to stretch and clarify...";
			this.filePrompt.TitleBarBadding = new System.Drawing.Size(0, 1);
			this.filePrompt.TitleBarHeight = 29;
			this.filePrompt.Visible = false;
			this.filePrompt.WarnOverwrite = false;
			this.filePrompt.WindowCursor = System.Windows.Forms.Cursors.Default;
			// 
			// menuStrip
			// 
			this.menuStrip.AutoSize = false;
			this.menuStrip.BackgroundImage = global::ImageClarifier.Properties.Resources.Cover;
			this.menuStrip.ForeColor = System.Drawing.Color.Black;
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.smoothStretchingToolStripMenuItem,
            this.restoreDefaultValuesToolStripMenuItem,
            this.modeToolStripMenuItem});
			this.menuStrip.Location = new System.Drawing.Point(4, 29);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new System.Drawing.Size(717, 24);
			this.menuStrip.TabIndex = 4;
			this.menuStrip.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.openToolStripMenuItem.Name = "StyledItem";
			this.openToolStripMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.openToolStripMenuItem.Size = new System.Drawing.Size(46, 25);
			this.openToolStripMenuItem.Text = "Open";
			this.openToolStripMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// saveAsToolStripMenuItem
			// 
			this.saveAsToolStripMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.saveAsToolStripMenuItem.Name = "StyledItem";
			this.saveAsToolStripMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(57, 25);
			this.saveAsToolStripMenuItem.Text = "Save As";
			this.saveAsToolStripMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
			// 
			// smoothStretchingToolStripMenuItem
			// 
			this.smoothStretchingToolStripMenuItem.Checked = true;
			this.smoothStretchingToolStripMenuItem.CheckOnClick = true;
			this.smoothStretchingToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.smoothStretchingToolStripMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.smoothStretchingToolStripMenuItem.Name = "StyledItem";
			this.smoothStretchingToolStripMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.smoothStretchingToolStripMenuItem.ShowCheckBox = true;
			this.smoothStretchingToolStripMenuItem.Size = new System.Drawing.Size(139, 25);
			this.smoothStretchingToolStripMenuItem.Text = "Smooth Stretching";
			this.smoothStretchingToolStripMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.smoothStretchingToolStripMenuItem.Click += new System.EventHandler(this.smoothStretchingToolStripMenuItem_Click);
			// 
			// restoreDefaultValuesToolStripMenuItem
			// 
			this.restoreDefaultValuesToolStripMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.restoreDefaultValuesToolStripMenuItem.Name = "StyledItem";
			this.restoreDefaultValuesToolStripMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.restoreDefaultValuesToolStripMenuItem.Size = new System.Drawing.Size(139, 25);
			this.restoreDefaultValuesToolStripMenuItem.Text = "Restore Default Values";
			this.restoreDefaultValuesToolStripMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.restoreDefaultValuesToolStripMenuItem.Click += new System.EventHandler(this.restoreDefaultValuesToolStripMenuItem_Click);
			// 
			// modeToolStripMenuItem
			// 
			this.modeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem5});
			this.modeToolStripMenuItem.IsComboBoxItem = true;
			this.modeToolStripMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.modeToolStripMenuItem.Name = "StyledItem";
			this.modeToolStripMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.modeToolStripMenuItem.Size = new System.Drawing.Size(194, 25);
			this.modeToolStripMenuItem.Text = "Crop if smaller stretch if larger";
			this.modeToolStripMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(233, 22);
			this.toolStripMenuItem1.Text = "Crop if smaller stretch if larger";
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(233, 22);
			this.toolStripMenuItem2.Text = "Stretch";
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(233, 22);
			this.toolStripMenuItem3.Text = "Keep Aspect Ratio";
			// 
			// toolStripMenuItem4
			// 
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			this.toolStripMenuItem4.Size = new System.Drawing.Size(233, 22);
			this.toolStripMenuItem4.Text = "Tile";
			// 
			// toolStripMenuItem5
			// 
			this.toolStripMenuItem5.Name = "toolStripMenuItem5";
			this.toolStripMenuItem5.Size = new System.Drawing.Size(233, 22);
			this.toolStripMenuItem5.Text = "Center";
			// 
			// splitContainer
			// 
			this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer.Location = new System.Drawing.Point(4, 53);
			this.splitContainer.Name = "splitContainer";
			this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer.Panel1
			// 
			this.splitContainer.Panel1.Controls.Add(this.aboutLabel);
			this.splitContainer.Panel1MinSize = 0;
			// 
			// splitContainer.Panel2
			// 
			this.splitContainer.Panel2.BackgroundImage = global::ImageClarifier.Properties.Resources.Cover;
			this.splitContainer.Panel2.Controls.Add(this.contrastSlider);
			this.splitContainer.Panel2.Controls.Add(this.claritySlider);
			this.splitContainer.Panel2.Controls.Add(this.scaleSlider);
			this.splitContainer.Panel2.Controls.Add(this.filterSlider);
			this.splitContainer.Panel2.Controls.Add(this.smoothnessSlider);
			this.splitContainer.Panel2MinSize = 0;
			this.splitContainer.Size = new System.Drawing.Size(717, 555);
			this.splitContainer.SplitterDistance = 385;
			this.splitContainer.TabIndex = 12;
			// 
			// aboutLabel
			// 
			this.aboutLabel.BackColor = System.Drawing.Color.Transparent;
			this.aboutLabel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Tile;
			this.aboutLabel.Blur = 4;
			this.aboutLabel.Font = new System.Drawing.Font("Calibri Light", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.aboutLabel.ForeColor = System.Drawing.Color.White;
			this.aboutLabel.Location = new System.Drawing.Point(0, -255);
			this.aboutLabel.Margin = new System.Windows.Forms.Padding(0);
			this.aboutLabel.Name = "aboutLabel";
			this.aboutLabel.RenderShadow = true;
			this.aboutLabel.ShadowOpacity = 1.35F;
			this.aboutLabel.Size = new System.Drawing.Size(695, 255);
			this.aboutLabel.TabIndex = 14;
			this.aboutLabel.Text = resources.GetString("aboutLabel.Text");
			this.aboutLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// contrastSlider
			// 
			this.contrastSlider.BackColor = System.Drawing.Color.Transparent;
			this.contrastSlider.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.contrastSlider.Increment = 0F;
			this.contrastSlider.LabelPadding = 2;
			this.contrastSlider.LargeChange = 0.25F;
			this.contrastSlider.Location = new System.Drawing.Point(0, 120);
			this.contrastSlider.Margin = new System.Windows.Forms.Padding(0);
			this.contrastSlider.Maximum = 1.5F;
			this.contrastSlider.Minimum = 0.85F;
			this.contrastSlider.MouseWheelBarPartitions = 8;
			this.contrastSlider.Name = "contrastSlider";
			this.contrastSlider.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
			this.contrastSlider.Size = new System.Drawing.Size(692, 30);
			this.contrastSlider.SmallChange = 0.1F;
			this.contrastSlider.TabIndex = 14;
			this.contrastSlider.Text = "5. Contrast:";
			this.contrastSlider.Value = 1F;
			this.contrastSlider.ValueChanged += new System.EventHandler(this.adjustContrastOnValueChange);
			// 
			// claritySlider
			// 
			this.claritySlider.BackColor = System.Drawing.Color.Transparent;
			this.claritySlider.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.claritySlider.Increment = 0F;
			this.claritySlider.LabelPadding = 2;
			this.claritySlider.LargeChange = 2F;
			this.claritySlider.Location = new System.Drawing.Point(0, 90);
			this.claritySlider.Margin = new System.Windows.Forms.Padding(0);
			this.claritySlider.Maximum = 64F;
			this.claritySlider.Name = "claritySlider";
			this.claritySlider.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
			this.claritySlider.Size = new System.Drawing.Size(692, 30);
			this.claritySlider.SmallChange = 5F;
			this.claritySlider.TabIndex = 7;
			this.claritySlider.Text = "4. Clarity:";
			this.claritySlider.Value = 0.4F;
			this.claritySlider.ValueChanged += new System.EventHandler(this.sharpenOnValueChange);
			// 
			// scaleSlider
			// 
			this.scaleSlider.BackColor = System.Drawing.Color.Transparent;
			this.scaleSlider.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.scaleSlider.Increment = 0F;
			this.scaleSlider.LabelPadding = 2;
			this.scaleSlider.LargeChange = 2F;
			this.scaleSlider.Location = new System.Drawing.Point(0, 0);
			this.scaleSlider.Margin = new System.Windows.Forms.Padding(0);
			this.scaleSlider.Maximum = 64F;
			this.scaleSlider.Minimum = 0.1F;
			this.scaleSlider.MouseWheelBarPartitions = 50;
			this.scaleSlider.Name = "scaleSlider";
			this.scaleSlider.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
			this.scaleSlider.Size = new System.Drawing.Size(692, 30);
			this.scaleSlider.SmallChange = 0.5F;
			this.scaleSlider.TabIndex = 12;
			this.scaleSlider.Text = "1. Scale:";
			this.scaleSlider.Value = 4F;
			this.scaleSlider.ValueChanged += new System.EventHandler(this.processOnValueChange);
			// 
			// filterSlider
			// 
			this.filterSlider.BackColor = System.Drawing.Color.Transparent;
			this.filterSlider.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.filterSlider.Increment = 0F;
			this.filterSlider.LabelPadding = 2;
			this.filterSlider.LargeChange = 0.2F;
			this.filterSlider.Location = new System.Drawing.Point(0, 30);
			this.filterSlider.Margin = new System.Windows.Forms.Padding(0);
			this.filterSlider.Maximum = 1F;
			this.filterSlider.Name = "filterSlider";
			this.filterSlider.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
			this.filterSlider.Size = new System.Drawing.Size(692, 30);
			this.filterSlider.SmallChange = 0.1F;
			this.filterSlider.TabIndex = 10;
			this.filterSlider.Text = "2. Scaling Filter:";
			this.filterSlider.Value = 0.95F;
			this.filterSlider.ValueChanged += new System.EventHandler(this.processOnValueChange);
			// 
			// smoothnessSlider
			// 
			this.smoothnessSlider.BackColor = System.Drawing.Color.Transparent;
			this.smoothnessSlider.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.smoothnessSlider.Increment = 0F;
			this.smoothnessSlider.LabelPadding = 2;
			this.smoothnessSlider.LargeChange = 0.5F;
			this.smoothnessSlider.Location = new System.Drawing.Point(0, 60);
			this.smoothnessSlider.Margin = new System.Windows.Forms.Padding(0);
			this.smoothnessSlider.Maximum = 32F;
			this.smoothnessSlider.Name = "smoothnessSlider";
			this.smoothnessSlider.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
			this.smoothnessSlider.Size = new System.Drawing.Size(692, 30);
			this.smoothnessSlider.SmallChange = 0.2F;
			this.smoothnessSlider.TabIndex = 5;
			this.smoothnessSlider.Text = "3. Smoothening:";
			this.smoothnessSlider.Value = 0.25F;
			this.smoothnessSlider.ValueChanged += new System.EventHandler(this.processOnValueChange);
			// 
			// ImageClarifier
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Black;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.ClientSize = new System.Drawing.Size(725, 612);
			this.Controls.Add(this.splitContainer);
			this.Controls.Add(this.menuStrip);
			this.EnableAeroBlur = false;
			this.Font = new System.Drawing.Font("Calibri Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip;
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MinimumSize = new System.Drawing.Size(350, 350);
			this.Name = "ImageClarifier";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Image Clarifier";
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.splitContainer.Panel1.ResumeLayout(false);
			this.splitContainer.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
			this.splitContainer.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		protected override void OnDisposed(DisposeFormEventArgs e) {
			if (e.DisposeMode == DisposeOptions.FullDisposal) {
				if (loadedImage != null) {
					loadedImage.Dispose();
					loadedImage = null;
				}
				if (blurredImage != null) {
					blurredImage.Dispose();
					blurredImage = null;
				}
				if (clarifiedImage != null) {
					clarifiedImage.Dispose();
					clarifiedImage = null;
				}
				if (resultantImage != null) {
					resultantImage.Dispose();
					resultantImage = null;
				}
			}
			base.OnDisposed(e);
		}
	}
}
 