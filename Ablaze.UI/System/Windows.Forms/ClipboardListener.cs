using System.Platforms.Windows;
using System.Runtime.CompilerServices;

namespace System.Windows.Forms {
	/// <summary>
	/// Monitors the clipboard, and raises an event whenever its contents change.
	/// </summary>
	public static class ClipboardListener {
		private static EventHandler dispose = Dispose;
		private static IntPtr parent = new IntPtr(-3);
		private static ClipboardNativeWindow nativeMonitor;
		private static event Action clipboardContentChanged;
		private static object SyncRoot = new object();

		/// <summary>
		/// Fired when the clipboard content has changed. Duplicate (same method) subscriptions are ignored.
		/// </summary>
		public static event Action ClipboardContentChanged {
			add {
				if (value == null)
					return;
				lock (SyncRoot) {
					clipboardContentChanged -= value;
					clipboardContentChanged += value;
					Enabled = true;
				}
			}
			remove {
				lock (SyncRoot)
					clipboardContentChanged -= value;
			}
		}

		/// <summary>
		/// Gets or sets whether the clipboard is monitored.
		/// </summary>
		public static bool Enabled {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return nativeMonitor != null;
			}
			private set {
				if (value == (nativeMonitor != null))
					return;
				if (value) {
					nativeMonitor = new ClipboardNativeWindow();
					CreateParams createParam = new CreateParams() {
						Style = 0,
						ExStyle = 0,
						ClassStyle = 0,
						Caption = nameof(ClipboardListener),
						Parent = parent
					};
					nativeMonitor.CreateHandle(createParam);
					Application.ApplicationExit += dispose;
				}
			}
		}

		private static void Dispose(object sender, EventArgs e) {
			Application.ApplicationExit -= dispose;
			if (nativeMonitor != null) {
				nativeMonitor.Dispose();
				nativeMonitor = null;
			}
		}

		private sealed class ClipboardNativeWindow : NativeWindow, IDisposable {
			private IntPtr OldHandle, NextHandle;
			private bool firstTime = true;

			internal ClipboardNativeWindow() {
			}

			public override void CreateHandle(CreateParams cp) {
				base.CreateHandle(cp);
				OldHandle = Handle;
				NextHandle = NativeApi.SetClipboardViewer(Handle);
			}

			public override void ReleaseHandle() {
				NativeApi.ChangeClipboardChain(Handle, NextHandle);
				base.ReleaseHandle();
			}

			public override void DestroyHandle() {
				NativeApi.ChangeClipboardChain(Handle, NextHandle);
				OldHandle = IntPtr.Zero;
				base.DestroyHandle();
			}

			protected override void OnThreadException(Exception e) {
				Application.OnThreadException(e);
			}

			protected override void OnHandleChange() {
				base.OnHandleChange();
				NativeApi.ChangeClipboardChain(OldHandle, Handle);
				OldHandle = Handle;
			}

			protected override void WndProc(ref Message m) {
				switch (m.Msg) {
					case 776:
						if (firstTime)
							firstTime = false;
						else {
							Action handler = clipboardContentChanged;
							if (handler != null) {
								try {
									handler();
								} catch {
								}
							}
							NativeApi.PostMessage(NextHandle, (WindowMessage) m.Msg, m.WParam, m.LParam);
						}
						return;
					case 781:
						if (m.WParam == NextHandle)
							NextHandle = m.LParam;
						else
							NativeApi.PostMessage(NextHandle, (WindowMessage) m.Msg, m.WParam, m.LParam);
						return;
					default:
						base.WndProc(ref m);
						return;
				}
			}

			~ClipboardNativeWindow() {
				Dispose();
			}

			public void Dispose() {
				DestroyHandle();
				GC.SuppressFinalize(this);
			}
		}
	}
}