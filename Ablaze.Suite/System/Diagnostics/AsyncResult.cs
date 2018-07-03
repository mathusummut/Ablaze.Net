using System.Runtime.CompilerServices;
using System.Threading;

namespace System.Diagnostics {
	/// <summary>
	/// A configurable IAsyncResult implementation.
	/// </summary>
	public sealed class AsyncResult : IAsyncResult {
		/// <summary>
		/// The AsyncResult instance to use instead of this one. This is useful for chaining asynchronous method calls.
		/// </summary>
		public IAsyncResult Handler;
		private bool isCompleted;

		/// <summary>
		/// Gets or setsa flag whether the task completed synchronously or asynchronously;
		/// </summary>
		public bool CompletedSynchronously {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the WaitHandle of the instance.
		/// </summary>
		public WaitHandle AsyncWaitHandle {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the state of the task.
		/// </summary>
		public object AsyncState {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets whether *this* task is completed (if Handler is not null, its IsCompleted value is returned for Get).
		/// </summary>
		public bool IsCompleted {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return Handler == null || Handler == this ? isCompleted : Handler.IsCompleted;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				isCompleted = value;
			}
		}

		/// <summary>
		/// Initializes a new IAsyncResult implementation.
		/// </summary>
		public AsyncResult() {
		}

		/// <summary>
		/// Initializes a new IAsyncResult implementation.
		/// </summary>
		/// <param name="isCompleted">Whether the task is already completed.</param>
		public AsyncResult(bool isCompleted) {
			this.isCompleted = isCompleted;
		}
	}
}