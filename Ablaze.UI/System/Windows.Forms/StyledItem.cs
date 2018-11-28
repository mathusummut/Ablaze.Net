using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;

namespace System.Windows.Forms {
	/// <summary>
	/// A customizable styled item.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxItemFilter("System.Windows.Forms")]
	[DesignTimeVisible(true)]
	[DesignerCategory("Components")]
	[Description("A customizable styled item.")]
	[DisplayName(nameof(StyledItem))]
	[DefaultEvent("Click")]
	public class StyledItem : ToolStripMenuItem, IItemRenderer, ICloneable {
		/// <summary>
		/// Fired when the image has been changed.
		/// </summary>
		public event EventHandler ImageChanged;
		/// <summary>
		/// Fired when the icon has been changed.
		/// </summary>
		public event EventHandler IconChanged;
		private ImageLayout backgroundLayout = ImageLayout.Stretch;
		private ToolStripItem toolStripItem;
		private object SyncRoot = new object();
		private StyledContextMenu contextMenu;
		private StyleRenderer itemRenderer, tempRenderer;
		private StyledCheckBox checkBox = new StyledCheckBox();
		private Size maximumSize;
		private bool isComboBoxItem, autoSize = true;
		internal IItemRenderer parent;

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
		/// Gets or sets the renderer to use for this item (null for default).
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public StyleRenderer Renderer {
			get {
				if (itemRenderer == null) {
					if (parent == null) {
						itemRenderer = new StyleRenderer();
						itemRenderer.FunctionToCallOnRefresh = UIAnimator.GetFunctionToInvalidateControlOnUpdate(Parent);
						return itemRenderer;
					} else
						return parent.ItemRenderer;
				} else
					return itemRenderer;
			}
			set {
				itemRenderer = value;
				Invalidate();
			}
		}

		/// <summary>
		/// Returns false.
		/// </summary>
		[Browsable(false)]
		public new bool DoubleClickEnabled {
			get {
				return false;
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
		/// The associated context menu to open on click (can be null if no sub-menu items are added).
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public StyledContextMenu ContextMenu {
			get {
				if (contextMenu == null)
					TransferItemsToContextMenu(this);
				return contextMenu;
			}
		}

		/// <summary>
		/// Gets or sets the image to show on the left of the item. Icon takes precendence over Image.
		/// </summary>
		[Description("Gets or sets the image to show instead of a checkbox.")]
		[DefaultValue(null)]
		public override Image Image {
			get {
				return CheckBox.Image;
			}
			set {
				CheckBox.Image = value;
				OnImageChanged(EventArgs.Empty);
				Invalidate();
			}
		}

		/// <summary>
		/// Gets or sets the icon to show on the left of the item. Icon takes precendence over Image.
		/// </summary>
		[Description("Gets or sets the icon to show instead of a checkbox.")]
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
		/// Gets or sets the item padding.
		/// </summary>
		[Description("Gets or sets the item padding.")]
		public override Padding Padding {
			get {
				return CheckBox.Padding;
			}
			set {
				if (value == CheckBox.Padding)
					return;
				CheckBox.Padding = value;
				FitToContent();
			}
		}

		/// <summary>
		/// Gets or sets the parent container of the item.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ToolStrip Parent {
			get {
				return base.Parent;
			}
			set {
				base.Parent = value;
				parent = value as IItemRenderer;
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
				Invalidate();
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
					Invalidate();
			}
		}

		/// <summary>
		/// Gets the renderer used for styling items.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public StyleRenderer ItemRenderer {
			get {
				lock (SyncRoot) {
					if (contextMenu == null) {
						if (tempRenderer == null)
							tempRenderer = Renderer;
						return tempRenderer;
					} else
						return contextMenu.ItemRenderer;
				}
			}
			set {
				lock (SyncRoot) {
					if (contextMenu == null) {
						if (tempRenderer == null)
							tempRenderer = new StyleRenderer(value);
						else
							tempRenderer.CopyConfigFrom(value);
					} else {
						contextMenu.ItemRenderer.CopyConfigFrom(value);
						contextMenu.UpdateRenderer();
					}
				}
			}
		}

		/// <summary>
		/// Gets the default item padding.
		/// </summary>
		[Browsable(false)]
		protected override Padding DefaultPadding {
			get {
				return new Padding(5, 3, 5, 3);
			}
		}

