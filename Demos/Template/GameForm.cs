using System;
using System.Diagnostics;
using System.Drawing;
using System.Graphics;
using System.Graphics.OGL;
using System.Graphics.Models;
using System.Threading;
using System.Windows.Forms;

namespace Template {
	/// <summary>
	/// The main window.
	/// </summary>
	public class GameForm : GraphicsForm {
		/// <summary>
		/// Initializes a game form instance.
		/// </summary>
		public GameForm() {
			InitializeComponent();
			InitializeGL(true);
			EnableFullscreenOnAltEnter = true;
			UpdateInterval = 6;
		}

		/// <summary>
		/// Called when the OpenGL context is initialized.
		/// </summary>
		protected override void OnGLInitialized() {
			base.OnGLInitialized();
			MeshComponent.SetupGLEnvironment();
			GL.Enable(EnableCap.Light0);
			GL.ClearColor(Color.MidnightBlue);
			GL.Light(LightName.Light0, LightParameter.Ambient, ColorF.White);
		}

		/// <summary>
		/// Called when the window is shown.
		/// </summary>
		/// <param name="e">Ignored.</param>
		protected override void OnShown(EventArgs e) {
			base.OnShown(e);
			UpdateTimerRunning = true;
		}

		/// <summary>
		/// Called when the frame needs to be updated.
		/// </summary>
		/// <param name="elapsedMilliseconds">The elapsed milliseconds.</param>
		protected override void OnUpdate(double elapsedMilliseconds) {
		}

		/// <summary>
		/// Called whenever a GL render takes place.
		/// </summary>
		protected override void OnPaintGL() {
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			//Enter drawing code here
		}

		/// <summary>
		/// The entry point of the application.
		/// </summary>
		[STAThread]
		public static void Main() {
			MessageLoop.Run(new GameForm());
		}

		/// <summary>
		/// Configures parameters related to the form.
		/// </summary>
		public void InitializeComponent() {
			this.SuspendLayout();
			//
			// GameForm
			//
			this.ClientSize = new System.Drawing.Size(646, 513);
			this.Location = new System.Drawing.Point(200, 200);
			this.Name = "GameForm";
			this.Text = "Game Form";
			this.ResumeLayout(false);

		}
	}
}
