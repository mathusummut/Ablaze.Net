using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Graphics.OGL;
using System.Numerics;

namespace System.Graphics.Models {
	/// <summary>
	/// Offers methods to manage 3D model structures collectively
	/// </summary>
	public class Model : IModel, IList<IModel>, ICollection<IModel>, IEnumerable<IModel>, IEnumerable {
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
		/// <summary>
		/// The parent of the model
		/// </summary>
		private IModel parent;
		private Vector3 currentLoc, currentRot, currentScale = Vector3.One;
		private ITexture defaultTexture;
		private float alpha = 1f;
		private bool keepCopyInMemory;
		private int vertices, indexCount;
		/// <summary>
		/// Used to safely iterate or modify the components in this model
		/// </summary>
		protected readonly object SyncRoot = new object();
		/// <summary>
		/// A list of the component structures managed by the model
		/// </summary>
		private List<IModel> componentList = new List<IModel>();

		/// <summary>
		/// Gets whether the collection is read-only, so returns false
		/// </summary>
		public bool IsReadOnly {
			get {
				return false;
			}
		}

		/// <summary>
		/// Gets a read-only wrapper for the components in the model
		/// </summary>
		public ReadOnlyCollection<IModel> Components {
			get {
				return componentList.AsReadOnly();
			}
		}

		/// <summary>
		/// Gets or sets the parent model (can be null, not guaranteed to be accurate)
		/// </summary>
		public IModel Parent {
			get {
				return parent;
			}
			set {
				parent = value;
			}
		}

