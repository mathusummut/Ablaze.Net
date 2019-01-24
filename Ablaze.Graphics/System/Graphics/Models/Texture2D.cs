using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Graphics.OGL;
using System.Runtime.CompilerServices;

namespace System.Graphics.Models {
	using Graphics = Drawing.Graphics;

	/// <summary>
	/// A managed wrapper for an OpenGL texture
	/// </summary>
	public sealed class Texture2D : ITexture, IEquatable<Texture2D> {
		/// <summary>
		/// An empty texture
		/// </summary>
		public static readonly Texture2D Empty = new Texture2D();
		private int id;
		private bool init, loadedFromHandle, lastFilter = true, lastMipmap = true;
		private Bitmap image, copy;
		private GraphicsContext parentContext;
		private TextureWrapMode lastTextureWrapModeX = TextureWrapMode.Repeat, lastTextureWrapModeY = TextureWrapMode.Repeat;
		/// <summary>
		/// Increases texture interpolation quality by reducing shimmering caused by aliasing, but uses more memory
		/// </summary>
		public bool UseMipmapping = true;
		private bool pad;
		/// <summary>
		/// Gets or sets whether to use a linear scaling filter or no filter on the texture
		/// </summary>
		public bool LinearScalingFilter = true;

		/// <summary>
		/// Gets whether mipmapping is supported
		/// </summary>
		public static bool? MipmapSupported {
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets whether the texture alpha components are premultiplied
		/// </summary>
		public bool Premultiplied {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets additional info to be stored with the texture
		/// </summary>
		public object Tag {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the texture name
		/// </summary>
		public string Name {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets what to do with the passed image after binding the texture into GPU memory
		/// </summary>
		public ImageParameterAction BindAction {
			get;
			set;
		}

		/// <summary>
		/// Gets the native OpenGL name of the texture (0 if not bound once yet)
		/// </summary>
		public int ID {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return id;
			}
		}

		/// <summary>
		/// Gets the current texture that will be bound upon calling Bind(), which in this case is simply "return this;"
		/// </summary>
		public ITexture Current {
			get {
				return this;
			}
		}

		/// <summary>
		/// Gets the size of the source bitmap
		/// </summary>
		public Size BitmapSize {
			get;
			private set;
		}

		/// <summary>
		/// Gets the size of the texture
		/// </summary>
		public Size TextureSize {
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets the texture wrap mode used for the X-axis of the image
		/// </summary>
		public TextureWrapMode TextureWrapModeX {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the texture wrap mode used for the Y-axis of the image
		/// </summary>
		public TextureWrapMode TextureWrapModeY {
			get;
			set;
		}

		/// <summary>
		/// Gets the texture handle
		/// </summary>
		public int Handle {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				if (id == 0 && image != null)
					Bind();
				return id;
			}
		}

		/// <summary>
		/// Gets the underlying bitmap of the texture
		/// </summary>
		public Bitmap Image {
			get {
				if (IsDisposed || TextureSize.Width == 0 || TextureSize.Height == 0)
					return null;
				else if (image != null)
					return image;
				else if (copy != null)
					return copy;
				Bind(TextureWrapMode.ClampToEdge);
				Bitmap bmp = new Bitmap(TextureSize.Width, TextureSize.Height);
				BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, Premultiplied ? PixelFormat.Format32bppPArgb : Drawing.Imaging.PixelFormat.Format32bppArgb);
				GL.GetTexImage(TextureTarget.Texture2D, 0, TargetPixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
				bmp.UnlockBits(data);
				return bmp;
			}
		}

		/// <summary>
		/// Gets whether the texture is empty
		/// </summary>
		public bool IsEmpty {
			get {
				return id == 0 && image == null;
			}
		}

		/// <summary>
		/// Gets whether the texture is disposed
		/// </summary>
		public bool IsDisposed {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return IsEmpty;
			}
		}

		/// <summary>
		/// Gets whether the texture has ever been bound
		/// </summary>
		public bool HasBeenBoundAtLeastOnce {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return image == null;
			}
		}

		/// <summary>
		/// An empty texture
		/// </summary>
		public Texture2D() {
			TextureWrapModeX = TextureWrapMode.Repeat;
			TextureWrapModeY = TextureWrapMode.Repeat;
		}

		/// <summary>
		/// Creates a new OpenGL texture from the specified bitmap image
		/// </summary>
		/// <param name="image">The image to create the OpenGL texture from</param>
		/// <param name="mode">Specifies which operation to perform on Non-Power-Of-Two-sized textures
		/// If the texture has both dimensions that are a power of two, then no operation is performed</param>
		/// <param name="bindAction">Determines what to do with the image after binding into GPU memory</param>
		/// <param name="linearScalingFilter">Whether to use a linear scaling filter or no filter on the texture</param>
		/// <param name="useMipmapping">Increases texture interpolation quality by reducing shimmering caused by aliasing, but uses more memory</param>
		public Texture2D(Bitmap image, NPotTextureScaleMode mode, ImageParameterAction bindAction = ImageParameterAction.RemoveReference, bool linearScalingFilter = true, bool useMipmapping = true) : this() {
			if (image == null)
				return;
			this.image = image;
			Premultiplied = image.PixelFormat == PixelFormat.Format32bppPArgb;
			BitmapSize = image.Size;
			if (mode == NPotTextureScaleMode.Pad)
				pad = true;
			TextureSize = mode == NPotTextureScaleMode.None ? BitmapSize : ImageLib.GetPowerOfTwoSize(BitmapSize, mode == NPotTextureScaleMode.ScaleUp || pad);
			BindAction = bindAction;
			LinearScalingFilter = linearScalingFilter;
			lastFilter = linearScalingFilter;
			UseMipmapping = useMipmapping;
			lastMipmap = useMipmapping;
		}

		/// <summary>
		/// Creates a new OpenGL texture of the specified size
		/// </summary>
		/// <param name="width">The width of the texture</param>
		/// <param name="height">The height of the texture</param>
		/// <param name="linearScalingFilter">Whether to use a linear scaling filter or no filter on the texture</param>
		/// <param name="useMipmapping">Increases texture interpolation quality by reducing shimmering caused by aliasing, but uses more memory</param>
		public Texture2D(int width, int height, bool linearScalingFilter = true, bool useMipmapping = true) : this() {
			BitmapSize = new Size(width, height);
			TextureSize = BitmapSize;
			LinearScalingFilter = linearScalingFilter;
			lastFilter = linearScalingFilter;
			UseMipmapping = useMipmapping;
			lastMipmap = useMipmapping;
			init = true;
		}

		/// <summary>
		/// Initializes a texture wrapper using the specified handle
		/// </summary>
		/// <param name="handle">The handle of the texture</param>
		public Texture2D(int handle) : this() {
			id = handle;
			loadedFromHandle = true;
		}

		/// <summary>
		/// Initializes textures from the specified file paths
		/// </summary>
		/// <param name="images">The paths to the image files to load</param>
		public static TextureCollection ToTextures(params string[] images) {
			if (images == null || images.Length == 0)
				return new TextureCollection();
			TextureCollection textures = new TextureCollection();
			TextureCollection temp;
			for (int i = 0; i < images.Length; i++) {
				temp = Parsers.TextureParser.Parse(images[i]);
				if (temp != null)
					textures.Add(temp);
			}
			return textures;
		}

		/// <summary>
		/// Initializes textures from the specified bitmaps using NPotTextureScaleMode.ScaleUp
		/// </summary>
		/// <param name="bindAction">Determines what to do with the image after binding into GPU memory</param>
		/// <param name="images">The bitmaps to create textures from</param>
		public static TextureCollection ToTextures(ImageParameterAction bindAction, params Bitmap[] images) {
			if (images == null || images.Length == 0)
				return new TextureCollection();
			TextureCollection textures = new TextureCollection();
			for (int i = 0; i < images.Length; i++)
				textures[i] = new Texture2D(images[i], NPotTextureScaleMode.ScaleUp, bindAction);
			return textures;
		}

		/// <summary>
		/// Binds the texture for use with OpenGL operations
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Bind() {
			Bind(TextureWrapModeX, TextureWrapModeY);
		}

		/// <summary>
		/// Binds the texture for use with OpenGL operations
		/// </summary>
		/// <param name="mode">The texture wrap mode to use for the X and Y axes of the image</param>
		public void Bind(TextureWrapMode mode) {
			Bind(mode, mode);
		}

		/// <summary>
		/// Binds the texture for use with OpenGL operations
		/// </summary>
		/// <param name="modeX">The texture wrap mode to use for the X-axis of the image</param>
		/// <param name="modeY">The texture wrap mode to use for the Y-axis of the image</param>
		public void Bind(TextureWrapMode modeX, TextureWrapMode modeY) {
			if (loadedFromHandle) {
				loadedFromHandle = false;
				int width, height;
				GL.GetTexParameter(TextureTarget.Texture2D, GetTextureParameter.TextureWidth, out width);
				GL.GetTexParameter(TextureTarget.Texture2D, GetTextureParameter.TextureHeight, out height);
				BitmapSize = new Size(width, height);
				TextureSize = BitmapSize;
			}
			if (image == null) {
				if (init) {
					GL.GenTextures(1, out id);
					parentContext = GraphicsContext.CurrentContext;
					GL.BindTexture(TextureTarget.Texture2D, id);
					GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, TextureSize.Width, TextureSize.Height, 0, TargetPixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);
					GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) (LinearScalingFilter ? TextureMinFilter.Linear : TextureMinFilter.Nearest));
					GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) (LinearScalingFilter ? TextureMagFilter.Linear : TextureMagFilter.Nearest));
					GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) modeX);
					GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) modeY);
					init = false;
				} else
					GL.BindTexture(TextureTarget.Texture2D, id);
				if (!(LinearScalingFilter == lastFilter || UseMipmapping == lastMipmap)) {
					lastFilter = LinearScalingFilter;
					bool mipmap = UseMipmapping;
					if (mipmap && !MipmapSupported.Value)
						mipmap = false;
					GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) (LinearScalingFilter ? (mipmap ? TextureMinFilter.LinearMipmapLinear : TextureMinFilter.Linear) : (mipmap ? TextureMinFilter.NearestMipmapLinear : TextureMinFilter.Nearest)));
					GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) (LinearScalingFilter ? TextureMagFilter.Linear : TextureMagFilter.Nearest));
				}
				if (lastTextureWrapModeX != modeX) {
					lastTextureWrapModeX = modeX;
					GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) modeX);
				}
				if (lastTextureWrapModeY != modeY) {
					lastTextureWrapModeY = modeY;
					GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) modeY);
				}
			} else {
				GL.GenTextures(1, out id);
				parentContext = GraphicsContext.CurrentContext;
				GL.BindTexture(TextureTarget.Texture2D, id);
				if (BitmapSize == TextureSize) {
					BitmapData data = image.LockBits(new Rectangle(Point.Empty, TextureSize), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
					GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, TextureSize.Width, TextureSize.Height, 0, TargetPixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
					image.UnlockBits(data);
				} else {
					using (Bitmap tempImage = new Bitmap(TextureSize.Width, TextureSize.Height, PixelFormat.Format32bppArgb)) {
						Rectangle rect = new Rectangle(Point.Empty, pad ? BitmapSize : TextureSize);
						using (Graphics g = Graphics.FromImage(tempImage)) {
							g.CompositingMode = CompositingMode.SourceCopy;
							g.CompositingQuality = CompositingQuality.HighQuality;
							g.InterpolationMode = InterpolationMode.HighQualityBicubic;
							g.PixelOffsetMode = PixelOffsetMode.HighQuality;
							g.DrawImage(image, rect);
						}
						BitmapData data = tempImage.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
						GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, TextureSize.Width, TextureSize.Height, 0, TargetPixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
						tempImage.UnlockBits(data);
					}
				}
				if (BindAction == ImageParameterAction.Dispose) {
					BindAction = ImageParameterAction.RemoveReference;
					image.Dispose();
				} else if (BindAction == ImageParameterAction.KeepReference)
					copy = image;
				image = null;
				if (!MipmapSupported.HasValue)
					MipmapSupported = GL.Delegates.glGenerateMipmap != null;
				bool mipmap = UseMipmapping;
				if (mipmap && !MipmapSupported.Value)
					mipmap = false;
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) (LinearScalingFilter ? (mipmap ? TextureMinFilter.LinearMipmapLinear : TextureMinFilter.Linear) : (mipmap ? TextureMinFilter.NearestMipmapLinear : TextureMinFilter.Nearest)));
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) (LinearScalingFilter ? TextureMagFilter.Linear : TextureMagFilter.Nearest));
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) modeX);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) modeY);
				lastTextureWrapModeX = modeX;
				lastTextureWrapModeY = modeY;
				if (mipmap) {
					try {
						GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
					} catch {
						if (GraphicsContext.IsGraphicsContextAvailable) {
							mipmap = false;
							MipmapSupported = false;
							UseMipmapping = false;
						}
					}
				}
			}
		}

		/// <summary>
		/// Unbinds the (any) texture (same as UnbindTexture2D())
		/// </summary>
		public void Unbind() {
			UnbindTexture2D();
		}

		/// <summary>
		/// Unbinds the (any) texture
		/// </summary>
		public static void UnbindTexture2D() {
			GL.BindTexture(TextureTarget.Texture2D, 0);
		}

		/// <summary>
		/// Updates the specified texture region
		/// </summary>
		/// <param name="source">A bitmap containing the region to update with</param>
		/// <param name="srcRegion">The region from the source bitmap to update with</param>
		/// <param name="textureRectLoc">The target texture coordinate to update the region at</param>
		public void UpdateRegion(Bitmap source, Rectangle srcRegion, Point textureRectLoc) {
			if (srcRegion.Width <= 0 || srcRegion.Height <= 0)
				return;
			Bind(TextureWrapMode.ClampToEdge);
			GL.PixelStore(PixelStoreParameter.UnpackRowLength, source.Width);
			BitmapData data = source.LockBits(srcRegion, ImageLockMode.ReadOnly, source.PixelFormat);
			GL.TexSubImage2D(TextureTarget.Texture2D, 0, textureRectLoc.X, textureRectLoc.Y, srcRegion.Width, srcRegion.Height, TargetPixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
			source.UnlockBits(data);
			GL.PixelStore(PixelStoreParameter.UnpackRowLength, 0);
		}

		/// <summary>
		/// Creates a TextureCollection instance containing the specified texture
		/// </summary>
		/// <param name="texture">The texture to initialize the collection from</param>
		public static implicit operator TextureCollection(Texture2D texture) {
			return new TextureCollection(texture);
		}

		/// <summary>
		/// Returns whether the texture is equal to another
		/// </summary>
		/// <param name="texture">The texture to check for equality to</param>
		/// <returns>Whether the texture is equal to another</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool Equals(Texture2D texture) {
			return texture != null && (id == texture.id && object.Equals(image, texture.image));
		}

		/// <summary>
		/// Returns whether the texture is equal to another
		/// </summary>
		/// <param name="obj">The texture to check for equality to</param>
		/// <returns>Whether the texture is equal to another</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool Equals(ITexture obj) {
			return Equals(obj as Texture2D);
		}

		/// <summary>
		/// Returns whether the texture is equal to another
		/// </summary>
		/// <param name="obj">The texture to check for equality to</param>
		/// <returns>Whether the texture is equal to another</returns>
		public override bool Equals(object obj) {
			return Equals(obj as Texture2D);
		}

		/// <summary>
		/// Returns the texture name
		/// </summary>
		/// <returns>The texture name</returns>
		public override int GetHashCode() {
			if (image == null)
				return id;
			else
				return id << 5 ^ image.GetHashCode();
		}

		/// <summary>
		/// Returns a string with the texture name
		/// </summary>
		/// <returns>A string with the texture name</returns>
		public override string ToString() {
			string name = Name;
			if (name != null)
				name = name.Trim();
			if (name == null || name.Length == 0)
				return "Texture2D: { Handle: " + id + " }";
			else
				return "Texture2D: { Handle: " + id + ", Name: " + name + " }";
		}

		/// <summary>
		/// Returns a copy of the current texture
		/// </summary>
		public object Clone() {
			return Clone(true);
		}

		/// <summary>
		/// Returns a copy of the current texture
		/// </summary>
		/// <param name="components">Whether to clone the internal components too</param>
		public ITexture Clone(bool components) {
			return new Texture2D(Image, pad ? NPotTextureScaleMode.Pad : (TextureSize == BitmapSize ? NPotTextureScaleMode.None : (TextureSize.Width <= BitmapSize.Width && TextureSize.Height <= BitmapSize.Height ? NPotTextureScaleMode.ScaleDown : NPotTextureScaleMode.ScaleUp)), BindAction, LinearScalingFilter, UseMipmapping);
		}

		/// <summary>
		/// Disposes of the texture and the resources consumed by it
		/// </summary>
		~Texture2D() {
			if (image != null) {
				if (BindAction == ImageParameterAction.Dispose) {
					BindAction = ImageParameterAction.RemoveReference;
					image.Dispose();
				}
				image = null;
			}
			copy = null;
			if (parentContext == null) {
				if (id != 0) {
					GraphicsContext.RaiseResourceLeakedEvent(this, LeakedWhile.Finalizing, new IntPtr(id));
					id = 0;
				}
			} else {
				int currentId = id;
				if (currentId != 0) {
					parentContext.InvokeOnGLThreadAsync(context => {
						try {
							GL.DeleteTextures(1, ref currentId);
						} catch {
							GraphicsContext.RaiseResourceLeakedEvent(this, LeakedWhile.Disposing, new IntPtr(currentId));
						}
					});
					id = 0;
				}
				parentContext = null;
			}
		}

		/// <summary>
		/// Disposes of the texture and the resources consumed by it
		/// </summary>
		public void Dispose() {
			Dispose(false);
		}

		/// <summary>
		/// Disposes of the texture and the resources consumed by it
		/// </summary>
		/// <param name="disposeChildren">Whether to dispose of the child components of the texture</param>
		public void Dispose(bool disposeChildren) {
			if (image != null) {
				if (BindAction == ImageParameterAction.Dispose) {
					BindAction = ImageParameterAction.RemoveReference;
					image.Dispose();
				}
				image = null;
			}
			copy = null;
			if (id != 0 && (parentContext == null || parentContext.IsCurrent)) {
				try {
					GL.DeleteTextures(1, ref id);
				} catch {
					GraphicsContext.RaiseResourceLeakedEvent(this, LeakedWhile.Disposing, new IntPtr(id));
				}
				id = 0;
				parentContext = null;
			}
			if (id == 0)
				GC.SuppressFinalize(this);
		}
	}
}