using System.ComponentModel;
using System.Collections.Concurrent;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Platforms.Windows;
using System.Text;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms {
	/// <summary>
	/// A label that supports custom styling and background shadow. '\v' characters will be interpreted as line spacing insertion.
	/// </summary>
	[ToolboxItem(true)]
	[DesignTimeVisible(true)]
	[DesignerCategory("CommonControls")]
	[Description("A label that supports custom styling and background shadow.")]
	[DisplayName(nameof(StyledLabel))]
	public class StyledLabel : Label, ISmartControl {
		private static char[] LinkSplitter = new char[] { ' ', '\t', '\n', '\r', '\v', '\0' };
		/*/// <summary>
		/// Called when a link is clicked.
		/// </summary>
		public event LinkLabelLinkClickedEventHandler LinkClicked;
		private SortedList<LinkLabel.Link> links = new SortedList<LinkLabel.Link>(LinkComparer);*/
		private static string defaultTabReplacement = "    ";
		private bool vertical, reduceCaching, isLocked, renderShadow = false, wrapping = true, invalidateShadow = true;
		private float lineSpacingMultiplier = 0.45f, rotation, shadowOffsetX, shadowOffsetY, outlineThickness = 1f, shadowOpacity = 0.8f;
		private TextRenderingHint hint = TextRenderingHint.AntiAliasGridFit;
		private object SyncRoot = new object(), ShadowSync = new object();
		private Color shadowColor = Color.Black, outline = Color.Transparent;
		private SizeF shadowScale = new SizeF(1f, 1f);
		private SizeF dpi = new SizeF(96f, 96f);
		private ConcurrentDictionary<Size, Size> cachedAutoSizes = new ConcurrentDictionary<Size, Size>();
		private PixelOffsetMode pixelAlignment = PixelOffsetMode.HighQuality;
		private GraphicsPath stringPath;
		private Graphics shadowGraphics;
		private Bitmap shadow;
		private string tabReplacement;
		private int blur = 2;
		private byte[] buffer;

		/*/// <summary>
		/// A list of links in the text. The list is refreshed every time the text is changed.
		/// </summary>
		public SortedList<LinkLabel.Link> Links {
			get {
				return links;
			}
		}*/

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
		/// Gets or sets the pixel alignment of the rendered text.
		/// </summary>
		[Description("Gets or sets the pixel alignment of the rendered text.")]
		[DefaultValue(2)]
		public PixelOffsetMode PixelAlignment {
			get {
				return pixelAlignment;
			}
			set {
				if (value == pixelAlignment)
					return;
				pixelAlignment = value;
				Invalidate(false);
			}
		}

		/// <summary>
		/// Gets or sets the text rotation in degrees within the label (alignment should be set to middle-center).
		/// </summary>
		[Description("Gets or sets the text rotation in degrees within the label (alignment should be set to middle-center).")]
		[DefaultValue(0f)]
		public float Rotation {
			get {
				return rotation;
			}
			set {
				if (rotation == value)
					return;
				rotation = value;
				Invalidate(false);
			}
		}

		/// <summary>
		/// Set to true to reduce caching, which slightly improves performance for labels that will continually change text and format attributes.
		/// </summary>
		[Description("Set to true to reduce caching, which slightly improves performance for labels that will continually change text and format attributes.")]
		[DefaultValue(false)]
		public bool ReduceCaching {
			get {
				return reduceCaching;
			}
			set {
				reduceCaching = value;
				if (reduceCaching && stringPath != null) {
					lock (SyncRoot) {
						stringPath.Dispose();
						stringPath = null;
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the pen to use to draw the outline (if not null it will be used instead of 'Outline').
		/// </summary>
		[Browsable(false)]
		[DefaultValue(null)]
		public Pen OutlinePen {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the brush to use to draw text (if not null it will be used instead of ForeColor).
		/// </summary>
		[Browsable(false)]
		[DefaultValue(null)]
		public Brush TextBrush {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the thickness of the outline.
		/// </summary>
		[Description("Gets or sets the thickness of the outline.")]
		[DefaultValue(1f)]
		public float OutlineThickness {
			get {
				return outlineThickness;
			}
			set {
				if (value < 0f)
					value = 0f;
				if (outlineThickness == value)
					return;
				outlineThickness = value;
				Invalidate(false);
			}
		}

		/// <summary>
		/// Gets or sets the outline of the text.
		/// </summary>
		[Description("Gets or sets the text rendering hint.")]
		[DefaultValue(typeof(Color), "0x00FFFFFF")]
		public Color Outline {
			get {
				return outline;
			}
			set {
				if (outline == value)
					return;
				outline = value;
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
				return hint;
			}
			set {
				if (value == hint)
					return;
				hint = value;
				CheckAutoSize();
			}
		}

		/// <summary>
		/// Gets or sets the Dpi to use for AutoSize calculation.
		/// </summary>
		[Description("Gets or sets the AutoSize DPI.")]
		[DefaultValue(typeof(SizeF), "96, 96")]
		public SizeF Dpi {
			get {
				return dpi;
			}
			set {
				if (value.Width <= 0f)
					value.Width = 96f;
				if (value.Height <= 0f)
					value.Height = 96f;
				if (dpi == value)
					return;
				dpi = value;
				CheckAutoSize();
			}
		}

		/// <summary>
		/// Gets or sets the shadow color.
		/// </summary>
		[Description("Gets or sets the shadow color.")]
		[DefaultValue(typeof(Color), "0x000000")]
		public Color ShadowColor {
			get {
				return shadowColor;
			}
			set {
				if (shadowColor == value)
					return;
				shadowColor = value;
				invalidateShadow = true;
				Invalidate(false);
			}
		}

		/// <summary>
		/// Gets or sets whether to render a shadow.
		/// </summary>
		[Description("Gets or sets whether to render a shadow.")]
		[DefaultValue(false)]
		public bool RenderShadow {
			get {
				return renderShadow;
			}
			set {
				if (renderShadow == value)
					return;
				renderShadow = value;
				if (renderShadow)
					ReinitGraphics();
				else {
					lock (ShadowSync) {
						if (shadow != null) {
							shadow.Dispose();
							shadow = null;
						}
						if (shadowGraphics != null) {
							shadowGraphics.Dispose();
							shadowGraphics = null;
						}
					}
				}
				CheckAutoSize();
			}
		}

		/// <summary>
		/// Gets or sets the degree of opacity of the shadow (can be any value >=0).
		/// </summary>
		[Description("Gets or sets the degree of opacity of the shadow (can be any value >=0).")]
		[DefaultValue(0.8f)]
		public float ShadowOpacity {
			get {
				return shadowOpacity;
			}
			set {
				if (shadowOpacity == value)
					return;
				shadowOpacity = value;
				invalidateShadow = true;
				Invalidate(false);
			}
		}

		/// <summary>
		/// Gets or sets the blur of the shadow.
		/// </summary>
		[Description("Gets or sets the blur of the shadow.")]
		[DefaultValue(2)]
		public int Blur {
			get {
				return blur;
			}
			set {
				if (value < 0)
					value = 0;
				if (blur == value)
					return;
				blur = value;
				CheckAutoSize();
			}
		}

		/// <summary>
		/// Gets or sets the scale the shadow.
		/// </summary>
		[Description("Gets or sets the scale the shadow.")]
		[DefaultValue(typeof(SizeF), "1, 1")]
		public SizeF ShadowScale {
			get {
				return shadowScale;
			}
			set {
				if (shadowScale == value)
					return;
				shadowScale = value;
				CheckAutoSize();
			}
		}

		/// <summary>
		/// Gets or sets the X offset of the shadow relative to text.
		/// </summary>
		[Description("Gets or sets the X offset of the shadow relative to text.")]
		[DefaultValue(0f)]
		public float ShadowOffsetX {
			get {
				return shadowOffsetX;
			}
			set {
				if (shadowOffsetX == value)
					return;
				shadowOffsetX = value;
				CheckAutoSize();
				Invalidate(false);
			}
		}

		/// <summary>
		/// Gets or sets the Y offset of the shadow relative to text.
		/// </summary>
		[Description("Gets or sets the Y offset of the shadow relative to text.")]
		[DefaultValue(0f)]
		public float ShadowOffsetY {
			get {
				return shadowOffsetY;
			}
			set {
				if (shadowOffsetY == value)
					return;
				shadowOffsetY = value;
				CheckAutoSize();
			}
		}

		/// <summary>
		/// Gets or sets the offset of the shadow relative to the text.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public PointF ShadowOffset {
			get {
				return new PointF(shadowOffsetX, shadowOffsetY);
			}
			set {
				if (shadowOffsetX == value.X && shadowOffsetY == value.Y)
					return;
				shadowOffsetX = value.X;
				shadowOffsetY = value.Y;
				CheckAutoSize();
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
				return base.BackgroundImageLayout;
			}

			set {
				if (base.BackgroundImageLayout == value)
					return;
				base.BackgroundImageLayout = value;
				if (base.BackgroundImage != null)
					Invalidate(false);
			}
		}

		/// <summary>
		/// Gets or sets whether the text is wrapped.
		/// </summary>
		[Description("Gets or sets whether the text is wrapped.")]
		[DefaultValue(true)]
		public bool Wrapping {
			get {
				return wrapping;
			}
			set {
				if (wrapping == value)
					return;
				wrapping = value;
				CheckAutoSize();
			}
		}

		/// <summary>
		/// Gets or sets whether the text is vertical.
		/// </summary>
		[Description("Gets or sets whether the text is vertical.")]
		[DefaultValue(false)]
		public bool Vertical {
			get {
				return vertical;
			}
			set {
				if (vertical == value)
					return;
				vertical = value;
				CheckAutoSize();
			}
		}

		/// <summary>
		/// Gets or sets the line spacing multiplier relative to font height.
		/// </summary>
		[Description("Gets or sets the line spacing multiplier relative to font height.")]
		[DefaultValue(0.45f)]
		public float LineSpacingMultiplier {
			get {
				return lineSpacingMultiplier;
			}
			set {
				if (lineSpacingMultiplier == value)
					return;
				lineSpacingMultiplier = value;
				CheckAutoSize();
			}
		}

		/// <summary>
		/// Gets or sets the characters \t will be replaced with (default: 4 spaces).
		/// </summary>
		public static string DefaultTabReplacement {
			get {
				return defaultTabReplacement;
			}
			set {
				if (value == null)
					value = "    ";
				defaultTabReplacement = value;
			}
		}

		/// <summary>
		/// Gets or sets the characters \t will be replaced with (default: 4 spaces).
		/// </summary>
		[Description("Gets or sets the characters \t will be replaced with (default: 4 spaces).")]
		[DefaultValue("    ")]
		public string TabReplacement {
			get {
				return tabReplacement;
			}
			set {
				if (value == null)
					value = defaultTabReplacement;
				tabReplacement = value;
			}
		}

		/// <summary>
		/// Gets or sets the label text. \v represents line spacing and \t will be resolved to the specified characters.
		/// </summary>
		[Description("Gets or sets the label text. \v represents line spacing and \t will be resolved to the specified characters.")]
		[DefaultValue(nameof(StyledLabel))]
		public override string Text {
			get {
				return base.Text;
			}
			set {
				if (value == null)
					value = string.Empty;
				base.Text = ReplaceTabs(value.Replace("\r", string.Empty));
				/*links.Clear();
				string[] candidateLinks = value.Split(LinkSplitter);
				int index = 0;
				string candidate;
				for (int i = 0; i < candidateLinks.Length; i++) {
					candidate = candidateLinks[i];
					if (candidate.Length >= 7 && (candidate.StartsWith("http://") || candidate.StartsWith("www.") || candidate.StartsWith("https://") || candidate.StartsWith("ftp://") || candidate.StartsWith("file://")))
						Links.Add(new LinkLabel.Link(index, candidate.Length, candidate));
					index += candidate.Length + 1;
				}*/
				CheckAutoSize();
			}
		}

		/// <summary>
		/// Initializes a new shadowed label.
		/// </summary>
		public StyledLabel() : this(null) {
		}

		/// <summary>
		/// Initializes a new shadowed label using the specified text.
		/// </summary>
		/// <param name="text">The text of the label.</param>
		public StyledLabel(string text) {
			CheckForIllegalCrossThreadCalls = false;
			SetStyle(ControlStyles.ResizeRedraw, false);
			SetAutoSizeMode(AutoSizeMode.GrowAndShrink);
			AutoSize = false;
			Name = nameof(StyledLabel);
			ForeColor = Color.Black;
			tabReplacement = defaultTabReplacement;
			SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.CacheText | ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserMouse, true);
			ReinitGraphics();
			using (Graphics g = Graphics.FromHwnd(IntPtr.Zero)) {
				IntPtr desktop = g.GetHdc();
				Dpi = new SizeF(NativeApi.GetDeviceCaps(desktop, DeviceCap.LOGPIXELSX), NativeApi.GetDeviceCaps(desktop, DeviceCap.LOGPIXELSY));
				g.ReleaseHdc(desktop);
			}
			if (text != null)
				Text = text;
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
		/// Called when the label is redrawn
		/// </summary>
		protected override void OnPaint(PaintEventArgs e) {
			DrawGdi(e.Graphics, PointF.Empty, Size, true, false);
			RaisePaintEvent(null, e);
		}

		/// <summary>
		/// Fired when the font of the label is changed.
		/// </summary>
		protected override void OnFontChanged(EventArgs e) {
			base.OnFontChanged(e);
			CheckAutoSize();
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
			if (!(width == oldSize.Width && height == oldSize.Height)) {
				cachedAutoSizes.Clear();
				if (stringPath != null) {
					lock (SyncRoot) {
						stringPath.Dispose();
						stringPath = null;
					}
				}
				invalidateShadow = true;
				lock (ShadowSync) {
					if (IsDisposed)
						return;
					if (shadow == null || shadow.Width < width || shadow.Height < height)
						ReinitGraphics();
					else
						shadowGraphics.SetClip(new Rectangle(Point.Empty, Size), CombineMode.Replace);
				}
				Invalidate(false);
			}
		}

		/// <summary>
		/// Draws the label onto the specified graphics canvas.
		/// </summary>
		/// <param name="g">The canvas to draw the label on.</param>
		public void DrawGdi(Graphics g) {
			DrawGdi(g, Location, Size, true, true);
		}

		/// <summary>
		/// Draws the label onto the specified graphics canvas at the specified location.
		/// </summary>
		/// <param name="g">The canvas to draw the label on.</param>
		/// <param name="location">The location to draw the label at.</param>
		/// <param name="drawChildren">Whether to draw the child controls</param>
		public void DrawGdi(Graphics g, Point location, bool drawChildren = true) {
			DrawGdi(g, location, Size, true, drawChildren);
		}

		/// <summary>
		/// Renders the label to an image. Child controls are not rendered.
		/// </summary>
		public Bitmap ToBitmap() {
			Bitmap bitmap = new Bitmap(Width, Height, PixelFormat.Format32bppPArgb);
			using (Graphics g = Graphics.FromImage(bitmap))
				DrawGdi(g, Point.Empty, Size, true, false);
			return bitmap;
		}

		/// <summary>
		/// Replaces tabs with the tab replacement string using tab spacing.
		/// </summary>
		/// <param name="str">The string to replace.</param>
		public string ReplaceTabs(string str) {
			if (tabReplacement.Length == 0)
				return str.Replace("\t", string.Empty);
			else {
				int toFill;
				for (int i = 0; i < str.Length;) {
					if (str[i] == '\t') {
						toFill = tabReplacement.Length - (i % tabReplacement.Length);
						str = str.Substring(0, i) + tabReplacement.Substring(0, toFill) + (i < str.Length - 1 ? str.Substring(i + 1) : string.Empty);
						i += toFill;
					} else
						i++;
				}
				return str;
			}
		}

		/// <summary>
		/// Replaces tabs with the default tab replacement string using tab spacing.
		/// </summary>
		/// <param name="str">The string to replace.</param>
		public static string ReplaceTabsWithDefault(string str) {
			if (defaultTabReplacement.Length == 0)
				return str.Replace("\t", string.Empty);
			else {
				int toFill;
				for (int i = 0; i < str.Length;) {
					if (str[i] == '\t') {
						toFill = defaultTabReplacement.Length - (i % defaultTabReplacement.Length);
						str = str.Substring(0, i) + defaultTabReplacement.Substring(0, toFill) + (i < str.Length - 1 ? str.Substring(i + 1) : string.Empty);
						i += toFill;
					} else
						i++;
				}
				return str;
			}
		}

		/// <summary>
		/// Replaces tabs with the specified character string using tab spacing.
		/// </summary>
		/// <param name="str">The string to replace.</param>
		/// <param name="tabReplacement">The characters to replace tabs with.</param>
		public static string ReplaceTabs(string str, string tabReplacement) {
			if (tabReplacement.Length == 0)
				return str.Replace("\t", string.Empty);
			else {
				int toFill;
				for (int i = 0; i < str.Length;) {
					if (str[i] == '\t') {
						toFill = tabReplacement.Length - (i % tabReplacement.Length);
						str = str.Substring(0, i) + tabReplacement.Substring(0, toFill) + (i < str.Length - 1 ? str.Substring(i + 1) : string.Empty);
						i += toFill;
					} else
						i++;
				}
				return str;
			}
		}

		/// <summary>
		/// Resizes the specified font enough so that the text fits the specified size better.
		/// </summary>
		/// <param name="font">The font to adjust.</param>
		/// <param name="text">The text to fit.</param>
		/// <param name="size">The size to fit the text to.</param>
		public static Font ResizeFont(Font font, string text, SizeF size) {
			Size currentSize = TextRenderer.MeasureText(text, font);
			return new Font(font.FontFamily, font.Size * Math.Min(size.Width / currentSize.Width, size.Height / currentSize.Height), font.Style, font.Unit);
		}

		/// <summary>
		/// Gets the dimensions of the character for the specified font.
		/// </summary>
		/// <param name="font">The font to use for evaluation.</param>
		/// <param name="character">The character whose metrics to evaluate.</param>
		public static SizeF GetCharSize(Font font, char character) {
			using (Font fnt = new Font(font.FontFamily, font.Size * 10f, font.Style, font.Unit)) {
				using (Bitmap b = new Bitmap(1, 1)) {
					using (Graphics g = Graphics.FromImage(b)) {
						IntPtr hdc = g.GetHdc();
						IntPtr prev = NativeApi.SelectObject(hdc, fnt.ToHfont());
						MAT2 matrix = new MAT2 {
							eM11 = new FIXED() { value = 1 },
							eM12 = new FIXED() { value = 0 },
							eM21 = new FIXED() { value = 0 },
							eM22 = new FIXED() { value = 1 }
						};
						GLYPHMETRICS metrics;
						NativeApi.GetGlyphOutline(hdc, character, 0, out metrics, 0, IntPtr.Zero, ref matrix);
						g.ReleaseHdcInternal(hdc);
						return new SizeF(metrics.gmBlackBoxX * 0.1f, metrics.gmBlackBoxY * 0.1f);
					}
				}
			}
		}

		/// <summary>
		/// Called when the padding of the label has changed.
		/// </summary>
		/// <param name="e">Ignored.</param>
		protected override void OnPaddingChanged(EventArgs e) {
			base.OnPaddingChanged(e);
			CheckAutoSize();
		}

		/// <summary>
		/// Called when the text alignment is changed.
		/// </summary>
		/// <param name="e">Ignored.</param>
		protected override void OnTextAlignChanged(EventArgs e) {
			base.OnTextAlignChanged(e);
			CheckAutoSize();
		}

		private void CheckAutoSize() {
			cachedAutoSizes.Clear();
			if (stringPath != null) {
				lock (SyncRoot) {
					stringPath.Dispose();
					stringPath = null;
				}
			}
			invalidateShadow = true;
			if (AutoSize) {
				Size size = GetAutoSize();
				if (GetAutoSizeMode() != AutoSizeMode.GrowAndShrink)
					size = new Size(Math.Max(Width, size.Width), Math.Max(Height, size.Height));
				if (size == Size)
					Invalidate(false);
				else
					Size = size;
			} else
				Invalidate(false);
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

		internal void DrawGdi(Graphics g, PointF location, Size size, bool drawBackColor, bool drawChildren) {
			if (size.Width <= 0 || size.Height <= 0)
				return;
			Padding padding = Padding;
			Size textArea = new Size(size.Width - padding.Horizontal, size.Height - padding.Vertical);
			ContentAlignment alignment = TextAlign;
			StringFormat format = StyleRenderer.GetFormat(alignment, RightToLeft, wrapping, vertical, AutoEllipsis);
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			g.CompositingQuality = CompositingQuality.HighQuality;
			g.SmoothingMode = SmoothingMode.HighQuality;
			g.TextRenderingHint = hint;
			if (!location.IsEmpty)
				g.TranslateTransform(location.X, location.Y);
			if (drawBackColor && BackColor.A != 0) {
				using (SolidBrush brush = new SolidBrush(BackColor))
					g.FillRectangle(brush, 0f, 0f, size.Width, size.Height);
			}
			SizeF centerOffset = new SizeF();
			if (rotation != 0f) {
				centerOffset = new SizeF(padding.Left + textArea.Width * 0.5f, padding.Top + textArea.Height * 0.5f);
				g.TranslateTransform(centerOffset.Width, centerOffset.Height);
				g.RotateTransform(rotation);
				g.TranslateTransform(-centerOffset.Width, -centerOffset.Height);
			}
			g.PixelOffsetMode = pixelAlignment;
			lock (ShadowSync) {
				if (renderShadow && !(shadowColor.A == 0 || shadowOpacity == 0f)) {
					if (isLocked) //ugly workaround
						return;
					isLocked = true;
					if (shadowGraphics == null || shadow == null || shadow.Width < size.Width || shadow.Height < size.Height)
						ReinitGraphics();
					if (invalidateShadow) {
						invalidateShadow = false;
						shadowGraphics.Clear(Color.Transparent);
						using (SolidBrush shadowBrush = new SolidBrush(shadowColor)) {
							GraphicsPath path = DrawString(shadowGraphics, Text, Font, shadowBrush, null, new RectangleF(padding.Left + shadowOffsetX, padding.Top + shadowOffsetY, textArea.Width, textArea.Height), format, lineSpacingMultiplier, null, !(reduceCaching || hint == TextRenderingHint.ClearTypeGridFit));
							if (path != null)
								path.Dispose();
						}
						try {
							int blur = this.blur;
							Tuple<int, int[][]> val = ImageLib.CalculateBlurKernel(blur);
							using (PixelWorker worker = PixelWorker.FromImage(shadow, false, true, false))
								ImageLib.BoxBlurAlpha(worker, blur, 2, shadowOpacity, ImageLib.Clamp(val.Item2[blur][shadowColor.A] * shadowOpacity / val.Item1), ref buffer, size.Width, size.Height);
						} catch {
						}
					}
					g.DrawImage(shadow, new RectangleF(0f, 0f, size.Width * shadowScale.Width, size.Height * shadowScale.Height), new RectangleF(0f, 0f, size.Width, size.Height), GraphicsUnit.Pixel);
					isLocked = false;
				}
			}
			Brush textBrush = TextBrush;
			if (textBrush == null && ForeColor.A != 0)
				textBrush = new SolidBrush(ForeColor);
			Pen outlinePen = null;
			if (OutlinePen == null) {
				if (!(outline.A == 0 || outlineThickness == 0f))
					outlinePen = new Pen(outline, outlineThickness);
			} else
				outlinePen = OutlinePen;
			if (stringPath == null) {
				lock (SyncRoot)
					stringPath = DrawString(g, Text, Font, textBrush, outlinePen, new RectangleF(padding.Left, padding.Top, textArea.Width, textArea.Height), format, lineSpacingMultiplier, stringPath, !(reduceCaching || hint == TextRenderingHint.ClearTypeGridFit));
			} else {
				lock (SyncRoot) {
					if (textBrush != null)
						g.FillPath(textBrush, stringPath);
					if (outlinePen != null)
						g.DrawPath(outlinePen, stringPath);
				}
			}
			if (rotation != 0f) {
				g.TranslateTransform(centerOffset.Width, centerOffset.Height);
				g.RotateTransform(-rotation);
				g.TranslateTransform(-centerOffset.Width, -centerOffset.Height);
			}
			if (drawChildren)
				g.DrawControls(Controls, Point.Empty, Rectangle.Ceiling(g.ClipBounds), true);
			if (!location.IsEmpty)
				g.TranslateTransform(-location.X, -location.Y);
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
		/// Gets the resultant size of the label if it were to be auto-sized with both grow and shrink enabled.
		/// </summary>
		public Size GetAutoSize() {
			return GetAutoSize(MaximumSize, true);
		}

		/// <summary>
		/// Gets the resultant size of the label if it were to be auto-sized with both grow and shrink enabled.
		/// </summary>
		/// <param name="maxBounds">The maximum size to allow (0 means unlimited).</param>
		/// <param name="includePadding">If true, padding is included within the size.</param>
		public virtual Size GetAutoSize(Size maxBounds, bool includePadding) {
			if (Text.Length == 0)
				return includePadding ? Size.Empty : new Size(Padding.Horizontal, Padding.Vertical);
			if (maxBounds.Width == 0)
				maxBounds.Width = int.MaxValue;
			if (maxBounds.Height == 0)
				maxBounds.Height = int.MaxValue;
			Size resultant;
			lock (SyncRoot) {
				if (!cachedAutoSizes.TryGetValue(maxBounds, out resultant)) {
					StringBuilder builder = new StringBuilder(Text);
					int spacingCount = 0;
					for (int i = 0; i < builder.Length;) {
						if (builder[i] == '\v') {
							spacingCount++;
							if (i != 0 && builder[i - 1] == '\n')
								builder.Remove(i, 1);
							else {
								builder[i] = '\n';
								i++;
							}
						} else
							i++;
					}
					SizeF size;
					using (Graphics canvas = CreateGraphics())
						size = canvas.MeasureString(builder.ToString(), Font, maxBounds, StyleRenderer.GetFormat(TextAlign, RightToLeft, wrapping, vertical, AutoEllipsis));
					size.Width += outlineThickness;
					size.Height += outlineThickness + spacingCount * Font.Height * lineSpacingMultiplier;
					if (renderShadow) {
						size.Width += Math.Abs(shadowOffsetX) + blur + Math.Max(size.Width * (shadowScale.Width - 1f), 0f);
						size.Height += Math.Abs(shadowOffsetY) + blur + Math.Max(size.Height * (shadowScale.Height - 1f), 0f);
					}
					double rot = rotation * Maths.DegToRadD;
					float sin = (float) Math.Sin(rot);
					float cos = (float) Math.Cos(rot);
					size.Width = Math.Abs(size.Width * cos) + Math.Abs(size.Height * sin);
					size.Height = Math.Abs(size.Width * sin) + Math.Abs(size.Height * cos);
					if (includePadding) {
						size.Width += Padding.Horizontal;
						size.Height += Padding.Vertical;
					}
					size.Width *= dpi.Width * 0.01041666666f;
					size.Height *= dpi.Height * 0.01041666666f;
					resultant = Size.Ceiling(size);
					cachedAutoSizes.TryAdd(maxBounds, resultant);
				}
			}
			if (maxBounds.Width > 0 && resultant.Width > maxBounds.Width)
				resultant.Width = maxBounds.Width;
			if (maxBounds.Height > 0 && resultant.Height > maxBounds.Height)
				resultant.Height = maxBounds.Height;
			return resultant;
		}

		/// <summary>
		/// Overrides some Windows messages of the underlying control.
		/// </summary>
		/// <param name="m">The message received.</param>
		protected override void WndProc(ref Message m) {
			if (m.Msg == (int) WindowMessage.NCHITTEST && !StyledForm.DesignMode)
				m.Result = TransparentControl.HTTRANSPARENT;
			else if (m.Msg == (int) WindowMessage.DISPLAYCHANGE) {
				using (Graphics g = Graphics.FromHwnd(IntPtr.Zero)) {
					IntPtr desktop = g.GetHdc();
					Dpi = new SizeF(NativeApi.GetDeviceCaps(desktop, DeviceCap.LOGPIXELSX), NativeApi.GetDeviceCaps(desktop, DeviceCap.LOGPIXELSY));
					g.ReleaseHdc(desktop);
				}
			} else
				base.WndProc(ref m);
		}

		private static GraphicsPath DrawString(Graphics g, string text, Font font, Brush textBrush, Pen outline, RectangleF area, StringFormat format, float lineSpacingMultiplier, GraphicsPath path, bool cache) {
			if (textBrush == null && outline == null)
				return null;
			g.CompositingMode = CompositingMode.SourceOver;
			if (outline == null && !cache)
				path = null;
			if (path == null) {
				StringBuilder builder = new StringBuilder(text.Length);
				string str;
				char c;
				float fontHeight = font.Height;
				path = outline == null && !cache ? null : new GraphicsPath();
				if (format.LineAlignment == StringAlignment.Far) {
					for (int i = text.Length - 1; i >= 0; i--) {
						c = text[i];
						if (c == '\n' || c == '\v') {
							if (builder.Length != 0) {
								str = builder.ToString().Reverse();
								if (path == null)
									g.DrawString(str, font, textBrush, area, format);
								else
									path.AddString(str, font.FontFamily, (int) font.Style, g.DpiY * font.SizeInPoints * 0.01388888888f, area, format);
							}
							if (c == '\n')
								area.Height -= builder.Length < 2 ? fontHeight : g.MeasureString(builder.ToString(), font, area.Size, format).Height;
							else
								area.Height -= (builder.Length < 2 ? fontHeight : g.MeasureString(builder.ToString(), font, area.Size, format).Height) * lineSpacingMultiplier;
							if (area.Height > 0)
								builder.Length = 0;
							else
								break;
						} else
							builder.Append(c);
					}
					if (builder.Length != 0) {
						str = builder.ToString().Reverse();
						if (path == null)
							g.DrawString(str, font, textBrush, area, format);
						else
							path.AddString(str, font.FontFamily, (int) font.Style, g.DpiY * font.SizeInPoints * 0.01388888888f, area, format);
					}
				} else {
					float lineHeight;
					for (int i = 0; i < text.Length; i++) {
						c = text[i];
						if (c == '\n' || c == '\v') {
							if (builder.Length != 0) {
								str = builder.ToString();
								if (path == null)
									g.DrawString(str, font, textBrush, area, format);
								else
									path.AddString(str, font.FontFamily, (int) font.Style, g.DpiY * font.SizeInPoints * 0.01388888888f, area, format);
							}
							if (c == '\n')
								lineHeight = builder.Length < 2 ? fontHeight : g.MeasureString(builder.ToString(), font, area.Size, format).Height;
							else
								lineHeight = (builder.Length < 2 ? fontHeight : g.MeasureString(builder.ToString(), font, area.Size, format).Height) * lineSpacingMultiplier;
							area.Y += lineHeight;
							area.Height -= lineHeight;
							if (area.Height > 0)
								builder.Length = 0;
							else
								break;
						} else
							builder.Append(c);
					}
					if (builder.Length != 0) {
						if (path == null)
							g.DrawString(builder.ToString(), font, textBrush, area, format);
						else
							path.AddString(builder.ToString(), font.FontFamily, (int) font.Style, g.DpiY * font.SizeInPoints * 0.01388888888f, area, format);
					}
				}
			}
			if (path != null) {
				if (textBrush != null)
					g.FillPath(textBrush, path);
				if (outline != null)
					g.DrawPath(outline, path);
			}
			return path;
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
		/// For internal use only. Reloads the shadow graphics.
		/// </summary>
		public void ReinitGraphics() {
			Size size = Size;
			if (size.Width <= 0 || size.Height <= 0)
				return;
			else {
				lock (ShadowSync) {
					if (shadow != null)
						shadow.Dispose();
					shadow = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppPArgb);
					if (shadowGraphics != null)
						shadowGraphics.Dispose();
					shadowGraphics = Graphics.FromImage(shadow);
					shadowGraphics.PixelOffsetMode = pixelAlignment;
					shadowGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
					shadowGraphics.CompositingQuality = CompositingQuality.HighQuality;
					shadowGraphics.SmoothingMode = SmoothingMode.HighQuality;
					shadowGraphics.TextRenderingHint = hint;
					shadowGraphics.SetClip(new Rectangle(Point.Empty, size), CombineMode.Replace);
					invalidateShadow = true;
				}
			}
		}

		/// <summary>
		/// Gets a string that describes this instance.
		/// </summary>
		public override string ToString() {
			return nameof(StyledLabel) + ": { Name: " + Name + ", Text: " + Text.Replace('\v', '\n') + " }";
		}

		/// <summary>
		/// Disposes of the label and its resources.
		/// </summary>
		/// <param name="disposing">Whether Dispose() was called manually or automatically.</param>
		protected override void Dispose(bool disposing) {
			base.Dispose(disposing);
			lock (ShadowSync) {
				renderShadow = false;
				if (shadow != null) {
					shadow.Dispose();
					shadow = null;
				}
				if (shadowGraphics != null) {
					shadowGraphics.Dispose();
					shadowGraphics = null;
				}
			}
			buffer = null;
		}
	}
}