		/// <summary>
		/// Gets or sets the name of the model
		/// </summary>
		public string Name {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets an object that contains properties relevant to this model
		/// </summary>
		public object Tag {
			get;
			set;
		}

		/// <summary>
		/// Gets whether the current model is considered empty without evaluating sub-components
		/// </summary>
		public bool IsEmpty {
			get {
				return vertices <= 0 || indexCount <= 0;
			}
		}

		/// <summary>
		/// Gets the number of components in the model (or frame count if AnimatedModel)
		/// </summary>
		public int Count {
			get {
				return componentList.Count;
			}
		}

		/// <summary>
		/// Gets or sets the mesh at the specified index. Setting value to null will remove the texture at the specified index
		/// </summary>
		/// <param name="index">The index of the mesh to return</param>
		public IModel this[int index] {
			get {
				lock (SyncRoot)
					return componentList[index];
			}
			set {
				lock (SyncRoot) {
					IModel newModel = componentList[index];
					if (value == newModel)
						return;
					newModel.Parent = null;
					vertices -= newModel.Vertices;
					indexCount -= newModel.IndexCount;
					if (value == null)
						componentList.RemoveAt(index);
					else {
						IModel parent = this;
						do {
							if (newModel == parent)
								throw new InvalidOperationException("Cannot add parent models as children to the current model");
							parent = parent.Parent;
						} while (parent != null);
						if (!newModel.KeepCopyInMemory && KeepCopyInMemory)
							newModel.KeepCopyInMemory = true;
						componentList[index] = value;
						if (value.DefaultTexture == null)
							value.DefaultTexture = defaultTexture;
						newModel.Parent = this;
						vertices += value.Vertices;
						indexCount += value.IndexCount;
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
				return indexCount / 3;
			}
		}

		/// <summary>
		/// Gets the number of indices in the model
		/// </summary>
		public int IndexCount {
			get {
				return indexCount;
			}
		}

		/// <summary>
		/// Gets or sets whether the model components are visible (rendered) or not
		/// </summary>
		public bool Visible {
			get;
			set;
		}

		/// <summary>
		/// If true, the backs of mesh components are not rendered
		/// </summary>
		public bool Cull {
			get {
				return componentList.Count == 0 ? true : componentList[0].Cull;
			}
			set {
				lock (SyncRoot) {
					for (int i = 0; i < componentList.Count; i++)
						componentList[i].Cull = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets the rendering mode of the polygons of the mesh
		/// </summary>
		public MeshMode MeshMode {
			get {
				return componentList.Count == 0 ? MeshMode.Normal : componentList[0].MeshMode;
			}
			set {
				lock (SyncRoot) {
					for (int i = 0; i < componentList.Count; i++)
						componentList[i].MeshMode = value;
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
		/// Gets or sets the default texture of the model components when none is specified
		/// </summary>
		public ITexture DefaultTexture {
			get {
				return defaultTexture;
			}
			set {
				if (value == defaultTexture)
					return;
				defaultTexture = value;
				lock (SyncRoot) {
					for (int i = 0; i < componentList.Count; i++)
						componentList[i].DefaultTexture = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets the textures of all components, one texture for every child component
		/// </summary>
		public TextureCollection Textures {
			get {
				if (componentList.Count == 0)
					return new TextureCollection();
				TextureCollection collection = new TextureCollection();
				lock (SyncRoot) {
					for (int i = 0; i < componentList.Count; i++)
						collection.Add(componentList[i].Textures);
				}
				return collection;
			}
			set {
				if (value == null || value.Count == 0) {
					lock (SyncRoot) {
						for (int i = 0; i < componentList.Count; i++)
							componentList[i].Textures = null;
					}
				} else {
					int index = 0;
					lock (SyncRoot) {
						for (int i = 0; i < componentList.Count; i++)
							componentList[i].Textures = new TextureCollection(value[index++ % value.Count]);
					}
				}
			}
		}

		/// <summary>
		/// Gets whether the model is disposed
		/// </summary>
		public bool IsDisposed {
			get {
				return componentList.Count == 0;
			}
		}

		/// <summary>
		/// Gets or sets whether to keep a copy of the vertices in memory even after loading the vertices into GPU memory (allows faster and multithreaded vertex retrieval)
		/// </summary>
		public bool KeepCopyInMemory {
			get {
				return keepCopyInMemory;
			}
			set {
				keepCopyInMemory = value;
				lock (SyncRoot) {
					for (int i = 0; i < componentList.Count; i++)
						componentList[i].KeepCopyInMemory = value;
				}
			}
		}

		/// <summary>
		/// Set to true if you want the vertex buffer to be flushed on next render
		/// </summary>
		public bool FlushBufferOnNextRender {
			get {
				return componentList.Count == 0 ? false : componentList[0].FlushBufferOnNextRender;
			}
			set {
				lock (SyncRoot) {
					for (int i = 0; i < componentList.Count; i++)
						componentList[i].FlushBufferOnNextRender = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets the type of the model for optimization
		/// </summary>
		public BufferUsageHint Optimization {
			get {
				return componentList.Count == 0 ? BufferUsageHint.StaticDraw : componentList[0].Optimization;
			}
			set {
				lock (SyncRoot) {
					for (int i = 0; i < componentList.Count; i++)
						componentList[i].Optimization = value;
				}
			}
		}

		/// <summary>
		/// Gets the location of the centre of the model
		/// </summary>
		public Vector3 Center {
			get {
				if (componentList.Count == 0)
					return Vector3.Zero;
				Vector3 total = Vector3.Zero;
				lock (SyncRoot) {
					for (int i = 0; i < componentList.Count; i++)
						total += componentList[i].Center;
				}
				return total / componentList.Count;
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
					for (int i = 0; i < componentList.Count; i++)
						componentList[i].Location += toMove;
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
					for (int i = 0; i < componentList.Count; i++)
						componentList[i].Scale = Vector3.Multiply(componentList[i].Scale, toScale);
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
					for (int i = 0; i < componentList.Count; i++)
						componentList[i].Rotation += toRotate;
				}
				RaiseRotationChanged();
			}
		}

		/// <summary>
		/// Gets the smallest X coordinate
		/// </summary>
		public float MinX {
			get {
				float minX = componentList.Count == 0 ? currentLoc.X : componentList[0].MinX;
				float temp;
				lock (SyncRoot) {
					for (int i = 1; i < componentList.Count; i++) {
						temp = componentList[i].MinX;
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
				float minY = componentList.Count == 0 ? currentLoc.Y : componentList[0].MinY;
				float temp;
				lock (SyncRoot) {
					for (int i = 1; i < componentList.Count; i++) {
						temp = componentList[i].MinY;
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
				float minZ = componentList.Count == 0 ? currentLoc.Z : componentList[0].MinZ;
				float temp;
				lock (SyncRoot) {
					for (int i = 1; i < componentList.Count; i++) {
						temp = componentList[i].MinZ;
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
				float maxX = componentList.Count == 0 ? currentLoc.X : componentList[0].MaxX;
				float temp;
				lock (SyncRoot) {
					for (int i = 1; i < componentList.Count; i++) {
						temp = componentList[i].MaxX;
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
				float maxY = componentList.Count == 0 ? currentLoc.Y : componentList[0].MaxY;
				float temp;
				lock (SyncRoot) {
					for (int i = 1; i < componentList.Count; i++) {
						temp = componentList[i].MaxY;
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
				float maxZ = componentList.Count == 0 ? currentLoc.Z : componentList[0].MaxZ;
				float temp;
				lock (SyncRoot) {
					for (int i = 1; i < componentList.Count; i++) {
						temp = componentList[i].MaxZ;
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
			Visible = true;
		}

		/// <summary>
		/// Creates a new model structure from the specified components
		/// </summary>
		/// <param name="components">The components that are to make up the model structure</param>
		public Model(IEnumerable<IModel> components) : this() {
			AddRange(components);
		}

		/// <summary>
		/// Creates a new model structure from the specified components
		/// </summary>
		/// <param name="components">The components that are to make up the model structure</param>
		public Model(IEnumerable<Model> components) : this() {
			AddRange(components);
		}

		/// <summary>
		/// Creates a new model structure from the specified components
		/// </summary>
		/// <param name="components">The components that are to make up the model structure</param>
		public Model(IEnumerable<MeshComponent> components) : this() {
			AddRange(components);
		}

		/// <summary>
		/// Creates a new model structure from the specified components
		/// </summary>
		/// <param name="components">The components that are to make up the model structure</param>
		public Model(params IModel[] components) : this() {
			AddRange(components);
		}

		/// <summary>
		/// Creates a new model structure from the specified components
		/// </summary>
		/// <param name="components">The components that are to make up the model structure</param>
		public Model(params Model[] components) : this() {
			AddRange((IEnumerable<Model>) components);
		}

		/// <summary>
		/// Creates a new model structure from the specified components.
		/// </summary>
		/// <param name="components">The components that are to make up the model structure</param>
		public Model(params MeshComponent[] components) : this() {
			AddRange((IEnumerable<MeshComponent>) components);
		}

		/// <summary>
		/// Clones the specified model structure
		/// </summary>
		/// <param name="model">The model structure to clone</param>
		public Model(Model model) : this(model, model == null ? null : model.componentList) {
		}

		private Model(Model model, List<IModel> components) : this() {
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
		/// Returns whether the specified model is in the current model collection (only a top-level search)
		/// </summary>
		/// <param name="model">The model to search for</param>
		public bool Contains(IModel model) {
			if (model == null)
				return false;
			else
				return componentList.Contains(model);
		}

		/// <summary>
		/// Copies the elements of the collection to an Array, starting at a particular Array index
		/// </summary>
		/// <param name="array">The array to copy the elements to</param>
		/// <param name="arrayIndex">The index at which to insert the elements</param>
		public void CopyTo(IModel[] array, int arrayIndex) {
			componentList.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Gets the component at the specified index without locking
		/// </summary>
		/// <param name="index">The index of the model to return</param>
		protected IModel GetComponent(int index) {
			return componentList[index];
		}

		/// <summary>
		/// Gets the index of the specified model, or -1 if not found
		/// </summary>
		/// <param name="model">The index of the model to replace</param>
		public int IndexOf(IModel model) {
			if (model == null)
				return -1;
			lock (SyncRoot)
				return componentList.IndexOf(model);
		}

		/// <summary>
		/// Replaces the specified model
		/// </summary>
		/// <param name="oldModel">The model to replace</param>
		/// <param name="newModel">The model to replace with</param>
		public void Replace(IModel oldModel, IModel newModel) {
			if (oldModel == null)
				return;
			lock (SyncRoot) {
				int index = componentList.IndexOf(oldModel);
				if (index != -1)
					this[index] = newModel;
			}
		}

		/// <summary>
		/// Adds the component to the model at the specified index
		/// </summary>
		/// <param name="index">The index to insert at. If the index is -1, the components inserted at the end</param>
		/// <param name="model">The component to add</param>
		public virtual void Insert(int index, IModel model) {
			if (model == null)
				return;
			lock (SyncRoot) {
				IModel parent = this;
				do {
					if (model == parent)
						throw new InvalidOperationException("Cannot add parent models as children to the current model");
					parent = parent.Parent;
				} while (parent != null);
				if (!model.KeepCopyInMemory && KeepCopyInMemory)
					model.KeepCopyInMemory = true;
				componentList.Insert(index == -1 ? Count : index, model);
				model.Parent = this;
				if (model.DefaultTexture == null)
					model.DefaultTexture = defaultTexture;
				vertices += model.Vertices;
				indexCount += model.IndexCount;
			}
		}

		/// <summary>
		/// Adds the specified component to the model
		/// </summary>
		/// <param name="model">The component to add</param>
		public void Add(IModel model) {
			Insert(-1, model);
		}

		/// <summary>
		/// Adds the specified components to the model
		/// </summary>
		/// <param name="models">The components to add</param>
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
		public void AddRange(IEnumerable<MeshComponent> models) {
			if (models == null)
				return;
			foreach (MeshComponent model in models)
				Add(model);
		}

		/// <summary>
		/// Removes the specified component from the model
		/// </summary>
		/// <param name="index">The index of the component to remove</param>
		public void RemoveAt(int index) {
			lock (SyncRoot) {
				IModel model = componentList[index];
				componentList.RemoveAt(index);
				model.Parent = null;
				vertices -= model.Vertices;
				indexCount -= model.IndexCount;
			}
		}

		/// <summary>
		/// Removes the specified component from the model
		/// </summary>
		/// <param name="model">The component to remove</param>
		public bool Remove(IModel model) {
			if (model == null)
				return false;
			lock (SyncRoot) {
				if (componentList.Remove(model)) {
					model.Parent = null;
					vertices -= model.Vertices;
					indexCount -= model.IndexCount;
					return true;
				} else
					return false;
			}
		}

		/// <summary>
		/// Removes all components from the model, and returns the old components that were used (can be discarded, useful for disposing them separately)
		/// </summary>
		public List<IModel> ClearComponents() {
			lock (SyncRoot) {
				List<IModel> oldComponents = componentList;
				componentList = new List<IModel>();
				vertices = 0;
				indexCount = 0;
				foreach (IModel component in oldComponents)
					component.Parent = null;
				return oldComponents;
			}
		}

		/// <summary>
		/// Removes all components from the model
		/// </summary>
		public void Clear() {
			ClearComponents();
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
			if (!Visible)
				return;
			RaiseRenderBegin();
			if (nextModel == null) {
				lock (SyncRoot) {
					for (int i = 0; i < componentList.Count; i++)
						componentList[i].Render();
				}
			} else {
				Model model = (Model) nextModel;
				lock (SyncRoot) {
					lock (model.SyncRoot) {
						for (int i = 0; i < componentList.Count; i++)
							componentList[i].Render(model.componentList[i]);
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
		protected void RaiseLocationChanged() {
			Action handler = LocationChanged;
			if (handler != null)
				handler();
		}

		/// <summary>
		/// Raises the ScaleChanged event
		/// </summary>
		protected void RaiseScaleChanged() {
			Action handler = ScaleChanged;
			if (handler != null)
				handler();
		}

		/// <summary>
		/// Raises the RotationChanged event
		/// </summary>
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
				for (int i = 0; i < componentList.Count; i++)
					componentList[i].TranslateMesh(vector);
			}
		}

		/// <summary>
		/// Enlarges the model to the specified scale
		/// </summary>
		/// <param name="scale">The scale to enlarge by</param>
		/// <param name="centre">The centre to enlarge against</param>
		public void ScaleMesh(float scale, Vector3 centre) {
			lock (SyncRoot) {
				for (int i = 0; i < componentList.Count; i++)
					componentList[i].ScaleMesh(scale, centre);
			}
		}

		/// <summary>
		/// Enlarges the model to the specified scale
		/// </summary>
		/// <param name="scale">Contains the scale of the X, Y and Z dimension individually</param>
		/// <param name="centre">The centre to enlarge against</param>
		public void ScaleMesh(Vector3 scale, Vector3 centre) {
			lock (SyncRoot) {
				for (int i = 0; i < componentList.Count; i++)
					componentList[i].ScaleMesh(scale, centre);
			}
		}

		/// <summary>
		/// Transforms the model using the specified matrix
		/// </summary>
		/// <param name="matrix">The matrix to multiply transform vertices with</param>
		public void ApplyMatrixTransformation(ref Matrix4 matrix) {
			lock (SyncRoot) {
				for (int i = 0; i < componentList.Count; i++)
					componentList[i].ApplyMatrixTransformation(ref matrix);
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
		public IEnumerator<IModel> GetEnumerator() {
			return componentList.GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through the models
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator() {
			return componentList.GetEnumerator();
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
				List<IModel> temp = new List<IModel>(componentList.Count);
				lock (SyncRoot) {
					for (int i = 0; i < componentList.Count; i++)
						temp.Add(componentList[i].Clone(true));
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
				return GetType().Name + ": { Count: " + Count + ", Vertices: " + Vertices + " }";
			else
				return GetType().Name + ": { Count: " + Count + ", Vertices: " + Vertices + ", Name: " + name + " }";
		}

		/// <summary>
		/// Called every time Dispose() is called. Warning: may be called from inside finalizer
		/// </summary>
		protected virtual void OnDisposing() {
		}

		/// <summary>
		/// Disposes of the resources used model
		/// </summary>
		~Model() {
			Dispose(false, false);
		}

		/// <summary>
		/// Disposes of the resources used by the model
		/// </summary>
		public void Dispose() {
			Dispose(false, true);
		}

		/// <summary>
		/// Disposes of the resources used by the model
		/// </summary>
		/// <param name="disposeChildren">Whether to dispose of the child components of the model</param>
		public void Dispose(bool disposeChildren) {
			Dispose(disposeChildren, true);
		}

		/// <summary>
		/// Disposes of the resources used by the model
		/// </summary>
		/// <param name="disposeChildren">Whether to dispose of the child components of the model</param>
		/// <param name="removeFromParent">Always set to true. Except in the Dispose() function of the parent, as all child components are removed automatically</param>
		public void Dispose(bool disposeChildren, bool removeFromParent) {
			OnDisposing();
			lock (SyncRoot) {
				if (removeFromParent) {
					Model tentativeParent = parent as Model;
					if (tentativeParent != null)
						tentativeParent.Remove(this);
				}
				parent = null;
				if (disposeChildren) {
					for (int i = 0; i < componentList.Count; i++)
						componentList[i].Dispose(true, false);
				}
				componentList.Clear();
				vertices = 0;
				indexCount = 0;
			}
		}
	}
}