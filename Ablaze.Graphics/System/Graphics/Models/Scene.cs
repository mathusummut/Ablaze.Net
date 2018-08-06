using System.Collections.Generic;
using System.Numerics;

namespace System.Graphics.Models {
	/// <summary>
	/// Represents a graphical scene composed of models and additional configurations such as camera and lighting parameters.
	/// </summary>
	public class Scene : Model {
		/// <summary>
		/// The default camera matrix.
		/// </summary>
		public static readonly Matrix4 DefaultCamera = Matrix4.LookAt(new Vector3(0f, 0f, 1f), Vector3.Zero, Vector3.UnitY);
		/// <summary>
		/// The camera matrix for the scene (the same as CameraMatrix).
		/// </summary>
		public Matrix4 Camera;
		/// <summary>
		/// The lighting parameters for the scene (same as LightSource).
		/// </summary>
		public Light Light = new Light(Vector3.Zero, Light.DefaultLightHue);

		/// <summary>
		/// Gets or sets the lighting parameters for the scene.
		/// </summary>
		public Light LightSource {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets whether lighting is enabled for the scene.
		/// </summary>
		public bool LightingEnabled {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the camera matrix for the scene.
		/// </summary>
		public Matrix4 CameraMatrix {
			get {
				return Camera;
			}
			set {
				Camera = value;
			}
		}

		/// <summary>
		/// Creates a new empty scene.
		/// </summary>
		public Scene() {
			RestoreDefaults();
		}

		/// <summary>
		/// Creates a new scene from the specified components.
		/// </summary>
		/// <param name="components">The components that are to make up the scene.</param>
		public Scene(IEnumerable<IModel> components) : base(components) {
			RestoreDefaults();
		}

		/// <summary>
		/// Creates a new scene from the specified components.
		/// </summary>
		/// <param name="components">The components that are to make up the scene.</param>
		public Scene(IEnumerable<Model> components) : base(components) {
			RestoreDefaults();
		}

		/// <summary>
		/// Creates a new scene from the specified components.
		/// </summary>
		/// <param name="components">The components that are to make up the scene.</param>
		public Scene(IEnumerable<MeshComponent> components) : base(components) {
			RestoreDefaults();
		}

		/// <summary>
		/// Creates a new scene from the specified components.
		/// </summary>
		/// <param name="components">The components that are to make up the scene.</param>
		public Scene(params IModel[] components) : base(components) {
			RestoreDefaults();
		}

		/// <summary>
		/// Creates a new model structure from the specified components.
		/// </summary>
		/// <param name="components">The components that are to make up the model structure.</param>
		public Scene(params Model[] components) : base(components) {
			RestoreDefaults();
		}

		/// <summary>
		/// Creates a new scene from the specified components
		/// </summary>
		/// <param name="components">The components that are to make up the scene</param>
		public Scene(params MeshComponent[] components) : base(components) {
			RestoreDefaults();
		}

		/// <summary>
		/// Clones the specified scene
		/// </summary>
		/// <param name="model">The scene to clone</param>
		public Scene(Scene model) : base(model) {
			RestoreDefaults();
		}

		/// <summary>
		/// Restore the scene parameters to the default
		/// </summary>
		public void RestoreDefaults() {
			Camera = DefaultCamera;
			LightingEnabled = true;
			Light.Hue = Light.DefaultLightHue;
		}

		/// <summary>
		/// Prepares the scene for loading (called just before LoadScene())
		/// </summary>
		public virtual void PrepareScene() {
		}

		/// <summary>
		/// Loads the scene. If this code is auto-generated, please do not modify it by hand unless you know what you are doing
		/// </summary>
		public virtual void LoadScene() {
		}

		/// <summary>
		/// Called after the scene is loaded
		/// </summary>
		public virtual void OnSceneLoaded() {
		}

		/// <summary>
		/// Called *after* the scene is rendered
		/// </summary>
		public virtual void OnRendered() {
		}

		/// <summary>
		/// Renders the scene (must be called on a thread on which an OpenGL context is set up, preferably on the thread on which the model was loaded).
		/// Bind the global shader calling Render().
		/// </summary>
		public override void Render() {
			GlobalShader shader = (GlobalShader) Shader.CurrentShader;
			bool light = LightingEnabled;
			shader.ConfigureLighting(light, ShaderSetMode.SetImmediately);
			if (light) {
				shader.SetUniformValue(GlobalShaderParams.LightHue.ToString(), Light.Hue, ShaderSetMode.SetImmediately);
				shader.SetUniformValue(GlobalShaderParams.LightPosition.ToString(), Light.Position, ShaderSetMode.SetImmediately);
				shader.SetUniformValue(GlobalShaderParams.PointLight.ToString(), Light.PointLight ? 1f : 0f, ShaderSetMode.SetImmediately);
			}
			base.Render();
		}

		/// <summary>
		/// Enables or disables lighting.
		/// </summary>
		/// <param name="enable">Whether to enable of disable lighting.</param>
		/// <param name="mode">The method to use to set the value.</param>
		public static void ConfigureLighting(bool enable, ShaderSetMode mode) {
			GlobalShader shader = (GlobalShader) Shader.CurrentShader;
			shader.ConfigureLighting(enable, mode);
		}
	}
}
