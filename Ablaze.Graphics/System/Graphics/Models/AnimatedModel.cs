using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Graphics.Models {
	/// <summary>
	/// Animates a 3D model, where every child model is treated as separate frame.
	/// All the frames of a model must have same hierarchical structure, and same number of vertices
	/// </summary>
	public class AnimatedModel : Model {
		private PreciseStopwatch stopwatch = new PreciseStopwatch();
		private bool firstRunning = true;
		private float frameInterval, frameOffset;

		/// <summary>
		/// Gets or sets whether the animation is currently running
		/// </summary>
		public bool Running {
			get {
				return stopwatch.Running;
			}
			set {
				stopwatch.Running = value;
			}
		}

		/// <summary>
		/// If set to true, restarts the model animation to the first frame on next render
		/// </summary>
		public bool RestartOnNextRender {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the current frame, with an added decimal to represent the linear interpolation weight of the next frame
		/// </summary>
		public float CurrentFramePosition {
			get {
				float frameIndex = (float) (stopwatch.ElapsedMilliseconds / frameInterval);
				int frameCount = Count;
				if (!Loop) {
					if (frameIndex < 0)
						return 0;
					else if (frameIndex > frameCount - 1)
						return frameCount - 1;
				}
				float frame = (frameIndex + frameOffset) % frameCount;
				if (frame < 0f)
					frame += frameCount;
				return frame;
			}
			set {
				frameOffset = value;
				stopwatch.ElapsedTicks = 0.0;
			}
		}

		/// <summary>
		/// Gets the current frame
		/// </summary>
		public IModel Current {
			get {
				return this[(int) CurrentFramePosition];
			}
		}

		/// <summary>
		/// Gets or sets the current frame
		/// </summary>
		public int CurrentFrame {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return (int) CurrentFramePosition;
			}
			set {
				CurrentFramePosition = value;
			}
		}

		/// <summary>
		/// Gets or sets the animation frame interval in milliseconds
		/// </summary>
		public float FrameInterval {
			get {
				return frameInterval;
			}
			set {
				if (value == 0f)
					throw new ArgumentException("Frame interval cannot be 0");
				else
					frameInterval = Math.Abs(value);
			}
		}

		/// <summary>
		/// Gets or sets whether the animation is looped automatically
		/// </summary>
		public bool Loop {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets whether linear interpolation between frames is performed for smoother animations
		/// </summary>
		public bool LinearInterpolation {
			get;
			set;
		}

		/// <summary>
		/// Constructs a new animated model
		/// </summary>
		/// <param name="frameInterval">The animation frame interval in milliseconds</param>
		/// <param name="loop">Whether the animation is looped continuously</param>
		/// <param name="linearInterpolation">Whether linear interpolation between frames is performed for smoother animations</param>
		public AnimatedModel(float frameInterval = 41.67f, bool loop = true, bool linearInterpolation = true) {
			RestartOnNextRender = true;
			FrameInterval = frameInterval;
			Loop = loop;
			LinearInterpolation = linearInterpolation;
		}

		/// <summary>
		/// Renders the current frame of the model (must be called on a thread on which an OpenGL context is set up, preferably on the thread on which the model was loaded)
		/// </summary>
		/// <param name="nextModel">The next mesh component to interpolate with (can be null)</param>
		public override void Render(IModel nextModel) {
			if (firstRunning) {
				firstRunning = false;
				stopwatch.Running = true;
			}
			RaiseRenderBegin();
			IModel thisFrame = null;
			float interpolate = 0f;
			if (RestartOnNextRender) {
				RestartOnNextRender = false;
				CurrentFramePosition = 0f;
				lock (SyncRoot) {
					if (Count != 0)
						thisFrame = this[0];
				}
			} else {
				lock (SyncRoot) {
					float currentFrame = CurrentFramePosition;
					int frameIndex = (int) currentFrame;
					thisFrame = this[frameIndex];
					if (LinearInterpolation) {
						if (nextModel == null)
							nextModel = this[(frameIndex + 1) % Count];
						interpolate = currentFrame - frameIndex;
					}
				}
			}
			if (thisFrame != null) {
				if (nextModel != null)
					((GlobalShader) Shader.CurrentShader).SetUniformValue(GlobalShaderParams.Interpolate.ToString(), interpolate, ShaderSetMode.SetImmediately);
				thisFrame.Render(nextModel);
			}
			RaiseRenderEnd();
		}

		/// <summary>
		/// Goes to the next frame
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void GoToNextFrame() {
			frameOffset++;
		}

		/// <summary>
		/// Goes to the previous frame
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void GoToPreviousFrame() {
			frameOffset--;
		}

		/// <summary>
		/// Combines the frames of the specified animated model into the current animated model
		/// </summary>
		/// <param name="model">The animated model whose frames to combine. The number of frames in the model must the same as in this one</param>
		public void CombineWith(AnimatedModel model) {
			if (model == null)
				return;
			lock (SyncRoot) {
				lock (model.SyncRoot) {
					if (model.Count == 0)
						return;
					else if (Count == 0)
						AddRange(model);
					else {
						IModel current;
						Model modelList;
						for (int i = 0; i < Count; i++) {
							current = GetComponent(i);
							modelList = current as Model;
							if (modelList == null)
								this[i] = new Model(current, model.GetComponent(i));
							else
								modelList.Add(model.GetComponent(i));
						}
					}
				}
			}
		}
	}
}