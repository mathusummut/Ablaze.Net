namespace System.Threading {
	/// <summary>
	/// A seprate non generic class that contains a global counter for fast path instances for all Ts that has been created, and adds an upper limit for all instances
	/// that uses the fast path, if this limit has been reached, all new instances will use the slow path
	/// </summary>
	internal static class ThreadLocalGlobalCounter {
		internal static int MAXIMUM_GLOBAL_COUNT = ThreadLocal<int>.MAXIMUM_TYPES_LENGTH * 4;
		internal static volatile int s_fastPathCount;
	}
}