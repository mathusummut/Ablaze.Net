using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Platforms.Windows;
using System.Reflection;
using System.Threading.Tasks;

namespace System.Windows.Forms {
	/// <summary>
	/// Adds a shadow to the specified borderless form.
	/// </summary>
	public class DropShadowForm : Form, IDrawable {
		/// <summary>
		/// Whether drop shadows are supported. Windows 2000 or later is required.
		/// </summary>
		public static bool IsSupported = Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major > 4;
		private Action<EventArgs> CallOnResize;
		private object BitmapSyncRoot = new object();
		private Color shadowColor = Color.FromArgb(160, Color.Black);
		private Bitmap shadowBitmap;
		private Rectangle beforeResize;
		private Point mouseStartingPoint;
		private Action updateShadow;
		private Form parent;
		private int shadowBlur = 4;
		private byte opacity = 255;
		internal bool needsRedraw = true;
		private bool wasShown, isBeingResizedTop, isBeingResizedTopLeft, isBeingResizedTopRight, isBeingResizedLeft, isBeingResizedRight,
			isBeingResizedBottom, isBeingResizedBottomLeft, isBeingResizedBottomRight, destroyHandle, showShadow = true;

		/// <summary>
		/// Gets whether the shadow is currently visible.
		/// </summary>
		[Browsable(false)]
		public bool IsShadowVisible {
			get {
				StyledForm styledOwner = parent as StyledForm;
				return !StyledForm.DesignMode && IsSupported && showShadow && shadowBlur != 0 && shadowColor.A != 0 && opacity != 0
					&& parent != null && parent.FormBorderStyle == FormBorderStyle.None && parent.WindowState == FormWindowState.Normal
					&& (styledOwner == null ? parent.Visible : (styledOwner.Visible && !(styledOwner.IsMinimized
					|| styledOwner.IsMinimizing || styledOwner.IsRestoring || styledOwner.IsFullScreen || styledOwner.AnimatingBounds
					|| styledOwner.IsClosing || styledOwner.IsFullScreen || styledOwner.Bounds == styledOwner.ParentScreen.WorkingArea)));
			}
		}

		/// <summary>
		/// Gets whether the control supports OpenGL rendering.
		/// </summary>
		[Browsable(false)]
		public bool SupportsGL {
			get {
				return false;
			}
		}

