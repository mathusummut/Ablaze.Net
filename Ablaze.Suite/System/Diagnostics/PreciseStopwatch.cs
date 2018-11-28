using System.Platforms.Windows;
using System.Runtime.CompilerServices;

namespace System.Diagnostics {
	/// <summary>
	/// A flexible stopwatch for measuring elapsed time very precisely
	/// </summary>
	public sealed class PreciseStopwatch : IEquatable<PreciseStopwatch> {
		private bool isRunning;
		private long startTimeStamp;
		private double elapsedTicks, speedMultiplier = 1.0;
		/// <summary>
		/// Gets whether high resolution timing is supported on the current platform
		/// </summary>
		public static readonly bool IsHighResolutionTimingSupported;
		/// <summary>
		/// Gets the amount of nanoseconds every platform-specific tick represents
		/// </summary>
		public static readonly double NanosecondsPerTick;

		/// <summary>
		/// Gets or sets the stopwatch speed multiplier (1 means time speed is normal)
		/// </summary>
		public double SpeedMultiplier {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return speedMultiplier;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				if (value == speedMultiplier)
					return;
				speedMultiplier = value;
				if (isRunning) {
					elapsedTicks += (TimeStamp - startTimeStamp) * speedMultiplier;
					startTimeStamp = TimeStamp;
				}
			}
		}

