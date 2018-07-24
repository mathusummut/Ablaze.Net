using System.Collections.Generic;
using System.Platforms.Windows;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Windows.Forms {
	/// <summary>
	/// Creates and handles message loops as a better alternative to the Application class.
	/// The Application class events will still be fully functional.
	/// </summary>
	public static class MessageLoop {
		private delegate bool PreTranslateFilterHandler(ref MSG msg);
		private static FieldInfo raiseEnterModal = typeof(Application).GetNestedType("ThreadContext", BindingFlags.NonPublic).GetField("enterModalHandler", BindingFlags.Instance | BindingFlags.NonPublic);
		private static FieldInfo raiseLeaveModal = typeof(Application).GetNestedType("ThreadContext", BindingFlags.NonPublic).GetField("leaveModalHandler", BindingFlags.Instance | BindingFlags.NonPublic);
		private static Action raiseApplicationExit = (Action) Delegate.CreateDelegate(typeof(Action), null, typeof(Application).GetMethod("RaiseExit", BindingFlags.Static | BindingFlags.NonPublic));
		private static Action raiseThreadExit = (Action) Delegate.CreateDelegate(typeof(Action), null, typeof(Application).GetMethod("RaiseThreadExit", BindingFlags.Static | BindingFlags.NonPublic));
		private static ParameterizedThreadStart disposeForm = DisposeForm;
		private static Func<object> GetCurrentThreadContext;
		private static EventHandler visibleChanged = Form_VisibleChanged;
		private static ThreadLocal<bool> messageLoop = new ThreadLocal<bool>();
		private static ThreadLocal<bool> closed = new ThreadLocal<bool>();
		private static volatile bool exitApplication;
		private static int messageLoopCount;
		private static ThreadLocal<KeyValuePair<Form, bool>> ForeForms = new ThreadLocal<KeyValuePair<Form, bool>>();
		private static PreTranslateFilterHandler Filter;

		/// <summary>
		/// Gets the total number of message loops currently running in the application
		/// </summary>
		public static int MessageLoopCount {
			get {
				return Volatile.Read(ref messageLoopCount);
			}
		}

		/// <summary>
		/// Gets whether a message loop was started on this thread
		/// </summary>
		public static bool MessageLoopStarted {
			get {
				return messageLoop.IsValueCreated && messageLoop.Value;
			}
		}

		static MessageLoop() {
			Control.CheckForIllegalCrossThreadCalls = false;
			NativeApi.RegisterWindowMessage("commdlg_help");
			Type invoker = typeof(FilterInvoker<,>).MakeGenericType(new Type[] {
				typeof(Application).GetNestedType("ThreadContext", BindingFlags.NonPublic),
				typeof(NativeWindow).Assembly.GetType("System.Windows.Forms.NativeMethods").GetNestedType(nameof(MSG), BindingFlags.Public) });
			Filter = (PreTranslateFilterHandler) Delegate.CreateDelegate(typeof(PreTranslateFilterHandler), null, invoker.GetMethod(nameof(Filter), BindingFlags.Public | BindingFlags.Static));
			GetCurrentThreadContext = (Func<object>) Delegate.CreateDelegate(typeof(Func<object>), null, invoker.GetMethod(nameof(GetCurrentThreadContext), BindingFlags.Static | BindingFlags.Public));
		}

		/// <summary>
		/// Processes the specified message accordingly.
		/// </summary>
		/// <param name="message">The message that was received.</param>
		public static void ProcessMessage(ref MSG message) {
			if (message.Message == WindowMessage.QUIT) {
				message.Message = WindowMessage.NULL;
				return;
			} else if (message.Message != WindowMessage.NULL && Filter(ref message))
				return;
			KeyValuePair<Form, bool> form = ForeForms.Value;
			bool toQuit = false;
			if (form.Key == null)
				toQuit = true;
			else if (form.Key.IsDisposed || !form.Key.IsHandleCreated || ((message.Message == WindowMessage.DESTROY || 
				(message.Message == WindowMessage.SHOWWINDOW && message.WParam == IntPtr.Zero && (message.LParam == IntPtr.Zero || message.LParam == new IntPtr(1))) ||
				(message.Message == WindowMessage.ENDSESSION && message.WParam == new IntPtr(1))) &&
				(message.HWnd == IntPtr.Zero || form.Key.Handle == message.HWnd))) {
				RemoveForm(form);
				toQuit = true;
			}
			NativeApi.TranslateMessage(ref message);
			try {
				NativeApi.DispatchMessage(ref message);
			} catch (Exception ex) {
				Application.OnThreadException(ex);
			}
			if (toQuit || closed.Value) {
				closed.Value = false;
				message.Message = WindowMessage.QUIT;
			}
		}

		private static void RemoveForm(KeyValuePair<Form, bool> form) {
			Form owner = form.Key.Owner;
			KeyValuePair<Form, bool> newForm = new KeyValuePair<Form, bool>(owner, true);
			ForeForms.Value = newForm;
			if (owner == null || owner.IsDisposed) {
				if (form.Key.IsHandleCreated)
					NativeApi.PostMessage(form.Key.Handle, WindowMessage.QUIT, IntPtr.Zero, IntPtr.Zero);
				else {
					NativeApi.PostThreadMessage(NativeApi.GetCurrentThreadId(), WindowMessage.QUIT, IntPtr.Zero, IntPtr.Zero);
					//NativeApi.PostMessage(new IntPtr(0xffff), WindowMessage.NULL, IntPtr.Zero, IntPtr.Zero);
				}
			} else {
				owner.Enabled = form.Value;
				Delegate del = raiseLeaveModal.GetValue(GetCurrentThreadContext()) as Delegate;
				if (del != null)
					del.DynamicInvoke(null, EventArgs.Empty);
				if (!owner.Visible)
					RemoveForm(newForm);
			}
		}

		/// <summary>
		/// Processes all Windows messages currently in the message queue and returns true if any event was found.
		/// </summary>
		public static MessageLoopResult DoEvents() {
			MSG message = new MSG();
			MessageLoopResult processed = MessageLoopResult.None;
			while (!exitApplication && NativeApi.PeekMessage(ref message, IntPtr.Zero, 0, 0, 1)) {
				processed = MessageLoopResult.MessageFound;
				ProcessMessage(ref message);
				if (message.Message == WindowMessage.QUIT)
					return MessageLoopResult.Quitting;
			}
			if (!exitApplication && processed == MessageLoopResult.MessageFound)
				Application.RaiseIdle(EventArgs.Empty);
			return processed;
		}

		/// <summary>
		/// Starts a message loop on this thread.
		/// </summary>
		/// <param name="checkMessageLoop">If true, a modal loop is not started if it already exists.</param>
		private static void Run(bool checkMessageLoop) {
			if (checkMessageLoop && MessageLoopStarted)
				return;
			messageLoop.Value = true;
			Interlocked.Increment(ref messageLoopCount);
			MSG message = new MSG();
			try {
				while (!exitApplication && NativeApi.GetMessage(ref message, IntPtr.Zero, 0, 0) > 0 && !exitApplication) {
					ProcessMessage(ref message);
					if (message.Message == WindowMessage.QUIT || DoEvents() == MessageLoopResult.Quitting) {
						raiseThreadExit();
						return;
					}
				}
			} finally {
				messageLoop.Value = false;
				if (Interlocked.Decrement(ref messageLoopCount) <= 0) {
					exitApplication = false;
					raiseApplicationExit();
				}
			}
		}

		private static DialogResult Run(Form form, bool dispose, bool modal) {
			if (form == null) {
				Run(!modal);
				return DialogResult.None;
			} else {
				try {
					KeyValuePair<Form, bool> owner = ForeForms.Value;
					if (owner.Key == null || owner.Key.IsDisposed)
						ForeForms.Value = new KeyValuePair<Form, bool>(form, true);
					else {
						try {
							form.Owner = owner.Key;
						} catch {
							form.Visible = true;
							Run(!modal);
							return form.DialogResult;
						}
						ForeForms.Value = new KeyValuePair<Form, bool>(form, owner.Key.Enabled);
						owner.Key.Enabled = false;
					}
					if (modal) {
						Delegate del = raiseEnterModal.GetValue(GetCurrentThreadContext()) as Delegate;
						if (del != null)
							del.DynamicInvoke(null, EventArgs.Empty);
					}
					form.FormClosed += Form_FormClosed;
					form.Visible = true;
					Run(!modal);
					return form.DialogResult;
				} finally {
					if (dispose) {
						Thread thread = new Thread(disposeForm);
						thread.Name = "FormDisposeThread";
						thread.IsBackground = true;
						thread.Start(form);
						thread.Join(1200);
					}
				}
			}
		}

		private static void Form_FormClosed(object sender, FormClosedEventArgs e) {
			Form currentForm = sender as Form;
			if (currentForm == null)
				return;
			currentForm.SetState(4096, true);
			currentForm.Visible = false;
			closed.Value = true;
		}

		private static void Form_VisibleChanged(object sender, EventArgs e) {
			Form currentForm = sender as Form;
			if (currentForm == null || currentForm.Visible)
				return;
			KeyValuePair<Form, bool> form = ForeForms.Value;
			if (currentForm == form.Key) {
				currentForm.VisibleChanged -= visibleChanged;
				RemoveForm(form);
			}
		}

		/// <summary>
		/// Starts a message loop on this thread and shows the specified form, and disposes it after use.
		/// </summary>
		/// <param name="form">Shows the specified form and starts a message loop.</param>
		/// <param name="dispose">If true, the form will be disposed after being shown.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static DialogResult Run(Form form, bool dispose = true) {
			return Run(form, dispose, false);
		}

		/// <summary>
		/// Shows the specified form as dialog.
		/// </summary>
		/// <param name="form">Shows the specified form and starts a message loop.</param>
		/// <param name="dispose">If true, the form will be disposed after being shown.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static DialogResult ShowDialog(Form form, bool dispose = true) {
			if (form != null) {
				//form.SetState(32, true);
				form.VisibleChanged += visibleChanged;
			}
			DialogResult result = Run(form, dispose, true);
			//if (form != null)
			//	form.SetState(32, false);
			return result;
		}

		private static void DisposeForm(object value) {
			((Form) value).Dispose();
		}

		/// <summary>
		/// Exits the message loop on this current thread.
		/// </summary>
		public static void ExitThread() {
			if (MessageLoopStarted)
				NativeApi.PostQuitMessage(0);
		}

		/// <summary>
		/// Exits the message loops of all threads.
		/// </summary>
		public static void Exit() {
			exitApplication = true;
			NativeApi.PostMessage(new IntPtr(0xffff), WindowMessage.NULL, IntPtr.Zero, IntPtr.Zero);
		}

		/// <summary>
		/// Here be dragon's venom!!!
		/// </summary>
		private static class FilterInvoker<ThreadContext, MSGInternal> {
			private delegate bool PreTranslateMessageHandler(ThreadContext threadContext, ref MSGInternal msg);
			private static Func<ThreadContext> getThreadContext = (Func<ThreadContext>) Delegate.CreateDelegate(typeof(Func<ThreadContext>), null, typeof(ThreadContext).GetMethod("FromCurrent", BindingFlags.NonPublic | BindingFlags.Static));
			private static Func<ThreadContext> currentThreadContext = (Func<ThreadContext>) Delegate.CreateDelegate(typeof(Func<ThreadContext>), null, typeof(ThreadContext).GetMethod("FromCurrent", BindingFlags.NonPublic | BindingFlags.Static));
			private static PreTranslateMessageHandler prefilter = (PreTranslateMessageHandler) Delegate.CreateDelegate(typeof(PreTranslateMessageHandler), typeof(ThreadContext).GetMethod("PreTranslateMessage", BindingFlags.NonPublic | BindingFlags.Instance));

			public static object GetCurrentThreadContext() {
				return currentThreadContext();
			}

			public static unsafe bool Filter(ref MSG message) {
				//not meant to be understood
				MSG* originalMSG = stackalloc MSG[1];
				*originalMSG = message;
				MSGInternal[] returnedMSG = new MSGInternal[1];
				TypedReference tr = __makeref(returnedMSG[0]);
				*(IntPtr*) (&tr) = new IntPtr(originalMSG);
				returnedMSG[0] = __refvalue(tr, MSGInternal);
				bool returnValue = prefilter(getThreadContext(), ref returnedMSG[0]);
				GCHandle handle = GCHandle.Alloc(returnedMSG, GCHandleType.Pinned);
				message = *(MSG*) handle.AddrOfPinnedObject();
				handle.Free();
				return returnValue;
			}
		}
	}

	/// <summary>
	/// Represents the current message loop state.
	/// </summary>
	[Flags]
	public enum MessageLoopResult {
		/// <summary>
		/// No message was found.
		/// </summary>
		None = 0,
		/// <summary>
		/// One or more messages were found and processed.
		/// </summary>
		MessageFound = 1,
		/// <summary>
		/// Messages were found and WM_QUIT was received.
		/// </summary>
		Quitting = 2,
	}
}