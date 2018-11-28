using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Text;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms {
	/// <summary>
	/// A lightweight customizable styled menu strip control.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxItemFilter("System.Windows.Forms")]
	[DesignTimeVisible(true)]
	[DesignerCategory("CommonControls")]
	[Description("A lightweight customizable styled menu strip control.")]
	[DisplayName(nameof(StyledMenuStripControl))]
	public class StyledMenuStripControl : Control, IItemRenderer {
		/// <summary>
		/// Fired when the background of the control is about to be painted.
		/// </summary>
		[Description("Fired when the background of the control is about to be painted.")]
		public event PaintEventHandler PaintBackground;
		/// <summary>
		/// Fired when the background of an item is about to be painted. The sender is the item.
		/// </summary>
		[Description("Fired when the background of an item is about to be painted.")]
		public event PaintEventHandler PaintItemBackground;
		private StyledLabel label;
		private UIAnimationHandler invalidate;
		private bool alignGradientWorkaround, contextMenuShown;
		private int wasHighlighted = -2, wasPressedOn = -2;
		private StyleRenderer itemRenderer = new StyleRenderer();
		/// <summary>
		/// A list of all the child items.
		/// </summary>
		private SyncedList<ToolStripItem> items = new SyncedList<ToolStripItem>();
		private TextRenderingHint hint = TextRenderingHint.ClearTypeGridFit;

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
				StyledItem styledItem;
				foreach (ToolStripItem item in Items) {
					styledItem = item as StyledItem;
					if (styledItem != null)
						styledItem.TextRenderingStyle = value;
				}
				Invalidate(false);
			}
		}

		/// <summary>
		/// Gets the renderer that draws the menu background.
		/// </summary>
		public StyleRenderer Renderer {
			get;
			private set;
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
		/// Gets a list of all the child items.
		/// </summary>
		[Description("Gets a list of all the child items.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public SyncedList<ToolStripItem> Items {
			get {
				return items;
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
		/// Gets the renderer used for styling items.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public StyleRenderer ItemRenderer {
			get {
				return itemRenderer;
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
		/// Gets or sets the docking location of the menu strip control.
		/// </summary>
		[DefaultValue(DockStyle.Top)]
		[Description("Gets or sets the docking location of the menu strip control.")]
		public override DockStyle Dock {
			get {
				return base.Dock;
			}
			set {
				base.Dock = value;
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
		public StyledMenuStripControl() {
			items.Filter = Filter;
			items.ShouldRemove = ShouldRemove;
			CheckForIllegalCrossThreadCalls = false;
			invalidate = UIAnimator.GetFunctionToInvalidateControlOnUpdate(this);
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
			Name = nameof(StyledMenuStripControl);
			Renderer = new StyleRenderer(invalidate) {
				RoundCornerRadius = 0f,
				RoundedEdges = false
			};
			Height = 24;
			itemRenderer.Invert = true;
			itemRenderer.FunctionToCallOnRefresh = invalidate;
			itemRenderer.RoundCornerRadius = 0f;
			itemRenderer.RoundedEdges = false;
			AutoSize = false;
			Dock = DockStyle.Top;
		}

		private bool Filter(ToolStripItem value) {
			if (value == null)
				return false;
			StyledItem item = value as StyledItem;
			if (item == null) {
				items.Add(new StyledItem(value, itemRenderer));
				return false;
			} else {
				item.parent = this;
				item.TextAlign = ContentAlignment.MiddleCenter;
				item.Renderer.FunctionToCallOnRefresh = invalidate;
				item.FitToContent();
				return true;
			}
		}

		private bool ShouldRemove(ToolStripItem value) {
			StyledItem item = value as StyledItem;
			if (item != null)
				item.parent = null;
			CheckAutoSize();
			return true;
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
		/// <param name="e">Ignored.</param>
		protected override void OnContextMenuStripChanged(EventArgs e) {
			base.OnContextMenuStripChanged(e);
			if (ContextMenuStrip != null)
				ContextMenuStrip.Renderer = itemRenderer;
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
		/// Updates the renderer by copying it to every item in the menu strip.
		/// </summary>
		public void UpdateRenderer() {
			StyleRenderer renderer = Renderer as StyleRenderer;
			if (renderer == null) {
				renderer = new StyleRenderer(invalidate);
				Renderer = renderer;
			}
			StyledItem styledItem;
			foreach (ToolStripItem item in Items) {
				styledItem = item as StyledItem;
				if (styledItem != null) {
					styledItem.Renderer.CopyConfigFrom(renderer);
					styledItem.UpdateItemRenderer();
				}
			}
		}

		private StyleRenderer FromIndex(int index) {
			if (index >= 0) {
				StyledItem item = items[index] as StyledItem;
				return item == null ? null : item.Renderer;
			} else if (index == -1)
				return Renderer;
			else
				return null;
		}

		internal void ContextMenuClosed() {
			contextMenuShown = false;
			if (wasPressedOn != -2) {
				if (wasPressedOn == -1)
					Renderer.Pressed = false;
				else {
					StyledItem item = items[wasPressedOn] as StyledItem;
					if (item != null)
						item.Renderer.Pressed = false;
				}
				wasPressedOn = -2;
			}
			if (wasHighlighted != -2) {
				if (wasHighlighted == -1)
					Renderer.MouseHovering = false;
				else {
					StyledItem item = items[wasHighlighted] as StyledItem;
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
						if (wasPressedOn == -1)
							Renderer.MarkMouseHasLeft();
						else {
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
			if (contextMenuShown)
				return;
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
			StyledItem item = items[index] as StyledItem;
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
			int index = 0;
			foreach (ToolStripItem item in items) {
				accumulator += item.Width;
				if (accumulator > x)
					return index;
				index++;
			}
			return -1;
		}

		/// <summary>
		/// Gets the X-coordinate of the item at the specified index. Returns -1 if the index is invalid.
		/// </summary>
		/// <param name="index">The index of the item to get the X-coordinate of.</param>
		public int GetLocationOf(int index) {
			if (index < 0 || index >= items.Count)
				return -1;
			int x = Padding.Left;
			lock (items.SyncRoot) {
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
				StyledItem styledItem;
				foreach (ToolStripItem item in items) {
					styledItem = item as StyledItem;
					if (styledItem != null) {
						styledItem.FitToContent();
						size = item.Size;
						totalWidth += size.Width;
						if (size.Height > maxHeight)
							maxHeight = size.Height;
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
				if (totalWidth == 0)
					totalWidth = 1;
				if (maxBounds.Width > 0 && totalWidth > maxBounds.Width)
					totalWidth = maxBounds.Width;
				if (maxBounds.Height > 0 && maxHeight > maxBounds.Height)
					maxHeight = maxBounds.Height;
				return new Size(totalWidth, maxHeight);
			}
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
		/// Called when the enabled state is changed.
		/// </summary>
		protected override void OnEnabledChanged(EventArgs e) {
			Renderer.Enabled = Enabled;
			base.OnEnabledChanged(e);
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
			base.SetBoundsCore(x, y, width, height, specified);
		}

		/// <summary>
		/// Not implemented.
		/// </summary>
		protected override void OnPaintBackground(PaintEventArgs pevent) {
		}

		/// <summary>
		/// Processes the specified shortcut keys and returns true if it invokes this item or a child item.
		/// </summary>
		/// <param name="keys">The keys that were pressed.</param>
		public virtual bool ProcessKeys(Keys keys) {
			if (keys == Keys.None)
				return false;
			StyledItem styledItem;
			foreach (ToolStripItem item in Items) {
				styledItem = item as StyledItem;
				if (styledItem != null && styledItem.ProcessKeys(keys))
					return true;
			}
			return false;
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
		public void Draw(Graphics g) {
			Draw(g, Location);
		}

		/// <summary>
		/// Draws the menu strip onto the specified graphics canvas at the specified location.
		/// </summary>
		/// <param name="g">The canvas to draw the menu strip on.</param>
		/// <param name="location">The location to draw the menu strip at.</param>
		public void Draw(Graphics g, Point location) {
			if (location != Point.Empty)
				g.TranslateTransform(location.X, location.Y);
			Size clientSize = ClientSize;
			Rectangle clientRect = new Rectangle(Point.Empty, clientSize);
			g.SetClip(clientRect);
			PaintEventArgs args = new PaintEventArgs(g, clientRect);
			RaisePaintEvent(PaintBackground, this, args);
			Renderer.RenderBackground(g, new Rectangle(Point.Empty, clientSize));
			Padding padding = Padding;
			if (!(padding.Left == 0 && padding.Top == 0))
				g.TranslateTransform(padding.Left, padding.Top);
			int height = clientSize.Height;
			int currentWidth, cumulative = 0;
			Size itemSize;
			Rectangle clipRect;
			StyledItem styledItem;
			foreach (ToolStripItem item in Items) {
				styledItem = item as StyledItem;
				if (styledItem != null) {
					currentWidth = styledItem.Width;
					cumulative += currentWidth;
					itemSize = new Size(currentWidth, height);
					clipRect = new Rectangle(Point.Empty, itemSize);
					g.SetClip(clipRect);
					RaisePaintEvent(PaintItemBackground, styledItem, new PaintEventArgs(g, clipRect));
					styledItem.Draw(g, clipRect.Size, alignGradientWorkaround);
					g.TranslateTransform(currentWidth, 0f);
				}
			}
			if (!(padding.Left == 0 && padding.Top == 0))
				g.TranslateTransform(-padding.Left, -padding.Top);
			if (label != null) {
				itemSize = new Size(clientSize.Width - cumulative, height);
				clipRect = new Rectangle(Point.Empty, itemSize);
				g.SetClip(clipRect);
				label.Size = itemSize;
				label.Draw(g);
			}
			g.TranslateTransform(-cumulative, 0f);
			g.SetClip(clientRect);
			RaisePaintEvent(StyleRenderer.PaintEventKey, args);
			if (location != Point.Empty)
				g.TranslateTransform(-location.X, -location.Y);
			g.ResetClip();
		}

		/// <summary>
		/// Paints the menu strip and its items.
		/// </summary>
		protected override void OnPaint(PaintEventArgs e) {
			Draw(e.Graphics, Point.Empty);
		}

		/// <summary>
		/// Gets a string that represents this instance.
		/// </summary>
		public override string ToString() {
			StringBuilder builder = new StringBuilder();
			builder.Append(nameof(StyledMenuStripControl));
			builder.Append(": ");
			builder.Append(Name);
			builder.Append(", Text: ");
			builder.Append(Text);
			lock (Items.SyncRoot) {
				if (Items.Items.Count != 0) {
					builder.Append(" { ");
					for (int i = 0; i < Items.Items.Count; i++) {
						if (Items.Items[i].Text.Length != 0) {
							builder.Append(Items.Items[i].Text);
							if (i != Items.Items.Count - 1)
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
			itemRenderer.Dispose();
		}
	}
}