		/// <summary>
		/// Gets whether the form is disposed.
		/// </summary>
		public new bool IsDisposed {
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets the opacity of the form.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new byte Opacity {
			get {
				return opacity;
			}
			set {
				if (opacity == value)
					return;
				opacity = value;
				UpdateShadow();
			}
		}

		/// <summary>
		/// Gets or sets whether the shadow should be shown.
		/// </summary>
		[Description("Gets or sets whether the shadow should be shown.")]
		[Browsable(true)]
		public bool ShowShadow {
			get {
				return showShadow;
			}
			set {
				if (showShadow == value)
					return;
				showShadow = value;
				UpdateShadow();
			}
		}

		/// <summary>
		/// Gets or sets the shadow color. An alpha of 160 usually looks best.
		/// </summary>
		[Description("Gets or sets the shadow color. An alpha of 160 usually looks best.")]
		public Color ShadowColor {
			get {
				return shadowColor;
			}
			set {
				if (shadowColor == value)
					return;
				Color oldColor = shadowColor;
				shadowColor = value;
				if (!((value.R == oldColor.R && value.G == oldColor.G && value.B == oldColor.B) || parent == null)) {
					lock (BitmapSyncRoot) {
						using (PixelWorker worker = PixelWorker.FromImage(shadowBitmap, false, true)) {
							int blur = ScaledShadowBlur;
							int twiceBlur = blur * 2;
							Size size = parent.Size;
							int width = size.Width + twiceBlur;
							int height = size.Height + twiceBlur;
							Action<int> setPixelColor = i => {
								int pixelIndex = i * 4;
								worker[pixelIndex] = value.B;
								worker[pixelIndex + 1] = value.G;
								worker[pixelIndex + 2] = value.R;
							};
							int horizontalRegionMax = blur * width;
							ParallelLoop.For(0, horizontalRegionMax, setPixelColor, ImageLib.ParallelCutoff);
							ParallelLoop.For(0, blur * size.Height, i => {
								int pixelIndex = ((i % size.Height + blur) * worker.Width + i / size.Height) * 4;
								worker[pixelIndex] = value.B;
								worker[pixelIndex + 1] = value.G;
								worker[pixelIndex + 2] = value.R;
							}, ImageLib.ParallelCutoff);
							ParallelLoop.For(0, blur * size.Height, i => {
								int pixelIndex = ((i % size.Height + blur) * worker.Width + i / size.Height + size.Width + blur) * 4;
								worker[pixelIndex] = value.B;
								worker[pixelIndex + 1] = value.G;
								worker[pixelIndex + 2] = value.R;
							}, ImageLib.ParallelCutoff);
							int start = (size.Height + blur) * width;
							ParallelLoop.For(start, start + horizontalRegionMax, setPixelColor, ImageLib.ParallelCutoff);
						}
					}
				}
				UpdateShadow();
			}
		}

		/// <summary>
		/// Gets or sets the shadow blur radius.
		/// </summary>
		[Description("Gets or sets the shadow blur radius.")]
		[DefaultValue(4)]
		public int ShadowBlur {
			get {
				return shadowBlur;
			}
			set {
				if (value < 0)
					value = 0;
				if (shadowBlur == value)
					return;
				shadowBlur = value;
				needsRedraw = true;
				UpdateShadow();
			}
		}

		private int ScaledShadowBlur {
			get {
				StyledForm styledOwner = parent as StyledForm;
				return styledOwner == null ? shadowBlur : (int) (shadowBlur * Math.Min(styledOwner.DpiScale.Width, styledOwner.DpiScale.Height));
			}
		}

		/// <summary>
		/// Gets or sets whether mouse resize is allowed.
		/// </summary>
		[Description("Gets or sets whether mouse resize is allowed.")]
		public bool AllowMouseResize {
			get;
			set;
		}

		/// <summary>
		/// Specifies the parameters used to create the form.
		/// </summary>
		protected override CreateParams CreateParams {
			get {
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= (int) (ExtendedWindowStyle.Layered | ExtendedWindowStyle.NoActivate);
				return cp;
			}
		}

		/// <summary>
		/// Gets whether the window should be shown without being activated.
		/// </summary>
		protected override bool ShowWithoutActivation {
			get {
				return true;
			}
		}

		/// <summary>
		/// Adds a shadow to the specified borderless form.
		/// </summary>
		/// <param name="form">The window to add the shadow to.</param>
		public DropShadowForm(Form form) {
			updateShadow = UpdateShadowInner;
			Name = nameof(DropShadowForm);
			Text = "DropShadow";
			FormBorderStyle = FormBorderStyle.None;
			FormBorderStyle style = form == null ? FormBorderStyle.None : form.FormBorderStyle;
			AllowMouseResize = style == FormBorderStyle.None || style == FormBorderStyle.Sizable || style == FormBorderStyle.SizableToolWindow;
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint | ControlStyles.UserMouse | ControlStyles.OptimizedDoubleBuffer | ControlStyles.CacheText, true);
			SetStyle(ControlStyles.Selectable, false);
			ShowInTaskbar = false;
			if (StyledForm.DesignMode || !IsSupported)
				return;
			parent = form;
			if (form != null) {
				CallOnResize = (Action<EventArgs>) Delegate.CreateDelegate(typeof(Action<EventArgs>), form, typeof(Form).GetMethod(nameof(OnResizeEnd), BindingFlags.NonPublic | BindingFlags.Instance));
				CreateHandle();
				Owner = form;
				if (form.Visible)
					OnOwnerVisibleChanged(this, EventArgs.Empty);
			}
		}

		private void OnOwnerResizeEnd(object sender, EventArgs e) {
			UpdateShadow();
		}

