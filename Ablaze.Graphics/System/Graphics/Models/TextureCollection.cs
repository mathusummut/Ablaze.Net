using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Graphics.OGL;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace System.Graphics.Models {
	/// <summary>
	/// Offers methods to manage textures collectively
	/// </summary>
	public class TextureCollection : ITexture, IEnumerable<ITexture>, IEnumerable {
		/// <summary>
		/// Used to safely iterate or modify the textures in this collection
		/// </summary>
		protected readonly object SyncRoot = new object();
		private ImageParameterAction bindAction;
		/// <summary>
		/// A list of the textures managed by the collection
		/// </summary>
		private List<ITexture> textureList = new List<ITexture>();

		/// <summary>
		/// Gets a read-only wrapper for the textures in the collection
		/// </summary>
		public ReadOnlyCollection<ITexture> Textures {
			get {
				return textureList.AsReadOnly();
			}
		}

		/// <summary>
		/// Gets or sets the name of the texture collection
		/// </summary>
		public string Name {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets an object that contains properties relevant to this collection
		/// </summary>
		public object Tag {
			get;
			set;
		}

		/// <summary>
		/// Gets the number of textures in the collection
		/// </summary>
		public int Count {
			get {
				return textureList.Count;
			}
		}

		/// <summary>
		/// Gets the texture at the specified index
		/// </summary>
		/// <param name="index">The index of the texture to return</param>
		public ITexture this[int index] {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				lock (SyncRoot)
					return textureList[index];
			}
			set {
				lock (SyncRoot) {
					if (value == null)
						textureList.RemoveAt(index);
					else
						textureList[index] = value;
				}
			}
		}

		/// <summary>
		/// Gets whether the collection is disposed
		/// </summary>
		public bool IsDisposed {
			get {
				return textureList.Count == 0;
			}
		}

		/// <summary>
		/// Gets or sets what to do with the passed image after binding the texture into GPU memory
		/// </summary>
		public ImageParameterAction BindAction {
			get {
				return bindAction;
			}
			set {
				bindAction = value;
				lock (SyncRoot) {
					for (int i = 0; i < textureList.Count; i++)
						textureList[i].BindAction = value;
				}
			}
		}

		/// <summary>
		/// Creates a new empty texture collection
		/// </summary>
		public TextureCollection() {
		}

		/// <summary>
		/// Creates a new collection from the specified textures
		/// </summary>
		/// <param name="textures">The textures that are to form the collection</param>
		public TextureCollection(IEnumerable<ITexture> textures) : this() {
			AddRange(textures);
		}

		/// <summary>
		/// Creates a new collection from the specified textures
		/// </summary>
		/// <param name="textures">The textures that are to form the collection</param>
		public TextureCollection(IEnumerable<TextureCollection> textures) : this() {
			AddRange(textures);
		}

		/// <summary>
		/// Creates a new collection from the specified textures
		/// </summary>
		/// <param name="textures">The textures that are to form the collection</param>
		public TextureCollection(IEnumerable<Texture2D> textures) : this() {
			AddRange(textures);
		}

		/// <summary>
		/// Creates a new collection from the specified textures
		/// </summary>
		/// <param name="textures">The textures that are to form the collection</param>
		public TextureCollection(params ITexture[] textures) : this() {
			AddRange(textures);
		}

		/// <summary>
		/// Creates a new collection from the specified textures
		/// </summary>
		/// <param name="textures">The textures that are to form the collection</param>
		public TextureCollection(params TextureCollection[] textures) : this() {
			AddRange(textures);
		}

		/// <summary>
		/// Creates a new collection from the specified textures
		/// </summary>
		/// <param name="textures">The textures that are to form the collection</param>
		public TextureCollection(params Texture2D[] textures) : this() {
			AddRange(textures);
		}

		/// <summary>
		/// Clones the specified texture collection
		/// </summary>
		/// <param name="textures">The texture collection to clone</param>
		public TextureCollection(TextureCollection textures) : this(textures, textures == null ? null : textures.textureList) {
		}

		private TextureCollection(TextureCollection collection, List<ITexture> textures) : this() {
			if (collection == null)
				return;
			AddRange(textures);
			bindAction = collection.BindAction;
			Name = collection.Name;
			Tag = collection.Tag;
		}

		/// <summary>
		/// Gets the texture at the specified index without locking
		/// </summary>
		/// <param name="index">The index of the texture to return</param>
		protected ITexture GetTexture(int index) {
			return textureList[index];
		}

		/// <summary>
		/// Gets the index of the specified texture, or -1 if not found
		/// </summary>
		/// <param name="texture">The index of the texture to replace</param>
		public int IndexOf(ITexture texture) {
			if (texture == null)
				return -1;
			lock (SyncRoot)
				return textureList.IndexOf(texture);
		}

		/// <summary>
		/// Replaces the specified model
		/// </summary>
		/// <param name="oldModel">The index of the model to replace</param>
		/// <param name="newModel">The model to replace with</param>
		public void Replace(IModel oldModel, IModel newModel) {
			if (oldModel == null || newModel == null)
				return;
			lock (SyncRoot)
				Replace(componentList.IndexOf(oldModel), newModel);
		}

		/// <summary>
		/// Replaces the model at the specified index
		/// </summary>
		/// <param name="index">The index of the model to replace</param>
		/// <param name="newModel">The model to replace with</param>
		public void Replace(int index, IModel newModel) {
			if (newModel == null)
				return;
			lock (SyncRoot) {
				IModel old = componentList[index];
				old.Parent = null;
				vertices -= old.Vertices;
				triangles -= old.Triangles;
				IModel parent = this;
				do {
					if (newModel == parent)
						throw new InvalidOperationException("Cannot add parent models as children to the current model");
					parent = parent.Parent;
				} while (parent != null);
				if (!newModel.KeepCopyInMemory && KeepCopyInMemory)
					newModel.KeepCopyInMemory = true;
				componentList[index] = newModel;
				newModel.Parent = this;
				vertices += newModel.Vertices;
				triangles += newModel.Triangles;
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
				vertices += model.Vertices;
				triangles += model.Triangles;
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
		/// <param name="index">The index of the component to remove</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool RemoveAt(int index) {
			lock (SyncRoot) {
				IModel model = componentList[index];
				componentList.RemoveAt(index);
				model.Parent = null;
				vertices -= model.Vertices;
				triangles -= model.Triangles;
				return true;
			}
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
				if (componentList.Remove(model)) {
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
				List<IModel> oldComponents = componentList;
				componentList = new List<IModel>();
				vertices = 0;
				triangles = 0;
				foreach (IModel component in oldComponents)
					component.Parent = null;
			}
		}

		/// <summary>
		/// Adds a reference to every texture in the collection
		/// </summary>
		public void AddReference() {
			lock (SyncRoot) {
				for (int i = 0; i < textureList.Count; i++)
					textureList[i].AddReference();
			}
		}

		/// <summary>
		/// Binds the texture at the last index for use with OpenGL operations
		/// </summary>
		public void Bind() {
			if (Count == 0)
				return;
			lock (SyncRoot) {
				if (Count != 0)
					textureList[textureList.Count - 1].Bind();
			}
		}

		/// <summary>
		/// Binds the textureat the last index for use with OpenGL operations
		/// </summary>
		/// <param name="mode">The texture wrap mode to use</param>
		public void Bind(TextureWrapMode mode) {
			if (Count == 0)
				return;
			lock (SyncRoot) {
				if (Count != 0)
					textureList[textureList.Count - 1].Bind(mode);
			}
		}

		/// <summary>
		/// Unbinds the (any) texture
		/// </summary>
		public void Unbind() {
			GL.BindTexture(TextureTarget.Texture2D, 0);
		}

		/// <summary>
		/// Returns an enumerator that iterates through the textures
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public IEnumerator<ITexture> GetEnumerator() {
			return textureList.GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through the textures
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		IEnumerator IEnumerable.GetEnumerator() {
			return textureList.GetEnumerator();
		}

		/// <summary>
		/// Creates a copy of the collection
		/// </summary>
		/// <returns>A copy of the collection</returns>
		public virtual object Clone() {
			return new TextureCollection(this);
		}

		/// <summary>
		/// Creates a copy of the collection
		/// </summary>
		/// <param name="cloneComponents">Whether to clone the integrated components as well</param>
		public virtual ITexture Clone(bool cloneComponents = true) {
			if (cloneComponents) {
				List<ITexture> temp = new List<ITexture>(textureList.Count);
				lock (SyncRoot) {
					for (int i = 0; i < textureList.Count; i++)
						temp.Add(textureList[i].Clone(true));
				}
				return new TextureCollection(this, temp);
			} else
				return new TextureCollection(this);
		}

		/// <summary>
		/// Gets a string that describes some aspects of the collection
		/// </summary>
		public override string ToString() {
			string name = Name == null ? null : Name.Trim();
			if (name == null || name.Length == 0)
				return GetType().Name + ": { Count: " + Count + " }";
			else
				return GetType().Name + ": { Count: " + Count + ", Name: " + name + " }";
		}

		/// <summary>
		/// Called every time Dispose() is called. It is not recommended to dispose if finalizer is true
		/// </summary>
		/// <param name="finalizer">Whether the model is being disposed automatically by the garbage collector</param>
		protected virtual void OnDisposing(bool finalizer) {
		}

		/// <summary>
		/// Disposes of the resources used collection
		/// </summary>
		~TextureCollection() {
			GraphicsContext.IsFinalizer = true;
			try {
				Dispose(true);
			} finally {
				GraphicsContext.IsFinalizer = false;
			}
		}

		/// <summary>
		/// Disposes of the resources used by the collection
		/// </summary>
		public void Dispose() {
			Dispose(true);
		}

		/// <summary>
		/// Disposes of the resources used by the collection
		/// </summary>
		/// <param name="disposeChildren">Whether to dispose of the child components of the collection</param>
		public void Dispose(bool disposeChildren) {
			OnDisposing(GraphicsContext.IsFinalizer);
			lock (SyncRoot) {
				if (disposeChildren) {
					for (int i = 0; i < textureList.Count; i++)
						textureList[i].Dispose(true);
				}
				textureList.Clear();
			}
		}
	}
}