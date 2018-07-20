using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Graphics;
using System.Graphics.OGL;
using System.Graphics.Models;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Dispatch;

namespace System.Windows.Forms {
	/// <summary>
	/// A styled window that is packed with features including automatic OpenGL context handling.
	/// </summary>
	[Description("A styled context window that is packed with features including automatic OpenGL context handling.")]
	[DisplayName(nameof(GraphicsForm))]
	[DefaultEvent(nameof(Load))]
	public class GraphicsForm : StyledForm {
		/// <summary>
		/// Fired when the GDI layer is in the process of re-painting.
		/// </summary>
		[Description("Fired when the GDI layer is in the process of re-painting.")]
		public event PaintEventHandler GdiInvalidated;
		private Func<Control, bool> gdiControlAdded, gdiControlRemoved;
		private InvalidateEventHandler GdiControlInvalidate;
		private ThreadLocal<bool> reentrant = new ThreadLocal<bool>();
		private object GdiSyncRoot = new object(), GLTransferSync = new object();
		private Func<object, object> invalidateGdiInner, callDrawBorder, makeCurrent, viewSizeChanged, unload;
		private MeshComponent rectMesh;
		private GlobalShader globalShader;
		private Padding glMargin;
		private GraphicsComponent graphicsComponent;
		private GraphicsContext glContext;
		private Bitmap gdiMask;
		private Drawing.Graphics gdiCanvas;
		private Texture2D GdiTexture;
		private Rectangle invalidatedRect, gdiBounds = new Rectangle(0, 0, 200, 200);
		private Func<object, object> PaintGL;
		private PreciseStopwatch rpsStopwatch = new PreciseStopwatch(), upsStopwatch = new PreciseStopwatch();
		private Size maskSize, clientSize;
		private AsyncTimer updateTimer;
		private DispatcherSlim GLDispatcher, GdiDispatcher;
		private int glQueueCount, gdiQueueCount, forceRedraw, rpsCounter, upsCounter, rendersPerSecond, updatesPerSecond, defaultInterval = 6;
		private bool gdiEnabled, isPausedBecauseOfMinimize, dontWaitForInterval, gdiCoordinateRelativeToGL = true;
		private double cumulativeUpdate, sampleIntervalInMs = 1000.0;
		private PaintEventHandler paintControl;
		/// <summary>
		/// A list of GDI controls that are added to the GDI overlay of the form (if enabled).
		/// </summary>
		public readonly SyncedList<Control> GdiControls = new SyncedList<Control>();
		/// <summary>
		/// Locked when OpenGL is about to be rendered.
		/// </summary>
		public readonly object RenderLock = new object();

		/// <summary>
		/// Gets the current Gdi thread dispatcher
		/// </summary>
		protected DispatcherSlim CurrentGdiDispatcher {
			get {
				if (GdiDispatcher == null)
					GdiDispatcher = new DispatcherSlim(nameof(GdiDispatcher), true, ExceptionMode.Log);
				return GdiDispatcher;
			}
		}

		/// <summary>
		/// OpenGL is supported don't worry, but this should return false.
		/// </summary>
		[Browsable(false)]
		public override bool SupportsGL {
			get {
				return false;
			}
		}

		/// <summary>
		/// Gets or sets whether to disable simultaneous updating and rendering (in case it causes glitches).
		/// </summary>
		[Description("Gets or sets whether to disable simultaneous updating and rendering (in case it causes glitches).")]
		[DefaultValue(false)]
		public bool DisableSimultaneousUpdatingAndRendering {
			get;
			set;
		}

		/// <summary>
		/// Gets the update timer for accessing its properties. The usual pattern is to call InvalidateGL() at the end of an update callback.
		/// </summary>
		private AsyncTimer UpdateTimer {
			get {
				if (updateTimer == null) {
					updateTimer = new AsyncTimer(defaultInterval);
					updateTimer.Tick += CallOnUpdate;
					updateTimer.DontWaitForInterval = dontWaitForInterval;
				}
				return updateTimer;
			}
		}

		/// <summary>
		/// Gets or sets the update timer interval in milliseconds.
		/// </summary>
		public int UpdateInterval {
			get {
				return updateTimer == null ? defaultInterval : updateTimer.Interval;
			}
			set {
				if (updateTimer == null)
					defaultInterval = value;
				else
					updateTimer.Interval = value;
			}
		}

		/// <summary>
		/// If true, update intervals are ignored and OnUpdate() is called immediately after the previous call (not recommended).
		/// </summary>
		public bool DontWaitForInterval {
			get {
				return updateTimer == null ? dontWaitForInterval : updateTimer.DontWaitForInterval;
			}
			set {
				if (updateTimer == null)
					dontWaitForInterval = value;
				else
					updateTimer.DontWaitForInterval = value;
			}
		}

		/// <summary>
		/// Gets or sets whether the GDI layer should be offset if the GL viewport margin is changed.
		/// </summary>
		[Description("Gets or sets whether the GDI layer should be offset if the GL viewport margin is changed.")]
		[DefaultValue(true)]
		public bool GdiCoordinateRelativeToGL {
			get {
				return gdiCoordinateRelativeToGL;
			}
			set {
				if (value == gdiCoordinateRelativeToGL)
					return;
				gdiCoordinateRelativeToGL = value;
				if (IsGdiEnabled) {
					if (IsGLEnabled)
						InvalidateGL();
					else
						Invalidate(false);
				}
			}
		}