		private void OnOwnerActivated(object sender, EventArgs e) {
			if (parent != null)
				NativeApi.SetWindowPos(Handle, parent.Handle, 0, 0, 0, 0, SetWindowPosFlags.NOMOVE | SetWindowPosFlags.NOSIZE | SetWindowPosFlags.NOACTIVATE | SetWindowPosFlags.NOREDRAW | SetWindowPosFlags.NOSENDCHANGING | SetWindowPosFlags.ASYNCWINDOWPOS | SetWindowPosFlags.DEFERERASE);
		}

		private void OnOwnerVisibleChanged(object sender, EventArgs e) {
			if (parent == null)
				return;
			Visible = parent.Visible;
			Owner_LocationChanged(sender, e);
			Owner_SizeChanged(sender, e);
			OnOwnerActivated(sender, e);
		}

		private void OnOwnerClosed(object sender, EventArgs e) {
			Visible = false;
		}

		private void Owner_LocationChanged(object sender, EventArgs eventArgs) {
			if (parent == null)
				return;
			int blur = ScaledShadowBlur;
			Location = parent.Location - new Size(blur, blur);
			bool isShadowVisible = IsShadowVisible;
			if (wasShown == isShadowVisible)
				return;
			wasShown = isShadowVisible;
			if (sender != this)
				UpdateShadow();
		}

		private void Owner_SizeChanged(object sender, EventArgs e) {
			if (parent == null)
				return;
			Size size = parent.Size;
			int twoTimesBlur = ScaledShadowBlur * 2;
			size.Width += twoTimesBlur;
			size.Height += twoTimesBlur;
			bool isShadowVisible = IsShadowVisible;
			if (Size == size && wasShown == isShadowVisible)
				return;
			wasShown = isShadowVisible;
			Size = size;
			needsRedraw = true;
			if (sender != this)
				UpdateShadow();
		}

		/// <summary>
		/// Updates the shadow size.
		/// </summary>
		public void UpdateSize() {
			Owner_SizeChanged(null, EventArgs.Empty);
		}

		/// <summary>
		/// Creates the handle for this window.
		/// </summary>
		protected override void CreateHandle() {
			lock (this) {
				if (!IsHandleCreated) {
					IsDisposed = false;
					this.SetState(2048, false);
					base.CreateHandle();
					if (parent == null)
						return;
					Owner_LocationChanged(this, EventArgs.Empty);
					Owner_SizeChanged(this, EventArgs.Empty);
					parent.LocationChanged += Owner_LocationChanged;
					parent.SizeChanged += Owner_SizeChanged;
					parent.FormClosing += OnOwnerClosed;
					parent.VisibleChanged += OnOwnerVisibleChanged;
					parent.Activated += OnOwnerActivated;
					parent.ResizeEnd += OnOwnerResizeEnd;
					UpdateShadow();
				}
			}
		}

		/// <summary>
		/// Destroys the handle of this window.
		/// </summary>
		protected override void DestroyHandle() {
			if (destroyHandle)
				base.DestroyHandle();
			if (parent == null)
				return;
			parent.LocationChanged += Owner_LocationChanged;
			parent.SizeChanged += Owner_SizeChanged;
			parent.FormClosing += OnOwnerClosed;
			parent.VisibleChanged += OnOwnerVisibleChanged;
			parent.Activated += OnOwnerActivated;
			parent.ResizeEnd += OnOwnerResizeEnd;
		}

		/// <summary>
		/// Called when a mouse button is pressed.
		/// </summary>
		/// <param name="e">The current mouse location.</param>
		protected override void OnMouseDown(MouseEventArgs e) {
			base.OnMouseDown(e);
			if (AllowMouseResize && e.Button == MouseButtons.Left) {
				if (parent == null)
					return;
				beforeResize = parent.Bounds;
				int blur = ScaledShadowBlur;
				Point rightBorder = new Point(beforeResize.Width + blur, beforeResize.Height + blur);
				if (e.Y < blur) {
					if (e.X < blur)
						isBeingResizedTopLeft = true;
					else if (e.X > rightBorder.X)
						isBeingResizedTopRight = true;
					else
						isBeingResizedTop = true;
				} else if (e.Y > rightBorder.Y) {
					if (e.X < blur)
						isBeingResizedBottomLeft = true;
					else if (e.X > rightBorder.X)
						isBeingResizedBottomRight = true;
					else
						isBeingResizedBottom = true;
				} else {
					if (e.X < blur)
						isBeingResizedLeft = true;
					else
						isBeingResizedRight = true;
				}
				Capture = true;
				mouseStartingPoint = Cursor.Position;
			}
		}

