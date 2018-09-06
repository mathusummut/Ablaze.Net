using System.Drawing;
using System.Platforms.Windows;
using System.Reflection;
using System.Threading;

namespace System.Windows.Forms {
	/// <summary>
	/// Initializes a new splash form.
	/// </summary>
	public class SplashForm : Form {
		private Point oldLoc, startCurLoc;
		private byte opacity = 255;

		internal byte OpacityInner {
			get {
				return opacity;
			}
			set {
				opacity = value;
				if (!IsDisposed)
					NativeApi.SetLayeredWindowAttributes(Handle, COLORREF.Empty, value, BlendFlags.ULW_ALPHA);
			}
		}

		/// <summary>
		/// Gets whether the closing animation is being played.
		/// </summary>
		public bool FadingOut {
			get;
			private set;
		}

		/// <summary>
		/// Gets whether the splash screen is being moved.
		/// </summary>
		public bool IsLeftMouseDown {
			get;
			private set;
		}

		/// <summary>
		/// Adds a drop-shadow effect.
		/// </summary>
		protected override CreateParams CreateParams {
			get {
				CreateParams cp = base.CreateParams;
				cp.ClassStyle |= (int) ClassStyle.DropShadow;
				return cp;
			}
		}

		/// <summary>
		/// Initializes a splash screen showing the specified image.
		/// </summary>
		/// <param name="background">The image to show.</param>
		public SplashForm(Image background) {
			SuspendLayout();
			SetStyle(ControlStyles.UserPaint | ControlStyles.CacheText, true);
			BackgroundImage = background;
			CausesValidation = false;
			ClientSize = background.Size;
			ControlBox = false;
			FormBorderStyle = FormBorderStyle.None;
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "Splash";
			ShowIcon = false;
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterScreen;
			Text = "Splash";
			TopMost = true;
			UseWaitCursor = true;
			ResumeLayout(false);
		}

		/// <summary>
		/// Sets whether the prompt is visible.
		/// </summary>
		/// <param name="value">The visibility flag.</param>
		protected override void SetVisibleCore(bool value) {
			base.SetVisibleCore(DesignMode ? false : value);
		}

		/// <summary>
		/// Called when the mouse is pressed.
		/// </summary>
		protected override void OnMouseDown(MouseEventArgs e) {
			base.OnMouseDown(e);
			if (e.Button == MouseButtons.Left && Enabled) {
				oldLoc = Location;
				startCurLoc = Cursor.Position;
				IsLeftMouseDown = true;
			}
		}

		/// <summary>
		/// Called when the mouse is moved.
		/// </summary>
		protected override void OnMouseMove(MouseEventArgs e) {
			base.OnMouseMove(e);
			if (IsLeftMouseDown)
				Location = oldLoc + (Size) (Cursor.Position - (Size) startCurLoc);
		}

		/// <summary>
		/// Called when the mouse is released.
		/// </summary>
		protected override void OnMouseUp(MouseEventArgs e) {
			base.OnMouseUp(e);
			if (e.Button == MouseButtons.Left)
				IsLeftMouseDown = false;
		}

		/// <summary>
		/// Called when the splash screen is shown.
		/// </summary>
		protected override void OnShown(EventArgs e) {
			base.OnShown(e);
			DoubleBuffered = true;
			AllowTransparency = true;
		}

		/// <summary>
		/// Called when the splash screen is painted.
		/// </summary>
		protected override void OnPaint(PaintEventArgs e) {
		}

		/// <summary>
		/// Called when the splash screen is being closed.
		/// </summary>
		protected override void OnFormClosing(FormClosingEventArgs e) {
			if (e.Cancel || opacity == 1)
				return;
			if (FadingOut)
				e.Cancel = true;
			else if (!StyledForm.DesignMode) {
				AllowTransparency = true;
				if (AllowTransparency) {
					FadingOut = true;
					e.Cancel = true;
					opacity = 255;
					UIAnimator.SharedAnimator.Animate(new FieldOrProperty(nameof(OpacityInner), this), (byte) 1, 0.2, 0.01, true, OnUpdate, true);
				}
			}
			base.OnFormClosing(e);
		}

		private bool OnUpdate(AnimationInfo state) {
			if (state.IsFinished) {
				FadingOut = false;
				Close();
				return true;
			} else
				return !IsDisposed;
		}

		/// <summary>
		/// Called when the splash screen is closed.
		/// </summary>
		protected override void OnFormClosed(FormClosedEventArgs e) {
			base.OnFormClosed(e);
			Dispose();
		}

		/// <summary>
		/// Starts a splash screen on a seperate thread.
		/// </summary>
		/// <param name="background">The background image of the splash screen.</param>
		/// <param name="onSeparateThread">Whether to run the splash screen on a separate thread.</param>
		public static SplashForm ShowSplash(Image background, bool onSeparateThread) {
			SplashForm splash = null;
			if (onSeparateThread) {
				using (ManualResetEventSlim resetEvent = new ManualResetEventSlim()) {
					Thread thread = new Thread(delegate () {
						splash = new SplashForm(background);
						resetEvent.Set();
						MessageLoop.Run(splash);
					});
					thread.Name = "SplashFormThread";
					thread.IsBackground = true;
					thread.SetApartmentState(ApartmentState.STA);
					thread.Start();
					resetEvent.Wait(5000);
				}
			} else {
				splash = new SplashForm(background);
				splash.Show();
			}
			return splash;
		}
	}
}