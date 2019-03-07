using System.Platforms.Unix;
using System.Runtime.CompilerServices;

namespace System {
	/// <summary>
	/// The type of underlying platform.
	/// </summary>
	[Flags]
	public enum PlatformType {
		/// <summary>
		/// The current platform is not supported.
		/// </summary>
		None,
		/// <summary>
		/// The current VM runtime is the .Net Framework.
		/// </summary>
		Net,
		/// <summary>
		/// The current VM runtime is the Mono Framework.
		/// </summary>
		Mono,
		/// <summary>
		/// The current underlying platform is Windows.
		/// </summary>
		Windows,
		/// <summary>
		/// The current underlying platform is Unix (ie. Linux or Mac OSX).
		/// </summary>
		Unix,
		/// <summary>
		/// The current underlying platform is Linux.
		/// </summary>
		Linux,
		/// <summary>
		/// The current underlying platform is Mac OSX.
		/// </summary>
		MacOSX,
		/// <summary>
		/// The current underlying platform is X11.
		/// </summary>
		X11,
		/// <summary>
		/// The current underlying platform is Xbox.
		/// </summary>
		Xbox,
		/// <summary>
		/// The current underlying platform is Android.
		/// </summary>
		Android,
		/// <summary>
		/// The current underlying platform is IOS.
		/// </summary>
		IOS
	}

	/// <summary>Provides information about the underlying OS and runtime.</summary>
	public static class Platform {
		/// <summary>
		/// The type of underlying platform.
		/// </summary>
		public static readonly PlatformType CurrentPlatform;

		/// <summary>
		/// Gets whether the OS is Windows and the version is XP or newer.
		/// </summary>
		public static bool IsWindowsXPOrNewer {
			get {
				OperatingSystem os = Environment.OSVersion;
				return os.Platform == PlatformID.Win32NT && (os.Version.Major > 5 || (os.Version.Major == 5 && os.Version.Minor >= 1));
			}
		}

		/// <summary>
		/// Gets whether the OS is Windows and the version is Vista or newer.
		/// </summary>
		public static bool IsWindowsVistaOrNewer {
			get {
				OperatingSystem os = Environment.OSVersion;
				return os.Platform == PlatformID.Win32NT && os.Version.Major >= 6;
			}
		}

		/// <summary>
		/// Gets whether the OS is Windows and the version is Windows 7 or newer.
		/// </summary>
		public static bool IsWindows7OrNewer {
			get {
				OperatingSystem os = Environment.OSVersion;
				return os.Platform == PlatformID.Win32NT && os.Version.Major >= 6 && os.Version.Minor >= 1;
			}
		}

		/// <summary>
		/// Gets whether the OS is Windows and the version is Windows 8 or newer.
		/// </summary>
		public static bool IsWindows8OrNewer {
			get {
				OperatingSystem os = Environment.OSVersion;
				return os.Platform == PlatformID.Win32NT && (os.Version.Major > 6 || (os.Version.Major == 6 && os.Version.Minor >= 2));
			}
		}

		/// <summary>
		/// Determines whether the current process is a 64-bit process.
		/// </summary>
		public static bool Is64BitProcess {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return IntPtr.Size == 8;
			}
		}

		static Platform() {
			if (Type.GetType("Mono.Runtime") == null)
				CurrentPlatform |= PlatformType.Net;
			else
				CurrentPlatform |= PlatformType.Mono;
#if ANDROID
	Type = PlatformType.Android;
#elif IPHONE
	Type = PlatformType.IOS;
#else
			switch (Environment.OSVersion.Platform) {
				case PlatformID.MacOSX:
					CurrentPlatform |= PlatformType.MacOSX | PlatformType.Unix;
					break;
				case PlatformID.Unix:
					try {
						switch (NativeApi.GetUnixKernelName()) {
							case "Linux":
								CurrentPlatform |= PlatformType.Linux;
								break;
							case "Darwin":
								CurrentPlatform |= PlatformType.MacOSX;
								break;
						}
						CurrentPlatform |= PlatformType.Unix;
					} catch {
					}
					break;
				case PlatformID.Xbox:
					CurrentPlatform |= PlatformType.Xbox;
					break;
				default:
					CurrentPlatform |= PlatformType.Windows;
					break;
			}
#endif
			if (IsOnSpecifiedPlatform(PlatformType.Linux)) {
				try {
					if (Platforms.X11.NativeApi.DefaultDisplay != IntPtr.Zero)
						CurrentPlatform |= PlatformType.X11;
				} catch {
				}
			}
		}

		/// <summary>
		/// Gets whether the specified platform type is the current one.
		/// </summary>
		/// <param name="type">The current underlying platform.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool IsOnSpecifiedPlatform(PlatformType type) {
			return (CurrentPlatform & type) == type;
		}
	}
}