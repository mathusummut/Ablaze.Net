using System.ComponentModel;
using System.Drawing;
using System.Platforms.Windows;
using System.Text;

namespace System.Windows.Forms {
	/// <summary>
	/// An embeddable console control.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxItemFilter("System.Windows.Forms")]
	[DesignTimeVisible(true)]
	[DesignerCategory("CommonControls")]
	[Description("An embeddable console control.")]
	[DisplayName(nameof(ConsoleControl))]
	public class ConsoleControl : NewRichTextBox {
		/// <summary>
		/// The console event handler is used for console events.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="args">The <see cref="string"/> instance containing the event data.</param>
		public delegate void ConsoleEventHandler(object sender, string args);
		/// <summary>
		/// The internal process interface used to interface with the process.
		/// </summary>
		private ProcessInterface processInterace = new ProcessInterface();
		/// <summary>
		/// Current position that input starts at.
		/// </summary>
		private int inputStart = -1;
		/// <summary>
		/// The is input enabled flag.
		/// </summary>
		private bool isInputEnabled = true;
		/// <summary>
		/// The last input string (used so that we can make sure we don't echo input twice).
		/// </summary>
		private string lastInput;
		/// <summary>
		/// Occurs when console output is produced.
		/// </summary>
		public event ConsoleEventHandler OnConsoleOutput;
		/// <summary>
		/// Occurs when console input is produced.
		/// </summary>
		public event ConsoleEventHandler OnConsoleInput;

