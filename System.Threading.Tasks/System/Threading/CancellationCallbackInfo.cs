namespace System.Threading {
	/// <summary>
	/// A helper class for collating the various bits of information required to execute
	/// cancellation callbacks.
	/// </summary>
	internal class CancellationCallbackInfo {
		internal readonly Action<object> Callback;
		internal readonly object StateForCallback;
		internal readonly SynchronizationContext TargetSyncContext;
		internal readonly ExecutionContext TargetExecutionContext;
		internal readonly CancellationTokenSource CancellationTokenSource;

		internal CancellationCallbackInfo(Action<object> callback, object stateForCallback, SynchronizationContext targetSyncContext, ExecutionContext targetExecutionContext, CancellationTokenSource cancellationTokenSource) {
			this.Callback = callback;
			this.StateForCallback = stateForCallback;
			this.TargetSyncContext = targetSyncContext;
			this.TargetExecutionContext = targetExecutionContext;
			this.CancellationTokenSource = cancellationTokenSource;
		}

		/// <summary>
		/// InternalExecuteCallbackSynchronously_GeneralPath
		/// This will be called on the target synchronization context, however, we still need to restore the required execution context
		/// </summary>
		internal void ExecuteCallback() {
			if (this.TargetExecutionContext != null)
				ExecutionContext.Run(this.TargetExecutionContext, new ContextCallback(CancellationCallbackInfo.ExecutionContextCallback), (object) this);
			else
				CancellationCallbackInfo.ExecutionContextCallback((object) this);
		}

		private static void ExecutionContextCallback(object obj) {
			CancellationCallbackInfo cancellationCallbackInfo = obj as CancellationCallbackInfo;
			cancellationCallbackInfo.Callback(cancellationCallbackInfo.StateForCallback);
		}
	}
}