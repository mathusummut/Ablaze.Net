namespace System.Windows.Forms {
	/// <summary>
	/// The ProcessEventArgs are arguments for a console event.
	/// </summary>
	public class ProcessEventArgs : EventArgs {
		/// <summary>
		/// Gets the content.
		/// </summary>
		public string Content {
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets the code.
		/// </summary>
		/// <value>
		/// The code.
		/// </value>
		public int? ExitCode {
			get;
			private set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ProcessEventArgs"/> class.
		/// </summary>
		public ProcessEventArgs() {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ProcessEventArgs"/> class.
		/// </summary>
		/// <param name="content">The content.</param>
		public ProcessEventArgs(string content) {
			//  Set the content and code.
			Content = content;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ProcessEventArgs"/> class.
		/// </summary>
		/// <param name="exitCode">The exit code.</param>
		public ProcessEventArgs(int exitCode) {
			//  Set the content and code.
			ExitCode = exitCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ProcessEventArgs"/> class.
		/// </summary>
		/// <param name="content">The content.</param>
		/// <param name="exitCode">The exit code.</param>
		public ProcessEventArgs(string content, int exitCode) {
			//  Set the content and code.
			Content = content;
			ExitCode = exitCode;
		}
	}
}