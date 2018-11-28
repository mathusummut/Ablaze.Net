using System.Drawing;
using System.Platforms.Windows;
using System.Threading;
using System.Runtime.CompilerServices;

namespace System.Windows.Forms {
	/// <summary>
	/// A C# API Layer for a global mouse hook.
	/// </summary>
	public static class MouseListener {
		/// <summary>
		/// An empty mouse event.
		/// </summary>
		public static readonly MouseEventArgs Empty = new MouseEventArgs(MouseButtons.None, 0, 0, 0, 0);
		/// <summary>
		/// The empty sender object.
		/// </summary>
		public static readonly object Sender = new object();
		/// <summary>
		/// If true, the current/next mouse event is consumed and won't propogate any further.
		/// Set to true if you change the cursor coordinates in a hook callback.
		/// </summary>
		public static bool ConsumeEvent;
		internal static IntPtr ModuleHandle = NativeApi.LoadLibrary("user32.dll");
		private static event MouseEventHandler mouseDown, cursorMove, mouseMove, mouseUp, mouseWheel, mouseDoubleClick;
		internal static object SyncLock = new object();
		private static IntPtr One = new IntPtr(1);
		private static DateTime lastTime;
		private static MouseEventArgs lastEventArgs = Empty;
		private static NativeApi.LowLevelMouseProc proc = HookCallback;
		private static IntPtr hookID;
		private static Point lastCurPos;
		private static EventHandler dispose = Dispose;
		private static int delta, clicks;
		private static bool enabled, hasDoubleClicked, disableTouchMouseEvents;
		private static Thread CurrentThread;

		/// <summary>
		/// Gets or sets whether to disable auto-generated mouse events caused by touch input.
		/// </summary>
		public static bool DisableTouchMouseEvents {
			get {
				return disableTouchMouseEvents;
			}
			set {
				if (value == disableTouchMouseEvents)
					return;
				lock (SyncLock) {
					disableTouchMouseEvents = true;
					Enabled = true;
				}
			}
		}

		/// <summary>
		/// Fired when a mouse button is pressed. Duplicate (same method) subscriptions are ignored.
		/// </summary>
		public static event MouseEventHandler MouseDown {
			add {
				if (value == null)
					return;
				lock (SyncLock) {
					mouseDown -= value;
					mouseDown += value;
					Enabled = true;
				}
			}
			remove {
				if (value == null)
					return;
				lock (SyncLock) {
					mouseDown -= value;
					CheckEnable();
				}
			}
		}

		/// <summary>
		/// Fired when the mouse is cursor has moved on screen. 'e' will hold the screen cursor position.
		/// Duplicate (same method) subscriptions are ignored.
		/// </summary>
		public static event MouseEventHandler CursorMove {
			add {
				if (value == null)
					return;
				lock (SyncLock) {
					cursorMove -= value;
					cursorMove += value;
					Enabled = true;
				}
			}
			remove {
				if (value == null)
					return;
				lock (SyncLock) {
					cursorMove -= value;
					CheckEnable();
				}
			}
		}

		/// <summary>
		/// Fired when the mouse has physically been moved irrespective of cursor movement and display bounds. 'e' will hold the unbounded cursor position.
		///  Duplicate (same method) subscriptions are ignored.
		/// </summary>
		public static event MouseEventHandler MouseMove {
			add {
				if (value == null)
					return;
				lock (SyncLock) {
					mouseMove -= value;
					mouseMove += value;
					Enabled = true;
				}
			}
			remove {
				if (value == null)
					return;
				lock (SyncLock) {
					mouseMove -= value;
					CheckEnable();
				}
			}
		}

		/// <summary>
		/// Fired when a mouse button is released. Duplicate (same method) subscriptions are ignored.
		/// </summary>
		public static event MouseEventHandler MouseUp {
			add {
				if (value == null)
					return;
				lock (SyncLock) {
					mouseUp -= value;
					mouseUp += value;
					Enabled = true;
				}
			}
			remove {
				if (value == null)
					return;
				lock (SyncLock) {
					mouseUp -= value;
					CheckEnable();
				}
			}
		}

		/// <summary>
		/// Fired when a mouse wheel is scrolled. Duplicate (same method) subscriptions are ignored.
		/// </summary>
		public static event MouseEventHandler MouseWheel {
			add {
				if (value == null)
					return;
				lock (SyncLock) {
					mouseWheel -= value;
					mouseWheel += value;
					Enabled = true;
				}
			}
			remove {
				if (value == null)
					return;
				lock (SyncLock) {
					mouseWheel -= value;
					CheckEnable();
				}
			}
		}

