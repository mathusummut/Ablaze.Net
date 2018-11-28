using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System.Windows.Forms {
	/// <summary>
	/// Animates values and variables in a smooth synchronized manner.
	/// </summary>
	public class UIAnimator : IDisposable {
		/// <summary>
		/// The default animator that is available for shared use with the default time interval.
		/// </summary>
		public static readonly UIAnimator SharedAnimator = new UIAnimator();
		private static UIAnimationHandler Empty = state => true;
		/// <summary>
		/// If true, all animation updates will be performed on separate ThreadPool threads. This usually prevents bottle-necking and deadlocks.
		/// </summary>
		public bool UpdateOnThreadPool = true;
		private static bool SingleCore = Environment.ProcessorCount == 1;
		/// <summary>
		/// Whether to disable animations on single-core processors.
		/// </summary>
		public static bool DisableOnSingleCoreProcessors = true;
		private ConcurrentDictionary<FieldOrProperty, AnimationInfo> AnimationCollection = new ConcurrentDictionary<FieldOrProperty, AnimationInfo>();
		private object DictLock = new object();
		private AsyncTimer animationTimer;
		private int timeInterval;
		private bool dontWaitForInterval, enabled = true;
		/// <summary>
		/// Called when the UIAnimator is updated.
		/// </summary>
		public event Action Updated;

		private AsyncTimer AnimationTimer {
			get {
				if (animationTimer == null) {
					animationTimer = new AsyncTimer(timeInterval);
					animationTimer.Tick += AnimationTimer_Tick;
					animationTimer.DontWaitForInterval = dontWaitForInterval;
				}
				return animationTimer;
			}
		}

		/// <summary>
		/// Gets whether the animator is disposed.
		/// </summary>
		public bool IsDisposed {
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets whether animations are enabled.
		/// </summary>
		public bool Enabled {
			get {
				return enabled;
			}
			set {
				enabled = value;
			}
		}

		/// <summary>
		/// If true, intervals are ignored and animations are updated immediately after the previous update (not recommended).
		/// </summary>
		public bool DontWaitForInterval {
			get {
				return dontWaitForInterval;
			}
			set {
				dontWaitForInterval = value;
				if (animationTimer != null)
					animationTimer.DontWaitForInterval = value;
			}
		}

		/// <summary>
		/// Gets or sets whether the animation update interval in milliseconds.
		/// </summary>
		public int Interval {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return timeInterval;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				timeInterval = value;
				if (animationTimer != null)
					animationTimer.Interval = value;
			}
		}

		/// <summary>
		/// Gets whether the animations are currently running.
		/// </summary>
		public bool Running {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return animationTimer != null && animationTimer.Running;
			}
		}

		/// <summary>
		/// Initializes the animator with the default time interval of 22ms.
		/// </summary>
		public UIAnimator() : this(22) {
		}

		/// <summary>
		/// Initializes the animator with the specified time interval.
		/// </summary>
		/// <param name="timeInterval">The animation time interval in milliseconds.</param>
		public UIAnimator(int timeInterval) {
			this.timeInterval = timeInterval;
		}

		/// <summary>
		/// Gets the current target value for the specified field/property of the specified instance if it is currently being animated (else null).
		/// </summary>
		/// <param name="entry">The field or property to check the target value of.</param>
		public object GetTarget(FieldOrProperty entry) {
			if (entry.Field == null && entry.Property == null)
				return null;
			AnimationInfo anim;
			if (AnimationCollection.TryGetValue(entry, out anim))
				return anim.targetValue;
			else
				return null;
		}

		/// <summary>
		/// Longest professional method name I have ever made.
		/// </summary>
		/// <param name="control">The control to invalidate on refresh.</param>
		/// <param name="redrawChildren">Whether to redraw the child controls on redraw.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static UIAnimationHandler GetFunctionToInvalidateControlOnUpdate(Control control, bool redrawChildren = false) {
			if (control == null)
				return Empty;
			else {
				return (state) => {
					if ((state == null || !state.IsFinished) && !control.IsDisposed) {
						try {
							control.Invalidate(redrawChildren);
							control.Update();
						} catch {
						}
					}
					return true;
				};
			}
		}

		/// <summary>
		/// Starts the animation using the specified parameters on the specified instance optionally refreshing the specified control using the specified update handler.
		/// Animations are halted the property takes Interval + 7 seconds to update.
		/// </summary>
		/// <param name="entry">The field/property whose value to animate.</param>
		/// <param name="targetValue">The target resultant value of the field (must be of a supported type).
		/// Supported types: Numeric values, Color, ColorF, Point, PointF, Size, SizeF, Rectangle, RectangleF, Vector2, Vector3, Vector4, Bitmap and PixelWorker.
		/// If using Bitmap or PixelWorker: the lock use for syncing animations is the 'Tag' property and the target image must be the same size and bits as the original.</param>
		/// <param name="gradient">The animation speed multiplier [0, 1] usually 0.4 (ie. the distance between the current value and the target value is multiplied by this value).</param>
		/// <param name="linearSpeed">The linear speed that is there to counteract the exponential infinity that one gets when dividing the distance between the current value and the target value with the animation speed.</param>
		/// <param name="stoppable">If true, the animation will stop if the value didn't update as expected.</param>
		/// <param name="onUpdate">The method to call when the property/field is updated (can be null). If the function returns false, the animation will halt.</param>
		/// <param name="callIfFinished">Whether to call onUpdate of the previous animation if there is one.</param>
		/// <param name="exceptions">Whether to catch and ignore any error that may crop up while animating.</param>
		/// <param name="tag">This object is passed to callback functions.</param>
		public void Animate(FieldOrProperty entry, object targetValue, double gradient, double linearSpeed, bool stoppable = true, UIAnimationHandler onUpdate = null, bool callIfFinished = true, ExceptionMode exceptions = ExceptionMode.Log, object tag = null) {
			if (IsDisposed || (entry.Field == null && entry.Property == null))
				return;
			AnimationInfo info;
			if (Extensions.DesignMode || !enabled || (SingleCore && DisableOnSingleCoreProcessors)) {
				info = AnimationInfo.CreateAnimation(this, entry, targetValue, targetValue, gradient, linearSpeed, stoppable, onUpdate, exceptions, tag);
				try {
					entry.SetValue(targetValue);
					info.state = AnimationState.UpdateSuccess;
				} catch (Exception e) {
					info.state = AnimationState.Halted;
					if (exceptions == ExceptionMode.Log)
						ErrorHandler.LogException(e, ErrorHandler.ExceptionToDetailedString("An error happened while updating the specified property.", e));
					else if (exceptions == ExceptionMode.Throw)
						throw;
				}
				if (onUpdate != null) {
					try {
						onUpdate(info);
					} catch (Exception e) {
						info.state = AnimationState.Halted;
						if (exceptions == ExceptionMode.Log)
							ErrorHandler.LogException(e, ErrorHandler.ExceptionToDetailedString("An error happened while calling the update callback when finished.", e));
						else if (exceptions == ExceptionMode.Throw)
							throw;
					}
				}
				if (info.state == AnimationState.UpdateSuccess) {
					info.state = AnimationState.Completed;
					if (onUpdate != null) {
						try {
							onUpdate(info);
						} catch (Exception e) {
							if (exceptions == ExceptionMode.Log)
								ErrorHandler.LogException(e, ErrorHandler.ExceptionToDetailedString("An error happened while calling the update callback when finished.", e));
							else if (exceptions == ExceptionMode.Throw)
								throw;
						}
					}
				}
				return;
			} else if (!entry.TargetType.IsAssignableFrom(targetValue.GetType()))
				throw new ArgumentException(string.Concat("The ", entry.Name, entry.Property == null ? " field" : " property", " type does not match or derive from the type of the target value."), nameof(targetValue));
			Halt(entry, true, false, callIfFinished);
			object currentValue = entry.Value;
			info = AnimationInfo.CreateAnimation(this, entry, currentValue, targetValue, gradient, linearSpeed, stoppable, onUpdate, exceptions, tag);
			if (currentValue.Equals(targetValue)) {
				info.state = AnimationState.Completed;
				if (onUpdate != null) {
					try {
						onUpdate(info);
					} catch (Exception e) {
						info.state = AnimationState.Halted;
						if (exceptions == ExceptionMode.Log)
							ErrorHandler.LogException(e, ErrorHandler.ExceptionToDetailedString("An error happened while calling the update callback when finished.", e));
						else if (exceptions == ExceptionMode.Throw)
							throw;
					}
				}
				return;
			}
			AnimationCollection.TryAdd(entry, info);
			lock (DictLock)
				AnimationTimer.Running = true;
		}

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		internal void Remove(FieldOrProperty info) {
			AnimationInfo temp;
			lock (DictLock) {
				AnimationCollection.TryRemove(info, out temp);
				if (AnimationCollection.Count == 0 && animationTimer != null)
					animationTimer.Running = false;
			}
		}

		/// <summary>
		/// Halts the animation on the specified field/property if it is running, and returns whether an animation was halted.
		/// </summary>
		/// <param name="entry">The field or property whose animation to halt.</param>
		/// <param name="haltUnstoppable">If true, even animations marked as unstoppable will be halted.</param>
		/// <param name="callIfFinished">Whether to call onUpdate of the previous animation if there is one.</param>
		public bool Halt(FieldOrProperty entry, bool haltUnstoppable = false, bool callIfFinished = true) {
			if (entry.Field == null && entry.Property == null)
				return false;
			else
				return Halt(entry, haltUnstoppable, true, callIfFinished);
		}

		private bool Halt(FieldOrProperty entry, bool haltUnstoppable, bool stopTimerIfEmpty, bool callIfFinished) {
			AnimationInfo anim;
			lock (DictLock) {
				if (AnimationCollection.TryGetValue(entry, out anim) && (haltUnstoppable || anim.Stoppable)) {
					AnimationCollection.TryRemove(entry, out anim);
					if (stopTimerIfEmpty && AnimationCollection.Count == 0 && animationTimer != null)
						animationTimer.Running = false;
				} else
					return false;
			}
			if (anim.state == AnimationState.Completed || anim.state == AnimationState.Halted)
				return true;
			UIAnimationHandler onUpdate = anim.OnUpdate;
			anim.OnUpdate = null;
			anim.state = AnimationState.Halted;
			if (callIfFinished && onUpdate != null) {
				ThreadPool.UnsafeQueueUserWorkItem(state => {
					bool lockWasTaken = false;
#if NET35
					try {
						lockWasTaken = Monitor.TryEnter(anim.SyncRoot, 7000);
#else
					lockWasTaken = false;
					try {
						Monitor.TryEnter(anim.SyncRoot, 7000, ref lockWasTaken);
#endif
						AnimationInfo ani = (AnimationInfo) state;
						try {
							onUpdate(ani);
						} catch (Exception e) {
							if (ani.Exceptions == ExceptionMode.Log)
								ErrorHandler.LogException(e, ErrorHandler.ExceptionToDetailedString("An error happened while calling the update callback when finished.", e));
							else if (ani.Exceptions == ExceptionMode.Throw)
								throw;
						}
					} finally {
						if (lockWasTaken)
							Monitor.Exit(anim.SyncRoot);
					}
				}, anim);
			}
			return true;
		}

		private void AnimationTimer_Tick(object sender) {
			foreach (KeyValuePair<FieldOrProperty, AnimationInfo> pair in AnimationCollection)
				pair.Value.Update(UpdateOnThreadPool);
			Action onUpdate = Updated;
			if (onUpdate != null)
				onUpdate();
		}

		/// <summary>
		/// Sets the specified field/property on the specified instance to the next step approaching the target value using the given animation parameters. Returns false when either the setting failed or it when the target value has been reached, else true.
		/// </summary>
		/// <param name="entry">The field/property whose value to animate.</param>
		/// <param name="targetValue">The target resultant value of the field (must be of a supported type).
		/// Supported types: Numeric values, Color, ColorF, Point, PointF, Size, SizeF, Rectangle, RectangleF, Vector2, Vector3, Vector4, Bitmap and PixelWorker.
		/// If using Bitmap or PixelWorker: the lock use for syncing animations is the 'Tag' property and the target image must be the same size and bits as the original.</param>
		/// <param name="gradient">The animation speed multiplier [0, 1] usually 0.4 (ie. the distance between the current value and the target value is multiplied by this value).</param>
		/// <param name="linearSpeed">The linear speed that is there to counteract the exponential infinity that one gets when dividing the distance between the current value and the target value with the animation speed.</param>
		/// <param name="exceptions">Whether to catch and ignore any error that may crop up while animating.</param>
		/// <param name="tag">This object is passed to callback functions.</param>
		public static AnimationState SetNextValue(FieldOrProperty entry, object targetValue, double gradient, double linearSpeed, ExceptionMode exceptions = ExceptionMode.Log, object tag = null) {
			if ((entry.Field == null && entry.Property == null))
				return AnimationState.Completed;
			else if (!entry.TargetType.IsAssignableFrom(targetValue.GetType()))
				throw new ArgumentException(string.Concat("The ", entry.Name, entry.Property == null ? " field " : " property ", "type does not match or derive from the type of the target value."), nameof(targetValue));
			AnimationInfo anim = AnimationInfo.CreateAnimation(null, entry, entry.Value, targetValue, gradient, linearSpeed, false, null, exceptions, tag);
			anim.SetNextValue();
			return anim.state;
		}

		/// <summary>
		/// Disposes of the animation timer.
		/// </summary>
		~UIAnimator() {
			Dispose();
		}

		/// <summary>
		/// Disposes of the animation timer.
		/// </summary>
		public void Dispose() {
			if (IsDisposed)
				return;
			IsDisposed = true;
			if (animationTimer != null) {
				animationTimer.Dispose();
				animationTimer = null;
			}
			AnimationCollection.Clear();
			GC.SuppressFinalize(this);
		}
	}
}