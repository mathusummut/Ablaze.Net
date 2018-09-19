using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Reflection;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms {
	/// <summary>
	/// A customizable styled button.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxItemFilter("System.Windows.Forms")]
	[DesignTimeVisible(true)]
	[DesignerCategory("CommonControls")]
	[Description("A customizable styled button.")]
	[DisplayName(nameof(StyledButton))]
	[DefaultEvent(nameof(Click))]
	public class StyledButton : Button, ISmartControl {
		private static object EventKeyDown = typeof(Control).GetField(nameof(EventKeyDown), BindingFlags.Static | BindingFlags.NonPublic);
		private static object EventKeyUp = typeof(Control).GetField(nameof(EventKeyUp), BindingFlags.Static | BindingFlags.NonPublic);
		private UIAnimationHandler animationHandler;
		/// <summary>
		/// Fired when the image has been changed.
		/// </summary>
		public event EventHandler ImageChanged;
		/// <summary>
		/// Fired when the icon has been changed.
		/// </summary>
		public event EventHandler IconChanged;
		/// <summary>
		/// Fired when the check state is changed.
		/// </summary>
		public event Action<CheckState> CheckStateChanged;
		private StyledCheckBox checkBox = new StyledCheckBox();
		private ImageLayout backgroundLayout = ImageLayout.Stretch;
		private FieldOrProperty forecolorProperty;
		private bool wasPressed, isInside;

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
		/// Gets a dummy layout engine.
		/// </summary>
		[Browsable(false)]
		public override LayoutEngine LayoutEngine {
			get {
				return DummyLayoutEngine.Instance;
			}
		}

		/// <summary>
		/// Gets the checkbox that renders the text.
		/// </summary>
		[Description("Gets the checkbox that renders the text.")]
		public StyledCheckBox CheckBox {
			get {
				return checkBox;
			}
		}

		/// <summary>
		/// Gets or sets the icon to show on the left of the item. Icon takes precendence over Image.
		/// </summary>
		[Description("Gets or sets the icon to show.")]
		[DefaultValue(null)]
		public Icon Icon {
			get {
				return CheckBox.Icon;
			}
			set {
				CheckBox.Icon = value;
				OnIconChanged(EventArgs.Empty);
				Invalidate();
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
		/// Gets or sets the text color to use when the button is in normal state.
		/// </summary>
		[Description("Gets or sets the text color to use when the button is in normal state.")]
		[DefaultValue(typeof(Color), "0x000000")]
		public Color NormalTextColor {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the text color to use when the button is in hovered state.
		/// </summary>
		[Description("Gets or sets the text color to use when the button is in hovered state.")]
		[DefaultValue(typeof(Color), "0x000000")]
		public Color HoverTextColor {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the text color to use when the button is in pressed state.
		/// </summary>
		[Description("Gets or sets the text color to use when the button is in pressed state.")]
		[DefaultValue(typeof(Color), "0x000000")]
		public Color PressedTextColor {
			get;
			set;
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
		/// Gets or sets the text to show in the button label.
		/// </summary>
		[Description("Gets or sets the text to show in the button label.")]
		public override string Text {
			get {
				return CheckBox.Text;
			}
			set {
				if (value == null)
					value = string.Empty;
				string text = CheckBox.Text;
				CheckBox.Text = CheckBox.Label.ReplaceTabs(value.Replace("\r", string.Empty));
				if (text == CheckBox.Text)
					return;
				OnTextChanged(EventArgs.Empty);
				CheckAutoSize();
			}
		}

		/// <summary>
		/// Gets or sets the button image.
		/// </summary>
		[Description("Gets or sets the button image.")]
		[DefaultValue(null)]
		public new Image Image {
			get {
				return CheckBox.Image;
			}
			set {
				CheckBox.Image = value;
				OnImageChanged(EventArgs.Empty);
			}
		}

		/// <summary>
		/// Gets or sets the background image.
		/// </summary>
		[Description("Gets or sets the background image.")]
		public override Image BackgroundImage {
			get {
				return base.BackgroundImage;
			}

			set {
				base.BackgroundImage = value;
				Invalidate(false);
			}
		}

		/// <summary>
		/// Gets or sets the check-state of the check box.
		/// </summary>
		[Description("Gets or sets the check-state of the check box.")]
		[DefaultValue(0)]
		public CheckState CheckState {
			get {
				return CheckBox.CheckState;
			}
			set {
				if (value == CheckBox.CheckState)
					return;
				CheckBox.CheckState = value;
				OnCheckStateChanged(CheckBox.CheckState);
			}
		}

		/// <summary>
		/// Gets or sets whether the check box is checked or not.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool Checked {
			get {
				return CheckBox.Checked;
			}
			set {
				if (value == Checked)
					return;
				CheckBox.Checked = value;
				OnCheckStateChanged(CheckBox.CheckState);
			}
		}

		/// <summary>
		/// Gets or sets the background image layout.
		/// </summary>
		[Description("Gets or sets the background image layout.")]
		[DefaultValue(3)]
		public override ImageLayout BackgroundImageLayout {
			get {
				return backgroundLayout;
			}

			set {
				if (backgroundLayout == value)
					return;
				backgroundLayout = value;
				if (BackgroundImage != null)
					Invalidate(false);
			}
		}

		/// <summary>
		/// Gets or sets the text alignment within the control.
		/// </summary>
		[Description("Gets or sets the text alignment within the control.")]
		public override ContentAlignment TextAlign {
			get {
				return CheckBox.TextAlign;
			}
			set {
				CheckBox.TextAlign = value;
			}
		}

		/// <summary>
		/// Gets the default item padding (3 pixels from every edge).
		/// </summary>
		[Browsable(false)]
		protected override Padding DefaultPadding {
			get {
				return new Padding(3);
			}
		}

		/// <summary>
		/// Gets or sets whether the item is checked when it is clicked.
		/// </summary>
		[Description("Gets or sets whether the item is checked when it is clicked.")]
		[DefaultValue(false)]
		public bool CheckOnClick {
			get {
				return CheckBox.CheckOnClick;
			}
			set {
				CheckBox.CheckOnClick = value;
				if (value)
					ShowCheckBox = true;
			}
		}

		/// <summary>
		/// Gets or sets whether a checkbox is shown.
		/// </summary>
		[Description("Gets or sets whether a checkbox is shown.")]
		[DefaultValue(false)]
		public bool ShowCheckBox {
			get {
				return CheckBox.ShowCheckBox;
			}
			set {
				if (value == CheckBox.ShowCheckBox)
					return;
				CheckBox.ShowCheckBox = value;
				FitToContent();
			}
		}

		/// <summary>
		/// Gets or sets the text color.
		/// </summary>
		[Description("Gets or sets the text color.")]
		[DefaultValue(typeof(Color), "0x000000")]
		public override Color ForeColor {
			get {
				return CheckBox.ForeColor;
			}
			set {
				if (value == CheckBox.ForeColor)
					return;
				CheckBox.ForeColor = value;
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
				return CheckBox.Font;
			}
			set {
				if (value == CheckBox.Font)
					return;
				CheckBox.Font = value;
				CheckAutoSize();
			}
		}

		/// <summary>
		/// Gets or sets whether to add checkbox padding to the control for context menu alignment.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool AddPaddingRegardless {
			get {
				return CheckBox.AddPaddingRegardless;
			}
			set {
				if (value == CheckBox.AddPaddingRegardless)
					return;
				CheckBox.AddPaddingRegardless = value;
				FitToContent();
			}
		}

		/// <summary>
		/// Gets or sets the Dpi to use for AutoSize calculation.
		/// </summary>
		[Description("Gets or sets the Dpi to use for AutoSize calculation.")]
		[DefaultValue(typeof(SizeF), "96, 96")]
		public SizeF Dpi {
			get {
				return CheckBox.Dpi;
			}
			set {
				if (value.Width <= 0f)
					value.Width = 96f;
				if (value.Height <= 0f)
					value.Height = 96f;
				if (value == Dpi)
					return;
				CheckBox.Dpi = value;
				CheckAutoSize();
			}
		}

		/// <summary>
		/// Gets or sets the text rendering hint.
		/// </summary>
		[Description("Gets or sets the text rendering hint.")]
		[DefaultValue((int) TextRenderingHint.ClearTypeGridFit)]
		public TextRenderingHint TextRenderingStyle {
			get {
				return CheckBox.TextRenderingStyle;
			}
			set {
				CheckBox.TextRenderingStyle = value;
				Invalidate(false);
			}
		}

		/// <summary>
		/// Initializes the styled button.
		/// </summary>
		public StyledButton() : this(null) {
		}

		/// <summary>
		/// Initializes the styled button with the specified text.
		/// </summary>
		/// <param name="text">The text to initialize the button width.</param>
		public StyledButton(string text) {
			CheckForIllegalCrossThreadCalls = false;
			SetStyle(ControlStyles.ResizeRedraw, false);
			forecolorProperty = new FieldOrProperty(nameof(ForeColor), this);
			SetAutoSizeMode(AutoSizeMode.GrowAndShrink);
			NormalTextColor = Color.Black;
			HoverTextColor = Color.Black;
			PressedTextColor = Color.Black;
			NormalInnerBorderWidth = 1f;
			FocusInnerBorderWidth = 2f;
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.UserMouse | ControlStyles.CacheText, true);
			Name = nameof(StyledButton);
			checkBox.AutoSize = false;
			checkBox.ShowCheckBox = false;
			checkBox.CheckOnClick = false;
			checkBox.BackColor = Color.Transparent;
			checkBox.TextAlign = ContentAlignment.MiddleCenter;
			checkBox.TextRenderingStyle = TextRenderingHint.ClearTypeGridFit;
			checkBox.Padding = DefaultPadding;
			if (text != null)
				checkBox.Text = text;
			animationHandler = UIAnimator.GetFunctionToInvalidateControlOnUpdate(this, false);
			Renderer = new StyleRenderer(animationHandler);
			Renderer.NormalInnerBorderColor = Color.FromArgb(177, 240, 245);
			if (!StyledForm.DesignMode)
				Renderer.CheckColors += Renderer_CheckColors;
		}

		private void Renderer_CheckColors() {
			if (Renderer.Pressed) {
				if (ForeColor != PressedTextColor)
					UIAnimator.SharedAnimator.Animate(forecolorProperty, PressedTextColor, Renderer.AnimationSpeed, 2.0, true, animationHandler, false);
			} else if (Renderer.MouseHovering) {
				if (ForeColor != HoverTextColor)
					UIAnimator.SharedAnimator.Animate(forecolorProperty, HoverTextColor, Renderer.AnimationSpeed, 2.0, true, animationHandler, false);
			} else {
				if (ForeColor != NormalTextColor)
					UIAnimator.SharedAnimator.Animate(forecolorProperty, NormalTextColor, Renderer.AnimationSpeed, 2.0, true, animationHandler, false);
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
		/// Gets the resultant size of the button if it were to be auto-sized with both grow and shrink enabled.
		/// </summary>
		public Size GetAutoSize() {
			return GetAutoSize(MaximumSize, true);
		}

		/// <summary>
		/// Calls GetAutoSize()
		/// </summary>
		/// <param name="proposedSize">The maximum size of the control</param>
		public override Size GetPreferredSize(Size proposedSize) {
			return GetAutoSize(proposedSize, true);
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
		/// Called when the padding has been changed.
		/// </summary>
		/// <param name="e">Ignored.</param>
		protected override void OnPaddingChanged(EventArgs e) {
			base.OnPaddingChanged(e);
			CheckBox.Padding = Padding;
			CheckAutoSize();
		}

		/// <summary>
		/// Gets the resultant size of the button if it were to be auto-sized with both grow and shrink enabled.
		/// </summary>
		/// <param name="maxBounds">The maximum size to allow (0 means unlimited).</param>
		///<param name="includePadding">If true, padding is included within the size.</param>
		public virtual Size GetAutoSize(Size maxBounds, bool includePadding) {
			return CheckBox.GetAutoSize(Size.Empty, includePadding);
		}

		/// <summary>
		/// Paints the button background (unimplemented).
		/// </summary>
		protected override void OnPaintBackground(PaintEventArgs pevent) {
		}

		/// <summary>
		/// Fired when the mouse enters the control.
		/// </summary>
		protected override void OnMouseEnter(EventArgs e) {
			base.OnMouseEnter(e);
			Renderer.MouseHovering = true;
			if (wasPressed)
				Renderer.Pressed = true;
		}

		/// <summary>
		/// Fired when the mouse leaves the control.
		/// </summary>
		protected override void OnMouseLeave(EventArgs e) {
			base.OnMouseLeave(e);
			Renderer.MarkMouseHasLeft();
		}

		/// <summary>
		/// Fired when the mouse is pressed on the control.
		/// </summary>
		protected override void OnMouseDown(MouseEventArgs mevent) {
			base.OnMouseDown(mevent);
			if (mevent.Button == MouseButtons.Left) {
				Capture = true;
				Renderer.Pressed = true;
				wasPressed = true;
				Focus();
			}
		}

		/// <summary>
		/// Fired when the mouse is moved.
		/// </summary>
		protected override void OnMouseMove(MouseEventArgs mevent) {
			base.OnMouseMove(mevent);
			bool nowInside = Size.Contains(mevent.Location);
			if (nowInside && !isInside)
				OnMouseEnter(mevent);
			else if (!nowInside && isInside)
				OnMouseLeave(mevent);
			isInside = nowInside;
		}

		/// <summary>
		/// Fires when the mouse is released on the control.
		/// </summary>
		protected override void OnMouseUp(MouseEventArgs mevent) {
			base.OnMouseUp(mevent);
			if (mevent.Button == MouseButtons.Left) {
				Capture = false;
				Renderer.Pressed = false;
				wasPressed = false;
				if (CheckOnClick && isInside) {
					Checked = !Checked;
					if (CheckState != CheckState.Unchecked)
						ShowCheckBox = true;
					Invalidate();
				}
			}
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

		private void CheckAutoSize() {
			if (AutoSize) {
				Size size = GetAutoSize();
				Size currentSize = Size;
				if (GetAutoSizeMode() == AutoSizeMode.GrowOnly)
					size = new Size(Math.Max(currentSize.Width, size.Width), Math.Max(currentSize.Height, size.Height));
				Size minimum = MinimumSize;
				if (minimum.Width > 0 && size.Width < minimum.Width)
					size.Width = minimum.Width;
				if (minimum.Height > 0 && size.Height < minimum.Height)
					size.Height = minimum.Height;
				if (size == currentSize)
					Invalidate(false);
				else
					Size = size;
			} else
				Invalidate(false);
		}

		/// <summary>
		/// Fired when auto-size is changed.
		/// </summary>
		protected override void OnAutoSizeChanged(EventArgs e) {
			base.OnAutoSizeChanged(e);
			CheckAutoSize();
		}

		/// <summary>
		/// Sets the size of the control to the autosize result.
		/// </summary>
		public virtual void FitToContent() {
			Size = GetAutoSize();
		}

		/// <summary>
		/// Fires when the button got input focus.
		/// </summary>
		protected override void OnGotFocus(EventArgs e) {
			Renderer.NormalInnerBorderWidth = FocusInnerBorderWidth;
			Renderer.HoverInnerBorderWidth = FocusInnerBorderWidth;
			base.OnGotFocus(e);
		}

		/// <summary>
		/// Fires when the button lost input focus.
		/// </summary>
		protected override void OnLostFocus(EventArgs e) {
			Renderer.NormalInnerBorderWidth = NormalInnerBorderWidth;
			Renderer.HoverInnerBorderWidth = NormalInnerBorderWidth;
			base.OnLostFocus(e);
		}

		/// <summary>
		/// Called when the enabled state is changed.
		/// </summary>
		protected override void OnEnabledChanged(EventArgs e) {
			Renderer.Enabled = Enabled;
			base.OnEnabledChanged(e);
		}

		/// <summary>
		/// Called when the size is changed.
		/// </summary>
		/// <param name="e">Nothing.</param>
		protected override void OnSizeChanged(EventArgs e) {
			base.OnSizeChanged(e);
			CheckBox.Size = Size;
		}

		/// <summary>
		/// Called when a key is pressed.
		/// </summary>
		/// <param name="kevent">The key event data.</param>
		protected override void OnKeyDown(KeyEventArgs kevent) {
			KeyEventHandler item = (KeyEventHandler) Events[EventKeyDown];
			if (item != null)
				item(this, kevent);
		}

		/// <summary>
		/// Called when a key is released.
		/// </summary>
		/// <param name="kevent">The key event data.</param>
		protected override void OnKeyUp(KeyEventArgs kevent) {
			KeyEventHandler item = (KeyEventHandler) Events[EventKeyUp];
			if (item != null)
				item(this, kevent);
		}

		/// <summary>
		/// Draws the button on the specified Graphics object
		/// </summary>
		/// <param name="g">The graphics object</param>
		public void DrawGdi(Graphics g) {
			DrawGdi(g, Location, true);
		}

		/// <summary>
		/// Draws the button on the specified Graphics object
		/// </summary>
		/// <param name="g">The graphics object</param>
		/// <param name="location">The coordinates to draw at</param>
		/// <param name="drawChildren">Whether to draw the child controls</param>
		public virtual void DrawGdi(Graphics g, Point location, bool drawChildren = true) {
			Renderer.RenderBackground(g, new RectangleF(location, Size), false, false, BackgroundImage, BackgroundImageLayout);
			CheckBox.DrawGdi(g, location);
			if (drawChildren)
				g.DrawControls(Controls, location, Rectangle.Ceiling(g.ClipBounds), drawChildren);
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
		/// Called when the bounds of the control is about to change.
		/// </summary>
		/// <param name="x">The new X-coordinate.</param>
		/// <param name="y">The new Y-coordinate.</param>
		/// <param name="width">The new width of the control.</param>
		/// <param name="height">The new height of the control.</param>
		/// <param name="specified">Which bounds are specified.</param>
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified) {
			Size oldSize = Size;
			if (AutoSize) {
				Size size = GetAutoSize();
				if (GetAutoSizeMode() == AutoSizeMode.GrowAndShrink) {
					width = size.Width;
					height = size.Height;
				} else {
					width = Math.Max(oldSize.Width, size.Width);
					height = Math.Max(oldSize.Height, size.Height);
				}
				specified |= BoundsSpecified.Size;
			}
			Size minimum = MinimumSize;
			Size maximum = MaximumSize;
			bool widthSpecified = (specified & BoundsSpecified.Width) == BoundsSpecified.Width;
			if (widthSpecified) {
				if (minimum.Width > 0 && width < minimum.Width)
					width = minimum.Width;
				if (maximum.Width > 0 && width > maximum.Width)
					width = maximum.Width;
			}
			bool heightSpecified = (specified & BoundsSpecified.Height) == BoundsSpecified.Height;
			if (heightSpecified) {
				if (minimum.Height > 0 && height < minimum.Height)
					height = minimum.Height;
				if (maximum.Height > 0 && height > maximum.Height)
					height = maximum.Height;
			}
			base.SetBoundsCore(x, y, width, height, specified);
			if ((widthSpecified && width != oldSize.Width) || (heightSpecified && height != oldSize.Height)) {
				CheckBox.Bounds = new Rectangle(0, 0, width, height);
				Invalidate(false);
			}
		}

		/// <summary>
		/// Paints the button
		/// </summary>
		protected override void OnPaint(PaintEventArgs pevent) {
			DrawGdi(pevent.Graphics, Point.Empty, false);
			RaisePaintEvent(StyleRenderer.PaintEventKey, pevent);
		}

		/// <summary>
		/// Gets a string that describes this instance.
		/// </summary>
		public override string ToString() {
			return nameof(StyledButton) + ": { Name: " + Name + ", Text: " + Text.Replace('\v', '\n') + " }";
		}

		/// <summary>
		/// Called when the button is being disposed.
		/// </summary>
		/// <param name="disposing">True to dispose managed resources.</param>
		protected override void Dispose(bool disposing) {
			Renderer.Dispose();
			CheckBox.Dispose();
			base.Dispose(disposing);
		}
	}
}