		/// <summary>
		/// Gets or sets the maximum auto-size for the item.
		/// </summary>
		[Description("Gets or sets the maximum auto-size for the item.")]
		public Size MaximumSize {
			get {
				return maximumSize;
			}
			set {
				if (value == maximumSize)
					return;
				maximumSize = value;
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
				FitToContent();
			}
		}

		/// <summary>
		/// Gets or sets whether the item is checked when it is clicked.
		/// </summary>
		[Description("Gets or sets whether the item is checked when it is clicked.")]
		[DefaultValue(false)]
		public new bool CheckOnClick {
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
		/// Gets or sets the text rendering hint.
		/// </summary>
		[Description("Gets or sets the text rendering hint.")]
		[DefaultValue(3)]
		public TextRenderingHint TextRenderingStyle {
			get {
				return CheckBox.TextRenderingStyle;
			}
			set {
				if (value == CheckBox.TextRenderingStyle)
					return;
				CheckBox.TextRenderingStyle = value;
				Invalidate();
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
		/// Gets or sets whether this item represents a combo box item.
		/// </summary>
		[Description("Gets or sets whether this item represents a combo box item.")]
		[DefaultValue(false)]
		public bool IsComboBoxItem {
			get {
				return isComboBoxItem;
			}
			set {
				if (value == isComboBoxItem)
					return;
				isComboBoxItem = value;
				Text = base.Text;
			}
		}

		/// <summary>
		/// Gets or sets the text of the label shown.
		/// </summary>
		[Description("Gets or sets the text of the label shown.")]
		[DefaultValue(nameof(StyledItem))]
		public override string Text {
			get {
				return base.Text;
			}
			set {
				if (value == null)
					value = string.Empty;
				base.Text = value;
				value = value.Replace("\r", string.Empty);
				if (isComboBoxItem)
					value = value.Length == 0 ? "▼" : value + " ▼";
				else if (ContextMenu != null) {
					if (Parent is StyledContextMenu)
						value = value.Length == 0 ? "▸" : value + " ▸";
				}
				if (CheckBox.Text != value) {
					CheckBox.Text = value;
					FitToContent();
				}
			}
		}

		/// <summary>
		/// Gets or sets whether the context menu should automatically resize to fit its contents.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool AutoSize {
			get {
				return autoSize;
			}
			set {
				autoSize = value;
			}
		}

		/// <summary>
		/// Gets or sets the alignment of each item.
		/// </summary>
		[Description("Gets or sets the alignment of each item.")]
		[DefaultValue((int) ContentAlignment.MiddleCenter)]
		public override ContentAlignment TextAlign {
			get {
				return CheckBox.TextAlign;
			}
			set {
				CheckBox.TextAlign = value;
				FitToContent();
			}
		}

		/// <summary>
		/// Initializes the styled item.
		/// </summary>
		public StyledItem()
			: this(null) {
		}

		/// <summary>
		/// Initializes the styled tool strip using the specified renderer options.
		/// </summary>
		/// <param name="renderer">The renderer parameters to copy.</param>
		public StyledItem(StyleRenderer renderer) : this(nameof(StyledItem), renderer) {
		}

		/// <summary>
		/// Initializes the styled tool strip using the specified renderer options.
		/// </summary>
		/// <param name="renderer">The renderer parameters to copy.</param>
		/// <param name="text">The text to display on the item.</param>
		public StyledItem(string text, StyleRenderer renderer) {
			checkBox.ShowCheckBox = false;
			checkBox.CheckOnClick = false;
			checkBox.Font = Font;
			checkBox.ForeColor = ForeColor;
			checkBox.CheckState = CheckState;
			checkBox.TextRenderingStyle = TextRenderingHint.ClearTypeGridFit;
			checkBox.Padding = DefaultPadding;
			Renderer = new StyleRenderer(renderer);
			base.AutoSize = false;
			Name = nameof(StyledItem);
			Text = text;
		}

		/// <summary>
		///  Initializes the styled item using the specified renderer options based on the specified ToolStripMenuItem.
		/// </summary>
		/// <param name="item">The item whose parameters to assimilate.</param>
		/// <param name="renderer">The renderer to use.</param>
		public StyledItem(ToolStripItem item, StyleRenderer renderer) : this(renderer) {
			if (item == null)
				return;
			ForeColor = item.ForeColor;
			Name = item.Name;
			Text = item.Text;
			toolStripItem = item;
			TextAlign = item.TextAlign;
			Image = item.Image;
			StyledItem styledItem = item as StyledItem;
			if (styledItem == null) {
				ToolStripMenuItem menuItem = item as ToolStripMenuItem;
				if (menuItem != null) {
					ShortcutKeys = menuItem.ShortcutKeys;
					CheckOnClick = menuItem.CheckOnClick;
					CheckState = menuItem.CheckState;
				}
			} else {
				Padding = item.Padding;
				CheckOnClick = styledItem.CheckOnClick;
				CheckState = styledItem.CheckState;
				TextRenderingStyle = styledItem.TextRenderingStyle;
				Dpi = styledItem.Dpi;
				Icon = styledItem.Icon;
				ShowCheckBox = styledItem.ShowCheckBox;
				IsComboBoxItem = styledItem.IsComboBoxItem;
				AddPaddingRegardless = styledItem.AddPaddingRegardless;
				autoSize = styledItem.autoSize;
			}
			TransferItemsToContextMenu(item as ToolStripDropDownItem);
		}

		/// <summary>
		/// Called when the bounds of the item are changed.
		/// </summary>
		/// <param name="rect">The new size of the item.</param>
		protected override void SetBounds(Rectangle rect) {
			Size oldSize = Size;
			base.SetBounds(rect);
			if (!(oldSize.Width == rect.Width && oldSize.Height == rect.Height))
				FitParentToContent();
		}

		/// <summary>
		/// Calls GetAutoSize()
		/// </summary>
		/// <param name="proposedSize">The maximum size of the control</param>
		public override Size GetPreferredSize(Size proposedSize) {
			return GetAutoSize(proposedSize, true);
		}

		/// <summary>
		/// Called when the the layout of the item is calculated.
		/// </summary>
		/// <param name="e">Ignored.</param>
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
		/// Processes the specified shortcut keys and returns true if it invokes this item or a child item.
		/// </summary>
		/// <param name="keys">The keys that were pressed.</param>
		public virtual bool ProcessKeys(Keys keys) {
			if (keys == Keys.None)
				return false;
			if (keys == ShortcutKeys) {
				OnClick(EventArgs.Empty);
				return true;
			} else {
				StyledContextMenu menu = ContextMenu;
				if (menu != null) {
					foreach (ToolStripItem item in menu.Items) {
						if (((StyledItem) item).ProcessKeys(keys))
							return true;
					}
				}
				return false;
			}
		}

		/// <summary>
		/// Fits the item its content.
		/// </summary>
		public virtual void FitToContent() {
			Size oldSize = Size;
			Size newSize = GetAutoSize();
			if (oldSize != newSize)
				Size = newSize;
			if (oldSize == Size)
				Invalidate();
			else
				FitParentToContent();
		}

		private void FitParentToContent() {
			if (parent == null || !parent.AutoSize)
				Invalidate();
			else
				parent.FitToContent();
		}

		/// <summary>
		/// Updates the item renderer of each sub-menu item to the value of ItemRenderer.
		/// </summary>
		public void UpdateItemRenderer() {
			lock (SyncRoot) {
				if (contextMenu != null)
					contextMenu.UpdateRenderer();
			}
		}

		/// <summary>
		/// Fired when the font is changed.
		/// </summary>
		protected override void OnFontChanged(EventArgs e) {
			CheckBox.Font = Font;
			base.OnFontChanged(e);
			FitToContent();
		}

		internal void TransferItemsToContextMenu(ToolStripDropDownItem item) {
			if (item == null || item.DropDownItems.Count == 0)
				return;
			lock (SyncRoot) {
				if (contextMenu == null) {
					contextMenu = new StyledContextMenu();
					contextMenu.parent = this;
					contextMenu.Renderer = tempRenderer == null ? Renderer : tempRenderer;
					if (tempRenderer != null) {
						tempRenderer.Dispose();
						tempRenderer = null;
					}
				}
				ToolStripItem[] items = new ToolStripItem[item.DropDownItems.Count];
				item.DropDownItems.CopyTo(items, 0);
				foreach (ToolStripItem element in items)
					contextMenu.Items.Add(element);
				if (!StyledForm.DesignMode)
					item.DropDownItems.Clear();
			}
			Text = base.Text;
		}

		/// <summary>
		/// Renders the control with its children onto the specified image
		/// </summary>
		/// <param name="bitmap">The image to draw onto</param>
		public void DrawToBitmap(Bitmap bitmap) {
			DrawToBitmap(bitmap as Image, new Rectangle(Point.Empty, Size), true);
		}

		/// <summary>
		/// Renders the control with its children onto the specified image
		/// </summary>
		/// <param name="image">The image to draw onto</param>
		public void DrawToBitmap(Image image) {
			DrawToBitmap(image, new Rectangle(Point.Empty, Size), true);
		}

		/// <summary>
		/// Renders the control with its children onto the specified image
		/// </summary>
		/// <param name="bitmap">The image to draw onto</param>
		/// <param name="targetBounds">The bounds within which the form is rendered</param>
		public void DrawToBitmap(Bitmap bitmap, Rectangle targetBounds) {
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
		/// Fired when the drop down menu is shown.
		/// </summary>
		protected override void OnDropDownShow(EventArgs e) {
			TransferItemsToContextMenu(this);
			if (!(Owner == null || contextMenu == null || parent is StyledContextMenu))
				contextMenu.Show(Owner, 0, Owner.Height);
		}

		/// <summary>
		/// Fired when the check state is changed.
		/// </summary>
		protected override void OnCheckStateChanged(EventArgs e) {
			if (CheckState != CheckState.Unchecked)
				ShowCheckBox = true;
			CheckBox.CheckState = CheckState;
			base.OnCheckStateChanged(e);
			Invalidate();
		}

		/// <summary>
		/// Draws the item onto the graphics canvas.
		/// </summary>
		protected override void OnPaint(PaintEventArgs e) {
			DrawGdi(e.Graphics);
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
		/// Fired when the forecolor is changed.
		/// </summary>
		protected override void OnForeColorChanged(EventArgs e) {
			base.OnForeColorChanged(e);
			CheckBox.ForeColor = ForeColor;
			Invalidate();
		}

		/// <summary>
		/// Causes the item to be redrawn.
		/// </summary>
		public new void Invalidate() {
			Control parent = Parent;
			if (parent != null) {
				StyledMenuStrip menu = parent as StyledMenuStrip;
				if (menu == null) {
					StyledContextMenu context = parent as StyledContextMenu;
					if (context == null)
						parent.Invalidate();
					else
						context.Invalidate();
				} else
					menu.Invalidate();
			}
		}

		/// <summary>
		/// Causes the item to be redrawn.
		/// </summary>
		/// <param name="rect">Ignored.</param>
		public new void Invalidate(Rectangle rect) {
			Control parent = Parent;
			if (parent != null)
				parent.Invalidate();
		}

		/// <summary>
		/// Called when the item has been clicked.
		/// </summary>
		protected override void OnClick(EventArgs e) {
			if (CheckOnClick)
				Checked = !Checked;
			base.OnClick(e);
			if (toolStripItem != null)
				toolStripItem.PerformClick();
		}

		/// <summary>
		/// Called when the enabled state is changed.
		/// </summary>
		protected override void OnEnabledChanged(EventArgs e) {
			Renderer.Enabled = Enabled && (Parent == null || Parent.Enabled);
			base.OnEnabledChanged(e);
		}

		/// <summary>
		/// Called when the parent enabled state is changed.
		/// </summary>
		protected override void OnParentEnabledChanged(EventArgs e) {
			Renderer.Enabled = Enabled && (Parent == null || Parent.Enabled);
			base.OnParentEnabledChanged(e);
		}

		/// <summary>
		/// Gets the resultant size of the item if it were to be auto-sized with both grow and shrink enabled.
		/// </summary>
		public Size GetAutoSize() {
			return GetAutoSize(MaximumSize, true);
		}

		/// <summary>
		/// Gets the resultant size of the item if it were to be auto-sized with both grow and shrink enabled.
		/// </summary>
		/// <param name="maxBounds">The maximum size to allow (0 means unlimited).</param>
		/// <param name="includePadding">If true, padding is included within the size.</param>
		public virtual Size GetAutoSize(Size maxBounds, bool includePadding) {
			return CheckBox.GetAutoSize(maxBounds, includePadding);
		}

		/// <summary>
		/// Draws the item on the specified canvas.
		/// </summary>
		/// <param name="g">The canvas to use.</param>
		public void DrawGdi(Graphics g) {
			DrawGdi(g, Bounds, true, false);
		}

		/// <summary>
		/// Draws the item on the specified canvas
		/// </summary>
		/// <param name="g">The canvas to use</param>
		/// <param name="location">The location to draw the item at</param>
		/// <param name="drawChildren">Does not do anything</param>
		public void DrawGdi(Graphics g, Point location, bool drawChildren = true) {
			DrawGdi(g, new Rectangle(location, Size), false, false);
		}

		/// <summary>
		/// Draws the item on the specified canvas.
		/// </summary>
		/// <param name="g">The canvas to use.</param>
		/// <param name="alignGradientWorkaround">Whether to align the gradient with the menu strip gradient if they are misaligned on a StyledMenuStrip.</param>
		public void DrawGdi(Graphics g, bool alignGradientWorkaround) {
			DrawGdi(g, Bounds, false, alignGradientWorkaround);
		}

		/// <summary>
		/// Draws the item on the specified canvas.
		/// </summary>
		/// <param name="g">The canvas to use.</param>
		/// <param name="size">The size to draw the item at.</param>
		/// <param name="alignGradientWorkaround">Whether to align the gradient with the menu strip gradient if they are misaligned on a StyledMenuStrip.</param>
		public void DrawGdi(Graphics g, Size size, bool alignGradientWorkaround = false) {
			DrawGdi(g, new Rectangle(Point.Empty, size), true, alignGradientWorkaround);
		}

		/// <summary>
		/// Draws the item on the specified canvas
		/// </summary>
		/// <param name="g">The canvas to use</param>
		/// <param name="location">The location to draw the item at</param>
		/// <param name="drawChildren">Does not do anything</param>
		/// <param name="alignGradientWorkaround">Whether to align the gradient with the menu strip gradient if they are misaligned on a StyledMenuStrip.</param>
		public void DrawGdi(Graphics g, Point location, bool drawChildren, bool alignGradientWorkaround = false) {
			DrawGdi(g, new Rectangle(location, Size), true, alignGradientWorkaround);
		}

		/// <summary>
		/// Draws the item on the specified canvas.
		/// </summary>
		/// <param name="g">The canvas to use.</param>
		/// <param name="bounds">The bounds to draw the item within.</param>
		/// <param name="drawChildren">Does not do anything</param>
		/// <param name="alignGradientWorkaround">Whether to align the gradient with the menu strip gradient if they are misaligned on a StyledMenuStrip.</param>
		public virtual void DrawGdi(Graphics g, Rectangle bounds, bool drawChildren, bool alignGradientWorkaround) {
			CheckBox.Bounds = bounds;
			Renderer.RenderBackground(g, bounds, false, alignGradientWorkaround, BackgroundImage, BackgroundImageLayout);
			CheckBox.DrawGdi(g);
		}

		/// <summary>
		/// Gets a string that describes this instance.
		/// </summary>
		public override string ToString() {
			return nameof(StyledItem) + ": { Name: " + Name + ", Text: " + base.Text.Replace('\v', '\n') + " }";
		}

		/// <summary>
		/// Returns a clone of this item.
		/// </summary>
		public object Clone() {
			return new StyledItem(this, Renderer);
		}

		/// <summary>
		/// Called when the button is being disposed.
		/// </summary>
		/// <param name="disposing">True to dispose managed resources.</param>
		protected override void Dispose(bool disposing) {
			if (contextMenu != null) {
				contextMenu.Dispose();
				contextMenu = null;
			}
			if (itemRenderer != null) {
				itemRenderer.Dispose();
				itemRenderer = null;
			}
			if (tempRenderer != null) {
				tempRenderer.Dispose();
				tempRenderer = null;
			}
			Renderer.Dispose();
			CheckBox.Dispose();
			base.Dispose(disposing);
		}
	}
}