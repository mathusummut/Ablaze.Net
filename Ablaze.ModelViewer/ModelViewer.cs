using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Graphics;
using System.Graphics.OGL;
using System.Graphics.Models;
using System.Graphics.Models.Parsers;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Dispatch;
using System.Windows.Forms;

namespace Ablaze.ModelViewer {
	/// <summary>
	/// The main window of the model viewer.
	/// </summary>
	public class ModelViewer : GraphicsForm {
		private static string Parameters;
		private UIAnimationHandler processUpdates;
		private WaitCallback renderUpdates;
		private FieldOrProperty currentAngleXProperty, currentAngleYProperty, currentViewTargetProperty, currentDistanceProperty, currentViewDirectionProperty;
		private Vector3 currentViewDirection = new Vector3(1f, 0.75f, 1f), viewDirection = new Vector3(1f, 0.75f, 1f), currentViewTarget, viewTarget, oldViewDirection, oldViewTarget, bounds;
		private Scene Scene = new Scene();
		private float currentAngleX = 0f, angleX, currentAngleY = 0f, angleY, angleZ, oldAngleX, oldAngleY;
		private float max, viewDistance = 0.1f, currentDistance = 0.1f;
		private Point startPos, offset;
		private StyledMenuStrip menuStrip;
		private StyledItem fileMenuItem, openMenuItem, changeTextureMenuItem, heightMapGeneratorMenuItem, saveAsMenuItem;
		private bool isLeftArrowDown, isRightArrowDown, isUpArrowDown, isDownArrowDown, isLeftMouseDown, isMiddleMouseDown, isRightMouseDown, loading, saving;
		private StyledLabel statusLabel;
		private int count, triangleCount;
		private HeightMapDialog childDialog;
		private FilePrompt filePrompt = new FilePrompt();

