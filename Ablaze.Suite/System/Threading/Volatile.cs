#if !NET45

using System.Runtime.Versioning;
using System.Runtime.ConstrainedExecution;
using System.Security;

namespace System.Threading {
	/// <summary>
	/// Methods for accessing memory with volatile semantics. These are preferred over Thread.VolatileRead
	/// and Thread.VolatileWrite, as these are implemented more efficiently.
	/// </summary>
	public static class Volatile {
		/// <summary>
		/// Reads the value of a field. The value is the latest written by any processor in a computer,
		/// regardless of the number of processors or the state of processor cache.</summary>
		/// <returns>The latest value written to the field by any processor.</returns>
		/// <param name="location">The field to be read.</param>
		[ResourceExposure(ResourceScope.None)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static bool Read(ref bool location) {
			bool value = location;
			Thread.MemoryBarrier();
			return value;
		}

		/// <summary>
		/// Reads the value of a field. The value is the latest written by any processor in a computer,
		/// regardless of the number of processors or the state of processor cache.</summary>
		/// <returns>The latest value written to the field by any processor.</returns>
		/// <param name="location">The field to be read.</param>
		[ResourceExposure(ResourceScope.None)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[CLSCompliant(false)]
		public static sbyte Read(ref sbyte location) {
			sbyte value = location;
			Thread.MemoryBarrier();
			return value;
		}

		/// <summary>
		/// Reads the value of a field. The value is the latest written by any processor in a computer,
		/// regardless of the number of processors or the state of processor cache.</summary>
		/// <returns>The latest value written to the field by any processor.</returns>
		/// <param name="location">The field to be read.</param>
		[ResourceExposure(ResourceScope.None)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static byte Read(ref byte location) {
			byte value = location;
			Thread.MemoryBarrier();
			return value;
		}

		/// <summary>
		/// Reads the value of a field. The value is the latest written by any processor in a computer,
		/// regardless of the number of processors or the state of processor cache.</summary>
		/// <returns>The latest value written to the field by any processor.</returns>
		/// <param name="location">The field to be read.</param>
		[ResourceExposure(ResourceScope.None)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static short Read(ref short location) {
			short value = location;
			Thread.MemoryBarrier();
			return value;
		}

		/// <summary>
		/// Reads the value of a field. The value is the latest written by any processor in a computer,
		/// regardless of the number of processors or the state of processor cache.</summary>
		/// <returns>The latest value written to the field by any processor.</returns>
		/// <param name="location">The field to be read.</param>
		[ResourceExposure(ResourceScope.None)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[CLSCompliant(false)]
		public static ushort Read(ref ushort location) {
			ushort value = location;
			Thread.MemoryBarrier();
			return value;
		}

		/// <summary>
		/// Reads the value of a field. The value is the latest written by any processor in a computer,
		/// regardless of the number of processors or the state of processor cache.</summary>
		/// <returns>The latest value written to the field by any processor.</returns>
		/// <param name="location">The field to be read.</param>
		[ResourceExposure(ResourceScope.None)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static int Read(ref int location) {
			int value = location;
			Thread.MemoryBarrier();
			return value;
		}

		/// <summary>
		/// Reads the value of a field. The value is the latest written by any processor in a computer,
		/// regardless of the number of processors or the state of processor cache.</summary>
		/// <returns>The latest value written to the field by any processor.</returns>
		/// <param name="location">The field to be read.</param>
		[ResourceExposure(ResourceScope.None)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[CLSCompliant(false)]
		public static uint Read(ref uint location) {
			uint value = location;
			Thread.MemoryBarrier();
			return value;
		}

		/// <summary>
		/// Reads the value of a field. The value is the latest written by any processor in a computer,
		/// regardless of the number of processors or the state of processor cache.</summary>
		/// <returns>The latest value written to the field by any processor.</returns>
		/// <param name="location">The field to be read. </param>
		[ResourceExposure(ResourceScope.None)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static long Read(ref long location) {
			return Interlocked.CompareExchange(ref location, 0, 0);
		}

		/// <summary>
		/// Reads the value of a field. The value is the latest written by any processor in a computer,
		/// regardless of the number of processors or the state of processor cache.</summary>
		/// <returns>The latest value written to the field by any processor.</returns>
		/// <param name="location">The field to be read.</param>
		[ResourceExposure(ResourceScope.None)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[CLSCompliant(false)]
		[SecuritySafeCritical] // contains unsafe code
		public static ulong Read(ref ulong location) {
			unsafe
			{
				fixed (ulong* pLocation = &location) {
					return (ulong) Interlocked.CompareExchange(ref *(long*) pLocation, 0, 0);
				}
			}
		}

		/// <summary>
		/// Reads the value of a field. The value is the latest written by any processor in a computer,
		/// regardless of the number of processors or the state of processor cache.</summary>
		/// <returns>The latest value written to the field by any processor.</returns>
		/// <param name="location">The field to be read.</param>
		[ResourceExposure(ResourceScope.None)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static IntPtr Read(ref IntPtr location) {
			IntPtr value = location;
			Thread.MemoryBarrier();
			return value;
		}

		/// <summary>
		/// Reads the value of a field. The value is the latest written by any processor in a computer,
		/// regardless of the number of processors or the state of processor cache.</summary>
		/// <returns>The latest value written to the field by any processor.</returns>
		/// <param name="location">The field to be read.</param>
		[ResourceExposure(ResourceScope.None)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[CLSCompliant(false)]
		public static UIntPtr Read(ref UIntPtr location) {
			UIntPtr value = location;
			Thread.MemoryBarrier();
			return value;
		}

		/// <summary>
		/// Reads the value of a field. The value is the latest written by any processor in a computer,
		/// regardless of the number of processors or the state of processor cache.</summary>
		/// <returns>The latest value written to the field by any processor.</returns>
		/// <param name="location">The field to be read.</param>
		[ResourceExposure(ResourceScope.None)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static float Read(ref float location) {
			float value = location;
			Thread.MemoryBarrier();
			return value;
		}

		/// <summary>
		/// Reads the value of a field. The value is the latest written by any processor in a computer,
		/// regardless of the number of processors or the state of processor cache.</summary>
		/// <returns>The latest value written to the field by any processor.</returns>
		/// <param name="location">The field to be read.</param>
		[ResourceExposure(ResourceScope.None)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static double Read(ref double location) {
			return Interlocked.CompareExchange(ref location, 0, 0);
		}

		/// <summary>
		/// Reads the value of a field. The value is the latest written by any processor in a computer,
		/// regardless of the number of processors or the state of processor cache.</summary>
		/// <returns>The latest value written to the field by any processor.</returns>
		/// <param name="location">The field to be read.</param>
		[ResourceExposure(ResourceScope.None)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[SecuritySafeCritical]
		public static T Read<T>(ref T location) where T : class {
			T value = location;
			Thread.MemoryBarrier();
			return value;
		}

		/// <summary>
		/// Writes a value to a field immediately, so that the value is visible to all processors in the computer.
		/// </summary>
		/// <param name="location">The field to which the value is to be written.</param>
		/// <param name="value">The value to be written.</param>
		[ResourceExposure(ResourceScope.None)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static void Write(ref bool location, bool value) {
			Thread.MemoryBarrier();
			location = value;
		}

		/// <summary>
		/// Writes a value to a field immediately, so that the value is visible to all processors in the computer.
		/// </summary>
		/// <param name="location">The field to which the value is to be written.</param>
		/// <param name="value">The value to be written.</param>
		[ResourceExposure(ResourceScope.None)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[CLSCompliant(false)]
		public static void Write(ref sbyte location, sbyte value) {
			Thread.MemoryBarrier();
			location = value;
		}

		/// <summary>
		/// Writes a value to a field immediately, so that the value is visible to all processors in the computer.
		/// </summary>
		/// <param name="location">The field to which the value is to be written.</param>
		/// <param name="value">The value to be written.</param>
		[ResourceExposure(ResourceScope.None)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static void Write(ref byte location, byte value) {
			Thread.MemoryBarrier();
			location = value;
		}

		/// <summary>
		/// Writes a value to a field immediately, so that the value is visible to all processors in the computer.
		/// </summary>
		/// <param name="location">The field to which the value is to be written.</param>
		/// <param name="value">The value to be written.</param>
		[ResourceExposure(ResourceScope.None)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static void Write(ref short location, short value) {
			Thread.MemoryBarrier();
			location = value;
		}

		/// <summary>
		/// Writes a value to a field immediately, so that the value is visible to all processors in the computer.
		/// </summary>
		/// <param name="location">The field to which the value is to be written.</param>
		/// <param name="value">The value to be written.</param>
		[ResourceExposure(ResourceScope.None)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[CLSCompliant(false)]
		public static void Write(ref ushort location, ushort value) {
			Thread.MemoryBarrier();
			location = value;
		}

		/// <summary>
		/// Writes a value to a field immediately, so that the value is visible to all processors in the computer.
		/// </summary>
		/// <param name="location">The field to which the value is to be written.</param>
		/// <param name="value">The value to be written.</param>
		[ResourceExposure(ResourceScope.None)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static void Write(ref int location, int value) {
			Thread.MemoryBarrier();
			location = value;
		}

		/// <summary>
		/// Writes a value to a field immediately, so that the value is visible to all processors in the computer.
		/// </summary>
		/// <param name="location">The field to which the value is to be written.</param>
		/// <param name="value">The value to be written.</param>
		[ResourceExposure(ResourceScope.None)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[CLSCompliant(false)]
		public static void Write(ref uint location, uint value) {
			Thread.MemoryBarrier();
			location = value;
		}

		/// <summary>
		/// Writes a value to a field immediately, so that the value is visible to all processors in the computer.
		/// </summary>
		/// <param name="location">The field to which the value is to be written.</param>
		/// <param name="value">The value to be written.</param>
		[ResourceExposure(ResourceScope.None)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static void Write(ref long location, long value) {
			Interlocked.Exchange(ref location, value);
		}

		/// <summary>
		/// Writes a value to a field immediately, so that the value is visible to all processors in the computer.
		/// </summary>
		/// <param name="location">The field to which the value is to be written.</param>
		/// <param name="value">The value to be written.</param>
		[ResourceExposure(ResourceScope.None)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[CLSCompliant(false)]
		[SecuritySafeCritical] // contains unsafe code
		public static void Write(ref ulong location, ulong value) {
			unsafe
			{
				fixed (ulong* pLocation = &location) {
					Interlocked.Exchange(ref *(long*) pLocation, (long) value);
				}
			}
		}

		/// <summary>
		/// Writes a value to a field immediately, so that the value is visible to all processors in the computer.
		/// </summary>
		/// <param name="location">The field to which the value is to be written.</param>
		/// <param name="value">The value to be written.</param>
		[ResourceExposure(ResourceScope.None)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static void Write(ref IntPtr location, IntPtr value) {
			Thread.MemoryBarrier();
			location = value;
		}

		/// <summary>
		/// Writes a value to a field immediately, so that the value is visible to all processors in the computer.
		/// </summary>
		/// <param name="location">The field to which the value is to be written.</param>
		/// <param name="value">The value to be written.</param>
		[ResourceExposure(ResourceScope.None)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[CLSCompliant(false)]
		public static void Write(ref UIntPtr location, UIntPtr value) {
			Thread.MemoryBarrier();
			location = value;
		}

		/// <summary>
		/// Writes a value to a field immediately, so that the value is visible to all processors in the computer.
		/// </summary>
		/// <param name="location">The field to which the value is to be written.</param>
		/// <param name="value">The value to be written.</param>
		[ResourceExposure(ResourceScope.None)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static void Write(ref float location, float value) {
			Thread.MemoryBarrier();
			location = value;
		}

		/// <summary>
		/// Writes a value to a field immediately, so that the value is visible to all processors in the computer.
		/// </summary>
		/// <param name="location">The field to which the value is to be written.</param>
		/// <param name="value">The value to be written.</param>
		[ResourceExposure(ResourceScope.None)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static void Write(ref double location, double value) {
			Interlocked.Exchange(ref location, value);
		}

		/// <summary>
		/// Writes a value to a field immediately, so that the value is visible to all processors in the computer.
		/// </summary>
		/// <param name="location">The field to which the value is to be written.</param>
		/// <param name="value">The value to be written.</param>
		[ResourceExposure(ResourceScope.None)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[SecuritySafeCritical] //the intrinsic implementation of this method contains unverifiable code
		public static void Write<T>(ref T location, T value) where T : class {
			Thread.MemoryBarrier();
			location = value;
		}
	}
}

#endif