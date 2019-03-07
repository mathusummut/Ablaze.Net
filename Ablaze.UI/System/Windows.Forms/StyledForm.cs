using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Platforms.Windows;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Dispatch;

namespace System.Windows.Forms {
	/// <summary>
	/// A styled window that is packed with features and is extremely customizable.
	/// </summary>
	[Description("A styled window that is packed with features and is extremely customizable.")]
	[DisplayName(nameof(StyledForm))]
	[DefaultEvent(nameof(Load))]
	public class StyledForm : Form, IDrawable {
		private static bool isInit;
		/// <summary>
		/// KEEP THIS ON TOP! (so that it is executed before other functions)
		/// </summary>
		private readonly bool initDummy = InitApp();
		private static SyncedList<StyledForm> openForms = new SyncedList<StyledForm>();
		/// <summary>
		/// Gets whether the form is currently running in design mode.
		/// </summary>
		public static new readonly bool DesignMode = LicenseManager.UsageMode == LicenseUsageMode.Designtime;
		private static object EventEnabled = typeof(Control).GetField(nameof(EventEnabled), BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
		private static object EventHandleCreated = typeof(Control).GetField(nameof(EventHandleCreated), BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
		private static object EventHandleDestroyed = typeof(Control).GetField(nameof(EventHandleDestroyed), BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
		private static object EventLoad = typeof(Form).GetField("EVENT_LOAD", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
		private static int CursorKey = (int) typeof(Control).GetField("PropCursor", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
		private static MethodInfo SetProperty = typeof(Control).Assembly.GetType("System.Windows.Forms.PropertyStore").GetMethod("SetObject", BindingFlags.Public | BindingFlags.Instance);
		private static int MouseEnterMsg = NativeApi.RegisterWindowMessage("WinFormsMouseEnter");
		internal static Action<Form, bool> SetCalledClosing = (Action<Form, bool>) Delegate.CreateDelegate(typeof(Action<Form, bool>),
			typeof(Form).GetProperty("CalledClosing", BindingFlags.Instance | BindingFlags.NonPublic).GetSetMethod(true));
		internal static Action<Form, bool> SetCalledCreateControl = (Action<Form, bool>) Delegate.CreateDelegate(typeof(Action<Form, bool>),
			typeof(Form).GetProperty("CalledCreateControl", BindingFlags.Instance | BindingFlags.NonPublic).GetSetMethod(true));
		internal static Action<Form, bool> SetCalledMakeVisible = (Action<Form, bool>) Delegate.CreateDelegate(typeof(Action<Form, bool>),
			typeof(Form).GetProperty("CalledMakeVisible", BindingFlags.Instance | BindingFlags.NonPublic).GetSetMethod(true));
		internal static Action<Form, bool> SetCalledOnLoad = (Action<Form, bool>) Delegate.CreateDelegate(typeof(Action<Form, bool>),
			typeof(Form).GetProperty("CalledOnLoad", BindingFlags.Instance | BindingFlags.NonPublic).GetSetMethod(true));
		private static WindowMessage InvokeMessage = (WindowMessage) NativeApi.RegisterWindowMessage("InvokeThreadCallbackMessage");
		/// <summary>
		/// A list of all styled forms that are currently open.
		/// </summary>
		public static readonly ReadOnlySyncedList<StyledForm> OpenStyledForms = openForms.AsReadOnly();
		private static bool useSetWindowComposition = Extensions.IsAeroEnabled && NativeApi.GetProcAddress(NativeApi.LoadLibrary("user32.dll"), "SetWindowCompositionAttribute") != IntPtr.Zero;
		private static Type TypeOfForm = typeof(Form);
		private static Action<Control> RecreateControlHandle = (Action<Control>) Delegate.CreateDelegate(typeof(Action<Control>), typeof(Control).GetMethod(nameof(RecreateHandle), BindingFlags.Instance | BindingFlags.NonPublic));
		private static Action<Control, MouseEventArgs> MouseWheelInvoke = (Action<Control, MouseEventArgs>) Delegate.CreateDelegate(typeof(Action<Control, MouseEventArgs>), typeof(Control).GetMethod(nameof(OnMouseWheel), BindingFlags.Instance | BindingFlags.NonPublic));
		private static Action<Control, MouseEventArgs> MouseUpInvoke = (Action<Control, MouseEventArgs>) Delegate.CreateDelegate(typeof(Action<Control, MouseEventArgs>), typeof(Control).GetMethod(nameof(OnMouseUp), BindingFlags.Instance | BindingFlags.NonPublic));
		private static Action<Control, EventArgs> ParentEnabledInvoke = (Action<Control, EventArgs>) Delegate.CreateDelegate(typeof(Action<Control, EventArgs>), typeof(Control).GetMethod(nameof(OnParentEnabledChanged), BindingFlags.Instance | BindingFlags.NonPublic));
		private static FieldInfo properties = typeof(Control).GetField("propertyStore", BindingFlags.Instance | BindingFlags.NonPublic);
		private ConcurrentQueue<InvocationData> callbacks = new ConcurrentQueue<InvocationData>();
		private event MouseEventHandler physicalMouseMove;
		private Action updateLayered;
		/// <summary>
		/// The renderer that handles the styling of the close button.
		/// </summary>
		private StyleRenderer closeButtonRenderer;
		/// <summary>
		/// The renderer that handles the styling of the maximize button.
		/// </summary>
		private StyleRenderer maximizeButtonRenderer;
		/// <summary>
		/// The renderer that handles the styling of the minimize button.
		/// </summary>
		private StyleRenderer minimizeButtonRenderer;
		private FieldOrProperty OpacityProperty, BoundsProperty, TopInnerProperty, BorderOpacityProperty, BorderOverlayProperty;
		/// <summary>
		/// The thread on which the control resides.
		/// </summary>
		private Thread residentThread;
		/// <summary>
		/// The locking object that is used to synchronize accesses to the border texture.
		/// </summary>
		public readonly object BorderSyncLock = new object();
		private static object GlobalBorderLock = new object();
		private static IntPtr One = new IntPtr(1);
		private object DisposeSyncRoot = new object(), oldControlSyncRoot = new object();
		/// <summary>
		/// Gets or sets the default active border texture.
		/// </summary>
		private static Bitmap defaultBorder;
		private Screen screen;
		private Brush MinimizeFill;
		private Pen XColor, MaxSymbolColor, outlineColor, inlineColor;
		private StyledLabel titleLabel;
		private UIAnimationHandler onMinimizeUpdate, onOpacityUpdate, onBoundsUpdate, onBorderUpdate;
		private MouseEventHandler physicalMouseMoveEvent, globalCursorMove, globalMouseUp;
		private Func<object, object> callOnResizeEnd, setVisibleCore;
		private Dictionary<Control, Size> OffsetTracker = new Dictionary<Control, Size>();
		private List<Control> oldControls = new List<Control>();
		private object offsetLock = new object();
		private Size SizeOffset, titleBarPadding = new Size(0, 1);
		private FormBorderStyle WindowBorder;
		private FormWindowState oldWindowState;
		private DisposeOptions disposed;
		private DialogResult dialogResult = DialogResult.Cancel;
		private Rectangle oldBounds, beforeResize;
		private Point mouseStartingPoint;
		private Cursor defaultCursor = Cursors.Default, borderCursor = Cursors.Default;
		private Bitmap border;
		private Color borderOverlay = Color.FromArgb(100, 0, 0, 0), currentBorderOverlay = Color.Empty;
		private Control lastDoubleClickControl, CaptureControl, MouseInsideControl /*, hasFocus*/;
		private Size closeSize, maximizeButtonSize, minimumSize, maximumSize;
		private float inactiveBorderOpacity = 0.5f, activeBorderOpacity = 0.75f, currentBorderOpacity = 0.5f;
		private byte opacity = 255, targetOpacity = 255, backcolorOpacity = 255;
		private int borderChanged, topBeforeMinimize, topInner, originalTopInner, borderWidth = 4, titleBarHeight = 29;
		private bool isFullScreen, wasMaximized, showBorder, wasSystemMenuShown, fullscreenGdiGLWorkaround, minimizing, wereBordersShown,
			isMaximized, isBeingMoved, isBeingResizedTop, isBeingResizedTopLeft, isBeingResizedTopRight, isBeingResizedLeft, mouseInBorder,
			isBeingResizedRight, isBeingResizedBottom, isBeingResizedBottomLeft, isBeingResizedBottomRight, hasMoved, restoring, animatingBounds,
			wasInsideMinimize, wasInsideMaximize, wasInsideClose, mouseDownOnBorder, animatingTopInner, borderActive, finalized, wasTopMost,
			gdiVisible, suppressLocation, wasShadowEnabled, inTaskbar = true, minimizeEnabled = true, maximizeEnabled = true, closeEnabled = true,
			cursorVisible = true, firstLoad = true, isResizable = true, showShadow = true, allowDrop = true, enableAeroBlur = true;
		private ImageLayout backgroundLayout = ImageLayout.Stretch;
		private const ChildSkip EligibleForMouse = ChildSkip.Disabled | ChildSkip.Invisible | ChildSkip.Transparent | ChildSkip.DontRespondToMouse;

		/// <summary>
		/// Fired when the mouse has physically been moved irrespective of cursor movement and display bounds.
		/// 'e' will hold the unbounded cursor position in viewport coordinates.
		/// </summary>
		public event MouseEventHandler PhysicalMouseMove {
			add {
				if (value == null)
					return;
				physicalMouseMove -= value;
				lock (MouseListener.SyncLock) {
					if (physicalMouseMove == null) {
						physicalMouseMove = value;
						MouseListener.MouseMove += physicalMouseMoveEvent;
					} else
						physicalMouseMove += value;
				}
			}
			remove {
				if (value == null)
					return;
				lock (MouseListener.SyncLock) {
					physicalMouseMove -= value;
					if (physicalMouseMove == null)
						MouseListener.MouseMove -= physicalMouseMoveEvent;
				}
			}
		}

		/// <summary>
		/// Gets whether the control supports OpenGL rendering.
		/// </summary>
		[Browsable(false)]
		public virtual bool SupportsGL {
			get {
				return false;
			}
		}

		/// <summary>
		/// Gets or sets the minimum size of the form. 0 means unlimited
		/// </summary>
		[Description("Gets or sets the minimum size of the form. 0 means unlimited")]
		public override Size MinimumSize {
			get {
				return minimumSize;
			}
			set {
				minimumSize = value;
				Rectangle bounds = Bounds;
				SetBoundsCore(bounds.X, bounds.Y, bounds.Width, bounds.Height, BoundsSpecified.Size);
			}
		}

		/// <summary>
		/// Gets or sets the maximum size of the form. 0 means unlimited
		/// </summary>
		[Description("Gets or sets the maximum size of the form. 0 means unlimited")]
		public override Size MaximumSize {
			get {
				return maximumSize;
			}
			set {
				maximumSize = value;
				Rectangle bounds = Bounds;
				SetBoundsCore(bounds.X, bounds.Y, bounds.Width, bounds.Height, BoundsSpecified.Size);
			}
		}

		/// <summary>
		/// Gets or sets the background color opacity. Only effective when aero blur is enabled.
		/// </summary>
		[Description("Gets or sets the background color opacity. Only effective when aero blur is enabled.")]
		[DefaultValue(255)]
		public byte BackColorOpacity {
			get {
				return backcolorOpacity;
			}
			set {
				if (value == backcolorOpacity)
					return;
				backcolorOpacity = value;
				if (EnableAeroBlur && Extensions.IsAeroEnabled && !AnimatingBounds && !IsGLEnabled)
					Invalidate(false);
			}
		}

		/// <summary>
		/// If true, the window background is not cleared before it is redrawn (not recommended). Use only if you plan to redraw the whole window
		/// yourself at a high refresh rate.
		/// </summary>
		[Browsable(false)]
		[DefaultValue(false)]
		public bool SuppressClear {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets whether to enable or disable the aero blur effect.
		/// </summary>
		[Description("Gets or sets whether to enable or disable the aero blur effect.")]
		[DefaultValue(true)]
		public bool EnableAeroBlur {
			get {
				return enableAeroBlur;
			}
			set {
				if (enableAeroBlur == value)
					return;
				enableAeroBlur = value;
				UpdateAeroBlur();
			}
		}

		/// <summary>
		/// Gets the current border opacity of the form
		/// </summary>
		[Browsable(false)]
		public float CurrentBorderOpacity {
			get {
				return currentBorderOpacity;
			}
		}

		/// <summary>
		/// Gets or sets the default border cursor.
		/// </summary>
		[Description("Gets or sets the default border cursor.")]
		public Cursor BorderCursor {
			get {
				return borderCursor;
			}
			set {
				if (value == null)
					value = Cursors.Default;
				if (value != borderCursor)
					borderCursor = value;
			}
		}

		/// <summary>
		/// Gets the thread on which the control resides.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Thread ResidentThread {
			get {
				return residentThread;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the caller should call an invoke method when making method calls to the control because the caller is on a different thread than the one the control was created on.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool InvokeRequired {
			get {
				return !(IsHandleCreated && Thread.CurrentThread == residentThread);
			}
		}

		/// <summary>
		/// Gets the current form DPI scale multiplier.
		/// </summary>
		[Browsable(false)]
		public SizeF DpiScale {
			get;
			private set;
		}

		/// <summary>
		/// If true, Dispose(DisposeOptions.FullDisposal) is called automatically after the form is closed.
		/// Set to true for main window forms that are present throughout the application lifetime.
		/// </summary>
		[Description("If true, Dispose(DisposeOptions.FullDisposal) is called automatically after the form is closed.")]
		[DefaultValue(false)]
		public bool FullDisposeOnClose {
			get;
			set;
		}

		/// <summary>
		/// Gets the screen that contains the form.
		/// </summary>
		[Browsable(false)]
		public Screen ParentScreen {
			get {
				return screen;
			}
		}

		/// <summary>
		/// Gets or sets the padding to add around title bar borders (affects control buttons and icon bounds).
		/// </summary>
		[Description("Gets or sets the padding to add around title bar borders (affects control buttons and icon bounds).")]
		[DefaultValue(typeof(Size), "0, 2")]
		public Size TitleBarBadding {
			get {
				return titleBarPadding;
			}
			set {
				if (titleBarPadding == value)
					return;
				titleBarPadding = value;
				TitleBarChanged(0, 0);
			}
		}

		/// <summary>
		/// Gets or sets the border opacity when the window is active.
		/// </summary>
		public float ActiveBorderOpacity {
			get {
				return activeBorderOpacity;
			}
			set {
				if (activeBorderOpacity == value)
					return;
				activeBorderOpacity = value;
				if (borderActive)
					UIAnimator.SharedAnimator.Animate(BorderOpacityProperty, value, 0.3, 0.01, true, onBorderUpdate, false);
			}
		}

		/// <summary>
		/// Gets or sets the border opacity when the window is inactive.
		/// </summary>
		public float InactiveBorderOpacity {
			get {
				return inactiveBorderOpacity;
			}
			set {
				if (inactiveBorderOpacity == value)
					return;
				inactiveBorderOpacity = value;
				if (!borderActive)
					UIAnimator.SharedAnimator.Animate(BorderOpacityProperty, value, 0.3, 0.01, true, onBorderUpdate, false);
			}
		}

		/// <summary>
		/// Gets or sets whether the shadow should be shown.
		/// </summary>
		[Description("Gets or sets whether the shadow should be shown.")]
		[Browsable(true)]
		public bool ShowShadow {
			get {
				return showShadow;
			}
			set {
				showShadow = value;
				if (Visible)
					UpdateShadow();
			}
		}

		/// <summary>
		/// Gets whether the border is currently visible.
		/// </summary>
		[Browsable(false)]
		public bool IsBorderVisible {
			get {
				return showBorder && !isFullScreen;
			}
		}

		/// <summary>
		/// If assigned a menu strip, it will be shown instead of the standard system menu when the taskbar is right-clicked.
		/// </summary>
		[Description("If assigned a menu strip, it will be shown instead of the standard system menu when the taskbar is right-clicked.")]
		public ContextMenuStrip SystemMenuStrip {
			get;
			set;
		}

		/// <summary>
		/// If assigned a menu strip, it will be shown instead of the standard system menu.
		/// </summary>
		[Description("If assigned a menu strip, it will be shown instead of the standard system menu when the taskbar is right-clicked.")]
		public ContextMenu SystemMenu {
			get;
			set;
		}

		/// <summary>
		/// Gets the renderer that handles the styling of the close button.
		/// </summary>
		[Browsable(false)]
		public StyleRenderer CloseButtonRenderer {
			get {
				return closeButtonRenderer;
			}
		}

		/// <summary>
		/// Gets the renderer that handles the styling of the maximize button.
		/// </summary>
		[Browsable(false)]
		public StyleRenderer MaximizeButtonRenderer {
			get {
				return maximizeButtonRenderer;
			}
		}

		/// <summary>
		/// Gets the renderer that handles the styling of the minimize button.
		/// </summary>
		[Browsable(false)]
		public StyleRenderer MinimizeButtonRenderer {
			get {
				return minimizeButtonRenderer;
			}
		}

		/// <summary>
		/// Gets or sets the default active border texture (or null to load default).
		/// </summary>
		public static Bitmap DefaultBorder {
			get {
				if (defaultBorder == null) {
					lock (GlobalBorderLock) {
						using (Bitmap border = Properties.Resources.Border)
							defaultBorder = ImageLib.ConvertPixelFormat(border, Drawing.Imaging.PixelFormat.Format32bppPArgb);
					}
				}
				return defaultBorder;
			}
			set {
				defaultBorder = value;
			}
		}

		/// <summary>
		/// Gets the current bounds of the close button (empty if the close button is not enabled).
		/// </summary>
		[Browsable(false)]
		public Rectangle CloseBounds {
			get {
				if (closeEnabled) {
					return new Rectangle(new Point(((isMaximized && (animatingTopInner || IsFullyMaximized)) ? ClientSize.Width - 1 : ClientSize.Width - (int) ((borderWidth + titleBarPadding.Width) * DpiScale.Height)) - closeSize.Width,
						(isMaximized && (animatingTopInner || IsFullyMaximized)) ? 0 : (int) ((borderWidth / 2 + titleBarPadding.Height) * DpiScale.Height)), closeSize);
				} else
					return Rectangle.Empty;
			}
		}

		/// <summary>
		/// Gets the current bounds of the maximize button (empty if the maximize button is not enabled).
		/// </summary>
		[Browsable(false)]
		public Rectangle MaximizeBounds {
			get {
				if (maximizeEnabled) {
					Rectangle bounds = new Rectangle(new Point((isMaximized && (animatingTopInner || IsFullyMaximized)) ? ClientSize.Width - 1 : ClientSize.Width - (int) ((borderWidth + titleBarPadding.Width) * DpiScale.Height),
						(isMaximized && (animatingTopInner || IsFullyMaximized)) ? 0 : (int) ((borderWidth / 2 + titleBarPadding.Height) * DpiScale.Height)), maximizeButtonSize);
					if (closeEnabled)
						bounds.X -= closeSize.Width;
					bounds.X -= maximizeButtonSize.Width;
					return bounds;
				} else
					return Rectangle.Empty;
			}
		}

		/// <summary>
		/// Gets the current bounds of the minimize button (empty if the minimize button is not enabled).
		/// </summary>
		[Browsable(false)]
		public Rectangle MinimizeBounds {
			get {
				if (MinimizeEnabled) {
					Rectangle bounds = new Rectangle(new Point((isMaximized && (animatingTopInner || IsFullyMaximized)) ? ClientSize.Width - 1 : ClientSize.Width - (int) ((borderWidth + titleBarPadding.Width) * DpiScale.Height),
						(isMaximized && (animatingTopInner || IsFullyMaximized)) ? 0 : (int) ((borderWidth / 2 + titleBarPadding.Height) * DpiScale.Height)), maximizeButtonSize);
					if (closeEnabled)
						bounds.X -= closeSize.Width;
					if (maximizeEnabled)
						bounds.X -= maximizeButtonSize.Width;
					bounds.X -= maximizeButtonSize.Width;
					return bounds;
				} else
					return Rectangle.Empty;
			}
		}

		/// <summary>
		/// Gets the current bounds of the control box (min, max and close combined).
		/// </summary>
		[Browsable(false)]
		public Rectangle ControlBoxBounds {
			get {
				Rectangle bounds = new Rectangle((isMaximized && (animatingTopInner || IsFullyMaximized)) ? ClientSize.Width - 1 : ClientSize.Width - (int) ((borderWidth + titleBarPadding.Width) * DpiScale.Height),
					(isMaximized && (animatingTopInner || IsFullyMaximized)) ? 0 : (int) ((borderWidth / 2 + titleBarPadding.Height) * DpiScale.Height), 0, closeSize.Height);
				if (closeEnabled) {
					bounds.X -= closeSize.Width;
					bounds.Width += closeSize.Width;
				}
				if (maximizeEnabled) {
					bounds.X -= maximizeButtonSize.Width;
					bounds.Width += maximizeButtonSize.Width;
				}
				if (MinimizeEnabled) {
					bounds.X -= maximizeButtonSize.Width;
					bounds.Width += maximizeButtonSize.Width;
				}
				return bounds;
			}
		}

		/// <summary>
		/// Gets or sets whether to enable fullscreen toggle on Alt+Enter.
		/// </summary>
		[Description("Gets or sets whether to enable fullscreen toggle on Alt+Enter.")]
		public bool EnableFullscreenOnAltEnter {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets whether the cursor is visible on the form.
		/// </summary>
		[Description("Gets or sets whether the cursor is visible on the form.")]
		[DefaultValue(true)]
		public bool CursorVisible {
			get {
				return cursorVisible;
			}
			set {
				if (value == cursorVisible)
					return;
				cursorVisible = value;
				if (value)
					Cursor.Show();
				else
					Cursor.Hide();
			}
		}

		/// <summary>
		/// Gets or sets whether to show the window icon on the taskbar.
		/// </summary>
		[Description("Gets or sets whether to show the window icon on the taskbar.")]
		[DefaultValue(true)]
		public new bool ShowInTaskbar {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				if (DesignMode)
					return inTaskbar;
				else
					return base.ShowInTaskbar;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				inTaskbar = value;
				if (!DesignMode && value != base.ShowInTaskbar) {
					if (IsHandleCreated)
						Invoke(new InvocationData(SetShowInTaskbarCore, value));
					else
						base.ShowInTaskbar = value;
				}
			}
		}

		/// <summary>
		/// Gets the title label.
		/// </summary>
		[Description("Gets the title label.")]
		public StyledLabel TitleLabel {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return titleLabel;
			}
		}

		internal int TopInner {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return topInner;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				topInner = value;
				try {
					Top = value;
				} catch {
					UIAnimator.SharedAnimator.Halt(TopInnerProperty, true, true);
				}
			}
		}

		/// <summary>
		/// Gets or sets the opacity of the form.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new byte Opacity {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return opacity;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				opacity = value;
				if (!DesignMode) {
					NativeApi.SetLayeredWindowAttributes(Handle, COLORREF.Empty, value, BlendFlags.ULW_ALPHA);
					/*DropShadowForm shadow = Shadow;
					if (shadow != null)
						shadow.Opacity = value;*/
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
				if (!(AnimatingBounds || IsGLEnabled || GetStyle(ControlStyles.Opaque)))
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
				if (!(BackgroundImage == null || AnimatingBounds || IsGLEnabled || GetStyle(ControlStyles.Opaque)))
					Invalidate(false);
			}
		}

		private Rectangle WorkingArea {
			get {
				Form parent = MdiParent;
				return parent == null ? screen.WorkingArea : parent.ClientRectangle;
			}
		}

		private bool IsFullyMaximized {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				Rectangle bounds = Bounds;
				Rectangle workingArea = WorkingArea;
				return Math.Abs(bounds.X - workingArea.X) <= 1 && Math.Abs(bounds.Y - workingArea.Y) <= 1 &&
					Math.Abs(bounds.Width - workingArea.Width) <= 1 && Math.Abs(bounds.Width - workingArea.Width) <= 1;
			}
		}

		/// <summary>
		/// Gets whether the window is disposed.
		/// </summary>
		[Browsable(false)]
		public new bool IsDisposed {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return disposed != DisposeOptions.None;
			}
		}

		/// <summary>
		/// Gets or sets whether to enable the fade in/out animation.
		/// </summary>
		[Description("Gets or sets whether to enable the fade in/out animation.")]
		[DefaultValue(true)]
		public bool EnableOpacityAnimation {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets whether to enable the minimize/restore animation.
		/// </summary>
		[Description("Gets or sets whether to enable the minimize/restore animation.")]
		[DefaultValue(true)]
		public bool EnableMinimizeAnimation {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets whether to enable the border active/inactive animation.
		/// </summary>
		[Description("Gets or sets whether to enable the border active/inactive animation.")]
		[DefaultValue(true)]
		public bool EnableBorderAnimation {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets whether to enable the maximize/fullscreen animation.
		/// </summary>
		[Description("Gets or sets whether to enable the maximize/fullscreen animation.")]
		[DefaultValue(true)]
		public bool EnableResizeAnimation {
			get;
			set;
		}

		/// <summary>
		/// Gets whether the bounds are currently being animated.
		/// </summary>
		[Browsable(false)]
		public bool AnimatingBounds {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return animatingBounds || animatingTopInner;
			}
		}

		/// <summary>
		/// Gets whether the minimize animation is currently in action.
		/// </summary>
		[Browsable(false)]
		public bool IsMinimizing {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return minimizing;
			}
		}

		/// <summary>
		/// Gets whether the restore from minimize animation is currently in action.
		/// </summary>
		[Browsable(false)]
		public bool IsRestoring {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return restoring;
			}
		}

		/// <summary>
		/// Gets the bounds of the icon area.
		/// </summary>
		[Browsable(false)]
		public Rectangle IconBounds {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				int size = (int) ((titleBarHeight - (2 + titleBarPadding.Height * 2)) * DpiScale.Height);
				return new Rectangle((isFullScreen || !showBorder || (isMaximized && (animatingTopInner || IsFullyMaximized))) ? 2 : (int) (borderWidth * DpiScale.Width) + 1, (int) (titleBarPadding.Height * DpiScale.Height) + 1, Icon == null ? 0 : size, size);
			}
		}

		/// <summary>
		/// Gets whether the OpenGL context is currently enabled.
		/// </summary>
		[Browsable(false)]
		public virtual bool IsGLEnabled {
			get {
				return false;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the control can accept data that the user drags onto it.
		/// </summary>
		public override bool AllowDrop {
			get {
				return allowDrop;
			}
			set {
				allowDrop = value;
				if (Platform.IsWindowsXPOrNewer)
					NativeApi.DragAcceptFiles(Handle, value);
			}
		}

		/// <summary>
		/// Gets the parameters that define the initial window style.
		/// </summary>
		protected override CreateParams CreateParams {
			get {
				CreateParams cp = base.CreateParams;
				if (DesignMode)
					return cp;
				cp.Caption = Text;
				cp.X = Left;
				cp.Y = Top;
				cp.Width = Width;
				cp.Height = Height;
				cp.ClassStyle = (int) (ClassStyle.DoubleClicks);
				cp.Style = unchecked((int) (WindowStyle.Popup | WindowStyle.SystemMenu | WindowStyle.ClipChildren | WindowStyle.ClipSiblings));
				cp.ExStyle = (int) (ExtendedWindowStyle.ApplicationWindow | ExtendedWindowStyle.ControlParent);
				if (!DesignMode)
					cp.ExStyle = (int) (ExtendedWindowStyle.Layered | ExtendedWindowStyle.AcceptFiles);
				return cp;
			}
		}

		/// <summary>
		/// Gets or sets the cursor icon that is used by default for the window.
		/// </summary>
		[Description("Gets or sets the cursor icon that is used by default for the window.")]
		public Cursor WindowCursor {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return DefaultCursor;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				if (value == null)
					value = Cursors.Default;
				if (Cursor == defaultCursor)
					Cursor = value;
				defaultCursor = value;
			}
		}

		/// <summary>
		/// Gets the cursor that is currently set as default (use WindowCursor instead).
		/// </summary>
		protected override sealed Cursor DefaultCursor {
			get {
				return defaultCursor;
			}
		}

		/// <summary>
		/// Gets or sets the cursor of the form.
		/// </summary>
		[Description("Gets or sets the cursor of the form.")]
		public override Cursor Cursor {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return base.Cursor;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				if (base.Cursor != value)
					base.Cursor = value;
			}
		}

		/// <summary>
		/// Gets or sets whether the window is resizable.
		/// </summary>
		[DefaultValue(true)]
		[Description("Gets or sets whether the window is resizable.")]
		public bool IsResizable {
			get {
				return isResizable;
			}
			set {
				isResizable = value;
			}
		}

		/// <summary>
		/// Gets whether the window is maximized.
		/// </summary>
		[Browsable(false)]
		public bool IsMaximized {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return isMaximized;
			}
		}

