using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace System.Diagnostics {
	/// <summary>
	/// Describes the error dialog behavior
	/// </summary>
	public enum ErrorDialogAction {
		/// <summary>
		/// The error dialog is shown and the user can choose an action to take
		/// </summary>
		NormalUserChoice,
		/// <summary>
		/// The error dialog is not shown and the exception is thrown instead
		/// </summary>
		ThrowRegardless,
		/// <summary>
		/// Writes the exception to console and attempts to ignore the exception
		/// </summary>
		SilentOperation
	}

	/// <summary>
	/// An enumeration of all the buttons that can be shown in the error dialog.
	/// </summary>
	[Flags]
	public enum ErrorDialogButton : int {
		/// <summary>
		/// Tries to ignore the error that happened.
		/// </summary>
		Ignore = 1,
		/// <summary>
		/// Throws the ecxeptions that happened.
		/// </summary>
		Throw = 2,
		/// <summary>
		/// Kills the current process immediately.
		/// </summary>
		Quit = 4,
	}

	/// <summary>
	/// A verbose error message dialog, perfect for advanced flexible exception handling.
	/// </summary>
	public static class ErrorHandler {
		private static object SyncRoot = new object();
		/// <summary>
		/// The error logger to use.
		/// </summary>
		public static IErrorLogger Logger = new ErrorLogger();
		private static Exception invariantException;
		/// <summary>
		/// The behavior of the error dialog
		/// </summary>
		public static ErrorDialogAction Behavior;
		private static bool Ignore;
		private static int dialogCount;
		/// <summary>
		/// A constant for showing the default buttons.
		/// </summary>
		public const ErrorDialogButton DefaultButtons = ErrorDialogButton.Ignore | ErrorDialogButton.Throw | ErrorDialogButton.Quit;

		/// <summary>
		/// Shows an error dialog handling the specified exceptions.
		/// </summary>
		/// <param name="ex">An error containing the exceptions whose info to display.</param>
		public static void Show(params Exception[] ex) {
			Show(null, DefaultButtons, ex);
		}

		/// <summary>
		/// Shows an error dialog handling the specified exceptions.
		/// </summary>
		/// <param name="ex">An error containing the exceptions whose info to display.</param>
		public static void ShowAsync(params Exception[] ex) {
			ShowAsync(null, DefaultButtons, ex);
		}

		/// <summary>
		/// Shows an error dialog handling the specified exceptions.
		/// </summary>
		/// <param name="buttons">The buttons to show.</param>
		/// <param name="ex">An error containing the exceptions whose info to display.</param>
		public static void Show(ErrorDialogButton buttons = DefaultButtons, params Exception[] ex) {
			Show(null, buttons, ex);
		}

		/// <summary>
		/// Shows an error dialog handling the specified exceptions.
		/// </summary>
		/// <param name="buttons">The buttons to show.</param>
		/// <param name="ex">An error containing the exceptions whose info to display.</param>
		public static void ShowAsync(ErrorDialogButton buttons = DefaultButtons, params Exception[] ex) {
			ShowAsync(null, buttons, ex);
		}

		/// <summary>
		/// Shows an error dialog handling the specified exceptions.
		/// </summary>
		/// <param name="message">The error message to show on top.</param>
		/// <param name="ex">An error containing the exceptions whose info to display.</param>
		public static void Show(string message, params Exception[] ex) {
			Show(message, DefaultButtons, ex);
		}

		/// <summary>
		/// Shows an error dialog handling the specified exceptions.
		/// </summary>
		/// <param name="message">The error message to show on top.</param>
		/// <param name="ex">An error containing the exceptions whose info to display.</param>
		public static void ShowAsync(string message, params Exception[] ex) {
			ShowAsync(message, DefaultButtons, ex);
		}

		/// <summary>
		/// Shows an error dialog handling the specified exceptions.
		/// </summary>
		/// <param name="message">The error message to show on top.</param>
		/// <param name="buttons">The buttons to show.</param>
		/// <param name="ex">An error containing the exceptions whose info to display.</param>
		public static void Show(string message, ErrorDialogButton buttons = DefaultButtons, params Exception[] ex) {
			if (Ignore)
				return;
			string fullMessage = ExceptionToDetailedString(message, ex);
			Exception e = ex.GetFirstNonNull();
			LogException(e, fullMessage);
			if (Behavior == ErrorDialogAction.SilentOperation)
				return;
			else if (Behavior == ErrorDialogAction.ThrowRegardless)
				throw new ApplicationException(message, e);
			else if (invariantException != null)
				throw invariantException;
			if (Volatile.Read(ref dialogCount) == 0) {
				try {
					using (ErrorDialogForm dialog = new ErrorDialogForm(fullMessage, buttons, e))
						dialog.ShowDialog();
				} catch {
				}
			}
		}

		/// <summary>
		/// Shows an error dialog handling the specified exceptions.
		/// </summary>
		/// <param name="message">The error message to show on top.</param>
		/// <param name="buttons">The buttons to show.</param>
		/// <param name="ex">An error containing the exceptions whose info to display.</param>
		public static void ShowAsync(string message, ErrorDialogButton buttons = DefaultButtons, params Exception[] ex) {
			if (Ignore)
				return;
			string fullMessage = ExceptionToDetailedString(message, ex);
			Exception e = ex.GetFirstNonNull();
			LogException(e, fullMessage);
			if (Behavior == ErrorDialogAction.SilentOperation)
				return;
			else if (Behavior == ErrorDialogAction.ThrowRegardless)
				throw new ApplicationException(message, e);
			else if (invariantException != null)
				throw invariantException;
			if (Volatile.Read(ref dialogCount) == 0) {
				try {
					ErrorDialogForm dialog = new ErrorDialogForm(fullMessage, buttons, e);
					dialog.TopMost = true;
					dialog.Show();
				} catch {
				}
			}
		}

		private static string DateToString(DateTime date) {
			StringBuilder builder = new StringBuilder(23);
			int temp = date.Day;
			builder.Append(temp < 10 ? "0" + temp : temp.ToStringLookup());
			builder.Append('/');
			temp = date.Month;
			builder.Append(temp < 10 ? "0" + temp : temp.ToStringLookup());
			builder.Append('/');
			builder.Append(date.Year.ToString());
			builder.Append(" at ");
			temp = date.Hour;
			builder.Append(temp < 10 ? "0" + temp : temp.ToStringLookup());
			builder.Append(':');
			temp = date.Minute;
			builder.Append(temp < 10 ? "0" + temp : temp.ToStringLookup());
			builder.Append(':');
			temp = date.Second;
			builder.Append(temp < 10 ? "0" + temp : temp.ToStringLookup());
			return builder.ToString();
		}

		/// <summary>
		/// Generates a helpful structured string that describes the specified exceptions.
		/// </summary>
		/// <param name="message">A custom message that is added to the full string (can be null).</param>
		/// <param name="ex">A list of the exceptions to show the details of.</param>
		public static string ExceptionToDetailedString(string message, params Exception[] ex) {
			StringBuilder text = new StringBuilder(message == null || message.Length == 0 ? "=>An error occurred on " + DateToString(DateTime.Now) + ".\n" : "=>An error occurred on " + DateToString(DateTime.Now) + ". \n\nException details:\n\n   " + message + "\n");
			if (ex == null || ex.Length == 0)
				text.Append("\nNo further details are available.");
			else {
				List<Exception> exceptions = new List<Exception>(ex);
				exceptions.RemoveAll(item => item == null);
				if (exceptions.Count == 0)
					text.Append("\nNo further details are available.");
				else {
					List<Exception> extracted;
					int stackLevel, j, i = 0;
					Exception currentException;
					string temp;
					foreach (Exception current in exceptions.Distinct()) {
						text.Append("\nException number ");
						text.Append(i);
						text.AppendLine(":");
						stackLevel = 0;
						extracted = ExtractFromException(current);
						for (j = extracted.Count - 1; j >= 0; j--) {
							currentException = extracted[j];
							text.Append("\nException stack level ");
							text.Append(stackLevel);
							text.Append("\n\nType: ");
							text.Append(currentException.GetType().ToString());
							temp = currentException.Message;
							if (temp != null) {
								temp = temp.Trim();
								if (temp.Length != 0) {
									text.Append(":\n\nMessage:\n\n   ");
									text.Append(temp);
								}
							}
							temp = currentException.Source;
							if (temp != null) {
								temp = temp.Trim();
								if (temp.Length != 0) {
									text.Append("\n\nSource: ");
									text.Append(currentException.Source);
								}
							}
							temp = currentException.StackTrace;
							if (temp != null) {
								temp = temp.Trim();
								if (temp.Length != 0) {
									text.AppendLine("\n\nStack Trace:\n");
									text.AppendLine(currentException.StackTrace);
								}
							}
							stackLevel++;
						}
						i++;
					}
				}
			}
			return text.ToString();
		}

		/// <summary>
		/// Logs the specified exception
		/// </summary>
		/// <param name="ex">The exception to log.</param>
		public static void LogException(Exception ex) {
			if (ex != null)
				LogException(ex, ExceptionToDetailedString(null, ex));
		}

		/// <summary>
		/// Raises the LogException event.
		/// </summary>
		/// <param name="ex">The exception to log.</param>
		/// <param name="fullMessage">The full message string to log. Call ExceptionToDetailedString() to create a properly-formatted string.</param>
		public static void LogException(Exception ex, string fullMessage) {
			if (ex == null && fullMessage == null)
				return;
			if (Behavior == ErrorDialogAction.ThrowRegardless)
				throw new Exception(fullMessage, ex);
			else {
				lock (SyncRoot) {
					IErrorLogger logger = Logger;
					if (logger != null) {
						try {
							logger.LogException(ex, fullMessage);
						} catch {
						}
					}
				}
			}
		}

		/// <summary>
		/// Extracts all inner exceptions from an exception.
		/// </summary>
		/// <param name="ex">The exception to extract inner exceptions from.</param>
		public static List<Exception> ExtractFromException(Exception ex) {
			List<Exception> list = new List<Exception>();
			HashSet<Exception> set = new HashSet<Exception>();
			while (!(ex == null || set.Contains(ex))) {
				set.Add(ex);
				list.Add(ex);
				ex = ex.InnerException;
			}
			return list;
		}

		private static int IncrementDialogCount() {
			return Interlocked.Increment(ref dialogCount);
		}

		private sealed class ErrorDialogForm : Form {
			private readonly int count = IncrementDialogCount(); //leave this here
			private Panel panel1;
			private Button ignoreButton, throwButton, quitButton;
			private CheckBox checkBox1;
			private RichTextBox richTextBox1;
			private Exception Exception;

			public ErrorDialogForm(string message, ErrorDialogButton buttons, Exception ex) {
				InitializeComponent();
				if (buttons != (ErrorDialogButton.Ignore | ErrorDialogButton.Throw | ErrorDialogButton.Quit)) { //not all buttons (no ignore, throw and quit)
					if (HasFlag(buttons, ErrorDialogButton.Ignore)) { //has ignore
						if (HasFlag(buttons, ErrorDialogButton.Throw)) { //has ignore and throw
							panel1.Controls.Remove(quitButton);
						} else if (HasFlag(buttons, ErrorDialogButton.Quit)) { //has ignore and exit (no throw)
							panel1.Controls.Remove(throwButton);
							quitButton.Location = new Point(175, 0);
						} else {
							panel1.Controls.Remove(throwButton);
							panel1.Controls.Remove(quitButton);
						}
					} else if (HasFlag(buttons, ErrorDialogButton.Throw)) { //has throw (no ignore)
						panel1.Controls.Remove(ignoreButton);
						throwButton.Location = new Point(100, 0);
						if (HasFlag(buttons, ErrorDialogButton.Quit)) { //has throw and exit (no ignore)
							quitButton.Location = new Point(175, 0);
						} else { //has throw (no ignore and exit)
							panel1.Controls.Remove(quitButton);
						}
					} else if (HasFlag(buttons, ErrorDialogButton.Quit)) { //has exit (no ignore and throw)
						panel1.Controls.Remove(ignoreButton);
						panel1.Controls.Remove(throwButton);
						panel1.Controls.Remove(checkBox1);
					} else { //has no buttons
						Controls.Remove(panel1);
					}
				} //else has all buttons
				richTextBox1.Text = message;
				Exception = ex;
			}

			private static bool HasFlag(ErrorDialogButton flags, ErrorDialogButton flagToCheck) {
				return (flags & flagToCheck) == flagToCheck;
			}

			private void InitializeComponent() {
				this.richTextBox1 = new System.Windows.Forms.RichTextBox();
				this.panel1 = new System.Windows.Forms.Panel();
				this.ignoreButton = new System.Windows.Forms.Button();
				this.quitButton = new System.Windows.Forms.Button();
				this.throwButton = new System.Windows.Forms.Button();
				this.checkBox1 = new System.Windows.Forms.CheckBox();
				this.panel1.SuspendLayout();
				this.SuspendLayout();
				//
				// richTextBox1
				// 
				this.richTextBox1.BackColor = System.Drawing.SystemColors.InactiveCaption;
				this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
				this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
				this.richTextBox1.Location = new System.Drawing.Point(0, 0);
				this.richTextBox1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
				this.richTextBox1.Name = "richTextBox1";
				this.richTextBox1.ReadOnly = true;
				this.richTextBox1.Size = new System.Drawing.Size(421, 357);
				this.richTextBox1.TabIndex = 1;
				this.richTextBox1.Text = string.Empty;
				this.richTextBox1.WordWrap = false;
				// 
				// panel1
				// 
				this.panel1.BackColor = System.Drawing.SystemColors.InactiveCaption;
				this.panel1.Controls.Add(this.checkBox1);
				this.panel1.Controls.Add(this.quitButton);
				this.panel1.Controls.Add(this.throwButton);
				this.panel1.Controls.Add(this.ignoreButton);
				this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
				this.panel1.Location = new System.Drawing.Point(0, 357);
				this.panel1.Name = "panel1";
				this.panel1.Size = new System.Drawing.Size(441, 32);
				this.panel1.TabIndex = 2;
				// 
				// ignoreButton
				// 
				this.ignoreButton.Location = new System.Drawing.Point(100, 0);
				this.ignoreButton.Margin = new System.Windows.Forms.Padding(0);
				this.ignoreButton.Name = "ignoreButton";
				this.ignoreButton.Size = new System.Drawing.Size(75, 32);
				this.ignoreButton.TabIndex = 0;
				this.ignoreButton.Text = "Ignore";
				this.ignoreButton.UseVisualStyleBackColor = true;
				this.ignoreButton.Click += new System.EventHandler(this.button1_Click);
				// 
				// throwButton
				// 
				this.throwButton.Location = new System.Drawing.Point(175, 0);
				this.throwButton.Margin = new System.Windows.Forms.Padding(0);
				this.throwButton.Name = "throwButton";
				this.throwButton.Size = new System.Drawing.Size(75, 32);
				this.throwButton.TabIndex = 2;
				this.throwButton.Text = "Throw";
				this.throwButton.UseVisualStyleBackColor = true;
				this.throwButton.Click += new System.EventHandler(this.button2_Click);
				// 
				// quitButton
				// 
				this.quitButton.Location = new System.Drawing.Point(250, 0);
				this.quitButton.Margin = new System.Windows.Forms.Padding(0);
				this.quitButton.Name = "quitButton";
				this.quitButton.Size = new System.Drawing.Size(75, 32);
				this.quitButton.TabIndex = 4;
				this.quitButton.Text = "Quit";
				this.quitButton.UseVisualStyleBackColor = true;
				this.quitButton.Click += new System.EventHandler(this.button3_Click);
				// 
				// checkBox1
				// 
				this.checkBox1.Location = new System.Drawing.Point(10, 0);
				this.checkBox1.Margin = new System.Windows.Forms.Padding(0);
				this.checkBox1.Name = "checkBox1";
				this.checkBox1.Size = new System.Drawing.Size(90, 32);
				this.checkBox1.TabIndex = 3;
				this.checkBox1.Text = "Apply to all";
				// 
				// ErrorDialog
				// 
				this.AcceptButton = this.ignoreButton;
				this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
				this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
				this.BackColor = System.Drawing.SystemColors.Menu;
				this.ClientSize = new System.Drawing.Size(420, 450);
				this.ControlBox = false;
				this.Controls.Add(this.richTextBox1);
				this.Controls.Add(this.panel1);
				this.Font = new System.Drawing.Font("Calibri Light", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
				this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
				this.MaximizeBox = false;
				this.MinimizeBox = false;
				this.Name = "ErrorDialog";
				this.ShowIcon = false;
				this.ShowInTaskbar = false;
				this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
				this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
				this.Text = "Error Dialog";
				this.TopMost = true;
				this.panel1.ResumeLayout(false);
				this.ResumeLayout(false);

			}

			private void button1_Click(object sender, EventArgs e) {
				Ignore = checkBox1.Checked;
				Close();
			}

			private void button2_Click(object sender, EventArgs e) {
				if (checkBox1.Checked && Exception != null) {
					invariantException = new ApplicationException("User chose to throw exception in the dialog.", Exception);
					Exception = invariantException;
				}
				Close();
				throw Exception;
			}

			private void button3_Click(object sender, EventArgs e) {
				Close();
				Environment.Exit(1);
			}

			protected override void OnFormClosed(FormClosedEventArgs e) {
				base.OnFormClosed(e);
				Interlocked.Decrement(ref dialogCount);
			}
		}
	}
}