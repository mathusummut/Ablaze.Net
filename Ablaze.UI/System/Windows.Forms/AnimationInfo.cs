#if !NET35
using System.Collections.Concurrent;
#endif
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace System.Windows.Forms {
	/// <summary>
	/// Called when an animation is updated. If the handler returns false, the animation will halt.
	/// </summary>
	/// <param name="state">The current state and parameters of the animation.</param>
	public delegate bool UIAnimationHandler(AnimationInfo state);

	/// <summary>
	/// Holds the animation parameters and state for the UIAnimator class.
	/// </summary>
	public abstract class AnimationInfo {
		private static Type[] constructorTypes = new Type[] {
			typeof(UIAnimator), typeof(FieldOrProperty), null, typeof(object), typeof(double), typeof(double), typeof(bool),
			typeof(UIAnimationHandler), typeof(ExceptionMode), typeof(object)
		};
		private static Type TypeOfAnimationInfo = typeof(AnimationInfo<>);
		/// <summary>
		/// The object that is transferred along with the animation.
		/// </summary>
		public object Tag;
		private double gradient, linearSpeed;
		/// <summary>
		/// Used for synchronizing animation callbacks.
		/// </summary>
		public readonly object SyncRoot = new object();
		/// <summary>
		/// If true, the animation will stop if the value didn't update as expected.
		/// </summary>
		public bool Stoppable;
		/// <summary>
		/// The elapsed time from the last update.
		/// </summary>
		public readonly PreciseStopwatch Elapsed = new PreciseStopwatch();
		/// <summary>
		/// The current state of the animation.
		/// </summary>
		internal AnimationState state;
		internal object targetValue;
		/// <summary>
		/// The field/property that is updated by this animation.
		/// </summary>
		public readonly FieldOrProperty Info;
		/// <summary>
		/// The parent animator of this instance.
		/// </summary>
		public readonly UIAnimator Parent;
		/// <summary>
		/// The function to be called after the value is updated.
		/// </summary>
		public UIAnimationHandler OnUpdate;
		/// <summary>
		/// Gets or sets the way to handle exceptions.
		/// </summary>
		public ExceptionMode Exceptions;
		/// <summary>
		/// Whether the animated field is a value type.
		/// </summary>
		public readonly bool IsValueType;

		/// <summary>
		/// Gets or sets the target value to approach with the animation.
		/// </summary>
		public object TargetValue {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return targetValue;
			}
		}

		/// <summary>
		/// The linear speed that is there to counteract the exponential infinity that one gets when dividing the distance between the current value and the target value with the animation speed.
		/// </summary>
		public double LinearSpeed {
			get {
				return linearSpeed;
			}
			set {
				linearSpeed = Math.Abs(value);
			}
		}

		/// <summary>
		/// Gets whether the animation is finished.
		/// </summary>
		public bool IsFinished {
			get {
				return !(state == AnimationState.NotYetBegun || state == AnimationState.UpdateSuccess);
			}
		}

		/// <summary>
		/// Gets the current state of the animation.
		/// </summary>
		public AnimationState State {
			get {
				return state;
			}
		}

		/// <summary>
		/// Gets or sets the animation speed multiplier [0, 1] (ie. the distance between the current value and the target value is multiplied by this value).
		/// </summary>
		public double Gradient {
			get {
				return gradient;
			}
			set {
				gradient = Math.Min(Math.Max(value, 0.01), 1.0);
			}
		}

		internal AnimationInfo(FieldOrProperty info, bool isValueType, UIAnimator parent, object tag) {
			Info = info;
			IsValueType = isValueType;
			Parent = parent;
			Tag = tag;
			Elapsed.Running = true;
		}

		internal static AnimationInfo CreateAnimation(UIAnimator parent, FieldOrProperty info, object currentValue, object targetValue, double gradient, double linearSpeed, bool stoppable, UIAnimationHandler onUpdate, ExceptionMode exceptions, object tag) {
			Type[] constructorSignature = new Type[10];
			Array.Copy(constructorTypes, constructorSignature, 10);
			constructorSignature[2] = info.TargetType;
			return (AnimationInfo) TypeOfAnimationInfo.MakeGenericType(info.TargetType).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, constructorSignature, null).Invoke(new object[] { parent, info, currentValue, targetValue, gradient, linearSpeed, stoppable, onUpdate, exceptions, tag });
		}

		internal abstract bool SetNextValue();

		/// <summary>
		/// Updates the animation.
		/// </summary>
		public abstract void Update(bool updateOnThreadPool = true);
	}

	/// <summary>
	/// Holds the animation parameters and state for the UIAnimator class.
	/// </summary>
	/// <typeparam name="T">The type of field/property that is updated.</typeparam>
	public sealed class AnimationInfo<T> : AnimationInfo {
		private static Type typeOfT = typeof(T);
		private static Dictionary<Type, MethodInfo> TransitionHandlerLookup = new Dictionary<Type, MethodInfo>();
		private delegate AnimationState TransitionHandler(T currentValue, object targetValue, double gradient, double linearSpeed, bool stoppable, ref object cachedValue, out T newValue);
		private static MethodInfo fieldSetter = typeof(Extensions).GetMethod(nameof(Extensions.CreateSetter));
		private static MethodInfo createSetter = typeof(AnimationInfo<T>).GetMethod(nameof(CreateSetter), BindingFlags.NonPublic | BindingFlags.Instance);
		private static Type TypeOfTransitionHandler = typeof(TransitionHandler), TypeOfActionT = typeof(Action<T>);
		private static TransitionHandler transitionHandler;
		/// <summary>
		/// The cached value of the property.
		/// </summary>
		private object CachedValue;
		private Action<T> setValue;
		/// <summary>
		/// The current value of the property. If the animation has completed successfully, the current value will be equal to the target value. If an error has happened, it may be null.
		/// </summary>
		private T currentValue;
		private static WaitCallback update = UpdateInner;
		private byte waitCount;

		/// <summary>
		/// The cached current value of the property. If the animation has completed successfully,
		/// the current value will be equal to the target value. If an error has happened, it may be null.
		/// </summary>
		public T CurrentValue {
			get {
				return currentValue;
			}
		}

		static AnimationInfo() {
			if (!StyledForm.DesignMode)
				LoadHandlers();
		}

		private AnimationInfo(UIAnimator parent, FieldOrProperty info, T currentValue, object targetValue, double gradient, double linearSpeed, bool stoppable, UIAnimationHandler onUpdate, ExceptionMode exceptions, object tag)
			: base(info, info.TargetType == null ? false : info.TargetType.IsValueType, parent, tag) {
			Exceptions = exceptions;
			this.currentValue = currentValue;
			Stoppable = stoppable;
			this.targetValue = targetValue;
			Gradient = gradient;
			LinearSpeed = linearSpeed;
			OnUpdate = onUpdate;
			state = AnimationState.NotYetBegun;
		}

		/// <summary>
		/// Changes the target value of this animation.
		/// </summary>
		/// <param name="newValue">The new target value.</param>
		public void ChangeTargetValue(object newValue) {
			CachedValue = null;
			targetValue = newValue;
			try {
				currentValue = (T) Info.Value;
			} catch (Exception e) {
				if (Exceptions == ExceptionMode.Log)
					ErrorHandler.LogException(e, ErrorHandler.ExceptionToDetailedString("An error happened while updating the target value of the specified property.", e));
				else if (Exceptions == ExceptionMode.Throw)
					throw;
			}
		}

		private static void LoadHandlers() {
			if (TransitionHandlerLookup.Count != 0)
				return;
			Assembly[] assemblies;
			try {
				assemblies = AppDomain.CurrentDomain.GetAssemblies();
			} catch {
				return;
			}
#if NET35
			Dictionary<Type, MethodInfo> transitionHandlerLookup = new Dictionary<Type, MethodInfo>();
			foreach (Assembly assembly in assemblies) {
#else
			ConcurrentDictionary<Type, MethodInfo> transitionHandlerLookup = new ConcurrentDictionary<Type, MethodInfo>();
			Parallel.ForEach(assemblies, assembly => {
#endif
				try {
#if NET35
					foreach (Type type in assembly.GetTypes()) {
#else
					Parallel.ForEach(assembly.GetTypes(), type => {
#endif
						try {
							MethodInfo[] methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
							MethodInfo method;
							object[] attributes;
							TransitionHandlerAttribute transitionAttr;
							for (int k = 0; k < methods.Length; k++) {
								method = methods[k];
								try {
									attributes = method.GetCustomAttributes(TransitionHandlerAttribute.Type, false);
									if (!(attributes == null || attributes.Length == 0)) {
										transitionAttr = attributes[0] as TransitionHandlerAttribute;
										if (transitionAttr != null) {
#if NET35
											transitionHandlerLookup.Add(transitionAttr.TargetType, method);
#else
											transitionHandlerLookup.TryAdd(transitionAttr.TargetType, method);
#endif
										}
									}
								} catch {
								}
							}
						} catch {
						}
					}
#if !NET35
					);
#endif
				} catch {
				}
			}
#if NET35
			TransitionHandlerLookup = transitionHandlerLookup;
#else
			);
			TransitionHandlerLookup = new Dictionary<Type, MethodInfo>(transitionHandlerLookup);
#endif
		}

		/// <summary>
		/// Updates the animation.
		/// </summary>
		public override void Update(bool updateOnThreadPool = true) {
			if (state != AnimationState.Halted && Volatile.Read(ref waitCount) < 2) {
				if (updateOnThreadPool)
					ThreadPool.UnsafeQueueUserWorkItem(update, this);
				else
					UpdateInner(this);
			}
		}

		private static void UpdateInner(object instance) {
			AnimationInfo<T> anim = (AnimationInfo<T>) instance;
			byte value = Volatile.Read(ref anim.waitCount);
			if (value >= 2)
				return;
			else
				Volatile.Write(ref anim.waitCount, (byte) (value + 1));
			UIAnimationHandler onUpdate;
			bool lockWasTaken = false;
#if NET35
			try {
				lockWasTaken = Monitor.TryEnter(anim.SyncRoot, 7000);
#else
			lockWasTaken = false;
			try {
				Monitor.TryEnter(anim.SyncRoot, 7000, ref lockWasTaken);
#endif
				bool needsUpdate = lockWasTaken && anim.SetNextValue();
				if (lockWasTaken && anim.state == AnimationState.UpdateSuccess) {
					if (needsUpdate) {
						onUpdate = anim.OnUpdate;
						if (onUpdate != null) {
							try {
								if (!onUpdate(anim)) {
									anim.state = AnimationState.Halted;
									if (anim.Parent != null)
										anim.Parent.Remove(anim.Info);
								}
							} catch (Exception e) {
								anim.state = AnimationState.Halted;
								if (anim.Parent != null)
									anim.Parent.Remove(anim.Info);
								if (anim.Exceptions == ExceptionMode.Log)
									ErrorHandler.LogException(e, ErrorHandler.ExceptionToDetailedString("An error happened while updating the specified property.", e));
								else if (anim.Exceptions == ExceptionMode.Throw)
									throw;
							}
						}
					}
				} else {
					if (anim.Parent != null)
						anim.Parent.Remove(anim.Info);
					onUpdate = anim.OnUpdate;
					anim.OnUpdate = null;
					if (onUpdate != null)
						onUpdate(anim);
				}
			} catch (Exception e) {
				if (anim.Exceptions == ExceptionMode.Log)
					ErrorHandler.LogException(e, ErrorHandler.ExceptionToDetailedString("An error happened while calling the update callback when finished.", e));
				else if (anim.Exceptions == ExceptionMode.Throw)
					throw;
			} finally {
				if (lockWasTaken)
					Monitor.Exit(anim.SyncRoot);
				anim.waitCount--;
			}
		}

		internal override bool SetNextValue() {
			double multiplier = PreciseStopwatch.ConvertToMilliseconds(Elapsed.RestartGet()) * 0.0333333333333333333333333;
			if (currentValue == null) {
				state = AnimationState.Completed;
				return false;
			} else {
				try {
					T oldCurrentValue = currentValue;
					if (transitionHandler == null) {
						LoadHandlers();
						transitionHandler = (TransitionHandler) Delegate.CreateDelegate(TypeOfTransitionHandler, TransitionHandlerLookup[Info.TargetType]);
					}
					state = transitionHandler(currentValue, targetValue, Gradient * multiplier, Math.Max(LinearSpeed * multiplier, 0.00001), Stoppable, ref CachedValue, out currentValue);
					if (oldCurrentValue.Equals(currentValue)) {
						if (!IsValueType && state == AnimationState.UpdateSuccess) {
							SetValue(currentValue);
							return true;
						} else
							return false;
					} else {
						if (state == AnimationState.UpdateSuccess)
							SetValue(currentValue);
						return true;
					}
				} catch (Exception e) {
					state = AnimationState.Halted;
					if (Exceptions == ExceptionMode.Throw)
						throw;
					else {
						if (Exceptions == ExceptionMode.Log)
							ErrorHandler.LogException(e, ErrorHandler.ExceptionToDetailedString("An error happened while updating the specified property.", e));
						return false;
					}
				}
			}
		}

		private void SetValue(T value) {
			if (setValue == null) {
				if (Info.Field == null && Info.Property == null)
					return;
				if (Info.Field == null) {
					MethodInfo setMethod = Info.Property.GetSetMethod(false);
					if (setMethod == null)
						setMethod = Info.Property.GetSetMethod(true);
					setValue = (Action<T>) Delegate.CreateDelegate(TypeOfActionT, Info.Instance, setMethod);
				} else
					setValue = (Action<T>) createSetter.MakeGenericMethod(new Type[] { Info.ContaningType }).Invoke(this, new object[] { fieldSetter.MakeGenericMethod(new Type[] { Info.ContaningType, typeOfT }).Invoke(null, new object[] { Info.Field }) });
			}
			setValue(value);
		}

		private Action<T> CreateSetter<S>(object setter) {
			Action<S, T> set = (Action<S, T>) setter;
			return delegate (T value) {
				set((S) Info.Instance, value);
			};
		}

		/// <summary>
		/// Gets a string that represents this instance.
		/// </summary>
		public override string ToString() {
			if (Info == null)
				return "null";
			StringBuilder builder = new StringBuilder(Info.ToString());
			builder.Append(": {");
			if (currentValue != null) {
				builder.Append(" Value: ");
				builder.Append(currentValue.ToString());
				builder.Append(",");
			}
			if (targetValue != null) {
				builder.Append(" Target: ");
				builder.Append(targetValue.ToString());
				builder.Append(",");
			}
			builder.Append(" State: ");
			builder.Append(state.ToString());
			builder.Append(" }");
			return builder.ToString();
		}
	}
}