		/// <summary>
		/// Called when the mouse is moved.
		/// </summary>
		/// <param name="e">The mouse event info.</param>
		protected override void OnMouseMove(MouseEventArgs e) {
			base.OnMouseMove(e);
			if (parent == null)
				return;
			Size minimumSize = parent.MinimumSize, maximumSize = parent.MaximumSize;
			Point mouseChange = Cursor.Position - (Size) mouseStartingPoint;
			Cursor resultantCursor = Cursors.Default;
			if (isBeingResizedTop) {
				int targetTop = beforeResize.Y + mouseChange.Y;
				int targetHeight = beforeResize.Height - mouseChange.Y;
				if (minimumSize.Height != 0 && targetHeight < minimumSize.Height) {
					int change = minimumSize.Height - targetHeight;
					targetHeight = minimumSize.Height;
					targetTop -= change;
				}
				if (maximumSize.Height != 0 && targetHeight > maximumSize.Height) {
					int change = maximumSize.Height - targetHeight;
					targetHeight = maximumSize.Height;
					targetTop -= change;
				}
				parent.Top = targetTop;
				parent.Height = targetHeight;
				resultantCursor = Cursors.SizeNS;
			} else if (isBeingResizedTopLeft) {
				int targetTop = beforeResize.Y + mouseChange.Y;
				int targetHeight = beforeResize.Height - mouseChange.Y;
				if (minimumSize.Height != 0 && targetHeight < minimumSize.Height) {
					int change = minimumSize.Height - targetHeight;
					targetHeight = minimumSize.Height;
					targetTop -= change;
				}
				if (maximumSize.Height != 0 && targetHeight > maximumSize.Height) {
					int change = maximumSize.Height - targetHeight;
					targetHeight = maximumSize.Height;
					targetTop -= change;
				}
				int targetLeft = beforeResize.X + mouseChange.X;
				int targetWidth = beforeResize.Width - mouseChange.X;
				if (minimumSize.Width != 0 && targetWidth < minimumSize.Width) {
					int change = minimumSize.Width - targetWidth;
					targetWidth = minimumSize.Width;
					targetLeft -= change;
				}
				if (maximumSize.Width != 0 && targetWidth > maximumSize.Width) {
					int change = maximumSize.Width - targetWidth;
					targetWidth = maximumSize.Width;
					targetLeft -= change;
				}
				parent.Bounds = new Rectangle(targetLeft, targetTop, targetWidth, targetHeight);
				resultantCursor = Cursors.SizeNWSE;
			} else if (isBeingResizedTopRight) {
				int targetTop = beforeResize.Y + mouseChange.Y;
				int targetHeight = beforeResize.Height - mouseChange.Y;
				if (minimumSize.Height != 0 && targetHeight < minimumSize.Height) {
					int change = minimumSize.Height - targetHeight;
					targetHeight = minimumSize.Height;
					targetTop -= change;
				}
				if (maximumSize.Height != 0 && targetHeight > maximumSize.Height) {
					int change = maximumSize.Height - targetHeight;
					targetHeight = maximumSize.Height;
					targetTop -= change;
				}
				int targetWidth = beforeResize.Width + mouseChange.X;
				if (minimumSize.Width != 0 && targetWidth < minimumSize.Width)
					targetWidth = minimumSize.Width;
				if (maximumSize.Width != 0 && targetWidth > maximumSize.Width)
					targetWidth = maximumSize.Width;
				parent.Top = targetTop;
				parent.Size = new Size(targetWidth, targetHeight);
				resultantCursor = Cursors.SizeNESW;
			} else if (isBeingResizedLeft) {
				int targetLeft = beforeResize.X + mouseChange.X;
				int targetWidth = beforeResize.Width - mouseChange.X;
				if (minimumSize.Width != 0 && targetWidth < minimumSize.Width) {
					int change = minimumSize.Width - targetWidth;
					targetWidth = minimumSize.Width;
					targetLeft -= change;
				}
				if (maximumSize.Width != 0 && targetWidth > maximumSize.Width) {
					int change = maximumSize.Width - targetWidth;
					targetWidth = maximumSize.Width;
					targetLeft -= change;
				}
				parent.Left = targetLeft;
				parent.Width = targetWidth;
				resultantCursor = Cursors.SizeWE;
			} else if (isBeingResizedRight) {
				int targetWidth = beforeResize.Width + mouseChange.X;
				if (minimumSize.Width != 0 && targetWidth < minimumSize.Width)
					targetWidth = minimumSize.Width;
				if (maximumSize.Width != 0 && targetWidth > maximumSize.Width)
					targetWidth = maximumSize.Width;
				parent.Width = targetWidth;
				resultantCursor = Cursors.SizeWE;
			} else if (isBeingResizedBottom) {
				int targetHeight = beforeResize.Height + mouseChange.Y;
				if (minimumSize.Height != 0 && targetHeight < minimumSize.Height)
					targetHeight = minimumSize.Height;
				if (maximumSize.Height != 0 && targetHeight > maximumSize.Height)
					targetHeight = maximumSize.Height;
				parent.Height = targetHeight;
				resultantCursor = Cursors.SizeNS;
			} else if (isBeingResizedBottomLeft) {
				int targetHeight = beforeResize.Height + mouseChange.Y;
				if (minimumSize.Height != 0 && targetHeight < minimumSize.Height)
					targetHeight = minimumSize.Height;
				if (maximumSize.Height != 0 && targetHeight > maximumSize.Height)
					targetHeight = maximumSize.Height;
				int targetLeft = beforeResize.X + mouseChange.X;
				int targetWidth = beforeResize.Width - mouseChange.X;
				if (minimumSize.Width != 0 && targetWidth < minimumSize.Width) {
					int change = minimumSize.Width - targetWidth;
					targetWidth = minimumSize.Width;
					targetLeft -= change;
				}
				if (maximumSize.Width != 0 && targetWidth > maximumSize.Width) {
					int change = maximumSize.Width - targetWidth;
					targetWidth = maximumSize.Width;
					targetLeft -= change;
				}
				parent.Left = targetLeft;
				parent.Size = new Size(targetWidth, targetHeight);
				resultantCursor = Cursors.SizeNESW;
			} else if (isBeingResizedBottomRight) {
				int targetHeight = beforeResize.Height + mouseChange.Y;
				if (minimumSize.Height != 0 && targetHeight < minimumSize.Height)
					targetHeight = minimumSize.Height;
				if (maximumSize.Height != 0 && targetHeight > maximumSize.Height)
					targetHeight = maximumSize.Height;
				int targetWidth = beforeResize.Width + mouseChange.X;
				if (minimumSize.Width != 0 && targetWidth < minimumSize.Width)
					targetWidth = minimumSize.Width;
				if (maximumSize.Width != 0 && targetWidth > maximumSize.Width)
					targetWidth = maximumSize.Width;
				parent.Size = new Size(targetWidth, targetHeight);
				resultantCursor = Cursors.SizeNWSE;
			} else if (AllowMouseResize) {
				int blur = ScaledShadowBlur;
				Point rightBorder = new Point(parent.Width + blur, parent.Height + blur);
				if (e.Y < blur) {
					if (e.X < blur)
						resultantCursor = Cursors.SizeNWSE;
					else if (e.X > rightBorder.X)
						resultantCursor = Cursors.SizeNESW;
					else
						resultantCursor = Cursors.SizeNS;
				} else if (e.Y >= rightBorder.Y) {
					if (e.X < blur)
						resultantCursor = Cursors.SizeNESW;
					else if (e.X > rightBorder.X)
						resultantCursor = Cursors.SizeNWSE;
					else
						resultantCursor = Cursors.SizeNS;
				} else if (e.X < blur || e.X > rightBorder.X)
					resultantCursor = Cursors.SizeWE;
			}
			if (Cursor.Handle != resultantCursor.Handle)
				Cursor = resultantCursor;
		}

