using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.Drawing {
	/// <summary>
	/// Determines what to do with the image after being passed as a parameter
	/// </summary>
	public enum ImageParameterAction {
		/// <summary>
		/// This is the default action, where the image reference is removed when it is no longer needed
		/// </summary>
		RemoveReference,
		/// <summary>
		/// A reference to the image will be kept for faster and easier retrieval
		/// </summary>
		KeepReference,
		/// <summary>
		/// The image will be automatically disposed when it is no longer used
		/// </summary>
		Dispose
	}

	/// <summary>
	/// Wraps a bitmap image to optimize for fast lock-free thread-safe pixel processing.
	/// Supports 32-bit BGRA, 24-bit BGR and 8-bit grayscale bitmaps.
	/// You can use foreach to iterate through all bytes/pixels of the enumerator with maximum speed.
	/// Enumeration type can be configured in ThreadLocal variables that are present as fields of the instance.
	/// </summary>
	public sealed class PixelWorker : IEnumerable<IntPtr>, IEnumerable, ICloneable, IDisposable {
		private static ConcurrentDictionary<Bitmap, PixelWorker> WrapperCollection = new ConcurrentDictionary<Bitmap, PixelWorker>();
		private int referenceCounter = 1;
		private Bitmap image;
		/// <summary>
		/// The pixel format of the image.
		/// </summary>
		private PixelFormat format;
		/// <summary>
		/// The size of the bitmap.
		/// </summary>
		private Size size;
		/// <summary>
		/// The image rectangle.
		/// </summary>
		private Rectangle bounds;
		/// <summary>
		/// What to do when the passed image is no longer used
		/// </summary>
		public ImageParameterAction ImageAction = ImageParameterAction.Dispose;
		/// <summary>
		/// The pixel components available in the buffer. If no buffering is used, this will be null.
		/// </summary>
		public byte[] Buffer;
		/// <summary>
		/// Points to the start of the native image (if any).
		/// </summary>
		private unsafe byte* scan0;
		/// <summary>
		/// Whether to skip alpha components during iteration. This is stored locally per thread.
		/// </summary>
		public readonly ThreadLocal<bool> SkipAlphaInIterator = new ThreadLocal<bool>();
		/// <summary>
		/// The enumarator step in bytes (components). The default byte increment is 1.
		/// If you set it to ComponentCount, the iteration will be for all pixels instead, and you can use a BgraColor to get the current pixel.
		/// </summary>
		public readonly ThreadLocal<int> IterationStep = new ThreadLocal<int>();
		private BitmapData bitmapData;
		private int stride, componentCount, width, height, pixelCount, pixelComponentCount, widthComponentCount, heightComponentCount;
		private bool writeOnDispose;
		/// <summary>
		/// For recreational purposes.
		/// </summary>
		public object Tag;
		/// <summary>
		/// The lock used to synchronize bitmap locking and unlocking.
		/// </summary>
		public object LockSync = new object();

		/// <summary>
		/// Gets the width of the image in pixels.
		/// </summary>
		public int Width {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return width;
			}
		}

		/// <summary>
		/// Gets the height of the image in pixels.
		/// </summary>
		public int Height {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return height;
			}
		}

		/// <summary>
		/// Gets the size of the bitmap.
		/// </summary>
		public Size Size {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return size;
			}
		}
		
		/// <summary>
		/// Gets the image rectangle.
		/// </summary>
		public Rectangle Bounds {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return bounds;
			}
		}

		/// <summary>
		/// Gets the number of pixels in the image (precalculated Width * Height).
		/// </summary>
		public int PixelCount {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return pixelCount;
			}
		}

		/// <summary>
		/// Gets the pixel format of the image.
		/// </summary>
		public PixelFormat Format {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return format;
			}
		}

		/// <summary>
		/// Gets the pixel format of the image (same as Format).
		/// </summary>
		public PixelFormat PixelFormat {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return format;
			}
		}

		/// <summary>
		/// Gets the number of components or channels per pixel.
		/// </summary>
		public int ComponentCount {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return componentCount;
			}
		}

		/// <summary>
		/// Gets the total number of components or channels (precalculated PixelCount * ComponentCount).
		/// </summary>
		public int PixelComponentCount {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return pixelComponentCount;
			}
		}

		/// <summary>
		/// Precalculated Width * ComponentCount.
		/// </summary>
		public int WidthComponentCount {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return widthComponentCount;
			}
		}

		/// <summary>
		/// Precalculated Height * ComponentCount.
		/// </summary>
		public int HeightComponentCount {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return heightComponentCount;
			}
		}

		/// <summary>
		/// Points to the start of the native image (null if buffer is used).
		/// </summary>
		[CLSCompliant(false)]
		public unsafe byte* Scan0 {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return scan0;
			}
		}

		/// <summary>
		/// Gets the pixel stride length.
		/// </summary>
		public int Stride {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return stride;
			}
		}

		/// <summary>
		/// Gets or sets the value of the specified pixel component.
		/// </summary>
		/// <param name="index">The byte index of the value to get/set.</param>
		public byte this[int index] {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				if (Buffer == null) {
					unsafe
					{
						return scan0[index];
					}
				} else
					return Buffer[index];
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				if (Buffer == null) {
					unsafe
					{
						scan0[index] = value;
					}
				} else
					Buffer[index] = value;
			}
		}

		/// <summary>
		/// Gets or sets the color of the specified pixel.
		/// </summary>
		/// <param name="x">The x-coordinate of the pixel to get/set.</param>
		/// <param name="y">The y-coordinate of the pixel to get/set.</param>
		public BgraColor this[int x, int y] {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return GetPixelBgra(((y * width) + x) * componentCount);
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				SetPixel(((y * width) + x) * componentCount, value);
			}
		}

		/// <summary>
		/// Gets or sets the value of the specified pixel component.
		/// </summary>
		/// <param name="x">The x-coordinate of the pixel.</param>
		/// <param name="y">The y-coordinate of the pixel.</param>
		/// <param name="c">The component index.</param>
		public byte this[int x, int y, int c] {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return this[((y * width) + x) * componentCount + c];
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				this[((y * width) + x) * componentCount + c] = value;
			}
		}

		/// <summary>
		/// Gets or sets the value of the specified pixel component.
		/// </summary>
		/// <param name="index">The byte index of the value to get/set.</param>
		public byte this[long index] {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				if (Buffer == null) {
					unsafe
					{
						return scan0[index];
					}
				} else
					return Buffer[index];
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				if (Buffer == null) {
					unsafe
					{
						scan0[index] = value;
					}
				} else
					Buffer[index] = value;
			}
		}

		/// <summary>
		/// Gets or sets the value of the specified pixel component.
		/// </summary>
		/// <param name="x">The x-coordinate of the pixel.</param>
		/// <param name="y">The y-coordinate of the pixel.</param>
		/// <param name="c">The component index.</param>
		public byte this[long x, long y, long c] {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return this[((y * width) + x) * componentCount + c];
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				this[((y * width) + x) * componentCount + c] = value;
			}
		}

		/// <summary>
		/// Gets whether changing pixel values are written to a buffer instead of directly to the image.
		/// </summary>
		public bool UsingBuffer {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return Buffer != null;
			}
		}

		/// <summary>
		/// Gets whether the image is currently locked.
		/// </summary>
		public bool IsLocked {
			get;
			private set;
		}

		/// <summary>
		/// Gets whether the image is disposed.
		/// </summary>
		public bool IsDisposed {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return Volatile.Read(ref referenceCounter) <= 0;
			}
		}

		/// <summary>
		/// Initializes a new PixelProcessingwrapper at the specified size.
		/// </summary>
		/// <param name="size">The size of the image.</param>
		public PixelWorker(Size size) : this(size.Width, size.Height) {
		}

		/// <summary>
		/// Initializes a new BGRA PixelProcessingwrapper at the specified size.
		/// </summary>
		/// <param name="width">The width of the image.</param>
		/// <param name="height">The height of the image.</param>
		public PixelWorker(int width, int height) : this(width, height, 4) {
		}

		/// <summary>
		/// Initializes a new PixelProcessingwrapper at the specified size.
		/// </summary>
		/// <param name="size">The size of the image.</param>
		/// <param name="pixelChannels">The number of channels per pixel (must be 1 (grayscale), 3 (BGR) or 4 (BGRA)).</param>
		public PixelWorker(Size size, int pixelChannels) : this(size.Width, size.Height, pixelChannels) {
		}

		/// <summary>
		/// Initializes a new PixelProcessingwrapper at the specified size.
		/// </summary>
		/// <param name="width">The width of the image.</param>
		/// <param name="height">The height of the image.</param>
		/// <param name="pixelChannels">The number of channels per pixel (must be 1 (grayscale), 3 (BGR) or 4 (BGRA)).</param>
		public PixelWorker(int width, int height, int pixelChannels) : this(new byte[width * height * pixelChannels], width, height, false) {
		}

		/// <summary>
		/// Initializes a new PixelProcessingwrapper at the specified size.
		/// </summary>
		/// <param name="size">The size of the image.</param>
		/// <param name="pixelChannels">The number of channels per pixel (must be 8bpp, 24bpp or 32bpp).</param>
		public PixelWorker(Size size, PixelFormat pixelChannels) : this(size.Width, size.Height, pixelChannels) {
		}

		/// <summary>
		/// Initializes a new PixelProcessingwrapper at the specified size.
		/// </summary>
		/// <param name="width">The width of the image.</param>
		/// <param name="height">The height of the image.</param>
		/// <param name="pixelChannels">The number of channels per pixel (must be 8bpp, 24bpp or 32bpp).</param>
		public PixelWorker(int width, int height, PixelFormat pixelChannels) : this(new byte[width * height * Image.GetPixelFormatSize(pixelChannels) / 8], width, height, false) {
		}

		/// <summary>
		/// Creates a new PixelWorker instance that is a copy of the specified instance.
		/// </summary>
		/// <param name="toCopy">The instance to copy.</param>
		public PixelWorker(PixelWorker toCopy) : this(toCopy == null ? null : toCopy.ToByteArray(true), toCopy == null ? 0 : toCopy.width, toCopy == null ? 0 : toCopy.height, false) {
		}

		/// <summary>
		/// Initializes a new PixelWorker from the specified image path.
		/// </summary>
		/// <param name="imagePath">The path to the image to load from.</param>
		/// <param name="useBuffer">Whether changing pixel values are written to a buffer instead of directly to an image (false usually means faster).</param>
		public PixelWorker(string imagePath, bool useBuffer = false) : this(Extensions.ImageFromFile(imagePath), useBuffer, false, ImageParameterAction.Dispose) {
		}

		/// <summary>
		/// Initializes a new PixelWorker from the specified image path.
		/// </summary>
		/// <param name="stream">The stream to load the image from.</param>
		/// <param name="useBuffer">Whether changing pixel values are written to a buffer instead of directly to an image (false usually means faster).</param>
		public PixelWorker(Stream stream, bool useBuffer = false) : this(new Bitmap(stream), useBuffer, false, ImageParameterAction.Dispose) {
		}

		private PixelWorker(Bitmap image, bool useBuffer, bool writeOnDispose, ImageParameterAction imageAction, bool lockBitmap = true) : this(image, new Rectangle(Point.Empty, image.Size), useBuffer, writeOnDispose, imageAction, lockBitmap) {
		}

		private PixelWorker(Bitmap image, Rectangle rect, bool useBuffer, bool writeOnDispose, ImageParameterAction imageAction, bool lockBitmap = true) {
			ImageAction = imageAction;
			this.writeOnDispose = writeOnDispose;
			this.image = image;
			format = image.PixelFormat;
			WrapperCollection.TryAdd(image, this);
			width = rect.Width;
			height = rect.Height;
			size = rect.Size;
			bounds = rect;
			pixelCount = rect.Width * rect.Height;
			componentCount = Image.GetPixelFormatSize(format) / 8;
			pixelComponentCount = pixelCount * componentCount;
			widthComponentCount = rect.Width * componentCount;
			heightComponentCount = rect.Height * componentCount;
			stride = ((widthComponentCount + 3) / 4) * 4;
			if (useBuffer || stride != widthComponentCount)
				LoadBuffer();
			else if (lockBitmap)
				LockBits(ImageLockMode.ReadWrite);
		}

		/// <summary>
		/// Initializes a new PixelProcessingwrapper from the specified pixel values.
		/// </summary>
		/// <param name="pixels">The pixel channels array of the image.</param>
		/// <param name="width">The width of the image.</param>
		/// <param name="height">The height of the image.</param>
		/// <param name="normalize">Whether to normalize the values to 255 before use.</param>
		public PixelWorker(float[] pixels, int width, int height, bool normalize = true) : this(ConvertToBytes(pixels, normalize), width, height, false) {
		}

		/// <summary>
		/// Initializes a new PixelProcessingwrapper from the pixel values.
		/// </summary>
		/// <param name="pixels">The pixel channels array of the image.</param>
		/// <param name="width">The width of the image.</param>
		/// <param name="height">The height of the image.</param>
		/// <param name="copyPixelArray">Whether to copy the pixel array or just reference it.</param>
		public PixelWorker(byte[] pixels, int width, int height, bool copyPixelArray) {
			if (pixels == null)
				throw new ArgumentNullException(nameof(pixels), "The array to initialize from cannot be null.");
			pixelComponentCount = pixels.Length;
			this.width = width;
			this.height = height;
			size = new Size(width, height);
			bounds = new Rectangle(0, 0, width, height);
			pixelCount = width * height;
			componentCount = pixelComponentCount / pixelCount;
			switch (componentCount) {
				case 4:
					format = PixelFormat.Format32bppArgb;
					break;
				case 3:
					format = PixelFormat.Format24bppRgb;
					break;
				case 1:
					format = PixelFormat.Format8bppIndexed;
					break;
				default:
					throw new ArgumentException("Only 8, 24 and 32 bpp images are officially supported.");
			}
			widthComponentCount = width * componentCount;
			stride = ((widthComponentCount + 3) / 4) * 4;
			heightComponentCount = height * componentCount;
			if (copyPixelArray) {
				Buffer = new byte[pixelComponentCount];
				System.Buffer.BlockCopy(pixels, 0, Buffer, 0, pixelComponentCount);
			} else
				Buffer = pixels;
		}

		/// <summary>
		/// Initializes a new PixelProcessingwrapper from the given locked bitmap data.
		/// </summary>
		/// <param name="bitmapData">The pixel channels array of the image.</param>
		/// <param name="useBuffer">Whether changing pixel values are written to a buffer instead of directly to the image (false usually means faster).</param>
		/// <param name="writeOnDispose">Whether to save changes on dispose.</param>
		public PixelWorker(BitmapData bitmapData, bool useBuffer, bool writeOnDispose) {
			if (bitmapData == null)
				throw new ArgumentNullException(nameof(bitmapData), "The bitmapData to initialize from cannot be null.");
			this.writeOnDispose = writeOnDispose;
			this.bitmapData = bitmapData;
			format = bitmapData.PixelFormat;
			int depth = Image.GetPixelFormatSize(format);
			if (!(depth == 32 || depth == 24 || depth == 8))
				throw new ArgumentException("Only 8, 24 and 32 bpp images are officially supported.");
			width = bitmapData.Width;
			height = bitmapData.Height;
			size = new Size(width, height);
			bounds = new Rectangle(0, 0, width, height);
			componentCount = depth / 8;
			pixelCount = width * height;
			pixelComponentCount = pixelCount * componentCount;
			widthComponentCount = width * componentCount;
			heightComponentCount = height * componentCount;
			stride = bitmapData.Stride;
			unsafe
			{
				scan0 = (byte*) bitmapData.Scan0;
			}
			if (useBuffer || stride != widthComponentCount) {
				Buffer = new byte[pixelComponentCount];
				unsafe
				{
					byte* i = scan0;
					for (int y = 0; y < pixelComponentCount; y += widthComponentCount) {
						Marshal.Copy(new IntPtr(i), Buffer, y, widthComponentCount);
						i += stride;
					}
				}
			}
		}

		/// <summary>
		/// Initializes a PixelWorker from the specified image.
		/// </summary>
		/// <param name="image">The image to get the required data from.</param>
		/// <param name="useBuffer">Whether changing pixel values are written to a buffer instead of directly to the image (false means faster).</param>
		/// <param name="writeOnDispose">Whether to write changes on dispose.</param>
		/// <param name="imageAction">What to do when the passed image is no longer used</param>
		/// <param name="doNotUseImageDirectly">If true, the image is copied into a buffer and the original is left untouched.</param>
		/// <param name="lockBitmap">For internal use only.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static PixelWorker FromImage(Bitmap image, bool useBuffer, bool writeOnDispose, ImageParameterAction imageAction = ImageParameterAction.RemoveReference, bool doNotUseImageDirectly = false, bool lockBitmap = true) {
			if (image == null)
				throw new ArgumentNullException(nameof(image), "The image cannot be null.");
			else if (image.IsDisposed())
				throw new ArgumentException("Cannot load a worker from a disposed image.", nameof(image));
			else
				return FromImage(image, new Rectangle(Point.Empty, image.Size), useBuffer, writeOnDispose, imageAction, doNotUseImageDirectly, lockBitmap);
		}

		/// <summary>
		/// Initializes a PixelWorker from the specified image. If the bit depth is not supported (not 8, 24 or 32), then null is returned.
		/// </summary>
		/// <param name="image">The image to get the required data from.</param>
		/// <param name="rect">The region to extract from the image for the pixel worker.</param>
		/// <param name="useBuffer">Whether changing pixel values are written to a buffer instead of directly to the image (false means faster).</param>
		/// <param name="writeOnDispose">Whether to write changes on dispose.</param>
		/// <param name="imageAction">Whether to dispose the image as well on dispose.</param>
		/// <param name="doNotUseImageDirectly">If true, the image is copied into a buffer and the original is left untouched.</param>
		/// <param name="lockBitmap">For internal use only.</param>
		public static PixelWorker FromImage(Bitmap image, Rectangle rect, bool useBuffer, bool writeOnDispose, ImageParameterAction imageAction = ImageParameterAction.RemoveReference, bool doNotUseImageDirectly = false, bool lockBitmap = true) {
			if (image == null)
				throw new ArgumentNullException(nameof(image), "The image cannot be null.");
			else if (image.IsDisposed())
				throw new ArgumentException("Cannot load a worker from a disposed image.", nameof(image));
			PixelWorker instance;
			if (!doNotUseImageDirectly && WrapperCollection.TryGetValue(image, out instance)) {
				if (instance.size != rect.Size)
					throw new ArgumentException("The image already has a pixel worker initialized from it.", nameof(image));
				Interlocked.Increment(ref instance.referenceCounter);
				if (imageAction == ImageParameterAction.KeepReference || instance.ImageAction == ImageParameterAction.KeepReference)
					instance.ImageAction = ImageParameterAction.KeepReference;
				else if (!(instance.ImageAction == ImageParameterAction.Dispose && imageAction == ImageParameterAction.Dispose))
					instance.ImageAction = ImageParameterAction.RemoveReference;
				if (instance.Buffer == null && useBuffer)
					instance.LoadBuffer();
				if (!instance.writeOnDispose)
					instance.writeOnDispose = writeOnDispose;
				return instance;
			} else {
				PixelFormat format = image.PixelFormat;
				int depth = Image.GetPixelFormatSize(format);
				if (!(depth == 32 || depth == 24 || depth == 8))
					throw new ArgumentException("Only 8, 24 and 32 bpp images are officially supported.");
				else if (doNotUseImageDirectly) {
					BitmapData data = image.LockBits(rect, ImageLockMode.ReadOnly, image.PixelFormat);
					int componentCount = depth / 8;
					byte[] buffer = new byte[rect.Width * rect.Height * componentCount];
					int widthComponentCount = rect.Width * componentCount;
					int pixelComponentCount = widthComponentCount * rect.Height;
					unsafe
					{
						byte* i = (byte*) data.Scan0;
						for (int y = 0; y < pixelComponentCount; y += widthComponentCount) {
							Marshal.Copy(new IntPtr(i), buffer, y, widthComponentCount);
							i += data.Stride;
						}
					}
					image.UnlockBits(data);
					if (imageAction == ImageParameterAction.Dispose)
						image.DisposeSafe();
					return new PixelWorker(buffer, rect.Width, rect.Height, false);
				} else
					return new PixelWorker(image, rect, useBuffer, writeOnDispose, imageAction, lockBitmap);
			}
		}

		/// <summary>
		/// Gets a copy of the image represented by this instance.
		/// </summary>
		public Bitmap ToBitmap() {
			Bitmap copy = new Bitmap(width, height, format);
			BitmapData data = copy.LockBits(bounds, ImageLockMode.WriteOnly, format);
			unsafe
			{
				if (UsingBuffer || image == null) {
					byte* i = (byte*) data.Scan0;
					for (int y = 0; y < pixelComponentCount; y += widthComponentCount) {
						Marshal.Copy(Buffer, y, new IntPtr(i), widthComponentCount);
						i += data.Stride;
					}
				} else {
					byte* iSrc = scan0;
					byte* iDest = (byte*) data.Scan0;
					for (int y = 0; y < height; y++) {
						Extensions.MemoryCopy(iSrc, iDest, (uint) widthComponentCount);
						iSrc += stride;
						iDest += data.Stride;
					}
				}
			}
			copy.UnlockBits(data);
			if (format == PixelFormat.Format8bppIndexed) {
				ColorPalette palette = copy.Palette;
				Color[] entries = palette.Entries;
				for (int i = 0; i < 256; i++)
					entries[i] = Color.FromArgb(i, i, i);
				copy.Palette = palette;
			}
			return copy;
		}

		/// <summary>
		/// Initializes a PixelWorker from the Bitmap using default values.
		/// </summary>
		/// <param name="image">The image to initialize from.</param>
		public static explicit operator PixelWorker(Bitmap image) {
			return FromImage(image, false, true);
		}

		/// <summary>
		/// Gets the underlying bitmap of the PixelWorker instance.
		/// WARNING: IMAGE MAY BE LOCKED.
		/// </summary>
		/// <param name="worker">The worker whose image to obtain.</param>
		public static explicit operator Bitmap(PixelWorker worker) {
			if (worker.image == null)
				worker.image = worker.ToBitmap();
			return worker.image;
		}

		/// <summary>
		/// DO NOT USE UNLESS NECESSARY. Refills the buffer from the bitmap memory.
		/// </summary>
		private void LoadBuffer() {
			if (image == null)
				return;
			lock (LockSync) {
				if (image == null || image.IsDisposed() || Buffer != null)
					return;
				Buffer = new byte[pixelComponentCount];
				if (!IsLocked) {
					IsLocked = true;
					bitmapData = image.LockBits(bounds, ImageLockMode.ReadOnly, format);
					unsafe
					{
						scan0 = (byte*) bitmapData.Scan0;
					}
					stride = bitmapData.Stride;
				}
				unsafe
				{
					byte* i = (byte*) bitmapData.Scan0;
					for (int y = 0; y < pixelComponentCount; y += widthComponentCount) {
						Marshal.Copy(new IntPtr(i), Buffer, y, widthComponentCount);
						i += stride;
					}
				}
				image.UnlockBits(bitmapData);
				IsLocked = false;
				if (ImageAction == ImageParameterAction.Dispose) {
					image.Dispose();
					image = null;
				} else if (ImageAction == ImageParameterAction.RemoveReference)
					image = null;
			}
		}

		/// <summary>
		/// Gets the pixels as a byte array.
		/// </summary>
		/// <param name="copyIfBuffer">If true and UsingBuffer is true, then the buffer is copied.</param>
		public byte[] ToByteArray(bool copyIfBuffer = false) {
			if (Buffer == null) {
				byte[] copy = new byte[pixelComponentCount];
				unsafe
				{
					Marshal.Copy(new IntPtr(scan0), copy, 0, pixelComponentCount);
				}
				return copy;
			} else if (copyIfBuffer) {
				byte[] copy = new byte[pixelComponentCount];
				System.Buffer.BlockCopy(Buffer, 0, copy, 0, pixelComponentCount);
				return copy;
			} else
				return Buffer;
		}

		/// <summary>
		/// Copies the pixel values from the specified bitmap into the buffer of this instance.
		/// </summary>
		/// <param name="image">The worker whose values to copy. DIMENSIONS MUST BE THE SAME AS THIS ONE.</param>
		public void CopyFrom(Bitmap image) {
			if (image == null)
				return;
			using (PixelWorker worker = FromImage(image, true, false, ImageParameterAction.RemoveReference, true))
				CopyFrom(worker.ToByteArray(false), false);
		}

		/// <summary>
		/// Copies the pixel values from the specified worker into the buffer of this instance.
		/// </summary>
		/// <param name="pixels">The worker whose values to copy. DIMENSIONS MUST BE THE SAME AS THIS ONE.</param>
		public void CopyFrom(PixelWorker pixels) {
			if (pixels != null)
				CopyFrom(pixels.ToByteArray(false), true);
		}

		/// <summary>
		/// Copies the specified pixel values into the buffer of this instance.
		/// </summary>
		/// <param name="values">The values to copy. Negative values will not render correctly. DIMENSIONS MUST BE THE SAME AS THIS ONE.</param>
		/// <param name="normalize">Whether to normalize the values to 255 before use.</param>
		public void CopyFrom(float[] values, bool normalize = true) {
			if (values == null)
				return;
			byte[] targetValues = Buffer == null ? new byte[values.Length] : Buffer;
			ConvertToBytes(values, targetValues, normalize);
			CopyFrom(targetValues, false);
		}

		private static byte[] ConvertToBytes(float[] values, bool normalize) {
			if (values == null)
				return null;
			byte[] targetValues = new byte[values.Length];
			ConvertToBytes(values, targetValues, normalize);
			return targetValues;
		}

		private static void ConvertToBytes(float[] values, byte[] targetValues, bool normalize) {
			if (normalize && values.Length != 0) {
				float current, currentMax = 0f, currentMin = values[0];
				int length = values.Length;
				for (int i = 0; i < length; ++i) {
					current = values[i];
					if (current > currentMax)
						currentMax = current;
					else if (current < currentMin)
						currentMin = current;
				}
				if (currentMax == currentMin)
					Extensions.Memset(targetValues, ImageLib.Clamp(currentMin), 0, targetValues.Length);
				else {
					currentMax = 255f / (currentMax - currentMin);
					ParallelLoop.For(0, values.Length, i => targetValues[i] = (byte) ((values[i] - currentMin) * currentMax));
				}
			} else {
				for (int i = 0; i < values.Length; ++i)
					targetValues[i] = ImageLib.Clamp(values[i]);
			}
		}

		/// <summary>
		/// Copies the specified pixel values into the buffer of this instance.
		/// </summary>
		/// <param name="pixels">The values to copy. DIMENSIONS MUST BE THE SAME AS THIS ONE.</param>
		/// <param name="copyPixelArray">If true, the buffer will be copied instead of simply referenced.</param>
		public void CopyFrom(byte[] pixels, bool copyPixelArray = true) {
			if (pixels == null)
				return;
			pixelComponentCount = pixels.Length;
			if (copyPixelArray) {
				if (Buffer == null)
					Buffer = new byte[pixelComponentCount];
				System.Buffer.BlockCopy(pixels, 0, Buffer, 0, pixelComponentCount);
			} else
				Buffer = pixels;
			componentCount = pixelComponentCount / pixelCount;
			switch (componentCount) {
				case 4:
					format = PixelFormat.Format32bppArgb;
					break;
				case 3:
					format = PixelFormat.Format24bppRgb;
					break;
				case 1:
					format = PixelFormat.Format8bppIndexed;
					break;
				default:
					throw new ArgumentException("Only 8, 24 and 32 bpp images are officially supported.");
			}
			widthComponentCount = width * componentCount;
			stride = ((widthComponentCount + 3) / 4) * 4;
			heightComponentCount = height * componentCount;
		}

		/// <summary>
		/// Gets the pixel index referred to by the specified coordinates.
		/// </summary>
		/// <param name="x">The X-coordinate of the pixel.</param>
		/// <param name="y">The Y-coordinate of the pixel.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public int GetPixelIndex(int x, int y) {
			return ((y * width) + x) * componentCount;
		}

		/// <summary>
		/// Gets the pixel point referred to by the specified pixel.
		/// </summary>
		/// <param name="index">The pixel number (horizontal scan).</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public Point GetPixelPointUsingPixelCount(int index) {
			return new Point(index % width, index / width);
		}

		/// <summary>
		/// Gets the pixel point referred to by the specified pixel.
		/// </summary>
		/// <param name="index">The pixel address (horizontal scan).</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public Point GetPixelPoint(int index) {
			index /= componentCount;
			return new Point(index % width, index / width);
		}

		/// <summary>
		/// Gets the color of specified pixel.
		/// </summary>
		/// <param name="pixel">The location of the pixel in the image.</param>
		/// <returns>The Color of the specified pixel in the image.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public Color GetPixel(Point pixel) {
			return GetPixel(pixel.X, pixel.Y);
		}

		/// <summary>
		/// Gets the color of specified pixel.
		/// </summary>
		/// <param name="pixel">The location of the pixel in the image.</param>
		/// <returns>The color of the specified pixel in the image.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public BgraColor GetPixelBgra(Point pixel) {
			return GetPixelBgra(pixel.X, pixel.Y);
		}

		/// <summary>
		/// Gets the color of specified pixel.
		/// </summary>
		/// <param name="x">The X-coordinate of the pixel in the image.</param>
		/// <param name="y">The Y-coordinate of the pixel in the image.</param>
		/// <returns>The Color of the specified pixel in the image.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public Color GetPixel(int x, int y) {
			return GetPixel(((y * width) + x) * componentCount);
		}

		/// <summary>
		/// Gets the color of specified pixel.
		/// </summary>
		/// <param name="x">The X-coordinate of the pixel in the image.</param>
		/// <param name="y">The Y-coordinate of the pixel in the image.</param>
		/// <returns>The color of the specified pixel in the image.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public BgraColor GetPixelBgra(int x, int y) {
			return GetPixelBgra(((y * width) + x) * componentCount);
		}

		/// <summary>
		/// Gets the color of specified pixel. If the pixel is out of range, the color of the closest pixel is returned.
		/// </summary>
		/// <param name="pixel">The location of the pixel in the image.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public Color GetPixelClamped(Point pixel) {
			return GetPixelClamped(pixel.X, pixel.Y);
		}

		/// <summary>
		/// Gets the color of specified pixel. If the pixel is out of range, the color of the closest pixel is returned.
		/// </summary>
		/// <param name="pixel">The location of the pixel in the image.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public BgraColor GetPixelClampedBgra(Point pixel) {
			return GetPixelClampedBgra(pixel.X, pixel.Y);
		}

		/// <summary>
		/// Gets the color of specified pixel. If the pixel is out of range, the color of the closest pixel is returned.
		/// </summary>
		/// <param name="x">The X-coordinate of the pixel in the image.</param>
		/// <param name="y">The Y-coordinate of the pixel in the image.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public Color GetPixelClamped(int x, int y) {
			if (x < 0)
				x = 0;
			else if (x >= width)
				x = width - 1;
			if (y < 0)
				y = 0;
			else if (y >= height)
				y = height - 1;
			return GetPixel(((y * width) + x) * componentCount);
		}

		/// <summary>
		/// Gets the color of specified pixel. If the pixel is out of range, the color of the closest pixel is returned.
		/// </summary>
		/// <param name="x">The X-coordinate of the pixel in the image.</param>
		/// <param name="y">The Y-coordinate of the pixel in the image.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public BgraColor GetPixelClampedBgra(int x, int y) {
			if (x < 0)
				x = 0;
			else if (x >= width)
				x = width - 1;
			if (y < 0)
				y = 0;
			else if (y >= height)
				y = height - 1;
			return GetPixelBgra(((y * width) + x) * componentCount);
		}

		/// <summary>
		/// Gets the color of specified pixel address.
		/// </summary>
		/// <param name="index">The index of the pixel to address.</param>
		/// <returns>The Color of the specified pixel in the image.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public Color GetPixel(int index) {
			switch (componentCount) {
				case 4:
					return Color.FromArgb(this[index + 3], this[index + 2], this[index + 1], this[index]);
				case 3:
					return Color.FromArgb(this[index + 2], this[index + 1], this[index]);
				case 1: {
						int c = this[index];
						return Color.FromArgb(c, c, c);
					}
				default:
					return Color.FromArgb(0, this[index + 1], this[index]);
			}
		}

		/// <summary>
		/// Gets the color of specified pixel.
		/// </summary>
		/// <param name="index">The index of the pixel to address.</param>
		/// <returns>The color of the specified pixel in the image.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public BgraColor GetPixelBgra(int index) {
			if (Buffer == null) {
				unsafe
				{
					return BgraColor.FromPointer(scan0 + index, componentCount);
				}
			} else {
				switch (componentCount) {
					case 4:
						return new BgraColor(Buffer[index + 3], Buffer[index + 2], Buffer[index + 1], Buffer[index]);
					case 3:
						return new BgraColor(Buffer[index + 2], Buffer[index + 1], Buffer[index]);
					case 1: {
							byte c = Buffer[index];
							return new BgraColor(c, c, c);
						}
					default:
						return new BgraColor(0, Buffer[index + 1], Buffer[index]);
				}
			}
		}

		/// <summary>
		/// Gets the color of specified pixel.
		/// </summary>
		/// <param name="index">The index from 0 to (pixelCount-1) of the pixel to address.</param>
		/// <returns>The Color of the specified pixel in the image.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public Color GetPixelUsingPixelCount(int index) {
			return GetPixel(index * componentCount);
		}

		/// <summary>
		/// Gets the color of specified pixel.
		/// </summary>
		/// <param name="index">The index from 0 to (pixelCount-1) of the pixel to address.</param>
		/// <returns>The color of the specified pixel in the image.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public BgraColor GetPixelUsingPixelCountBgra(int index) {
			return GetPixelBgra(index * componentCount);
		}

		/// <summary>
		/// Gets the color of specified pixel.
		/// </summary>
		/// <param name="index">The index from 0 to (pixelCount-1) of the pixel to address.</param>
		/// <returns>The Color of the specified pixel in the image.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public Color Get32BitPixelUsingPixelCount(int index) {
			index *= 4;
			return Color.FromArgb(this[index + 3], this[index + 2], this[index + 1], this[index]);
		}

		/// <summary>
		/// Gets the color of specified pixel.
		/// </summary>
		/// <param name="index">The index from 0 to (pixelCount-1) of the pixel to address.</param>
		/// <returns>The color of the specified pixel in the image.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public BgraColor Get32BitPixelUsingPixelCountBgra(int index) {
			return Get32BitPixelBgra(index * 4);
		}

		/// <summary>
		/// Gets the color of specified pixel.
		/// </summary>
		/// <param name="index">The index from 0 to (pixelCount-1) of the pixel to address.</param>
		/// <returns>The Color of the specified pixel in the image.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public Color Get24BitPixelUsingPixelCount(int index) {
			index *= 3;
			return Color.FromArgb(this[index + 2], this[index + 1], this[index]);
		}

		/// <summary>
		/// Gets the color of specified pixel.
		/// </summary>
		/// <param name="index">The index from 0 to (pixelCount-1) of the pixel to address.</param>
		/// <returns>The color of the specified pixel in the image.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public BgraColor Get24BitPixelUsingPixelCountBgra(int index) {
			return Get24BitPixelBgra(index * 3);
		}

		/// <summary>
		/// Gets the color of specified pixel.
		/// </summary>
		/// <param name="index">The index from 0 to (pixelCount-1) of the pixel to address.</param>
		/// <returns>The Color of the specified pixel in the image.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public Color Get8BitPixelUsingPixelCount(int index) {
			int c = this[index];
			return Color.FromArgb(c, c, c);
		}

		/// <summary>
		/// Gets the color of specified pixel.
		/// </summary>
		/// <param name="index">The index from 0 to (pixelCount-1) of the pixel to address.</param>
		/// <returns>The color of the specified pixel in the image.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public BgraColor Get8BitPixelUsingPixelCountBgra(int index) {
			byte c = this[index];
			return new BgraColor(c, c, c);
		}

		/// <summary>
		/// Gets the color of specified pixel.
		/// </summary>
		/// <param name="index">The index of the pixel to address.</param>
		/// <returns>The Color of the specified pixel in the image.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public Color Get32BitPixel(int index) {
			return Color.FromArgb(this[index + 3], this[index + 2], this[index + 1], this[index]);
		}

		/// <summary>
		/// Gets the color of specified pixel.
		/// </summary>
		/// <param name="index">The index of the pixel to address.</param>
		/// <returns>The color of the specified pixel in the image.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public BgraColor Get32BitPixelBgra(int index) {
			if (Buffer == null) {
				unsafe
				{
					return *((BgraColor*) (scan0 + index));
				}
			} else
				return new BgraColor(Buffer[index + 3], Buffer[index + 2], Buffer[index + 1], Buffer[index]);
		}

		/// <summary>
		/// Gets the color of specified pixel.
		/// </summary>
		/// <param name="index">The index of the pixel to address.</param>
		/// <returns>The Color of the specified pixel in the image.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public Color Get24BitPixel(int index) {
			return Color.FromArgb(this[index + 2], this[index + 1], this[index]);
		}

		/// <summary>
		/// Gets the color of specified pixel.
		/// </summary>
		/// <param name="index">The index of the pixel to address.</param>
		/// <returns>The color of the specified pixel in the image.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public BgraColor Get24BitPixelBgra(int index) {
			if (Buffer == null) {
				unsafe
				{
					byte* ptr = scan0 + index;
					return new BgraColor() {
						B = *ptr,
						G = *(ptr + 1),
						R = *(ptr + 2),
						A = 255
					};
				}
			} else
				return new BgraColor(Buffer[index + 2], Buffer[index + 1], Buffer[index]);
		}

		/// <summary>
		/// Gets whether the specified component is an alpha component.
		/// </summary>
		/// <param name="componentIndex">The index of the component.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool IsAlpha(int componentIndex) {
			return componentCount == 4 && componentIndex % 4 == 3;
		}

		/// <summary>
		/// Gets the color of specified pixel.
		/// </summary>
		/// <param name="index">The index of the pixel to address.</param>
		/// <returns>The Color of the specified pixel in the image.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public Color Get8BitPixel(int index) {
			int c = this[index];
			return Color.FromArgb(c, c, c);
		}

		/// <summary>
		/// Gets the color of specified pixel.
		/// </summary>
		/// <param name="index">The index of the pixel to address.</param>
		/// <returns>The color of the specified pixel in the image.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public BgraColor Get8BitPixelBgra(int index) {
			byte c = this[index];
			return new BgraColor(c, c, c);
		}

		/// <summary>
		/// Gets the color of specified pixel.
		/// </summary>
		/// <param name="x">The X-coordinate of the pixel in the image.</param>
		/// <param name="y">The Y-coordinate of the pixel in the image.</param>
		/// <returns>The Color of the specified pixel in the image.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public Color Get32BitPixel(int x, int y) {
			return Get32BitPixel((y * width + x) * 4);
		}

		/// <summary>
		/// Gets the color of specified pixel.
		/// </summary>
		/// <param name="x">The X-coordinate of the pixel in the image.</param>
		/// <param name="y">The Y-coordinate of the pixel in the image.</param>
		/// <returns>The color of the specified pixel in the image.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public BgraColor Get32BitPixelBgra(int x, int y) {
			return Get32BitPixelBgra((y * width + x) * 4);
		}

		/// <summary>
		/// Gets the color of specified pixel.
		/// </summary>
		/// <param name="x">The X-coordinate of the pixel in the image.</param>
		/// <param name="y">The Y-coordinate of the pixel in the image.</param>
		/// <returns>The Color of the specified pixel in the image.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public Color Get24BitPixel(int x, int y) {
			return Get24BitPixel((y * width + x) * 3);
		}

		/// <summary>
		/// Gets the color of specified pixel.
		/// </summary>
		/// <param name="x">The X-coordinate of the pixel in the image.</param>
		/// <param name="y">The Y-coordinate of the pixel in the image.</param>
		/// <returns>The color of the specified pixel in the image.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public BgraColor Get24BitPixelBgra(int x, int y) {
			return Get24BitPixelBgra((y * width + x) * 3);
		}

		/// <summary>
		/// Gets all the colors of pixels (slow).
		/// </summary>
		/// <returns>The array of pixel colors.</returns>
		public Color[] GetAllPixels() {
			Color[] colors = new Color[pixelCount];
			int i;
			switch (componentCount) {
				case 4:
					for (i = 0; i < pixelComponentCount; i += 4)
						colors[i >> 2] = Color.FromArgb(this[i + 3], this[i + 2], this[i + 1], this[i]);
					break;
				case 3:
					int temp = 0;
					for (i = 0; i < pixelComponentCount; i += 3, temp++)
						colors[temp] = Color.FromArgb(this[i + 2], this[i + 1], this[i]);
					break;
				case 1: {
						byte component;
						for (i = 0; i < pixelCount; i++) {
							component = this[i];
							colors[i] = Color.FromArgb(component, component, component);
						}
					}
					break;
				default:
					for (i = 0; i < pixelComponentCount; i += 2)
						colors[i >> 1] = Color.FromArgb(0, this[i + 1], this[i]);
					break;
			}
			return colors;
		}

		/// <summary>
		/// Gets all the colors of pixels.
		/// </summary>
		/// <returns>The array of pixel colors.</returns>
		public unsafe BgraColor[] GetAllPixelsBgra() {
			BgraColor[] colors = new BgraColor[pixelCount];
			using (PointerWrapper imgPtr = PinPointer()) {
				byte* ptr = (byte*) imgPtr.Pointer;
				int i;
				switch (componentCount) {
					case 4:
						BgraColor* color = (BgraColor*) ptr;
						for (i = 0; i < pixelCount; i++, color++)
							colors[i] = *color;
						break;
					case 3:
						for (i = 0; i < pixelCount; i++, ptr += 3)
							colors[i] = new BgraColor(*(ptr + 2), *(ptr + 1), *ptr);
						break;
					case 1: {
							byte component;
							for (i = 0; i < pixelCount; i++, ptr++) {
								component = *ptr;
								colors[i] = new BgraColor(component, component, component);
							}
						}
						break;
					default:
						for (i = 0; i < pixelCount; i++, ptr += 2)
							colors[i] = new BgraColor(0, *(ptr + 1), *ptr);
						break;
				}
			}
			return colors;
		}

		/// <summary>
		/// Clears all pixels and set all components to the specified value.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Clear(byte value) {
			if (Buffer == null) {
				unsafe
				{
					Extensions.Memset(new IntPtr(scan0), value, pixelComponentCount);
				}
			} else
				Extensions.Memset(Buffer, value, 0, pixelComponentCount);
		}

		/// <summary>
		/// Clears all pixels.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Clear() {
			if (Buffer == null) {
				unsafe
				{
					Extensions.Memset(new IntPtr(scan0), 0, pixelComponentCount);
				}
			} else
				Array.Clear(Buffer, 0, pixelComponentCount);
		}

		/// <summary>
		/// Clears all pixels with the specified color.
		/// </summary>
		/// <param name="color">The color to set all pixels to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Clear(Color color) {
			Clear((BgraColor) color);
		}

		/// <summary>
		/// Clears all pixels with the specified color.
		/// </summary>
		/// <param name="color">The color to set all pixels to.</param>
		public void Clear(BgraColor color) {
			if (Buffer == null) {
				unsafe
				{
					byte* last = scan0 + pixelComponentCount;
					switch (componentCount) {
						case 4:
							for (byte* pointer = scan0; pointer < last; pointer += 4)
								*((BgraColor*) pointer) = color;
							break;
						case 3:
							for (byte* pointer = scan0; pointer < last;) {
								*pointer = color.B;
								pointer++;
								*pointer = color.G;
								pointer++;
								*pointer = color.R;
								pointer++;
							}
							break;
						case 1:
							for (byte* pointer = scan0; pointer < last; pointer++)
								*pointer = color.B;
							break;
						default:
							for (byte* pointer = scan0; pointer < last;) {
								*pointer = color.B;
								pointer++;
								*pointer = color.G;
								pointer++;
							}
							break;
					}
				}
			} else {
				switch (componentCount) {
					case 4:
						for (int i = 0; i < pixelComponentCount; i += 4) {
							Buffer[i] = color.B;
							Buffer[i + 1] = color.G;
							Buffer[i + 2] = color.R;
							Buffer[i + 3] = color.A;
						}
						break;
					case 3:
						for (int i = 0; i < pixelComponentCount; i += 3) {
							Buffer[i] = color.B;
							Buffer[i + 1] = color.G;
							Buffer[i + 2] = color.R;
						}
						break;
					case 1:
						for (int i = 0; i < pixelCount; i++)
							Buffer[i] = color.B;
						break;
					case 2:
						for (int i = 0; i < pixelComponentCount; i += 2) {
							Buffer[i] = color.B;
							Buffer[i + 1] = color.G;
						}
						break;
				}
			}
		}

		/// <summary>
		/// Gets the color of specified pixel.
		/// </summary>
		/// <param name="x">The X-coordinate of the pixel in the image.</param>
		/// <param name="y">The Y-coordinate of the pixel in the image.</param>
		/// <returns>The Color of the specified pixel in the image.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public byte Get8BitPixel(int x, int y) {
			return this[y * width + x];
		}

		/// <summary>
		/// Sets the color of the specified pixel.
		/// </summary>
		/// <param name="pixel">The location of the pixel in the image.</param>
		/// <param name="color">The color to set the specified pixel's color to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void SetPixel(Point pixel, Color color) {
			SetPixel(pixel.X, pixel.Y, ref color);
		}

		/// <summary>
		/// Sets the color of the specified pixel.
		/// </summary>
		/// <param name="pixel">The location of the pixel in the image.</param>
		/// <param name="color">The color to set the specified pixel's color to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public void SetPixel(Point pixel, ref Color color) {
			SetPixel(pixel.X, pixel.Y, ref color);
		}

		/// <summary>
		/// Sets the color of the specified pixel.
		/// </summary>
		/// <param name="pixel">The location of the pixel in the image.</param>
		/// <param name="color">The color to set the specified pixel's color to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void SetPixel(Point pixel, BgraColor color) {
			SetPixel(pixel.X, pixel.Y, ref color);
		}

		/// <summary>
		/// Sets the color of the specified pixel.
		/// </summary>
		/// <param name="pixel">The location of the pixel in the image.</param>
		/// <param name="color">The color to set the specified pixel's color to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public void SetPixel(Point pixel, ref BgraColor color) {
			SetPixel(pixel.X, pixel.Y, ref color);
		}

		/// <summary>
		/// Sets the color of the specified pixel.
		/// </summary>
		/// <param name="x">The X-coordinate of the pixel in the image.</param>
		/// <param name="y">The Y-coordinate of the pixel in the image.</param>
		/// <param name="color">The color to set the specified pixel's color to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void SetPixel(int x, int y, Color color) {
			SetPixel(((y * width) + x) * componentCount, ref color);
		}

		/// <summary>
		/// Sets the color of the specified pixel.
		/// </summary>
		/// <param name="x">The X-coordinate of the pixel in the image.</param>
		/// <param name="y">The Y-coordinate of the pixel in the image.</param>
		/// <param name="color">The color to set the specified pixel's color to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public void SetPixel(int x, int y, ref Color color) {
			SetPixel(((y * width) + x) * componentCount, ref color);
		}

		/// <summary>
		/// Sets the color of the specified pixel.
		/// </summary>
		/// <param name="x">The X-coordinate of the pixel in the image.</param>
		/// <param name="y">The Y-coordinate of the pixel in the image.</param>
		/// <param name="color">The color to set the specified pixel's color to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void SetPixel(int x, int y, BgraColor color) {
			SetPixel(((y * width) + x) * componentCount, ref color);
		}

		/// <summary>
		/// Sets the color of the specified pixel.
		/// </summary>
		/// <param name="x">The X-coordinate of the pixel in the image.</param>
		/// <param name="y">The Y-coordinate of the pixel in the image.</param>
		/// <param name="color">The color to set the specified pixel's color to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public void SetPixel(int x, int y, ref BgraColor color) {
			SetPixel(((y * width) + x) * componentCount, ref color);
		}

		/// <summary>
		/// Sets the color of the specified pixel.
		/// </summary>
		/// <param name="index">The index of the pixel to address.</param>
		/// <param name="color">The color to set the pixel to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void SetPixel(int index, Color color) {
			switch (componentCount) {
				case 4:
					this[index] = color.B;
					this[index + 1] = color.G;
					this[index + 2] = color.R;
					this[index + 3] = color.A;
					break;
				case 3:
					this[index] = color.B;
					this[index + 1] = color.G;
					this[index + 2] = color.R;
					break;
				default:
					this[index] = color.B;
					break;
			}
		}

		/// <summary>
		/// Sets the color of the specified pixel.
		/// </summary>
		/// <param name="index">The index of the pixel to address.</param>
		/// <param name="color">The color to set the pixel to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public void SetPixel(int index, ref Color color) {
			switch (componentCount) {
				case 4:
					this[index] = color.B;
					this[index + 1] = color.G;
					this[index + 2] = color.R;
					this[index + 3] = color.A;
					break;
				case 3:
					this[index] = color.B;
					this[index + 1] = color.G;
					this[index + 2] = color.R;
					break;
				default:
					this[index] = color.B;
					break;
			}
		}

		/// <summary>
		/// Sets the color of the specified pixel.
		/// </summary>
		/// <param name="index">The index of the pixel to address.</param>
		/// <param name="color">The color to set the pixel to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void SetPixel(int index, BgraColor color) {
			if (Buffer == null) {
				unsafe
				{
					BgraColor.SetColor(new IntPtr(scan0 + index), componentCount, ref color);
				}
			} else {
				switch (componentCount) {
					case 4:
						Buffer[index] = color.B;
						Buffer[index + 1] = color.G;
						Buffer[index + 2] = color.R;
						Buffer[index + 3] = color.A;
						break;
					case 3:
						Buffer[index] = color.B;
						Buffer[index + 1] = color.G;
						Buffer[index + 2] = color.R;
						break;
					default:
						Buffer[index] = color.B;
						break;
				}
			}
		}

		/// <summary>
		/// Sets the color of the specified pixel.
		/// </summary>
		/// <param name="index">The index of the pixel to address.</param>
		/// <param name="color">The color to set the pixel to.</param>
		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void SetPixel(int index, ref BgraColor color) {
			if (Buffer == null) {
				unsafe
				{
					BgraColor.SetColor(new IntPtr(scan0 + index), componentCount, ref color);
				}
			} else {
				switch (componentCount) {
					case 4:
						Buffer[index] = color.B;
						Buffer[index + 1] = color.G;
						Buffer[index + 2] = color.R;
						Buffer[index + 3] = color.A;
						break;
					case 3:
						Buffer[index] = color.B;
						Buffer[index + 1] = color.G;
						Buffer[index + 2] = color.R;
						break;
					default:
						Buffer[index] = color.B;
						break;
				}
			}
		}

		/// <summary>
		/// Sets the color of the specified pixel.
		/// </summary>
		/// <param name="index">The index from 0 to (pixelCount-1) of the pixel to address.</param>
		/// <param name="color">The color to set the pixel to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void SetPixelUsingPixelCount(int index, Color color) {
			SetPixel(index * componentCount, ref color);
		}

		/// <summary>
		/// Sets the color of the specified pixel.
		/// </summary>
		/// <param name="index">The index from 0 to (pixelCount-1) of the pixel to address.</param>
		/// <param name="color">The color to set the pixel to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public void SetPixelUsingPixelCount(int index, ref Color color) {
			SetPixel(index * componentCount, ref color);
		}

		/// <summary>
		/// Sets the color of the specified pixel.
		/// </summary>
		/// <param name="index">The index from 0 to (pixelCount-1) of the pixel to address.</param>
		/// <param name="color">The color to set the pixel to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void SetPixelUsingPixelCount(int index, BgraColor color) {
			SetPixel(index * componentCount, ref color);
		}

		/// <summary>
		/// Sets the color of the specified pixel.
		/// </summary>
		/// <param name="index">The index from 0 to (pixelCount-1) of the pixel to address.</param>
		/// <param name="color">The color to set the pixel to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public void SetPixelUsingPixelCount(int index, ref BgraColor color) {
			SetPixel(index * componentCount, ref color);
		}

		/// <summary>
		/// Sets the color of the specified pixel.
		/// </summary>
		/// <param name="index">The index from 0 to (pixelCount-1) of the pixel to address.</param>
		/// <param name="color">The color to set the pixel to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Set32BitPixelUsingPixelCount(int index, Color color) {
			Set32BitPixel(index * 4, ref color);
		}

		/// <summary>
		/// Sets the color of the specified pixel.
		/// </summary>
		/// <param name="index">The index from 0 to (pixelCount-1) of the pixel to address.</param>
		/// <param name="color">The color to set the pixel to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public void Set32BitPixelUsingPixelCount(int index, ref Color color) {
			Set32BitPixel(index * 4, ref color);
		}

		/// <summary>
		/// Sets the color of the specified pixel.
		/// </summary>
		/// <param name="index">The index from 0 to (pixelCount-1) of the pixel to address.</param>
		/// <param name="color">The color to set the pixel to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Set32BitPixelUsingPixelCount(int index, BgraColor color) {
			Set32BitPixel(index * 4, ref color);
		}

		/// <summary>
		/// Sets the color of the specified pixel.
		/// </summary>
		/// <param name="index">The index from 0 to (pixelCount-1) of the pixel to address.</param>
		/// <param name="color">The color to set the pixel to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public void Set32BitPixelUsingPixelCount(int index, ref BgraColor color) {
			Set32BitPixel(index * 4, ref color);
		}

		/// <summary>
		/// Sets the color of the specified pixel.
		/// </summary>
		/// <param name="index">The index from 0 to (pixelCount-1) of the pixel to address.</param>
		/// <param name="color">The color to set the pixel to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Set24BitPixelUsingPixelCount(int index, Color color) {
			Set24BitPixel(index * 3, ref color);
		}

		/// <summary>
		/// Sets the color of the specified pixel.
		/// </summary>
		/// <param name="index">The index from 0 to (pixelCount-1) of the pixel to address.</param>
		/// <param name="color">The color to set the pixel to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public void Set24BitPixelUsingPixelCount(int index, ref Color color) {
			Set24BitPixel(index * 3, ref color);
		}

		/// <summary>
		/// Sets the color of the specified pixel.
		/// </summary>
		/// <param name="index">The index from 0 to (pixelCount-1) of the pixel to address.</param>
		/// <param name="color">The color to set the pixel to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Set8BitPixelUsingPixelCount(int index, byte color) {
			this[index] = color;
		}

		/// <summary>
		/// Sets the color of the specified pixel.
		/// </summary>
		/// <param name="index">The index of the pixel to address.</param>
		/// <param name="color">The color to set the pixel to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Set32BitPixel(int index, Color color) {
			this[index] = color.B;
			this[index + 1] = color.G;
			this[index + 2] = color.R;
			this[index + 3] = color.A;
		}

		/// <summary>
		/// Sets the color of the specified pixel.
		/// </summary>
		/// <param name="index">The index of the pixel to address.</param>
		/// <param name="color">The color to set the pixel to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public void Set32BitPixel(int index, ref Color color) {
			this[index] = color.B;
			this[index + 1] = color.G;
			this[index + 2] = color.R;
			this[index + 3] = color.A;
		}

		/// <summary>
		/// Sets the color of the specified pixel.
		/// </summary>
		/// <param name="index">The index of the pixel to address.</param>
		/// <param name="color">The color to set the pixel to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Set32BitPixel(int index, BgraColor color) {
			if (Buffer == null) {
				unsafe
				{
					*((BgraColor*) (scan0 + index)) = color;
				}
			} else {
				Buffer[index] = color.B;
				Buffer[index + 1] = color.G;
				Buffer[index + 2] = color.R;
			}
		}

		/// <summary>
		/// Sets the color of the specified pixel.
		/// </summary>
		/// <param name="index">The index of the pixel to address.</param>
		/// <param name="color">The color to set the pixel to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public void Set32BitPixel(int index, ref BgraColor color) {
			if (Buffer == null) {
				unsafe
				{
					*((BgraColor*) (scan0 + index)) = color;
				}
			} else {
				Buffer[index] = color.B;
				Buffer[index + 1] = color.G;
				Buffer[index + 2] = color.R;
			}
		}

		/// <summary>
		/// Sets the color of the specified pixel.
		/// </summary>
		/// <param name="index">The index of the pixel to address.</param>
		/// <param name="color">The color to set the pixel to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Set24BitPixel(int index, Color color) {
			this[index] = color.B;
			this[index + 1] = color.G;
			this[index + 2] = color.R;
		}

		/// <summary>
		/// Sets the color of the specified pixel.
		/// </summary>
		/// <param name="index">The index of the pixel to address.</param>
		/// <param name="color">The color to set the pixel to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public void Set24BitPixel(int index, ref Color color) {
			this[index] = color.B;
			this[index + 1] = color.G;
			this[index + 2] = color.R;
		}

		/// <summary>
		/// Sets the color of the specified pixel.
		/// </summary>
		/// <param name="index">The index of the pixel to address.</param>
		/// <param name="color">The color to set the pixel to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Set24BitPixel(int index, BgraColor color) {
			if (Buffer == null) {
				unsafe
				{
					byte* ptr = scan0 + index;
					*ptr = color.B;
					*(ptr + 1) = color.G;
					*(ptr + 2) = color.R;
				}
			} else {
				Buffer[index] = color.B;
				Buffer[index + 1] = color.G;
				Buffer[index + 2] = color.R;
			}
		}

		/// <summary>
		/// Sets the color of the specified pixel.
		/// </summary>
		/// <param name="index">The index of the pixel to address.</param>
		/// <param name="color">The color to set the pixel to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public void Set24BitPixel(int index, ref BgraColor color) {
			if (Buffer == null) {
				unsafe
				{
					byte* ptr = scan0 + index;
					*ptr = color.B;
					*(ptr + 1) = color.G;
					*(ptr + 2) = color.R;
				}
			} else {
				Buffer[index] = color.B;
				Buffer[index + 1] = color.G;
				Buffer[index + 2] = color.R;
			}
		}

		/// <summary>
		/// Sets the color of the specified pixel.
		/// </summary>
		/// <param name="index">The index of the pixel to address.</param>
		/// <param name="color">The color to set the pixel to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Set8BitPixel(int index, byte color) {
			this[index] = color;
		}

		/// <summary>
		/// Sets the color of the specified pixel.
		/// </summary>
		/// <param name="x">The X-coordinate of the pixel in the image.</param>
		/// <param name="y">The Y-coordinate of the pixel in the image.</param>
		/// <param name="color">The color to set the specified pixel's color to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Set32BitPixel(int x, int y, Color color) {
			Set32BitPixel((y * width) + x * 4, ref color);
		}

		/// <summary>
		/// Sets the color of the specified pixel.
		/// </summary>
		/// <param name="x">The X-coordinate of the pixel in the image.</param>
		/// <param name="y">The Y-coordinate of the pixel in the image.</param>
		/// <param name="color">The color to set the specified pixel's color to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public void Set32BitPixel(int x, int y, ref Color color) {
			Set32BitPixel((y * width) + x * 4, ref color);
		}

		/// <summary>
		/// Sets the color of the specified pixel.
		/// </summary>
		/// <param name="x">The X-coordinate of the pixel in the image.</param>
		/// <param name="y">The Y-coordinate of the pixel in the image.</param>
		/// <param name="color">The color to set the specified pixel's color to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Set32BitPixel(int x, int y, BgraColor color) {
			Set32BitPixel((y * width) + x * 4, color);
		}

		/// <summary>
		/// Sets the color of the specified pixel.
		/// </summary>
		/// <param name="x">The X-coordinate of the pixel in the image.</param>
		/// <param name="y">The Y-coordinate of the pixel in the image.</param>
		/// <param name="color">The color to set the specified pixel's color to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public void Set32BitPixel(int x, int y, ref BgraColor color) {
			Set32BitPixel((y * width) + x * 4, ref color);
		}

		/// <summary>
		/// Sets the color of the specified pixel.
		/// </summary>
		/// <param name="x">The X-coordinate of the pixel in the image.</param>
		/// <param name="y">The Y-coordinate of the pixel in the image.</param>
		/// <param name="color">The color to set the specified pixel's color to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Set24BitPixel(int x, int y, Color color) {
			Set24BitPixel((y * width) + x * 3, ref color);
		}

		/// <summary>
		/// Sets the color of the specified pixel.
		/// </summary>
		/// <param name="x">The X-coordinate of the pixel in the image.</param>
		/// <param name="y">The Y-coordinate of the pixel in the image.</param>
		/// <param name="color">The color to set the specified pixel's color to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public void Set24BitPixel(int x, int y, ref Color color) {
			Set24BitPixel((y * width) + x * 3, ref color);
		}

		/// <summary>
		/// Sets the color of the specified pixel.
		/// </summary>
		/// <param name="x">The X-coordinate of the pixel in the image.</param>
		/// <param name="y">The Y-coordinate of the pixel in the image.</param>
		/// <param name="color">The color to set the specified pixel's color to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Set24BitPixel(int x, int y, BgraColor color) {
			Set24BitPixel((y * width) + x * 3, ref color);
		}

		/// <summary>
		/// Sets the color of the specified pixel.
		/// </summary>
		/// <param name="x">The X-coordinate of the pixel in the image.</param>
		/// <param name="y">The Y-coordinate of the pixel in the image.</param>
		/// <param name="color">The color to set the specified pixel's color to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public void Set24BitPixel(int x, int y, ref BgraColor color) {
			Set24BitPixel((y * width) + x * 3, ref color);
		}

		/// <summary>
		/// Sets the color of the specified pixel.
		/// </summary>
		/// <param name="x">The X-coordinate of the pixel in the image.</param>
		/// <param name="y">The Y-coordinate of the pixel in the image.</param>
		/// <param name="color">The color to set the specified pixel's color to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Set8BitPixel(int x, int y, byte color) {
			int i = (y * width) + x;
			this[i] = color;
		}

		/// <summary>
		/// Sets all the pixels to the specified colors (slow).
		/// </summary>
		/// <param name="colors">The array of colors to set the pixels to.</param>
		public void SetAllPixels(Color[] colors) {
			Color current;
			int index = 0;
			if (Buffer == null) {
				unsafe
				{
					byte* last = scan0 + pixelComponentCount;
					switch (componentCount) {
						case 4:
							for (byte* pointer = scan0; pointer < last; index++) {
								current = colors[index];
								*pointer = current.B;
								pointer++;
								*pointer = current.G;
								pointer++;
								*pointer = current.R;
								pointer++;
								*pointer = current.A;
								pointer++;
							}
							break;
						case 3:
							for (byte* pointer = scan0; pointer < last; index++) {
								current = colors[index];
								*pointer = current.B;
								pointer++;
								*pointer = current.G;
								pointer++;
								*pointer = current.R;
								pointer++;
							}
							break;
						case 1:
							for (byte* pointer = scan0; pointer < last; pointer++, index++)
								*pointer = colors[index].B;
							break;
						default:
							for (byte* pointer = scan0; pointer < last; index++) {
								current = colors[index];
								*pointer = current.B;
								pointer++;
								*pointer = current.G;
								pointer++;
							}
							break;
					}
				}
			} else {
				switch (componentCount) {
					case 4:
						for (int i = 0; i < colors.Length; i++) {
							current = colors[i];
							Buffer[index] = current.B;
							index++;
							Buffer[index] = current.G;
							index++;
							Buffer[index] = current.R;
							index++;
							Buffer[index] = current.A;
							index++;
						}
						break;
					case 3:
						for (int i = 0; i < colors.Length; i++) {
							current = colors[i];
							Buffer[index] = current.B;
							index++;
							Buffer[index] = current.G;
							index++;
							Buffer[index] = current.R;
							index++;
						}
						break;
					case 1:
						for (int i = 0; i < colors.Length; i++)
							Buffer[i] = colors[i].B;
						break;
					default:
						for (int i = 0; i < colors.Length; i++) {
							current = colors[i];
							Buffer[index] = current.B;
							index++;
							Buffer[index] = current.G;
							index++;
						}
						break;
				}
			}
		}

		/// <summary>
		/// Sets all the pixels to the specified colors (fast).
		/// </summary>
		/// <param name="colors">The array of colors to set the pixels to. Must be the same number of pixels.</param>
		public unsafe void SetAllPixels(BgraColor[] colors) {
			int index = 0;
			using (PointerWrapper imgPtr = PinPointer()) {
				byte* pointer = (byte*) imgPtr.Pointer;
				if (componentCount == 4) {
					fixed (BgraColor* ptr = colors)
						Extensions.MemoryCopy((byte*) ptr, scan0, (uint) pixelComponentCount);
					return;
				} else {
					byte* last = pointer + pixelComponentCount;
					BgraColor current;
					if (componentCount == 3) {
						for (; pointer < last; index++) {
							current = colors[index];
							*pointer = current.B;
							pointer++;
							*pointer = current.G;
							pointer++;
							*pointer = current.R;
							pointer++;
						}
					} else if (componentCount == 1) {
						for (; pointer < last; pointer++, index++)
							*pointer = colors[index].B;
					} else {
						for (; pointer < last; index++) {
							current = colors[index];
							*pointer = current.B;
							pointer++;
							*pointer = current.G;
							pointer++;
						}
					}
				}
			}
		}

		/// <summary>
		/// Gets a float array that holds a copy of the values contained in this instance.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public float[] ToFloatArray() {
			float[] copy = new float[pixelComponentCount];
			if (Buffer == null) {
				unsafe
				{
					for (int i = 0; i < copy.Length; i++)
						copy[i] = scan0[i];
				}
			} else {
				byte[] buffer = this.Buffer;
				for (int i = 0; i < buffer.Length; i++)
					copy[i] = buffer[i];
			}
			return copy;
		}

		/// <summary>
		/// Gets a float array that holds a copy of the values contained in this instance.
		/// </summary>
		/// <param name="multiplier"></param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public float[] ToFloatArray(float multiplier) {
			float[] copy = new float[pixelComponentCount];
			if (Buffer == null) {
				unsafe
				{
					for (int i = 0; i < copy.Length; i++)
						copy[i] = scan0[i] * multiplier;
				}
			} else {
				byte[] buffer = this.Buffer;
				for (int i = 0; i < buffer.Length; i++)
					copy[i] = buffer[i] * multiplier;
			}
			return copy;
		}

		/// <summary>
		/// Applies a transformation function to every pixel in an image (slow).
		/// </summary>
		/// <param name="transformationFunction">The transformation function that takes a pixel index and color as an input and returns the new Color.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void ApplyFunctionToAllPixels(Func<int, Color, Color> transformationFunction) {
			ParallelLoop.For(0, pixelCount, i => SetPixelUsingPixelCount(i, transformationFunction(i, GetPixelUsingPixelCount(i))));
		}

		/// <summary>
		/// Applies a transformation function to every pixel in an image (slow).
		/// </summary>
		/// <param name="transformationFunction">The transformation function that takes a pixel index and Color and the corresponding pixel from the given image as an input and returns the new Color.</param>
		/// <param name="paramImage">The image whose corresponding pixels to pass as parameters.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void ApplyFunctionToAllPixels(Func<int, Color, Color, Color> transformationFunction, PixelWorker paramImage) {
			ParallelLoop.For(0, pixelCount, i => SetPixelUsingPixelCount(i, transformationFunction(i, GetPixelUsingPixelCount(i), paramImage.GetPixelUsingPixelCount(i))));
		}

		/// <summary>
		/// Applies a transformation function to every pixel in an image (slow).
		/// </summary>
		/// <param name="transformationFunction">The transformation function that takes a pixel index and Color and some parameters as an input and returns the new Color.</param>
		/// <param name="param2">Param2</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void ApplyFunctionToAllPixels<Param2>(Func<int, Color, Param2, Color> transformationFunction, Param2 param2) {
			ParallelLoop.For(0, pixelCount, i => SetPixelUsingPixelCount(i, transformationFunction(i, GetPixelUsingPixelCount(i), param2)));
		}

		/// <summary>
		/// Applies a transformation function to every pixel in an image (slow).
		/// </summary>
		/// <param name="transformationFunction">The transformation function that takes a pixel index and Color and the corresponding pixel from the given image and some parameters as an input and returns the new Color.</param>
		/// <param name="paramImage">The image whose corresponding pixels to pass as parameters.</param>
		/// <param name="param3">Param3</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void ApplyFunctionToAllPixels<Param3>(Func<int, Color, Color, Param3, Color> transformationFunction, PixelWorker paramImage, Param3 param3) {
			ParallelLoop.For(0, pixelCount, i => SetPixelUsingPixelCount(i, transformationFunction(i, GetPixelUsingPixelCount(i), paramImage.GetPixelUsingPixelCount(i), param3)));
		}

		/// <summary>
		/// Applies a transformation function to every pixel in an image (slow).
		/// </summary>
		/// <param name="transformationFunction">The transformation function that takes a pixel index and Color and some parameters as an input and returns the new Color.</param>
		/// <param name="param2">Param2</param>
		/// <param name="param3">Param3</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void ApplyFunctionToAllPixels<Param2, Param3>(Func<int, Color, Param2, Param3, Color> transformationFunction, Param2 param2, Param3 param3) {
			ParallelLoop.For(0, pixelCount, i => SetPixelUsingPixelCount(i, transformationFunction(i, GetPixelUsingPixelCount(i), param2, param3)));
		}

		/// <summary>
		/// Applies a transformation function to every pixel in an image (slow).
		/// </summary>
		/// <param name="transformationFunction">The transformation function that takes a pixel index and Color and the corresponding pixel from the given image and some parameters as an input and returns the new Color.</param>
		/// <param name="paramImage">The image whose corresponding pixels to pass as parameters.</param>
		/// <param name="param3">Param3</param>
		/// <param name="param4">Param4</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void ApplyFunctionToAllPixels<Param3, Param4>(Func<int, Color, Color, Param3, Param4, Color> transformationFunction, PixelWorker paramImage, Param3 param3, Param4 param4) {
			ParallelLoop.For(0, pixelCount, i => SetPixelUsingPixelCount(i, transformationFunction(i, GetPixelUsingPixelCount(i), paramImage.GetPixelUsingPixelCount(i), param3, param4)));
		}

		/// <summary>
		/// Applies a transformation function to every pixel in an image (slow).
		/// </summary>
		/// <param name="transformationFunction">The transformation function that takes a pixel index and Color and some parameters as an input and returns the new Color.</param>
		/// <param name="param2">Param2</param>
		/// <param name="param3">Param3</param>
		/// <param name="param4">Param4</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void ApplyFunctionToAllPixels<Param2, Param3, Param4>(Func<int, Color, Param2, Param3, Param4, Color> transformationFunction, Param2 param2, Param3 param3, Param4 param4) {
			ParallelLoop.For(0, pixelCount, i => SetPixelUsingPixelCount(i, transformationFunction(i, GetPixelUsingPixelCount(i), param2, param3, param4)));
		}

		/// <summary>
		/// Applies a transformation function to every pixel in an image (slow).
		/// </summary>
		/// <param name="transformationFunction">The transformation function that takes a pixel index and Color and the corresponding pixel from the given image and some parameters as an input and returns the new Color.</param>
		/// <param name="paramImage">The image whose corresponding pixels to pass as parameters.</param>
		/// <param name="param3">Param3</param>
		/// <param name="param4">Param4</param>
		/// <param name="param5">Param5</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void ApplyFunctionToAllPixels<Param3, Param4, Param5>(Func<int, Color, Color, Param3, Param4, Param5, Color> transformationFunction, PixelWorker paramImage, Param3 param3, Param4 param4, Param5 param5) {
			ParallelLoop.For(0, pixelCount, i => SetPixelUsingPixelCount(i, transformationFunction(i, GetPixelUsingPixelCount(i), paramImage.GetPixelUsingPixelCount(i), param3, param4, param5)));
		}

		/// <summary>
		/// Applies a transformation function to every pixel in an image (slow).
		/// </summary>
		/// <param name="transformationFunction">The transformation function that takes a pixel index and Color and some parameters as an input and returns the new Color.</param>
		/// <param name="param2">Param2</param>
		/// <param name="param3">Param3</param>
		/// <param name="param4">Param4</param>
		/// <param name="param5">Param5</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void ApplyFunctionToAllPixels<Param2, Param3, Param4, Param5>(Func<int, Color, Param2, Param3, Param4, Param5, Color> transformationFunction, Param2 param2, Param3 param3, Param4 param4, Param5 param5) {
			ParallelLoop.For(0, pixelCount, i => SetPixelUsingPixelCount(i, transformationFunction(i, GetPixelUsingPixelCount(i), param2, param3, param4, param5)));
		}

		/// <summary>
		/// Applies a transformation function to every pixel in an image.
		/// </summary>
		/// <param name="transformationFunction">The transformation function that takes a pixel index and BgraColor as an input and returns the new BgraColor.</param>
		[CLSCompliant(false)]
		public unsafe void ApplyFunctionToAllPixels(Func<int, BgraColor, BgraColor> transformationFunction) {
			using (PointerWrapper imgPtr = PinPointer()) {
				byte* scan0 = (byte*) imgPtr.Pointer;
				switch (componentCount) {
					case 4:
						ParallelLoop.For(0, pixelCount, i => {
							BgraColor* ptr = (BgraColor*) (scan0 + i * 4);
							*ptr = transformationFunction(i, *ptr);
						});
						break;
					case 3:
						ParallelLoop.For(0, pixelCount, i => {
							byte* ptr = scan0 + i * 3;
							BgraColor color = transformationFunction(i, new BgraColor() {
								B = *ptr,
								G = *(ptr + 1),
								R = *(ptr + 2),
								A = 255
							});
							*ptr = color.B;
							*(ptr + 1) = color.G;
							*(ptr + 2) = color.R;
						});
						break;
					case 1:
						ParallelLoop.For(0, pixelCount, i => {
							byte* ptr = scan0 + i;
							byte val = *ptr;
							BgraColor color = transformationFunction(i, new BgraColor() {
								B = val,
								G = val,
								R = val,
								A = 255
							});
							*ptr = color.B;
						});
						break;
					default:
						ParallelLoop.For(0, pixelCount, i => {
							byte* ptr = scan0 + (i << 1);
							BgraColor color = transformationFunction(i, new BgraColor() {
								B = *ptr,
								G = *(ptr + 1),
								R = 0,
								A = 255
							});
							*ptr = color.B;
							*(ptr + 1) = color.G;
						});
						break;
				}
			}
		}

		/// <summary>
		/// Applies a transformation function to every pixel in an image.
		/// </summary>
		/// <param name="transformationFunction">The transformation function that takes a pixel index and BgraColor and some parameters as an input and returns the new BgraColor.</param>
		/// <param name="param2">Param2</param>
		[CLSCompliant(false)]
		public unsafe void ApplyFunctionToAllPixels<Param2>(Func<int, BgraColor, Param2, BgraColor> transformationFunction, Param2 param2) {
			using (PointerWrapper imgPtr = PinPointer()) {
				byte* scan0 = (byte*) imgPtr.Pointer;
				switch (componentCount) {
					case 4:
						ParallelLoop.For(0, pixelCount, i => {
							BgraColor* ptr = (BgraColor*) (scan0 + i * 4);
							*ptr = transformationFunction(i, *ptr, param2);
						});
						break;
					case 3:
						ParallelLoop.For(0, pixelCount, i => {
							byte* ptr = scan0 + i * 3;
							BgraColor color = transformationFunction(i, new BgraColor() {
								B = *ptr,
								G = *(ptr + 1),
								R = *(ptr + 2),
								A = 255
							}, param2);
							*ptr = color.B;
							*(ptr + 1) = color.G;
							*(ptr + 2) = color.R;
						});
						break;
					case 1:
						ParallelLoop.For(0, pixelCount, i => {
							byte* ptr = scan0 + i;
							byte val = *ptr;
							BgraColor color = transformationFunction(i, new BgraColor() {
								B = val,
								G = val,
								R = val,
								A = 255
							}, param2);
							*ptr = color.B;
						});
						break;
					default:
						ParallelLoop.For(0, pixelCount, i => {
							byte* ptr = scan0 + (i << 1);
							BgraColor color = transformationFunction(i, new BgraColor() {
								B = *ptr,
								G = *(ptr + 1),
								R = 0,
								A = 255
							}, param2);
							*ptr = color.B;
							*(ptr + 1) = color.G;
						});
						break;
				}
			}
		}

		/// <summary>
		/// Applies a transformation function to every pixel in an image.
		/// </summary>
		/// <param name="transformationFunction">The transformation function that takes a pixel index and BgraColor and some parameters as an input and returns the new BgraColor.</param>
		/// <param name="param2">Param2</param>
		/// <param name="param3">Param3</param>
		[CLSCompliant(false)]
		public unsafe void ApplyFunctionToAllPixels<Param2, Param3>(Func<int, BgraColor, Param2, Param3, BgraColor> transformationFunction, Param2 param2, Param3 param3) {
			using (PointerWrapper imgPtr = PinPointer()) {
				byte* scan0 = (byte*) imgPtr.Pointer;
				switch (componentCount) {
					case 4:
						ParallelLoop.For(0, pixelCount, i => {
							BgraColor* ptr = (BgraColor*) (scan0 + i * 4);
							*ptr = transformationFunction(i, *ptr, param2, param3);
						});
						break;
					case 3:
						ParallelLoop.For(0, pixelCount, i => {
							byte* ptr = scan0 + i * 3;
							BgraColor color = transformationFunction(i, new BgraColor() {
								B = *ptr,
								G = *(ptr + 1),
								R = *(ptr + 2),
								A = 255
							}, param2, param3);
							*ptr = color.B;
							*(ptr + 1) = color.G;
							*(ptr + 2) = color.R;
						});
						break;
					case 1:
						ParallelLoop.For(0, pixelCount, i => {
							byte* ptr = scan0 + i;
							byte val = *ptr;
							BgraColor color = transformationFunction(i, new BgraColor() {
								B = val,
								G = val,
								R = val,
								A = 255
							}, param2, param3);
							*ptr = color.B;
						});
						break;
					default:
						ParallelLoop.For(0, pixelCount, i => {
							byte* ptr = scan0 + (i << 1);
							BgraColor color = transformationFunction(i, new BgraColor() {
								B = *ptr,
								G = *(ptr + 1),
								R = 0,
								A = 255
							}, param2, param3);
							*ptr = color.B;
							*(ptr + 1) = color.G;
						});
						break;
				}
			}
		}

		/// <summary>
		/// Applies a transformation function to every pixel in an image.
		/// </summary>
		/// <param name="transformationFunction">The transformation function that takes a pixel index and BgraColor and some parameters as an input and returns the new BgraColor.</param>
		/// <param name="param2">Param2</param>
		/// <param name="param3">Param3</param>
		/// <param name="param4">Param4</param>
		[CLSCompliant(false)]
		public unsafe void ApplyFunctionToAllPixels<Param2, Param3, Param4>(Func<int, BgraColor, Param2, Param3, Param4, BgraColor> transformationFunction, Param2 param2, Param3 param3, Param4 param4) {
			using (PointerWrapper imgPtr = PinPointer()) {
				byte* scan0 = (byte*) imgPtr.Pointer;
				switch (componentCount) {
					case 4:
						ParallelLoop.For(0, pixelCount, i => {
							BgraColor* ptr = (BgraColor*) (scan0 + i * 4);
							*ptr = transformationFunction(i, *ptr, param2, param3, param4);
						});
						break;
					case 3:
						ParallelLoop.For(0, pixelCount, i => {
							byte* ptr = scan0 + i * 3;
							BgraColor color = transformationFunction(i, new BgraColor() {
								B = *ptr,
								G = *(ptr + 1),
								R = *(ptr + 2),
								A = 255
							}, param2, param3, param4);
							*ptr = color.B;
							*(ptr + 1) = color.G;
							*(ptr + 2) = color.R;
						});
						break;
					case 1:
						ParallelLoop.For(0, pixelCount, i => {
							byte* ptr = scan0 + i;
							byte val = *ptr;
							BgraColor color = transformationFunction(i, new BgraColor() {
								B = val,
								G = val,
								R = val,
								A = 255
							}, param2, param3, param4);
							*ptr = color.B;
						});
						break;
					default:
						ParallelLoop.For(0, pixelCount, i => {
							byte* ptr = scan0 + (i << 1);
							BgraColor color = transformationFunction(i, new BgraColor() {
								B = *ptr,
								G = *(ptr + 1),
								R = 0,
								A = 255
							}, param2, param3, param4);
							*ptr = color.B;
							*(ptr + 1) = color.G;
						});
						break;
				}
			}
		}

		/// <summary>
		/// Applies a transformation function to every pixel in an image.
		/// </summary>
		/// <param name="transformationFunction">The transformation function that takes a pixel index and BgraColor and some parameters as an input and returns the new BgraColor.</param>
		/// <param name="param2">Param2</param>
		/// <param name="param3">Param3</param>
		/// <param name="param4">Param4</param>
		/// <param name="param5">Param5</param>
		[CLSCompliant(false)]
		public unsafe void ApplyFunctionToAllPixels<Param2, Param3, Param4, Param5>(Func<int, BgraColor, Param2, Param3, Param4, Param5, BgraColor> transformationFunction, Param2 param2, Param3 param3, Param4 param4, Param5 param5) {
			using (PointerWrapper imgPtr = PinPointer()) {
				byte* scan0 = (byte*) imgPtr.Pointer;
				switch (componentCount) {
					case 4:
						ParallelLoop.For(0, pixelCount, i => {
							BgraColor* ptr = (BgraColor*) (scan0 + i * 4);
							*ptr = transformationFunction(i, *ptr, param2, param3, param4, param5);
						});
						break;
					case 3:
						ParallelLoop.For(0, pixelCount, i => {
							byte* ptr = scan0 + i * 3;
							BgraColor color = transformationFunction(i, new BgraColor() {
								B = *ptr,
								G = *(ptr + 1),
								R = *(ptr + 2),
								A = 255
							}, param2, param3, param4, param5);
							*ptr = color.B;
							*(ptr + 1) = color.G;
							*(ptr + 2) = color.R;
						});
						break;
					case 1:
						ParallelLoop.For(0, pixelCount, i => {
							byte* ptr = scan0 + i;
							byte val = *ptr;
							BgraColor color = transformationFunction(i, new BgraColor() {
								B = val,
								G = val,
								R = val,
								A = 255
							}, param2, param3, param4, param5);
							*ptr = color.B;
						});
						break;
					default:
						ParallelLoop.For(0, pixelCount, i => {
							byte* ptr = scan0 + (i << 1);
							BgraColor color = transformationFunction(i, new BgraColor() {
								B = *ptr,
								G = *(ptr + 1),
								R = 0,
								A = 255
							}, param2, param3, param4, param5);
							*ptr = color.B;
							*(ptr + 1) = color.G;
						});
						break;
				}
			}
		}

		/// <summary>
		/// Applies a transformation function to every pixel in an image.
		/// </summary>
		/// <param name="transformationFunction">The transformation function that takes a pixel index and Color and some parameters as an input and returns the new Color.</param>
		/// <param name="paramImage">The image whose corresponding pixels to pass as parameters.</param>
		[CLSCompliant(false)]
		public unsafe void ApplyFunctionToAllPixels(Func<int, BgraColor, BgraColor, BgraColor> transformationFunction, PixelWorker paramImage) {
			using (PointerWrapper imgPtr1 = PinPointer()) {
				byte* scan0 = (byte*) imgPtr1.Pointer;
				using (PointerWrapper imgPtr2 = paramImage.PinPointer()) {
					byte* scan1 = (byte*) imgPtr2.Pointer;
					switch (componentCount) {
						case 4:
							switch (paramImage.componentCount) {
								case 4:
									ParallelLoop.For(0, pixelCount, i => {
										int component = i * 4;
										BgraColor* ptr = (BgraColor*) (scan0 + component);
										*ptr = transformationFunction(i, *ptr, *((BgraColor*) (scan1 + component)));
									});
									break;
								case 3:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + i * 4;
										byte* ptr2 = scan1 + i * 3;
										*((BgraColor*) ptr) = transformationFunction(i, *((BgraColor*) ptr), new BgraColor() {
											B = *ptr2,
											G = *(ptr2 + 1),
											R = *(ptr2 + 2),
											A = 255
										});
									});
									break;
								case 1:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + i * 4;
										byte val = *(scan1 + i);
										*((BgraColor*) ptr) = transformationFunction(i, *((BgraColor*) ptr), new BgraColor() {
											B = val,
											G = val,
											R = val,
											A = 255
										});
									});
									break;
								case 2:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + i * 4;
										byte* ptr2 = scan1 + (i << 1);
										*((BgraColor*) ptr) = transformationFunction(i, *((BgraColor*) ptr), new BgraColor() {
											B = *ptr2,
											G = *(ptr2 + 1),
											R = 0,
											A = 255
										});
									});
									break;
							}
							break;
						case 3:
							switch (paramImage.componentCount) {
								case 4:
									ParallelLoop.For(0, pixelCount, i => {
										int component = i * 3;
										byte* ptr = scan0 + component;
										BgraColor color = transformationFunction(i, new BgraColor() {
											B = *ptr,
											G = *(ptr + 1),
											R = *(ptr + 2),
											A = 255
										}, *((BgraColor*) (scan1 + i * 4)));
										*ptr = color.B;
										*(ptr + 1) = color.G;
										*(ptr + 2) = color.R;
									});
									break;
								case 3:
									ParallelLoop.For(0, pixelCount, i => {
										int component = i * 3;
										byte* ptr = scan0 + component;
										byte* ptr2 = scan1 + component;
										BgraColor color = transformationFunction(i, new BgraColor() {
											B = *ptr,
											G = *(ptr + 1),
											R = *(ptr + 2),
											A = 255
										}, new BgraColor() {
											B = *ptr2,
											G = *(ptr2 + 1),
											R = *(ptr2 + 2),
											A = 255
										});
										*ptr = color.B;
										*(ptr + 1) = color.G;
										*(ptr + 2) = color.R;
									});
									break;
								case 1:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + i * 3;
										byte val2 = *(scan1 + i);
										BgraColor color = transformationFunction(i, new BgraColor() {
											B = *ptr,
											G = *(ptr + 1),
											R = *(ptr + 2),
											A = 255
										}, new BgraColor() {
											B = val2,
											G = val2,
											R = val2,
											A = 255
										});
										*ptr = color.B;
										*(ptr + 1) = color.G;
										*(ptr + 2) = color.R;
									});
									break;
								default:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + i * 3;
										byte* ptr2 = scan1 + (i << 1);
										BgraColor color = transformationFunction(i, new BgraColor() {
											B = *ptr,
											G = *(ptr + 1),
											R = *(ptr + 2),
											A = 255
										}, new BgraColor() {
											B = *ptr2,
											G = *(ptr2 + 1),
											R = 0,
											A = 255
										});
										*ptr = color.B;
										*(ptr + 1) = color.G;
										*(ptr + 2) = color.R;
									});
									break;
							}
							break;
						case 1:
							switch (paramImage.componentCount) {
								case 4:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + i;
										byte val = *ptr;
										*ptr = transformationFunction(i, new BgraColor() {
											B = val,
											G = val,
											R = val,
											A = 255
										}, *((BgraColor*) (scan1 + i * 4))).B;
									});
									break;
								case 3:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + i;
										byte val = *ptr;
										byte* ptr2 = scan1 + i * 3;
										*ptr = transformationFunction(i, new BgraColor() {
											B = val,
											G = val,
											R = val,
											A = 255
										}, new BgraColor() {
											B = *ptr2,
											G = *(ptr2 + 1),
											R = *(ptr2 + 2),
											A = 255
										}).B;
									});
									break;
								case 1:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + i;
										byte val = *ptr;
										byte val1 = *(scan1 + i);
										*ptr = transformationFunction(i, new BgraColor() {
											B = val,
											G = val,
											R = val,
											A = 255
										}, new BgraColor() {
											B = val1,
											G = val1,
											R = val1,
											A = 255
										}).B;
									});
									break;
								default:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + i;
										byte val = *ptr;
										byte* ptr2 = scan1 + (i << 1);
										*ptr = transformationFunction(i, new BgraColor() {
											B = val,
											G = val,
											R = val,
											A = 255
										}, new BgraColor() {
											B = *ptr2,
											G = *(ptr2 + 1),
											R = 0,
											A = 255
										}).B;
									});
									break;
							}
							break;
						default:
							switch (paramImage.componentCount) {
								case 4:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + (i << 1);
										BgraColor color = transformationFunction(i, new BgraColor() {
											B = *ptr,
											G = *(ptr + 1),
											R = 0,
											A = 255
										}, *((BgraColor*) (scan1 + i * 4)));
										*ptr = color.B;
										*(ptr + 1) = color.G;
									});
									break;
								case 3:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + (i << 1);
										byte* ptr2 = scan1 + i * 3;
										BgraColor color = transformationFunction(i, new BgraColor() {
											B = *ptr,
											G = *(ptr + 1),
											R = 0,
											A = 255
										}, new BgraColor() {
											B = *ptr2,
											G = *(ptr2 + 1),
											R = *(ptr2 + 2),
											A = 255
										});
										*ptr = color.B;
										*(ptr + 1) = color.G;
									});
									break;
								case 1:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + (i << 1);
										byte val2 = *(scan1 + i);
										BgraColor color = transformationFunction(i, new BgraColor() {
											B = *ptr,
											G = *(ptr + 1),
											R = 0,
											A = 255
										}, new BgraColor() {
											B = val2,
											G = val2,
											R = val2,
											A = 255
										});
										*ptr = color.B;
										*(ptr + 1) = color.G;
									});
									break;
								default:
									ParallelLoop.For(0, pixelCount, i => {
										int component = i << 1;
										byte* ptr = scan0 + component;
										byte* ptr2 = scan1 + component;
										BgraColor color = transformationFunction(i, new BgraColor() {
											B = *ptr,
											G = *(ptr + 1),
											R = 0,
											A = 255
										}, new BgraColor() {
											B = *ptr2,
											G = *(ptr2 + 1),
											R = 0,
											A = 255
										});
										*ptr = color.B;
										*(ptr + 1) = color.G;
									});
									break;
							}
							break;
					}
				}
			}
		}

		/// <summary>
		/// Applies a transformation function to every pixel in an image.
		/// </summary>
		/// <param name="transformationFunction">The transformation function that takes a pixel index and Color and some parameters as an input and returns the new Color.</param>
		/// <param name="paramImage">The image whose corresponding pixels to pass as parameters.</param>
		/// <param name="param3">Param3</param>
		[CLSCompliant(false)]
		public unsafe void ApplyFunctionToAllPixels<Param3>(Func<int, BgraColor, BgraColor, Param3, BgraColor> transformationFunction, PixelWorker paramImage, Param3 param3) {
			using (PointerWrapper imgPtr1 = PinPointer()) {
				byte* scan0 = (byte*) imgPtr1.Pointer;
				using (PointerWrapper imgPtr2 = paramImage.PinPointer()) {
					byte* scan1 = (byte*) imgPtr2.Pointer;
					switch (componentCount) {
						case 4:
							switch (paramImage.componentCount) {
								case 4:
									ParallelLoop.For(0, pixelCount, i => {
										int component = i * 4;
										BgraColor* ptr = (BgraColor*) (scan0 + component);
										*ptr = transformationFunction(i, *ptr, *((BgraColor*) (scan1 + component)), param3);
									});
									break;
								case 3:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + i * 4;
										byte* ptr2 = scan1 + i * 3;
										*((BgraColor*) ptr) = transformationFunction(i, *((BgraColor*) ptr), new BgraColor() {
											B = *ptr2,
											G = *(ptr2 + 1),
											R = *(ptr2 + 2),
											A = 255
										}, param3);
									});
									break;
								case 1:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + i * 4;
										byte val = *(scan1 + i);
										*((BgraColor*) ptr) = transformationFunction(i, *((BgraColor*) ptr), new BgraColor() {
											B = val,
											G = val,
											R = val,
											A = 255
										}, param3);
									});
									break;
								case 2:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + i * 4;
										byte* ptr2 = scan1 + (i << 1);
										*((BgraColor*) ptr) = transformationFunction(i, *((BgraColor*) ptr), new BgraColor() {
											B = *ptr2,
											G = *(ptr2 + 1),
											R = 0,
											A = 255
										}, param3);
									});
									break;
							}
							break;
						case 3:
							switch (paramImage.componentCount) {
								case 4:
									ParallelLoop.For(0, pixelCount, i => {
										int component = i * 3;
										byte* ptr = scan0 + component;
										BgraColor color = transformationFunction(i, new BgraColor() {
											B = *ptr,
											G = *(ptr + 1),
											R = *(ptr + 2),
											A = 255
										}, *((BgraColor*) (scan1 + i * 4)), param3);
										*ptr = color.B;
										*(ptr + 1) = color.G;
										*(ptr + 2) = color.R;
									});
									break;
								case 3:
									ParallelLoop.For(0, pixelCount, i => {
										int component = i * 3;
										byte* ptr = scan0 + component;
										byte* ptr2 = scan1 + component;
										BgraColor color = transformationFunction(i, new BgraColor() {
											B = *ptr,
											G = *(ptr + 1),
											R = *(ptr + 2),
											A = 255
										}, new BgraColor() {
											B = *ptr2,
											G = *(ptr2 + 1),
											R = *(ptr2 + 2),
											A = 255
										}, param3);
										*ptr = color.B;
										*(ptr + 1) = color.G;
										*(ptr + 2) = color.R;
									});
									break;
								case 1:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + i * 3;
										byte val2 = *(scan1 + i);
										BgraColor color = transformationFunction(i, new BgraColor() {
											B = *ptr,
											G = *(ptr + 1),
											R = *(ptr + 2),
											A = 255
										}, new BgraColor() {
											B = val2,
											G = val2,
											R = val2,
											A = 255
										}, param3);
										*ptr = color.B;
										*(ptr + 1) = color.G;
										*(ptr + 2) = color.R;
									});
									break;
								default:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + i * 3;
										byte* ptr2 = scan1 + (i << 1);
										BgraColor color = transformationFunction(i, new BgraColor() {
											B = *ptr,
											G = *(ptr + 1),
											R = *(ptr + 2),
											A = 255
										}, new BgraColor() {
											B = *ptr2,
											G = *(ptr2 + 1),
											R = 0,
											A = 255
										}, param3);
										*ptr = color.B;
										*(ptr + 1) = color.G;
										*(ptr + 2) = color.R;
									});
									break;
							}
							break;
						case 1:
							switch (paramImage.componentCount) {
								case 4:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + i;
										byte val = *ptr;
										*ptr = transformationFunction(i, new BgraColor() {
											B = val,
											G = val,
											R = val,
											A = 255
										}, *((BgraColor*) (scan1 + i * 4)), param3).B;
									});
									break;
								case 3:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + i;
										byte val = *ptr;
										byte* ptr2 = scan1 + i * 3;
										*ptr = transformationFunction(i, new BgraColor() {
											B = val,
											G = val,
											R = val,
											A = 255
										}, new BgraColor() {
											B = *ptr2,
											G = *(ptr2 + 1),
											R = *(ptr2 + 2),
											A = 255
										}, param3).B;
									});
									break;
								case 1:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + i;
										byte val = *ptr;
										byte val1 = *(scan1 + i);
										*ptr = transformationFunction(i, new BgraColor() {
											B = val,
											G = val,
											R = val,
											A = 255
										}, new BgraColor() {
											B = val1,
											G = val1,
											R = val1,
											A = 255
										}, param3).B;
									});
									break;
								default:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + i;
										byte val = *ptr;
										byte* ptr2 = scan1 + (i << 1);
										*ptr = transformationFunction(i, new BgraColor() {
											B = val,
											G = val,
											R = val,
											A = 255
										}, new BgraColor() {
											B = *ptr2,
											G = *(ptr2 + 1),
											R = 0,
											A = 255
										}, param3).B;
									});
									break;
							}
							break;
						default:
							switch (paramImage.componentCount) {
								case 4:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + (i << 1);
										BgraColor color = transformationFunction(i, new BgraColor() {
											B = *ptr,
											G = *(ptr + 1),
											R = 0,
											A = 255
										}, *((BgraColor*) (scan1 + i * 4)), param3);
										*ptr = color.B;
										*(ptr + 1) = color.G;
									});
									break;
								case 3:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + (i << 1);
										byte* ptr2 = scan1 + i * 3;
										BgraColor color = transformationFunction(i, new BgraColor() {
											B = *ptr,
											G = *(ptr + 1),
											R = 0,
											A = 255
										}, new BgraColor() {
											B = *ptr2,
											G = *(ptr2 + 1),
											R = *(ptr2 + 2),
											A = 255
										}, param3);
										*ptr = color.B;
										*(ptr + 1) = color.G;
									});
									break;
								case 1:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + (i << 1);
										byte val2 = *(scan1 + i);
										BgraColor color = transformationFunction(i, new BgraColor() {
											B = *ptr,
											G = *(ptr + 1),
											R = 0,
											A = 255
										}, new BgraColor() {
											B = val2,
											G = val2,
											R = val2,
											A = 255
										}, param3);
										*ptr = color.B;
										*(ptr + 1) = color.G;
									});
									break;
								default:
									ParallelLoop.For(0, pixelCount, i => {
										int component = i << 1;
										byte* ptr = scan0 + component;
										byte* ptr2 = scan1 + component;
										BgraColor color = transformationFunction(i, new BgraColor() {
											B = *ptr,
											G = *(ptr + 1),
											R = 0,
											A = 255
										}, new BgraColor() {
											B = *ptr2,
											G = *(ptr2 + 1),
											R = 0,
											A = 255
										}, param3);
										*ptr = color.B;
										*(ptr + 1) = color.G;
									});
									break;
							}
							break;
					}
				}
			}
		}

		/// <summary>
		/// Applies a transformation function to every pixel in an image.
		/// </summary>
		/// <param name="transformationFunction">The transformation function that takes a pixel index and Color and some parameters as an input and returns the new Color.</param>
		/// <param name="paramImage">The image whose corresponding pixels to pass as parameters.</param>
		/// <param name="param3">Param3</param>
		/// <param name="param4">Param4</param>
		[CLSCompliant(false)]
		public unsafe void ApplyFunctionToAllPixels<Param3, Param4>(Func<int, BgraColor, BgraColor, Param3, Param4, BgraColor> transformationFunction, PixelWorker paramImage, Param3 param3, Param4 param4) {
			using (PointerWrapper imgPtr1 = PinPointer()) {
				byte* scan0 = (byte*) imgPtr1.Pointer;
				using (PointerWrapper imgPtr2 = paramImage.PinPointer()) {
					byte* scan1 = (byte*) imgPtr2.Pointer;
					switch (componentCount) {
						case 4:
							switch (paramImage.componentCount) {
								case 4:
									ParallelLoop.For(0, pixelCount, i => {
										int component = i * 4;
										BgraColor* ptr = (BgraColor*) (scan0 + component);
										*ptr = transformationFunction(i, *ptr, *((BgraColor*) (scan1 + component)), param3, param4);
									});
									break;
								case 3:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + i * 4;
										byte* ptr2 = scan1 + i * 3;
										*((BgraColor*) ptr) = transformationFunction(i, *((BgraColor*) ptr), new BgraColor() {
											B = *ptr2,
											G = *(ptr2 + 1),
											R = *(ptr2 + 2),
											A = 255
										}, param3, param4);
									});
									break;
								case 1:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + i * 4;
										byte val = *(scan1 + i);
										*((BgraColor*) ptr) = transformationFunction(i, *((BgraColor*) ptr), new BgraColor() {
											B = val,
											G = val,
											R = val,
											A = 255
										}, param3, param4);
									});
									break;
								case 2:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + i * 4;
										byte* ptr2 = scan1 + (i << 1);
										*((BgraColor*) ptr) = transformationFunction(i, *((BgraColor*) ptr), new BgraColor() {
											B = *ptr2,
											G = *(ptr2 + 1),
											R = 0,
											A = 255
										}, param3, param4);
									});
									break;
							}
							break;
						case 3:
							switch (paramImage.componentCount) {
								case 4:
									ParallelLoop.For(0, pixelCount, i => {
										int component = i * 3;
										byte* ptr = scan0 + component;
										BgraColor color = transformationFunction(i, new BgraColor() {
											B = *ptr,
											G = *(ptr + 1),
											R = *(ptr + 2),
											A = 255
										}, *((BgraColor*) (scan1 + i * 4)), param3, param4);
										*ptr = color.B;
										*(ptr + 1) = color.G;
										*(ptr + 2) = color.R;
									});
									break;
								case 3:
									ParallelLoop.For(0, pixelCount, i => {
										int component = i * 3;
										byte* ptr = scan0 + component;
										byte* ptr2 = scan1 + component;
										BgraColor color = transformationFunction(i, new BgraColor() {
											B = *ptr,
											G = *(ptr + 1),
											R = *(ptr + 2),
											A = 255
										}, new BgraColor() {
											B = *ptr2,
											G = *(ptr2 + 1),
											R = *(ptr2 + 2),
											A = 255
										}, param3, param4);
										*ptr = color.B;
										*(ptr + 1) = color.G;
										*(ptr + 2) = color.R;
									});
									break;
								case 1:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + i * 3;
										byte val2 = *(scan1 + i);
										BgraColor color = transformationFunction(i, new BgraColor() {
											B = *ptr,
											G = *(ptr + 1),
											R = *(ptr + 2),
											A = 255
										}, new BgraColor() {
											B = val2,
											G = val2,
											R = val2,
											A = 255
										}, param3, param4);
										*ptr = color.B;
										*(ptr + 1) = color.G;
										*(ptr + 2) = color.R;
									});
									break;
								default:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + i * 3;
										byte* ptr2 = scan1 + (i << 1);
										BgraColor color = transformationFunction(i, new BgraColor() {
											B = *ptr,
											G = *(ptr + 1),
											R = *(ptr + 2),
											A = 255
										}, new BgraColor() {
											B = *ptr2,
											G = *(ptr2 + 1),
											R = 0,
											A = 255
										}, param3, param4);
										*ptr = color.B;
										*(ptr + 1) = color.G;
										*(ptr + 2) = color.R;
									});
									break;
							}
							break;
						case 1:
							switch (paramImage.componentCount) {
								case 4:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + i;
										byte val = *ptr;
										*ptr = transformationFunction(i, new BgraColor() {
											B = val,
											G = val,
											R = val,
											A = 255
										}, *((BgraColor*) (scan1 + i * 4)), param3, param4).B;
									});
									break;
								case 3:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + i;
										byte val = *ptr;
										byte* ptr2 = scan1 + i * 3;
										*ptr = transformationFunction(i, new BgraColor() {
											B = val,
											G = val,
											R = val,
											A = 255
										}, new BgraColor() {
											B = *ptr2,
											G = *(ptr2 + 1),
											R = *(ptr2 + 2),
											A = 255
										}, param3, param4).B;
									});
									break;
								case 1:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + i;
										byte val = *ptr;
										byte val1 = *(scan1 + i);
										*ptr = transformationFunction(i, new BgraColor() {
											B = val,
											G = val,
											R = val,
											A = 255
										}, new BgraColor() {
											B = val1,
											G = val1,
											R = val1,
											A = 255
										}, param3, param4).B;
									});
									break;
								default:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + i;
										byte val = *ptr;
										byte* ptr2 = scan1 + (i << 1);
										*ptr = transformationFunction(i, new BgraColor() {
											B = val,
											G = val,
											R = val,
											A = 255
										}, new BgraColor() {
											B = *ptr2,
											G = *(ptr2 + 1),
											R = 0,
											A = 255
										}, param3, param4).B;
									});
									break;
							}
							break;
						default:
							switch (paramImage.componentCount) {
								case 4:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + (i << 1);
										BgraColor color = transformationFunction(i, new BgraColor() {
											B = *ptr,
											G = *(ptr + 1),
											R = 0,
											A = 255
										}, *((BgraColor*) (scan1 + i * 4)), param3, param4);
										*ptr = color.B;
										*(ptr + 1) = color.G;
									});
									break;
								case 3:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + (i << 1);
										byte* ptr2 = scan1 + i * 3;
										BgraColor color = transformationFunction(i, new BgraColor() {
											B = *ptr,
											G = *(ptr + 1),
											R = 0,
											A = 255
										}, new BgraColor() {
											B = *ptr2,
											G = *(ptr2 + 1),
											R = *(ptr2 + 2),
											A = 255
										}, param3, param4);
										*ptr = color.B;
										*(ptr + 1) = color.G;
									});
									break;
								case 1:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + (i << 1);
										byte val2 = *(scan1 + i);
										BgraColor color = transformationFunction(i, new BgraColor() {
											B = *ptr,
											G = *(ptr + 1),
											R = 0,
											A = 255
										}, new BgraColor() {
											B = val2,
											G = val2,
											R = val2,
											A = 255
										}, param3, param4);
										*ptr = color.B;
										*(ptr + 1) = color.G;
									});
									break;
								default:
									ParallelLoop.For(0, pixelCount, i => {
										int component = i << 1;
										byte* ptr = scan0 + component;
										byte* ptr2 = scan1 + component;
										BgraColor color = transformationFunction(i, new BgraColor() {
											B = *ptr,
											G = *(ptr + 1),
											R = 0,
											A = 255
										}, new BgraColor() {
											B = *ptr2,
											G = *(ptr2 + 1),
											R = 0,
											A = 255
										}, param3, param4);
										*ptr = color.B;
										*(ptr + 1) = color.G;
									});
									break;
							}
							break;
					}
				}
			}
		}

		/// <summary>
		/// Applies a transformation function to every pixel in an image.
		/// </summary>
		/// <param name="transformationFunction">The transformation function that takes a pixel index Color and some parameters as an input and returns the new Color.</param>
		/// <param name="paramImage">The image whose corresponding pixels to pass as parameters.</param>
		/// <param name="param3">Param3</param>
		/// <param name="param4">Param4</param>
		/// <param name="param5">Param5</param>
		[CLSCompliant(false)]
		public unsafe void ApplyFunctionToAllPixels<Param3, Param4, Param5>(Func<int, BgraColor, BgraColor, Param3, Param4, Param5, BgraColor> transformationFunction, PixelWorker paramImage, Param3 param3, Param4 param4, Param5 param5) {
			using (PointerWrapper imgPtr1 = PinPointer()) {
				byte* scan0 = (byte*) imgPtr1.Pointer;
				using (PointerWrapper imgPtr2 = paramImage.PinPointer()) {
					byte* scan1 = (byte*) imgPtr2.Pointer;
					switch (componentCount) {
						case 4:
							switch (paramImage.componentCount) {
								case 4:
									ParallelLoop.For(0, pixelCount, i => {
										int component = i * 4;
										BgraColor* ptr = (BgraColor*) (scan0 + component);
										*ptr = transformationFunction(i, *ptr, *((BgraColor*) (scan1 + component)), param3, param4, param5);
									});
									break;
								case 3:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + i * 4;
										byte* ptr2 = scan1 + i * 3;
										*((BgraColor*) ptr) = transformationFunction(i, *((BgraColor*) ptr), new BgraColor() {
											B = *ptr2,
											G = *(ptr2 + 1),
											R = *(ptr2 + 2),
											A = 255
										}, param3, param4, param5);
									});
									break;
								case 1:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + i * 4;
										byte val = *(scan1 + i);
										*((BgraColor*) ptr) = transformationFunction(i, *((BgraColor*) ptr), new BgraColor() {
											B = val,
											G = val,
											R = val,
											A = 255
										}, param3, param4, param5);
									});
									break;
								case 2:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + i * 4;
										byte* ptr2 = scan1 + (i << 1);
										*((BgraColor*) ptr) = transformationFunction(i, *((BgraColor*) ptr), new BgraColor() {
											B = *ptr2,
											G = *(ptr2 + 1),
											R = 0,
											A = 255
										}, param3, param4, param5);
									});
									break;
							}
							break;
						case 3:
							switch (paramImage.componentCount) {
								case 4:
									ParallelLoop.For(0, pixelCount, i => {
										int component = i * 3;
										byte* ptr = scan0 + component;
										BgraColor color = transformationFunction(i, new BgraColor() {
											B = *ptr,
											G = *(ptr + 1),
											R = *(ptr + 2),
											A = 255
										}, *((BgraColor*) (scan1 + i * 4)), param3, param4, param5);
										*ptr = color.B;
										*(ptr + 1) = color.G;
										*(ptr + 2) = color.R;
									});
									break;
								case 3:
									ParallelLoop.For(0, pixelCount, i => {
										int component = i * 3;
										byte* ptr = scan0 + component;
										byte* ptr2 = scan1 + component;
										BgraColor color = transformationFunction(i, new BgraColor() {
											B = *ptr,
											G = *(ptr + 1),
											R = *(ptr + 2),
											A = 255
										}, new BgraColor() {
											B = *ptr2,
											G = *(ptr2 + 1),
											R = *(ptr2 + 2),
											A = 255
										}, param3, param4, param5);
										*ptr = color.B;
										*(ptr + 1) = color.G;
										*(ptr + 2) = color.R;
									});
									break;
								case 1:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + i * 3;
										byte val2 = *(scan1 + i);
										BgraColor color = transformationFunction(i, new BgraColor() {
											B = *ptr,
											G = *(ptr + 1),
											R = *(ptr + 2),
											A = 255
										}, new BgraColor() {
											B = val2,
											G = val2,
											R = val2,
											A = 255
										}, param3, param4, param5);
										*ptr = color.B;
										*(ptr + 1) = color.G;
										*(ptr + 2) = color.R;
									});
									break;
								default:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + i * 3;
										byte* ptr2 = scan1 + (i << 1);
										BgraColor color = transformationFunction(i, new BgraColor() {
											B = *ptr,
											G = *(ptr + 1),
											R = *(ptr + 2),
											A = 255
										}, new BgraColor() {
											B = *ptr2,
											G = *(ptr2 + 1),
											R = 0,
											A = 255
										}, param3, param4, param5);
										*ptr = color.B;
										*(ptr + 1) = color.G;
										*(ptr + 2) = color.R;
									});
									break;
							}
							break;
						case 1:
							switch (paramImage.componentCount) {
								case 4:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + i;
										byte val = *ptr;
										*ptr = transformationFunction(i, new BgraColor() {
											B = val,
											G = val,
											R = val,
											A = 255
										}, *((BgraColor*) (scan1 + i * 4)), param3, param4, param5).B;
									});
									break;
								case 3:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + i;
										byte val = *ptr;
										byte* ptr2 = scan1 + i * 3;
										*ptr = transformationFunction(i, new BgraColor() {
											B = val,
											G = val,
											R = val,
											A = 255
										}, new BgraColor() {
											B = *ptr2,
											G = *(ptr2 + 1),
											R = *(ptr2 + 2),
											A = 255
										}, param3, param4, param5).B;
									});
									break;
								case 1:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + i;
										byte val = *ptr;
										byte val1 = *(scan1 + i);
										*ptr = transformationFunction(i, new BgraColor() {
											B = val,
											G = val,
											R = val,
											A = 255
										}, new BgraColor() {
											B = val1,
											G = val1,
											R = val1,
											A = 255
										}, param3, param4, param5).B;
									});
									break;
								default:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + i;
										byte val = *ptr;
										byte* ptr2 = scan1 + (i << 1);
										*ptr = transformationFunction(i, new BgraColor() {
											B = val,
											G = val,
											R = val,
											A = 255
										}, new BgraColor() {
											B = *ptr2,
											G = *(ptr2 + 1),
											R = 0,
											A = 255
										}, param3, param4, param5).B;
									});
									break;
							}
							break;
						default:
							switch (paramImage.componentCount) {
								case 4:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + (i << 1);
										BgraColor color = transformationFunction(i, new BgraColor() {
											B = *ptr,
											G = *(ptr + 1),
											R = 0,
											A = 255
										}, *((BgraColor*) (scan1 + i * 4)), param3, param4, param5);
										*ptr = color.B;
										*(ptr + 1) = color.G;
									});
									break;
								case 3:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + (i << 1);
										byte* ptr2 = scan1 + i * 3;
										BgraColor color = transformationFunction(i, new BgraColor() {
											B = *ptr,
											G = *(ptr + 1),
											R = 0,
											A = 255
										}, new BgraColor() {
											B = *ptr2,
											G = *(ptr2 + 1),
											R = *(ptr2 + 2),
											A = 255
										}, param3, param4, param5);
										*ptr = color.B;
										*(ptr + 1) = color.G;
									});
									break;
								case 1:
									ParallelLoop.For(0, pixelCount, i => {
										byte* ptr = scan0 + (i << 1);
										byte val2 = *(scan1 + i);
										BgraColor color = transformationFunction(i, new BgraColor() {
											B = *ptr,
											G = *(ptr + 1),
											R = 0,
											A = 255
										}, new BgraColor() {
											B = val2,
											G = val2,
											R = val2,
											A = 255
										}, param3, param4, param5);
										*ptr = color.B;
										*(ptr + 1) = color.G;
									});
									break;
								default:
									ParallelLoop.For(0, pixelCount, i => {
										int component = i << 1;
										byte* ptr = scan0 + component;
										byte* ptr2 = scan1 + component;
										BgraColor color = transformationFunction(i, new BgraColor() {
											B = *ptr,
											G = *(ptr + 1),
											R = 0,
											A = 255
										}, new BgraColor() {
											B = *ptr2,
											G = *(ptr2 + 1),
											R = 0,
											A = 255
										}, param3, param4, param5);
										*ptr = color.B;
										*(ptr + 1) = color.G;
									});
									break;
							}
							break;
					}
				}
			}
		}

		/// <summary>
		/// Swaps the specified components by index (between and including 0 and PixelComponentCount - 1).
		/// </summary>
		/// <param name="c1">The first component index.</param>
		/// <param name="c2">The second component index.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Swap(int c1, int c2) {
			byte temp;
			if (Buffer == null) {
				unsafe
				{
					byte* first = scan0 + c1;
					byte* second = scan0 + c2;
					temp = *first;
					*first = *second;
					*second = temp;
				}
			} else {
				temp = Buffer[c1];
				Buffer[c1] = Buffer[c2];
				Buffer[c2] = temp;
			}
		}

		/// <summary>
		/// Swaps the specified pixels by location.
		/// </summary>
		/// <param name="p1">The first pixel.</param>
		/// <param name="p2">The second pixel.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Swap(Point p1, Point p2) {
			SwapPixel(p1.Y * width + p1.X, p2.Y * width + p2.X);
		}

		/// <summary>
		/// Swaps the specified pixels by location.
		/// </summary>
		/// <param name="x1">The X-coordinate of the first pixel.</param>
		/// <param name="y1">The Y-coordinate of the first pixel.</param>
		/// <param name="x2">The X-coordinate of the second pixel.</param>
		/// <param name="y2">The Y-coordinate of the second pixel.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Swap(int x1, int y1, int x2, int y2) {
			SwapPixel(y1 * width + x1, y2 * width + x2);
		}

		/// <summary>
		/// Swaps the specified pixels by index  (between and including 0 and pixelCount - 1).
		/// </summary>
		/// <param name="p1">The index of the first pixel.</param>
		/// <param name="p2">The index of the second pixel.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void SwapPixel(int p1, int p2) {
			int p1Loc = p1 * componentCount;
			int p2Loc = p2 * componentCount;
			using (PointerWrapper imgPtr = PinPointer()) {
				unsafe
				{
					byte* ptr = (byte*) imgPtr.Pointer;
					byte* first = ptr + p1Loc;
					byte* second = ptr + p2Loc;
					byte temp;
					int cc = componentCount;
					for (int i = 0; i < cc; i++) {
						temp = *first;
						*first = *second;
						*second = temp;
						first++;
						second++;
					}
				}
			}
		}

		/// <summary>
		/// Isolates and extracts the specified color channel from the image.
		/// </summary>
		/// <param name="channel">0 for Blue, 1 for Green, 2 for Red, 3 for Alpha.</param>
		public PixelWorker ExtractChannel(int channel) {
			if (componentCount == 1)
				return this;
			PixelWorker resultant = new PixelWorker(width, height, 1);
			if (channel >= 0) {
				if (channel < componentCount) {
					if (Buffer == null) {
						unsafe
						{
							ParallelLoop.For(0, pixelCount, i => resultant.Buffer[i] = scan0[i * componentCount + channel], ImageLib.ParallelCutoff);
						}
					} else
						ParallelLoop.For(0, pixelCount, i => resultant.Buffer[i] = Buffer[i * componentCount + channel], ImageLib.ParallelCutoff);
				} else
					Extensions.Initialize<byte>(resultant.Buffer, 255);
			}
			return resultant;
		}

		/// <summary>
		/// Premultiplies the alpha components of the image.
		/// </summary>
		public unsafe void PremultiplyAlpha() {
			if (componentCount == 4) {
				using (PointerWrapper imgPtr = PinPointer()) {
					ParallelLoop.For((byte*) imgPtr.Pointer, pixelCount, 4L, delegate (IntPtr i) {
						BgraColor* ptr = (BgraColor*) i;
						*ptr = ptr->PremultiplyAlpha();
					}, ImageLib.ParallelCutoff / 3);
				}
			}
		}

		/// <summary>
		/// Swaps the specified channels (0 => B, 1 => G, 2 => R, 3 => A).
		/// </summary>
		/// <param name="channel1">The channel to swap.</param>
		/// <param name="channel2">The channel to swap with.</param>
		public unsafe void SwapChannels(int channel1, int channel2) {
			if (componentCount == 1 || channel1 == channel2 || channel1 < 0 || channel2 < 0 || channel2 > 3)
				return;
			else if (channel1 > channel2) {
				int temp = channel1;
				channel1 = channel2;
				channel2 = temp;
			}
			if (channel1 >= componentCount)
				return;
			int diff = channel2 - channel1;
			using (PointerWrapper imgPtr = PinPointer()) {
				byte* ptr = (byte*) imgPtr.Pointer;
				switch (componentCount) {
					case 4:
						ParallelLoop.For(channel1, PixelComponentCount, 4, i => {
							byte* chan1 = ptr + i;
							byte* chan2 = chan1 + diff;
							byte val = *chan1;
							*chan1 = *chan2;
							*chan2 = val;
						}, ImageLib.ParallelCutoff);
						break;
					case 3:
						if (channel2 == 3)
							ParallelLoop.For(channel1, PixelComponentCount, 3, i => ptr[i] = 255, ImageLib.ParallelCutoff);
						else {
							ParallelLoop.For(channel1, PixelComponentCount, 3, i => {
								byte* chan1 = ptr + i;
								byte* chan2 = chan1 + diff;
								byte val = *chan1;
								*chan1 = *chan2;
								*chan2 = val;
							}, ImageLib.ParallelCutoff);
						}
						break;
					default:
						if (channel2 == 3)
							ParallelLoop.For(channel1, PixelComponentCount, 2, i => ptr[i] = 255, ImageLib.ParallelCutoff);
						else if (channel2 == 2)
							ParallelLoop.For(channel1, PixelComponentCount, 2, i => ptr[i] = 0, ImageLib.ParallelCutoff);
						else {
							ParallelLoop.For(channel1, PixelComponentCount, 2, i => {
								byte* chan1 = ptr + i;
								byte* chan2 = chan1 + 1;
								byte val = *chan1;
								*chan1 = *chan2;
								*chan2 = val;
							}, ImageLib.ParallelCutoff);
						}
						break;
				}
			}
		}

		/*public void CopyRegion(PixelWorker source, Rectangle sourceRect, Point targetLocation) {

		}*/

		/*public void PadOrCrop(Size newSize) {
			if (newSize.Width < 0)
				newSize.Width = 0;
			if (newSize.Height < 0)
				newSize.Height = 0;
			if (size == Size)
				return;
			byte[] newBufferPixels = new byte[pixelCount];
			unsafe
			{
				using (PointerWrapper imgPtr = PinPointer()) {
					byte* ptr = (byte*) imgPtr.Pointer;
					if (width == newSize.Width) {
						if (newSize.Height > height) {
						} else {
						}
					} else {
					}
				}
			}
			RemoveBitmap();
			bufferPixels = newBufferPixels;
			width = newSize.Width;
			height = newSize.Height;
			size = newSize;
			bounds = new Rectangle(Point.Empty, newSize);
			pixelCount = width * height;
			stride = ((width + 3) / 4) * 4;
			widthComponentCount = width * componentCount;
			heightComponentCount = height * componentCount;
			pixelComponentCount = pixelCount * componentCount;
		}*/

		/// <summary>
		/// Rotates, flips, or rotates and flips the image.
		/// </summary>
		/// <param name="mode">The transformation to apply.</param>
		public void RotateFlip(RotateFlipType mode) {
			switch ((int) mode) {
				case 1: //Rotate90FlipNone, Rotate270FlipXY
					Rotate90(true);
					break;
				case 2: //RotateNoneFlipXY, Rotate180FlipNone
					Flip(true, true);
					break;
				case 3: //Rotate90FlipXY, Rotate270FlipNone
					Rotate90(false);
					break;
				case 4: //RotateNoneFlipX, Rotate180FlipY
					Flip(true, false);
					break;
				case 5: //Rotate90FlipX, Rotate270FlipY
					if (width == height) {
						if (Buffer == null) {
							ParallelLoop.For(0, height - 1, y => {
								int c;
								int rowPos = y * width;
								int cc = componentCount;
								byte current;
								unsafe
								{
									byte* ptr1, ptr2, loc1, loc2;
									for (int x = y + 1; x < width; x++) {
										loc1 = scan0 + (x * width + y) * cc;
										loc2 = scan0 + (rowPos + x) * cc;
										for (c = 0; c < cc; c++) {
											ptr1 = loc1 + c;
											current = *ptr1;
											ptr2 = loc2 + c;
											*ptr1 = *ptr2;
											*ptr2 = current;
										}
									}
								}
							}, ImageLib.ParallelCutoff);
						} else {
							ParallelLoop.For(0, height - 1, y => {
								int c, loc1, loc2, index1, index2;
								int rowPos = y * width;
								int cc = componentCount;
								byte current;
								for (int x = y + 1; x < width; x++) {
									loc1 = (x * width + y) * cc;
									loc2 = (rowPos + x) * cc;
									for (c = 0; c < cc; c++) {
										index1 = loc1 + c;
										current = Buffer[index1];
										index2 = loc2 + c;
										Buffer[index1] = Buffer[index2];
										Buffer[index2] = current;
									}
								}
							}, ImageLib.ParallelCutoff);
						}
					} else {
						byte[] newBuffer = new byte[pixelComponentCount];
						ParallelLoop.For(0, pixelComponentCount, i => {
							int component = i % componentCount;
							int pixelIndex = i / componentCount;
							int x = pixelIndex % width, y = pixelIndex / width;
							newBuffer[(y + x * height) * componentCount + component] = this[i];
						}, ImageLib.ParallelCutoff);
						MarkBufferResize(height, width);
						Buffer = newBuffer;
					}
					break;
				case 6: //RotateNoneFlipY, Rotate180FlipX
					Flip(false, true);
					break;
				case 7: //Rotate90FlipY, Rotate270FlipX
					if (width == height) {
						if (Buffer == null) {
							ParallelLoop.For(0, height - 1, y => {
								int c;
								int rowPos = (height - y - 1) * width;
								int cc = componentCount;
								byte current;
								unsafe
								{
									byte* ptr1, ptr2, loc1, loc2;
									for (int x = y + 1; x < width; x++) {
										loc1 = scan0 + ((width - x - 1) * width + y) * cc;
										loc2 = scan0 + (rowPos + x) * cc;
										for (c = 0; c < cc; c++) {
											ptr1 = loc1 + c;
											current = *ptr1;
											ptr2 = loc2 + c;
											*ptr1 = *ptr2;
											*ptr2 = current;
										}
									}
								}
							}, ImageLib.ParallelCutoff);
						} else {
							ParallelLoop.For(0, height - 1, y => {
								int c, loc1, loc2, index1, index2;
								int rowPos = (height - y - 1) * width;
								int cc = componentCount;
								byte current;
								for (int x = y + 1; x < width; x++) {
									loc1 = ((width - x - 1) * width + y) * cc;
									loc2 = (rowPos + x) * cc;
									for (c = 0; c < cc; c++) {
										index1 = loc1 + c;
										current = Buffer[index1];
										index2 = loc2 + c;
										Buffer[index1] = Buffer[index2];
										Buffer[index2] = current;
									}
								}
							}, ImageLib.ParallelCutoff);
						}
					} else {
						byte[] newBuffer = new byte[pixelComponentCount];
						ParallelLoop.For(0, pixelComponentCount, i => {
							int component = i % componentCount;
							int pixelIndex = i / componentCount;
							int x = pixelIndex % width, y = pixelIndex / width;
							newBuffer[(height - (y + 1) + (width - (x + 1)) * height) * componentCount + component] = this[i];
						}, ImageLib.ParallelCutoff);
						MarkBufferResize(height, width);
						Buffer = newBuffer;
					}
					break;
			}
		}

		/// <summary>
		/// Rotates the image 90 degrees.
		/// </summary>
		/// <param name="clockwise">If true the image will be rotated 90 degrees clockwise, otherwise it will be rotated 90 degrees anticlockwise.</param>
		public void Rotate90(bool clockwise) {
			if (width == height) {
				int halfWidth = (width + 1) / 2;
				if (Buffer == null) {
					ParallelLoop.For(0, height / 2, y => {
						int c;
						int cc = componentCount;
						byte current;
						unsafe
						{
							byte* ptr1, ptr2, ptr3, ptr4, loc1, loc2, loc3, loc4;
							for (int x = 0; x < halfWidth; x++) {
								if (clockwise) {
									loc4 = scan0 + (x + y * width) * cc;
									loc3 = scan0 + (height - y - 1 + x * width) * cc;
									loc2 = scan0 + ((height - y) * width - x - 1) * cc;
									loc1 = scan0 + (y + (height - x - 1) * width) * cc;
								} else {
									loc1 = scan0 + (x + y * width) * cc;
									loc2 = scan0 + (height - y - 1 + x * width) * cc;
									loc3 = scan0 + ((height - y) * width - x - 1) * cc;
									loc4 = scan0 + (y + (height - x - 1) * width) * cc;
								}
								for (c = 0; c < cc; c++) {
									ptr1 = loc1 + c;
									ptr2 = loc2 + c;
									ptr3 = loc3 + c;
									ptr4 = loc4 + c;
									current = *ptr1;
									*ptr1 = *ptr2;
									*ptr2 = *ptr3;
									*ptr3 = *ptr4;
									*ptr4 = current;
								}
							}
						}
					}, ImageLib.ParallelCutoff);
				} else {
					ParallelLoop.For(0, height / 2, y => {
						int c, loc1, loc2, loc3, loc4, index1, index2, index3, index4;
						int rowPos = y * width;
						int cc = componentCount;
						byte current;
						for (int x = 0; x < halfWidth; x++) {
							if (clockwise) {
								loc4 = (x + y * width) * cc;
								loc3 = (height - y - 1 + x * width) * cc;
								loc2 = ((height - y) * width - x - 1) * cc;
								loc1 = (y + (height - x - 1) * width) * cc;
							} else {
								loc1 = (x + y * width) * cc;
								loc2 = (height - y - 1 + x * width) * cc;
								loc3 = ((height - y) * width - x - 1) * cc;
								loc4 = (y + (height - x - 1) * width) * cc;
							}
							for (c = 0; c < cc; c++) {
								index1 = loc1 + c;
								index2 = loc2 + c;
								index3 = loc3 + c;
								index4 = loc4 + c;
								current = Buffer[index1];
								Buffer[index1] = Buffer[index2];
								Buffer[index2] = Buffer[index3];
								Buffer[index3] = Buffer[index4];
								Buffer[index4] = current;
							}
						}
					}, ImageLib.ParallelCutoff);
				}
			} else {
				byte[] newBuffer = new byte[pixelComponentCount];
				if (clockwise) {
					ParallelLoop.For(0, pixelComponentCount, i => {
						int component = i % componentCount;
						int pixelIndex = i / componentCount;
						int x = pixelIndex % width, y = pixelIndex / width;
						newBuffer[(height - (y + 1) + x * height) * componentCount + component] = this[i];
					}, ImageLib.ParallelCutoff);
				} else {
					ParallelLoop.For(0, pixelComponentCount, i => {
						int component = i % componentCount;
						int pixelIndex = i / componentCount;
						int x = pixelIndex % width, y = pixelIndex / width;
						newBuffer[(y + (width - (x + 1)) * height) * componentCount + component] = this[i];
					}, ImageLib.ParallelCutoff);
				}
				MarkBufferResize(height, width);
				Buffer = newBuffer;
			}
		}

		/// <summary>
		/// FOR INTERNAL USE ONLY: Marks that the buffer size has been changed.
		/// </summary>
		/// <param name="newWidth">The new image width.</param>
		/// <param name="newHeight">The new image height.</param>
		public void MarkBufferResize(int newWidth, int newHeight) {
			if (newWidth < 0)
				newWidth = 0;
			if (newHeight < 0)
				newHeight = 0;
			if (width == newWidth && height == newHeight)
				return;
			RemoveBitmap();
			width = newWidth;
			height = newHeight;
			size = new Size(newWidth, newHeight);
			bounds = new Rectangle(Point.Empty, size);
			stride = ((newWidth + 3) / 4) * 4;
			widthComponentCount = newWidth * componentCount;
			heightComponentCount = newHeight * componentCount;
		}

		/// <summary>
		/// Rotates the image 180 degrees.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Rotate180() {
			Flip(true, true);
		}

		/// <summary>
		/// Flips the image in the orientation desired.
		/// </summary>
		/// <param name="flipX">Whether to flip horizontally.</param>
		/// <param name="flipY">Whether to flip vertically.</param>
		public void Flip(bool flipX, bool flipY) {
			int halfWidth = width / 2, halfHeight = height / 2;
			if (flipX && flipY) {
				bool isOdd = (width & 1) == 1;
				if (Buffer == null) {
					ParallelLoop.For(0, height, y => {
						int rowPos = y * width;
						int c, cc = componentCount;
						byte current;
						unsafe
						{
							byte* ptr1, ptr2, loc1, loc2;
							for (int x = 0; x < halfWidth; x++) {
								loc1 = scan0 + ((height - y) * width - (x + 1)) * cc;
								loc2 = scan0 + (rowPos + x) * cc;
								for (c = 0; c < cc; c++) {
									ptr1 = loc1 + c;
									current = *ptr1;
									ptr2 = loc2 + c;
									*ptr1 = *ptr2;
									*ptr2 = current;
								}
							}
						}
					}, ImageLib.ParallelCutoff);
					if (isOdd) {
						ParallelLoop.For(0, halfHeight, y => {
							int c;
							int cc = componentCount;
							byte current;
							unsafe
							{
								byte* loc1, loc2, ptr1, ptr2;
								loc1 = scan0 + ((height - (y + 1)) * width + halfWidth) * cc;
								loc2 = scan0 + (y * width + halfWidth) * cc;
								for (c = 0; c < cc; c++) {
									ptr1 = loc1 + c;
									current = *ptr1;
									ptr2 = loc2 + c;
									*ptr1 = *ptr2;
									*ptr2 = current;
								}
							}
						}, ImageLib.ParallelCutoff);
					}
				} else {
					ParallelLoop.For(0, height, y => {
						int c, loc1, loc2, index1, index2;
						int rowPos = y * width;
						int cc = componentCount;
						byte current;
						for (int x = 0; x < halfWidth; x++) {
							loc1 = ((height - y) * width - (x + 1)) * cc;
							loc2 = (rowPos + x) * cc;
							for (c = 0; c < cc; c++) {
								index1 = loc1 + c;
								current = Buffer[index1];
								index2 = loc2 + c;
								Buffer[index1] = Buffer[index2];
								Buffer[index2] = current;
							}
						}
					}, ImageLib.ParallelCutoff);
					if (isOdd) {
						ParallelLoop.For(0, halfHeight, y => {
							byte current;
							int c, loc1, loc2, index1, index2;
							int cc = componentCount;
							loc1 = ((height - (y + 1)) * width + halfWidth) * cc;
							loc2 = (y * width + halfWidth) * cc;
							for (c = 0; c < cc; c++) {
								index1 = loc1 + c;
								current = Buffer[index1];
								index2 = loc2 + c;
								Buffer[index1] = Buffer[index2];
								Buffer[index2] = current;
							}
						}, ImageLib.ParallelCutoff);
					}
				}
			} else if (flipX) {
				if (Buffer == null) {
					ParallelLoop.For(0, height, y => {
						int c;
						int rowPos = y * width;
						int cc = componentCount;
						byte current;
						unsafe
						{
							byte* ptr1, ptr2, loc1, loc2;
							for (int x = 0; x < halfWidth; x++) {
								loc1 = scan0 + (rowPos + width - (x + 1)) * cc;
								loc2 = scan0 + (rowPos + x) * cc;
								for (c = 0; c < cc; c++) {
									ptr1 = loc1 + c;
									current = *ptr1;
									ptr2 = loc2 + c;
									*ptr1 = *ptr2;
									*ptr2 = current;
								}
							}
						}
					}, ImageLib.ParallelCutoff);
				} else {
					ParallelLoop.For(0, height, y => {
						int c, loc1, loc2, index1, index2;
						int rowPos = y * width;
						int cc = componentCount;
						byte current;
						for (int x = 0; x < halfWidth; x++) {
							loc1 = (rowPos + width - (x + 1)) * cc;
							loc2 = (rowPos + x) * cc;
							for (c = 0; c < cc; c++) {
								index1 = loc1 + c;
								current = Buffer[index1];
								index2 = loc2 + c;
								Buffer[index1] = Buffer[index2];
								Buffer[index2] = current;
							}
						}
					}, ImageLib.ParallelCutoff);
				}
			} else if (flipY) {
				if (Buffer == null) {
					ParallelLoop.For(0, width, x => {
						int c;
						int cc = componentCount;
						byte current;
						unsafe
						{
							byte* loc1, loc2, ptr1, ptr2;
							for (int y = 0; y < halfHeight; y++) {
								loc1 = scan0 + ((height - (y + 1)) * width + x) * cc;
								loc2 = scan0 + (y * width + x) * cc;
								for (c = 0; c < cc; c++) {
									ptr1 = loc1 + c;
									current = *ptr1;
									ptr2 = loc2 + c;
									*ptr1 = *ptr2;
									*ptr2 = current;
								}
							}
						}
					}, ImageLib.ParallelCutoff);
				} else {
					ParallelLoop.For(0, width, x => {
						int c, loc1, loc2, index1, index2;
						int cc = componentCount;
						byte current;
						for (int y = 0; y < halfHeight; y++) {
							loc1 = ((height - (y + 1)) * width + x) * cc;
							loc2 = (y * width + x) * cc;
							for (c = 0; c < cc; c++) {
								index1 = loc1 + c;
								current = Buffer[index1];
								index2 = loc2 + c;
								Buffer[index1] = Buffer[index2];
								Buffer[index2] = current;
							}
						}
					}, ImageLib.ParallelCutoff);
				}
			}
		}

		/// <summary>
		/// Converts this image to 8-bit grayscale image (beware that this starts writing to a new buffer unless it's already grayscale).
		/// </summary>
		/// <param name="premultiplyAlpha">If true, the alpha channel is premultiplied to the color channels.</param>
		public unsafe void ConvertToGrayscale(bool premultiplyAlpha = false) {
			if (componentCount == 1)
				return;
			if (premultiplyAlpha)
				PremultiplyAlpha();
			byte[] newBufferPixels = new byte[pixelCount];
			using (PointerWrapper imgPtr = PinPointer()) {
				byte* ptr = (byte*) imgPtr.Pointer;
				switch (componentCount) {
					case 4:
						ParallelLoop.For(0, pixelCount, i => newBufferPixels[i] = (byte) ImageLib.ToGrayscale(*(BgraColor*) (ptr + i * 4)), ImageLib.ParallelCutoff);
						break;
					case 3:
						ParallelLoop.For(0, pixelCount, i => {
							byte* val = ptr + i * 3;
							newBufferPixels[i] = (byte) ImageLib.ToGrayscale(*(val + 2), *(val + 1), *val);
						}, ImageLib.ParallelCutoff);
						break;
					default:
						ParallelLoop.For(0, pixelCount, i => {
							byte* val = ptr + i * 2;
							newBufferPixels[i] = (byte) ((*(val + 1) + *val) / 2);
						}, ImageLib.ParallelCutoff);
						break;
				}
			}
			RemoveBitmap();
			Buffer = newBufferPixels;
			stride = ((width + 3) / 4) * 4;
			widthComponentCount = width;
			heightComponentCount = height;
			pixelComponentCount = pixelCount;
			componentCount = 1;
			format = PixelFormat.Format8bppIndexed;
		}

		/// <summary>
		/// Converts this image to 24-bit BGR image (beware that this starts writing to a new buffer unless it's already BGR).
		/// </summary>
		/// <param name="premultiplyAlpha">If true, the alpha channel is premultiplied to the color channels.</param>
		public unsafe void ConvertTo24Bit(bool premultiplyAlpha = false) {
			if (componentCount == 3)
				return;
			if (premultiplyAlpha)
				PremultiplyAlpha();
			byte[] newBufferPixels = new byte[pixelCount * 3];
			using (PointerWrapper imgPtr = PinPointer()) {
				byte* ptr = (byte*) imgPtr.Pointer;
				switch (componentCount) {
					case 4:
						ParallelLoop.For(0, pixelCount, i => {
							BgraColor color = ImageLib.Overlay(BgraColor.Black, *(BgraColor*) (ptr + i * 4));
							int offset = i * 3;
							newBufferPixels[offset] = color.B;
							newBufferPixels[offset + 1] = color.G;
							newBufferPixels[offset + 2] = color.R;
						}, ImageLib.ParallelCutoff);
						break;
					case 1:
						ParallelLoop.For(0, pixelCount, i => {
							int offset = i * 3;
							byte val = ptr[i];
							newBufferPixels[offset] = val;
							newBufferPixels[offset + 1] = val;
							newBufferPixels[offset + 2] = val;
						}, ImageLib.ParallelCutoff);
						break;
					default:
						ParallelLoop.For(0, pixelCount, i => {
							int offset = i * 3;
							byte* p = ptr + (i * 2);
							newBufferPixels[offset] = *p;
							newBufferPixels[offset + 1] = *(p + 1);
							newBufferPixels[offset + 2] = 0;
						}, ImageLib.ParallelCutoff);
						break;
				}
			}
			RemoveBitmap();
			Buffer = newBufferPixels;
			stride = ((width + 3) / 4) * 4;
			widthComponentCount = width * 3;
			heightComponentCount = height * 3;
			pixelComponentCount = pixelCount * 3;
			componentCount = 3;
			format = PixelFormat.Format24bppRgb;
		}

		/// <summary>
		/// Converts this image to 32-bit BGRA image (beware that this starts writing to a new buffer unless it's already BGRA).
		/// </summary>
		public unsafe void ConvertTo32Bit() {
			if (componentCount == 4)
				return;
			byte[] newBufferPixels = new byte[pixelCount * 4];
			using (PointerWrapper imgPtr = PinPointer()) {
				byte* ptr = (byte*) imgPtr.Pointer;
				switch (componentCount) {
					case 3:
						ParallelLoop.For(0, pixelCount, i => {
							byte* p = ptr + (i * 3);
							int offset = i * 4;
							newBufferPixels[offset] = *p;
							newBufferPixels[offset + 1] = p[1];
							newBufferPixels[offset + 2] = p[2];
							newBufferPixels[offset + 3] = 255;
						}, ImageLib.ParallelCutoff);
						break;
					case 1:
						ParallelLoop.For(0, pixelCount, i => {
							int offset = i * 4;
							byte val = ptr[i];
							newBufferPixels[offset] = val;
							newBufferPixels[offset + 1] = val;
							newBufferPixels[offset + 2] = val;
							newBufferPixels[offset + 3] = 255;
						}, ImageLib.ParallelCutoff);
						break;
					default:
						ParallelLoop.For(0, pixelCount, i => {
							byte* p = ptr + (i * 2);
							int offset = i * 4;
							newBufferPixels[offset] = *p;
							newBufferPixels[offset + 1] = p[1];
							newBufferPixels[offset + 2] = 0;
							newBufferPixels[offset + 3] = 255;
						}, ImageLib.ParallelCutoff);
						break;
				}
			}
			RemoveBitmap();
			Buffer = newBufferPixels;
			stride = width;
			widthComponentCount = width * 4;
			heightComponentCount = height * 4;
			pixelComponentCount = pixelCount * 4;
			componentCount = 4;
			format = PixelFormat.Format32bppArgb;
		}

		private void RemoveBitmap() {
			if (image == null)
				return;
			lock (LockSync) {
				if (image != null) {
					if (IsLocked) {
						image.UnlockBits(bitmapData);
						IsLocked = false;
					}
					PixelWorker temp;
					WrapperCollection.TryRemove(image, out temp);
					if (ImageAction == ImageParameterAction.Dispose)
						image.DisposeSafe();
					image = null;
				}
			}
		}

		/// <summary>
		/// Gets a pointer to the start of the image in memory. Be sure to dispose the pointer after use.
		/// </summary>
		public PointerWrapper PinPointer() {
			return new PointerWrapper(this);
		}

		/// <summary>
		/// Writes all modifications to the bitmap.
		/// </summary>
		/// <param name="unlockAfter">Whether to unlock bitmap after writing changes.</param>
		public void WriteChanges(bool unlockAfter = true) {
			lock (LockSync) {
				if (image.IsDisposed())
					return;
				else if (Buffer != null) {
					if (!IsLocked) {
						IsLocked = true;
						bitmapData = image.LockBits(bounds, unlockAfter ? ImageLockMode.WriteOnly : ImageLockMode.ReadWrite, format);
					}
					unsafe
					{
						scan0 = (byte*) bitmapData.Scan0;
						stride = bitmapData.Stride;
						byte* i = scan0;
						for (int y = 0; y < pixelComponentCount; y += widthComponentCount) {
							Marshal.Copy(Buffer, y, new IntPtr(i), widthComponentCount);
							i += stride;
						}
					}
				}
				if (unlockAfter && IsLocked) {
					image.UnlockBits(bitmapData);
					IsLocked = false;
				}
			}
		}

		/// <summary>
		/// DO NOT USE UNLESS NECESSARY. Locks the bits of the image.
		/// </summary>
		/// <param name="mode">The mode to lock with.</param>
		public void LockBits(ImageLockMode mode) {
			lock (LockSync) {
				if (IsLocked || image.IsDisposed())
					return;
				IsLocked = true;
				bitmapData = image.LockBits(bounds, mode, format);
				unsafe
				{
					scan0 = (byte*) bitmapData.Scan0;
				}
				stride = bitmapData.Stride;
			}
		}

		/// <summary>
		/// DO NOT USE UNLESS NECESSARY. Unlocks the bits of the image.
		/// </summary>
		public void UnlockBits() {
			lock (LockSync) {
				if (IsLocked && !image.IsDisposed())
					image.UnlockBits(bitmapData);
				IsLocked = false;
			}
		}

		/// <summary>
		/// Returns an enumerator that quickly iterates through the bytes of the PixelWorker.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public IEnumerator<IntPtr> GetEnumerator() {
			int increment = IterationStep.Value;
			if (increment < 1)
				increment = 1;
			IEnumerator<IntPtr> returnValue;
			if (SkipAlphaInIterator.IsValueCreated && SkipAlphaInIterator.Value && componentCount == 4)
				returnValue = new SkipAlphaEnumerator(this, increment);
			else
				returnValue = new DirectComponentEnumerator(this, increment);
			return returnValue;
		}

		/// <summary>
		/// Returns an enumerator that quickly iterates through the bytes of the PixelWorker.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		IEnumerator IEnumerable.GetEnumerator() {
			return this.GetEnumerator();
		}

		/// <summary>
		/// Makes a shallow clone of this instance in place.
		/// Do this when you want to keep a reference from being disposed by simply increasing the reference counter.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void ShallowClone() {
			Interlocked.Increment(ref referenceCounter);
			LoadBuffer();
		}

		/// <summary>
		/// Makes a shallow clone the PixelWorker.
		/// Do this when you want to keep a reference from being disposed by simply increasing the reference counter.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public object Clone() {
			ShallowClone();
			return this;
		}

		/// <summary>
		/// Returns a string that describes this instance.
		/// </summary>
		public override string ToString() {
			return "{ size: " + width + "x" + height + ", " + format + ", Buffered: " + UsingBuffer + " }";
		}

		/// <summary>
		/// Disposes the current instance.
		/// </summary>
		~PixelWorker() {
			Interlocked.Exchange(ref referenceCounter, 0);
			Dispose(false);
		}

		/// <summary>
		/// Disposes of the original image.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Dispose() {
			Dispose(writeOnDispose);
		}

		/// <summary>
		/// Disposes of the original image.
		/// </summary>
		/// <param name="writeChanges">Whether to write any modifications to the image.</param>
		public void Dispose(bool writeChanges) {
			if (IsDisposed)
				return;
			if (!writeOnDispose && writeChanges)
				writeOnDispose = true;
			if (Interlocked.Decrement(ref referenceCounter) > 0)
				return;
			if (Buffer == null || writeChanges)
				WriteChanges(true);
			if (image != null) {
				PixelWorker temp;
				WrapperCollection.TryRemove(image, out temp);
				if (ImageAction == ImageParameterAction.Dispose)
					image.DisposeSafe();
				image = null;
			}
			Buffer = null;
			SkipAlphaInIterator.Dispose();
			IterationStep.Dispose();
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Handles a pointer to an image in memory. DO NOT CALL DISPOSE TWICE.
		/// This intended to be used within a 'using' block.
		/// </summary>
		public struct PointerWrapper : IDisposable {
			/// <summary>
			/// A pointer to the image in memory.
			/// </summary>
			public IntPtr Pointer;
			private GCHandle GCHandle;

			/// <summary>
			/// Gets whether the pointer is disposed.
			/// </summary>
			public bool IsDisposed {
				get {
					return !GCHandle.IsAllocated;
				}
			}

			internal PointerWrapper(PixelWorker parent) {
				if (parent.Buffer == null) {
					unsafe
					{
						Pointer = new IntPtr(parent.scan0);
						GCHandle = new GCHandle();
					}
				} else {
					GCHandle = GCHandle.Alloc(parent.Buffer, GCHandleType.Pinned);
					Pointer = GCHandle.AddrOfPinnedObject();
				}
			}

			/// <summary>
			/// Frees the pointer.
			/// </summary>
			public void Dispose() {
				if (GCHandle.IsAllocated) {
					GCHandle.Free();
					GCHandle = new GCHandle();
				}
			}
		}

		[Serializable]
		private struct DirectComponentEnumerator : IEnumerator<IntPtr>, IEnumerator, IDisposable {
			private unsafe byte* Scan0, Index, Terminator;
			private IntPtr current;
			private PointerWrapper pinnedArray;
			private int increment;

			public IntPtr Current {
#if NET45
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
				get {
					return current;
				}
			}

			object IEnumerator.Current {
#if NET45
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
				get {
					return current;
				}
			}

			public DirectComponentEnumerator(PixelWorker wrapper, int increment) {
				unsafe
				{
					pinnedArray = wrapper.PinPointer();
					Scan0 = (byte*) pinnedArray.Pointer;
					Index = Scan0 - increment;
					Terminator = Scan0 + wrapper.pixelComponentCount;
				}
				this.increment = increment;
				current = IntPtr.Zero;
			}

#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public bool MoveNext() {
				unsafe
				{
					Index += increment;
					if (Index < Terminator) {
						unsafe
						{
							current = new IntPtr(Index);
						}
						return true;
					} else
						return false;
				}
			}

#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public void Reset() {
				unsafe
				{
					Index = Scan0 - increment;
				}
				current = IntPtr.Zero;
			}

