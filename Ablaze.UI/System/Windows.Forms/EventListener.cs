using System.Drawing;
using System.Platforms.Windows;
using System.Runtime.CompilerServices;

namespace System.Windows.Forms {
	/// <summary>
	/// Listens to application events (usually keyboard and mouse input).
	/// </summary>
	public class EventListener : IMessageFilter {
		private static EventListener listener = new EventListener();
		private static object SyncLock = new object();
		private static event KeyEventHandler keyPress, keyRelease;
		private static event MouseEventHandler mouseDown, cursorMove, mouseUp, mouseWheel, mouseDoubleClick;
		private static MouseEventArgs lastEventArgs = MouseListener.Empty;
		private static DateTime lastTime;
		private static int delta, clicks;
		private static bool hasDoubleClicked, enabled;
		/// <summary>
		/// If true, the current/next event is consumed and won't propogate any further.
		/// </summary>
		public static bool ConsumeEvent;

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
		/// Fired when a key is pressed. Duplicate (same method) subscriptions are ignored.
		/// </summary>
		public static event KeyEventHandler KeyPress {
			add {
				if (value == null)
					return;
				lock (SyncLock) {
					keyPress -= value;
					keyPress += value;
					Enabled = true;
				}
			}
			remove {
				if (value == null)
					return;
				lock (SyncLock) {
					keyPress -= value;
					CheckEnable();
				}
			}
		}

		/// <summary>
		/// Fired when a key is released. Duplicate (same method) subscriptions are ignored.
		/// </summary>
		public static event KeyEventHandler KeyRelease {
			add {
				if (value == null)
					return;
				lock (SyncLock) {
					keyRelease -= value;
					keyRelease += value;
					Enabled = true;
				}
			}
			remove {
				if (value == null)
					return;
				lock (SyncLock) {
					keyRelease -= value;
					CheckEnable();
				}
			}
		}

		/// <summary>
		/// Gets the last application event.
		/// </summary>
		[CLSCompliant(false)]
		public static WindowMessage LastEvent {
			get;
			private set;
		}

		/// <summary>
		/// Whether the key press hook is enabled.
		/// </summary>
		public static bool Enabled {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return enabled;
			}
			private set {
				if (value == enabled || StyledForm.DesignMode)
					return;
				else if (value)
					Application.AddMessageFilter(listener);
				else
					Application.RemoveMessageFilter(listener);
				enabled = value;
			}
		}

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		private static void CheckEnable() {
			if (mouseDown == null && cursorMove == null && mouseUp == null && mouseWheel == null && mouseDoubleClick == null && keyPress == null && keyRelease == null)
				Enabled = false;
		}

		private EventListener() {
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
			if (returnValue) {
				if (hasDoubleClicked) {
					hasDoubleClicked = false;
					returnValue = false;
				}
			}
			hasDoubleClicked = false;
			return returnValue;
		}

		/// <summary>
		/// Processes the specified message and fires the appropriate event.
		/// </summary>
		/// <param name="m">The message to filter.</param>
		public bool PreFilterMessage(ref Message m) {
			MouseEventArgs e;
			Point point;
			Keys keyData;
			switch (LastEvent = (WindowMessage) m.Msg) {
				case WindowMessage.LBUTTONDOWN:
					point = Cursor.Position;
					RaiseMouseEvent(mouseDown, new MouseEventArgs(MouseButtons.Left, clicks, point.X, point.Y, delta));
					break;
				case WindowMessage.LBUTTONUP:
					point = Cursor.Position;
					e = new MouseEventArgs(MouseButtons.Left, ++clicks, point.X, point.Y, delta);
					RaiseMouseEvent(mouseUp, e);
					if (IsDoubleClick(e))
						RaiseMouseEvent(mouseDoubleClick, e);
					break;
				case WindowMessage.MOUSEMOVE:
					MouseButtons buttons = MouseListener.GetButtonsPressed();
					point = Cursor.Position;
					RaiseMouseEvent(cursorMove, new MouseEventArgs(buttons, clicks, point.X, point.Y, delta));
					break;
				case WindowMessage.MOUSEWHEEL:
					delta = unchecked((short) ((ushort) ((ulong) m.WParam >> 16)));
					point = Cursor.Position;
					RaiseMouseEvent(mouseWheel, new MouseEventArgs(MouseButtons.Middle, clicks, point.X, point.Y, delta));
					break;
				case WindowMessage.RBUTTONDOWN:
					point = Cursor.Position;
					RaiseMouseEvent(mouseDown, new MouseEventArgs(MouseButtons.Right, clicks, point.X, point.Y, delta));
					break;
				case WindowMessage.RBUTTONUP:
					point = Cursor.Position;
					e = new MouseEventArgs(MouseButtons.Right, ++clicks, point.X, point.Y, delta);
					RaiseMouseEvent(mouseUp, e);
					if (IsDoubleClick(e))
						RaiseMouseEvent(mouseDoubleClick, e);
					break;
				case WindowMessage.MBUTTONDOWN:
					point = Cursor.Position;
					RaiseMouseEvent(mouseDown, new MouseEventArgs(MouseButtons.Middle, clicks, point.X, point.Y, delta));
					break;
				case WindowMessage.MBUTTONUP:
					point = Cursor.Position;
					e = new MouseEventArgs(MouseButtons.Middle, ++clicks, point.X, point.Y, delta);
					RaiseMouseEvent(mouseUp, e);
					if (IsDoubleClick(e))
						RaiseMouseEvent(mouseDoubleClick, e);
					break;

				case WindowMessage.KEYDOWN:
				case WindowMessage.SYSKEYDOWN:
					keyData = (Keys) m.WParam;
					if (KeyListener.IsKeyDown(Keys.ControlKey))
						keyData |= Keys.Control;
					if (KeyListener.IsKeyDown(Keys.ShiftKey))
						keyData |= Keys.Shift;
					if (KeyListener.IsKeyDown(Keys.Menu))
						keyData |= Keys.Alt;
					RaiseKeyEvent(keyPress, new KeyEventArgs(keyData));
					break;
				case WindowMessage.KEYUP:
				case WindowMessage.SYSKEYUP:
					keyData = (Keys) m.WParam;
					if (KeyListener.IsKeyDown(Keys.ControlKey))
						keyData |= Keys.Control;
					if (KeyListener.IsKeyDown(Keys.ShiftKey))
						keyData |= Keys.Shift;
					if (KeyListener.IsKeyDown(Keys.Menu))
						keyData |= Keys.Alt;
					RaiseKeyEvent(keyRelease, new KeyEventArgs(keyData));
					break;
			}
			if (ConsumeEvent) {
				ConsumeEvent = false;
				return true;
			} else
				return false;
		}

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		private void RaiseMouseEvent(MouseEventHandler handler, MouseEventArgs args) {
			if (handler != null) {
				try {
					handler(this, args);
				} catch {
				}
			}
		}

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		private void RaiseKeyEvent(KeyEventHandler handler, KeyEventArgs args) {
			if (handler != null) {
				try {
					handler(this, args);
				} catch {
				}
			}
		}
	}
}
