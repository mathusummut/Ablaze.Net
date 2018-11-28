using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace System {
	/// <summary>
	/// Contains methods for generating random numbers.
	/// </summary>
	public static class UniformRandom {
		private static ulong[] Seeds = new ulong[56];
		/// <summary>
		/// The current random value index counter.
		/// </summary>
		private static uint Index;

		/// <summary>
		/// Generates a random color (alpha is always 255).
		/// </summary>
		public static Color RandomColor {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return Color.FromArgb(RandomByte, RandomByte, RandomByte);
			}
		}

		/// <summary>
		/// Generates a random color with a random alpha component.
		/// </summary>
		public static Color RandomColorWithAlpha {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return Color.FromArgb(RandomByte, RandomByte, RandomByte, RandomByte);
			}
		}

		/// <summary>
		/// Generates a random byte.
		/// </summary>
		[CLSCompliant(false)]
		public static byte RandomByte {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return (byte) (Random % 256);
			}
		}

		/// <summary>
		/// Generates a random integer.
		/// </summary>
		[CLSCompliant(false)]
		public static ulong Random {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				unchecked {
					uint index = ++Index % 56;
					ulong value = Seeds[index] - Seeds[(index + 21) % 56];
					Seeds[index] = value;
					return value;
				}
			}
		}

		/// <summary>
		/// Generates a random value in the range [0, 1).
		/// </summary>
		[CLSCompliant(false)]
		public static float RandomFloat {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return (float) (Random * 5.421010862427522170331137592055280434137021303416885e-20);
			}
		}

		/// <summary>
		/// Generates a random value in the range [0, 1).
		/// </summary>
		[CLSCompliant(false)]
		public static double RandomDouble {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return Random * 5.421010862427522170331137592055280434137021303416885e-20;
			}
		}

		static UniformRandom() {
			unchecked {
				ulong seed = 1389880606230557208ul - (ulong) PreciseStopwatch.TimeStamp;
				Seeds[55] = seed;
				ulong num = 1;
				for (int num1, i = 1; i < 55; i++) {
					num1 = 21 * i % 55;
					Seeds[num1] = num;
					num = seed - num;
					seed = Seeds[num1];
				}
				int j;
				for (int i = 1; i < 5; i++) {
					for (j = 1; j < 56; j++)
						Seeds[j] -= Seeds[1 + (j + 30) % 55];
				}
			}
		}

		/// <summary>
		/// Returns a random double in the specified range for the specified integer value.
		/// </summary>
		/// <param name="value">The value to map to the specified range.</param>
		/// <param name="min">The inclusive minimum value in the range.</param>
		/// <param name="max">The excluded maximum value in the range.</param>
		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static double ToInterval(this ulong value, double min, double max) {
			if (min > max) {
				double temp = min;
				min = max;
				max = temp;
			}
			return (value * 5.421010862427522170331137592055280434137021303416885e-20) * (max - min) + min;
		}
	}
}