		/// <summary>
		/// Gets or sets whether to enable the update timer, which calls OnUpdate() periodically.
		/// The usual pattern is to call InvalidateGL() at the end of an update callback.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool UpdateTimerRunning {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return updateTimer != null && updateTimer.Running;
			}
			set {
				if (UpdateTimerRunning == value)
					return;
				else if (value && IsMinimized && !IsClosing) {
					isPausedBecauseOfMinimize = true;
					value = false;
				} else
					isPausedBecauseOfMinimize = false;
				if (value)
					upsStopwatch.Restart();
				else {
					cumulativeUpdate = 0.0;
					upsCounter = 0;
				}
				UpdateTimer.Running = value;
			}
		}

		/// <summary>
		/// Gets the current number of renders occurring in a second (can be larger than the number of FPS).
		/// </summary>
		[Browsable(false)]
		public int RendersPerSecond {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return rpsStopwatch.ElapsedMilliseconds <= (sampleIntervalInMs * 2.0) ? rendersPerSecond : 0;
			}
		}

		/// <summary>
		/// Gets the current number of frame updates occurring in a second (can be larger than the number of renders per second).
		/// </summary>
		[Browsable(false)]
		public int UpdatesPerSecond {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return upsStopwatch.ElapsedMilliseconds <= (sampleIntervalInMs * 2.0) ? updatesPerSecond : 0;
			}
		}

		/// <summary>
		/// Gets the FPS, where a frame is defined as an update and a render. 
		/// </summary>
		[Browsable(false)]
		public int FramesPerSecond {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return UpdatesPerSecond == 0 ? rendersPerSecond : Math.Min(updatesPerSecond, rendersPerSecond);
			}
		}

		/// <summary>
		/// Gets or sets the sample interval in milliseconds for calculating the number of updates and frames per second.
		/// </summary>
		[Description("Gets or sets the sample interval in milliseconds for calculating the number of updates and frames per second.")]
		[DefaultValue(1000.0)]
		public double SampleIntervalInMs {
			get {
				return sampleIntervalInMs;
			}
			set {
				if (value <= 0.0)
					value = 1000.0;
				sampleIntervalInMs = value;
			}
		}

		/// <summary>
		/// If true, the GDI layer refresh rate is reduced if it is run asynchronously. This is to reduce CPU usage.
		/// </summary>
		[DefaultValue(false)]
		[Description("If true, the GDI layer refresh rate is reduced if it is run asynchronously. This is to reduce CPU usage.")]
		public bool ReduceGdiCpuUsage {
			get;
			set;
		}

		/// <summary>
		/// Gets whether the OpenGL context is currently enabled.
		/// </summary>
		[Browsable(false)]
		public override sealed bool IsGLEnabled {
			get {
				return GLContext != null;
			}
		}

		/// <summary>
		/// Gets the thread on which the OpenGL context is present (can be null).
		/// </summary>
		[Browsable(false)]
		public Thread GLThread {
			get {
				GraphicsContext context = GLContext;
				return context == null ? (IsGLEnabled ? (GLDispatcher == null ? ResidentThread : GLDispatcher.DispatchThread) : null) : context.CurrentContextThread;
			}
		}

		/// <summary>
		/// If true, the invalidated GDI region is cleared to transparent before drawing.
		/// </summary>
		[Description("If true, the invalidated GDI region is cleared to transparent before drawing.")]
		[DefaultValue(true)]
		public bool ClearInvalidatedGdi {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the XYZ rotation of the GDI layer. Kinda weird feature, might not work properly
		/// </summary>
		[Browsable(false)]
		public Vector3 GdiRotation {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the scale of the GDI layer. Kinda weird feature.
		/// </summary>
		[Browsable(false)]
		public Vector2 GdiScale {
			get;
			set;
		}

		/// <summary>
		/// Gets whether OpenGL is currently initialized on a separate thread.
		/// </summary>
		[Browsable(false)]
		public bool IsGLOnSeparateThread {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				GraphicsContext context = GLContext;
				return !(context == null || GLDispatcher == null) && GLDispatcher.DispatchThread == context.CurrentContextThread;
			}
		}

		/// <summary>
		/// Gets the current GDI layer location relative to viewport coordinates.
		/// </summary>
		[Browsable(false)]
		public Point CurrentGdiLocation {
			get {
				return gdiCoordinateRelativeToGL ? new Point(gdiBounds.X + glMargin.Left, gdiBounds.Y + glMargin.Top) : gdiBounds.Location;
			}
		}

		/// <summary>
		/// Gets or sets the location of the GDI layer relative to the view port (unless GdiCoordinateRelativeToGL is true).
		/// </summary>
		[Description("Gets or sets the location of the GDI layer relative to the view port (unless GdiCoordinateRelativeToGL is true).")]
		public Point GdiLocation {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return gdiBounds.Location;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				if (value == gdiBounds.Location)
					return;
				gdiBounds.Location = value;
				if (IsGLEnabled)
					InvalidateGL(false);
				else
					Invalidate(false);
			}
		}

		/// <summary>
		/// Gets or sets the margin of the OpenGL scissor box from the view port of the window. This is to account for menu bars, status bars...etc.
		/// </summary>
		[Description("Gets or sets the margin of the OpenGL scissor box from the view port of the window. This is to account for menu bars, status bars...etc.")]
		public Padding GLMargin {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return glMargin;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				if (value == glMargin)
					return;
				glMargin = value;
				InvokeOnGLThreadAsync(new InvocationData(viewSizeChanged), true);
			}
		}

		/// <summary>
		/// If true, the viewport is refreshed synchronously instead of asynchronously during a resize animation.
		/// </summary>
		[DefaultValue(true)]
		public bool ReduceFlickerOnResize {
			get;
			set;
		}

		/// <summary>
		/// Gets the OpenGL context of the window (or null if not initialized).
		/// </summary>
		[Browsable(false)]
		public virtual GraphicsContext GLContext {
			get {
				if (glContext != null && glContext.IsDisposed)
					glContext = null;
				return glContext;
			}
		}

		/// <summary>
		/// Gets the GL context view port scissor box in GL coordinates (ie. Y-coordinate is flipped).
		/// </summary>
		[Browsable(false)]
		public Rectangle GLViewport {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				Rectangle port = /*new Rectangle(new Point(CurrentBorderWidth, CurrentBorderWidth), ViewSize)*/
					new Rectangle(Point.Empty, ClientSize);
				port.X += glMargin.Left;
				port.Y += glMargin.Bottom;
				port.Width -= glMargin.Left + glMargin.Right;
				if (port.Width < 0)
					port.Width = 0;
				port.Height -= glMargin.Top + glMargin.Bottom;
				if (port.Height < 0)
					port.Height = 0;
				return port;
			}
		}

		/// <summary>
		/// Gets the graphics component that identifies this window to use for setting up a GL context.
		/// </summary>
		[Browsable(false)]
		public virtual GraphicsComponent GraphicsComponent {
			get {
				if (graphicsComponent != null && graphicsComponent.IsDisposed)
					graphicsComponent = null;
				return graphicsComponent;
			}
		}

		/// <summary>
		/// Gets whether the GL context is currently available for use on this very thread.
		/// </summary>
		[Browsable(false)]
		public bool IsGLContextAvailable {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				GraphicsContext context = GLContext;
				return !(context == null || DesignMode) && context.IsCurrent;
			}
		}

		/// <summary>
		/// Gets or sets whether the border should be rendered on the GDI layer of the form. Preferably disabled.
		/// </summary>
		[Description("Gets or sets whether the border should be rendered on the GDI layer of the form")]
		[DefaultValue(true)]
		public bool RenderBorderOnGdiLayer {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets whether a GDI layer is enabled on top of the form.
		/// </summary>
		[Description("Gets or sets whether a GDI layer is enabled on top of the form.")]
		[DefaultValue(false)]
		public bool IsGdiEnabled {
			get {
				return gdiEnabled && !DesignMode;
			}
			set {
				if (value == gdiEnabled || DesignMode)
					return;
				gdiEnabled = value;
				if (value) {
					OnGdiEnabled();
					InvalidateGdi(GdiRenderMode.GdiAsync);
				}
			}
		}

		/// <summary>
		/// Gets whether the OpenGL context is currently being unloaded.
		/// </summary>
		[Browsable(false)]
		public bool Unloading {
			get;
			private set;
		}

		/// <summary>
		/// Modifies the parameters used to create the window.
		/// </summary>
		protected override CreateParams CreateParams {
			get {
				CreateParams cp = base.CreateParams;
				cp.ClassStyle |= (int) Platforms.Windows.ClassStyle.OwnDC;
				return cp;
			}
		}

		/// <summary>
		/// Initializes a new window that can be used for OpenGL rendering and stuff.
		/// </summary>
		public GraphicsForm() {
		}

		/// <summary>
		/// Called when the constructor is about to start. This is called only once.
		/// Don't do anything fancy, almost everything is null or uninitialized.
		/// </summary>
		protected override void OnConstructorStarted() {
			base.OnConstructorStarted();
			RenderBorderOnGdiLayer = true;
			ReduceFlickerOnResize = true;
			EnableAeroBlur = false;
			callDrawBorder = CallDrawBorder;
			paintControl = Control_Paint;
			GdiControlInvalidate = Ctrl_Invalidated;
			gdiControlAdded = GdiControlAdded;
			gdiControlRemoved = GdiControlRemoved;
			GdiControls.Filter = gdiControlAdded;
			GdiControls.ShouldRemove = gdiControlRemoved;
			PaintGL = CallPaintGL;
			ClearInvalidatedGdi = true;
			unload = CallUnload;
			GdiScale = Vector2.One;
			invalidateGdiInner = InvalidateGdiInner;
			makeCurrent = MakeCurrent;
			viewSizeChanged = CallViewSizeChanged;
			gdiControlAdded = GdiControlAdded;
		}

		private object CallPaintGL(object param) {
			globalShader.Bind();
			lock (RenderLock) {
				if (!IsGLEnabled || Unloading)
					return null;
				OnPaintGL();
			}
			if (IsGLEnabled && !Unloading) {
				if (gdiEnabled) {
					if (invalidatedRect != Rectangle.Empty || GdiTexture == null || GdiTexture.BitmapSize != maskSize) {
						if (GdiTexture == null || GdiTexture.BitmapSize != maskSize) {
							lock (GdiSyncRoot) {
								if (GdiTexture != null)
									GdiTexture.Dispose();
								GdiTexture = new Texture2D(gdiMask, NPotTextureScaleMode.None, false, true, false);
								GdiTexture.Bind();
								invalidatedRect = Rectangle.Empty;
							}
						} else {
							Rectangle rect = invalidatedRect;
							if (rect.Right > maskSize.Width)
								rect.Width = maskSize.Width - rect.X;
							if (rect.Bottom > maskSize.Height)
								rect.Height = maskSize.Height - rect.Y;
							if (GdiTexture.HasBeenBoundAtLeastOnce)
								GdiTexture.Bind();
							else {
								lock (GdiSyncRoot)
									GdiTexture.Bind();
							}
							if (gdiMask != null && (Volatile.Read(ref gdiQueueCount) == 0 || Volatile.Read(ref forceRedraw) == 1)) {
								Interlocked.Exchange(ref forceRedraw, 0);
								lock (GdiSyncRoot) {
									GdiTexture.UpdateRegion(gdiMask, rect, rect.Location);
									invalidatedRect = Rectangle.Empty;
								}
							}
						}
					} else
						GdiTexture.Bind();
					globalShader.Bind();
					Rectangle glViewport = GLViewport;
					Mesh2D.Setup2D(glViewport.Size);
					Mesh2D.DrawTexture2D(GdiTexture, Vector3.Zero, new Vector2(maskSize.Width * GdiScale.X, maskSize.Height * GdiScale.Y), GdiRotation, rectMesh);
				}
				if (!RenderBorderOnGdiLayer)
					DrawBorderGL();
				GraphicsContext context = GLContext;
				if (context != null)
					context.SwapBuffers();
			}
			double elapsed = rpsStopwatch.ElapsedMilliseconds;
			if (elapsed >= sampleIntervalInMs) {
				elapsed -= sampleIntervalInMs;
				if (elapsed >= sampleIntervalInMs) {
					rendersPerSecond = 0;
					rpsStopwatch.ElapsedMilliseconds = elapsed % sampleIntervalInMs;
				} else {
					rendersPerSecond = (int) Math.Ceiling((rpsCounter + 1) * 1000.0 / sampleIntervalInMs);
					rpsStopwatch.ElapsedMilliseconds = elapsed;
				}
				rpsCounter = 0;
				OnFramesPerSecondUpdated();
			} else
				rpsCounter++;
			if (Volatile.Read(ref glQueueCount) >= 1)
				Interlocked.Decrement(ref glQueueCount);
			return null;
		}

		/// <summary>
		/// Called when the update is finished.
		/// </summary>
		private void CallOnUpdate(AsyncTimer sender) {
			double elapsedMs = upsStopwatch.RestartGet();
			cumulativeUpdate += elapsedMs;
			if (cumulativeUpdate >= sampleIntervalInMs) {
				cumulativeUpdate -= sampleIntervalInMs;
				if (cumulativeUpdate >= sampleIntervalInMs) {
					updatesPerSecond = 0;
					cumulativeUpdate %= sampleIntervalInMs;
				} else
					updatesPerSecond = (int) Math.Ceiling((upsCounter + 1) * 1000.0 / sampleIntervalInMs);
				upsCounter = 0;
			} else
				upsCounter++;
			if (elapsedMs == 0.0)
				elapsedMs = updateTimer.Interval;
			if (DisableSimultaneousUpdatingAndRendering) {
				lock (RenderLock)
					OnUpdate(elapsedMs);
			} else
				OnUpdate(elapsedMs);
		}

		/// <summary>
		/// Called when the window handle is created.
		/// </summary>
		/// <param name="e">Ignored</param>
		protected override void OnHandleCreated(EventArgs e) {
			base.OnHandleCreated(e);
			if (IsGLEnabled) {
				GraphicsComponent component = GraphicsComponent;
				if (component != null)
					ReinitializeGL(GLDispatcher, component.GraphicsMode, component.Version, glContext);
			}
		}

		/// <summary>
		/// Called when the update timer has ticked.
		/// </summary>
		/// <param name="elapsedMilliseconds">The milliseconds elapsed since the last frame.</param>
		protected virtual void OnUpdate(double elapsedMilliseconds) {
		}

		/// <summary>
		/// Called when the minimize animation is completed.
		/// </summary>
		protected override void OnMinimizeAnimationFinished() {
			base.OnMinimizeAnimationFinished();
			if (!IsMinimized)
				InvokeOnGLThreadAsync(new InvocationData(PaintGL), false);
		}

		/// <summary>
		/// Called when the window fade-in animation is completed.
		/// </summary>
		protected override void OnFadeInCompleted() {
			base.OnFadeInCompleted();
			InvalidateGL(false);
		}

		/// <summary>
		/// Called when the minimize state of the window is changed.
		/// </summary>
		protected override void OnMinimizeChanged() {
			base.OnMinimizeChanged();
			if (IsMinimized) {
				if (UpdateTimerRunning) {
					UpdateTimerRunning = false;
					isPausedBecauseOfMinimize = true;
				}
			} else {
				if (isPausedBecauseOfMinimize)
					UpdateTimerRunning = true;
			}
		}

		/// <summary>
		/// Scales a control's location, size, padding and margin.
		/// </summary>
		/// <param name="factor">The factor by which the height and width of the control will be scaled.</param>
		/// <param name="specified">A <see cref="T:System.Windows.Forms.BoundsSpecified" /> value that specifies the bounds of the control to use when defining its size and position.</param>
		protected override void ScaleControl(SizeF factor, BoundsSpecified specified) {
			base.ScaleControl(factor, specified);
			lock (GdiControls.SyncRoot) {
				for (int i = 0; i < GdiControls.Items.Count; i++)
					GdiControls.Items[i].Scale(factor);
			}
		}

		/// <summary>
		/// Causes OpenGL to re-render the scene synchronously or asynchronously.
		/// </summary>
		/// <param name="sync">If true, the method waits for the re-rendering to complete before returning.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public InvocationData InvalidateGL(bool sync = false) {
			InvocationData invocation = new InvocationData(PaintGL);
			if (!IsGLEnabled || Volatile.Read(ref glQueueCount) >= 2) {
				invocation.State = InvokeState.Completed;
				return invocation;
			} else {
				Interlocked.Increment(ref glQueueCount);
				if (sync)
					InvokeOnGLThreadSync(invocation, 100, false);
				else
					InvokeOnGLThreadAsync(invocation, false);
				return invocation;
			}
		}

		/// <summary>
		/// Makes the specified invocation synchronously on the OpenGL thread.
		/// </summary>
		/// <param name="parameters">The invocation to make.</param>
		/// <param name="timeout">The sync timeout (leave to 0 to wait indefinitely). The timout is only used if OpenGL is loaded on a separate thread.</param>
		/// <param name="firstClass">For internal use only. Sets whether the invocation should be addressed immediately.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void InvokeOnGLThreadSync(InvocationData parameters, int timeout = 0, bool firstClass = false) {
			if (IsGLEnabled) {
				Thread contextThread = GLThread;
				if (GLDispatcher == null || GLDispatcher.DispatchThread != contextThread) {
					Thread currentThread = Thread.CurrentThread;
					if (currentThread == contextThread)
						parameters.InvokeOnCurrentThread();
					else if (contextThread == ResidentThread) {
						if (IsHandleCreated) {
							Invoke(parameters, timeout);
						}/* else
							throw new InvalidOperationException("The window handle is not created, hence cannot invoke.");*/
					}/* else
						throw new InvalidOperationException("The context thread is unrecognized.");*/
				} else
					GLDispatcher.Invoke(parameters, timeout, firstClass);
			}
		}

		/// <summary>
		/// Makes the specified invocation asynchronously on the OpenGL thread.
		/// </summary>
		/// <param name="parameters">The invocation to make.</param>
		/// <param name="firstClass">For internal use only. Sets whether the invocation should be addressed immediately.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void InvokeOnGLThreadAsync(InvocationData parameters, bool firstClass = false) {
			if (IsGLEnabled) {
				Thread contextThread = GLThread;
				if (GLDispatcher == null || GLDispatcher.DispatchThread != contextThread) {
					Thread currentThread = Thread.CurrentThread;
					if (currentThread == contextThread)
						parameters.InvokeOnCurrentThread();
					else if (contextThread == ResidentThread) {
						if (IsHandleCreated) {
							BeginInvoke(parameters);
						}/* else
							throw new InvalidOperationException("The window handle is not created, hence cannot invoke.");*/
					}/* else
						throw new InvalidOperationException("The context thread is unrecognized.");*/
				} else
					GLDispatcher.BeginInvoke(parameters, firstClass);
			}
		}

		/// <summary>
		/// Gets the child control that is found in the GDI layer at the specified viewport coordinate.
		/// </summary>
		/// <param name="position">The viewport coordinate to check at.</param>
		/// <param name="skipParameters">Sets which control categories are skipped.</param>
		/// <param name="clientToCtrl">If a GDI child control is found, its value is 'position' but in client coordinates relative to the returned control,
		/// else it's untouched.</param>
		protected override sealed Control GetGdiChildAtPoint(Point position, ChildSkip skipParameters, ref Point clientToCtrl) {
			if (DesignMode || !gdiEnabled)
				return null;
			Stack<Control> controls = new Stack<Control>();
			foreach (Control gdi in GdiControls)
				controls.Push(gdi);
			Control child, candidate = null;
			Control.ControlCollection collection;
			while (controls.Count != 0) {
				child = controls.Pop();
				if (CheckGdiEligibleParameters(child, position, skipParameters, ref clientToCtrl)) {
					candidate = child;
					controls.Clear();
				}
				collection = child.Controls;
				for (int i = 0; i < collection.Count; i++)
					controls.Push(collection[i]);
			}
			return candidate;
		}

		private bool CheckGdiEligibleParameters(Control ctrl, Point viewportCoordinate, ChildSkip toCheck, ref Point clientToCtrl) {
			if (ctrl == null)
				return false;
			Rectangle rect = new Rectangle(CurrentGdiLocation + (Size) Extensions.PointToScreen(ctrl, Point.Empty), ctrl.ClientSize);
			Point localClientCoord = new Point(viewportCoordinate.X - rect.X, viewportCoordinate.Y - rect.Y);
			if (localClientCoord.X >= 0 && localClientCoord.Y >= 0 && localClientCoord.X < rect.Width && localClientCoord.Y < rect.Height
				&& (ctrl.Region == null || ctrl.Region.IsVisible(viewportCoordinate))
				&& ((toCheck & ChildSkip.Invisible) != ChildSkip.Invisible || ctrl.Visible || ctrl is Form)
				&& ((toCheck & ChildSkip.Disabled) != ChildSkip.Disabled || ctrl.Enabled)
				&& ((toCheck & ChildSkip.DontRespondToMouse) != ChildSkip.DontRespondToMouse || InvokeWndProc(ctrl, new Message() {
					Msg = (int) Platforms.Windows.WindowMessage.NCHITTEST,
					HWnd = ctrl.Handle,
					WParam = IntPtr.Zero,
					LParam = new IntPtr(((rect.Y + viewportCoordinate.Y) << 16) | ((rect.X + viewportCoordinate.X) & 0xFFFF))
				}) != TransparentControl.HTTRANSPARENT)) {
				clientToCtrl = localClientCoord;
				return true;
			} else
				return false;
		}

		private static IntPtr InvokeWndProc(Control ctrl, Message msg) {
			return ctrl.CallWndProc(ref msg);
		}

		/// <summary>
		/// Called when the window is activated.
		/// </summary>
		protected override void OnActivated(EventArgs e) {
			base.OnActivated(e);
			//InvokeOnGLThreadSync(new InvocationData(makeCurrent, GraphicsComponent), true);
			InvalidateGL(false);
		}

		private object CallViewSizeChanged(object param) {
			Rectangle openGLScissorBox = GLViewport;
			GL.Viewport(openGLScissorBox);
			GL.Scissor(openGLScissorBox.X, openGLScissorBox.Y, openGLScissorBox.Width, openGLScissorBox.Height);
			globalShader.Bind();
			OnViewSizeChanged();
			CallPaintGL(param);
			return null;
		}

		/// <summary>
		/// Called when the GDI layer is enabled. Be sure to call base.OnGdiEnabled() in any child classes *after* the GDI layer is initalized if applicable.
		/// </summary>
		protected virtual void OnGdiEnabled() {
		}

		private object MakeCurrent(object param) {
			GraphicsContext context = GLContext;
			if (context != null)
				context.MakeCurrent(param as GraphicsComponent);
			return null;
		}

		/// <summary>
		/// Transfers the OpenGL context to the main thread. Should only be called for debugging purposes.
		/// </summary>
		public void TransferGLToMainThread() {
			if (IsGLEnabled && IsHandleCreated) {
				lock (GLTransferSync) {
					if (GLThread == ResidentThread)
						return;
					InvokeOnGLThreadSync(new InvocationData(makeCurrent, null), 0, true);
					if (Thread.CurrentThread == ResidentThread) {
						GraphicsContext context = GLContext;
						if (context != null)
							context.MakeCurrent(GraphicsComponent);
					} else
						Invoke(new InvocationData(makeCurrent, GraphicsComponent));
				}
			}
		}

		/// <summary>
		/// Transfers the OpenGL context onto its own dedicated thead. Should only be called for debugging purposes.
		/// </summary>
		public void TransferGLToSeparateThread() {
			if (IsGLEnabled) {
				lock (GLTransferSync) {
					if (GLThread != ResidentThread)
						return;
					else if (Thread.CurrentThread == ResidentThread) {
						GraphicsContext context = GLContext;
						if (context != null)
							context.MakeCurrent(null as GraphicsComponent);
					} else
						Invoke(new InvocationData(makeCurrent, null));
					if (GLDispatcher == null)
						GLDispatcher = new DispatcherSlim(nameof(GLDispatcher), true, ExceptionMode.Log);
					GLDispatcher.Invoke(new InvocationData(makeCurrent, GraphicsComponent), 0, true);
				}
			}
		}

		/// <summary>
		/// Initializes an OpenGL rendering context on this window.
		/// </summary>
		/// <param name="onSeparateThread">Whether to dedicate a new thread specifically for issuing OpenGL commands (recommended).</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void InitializeGL(bool onSeparateThread) {
			InitializeGL(onSeparateThread, GraphicsMode.Empty, new MajorMinorVersion(3));
		}

		/// <summary>
		/// Initializes an OpenGL rendering context on this window.
		/// </summary>
		/// <param name="onSeparateThread">Whether to dedicate a new thread specifically for issuing OpenGL commands (recommended).</param>
		/// <param name="mode">The graphics mode to use.</param>
		/// <param name="version">The generation of the OpenGL context to try to create.</param>
		/// <param name="shareResources">Optionally sets a graphics context to share resources with.</param>
		public void InitializeGL(bool onSeparateThread, GraphicsMode mode, MajorMinorVersion version, GraphicsContext shareResources = null) {
			InitializeGL(this, onSeparateThread, mode, version, shareResources);
		}

		/// <summary>
		/// Initializes an OpenGL rendering context on this window.
		/// </summary>
		/// <param name="control">The control on which to initialize the OpenGL context.</param>
		/// <param name="onSeparateThread">Whether to dedicate a new thread specifically for issuing OpenGL commands (recommended).</param>
		/// <param name="mode">The graphics mode to use.</param>
		/// <param name="version">The generation of the OpenGL context to try to create.</param>
		/// <param name="shareResources">Optionally sets a graphics context to share resources with.</param>
		public void InitializeGL(Control control, bool onSeparateThread, GraphicsMode mode, MajorMinorVersion version, GraphicsContext shareResources = null) {
			if (DesignMode || IsGLEnabled)
				return;
			FullscreenGdiGLWorkaround = true;
			rpsStopwatch.Running = true;
			ReinitializeGL(control, onSeparateThread ? new DispatcherSlim(nameof(GLDispatcher), true, ExceptionMode.Log) : null, mode, version, shareResources);
		}

		/// <summary>
		/// Re-creates an OpenGL context. InitializeGL() HAS TO BE CALLED BEFORE THIS! ReinitializeGL() is automatically called if the window handle is re-created.
		/// </summary>
		/// <param name="dispatcher">The dispatcher to use for issuing OpenGL commands (if null, OpenGL will be initialized on the current thread).</param>
		/// <param name="mode">The graphics mode to use.</param>
		/// <param name="version">The generation of the OpenGL context to try to create.</param>
		/// <param name="shareResources">Optionally sets a graphics context to share resources with.</param>
		public void ReinitializeGL(DispatcherSlim dispatcher, GraphicsMode mode, MajorMinorVersion version, GraphicsContext shareResources) {
			ReinitializeGL(this, dispatcher, mode, version, shareResources);
		}

		/// <summary>
		/// Re-creates an OpenGL context. InitializeGL() HAS TO BE CALLED BEFORE THIS! ReinitializeGL() is automatically called if the window handle is re-created.
		/// </summary>
		/// <param name="control">The control on which to initialize the OpenGL context.</param>
		/// <param name="dispatcher">The dispatcher to use for issuing OpenGL commands (if null, OpenGL will be initialized on the current thread).</param>
		/// <param name="mode">The graphics mode to use.</param>
		/// <param name="version">The generation of the OpenGL context to try to create.</param>
		/// <param name="shareResources">Optionally sets a graphics context to share resources with.</param>
		public void ReinitializeGL(Control control, DispatcherSlim dispatcher, GraphicsMode mode, MajorMinorVersion version, GraphicsContext shareResources) {
			if (DesignMode)
				return;
			else if (control == null)
				control = this;
			control.SetStyle(ControlStyles.OptimizedDoubleBuffer, false);
			if (control != this) {
				control.Paint -= paintControl;
				control.Paint += paintControl;
			}
			lock (this) {
				graphicsComponent = new GraphicsComponent(control == null ? Handle : control.Handle, mode, version);
				GLDispatcher = dispatcher;
				if (dispatcher == null)
					InitGLInner(shareResources);
				else
					dispatcher.Invoke(new InvocationData(InitGLInner, shareResources), 0, true);
			}
		}

		private void Control_Paint(object sender, PaintEventArgs e) {
			InvalidateGL();
		}

		private void SetGdiSize(Size value, bool forceInvalidate = false) {
			if (value.Width < 1)
				value.Width = 1;
			if (value.Height < 1)
				value.Height = 1;
			if (value == gdiBounds.Size)
				return;
			bool invalidate = forceInvalidate || value.Width > gdiBounds.Width || value.Height > gdiBounds.Height;
			gdiBounds.Size = value;
			if (invalidate)
				InvalidateGdi(GdiRenderMode.GdiAsync);
		}

		/// <summary>
		/// Disposes of the OpenGL context.
		/// </summary>
		/// <param name="callOnUnload">Whether to unload the OpenGL resources along with the context.</param>
		public void DisposeGLContext(bool callOnUnload = true) {
			if (IsGLEnabled) {
				Unloading = true;
				DispatcherSlim dispatcher = GLDispatcher;
				GLDispatcher = null;
				Control ctrl = Control.FromHandle(graphicsComponent.Control);
				if (ctrl == null)
					ctrl = this;
				if (ctrl != this)
					ctrl.Paint -= paintControl;
				if (dispatcher == null)
					InvokeOnGLThreadSync(new InvocationData(unload, callOnUnload), 2500, false);
				else {
					dispatcher.Invoke(new InvocationData(unload, callOnUnload), 1500, false);
					dispatcher.Dispose();
				}
				Unloading = false;
				ctrl.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
				ctrl.Invalidate(false);
			}
		}

		private object CallUnload(object param) {
			if ((bool) param) {
				OnUnload();
				if (rectMesh != null) {
					rectMesh.Dispose();
					rectMesh = null;
				}
				if (globalShader != null) {
					globalShader.Dispose();
					globalShader = null;
				}
				if (GdiTexture != null) {
					GdiTexture.Dispose();
					GdiTexture = null;
				}
			}
			if (glContext != null) {
				try {
					glContext.Dispose();
					glContext = null;
				} catch {
				}
			}
			if (graphicsComponent != null) {
				try {
					graphicsComponent.Dispose();
					graphicsComponent = null;
				} catch {
				}
			}
			return null;
		}

		/// <summary>
		/// Called when the window is now starting to close.
		/// </summary>
		protected override void OnClosing() {
			if (updateTimer != null)
				UpdateTimerRunning = false;
			base.OnClosing();
		}

		/// <summary>
		/// Called when the form is closed.
		/// </summary>
		/// <param name="e">The close reason.</param>
		protected override void OnFormClosed(FormClosedEventArgs e) {
			DisposeGLContext(true);
			base.OnFormClosed(e);
		}

		private object InitGLInner(object param) {
			try {
				GraphicsContext shareContext = param as GraphicsContext;
				GraphicsContext oldGLContext = glContext;
				glContext = graphicsComponent.CreateContext(shareContext);
				glContext.MakeCurrent(graphicsComponent);
				if (shareContext != null && shareContext == oldGLContext)
					oldGLContext.Dispose();
				rectMesh = Mesh2D.CreateShared2DMeshRect();
				globalShader = new GlobalShader();
				globalShader.Bind();
				GL.ClearColor(BackColor);
				OnGLInitialized();
				GL.Enable(EnableCap.ScissorTest);
				CallViewSizeChanged(null);
			} catch (Exception e) {
				ErrorHandler.Show("GL failed to initialize.", e);
			}
			return null;
		}

		/// <summary>
		/// Called when the OpenGL context is initialized.
		/// </summary>
		protected virtual void OnGLInitialized() {
		}

		/// <summary>
		/// Draws the border at {0, 0} in the current OpenGL context (assumes the GL matrix is set to orthographic and maps to pixel coordinates,
		/// and that the current graphics context of the border is the same as the one it was created).
		/// </summary>
		public virtual void DrawBorderGL() {
		}

		/// <summary>
		/// Redraws the border
		/// </summary>
		/// <param name="sync">Whether to wait for the painting to be complete</param>
		/// <param name="rect">The invalidated bounds</param>
		protected override void RedrawBorder(bool sync, Rectangle rect) {
			if (IsGLEnabled) {
				if (RenderBorderOnGdiLayer) {
					if (sync) {
						bool invalidate = gdiCanvas == null;
						if (!invalidate) {
							lock (GdiSyncRoot) {
								if (gdiCanvas == null)
									invalidate = true;
								else {
									DrawBorderGdi(gdiCanvas, rect);
									invalidatedRect = Rectangle.Union(rect, invalidatedRect);
									InvalidateGL(false);
								}
							}
						}
						if (invalidate)
							InvalidateGdi(GdiRenderMode.CurrentSync, rect);
					} else
						CurrentGdiDispatcher.BeginInvoke(new InvocationData(callDrawBorder, rect));
				} else
					InvalidateGL(sync);
			} else
				base.RedrawBorder(sync, rect);
		}

		private object CallDrawBorder(object param) {
			Rectangle rect = (Rectangle) param;
			bool invalidate = gdiCanvas == null;
			if (!invalidate) {
				lock (GdiSyncRoot) {
					if (gdiCanvas == null)
						invalidate = true;
					else {
						DrawBorderGdi(gdiCanvas, rect);
						invalidatedRect = Rectangle.Union(rect, invalidatedRect);
						InvalidateGL(false);
					}
				}
			}
			if (invalidate)
				InvalidateGdi(GdiRenderMode.CurrentSync, rect);
			return null;
		}

		/// <summary>
		/// Causes the GDI layer to be redrawn.
		/// </summary>
		/// <param name="renderThread">The thread on which to render the GDI layer.</param>
		public void InvalidateGdi(GdiRenderMode renderThread = GdiRenderMode.GdiAsync) {
			InvalidateGdi(renderThread, new Rectangle(Point.Empty, gdiBounds.Size));
		}

		/// <summary>
		/// Causes the GDI layer to be redrawn. The rectangle is in client coordinates.
		/// </summary>
		/// <param name="renderThread">The thread on which to render the GDI layer</param>
		/// <param name="rect">The sub-rectangle to invalidate relative to the GDI layer (the smaller the area to update, the better)</param>
		public void InvalidateGdi(GdiRenderMode renderThread, Rectangle rect) {
			InvalidateGdi(renderThread, rect, true);
		}

		private void InvalidateGdi(GdiRenderMode renderThread, Rectangle rect, bool forceInvalidate) {
			if (IsDisposed || !gdiEnabled || (AnimatingBounds && !forceInvalidate))
				return;
			else if (!IsGLEnabled) {
				Point offset = ViewPortLocation + (Size) CurrentGdiLocation;
				rect.X -= offset.X;
				rect.Y -= offset.Y;
				Invalidate(rect, false);
			} else if (Volatile.Read(ref gdiQueueCount) < 2) {
				Interlocked.Increment(ref gdiQueueCount);
				switch (renderThread) {
					case GdiRenderMode.GdiAsync:
						CurrentGdiDispatcher.BeginInvoke(new InvocationData(invalidateGdiInner, new Tuple<Rectangle, bool>(rect, true)));
						break;
					case GdiRenderMode.MainAsync:
						if (Thread.CurrentThread == ResidentThread)
							InvalidateGdiInner(rect);
						else
							BeginInvoke(new InvocationData(invalidateGdiInner, rect));
						break;
					case GdiRenderMode.MainSync:
						if (Thread.CurrentThread == ResidentThread)
							InvalidateGdiInner(rect);
						else
							Invoke(new InvocationData(invalidateGdiInner, rect));
						break;
					case GdiRenderMode.GdiSync:
						CurrentGdiDispatcher.Invoke(new InvocationData(invalidateGdiInner, rect));
						break;
					default:
						InvalidateGdiInner(rect);
						break;
				}
			}
		}

		private object InvalidateGdiInner(object state) {
			if (IsClosing || !gdiEnabled || reentrant.Value)
				return null;
			reentrant.Value = true;
			if (invalidatedRect != Rectangle.Empty) {
				Interlocked.Exchange(ref forceRedraw, 1);
				InvalidateGL(false);
			}
			Rectangle rectToRedraw;
			Tuple<Rectangle, bool> param = null;
			if (state is Rectangle)
				rectToRedraw = (Rectangle) state;
			else {
				param = (Tuple<Rectangle, bool>) state;
				rectToRedraw = param.Item1;
			}
			if (gdiMask == null || maskSize.Width < gdiBounds.Width || maskSize.Height < gdiBounds.Height) {
				int width, height;
				if (gdiMask == null) {
					width = gdiBounds.Width;
					height = gdiBounds.Height;
				} else {
					width = Math.Max(gdiBounds.Width, maskSize.Width);
					height = Math.Max(gdiBounds.Height, maskSize.Height);
				}
				Size screenSize = ParentScreen.Bounds.Size;
				width = Math.Max(width, screenSize.Width + 1);
				height = Math.Max(height, screenSize.Height + 1);
				maskSize = new Size(width, height);
				lock (GdiSyncRoot) {
					if (gdiMask != null)
						gdiMask.Dispose();
					gdiMask = new Bitmap(width, height, Drawing.Imaging.PixelFormat.Format32bppPArgb);
					if (gdiCanvas != null)
						gdiCanvas.Dispose();
					gdiCanvas = Drawing.Graphics.FromImage(gdiMask);
				}
			}
			if (rectToRedraw.X < 0)
				rectToRedraw.X = 0;
			if (rectToRedraw.Y < 0)
				rectToRedraw.Y = 0;
			if (rectToRedraw.Right > gdiBounds.Width)
				rectToRedraw.Width = gdiBounds.Width - rectToRedraw.X;
			if (rectToRedraw.Bottom > gdiBounds.Height)
				rectToRedraw.Height = gdiBounds.Height - rectToRedraw.Y;
			if (rectToRedraw.Width <= 0 || rectToRedraw.Height <= 0) {
				reentrant.Value = false;
				Interlocked.Decrement(ref gdiQueueCount);
				return null;
			}
			lock (GdiSyncRoot) {
				if (gdiCanvas == null) {
					reentrant.Value = false;
					Interlocked.Decrement(ref gdiQueueCount);
					return null;
				}
				Region oldClipRegion = gdiCanvas.Clip;
				gdiCanvas.SetClip(rectToRedraw);
				Point offset = ViewPortLocation + (Size) CurrentGdiLocation;
				bool applyOffset = !offset.IsEmpty;
				if (applyOffset) {
					gdiCanvas.TranslateTransform(offset.X, offset.Y);
					rectToRedraw.X -= offset.X;
					rectToRedraw.Y -= offset.Y;
				}
				OnPaintGdi(gdiCanvas, rectToRedraw, true);
				if (applyOffset) {
					gdiCanvas.TranslateTransform(-offset.X, -offset.Y);
					rectToRedraw.X += offset.X;
					rectToRedraw.Y += offset.Y;
				}
				if (IsGLEnabled && RenderBorderOnGdiLayer && !AnimatingBounds)
					DrawBorderGdi(gdiCanvas, rectToRedraw);
				PaintEventHandler handler = GdiInvalidated;
				if (handler != null)
					handler(this, new PaintEventArgs(gdiCanvas, rectToRedraw));
				gdiCanvas.Clip = oldClipRegion;
				invalidatedRect = Rectangle.Union(rectToRedraw, invalidatedRect);
			}
			reentrant.Value = false;
			Interlocked.Decrement(ref gdiQueueCount);
			if (IsGLEnabled) {
				InvalidateGL(false);
				if (param != null && param.Item2 && ReduceGdiCpuUsage)
					Thread.Sleep(17);
			}
			return null;
		}

		/// <summary>
		/// Called when the form is enabled or disabled.
		/// </summary>
		protected override void OnEnabledChanged(EventArgs e) {
			base.OnEnabledChanged(e);
			InvalidateGL(false);
		}

		private bool GdiControlAdded(Control ctrl) {
			if (ctrl == null || GdiControls.Contains(ctrl))
				return false;
			GdiControls.Items.Add(ctrl);
			StyledForm form = ctrl as StyledForm;
			if (form != null)
				form.SetGdiVisible(true);
			if (!(DpiScale.Width == 1f && DpiScale.Height == 1f))
				ctrl.Scale(new SizeF(DpiScale.Width, DpiScale.Height));
			ctrl.Invalidated += GdiControlInvalidate;
			IntPtr handle = ctrl.Handle;
			InvalidateGdi(GdiRenderMode.GdiAsync, GetInvalidateRect(ctrl, ctrl.ClientRectangle), false);
			return false;
		}

		private bool GdiControlRemoved(Control ctrl) {
			if (ctrl == null)
				return false;
			int index = GdiControls.Items.IndexOf(ctrl);
			if (index == -1)
				return false;
			ctrl.Invalidated -= GdiControlInvalidate;
			Rectangle rect = GetInvalidateRect(ctrl, ctrl.ClientRectangle);
			GdiControls.Items.RemoveAt(index);
			StyledForm form = ctrl as StyledForm;
			if (form != null)
				form.SetGdiVisible(false);
			if (!(DpiScale.Width == 1f && DpiScale.Height == 1f))
				ctrl.Scale(new SizeF(1f / DpiScale.Width, 1f / DpiScale.Height));
			InvalidateGdi(GdiRenderMode.GdiAsync, rect);
			return false;
		}

		private void Ctrl_Invalidated(object sender, InvalidateEventArgs e) {
			Control control = sender as Control;
			if (control != null && !AnimatingBounds) {
				Rectangle rect = e.InvalidRect;
				rect.Offset(ViewPortLocation);
				InvalidateGdi(GdiRenderMode.GdiAsync, GetInvalidateRect(control, rect));
			}
		}

		private Rectangle GetInvalidateRect(Control control, Rectangle clientRect) {
			return new Rectangle(ViewPortLocation + (Size) Extensions.PointToScreen(control, clientRect.Location), clientRect.Size);
		}

		/// <summary>
		/// Destroys the underlying window handle.
		/// </summary>
		protected override void DestroyHandle() {
			if (Disposing) {
				gdiEnabled = false;
				lock (GdiSyncRoot) {
					if (gdiMask != null) {
						gdiMask.Dispose();
						gdiMask = null;
					}
					if (gdiCanvas != null) {
						gdiCanvas.Dispose();
						gdiCanvas = null;
					}
					if (GdiDispatcher != null) {
						GdiDispatcher.Dispose(true, 500);
						GdiDispatcher = null;
					}
					reentrant.Dispose();
				}
			}
			base.DestroyHandle();
		}

		/// <summary>
		/// Called when the GDI layer needs a repaint.
		/// </summary>
		/// <param name="g">The Graphics object to use to paint on.</param>
		/// <param name="clippingRect">The clipping rectangle that was invalidated.</param>
		/// <param name="clearBeforeRedraw">If true, the GDI layer is cleared before redraw.</param>
		protected virtual void OnPaintGdi(Drawing.Graphics g, Rectangle clippingRect, bool clearBeforeRedraw) {
			if (clearBeforeRedraw)
				g.Clear(Color.Transparent);
			g.DrawControls(GdiControls, Point.Empty, true);
		}

		/// <summary>
		/// Called whenever a GL render takes place.
		/// </summary>
		protected virtual void OnPaintGL() {
		}

		/// <summary>
		/// Called when the client size is changed.
		/// </summary>
		protected override void OnClientSizeChanged(EventArgs e) {
			Size newClientSize = ClientSize;
			if (clientSize == newClientSize)
				return;
			bool needsGdiRepaint = newClientSize.Width > clientSize.Width || newClientSize.Height > clientSize.Height;
			clientSize = newClientSize;
			base.OnClientSizeChanged(e);
			if (IsMinimized)
				return;
			else if (IsGLEnabled) {
				if (ReduceFlickerOnResize)
					InvokeOnGLThreadSync(new InvocationData(viewSizeChanged), 75, true);
				else
					InvokeOnGLThreadAsync(new InvocationData(viewSizeChanged), true);
				if (!AnimatingBounds && needsGdiRepaint && gdiEnabled)
					InvalidateGdi(GdiRenderMode.GdiAsync);
			} else if (!AnimatingBounds)
				Invalidate(false);
		}

		/// <summary>
		/// Called when the window is resized
		/// </summary>
		protected override void OnResize(EventArgs e) {
			base.OnResize(e);
			SetGdiSize(Size, false);
		}

		/// <summary>
		/// Called when resizing has ended.
		/// </summary>
		protected override void OnResizeEnd(EventArgs e) {
			base.OnResizeEnd(e);
			if (IsMinimized)
				return;
			else if (IsGLEnabled) {
				InvokeOnGLThreadAsync(new InvocationData(viewSizeChanged), true);
				if (gdiEnabled)
					InvalidateGdi(GdiRenderMode.GdiAsync);
			} else if (!AnimatingBounds)
				Invalidate(false);
		}

		/// <summary>
		/// Sets the visibility of the form to true or false depending on the parameter.
		/// </summary>
		/// <param name="value">Whether the form should be visible or not.</param>
		protected override void SetVisibleCore(bool value) {
			base.SetVisibleCore(value);
			if (value) {
				if (gdiEnabled)
					InvalidateGdi(GdiRenderMode.GdiAsync);
				else
					InvalidateGL(false);
			}
		}

		/// <summary>
		/// Called when the window needs to be re-drawn. If you are going to override this method, call this base method *AFTER* your drawing is completed.
		/// </summary>
		/// <param name="g">The Graphics object to use to paint on.</param>
		/// <param name="clippingRect">The clipping rectangle that was invalidated.</param>
		protected override void OnPaint(Drawing.Graphics g, Rectangle clippingRect) {
			base.OnPaint(g, clippingRect);
			if (gdiEnabled) {
				Point offset = CurrentGdiLocation;
				if (!offset.IsEmpty) {
					g.TranslateTransform(offset.X, offset.Y);
					clippingRect.X -= offset.X;
					clippingRect.Y -= offset.Y;
				}
				if (GdiRotation.X != 0f)
					g.RotateTransform(GdiRotation.X);
				lock (GdiSyncRoot)
					OnPaintGdi(g, clippingRect, false);
				if (GdiRotation.X != 0f)
					g.RotateTransform(-GdiRotation.X);
				if (!offset.IsEmpty)
					g.TranslateTransform(-offset.X, -offset.Y);
			}
		}

		/// <summary>
		/// Called whenever the view size has changed. This is called on the OpenGL thread.
		/// </summary>
		protected virtual void OnViewSizeChanged() {
		}

		/// <summary>
		/// Called when the window is being closed, but the context is still alive. Place GL-related cleanup code here.
		/// </summary>
		protected virtual void OnUnload() {
		}

		/// <summary>
		/// Called when the window is being disposed. BE SURE TO CALL base.OnDisposing(e) WHEN OVERRIDING!!
		/// </summary>
		/// <param name="e">Whether managed resources are about to be disposed.</param>
		protected override void OnDisposing(DisposeFormEventArgs e) {
			if (updateTimer != null)
				UpdateTimerRunning = false;
			if (e.DisposeMode == DisposeOptions.FullDisposal) {
				if (updateTimer != null) {
					updateTimer.Dispose();
					updateTimer = null;
				}
				foreach (Control ctrl in GdiControls) {
					if (!ctrl.IsDisposed)
						ctrl.Dispose();
				}
				GdiControls.Clear();
			}
			base.OnDisposing(e);
		}
	}
}