		/// <summary>
		/// Fired when a mouse button is double-clicked. Duplicate (same method) subscriptions are ignored.
		/// </summary>
		public static event MouseEventHandler MouseDoubleClick {
			add {
				if (value == null)
					return;
				lock (SyncLock) {
					mouseDoubleClick -= value;
					mouseDoubleClick += value;
					Enabled = true;
				}
			}
			remove {
				if (value == null)
					return;
				lock (SyncLock) {
					mouseDoubleClick -= value;
					CheckEnable();
				}
			}
		}

		/// <summary>
		/// Gets whether the currently executing code is within a global callback hook.
		/// </summary>
		public static bool CalledFromHook {
			get {
				return Thread.CurrentThread == CurrentThread;
			}
		}

		/// <summary>
		/// Gets the last native mouse event.
		/// </summary>
		public static MouseMessages LastMouseEvent {
			get;
			private set;
		}

		/// <summary>
		/// Gets the current mouse state.
		/// </summary>
		public static MouseEventArgs State {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				Point pos = Cursor.Position;
				return new MouseEventArgs(MouseButtons.None, clicks, pos.X, pos.Y, delta);
			}
		}

		/// <summary>
		/// Gets the last cursor movement offset vector (bounded by display size, can be 0).
		/// </summary>
		public static Point LastCursorMovement {
			get;
			private set;
		}

		/// <summary>
		/// Gets the last mouse movement offset vector (not bounded by display size).
		/// </summary>
		public static Point LastMouseMovement {
			get;
			private set;
		}

		/// <summary>
		/// Whether the key press hook is enabled.
		/// </summary>
		private static bool Enabled {
			get {
				return enabled;
			}
			set {
				if (value == enabled || StyledForm.DesignMode)
					return;
				else if (value) {
					hookID = NativeApi.SetWindowsHookEx(14, proc, ModuleHandle, 0u);
					Application.ApplicationExit += dispose;
				} else {
					Application.ApplicationExit -= dispose;
					NativeApi.UnhookWindowsHookEx(hookID);
				}
				enabled = value;
			}
		}

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		private static void CheckEnable() {
			if (!disableTouchMouseEvents && mouseDown == null && cursorMove == null && mouseMove == null && mouseUp == null && mouseWheel == null && mouseDoubleClick == null)
				Enabled = false;
		}

