using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Threading {
	/// <summary>
	/// Provides a mutual exclusion lock primitive where a thread trying to acquire the lock waits in a loop
	/// repeatedly checking until the lock becomes available.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Spin locks can be used for leaf-level locks where the object allocation implied by using a <see cref="T:System.Threading.Monitor" />, in size or due to garbage collection pressure, is overly
	/// expensive. Avoiding blocking is another reason that a spin lock can be useful, however if you expect
	/// any significant amount of blocking, you are probably best not using spin locks due to excessive
	/// spinning. Spinning can be beneficial when locks are fine grained and large in number (for example, a
	/// lock per node in a linked list) as well as when lock hold times are always extremely short. In
	/// general, while holding a spin lock, one should avoid blocking, calling anything that itself may
	/// block, holding more than one spin lock at once, making dynamically dispatched calls (interface and
	/// virtuals), making statically dispatched calls into any code one doesn't own, or allocating memory.
	/// </para>
	/// <para>
	/// <see cref="T:System.Threading.SpinLock" /> should only be used when it's been determined that doing so will improve an
	/// application's performance. It's also important to note that <see cref="T:System.Threading.SpinLock" /> is a value type,
	/// for performance reasons. As such, one must be very careful not to accidentally copy a SpinLock
	/// instance, as the two instances (the original and the copy) would then be completely independent of
	/// one another, which would likely lead to erroneous behavior of the application. If a SpinLock instance
	/// must be passed around, it should be passed by reference rather than by value.
	/// </para>
	/// <para>
	/// Do not store <see cref="T:System.Threading.SpinLock" /> instances in readonly fields.
	/// </para>
	/// <para>
	/// All members of <see cref="T:System.Threading.SpinLock" /> are thread-safe and may be used from multiple threads
	/// concurrently.
	/// </para>
	/// </remarks>
	[DebuggerDisplay("IsHeld = {IsHeld}")]
	[ComVisible(false)]
	[DebuggerTypeProxy(typeof(SpinLock.SystemThreading_SpinLockDebugView))]
	[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true, Synchronization = true)]
	public struct SpinLock {
		private static int MAXIMUM_WAITERS = 2147483646;
		private const int SPINNING_FACTOR = 100;
		private const int SLEEP_ONE_FREQUENCY = 40;
		private const int SLEEP_ZERO_FREQUENCY = 10;
		private const int TIMEOUT_CHECK_FREQUENCY = 10;
		private const int LOCK_ID_DISABLE_MASK = -2147483648;
		private const int LOCK_ANONYMOUS_OWNED = 1;
		private const int WAITERS_MASK = 2147483646;
		private const int LOCK_UNOWNED = 0;
		private volatile int m_owner;

		/// <summary>
		/// Gets whether the lock is currently held by any thread.
		/// </summary>
		public bool IsHeld {
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get {
				if (this.IsThreadOwnerTrackingEnabled)
					return this.m_owner != 0;
				return (this.m_owner & 1) != 0;
			}
		}

		/// <summary>
		/// Gets whether the lock is currently held by any thread.
		/// </summary>
		/// <summary>Gets whether the lock is held by the current thread.</summary>
		/// <remarks>
		/// If the lock was initialized to track owner threads, this will return whether the lock is acquired
		/// by the current thread. It is invalid to use this property when the lock was initialized to not
		/// track thread ownership.
		/// </remarks>
		/// <exception cref="T:System.InvalidOperationException">
		/// Thread ownership tracking is disabled.
		/// </exception>
		public bool IsHeldByCurrentThread {
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get {
				if (!this.IsThreadOwnerTrackingEnabled)
					throw new InvalidOperationException("SpinLock_IsHeldByCurrentThread");
				return (this.m_owner & int.MaxValue) == Thread.CurrentThread.ManagedThreadId;
			}
		}

		/// <summary>Gets whether thread ownership tracking is enabled for this instance.</summary>
		public bool IsThreadOwnerTrackingEnabled {
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get {
				return (this.m_owner & int.MinValue) == 0;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Threading.SpinLock" />
		/// structure with the option to track thread IDs to improve debugging.
		/// </summary>
		/// <remarks>
		/// The default constructor for <see cref="T:System.Threading.SpinLock" /> tracks thread ownership.
		/// </remarks>
		/// <param name="enableThreadOwnerTracking">Whether to capture and use thread IDs for debugging
		/// purposes.</param>
		public SpinLock(bool enableThreadOwnerTracking) {
			this.m_owner = 0;
			if (enableThreadOwnerTracking)
				return;
			this.m_owner |= int.MinValue;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Threading.SpinLock" />
		/// structure with the option to track thread IDs to improve debugging.
		/// </summary>
		/// <remarks>
		/// The default constructor for <see cref="T:System.Threading.SpinLock" /> tracks thread ownership.
		/// </remarks>
		/// <summary>
		/// Acquires the lock in a reliable manner, such that even if an exception occurs within the method
		/// call, <paramref name="lockTaken" /> can be examined reliably to determine whether the lock was
		/// acquired.
		/// </summary>
		/// <remarks>
		/// <see cref="T:System.Threading.SpinLock" /> is a non-reentrant lock, meaning that if a thread holds the lock, it is
		/// not allowed to enter the lock again. If thread ownership tracking is enabled (whether it's
		/// enabled is available through <see cref="P:System.Threading.SpinLock.IsThreadOwnerTrackingEnabled" />), an exception will be
		/// thrown when a thread tries to re-enter a lock it already holds. However, if thread ownership
		/// tracking is disabled, attempting to enter a lock already held will result in deadlock.
		/// </remarks>
		/// <param name="lockTaken">True if the lock is acquired; otherwise, false. <paramref name="lockTaken" /> must be initialized to false prior to calling this method.</param>
		/// <exception cref="T:System.Threading.LockRecursionException">
		/// Thread ownership tracking is enabled, and the current thread has already acquired this lock.
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		/// The <paramref name="lockTaken" /> argument must be initialized to false prior to calling Enter.
		/// </exception>
		public void Enter(ref bool lockTaken) {
			if (lockTaken) {
				lockTaken = false;
				throw new ArgumentException("SpinLock_TryReliableEnter_ArgumentException");
			}
			int owner = this.m_owner;
			int num = 0;
			if ((this.m_owner & int.MinValue) == 0) {
				if (owner == 0)
					num = Thread.CurrentThread.ManagedThreadId;
			} else if ((owner & 1) == 0)
				num = owner | 1;
			if (num != 0) {
				Thread.BeginCriticalRegion();
				if (Interlocked.CompareExchange(ref this.m_owner, num, owner) == owner) {
					lockTaken = true;
					return;
				}
				Thread.EndCriticalRegion();
			}
			this.ContinueTryEnter(-1, ref lockTaken);
		}

		/// <summary>
		/// Attempts to acquire the lock in a reliable manner, such that even if an exception occurs within
		/// the method call, <paramref name="lockTaken" /> can be examined reliably to determine whether the
		/// lock was acquired.
		/// </summary>
		/// <remarks>
		/// Unlike <see cref="M:System.Threading.SpinLock.Enter(System.Boolean@)" />, TryEnter will not block waiting for the lock to be available. If the
		/// lock is not available when TryEnter is called, it will return immediately without any further
		/// spinning.
		/// </remarks>
		/// <param name="lockTaken">True if the lock is acquired; otherwise, false. <paramref name="lockTaken" /> must be initialized to false prior to calling this method.</param>
		/// <exception cref="T:System.Threading.LockRecursionException">
		/// Thread ownership tracking is enabled, and the current thread has already acquired this lock.
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		/// The <paramref name="lockTaken" /> argument must be initialized to false prior to calling TryEnter.
		/// </exception>
		public void TryEnter(ref bool lockTaken) {
			this.TryEnter(0, ref lockTaken);
		}

		/// <summary>
		/// Attempts to acquire the lock in a reliable manner, such that even if an exception occurs within
		/// the method call, <paramref name="lockTaken" /> can be examined reliably to determine whether the
		/// lock was acquired.
		/// </summary>
		/// <remarks>
		/// Unlike <see cref="M:System.Threading.SpinLock.Enter(System.Boolean@)" />, TryEnter will not block indefinitely waiting for the lock to be
		/// available. It will block until either the lock is available or until the <paramref name="timeout" />
		/// has expired.
		/// </remarks>
		/// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds
		/// to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to wait indefinitely.
		/// </param>
		/// <param name="lockTaken">True if the lock is acquired; otherwise, false. <paramref name="lockTaken" /> must be initialized to false prior to calling this method.</param>
		/// <exception cref="T:System.Threading.LockRecursionException">
		/// Thread ownership tracking is enabled, and the current thread has already acquired this lock.
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		/// The <paramref name="lockTaken" /> argument must be initialized to false prior to calling TryEnter.
		/// </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="timeout" /> is a negative
		/// number other than -1 milliseconds, which represents an infinite time-out -or- timeout is greater
		/// than <see cref="F:System.Int32.MaxValue" /> milliseconds.
		/// </exception>
		public void TryEnter(TimeSpan timeout, ref bool lockTaken) {
			long totalMilliseconds = (long) timeout.TotalMilliseconds;
			if (totalMilliseconds < -1L || totalMilliseconds > (long) int.MaxValue)
				throw new ArgumentOutOfRangeException("timeout", (object) timeout, "SpinLock_TryEnter_ArgumentOutOfRange");
			this.TryEnter((int) timeout.TotalMilliseconds, ref lockTaken);
		}

		/// <summary>
		/// Attempts to acquire the lock in a reliable manner, such that even if an exception occurs within
		/// the method call, <paramref name="lockTaken" /> can be examined reliably to determine whether the
		/// lock was acquired.
		/// </summary>
		/// <remarks>
		/// Unlike <see cref="M:System.Threading.SpinLock.Enter(System.Boolean@)" />, TryEnter will not block indefinitely waiting for the lock to be
		/// available. It will block until either the lock is available or until the <paramref name="millisecondsTimeout" /> has expired.
		/// </remarks>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to wait indefinitely.</param>
		/// <param name="lockTaken">True if the lock is acquired; otherwise, false. <paramref name="lockTaken" /> must be initialized to false prior to calling this method.</param>
		/// <exception cref="T:System.Threading.LockRecursionException">
		/// Thread ownership tracking is enabled, and the current thread has already acquired this lock.
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		/// The <paramref name="lockTaken" /> argument must be initialized to false prior to calling TryEnter.
		/// </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="millisecondsTimeout" /> is
		/// a negative number other than -1, which represents an infinite time-out.</exception>
		public void TryEnter(int millisecondsTimeout, ref bool lockTaken) {
			if (lockTaken) {
				lockTaken = false;
				throw new ArgumentException("SpinLock_TryReliableEnter_ArgumentException");
			}
			if (millisecondsTimeout < -1)
				throw new ArgumentOutOfRangeException("millisecondsTimeout", (object) millisecondsTimeout, "SpinLock_TryEnter_ArgumentOutOfRange");
			int owner = this.m_owner;
			int num = 0;
			if (this.IsThreadOwnerTrackingEnabled) {
				if (owner == 0)
					num = Thread.CurrentThread.ManagedThreadId;
			} else if ((owner & 1) == 0)
				num = owner | 1;
			if (num != 0) {
				Thread.BeginCriticalRegion();
				if (Interlocked.CompareExchange(ref this.m_owner, num, owner) == owner) {
					lockTaken = true;
					return;
				}
				Thread.EndCriticalRegion();
			}
			this.ContinueTryEnter(millisecondsTimeout, ref lockTaken);
		}

		/// <summary>
		/// Try acquire the lock with long path, this is usually called after the first path in Enter and
		/// TryEnter failed The reason for short path is to make it inline in the run time which improves the
		/// performance. This method assumed that the parameter are validated in Enter ir TryENter method
		/// </summary>
		/// <param name="millisecondsTimeout">The timeout milliseconds</param>
		/// <param name="lockTaken">The lockTaken param</param>
		private void ContinueTryEnter(int millisecondsTimeout, ref bool lockTaken) {
			long startTicks = 0;
			if (millisecondsTimeout != -1 && millisecondsTimeout != 0)
				startTicks = DateTime.UtcNow.Ticks;
			if (this.IsThreadOwnerTrackingEnabled) {
				this.ContinueTryEnterWithThreadTracking(millisecondsTimeout, startTicks, ref lockTaken);
			} else {
				SpinWait spinWait = new SpinWait();
				int owner1;
				while (true) {
					owner1 = this.m_owner;
					if ((owner1 & 1) == 0) {
						Thread.BeginCriticalRegion();
						if (Interlocked.CompareExchange(ref this.m_owner, owner1 | 1, owner1) != owner1)
							Thread.EndCriticalRegion();
						else
							break;
					} else if ((owner1 & 2147483646) == SpinLock.MAXIMUM_WAITERS || Interlocked.CompareExchange(ref this.m_owner, owner1 + 2, owner1) == owner1)
						goto label_11;
					spinWait.SpinOnce();
				}
				lockTaken = true;
				return;
				label_11:
				if (millisecondsTimeout == 0 || millisecondsTimeout != -1 && SpinLock.TimeoutExpired(startTicks, millisecondsTimeout)) {
					this.DecrementWaiters();
				} else {
					int num1 = (owner1 + 2 & 2147483646) / 2;
					int processorCount = Environment.ProcessorCount;
					if (num1 < processorCount) {
						int num2 = 1;
						for (int index = 1; index <= num1 * 100; ++index) {
							Thread.SpinWait((num1 + index) * 100 * num2);
							if (num2 < processorCount)
								++num2;
							int owner2 = this.m_owner;
							if ((owner2 & 1) == 0) {
								Thread.BeginCriticalRegion();
								if (Interlocked.CompareExchange(ref this.m_owner, (owner2 & 2147483646) == 0 ? owner2 | 1 : owner2 - 2 | 1, owner2) == owner2) {
									lockTaken = true;
									return;
								}
								Thread.EndCriticalRegion();
							}
						}
					}
					if (millisecondsTimeout != -1 && SpinLock.TimeoutExpired(startTicks, millisecondsTimeout)) {
						this.DecrementWaiters();
					} else {
						int num2 = 0;
						while (true) {
							int owner2 = this.m_owner;
							if ((owner2 & 1) == 0) {
								Thread.BeginCriticalRegion();
								if (Interlocked.CompareExchange(ref this.m_owner, (owner2 & 2147483646) == 0 ? owner2 | 1 : owner2 - 2 | 1, owner2) != owner2)
									Thread.EndCriticalRegion();
								else
									break;
							}
							if (num2 % 40 == 0)
								Thread.Sleep(1);
							else if (num2 % 10 == 0)
								Thread.Sleep(0);
							if (num2 % 10 != 0 || millisecondsTimeout == -1 || !SpinLock.TimeoutExpired(startTicks, millisecondsTimeout))
								++num2;
							else
								goto label_36;
						}
						lockTaken = true;
						return;
						label_36:
						this.DecrementWaiters();
					}
				}
			}
		}

		/// <summary>
		/// decrements the waiters, in case of the timeout is expired
		/// </summary>
		private void DecrementWaiters() {
			SpinWait spinWait = new SpinWait();
			while (true) {
				int owner = this.m_owner;
				if ((owner & 2147483646) != 0 && Interlocked.CompareExchange(ref this.m_owner, owner - 2, owner) != owner)
					spinWait.SpinOnce();
				else
					break;
			}
		}

		/// <summary>ContinueTryEnter for the thread tracking mode enabled</summary>
		private void ContinueTryEnterWithThreadTracking(int millisecondsTimeout, long startTicks, ref bool lockTaken) {
			int comparand = 0;
			int managedThreadId = Thread.CurrentThread.ManagedThreadId;
			if (this.m_owner == managedThreadId)
				throw new InvalidOperationException("SpinLock_TryEnter_LockRecursionException");
			SpinWait spinWait = new SpinWait();
			do {
				spinWait.SpinOnce();
				if (this.m_owner == comparand) {
					Thread.BeginCriticalRegion();
					if (Interlocked.CompareExchange(ref this.m_owner, managedThreadId, comparand) == comparand) {
						lockTaken = true;
						break;
					}
					Thread.EndCriticalRegion();
				}
			}
			while (millisecondsTimeout != 0 && (millisecondsTimeout == -1 || !spinWait.NextSpinWillYield || !SpinLock.TimeoutExpired(startTicks, millisecondsTimeout)));
		}

		/// <summary>Helper function to validate the timeout</summary>
		/// <param name="startTicks"> The start time in ticks</param>
		/// <param name="originalWaitTime">The orginal wait time</param>
		/// <returns>True if expired, false otherwise</returns>
		private static bool TimeoutExpired(long startTicks, int originalWaitTime) {
			return DateTime.UtcNow.Ticks - startTicks >= (long) originalWaitTime * 10000L;
		}

		/// <summary>Releases the lock.</summary>
		/// <remarks>
		/// The default overload of <see cref="M:System.Threading.SpinLock.Exit" /> provides the same behavior as if calling <see cref="M:System.Threading.SpinLock.Exit(System.Boolean)" /> using true as the argument.
		/// </remarks>
		/// <exception cref="T:System.Threading.SynchronizationLockException">
		/// Thread ownership tracking is enabled, and the current thread is not the owner of this lock.
		/// </exception>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public void Exit() {
			this.Exit(true);
		}

		/// <summary>Releases the lock.</summary>
		/// <param name="useMemoryBarrier">
		/// A Boolean value that indicates whether a memory fence should be issued in order to immediately
		/// publish the exit operation to other threads.
		/// </param>
		/// <remarks>
		/// Calling <see cref="M:System.Threading.SpinLock.Exit(System.Boolean)" /> with the <paramref name="useMemoryBarrier" /> argument set to
		/// true will improve the fairness of the lock at the expense of some performance. The default <see cref="M:System.Threading.SpinLock.Enter(System.Boolean@)" />
		/// overload behaves as if specifying true for <paramref name="useMemoryBarrier" />.
		/// </remarks>
		/// <exception cref="T:System.Threading.SynchronizationLockException">
		/// Thread ownership tracking is enabled, and the current thread is not the owner of this lock.
		/// </exception>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public void Exit(bool useMemoryBarrier) {
			if (this.IsThreadOwnerTrackingEnabled && !this.IsHeldByCurrentThread)
				throw new SynchronizationLockException("SpinLock_Exit_SynchronizationLockException");
			if (useMemoryBarrier) {
				if (this.IsThreadOwnerTrackingEnabled)
					Interlocked.Exchange(ref this.m_owner, 0);
				else
					Interlocked.Decrement(ref this.m_owner);
			} else
				this.m_owner = !this.IsThreadOwnerTrackingEnabled ? this.m_owner - 1 : 0;
			Thread.EndCriticalRegion();
		}

		/// <summary>
		/// Internal class used by debug type proxy attribute to display the owner thread ID
		/// </summary>
		internal class SystemThreading_SpinLockDebugView {
			private SpinLock m_spinLock;

			/// <summary>
			/// Checks if the lock is held by the current thread or not
			/// </summary>
			public bool? IsHeldByCurrentThread {
				get {
					try {
						return new bool?(this.m_spinLock.IsHeldByCurrentThread);
					} catch (InvalidOperationException) {
						return new bool?();
					}
				}
			}

			/// <summary>Gets the current owner thread, zero if it is released</summary>
			public int? OwnerThreadID {
				get {
					if (this.m_spinLock.IsThreadOwnerTrackingEnabled)
						return new int?(this.m_spinLock.m_owner);
					return new int?();
				}
			}

			/// <summary>
			///  Gets whether the lock is currently held by any thread or not.
			/// </summary>
			public bool IsHeld {
				get {
					return this.m_spinLock.IsHeld;
				}
			}

			/// <summary>SystemThreading_SpinLockDebugView constructor</summary>
			/// <param name="spinLock">The SpinLock to be proxied.</param>
			public SystemThreading_SpinLockDebugView(SpinLock spinLock) {
				this.m_spinLock = spinLock;
			}
		}
	}
}