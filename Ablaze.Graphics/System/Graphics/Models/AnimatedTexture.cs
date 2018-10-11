using System.Collections.Generic;
using System.Diagnostics;
using System.Graphics.OGL;
using System.Runtime.CompilerServices;

namespace System.Graphics.Models {
	/// <summary>
	/// Animates the textures in the collection by moving the selected index as time goes by
	/// </summary>
	public class AnimatedTexture : TextureCollection {
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
		/// If set to true, restarts the texture animation to the first frame on next bind
		/// </summary>
		public bool RestartOnNextRender {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the current texture, with an added decimal to represent the linear interpolation weight of the next texture
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
		/// Gets or sets the current texture
		/// </summary>
		public override int BindIndex {
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
		/// Gets or sets whether the texture animation is looped automatically
		/// </summary>
		public bool Loop {
			get;
			set;
		}

		/// <summary>
		/// Constructs a new animated texture collection
		/// </summary>
		/// <param name="frameInterval">The animation frame interval in milliseconds</param>
		/// <param name="loop">Whether the animation is looped continuously</param>
		public AnimatedTexture(float frameInterval = 41.67f, bool loop = true) {
			RestartOnNextRender = true;
			FrameInterval = frameInterval;
			Loop = loop;
		}

		/// <summary>
		/// Binds the texture at the index specified by BindIndex for use with OpenGL operations
		/// </summary>
		public override void Bind() {
			if (firstRunning) {
				firstRunning = false;
				stopwatch.Running = true;
			}
			if (RestartOnNextRender) {
				RestartOnNextRender = false;
				CurrentFramePosition = 0f;
			}
			base.Bind();
		}

		/// <summary>
		/// Binds the texture at the index specified by BindIndex for use with OpenGL operations
		/// </summary>
		/// <param name="mode">The texture wrap mode to use</param>
		public override void Bind(TextureWrapMode mode) {
			if (firstRunning) {
				firstRunning = false;
				stopwatch.Running = true;
			}
			if (RestartOnNextRender) {
				RestartOnNextRender = false;
				CurrentFramePosition = 0f;
			}
			base.Bind(mode);
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
		/// Combines the frames of the specified animated texture into the current animated texture
		/// </summary>
		/// <param name="collection">The animated texture whose frames to combine. The number of frames in the collection must the same as in this one</param>
		public void CombineWith(AnimatedTexture collection) {
			if (collection == null)
				return;
			lock (SyncRoot) {
				lock (collection.SyncRoot) {
					if (collection.Count == 0)
						return;
					else if (Count == 0)
						AddRange((IEnumerable<ITexture>) collection);
					else {
						ITexture current;
						TextureCollection textureList;
						for (int i = 0; i < Count; i++) {
							current = GetTexture(i);
							textureList = current as TextureCollection;
							if (textureList == null)
								this[i] = new TextureCollection(current, collection.GetTexture(i));
							else
								textureList.Add(collection.GetTexture(i));
						}
					}
				}
			}
		}
	}
}