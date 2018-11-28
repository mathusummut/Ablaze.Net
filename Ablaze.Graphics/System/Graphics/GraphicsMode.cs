using System.Runtime.CompilerServices;

namespace System.Graphics {
	/// <summary>
	/// Defines the pixel format for graphics operations.
	/// </summary>
	[Serializable]
	public struct GraphicsMode : IEquatable<GraphicsMode> {
		/// <summary>
		/// The ColorFormat of the color buffer.
		/// </summary>
		public readonly ColorFormat ColorFormat;
		/// <summary>
		/// The ColorFormat of the accumulation buffer.
		/// </summary>
		public readonly ColorFormat AccumFormat;
		/// <summary>
		/// The number of bits in the depth buffer.
		/// </summary>
		public readonly int Depth;
		/// <summary>
		/// The number of bits in the stencil buffer.
		/// </summary>
		public readonly int Stencil;
		/// <summary>
		/// Whether to use double-buffering.
		/// </summary>
		public readonly bool DoubleBuffering;
		/// <summary>
		/// The number of samples for FSAA.
		/// </summary>
		public readonly int Samples;
		/// <summary>
		/// Set to true for a GraphicsMode with stereographic capabilities.
		/// </summary>
		public readonly bool Stereo;
		internal readonly IntPtr? Index;
		/// <summary>
		/// Represents an empty GraphicsMode.
		/// </summary>
		public static readonly GraphicsMode Empty = new GraphicsMode();
		/// <summary>
		/// Returns the default GraphicsMode.
		/// </summary>
		public static readonly GraphicsMode Default = new GraphicsMode(null, ColorFormat.Bit32, 24, 8, 0, ColorFormat.Empty, true, false);

		internal GraphicsMode(IntPtr? index, ColorFormat color, int depth, int stencil, int samples, ColorFormat accum, bool doubleBuffering, bool stereo) {
			Index = index;
			ColorFormat = color;
			Depth = depth;
			Stencil = stencil;
			Samples = samples < 0 ? 0 : (samples > 64 ? 64 : (int) Maths.FloorPowerOfTwo((uint) samples));
			AccumFormat = accum;
			DoubleBuffering = doubleBuffering;
			Stereo = stereo;
		}

		/// <summary>Constructs a new GraphicsMode with the specified parameters.</summary>
		/// <param name="color">The ColorFormat of the color buffer.</param>
		public GraphicsMode(ColorFormat color)
			: this(null, color, Default.Depth, Default.Stencil, Default.Samples, Default.AccumFormat, Default.DoubleBuffering, Default.Stereo) {
		}

		/// <summary>Constructs a new GraphicsMode with the specified parameters.</summary>
		/// <param name="color">The ColorFormat of the color buffer.</param>
		/// <param name="depth">The number of bits in the depth buffer.</param>
		public GraphicsMode(ColorFormat color, int depth)
			: this(null, color, depth, Default.Stencil, Default.Samples, Default.AccumFormat, Default.DoubleBuffering, Default.Stereo) {
		}

		/// <summary>Constructs a new GraphicsMode with the specified parameters.</summary>
		/// <param name="color">The ColorFormat of the color buffer.</param>
		/// <param name="depth">The number of bits in the depth buffer.</param>
		/// <param name="stencil">The number of bits in the stencil buffer.</param>
		public GraphicsMode(ColorFormat color, int depth, int stencil)
			: this(null, color, depth, stencil, Default.Samples, Default.AccumFormat, Default.DoubleBuffering, Default.Stereo) {
		}

		/// <summary>Constructs a new GraphicsMode with the specified parameters.</summary>
		/// <param name="color">The ColorFormat of the color buffer.</param>
		/// <param name="depth">The number of bits in the depth buffer.</param>
		/// <param name="stencil">The number of bits in the stencil buffer.</param>
		/// <param name="samples">The number of samples for FSAA.</param>
		public GraphicsMode(ColorFormat color, int depth, int stencil, int samples)
			: this(null, color, depth, stencil, samples, Default.AccumFormat, Default.DoubleBuffering, Default.Stereo) {
		}

