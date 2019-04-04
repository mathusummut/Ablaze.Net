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
	public class TextureCollection : ITexture, IList<ITexture>, ICollection<ITexture>, IEnumerable<ITexture>, IEnumerable {
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
		/// Gets whether the collection is read-only, so returns false
		/// </summary>
		public bool IsReadOnly {
			get {
				return false;
			}
		}

		/// <summary>
		/// Gets or sets whether the texture alpha components are premultiplied. The texture affected is the one specified by BindIndex
		/// </summary>
		public bool Premultiplied {
			get {
				ITexture current = Current;
				if (current == null)
					return false;
				else
					return current.Premultiplied;
			}
			set {
				ITexture current = Current;
				if (current != null)
					current.Premultiplied = value;
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
		/// Gets or sets the index of the texture to bind, or negative numbers to mean relative to the current Count value (ex. -1 to mean that the last texture at Count - 1 is bound)
		/// </summary>
		public virtual int BindIndex {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the texture at the specified index. Setting to null removes the texture at the specified index
		/// </summary>
		/// <param name="index">The index of the texture to modify</param>
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
		/// Gets the current texture pointed by BindIndex (can be null)
		/// </summary>
		public ITexture Current {
			get {
				if (Count == 0)
					return null;
				lock (SyncRoot) {
					if (Count == 0)
						return null;
					else {
						int index = BindIndex;
						if (index < 0)
							index = textureList.Count + index;
						return textureList[index];
					}
				}
			}
		}

		/// <summary>
		/// Gets whether the texture collection is empty
		/// </summary>
		public bool IsEmpty {
			get {
				if (Count != 0) {
					lock (SyncRoot) {
						for (int i = 0; i < textureList.Count; i++) {
							if (!textureList[i].IsEmpty)
								return false;
						}
					}
				}
				return true;
			}
		}

		/// <summary>
		/// Gets whether the collection is disposed
		/// </summary>
		public bool IsDisposed {
			get {
				return IsEmpty;
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
			BindIndex = collection.BindIndex;
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
		/// Copies the elements of the collection to an Array, starting at a particular Array index
		/// </summary>
		/// <param name="array">The array to copy the elements to</param>
		/// <param name="arrayIndex">The index at which to insert the elements</param>
		public void CopyTo(ITexture[] array, int arrayIndex) {
			textureList.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Binds the texture at the index specified by BindIndex for use with OpenGL operations
		/// </summary>
		public virtual void Bind() {
			ITexture current = Current;
			if (Current == null)
				Unbind();
			else
				current.Bind();
		}

		/// <summary>
		/// Binds the texture at the index specified by BindIndex for use with OpenGL operations
		/// </summary>
		/// <param name="mode">The texture wrap mode to use</param>
		public virtual void Bind(TextureWrapMode mode) {
			ITexture current = Current;
			if (Current == null)
				Unbind();
			else
				current.Bind(mode);
		}

		/// <summary>
		/// Returns whether the specified texture is in the current texture collection (only a top-level search)
		/// </summary>
		/// <param name="texture">The texture to search for</param>
		public bool Contains(ITexture texture) {
			if (texture == null)
				return false;
			else
				return textureList.Contains(texture);
		}

		/// <summary>
		/// Unbinds the (any) texture
		/// </summary>
		public void Unbind() {
			Texture2D.UnbindTexture2D();
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