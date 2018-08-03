using System;
using System.Drawing;
using System.Graphics.Models;
using System.Graphics.OGL;
using System.Numerics;
using System.Threading.Dispatch;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;

namespace Particles {
	public class ParticlesDemo : GraphicsForm {
		public static BgraColor EdgeColor = BgraColor.Red, IgnoreColor = BgraColor.Black;
		public const float threshold = 1000f;
		public const int edgeTolerance = 25, borderOffset = 15, noiseRemovalRadius = 12, significantCloseEdgeCount = 6;
		private object SyncRoot = new object(), bitmapSyncRoot = new object();
		internal int widthMinusOne, heightMinusOne, widthMinusBorderOffset, heightMinusBorderOffset;
		private static VideoCaptureDevice webcam;
		private Texture2D texture;
		private BgraColor[] resultant;
		private Vector2 webcamSize;
		private int startI, endI;
		internal int webcamWidth, webcamHeight;
		internal bool[][] isEdge, edgeBuffer, ignore;
		private Bitmap bitmap;
		private MeshComponent webcamMesh;
		private ParticleManager Particles;
		private bool takeSnapshot, showEdges = true;
		private Action<int> removeNoise;
		private Action<int, object> checkPixel;
		private Func<object, object> updateTexture;
		private static FilterInfoCollection devices;
		private static float[] xRows = new float[] { -1f, 0f, 1f };
		private static float[][] xMult = new float[][] { xRows, new float[] { -2f, 0f, 2f }, xRows };
		private static float[][] yMult = new float[][] { new float[] { -1f, -2f, -1f }, new float[] { 0f, 0f, 0f }, new float[] { 1f, 2f, 1f } };

		public ParticlesDemo() {
			updateTexture = UpdateTexture;
			InitializeComponent();
			BackColor = (Color) IgnoreColor;
			EnableFullscreenOnAltEnter = true;
			UpdateInterval = 30;
			checkPixel = CheckPixel;
			removeNoise = RemoveNoise;
			webcam.NewFrame += Webcam_NewFrame;
			Size size = webcam.VideoResolution.FrameSize;
			webcamWidth = size.Width;
			webcamHeight = size.Height;
			webcamSize = new Vector2(webcamWidth, webcamHeight);
			texture = new Texture2D(webcamWidth, webcamHeight, true, false);
			bitmap = new Bitmap(webcamWidth, webcamHeight);
			widthMinusOne = webcamWidth - 1;
			heightMinusOne = webcamHeight - 1;
			widthMinusBorderOffset = webcamWidth - borderOffset;
			heightMinusBorderOffset = webcamHeight - borderOffset;
			startI = borderOffset * (webcamWidth + 1);
			endI = (webcamHeight - borderOffset) * webcamWidth - borderOffset;
			resultant = new BgraColor[webcamWidth * webcamHeight];
			isEdge = new bool[webcamWidth][];
			for (int i = 0; i < webcamWidth; i++)
				isEdge[i] = new bool[webcamHeight];
			edgeBuffer = new bool[webcamWidth][];
			for (int i = 0; i < webcamWidth; i++)
				edgeBuffer[i] = new bool[webcamHeight];
			ignore = new bool[webcamWidth][];
			for (int i = 0; i < webcamWidth; i++)
				ignore[i] = new bool[webcamHeight];
			int x, y;
			for (y = 0; y < borderOffset; y++) {
				for (x = 0; x < webcamWidth; x++)
					ignore[x][y] = true;
			}
			for (y = heightMinusBorderOffset; y < webcamHeight; y++) {
				for (x = 0; x < webcamWidth; x++)
					ignore[x][y] = true;
			}
			for (x = 0; x < borderOffset; x++) {
				for (y = 0; y < webcamHeight; y++)
					ignore[x][y] = true;
			}
			for (x = widthMinusBorderOffset; x < webcamWidth; x++) {
				for (y = 0; y < webcamHeight; y++)
					ignore[x][y] = true;
			}
			for (int i = 0; i < resultant.Length; i++)
				resultant[i] = IgnoreColor;
			Particles = new ParticleManager(this);
			InitializeGL(true);
		}

		protected override void OnShown(EventArgs e) {
			base.OnShown(e);
			webcam.Start();
			UpdateTimerRunning = true;
			MouseUp += ParticlesDemo_MouseUp;
		}

