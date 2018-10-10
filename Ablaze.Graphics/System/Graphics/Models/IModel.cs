using System.Numerics;
using System.Graphics.OGL;

namespace System.Graphics.Models {
	/// <summary>
	/// The base class for a 3D model structure.
	/// </summary>
	public interface IModel : ICloneable, IDisposable {
		/// <summary>
		/// Called when a model is about to be rendered.
		/// </summary>
		event Action RenderBegin;
		/// <summary>
		/// Called when a model has been rendered.
		/// </summary>
		event Action RenderEnd;
		/// <summary>
		/// Called when the location of the model has changed.
		/// </summary>
		event Action LocationChanged;
		/// <summary>
		/// Called when the scale of the model has changed.
		/// </summary>
		event Action ScaleChanged;
		/// <summary>
		/// Called when the rotation of the model has changed.
		/// </summary>
		event Action RotationChanged;

		/// <summary>
		/// Gets the number of components in the model.
		/// </summary>
		int Count {
			get;
		}

		/// <summary>
		/// Gets or sets the name of the component.
		/// </summary>
		string Name {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets an object that contains properties relevant to this mesh.
		/// </summary>
		object Tag {
			get;
			set;
		}

		/// <summary>
		/// Gets the number of vertices in the model.
		/// </summary>
		int Vertices {
			get;
		}

		/// <summary>
		/// Gets the number of triangles in the model.
		/// </summary>
		int Triangles {
			get;
		}

		/// <summary>
		/// If true, the backs of mesh components are not rendered.
		/// </summary>
		bool Cull {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the rendering mode of the polygons of the mesh.
		/// </summary>
		MeshMode MeshMode {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the transparency of the mesh material (1 is opaque, 0 is transparent).
		/// </summary>
		float Alpha {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets whether the model is visible (rendered) or not.
		/// </summary>
		bool Visible {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets whether to keep a copy of the vertices in memory even after loading the vertices into GPU memory (allows faster and multithreaded vertex retrieval)
		/// </summary>
		bool KeepCopyInMemory {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the parent model (can be null)
		/// </summary>
		IModel Parent {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the default textures of the components when none is specified
		/// </summary>
		TextureCollection DefaultTextures {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the textures of all components
		/// </summary>
		TextureCollection Textures {
			get;
			set;
		}

		/// <summary>
		/// Gets whether the model is disposed.
		/// </summary>
		bool IsDisposed {
			get;
		}

		/// <summary>
		/// Set to true if you want the vertex buffer to be flushed on next render.
		/// </summary>
		bool FlushBufferOnNextRender {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the type of the model for optimization.
		/// </summary>
		BufferUsageHint Optimization {
			get;
			set;
		}

		/// <summary>
		/// Gets the location of the centre of the model.
		/// </summary>
		Vector3 Center {
			get;
		}

		/// <summary>
		/// Gets or sets the relative location of the model.
		/// </summary>
		Vector3 Location {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the relative scale of the model.
		/// </summary>
		Vector3 Scale {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the relative rotation of the model (angle X, angle Y, angle Z).
		/// </summary>
		Vector3 Rotation {
			get;
			set;
		}

		/// <summary>
		/// Gets the smallest X coordinate.
		/// </summary>
		float MinX {
			get;
		}

		/// <summary>
		/// Gets the smallest Y coordinate.
		/// </summary>
		float MinY {
			get;
		}

		/// <summary>
		/// Gets the smallest Z coordinate.
		/// </summary>
		float MinZ {
			get;
		}

		/// <summary>
		/// Gets the largest X coordinate.
		/// </summary>
		float MaxX {
			get;
		}

		/// <summary>
		/// Gets the largest Y coordinate.
		/// </summary>
		float MaxY {
			get;
		}

		/// <summary>
		/// Gets the largest Z coordinate.
		/// </summary>
		float MaxZ {
			get;
		}

		/// <summary>
		/// Gets the width of the bounding box of the model.
		/// </summary>
		float Width {
			get;
		}

		/// <summary>
		/// Gets the height of the bounding box of the model.
		/// </summary>
		float Height {
			get;
		}

		/// <summary>
		/// Gets the depth of the bounding box of the model.
		/// </summary>
		float Depth {
			get;
		}

		/// <summary>
		/// Gets the bounding box sizes of the model.
		/// </summary>
		Vector3 Bounds {
			get;
		}

		/// <summary>
		/// Translate the model by the specified vector.
		/// </summary>
		/// <param name="vector">The vector to translate at.</param>
		void TranslateMesh(Vector3 vector);

		/// <summary>
		/// Enlarges the model to the specified scale.
		/// </summary>
		/// <param name="scale">The scale to enlarge by.</param>
		/// <param name="centre">The centre to enlarge against.</param>
		void ScaleMesh(float scale, Vector3 centre);

		/// <summary>
		/// Enlarges the model to the specified scale.
		/// </summary>
		/// <param name="scale">Contains the scale of the X, Y and Z dimension individually.</param>
		/// <param name="centre">The centre to enlarge against.</param>
		void ScaleMesh(Vector3 scale, Vector3 centre);

		/// <summary>
		/// Rotates the model.
		/// </summary>
		/// <param name="rotation">Contains the rotation of the X, Y and Z axis individually.</param>
		void RotateMesh(Vector3 rotation);

		/// <summary>
		/// Transforms the model using the specified matrix.
		/// </summary>
		/// <param name="matrix">The matrix to multiply transform vertices with.</param>
		void ApplyMatrixTransformation(ref Matrix4 matrix);

		/// <summary>
		/// Renders the model (must be called on a thread on which an OpenGL context is set up, preferably on the thread on which the model was loaded).
		/// </summary>
		void Render();

		/// <summary>
		/// Renders the model (must be called on a thread on which an OpenGL context is set up, preferably on the thread on which the model was loaded).
		/// </summary>
		/// <param name="nextModel">The next mesh component to interpolate with (can be null)</param>
		void Render(IModel nextModel);

		/// <summary>
		/// Creates a copy of the model.
		/// </summary>
		/// <param name="components">Whether to clone the internal components too.</param>
		IModel Clone(bool components);

		/// <summary>
		/// Disposes of the resources used by the model.
		/// </summary>
		/// <param name="disposeChildren">Whether to dispose of the child components of the model.</param>
		void Dispose(bool disposeChildren);

		/// <summary>
		/// Disposes of the resources used by the model.
		/// </summary>
		/// <param name="disposeChildren">Whether to dispose of the child components of the model.</param>
		/// <param name="removeFromParent">Always set to true. Except in the Dispose() function of the parent, as all child components are removed automatically.</param>
		void Dispose(bool disposeChildren, bool removeFromParent);
	}
}