		/// <summary>
		/// Gets or sets a value indicating whether this instance is input enabled.
		/// </summary>
		[Description("Gets or sets a value indicating whether this instance is input enabled.")]
		[DefaultValue(true)]
		public bool IsInputEnabled {
			get {
				return isInputEnabled;
			}
			set {
				isInputEnabled = value;
				if (IsProcessRunning)
					ReadOnly = !value;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is process running.
		/// </summary>
		[Browsable(false)]
		public bool IsProcessRunning {
			get {
				return processInterace.IsProcessRunning;
			}
		}

		/// <summary>
		/// Gets the process interface.
		/// </summary>
		[Browsable(false)]
		public ProcessInterface ProcessInterface {
			get {
				return processInterace;
			}
		}

		/// <summary>
		/// Gets or sets whether to terminate the invoked process on close.
		/// </summary>
		[Description("Gets or sets whether to terminate the invoked process on close.")]
		[DefaultValue(false)]
		public bool TerminateProcessOnClose {
			get;
			set;
		}

		/// <summary>
		/// Specified that the RichTextBox instance should use RichEdit50W API instead of the old RichEdit20W.
		/// </summary>
		protected override CreateParams CreateParams {
			get {
				CreateParams parameters = base.CreateParams;
				try {
					NativeApi.LoadLibrary("MsftEdit.dll");
					parameters.ClassName = "RichEdit50W";
				} catch {
				}
				return parameters;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ConsoleControl"/> class.
		/// </summary>
		public ConsoleControl() {
			CheckForIllegalCrossThreadCalls = false;
			AcceptsTab = true;
			BackColor = Color.Black;
			Font = new Font("Consolas", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
			ForeColor = Color.White;
			Name = nameof(ConsoleControl);
			ReadOnly = true;
			Size = new Size(150, 150);
			TabIndex = 0;
			Text = string.Empty;
			IsInputEnabled = true;
			TerminateProcessOnClose = true;
			processInterace.ProcessOutput += processInterace_OnProcessOutput;
			processInterace.ProcessError += processInterace_OnProcessError;
			processInterace.ProcessExit += processInterace_OnProcessExit;
		}

		/// <summary>
		/// Handles the OnProcessError event of the processInterace control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="args">The <see cref="ProcessEventArgs"/> instance containing the event data.</param>
		private void processInterace_OnProcessError(object sender, ProcessEventArgs args) {
			WriteOutput(args.Content, Color.Red);
			FireConsoleOutputEvent(args.Content);
		}

		/// <summary>
		/// Handles the OnProcessOutput event of the processInterace control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="args">The <see cref="ProcessEventArgs"/> instance containing the event data.</param>
		private void processInterace_OnProcessOutput(object sender, ProcessEventArgs args) {
			WriteOutput(args.Content, ForeColor);
			FireConsoleOutputEvent(args.Content);
		}

		/// <summary>
		/// Handles the OnProcessExit event of the processInterace control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="args">The <see cref="ProcessEventArgs"/> instance containing the event data.</param>
		private void processInterace_OnProcessExit(object sender, ProcessEventArgs args) {
			if (IsHandleCreated)
				ReadOnly = true;
		}

		/// <summary>
		/// Fired when a key is being pressed.
		/// </summary>
		protected override void OnKeyDown(KeyEventArgs e) {
			if ((SelectionStart <= inputStart) && e.KeyCode == Keys.Back)
				e.SuppressKeyPress = true;
			if (e.KeyCode == Keys.Return) {
				if (SelectionStart >= inputStart)
					WriteInput(Text.Substring(inputStart, SelectionStart - inputStart), ForeColor, false);
				else
					e.SuppressKeyPress = true;
			} else if (SelectionStart < inputStart) {
				if (Extensions.ToString(e.KeyCode, e.Shift, e.Alt, e.Control).Length != 0) {
					string text = Text;
					int i = inputStart;
					while (i < text.Length && !(text[i] == '\r' || text[i] == '\n'))
						i++;
					SelectionStart = i;
				}
			}
			base.OnKeyDown(e);
		}

		/// <summary>
		/// Writes the output to the console control.
		/// </summary>
		/// <param name="output">The output.</param>
		/// <param name="color">The color.</param>
		public void WriteOutput(string output, Color color) {
			if ((!string.IsNullOrEmpty(lastInput) && (output == lastInput || output.Replace("\r\n", string.Empty) == lastInput)) || !IsHandleCreated)
				return;
			SelectionColor = color;
			SelectedText += output;
			inputStart = SelectionStart;
		}

		/// <summary>
		/// Clears the output.
		/// </summary>
		public void ClearOutput() {
			Clear();
			inputStart = 0;
		}

		/// <summary>
		/// Writes the input to the console control.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <param name="color">The color.</param>
		/// <param name="echo">Whether to echo the input.</param>
		public void WriteInput(string input, Color color, bool echo) {
			if (echo) {
				SelectionColor = color;
				SelectedText += input;
				inputStart = SelectionStart;
			}
			lastInput = input;
			processInterace.WriteInput(input);
			FireConsoleInputEvent(input);
		}

		/// <summary>
		/// Runs a process.
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		/// <param name="arguments">The arguments.</param>
		public void StartProcess(string fileName, string arguments = null) {
			processInterace.StartProcess(fileName, arguments);
			if (IsInputEnabled)
				ReadOnly = false;
		}

		/// <summary>
		/// Stops the process.
		/// </summary>
		public void StopProcess() {
			processInterace.StopProcess();
		}

		/// <summary>
		/// Fires the console output event.
		/// </summary>
		/// <param name="content">The content.</param>
		private void FireConsoleOutputEvent(string content) {
			ConsoleEventHandler handler = OnConsoleOutput;
			if (handler != null)
				handler(this, content);
		}

		/// <summary>
		/// Fires the console input event.
		/// </summary>
		/// <param name="content">The content.</param>
		private void FireConsoleInputEvent(string content) {
			ConsoleEventHandler handler = OnConsoleInput;
			if (handler != null)
				handler(this, content);
		}

		/// <summary>
		/// Called when the handle is destroyed.
		/// </summary>
		protected override void OnHandleDestroyed(EventArgs e) {
			base.OnHandleDestroyed(e);
			if (TerminateProcessOnClose && processInterace.IsProcessRunning)
				processInterace.Process.Kill();
		}
	}
}