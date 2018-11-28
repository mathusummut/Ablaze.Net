using System;
using System.Collections.Generic;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using SlimDX;
using SlimDX.Direct3D9;

namespace PhysicsEngine {
	public class PhysicsEngine : StyledForm {
		private Device device;
		private Direct3D context;
		private WorldState worldState;
		private const int updateInterval = 7;
		private const float stepMultplier = 0.0006f * updateInterval;
		private AsyncTimer timer;

		[STAThread]
		public static void Main() {
			MessageLoop.Run(new PhysicsEngine());
		}

		public PhysicsEngine() {
			InitializeComponent();
			SetStyle(ControlStyles.OptimizedDoubleBuffer, false);
			timer = new AsyncTimer(updateInterval);
			SuppressClear = true;
			EnableFullscreenOnAltEnter = true;
		}

		protected override void OnKeyDown(KeyEventArgs e) {
			if (e.KeyCode == Keys.P)
				timer.Running = !timer.Running;
		}

		protected override void OnShown(EventArgs e) {
			base.OnShown(e);
			PresentParameters present = new PresentParameters();
			present.Windowed = true;
			present.SwapEffect = SwapEffect.Flip;
			present.EnableAutoDepthStencil = true;
			present.AutoDepthStencilFormat = Format.D16;
			Screen current = Screen.FromControl(this);
			present.BackBufferWidth = current.Bounds.Width;
			present.BackBufferHeight = current.Bounds.Height;
			context = new Direct3D();
			device = new Device(context, 0, DeviceType.Hardware, Handle, CreateFlags.HardwareVertexProcessing, present);
			Material mtrl = new Material();
			mtrl.Diffuse = Color.White;
			device.Material = mtrl;
			device.SetRenderState(RenderState.Lighting, true);
			device.SetLight(0, new Light() {
				Type = LightType.Directional,
				Direction = new Vector3(0.3f, -0.5f, 0.2f),
				Diffuse = Color.White,
			});
			device.EnableLight(0, true);
			device.SetRenderState(RenderState.ScissorTestEnable, true);
			device.ScissorRect = ViewPort;
			List<RigidBody> bodies = new List<RigidBody>();
			RigidBodyType boxType = new RigidBodyType(Environment.CurrentDirectory + "\\Models\\box.x", device);
			for (int i = 0; i < 7; i++)
				bodies.Add(new RigidBody(boxType, new Vector3(0, 1 + 3 * i, 0), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), new Vector3(0, 0, 0), .2f, .5f, 1.0f));
			bodies.Add(new RigidBody(boxType, new Vector3(-40, 4, 0), new Vector3(10, 0, 0), new Quaternion(0, 0, 0, 0), new Vector3(0, 0, 0), .2f, .5f, float.PositiveInfinity));
			bodies.Add(new RigidBody(new RigidBodyType(Environment.CurrentDirectory + "\\Models\\platform.x", device), new Vector3(0, -5, 0), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), new Vector3(0, 0, 0), .2f, .5f, float.PositiveInfinity));
			worldState = new WorldState(bodies);
			timer.Tick += Timer_Elapsed;
			timer.Running = true;
		}

		private void Timer_Elapsed(object sender) {
			worldState.Update(stepMultplier);
			try {
				Invoke(new Action(OnRender));
			} catch {
				timer.Running = false;
			}
		}

		protected override void OnClientSizeChanged(EventArgs e) {
			base.OnClientSizeChanged(e);
			if (device != null)
				device.ScissorRect = ViewPort;
		}

		public void OnRender() {
			if (device == null)
				return;
			device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, EnableAeroBlur && Extensions.IsAeroEnabled ? Color.FromArgb(180, Color.Black) : Color.Black, 1.0f, 0, new Rectangle[] { ViewPort });
			device.BeginScene();
			device.SetTransform(TransformState.View, Matrix.LookAtLH(new Vector3(0.0f, 20.0f, -32.0f), new Vector3(0, 0, 0), new Vector3(0.0f, 1.0f, 0.0f)));
			device.SetTransform(TransformState.Projection, Matrix.PerspectiveFovLH((float) Math.PI / 4, (float) Width / Height, 1.0f, 100.0f));
			// Render the world
			worldState.Render(device);
			device.EndScene();
			device.Present();
		}

		protected override bool OnQueryClose(CloseReason reason) {
			timer.Dispose();
			worldState.Dispose();
			device.Dispose();
			device = null;
			context.Dispose();
			return true;
		}

		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PhysicsEngine));
			this.SuspendLayout();
			// 
			// PhysicsEngine
			// 
			this.ClientSize = new System.Drawing.Size(722, 456);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PhysicsEngine";
			this.Text = "PhysicsEngine";
			this.ResumeLayout(false);

		}
	}
}