using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Threading {
	/// <summary>
	/// Initializes a lock with timeout capabilities. Example usage: using (new TimedLock(root, 7000)) { }
	/// </summary>
	public struct TimedLock : IDisposable {
		/// <summary>
		/// Whether the lock was taken.
		/// </summary>
		public readonly bool LockWasTaken;
		/// <summary>
		/// The object that is used to lock onto.
		/// </summary>
		public readonly object SyncRoot;

		/// <summary>
		/// Initializes a lock with timeout capabilities.
		/// </summary>
		/// <param name="syncRoot">The object that is used to lock onto.</param>
		/// <param name="milliseconds">The lock timeout in milliseconds.</param>
		/// <param name="exceptions">Specifies how exceptions are handled.</param>
		public TimedLock(object syncRoot, int milliseconds = 10000, ExceptionMode exceptions = ExceptionMode.Log) {
			SyncRoot = syncRoot;
#if NET35
			bool lockWasTaken = Monitor.TryEnter(syncRoot, milliseconds);
#else
			bool lockWasTaken = false;
			Monitor.TryEnter(syncRoot, milliseconds, ref lockWasTaken);
#endif
			LockWasTaken = lockWasTaken;
			if (!lockWasTaken) {
				if (exceptions == ExceptionMode.Log)
					ErrorHandler.LogException(new TimeoutException("The lock could not be obtained within the specified " + milliseconds + " timeout."));
				else if (exceptions == ExceptionMode.Throw)
					throw new TimeoutException("The lock could not be obtained within the specified " + milliseconds + " timeout.");
			}
		}

		/// <summary>
		/// Releases of the lock if it was taken.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Dispose() {
			if (LockWasTaken)
				Monitor.Exit(SyncRoot);
		}
	}
}