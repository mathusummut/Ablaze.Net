using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;

namespace System.Graphics.Models {
	using OGL;

	/// <summary>
	/// A managed wrapper for an OpenGL texture.
	/// </summary>
	public sealed class Texture2D : ITexture, IEquatable<Texture2D> {
		/// <summary>
		/// An empty texture.
		/// </summary>
		public static readonly Texture2D Empty = new Texture2D();
		/// <summary>
		/// A texture array containing one empty texture.
		/// </summary>
		public static readonly ITexture[] EmptyTexture = new ITexture[] { Empty };
		/// <summary>
		/// Whether to keep a managed copy of the images for quick retrieval.
		/// </summary>
		public static bool KeepBuffer;
		private int name, references;
		private bool init, loadedFromHandle, lastFilter = true, lastMipmap = true;
		private Bitmap image, copy;
		/// <summary>
		/// Increases texture interpolation quality by reducing shimmering caused by aliasing, but uses more memory.
		/// </summary>
		public bool UseMipmapping = true;
		private bool disposeImage, pad;
		/// <summary>
		/// Gets or sets whether to use a linear scaling filter or no filter on the texture.
		/// </summary>
		public bool LinearScalingFilter = true;

		/// <summary>
		/// Gets or sets whether the texture alpha components are premultiplied.
		/// </summary>
		public bool Premultiplied {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets additional info to be stored with the texture.
		/// </summary>
		public object Info {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the texture name.
		/// </summary>
		public string ID {
			get;
			set;
		}

		/// <summary>
		/// Gets the native OpenGL name of the texture (0 if not bound once yet).
		/// </summary>
		public int Name {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return name;
			}
		}

		/// <summary>
		/// Gets the size of the source bitmap.
		/// </summary>
		public Size BitmapSize {
			get;
			private set;
		}

		/// <summary>
		/// Gets the size of the texture.
		/// </summary>
		public Size TextureSize {
			get;
			private set;
		}

		/// <summary>
		/// Gets the last texture wrap mode used.
		/// </summary>
		public TextureWrapMode LastTextureWrapMode {
			get;
			private set;
		}

		/// <summary>
		/// Gets whether mipmapping is supported.
		/// </summary>
		public static bool? MipmapSupported {
			get;
			private set;
		}

		/// <summary>
		/// Gets the texture handle.
		/// </summary>
		public int Handle {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				if (name == 0 && image != null)
					Bind();
				return name;
			}
		}

