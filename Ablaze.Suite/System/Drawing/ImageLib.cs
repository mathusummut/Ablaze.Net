using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Drawing {
	/// <summary>
	/// Represents a mirror mode.
	/// </summary>
	public enum MirrorMode {
		/// <summary>
		/// Mirrors the left half of the image horizontally onto the right half of the image.
		/// </summary>
		HorizontalMirrorLeft,
		/// <summary>
		/// Mirrors the right half of the image horizontally onto the left half of the image.
		/// </summary>
		HorizontalMirrorRight,
		/// <summary>
		/// Mirrors the upper half of the image vertically onto the lower half of the image.
		/// </summary>
		VerticalMirrorTop,
		/// <summary>
		/// Mirrors the lower half of the image vertically onto the upper half of the image.
		/// </summary>
		VerticalMirrorBottom
	}

	/// <summary>
	/// Represents a type of convolution filter to use.
	/// </summary>
	public enum Filter {
		/// <summary>
		/// A gaussian blur filter.
		/// </summary>
		GaussianBlur,
		/// <summary>
		/// An unsharp mask filter.
		/// </summary>
		UnsharpMask,
		/// <summary>
		/// A sharpening filter.
		/// </summary>
		Sharpen
	}

	/// <summary>
	/// Represents a direction.
	/// </summary>
	public enum Direction {
		/// <summary>
		/// Up
		/// </summary>
		Up,
		/// <summary>
		/// Down
		/// </summary>
		Down,
		/// <summary>
		/// Right
		/// </summary>
		Right,
		/// <summary>
		/// Left
		/// </summary>
		Left
	}

	/// <summary>
	/// An image manipulation library written in C#. BgraColor is faster than Color because it directly maps with the underlying Bitmap representation.
	/// Only supports 32-bit BGRA, 24-bit BGR and 8-bit grayscale bitmaps.
	/// </summary>
	public static class ImageLib {
		/// <summary>
		/// Wraparound tiling attributes.
		/// </summary>
		public static readonly ImageAttributes TilingAttributes;
		private static object sharpenSyncRoot = new object();
		private static object blurSyncRoot = new object();
		private static LazyList<Tuple<int, int[][]>> SharpenKernels = new LazyList<Tuple<int, int[][]>>();
		private static LazyList<Tuple<int, int[][]>> GaussianKernels = new LazyList<Tuple<int, int[][]>>();
		private static float[] prewittX = new float[] { 1f, 0f, -1f };
		private static float[] prewittY = new float[] { 1f, 1f, 1f };
		private static float[] xRows = new float[] { -1f, 0f, 1f };
		private static float[][] SobelKernelX = new float[][] { xRows, new float[] { -2f, 0f, 2f }, xRows };
		private static float[][] SobelKernelY = new float[][] { new float[] { -1f, -2f, -1f }, new float[] { 0f, 0f, 0f }, new float[] { 1f, 2f, 1f } };
		private static float[] b1 = new float[] { 1f, 0f, 0f, 0f, 0f };
		private static float[] b2 = new float[] { 0f, 1f, 0f, 0f, 0f };
		private static float[] b3 = new float[] { 0f, 0f, 1f, 0f, 0f };
		private static float[] b5 = new float[] { 0f, 0f, 0f, 0f, 1f };
		private static float[] squareRoots = new float[256];
		private static byte[] logs = new byte[256];
		private static float[] unclampedLogs = new float[256];
		/// <summary>
		/// The loop parallelization cutoff.
		/// </summary>
		public static int ParallelCutoff = 8192;

		static ImageLib() {
			TilingAttributes = new ImageAttributes();
			TilingAttributes.SetWrapMode(WrapMode.TileFlipXY);
			double value;
			for (int i = 0; i < 256; i++) {
				value = i * 0.00392156863;
				squareRoots[i] = (float) Math.Sqrt(value);
				unclampedLogs[i] = (float) (Math.Log(value + 1.0) * 255);
				logs[i] = Clamp(unclampedLogs[i]);
			}
		}

		/// <summary>
		/// Draws a triangle on the specified canvas.
		/// </summary>
		/// <param name="g">The canvas to draw on.</param>
		/// <param name="bounds">The bounds to draw the triangle inside.</param>
		/// <param name="brush">The brush to draw with.</param>
		/// <param name="direction">The direction the triangle is pointing towards.</param>
		public static void DrawTriangle(this Graphics g, RectangleF bounds, Brush brush, Direction direction = Direction.Up) {
			float halfWidth = bounds.Width * 0.5f;
			float halfHeight = bounds.Height * 0.5f;
			PointF p0;
			PointF p1;
			PointF p2;
			switch (direction) {
				case Direction.Down:
					p0 = new PointF(bounds.X + halfWidth, bounds.Bottom);
					p1 = new PointF(bounds.X, bounds.Y);
					p2 = new PointF(bounds.Right, bounds.Y);
					break;
				case Direction.Left:
					p0 = new PointF(bounds.X, bounds.Y + halfHeight);
					p1 = new PointF(bounds.Right, bounds.Y);
					p2 = new PointF(bounds.Right, bounds.Bottom);
					break;
				case Direction.Right:
					p0 = new PointF(bounds.Right, bounds.Y + halfHeight);
					p1 = new PointF(bounds.X, bounds.Bottom);
					p2 = new PointF(bounds.X, bounds.Y);
					break;
				default:
					p0 = new PointF(bounds.X + halfWidth, bounds.Y);
					p1 = new PointF(bounds.X, bounds.Bottom);
					p2 = new PointF(bounds.Right, bounds.Bottom);
					break;
			}
			g.FillPolygon(brush, new PointF[] { p0, p1, p2 });
		}

		/// <summary>
		/// Returns the size of the bitmap after trimming transparent pixels from each side.
		/// </summary>
		/// <param name="source">The bitmap to use as source.</param>
		public static Rectangle GetTrimBounds(this Bitmap source) {
			using (PixelWorker data = PixelWorker.FromImage(source, false, false, ImageParameterAction.RemoveReference))
				return GetTrimBounds(source);
		}

		/// <summary>
		/// Returns the size of the bitmap after trimming transparent pixels from each side.
		/// </summary>
		/// <param name="source">The PixelWorker to use.</param>
		public static Rectangle GetTrimBounds(this PixelWorker source) {
			int xMin = int.MaxValue;
			int xMax = int.MinValue;
			int yMin = int.MaxValue;
			int yMax = int.MinValue;
			int x, y;
			for (x = 0; x < source.Width; x++) {
				for (y = 0; y < source.Height; y++) {
					if (source.GetPixelBgra(x, y).A != 0) {
						xMin = x;
						goto findYMin;
					}
				}
			}
			return Rectangle.Empty;

			findYMin:
			for (y = 0; y < source.Height; y++) {
				for (x = xMin; x < source.Width; x++) {
					if (source.GetPixelBgra(x, y).A != 0) {
						yMin = y;
						goto findXMax;
					}
				}
			}

			findXMax:
			for (x = source.Width - 1; x > xMin; x--) {
				for (y = yMin; y < source.Height; y++) {
					if (source.GetPixelBgra(x, y).A != 0) {
						xMax = x;
						goto findYMax;
					}
				}
			}
			xMax = xMin;

			findYMax:
			for (y = source.Height - 1; y > yMin; y--) {
				for (x = xMin; x <= xMax; x++) {
					if (source.GetPixelBgra(x, y).A != 0) {
						yMax = y;
						goto finished;
					}
				}
			}
			yMax = yMin;

			finished:
			return Rectangle.FromLTRB(xMin, yMin, xMax + 1, yMax + 1);
		}

		/// <summary>
		/// Gets whether the colors appear as equal (the same or alpha is 0 on both).
		/// </summary>
		/// <param name="a">The first color.</param>
		/// <param name="b">The second color.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool LookEqual(this Color a, Color b) {
			return (a.A == 0 && b.A == 0) || a == b;
		}

		/// <summary>
		/// Gets whether the colors appear as equal (the same or alpha is 0 on both).
		/// </summary>
		/// <param name="a">The first color.</param>
		/// <param name="b">The second color.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool LookEqual(this BgraColor a, BgraColor b) {
			return (a.A == 0 && b.A == 0) || a == b;
		}

		/// <summary>
		/// Trims transparent pixels from each side and returns the result.
		/// </summary>
		/// <param name="source">The bitmap to use as source.</param>
		public static Bitmap Trim(this Bitmap source) {
			return Crop(source, GetTrimBounds(source));
		}

		/// <summary>
		/// Blends two images, modifies the base image based on the mix level of the overlay image. The alpha components will be composited with each other.
		/// </summary>
		/// <param name="baseImage">The image to impose on and modify.</param>
		/// <param name="overlayImage">The image to use as overlay. If it is smaller than the base image, then an error will occur, if it is larger, graphics cropping will occur.</param>
		/// <param name="opacity">The opacity multiplier of the overlay.</param>
		/// <param name="layout">The layout style to use.</param>
		/// <param name="interpolation">The interpolation mode to use.</param>
		public static void Overlay(this Bitmap baseImage, Bitmap overlayImage, float opacity, ImageLayout layout = ImageLayout.Stretch, InterpolationMode interpolation = InterpolationMode.HighQualityBicubic) {
			if (baseImage == overlayImage || baseImage == null || overlayImage == null || opacity == 0f)
				return;
			using (PixelWorker lockedBase = PixelWorker.FromImage(baseImage, false, true, ImageParameterAction.RemoveReference))
				Overlay(lockedBase, overlayImage, opacity, layout, interpolation);
		}

		/// <summary>
		/// Blends two images, modifies the base image based on the mix level of the overlay image. The alpha components will be composited with each other.
		/// </summary>
		/// <param name="baseImage">The image to impose on and modify. WriteChanges() is not called, that's on you.</param>
		/// <param name="overlayImage">The image to use as overlay. If it is smaller than the base image, than an error will occur, if it is larger, graphics cropping will occur.</param>
		/// <param name="opacity">The opacity multiplier of the overlay.</param>
		/// <param name="layout">The layout style to use.</param>
		/// <param name="interpolation">The interpolation mode to use.</param>
		public static void Overlay(this PixelWorker baseImage, Bitmap overlayImage, float opacity, ImageLayout layout = ImageLayout.Stretch, InterpolationMode interpolation = InterpolationMode.HighQualityBicubic) {
			if (baseImage == null || overlayImage == null || opacity == 0f)
				return;
			bool disposeOverlayImage = !(baseImage.Size == overlayImage.Size && baseImage.ComponentCount * 8 == Image.GetPixelFormatSize(overlayImage.PixelFormat));
			if (disposeOverlayImage)
				overlayImage = ResizeWithLayout(overlayImage, baseImage.Size, layout, interpolation);
			using (PixelWorker lockedOverlay = PixelWorker.FromImage(overlayImage, false, false, disposeOverlayImage ? ImageParameterAction.Dispose : ImageParameterAction.RemoveReference))
				Overlay(baseImage, lockedOverlay, opacity);
		}

		/// <summary>
		/// Blends two images, modifies the base image based on the mix level of the overlay image. The alpha components will be composited with each other.
		/// </summary>
		/// <param name="baseImage">The image to impose on and modify. WriteChanges() is not called, that's on you.</param>
		/// <param name="overlayImage">The image to use as overlay. Its size must be equal to the size of the base image.</param>
		/// <param name="opacity">The opacity multiplier of the overlay.</param>
		public static void Overlay(this PixelWorker baseImage, PixelWorker overlayImage, float opacity) {
			if (!(baseImage == null || overlayImage == null || opacity == 0f))
				baseImage.ApplyFunctionToAllPixels(Overlay, overlayImage, opacity);
		}

		/// <summary>
		/// Blends two images, modifies the base image based on the mix level of the overlay image. The alpha components will be averaged out, shifted towards with each other depending on the opacity.
		/// </summary>
		/// <param name="baseImage">The image to impose on and modify.</param>
		/// <param name="overlayImage">The image to transition into.</param>
		/// <param name="opacity">The point in time during the transition.</param>
		/// <param name="layout">The layout to use if the overlay image is not the same size as the base image.</param>
		/// <param name="interpolation">The interpolation algorithm to use if the overlay is not the same size.</param>
		public static void Transition(this Bitmap baseImage, Bitmap overlayImage, float opacity, ImageLayout layout = ImageLayout.Stretch, InterpolationMode interpolation = InterpolationMode.HighQualityBicubic) {
			if (baseImage == overlayImage || baseImage == null || overlayImage == null || opacity == 0f)
				return;
			using (PixelWorker lockedBase = PixelWorker.FromImage(baseImage, false, true, ImageParameterAction.RemoveReference))
				Transition(lockedBase, overlayImage, opacity, layout, interpolation);
		}

		/// <summary>
		/// Blends two images, modifies the base image based on the mix level of the overlay image. The alpha components will be averaged out, shifted towards with each other depending on the opacity.
		/// </summary>
		/// <param name="baseImage">The image to impose on and modify. WriteChanges() is not called, that's on you.</param>
		/// <param name="overlayImage">The image to transition into.</param>
		/// <param name="opacity">The point in time during the transition.</param>
		/// <param name="layout">The layout to use if the overlay image is not the same size as the base image.</param>
		/// <param name="interpolation">The interpolation algorithm to use if the overlay is not the same size.</param>
		public static void Transition(this PixelWorker baseImage, Bitmap overlayImage, float opacity, ImageLayout layout = ImageLayout.Stretch, InterpolationMode interpolation = InterpolationMode.HighQualityBicubic) {
			if (baseImage == null || overlayImage == null || opacity == 0f)
				return;
			bool disposeOverlayImage = !(baseImage.Size == overlayImage.Size && baseImage.ComponentCount * 8 == Image.GetPixelFormatSize(overlayImage.PixelFormat));
			if (disposeOverlayImage)
				overlayImage = ResizeWithLayout(overlayImage, baseImage.Size, layout, interpolation);
			using (PixelWorker lockedOverlay = PixelWorker.FromImage(overlayImage, false, false, disposeOverlayImage ? ImageParameterAction.Dispose : ImageParameterAction.RemoveReference))
				Transition(baseImage, lockedOverlay, opacity);
		}

		/// <summary>
		/// Blends two images, modifies the base image based on the mix level of the overlay image. The alpha components will be averaged out, shifted towards with each other depending on the opacity.
		/// </summary>
		/// <param name="baseImage">The image to impose on and modify. WriteChanges() is not called, that's on you.</param>
		/// <param name="overlayImage">The image to transition into. Must be the same size as the base image.</param>
		/// <param name="opacity">The point in time during the transition.</param>
		public static void Transition(this PixelWorker baseImage, PixelWorker overlayImage, float opacity) {
			if (!(baseImage == null || overlayImage == null || opacity == 0f))
				baseImage.ApplyFunctionToAllPixels(Transition, overlayImage, opacity);
		}

		/// <summary>
		/// Blends two images, modifies the base image based on the mix level of the overlay image. The alpha components will be averaged out, shifted towards with each other depending on the opacity.
		/// </summary>
		/// <param name="baseImage">The image to impose on and modify.</param>
		/// <param name="overlayImage">The image to transition into.</param>
		/// <param name="kernel">A kernel [x][y] of opacities where 0 is 100% base pixel, and 1 is 100% overlay pixel.
		/// THE KERNEL MUST BE THE SAME SIZE AS THE BASE IMAGE.</param>
		/// <param name="layout">The layout to use if the overlay image is not the same size as the base image.</param>
		/// <param name="interpolation">The interpolation algorithm to use if the overlay is not the same size.</param>
		public static void Transition(this Bitmap baseImage, Bitmap overlayImage, float[][] kernel, ImageLayout layout = ImageLayout.Stretch, InterpolationMode interpolation = InterpolationMode.HighQualityBicubic) {
			if (baseImage == overlayImage || baseImage == null || overlayImage == null)
				return;
			using (PixelWorker lockedBase = PixelWorker.FromImage(baseImage, false, true, ImageParameterAction.RemoveReference))
				Transition(lockedBase, overlayImage, kernel, layout, interpolation);
		}

		/// <summary>
		/// Blends two images, modifies the base image based on the mix level of the overlay image. The alpha components will be averaged out, shifted towards with each other depending on the opacity.
		/// </summary>
		/// <param name="baseImage">The image to impose on and modify. WriteChanges() is not called, that's on you.</param>
		/// <param name="overlayImage">The image to transition into.</param>
		/// <param name="kernel">A kernel [x][y] of opacities where 0 is 100% base pixel, and 1 is 100% overlay pixel.
		/// THE KERNEL MUST BE THE SAME SIZE AS THE BASE IMAGE.</param>
		/// <param name="layout">The layout to use if the overlay image is not the same size as the base image.</param>
		/// <param name="interpolation">The interpolation algorithm to use if the overlay is not the same size.</param>
		public static void Transition(this PixelWorker baseImage, Bitmap overlayImage, float[][] kernel, ImageLayout layout = ImageLayout.Stretch, InterpolationMode interpolation = InterpolationMode.HighQualityBicubic) {
			if (baseImage == null || overlayImage == null)
				return;
			bool disposeOverlayImage = !(baseImage.Size == overlayImage.Size && baseImage.ComponentCount * 8 == Image.GetPixelFormatSize(overlayImage.PixelFormat));
			if (disposeOverlayImage)
				overlayImage = ResizeWithLayout(overlayImage, baseImage.Size, layout, interpolation);
			using (PixelWorker lockedOverlay = PixelWorker.FromImage(overlayImage, false, false, disposeOverlayImage ? ImageParameterAction.Dispose : ImageParameterAction.RemoveReference))
				Transition(baseImage, lockedOverlay, kernel);
		}

		/// <summary>
		/// Blends two images, modifies the base image based on the mix level of the overlay image. The alpha components will be averaged out, shifted towards with each other depending on the opacity.
		/// </summary>
		/// <param name="baseImage">The image to impose on and modify. WriteChanges() is not called, that's on you.</param>
		/// <param name="overlayImage">The image to transition into. Must be the same size as the base image.</param>
		/// <param name="kernel">A kernel [x][y] of opacities where 0 is 100% base pixel, and 1 is 100% overlay pixel.
		/// THE KERNEL MUST BE THE SAME SIZE AS THE BASE IMAGE.</param>
		public static void Transition(this PixelWorker baseImage, PixelWorker overlayImage, float[][] kernel) {
			if (baseImage == null || overlayImage == null)
				return;
			baseImage.ApplyFunctionToAllPixels(delegate (int i, BgraColor basePixel, BgraColor overlayPixel) {
				float opacity = kernel[i % baseImage.Width][i / baseImage.Width];
				float colorOpacity = overlayPixel.A * opacity * 0.00392156862f;
				return new BgraColor(Clamp(basePixel.A + ((overlayPixel.A - basePixel.A) * opacity)), Clamp(basePixel.R + (overlayPixel.R - basePixel.R) * colorOpacity), Clamp(basePixel.G + (overlayPixel.G - basePixel.G) * colorOpacity), Clamp(basePixel.B + (overlayPixel.B - basePixel.B) * colorOpacity));
			}, overlayImage);
		}

		/// <summary>
		/// Adjusts the contrast of an image.
		/// </summary>
		/// <param name="image">The image whose contrast to adjust.</param>
		/// <param name="value">Sets the contrast from 0 (no contrast) upwards. 1 means the image will not be modified.</param>
		public static void ChangeContrast(this Bitmap image, float value) {
			if (image == null || value == 1f)
				return;
			using (PixelWorker source = PixelWorker.FromImage(image, false, true, ImageParameterAction.RemoveReference))
				ChangeContrast(source, value);
		}

		/// <summary>
		/// Adjusts the contrast of an image.
		/// </summary>
		/// <param name="source">The image whose contrast to adjust. WriteChanges() is not called, that's on you.</param>
		/// <param name="value">Sets the contrast from 0 (no contrast) upwards. 1 means the image will not be modified.</param>
		public static void ChangeContrast(this PixelWorker source, float value) {
			if (source == null || value == 1f)
				return;
			value *= value;
			if (source.ComponentCount == 4) {
				if (source.Buffer == null) {
					unsafe
					{
						ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
							if (i % 4 != 3) {
								byte* index = source.Scan0 + i;
								*index = Clamp(((*index - 128f) * value) + 128f);
							}
						}, ParallelCutoff);
					}
				} else {
					ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
						if (i % 4 != 3)
							source.Buffer[i] = Clamp(((source.Buffer[i] - 128f) * value) + 128f);
					}, ParallelCutoff);
				}
			} else {
				if (source.Buffer == null) {
					unsafe
					{
						ParallelLoop.For(source.Scan0, source.PixelComponentCount, 1L, i => {
							byte* index = (byte*) i;
							*index = Clamp(((*index - 128f) * value) + 128f);
						}, ParallelCutoff);
					}
				} else {
					ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
						source.Buffer[i] = Clamp(((source.Buffer[i] - 128f) * value) + 128f);
					}, ParallelCutoff);
				}
			}
		}

		/// <summary>
		/// Changes the brightness of the specified image.
		/// </summary>
		/// <param name="image">The image whose brightness to change.</param>
		/// <param name="multiplier">The brightness multiplier (0 means black, 1 means brightness is unchanged, larger numbers increase brightness).</param>
		public static void ChangeBrightness(this Bitmap image, float multiplier) {
			if (image == null || multiplier == 1f)
				return;
			using (PixelWorker source = PixelWorker.FromImage(image, false, true, ImageParameterAction.RemoveReference))
				ChangeBrightness(source, multiplier);
		}

		/// <summary>
		/// Changes the brightness of the specified image.
		/// </summary>
		/// <param name="source">The image whose brightness to change. WriteChanges() is not called, that's on you.</param>
		/// <param name="multiplier">The brightness multiplier (0 means black, 1 means brightness is unchanged, larger numbers increase brightness).</param>
		public static void ChangeBrightness(this PixelWorker source, float multiplier) {
			if (source == null || multiplier == 1f)
				return;
			else if (source.ComponentCount == 4) {
				if (source.Buffer == null) {
					unsafe
					{
						ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
							if (i % 4 != 3) {
								byte* index = source.Scan0 + i;
								*index = Clamp(*index * multiplier);
							}
						}, ParallelCutoff);
					}
				} else {
					ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
						if (i % 4 != 3)
							source.Buffer[i] = Clamp(source.Buffer[i] * multiplier);
					}, ParallelCutoff);
				}
			} else {
				if (source.Buffer == null) {
					unsafe
					{
						ParallelLoop.For(source.Scan0, source.PixelComponentCount, 1L, i => {
							byte* index = (byte*) i;
							*index = Clamp(*index * multiplier);
						}, ParallelCutoff);
					}
				} else {
					ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
						source.Buffer[i] = Clamp(source.Buffer[i] * multiplier);
					}, ParallelCutoff);
				}
			}
		}

		/// <summary>
		/// Changes the brightness of the specified image and normalizes before truncation.
		/// </summary>
		/// <param name="image">The image whose brightness to change.</param>
		/// <param name="multiplier">The brightness multiplier (0 means black, 1 means brightness is unchanged, larger numbers increase brightness).</param>
		public static void ChangeBrightnessNormalize(this Bitmap image, float multiplier) {
			if (image == null)
				return;
			using (PixelWorker source = PixelWorker.FromImage(image, false, true, ImageParameterAction.RemoveReference))
				ChangeBrightnessNormalize(source, multiplier);
		}

		/// <summary>
		/// Changes the brightness of the specified image and normalizes before truncation.
		/// </summary>
		/// <param name="source">The image whose brightness to change. WriteChanges() is not called, that's on you.</param>
		/// <param name="multiplier">The brightness multiplier (0 means black, 1 means brightness is unchanged, larger numbers increase brightness).</param>
		public static void ChangeBrightnessNormalize(this PixelWorker source, float multiplier) {
			if (source == null)
				return;
			else if (source.ComponentCount == 4) {
				float[] results = new float[source.PixelComponentCount];
				float val, max = 0f;
				for (int i = 0; i < results.Length; i++) {
					if (i % 4 != 3) {
						val = source[i] * multiplier;
						results[i] = val;
						if (val > max)
							val = max;
					}
				}
				if (max == 0f)
					max = 1f;
				else
					max = 255f / max;
				ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
					if (i % 4 != 3)
						source[i] = (byte) (results[i] * max);
				}, ParallelCutoff);
			} else {
				float[] results = new float[source.PixelComponentCount];
				float val, max = 0f;
				for (int i = 0; i < results.Length; i++) {
					val = source[i] * multiplier;
					results[i] = val;
					if (val > max)
						val = max;
				}
				if (max == 0f)
					max = 1f;
				else
					max = 255f / max;
				ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
					source[i] = (byte) (results[i] * max);
				}, ParallelCutoff);
			}
		}

		/// <summary>
		/// Raises the values of the specified image with the specified exponent.
		/// </summary>
		/// <param name="image">The image whose values to raise.</param>
		/// <param name="exponent">The exponent to raise with.</param>
		public static void RaiseBy(this Bitmap image, double exponent) {
			if (image == null || exponent == 1.0f)
				return;
			using (PixelWorker source = PixelWorker.FromImage(image, false, true, ImageParameterAction.RemoveReference))
				RaiseBy(source, exponent);
		}

		/// <summary>
		/// Raises the values of the specified image with the specified exponent.
		/// </summary>
		/// <param name="source">The image whose values to raise. WriteChanges() is not called, that's on you.</param>
		/// <param name="exponent">The exponent to raise with.</param>
		public static void RaiseBy(this PixelWorker source, double exponent) {
			if (source == null || exponent == 1f)
				return;
			byte[] lookup = new byte[256];
			for (int i = 0; i < 256; i++)
				lookup[i] = (byte) Math.Min(255, Math.Pow(i * 0.00392156862, exponent) * 255.0);
			if (source.ComponentCount == 4) {
				if (source.Buffer == null) {
					unsafe
					{
						ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
							if (i % 4 != 3) {
								byte* index = source.Scan0 + i;
								*index = lookup[*index];
							}
						}, ParallelCutoff);
					}
				} else {
					ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
						if (i % 4 != 3)
							source.Buffer[i] = lookup[source.Buffer[i]];
					}, ParallelCutoff);
				}
			} else {
				if (source.Buffer == null) {
					unsafe
					{
						ParallelLoop.For(source.Scan0, source.PixelComponentCount, 1L, i => {
							byte* index = (byte*) i;
							*index = lookup[*index];
						}, ParallelCutoff);
					}
				} else {
					ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
						source.Buffer[i] = lookup[source.Buffer[i]];
					}, ParallelCutoff);
				}
			}
		}

		/// <summary>
		/// Dilates then erodes the image at the specified radius.
		/// </summary>
		/// <param name="image">The image to open morphologically.</param>
		/// <param name="radius">The radius to open at.</param>
		public static void OpenFilter(this Bitmap image, int radius) {
			if (image == null || radius <= 0)
				return;
			using (PixelWorker source = PixelWorker.FromImage(image, false, true, ImageParameterAction.RemoveReference))
				OpenFilter(source, radius);
		}

		/// <summary>
		/// Dilates then erodes the image at the specified radius.
		/// </summary>
		/// <param name="source">The image to open morphologically. WriteChanges() is not called, that's on you.</param>
		/// <param name="radius">The radius to open at.</param>
		public static void OpenFilter(this PixelWorker source, int radius) {
			if (source == null || radius <= 0)
				return;
			using (PixelWorker buffer = new PixelWorker(source.Width, source.Height, source.Format)) {
				Dilate(source, buffer, radius);
				Erode(buffer, source, radius);
			}
		}

		/// <summary>
		/// Dilates then erodes the image at the specified radius.
		/// </summary>
		/// <param name="image">The image to open morphologically.</param>
		/// <param name="radius">The radius to open at.</param>
		public static void OpenFilter(this Bitmap image, float radius) {
			if (image == null || radius <= 0)
				return;
			using (PixelWorker source = PixelWorker.FromImage(image, false, true, ImageParameterAction.RemoveReference))
				OpenFilter(source, radius);
		}

		/// <summary>
		/// Dilates then erodes the image at the specified radius.
		/// </summary>
		/// <param name="source">The image to open morphologically. WriteChanges() is not called, that's on you.</param>
		/// <param name="radius">The radius to open at.</param>
		public static void OpenFilter(this PixelWorker source, float radius) {
			if (source == null || radius <= 0)
				return;
			using (Bitmap bitmap = Dilate(source, radius)) {
				using (Bitmap temp = Erode(bitmap, radius))
					source.CopyFrom(temp);
			}
		}

		/// <summary>
		/// Erodes then dilates the image at the specified radius.
		/// </summary>
		/// <param name="image">The image to close morphologically.</param>
		/// <param name="radius">The radius to close at.</param>
		public static void CloseFilter(this Bitmap image, int radius) {
			if (image == null || radius <= 0)
				return;
			using (PixelWorker source = PixelWorker.FromImage(image, false, true, ImageParameterAction.RemoveReference))
				CloseFilter(source, radius);
		}

		/// <summary>
		/// Erodes then dilates the image at the specified radius.
		/// </summary>
		/// <param name="source">The image to close morphologically. WriteChanges() is not called, that's on you.</param>
		/// <param name="radius">The radius to close at.</param>
		public static void CloseFilter(this PixelWorker source, int radius) {
			if (source == null || radius <= 0)
				return;
			using (PixelWorker buffer = new PixelWorker(source.Width, source.Height, source.Format)) {
				Erode(source, buffer, radius);
				Dilate(buffer, source, radius);
			}
		}

		/// <summary>
		/// Erodes then dilates the image at the specified radius.
		/// </summary>
		/// <param name="image">The image to close morphologically.</param>
		/// <param name="radius">The radius to close at.</param>
		public static void CloseFilter(this Bitmap image, float radius) {
			if (image == null || radius <= 0)
				return;
			using (PixelWorker source = PixelWorker.FromImage(image, false, true, ImageParameterAction.RemoveReference))
				CloseFilter(source, radius);
		}

		/// <summary>
		/// Erodes then dilates the image at the specified radius.
		/// </summary>
		/// <param name="source">The image to close morphologically. WriteChanges() is not called, that's on you.</param>
		/// <param name="radius">The radius to close at.</param>
		public static void CloseFilter(this PixelWorker source, float radius) {
			if (source == null || radius <= 0)
				return;
			using (Bitmap bitmap = Erode(source, radius)) {
				using (Bitmap temp = Dilate(bitmap, radius))
					source.CopyFrom(temp);
			}
		}

		/// <summary>
		/// Adds salt and pepper noise to the specified image.
		/// </summary>
		/// <param name="image">The image whose values to add random noise to.</param>
		/// <param name="frequency">A value between 0 and 1, where 0 indicates no noise is to be added, and 1 indicates noise only is to remain.</param>
		/// <param name="pepper">The magnitude of the pepper.</param>
		/// <param name="salt">The magnitude of the salt.</param>
		public static void SaltAndPepperNoise(this Bitmap image, double frequency = 0.08, byte salt = 255, byte pepper = 0) {
			if (image == null || frequency == 0.0f)
				return;
			using (PixelWorker source = PixelWorker.FromImage(image, false, true, ImageParameterAction.RemoveReference))
				SaltAndPepperNoise(source, frequency, salt, pepper);
		}

		/// <summary>
		/// Adds salt and pepper noise to the specified image.
		/// </summary>
		/// <param name="source">The image whose values to add random noise to. WriteChanges is not called, that's on you.</param>
		/// <param name="frequency">A value between 0 and 1, where 0 indicates no noise is to be added, and 1 indicates noise only is to remain.</param>
		/// <param name="pepper">The magnitude of the pepper.</param>
		/// <param name="salt">The magnitude of the salt.</param>
		public static void SaltAndPepperNoise(this PixelWorker source, double frequency = 0.08, byte salt = 255, byte pepper = 0) {
			if (source == null || frequency == 1f)
				return;
			double minAllowed = frequency * 0.5;
			double maxAllowed = 1.0 - minAllowed;
			if (source.Buffer == null) {
				unsafe
				{
					ParallelLoop.For(source.Scan0, source.PixelComponentCount, 1L, i => {
						byte* index = (byte*) i;
						double random = UniformRandom.RandomDouble;
						if (random < minAllowed)
							*index = pepper;
						else if (random > maxAllowed)
							*index = salt;
					}, ParallelCutoff);
				}
			} else {
				ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
					double random = UniformRandom.RandomDouble;
					if (random <= minAllowed)
						source.Buffer[i] = pepper;
					else if (random >= maxAllowed)
						source.Buffer[i] = salt;
				}, ParallelCutoff);
			}
		}

		/// <summary>
		/// Applies base e logarithm to the specified image.
		/// </summary>
		/// <param name="image">The image to log.</param>
		public static void Logarithm(this Bitmap image) {
			if (image == null)
				return;
			using (PixelWorker source = PixelWorker.FromImage(image, false, true, ImageParameterAction.RemoveReference))
				Logarithm(source);
		}

		/// <summary>
		/// Applies base e logarithm to the specified image.
		/// </summary>
		/// <param name="source">The image to log. WriteChanges() is not called, that's on you.</param>
		public static void Logarithm(this PixelWorker source) {
			if (source == null)
				return;
			float[] values = new float[source.PixelComponentCount];
			if (source.ComponentCount == 4) {
				if (source.Buffer == null) {
					unsafe
					{
						byte* scan0 = source.Scan0;
						for (int i = 0; i < values.Length; i++)
							values[i] = i % 4 == 3 ? scan0[i] : unclampedLogs[scan0[i]];
					}
				} else {
					byte[] buffer = source.Buffer;
					for (int i = 0; i < buffer.Length; i++) {
						values[i] = i % 4 == 3 ? buffer[i] : unclampedLogs[buffer[i]];
					}
				}
			} else {
				if (source.Buffer == null) {
					unsafe
					{
						byte* scan0 = source.Scan0;
						for (int i = 0; i < values.Length; i++)
							values[i] = unclampedLogs[scan0[i]];
					}
				} else {
					byte[] buffer = source.Buffer;
					for (int i = 0; i < buffer.Length; i++)
						values[i] = unclampedLogs[buffer[i]];
				}
			}
			source.CopyFrom(values, true);
		}

		/// <summary>
		/// Applies base e logarithm to the specified color.
		/// </summary>
		/// <param name="color">The color to log.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static BgraColor Logarithm(BgraColor color) {
			return new BgraColor(logs[color.A], logs[color.R], logs[color.G], logs[color.B]);
		}

		/// <summary>
		/// Applies base e logarithm to the specified color.
		/// </summary>
		/// <param name="color">The color to log.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Color Logarithm(Color color) {
			return Color.FromArgb(logs[color.A], logs[color.R], logs[color.G], logs[color.B]);
		}

		/// <summary>
		/// Adds the specified image to the bitmap. If the images are not the same size, the second image is stretched to the size of the first one before addition.
		/// </summary>
		/// <param name="baseImage">The bitmap to overwrite.</param>
		/// <param name="toAdd">The image to add to the other one.</param>
		/// <param name="layout">The layout style to use.</param>
		/// <param name="interpolation">The interpolation mode to use.</param>
		public static void AddWith(this Bitmap baseImage, Bitmap toAdd, ImageLayout layout = ImageLayout.Stretch, InterpolationMode interpolation = InterpolationMode.HighQualityBicubic) {
			if (baseImage == null || toAdd == null)
				return;
			using (PixelWorker source = PixelWorker.FromImage(baseImage, false, true, ImageParameterAction.RemoveReference))
				AddWith(source, toAdd, layout, interpolation);
		}

		/// <summary>
		/// Adds the specified image to the bitmap.
		/// </summary>
		/// <param name="baseImage">The bitmap to overwrite.</param>
		/// <param name="toAdd">The image to add to the other one.</param>
		/// <param name="layout">The layout style to use.</param>
		/// <param name="interpolation">The interpolation mode to use.</param>
		public static void AddWith(this PixelWorker baseImage, Bitmap toAdd, ImageLayout layout = ImageLayout.Stretch, InterpolationMode interpolation = InterpolationMode.HighQualityBicubic) {
			if (baseImage == null || toAdd == null)
				return;
			bool disposeOverlayImage = !(baseImage.Size == toAdd.Size && baseImage.ComponentCount * 8 == Image.GetPixelFormatSize(toAdd.PixelFormat));
			if (disposeOverlayImage)
				toAdd = ResizeWithLayout(toAdd, baseImage.Size, layout, interpolation);
			using (PixelWorker lockedOverlay = PixelWorker.FromImage(toAdd, false, false, disposeOverlayImage ? ImageParameterAction.Dispose : ImageParameterAction.RemoveReference))
				AddWith(baseImage, lockedOverlay);
		}

		/// <summary>
		/// Adds the specified image to the bitmap.
		/// </summary>
		/// <param name="baseImage">The bitmap to overwrite. WriteChanges() is not called, that's on you.</param>
		/// <param name="toAdd">The image to add to the other one. Its size must be equal to the size of the base image.</param>
		public static void AddWith(this PixelWorker baseImage, PixelWorker toAdd) {
			if (!(baseImage == null || toAdd == null))
				baseImage.ApplyFunctionToAllPixels(Add, toAdd);
		}

		/// <summary>
		/// Applies a bitwise 'and' operation for the specified images.
		/// </summary>
		/// <param name="baseImage">The bitmap to overwrite.</param>
		/// <param name="mask">The image to mask the other one.</param>
		/// <param name="ignoreAlpha">If true, alpha is not masked.</param>
		/// <param name="layout">The layout style to use.</param>
		/// <param name="interpolation">The interpolation mode to use.</param>
		public static void BitwiseAnd(this Bitmap baseImage, Bitmap mask, bool ignoreAlpha = true, ImageLayout layout = ImageLayout.Stretch, InterpolationMode interpolation = InterpolationMode.HighQualityBicubic) {
			if (baseImage == null || mask == null)
				return;
			using (PixelWorker source = PixelWorker.FromImage(baseImage, false, true, ImageParameterAction.RemoveReference))
				BitwiseAnd(source, mask, ignoreAlpha, layout, interpolation);
		}

		/// <summary>
		/// Applies a bitwise 'and' operation for the specified images.
		/// </summary>
		/// <param name="baseImage">The bitmap to overwrite.</param>
		/// <param name="mask">The image to mask the other one.</param>
		/// <param name="ignoreAlpha">If true, alpha is not masked.</param>
		/// <param name="layout">The layout style to use.</param>
		/// <param name="interpolation">The interpolation mode to use.</param>
		public static void BitwiseAnd(this PixelWorker baseImage, Bitmap mask, bool ignoreAlpha = true, ImageLayout layout = ImageLayout.Stretch, InterpolationMode interpolation = InterpolationMode.HighQualityBicubic) {
			if (baseImage == null || mask == null)
				return;
			bool disposeOverlayImage = !(baseImage.Size == mask.Size && baseImage.ComponentCount * 8 == Image.GetPixelFormatSize(mask.PixelFormat));
			if (disposeOverlayImage)
				mask = ResizeWithLayout(mask, baseImage.Size, layout, interpolation);
			using (PixelWorker lockedOverlay = PixelWorker.FromImage(mask, false, false, disposeOverlayImage ? ImageParameterAction.Dispose : ImageParameterAction.RemoveReference))
				BitwiseAnd(baseImage, lockedOverlay, ignoreAlpha);
		}

		/// <summary>
		/// Applies a bitwise 'and' operation for the specified images.
		/// </summary>
		/// <param name="baseImage">The bitmap to overwrite. WriteChanges() is not called, that's on you.</param>
		/// <param name="mask">The image to mask the other one. Its size must be equal to the size of the base image.</param>
		/// <param name="ignoreAlpha">If true, alpha is not masked.</param>
		public static void BitwiseAnd(this PixelWorker baseImage, PixelWorker mask, bool ignoreAlpha = true) {
			if (baseImage == null || mask == null)
				return;
			else if (ignoreAlpha) {
				baseImage.ApplyFunctionToAllPixels(delegate (int i, BgraColor color1, BgraColor color2) {
					return new BgraColor(color1.A, (byte) (color1.R & color2.R), (byte) (color1.G & color2.G), (byte) (color1.B & color2.B));
				}, mask);
			} else {
				baseImage.ApplyFunctionToAllPixels(delegate (int i, BgraColor color1, BgraColor color2) {
					return new BgraColor((byte) (color1.A & color2.A), (byte) (color1.R & color2.R), (byte) (color1.G & color2.G), (byte) (color1.B & color2.B));
				}, mask);
			}
		}

		/// <summary>
		/// Applies a bitwise 'or' operation for the specified images.
		/// </summary>
		/// <param name="baseImage">The bitmap to overwrite.</param>
		/// <param name="mask">The image to mask the other one.</param>
		/// <param name="layout">The layout style to use.</param>
		/// <param name="interpolation">The interpolation mode to use.</param>
		public static void BitwiseOr(this Bitmap baseImage, Bitmap mask, ImageLayout layout = ImageLayout.Stretch, InterpolationMode interpolation = InterpolationMode.HighQualityBicubic) {
			if (baseImage == null || mask == null)
				return;
			using (PixelWorker source = PixelWorker.FromImage(baseImage, false, true, ImageParameterAction.RemoveReference))
				BitwiseOr(source, mask, layout, interpolation);
		}

		/// <summary>
		/// Applies a bitwise 'or' operation for the specified images.
		/// </summary>
		/// <param name="baseImage">The bitmap to overwrite.</param>
		/// <param name="mask">The image to mask the other one.</param>
		/// <param name="layout">The layout style to use.</param>
		/// <param name="interpolation">The interpolation mode to use.</param>
		public static void BitwiseOr(this PixelWorker baseImage, Bitmap mask, ImageLayout layout = ImageLayout.Stretch, InterpolationMode interpolation = InterpolationMode.HighQualityBicubic) {
			if (baseImage == null || mask == null)
				return;
			bool disposeOverlayImage = !(baseImage.Size == mask.Size && baseImage.ComponentCount * 8 == Image.GetPixelFormatSize(mask.PixelFormat));
			if (disposeOverlayImage)
				mask = ResizeWithLayout(mask, baseImage.Size, layout, interpolation);
			using (PixelWorker lockedOverlay = PixelWorker.FromImage(mask, false, false, disposeOverlayImage ? ImageParameterAction.Dispose : ImageParameterAction.RemoveReference))
				BitwiseOr(baseImage, lockedOverlay);
		}

		/// <summary>
		/// Applies a bitwise 'or' operation for the specified images.
		/// </summary>
		/// <param name="baseImage">The bitmap to overwrite. WriteChanges() is not called, that's on you.</param>
		/// <param name="mask">The image to mask the other one. Its size must be equal to the size of the base image.</param>
		public static void BitwiseOr(this PixelWorker baseImage, PixelWorker mask) {
			if (!(baseImage == null || mask == null))
				baseImage.ApplyFunctionToAllPixels(BitwiseOr, mask);
		}

		private static BgraColor BitwiseOr(int i, BgraColor color1, BgraColor color2) {
			return new BgraColor((byte) (color1.A | color2.A), (byte) (color1.R | color2.R), (byte) (color1.G | color2.G), (byte) (color1.B | color2.B));
		}

		/// <summary>
		/// Applies a bitwise 'xor' operation for the specified images.
		/// </summary>
		/// <param name="baseImage">The bitmap to overwrite.</param>
		/// <param name="mask">The image to mask the other one.</param>
		/// <param name="ignoreAlpha">If true, alpha is not masked.</param>
		/// <param name="layout">The layout style to use.</param>
		/// <param name="interpolation">The interpolation mode to use.</param>
		public static void BitwiseXor(this Bitmap baseImage, Bitmap mask, bool ignoreAlpha = true, ImageLayout layout = ImageLayout.Stretch, InterpolationMode interpolation = InterpolationMode.HighQualityBicubic) {
			if (baseImage == null || mask == null)
				return;
			using (PixelWorker source = PixelWorker.FromImage(baseImage, false, true, ImageParameterAction.RemoveReference))
				BitwiseXor(source, mask, ignoreAlpha, layout, interpolation);
		}

		/// <summary>
		/// Applies a bitwise 'xor' operation for the specified images.
		/// </summary>
		/// <param name="baseImage">The bitmap to overwrite.</param>
		/// <param name="mask">The image to mask the other one.</param>
		/// <param name="ignoreAlpha">If true, alpha is not masked.</param>
		/// <param name="layout">The layout style to use.</param>
		/// <param name="interpolation">The interpolation mode to use.</param>
		public static void BitwiseXor(this PixelWorker baseImage, Bitmap mask, bool ignoreAlpha = true, ImageLayout layout = ImageLayout.Stretch, InterpolationMode interpolation = InterpolationMode.HighQualityBicubic) {
			if (baseImage == null || mask == null)
				return;
			bool disposeOverlayImage = !(baseImage.Size == mask.Size && baseImage.ComponentCount * 8 == Image.GetPixelFormatSize(mask.PixelFormat));
			if (disposeOverlayImage)
				mask = ResizeWithLayout(mask, baseImage.Size, layout, interpolation);
			using (PixelWorker lockedOverlay = PixelWorker.FromImage(mask, false, false, disposeOverlayImage ? ImageParameterAction.Dispose : ImageParameterAction.RemoveReference))
				BitwiseXor(baseImage, lockedOverlay, ignoreAlpha);
		}

		/// <summary>
		/// Applies a bitwise 'xor' operation for the specified images.
		/// </summary>
		/// <param name="baseImage">The bitmap to overwrite. WriteChanges() is not called, that's on you.</param>
		/// <param name="mask">The image to mask the other one. Its size must be equal to the size of the base image.</param>
		/// <param name="ignoreAlpha">If true, alpha is not masked.</param>
		public static void BitwiseXor(this PixelWorker baseImage, PixelWorker mask, bool ignoreAlpha = true) {
			if (baseImage == null || mask == null)
				return;
			else if (ignoreAlpha) {
				baseImage.ApplyFunctionToAllPixels(delegate (int i, BgraColor color1, BgraColor color2) {
					return new BgraColor(color1.A, (byte) (color1.R ^ color2.R), (byte) (color1.G ^ color2.G), (byte) (color1.B ^ color2.B));
				}, mask);
			} else {
				baseImage.ApplyFunctionToAllPixels(delegate (int i, BgraColor color1, BgraColor color2) {
					return new BgraColor((byte) (color1.A ^ color2.A), (byte) (color1.R ^ color2.R), (byte) (color1.G ^ color2.G), (byte) (color1.B ^ color2.B));
				}, mask);
			}
		}

		/// <summary>
		/// Adds the specified image to the bitmap and normalizes the output before clipping.
		/// If the images are not the same size, the second image is stretched to the size of the first one before addition.
		/// </summary>
		/// <param name="baseImage">The bitmap to overwrite.</param>
		/// <param name="toAdd">The image to add to the other one.</param>
		/// <param name="layout">The layout style to use.</param>
		/// <param name="interpolation">The interpolation mode to use.</param>
		public static void AddAndNormalizeWith(this Bitmap baseImage, Bitmap toAdd, ImageLayout layout = ImageLayout.Stretch, InterpolationMode interpolation = InterpolationMode.HighQualityBicubic) {
			if (baseImage == null || toAdd == null)
				return;
			using (PixelWorker source = PixelWorker.FromImage(baseImage, false, true, ImageParameterAction.RemoveReference))
				AddAndNormalizeWith(source, toAdd, layout, interpolation);
		}

		/// <summary>
		/// Adds the specified image to the bitmap and normalizes the output before clipping.
		/// If the images are not the same size, the second image is stretched to the size of the first one before addition.
		/// </summary>
		/// <param name="baseImage">The bitmap to overwrite.</param>
		/// <param name="toAdd">The image to add to the other one.</param>
		/// <param name="layout">The layout style to use.</param>
		/// <param name="interpolation">The interpolation mode to use.</param>
		public static void AddAndNormalizeWith(this PixelWorker baseImage, Bitmap toAdd, ImageLayout layout = ImageLayout.Stretch, InterpolationMode interpolation = InterpolationMode.HighQualityBicubic) {
			if (baseImage == null || toAdd == null)
				return;
			bool disposeOverlayImage = !(baseImage.Size == toAdd.Size && baseImage.ComponentCount * 8 == Image.GetPixelFormatSize(toAdd.PixelFormat));
			if (disposeOverlayImage)
				toAdd = ResizeWithLayout(toAdd, baseImage.Size, layout, interpolation);
			using (PixelWorker lockedOverlay = PixelWorker.FromImage(toAdd, false, false, disposeOverlayImage ? ImageParameterAction.Dispose : ImageParameterAction.RemoveReference))
				AddAndNormalizeWith(baseImage, lockedOverlay);
		}

		/// <summary>
		/// Adds the specified image to the bitmap and normalizes the output before clipping.
		/// If the images are not the same size, the second image is stretched to the size of the first one before addition.
		/// </summary>
		/// <param name="baseImage">The bitmap to overwrite.</param>
		/// <param name="toAdd">The image to add to the other one.</param>
		public static void AddAndNormalizeWith(this PixelWorker baseImage, PixelWorker toAdd) {
			if (baseImage == null || toAdd == null)
				return;
			int pixelComponentCount = baseImage.PixelComponentCount;
			short temp, max = 0;
			short[] components = new short[pixelComponentCount];
			for (int i = 0; i < pixelComponentCount; i++) {
				temp = (short) (baseImage[i] + toAdd[i]);
				components[i] = temp;
				if (temp > max)
					max = temp;
			}
			float mult = max == 0 ? 0f : 255f / max;
			ParallelLoop.For(0, pixelComponentCount, i => {
				baseImage[i] = (byte) (components[i] * mult);
			}, ParallelCutoff);
		}

		/// <summary>
		/// Multiplies the specified image to the bitmap and normalizes the output before clipping.
		/// If the images are not the same size, the second image is stretched to the size of the first one before multiplication.
		/// </summary>
		/// <param name="baseImage">The bitmap to overwrite.</param>
		/// <param name="toMult">The image to multiply with the other one.</param>
		/// <param name="layout">The layout style to use.</param>
		/// <param name="interpolation">The interpolation mode to use.</param>
		public static void MultiplyAndNormalizeWith(this Bitmap baseImage, Bitmap toMult, ImageLayout layout = ImageLayout.Stretch, InterpolationMode interpolation = InterpolationMode.HighQualityBicubic) {
			if (baseImage == null || toMult == null)
				return;
			using (PixelWorker source = PixelWorker.FromImage(baseImage, false, true, ImageParameterAction.RemoveReference))
				MultiplyAndNormalizeWith(source, toMult, layout, interpolation);
		}

		/// <summary>
		/// Multiplies the specified image to the bitmap and normalizes the output before clipping.
		/// If the images are not the same size, the second image is stretched to the size of the first one before multiplication.
		/// </summary>
		/// <param name="baseImage">The bitmap to overwrite.</param>
		/// <param name="toMult">The image to multiply with the other one.</param>
		/// <param name="layout">The layout style to use.</param>
		/// <param name="interpolation">The interpolation mode to use.</param>
		public static void MultiplyAndNormalizeWith(this PixelWorker baseImage, Bitmap toMult, ImageLayout layout = ImageLayout.Stretch, InterpolationMode interpolation = InterpolationMode.HighQualityBicubic) {
			if (baseImage == null || toMult == null)
				return;
			bool disposeOverlayImage = !(baseImage.Size == toMult.Size && baseImage.ComponentCount * 8 == Image.GetPixelFormatSize(toMult.PixelFormat));
			if (disposeOverlayImage)
				toMult = ResizeWithLayout(toMult, baseImage.Size, layout, interpolation);
			using (PixelWorker lockedOverlay = PixelWorker.FromImage(toMult, false, false, disposeOverlayImage ? ImageParameterAction.Dispose : ImageParameterAction.RemoveReference))
				MultiplyAndNormalizeWith(baseImage, lockedOverlay);
		}

		/// <summary>
		/// Multiplies the specified image to the bitmap and normalizes the output before clipping.
		/// If the images are not the same size, the second image is stretched to the size of the first one before multiplication.
		/// </summary>
		/// <param name="baseImage">The bitmap to overwrite.</param>
		/// <param name="toMult">The image to multiply with the other one.</param>
		public static void MultiplyAndNormalizeWith(this PixelWorker baseImage, PixelWorker toMult) {
			if (baseImage == null || toMult == null)
				return;
			int pixelComponentCount = baseImage.PixelComponentCount;
			int temp, max = 0;
			int[] components = new int[pixelComponentCount];
			for (int i = 0; i < pixelComponentCount; i++) {
				temp = baseImage[i] * toMult[i];
				components[i] = temp;
				if (temp > max)
					max = temp;
			}
			float mult = max == 0 ? 0f : 255f / max;
			ParallelLoop.For(0, pixelComponentCount, i => {
				baseImage[i] = (byte) (components[i] * mult);
			}, ParallelCutoff);
		}

		/// <summary>
		/// Subtracts the specified image from the bitmap. If the images are not the same size, the second image is stretched to the size of the first one before subtraction.
		/// </summary>
		/// <param name="baseImage">The bitmap to overwrite.</param>
		/// <param name="filter">The image to subtract from the other one.</param>
		/// <param name="layout">The layout style to use.</param>
		/// <param name="interpolation">The interpolation mode to use.</param>
		/// <param name="subtractAlpha">Whether to subtract any alpha components as well.</param>
		public static void Subtract(this Bitmap baseImage, Bitmap filter, bool subtractAlpha = false, ImageLayout layout = ImageLayout.Stretch, InterpolationMode interpolation = InterpolationMode.HighQualityBicubic) {
			if (baseImage == null || filter == null)
				return;
			using (PixelWorker source = PixelWorker.FromImage(baseImage, false, true, ImageParameterAction.RemoveReference))
				Subtract(source, filter, subtractAlpha, layout, interpolation);
		}

		/// <summary>
		/// Subtracts the specified image from the bitmap.
		/// </summary>
		/// <param name="baseImage">The bitmap to overwrite.</param>
		/// <param name="filter">The image to subtract from the other one.</param>
		/// <param name="layout">The layout style to use.</param>
		/// <param name="interpolation">The interpolation mode to use.</param>
		/// <param name="subtractAlpha">Whether to subtract any alpha components as well.</param>
		public static void Subtract(this PixelWorker baseImage, Bitmap filter, bool subtractAlpha = false, ImageLayout layout = ImageLayout.Stretch, InterpolationMode interpolation = InterpolationMode.HighQualityBicubic) {
			if (baseImage == null || filter == null)
				return;
			bool disposeOverlayImage = !(baseImage.Size == filter.Size && baseImage.ComponentCount * 8 == Image.GetPixelFormatSize(filter.PixelFormat));
			if (disposeOverlayImage)
				filter = ResizeWithLayout(filter, baseImage.Size, layout, interpolation);
			using (PixelWorker lockedOverlay = PixelWorker.FromImage(filter, false, false, disposeOverlayImage ? ImageParameterAction.Dispose : ImageParameterAction.RemoveReference))
				Subtract(baseImage, lockedOverlay, subtractAlpha);
		}

		/// <summary>
		/// Subtracts the specified image from the bitmap.
		/// </summary>
		/// <param name="baseImage">The bitmap to overwrite. WriteChanges() is not called, that's on you.</param>
		/// <param name="filter">The image to subtract from the other one. Its size must be equal to the size of the base image.</param>
		/// <param name="subtractAlpha">Whether to subtract any alpha components as well.</param>
		public static void Subtract(this PixelWorker baseImage, PixelWorker filter, bool subtractAlpha = false) {
			if (baseImage == null || filter == null)
				return;
			Func<int, BgraColor, BgraColor, BgraColor> subtract;
			if (subtractAlpha)
				subtract = (i, left, right) => new BgraColor(Clamp(left.A - right.A), Clamp(left.R - right.R), Clamp(left.G - right.G), Clamp(left.B - right.B));
			else
				subtract = (i, left, right) => new BgraColor(left.A, Clamp(left.R - right.R), Clamp(left.G - right.G), Clamp(left.B - right.B));
			baseImage.ApplyFunctionToAllPixels(subtract, filter);
		}

		/// <summary>
		/// Changes the lightness of the specified image.
		/// </summary>
		/// <param name="image">The image whose lightness to change.</param>
		/// <param name="offset">The color offset.</param>
		public static void ChangeLightness(this Bitmap image, int offset) {
			if (image == null || offset == 0f)
				return;
			using (PixelWorker source = PixelWorker.FromImage(image, false, true, ImageParameterAction.RemoveReference))
				ChangeLightness(source, offset);
		}

		/// <summary>
		/// Changes the lightness of the specified image.
		/// </summary>
		/// <param name="source">The image whose lightness to change. WriteChanges() is not called, that's on you.</param>
		/// <param name="offset">The color offset.</param>
		public static void ChangeLightness(this PixelWorker source, int offset) {
			if (source == null || offset == 0f)
				return;
			else if (source.ComponentCount == 4) {
				if (source.Buffer == null) {
					unsafe
					{
						ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
							if (i % 4 != 3) {
								byte* index = source.Scan0 + i;
								*index = Clamp(*index + offset);
							}
						}, ParallelCutoff);
					}
				} else {
					ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
						if (i % 4 != 3)
							source.Buffer[i] = Clamp(source.Buffer[i] + offset);
					}, ParallelCutoff);
				}
			} else {
				if (source.Buffer == null) {
					unsafe
					{
						ParallelLoop.For(source.Scan0, source.PixelComponentCount, 1L, i => {
							byte* index = (byte*) i;
							*index = Clamp(*index + offset);
						}, ParallelCutoff);
					}
				} else {
					ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
						source.Buffer[i] = Clamp(source.Buffer[i] + offset);
					}, ParallelCutoff);
				}
			}
		}

		/// <summary>
		/// Applies a bitwise 'and' to the colors of the image using the specified mask.
		/// </summary>
		/// <param name="image">The image to mask.</param>
		/// <param name="mask">The mask to bitwise 'and' with.</param>
		/// <param name="ignoreAlpha">If true, the alpha channel will be left intact.</param>
		public static void BitwiseAnd(this Bitmap image, byte mask, bool ignoreAlpha = true) {
			if (image == null)
				return;
			using (PixelWorker source = PixelWorker.FromImage(image, false, true, ImageParameterAction.RemoveReference))
				BitwiseAnd(source, mask, ignoreAlpha);
		}

		/// <summary>
		/// Applies a bitwise 'and' to the colors of the image using the specified mask.
		/// </summary>
		/// <param name="source">The image to mask. WriteChanges() is not called, that's on you.</param>
		/// <param name="mask">The mask to bitwise 'and' with.</param>
		/// <param name="ignoreAlpha">If true, the alpha channel will be left intact.</param>
		public static void BitwiseAnd(this PixelWorker source, byte mask, bool ignoreAlpha = true) {
			if (source == null)
				return;
			else if (ignoreAlpha && source.ComponentCount == 4) {
				if (source.Buffer == null) {
					unsafe
					{
						ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
							if (i % 4 != 3) {
								byte* index = source.Scan0 + i;
								*index = (byte) (*index & mask);
							}
						}, ParallelCutoff);
					}
				} else {
					ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
						if (i % 4 != 3)
							source.Buffer[i] = (byte) (source.Buffer[i] & mask);
					}, ParallelCutoff);
				}
			} else {
				if (source.Buffer == null) {
					unsafe
					{
						ParallelLoop.For(source.Scan0, source.PixelComponentCount, 1L, i => {
							byte* index = (byte*) i;
							*index = (byte) (*index & mask);
						}, ParallelCutoff);
					}
				} else {
					ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
						source.Buffer[i] = (byte) (source.Buffer[i] & mask);
					}, ParallelCutoff);
				}
			}
		}

		/// <summary>
		/// Applies a bitwise 'or' to the colors of the image using the specified mask.
		/// </summary>
		/// <param name="image">The image to mask.</param>
		/// <param name="mask">The mask to bitwise 'or' with.</param>
		/// <param name="ignoreAlpha">If true, the alpha channel will be left intact.</param>
		public static void BitwiseOr(this Bitmap image, byte mask, bool ignoreAlpha = true) {
			if (image == null)
				return;
			using (PixelWorker source = PixelWorker.FromImage(image, false, true, ImageParameterAction.RemoveReference))
				BitwiseOr(source, mask, ignoreAlpha);
		}

		/// <summary>
		/// Applies a bitwise 'or' to the colors of the image using the specified mask.
		/// </summary>
		/// <param name="source">The image to mask. WriteChanges() is not called, that's on you.</param>
		/// <param name="mask">The mask to bitwise 'or' with.</param>
		/// <param name="ignoreAlpha">If true, the alpha channel will be left intact.</param>
		public static void BitwiseOr(this PixelWorker source, byte mask, bool ignoreAlpha = true) {
			if (source == null)
				return;
			else if (ignoreAlpha && source.ComponentCount == 4) {
				if (source.Buffer == null) {
					unsafe
					{
						ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
							if (i % 4 != 3) {
								byte* index = source.Scan0 + i;
								*index = (byte) (*index | mask);
							}
						}, ParallelCutoff);
					}
				} else {
					ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
						if (i % 4 != 3)
							source.Buffer[i] = (byte) (source.Buffer[i] | mask);
					}, ParallelCutoff);
				}
			} else {
				if (source.Buffer == null) {
					unsafe
					{
						ParallelLoop.For(source.Scan0, source.PixelComponentCount, 1L, i => {
							byte* index = (byte*) i;
							*index = (byte) (*index | mask);
						}, ParallelCutoff);
					}
				} else {
					ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
						source.Buffer[i] = (byte) (source.Buffer[i] | mask);
					}, ParallelCutoff);
				}
			}
		}

		/// <summary>
		/// Applies a bitwise 'xor' to the colors of the image using the specified mask.
		/// </summary>
		/// <param name="image">The image to mask.</param>
		/// <param name="mask">The mask to bitwise 'xor' with.</param>
		/// <param name="ignoreAlpha">If true, the alpha channel will be left intact.</param>
		public static void BitwiseXor(this Bitmap image, byte mask, bool ignoreAlpha = true) {
			if (image == null)
				return;
			using (PixelWorker source = PixelWorker.FromImage(image, false, true, ImageParameterAction.RemoveReference))
				BitwiseXor(source, mask, ignoreAlpha);
		}

		/// <summary>
		/// Applies a bitwise 'xor' to the colors of the image using the specified mask.
		/// </summary>
		/// <param name="source">The image to mask. WriteChanges() is not called, that's on you.</param>
		/// <param name="mask">The mask to bitwise 'xor' with.</param>
		/// <param name="ignoreAlpha">If true, the alpha channel will be left intact.</param>
		public static void BitwiseXor(this PixelWorker source, byte mask, bool ignoreAlpha = true) {
			if (source == null)
				return;
			else if (ignoreAlpha && source.ComponentCount == 4) {
				if (source.Buffer == null) {
					unsafe
					{
						ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
							if (i % 4 != 3) {
								byte* index = source.Scan0 + i;
								*index = (byte) (*index ^ mask);
							}
						}, ParallelCutoff);
					}
				} else {
					ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
						if (i % 4 != 3)
							source.Buffer[i] = (byte) (source.Buffer[i] ^ mask);
					}, ParallelCutoff);
				}
			} else {
				if (source.Buffer == null) {
					unsafe
					{
						ParallelLoop.For(source.Scan0, source.PixelComponentCount, 1L, i => {
							byte* index = (byte*) i;
							*index = (byte) (*index ^ mask);
						}, ParallelCutoff);
					}
				} else {
					ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
						source.Buffer[i] = (byte) (source.Buffer[i] ^ mask);
					}, ParallelCutoff);
				}
			}
		}

		/// <summary>
		/// Stretches the image to the specified size using the specified interpolation method.
		/// </summary>
		/// <param name="image">The source image to stretch.</param>
		/// <param name="newSize">The desired size of the stretched image.</param>
		/// <param name="interpolation">The interpolation method to use.</param>
		public static Bitmap Stretch(Image image, Size newSize, InterpolationMode interpolation = InterpolationMode.HighQualityBicubic) {
			Bitmap newBitmap = new Bitmap(newSize.Width, newSize.Height, image.PixelFormat);
			using (Graphics g = Graphics.FromImage(newBitmap)) {
				g.PixelOffsetMode = PixelOffsetMode.HighQuality;
				g.CompositingMode = CompositingMode.SourceCopy;
				g.InterpolationMode = interpolation;
				DrawStretched(g, image, newSize);
			}
			return newBitmap;
		}

		/// <summary>
		/// Draws the specified image stretced to the specified size on the specified graphics canvas at (0, 0).
		/// </summary>
		/// <param name="g">The graphics canvas to draw the stretched image on.</param>
		/// <param name="image">The source image to stretch.</param>
		/// <param name="size">The desired size of the stretched image.</param>
		public static void DrawStretched(this Graphics g, Image image, Size size) {
			g.DrawImage(image, ToDestPoints(Point.Empty, size), new Rectangle(Point.Empty, image.Size), GraphicsUnit.Pixel, TilingAttributes);
		}

		/// <summary>
		/// Resizes the image to the specified size using the specified interpolation method and layout.
		/// </summary>
		/// <param name="image">The source image to resize.</param>
		/// <param name="newSize">The desired size of the stretched image.</param>
		/// <param name="layout">The desired output image layout.</param>
		/// <param name="interpolation">The interpolation method to use.</param>
		public static Bitmap ResizeWithLayout(this Image image, Size newSize, ImageLayout layout = ImageLayout.Stretch, InterpolationMode interpolation = InterpolationMode.HighQualityBicubic) {
			Bitmap resultantImage = new Bitmap(newSize.Width, newSize.Height, image.PixelFormat);
			using (Graphics g = Graphics.FromImage(resultantImage)) {
				g.PixelOffsetMode = PixelOffsetMode.HighQuality;
				g.CompositingMode = CompositingMode.SourceCopy;
				g.InterpolationMode = interpolation;
				DrawImageWithLayout(g, image, new Rectangle(Point.Empty, newSize), layout);
			}
			return resultantImage;
		}

		/// <summary>
		/// Draws the specified image resized to the specified size on the given graphics canvas using the specified layout.
		/// </summary>
		/// <param name="g">The graphics canvas to draw the stretched image on.</param>
		/// <param name="image">The source image to resize.</param>
		/// <param name="area">The desired area to draw the stretched image.</param>
		/// <param name="layout">The desured layout of the image.</param>
		public static void DrawImageWithLayout(this Graphics g, Image image, RectangleF area, ImageLayout layout) {
			Rectangle temp = Rectangle.Ceiling(area);
			if ((RectangleF) temp == area && image.Size == temp.Size)
				g.DrawImageUnscaled(image, temp.Location);
			else {
				switch (layout) {
					case ImageLayout.None:
						RectangleF workaround = area;
						if (g.InterpolationMode == InterpolationMode.NearestNeighbor || g.InterpolationMode == InterpolationMode.Invalid) {
							workaround.Width += (workaround.Width / image.Width) * 0.5f;
							workaround.Height += (workaround.Height / image.Height) * 0.5f;
						}
						g.DrawImageUnscaledAndClipped(image, Rectangle.Truncate(workaround));
						break;
					case ImageLayout.Stretch:
						g.DrawImage(image, ToDestPoints(area), new Rectangle(Point.Empty, image.Size), GraphicsUnit.Pixel, TilingAttributes);
						break;
					case ImageLayout.Zoom:
						float nPercentW = area.Width / image.Width;
						float nPercentH = area.Height / image.Height;
						if (nPercentH < nPercentW)
							g.DrawImage(image, ToDestPoints(area.X + (area.Width - (image.Width * nPercentH)) * 0.5f, area.Y, image.Width * nPercentH, image.Height * nPercentH), new Rectangle(Point.Empty, image.Size), GraphicsUnit.Pixel, TilingAttributes);
						else
							g.DrawImage(image, ToDestPoints(area.X, area.Y + ((area.Height - (image.Height * nPercentW)) * 0.5f), image.Width * nPercentW, image.Height * nPercentW), new Rectangle(Point.Empty, image.Size), GraphicsUnit.Pixel, TilingAttributes);
						break;
					case ImageLayout.Tile:
						using (TextureBrush brush = new TextureBrush(image, WrapMode.Tile)) {
							brush.TranslateTransform(area.X, area.Y);
							g.FillRectangle(brush, area);
						}
						break;
					case ImageLayout.Center:
						RectangleF centered = new RectangleF(Center(area.Size, new PointF(image.Width * 0.5f, image.Height * 0.5f)), area.Size);
						if (centered.X < 0f) {
							centered.X = 0f;
							centered.Width = image.Width;
						}
						if (centered.Y < 0f) {
							centered.Y = 0f;
							centered.Height = image.Height;
						}
						g.DrawImage(image, ToDestPoints(area), centered, GraphicsUnit.Pixel, TilingAttributes);
						break;
				}
			}
		}

		/// <summary>
		/// Returns the center of the specified rectangle.
		/// </summary>
		/// <param name="rect">The rectangle to get the center of.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Point CenterOf(this Rectangle rect) {
			return new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
		}

		/// <summary>
		/// Aligns the specified rectangle inside the given bounds and returns the new location of the rectangle after alignment.
		/// </summary>
		/// <param name="bounds">The boundaries to align the rectangle into.</param>
		/// <param name="size">The size of the rectangle to center.</param>
		/// <param name="alignment">The alignment of the rectangle.</param>
		public static Point AlignRectangle(this Rectangle bounds, Size size, ContentAlignment alignment) {
			switch (alignment) {
				case ContentAlignment.MiddleCenter:
					return new Point(bounds.X + (bounds.Width - size.Width) / 2, bounds.Y + (bounds.Height - size.Height) / 2);
				case ContentAlignment.MiddleLeft:
					return new Point(bounds.X, bounds.Y + (bounds.Height - size.Height) / 2);
				case ContentAlignment.MiddleRight:
					return new Point(bounds.Right - size.Width, bounds.Y + (bounds.Height - size.Height) / 2);
				case ContentAlignment.TopCenter:
					return new Point(bounds.X + (bounds.Width - size.Width) / 2, bounds.Y);
				case ContentAlignment.TopLeft:
					return bounds.Location;
				case ContentAlignment.TopRight:
					return new Point(bounds.Right - size.Width, bounds.Y);
				case ContentAlignment.BottomCenter:
					return new Point(bounds.X + (bounds.Width - size.Width) / 2, bounds.Bottom - size.Height);
				case ContentAlignment.BottomLeft:
					return new Point(bounds.X, bounds.Bottom - size.Height);
				default:
					return new Point(bounds.Right - size.Width, bounds.Bottom - size.Height);
			}
		}

		/// <summary>
		/// Aligns the specified rectangle inside the given bounds and returns the new location of the rectangle after alignment.
		/// </summary>
		/// <param name="bounds">The boundaries to align the rectangle into.</param>
		/// <param name="size">The size of the rectangle to center.</param>
		/// <param name="alignment">The alignment of the rectangle.</param>
		public static PointF AlignRectangle(this RectangleF bounds, SizeF size, ContentAlignment alignment) {
			switch (alignment) {
				case ContentAlignment.MiddleCenter:
					return new PointF(bounds.X + (bounds.Width - size.Width) * 0.5f, bounds.Y + (bounds.Height - size.Height) * 0.5f);
				case ContentAlignment.MiddleLeft:
					return new PointF(bounds.X, bounds.Y + (bounds.Height - size.Height) * 0.5f);
				case ContentAlignment.MiddleRight:
					return new PointF(bounds.Right - size.Width, bounds.Y + (bounds.Height - size.Height) * 0.5f);
				case ContentAlignment.TopCenter:
					return new PointF(bounds.X + (bounds.Width - size.Width) * 0.5f, bounds.Y);
				case ContentAlignment.TopLeft:
					return bounds.Location;
				case ContentAlignment.TopRight:
					return new PointF(bounds.Right - size.Width, bounds.Y);
				case ContentAlignment.BottomCenter:
					return new PointF(bounds.X + (bounds.Width - size.Width) * 0.5f, bounds.Bottom - size.Height);
				case ContentAlignment.BottomLeft:
					return new PointF(bounds.X, bounds.Bottom - size.Height);
				default:
					return new PointF(bounds.Right - size.Width, bounds.Bottom - size.Height);
			}
		}

		/// <summary>
		/// Returns the center of the specified rectangle.
		/// </summary>
		/// <param name="rect">The rectangle to get the center of.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static PointF CenterOf(this RectangleF rect) {
			return new PointF(rect.X + rect.Width * 0.5f, rect.Y + rect.Height * 0.5f);
		}

		/// <summary>
		/// Centers the specified rectangle around the specified point by returning its new location.
		/// </summary>
		/// <param name="size">The size of the rectangle to center.</param>
		/// <param name="center">The point to center the rectangle around.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Point Center(Size size, Point center) {
			return new Point(center.X - size.Width / 2, center.Y - size.Height / 2);
		}

		/// <summary>
		/// Centers the specified rectangle around the specified point by returning its new location.
		/// </summary>
		/// <param name="size">The size of the rectangle to center.</param>
		/// <param name="center">The point to center the rectangle around.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static PointF Center(SizeF size, PointF center) {
			return new PointF(center.X - size.Width * 0.5f, center.Y - size.Height * 0.5f);
		}

		/// <summary>
		/// Sets the backround color the image is overlaid upon (only has an effect if the specified image contains transparency).
		/// </summary>
		/// <param name="image">The image whose backcolor to change.</param>
		/// <param name="backColor">The color to use for the specified image to be overlaid upon.</param>
		/// <param name="opacity">The opacity to multiply the backcolor with.</param>
		public static void SetBackColor(this Bitmap image, Color backColor, float opacity) {
			SetBackColor(image, (BgraColor) backColor, opacity);
		}

		/// <summary>
		/// Sets the backround color the image is overlaid upon (only has an effect if the specified image contains transparency).
		/// </summary>
		/// <param name="image">The image whose backcolor to change.</param>
		/// <param name="backColor">The color to use for the specified image to be overlaid upon.</param>
		/// <param name="opacity">The opacity to multiply the backcolor with.</param>
		public static void SetBackColor(this Bitmap image, BgraColor backColor, float opacity) {
			if (opacity == 0f || backColor.A == 0)
				return;
			using (PixelWorker source = PixelWorker.FromImage(image, false, true, ImageParameterAction.RemoveReference))
				SetBackColor(source, backColor, opacity);
		}

		/// <summary>
		/// Sets the backround color the image is overlaid upon (only has an effect if the specified image contains transparency).
		/// </summary>
		/// <param name="source">The image whose backcolor to change. WriteChanges() is not called, that's on you.</param>
		/// <param name="backColor">The color to use for the specified image to be overlaid upon.</param>
		/// <param name="opacity">The opacity to multiply the backcolor with.</param>
		public static void SetBackColor(this PixelWorker source, Color backColor, float opacity) {
			SetBackColor(source, (BgraColor) backColor, opacity);
		}

		/// <summary>
		/// Sets the backround color the image is overlaid upon (only has an effect if the specified image contains transparency).
		/// </summary>
		/// <param name="source">The image whose backcolor to change. WriteChanges() is not called, that's on you.</param>
		/// <param name="backColor">The color to use for the specified image to be overlaid upon.</param>
		/// <param name="opacity">The opacity to multiply the backcolor with.</param>
		public static void SetBackColor(this PixelWorker source, BgraColor backColor, float opacity) {
			if (!(opacity == 0f || backColor.A == 0))
				source.ApplyFunctionToAllPixels(Overlay, backColor, opacity);
		}

		/// <summary>
		/// Changes the image opacity by multiplying all colors with the specified component.
		/// </summary>
		/// <param name="image">The image whose global opacity to set.</param>
		/// <param name="opacity">The multiplier to use.</param>
		public static void SetImageOpacity(this Bitmap image, float opacity) {
			if (image.PixelFormat == PixelFormat.Format32bppArgb || image.PixelFormat == PixelFormat.Format32bppPArgb) {
				using (PixelWorker source = PixelWorker.FromImage(image, false, true, ImageParameterAction.RemoveReference))
					SetImageOpacity(source, opacity);
			}
		}

		/// <summary>
		/// Changes the image opacity by multiplying all colors with the specified component.
		/// </summary>
		/// <param name="source">The image whose global opacity to set. WriteChanges() is not called, that's on you.</param>
		/// <param name="opacity">The multiplier to use.</param>
		public static void SetImageOpacity(this PixelWorker source, float opacity) {
			if (source.ComponentCount == 4) {
				if (source.Buffer == null) {
					unsafe
					{
						ParallelLoop.For(source.Scan0 + 3, source.PixelCount, 4L, delegate (IntPtr i) {
							byte* index = (byte*) i;
							*index = Clamp(*index * opacity);
						}, ParallelCutoff);
					}
				} else {
					ParallelLoop.For(3, source.PixelComponentCount, 4, delegate (int i) {
						source.Buffer[i] = Clamp(source.Buffer[i] * opacity);
					}, ParallelCutoff);
				}
			}
		}

		/// <summary>
		/// Mirrors half of the image onto the other half.
		/// </summary>
		/// <param name="image">The image to mirror.</param>
		/// <param name="mode">The mirror mode to use.</param>
		public static void Mirror(this Bitmap image, MirrorMode mode) {
			using (PixelWorker source = PixelWorker.FromImage(image, false, true, ImageParameterAction.RemoveReference))
				Mirror(source, mode);
		}

		/// <summary>
		/// Mirrors half of the image onto the other half.
		/// </summary>
		/// <param name="source">The image to mirror. WriteChanges() is not called, that's on you.</param>
		/// <param name="mode">The mirror mode to use.</param>
		public static void Mirror(this PixelWorker source, MirrorMode mode) {
			int halfWidth = source.Width / 2, halfHeight = source.Height / 2;
			switch (mode) {
				case MirrorMode.HorizontalMirrorLeft:
					ParallelLoop.For(0, source.Height, y => {
						int c, loc1, loc2;
						int rowPos = y * source.Width;
						int cc = source.ComponentCount;
						for (int x = 0; x < halfWidth; x++) {
							loc1 = (rowPos + source.Width - (x + 1)) * cc;
							loc2 = (rowPos + x) * cc;
							for (c = 0; c < cc; c++)
								source[loc1 + c] = source[loc2 + c];
						}
					}, ParallelCutoff);
					break;
				case MirrorMode.HorizontalMirrorRight:
					ParallelLoop.For(0, source.Height, y => {
						int c, loc1, loc2;
						int rowPos = y * source.Width;
						int cc = source.ComponentCount;
						for (int x = 0; x < halfWidth; x++) {
							loc1 = (rowPos + x) * cc;
							loc2 = (rowPos + source.Width - (x + 1)) * cc;
							for (c = 0; c < cc; c++)
								source[loc1 + c] = source[loc2 + c];
						}
					}, ParallelCutoff);
					break;
				case MirrorMode.VerticalMirrorTop:
					ParallelLoop.For(0, source.Width, x => {
						int c, loc1, loc2;
						int cc = source.ComponentCount;
						for (int y = 0; y < halfHeight; y++) {
							loc1 = ((source.Height - (y + 1)) * source.Width + x) * cc;
							loc2 = (y * source.Width + x) * cc;
							for (c = 0; c < cc; c++)
								source[loc1 + c] = source[loc2 + c];
						}
					}, ParallelCutoff);
					break;
				default:
					ParallelLoop.For(0, source.Width, x => {
						int c, loc1, loc2;
						int cc = source.ComponentCount;
						for (int y = 0; y < halfHeight; y++) {
							loc1 = (y * source.Width + x) * cc;
							loc2 = ((source.Height - (y + 1)) * source.Width + x) * cc;
							for (c = 0; c < cc; c++)
								source[loc1 + c] = source[loc2 + c];
						}
					}, ParallelCutoff);
					break;
			}
		}

		/// <summary>
		/// Draws the specified image on the given canvas using the specified opacity.
		/// </summary>
		/// <param name="g">The graphics canvas to draw the faded image on.</param>
		/// <param name="image">The image to draw.</param>
		/// <param name="dest">The destination rectangle.</param>
		/// <param name="opacity">The opacity to multiply the image with.</param>
		public static void DrawFaded(this Graphics g, Image image, RectangleF dest, float opacity) {
			if (opacity == 0f)
				return;
			ImageAttributes attr = GetOpacityAttributes(opacity);
			attr.SetWrapMode(WrapMode.TileFlipXY);
			g.DrawImage(image, ToDestPoints(dest), new Rectangle(Point.Empty, image.Size), GraphicsUnit.Pixel, attr);
		}

		/// <summary>
		/// Performs a fourier shift on the image. Two fourier shifts cancel each other out.
		/// </summary>
		/// <param name="image">The image to shift.</param>
		public static void FFTShift(this Bitmap image) {
			using (PixelWorker source = PixelWorker.FromImage(image, false, true, ImageParameterAction.RemoveReference))
				FFTShift(source);
		}

		/// <summary>
		/// Performs a fourier shift on the image. Two fourier shifts cancel each other out.
		/// </summary>
		/// <param name="source">The image to shift. WriteChanges() is not called, that's on you.</param>
		public static void FFTShift(this PixelWorker source) {
			int halfWidth = source.Width / 2, halfHeight = source.Height / 2;
			ParallelLoop.For(0, halfHeight, y => {
				int width = source.Width;
				int index1, index2, index3, index4, c;
				byte temp;
				int cc = source.ComponentCount;
				for (int x = 0; x < halfWidth; x++) {
					index1 = (x + y * width) * cc;
					index2 = (x + halfWidth + (y + halfHeight) * width) * cc;
					index3 = (x + (y + halfHeight) * width) * cc;
					index4 = (x + halfWidth + y * width) * cc;
					for (c = 0; c < cc; c++) {
						temp = source[index1];
						source[index1] = source[index2];
						source[index2] = temp;
						temp = source[index3];
						source[index3] = source[index4];
						source[index4] = temp;
						index1++;
						index2++;
						index3++;
						index4++;
					}
				}
			}, ParallelCutoff);
		}

		/// <summary>
		/// Copies the specified image.
		/// </summary>
		/// <param name="image">The image to copy.</param>
		public static unsafe Bitmap FastCopy(this Bitmap image) {
			int width = image.Width;
			int height = image.Height;
			PixelFormat format = image.PixelFormat;
			int widthComponentCount = width * Image.GetPixelFormatSize(format) / 8;
			Rectangle rect = new Rectangle(0, 0, width, height);
			BitmapData orig = image.LockBits(rect, ImageLockMode.ReadOnly, format);
			Bitmap result = new Bitmap(width, height, format);
			BitmapData rData = result.LockBits(rect, ImageLockMode.WriteOnly, format);
			byte* iSrc = (byte*) orig.Scan0;
			byte* iDest = (byte*) rData.Scan0;
			for (int y = 0; y < height; y++) {
				Extensions.MemoryCopy(iSrc, iDest, (uint) widthComponentCount);
				iSrc += orig.Stride;
				iDest += rData.Stride;
			}
			image.UnlockBits(orig);
			result.UnlockBits(rData);
			if (format == PixelFormat.Format8bppIndexed) {
				ColorPalette palette = result.Palette;
				Color[] entries = palette.Entries;
				for (int i = 0; i < 256; i++)
					entries[i] = Color.FromArgb(i, i, i);
				result.Palette = palette;
			}
			return result;
		}

		/// <summary>
		/// Converts the bitmap pixel format or copies the bitmap if the format is the same.
		/// </summary>
		/// <param name="image">The image to use.</param>
		/// <param name="format">The destination pixel format of the image.</param>
		public static Bitmap ConvertPixelFormat(this Bitmap image, PixelFormat format) {
			Bitmap result = new Bitmap(image.Width, image.Height, format);
			using (Graphics g = Graphics.FromImage(result)) {
				g.PixelOffsetMode = PixelOffsetMode.None;
				g.InterpolationMode = InterpolationMode.NearestNeighbor;
				g.CompositingMode = CompositingMode.SourceCopy;
				g.DrawImage(image, new Rectangle(Point.Empty, result.Size));
			}
			return result;
		}

		/// <summary>
		/// Converts the bitmap pixel format and stretches it to the specified width and height.
		/// </summary>
		/// <param name="image">The image to use.</param>
		/// <param name="format">The destination pixel format of the image.</param>
		/// <param name="newWidth">The width of the resultant image.</param>
		/// <param name="newHeight">The height of the resultant image.</param>
		public static Bitmap ConvertPixelFormat(this Bitmap image, int newWidth, int newHeight, PixelFormat format) {
			Bitmap result = new Bitmap(newWidth, newHeight, format);
			using (Graphics g = Graphics.FromImage(result)) {
				g.PixelOffsetMode = PixelOffsetMode.HighQuality;
				g.InterpolationMode = InterpolationMode.HighQualityBicubic;
				g.CompositingMode = CompositingMode.SourceCopy;
				g.DrawImage(image, new Rectangle(Point.Empty, result.Size));
			}
			return result;
		}

		/// <summary>
		/// Gets the ImageAttributes from the specified opacity.
		/// </summary>
		/// <param name="opacity">The opacity attribute.</param>
		/// <param name="wrap">The image wrap mode.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static ImageAttributes GetOpacityAttributes(float opacity, WrapMode wrap = WrapMode.Clamp) {
			ImageAttributes attributes = new ImageAttributes();
			attributes.SetColorMatrix(new ColorMatrix(new float[][] { b1, b2, b3, new float[] { 0f, 0f, 0f, opacity, 0f }, b5 }),
				ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
			attributes.SetWrapMode(wrap);
			return attributes;
		}

		/// <summary>
		/// Gets the corresponding DestPoints from the given rectangle.
		/// </summary>
		/// <param name="rectangle">The rectangle to extract the destination points from.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Point[] ToDestPoints(this Rectangle rectangle) {
			return new Point[3] { new Point(rectangle.X, rectangle.Y), new Point(rectangle.Right, rectangle.Y), new Point(rectangle.X, rectangle.Bottom) };
		}

		/// <summary>
		/// Gets the corresponding DestPoints from the given rectangle.
		/// </summary>
		/// <param name="rectangle">The rectangle to extract the destination points from.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static PointF[] ToDestPoints(this RectangleF rectangle) {
			return new PointF[3] { new PointF(rectangle.X, rectangle.Y), new PointF(rectangle.Right, rectangle.Y), new PointF(rectangle.X, rectangle.Bottom) };
		}

		/// <summary>
		/// Gets the corresponding DestPoints from the given rectangle.
		/// </summary>
		/// <param name="x">The X-coordinate of the upper-left corner of the rectangle.</param>
		/// <param name="y">The Y-coordinate of the upper-left corner of the rectangle.</param>
		/// <param name="width">The width of the rectangle.</param>
		/// <param name="height">The height of the rectangle.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Point[] ToDestPoints(int x, int y, int width, int height) {
			return new Point[3] { new Point(x, y), new Point(x + width, y), new Point(x, y + height) };
		}

		/// <summary>
		/// Gets the corresponding DestPoints from the given rectangle.
		/// </summary>
		/// <param name="x">The X-coordinate of the upper-left corner of the rectangle.</param>
		/// <param name="y">The Y-coordinate of the upper-left corner of the rectangle.</param>
		/// <param name="width">The width of the rectangle.</param>
		/// <param name="height">The height of the rectangle.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static PointF[] ToDestPoints(float x, float y, float width, float height) {
			return new PointF[3] { new PointF(x, y), new PointF(x + width, y), new PointF(x, y + height) };
		}

		/// <summary>
		/// Gets the corresponding DestPoints from the given rectangle.
		/// </summary>
		/// <param name="location">The coordinates of the upper-left corner of the rectangle.</param>
		/// <param name="size">The size of the rectangle.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Point[] ToDestPoints(Point location, Size size) {
			return new Point[3] { location, new Point(location.X + size.Width, location.Y), new Point(location.X, location.Y + size.Height) };
		}

		/// <summary>
		/// Gets the corresponding DestPoints from the given rectangle.
		/// </summary>
		/// <param name="location">The coordinates of the upper-left corner of the rectangle.</param>
		/// <param name="size">The size of the rectangle.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static PointF[] ToDestPoints(PointF location, SizeF size) {
			return new PointF[3] { location, new PointF(location.X + size.Width, location.Y), new PointF(location.X, location.Y + size.Height) };
		}

		/// <summary>
		/// Composes the given pixels by calculating the addition of their colors and returning the result with the specified alpha component.
		/// </summary>
		/// <param name="basePixel">The base pixel color.</param>
		/// <param name="overlayPixel">The pixel whose color is to be overlaid on the base pixel.</param>
		/// <param name="alpha">The alpha component of the resulting color.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Color Compose(this Color basePixel, Color overlayPixel, byte alpha) {
			float colorOpacity = overlayPixel.A * 0.00392156862f;
			return Color.FromArgb(alpha, (byte) (basePixel.R + (overlayPixel.R - basePixel.R) * colorOpacity), (byte) (basePixel.G + (overlayPixel.G - basePixel.G) * colorOpacity), (byte) (basePixel.B + (overlayPixel.B - basePixel.B) * colorOpacity));
		}

		/// <summary>
		/// Composes the given pixels by calculating the addition of their colors and returning the result with the specified alpha component.
		/// </summary>
		/// <param name="basePixel">The base pixel color.</param>
		/// <param name="overlayPixel">The pixel whose color is to be overlaid on the base pixel.</param>
		/// <param name="alpha">The alpha component of the resulting color.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static BgraColor Compose(this BgraColor basePixel, BgraColor overlayPixel, byte alpha) {
			float colorOpacity = overlayPixel.A * 0.00392156862f;
			return new BgraColor(alpha, (byte) (basePixel.R + (overlayPixel.R - basePixel.R) * colorOpacity), (byte) (basePixel.G + (overlayPixel.G - basePixel.G) * colorOpacity), (byte) (basePixel.B + (overlayPixel.B - basePixel.B) * colorOpacity));
		}

		/// <summary>
		/// Overlays the specified pixel on top of the base pixel and returns the resultant color.
		/// </summary>
		/// <param name="basePixel">The base pixel color.</param>
		/// <param name="overlayPixel">The pixel whose color is to be overlaid on the base pixel.</param>
		/// <param name="opacity">The opacity multiplier of the overlay.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Color Overlay(this Color basePixel, Color overlayPixel, float opacity) {
			if (opacity == 0f)
				return basePixel;
			float colorOpacity = overlayPixel.A * opacity * 0.00392156862f;
			return Color.FromArgb(Clamp(basePixel.A + ((~basePixel.A) * colorOpacity)), Clamp(basePixel.R + (overlayPixel.R - basePixel.R) * colorOpacity), Clamp(basePixel.G + (overlayPixel.G - basePixel.G) * colorOpacity), Clamp(basePixel.B + (overlayPixel.B - basePixel.B) * colorOpacity));
		}

		/// <summary>
		/// Overlays the specified pixel on top of the base pixel and returns the resultant color.
		/// </summary>
		/// <param name="basePixel">The base pixel color.</param>
		/// <param name="overlayPixel">The pixel whose color is to be overlaid on the base pixel.</param>
		/// <param name="opacity">The opacity multiplier of the overlay.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static BgraColor Overlay(this BgraColor basePixel, BgraColor overlayPixel, float opacity) {
			if (opacity == 0f)
				return basePixel;
			float colorOpacity = overlayPixel.A * opacity * 0.00392156862f;
			return new BgraColor(Clamp(basePixel.A + ((~basePixel.A) * colorOpacity)), Clamp(basePixel.R + (overlayPixel.R - basePixel.R) * colorOpacity), Clamp(basePixel.G + (overlayPixel.G - basePixel.G) * colorOpacity), Clamp(basePixel.B + (overlayPixel.B - basePixel.B) * colorOpacity));
		}

		/// <summary>
		/// Overlays the specified pixel on top of the base pixel and returns the resultant color.
		/// </summary>
		private static BgraColor Overlay(int i, BgraColor basePixel, BgraColor overlayPixel, float opacity) {
			float colorOpacity = overlayPixel.A * opacity * 0.00392156862f;
			return new BgraColor(Clamp(basePixel.A + ((~basePixel.A) * colorOpacity)), Clamp(basePixel.R + (overlayPixel.R - basePixel.R) * colorOpacity), Clamp(basePixel.G + (overlayPixel.G - basePixel.G) * colorOpacity), Clamp(basePixel.B + (overlayPixel.B - basePixel.B) * colorOpacity));
		}

		/// <summary>
		/// Overlays the specified pixel on top of the base pixel and returns the resultant color.
		/// </summary>
		/// <param name="basePixel">The base pixel color.</param>
		/// <param name="overlayPixel">The pixel whose color is to be overlaid on the base pixel.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Color Overlay(this Color basePixel, Color overlayPixel) {
			float colorOpacity = overlayPixel.A * 0.00392156862f;
			return Color.FromArgb(Clamp(basePixel.A + ((~basePixel.A) * colorOpacity)), (byte) (basePixel.R + (overlayPixel.R - basePixel.R) * colorOpacity), (byte) (basePixel.G + (overlayPixel.G - basePixel.G) * colorOpacity), (byte) (basePixel.B + (overlayPixel.B - basePixel.B) * colorOpacity));
		}

		/// <summary>
		/// Overlays the specified pixel on top of the base pixel and returns the resultant color.
		/// </summary>
		/// <param name="basePixel">The base pixel color.</param>
		/// <param name="overlayPixel">The pixel whose color is to be overlaid on the base pixel.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static BgraColor Overlay(this BgraColor basePixel, BgraColor overlayPixel) {
			float colorOpacity = overlayPixel.A * 0.00392156862f;
			return new BgraColor(Clamp(basePixel.A + ((~basePixel.A) * colorOpacity)), (byte) (basePixel.R + (overlayPixel.R - basePixel.R) * colorOpacity), (byte) (basePixel.G + (overlayPixel.G - basePixel.G) * colorOpacity), (byte) (basePixel.B + (overlayPixel.B - basePixel.B) * colorOpacity));
		}

		/// <summary>
		/// Computes the transition color at the state specified.
		/// </summary>
		/// <param name="basePixel">The original pixel color.</param>
		/// <param name="overlayPixel">The target pixel color.</param>
		/// <param name="opacity">The transition state (0 - original, 1 - target color).</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Color Transition(this Color basePixel, Color overlayPixel, float opacity) {
			return Color.FromArgb(Clamp(basePixel.A + ((overlayPixel.A - basePixel.A) * opacity)), Clamp(basePixel.R + (overlayPixel.R - basePixel.R) * opacity), Clamp(basePixel.G + (overlayPixel.G - basePixel.G) * opacity), Clamp(basePixel.B + (overlayPixel.B - basePixel.B) * opacity));
		}

		/// <summary>
		/// Computes the transition color at the state specified.
		/// </summary>
		/// <param name="basePixel">The original pixel color.</param>
		/// <param name="overlayPixel">The target pixel color.</param>
		/// <param name="opacity">The transition state (0 - original, 1 - target color).</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static BgraColor Transition(this BgraColor basePixel, BgraColor overlayPixel, float opacity) {
			return new BgraColor(Clamp(basePixel.A + ((overlayPixel.A - basePixel.A) * opacity)), Clamp(basePixel.R + (overlayPixel.R - basePixel.R) * opacity), Clamp(basePixel.G + (overlayPixel.G - basePixel.G) * opacity), Clamp(basePixel.B + (overlayPixel.B - basePixel.B) * opacity));
		}

		/// <summary>
		/// Computes the transition color at the state specified.
		/// </summary>
		private static BgraColor Transition(int i, BgraColor basePixel, BgraColor overlayPixel, float opacity) {
			return new BgraColor(Clamp(basePixel.A + ((overlayPixel.A - basePixel.A) * opacity)), Clamp(basePixel.R + (overlayPixel.R - basePixel.R) * opacity), Clamp(basePixel.G + (overlayPixel.G - basePixel.G) * opacity), Clamp(basePixel.B + (overlayPixel.B - basePixel.B) * opacity));
		}

		/// <summary>
		/// Computes the transition color at the state specified.
		/// </summary>
		/// <param name="basePixel">The original pixel color.</param>
		/// <param name="overlayPixel">The target pixel color.</param>
		/// <param name="gradient">The transition state (0 - original, 1 - target color).</param>
		/// <param name="offset">The transition linear offset.</param>
		public static ColorF Transition(this ColorF basePixel, ColorF overlayPixel, float gradient, float offset) {
			if (gradient == 0f)
				return basePixel;
			else if (gradient == 1f)
				return overlayPixel;
			Vector4 result = Vector4.Lerp(basePixel.Components, overlayPixel.Components, gradient);

			offset = Math.Abs(offset);
			if (offset > 0f) {
				result = Vector4.Clamp(result, Vector4.Zero, Vector4.One);
				if (Math.Abs(overlayPixel.A - result.X) <= offset)
					result.X = overlayPixel.A;
				else if (overlayPixel.A > result.X)
					result.X += offset;
				else
					result.X -= offset;

				if (Math.Abs(overlayPixel.R - result.Y) <= offset)
					result.Y = overlayPixel.R;
				else if (overlayPixel.R > result.Y)
					result.Y += offset;
				else
					result.Y -= offset;

				if (Math.Abs(overlayPixel.G - result.Z) <= offset)
					result.Z = overlayPixel.G;
				else if (overlayPixel.G > result.Z)
					result.Z += offset;
				else
					result.Z -= offset;

				if (Math.Abs(overlayPixel.B - result.W) <= offset)
					result.W = overlayPixel.B;
				else if (overlayPixel.B > result.W)
					result.W += offset;
				else
					result.W -= offset;
			}

			return new ColorF(result);
		}

		/// <summary>
		/// Computes the transition value at the state specified.
		/// </summary>
		/// <param name="baseValue">The original value.</param>
		/// <param name="targetValue">The target value.</param>
		/// <param name="state">The transition state (0 - original value, 1 - target value).</param>
		/// <param name="offset">The transition linear offset.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static float Transition(this float baseValue, float targetValue, float state, float offset) {
			baseValue += (targetValue - baseValue) * state;
			offset = Math.Abs(offset);
			if (Math.Abs(targetValue - baseValue) <= offset)
				baseValue = targetValue;
			else if (targetValue > baseValue)
				baseValue += offset;
			else
				baseValue -= offset;
			return baseValue;
		}

		/// <summary>
		/// Computes the transition value at the state specified.
		/// </summary>
		/// <param name="baseValue">The original value.</param>
		/// <param name="targetValue">The target value.</param>
		/// <param name="state">The transition state (0 - original value, 1 - target value).</param>
		/// <param name="offset">The transition linear offset.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static double Transition(this double baseValue, double targetValue, double state, double offset) {
			baseValue += (targetValue - baseValue) * state;
			offset = Math.Abs(offset);
			if (Math.Abs(targetValue - baseValue) <= offset)
				baseValue = targetValue;
			else if (targetValue > baseValue)
				baseValue += offset;
			else
				baseValue -= offset;
			return baseValue;
		}

		/// <summary>
		/// Computes the transition value at the state specified but clamps gradient to the range [0, 1].
		/// </summary>
		/// <param name="basePixel">The original pixel color.</param>
		/// <param name="overlayPixel">The target pixel color.</param>
		/// <param name="gradient">The transition state (0 - original, 1 - target color).</param>
		/// <param name="offset">The transition linear offset.</param>
		public static ColorF TransitionClamp(this ColorF basePixel, ColorF overlayPixel, float gradient, float offset) {
			if (gradient <= float.Epsilon)
				return basePixel;
			else if (gradient >= 1f)
				return overlayPixel;
			Vector4 result = Vector4.Lerp(basePixel.Components, overlayPixel.Components, gradient);

			offset = Math.Abs(offset);
			if (offset > 0f) {
				result = Vector4.Clamp(result, Vector4.Zero, Vector4.One);
				if (Math.Abs(overlayPixel.A - result.X) <= offset)
					result.X = overlayPixel.A;
				else if (overlayPixel.A > result.X)
					result.X += offset;
				else
					result.X -= offset;

				if (Math.Abs(overlayPixel.R - result.Y) <= offset)
					result.Y = overlayPixel.R;
				else if (overlayPixel.R > result.Y)
					result.Y += offset;
				else
					result.Y -= offset;

				if (Math.Abs(overlayPixel.G - result.Z) <= offset)
					result.Z = overlayPixel.G;
				else if (overlayPixel.G > result.Z)
					result.Z += offset;
				else
					result.Z -= offset;

				if (Math.Abs(overlayPixel.B - result.W) <= offset)
					result.W = overlayPixel.B;
				else if (overlayPixel.B > result.W)
					result.W += offset;
				else
					result.W -= offset;
			}

			return new ColorF(result);
		}

		/// <summary>
		/// Computes the transition value at the state specified but clamps gradient to the range [0, 1].
		/// </summary>
		/// <param name="baseValue">The original value.</param>
		/// <param name="targetValue">The target value.</param>
		/// <param name="state">The transition state (0 - original value, 1 - target value).</param>
		/// <param name="offset">The transition linear offset.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static float TransitionClamp(this float baseValue, float targetValue, float state, float offset) {
			if (state <= float.Epsilon)
				return baseValue;
			else if (state >= 1f)
				return targetValue;
			baseValue += (targetValue - baseValue) * state;
			offset = Math.Abs(offset);
			if (Math.Abs(targetValue - baseValue) <= offset)
				baseValue = targetValue;
			else if (targetValue > baseValue)
				baseValue += offset;
			else
				baseValue -= offset;
			return baseValue;
		}

		/// <summary>
		/// Computes the transition value at the state specified but clamps gradient to the range [0, 1].
		/// </summary>
		/// <param name="baseValue">The original value.</param>
		/// <param name="targetValue">The target value.</param>
		/// <param name="state">The transition state (0 - original value, 1 - target value).</param>
		/// <param name="offset">The transition linear offset.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static double TransitionClamp(this double baseValue, double targetValue, double state, double offset) {
			if (state <= float.Epsilon)
				return baseValue;
			else if (state >= 1f)
				return targetValue;
			baseValue += (targetValue - baseValue) * state;
			offset = Math.Abs(offset);
			if (Math.Abs(targetValue - baseValue) <= offset)
				baseValue = targetValue;
			else if (targetValue > baseValue)
				baseValue += offset;
			else
				baseValue -= offset;
			return baseValue;
		}

		/// <summary>
		/// Computes the transition value at the state specified.
		/// </summary>
		/// <param name="baseValue">The original value.</param>
		/// <param name="targetValue">The target value.</param>
		/// <param name="state">The transition state (0 - original value, 1 - target value).</param>
		public static float Transition(this float baseValue, float targetValue, float state) {
			return baseValue + (targetValue - baseValue) * state;
		}

		/// <summary>
		/// Computes the transition value at the state specified.
		/// </summary>
		/// <param name="baseValue">The original value.</param>
		/// <param name="targetValue">The target value.</param>
		/// <param name="state">The transition state (0 - original value, 1 - target value).</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static double Transition(this double baseValue, double targetValue, double state) {
			return baseValue + (targetValue - baseValue) * state;
		}

		/// <summary>
		/// Applies the specifies filter to the given image.
		/// </summary>
		/// <param name="image">The image to apply the filter to.</param>
		/// <param name="type">The type of convolution filter to apply.</param>
		/// <param name="radius">The radius of the blur filter. Non-integer values are slower to process.</param>
		/// <param name="amount">The mix level of the filter.</param>
		public static void ApplyFilter(this Bitmap image, Filter type, float radius, float amount = 1f) {
			if (radius == 0f || amount == 0f)
				return;
			byte[] buffer = null;
			using (PixelWorker source = PixelWorker.FromImage(image, false, true, ImageParameterAction.RemoveReference))
				ApplyFilter(source, type, radius, amount, ref buffer);
		}

		/// <summary>
		/// Applies the specifies filter to the given image.
		/// </summary>
		/// <param name="image">The image to apply the filter to.</param>
		/// <param name="type">The type of convolution filter to apply.</param>
		/// <param name="radius">The radius of the blur filter. Non-integer values are slower to process.</param>
		/// <param name="amount">The mix level of the filter.</param>
		/// <param name="buffer">An optional buffer the size of PixelComponentCount of the image (or more) to use as working memory.</param>
		public static void ApplyFilter(this Bitmap image, Filter type, float radius, float amount, ref byte[] buffer) {
			if (radius == 0f || amount == 0f)
				return;
			using (PixelWorker source = PixelWorker.FromImage(image, false, true, ImageParameterAction.RemoveReference))
				ApplyFilter(source, type, radius, amount, ref buffer);
		}

		/// <summary>
		/// Applies the specifies filter to the given image.
		/// </summary>
		/// <param name="source">The image to apply the filter to The image to apply the filter to. WriteChanges() is not called, that's on you.</param>
		/// <param name="type">The type of convolution filter to apply.</param>
		/// <param name="radius">The radius of the blur filter. Non-integer values are slower to process.</param>
		/// <param name="amount">The mix level of the filter.</param>
		public static void ApplyFilter(this PixelWorker source, Filter type, float radius, float amount = 1f) {
			byte[] buffer = null;
			ApplyFilter(source, type, radius, amount, ref buffer);
		}

		/// <summary>
		/// Applies the specifies filter to the given image.
		/// </summary>
		/// <param name="source">The image to apply the filter to The image to apply the filter to. WriteChanges() is not called, that's on you.</param>
		/// <param name="type">The type of convolution filter to apply.</param>
		/// <param name="radius">The radius of the blur filter. Non-integer values are slower to process.</param>
		/// <param name="amount">The mix level of the filter.</param>
		/// <param name="buffer">An optional buffer the size of PixelComponentCount of the image (or more) to use as working memory.</param>
		public static void ApplyFilter(this PixelWorker source, Filter type, float radius, float amount, ref byte[] buffer) {
			if (radius == 0f || amount == 0f)
				return;
			radius = Math.Min(radius, Math.Max(source.Width, source.Height));
			switch (type) {
				case Filter.GaussianBlur:
					if (radius < 0f) {
						radius = -radius;
						type = Filter.UnsharpMask;
					}
					radius *= amount;
					break;
				case Filter.UnsharpMask:
					if (radius < 0f) {
						radius = -radius;
						type = Filter.GaussianBlur;
					}
					radius *= amount;
					break;
				case Filter.Sharpen:
					if (radius < 0f) {
						radius = -radius;
						type = Filter.GaussianBlur;
					}
					break;
			}
			float fraction = radius % 1f;
			if (fraction <= float.Epsilon)
				ApplyFilter(source, type, (int) radius, amount, ref buffer);
			else {
				using (PixelWorker wrapper = new PixelWorker(source.ToByteArray(true), source.Width, source.Height, false)) {
					if (radius <= 1f) {
						ApplyFilter(wrapper, type, 1, amount, ref buffer);
						Transition(source, wrapper, radius);
					} else {
						if (buffer == null || buffer.Length < source.PixelComponentCount)
							buffer = new byte[source.PixelComponentCount];
						ApplyFilter(source, type, (int) radius, amount, ref buffer);
						ApplyFilter(wrapper, type, (int) radius + 1, amount, ref buffer);
						Transition(source, wrapper, fraction);
					}
				}
			}
		}

		/// <summary>
		/// Applies the specifies filter to the given image.
		/// </summary>
		/// <param name="image">The image to apply the filter to.</param>
		/// <param name="type">The type of convolution filter to apply.</param>
		/// <param name="radius">The radius of the blur filter.</param>
		/// <param name="amount">The mix level of the filter.</param>
		public static void ApplyFilter(this Bitmap image, Filter type, int radius, float amount = 1f) {
			byte[] buffer = null;
			using (PixelWorker source = PixelWorker.FromImage(image, false, true, ImageParameterAction.RemoveReference))
				ApplyFilter(source, type, radius, amount, ref buffer);
		}

		/// <summary>
		/// Applies the specifies filter to the given image.
		/// </summary>
		/// <param name="image">The image to apply the filter to.</param>
		/// <param name="type">The type of convolution filter to apply.</param>
		/// <param name="radius">The radius of the blur filter.</param>
		/// <param name="amount">The mix level of the filter.</param>
		/// <param name="buffer">An optional buffer the size of PixelComponentCount of the image (or more) to use as working memory.</param>
		public static void ApplyFilter(this Bitmap image, Filter type, int radius, float amount, ref byte[] buffer) {
			using (PixelWorker source = PixelWorker.FromImage(image, false, true, ImageParameterAction.RemoveReference))
				ApplyFilter(source, type, radius, amount, ref buffer);
		}

		/// <summary>
		/// Applies the specifies filter to the given image.
		/// </summary>
		/// <param name="source">The image to apply the filter to. WriteChanges() is not called, that's on you.</param>
		/// <param name="type">The type of convolution filter to apply.</param>
		/// <param name="radius">The radius of the blur filter.</param>
		/// <param name="amount">The mix level of the filter.</param>
		public static void ApplyFilter(this PixelWorker source, Filter type, int radius, float amount = 1f) {
			byte[] buffer = null;
			ApplyFilter(source, type, radius, amount, ref buffer);
		}

		/// <summary>
		/// Applies the specifies filter to the given image.
		/// </summary>
		/// <param name="source">The image to apply the filter to. WriteChanges() is not called, that's on you.</param>
		/// <param name="type">The type of convolution filter to apply.</param>
		/// <param name="radius">The radius of the blur filter.</param>
		/// <param name="amount">The mix level of the filter.</param>
		/// <param name="buffer">An optional buffer the size of PixelComponentCount of the image (or more) to use as working memory.</param>
		public static void ApplyFilter(this PixelWorker source, Filter type, int radius, float amount, ref byte[] buffer) {
			if (radius == 0 || amount == 0f)
				return;
			radius = Math.Min(radius, Math.Max(source.Width, source.Height));
			int kernelSum = 0;
			int kernelLength = radius + radius + 1;
			int[][] multable;
			Tuple<int, int[][]> kernel;
			if (type == Filter.Sharpen)
				kernel = CalculateSharpenKernel(radius);
			else
				kernel = CalculateBlurKernel(radius);
			kernelSum = kernel.Item1;
			multable = kernel.Item2;
			if (buffer == null || buffer.Length < source.PixelComponentCount)
				buffer = new byte[source.PixelComponentCount];
			byte[] container = buffer;
			float inverseSquare = amount / (radius * radius);
			ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
				int componentCount = source.ComponentCount;
				int componentIndex = i % componentCount;
				int pixelIndex = i / componentCount;
				int start = ((pixelIndex / source.Width) * source.Width) * componentCount + componentIndex;
				int last = start + source.WidthComponentCount - componentCount;
				int read = (pixelIndex - radius) * componentCount + componentIndex;
				int sum = 0;
				for (int kerneli = 0; kerneli < kernelLength; kerneli++) {
					if (read < start)
						sum += multable[kerneli][source[start]];
					else if (read > last)
						sum += multable[kerneli][source[last]];
					else
						sum += multable[kerneli][source[read]];
					read += componentCount;
				}
				container[i] = type == Filter.Sharpen ? Clamp(source[i] + sum * inverseSquare) : (byte) (sum / kernelSum);
			});
			ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
				int widthComponentCount = source.WidthComponentCount;
				int pixelIndex = i / source.ComponentCount;
				int componentIndex = i % source.ComponentCount;
				int x = pixelIndex % source.Width;
				int start = x * source.ComponentCount + componentIndex;
				int last = start + source.PixelComponentCount - widthComponentCount;
				int read = ((pixelIndex / source.Width - radius) * source.Width + x) * source.ComponentCount + componentIndex;
				int sum = 0;
				for (int kerneli = 0; kerneli < kernelLength; kerneli++) {
					if (read < start)
						sum += multable[kerneli][container[start]];
					else if (read > last)
						sum += multable[kerneli][container[last]];
					else
						sum += multable[kerneli][container[read]];
					read += widthComponentCount;
				}
				switch (type) {
					case Filter.GaussianBlur:
						source[i] = (byte) (sum / kernelSum);
						break;
					case Filter.UnsharpMask:
						source[i] = Clamp(source[i] * 2 - sum / kernelSum);
						break;
					default:
						source[i] = Clamp(container[i] + sum * inverseSquare);
						break;
				}
			});
		}

		/// <summary>
		/// For internal use only.
		/// </summary>
		/// <param name="radius">The blur radius.</param>
		public static Tuple<int, int[][]> CalculateBlurKernel(int radius) {
			Tuple<int, int[][]> val;
			lock (blurSyncRoot)
				val = GaussianKernels[radius];
			if (val == null) {
				int radInc = radius + 1;
				int kernelLength = radInc + radius;
				int kernelSum = 0;
				int[][] multable = new int[kernelLength][];
				for (int i = 0; i < kernelLength; i++)
					multable[i] = new int[256];
				int left, right, index, j;
				for (int kernelIndex = 1; kernelIndex < radInc; kernelIndex++) {
					left = radius - kernelIndex;
					right = radius + kernelIndex;
					index = left + 1;
					index *= index;
					kernelSum += index + index;
					for (j = 0; j < 256; j++)
						multable[left][j] = multable[right][j] = index * j;
				}
				index = radius + 1;
				index *= index;
				kernelSum += index;
				for (j = 0; j < 256; j++)
					multable[radius][j] = index * j;
				val = new Tuple<int, int[][]>(kernelSum, multable);
				lock (blurSyncRoot)
					GaussianKernels[radius] = val;
			}
			return val;
		}

		private static Tuple<int, int[][]> CalculateSharpenKernel(int radius) {
			Tuple<int, int[][]> val;
			lock (sharpenSyncRoot)
				val = SharpenKernels[radius];
			if (val == null) {
				int radInc = radius + 1;
				int kernelLength = radInc + radius;
				int kernelSum = 0;
				int[][] multable = new int[kernelLength][];
				for (int i = 0; i < kernelLength; i++)
					multable[i] = new int[256];
				int left, right, index, j;
				for (int kernelIndex = 1; kernelIndex < radInc; kernelIndex++) {
					index = radInc - kernelIndex;
					left = radius - kernelIndex;
					right = radius + kernelIndex;
					kernelSum += index + index;
					for (j = 0; j < 256; j++)
						multable[left][j] = multable[right][j] = -index * j;
				}
				for (j = 0; j < 256; j++)
					multable[radius][j] = kernelSum * j;
				val = new Tuple<int, int[][]>(kernelSum, multable);
				lock (sharpenSyncRoot)
					SharpenKernels[radius] = val;
			}
			return val;
		}

		/// <summary>
		/// Applies the specifies filter to the given image in the spacial domain. KERNEL LENGTH SHOULD BE ODD FOR SYMMETRY.
		/// </summary>
		/// <param name="image">The image to apply the filter to.</param>
		/// <param name="kernelHorizontal">The 1D kernel to convolve horizontally with (can be null).</param>
		/// <param name="kernelVertical">The 1D kernel to convolve vertically with (can be null).</param>
		/// <param name="skipAlpha">If true, the alpha channel will be untouched.</param>
		public static void ApplyFilter(this Bitmap image, float[] kernelHorizontal, float[] kernelVertical, bool skipAlpha = false) {
			if ((kernelHorizontal == null || kernelHorizontal.Length == 0) && (kernelVertical == null || kernelVertical.Length == 0))
				return;
			using (PixelWorker source = PixelWorker.FromImage(image, false, true, ImageParameterAction.RemoveReference))
				ApplyFilter(source, kernelHorizontal, kernelVertical, skipAlpha);
		}

		/// <summary>
		/// Convolves the specified kernel with the given image in the spacial domain. KERNEL LENGTH SHOULD BE ODD FOR SYMMETRY.
		/// </summary>
		/// <param name="source">The image to apply the filter to. WriteChanges() is not called, that's on you.</param>
		/// <param name="kernelHorizontal">The 1D kernel to convolve horizontally with (can be null).</param>
		/// <param name="kernelVertical">The 1D kernel to convolve vertically with (can be null).</param>
		/// <param name="skipAlpha">If true, the alpha channel will be untouched.</param>
		public static void ApplyFilter(this PixelWorker source, float[] kernelHorizontal, float[] kernelVertical, bool skipAlpha = false) {
			if (kernelHorizontal != null && kernelHorizontal.Length == 0)
				kernelHorizontal = null;
			if (kernelVertical != null && kernelVertical.Length == 0)
				kernelVertical = null;
			if (kernelHorizontal == null && kernelVertical == null)
				return;
			float kernelSum = 0f;
			int radius = 0, kernelLength = 0;
			bool needsBufferTransfer = kernelVertical == null || kernelHorizontal == null;
			byte[] buffer = null;
			if (kernelHorizontal != null) {
				kernelSum = 0f;
				kernelLength = kernelHorizontal.Length;
				for (int i = 0; i < kernelLength; i++)
					kernelSum += kernelHorizontal[i];
				radius = kernelHorizontal.Length / 2;
				buffer = new byte[source.PixelComponentCount];
				ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
					int componentCount = source.ComponentCount;
					int componentIndex = i % componentCount;
					if (skipAlpha && componentIndex == 3)
						buffer[i] = source[i];
					else {
						int pixelIndex = i / componentCount;
						int start = ((pixelIndex / source.Width) * source.Width) * componentCount + componentIndex;
						int last = start + source.WidthComponentCount - componentCount;
						int read = (pixelIndex - radius) * componentCount + componentIndex;
						float sum = 0f;
						for (int kerneli = 0; kerneli < kernelLength; kerneli++) {
							if (read < start)
								sum += kernelHorizontal[kerneli] * source[start];
							else if (read > last)
								sum += kernelHorizontal[kerneli] * source[last];
							else
								sum += kernelHorizontal[kerneli] * source[read];
							read += componentCount;
						}
						buffer[i] = kernelSum == 0f ? Clamp(sum) : Clamp(sum / kernelSum);
					}
				});
			}
			if (kernelVertical != null) {
				if (kernelHorizontal != kernelVertical) {
					kernelSum = 0f;
					kernelLength = kernelVertical.Length;
					for (int i = 0; i < kernelLength; i++)
						kernelSum += kernelVertical[i];
					radius = kernelVertical.Length / 2;
				}
				if (buffer == null)
					buffer = new byte[source.PixelComponentCount];
				ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
					int componentIndex = i % source.ComponentCount;
					if (skipAlpha && componentIndex == 3) {
						if (needsBufferTransfer)
							buffer[i] = source[i];
						else
							source[i] = buffer[i];
					} else {
						int widthComponentCount = source.WidthComponentCount;
						int pixelIndex = i / source.ComponentCount;
						int x = pixelIndex % source.Width;
						int start = x * source.ComponentCount + componentIndex;
						int last = start + source.PixelComponentCount - widthComponentCount;
						int read = ((pixelIndex / source.Width - radius) * source.Width + x) * source.ComponentCount + componentIndex;
						float sum = 0f;
						if (needsBufferTransfer) {
							for (int kerneli = 0; kerneli < kernelLength; kerneli++) {
								if (read < start)
									sum += kernelVertical[kerneli] * source[start];
								else if (read > last)
									sum += kernelVertical[kerneli] * source[last];
								else
									sum += kernelVertical[kerneli] * source[read];
								read += widthComponentCount;
							}
							buffer[i] = kernelSum == 0f ? Clamp(sum) : Clamp(sum / kernelSum);
						} else {
							for (int kerneli = 0; kerneli < kernelLength; kerneli++) {
								if (read < start)
									sum += kernelVertical[kerneli] * buffer[start];
								else if (read > last)
									sum += kernelVertical[kerneli] * buffer[last];
								else
									sum += kernelVertical[kerneli] * buffer[read];
								read += widthComponentCount;
							}
							source[i] = kernelSum == 0f ? Clamp(sum) : Clamp(sum / kernelSum);
						}
					}
				});
			}
			if (needsBufferTransfer)
				source.CopyFrom(buffer, false);
		}

		/// <summary>
		/// Convolves the specified kernel with the given image in the spacial domain. KERNEL LENGTHS SHOULD BE ODD FOR SYMMETRY.
		/// </summary>
		/// <param name="image">The image to apply the filter to.</param>
		/// <param name="kernel">The 2D [x][y] kernel to convolve with.</param>
		/// <param name="skipAlpha">If true, the alpha channel will be untouched.</param>
		[CLSCompliant(false)]
		public static Bitmap ApplyFilter(this Bitmap image, float[][] kernel, bool skipAlpha = false) {
			using (PixelWorker source = PixelWorker.FromImage(image, false, false, ImageParameterAction.RemoveReference))
				return ApplyFilter(source, kernel, skipAlpha);
		}

		/// <summary>
		/// Convolves the specified kernel with the given image in the spacial domain. KERNEL LENGTH SHOULD BE ODD FOR SYMMETRY.
		/// </summary>
		/// <param name="source">The image to apply the filter to. WriteChanges() is not called, that's on you.</param>
		/// <param name="kernel">The 2D [x][y] kernel to convolve with.</param>
		/// <param name="skipAlpha">If true, the alpha channel will be untouched.</param>
		[CLSCompliant(false)]
		public static Bitmap ApplyFilter(this PixelWorker source, float[][] kernel, bool skipAlpha = false) {
			Bitmap resultant = new Bitmap(source.Width, source.Height, source.Format);
			float kernelSum = 0f;
			int kernelWidth = kernel.Length;
			int kernelHeight = kernel[0].Length;
			int j;
			float[] temp;
			for (int i = 0; i < kernelWidth; i++) {
				temp = kernel[i];
				for (j = 0; j < kernelHeight; j++)
					kernelSum += temp[j];
			}
			int radiusX = kernelWidth / 2, radiusY = kernelHeight / 2;
			int radiusXMax = (kernelWidth & 1) == 0 ? radiusX : radiusX + 1;
			int radiusYMax = (kernelHeight & 1) == 0 ? radiusY : radiusY + 1;
			using (PixelWorker wrapper = PixelWorker.FromImage(resultant, false, true, ImageParameterAction.RemoveReference)) {
				ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
					int componentCount = source.ComponentCount;
					int componentIndex = i % componentCount;
					if (skipAlpha && componentIndex == 3)
						wrapper[i] = source[i];
					else {
						int pixelIndex = i / componentCount;
						int sourceWidth = source.Width, sourceHeight = source.Height;
						int x = pixelIndex % sourceWidth;
						int y = pixelIndex / sourceWidth;
						float sum = 0f;
						int filterX, index, offset, tempX, tempY;
						for (int filterY = -radiusY; filterY < radiusYMax; filterY++) {
							tempY = y + filterY;
							if (tempY < 0)
								tempY = 0;
							else if (tempY >= sourceHeight)
								tempY = sourceHeight - 1;
							offset = tempY * sourceWidth;
							for (filterX = -radiusX; filterX < radiusXMax; filterX++) {
								tempX = x + filterX;
								if (tempX < 0)
									tempX = 0;
								else if (tempX >= sourceWidth)
									tempX = sourceWidth - 1;
								index = (offset + tempX) * componentCount + componentIndex;
								sum += kernel[filterX + radiusX][filterY + radiusY] * source[index];
							}
						}
						wrapper[i] = kernelSum == 0f ? Clamp(sum) : Clamp(sum / kernelSum);
					}
				});
			}
			return resultant;
		}

		/// <summary>
		/// Convolves the specified kernel with the given values in the spacial domain. KERNEL LENGTH SHOULD BE ODD FOR SYMMETRY.
		/// </summary>
		/// <param name="source">The image to apply the filter to. WriteChanges() is not called, that's on you.</param>
		/// <param name="width">The width of the image window represented by the source values.</param>
		/// <param name="height">The height of the image window represented by the source values.</param>
		/// <param name="kernel">The 2D [x][y] kernel to convolve with.</param>
		/// <param name="skipAlpha">If true, the alpha channel (assumes BGRA) will be untouched.</param>
		[CLSCompliant(false)]
		public static float[] ApplyFilter(this float[] source, int width, int height, float[][] kernel, bool skipAlpha = false) {
			float kernelSum = 0f;
			int kernelWidth = kernel.Length;
			int kernelHeight = kernel[0].Length;
			int j;
			float[] temp;
			for (int i = 0; i < kernelWidth; i++) {
				temp = kernel[i];
				for (j = 0; j < kernelHeight; j++)
					kernelSum += temp[j];
			}
			int radiusX = kernelWidth / 2, radiusY = kernelHeight / 2;
			int radiusXMax = (kernelWidth & 1) == 0 ? radiusX : radiusX + 1;
			int radiusYMax = (kernelHeight & 1) == 0 ? radiusY : radiusY + 1;
			float[] resultant = new float[source.Length];
			int componentCount = source.Length / (width * height);
			ParallelLoop.For(0, source.Length, delegate (int i) {
				int componentIndex = i % componentCount;
				if (skipAlpha && componentIndex == 3)
					resultant[i] = source[i];
				else {
					int pixelIndex = i / componentCount;
					int x = pixelIndex % width;
					int y = pixelIndex / width;
					float sum = 0f;
					int filterX, index, offset, tempX, tempY;
					for (int filterY = -radiusY; filterY < radiusYMax; filterY++) {
						tempY = y + filterY;
						if (tempY < 0)
							tempY = 0;
						else if (tempY >= height)
							tempY = height - 1;
						offset = tempY * width;
						for (filterX = -radiusX; filterX < radiusXMax; filterX++) {
							tempX = x + filterX;
							if (tempX < 0)
								tempX = 0;
							else if (tempX >= width)
								tempX = width - 1;
							index = (offset + tempX) * componentCount + componentIndex;
							sum += kernel[filterX + radiusX][filterY + radiusY] * source[index];
						}
					}
					resultant[i] = kernelSum == 0f ? sum : sum / kernelSum;
				}
			});
			return resultant;
		}

		/// <summary>
		/// Convolves the specified kernel with the given image in the spacial domain. KERNEL LENGTH SHOULD BE ODD FOR SYMMETRY.
		/// </summary>
		/// <param name="source">The image to apply the filter to. WriteChanges() is not called, that's on you.</param>
		/// <param name="width">The width of the image window represented by the source values.</param>
		/// <param name="height">The height of the image window represented by the source values.</param>
		/// <param name="kernelHorizontal">The 1D kernel to convolve horizontally with (can be null).</param>
		/// <param name="kernelVertical">The 1D kernel to convolve vertically with (can be null).</param>
		/// <param name="skipAlpha">If true, the alpha channel (assumes BGRA) will be untouched.</param>
		public static void ApplyFilter(this float[] source, int width, int height, float[] kernelHorizontal, float[] kernelVertical, bool skipAlpha = false) {
			if (kernelHorizontal != null && kernelHorizontal.Length == 0)
				kernelHorizontal = null;
			if (kernelVertical != null && kernelVertical.Length == 0)
				kernelVertical = null;
			if (kernelHorizontal == null && kernelVertical == null)
				return;
			float kernelSum = 0f;
			int radius = 0, kernelLength = 0;
			int componentCount = source.Length / (width * height);
			bool needsBufferTransfer = kernelVertical == null || kernelHorizontal == null;
			float[] buffer = null;
			int widthComponentCount = width * componentCount;
			if (kernelHorizontal != null) {
				kernelSum = 0f;
				kernelLength = kernelHorizontal.Length;
				for (int i = 0; i < kernelLength; i++)
					kernelSum += kernelHorizontal[i];
				radius = kernelHorizontal.Length / 2;
				buffer = new float[source.Length];
				ParallelLoop.For(0, source.Length, delegate (int i) {
					int componentIndex = i % componentCount;
					if (skipAlpha && componentIndex == 3)
						buffer[i] = source[i];
					else {
						int pixelIndex = i / componentCount;
						int start = ((pixelIndex / width) * width) * componentCount + componentIndex;
						int last = start + widthComponentCount - componentCount;
						int read = (pixelIndex - radius) * componentCount + componentIndex;
						float sum = 0f;
						for (int kerneli = 0; kerneli < kernelLength; kerneli++) {
							if (read < start)
								sum += kernelHorizontal[kerneli] * source[start];
							else if (read > last)
								sum += kernelHorizontal[kerneli] * source[last];
							else
								sum += kernelHorizontal[kerneli] * source[read];
							read += componentCount;
						}
						buffer[i] = kernelSum == 0f ? sum : sum / kernelSum;
					}
				});
			}
			if (kernelVertical != null) {
				if (kernelHorizontal != kernelVertical) {
					kernelSum = 0f;
					kernelLength = kernelVertical.Length;
					for (int i = 0; i < kernelLength; i++)
						kernelSum += kernelVertical[i];
					radius = kernelVertical.Length / 2;
				}
				if (buffer == null)
					buffer = new float[source.Length];
				ParallelLoop.For(0, source.Length, delegate (int i) {
					int pixelIndex = i / componentCount;
					int componentIndex = i % componentCount;
					if (skipAlpha && componentIndex == 3) {
						if (needsBufferTransfer)
							buffer[i] = source[i];
						else
							source[i] = buffer[i];
					} else {
						int x = pixelIndex % width;
						int start = x * componentCount + componentIndex;
						int last = start + source.Length - widthComponentCount;
						int read = ((pixelIndex / width - radius) * width + x) * componentCount + componentIndex;
						float sum = 0f;
						if (needsBufferTransfer) {
							for (int kerneli = 0; kerneli < kernelLength; kerneli++) {
								if (read < start)
									sum += kernelVertical[kerneli] * source[start];
								else if (read > last)
									sum += kernelVertical[kerneli] * source[last];
								else
									sum += kernelVertical[kerneli] * source[read];
								read += widthComponentCount;
							}
							buffer[i] = kernelSum == 0f ? sum : sum / kernelSum;
						} else {
							for (int kerneli = 0; kerneli < kernelLength; kerneli++) {
								if (read < start)
									sum += kernelVertical[kerneli] * buffer[start];
								else if (read > last)
									sum += kernelVertical[kerneli] * buffer[last];
								else
									sum += kernelVertical[kerneli] * buffer[read];
								read += widthComponentCount;
							}
							source[i] = kernelSum == 0f ? sum : sum / kernelSum;
						}
					}
				});
			}
			if (needsBufferTransfer)
				Buffer.BlockCopy(buffer, 0, source, 0, buffer.Length * 4);
		}

		/// <summary>
		/// Calculates a signed distance field for each color channel in the image.
		/// </summary>
		/// <param name="image">The image to calculate SDF for.</param>
		/// <param name="searchDistance">The search radius for each pixel.</param>
		/// <param name="threshold">The value to threshold each channel at.</param>
		public static Bitmap SignedDistanceField(this Bitmap image, float searchDistance, byte threshold = 128) {
			if (searchDistance == 0)
				return image.FastCopy();
			using (PixelWorker source = PixelWorker.FromImage(image, false, false, ImageParameterAction.RemoveReference))
				return SignedDistanceField(source, searchDistance, threshold);
		}

		/// <summary>
		/// Calculates a signed distance field for each color channel in the image.
		/// </summary>
		/// <param name="source">The image to calculate SDF for.</param>
		/// <param name="searchDistance">The search radius for each pixel.</param>
		/// <param name="threshold">The value to threshold each channel at.</param>
		public static Bitmap SignedDistanceField(this PixelWorker source, float searchDistance, byte threshold = 128) {
			if (searchDistance == 0)
				return source.ToBitmap();
			searchDistance = Math.Min(Math.Abs(searchDistance), Math.Max(source.Width, source.Height));
			float fraction = searchDistance % 1f;
			if (fraction <= float.Epsilon)
				return SignedDistanceField(source, (int) searchDistance, threshold);
			else if (searchDistance <= 1f) {
				Bitmap result = SignedDistanceField(source, 1, threshold);
				using (PixelWorker overlay = PixelWorker.FromImage(result, false, true, ImageParameterAction.RemoveReference))
					Transition(overlay, source, 1f - searchDistance);
				return result;
			} else {
				Bitmap result = SignedDistanceField(source, (int) searchDistance, threshold);
				using (Bitmap overlay = SignedDistanceField(source, (int) searchDistance + 1, threshold))
					Transition(result, overlay, fraction);
				return result;
			}
		}

		/// <summary>
		/// Calculates a signed distance field for each color channel in the image.
		/// </summary>
		/// <param name="image">The image to calculate SDF for.</param>
		/// <param name="searchDistance">The search radius for each pixel.</param>
		/// <param name="threshold">The value to threshold each channel at.</param>
		public static Bitmap SignedDistanceField(this Bitmap image, int searchDistance, byte threshold = 128) {
			if (searchDistance == 0)
				return image.FastCopy();
			searchDistance = Math.Abs(searchDistance);
			using (PixelWorker source = PixelWorker.FromImage(image, false, false, ImageParameterAction.RemoveReference))
				return SignedDistanceField(source, searchDistance, threshold);
		}

		/// <summary>
		/// Calculates a signed distance field for each color channel in the image.
		/// </summary>
		/// <param name="source">The image to calculate SDF for.</param>
		/// <param name="searchDistance">The search radius for each pixel.</param>
		/// <param name="threshold">The value to threshold each channel at.</param>
		public static Bitmap SignedDistanceField(this PixelWorker source, int searchDistance, byte threshold = 128) {
			if (searchDistance == 0)
				return source.ToBitmap();
			searchDistance = Math.Min(Math.Abs(searchDistance), Math.Max(source.Width, source.Height));
			Bitmap resultant = new Bitmap(source.Width, source.Height, source.Format);
			using (PixelWorker wrapper = PixelWorker.FromImage(resultant, false, true, ImageParameterAction.RemoveReference)) {
				ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
					int componentCount = source.ComponentCount;
					int componentIndex = i % componentCount;
					if (componentIndex == 3)
						wrapper[i] = source[i];
					else {
						int pixelIndex = i / componentCount;
						int sourceWidth = source.Width, sourceHeight = source.Height;
						int x = pixelIndex % sourceWidth;
						int y = pixelIndex / sourceWidth;
						int filterX, index, offset, tempX, tempY;
						int currentDist, distance = int.MaxValue;
						byte current;
						for (int filterY = -searchDistance; filterY <= searchDistance; filterY++) {
							tempY = y + filterY;
							if (tempY >= 0 && tempY < sourceHeight) {
								offset = tempY * sourceWidth;
								for (filterX = -searchDistance; filterX <= searchDistance; filterX++) {
									tempX = x + filterX;
									if (tempX >= 0 && tempX < sourceWidth) {
										index = (offset + tempX) * componentCount + componentIndex;
										current = source[index];
										if (current >= threshold) {
											currentDist = (filterX * filterX) + (filterY * filterY);
											if (Math.Abs(currentDist) < Math.Abs(distance))
												distance = currentDist;
										}
									}
								}
							}
						}
						byte result;
						if (distance == int.MaxValue) {
							result = source[i];
							result = result < threshold ? (byte) 0 : (byte) 255;
						} else
							result = (byte) (255.0 - ClampDouble((Math.Sqrt(distance).Clamp(-searchDistance, searchDistance) + searchDistance) * 255.0 / (searchDistance + searchDistance)));
						wrapper[i] = result;
					}
				});
			}
			return resultant;
		}

		/// <summary>
		/// Shrinks the brighter regions in the image.
		/// </summary>
		/// <param name="image">The image to erode.</param>
		/// <param name="radius">The radius to erode at in pixels.</param>
		public static Bitmap Erode(this Bitmap image, float radius) {
			using (PixelWorker source = PixelWorker.FromImage(image, false, false, ImageParameterAction.RemoveReference))
				return Erode(source, radius);
		}

		/// <summary>
		/// Shrinks the brighter regions in the image.
		/// </summary>
		/// <param name="source">The image to erode.</param>
		/// <param name="radius">The radius to erode at in pixels.</param>
		public static Bitmap Erode(this PixelWorker source, float radius) {
			if (radius <= 0f)
				return source.ToBitmap();
			radius = Math.Min(radius, Math.Max(source.Width, source.Height));
			float fraction = radius % 1f;
			if (fraction <= float.Epsilon)
				return Erode(source, (int) radius);
			else if (radius <= 1f) {
				Bitmap result = Erode(source, 1);
				using (PixelWorker overlay = PixelWorker.FromImage(result, false, true, ImageParameterAction.RemoveReference))
					Transition(overlay, source, 1f - radius);
				return result;
			} else {
				Bitmap result = Erode(source, (int) radius);
				using (Bitmap overlay = Erode(source, (int) radius + 1))
					Transition(result, overlay, fraction);
				return result;
			}
		}

		/// <summary>
		/// Shrinks the brighter regions in the image.
		/// </summary>
		/// <param name="image">The image to erode.</param>
		/// <param name="radius">The radius to erode at in pixels.</param>
		public static Bitmap Erode(this Bitmap image, int radius) {
			using (PixelWorker source = PixelWorker.FromImage(image, false, false, ImageParameterAction.RemoveReference))
				return Erode(source, radius);
		}

		/// <summary>
		/// Shrinks the brighter regions in the image.
		/// </summary>
		/// <param name="source">The image to erode.</param>
		/// <param name="radius">The radius to erode at in pixels.</param>
		public static Bitmap Erode(this PixelWorker source, int radius) {
			Bitmap resultant = new Bitmap(source.Width, source.Height, source.Format);
			using (PixelWorker wrapper = PixelWorker.FromImage(resultant, false, true, ImageParameterAction.RemoveReference))
				Erode(source, wrapper, radius);
			return resultant;
		}

		private static void Erode(this PixelWorker source, PixelWorker output, int radius) {
			radius = Math.Min(radius, Math.Max(source.Width, source.Height));
			ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
				int componentCount = source.ComponentCount;
				int componentIndex = i % componentCount;
				int pixelIndex = i / componentCount;
				int sourceWidth = source.Width, sourceHeight = source.Height;
				int x = pixelIndex % sourceWidth;
				int y = pixelIndex / sourceWidth;
				int filterX, index, offset, tempX, tempY;
				byte min = 255;
				for (int filterY = -radius; filterY <= radius; filterY++) {
					tempY = y + filterY;
					if (tempY >= 0 && tempY < sourceHeight) {
						offset = tempY * sourceWidth;
						for (filterX = -radius; filterX <= radius; filterX++) {
							tempX = x + filterX;
							if (tempX >= 0 && tempX < sourceWidth) {
								index = (offset + tempX) * componentCount + componentIndex;
								min = Math.Min(min, source[index]);
							}
						}
					}
				}
				output[i] = min;
			});
		}

		/// <summary>
		/// Enlarges the brighter regions in the image.
		/// </summary>
		/// <param name="image">The image to dilate.</param>
		/// <param name="radius">The radius to dilate at in pixels.</param>
		public static Bitmap Dilate(this Bitmap image, int radius) {
			using (PixelWorker source = PixelWorker.FromImage(image, false, false, ImageParameterAction.RemoveReference))
				return Dilate(source, radius);
		}

		/// <summary>
		/// Enlarges the brighter regions in the image.
		/// </summary>
		/// <param name="source">The image to dilate.</param>
		/// <param name="radius">The radius to dilate at in pixels.</param>
		public static Bitmap Dilate(this PixelWorker source, int radius) {
			Bitmap resultant = new Bitmap(source.Width, source.Height, source.Format);
			using (PixelWorker wrapper = PixelWorker.FromImage(resultant, false, true, ImageParameterAction.RemoveReference))
				Dilate(source, wrapper, radius);
			return resultant;
		}

		/// <summary>
		/// Enlarges the brighter regions in the image.
		/// </summary>
		/// <param name="image">The image to dilate.</param>
		/// <param name="radius">The radius to dilate at in pixels.</param>
		public static Bitmap Dilate(this Bitmap image, float radius) {
			using (PixelWorker source = PixelWorker.FromImage(image, false, false, ImageParameterAction.RemoveReference))
				return Dilate(source, radius);
		}

		/// <summary>
		/// Enlarges the brighter regions in the image.
		/// </summary>
		/// <param name="source">The image to dilate.</param>
		/// <param name="radius">The radius to dilate at in pixels.</param>
		public static Bitmap Dilate(this PixelWorker source, float radius) {
			if (radius <= 0f)
				return source.ToBitmap();
			radius = Math.Min(radius, Math.Max(source.Width, source.Height));
			float fraction = radius % 1f;
			if (fraction <= float.Epsilon)
				return Dilate(source, (int) radius);
			else if (radius <= 1f) {
				Bitmap result = Dilate(source, 1);
				using (PixelWorker overlay = PixelWorker.FromImage(result, false, true, ImageParameterAction.RemoveReference))
					Transition(overlay, source, 1f - radius);
				return result;
			} else {
				Bitmap result = Dilate(source, (int) radius);
				using (Bitmap overlay = Dilate(source, (int) radius + 1))
					Transition(result, overlay, fraction);
				return result;
			}
		}

		private static void Dilate(this PixelWorker source, PixelWorker output, int radius) {
			radius = Math.Min(radius, Math.Max(source.Width, source.Height));
			ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
				int componentCount = source.ComponentCount;
				int componentIndex = i % componentCount;
				int pixelIndex = i / componentCount;
				int sourceWidth = source.Width, sourceHeight = source.Height;
				int x = pixelIndex % sourceWidth;
				int y = pixelIndex / sourceWidth;
				int filterX, index, offset, tempX, tempY;
				byte max = 0;
				for (int filterY = -radius; filterY <= radius; filterY++) {
					tempY = y + filterY;
					if (tempY >= 0 && tempY < sourceHeight) {
						offset = tempY * sourceWidth;
						for (filterX = -radius; filterX <= radius; filterX++) {
							tempX = x + filterX;
							if (tempX >= 0 && tempX < sourceWidth) {
								index = (offset + tempX) * componentCount + componentIndex;
								max = Math.Max(max, source[index]);
							}
						}
					}
				}
				output[i] = max;
			});
		}

		/// <summary>
		/// Initializes a PixelWorker from the specified image.
		/// </summary>
		/// <param name="image">The image to get the required data from.</param>
		/// <param name="useBuffer">Whether changing pixel values are written to a buffer instead of directly to the image (false means faster).</param>
		/// <param name="writeOnDispose">Whether to write changes on dispose.</param>
		/// <param name="imageAction">What to do when the passed image is no longer used</param>
		/// <param name="doNotUseImageDirectly">If true, the image is copied into a buffer and the original is left untouched.</param>
		public static PixelWorker ToWorker(this Bitmap image, bool useBuffer, bool writeOnDispose, ImageParameterAction imageAction = ImageParameterAction.RemoveReference, bool doNotUseImageDirectly = false) {
			return PixelWorker.FromImage(image, useBuffer, writeOnDispose, imageAction, doNotUseImageDirectly);
		}

		/// <summary>
		/// Changes the gamma of the specified image.
		/// </summary>
		/// <param name="image">The color whose gamma to change.</param>
		/// <param name="gamma">>The gamma multiplier (0 means black, 1 means gamma is unchanged, larger numbers increase gamma).</param>
		public static void ChangeGamma(this Bitmap image, float gamma) {
			if (image == null || gamma == 1f)
				return;
			using (PixelWorker source = PixelWorker.FromImage(image, false, true, ImageParameterAction.RemoveReference))
				ChangeGamma(source, gamma);
		}

		/// <summary>
		/// Changes the gamma of the specified image.
		/// </summary>
		/// <param name="source">The color whose gamma to change. WriteChanges() is not called, that's on you.</param>
		/// <param name="gamma">The gamma multiplier (0 means black, 1 means gamma is unchanged, larger numbers increase gamma).</param>
		public static void ChangeGamma(this PixelWorker source, float gamma) {
			if (source == null || gamma == 1f)
				return;
			unsafe
			{
				byte* gammaArray = stackalloc byte[256];
				gamma = 1f / gamma;
				for (int i = 0; i < 256; i++)
					gammaArray[i] = (byte) Math.Min(255f, (int) ((255f * Math.Pow(i * 0.00392156862f, gamma)) + 0.5f));
				if (source.ComponentCount == 4) {
					if (source.Buffer == null) {
						unsafe
						{
							ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
								if (i % 4 != 3) {
									byte* index = source.Scan0 + i;
									*index = gammaArray[*index];
								}
							}, ParallelCutoff);
						}
					} else {
						ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
							if (i % 4 != 3)
								source.Buffer[i] = gammaArray[source.Buffer[i]];
						}, ParallelCutoff);
					}
				} else {
					if (source.Buffer == null) {
						unsafe
						{
							ParallelLoop.For(source.Scan0, source.PixelComponentCount, 1L, i => {
								byte* index = (byte*) i;
								*index = gammaArray[*index];
							}, ParallelCutoff);
						}
					} else {
						ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
							source.Buffer[i] = gammaArray[source.Buffer[i]];
						}, ParallelCutoff);
					}
				}
			}
		}

		/// <summary>
		/// Changes the gamma of the specified color.
		/// </summary>
		/// <param name="color">The color whose gamma to change.</param>
		/// <param name="gamma">The gamma multiplier (0 means black, 1 means gamma is unchanged, larger numbers increase gamma).</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Color ChangeGamma(this Color color, float gamma) {
			gamma = 1f / gamma;
			return Color.FromArgb((int) Math.Min(255f, (int) ((255f * Math.Pow(color.A * 0.00392156862f, gamma)) + 0.5f)),
								(int) Math.Min(255f, (int) ((255f * Math.Pow(color.R * 0.00392156862f, gamma)) + 0.5f)),
								(int) Math.Min(255f, (int) ((255f * Math.Pow(color.G * 0.00392156862f, gamma)) + 0.5f)),
								(int) Math.Min(255f, (int) ((255f * Math.Pow(color.B * 0.00392156862f, gamma)) + 0.5f)));
		}

		/// <summary>
		/// Changes the gamma of the specified color.
		/// </summary>
		/// <param name="color">The color whose gamma to change.</param>
		/// <param name="gamma">The gamma multiplier (0 means black, 1 means gamma is unchanged, larger numbers increase gamma).</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static BgraColor ChangeGamma(this BgraColor color, float gamma) {
			gamma = 1f / gamma;
			return new BgraColor((byte) Math.Min(255f, (byte) ((255f * Math.Pow(color.A * 0.00392156862f, gamma)) + 0.5f)),
								(byte) Math.Min(255f, (byte) ((255f * Math.Pow(color.R * 0.00392156862f, gamma)) + 0.5f)),
								(byte) Math.Min(255f, (byte) ((255f * Math.Pow(color.G * 0.00392156862f, gamma)) + 0.5f)),
								(byte) Math.Min(255f, (byte) ((255f * Math.Pow(color.B * 0.00392156862f, gamma)) + 0.5f)));
		}

		/// <summary>
		/// Resizes the image using the nearest neighbor algorithm.
		/// </summary>
		/// <param name="image">The image to resize.</param>
		/// <param name="newSize">The new size of the image.</param>
		public static Bitmap ResizeNearestNeighbor(this Bitmap image, Size newSize) {
			using (PixelWorker source = PixelWorker.FromImage(image, false, false, ImageParameterAction.RemoveReference))
				return ResizeNearestNeighbor(source, newSize);
		}

		/// <summary>
		/// Resizes the image using the nearest neighbor algorithm.
		/// </summary>
		/// <param name="source">The image to resize.</param>
		/// <param name="newSize">The new size of the image.</param>
		public static Bitmap ResizeNearestNeighbor(this PixelWorker source, Size newSize) {
			float ratioX = ((float) source.Width) / newSize.Width;
			float ratioY = ((float) source.Height) / newSize.Height;
			Bitmap newImage = new Bitmap(newSize.Width, newSize.Height, source.Format);
			using (PixelWorker newBitmap = PixelWorker.FromImage(newImage, false, true, ImageParameterAction.RemoveReference)) {
				ParallelLoop.For(0, newBitmap.PixelComponentCount, delegate (int i) {
					int pixelNum = i / newBitmap.ComponentCount;
					newBitmap[i] = source[(((int) Math.Round((pixelNum / newSize.Width) * ratioY)) * source.Width +
						(int) Math.Round((pixelNum % newSize.Width) * ratioX)) * newBitmap.ComponentCount + i % newBitmap.ComponentCount];
				}, ParallelCutoff);
			}
			return newImage;
		}

		/// <summary>
		/// Resizes the image using linear interpolation.
		/// </summary>
		/// <param name="image">The image to resize.</param>
		/// <param name="newSize">The new size of the image.</param>
		public static Bitmap ResizeBilinear(this Bitmap image, Size newSize) {
			using (PixelWorker source = PixelWorker.FromImage(image, false, false, ImageParameterAction.RemoveReference))
				return ResizeBilinear(source, newSize);
		}

		/// <summary>
		/// Resizes the image using linear interpolation.
		/// </summary>
		/// <param name="source">The image to resize.</param>
		/// <param name="newSize">The new size of the image.</param>
		public static Bitmap ResizeBilinear(this PixelWorker source, Size newSize) {
			float ratioX = ((float) source.Width) / newSize.Width;
			float ratioY = ((float) source.Height) / newSize.Height;
			Bitmap newImage = new Bitmap(newSize.Width, newSize.Height, source.Format);
			using (PixelWorker newBitmap = PixelWorker.FromImage(newImage, false, true, ImageParameterAction.RemoveReference)) {
				ParallelLoop.For(0, newBitmap.PixelComponentCount, delegate (int i) {
					int cc = source.ComponentCount;
					int pixelNum = i / cc;
					float sourceX = (pixelNum % newSize.Width) * ratioX;
					float sourceY = (pixelNum / newSize.Width) * ratioY;
					int sX = (int) sourceX, sY = (int) sourceY;
					int componentIndex = i % cc;
					bool isInRangeX = sourceX < source.Width - 1;
					bool isInRangeY = sourceY < source.Height - 1;
					int x1y1 = (sY * source.Width + sX) * cc + componentIndex;
					int x2y1 = isInRangeX ? (sY * source.Width + sX + 1) * cc + componentIndex : x1y1;
					int x1y2 = isInRangeY ? ((sY + 1) * source.Width + sX) * cc + componentIndex : x1y1;
					int x2y2;
					if (isInRangeX) {
						if (isInRangeY)
							x2y2 = ((sY + 1) * source.Width + sX + 1) * cc + componentIndex;
						else
							x2y2 = x2y1;
					} else if (isInRangeY)
						x2y2 = x1y2;
					else
						x2y2 = x1y1;
					newBitmap[i] = InterpolateLinear(source[x1y1], source[x2y1], source[x1y2], source[x2y2], sourceX % 1f, sourceY % 1f);
				}, ParallelCutoff / 4);
			}
			return newImage;
		}

		/// <summary>
		/// Resizes the image using bicubic interpolation.
		/// </summary>
		/// <param name="image">The image to resize.</param>
		/// <param name="newSize">The new size of the image.</param>
		public static Bitmap ResizeBicubic(this Bitmap image, Size newSize) {
			using (PixelWorker source = PixelWorker.FromImage(image, false, false, ImageParameterAction.RemoveReference))
				return ResizeBicubic(source, newSize);
		}

		/// <summary>
		/// Resizes the image using bicubic interpolation.
		/// </summary>
		/// <param name="source">The image to resize.</param>
		/// <param name="newSize">The new size of the image.</param>
		public static Bitmap ResizeBicubic(this PixelWorker source, Size newSize) {
			double ratioX = ((double) source.Width) / newSize.Width;
			double ratioY = ((double) source.Height) / newSize.Height;
			Bitmap newImage = new Bitmap(newSize.Width, newSize.Height, source.Format);
			using (PixelWorker newBitmap = PixelWorker.FromImage(newImage, false, true, ImageParameterAction.RemoveReference)) {
				ParallelLoop.For(0, newBitmap.PixelCount, delegate (int i) {
					double sourceX = (i % newSize.Width) * ratioX;
					double sourceY = (i / newSize.Width) * ratioY;
					int s2X = (int) sourceX, s2Y = (int) sourceY;
					int s1X = s2X - 1, s1Y = s2Y - 1;
					int s3X = s2X + 1, s3Y = s2Y + 1;
					int s4X = s2X + 2, s4Y = s2Y + 2;
					newBitmap.SetPixelUsingPixelCount(i, InterpolateCubic(source.GetPixelClampedBgra(s1X, s1Y), source.GetPixelClampedBgra(s2X, s1Y), source.GetPixelClampedBgra(s3X, s1Y), source.GetPixelClampedBgra(s4X, s1Y),
						source.GetPixelClampedBgra(s1X, s2Y), source.GetPixelBgra(s2X, s2Y), source.GetPixelClampedBgra(s3X, s2Y), source.GetPixelClampedBgra(s4X, s2Y),
						source.GetPixelClampedBgra(s1X, s3Y), source.GetPixelClampedBgra(s2X, s3Y), source.GetPixelClampedBgra(s3X, s3Y), source.GetPixelClampedBgra(s4X, s3Y),
						source.GetPixelClampedBgra(s1X, s4Y), source.GetPixelClampedBgra(s2X, s4Y), source.GetPixelClampedBgra(s3X, s4Y), source.GetPixelClampedBgra(s4X, s4Y),
						sourceX % 1.0, sourceY % 1.0));
				}, ParallelCutoff / 16);
			}
			return newImage;
		}

		/// <summary>
		/// Resizes the image using Lanczos3 interpolation.
		/// </summary>
		/// <param name="image">The image to resize.</param>
		/// <param name="newSize">The new size of the image.</param>
		public static Bitmap ResizeLanczos(this Bitmap image, Size newSize) {
			using (PixelWorker source = PixelWorker.FromImage(image, false, false, ImageParameterAction.RemoveReference))
				return ResizeLanczos(source, newSize);
		}

		/// <summary>
		/// Resizes the image using Lanczos3 interpolation.
		/// </summary>
		/// <param name="source">The image to resize.</param>
		/// <param name="newSize">The new size of the image.</param>
		public static Bitmap ResizeLanczos(this PixelWorker source, Size newSize) {
			double ratioX = ((double) source.Width) / newSize.Width;
			double ratioY = ((double) source.Height) / newSize.Height;
			Bitmap newImage = new Bitmap(newSize.Width, newSize.Height, source.Format);
			using (PixelWorker newBitmap = PixelWorker.FromImage(newImage, false, true, ImageParameterAction.RemoveReference)) {
				ParallelLoop.For(0, newBitmap.PixelComponentCount /*newBitmap.PixelCount*/, delegate (int i) {
					/*double sourceX = (i % newSize.Width) * ratioX;
					double sourceY = (i / newSize.Width) * ratioY;
					int s3X = (int) sourceX, s3Y = (int) sourceY;
					int s1X = s3X - 2, s1Y = s3Y - 2;
					int s2X = s3X - 1, s2Y = s3Y - 1;
					int s4X = s3X + 1, s4Y = s3Y + 1;
					int s5X = s3X + 2, s5Y = s3Y + 2;
					int s6X = s3X + 3, s6Y = s3Y + 3;
					newBitmap.SetPixelUsingPixelCount(i, InterpolateLanczos(source.GetPixelClampedBgra(s1X, s1Y), source.GetPixelClampedBgra(s2X, s1Y), source.GetPixelClampedBgra(s3X, s1Y), source.GetPixelClampedBgra(s4X, s1Y), source.GetPixelClampedBgra(s5X, s1Y), source.GetPixelClampedBgra(s5X, s1Y),
						source.GetPixelClampedBgra(s1X, s2Y), source.GetPixelBgra(s2X, s2Y), source.GetPixelClampedBgra(s3X, s2Y), source.GetPixelClampedBgra(s4X, s2Y), source.GetPixelClampedBgra(s5X, s2Y), source.GetPixelClampedBgra(s6X, s2Y),
						source.GetPixelClampedBgra(s1X, s3Y), source.GetPixelClampedBgra(s2X, s3Y), source.GetPixelClampedBgra(s3X, s3Y), source.GetPixelClampedBgra(s4X, s3Y), source.GetPixelClampedBgra(s5X, s3Y), source.GetPixelClampedBgra(s6X, s3Y),
						source.GetPixelClampedBgra(s1X, s4Y), source.GetPixelClampedBgra(s2X, s4Y), source.GetPixelClampedBgra(s3X, s4Y), source.GetPixelClampedBgra(s4X, s4Y), source.GetPixelClampedBgra(s5X, s4Y), source.GetPixelClampedBgra(s6X, s4Y),
						source.GetPixelClampedBgra(s1X, s5Y), source.GetPixelClampedBgra(s2X, s5Y), source.GetPixelClampedBgra(s3X, s5Y), source.GetPixelClampedBgra(s4X, s5Y), source.GetPixelClampedBgra(s5X, s5Y), source.GetPixelClampedBgra(s6X, s5Y),
						source.GetPixelClampedBgra(s1X, s6Y), source.GetPixelClampedBgra(s2X, s6Y), source.GetPixelClampedBgra(s3X, s6Y), source.GetPixelClampedBgra(s4X, s6Y), source.GetPixelClampedBgra(s5X, s6Y), source.GetPixelClampedBgra(s6X, s6Y),
						sourceX % 1.0, sourceY % 1.0));*/

					int componentCount = source.ComponentCount;
					int componentIndex = i % componentCount;
					int pixelIndex = i / componentCount;
					int sourceWidth = source.Width, sourceHeight = source.Height;
					double sourceX = (pixelIndex % newSize.Width) * ratioX;
					double sourceY = (pixelIndex / newSize.Width) * ratioY;
					int srcX = (int) sourceX;
					int srcY = (int) sourceY;
					sourceX %= 1.0;
					sourceY %= 1.0;
					int filterX, offset, tempX, tempY;
					double lanczosFactor, val = 0.0, sum = 0.0;
					for (int filterY = -2; filterY <= 3; filterY++) {
						tempY = srcY + filterY;
						if (tempY >= 0 && tempY < sourceHeight) {
							offset = tempY * sourceWidth;
							for (filterX = -2; filterX <= 3; filterX++) {
								tempX = srcX + filterX;
								if (tempX >= 0 && tempX < sourceWidth) {
									lanczosFactor = LanczosFilter(sourceX - filterX) * LanczosFilter(sourceY - filterY);
									val += source[(offset + tempX) * componentCount + componentIndex] * lanczosFactor;
									sum += lanczosFactor;
								}
							}
						}
					}
					newBitmap[i] = Clamp(val / sum);
				});
			}
			return newImage;
		}

		/// <summary>
		/// Interpolates the specified values using Lanczos (3x3) sampling.
		/// </summary>
		/// <param name="x1y1">The top-left pixel at (-2, -2).</param>
		/// <param name="x2y1">x2y1</param>
		/// <param name="x3y1">x3y1</param>
		/// <param name="x4y1">x4y1</param>
		/// <param name="x5y1">x5y1</param>
		/// <param name="x6y1">x6y1</param>
		/// <param name="x1y2">x1y2</param>
		/// <param name="x2y2">x2y2</param>
		/// <param name="x3y2">x3y2</param>
		/// <param name="x4y2">x4y2</param>
		/// <param name="x5y2">x5y2</param>
		/// <param name="x6y2">x6y2</param>
		/// <param name="x1y3">x1y3</param>
		/// <param name="x2y3">x2y3</param>
		/// <param name="x3y3">x3y3</param>
		/// <param name="x4y3">x4y3</param>
		/// <param name="x5y3">x5y3</param>
		/// <param name="x6y3">x6y3</param>
		/// <param name="x1y4">x1y4</param>
		/// <param name="x2y4">x2y4</param>
		/// <param name="x3y4">x3y4</param>
		/// <param name="x4y4">x4y4</param>
		/// <param name="x5y4">x5y4</param>
		/// <param name="x6y4">x6y4</param>
		/// <param name="x1y5">x1y5</param>
		/// <param name="x2y5">x2y5</param>
		/// <param name="x3y5">x3y5</param>
		/// <param name="x4y5">x4y5</param>
		/// <param name="x5y5">x5y5</param>
		/// <param name="x6y5">x6y5</param>
		/// <param name="x1y6">x1y6</param>
		/// <param name="x2y6">x2y6</param>
		/// <param name="x3y6">x3y6</param>
		/// <param name="x4y6">x4y6</param>
		/// <param name="x5y6">x5y6</param>
		/// <param name="x6y6">The bottom-right pixel at (3, 3).</param>
		/// <param name="x">A value from 0 to 1 that reflects the X-coordinate of the output pixel relative to the middle pixels.</param>
		/// <param name="y">A value from 0 to 1 that reflects the Y-coordinate of the output pixel relative to the middle pixels.</param>
		public static byte InterpolateLanczos(byte x1y1, byte x2y1, byte x3y1, byte x4y1, byte x5y1, byte x6y1,
											byte x1y2, byte x2y2, byte x3y2, byte x4y2, byte x5y2, byte x6y2,
											byte x1y3, byte x2y3, byte x3y3, byte x4y3, byte x5y3, byte x6y3,
											byte x1y4, byte x2y4, byte x3y4, byte x4y4, byte x5y4, byte x6y4,
											byte x1y5, byte x2y5, byte x3y5, byte x4y5, byte x5y5, byte x6y5,
											byte x1y6, byte x2y6, byte x3y6, byte x4y6, byte x5y6, byte x6y6, double x, double y) {
			double lanczosFactor, val = 0.0, sum = 0.0;
			double x1 = x + 2.0, x2 = x + 1.0, x4 = x - 1.0, x5 = x - 2.0, x6 = x - 3.0;
			double y1 = y + 2.0, y2 = y + 1.0, y4 = y - 1.0, y5 = y - 2.0, y6 = y - 3.0;

			lanczosFactor = LanczosFilter(x1) * LanczosFilter(y1);
			val += x1y1 * lanczosFactor;
			sum += lanczosFactor;
			lanczosFactor = LanczosFilter(x2) * LanczosFilter(y1);
			val += x2y1 * lanczosFactor;
			sum += lanczosFactor;
			lanczosFactor = LanczosFilter(x) * LanczosFilter(y1);
			val += x3y1 * lanczosFactor;
			sum += lanczosFactor;
			lanczosFactor = LanczosFilter(x4) * LanczosFilter(y1);
			val += x4y1 * lanczosFactor;
			sum += lanczosFactor;
			lanczosFactor = LanczosFilter(x5) * LanczosFilter(y1);
			val += x5y1 * lanczosFactor;
			sum += lanczosFactor;
			lanczosFactor = LanczosFilter(x6) * LanczosFilter(y1);
			val += x6y1 * lanczosFactor;
			sum += lanczosFactor;

			lanczosFactor = LanczosFilter(x1) * LanczosFilter(y2);
			val += x1y2 * lanczosFactor;
			sum += lanczosFactor;
			lanczosFactor = LanczosFilter(x2) * LanczosFilter(y2);
			val += x2y2 * lanczosFactor;
			sum += lanczosFactor;
			lanczosFactor = LanczosFilter(x) * LanczosFilter(y2);
			val += x3y2 * lanczosFactor;
			sum += lanczosFactor;
			lanczosFactor = LanczosFilter(x4) * LanczosFilter(y2);
			val += x4y2 * lanczosFactor;
			sum += lanczosFactor;
			lanczosFactor = LanczosFilter(x5) * LanczosFilter(y2);
			val += x5y2 * lanczosFactor;
			sum += lanczosFactor;
			lanczosFactor = LanczosFilter(x6) * LanczosFilter(y2);
			val += x6y2 * lanczosFactor;
			sum += lanczosFactor;

			lanczosFactor = LanczosFilter(x1) * LanczosFilter(y);
			val += x1y3 * lanczosFactor;
			sum += lanczosFactor;
			lanczosFactor = LanczosFilter(x2) * LanczosFilter(y);
			val += x2y3 * lanczosFactor;
			sum += lanczosFactor;
			lanczosFactor = LanczosFilter(x) * LanczosFilter(y);
			val += x3y3 * lanczosFactor;
			sum += lanczosFactor;
			lanczosFactor = LanczosFilter(x4) * LanczosFilter(y);
			val += x4y3 * lanczosFactor;
			sum += lanczosFactor;
			lanczosFactor = LanczosFilter(x5) * LanczosFilter(y);
			val += x5y3 * lanczosFactor;
			sum += lanczosFactor;
			lanczosFactor = LanczosFilter(x6) * LanczosFilter(y);
			val += x6y3 * lanczosFactor;
			sum += lanczosFactor;

			lanczosFactor = LanczosFilter(x1) * LanczosFilter(y4);
			val += x1y4 * lanczosFactor;
			sum += lanczosFactor;
			lanczosFactor = LanczosFilter(x2) * LanczosFilter(y4);
			val += x2y4 * lanczosFactor;
			sum += lanczosFactor;
			lanczosFactor = LanczosFilter(x) * LanczosFilter(y4);
			val += x3y4 * lanczosFactor;
			sum += lanczosFactor;
			lanczosFactor = LanczosFilter(x4) * LanczosFilter(y4);
			val += x4y4 * lanczosFactor;
			sum += lanczosFactor;
			lanczosFactor = LanczosFilter(x5) * LanczosFilter(y4);
			val += x5y4 * lanczosFactor;
			sum += lanczosFactor;
			lanczosFactor = LanczosFilter(x6) * LanczosFilter(y4);
			val += x6y4 * lanczosFactor;
			sum += lanczosFactor;

			lanczosFactor = LanczosFilter(x1) * LanczosFilter(y5);
			val += x1y5 * lanczosFactor;
			sum += lanczosFactor;
			lanczosFactor = LanczosFilter(x2) * LanczosFilter(y5);
			val += x2y5 * lanczosFactor;
			sum += lanczosFactor;
			lanczosFactor = LanczosFilter(x) * LanczosFilter(y5);
			val += x3y5 * lanczosFactor;
			sum += lanczosFactor;
			lanczosFactor = LanczosFilter(x4) * LanczosFilter(y5);
			val += x4y5 * lanczosFactor;
			sum += lanczosFactor;
			lanczosFactor = LanczosFilter(x5) * LanczosFilter(y5);
			val += x5y5 * lanczosFactor;
			sum += lanczosFactor;
			lanczosFactor = LanczosFilter(x6) * LanczosFilter(y5);
			val += x6y5 * lanczosFactor;
			sum += lanczosFactor;

			lanczosFactor = LanczosFilter(x1) * LanczosFilter(y6);
			val += x1y6 * lanczosFactor;
			sum += lanczosFactor;
			lanczosFactor = LanczosFilter(x2) * LanczosFilter(y6);
			val += x2y6 * lanczosFactor;
			sum += lanczosFactor;
			lanczosFactor = LanczosFilter(x) * LanczosFilter(y6);
			val += x3y6 * lanczosFactor;
			sum += lanczosFactor;
			lanczosFactor = LanczosFilter(x4) * LanczosFilter(y6);
			val += x4y6 * lanczosFactor;
			sum += lanczosFactor;
			lanczosFactor = LanczosFilter(x5) * LanczosFilter(y6);
			val += x5y6 * lanczosFactor;
			sum += lanczosFactor;
			lanczosFactor = LanczosFilter(x6) * LanczosFilter(y6);
			val += x6y6 * lanczosFactor;
			sum += lanczosFactor;

			return Clamp(val / sum);
		}

		/// <summary>
		/// Interpolates the specified values using Lanczos (3x3) sampling.
		/// </summary>
		/// <param name="x1y1">The top-left pixel at (-2, -2).</param>
		/// <param name="x2y1">x2y1</param>
		/// <param name="x3y1">x3y1</param>
		/// <param name="x4y1">x4y1</param>
		/// <param name="x5y1">x5y1</param>
		/// <param name="x6y1">x6y1</param>
		/// <param name="x1y2">x1y2</param>
		/// <param name="x2y2">x2y2</param>
		/// <param name="x3y2">x3y2</param>
		/// <param name="x4y2">x4y2</param>
		/// <param name="x5y2">x5y2</param>
		/// <param name="x6y2">x6y2</param>
		/// <param name="x1y3">x1y3</param>
		/// <param name="x2y3">x2y3</param>
		/// <param name="x3y3">x3y3</param>
		/// <param name="x4y3">x4y3</param>
		/// <param name="x5y3">x5y3</param>
		/// <param name="x6y3">x6y3</param>
		/// <param name="x1y4">x1y4</param>
		/// <param name="x2y4">x2y4</param>
		/// <param name="x3y4">x3y4</param>
		/// <param name="x4y4">x4y4</param>
		/// <param name="x5y4">x5y4</param>
		/// <param name="x6y4">x6y4</param>
		/// <param name="x1y5">x1y5</param>
		/// <param name="x2y5">x2y5</param>
		/// <param name="x3y5">x3y5</param>
		/// <param name="x4y5">x4y5</param>
		/// <param name="x5y5">x5y5</param>
		/// <param name="x6y5">x6y5</param>
		/// <param name="x1y6">x1y6</param>
		/// <param name="x2y6">x2y6</param>
		/// <param name="x3y6">x3y6</param>
		/// <param name="x4y6">x4y6</param>
		/// <param name="x5y6">x5y6</param>
		/// <param name="x6y6">The bottom-right pixel at (3, 3).</param>
		/// <param name="x">A value from 0 to 1 that reflects the X-coordinate of the output pixel relative to the middle pixels.</param>
		/// <param name="y">A value from 0 to 1 that reflects the Y-coordinate of the output pixel relative to the middle pixels.</param>
		public static BgraColor InterpolateLanczos(BgraColor x1y1, BgraColor x2y1, BgraColor x3y1, BgraColor x4y1, BgraColor x5y1, BgraColor x6y1,
											BgraColor x1y2, BgraColor x2y2, BgraColor x3y2, BgraColor x4y2, BgraColor x5y2, BgraColor x6y2,
											BgraColor x1y3, BgraColor x2y3, BgraColor x3y3, BgraColor x4y3, BgraColor x5y3, BgraColor x6y3,
											BgraColor x1y4, BgraColor x2y4, BgraColor x3y4, BgraColor x4y4, BgraColor x5y4, BgraColor x6y4,
											BgraColor x1y5, BgraColor x2y5, BgraColor x3y5, BgraColor x4y5, BgraColor x5y5, BgraColor x6y5,
											BgraColor x1y6, BgraColor x2y6, BgraColor x3y6, BgraColor x4y6, BgraColor x5y6, BgraColor x6y6, double x, double y) {
			return new BgraColor(InterpolateLanczos(x1y1.A, x2y1.A, x3y1.A, x4y1.A, x5y1.A, x6y1.A,
													x1y2.A, x2y2.A, x3y2.A, x4y2.A, x5y2.A, x6y2.A,
													x1y3.A, x2y3.A, x3y3.A, x4y3.A, x5y3.A, x6y3.A,
													x1y4.A, x2y4.A, x3y4.A, x4y4.A, x5y4.A, x6y4.A,
													x1y5.A, x2y5.A, x3y5.A, x4y5.A, x5y5.A, x6y5.A,
													x1y6.A, x2y6.A, x3y6.A, x4y6.A, x5y6.A, x6y6.A, x, y),
								 InterpolateLanczos(x1y1.R, x2y1.R, x3y1.R, x4y1.R, x5y1.R, x6y1.R,
													x1y2.R, x2y2.R, x3y2.R, x4y2.R, x5y2.R, x6y2.R,
													x1y3.R, x2y3.R, x3y3.R, x4y3.R, x5y3.R, x6y3.R,
													x1y4.R, x2y4.R, x3y4.R, x4y4.R, x5y4.R, x6y4.R,
													x1y5.R, x2y5.R, x3y5.R, x4y5.R, x5y5.R, x6y5.R,
													x1y6.R, x2y6.R, x3y6.R, x4y6.R, x5y6.R, x6y6.R, x, y),
								 InterpolateLanczos(x1y1.G, x2y1.G, x3y1.G, x4y1.G, x5y1.G, x6y1.G,
													x1y2.G, x2y2.G, x3y2.G, x4y2.G, x5y2.G, x6y2.G,
													x1y3.G, x2y3.G, x3y3.G, x4y3.G, x5y3.G, x6y3.G,
													x1y4.G, x2y4.G, x3y4.G, x4y4.G, x5y4.G, x6y4.G,
													x1y5.G, x2y5.G, x3y5.G, x4y5.G, x5y5.G, x6y5.G,
													x1y6.G, x2y6.G, x3y6.G, x4y6.G, x5y6.G, x6y6.G, x, y),
								 InterpolateLanczos(x1y1.B, x2y1.B, x3y1.B, x4y1.B, x5y1.B, x6y1.B,
													x1y2.B, x2y2.B, x3y2.B, x4y2.B, x5y2.B, x6y2.B,
													x1y3.B, x2y3.B, x3y3.B, x4y3.B, x5y3.B, x6y3.B,
													x1y4.B, x2y4.B, x3y4.B, x4y4.B, x5y4.B, x6y4.B,
													x1y5.B, x2y5.B, x3y5.B, x4y5.B, x5y5.B, x6y5.B,
													x1y6.B, x2y6.B, x3y6.B, x4y6.B, x5y6.B, x6y6.B, x, y));
		}

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		private static double LanczosFilter(double t) {
			if (t < 0.0)
				t = -t;
			if (t > 3.0)
				return 0.0;
			else
				return Maths.Sinc(t) * Maths.Sinc(t * 0.333333333333333333333);
		}

		/// <summary>
		/// Adds the respective components of the colors.
		/// </summary>
		private static BgraColor Add(int i, BgraColor left, BgraColor right) {
			return new BgraColor(Clamp(left.A + right.A), Clamp(left.R + right.R), Clamp(left.G + right.G), Clamp(left.B + right.B));
		}

		/// <summary>
		/// Adds the respective components of the colors.
		/// </summary>
		/// <param name="left">The first color.</param>
		/// <param name="right">The second color.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static BgraColor Add(this BgraColor left, BgraColor right) {
			return new BgraColor(Clamp(left.A + right.A), Clamp(left.R + right.R), Clamp(left.G + right.G), Clamp(left.B + right.B));
		}

		/// <summary>
		/// Subtracts the specified color from the given color.
		/// </summary>
		/// <param name="left">The first color.</param>
		/// <param name="right">The color to subtract from the first.</param>
		/// <param name="subtractAlpha">Whether to subtract the alpha component as well.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static BgraColor Subtract(this BgraColor left, BgraColor right, bool subtractAlpha = false) {
			return new BgraColor(subtractAlpha ? Clamp(left.A - right.A) : left.A, Clamp(left.R - right.R), Clamp(left.G - right.G), Clamp(left.B - right.B));
		}

		/// <summary>
		/// Adds the respective components of the colors.
		/// </summary>
		/// <param name="left">The first color.</param>
		/// <param name="right">The second color.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Color Add(this Color left, Color right) {
			return Color.FromArgb(Clamp(left.A + right.A), Clamp(left.R + right.R), Clamp(left.G + right.G), Clamp(left.B + right.B));
		}

		/// <summary>
		/// Subtracts the specified color from the given color.
		/// </summary>
		/// <param name="left">The first color.</param>
		/// <param name="right">The color to subtract from the first.</param>
		/// <param name="subtractAlpha">Whether to subtract the alpha component as well.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Color Subtract(this Color left, Color right, bool subtractAlpha = false) {
			return Color.FromArgb(subtractAlpha ? Clamp(left.A - right.A) : left.A, Clamp(left.R - right.R), Clamp(left.G - right.G), Clamp(left.B - right.B));
		}

		/// <summary>
		/// Converts the specified image to a grayscale.
		/// </summary>
		/// <param name="image">The image whose colors to convert to grayscale.</param>
		/// <param name="premultiplyAlpha">If true, the alpha channel is premultiplied to the color channels.</param>
		public static Bitmap ToGrayscale(this Bitmap image, bool premultiplyAlpha = false) {
			using (PixelWorker source = PixelWorker.FromImage(image, false, false, ImageParameterAction.RemoveReference)) {
				source.ConvertToGrayscale(premultiplyAlpha);
				return source.ToBitmap();
			}
		}

		/// <summary>
		/// Converts the specified image to a 24-bit BGR.
		/// </summary>
		/// <param name="image">The image whose colors to convert to 24-bit.</param>
		/// <param name="premultiplyAlpha">If true, the alpha channel is premultiplied to the color channels.</param>
		public static Bitmap To24Bit(this Bitmap image, bool premultiplyAlpha = false) {
			using (PixelWorker source = PixelWorker.FromImage(image, false, false, ImageParameterAction.RemoveReference)) {
				source.ConvertTo24Bit(premultiplyAlpha);
				return source.ToBitmap();
			}
		}

		/// <summary>
		/// Converts the specified image to a 32-bit BGRA.
		/// </summary>
		/// <param name="image">The image whose colors to convert to 32-bit.</param>
		public static Bitmap To32Bit(this Bitmap image) {
			using (PixelWorker source = PixelWorker.FromImage(image, false, false, ImageParameterAction.RemoveReference)) {
				source.ConvertTo32Bit();
				return source.ToBitmap();
			}
		}

		/// <summary>
		/// Inverts the colors of the specified image.
		/// </summary>
		/// <param name="image">The image whose colors to invert.</param>
		/// <param name="ignoreAlpha">If true, alpha is left intact.</param>
		public static void Invert(this Bitmap image, bool ignoreAlpha = true) {
			if (image == null)
				return;
			using (PixelWorker source = PixelWorker.FromImage(image, false, true, ImageParameterAction.RemoveReference))
				Invert(source, ignoreAlpha);
		}

		/// <summary>
		/// Inverts the colors of the specified image.
		/// </summary>
		/// <param name="source">The image whose colors to invert.</param>
		/// <param name="ignoreAlpha">If true, alpha is left intact.</param>
		public static void Invert(this PixelWorker source, bool ignoreAlpha = true) {
			if (source == null)
				return;
			else if (source.ComponentCount == 4 && ignoreAlpha) {
				if (source.Buffer == null) {
					unsafe
					{
						ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
							if (i % 4 != 3) {
								byte* index = source.Scan0 + i;
								*index = (byte) ~*index;
							}
						}, ParallelCutoff);
					}
				} else {
					ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
						if (i % 4 != 3)
							source.Buffer[i] = (byte) (~source.Buffer[i]);
					}, ParallelCutoff);
				}
			} else {
				if (source.Buffer == null) {
					unsafe
					{
						ParallelLoop.For(source.Scan0, source.PixelComponentCount, 1L, i => {
							byte* index = (byte*) i;
							*index = (byte) (~*index);
						}, ParallelCutoff);
					}
				} else {
					ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
						source.Buffer[i] = (byte) (~source.Buffer[i]);
					}, ParallelCutoff);
				}
			}
		}

		/// <summary>
		/// Converts the specified grayscale color to RGB.
		/// </summary>
		/// <param name="grayscale">The grayscale color to convert.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Color ToRGB(byte grayscale) {
			return Color.FromArgb(grayscale, grayscale, grayscale);
		}

		/// <summary>
		/// Converts the specified grayscale color to BGR.
		/// </summary>
		/// <param name="grayscale">The grayscale color to convert.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static BgraColor ToBGR(byte grayscale) {
			return new BgraColor(grayscale, grayscale, grayscale);
		}

		/// <summary>
		/// Converts the specified grayscale color to ARGB.
		/// </summary>
		/// <param name="grayscale">The grayscale color to convert.</param>
		/// <param name="alpha">The alpha component of the output.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Color ToARGB(byte grayscale, byte alpha) {
			return Color.FromArgb(alpha, grayscale, grayscale, grayscale);
		}

		/// <summary>
		/// Converts the specified grayscale color to BGRA.
		/// </summary>
		/// <param name="grayscale">The grayscale color to convert.</param>
		/// <param name="alpha">The alpha component of the output.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static BgraColor ToBGRA(byte grayscale, byte alpha) {
			return new BgraColor(alpha, grayscale, grayscale, grayscale);
		}

		/// <summary>
		/// Converts the specified color to a grayscale in the range [0, 256).
		/// </summary>
		/// <param name="color">The color to convert to grayscale.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static float ToGrayscale(this Color color) {
			return (0.299f * color.R + 0.587f * color.G + 0.114f * color.B) * color.A * 0.00392156863f;
		}

		/// <summary>
		/// Converts the specified color to a grayscale in the range [0, 256).
		/// </summary>
		/// <param name="color">The color to convert to grayscale.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static float ToGrayscale(this BgraColor color) {
			return (0.299f * color.R + 0.587f * color.G + 0.114f * color.B) * color.A * 0.00392156863f;
		}

		/// <summary>
		/// Converts the specified color to a grayscale in the range [0, 256).
		/// </summary>
		/// <param name="color">The color to convert to grayscale.</param>
		/// <param name="alpha">The alpha value to use instead of the color's.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static byte ToGrayscale(this Color color, byte alpha) {
			return (byte) ((0.299f * color.R + 0.587f * color.G + 0.114f * color.B) * alpha * 0.00392156863f);
		}

		/// <summary>
		/// Converts the specified color to a grayscale in the range [0, 255].
		/// </summary>
		/// <param name="color">The color to convert to grayscale.</param>
		/// <param name="alpha">The alpha value to use instead of the color's.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static byte ToGrayscale(this BgraColor color, byte alpha) {
			return (byte) ((0.299f * color.R + 0.587f * color.G + 0.114f * color.B) * alpha * 0.00392156863f);
		}

		/// <summary>
		/// Converts the specified color to a grayscale color.
		/// </summary>
		/// <param name="color">The color to convert to grayscale.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Color ToGrayscaleColor(this Color color) {
			byte value = (byte) (0.299f * color.R + 0.587f * color.G + 0.114f * color.B);
			return Color.FromArgb(color.A, value, value, value);
		}

		/// <summary>
		/// Converts the specified color to a grayscale color.
		/// </summary>
		/// <param name="color">The color to convert to grayscale.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static BgraColor ToGrayscaleColor(this BgraColor color) {
			byte value = (byte) (0.299f * color.R + 0.587f * color.G + 0.114f * color.B);
			return new BgraColor(color.A, value, value, value);
		}

		/// <summary>
		/// Converts the specified color to a grayscale in the range [0, 256).
		/// </summary>
		/// <param name="color">The color to convert to grayscale.</param>
		/// <param name="opacity">The opacity to multiply to the color.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static float ToGrayscale(this Color color, float opacity) {
			return (0.299f * color.R + 0.587f * color.G + 0.114f * color.B) * color.A * 0.00392156863f * opacity;
		}

		/// <summary>
		/// Converts the specified color to a grayscale in the range [0, 256).
		/// </summary>
		/// <param name="color">The color to convert to grayscale.</param>
		/// <param name="opacity">The opacity to multiply to the color.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static float ToGrayscale(this BgraColor color, float opacity) {
			return (0.299f * color.R + 0.587f * color.G + 0.114f * color.B) * color.A * 0.00392156863f * opacity;
		}

		/// <summary>
		/// Converts the specified color to a grayscale in the range [0, 256).
		/// </summary>
		/// <param name="a">The alpha component.</param>
		/// <param name="r">The red component.</param>
		/// <param name="g">The green component.</param>
		/// <param name="b">The blue component.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static float ToGrayscale(byte a, byte r, byte g, byte b) {
			return (0.299f * r + 0.587f * g + 0.114f * b) * a * 0.00392156863f;
		}

		/// <summary>
		/// Converts the specified color to a grayscale in the range [0, 256).
		/// </summary>
		/// <param name="r">The red component.</param>
		/// <param name="g">The green component.</param>
		/// <param name="b">The blue component.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static float ToGrayscale(byte r, byte g, byte b) {
			return 0.299f * r + 0.587f * g + 0.114f * b;
		}

		/// <summary>
		/// Premultiplies the alpha components of the image.
		/// </summary>
		/// <param name="image">The image whose color values to premultply.</param>
		public static void PremultiplyAlpha(this Bitmap image) {
			using (PixelWorker source = PixelWorker.FromImage(image, false, true, ImageParameterAction.RemoveReference))
				source.PremultiplyAlpha();
		}

		/// <summary>
		/// Isolates and extracts the specified color channel from the image.
		/// </summary>
		/// <param name="image">The image whose color channel to extract.</param>
		/// <param name="channel">0 for Blue, 1 for Green, 2 for Red, 3 for Alpha.</param>
		public static PixelWorker ExtractChannel(this Bitmap image, int channel) {
			using (PixelWorker source = PixelWorker.FromImage(image, false, false, ImageParameterAction.RemoveReference))
				return source.ExtractChannel(channel);
		}

		/// <summary>
		/// Applies a median sharpen filter to the specified image and returns the result.
		/// </summary>
		/// <param name="image">The image to use for processing (will not be written to).</param>
		/// <param name="radius">The median sharpen filter radius.</param>
		public static Bitmap MedianEnhance(this Bitmap image, int radius) {
			if (radius <= 0)
				return image.FastCopy();
			using (PixelWorker worker = PixelWorker.FromImage(image, false, true))
				return MedianEnhance(worker, radius);
		}

		/// <summary>
		/// Applies a median sharpen filter to the specified image and returns the result.
		/// </summary>
		/// <param name="source">The image to use for processing (will not be written to).</param>
		/// <param name="radius">The median sharpen filter radius.</param>
		public static Bitmap MedianEnhance(this PixelWorker source, int radius) {
			if (radius <= 0)
				return source.ToBitmap();
			Bitmap image = MedianFilter(source, radius);
			using (PixelWorker worker = PixelWorker.FromImage(image, false, true))
				Transition(worker, source, 2f);
			return image;
		}

		/// <summary>
		/// Applies a median sharpen filter to the specified image and returns the result.
		/// </summary>
		/// <param name="image">The image to use for processing (will not be written to).</param>
		/// <param name="radius">The median sharpen filter radius.</param>
		public static Bitmap MedianEnhance(this Bitmap image, float radius) {
			if (radius <= 0f)
				return image.FastCopy();
			using (PixelWorker worker = PixelWorker.FromImage(image, false, true))
				return MedianEnhance(worker, radius);
		}

		/// <summary>
		/// Applies a median sharpen filter to the specified image and returns the result.
		/// </summary>
		/// <param name="source">The image to use for processing (will not be written to).</param>
		/// <param name="radius">The median sharpen filter radius.</param>
		public static Bitmap MedianEnhance(this PixelWorker source, float radius) {
			if (radius <= 0f)
				return source.ToBitmap();
			radius = Math.Min(radius, Math.Max(source.Width, source.Height));
			float fraction = radius % 1f;
			if (fraction <= float.Epsilon)
				return MedianEnhance(source, (int) radius);
			else if (radius <= 1f) {
				Bitmap result = MedianEnhance(source, 1);
				using (PixelWorker overlay = PixelWorker.FromImage(result, false, true, ImageParameterAction.RemoveReference))
					Transition(overlay, source, 1f - radius);
				return result;
			} else {
				Bitmap result = MedianEnhance(source, (int) radius);
				using (Bitmap overlay = MedianEnhance(source, (int) radius + 1))
					Transition(result, overlay, fraction);
				return result;
			}
		}

		/// <summary>
		/// Applies a median filter to the specified image and returns the result.
		/// </summary>
		/// <param name="image">The image to use for processing (will not be written to).</param>
		/// <param name="radius">The median filter radius. Integer values are faster to process.</param>
		public static Bitmap MedianFilter(this Bitmap image, float radius) {
			if (radius <= 0f)
				return image.FastCopy();
			using (PixelWorker source = PixelWorker.FromImage(image, false, false, ImageParameterAction.RemoveReference))
				return MedianFilter(source, radius);
		}

		/// <summary>
		/// Applies a median filter to the specified image and returns a result.
		/// </summary>
		/// <param name="source">The image to use for processing (will not be written to).</param>
		/// <param name="radius">The median filter radius. Integer values are faster to process.</param>
		public static Bitmap MedianFilter(this PixelWorker source, float radius) {
			if (radius <= 0f)
				return source.ToBitmap();
			radius = Math.Min(radius, Math.Max(source.Width, source.Height));
			float fraction = radius % 1f;
			if (fraction <= float.Epsilon)
				return MedianFilter(source, (int) radius);
			else if (radius <= 1f) {
				Bitmap result = MedianFilter(source, 1);
				using (PixelWorker overlay = PixelWorker.FromImage(result, false, true, ImageParameterAction.RemoveReference))
					Transition(overlay, source, 1f - radius);
				return result;
			} else {
				Bitmap result = MedianFilter(source, (int) radius);
				using (Bitmap overlay = MedianFilter(source, (int) radius + 1))
					Transition(result, overlay, fraction);
				return result;
			}
		}

		/// <summary>
		/// Applies a median filter to the specified image and returns a result.
		/// </summary>
		/// <param name="image">The image to use for processing (will not be written to).</param>
		/// <param name="radius">The median filter radius.</param>
		public static Bitmap MedianFilter(this Bitmap image, int radius) {
			if (radius <= 0)
				return image.FastCopy();
			using (PixelWorker source = PixelWorker.FromImage(image, false, false, ImageParameterAction.RemoveReference))
				return MedianFilter(source, radius);
		}

		/// <summary>
		/// Applies a median filter to the specified image and returns a result.
		/// </summary>
		/// <param name="source">The image to use for processing (will not be written to).</param>
		/// <param name="radius">The median filter radius.</param>
		public static Bitmap MedianFilter(this PixelWorker source, int radius) {
			Bitmap resultant;
			if (radius > 0) {
				radius = Math.Min(radius, Math.Max(source.Width, source.Height));
				resultant = new Bitmap(source.Width, source.Height, source.Format);
				int size = radius + radius + 1;
				size *= size;
				unsafe
				{
					using (PixelWorker wrapper = PixelWorker.FromImage(resultant, false, true, ImageParameterAction.RemoveReference)) {
						ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
							byte* neighbourPixels = stackalloc byte[size];
							int count = 0;
							int componentCount = source.ComponentCount;
							int componentIndex = i % componentCount;
							int pixelIndex = i / componentCount;
							int x = pixelIndex % source.Width;
							int y = pixelIndex / source.Width;
							int filterX, offset, tempX;
							for (int filterY = -radius; filterY <= radius; filterY++) {
								offset = y + filterY;
								if (offset >= 0 && offset < source.Height) {
									offset = offset * source.Width + x;
									for (filterX = -radius; filterX <= radius; filterX++) {
										tempX = x + filterX;
										if (tempX >= 0 && tempX < source.Width) {
											neighbourPixels[count] = source[(offset + filterX) * componentCount + componentIndex];
											count++;
										}
									}
								}
							}
							wrapper[i] = GetMedianInPlace(neighbourPixels, count);
						});
					}
				}
			} else
				resultant = source.ToBitmap();
			return resultant;
		}

		/// <summary>
		/// Blurs the specified image using a blazing fast in-place box blur approximation.
		/// </summary>
		/// <param name="image">The image to blur.</param>
		/// <param name="radius">The blur radius.</param>
		/// <param name="passes">The number of passes to apply. At least 2 are recommended for quality.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void BoxBlur(this Bitmap image, float radius, int passes = 2) {
			byte[] buffer = null;
			BoxBlur(image, radius, passes, ref buffer);
		}

		/// <summary>
		/// Blurs the specified image using a blazing fast in-place box blur approximation.
		/// </summary>
		/// <param name="image">The image to blur.</param>
		/// <param name="radius">The blur radius.</param>
		/// <param name="passes">The number of passes to apply. At least 2 are recommended for quality.</param>
		/// <param name="buffer">An optional buffer the size of PixelComponentCount of the image (or more) to use as working memory.</param>
		public static void BoxBlur(this Bitmap image, float radius, int passes, ref byte[] buffer) {
			if (radius <= 0f || passes <= 0)
				return;
			using (PixelWorker worker = PixelWorker.FromImage(image, false, true))
				BoxBlur(worker, radius, passes, ref buffer);
		}

		/// <summary>
		/// Blurs the specified image using a blazing fast in-place box blur approximation.
		/// </summary>
		/// <param name="source">The image to blur.</param>
		/// <param name="radius">The blur radius.</param>
		/// <param name="passes">The number of passes to apply. At least 2 are recommended for quality.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void BoxBlur(this PixelWorker source, float radius, int passes = 2) {
			byte[] buffer = null;
			BoxBlur(source, radius, passes, ref buffer);
		}

		/// <summary>
		/// Blurs the specified image using a blazing fast in-place box blur approximation.
		/// </summary>
		/// <param name="source">The image to blur.</param>
		/// <param name="radius">The blur radius.</param>
		/// <param name="passes">The number of passes to apply. At least 2 are recommended for quality.</param>
		/// <param name="buffer">An optional buffer the size of PixelComponentCount of the image (or more) to use as working memory.</param>
		public static void BoxBlur(this PixelWorker source, float radius, int passes, ref byte[] buffer) {
			if (radius <= 0f || passes <= 0)
				return;
			float fraction = radius % 1f;
			if (fraction <= float.Epsilon)
				BoxBlur(source, (int) radius, passes, ref buffer);
			else {
				using (PixelWorker wrapper = new PixelWorker(source.ToByteArray(true), source.Width, source.Height, false)) {
					if (buffer == null || buffer.Length < source.PixelComponentCount)
						buffer = new byte[source.PixelComponentCount];
					BoxBlur(source, (int) radius, passes, ref buffer);
					BoxBlur(wrapper, (int) radius + 1, passes, ref buffer);
					Transition(source, wrapper, fraction);
				}
			}
		}

		/// <summary>
		/// Blurs the specified image using a blazing fast in-place box blur approximation.
		/// </summary>
		/// <param name="image">The image to blur.</param>
		/// <param name="radius">The blur radius.</param>
		/// <param name="passes">The number of passes to apply. At least 2 are recommended for quality.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void BoxBlur(this Bitmap image, int radius, int passes = 2) {
			byte[] buffer = null;
			BoxBlur(image, radius, passes, ref buffer);
		}

		/// <summary>
		/// Blurs the specified image using a blazing fast in-place box blur approximation.
		/// </summary>
		/// <param name="image">The image to blur.</param>
		/// <param name="radius">The blur radius.</param>
		/// <param name="passes">The number of passes to apply. At least 2 are recommended for quality.</param>
		/// <param name="buffer">An optional buffer the size of PixelComponentCount of the image (or more) to use as working memory.</param>
		public static void BoxBlur(this Bitmap image, int radius, int passes, ref byte[] buffer) {
			if (radius <= 0 || passes <= 0)
				return;
			using (PixelWorker worker = PixelWorker.FromImage(image, false, true))
				BoxBlur(worker, radius, passes, ref buffer);
		}

		/// <summary>
		/// Blurs the specified image using a blazing fast in-place box blur approximation.
		/// </summary>
		/// <param name="source">The image to blur.</param>
		/// <param name="radius">The blur radius.</param>
		/// <param name="passes">The number of passes to apply. At least 2 are recommended for quality.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void BoxBlur(this PixelWorker source, int radius, int passes = 2) {
			byte[] buffer = null;
			BoxBlur(source, radius, passes, ref buffer);
		}

		/// <summary>
		/// Blurs the specified image using a blazing fast in-place box blur approximation.
		/// </summary>
		/// <param name="source">The image to blur.</param>
		/// <param name="radius">The blur radius.</param>
		/// <param name="passes">The number of passes to apply. At least 2 are recommended for quality.</param>
		/// <param name="buffer">An optional buffer the size of PixelComponentCount of the image (or more) to use as working memory.</param>
		public static void BoxBlur(this PixelWorker source, int radius, int passes, ref byte[] buffer) {
			if (radius <= 0 || passes <= 0)
				return;
			int cc = source.ComponentCount, width = source.Width, height = source.Height, c;
			int max = Math.Min(width, height) / 2;
			if (max <= 1)
				return;
			if (radius > max)
				radius = max;
			float mult = 1f / (radius + radius + 1);
			if (buffer == null || buffer.Length < source.PixelComponentCount)
				buffer = new byte[source.PixelComponentCount];
			byte[] container = buffer;
			for (int pass = 0; pass < passes; pass++) {
				for (c = 0; c < cc; c++) {
					ParallelLoop.For(0, height, i => {
						int ti = i * width;
						int li = ti;
						int ri = ti + radius;
						byte fv = source[ti * cc + c];
						byte lv = source[(ti + width - 1) * cc + c];
						int val = (radius + 1) * fv;
						int j;
						for (j = 0; j < radius; j++)
							val += source[(ti + j) * cc + c];
						for (j = 0; j <= radius; j++) {
							val += source[ri * cc + c] - fv;
							ri++;
							container[ti * cc + c] = Clamp(val * mult);
							ti++;
						}
						int diff = width - radius;
						for (j = radius + 1; j < diff; j++) {
							val += source[ri * cc + c] - source[li * cc + c];
							ri++;
							li++;
							container[ti * cc + c] = Clamp(val * mult);
							ti++;
						}
						for (j = diff; j < width; j++) {
							val += lv - source[li * cc + c];
							li++;
							container[ti * cc + c] = Clamp(val * mult);
							ti++;
						}
					});
					ParallelLoop.For(0, width, i => {
						int ti = i;
						int li = ti;
						int ri = ti + radius * width;
						byte fv = container[ti * cc + c];
						byte lv = container[(ti + width * (height - 1)) * cc + c];
						int val = (radius + 1) * fv;
						int j;
						for (j = 0; j < radius; j++)
							val += container[(ti + j * width) * cc + c];
						for (j = 0; j <= radius; j++) {
							val += container[ri * cc + c] - fv;
							source[ti * cc + c] = Clamp(val * mult);
							ri += width;
							ti += width;
						}
						int diff = height - radius;
						for (j = radius + 1; j < diff; j++) {
							val += container[ri * cc + c] - container[li * cc + c];
							source[ti * cc + c] = Clamp(val * mult);
							li += width;
							ri += width;
							ti += width;
						}
						for (j = diff; j < height; j++) {
							val += lv - container[li * cc + c];
							source[ti * cc + c] = Clamp(val * mult);
							li += width;
							ti += width;
						}
					});
				}
			}
		}

		/// <summary>
		/// Blurs the alpha channel of specified image using a blazing fast in-place box blur approximation.
		/// </summary>
		/// <param name="image">The image to blur.</param>
		/// <param name="radius">The blur radius.</param>
		/// <param name="passes">The number of passes to apply. At least 2 are recommended for quality.</param>
		/// <param name="multiplier">The multiplier to multiply the alpha channel with.</param>
		/// <param name="maxAlpha">The maximum alpha cap.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void BoxBlurAlpha(this Bitmap image, float radius, int passes = 2, float multiplier = 1f, byte maxAlpha = 255) {
			byte[] buffer = null;
			BoxBlurAlpha(image, radius, passes, multiplier, maxAlpha, ref buffer);
		}

		/// <summary>
		/// Blurs the alpha channel of specified image using a blazing fast in-place box blur approximation.
		/// </summary>
		/// <param name="image">The image to blur.</param>
		/// <param name="radius">The blur radius.</param>
		/// <param name="passes">The number of passes to apply. At least 2 are recommended for quality.</param>
		/// <param name="multiplier">The multiplier to multiply the alpha channel with.</param>
		/// <param name="maxAlpha">The maximum alpha cap.</param>
		/// <param name="buffer">An optional buffer the size of PixelCount of the image (or more) to use as working memory.</param>
		public static void BoxBlurAlpha(this Bitmap image, float radius, int passes, float multiplier, byte maxAlpha, ref byte[] buffer) {
			if (radius <= 0f || passes <= 0 || !(image.PixelFormat == PixelFormat.Format32bppArgb || image.PixelFormat == PixelFormat.Format32bppPArgb))
				return;
			using (PixelWorker worker = PixelWorker.FromImage(image, false, true))
				BoxBlurAlpha(worker, radius, passes, multiplier, maxAlpha, ref buffer);
		}

		/// <summary>
		/// Blurs the alpha channel of specified image using a blazing fast in-place box blur approximation.
		/// </summary>
		/// <param name="source">The image to blur.</param>
		/// <param name="radius">The blur radius.</param>
		/// <param name="passes">The number of passes to apply. At least 2 are recommended for quality.</param>
		/// <param name="multiplier">The multiplier to multiply the alpha channel with.</param>
		/// <param name="maxAlpha">The maximum alpha cap.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void BoxBlurAlpha(this PixelWorker source, float radius, int passes = 2, float multiplier = 1f, byte maxAlpha = 255) {
			byte[] buffer = null;
			BoxBlurAlpha(source, radius, passes, multiplier, maxAlpha, ref buffer);
		}

		/// <summary>
		/// Blurs the alpha channel of specified image using a blazing fast in-place box blur approximation.
		/// </summary>
		/// <param name="source">The image to blur.</param>
		/// <param name="radius">The blur radius.</param>
		/// <param name="passes">The number of passes to apply. At least 2 are recommended for quality.</param>
		/// <param name="multiplier">The multiplier to multiply the alpha channel with.</param>
		/// <param name="maxAlpha">The maximum alpha cap.</param>
		/// <param name="buffer">An optional buffer the size of PixelCount of the image (or more) to use as working memory.</param>
		public static void BoxBlurAlpha(this PixelWorker source, float radius, int passes, float multiplier, byte maxAlpha, ref byte[] buffer) {
			if (radius <= 0f || passes <= 0 || source.ComponentCount != 4)
				return;
			float fraction = radius % 1f;
			if (fraction <= float.Epsilon)
				BoxBlurAlpha(source, (int) radius, passes, multiplier, maxAlpha, ref buffer);
			else {
				using (PixelWorker wrapper = new PixelWorker(source.ToByteArray(true), source.Width, source.Height, false)) {
					if (buffer == null || buffer.Length < source.PixelCount)
						buffer = new byte[source.PixelCount];
					BoxBlurAlpha(source, (int) radius, passes, multiplier, maxAlpha, ref buffer);
					BoxBlurAlpha(wrapper, (int) radius + 1, passes, multiplier, maxAlpha, ref buffer);
					Transition(source, wrapper, fraction);
				}
			}
		}

		/// <summary>
		/// Blurs the alpha channel of specified image using a blazing fast in-place box blur approximation.
		/// </summary>
		/// <param name="image">The image to blur.</param>
		/// <param name="radius">The blur radius.</param>
		/// <param name="passes">The number of passes to apply. At least 2 are recommended for quality.</param>
		/// <param name="multiplier">The multiplier to multiply the alpha channel with.</param>
		/// <param name="maxAlpha">The maximum alpha cap.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void BoxBlurAlpha(this Bitmap image, int radius, int passes = 2, float multiplier = 1f, byte maxAlpha = 255) {
			byte[] buffer = null;
			BoxBlurAlpha(image, radius, passes, multiplier, maxAlpha, ref buffer);
		}

		/// <summary>
		/// Blurs the alpha channel of specified image using a blazing fast in-place box blur approximation.
		/// </summary>
		/// <param name="image">The image to blur.</param>
		/// <param name="radius">The blur radius.</param>
		/// <param name="passes">The number of passes to apply. At least 2 are recommended for quality.</param>
		/// <param name="multiplier">The multiplier to multiply the alpha channel with.</param>
		/// <param name="maxAlpha">The maximum alpha cap.</param>
		/// <param name="buffer">An optional buffer the size of PixelCount of the image (or more) to use as working memory.</param>
		public static void BoxBlurAlpha(this Bitmap image, int radius, int passes, float multiplier, byte maxAlpha, ref byte[] buffer) {
			if (radius <= 0 || passes <= 0 || !(image.PixelFormat == PixelFormat.Format32bppArgb || image.PixelFormat == PixelFormat.Format32bppPArgb))
				return;
			using (PixelWorker worker = PixelWorker.FromImage(image, false, true))
				BoxBlurAlpha(worker, radius, passes, multiplier, maxAlpha, ref buffer, worker.Width, worker.Height);
		}

		/// <summary>
		/// Blurs the alpha channel of the specified image using a blazing fast in-place box blur approximation.
		/// </summary>
		/// <param name="source">The image to blur.</param>
		/// <param name="radius">The blur radius.</param>
		/// <param name="passes">The number of passes to apply. At least 2 are recommended for quality.</param>
		/// <param name="multiplier">The multiplier to multiply the alpha channel with.</param>
		/// <param name="maxAlpha">The maximum alpha cap.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void BoxBlurAlpha(this PixelWorker source, int radius, int passes = 2, float multiplier = 1f, byte maxAlpha = 255) {
			byte[] buffer = null;
			BoxBlurAlpha(source, radius, passes, multiplier, maxAlpha, ref buffer, source.Width, source.Height);
		}

		/// <summary>
		/// Blurs the alpha channel of the specified image using a blazing fast in-place box blur approximation.
		/// </summary>
		/// <param name="source">The image to blur.</param>
		/// <param name="radius">The blur radius.</param>
		/// <param name="passes">The number of passes to apply. At least 2 are recommended for quality.</param>
		/// <param name="multiplier">The multiplier to multiply the alpha channel with.</param>
		/// <param name="maxAlpha">The maximum alpha cap.</param>
		/// <param name="buffer">An optional buffer the size of PixelCount of the image (or more) to use as working memory.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void BoxBlurAlpha(this PixelWorker source, int radius, int passes, float multiplier, byte maxAlpha, ref byte[] buffer) {
			BoxBlurAlpha(source, radius, passes, multiplier, maxAlpha, ref buffer, source.Width, source.Height);
		}

		/// <summary>
		/// For internal use only. Blurs the alpha channel of the specified image using a blazing fast in-place box blur approximation.
		/// </summary>
		/// <param name="source">The image to blur.</param>
		/// <param name="radius">The blur radius.</param>
		/// <param name="passes">The number of passes to apply. At least 2 are recommended for quality.</param>
		/// <param name="multiplier">The multiplier to multiply the alpha channel with.</param>
		/// <param name="maxAlpha">The maximum alpha cap.</param>
		/// <param name="buffer">An optional buffer the size of PixelCount of the image (or more) to use as working memory.</param>
		/// <param name="blurWidth">The width of the image.</param>
		/// <param name="blurHeight">The height of image.</param>
		public static void BoxBlurAlpha(this PixelWorker source, int radius, int passes, float multiplier, byte maxAlpha, ref byte[] buffer, int blurWidth, int blurHeight) {
			if (radius <= 0 || passes <= 0 || source.ComponentCount != 4)
				return;
			int width = source.Width;
			int max = Math.Min(width, blurHeight) / 2;
			if (max <= 1)
				return;
			if (radius > max)
				radius = max;
			float mult = multiplier / (radius + radius + 1);
			if (buffer == null || buffer.Length < source.PixelCount)
				buffer = new byte[source.PixelCount];
			byte[] container = buffer;
			for (int pass = 0; pass < passes; pass++) {
				ParallelLoop.For(0, blurHeight, i => {
					int ti = i * width;
					int li = ti;
					int ri = ti + radius;
					byte fv = source[ti * 4 + 3];
					byte lv = source[(ti + blurWidth - 1) * 4 + 3];
					int val = (radius + 1) * fv;
					int j;
					for (j = 0; j < radius; j++)
						val += source[(ti + j) * 4 + 3];
					for (j = 0; j <= radius; j++) {
						val += source[ri * 4 + 3] - fv;
						ri++;
						container[ti] = (byte) Maths.Clamp(val * mult, 0f, maxAlpha);
						ti++;
					}
					int diff = blurWidth - radius;
					for (j = radius + 1; j < diff; j++) {
						val += source[ri * 4 + 3] - source[li * 4 + 3];
						ri++;
						li++;
						container[ti] = (byte) Maths.Clamp(val * mult, 0f, maxAlpha);
						ti++;
					}
					for (j = diff; j < blurWidth; j++) {
						val += lv - source[li * 4 + 3];
						li++;
						container[ti] = (byte) Maths.Clamp(val * mult, 0f, maxAlpha);
						ti++;
					}
				});
				ParallelLoop.For(0, blurWidth, i => {
					int ti = i;
					int li = ti;
					int ri = ti + radius * width;
					byte fv = container[ti];
					byte lv = container[ti + width * (blurHeight - 1)];
					int val = (radius + 1) * fv;
					int j;
					for (j = 0; j < radius; j++)
						val += container[ti + j * blurWidth];
					for (j = 0; j <= radius; j++) {
						val += container[ri] - fv;
						source[ti * 4 + 3] = (byte) Maths.Clamp(val * mult, 0f, maxAlpha);
						ri += width;
						ti += width;
					}
					int diff = blurHeight - radius;
					for (j = radius + 1; j < diff; j++) {
						val += container[ri] - container[li];
						source[ti * 4 + 3] = (byte) Maths.Clamp(val * mult, 0f, maxAlpha);
						li += width;
						ri += width;
						ti += width;
					}
					for (j = diff; j < blurHeight; j++) {
						val += lv - container[li];
						source[ti * 4 + 3] = (byte) Maths.Clamp(val * mult, 0f, maxAlpha);
						li += width;
						ti += width;
					}
				});
			}
		}

		/// <summary>
		/// Sharpens the specified image using a fast unsharp mask approximation.
		/// </summary>
		/// <param name="image">The image to sharpen.</param>
		/// <param name="radius">The sharpen radius.</param>
		/// <param name="passes">The number of passes to apply. At least 2 are recommended for quality.</param>
		public static void BoxSharpen(this Bitmap image, int radius, int passes = 2) {
			if (radius <= 0 || passes <= 0)
				return;
			using (PixelWorker worker = PixelWorker.FromImage(image, false, true))
				BoxSharpen(worker, radius, passes);
		}

		/// <summary>
		/// Sharpens the specified image using a fast unsharp mask approximation.
		/// </summary>
		/// <param name="source">The image to sharpen.</param>
		/// <param name="radius">The sharpen radius.</param>
		/// <param name="passes">The number of passes to apply. At least 2 are recommended for quality.</param>
		public static void BoxSharpen(this PixelWorker source, int radius, int passes = 2) {
			if (radius <= 0 || passes <= 0)
				return;
			using (PixelWorker wrapper = new PixelWorker(source.ToByteArray(true), source.Width, source.Height, false)) {
				BoxBlur(wrapper, radius, passes);
				Transition(source, wrapper, -1f);
			}
		}

		/// <summary>
		/// Sharpens the specified image using a fast unsharp mask approximation.
		/// </summary>
		/// <param name="image">The image to sharpen.</param>
		/// <param name="radius">The sharpen radius.</param>
		/// <param name="passes">The number of passes to apply. At least 2 are recommended for quality.</param>
		public static void BoxSharpen(this Bitmap image, float radius, int passes = 2) {
			if (radius <= 0f || passes <= 0)
				return;
			using (PixelWorker worker = PixelWorker.FromImage(image, false, true))
				BoxSharpen(worker, radius, passes);
		}

		/// <summary>
		/// Sharpens the specified image using a fast unsharp mask approximation.
		/// </summary>
		/// <param name="source">The image to sharpen.</param>
		/// <param name="radius">The sharpen radius.</param>
		/// <param name="passes">The number of passes to apply. At least 2 are recommended for quality.</param>
		public static void BoxSharpen(this PixelWorker source, float radius, int passes = 2) {
			if (radius <= 0f || passes <= 0)
				return;
			float fraction = radius % 1f;
			if (fraction <= float.Epsilon)
				BoxSharpen(source, (int) radius, passes);
			else {
				using (PixelWorker original = new PixelWorker(source)) {
					using (PixelWorker wrapper = new PixelWorker(original)) {
						BoxBlur(wrapper, (int) radius, passes);
						Transition(source, wrapper, -1f);
						wrapper.CopyFrom(original);
						BoxBlur(wrapper, (int) radius + 1, passes);
						Transition(original, wrapper, -1f);
						Transition(source, original, fraction);
					}
				}
			}
		}

		/// <summary>
		/// Applies a discrete Prewitt edge detection filter to the specified image.
		/// </summary>
		/// <param name="image">The image to overlay edges onto.</param>
		public static void PrewittEdgeFilter(this Bitmap image) {
			using (PixelWorker worker = PixelWorker.FromImage(image, false, true))
				PrewittEdgeFilter(worker);
		}

		/// <summary>
		/// Applies a discrete Prewitt edge detection filter to the specified image.
		/// </summary>
		/// <param name="source">The image to overlay edges onto.</param>
		public static void PrewittEdgeFilter(this PixelWorker source) {
			float[] gX = source.ToFloatArray();
			float[] gY = new float[gX.Length];
			Array.Copy(gX, gY, gX.Length);
			ApplyFilter(gX, source.Width, source.Height, prewittX, prewittY, true);
			ApplyFilter(gY, source.Width, source.Height, prewittY, prewittX, true);
			ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
				source[i] = Clamp(Math.Sqrt(gX[i] * gX[i] + gY[i] * gY[i]));
			});
		}

		/// <summary>
		/// Applies a discrete Sobel edge detection filter to the specified image.
		/// </summary>
		/// <param name="image">The image to overlay edges onto.</param>
		public static void SobelEdgeFilter(this Bitmap image) {
			using (PixelWorker worker = PixelWorker.FromImage(image, false, true))
				SobelEdgeFilter(worker);
		}

		/// <summary>
		/// Applies a discrete Sobel edge detection filter to the specified image.
		/// </summary>
		/// <param name="source">The image to overlay edges onto. WriteChanges() is not called, that's on you.</param>
		public static void SobelEdgeFilter(this PixelWorker source) {
			float[] gX = source.ToFloatArray();
			float[] gY = new float[gX.Length];
			Array.Copy(gX, gY, gX.Length);
			ApplyFilter(gX, source.Width, source.Height, prewittX, SobelKernelY[2], true);
			ApplyFilter(gY, source.Width, source.Height, SobelKernelY[2], prewittX, true);
			ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
				source[i] = Clamp(Math.Sqrt(gX[i] * gX[i] + gY[i] * gY[i]));
			});
		}

		/// <summary>
		/// Applies a discrete Sobel edge detection filter to the specified image and returns a boolean matrix [x][y]
		/// with 'true' for edges and 'false' for non-edges.
		/// </summary>
		/// <param name="image">The image to use for processing (will not be written to).</param>
		/// <param name="threshold">The edge cutoff threshold (greater than 0).</param>
		public static bool[][] SobelEdgeFilter(this Bitmap image, float threshold = 8500f) {
			using (PixelWorker source = PixelWorker.FromImage(image, false, false, ImageParameterAction.RemoveReference))
				return SobelEdgeFilter(source, threshold);
		}

		/// <summary>
		/// Applies a discrete Sobel edge detection filter to the specified image and returns a boolean matrix [x][y]
		/// with 'true' for edges and 'false' for non-edges.
		/// </summary>
		/// <param name="source">The image to use for processing (will not be written to).</param>
		/// <param name="threshold">The edge cutoff threshold (greater than 0).</param>
		public static bool[][] SobelEdgeFilter(this PixelWorker source, float threshold = 8500f) {
			int w = source.Width;
			bool[][] results = new bool[w][];
			int h = source.Height;
			for (int t = 0; t < w; t++)
				results[t] = new bool[h];
			ParallelLoop.For(0, source.PixelCount, delegate (int i) {
				int width = source.Width;
				int x = i % width;
				int y = i / width;
				float xTotal = 0f, yTotal = 0f, tempXMult, tempYMult;
				float luminosity;
				int offsetX;
				BgraColor temp;
				for (int offsetY = 0; offsetY < 3; offsetY++) {
					for (offsetX = 0; offsetX < 3; offsetX++) {
						temp = source.GetPixelClampedBgra(x + offsetX - 1, y + offsetY - 1);
						luminosity = 0.299f * temp.R + 0.587f * temp.G + 0.114f * temp.B;
						tempXMult = SobelKernelX[offsetX][offsetY];
						tempYMult = SobelKernelY[offsetX][offsetY];
						xTotal += tempXMult * luminosity;
						yTotal += tempYMult * luminosity;
					}
				}
				results[x][y] = xTotal * xTotal + yTotal * yTotal > threshold;
			}, ParallelCutoff / 9);
			return results;
		}

		/// <summary>
		/// Shrinks the "true" (1) regions of in the array.
		/// </summary>
		/// <param name="source">The thresholded values to use, where the array format is [x][y].</param>
		/// <param name="radius">The radius to erode at in pixels.</param>
		public static bool[][] Erode(this bool[][] source, int radius) {
			int width = source.Length;
			bool[][] resultant = new bool[width][];
			if (!(width == 0 || source[0] == null)) {
				int height = source[0].Length;
				radius = Math.Min(radius, Math.Max(width, height));
				for (int x = 0; x < width; x++)
					resultant[x] = new bool[height];
				if (height != 0) {
					ParallelLoop.For(0, width, delegate (int x) {
						bool result;
						int maxX, maxY, targetX, targetY;
						for (int y = 0; y < height; y++) {
							maxX = x + radius;
							maxY = y + radius;
							result = true;
							for (targetX = x - radius; result && targetX <= maxX; targetX++) {
								if (targetX >= 0 && targetX < width) {
									for (targetY = y - radius; result && targetY <= maxY; targetY++) {
										if (targetY >= 0 && targetY < height)
											result &= source[targetX][targetY];
									}
								}
							}
							resultant[x][y] = result;
						}
					});
				}
			}
			return resultant;
		}

		/// <summary>
		/// Expands the "true" (1) regions of in the array.
		/// </summary>
		/// <param name="source">The thresholded values to use, where the array format is [x][y].</param>
		/// <param name="radius">The radius to dilate at in pixels.</param>
		public static bool[][] Dilate(this bool[][] source, int radius) {
			int width = source.Length;
			bool[][] resultant = new bool[width][];
			if (!(width == 0 || source[0] == null)) {
				int height = source[0].Length;
				radius = Math.Min(radius, Math.Max(width, height));
				for (int x = 0; x < width; x++)
					resultant[x] = new bool[height];
				if (height != 0) {
					for (int x = 0; x < width; x++) {
						bool result;
						int maxX, maxY, targetX, targetY;
						for (int y = 0; y < height; y++) {
							maxX = x + radius;
							maxY = y + radius;
							result = false;
							for (targetX = x - radius; !result && targetX <= maxX; targetX++) {
								if (targetX >= 0 && targetX < width) {
									for (targetY = y - radius; !result && targetY <= maxY; targetY++) {
										if (targetY >= 0 && targetY < height)
											result |= source[targetX][targetY];
									}
								}
							}
							resultant[x][y] = result;
						}
					}
				}
			}
			return resultant;
		}

		/// <summary>
		/// Applies the specified filter using Fourier transform.
		/// </summary>
		/// <param name="image">The image to transform.</param>
		/// <param name="kernel">The filter to convolve with [x][y].</param>
		/// <param name="ignoreAlpha">False to also apply to alpha components, true to leave alpha intact.</param>
		public static PixelWorker ConvolveFFT(this Bitmap image, float[][] kernel, bool ignoreAlpha = false) {
			using (FourierWorker fourier = new FourierWorker(image, true, ignoreAlpha)) {
				using (FourierWorker k = new FourierWorker(kernel, true))
					fourier.MultiplyWith(k);
				return fourier.ConvertToBitmapInPlace();
			}
		}

		/// <summary>
		/// Performs Gaussian Blur using Fourier transform. Use when the blur radius you need is very large.
		/// </summary>
		/// <param name="image">The image to blur.</param>
		/// <param name="sigma">The blur radius.</param>
		/// <param name="ignoreAlpha">False to also blur alpha components, true to leave alpha intact.</param>
		public static PixelWorker GaussianBlurFFT(this Bitmap image, double sigma, bool ignoreAlpha = false) {
			using (FourierWorker fourier = new FourierWorker(image, true, ignoreAlpha)) {
				using (FourierWorker gaussian = new FourierWorker(GaussianKernel(fourier.FourierWidth, fourier.FourierHeight, sigma), true))
					fourier.MultiplyWith(gaussian);
				return fourier.ConvertToBitmapInPlace();
			}
		}

		/// <summary>
		/// Performs Gaussian Blur using Fourier transform. Use when the blur radius you need is very large.
		/// </summary>
		/// <param name="image">The image to blur.</param>
		/// <param name="sigma">The blur radius.</param>
		/// <param name="ignoreAlpha">False to also blur alpha components, true to leave alpha intact.</param>
		public static PixelWorker GaussianBlurFFT(this PixelWorker image, double sigma, bool ignoreAlpha = false) {
			using (FourierWorker fourier = new FourierWorker(image, true, ignoreAlpha)) {
				using (FourierWorker gaussian = new FourierWorker(GaussianKernel(fourier.FourierWidth, fourier.FourierHeight, sigma), true))
					fourier.MultiplyWith(gaussian);
				return fourier.ConvertToBitmapInPlace();
			}
		}

		/// <summary>
		/// Multiplies element-wise the values of the image by the values of the specified kernel [x][y].
		/// </summary>
		/// <param name="image">The image whose values to multiply.</param>
		/// <param name="multiplier">The kernel to multiply with.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void MultiplyWith(this Bitmap image, float[][] multiplier) {
			MultiplyWith(image, new float[][][] { multiplier });
		}

		/// <summary>
		/// Multiplies element-wise the values of the image by the values of the specified kernel [x][y].
		/// </summary>
		/// <param name="image">The image whose values to multiply. WriteChanges() is not called, that's on you.</param>
		/// <param name="multiplier">The kernel to multiply with.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void MultiplyWith(this PixelWorker image, float[][] multiplier) {
			MultiplyWith(image, new float[][][] { multiplier });
		}

		/// <summary>
		/// Multiplies element-wise the values of the image by the values of the specified kernel [channel][x][y].
		/// </summary>
		/// <param name="image">The image whose values to multiply.</param>
		/// <param name="multiplier">The kernel to multiply with. If the kernel has one channel, it will be multiplied with all channels of this image, else the corresponding channels will be multiplied.</param>
		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void MultiplyWith(this Bitmap image, float[][][] multiplier) {
			using (PixelWorker worker = PixelWorker.FromImage(image, false, true))
				MultiplyWith(worker, multiplier);
		}

		/// <summary>
		/// Multiplies element-wise the values of the image by the values of the specified kernel [channel][x][y].
		/// </summary>
		/// <param name="image">The image whose values to multiply. WriteChanges() is not called, that's on you.</param>
		/// <param name="multiplier">The kernel to multiply with. If the kernel has one channel, it will be multiplied with all channels of this image, else the corresponding channels will be multiplied.</param>
		[CLSCompliant(false)]
		public static void MultiplyWith(this PixelWorker image, float[][][] multiplier) {
			int componentCount = multiplier.Length, width = multiplier[0].Length, height = multiplier[0][0].Length;
			if (!(image.Width == width && image.Height == height))
				throw new ArgumentException("The image must have the same width and height as the kernel.", nameof(multiplier));
			if (componentCount == 1) {
				float[][] mult = multiplier[0];
				ParallelLoop.For(0, image.PixelComponentCount, i => {
					int pixelIndex = i / image.ComponentCount;
					image[i] = Clamp(image[i] * mult[pixelIndex % width][pixelIndex / width]);
				}, ParallelCutoff);
			} else {
				int min = Math.Min(image.ComponentCount, componentCount);
				ParallelLoop.For(0, image.PixelComponentCount, i => {
					int pixelIndex = i / image.ComponentCount;
					image[i] = Clamp(image[i] * multiplier[i % image.ComponentCount][pixelIndex % width][pixelIndex / width]);
				}, ParallelCutoff);
			}
		}

		/// <summary>
		/// Divides element-wise the values of the image by the values of the specified kernel [x][y].
		/// </summary>
		/// <param name="image">The image whose values to divide.</param>
		/// <param name="divisor">The kernel to divide by.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void DivideBy(this Bitmap image, float[][] divisor) {
			DivideBy(image, new float[][][] { divisor });
		}

		/// <summary>
		/// Divides element-wise the values of the image by the values of the specified kernel [x][y].
		/// </summary>
		/// <param name="image">The image whose values to divide. WriteChanges() is not called, that's on you.</param>
		/// <param name="divisor">The kernel to divide by.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void DivideBy(this PixelWorker image, float[][] divisor) {
			DivideBy(image, new float[][][] { divisor });
		}

		/// <summary>
		/// Divides element-wise the values of the image by the values of the specified kernel [x][y].
		/// </summary>
		/// <param name="image">The image whose values to divide. WriteChanges() is not called, that's on you.</param>
		/// <param name="divisor">The kernel to divide by. If the kernel has one channel, all channels of this image will be divided by it, else the corresponding channels will be divided.</param>
		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void DivideBy(this Bitmap image, float[][][] divisor) {
			using (PixelWorker worker = PixelWorker.FromImage(image, false, true))
				DivideBy(worker, divisor);
		}

		/// <summary>
		/// Divides element-wise the values of the image by the values of the specified kernel [x][y].
		/// </summary>
		/// <param name="image">The image whose values to divide. WriteChanges() is not called, that's on you.</param>
		/// <param name="divisor">The kernel to divide by. If the kernel has one channel, all channels of this image will be divided by it, else the corresponding channels will be divided.</param>
		[CLSCompliant(false)]
		public static void DivideBy(this PixelWorker image, float[][][] divisor) {
			int componentCount = divisor.Length, width = divisor[0].Length, height = divisor[0][0].Length;
			if (!(image.Width == width && image.Height == height))
				throw new ArgumentException("The image must have the same width and height as the kernel.", nameof(divisor));
			if (componentCount == 1) {
				float[][] div = divisor[0];
				ParallelLoop.For(0, image.PixelComponentCount, i => {
					int pixelIndex = i / image.ComponentCount;
					float val = div[pixelIndex % width][pixelIndex / width];
					if (val == 0f)
						image[i] = 255;
					else
						image[i] = Clamp(image[i] / val);
				}, ParallelCutoff);
			} else {
				int min = Math.Min(image.ComponentCount, componentCount);
				ParallelLoop.For(0, image.PixelComponentCount, i => {
					int pixelIndex = i / image.ComponentCount;
					float val = divisor[i % image.ComponentCount][pixelIndex % width][pixelIndex / width];
					if (val == 0f)
						image[i] = 255;
					else
						image[i] = Clamp(image[i] / val);
				}, ParallelCutoff);
			}
		}

		/// <summary>
		/// Gets a bitmap representation from the specified boolean values [x][y].
		/// </summary>
		/// <param name="values">The values to represent. All columns must be of the same height.</param>
		public static Bitmap ToBitmap(this bool[][] values) {
			return ToBitmap(values, BgraColor.White, BgraColor.Black, 1);
		}

		/// <summary>
		/// Gets a bitmap representation from the specified boolean values [x][y].
		/// </summary>
		/// <param name="values">The values to represent. All columns must be of the same height.</param>
		/// <param name="forTrue">The color to use for true values.</param>
		/// <param name="forFalse">The color to use for false values.</param>
		/// <param name="bitDepth">The bit-depth of the final image (can be 1, 3 or 4). 1 is grayscale, 3 is RGB and 4 is ARGB.</param>
		public static Bitmap ToBitmap(this bool[][] values, Color forTrue, Color forFalse, int bitDepth = 1) {
			return ToBitmap(values, (BgraColor) forTrue, (BgraColor) forFalse, bitDepth);
		}

		/// <summary>
		/// Gets a bitmap representation from the specified boolean values [x][y].
		/// </summary>
		/// <param name="values">The values to represent. All columns must be of the same height.</param>
		/// <param name="forTrue">The color to use for true values.</param>
		/// <param name="forFalse">The color to use for false values.</param>
		/// <param name="bitDepth">The bit-depth of the final image (can be 1, 3 or 4). 1 is grayscale, 3 is RGB and 4 is ARGB.</param>
		public static Bitmap ToBitmap(this bool[][] values, BgraColor forTrue, BgraColor forFalse, int bitDepth = 1) {
			int width = values.Length, height = values[0].Length;
			Bitmap result = new Bitmap(width, height, bitDepth == 4 ? PixelFormat.Format32bppArgb : (bitDepth == 3 ? PixelFormat.Format24bppRgb : PixelFormat.Format8bppIndexed));
			using (PixelWorker worker = PixelWorker.FromImage(result, false, true)) {
				ParallelLoop.For(0, width, x => {
					bool[] temp = values[x];
					for (int y = 0; y < height; y++)
						worker.SetPixel(x, y, temp[y] ? forTrue : forFalse);
				}, ParallelCutoff / height);
			}
			return result;
		}

		/// <summary>
		/// Gets a bitmap representation from the specified boolean values [x][y].
		/// </summary>
		/// <param name="values">The values to represent. All columns must be of the same height.</param>
		/// <param name="image">The image whose pixels to sample for 'true' values.</param>
		/// <param name="forFalse">The color to use for 'false' values.</param>
		public static Bitmap ToBitmap(this bool[][] values, Bitmap image, BgraColor forFalse) {
			using (PixelWorker source = PixelWorker.FromImage(image, false, false, ImageParameterAction.RemoveReference))
				return ToBitmap(values, source, forFalse);
		}

		/// <summary>
		/// Gets a bitmap representation from the specified boolean values [x][y].
		/// </summary>
		/// <param name="values">The values to represent. All columns must be of the same height.</param>
		/// <param name="image">The image whose pixels to sample for 'true' values.</param>
		/// <param name="forFalse">The color to use for 'false' values.</param>
		public static Bitmap ToBitmap(this bool[][] values, PixelWorker image, BgraColor forFalse) {
			int width = values.Length, height = values[0].Length;
			Bitmap result = new Bitmap(width, height, image.Format);
			using (PixelWorker worker = PixelWorker.FromImage(result, false, true)) {
				ParallelLoop.For(0, width, x => {
					bool[] temp = values[x];
					for (int y = 0; y < height; y++)
						worker.SetPixel(x, y, temp[y] ? image.GetPixelBgra(x, y) : forFalse);
				}, ParallelCutoff / height);
			}
			return result;
		}

		/// <summary>
		/// Gets the median of the specified bytes.
		/// </summary>
		/// <param name="values">The array of values whose median to find.</param>
		public static byte GetMedianElement(this byte[] values) {
			if (values.Length <= 1)
				return values[0];
			else if (values.Length == 2)
				return Math.Max(values[0], values[1]);
			byte[] array = new byte[values.Length];
			Buffer.BlockCopy(values, 0, array, 0, values.Length);
			int startIndex = 0;
			int endIndex = array.Length - 1;
			int mid = values.Length / 2;
			int pivotIndex = mid;
			int i;
			byte pivotValue;
			while (endIndex > startIndex) {
				pivotValue = array[pivotIndex];
				array[pivotIndex] = array[endIndex];
				array[endIndex] = pivotValue;
				pivotIndex = startIndex;
				byte temp;
				for (i = pivotIndex; i < endIndex; i++) {
					if (array[i] < pivotValue) {
						temp = array[i];
						array[i] = array[pivotIndex];
						array[pivotIndex] = temp;
						pivotIndex++;
					}
				}
				temp = array[endIndex];
				array[endIndex] = array[pivotIndex];
				array[pivotIndex] = temp;
				if (pivotIndex == mid)
					break;
				else if (pivotIndex > mid)
					endIndex = pivotIndex - 1;
				else
					startIndex = pivotIndex + 1;
				pivotIndex = (startIndex + endIndex - 1) / 2;
			}
			return array[mid];
		}

		private static unsafe byte GetMedianInPlace(byte* array, int count) {
			if (count <= 2)
				return *array;
			int startIndex = 0;
			int endIndex = count - 1;
			count /= 2;
			int pivotIndex = count;
			int i;
			byte pivotValue;
			while (endIndex > startIndex) {
				pivotValue = array[pivotIndex];
				array[pivotIndex] = array[endIndex];
				array[endIndex] = pivotValue;
				pivotIndex = startIndex;
				byte temp;
				for (i = pivotIndex; i < endIndex; i++) {
					if (array[i] < pivotValue) {
						temp = array[i];
						array[i] = array[pivotIndex];
						array[pivotIndex] = temp;
						pivotIndex++;
					}
				}
				temp = array[endIndex];
				array[endIndex] = array[pivotIndex];
				array[pivotIndex] = temp;
				if (pivotIndex == count)
					break;
				else if (pivotIndex > count)
					endIndex = pivotIndex - 1;
				else
					startIndex = pivotIndex + 1;
				pivotIndex = (startIndex + endIndex - 1) / 2;
			}
			return array[count];
		}

		/// <summary>
		/// Applies pixel offset anti-aliasing to the specified image.
		/// </summary>
		/// <param name="image">The image to apply anti-aliasing to.</param>
		/// <param name="weight">The weight to use for anti-aliasing from 0 to 1 (0 means no anti-aliasing).</param>
		public static void AntiAlias(this Bitmap image, float weight = 0.3f) {
			if (weight == 0f)
				return;
			using (PixelWorker source = PixelWorker.FromImage(image, false, true, ImageParameterAction.RemoveReference))
				AntiAlias(source, weight);
		}

		/// <summary>
		/// Applies pixel offset anti-aliasing to the specified image.
		/// </summary>
		/// <param name="source">The image to apply anti-aliasing to. WriteChanges() is not called, that's on you.</param>
		/// <param name="weight">The weight to use for anti-aliasing from 0 to 1 (0 means no anti-aliasing).</param>
		public static void AntiAlias(this PixelWorker source, float weight = 1f) {
			if (weight == 0f)
				return;
			weight = Maths.Clamp(Math.Abs(weight), 0f, 1f) * 0.5f;
			using (PixelWorker resultant = new PixelWorker(source.Width, source.Height, source.ComponentCount)) {
				ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
					int componentCount = source.ComponentCount;
					int pixelIndex = i / componentCount;
					int width = source.Width;
					int x = pixelIndex % width;
					int y = pixelIndex / width;
					if (!(x == width - 1 || y == source.Height - 1)) {
						int x1y2 = i + source.WidthComponentCount;
						int x2y2 = x1y2 + componentCount;
						resultant[i] = InterpolateLinear(source[i], source[i + componentCount], source[x1y2], source[x2y2], weight, weight);
					}
				}, ParallelCutoff);
				weight = 1f - weight;
				ParallelLoop.For(source.ComponentCount, source.PixelComponentCount, delegate (int i) {
					int componentCount = source.ComponentCount;
					int pixelIndex = i / componentCount;
					int width = source.Width;
					int x = pixelIndex % width;
					int y = pixelIndex / width;
					if (!(x == width - 1 || y == source.Height - 1)) {
						int x1y2 = i + source.WidthComponentCount;
						int x2y2 = x1y2 + componentCount;
						source[i] = InterpolateLinear(resultant[x2y2], resultant[i + componentCount], resultant[x1y2], resultant[i], weight, weight);
					}
				}, ParallelCutoff);
			}
		}

		/// <summary>
		/// Performs per-channel thresholding. If needed, convert to grayscale before thresholding.
		/// </summary>
		/// <param name="image">The image to clamp.</param>
		/// <param name="cutoff">The threshold to use.</param>
		/// <param name="ignoreAlpha">If true, the alpha channel is not clamped.</param>
		public static void Threshold(this Bitmap image, byte cutoff = 128, bool ignoreAlpha = true) {
			if (image == null)
				return;
			using (PixelWorker source = PixelWorker.FromImage(image, false, true, ImageParameterAction.RemoveReference))
				Threshold(source, cutoff, ignoreAlpha);
		}

		/// <summary>
		/// Performs per-channel thresholding. If needed, convert to grayscale before thresholding.
		/// </summary>
		/// <param name="source">The image to clamp.</param>
		/// <param name="cutoff">The threshold to use (inclusive).</param>
		/// <param name="ignoreAlpha">If true, the alpha channel is not clamped.</param>
		public static void Threshold(this PixelWorker source, byte cutoff = 128, bool ignoreAlpha = true) {
			if (source == null)
				return;
			else if (source.ComponentCount == 4 && ignoreAlpha) {
				if (source.Buffer == null) {
					unsafe
					{
						ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
							if (i % 4 != 3) {
								byte* index = source.Scan0 + i;
								*index = *index < cutoff ? (byte) 0 : (byte) 255;
							}
						}, ParallelCutoff);
					}
				} else {
					ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
						if (i % 4 != 3)
							source.Buffer[i] = source.Buffer[i] < cutoff ? (byte) 0 : (byte) 255;
					}, ParallelCutoff);
				}
			} else {
				if (source.Buffer == null) {
					unsafe
					{
						ParallelLoop.For(source.Scan0, source.PixelComponentCount, 1L, i => {
							byte* index = (byte*) i;
							*index = *index < cutoff ? (byte) 0 : (byte) 255;
						}, ParallelCutoff);
					}
				} else {
					ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
						source.Buffer[i] = source.Buffer[i] < cutoff ? (byte) 0 : (byte) 255;
					}, ParallelCutoff);
				}
			}
		}

		/// <summary>
		/// Performs per-channel thresholding and dithering. If needed, convert to grayscale before thresholding.
		/// </summary>
		/// <param name="image">The image to clamp.</param>
		/// <param name="cutoff">The threshold to use.</param>
		/// <param name="noiser">True to decrease dithering patterns (slower of course).</param>
		/// <param name="ignoreAlpha">If true, the alpha channel is not clamped.</param>
		public static void ThresholdDither(this Bitmap image, byte cutoff = 128, bool noiser = false, bool ignoreAlpha = true) {
			if (image == null)
				return;
			using (PixelWorker source = PixelWorker.FromImage(image, false, true, ImageParameterAction.RemoveReference))
				ThresholdDither(source, cutoff, noiser, ignoreAlpha);
		}

		/// <summary>
		/// Performs per-channel thresholding and dithering. If needed, convert to grayscale before thresholding. (Slow)
		/// </summary>
		/// <param name="source">The image to clamp.</param>
		/// <param name="cutoff">The threshold to use (inclusive).</param>
		/// <param name="noiser">True to decrease dithering patterns (slower of course).</param>
		/// <param name="ignoreAlpha">If true, the alpha channel is not clamped.</param>
		public static void ThresholdDither(this PixelWorker source, byte cutoff = 128, bool noiser = false, bool ignoreAlpha = true) {
			if (source == null)
				return;
			using (PixelWorker result = new PixelWorker(source.Width, source.Height, source.ComponentCount)) {
				int width = source.Width, height = source.Height;
				BgraColor currentPixel, bestColor, temp;
				int errorA, errorR, errorG, errorB, tempX, yPlusOne, x, random;
				for (int y = 0; y < height; y++) {
					for (x = 0; x < width; x++) {
						currentPixel = source.GetPixelBgra(x, y);
						bestColor = Threshold(currentPixel, cutoff, ignoreAlpha);
						result.SetPixel(x, y, bestColor);
						errorR = currentPixel.R - bestColor.R;
						errorG = currentPixel.G - bestColor.G;
						errorB = currentPixel.B - bestColor.B;
						errorA = currentPixel.A - bestColor.A;
						if (noiser) {
							random = (int) UniformRandom.Random.ToInterval(-30.0, 30.0);
							errorR += random;
							errorG += random;
							errorB += random;
							errorA += random;
						}
						tempX = x + 1;
						if (tempX < width) {
							temp = source.GetPixelBgra(tempX, y);
							source.SetPixel(tempX, y, new BgraColor(ignoreAlpha ? temp.A : Clamp(temp.A + ((errorA * 7) >> 4)),
								Clamp(temp.R + ((errorR * 7) >> 4)), Clamp(temp.G + ((errorG * 7) >> 4)), Clamp(temp.B + ((errorB * 7) >> 4))));
						}
						yPlusOne = y + 1;
						if (yPlusOne < height) {
							if (x != 0) {
								tempX = x - 1;
								temp = source.GetPixelBgra(tempX, yPlusOne);
								source.SetPixel(tempX, yPlusOne, new BgraColor(ignoreAlpha ? temp.A : Clamp(temp.A + ((errorA * 3) >> 4)),
									Clamp(temp.R + ((errorR * 3) >> 4)), Clamp(temp.G + ((errorG * 3) >> 4)), Clamp(temp.B + ((errorB * 3) >> 4))));
							}
							temp = source.GetPixelBgra(x, yPlusOne);
							source.SetPixel(x, yPlusOne, new BgraColor(ignoreAlpha ? temp.A : Clamp(temp.A + ((errorA * 5) >> 4)),
								Clamp(temp.R + ((errorR * 5) >> 4)), Clamp(temp.G + ((errorG * 5) >> 4)), Clamp(temp.B + ((errorB * 5) >> 4))));
							tempX = x + 1;
							if (tempX < width) {
								temp = source.GetPixelBgra(tempX, yPlusOne);
								source.SetPixel(tempX, yPlusOne, new BgraColor(ignoreAlpha ? temp.A : Clamp(temp.A + (errorA >> 4)),
									Clamp(temp.R + (errorR >> 4)), Clamp(temp.G + (errorG >> 4)), Clamp(temp.B + (errorB >> 4))));
							}
						}
					}
				}
				source.CopyFrom(result);
			}
		}

		/*/// <summary>
		/// Counts the number of contiguous shapes in the image by removing dark background colors using the specified threshold.
		/// </summary>
		/// <param name="image">The source image.</param>
		/// <param name="cutoff">The background color threshold.</param>
		public static int GetContiguousShapeCount(this Bitmap image, byte cutoff) {
			using (PixelWorker worker = PixelWorker.FromBitmap(image, false, true))
				return GetContiguousShapeCount(image, cutoff);
		}

		/// <summary>
		/// Counts the number of contiguous shapes in the image by removing dark background colors using the specified threshold.
		/// </summary>
		/// <param name="source">The source image.</param>
		/// <param name="cutoff">The background color threshold.</param>
		public static int GetContiguousShapeCount(this PixelWorker source, byte cutoff) {
			int width = source.Width;
			int height = source.Height;
			int[][] labels = new int[width][];
			int i;
			for (i = 0; i < width; i++)
				labels[i] = new int[height];
			int counter = 1;
			HashSet<int> equal = new HashSet<int>();
			int y, currLabel;
			List<Point> indices = new List<Point>();
			int yMinusOne;
			for (int x = 0; x < width; x++) {
				for (y = 0; y < height; y++) {
					if (!GetThreshold(source.GetPixelBgra(x, y), cutoff))
						continue;
					indices.Clear();
					if (y != 0) {
						yMinusOne = y - 1;
						indices.Add(new Point(x, yMinusOne));
						if (x != 0)
							indices.Add(new Point(x - 1, yMinusOne));
						if (x != width - 1)
							indices.Add(new Point(x + 1, yMinusOne));
					}
					if (x != 0)
						indices.Add(new Point(x - 1, y));
					foreach (Point point in indices) {
						currLabel = labels[point.X][point.Y];
						if (currLabel != 0) {
							if (labels[x][y] == 0)
								labels[x][y] = currLabel;
							else if (currLabel != labels[x][y])
								equal.Add(Math.Max(labels[x][y], currLabel));
						}
					}
					if (labels[x][y] == 0) {
						labels[x][y] = counter;
						counter++;
					}
				}
			}
			return counter - equal.Count;
		}*/

		/// <summary>
		/// Performs per-channel thresholding.
		/// </summary>
		/// <param name="color">The color to clamp.</param>
		/// <param name="cutoff">The threshold to use.</param>
		/// <param name="ignoreAlpha">If true, the alpha channel is not clamped.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Color Threshold(this Color color, byte cutoff, bool ignoreAlpha = true) {
			return Color.FromArgb(ignoreAlpha ? color.A : (color.A < cutoff ? 0 : 255), color.R < cutoff ? 0 : 255, color.G < cutoff ? 0 : 255, color.B < cutoff ? 0 : 255);
		}

		/// <summary>
		/// Performs grayscale thresholding.
		/// </summary>
		/// <param name="color">The color to clamp.</param>
		/// <param name="cutoff">The threshold to use.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool GetThreshold(this Color color, byte cutoff) {
			return ToGrayscale(color) >= cutoff;
		}

		/// <summary>
		/// Performs per-channel thresholding.
		/// </summary>
		/// <param name="color">The color to clamp.</param>
		/// <param name="cutoff">The threshold to use.</param>
		/// <param name="ignoreAlpha">If true, the alpha channel is not clamped.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static BgraColor Threshold(this BgraColor color, byte cutoff, bool ignoreAlpha = true) {
			return new BgraColor(ignoreAlpha ? color.A : (color.A < cutoff ? (byte) 0 : (byte) 255), color.R < cutoff ? (byte) 0 : (byte) 255, color.G < cutoff ? (byte) 0 : (byte) 255, color.B < cutoff ? (byte) 0 : (byte) 255);
		}

		/// <summary>
		/// Performs grayscale thresholding.
		/// </summary>
		/// <param name="color">The color to clamp.</param>
		/// <param name="cutoff">The threshold to use.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool GetThreshold(this BgraColor color, byte cutoff) {
			return ToGrayscale(color) >= cutoff;
		}

		/// <summary>
		/// Performs grayscale thresholding and returns a boolean array [x][y].
		/// </summary>
		/// <param name="image">The image to clamp.</param>
		/// <param name="cutoff">The threshold to use.</param>
		public static bool[][] GetThreshold(this Bitmap image, byte cutoff = 128) {
			using (PixelWorker source = PixelWorker.FromImage(image, false, true, ImageParameterAction.RemoveReference))
				return GetThreshold(source, cutoff);
		}

		/// <summary>
		/// Performs grayscale thresholding and returns a boolean array [x][y].
		/// </summary>
		/// <param name="source">The image to clamp.</param>
		/// <param name="cutoff">The threshold to use.</param>
		public static bool[][] GetThreshold(this PixelWorker source, byte cutoff = 128) {
			float threshold = cutoff;
			bool[][] results = new bool[source.Width][];
			for (int i = 0; i < source.Width; i++)
				results[i] = new bool[source.Height];
			ParallelLoop.For(0, source.PixelCount, delegate (int i) {
				results[i % source.Width][i / source.Width] = ToGrayscale(source.GetPixelUsingPixelCountBgra(i)) >= threshold;
			}, ParallelCutoff);
			return results;
		}

		/// <summary>
		/// Gets the color that is nearest to the specified color.
		/// </summary>
		/// <param name="color">The color to find the closest to.</param>
		/// <param name="palette">The palette of colors to choose from.</param>
		public static Color GetNearestColor(this Color color, params Color[] palette) {
			if (palette == null || palette.Length == 0)
				return color;
			else if (palette.Length == 1)
				return palette[0];
			else
				return GetNearestColorInner(color, palette);
		}

		private static Color GetNearestColorInner(this Color color, Color[] palette) {
			int distanceSquared, minDistanceSquared = 260101; /*255 * 255 + 255 * 255 + 255 * 255 + 255 * 255 + 1*/
			Color bestColor = Color.Black;
			int rDiff, gDiff, bDiff, aDiff;
			Color paletteColor;
			for (int i = 0; i < palette.Length; i++) {
				paletteColor = palette[i];
				rDiff = color.R - paletteColor.R;
				gDiff = color.G - paletteColor.G;
				bDiff = color.B - paletteColor.B;
				aDiff = color.A - paletteColor.A;
				distanceSquared = rDiff * rDiff + gDiff * gDiff + bDiff * bDiff + aDiff * aDiff;
				if (distanceSquared < minDistanceSquared) {
					minDistanceSquared = distanceSquared;
					bestColor = paletteColor;
				}
			}
			return bestColor;
		}

		/// <summary>
		/// Gets the color that is nearest to the specified color.
		/// </summary>
		/// <param name="color">The color to find the closest to.</param>
		/// <param name="palette">The palette of colors to choose from.</param>
		public static BgraColor GetNearestColor(this BgraColor color, params BgraColor[] palette) {
			if (palette == null || palette.Length == 0)
				return color;
			else if (palette.Length == 1)
				return palette[0];
			else
				return GetNearestColorInner(color, palette);
		}

		private static BgraColor GetNearestColorInner(this BgraColor color, BgraColor[] palette) {
			int distanceSquared, minDistanceSquared = 260101; /*255 * 255 + 255 * 255 + 255 * 255 + 255 * 255 + 1*/
			BgraColor bestColor = BgraColor.Black;
			int rDiff, gDiff, bDiff, aDiff;
			BgraColor paletteColor;
			for (int i = 0; i < palette.Length; i++) {
				paletteColor = palette[i];
				rDiff = color.R - paletteColor.R;
				gDiff = color.G - paletteColor.G;
				bDiff = color.B - paletteColor.B;
				aDiff = color.A - paletteColor.A;
				distanceSquared = rDiff * rDiff + gDiff * gDiff + bDiff * bDiff + aDiff * aDiff;
				if (distanceSquared < minDistanceSquared) {
					minDistanceSquared = distanceSquared;
					bestColor = paletteColor;
				}
			}
			return bestColor;
		}

		/// <summary>
		/// Gets all the colors of pixels (slow).
		/// </summary>
		/// <param name="image">The image whose pixels to load.</param>
		public static Color[] GetAllPixels(this Bitmap image) {
			using (PixelWorker worker = PixelWorker.FromImage(image, false, true))
				return worker.GetAllPixels();
		}

		/// <summary>
		/// Gets all the colors of pixels.
		/// </summary>
		/// <param name="image">The image whose pixels to load.</param>
		public static BgraColor[] GetAllPixelsBgra(this Bitmap image) {
			using (PixelWorker worker = PixelWorker.FromImage(image, false, true))
				return worker.GetAllPixelsBgra();
		}

		/// <summary>
		/// Sets all the pixels to the specified colors (slow).
		/// </summary>
		/// <param name="image">The image whose colors to set.</param>
		/// <param name="colors">The array of colors to set the pixels to. Must be the same size as the image.</param>
		public static void SetAllPixels(this Bitmap image, Color[] colors) {
			using (PixelWorker worker = PixelWorker.FromImage(image, false, true))
				worker.SetAllPixels(colors);
		}

		/// <summary>
		/// Sets all the pixels to the specified colors (fast).
		/// </summary>
		/// <param name="image">The image whose colors to set.</param>
		/// <param name="colors">The array of colors to set the pixels to. Must be the same size as the image.</param>
		public static void SetAllPixels(this Bitmap image, BgraColor[] colors) {
			using (PixelWorker worker = PixelWorker.FromImage(image, false, true))
				worker.SetAllPixels(colors);
		}

		/// <summary>
		/// Performs quantization on the specified palette.
		/// </summary>
		/// <param name="source">The image to quantize.</param>
		/// <param name="palette">The palette of colors to use.</param>
		public static void Quantize(this Bitmap source, params BgraColor[] palette) {
			if (source == null || palette == null || palette.Length == 0)
				return;
			using (PixelWorker worker = PixelWorker.FromImage(source, false, true))
				Quantize(worker, palette);
		}

		/// <summary>
		/// Performs quantization on the specified palette.
		/// </summary>
		/// <param name="source">The image to quantize.</param>
		/// <param name="palette">The palette of colors to use.</param>
		public static void Quantize(this Bitmap source, params Color[] palette) {
			if (source == null || palette == null || palette.Length == 0)
				return;
			using (PixelWorker worker = PixelWorker.FromImage(source, false, true))
				Quantize(worker, palette);
		}

		/// <summary>
		/// Performs quantization on the specified palette.
		/// </summary>
		/// <param name="source">The image to quantize.</param>
		/// <param name="palette">The palette of colors to use.</param>
		public static void Quantize(this PixelWorker source, params BgraColor[] palette) {
			if (!(source == null || palette == null || palette.Length == 0))
				ParallelLoop.For(0, source.PixelCount, i => source.SetPixelUsingPixelCount(i, GetNearestColorInner(source.GetPixelUsingPixelCountBgra(i), palette)));
		}

		/// <summary>
		/// Performs quantization on the specified palette.
		/// </summary>
		/// <param name="source">The image to quantize.</param>
		/// <param name="palette">The palette of colors to use.</param>
		public static void Quantize(this PixelWorker source, params Color[] palette) {
			if (!(source == null || palette == null || palette.Length == 0))
				ParallelLoop.For(0, source.PixelCount, i => source.SetPixelUsingPixelCount(i, GetNearestColorInner(source.GetPixelUsingPixelCount(i), palette)));
		}

		/// <summary>
		/// Performs dithered quantization on the specified palette.
		/// </summary>
		/// <param name="source">The image to quantize.</param>
		/// <param name="noiser">True to decrease dithering patterns (slower of course).</param>
		/// <param name="palette">The palette of colors to use.</param>
		public static void QuantizeDither(this Bitmap source, bool noiser = false, params BgraColor[] palette) {
			if (source == null || palette == null || palette.Length == 0)
				return;
			using (PixelWorker worker = PixelWorker.FromImage(source, false, true))
				QuantizeDither(worker, noiser, palette);
		}

		/// <summary>
		/// Performs dithered quantization on the specified palette.
		/// </summary>
		/// <param name="source">The image to quantize.</param>
		/// <param name="noiser">True to decrease dithering patterns (slower of course).</param>
		/// <param name="palette">The palette of colors to use.</param>
		public static void QuantizeDither(this Bitmap source, bool noiser = false, params Color[] palette) {
			if (source == null || palette == null || palette.Length == 0)
				return;
			using (PixelWorker worker = PixelWorker.FromImage(source, false, true))
				QuantizeDither(worker, noiser, palette);
		}

		/// <summary>
		/// Performs dithered quantization on the specified palette. (Slow)
		/// </summary>
		/// <param name="source">The image to quantize.</param>
		/// <param name="noiser">True to decrease dithering patterns (slower of course).</param>
		/// <param name="palette">The palette of colors to use.</param>
		public static void QuantizeDither(this PixelWorker source, bool noiser = false, params BgraColor[] palette) {
			if (source == null || palette == null || palette.Length == 0)
				return;
			using (PixelWorker result = new PixelWorker(source.Width, source.Height, source.ComponentCount)) {
				int width = source.Width, height = source.Height;
				BgraColor currentPixel, bestColor, temp;
				int errorA, errorR, errorG, errorB, tempX, yPlusOne, x, random;
				for (int y = 0; y < height; y++) {
					for (x = 0; x < width; x++) {
						currentPixel = source.GetPixelBgra(x, y);
						bestColor = GetNearestColorInner(currentPixel, palette);
						result.SetPixel(x, y, bestColor);
						errorR = currentPixel.R - bestColor.R;
						errorG = currentPixel.G - bestColor.G;
						errorB = currentPixel.B - bestColor.B;
						errorA = currentPixel.A - bestColor.A;
						if (noiser) {
							random = (int) UniformRandom.Random.ToInterval(-30.0, 30.0);
							errorR += random;
							errorG += random;
							errorB += random;
							errorA += random;
						}
						tempX = x + 1;
						if (tempX < width) {
							temp = source.GetPixelBgra(tempX, y);
							source.SetPixel(tempX, y, new BgraColor(Clamp(temp.A + ((errorA * 7) >> 4)), Clamp(temp.R + ((errorR * 7) >> 4)),
								Clamp(temp.G + ((errorG * 7) >> 4)), Clamp(temp.B + ((errorB * 7) >> 4))));
						}
						yPlusOne = y + 1;
						if (yPlusOne < height) {
							if (x != 0) {
								tempX = x - 1;
								temp = source.GetPixelBgra(tempX, yPlusOne);
								source.SetPixel(tempX, yPlusOne, new BgraColor(Clamp(temp.A + ((errorA * 3) >> 4)), Clamp(temp.R + ((errorR * 3) >> 4)),
									Clamp(temp.G + ((errorG * 3) >> 4)), Clamp(temp.B + ((errorB * 3) >> 4))));
							}
							temp = source.GetPixelBgra(x, yPlusOne);
							source.SetPixel(x, yPlusOne, new BgraColor(Clamp(temp.A + ((errorA * 5) >> 4)), Clamp(temp.R + ((errorR * 5) >> 4)),
								Clamp(temp.G + ((errorG * 5) >> 4)), Clamp(temp.B + ((errorB * 5) >> 4))));
							tempX = x + 1;
							if (tempX < width) {
								temp = source.GetPixelBgra(tempX, yPlusOne);
								source.SetPixel(tempX, yPlusOne, new BgraColor(Clamp(temp.A + (errorA >> 4)), Clamp(temp.R + (errorR >> 4)),
									Clamp(temp.G + (errorG >> 4)), Clamp(temp.B + (errorB >> 4))));
							}
						}
					}
				}
				source.CopyFrom(result);
			}
		}

		/// <summary>
		/// Performs dithered quantization on the specified palette. (Slow)
		/// </summary>
		/// <param name="source">The image to quantize.</param>
		/// <param name="noiser">True to decrease dithering patterns (slower of course).</param>
		/// <param name="palette">The palette of colors to use.</param>
		public static void QuantizeDither(this PixelWorker source, bool noiser = false, params Color[] palette) {
			if (source == null || palette == null || palette.Length == 0)
				return;
			using (PixelWorker result = new PixelWorker(source.Width, source.Height, source.ComponentCount)) {
				int width = source.Width, height = source.Height;
				Color currentPixel, bestColor, temp;
				int errorA, errorR, errorG, errorB, tempX, yPlusOne, x, random;
				for (int y = 0; y < height; y++) {
					for (x = 0; x < width; x++) {
						currentPixel = source.GetPixel(x, y);
						bestColor = GetNearestColorInner(currentPixel, palette);
						result.SetPixel(x, y, bestColor);
						errorR = currentPixel.R - bestColor.R;
						errorG = currentPixel.G - bestColor.G;
						errorB = currentPixel.B - bestColor.B;
						errorA = currentPixel.A - bestColor.A;
						if (noiser) {
							random = (int) UniformRandom.Random.ToInterval(-30.0, 30.0);
							errorR += random;
							errorG += random;
							errorB += random;
							errorA += random;
						}
						tempX = x + 1;
						if (tempX < width) {
							temp = source.GetPixel(tempX, y);
							source.SetPixel(tempX, y, Color.FromArgb(Clamp(temp.A + ((errorA * 7) >> 4)), Clamp(temp.R + ((errorR * 7) >> 4)),
								Clamp(temp.G + ((errorG * 7) >> 4)), Clamp(temp.B + ((errorB * 7) >> 4))));
						}
						yPlusOne = y + 1;
						if (yPlusOne < height) {
							if (x != 0) {
								tempX = x - 1;
								temp = source.GetPixel(tempX, yPlusOne);
								source.SetPixel(tempX, yPlusOne, Color.FromArgb(Clamp(temp.A + ((errorA * 3) >> 4)), Clamp(temp.R + ((errorR * 3) >> 4)),
									Clamp(temp.G + ((errorG * 3) >> 4)), Clamp(temp.B + ((errorB * 3) >> 4))));
							}
							temp = source.GetPixel(x, yPlusOne);
							source.SetPixel(x, yPlusOne, Color.FromArgb(Clamp(temp.A + ((errorA * 5) >> 4)), Clamp(temp.R + ((errorR * 5) >> 4)),
								Clamp(temp.G + ((errorG * 5) >> 4)), Clamp(temp.B + ((errorB * 5) >> 4))));
							tempX = x + 1;
							if (tempX < width) {
								temp = source.GetPixel(tempX, yPlusOne);
								source.SetPixel(tempX, yPlusOne, Color.FromArgb(Clamp(temp.A + (errorA >> 4)), Clamp(temp.R + (errorR >> 4)),
									Clamp(temp.G + (errorG >> 4)), Clamp(temp.B + (errorB >> 4))));
							}
						}
					}
				}
				source.CopyFrom(result);
			}
		}

		/// <summary>
		/// Interpolates the specified values linearly.
		/// </summary>
		/// <param name="x1y1">The top-left pixel.</param>
		/// <param name="x2y1">The top-right pixel.</param>
		/// <param name="x1y2">The bottom-left pixel.</param>
		/// <param name="x2y2">The bottom-right pixel.</param>
		/// <param name="x">A value between 0 and 1 that reflects the X-coordinate of the output pixel relative to x1y1 which is (0, 0).</param>
		/// <param name="y">A value between 0 and 1 that reflects the Y-coordinate of the output pixel relative to x1y1 which is (0, 0).</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static byte InterpolateLinear(byte x1y1, byte x2y1, byte x1y2, byte x2y2, float x, float y) {
			float oneMinusX = 1f - x;
			float firstMidPoint = oneMinusX * x1y1 + x * x2y1;
			float secondMidPoint = oneMinusX * x1y2 + x * x2y2;
			return Clamp((1f - y) * firstMidPoint + y * secondMidPoint);
		}

		/// <summary>
		/// Interpolates the specified colors linearly.
		/// </summary>
		/// <param name="x1y1">The top-left pixel.</param>
		/// <param name="x2y1">The top-right pixel.</param>
		/// <param name="x1y2">The bottom-left pixel.</param>
		/// <param name="x2y2">The bottom-right pixel.</param>
		/// <param name="x">A value between 0 and 1 that reflects the X-coordinate of the output pixel relative to x1y1 which is (0, 0).</param>
		/// <param name="y">A value between 0 and 1 that reflects the Y-coordinate of the output pixel relative to x1y1 which is (0, 0).</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Color InterpolateLinear(Color x1y1, Color x2y1, Color x1y2, Color x2y2, float x, float y) {
			float oneMinusX = 1f - x;
			Vector4 firstMidPoint = new Vector4(oneMinusX * x1y1.A + x * x2y1.A, oneMinusX * x1y1.R + x * x2y1.R, oneMinusX * x1y1.G + x * x2y1.G, oneMinusX * x1y1.B + x * x2y1.B);
			Vector4 secondMidPoint = new Vector4(oneMinusX * x1y2.A + x * x2y2.A, oneMinusX * x1y2.R + x * x2y2.R, oneMinusX * x1y2.G + x * x2y2.G, oneMinusX * x1y2.B + x * x2y2.B);
			firstMidPoint = (1f - y) * firstMidPoint + y * secondMidPoint;
			return Color.FromArgb(Clamp(firstMidPoint.X), Clamp(firstMidPoint.Y), Clamp(firstMidPoint.Z), Clamp(firstMidPoint.W));
		}

		/// <summary>
		/// Interpolates the specified colors linearly.
		/// </summary>
		/// <param name="x1y1">The top-left pixel.</param>
		/// <param name="x2y1">The top-right pixel.</param>
		/// <param name="x1y2">The bottom-left pixel.</param>
		/// <param name="x2y2">The bottom-right pixel.</param>
		/// <param name="x">A value between 0 and 1 that reflects the X-coordinate of the output pixel relative to x1y1 which is (0, 0).</param>
		/// <param name="y">A value between 0 and 1 that reflects the Y-coordinate of the output pixel relative to x1y1 which is (0, 0).</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static BgraColor InterpolateLinear(BgraColor x1y1, BgraColor x2y1, BgraColor x1y2, BgraColor x2y2, float x, float y) {
			float oneMinusX = 1f - x;
			Vector4 firstMidPoint = new Vector4(oneMinusX * x1y1.A + x * x2y1.A, oneMinusX * x1y1.R + x * x2y1.R, oneMinusX * x1y1.G + x * x2y1.G, oneMinusX * x1y1.B + x * x2y1.B);
			Vector4 secondMidPoint = new Vector4(oneMinusX * x1y2.A + x * x2y2.A, oneMinusX * x1y2.R + x * x2y2.R, oneMinusX * x1y2.G + x * x2y2.G, oneMinusX * x1y2.B + x * x2y2.B);
			firstMidPoint = (1f - y) * firstMidPoint + y * secondMidPoint;
			return new BgraColor(Clamp(firstMidPoint.X), Clamp(firstMidPoint.Y), Clamp(firstMidPoint.Z), Clamp(firstMidPoint.W));
		}

		/// <summary>
		/// Interpolates the specified values cubically.
		/// </summary>
		/// <param name="x1y1">The top-left pixel at (-1, -1).</param>
		/// <param name="x2y1">x2y1</param>
		/// <param name="x3y1">x3y1</param>
		/// <param name="x4y1">x4y1</param>
		/// <param name="x1y2">x1y2</param>
		/// <param name="x2y2">x2y2</param>
		/// <param name="x3y2">x3y2</param>
		/// <param name="x4y2">x4y2</param>
		/// <param name="x1y3">x1y3</param>
		/// <param name="x2y3">x2y3</param>
		/// <param name="x3y3">x3y3</param>
		/// <param name="x4y3">x4y3</param>
		/// <param name="x1y4">x1y4</param>
		/// <param name="x2y4">x2y4</param>
		/// <param name="x3y4">x3y4</param>
		/// <param name="x4y4">The bottom-right pixel at (2, 2).</param>
		/// <param name="x">A value from -1 to 2 that reflects the X-coordinate of the output pixel relative to x1y1 which is (-1, -1).
		/// 0 to 1 refer to the middle pixels.</param>
		/// <param name="y">A value from -1 to 2 that reflects the Y-coordinate of the output pixel relative to x1y1 which is (-1, -1).
		/// 0 to 1 refer to the middle pixels.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static byte InterpolateCubic(byte x1y1, byte x2y1, byte x3y1, byte x4y1,
													byte x1y2, byte x2y2, byte x3y2, byte x4y2,
													byte x1y3, byte x2y3, byte x3y3, byte x4y3,
													byte x1y4, byte x2y4, byte x3y4, byte x4y4, double x, double y) {
			double p0 = x2y1 + 0.5 * x * (x3y1 - x1y1 + x * (2.0 * x1y1 - 5.0 * x2y1 + 4.0 * x3y1 - x4y1 + x * (3.0 * (x2y1 - x3y1) + x4y1 - x1y1)));
			double p1 = x2y2 + 0.5 * x * (x3y2 - x1y2 + x * (2.0 * x1y2 - 5.0 * x2y2 + 4.0 * x3y2 - x4y2 + x * (3.0 * (x2y2 - x3y2) + x4y2 - x1y2)));
			double p2 = x2y3 + 0.5 * x * (x3y3 - x1y3 + x * (2.0 * x1y3 - 5.0 * x2y3 + 4.0 * x3y3 - x4y3 + x * (3.0 * (x2y3 - x3y3) + x4y3 - x1y3)));
			double p3 = x2y4 + 0.5 * x * (x3y4 - x1y4 + x * (2.0 * x1y4 - 5.0 * x2y4 + 4.0 * x3y4 - x4y4 + x * (3.0 * (x2y4 - x3y4) + x4y4 - x1y4)));
			return Clamp(p1 + 0.5 * y * (p2 - p0 + y * (2.0 * p0 - 5.0 * p1 + 4.0 * p2 - p3 + y * (3.0 * (p1 - p2) + p3 - p0))));
		}

		/// <summary>
		/// Interpolates the specified values cubically.
		/// </summary>
		/// <param name="x1y1">The top-left pixel at (-1, -1).</param>
		/// <param name="x2y1">x2y1</param>
		/// <param name="x3y1">x3y1</param>
		/// <param name="x4y1">x4y1</param>
		/// <param name="x1y2">x1y2</param>
		/// <param name="x2y2">x2y2</param>
		/// <param name="x3y2">x3y2</param>
		/// <param name="x4y2">x4y2</param>
		/// <param name="x1y3">x1y3</param>
		/// <param name="x2y3">x2y3</param>
		/// <param name="x3y3">x3y3</param>
		/// <param name="x4y3">x4y3</param>
		/// <param name="x1y4">x1y4</param>
		/// <param name="x2y4">x2y4</param>
		/// <param name="x3y4">x3y4</param>
		/// <param name="x4y4">The bottom-right pixel at (2, 2).</param>
		/// <param name="x">A value from -1 to 2 that reflects the X-coordinate of the output pixel relative to x1y1 which is (-1, -1).
		/// 0 to 1 refer to the middle pixels.</param>
		/// <param name="y">A value from -1 to 2 that reflects the Y-coordinate of the output pixel relative to x1y1 which is (-1, -1).
		/// 0 to 1 refer to the middle pixels.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static float InterpolateCubic(float x1y1, float x2y1, float x3y1, float x4y1,
													float x1y2, float x2y2, float x3y2, float x4y2,
													float x1y3, float x2y3, float x3y3, float x4y3,
													float x1y4, float x2y4, float x3y4, float x4y4, float x, float y) {
			float p0 = x2y1 + 0.5f * x * (x3y1 - x1y1 + x * (2f * x1y1 - 5f * x2y1 + 4f * x3y1 - x4y1 + x * (3f * (x2y1 - x3y1) + x4y1 - x1y1)));
			float p1 = x2y2 + 0.5f * x * (x3y2 - x1y2 + x * (2f * x1y2 - 5f * x2y2 + 4f * x3y2 - x4y2 + x * (3f * (x2y2 - x3y2) + x4y2 - x1y2)));
			float p2 = x2y3 + 0.5f * x * (x3y3 - x1y3 + x * (2f * x1y3 - 5f * x2y3 + 4f * x3y3 - x4y3 + x * (3f * (x2y3 - x3y3) + x4y3 - x1y3)));
			float p3 = x2y4 + 0.5f * x * (x3y4 - x1y4 + x * (2f * x1y4 - 5f * x2y4 + 4f * x3y4 - x4y4 + x * (3f * (x2y4 - x3y4) + x4y4 - x1y4)));
			return p1 + 0.5f * y * (p2 - p0 + y * (2f * p0 - 5f * p1 + 4f * p2 - p3 + y * (3f * (p1 - p2) + p3 - p0)));
		}

		/// <summary>
		/// Interpolates the specified values cubically.
		/// </summary>
		/// <param name="p0">[sample: -1] The left neighboring value.</param>
		/// <param name="p1">[sample: 0] The value on the left to interpolate</param>
		/// <param name="p2">[sample: 1] The value on the right to interpolate</param>
		/// <param name="p3">[sample: 2] The right neighboring value.</param>
		/// <param name="sample">A value from -1 to 2 that reflects the coordinate of the output pixel relative to p1.
		/// 0 to 1 refer to the middle pixels.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static float InterpolateCubic(float p0, float p1, float p2, float p3, float sample) {
			return p1 + 0.5f * sample * (p2 - p0 + sample * (2f * p0 - 5f * p1 + 4f * p2 - p3 + sample * (3f * (p1 - p2) + p3 - p0)));
		}

		/// <summary>
		/// Interpolates the specified values cubically.
		/// </summary>
		/// <param name="p0">[sample: -1] The left neighboring value.</param>
		/// <param name="p1">[sample: 0] The value on the left to interpolate</param>
		/// <param name="p2">[sample: 1] The value on the right to interpolate</param>
		/// <param name="p3">[sample: 2] The right neighboring value.</param>
		/// <param name="sample">A value from -1 to 2 that reflects the coordinate of the output pixel relative to p1.
		/// 0 to 1 refer to the middle pixels.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static ComplexF InterpolateCubic(ComplexF p0, ComplexF p1, ComplexF p2, ComplexF p3, float sample) {
			return p1 + 0.5f * sample * (p2 - p0 + sample * (2f * p0 - 5f * p1 + 4f * p2 - p3 + sample * (3f * (p1 - p2) + p3 - p0)));
		}

		/// <summary>
		/// Interpolates the specified pixels cubically.
		/// </summary>
		/// <param name="x1y1">The top-left pixel at (-1, -1).</param>
		/// <param name="x2y1">x2y1</param>
		/// <param name="x3y1">x3y1</param>
		/// <param name="x4y1">x4y1</param>
		/// <param name="x1y2">x1y2</param>
		/// <param name="x2y2">x2y2</param>
		/// <param name="x3y2">x3y2</param>
		/// <param name="x4y2">x4y2</param>
		/// <param name="x1y3">x1y3</param>
		/// <param name="x2y3">x2y3</param>
		/// <param name="x3y3">x3y3</param>
		/// <param name="x4y3">x4y3</param>
		/// <param name="x1y4">x1y4</param>
		/// <param name="x2y4">x2y4</param>
		/// <param name="x3y4">x3y4</param>
		/// <param name="x4y4">The bottom-right pixel at (2, 2).</param>
		/// <param name="x">A value from -1 to 2 that reflects the X-coordinate of the output pixel relative to x1y1 which is (-1, -1).
		/// 0 to 1 refer to the middle pixels.</param>
		/// <param name="y">A value from -1 to 2 that reflects the Y-coordinate of the output pixel relative to x1y1 which is (-1, -1).
		/// 0 to 1 refer to the middle pixels.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static BgraColor InterpolateCubic(BgraColor x1y1, BgraColor x2y1, BgraColor x3y1, BgraColor x4y1,
													BgraColor x1y2, BgraColor x2y2, BgraColor x3y2, BgraColor x4y2,
													BgraColor x1y3, BgraColor x2y3, BgraColor x3y3, BgraColor x4y3,
													BgraColor x1y4, BgraColor x2y4, BgraColor x3y4, BgraColor x4y4, double x, double y) {
			return new BgraColor(InterpolateCubic(x1y1.A, x2y1.A, x3y1.A, x4y1.A, x1y2.A, x2y2.A, x3y2.A, x4y2.A, x1y3.A, x2y3.A, x3y3.A, x4y3.A, x1y4.A, x2y4.A, x3y4.A, x4y4.A, x, y),
				InterpolateCubic(x1y1.R, x2y1.R, x3y1.R, x4y1.R, x1y2.R, x2y2.R, x3y2.R, x4y2.R, x1y3.R, x2y3.R, x3y3.R, x4y3.R, x1y4.R, x2y4.R, x3y4.R, x4y4.R, x, y),
				InterpolateCubic(x1y1.G, x2y1.G, x3y1.G, x4y1.G, x1y2.G, x2y2.G, x3y2.G, x4y2.G, x1y3.G, x2y3.G, x3y3.G, x4y3.G, x1y4.G, x2y4.G, x3y4.G, x4y4.G, x, y),
				InterpolateCubic(x1y1.B, x2y1.B, x3y1.B, x4y1.B, x1y2.B, x2y2.B, x3y2.B, x4y2.B, x1y3.B, x2y3.B, x3y3.B, x4y3.B, x1y4.B, x2y4.B, x3y4.B, x4y4.B, x, y));
		}

		/// <summary>
		/// Interpolates the specified pixels cubically.
		/// </summary>
		/// <param name="x1y1">The top-left pixel at (-1, -1).</param>
		/// <param name="x2y1">x2y1</param>
		/// <param name="x3y1">x3y1</param>
		/// <param name="x4y1">x4y1</param>
		/// <param name="x1y2">x1y2</param>
		/// <param name="x2y2">x2y2</param>
		/// <param name="x3y2">x3y2</param>
		/// <param name="x4y2">x4y2</param>
		/// <param name="x1y3">x1y3</param>
		/// <param name="x2y3">x2y3</param>
		/// <param name="x3y3">x3y3</param>
		/// <param name="x4y3">x4y3</param>
		/// <param name="x1y4">x1y4</param>
		/// <param name="x2y4">x2y4</param>
		/// <param name="x3y4">x3y4</param>
		/// <param name="x4y4">The bottom-right pixel at (2, 2).</param>
		/// <param name="x">A value from -1 to 2 that reflects the X-coordinate of the output pixel relative to x1y1 which is (-1, -1).
		/// 0 to 1 refer to the middle pixels.</param>
		/// <param name="y">A value from -1 to 2 that reflects the Y-coordinate of the output pixel relative to x1y1 which is (-1, -1).
		/// 0 to 1 refer to the middle pixels.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Color InterpolateCubic(Color x1y1, Color x2y1, Color x3y1, Color x4y1,
													Color x1y2, Color x2y2, Color x3y2, Color x4y2,
													Color x1y3, Color x2y3, Color x3y3, Color x4y3,
													Color x1y4, Color x2y4, Color x3y4, Color x4y4, double x, double y) {
			return Color.FromArgb(InterpolateCubic(x1y1.A, x2y1.A, x3y1.A, x4y1.A, x1y2.A, x2y2.A, x3y2.A, x4y2.A, x1y3.A, x2y3.A, x3y3.A, x4y3.A, x1y4.A, x2y4.A, x3y4.A, x4y4.A, x, y),
				InterpolateCubic(x1y1.R, x2y1.R, x3y1.R, x4y1.R, x1y2.R, x2y2.R, x3y2.R, x4y2.R, x1y3.R, x2y3.R, x3y3.R, x4y3.R, x1y4.R, x2y4.R, x3y4.R, x4y4.R, x, y),
				InterpolateCubic(x1y1.G, x2y1.G, x3y1.G, x4y1.G, x1y2.G, x2y2.G, x3y2.G, x4y2.G, x1y3.G, x2y3.G, x3y3.G, x4y3.G, x1y4.G, x2y4.G, x3y4.G, x4y4.G, x, y),
				InterpolateCubic(x1y1.B, x2y1.B, x3y1.B, x4y1.B, x1y2.B, x2y2.B, x3y2.B, x4y2.B, x1y3.B, x2y3.B, x3y3.B, x4y3.B, x1y4.B, x2y4.B, x3y4.B, x4y4.B, x, y));
		}

		/// <summary>
		/// Crops the given image to the specified rectangle.
		/// </summary>
		/// <param name="source">The source image to crop.</param>
		/// <param name="section">The section to keep.</param>
		public static Bitmap Crop(this Bitmap source, Rectangle section) {
			Bitmap bmp = new Bitmap(section.Width, section.Height, source.PixelFormat);
			using (Graphics g = Graphics.FromImage(bmp)) {
				g.PixelOffsetMode = PixelOffsetMode.None;
				g.CompositingMode = CompositingMode.SourceCopy;
				g.InterpolationMode = InterpolationMode.NearestNeighbor;
				g.DrawImage(source, 0, 0, section, GraphicsUnit.Pixel);
			}
			return bmp;
		}

		/// <summary>
		/// Changes the brightness of the specified color.
		/// </summary>
		/// <param name="color">The color whose brightness to change.</param>
		/// <param name="multiplier">The brightness multiplier (0 means black, 1 means brightness is unchanged, larger numbers increase brightness).</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Color ChangeBrightness(this Color color, float multiplier) {
			return Color.FromArgb(color.A, Clamp(color.R * multiplier), Clamp(color.G * multiplier), Clamp(color.B * multiplier));
		}

		/// <summary>
		/// Changes the brightness of the specified color.
		/// </summary>
		/// <param name="color">The color whose brightness to change.</param>
		/// <param name="multiplier">The brightness multiplier (0 means black, 1 means brightness is unchanged, larger numbers increase brightness).</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static BgraColor ChangeBrightness(this BgraColor color, float multiplier) {
			return new BgraColor(color.A, Clamp(color.R * multiplier), Clamp(color.G * multiplier), Clamp(color.B * multiplier));
		}

		/// <summary>
		/// Multiplies the specified color with the specified weight including alpha components.
		/// If you don't want to add alpha components, use ChangeBrightness instead.
		/// </summary>
		/// <param name="color">The color to multiply.</param>
		/// <param name="multiplier">The weight to multiply by.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Color Multiply(this Color color, float multiplier) {
			return Color.FromArgb(Clamp(color.A * multiplier), Clamp(color.R * multiplier), Clamp(color.G * multiplier), Clamp(color.B * multiplier));
		}

		/// <summary>
		/// Multiplies the specified color with the specified weight including alpha components.
		/// If you don't want to add alpha components, use ChangeBrightness instead.
		/// </summary>
		/// <param name="color">The color to multiply.</param>
		/// <param name="multiplier">The weight to multiply by.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static BgraColor Multiply(this BgraColor color, float multiplier) {
			return new BgraColor(Clamp(color.A * multiplier), Clamp(color.R * multiplier), Clamp(color.G * multiplier), Clamp(color.B * multiplier));
		}

		/// <summary>
		/// Changes the lightness of the specified color.
		/// </summary>
		/// <param name="color">The color whose lightness to change.</param>
		/// <param name="offset">The color offset.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Color ChangeLightness(this Color color, int offset) {
			return Color.FromArgb(color.A, Clamp(color.R + offset), Clamp(color.G + offset), Clamp(color.B + offset));
		}

		/// <summary>
		/// Changes the lightness of the specified color.
		/// </summary>
		/// <param name="color">The color whose lightness to change.</param>
		/// <param name="offset">The color offset.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static BgraColor ChangeLightness(this BgraColor color, int offset) {
			return new BgraColor(color.A, Clamp(color.R + offset), Clamp(color.G + offset), Clamp(color.B + offset));
		}

		/// <summary>
		/// Pre-multiplies alpha with the color values.
		/// </summary>
		/// <param name="color">The color whose alpha to premultiply.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Color PremultiplyAlpha(this Color color) {
			float opacity = squareRoots[color.A];
			return Color.FromArgb((byte) (opacity * 255.9999f), (byte) (color.R * opacity), (byte) (color.G * opacity), (byte) (color.B * opacity));
		}

		/// <summary>
		/// Pre-multiplies alpha with the color values.
		/// </summary>
		/// <param name="color">The color whose alpha to premultiply.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static BgraColor PremultiplyAlpha(this BgraColor color) {
			float opacity = squareRoots[color.A];
			return new BgraColor((byte) (opacity * 255.9999f), (byte) (color.R * opacity), (byte) (color.G * opacity), (byte) (color.B * opacity));
		}

		/// <summary>
		/// Inverts the specified color.
		/// </summary>
		/// <param name="color">The color to invert.</param>
		/// <param name="ignoreAlpha">If true, alpha is left intact.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Color Invert(this Color color, bool ignoreAlpha = true) {
			return Color.FromArgb(ignoreAlpha ? color.A : ~color.A, ~color.R, ~color.G, ~color.B);
		}

		/// <summary>
		/// Inverts the specified color.
		/// </summary>
		/// <param name="color">The color to invert.</param>
		/// <param name="ignoreAlpha">If true, alpha is left intact.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static BgraColor Invert(this BgraColor color, bool ignoreAlpha = true) {
			return new BgraColor(ignoreAlpha ? color.A : (byte) ~color.A, (byte) ~color.R, (byte) ~color.G, (byte) ~color.B);
		}

		/// <summary>
		/// Applies bitwise 'and' to the colors.
		/// </summary>
		/// <param name="color1">The first color.</param>
		/// <param name="color2">The second color.</param>
		/// <param name="ignoreAlpha">If true, the alpha of color2 will be ignored.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Color BitwiseAnd(this Color color1, Color color2, bool ignoreAlpha = true) {
			return Color.FromArgb(ignoreAlpha ? color1.A : (color1.A & color2.A), (color1.R & color2.R), (color1.G & color2.G), (color1.B & color2.B));
		}

		/// <summary>
		/// Applies bitwise 'and' to the colors.
		/// </summary>
		/// <param name="color1">The first color.</param>
		/// <param name="color2">The second color.</param>
		/// <param name="ignoreAlpha">If true, the alpha of color2 will be ignored.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static BgraColor BitwiseAnd(this BgraColor color1, BgraColor color2, bool ignoreAlpha = true) {
			return new BgraColor(ignoreAlpha ? color1.A : (byte) (color1.A & color2.A), (byte) (color1.R & color2.R), (byte) (color1.G & color2.G), (byte) (color1.B & color2.B));
		}

		/// <summary>
		/// Applies a bitwise 'and' mask to the color.
		/// </summary>
		/// <param name="color">The color.</param>
		/// <param name="mask">The mask to 'and' with.</param>
		/// <param name="ignoreAlpha">If true, the alpha channel will not be masked.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Color BitwiseAnd(this Color color, byte mask, bool ignoreAlpha = true) {
			return Color.FromArgb(ignoreAlpha ? color.A : (color.A & mask), (color.R & mask), (color.G & mask), (color.B & mask));
		}

		/// <summary>
		/// Applies a bitwise 'and' mask to the color.
		/// </summary>
		/// <param name="color">The color.</param>
		/// <param name="mask">The mask to 'and' with.</param>
		/// <param name="ignoreAlpha">If true, the alpha channel will not be masked.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static BgraColor BitwiseAnd(this BgraColor color, byte mask, bool ignoreAlpha = true) {
			return new BgraColor(ignoreAlpha ? color.A : (byte) (color.A & mask), (byte) (color.R & mask), (byte) (color.G & mask), (byte) (color.B & mask));
		}

		/// <summary>
		/// Applies bitwise 'or' to the colors.
		/// </summary>
		/// <param name="color1">The first color.</param>
		/// <param name="color2">The second color.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Color BitwiseOr(this Color color1, Color color2) {
			return Color.FromArgb((color1.A | color2.A), (color1.R | color2.R), (color1.G | color2.G), (color1.B | color2.B));
		}

		/// <summary>
		/// Applies bitwise 'or' to the colors.
		/// </summary>
		/// <param name="color1">The first color.</param>
		/// <param name="color2">The second color.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static BgraColor BitwiseOr(this BgraColor color1, BgraColor color2) {
			return new BgraColor((byte) (color1.A | color2.A), (byte) (color1.R | color2.R), (byte) (color1.G | color2.G), (byte) (color1.B | color2.B));
		}

		/// <summary>
		/// Applies a bitwise 'or' mask to the color.
		/// </summary>
		/// <param name="color">The color.</param>
		/// <param name="mask">The mask to 'or' with.</param>
		/// <param name="ignoreAlpha">If true, the alpha channel will not be masked.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Color BitwiseOr(this Color color, byte mask, bool ignoreAlpha = true) {
			return Color.FromArgb(ignoreAlpha ? color.A : (color.A | mask), (color.R | mask), (color.G | mask), (color.B | mask));
		}

		/// <summary>
		/// Applies a bitwise 'or' mask to the color.
		/// </summary>
		/// <param name="color">The color.</param>
		/// <param name="mask">The mask to 'or' with.</param>
		/// <param name="ignoreAlpha">If true, the alpha channel will not be masked.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static BgraColor BitwiseOr(this BgraColor color, byte mask, bool ignoreAlpha = true) {
			return new BgraColor(ignoreAlpha ? color.A : (byte) (color.A | mask), (byte) (color.R | mask), (byte) (color.G | mask), (byte) (color.B | mask));
		}

		/// <summary>
		/// Applies bitwise 'xor' to the colors.
		/// </summary>
		/// <param name="color1">The first color.</param>
		/// <param name="color2">The second color.</param>
		/// <param name="ignoreAlpha">If true, the alpha of color2 will be ignored.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Color BitwiseXor(this Color color1, Color color2, bool ignoreAlpha = true) {
			return Color.FromArgb(ignoreAlpha ? color1.A : (color1.A ^ color2.A), (color1.R ^ color2.R), (color1.G ^ color2.G), (color1.B ^ color2.B));
		}

		/// <summary>
		/// Applies bitwise 'xor' to the colors.
		/// </summary>
		/// <param name="color1">The first color.</param>
		/// <param name="color2">The second color.</param>
		/// <param name="ignoreAlpha">If true, the alpha of color2 will be ignored.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static BgraColor BitwiseXor(this BgraColor color1, BgraColor color2, bool ignoreAlpha = true) {
			return new BgraColor(ignoreAlpha ? color1.A : (byte) (color1.A ^ color2.A), (byte) (color1.R ^ color2.R), (byte) (color1.G ^ color2.G), (byte) (color1.B ^ color2.B));
		}

		/// <summary>
		/// Applies a bitwise 'xor' mask to the color.
		/// </summary>
		/// <param name="color">The color.</param>
		/// <param name="mask">The mask to 'xor' with.</param>
		/// <param name="ignoreAlpha">If true, the alpha channel will not be masked.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Color BitwiseXor(this Color color, byte mask, bool ignoreAlpha = true) {
			return Color.FromArgb(ignoreAlpha ? color.A : (color.A ^ mask), (color.R ^ mask), (color.G ^ mask), (color.B ^ mask));
		}

		/// <summary>
		/// Applies a bitwise 'xor' mask to the color.
		/// </summary>
		/// <param name="color">The color.</param>
		/// <param name="mask">The mask to 'xor' with.</param>
		/// <param name="ignoreAlpha">If true, the alpha channel will not be masked.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static BgraColor BitwiseXor(this BgraColor color, byte mask, bool ignoreAlpha = true) {
			return new BgraColor(ignoreAlpha ? color.A : (byte) (color.A ^ mask), (byte) (color.R ^ mask), (byte) (color.G ^ mask), (byte) (color.B ^ mask));
		}

		/// <summary>
		/// Clamps the value to byte range.
		/// </summary>
		/// <param name="value">The value to clamp.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static byte Clamp(float value) {
			return value < 255f ? (value > 0f ? (byte) value : (byte) 0) : (byte) 255;
		}

		/// <summary>
		/// Clamps the value to byte range.
		/// </summary>
		/// <param name="value">The value to clamp.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static byte Clamp(double value) {
			return value < 255.0 ? (value > 0.0 ? (byte) value : (byte) 0) : (byte) 255;
		}

		/// <summary>
		/// Clamps the value to byte range.
		/// </summary>
		/// <param name="value">The value to clamp.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static double ClampDouble(double value) {
			return value < 255 ? (value > 0.0 ? value : 0.0) : 255;
		}

		/// <summary>
		/// Clamps the value to byte range.
		/// </summary>
		/// <param name="value">The value to clamp.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static byte Clamp(int value) {
			return value < 255 ? (value > 0 ? (byte) value : (byte) 0) : (byte) 255;
		}

		/// <summary>
		/// Gets the corresponding power-of-2 size for the specified image size.
		/// </summary>
		/// <param name="size">The bitmap size.</param>
		/// <param name="roundUp">Whether to round up (upscale) or down (downscale).</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Size GetPowerOfTwoSize(Size size, bool roundUp) {
			if (roundUp)
				return new Size((int) Maths.CeilingPowerOfTwo((uint) size.Width), (int) Maths.CeilingPowerOfTwo((uint) size.Height));
			else
				return new Size((int) Maths.FloorPowerOfTwo((uint) size.Width), (int) Maths.FloorPowerOfTwo((uint) size.Height));
		}

		/// <summary>
		/// Creates a Color from alpha, hue, saturation and brightness.
		/// </summary>
		/// <param name="alpha">The alpha channel value (0 to 255).</param>
		/// <param name="hue">The hue value (0 to 360).</param>
		/// <param name="saturation">The saturation value (0 to 1).</param>
		/// <param name="brightness">The brightness value (0 to 1).</param>
		public static Color FromAhsb(int alpha, float hue, float saturation, float brightness) {
			if (saturation <= 0)
				return Color.FromArgb(alpha, (int) (brightness * 255), (int) (brightness * 255), (int) (brightness * 255));
			float fMax, fMin;
			if (brightness > 0.5) {
				fMax = brightness - (brightness * saturation) + saturation;
				fMin = brightness + (brightness * saturation) - saturation;
			} else {
				fMax = brightness + (brightness * saturation);
				fMin = brightness - (brightness * saturation);
			}
			int iSextant = (int) Math.Floor(hue / 60f);
			if (300f <= hue)
				hue -= 360f;
			hue /= 60f;
			hue -= 2f * (float) Math.Floor(((iSextant + 1f) % 6f) / 2f);
			int iMax = (int) (fMax * 255);
			int iMid = (int) (((iSextant & 1) == 0 ? hue * (fMax - fMin) + fMin : fMin - hue * (fMax - fMin)) * 255);
			int iMin = (int) (fMin * 255);
			switch (iSextant) {
				case 1:
					return Color.FromArgb(alpha, iMid, iMax, iMin);
				case 2:
					return Color.FromArgb(alpha, iMin, iMax, iMid);
				case 3:
					return Color.FromArgb(alpha, iMin, iMid, iMax);
				case 4:
					return Color.FromArgb(alpha, iMid, iMin, iMax);
				case 5:
					return Color.FromArgb(alpha, iMax, iMin, iMid);
				default:
					return Color.FromArgb(alpha, iMax, iMid, iMin);
			}
		}

		/// <summary>
		/// Gets a 2D Gaussian kernel of the specified size.
		/// </summary>
		/// <param name="width">The width of the kernel.</param>
		/// <param name="height">The height of the kernel.</param>
		/// <param name="sigma">The weight to use.</param>
		public static float[][] GaussianKernel(int width, int height, double sigma = 1.0) {
			float[][] kernel = new float[width][];
			int i;
			for (i = 0; i < width; i++)
				kernel[i] = new float[height];
			sigma = 1.0 / (2.0 * sigma * sigma);
			float current, total = 0f;
			int x, y, j;
			for (y = -height / 2, j = 0; j < height; y++, j++) {
				for (x = -width / 2, i = 0; i < width; x++, i++) {
					current = (float) Math.Exp(-(x * x + y * y) * sigma);
					total += current;
					kernel[i][j] = current;
				}
			}
			total = 1f / total;
			ParallelLoop.For(0, width, X => {
				for (int Y = 0; Y < height; Y++)
					kernel[X][Y] *= total;
			}, ParallelCutoff);
			return kernel;
		}

		/// <summary>
		/// Gets a 2D Gaussian sharpen kernel of the specified size.
		/// </summary>
		/// <param name="width">The width of the kernel.</param>
		/// <param name="height">The height of the kernel.</param>
		/// <param name="sigma">The weight to use.</param>
		public static float[][] SharpenKernel(int width, int height, double sigma = 1.0) {
			float[][] kernel = GaussianKernel(width, height, sigma);
			if (kernel.Length == 0 || kernel[0].Length == 0)
				return kernel;
			float mult = 1f / kernel[0][0];
			if (mult == float.MaxValue || mult == float.PositiveInfinity)
				mult = int.MaxValue;
			float sum = 0f;
			int x, y;
			float[] temp;
			for (x = 0; x < width; x++) {
				temp = kernel[x];
				for (y = 0; y < height; y++) {
					temp[y] *= mult;
					sum += temp[y];
				}
			}
			int widthDiv2 = width / 2, heightDiv2 = height / 2;
			ParallelLoop.For(0, width, X => {
				float[] t = kernel[X];
				for (int Y = 0; Y < height; Y++) {
					if (X == widthDiv2 && Y == heightDiv2)
						t[Y] = 2f * sum - t[Y];
					else
						t[Y] = -t[Y];
				}
			}, ParallelCutoff);
			return kernel;
		}

		/// <summary>
		/// Normalizes the colors of an image.
		/// </summary>
		/// <param name="image">The image whose colors to normalize.</param>
		/// <param name="normalizeAlpha">Whether to include alpha in the normalization process.</param>
		public static void Normalize(this Bitmap image, bool normalizeAlpha = false) {
			using (PixelWorker source = PixelWorker.FromImage(image, false, true, ImageParameterAction.RemoveReference))
				Normalize(source, normalizeAlpha);
		}

		/// <summary>
		/// Normalizes the colors of an image.
		/// </summary>
		/// <param name="source">The image whose colors to normalize. WriteChanges() is not called, that's on you.</param>
		/// <param name="normalizeAlpha">Whether to include alpha in the normalization process.</param>
		public static void Normalize(this PixelWorker source, bool normalizeAlpha = false) {
			if (!normalizeAlpha && source.ComponentCount == 4) {
				if (source.Buffer == null) {
					unsafe
					{
						byte current, max = 0, min = source.Scan0[0];
						int count = source.PixelComponentCount;
						for (int i = 0; i < count; i++) {
							if (i % 4 != 3) {
								current = source.Scan0[i];
								if (current > max)
									max = current;
								else if (current < min)
									min = current;
							}
						}
						if (!(max == 255 && min == 0)) {
							if (max == min)
								return;
							float multiplier = 255f / (max - min);
							ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
								if (i % 4 != 3) {
									byte* index = source.Scan0 + i;
									*index = (byte) (multiplier * (*index - min));
								}
							});
						}
					}
				} else {
					byte current, max = 0, min = source.Buffer[0];
					int count = source.PixelComponentCount;
					for (int i = 0; i < count; i++) {
						if (i % 4 != 3) {
							current = source.Buffer[i];
							if (current > max)
								max = current;
							else if (current < min)
								min = current;
						}
					}
					if (!(max == 255 && min == 0)) {
						if (max == min)
							return;
						float multiplier = 255f / (max - min);
						ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
							if (i % 4 != 3)
								source.Buffer[i] = (byte) (multiplier * (source.Buffer[i] - min));
						}, ParallelCutoff);
					}
				}
			} else {
				if (source.Buffer == null) {
					unsafe
					{
						byte current, max = 0, min = source.Scan0[0];
						byte* end = source.Scan0 + source.PixelComponentCount;
						for (byte* i = source.Scan0; i < end; i++) {
							current = *i;
							if (current > max)
								max = current;
							else if (current < min)
								min = current;
						}
						if (!(max == 255 && min == 0)) {
							if (max == min)
								return;
							float multiplier = 255f / (max - min);
							ParallelLoop.For(source.Scan0, source.PixelComponentCount, 1L, delegate (IntPtr i) {
								byte* index = (byte*) i;
								*index = (byte) (multiplier * (*index - min));
							}, ParallelCutoff);
						}
					}
				} else {
					byte current, max = 0, min = source.Buffer[0];
					int count = source.PixelComponentCount;
					for (int i = 0; i < count; i++) {
						current = source.Buffer[i];
						if (current > max)
							max = current;
						else if (current < min)
							min = current;
					}
					if (!(max == 255 && min == 0)) {
						if (max == min)
							return;
						float multiplier = 255f / (max - min);
						ParallelLoop.For(0, source.PixelComponentCount, delegate (int i) {
							source.Buffer[i] = (byte) (multiplier * (source.Buffer[i] - min));
						}, ParallelCutoff);
					}
				}
			}
		}

		#region XBR

		/// <summary>
		/// Scales the specified image to double its size using an XBR filter.
		/// </summary>
		/// <param name="image">The image to scale.</param>
		public static Bitmap ApplyXbr2(this Bitmap image) {
			using (PixelWorker worker = PixelWorker.FromImage(image, false, false))
				return ApplyXbr2(worker);
		}

		/// <summary>
		/// Scales the specified image to double its size using an XBR filter.
		/// </summary>
		/// <param name="source">The image to scale.</param>
		public static Bitmap ApplyXbr2(this PixelWorker source) {
			Bitmap resultant = new Bitmap(source.Width * 2, source.Height * 2, source.Format);
			using (PixelWorker worker = PixelWorker.FromImage(resultant, false, false)) {
				ParallelLoop.For(0, source.PixelCount, delegate (int i) {
					int x = i % source.Width;
					int y = i / source.Width;
					Xbr2X(source, x, y, worker, x * 2, y * 2, true);
				});
			}
			return resultant;
		}

		/// <summary>
		/// Scales the specified image to triple its size using an XBR filter.
		/// </summary>
		/// <param name="image">The image to scale.</param>
		public static Bitmap ApplyXbr3(this Bitmap image) {
			using (PixelWorker worker = PixelWorker.FromImage(image, false, false))
				return ApplyXbr3(worker);
		}

		/// <summary>
		/// Scales the specified image to triple its size using an XBR filter.
		/// </summary>
		/// <param name="source">The image to scale.</param>
		public static Bitmap ApplyXbr3(this PixelWorker source) {
			Bitmap resultant = new Bitmap(source.Width * 3, source.Height * 3, source.Format);
			using (PixelWorker worker = PixelWorker.FromImage(resultant, false, false)) {
				ParallelLoop.For(0, source.PixelCount, delegate (int i) {
					int x = i % source.Width;
					int y = i / source.Width;
					Xbr3X(source, x, y, worker, x * 3, y * 3, true);
				});
			}
			return resultant;
		}

		/// <summary>
		/// Scales the specified image to quadruple its size using an XBR filter.
		/// </summary>
		/// <param name="image">The image to scale.</param>
		public static Bitmap ApplyXbr4(this Bitmap image) {
			using (PixelWorker worker = PixelWorker.FromImage(image, false, false))
				return ApplyXbr4(worker);
		}

		/// <summary>
		/// Scales the specified image to quadruple its size using an XBR filter.
		/// </summary>
		/// <param name="source">The image to scale.</param>
		public static Bitmap ApplyXbr4(this PixelWorker source) {
			Bitmap resultant = new Bitmap(source.Width * 4, source.Height * 4, source.Format);
			using (PixelWorker worker = PixelWorker.FromImage(resultant, false, false)) {
				ParallelLoop.For(0, source.PixelCount, delegate (int i) {
					int x = i % source.Width;
					int y = i / source.Width;
					Xbr4X(source, x, y, worker, x * 4, y * 4, true);
				});
			}
			return resultant;
		}

		private static void AlphaBlend128W(ref BgraColor dst, BgraColor src, bool blend) {
			if (blend)
				dst = Interpolate(dst, src);
		}

		private static void AlphaBlend192W(ref BgraColor dst, BgraColor src, bool blend) {
			dst = (blend ? Interpolate(dst, src, 1, 3) : src);
		}

		private static void AlphaBlend224W(ref BgraColor dst, BgraColor src, bool blend) {
			dst = (blend ? Interpolate(dst, src, 1, 7) : src);
		}

		private static void AlphaBlend32W(ref BgraColor dst, BgraColor src, bool blend) {
			if (blend)
				dst = Interpolate(dst, src, 7, 1);
		}

		private static void AlphaBlend64W(ref BgraColor dst, BgraColor src, bool blend) {
			if (blend)
				dst = Interpolate(dst, src, 3, 1);
		}

		private static BgraColor Interpolate(BgraColor pixel1, BgraColor pixel2) {
			return new BgraColor((byte) (pixel1.A + pixel2.A >> 1), (byte) (pixel1.R + pixel2.R >> 1), (byte) (pixel1.G + pixel2.G >> 1), (byte) (pixel1.B + pixel2.B >> 1));
		}

		private static BgraColor Interpolate(BgraColor pixel1, BgraColor pixel2, byte quantifier1, byte quantifier2) {
			ushort num = (ushort) (quantifier1 + quantifier2);
			return new BgraColor((byte) ((pixel1.A * quantifier1 + pixel2.A * quantifier2) / num), (byte) ((pixel1.R * quantifier1 + pixel2.R * quantifier2) / num), (byte) ((pixel1.G * quantifier1 + pixel2.G * quantifier2) / num), (byte) ((pixel1.B * quantifier1 + pixel2.B * quantifier2) / num));
		}

		private static void Dia(out BgraColor n15, ref BgraColor n14, ref BgraColor n11, BgraColor pixel, bool blend) {
			AlphaBlend128W(ref n11, pixel, blend);
			AlphaBlend128W(ref n14, pixel, blend);
			n15 = pixel;
		}

		private static void Dia2X(ref BgraColor n3, BgraColor pixel, bool blend) {
			AlphaBlend128W(ref n3, pixel, blend);
		}

		private static void Dia3X(ref BgraColor n8, ref BgraColor n5, ref BgraColor n7, BgraColor pixel, bool blend) {
			AlphaBlend224W(ref n8, pixel, blend);
			AlphaBlend32W(ref n5, pixel, blend);
			AlphaBlend32W(ref n7, pixel, blend);
		}

		private static void Kernel2Xv5(BgraColor pe, BgraColor pi, BgraColor ph, BgraColor pf, BgraColor pg, BgraColor pc, BgraColor pd, BgraColor pb, BgraColor f4, BgraColor i4, BgraColor h5, BgraColor i5, ref BgraColor n1, ref BgraColor n2, ref BgraColor n3, bool blend) {
			if ((pe == ph ? true : pe == pf)) {
				return;
			}
			uint num = YuvDifference(pe, pc) + YuvDifference(pe, pg) + YuvDifference(pi, h5) + YuvDifference(pi, f4) + (YuvDifference(ph, pf) << 2);
			uint num1 = YuvDifference(ph, pd) + YuvDifference(ph, i5) + YuvDifference(pf, i4) + YuvDifference(pf, pb) + (YuvDifference(pe, pi) << 2);
			BgraColor pixel = (YuvDifference(pe, pf) <= YuvDifference(pe, ph) ? pf : ph);
			if (num < num1 && (pf != pb && ph != pd || pe == pi && pf != i4 && ph != i5 || pe == pg || pe == pc)) {
				uint num2 = YuvDifference(pf, pg);
				uint num3 = YuvDifference(ph, pc);
				bool flag = (pe == pc ? false : pb != pc);
				bool flag1 = (pe == pg ? false : pd != pg);
				if ((num2 << 1 > num3 || !flag1) && (num2 < num3 << 1 || !flag)) {
					Dia2X(ref n3, pixel, blend);
					return;
				}
				if (num2 << 1 <= num3 && flag1) {
					Left22X(ref n3, ref n2, pixel, blend);
				}
				if (num2 >= num3 << 1 && flag) {
					Up22X(ref n3, ref n1, pixel, blend);
					return;
				}
			} else if (num <= num1) {
				AlphaBlend64W(ref n3, pixel, blend);
			}
		}

		private static void Kernel3X(BgraColor pe, BgraColor pi, BgraColor ph, BgraColor pf, BgraColor pg, BgraColor pc, BgraColor pd, BgraColor pb, BgraColor f4, BgraColor i4, BgraColor h5, BgraColor i5, ref BgraColor n2, ref BgraColor n5, ref BgraColor n6, ref BgraColor n7, ref BgraColor n8, bool blend) {
			bool flag;
			bool flag2;
			if ((pe == ph ? true : pe == pf)) {
				return;
			}
			uint num = YuvDifference(pe, pc) + YuvDifference(pe, pg) + YuvDifference(pi, h5) + YuvDifference(pi, f4) + (YuvDifference(ph, pf) << 2);
			uint num1 = YuvDifference(ph, pd) + YuvDifference(ph, i5) + YuvDifference(pf, i4) + YuvDifference(pf, pb) + (YuvDifference(pe, pi) << 2);
			if (num >= num1) {
				flag2 = false;
			} else {
				flag2 = ((pf == pb || ph == pd) && (pe != pi || pf == i4 || ph == i5) && pe != pg ? pe == pc : true);
			}
			flag = flag2;
			if (!flag) {
				if (num <= num1) {
					AlphaBlend128W(ref n8, (YuvDifference(pe, pf) <= YuvDifference(pe, ph) ? pf : ph), blend);
				}
				return;
			}
			uint num2 = YuvDifference(pf, pg);
			uint num3 = YuvDifference(ph, pc);
			bool flag3 = (pe == pc ? false : pb != pc);
			bool flag4 = (pe == pg ? false : pd != pg);
			BgraColor pixel = (YuvDifference(pe, pf) <= YuvDifference(pe, ph) ? pf : ph);
			if (num2 << 1 <= num3 && flag4 && num2 >= num3 << 1 && flag3) {
				LeftUp23X(ref n7, out n5, ref n6, ref n2, out n8, pixel, blend);
				return;
			}
			if (num2 << 1 <= num3 && flag4) {
				Left23X(ref n7, ref n5, ref n6, out n8, pixel, blend);
				return;
			}
			if (num2 < num3 << 1 || !flag3) {
				Dia3X(ref n8, ref n5, ref n7, pixel, blend);
				return;
			}
			Up23X(ref n5, ref n7, ref n2, out n8, pixel, blend);
		}

		private static void Kernel4Xv2(BgraColor pe, BgraColor pi, BgraColor ph, BgraColor pf, BgraColor pg, BgraColor pc, BgraColor pd, BgraColor pb, BgraColor f4, BgraColor i4, BgraColor h5, BgraColor i5, ref BgraColor n15, ref BgraColor n14, ref BgraColor n11, ref BgraColor n3, ref BgraColor n7, ref BgraColor n10, ref BgraColor n13, ref BgraColor n12, bool blend) {
			if ((pe == ph ? true : pe == pf)) {
				return;
			}
			uint num = YuvDifference(pe, pc) + YuvDifference(pe, pg) + YuvDifference(pi, h5) + YuvDifference(pi, f4) + (YuvDifference(ph, pf) << 2);
			uint num1 = YuvDifference(ph, pd) + YuvDifference(ph, i5) + YuvDifference(pf, i4) + YuvDifference(pf, pb) + (YuvDifference(pe, pi) << 2);
			BgraColor pixel = (YuvDifference(pe, pf) <= YuvDifference(pe, ph) ? pf : ph);
			if (num < num1 && (pf != pb && ph != pd || pe == pi && pf != i4 && ph != i5 || pe == pg || pe == pc)) {
				uint num2 = YuvDifference(pf, pg);
				uint num3 = YuvDifference(ph, pc);
				bool flag = (pe == pc ? false : pb != pc);
				bool flag1 = (pe == pg ? false : pd != pg);
				if ((num2 << 1 > num3 || !flag1) && (num2 < num3 << 1 || !flag)) {
					Dia(out n15, ref n14, ref n11, pixel, blend);
					return;
				}
				if (num2 << 1 <= num3 && flag1) {
					Left2(out n15, out n14, ref n11, ref n13, ref n12, ref n10, pixel, blend);
				}
				if (num2 >= num3 << 1 && flag) {
					Up2(out n15, ref n14, out n11, ref n3, ref n7, ref n10, pixel, blend);
					return;
				}
			} else if (num <= num1) {
				AlphaBlend128W(ref n15, pixel, blend);
			}
		}

		private static void Left2(out BgraColor n15, out BgraColor n14, ref BgraColor n11, ref BgraColor n13, ref BgraColor n12, ref BgraColor n10, BgraColor pixel, bool blend) {
			AlphaBlend192W(ref n11, pixel, blend);
			AlphaBlend192W(ref n13, pixel, blend);
			AlphaBlend64W(ref n10, pixel, blend);
			AlphaBlend64W(ref n12, pixel, blend);
			n14 = pixel;
			n15 = pixel;
		}

		private static void Left22X(ref BgraColor n3, ref BgraColor n2, BgraColor pixel, bool blend) {
			AlphaBlend192W(ref n3, pixel, blend);
			AlphaBlend64W(ref n2, pixel, blend);
		}

		private static void Left23X(ref BgraColor n7, ref BgraColor n5, ref BgraColor n6, out BgraColor n8, BgraColor pixel, bool blend) {
			AlphaBlend192W(ref n7, pixel, blend);
			AlphaBlend64W(ref n5, pixel, blend);
			AlphaBlend64W(ref n6, pixel, blend);
			n8 = pixel;
		}

		private static void LeftUp23X(ref BgraColor n7, out BgraColor n5, ref BgraColor n6, ref BgraColor n2, out BgraColor n8, BgraColor pixel, bool blend) {
			AlphaBlend192W(ref n7, pixel, blend);
			AlphaBlend64W(ref n6, pixel, blend);
			n5 = n7;
			n2 = n6;
			n8 = pixel;
		}

		private static void Up2(out BgraColor n15, ref BgraColor n14, out BgraColor n11, ref BgraColor n3, ref BgraColor n7, ref BgraColor n10, BgraColor pixel, bool blend) {
			AlphaBlend192W(ref n14, pixel, blend);
			AlphaBlend192W(ref n7, pixel, blend);
			AlphaBlend64W(ref n10, pixel, blend);
			AlphaBlend64W(ref n3, pixel, blend);
			n11 = pixel;
			n15 = pixel;
		}

		private static void Up22X(ref BgraColor n3, ref BgraColor n1, BgraColor pixel, bool blend) {
			AlphaBlend192W(ref n3, pixel, blend);
			AlphaBlend64W(ref n1, pixel, blend);
		}

		private static void Up23X(ref BgraColor n5, ref BgraColor n7, ref BgraColor n2, out BgraColor n8, BgraColor pixel, bool blend) {
			AlphaBlend192W(ref n5, pixel, blend);
			AlphaBlend64W(ref n7, pixel, blend);
			AlphaBlend64W(ref n2, pixel, blend);
			n8 = pixel;
		}

		private static uint YuvDifference(BgraColor a, BgraColor b) {
			return (uint) (48 * Math.Abs(a.Luminance - b.Luminance) + 6 * Math.Abs(a.ChrominanceV - b.ChrominanceV) + 7 * Math.Abs(a.ChrominanceU - b.ChrominanceU));
		}

		private static void Xbr2X(PixelWorker sourceImage, int srcX, int srcY, PixelWorker targetImage, int tgtX, int tgtY, bool allowAlphaBlending) {
			BgraColor item = sourceImage.GetPixelClampedBgra(srcX - 1, srcY - 1);
			BgraColor pixel = sourceImage.GetPixelClampedBgra(srcX, srcY - 1);
			BgraColor item1 = sourceImage.GetPixelClampedBgra(srcX + 1, srcY - 1);
			BgraColor pixel1 = sourceImage.GetPixelClampedBgra(srcX - 1, srcY);
			BgraColor item2 = sourceImage[srcX, srcY];
			BgraColor pixel2 = sourceImage.GetPixelClampedBgra(srcX + 1, srcY);
			BgraColor item3 = sourceImage.GetPixelClampedBgra(srcX - 1, srcY + 1);
			BgraColor pixel3 = sourceImage.GetPixelClampedBgra(srcX, srcY + 1);
			BgraColor item4 = sourceImage.GetPixelClampedBgra(srcX + 1, srcY + 1);
			BgraColor pixel4 = sourceImage.GetPixelClampedBgra(srcX - 1, srcY - 2);
			BgraColor item5 = sourceImage.GetPixelClampedBgra(srcX, srcY - 2);
			BgraColor pixel5 = sourceImage.GetPixelClampedBgra(srcX + 1, srcY - 2);
			BgraColor item6 = sourceImage.GetPixelClampedBgra(srcX - 2, srcY - 1);
			BgraColor pixel6 = sourceImage.GetPixelClampedBgra(srcX - 2, srcY);
			BgraColor item7 = sourceImage.GetPixelClampedBgra(srcX - 2, srcY + 1);
			BgraColor pixel7 = sourceImage.GetPixelClampedBgra(srcX + 2, srcY - 1);
			BgraColor item8 = sourceImage.GetPixelClampedBgra(srcX + 2, srcY);
			BgraColor pixel8 = sourceImage.GetPixelClampedBgra(srcX + 2, srcY + 1);
			BgraColor item9 = sourceImage.GetPixelClampedBgra(srcX - 1, srcY + 2);
			BgraColor pixel9 = sourceImage.GetPixelClampedBgra(srcX, srcY + 2);
			BgraColor item10 = sourceImage.GetPixelClampedBgra(srcX + 1, srcY + 2);
			BgraColor pixel10 = item2;
			BgraColor pixel11 = pixel10;
			BgraColor pixel12 = pixel10;
			BgraColor pixel13 = pixel10;
			BgraColor pixel14 = pixel10;
			Kernel2Xv5(item2, item4, pixel3, pixel2, item3, item1, pixel1, pixel, item8, pixel8, pixel9, item10, ref pixel13, ref pixel12, ref pixel11, allowAlphaBlending);
			Kernel2Xv5(item2, item1, pixel2, pixel, item4, item, pixel3, pixel1, item5, pixel5, item8, pixel7, ref pixel14, ref pixel11, ref pixel13, allowAlphaBlending);
			Kernel2Xv5(item2, item, pixel, pixel1, item1, item3, pixel2, pixel3, pixel6, item6, item5, pixel4, ref pixel12, ref pixel13, ref pixel14, allowAlphaBlending);
			Kernel2Xv5(item2, item3, pixel1, pixel3, item, item4, pixel, pixel2, pixel9, item9, pixel6, item7, ref pixel11, ref pixel14, ref pixel12, allowAlphaBlending);
			targetImage[tgtX, tgtY] = pixel14;
			targetImage[tgtX + 1, tgtY] = pixel13;
			targetImage[tgtX, tgtY + 1] = pixel12;
			targetImage[tgtX + 1, tgtY + 1] = pixel11;
		}

		private static void Xbr3X(PixelWorker sourceImage, int srcX, int srcY, PixelWorker targetImage, int tgtX, int tgtY, bool allowAlphaBlending) {
			BgraColor item = sourceImage.GetPixelClampedBgra(srcX - 1, srcY - 1);
			BgraColor pixel = sourceImage.GetPixelClampedBgra(srcX, srcY - 1);
			BgraColor item1 = sourceImage.GetPixelClampedBgra(srcX + 1, srcY - 1);
			BgraColor pixel1 = sourceImage.GetPixelClampedBgra(srcX - 1, srcY);
			BgraColor item2 = sourceImage[srcX, srcY];
			BgraColor pixel2 = sourceImage.GetPixelClampedBgra(srcX + 1, srcY);
			BgraColor item3 = sourceImage.GetPixelClampedBgra(srcX - 1, srcY + 1);
			BgraColor pixel3 = sourceImage.GetPixelClampedBgra(srcX, srcY + 1);
			BgraColor item4 = sourceImage.GetPixelClampedBgra(srcX + 1, srcY + 1);
			BgraColor pixel4 = sourceImage.GetPixelClampedBgra(srcX - 1, srcY - 2);
			BgraColor item5 = sourceImage.GetPixelClampedBgra(srcX, srcY - 2);
			BgraColor pixel5 = sourceImage.GetPixelClampedBgra(srcX + 1, srcY - 2);
			BgraColor item6 = sourceImage.GetPixelClampedBgra(srcX - 2, srcY - 1);
			BgraColor pixel6 = sourceImage.GetPixelClampedBgra(srcX - 2, srcY);
			BgraColor item7 = sourceImage.GetPixelClampedBgra(srcX - 2, srcY + 1);
			BgraColor pixel7 = sourceImage.GetPixelClampedBgra(srcX + 2, srcY - 1);
			BgraColor item8 = sourceImage.GetPixelClampedBgra(srcX + 2, srcY);
			BgraColor pixel8 = sourceImage.GetPixelClampedBgra(srcX + 2, srcY + 1);
			BgraColor item9 = sourceImage.GetPixelClampedBgra(srcX - 1, srcY + 2);
			BgraColor pixel9 = sourceImage.GetPixelClampedBgra(srcX, srcY + 2);
			BgraColor item10 = sourceImage.GetPixelClampedBgra(srcX + 1, srcY + 2);
			BgraColor pixel10 = item2;
			BgraColor pixel11 = pixel10;
			BgraColor pixel12 = pixel10;
			BgraColor pixel13 = pixel10;
			BgraColor pixel14 = pixel10;
			BgraColor pixel15 = pixel10;
			BgraColor pixel16 = pixel10;
			BgraColor pixel17 = pixel10;
			BgraColor pixel18 = pixel10;
			BgraColor pixel19 = pixel10;
			Kernel3X(item2, item4, pixel3, pixel2, item3, item1, pixel1, pixel, item8, pixel8, pixel9, item10, ref pixel17, ref pixel14, ref pixel13, ref pixel12, ref pixel11, allowAlphaBlending);
			Kernel3X(item2, item1, pixel2, pixel, item4, item, pixel3, pixel1, item5, pixel5, item8, pixel7, ref pixel19, ref pixel18, ref pixel11, ref pixel14, ref pixel17, allowAlphaBlending);
			Kernel3X(item2, item, pixel, pixel1, item1, item3, pixel2, pixel3, pixel6, item6, item5, pixel4, ref pixel13, ref pixel16, ref pixel17, ref pixel18, ref pixel19, allowAlphaBlending);
			Kernel3X(item2, item3, pixel1, pixel3, item, item4, pixel, pixel2, pixel9, item9, pixel6, item7, ref pixel11, ref pixel12, ref pixel19, ref pixel16, ref pixel13, allowAlphaBlending);
			targetImage[tgtX, tgtY] = pixel19;
			targetImage[tgtX + 1, tgtY] = pixel18;
			targetImage[tgtX + 2, tgtY] = pixel17;
			targetImage[tgtX, tgtY + 1] = pixel16;
			targetImage[tgtX + 1, tgtY + 1] = pixel15;
			targetImage[tgtX + 2, tgtY + 1] = pixel14;
			targetImage[tgtX, tgtY + 2] = pixel13;
			targetImage[tgtX + 1, tgtY + 2] = pixel12;
			targetImage[tgtX + 2, tgtY + 2] = pixel11;
		}

		private static void Xbr4X(PixelWorker sourceImage, int srcX, int srcY, PixelWorker targetImage, int tgtX, int tgtY, bool allowAlphaBlending) {
			BgraColor item = sourceImage.GetPixelClampedBgra(srcX - 1, srcY - 1);
			BgraColor pixel = sourceImage.GetPixelClampedBgra(srcX, srcY - 1);
			BgraColor item1 = sourceImage.GetPixelClampedBgra(srcX + 1, srcY - 1);
			BgraColor pixel1 = sourceImage.GetPixelClampedBgra(srcX - 1, srcY);
			BgraColor item2 = sourceImage[srcX, srcY];
			BgraColor pixel2 = sourceImage.GetPixelClampedBgra(srcX + 1, srcY);
			BgraColor item3 = sourceImage.GetPixelClampedBgra(srcX - 1, srcY + 1);
			BgraColor pixel3 = sourceImage.GetPixelClampedBgra(srcX, srcY + 1);
			BgraColor item4 = sourceImage.GetPixelClampedBgra(srcX + 1, srcY + 1);
			BgraColor pixel4 = sourceImage.GetPixelClampedBgra(srcX - 1, srcY - 2);
			BgraColor item5 = sourceImage.GetPixelClampedBgra(srcX, srcY - 2);
			BgraColor pixel5 = sourceImage.GetPixelClampedBgra(srcX + 1, srcY - 2);
			BgraColor item6 = sourceImage.GetPixelClampedBgra(srcX - 2, srcY - 1);
			BgraColor pixel6 = sourceImage.GetPixelClampedBgra(srcX - 2, srcY);
			BgraColor item7 = sourceImage.GetPixelClampedBgra(srcX - 2, srcY + 1);
			BgraColor pixel7 = sourceImage.GetPixelClampedBgra(srcX + 2, srcY - 1);
			BgraColor item8 = sourceImage.GetPixelClampedBgra(srcX + 2, srcY);
			BgraColor pixel8 = sourceImage.GetPixelClampedBgra(srcX + 2, srcY + 1);
			BgraColor item9 = sourceImage.GetPixelClampedBgra(srcX - 1, srcY + 2);
			BgraColor pixel9 = sourceImage.GetPixelClampedBgra(srcX, srcY + 2);
			BgraColor item10 = sourceImage.GetPixelClampedBgra(srcX + 1, srcY + 2);
			BgraColor pixel10 = item2;
			BgraColor pixel11 = pixel10;
			BgraColor pixel12 = pixel10;
			BgraColor pixel13 = pixel10;
			BgraColor pixel14 = pixel10;
			BgraColor pixel15 = pixel10;
			BgraColor pixel16 = pixel10;
			BgraColor pixel17 = pixel10;
			BgraColor pixel18 = pixel10;
			BgraColor pixel19 = pixel10;
			BgraColor pixel20 = pixel10;
			BgraColor pixel21 = pixel10;
			BgraColor pixel22 = pixel10;
			BgraColor pixel23 = pixel10;
			BgraColor pixel24 = pixel10;
			BgraColor pixel25 = pixel10;
			BgraColor pixel26 = pixel10;
			Kernel4Xv2(item2, item4, pixel3, pixel2, item3, item1, pixel1, pixel, item8, pixel8, pixel9, item10, ref pixel11, ref pixel12, ref pixel15, ref pixel23, ref pixel19, ref pixel16, ref pixel13, ref pixel14, allowAlphaBlending);
			Kernel4Xv2(item2, item1, pixel2, pixel, item4, item, pixel3, pixel1, item5, pixel5, item8, pixel7, ref pixel23, ref pixel19, ref pixel24, ref pixel26, ref pixel25, ref pixel20, ref pixel15, ref pixel11, allowAlphaBlending);
			Kernel4Xv2(item2, item, pixel, pixel1, item1, item3, pixel2, pixel3, pixel6, item6, item5, pixel4, ref pixel26, ref pixel25, ref pixel22, ref pixel14, ref pixel18, ref pixel21, ref pixel24, ref pixel23, allowAlphaBlending);
			Kernel4Xv2(item2, item3, pixel1, pixel3, item, item4, pixel, pixel2, pixel9, item9, pixel6, item7, ref pixel14, ref pixel18, ref pixel13, ref pixel11, ref pixel12, ref pixel17, ref pixel22, ref pixel26, allowAlphaBlending);
			targetImage[tgtX, tgtY] = pixel26;
			targetImage[tgtX + 1, tgtY] = pixel25;
			targetImage[tgtX + 2, tgtY] = pixel24;
			targetImage[tgtX + 3, tgtY] = pixel23;
			targetImage[tgtX, tgtY + 1] = pixel22;
			targetImage[tgtX + 1, tgtY + 1] = pixel21;
			targetImage[tgtX + 2, tgtY + 1] = pixel20;
			targetImage[tgtX + 3, tgtY + 1] = pixel19;
			targetImage[tgtX, tgtY + 2] = pixel18;
			targetImage[tgtX + 1, tgtY + 2] = pixel17;
			targetImage[tgtX + 2, tgtY + 2] = pixel16;
			targetImage[tgtX + 3, tgtY + 2] = pixel15;
			targetImage[tgtX, tgtY + 3] = pixel14;
			targetImage[tgtX + 1, tgtY + 3] = pixel13;
			targetImage[tgtX + 2, tgtY + 3] = pixel12;
			targetImage[tgtX + 3, tgtY + 3] = pixel11;
		}

		#endregion
	}
}