		/// <summary>
		/// Gets or sets whether the stopwatch is started
		/// </summary>
		public bool Running {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return isRunning;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				if (value == isRunning)
					return;
				isRunning = value;
				if (value)
					startTimeStamp = TimeStamp;
				else
					elapsedTicks += (TimeStamp - startTimeStamp) * speedMultiplier;
			}
		}

		/// <summary>
		/// Gets the elapsed time
		/// </summary>
		public TimeSpan Elapsed {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return new TimeSpan((long) Elapsed100NanosecondTicks);
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				Elapsed100NanosecondTicks = value.Ticks;
			}
		}

		/// <summary>
		/// Gets the elapsed time in platform-specific ticks
		/// </summary>
		public double ElapsedTicks {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return isRunning ? elapsedTicks + (TimeStamp - startTimeStamp) * speedMultiplier : elapsedTicks;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				elapsedTicks = value;
				startTimeStamp = TimeStamp;
			}
		}

		/// <summary>
		/// Gets the elapsed time in fortnights
		/// </summary>
		public double ElapsedFortnights {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return (NanosecondsPerTick * 0.00000000000000082671957671957672) * ElapsedTicks;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				ElapsedTicks = value * 1209600000000000.0 / NanosecondsPerTick;
			}
		}

		/// <summary>
		/// Gets the elapsed time in weeks
		/// </summary>
		public double ElapsedWeeks {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return (NanosecondsPerTick * 0.00000000000000165343915343915344) * ElapsedTicks;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				ElapsedTicks = value * 604800000000000.0 / NanosecondsPerTick;
			}
		}

		/// <summary>
		/// Gets the elapsed time in days
		/// </summary>
		public double ElapsedDays {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return (NanosecondsPerTick * 0.00000000000001157407407407407407) * ElapsedTicks;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				ElapsedTicks = value * 86400000000000.0 / NanosecondsPerTick;
			}
		}

		/// <summary>
		/// Gets the elapsed time in hours
		/// </summary>
		public double ElapsedHours {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return (NanosecondsPerTick * 0.00000000000027777777777777777777) * ElapsedTicks;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				ElapsedTicks = value * 3600000000000.0 / NanosecondsPerTick;
			}
		}

		/// <summary>
		/// Gets the elapsed time in minutes
		/// </summary>
		public double ElapsedMinutes {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return (NanosecondsPerTick * 0.00000000001666666666666666666666) * ElapsedTicks;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				ElapsedTicks = value * 60000000000.0 / NanosecondsPerTick;
			}
		}

		/// <summary>
		/// Gets the elapsed time in seconds
		/// </summary>
		public double ElapsedSeconds {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return (NanosecondsPerTick * 0.000000001) * ElapsedTicks;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				ElapsedTicks = value * 1000000000.0 / NanosecondsPerTick;
			}
		}

		/// <summary>
		/// Gets the elapsed time in milliseconds
		/// </summary>
		public double ElapsedMilliseconds {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return (NanosecondsPerTick * 0.000001) * ElapsedTicks;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				ElapsedTicks = value * 1000000.0 / NanosecondsPerTick;
			}
		}

		/// <summary>
		/// Gets the elapsed time in microseconds
		/// </summary>
		public double ElapsedMicroseconds {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return (NanosecondsPerTick * 0.001) * ElapsedTicks;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				ElapsedTicks = value * 1000.0 / NanosecondsPerTick;
			}
		}

		/// <summary>
		/// Gets the elapsed time in standard ticks (not platform-specific)
		/// </summary>
		public double Elapsed100NanosecondTicks {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return ElapsedTicks * (NanosecondsPerTick * 0.01);
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				ElapsedTicks = value * 100.0 / NanosecondsPerTick;
			}
		}

		/// <summary>
		/// Gets the elapsed time in nanoseconds
		/// </summary>
		public double ElapsedNanoseconds {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return NanosecondsPerTick * ElapsedTicks;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				ElapsedTicks = value / NanosecondsPerTick;
			}
		}

		/// <summary>
		/// Gets the current time in platform-specific ticks
		/// </summary>
		public static long TimeStamp {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				if (IsHighResolutionTimingSupported) {
					long num;
					NativeApi.QueryPerformanceCounter(out num);
					return num;
				} else
					return DateTime.UtcNow.Ticks;
			}
		}

		static PreciseStopwatch() {
			long frequency;
			IsHighResolutionTimingSupported = NativeApi.QueryPerformanceFrequency(out frequency);
			if (IsHighResolutionTimingSupported)
				NanosecondsPerTick = 1000000000.0 / frequency;
			else
				NanosecondsPerTick = 100.0;
		}

		/// <summary>
		/// Initializes a new high-resolution stopwatch
		/// </summary>
		public PreciseStopwatch() {
		}

		/// <summary>
		/// Initializes a new high-resolution stopwatch with pre-elapsed ticks
		/// </summary>
		/// <param name="preElapsedTicks">The starting elapsed ticks</param>
		public PreciseStopwatch(double preElapsedTicks) {
			elapsedTicks = preElapsedTicks;
		}

		/// <summary>
		/// Initializes a new high-resolution stopwatch with pre-elapsed ticks
		/// </summary>
		/// <param name="stopwatch">The stopwatch to clone</param>
		public PreciseStopwatch(PreciseStopwatch stopwatch) {
			elapsedTicks = stopwatch.ElapsedTicks;
		}

		/// <summary>
		/// Initializes a new high-resolution stopwatch with pre-elapsed ticks
		/// </summary>
		/// <param name="stopwatch">The stopwatch to clone</param>
		public PreciseStopwatch(Stopwatch stopwatch) {
			elapsedTicks = stopwatch.ElapsedTicks;
		}

		/// <summary>
		/// Resets and starts stopwatch
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Restart() {
			ElapsedTicks = 0.0;
			isRunning = true;
		}

		/// <summary>
		/// Resets and starts stopwatch and returns the elapsed ticks before restarting
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public double RestartGet() {
			long timeStamp = TimeStamp;
			double oldElapsed = isRunning ? elapsedTicks + (timeStamp - startTimeStamp) * speedMultiplier : elapsedTicks;
			elapsedTicks = 0.0;
			startTimeStamp = timeStamp;
			isRunning = true;
			return oldElapsed;
		}

		/// <summary>
		/// Resets and stops stopwatch and returns the elapsed ticks before resetting
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public double ResetStopGet() {
			long timeStamp = TimeStamp;
			double oldElapsed = isRunning ? elapsedTicks + (timeStamp - startTimeStamp) * speedMultiplier : elapsedTicks;
			elapsedTicks = 0.0;
			isRunning = false;
			return oldElapsed;
		}

		/// <summary>
		/// Resets and stops the stopwatch
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void ResetStop() {
			ElapsedTicks = 0.0;
			isRunning = false;
		}

		/// <summary>
		/// Resets the stopwatch without affecting whether it's running and returns the elapsed ticks before resetting
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public double ResetGet() {
			long timeStamp = TimeStamp;
			double oldElapsed = isRunning ? elapsedTicks + (timeStamp - startTimeStamp) * speedMultiplier : elapsedTicks;
			elapsedTicks = 0.0;
			startTimeStamp = timeStamp;
			return oldElapsed;
		}

		/// <summary>
		/// Resets the stopwatch without affecting whether it's running (same as setting ElapsedTicks to 0)
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Reset() {
			ElapsedTicks = 0.0;
		}

		/// <summary>
		/// Starts the stopwatch and returns the elapsed ticks before starting
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public double Start() {
			if (isRunning)
				return elapsedTicks + (TimeStamp - startTimeStamp) * speedMultiplier;
			else {
				startTimeStamp = TimeStamp;
				isRunning = true;
				return elapsedTicks;
			}
		}

		/// <summary>
		/// Stops the stopwatch and returns the elapsed ticks upon stopping
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public double Stop() {
			Running = false;
			return elapsedTicks;
		}

		/// <summary>
		/// Initializes a new stopwatch and starts it at one go
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static PreciseStopwatch StartNew() {
			PreciseStopwatch stopwatch = new PreciseStopwatch() {
				Running = true
			};
			return stopwatch;
		}

		/// <summary>
		/// Converts nanoseconds to platform-specific ticks
		/// </summary>
		/// <param name="nanoseconds">The value of nanoseconds to convert to platform-specific ticks</param>
		/// <returns>The equivalent of the nanoseconds in platform-specific ticks</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static double ConvertToTicks(double nanoseconds) {
			return nanoseconds / NanosecondsPerTick;
		}

		/// <summary>
		/// Converts platform-specific ticks to nanoseconds
		/// </summary>
		/// <param name="ticks">The value of platform-specific ticks to convert to nanoseconds</param>
		/// <returns>The equivalent of the platform-specific ticks in nanoseconds</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static double ConvertToNanoseconds(double ticks) {
			return ticks * NanosecondsPerTick;
		}

		/// <summary>
		/// Converts platform-specific ticks to microseconds
		/// </summary>
		/// <param name="ticks">The value of platform-specific ticks to convert to microseconds</param>
		/// <returns>The equivalent of the platform-specific ticks in microseconds</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static double ConvertToMicroseconds(double ticks) {
			return ticks * (NanosecondsPerTick * 0.001);
		}

		/// <summary>
		/// Converts platform-specific ticks to milliseconds
		/// </summary>
		/// <param name="ticks">The value of platform-specific ticks to convert to milliseconds</param>
		/// <returns>The equivalent of the platform-specific ticks in milliseconds</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static double ConvertToMilliseconds(double ticks) {
			return ticks * (NanosecondsPerTick * 0.000001);
		}

		/// <summary>
		/// Converts platform-specific ticks to seconds
		/// </summary>
		/// <param name="ticks">The value of platform-specific ticks to convert to seconds</param>
		/// <returns>The equivalent of the platform-specific ticks in seconds</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static double ConvertToSeconds(double ticks) {
			return ticks * (NanosecondsPerTick * 0.000000001);
		}

		/// <summary>
		/// Calculates the hash code for this PreciseStopwatch
		/// </summary>
		/// <returns>A System.Int32 containing the hashcode of this PreciseStopwatch</returns>
		public override int GetHashCode() {
			return unchecked(speedMultiplier.GetHashCode() << 17 ^ elapsedTicks.GetHashCode() << 13 ^ startTimeStamp.GetHashCode() << 7 ^ isRunning.GetHashCode());
		}

		/// <summary>
		/// Creates a System.String that describes this PreciseStopwatch
		/// </summary>
		/// <returns>A System.String that describes this PreciseStopwatch</returns>
		public override string ToString() {
			return "{ " + ElapsedMilliseconds + "ms elapsed }";
		}

		/// <summary>
		/// Compares whether this PreciseStopwatch is equal to the specified object
		/// </summary>
		/// <param name="obj">An object to compare to</param>
		public override bool Equals(object obj) {
			return Equals(obj as PreciseStopwatch);
		}

		/// <summary>
		/// Compares whether this PreciseStopwatch is equal to the specified object
		/// </summary>
		/// <param name="other">The object to compare to</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool Equals(PreciseStopwatch other) {
			return other == null ? false : (isRunning == other.isRunning && speedMultiplier == other.speedMultiplier && ElapsedTicks == other.ElapsedTicks);
		}
	}
}