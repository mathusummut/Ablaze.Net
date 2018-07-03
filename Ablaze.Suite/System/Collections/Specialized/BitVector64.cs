using System.Runtime.CompilerServices;
using System.Text;

namespace System.Collections.Specialized {
	using Section = BitVector32.Section;

	/// <summary>
	/// Provides a simple structure that stores Boolean values and small integers in 64 bits of memory.
	/// </summary>
	public struct BitVector64 : IEquatable<BitVector64> {
		/// <summary>
		/// The actual numeric data representation of this BitVector.
		/// </summary>
		[CLSCompliant(false)]
		public ulong Data;

		/// <summary>
		/// Gets or sets the specified bit.
		/// </summary>
		/// <param name="bitIndex">The bit index to get or set (0 to 63).</param>
		public bool this[int bitIndex] {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				ulong mask = 1ul << bitIndex;
				return (Data & mask) == mask;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				if (value)
					Data |= 1u << bitIndex;
				else
					Data &= ~(1u << bitIndex);
			}
		}

		/// <summary>
		/// Gets or sets the value stored in the specified Section.
		/// </summary>
		/// <param name="section">The section whose value to get or set.</param>
		[CLSCompliant(false)]
		public ulong this[Section section] {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return (Data >> section.Offset) & (ulong) section.Mask;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				Data &= (ulong) ~(section.Mask << section.Offset);
				Data |= (value << section.Offset);
			}
		}

		/// <summary>
		/// Initializes a new BitVector64 from the specified source.
		/// </summary>
		/// <param name="source">The data source.</param>
		public BitVector64(BitVector32 source) {
			Data = unchecked((ulong) source.Data);
		}

		/// <summary>
		/// Initializes a new BitVector64 from the specified source.
		/// </summary>
		/// <param name="source">The data source.</param>
		[CLSCompliant(false)]
		public BitVector64(ulong source) {
			Data = source;
		}

		/// <summary>
		/// Initializes a new BitVector64 from the specified source.
		/// </summary>
		/// <param name="source">The data source.</param>
		public BitVector64(int source) {
			Data = unchecked((ulong) source);
		}

		/// <summary>
		/// Gets whether the specified flags are set by applying the mask using an AND operation.
		/// </summary>
		/// <param name="mask">The mask to apply.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public bool AreSet(ulong mask) {
			return (Data & mask) == mask;
		}

		/// <summary>
		/// Sets the specified value to the flags specified by the mask.
		/// </summary>
		/// <param name="mask">The mask of the bits to set.</param>
		/// <param name="value">The value to set.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public void SetFlags(ulong mask, bool value) {
			if (value)
				Data |= mask;
			else
				Data &= ~mask;
		}

		/// <summary>
		/// Creates the first mask in a series of masks that can be used to retrieve individual bits in a BitVector64 that is set up as bit flags.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static long CreateMask() {
			return CreateMask(0);   // 1;
		}

		/// <summary>
		/// Creates an additional mask following the specified mask in a series of masks that can be used to retrieve individual bits in a BitVector64 that is set up as bit flags.
		/// </summary>
		/// <param name="previous">The mask that indicates the previous bit flag.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static long CreateMask(long previous) {
			return previous == 0 ? 1 : previous << 1;
		}

		/// <summary>
		/// Creates the first Section in a series of sections that contain small integers.
		/// </summary>
		/// <param name="maxValue">A 32-bit signed integer that specifies the maximum value for the new Section.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Section CreateSection(int maxValue) {
			return CreateSection(maxValue, new Section());
		}

		/// <summary>
		/// Creates a new Section following the specified Section in a series of sections that contain small integers.
		/// </summary>
		/// <param name="maxValue">A 32-bit signed integer that specifies the maximum value for the new Section.</param>
		/// <param name="previous">The previous section in the BitVector.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Section CreateSection(int maxValue, Section previous) {
			Section section = new Section();
			unsafe
			{
				*((short*) &section) = (short) (Maths.CeilingPowerOfTwo((uint) maxValue + 1) - 1); //mask
				*(((short*) &section) + 1) = (short) (previous.Offset + Maths.NumberOfSetBits(previous.Mask)); //offset
			}
			return section;
		}

		/// <summary>
		/// Gets whether the specified BitVector64 instances have the same bits set.
		/// </summary>
		/// <param name="left">The first set of flags to compare.</param>
		/// <param name="right">The second set of flags to compare.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator ==(BitVector64 left, BitVector64 right) {
			return left.Data == right.Data;
		}

		/// <summary>
		/// Gets whether the specified BitVector64 instances have different bits set.
		/// </summary>
		/// <param name="left">The first set of flags to compare.</param>
		/// <param name="right">The second set of flags to compare.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator !=(BitVector64 left, BitVector64 right) {
			return left.Data != right.Data;
		}

		/// <summary>
		/// Gets whether the specified BitVector64 is considered equal to this instance.
		/// </summary>
		/// <param name="bits">The BitVector64 to check for equality.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool Equals(BitVector64 bits) {
			return Data == bits.Data;
		}

		/// <summary>
		/// Gets whether the specified object is considered equal to this instance.
		/// </summary>
		/// <param name="obj">The object to check for equality.</param>
		public override bool Equals(object obj) {
			return obj is BitVector64 && Data == ((BitVector64) obj).Data;
		}

		/// <summary>
		/// Gets a hash code from this instance.
		/// </summary>
		public override int GetHashCode() {
			return Data.GetHashCode();
		}

		/// <summary>
		/// Gets a string that represents this instance.
		/// </summary>
		public override string ToString() {
			StringBuilder sb = new StringBuilder(0x2d);
			sb.Append("BitVector64{");
			ulong data = Data;
			for (int i = 0; i < 0x40; i++) {
				sb.Append(((data & 0x8000000000000000) == 0) ? '0' : '1');
				data = data << 1;
			}

			sb.Append("}");
			return sb.ToString();
		}
	}
}