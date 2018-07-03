using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Threading.Dispatch {
	/// <summary>
	/// A more optimized version of System.Threading.Dispatch.Dispatcher.
	/// </summary>
	public sealed class DispatcherSlim : IDisposable {
		/// <summary>
		/// Invokes a delegate using the specified parameters. The return value of the call should be returned.
		/// </summary>
		/// <param name="method">The method to invoke.</param>
		/// <param name="parameter">The parameter to use.</param>
		public delegate object DelegateInvoker(Func<object, object> method, object parameter);

		private static ConcurrentDictionary<Thread, byte> WaitCounters = new ConcurrentDictionary<Thread, byte>();
		private ConcurrentQueue<InvocationData> invocations = new ConcurrentQueue<InvocationData>();
		private ConcurrentQueue<InvocationData> firstClassInvocations = new ConcurrentQueue<InvocationData>();
		private AutoResetEvent threadResetEvent = new AutoResetEvent(false);
		private object initEventLock = new object();
		private ThreadPriority threadPriority;
		private Thread dispatchThread;
		private bool isDisposed, running = true;
		/// <summary>
		/// Just for debugging purposes.
		/// </summary>
		public object Tag;
		/// <summary>
		/// Just for debugging purposes.
		/// </summary>
		public string Name;
		/// <summary>
		/// Whether the thread is a background thread.
		/// </summary>
		public readonly bool IsBackground;
		/// <summary>
		/// Calls the delegate method, useful for intercepting invocations (default: null).
		/// </summary>
		public DelegateInvoker InvokeHandler;
		/// <summary>
		/// Gets or sets the maximum length of the queue. If max length is exceeded, new invocations are ignored until the queue has cleared below max.
		/// </summary>
		public int QueueCap;
		/// <summary>
		/// Gets or sets whether to catch exceptions.
		/// </summary>
		public ExceptionMode Exceptions;
		/// <summary>
		/// Whether to log to console when a deadlock is detected and resolved.
		/// </summary>
		public bool LogOnDeadlockResolve;

		/// <summary>
		/// Whether dispatcher is currently busy invoking methods.
		/// </summary>
		public bool IsExecuting {
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets whether the dispatcher is performing invocations.
		/// </summary>
		public bool Running {
			get {
				return running;
			}
			set {
				if (running == value)
					return;
				running = value;
				if (value && QueueCount != 0)
					threadResetEvent.Set();
			}
		}

		/// <summary>
		/// True if the current thread is not the dispatcher thread.
		/// </summary>
		public bool InvokeRequired {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return !(Thread.CurrentThread == DispatchThread || dispatchThread == null);
			}
		}

		/// <summary>
		/// Gets the number of queued invocations. Each invocation is dequeued before executing it.
		/// </summary>
		public int QueueCount {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return invocations.Count + firstClassInvocations.Count;
			}
		}

		/// <summary>
		/// Gets whether the dispatcher is disposed.
		/// </summary>
		public bool IsDisposed {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				if (!(dispatchThread == null || dispatchThread.IsAlive))
					Dispose(false);
				return isDisposed;
			}
		}

		/// <summary>
		/// Gets the thread on which the dispatcher resides.
		/// </summary>
		public Thread DispatchThread {
			get {
				if (dispatchThread == null && !isDisposed)
					CheckInitThread();
				else if (!dispatchThread.IsAlive)
					Dispose(false);
				return dispatchThread;
			}
		}

		/// <summary>
		/// Gets or sets the priority of the background thread.
		/// </summary>
		public ThreadPriority ThreadPriority {
			get {
				return threadPriority;
			}
			set {
				if (threadPriority == value)
					return;
				threadPriority = value;
				Thread dispatchThread = DispatchThread;
				if (dispatchThread != null)
					dispatchThread.Priority = value;
			}
		}

		/// <summary>
		/// Initializes and starts new instance of DispatcherSlim.
		/// </summary>
		/// <param name="name">Optional. Useful for debugging.</param>
		/// <param name="isBackground">Whether the thread is a background thread.</param>
		/// <param name="exceptions">Whether to catch any exceptions that may come up.</param>
		/// <param name="threadPriority">The priority of the background thread.</param>
		/// <param name="queueCap">The invocation count cap. If max queue length is exceeded, new invocations are ignored until the queue has cleared below max.</param>
		public DispatcherSlim(string name, bool isBackground, ExceptionMode exceptions = ExceptionMode.Throw, ThreadPriority threadPriority = ThreadPriority.Normal, int queueCap = int.MaxValue) {
			Name = name;
			QueueCap = queueCap;
			Exceptions = exceptions;
			IsBackground = isBackground;
			this.threadPriority = threadPriority;
		}

		private void CheckInitThread() {
			lock (initEventLock) {
				if (!IsDisposed && dispatchThread == null) {
					dispatchThread = new Thread(RunDispatchLoop);
					string name = Name;
					if (name != null)
						dispatchThread.Name = name;
					dispatchThread.IsBackground = IsBackground;
					dispatchThread.Priority = threadPriority;
					dispatchThread.Start();
				}
			}
		}

		/// <summary>
		/// Asychronously invokes the specified method with the specified parameters.
		/// </summary>
		/// <param name="method">The method to invoke.</param>
		/// <param name="parameter">The parameter to use it with.</param>
		public void BeginInvoke(Func<object, object> method, object parameter = null) {
			BeginInvoke(new InvocationData(method, parameter));
		}

		/// <summary>
		/// Asychronously invokes the specified method with the specified parameters.
		/// </summary>
		/// <param name="e">Holds the data required for an invocation.</param>
		/// <param name="firstClass">Whether this method call should be prioritized above others (use sparingly).</param>
		public void BeginInvoke(InvocationData e, bool firstClass = false) {
			if (e == null || QueueCount >= QueueCap || IsDisposed)
				return;
			if (Thread.CurrentThread == DispatchThread)
				e.InvokeOnCurrentThread();
			else {
				if (firstClass)
					firstClassInvocations.Enqueue(e);
				else
					invocations.Enqueue(e);
				e.State = InvokeState.Queued;
				if (threadResetEvent != null)
					threadResetEvent.Set();
			}
		}

		/// <summary>
		/// Sychronously invokes the specified method with the specified parameters.
		/// </summary>
		/// <param name="method">The method to invoke.</param>
		/// <param name="parameter">The parameter to use it with.</param>
		/// <param name="timeout">The waiting timeout for the invocation in milliseconds (only used if the value is greater than 0).</param>
		public object Invoke(Func<object, object> method, object parameter = null, int timeout = 0) {
			return Invoke(new InvocationData(method, parameter), timeout, false);
		}

		/// <summary>
		/// Sychronously invokes the specified method with the specified parameters.
		/// </summary>
		/// <param name="e">Holds the data required for an invocation.</param>
		/// <param name="firstClass">Whether this method call should be prioritized above others (use sparingly).</param>
		/// <param name="timeout">The waiting timeout for the invocation in milliseconds (only used if the value is greater than 0).</param>
		public object Invoke(InvocationData e, int timeout = 0, bool firstClass = false) {
			if (e == null)
				return null;
			else if (QueueCount < QueueCap && !IsDisposed) {
				Thread current = Thread.CurrentThread;
				if (current == DispatchThread)
					return e.InvokeOnCurrentThread();
				else {
					e.State = InvokeState.Queued;
					if (WaitCounters.ContainsKey(current)) {
						firstClassInvocations.Enqueue(e);
						if (threadResetEvent != null)
							threadResetEvent.Set();
						if (LogOnDeadlockResolve)
							ErrorHandler.LogException(new InvalidOperationException("A deadlock has been detected and resolved."));
					} else {
						if (firstClass)
							firstClassInvocations.Enqueue(e);
						else
							invocations.Enqueue(e);
						if (threadResetEvent != null)
							threadResetEvent.Set();
						WaitCounters.TryAdd(current, 1);
						if (timeout > 0)
							e.ResetEvent.Wait(timeout);
						byte val;
						WaitCounters.TryRemove(current, out val);
						e.CompletedSynchronously = true;
					}
					e.Dispose();
				}
			}
			return e.AsyncState;
		}

		/// <summary>
		/// Waits for the invocation to finish.
		/// </summary>
		/// <param name="e">The invocation that was initiated.</param>
		/// <param name="timeout">The waiting timeout for the invocation in milliseconds (only used if the value is greater than 0).</param>
		public object EndInvoke(InvocationData e, int timeout = 0) {
			if (e == null)
				return null;
			else if (IsDisposed || DispatchThread == null)
				return e.AsyncState;
			Thread current = Thread.CurrentThread;
			if (current != dispatchThread) {
				if (WaitCounters.ContainsKey(current)) {
					if (LogOnDeadlockResolve)
						ErrorHandler.LogException(new InvalidOperationException("A deadlock has been detected and resolved."));
					return e.AsyncState;
				} else {
					if (!(e.State == InvokeState.Queued || e.State == InvokeState.Started))
						return e.AsyncState;
					WaitCounters.TryAdd(current, 1);
					e.ResetEvent.Wait(timeout);
					byte val;
					WaitCounters.TryRemove(current, out val);
					e.Dispose();
				}
			}
			e.CompletedSynchronously = true;
			return e.AsyncState;
		}

		/// <summary>
		/// Interrupts the normal execution of the dispatcher (highly not recommended).
		/// </summary>
		/// <param name="abortCurrentThread">Whether to abort the current thread mid-execution.</param>
		/// <param name="restartThread">Whether to create a new fresh thread.</param>
		/// <param name="clearQueue">Whether to clear the dispatch queue.</param>
		public void InterruptExecution(bool abortCurrentThread, bool restartThread, bool clearQueue) {
			if (clearQueue) {
				invocations = new ConcurrentQueue<InvocationData>();
				firstClassInvocations = new ConcurrentQueue<InvocationData>();
			}
			if ((abortCurrentThread || restartThread) && DispatchThread != null) {
				if (abortCurrentThread) {
					try {
						dispatchThread.Abort();
					} catch {
					}
				}
				if (restartThread)
					CheckInitThread();
			}
		}

		private void RunDispatchLoop() {
			InvocationData e;
			try {
				do {
					if (QueueCount == 0 || !running) {
						IsExecuting = false;
						threadResetEvent.WaitOne();
					} else {
						IsExecuting = true;
						while (running && !isDisposed && firstClassInvocations.TryDequeue(out e))
							InvokeInner(e);
						while (running && !isDisposed && invocations.TryDequeue(out e)) {
							InvokeInner(e);
							while (running && !isDisposed && firstClassInvocations.TryDequeue(out e))
								InvokeInner(e);
						}
					}
				} while (!isDisposed);
			} catch (ThreadAbortException) {
			}
			threadResetEvent.Close();
			threadResetEvent = null;
		}

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		private void InvokeInner(InvocationData e) {
			e.State = InvokeState.Started;
			try {
				DelegateInvoker invokeHandler = InvokeHandler;
				e.AsyncState = invokeHandler == null ? e.Method(e.Parameter) : invokeHandler(e.Method, e.Parameter);
				e.State = InvokeState.Completed;
			} catch (Exception ex) {
				e.State = InvokeState.Error;
				if (Exceptions == ExceptionMode.Log)
					ErrorHandler.LogException(ex, ErrorHandler.ExceptionToDetailedString("An error occurred while invoking a method.", ex));
				else if (Exceptions == ExceptionMode.Throw) {
					e.ResetEvent.Set();
					throw;
				}
			}
			e.ResetEvent.Set();
		}

		/// <summary>
		/// Disposes of the dispatcher asynchronously.
		/// </summary>
		~DispatcherSlim() {
			Dispose(false);
		}

		/// <summary>
		/// Disposes of the dispatcher asynchronously.
		/// </summary>
		public void Dispose() {
			Dispose(false);
		}

		/// <summary>
		/// Disposes of the dispatcher, specifying whether it is disposed synchronously or asynchronously.
		/// </summary>
		/// <param name="sync">Whether the dispatcher is disposed synchronously or asynchronously.</param>
		/// <param name="timeout">The sync timeout (leave to 0 to wait indefinitely).</param>
		public void Dispose(bool sync, int timeout = 0) {
			if (isDisposed)
				return;
			isDisposed = true;
			if (Thread.CurrentThread == dispatchThread)
				sync = false;
			if (dispatchThread != null && dispatchThread.IsAlive) {
				threadResetEvent.Set();
				if (sync) {
					if (timeout > 0)
						dispatchThread.Join(timeout);
					else
						dispatchThread.Join();
				}
			}
			dispatchThread = null;
			GC.SuppressFinalize(this);
		}
	}
}