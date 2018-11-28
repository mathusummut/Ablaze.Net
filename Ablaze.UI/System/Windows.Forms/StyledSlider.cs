using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms {
	/// <summary>
	/// A slider that supports custom coloring and is more precise and flexible than the stock ScrollBar.
	/// </summary>
	[ToolboxItem(true)]
	[DesignTimeVisible(true)]
	[ToolboxItemFilter("System.Windows.Forms")]
	[DesignerCategory("CommonControls")]
	[Description("A slider that supports custom coloring and is more precise and flexible than the stock ScrollBar.")]
	[DisplayName(nameof(StyledSlider))]
	[DefaultEvent(nameof(ValueChanged))]
	public class StyledSlider : Control, IDrawable {
		/// <summary>
		/// Renders the slider knob
		/// </summary>
		public readonly StyleRenderer KnobRenderer;
		/// <summary>
		/// Renders the slider bar
		/// </summary>
		public readonly StyleRenderer BarRenderer;
		private float increment = 0f, knobSize = 14f, trackerValue = 50f, barMinimum = 1f, barMaximum = 100f, smallChange = 1f, largeChange = 5f;
		private StyledLabel label = new StyledLabel();
		private int labelPadding = 2, mouseWheelBarPartitions = 10;
		private bool wasPressed;
		/// <summary>
		/// Fired when the slider position has changed.
		/// </summary>
		[Description("Fired when the slider position has changed.")]
		public event EventHandler ValueChanged;

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
		/// Gets the label that renders the text
		/// </summary>
		[Description("Gets the label that renders the text")]
		public StyledLabel Label {
			get {
				return label;
			}
		}

		/// <summary>
		/// Gets or sets the padding that is added between the label and the slider
		/// </summary>
		[Description("Gets or sets the padding that is added between the label and the slider")]
		public int LabelPadding {
			get {
				return labelPadding;
			}
			set {
				if (labelPadding == value)
					return;
				labelPadding = value;
				Padding padding = label.Padding;
				if (Vertical)
					label.Padding = new Padding(padding.Left, padding.Top, padding.Right, labelPadding);
				else
					label.Padding = new Padding(padding.Left, padding.Top, labelPadding, padding.Bottom);
				Invalidate(false);
			}
		}

		/// <summary>
		/// Initializes the control as transparent.
		/// </summary>
		protected override CreateParams CreateParams {
			get {
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= (int) Platforms.Windows.ExtendedWindowStyle.Transparent;
				return cp;
			}
		}

		/// <summary>
		/// Gets or sets whether the label should resize to fit its content
		/// </summary>
		[Description("Gets or sets whether the label should resize to fit its content")]
		[DefaultValue(true)]
		public bool LabelAutoSize {
			get {
				return label.AutoSize;
			}
			set {
				if (value == label.AutoSize)
					return;
				label.AutoSize = value;
				Invalidate(false);
			}
		}

		private RectangleF KnobBoundsInner {
			get {
				Size clientSize = ClientSize;
				Size labelSize = label.Size;
				Padding padding = Padding;
				return Vertical ? new RectangleF(padding.Left + 1, ((trackerValue - barMinimum) * (clientSize.Height - (labelSize.Height + knobSize + label.Padding.Vertical + Padding.Bottom))) / (barMaximum - barMinimum), clientSize.Width - (padding.Horizontal + 2), knobSize) :
					new RectangleF(((trackerValue - barMinimum) * (clientSize.Width - (labelSize.Width + knobSize + label.Padding.Horizontal + Padding.Right))) / (barMaximum - barMinimum), padding.Top + 1, knobSize, clientSize.Height - (padding.Vertical + 2));
			}
		}

		/// <summary>
		/// Gets the boundary rectangle of the knob.
		/// </summary>
		[Browsable(false)]
		public RectangleF KnobBounds {
			get {
				RectangleF knobBoundsInner = KnobBoundsInner;
				if (Vertical)
					knobBoundsInner.Y += label.Height + label.Padding.Vertical;
				else
					knobBoundsInner.X += label.Width + label.Padding.Horizontal;
				return knobBoundsInner;
			}
		}

		/// <summary>
		/// Gets or sets the size of the knob.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public float KnobSize {
			get {
				return knobSize;
			}
			set {
				value = Math.Max(1f, Math.Min(value, Vertical ? ClientSize.Height - label.Padding.Vertical : ClientSize.Width - label.Padding.Horizontal));
				if (value == knobSize)
					return;
				knobSize = value;
				Invalidate(false);
			}
		}

		/// <summary>
		/// Gets or sets whether the orientation of the slider is vertical
		/// </summary>
		[Description("Gets or sets whether the orientation of the slider is vertical")]
		[DefaultValue(true)]
		public bool Vertical {
			get {
				return label.Vertical;
			}
			set {
				if (value == label.Vertical)
					return;
				Padding padding = Padding;
				if (value) {
					int width = ClientSize.Width;
					label.MaximumSize = new Size(width, 0);
					label.MinimumSize = new Size(width, 0);
					label.Padding = new Padding(padding.Left, padding.Top, padding.Right, labelPadding);
				} else {
					int height = ClientSize.Height;
					label.MaximumSize = new Size(0, height);
					label.MinimumSize = new Size(0, height);
					label.Padding = new Padding(padding.Left, padding.Top, labelPadding, padding.Bottom);
				}
				label.Vertical = value;
				Size size = Size;
				int temp = size.Width;
				size.Width = size.Height;
				size.Height = temp;
				Size = size;
				Invalidate(false);
			}
		}

		/// <summary>
		/// Gets or sets the increments supported by the slider (ex. 0 for no limit, 1 for whole numbers, 2 for even numbers...).
		/// </summary>
		[Description("Gets or sets the increments supported by the slider (ex. 1 for whole numbers, 2 for even numbers...).")]
		public float Increment {
			get {
				return increment;
			}
			set {
				value = Math.Abs(value);
				if (increment == value)
					return;
				increment = value;
				Value = trackerValue;
			}
		}

		/// <summary>
		/// Gets or sets the value of the slider.
		/// </summary>
		[Description("Gets or sets the value of the slider.")]
		[DefaultValue(50f)]
		public float Value {
			get {
				return trackerValue;
			}
			set {
				//Snap
				float minimum = barMinimum;
				float maximum = barMaximum;
				if (increment > float.Epsilon) {
					float diff = value - minimum;
					float remainder = diff % increment;
					if (remainder < increment * 0.5f)
						value -= remainder;
					else
						value += increment - remainder;
				}
				if (value < minimum)
					value = minimum;
				else if (value > maximum)
					value = maximum;
				if (value == trackerValue)
					return;
				trackerValue = value;
				RaiseValueChanged();
				Invalidate(false);
			}
		}

		/// <summary>
		/// Gets or sets the minimum value.
		/// </summary>
		[Description("Gets or sets the minimum value.")]
		[DefaultValue(0f)]
		public float Minimum {
			get {
				return barMinimum;
			}
			set {
				if (value > barMaximum)
					value = barMaximum;
				if (value == barMinimum)
					return;
				barMinimum = value;
				Value = trackerValue;
			}
		}

		/// <summary>
		/// Gets or sets the maximum value.
		/// </summary>
		[Description("Gets or sets the maximum value.")]
		[DefaultValue(100f)]
		public float Maximum {
			get {
				return barMaximum;
			}
			set {
				if (value < barMinimum)
					value = barMinimum;
				if (value == barMaximum)
					return;
				barMaximum = value;
				Value = trackerValue;
			}
		}

		/// <summary>
		/// Gets or sets change when directional keys are pressed.
		/// </summary>
		[Description("Gets or sets change when directional keys are pressed.")]
		[DefaultValue(1f)]
		public float SmallChange {
			get {
				return smallChange;
			}
			set {
				smallChange = Math.Abs(value);
			}
		}

		/// <summary>
		/// Gets or sets change when Page Up/Down keys are pressed.
		/// </summary>
		[Description("Gets or sets change when Page Up/Down keys are pressed.")]
		[DefaultValue(5f)]
		public float LargeChange {
			get {
				return largeChange;
			}
			set {
				largeChange = Math.Abs(value);
			}
		}

		/// <summary>
		/// Gets or sets the number of mouse wheel bar partitions.
		/// </summary>
		[Description("Gets or sets the number of mouse wheel bar partitions.")]
		[DefaultValue(10)]
		public int MouseWheelBarPartitions {
			get {
				return mouseWheelBarPartitions;
			}
			set {
				if (value < 1)
					value = 1;
				mouseWheelBarPartitions = value;
			}
		}

		/// <summary>
		/// Gets or sets the text to show in the slider label.
		/// </summary>
		[Description("Gets or sets the text to show in the slider label.")]
		public override string Text {
			get {
				return label.Text;
			}
			set {
				if (value == null)
					value = string.Empty;
				string text = label.Text;
				label.Text = label.ReplaceTabs(value.Replace("\r", string.Empty));
				if (text == label.Text)
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
				return label.Dpi;
			}
			set {
				if (value.Width <= 0f)
					value.Width = 96f;
				if (value.Height <= 0f)
					value.Height = 96f;
				if (value == Dpi)
					return;
				label.Dpi = value;
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
				return label.TextRenderingStyle;
			}
			set {
				label.TextRenderingStyle = value;
				Invalidate(false);
			}
		}

		/// <summary>
		/// Gets the default padding of the control (1 pixel from each edge).
		/// </summary>
		protected override Padding DefaultPadding {
			get {
				return new Padding(1);
			}
		}

		/// <summary>
		/// Gets or sets the text color.
		/// </summary>
		[Description("Gets or sets the text color.")]
		[DefaultValue(typeof(Color), "0x000000")]
		public override Color ForeColor {
			get {
				return label.ForeColor;
			}
			set {
				if (value == label.ForeColor)
					return;
				label.ForeColor = value;
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
				return label.Font;
			}
			set {
				if (value == label.Font)
					return;
				label.Font = value;
				Invalidate(false);
			}
		}

		/// <summary>
		/// Initializes a new instance of a Slider control with default values.
		/// </summary>
		public StyledSlider() : this(0f, 100f, 50f) {
		}

		/// <summary>
		/// Initializes a new instance of a Slider control.
		/// </summary>
		/// <param name="min">The minimum value.</param>
		/// <param name="max">The maximum value.</param>
		/// <param name="value">The current value.</param>
		/// <param name="increment">The increments supported by the slider (ex. 0 for no limit, 1 for whole numbers, 2 for even numbers...).</param>
		public StyledSlider(float min, float max, float value, float increment = 0f) {
			CheckForIllegalCrossThreadCalls = false;
			float diff = Math.Abs(max - min);
			smallChange = diff * 0.05f;
			largeChange = diff * 0.125f;
			Name = nameof(StyledSlider);
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.Selectable | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint | ControlStyles.UserMouse | ControlStyles.CacheText, true);
			SetStyle(ControlStyles.ResizeRedraw, false);
			label.BackColor = Color.Transparent;
			label.Padding = new Padding(1, 1, labelPadding, 1);
			label.RenderShadow = false;
			label.TextAlign = ContentAlignment.MiddleLeft;
			label.Text = nameof(StyledSlider);
			label.AutoSize = true;
			UIAnimationHandler function = UIAnimator.GetFunctionToInvalidateControlOnUpdate(this);
			KnobRenderer = new StyleRenderer(function);
			KnobRenderer.Border = Color.FromArgb(65, 65, 65);
			BarRenderer = new StyleRenderer(function);
			BarRenderer.SuppressColorChecking = true;
			BarRenderer.SuppressFunctionCallOnRefresh = true;
			BarRenderer.Border = Color.FromArgb(65, 65, 65);
			BarRenderer.NormalBackgroundTop = ImageLib.ChangeLightness(Color.LightBlue, 40);
			BarRenderer.NormalBackgroundBottom = Color.LightSteelBlue;
			BarRenderer.HoverBackgroundTop = ImageLib.ChangeLightness(Color.LightSteelBlue, 30);
			BarRenderer.HoverBackgroundBottom = Color.AliceBlue;
			BarRenderer.PressedBackgroundTop = ImageLib.ChangeLightness(Color.LightSteelBlue, -20);
			BarRenderer.PressedBackgroundBottom = ImageLib.ChangeLightness(Color.AliceBlue, -60);
			BarRenderer.Invert = true;
			BarRenderer.SuppressColorChecking = false;
			BarRenderer.SuppressFunctionCallOnRefresh = false;
			BackColor = Color.Transparent;
			Padding = new Padding(1);
			Reset(min, max, value, increment);
		}

		/// <summary>
		/// Resets the slider to the specified values.
		/// </summary>
		/// <param name="min">The minimum value.</param>
		/// <param name="max">The maximum value.</param>
		/// <param name="value">The current value shown by the slider.</param>
		/// <param name="increment">The increments supported by the slider (ex. 0 for no limit, 1 for whole numbers, 2 for even numbers...).</param>
		public virtual void Reset(float min, float max, float value, float increment = 0f) {
			this.increment = increment;
			Minimum = min;
			Maximum = max;
			Value = value;
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
		/// Not implemented yet.
		/// Draws the control with its children in the current OpenGL context (assumes the GL matrix is set to orthographic and maps to pixel coordinates)
		/// </summary>
		/// <param name="location">The location to draw at</param>
		/// <param name="drawChildren">Whether to draw child controls as well</param>
		public virtual void DrawGL(Point location, bool drawChildren) {
			throw new NotImplementedException(nameof(DrawGL) + " is not implemented.");
		}

		/// <summary>
		/// Draws the slider on the specified Graphics object.
		/// </summary>
		/// <param name="g">The graphics object.</param>
		public void DrawGdi(Graphics g) {
			DrawGdi(g, Location, true);
		}

		/// <summary>
		/// Draws the slider on the specified Graphics object
		/// </summary>
		/// <param name="g">The graphics object</param>
		/// <param name="location">The coordinates to draw at</param>
		/// <param name="drawChildren">Whether to draw the child controls</param>
		public void DrawGdi(Graphics g, Point location, bool drawChildren = true) {
			g.CompositingQuality = CompositingQuality.HighQuality;
			g.PixelOffsetMode = PixelOffsetMode.HighQuality;
			g.SmoothingMode = SmoothingMode.HighQuality;
			Size labelTotalPadding = label.Padding.Size;
			Size clientSize = ClientSize;
			Size sliderBounds = clientSize;
			Size labelSize = label.Size;
			Padding padding = Padding;
			if (Vertical) {
				sliderBounds.Width -= padding.Horizontal;
				sliderBounds.Height -= labelSize.Height + labelTotalPadding.Height + padding.Bottom;
			} else {
				sliderBounds.Width -= labelSize.Width + labelTotalPadding.Width + padding.Right;
				sliderBounds.Height -= padding.Vertical;
			}
			Region oldClipRegion = g.Clip;
			RectangleF barRect;
			Rectangle clip = new Rectangle(location, clientSize);
			g.SetClip(clip);
			if (BackColor.A != 0) {
				using (SolidBrush brush = new SolidBrush(BackColor))
					g.FillRectangle(brush, new Rectangle(Point.Empty, clientSize));
			}
			label.DrawGdi(g, location);
			float div3;
			if (Vertical) {
				div3 = sliderBounds.Width * 0.3333333333333333f;
				barRect = new RectangleF(location.X + padding.Left + div3, location.Y + labelSize.Height + labelTotalPadding.Height, div3, sliderBounds.Height);
			} else {
				div3 = sliderBounds.Height * 0.3333333333333333f;
				barRect = new RectangleF(location.X + labelSize.Width + labelTotalPadding.Width, location.Y + padding.Top + div3, sliderBounds.Width, div3);
			}
			BarRenderer.RenderBackground(g, barRect);
			RectangleF knobBounds = KnobBounds;
			knobBounds.Offset(location);
			KnobRenderer.RenderBackground(g, Rectangle.Truncate(knobBounds));
			if (drawChildren)
				g.DrawControls(Controls, location, clip, true);
			g.Clip = oldClipRegion;
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
		/// Called when the control is to be redrawn
		/// </summary>
		protected override void OnPaint(PaintEventArgs e) {
			DrawGdi(e.Graphics, Point.Empty, false);
			RaisePaintEvent(null, e);
		}

		/// <summary>
		/// Called when the slider is enabled or disabled.
		/// </summary>
		/// <param name="e">Ignored.</param>
		protected override void OnEnabledChanged(EventArgs e) {
			KnobRenderer.Enabled = Enabled;
			BarRenderer.Enabled = Enabled;
			base.OnEnabledChanged(e);
		}

		/// <summary>
		/// Called when the padding has been changed.
		/// </summary>
		/// <param name="e">Ignored.</param>
		protected override void OnPaddingChanged(EventArgs e) {
			base.OnPaddingChanged(e);
			Padding padding = Padding;
			if (Vertical)
				label.Padding = new Padding(padding.Left, padding.Top, padding.Right, labelPadding);
			else
				label.Padding = new Padding(padding.Left, padding.Top, labelPadding, padding.Bottom);
			Invalidate(false);
		}

		/// <summary>
		/// Called when the mouse has left the control.
		/// </summary>
		/// <param name="e">Ignored.</param>
		protected override void OnMouseLeave(EventArgs e) {
			KnobRenderer.MarkMouseHasLeft();
			BarRenderer.MarkMouseHasLeft();
			base.OnMouseLeave(e);
		}

		/// <summary>
		/// Called when a mouse button is pressed.
		/// </summary>
		/// <param name="e">The mouse event data.</param>
		protected override void OnMouseDown(MouseEventArgs e) {
			base.OnMouseDown(e);
			if (e.Button == MouseButtons.Left) {
				Capture = true;
				Focus();
				BarRenderer.MarkMouseHasLeft();
				Point loc = e.Location;
				Size padding = label.Size;
				Rectangle bounds = ClientRectangle;
				if (Vertical) {
					padding.Height += label.Padding.Vertical;
					bounds.Height -= padding.Height + Padding.Bottom;
					loc.Y -= padding.Height;
				} else {
					padding.Width += label.Padding.Horizontal;
					bounds.Width -= padding.Width + Padding.Right;
					loc.X -= padding.Width;
				}
				if (bounds.Contains(loc)) {
					KnobRenderer.Pressed = true;
					wasPressed = true;
					if (!KnobBoundsInner.Contains(loc))
						SetValueRelativeTo(loc, bounds);
				}
			}
		}

		/// <summary>
		/// Called when the mouse is moved.
		/// </summary>
		/// <param name="e">The mouse event data.</param>
		protected override void OnMouseMove(MouseEventArgs e) {
			base.OnMouseMove(e);
			Point loc = e.Location;
			Size padding = label.Size;
			Rectangle bounds = ClientRectangle;
			if (Vertical) {
				padding.Height += label.Padding.Vertical;
				bounds.Height -= padding.Height + Padding.Bottom;
				loc.Y -= padding.Height;
			} else {
				padding.Width += label.Padding.Horizontal;
				bounds.Width -= padding.Width + Padding.Right;
				loc.X -= padding.Width;
			}
			if (bounds.Contains(loc)) {
				if (KnobBoundsInner.Contains(loc)) {
					BarRenderer.MouseHovering = false;
					KnobRenderer.MouseHovering = true;
					if (wasPressed)
						SetValueRelativeTo(loc, bounds);
				} else {
					if (wasPressed)
						SetValueRelativeTo(loc, bounds);
					else {
						BarRenderer.MouseHovering = true;
						KnobRenderer.MouseHovering = false;
					}
				}
			} else if (wasPressed)
				SetValueRelativeTo(loc, bounds);
			else
				OnMouseLeave(e);
		}

		private void SetValueRelativeTo(Point loc, Rectangle bounds) {
			Value = ((Vertical ? loc.Y : loc.X) - knobSize * 0.5f) * (barMaximum - barMinimum) / ((Vertical ? bounds.Height : bounds.Width) - knobSize) + barMinimum;
		}

		/// <summary>
		/// Called when a mouse button is released
		/// </summary>
		/// <param name="e">The mouse event data</param>
		protected override void OnMouseUp(MouseEventArgs e) {
			base.OnMouseUp(e);
			wasPressed = false;
			Capture = false;
			KnobRenderer.Pressed = false;
		}

		/// <summary>
		/// Called when the slider size is changed
		/// </summary>
		/// <param name="e">Ignored</param>
		protected override void OnClientSizeChanged(EventArgs e) {
			base.OnClientSizeChanged(e);
			Size clientSize = ClientSize;
			if (Vertical) {
				int width = ClientSize.Width;
				label.MaximumSize = new Size(width, 0);
				label.MinimumSize = new Size(width, 0);
			} else {
				int height = ClientSize.Height;
				label.MaximumSize = new Size(0, height);
				label.MinimumSize = new Size(0, height);
			}
			Invalidate(false);
		}

		/// <summary>
		/// Called when the mouse wheel is scrolled.
		/// </summary>
		/// <param name="e">The mouse event data.</param>
		protected override void OnMouseWheel(MouseEventArgs e) {
			base.OnMouseWheel(e);
			Value += e.Delta * 0.00833333333f * (barMaximum - barMinimum) / mouseWheelBarPartitions;
		}

		private void RaiseValueChanged() {
			EventHandler handler = ValueChanged;
			if (handler != null)
				handler(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises a key is released.
		/// </summary>
		/// <param name="e">The key that was released.</param>
		protected override void OnKeyUp(KeyEventArgs e) {
			base.OnKeyUp(e);
			switch (e.KeyCode) {
				case Keys.Down:
				case Keys.Left:
					Value -= smallChange;
					break;
				case Keys.Up:
				case Keys.Right:
					Value += smallChange;
					break;
				case Keys.Home:
					Value = barMinimum;
					break;
				case Keys.End:
					Value = barMaximum;
					break;
				case Keys.PageDown:
					Value -= largeChange;
					break;
				case Keys.PageUp:
					Value += largeChange;
					break;
			}
		}

		/// <summary>
		/// Processes a dialog key.
		/// </summary>
		/// <param name="keyData">The keys that were pressed.</param>
		protected override bool ProcessDialogKey(Keys keyData) {
			if (keyData == Keys.Tab | ModifierKeys == Keys.Shift)
				return base.ProcessDialogKey(keyData);
			else {
				OnKeyDown(new KeyEventArgs(keyData));
				return true;
			}
		}

		/// <summary>
		/// Gets a string that describes this instance.
		/// </summary>
		public override string ToString() {
			return nameof(StyledSlider) + ": { Name: " + Name + ", Text: " + Text.Replace('\v', '\n') + " }";
		}

		/// <summary>
		/// Called when the slider is being disposed.
		/// </summary>
		/// <param name="disposing">True to dispose managed resources.</param>
		protected override void Dispose(bool disposing) {
			KnobRenderer.Dispose();
			BarRenderer.Dispose();
			label.Dispose();
			base.Dispose(disposing);
		}
	}
}