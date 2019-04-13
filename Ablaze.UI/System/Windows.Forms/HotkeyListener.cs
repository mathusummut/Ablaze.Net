using System.Collections.Generic;
using System.Platforms.Windows;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System.Windows.Forms {
	/// <summary>
	/// A C# API Layer for handling global hotkeys.
	/// </summary>
	public static class HotkeyListener {
		private delegate bool RegisterHotKeyDelegate(IntPtr hwnd, int id, uint modifiers, uint key);
		private delegate bool UnregisterHotKeyDelegate(IntPtr hwnd, int id);

		private static Delegate Register = new RegisterHotKeyDelegate(NativeApi.RegisterHotKey), Unregister = new UnregisterHotKeyDelegate(NativeApi.UnregisterHotKey);
		private static Dictionary<uint, Hotkey> Hotkeys = new Dictionary<uint, Hotkey>();
		private static WaitCallback raiseHotkey = RaiseHotkey;
		private static object SyncRoot = new object();
		private static bool enabled, firstTime = true;
		private static ManualResetEventSlim resetEvent = new ManualResetEventSlim();
		private static EventHandler idle = Application_Idle;
		private static IntPtr handle;
		private static MessageWindow window;
		private static int ID;
		private static event Action<Hotkey> hotkeyPressed;

		/// <summary>
		/// Fired when the registered hotkey combination is pressed. Duplicate (same method) subscriptions are ignored.
		/// </summary>
		public static event Action<Hotkey> HotkeyPressed {
			add {
				if (value == null)
					return;
				lock (SyncRoot) {
					hotkeyPressed -= value;
					hotkeyPressed += value;
					Enabled = true;
				}
			}
			remove {
				lock (SyncRoot)
					hotkeyPressed -= value;
			}
		}

		/// <summary>
		/// Gets or sets whether the hotkey listener is enabled.
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
				enabled = value;
				if (value) {
					if (window == null) {
						Thread messageLoopThread = new Thread(InitWindow);
						messageLoopThread.SetApartmentState(ApartmentState.STA);
						messageLoopThread.Name = "HotkeyListenerThread";
						messageLoopThread.IsBackground = true;
						messageLoopThread.Start();
						if (resetEvent != null) {
							resetEvent.Wait(5000);
							resetEvent = null;
						}
					}
					if (firstTime) {
						firstTime = false;
						Application.ApplicationExit += Application_ApplicationExit;
					}
				}
			}
		}

		private static void InitWindow() {
			window = new MessageWindow();
			Application.Idle += idle;
			MessageLoop.Run(window);
		}

		private static void Application_Idle(object sender, EventArgs e) {
			Application.Idle -= idle;
			idle = null;
			resetEvent.Set();
		}

		private static void Application_ApplicationExit(object sender, EventArgs e) {
			enabled = false;
			if (window == null)
				return;
			lock (SyncRoot) {
				foreach (Hotkey hotkey in Hotkeys.Values)
					window.Invoke(Unregister, handle, hotkey.ID);
				Hotkeys.Clear();
			}
		}

		/// <summary>
		/// Registers the specified hotkey.
		/// </summary>
		/// <param name="hotkey">The hotkey to register.</param>
		public static void RegisterHotKey(Hotkey hotkey) {
			Enabled = true;
			hotkey.ID = 0;
			uint hash = hotkey.TrueHash;
			int id;
			lock (SyncRoot) {
				if (Hotkeys.ContainsKey(hash))
					return;
				id = ++ID;
				if (window != null) {
					if (!((bool) window.Invoke(Register, handle, id, (uint) hotkey.Modifiers, (uint) hotkey.Key))) {
						if ((hotkey.Modifiers & KeyModifiers.NoRepeat) == KeyModifiers.NoRepeat) {
							if (!((bool) window.Invoke(Register, handle, id, (uint) (hotkey.Modifiers & ~KeyModifiers.NoRepeat), (uint) hotkey.Key)))
								return;
						} else
							return;
					}
				}
				hotkey.ID = id;
				Hotkeys.Add(hash, hotkey);
			}
		}

		/// <summary>
		/// Unregisters the specified hotkey.
		/// </summary>
		/// <param name="hotkey">The hotkey to unregister.</param>
		public static void UnregisterHotKey(Hotkey hotkey) {
			uint hash = hotkey.TrueHash;
			hotkey.ID = 0;
			lock (SyncRoot) {
				Hotkey keyID;
				if (Hotkeys.TryGetValue(hash, out keyID))
					hotkey.ID = keyID.ID;
				else
					return;
			}
			if (hotkey.ID != 0) {
				if (window != null)
					window.Invoke(Unregister, handle, hotkey.ID);
				hotkey.ID = 0;
			}
			lock (SyncRoot)
				Hotkeys.Remove(hash);
		}

		private static void RaiseHotkey(object key) {
			Action<Hotkey> handler = hotkeyPressed;
			if (handler != null) {
				try {
					handler((Hotkey) key);
				} catch {
				}
			}
		}

		private sealed class MessageWindow : Form {
			public MessageWindow() {
				CheckForIllegalCrossThreadCalls = false;
				handle = Handle;
			}

			protected override void WndProc(ref Message m) {
				if (m.Msg == (int) WindowMessage.HOTKEY && enabled) {
					uint trueHash = (uint) m.LParam.ToInt64();
					if (hotkeyPressed != null)
						ThreadPool.UnsafeQueueUserWorkItem(raiseHotkey, Hotkeys.ContainsKey(trueHash) ? Hotkeys[trueHash] : new Hotkey(trueHash));
				}
				base.WndProc(ref m);
			}

			protected override void SetVisibleCore(bool value) {
				base.SetVisibleCore(false);
			}

			protected override void OnFormClosing(FormClosingEventArgs e) {
				Application_ApplicationExit(this, EventArgs.Empty);
				base.OnFormClosing(e);
			}

			protected override void OnHandleDestroyed(EventArgs e) {
				base.OnHandleDestroyed(e);
				window = null;
			}
		}
	}
}