		/// <summary>
		/// Initializes a ModelViewer instance.
		/// </summary>
		public ModelViewer() {
			StyledMessageBox.Show("A C# application for checking if websites are online or not.\nClick \"Add New...\" to add a website to the list.\nClick on the website to check status and enable/disable ping.\nPinging should only be done when absolutely necessary in order to get more accurate results.\nRight-click the website to remove from list.\nDouble-click the website to open it.", "Help", true, MessageBoxButtons.OK, MessageBoxIcon.Information);
			UIAnimator.SharedAnimator.UpdateOnThreadPool = false;
			InitializeComponent();
			try {
				Icon = System.Properties.Resources.Ablaze;
			} catch {
			}
			childDialog = new HeightMapDialog();
			childDialog.ComponentLoaded += ChildDialog_ComponentLoaded;
			EnableFullscreenOnAltEnter = true;
			ReduceGdiCpuUsage = true;
			IsGdiEnabled = true;
			int msaa = 0;
			try {
				Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
				if (FileUtils.FileExists("config.hjson")) {
					string config = FileUtils.ReadAllText("config.hjson");
					IDictionary<string, JsonObject> values = HjsonValue.Parse(config);
					JsonObject value;
					if (values.TryGetValue(nameof(msaa), out value))
						msaa = (int) value;
				}
			} catch {
			}
			BackColor = Color.MidnightBlue;
			InitializeGL(true, new GraphicsMode(ColorFormat.Bit32, 24, 8, msaa, ColorFormat.Empty, true, false), new MajorMinorVersion(1));
			statusLabel = new StyledLabel() {
				BackColor = Color.Transparent,
				Blur = 2,
				Font = new Font("Cambria", 13f, FontStyle.Regular, GraphicsUnit.Point, 0),
				ForeColor = Color.White,
				LineSpacingMultiplier = 0.25f,
				Name = "Status",
				Outline = Color.Transparent,
				ReduceCaching = true,
				PixelAlignment = PixelOffsetMode.None,
				RenderShadow = true,
				ShadowOpacity = 1.35f,
				Bounds = new Rectangle(10, 10, 320, 145),
				Text = "Loading..."
			};
			GdiControls.Add(statusLabel);
			UpdateInterval = 6;
			StyleRenderer renderer = (StyleRenderer) menuStrip.Renderer;
			renderer.SuppressColorChecking = true;
			renderer.SuppressFunctionCallOnRefresh = true;
			renderer.Border = Color.FromArgb(125, Color.SteelBlue);
			renderer.NormalInnerBorderColor = renderer.Border;
			renderer.HoverInnerBorderColor = renderer.Border;
			renderer.NormalBackgroundTop = renderer.Border;
			renderer.NormalBackgroundBottom = ImageLib.ChangeLightness(renderer.Border, -60);
			renderer.HoverBackgroundTop = renderer.NormalBackgroundTop;
			renderer.HoverBackgroundBottom = renderer.NormalBackgroundBottom;
			renderer.PressedBackgroundTop = renderer.NormalBackgroundTop;
			renderer.PressedBackgroundBottom = renderer.NormalBackgroundBottom;
			renderer.SuppressColorChecking = false;
			renderer.SuppressFunctionCallOnRefresh = false;
			fileMenuItem.Renderer = new StyleRenderer(renderer);
			fileMenuItem.Renderer.SuppressColorChecking = true;
			fileMenuItem.Renderer.SuppressFunctionCallOnRefresh = true;
			fileMenuItem.Renderer.Border = Color.Transparent;
			fileMenuItem.Renderer.NormalInnerBorderColor = Color.Transparent;
			fileMenuItem.Renderer.HoverInnerBorderColor = Color.Transparent;
			fileMenuItem.Renderer.NormalBackgroundTop = Color.Transparent;
			fileMenuItem.Renderer.NormalBackgroundBottom = Color.Transparent;
			fileMenuItem.Renderer.HoverBackgroundTop = ImageLib.ChangeLightness(renderer.NormalBackgroundTop, 30);
			fileMenuItem.Renderer.HoverBackgroundBottom = ImageLib.ChangeLightness(renderer.NormalBackgroundBottom, 30);
			fileMenuItem.Renderer.PressedBackgroundTop = ImageLib.ChangeLightness(renderer.NormalBackgroundTop, -40);
			fileMenuItem.Renderer.PressedBackgroundBottom = ImageLib.ChangeLightness(renderer.NormalBackgroundBottom, -40);
			fileMenuItem.Renderer.SuppressColorChecking = false;
			fileMenuItem.Renderer.SuppressFunctionCallOnRefresh = false;
			menuStrip.PaintBackground += MenuStrip_PaintBackground;
			fileMenuItem.ItemRenderer.SuppressColorChecking = true;
			fileMenuItem.ItemRenderer.SuppressFunctionCallOnRefresh = true;
			fileMenuItem.ItemRenderer.Border = Color.FromArgb(150, Color.SteelBlue);
			fileMenuItem.ItemRenderer.NormalInnerBorderColor = Color.FromArgb(100, Color.SteelBlue);
			fileMenuItem.ItemRenderer.HoverInnerBorderColor = Color.FromArgb(200, Color.LightSteelBlue);
			fileMenuItem.ItemRenderer.NormalBackgroundTop = Color.FromArgb(100, Color.SteelBlue);
			fileMenuItem.ItemRenderer.NormalBackgroundBottom = ImageLib.ChangeLightness(fileMenuItem.ItemRenderer.NormalBackgroundTop, -60);
			fileMenuItem.ItemRenderer.HoverBackgroundTop = Color.FromArgb(200, Color.LightSteelBlue);
			fileMenuItem.ItemRenderer.HoverBackgroundBottom = ImageLib.ChangeLightness(fileMenuItem.ItemRenderer.HoverBackgroundTop, -60);
			fileMenuItem.ItemRenderer.PressedBackgroundTop = Color.FromArgb(100, ImageLib.ChangeLightness(Color.SteelBlue, -40));
			fileMenuItem.ItemRenderer.PressedBackgroundBottom = ImageLib.ChangeLightness(fileMenuItem.ItemRenderer.PressedBackgroundTop, -60);
			fileMenuItem.ItemRenderer.SuppressColorChecking = false;
			fileMenuItem.ItemRenderer.SuppressFunctionCallOnRefresh = false;
			fileMenuItem.UpdateItemRenderer();
			if (fileMenuItem.ContextMenu != null)
				fileMenuItem.ContextMenu.PaintBackground += ContextMenu_PaintBackground;
			DragEnter += ModelViewer_DragEnter;
			DragDrop += ModelViewer_DragDrop;
			currentAngleXProperty = new FieldOrProperty(nameof(currentAngleX), this);
			currentAngleYProperty = new FieldOrProperty(nameof(currentAngleY), this);
			currentViewTargetProperty = new FieldOrProperty(nameof(currentViewTarget), this);
			currentDistanceProperty = new FieldOrProperty(nameof(currentDistance), this);
			currentViewDirectionProperty = new FieldOrProperty(nameof(currentViewDirection), this);
			renderUpdates = RenderUpdates;
			IndexBuffer.KeepBuffer = true;
			MeshComponent.KeepBuffer = true;
			processUpdates = ProcessUpdates;
			MouseDown += ModelViewer_MouseDown;
			PhysicalMouseMove += ModelViewer_PhysicalMouseMove;
			MouseUp += ModelViewer_MouseUp;
			MouseWheel += ModelViewer_MouseWheel;
		}

