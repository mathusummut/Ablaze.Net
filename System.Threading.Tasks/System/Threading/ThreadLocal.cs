using System.Collections.Concurrent;
using System.Diagnostics;
using System.Security;
using System.Security.Permissions;

namespace System.Threading {
	/// <summary>Provides thread-local storage of data.</summary>
	/// <typeparam name="T">Specifies the type of data stored per-thread.</typeparam>
	/// <remarks>
	/// <para>
	/// With the exception of <see cref="M:System.Threading.ThreadLocal`1.Dispose" />, all public and protected members of
	/// <see cref="T:System.Threading.ThreadLocal`1" /> are thread-safe and may be used
	/// concurrently from multiple threads.
	/// </para>
	/// </remarks>
	[DebuggerDisplay("IsValueCreated={IsValueCreated}, Value={ValueForDebugDisplay}")]
	[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true, Synchronization = true)]
	public class ThreadLocal<T> : IDisposable {
		private static Type[] s_dummyTypes = new Type[16] { typeof(ThreadLocal<T>.C0), typeof(ThreadLocal<T>.C1), typeof(ThreadLocal<T>.C2), typeof(ThreadLocal<T>.C3), typeof(ThreadLocal<T>.C4), typeof(ThreadLocal<T>.C5), typeof(ThreadLocal<T>.C6), typeof(ThreadLocal<T>.C7), typeof(ThreadLocal<T>.C8), typeof(ThreadLocal<T>.C9), typeof(ThreadLocal<T>.C10), typeof(ThreadLocal<T>.C11), typeof(ThreadLocal<T>.C12), typeof(ThreadLocal<T>.C13), typeof(ThreadLocal<T>.C14), typeof(ThreadLocal<T>.C15) };
		private static int s_currentTypeId = -1;
		private static ConcurrentStack<int> s_availableIndices = new ConcurrentStack<int>();
		private static int TYPE_DIMENSIONS = typeof(ThreadLocal<>.GenericHolder<,,>).GetGenericArguments().Length;
		internal static int MAXIMUM_TYPES_LENGTH = (int) Math.Pow((double) ThreadLocal<T>.s_dummyTypes.Length, (double) (ThreadLocal<T>.TYPE_DIMENSIONS - 1));
		private ThreadLocal<T>.HolderBase m_holder;
		private Func<T> m_valueFactory;
		private int m_currentInstanceIndex;

		/// <summary>
		/// Gets or sets the value of this instance for the current thread.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">
		/// The initialization function referenced <see cref="P:System.Threading.ThreadLocal`1.Value" /> in an improper manner.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.ThreadLocal`1" /> instance has been disposed.
		/// </exception>
		/// <remarks>
		/// If this instance was not previously initialized for the current thread,
		/// accessing <see cref="P:System.Threading.ThreadLocal`1.Value" /> will attempt to initialize it. If an initialization function was
		/// supplied during the construction, that initialization will happen by invoking the function
		/// to retrieve the initial value for <see cref="P:System.Threading.ThreadLocal`1.Value" />.  Otherwise, the default value of
		/// <typeparamref name="T" /> will be used.
		/// </remarks>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public T Value {
			get {
				if (this.m_holder == null)
					throw new ObjectDisposedException("ThreadLocal_Disposed");
				ThreadLocal<T>.Boxed boxed = this.m_holder.Boxed;
				if (boxed == null || boxed.m_ownerHolder != this.m_holder)
					boxed = this.CreateValue();
				return boxed.Value;
			}
			set {
				if (this.m_holder == null)
					throw new ObjectDisposedException("ThreadLocal_Disposed");
				ThreadLocal<T>.Boxed boxed = this.m_holder.Boxed;
				if (boxed != null && boxed.m_ownerHolder == this.m_holder)
					boxed.Value = value;
				else
					this.m_holder.Boxed = new ThreadLocal<T>.Boxed() {
						Value = value,
						m_ownerHolder = this.m_holder
					};
			}
		}

		/// <summary>
		/// Gets whether <see cref="P:System.Threading.ThreadLocal`1.Value" /> is initialized on the current thread.
		/// </summary>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.ThreadLocal`1" /> instance has been disposed.
		/// </exception>
		public bool IsValueCreated {
			get {
				if (this.m_holder == null)
					throw new ObjectDisposedException("ThreadLocal_Disposed");
				ThreadLocal<T>.Boxed boxed = this.m_holder.Boxed;
				if (boxed != null)
					return boxed.m_ownerHolder == this.m_holder;
				return false;
			}
		}

		/// <summary>Gets the value of the ThreadLocal&lt;T&gt; for debugging display purposes. It takes care of getting
		/// the value for the current thread in the ThreadLocal mode.</summary>
		internal T ValueForDebugDisplay {
			get {
				if (this.m_holder == null || this.m_holder.Boxed == null || this.m_holder.Boxed.m_ownerHolder != this.m_holder)
					return default(T);
				return this.m_holder.Boxed.Value;
			}
		}

		/// <summary>
		/// Initializes the <see cref="T:System.Threading.ThreadLocal`1" /> instance.
		/// </summary>
		[SecuritySafeCritical]
		public ThreadLocal() {
			if (this.FindNextTypeIndex()) {
				Type[] typesFromIndex = this.GetTypesFromIndex();
				new PermissionSet(PermissionState.Unrestricted).Assert();
				try {
					this.m_holder = (ThreadLocal<T>.HolderBase) Activator.CreateInstance(typeof(ThreadLocal<>.GenericHolder<,,>).MakeGenericType(typesFromIndex));
				} finally {
					PermissionSet.RevertAssert();
				}
			} else
				this.m_holder = (ThreadLocal<T>.HolderBase) new ThreadLocal<T>.TLSHolder();
		}

		/// <summary>
		/// Initializes the <see cref="T:System.Threading.ThreadLocal`1" /> instance with the
		/// specified <paramref name="valueFactory" /> function.
		/// </summary>
		/// <param name="valueFactory">
		/// The <see cref="T:System.Func{T}" /> invoked to produce a lazily-initialized value when
		/// an attempt is made to retrieve <see cref="P:System.Threading.ThreadLocal`1.Value" /> without it having been previously initialized.
		/// </param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="valueFactory" /> is a null reference (Nothing in Visual Basic).
		/// </exception>
		public ThreadLocal(Func<T> valueFactory)
		  : this() {
			if (valueFactory == null)
				throw new ArgumentNullException("valueFactory");
			this.m_valueFactory = valueFactory;
		}

		/// <summary>
		/// Releases the resources used by this <see cref="T:System.Threading.ThreadLocal{T}" /> instance.
		/// </summary>
		~ThreadLocal() {
			this.Dispose(false);
		}

		/// <summary>
		/// Releases the resources used by this <see cref="T:System.Threading.ThreadLocal{T}" /> instance.
		/// </summary>
		/// <remarks>
		/// Unlike most of the members of <see cref="T:System.Threading.ThreadLocal{T}" />, this method is not thread-safe.
		/// </remarks>
		public void Dispose() {
			this.Dispose(true);
			GC.SuppressFinalize((object) this);
		}

		/// <summary>
		/// Releases the resources used by this <see cref="T:System.Threading.ThreadLocal{T}" /> instance.
		/// </summary>
		/// <param name="disposing">
		/// A Boolean value that indicates whether this method is being called due to a call to <see cref="M:System.Threading.ThreadLocal`1.Dispose" />.
		/// </param>
		/// <remarks>
		/// Unlike most of the members of <see cref="T:System.Threading.ThreadLocal{T}" />, this method is not thread-safe.
		/// </remarks>
		protected virtual void Dispose(bool disposing) {
			int currentInstanceIndex = this.m_currentInstanceIndex;
			if (currentInstanceIndex > -1 && Interlocked.CompareExchange(ref this.m_currentInstanceIndex, -1, currentInstanceIndex) == currentInstanceIndex)
				ThreadLocal<T>.s_availableIndices.Push(currentInstanceIndex);
			this.m_holder = (ThreadLocal<T>.HolderBase) null;
		}

		/// <summary>
		/// Tries to get a unique index for the current instance of type T, it first tries to get it from the pool if it is not empty, otherwise it
		/// increments the global counter if it is still below the maximum, otherwise it fails and returns -1
		/// </summary>
		/// <returns>True if there is an index available, false otherwise</returns>
		private bool FindNextTypeIndex() {
			int result = -1;
			if (ThreadLocal<T>.s_availableIndices.TryPop(out result)) {
				this.m_currentInstanceIndex = result;
				return true;
			}
			if (ThreadLocal<T>.s_currentTypeId < ThreadLocal<T>.MAXIMUM_TYPES_LENGTH - 1 && ThreadLocalGlobalCounter.s_fastPathCount < ThreadLocalGlobalCounter.MAXIMUM_GLOBAL_COUNT && Interlocked.Increment(ref ThreadLocalGlobalCounter.s_fastPathCount) <= ThreadLocalGlobalCounter.MAXIMUM_GLOBAL_COUNT) {
				int num = Interlocked.Increment(ref ThreadLocal<T>.s_currentTypeId);
				if (num < ThreadLocal<T>.MAXIMUM_TYPES_LENGTH) {
					this.m_currentInstanceIndex = num;
					return true;
				}
			}
			this.m_currentInstanceIndex = -1;
			return false;
		}

		/// <summary>
		/// Gets an array of types that will be used as generic parameters for the GenericHolder class
		/// </summary>
		/// <returns>The types array</returns>
		private Type[] GetTypesFromIndex() {
			Type[] typeArray = new Type[ThreadLocal<T>.TYPE_DIMENSIONS];
			typeArray[0] = typeof(T);
			int currentInstanceIndex = this.m_currentInstanceIndex;
			for (int index = 1; index < ThreadLocal<T>.TYPE_DIMENSIONS; ++index) {
				typeArray[index] = ThreadLocal<T>.s_dummyTypes[currentInstanceIndex % ThreadLocal<T>.s_dummyTypes.Length];
				currentInstanceIndex /= ThreadLocal<T>.s_dummyTypes.Length;
			}
			return typeArray;
		}

		/// <summary>Creates and returns a string representation of this instance for the current thread.</summary>
		/// <returns>The result of calling <see cref="M:System.Object.ToString" /> on the <see cref="P:System.Threading.ThreadLocal`1.Value" />.</returns>
		/// <exception cref="T:System.NullReferenceException">
		/// The <see cref="P:System.Threading.ThreadLocal`1.Value" /> for the current thread is a null reference (Nothing in Visual Basic).
		/// </exception>
		/// <exception cref="T:System.InvalidOperationException">
		/// The initialization function referenced <see cref="P:System.Threading.ThreadLocal`1.Value" /> in an improper manner.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.ThreadLocal`1" /> instance has been disposed.
		/// </exception>
		/// <remarks>
		/// Calling this method forces initialization for the current thread, as is the
		/// case with accessing <see cref="P:System.Threading.ThreadLocal`1.Value" /> directly.
		/// </remarks>
		public override string ToString() {
			return this.Value.ToString();
		}

		/// <summary>
		/// Private helper function to lazily create the value using the calueSelector if specified in the constructor or the default parameterless constructor
		/// </summary>
		/// <returns>Returns the boxed object</returns>
		private ThreadLocal<T>.Boxed CreateValue() {
			ThreadLocal<T>.Boxed boxed = new ThreadLocal<T>.Boxed();
			boxed.m_ownerHolder = this.m_holder;
			boxed.Value = this.m_valueFactory == null ? default(T) : this.m_valueFactory();
			if (this.m_holder.Boxed != null && this.m_holder.Boxed.m_ownerHolder == this.m_holder)
				throw new InvalidOperationException("ThreadLocal_Value_RecursiveCallsToValue");
			this.m_holder.Boxed = boxed;
			return boxed;
		}

		private sealed class C0 {
		}

		private sealed class C1 {
		}

		private sealed class C2 {
		}

		private sealed class C3 {
		}

		private sealed class C4 {
		}

		private sealed class C5 {
		}

		private sealed class C6 {
		}

		private sealed class C7 {
		}

		private sealed class C8 {
		}

		private sealed class C9 {
		}

		private sealed class C10 {
		}

		private sealed class C11 {
		}

		private sealed class C12 {
		}

		private sealed class C13 {
		}

		private sealed class C14 {
		}

		private sealed class C15 {
		}

		/// <summary>The base abstract class for the holder</summary>
		private abstract class HolderBase {
			internal abstract ThreadLocal<T>.Boxed Boxed {
				get; set;
			}
		}

		/// <summary>The TLS holder representation</summary>
		private sealed class TLSHolder : ThreadLocal<T>.HolderBase {
			private LocalDataStoreSlot m_slot = Thread.AllocateDataSlot();

			internal override ThreadLocal<T>.Boxed Boxed {
				get {
					return (ThreadLocal<T>.Boxed) Thread.GetData(this.m_slot);
				}
				set {
					Thread.SetData(this.m_slot, (object) value);
				}
			}
		}

		/// <summary>The generic holder representation</summary>
		/// <typeparam name="U">Dummy param</typeparam>
		/// <typeparam name="V">Dummy param</typeparam>
		/// <typeparam name="W">Dummy param</typeparam>
		private sealed class GenericHolder<U, V, W> : ThreadLocal<T>.HolderBase {
			[ThreadStatic]
			private static ThreadLocal<T>.Boxed s_value;

			internal override ThreadLocal<T>.Boxed Boxed {
				get {
					return ThreadLocal<T>.GenericHolder<U, V, W>.s_value;
				}
				set {
					ThreadLocal<T>.GenericHolder<U, V, W>.s_value = value;
				}
			}
		}

		/// <summary>wrapper to the actual value</summary>
		private sealed class Boxed {
			internal T Value;
			internal ThreadLocal<T>.HolderBase m_ownerHolder;
		}
	}
}