using System.Collections.Generic;
using System.Linq;

namespace System.Text {
	/// <summary>Options for Save.</summary>
	public class HjsonOptions {
		IHjsonDsfProvider[] dsf;

		/// <summary>Initializes a new instance of this class.</summary>
		public HjsonOptions() {
			EmitRootBraces = true;
			DisposeStream = true;
		}

		/// <summary>Keep white space and comments.</summary>
		public bool KeepWsc {
			get; set;
		}

		/// <summary>Show braces at the root level (default true).</summary>
		public bool EmitRootBraces {
			get; set;
		}

		/// <summary>Whether to dispose the stream after reading the object.</summary>
		public bool DisposeStream {
			get; set;
		}

		/// <summary>
		/// Gets or sets DSF providers.
		/// </summary>
		public IEnumerable<IHjsonDsfProvider> DsfProviders {
			get {
				return dsf ?? Enumerable.Empty<IHjsonDsfProvider>();
			}
			set {
				dsf = value.ToArray();
			}
		}
	}
}