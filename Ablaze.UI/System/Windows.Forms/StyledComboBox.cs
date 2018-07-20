using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Text;
using System.Reflection;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms {
	/// <summary>
	/// A customizable styled combo box.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxItemFilter("System.Windows.Forms")]
	[DesignTimeVisible(true)]
	[DesignerCategory("CommonControls")]
	[Description("A customizable styled combo box.")]
	[DisplayName(nameof(StyledComboBox))]
	[DefaultEvent("SelectedIndexChanged")]
	public class StyledComboBox : ComboBox, ISmartControl {
		private const string Key = "&&";
		private FieldOrProperty forecolorProperty;
		private static object selectedIndexKey = typeof(ComboBox).GetField("EVENT_SELECTEDINDEXCHANGED", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
		private static object selectedItemKey = typeof(ComboBox).GetField("EVENT_SELECTEDITEMCHANGED", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
		private static object selectionChangeCommittedKey = typeof(ComboBox).GetField("EVENT_SELECTIONCHANGECOMMITTED", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
		private static PropertyInfo ComponentEvents = typeof(Component).GetProperty("Events", BindingFlags.Instance | BindingFlags.NonPublic);
		private bool wasPressed, isInside;
		private ImageLayout backgroundLayout = ImageLayout.Stretch;
		private StyleRenderer itemRenderer;
		private EventHandlerList EventList;
		private StyledLabel label = new StyledLabel();
		private SyncedList<object> list = new SyncedList<object>();

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
		/// Gets whether the control supports OpenGL rendering.
		/// </summary>
		[Browsable(false)]
		public bool SupportsGL {
			get {
				return false;
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
		/// Gets the renderer used for styling.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public StyleRenderer Renderer {
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets the text color to use when the combo box is in normal state.
		/// </summary>
		[Description("Gets or sets the text color to use when the combo box is in normal state.")]
		[DefaultValue(typeof(Color), "0x000000")]
		public Color NormalTextColor {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the text color to use when the combo box is in hovered state.
		/// </summary>
		[Description("Gets or sets the text color to use when the combo box is in hovered state.")]
		[DefaultValue(typeof(Color), "0x000000")]
		public Color HoverTextColor {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the text color to use when the combo box is in pressed state.
		/// </summary>
		[Description("Gets or sets the text color to use when the combo box is in pressed state.")]
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
		/// Gets whether the combo box has input focus.
		/// </summary>
		[Browsable(false)]
		public bool HasFocus {
			get;
			private set;
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
		/// Gets or sets the text to show in the combo box label.
		/// </summary>
		[Description("Gets or sets the text to show in the combo box label.")]
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
		/// Gets or sets whether to use AutoSize on the combo box.
		/// </summary>
		[DefaultValue(false)]
		[Description("Gets or sets whether to use AutoSize on the combo box.")]
		public override bool AutoSize {
			get {
				return base.AutoSize;
			}
			set {
				base.AutoSize = value;
				CheckAutoSize();
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
		/// Gets a list of the items that are loaded in the context menu.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public new SyncedList<object> Items {
			get {
				TransferToContextMenu();
				return list;
			}
		}

		/// <summary>
		/// Gets or sets the string shown by the combo box.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new string SelectedText {
			get {
				return Label.Text;
			}
			set {
				Label.Text = value;
			}
		}

		/// <summary>
		/// Gets or sets the string shown by the combo box.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new object SelectedItem {
			get {
				return Label.Text;
			}
			set {
				if (value == null)
					value = string.Empty;
				Label.Text = value.ToString();
			}
		}

		/// <summary>
		/// Gets or sets the string shown by the combo box.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new object SelectedValue {
			get {
				return Label.Text;
			}
			set {
				if (value == null)
					value = string.Empty;
				Label.Text = value.ToString();
			}
		}

		/// <summary>
		/// Gets or sets the index that is selected.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override int SelectedIndex {
			get {
				string text = Label.Text;
				ToolStripItemCollection items = ContextMenuStrip.Items;
				for (int i = 0; i < items.Count; i++) {
					if (items[i].Text == text)
						return i;
				}
				return -1;
			}
			set {
				if (value < 0)
					return;
				TransferToContextMenu();
				if (value == SelectedIndex || (value == 0 && ContextMenuStrip.Items.Count == 0))
					return;
				Text = ContextMenuStrip.Items[value].Text;
				RaiseChange();
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
		/// Initializes the styled combo box.
		/// </summary>
		public StyledComboBox() : this(null) {
		}

		/// <summary>
		/// Initializes the styled combo box with the specified text.
		/// </summary>
		/// <param name="text">The text to initialize the combo box width.</param>
		public StyledComboBox(string text) {
			CheckForIllegalCrossThreadCalls = false;
			SetStyle(ControlStyles.ResizeRedraw, false);
			SetAutoSizeMode(AutoSizeMode.GrowAndShrink);
			forecolorProperty = new FieldOrProperty(nameof(ForeColor), this);
			IntegralHeight = false;
			DrawMode = DrawMode.OwnerDrawFixed;
			list.Filter += AddItem;
			list.ShouldRemove += RemoveItem;
			EventList = (EventHandlerList) ComponentEvents.GetValue(this, null);
			DropDownHeight = 1;
			DropDownStyle = ComboBoxStyle.DropDownList;
			Name = nameof(StyledComboBox);
			base.Items.Add(Key);
			Renderer = new StyleRenderer(UIAnimator.GetFunctionToInvalidateControlOnUpdate(this, false)) {
				SuppressColorChecking = true,
				SuppressFunctionCallOnRefresh = true,
				RoundCornerRadius = 0f,
				RoundedEdges = false,
				NormalInnerBorderColor = Color.FromArgb(177, 240, 245),
			};
			if (!StyledForm.DesignMode)
				Renderer.CheckColors += Renderer_CheckColors;
			Renderer.SuppressColorChecking = false;
			Renderer.SuppressFunctionCallOnRefresh = false;
			itemRenderer = new StyleRenderer(Renderer) {
				RoundCornerRadius = 0f,
				RoundedEdges = false,
				Invert = !Renderer.Invert
			};
			StyledContextMenu menu = new StyledContextMenu();
			menu.Alignment = ContentAlignment.MiddleLeft;
			menu.AutoSize = false;
			menu.Width = Width;
			menu.Renderer = itemRenderer;
			ContextMenuStrip = menu;
			NormalTextColor = Color.Black;
			HoverTextColor = Color.Black;
			PressedTextColor = Color.Black;
			NormalInnerBorderWidth = 1f;
			FocusInnerBorderWidth = 2f;
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.UserMouse | ControlStyles.CacheText, true);
			label.BackColor = Color.Transparent;
			label.TextAlign = ContentAlignment.MiddleLeft;
			label.TextRenderingStyle = TextRenderingHint.ClearTypeGridFit;
			label.Padding = DefaultPadding;
			label.RenderShadow = false;
			if (text != null)
				label.Text = text;
			CreateHandle();
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
		/// Adds the specified item to the combo box.
		/// </summary>
		/// <param name="item">The item to add to the combo box.</param>
		public void AddItem(string item) {
			if (item != null)
				AddItem(new StyledItem(item, itemRenderer));
		}

		/// <summary>
		/// Adds the specified item to the combo box.
		/// </summary>
		/// <param name="item">The item to add to the combo box.</param>
		public void AddItem(StyledItem item) {
			if (item != null) {
				item.Click += Item_Click;
				Items.Add(item.Text);
				ContextMenuStrip.Items.Add(item);
			}
		}

		private bool AddItem(object value) {
			if (value == null)
				return false;
			else {
				StyledItem item = new StyledItem(value.ToString(), itemRenderer);
				item.Click += Item_Click;
				ContextMenuStrip.Items.Add(item);
				return true;
			}
		}

		/// <summary>
		/// Removes the specified item from the combo box and returns whether it was found.
		/// </summary>
		/// <param name="item">The item to remove from the combo box.</param>
		public void RemoveItem(StyledItem item) {
			if (item != null) {
				Items.Remove(item.Text);
				ContextMenuStrip.Items.Remove(item);
			}
		}

		private bool RemoveItem(object value) {
			if (value == null)
				return false;
			else {
				RemoveItem(value.ToString(), false, false);
				return true;
			}
		}

		/// <summary>
		/// Removes the specified item from the combo box and returns whether it was found.
		/// </summary>
		/// <param name="item">The item to remove from the combo box.</param>
		/// <param name="ignoreCase">Whether the item search is case sensitive.</param>
		public bool RemoveItem(string item, bool ignoreCase = false) {
			return RemoveItem(item, ignoreCase, true);
		}

		private bool RemoveItem(string item, bool ignoreCase, bool removeFromList) {
			if (item == null)
				return false;
			ToolStripItemCollection items = ContextMenuStrip.Items;
			StringComparison comparison = ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture;
			for (int i = 0; i < items.Count; i++) {
				if (item.Equals(items[i].Text, comparison)) {
					if (removeFromList) {
						try {
							Items.RemoveAt(i);
						} catch {
						}
					}
					items.RemoveAt(i);
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Removes all the items from the combo box.
		/// </summary>
		public void ClearItems() {
			ContextMenuStrip.Items.Clear();
		}

		/// <summary>
		/// Called when the combo box is made visible.
		/// </summary>
		/// <param name="e">Ignored.</param>
		protected override void OnVisibleChanged(EventArgs e) {
			base.OnVisibleChanged(e);
			if (Visible) {
				TransferToContextMenu();
				OnMouseEnter(e);
				OnMouseLeave(e);
			}
		}

		private void TransferToContextMenu() {
			string text;
			int index = -1;
			for (int i = 0; i < base.Items.Count; i++) {
				text = base.Items[i].ToString();
				if (text == Key)
					index = i;
				else
					AddItem(text);
			}
			if (Label.Text.Length == 0 && ContextMenuStrip.Items.Count != 0)
				Text = ContextMenuStrip.Items[0].Text;
			CheckAutoSize();
			if (!StyledForm.DesignMode) {
				base.Items.Clear();
				base.Items.Add(Key);
			}
		}

		private void Item_Click(object sender, EventArgs e) {
			ToolStripItem item = sender as ToolStripItem;
			if (item != null) {
				Text = item.Text;
				RaiseChange();
			}
		}

		private void RaiseChange() {
			Delegate delegates = EventList[selectedIndexKey];
			if (delegates != null)
				((EventHandler) delegates)(this, EventArgs.Empty);
			delegates = EventList[selectedItemKey];
			if (delegates != null)
				((EventHandler) delegates)(this, EventArgs.Empty);
			delegates = EventList[selectionChangeCommittedKey];
			if (delegates != null)
				((EventHandler) delegates)(this, EventArgs.Empty);
		}

		/// <summary>
		/// Do not use.
		/// </summary>
		protected override void OnSelectedValueChanged(EventArgs e) {
		}
		/// <summary>
		/// Does nothing.
		/// </summary>
		protected override void RefreshItem(int index) {
		}
		/// <summary>
		/// Does nothing.
		/// </summary>
		protected override void RefreshItems() {
		}
		/// <summary>
		/// Does nothing.
		/// </summary>
		protected override void OnDrawItem(DrawItemEventArgs e) {
		}
		/// <summary>
		/// Does nothing.
		/// </summary>
		protected override void OnMeasureItem(MeasureItemEventArgs e) {
		}
		/// <summary>
		/// Does nothing.
		/// </summary>
		protected override void SetItemCore(int index, object value) {
		}
		/// <summary>
		/// Does nothing.
		/// </summary>
		protected override void SetItemsCore(IList value) {
		}

		/// <summary>
		/// Called when the dropdown menu is clicked.
		/// </summary>
		/// <param name="e">Ignored.</param>
		protected override void OnClick(EventArgs e) {
			ContextMenuStrip.Show(this, 0, Height);
		}

		/// <summary>
		/// Updates the item renderer of each sub-menu item to the value of ItemRenderer.
		/// </summary>
		public void UpdateItemRenderer() {
			if (ContextMenuStrip != null)
				ContextMenuStrip.Renderer = itemRenderer;
		}

		/// <summary>
		/// Gets the resultant size of the combo box if it were to be auto-sized with both grow and shrink enabled.
		/// </summary>
		public Size GetAutoSize() {
			return GetAutoSize(MaximumSize, true);
		}

		/// <summary>
		/// Called when the padding has been changed.
		/// </summary>
		/// <param name="e">Ignored.</param>
		protected override void OnPaddingChanged(EventArgs e) {
			base.OnPaddingChanged(e);
			Label.Padding = Padding;
			CheckAutoSize();
		}

		/// <summary>
		/// Gets the resultant size of the combo box if it were to be auto-sized with both grow and shrink enabled.
		/// </summary>
		/// <param name="maxBounds">The maximum size to allow (0 means unlimited).</param>
		/// <param name="includePadding">If true, padding is included within the size.</param>
		public virtual Size GetAutoSize(Size maxBounds, bool includePadding) {
			Size labelAutoSize = new Size(((StyledContextMenu) ContextMenuStrip).GetAutoSize(Size.Empty, includePadding).Width, Label.GetAutoSize(Size.Empty, includePadding).Height);
			Size resultant = new Size(labelAutoSize.Width + (int) StyledLabel.GetCharSize(Label.Font, 'M').Width * 2, labelAutoSize.Height);
			if (maxBounds.Width > 0 && resultant.Width > maxBounds.Width)
				resultant.Width = maxBounds.Width;
			if (maxBounds.Height > 0 && resultant.Height > maxBounds.Height)
				resultant.Height = maxBounds.Height;
			return resultant;
		}

		/// <summary>
		/// Sets the size of the control to the autosize result.
		/// </summary>
		public virtual void FitToContent() {
			Size = GetAutoSize();
		}

		/// <summary>
		/// Paints the combo box background (unimplemented).
		/// </summary>
		protected override void OnPaintBackground(PaintEventArgs pevent) {
		}

		/// <summary>
		/// Fired when the mouse enters the control.
		/// </summary>
		protected override void OnMouseEnter(EventArgs e) {
			Renderer.MouseHovering = true;
			isInside = true;
			if (wasPressed)
				Renderer.Pressed = true;
		}

		/// <summary>
		/// Fired when the mouse leaves the control.
		/// </summary>
		protected override void OnMouseLeave(EventArgs e) {
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
			if (nowInside && !isInside)
				OnMouseEnter(mevent);
			else if (!nowInside && isInside)
				OnMouseLeave(mevent);
		}

		/// <summary>
		/// Fires when the mouse is released on the control.
		/// </summary>
		protected override void OnMouseUp(MouseEventArgs mevent) {
			base.OnMouseUp(mevent);
			if (mevent.Button == MouseButtons.Left) {
				Platforms.Windows.NativeApi.ReleaseCapture();
				Renderer.Pressed = false;
				wasPressed = false;
			}
		}

		private void CheckAutoSize() {
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
		/// Fired when auto-size is changed.
		/// </summary>
		protected override void OnAutoSizeChanged(EventArgs e) {
			base.OnAutoSizeChanged(e);
			CheckAutoSize();
		}

		/// <summary>
		/// Fires when the combo box got input focus.
		/// </summary>
		protected override void OnGotFocus(EventArgs e) {
			HasFocus = true;
			Renderer.NormalInnerBorderWidth = FocusInnerBorderWidth;
			Renderer.HoverInnerBorderWidth = FocusInnerBorderWidth;
			base.OnGotFocus(e);
		}

		/// <summary>
		/// Fires when the button lost input focus.
		/// </summary>
		protected override void OnLostFocus(EventArgs e) {
			HasFocus = false;
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
		/// Draws the combo box on the specified Graphics object
		/// </summary>
		/// <param name="g">The graphics object</param>
		public void DrawGdi(Graphics g) {
			DrawGdi(g, Location);
		}

		/// <summary>
		/// Draws the combo box on the specified Graphics object
		/// </summary>
		/// <param name="g">The graphics object</param>
		/// <param name="location">The location to draw at</param>
		/// <param name="drawChildren">Whether to draw the child controls</param>
		public virtual void DrawGdi(Graphics g, Point location, bool drawChildren = true) {
			Size size = Size;
			Renderer.RenderBackground(g, new RectangleF(location, size), false, false, BackgroundImage, BackgroundImageLayout);
			Label.DrawGdi(g, location + new Size(Padding.Left, Padding.Top), size - new Size(Padding.Horizontal, Padding.Vertical), true, false);
			SizeF charSize = StyledLabel.GetCharSize(Label.Font, 'M');
			g.SmoothingMode = Drawing.Drawing2D.SmoothingMode.HighQuality;
			g.PixelOffsetMode = Drawing.Drawing2D.PixelOffsetMode.HighQuality;
			using (SolidBrush brush = new SolidBrush(ForeColor))
				g.DrawTriangle(new RectangleF(size.Width - (charSize.Width + 5f), (size.Height - charSize.Height) * 0.5f, charSize.Width, charSize.Height), brush, Direction.Down);
			if (drawChildren)
				g.DrawControls(Controls, location, true);
		}

		/// <summary>
		/// Not implemented yet.
		/// Draws the control with its children in the current OpenGL context (assumes the GL matrix is set to orthographic and maps to pixel coordinates).
		/// </summary>
		/// <param name="location">The location to draw at</param>
		/// <param name="drawChildren">Whether to draw child controls</param>
		public virtual void DrawGL(Point location, bool drawChildren) {
			throw new NotImplementedException(nameof(DrawGL) + " is not implemented.");
		}

		/// <summary>
		/// Called when a message needs to be processed by the control.
		/// </summary>
		/// <param name="m">The message to process.</param>
		protected override void WndProc(ref Message m) {
			switch (m.Msg) {
				case 8235:
				case 8236:
				case 8465:
				case 792:
				case 528:
					return;
				default:
					base.WndProc(ref m);
					return;
			}
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
				Label.Bounds = new Rectangle(0, 0, width, height);
				ContextMenuStrip.Width = width;
				Invalidate(false);
			}
		}

		/// <summary>
		/// Paints the combo box
		/// </summary>
		protected override void OnPaint(PaintEventArgs pevent) {
			DrawGdi(pevent.Graphics, Point.Empty, false);
			RaisePaintEvent(null, pevent);
		}

		/// <summary>
		/// Gets a string that describes this instance
		/// </summary>
		public override string ToString() {
			return nameof(StyledComboBox) + ": { Name: " + Name + ", Text: " + Text.Replace('\v', '\n') + " }";
		}

		/// <summary>
		/// Called when the combo box is being disposed.
		/// </summary>
		/// <param name="disposing">True to dispose managed resources.</param>
		protected override void Dispose(bool disposing) {
			Renderer.Dispose();
			itemRenderer.Dispose();
			Label.Dispose();
			base.Dispose(disposing);
		}
	}
}