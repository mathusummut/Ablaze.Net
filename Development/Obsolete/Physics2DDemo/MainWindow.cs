using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Graphics.GL;
using System.Graphics.Models;
using System.Graphics.Models.Parsers;
using System.Physics2D;
using System.Threading;
using System.Windows.Forms;

namespace ManagedEnginePhysicsDemo {
	public class MainWindow : ContextWindow {
		private StyledLabel fpsLabel = new StyledLabel("FPS: 0");
		private Physics Engine;
		private ElliseParticle wheelA, wheelB;
		private int oldFPS;
		public static SplashScreen splash;

		public MainWindow()	{
			InitializeGL();
			IsGdiEnabled = true;
			Text = "2D Physics Engine Demo";
			Icon = Properties.Resources.Icon;
			Updater.TimeInterval = 1.0;
			Updater.Mode = AsyncTimer.TimerMode.Accurate;
			Updater.Tick += Updater_Tick;
			ModelStructure.Setup();
			Mesh2D.Setup(new Vector2(645f, 510f), -10f, false);
			GL.Enable(EnableCap.DepthTest);
			fpsLabel.AutoSize = false;
			fpsLabel.Location = new Point(10, 10);
			fpsLabel.Size = new Size(110, 30);
			fpsLabel.ForeColor = Color.Yellow;
			fpsLabel.BackColor = Color.Transparent;
			Font = new Font("Calibri Light", 16f);
			fpsLabel.Font = new Font("Calibri Light", 15f);
			Size screenSize = Screen.GetBounds(this).Size;
			Location = new Point(screenSize.Width / 2 - Width / 2, screenSize.Height / 2 - Height / 2);
			Engine = new Physics(100.0);
			InitWorld();
			Engine.Running = true;
		}

