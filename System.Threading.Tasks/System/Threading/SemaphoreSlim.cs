// Decompiled with JetBrains decompiler
// Type: System.Threading.SemaphoreSlim
// Assembly: System.Threading, Version=1.0.2856.102, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DBA5258D-29E2-4AB4-BCBD-79B4549AF8F7
// Assembly location: C:\Users\mathu\Documents\Visual Studio 2015\Projects\ConsoleApplication1\packages\TaskParallelLibrary.1.0.2856.0\lib\Net35\System.Threading.dll

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Threading {
	/// <summary>
	/// Limits the number of threads that can access a resource or pool of resources concurrently.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The <see cref="T:System.Threading.SemaphoreSlim" /> provides a lightweight semaphore class that doesn't
	/// use Windows kernel semaphores.
	/// </para>
	/// <para>
	/// All public and protected members of <see cref="T:System.Threading.SemaphoreSlim" /> are thread-safe and may be used
	/// concurrently from multiple threads, with the exception of Dispose, which
	/// must only be used when all other operations on the <see cref="T:System.Threading.SemaphoreSlim" /> have
	/// completed.
	/// </para>
	/// </remarks>
	[DebuggerDisplay("Current Count = {m_currentCount}")]
	[ComVisible(false)]
	[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true, Synchronization = true)]
	public class SemaphoreSlim : IDisposable {
		/// <summary>
		/// Private helper method to wake up waiters when a cancellationToken gets canceled.
		/// </summary>
		private static Action<object> s_cancellationTokenCanceledEventHandler = new Action<object>(SemaphoreSlim.CancellationTokenCanceledEventHandler);
		private const int NO_MAXIMUM = 2147483647;
		private volatile int m_currentCount;
		private readonly int m_maxCount;
		private volatile int m_waitCount;
		private object m_lockObj;
		private ManualResetEvent m_waitHandle;

		/// <summary>
		/// Gets the current count of the <see cref="T:System.Threading.SemaphoreSlim" />.
		/// </summary>
		/// <value>The current count of the <see cref="T:System.Threading.SemaphoreSlim" />.</value>
		public int CurrentCount {
			get {
				return this.m_currentCount;
			}
		}

		/// <summary>
		/// Returns a <see cref="T:System.Threading.WaitHandle" /> that can be used to wait on the semaphore.
		/// </summary>
		/// <value>A <see cref="T:System.Threading.WaitHandle" /> that can be used to wait on the
		/// semaphore.</value>
		/// <remarks>
		/// A successful wait on the <see cref="P:System.Threading.SemaphoreSlim.AvailableWaitHandle" /> does not imply a successful wait on
		/// the <see cref="T:System.Threading.SemaphoreSlim" /> itself, nor does it decrement the semaphore's
		/// count. <see cref="P:System.Threading.SemaphoreSlim.AvailableWaitHandle" /> exists to allow a thread to block waiting on multiple
		/// semaphores, but such a wait should be followed by a true wait on the target semaphore.
		/// </remarks>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.SemaphoreSlim" /> has been disposed.</exception>
		public WaitHandle AvailableWaitHandle {
			get {
				this.CheckDispose();
				if (this.m_waitHandle != null)
					return (WaitHandle) this.m_waitHandle;
				lock (this.m_lockObj) {
					if (this.m_waitHandle == null)
						this.m_waitHandle = new ManualResetEvent(this.m_currentCount != 0);
				}
				return (WaitHandle) this.m_waitHandle;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Threading.SemaphoreSlim" /> class, specifying
		/// the initial number of requests that can be granted concurrently.
		/// </summary>
		/// <param name="initialCount">The initial number of requests for the semaphore that can be granted
		/// concurrently.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="initialCount" />
		/// is less than 0.</exception>
		public SemaphoreSlim(int initialCount)
		  : this(initialCount, int.MaxValue) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Threading.SemaphoreSlim" /> class, specifying
		/// the initial and maximum number of requests that can be granted concurrently.
		/// </summary>
		/// <param name="initialCount">The initial number of requests for the semaphore that can be granted
		/// concurrently.</param>
		/// <param name="maxCount">The maximum number of requests for the semaphore that can be granted
		/// concurrently.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException"> <paramref name="initialCount" />
		/// is less than 0. -or-
		/// <paramref name="initialCount" /> is greater than <paramref name="maxCount" />. -or-
		/// <paramref name="maxCount" /> is less than 0.</exception>
		public SemaphoreSlim(int initialCount, int maxCount) {
			if (initialCount < 0 || initialCount > maxCount)
				throw new ArgumentOutOfRangeException("initialCount", (object) initialCount, SemaphoreSlim.GetResourceString("SemaphoreSlim_ctor_InitialCountWrong"));
			if (maxCount <= 0)
				throw new ArgumentOutOfRangeException("maxCount", (object) maxCount, SemaphoreSlim.GetResourceString("SemaphoreSlim_ctor_MaxCountWrong"));
			this.m_maxCount = maxCount;
			this.m_lockObj = new object();
			this.m_currentCount = initialCount;
		}

		/// <summary>
		/// Blocks the current thread until it can enter the <see cref="T:System.Threading.SemaphoreSlim" />.
		/// </summary>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has already been
		/// disposed.</exception>
		public void Wait() {
			this.Wait(-1, new CancellationToken());
		}

		/// <summary>
		/// Blocks the current thread until it can enter the <see cref="T:System.Threading.SemaphoreSlim" />, while observing a
		/// <see cref="T:System.Threading.CancellationToken" />.
		/// </summary>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> token to
		/// observe.</param>
		/// <exception cref="T:System.OperationCanceledException"><paramref name="cancellationToken" /> was
		/// canceled.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has already been
		/// disposed.</exception>
		public void Wait(CancellationToken cancellationToken) {
			this.Wait(-1, cancellationToken);
		}

		/// <summary>
		/// Blocks the current thread until it can enter the <see cref="T:System.Threading.SemaphoreSlim" />, using a <see cref="T:System.TimeSpan" /> to measure the time interval.
		/// </summary>
		/// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds
		/// to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to wait indefinitely.
		/// </param>
		/// <returns>true if the current thread successfully entered the <see cref="T:System.Threading.SemaphoreSlim" />;
		/// otherwise, false.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="timeout" /> is a negative
		/// number other than -1 milliseconds, which represents an infinite time-out -or- timeout is greater
		/// than <see cref="F:System.Int32.MaxValue" />.</exception>
		public bool Wait(TimeSpan timeout) {
			long totalMilliseconds = (long) timeout.TotalMilliseconds;
			if (totalMilliseconds < -1L || totalMilliseconds > (long) int.MaxValue)
				throw new ArgumentOutOfRangeException("timeout", (object) timeout, SemaphoreSlim.GetResourceString("SemaphoreSlim_Wait_TimeoutWrong"));
			return this.Wait((int) timeout.TotalMilliseconds, new CancellationToken());
		}

		/// <summary>
		/// Blocks the current thread until it can enter the <see cref="T:System.Threading.SemaphoreSlim" />, using a <see cref="T:System.TimeSpan" /> to measure the time interval, while observing a <see cref="T:System.Threading.CancellationToken" />.
		/// </summary>
		/// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds
		/// to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to wait indefinitely.
		/// </param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to
		/// observe.</param>
		/// <returns>true if the current thread successfully entered the <see cref="T:System.Threading.SemaphoreSlim" />;
		/// otherwise, false.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="timeout" /> is a negative
		/// number other than -1 milliseconds, which represents an infinite time-out -or- timeout is greater
		/// than <see cref="F:System.Int32.MaxValue" />.</exception>
		/// <exception cref="T:System.OperationCanceledException"><paramref name="cancellationToken" /> was canceled.</exception>
		public bool Wait(TimeSpan timeout, CancellationToken cancellationToken) {
			long totalMilliseconds = (long) timeout.TotalMilliseconds;
			if (totalMilliseconds < -1L || totalMilliseconds > (long) int.MaxValue)
				throw new ArgumentOutOfRangeException("timeout", (object) timeout, SemaphoreSlim.GetResourceString("SemaphoreSlim_Wait_TimeoutWrong"));
			return this.Wait((int) timeout.TotalMilliseconds, cancellationToken);
		}

		/// <summary>
		/// Blocks the current thread until it can enter the <see cref="T:System.Threading.SemaphoreSlim" />, using a 32-bit
		/// signed integer to measure the time interval.
		/// </summary>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" />(-1) to wait indefinitely.</param>
		/// <returns>true if the current thread successfully entered the <see cref="T:System.Threading.SemaphoreSlim" />;
		/// otherwise, false.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="millisecondsTimeout" /> is a
		/// negative number other than -1, which represents an infinite time-out.</exception>
		public bool Wait(int millisecondsTimeout) {
			return this.Wait(millisecondsTimeout, new CancellationToken());
		}

		/// <summary>
		/// Blocks the current thread until it can enter the <see cref="T:System.Threading.SemaphoreSlim" />,
		/// using a 32-bit signed integer to measure the time interval,
		/// while observing a <see cref="T:System.Threading.CancellationToken" />.
		/// </summary>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" />(-1) to
		/// wait indefinitely.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to observe.</param>
		/// <returns>true if the current thread successfully entered the <see cref="T:System.Threading.SemaphoreSlim" />; otherwise, false.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="millisecondsTimeout" /> is a negative number other than -1,
		/// which represents an infinite time-out.</exception>
		/// <exception cref="T:System.OperationCanceledException"><paramref name="cancellationToken" /> was canceled.</exception>
		public bool Wait(int millisecondsTimeout, CancellationToken cancellationToken) {
			this.CheckDispose();
			if (millisecondsTimeout < -1)
				throw new ArgumentOutOfRangeException("totalMilliSeconds", (object) millisecondsTimeout, SemaphoreSlim.GetResourceString("SemaphoreSlim_Wait_TimeoutWrong"));
			cancellationToken.ThrowIfCancellationRequested();
			long startTimeTicks = 0;
			if (millisecondsTimeout != -1 && millisecondsTimeout > 0)
				startTimeTicks = DateTime.UtcNow.Ticks;
			bool taken = false;
			CancellationTokenRegistration tokenRegistration = cancellationToken.Register(SemaphoreSlim.s_cancellationTokenCanceledEventHandler, (object) this);
			try {
				SpinWait spinWait = new SpinWait();
				while (this.m_currentCount == 0) {
					if (!spinWait.NextSpinWillYield)
						spinWait.SpinOnce();
					else
						break;
				}
				try {
				} finally {
					Monitor.Enter(this.m_lockObj);
					if (taken)
						++this.m_waitCount;
				}
				if (this.m_currentCount == 0 && (millisecondsTimeout == 0 || !this.WaitUntilCountOrTimeout(millisecondsTimeout, startTimeTicks, cancellationToken)))
					return false;
				--this.m_currentCount;
				if (this.m_waitHandle != null) {
					if (this.m_currentCount == 0)
						this.m_waitHandle.Reset();
				}
			} finally {
				if (taken) {
					--this.m_waitCount;
					Monitor.Exit(this.m_lockObj);
				}
				tokenRegistration.Dispose();
			}
			return true;
		}

		/// <summary>
		/// Local helper function, waits on the monitor until the monitor recieves signal or the
		/// timeout is expired
		/// </summary>
		/// <param name="millisecondsTimeout">The maximum timeout</param>
		/// <param name="startTimeTicks">The start ticks to calculate the elapsed time</param>
		/// <param name="cancellationToken">The CancellationToken to observe.</param>
		/// <returns>true if the monitor recieved a signal, false if the timeout expired</returns>
		private bool WaitUntilCountOrTimeout(int millisecondsTimeout, long startTimeTicks, CancellationToken cancellationToken) {
			int millisecondsTimeout1 = -1;
			while (this.m_currentCount == 0) {
				cancellationToken.ThrowIfCancellationRequested();
				if (millisecondsTimeout != -1) {
					millisecondsTimeout1 = SemaphoreSlim.UpdateTimeOut(startTimeTicks, millisecondsTimeout);
					if (millisecondsTimeout1 <= 0)
						return false;
				}
				if (!Monitor.Wait(this.m_lockObj, millisecondsTimeout1))
					return false;
			}
			return true;
		}

		/// <summary>
		/// Exits the <see cref="T:System.Threading.SemaphoreSlim" /> once.
		/// </summary>
		/// <returns>The previous count of the <see cref="T:System.Threading.SemaphoreSlim" />.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has already been
		/// disposed.</exception>
		public int Release() {
			return this.Release(1);
		}

		/// <summary>
		/// Exits the <see cref="T:System.Threading.SemaphoreSlim" /> a specified number of times.
		/// </summary>
		/// <param name="releaseCount">The number of times to exit the semaphore.</param>
		/// <returns>The previous count of the <see cref="T:System.Threading.SemaphoreSlim" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="releaseCount" /> is less
		/// than 1.</exception>
		/// <exception cref="T:System.Threading.SemaphoreFullException">The <see cref="T:System.Threading.SemaphoreSlim" /> has
		/// already reached its maximum size.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has already been
		/// disposed.</exception>
		public int Release(int releaseCount) {
			this.CheckDispose();
			if (releaseCount < 1)
				throw new ArgumentOutOfRangeException("releaseCount", (object) releaseCount, SemaphoreSlim.GetResourceString("SemaphoreSlim_Release_CountWrong"));
			lock (this.m_lockObj) {
				if (this.m_maxCount - this.m_currentCount < releaseCount)
					throw new SemaphoreFullException();
				this.m_currentCount += releaseCount;
				if (this.m_currentCount == 1 || this.m_waitCount == 1)
					Monitor.Pulse(this.m_lockObj);
				else if (this.m_waitCount > 1)
					Monitor.PulseAll(this.m_lockObj);
				if (this.m_waitHandle != null && this.m_currentCount - releaseCount == 0)
					this.m_waitHandle.Set();
				return this.m_currentCount - releaseCount;
			}
		}

		~SemaphoreSlim() {
			Dispose();
		}

		/// <summary>
		/// Releases all resources used by the current instance of <see cref="T:System.Threading.SemaphoreSlim" />.
		/// </summary>
		/// <remarks>
		/// Unlike most of the members of <see cref="T:System.Threading.SemaphoreSlim" />, <see cref="M:System.Threading.SemaphoreSlim.Dispose" /> is not
		/// thread-safe and may not be used concurrently with other members of this instance.
		/// </remarks>
		public void Dispose() {
			this.Dispose(true);
			GC.SuppressFinalize((object) this);
		}

		/// <summary>
		/// When overridden in a derived class, releases the unmanaged resources used by the
		/// <see cref="T:System.Threading.ManualResetEventSlim" />, and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources;
		/// false to release only unmanaged resources.</param>
		/// <remarks>
		/// Unlike most of the members of <see cref="T:System.Threading.SemaphoreSlim" />, <see cref="M:System.Threading.SemaphoreSlim.Dispose(System.Boolean)" /> is not
		/// thread-safe and may not be used concurrently with other members of this instance.
		/// </remarks>
		protected virtual void Dispose(bool disposing) {
			if (!disposing)
				return;
			if (this.m_waitHandle != null) {
				this.m_waitHandle.Close();
				this.m_waitHandle = (ManualResetEvent) null;
			}
			this.m_lockObj = (object) null;
		}

		/// <summary>Helper function to measure and update the wait time</summary>
		/// <param name="startTimeTicks"> The first time (in Ticks) observed when the wait started</param>
		/// <param name="originalWaitMillisecondsTimeout">The orginal wait timeoutout in milliseconds</param>
		/// <returns>The new wait time in milliseconds, -1 if the time expired</returns>
		private static int UpdateTimeOut(long startTimeTicks, int originalWaitMillisecondsTimeout) {
			long num1 = (DateTime.UtcNow.Ticks - startTimeTicks) / 10000L;
			if (num1 > (long) int.MaxValue)
				return 0;
			int num2 = originalWaitMillisecondsTimeout - (int) num1;
			if (num2 <= 0)
				return 0;
			return num2;
		}

		private static void CancellationTokenCanceledEventHandler(object obj) {
			SemaphoreSlim semaphoreSlim = obj as SemaphoreSlim;
			lock (semaphoreSlim.m_lockObj)
				Monitor.PulseAll(semaphoreSlim.m_lockObj);
		}

		/// <summary>
		/// Checks the dispose status by checking the lock object, if it is null means that object
		/// has been disposed and throw ObjectDisposedException
		/// </summary>
		private void CheckDispose() {
			if (this.m_lockObj == null)
				throw new ObjectDisposedException((string) null, SemaphoreSlim.GetResourceString("SemaphoreSlim_Disposed"));
		}

		/// <summary>
		/// local helper function to retrieve the exception string message from the resource file
		/// </summary>
		/// <param name="str">The key string</param>
		private static string GetResourceString(string str) {
			return str;
		}
	}
}