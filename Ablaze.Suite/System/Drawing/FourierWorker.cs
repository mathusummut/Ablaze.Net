using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace System.Drawing {
	/// <summary>
	/// Represents the fourier transformation of an image.
	/// </summary>
	public sealed class FourierWorker : ICloneable, IDisposable {
		private ComplexF[][] values;
		private PixelWorker alphaReference;
		private bool ShiftAxes, FFTAfter;
		/// <summary>
		/// The width of the image in pixels.
		/// </summary>
		public readonly int TargetWidth;
		/// <summary>
		/// The height of the image in pixels.
		/// </summary>
		public readonly int TargetHeight;
		/// <summary>
		/// The respective power-of-two width of this image.
		/// </summary>
		public readonly int FourierWidth;
		/// <summary>
		/// The respective power-of-two height of this image.
		/// </summary>
		public readonly int FourierHeight;
		/// <summary>
		/// The respective power-of-two size of this image.
		/// </summary>
		public readonly int FourierPixelCount;
		/// <summary>
		/// The respective power-of-two size of this image.
		/// </summary>
		public readonly int FourierPixelComponentCount;
		/// <summary>
		/// The number of components or channels per pixel (Depth / 8).
		/// </summary>
		public readonly int ComponentCount;
		/// <summary>
		/// The number of pixels in the image (Width * Height).
		/// </summary>
		public readonly int TargetPixelCount;
		/// <summary>
		/// The total number of components or channels (PixelCount * ComponentCount).
		/// </summary>
		public readonly int TargetPixelComponentCount;
		/// <summary>
		/// The maximum component value found in the original image.
		/// </summary>
		public readonly byte OriginalMax;
		/// <summary>
		/// Gets or sets the normalization cap to use. By default, it is equal to OriginalMax.
		/// </summary>
		public byte NormalizationCap;
		/// <summary>
		/// The size of the tagret bitmap.
		/// </summary>
		public readonly Size TargetSize;
		/// <summary>
		/// The respective power-of-two size of this image.
		/// </summary>
		public readonly Size FourierSize;
		/// <summary>
		/// For recreational purposes.
		/// </summary>
		public object Tag;
		private int fourierWidthLog2, fourierHeightLog2;

		/// <summary>
		/// Gets the specified value.
		/// </summary>
		/// <param name="component">The component index (BGRA).</param>
		/// <param name="index">The pixel index. Index = Y * FourierWidth + X.</param>
		public ComplexF this[int component, int index] {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return values == null ? ComplexF.Zero : values[component][index];
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				if (values == null)
					return;
				else
					values[component][index] = value;
			}
		}

		/// <summary>
		/// Gets the specified value.
		/// </summary>
		/// <param name="component">The component index (BGRA).</param>
		/// <param name="x">The X-coordinate of the value.</param>
		/// <param name="y">The Y-coordinate of the value.</param>
		public ComplexF this[int component, int x, int y] {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return values == null ? ComplexF.Zero : values[component][y * FourierWidth + x];
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				if (values == null)
					return;
				else
					values[component][y * FourierWidth + x] = value;
			}
		}

		/// <summary>
		/// Gets whether this instance is disposed.
		/// </summary>
		public bool IsDisposed {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return values == null;
			}
		}

		/// <summary>
		/// Calculates the fast fourier transform of the image.
		/// </summary>
		/// <param name="image">The image to get calculate the FFT from.</param>
		/// <param name="shiftAxes">Whether to perform FFTShift on the Fourier image.</param>
		/// <param name="ignoreAlpha">Whether to ignore the alpha component of the image (if it is 32-bit).</param>
		/// <param name="disposeImage">Whether to dispose the image after use.</param>
		/// <param name="targetWidth">The height to resize the kernel to, or -1 to leave kernel same width. Can only be larger than the kernel width.</param>
		/// <param name="targetHeight">The height to resize the kernel to, or -1 to leave kernel same height. Can only be larger than the kernel height.</param>
		public FourierWorker(Bitmap image, bool shiftAxes = true, bool ignoreAlpha = false, bool disposeImage = false, int targetWidth = -1, int targetHeight = -1) : this(PixelWorker.FromImage(image, false, false, disposeImage ? ImageParameterAction.Dispose : ImageParameterAction.RemoveReference), shiftAxes, ignoreAlpha, true, targetWidth, targetHeight) {
		}

		/// <summary>
		/// Calculates the fast fourier transform of the image.
		/// </summary>
		/// <param name="image">The image to get calculate the FFT from.</param>
		/// <param name="shiftAxes">Whether to perform FFTShift on the Fourier image.</param>
		/// <param name="ignoreAlpha">Whether to ignore the alpha component of the image (if it is 32-bit).</param>
		/// <param name="disposeImage">Whether to dispose the image after use.</param>
		/// <param name="targetWidth">The height to resize the kernel to, or -1 to leave kernel same width. Can only be larger than the kernel width.</param>
		/// <param name="targetHeight">The height to resize the kernel to, or -1 to leave kernel same height. Can only be larger than the kernel height.</param>
		public FourierWorker(PixelWorker image, bool shiftAxes = true, bool ignoreAlpha = false, bool disposeImage = false, int targetWidth = -1, int targetHeight = -1) {
			if (image == null)
				return;
			int componentCount = image.ComponentCount, targetPixelCount = image.PixelCount;
			if (ignoreAlpha && componentCount == 4) {
				componentCount = 3;
				alphaReference = image;
				alphaReference.ShallowClone();
			}
			values = new ComplexF[componentCount][];
			ComponentCount = componentCount;
			TargetWidth = image.Width;
			TargetHeight = image.Height;
			TargetPixelCount = image.PixelCount;
			TargetPixelComponentCount = image.PixelComponentCount;
			TargetSize = new Size(TargetWidth, TargetHeight);
			FourierWidth = (int) Maths.CeilingPowerOfTwo((uint) (targetWidth <= TargetWidth ? TargetWidth : targetWidth));
			FourierHeight = (int) Maths.CeilingPowerOfTwo((uint) (targetHeight <= TargetHeight ? TargetHeight : targetHeight));
			FourierSize = new Size(FourierWidth, FourierHeight);
			FourierPixelCount = FourierWidth * FourierHeight;
			FourierPixelComponentCount = FourierPixelCount * ComponentCount;
			fourierWidthLog2 = Maths.Log2((uint) FourierWidth);
			fourierHeightLog2 = Maths.Log2((uint) FourierHeight);
			ComplexF[] current;
			byte comp;
			int c, x = 0, y = 0, fourierWidth = FourierWidth, fourierHeight = FourierHeight, tw = TargetWidth;
			int tcc = image.ComponentCount, txMinusOne = TargetWidth - 1, tyMinusOne = TargetHeight - 1;
			for (c = 0; c < componentCount; c++) {
				current = new ComplexF[FourierPixelCount];
				values[c] = current;
				for (y = 0; y < fourierHeight; y++) {
					for (x = 0; x < fourierWidth; x++) {
						comp = image.GetPixelBgra((Math.Min(y, tyMinusOne) * tw + Math.Min(x, txMinusOne)) * tcc)[c];
						if (comp > OriginalMax)
							OriginalMax = comp;
						current[y * fourierWidth + x] = new ComplexF() {
							Real = comp,
						};
					}
				}
			}
			NormalizationCap = OriginalMax;
			FFT(true);
			if (shiftAxes) {
				ShiftAxes = true;
				FFTShift();
			}
			if (disposeImage)
				image.Dispose();
		}

		/// <summary>
		/// Calculates the FFT of a kernel. No normalization is performed on the kernel.
		/// </summary>
		/// <param name="kernel">The 2D kernel to calculate the FFT from. The first index is the X-coordinate, the second index is the Y-coordinate.
		/// All columns must be of equal length.</param>
		/// <param name="shiftAxes">Whether to perform FFTShift on the Fourier image.</param>
		/// <param name="targetWidth">The height to resize the kernel to, or -1 to leave kernel same width. Can only be larger than the kernel width.</param>
		/// <param name="targetHeight">The height to resize the kernel to, or -1 to leave kernel same height. Can only be larger than the kernel height.</param>
		public FourierWorker(float[][] kernel, bool shiftAxes = true, int targetWidth = -1, int targetHeight = -1) : this(new float[][][] { kernel }, shiftAxes, targetWidth, targetHeight) {
		}

		/// <summary>
		/// Calculates the FFT of a kernel. No normalization is performed on the kernel.
		/// </summary>
		/// <param name="kernel">An array of 2D kernels to calculate the FFT from.
		/// The first index is the component index, the second index is the X-coordinate, the third index is the Y-coordinate.
		/// All columns must be of equal length.</param>
		/// <param name="shiftAxes">Whether to perform FFTShift on the Fourier image.</param>
		/// <param name="targetWidth">The height to resize the kernel to, or -1 to leave kernel same width. If it's smaller than the kernel width, it is ignored.</param>
		/// <param name="targetHeight">The height to resize the kernel to, or -1 to leave kernel same height. If it's smaller than the kernel width, it is ignored.</param>
		[CLSCompliant(false)]
		public FourierWorker(float[][][] kernel, bool shiftAxes = true, int targetWidth = -1, int targetHeight = -1) {
			int cc = kernel == null ? 0 : kernel.Length;
			ComponentCount = cc;
			float[][] k = cc == 0 ? null : kernel[0];
			if (targetWidth <= 0) {
				TargetWidth = k == null ? 0 : k.Length;
				FourierWidth = (int) Maths.CeilingPowerOfTwo((uint) TargetWidth);
			} else {
				TargetWidth = k == null ? 0 : k.Length;
				FourierWidth = (int) Maths.CeilingPowerOfTwo((uint) (targetWidth <= TargetWidth ? TargetWidth : targetWidth));
			}
			if (targetHeight <= 0) {
				TargetHeight = (k == null || k.Length == 0) ? 0 : (k[0] == null ? 0 : k[0].Length);
				FourierHeight = (int) Maths.CeilingPowerOfTwo((uint) TargetHeight);
			} else {
				TargetHeight = (k == null || k.Length == 0) ? 0 : (k[0] == null ? 0 : k[0].Length);
				FourierHeight = (int) Maths.CeilingPowerOfTwo((uint) (targetHeight <= TargetHeight ? TargetHeight : targetHeight));
			}
			OriginalMax = 255;
			NormalizationCap = 255;
			TargetPixelCount = TargetWidth * TargetHeight;
			TargetPixelComponentCount = TargetPixelCount * cc;
			FourierPixelCount = FourierWidth * FourierHeight;
			FourierPixelComponentCount = FourierPixelCount * cc;
			fourierWidthLog2 = Maths.Log2((uint) FourierWidth);
			fourierHeightLog2 = Maths.Log2((uint) FourierHeight);
			TargetSize = new Size(TargetWidth, TargetHeight);
			FourierSize = new Size(FourierWidth, FourierHeight);
			values = new ComplexF[cc][];
			int c;
			for (c = 0; c < cc; c++)
				values[c] = new ComplexF[FourierPixelCount];
			if (kernel == null)
				return;
			targetWidth = TargetWidth;
			targetHeight = TargetHeight;
			int fourierWidth = FourierWidth;
			float[] temp;
			ComplexF[] current;
			int x, y, xDiv, yDiv, xOffset, yOffset;
			for (c = 0; c < cc; c++) {
				current = values[c];
				k = kernel[c];
				x = 0;
				xDiv = (targetWidth & 1) == 0 ? -1 : targetWidth / 2;
				xOffset = (fourierWidth - targetWidth) / 2;
				while (x < targetWidth) {
					temp = k[x];
					y = 0;
					yOffset = (FourierHeight - targetHeight) / 2;
					yDiv = (targetHeight & 1) == 0 ? -1 : targetHeight / 2;
					while (y < targetHeight) {
						current[(y + yOffset) * fourierWidth + x + xOffset] = new ComplexF() {
							Real = temp[y]
						};
						if (y == yDiv) {
							yDiv = -1;
							yOffset++;
						} else
							y++;
					}
					if (x == xDiv) {
						xDiv = -1;
						xOffset++;
					} else
						x++;
				}
			}
			FFT(true);
			if (shiftAxes) {
				ShiftAxes = true;
				FFTShift();
			}
		}

		/// <summary>
		/// Copies the specified image into a new instance.
		/// </summary>
		/// <param name="image">The Fouurier image to copy.</param>
		public FourierWorker(FourierWorker image) {
			if (image == null || image.values == null)
				return;
			FFTAfter = image.FFTAfter;
			OriginalMax = image.OriginalMax;
			NormalizationCap = image.NormalizationCap;
			FourierWidth = image.FourierWidth;
			FourierHeight = image.FourierHeight;
			FourierSize = image.FourierSize;
			FourierPixelCount = image.FourierPixelCount;
			FourierPixelComponentCount = image.FourierPixelComponentCount;
			alphaReference = image.alphaReference;
			if (alphaReference != null)
				alphaReference.ShallowClone();
			ShiftAxes = image.ShiftAxes;
			TargetWidth = image.TargetWidth;
			TargetHeight = image.TargetHeight;
			TargetSize = image.TargetSize;
			ComponentCount = image.ComponentCount;
			TargetPixelCount = image.TargetPixelCount;
			TargetPixelComponentCount = image.TargetPixelComponentCount;
			fourierWidthLog2 = image.fourierWidthLog2;
			fourierHeightLog2 = image.fourierHeightLog2;
			values = new ComplexF[ComponentCount][];
			for (int c = 0; c < ComponentCount; c++) {
				values[c] = new ComplexF[FourierPixelCount];
				Array.Copy(image.values[c], values[c], FourierPixelCount);
			}
		}

		/// <summary>
		/// Transforms this image by adding it with the specified kernel.
		/// </summary>
		/// <param name="kernel">The image to add with. If the kernel has one channel, it will be added with all channels of this image, else the corresponding channels will be added.</param>
		public void AddWith(FourierWorker kernel) {
			if (!(kernel.FourierWidth == FourierWidth && kernel.FourierHeight == FourierHeight))
				throw new ArgumentException("The passed FourierWorker must have the same FourierWidth and FourierHeight as this instance.", nameof(kernel));
			else if (kernel.ComponentCount == 1) {
				ComplexF[] add = kernel.values[0];
				ParallelLoop.For(0, FourierPixelCount, i => {
					int componentCount = ComponentCount;
					for (int c = 0; c < componentCount; c++)
						values[c][i] += add[i];
				}, ImageLib.ParallelCutoff / ComponentCount);
			} else {
				int min = Math.Min(ComponentCount, kernel.ComponentCount);
				ComplexF[][] val = kernel.values;
				ParallelLoop.For(0, FourierPixelCount, i => {
					for (int c = 0; c < min; c++)
						values[c][i] += val[c][i];
				}, ImageLib.ParallelCutoff / min);
			}
		}

		/// <summary>
		/// Transforms this image by subtracting the specified filter from it.
		/// </summary>
		/// <param name="kernel">The filter to subtract from this image. If the filter has one channel, it will be subtracted from all channels of this image, else the corresponding channels will be subtracted.</param>
		public void SubtractFrom(FourierWorker kernel) {
			if (!(kernel.FourierWidth == FourierWidth && kernel.FourierHeight == FourierHeight))
				throw new ArgumentException("The passed FourierWorker must have the same FourierWidth and FourierHeight as this instance.", nameof(kernel));
			else if (kernel.ComponentCount == 1) {
				ComplexF[] add = kernel.values[0];
				ParallelLoop.For(0, FourierPixelCount, i => {
					int componentCount = ComponentCount;
					for (int c = 0; c < componentCount; c++)
						values[c][i] -= add[i];
				}, ImageLib.ParallelCutoff / ComponentCount);
			} else {
				int min = Math.Min(ComponentCount, kernel.ComponentCount);
				ComplexF[][] val = kernel.values;
				ParallelLoop.For(0, FourierPixelCount, i => {
					for (int c = 0; c < min; c++)
						values[c][i] -= val[c][i];
				}, ImageLib.ParallelCutoff / min);
			}
		}

		/// <summary>
		/// Transforms this image by multiplying it element-wise with the specified kernel.
		/// </summary>
		/// <param name="kernel">The kernel to multiply with. If the kernel has one channel, it will be multiplied with all channels of this image, else the corresponding channels will be multiplied.</param>
		public void MultiplyWith(FourierWorker kernel) {
			if (!(kernel.FourierWidth == FourierWidth && kernel.FourierHeight == FourierHeight))
				throw new ArgumentException("The passed FourierWorker must have the same FourierWidth and FourierHeight as this instance.", nameof(kernel));
			if (kernel.ShiftAxes)
				FFTAfter = !FFTAfter;
			if (kernel.ComponentCount == 1) {
				ComplexF[] mult = kernel.values[0];
				ParallelLoop.For(0, FourierPixelCount, i => {
					int componentCount = ComponentCount;
					for (int c = 0; c < componentCount; c++)
						values[c][i] *= mult[i];
				}, ImageLib.ParallelCutoff / ComponentCount);
			} else {
				int min = Math.Min(ComponentCount, kernel.ComponentCount);
				ComplexF[][] val = kernel.values;
				ParallelLoop.For(0, FourierPixelCount, i => {
					for (int c = 0; c < min; c++)
						values[c][i] *= val[c][i];
				}, ImageLib.ParallelCutoff / min);
			}
		}

		/// <summary>
		/// Transforms this image by dividing it element-wise with the specified kernel.
		/// </summary>
		/// <param name="kernel">The kernel to divide by. If the kernel has one channel, it will be divided from all channels of this image, else the corresponding channels will be divided.</param>
		public void DivideBy(FourierWorker kernel) {
			if (!(kernel.FourierWidth == FourierWidth && kernel.FourierHeight == FourierHeight))
				throw new ArgumentException("The passed FourierWorker must have the same FourierWidth and FourierHeight as this instance.", nameof(kernel));
			if (kernel.ShiftAxes)
				FFTAfter = !FFTAfter;
			if (kernel.ComponentCount == 1) {
				ComplexF[] mult = kernel.values[0];
				ParallelLoop.For(0, FourierPixelCount, i => {
					int componentCount = ComponentCount;
					for (int c = 0; c < componentCount; c++)
						values[c][i] /= mult[i];
				}, ImageLib.ParallelCutoff / ComponentCount);
			} else {
				int min = Math.Min(ComponentCount, kernel.ComponentCount);
				ComplexF[][] val = kernel.values;
				ParallelLoop.For(0, FourierPixelCount, i => {
					for (int c = 0; c < min; c++)
						values[c][i] /= val[c][i];
				}, ImageLib.ParallelCutoff / min);
			}
		}

		/// <summary>
		/// Applies the specified function to all the values in the image.
		/// </summary>
		/// <param name="function">The function to apply. The parameter is the value, and the return value is the new corresponding value.</param>
		public void ApplyFunctionToAllValues(Func<ComplexF, ComplexF> function) {
			if (values == null)
				return;
			ParallelLoop.For(0, FourierPixelCount, i => {
				int componentCount = ComponentCount;
				for (int c = 0; c < componentCount; c++)
					values[c][i] = function(values[c][i]);
			}, ImageLib.ParallelCutoff / ComponentCount);
		}

		/// <summary>
		/// Applies the specified function to all the values in the image.
		/// </summary>
		/// <param name="function">The function to apply. The first parameter is the value, the second parameter is the coordinate of the point in the image, and the return value is the new corresponding value.</param>
		public void ApplyFunctionToAllValues(Func<ComplexF, Point, ComplexF> function) {
			if (values == null)
				return;
			ParallelLoop.For(0, FourierPixelCount, i => {
				int width = FourierWidth, componentCount = ComponentCount;
				for (int c = 0; c < componentCount; c++)
					values[c][i] = function(values[c][i], new Point(i % width, i / width));
			}, ImageLib.ParallelCutoff / ComponentCount);
		}

		/// <summary>
		/// Performs Fourier inverse on the image in place. If you need to retain the Fourier image, copy it first.
		/// </summary>
		public PixelWorker ConvertToBitmapInPlace() {
			if (!FFTAfter && ShiftAxes) {
				ShiftAxes = false;
				FFTShift();
			}
			FFT(false);
			if (FFTAfter && ShiftAxes) {
				ShiftAxes = false;
				FFTShift();
			}
			int pc = FourierPixelCount, index;
			float[][] results = new float[ComponentCount][];
			float[] array;
			float current, max = 0f;
			for (int comp = 0; comp < ComponentCount; comp++) {
				array = new float[pc];
				results[comp] = array;
				for (index = 0; index < pc; index++) {
					current = values[comp][index].Magnitude;
					array[index] = current;
					if (current > max)
						max = current;
				}
			}
			if (max == 0f)
				max = 1f;
			else
				max = OriginalMax / max;
			bool addAlpha = !(ComponentCount == 4 || alphaReference == null || alphaReference.IsDisposed);
			int byteDepth = addAlpha ? 4 : ComponentCount;
			byte[] resultant = new byte[TargetPixelCount * byteDepth];
			ParallelLoop.For(0, TargetPixelCount, i => {
				int componentCount = ComponentCount;
				int offset = i * byteDepth;
				int fourierIndex = (i / TargetWidth) * FourierWidth + i % TargetWidth;
				for (int c = 0; c < componentCount; c++)
					resultant[offset + c] = (byte) (results[c][fourierIndex] * max);
				if (addAlpha) {
					offset += 3;
					resultant[offset] = alphaReference[offset];
				}
			}, ImageLib.ParallelCutoff / ComponentCount);
			return new PixelWorker(resultant, TargetWidth, TargetHeight, false);
		}

		/// <summary>
		/// Generates a plot of the magnitudes of the complex values of the Fourier image.
		/// </summary>
		/// <param name="ignoreAlpha">If true, the alpha channel is ignored. If false, the image might end up transparent.</param>
		public PixelWorker GenerateMagnitudePlot(bool ignoreAlpha = true) {
			if (values == null)
				return null;
			int pc = FourierPixelCount, index;
			int componentCount = ComponentCount;
			if (componentCount == 4 && ignoreAlpha)
				componentCount = 3;
			float[][] results = new float[componentCount][];
			float[] array;
			float current, max = 0f, min = float.PositiveInfinity;
			for (int comp = 0; comp < componentCount; comp++) {
				array = new float[pc];
				results[comp] = array;
				for (index = 0; index < pc; index++) {
					current = values[comp][index].MagnitudeDB;
					array[index] = current;
					if (current > max)
						max = current;
					if (current < min)
						min = current;
				}
			}
			if (max == min)
				max = min + 1f;
			else
				max = 255f / (max - min);
			byte[] resultant = new byte[pc * componentCount];
			ParallelLoop.For(0, FourierPixelCount, i => {
				int offset = i * componentCount;
				for (int c = 0; c < componentCount; c++)
					resultant[offset + c] = (byte) ((results[c][i] - min) * max);
			}, ImageLib.ParallelCutoff / componentCount);
			return new PixelWorker(resultant, FourierWidth, FourierHeight, false);
		}

		/// <summary>
		/// Generates a plot of the phases of the complex values of the Fourier image.
		/// </summary>
		/// <param name="ignoreAlpha">If true, the alpha channel is ignored. If false, the image might end up transparent.</param>
		public PixelWorker GeneratePhasePlot(bool ignoreAlpha = true) {
			if (values == null)
				return null;
			int componentCount = ComponentCount;
			if (componentCount == 4 && ignoreAlpha)
				componentCount = 3;
			byte[] resultant = new byte[FourierPixelCount * componentCount];
			const float Normalizer = 255f / Maths.TwoPIInverseF;
			ParallelLoop.For(0, FourierPixelCount, i => {
				int offset = i * componentCount;
				for (int c = 0; c < componentCount; c++)
					resultant[offset + c] = (byte) (Normalizer * ((values[c][i].Phase + Maths.TwoPiF) % Maths.TwoPiF));
			}, ImageLib.ParallelCutoff / componentCount);
			return new PixelWorker(resultant, FourierWidth, FourierHeight, false);
		}

		/// <summary>
		/// Returns a deep-clone copy of this image.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public object Clone() {
			return new FourierWorker(this);
		}

		/// <summary>
		/// Computes an in-place complex-to-complex 2D FFT.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void FFT() {
			FFT(true);
		}

		/// <summary>
		/// Computes an in-place complex-to-complex inverse 2D FFT.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void InverseFFT() {
			FFT(false);
		}

		/// <summary>
		/// Computes an in-place complex-to-complex 2D FFT.
		/// </summary>
		/// <param name="forward">Whether to transform forward or backward.</param>
		private void FFT(bool forward) {
			if (values == null)
				return;
			ParallelLoop.For(0, FourierHeight, y => {
				int i = y * FourierWidth;
				int widthLog2 = fourierWidthLog2, componentCount = ComponentCount;
				for (int c = 0; c < componentCount; c++)
					FFT(values[c], 1, i, widthLog2, forward);
			}, ImageLib.ParallelCutoff / ComponentCount);
			ParallelLoop.For(0, FourierWidth, i => {
				int heightLog2 = fourierHeightLog2, componentCount = ComponentCount, width = FourierWidth;
				for (int c = 0; c < componentCount; c++)
					FFT(values[c], width, i, heightLog2, forward);
			}, ImageLib.ParallelCutoff / ComponentCount);
		}

		/// <summary>
		/// Performs a fourier shift on the image. Two fourier shifts cancel each other out.
		/// </summary>
		public void FFTShift() {
			if (values == null)
				return;
			int halfWidth = FourierWidth / 2, halfHeight = FourierHeight / 2;
			ComplexF[] current;
			ComplexF temp;
			for (int c = 0; c < ComponentCount; c++) {
				current = values[c];
				ParallelLoop.For(0, halfHeight, y => {
					int width = FourierWidth;
					int index1, index2;
					for (int x = 0; x < halfWidth; x++) {
						index1 = x + y * width;
						index2 = x + halfWidth + (y + halfHeight) * width;
						temp = current[index1];
						current[index1] = current[index2];
						current[index2] = temp;
						index1 = x + (y + halfHeight) * width;
						index2 = x + halfWidth + y * width;
						temp = current[index1];
						current[index1] = current[index2];
						current[index2] = temp;
					}
				}, ImageLib.ParallelCutoff);
			}
		}

		/// <summary>
		/// Performs a fourier shift on the specified values.
		/// </summary>
		/// <param name="values">The values to shift.</param>
		/// <param name="forward">True for fftshift, false for inverse fftshift.</param>
		public static ComplexF[] FFTShift(ComplexF[] values, bool forward) {
			if (values == null)
				return null;
			int length = values.Length;
			ComplexF[] shifted = new ComplexF[length];
			int halfLength = length / 2, offset = (int) Math.Ceiling(length * 0.5);
			if (forward) {
				ParallelLoop.For(0, halfLength, i => {
					shifted[i + halfLength] = values[i];
					shifted[i] = values[i + offset];
				}, ImageLib.ParallelCutoff);
				if (halfLength != offset)
					shifted[length - 1] = values[halfLength];
			} else {
				ParallelLoop.For(0, halfLength, i => {
					shifted[i + offset] = values[i];
					shifted[i] = values[i + halfLength];
				}, ImageLib.ParallelCutoff);
				if (halfLength != offset)
					shifted[halfLength] = values[length - 1];
			}
			return shifted;
		}

		/// <summary>
		/// Computes an in-place 1D fourier transform of the specified values.
		/// </summary>
		/// <param name="values">An array of the values to transform.</param>
		/// <param name="forward">True for FFT, false for inverse FFT.</param>
		/// <param name="truncatePadding">Whether to trim trailing 0s.</param>
		public static ComplexF[] FFT(ComplexF[] values, bool forward, bool truncatePadding = true) {
			if (values == null)
				return null;
			int length = values.Length;
			if (length == 0)
				return new ComplexF[length];
			else if (length == 1)
				return new ComplexF[] { values[0] };
			int targetLength = (int) Maths.CeilingPowerOfTwo((uint) length);
			ComplexF[] resultant = new ComplexF[targetLength];
			Array.Copy(values, resultant, length);
			FFT(resultant, 1L, 0L, Maths.Log2((uint) targetLength), forward);
			if (truncatePadding) {
				int i = targetLength - 1;
				while (resultant[i].MagnitudeSquared <= 0.000001f) {
					i--;
					if (i == -1)
						return new ComplexF[0];
				}
				if (i != targetLength - 1) {
					ComplexF[] oldResultant = resultant;
					resultant = new ComplexF[i + 1];
					Array.Copy(oldResultant, resultant, resultant.Length);
				}
			}
			return resultant;
		}

		/// <summary>
		/// Computes an in-place complex-to-complex FFT.
		/// </summary>
		private static void FFT(ComplexF[] values, long indexMultiplier, long indexOffset, int log2Length, bool forward) {
			long i, i1, k;
			double u1, u2, z;
			ComplexF t, temp;
			long nn = 1L << log2Length;
			long i2 = nn >> 1;
			long j = 0L;
			long nMinusOne = nn - 1L;
			long index1, index2;
			for (i = 0; i < nMinusOne; i++) {
				if (i < j) {
					index1 = i * indexMultiplier + indexOffset;
					index2 = j * indexMultiplier + indexOffset;
					t = values[index1];
					values[index1] = values[index2];
					values[index2] = t;
				}
				k = i2;
				while (k <= j) {
					j -= k;
					k >>= 1;
				}
				j += k;
			}
			double c1 = -1.0;
			double c2 = 0.0;
			long l1, l2 = 1L;
			double mult = forward ? -1.0 : 1.0;
			for (long l = 0L; l < log2Length; l++) {
				l1 = l2;
				l2 <<= 1;
				u1 = 1.0;
				u2 = 0.0;
				for (j = 0L; j < l1; j++) {
					for (i = j; i < nn; i += l2) {
						i1 = i + l1;
						index1 = i * indexMultiplier + indexOffset;
						index2 = i1 * indexMultiplier + indexOffset;
						t = values[index2];
						t = new ComplexF((float) (u1 * t.Real - u2 * t.Imaginary), (float) (u1 * t.Imaginary + u2 * t.Real));
						temp = values[index1];
						values[index2] = temp - t;
						values[index1] = temp + t;
					}
					z = u1 * c1 - u2 * c2;
					u2 = u1 * c2 + u2 * c1;
					u1 = z;
				}
				c2 = Math.Sqrt((1.0 - c1) * 0.5) * mult;
				c1 = Math.Sqrt((1.0 + c1) * 0.5);
			}
			if (forward) {
				float multiplier = 1f / nn;
				for (i = 0L; i < nn; i++) {
					index1 = i * indexMultiplier + indexOffset;
					temp = values[index1];
					values[index1] = new ComplexF(temp.Real * multiplier, temp.Imaginary * multiplier);
				}
			}
		}

		/// <summary>
		/// Disposes of the resources used by this image.
		/// </summary>
		~FourierWorker() {
			Dispose();
		}

		/// <summary>
		/// Disposes of the resources used by this image.
		/// </summary>
		public void Dispose() {
			if (values == null)
				return;
			values = null;
			if (alphaReference != null)
				alphaReference.Dispose();
			GC.SuppressFinalize(this);
		}
	}
}