		/// <summary>
		/// Gets whether the specified button is currently pressed.
		/// </summary>
		/// <param name="button">The button to check.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool IsButtonDown(MouseButtons button) {
			return NativeApi.GetAsyncKeyState(ToKey(button)) < 0;
		}

		/// <summary>
		/// Gets whether the specified button is toggled (ie. pressed and released back to back).
		/// </summary>
		/// <param name="button">The button to check.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool IsButtonToggled(MouseButtons button) {
			return (NativeApi.GetAsyncKeyState(ToKey(button)) & 1) == 1;
		}

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		private static Keys ToKey(MouseButtons button) {
			switch (button) {
				case MouseButtons.Left:
					return Keys.LButton;
				case MouseButtons.Right:
					return Keys.RButton;
				case MouseButtons.Middle:
					return Keys.MButton;
				case MouseButtons.XButton1:
					return Keys.XButton1;
				case MouseButtons.XButton2:
					return Keys.XButton2;
				default:
					return Keys.None;
			}
		}

		private static bool IsDoubleClick(MouseEventArgs e) {
			DateTime now = DateTime.Now;
			TimeSpan elapsed = now - lastTime;
			lastTime = now;
			bool returnValue = e.Button == lastEventArgs.Button &&
				elapsed.Milliseconds <= SystemInformation.DoubleClickTime &&
				Math.Abs(e.X - lastEventArgs.X) <= 5 &&
				Math.Abs(e.Y - lastEventArgs.Y) <= 5;
			lastEventArgs = e;
			if (returnValue && hasDoubleClicked) {
				hasDoubleClicked = false;
				returnValue = false;
			}
			hasDoubleClicked = false;
			return returnValue;
		}

		private static IntPtr HookCallback(int nCode, IntPtr wParam, ref MSLLHOOKSTRUCT lParam) {
			if (nCode >= 0) {
				MouseEventArgs e;
				CurrentThread = Thread.CurrentThread;
				if (disableTouchMouseEvents && unchecked((((uint) (ulong) lParam.dwExtraInfo) & 0xFF515700) == 0xFF515700)) //MOUSEEVENTF_FROMTOUCH
					return One;
				LastMouseEvent = (MouseMessages) wParam.ToInt32();
				switch (LastMouseEvent) {
					case MouseMessages.LBUTTONDOWN:
						RaiseMouseEvent(mouseDown, new MouseEventArgs(MouseButtons.Left, clicks, lParam.pt.X, lParam.pt.Y, delta));
						break;
					case MouseMessages.LBUTTONUP:
						e = new MouseEventArgs(MouseButtons.Left, ++clicks, lParam.pt.X, lParam.pt.Y, delta);
						RaiseMouseEvent(mouseUp, e);
						if (IsDoubleClick(e))
							RaiseMouseEvent(mouseDoubleClick, e);
						break;
					case MouseMessages.MOUSEMOVE:
						LastMouseMovement = new Point(lParam.pt.X - lastCurPos.X, lParam.pt.Y - lastCurPos.Y);
						Point pos = Cursor.Position;
						LastCursorMovement = new Point(pos.X - lastCurPos.X, pos.Y - lastCurPos.Y);
						MouseButtons buttons = GetButtonsPressed();
						RaiseMouseEvent(mouseMove, new MouseEventArgs(buttons, clicks, lParam.pt.X, lParam.pt.Y, delta));
						if (pos != lastCurPos) {
							lastCurPos = pos;
							RaiseMouseEvent(cursorMove, new MouseEventArgs(buttons, clicks, pos.X, pos.Y, delta));
						}
						break;
					case MouseMessages.MOUSEWHEEL:
						delta = unchecked((short) ((ushort) (lParam.mouseData >> 16)));
						RaiseMouseEvent(mouseWheel, new MouseEventArgs(MouseButtons.Middle, clicks, lParam.pt.X, lParam.pt.Y, delta));
						break;
					case MouseMessages.RBUTTONDOWN:
						RaiseMouseEvent(mouseDown, new MouseEventArgs(MouseButtons.Right, clicks, lParam.pt.X, lParam.pt.Y, delta));
						break;
					case MouseMessages.RBUTTONUP:
						e = new MouseEventArgs(MouseButtons.Right, ++clicks, lParam.pt.X, lParam.pt.Y, delta);
						RaiseMouseEvent(mouseUp, e);
						if (IsDoubleClick(e))
							RaiseMouseEvent(mouseDoubleClick, e);
						break;
					case MouseMessages.MBUTTONDOWN:
						RaiseMouseEvent(mouseDown, new MouseEventArgs(MouseButtons.Middle, clicks, lParam.pt.X, lParam.pt.Y, delta));
						break;
					case MouseMessages.MBUTTONUP:
						e = new MouseEventArgs(MouseButtons.Middle, ++clicks, lParam.pt.X, lParam.pt.Y, delta);
						RaiseMouseEvent(mouseUp, e);
						if (IsDoubleClick(e))
							RaiseMouseEvent(mouseDoubleClick, e);
						break;
				}
				CurrentThread = null;
				if (ConsumeEvent) {
					ConsumeEvent = false;
					return One;
				}
			}
			return NativeApi.CallNextHookEx(hookID, nCode, wParam, ref lParam);
		}

		/// <summary>
		/// Gets the mouse buttons that are currently pressed.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static MouseButtons GetButtonsPressed() {
			MouseButtons pressed = MouseButtons.None;
			if (KeyListener.IsKeyDown(Keys.LButton))
				pressed |= MouseButtons.Left;
			if (KeyListener.IsKeyDown(Keys.RButton))
				pressed |= MouseButtons.Right;
			if (KeyListener.IsKeyDown(Keys.MButton))
				pressed |= MouseButtons.Middle;
			if (KeyListener.IsKeyDown(Keys.XButton1))
				pressed |= MouseButtons.XButton1;
			if (KeyListener.IsKeyDown(Keys.XButton2))
				pressed |= MouseButtons.XButton2;
			return pressed;
		}

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		private static void RaiseMouseEvent(MouseEventHandler handler, MouseEventArgs args) {
			if (handler != null) {
				try {
					handler(Sender, args);
				} catch {
				}
			}
		}

		private static void Dispose(object sender, EventArgs e) {
			Enabled = false;
		}
	}
}