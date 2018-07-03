using System;
using System.Drawing;
using System.Graphics.Models;
using System.Graphics.Models.Parsers;
using System.Graphics.OGL;
using System.Numerics;
using System.Windows.Forms;

namespace Crash {
	/// <summary>
	/// The main window.
	/// </summary>
	public class GameForm : GraphicsForm {
		private Scene trucks;
		private Vector3 viewTarget = Vector3.Zero;
		private Vector3 up = Vector3.UnitY;
		private Vector3 eye = new Vector3(300f, 0f, 0f);

		/// <summary>
		/// Initializes a game form instance.
		/// </summary>
		public GameForm() {
			InitializeComponent();
			trucks = new Scene(ModelParser.Parse(@"..\..\..\Models\Truck.obj"));
			BackColor = Color.MidnightBlue;
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
		}

		/// <summary>
		/// Called when the window is shown.
		/// </summary>
		/// <param name="e">Ignored.</param>
		protected override void OnShown(EventArgs e) {
			base.OnShown(e);
			UpdateTimerRunning = true;
		}

		protected override void OnUpdate(double elapsedMilliseconds) {
			float thing = (float) elapsedMilliseconds;
			if (KeyListener.IsKeyDown(Keys.Left)) {
				eye.X -= thing;
				viewTarget.X -= thing;
			}
			if (KeyListener.IsKeyDown(Keys.Right)) {
				eye.X += thing;
				viewTarget.X += thing;
			}
			if (KeyListener.IsKeyDown(Keys.Up)) {
				eye.Y += thing;
				viewTarget.Y += thing;
			}
			if (KeyListener.IsKeyDown(Keys.Down)) {
				eye.Y -= thing;
				viewTarget.Y -= thing;
			}
			InvalidateGL();
		}

		protected override void OnKeyDown(KeyEventArgs e) {
			base.OnKeyDown(e);
			if ((int) e.KeyCode == 32) {
				IModel clone = trucks[trucks.Count - 1].Clone(true);
				clone.Location += new Vector3(5, 5, 5);
				clone.Scale *= 1.5f;
				clone.Rotation += new Vector3(1.2f, .2f, 2.4f);
				trucks.Add(clone);
			}
		}

		/// <summary>
		/// Called whenever a GL render takes place.
		/// </summary>
		protected override void OnPaintGL() {
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			//Enter drawing code here
			Matrix4 camera = Matrix4.LookAt(ref eye, ref viewTarget, ref up);
			MeshComponent.Setup3D(ViewSize, ref camera, 10f);
			trucks.Render();
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
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.ClientSize = new System.Drawing.Size(614, 475);
			this.Location = new System.Drawing.Point(200, 200);
			this.Name = "GameForm";
			this.Text = "Game Form";
			this.ResumeLayout(false);

		}
	}
}