		/// <summary>
		/// Called when a mouse button is released.
		/// </summary>
		/// <param name="e">The mouse event info.</param>
		protected override void OnMouseUp(MouseEventArgs e) {
			base.OnMouseUp(e);
			Capture = false;
			bool wasResized = isBeingResizedTop || isBeingResizedTopLeft || isBeingResizedTopRight || isBeingResizedLeft || isBeingResizedRight || isBeingResizedBottom || isBeingResizedBottomLeft || isBeingResizedBottomRight;
			isBeingResizedTop = false;
			isBeingResizedTopLeft = false;
			isBeingResizedTopRight = false;
			isBeingResizedLeft = false;
			isBeingResizedRight = false;
			isBeingResizedBottom = false;
			isBeingResizedBottomLeft = false;
			isBeingResizedBottomRight = false;
			Cursor = DefaultCursor;
			if (wasResized && CallOnResize != null)
				CallOnResize(EventArgs.Empty);
			if (parent != null)
				parent.Focus();
		}

		/// <summary>
		/// Called when the window background is painted.
		/// </summary>
		/// <param name="e">The graphics object to draw onto.</param>
		protected override void OnPaintBackground(PaintEventArgs e) {
		}

		/// <summary>
		/// Called when the window is painted.
		/// </summary>
		/// <param name="e">The graphics object to draw onto.</param>
		protected override void OnPaint(PaintEventArgs e) {
		}

