using System.Diagnostics;
using System.Platforms.Windows;

namespace System.Threading {
	/// <summary>
	/// Flexibly handles calling methods asynchronously at given intervals on a dedicated thread.
	/// It is a very accurate timer, and can be used to time gameplay renders and updates.
	/// </summary>
	public sealed class AsyncTimer : IDisposable {
		private static int timerCount;
		/// <summary>
		/// Fired when the timer is elapsed.
		/// </summary>
		public event Action<AsyncTimer> Tick;
		/// <summary>
		/// Fired when Tick callback is completed.
		/// </summary>
		public event Action<AsyncTimer> TickFinished;
		/// <summary>
		/// Gets or sets whether to catch exceptions.
		/// </summary>
		public ExceptionMode Exceptions = ExceptionMode.Throw;
		/// <summary>
		/// If true, intervals are ignored and the callback is called immediately after the previous call (not recommended).
		/// </summary>
		public bool DontWaitForInterval;
		/// <summary>
		/// Available for your use.
		/// </summary>
		public object Tag;
		private IntPtr handle;
		private bool running;
		private int interval;

		/// <summary>
		/// Gets whether the timer is disposed.
		/// </summary>
		public bool IsDisposed {
			get {
				return handle == IntPtr.Zero;
			}
		}

		/// <summary>
		/// Gets or sets whether the timer is running.
		/// </summary>
		public bool Running {
			get {
				return running;
			}
			set {
				if (value == running)
					return;
				running = value;
				if (!(Extensions.DesignMode || IsDisposed)) {
					long targetInterval;
					if (value) {
						targetInterval = (long) interval * 10;
						NativeApi.SetWaitableTimer(handle, ref targetInterval, interval, IntPtr.Zero, IntPtr.Zero, true);
					} else {
						targetInterval = 0L;
						NativeApi.SetWaitableTimer(handle, ref targetInterval, 0, IntPtr.Zero, IntPtr.Zero, true);
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the interval elapse timer in milliseconds.
		/// </summary>
		public int Interval {
			get {
				return interval;
			}
			set {
				if (value < 1)
					value = 1;
				if (interval == value)
					return;
				interval = value;
				if (!(Extensions.DesignMode || IsDisposed)) {
					long targetInterval = (long) interval * 10;
					NativeApi.SetWaitableTimer(handle, ref targetInterval, interval, IntPtr.Zero, IntPtr.Zero, true);
				}
			}
		}

		/// <summary>
		/// Initializes a new asynchronous timer with the 1 second intervals by default.
		/// </summary>
		public AsyncTimer() : this(1000) {
		}

		/// <summary>
		/// Initializes a new asynchronous timer with the specified tick period.
		/// </summary>
		/// <param name="interval">The interval length between ticks in milliseconds.</param>
		/// <param name="onBackgroundThread">Only set to false if you plan to exclusively use WaitForTick().</param>
		public AsyncTimer(int interval, bool onBackgroundThread = true) {
			if (Interlocked.Increment(ref timerCount) == 1)
				NativeApi.TimeBeginPeriod(6);
			this.interval = interval;
			if (!Extensions.DesignMode) {
				handle = NativeApi.CreateWaitableTimer(IntPtr.Zero, false, null);
				if (onBackgroundThread) {
					Thread thread = new Thread(RunTimerLoop);
					thread.Name = "TimerThread";
					thread.IsBackground = true;
					thread.Start();
				} else
					Running = true;
			}
		}

		/// <summary>
		/// Waits for the next timer tick. If the timer is not running, the function will still wait for the next tick.
		/// </summary>
		/// <param name="timeout">The timeout interval, in milliseconds. If timeout is -1, the function will wait for a tick indefnitely.
		/// If a nonzero value is specified, the function waits for a tick unless the specified timeout runs out.
		/// If timeout is zero, the function does not enter a wait state if the object is not signaled; it always returns immediately.</param>
		public void WaitForTick(int timeout = -1) {
			if (!DontWaitForInterval)
				NativeApi.WaitForSingleObject(handle, timeout);
		}

		private void RunTimerLoop() {
			IntPtr handle = this.handle;
			Action<AsyncTimer> tick;
			try {
				do {
					WaitForTick();
					if (IsDisposed)
						break;
					tick = Tick;
					if (tick != null) {
						try {
							tick(this);
						} catch (Exception ex) {
							if (Exceptions == ExceptionMode.Log)
								ErrorHandler.LogException(ex, ErrorHandler.ExceptionToDetailedString("An error happened while invoking a timer callback.", ex));
							else if (Exceptions == ExceptionMode.Ignore)
								throw;
						}
					}
					tick = TickFinished;
					if (tick != null) {
						try {
							tick(this);
						} catch (Exception ex) {
							if (Exceptions == ExceptionMode.Log)
								ErrorHandler.LogException(ex, ErrorHandler.ExceptionToDetailedString("An error happened while invoking a timer callback.", ex));
							else if (Exceptions == ExceptionMode.Ignore)
								throw;
						}
					}
				} while (!IsDisposed);
			} catch (ThreadAbortException) {
			}
			NativeApi.CloseHandle(handle);
			running = false;
		}

		/// <summary>
		/// Disposes of the resources used by the timer.
		/// </summary>
		~AsyncTimer() {
			Dispose();
		}

		/// <summary>
		/// Disposes of the resources used by the timer.
		/// </summary>
		public void Dispose() {
			if (IsDisposed)
				return;
			IntPtr handle = this.handle;
			this.handle = IntPtr.Zero;
			running = false;
			long targetInterval = 0L;
			NativeApi.SetWaitableTimer(handle, ref targetInterval, 0, IntPtr.Zero, IntPtr.Zero, true);
			if (Interlocked.Decrement(ref timerCount) == 0)
				NativeApi.TimeEndPeriod(6);
			GC.SuppressFinalize(this);
		}
	}
}