		/// <summary>Constructs a new GraphicsMode with the specified parameters.</summary>
		/// <param name="color">The ColorFormat of the color buffer.</param>
		/// <param name="depth">The number of bits in the depth buffer.</param>
		/// <param name="stencil">The number of bits in the stencil buffer.</param>
		/// <param name="samples">The number of samples for FSAA.</param>
		/// <param name="accum">The ColorFormat of the accumulation buffer.</param>
		public GraphicsMode(ColorFormat color, int depth, int stencil, int samples, ColorFormat accum)
			: this(null, color, depth, stencil, samples, accum, Default.DoubleBuffering, Default.Stereo) {
		}

		/// <summary>Constructs a new GraphicsMode with the specified parameters.</summary>
		/// <param name="color">The ColorFormat of the color buffer.</param>
		/// <param name="depth">The number of bits in the depth buffer.</param>
		/// <param name="stencil">The number of bits in the stencil buffer.</param>
		/// <param name="samples">The number of samples for FSAA.</param>
		/// <param name="accum">The ColorFormat of the accumulation buffer.</param>
		/// <param name="doubleBuffering">Whether to use double-buffering.</param>
		public GraphicsMode(ColorFormat color, int depth, int stencil, int samples, ColorFormat accum, bool doubleBuffering)
			: this(null, color, depth, stencil, samples, accum, doubleBuffering, Default.Stereo) {
		}

		/// <summary>Constructs a new GraphicsMode with the specified parameters.</summary>
		/// <param name="color">The ColorFormat of the color buffer.</param>
		/// <param name="depth">The number of bits in the depth buffer.</param>
		/// <param name="stencil">The number of bits in the stencil buffer.</param>
		/// <param name="samples">The number of samples for FSAA.</param>
		/// <param name="accum">The ColorFormat of the accumulation buffer.</param>
		/// <param name="stereo">Set to true for a GraphicsMode with stereographic capabilities.</param>
		/// <param name="doubleBuffering">Whether to use double-buffering.</param>
		public GraphicsMode(ColorFormat color, int depth, int stencil, int samples, ColorFormat accum, bool doubleBuffering, bool stereo)
			: this(null, color, depth, stencil, samples, accum, doubleBuffering, stereo) {
		}

		/// <summary>Returns a System.String describing the current GraphicsFormat.</summary>
		/// <returns>! System.String describing the current GraphicsFormat.</returns>
		public override string ToString() {
			return string.Format("Index: {0}, Color: {1}, Depth: {2}, Stencil: {3}, Samples: {4}, Accum: {5}, Double-Buffering: {6}, Stereo: {7}",
				Index, ColorFormat, Depth, Stencil, Samples, AccumFormat, DoubleBuffering, Stereo);
		}

		/// <summary>
		/// Returns the hashcode for this instance.
		/// </summary>
		/// <returns>A <see cref="Int32"/> hashcode for this instance.</returns>
		public override int GetHashCode() {
			return unchecked((ColorFormat.GetHashCode() << 17) ^ (Depth << 13) ^ Index.GetHashCode());
		}

		/// <summary>
		/// Indicates whether obj is equal to this instance.
		/// </summary>
		/// <param name="obj">An object instance to compare for equality.</param>
		/// <returns>True, if obj equals this instance; false otherwise.</returns>
		public override bool Equals(object obj) {
			return obj is GraphicsMode ? Equals((GraphicsMode) obj) : false;
		}

		/// <summary>
		/// Indicates whether other represents the same mode as this instance.
		/// </summary>
		/// <param name="other">The GraphicsMode to compare to.</param>
		/// <returns>True, if other is equal to this instance; false otherwise.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool Equals(GraphicsMode other) {
			return (Index.HasValue && other.Index.HasValue && Index == other.Index) ||
				(ColorFormat == other.ColorFormat && AccumFormat == other.AccumFormat && Depth == other.Depth &&
				Stencil == other.Stencil && DoubleBuffering == other.DoubleBuffering && Samples == other.Samples);
		}

		/// <summary>
		/// Compares two instances for equality.
		/// </summary>
		/// <param name="left">The left operand.</param>
		/// <param name="right">The right operand.</param>
		/// <returns>True if both instances are equal; false otherwise.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator ==(GraphicsMode left, GraphicsMode right) {
			return left.Equals(right);
		}

		/// <summary>
		/// Compares two instances for inequality.
		/// </summary>
		/// <param name="left">The left operand.</param>
		/// <param name="right">The right operand.</param>
		/// <returns>True if both instances are not equal; false otherwise.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator !=(GraphicsMode left, GraphicsMode right) {
			return !left.Equals(right);
		}
	}
}