		/// <summary>
		/// Draws the shadow onto the specified graphics canvas.
		/// </summary>
		/// <param name="g">The graphics canvas to draw on.</param>
		public void DrawGdi(Graphics g) {
			DrawGdi(g, parent == null ? Point.Empty : parent.Location - new Size(shadowBlur, shadowBlur));
		}

		/// <summary>
		/// Draws the shadow onto the specified graphics canvas.
		/// </summary>
		/// <param name="g">The graphics canvas to draw on.</param>
		/// <param name="location">The location at which to draw the shadow image.</param>
		/// <param name="drawChildren">Whether to draw the child controls.</param>
		public void DrawGdi(Graphics g, Point location, bool drawChildren = true) {
			if (opacity == 0)
				return;
			else if (opacity == 255) {
				lock (BitmapSyncRoot) {
					if (shadowBitmap != null)
						g.DrawImageUnscaled(shadowBitmap, location);
				}
			} else {
				lock (BitmapSyncRoot) {
					if (shadowBitmap != null)
						ImageLib.DrawFaded(g, shadowBitmap, new RectangleF(location, shadowBitmap.Size), opacity * 0.00392156863f);
				}
			}
			if (drawChildren)
				g.DrawControls(Controls, location, Rectangle.Ceiling(g.ClipBounds), true);
		}

		/// <summary>
		/// Not implemented yet.
		/// Draws the control with its children in the current OpenGL context (assumes the GL matrix is set to orthographic and maps to pixel coordinates)
		/// </summary>
		/// <param name="location">The location to draw at</param>
		/// <param name="drawChildren">Whether to draw child controls as well</param>
		public virtual void DrawGL(Point location, bool drawChildren) {
			throw new NotImplementedException(nameof(DrawGL) + " is not implemented.");
		}

