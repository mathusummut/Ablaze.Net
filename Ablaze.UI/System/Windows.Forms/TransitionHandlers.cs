using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.Windows.Forms {
	internal static class TransitionHandlers {
		[TransitionHandler(typeof(ColorF))]
		public static AnimationState Transition(ColorF currentValue, object targetValue, double gradient, double linearSpeed, bool stoppable, ref object cachedValue, out ColorF newValue) {
			newValue = currentValue;
			if (stoppable && !(cachedValue == null || (ColorF) cachedValue == currentValue))
				return AnimationState.ValueDidntUpdateAsExpected;
			ColorF target = (ColorF) targetValue;
			ColorF transitionVal = ImageLib.TransitionClamp(currentValue, target, (float) gradient, (float) linearSpeed);
			if (Math.Abs(transitionVal.A - currentValue.A) <= float.Epsilon &&
				Math.Abs(transitionVal.R - currentValue.R) <= float.Epsilon &&
				Math.Abs(transitionVal.G - currentValue.G) <= float.Epsilon &&
				Math.Abs(transitionVal.B - currentValue.B) <= float.Epsilon)
				return AnimationState.Completed;
			else {
				newValue = transitionVal;
				cachedValue = newValue;
				return AnimationState.UpdateSuccess;
			}
		}

		[TransitionHandler(typeof(Color))]
		public static AnimationState Transition(Color currentValue, object targetValue, double gradient, double linearSpeed, bool stoppable, ref object cachedValue, out Color newValue) {
			newValue = currentValue;
			Color target = (Color) targetValue;
			if (currentValue == target)
				return AnimationState.Completed;
			Vector4 cached;
			if (cachedValue == null)
				cached = new Vector4(currentValue.A, currentValue.R, currentValue.G, currentValue.B);
			else {
				cached = (Vector4) cachedValue;
				if (stoppable && FloorColor(cached) != currentValue)
					return AnimationState.ValueDidntUpdateAsExpected;
			}
			Vector4 transitionVal = TransitionClamp(cached, new Vector4(target.A, target.R, target.G, target.B), (float) gradient, (float) linearSpeed);
			if (Math.Abs(transitionVal.X - currentValue.A) <= float.Epsilon &&
				Math.Abs(transitionVal.Y - currentValue.R) <= float.Epsilon &&
				Math.Abs(transitionVal.Z - currentValue.G) <= float.Epsilon &&
				Math.Abs(transitionVal.W - currentValue.B) <= float.Epsilon)
				newValue = target;
			else {
				newValue = FloorColor(transitionVal);
				cachedValue = transitionVal;
			}
			return AnimationState.UpdateSuccess;
		}

		[TransitionHandler(typeof(BgraColor))]
		public static AnimationState Transition(BgraColor currentValue, object targetValue, double gradient, double linearSpeed, bool stoppable, ref object cachedValue, out BgraColor newValue) {
			newValue = currentValue;
			BgraColor target = (BgraColor) targetValue;
			if (currentValue == target)
				return AnimationState.Completed;
			Vector4 cached;
			if (cachedValue == null)
				cached = new Vector4(currentValue.A, currentValue.R, currentValue.G, currentValue.B);
			else {
				cached = (Vector4) cachedValue;
				if (stoppable && FloorBgraColor(cached) != currentValue)
					return AnimationState.ValueDidntUpdateAsExpected;
			}
			Vector4 transitionVal = TransitionClamp(cached, new Vector4(target.A, target.R, target.G, target.B), (float) gradient, (float) linearSpeed);
			if (Math.Abs(transitionVal.X - currentValue.A) <= float.Epsilon &&
				Math.Abs(transitionVal.Y - currentValue.R) <= float.Epsilon &&
				Math.Abs(transitionVal.Z - currentValue.G) <= float.Epsilon &&
				Math.Abs(transitionVal.W - currentValue.B) <= float.Epsilon)
				newValue = target;
			else {
				newValue = FloorBgraColor(transitionVal);
				cachedValue = transitionVal;
			}
			return AnimationState.UpdateSuccess;
		}

		/// <summary>
		/// Computes the transition value at the state specified but clamps gradient to the range [0, 1].
		/// </summary>
		/// <param name="basePixel">The original pixel color.</param>
		/// <param name="overlayPixel">The target pixel color.</param>
		/// <param name="gradient">The transition state (0 - original, 1 - target color).</param>
		/// <param name="offset">The transition linear offset.</param>
		private static Vector4 TransitionClamp(this Vector4 basePixel, Vector4 overlayPixel, float gradient, float offset) {
			if (gradient <= float.Epsilon)
				return basePixel;
			else if (gradient >= 1f)
				return overlayPixel;
			Vector4 result = Vector4.Lerp(basePixel, overlayPixel, gradient);

			offset = Math.Abs(offset);
			if (offset > 0f) {
				result = Vector4.Clamp(result, Vector4.Zero, new Vector4(255f));
				if (Math.Abs(overlayPixel.X - result.X) <= offset)
					result.X = overlayPixel.X;
				else if (overlayPixel.X > result.X)
					result.X += offset;
				else
					result.X -= offset;

				if (Math.Abs(overlayPixel.Y - result.Y) <= offset)
					result.Y = overlayPixel.Y;
				else if (overlayPixel.Y > result.Y)
					result.Y += offset;
				else
					result.Y -= offset;

				if (Math.Abs(overlayPixel.Z - result.Z) <= offset)
					result.Z = overlayPixel.Z;
				else if (overlayPixel.Z > result.Z)
					result.Z += offset;
				else
					result.Z -= offset;

				if (Math.Abs(overlayPixel.W - result.W) <= offset)
					result.W = overlayPixel.W;
				else if (overlayPixel.W > result.W)
					result.W += offset;
				else
					result.W -= offset;
			}

			return result;
		}

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		private static BgraColor FloorBgraColor(Vector4 color) {
			return new BgraColor((byte) color.X, (byte) color.Y, (byte) color.Z, (byte) color.W);
		}

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		private static Color FloorColor(Vector4 color) {
			return Color.FromArgb((int) color.X, (int) color.Y, (int) color.Z, (int) color.W);
		}

		[TransitionHandler(typeof(Bitmap))]
		public static AnimationState Transition(Bitmap currentValue, object targetValue, double gradient, double linearSpeed, bool stoppable, ref object cachedValue, out Bitmap newValue) {
			newValue = currentValue;
			Bitmap targetBitmap = (Bitmap) targetValue;
			if (targetBitmap == currentValue)
				return AnimationState.Completed;
			BitmapAnimationWrapper transitionData;
			if (cachedValue == null) {
				transitionData = new BitmapAnimationWrapper(currentValue, targetBitmap);
				if (transitionData.CurrentValues == null || transitionData.Target == null)
					return AnimationState.Completed;
				cachedValue = transitionData;
			} else
				transitionData = (BitmapAnimationWrapper) cachedValue;
			byte completed = 1;
			float[] currentValues = transitionData.CurrentValues;
			byte[] target = transitionData.Target;
			ParallelLoop.For(0, currentValues.Length, i => currentValues[i] = (float) ImageLib.TransitionClamp(currentValues[i], target[i], gradient, linearSpeed));
			lock (transitionData.SyncLock) {
				if (currentValue.IsDisposed())
					return AnimationState.Completed;
				try {
					using (PixelWorker worker = PixelWorker.FromImage(currentValue, false, true)) {
						ParallelLoop.For(0, currentValues.Length, i => {
							byte val = (byte) currentValues[i];
							worker[i] = val;
							if (val != target[i])
								Volatile.Write(ref completed, 0);
						}, ImageLib.ParallelCutoff);
					}
				} catch {
					return AnimationState.Completed;
				}
			}
			return Volatile.Read(ref completed) == 1 ? AnimationState.Completed : AnimationState.UpdateSuccess;
		}

		[TransitionHandler(typeof(PixelWorker))]
		public static AnimationState Transition(PixelWorker currentValue, object targetValue, double gradient, double linearSpeed, bool stoppable, ref object cachedValue, out PixelWorker newValue) {
			newValue = currentValue;
			PixelWorker targetBitmap = (PixelWorker) targetValue;
			if (targetBitmap == currentValue)
				return AnimationState.Completed;
			BitmapAnimationWrapper transitionData;
			if (cachedValue == null) {
				transitionData = new BitmapAnimationWrapper(currentValue, targetBitmap);
				if (transitionData.CurrentValues == null || transitionData.Target == null)
					return AnimationState.Completed;
				cachedValue = transitionData;
			} else
				transitionData = (BitmapAnimationWrapper) cachedValue;
			byte completed = 1;
			float[] currentValues = transitionData.CurrentValues;
			byte[] target = transitionData.Target;
			ParallelLoop.For(0, currentValues.Length, i => currentValues[i] = (float) ImageLib.TransitionClamp(currentValues[i], target[i], gradient, linearSpeed));
			lock (transitionData.SyncLock) {
				ParallelLoop.For(0, currentValues.Length, i => {
					byte val = (byte) currentValues[i];
					currentValue[i] = val;
					if (val != target[i])
						Volatile.Write(ref completed, 0);
				}, ImageLib.ParallelCutoff);
			}
			return Volatile.Read(ref completed) == 1 ? AnimationState.Completed : AnimationState.UpdateSuccess;
		}

		[TransitionHandler(typeof(RectangleF))]
		public static AnimationState Transition(RectangleF currentValue, object targetValue, double gradient, double linearSpeed, bool stoppable, ref object cachedValue, out RectangleF newValue) {
			newValue = currentValue;
			if (stoppable && !(cachedValue == null || (RectangleF) cachedValue == currentValue))
				return AnimationState.ValueDidntUpdateAsExpected;
			RectangleF target = (RectangleF) targetValue;
			RectangleF transitionVal = new RectangleF((float) ImageLib.TransitionClamp(currentValue.X, target.X, gradient, linearSpeed),
				(float) ImageLib.TransitionClamp(currentValue.Y, target.Y, gradient, linearSpeed),
				(float) ImageLib.TransitionClamp(currentValue.Width, target.Width, gradient, linearSpeed),
				(float) ImageLib.TransitionClamp(currentValue.Height, target.Height, gradient, linearSpeed));
			if (Math.Abs(transitionVal.X - currentValue.X) <= float.Epsilon &&
				Math.Abs(transitionVal.Y - currentValue.Y) <= float.Epsilon &&
				Math.Abs(transitionVal.Width - currentValue.Width) <= float.Epsilon &&
				Math.Abs(transitionVal.Height - currentValue.Height) <= float.Epsilon)
				return AnimationState.Completed;
			else {
				newValue = transitionVal;
				cachedValue = newValue;
				return AnimationState.UpdateSuccess;
			}
		}

		[TransitionHandler(typeof(Rectangle))]
		public static AnimationState Transition(Rectangle currentValue, object targetValue, double gradient, double linearSpeed, bool stoppable, ref object cachedValue, out Rectangle newValue) {
			newValue = currentValue;
			Rectangle target = (Rectangle) targetValue;
			if (currentValue == target)
				return AnimationState.Completed;
			RectangleF cached;
			if (cachedValue == null)
				cached = currentValue;
			else {
				cached = (RectangleF) cachedValue;
				if (stoppable && Rectangle.Truncate(cached) != currentValue)
					return AnimationState.ValueDidntUpdateAsExpected;
			}
			RectangleF transitionVal = new RectangleF((float) Maths.Clamp(ImageLib.TransitionClamp(cached.X, target.X, gradient, linearSpeed), int.MinValue, int.MaxValue),
				(float) Maths.Clamp(ImageLib.TransitionClamp(cached.Y, target.Y, gradient, linearSpeed), int.MinValue, int.MaxValue),
				(float) Maths.Clamp(ImageLib.TransitionClamp(cached.Width, target.Width, gradient, linearSpeed), int.MinValue, int.MaxValue),
				(float) Maths.Clamp(ImageLib.TransitionClamp(cached.Height, target.Height, gradient, linearSpeed), int.MinValue, int.MaxValue));
			if (Math.Abs(transitionVal.X - currentValue.X) <= float.Epsilon &&
				Math.Abs(transitionVal.Y - currentValue.Y) <= float.Epsilon &&
				Math.Abs(transitionVal.Width - currentValue.Width) <= float.Epsilon &&
				Math.Abs(transitionVal.Height - currentValue.Height) <= float.Epsilon)
				newValue = target;
			else {
				newValue = Rectangle.Truncate(transitionVal);
				cachedValue = transitionVal;
			}
			return AnimationState.UpdateSuccess;
		}

		[TransitionHandler(typeof(SizeF))]
		public static AnimationState Transition(SizeF currentValue, object targetValue, double gradient, double linearSpeed, bool stoppable, ref object cachedValue, out SizeF newValue) {
			newValue = currentValue;
			if (stoppable && !(cachedValue == null || (SizeF) cachedValue == currentValue))
				return AnimationState.ValueDidntUpdateAsExpected;
			SizeF target = (SizeF) targetValue;
			SizeF transitionVal = new SizeF((float) ImageLib.TransitionClamp(currentValue.Width, target.Width, gradient, linearSpeed),
				(float) ImageLib.TransitionClamp(currentValue.Height, target.Height, gradient, linearSpeed));
			if (Math.Abs(transitionVal.Width - currentValue.Width) <= float.Epsilon &&
				Math.Abs(transitionVal.Height - currentValue.Height) <= float.Epsilon)
				return AnimationState.Completed;
			else {
				newValue = transitionVal;
				cachedValue = newValue;
				return AnimationState.UpdateSuccess;
			}
		}

		[TransitionHandler(typeof(Size))]
		public static AnimationState Transition(Size currentValue, object targetValue, double gradient, double linearSpeed, bool stoppable, ref object cachedValue, out Size newValue) {
			newValue = currentValue;
			Size target = (Size) targetValue;
			if (currentValue == target)
				return AnimationState.Completed;
			SizeF cached;
			if (cachedValue == null)
				cached = currentValue;
			else {
				cached = (SizeF) cachedValue;
				if (stoppable && Size.Truncate(cached) != currentValue)
					return AnimationState.ValueDidntUpdateAsExpected;
			}
			SizeF transitionVal = new SizeF((float) Maths.Clamp(ImageLib.TransitionClamp(cached.Width, target.Width, gradient, linearSpeed), int.MinValue, int.MaxValue),
				(float) Maths.Clamp(ImageLib.TransitionClamp(cached.Height, target.Height, gradient, linearSpeed), int.MinValue, int.MaxValue));
			if (Math.Abs(transitionVal.Width - currentValue.Width) <= float.Epsilon &&
				Math.Abs(transitionVal.Height - currentValue.Height) <= float.Epsilon)
				newValue = target;
			else {
				newValue = Size.Truncate(transitionVal);
				cachedValue = transitionVal;
			}
			return AnimationState.UpdateSuccess;
		}

		[TransitionHandler(typeof(Vector4))]
		public static AnimationState Transition(Vector4 currentValue, object targetValue, double gradient, double linearSpeed, bool stoppable, ref object cachedValue, out Vector4 newValue) {
			newValue = currentValue;
			if (stoppable && !(cachedValue == null || (Vector4) cachedValue == currentValue))
				return AnimationState.ValueDidntUpdateAsExpected;
			Vector4 target = (Vector4) targetValue;
			Vector4 transitionVal = new Vector4((float) ImageLib.TransitionClamp(currentValue.X, target.X, gradient, linearSpeed),
				(float) ImageLib.TransitionClamp(currentValue.Y, target.Y, gradient, linearSpeed),
				(float) ImageLib.TransitionClamp(currentValue.Z, target.Z, gradient, linearSpeed),
				(float) ImageLib.TransitionClamp(currentValue.W, target.W, gradient, linearSpeed));
			if (Math.Abs(transitionVal.X - currentValue.X) <= float.Epsilon &&
				Math.Abs(transitionVal.Y - currentValue.Y) <= float.Epsilon &&
				Math.Abs(transitionVal.Z - currentValue.Z) <= float.Epsilon &&
				Math.Abs(transitionVal.W - currentValue.W) <= float.Epsilon)
				return AnimationState.Completed;
			else {
				newValue = transitionVal;
				cachedValue = newValue;
				return AnimationState.UpdateSuccess;
			}
		}

		[TransitionHandler(typeof(Vector3))]
		public static AnimationState Transition(Vector3 currentValue, object targetValue, double gradient, double linearSpeed, bool stoppable, ref object cachedValue, out Vector3 newValue) {
			newValue = currentValue;
			if (stoppable && !(cachedValue == null || (Vector3) cachedValue == currentValue))
				return AnimationState.ValueDidntUpdateAsExpected;
			Vector3 target = (Vector3) targetValue;
			Vector3 transitionVal = new Vector3((float) ImageLib.TransitionClamp(currentValue.X, target.X, gradient, linearSpeed),
				(float) ImageLib.TransitionClamp(currentValue.Y, target.Y, gradient, linearSpeed),
				(float) ImageLib.TransitionClamp(currentValue.Z, target.Z, gradient, linearSpeed));
			if (Math.Abs(transitionVal.X - currentValue.X) <= float.Epsilon &&
				Math.Abs(transitionVal.Y - currentValue.Y) <= float.Epsilon &&
				Math.Abs(transitionVal.Z - currentValue.Z) <= float.Epsilon)
				return AnimationState.Completed;
			else {
				newValue = transitionVal;
				cachedValue = newValue;
				return AnimationState.UpdateSuccess;
			}
		}

		[TransitionHandler(typeof(Vector2))]
		public static AnimationState Transition(Vector2 currentValue, object targetValue, double gradient, double linearSpeed, bool stoppable, ref object cachedValue, out Vector2 newValue) {
			newValue = currentValue;
			if (stoppable && !(cachedValue == null || (Vector2) cachedValue == currentValue))
				return AnimationState.ValueDidntUpdateAsExpected;
			Vector2 target = (Vector2) targetValue;
			Vector2 transitionVal = new Vector2((float) ImageLib.TransitionClamp(currentValue.X, target.X, gradient, linearSpeed),
				(float) ImageLib.TransitionClamp(currentValue.Y, target.Y, gradient, linearSpeed));
			if (Math.Abs(transitionVal.X - currentValue.X) <= float.Epsilon &&
				Math.Abs(transitionVal.Y - currentValue.Y) <= float.Epsilon)
				return AnimationState.Completed;
			else {
				newValue = transitionVal;
				cachedValue = newValue;
				return AnimationState.UpdateSuccess;
			}
		}

		[TransitionHandler(typeof(PointF))]
		public static AnimationState Transition(PointF currentValue, object targetValue, double gradient, double linearSpeed, bool stoppable, ref object cachedValue, out PointF newValue) {
			newValue = currentValue;
			if (stoppable && !(cachedValue == null || (PointF) cachedValue == currentValue))
				return AnimationState.ValueDidntUpdateAsExpected;
			PointF target = (PointF) targetValue;
			PointF transitionVal = new PointF((float) ImageLib.TransitionClamp(currentValue.X, target.X, gradient, linearSpeed),
				(float) ImageLib.TransitionClamp(currentValue.Y, target.Y, gradient, linearSpeed));
			if (Math.Abs(transitionVal.X - currentValue.X) <= float.Epsilon &&
				Math.Abs(transitionVal.Y - currentValue.Y) <= float.Epsilon)
				return AnimationState.Completed;
			else {
				newValue = transitionVal;
				cachedValue = newValue;
				return AnimationState.UpdateSuccess;
			}
		}

		[TransitionHandler(typeof(Point))]
		public static AnimationState Transition(Point currentValue, object targetValue, double gradient, double linearSpeed, bool stoppable, ref object cachedValue, out Point newValue) {
			newValue = currentValue;
			Point target = (Point) targetValue;
			if (currentValue == target)
				return AnimationState.Completed;
			PointF cached;
			if (cachedValue == null)
				cached = currentValue;
			else {
				cached = (PointF) cachedValue;
				if (stoppable && Point.Truncate(cached) != currentValue)
					return AnimationState.ValueDidntUpdateAsExpected;
			}
			PointF transitionVal = new PointF((float) Maths.Clamp(ImageLib.TransitionClamp(cached.X, target.X, gradient, linearSpeed), int.MinValue, int.MaxValue),
				(float) Maths.Clamp(ImageLib.TransitionClamp(cached.Y, target.Y, gradient, linearSpeed), int.MinValue, int.MaxValue));
			if (Math.Abs(transitionVal.X - currentValue.X) <= float.Epsilon &&
				Math.Abs(transitionVal.Y - currentValue.Y) <= float.Epsilon)
				newValue = target;
			else {
				newValue = Point.Truncate(transitionVal);
				cachedValue = transitionVal;
			}
			return AnimationState.UpdateSuccess;
		}

		[TransitionHandler(typeof(double))]
		public static AnimationState Transition(double currentValue, object targetValue, double gradient, double linearSpeed, bool stoppable, ref object cachedValue, out double newValue) {
			newValue = currentValue;
			if (stoppable && !(cachedValue == null || Convert.ToDouble(cachedValue) == currentValue))
				return AnimationState.ValueDidntUpdateAsExpected;
			double transitionVal = ImageLib.TransitionClamp(currentValue, Convert.ToDouble(targetValue), gradient, linearSpeed);
			if (Math.Abs(transitionVal - currentValue) <= float.Epsilon)
				return AnimationState.Completed;
			else {
				newValue = transitionVal;
				cachedValue = newValue;
				return AnimationState.UpdateSuccess;
			}
		}

		[TransitionHandler(typeof(float))]
		public static AnimationState Transition(float currentValue, object targetValue, double gradient, double linearSpeed, bool stoppable, ref object cachedValue, out float newValue) {
			newValue = currentValue;
			if (stoppable && !(cachedValue == null || Convert.ToSingle(cachedValue) == currentValue))
				return AnimationState.ValueDidntUpdateAsExpected;
			float transitionVal = (float) ImageLib.TransitionClamp(currentValue, Convert.ToDouble(targetValue), gradient, linearSpeed);
			if (Math.Abs(transitionVal - currentValue) <= float.Epsilon)
				return AnimationState.Completed;
			else {
				newValue = transitionVal;
				cachedValue = newValue;
				return AnimationState.UpdateSuccess;
			}
		}

		[TransitionHandler(typeof(decimal))]
		public static AnimationState Transition(decimal currentValue, object targetValue, double gradient, double linearSpeed, bool stoppable, ref object cachedValue, out decimal newValue) {
			newValue = currentValue;
			if (stoppable && !(cachedValue == null || Convert.ToDecimal(cachedValue) == currentValue))
				return AnimationState.ValueDidntUpdateAsExpected;
			float transitionVal = (float) ImageLib.TransitionClamp((float) currentValue, Convert.ToDouble(targetValue), gradient, linearSpeed);
			if (Math.Abs((decimal) transitionVal - currentValue) <= (decimal) float.Epsilon)
				return AnimationState.Completed;
			else {
				newValue = (decimal) transitionVal;
				cachedValue = newValue;
				return AnimationState.UpdateSuccess;
			}
		}

		[TransitionHandler(typeof(int))]
		public static AnimationState Transition(int currentValue, object targetValue, double gradient, double linearSpeed, bool stoppable, ref object cachedValue, out int newValue) {
			newValue = currentValue;
			int target = Convert.ToInt32(targetValue);
			if (currentValue == target)
				return AnimationState.Completed;
			double cached;
			if (cachedValue == null)
				cached = currentValue;
			else {
				cached = (double) cachedValue;
				if (stoppable && ((int) cached) != currentValue)
					return AnimationState.ValueDidntUpdateAsExpected;
			}
			double transitionVal = Maths.Clamp(ImageLib.TransitionClamp(cached, target, gradient, linearSpeed), int.MinValue, int.MaxValue);
			if (Math.Abs(transitionVal - currentValue) <= float.Epsilon)
				newValue = target;
			else {
				newValue = (int) transitionVal;
				cachedValue = transitionVal;
			}
			return AnimationState.UpdateSuccess;
		}

		[TransitionHandler(typeof(long))]
		public static AnimationState Transition(long currentValue, object targetValue, double gradient, double linearSpeed, bool stoppable, ref object cachedValue, out long newValue) {
			newValue = currentValue;
			long target = Convert.ToInt64(targetValue);
			if (currentValue == target)
				return AnimationState.Completed;
			double cached;
			if (cachedValue == null)
				cached = currentValue;
			else {
				cached = (double) cachedValue;
				if (stoppable && ((long) cached) != currentValue)
					return AnimationState.ValueDidntUpdateAsExpected;
			}
			double transitionVal = Maths.Clamp(ImageLib.TransitionClamp(cached, target, gradient, linearSpeed), long.MinValue, long.MaxValue);
			if (Math.Abs(transitionVal - currentValue) <= float.Epsilon)
				newValue = target;
			else {
				newValue = (long) transitionVal;
				cachedValue = transitionVal;
			}
			return AnimationState.UpdateSuccess;
		}

		[TransitionHandler(typeof(short))]
		public static AnimationState Transition(short currentValue, object targetValue, double gradient, double linearSpeed, bool stoppable, ref object cachedValue, out short newValue) {
			newValue = currentValue;
			short target = Convert.ToInt16(targetValue);
			if (currentValue == target)
				return AnimationState.Completed;
			double cached;
			if (cachedValue == null)
				cached = currentValue;
			else {
				cached = (double) cachedValue;
				if (stoppable && ((short) cached) != currentValue)
					return AnimationState.ValueDidntUpdateAsExpected;
			}
			double transitionVal = Maths.Clamp(ImageLib.TransitionClamp(cached, target, gradient, linearSpeed), short.MinValue, short.MaxValue);
			if (Math.Abs(transitionVal - currentValue) <= float.Epsilon)
				newValue = target;
			else {
				newValue = (short) transitionVal;
				cachedValue = transitionVal;
			}
			return AnimationState.UpdateSuccess;
		}

		[TransitionHandler(typeof(byte))]
		public static AnimationState Transition(byte currentValue, object targetValue, double gradient, double linearSpeed, bool stoppable, ref object cachedValue, out byte newValue) {
			newValue = currentValue;
			byte target = Convert.ToByte(targetValue);
			if (currentValue == target)
				return AnimationState.Completed;
			double cached;
			if (cachedValue == null)
				cached = currentValue;
			else {
				cached = (double) cachedValue;
				if (stoppable && ((byte) cached) != currentValue)
					return AnimationState.ValueDidntUpdateAsExpected;
			}
			double transitionVal = Maths.Clamp(ImageLib.TransitionClamp(cached, target, gradient, linearSpeed), byte.MinValue, byte.MaxValue);
			if (Math.Abs(transitionVal - currentValue) <= float.Epsilon)
				newValue = target;
			else {
				newValue = (byte) transitionVal;
				cachedValue = transitionVal;
			}
			return AnimationState.UpdateSuccess;
		}

		[TransitionHandler(typeof(sbyte))]
		public static AnimationState Transition(sbyte currentValue, object targetValue, double gradient, double linearSpeed, bool stoppable, ref object cachedValue, out sbyte newValue) {
			newValue = currentValue;
			sbyte target = Convert.ToSByte(targetValue);
			if (currentValue == target)
				return AnimationState.Completed;
			double cached;
			if (cachedValue == null)
				cached = currentValue;
			else {
				cached = (double) cachedValue;
				if (stoppable && ((sbyte) cached) != currentValue)
					return AnimationState.ValueDidntUpdateAsExpected;
			}
			double transitionVal = Maths.Clamp(ImageLib.TransitionClamp(cached, target, gradient, linearSpeed), sbyte.MinValue, sbyte.MaxValue);
			if (Math.Abs(transitionVal - currentValue) <= float.Epsilon)
				newValue = target;
			else {
				newValue = (sbyte) transitionVal;
				cachedValue = transitionVal;
			}
			return AnimationState.UpdateSuccess;
		}

		[TransitionHandler(typeof(uint))]
		public static AnimationState Transition(uint currentValue, object targetValue, double gradient, double linearSpeed, bool stoppable, ref object cachedValue, out uint newValue) {
			newValue = currentValue;
			uint target = Convert.ToUInt32(targetValue);
			if (currentValue == target)
				return AnimationState.Completed;
			double cached;
			if (cachedValue == null)
				cached = currentValue;
			else {
				cached = (double) cachedValue;
				if (stoppable && ((uint) cached) != currentValue)
					return AnimationState.ValueDidntUpdateAsExpected;
			}
			double transitionVal = Maths.Clamp(ImageLib.TransitionClamp(cached, target, gradient, linearSpeed), uint.MinValue, uint.MaxValue);
			if (Math.Abs(transitionVal - currentValue) <= float.Epsilon)
				newValue = target;
			else {
				newValue = (uint) transitionVal;
				cachedValue = transitionVal;
			}
			return AnimationState.UpdateSuccess;
		}

		[TransitionHandler(typeof(ushort))]
		public static AnimationState Transition(ushort currentValue, object targetValue, double gradient, double linearSpeed, bool stoppable, ref object cachedValue, out ushort newValue) {
			newValue = currentValue;
			ushort target = Convert.ToUInt16(targetValue);
			if (currentValue == target)
				return AnimationState.Completed;
			double cached;
			if (cachedValue == null)
				cached = currentValue;
			else {
				cached = (double) cachedValue;
				if (stoppable && ((ushort) cached) != currentValue)
					return AnimationState.ValueDidntUpdateAsExpected;
			}
			double transitionVal = Maths.Clamp(ImageLib.TransitionClamp(cached, target, gradient, linearSpeed), ushort.MinValue, ushort.MaxValue);
			if (Math.Abs(transitionVal - currentValue) <= float.Epsilon)
				newValue = target;
			else {
				newValue = (ushort) transitionVal;
				cachedValue = transitionVal;
			}
			return AnimationState.UpdateSuccess;
		}

		[TransitionHandler(typeof(ulong))]
		public static AnimationState Transition(ulong currentValue, object targetValue, double gradient, double linearSpeed, bool stoppable, ref object cachedValue, out ulong newValue) {
			newValue = currentValue;
			ulong target = Convert.ToUInt64(targetValue);
			if (currentValue == target)
				return AnimationState.Completed;
			double cached;
			if (cachedValue == null)
				cached = currentValue;
			else {
				cached = (double) cachedValue;
				if (stoppable && ((ulong) cached) != currentValue)
					return AnimationState.ValueDidntUpdateAsExpected;
			}
			double transitionVal = Maths.Clamp(ImageLib.TransitionClamp(cached, target, gradient, linearSpeed), ulong.MinValue, ulong.MaxValue);
			if (Math.Abs(transitionVal - currentValue) <= float.Epsilon)
				newValue = target;
			else {
				newValue = (ulong) transitionVal;
				cachedValue = transitionVal;
			}
			return AnimationState.UpdateSuccess;
		}

		/// <summary>
		/// Initializes a wrapper for bitmap animation in the Animator class.
		/// </summary>
		private sealed class BitmapAnimationWrapper {
			public float[] CurrentValues;
			public byte[] Target;
			/// <summary>
			/// The lock to use to sync animation updating.
			/// </summary>
			public object SyncLock;

			public BitmapAnimationWrapper(PixelWorker image, PixelWorker target) {
				SyncLock = target.Tag;
				if (SyncLock == null) {
					SyncLock = image.Tag;
					if (SyncLock == null)
						SyncLock = new object();
				}
				lock (SyncLock) {
					CurrentValues = image.ToFloatArray();
					Target = target.ToByteArray(true);
				}
			}

			public BitmapAnimationWrapper(Bitmap image, Bitmap target) {
				SyncLock = target.Tag;
				if (SyncLock == null) {
					SyncLock = image.Tag;
					if (SyncLock == null)
						SyncLock = new object();
				}
				lock (SyncLock) {
					PixelWorker worker;
					try {
						if (image.IsDisposed() || target.IsDisposed())
							return;
						worker = PixelWorker.FromImage(image, false, false);
						CurrentValues = worker.ToFloatArray();
						worker.Dispose();
						worker = PixelWorker.FromImage(target, true, false, ImageParameterAction.RemoveReference, true);
						Target = worker.Buffer;
						worker.Dispose();
					} catch /*(Exception e)*/ {
						//ErrorHandler.LogException(e, ErrorHandler.ExceptionToDetailedString("The bitmap animation cannot continue.", e));
					}
				}
			}
		}
	}
}