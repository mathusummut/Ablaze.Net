using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.CompilerServices;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms {
	/// <summary>
	/// A customizable styled check box.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxItemFilter("System.Windows.Forms")]
	[DesignTimeVisible(true)]
	[DesignerCategory("CommonControls")]
	[Description("A customizable styled check box.")]
	[DisplayName(nameof(StyledCheckBox))]
	[DefaultEvent(nameof(CheckStateChanged))]
	public class StyledCheckBox : TransparentControl, ISmartControl {
		private bool wasPressed, isInside, isRadioButton;
		private Brush brush = Brushes.Black;
		/// <summary>
		/// Fired when the check state is changed.
		/// </summary>
		public event Action<CheckState> CheckStateChanged;
		private CheckState state = CheckState.Unchecked;
		private Rectangle checkBox = new Rectangle(0, 0, 5, 5);
		private Image image;
		private Icon icon;
		private bool showCheckBox = true, addPaddingRegardless;
		/// <summary>
		/// Fired when the image has been changed.
		/// </summary>
		public event EventHandler ImageChanged;
		/// <summary>
		/// Fired when the icon has been changed.
		/// </summary>
		public event EventHandler IconChanged;

		/// <summary>
		/// Gets a dummy layout engine.
		/// </summary>
		[Browsable(false)]
		public override LayoutEngine LayoutEngine {
			get {
				return DummyLayoutEngine.Instance;
			}
		}

		/// <summary>
		/// Gets whether the control supports OpenGL rendering.
		/// </summary>
		public bool SupportsGL {
			get {
				return false;
			}
		}

		/// <summary>
		/// Gets the label that renders the text.
		/// </summary>
		[Description("Gets the label that renders the text.")]
		public StyledLabel Label {
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets whether the checkbox is checked when it is clicked/pressed.
		/// </summary>
		[Description("Gets or sets whether the checkbox is checked when it is clicked/pressed.")]
		[DefaultValue(true)]
		public bool CheckOnClick {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets whether the checkbox is shown.
		/// </summary>
		[Description("Gets or sets whether the checkbox is shown.")]
		[DefaultValue(true)]
		public bool ShowCheckBox {
			get {
				return showCheckBox;
			}
			set {
				if (value == showCheckBox)
					return;
				showCheckBox = value;
				CheckAutoSize();
			}
		}

		/// <summary>
		/// Gets or sets whether checkbox padding is added regardless if the checkbox is shown or not.
		/// </summary>
		[Description("Gets or sets whether checkbox padding is added regardless if the checkbox is shown or not.")]
		[DefaultValue(false)]
		public bool AddPaddingRegardless {
			get {
				return addPaddingRegardless;
			}
			set {
				if (value == addPaddingRegardless)
					return;
				addPaddingRegardless = value;
				if (!showCheckBox && image == null && icon == null)
					CheckAutoSize();
			}
		}

		/// <summary>
		/// Gets or sets the image to show instead of a checkbox. Icon takes precendence over Image.
		/// </summary>
		[Description("Gets or sets the image to show instead of a checkbox. Icon takes precendence over Image.")]
		[DefaultValue(null)]
		public Image Image {
			get {
				return image;
			}
			set {
				if (value == null && image == null)
					return;
				image = value;
				OnImageChanged(EventArgs.Empty);
				if (icon == null)
					Invalidate(false);
			}
		}

		/// <summary>
		/// Gets or sets the icon to show instead of a checkbox. Icon takes precendence over Image.
		/// </summary>
		[Description("Gets or sets the icon to show instead of a checkbox. Icon takes precendence over Image.")]
		[DefaultValue(null)]
		public Icon Icon {
			get {
				return icon;
			}
			set {
				if (icon == null && icon == null)
					return;
				icon = value;
				OnIconChanged(EventArgs.Empty);
				Invalidate(false);
			}
		}

		/// <summary>
		/// Gets or sets the text color.
		/// </summary>
		[Description("Gets or sets the text color.")]
		[DefaultValue(typeof(Color), "0x000000")]
		public override Color ForeColor {
			get {
				return Label.ForeColor;
			}
			set {
				if (value == Label.ForeColor)
					return;
				Label.ForeColor = value;
				Invalidate(false);
			}
		}

		/// <summary>
		/// Gets or sets the text font.
		/// </summary>
		[Description("Gets or sets the text font.")]
		[DefaultValue(null)]
		public override Font Font {
			get {
				return Label.Font;
			}
			set {
				if (value == Label.Font)
					return;
				Label.Font = value;
				CheckAutoSize();
			}
		}

		/// <summary>
		/// Gets or sets whether the checkbox is rendered as a radio button.
		/// </summary>
		[Description("Gets or sets whether the checkbox is rendered as a radio button.")]
		[DefaultValue(false)]
		public bool IsRadioButton {
			get {
				return isRadioButton;
			}
			set {
				if (value == isRadioButton)
					return;
				isRadioButton = value;
				if (showCheckBox && image == null && icon == null)
					Invalidate(false);
			}
		}

		/// <summary>
		/// Gets the renderer used for styling.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public StyleRenderer Renderer {
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets the normal inner border width.
		/// </summary>
		[Description("Gets or sets the normal inner border width.")]
		[DefaultValue(1f)]
		public float NormalInnerBorderWidth {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the focused inner border width.
		/// </summary>
		[Description("Gets or sets the focused inner border width.")]
		[DefaultValue(2f)]
		public float FocusInnerBorderWidth {
			get;
			set;
		}

		/// <summary>
		/// Gets whether the check box has input focus.
		/// </summary>
		[Browsable(false)]
		public bool HasFocus {
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets the text to show in the check box label.
		/// </summary>
		[Description("Gets or sets the text to show in the check box label.")]
		[DefaultValue(nameof(StyledCheckBox))]
		public override string Text {
			get {
				return Label.Text;
			}
			set {
				if (value == null)
					value = string.Empty;
				string text = Label.Text;
				Label.Text = Label.ReplaceTabs(value.Replace("\r", string.Empty));
				if (text == Label.Text)
					return;
				OnTextChanged(EventArgs.Empty);
				CheckAutoSize();
			}
		}

		/// <summary>
		/// Gets or sets the check-state of the check box.
		/// </summary>
		[Description("Gets or sets the check-state of the check box.")]
		[DefaultValue(0)]
		public CheckState CheckState {
			get {
				return state;
			}
			set {
				if (value == state)
					return;
				state = value;
				OnCheckStateChanged(state);
			}
		}

		/// <summary>
		/// Gets or sets the Dpi to use for AutoSize calculation.
		/// </summary>
		[Description("Gets or sets the Dpi to use for AutoSize calculation.")]
		[DefaultValue(typeof(SizeF), "96, 96")]
		public SizeF Dpi {
			get {
				return Label.Dpi;
			}
			set {
				if (value.Width <= 0f)
					value.Width = 96f;
				if (value.Height <= 0f)
					value.Height = 96f;
				if (value == Dpi)
					return;
				Label.Dpi = value;
				CheckAutoSize();
			}
		}

		/// <summary>
		/// Gets or sets whether the check box is checked or not.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool Checked {
			get {
				return state == CheckState.Checked || state == CheckState.Indeterminate;
			}
			set {
				if (value == Checked)
					return;
				state = value ? CheckState.Checked : CheckState.Unchecked;
				OnCheckStateChanged(state);
			}
		}

		/// <summary>
		/// Gets or sets the text rendering hint.
		/// </summary>
		[Description("Gets or sets the text rendering hint.")]
		[DefaultValue(3)]
		public TextRenderingHint TextRenderingStyle {
			get {
				return Label.TextRenderingStyle;
			}
			set {
				Label.TextRenderingStyle = value;
				Invalidate(false);
			}
		}

		/// <summary>
		/// Gets or sets the alignment the checkbox.
		/// </summary>
		[Description("Gets or sets the alignment the checkbox.")]
		[DefaultValue((int) ContentAlignment.MiddleLeft)]
		public ContentAlignment TextAlign {
			get {
				return Label.TextAlign;
			}
			set {
				Label.TextAlign = value;
				Invalidate(false);
			}
		}

		/// <summary>
		/// Gets or sets whether the checkbox should be auto-sized.
		/// </summary>
		[Description("Gets or sets whether the checkbox should be auto-sized.")]
		[DefaultValue(false)]
		public override bool AutoSize {
			get {
				return base.AutoSize;
			}
			set {
				base.AutoSize = value;
			}
		}

		/// <summary>
		/// Initializes the styled check box.
		/// </summary>
		public StyledCheckBox() : this(null) {
		}

		/// <summary>
		/// Initializes the styled check box using the specified text.
		/// </summary>
		/// <param name="text">The initial check box text.</param>
		public StyledCheckBox(string text) {
			CheckForIllegalCrossThreadCalls = false;
			SetAutoSizeMode(AutoSizeMode.GrowAndShrink);
			CheckOnClick = true;
			SetStyle(ControlStyles.UserMouse | ControlStyles.OptimizedDoubleBuffer | ControlStyles.CacheText, true);
			SetStyle(ControlStyles.ResizeRedraw, false);
			NormalInnerBorderWidth = 1f;
			FocusInnerBorderWidth = 2f;
			Label = new StyledLabel() {
				BackColor = Color.Transparent,
				RenderShadow = false,
				TextAlign = ContentAlignment.MiddleLeft,
				Padding = new Padding(2, 0, 0, 0)
			};
			Name = nameof(StyledCheckBox);
			BackColor = Color.Transparent;
			if (text != null)
				Label.Text = text;
			base.AutoSize = false;
			Label.AutoSize = false;
			Renderer = new StyleRenderer(UpdateFrame);
			Renderer.SuppressColorChecking = true;
			Renderer.SuppressFunctionCallOnRefresh = true;
			Renderer.NormalInnerBorderColor = Color.FromArgb(198, 229, 229);
			Renderer.SuppressColorChecking = false;
			Renderer.SuppressFunctionCallOnRefresh = false;
		}

		private bool UpdateFrame(AnimationInfo state) {
			if (IsDisposed || Disposing || Renderer == null)
				return false;
			else if (icon == null && image == null && showCheckBox) {
				try {
					Invalidate(checkBox, false);
				} catch {
					return false;
				}
				return true;
			} else {
				if (state != null)
					state.Gradient = 1.0;
				return true;
			}
		}

		/// <summary>
		/// Invalidates the entire surface of the control and causes the control to be redrawn
		/// </summary>
		public new void Invalidate() {
			Invalidate(false);
		}

		/// <summary>
		/// Invalidates the entire surface of the control and causes the control to be redrawn
		/// </summary>
		/// <param name="invalidateChildren">If true, child controls are invalidated as well</param>
		public new void Invalidate(bool invalidateChildren) {
			if (IsHandleCreated)
				base.Invalidate(invalidateChildren);
			else
				NotifyInvalidate(ClientRectangle);
		}

		/// <summary>
		/// Invalidates the specified region of the control in client coordinates</summary>
		/// <param name="rect">The region to invalidate in client coordinates</param>
		public new void Invalidate(Rectangle rect) {
			Invalidate(rect, false);
		}

		/// <summary>
		/// Invalidates the specified region of the control in client coordinates</summary>
		/// <param name="rect">The region to invalidate in client coordinates</param>
		/// <param name="invalidateChildren">If true, child controls are invalidated as well</param>
		public virtual new void Invalidate(Rectangle rect, bool invalidateChildren) {
			if (IsHandleCreated)
				base.Invalidate(rect, invalidateChildren);
			else
				NotifyInvalidate(rect);
		}

		/// <summary>
		/// Renders the control with its children onto the specified image
		/// </summary>
		/// <param name="bitmap">The image to draw onto</param>
		public void DrawToBitmap(Bitmap bitmap) {
			DrawToBitmap(bitmap as Image, ClientRectangle, true);
		}

		/// <summary>
		/// Renders the control with its children onto the specified image
		/// </summary>
		/// <param name="image">The image to draw onto</param>
		public void DrawToBitmap(Image image) {
			DrawToBitmap(image, ClientRectangle, true);
		}

		/// <summary>
		/// Renders the control with its children onto the specified image
		/// </summary>
		/// <param name="bitmap">The image to draw onto</param>
		/// <param name="targetBounds">The bounds within which the form is rendered</param>
		public new void DrawToBitmap(Bitmap bitmap, Rectangle targetBounds) {
			DrawToBitmap(bitmap as Image, targetBounds, true);
		}

		/// <summary>
		/// Renders the control with its children onto the specified image
		/// </summary>
		/// <param name="image">The image to draw onto</param>
		/// <param name="targetBounds">The bounds within which the form is rendered</param>
		public void DrawToBitmap(Image image, Rectangle targetBounds) {
			DrawToBitmap(image, targetBounds, true);
		}

		/// <summary>
		/// Renders the control with its children onto the specified image
		/// </summary>
		/// <param name="bitmap">The image to draw onto</param>
		/// <param name="targetBounds">The bounds within which the form is rendered</param>
		/// <param name="drawChildren">Whether to draw the child controls</param>
		public void DrawToBitmap(Bitmap bitmap, Rectangle targetBounds, bool drawChildren) {
			DrawToBitmap(bitmap as Image, targetBounds, drawChildren);
		}

		/// <summary>
		/// Renders the control onto the specified image
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
		/// Gets the size of the check box.
		/// </summary>
		public virtual Size GetCheckBoxSize() {
			return GetCheckBoxSize(Label);
		}

		/// <summary>
		/// Calculates the check box size that would be associated the specified label.
		/// </summary>
		/// <param name="label">The label to calculate the size of the check box from.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Size GetCheckBoxSize(StyledLabel label) {
			return GetCheckBoxSize(label.GetAutoSize(Size.Empty, false));
		}

		private static Size GetCheckBoxSize(Size labelSize) {
			int checkmarkWidth = Math.Max(Math.Min(labelSize.Width, labelSize.Height) - 2, 7);
			Size resultant = new Size(labelSize.Width + checkmarkWidth + 5, labelSize.Height);
			int maxCheckmarkWidth = Math.Max(Math.Min(resultant.Width - 3, resultant.Height - 3), 7);
			int minCheckmarkWidth = Math.Max((resultant.Width + resultant.Height) / 20, 7);
			if (maxCheckmarkWidth == minCheckmarkWidth)
				return new Size(maxCheckmarkWidth, maxCheckmarkWidth);
			else if (maxCheckmarkWidth < minCheckmarkWidth) {
				int temp = minCheckmarkWidth;
				minCheckmarkWidth = maxCheckmarkWidth;
				maxCheckmarkWidth = temp;
			}
			int width = Math.Max(minCheckmarkWidth, Math.Min(resultant.Width - labelSize.Width, maxCheckmarkWidth));
			return new Size(width, width);
		}

		/// <summary>
		/// Gets the resultant size of the checkbox control if it were to be auto-sized with both grow and shrink enabled.
		/// </summary>
		public Size GetAutoSize() {
			return GetAutoSize(MaximumSize, true);
		}

		/// <summary>
		/// Gets the resultant size of the checkbox control if it were to be auto-sized with both grow and shrink enabled.
		/// </summary>
		/// <param name="maxBounds">The maximum size to allow (0 means unlimited).</param>
		/// <param name="includePadding">If true, padding is included within the size.</param>
		public virtual Size GetAutoSize(Size maxBounds, bool includePadding) {
			Size resultant = Label.GetAutoSize(Size.Empty, false);
			if (addPaddingRegardless || showCheckBox || image != null || icon != null) {
				int checkBoxWidth = GetCheckBoxSize(resultant).Width;
				resultant = new Size(1 + checkBoxWidth + checkBoxWidth / 5 + Label.Padding.Left + resultant.Width, resultant.Height);
			}
			Padding padding = Padding;
			if (includePadding) {
				resultant.Width += padding.Horizontal;
				resultant.Height += padding.Vertical;
			}
			if (maxBounds.Width > 0 && resultant.Width > maxBounds.Width)
				resultant.Width = maxBounds.Width;
			if (maxBounds.Height > 0 && resultant.Height > maxBounds.Height)
				resultant.Height = maxBounds.Height;
			return resultant;
		}

		/// <summary>
		/// Called when the bounds of the control is about to change.
		/// </summary>
		/// <param name="x">The new X-coordinate.</param>
		/// <param name="y">The new Y-coordinate.</param>
		/// <param name="width">The new width of the control.</param>
		/// <param name="height">The new height of the control.</param>
		/// <param name="specified">Which bounds are specified.</param>
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified) {
			if (AutoSize) {
				Size size = GetAutoSize();
				if (GetAutoSizeMode() == AutoSizeMode.GrowAndShrink) {
					width = size.Width;
					height = size.Height;
				} else {
					width = Math.Max(width, size.Width);
					height = Math.Max(height, size.Height);
				}
				specified |= BoundsSpecified.Size;
			}
			Size oldSize = Size;
			base.SetBoundsCore(x, y, width, height, specified);
			if (!(width == oldSize.Width && height == oldSize.Height))
				RefreshLayout();
		}

		/// <summary>
		/// Rearranges the layout of the checkbox and label appropriately within the control bounds.
		/// </summary>
		public void RefreshLayout() {
			Size clientSize = ClientSize;
			if (clientSize.Width <= 0 || clientSize.Height <= 0)
				return;
			int checkBoxWidth = GetCheckBoxSize().Width;
			checkBox = new Rectangle(Padding.Left + 1, (clientSize.Height - checkBoxWidth) / 2, checkBoxWidth, checkBoxWidth);
			Label.AutoSize = false;
			if (addPaddingRegardless || showCheckBox || icon != null || image != null) {
				int left = Padding.Left + 1 + checkBoxWidth + checkBoxWidth / 5;
				Padding padding = Padding;
				Label.Padding = new Padding(2, padding.Top, padding.Right, padding.Bottom);
				Label.Bounds = new Rectangle(left, 0, Math.Max(1, clientSize.Width - left), clientSize.Height);
			} else {
				Label.Padding = Padding;
				Label.Bounds = new Rectangle(Point.Empty, clientSize);
			}
			Invalidate(false);
		}

		/// <summary>
		/// Does nothing.
		/// </summary>
		protected override void InitLayout() {
		}
		/// <summary>
		/// Does nothing.
		/// </summary>
		protected override void OnLayout(LayoutEventArgs e) {
		}

		/// <summary>
		/// Called when the image has been changed.
		/// </summary>
		/// <param name="e">Empty.</param>
		protected virtual void OnImageChanged(EventArgs e) {
			EventHandler imageChanged = ImageChanged;
			if (imageChanged != null)
				imageChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Called when the icon has been changed.
		/// </summary>
		/// <param name="e">Empty.</param>
		protected virtual void OnIconChanged(EventArgs e) {
			EventHandler iconChanged = IconChanged;
			if (iconChanged != null)
				iconChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Called when the padding of the control is changed.
		/// </summary>
		/// <param name="e">Ignored.</param>
		protected override void OnPaddingChanged(EventArgs e) {
			base.OnPaddingChanged(e);
			RefreshLayout();
		}

		/// <summary>
		/// Fired when auto-size is changed.
		/// </summary>
		protected override void OnAutoSizeChanged(EventArgs e) {
			base.OnAutoSizeChanged(e);
			CheckAutoSize();
		}

		private void CheckAutoSize() {
			if (Label == null)
				return;
			if (AutoSize) {
				Size size = GetAutoSize();
				if (GetAutoSizeMode() != AutoSizeMode.GrowAndShrink)
					size = new Size(Math.Max(Width, size.Width), Math.Max(Height, size.Height));
				if (size == Size)
					RefreshLayout();
				else
					Size = size;
			} else
				RefreshLayout();
		}

		/// <summary>
		/// Sets the size of the control to the autosize result.
		/// </summary>
		public virtual void FitToContent() {
			Size = GetAutoSize();
		}

		/// <summary>
		/// Called when the backcolor is changed.
		/// </summary>
		protected override void OnBackColorChanged(EventArgs e) {
			base.OnBackColorChanged(e);
			brush.DisposeSafe();
			brush = new SolidBrush(BackColor);
		}

		/// <summary>
		/// Fired when the check state is changed.
		/// </summary>
		/// <param name="state">The new check-state.</param>
		protected virtual void OnCheckStateChanged(CheckState state) {
			Renderer.CheckState = state;
			Action<CheckState> handler = CheckStateChanged;
			if (handler != null)
				handler(state);
		}

		/// <summary>
		/// Fired when the mouse leaves the control.
		/// </summary>
		protected override void OnMouseLeave(EventArgs e) {
			base.OnMouseLeave(e);
			Renderer.MarkMouseHasLeft();
			isInside = false;
		}

		/// <summary>
		/// Fired when the mouse is pressed on the control.
		/// </summary>
		protected override void OnMouseDown(MouseEventArgs mevent) {
			base.OnMouseDown(mevent);
			if (mevent.Button == MouseButtons.Left) {
				Capture = true;
				Focus();
				Renderer.Pressed = true;
				wasPressed = true;
			}
		}

		/// <summary>
		/// Fired when the mouse is moved.
		/// </summary>
		protected override void OnMouseMove(MouseEventArgs mevent) {
			base.OnMouseMove(mevent);
			bool nowInside = Size.Contains(mevent.Location);
			if (nowInside && !isInside) {
				Renderer.MouseHovering = true;
				isInside = true;
				if (wasPressed)
					Renderer.Pressed = true;
			} else if (!nowInside && isInside)
				OnMouseLeave(mevent);
		}

		/// <summary>
		/// Fires when the mouse is released on the control.
		/// </summary>
		protected override void OnMouseUp(MouseEventArgs mevent) {
			if (mevent.Button == MouseButtons.Left) {
				Capture = false;
				Renderer.Pressed = false;
				wasPressed = false;
				if (CheckOnClick && Size.Contains(mevent.Location) && (showCheckBox || image != null || icon != null))
					Checked = !Checked;
			}
			base.OnMouseUp(mevent);
		}

		/// <summary>
		/// Fired when a key is released on the control.
		/// </summary>
		protected override void OnKeyUp(KeyEventArgs e) {
			base.OnKeyUp(e);
			if (CheckOnClick && (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter) && (showCheckBox || image != null || icon != null))
				Checked = !Checked;
		}

		/// <summary>
		/// Fired when the check box gets input focus.
		/// </summary>
		protected override void OnGotFocus(EventArgs e) {
			HasFocus = true;
			Renderer.NormalInnerBorderWidth = FocusInnerBorderWidth;
			Renderer.HoverInnerBorderWidth = FocusInnerBorderWidth;
			base.OnGotFocus(e);
		}

		/// <summary>
		/// Fires when the check box loses input focus.
		/// </summary>
		protected override void OnLostFocus(EventArgs e) {
			HasFocus = false;
			Renderer.NormalInnerBorderWidth = NormalInnerBorderWidth;
			Renderer.HoverInnerBorderWidth = NormalInnerBorderWidth;
			base.OnLostFocus(e);
		}

		/// <summary>
		/// Draws the check box on the specified Graphics object.
		/// </summary>
		/// <param name="g">The graphics object.</param>
		public void DrawGdi(Graphics g) {
			DrawGdi(g, Location);
		}

		/// <summary>
		/// Draws the check box on the specified Graphics object.
		/// </summary>
		/// <param name="g">The graphics object.</param>
		/// <param name="location">The location to draw at.</param>
		/// <param name="drawChildren">Whether to draw the child controls</param>
		public virtual void DrawGdi(Graphics g, Point location, bool drawChildren = true) {
			Rectangle rect = new Rectangle(location, ClientSize);
			if (BackColor.A != 0)
				g.FillRectangle(brush, rect);
			Label.DrawGdi(g, Label.Location + (Size) location);
			Icon icon = Icon;
			Rectangle checkBounds = checkBox;
			checkBounds.X += location.X;
			checkBounds.Y += location.Y;
			if (icon == null) {
				Image image = Image;
				if (image == null) {
					if (showCheckBox)
						Renderer.RenderCheckBox(g, checkBounds, Enabled, isRadioButton);
				} else {
					g.PixelOffsetMode = PixelOffsetMode.HighQuality;
					g.CompositingQuality = CompositingQuality.HighQuality;
					g.DrawImage(image, checkBounds);
				}
			} else {
				g.PixelOffsetMode = PixelOffsetMode.HighQuality;
				g.CompositingQuality = CompositingQuality.HighQuality;
				g.DrawIcon(icon, checkBounds);
			}
		}

		/// <summary>
		/// Not implemented yet.
		/// Draws the control with its children in the current OpenGL context (assumes the GL matrix is set to orthographic and maps to pixel coordinates).
		/// </summary>
		/// <param name="location">The location to draw at</param>
		/// <param name="drawChildren">Whether to draw the child controls</param>
		public virtual void DrawGL(Point location, bool drawChildren) {
			throw new NotImplementedException(nameof(DrawGL) + " is not implemented.");
		}

		/// <summary>
		/// Paints the check box
		/// </summary>
		protected override void OnPaint(PaintEventArgs pevent) {
			DrawGdi(pevent.Graphics, Point.Empty, false);
			RaisePaintEvent(StyleRenderer.PaintEventKey, pevent);
		}

		/// <summary>
		/// Gets a string that describes this instance
		/// </summary>
		public override string ToString() {
			return nameof(StyledCheckBox) + ": { Name: " + Name + ", Text: " + Text.Replace('\v', '\n') + " }";
		}

		/// <summary>
		/// Called when the checkbox is being disposed.
		/// </summary>
		/// <param name="disposing">Whether to dispose managed resources.</param>
		protected override void Dispose(bool disposing) {
			Renderer.Dispose();
			Label.Dispose();
			brush.DisposeSafe();
			base.Dispose(disposing);
		}
	}
}