		private void RedrawShadow() {
			if (parent == null)
				return;
			BgraColor color = new BgraColor(255, shadowColor);
			Owner_LocationChanged(this, EventArgs.Empty);
			Owner_SizeChanged(this, EventArgs.Empty);
			needsRedraw = false;
			Size size = parent.Size;
			int blur = ScaledShadowBlur;
			int twiceBlur = blur * 2;
			int width = size.Width + twiceBlur;
			int height = size.Height + twiceBlur;
			Point min = new Point(blur - 1, blur - 1);
			Point max = new Point(size.Width + blur, size.Height + blur);
			lock (BitmapSyncRoot) {
				if (shadowBitmap != null)
					shadowBitmap.Dispose();
				shadowBitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
				if (blur > 0) {
					using (PixelWorker worker = PixelWorker.FromImage(shadowBitmap, false, true)) {
						Action<int> setPixelColor = i => {
							int x = i % width;
							int y = i / width;
							float dx = Math.Max(Math.Max(min.X - x, x - max.X), 0);
							float dy = Math.Max(Math.Max(min.Y - y, y - max.Y), 0);
							worker.SetPixelUsingPixelCount(i, new BgraColor((byte) ImageLib.Transition(255.0, 0.0, Math.Min(Math.Sqrt(dx * dx + dy * dy) / blur, 1.0)), color.R, color.G, color.B));
						};
						int horizontalRegionMax = blur * width;
						ParallelLoop.For(0, horizontalRegionMax, setPixelColor, ImageLib.ParallelCutoff);
						ParallelLoop.For(0, blur * size.Height, i => {
							int x = i / size.Height;
							int y = i % size.Height + blur;
							float dx = Math.Max(Math.Max(min.X - x, x - max.X), 0);
							float dy = Math.Max(Math.Max(min.Y - y, y - max.Y), 0);
							worker[x, y] = new BgraColor((byte) ImageLib.Transition(255.0, 0.0, Math.Min(Math.Sqrt(dx * dx + dy * dy) / blur, 1.0)), color.R, color.G, color.B);
						}, ImageLib.ParallelCutoff);
						ParallelLoop.For(0, blur * size.Height, i => {
							int x = i / size.Height + size.Width + blur;
							int y = i % size.Height + blur;
							float dx = Math.Max(Math.Max(min.X - x, x - max.X), 0);
							float dy = Math.Max(Math.Max(min.Y - y, y - max.Y), 0);
							worker[x, y] = new BgraColor((byte) ImageLib.Transition(255.0, 0.0, Math.Min(Math.Sqrt(dx * dx + dy * dy) / blur, 1.0)), color.R, color.G, color.B);
						}, ImageLib.ParallelCutoff);
						int start = (size.Height + blur) * width;
						ParallelLoop.For(start, start + horizontalRegionMax, setPixelColor, ImageLib.ParallelCutoff);
					}
				}
			}
			UpdateShadow();
		}

		/// <summary>
		/// Renders the form and its children onto the specified image
		/// </summary>
		/// <param name="bitmap">The bitmap to draw onto</param>
		public void DrawToBitmap(Bitmap bitmap) {
			DrawToBitmap(bitmap as Image, ClientRectangle, true);
		}

		/// <summary>
		/// Renders the form and its children onto the specified image
		/// </summary>
		/// <param name="image">The image to draw onto</param>
		public void DrawToBitmap(Image image) {
			DrawToBitmap(image, ClientRectangle, true);
		}

		/// <summary>
		/// Renders the form and its children onto the specified image
		/// </summary>
		/// <param name="image">The image to draw onto.</param>
		/// <param name="targetBounds">The bounds within which the form is rendered</param>
		public void DrawToBitmap(Image image, Rectangle targetBounds) {
			DrawToBitmap(image, targetBounds, true);
		}

		/// <summary>
		/// Renders the form and its children onto the specified image
		/// </summary>
		/// <param name="bitmap">The image to draw onto</param>
		/// <param name="targetBounds">The bounds within which the form is rendered</param>
		public new void DrawToBitmap(Bitmap bitmap, Rectangle targetBounds) {
			DrawToBitmap(bitmap as Image, targetBounds, true);
		}

		/// <summary>
		/// Renders the form onto the specified image
		/// </summary>
		/// <param name="bitmap">The image to draw onto</param>
		/// <param name="targetBounds">The bounds within which the form is rendered</param>
		/// <param name="drawChildren">Whether to draw the child controls</param>
		public void DrawToBitmap(Bitmap bitmap, Rectangle targetBounds, bool drawChildren) {
			DrawToBitmap(bitmap as Image, targetBounds, drawChildren);
		}