		private static void ModelViewer_DragEnter(object sender, DragEventArgs e) {
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
				e.Effect = DragDropEffects.Copy;
		}

		private void ModelViewer_DragDrop(object sender, DragEventArgs e) {
			string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
			if (!(files == null || files.Length == 0))
				Open(files);
		}

		/// <summary>
		/// Called when the OpenGL context is initialized.
		/// </summary>
		protected override void OnGLInitialized() {
			base.OnGLInitialized();
			//if (Extensions.IsAeroEnabled && EnableAeroBlur)
			//	GL.ClearColor(Color.FromArgb(128, BackColor));
			MeshComponent.SetupGLEnvironment();
		}

		/// <summary>
		/// Called when the window fade-in animation has completed.
		/// </summary>
		protected override void OnFadeInCompleted() {
			base.OnFadeInCompleted();
			UIAnimator.SharedAnimator.Interval = 6;
		}

		private void ContextMenu_PaintBackground(object sender, PaintEventArgs e) {
			Point offset = (menuStrip.Location + new Size(0, menuStrip.Height)) + (Size) e.ClipRectangle.Location;
			Bitmap bitmap;
			lock (BorderSyncLock)
				bitmap = Border.FastCopy();
			using (PixelWorker wrapper = PixelWorker.FromImage(bitmap, false, true, false)) {
				wrapper.ApplyFunctionToAllPixels(delegate (int i, BgraColor color) {
					return ImageLib.ChangeLightness(color, -60);
				});
			}
			using (TextureBrush brush = new TextureBrush(bitmap)) {
				if (!offset.IsEmpty)
					brush.TranslateTransform(-offset.X, -offset.Y);
				e.Graphics.FillRectangle(brush, e.ClipRectangle);
			}
			bitmap.Dispose();
		}

		private void MenuStrip_PaintBackground(object sender, PaintEventArgs e) {
			Point offset = menuStrip.Location + (Size) e.ClipRectangle.Location;
			lock (BorderSyncLock) {
				using (TextureBrush brush = new TextureBrush(Border)) {
					if (!offset.IsEmpty)
						brush.TranslateTransform(-offset.X, -offset.Y);
					e.Graphics.FillRectangle(brush, new Rectangle(Point.Empty, menuStrip.Size));
				}
			}
		}

		/// <summary>
		/// Called when the update timer has ticked.
		/// </summary>
		/// <param name="elapsedMilliseconds">The milliseconds elapsed since the last frame.</param>
		protected override void OnUpdate(double elapsedMilliseconds) {
			float elapsed = (float) elapsedMilliseconds;
			if (isLeftArrowDown || isRightArrowDown || isUpArrowDown || isDownArrowDown) {
				if (isLeftArrowDown)
					angleZ -= 0.002f * elapsed;
				else if (isRightArrowDown)
					angleZ += 0.002f * elapsed;
				if (isUpArrowDown || isDownArrowDown) {
					if (isUpArrowDown)
						currentAngleX -= 0.002f * elapsed;
					if (isDownArrowDown)
						currentAngleX += 0.002f * elapsed;
					angleX = currentAngleX;
				}
				RenderUpdates(null);
			} else
				UpdateTimerRunning = false;
		}