		private void InitWorld() {
			//air
			Vector2D halfWindow = new Vector2D(ClientSize.Width * 0.5, ClientSize.Height * 0.5);
			Fluid air = new Fluid(new PolygonParticle(halfWindow, halfWindow, 0.01, 0.01, 0.001, true, true, true, 9f, 0, 0, TextureParser.Parse("sunset.jpg")[0], Vector2D.Zero), 1.0, 0.001);
			air.AddFluidCurrent(new Force(new Vector2D(0.0, 0.02), 1.0, 0, false, true, false));
			Engine.Particles.Add(air);
			//floor
			Texture woodTexture = TextureParser.Parse("wood.bmp")[0];
			PolygonParticle floor = new PolygonParticle(new Vector2D(325.0), new Vector2D(175.0, 25.0), 100.0, 0.2, 0.01, true, true, true, 4f, 0, 0, woodTexture, Vector2D.Zero);
			Engine.Particles.Add(floor);
			//lift
			Engine.Particles.Add(new Lift(new PolygonParticle(new Vector2D(560.0, 300.0), new Vector2D(59.0, 5.0), 10.0, 0.4, 0.01, false, true, true, 4f, 0, 0, woodTexture, Vector2D.Zero), new LinearPath(10.0, true, new Vector2D(560.0, 300.0), new Vector2D(560.0, 500.0))));
			//tree top
			Texture rockTexture = TextureParser.Parse("rock.bmp")[0];
			ElliseParticle treeTop = new ElliseParticle(new Vector2D(300.0, 420.0), new Vector2D(20.0), 10.0, 0.2, 0.01, false, true, true, 3f, 0, 0, rockTexture, Vector2D.Zero);
			Engine.Particles.Add(treeTop);
			//tree roots
			ElliseParticle treeRoots = new ElliseParticle(new Vector2D(300.0, 500.0), Vector2D.Zero, 1.0, 0.2, 0.01, true, false, false, 0f, 0, 0, null, Vector2D.Zero);
			Engine.Particles.Add(treeRoots);
			//tree bark
			Engine.Particles.Add(new Spring(treeTop, treeRoots, new Vector2D(0.3, 8.0), -1.0, 2.0, 0.2, 0.01, true, true, 3f, 0, 0, woodTexture, Vector2D.Zero, 0.0, 0.0, 1000.0, true, true, true));
			//cellar floor
			Engine.Particles.Add(new PolygonParticle(new Vector2D(324.0, 500.0), new Vector2D(325.0, 25.0), 1.0, 0.2, 0.01, true, true, true, -4f, 0, 0, woodTexture, Vector2D.Zero));
			//water
			PolygonParticle water = new PolygonParticle(new Vector2D(325.0, 440.0), new Vector2D(295.0, 50.0), 20.0, 0.05, 0.005, true, true, true, 0f, 0, 0, TextureParser.Parse("water.png")[0], Vector2D.Zero);
			water.Model.HasAlpha = true;
			Engine.Particles.Add(new Fluid(water, 1.0, 0.05));
			//floor bump A
			Engine.Particles.Add(new PolygonParticle(new Vector2D(400.0, 300.0), new Vector2D(60.0, 25.0), 1.0, 0.2, 0.01, true, true, true, 5f, 0, 0, woodTexture, Vector2D.Zero, 0.4));
			//floor bump B
			Engine.Particles.Add(new PolygonParticle(new Vector2D(310.0, 300.0), new Vector2D(60.0, 25.0), 1.0, 0.2, 0.01, true, true, true, 5f, 0, 0, woodTexture, Vector2D.Zero, -0.4));
			//floor left angle
			Engine.Particles.Add(new PolygonParticle(new Vector2D(40.0, 290.0), new Vector2D(40.0, 10.0), 1.0, 0.2, 0.01, true, true, true, 4f, 0, 0, woodTexture, Vector2D.Zero, 0.8));
			//left wall
			Engine.Particles.Add(new PolygonParticle(new Vector2D(15.0, 99.0), new Vector2D(15.0, 400.0), 1.0, 0.2, 0.01, true, true, true, -4f, 0, 0, woodTexture, Vector2D.Zero));
			//right wall
			Engine.Particles.Add(new PolygonParticle(new Vector2D(634.0, 99.0), new Vector2D(15.0, 400.0), 1.0, 0.2, 0.01, true, true, true, -4f, 0, 0, woodTexture, Vector2D.Zero));
			//bridge start
			PolygonParticle bridgeStart = new PolygonParticle(new Vector2D(80.0, 70.0), new Vector2D(75.0, 13.0), 1.0, 0.2, 0.01, true, true, true, 4f, 0, 0, woodTexture, Vector2D.Zero, 0.0, 0.0, 0.0, 0.0, false, true);
			Engine.Particles.Add(bridgeStart);
			//bridge end
			PolygonParticle bridgeEnd = new PolygonParticle(new Vector2D(380.0, 70.0), new Vector2D(50.0, 13.0), 1.0, 0.2, 0.01, true, true, true, 4f, 0, 0, woodTexture, Vector2D.Zero, 0.0, 0.0, 0.0, 0.0, true);
			Engine.Particles.Add(bridgeEnd);
			//bridge end angle
			Engine.Particles.Add(new PolygonParticle(new Vector2D(455.0, 102.0), new Vector2D(50.0, 13.0), 1.0, 0.2, 0.01, true, true, true, 4f, 0, 0, woodTexture, Vector2D.Zero, 0.8));
			//right wall angle
			Engine.Particles.Add(new PolygonParticle(new Vector2D(595.0, 102.0), new Vector2D(50.0, 13.0), 1.0, 0.2, 0.01, true, true, true, 4f, 0, 0, woodTexture, Vector2D.Zero, -0.8));
			//cannon
			PolygonParticle cannonRect = new PolygonParticle(new Vector2D(350.0, 180.0), new Vector2D(35.0, 7.0), 50, 0.1, 0.01, false, true, true, 4f, 0, 0, woodTexture, Vector2D.Zero, 1.0);
			ElliseParticle projectile = new ElliseParticle(new Vector2D(100.0), new Vector2D(3.5), 1.0, 0.2, 0.01, false, true, true, 2f, 0, 0, woodTexture, Vector2D.Zero, 0.0, 0.0, 0.0, 0.0, 10000.0);
            Engine.Particles.Add(new Cannon(cannonRect,	projectile, Vector2D.One, 100.0, 100.0, (cannonRect.Vertex1 + cannonRect.Vertex2) * 0.5 + projectile.Radius - cannonRect.Location));
			//bridge particle A
			ElliseParticle bridgePA = new ElliseParticle(new Vector2D(200.0, 70.0), new Vector2D(4.0), 1.0, 0.2, 0.01, false, true, true, 4f, 0, 0, rockTexture, Vector2D.Zero);
			Engine.Particles.Add(bridgePA);
			//bridge particle B
			ElliseParticle bridgePB = new ElliseParticle(new Vector2D(240.0, 70.0), new Vector2D(4.0), 1.0, 0.2, 0.01, false, true, true, 4f, 0, 0, rockTexture, Vector2D.Zero);
			Engine.Particles.Add(bridgePB);
			//bridge particle C
			ElliseParticle bridgePC = new ElliseParticle(new Vector2D(280.0, 70.0), new Vector2D(4.0), 1.0, 0.2, 0.01, false, true, true, 4f, 0, 0, rockTexture, Vector2D.Zero);
			Engine.Particles.Add(bridgePC);
			//bridge connector A
			Engine.Particles.Add(new Spring(bridgeStart.Corner1, bridgePA, new Vector2D(0.3, 5.0), 0.4, 1, 0.2, 0.01, true, true, 4f, 0, 0, woodTexture, Vector2D.Zero, 0.0, 0.0, 1000.0, true, true, true));
			//bridge connector B
			Engine.Particles.Add(new Spring(bridgePA, bridgePB, new Vector2D(0.4, 1.0), 0.4, 1.0, 0.2, 0.01, true, true, 4f, 0, 0, woodTexture, Vector2D.Zero, 0.0, 0.0, 1000.0, true, true, true));
			//bridge connector C
			Engine.Particles.Add(new Spring(bridgePB, bridgePC, new Vector2D(0.3, 5.0), 0.4, 1.0, 0.2, 0.01, true, true, 4f, 0, 0, woodTexture, Vector2D.Zero, 0.0, 0.0, 1000.0, true, true, true));
			//bridge connector D
			Engine.Particles.Add(new Spring(bridgePC, bridgeEnd.Corner0, new Vector2D(0.3, 5.0), 0.4, 1.0, 0.3, 0.01, true, true, 4f, 0, 0, woodTexture, Vector2D.Zero, 0.0, 0.0, 1000.0, true, true, true));
			//wheel A
			wheelA = new ElliseParticle(new Vector2D(60.0, 10.0), new Vector2D(20.0), 3.0, 0.2, 0.02, false, true, true, 2f, 0, 0, rockTexture, Vector2D.Zero, 0.0, 0.0, 0.5, 2.0);
			Engine.Particles.Add(wheelA);
			//wheel B
			wheelB = new ElliseParticle(new Vector2D(140.0, 10.0), new Vector2D(20.0), 3.0, 0.2, 0.02, false, true, true, 2f, 0, 0, rockTexture, Vector2D.Zero, 0.0, 0.0, 0.5, 2.0);
			Engine.Particles.Add(wheelB);
			//wheel connector
			Engine.Particles.Add(new Spring(wheelA, wheelB, new Vector2D(0.5, 5.0), 5.0, 5.0, 0.2, 0.01, true, true, 2.2f, 0, 0, woodTexture, Vector2D.Zero, 0.0, 0.0, 1000.0, true, true, true));
			//little rectangle
			PolygonParticle littleRect = new PolygonParticle(new Vector2D(547.0, 238.0), new Vector2D(5.0), 5.0, 0.2, 0.01, false, true, true, 4f, 0, 0, woodTexture, Vector2D.Zero);
			Engine.Particles.Add(littleRect);
			//rotating rectangle
			PolygonParticle rotatingRect = new PolygonParticle(new Vector2D(525.0, 180.0), new Vector2D(35.0, 7.0), 1.0, 0.2, 0.01, true, true, true, 4f, 0, 0, woodTexture, Vector2D.Zero, 0.0, 0.015, 0.015, 0.0, false, true);
			Engine.Particles.Add(rotatingRect);
			//rotating rectangle connector
			Engine.Particles.Add(new Spring(rotatingRect.Corner1, littleRect, new Vector2D(0.5, 1.0), 15.0, 1.0, 0.2, 0.01, false, true, 7f, 0, 0, woodTexture, Vector2D.Zero, 0.0, 0.0, 1000.0, true, true, true));
			Random rnd = new Random();
			for (int px = 120; px < 520; px += 7) {
				//little rectangle
				Engine.Particles.Add(new PolygonParticle(new Vector2D(px, 200.0), new Vector2D(5.0, 2.5), 1.0, 0.2, 0.01, false, true, true, 2f, 0, 0, woodTexture, Vector2D.Zero, rnd.NextDouble() * Math.PI));
				//little circle
				Engine.Particles.Add(new ElliseParticle(new Vector2D(px + 40.0, 210.0), new Vector2D(3.5), 2.0, 0.2, 0.01, false, true, true, 2f, 0, 0, rockTexture, Vector2D.Zero));
			}
		}