		/// <summary>
		/// Renders the form onto the specified image
		/// </summary>
		/// <param name="image">The image to draw onto</param>
		/// <param name="targetBounds">The bounds within which the form is rendered</param>
		/// <param name="drawChildren">Whether to draw the child controls</param>
		public void DrawToBitmap(Image image, Rectangle targetBounds, bool drawChildren) {
			if (image == null)
				return;
			Size size = Size;
			if (targetBounds.Width > size.Width)
				targetBounds.Width = size.Width;
			if (targetBounds.Height > size.Height)
				targetBounds.Height = size.Height;
			using (Graphics g = Graphics.FromImage(image)) {
				g.SetClip(targetBounds);
				DrawGdi(g, targetBounds.Location, drawChildren);
				g.DrawImageUnscaledAndClipped(image, targetBounds);
			}
		}

		/// <summary>
		/// Called when a message is sent to the window.
		/// </summary>
		/// <param name="m">The message that was sent to the window.</param>
		protected override void WndProc(ref Message m) {
			if (m.Msg == (int) WindowMessage.ERASEBKGND)
				m.Result = new IntPtr(1);
			else
				base.WndProc(ref m);
		}

		/// <summary>
		/// Updates the shadow.
		/// </summary>
		public void UpdateShadow() {
			BeginInvoke(updateShadow);
		}

		/// <summary>
		/// Updates the shadow.
		/// </summary>
		private void UpdateShadowInner() {
			bool isVisible = IsShadowVisible;
			if (isVisible) {
				MdiParent = parent.MdiParent;
				Visible = true;
				if (shadowBitmap == null || (isVisible && needsRedraw))
					RedrawShadow();
				IntPtr screenDc = NativeApi.GetDC(IntPtr.Zero);
				IntPtr memDc = NativeApi.CreateCompatibleDC(screenDc);
				IntPtr hBitmap = IntPtr.Zero;
				IntPtr oldBitmap = IntPtr.Zero;
				Point pointSource = Point.Empty;
				int blur = ScaledShadowBlur;
				Point topPos = parent.Location - new Size(blur, blur);
				BLENDFUNCTION blend = new BLENDFUNCTION() {
					BlendOp = 0,
					BlendFlags = 0,
					SourceConstantAlpha = (byte) (0.00392156863f * shadowColor.A * opacity),
					AlphaFormat = 1
				};
				try {
					lock (BitmapSyncRoot) {
						hBitmap = shadowBitmap.GetHbitmap(Color.FromArgb(0));
						oldBitmap = NativeApi.SelectObject(memDc, hBitmap);
						Size size = shadowBitmap.Size;
						NativeApi.UpdateLayeredWindow(Handle, screenDc, ref topPos, ref size, memDc, ref pointSource, 0, ref blend, BlendFlags.ULW_ALPHA);
					}
				} finally {
					NativeApi.ReleaseDC(IntPtr.Zero, screenDc);
					if (hBitmap != IntPtr.Zero) {
						NativeApi.SelectObject(memDc, oldBitmap);
						NativeApi.DeleteObject(hBitmap);
					}
					NativeApi.DeleteDC(memDc);
				}
			} else
				Visible = false;
		}

		/// <summary>
		/// Gets a string that describes this instance.
		/// </summary>
		public override string ToString() {
			return nameof(DropShadowForm) + ": " + Name + ", Parent: " + (parent == null ? "null" : parent.Name);
		}

		/// <summary>
		/// Called when the form is disposed.
		/// </summary>
		/// <param name="disposing">Whether to dispose managed resources as well.</param>
		protected override void Dispose(bool disposing) {
			if (IsDisposed)
				return;
			IsDisposed = true;
			if (disposing) {
				destroyHandle = true;
				lock (this) {
					if (IsHandleCreated)
						DestroyHandle();
				}
			}
			StyledForm.SetCalledClosing(this, false);
			StyledForm.SetCalledMakeVisible(this, false);
			StyledForm.SetCalledOnLoad(this, false);
			StyledForm.SetCalledCreateControl(this, false);
			lock (this) {
				try {
					base.Dispose(disposing);
				} catch {
				}
				this.SetState(32, false);
				this.SetState(262144, false);
			}
			if (disposing) {
				if (shadowBitmap != null) {
					lock (BitmapSyncRoot) {
						shadowBitmap.Dispose();
						shadowBitmap = null;
					}
				}
			}
		}
	}
}