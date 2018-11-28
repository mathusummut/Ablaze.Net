using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Graphics.OGL;
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
		/// Gets or sets whether the texture alpha components are premultiplied. The texture affected is the one specified by BindIndex
		/// </summary>
		public bool Premultiplied {
			get {
				if (Count == 0)
					return false;
				lock (SyncRoot) {
					if (Count == 0)
						return false;
					else {
						int index = BindIndex;
						if (index == -1)
							index = textureList.Count - 1;
						return textureList[index].Premultiplied;
					}
				}
			}
			set {
				if (Count == 0)
					return;
				lock (SyncRoot) {
					if (Count == 0)
						return;
					else {
						int index = BindIndex;
						if (index == -1)
							index = textureList.Count - 1;
						textureList[index].Premultiplied = value;
					}
				}
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
		/// Gets or sets the index of the texture to bind, or -1 to mean that the last texture (at Count - 1) is bound
		/// </summary>
		public virtual int BindIndex {
			get;
			set;
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
			BindIndex = -1;
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
			AddRange((IEnumerable<TextureCollection>) textures);
		}

		/// <summary>
		/// Creates a new collection from the specified textures
		/// </summary>
		/// <param name="textures">The textures that are to form the collection</param>
		public TextureCollection(params Texture2D[] textures) : this() {
			AddRange((IEnumerable<Texture2D>) textures);
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
		/// Replaces the specified texture
		/// </summary>
		/// <param name="oldTexture">The texture to replace</param>
		/// <param name="newTexture">The texture to replace with</param>
		public void Replace(ITexture oldTexture, ITexture newTexture) {
			if (oldTexture == null)
				return;
			lock (SyncRoot) {
				int index = textureList.IndexOf(oldTexture);
				if (index != -1)
					this[index] = newTexture;
			}
		}

		/// <summary>
		/// Adds the texture to the collection at the specified index
		/// </summary>
		/// <param name="index">The index to insert at. If the index is -1, the texture inserted at the end</param>
		/// <param name="texture">The texture to add</param>
		public virtual void Insert(int index, ITexture texture) {
			if (texture == null)
				return;
			lock (SyncRoot)
				textureList.Insert(index == -1 ? Count : index, texture);
		}

		/// <summary>
		/// Adds the specified texture to the collection
		/// </summary>
		/// <param name="texture">The texture to add</param>
		public void Add(ITexture texture) {
			Insert(-1, texture);
		}

		/// <summary>
		/// Adds the specified textures to the collection
		/// </summary>
		/// <param name="textures">The textures to add</param>
		public void AddRange(IEnumerable<ITexture> textures) {
			if (textures == null)
				return;
			foreach (ITexture texture in textures)
				Add(texture);
		}

		/// <summary>
		/// Adds the specified textures to the collection
		/// </summary>
		/// <param name="textures">The textures to add</param>
		public void AddRange(IEnumerable<TextureCollection> textures) {
			if (textures == null)
				return;
			foreach (TextureCollection texture in textures)
				Add(texture);
		}

		/// <summary>
		/// Adds the specified textures to the collection
		/// </summary>
		/// <param name="textures">The textures to add</param>
		public void AddRange(IEnumerable<Texture2D> textures) {
			if (textures == null)
				return;
			foreach (Texture2D texture in textures)
				Add(texture);
		}

		/// <summary>
		/// Removes the specified texture from the collection
		/// </summary>
		/// <param name="index">The index of the texture to remove</param>
		public void RemoveAt(int index) {
			lock (SyncRoot)
				textureList.RemoveAt(index);
		}

		/// <summary>
		/// Removes the specified texture from the collection
		/// </summary>
		/// <param name="texture">The texture to remove</param>
		public bool Remove(ITexture texture) {
			if (texture == null)
				return false;
			lock (SyncRoot)
				return textureList.Remove(texture);
		}

		/// <summary>
		/// Removes all textures from the collection
		/// </summary>
		public void Clear() {
			lock (SyncRoot)
				textureList = new List<ITexture>();
		}

		/// <summary>
		/// Binds the texture at the index specified by BindIndex for use with OpenGL operations
		/// </summary>
		public virtual void Bind() {
			if (Count == 0)
				Unbind();
			else {
				lock (SyncRoot) {
					if (Count == 0)
						Unbind();
					else {
						int index = BindIndex;
						if (index == -1)
							index = textureList.Count - 1;
						textureList[index].Bind();
					}
				}
			}
		}

		/// <summary>
		/// Binds the texture at the index specified by BindIndex for use with OpenGL operations
		/// </summary>
		/// <param name="mode">The texture wrap mode to use</param>
		public virtual void Bind(TextureWrapMode mode) {
			if (Count == 0)
				Unbind();
			else {
				lock (SyncRoot) {
					if (Count == 0)
						Unbind();
					else {
						int index = BindIndex;
						if (index == -1)
							index = textureList.Count - 1;
						textureList[index].Bind(mode);
					}
				}
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
		public IEnumerator<ITexture> GetEnumerator() {
			return textureList.GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through the textures
		/// </summary>
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
		/// Called every time Dispose() is called. Warning: may be called from inside finalizer
		/// </summary>
		protected virtual void OnDisposing() {
		}

		/// <summary>
		/// Disposes of the resources used collection
		/// </summary>
		~TextureCollection() {
			Dispose(false);
		}

		/// <summary>
		/// Disposes of the resources used by the collection
		/// </summary>
		public void Dispose() {
			Dispose(false);
		}

		/// <summary>
		/// Disposes of the resources used by the collection
		/// </summary>
		/// <param name="disposeChildren">Whether to dispose of the child components of the collection</param>
		public void Dispose(bool disposeChildren) {
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