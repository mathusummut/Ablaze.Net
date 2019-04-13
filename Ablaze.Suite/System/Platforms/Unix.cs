using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Platforms.Unix {
	/// <summary>
	/// Unix function library.
	/// </summary>
	[SuppressUnmanagedCodeSecurity]
	public static class NativeApi {
		/// <summary>
		/// Struct for uname
		/// </summary>
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
		public struct utsname {
			/// <summary>
			/// Name of this implementation of the operating system
			/// </summary>
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string sysname;
			/// <summary>
			/// Name of this node within an implementation-dependent communications network
			/// </summary>
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string nodename;
			/// <summary>
			/// Current release level of this implementation
			/// </summary>
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string release;
			/// <summary>
			/// Current version level of this release
			/// </summary>
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string version;
			/// <summary>
			/// Name of the hardware type on which the system is running
			/// </summary>
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string machine;
			/// <summary>
			/// Padding
			/// </summary>
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
			public string extraJustInCase;
		}

		/// <summary>
		/// Detects the unix kernel by p/invoking uname (libc).
		/// </summary>
		public static string GetUnixKernelName() {
			utsname uts;
			uname(out uts);
			return uts.sysname;
		}

		/// <summary>
		/// Gets name and information about current kernel.
		/// </summary>
		/// <param name="uname_struct">System information</param>
		[DllImport("libc")]
		public static extern void uname(out utsname uname_struct);
	}
}