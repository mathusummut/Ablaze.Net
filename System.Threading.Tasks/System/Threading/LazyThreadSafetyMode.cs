namespace System.Threading {
	/// <summary>
	/// Specifies how a <see cref="T:System.Threading.Lazy{T}" /> instance should synchronize access among multiple threads.
	/// </summary>
	public enum LazyThreadSafetyMode {
		None,
		PublicationOnly,
		ExecutionAndPublication,
	}
}