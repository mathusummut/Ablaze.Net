using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System.Windows.Forms {
	/// <summary>
	/// Renders controls in robust, highly customizable style.
	/// </summary>
	public class StyleRenderer : ToolStripProfessionalRenderer, ICloneable, IDisposable {
		/// <summary>
		/// The control paint event key.
		/// </summary>
		public static readonly object PaintEventKey = typeof(Control).GetField("EventPaint", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
		/// <summary>
		/// Fired when the colors are refreshed and animations are started if any relevant colors have changed.
		/// </summary>
		[Description("Fired when the colors are refreshed and animations are started if any relevant colors have changed.")]
		public event Action CheckColors;
		private WaitCallback checkColors;
		private UIAnimationHandler functionToCallOnRefresh;
		private FieldOrProperty currentBackgroundTopField, currentBackgroundBottomField, currentCornerRadiusField, currentBorderField,
			currentInnerBorderColorField, currentInnerBorderWidthField, currentCheckmarkColorField, currentCheckmarkOpacityMultiplierField;
		private Color currentBorder = Color.FromArgb(200, 200, 200); //Color.FromArgb(255, 65, 65, 65)
		private Color currentBackgroundTop = Color.FromArgb(245, 245, 245);
		private Color currentBackgroundBottom = Color.FromArgb(220, 220, 220);
		private Color currentInnerBorderColor = Color.Transparent;
		private Color currentCheckmarkColor = Color.Transparent;
		private float currentInnerBorderWidth = 1f;
		private float currentCornerRadius;
		private float currentCheckmarkOpacityMultiplier;
		private Color border = Color.FromArgb(200, 200, 200); //Color.FromArgb(255, 65, 65, 65)
		private Color normalBackgroundTop = Color.FromArgb(245, 245, 245);
		private Color normalBackgroundBottom = Color.FromArgb(220, 220, 220);
		private Color hoverBackgroundTop = Color.FromArgb(245, 250, 250);
		private Color hoverBackgroundBottom = Color.FromArgb(192, 227, 227);
		private Color pressedBackgroundTop = Color.FromArgb(184, 220, 220);
		private Color pressedBackgroundBottom = Color.FromArgb(224, 248, 248);
		private Color normalInnerBorderColor = Color.Transparent;
		private Color hoverInnerBorderColor = Color.FromArgb(177, 240, 245);
		private Color checkmarkColor = Color.FromArgb(120, 137, 170);
		private float normalInnerBorderWidth = 1f;
		private float hoverInnerBorderWidth = 1f;
		private float roundCornerRadius;
		private double animationSpeed = 0.3;
		private double linearSpeed = 2.0;
		private bool invert, mouseHovering, pressed, suppressFunctionCallOnRefresh, suppressColorChecking, dirty, enabled = true;
		private PixelOffsetMode pixelAlignment = PixelOffsetMode.HighQuality;
		private CheckState checkState;
		private Blend backgroundBlend = new Blend(4) {
			Positions = new float[] { 0f, 0.5f, 0.5f, 1f },
			Factors = new float[] { 0f, 0.2f, 1f, 0.3f }
		};

		private UIAnimationHandler Callback {
			get {
				return suppressFunctionCallOnRefresh ? null : functionToCallOnRefresh;
			}
		}

		/// <summary>
		/// Gets or sets whether to suppress calling the refresh function.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool SuppressFunctionCallOnRefresh {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return suppressFunctionCallOnRefresh;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				if (value == suppressFunctionCallOnRefresh)
					return;
				suppressFunctionCallOnRefresh = value;
				if (!(value || functionToCallOnRefresh == null))
					functionToCallOnRefresh(null);
			}
		}

		/// <summary>
		/// Gets or sets whether to suppress color checking.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool SuppressColorChecking {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return suppressColorChecking;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				if (value == suppressColorChecking)
					return;
				suppressColorChecking = value;
				if (!value && dirty)
					ThreadPool.UnsafeQueueUserWorkItem(checkColors, null);
			}
		}

		/// <summary>
		/// Gets or sets the function to call on refresh (usually one that repaints the control).
		/// THE ANIMATION PARAMETER CAN BE NULL!
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public UIAnimationHandler FunctionToCallOnRefresh {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return functionToCallOnRefresh;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				if (value == functionToCallOnRefresh)
					return;
				functionToCallOnRefresh = value;
				if (!(suppressFunctionCallOnRefresh || value == null))
					value(null);
			}
		}

		/// <summary>
		/// Gets the current checkmark opacity multiplier at the moment.
		/// </summary>
		[Browsable(false)]
		public float CurrentCheckmarkOpacityMultiplier {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return currentCheckmarkOpacityMultiplier;
			}
		}

		/// <summary>
		/// Gets the current border color at the moment.
		/// </summary>
		[Browsable(false)]
		public Color CurrentBorder {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return currentBorder;
			}
		}

		/// <summary>
		/// Gets the current background top color at the moment.
		/// </summary>
		[Browsable(false)]
		public Color CurrentBackgroundTop {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return currentBackgroundTop;
			}
		}

		/// <summary>
		/// Gets the current background bottom color at the moment.
		/// </summary>
		[Browsable(false)]
		public Color CurrentBackgroundBottom {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return currentBackgroundBottom;
			}
		}

		/// <summary>
		/// Gets the current inner border color at the moment.
		/// </summary>
		[Browsable(false)]
		public Color CurrentInnerBorderColor {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return currentInnerBorderColor;
			}
		}

		/// <summary>
		/// Gets the current checkmark color at the moment.
		/// </summary>
		[Browsable(false)]
		public Color CurrentCheckmarkColor {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return currentCheckmarkColor;
			}
		}

		/// <summary>
		/// Gets the current inner border width at the moment.
		/// </summary>
		[Browsable(false)]
		public float CurrentInnerBorderWidth {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return currentInnerBorderWidth;
			}
		}

		/// <summary>
		/// Gets the current corner radius at the moment.
		/// </summary>
		[Browsable(false)]
		public float CurrentCornerRadius {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return currentCornerRadius;
			}
		}

		/// <summary>
		/// Gets or sets the animator for this instance. If null or disposed, the renderer will use an animator that is shared between all renderers.
		/// Null is the default. A shared renderer is recommended for UI.
		/// </summary>
		[Browsable(false)]
		public UIAnimator InstanceAnimator {
			get;
			set;
		}

		private UIAnimator Animator {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return InstanceAnimator == null || InstanceAnimator.IsDisposed ? UIAnimator.SharedAnimator : InstanceAnimator;
			}
		}

		/// <summary>
		/// Gets or sets whether to invert the top and bottom colors.
		/// </summary>
		[Description("Gets or sets whether to invert the top and bottom colors.")]
		[DefaultValue(false)]
		public bool Invert {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return invert;
			}
			set {
				if (value == invert)
					return;
				invert = value;
				if (currentBackgroundBottom == currentBackgroundTop)
					return;
				object top = Animator.GetTarget(currentBackgroundTopField);
				object bottom = Animator.GetTarget(currentBackgroundBottomField);
				Color oldBackgroundTop = currentBackgroundTop;
				UIAnimator.SharedAnimator.Animate(currentBackgroundTopField, bottom == null ? currentBackgroundBottom : bottom, animationSpeed, linearSpeed, true, Callback, false);
				UIAnimator.SharedAnimator.Animate(currentBackgroundBottomField, top == null ? oldBackgroundTop : top, animationSpeed, linearSpeed, true, Callback, false);
			}
		}

		/// <summary>
		/// Gets or sets whether the control appears enabled.
		/// </summary>
		[Description("Gets or sets whether the control appears enabled.")]
		[DefaultValue(true)]
		public bool Enabled {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return enabled;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				if (value == enabled)
					return;
				enabled = value;
				ThreadPool.UnsafeQueueUserWorkItem(checkColors, null);
			}
		}

		/// <summary>
		/// Gets or sets the pixel alignment mode.
		/// </summary>
		[Description("Gets or sets the pixel alignment mode.")]
		[DefaultValue(2)]
		public PixelOffsetMode PixelAlignment {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return pixelAlignment;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				if (value == pixelAlignment)
					return;
				pixelAlignment = value;
				if (!(suppressFunctionCallOnRefresh || functionToCallOnRefresh == null))
					functionToCallOnRefresh(null);
			}
		}

		/// <summary>
		/// Gets or sets the animation speed multiplier [0, 1] (ie. the distance between the current value and the target value is multiplied by this value).
		/// </summary>
		[Description("Gets or sets the animation speed multiplier [0, 1] (ie. the distance between the current value and the target value is multiplied by this value).")]
		[DefaultValue(0.3)]
		public double AnimationSpeed {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return animationSpeed;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				if (value < 0.01)
					value = 0.01;
				if (value > 1.0)
					value = 1.0;
				animationSpeed = value;
			}
		}

		/// <summary>
		/// Gets or sets the linear speed that is there to counteract the exponential infinity that one gets when dividing the distance between the current value and the target value with the animation speed.
		/// </summary>
		[Description("Gets or sets the linear speed that is there to counteract the exponential infinity that one gets when dividing the distance between the current value and the target value with the animation speed.")]
		[DefaultValue(2.0)]
		public double LinearSpeed {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return linearSpeed;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				linearSpeed = Math.Abs(value);
			}
		}

		/// <summary>
		/// Gets or sets the blending style used to draw the background.
		/// </summary>
		[Description("Gets or sets the blending style used to draw the background.")]
		public Blend BackgroundBlend {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return backgroundBlend;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				if (value == backgroundBlend)
					return;
				backgroundBlend = value;
				if (!(suppressFunctionCallOnRefresh || functionToCallOnRefresh == null))
					functionToCallOnRefresh(null);
			}
		}

		/// <summary>
		/// Gets or sets the checkmark state.
		/// </summary>
		[Description("Gets or sets the checkmark state.")]
		[DefaultValue(0)]
		public CheckState CheckState {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return checkState;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				if (value == checkState)
					return;
				checkState = value;
				ThreadPool.UnsafeQueueUserWorkItem(checkColors, null);
			}
		}

		/// <summary>
		/// Gets or sets the checkmark color.
		/// </summary>
		[Description("Gets or sets the checkmark color.")]
		[DefaultValue(typeof(Color), "0x7889AA")]
		public Color CheckmarkColor {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return checkmarkColor;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				if (value == checkmarkColor)
					return;
				checkmarkColor = value;
				ThreadPool.UnsafeQueueUserWorkItem(checkColors, null);
			}
		}

		/// <summary>
		/// Gets or sets the normal background top gradient.
		/// </summary>
		[Description("Gets or sets the normal background top gradient.")]
		[DefaultValue(typeof(Color), "0xF5F5F5")]
		public Color NormalBackgroundTop {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return normalBackgroundTop;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				if (value == normalBackgroundTop)
					return;
				normalBackgroundTop = value;
				ThreadPool.UnsafeQueueUserWorkItem(checkColors, null);
			}
		}

		/// <summary>
		/// Gets or sets the normal background bottom gradient.
		/// </summary>
		[DefaultValue(typeof(Color), "0xDCDCDC")]
		[Description("Gets or sets the normal background bottom gradient.")]
		public Color NormalBackgroundBottom {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return normalBackgroundBottom;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				if (value == normalBackgroundBottom)
					return;
				normalBackgroundBottom = value;
				ThreadPool.UnsafeQueueUserWorkItem(checkColors, null);
			}
		}

		/// <summary>
		/// Gets or sets the hover background top gradient.
		/// </summary>
		[Description("Gets or sets the hover background top gradient.")]
		[DefaultValue(typeof(Color), "0xFFF9DA")]
		public Color HoverBackgroundTop {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return hoverBackgroundTop;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				if (value == hoverBackgroundTop)
					return;
				hoverBackgroundTop = value;
				ThreadPool.UnsafeQueueUserWorkItem(checkColors, null);
			}
		}

		/// <summary>
		/// Gets or sets the hover background bottom gradient.
		/// </summary>
		[Description("Gets or sets the hover background bottom gradient.")]
		[DefaultValue(typeof(Color), "0xEDBD3E")]
		public Color HoverBackgroundBottom {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return hoverBackgroundBottom;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				if (value == hoverBackgroundBottom)
					return;
				hoverBackgroundBottom = value;
				ThreadPool.UnsafeQueueUserWorkItem(checkColors, null);
			}
		}

		/// <summary>
		/// Gets or sets the pressed background top gradient.
		/// </summary>
		[Description("Gets or sets the pressed background top gradient.")]
		[DefaultValue(typeof(Color), "0xF5CF39")]
		public Color PressedBackgroundTop {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return pressedBackgroundTop;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				if (value == pressedBackgroundTop)
					return;
				pressedBackgroundTop = value;
				ThreadPool.UnsafeQueueUserWorkItem(checkColors, null);
			}
		}

		/// <summary>
		/// Gets or sets the pressed background bottom gradient.
		/// </summary>
		[Description("Gets or sets the pressed background bottom gradient.")]
		[DefaultValue(typeof(Color), "0xF5E17C")]
		public Color PressedBackgroundBottom {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return pressedBackgroundBottom;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				if (value == pressedBackgroundBottom)
					return;
				pressedBackgroundBottom = value;
				ThreadPool.UnsafeQueueUserWorkItem(checkColors, null);
			}
		}

		/// <summary>
		/// Gets or sets the normal inner border color.
		/// </summary>
		[Description("Gets or sets the normal inner border color.")]
		[DefaultValue(typeof(Color), "0x00000000")]
		public Color NormalInnerBorderColor {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return normalInnerBorderColor;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				if (value == normalInnerBorderColor)
					return;
				normalInnerBorderColor = value;
				ThreadPool.UnsafeQueueUserWorkItem(checkColors, null);
			}
		}

		/// <summary>
		/// Gets or sets the hover inner border color.
		/// </summary>
		[Description("Gets or sets the normal inner border color.")]
		[DefaultValue(typeof(Color), "0xF5CF39")]
		public Color HoverInnerBorderColor {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return hoverInnerBorderColor;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				if (value == hoverInnerBorderColor)
					return;
				hoverInnerBorderColor = value;
				ThreadPool.UnsafeQueueUserWorkItem(checkColors, null);
			}
		}

		/// <summary>
		/// Gets or sets the normal inner border width.
		/// </summary>
		[Description("Gets or sets the normal inner border width.")]
		[DefaultValue(1f)]
		public float NormalInnerBorderWidth {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return normalInnerBorderWidth;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				if (value == normalInnerBorderWidth)
					return;
				normalInnerBorderWidth = value;
				ThreadPool.UnsafeQueueUserWorkItem(checkColors, null);
			}
		}

		/// <summary>
		/// Gets or sets the hover inner border width.
		/// </summary>
		[Description("Gets or sets the hover inner border width.")]
		[DefaultValue(1f)]
		public float HoverInnerBorderWidth {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return hoverInnerBorderWidth;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				if (value == hoverInnerBorderWidth)
					return;
				hoverInnerBorderWidth = value;
				ThreadPool.UnsafeQueueUserWorkItem(checkColors, null);
			}
		}

		/// <summary>
		/// Gets or sets the radius of the rounded corners.
		/// </summary>
		[Description("Gets or sets the radius of the rounded corners.")]
		[DefaultValue(1f)]
		public float RoundCornerRadius {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return roundCornerRadius;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				if (value == roundCornerRadius)
					return;
				roundCornerRadius = value;
				UIAnimator.SharedAnimator.Animate(currentCornerRadiusField, roundCornerRadius, animationSpeed, linearSpeed * 0.01, true, Callback, false);
			}
		}

		/// <summary>
		/// Gets or sets the border color.
		/// </summary>
		[Description("Gets or sets the border color.")]
		[DefaultValue(typeof(Color), "0xE1DBBC")]
		public Color Border {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return border;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				if (value == border)
					return;
				border = value;
				UIAnimator.SharedAnimator.Animate(currentBorderField, border, animationSpeed, linearSpeed, true, Callback, false);
			}
		}

		/// <summary>
		/// Gets or sets whether the mouse is hovering over the button.
		/// </summary>
		[Description("Gets or sets whether the mouse is hovering over the button.")]
		[DefaultValue(false)]
		public bool MouseHovering {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return mouseHovering;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				if (value == mouseHovering)
					return;
				mouseHovering = value;
				ThreadPool.UnsafeQueueUserWorkItem(checkColors, null);
			}
		}

		/// <summary>
		/// Gets or sets whether the left mouse button is currently pressed over the control.
		/// </summary>
		[Description("Gets or sets whether the left mouse button is currently pressed over the control.")]
		[DefaultValue(false)]
		public bool Pressed {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return pressed;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				if (value == pressed)
					return;
				pressed = value;
				ThreadPool.UnsafeQueueUserWorkItem(checkColors, null);
			}
		}

		/// <summary>
		/// Initializes a new StyleRenderer using the default shared animator.
		/// </summary>
		public StyleRenderer() {
			LoadFieldInfo();
			RoundedEdges = true;
		}

		/// <summary>
		/// Initializes a new StyleRenderer using the specified function as default to handle control invalidation.
		/// </summary>
		/// <param name="defaultFunctionToInvalidateControlOnRefresh">The animation parameter can be null!</param>
		public StyleRenderer(UIAnimationHandler defaultFunctionToInvalidateControlOnRefresh)
			: this() {
			FunctionToCallOnRefresh = defaultFunctionToInvalidateControlOnRefresh;
		}

		/// <summary>
		/// Initializes a new StyleRenderer with the specified color table.
		/// </summary>
		/// <param name="colorTable">The color table to use.</param>
		/// <param name="defaultFunctionToInvalidateControlOnRefresh">The animation parameter can be null!</param>
		public StyleRenderer(ProfessionalColorTable colorTable, UIAnimationHandler defaultFunctionToInvalidateControlOnRefresh)
			: base(colorTable) {
			LoadFieldInfo();
			RoundedEdges = true;
			FunctionToCallOnRefresh = defaultFunctionToInvalidateControlOnRefresh;
		}

		/// <summary>
		/// Initializes a new StyleRenderer.
		/// </summary>
		/// <param name="renderer">The renderer to obtain the settings from (can be null).</param>
		public StyleRenderer(StyleRenderer renderer) : base(renderer == null ? new ProfessionalColorTable() : renderer.ColorTable) {
			LoadFieldInfo();
			CopyConfigFrom(renderer);
		}

		private void LoadFieldInfo() {
			checkColors = CallCheckColors;
			currentBackgroundTopField = new FieldOrProperty(nameof(currentBackgroundTop), this);
			currentBackgroundBottomField = new FieldOrProperty(nameof(currentBackgroundBottom), this);
			currentCornerRadiusField = new FieldOrProperty(nameof(currentCornerRadius), this);
			currentBorderField = new FieldOrProperty(nameof(currentBorder), this);
			currentInnerBorderColorField = new FieldOrProperty(nameof(currentInnerBorderColor), this);
			currentInnerBorderWidthField = new FieldOrProperty(nameof(currentInnerBorderWidth), this);
			currentCheckmarkColorField = new FieldOrProperty(nameof(currentCheckmarkColor), this);
			currentCheckmarkOpacityMultiplierField = new FieldOrProperty(nameof(currentCheckmarkOpacityMultiplier), this);
		}

		/// <summary>
		/// Copies the settings from the specified instance into this instance.
		/// </summary>
		/// <param name="renderer">The renderer to copy configuration from (if null then nothing is performed).</param>
		public void CopyConfigFrom(StyleRenderer renderer) {
			if (renderer == null)
				return;
			RoundedEdges = renderer.RoundedEdges;
			currentBorder = renderer.currentBorder;
			currentBackgroundTop = renderer.currentBackgroundTop;
			currentBackgroundBottom = renderer.currentBackgroundBottom;
			currentInnerBorderColor = renderer.currentInnerBorderColor;
			currentCheckmarkColor = renderer.currentCheckmarkColor;
			currentInnerBorderWidth = renderer.currentInnerBorderWidth;
			currentCheckmarkOpacityMultiplier = renderer.currentCheckmarkOpacityMultiplier;
			currentCornerRadius = renderer.currentCornerRadius;
			enabled = renderer.enabled;
			Border = renderer.Border;
			normalBackgroundTop = renderer.normalBackgroundTop;
			normalBackgroundBottom = renderer.normalBackgroundBottom;
			hoverBackgroundTop = renderer.hoverBackgroundTop;
			hoverBackgroundBottom = renderer.hoverBackgroundBottom;
			pressedBackgroundTop = renderer.pressedBackgroundTop;
			pressedBackgroundBottom = renderer.pressedBackgroundBottom;
			normalInnerBorderColor = renderer.normalInnerBorderColor;
			hoverInnerBorderColor = renderer.hoverInnerBorderColor;
			checkmarkColor = renderer.checkmarkColor;
			normalInnerBorderWidth = renderer.normalInnerBorderWidth;
			hoverInnerBorderWidth = renderer.hoverInnerBorderWidth;
			pixelAlignment = renderer.pixelAlignment;
			animationSpeed = renderer.animationSpeed;
			linearSpeed = renderer.linearSpeed;
			roundCornerRadius = renderer.roundCornerRadius;
			invert = renderer.invert;
			backgroundBlend = renderer.backgroundBlend == null ? null : new Blend() {
				Factors = (float[]) renderer.backgroundBlend.Factors.Clone(),
				Positions = (float[]) renderer.backgroundBlend.Positions.Clone()
			};
			if (functionToCallOnRefresh == null)
				FunctionToCallOnRefresh = renderer.functionToCallOnRefresh;
			else if (!suppressFunctionCallOnRefresh)
				functionToCallOnRefresh(null);
			ThreadPool.UnsafeQueueUserWorkItem(checkColors, null);
		}

		private void CallCheckColors(object state) {
			OnCheckColor();
		}

		/// <summary>
		/// Checks whether the colors are at their proprietary target value, and if not animates them towards it.
		/// </summary>
		protected virtual void OnCheckColor() {
			if (suppressColorChecking) {
				dirty = true;
				return;
			}
			dirty = false;
			Color targetBackgroundTop, targetBackgroundBottom, targetInnerBorderColor, targetCheckmarkColor;
			float targetInnerBorderWidth, targetCheckmarkOpacityMultiplier;
			if (pressed) {
				targetBackgroundTop = pressedBackgroundTop;
				targetBackgroundBottom = pressedBackgroundBottom;
				targetInnerBorderColor = hoverInnerBorderColor;
				targetInnerBorderWidth = hoverInnerBorderWidth;
			} else if (mouseHovering) {
				targetBackgroundTop = hoverBackgroundTop;
				targetBackgroundBottom = hoverBackgroundBottom;
				targetInnerBorderColor = hoverInnerBorderColor;
				targetInnerBorderWidth = hoverInnerBorderWidth;
			} else {
				targetBackgroundTop = normalBackgroundTop;
				targetBackgroundBottom = normalBackgroundBottom;
				targetInnerBorderColor = normalInnerBorderColor;
				targetInnerBorderWidth = normalInnerBorderWidth;
			}
			if (checkState == CheckState.Unchecked) {
				targetCheckmarkColor = Color.Transparent;
				targetCheckmarkOpacityMultiplier = 0f;
			} else {
				targetCheckmarkColor = checkmarkColor;
				targetCheckmarkOpacityMultiplier = 1f;
			}
			if (!enabled) {
				targetBackgroundTop = GetDisabled(targetBackgroundTop);
				targetBackgroundBottom = GetDisabled(targetBackgroundBottom);
				targetInnerBorderColor = GetDisabled(targetInnerBorderColor);
				targetCheckmarkColor = GetDisabled(targetCheckmarkColor);
			}
			UIAnimator.SharedAnimator.Animate(currentBackgroundTopField, targetBackgroundTop, animationSpeed, linearSpeed, true, Callback, false);
			UIAnimator.SharedAnimator.Animate(currentBackgroundBottomField, targetBackgroundBottom, animationSpeed, linearSpeed, true, Callback, false);
			UIAnimator.SharedAnimator.Animate(currentInnerBorderColorField, targetInnerBorderColor, animationSpeed, linearSpeed, true, Callback, false);
			UIAnimator.SharedAnimator.Animate(currentInnerBorderWidthField, targetInnerBorderWidth, animationSpeed, linearSpeed * 0.01, true, Callback, false);
			UIAnimator.SharedAnimator.Animate(currentCheckmarkColorField, targetCheckmarkColor, animationSpeed, linearSpeed, true, Callback, false);
			UIAnimator.SharedAnimator.Animate(currentCheckmarkOpacityMultiplierField, targetCheckmarkOpacityMultiplier, animationSpeed, linearSpeed * 0.01f, true, Callback, false);
			Action checkColors = CheckColors;
			if (checkColors != null)
				checkColors();
		}

		/// <summary>
		/// Returns whether the two instances of StyleRenderer have an identical color scheme.
		/// </summary>
		/// <param name="a">The first instance.</param>
		/// <param name="b">The second instance.</param>
		public static bool HaveSameColorScheme(StyleRenderer a, StyleRenderer b) {
			if (a == b)
				return true;
			else if (a == null || b == null)
				return false;
			else if (a.border == b.border &&
				a.normalBackgroundTop == b.normalBackgroundTop &&
				a.normalBackgroundBottom == b.normalBackgroundBottom &&
				a.hoverBackgroundTop == b.hoverBackgroundTop &&
				a.hoverBackgroundBottom == b.hoverBackgroundBottom &&
				a.pressedBackgroundTop == b.pressedBackgroundTop &&
				a.pressedBackgroundBottom == b.pressedBackgroundBottom &&
				a.normalInnerBorderColor == b.normalInnerBorderColor &&
				a.hoverInnerBorderColor == b.hoverInnerBorderColor &&
				a.checkmarkColor == b.checkmarkColor &&
				Math.Abs(a.normalInnerBorderWidth - b.normalInnerBorderWidth) <= float.Epsilon &&
				Math.Abs(a.hoverInnerBorderWidth - b.hoverInnerBorderWidth) <= float.Epsilon &&
				Math.Abs(a.roundCornerRadius - b.roundCornerRadius) <= float.Epsilon &&
				a.invert == b.invert) {
				if (a.backgroundBlend == b.BackgroundBlend)
					return true;
				else if (a.backgroundBlend == null || b.backgroundBlend == null)
					return false;
				else
					return AreEqual(a.backgroundBlend.Factors, b.backgroundBlend.Factors) && AreEqual(a.backgroundBlend.Positions, b.backgroundBlend.Positions);
			} else
				return false;
		}

		private static bool AreEqual(float[] a, float[] b) {
			if (a == null && b == null)
				return true;
			else if (a == null || b == null)
				return false;
			else if (a.Length == b.Length) {
				for (int i = 0; i < a.Length; i++) {
					if (a[i] != b[i])
						return false;
				}
				return true;
			} else
				return false;
		}

		/// <summary>
		/// Renders the checkbox and checkmark.
		/// </summary>
		/// <param name="e">Contains the event data.</param>
		protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e) {
			Pressed = e.Item.Pressed;
			MouseHovering = e.Item.Selected;
			if (checkState == CheckState.Unchecked)
				CheckState = CheckState.Checked;
			base.OnRenderItemCheck(e);
		}

		/// <summary>
		/// Renders the items of a drop-down list.
		/// </summary>
		/// <param name="e">Contains the event data.</param>
		protected override void OnRenderDropDownButtonBackground(ToolStripItemRenderEventArgs e) {
			Pressed = e.Item.Pressed;
			MouseHovering = e.Item.Selected;
			RenderBackground(e.Graphics, new Rectangle(1, 0, e.Item.Width - 1, e.Item.Height), false, e.Item.Enabled, false);
		}

		/// <summary>
		/// Renders the item background.
		/// </summary>
		/// <param name="e">Contains the event data.</param>
		protected override void OnRenderItemBackground(ToolStripItemRenderEventArgs e) {
			RenderBackground(e.Graphics, new Rectangle(1, 0, e.Item.Width - 1, e.Item.Height), false, e.Item.Enabled, false);
		}

		/// <summary>
		/// Renders the items of a menu strip.
		/// </summary>
		/// <param name="e">Contains the event data.</param>
		protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e) {
			RenderBackground(e.Graphics, new Rectangle(1, 0, e.Item.Width - 1, e.Item.Height), false, e.Item.Enabled, false);
		}

		/// <summary>
		/// Renders the background of a button.
		/// </summary>
		/// <param name="e">Contains the event data.</param>
		protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e) {
			Pressed = e.Item.Pressed;
			MouseHovering = e.Item.Selected;
			RenderBackground(e.Graphics, new Rectangle(1, 0, e.Item.Width - 1, e.Item.Height), false, e.Item.Enabled, false);
		}

		/// <summary>
		/// Marks that the mouse has left the control.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void MarkMouseHasLeft() {
			if (mouseHovering && pressed) {
				mouseHovering = false;
				pressed = false;
				ThreadPool.UnsafeQueueUserWorkItem(checkColors, null);
			} else {
				MouseHovering = false;
				Pressed = false;
			}
		}

		/// <summary>
		/// Renders the styled control.
		/// </summary>
		/// <param name="g">The graphics canvas to render on.</param>
		/// <param name="bounds">The bounds of the control.</param>
		/// <param name="isEllipse">If true, the item will be rendered as an ellipse instead of a box.</param>
		/// <param name="alignGradientWorkaround">Whether to reduce item height by 1 in order to align the gradient with the menu strip.</param>
		/// <param name="controlBackground">The background image of the control.</param>
		/// <param name="layout">The background image layout.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void RenderBackground(Graphics g, RectangleF bounds, bool isEllipse = false, bool alignGradientWorkaround = false, Image controlBackground = null, ImageLayout layout = ImageLayout.Stretch) {
			RenderBackground(g, bounds, invert, true, isEllipse, alignGradientWorkaround, controlBackground, layout);
		}

		private void RenderBackground(Graphics g, RectangleF bounds, bool invert, bool isEnabled, bool isEllipse = false, bool alignGradientWorkaround = false, Image controlBackground = null, ImageLayout layout = ImageLayout.Stretch) {
			if (bounds.Width <= 0f || bounds.Height <= 0f)
				return;
			g.SmoothingMode = SmoothingMode.HighQuality;
			g.PixelOffsetMode = pixelAlignment;
			g.CompositingQuality = CompositingQuality.HighQuality;
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			float radius = RoundedEdges ? currentCornerRadius : 0f;
			float originalHeight = bounds.Height;
			if (alignGradientWorkaround && bounds.Height > 1f)
				bounds.Height--;
			if (controlBackground != null) {
				try {
					ImageLib.DrawImageWithLayout(g, controlBackground, bounds, layout);
				} catch {
				}
			}
			bool boundsIntegers = bounds.X.RepresentsInteger() && bounds.Y.RepresentsInteger() && bounds.Width.RepresentsInteger() && bounds.Height.RepresentsInteger();
			Rectangle boundsInt = boundsIntegers ? Rectangle.Round(bounds) : Rectangle.Empty;
			if (!(currentBackgroundTop.A == 0 && currentBackgroundBottom.A == 0)) {
				using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(bounds, isEnabled ? currentBackgroundTop : GetDisabled(currentBackgroundTop), isEnabled ? currentBackgroundBottom : GetDisabled(currentBackgroundBottom), invert ? 270f : 90f)) {
					if (BackgroundBlend != null)
						linearGradientBrush.Blend = backgroundBlend;
					if (isEllipse)
						g.FillEllipse(linearGradientBrush, bounds);
					else if (radius <= float.Epsilon) {
						if (boundsIntegers) {
							g.SmoothingMode = SmoothingMode.None;
							g.PixelOffsetMode = PixelOffsetMode.None;
							g.FillRectangle(linearGradientBrush, boundsInt);
							g.SmoothingMode = SmoothingMode.HighQuality;
							g.PixelOffsetMode = pixelAlignment;
						} else
							g.FillRectangle(linearGradientBrush, bounds);
					} else {
						using (GraphicsPath background = GetRoundedRectPath(bounds, radius))
							g.FillPath(linearGradientBrush, background);
					}
				}
			}
			bounds.Height = originalHeight;
			boundsInt.Height = (int) Math.Round(originalHeight);
			if (currentBorder.A != 0) {
				using (Pen pen = new Pen(isEnabled ? currentBorder : GetDisabled(currentBorder))) {
					if (isEllipse)
						g.DrawEllipse(pen, bounds);
					else if (radius <= float.Epsilon) {
						if (boundsIntegers) {
							g.SmoothingMode = SmoothingMode.None;
							g.PixelOffsetMode = PixelOffsetMode.None;
							g.DrawRectangle(pen, boundsInt);
							g.SmoothingMode = SmoothingMode.HighQuality;
							g.PixelOffsetMode = pixelAlignment;
						} else
							g.DrawRectangle(pen, bounds.X, bounds.Y, bounds.Width, bounds.Height);
					} else {
						using (GraphicsPath background = GetRoundedRectPath(bounds, radius))
							g.DrawPath(pen, background);
					}
				}
			}
			if (currentInnerBorderWidth <= 0.5f || currentInnerBorderColor.A == 0)
				return;
			bounds.Inflate(-currentInnerBorderWidth, -currentInnerBorderWidth);
			boundsIntegers &= currentInnerBorderWidth.RepresentsInteger();
			if (boundsIntegers)
				boundsInt = Rectangle.Round(bounds);
			radius = Math.Max(0f, radius - currentInnerBorderWidth);
			using (Pen border = new Pen(isEnabled ? currentInnerBorderColor : GetDisabled(currentInnerBorderColor), currentInnerBorderWidth)) {
				if (isEllipse)
					g.DrawEllipse(border, bounds);
				else if (radius <= float.Epsilon) {
					if (boundsIntegers) {
						g.SmoothingMode = SmoothingMode.None;
						g.PixelOffsetMode = PixelOffsetMode.None;
						g.DrawRectangle(border, boundsInt);
						g.SmoothingMode = SmoothingMode.HighQuality;
						g.PixelOffsetMode = pixelAlignment;
					} else
						g.DrawRectangle(border, bounds.X, bounds.Y, bounds.Width, bounds.Height);
				} else {
					using (GraphicsPath innerBorder = GetRoundedRectPath(bounds, radius))
						g.DrawPath(border, innerBorder);
				}
			}
		}

		/// <summary>
		/// Renders the styled checkbox.
		/// </summary>
		/// <param name="g">The graphics canvas to draw on.</param>
		/// <param name="checkmarkBounds">The checkmark bounds.</param>
		/// <param name="enabled">Whether the control appears enabled.</param>
		/// <param name="radioButton">If true, a radio button is rendered instead of a checkbox.</param>
		public void RenderCheckBox(Graphics g, RectangleF checkmarkBounds, bool enabled, bool radioButton = false) {
			RenderBackground(g, checkmarkBounds, !invert, enabled, radioButton);
			if (checkState == CheckState.Unchecked && (checkmarkColor.A == 0 || currentCheckmarkOpacityMultiplier == 0f))
				return;
			float innerPenDiam = (currentInnerBorderWidth + 1f) * 2f;
			RectangleF bounds = new RectangleF(checkmarkBounds.X + currentInnerBorderWidth + 1f, checkmarkBounds.Y + currentInnerBorderWidth + 1f, checkmarkBounds.Width - innerPenDiam, checkmarkBounds.Height - innerPenDiam);
			g.PixelOffsetMode = PixelOffsetMode.HighQuality;
			g.SmoothingMode = SmoothingMode.HighQuality;
			g.CompositingQuality = CompositingQuality.HighQuality;
			Color topColor = ImageLib.ChangeLightness(currentCheckmarkColor, 25);
			if (!enabled)
				topColor = GetDisabled(topColor);
			Color bottomColor = ImageLib.ChangeLightness(currentCheckmarkColor, -25);
			if (!enabled)
				bottomColor = GetDisabled(bottomColor);
			if (checkState == CheckState.Indeterminate) {
				using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(bounds, topColor, bottomColor, invert ? 270f : 90f)) {
					if (BackgroundBlend != null)
						linearGradientBrush.Blend = backgroundBlend;
					if (radioButton)
						g.FillEllipse(linearGradientBrush, bounds);
					else {
						float radius = RoundedEdges ? currentCornerRadius : 0f;
						if (radius <= float.Epsilon) {
							bool boundsIntegers = bounds.X.RepresentsInteger() && bounds.Y.RepresentsInteger() && bounds.Width.RepresentsInteger() && bounds.Height.RepresentsInteger();
							if (boundsIntegers) {
								g.SmoothingMode = SmoothingMode.None;
								g.PixelOffsetMode = PixelOffsetMode.None;
								g.FillRectangle(linearGradientBrush, Rectangle.Round(bounds));
								g.SmoothingMode = SmoothingMode.HighQuality;
								g.PixelOffsetMode = pixelAlignment;
							} else
								g.FillRectangle(linearGradientBrush, bounds);
						} else {
							using (GraphicsPath background = GetRoundedRectPath(bounds, radius))
								g.FillPath(linearGradientBrush, background);
						}
					}
				}
			} else {
				using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(bounds,
					Color.FromArgb((int) (currentCheckmarkOpacityMultiplier * topColor.A), topColor),
					Color.FromArgb((int) (currentCheckmarkOpacityMultiplier * bottomColor.A), bottomColor), invert ? 270f : 90f)) {
					if (BackgroundBlend != null)
						linearGradientBrush.Blend = backgroundBlend;
					if (radioButton) {
						float halfWidth = bounds.Width * 0.42f, halfHeight = bounds.Height * 0.42f;
						bounds.X += halfWidth * 0.5f;
						bounds.Y += halfHeight * 0.5f;
						bounds.Width -= halfWidth;
						bounds.Height -= halfHeight;
						g.FillEllipse(linearGradientBrush, bounds);
					} else {
						using (GraphicsPath checkmark = GetCheckmarkPath(bounds))
							g.FillPath(linearGradientBrush, checkmark);
					}
				}
			}
		}

		/// <summary>
		/// Gets a graphics path of a checkmark within the specified bounds.
		/// </summary>
		/// <param name="bounds">The bounds to draw the checkmark inside.</param>
		public static GraphicsPath GetCheckmarkPath(RectangleF bounds) {
			GraphicsPath path = new GraphicsPath();
			if (bounds.Width == 0f && bounds.Height == 0f)
				return path;
			path.StartFigure();
			path.AddBezier(bounds.X + 0.22f * bounds.Width, bounds.Y + 0.93f * bounds.Height,
				bounds.X + 0.17f * bounds.Width, bounds.Y + 0.81f * bounds.Height,
				bounds.X + 0.11f * bounds.Width, bounds.Y + 0.74f * bounds.Height,
				bounds.X + 0.05f * bounds.Width, bounds.Y + 0.67f * bounds.Height);
			path.AddBezier(bounds.X + 0.05f * bounds.Width, bounds.Y + 0.67f * bounds.Height,
				bounds.X + 0.04f * bounds.Width, bounds.Y + 0.65f * bounds.Height,
				bounds.X + 0.02f * bounds.Width, bounds.Y + 0.63f * bounds.Height,
				bounds.X + 0.02f * bounds.Width, bounds.Y + 0.63f * bounds.Height);
			path.AddBezier(bounds.X + 0.02f * bounds.Width, bounds.Y + 0.63f * bounds.Height,
				bounds.X, bounds.Y + 0.6f * bounds.Height,
				bounds.X, bounds.Y + 0.57f * bounds.Height,
				bounds.X + 0.01f * bounds.Width, bounds.Y + 0.54f * bounds.Height);
			path.AddBezier(bounds.X + 0.01f * bounds.Width, bounds.Y + 0.54f * bounds.Height,
				bounds.X + 0.02f * bounds.Width, bounds.Y + 0.5f * bounds.Height,
				bounds.X + 0.05f * bounds.Width, bounds.Y + 0.49f * bounds.Height,
				bounds.X + 0.07f * bounds.Width, bounds.Y + 0.49f * bounds.Height);
			path.AddBezier(bounds.X + 0.07f * bounds.Width, bounds.Y + 0.49f * bounds.Height,
				bounds.X + 0.11f * bounds.Width, bounds.Y + 0.49f * bounds.Height,
				bounds.X + 0.16f * bounds.Width, bounds.Y + 0.53f * bounds.Height,
				bounds.X + 0.22f * bounds.Width, bounds.Y + 0.6f * bounds.Height);
			path.AddBezier(bounds.X + 0.22f * bounds.Width, bounds.Y + 0.6f * bounds.Height,
				bounds.X + 0.26f * bounds.Width, bounds.Y + 0.65f * bounds.Height,
				bounds.X + 0.29f * bounds.Width, bounds.Y + 0.7f * bounds.Height,
				bounds.X + 0.33f * bounds.Width, bounds.Y + 0.76f * bounds.Height);
			path.AddBezier(bounds.X + 0.33f * bounds.Width, bounds.Y + 0.76f * bounds.Height,
				bounds.X + 0.44f * bounds.Width, bounds.Y + 0.57f * bounds.Height,
				bounds.X + 0.56f * bounds.Width, bounds.Y + 0.4f * bounds.Height,
				bounds.X + 0.69f * bounds.Width, bounds.Y + 0.24f * bounds.Height);
			path.AddBezier(bounds.X + 0.69f * bounds.Width, bounds.Y + 0.24f * bounds.Height,
				bounds.X + 0.75f * bounds.Width, bounds.Y + 0.17f * bounds.Height,
				bounds.X + 0.82f * bounds.Width, bounds.Y + 0.09f * bounds.Height,
				bounds.X + 0.85f * bounds.Width, bounds.Y + 0.06f * bounds.Height);
			path.AddBezier(bounds.X + 0.85f * bounds.Width, bounds.Y + 0.06f * bounds.Height,
				bounds.X + 0.88f * bounds.Width, bounds.Y + 0.03f * bounds.Height,
				bounds.X + 0.9f * bounds.Width, bounds.Y + 0.01f * bounds.Height,
				bounds.X + 0.91f * bounds.Width, bounds.Y + 0.01f * bounds.Height);
			path.AddBezier(bounds.X + 0.91f * bounds.Width, bounds.Y + 0.01f * bounds.Height,
				bounds.X + 0.92f * bounds.Width, bounds.Y,
				bounds.X + 0.93f * bounds.Width, bounds.Y,
				bounds.X + 0.94f * bounds.Width, bounds.Y);
			path.AddBezier(bounds.X + 0.94f * bounds.Width, bounds.Y,
				bounds.X + 0.96f * bounds.Width, bounds.Y,
				bounds.X + 0.99f * bounds.Width, bounds.Y + 0.02f * bounds.Height,
				bounds.X + bounds.Width, bounds.Y + 0.05f * bounds.Height);
			path.AddBezier(bounds.X + bounds.Width, bounds.Y + 0.05f * bounds.Height,
				bounds.X + bounds.Width, bounds.Y + 0.07f * bounds.Height,
				bounds.X + bounds.Width, bounds.Y + 0.09f * bounds.Height,
				bounds.X + 0.99f * bounds.Width, bounds.Y + 0.11f * bounds.Height);
			path.AddBezier(bounds.X + 0.99f * bounds.Width, bounds.Y + 0.11f * bounds.Height,
				bounds.X + 0.98f * bounds.Width, bounds.Y + 0.14f * bounds.Height,
				bounds.X + 0.97f * bounds.Width, bounds.Y + 0.16f * bounds.Height,
				bounds.X + 0.91f * bounds.Width, bounds.Y + 0.23f * bounds.Height);
			path.AddBezier(bounds.X + 0.91f * bounds.Width, bounds.Y + 0.23f * bounds.Height,
				bounds.X + 0.82f * bounds.Width, bounds.Y + 0.35f * bounds.Height,
				bounds.X + 0.77f * bounds.Width, bounds.Y + 0.41f * bounds.Height,
				bounds.X + 0.73f * bounds.Width, bounds.Y + 0.47f * bounds.Height);
			path.AddBezier(bounds.X + 0.73f * bounds.Width, bounds.Y + 0.47f * bounds.Height,
				bounds.X + 0.63f * bounds.Width, bounds.Y + 0.61f * bounds.Height,
				bounds.X + 0.54f * bounds.Width, bounds.Y + 0.74f * bounds.Height,
				bounds.X + 0.44f * bounds.Width, bounds.Y + 0.94f * bounds.Height);
			path.AddBezier(bounds.X + 0.44f * bounds.Width, bounds.Y + 0.94f * bounds.Height,
				bounds.X + 0.44f * bounds.Width, bounds.Y + 0.94f * bounds.Height,
				bounds.X + 0.41f * bounds.Width, bounds.Y + bounds.Height,
				bounds.X + 0.32f * bounds.Width, bounds.Y + bounds.Height);
			path.AddBezier(bounds.X + 0.32f * bounds.Width, bounds.Y + bounds.Height,
				bounds.X + 0.24f * bounds.Width, bounds.Y + bounds.Height,
				bounds.X + 0.22f * bounds.Width, bounds.Y + 0.93f * bounds.Height,
				bounds.X + 0.22f * bounds.Width, bounds.Y + 0.923f * bounds.Height);
			path.CloseFigure();
			return path;
		}

		/// <summary>
		/// Gets the disabled version of the specified color.
		/// </summary>
		/// <param name="color">The color to make look disabled.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Color GetDisabled(Color color) {
			byte disabled = ImageLib.ToGrayscale(color, color.A);
			disabled = (byte) (disabled < 40 ? 0 : disabled - 40);
			return Color.FromArgb(color.A, disabled, disabled, disabled);
		}

		/// <summary>
		/// Gets the disabled version of the specified color.
		/// </summary>
		/// <param name="color">The color to make look disabled.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static BgraColor GetDisabled(BgraColor color) {
			byte disabled = ImageLib.ToGrayscale(color, color.A);
			disabled = (byte) (disabled < 40 ? 0 : disabled - 40);
			return new BgraColor(color.A, disabled, disabled, disabled);
		}

		/// <summary>
		/// Returns a clone of the style renderer with copied parameters (except for the parent property).
		/// </summary>
		public virtual object Clone() {
			return new StyleRenderer(this);
		}

		/// <summary>
		/// Gets the path of a rounded with the specified parameters.
		/// </summary>
		/// <param name="rect">The boundaries of the rectangle.</param>
		/// <param name="radius">The radius of the rounded corners.</param>
		public static GraphicsPath GetRoundedRectPath(RectangleF rect, float radius) {
			GraphicsPath path = new GraphicsPath();
			if (radius <= float.Epsilon)
				path.AddRectangle(rect);
			else {
				path.StartFigure();
				path.AddArc(rect.X, rect.Y, radius, radius, 180f, 90f);
				path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270f, 90f);
				path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0f, 90f);
				path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90f, 90f);
				path.CloseFigure();
			}
			return path;
		}

		/// <summary>
		/// Gets the most appropriate StringFormat using the specified parameters.
		/// </summary>
		/// <param name="alignment">The alignment style to use.</param>
		/// <param name="rightToLeft">Whether to align left or right.</param>
		/// <param name="vertical">Whether the text is vertical or horizontal.</param>
		/// <param name="ellipsis">Whether to use ellipsis (...) when the text does not properly fit in the bounds.</param>
		/// <param name="wrapping">Whether to wrap text the text in the bounds.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static StringFormat GetFormat(ContentAlignment alignment, RightToLeft rightToLeft, bool wrapping = false, bool vertical = false, bool ellipsis = false) {
			return GetFormat(alignment, wrapping, vertical, ellipsis, rightToLeft == RightToLeft.Yes);
		}

		/// <summary>
		/// Gets the most appropriate StringFormat using the specified parameters.
		/// </summary>
		/// <param name="alignment">The alignment style to use.</param>
		/// <param name="rightToLeft">Whether to align left or right.</param>
		/// <param name="vertical">Whether the text is vertical or horizontal.</param>
		/// <param name="ellipsis">Whether to use ellipsis (...) when the text does not properly fit in the bounds.</param>
		/// <param name="wrapping">Whether to wrap text the text in the bounds.</param>
		public static StringFormat GetFormat(ContentAlignment alignment, bool wrapping = false, bool vertical = false, bool ellipsis = false, bool rightToLeft = false) {
			StringFormat format;
			bool isNotWrapping = !wrapping;
			if (vertical || isNotWrapping || rightToLeft) {
				if (vertical && isNotWrapping && rightToLeft)
					format = new StringFormat(StringFormatFlags.DirectionVertical | StringFormatFlags.NoWrap | StringFormatFlags.DirectionRightToLeft);
				else if (vertical && isNotWrapping)
					format = new StringFormat(StringFormatFlags.DirectionVertical | StringFormatFlags.NoWrap);
				else if (isNotWrapping && rightToLeft)
					format = new StringFormat(StringFormatFlags.NoWrap | StringFormatFlags.DirectionRightToLeft);
				else if (vertical && rightToLeft)
					format = new StringFormat(StringFormatFlags.DirectionVertical | StringFormatFlags.DirectionRightToLeft);
				else if (vertical)
					format = new StringFormat(StringFormatFlags.DirectionVertical);
				else if (isNotWrapping)
					format = new StringFormat(StringFormatFlags.NoWrap);
				else
					format = new StringFormat(StringFormatFlags.DirectionRightToLeft);
			} else
				format = new StringFormat();
			format.FormatFlags |= StringFormatFlags.NoClip;
			switch (alignment) {
				case ContentAlignment.TopLeft:
					format.Alignment = StringAlignment.Near;
					format.LineAlignment = StringAlignment.Near;
					break;
				case ContentAlignment.MiddleLeft:
					format.Alignment = StringAlignment.Near;
					format.LineAlignment = StringAlignment.Center;
					break;
				case ContentAlignment.BottomLeft:
					format.Alignment = StringAlignment.Near;
					format.LineAlignment = StringAlignment.Far;
					break;
				case ContentAlignment.TopCenter:
					format.Alignment = StringAlignment.Center;
					format.LineAlignment = StringAlignment.Near;
					break;
				case ContentAlignment.MiddleCenter:
					format.Alignment = StringAlignment.Center;
					format.LineAlignment = StringAlignment.Center;
					break;
				case ContentAlignment.BottomCenter:
					format.Alignment = StringAlignment.Center;
					format.LineAlignment = StringAlignment.Far;
					break;
				case ContentAlignment.TopRight:
					format.Alignment = StringAlignment.Far;
					format.LineAlignment = StringAlignment.Near;
					break;
				case ContentAlignment.MiddleRight:
					format.Alignment = StringAlignment.Far;
					format.LineAlignment = StringAlignment.Center;
					break;
				case ContentAlignment.BottomRight:
					format.Alignment = StringAlignment.Far;
					format.LineAlignment = StringAlignment.Far;
					break;
			}
			format.Trimming = ellipsis ? StringTrimming.EllipsisCharacter : StringTrimming.Character;
			return format;
		}

		/// <summary>
		/// Disposes of the animation timer.
		/// </summary>
		~StyleRenderer() {
			Dispose();
		}

		/// <summary>
		/// Disposes of the animation timer.
		/// </summary>
		public void Dispose() {
			if (InstanceAnimator == null || InstanceAnimator == UIAnimator.SharedAnimator)
				return;
			InstanceAnimator.Dispose();
			InstanceAnimator = null;
			GC.SuppressFinalize(this);
		}
	}
}