#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public void Dispose() {
				pinnedArray.Dispose();
			}
		}

		[Serializable]
		private struct SkipAlphaEnumerator : IEnumerator<IntPtr>, IEnumerator, IDisposable {
			private IntPtr current;
			private PointerWrapper pinnedArray;
			private unsafe byte* Scan0;
			private int PixelComponentCount, Index, increment;

			public IntPtr Current {
#if NET45
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
				get {
					return current;
				}
			}

			object IEnumerator.Current {
#if NET45
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
				get {
					return current;
				}
			}

			public SkipAlphaEnumerator(PixelWorker wrapper, int increment) {
				unsafe
				{
					pinnedArray = wrapper.PinPointer();
					Scan0 = (byte*) pinnedArray.Pointer;
				}
				PixelComponentCount = wrapper.pixelComponentCount;
				this.increment = increment;
				Index = -increment;
				current = IntPtr.Zero;
			}

#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public bool MoveNext() {
				Index += increment;
				if (Index % 4 == 3)
					Index += increment;
				if (Index < PixelComponentCount) {
					unsafe
					{
						current = new IntPtr(Scan0 + Index);
					}
					return true;
				} else
					return false;
			}

#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public void Reset() {
				Index = -increment;
				current = IntPtr.Zero;
			}

#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public void Dispose() {
				pinnedArray.Dispose();
			}
		}
	}
}