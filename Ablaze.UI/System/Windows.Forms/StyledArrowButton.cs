using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms {
	/// <summary>
	/// A styled arrow-shaped button.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxItemFilter("System.Windows.Forms")]
	[DesignTimeVisible(true)]
	[DesignerCategory("CommonControls")]
	[Description("A styled arrow-shaped button.")]
	[DisplayName(nameof(StyledArrowButton))]
	[DefaultEvent(nameof(Click))]
	public class StyledArrowButton : TransparentControl, IDrawable {
		private GraphicsPath graphicsPath = new GraphicsPath();
		private Brush brush = Brushes.Black;
		private FieldOrProperty forecolorProperty;
		private StyledLabel label = new StyledLabel();
		private ImageLayout backgroundLayout = ImageLayout.Stretch;
		private float rotation;
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
		/// Gets the label that renders the text.
		/// </summary>
		[Description("Gets the label that renders the text.")]
		public StyledLabel Label {
			get {
				return label;
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
		/// Gets or sets the text to show in the button label.
		/// </summary>
		[Description("Gets or sets the text to show in the button label.")]
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
				Invalidate(false);
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
				Invalidate(false);
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
		/// Gets or sets the arrow button pointing direction in degrees.
		/// </summary>
		[Description("Gets or sets the arrow button pointing direction in degrees.")]
		[DefaultValue(0f)]
		public float Rotation {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return rotation;
			}
			set {
				value %= 360f;
				if (value < 0f)
					value += 360f;
				if (value == rotation)
					return;
				rotation = value;
				RecalculatePoints();
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
				Invalidate(false);
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
		/// Gets a dummy layout engine.
		/// </summary>
		[Browsable(false)]
		public override LayoutEngine LayoutEngine {
			get {
				return DummyLayoutEngine.Instance;
			}
		}

		/// <summary>
		/// Initializes an arrow button.
		/// </summary>
		public StyledArrowButton() : this(null) {
		}

		/// <summary>
		/// Initializes the arrow button with the specified text.
		/// </summary>
		/// <param name="text">The text to initialize the button width.</param>
		public StyledArrowButton(string text) {
			CheckForIllegalCrossThreadCalls = false;
			SetStyle(ControlStyles.ResizeRedraw, false);
			forecolorProperty = new FieldOrProperty(nameof(ForeColor), this);
			NormalTextColor = Color.Black;
			HoverTextColor = Color.Black;
			PressedTextColor = Color.Black;
			label.BackColor = Color.Transparent;
			label.TextAlign = ContentAlignment.MiddleCenter;
			label.TextRenderingStyle = TextRenderingHint.ClearTypeGridFit;
			label.RenderShadow = false;
			Name = nameof(StyledArrowButton);
			if (text != null)
				label.Text = text;
			Renderer = new StyleRenderer(UIAnimator.GetFunctionToInvalidateControlOnUpdate(this, false)) {
				SuppressColorChecking = true,
				SuppressFunctionCallOnRefresh = true,
				RoundCornerRadius = 0f,
				RoundedEdges = false,
				NormalInnerBorderColor = Color.FromArgb(235, 229, 198),
				HoverInnerBorderColor = Color.FromArgb(245, 207, 57)
			};
			if (!StyledForm.DesignMode)
				Renderer.CheckColors += Renderer_CheckColors;
			Renderer.SuppressColorChecking = false;
			Renderer.SuppressFunctionCallOnRefresh = false;
		}

		private void RecalculatePoints() {
			Size size = Size;
			float dx = size.Width - 3;
			float dy = size.Height - 3;
			PointF[] points = new PointF[] { new PointF(-dx * 0.25f, dy * 0.5f), new PointF(dx * 0.25f, dy * 0.5f), new PointF(dx * 0.25f, 0f), new PointF(dx * 0.5f, 0f), new PointF(0f, -dy * 0.5f), new PointF(-dx * 0.5f, 0f), new PointF(-dx * 0.25f, 0f), new PointF(-dx * 0.25f, dy * 0.5f) };
			PointF centerPoint = new PointF(size.Width * 0.5f, size.Height * 0.5f);
			float bog = Maths.DegToRadF * rotation;
			float cosA = (float) Math.Cos(bog);
			float sinA = (float) Math.Sin(bog);
			float a, b;
			int i;
			for (i = 0; i < 8; i++) {
				a = points[i].X;
				b = points[i].Y;
				points[i].X = a * cosA - b * sinA;
				points[i].Y = b * cosA + a * sinA;
			}
			float xMax = points[0].X, yMax = points[0].Y;
			float xMin = xMax, yMin = yMax;
			for (i = 1; i < 8; i++) {
				if (points[i].X > xMax)
					xMax = points[i].X;
				else if (points[i].X < xMin)
					xMin = points[i].X;
				if (points[i].Y > yMax)
					yMax = points[i].Y;
				else if (points[i].Y < yMin)
					yMin = points[i].Y;
			}
			float minMultiplierX = (1f - centerPoint.X) / xMin;
			float minMultiplierY = (1f - centerPoint.Y) / yMin;
			float maxMultiplierX = (centerPoint.X - 1f) / xMax;
			float maxMultiplierY = (centerPoint.Y - 1f) / yMax;
			for (i = 0; i < 8; i++) {
				points[i].X *= points[i].X < 0 ? minMultiplierX : maxMultiplierX;
				points[i].Y *= points[i].Y < 0 ? minMultiplierY : maxMultiplierY;
			}
			graphicsPath.Reset();
			for (i = 0; i < points.Length - 1; i++)
				graphicsPath.AddLine(centerPoint.X + points[i].X, centerPoint.Y + points[i].Y, centerPoint.X + points[i + 1].X, centerPoint.Y + points[i + 1].Y);
			Region = new Region(graphicsPath);
			Invalidate(false);
		}

		private void Renderer_CheckColors() {
			if (Renderer.Pressed) {
				if (ForeColor != PressedTextColor)
					UIAnimator.SharedAnimator.Animate(forecolorProperty, PressedTextColor, Renderer.AnimationSpeed, 2.0, true, UIAnimator.GetFunctionToInvalidateControlOnUpdate(this), false);
			} else if (Renderer.MouseHovering) {
				if (ForeColor != HoverTextColor)
					UIAnimator.SharedAnimator.Animate(forecolorProperty, HoverTextColor, Renderer.AnimationSpeed, 2.0, true, UIAnimator.GetFunctionToInvalidateControlOnUpdate(this), false);
			} else {
				if (ForeColor != NormalTextColor)
					UIAnimator.SharedAnimator.Animate(forecolorProperty, NormalTextColor, Renderer.AnimationSpeed, 2.0, true, UIAnimator.GetFunctionToInvalidateControlOnUpdate(this), false);
			}
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
		/// Does nothing
		/// </summary>
		protected override void InitLayout() {
		}
		/// <summary>
		/// Does nothing
		/// </summary>
		protected override void OnLayout(LayoutEventArgs e) {
		}

		/// <summary>
		/// Called when the padding has been changed.
		/// </summary>
		/// <param name="e">Ignored.</param>
		protected override void OnPaddingChanged(EventArgs e) {
			Label.Padding = Padding;
			base.OnPaddingChanged(e);
			Invalidate(false);
		}

		/// <summary>
		/// Called when the size has been changed.
		/// </summary>
		protected override void OnSizeChanged(EventArgs e) {
			base.OnSizeChanged(e);
			Label.Size = Size;
			RecalculatePoints();
		}

		/// <summary>
		/// Fired when the mouse enters the control.
		/// </summary>
		protected override void OnMouseEnter(EventArgs e) {
			base.OnMouseEnter(e);
			Renderer.MouseHovering = true;
			isInside = true;
			if (wasPressed)
				Renderer.Pressed = true;
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
		/// <param name="mevent">The mouse event args.</param>
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
		/// <param name="mevent">The mouse event args.</param>
		protected override void OnMouseMove(MouseEventArgs mevent) {
			base.OnMouseMove(mevent);
			bool nowInside = Size.Contains(mevent.Location);
			if (nowInside && !isInside)
				OnMouseEnter(mevent);
			else if (!nowInside && isInside)
				OnMouseLeave(mevent);
		}

		/// <summary>
		/// Fires when the mouse is released on the control.
		/// </summary>
		/// <param name="mevent">The mouse event args.</param>
		protected override void OnMouseUp(MouseEventArgs mevent) {
			base.OnMouseUp(mevent);
			if (mevent.Button == MouseButtons.Left) {
				Capture = false;
				Renderer.Pressed = false;
				wasPressed = false;
			}
		}

		/// <summary>
		/// Fired when a key is released.
		/// </summary>
		/// <param name="e">The key event args.</param>
		protected override void OnKeyUp(KeyEventArgs e) {
			base.OnKeyUp(e);
			if (e.KeyData == Keys.Enter || e.KeyData == Keys.Space)
				OnClick(e);
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
		/// Called when the enabled state is changed.
		/// </summary>
		protected override void OnEnabledChanged(EventArgs e) {
			Renderer.Enabled = Enabled;
			base.OnEnabledChanged(e);
		}

		/// <summary>
		/// Draws the button on the specified Graphics object.
		/// </summary>
		/// <param name="g">The graphics object.</param>
		public void DrawGdi(Graphics g) {
			DrawGdi(g, Location);
		}

		/// <summary>
		/// Draws the arrow button on the specified Graphics object
		/// </summary>
		/// <param name="g">The graphics object</param>
		/// <param name="location">The coordinates to draw at</param>
		/// <param name="drawChildren">Whether to draw the child controls</param>
		public virtual void DrawGdi(Graphics g, Point location, bool drawChildren = true) {
			if (!location.IsEmpty)
				g.TranslateTransform(location.X, location.Y);
			Rectangle rect = ClientRectangle;
			Region oldClipRegion = g.Clip;
			g.Clip = Region;
			if (BackColor.A != 0)
				g.FillRectangle(brush, rect);
			g.SmoothingMode = SmoothingMode.HighQuality;
			g.CompositingQuality = CompositingQuality.HighQuality;
			g.TextRenderingHint = Label.TextRenderingStyle;
			using (LinearGradientBrush b = Renderer.Invert ? new LinearGradientBrush(rect, Renderer.CurrentBackgroundBottom, Renderer.CurrentBackgroundTop, rotation, true)
				: new LinearGradientBrush(rect, Renderer.CurrentBackgroundTop, Renderer.CurrentBackgroundBottom, rotation, true)) {
				if (Renderer.BackgroundBlend != null)
					b.Blend = Renderer.BackgroundBlend;
				g.FillPath(b, graphicsPath);
			}
			using (Pen border = new Pen(Renderer.CurrentBorder, 1f))
				g.DrawPath(border, graphicsPath);
			if (Text.Length != 0)
				Label.DrawGdi(g);
			RaisePaintEvent(StyleRenderer.PaintEventKey, new PaintEventArgs(g, rect));
			if (drawChildren)
				g.DrawControls(Controls, Point.Empty, true);
			if (!location.IsEmpty)
				g.TranslateTransform(-location.X, -location.Y);
			g.Clip = oldClipRegion;
		}

		/// <summary>
		/// Not implemented yet.
		/// Draws the control with its children in the current OpenGL context (assumes the GL matrix is set to orthographic and maps to pixel coordinates)
		/// </summary>
		/// <param name="location">The location to draw at</param>
		/// <param name="drawChildren">Whether to draw the child controls</param>
		public virtual void DrawGL(Point location, bool drawChildren) {
			throw new NotImplementedException(nameof(DrawGL) + " is not implemented.");
		}

		/// <summary>
		/// Draws the arrow button
		/// </summary>
		protected override void OnPaint(PaintEventArgs pevent) {
			DrawGdi(pevent.Graphics, Point.Empty, false);
		}

		/// <summary>
		/// Rotates the text in the label to match the arrow direction in the most readable manner possible.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void RotateTextToMatchArrow() {
			Label.Rotation = rotation < 175f || rotation > 330f ? rotation - 90f : rotation + 90f;
		}

		/// <summary>
		/// Gets a string that describes this instance.
		/// </summary>
		public override string ToString() {
			return nameof(StyledArrowButton) + ": { Name: " + Name + ", Text: " + Text.Replace('\v', '\n') + " }";
		}

		/// <summary>
		/// Called when the button is disposed.
		/// </summary>
		/// <param name="disposing">Whether Dispose() was called manually or automatically.</param>
		protected override void Dispose(bool disposing) {
			base.Dispose(disposing);
			Label.Dispose();
			brush.DisposeSafe();
			graphicsPath.Dispose();
		}
	}
}