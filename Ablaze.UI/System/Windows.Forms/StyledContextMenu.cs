using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Platforms.Windows;
using System.Reflection;
using System.Text;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms {
	/// <summary>
	/// A customizable styled context menu.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxItemFilter("System.Windows.Forms")]
	[DesignTimeVisible(true)]
	[DesignerCategory("Components")]
	[Description("A customizable styled context menu.")]
	[DisplayName(nameof(StyledContextMenu))]
	public class StyledContextMenu : ContextMenuStrip, IItemRenderer {
		private static StyledContextMenu lastContextMenu;
		private static object contextMenuSyncRoot = new object();
		/// <summary>
		/// Fired when the background of the control is about to be painted.
		/// </summary>
		[Description("Fired when the background of the control is about to be painted.")]
		public event PaintEventHandler PaintBackground;
		private static Action<ToolStripItem, EventArgs> onParentEnabled = (Action<ToolStripItem, EventArgs>) Delegate.CreateDelegate(typeof(Action<ToolStripItem, EventArgs>), typeof(ToolStripItem).GetMethod("OnParentEnabledChanged", Reflection.BindingFlags.Instance | Reflection.BindingFlags.NonPublic));
		private static PropertyInfo constrained = typeof(ToolStripDropDown).GetProperty("WorkingAreaConstrained", BindingFlags.Instance | BindingFlags.NonPublic);
		internal StyledItem parent;
		private object SyncRoot = new object();
		private UIAnimationHandler invalidate;
		internal int wasPressedOn = -1, maxItemHeight = 1;
		private int wasHighlighted = -1;
		private bool isMouseDown, autoSize = true;
		private ImageLayout backgroundLayout = ImageLayout.Stretch;
		internal ContentAlignment alignment = ContentAlignment.MiddleLeft;
		private byte opacity = 1;

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
		/// Gets the last control that caused this context menu to be displayed.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Control SourceControl {
			get {
				Control ctrl = base.SourceControl;
				if (ctrl == null)
					ctrl = parent == null ? null : parent.parent as Control;
				return ctrl;
			}
		}

		internal byte OpacityInner {
			get {
				return opacity;
			}
			set {
				opacity = value;
				if (!IsDisposed)
					NativeApi.SetLayeredWindowAttributes(Handle, COLORREF.Empty, value, BlendFlags.ULW_ALPHA);
			}
		}

		/// <summary>
		/// Gets the maximum item size.
		/// </summary>
		protected override Size MaxItemSize {
			get {
				if (maxItemHeight == 0)
					FitToContent();
				return new Size(Width, maxItemHeight);
			}
		}

		/// <summary>
		/// Gets whether a fade-in animation is currently taking place.
		/// </summary>
		[Browsable(false)]
		public bool FadingIn {
			get;
			private set;
		}

		/// <summary>
		/// Adds a drop-shadow effect.
		/// </summary>
		protected override CreateParams CreateParams {
			get {
				CreateParams cp = base.CreateParams;
				if (!StyledForm.DesignMode)
					cp.ExStyle |= (int) ExtendedWindowStyle.Layered;
				return cp;
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
		/// Gets or sets the alignment of each item.
		/// </summary>
		[Description("Gets or sets the alignment of each item.")]
		[DefaultValue((int) ContentAlignment.MiddleCenter)]
		public ContentAlignment Alignment {
			get {
				return alignment;
			}
			set {
				if (value == alignment)
					return;
				alignment = value;
				StyledItem item;
				lock (SyncRoot) {
					for (int i = 0; i < Items.Count; i++) {
						item = Items[i] as StyledItem;
						if (item != null)
							item.TextAlign = alignment;
					}
				}
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
		/// Gets or sets whether the context menu should automatically close when the user clicks outside.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool AutoClose {
			get;
			set;
		}

		/// <summary>
		/// Gets the renderer used for styling items.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public StyleRenderer ItemRenderer {
			get {
				return Renderer as StyleRenderer;
			}
		}

		/// <summary>
		/// Initializes the styled context menu.
		/// </summary>
		public StyledContextMenu() {
			CheckForIllegalCrossThreadCalls = false;
			if (!StyledForm.DesignMode)
				base.AutoClose = false;
			AutoClose = true;
			Name = nameof(StyledContextMenu);
			invalidate = UIAnimator.GetFunctionToInvalidateControlOnUpdate(this);
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque | ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserMouse | ControlStyles.CacheText, true);
			SetStyle(ControlStyles.ResizeRedraw, false);
			AllowTransparency = true;
			StyleRenderer renderer = new StyleRenderer(invalidate);
			renderer.RoundCornerRadius = 0f;
			renderer.RoundedEdges = false;
			Renderer = renderer;
			base.AutoSize = false;
			Size = new Size(100, 10);
			if (StyledForm.DesignMode) {
				MouseDown += Menu_MouseDown;
				MouseMove += Menu_CursorMove;
				MouseUp += Menu_MouseUp;
			}
		}

		/// <summary>
		/// Initializes the styled context menu.
		/// </summary>
		public StyledContextMenu(IContainer component) : this() {
			if (component != null)
				component.Add(this);
		}

		/// <summary>
		/// Not implemented.
		/// </summary>
		public new void SuspendLayout() {
		}
		/// <summary>
		/// Not implemented.
		/// </summary>
		public new void ResumeLayout() {
		}
		/// <summary>
		/// Not implemented.
		/// </summary>
		public new void ResumeLayout(bool val) {
		}

		/// <summary>
		/// Updates the size of the context menu to fit its contents.
		/// </summary>
		public virtual void FitToContent() {
			ClientSize = GetAutoSize();
			Invalidate();
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
				Invalidate(false);
		}

		/// <summary>
		/// Gets the resultant size of the context menu if it were to be auto-sized with both grow and shrink enabled.
		/// </summary>
		public Size GetAutoSize() {
			return GetAutoSize(MaximumSize, true);
		}

		/// <summary>
		/// Gets the size of the context menu if it were to fit its contents.
		/// </summary>
		/// <param name="maxBounds">The maximum size to allow (0 means unlimited).</param>
		/// <param name="includePadding">Ignored.</param>
		public virtual Size GetAutoSize(Size maxBounds, bool includePadding) {
			if (Items.Count == 0)
				return new Size(1, 1);
			else {
				int maxWidth = 1;
				maxItemHeight = 1;
				Size size;
				lock (SyncRoot) {
					bool addPadding = false;
					StyledItem item;
					for (int i = 0; i < Items.Count; i++) {
						item = Items[i] as StyledItem;
						if (item != null && (item.ShowCheckBox || item.Image != null || item.Icon != null)) {
							addPadding = true;
							break;
						}
					}
					for (int i = 0; i < Items.Count; i++) {
						item = Items[i] as StyledItem;
						if (item != null) {
							item.AddPaddingRegardless = addPadding;
							item.FitToContent();
							size = item.Size;
							if (size.Width > maxWidth)
								maxWidth = size.Width;
							if (size.Height > maxItemHeight)
								maxItemHeight = size.Height;
						}
					}
				}
				if (maxBounds.Width > 0 && maxWidth > maxBounds.Width)
					maxWidth = maxBounds.Width;
				int totalHeight = maxItemHeight * Items.Count;
				if (maxBounds.Height > 0 && totalHeight > maxBounds.Height)
					totalHeight = maxBounds.Height;
				return new Size(maxWidth, totalHeight);
			}
		}

		/// <summary>
		/// Called when the visibility of the conext menu is about to change.
		/// </summary>
		/// <param name="visible">Whether the context menu will be shown or not.</param>
		protected override void SetVisibleCore(bool visible) {
			if (Items.Count == 0 && visible)
				Close();
			else if (visible != Visible) {
				if (visible) {
					if (lastContextMenu != null) {
						lock (contextMenuSyncRoot) {
							if (lastContextMenu != null) {
								bool close = true;
								StyledItem styledItem;
								foreach (ToolStripItem item in lastContextMenu.Items) {
									styledItem = item as StyledItem;
									if (styledItem != null && styledItem.ContextMenu != null && styledItem.ContextMenu == this) {
										close = false;
										break;
									}
								}
								if (close) {
									lastContextMenu.Close();
									lastContextMenu = null;
								}
							}
						}
					}
				}
				if (visible && !StyledForm.DesignMode && AllowTransparency) {
					FadingIn = true;
					IntPtr handle = Handle;
					opacity = 1;
					NativeApi.SetLayeredWindowAttributes(Handle, COLORREF.Empty, 1, BlendFlags.ULW_ALPHA);
					UIAnimator.SharedAnimator.Animate(new FieldOrProperty(nameof(OpacityInner), this), (byte) 255, 0.35, 0.05, true, FadeInFinished, true);
				} else if (!(visible || StyledForm.DesignMode)) {
					EventListener.MouseDown -= Menu_MouseDown;
					EventListener.CursorMove -= Menu_CursorMove;
					EventListener.MouseUp -= Menu_MouseUp;
				}
				base.SetVisibleCore(visible);
				if (visible) {
					lock (contextMenuSyncRoot)
						lastContextMenu = this;
					if (!StyledForm.DesignMode) {
						constrained.SetValue(this, false, null);
						EventListener.MouseDown += Menu_MouseDown;
						EventListener.CursorMove += Menu_CursorMove;
						EventListener.MouseUp += Menu_MouseUp;
					}
				}
			}
		}

		private bool FadeInFinished(AnimationInfo state) {
			if (state.IsFinished) {
				FadingIn = false;
				return true;
			} else
				return !IsDisposed;
		}

		/// <summary>
		/// Updates the renderer by copying it to every item in the context menu.
		/// </summary>
		public void UpdateRenderer() {
			StyleRenderer renderer = Renderer as StyleRenderer;
			if (renderer == null) {
				renderer = new StyleRenderer(invalidate);
				Renderer = renderer;
			}
			lock (SyncRoot) {
				StyledItem item;
				for (int i = 0; i < Items.Count; i++) {
					item = Items[i] as StyledItem;
					if (item != null && item.Renderer != renderer) {
						item.Renderer.CopyConfigFrom(renderer);
						item.UpdateItemRenderer();
					}
				}
			}
		}

		/// <summary>
		/// Updates the item renderer on each item to the value of Renderer.
		/// </summary>
		protected override void OnRendererChanged(EventArgs e) {
			base.OnRendererChanged(e);
			UpdateRenderer();
		}

		/// <summary>
		/// Fired when an item is added to the context menu.
		/// </summary>
		protected override void OnItemAdded(ToolStripItemEventArgs e) {
			StyledItem item = e.Item as StyledItem;
			if (item == null) {
				StyledItem newItem = new StyledItem(e.Item, Renderer as StyleRenderer);
				lock (SyncRoot) {
					Items.Add(newItem);
					Items.Remove(e.Item);
				}
			} else {
				item.Parent = this;
				item.TextAlign = alignment;
				item.Renderer.CopyConfigFrom(Renderer as StyleRenderer);
				item.Renderer.FunctionToCallOnRefresh = invalidate;
				DisplayedItems.Add(item);
				FitToContent();
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
			DisplayedItems.Remove(item);
			base.OnItemRemoved(e);
			FitToContent();
		}

		/// <summary>
		/// Does nothing.
		/// </summary>
		/// <param name="se">Ignored.</param>
		protected override void OnScroll(ScrollEventArgs se) {
		}

		/// <summary>
		/// Fired when the forecolor is changed.
		/// </summary>
		protected override void OnForeColorChanged(EventArgs e) {
			base.OnForeColorChanged(e);
			lock (SyncRoot) {
				for (int i = 0; i < Items.Count; i++)
					Items[i].ForeColor = ForeColor;
			}
			Invalidate(false);
		}

		/// <summary>
		/// Fired when the font is changed.
		/// </summary>
		protected override void OnFontChanged(EventArgs e) {
			base.OnFontChanged(e);
			lock (SyncRoot) {
				for (int i = 0; i < Items.Count; i++)
					Items[i].Font = Font;
			}
			FitToContent();
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

		/// <summary>
		/// Called when the context menu is being opened.
		/// </summary>
		/// <param name="e">Allows you to stop the context menu from being opened.</param>
		protected override void OnOpening(CancelEventArgs e) {
			base.OnOpening(e);
			constrained.SetValue(this, false, null);
			lock (SyncRoot) {
				StyledItem item;
				for (int i = 0; i < Items.Count; i++) {
					item = Items[i] as StyledItem;
					if (item != null) {
						item.TransferItemsToContextMenu(item);
						item.Text = item.Text;
					}
				}
			}
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
			item.TextAlign = ContentAlignment.MiddleCenter;
			item.Renderer.FunctionToCallOnRefresh = invalidate;
			item.Click += onClick;
			return item;
		}

		/// <summary>
		/// Called when the mouse is pressed.
		/// </summary>
		/// <param name="mea">The mouse event parameters.</param>
		protected override void OnMouseDown(MouseEventArgs mea) {
		}

		/// <summary>
		/// Called when the mouse is moved.
		/// </summary>
		/// <param name="mea">The mouse event parameters.</param>
		protected override void OnMouseMove(MouseEventArgs mea) {
		}

		/// <summary>
		/// Called when the mouse is released.
		/// </summary>
		/// <param name="mea">The mouse event parameters.</param>
		protected override void OnMouseUp(MouseEventArgs mea) {
		}

		private void Menu_MouseDown(object sender, MouseEventArgs e) {
			Point client;
			if (StyledForm.DesignMode)
				client = e.Location;
			else
				client = PointToClient(e.Location);
			bool isOut = !Size.Contains(client);
			if (isOut) {
				if (sender != null && AutoClose && !IsPointInSubMenu(e.Location))
					Close();
			} else if (e.Button == MouseButtons.Left) {
				isMouseDown = true;
				wasPressedOn = GetItemAt(StyledForm.DesignMode ? e.Y : PointToClient(e.Location).Y);
				if (wasPressedOn != -1) {
					StyledItem item = Items[wasPressedOn] as StyledItem;
					if (item != null)
						item.Renderer.Pressed = true;
				}
			}
		}

		private bool IsPointInSubMenu(Point location) {
			Queue<StyledContextMenu> menus = new Queue<StyledContextMenu>();
			menus.Enqueue(this);
			StyledContextMenu contextMenu, temp;
			StyledItem item;
			do {
				contextMenu = menus.Dequeue();
				lock (SyncRoot) {
					for (int i = 0; i < contextMenu.Items.Count; i++) {
						item = contextMenu.Items[i] as StyledItem;
						if (item != null) {
							temp = item.ContextMenu;
							if (temp != null) {
								if (temp.Visible && temp.Size.Contains(temp.PointToClient(location)))
									return true;
								else
									menus.Enqueue(temp);
							}
						}
					}
				}
			} while (menus.Count != 0);
			return false;
		}

		private void Menu_CursorMove(object sender, MouseEventArgs e) {
			if (IsDisposed)
				return;
			StyledItem item;
			if (wasPressedOn == -1 && e.Button == MouseButtons.Left)
				Menu_MouseDown(null, e);
			else if (wasPressedOn != -1 && e.Button != MouseButtons.Left) {
				item = Items[wasPressedOn] as StyledItem;
				if (item != null)
					item.Renderer.Pressed = false;
			}
			Point client;
			if (StyledForm.DesignMode)
				client = e.Location;
			else
				client = PointToClient(e.Location);
			bool isOut = !Size.Contains(client);
			int itemIndex = isOut ? -1 : GetItemAt(client.Y);
			if (itemIndex == -1)
				isOut = true;
			if (isOut && wasPressedOn == -1 && !StyledForm.DesignMode) {
				IntPtr handle = NativeApi.WindowFromPoint(e.Location);
				if (handle != IntPtr.Zero && (!isMouseDown || (SourceControl != null && handle == SourceControl.Handle))) {
					Win32Rectangle rect;
					if (NativeApi.GetWindowRect(handle, out rect))
						NativeApi.SendNotifyMessage(handle, EventListener.LastEvent, new IntPtr(StyledForm.GetButtonStates()), new IntPtr(((e.Y - rect.Top) << 16) | ((e.X - rect.Left) & 0xFFFF)));
				}
			}
			if (wasHighlighted != itemIndex) {
				bool changeHighlight = true;
				if (wasHighlighted != -1) {
					item = Items[wasHighlighted] as StyledItem;
					if (item != null) {
						if (item.ContextMenu == null) {
							item.Renderer.MarkMouseHasLeft();
						} else if (!isOut) {
							item.Renderer.MarkMouseHasLeft();
							item.ContextMenu.Close();
						} else
							changeHighlight = false;
					}
				}
				if (!isOut) {
					item = Items[itemIndex] as StyledItem;
					if (item != null) {
						item.Renderer.Pressed = wasPressedOn != -1;
						item.Renderer.MouseHovering = true;
						if (item.Enabled && item.ContextMenu != null)
							item.ContextMenu.Show(this, Width, GetLocationOf(itemIndex));
					}
				}
				if (changeHighlight)
					wasHighlighted = itemIndex;
			}
		}

		private void Menu_MouseUp(object sender, MouseEventArgs e) {
			if (e.Button != MouseButtons.Left)
				return;
			isMouseDown = false;
			Point client = StyledForm.DesignMode ? e.Location : PointToClient(e.Location);
			StyledItem item;
			if (Size.Contains(client)) {
				int itemIndex = GetItemAt(client.Y);
				if (itemIndex == -1)
					ClearHighlighting(true);
				else {
					item = Items[itemIndex] as StyledItem;
					if (!(StyledForm.DesignMode || item == null)) {
						if (AutoClose && item.ContextMenu == null)
							CloseAll();
						if (item.Enabled) {
							item.PerformClick();
							if (item.ContextMenu != null)
								item.ContextMenu.Show(this, Width, GetLocationOf(itemIndex));
							if (parent != null && parent.IsComboBoxItem)
								parent.Text = item.Text;
						}
					}
					ClearHighlighting(!Visible);
				}
			} else
				ClearHighlighting(!IsPointInSubMenu(e.Location));
		}

		private void ClearHighlighting(bool clearHighlight) {
			StyledItem item;
			if (clearHighlight && wasHighlighted != -1) {
				item = Items[wasHighlighted] as StyledItem;
				if (item != null)
					item.Renderer.MouseHovering = false;
				wasHighlighted = -1;
			}
			if (wasPressedOn != -1) {
				item = Items[wasPressedOn] as StyledItem;
				if (item != null)
					item.Renderer.Pressed = false;
				wasPressedOn = -1;
			}
		}

		private void CloseAll() {
			StyledContextMenu parentContext = this;
			StyledItem parent;
			do {
				parentContext.Close();
				parent = parentContext.parent;
				if (parent == null)
					break;
				parentContext = parent.parent as StyledContextMenu;
			} while (parentContext != null);
		}

		/// <summary>
		/// Gets the item at the specified y coordinate in the context menu.
		/// </summary>
		/// <param name="y">The y coordinate.</param>
		public int GetItemAt(int y) {
			int count = Items.Count;
			if (y < 0 || y > ClientSize.Height || count == 0)
				return -1;
			else
				return Math.Min(y / maxItemHeight, count - 1);
		}

		/// <summary>
		/// Gets the Y-coordinate of the item at the specified index. Returns -1 if the index is invalid.
		/// </summary>
		/// <param name="index">The index of the item to get the Y-coordinate of.</param>
		public int GetLocationOf(int index) {
			if (index < 0 || index >= Items.Count)
				return -1;
			else
				return index * maxItemHeight;
		}

		/// <summary>
		/// Processes the specified keys.
		/// </summary>
		/// <param name="m">The associated message.</param>
		/// <param name="keyData">The keys that were pressed.</param>
		protected override bool ProcessCmdKey(ref Message m, Keys keyData) {
			if (keyData == Keys.Escape)
				CloseAll();
			return true;
		}

		/// <summary>
		/// Called when the the layout of the context menu is calculated.
		/// </summary>
		/// <param name="levent">Ignored.</param>
		protected override void OnLayout(LayoutEventArgs levent) {
		}

		/// <summary>
		/// Not implemented.
		/// </summary>
		protected override void OnPaintBackground(PaintEventArgs e) {
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
		/// Draws the context menu onto the specified graphics canvas
		/// </summary>
		/// <param name="g">The canvas to draw the context menu on</param>
		public void DrawGdi(Graphics g) {
			DrawGdi(g, Location, ClientRectangle, true);
		}

		/// <summary>
		/// Draws the context menu onto the specified graphics canvas at the specified location
		/// </summary>
		/// <param name="g">The canvas to draw the context menu on</param>
		/// <param name="location">The location to draw the context menu at</param>
		/// <param name="drawChildren">Whether to draw the child controls</param>
		public void DrawGdi(Graphics g, Point location, bool drawChildren = true) {
			DrawGdi(g, location, ClientRectangle, drawChildren);
		}

		/// <summary>
		/// Draws the context menu onto the specified graphics canvas at the specified location.
		/// </summary>
		/// <param name="g">The canvas to draw the context menu on.</param>
		/// <param name="location">The location to draw the context menu at.</param>
		/// <param name="invalidated">The client rectangle that was invalidated.</param>
		/// <param name="drawChildren">Whether to draw the child controls</param>
		protected virtual void DrawGdi(Graphics g, Point location, Rectangle invalidated, bool drawChildren) {
			int startItem = invalidated.Y / maxItemHeight;
			int lastItem = Math.Min(Items.Count - 1, invalidated.Bottom / maxItemHeight);
			location.Y += startItem * maxItemHeight;
			if (!location.IsEmpty)
				g.TranslateTransform(location.X, location.Y);
			Region oldClipRegion = g.Clip;
			Image backgroundImage = BackgroundImage;
			if (backgroundImage != null) {
				try {
					ImageLib.DrawImageWithLayout(g, backgroundImage, ClientRectangle, BackgroundImageLayout);
				} catch {
				}
			}
			g.SetClip(invalidated);
			PaintEventArgs args = new PaintEventArgs(g, invalidated);
			RaisePaintEvent(PaintBackground, this, args);
			StyledItem item;
			Size itemSize = new Size(Width, maxItemHeight);
			lock (SyncRoot) {
				for (int i = startItem; i <= lastItem; i++) {
					item = Items[i] as StyledItem;
					if (item != null) {
						item.DrawGdi(g, itemSize);
						g.TranslateTransform(0f, maxItemHeight);
					}
				}
			}
			int cumulative = (lastItem + 1 - startItem) * maxItemHeight;
			if (cumulative != 0)
				g.TranslateTransform(0f, -cumulative);
			g.SetClip(invalidated);
			RaisePaintEvent(StyleRenderer.PaintEventKey, args);
			if (drawChildren)
				g.DrawControls(Controls, Point.Empty, invalidated, true);
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
		/// Draws the context menu and its items
		/// </summary>
		/// <param name="e">The graphics canvas to draw on</param>
		protected override void OnPaint(PaintEventArgs e) {
			DrawGdi(e.Graphics, Point.Empty, e.ClipRectangle, false);
		}

		/// <summary>
		/// Called when the context menu is closing
		/// </summary>
		/// <param name="e">The close reason</param>
		protected override void OnClosing(ToolStripDropDownClosingEventArgs e) {
			base.OnClosing(e);
			FadingIn = false;
			EventListener.MouseDown -= Menu_MouseDown;
			EventListener.CursorMove -= Menu_CursorMove;
			EventListener.MouseUp -= Menu_MouseUp;
			StyledItem item;
			if (wasPressedOn != -1) {
				item = Items[wasPressedOn] as StyledItem;
				if (item != null)
					item.Renderer.Pressed = false;
				wasPressedOn = -1;
			}
			if (wasHighlighted != -1) {
				item = Items[wasHighlighted] as StyledItem;
				if (item != null)
					item.Renderer.MouseHovering = false;
				wasHighlighted = -1;
			}
			StyledMenuStrip strip = SourceControl as StyledMenuStrip;
			if (strip != null)
				strip.ContextMenuClosed();
			StyledContextMenu temp;
			lock (SyncRoot) {
				for (int i = 0; i < Items.Count; i++) {
					item = Items[i] as StyledItem;
					if (item != null) {
						temp = item.ContextMenu;
						if (temp != null && temp.Visible)
							temp.Close();
					}
				}
			}
		}

		/// <summary>
		/// Gets a string that represents this instance.
		/// </summary>
		public override string ToString() {
			StringBuilder builder = new StringBuilder();
			builder.Append(nameof(StyledContextMenu));
			builder.Append(": ");
			builder.Append(Name);
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
	}
}