using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Threading {
	/// <summary>
	/// Enables multiple tasks to cooperatively work on an algorithm in parallel through multiple phases.
	/// </summary>
	/// <remarks>
	/// <para>
	/// A group of tasks cooperate by moving through a series of phases, where each in the group signals it
	/// has arrived at the <see cref="T:System.Threading.Barrier" /> in a given phase and implicitly waits for all others to
	/// arrive. The same <see cref="T:System.Threading.Barrier" /> can be used for multiple phases.
	/// </para>
	/// <para>
	/// All public and protected members of <see cref="T:System.Threading.Barrier" /> are thread-safe and may be used
	/// concurrently from multiple threads, with the exception of Dispose, which
	/// must only be used when all other operations on the <see cref="T:System.Threading.Barrier" /> have
	/// completed.
	/// </para>
	/// </remarks>
	[DebuggerDisplay("Participant Count={ParticipantCount},Participants Remaining={ParticipantsRemaining}")]
	[ComVisible(false)]
	[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true, Synchronization = true)]
	public class Barrier : IDisposable {
		private const int CURRENT_MASK = 2147418112;
		private const int TOTAL_MASK = 32767;
		private const int SENSE_MASK = -2147483648;
		private const int MAX_PARTICIPANTS = 32767;
		private volatile int m_currentTotalCount;
		private long m_currentPhase;
		private bool m_disposed;
		private ManualResetEventSlim m_oddEvent;
		private ManualResetEventSlim m_evenEvent;
		private ExecutionContext m_ownerThreadContext;
		private Action<Barrier> m_postPhaseAction;
		private Exception m_exception;
		private int m_actionCallerID;

		/// <summary>
		/// Gets the number of participants in the barrier that haven’t yet signaled
		/// in the current phase.
		/// </summary>
		/// <remarks>
		/// This could be 0 during a post-phase action delegate execution or if the
		/// ParticipantCount is 0.
		/// </remarks>
		public int ParticipantsRemaining {
			get {
				int currentTotalCount = this.m_currentTotalCount;
				return (currentTotalCount & (int) short.MaxValue) - ((currentTotalCount & 2147418112) >> 16);
			}
		}

		/// <summary>Gets the total number of participants in the barrier.</summary>
		public int ParticipantCount {
			get {
				return this.m_currentTotalCount & (int) short.MaxValue;
			}
		}

		/// <summary>Gets the number of the barrier's current phase.</summary>
		public long CurrentPhaseNumber {
			get {
				return this.m_currentPhase;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Threading.Barrier" /> class.
		/// </summary>
		/// <param name="participantCount">The number of participating threads.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException"> <paramref name="participantCount" /> is less than 0
		/// or greater than <see cref="T:System.Int32.MaxValue" />.</exception>
		public Barrier(int participantCount)
		  : this(participantCount, (Action<Barrier>) null) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Threading.Barrier" /> class.
		/// </summary>
		/// <param name="participantCount">The number of participating threads.</param>
		/// <param name="postPhaseAction">The <see cref="T:System.Action`1" /> to be executed after each
		/// phase.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException"> <paramref name="participantCount" /> is less than 0
		/// or greater than <see cref="T:System.Int32.MaxValue" />.</exception>
		/// <remarks>
		/// The <paramref name="postPhaseAction" /> delegate will be executed after
		/// all participants have arrived at the barrier in one phase.  The participants
		/// will not be released to the next phase until the postPhaseAction delegate
		/// has completed execution.
		/// </remarks>
		public Barrier(int participantCount, Action<Barrier> postPhaseAction) {
			if (participantCount < 0 || participantCount > (int) short.MaxValue)
				throw new ArgumentOutOfRangeException("participantCount", (object) participantCount, "Barrier_ctor_ArgumentOutOfRange");
			this.m_currentTotalCount = participantCount;
			this.m_postPhaseAction = postPhaseAction;
			this.m_oddEvent = new ManualResetEventSlim(true);
			this.m_evenEvent = new ManualResetEventSlim(false);
			if (postPhaseAction != null && !ExecutionContext.IsFlowSuppressed())
				this.m_ownerThreadContext = ExecutionContext.Capture();
			this.m_actionCallerID = 0;
		}

		/// <summary>
		/// Extract the three variables current, total and sense from a given big variable
		/// </summary>
		/// <param name="currentTotal">The integer variable that contains the other three variables</param>
		/// <param name="current">The current cparticipant count</param>
		/// <param name="total">The total participants count</param>
		/// <param name="sense">The sense flag</param>
		private void GetCurrentTotal(int currentTotal, out int current, out int total, out bool sense) {
			total = currentTotal & (int) short.MaxValue;
			current = (currentTotal & 2147418112) >> 16;
			sense = (currentTotal & int.MinValue) == 0;
		}

		/// <summary>
		/// Write the three variables current. total and the sense to the m_currentTotal
		/// </summary>
		/// <param name="currentTotal">The old current total to compare</param>
		/// <param name="current">The current cparticipant count</param>
		/// <param name="total">The total participants count</param>
		/// <param name="sense">The sense flag</param>
		/// <returns>True if the CAS succeeded, false otherwise</returns>
		private bool SetCurrentTotal(int currentTotal, int current, int total, bool sense) {
			int num = current << 16 | total;
			if (!sense)
				num |= int.MinValue;
			return Interlocked.CompareExchange(ref this.m_currentTotalCount, num, currentTotal) == currentTotal;
		}

		/// <summary>
		/// Notifies the <see cref="T:System.Threading.Barrier" /> that there will be an additional participant.
		/// </summary>
		/// <returns>The phase number of the barrier in which the new participants will first
		/// participate.</returns>
		/// <exception cref="T:System.InvalidOperationException">
		/// Adding a participant would cause the barrier's participant count to
		/// exceed <see cref="T:System.Int16.MaxValue" />.
		/// </exception>
		/// <exception cref="T:System.InvalidOperationException">
		/// The method was invoked from within a post-phase action.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has already been
		/// disposed.</exception>
		public long AddParticipant() {
			try {
				return this.AddParticipants(1);
			} catch (ArgumentOutOfRangeException) {
				throw new InvalidOperationException("Barrier_AddParticipants_Overflow_ArgumentOutOfRange");
			}
		}

		/// <summary>
		/// Notifies the <see cref="T:System.Threading.Barrier" /> that there will be additional participants.
		/// </summary>
		/// <param name="participantCount">The number of additional participants to add to the
		/// barrier.</param>
		/// <returns>The phase number of the barrier in which the new participants will first
		/// participate.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="participantCount" /> is less than
		/// 0.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Adding <paramref name="participantCount" /> participants would cause the
		/// barrier's participant count to exceed <see cref="T:System.Int16.MaxValue" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		/// The method was invoked from within a post-phase action.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has already been
		/// disposed.</exception>
		public long AddParticipants(int participantCount) {
			this.ThrowIfDisposed();
			if (participantCount < 1)
				throw new ArgumentOutOfRangeException("participantCount", (object) participantCount, "Barrier_AddParticipants_NonPositive_ArgumentOutOfRange");
			if (participantCount > (int) short.MaxValue)
				throw new ArgumentOutOfRangeException("participantCount", "Barrier_AddParticipants_Overflow_ArgumentOutOfRange");
			if (this.m_actionCallerID != 0 && Thread.CurrentThread.ManagedThreadId == this.m_actionCallerID)
				throw new InvalidOperationException("Barrier_InvalidOperation_CalledFromPHA");
			SpinWait spinWait = new SpinWait();
			bool sense;
			while (true) {
				int currentTotalCount = this.m_currentTotalCount;
				int current;
				int total;
				this.GetCurrentTotal(currentTotalCount, out current, out total, out sense);
				if (participantCount + total <= (int) short.MaxValue) {
					if (!this.SetCurrentTotal(currentTotalCount, current, total + participantCount, sense))
						spinWait.SpinOnce();
					else
						goto label_10;
				} else
					break;
			}
			throw new ArgumentOutOfRangeException("participantCount", "Barrier_AddParticipants_Overflow_ArgumentOutOfRange");
			label_10:
			long currentPhase = this.m_currentPhase;
			long num = sense != (currentPhase % 2L == 0L) ? currentPhase + 1L : currentPhase;
			if (num != currentPhase) {
				if (sense)
					this.m_oddEvent.Wait();
				else
					this.m_evenEvent.Wait();
			} else if (sense && this.m_evenEvent.IsSet)
				this.m_evenEvent.Reset();
			else if (!sense && this.m_oddEvent.IsSet)
				this.m_oddEvent.Reset();
			return num;
		}

		/// <summary>
		/// Notifies the <see cref="T:System.Threading.Barrier" /> that there will be one less participant.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">The barrier already has 0
		/// participants.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		/// The method was invoked from within a post-phase action.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has already been
		/// disposed.</exception>
		public void RemoveParticipant() {
			this.RemoveParticipants(1);
		}

		/// <summary>
		/// Notifies the <see cref="T:System.Threading.Barrier" /> that there will be fewer participants.
		/// </summary>
		/// <param name="participantCount">The number of additional participants to remove from the barrier.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="participantCount" /> is less than
		/// 0.</exception>
		/// <exception cref="T:System.InvalidOperationException">The barrier already has 0 participants.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		/// The method was invoked from within a post-phase action.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has already been
		/// disposed.</exception>
		public void RemoveParticipants(int participantCount) {
			this.ThrowIfDisposed();
			if (participantCount < 1)
				throw new ArgumentOutOfRangeException("participantCount", (object) participantCount, "Barrier_RemoveParticipants_NonPositive_ArgumentOutOfRange");
			if (this.m_actionCallerID != 0 && Thread.CurrentThread.ManagedThreadId == this.m_actionCallerID)
				throw new InvalidOperationException("Barrier_InvalidOperation_CalledFromPHA");
			SpinWait spinWait = new SpinWait();
			bool sense;
			while (true) {
				int currentTotalCount = this.m_currentTotalCount;
				int current;
				int total;
				this.GetCurrentTotal(currentTotalCount, out current, out total, out sense);
				if (total >= participantCount) {
					if (total - participantCount >= current) {
						int num = total - participantCount;
						if (num > 0 && current == num) {
							if (this.SetCurrentTotal(currentTotalCount, 0, total - participantCount, !sense))
								goto label_11;
						} else if (this.SetCurrentTotal(currentTotalCount, current, total - participantCount, sense))
							goto label_14;
						spinWait.SpinOnce();
					} else
						goto label_8;
				} else
					break;
			}
			throw new ArgumentOutOfRangeException("participantCount", "Barrier_RemoveParticipants_ArgumentOutOfRange");
			label_8:
			throw new InvalidOperationException("Barrier_RemoveParticipants_InvalidOperation");
			label_11:
			this.FinishPhase(sense);
			return;
			label_14:
			;
		}

		/// <summary>
		/// Signals that a participant has reached the <see cref="T:System.Threading.Barrier" /> and waits for all other
		/// participants to reach the barrier as well.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">
		/// The method was invoked from within a post-phase action, the barrier currently has 0 participants,
		/// or the barrier is being used by more threads than are registered as participants.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has already been
		/// disposed.</exception>
		public void SignalAndWait() {
			this.SignalAndWait(new CancellationToken());
		}

		/// <summary>
		/// Signals that a participant has reached the <see cref="T:System.Threading.Barrier" /> and waits for all other
		/// participants to reach the barrier, while observing a <see cref="T:System.Threading.CancellationToken" />.
		/// </summary>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to
		/// observe.</param>
		/// <exception cref="T:System.InvalidOperationException">
		/// The method was invoked from within a post-phase action, the barrier currently has 0 participants,
		/// or the barrier is being used by more threads than are registered as participants.
		/// </exception>
		/// <exception cref="T:System.OperationCanceledException"><paramref name="cancellationToken" /> has been
		/// canceled.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has already been
		/// disposed.</exception>
		public void SignalAndWait(CancellationToken cancellationToken) {
			this.SignalAndWait(-1, cancellationToken);
		}

		/// <summary>
		/// Signals that a participant has reached the <see cref="T:System.Threading.Barrier" /> and waits for all other
		/// participants to reach the barrier as well, using a
		/// <see cref="T:System.TimeSpan" /> to measure the time interval.
		/// </summary>
		/// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of
		/// milliseconds to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to
		/// wait indefinitely.</param>
		/// <returns>true if all other participants reached the barrier; otherwise, false.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="timeout" />is a negative number
		/// other than -1 milliseconds, which represents an infinite time-out, or it is greater than
		/// <see cref="T:System.Int32.MaxValue" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		/// The method was invoked from within a post-phase action, the barrier currently has 0 participants,
		/// or the barrier is being used by more threads than are registered as participants.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has already been
		/// disposed.</exception>
		public bool SignalAndWait(TimeSpan timeout) {
			return this.SignalAndWait(timeout, new CancellationToken());
		}

		/// <summary>
		/// Signals that a participant has reached the <see cref="T:System.Threading.Barrier" /> and waits for all other
		/// participants to reach the barrier as well, using a
		/// <see cref="T:System.TimeSpan" /> to measure the time interval, while observing a <see cref="T:System.Threading.CancellationToken" />.
		/// </summary>
		/// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of
		/// milliseconds to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to
		/// wait indefinitely.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to
		/// observe.</param>
		/// <returns>true if all other participants reached the barrier; otherwise, false.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="timeout" />is a negative number
		/// other than -1 milliseconds, which represents an infinite time-out.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		/// The method was invoked from within a post-phase action, the barrier currently has 0 participants,
		/// or the barrier is being used by more threads than are registered as participants.
		/// </exception>
		/// <exception cref="T:System.OperationCanceledException"><paramref name="cancellationToken" /> has been
		/// canceled.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has already been
		/// disposed.</exception>
		public bool SignalAndWait(TimeSpan timeout, CancellationToken cancellationToken) {
			long totalMilliseconds = (long) timeout.TotalMilliseconds;
			if (totalMilliseconds < -1L || totalMilliseconds > (long) int.MaxValue)
				throw new ArgumentOutOfRangeException("timeout", (object) timeout, "Barrier_SignalAndWait_ArgumentOutOfRange");
			return this.SignalAndWait((int) timeout.TotalMilliseconds, cancellationToken);
		}

		/// <summary>
		/// Signals that a participant has reached the <see cref="T:System.Threading.Barrier" /> and waits for all other
		/// participants to reach the barrier as well, using a
		/// 32-bit signed integer to measure the time interval.
		/// </summary>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" />(-1) to wait indefinitely.</param>
		/// <returns>true if all other participants reached the barrier; otherwise, false.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="millisecondsTimeout" /> is a
		/// negative number other than -1, which represents an infinite time-out.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		/// The method was invoked from within a post-phase action, the barrier currently has 0 participants,
		/// or the barrier is being used by more threads than are registered as participants.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has already been
		/// disposed.</exception>
		public bool SignalAndWait(int millisecondsTimeout) {
			return this.SignalAndWait(millisecondsTimeout, new CancellationToken());
		}

		/// <summary>
		/// Signals that a participant has reached the barrier and waits for all other participants to reach
		/// the barrier as well, using a
		/// 32-bit signed integer to measure the time interval, while observing a <see cref="T:System.Threading.CancellationToken" />.
		/// </summary>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" />(-1) to wait indefinitely.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to
		/// observe.</param>
		/// <returns>true if all other participants reached the barrier; otherwise, false.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="millisecondsTimeout" /> is a
		/// negative number other than -1, which represents an infinite time-out.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		/// The method was invoked from within a post-phase action, the barrier currently has 0 participants,
		/// or the barrier is being used by more threads than are registered as participants.
		/// </exception>
		/// <exception cref="T:System.OperationCanceledException"><paramref name="cancellationToken" /> has been
		/// canceled.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has already been
		/// disposed.</exception>
		public bool SignalAndWait(int millisecondsTimeout, CancellationToken cancellationToken) {
			this.ThrowIfDisposed();
			cancellationToken.ThrowIfCancellationRequested();
			if (millisecondsTimeout < -1)
				throw new ArgumentOutOfRangeException("millisecondsTimeout", (object) millisecondsTimeout, "Barrier_SignalAndWait_ArgumentOutOfRange");
			if (this.m_actionCallerID != 0 && Thread.CurrentThread.ManagedThreadId == this.m_actionCallerID)
				throw new InvalidOperationException("Barrier_InvalidOperation_CalledFromPHA");
			SpinWait spinWait = new SpinWait();
			int current;
			int total;
			bool sense1;
			while (true) {
				int currentTotalCount = this.m_currentTotalCount;
				this.GetCurrentTotal(currentTotalCount, out current, out total, out sense1);
				if (total != 0) {
					if (current != 0 || sense1 == (this.m_currentPhase % 2L == 0L)) {
						if (current + 1 == total) {
							if (this.SetCurrentTotal(currentTotalCount, 0, total, !sense1))
								goto label_11;
						} else if (this.SetCurrentTotal(currentTotalCount, current + 1, total, sense1))
							goto label_14;
						spinWait.SpinOnce();
					} else
						goto label_8;
				} else
					break;
			}
			throw new InvalidOperationException("Barrier_SignalAndWait_InvalidOperation_ZeroTotal");
			label_8:
			throw new InvalidOperationException("Barrier_SignalAndWait_InvalidOperation_ThreadsExceeded");
			label_11:
			this.FinishPhase(sense1);
			return true;
			label_14:
			long currentPhase = this.m_currentPhase;
			ManualResetEventSlim manualResetEventSlim = sense1 ? this.m_evenEvent : this.m_oddEvent;
			bool flag1 = false;
			bool flag2 = false;
			try {
				flag2 = manualResetEventSlim.Wait(millisecondsTimeout, cancellationToken);
			} catch (OperationCanceledException2) {
				flag1 = true;
			}
			if (!flag2) {
				spinWait.Reset();
				while (true) {
					int currentTotalCount = this.m_currentTotalCount;
					bool sense2;
					this.GetCurrentTotal(currentTotalCount, out current, out total, out sense2);
					if (currentPhase == this.m_currentPhase && sense1 == sense2) {
						if (!this.SetCurrentTotal(currentTotalCount, current - 1, total, sense1))
							spinWait.SpinOnce();
						else
							goto label_22;
					} else
						break;
				}
				manualResetEventSlim.Wait();
				goto label_26;
				label_22:
				if (flag1)
					throw new OperationCanceledException2("Common_OperationCanceled", cancellationToken);
				return false;
			}
			label_26:
			if (this.m_exception != null)
				throw new BarrierPostPhaseException(this.m_exception);
			return true;
		}

		/// <summary>
		/// Finish the phase by invoking the post phase action, and setting the event, this must be called by the
		/// last arrival thread
		/// </summary>
		/// <param name="observedSense">The current phase sense</param>
		private void FinishPhase(bool observedSense) {
			if (this.m_postPhaseAction != null) {
				try {
					this.m_actionCallerID = Thread.CurrentThread.ManagedThreadId;
					if (this.m_ownerThreadContext != null)
						ExecutionContext.Run(this.m_ownerThreadContext.CreateCopy(), (ContextCallback) (i => this.m_postPhaseAction(this)), (object) null);
					else
						this.m_postPhaseAction(this);
					this.m_exception = (Exception) null;
				} catch (Exception ex) {
					this.m_exception = ex;
				} finally {
					this.m_actionCallerID = 0;
					this.SetResetEvents(observedSense);
					if (this.m_exception != null)
						throw new BarrierPostPhaseException(this.m_exception);
				}
			} else
				this.SetResetEvents(observedSense);
		}

		/// <summary>
		/// Sets the current phase event and reset the next phase event
		/// </summary>
		/// <param name="observedSense">The current phase sense</param>
		private void SetResetEvents(bool observedSense) {
			++this.m_currentPhase;
			if (observedSense) {
				this.m_oddEvent.Reset();
				this.m_evenEvent.Set();
			} else {
				this.m_evenEvent.Reset();
				this.m_oddEvent.Set();
			}
		}

		/// <summary>
		/// Releases all resources used by the current instance of <see cref="T:System.Threading.Barrier" />.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">
		/// The method was invoked from within a post-phase action.
		/// </exception>
		/// <remarks>
		/// Unlike most of the members of <see cref="T:System.Threading.Barrier" />, Dispose is not thread-safe and may not be
		/// used concurrently with other members of this instance.
		/// </remarks>
		public void Dispose() {
			if (this.m_actionCallerID != 0 && Thread.CurrentThread.ManagedThreadId == this.m_actionCallerID)
				throw new InvalidOperationException("Barrier_InvalidOperation_CalledFromPHA");
			this.Dispose(true);
			GC.SuppressFinalize((object) this);
		}

		/// <summary>
		/// When overridden in a derived class, releases the unmanaged resources used by the
		/// <see cref="T:System.Threading.Barrier" />, and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release
		/// only unmanaged resources.</param>
		/// <remarks>
		/// Unlike most of the members of <see cref="T:System.Threading.Barrier" />, Dispose is not thread-safe and may not be
		/// used concurrently with other members of this instance.
		/// </remarks>
		protected virtual void Dispose(bool disposing) {
			if (this.m_disposed)
				return;
			if (disposing) {
				this.m_oddEvent.Dispose();
				this.m_evenEvent.Dispose();
			}
			this.m_disposed = true;
		}

		/// <summary>
		/// Throw ObjectDisposedException if the barrier is disposed
		/// </summary>
		private void ThrowIfDisposed() {
			if (this.m_disposed)
				throw new ObjectDisposedException("Barrier", "Barrier_Dispose");
		}
	}
}