		private void Updater_Tick(object sender, double elapsedMilliseconds) {
			const double speed = 0.01f;
			if (IsKeyDown(Keys.Right)) {
				wheelA.RotationalAcceleration += speed;
				wheelB.RotationalAcceleration += speed;
			} else if (IsKeyDown(Keys.Left)) {
				wheelA.RotationalAcceleration -= speed;
				wheelB.RotationalAcceleration -= speed;
			}
			Engine.Update();
			MarkUpdateFinished();
		}

		protected override void OnKeyDown(KeyEventArgs e) {
			switch (e.KeyCode) {
				case Keys.P:
					bool notPaused = Updater.Paused;
					Engine.Running = notPaused;
					Updater.Paused = !Updater.Paused;
					break;
				case Keys.R:
					Updater.Paused = true;
					Engine.Dispose();
					InitWorld();
					Engine.Running = true;
					Updater.Paused = false;
					break;
				case Keys.F11:
					IsFullScreen = !IsFullScreen;
					break;
				case Keys.F4:
					if (ModifierKeys.HasFlag(Keys.Alt))
						Close();
					break;
			}
		}

		protected override void OnKeyUp(KeyEventArgs e) {
			wheelA.RotationalAcceleration = 0f;
			wheelB.RotationalAcceleration = 0f;
		}