		private void ChangeTextureMenuItem_Click(object sender, EventArgs e) {
			if (Scene.Count == 0)
				StyledMessageBox.Show("No models have been loaded yet.", "Message");
			else {
				string[] textures = null;
				filePrompt.Open = true;
				filePrompt.Multiselect = true;
				filePrompt.AllFilesString = "All Image Types";
				filePrompt.Filter = string.Empty;
				filePrompt.Title = "Choose the new texture(s) to use...";
				if (MessageLoop.ShowDialog(filePrompt, false) == DialogResult.OK) {
					textures = filePrompt.FileNames;
					if (textures == null) {
						Scene.Textures = null;
						InvalidateGL(false);
					} else {
						try {
							ITexture[] loadedTextures = Texture2D.ToTextures(textures);
							Scene.Textures = loadedTextures;
							InvalidateGL(false);
						} catch (Exception ex) {
							StyledMessageBox.Show("An error occurred while loading the selected textures:\n" + ex.Message, "Message", true, MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
					}
				}
			}
		}

		private void ChildDialog_ComponentLoaded(MeshComponent obj) {
			if (Scene.Count != 0)
				InvokeOnGLThreadAsync(new InvocationData(Dispose3DModel, new List<IModel>(Scene)));
			Model model = new Model(obj);
			model.Cull = false;
			Scene.Add(model);
			UpdateModelStats();
			viewDistance = max * 0.8f;
			UIAnimator.SharedAnimator.Animate(currentDistanceProperty, viewDistance, 0.25f, 0.0005f, true, processUpdates, false);
		}

		private void HeightMapGeneratorMenuItem_Click(object sender, EventArgs e) {
			childDialog.Show();
		}

		private void OpenMenuItem_Click(object sender, EventArgs e) {
			string[] filenames = null;
			filePrompt.Open = true;
			filePrompt.Multiselect = true;
			filePrompt.AllFilesString = "All Files";
			filePrompt.Filter = "3D Model|*.m3d;*.obj";
			filePrompt.Title = "Choose file(s) to open...";
			if (MessageLoop.ShowDialog(filePrompt, false) == DialogResult.OK)
				filenames = filePrompt.FileNames;
			if (filenames != null)
				Open(filenames);
		}

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		private void Open(params string[] meshes) {
			ThreadPool.UnsafeQueueUserWorkItem(LoadModel, meshes);
		}

		private void LoadModel(object param) {
			if (Scene.Count != 0)
				InvokeOnGLThreadAsync(new InvocationData(Dispose3DModel, new List<IModel>(Scene)));
			loading = true;
			statusLabel.Text = "Loading...";
			Rectangle rect = statusLabel.Bounds;
			rect.Offset(ViewPortLocation);
			InvalidateGdi(GdiRenderMode.GdiAsync, rect);
			Exception e = null;
			string[] meshes = (string[]) param;
			float zCount = 0f;
			ITexture loaded;
			Vector2 size;
			string substring;
			foreach (string mesh in meshes) {
				try {
					if (mesh.Length >= 4 && mesh.Substring(mesh.Length - 4) == ".mtl") {
						substring = mesh.Substring(0, mesh.Length - 4) + ".obj";
						if (FileUtils.FileExists(substring))
							Scene.AddRange(ModelParser.Parse(substring, null as string[]));
						else
							Scene.AddRange(ModelParser.Parse(mesh, null as string[]));
					} else
						Scene.AddRange(ModelParser.Parse(mesh, null as string[]));
				} catch (Exception ex) {
					try {
						loaded = TextureParser.Parse(mesh)[0];
						size = ((Texture2D) loaded).BitmapSize.ToVector2();
						Scene.Add(new Model(Mesh2D.MeshFromTexture(loaded, new Vector3(-size.X * 0.5f, -size.Y * 0.5f, zCount), size, true, false)));
						zCount += 1000f;
					} catch {
						if (e == null)
							e = ex;
					}
				}
			}
			Scene.Cull = false;
			if (e == null) {
				UpdateModelStats();
				viewDistance = max * 0.8f;
				UIAnimator.SharedAnimator.Animate(currentDistanceProperty, viewDistance, 0.25f, 0.0005f, true, processUpdates, false);
				loading = false;
				RenderUpdates(null);
			} else {
				loading = false;
				StyledMessageBox.Show("An error occurred while loading the selected mesh:\n" + e.Message, "Message", true, MessageBoxButtons.OK, MessageBoxIcon.Error);
				InvalidateGdi(GdiRenderMode.GdiAsync);
			}
		}

		private void SaveMenuItem_Click(object sender, EventArgs e) {
			filePrompt.Open = false;
			filePrompt.Multiselect = true;
			filePrompt.AllFilesString = "All Files";
			filePrompt.Filter = "Mesh 3D Model|*.m3d|WaveFront Obj 3D Mesh|*.obj";
			filePrompt.FileName = "Model.m3d";
			filePrompt.Title = "Choose where to save mesh...";
			if (MessageLoop.ShowDialog(filePrompt, false) == DialogResult.OK) {
				saving = true;
				ThreadPool.UnsafeQueueUserWorkItem(SaveItem, filePrompt.FileName);
			}
		}

		private void SaveItem(object param) {
			string fileName = (string) param;
			try {
				using (BufferedStream meshStream = FileUtils.LoadFileBuffered(fileName, FileUtils.FileExists(fileName) ? FileMode.Truncate : FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read, true)) {
					ModelParser.Save(Scene, meshStream, Path.GetExtension(fileName));
					saving = false;
					StyledMessageBox.Show("Saving was successful!", "Info", true, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, null, 4000);
				}
			} catch (Exception ex) {
				saving = false;
				StyledMessageBox.Show("An error occurred while saving the mesh:\n" + ex.Message, "Message", true, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// Called when the window is shown.
		/// </summary>
		/// <param name="e">Ignored.</param>
		protected override void OnShown(EventArgs e) {
			base.OnShown(e);
			if (Parameters == null)
				ThreadPool.UnsafeQueueUserWorkItem(renderUpdates, null);
			else {
				Open(Parameters);
				Parameters = null;
			}
		}

		private void ModelViewer_MouseDown(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Middle || e.Button == MouseButtons.Right) {
				if (e.Button == MouseButtons.Left)
					isLeftMouseDown = true;
				if (e.Button == MouseButtons.Middle)
					isMiddleMouseDown = true;
				if (e.Button == MouseButtons.Right)
					isRightMouseDown = true;
				Size viewSize = ViewSize;
				if (new Rectangle(0, menuStrip.Height, viewSize.Width, viewSize.Height - menuStrip.Height).Contains(e.Location)) {
					CursorVisible = false;
					startPos = Cursor.Position;
					offset = Point.Empty;
					oldAngleX = angleX;
					oldAngleY = angleY;
					oldViewTarget = viewTarget;
					oldViewDirection = viewDirection;
					Capture = true;
				}
			}
		}

		private void ModelViewer_PhysicalMouseMove(object sender, MouseEventArgs e) {
			if (isLeftMouseDown || isMiddleMouseDown || isRightMouseDown) {
				Rectangle bounds = ViewPort;
				bounds.Location += (Size) Location;
				Point pos = PointFromViewPortToScreen(e.Location);
				Point oldPos = pos;
				if (pos.X <= bounds.X)
					pos.X += bounds.Width - 2;
				else if (pos.X >= bounds.Right - 1)
					pos.X -= bounds.Width - 2;
				if (pos.Y <= bounds.Y)
					pos.Y += bounds.Height - 2;
				else if (pos.Y >= bounds.Bottom - 1)
					pos.Y -= bounds.Height - 2;
				if (pos != oldPos) {
					offset = new Point(offset.X - (pos.X - oldPos.X), offset.Y - (pos.Y - oldPos.Y));
					Cursor.Position = pos;
					MouseListener.ConsumeEvent = true;
				}
				if (isLeftMouseDown) {
					angleY = oldAngleY + (pos.X + offset.X - startPos.X) * 0.003f;
					angleX = oldAngleX + (pos.Y + offset.Y - startPos.Y) * 0.003f;
					UIAnimator.SharedAnimator.Animate(currentAngleXProperty, angleX, 0.5f, 0.0005f, true, processUpdates, false);
					UIAnimator.SharedAnimator.Animate(currentAngleYProperty, angleY, 0.5f, 0.0005f, true, processUpdates, false);
				} else if (isMiddleMouseDown) {
					viewTarget = oldViewTarget + new Vector3(((startPos.X - pos.X) - offset.X) * 0.1f, (pos.Y + offset.Y - startPos.Y) * 0.1f, 0f);
					UIAnimator.SharedAnimator.Animate(currentViewTargetProperty, viewTarget, 0.5f, 0.0005f, true, processUpdates, false);
				} else if (isRightMouseDown) {
					viewDirection = oldViewDirection + new Vector3(((startPos.X - pos.X) - offset.X) * 0.005f, (pos.Y + offset.Y - startPos.Y) * 0.005f, 0f);
					UIAnimator.SharedAnimator.Animate(currentViewDirectionProperty, viewDirection, 0.5f, 0.0005f, true, processUpdates, false);
				}
				ThreadPool.UnsafeQueueUserWorkItem(renderUpdates, null);
			}
		}

		private void ModelViewer_MouseUp(object sender, MouseEventArgs e) {
			Capture = false;
			bool mouseUp = false;
			if (isLeftMouseDown && e.Button == MouseButtons.Left) {
				isLeftMouseDown = false;
				mouseUp = true;
			}
			if (isMiddleMouseDown && e.Button == MouseButtons.Middle) {
				isMiddleMouseDown = false;
				mouseUp = true;
			}
			if (isRightMouseDown && e.Button == MouseButtons.Right) {
				isRightMouseDown = false;
				mouseUp = true;
			}
			if (mouseUp && !(isLeftMouseDown || isMiddleMouseDown || isRightMouseDown)) {
				Cursor.Position = startPos;
				CursorVisible = true;
			}
		}

		private void ModelViewer_MouseWheel(object sender, MouseEventArgs e) {
			viewDistance -= e.Delta * 0.0009f * viewDistance;
			if (viewDistance <= 0.1f)
				viewDistance = 0.1f;
			UIAnimator.SharedAnimator.Animate(currentDistanceProperty, viewDistance, 0.25f, 0.0005f, true, processUpdates, false);
		}

		/// <summary>
		/// Called when a key or key combination has been pressed.
		/// </summary>
		/// <param name="msg">The corresponding window message.</param>
		/// <param name="keyData">The keys that were pressed.</param>
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
			if (menuStrip.ProcessKeys(keyData))
				return true;
			else
				return base.ProcessCmdKey(ref msg, keyData);
		}

		/// <summary>
		///  Called when a key is pressed.
		/// </summary>
		/// <param name="e">The key that was pressed.</param>
		protected override void OnKeyDown(KeyEventArgs e) {
			base.OnKeyDown(e);
			switch (e.KeyCode) {
				case Keys.Left:
					isLeftArrowDown = true;
					UpdateTimerRunning = true;
					break;
				case Keys.Right:
					isRightArrowDown = true;
					UpdateTimerRunning = true;
					break;
				case Keys.Up:
					isUpArrowDown = true;
					UpdateTimerRunning = true;
					break;
				case Keys.Down:
					isDownArrowDown = true;
					UpdateTimerRunning = true;
					break;
				case Keys.T:
					ShowInTaskbar = !ShowInTaskbar;
					break;
			}
		}

		/// <summary>
		///  Called when a key is released.
		/// </summary>
		/// <param name="e">The key that was released.</param>
		protected override void OnKeyUp(KeyEventArgs e) {
			base.OnKeyUp(e);
			switch (e.KeyCode) {
				case Keys.Left:
					isLeftArrowDown = false;
					break;
				case Keys.Right:
					isRightArrowDown = false;
					break;
				case Keys.Up:
					isUpArrowDown = false;
					break;
				case Keys.Down:
					isDownArrowDown = false;
					break;
				case Keys.Escape:
					Capture = false;
					break;
				default:
					if (e.Control && e.KeyCode == Keys.R) {
						viewDirection = new Vector3(1F, 0.75F, 1F);
						UIAnimator.SharedAnimator.Animate(currentViewDirectionProperty, viewDirection, 0.25f, 0.0005f, true, processUpdates, false);
						viewTarget = Vector3.Zero;
						UIAnimator.SharedAnimator.Animate(currentViewTargetProperty, viewTarget, 0.25f, 0.0005f, true, processUpdates, false);
						angleX = 0f;
						UIAnimator.SharedAnimator.Animate(currentAngleXProperty, angleX, 0.25f, 0.0005f, true, processUpdates, false);
						angleY = 0f;
						UIAnimator.SharedAnimator.Animate(currentAngleYProperty, angleY, 0.25f, 0.0005f, true, processUpdates, false);
						angleZ = 0f;
						viewDistance = Math.Max(max * 0.8f, 0.1f);
						UIAnimator.SharedAnimator.Animate(currentDistanceProperty, viewDistance, 0.25f, 0.0005f, true, processUpdates, false);
					}
					break;
			}
		}

		private bool ProcessUpdates(AnimationInfo state) {
			RenderUpdates(null);
			return !IsDisposed;
		}

		private void UpdateModelStats() {
			count = Scene.Vertices;
			triangleCount = Scene.Triangles;
			Scene.Optimization = BufferUsageHint.StaticDraw;
			bounds = Scene.Bounds;
			max = Math.Max(bounds.GetMaxComponent(), 0.05f);
		}

		private void RenderUpdates(object state) {
			Vector3 up = Vector3.UnitY;
			Vector3 eye = currentViewDirection * currentDistance;
			Matrix4 transformation = Matrix4.CreateRotationZ(angleZ) * Matrix4.CreateRotationY(currentAngleY) * Matrix4.CreateRotationX(currentAngleX);
			Scene.Camera = transformation * Matrix4.LookAt(ref eye, ref currentViewTarget, ref up);
			//Scene.Light.Position = /*eye*/ new Vector3(20f, 0f, 0f);
			//Scene.Light.PointLight = true;
			//Fade light with distance
			InvalidateGL(false);
			if (!loading || IsClosing || IsDisposed) {
				string text = string.Format("Distance from origin: {0}\v\nAngle: {{{1}, {2}, {3}}}\v\nBounds: {{{4}, {5}, {6}}}\v\nVertices: {7}\v\nTriangles: {8}",
					Math.Round(currentDistance, 3),
					Math.Round(currentAngleX, 3),
					Math.Round(currentAngleY, 3),
					Math.Round(angleZ, 3),
					Math.Round(bounds.X, 3),
					Math.Round(bounds.Y, 3),
					Math.Round(bounds.Z, 3),
					count,
					triangleCount);
				if (statusLabel.Text != text)
					statusLabel.Text = text;
			}
		}

		/// <summary>
		/// Called whenever a GL render takes place.
		/// </summary>
		protected override void OnPaintGL() {
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			MeshComponent.Setup3D(GLViewport.Size.ToVector2(), ref Scene.Camera, (float) Math.Max((Math.Pow(currentDistance * 0.5, 0.3)) * 0.75 - 1.5, 0.001));
			Scene.Render();
		}

		/// <summary>
		/// Called when the window is being closed, but the context is still alive. Place GL-related cleanup code here.
		/// </summary>
		protected override void OnUnload() {
			Dispose3DModel(new List<IModel>(Scene));
			base.OnUnload();
		}

		/// <summary>
		/// Disposes of the 3D model that is loaded.
		/// </summary>
		public static object Dispose3DModel(object param) {
			foreach (IModel model in (IEnumerable<IModel>) param)
				model.Dispose();
			return null;
		}

		/// <summary>
		/// Whether to close.
		/// </summary>
		/// <param name="reason">The reason to close.</param>
		protected override bool OnQueryClose(CloseReason reason) {
			return !saving;
		}

		/// <summary>
		/// Called when the window is being disposed.
		/// </summary>
		/// <param name="e">Whether managed resources are about to be disposed.</param>
		protected override void OnDisposing(DisposeFormEventArgs e) {
			base.OnDisposing(e);
			if (e.DisposeMode == DisposeOptions.FullDisposal) {
				filePrompt.Dispose();
				childDialog.Dispose();
			}
		}

		/// <summary>
		/// The entry point of the application.
		/// </summary>
		[STAThread]
		public static void Main(string[] args) {
#if NET45
			try {
				ProfileOptimization.SetProfileRoot(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));
				ProfileOptimization.StartProfile("LoadCache.prf");
			} catch {
			}
#endif
			if (args.Length != 0)
				Parameters = args[0].Trim();
			MessageLoop.Run(new ModelViewer());
		}

		/// <summary>
		/// Initializes the window and its controls.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModelViewer));
			this.menuStrip = new System.Windows.Forms.StyledMenuStrip();
			this.fileMenuItem = new System.Windows.Forms.StyledItem();
			this.openMenuItem = new System.Windows.Forms.StyledItem();
			this.changeTextureMenuItem = new System.Windows.Forms.StyledItem();
			this.heightMapGeneratorMenuItem = new System.Windows.Forms.StyledItem();
			this.saveAsMenuItem = new System.Windows.Forms.StyledItem();
			this.menuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip
			// 
			this.menuStrip.AlignGradientWorkaround = true;
			this.menuStrip.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenuItem});
			this.menuStrip.Location = new System.Drawing.Point(4, 31);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new System.Drawing.Size(641, 24);
			this.menuStrip.TabIndex = 9;
			this.menuStrip.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// fileMenuItem
			// 
			this.fileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.openMenuItem,
			this.changeTextureMenuItem,
			this.heightMapGeneratorMenuItem,
			this.saveAsMenuItem});
			this.fileMenuItem.ForeColor = System.Drawing.Color.White;
			this.fileMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.fileMenuItem.Name = "StyledItem";
			this.fileMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.fileMenuItem.Size = new System.Drawing.Size(34, 25);
			this.fileMenuItem.Text = "File";
			this.fileMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// openMenuItem
			// 
			this.openMenuItem.ForeColor = System.Drawing.Color.White;
			this.openMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.openMenuItem.Name = "StyledItem";
			this.openMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.openMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.openMenuItem.Size = new System.Drawing.Size(202, 25);
			this.openMenuItem.Text = "Open                                 (Ctrl+O)";
			this.openMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.openMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.openMenuItem.Click += new System.EventHandler(this.OpenMenuItem_Click);
			// 
			// changeTextureMenuItem
			// 
			this.changeTextureMenuItem.ForeColor = System.Drawing.Color.White;
			this.changeTextureMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.changeTextureMenuItem.Name = "StyledItem";
			this.changeTextureMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.changeTextureMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
			this.changeTextureMenuItem.Size = new System.Drawing.Size(196, 25);
			this.changeTextureMenuItem.Text = "Change Texture(s)            (Ctrl+T)";
			this.changeTextureMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.changeTextureMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.changeTextureMenuItem.Click += new System.EventHandler(this.ChangeTextureMenuItem_Click);
			// 
			// heightMapGeneratorMenuItem
			// 
			this.heightMapGeneratorMenuItem.ForeColor = System.Drawing.Color.White;
			this.heightMapGeneratorMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.heightMapGeneratorMenuItem.Name = "StyledItem";
			this.heightMapGeneratorMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.heightMapGeneratorMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
			this.heightMapGeneratorMenuItem.Size = new System.Drawing.Size(196, 25);
			this.heightMapGeneratorMenuItem.Text = "Height Map Generator    (Ctrl+H)";
			this.heightMapGeneratorMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.heightMapGeneratorMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.heightMapGeneratorMenuItem.Click += new System.EventHandler(this.HeightMapGeneratorMenuItem_Click);
			// 
			// saveAsMenuItem
			// 
			this.saveAsMenuItem.ForeColor = System.Drawing.Color.White;
			this.saveAsMenuItem.MaximumSize = new System.Drawing.Size(0, 0);
			this.saveAsMenuItem.Name = "StyledItem";
			this.saveAsMenuItem.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.saveAsMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.saveAsMenuItem.Size = new System.Drawing.Size(197, 25);
			this.saveAsMenuItem.Text = "Save As                             (Ctrl+S)";
			this.saveAsMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.saveAsMenuItem.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.saveAsMenuItem.Click += new System.EventHandler(this.SaveMenuItem_Click);
			// 
			// ModelViewer
			// 
			this.AllowDrop = true;
			this.ClientSize = new System.Drawing.Size(649, 516);
			this.Controls.Add(this.menuStrip);
			this.Font = new System.Drawing.Font("Calibri Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.GdiLocation = new System.Drawing.Point(0, 24);
			this.Location = new System.Drawing.Point(200, 200);
			this.MainMenuStrip = this.menuStrip;
			this.Margin = new System.Windows.Forms.Padding(0);
			this.MinimumSize = new System.Drawing.Size(400, 300);
			this.Name = "ModelViewer";
			this.Text = "Model Viewer";
			this.Controls.SetChildIndex(this.menuStrip, 0);
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.ResumeLayout(false);

		}
	}
}