		private void Webcam_NewFrame(object sender, NewFrameEventArgs eventArgs) {
			BgraColor[] colors = eventArgs.Frame.GetAllPixelsBgra();
			ParallelLoop.For(startI, endI, 1, new PixelCanvas(colors, edgeBuffer, false), checkPixel);
			lock (SyncRoot)
				ParallelLoop.For(startI, endI, removeNoise);
			if (takeSnapshot) {
				TakeSnapshotToIgnore(colors);
				takeSnapshot = false;
			}
			UpdateWebcamCanvas();
		}

		private void TakeSnapshotToIgnore(BgraColor[] colors) {
			for (int y = 0; y < borderOffset; y++) {
				for (int x = 0; x < webcamWidth; x++)
					ignore[x][y] = true;
			}
			for (int y = heightMinusBorderOffset; y < webcamHeight; y++) {
				for (int x = 0; x < webcamWidth; x++)
					ignore[x][y] = true;
			}
			for (int x = 0; x < borderOffset; x++) {
				for (int y = 0; y < webcamHeight; y++)
					ignore[x][y] = true;
			}
			for (int x = widthMinusBorderOffset; x < webcamWidth; x++) {
				for (int y = 0; y < webcamHeight; y++)
					ignore[x][y] = true;
			}
			for (int y = borderOffset; y < heightMinusBorderOffset; y++) {
				for (int x = borderOffset; x < widthMinusBorderOffset; x++)
					ignore[x][y] = false;
			}
			PixelCanvas parameter = new PixelCanvas(colors, ignore, false);
			ParallelLoop.For(startI, endI, delegate (int i) {
				resultant[i] = IgnoreColor;
				CheckPixel(i, parameter);
				int x = i % webcamWidth;
				int y = i / webcamWidth;
				if (ignore[x][y]) {
					int tempX, tempY, xOffset;
					for (int yOffset = -edgeTolerance; yOffset < edgeTolerance; yOffset++) {
						tempY = y + yOffset;
						if (tempY >= borderOffset && tempY < heightMinusBorderOffset) {
							for (xOffset = -edgeTolerance; xOffset < edgeTolerance; xOffset++) {
								tempX = x + xOffset;
								if (tempX >= borderOffset && tempX < widthMinusBorderOffset)
									ignore[tempX][tempY] = true;
							}
						}
					}
				}
			});
		}

		protected override void OnKeyUp(KeyEventArgs e) {
			base.OnKeyUp(e);
			if (e.KeyCode == Keys.Space)
				takeSnapshot = true;
		}

		private void UpdateWebcamCanvas() {
			if (showEdges) {
				lock (bitmapSyncRoot) {
					bitmap.SetAllPixels(resultant);
					if (IsGdiEnabled)
						InvalidateGdi();
					else
						InvokeOnGLThreadSync(new InvocationData(updateTexture, bitmap));
				}
			}
		}

		private void ParticlesDemo_MouseUp(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Right) {
				showEdges = !showEdges;
				if (!showEdges) {
					lock (SyncRoot) {
						for (int i = 0; i < resultant.Length; i++)
							resultant[i] = IgnoreColor;
						UpdateWebcamCanvas();
					}
				}
			}
		}

		protected override void OnUpdate(double elapsedMilliseconds) {
			ParticleManager particles = Particles;
			if (particles != null)
				particles.Update();
			InvalidateGL();
		}

		protected override void OnPaintGdi(Graphics g, Rectangle clippingRect, bool clearBeforeRedraw) {
			Size view = ViewSize;
			lock (bitmapSyncRoot)
				g.DrawImage(bitmap, new Rectangle(Point.Empty, view));
			view.Width -= borderOffset * 2;
			view.Height -= borderOffset * 2;
			Particles.Render2D(g, view);
		}

		private object UpdateTexture(object image) {
			Bitmap bitmap = (Bitmap) image;
			texture.UpdateRegion(bitmap.ConvertPixelFormat(System.Drawing.Imaging.PixelFormat.Format32bppArgb), new Rectangle(0, 0, webcamWidth, webcamHeight), Point.Empty);
			OnPaintGL();
			return null;
		}

