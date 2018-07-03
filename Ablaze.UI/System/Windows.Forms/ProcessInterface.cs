using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace System.Windows.Forms {
	/// <summary>
	/// A class the wraps a process, allowing programmatic input and output.
	/// </summary>
	public class ProcessInterface {
		/// <summary>
		/// A ProcessEventHandler is a delegate for process input/output events.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="args">The <see cref="ProcessEventArgs"/> instance containing the event data.</param>
		public delegate void ProcessEventHandler(object sender, ProcessEventArgs args);

		/// <summary>
		/// The underlying process.
		/// </summary>
		private Process process;
		/// <summary>
		/// The input writer.
		/// </summary>
		private StreamWriter inputWriter;
		/// <summary>
		/// The output reader.
		/// </summary>
		private TextReader outputReader;
		/// <summary>
		/// The error reader.
		/// </summary>
		private TextReader errorReader;
		/// <summary>
		/// The output worker.
		/// </summary>
		private BackgroundWorker outputWorker = new BackgroundWorker();
		/// <summary>
		/// The error worker.
		/// </summary>
		private BackgroundWorker errorWorker = new BackgroundWorker();
		/// <summary>
		/// Current process file name.
		/// </summary>
		private string processFileName;
		/// <summary>
		/// Arguments sent to the current process.
		/// </summary>
		private string processArguments;
		/// <summary>
		/// Occurs when process output is produced.
		/// </summary>
		public event ProcessEventHandler ProcessOutput;
		/// <summary>
		/// Occurs when process error output is produced.
		/// </summary>
		public event ProcessEventHandler ProcessError;
		/// <summary>
		/// Occurs when process input is produced.
		/// </summary>
		public event ProcessEventHandler ProcessInput;
		/// <summary>
		/// Occurs when the process ends.
		/// </summary>
		public event ProcessEventHandler ProcessExit;

		/// <summary>
		/// Gets a value indicating whether this instance is process running.
		/// </summary>
		public bool IsProcessRunning {
			get {
				try {
					return !(process == null || process.HasExited);
				} catch {
					return false;
				}
			}
		}

		/// <summary>
		/// Gets the process.
		/// </summary>
		public Process Process {
			get {
				return process;
			}
		}

		/// <summary>
		/// Gets the name of the process.
		/// </summary>
		/// <value>
		/// The name of the process.
		/// </value>
		public string ProcessFileName {
			get {
				return processFileName;
			}
		}

		/// <summary>
		/// Gets the process arguments.
		/// </summary>
		public string ProcessArguments {
			get {
				return processArguments;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ProcessInterface"/> class.
		/// </summary>
		public ProcessInterface() {
			//  Configure the output worker.
			outputWorker.WorkerReportsProgress = true;
			outputWorker.WorkerSupportsCancellation = true;
			outputWorker.DoWork += outputWorker_DoWork;
			outputWorker.ProgressChanged += outputWorker_ProgressChanged;

			//  Configure the error worker.
			errorWorker.WorkerReportsProgress = true;
			errorWorker.WorkerSupportsCancellation = true;
			errorWorker.DoWork += errorWorker_DoWork;
			errorWorker.ProgressChanged += errorWorker_ProgressChanged;
		}

		/// <summary>
		/// Handles the ProgressChanged event of the outputWorker control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="ProgressChangedEventArgs"/> instance containing the event data.</param>
		private void outputWorker_ProgressChanged(object sender, ProgressChangedEventArgs e) {
			//  We must be passed a string in the user state.
			string output = e.UserState as string;
			if (output != null) {
				//  Fire the output event.
				RaiseProcessEvent(ProcessOutput, new ProcessEventArgs(output));
			}
		}

		/// <summary>
		/// Handles the DoWork event of the outputWorker control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="DoWorkEventArgs"/> instance containing the event data.</param>
		private void outputWorker_DoWork(object sender, DoWorkEventArgs e) {
			char[] buffer = new char[1024];
			int count;
			StringBuilder builder;
			while (!outputWorker.CancellationPending) {
				do {
					builder = new StringBuilder();
					count = outputReader.Read(buffer, 0, 1024);
					builder.Append(buffer, 0, count);
					outputWorker.ReportProgress(0, builder.ToString());
				} while (count > 0);
				Thread.Sleep(200);
			}
		}

		/// <summary>
		/// Handles the ProgressChanged event of the errorWorker control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="ProgressChangedEventArgs"/> instance containing the event data.</param>
		private void errorWorker_ProgressChanged(object sender, ProgressChangedEventArgs e) {
			//  The userstate must be a string.
			string error = e.UserState as string;
			if (error != null) {
				//  Fire the error event.
				RaiseProcessEvent(ProcessError, new ProcessEventArgs(error));
			}
		}

		/// <summary>
		/// Handles the DoWork event of the errorWorker control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="DoWorkEventArgs"/> instance containing the event data.</param>
		private void errorWorker_DoWork(object sender, DoWorkEventArgs e) {
			int count;
			char[] buffer = new char[1024];
			StringBuilder builder;
			while (!errorWorker.CancellationPending) {
				do {
					builder = new StringBuilder();
					count = errorReader.Read(buffer, 0, 1024);
					builder.Append(buffer, 0, count);
					outputWorker.ReportProgress(0, builder.ToString());
				} while (count > 0);
				Thread.Sleep(200);
			}
		}

		/// <summary>
		/// Runs a process.
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		/// <param name="arguments">The arguments.</param>
		public void StartProcess(string fileName, string arguments = null) {
			//  Create the process start info.
			ProcessStartInfo processStartInfo = new ProcessStartInfo(fileName, arguments) {

				//  Set the options.
				UseShellExecute = false,
				ErrorDialog = false,
				CreateNoWindow = true,

				//  Specify redirection.
				RedirectStandardError = true,
				RedirectStandardInput = true,
				RedirectStandardOutput = true
			};

			//  Create the process.
			process = new Process();
			process.EnableRaisingEvents = true;
			process.StartInfo = processStartInfo;
			process.Exited += currentProcess_Exited;

			//  Start the process.
			try {
				process.Start();
			} catch {
				return;
			}

			//  Store name and arguments.
			processFileName = fileName;
			processArguments = arguments;

			//  Create the readers and writers.
			inputWriter = process.StandardInput;
			outputReader = TextReader.Synchronized(process.StandardOutput);
			errorReader = TextReader.Synchronized(process.StandardError);

			//  Run the workers that read output and error.
			outputWorker.RunWorkerAsync();
			errorWorker.RunWorkerAsync();
		}

		/// <summary>
		/// Stops the process.
		/// </summary>
		public void StopProcess() {
			//  Handle the trivial case.
			if (IsProcessRunning)
				process.Kill();
		}

		/// <summary>
		/// Handles the Exited event of the currentProcess control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void currentProcess_Exited(object sender, EventArgs e) {
			//  Fire process exited.
			RaiseProcessEvent(ProcessExit, new ProcessEventArgs(process.ExitCode));
			//  Disable the threads.
			outputWorker.CancelAsync();
			errorWorker.CancelAsync();
			inputWriter = null;
			outputReader = null;
			errorReader = null;
			process = null;
			processFileName = null;
			processArguments = null;
			outputWorker.Dispose();
			errorWorker.Dispose();
		}

		/// <summary>
		/// Fires the process output event.
		/// </summary>
		/// <param name="handler">The event to fire.</param>
		/// <param name="content">The content.</param>
		private void RaiseProcessEvent(ProcessEventHandler handler, ProcessEventArgs content) {
			//  Get the event and fire it.
			if (handler != null)
				handler(this, content);
		}

		/// <summary>
		/// Writes the input.
		/// </summary>
		/// <param name="input">The input.</param>
		public void WriteInput(string input) {
			if (IsProcessRunning) {
				inputWriter.WriteLine(input);
				inputWriter.Flush();
				RaiseProcessEvent(ProcessInput, new ProcessEventArgs(input));
			}
		}
	}
}