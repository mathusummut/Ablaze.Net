using System.Runtime.CompilerServices;

namespace System.Numerics {
	internal class Helper {
		/// <summary>
		/// Combines two hash codes, useful for combining hash codes of individual vector elements
		/// </summary>
		internal static int CombineHashCodes(int h1, int h2) {
			return (h1 << 5) + h1 ^ h2;
		}

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static unsafe byte GetByteWithAllBitsSet() {
			byte num = 0;
			*&num = byte.MaxValue;
			return num;
		}

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static unsafe sbyte GetSByteWithAllBitsSet() {
			sbyte num = 0;
			*&num = (sbyte) -1;
			return num;
		}

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static unsafe ushort GetUInt16WithAllBitsSet() {
			ushort num = 0;
			*&num = ushort.MaxValue;
			return num;
		}

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static unsafe short GetInt16WithAllBitsSet() {
			short num = 0;
			*&num = (short) -1;
			return num;
		}

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static unsafe uint GetUInt32WithAllBitsSet() {
			uint num = 0;
			*&num = uint.MaxValue;
			return num;
		}

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static unsafe int GetInt32WithAllBitsSet() {
			int num = 0;
			*&num = -1;
			return num;
		}

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static unsafe ulong GetUInt64WithAllBitsSet() {
			ulong num = 0;
			*&num = ulong.MaxValue;
			return num;
		}

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static unsafe long GetInt64WithAllBitsSet() {
			long num = 0;
			*&num = -1L;
			return num;
		}

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static unsafe float GetSingleWithAllBitsSet() {
			float num = 0.0f;
			*(int*) &num = -1;
			return num;
		}

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static unsafe double GetDoubleWithAllBitsSet() {
			double num = 0.0;
			*(long*) &num = -1L;
			return num;
		}
	}
}