		private void CheckPixel(int i, object parameter) {
			PixelCanvas canvas = (PixelCanvas) parameter;
			BgraColor[] pixels = canvas.colors;
			int x = i % webcamWidth;
			if (x < borderOffset || x >= widthMinusBorderOffset)
				return;
			int y = i / webcamWidth;
			if (canvas.checkIgnore && ignore[x][y])
				canvas.results[x][y] = false;
			else {
				float xTotal = 0f, yTotal = 0f, tempXMult, tempYMult;
				BgraColor pixel;
				float luminosity;
				int offsetX, totalYOffset;
				for (int offsetY = 0; offsetY <= 2; offsetY++) {
					totalYOffset = (y + (offsetY - 1)) * webcamWidth - 1;
					for (offsetX = 0; offsetX <= 2; offsetX++) {
						pixel = pixels[x + offsetX + totalYOffset];
						luminosity = 0.299f * pixel.R + 0.587f * pixel.G + 0.114f * pixel.B;
						tempXMult = xMult[offsetX][offsetY];
						tempYMult = yMult[offsetX][offsetY];
						xTotal += tempXMult * luminosity;
						yTotal += tempYMult * luminosity;
					}
				}
				canvas.results[x][y] = xTotal * xTotal + yTotal * yTotal > threshold;
			}
		}

		private void RemoveNoise(int i) {
			int x = i % webcamWidth;
			if (x < borderOffset || x >= widthMinusBorderOffset)
				return;
			int y = i / webcamWidth;
			if (edgeBuffer[x][y] && !ignore[x][y]) {
				int currentX, currentY, counter = 0;
				ParticleManager manager = Particles;
				if (manager == null)
					return;
				Point[] concentric = manager.concentricPoints;
				for (int pointIndex = 0; pointIndex < noiseRemovalRadius && counter < significantCloseEdgeCount; pointIndex++) {
					currentX = x + concentric[pointIndex].X;
					currentY = y + concentric[pointIndex].Y;
					if (currentY >= borderOffset && currentY < heightMinusBorderOffset && currentX >= borderOffset && currentX < widthMinusBorderOffset && edgeBuffer[currentX][currentY] && !ignore[currentX][currentY])
						counter++;
				}
				if (counter < significantCloseEdgeCount) {
					edgeBuffer[x][y] = false;
					resultant[i] = IgnoreColor;
				} else
					resultant[i] = EdgeColor;
			} else
				resultant[i] = IgnoreColor;
			isEdge[x][y] = edgeBuffer[x][y];
		}

		/// <summary>
		/// Called when the OpenGL context is initialized.
		/// </summary>
		protected override void OnGLInitialized() {
			base.OnGLInitialized();
			MeshComponent.SetupGLEnvironment();
			Matrix4 ortho = Matrix4.CreateOrthographicOffCenter(-1f, 1f, 1f, -1f, -1f, 5f);
			Mesh2D.Setup2D(ref ortho);
			webcamMesh = Mesh2D.CreateShared2DMeshRect();
		}

		/// <summary>
		/// Called whenever a GL render takes place.
		/// </summary>
		protected override void OnPaintGL() {
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			Mesh2D.DrawTexture2D(texture, Vector3.Zero, new Vector3(-1f, -1f, 4f), new Vector2(2f, 2f), Vector3.Zero, webcamMesh);
			Particles.Render();
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		public static void Main() {
			Application.EnableVisualStyles();
			System.Diagnostics.ErrorHandler.Behavior = System.Diagnostics.ErrorDialogAction.ThrowRegardless;
			devices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
			if (devices.Count == 0) {
				if (StyledMessageBox.Show("No webcam device has been found. Retry?", "Error", false, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
					Main();
				return;
			}
			using (VideoCaptureDeviceForm form = new VideoCaptureDeviceForm()) {
				if (form.ShowDialog() != DialogResult.OK)
					return;
				webcam = form.VideoDevice;
				if (webcam == null)
					return;
				MessageLoop.Run(new ParticlesDemo());
			}
		}

		protected override void OnUnload() {
			if (Particles != null) {
				Particles.Dispose();
				Particles = null;
			}
			if (webcamMesh != null) {
				webcamMesh.Dispose();
				webcamMesh = null;
			}
		}

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ParticlesDemo));
			this.SuspendLayout();
			// 
			// ParticlesDemo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.ClientSize = new System.Drawing.Size(548, 406);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "ParticlesDemo";
			this.Text = "Particles Demo";
			this.ResumeLayout(false);

		}

		private class PixelCanvas {
			public BgraColor[] colors;
			public bool[][] results;
			public bool checkIgnore;

			public PixelCanvas(BgraColor[] colors, bool[][] results, bool checkIgnore) {
				this.colors = colors;
				this.results = results;
				this.checkIgnore = checkIgnore;
			}
		}
	}
}