		/// <summary>
		/// Gets whether a fade animation is currently taking place.
		/// </summary>
		[Browsable(false)]
		public FadeState FadeState {
			get;
			private set;
		}

		/// <summary>
		/// Gets whether the window is currently minimized.
		/// </summary>
		[Browsable(false)]
		public bool IsMinimized {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return minimizing || WindowState == FormWindowState.Minimized;
			}
		}

		/// <summary>
		/// Gets or sets the border texture of the window. Be sure to use lock(BorderSyncLock) before using the bitmap or making any modifications to it.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Bitmap Border {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				lock (BorderSyncLock)
					return border;
			}
			set {
				lock (GlobalBorderLock) {
					lock (BorderSyncLock) {
						if (value == null)
							value = DefaultBorder;
						if (value == border)
							return;
						border = ImageLib.ConvertPixelFormat(value, Drawing.Imaging.PixelFormat.Format32bppPArgb);
					}
				}
				RedrawBorder(false);
				OnBorderTextureChanged();
			}
		}

		/// <summary>
		/// Gets whether the window is currently active.
		/// </summary>
		[Browsable(false)]
		public bool Active {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return Enabled && ActiveForm == this;
			}
		}

		/// <summary>
		/// Gets or sets the color of the window outline.
		/// </summary>
		[Description("Gets or sets the color of the window outline.")]
		public Color OutlineColor {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return outlineColor.Color;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				outlineColor.Color = value;
			}
		}

		/// <summary>
		/// Gets or sets the color of the window client area outline.
		/// </summary>
		[Description("Gets or sets the color of the client area outline.")]
		public Color InlineColor {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return inlineColor.Color;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				inlineColor.Color = value;
			}
		}

		/// <summary>
		/// Gets or sets whether the close button is enabled.
		/// </summary>
		[Description("Gets or sets whether the close button is enabled.")]
		[DefaultValue(true)]
		public bool CloseEnabled {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return closeEnabled;
			}
			set {
				if (value == closeEnabled)
					return;
				closeEnabled = value;
				RefreshBorder();
			}
		}

		/// <summary>
		/// Gets or sets whether the maximize button is enabled.
		/// </summary>
		[Description("Gets or sets whether the maximize button is enabled.")]
		[DefaultValue(true)]
		public bool MaximizeEnabled {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return maximizeEnabled;
			}
			set {
				if (value == maximizeEnabled)
					return;
				maximizeEnabled = value;
				RefreshBorder();
			}
		}

		/// <summary>
		/// Gets or sets whether the minimize button is enabled.
		/// </summary>
		[Description("Gets or sets whether the minimize button is enabled.")]
		[DefaultValue(true)]
		public bool MinimizeEnabled {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return minimizeEnabled && ShowInTaskbar;
			}
			set {
				if (value == minimizeEnabled)
					return;
				minimizeEnabled = value;
				RefreshBorder();
			}
		}

		/// <summary>
		/// Gets or sets whether to show the outer border.
		/// </summary>
		[Description("Gets or sets whether to show the outer border.")]
		[DefaultValue(true)]
		public bool ShowBorder {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return showBorder;
			}
			set {
				if (showBorder == value)
					return;
				showBorder = value;
				if (isFullScreen)
					return;
				else if (value) {
					RefreshBorder();
					OnShowBorderChanged();
				} else {
					OnBorderSizeChangedInner(-borderWidth, -titleBarHeight);
					OnShowBorderChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets the window border width.
		/// </summary>
		[Description("Gets or sets the window border width.")]
		public int BorderWidth {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return borderWidth;
			}
			set {
				if (value < 1)
					value = 1;
				if (borderWidth == value)
					return;
				int diffWidth = value - borderWidth;
				borderWidth = value;
				int diffTitleBar = 0;
				int minTitleBar = value + value;
				if (titleBarHeight < minTitleBar) {
					diffTitleBar = minTitleBar - titleBarHeight;
					titleBarHeight = minTitleBar;
				}
				TitleBarChanged(diffWidth, diffTitleBar);
			}
		}

		/// <summary>
		/// Gets or sets the window title bar height.
		/// </summary>
		[Description("Gets or sets the window title bar height.")]
		public int TitleBarHeight {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return titleBarHeight;
			}
			set {
				int min = borderWidth * 2;
				if (value < min)
					value = min;
				int diff = value - titleBarHeight;
				titleBarHeight = value;
				TitleBarChanged(0, diff);
			}
		}

		/// <summary>
		/// Gets whether the window is currently in the process of becoming closed.
		/// </summary>
		[Browsable(false)]
		public bool IsClosing {
			get;
			private set;
		}

		/// <summary>
		/// Gets the current border width at this point in time (0 if the border is hidden, the window is maximized or fullscreen).
		/// </summary>
		[Browsable(false)]
		public int CurrentBorderWidth {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return !IsBorderVisible || (isMaximized && (animatingTopInner || IsFullyMaximized)) ? 0 : (int) (borderWidth * DpiScale.Height);
			}
		}

		/// <summary>
		/// Gets the current title bar height at this point in time (0 if the border is hidden or the window is fullscreen).
		/// </summary>
		[Browsable(false)]
		public int CurrentTitleBarHeight {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return IsBorderVisible ? (int) (titleBarHeight * DpiScale.Height) : 0;
			}
		}

		/// <summary>
		/// Gets the actual location of the window on the screen.
		/// </summary>
		[Browsable(false)]
		public Point ClientLocation {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				Point offset = PointToClient(Location);
				return new Point(-offset.X, -offset.Y);
			}
		}

		/// <summary>
		/// Gets or sets the window view port. This get the actual client area, taking all relevant window states into account.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Rectangle ViewPort {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return new Rectangle(ViewPortLocation, ViewSize);
			}
		}

		/// <summary>
		/// Gets or sets the window view port location. This get the actual client area location, taking all relevant window states into account.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Point ViewPortLocation {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return new Point(CurrentBorderWidth, CurrentTitleBarHeight) - (Size) PointToClient(Location);
			}
		}

		/// <summary>
		/// Gets or sets the window view size. This get the actual client area size, taking all relevant window states into account.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Size ViewSize {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				int borderWidth = CurrentBorderWidth;
				return new Size(ClientSize.Width - borderWidth * 2, ClientSize.Height - (CurrentTitleBarHeight + borderWidth));
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				ClientSize += value - ViewSize;
			}
		}

		/// <summary>
		/// When using OpenGL and GDI+ controls on Windows, if the window is exactly the size of the screen,
		/// native GDI controls (not the OpenGL GDI layer) glitch out and vanish. If the workaround is enabled,
		/// at fullscreen, the height of the window is set to the height of the screen + 1.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool FullscreenGdiGLWorkaround {
			get {
				return fullscreenGdiGLWorkaround;
			}
			set {
				if (value == fullscreenGdiGLWorkaround)
					return;
				fullscreenGdiGLWorkaround = value;
				if (isFullScreen) {
					int height = screen.Bounds.Height;
					if (value)
						height++;
					Height = height;
				}
			}
		}

		/// <summary>
		/// Gets or sets the text color.
		/// </summary>
		[Description("Gets or sets the text color.")]
		[DefaultValue(typeof(Color), "0xFFFFFF")]
		public override Color ForeColor {
			get {
				return titleLabel.ForeColor;
			}
			set {
				if (value == titleLabel.ForeColor)
					return;
				titleLabel.ForeColor = value;
				RedrawBorder(false);
			}
		}

		/// <summary>
		/// Gets or sets the text font.
		/// </summary>
		[Description("Gets or sets the text font.")]
		[DefaultValue(null)]
		public override Font Font {
			get {
				return titleLabel.Font;
			}
			set {
				if (value == titleLabel.Font)
					return;
				titleLabel.Font = value;
				RedrawBorder(false);
			}
		}

		/// <summary>
		/// Gets or sets whether the window is fullscreen.
		/// </summary>
		[DefaultValue(false)]
		public bool IsFullScreen {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return isFullScreen;
			}
			set {
				if (isFullScreen == value)
					return;
				isFullScreen = value;
				if (DesignMode)
					return;
				else if (value) {
					WindowBorder = FormBorderStyle;
					if (WindowState == FormWindowState.Maximized)
						isMaximized = true;
					if (!isMaximized)
						oldBounds = GetTargetBounds();
					wasMaximized = isMaximized;
					if (WindowBorder != FormBorderStyle.None)
						FormBorderStyle = FormBorderStyle.None;
					if (isMaximized) {
						WindowState = FormWindowState.Normal;
						isMaximized = false;
					} else if (IsMinimized)
						WindowState = FormWindowState.Normal;
					Padding = Padding.Empty;
					AnimateBoundsTo(screen.Bounds);
					wasTopMost = TopMost;
					TopMost = true;
					Activate();
					OnEnterFullScreen();
				} else {
					if (WindowBorder != FormBorderStyle.None)
						FormBorderStyle = WindowBorder;
					if (wasMaximized) {
						Rectangle oldBoundsTemp = oldBounds;
						Maximize();
						oldBounds = oldBoundsTemp;
					} else
						AnimateBoundsTo(oldBounds);
					TopMost = wasTopMost;
					OnLeaveFullScreen();
				}
			}
		}

		/// <summary>
		/// Gets or sets whether the form is visibly presented to the user.
		/// </summary>
		public new bool Visible {
			get {
				return gdiVisible || base.Visible;
			}
			set {
				base.Visible = value;
			}
		}

		/// <summary>
		/// Gets the default window size
		/// </summary>
		[Browsable(false)]
		protected override Size DefaultSize {
			get {
				return new Size(640, 480);
			}
		}

		/// <summary>
		/// Gets the default minimum window size
		/// </summary>
		[Browsable(false)]
		protected override Size DefaultMinimumSize {
			get {
				return new Size(200, 50);
			}
		}

		/// <summary>
		/// Gets or sets the window caption text in the title bar.
		/// </summary>
		[Description("Gets or sets the window caption text in the title bar.")]
		[DefaultValue("Styled Form")]
		public override string Text {
			get {
				return base.Text;
			}
			set {
				if (value == null)
					value = string.Empty;
				if (base.Text == value)
					return;
				base.Text = value;
				OnTextChanged(EventArgs.Empty);
				if (titleLabel == null)
					return;
				titleLabel.Text = value.Trim();
				RedrawBorder(false);
			}
		}

		/// <summary>
		/// Initializes a new styled window instance.
		/// </summary>
		/// <param name="container">The container to add this form to.</param>
		public StyledForm(IContainer container) : this() {
			if (container != null)
				container.Add(this);
		}

		/// <summary>
		/// Initializes a new styled window instance.
		/// </summary>
		public StyledForm() {
			DpiScale = new SizeF(1f, 1f);
			residentThread = Thread.CurrentThread;
			setVisibleCore = SetVisible;
			physicalMouseMoveEvent = MouseListener_MouseMove;
			globalCursorMove = MouseListener_CursorMove;
			globalMouseUp = MouseListener_MouseUp;
			updateLayered = (Action) Delegate.CreateDelegate(typeof(Action), this, TypeOfForm.GetMethod("UpdateLayered", BindingFlags.Instance | BindingFlags.NonPublic));
			BoundsProperty = new FieldOrProperty(nameof(Bounds), this);
			TopInnerProperty = new FieldOrProperty(nameof(TopInner), this);
			BorderOpacityProperty = new FieldOrProperty(nameof(currentBorderOpacity), this);
			BorderOverlayProperty = new FieldOrProperty(nameof(currentBorderOverlay), this);
			OpacityProperty = new FieldOrProperty(typeof(StyledForm).GetProperties(BindingFlags.Instance | BindingFlags.Public).Single(p => p.Name == nameof(Opacity) && p.PropertyType.Equals(typeof(byte))), this);
			onMinimizeUpdate = OnMinimizeUpdate;
			onOpacityUpdate = OnOpacityUpdate;
			onBoundsUpdate = OnBoundsUpdate;
			onBorderUpdate = OnBorderUpdate;
			callOnResizeEnd = CallOnResizeEnd;
			CheckForIllegalCrossThreadCalls = false;
			Name = nameof(StyledForm);
			OnConstructorStarted();
			SetStyle(ControlStyles.StandardClick | ControlStyles.UserMouse | ControlStyles.ResizeRedraw, false);
			SetStyle(ControlStyles.CacheText | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);
			Cursor = DefaultCursor;
			minimumSize = DefaultMinimumSize;
			maximumSize = DefaultMaximumSize;
			EnableBorderAnimation = true;
			EnableResizeAnimation = true;
			EnableOpacityAnimation = true;
			EnableMinimizeAnimation = true;
			AutoScaleMode = AutoScaleMode.None;
			AutoScaleDimensions = new SizeF(6F, 13F);
			CausesValidation = false;
			KeyPreview = true;
			StartPosition = FormStartPosition.Manual;
			AllowTransparency = true;
			DoubleBuffered = true;
			ShowIcon = true;
			Text = "Styled Form";
			XColor = new Pen(Color.White, 1F);
			MaxSymbolColor = new Pen(Color.White, 1F);
			outlineColor = new Pen(Color.Black, 1f);
			inlineColor = new Pen(Color.Black, 1f);
			MinimizeFill = new SolidBrush(Color.White);
			Border = DefaultBorder;
			closeButtonRenderer = new StyleRenderer();
			closeButtonRenderer.SuppressColorChecking = true;
			closeButtonRenderer.Border = Color.FromArgb(65, 65, 65);
			closeButtonRenderer.NormalBackgroundTop = Color.FromArgb(235, 50, 50);
			closeButtonRenderer.NormalBackgroundBottom = Color.FromArgb(150, 0, 0);
			closeButtonRenderer.HoverBackgroundTop = Color.FromArgb(215, 185, 185);
			closeButtonRenderer.HoverBackgroundBottom = Color.FromArgb(113, 68, 68);
			closeButtonRenderer.PressedBackgroundTop = Color.FromArgb(214, 75, 75);
			closeButtonRenderer.PressedBackgroundBottom = Color.FromArgb(64, 0, 0);
			closeButtonRenderer.NormalInnerBorderWidth = 0f;
			closeButtonRenderer.HoverInnerBorderWidth = 0f;
			closeButtonRenderer.RoundCornerRadius = 0f;
			closeButtonRenderer.RoundedEdges = false;
			closeButtonRenderer.SuppressColorChecking = false;
			maximizeButtonRenderer = new StyleRenderer();
			maximizeButtonRenderer.SuppressColorChecking = true;
			maximizeButtonRenderer.Border = closeButtonRenderer.Border;
			maximizeButtonRenderer.NormalBackgroundTop = Color.FromArgb(190, 190, 210);
			maximizeButtonRenderer.NormalBackgroundBottom = Color.SlateGray;
			maximizeButtonRenderer.HoverBackgroundTop = Color.FromArgb(80, 80, 200);
			maximizeButtonRenderer.HoverBackgroundBottom = Color.FromArgb(0, 0, 180);
			maximizeButtonRenderer.PressedBackgroundTop = Color.FromArgb(75, 75, 160);
			maximizeButtonRenderer.PressedBackgroundBottom = Color.FromArgb(0, 0, 64);
			maximizeButtonRenderer.NormalInnerBorderWidth = 0f;
			maximizeButtonRenderer.HoverInnerBorderWidth = 0f;
			maximizeButtonRenderer.RoundCornerRadius = closeButtonRenderer.RoundCornerRadius;
			maximizeButtonRenderer.RoundedEdges = closeButtonRenderer.RoundedEdges;
			maximizeButtonRenderer.SuppressColorChecking = false;
			minimizeButtonRenderer = new StyleRenderer();
			minimizeButtonRenderer.SuppressColorChecking = true;
			minimizeButtonRenderer.Border = closeButtonRenderer.Border;
			minimizeButtonRenderer.NormalBackgroundTop = maximizeButtonRenderer.NormalBackgroundTop;
			minimizeButtonRenderer.NormalBackgroundBottom = maximizeButtonRenderer.NormalBackgroundBottom;
			minimizeButtonRenderer.HoverBackgroundTop = maximizeButtonRenderer.HoverBackgroundTop;
			minimizeButtonRenderer.HoverBackgroundBottom = maximizeButtonRenderer.HoverBackgroundBottom;
			minimizeButtonRenderer.PressedBackgroundTop = maximizeButtonRenderer.PressedBackgroundTop;
			minimizeButtonRenderer.PressedBackgroundBottom = maximizeButtonRenderer.PressedBackgroundBottom;
			minimizeButtonRenderer.NormalInnerBorderWidth = 0f;
			minimizeButtonRenderer.HoverInnerBorderWidth = 0f;
			minimizeButtonRenderer.RoundCornerRadius = closeButtonRenderer.RoundCornerRadius;
			minimizeButtonRenderer.RoundedEdges = closeButtonRenderer.RoundedEdges;
			minimizeButtonRenderer.SuppressColorChecking = false;
			screen = Screen.FromControl(this);
			titleLabel = new StyledLabel(Text);
			titleLabel.BackColor = Color.Transparent;
			titleLabel.ForeColor = Color.White;
			titleLabel.AutoSize = false;
			titleLabel.RenderShadow = true;
			titleLabel.Font = new Font("Microsoft Sans Serif", 10F);
			titleLabel.TextAlign = ContentAlignment.MiddleCenter;
			titleLabel.ShadowOpacity = 1.3f;
			closeButtonRenderer.FunctionToCallOnRefresh = CloseButtonFunc;
			maximizeButtonRenderer.FunctionToCallOnRefresh = MaximizeButtonFunc;
			minimizeButtonRenderer.FunctionToCallOnRefresh = MinimizeButtonFunc;
			FormBorderStyle = FormBorderStyle.None;
			ShowBorder = true;
			InitializeComponent();
		}

		private object CallOnResizeEnd(object parameter) {
			OnResizeEnd(parameter as EventArgs);
			return null;
		}

		private void RefreshDpi() {
			int dpiX, dpiY;
			using (Graphics g = Graphics.FromHwnd(IntPtr.Zero)) {
				IntPtr desktop = g.GetHdc();
				dpiX = NativeApi.GetDeviceCaps(desktop, DeviceCap.LOGPIXELSX);
				dpiY = NativeApi.GetDeviceCaps(desktop, DeviceCap.LOGPIXELSY);
				g.ReleaseHdc(desktop);
			}
			SizeF oldScale = DpiScale;
			DpiScale = new SizeF(dpiX * 0.01041666666667f, dpiY * 0.01041666666667f);
			if (!(oldScale.Width == DpiScale.Width && oldScale.Height == DpiScale.Height)) {
				Scale(new SizeF(DpiScale.Width / oldScale.Width, DpiScale.Height / oldScale.Height));
				TitleBarChanged(0, 0);
				OnDpiChanged();
			}
		}

		/// <summary>
		/// Called when the constructor is about to start. This is called only once.
		/// Don't do anything fancy, almost everything is null or uninitialized
		/// </summary>
		protected virtual void OnConstructorStarted() {
		}

		/// <summary>
		/// Only here for designer compatibility
		/// </summary>
		private void InitializeComponent() {
		}

		/// <summary>
		/// Performs preliminary configuration to ensure stability.
		/// </summary>
		private static bool InitApp() {
			WindowsFormsSynchronizationContext.AutoInstall = false;
			if (isInit)
				return false;
			isInit = true;
			if (Platform.IsWindowsVistaOrNewer) {
				try {
					NativeApi.SetProcessDpiAwareness(DpiAwareness.PerMonitorAware);
				} catch {
					try {
						NativeApi.SetProcessDPIAware();
					} catch {
					}
				}
			}
			if (Platform.IsWindowsXPOrNewer) {
				Application.EnableVisualStyles();
				Application.VisualStyleState = VisualStyles.VisualStyleState.ClientAndNonClientAreasEnabled;
			}
			typeof(Control).GetField("UseCompatibleTextRenderingDefault", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, false); //Application.SetCompatibleTextRenderingDefault(false);
			typeof(NativeWindow).GetField("userSetProcFlagsForApp", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, (byte) 1); //Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
			if (Platform.IsWindowsVistaOrNewer) {
				NativeApi.ChangeWindowMessageFilter(WindowMessage.DROPFILES, MessageFilter.Allow);
				NativeApi.ChangeWindowMessageFilter(WindowMessage.COPYGLOBALDATA, MessageFilter.Allow);
			}
			//Disable SystemEvents due to buggy behaviour
			Type systemEvents = typeof(Stack<>).Assembly.GetType("Microsoft.Win32.SystemEvents");
			if (systemEvents == null)
				return true;
			systemEvents.GetMethod("Shutdown", BindingFlags.Static | BindingFlags.NonPublic, null, Type.EmptyTypes, null).Invoke(null, null);
			systemEvents.GetField("_handlers", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, null);
			systemEvents.GetField(nameof(systemEvents), BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, systemEvents.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null).Invoke(null));
			AppDomain.CurrentDomain.UnhandledException += UniversalExceptionHandler;
			Application.ThreadException += Application_ThreadException;
			return true;
		}

		/// <summary>
		/// Executes the specified delegate asynchronously on the thread that the control's underlying handle was created on.
		/// The InvocationData overload is much faster.
		/// </summary>
		/// <param name="method">A delegate to a method that takes no parameters. </param>
		public new IAsyncResult BeginInvoke(Delegate method) {
			return BeginInvoke(method, null);
		}

		/// <summary>
		/// Executes the specified delegate asynchronously with the specified arguments, on the thread that the control's underlying handle was created on.
		/// The InvocationData overload is much faster.
		/// </summary>
		/// <param name="method">A delegate to a method that takes parameters of the same number and type that are contained in the args parameter.</param>
		/// <param name="args">An array of objects to pass as arguments to the given method. This can be null if no arguments are needed. </param>
		public new IAsyncResult BeginInvoke(Delegate method, params object[] args) {
			if (method == null)
				return null;
			InvocationData invocation = new InvocationData((obj) => method.DynamicInvoke(args), null);
			BeginInvoke(invocation);
			return invocation;
		}

		/// <summary>
		/// Asychronously invokes the specified method with the specified parameters.
		/// </summary>
		/// <param name="invocation">Holds the data required for an invocation.</param>
		public void BeginInvoke(InvocationData invocation) {
			if (invocation == null)
				return;
			else if (IsHandleCreated && !IsDisposed) {
				callbacks.Enqueue(invocation);
				NativeApi.PostMessage(Handle, InvokeMessage, IntPtr.Zero, IntPtr.Zero);
			} else
				invocation.InvokeOnCurrentThread();
		}

		/// <summary>
		/// Executes the specified delegate on the thread that owns the control's underlying window handle.
		/// The InvocationData overload is much faster.
		/// </summary>
		/// <param name="method">A delegate that contains a method to be called in the control's thread context. </param>
		public new object Invoke(Delegate method) {
			return Invoke(method, null);
		}

		/// <summary>
		/// Executes the specified delegate, on the thread that owns the control's underlying window handle, with the specified list of arguments.
		/// The InvocationData overload is much faster.
		/// </summary>
		/// <param name="method">A delegate to a method that takes parameters of the same number and type that are contained in the args parameter.</param>
		/// <param name="args">An array of objects to pass as arguments to the specified method. This parameter can be null if the method takes no arguments.</param>
		public new object Invoke(Delegate method, params object[] args) {
			if (method == null)
				return null;
			InvocationData invocation = new InvocationData((obj) => method.DynamicInvoke(args), null);
			Invoke(invocation);
			return invocation;
		}

		/// <summary>
		/// Invokes the specified method on the window's thread synchronously.
		/// </summary>
		/// <param name="invocation">Holds the data required for an invocation.</param>
		/// <param name="timeout">The invocation timeout. If the timeout is zero or smaller, the timeout is indefinite.</param>
		public object Invoke(InvocationData invocation, int timeout = 0) {
			if (invocation == null)
				return null;
			else if (IsHandleCreated && !IsDisposed) {
				callbacks.Enqueue(invocation);
				if (timeout <= 0)
					NativeApi.SendMessage(Handle, InvokeMessage, IntPtr.Zero, IntPtr.Zero);
				else {
					IntPtr output;
					NativeApi.SendMessageTimeout(Handle, InvokeMessage, IntPtr.Zero, IntPtr.Zero, SendMessageTimeoutFlags.NORMAL, (uint) timeout, out output);
				}
			} else
				invocation.InvokeOnCurrentThread();
			invocation.CompletedSynchronously = true;
			return invocation.AsyncState;
		}

		private static void UniversalExceptionHandler(object sender, UnhandledExceptionEventArgs e) {
			ErrorHandler.Show("An unhandled error has occurred.", e.ExceptionObject as Exception);
		}

		private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e) {
			ErrorHandler.Show("A thread exception has occurred.", e.Exception);
		}

		/// <summary>
		/// Creates the handle for this window.
		/// </summary>
		protected override void CreateHandle() {
			using (TimedLock timedLock = new TimedLock(this, 3000)) {
				if (!(IsClosing || IsHandleCreated)) {
					residentThread = Thread.CurrentThread;
					bool wasDisposed = IsDisposed;
					disposed = DisposeOptions.None;
					this.SetState(2048, false);
					base.CreateHandle();
					if (Platform.IsWindowsXPOrNewer)
						NativeApi.DragAcceptFiles(Handle, allowDrop);
					if (!(DesignMode || updateLayered == null))
						updateLayered();
					UpdateAeroBlur();
					if (Platform.IsWindows7OrNewer)
						NativeApi.RegisterTouchWindow(Handle, 1u);
					if (wasDisposed) {
						using (TimedLock timeLock = new TimedLock(oldControlSyncRoot, 2000)) {
							Control child;
							for (int i = 0; i < oldControls.Count; i++) {
								child = oldControls[i];
								if (child.InvokeRequired)
									RecreateControlHandle(child);
								try {
									Controls.Add(child);
								} catch {
								}
							}
							oldControls.Clear();
						}
					}
				}
			}
		}

		private void UpdateAeroBlur() {
			if (Extensions.IsAeroEnabled) {
				IntPtr handle = Handle;
				if (useSetWindowComposition) {
					AccentPolicy accent = new AccentPolicy();
					accent.AccentState = enableAeroBlur ? AccentState.ENABLE_BLURBEHIND : AccentState.DISABLED;
					WindowCompositionAttributeData data = new WindowCompositionAttributeData();
					data.Attribute = DwmWindowAttribute.ACCENT_POLICY;
					data.SizeOfData = AccentPolicy.Size;
					unsafe
					{
						data.Data = new IntPtr(&accent);
					}
					NativeApi.SetWindowCompositionAttribute(handle, ref data);
				}
				DWM_BLURBEHIND style = new DWM_BLURBEHIND() {
					dwFlags = DWM_BB.Enable,
					fEnable = enableAeroBlur
				};
				if (NativeApi.DwmEnableBlurBehindWindow(handle, ref style) != IntPtr.Zero)
					enableAeroBlur = false;
			} else
				enableAeroBlur = false;
		}

		/// <summary>
		/// Called when the ShowInTaskbar property has changed
		/// </summary>
		/// <param name="showInTaskbar">The new value of the property</param>
		protected virtual void OnShowInTaskbarChanged(bool showInTaskbar) {
		}

		private object SetShowInTaskbarCore(object newValue) {
			bool newValueBool = (bool) newValue;
			if (newValueBool) {
				bool wasVisible = Visible;
				if (wasVisible)
					SetVisibleNoAnimation(false);
				try {
					base.ShowInTaskbar = true;
				} catch {
				}
				if (wasVisible)
					SetVisibleNoAnimation(true);
			} else
				base.ShowInTaskbar = false;
			OnShowInTaskbarChanged(newValueBool);
			return null;
		}

		/// <summary>
		/// Destroys the current window handle.
		/// </summary>
		protected override void DestroyHandle() {
			Thread thread = new Thread(base.DestroyHandle);
			thread.IsBackground = true;
			thread.Start();
			thread.Join(1200);
		}

		/// <summary>
		/// Called when the form handle is created.
		/// </summary>
		/// <param name="e">Ignored.</param>
		protected override void OnHandleCreated(EventArgs e) {
			EventHandler item = (EventHandler) Events[EventHandleCreated];
			if (item != null)
				item(this, e);
		}

		/// <summary>
		/// Called when the form handle is destroyed.
		/// </summary>
		/// <param name="e">Ignored.</param>
		protected override void OnHandleDestroyed(EventArgs e) {
			EventHandler item = (EventHandler) Events[EventHandleDestroyed];
			if (item != null)
				item(this, e);
		}

		/// <summary>
		/// Invalidates the entire surface of the control and causes the control to be redrawn.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public new void Invalidate() {
			Invalidate(false);
		}

		/// <summary>
		/// Invalidates the entire surface of the control and causes the control to be redrawn.
		/// </summary>
		/// <param name="invalidateChildren">If true, child controls are invalidated as well.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public new void Invalidate(bool invalidateChildren) {
			if (invalidateChildren && IsGLEnabled) {
				for (int i = 0; i < Controls.Count; i++)
					Controls[i].Invalidate(true);
			} else if (!IsGLEnabled) {
				if (IsHandleCreated)
					base.Invalidate(ViewPort, invalidateChildren);
				else
					NotifyInvalidate(ViewPort);
			}
		}

		/// <summary>
		/// Invalidates the specified region of the control (in viewport coordinates).</summary>
		/// <param name="rect">The region to invalidate in viewport coordinates.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public new void Invalidate(Rectangle rect) {
			Invalidate(rect, false);
		}

		/// <summary>
		/// Invalidates the specified region of the control (in viewport coordinates).</summary>
		/// <param name="rect">The region to invalidate in viewport coordinates.</param>
		/// <param name="invalidateChildren">If true, child controls are invalidated as well.</param>
		public virtual new void Invalidate(Rectangle rect, bool invalidateChildren) {
			rect.Offset(ViewPortLocation);
			Size size = ClientSize;
			if (rect.Right > size.Width)
				rect.Width = size.Width - rect.X;
			if (rect.Bottom > size.Height)
				rect.Height = size.Height - rect.Y;
			if (invalidateChildren && IsGLEnabled) {
				Control ctrl;
				Rectangle client = new Rectangle();
				for (int i = 0; i < Controls.Count; i++) {
					ctrl = Controls[i];
					client.Location = ctrl.Location;
					client = new Rectangle(rect.X - client.X, rect.Y - client.Y, rect.Width, rect.Height);
					if (client.IntersectsWith(new Rectangle(Point.Empty, ctrl.Size)))
						ctrl.Invalidate(client, true);
				}
			} else if (!IsGLEnabled) {
				if (IsHandleCreated)
					base.Invalidate(rect, invalidateChildren);
				else
					NotifyInvalidate(rect);
			}
		}

		/// <summary>
		/// Shows the form to the user.
		/// </summary>
		public new void Show() {
			Visible = true;
		}

		/// <summary>
		/// Shows the form with the specified owner to the user.
		/// </summary>
		/// <param name="owner">The owner of the window.</param>
		public new void Show(IWin32Window owner) {
			Visible = true;
			Form form = owner as Form;
			if (form != null)
				Owner = form;
		}

		/// <summary>
		/// Shows the form as a modal dialog box.
		/// </summary>
		public new DialogResult ShowDialog() {
			return MessageLoop.ShowDialog(this, false);
		}

		/// <summary>
		/// Shows the form as a modal dialog box with the specified owner.
		/// </summary>
		/// <param name="owner">The owner of the window.</param>
		public new DialogResult ShowDialog(IWin32Window owner) {
			return MessageLoop.ShowDialog(this, false);
		}

		/// <summary>
		/// Sets the visible property to the specified value immediately without animating.
		/// </summary>
		/// <param name="value">Whether the form should now be visible.</param>
		public void SetVisibleNoAnimation(bool value) {
			bool wasOpacityAnimationEnabled = EnableOpacityAnimation;
			EnableOpacityAnimation = false;
			Visible = value;
			EnableOpacityAnimation = wasOpacityAnimationEnabled;
		}

		private bool CloseButtonFunc(AnimationInfo state) {
			if (closeEnabled)
				RedrawBorder(true, CloseBounds);
			return true;
		}

		private bool MaximizeButtonFunc(AnimationInfo state) {
			if (maximizeEnabled)
				RedrawBorder(true, MaximizeBounds);
			return true;
		}

		private bool MinimizeButtonFunc(AnimationInfo state) {
			if (MinimizeEnabled)
				RedrawBorder(true, MinimizeBounds);
			return true;
		}

		/// <summary>
		/// Called whenever a child control is added to the form.
		/// </summary>
		/// <param name="e">The control that was added.</param>
		protected override void OnControlAdded(ControlEventArgs e) {
			base.OnControlAdded(e);
			if (!(DpiScale.Width == 1f && DpiScale.Height == 1f))
				e.Control.Scale(new SizeF(DpiScale.Width, DpiScale.Height));
		}

		/// <summary>
		/// Called whenever a child control is removed from the form.
		/// </summary>
		/// <param name="e">The control that was added.</param>
		protected override void OnControlRemoved(ControlEventArgs e) {
			base.OnControlRemoved(e);
			if (!(DpiScale.Width == 1f && DpiScale.Height == 1f))
				e.Control.Scale(new SizeF(1f / DpiScale.Width, 1f / DpiScale.Height));
			//UIScaler.RemoveFromScaler(e.Control);
		}

		private void border_MouseLeave(object sender, EventArgs e) {
			wasInsideClose = false;
			wasInsideMaximize = false;
			wasInsideMinimize = false;
			closeButtonRenderer.MarkMouseHasLeft();
			maximizeButtonRenderer.MarkMouseHasLeft();
			minimizeButtonRenderer.MarkMouseHasLeft();
		}

		/// <summary>
		/// Repaints the border.
		/// </summary>
		/// <param name="sync">Whether to wait for the painting to be complete.</param>
		private void RedrawBorder(bool sync) {
			RedrawBorder(sync, ClientRectangle);
		}

		/// <summary>
		/// Redraws the border
		/// </summary>
		/// <param name="sync">Whether to wait for the painting to be complete</param>
		/// <param name="rect">The invalidated bounds</param>
		protected virtual void RedrawBorder(bool sync, Rectangle rect) {
			if (!AnimatingBounds && IsBorderVisible) {
				using (Region region = new Region(rect)) {
					region.Exclude(ViewPort);
					base.Invalidate(region);
				}
				if (sync)
					Update();
			}
		}

		/// <summary>
		/// Gets whether the specified point is considered to currently be inside the close button.
		/// </summary>
		/// <param name="point">The point to check in client-coordinates.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool IsPointInCloseButton(Point point) {
			if (closeEnabled) {
				Rectangle rect = CloseBounds;
				rect.Width++;
				rect.Height += 2;
				return rect.Contains(point);
			} else
				return false;
		}

		/// <summary>
		/// Gets whether the specified point is considered to currently be inside the maximize button.
		/// </summary>
		/// <param name="point">The point to check in client-coordinates.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool IsPointInMaximizeButton(Point point) {
			if (maximizeEnabled) {
				Rectangle rect = MaximizeBounds;
				if (!closeEnabled)
					rect.Width++;
				rect.Height += 2;
				return rect.Contains(point);
			} else
				return false;
		}

		/// <summary>
		/// Gets whether the specified point is considered to currently be inside the minimize button.
		/// </summary>
		/// <param name="point">The point to check in client-coordinates.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool IsPointInMinimizeButton(Point point) {
			if (MinimizeEnabled) {
				Rectangle rect = MinimizeBounds;
				if (!(closeEnabled || maximizeEnabled))
					rect.Width++;
				rect.Height += 2;
				return rect.Contains(point);
			} else
				return false;
		}

		/// <summary>
		/// Gets whether the specified client point is inside the border.
		/// </summary>
		/// <param name="point">The point to check in client coordinates.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool IsPointInBorder(Point point) {
			return IsBorderVisible && Enabled && ClientRectangle.Contains(point) && !ViewPort.Contains(point);
		}

		private void border_MouseDown(object sender, MouseEventArgs e) {
			wasSystemMenuShown = false;
			if (e.Button == MouseButtons.Left) {
				mouseDownOnBorder = true;
				HaltUpDownAnimation();
				CaptureControl = this;
				Capture = true;
				mouseStartingPoint = Cursor.Position;
				if (IsPointInCloseButton(e.Location))
					closeButtonRenderer.Pressed = true;
				else if (IsPointInMaximizeButton(e.Location))
					maximizeButtonRenderer.Pressed = true;
				else if (IsPointInMinimizeButton(e.Location))
					minimizeButtonRenderer.Pressed = true;
				else {
					Rectangle bounds = Bounds;
					bool max = isMaximized && (animatingTopInner || IsFullyMaximized);
					if (isResizable) {
						if (max)
							isBeingMoved = true;
						else {
							int borderWidth = (int) (this.borderWidth * DpiScale.Height);
							if (e.Y < borderWidth) {
								if (e.X < borderWidth)
									isBeingResizedTopLeft = true;
								else if (e.X > bounds.Width - borderWidth)
									isBeingResizedTopRight = true;
								else
									isBeingResizedTop = true;
							} else if (e.Y > bounds.Height - borderWidth) {
								if (e.X < borderWidth)
									isBeingResizedBottomLeft = true;
								else if (e.X > bounds.Width - borderWidth)
									isBeingResizedBottomRight = true;
								else
									isBeingResizedBottom = true;
							} else {
								if (e.X < borderWidth)
									isBeingResizedLeft = true;
								else if (e.X > bounds.Width - borderWidth)
									isBeingResizedRight = true;
								else
									isBeingMoved = true;
							}
						}
					}
					beforeResize = max ? oldBounds : bounds;
				}
			}
		}

		/// <summary>
		/// Gets the bounds of the window, and if it is currently being animated, returns the target bounds.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public Rectangle GetTargetBounds() {
			if (animatingBounds) {
				object target = UIAnimator.SharedAnimator.GetTarget(BoundsProperty);
				return target == null ? Bounds : (Rectangle) target;
			} else
				return Bounds;
		}

		private void border_MouseMove(object sender, MouseEventArgs e) {
			if (suppressLocation)
				return;
			Point curPos = Cursor.Position;
			Point mouseChange = curPos - (Size) mouseStartingPoint;
			Cursor resultantCursor = borderCursor;
			if (isBeingResizedTop) {
				int targetTop = beforeResize.Y + mouseChange.Y;
				int targetHeight = beforeResize.Height - mouseChange.Y;
				if (MinimumSize.Height != 0 && targetHeight < MinimumSize.Height) {
					int change = MinimumSize.Height - targetHeight;
					targetHeight = MinimumSize.Height;
					targetTop -= change;
				}
				if (MaximumSize.Height != 0 && targetHeight > MaximumSize.Height) {
					int change = MaximumSize.Height - targetHeight;
					targetHeight = MaximumSize.Height;
					targetTop -= change;
				}
				Top = targetTop;
				Height = targetHeight;
				resultantCursor = Cursors.SizeNS;
			} else if (isBeingResizedTopLeft) {
				int targetTop = beforeResize.Y + mouseChange.Y;
				int targetHeight = beforeResize.Height - mouseChange.Y;
				if (MinimumSize.Height != 0 && targetHeight < MinimumSize.Height) {
					int change = MinimumSize.Height - targetHeight;
					targetHeight = MinimumSize.Height;
					targetTop -= change;
				}
				if (MaximumSize.Height != 0 && targetHeight > MaximumSize.Height) {
					int change = MaximumSize.Height - targetHeight;
					targetHeight = MaximumSize.Height;
					targetTop -= change;
				}
				int targetLeft = beforeResize.X + mouseChange.X;
				int targetWidth = beforeResize.Width - mouseChange.X;
				if (MinimumSize.Width != 0 && targetWidth < MinimumSize.Width) {
					int change = MinimumSize.Width - targetWidth;
					targetWidth = MinimumSize.Width;
					targetLeft -= change;
				}
				if (MaximumSize.Width != 0 && targetWidth > MaximumSize.Width) {
					int change = MaximumSize.Width - targetWidth;
					targetWidth = MaximumSize.Width;
					targetLeft -= change;
				}
				Bounds = new Rectangle(targetLeft, targetTop, targetWidth, targetHeight);
				resultantCursor = Cursors.SizeNWSE;
			} else if (isBeingResizedTopRight) {
				int targetTop = beforeResize.Y + mouseChange.Y;
				int targetHeight = beforeResize.Height - mouseChange.Y;
				if (MinimumSize.Height != 0 && targetHeight < MinimumSize.Height) {
					int change = MinimumSize.Height - targetHeight;
					targetHeight = MinimumSize.Height;
					targetTop -= change;
				}
				if (MaximumSize.Height != 0 && targetHeight > MaximumSize.Height) {
					int change = MaximumSize.Height - targetHeight;
					targetHeight = MaximumSize.Height;
					targetTop -= change;
				}
				int targetWidth = beforeResize.Width + mouseChange.X;
				if (MinimumSize.Width != 0 && targetWidth < MinimumSize.Width)
					targetWidth = MinimumSize.Width;
				if (MaximumSize.Width != 0 && targetWidth > MaximumSize.Width)
					targetWidth = MaximumSize.Width;
				Top = targetTop;
				Size = new Size(targetWidth, targetHeight);
				resultantCursor = Cursors.SizeNESW;
			} else if (isBeingResizedLeft) {
				int targetLeft = beforeResize.X + mouseChange.X;
				int targetWidth = beforeResize.Width - mouseChange.X;
				if (MinimumSize.Width != 0 && targetWidth < MinimumSize.Width) {
					int change = MinimumSize.Width - targetWidth;
					targetWidth = MinimumSize.Width;
					targetLeft -= change;
				}
				if (MaximumSize.Width != 0 && targetWidth > MaximumSize.Width) {
					int change = MaximumSize.Width - targetWidth;
					targetWidth = MaximumSize.Width;
					targetLeft -= change;
				}
				Left = targetLeft;
				Width = targetWidth;
				resultantCursor = Cursors.SizeWE;
			} else if (isBeingResizedRight) {
				int targetWidth = beforeResize.Width + mouseChange.X;
				if (MinimumSize.Width != 0 && targetWidth < MinimumSize.Width)
					targetWidth = MinimumSize.Width;
				if (MaximumSize.Width != 0 && targetWidth > MaximumSize.Width)
					targetWidth = MaximumSize.Width;
				Width = targetWidth;
				resultantCursor = Cursors.SizeWE;
			} else if (isBeingResizedBottom) {
				int targetHeight = beforeResize.Height + mouseChange.Y;
				if (MinimumSize.Height != 0 && targetHeight < MinimumSize.Height)
					targetHeight = MinimumSize.Height;
				if (MaximumSize.Height != 0 && targetHeight > MaximumSize.Height)
					targetHeight = MaximumSize.Height;
				Height = targetHeight;
				resultantCursor = Cursors.SizeNS;
			} else if (isBeingResizedBottomLeft) {
				int targetHeight = beforeResize.Height + mouseChange.Y;
				if (MinimumSize.Height != 0 && targetHeight < MinimumSize.Height)
					targetHeight = MinimumSize.Height;
				if (MaximumSize.Height != 0 && targetHeight > MaximumSize.Height)
					targetHeight = MaximumSize.Height;
				int targetLeft = beforeResize.X + mouseChange.X;
				int targetWidth = beforeResize.Width - mouseChange.X;
				if (MinimumSize.Width != 0 && targetWidth < MinimumSize.Width) {
					int change = MinimumSize.Width - targetWidth;
					targetWidth = MinimumSize.Width;
					targetLeft -= change;
				}
				if (MaximumSize.Width != 0 && targetWidth > MaximumSize.Width) {
					int change = MaximumSize.Width - targetWidth;
					targetWidth = MaximumSize.Width;
					targetLeft -= change;
				}
				Left = targetLeft;
				Size = new Size(targetWidth, targetHeight);
				resultantCursor = Cursors.SizeNESW;
			} else if (isBeingResizedBottomRight) {
				int targetHeight = beforeResize.Height + mouseChange.Y;
				if (MinimumSize.Height != 0 && targetHeight < MinimumSize.Height)
					targetHeight = MinimumSize.Height;
				if (MaximumSize.Height != 0 && targetHeight > MaximumSize.Height)
					targetHeight = MaximumSize.Height;
				int targetWidth = beforeResize.Width + mouseChange.X;
				if (MinimumSize.Width != 0 && targetWidth < MinimumSize.Width)
					targetWidth = MinimumSize.Width;
				if (MaximumSize.Width != 0 && targetWidth > MaximumSize.Width)
					targetWidth = MaximumSize.Width;
				Size = new Size(targetWidth, targetHeight);
				resultantCursor = Cursors.SizeNWSE;
			} else if (isBeingMoved) {
				if (!isMaximized || (mouseChange.X > 5 || mouseChange.X < -5) || (mouseChange.Y > 5 || mouseChange.Y < -5)) {
					hasMoved = true;
					int currentTitleBarWidth = CurrentTitleBarHeight;
					resultantCursor = Cursors.SizeAll;
					Point oldMouseLoc = e.Location;
					Point mouseLoc = oldMouseLoc;
					if (mouseLoc.X < 0)
						mouseLoc.X = 0;
					else if (mouseLoc.X >= ClientSize.Width)
						mouseLoc.X = ClientSize.Width - 1;
					if (mouseLoc.Y < 0)
						mouseLoc.Y = 0;
					else if (mouseLoc.Y >= currentTitleBarWidth)
						mouseLoc.Y = currentTitleBarWidth - 1;
					if (mouseLoc != oldMouseLoc)
						mouseStartingPoint = new Point(mouseStartingPoint.X + mouseLoc.X - oldMouseLoc.X, mouseStartingPoint.Y + mouseLoc.Y - oldMouseLoc.Y);
					mouseChange = curPos - (Size) mouseStartingPoint;
					if ((isMaximized && (animatingTopInner || IsFullyMaximized))) {
						if (!wasSystemMenuShown) {
							suppressLocation = true;
							Point oldLoc = Location;
							oldBounds.X = oldLoc.X + (ClientSize.Width - oldBounds.Width) / 2;
							oldBounds.Y = oldLoc.Y + e.Y - titleBarHeight / 2;
							Maximize(false);
							suppressLocation = false;
							mouseStartingPoint = curPos;
							beforeResize = Bounds;
						}
					} else {
						suppressLocation = true;
						Location = new Point(beforeResize.X + mouseChange.X, beforeResize.Y + mouseChange.Y);
						suppressLocation = false;
					}
				}
			} else {
				if (mouseDownOnBorder) {
					if (wasInsideClose) {
						if (IsPointInCloseButton(e.Location))
							closeButtonRenderer.Pressed = true;
						else
							closeButtonRenderer.Pressed = false;
					}
					if (wasInsideMaximize) {
						if (IsPointInMaximizeButton(e.Location))
							maximizeButtonRenderer.Pressed = true;
						else
							maximizeButtonRenderer.Pressed = false;
					}
					if (wasInsideMinimize) {
						if (IsPointInMinimizeButton(e.Location))
							minimizeButtonRenderer.Pressed = true;
						else
							minimizeButtonRenderer.Pressed = false;
					}
				} else {
					if (IsPointInCloseButton(e.Location)) {
						wasInsideClose = true;
						closeButtonRenderer.MouseHovering = true;
					} else if (wasInsideClose) {
						wasInsideClose = false;
						closeButtonRenderer.MarkMouseHasLeft();
					}
					if (IsPointInMaximizeButton(e.Location)) {
						wasInsideMaximize = true;
						maximizeButtonRenderer.MouseHovering = true;
					} else if (wasInsideMaximize) {
						wasInsideMaximize = false;
						maximizeButtonRenderer.MarkMouseHasLeft();
					}
					if (IsPointInMinimizeButton(e.Location)) {
						wasInsideMinimize = true;
						minimizeButtonRenderer.MouseHovering = true;
					} else if (wasInsideMinimize) {
						wasInsideMinimize = false;
						minimizeButtonRenderer.MarkMouseHasLeft();
					}
				}
				if (isResizable && !(wasInsideClose || wasInsideMaximize || wasInsideMinimize || (isMaximized && (animatingTopInner || IsFullyMaximized)))) {
					int borderWidth = (int) (this.borderWidth * DpiScale.Height);
					if (e.Y < borderWidth) {
						if (e.X < borderWidth)
							resultantCursor = Cursors.SizeNWSE;
						else if (e.X > ClientSize.Width - borderWidth)
							resultantCursor = Cursors.SizeNESW;
						else
							resultantCursor = Cursors.SizeNS;
					} else if (e.Y >= ClientSize.Height - borderWidth) {
						if (e.X < borderWidth)
							resultantCursor = Cursors.SizeNESW;
						else if (e.X > ClientSize.Width - borderWidth)
							resultantCursor = Cursors.SizeNWSE;
						else
							resultantCursor = Cursors.SizeNS;
					} else if (e.X < borderWidth || e.X > ClientSize.Width - borderWidth)
						resultantCursor = Cursors.SizeWE;
				}
			}
			if (IsBorderVisible)
				Cursor = resultantCursor;
		}

		private void border_MouseUp(object sender, MouseEventArgs e) {
			CaptureControl = null;
			mouseDownOnBorder = false;
			bool wasResized = isBeingResizedTop || isBeingResizedTopLeft || isBeingResizedTopRight || isBeingResizedLeft || isBeingResizedRight || isBeingResizedBottom || isBeingResizedBottomLeft || isBeingResizedBottomRight;
			bool wereBoundsChanging = wasResized || hasMoved;
			isBeingMoved = false;
			isBeingResizedTop = false;
			isBeingResizedTopLeft = false;
			isBeingResizedTopRight = false;
			isBeingResizedLeft = false;
			isBeingResizedRight = false;
			isBeingResizedBottom = false;
			isBeingResizedBottomLeft = false;
			isBeingResizedBottomRight = false;
			if (hasMoved) {
				hasMoved = false;
				Rectangle screenBounds = screen.Bounds;
				Rectangle area = WorkingArea;
				Point curPos = Cursor.Position;
				if (curPos.Y == screenBounds.Top) {
					if (curPos.X == screenBounds.Left)
						AnimateBoundsTo(new Rectangle(area.Left, area.Top, area.Width / 2, area.Height));
					else if (curPos.X == screenBounds.Right - 1)
						AnimateBoundsTo(new Rectangle(area.Left + area.Width / 2, area.Top, area.Width / 2, area.Height));
					else if (maximizeEnabled)
						Maximize();
				} else if (wasResized)
					OnResizeEnd(e);
			} else if (wasResized)
				OnResizeEnd(e);
			Cursor = DefaultCursor;
			if (e.Button == MouseButtons.Left) {
				closeButtonRenderer.Pressed = false;
				maximizeButtonRenderer.Pressed = false;
				minimizeButtonRenderer.Pressed = false;
				if (!wereBoundsChanging) {
					if (wasInsideClose && IsPointInCloseButton(e.Location))
						CloseAsync();
					else
						closeButtonRenderer.MarkMouseHasLeft();
					if (wasInsideMaximize && IsPointInMaximizeButton(e.Location))
						Maximize();
					else
						maximizeButtonRenderer.MarkMouseHasLeft();
					if (wasInsideMinimize && IsPointInMinimizeButton(e.Location))
						Minimize();
					else
						minimizeButtonRenderer.MarkMouseHasLeft();
				}
			} else if (e.Button == MouseButtons.Right)
				ShowSystemMenu();
		}

		private void border_MouseDoubleClick(object sender, MouseEventArgs e) {
			if (maximizeEnabled && !(IsPointInCloseButton(e.Location) || IsPointInMinimizeButton(e.Location) || IsPointInMaximizeButton(e.Location))) {
				mouseDownOnBorder = false;
				isBeingMoved = false;
				Maximize();
			}
		}

		/// <summary>
		/// Translates the specified client coordinates to the corresponding screen location.
		/// </summary>
		/// <param name="clientPoint">The point in client coordinates to translate to screen coordinates.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public new Point PointToScreen(Point clientPoint) {
			return Extensions.PointToScreen(this, clientPoint);
		}

		/// <summary>
		/// Translates the specified screen coordinates to the corresponding client coordinate relative to the form.
		/// </summary>
		/// <param name="screenPoint">The point on the screen to translate to client coordinates.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public new Point PointToClient(Point screenPoint) {
			return Extensions.PointToClient(this, screenPoint);
		}

		/// <summary>
		/// Translates the specified screen coordinates to the corresponding client coordinate relative to the form.
		/// </summary>
		/// <param name="screenRect">The rectangle on the screen to translate to client coordinates.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public new Rectangle RectangleToClient(Rectangle screenRect) {
			screenRect.Location = PointToClient(screenRect.Location);
			return screenRect;
		}

		/// <summary>
		/// Translates the specified client coordinates to the corresponding screen location.
		/// </summary>
		/// <param name="clientRect">The rectangle in client coordinates to translate to screen coordinates.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public new Rectangle RectangleToScreen(Rectangle clientRect) {
			clientRect.Location = PointToScreen(clientRect.Location);
			return clientRect;
		}

		/// <summary>
		/// Opens the system context menu.
		/// </summary>
		public void ShowSystemMenu() {
			if (DesignMode)
				return;
			else if (SystemMenuStrip != null)
				SystemMenuStrip.Show(Cursor.Position);
			else if (SystemMenu != null)
				SystemMenu.Show(this, PointToClient(Cursor.Position));
			else {
				IntPtr handle = Handle;
				IntPtr wMenu = NativeApi.GetSystemMenu(handle, false);
				NativeApi.EnableMenuItem(wMenu, SystemCommand.SC_MAXIMIZE, maximizeEnabled ? SysCommandState.MF_ENABLED : SysCommandState.MF_GRAYED);
				NativeApi.EnableMenuItem(wMenu, SystemCommand.SC_SIZE, SysCommandState.MF_GRAYED);
				NativeApi.EnableMenuItem(wMenu, SystemCommand.SC_MINIMIZE, MinimizeEnabled ? SysCommandState.MF_ENABLED : SysCommandState.MF_GRAYED);
				const uint TPM_LEFTALIGN = 0x0000, TPM_RETURNCMD = 0x0100;
				Point pos = Cursor.Position;
				switch ((SystemCommand) NativeApi.TrackPopupMenuEx(wMenu, TPM_LEFTALIGN | TPM_RETURNCMD, pos.X, pos.Y, handle, IntPtr.Zero)) {
					case SystemCommand.SC_CLOSE:
						CloseAsync();
						break;
					case SystemCommand.SC_MOVE:
						if (IsBorderVisible) {
							if (isMaximized)
								Maximize(false);
							Point center = new Point(ClientSize.Width / 2, titleBarHeight / 2);
							mouseStartingPoint = PointToScreen(center);
							CaptureControl = this;
							MouseEventArgs mouseEvent = new MouseEventArgs(MouseButtons.Left, 1, center.X, center.Y, 0);
							border_MouseDown(this, mouseEvent);
							mouseStartingPoint = PointToScreen(center);
							Cursor.Position = mouseStartingPoint;
							Capture = true;
							MouseListener.CursorMove += globalCursorMove;
							MouseListener.MouseUp += globalMouseUp;
						}
						break;
					case SystemCommand.SC_MAXIMIZE:
						if (maximizeEnabled)
							Maximize();
						break;
					case SystemCommand.SC_MINIMIZE:
						if (WindowState != FormWindowState.Minimized)
							Minimize();
						break;
					case SystemCommand.SC_RESTORE:
						if (IsMinimized)
							Restore();
						break;
				}
				wasSystemMenuShown = true;
			}
		}

		private void MouseListener_CursorMove(object sender, MouseEventArgs e) {
			Point relative = PointToClient(e.Location);
			border_MouseMove(this, new MouseEventArgs(e.Button, e.Clicks, relative.X, relative.Y, e.Delta));
		}

		private void MouseListener_MouseUp(object sender, MouseEventArgs e) {
			MouseListener.CursorMove -= globalCursorMove;
			MouseListener.MouseUp -= globalMouseUp;
		}

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		internal static int GetButtonStates() {
			int wParam = 0;
			if (KeyListener.IsKeyDown(Keys.LButton))
				wParam |= 1;
			if (KeyListener.IsKeyDown(Keys.RButton))
				wParam |= 2;
			if (KeyListener.IsKeyDown(Keys.Shift))
				wParam |= 4;
			if (KeyListener.IsKeyDown(Keys.Control))
				wParam |= 8;
			if (KeyListener.IsKeyDown(Keys.MButton))
				wParam |= 16;
			if (KeyListener.IsKeyDown(Keys.XButton1))
				wParam |= 32;
			if (KeyListener.IsKeyDown(Keys.XButton2))
				wParam |= 64;
			return wParam;
		}

		/// <summary>
		/// Called when the window layout is calculated.
		/// </summary>
		/// <param name="levent">The layout arguments.</param>
		protected override void OnLayout(LayoutEventArgs levent) {
			if (DesignMode)
				Invoke(new InvocationData(l => {
					base.OnLayout((LayoutEventArgs) l);
					return null;
				}, levent));
			else
				base.OnLayout(levent);
		}

		/// <summary>
		/// Use the MouseDown event instead.
		/// </summary>
		protected sealed override void OnMouseDown(MouseEventArgs e) {
			if (IsPointInBorder(e.Location)) {
				border_MouseDown(this, e);
				return;
			}
			Point relativeLoc = PointFromClientToViewPort(e.Location);
			Control gdiCtrl = CaptureControl;
			Point clientLoc = relativeLoc;
			if (gdiCtrl == null)
				gdiCtrl = GetGdiChildAtPoint(relativeLoc, EligibleForMouse, ref clientLoc);
			else
				clientLoc = PointFromViewPortToGdiLayer(clientLoc - (Size) Extensions.PointToScreen(gdiCtrl, Point.Empty));
			wasSystemMenuShown = false;
			if (gdiCtrl == null || gdiCtrl == this) {
				CaptureControl = this;
				base.OnMouseDown(new MouseEventArgs(e.Button, e.Clicks, relativeLoc.X, relativeLoc.Y, e.Delta));
			} else {
				CaptureControl = gdiCtrl;
				WindowMessage msg;
				int wParam = GetButtonStates();
				switch (e.Button) {
					case MouseButtons.Left:
						msg = WindowMessage.LBUTTONDOWN;
						break;
					case MouseButtons.Middle:
						msg = WindowMessage.MBUTTONDOWN;
						break;
					case MouseButtons.Right:
						msg = WindowMessage.RBUTTONDOWN;
						break;
					case MouseButtons.XButton1:
						msg = WindowMessage.XBUTTONDOWN;
						wParam |= 0x10000;
						break;
					case MouseButtons.XButton2:
						msg = WindowMessage.XBUTTONDOWN;
						wParam |= 0x20000;
						break;
					default:
						msg = WindowMessage.NULL;
						break;
				}
				if (msg != WindowMessage.NULL) {
					Message message = new Message() {
						HWnd = gdiCtrl.Handle,
						Msg = (int) msg,
						WParam = new IntPtr(wParam),
						LParam = new IntPtr((clientLoc.Y << 16) | (clientLoc.X & 0xFFFF)),
					};
					gdiCtrl.CallWndProc(ref message);
				}
			}
			Activate();
			Capture = true;
		}

		/// <summary>
		/// Use the MouseMove event instead.
		/// </summary>
		protected sealed override void OnMouseMove(MouseEventArgs e) {
			bool isMouseInBorder = IsPointInBorder(e.Location);
			if (mouseDownOnBorder || isMouseInBorder) {
				mouseInBorder = isMouseInBorder;
				border_MouseMove(this, e);
				return;
			} else {
				bool hasLeftBorder = !mouseDownOnBorder && mouseInBorder && !isMouseInBorder;
				mouseInBorder = false;
				if (hasLeftBorder)
					border_MouseLeave(this, EventArgs.Empty);
			}
			Point relativeLoc = PointFromClientToViewPort(e.Location);
			Control gdiCtrl = CaptureControl;
			Point clientLoc = relativeLoc;
			if (gdiCtrl == null)
				gdiCtrl = GetGdiChildAtPoint(relativeLoc, EligibleForMouse, ref clientLoc);
			else
				clientLoc = PointFromViewPortToGdiLayer(clientLoc - (Size) Extensions.PointToScreen(gdiCtrl, Point.Empty));
			Message message;
			if (gdiCtrl == null || gdiCtrl == this) {
				if (!(MouseInsideControl == null || MouseInsideControl == this)) {
					message = new Message() {
						HWnd = MouseInsideControl.Handle,
						Msg = (int) WindowMessage.MOUSELEAVE,
						WParam = IntPtr.Zero,
						LParam = IntPtr.Zero,
					};
					MouseInsideControl.CallWndProc(ref message);
					OnMouseEnter(e);
				}
				MouseInsideControl = gdiCtrl;
				Cursor = defaultCursor;
				base.OnMouseMove(new MouseEventArgs(e.Button, e.Clicks, relativeLoc.X, relativeLoc.Y, e.Delta));
			} else {
				if (MouseInsideControl != gdiCtrl) {
					if (MouseInsideControl == null || MouseInsideControl == this) {
						OnMouseLeave(e);
						border_MouseLeave(this, EventArgs.Empty);
					} else {
						message = new Message() {
							HWnd = MouseInsideControl.Handle,
							Msg = (int) WindowMessage.MOUSELEAVE,
							WParam = IntPtr.Zero,
							LParam = IntPtr.Zero,
						};
						MouseInsideControl.CallWndProc(ref message);
					}
					message = new Message() {
						HWnd = gdiCtrl.Handle,
						Msg = MouseEnterMsg,
						WParam = IntPtr.Zero,
						LParam = IntPtr.Zero,
					};
					gdiCtrl.CallWndProc(ref message);
				}
				MouseInsideControl = gdiCtrl;
				message = new Message() {
					HWnd = gdiCtrl.Handle,
					Msg = (int) WindowMessage.MOUSEMOVE,
					WParam = new IntPtr(GetButtonStates()),
					LParam = new IntPtr((clientLoc.Y << 16) | (clientLoc.X & 0xFFFF)),
				};
				gdiCtrl.CallWndProc(ref message);
				Cursor = gdiCtrl.Cursor;
			}
		}

		/// <summary>
		/// Called when the mouse leaves the form.
		/// </summary>
		/// <param name="e">Ignored.</param>
		protected override void OnMouseLeave(EventArgs e) {
			border_MouseLeave(this, e);
			base.OnMouseLeave(e);
		}

		/// <summary>
		/// Use the MouseUp event instead.
		/// </summary>
		protected sealed override void OnMouseUp(MouseEventArgs e) {
			Capture = false;
			bool isMouseInBorder = IsPointInBorder(e.Location);
			if (mouseDownOnBorder || isMouseInBorder) {
				mouseInBorder = isMouseInBorder;
				border_MouseUp(this, e);
				if (!isMouseInBorder)
					border_MouseLeave(this, EventArgs.Empty);
				return;
			}
			Point relativeLoc = PointFromClientToViewPort(e.Location);
			Point clientLoc = relativeLoc;
			Control gdiCtrl = CaptureControl;
			if (gdiCtrl == null)
				gdiCtrl = GetGdiChildAtPoint(relativeLoc, EligibleForMouse, ref clientLoc);
			else
				clientLoc = PointFromViewPortToGdiLayer(clientLoc - (Size) Extensions.PointToScreen(gdiCtrl, Point.Empty));
			lastDoubleClickControl = gdiCtrl;
			if (gdiCtrl == null || gdiCtrl == this)
				base.OnMouseUp(new MouseEventArgs(e.Button, e.Clicks, relativeLoc.X, relativeLoc.Y, e.Delta));
			else {
				WindowMessage msg;
				int wParam = GetButtonStates();
				switch (e.Button) {
					case MouseButtons.Left:
						msg = WindowMessage.LBUTTONUP;
						break;
					case MouseButtons.Middle:
						msg = WindowMessage.MBUTTONUP;
						break;
					case MouseButtons.Right:
						msg = WindowMessage.RBUTTONUP;
						break;
					case MouseButtons.XButton1:
						msg = WindowMessage.XBUTTONUP;
						wParam |= 0x10000;
						break;
					case MouseButtons.XButton2:
						msg = WindowMessage.XBUTTONUP;
						wParam |= 0x20000;
						break;
					default:
						msg = WindowMessage.NULL;
						break;
				}
				if (msg != WindowMessage.NULL) {
					MouseEventArgs args = new MouseEventArgs(e.Button, e.Clicks, clientLoc.X, clientLoc.Y, e.Delta);
					if (msg == WindowMessage.RBUTTONUP) {
						MouseUpInvoke(gdiCtrl, args);
						gdiCtrl.SetState(67108864, false);
						gdiCtrl.SetState(134217728, false);
						gdiCtrl.SetState(268435456, false);
					} else {
						Message message = new Message() {
							HWnd = gdiCtrl.Handle,
							Msg = (int) msg,
							WParam = new IntPtr(wParam),
							LParam = new IntPtr((clientLoc.Y << 16) | (clientLoc.X & 0xFFFF)),
						};
						gdiCtrl.CallWndProc(ref message);
						if (msg == WindowMessage.LBUTTONUP && clientLoc.X >= 0 && clientLoc.Y >= 0 && clientLoc.X < gdiCtrl.Width && clientLoc.Y < gdiCtrl.Height)
							InvokeOnClick(gdiCtrl, args);
					}
				}
			}
			CaptureControl = null;
		}

		/// <summary>
		/// Use the MouseDoubleClick event instead.
		/// </summary>
		protected override void OnMouseDoubleClick(MouseEventArgs e) {
			if (IsPointInBorder(e.Location)) {
				border_MouseDoubleClick(this, e);
				return;
			}
			Control gdiCtrl = CaptureControl;
			Point relativeLoc = PointFromClientToViewPort(e.Location);
			Point clientLoc = relativeLoc;
			if (gdiCtrl == null)
				gdiCtrl = GetGdiChildAtPoint(relativeLoc, EligibleForMouse, ref clientLoc);
			else
				clientLoc = PointFromViewPortToGdiLayer(clientLoc - (Size) Extensions.PointToScreen(gdiCtrl, Point.Empty));
			if (gdiCtrl == null || gdiCtrl == this) {
				if (lastDoubleClickControl == null || lastDoubleClickControl == this)
					base.OnMouseDoubleClick(new MouseEventArgs(e.Button, e.Clicks, relativeLoc.X, relativeLoc.Y, e.Delta));
			} else if (gdiCtrl == lastDoubleClickControl) {
				WindowMessage msg;
				int wParam = GetButtonStates();
				switch (e.Button) {
					case MouseButtons.Left:
						msg = WindowMessage.LBUTTONDBLCLK;
						break;
					case MouseButtons.Middle:
						msg = WindowMessage.MBUTTONDBLCLK;
						break;
					case MouseButtons.Right:
						msg = WindowMessage.RBUTTONDBLCLK;
						break;
					case MouseButtons.XButton1:
						msg = WindowMessage.XBUTTONDBLCLK;
						wParam |= 0x10000;
						break;
					case MouseButtons.XButton2:
						msg = WindowMessage.XBUTTONDBLCLK;
						wParam |= 0x20000;
						break;
					default:
						msg = WindowMessage.NULL;
						break;
				}
				Message message = new Message() {
					HWnd = gdiCtrl.Handle,
					Msg = (int) msg,
					WParam = new IntPtr(wParam),
					LParam = new IntPtr((clientLoc.Y << 16) | (clientLoc.X & 0xFFFF)),
				};
				gdiCtrl.CallWndProc(ref message);
			}
		}

		/// <summary>
		/// Use the MouseWheel event instead.
		/// </summary>
		protected sealed override void OnMouseWheel(MouseEventArgs e) {
			Control gdiCtrl = CaptureControl;
			Point relativeLoc = PointFromClientToViewPort(e.Location);
			Point clientLoc = relativeLoc;
			if (gdiCtrl == null)
				gdiCtrl = GetGdiChildAtPoint(relativeLoc, EligibleForMouse, ref clientLoc);
			else
				clientLoc = PointFromViewPortToGdiLayer(clientLoc - (Size) Extensions.PointToScreen(gdiCtrl, Point.Empty));
			if (gdiCtrl == null || gdiCtrl == this)
				base.OnMouseWheel(new MouseEventArgs(e.Button, e.Clicks, relativeLoc.X, relativeLoc.Y, e.Delta));
			else
				MouseWheelInvoke(gdiCtrl, new MouseEventArgs(e.Button, e.Clicks, clientLoc.X, clientLoc.Y, e.Delta));
		}

		private void MouseListener_MouseMove(object sender, MouseEventArgs e) {
			MouseEventHandler handler = physicalMouseMove;
			if (handler != null && (CaptureControl == null || CaptureControl == this) && !(IsClosing || IsDisposed) && IsHandleCreated && Visible && Enabled && (Capture || NativeApi.WindowFromPoint(e.Location) == Handle)) {
				Point mouseLoc = PointFromScreenToViewPort(e.Location);
				if (GetChildAtPoint(mouseLoc, EligibleForMouse) == null)
					handler(this, new MouseEventArgs(e.Button, e.Clicks, mouseLoc.X, mouseLoc.Y, e.Delta));
			}
		}

		/// <summary>
		/// Called when the border texture has been updated
		/// </summary>
		protected virtual void OnBorderTextureChanged() {
		}

		/// <summary>
		/// Use the MouseDoubleClick event instead.
		/// </summary>
		protected sealed override void OnDoubleClick(EventArgs e) {
		}

		/// <summary>
		/// Use the MouseUp event instead.
		/// </summary>
		protected sealed override void OnClick(EventArgs e) {
		}

		/// <summary>
		/// Use the MouseUp event instead.
		/// </summary>
		protected sealed override void OnMouseClick(MouseEventArgs e) {
		}

		/// <summary>
		/// For internal use only.
		/// </summary>
		public void SetGdiVisible(bool value) {
			gdiVisible = value;
			if (value) {
				RefreshDpi();
				FadeState = FadeState.Normal;
				OnShown(EventArgs.Empty);
				opacity = 255;
				OnFadeInCompleted();
				if (!ShowWithoutActivation)
					Activate();
			}
		}

		/// <summary>
		/// Gets the child control that is found at the specified viewport coordinate (includes GDI layer coordinates).
		/// </summary>
		/// <param name="position">The viewport coordinate to check at.</param>
		public new Control GetChildAtPoint(Point position) {
			return GetChildAtPoint(position, ChildSkip.None);
		}

		/// <summary>
		/// Gets the child control that is found at the specified viewport coordinate (includes GDI layer controls).
		/// </summary>
		/// <param name="position">The viewport coordinate to check at.</param>
		/// <param name="skipParameters">Sets which control categories are skipped.</param>
		public new Control GetChildAtPoint(Point position, GetChildAtPointSkip skipParameters) {
			bool gdi;
			return GetChildAtPoint(position, (ChildSkip) skipParameters, out gdi);
		}

		/// <summary>
		/// Gets the child control that is found at the specified viewport coordinate (includes GDI layer controls).
		/// </summary>
		/// <param name="position">The viewport coordinate to check at.</param>
		/// <param name="skipParameters">Sets which control categories are skipped.</param>
		public Control GetChildAtPoint(Point position, ChildSkip skipParameters) {
			bool gdi;
			return GetChildAtPoint(position, skipParameters, out gdi);
		}

		/// <summary>
		/// Gets the child control that is found in the GDI layer at the specified viewport coordinate.
		/// </summary>
		/// <param name="position">The viewport coordinate to check at.</param>
		/// <param name="skipParameters">Sets which control categories are skipped.</param>
		/// <param name="clientToCtrl">If a GDI child control is found, its value is 'position' but in client coordinates relative to the returned control,
		/// else it's untouched.</param>
		protected virtual Control GetGdiChildAtPoint(Point position, ChildSkip skipParameters, ref Point clientToCtrl) {
			return null;
		}

		/// <summary>
		/// Gets the child control that is found at the specified viewport coordinate (includes GDI layer controls).
		/// </summary>
		/// <param name="position">The viewport coordinate to check at.</param>
		/// <param name="skipParameters">Sets which control categories are skipped.</param>
		/// <param name="isGdi">Whether the selected control is in the GDI layer.</param>
		public Control GetChildAtPoint(Point position, ChildSkip skipParameters, out bool isGdi) {
			Point client = position;
			Control candidate = GetGdiChildAtPoint(position, skipParameters, ref client);
			isGdi = candidate != null;
			if (isGdi)
				return candidate;
			else {
				Control ctrl = base.GetChildAtPoint(PointFromViewPortToClient(position), (GetChildAtPointSkip) (skipParameters & ~ChildSkip.DontRespondToMouse));
				if (ctrl == null || (skipParameters & ChildSkip.DontRespondToMouse) == ChildSkip.None)
					return ctrl;
				else {
					Point point = PointFromViewPortToScreen(position);
					Message message = new Message() {
						Msg = (int) WindowMessage.NCHITTEST,
						HWnd = ctrl.Handle,
						WParam = IntPtr.Zero,
						LParam = new IntPtr((point.Y << 16) | (point.X & 0xFFFF))
					};
					if (ctrl.CallWndProc(ref message) == TransparentControl.HTTRANSPARENT)
						return null;
					else
						return ctrl;
				}
			}
		}

		/// <summary>
		/// Converts a point from view port space to a point with coordinates relative to the GDI canvas.
		/// </summary>
		/// <param name="viewPortPoint">The point relative to the view port.</param>
		protected virtual Point PointFromViewPortToGdiLayer(Point viewPortPoint) {
			return viewPortPoint;
		}

		/// <summary>
		/// Converts a point from screen space to a point with coordinates relative to the view port.
		/// </summary>
		/// <param name="screenPoint">The point on screen (absolute).</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public Point PointFromScreenToViewPort(Point screenPoint) {
			return PointFromClientToViewPort(PointToClient(screenPoint));
		}

		/// <summary>
		/// Converts a point from client space to a point with coordinates relative to the view port.
		/// </summary>
		/// <param name="clientPoint">The point relative to the client area.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public Point PointFromClientToViewPort(Point clientPoint) {
			Point viewLoc = ViewPortLocation;
			return new Point(clientPoint.X - viewLoc.X, clientPoint.Y - viewLoc.Y);
		}

		/// <summary>
		/// Converts a point from view port space to a point with coordinates relative to the client area.
		/// </summary>
		/// <param name="viewPortPoint">The point relative to the view port.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public Point PointFromViewPortToClient(Point viewPortPoint) {
			Point viewLoc = ViewPortLocation;
			return new Point(viewPortPoint.X + viewLoc.X, viewPortPoint.Y + viewLoc.Y);
		}

		/// <summary>
		/// Converts a point from view port space to the corresponding screen coordinate.
		/// </summary>
		/// <param name="viewPortPoint">The point relative to the view port.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public Point PointFromViewPortToScreen(Point viewPortPoint) {
			return PointToScreen(PointFromViewPortToClient(viewPortPoint));
		}

		/// <summary>
		/// Animates the window size to the specified size.
		/// </summary>
		/// <param name="bounds">The target window size.</param>
		public void AnimateBoundsTo(Rectangle bounds) {
			if (fullscreenGdiGLWorkaround && bounds == screen.Bounds)
				bounds.Width++;
			if (FormBorderStyle == FormBorderStyle.None && EnableResizeAnimation && !DesignMode) {
				lock (onBoundsUpdate) {
					animatingBounds = true;
					UIAnimator.SharedAnimator.Animate(BoundsProperty, bounds, 0.7, 2.0, true, onBoundsUpdate, false);
				}
			} else {
				Bounds = bounds;
				OnBoundsUpdate(null);
			}
		}

		/// <summary>
		/// Halts the animation that happens when the window is minimized or restored (when the window sinks down or goes back up).
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void HaltUpDownAnimation() {
			if (animatingTopInner) {
				UIAnimator.SharedAnimator.Halt(TopInnerProperty, true, true);
				animatingTopInner = false;
			}
		}

		/// <summary>
		/// Processes the specified keys.
		/// </summary>
		/// <param name="msg">Not really used.</param>
		/// <param name="keyData">The key combination that is currently pressed.</param>
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
			if (keyData == Keys.Enter) {
				IButtonControl control = AcceptButton;
				if (control != null) {
					control.PerformClick();
					return true;
				}
			}
			if ((keyData & Keys.Alt) == Keys.Alt) {
				switch (keyData) {
					case Keys.Alt | Keys.Enter:
						if (EnableFullscreenOnAltEnter) {
							IsFullScreen = !IsFullScreen;
							Activate();
						}
						return true;
					case Keys.Alt | Keys.Space:
						if (!isFullScreen)
							ShowSystemMenu();
						return true;
					case Keys.Alt | Keys.Down:
						if (MinimizeEnabled && !IsMinimized)
							Minimize();
						else
							Activate();
						return true;
					case Keys.Alt | Keys.Up:
						if (maximizeEnabled)
							Maximize();
						Activate();
						return true;
					case Keys.Alt | Keys.F4:
						CloseAsync();
						return true;
					case Keys.Alt | Keys.Tab:
						return base.ProcessCmdKey(ref msg, keyData);
					default:
						return true;
				}
			} else
				return base.ProcessCmdKey(ref msg, keyData);
		}

		/// <summary>
		/// Processes the tab key.
		/// </summary>
		/// <param name="forward">Whether to continue processing the key.</param>
		protected override bool ProcessTabKey(bool forward) {
			Activate();
			return base.ProcessTabKey(forward);
		}

		/// <summary>
		/// Maximizes the window, or unmaximizes it if already maximized.
		/// </summary>
		/// <param name="animate">Whether the maximization should be animated.</param>
		public void Maximize(bool animate = true) {
			if (isFullScreen)
				return;
			isMaximized = !isMaximized;
			if (isMaximized) {
				UpdateShadow();
				oldBounds = GetTargetBounds();
				if (animate)
					AnimateBoundsTo(WorkingArea);
				else
					Bounds = WorkingArea;
			} else {
				WindowState = FormWindowState.Normal;
				UpdateShadow();
				TitleBarHeight = titleBarHeight;
				if (animate)
					AnimateBoundsTo(oldBounds);
				else
					Bounds = oldBounds;
			}
			OnClientSizeChanged(null);
			OnMaximizeChanged();
			RedrawBorder(false);
		}

		private void UpdateShadow() {
			bool shouldShowShadow = showShadow && Visible && !isFullScreen && !isMaximized;
			if (!Extensions.IsAeroEnabled || wasShadowEnabled == shouldShowShadow)
				return;
			wasShadowEnabled = shouldShowShadow;
			int v = (int) (shouldShowShadow ? DWMNCRENDERINGPOLICY.DWMNCRP_ENABLED : DWMNCRENDERINGPOLICY.DWMNCRP_DISABLED);
			IntPtr handle = Handle;
			NativeApi.DwmSetWindowAttribute(handle, DwmWindowAttribute.NCRENDERING_POLICY, ref v, sizeof(int));
			int enable = 0;
			NativeApi.DwmSetWindowAttribute(handle, DwmWindowAttribute.ALLOW_NCPAINT, ref enable, sizeof(int));
			MARGINS margins = new MARGINS() {
				leftWidth = 0,
				topHeight = 0,
				rightWidth = 0,
				bottomHeight = shouldShowShadow ? 1 : 0
			};
			NativeApi.DwmExtendFrameIntoClientArea(handle, ref margins);
		}

		/// <summary>
		/// Minimizes the window, or restores it if it already minimized.
		/// </summary>
		public void Minimize() {
			if (IsMinimized)
				Restore();
			else if (minimizing)
				return;
			animatingTopInner = true;
			restoring = false;
			minimizing = true;
			topBeforeMinimize = Top;
			topInner = topBeforeMinimize;
			Form[] ownedForms = OwnedForms;
			if (ownedForms.Length != 0) {
				for (int i = 0; i < ownedForms.Length; i++)
					NativeApi.SetWindowLong(ownedForms[i].Handle, GetWindowLongOffsets.HWNDPARENT, UIntPtr.Zero);
			}
			if (EnableMinimizeAnimation) {
				targetOpacity = Math.Max(opacity, (byte) 1);
				UIAnimator.SharedAnimator.Animate(TopInnerProperty, screen.Bounds.Bottom - 1, 0.5, 2.0, false, onMinimizeUpdate, true);
			} else
				OnMinimizeFinished();
			OnMinimizeChanged();
		}

		private void Restore() {
			if (restoring)
				return;
			animatingTopInner = true;
			minimizing = false;
			restoring = true;
			originalTopInner = topInner;
			if (EnableMinimizeAnimation && EnableOpacityAnimation && AllowTransparency && !DesignMode)
				Opacity = 1;
			WindowState = FormWindowState.Normal;
			topInner = Top;
			if (EnableMinimizeAnimation)
				UIAnimator.SharedAnimator.Animate(TopInnerProperty, topBeforeMinimize, 0.5, 2.0, false, onMinimizeUpdate, true);
			else
				OnMinimizeFinished();
			OnMinimizeChanged();
		}

		private bool OnMinimizeUpdate(AnimationInfo state) {
			if (state.IsFinished) {
				Top = topInner;
				OnMinimizeFinished();
				return true;
			} else if (EnableOpacityAnimation && AllowTransparency) {
				int diff = restoring ? originalTopInner - topBeforeMinimize : (int) state.TargetValue - topBeforeMinimize;
				if (diff == 0) {
					Opacity = restoring ? targetOpacity : (byte) 1;
					return false;
				} else {
					Opacity = (byte) (targetOpacity - ImageLib.ClampDouble((topInner - topBeforeMinimize) * targetOpacity / diff));
					return !(IsClosing || IsDisposed);
				}
			} else
				return !(IsClosing || IsDisposed);
		}

		private void OnMinimizeFinished() {
			animatingTopInner = false;
			if (minimizing) {
				minimizing = false;
				WindowState = FormWindowState.Minimized;
			} else if (restoring) {
				restoring = false;
				TitleBarChanged(0, 0);
			}
			OnResizeEnd(EventArgs.Empty);
			OnMinimizeAnimationFinished();
		}

		/// <summary>
		/// Renders the form and its child controls onto the specified image.
		/// </summary>
		/// <param name="bitmap">The image to draw onto.</param>
		public void DrawToBitmap(Bitmap bitmap) {
			DrawToBitmap(bitmap as Image, ClientRectangle, true);
		}

		/// <summary>
		/// Renders the form and its child controls onto the specified image.
		/// </summary>
		/// <param name="image">The image to draw onto.</param>
		public void DrawToBitmap(Image image) {
			DrawToBitmap(image, ClientRectangle, true);
		}

		/// <summary>
		/// Renders the form and its child controls onto the specified image.
		/// </summary>
		/// <param name="bitmap">The image to draw onto.</param>
		/// <param name="targetBounds">The bounds within which the form is rendered.</param>
		public new void DrawToBitmap(Bitmap bitmap, Rectangle targetBounds) {
			DrawToBitmap(bitmap as Image, targetBounds, true);
		}

		/// <summary>
		/// Renders the form and its child controls onto the specified image
		/// </summary>
		/// <param name="image">The image to draw onto</param>
		/// <param name="targetBounds">The bounds within which the form is rendered</param>
		public void DrawToBitmap(Image image, Rectangle targetBounds) {
			DrawToBitmap(image, targetBounds, true);
		}

		/// <summary>
		/// Renders the form onto the specified image
		/// </summary>
		/// <param name="bitmap">The image to draw onto</param>
		/// <param name="targetBounds">The bounds within which the form is rendered</param>
		/// <param name="drawChildren">Whether to draw the child controls</param>
		public void DrawToBitmap(Bitmap bitmap, Rectangle targetBounds, bool drawChildren) {
			DrawToBitmap(bitmap as Image, targetBounds, drawChildren);
		}

		/// <summary>
		/// Renders the form onto the specified image
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
		/// Closes the window asynchronously.
		/// </summary>
		public void CloseAsync() {
			if (Modal)
				Close();
			else {
				if (IsHandleCreated) {
					TypeOfForm.GetField("closeReason", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(this, CloseReason.UserClosing);
					NativeApi.PostMessage(Handle, WindowMessage.CLOSE, IntPtr.Zero, IntPtr.Zero);
				} else
					Dispose(DisposeOptions.FormOnly);
			}
		}

		private bool OnOpacityUpdate(AnimationInfo state) {
			if (state.IsFinished) {
				FadeState fadeState = (FadeState) state.Tag;
				if (fadeState == FadeState.FadingIn) {
					FadeState = FadeState.Normal;
					if (!ShowWithoutActivation) {
						Activate();
						ActivateBorder();
					}
					OnFadeInCompleted();
				} else if (fadeState == FadeState.FadingOut) {
					if (Thread.CurrentThread == residentThread)
						SetVisibleCore(false);
					else {
						try {
							Invoke(new InvocationData(setVisibleCore, false));
						} catch {
						}
					}
					FadeState = FadeState.Normal;
					if (IsClosing)
						CloseAsync();
				}
				return true;
			} else
				return !IsDisposed;
		}

		private bool OnBoundsUpdate(AnimationInfo state) {
			if (state == null || state.IsFinished) {
				lock (onBoundsUpdate)
					animatingBounds = false;
				TitleBarChanged(0, 0);
				if (Thread.CurrentThread == residentThread)
					OnResizeEnd(EventArgs.Empty);
				else {
					try {
						BeginInvoke(new InvocationData(callOnResizeEnd, EventArgs.Empty));
					} catch {
					}
				}
				if (isFullScreen)
					Activate();
				return true;
			} else
				return !IsDisposed;
		}

		private bool OnBorderUpdate(AnimationInfo state) {
			RedrawBorder(true);
			return !IsDisposed;
		}

		/// <summary>
		/// Called when resizing has finished.
		/// </summary>
		/// <param name="e">Ignored.</param>
		protected override void OnResizeEnd(EventArgs e) {
			base.OnResizeEnd(e);
			if (!IsGLEnabled)
				base.Invalidate(false);
		}

		/// <summary>
		/// Called when the minimize animation is completed.
		/// </summary>
		protected virtual void OnMinimizeAnimationFinished() {
		}

		private object SetVisible(object value) {
			SetVisibleCore((bool) value);
			return null;
		}

		/// <summary>
		/// Configures the visibility of the window.
		/// </summary>
		/// <param name="value">True to set the window to visible, else false.</param>
		protected override void SetVisibleCore(bool value) {
			if (Visible == value)
				return;
			if (value) {
				openForms.Add(this);
				RefreshDpi();
				if (!IsHandleCreated)
					CreateHandle();
				if (firstLoad) {
					firstLoad = false;
					ResumeLayout(false);
					if (!DesignMode) {
						if (screen == null)
							screen = Screen.FromControl(this);
						if (MdiParent == null) {
							Rectangle bounds = Screen.FromPoint(Cursor.Position).WorkingArea;
							Location = new Point(bounds.X + (bounds.Width - Width) / 2, bounds.Y + (bounds.Height - Height) / 2);
						} else {
							Size size = MdiParent.ClientSize;
							Location = new Point((size.Width - Width) / 2, (size.Height - Height) / 2);
						}
					}
				}
				if (IsMinimized) {
					base.SetVisibleCore(true);
					OnShown(EventArgs.Empty);
					if (IsMinimized && !restoring)
						Restore();
					OnFadeInCompleted();
				} else {
					if (FadeState == FadeState.FadingIn) {
						base.SetVisibleCore(true);
						OnShown(EventArgs.Empty);
					} else {
						if (EnableOpacityAnimation && AllowTransparency && !DesignMode) {
							Opacity = 1;
							FadeState = FadeState.FadingIn;
							base.SetVisibleCore(true);
							OnShown(EventArgs.Empty);
							UIAnimator.SharedAnimator.Animate(OpacityProperty, (byte) 255, 0.5, 0.08, true, onOpacityUpdate, true, ExceptionMode.Log, FadeState.FadingIn);
						} else {
							FadeState = FadeState.Normal;
							base.SetVisibleCore(true);
							OnShown(EventArgs.Empty);
							OnFadeInCompleted();
						}
					}
				}
				wasShadowEnabled = false;
				UpdateShadow();
				if (!ShowWithoutActivation) {
					Activate();
					Control defaultButton = AcceptButton as Control;
					if (defaultButton != null)
						defaultButton.Focus();
				}
				RefreshBorder();
			} else {
				if (!(DesignMode || FadeState == FadeState.FadingOut || WindowState == FormWindowState.Minimized) && EnableOpacityAnimation && AllowTransparency) {
					FadeState = FadeState.FadingOut;
					UIAnimator.SharedAnimator.Animate(OpacityProperty, (byte) 1, 0.7, 0.08, true, onOpacityUpdate, true, ExceptionMode.Log, FadeState.FadingOut);
				} else {
					FadeState = FadeState.Normal;
					base.SetVisibleCore(false);
					OnFadeOutCompleted();
					openForms.Remove(this);
				}
			}
		}

		private void TitleBarChanged(int widthDiff, int titleBarDiff) {
			int newHeight = (int) ((titleBarHeight - (titleBarPadding.Height * 2 + borderWidth)) * DpiScale.Height) - 1;
			closeSize = new Size((int) (titleBarHeight * 1.5f * DpiScale.Height) - 1, newHeight);
			maximizeButtonSize = new Size((int) (titleBarHeight * DpiScale.Height) - 1, newHeight);
			OnBorderSizeChangedInner(widthDiff, titleBarDiff);
			OnBorderLayoutChanged();
		}

		private void OnBorderSizeChangedInner(int widthDiff, int titleBarDiff) {
			if (!(widthDiff == 0 && titleBarDiff == 0)) {
				borderChanged++;
				Size += new Size((int) (widthDiff * 2 * DpiScale.Height), (int) ((widthDiff + titleBarDiff) * DpiScale.Height));
			}
			Point clientLoc = new Point(CurrentBorderWidth, CurrentTitleBarHeight);
			int borderWidth = (int) (this.borderWidth * DpiScale.Width);
			Padding = isFullScreen || !showBorder ? Padding.Empty : ((isMaximized && (animatingTopInner || IsFullyMaximized)) ? new Padding(0, clientLoc.Y, 0, 0) : new Padding(clientLoc.X, clientLoc.Y, borderWidth, borderWidth));
			RefreshBorder();
		}

		/// <summary>
		/// Called when the form DPI has been changed.
		/// </summary>
		protected virtual void OnDpiChanged() {
		}

		/// <summary>
		/// Call all the controls have the wrong border offset.
		/// </summary>
		/// <param name="adjustSize">Whether to adjust the window size compensate as well.</param>
		public void ApplyOffsetsToChildren(bool adjustSize = false) {
			Size currentOffset, targetOffset = new Size(0, CurrentTitleBarHeight);
			if (targetOffset.IsEmpty)
				return;
			if (adjustSize) {
				Size += targetOffset - SizeOffset;
				SizeOffset = targetOffset;
			}
			lock (offsetLock) {
				foreach (Control control in Controls) {
					if (OffsetTracker.ContainsKey(control)) {
						currentOffset = OffsetTracker[control];
						control.Location += targetOffset - currentOffset;
						OffsetTracker[control] = targetOffset;
					} else {
						OffsetTracker.Add(control, targetOffset);
						control.Location += targetOffset;
					}
				}
			}
		}

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		private int GetControlsLeft() {
			int newLeft = (isMaximized && (animatingTopInner || IsFullyMaximized)) ? ClientSize.Width - 1 : ClientSize.Width - (int) (this.borderWidth * DpiScale.Height);
			int count = 0;
			if (closeEnabled) {
				newLeft -= CloseBounds.Width;
				count++;
			}
			if (maximizeEnabled) {
				newLeft -= MaximizeBounds.Width;
				count++;
			}
			if (MinimizeEnabled)
				newLeft -= MinimizeBounds.Width;
			return newLeft - count;
		}

		/// <summary>
		/// Called when the window is loaded for the first time.
		/// </summary>
		/// <param name="e">Ignored.</param>
		protected override void OnLoad(EventArgs e) {
			EventHandler eventHandler = (EventHandler) Events[EventLoad];
			if (eventHandler != null)
				eventHandler(this, e);
		}

		/*private static string FilterFrequentMessages(WindowMessage message) {
			switch (message) {
				case WindowMessage.MOUSEMOVE:
				case WindowMessage.NCMOUSEMOVE:
				case WindowMessage.NCHITTEST:
				case WindowMessage.SETCURSOR:
				case WindowMessage.CTLCOLORBTN:
				case WindowMessage.CTLCOLORDLG:
				case WindowMessage.CTLCOLOREDIT:
				case WindowMessage.CTLCOLORLISTBOX:
				case WindowMessage.CTLCOLORMSGBOX:
				case WindowMessage.CTLCOLORSCROLLBAR:
				case WindowMessage.CTLCOLORSTATIC:
				case WindowMessage.ENTERIDLE:
				case WindowMessage.CANCELMODE:
				case WindowMessage.SYSTIMER:
				case WindowMessage.NULL:
					return string.Empty;
				default:
					return message.ToString();
			}
		}*/

		/// <summary>
		/// Called when the screen is pressed.
		/// </summary>
		/// <param name="touch">The touch event details.</param>
		protected virtual void OnTouchDown(TouchEventArgs touch) {
		}

		/// <summary>
		/// Called when a finger/pen is dragged across the screen.
		/// </summary>
		/// <param name="touch">The touch event details.</param>
		protected virtual void OnTouchMove(TouchEventArgs touch) {
		}

		/// <summary>
		/// Called when the screen is released.
		/// </summary>
		/// <param name="touch">The touch event details.</param>
		protected virtual void OnTouchUp(TouchEventArgs touch) {
		}

		/// <summary>
		/// Called when the display resolution changed.
		/// </summary>
		protected virtual void OnDisplaySettingsChanged() {
		}

		/// <summary>
		/// Processes the appropriate windows internal messages accordingly.
		/// </summary>
		/// <param name="m">The message to process.</param>
		protected override void WndProc(ref Message m) {
			if ((WindowMessage) m.Msg == InvokeMessage) {
				InvocationData e;
				while (!IsDisposed && callbacks.TryDequeue(out e)) {
					e.State = InvokeState.Started;
					try {
						e.AsyncState = e.Method(e.Parameter);
						e.State = InvokeState.Completed;
					} catch {
						e.State = InvokeState.Error;
						throw;
					}
				}
				return;
			} else if (!DesignMode) {
				switch ((WindowMessage) m.Msg) {
					case WindowMessage.DISPLAYCHANGE:
						screen = Screen.FromControl(this);
						RefreshDpi();
						if (isFullScreen)
							AnimateBoundsTo(screen.Bounds);
						else if (isMaximized)
							AnimateBoundsTo(WorkingArea);
						OnDisplaySettingsChanged();
						return;
					case WindowMessage.CONTEXTMENU: {
							Point curPos = Cursor.Position;
							Point client = PointToClient(curPos);
							Point inViewPort = PointFromClientToViewPort(client);
							if (ViewSize.Contains(inViewPort)) {
								Control control = GetGdiChildAtPoint(inViewPort, EligibleForMouse, ref inViewPort);
								if (control == null || control == this)
									break;
								else if (control.ContextMenuStrip != null)
									control.ContextMenuStrip.Show(curPos);
								else if (control.ContextMenu != null)
									control.ContextMenu.Show(this, client);
								else
									return;
							} else
								return;
							break;
						}
					case WindowMessage.SYSCOMMAND:
						if ((SystemCommand) m.WParam == SystemCommand.SC_MINIMIZE) {
							if (FormBorderStyle == FormBorderStyle.None) {
								Form[] ownedForms = OwnedForms;
								if (ownedForms.Length != 0) {
									for (int i = 0; i < ownedForms.Length; i++)
										NativeApi.SetWindowLong(ownedForms[i].Handle, GetWindowLongOffsets.HWNDPARENT, UIntPtr.Zero);
								}
							} else {
								m.Result = IntPtr.Zero;
								Minimize();
								return;
							}
						}
						break;
					case WindowMessage.SETICON:
						if (showBorder && ShowIcon)
							RedrawBorder(false, IconBounds);
						break;
					case WindowMessage.ERASEBKGND:
					case WindowMessage.NCHITTEST:
						m.Result = One;
						return;
					case WindowMessage.NCCALCSIZE:
					case WindowMessage.NCPAINT:
						if (FormBorderStyle == FormBorderStyle.None)
							return;
						else
							break;
					case WindowMessage.ENTERIDLE:
					case WindowMessage.NULL:
					case WindowMessage.NCLBUTTONDOWN:
						return;
					case WindowMessage.NCACTIVATE:
						if (m.WParam == One) {
							ActivateBorder();
							if (isFullScreen)
								TopMost = true;
						} else {
							DeactivateBorder();
							if (isFullScreen)
								TopMost = false;
						}
						break;
					case WindowMessage.ACTIVATE:
						if (m.WParam != IntPtr.Zero)
							ActivateBorder();
						break;
					case WindowMessage.LBUTTONDBLCLK:
						OnMouseDoubleClick(new MouseEventArgs(MouseButtons.Left, 2, (int) ((uint) m.LParam & 0xFFFF), (int) ((uint) m.LParam >> 16), 0));
						return;
					case WindowMessage.MBUTTONDBLCLK:
						OnMouseDoubleClick(new MouseEventArgs(MouseButtons.Middle, 2, (int) ((uint) m.LParam & 0xFFFF), (int) ((uint) m.LParam >> 16), 0));
						return;
					case WindowMessage.RBUTTONDBLCLK:
						OnMouseDoubleClick(new MouseEventArgs(MouseButtons.Right, 2, (int) ((uint) m.LParam & 0xFFFF), (int) ((uint) m.LParam >> 16), 0));
						return;
					case WindowMessage.XBUTTONDBLCLK:
						OnMouseDoubleClick(new MouseEventArgs(((uint) m.WParam >> 16) == 1u ? MouseButtons.XButton1 : MouseButtons.XButton2, 2, (int) ((uint) m.LParam & 0xFFFF), (int) ((uint) m.LParam >> 16), 0));
						return;
					case WindowMessage.DROPFILES:
						if (Platform.IsWindowsXPOrNewer) {
							string[] str = null;
							StringBuilder stringBuilder = new StringBuilder(260);
							uint num = NativeApi.DragQueryFile(m.WParam, 0xFFFFFFFFu, null, 0u);
							if (num > 0) {
								str = new string[num];
								for (uint i = 0; i < num; i++) {
									if (NativeApi.DragQueryFile(m.WParam, i, stringBuilder, (uint) stringBuilder.Capacity) != 0)
										str[i] = stringBuilder.ToString();
								}
							}
							NativeApi.DragFinish(m.WParam);
							Point curPos = PointFromScreenToViewPort(Cursor.Position);
							OnDragDrop(new DragEventArgs(new DataObject(DataFormats.FileDrop, str), (int) ModifierKeys, curPos.X, curPos.Y, DragDropEffects.Copy, DragDropEffects.Copy));
						}
						return;
					case WindowMessage.TOUCH:
						if (Platform.IsWindows7OrNewer) {
							int inputCount = m.WParam.ToInt32() & 0xFFFF;
							TouchInput[] inputs = new TouchInput[inputCount];
							if (NativeApi.GetTouchInputInfo(m.LParam, inputCount, inputs, TouchInput.Size)) {
								TouchInput input;
								Rectangle bounds;
								TouchEventArgs args;
								for (int i = 0; i < inputCount; i++) {
									input = inputs[i];
									if (input.ID != 0) {
										bounds = new Rectangle(PointToClient(new Point(input.X / 100, input.Y / 100)), new Size(1, 1));
										if ((input.Mask & TouchInputMask.ContactArea) == TouchInputMask.ContactArea)
											bounds.Size = new Size((int) (input.ContactWidth / 100), (int) (input.ContactHeight / 100));
										args = new TouchEventArgs(unchecked((int) input.ID), bounds,
											(input.Flags & TouchEvent.Primary) == TouchEvent.Primary, (input.Flags & TouchEvent.Palm) == TouchEvent.Palm);
										if ((input.Flags & TouchEvent.Down) == TouchEvent.Down)
											OnTouchDown(args);
										else if ((input.Flags & TouchEvent.Up) == TouchEvent.Up)
											OnTouchUp(args);
										else if ((input.Flags & TouchEvent.Move) == TouchEvent.Move)
											OnTouchMove(args);
									}
								}
								NativeApi.CloseTouchInputHandle(m.LParam);
								return;
							}
						}
						break;
					case WindowMessage.CTLINIT:
					case WindowMessage.CTLCOLOR:
					case WindowMessage.CTLCOLORBTN:
					case WindowMessage.CTLCOLORDLG:
					case WindowMessage.CTLCOLOREDIT:
					case WindowMessage.CTLCOLORLISTBOX:
					case WindowMessage.CTLCOLORMSGBOX:
					case WindowMessage.CTLCOLORSCROLLBAR:
					case WindowMessage.CTLCOLORSTATIC:
						return;
				}
			}
			base.WndProc(ref m);
		}

		private void ActivateBorder() {
			if (borderActive || !Enabled)
				return;
			borderActive = true;
			AnimateBorder();
		}

		private void DeactivateBorder() {
			if (!borderActive)
				return;
			borderActive = false;
			AnimateBorder();
		}

		private void AnimateBorder() {
			if (EnableBorderAnimation) {
				UIAnimator.SharedAnimator.Animate(BorderOpacityProperty, borderActive ? activeBorderOpacity : inactiveBorderOpacity, 0.3, 0.01, true, onBorderUpdate, false);
				UIAnimator.SharedAnimator.Animate(BorderOverlayProperty, borderActive ? Color.Empty : borderOverlay, 0.3, 0.01, true, null, false);
			} else {
				currentBorderOpacity = borderActive ? activeBorderOpacity : inactiveBorderOpacity;
				currentBorderOverlay = borderActive ? Color.Transparent : borderOverlay;
				RedrawBorder(false);
			}
		}

		/// <summary>
		/// Called when the bounds of the form is about to change.
		/// </summary>
		/// <param name="x">The new X-coordinate.</param>
		/// <param name="y">The new Y-coordinate.</param>
		/// <param name="width">The new width of the form.</param>
		/// <param name="height">The new height of the form.</param>
		/// <param name="specified">Which bounds are specified.</param>
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified) {
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
			Rectangle screenBounds;
			if (MdiParent == null)
				screenBounds = SystemInformation.VirtualScreen;
			else
				screenBounds = new Rectangle(Point.Empty, MdiParent.ClientSize);
			Rectangle bounds = Bounds;
			int borderSize = CurrentBorderWidth / 2;
			int minX = screenBounds.X + borderSize - (widthSpecified ? width : bounds.Width);
			if (x < minX)
				x = minX;
			else {
				int maxX = screenBounds.Right - borderSize;
				if (x > maxX)
					x = maxX;
			}
			int halfTitleBar = titleBarHeight / 2;
			int minY = screenBounds.Y - halfTitleBar;
			if (y < minY)
				y = minY;
			else {
				int maxY = screenBounds.Bottom - halfTitleBar;
				if (y > maxY)
					y = maxY;
			}
			if (widthSpecified && fullscreenGdiGLWorkaround && x == 0 && y == 0) {
				Size targetBounds = screen.Bounds.Size;
				if (width == targetBounds.Width && height == targetBounds.Height)
					width++;
			}
			base.SetBoundsCore(x, y, width, height, specified);
			if (!(x == bounds.X && y == bounds.Y)) {
				screen = Screen.FromControl(this);
				if ((widthSpecified && width == bounds.Width) && (heightSpecified && height == bounds.Height) || !(widthSpecified || heightSpecified))
					RefreshBorder();
			}
		}

		/// <summary>
		/// Use OnClientSizeChanged() or OnResize() instead. Called when the window client size has changed.
		/// </summary>
		protected override sealed void OnSizeChanged(EventArgs e) {
			FormWindowState state = WindowState;
			if (oldWindowState != state) {
				bool wasMinimized = oldWindowState == FormWindowState.Minimized;
				oldWindowState = state;
				OnStateChanged();
				if (wasMinimized && state != FormWindowState.Minimized)
					Restore();
			}
			base.OnSizeChanged(e);
			if (borderChanged == 0)
				OnBorderSizeChangedInner(0, 0);
			else
				borderChanged--;
			CheckBorderStyle();
			if (!AnimatingBounds)
				Invalidate(false);
		}

		private void CheckBorderStyle() {
			if (isFullScreen) {
				if (FormBorderStyle != FormBorderStyle.None) {
					FormBorderStyle = FormBorderStyle.None;
					UpdateShadow();
				}
			} else {
				if (FormBorderStyle != FormBorderStyle.None && showBorder) {
					FormBorderStyle = FormBorderStyle.None;
					UpdateShadow();
				}
				if (WindowBorder != FormBorderStyle) {
					WindowBorder = FormBorderStyle;
					UpdateShadow();
					OnBorderStyleChanged();
				}
			}
		}

		private void RefreshBorder() {
			if (showBorder && !(IsMinimized || minimizing || restoring || isFullScreen || animatingTopInner || AnimatingBounds || IsClosing)) {
				bool bordersShown = !(isMaximized && IsFullyMaximized);
				int titleBarHeight = (int) (this.titleBarHeight * DpiScale.Height);
				if (bordersShown == wereBordersShown)
					RedrawBorder(false);
				else {
					wereBordersShown = bordersShown;
					TitleBarChanged(0, 0);
				}
			}
		}

		/// <summary>
		/// Use GraphicsForm instead StyledForm to use DrawGL 
		/// </summary>
		/// <param name="location">The location to draw at</param>
		/// <param name="drawChildren">Whether to draw the child controls</param>
		public virtual void DrawGL(Point location, bool drawChildren) {
			throw new NotImplementedException("Use GraphicsForm instead StyledForm to use " + nameof(DrawGL) + ".");
		}

		/// <summary>
		/// Draws the form onto the specified canvas
		/// </summary>
		/// <param name="g">The graphics canvas to draw on</param>
		public void DrawGdi(Graphics g) {
			DrawGdi(g, Location, true);
		}

		/// <summary>
		/// Draws the form onto the specified canvas
		/// </summary>
		/// <param name="g">The graphics canvas to draw on</param>
		/// <param name="location">The location to draw the form at</param>
		/// <param name="drawChildren">Whether to draw the child controls</param>
		public void DrawGdi(Graphics g, Point location, bool drawChildren = true) {
			if (!location.IsEmpty)
				g.TranslateTransform(location.X, location.Y);
			Rectangle rect = ClientRectangle;
			OnPaintBackgroundInner(g, rect);
			OnPaintInner(g, rect);
			if (drawChildren)
				g.DrawControls(Controls, Point.Empty, rect, true);
			if (!location.IsEmpty)
				g.TranslateTransform(-location.X, -location.Y);
		}

		/// <summary>
		/// Called when the window background is painted.
		/// </summary>
		/// <param name="e">The Graphics object to use to paint on.</param>
		protected override sealed void OnPaintBackground(PaintEventArgs e) {
			if (DesignMode || !(AnimatingBounds || IsGLEnabled))
				OnPaintBackgroundInner(e.Graphics, e.ClipRectangle);
		}

		private void OnPaintBackgroundInner(Graphics g, Rectangle clippingRect) {
			g.PixelOffsetMode = PixelOffsetMode.HighQuality;
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			g.CompositingMode = CompositingMode.SourceOver;
			g.CompositingQuality = CompositingQuality.HighQuality;
			OnPaintBackground(g, clippingRect);
		}

		/// <summary>
		/// Called when the window background is painted.
		/// </summary>
		/// <param name="g">The Graphics object to use to paint on.</param>
		/// <param name="clippingRect">The clipping rectangle that was invalidated in client coordinates (ie. includes border).</param>
		protected virtual void OnPaintBackground(Graphics g, Rectangle clippingRect) {
			if (!SuppressClear)
				g.Clear(enableAeroBlur && Extensions.IsAeroEnabled ? Color.FromArgb(backcolorOpacity, BackColor) : BackColor);
			Image image = BackgroundImage;
			if (image != null)
				g.DrawImageWithLayout(image, ViewPort, BackgroundImageLayout);
		}

		/// <summary>
		/// Called when the window is drawn.
		/// </summary>
		/// <param name="e">The graphics canvas to draw on.</param>
		protected override sealed void OnPaint(PaintEventArgs e) {
			if (DesignMode || !(AnimatingBounds || IsGLEnabled))
				OnPaintInner(e.Graphics, e.ClipRectangle);
		}

		private void OnPaintInner(Graphics g, Rectangle clippingRect) {
			Point offset = ViewPortLocation;
			bool applyOffset = !offset.IsEmpty;
			if (applyOffset) {
				g.TranslateTransform(offset.X, offset.Y);
				clippingRect.X -= offset.X;
				clippingRect.Y -= offset.Y;
			}
			OnPaint(g, clippingRect);
			if (applyOffset) {
				g.TranslateTransform(-offset.X, -offset.Y);
				clippingRect.X += offset.X;
				clippingRect.Y += offset.Y;
			}
			if (!AnimatingBounds)
				DrawBorderGdi(g, clippingRect);
		}

		/// <summary>
		/// Called when the window is painted. If you are going to override this method, call this base method *AFTER* your drawing is completed.
		/// </summary>
		/// <param name="g">The Graphics object to use to paint on.</param>
		/// <param name="clippingRect">The clipping rectangle that was invalidated in viewport coordinates (ie. excludes border).</param>
		protected virtual void OnPaint(Graphics g, Rectangle clippingRect) {
			base.OnPaint(new PaintEventArgs(g, clippingRect));
		}

		/// <summary>
		/// Draws the border onto the specified canvas at {0, 0}.
		/// </summary>
		/// <param name="g">The canvas to draw onto.</param>
		/// <param name="rect">The section of the border to draw.</param>
		public virtual void DrawBorderGdi(Graphics g, Rectangle rect) {
			if (!IsBorderVisible)
				return;
			Rectangle viewport = ViewPort;
			Size clientSize = ClientSize;
			if (clientSize.Width <= 0 || clientSize.Height <= 0)
				return;
			int borderWidth = (int) (this.borderWidth * DpiScale.Height);
			if (clientSize.Width < borderWidth * 2)
				clientSize.Width = borderWidth * 2;
			bool usingAeroBlur = enableAeroBlur && Extensions.IsAeroEnabled;
			int titleBarHeight = (int) (this.titleBarHeight * DpiScale.Height);
			using (Region region = new Region(rect)) {
				region.Exclude(viewport);
				if (!region.IsVisible(rect))
					return;
				lock (BorderSyncLock) {
					Bitmap currentBorder = border;
					if (currentBorder == null)
						return;
					g.CompositingMode = CompositingMode.SourceCopy;
					g.SmoothingMode = SmoothingMode.None;
					g.PixelOffsetMode = PixelOffsetMode.None;
					using (TextureBrush brush = (usingAeroBlur || IsGLEnabled) ? new TextureBrush(currentBorder, new Rectangle(Point.Empty, currentBorder.Size), ImageLib.GetOpacityAttributes(currentBorderOpacity, WrapMode.Tile)) : new TextureBrush(border)) {
						if (!rect.Location.IsEmpty)
							brush.TranslateTransform(-rect.X, -rect.Y);
						g.FillRegion(brush, region);
					}
				}
				g.CompositingMode = CompositingMode.SourceOver;
				Rectangle iconBounds = IconBounds;
				titleLabel.Bounds = new Rectangle(iconBounds.Right, 1, GetControlsLeft() - iconBounds.Right, titleBarHeight);
				if (rect.IntersectsWith(titleLabel.Bounds))
					titleLabel.DrawGdi(g);
				Icon icon = Icon;
				if (iconBounds.Width > 0 && icon != null && ShowIcon && rect.IntersectsWith(iconBounds))
					g.DrawIcon(icon, iconBounds);
				StyleRenderer renderer;
				bool enabled = Enabled;
				if (closeEnabled) {
					Rectangle closeBounds = CloseBounds;
					if (rect.IntersectsWith(closeBounds)) {
						int size = (int) (closeBounds.Height * 0.45F);
						Rectangle bounds = new Rectangle(closeBounds.X + (closeBounds.Width - size) / 2 + 1, closeBounds.Y + (closeBounds.Height - size) / 2 + 1, size, size);
						renderer = closeButtonRenderer;
						if (renderer == null)
							return;
						renderer.Enabled = enabled;
						renderer.RenderBackground(g, closeBounds);
						g.DrawLine(XColor, bounds.Location, new PointF(bounds.Right, bounds.Bottom));
						g.DrawLine(XColor, bounds.X, bounds.Bottom, bounds.Right, bounds.Y);
					}
				}
				bool maximized = isMaximized && (animatingTopInner || IsFullyMaximized);
				if (maximizeEnabled) {
					Rectangle maximizeBounds = MaximizeBounds;
					if (rect.IntersectsWith(maximizeBounds)) {
						int bounds = (int) (maximizeBounds.Height * 0.45F);
						Point location = new Point(maximizeBounds.X + (maximizeBounds.Width - bounds) / 2, maximizeBounds.Y + (maximizeBounds.Height + 1 - bounds) / 2);
						renderer = maximizeButtonRenderer;
						if (renderer == null)
							return;
						renderer.Enabled = enabled;
						renderer.RenderBackground(g, maximizeBounds);
						g.SmoothingMode = SmoothingMode.None;
						g.PixelOffsetMode = PixelOffsetMode.None;
						if (maximized) {
							int difference = (int) (maximizeBounds.Height * 0.14F);
							int size = bounds - difference;
							int differentX = location.X + difference;
							int differentY = location.Y + difference;
							int boundingX = location.X + bounds;
							int boundingY = location.Y + bounds;
							int boundingYWODiff = boundingY - difference;
							g.DrawRectangle(MaxSymbolColor, location.X, differentY, size, size);
							g.DrawLine(MaxSymbolColor, differentX, location.Y, differentX, differentY);
							g.DrawLine(MaxSymbolColor, differentX, location.Y, boundingX, location.Y);
							g.DrawLine(MaxSymbolColor, boundingX, location.Y, boundingX, boundingYWODiff);
							g.DrawLine(MaxSymbolColor, boundingX, boundingYWODiff, boundingX - difference, boundingYWODiff);
						} else
							g.DrawRectangle(MaxSymbolColor, location.X, location.Y, bounds, bounds);
					}
				}
				if (MinimizeEnabled) {
					Rectangle minimizeBounds = MinimizeBounds;
					if (rect.IntersectsWith(minimizeBounds)) {
						const int sizeY = 1;
						int sizeX = minimizeBounds.Height / 2;
						renderer = minimizeButtonRenderer;
						if (renderer == null)
							return;
						renderer.Enabled = enabled;
						renderer.RenderBackground(g, minimizeBounds);
						g.SmoothingMode = SmoothingMode.None;
						g.PixelOffsetMode = PixelOffsetMode.None;
						g.FillRectangle(MinimizeFill, minimizeBounds.X + (minimizeBounds.Width - sizeX) / 2 + 1, minimizeBounds.Y + (int) ((minimizeBounds.Height + 1) * 0.7F) - sizeY, sizeX, sizeY);
					}
				}
				if (currentBorderOverlay.A != 0) {
					using (SolidBrush borderOverlay = new SolidBrush(currentBorderOverlay))
						g.FillRectangle(borderOverlay, ControlBoxBounds);
				}
				g.SmoothingMode = SmoothingMode.None;
				g.PixelOffsetMode = PixelOffsetMode.None;
				Rectangle workingArea = WorkingArea;
				Rectangle windowBounds = Bounds;
				bool fullWidth = windowBounds.Width == workingArea.Width && windowBounds.X == workingArea.X;
				bool fullHeight = windowBounds.Height == workingArea.Height && windowBounds.Y == workingArea.Y;
				if (!(outlineColor.Color.A == 0 || (fullWidth && fullHeight))) {
					if (!fullWidth) {
						g.DrawLine(outlineColor, 0, 0, 0, clientSize.Height);
						g.DrawLine(outlineColor, clientSize.Width - 1, 0, clientSize.Width - 1, clientSize.Height);
					}
					if (!fullHeight) {
						g.DrawLine(outlineColor, 0, 0, clientSize.Width, 0);
						g.DrawLine(outlineColor, 0, clientSize.Height - 1, clientSize.Width - 1, clientSize.Height - 1);
					}
				}
				if (inlineColor.Color.A != 0) {
					if (maximized)
						g.DrawLine(inlineColor, 0, titleBarHeight - 1, clientSize.Width - 1, titleBarHeight - 1);
					else {
						g.DrawLine(inlineColor, borderWidth - 1, titleBarHeight - 1, clientSize.Width - borderWidth, titleBarHeight - 1);
						g.DrawLine(inlineColor, borderWidth - 1, clientSize.Height - borderWidth, clientSize.Width - borderWidth, clientSize.Height - borderWidth);
						g.DrawLine(inlineColor, borderWidth - 1, titleBarHeight, borderWidth - 1, clientSize.Height - borderWidth);
						g.DrawLine(inlineColor, clientSize.Width - borderWidth, titleBarHeight, clientSize.Width - borderWidth, clientSize.Height - borderWidth);
					}
				}
			}
		}

		/// <summary>
		/// Called when the window state is actually changed (for example, minimize is fully completed, the window is maximized or fullscreen...).
		/// </summary>
		protected virtual void OnStateChanged() {
		}

		/// <summary>
		/// Called whenever the window is minimized or restored.
		/// </summary>
		protected virtual void OnMinimizeChanged() {
		}

		/// <summary>
		/// Called whenever the window is maximized or unmaximized.
		/// </summary>
		protected virtual void OnMaximizeChanged() {
		}

		/// <summary>
		/// Called when the FPS count is updated.
		/// </summary>
		protected virtual void OnFramesPerSecondUpdated() {
		}

		/// <summary>
		/// Called when the window fade-in is completed.
		/// </summary>
		protected virtual void OnFadeInCompleted() {
		}

		/// <summary>
		/// Called when the ShowBorder value has changed.
		/// </summary>
		protected virtual void OnShowBorderChanged() {
		}

		/// <summary>
		/// Called when the border layout is adjusted.
		/// </summary>
		protected virtual void OnBorderLayoutChanged() {
		}

		/// <summary>
		/// Called when the window enters full screen mode.
		/// </summary>
		protected virtual void OnEnterFullScreen() {
		}

		/// <summary>
		/// Called when the window leaves full screen mode.
		/// </summary>
		protected virtual void OnLeaveFullScreen() {
		}

		/// <summary>
		/// Called when FormBorderStyle has changed.
		/// </summary>
		protected virtual void OnBorderStyleChanged() {
		}

		/// <summary>
		/// Called when the fade out is completed.
		/// </summary>
		protected virtual void OnFadeOutCompleted() {
		}

		/// <summary>
		/// Called when the form is enabled or disabled.
		/// </summary>
		protected override void OnEnabledChanged(EventArgs e) {
			EventHandler item = Events[EventEnabled] as EventHandler;
			if (item != null)
				item(this, e);
			Control.ControlCollection obj = Controls;
			for (int i = 0; i < obj.Count; i++)
				ParentEnabledInvoke(obj[i], e);
			if (IsHandleCreated && !isFullScreen && showBorder && ActiveForm == this) {
				if (Enabled)
					ActivateBorder();
				else
					DeactivateBorder();
			}
		}

		/// <summary>
		/// Will be called when a close message is sent to the form. Return true to close, else false to cancel and ignore closure.
		/// </summary>
		/// <param name="reason">The event that caused a close message to be sent to the form.</param>
		protected virtual bool OnQueryClose(CloseReason reason) {
			return true;
		}

		/// <summary>
		/// Don't use this, override OnQueryClose() instead.
		/// </summary>
		/// <param name="e">The cancel event arguments.</param>
		protected override sealed void OnClosing(CancelEventArgs e) {
		}

		/// <summary>
		/// Called when the window is now starting to close.
		/// </summary>
		protected virtual void OnClosing() {
		}

		/// <summary>
		/// Don't use this, override OnQueryClose() instead.
		/// </summary>
		/// <param name="e">The cancel event arguments.</param>
		protected override sealed void OnFormClosing(FormClosingEventArgs e) {
			DialogResult currentResult = DialogResult;
			if (currentResult == DialogResult.Cancel || currentResult == DialogResult.None)
				DialogResult = dialogResult;
			else
				dialogResult = currentResult;
			if (FadeState == FadeState.FadingOut) {
				if (!IsClosing) {
					IsClosing = true;
					OnClosing();
				}
				e.Cancel = true;
			} else if (IsClosing || !Visible) {
				SetVisibleCore(false);
				FadeState = FadeState.Normal;
				base.OnFormClosing(e);
			} else if (OnQueryClose(e.CloseReason)) {
				Capture = false;
				IsClosing = true;
				OnClosing();
				SetVisibleCore(false);
				if (EnableOpacityAnimation && AllowTransparency && !DesignMode && WindowState != FormWindowState.Minimized)
					e.Cancel = true;
				else {
					FadeState = FadeState.Normal;
					base.OnFormClosing(e);
				}
			} else
				e.Cancel = true;
		}

		/// <summary>
		/// Gets a string that describes this instance.
		/// </summary>
		public override string ToString() {
			return nameof(StyledForm) + ": " + Text;
		}

		/// <summary>
		/// Override OnFormClosed() instead.
		/// </summary>
		protected override sealed void OnClosed(EventArgs e) {
		}

		/// <summary>
		/// Called when the window is closed.
		/// </summary>
		/// <param name="e">The reason the window is closed.</param>
		protected override void OnFormClosed(FormClosedEventArgs e) {
			UIScaler.RemoveFromScaler(this);
			IsClosing = false;
			CaptureControl = null;
			MouseInsideControl = null;
			base.OnFormClosed(e);
			lock (openForms.SyncRoot) {
				if (openForms.Count != 0)
					openForms[openForms.Count - 1].Activate();
			}
			Dispose(FullDisposeOnClose ? DisposeOptions.FullDisposal : DisposeOptions.FormOnly);
		}

		/// <summary>
		/// Called when the form is about to dispose its resources.
		/// If disposeManaged is false, you probably shouldn't dispose anything.
		/// If the return value is false, disposal is cancelled.
		/// There is no guarantee that this function is called (only) once for each form.
		/// </summary>
		/// <param name="e">Contains the current dispose mode. Setting the DisposeMode flag in DisposeFormEventArgs can override the current dispose mode.</param>
		protected virtual void OnDisposing(DisposeFormEventArgs e) {
		}

		/// <summary>
		/// Called when the form is disposed. If disposeManaged is false, you probably shouldn't dispose anything.
		/// There is no guarantee that this function is called (only) once for each form.
		/// </summary>
		/// <param name="e">Contains the current dispose mode.</param>
		protected virtual void OnDisposed(DisposeFormEventArgs e) {
		}

		/// <summary>
		/// Disposes of the form (disposes of managed resources as well).
		/// </summary>
		~StyledForm() {
			if (finalized)
				return;
			finalized = true;
			if (disposed != DisposeOptions.FullDisposal)
				Dispose(DisposeOptions.FullDisposal);
		}

		/// <summary>
		/// Disposes of the form (disposes of managed resources as well).
		/// </summary>
		public new void Dispose() {
			Dispose(DisposeOptions.FullDisposal);
		}

		/// <summary>
		/// Called when the form is being disposed. Returns false if the disposal is cancelled by OnDisposing(), else true.
		/// </summary>
		/// <param name="options">Whether to dispose of managed resources as well.</param>
		public bool Dispose(DisposeOptions options) {
			bool managedOnly, fullDisposal;
			lock (DisposeSyncRoot) {
				DisposeOptions isDisposed = disposed;
				if ((int) isDisposed >= (int) options)
					return true;
				DisposeFormEventArgs param = new DisposeFormEventArgs(options);
				OnDisposing(param);
				options = param.DisposeMode;
				if (options == DisposeOptions.None)
					return false;
				fullDisposal = options == DisposeOptions.FullDisposal;
				managedOnly = isDisposed == DisposeOptions.FormOnly && fullDisposal;
				disposed = options;
			}
			try {
				UIScaler.RemoveFromScaler(this);
				MouseListener.CursorMove -= globalCursorMove;
				MouseListener.MouseUp -= globalMouseUp;
				SetCalledClosing(this, false);
				SetCalledMakeVisible(this, false);
				SetCalledOnLoad(this, false);
				SetCalledCreateControl(this, false);
				if (fullDisposal) {
					using (TimedLock timeLock = new TimedLock(oldControlSyncRoot, 2000)) {
						Control ctrl;
						for (int i = 0; i < oldControls.Count; i++) {
							ctrl = oldControls[i];
							if (!(ctrl == null || ctrl.IsDisposed))
								ctrl.Dispose();
						}
						oldControls.Clear();
					}
					MinimizeFill.DisposeSafe();
					MinimizeFill = null;
					lock (BorderSyncLock) {
						border.DisposeSafe();
						border = null;
					}
					XColor.DisposeSafe();
					outlineColor.DisposeSafe();
					inlineColor.DisposeSafe();
					if (titleLabel != null) {
						if (!titleLabel.IsDisposed)
							titleLabel.Dispose();
						titleLabel = null;
					}
					if (closeButtonRenderer != null) {
						closeButtonRenderer.Dispose();
						closeButtonRenderer = null;
					}
					if (maximizeButtonRenderer != null) {
						maximizeButtonRenderer.Dispose();
						maximizeButtonRenderer = null;
					}
					if (minimizeButtonRenderer != null) {
						minimizeButtonRenderer.Dispose();
						minimizeButtonRenderer = null;
					}
					Events.Dispose();
				} else {
					using (TimedLock timeLock = new TimedLock(oldControlSyncRoot, 2000)) {
						Control ctrl;
						for (int i = 0; i < Controls.Count; i++) {
							ctrl = Controls[i];
							if (ctrl != null)
								oldControls.Add(ctrl);
						}
						Controls.Clear();
					}
				}
				return true;
			} finally {
				if (disposed == DisposeOptions.FullDisposal)
					GC.SuppressFinalize(this);
				else if (!finalized)
					GC.ReRegisterForFinalize(this);
				if (!managedOnly)
					Dispose(fullDisposal);
			}
		}

		/// <summary>
		/// Please override OnDisposing() instead. If not looking to override, please use Dispose(DisposeOptions options) instead.
		/// </summary>
		/// <param name="disposing">Whether to dispose of managed resources as well.</param>
		protected override sealed void Dispose(bool disposing) {
			if (disposing) {
				StackTrace stackTrace = new StackTrace(); //an ugly workaround
				MethodBase caller;
				StackFrame frame;
				for (int i = 1; i <= 3; i++) {
					frame = stackTrace.GetFrame(i);
					if (frame == null)
						break;
					caller = frame.GetMethod();
					if (caller.Name == "WmClose" && caller.DeclaringType == TypeOfForm) {
						disposing = false;
						break;
					}
				}
			}
			DisposeOptions mode = disposing ? DisposeOptions.FullDisposal : DisposeOptions.FormOnly;
			if (Dispose(mode) && !base.IsDisposed) {
				using (TimedLock timeLock = new TimedLock(this, 3000)) {
					try {
						base.Dispose(disposing);
					} catch {
					}
					this.SetState(32, false);
					this.SetState(262144, false);
				}
				OnDisposed(new DisposeFormEventArgs(mode));
			}
		}
	}

	/// <summary>
	/// Represents the styled form state.
	/// </summary>
	public enum FadeState {
		/// <summary>
		/// No fading animation is taking place.
		/// </summary>
		Normal = 0,
		/// <summary>
		/// The window is being faded in.
		/// </summary>
		FadingIn,
		/// <summary>
		/// The window is being faded out.
		/// </summary>
		FadingOut
	}
}