using System.Collections.Generic;
using System.Threading;
using System.Platforms.Windows;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Windows.Forms {
	/// <summary>
	/// A C# API Layer for a global key hook.
	/// </summary>
	public static class KeyListener {
		private static event KeyEventHandler keyPress, keyRelease;
		private static object SyncLock = new object();
		/// <summary>
		/// If true, the current/next event is consumed and won't propogate any further.
		/// </summary>
		public static bool ConsumeEvent;
		/// <summary>
		/// Delegates the method called on key event.
		/// </summary>
		/// <param name="value">The key involved represented as a string.</param>
		/// <param name="isChar">Whether the key involved is a character key.</param>
		/// <param name="key">The key involved.</param>
		public delegate void KeyEventHandler(string value, bool isChar, Keys key);
		private static bool enabled, lastWasDeadKey, DeadKeyOver;
		private static List<object[]> DeadKeys = new List<object[]>();
		private static IntPtr WM_KEYDOWN = new IntPtr(0x0100);
		private static IntPtr WM_SYSKEYDOWN = new IntPtr(0x0104);
		private static IntPtr WM_KEYUP = new IntPtr(0x101);
		private static IntPtr WM_SYSKEYUP = new IntPtr(0x105);
		private static NativeApi.LowLevelKeyboardProc proc = HookCallback;
		private static EventHandler dispose = Dispose;
		private static IntPtr hookID;
		private static Thread CurrentThread;

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
					if (keyPress == null && keyRelease == null)
						Enabled = false;
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
					if (keyPress == null && keyRelease == null)
						Enabled = false;
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
		/// Gets or sets whether the key press hook is enabled.
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
				else if (value) {
					hookID = NativeApi.SetWindowsHookEx(13, proc, MouseListener.ModuleHandle, 0u);
					Application.ApplicationExit += dispose;
				} else {
					Application.ApplicationExit -= dispose;
					NativeApi.UnhookWindowsHookEx(hookID);
				}
				enabled = value;
			}
		}

		/// <summary>
		/// Gets whether the specified key is currently pressed.
		/// </summary>
		/// <param name="key">The key to check.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool IsKeyDown(Keys key) {
			return NativeApi.GetAsyncKeyState(key) < 0;
		}

		/// <summary>
		/// Gets whether the specified key is toggled (ie. pressed and released back to back).
		/// </summary>
		/// <param name="key">The key to check.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool IsKeyToggled(Keys key) {
			return (NativeApi.GetAsyncKeyState(key) & 1) == 1;
		}

		private static IntPtr HookCallback(int nCode, IntPtr wParam, ref KBDLLHOOKSTRUCT lParam) {
			if (nCode >= 0) {
				CurrentThread = Thread.CurrentThread;
				try {
					if ((wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN) && keyPress != null)
						ProcessKeys((uint) lParam.vkCode, (uint) lParam.scanCode, true);
					else if ((wParam == WM_KEYUP || wParam == WM_SYSKEYUP) && keyRelease != null)
						ProcessKeys((uint) lParam.vkCode, (uint) lParam.scanCode, false);
				} catch {
				}
				CurrentThread = null;
				if (ConsumeEvent) {
					ConsumeEvent = false;
					return new IntPtr(1);
				}
			}
			return NativeApi.CallNextHookEx(hookID, nCode, wParam, ref lParam);
		}

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		private static bool IsDeadKey(uint vkCode) {
			return (NativeApi.MapVirtualKey(vkCode, MapVirtualKeyType.VirtualKeyToCharacter) & 0x80000000) != 0;
		}

		private static byte[] GetKeyboardState() {
			byte[] result = new byte[256];
			for (int i = 0; i < result.Length; i++)
				result[i] = (byte) NativeApi.GetAsyncKeyState((Keys) i);
			return result;
		}

		private static void ProcessKeys(uint vkcode, uint nScanCode, bool isDown) {
			if (IsDeadKey(vkcode)) {
				lastWasDeadKey = true;
				DeadKeys.Add(new object[] { vkcode, nScanCode, isDown, GetKeyboardState() });
				return;
			}
			if (lastWasDeadKey) {
				DeadKeyOver = true;
				lastWasDeadKey = false;
				DeadKeys.Add(new object[] { vkcode, nScanCode, isDown, GetKeyboardState() });
				return;
			}
			if (DeadKeyOver) {
				object[] objArray;
				for (int i = 0; i < DeadKeys.Count; i++) {
					objArray = DeadKeys[i];
					RaiseAppropriateEvent((uint) objArray[0], (uint) objArray[1], (bool) objArray[2], (byte[]) objArray[3]);
					if (IsDeadKey((uint) objArray[0]))
						NativeApi.ToAscii(vkcode, nScanCode, (byte[]) objArray[3], new StringBuilder(2), 0);
				}
				DeadKeys.Clear();
			}
			RaiseAppropriateEvent(vkcode, nScanCode, isDown, GetKeyboardState());
		}

		private static void RaiseAppropriateEvent(uint vkcode, uint nScanCode, bool isDown, byte[] keyboardState) {
			string result = ((Keys) vkcode).ToString();
			if (vkcode >= 0x20 && Control.ModifierKeys != Keys.Control) {
				StringBuilder szKey = new StringBuilder(2);
				DeadKeyOver = false;
				if ((uint) NativeApi.ToAscii(vkcode, nScanCode, keyboardState, szKey, 0) > 0)
					result = szKey.ToString().Substring(0, 1);
			}
			KeyEventHandler handler = isDown ? keyPress : keyRelease;
			if (handler != null)
				handler(result, result.Length == 1, (Keys) vkcode);
		}

		private static void Dispose(object sender, EventArgs e) {
			Enabled = false;
		}
	}
}