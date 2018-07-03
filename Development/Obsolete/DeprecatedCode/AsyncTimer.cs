using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Dispatch;

namespace System.Windows.Forms {
	/// <summary>
	/// The async timer mode.
	/// </summary>
	public enum TimerMode {
		/// <summary>
		/// The CPU usage will be reduced at the expense of accuracy.
		/// This mode guarantees low CPU overhead even at tight intervals, but has the worst accuracy of all modes.
		/// </summary>
		LowCpuUsage = 0,
		/// <summary>
		/// The CPU usage will be reduced at the expense of accuracy only when the timer is idle between ticks. This is the default mode, and it is recommended for intervals greater than 50ms.
		/// </summary>
		LowCpuWhenIdle = 1,
		/// <summary>
		/// The timer will be very accurate, ideal for gameplay and fast, steady intervals. It consumes more CPU cycles though, so beware.
		/// </summary>
		Accurate = 2
	}

	/// <summary>
	/// Flexibly handles calling methods asynchronously at given intervals on a dedicated thread.
	/// It is a very accurate timer, and can be used to time gameplay renders and updates.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxItemFilter("System.Windows.Forms")]
	[DesignTimeVisible(true)]
	[Description("An asynchronous timer.")]
	[DisplayName(nameof(AsyncTimer))]
	[DefaultEvent(nameof(Tick))]
	public class AsyncTimer : Component {
		/// <summary>
		/// Handles the AsyncTimer tick event.
		/// </summary>
		/// <param name="sender">The AsyncTimer that caused the tick.</param>
		/// <param name="elapsedMilliseconds">The elapsed milliseconds since the last tick.</param>
		public delegate void TickEventHandler(object sender, double elapsedMilliseconds);
		private static ConcurrentDictionary<Thread, byte> WaitCounters = new ConcurrentDictionary<Thread, byte>();
		private static SyncedList<AsyncTimer> Timers = new SyncedList<AsyncTimer>();
		private static ThreadStart globalLoopStart = StartGlobalLoop;
		private static AutoResetEvent globalResetEvent = new AutoResetEvent(false);
		private static TimerMode GlobalMode;
		private static Thread globalThread;
		private static bool applicationRunning = true;
		/// <summary>
		/// Gets or sets whether to catch exceptions.
		/// </summary>
		public bool CatchExceptions;
		/// <summary>
		/// Just for debugging purposes.
		/// </summary>
		public string Name;
		/// <summary>
		/// Whether to log to console when a deadlock is detected and resolved.
		/// </summary>
		public bool LogOnDeadlockResolve;
		/// <summary>
		/// Calls the delegate method, useful for intercepting invocations (default: null).
		/// </summary>
		public DispatcherSlim.DelegateInvoker InvokeHandler;
		private PreciseStopwatch updateWatch = new PreciseStopwatch();
		private ThreadStart timerThread;
		private object SyncRoot = new object();
		private AutoResetEvent threadResetEvent = new AutoResetEvent(false);
		private TimerMode mode = TimerMode.LowCpuWhenIdle;
		private double additive, updateInterval = PreciseStopwatch.ConvertToTicks(1000000000.0);
		private bool isPaused, enabledDesignMode;
		private Thread updateThread;
		private ConcurrentQueue<InvocationData> invocations = new ConcurrentQueue<InvocationData>();
		/// <summary>
		/// Occurs when the time interval has elapsed.
		/// </summary>
		public event TickEventHandler Tick;

		/// <summary>
		/// Gets or sets whether to execute delayed queued ticks or to discard them.
		/// Only appropriate when time accuracy and steadiness is not a priority but method call count is.
		/// </summary>
		[Description("Gets or sets whether to execute delayed queued ticks or to discard them.")]
		[DefaultValue(false)]
		public bool PileTicks {
			get;
			set;
		}

		/// <summary>
		/// Gets the elapsed milliseconds since the last tick.
		/// </summary>
		[Browsable(false)]
		public double ElapsedMilliseconds {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				return updateWatch.ElapsedMilliseconds;
			}
		}

