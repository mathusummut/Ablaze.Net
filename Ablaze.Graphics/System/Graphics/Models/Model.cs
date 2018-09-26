using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Graphics.OGL;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace System.Graphics.Models {
	/// <summary>
	/// Offers methods to manage 3D model structures collectively
	/// </summary>
	public class Model : IModel, IEnumerable<IModel>, IEnumerable {
		/// <summary>
		/// Called when a model is about to be rendered
		/// </summary>
		public event Action RenderBegin;
		/// <summary>
		/// Called when a model has been rendered
		/// </summary>
		public event Action RenderEnd;
		/// <summary>
		/// Called when the location of the model has changed
		/// </summary>
		public event Action LocationChanged;
		/// <summary>
		/// Called when the scale of the model has changed
		/// </summary>
		public event Action ScaleChanged;
		/// <summary>
		/// Called when the rotation of the model has changed
		/// </summary>
		public event Action RotationChanged;
		private IModel parent;
		private Vector3 currentLoc, currentRot, currentScale = Vector3.One;
		private float alpha = 1f;
		private int vertices, triangles;
		/// <summary>
		/// Used to safely iterate or modify the components in this model
		/// </summary>
		protected readonly object SyncRoot = new object();
		/// <summary>
		/// A list of the component structures managed by the model
		/// </summary>
		private List<IModel> components = new List<IModel>();

		/// <summary>
		/// Gets a read-only wrapper for the components in the model
		/// </summary>
		public ReadOnlyCollection<IModel> Components {
			get {
				return components.AsReadOnly();
			}
		}

		/// <summary>
		/// Gets or sets the parent model (can be null)
		/// </summary>
		public IModel Parent {
			get {
				return parent;
			}
			set {
				if (value == parent)
					return;
				if (parent != null)
					parent.Remove(this);
				if (value != null)
					value.Add(this);
				parent = value;
			}
		}

		/// <summary>
		/// Gets or sets the name of the component
		/// </summary>
		public string Name {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets an object that contains properties relevant to this mesh
		/// </summary>
		public object Tag {
			get;
			set;
		}

		/// <summary>
		/// Gets the number of components in the model
		/// </summary>
		public int Count {
			get {
				return components.Count;
			}
		}

		/// <summary>
		/// Gets the mesh at the specified index
		/// </summary>
		/// <param name="index">The index of the mesh to return</param>
		public IModel this[int index] {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				lock (SyncRoot)
					return components[index];
			}
			set {
				lock (SyncRoot) {
					IModel val = components[index];
					if (value == val)
						return;
					vertices -= val.Vertices;
					triangles -= val.Triangles;
					if (value == null)
						components.RemoveAt(index);
					else {
						components[index] = value;
						vertices += value.Vertices;
						triangles += value.Triangles;
					}
				}
			}
		}

		/// <summary>
		/// Gets the number of points in the model
		/// </summary>
		public int Vertices {
			get {
				return vertices;
			}
		}

		/// <summary>
		/// Gets the number of triangles in the model
		/// </summary>
		public int Triangles {
			get {
				return triangles;
			}
		}

		/// <summary>
		/// Whether the model components are visible (rendered) or not
		/// </summary>
		public bool IsVisible {
			get {
				return components.Count == 0 ? true : components[0].IsVisible;
			}
			set {
				lock (SyncRoot) {
					for (int i = 0; i < components.Count; i++)
						components[i].IsVisible = value;
				}
			}
		}

		/// <summary>
		/// If true, the backs of mesh components are not rendered
		/// </summary>
		public bool Cull {
			get {
				return components.Count == 0 ? true : components[0].Cull;
			}
			set {
				lock (SyncRoot) {
					for (int i = 0; i < components.Count; i++)
						components[i].Cull = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets the rendering mode of the polygons of the mesh
		/// </summary>
		public MeshMode MeshMode {
			get {
				return components.Count == 0 ? MeshMode.Normal : components[0].MeshMode;
			}
			set {
				lock (SyncRoot) {
					for (int i = 0; i < components.Count; i++)
						components[i].MeshMode = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets the transparency multiplier of the mesh material (1 is opaque, 0 is transparent)
		/// </summary>
		public float Alpha {
			get {
				return alpha;
			}
			set {
				alpha = value;
			}
		}

		/// <summary>
		/// Gets or sets the default textures of the model components when none is specified
		/// </summary>
		public ITexture[] DefaultTextures {
			get {
				if (components.Count == 0)
					return Texture2D.EmptyTexture;
				List<ITexture> defaultTextures = new List<ITexture>(components.Count);
				lock (SyncRoot) {
					for (int i = 0; i < components.Count; i++)
						defaultTextures.AddRange(components[i].DefaultTextures);
				}
				return defaultTextures.ToArray();
			}
			set {
				if (value == null || value.Length == 0)
					value = Texture2D.EmptyTexture;
				int index = 0;
				MeshComponent model;
				IModel component;
				lock (SyncRoot) {
					for (int i = 0; i < components.Count; i++) {
						component = components[i];
						model = component as MeshComponent;
						if (model == null)
							component.DefaultTextures = value;
						else {
							model.DefaultTextures = new ITexture[] { value[index] };
							if (index != value.Length - 1)
								index++;
						}
					}
				}
			}
		}

		/// <summary>
		/// Sets the textures of all components
		/// </summary>
		public ITexture[] Textures {
			get {
				if (components.Count == 0)
					return Texture2D.EmptyTexture;
				List<ITexture> textures = new List<ITexture>(components.Count);
				lock (SyncRoot) {
					for (int i = 0; i < components.Count; i++)
						textures.AddRange(components[i].Textures);
				}
				return textures.ToArray();
			}
			set {
				if (value == null || value.Length == 0)
					value = components.Count == 0 ? Texture2D.EmptyTexture : components[0].DefaultTextures;
				int index = 0;
				MeshComponent model;
				IModel component;
				lock (SyncRoot) {
					for (int i = 0; i < components.Count; i++) {
						component = components[i];
						model = component as MeshComponent;
						if (model == null)
							component.Textures = value;
						else {
							model.Textures = new ITexture[] { value[index] };
							if (index != value.Length - 1)
								index++;
						}
					}
				}
			}
		}

		/// <summary>
		/// Gets whether the model is disposed
		/// </summary>
		public bool IsDisposed {
			get {
				return components.Count == 0;
			}
		}

		/// <summary>
		/// Set to true if you want the vertex buffer to be flushed on next render
		/// </summary>
		public bool FlushBufferOnNextRender {
			get {
				return components.Count == 0 ? false : components[0].FlushBufferOnNextRender;
			}
			set {
				lock (SyncRoot) {
					for (int i = 0; i < components.Count; i++)
						components[i].FlushBufferOnNextRender = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets the type of the model for optimization
		/// </summary>
		public BufferUsageHint Optimization {
			get {
				return components.Count == 0 ? BufferUsageHint.StaticDraw : components[0].Optimization;
			}
			set {
				lock (SyncRoot) {
					for (int i = 0; i < components.Count; i++)
						components[i].Optimization = value;
				}
			}
		}

		/// <summary>
		/// Gets the location of the centre of the model
		/// </summary>
		public Vector3 Center {
			get {
				if (components.Count == 0)
					return Vector3.Zero;
				Vector3 total = Vector3.Zero;
				lock (SyncRoot) {
					for (int i = 0; i < components.Count; i++)
						total += components[i].Center;
				}
				return total / components.Count;
			}
		}

		/// <summary>
		/// Gets or sets the relative location of the model
		/// </summary>
		public Vector3 Location {
			get {
				return currentLoc;
			}
			set {
				if (value == currentLoc)
					return;
				Vector3 toMove = value - currentLoc;
				currentLoc = value;
				lock (SyncRoot) {
					for (int i = 0; i < components.Count; i++)
						components[i].Location += toMove;
				}
				RaiseLocationChanged();
			}
		}

		/// <summary>
		/// Gets or sets the relative scale of the model
		/// </summary>
		public Vector3 Scale {
			get {
				return currentScale;
			}
			set {
				if (Math.Abs(value.X) <= float.Epsilon)
					value.X = 0.000001f;
				if (Math.Abs(value.Y) <= float.Epsilon)
					value.Y = 0.000001f;
				if (Math.Abs(value.Z) <= float.Epsilon)
					value.Z = 0.000001f;
				if (value == currentScale)
					return;
				Vector3 toScale = Vector3.Divide(value, currentScale);
				currentScale = value;
				lock (SyncRoot) {
					for (int i = 0; i < components.Count; i++)
						components[i].Scale = Vector3.Multiply(components[i].Scale, toScale);
				}
				RaiseScaleChanged();
			}
		}

		/// <summary>
		/// Gets or sets the relative rotation of the model
		/// </summary>
		public Vector3 Rotation {
			get {
				return currentRot;
			}
			set {
				if (value == currentRot)
					return;
				Vector3 toRotate = value - currentRot;
				currentRot = value;
				lock (SyncRoot) {
					for (int i = 0; i < components.Count; i++)
						components[i].Rotation += toRotate;
				}
				RaiseRotationChanged();
			}
		}

		/// <summary>
		/// Gets the smallest X coordinate
		/// </summary>
		public float MinX {
			get {
				float minX = components.Count == 0 ? currentLoc.X : components[0].MinX;
				float temp;
				lock (SyncRoot) {
					for (int i = 1; i < components.Count; i++) {
						temp = components[i].MinX;
						if (temp < minX)
							minX = temp;
					}
				}
				return minX;
			}
		}

		/// <summary>
		/// Gets the smallest Y coordinate
		/// </summary>
		public float MinY {
			get {
				float minY = components.Count == 0 ? currentLoc.Y : components[0].MinY;
				float temp;
				lock (SyncRoot) {
					for (int i = 1; i < components.Count; i++) {
						temp = components[i].MinY;
						if (temp < minY)
							minY = temp;
					}
				}
				return minY;
			}
		}

		/// <summary>
		/// Gets the smallest Z coordinate
		/// </summary>
		public float MinZ {
			get {
				float minZ = components.Count == 0 ? currentLoc.Z : components[0].MinZ;
				float temp;
				lock (SyncRoot) {
					for (int i = 1; i < components.Count; i++) {
						temp = components[i].MinZ;
						if (temp < minZ)
							minZ = temp;
					}
				}
				return minZ;
			}
		}

		/// <summary>
		/// Gets the largest X coordinate
		/// </summary>
		public float MaxX {
			get {
				float maxX = components.Count == 0 ? currentLoc.X : components[0].MaxX;
				float temp;
				lock (SyncRoot) {
					for (int i = 1; i < components.Count; i++) {
						temp = components[i].MaxX;
						if (temp > maxX)
							maxX = temp;
					}
				}
				return maxX;
			}
		}

		/// <summary>
		/// Gets the largest Y coordinate
		/// </summary>
		public float MaxY {
			get {
				float maxY = components.Count == 0 ? currentLoc.Y : components[0].MaxY;
				float temp;
				lock (SyncRoot) {
					for (int i = 1; i < components.Count; i++) {
						temp = components[i].MaxY;
						if (temp > maxY)
							maxY = temp;
					}
				}
				return maxY;
			}
		}

		/// <summary>
		/// Gets the largest Z coordinate
		/// </summary>
		public float MaxZ {
			get {
				float maxZ = components.Count == 0 ? currentLoc.Z : components[0].MaxZ;
				float temp;
				lock (SyncRoot) {
					for (int i = 1; i < components.Count; i++) {
						temp = components[i].MaxZ;
						if (temp > maxZ)
							maxZ = temp;
					}
				}
				return maxZ;
			}
		}

		/// <summary>
		/// Gets the width of the bounding box of the model
		/// </summary>
		public float Width {
			get {
				return MaxX - MinX;
			}
		}

		/// <summary>
		/// Gets the height of the bounding box of the model
		/// </summary>
		public float Height {
			get {
				return MaxY - MinY;
			}
		}

		/// <summary>
		/// Gets the depth of the bounding box of the model
		/// </summary>
		public float Depth {
			get {
				return MaxZ - MinZ;
			}
		}

		/// <summary>
		/// Gets the bounding box sizes of the model
		/// </summary>
		public Vector3 Bounds {
			get {
				return new Vector3(Width, Height, Depth);
			}
		}

		/// <summary>
		/// Creates a new empty model structure
		/// </summary>
		public Model() {
		}

		/// <summary>
		/// Creates a new model structure from the specified components
		/// </summary>
		/// <param name="components">The components that are to make up the model structure</param>
		public Model(IEnumerable<IModel> components) {
			AddRange(components);
		}

		/// <summary>
		/// Creates a new model structure from the specified components
		/// </summary>
		/// <param name="components">The components that are to make up the model structure</param>
		public Model(IEnumerable<Model> components) {
			AddRange(components);
		}

		/// <summary>
		/// Creates a new model structure from the specified components
		/// </summary>
		/// <param name="components">The components that are to make up the model structure</param>
		public Model(IEnumerable<MeshComponent> components) {
			AddRange(components);
		}

		/// <summary>
		/// Creates a new model structure from the specified components
		/// </summary>
		/// <param name="components">The components that are to make up the model structure</param>
		public Model(params IModel[] components) {
			AddRange(components);
		}

		/// <summary>
		/// Creates a new model structure from the specified components
		/// </summary>
		/// <param name="components">The components that are to make up the model structure</param>
		public Model(params Model[] components) {
			AddRange(components);
		}

		/// <summary>
		/// Creates a new model structure from the specified components.
		/// </summary>
		/// <param name="components">The components that are to make up the model structure</param>
		public Model(params MeshComponent[] components) {
			AddRange(components);
		}

		/// <summary>
		/// Clones the specified model structure
		/// </summary>
		/// <param name="model">The model structure to clone</param>
		public Model(Model model) : this(model, model == null ? null : model.components) {
		}

		private Model(Model model, List<IModel> components) {
			if (model == null)
				return;
			AddRange(components);
			currentLoc = model.currentLoc;
			currentRot = model.currentRot;
			currentScale = model.currentScale;
			Name = model.Name;
			Tag = model.Tag;
		}

		/// <summary>
		/// Adds the specified component to the model
		/// </summary>
		/// <param name="model">The component to add</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Add(IModel model) {
			if (model == null)
				return;
			lock (SyncRoot) {
				if (components.Contains(model))
					return;
				IModel parent = this;
				do {
					if (model == parent)
						throw new InvalidOperationException("Cannot add parent models as children to the current model.");
					parent = parent.Parent;
				} while (parent != null);
				components.Add(model);
				model.Parent = this;
				vertices += model.Vertices;
				triangles += model.Triangles;
			}
		}

		/// <summary>
		/// Adds the specified components to the model
		/// </summary>
		/// <param name="models">The components to add</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void AddRange(IEnumerable<IModel> models) {
			if (models == null)
				return;
			foreach (IModel model in models)
				Add(model);
		}

		/// <summary>
		/// Adds the specified components to the model
		/// </summary>
		/// <param name="models">The components to add</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void AddRange(IEnumerable<Model> models) {
			if (models == null)
				return;
			foreach (Model model in models)
				Add(model);
		}

		/// <summary>
		/// Adds the specified components to the model
		/// </summary>
		/// <param name="models">The components to add</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void AddRange(IEnumerable<MeshComponent> models) {
			if (models == null)
				return;
			foreach (MeshComponent model in models)
				Add(model);
		}

		/// <summary>
		/// Adds the specified components to the model
		/// </summary>
		/// <param name="models">The components to add</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void AddRange(params IModel[] models) {
			if (models == null)
				return;
			foreach (IModel model in models)
				Add(model);
		}

		/// <summary>
		/// Adds the specified components to the model
		/// </summary>
		/// <param name="models">The components to add</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void AddRange(params Model[] models) {
			if (models == null)
				return;
			foreach (Model model in models)
				Add(model);
		}

		/// <summary>
		/// Adds the specified components to the model
		/// </summary>
		/// <param name="models">The components to add</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void AddRange(params MeshComponent[] models) {
			if (models == null)
				return;
			foreach (MeshComponent model in models)
				Add(model);
		}

		/// <summary>
		/// Removes the specified component from the model
		/// </summary>
		/// <param name="model">The components to remove</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool Remove(IModel model) {
			if (model == null)
				return false;
			lock (SyncRoot) {
				if (components.Remove(model)) {
					model.Parent = null;
					vertices -= model.Vertices;
					triangles -= model.Triangles;
					return true;
				} else
					return false;
			}
		}

		/// <summary>
		/// Removes all components from the model
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Clear() {
			lock (SyncRoot) {
				List<IModel> oldComponents = components;
				components = new List<IModel>();
				vertices = 0;
				triangles = 0;
				foreach (IModel component in oldComponents)
					component.Parent = null;
			}
		}

		/// <summary>
		/// Renders the model (must be called on a thread on which an OpenGL context is set up, preferably on the thread on which the model was loaded)
		/// </summary>
		public void Render() {
			Render(null);
		}

		/// <summary>
		/// Renders the model (must be called on a thread on which an OpenGL context is set up, preferably on the thread on which the model was loaded)
		/// </summary>
		/// <param name="nextModel">The next mesh component to interpolate with (can be null)</param>
		public virtual void Render(IModel nextModel) {
			RaiseRenderBegin();
			if (nextModel == null) {
				lock (SyncRoot) {
					for (int i = 0; i < components.Count; i++)
						components[i].Render();
				}
			} else {
				Model model = (Model) nextModel;
				lock (SyncRoot) {
					lock (model.SyncRoot) {
						for (int i = 0; i < components.Count; i++)
							components[i].Render(model.components[i]);
					}
				}
			}
			RaiseRenderEnd();
		}

		/// <summary>
		/// Raises the RenderBegin event
		/// </summary>
		protected void RaiseRenderBegin() {
			Action handler = RenderBegin;
			if (handler != null)
				handler();
		}

		/// <summary>
		/// Raises the RenderEnd event
		/// </summary>
		protected void RaiseRenderEnd() {
			Action handler = RenderEnd;
			if (handler != null)
				handler();
		}

		/// <summary>
		/// Raises the LocationChanged event
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		protected void RaiseLocationChanged() {
			Action handler = LocationChanged;
			if (handler != null)
				handler();
		}

		/// <summary>
		/// Raises the ScaleChanged event
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		protected void RaiseScaleChanged() {
			Action handler = ScaleChanged;
			if (handler != null)
				handler();
		}

		/// <summary>
		/// Raises the RotationChanged event
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		protected void RaiseRotationChanged() {
			Action handler = RotationChanged;
			if (handler != null)
				handler();
		}

		/// <summary>
		/// Translate the model by the specified vector
		/// </summary>
		/// <param name="vector">The vector to translate at</param>
		public void TranslateMesh(Vector3 vector) {
			lock (SyncRoot) {
				for (int i = 0; i < components.Count; i++)
					components[i].TranslateMesh(vector);
			}
		}

		/// <summary>
		/// Enlarges the model to the specified scale
		/// </summary>
		/// <param name="scale">The scale to enlarge by</param>
		/// <param name="centre">The centre to enlarge against</param>
		public void ScaleMesh(float scale, Vector3 centre) {
			lock (SyncRoot) {
				for (int i = 0; i < components.Count; i++)
					components[i].ScaleMesh(scale, centre);
			}
		}

		/// <summary>
		/// Enlarges the model to the specified scale
		/// </summary>
		/// <param name="scale">Contains the scale of the X, Y and Z dimension individually</param>
		/// <param name="centre">The centre to enlarge against</param>
		public void ScaleMesh(Vector3 scale, Vector3 centre) {
			lock (SyncRoot) {
				for (int i = 0; i < components.Count; i++)
					components[i].ScaleMesh(scale, centre);
			}
		}

		/// <summary>
		/// Transforms the model using the specified matrix
		/// </summary>
		/// <param name="matrix">The matrix to multiply transform vertices with</param>
		public void ApplyMatrixTransformation(ref Matrix4 matrix) {
			lock (SyncRoot) {
				for (int i = 0; i < components.Count; i++)
					components[i].ApplyMatrixTransformation(ref matrix);
			}
		}

		/// <summary>
		/// Rotates the model
		/// </summary>
		/// <param name="rotation">The matrix to multiply transform vertices with</param>
		public void RotateMesh(Vector3 rotation) {
			if (rotation == Vector3.Zero)
				return;
			Matrix4 temp = Matrix4.CreateRotationZ(rotation.Z) * Matrix4.CreateRotationY(rotation.Y) * Matrix4.CreateRotationX(rotation.X);
			ApplyMatrixTransformation(ref temp);
		}

		/// <summary>
		/// Returns an enumerator that iterates through the models
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public IEnumerator<IModel> GetEnumerator() {
			return components.GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through the models
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		IEnumerator IEnumerable.GetEnumerator() {
			return components.GetEnumerator();
		}

		/// <summary>
		/// Creates a copy of the model
		/// </summary>
		/// <returns>A copy of the model</returns>
		public virtual object Clone() {
			return new Model(this);
		}

		/// <summary>
		/// Creates a copy of the model
		/// </summary>
		/// <param name="cloneComponents">Whether to clone the integrated components as well</param>
		/// <returns>A copy of the model.</returns>
		public virtual IModel Clone(bool cloneComponents = true) {
			if (cloneComponents) {
				List<IModel> temp = new List<IModel>(components.Count);
				lock (SyncRoot) {
					for (int i = 0; i < components.Count; i++)
						temp.Add(components[i].Clone(true));
				}
				return new Model(this, temp);
			} else
				return new Model(this);
		}

		/// <summary>
		/// Gets a string that describes some aspects of the model
		/// </summary>
		public override string ToString() {
			string name = Name == null ? null : Name.Trim();
			if (name == null || name.Length == 0)
				return "{ Vertices: " + Vertices + " }";
			else
				return "{ Vertices: " + Vertices + ", Name: " + name + " }";
		}

		/// <summary>
		/// Called every time Dispose() is called. It is not recommended to dispose if finalizer is true
		/// </summary>
		/// <param name="finalizer">Whether the model is being disposed automatically by the garbage collector</param>
		protected virtual void OnDisposing(bool finalizer) {
		}

		/// <summary>
		/// Disposes of the resources used model
		/// </summary>
		~Model() {
			Dispose(true, true, true);
		}

		/// <summary>
		/// Disposes of the resources used by the model
		/// </summary>
		public void Dispose() {
			Dispose(true, true, false);
		}

		/// <summary>
		/// Disposes of the resources used by the model
		/// </summary>
		/// <param name="disposeChildren">Whether to dispose of the child components of the model</param>
		public void Dispose(bool disposeChildren) {
			Dispose(disposeChildren, true, false);
		}

		/// <summary>
		/// Disposes of the resources used by the model
		/// </summary>
		/// <param name="disposeChildren">Whether to dispose of the child components of the model</param>
		/// <param name="removeFromParent">Always set to true. Except in the Dispose() function of the parent, as all child components are removed automatically</param>
		public void Dispose(bool disposeChildren, bool removeFromParent) {
			Dispose(disposeChildren, removeFromParent, false);
		}

		private void Dispose(bool disposeChildren, bool removeFromParent, bool finalizer) {
			OnDisposing(finalizer);
			lock (SyncRoot) {
				if (parent != null) {
					if (removeFromParent)
						parent.Remove(this);
					parent = null;
				}
				if (disposeChildren) {
					for (int i = 0; i < components.Count; i++)
						components[i].Dispose(true, false);
				}
				components.Clear();
				vertices = 0;
				triangles = 0;
			}
		}
	}
}