		/// <summary>
		/// Gets the underlying bitmap of the texture.
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
				BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, Premultiplied ? Drawing.Imaging.PixelFormat.Format32bppPArgb : Drawing.Imaging.PixelFormat.Format32bppArgb);
				GL.GetTexImage(TextureTarget.Texture2D, 0, PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
				bmp.UnlockBits(data);
				return bmp;
			}
		}

		/// <summary>
		/// Gets whether the texture is disposed.
		/// </summary>
		public bool IsDisposed {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return name == 0 && image == null;
			}
		}

		/// <summary>
		/// Gets whether the texture has ever been bound.
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
		/// An empty texture.
		/// </summary>
		public Texture2D() {
		}

		/// <summary>
		/// Creates a new OpenGL texture from the specified bitmap image.
		/// </summary>
		/// <param name="image">The image to create the OpenGL texture from.</param>
		/// <param name="mode">Specifies which operation to perform on Non-Power-Of-Two-sized textures.
		/// If the texture has both dimensions that are a power of two, then no operation is performed.</param>
		/// <param name="disposeImage">Whether to dispose the pixel worker after the texture is created.</param>
		/// <param name="linearScalingFilter">Whether to use a linear scaling filter or no filter on the texture.</param>
		/// <param name="useMipmapping">Increases texture interpolation quality by reducing shimmering caused by aliasing, but uses more memory.</param>
		public Texture2D(Bitmap image, NPotTextureScaleMode mode, bool disposeImage, bool linearScalingFilter = true, bool useMipmapping = true) {
			if (image == null)
				return;
			this.image = image;
			Premultiplied = image.PixelFormat == Drawing.Imaging.PixelFormat.Format32bppPArgb;
			BitmapSize = image.Size;
			if (mode == NPotTextureScaleMode.Pad)
				pad = true;
			TextureSize = mode == NPotTextureScaleMode.None ? BitmapSize : ImageLib.GetPowerOfTwoSize(BitmapSize, mode == NPotTextureScaleMode.ScaleUp || pad);
			this.disposeImage = disposeImage;
			LinearScalingFilter = linearScalingFilter;
			lastFilter = linearScalingFilter;
			UseMipmapping = useMipmapping;
			lastMipmap = useMipmapping;
		}

		/// <summary>
		/// Creates a new OpenGL texture of the specified size.
		/// </summary>
		/// <param name="width">The width of the texture.</param>
		/// <param name="height">The height of the texture.</param>
		/// <param name="linearScalingFilter">Whether to use a linear scaling filter or no filter on the texture.</param>
		/// <param name="useMipmapping">Increases texture interpolation quality by reducing shimmering caused by aliasing, but uses more memory.</param>
		public Texture2D(int width, int height, bool linearScalingFilter = true, bool useMipmapping = true) {
			BitmapSize = new Size(width, height);
			TextureSize = BitmapSize;
			LinearScalingFilter = linearScalingFilter;
			lastFilter = linearScalingFilter;
			UseMipmapping = useMipmapping;
			lastMipmap = useMipmapping;
			init = true;
		}

		/// <summary>
		/// Initializes a texture wrapper using the specified handle.
		/// </summary>
		/// <param name="handle">The handle of the texture.</param>
		public Texture2D(int handle) {
			name = handle;
			loadedFromHandle = true;
		}

		/// <summary>
		/// Initializes textures from the specified file paths.
		/// </summary>
		/// <param name="images">The paths to the image files to load.</param>
		public static ITexture[] ToTextures(params string[] images) {
			if (images == null || images.Length == 0)
				return new ITexture[0];
			List<ITexture> textures = new List<ITexture>(images.Length);
			ITexture[] temp;
			for (int i = 0; i < images.Length; i++) {
				temp = Parsers.TextureParser.Parse(images[i]);
				if (temp != null)
					textures.AddRange(temp);
			}
			return textures.ToArray();
		}

		/// <summary>
		/// Initializes textures from the specified bitmaps using NPotTextureScaleMode.ScaleUp.
		/// </summary>
		/// <param name="images">The bitmaps to create textures from.</param>
		public static ITexture[] ToTextures(params Bitmap[] images) {
			if (images == null || images.Length == 0)
				return new ITexture[0];
			ITexture[] textures = new ITexture[images.Length];
			for (int i = 0; i < images.Length; i++)
				textures[i] = new Texture2D(images[i], NPotTextureScaleMode.ScaleUp, true);
			return textures;
		}

		/// <summary>
		/// Binds the texture for use with OpenGL operations.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Bind() {
			Bind(LastTextureWrapMode);
		}

		/// <summary>
		/// Binds the texture for use with OpenGL operations.
		/// </summary>
		/// <param name="mode">The texture wrap mode to use.</param>
		public void Bind(TextureWrapMode mode) {
			if (mode == 0 && LastTextureWrapMode == 0)
				mode = TextureWrapMode.Repeat;
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
					GL.GenTextures(1, out name);
					GL.BindTexture(TextureTarget.Texture2D, name);
					GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, TextureSize.Width, TextureSize.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);
					init = false;
				} else
					GL.BindTexture(TextureTarget.Texture2D, name);
				if (!(LinearScalingFilter == lastFilter || UseMipmapping == lastMipmap)) {
					lastFilter = LinearScalingFilter;
					bool mipmap = UseMipmapping;
					if (mipmap && !MipmapSupported.Value)
						mipmap = false;
					GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) (LinearScalingFilter ? (mipmap ? TextureMinFilter.LinearMipmapLinear : TextureMinFilter.Linear) : (mipmap ? TextureMinFilter.NearestMipmapLinear : TextureMinFilter.Nearest)));
					GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) (LinearScalingFilter ? TextureMagFilter.Linear : TextureMagFilter.Nearest));
				}
				if (LastTextureWrapMode != mode) {
					LastTextureWrapMode = mode;
					GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) mode);
					GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) mode);
				}
			} else {
				GL.GenTextures(1, out name);
				GL.BindTexture(TextureTarget.Texture2D, name);
				if (BitmapSize == TextureSize) {
					BitmapData data = image.LockBits(new Rectangle(Point.Empty, TextureSize), ImageLockMode.ReadOnly, Drawing.Imaging.PixelFormat.Format32bppArgb);
					GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, TextureSize.Width, TextureSize.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
					image.UnlockBits(data);
				} else {
					using (Bitmap tempImage = new Bitmap(TextureSize.Width, TextureSize.Height, Drawing.Imaging.PixelFormat.Format32bppArgb)) {
						Rectangle rect = new Rectangle(Point.Empty, pad ? BitmapSize : TextureSize);
						using (Drawing.Graphics g = Drawing.Graphics.FromImage(tempImage)) {
							g.CompositingMode = CompositingMode.SourceCopy;
							g.CompositingQuality = CompositingQuality.HighQuality;
							g.InterpolationMode = InterpolationMode.HighQualityBicubic;
							g.PixelOffsetMode = PixelOffsetMode.HighQuality;
							g.DrawImage(image, rect);
						}
						BitmapData data = tempImage.LockBits(rect, ImageLockMode.ReadOnly, Drawing.Imaging.PixelFormat.Format32bppArgb);
						GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, TextureSize.Width, TextureSize.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
						tempImage.UnlockBits(data);
					}
				}
				if (disposeImage) {
					image.Dispose();
					disposeImage = false;
				} else if (KeepBuffer)
					copy = image;
				image = null;
				if (!MipmapSupported.HasValue)
					MipmapSupported = GL.Delegates.glGenerateMipmap != null;
				bool mipmap = UseMipmapping;
				if (mipmap && !MipmapSupported.Value)
					mipmap = false;
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) (LinearScalingFilter ? (mipmap ? TextureMinFilter.LinearMipmapLinear : TextureMinFilter.Linear) : (mipmap ? TextureMinFilter.NearestMipmapLinear : TextureMinFilter.Nearest)));
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) (LinearScalingFilter ? TextureMagFilter.Linear : TextureMagFilter.Nearest));
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) mode);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) mode);
				LastTextureWrapMode = mode;
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
		/// Unbinds the (any) texture.
		/// </summary>
		public void Unbind() {
			GL.BindTexture(TextureTarget.Texture2D, 0);
		}

		/// <summary>
		/// Updates the specified texture region.
		/// </summary>
		/// <param name="source">A bitmap containing the region to update with.</param>
		/// <param name="srcRegion">The region from the source bitmap to update with.</param>
		/// <param name="textureRectLoc">The target texture coordinate to update the region at.</param>
		public void UpdateRegion(Bitmap source, Rectangle srcRegion, Point textureRectLoc) {
			Bind(TextureWrapMode.ClampToEdge);
			GL.PixelStore(PixelStoreParameter.UnpackRowLength, source.Width);
			BitmapData data = source.LockBits(srcRegion, ImageLockMode.ReadOnly, source.PixelFormat);
			GL.TexSubImage2D(TextureTarget.Texture2D, 0, textureRectLoc.X, textureRectLoc.Y, srcRegion.Width, srcRegion.Height, PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
			source.UnlockBits(data);
			GL.PixelStore(PixelStoreParameter.UnpackRowLength, 0);
		}

		/// <summary>
		/// Returns whether the texture is equal to another.
		/// </summary>
		/// <param name="texture">The texture to check for equality to.</param>
		/// <returns>Whether the texture is equal to another.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool Equals(Texture2D texture) {
			return texture != null && (name == texture.name && object.Equals(image, texture.image));
		}

		/// <summary>
		/// Returns whether the texture is equal to another.
		/// </summary>
		/// <param name="obj">The texture to check for equality to.</param>
		/// <returns>Whether the texture is equal to another.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool Equals(ITexture obj) {
			return Equals(obj as Texture2D);
		}

		/// <summary>
		/// Returns whether the texture is equal to another.
		/// </summary>
		/// <param name="obj">The texture to check for equality to.</param>
		/// <returns>Whether the texture is equal to another.</returns>
		public override bool Equals(object obj) {
			return Equals(obj as Texture2D);
		}

		/// <summary>
		/// Returns the texture name.
		/// </summary>
		/// <returns>The texture name.</returns>
		public override int GetHashCode() {
			if (image == null)
				return name;
			else
				return name << 5 ^ image.GetHashCode();
		}

		/// <summary>
		/// Returns a string with the texture name.
		/// </summary>
		/// <returns>A string with the texture name.</returns>
		public override string ToString() {
			string str = ID == null ? null : ID.Trim();
			if (str == null || str.Length == 0)
				return "{ Handle: " + name + " }";
			else
				return "{ Handle: " + name + ", Name: " + str + " }";
		}

		/// <summary>
		/// Adds a reference to this texture.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void AddReference() {
			references++;
		}

		/// <summary>
		/// Disposes of the texture and the resources consumed by it.
		/// </summary>
		~Texture2D() {
			if (image != null) {
				if (disposeImage) {
					image.Dispose();
					disposeImage = false;
				}
				image = null;
			}
			if (name != 0) {
				GraphicsContext.RaiseResourceLeakedEvent(this, LeakedWhile.Finalizing, new IntPtr(name));
				name = 0;
			}
		}

		/// <summary>
		/// Disposes of the texture and the resources consumed by it.
		/// </summary>
		public void Dispose() {
			Dispose(false);
		}

		/// <summary>
		/// Disposes of the texture and the resources consumed by it.
		/// </summary>
		/// <param name="forceDispose">If true, the reference count is ignored, forcing the texture to be disposed, unless it is already disposed.</param>
		public void Dispose(bool forceDispose) {
			if (references > 0)
				references--;
			if (references <= 0 || forceDispose) {
				if (image != null) {
					if (disposeImage) {
						image.Dispose();
						disposeImage = false;
					}
					image = null;
				}
				if (name != 0) {
					try {
						GL.DeleteTextures(1, ref name);
					} catch {
						GraphicsContext.RaiseResourceLeakedEvent(this, LeakedWhile.Disposing, new IntPtr(name));
					}
					name = 0;
				}
				GC.SuppressFinalize(this);
			}
		}
	}

	/// <summary>
	/// Specifies which operation to perform on Non-Power-Of-Two-sized textures.
	/// </summary>
	public enum NPotTextureScaleMode {
		/// <summary>
		/// Performs no operation (allows non-power of two textures to be used).
		/// </summary>
		None = 0,
		/// <summary>
		/// Stretches the texture to dimensions that are a power of two and larger or equal than the current texture dimensions.
		/// </summary>
		ScaleUp,
		/// <summary>
		/// Stretches the texture to dimensions that are a power of two and smaller or equal than the current texture dimensions.
		/// </summary>
		ScaleDown,
		/// <summary>
		/// Pads the texture with zeros until its dimensions become a power of two.
		/// </summary>
		Pad
	}
}