		/// <summary>
		/// Gets the PreciseStopwatch instance that governs this AsyncTimer.
		/// </summary>
		[Browsable(false)]
		public PreciseStopwatch Elapsed {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				return updateWatch;
			}
		}

		/// <summary>
		/// This is only invoked when FinishesAsynchronously is set to true.
		/// Returns whether all previous Tick method calls should be considered completed and OnFinished should be called.
		/// </summary>
		[Browsable(false)]
		protected virtual bool IsFinished {
			get {
				return true;
			}
		}

		/// <summary>
		/// Gets whether the timer is disposed.
		/// </summary>
		[Browsable(false)]
		public bool IsDisposed {
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets whether the timer will keep firing events periodically.
		/// </summary>
		[Description("Gets or sets whether the timer will keep firing events periodically.")]
		[DefaultValue(true)]
		public bool AutoReset {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets whether the actions invoked by the Tick event will require a completion flag from IsFinished
		/// in order to consider all previous method calls as completed and call OnFinished and get ready for another firing of the Tick event.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		protected bool FinishesAsynchronously {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets whether the timer thread is initialized and running.
		/// If you are setting it to false, it's the same as pausing but it also resets the stopwatch.
		/// </summary>
		[Description("Gets or sets whether the timer thread is initialized and running.")]
		[DefaultValue(false)]
		public bool Enabled {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				return Extensions.DesignMode ? enabledDesignMode : IsEnabled;
			}
			set {
				if (Extensions.DesignMode)
					enabledDesignMode = value;
				else if (value == Enabled)
					Paused = !value;
				else if (value) {
					lock (SyncRoot) {
						if (updateThread == null) {
							updateThread = new Thread(timerThread) {
								IsBackground = true
							};
							updateThread.Start();
						}
						if (!isPaused)
							AddTimer();
					}
				} else {
					Paused = true;
					updateWatch.ElapsedTicks = 0.0;
				}
			}
		}

		[Browsable(false)]
		internal bool IsEnabled {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				return updateThread != null && updateThread.IsAlive;
			}
		}

		/// <summary>
		/// Gets the thread that is used for timing. Can be null if the timer is not enabled or is disposed.
		/// </summary>
		[Browsable(false)]
		public Thread UpdateThread {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				if (!(updateThread == null || updateThread.IsAlive)) {
					updateThread = null;
					updateWatch.Running = false;
					Timers.Remove(this);
				}
				return updateThread;
			}
		}

		/// <summary>
		/// Gets or sets the timer mode to use.
		/// </summary>
		[Description("Gets or sets the timer mode to use.")]
		[DefaultValue((int) TimerMode.LowCpuWhenIdle)]
		public TimerMode Mode {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				return mode;
			}
			set {
				if (value == mode)
					return;
				mode = value;
				if (Enabled && !isPaused)
					CheckMode();
			}
		}

		/// <summary>
		/// Gets or sets whether the timer is paused.
		/// </summary>
		[Description("Gets or sets whether the timer is paused.")]
		[DefaultValue(false)]
		public bool Paused {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				return isPaused;
			}
			set {
				if (isPaused == value)
					return;
				isPaused = value;
				if (Extensions.DesignMode)
					return;
				else if (value) {
					updateWatch.Running = false;
					Timers.Remove(this);
				} else if (!(updateThread == null || updateInterval == double.MaxValue || updateInterval == double.PositiveInfinity || double.IsNaN(updateInterval)))
					AddTimer();
			}
		}

		/// <summary>
		/// Gets or sets the interval in milliseconds between each tick (must be greater than zero).
		/// </summary>
		[Description("Gets or sets the interval in milliseconds between each tick (must be greater than zero).")]
		[DefaultValue(1000.0)]
		public double Interval {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				return PreciseStopwatch.ConvertToNanoseconds(updateInterval) * 0.000001;
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				if (value <= 0.0)
					value = 1000.0;
				value = PreciseStopwatch.ConvertToTicks(value * 1000000.0);
				updateInterval = value;
				if (value == double.MaxValue || value == double.PositiveInfinity || double.IsNaN(value))
					Paused = true;
			}
		}

		static AsyncTimer() {
			Application.ApplicationExit += Application_ApplicationExit;
		}

		/// <summary>
		/// Initializes a new asynchronous timer.
		/// </summary>
		public AsyncTimer() {
			timerThread = StartTimerLoop;
			AutoReset = true;
		}

		/// <summary>
		/// Initializes a new asynchronous timer.
		/// </summary>
		/// <param name="container">The container to add this timer to.</param>
		public AsyncTimer(IContainer container) : this() {
			if (container != null)
				container.Add(this);
		}

		/// <summary>
		/// Initializes a new asynchronous timer with the specified interval time.
		/// </summary>
		/// <param name="interval">The interval in milliseconds between each tick (must be greater than zero).</param>
		public AsyncTimer(double interval) : this() {
			Interval = interval;
		}

		private static void Application_ApplicationExit(object sender, EventArgs e) {
			applicationRunning = false;
			globalResetEvent.Set();
			globalThread = null;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool CheckTimer() {
			bool isElapsed = updateWatch.ElapsedTicks >= updateInterval;
			if (isElapsed)
				threadResetEvent.Set();
			return isElapsed;
		}

		private void StartTimerLoop() {
			updateWatch.ElapsedTicks = 0.0;
			updateWatch.Running = true;
			double elapsed;
			bool taskInProgress = false;
			InvocationData e;
			do {
				threadResetEvent.WaitOne();
				elapsed = updateWatch.ElapsedTicks;
				if (elapsed >= updateInterval && !isPaused) {
					updateWatch.ElapsedTicks = PileTicks ? elapsed - updateInterval : elapsed % updateInterval;
					elapsed = PreciseStopwatch.ConvertToNanoseconds(elapsed) * 0.000001;
					if (FinishesAsynchronously) {
						if (IsFinished) {
							if (taskInProgress) {
								taskInProgress = false;
								OnFinished();
							}
							OnTick(elapsed + additive);
							additive = 0.0;
							taskInProgress = true;
						} else
							additive += elapsed;
					} else {
						OnTick(elapsed + additive);
						additive = 0.0;
						OnFinished();
						taskInProgress = false;
					}
					if (!AutoReset)
						Paused = true;
				}
				while (!IsDisposed && invocations.TryDequeue(out e)) {
					//InvokeInner
					e.State = InvokeState.Started;
					try {
						e.AsyncState = InvokeHandler == null ? e.Method(e.Parameter) : InvokeHandler(e.Method, e.Parameter);
						e.State = InvokeState.Completed;
					} catch (Exception ex) {
						e.State = InvokeState.Error;
						if (CatchExceptions)
							ErrorHandler.LogException(ex, ErrorHandler.ExceptionToDetailedString("An error happened while invoking a timer callback.", ex));
						else
							throw;
					}
					e.ResetEvent.Set();
				}
			} while (!IsDisposed);
			if (threadResetEvent != null) {
				threadResetEvent.Dispose();
				threadResetEvent = null;
			}
		}

		/// <summary>
		/// Invokes the method with the specified parameters asynchronously on the update thread. Execution starts when the timer is enabled.
		/// </summary>
		/// <param name="e">Holds the data required for an invocation.</param>
		public void BeginInvoke(InvocationData e) {
			if (e == null || IsDisposed)
				return;
			if (Thread.CurrentThread == UpdateThread)
				InvokeOnCurrentThread(e);
			else {
				invocations.Enqueue(e);
				e.State = InvokeState.Queued;
				if (threadResetEvent != null)
					threadResetEvent.Set();
			}
		}

		/// <summary>
		/// Invokes the method with the specified parameters synchronously on the update thread. Execution starts when the timer is enabled.
		/// </summary>
		/// <param name="e">Holds the data required for an invocation.</param>
		public object Invoke(InvocationData e) {
			if (e == null)
				return null;
			else if (!Enabled)
				BeginInvoke(e);
			else if (!IsDisposed) {
				Thread current = Thread.CurrentThread;
				if (current == UpdateThread)
					return InvokeOnCurrentThread(e);
				else {
					e.State = InvokeState.Queued;
					if (WaitCounters.ContainsKey(current)) {
						invocations.Enqueue(e);
						if (threadResetEvent != null)
							threadResetEvent.Set();
						if (LogOnDeadlockResolve)
							ErrorHandler.LogException(new InvalidOperationException("A deadlock has been detected and resolved."));
					} else {
						invocations.Enqueue(e);
						if (threadResetEvent != null)
							threadResetEvent.Set();
						WaitCounters.TryAdd(current, 1);
						e.ResetEvent.Wait();
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
		public object EndInvoke(InvocationData e) {
			if (e == null)
				return null;
			else if (!e.CanConvertToSync || IsDisposed || !Enabled)
				return e.AsyncState;
			Thread current = Thread.CurrentThread;
			if (current != updateThread) {
				if (WaitCounters.ContainsKey(current)) {
					if (LogOnDeadlockResolve)
						ErrorHandler.LogException(new InvalidOperationException("A deadlock has been detected and resolved."));
					return e.AsyncState;
				} else {
					if (!(e.State == InvokeState.Queued || e.State == InvokeState.Started))
						return e.AsyncState;
					WaitCounters.TryAdd(current, 1);
					e.ResetEvent.Wait();
					byte val;
					WaitCounters.TryRemove(current, out val);
					e.Dispose();
				}
			}
			e.CompletedSynchronously = true;
			return e.AsyncState;
		}

		/// <summary>
		/// Performs the specified invocation on the current thread.
		/// </summary>
		/// <param name="e">The data describing the invocation to perform.</param>
		public static object InvokeOnCurrentThread(InvocationData e) {
			e.State = InvokeState.Started;
			e.AsyncState = e.Method(e.Parameter);
			e.State = InvokeState.Completed;
			e.CompletedSynchronously = true;
			return e.AsyncState;
		}

		private void AddTimer() {
			updateWatch.Running = true;
			Timers.Add(this);
			globalResetEvent.Set();
			if (globalThread == null) {
				globalThread = new Thread(globalLoopStart) {
					IsBackground = true
				};
				globalThread.Start();
			}
			CheckMode();
		}

		private static void StartGlobalLoop() {
			bool idle;
			do {
				idle = true;
				foreach (AsyncTimer timer in Timers) {
					if (timer.CheckTimer())
						idle = false;
				}
				if (GlobalMode == TimerMode.LowCpuUsage)
					Thread.Sleep(1);
				else if (idle && GlobalMode == TimerMode.LowCpuWhenIdle)
					Thread.Sleep(1);
				else
					Thread.Yield();
				if (Timers.Count == 0)
					globalResetEvent.WaitOne();
			} while (applicationRunning);
		}

		private void CheckMode() {
			if (mode == TimerMode.Accurate)
				GlobalMode = TimerMode.Accurate;
			else if (mode == TimerMode.LowCpuWhenIdle) {
				int max = (int) TimerMode.LowCpuUsage;
				int value;
				foreach (AsyncTimer timer in Timers) {
					value = (int) timer.mode;
					if (value == (int) TimerMode.Accurate) {
						GlobalMode = TimerMode.Accurate;
						return;
					} else if (value > max)
						max = value;
				}
				GlobalMode = mode;
			}
		}

		/// <summary>
		/// Called when the timer is elapsed.
		/// </summary>
		/// <param name="elapsedMilliseconds">The elapsed milliseconds since the last tick.</param>
		protected virtual void OnTick(double elapsedMilliseconds) {
			TickEventHandler handler = Tick;
			if (handler != null)
				handler(this, elapsedMilliseconds);
		}

		/// <summary>
		/// Called when the previously queued tick method call has been completed.
		/// </summary>
		protected virtual void OnFinished() {
		}

		/// <summary>
		/// Disposes of the timer.
		/// </summary>
		/// <param name="disposing">Whether to dispose managed resources.</param>
		protected override void Dispose(bool disposing) {
			if (IsDisposed || updateThread == null)
				return;
			Paused = true;
			IsDisposed = true;
			if (Enabled)
				threadResetEvent.Set();
			else if (threadResetEvent != null) {
				threadResetEvent.Dispose();
				threadResetEvent = null;
			}
			updateThread = null;
			base.Dispose(disposing);
		}
	}
}