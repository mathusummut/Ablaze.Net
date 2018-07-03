using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace RippleDemo {
	/// <summary>
	/// The form that views the ripple demo.
	/// </summary>
	public class RippleDemoForm : StyledForm {
		private static Bitmap image;
		private AsyncTimer asyncTimer = new AsyncTimer(100);
		private object SyncRoot = new object();

		/// <summary>
		/// Initializes the ripple demo.
		/// </summary>
		public RippleDemoForm() {
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
			InitializeComponent();
			asyncTimer.Tick += asyncTimer_Tick;
		}

		/// <summary>
		/// Main entry point.
		/// </summary>
		[STAThread]
		public static void Main(string[] args) {
#if NET45
			try {
				System.Runtime.ProfileOptimization.SetProfileRoot(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location));
				System.Runtime.ProfileOptimization.StartProfile("LoadCache.prf");
			} catch {
			}
#endif
			Start:
			if (args.Length == 0) {
				FilePrompt dialog = new FilePrompt();
				dialog.FileName = "Image.jpg";
				dialog.Title = "Choose an image...";
				dialog.Filter = "Image Files|" + Extensions.ImageFileExtensions;
				if (MessageLoop.ShowDialog(dialog) == DialogResult.OK) {
					try {
						image = Extensions.ImageFromFile(dialog.FileName);
						MessageLoop.Run(new RippleDemoForm());
					} catch (Exception ex) {
						StyledMessageBox.Show("An error occurred while loading the specified image:\n" + ex.Message, "Error", true, MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			} else {
				string file = args[0].Trim();
				if (FileUtils.FileExists(file)) {
					try {
						image = Extensions.ImageFromFile(file);
						MessageLoop.Run(new RippleDemoForm());
					} catch {
						args = new string[0];
						goto Start;
					}
				} else {
					args = new string[0];
					goto Start;
				}
			}
		}

		/// <summary>
		/// Called when the window is shown.
		/// </summary>
		/// <param name="e">Ignored.</param>
		protected override void OnShown(EventArgs e) {
			base.OnShown(e);
			asyncTimer.Running = true;
		}

		private void asyncTimer_Tick(object sender) {
			lock (SyncRoot) {
				image.SobelEdgeFilter();
				image.ChangeBrightness(1.2f);
			}
			Invalidate(false);
			Update();
		}

		/// <summary>
		/// Called when the form is painted.
		/// </summary>
		/// <param name="g">The Graphics object to use to paint on.</param>
		/// <param name="clippingRect">The clipping rectangle that was invalidated.</param>
		protected override void OnPaint(Graphics g, Rectangle clippingRect) {
			g.PixelOffsetMode = PixelOffsetMode.HighQuality;
			g.CompositingQuality = CompositingQuality.HighQuality;
			g.SmoothingMode = SmoothingMode.HighQuality;
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			lock (SyncRoot)
				g.DrawImage(image, new Rectangle(Point.Empty, ViewSize));
			base.OnPaint(g, clippingRect);
		}

		/// <summary>
		/// Called when the window is about to be closed.
		/// </summary>
		/// <param name="reason">The close reason.</param>
		protected override bool OnQueryClose(CloseReason reason) {
			asyncTimer.Dispose();
			lock (SyncRoot)
				return true;
		}

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RippleDemoForm));
			this.SuspendLayout();
			// 
			// RippleDemoForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(760, 585);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "RippleDemoForm";
			this.Text = "Ripple Demo";
			this.ResumeLayout(false);

		}
	}
}