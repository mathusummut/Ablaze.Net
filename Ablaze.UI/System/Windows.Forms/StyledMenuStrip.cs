using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Text;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms {
	/// <summary>
	/// A customizable styled menu strip.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxItemFilter("System.Windows.Forms")]
	[DesignTimeVisible(true)]
	[DesignerCategory("CommonControls")]
	[Description("A customizable styled menu strip.")]
	[DisplayName(nameof(StyledMenuStrip))]
	public class StyledMenuStrip : MenuStrip, IItemRenderer {
		/// <summary>
		/// Fired when the background of the control is about to be painted.
		/// </summary>
		[Description("Fired when the background of the control is about to be painted.")]
		public event PaintEventHandler PaintBackground;
		private static Action<ToolStripItem, EventArgs> onParentEnabled = (Action<ToolStripItem, EventArgs>) Delegate.CreateDelegate(typeof(Action<ToolStripItem, EventArgs>), typeof(ToolStripItem).GetMethod("OnParentEnabledChanged", Reflection.BindingFlags.Instance | Reflection.BindingFlags.NonPublic));
		private StyledLabel label;
		private object SyncRoot = new object();
		private UIAnimationHandler invalidate;
		private bool alignGradientWorkaround, contextMenuShown, wasDown;
		private int wasHighlighted = -2, wasPressedOn = -2;
		private StyleRenderer itemRenderer;
		private ImageLayout backgroundLayout = ImageLayout.Stretch;
		private TextRenderingHint hint = TextRenderingHint.ClearTypeGridFit;

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
				return hint;
			}
			set {
				if (hint == value)
					return;
				hint = value;
				if (label != null)
					label.TextRenderingStyle = value;
				lock (SyncRoot) {
					StyledItem item;
					for (int i = 0; i < Items.Count; i++) {
						item = Items[i] as StyledItem;
						if (item != null)
							item.TextRenderingStyle = value;
					}
				}
				Invalidate(false);
			}
		}

		/// <summary>
		/// Gets the label associated with the menu strip.
		/// </summary>
		[Browsable(false)]
		private StyledLabel Label {
			get {
				if (label == null) {
					label = new StyledLabel();
					label.ForeColor = ForeColor;
					label.Dock = DockStyle.Fill;
					label.BackColor = Color.Transparent;
					label.TextAlign = ContentAlignment.MiddleLeft;
					label.TextRenderingStyle = hint;
					label.RenderShadow = false;
					label.Location = Point.Empty;
					Padding padding = Padding;
					label.Padding = new Padding(3, padding.Top, padding.Right, padding.Bottom);
				}
				return label;
			}
		}

		/// <summary>
		/// Gets whether an item is clicked and its context menu is currently opened.
		/// </summary>
		[Browsable(false)]
		public bool ContextMenuShown {
			get {
				return contextMenuShown;
			}
		}

		/// <summary>
		/// Whether to reduce item height by 1 in order to align the gradient with the menu strip.
		/// </summary>
		[Description("Whether to reduce item height by 1 in order to align the gradient with the menu strip.")]
		[DefaultValue(false)]
		public bool AlignGradientWorkaround {
			get {
				return alignGradientWorkaround;
			}
			set {
				if (value == alignGradientWorkaround)
					return;
				alignGradientWorkaround = value;
				Invalidate(true);
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
		/// Gets the renderer used for styling items.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public StyleRenderer ItemRenderer {
			get {
				return itemRenderer == null ? Renderer as StyleRenderer : itemRenderer;
			}
			set {
				itemRenderer = value;
				if (value == null)
					value = Renderer as StyleRenderer;
				value.FunctionToCallOnRefresh = invalidate;
				lock (SyncRoot) {
					StyledItem item;
					for (int i = 0; i < Items.Count; i++) {
						item = Items[i] as StyledItem;
						if (item != null) {
							item.Renderer.CopyConfigFrom(value);
							item.UpdateItemRenderer();
						}
					}
				}
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
		/// Gets the default padding (none).
		/// </summary>
		protected override Padding DefaultPadding {
			get {
				return Padding.Empty;
			}
		}

		/// <summary>
		/// Gets or sets the text displayed on the menu strip.
		/// </summary>
		[Description("Gets or sets the text displayed on the menu strip.")]
		[DefaultValue("")]
		public override string Text {
			get {
				return label == null ? string.Empty : label.Text;
			}
			set {
				if (value == null)
					value = string.Empty;
				if (label == null && value.Length == 0)
					return;
				string text = Label.Text;
				label.Text = value;
				if (label.Text != text) {
					OnTextChanged(EventArgs.Empty);
					Invalidate(false);
				}
			}
		}

		/// <summary>
		/// Initializes the styled menu strip.
		/// </summary>
		public StyledMenuStrip() {
			CheckForIllegalCrossThreadCalls = false;
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserMouse | ControlStyles.CacheText, true);
			SetStyle(ControlStyles.ResizeRedraw, false);
			ForeColor = Color.Black;
			AutoSize = false;
			Name = nameof(StyledMenuStrip);
			invalidate = UIAnimator.GetFunctionToInvalidateControlOnUpdate(this);
			StyleRenderer renderer = new StyleRenderer(invalidate);
			renderer.RoundCornerRadius = 0f;
			renderer.RoundedEdges = false;
			Renderer = renderer;
		}

		/// <summary>
		/// Called when the menu text color has changed.
		/// </summary>
		/// <param name="e">Ignored.</param>
		protected override void OnForeColorChanged(EventArgs e) {
			base.OnForeColorChanged(e);
			if (label != null) {
				label.ForeColor = ForeColor;
				Invalidate(false);
			}
		}

		/// <summary>
		/// Called when a context menu strip is assigned to the context menu.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnContextMenuStripChanged(EventArgs e) {
			base.OnContextMenuStripChanged(e);
			if (ContextMenuStrip != null)
				ContextMenuStrip.Renderer = ItemRenderer;
		}

		/// <summary>
		/// Does nothing.
		/// </summary>
		protected override void InitLayout() {
		}
		/// <summary>
		/// Does nothing.
		/// </summary>
		protected override void SetDisplayedItems() {
		}
		/// <summary>
		/// Does nothing.
		/// </summary>
		protected override void OnLayoutStyleChanged(EventArgs e) {
		}
		/// <summary>
		/// Does nothing.
		/// </summary>
		protected override void OnLayoutCompleted(EventArgs e) {
		}
		/// <summary>
		/// Does nothing.
		/// </summary>
		protected override void OnLayout(LayoutEventArgs e) {
		}

		/// <summary>
		/// Updates the renderer by copying it to every item in the menu strip.
		/// </summary>
		public void UpdateRenderer() {
			StyleRenderer renderer = ItemRenderer as StyleRenderer;
			if (renderer == null) {
				renderer = new StyleRenderer(invalidate);
				Renderer = renderer;
			}
			lock (SyncRoot) {
				StyledItem item;
				ToolStripItem temp;
				for (int i = 0; i < Items.Count; i++) {
					temp = Items[i];
					temp.Font = Font;
					item = temp as StyledItem;
					if (item != null && item.Renderer != renderer) {
						item.Renderer.CopyConfigFrom(renderer);
						item.UpdateItemRenderer();
					}
				}
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
		/// Creates a StyledItem instance from the specified parameters.
		/// </summary>
		/// <param name="text">The text to show on the item.</param>
		/// <param name="image">The image to show on the item (can be null).</param>
		/// <param name="onClick">To be called when the item is clicked.</param>
		protected override ToolStripItem CreateDefaultItem(string text, Image image, EventHandler onClick) {
			StyledItem item = new StyledItem(text, null);
			item.Image = image;
			item.Font = Font;
			item.TextAlign = ContentAlignment.MiddleCenter;
			item.Renderer.FunctionToCallOnRefresh = invalidate;
			item.FitToContent();
			item.Click += onClick;
			return item;
		}

		/// <summary>
		/// Fired when an item is added to the menu strip.
		/// </summary>
		protected override void OnItemAdded(ToolStripItemEventArgs e) {
			StyledItem item = e.Item as StyledItem;
			if (item == null) {
				lock (SyncRoot) {
					Items.Add(new StyledItem(e.Item, ItemRenderer));
					Items.Remove(e.Item);
				}
			} else {
				item.parent = this;
				item.TextAlign = ContentAlignment.MiddleCenter;
				item.Renderer.FunctionToCallOnRefresh = invalidate;
				item.FitToContent();
				CheckAutoSize();
			}
		}

		/// <summary>
		/// Fired when an item is removed from the context menu.
		/// </summary>
		/// <param name="e">The tool strip item that was removed.</param>
		protected override void OnItemRemoved(ToolStripItemEventArgs e) {
			StyledItem item = e.Item as StyledItem;
			if (item != null)
				item.parent = null;
			base.OnItemRemoved(e);
			CheckAutoSize();
		}

		/// <summary>
		/// Updates the item renderer.
		/// </summary>
		protected override void OnRendererChanged(EventArgs e) {
			base.OnRendererChanged(e);
			if (!(Renderer is StyleRenderer))
				Renderer = new StyleRenderer(invalidate);
		}

		private StyleRenderer FromIndex(int index) {
			if (index >= 0) {
				StyledItem item = Items[index] as StyledItem;
				return item == null ? null : item.Renderer;
			} else if (index == -1)
				return Renderer as StyleRenderer;
			else
				return null;
		}

		internal void ContextMenuClosed() {
			contextMenuShown = false;
			StyleRenderer renderer;
			StyledItem item;
			if (wasPressedOn != -2) {
				if (wasPressedOn == -1) {
					renderer = Renderer as StyleRenderer;
					if (renderer != null)
						renderer.Pressed = false;
				} else {
					item = Items[wasPressedOn] as StyledItem;
					if (item != null)
						item.Renderer.Pressed = false;
				}
				wasPressedOn = -2;
			}
			if (wasHighlighted != -2) {
				if (wasHighlighted == -1) {
					renderer = Renderer as StyleRenderer;
					if (renderer != null)
						renderer.MouseHovering = false;
				} else {
					item = Items[wasHighlighted] as StyledItem;
					if (item != null)
						item.Renderer.MouseHovering = false;
				}
				wasHighlighted = -2;
			}
		}

		/// <summary>
		/// Fired when the mouse is pressed on the control.
		/// </summary>
		protected override void OnMouseDown(MouseEventArgs mevent) {
			if (mevent.Button == MouseButtons.Left) {
				wasDown = true;
				wasPressedOn = GetItemAt(mevent.X);
				StyleRenderer renderer = FromIndex(wasPressedOn);
				if (renderer != null)
					renderer.Pressed = true;
			}
		}

		/// <summary>
		/// Fired when the mouse is moved over the context menu.
		/// </summary>
		protected override void OnMouseMove(MouseEventArgs mea) {
			if (!contextMenuShown)
				Capture = true;
			if (Size.Contains(mea.Location)) {
				int itemIndex = GetItemAt(mea.X);
				if (wasHighlighted == itemIndex || (contextMenuShown && itemIndex == -1))
					return;
				StyleRenderer renderer = FromIndex(wasHighlighted);
				if (renderer != null)
					renderer.MarkMouseHasLeft();
				renderer = FromIndex(itemIndex);
				if (renderer != null)
					renderer.MouseHovering = true;
				if ((wasPressedOn != -2 && MouseListener.IsButtonDown(MouseButtons.Left)) || contextMenuShown) {
					if (!(wasPressedOn == itemIndex || wasPressedOn == -2)) {
						if (wasPressedOn == -1) {
							StyleRenderer menuRenderer = Renderer as StyleRenderer;
							if (menuRenderer != null)
								menuRenderer.MarkMouseHasLeft();
						} else {
							StyledItem item = Items[wasPressedOn] as StyledItem;
							if (item != null)
								item.Renderer.MarkMouseHasLeft();
						}
					}
					if (renderer != null)
						renderer.Pressed = true;
					wasPressedOn = itemIndex;
				}
				wasHighlighted = itemIndex;
				if (contextMenuShown)
					OpenMenuItem(itemIndex, false);
			} else if (!contextMenuShown) {
				StyleRenderer renderer;
				if (wasPressedOn == -2) {
					if (!contextMenuShown)
						Capture = false;
				} else {
					renderer = FromIndex(wasPressedOn);
					if (renderer != null)
						renderer.MarkMouseHasLeft();
				}
				if (wasHighlighted == -2)
					return;
				renderer = FromIndex(wasHighlighted);
				if (renderer != null)
					renderer.MouseHovering = false;
				wasHighlighted = -2;
			}
		}

		/// <summary>
		/// Fires when the mouse is released on the control.
		/// </summary>
		protected override void OnMouseUp(MouseEventArgs mevent) {
			if (contextMenuShown || !wasDown)
				return;
			wasDown = false;
			base.OnMouseUp(mevent);
			if (mevent.Button == MouseButtons.Left) {
				int item = GetItemAt(mevent.X);
				if (Size.Contains(mevent.Location))
					OpenMenuItem(item, true);
				if (!contextMenuShown) {
					if (wasPressedOn != -2) {
						StyleRenderer renderer = FromIndex(wasPressedOn);
						if (renderer != null)
							renderer.Pressed = false;
						wasPressedOn = -2;
					}
					if (wasHighlighted != -2) {
						StyleRenderer renderer = FromIndex(wasHighlighted);
						if (renderer != null)
							renderer.MouseHovering = false;
						wasHighlighted = -2;
					}
				}
			}
		}

		private void OpenMenuItem(int index, bool clicked) {
			wasPressedOn = index;
			if (index == -1)
				return;
			StyledItem item = Items[index] as StyledItem;
			if (item == null || !item.Enabled)
				return;
			if (item.ContextMenu == null || item.ContextMenu.Items.Count == 0) {
				if (clicked)
					item.PerformClick();
			} else {
				contextMenuShown = true;
				if (!clicked && MouseListener.IsButtonDown(MouseButtons.Left))
					item.ContextMenu.wasPressedOn = 0;
				item.ContextMenu.Alignment = ContentAlignment.MiddleLeft;
				item.ContextMenu.Show(this, GetLocationOf(index), Height);
				contextMenuShown = true;
			}
		}

		/// <summary>
		/// Gets the item index located at the specified X-coordinate. Returns -1 if there are no items at X.
		/// </summary>
		/// <param name="x">The X-coordinate to get the item at.</param>
		public int GetItemAt(int x) {
			int accumulator = Padding.Left;
			if (x < accumulator || x > ClientSize.Width || Items.Count == 0)
				return -1;
			lock (SyncRoot) {
				for (int i = 0; i < Items.Count; i++) {
					accumulator += Items[i].Width;
					if (accumulator > x)
						return i;
				}
			}
			return -1;
		}

		/// <summary>
		/// Gets the X-coordinate of the item at the specified index. Returns -1 if the index is invalid.
		/// </summary>
		/// <param name="index">The index of the item to get the X-coordinate of.</param>
		public int GetLocationOf(int index) {
			if (index < 0 || index >= Items.Count)
				return -1;
			int x = Padding.Left;
			lock (SyncRoot) {
				for (int i = 0; i < index; i++)
					x += Items[i].Width;
			}
			return x;
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
			if (Items.Count == 0)
				return new Size(1, 1);
			else {
				int totalWidth = 0, maxHeight = 1;
				Size size;
				StyledItem item;
				lock (SyncRoot) {
					for (int i = 0; i < Items.Count; i++) {
						item = Items[i] as StyledItem;
						if (item != null) {
							item.FitToContent();
							size = item.Size;
							totalWidth += size.Width;
							if (size.Height > maxHeight)
								maxHeight = size.Height;
						}
					}
				}
				if (label != null) {
					Size labelSize = label.GetAutoSize(Size.Empty, false);
					totalWidth += 3 + labelSize.Width;
					if (labelSize.Height > maxHeight)
						maxHeight = labelSize.Height;
				}
				if (includePadding) {
					Padding padding = Padding;
					totalWidth += padding.Horizontal;
					maxHeight += padding.Vertical;
				}
				if (totalWidth <= 0)
					totalWidth = 1;
				if (maxBounds.Width > 0 && totalWidth > maxBounds.Width)
					totalWidth = maxBounds.Width;
				if (maxBounds.Height > 0 && maxHeight > maxBounds.Height)
					maxHeight = maxBounds.Height;
				return new Size(totalWidth, maxHeight);
			}
		}

		/// <summary>
		/// Calls GetAutoSize()
		/// </summary>
		/// <param name="proposedSize">The maximum size of the control</param>
		public override Size GetPreferredSize(Size proposedSize) {
			return GetAutoSize(proposedSize, true);
		}

		/// <summary>
		/// Called when the font of the menu strip has changed.
		/// </summary>
		/// <param name="e">Ignored.</param>
		protected override void OnFontChanged(EventArgs e) {
			base.OnFontChanged(e);
			if (label != null)
				label.Font = Font;
			CheckAutoSize();
		}

		/// <summary>
		/// Sets the size of the control to the autosize result.
		/// </summary>
		public virtual void FitToContent() {
			Size = GetAutoSize();
			Invalidate();
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
		/// Processes the specified shortcut keys and returns true if it invokes this item or a child item.
		/// </summary>
		/// <param name="keys">The keys that were pressed.</param>
		public virtual bool ProcessKeys(Keys keys) {
			if (keys == Keys.None)
				return false;
			lock (SyncRoot) {
				StyledItem styledItem;
				foreach (ToolStripItem item in Items) {
					styledItem = item as StyledItem;
					if (styledItem != null && styledItem.ProcessKeys(keys))
						return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Called when the enabled state is changed.
		/// </summary>
		protected override void OnEnabledChanged(EventArgs e) {
			StyleRenderer renderer = Renderer as StyleRenderer;
			if (renderer != null)
				renderer.Enabled = Enabled;
			base.OnEnabledChanged(e);
			lock (SyncRoot) {
				foreach (ToolStripItem item in Items)
					onParentEnabled(item, e);
			}
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
		/// Called when the padding of the menu strip has changed.
		/// </summary>
		/// <param name="e">Ignored.</param>
		protected override void OnPaddingChanged(EventArgs e) {
			base.OnPaddingChanged(e);
			if (label != null) {
				Padding padding = Padding;
				label.Padding = new Padding(3, padding.Top, padding.Right, padding.Bottom);
			}
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
			if ((widthSpecified && width != oldSize.Width) || (heightSpecified && height != oldSize.Height))
				Invalidate(false);
		}

		/// <summary>
		/// Not implemented.
		/// </summary>
		protected override void OnPaintBackground(PaintEventArgs pevent) {
		}

		/// <summary>
		/// Raises a paint event.
		/// </summary>
		/// <param name="handler">The event to call.</param>
		/// <param name="sender">The sender that needs to be painted.</param>
		/// <param name="e">The PaintEventArgs containing the appropriate graphics object.</param>
		protected virtual void RaisePaintEvent(PaintEventHandler handler, object sender, PaintEventArgs e) {
			if (handler != null)
				handler(sender, e);
		}

		/// <summary>
		/// Draws the menu strip onto the specified graphics canvas.
		/// </summary>
		/// <param name="g">The canvas to draw the menu strip on.</param>
		public void DrawGdi(Graphics g) {
			DrawGdi(g, Location);
		}

		/// <summary>
		/// Draws the menu strip onto the specified graphics canvas at the specified location.
		/// </summary>
		/// <param name="g">The canvas to draw the menu strip on.</param>
		/// <param name="location">The location to draw the menu strip at.</param>
		/// <param name="drawChildren">Whether to draw the child controls</param>
		public void DrawGdi(Graphics g, Point location, bool drawChildren = true) {
			if (!location.IsEmpty)
				g.TranslateTransform(location.X, location.Y);
			Size clientSize = ClientSize;
			Rectangle clientRect = new Rectangle(Point.Empty, clientSize);
			Region oldClipRegion = g.Clip;
			g.SetClip(clientRect);
			PaintEventArgs args = new PaintEventArgs(g, clientRect);
			RaisePaintEvent(PaintBackground, this, args);
			((StyleRenderer) Renderer).RenderBackground(g, new Rectangle(Point.Empty, clientSize), false, false, BackgroundImage, BackgroundImageLayout);
			Padding padding = Padding;
			if (!(padding.Left == 0 && padding.Top == 0))
				g.TranslateTransform(padding.Left, padding.Top);
			int height = clientSize.Height;
			int currentWidth, cumulative = 0;
			StyledItem item;
			Size itemSize;
			Rectangle clipRect;
			lock (SyncRoot) {
				for (int i = 0; i < Items.Count; i++) {
					item = Items[i] as StyledItem;
					if (item != null) {
						currentWidth = item.Width;
						cumulative += currentWidth;
						itemSize = new Size(currentWidth, height);
						clipRect = new Rectangle(Point.Empty, itemSize);
						g.SetClip(clipRect);
						item.DrawGdi(g, clipRect.Size, alignGradientWorkaround);
						g.TranslateTransform(currentWidth, 0f);
					}
				}
			}
			if (!(padding.Left == 0 && padding.Top == 0))
				g.TranslateTransform(-padding.Left, -padding.Top);
			if (label != null) {
				itemSize = new Size(clientSize.Width - cumulative, height);
				clipRect = new Rectangle(Point.Empty, itemSize);
				g.SetClip(clipRect);
				label.Size = itemSize;
				label.DrawGdi(g);
			}
			if (cumulative != 0)
				g.TranslateTransform(-cumulative, 0f);
			g.SetClip(clientRect);
			RaisePaintEvent(StyleRenderer.PaintEventKey, args);
			if (drawChildren)
				g.DrawControls(Controls, Point.Empty, clientRect, true);
			if (!location.IsEmpty)
				g.TranslateTransform(-location.X, -location.Y);
			g.Clip = oldClipRegion;
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
		/// Paints the menu strip and its items
		/// </summary>
		/// <param name="e">The graphics canvas to draw on</param>
		protected override void OnPaint(PaintEventArgs e) {
			DrawGdi(e.Graphics, Point.Empty, false);
		}

		/// <summary>
		/// Gets a string that represents this instance.
		/// </summary>
		public override string ToString() {
			StringBuilder builder = new StringBuilder();
			builder.Append(nameof(StyledMenuStrip));
			builder.Append(": ");
			builder.Append(Name);
			builder.Append(", Text: ");
			builder.Append(Text);
			lock (SyncRoot) {
				if (Items.Count != 0) {
					builder.Append(" { ");
					for (int i = 0; i < Items.Count; i++) {
						if (Items[i].Text.Length != 0) {
							builder.Append(Items[i].Text);
							if (i != Items.Count - 1)
								builder.Append(", ");
						}
					}
					builder.Append(" }");
				}
			}
			return builder.ToString();
		}

		/// <summary>
		/// Disposes the menu strip.
		/// </summary>
		/// <param name="disposing">Whether to dispose managed resources.</param>
		protected override void Dispose(bool disposing) {
			base.Dispose(disposing);
			if (label != null) {
				label.Dispose();
				label = null;
			}
			if (itemRenderer != null) {
				itemRenderer.Dispose();
				itemRenderer = null;
			}
		}
	}
}