		protected override void OnRendered() {
			SetToDefaultState();
			Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.Normal;
			splash.Close();
			splash.Dispose();
			splash = null;
			Updater.Enabled = true;
		}

		protected override void OnFPSUpdated() {
			if (FramesPerSecond == oldFPS)
				return;
			oldFPS = FramesPerSecond;
			InvalidateGdi();
		}

		protected override void OnPaintGDILayer(Graphics g) {
			fpsLabel.Text = "FPS: " + oldFPS;
			GDIGraphics.SetClip(ClientRectangle, CombineMode.Replace);
			GDIGraphics.Clear(Color.Transparent);
			fpsLabel.DrawOnCanvas(GDIGraphics);
		}

		protected override void OnRender() {
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			Engine.Render();
			base.OnRender();
		}

		protected override void OnUnload() {
			Engine.Dispose();
			fpsLabel.Dispose();
		}

		protected override void OnDisposed() {
			Application.ExitThread();
		}

		private static void UniversalExceptionHandler(object sender, UnhandledExceptionEventArgs e) {
			ErrorDialog.Show("A serious error has occurred.", true, (Exception) e.ExceptionObject);
		}

		private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e) {
			ErrorDialog.Show("A thread exception has occurred.", true, e.Exception);
		}

		protected override void InitializeComponent() {
			this.SuspendLayout();
			// 
			// MainWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.ClientSize = new System.Drawing.Size(919, 695);
			this.Location = new System.Drawing.Point(0, 0);
			this.Name = "MainWindow";
			this.Opacity = 1D;
			this.Text = "2D ManagedEngine Demo";
			this.ViewSize = new System.Drawing.Size(903, 653);
			this.ResumeLayout(false);

		}

		[STAThread]
		public static void Main() {
			Process current = Process.GetCurrentProcess();
			current.PriorityBoostEnabled = true;
			current.PriorityClass = ProcessPriorityClass.High;
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UniversalExceptionHandler);
			Application.ThreadException += Application_ThreadException;
			splash = new SplashScreen(Properties.Resources.Cover);
			splash.Show();
			Application.Run(new MainWindow());
		}
	}
}