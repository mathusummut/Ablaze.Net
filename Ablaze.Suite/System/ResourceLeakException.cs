using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System {
	/// <summary>
	/// Signifies that a resource has leaked.
	/// </summary>
	[ComVisible(true)]
	[Serializable]
	public class ResourceLeakException : Exception {
		/// <summary>
		/// An object that represents the resource that leaked.
		/// </summary>
		public readonly object Resource;

		/// <summary>
		/// Initializes a new instance of the ResourceLeakException class.
		/// </summary>
		/// <param name="resource">An object that represents the resource that leaked.</param>
		public ResourceLeakException(object resource) {
			Resource = resource;
		}

		/// <summary>
		/// Initializes a new instance of the ResourceLeakException class.
		/// </summary>
		/// <param name = "resource" > An object that represents the resource that leaked.</param>
		/// <param name="message">The message that describes what leaked.</param>
		public ResourceLeakException(object resource, string message) : base(message) {
			Resource = resource;
		}

		/// <summary>
		/// Initializes a new instance of the ResourceLeakException class.
		/// </summary>
		/// <param name="resource" > An object that represents the resource that leaked.</param>
		/// <param name="message">The message that describes what leaked.</param>
		/// <param name="innerException">The exception that is the cause of the current exception.</param>
		public ResourceLeakException(object resource, string message, Exception innerException) : base(message, innerException) {
			Resource = resource;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.InvalidOperationException" /> class with serialized data.</summary>
		/// <param name="info">The object that holds the serialized object data. </param>
		/// <param name="context">The contextual information about the source or destination. </param>
		protected ResourceLeakException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
	}
}