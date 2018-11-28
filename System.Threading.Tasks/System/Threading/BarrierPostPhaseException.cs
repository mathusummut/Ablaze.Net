using System.Runtime.Serialization;
using System.Security;

namespace System.Threading {
	/// <summary>
	/// The exception that is thrown when the post-phase action of a <see cref="T:System.Threading.Barrier" /> fails.
	/// </summary>
	[Serializable]
	public class BarrierPostPhaseException : Exception {
		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Threading.BarrierPostPhaseException" /> class.
		/// </summary>
		public BarrierPostPhaseException()
		  : this((string) null) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Threading.BarrierPostPhaseException" /> class with the specified inner exception.
		/// </summary>
		/// <param name="innerException">The exception that is the cause of the current exception.</param>
		public BarrierPostPhaseException(Exception innerException)
		  : this((string) null, innerException) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Threading.BarrierPostPhaseException" /> class with a specified error message.
		/// </summary>
		/// <param name="message">A string that describes the exception.</param>
		public BarrierPostPhaseException(string message)
		  : this(message, (Exception) null) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Threading.BarrierPostPhaseException" /> class with a specified error message and inner exception.
		/// </summary>
		/// <param name="message">A string that describes the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception.</param>
		public BarrierPostPhaseException(string message, Exception innerException)
		  : base(message == null ? "BarrierPostPhaseException" : message, innerException) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Threading.BarrierPostPhaseException" /> class with serialized data.
		/// </summary>
		/// <param name="info">The object that holds the serialized object data.</param>
		/// <param name="context">An object that describes the source or destination of the serialized data.</param>
		[SecurityCritical]
		protected BarrierPostPhaseException(SerializationInfo info, StreamingContext context)
		  : base(info, context) {
		}
	}
}