using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System.Threading.Dispatch {
	/// <summary>
	/// Holds the data required for a synchronous or asynchronous invocation.
	/// </summary>
	public sealed class InvocationData : IAsyncResult, IComparer, IComparer<InvocationData>, IComparable<InvocationData>, IComparable, IDisposable {
		/// <summary>
		/// Gets the method to invoke.
		/// </summary>
		public readonly Func<object, object> Method;
		/// <summary>
		/// Gets the parameter to pass to the method.
		/// </summary>
		public object Parameter;
		private ManualResetEventSlim resetEvent = new ManualResetEventSlim();
		private object asyncState, readonlySyncRoot = new object();
		private sbyte priority;
		private InvokeState state;
		private bool completedSynchronously;
		/// <summary>
		/// Available for use.
		/// </summary>
		public object Tag;

		/// <summary>
		/// Gets the reset event to use as a wait handle.
		/// </summary>
		public ManualResetEventSlim ResetEvent {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				if (resetEvent == null) {
					lock (readonlySyncRoot) {
						if (resetEvent == null)
							resetEvent = new ManualResetEventSlim();
					}
				}
				return resetEvent;
			}
		}

		/// <summary>
		/// Gets the return value of the method. If the invocation has not yet completed, gets null.
		/// </summary>
		public object AsyncState {
			get {
				IAsyncResult state = asyncState as IAsyncResult;
				if (state == null)
					return asyncState;
				else
					return state.AsyncState;
			}
			set {
				asyncState = value;
			}
		}

		/// <summary>
		/// Gets whether the operation completed synchronously.
		/// </summary>
		public bool CompletedSynchronously {
			get {
				IAsyncResult state = asyncState as IAsyncResult;
				if (state == null)
					return completedSynchronously;
				else
					return state.CompletedSynchronously;
			}
			set {
				completedSynchronously = value;
			}
		}

		/// <summary>
		/// Gets the current state of the invocation.
		/// </summary>
		public InvokeState State {
			get {
				IAsyncResult st = asyncState as IAsyncResult;
				if (st == null)
					return state;
				else
					return st.IsCompleted ? InvokeState.Completed : InvokeState.Started;
			}
			set {
				state = value;
			}
		}

		/// <summary>
		/// Gets the wait handle of the invocation. Use System.Threading.Dispatch.Dispatcher.EndInvoke() instead.
		/// </summary>
		public WaitHandle AsyncWaitHandle {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return resetEvent.WaitHandle;
			}
		}

		/// <summary>
		/// Gets whether this instance has been queued for invocation.
		/// </summary>
		public bool IsQueued {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return State == InvokeState.Queued;
			}
		}

		/// <summary>
		/// Gets whether the invocation has started.
		/// </summary>
		public bool IsStarted {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return State == InvokeState.Started;
			}
		}

		/// <summary>
		/// Gets whether the operation is completed.
		/// </summary>
		public bool IsCompleted {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return State == InvokeState.Completed || State == InvokeState.Error;
			}
		}

		/// <summary>
		/// Gets or sets the priority of the invocation.
		/// </summary>
		[CLSCompliant(false)]
		public sbyte Priority {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return priority;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				if (value == priority)
					return;
				priority = value;
				//dispatcher.ChangePriority(this);
			}
		}

		/// <summary>
		/// Initializes an invocation.
		/// </summary>
		/// <param name="method">The method to invoke.</param>
		/// <param name="parameter">The parameter to use it with.</param>
		public InvocationData(Func<object, object> method, object parameter = null) {
			if (method == null)
				throw new ArgumentNullException(nameof(method), "The invocation method can't be null.");
			Method = method;
			Parameter = parameter;
		}

		/// <summary>
		/// Performs the specified invocation on the current thread.
		/// </summary>
		public object InvokeOnCurrentThread() {
			State = InvokeState.Started;
			AsyncState = Method(Parameter);
			State = InvokeState.Completed;
			CompletedSynchronously = true;
			return AsyncState;
		}

		/// <summary>
		/// Compares the priority of instance to the priority of the one specified.
		/// </summary>
		/// <param name="obj">The instance of InvokeEventArgs to compare the priority to.</param>
		/// <returns>The comparison between the priority of this instance to the priority of the one specified.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public int CompareTo(object obj) {
			InvocationData args = obj as InvocationData;
			return args == null ? 0 : CompareTo(args);
		}

		/// <summary>
		/// Compares the priority of the instance to the priority of the one specified.
		/// </summary>
		/// <param name="e">The instance of InvokeEventArgs to compare the priority to.</param>
		/// <returns>The comparison between the priority of this instance to the priority of the one specified.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public int CompareTo(InvocationData e) {
			return e == null ? 0 : Priority.CompareTo(e.Priority);
		}

		/// <summary>
		/// Compares the priority of the instance of InvokeEventArgs to the priority of the one specified.
		/// </summary>
		/// <param name="instance">The instance of InvokeEventArgs.</param>
		/// <param name="toCompareWith">The instance of InvokeEventArgs to compare the priority to.</param>
		/// <returns>The comparison between the priority of the instance to the priority of the one specified.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public int Compare(InvocationData instance, InvocationData toCompareWith) {
			return instance == null ? 0 : instance.CompareTo(toCompareWith);
		}

		/// <summary>
		/// Compares the priority of the instance of InvokeEventArgs to the priority of the one specified.
		/// </summary>
		/// <param name="instance">The instance of InvokeEventArgs.</param>
		/// <param name="toCompareWith">The instance of InvokeEventArgs to compare the priority to.</param>
		/// <returns>The comparison between the priority of the instance to the priority of the one specified.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public int Compare(object instance, object toCompareWith) {
			return CompareStatic(instance as InvocationData, toCompareWith as InvocationData);
		}

		/// <summary>
		/// Compares the priority of the instance of InvokeEventArgs to the priority of the one specified.
		/// </summary>
		/// <param name="instance">The instance of InvokeEventArgs.</param>
		/// <param name="toCompareWith">The instance of InvokeEventArgs to compare the priority to.</param>
		/// <returns>The comparison between the priority of the instance to the priority of the one specified.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static int CompareStatic(InvocationData instance, InvocationData toCompareWith) {
			return instance == null ? 0 : instance.CompareTo(toCompareWith);
		}

		/// <summary>
		/// Compares the priority of the instance of InvokeEventArgs to the priority of the one specified.
		/// </summary>
		/// <param name="instance">The instance of InvokeEventArgs.</param>
		/// <param name="toCompareWith">The instance of InvokeEventArgs to compare the priority to.</param>
		/// <returns>The comparison between the priority of the instance to the priority of the one specified.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static int CompareStatic(object instance, object toCompareWith) {
			return CompareStatic(instance as InvocationData, toCompareWith as InvocationData);
		}

		/// <summary>
		/// Disposes of this instance.
		/// </summary>
		~InvocationData() {
			Dispose();
		}

		/// <summary>
		/// Disposes of this instance.
		/// </summary>
		public void Dispose() {
			if (resetEvent == null)
				return;
			lock (readonlySyncRoot) {
				if (resetEvent != null) {
					resetEvent.Dispose();
					resetEvent = null;
				}
			}
			GC.SuppressFinalize(this);
		}
	}
}