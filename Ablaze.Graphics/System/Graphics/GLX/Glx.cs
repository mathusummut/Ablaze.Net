//Ported from OpenTK, and excellent library.

#pragma warning disable 1591

using System.Platforms.X11;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Graphics.GLX {
	/// <summary>
	/// Provides access to GLX functions.
	/// </summary>
	[Security.SuppressUnmanagedCodeSecurity]
	public static class Glx {
		/// <summary>
		/// The GLX library.
		/// </summary>
		public const string Library = "libGL.so.1";
		/// <summary>
		/// The native prefix of every glX function.
		/// </summary>
		public const string Prefix = "glX";

		/// <summary>
		/// Gets whether GLX is supported.
		/// </summary>
		public static bool IsSupported {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				try {
					GetCurrentContext();
				} catch {
					return false;
				}
				return true;
			}
		}

		[DllImport(Library, EntryPoint = "glXIsDirect")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern bool IsDirect(IntPtr dpy, IntPtr context);

		[DllImport(Library, EntryPoint = "glXQueryExtension")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern bool QueryExtension(IntPtr dpy, ref int errorBase, ref int eventBase);

		[DllImport(Library, EntryPoint = "glXQueryExtensionsString")]
		[Security.SuppressUnmanagedCodeSecurity]
		static extern IntPtr QueryExtensionsStringInternal(IntPtr dpy, int screen);

		public static string QueryExtensionsString(IntPtr dpy, int screen) {
			return Marshal.PtrToStringAnsi(QueryExtensionsStringInternal(dpy, screen));
		}

		[DllImport(Library, EntryPoint = "glXCreateContext")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr CreateContext(IntPtr dpy, IntPtr vis, IntPtr shareList, bool direct);

		[CLSCompliant(false)]
		[DllImport(Library, EntryPoint = "glXCreateContext")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr CreateContext(IntPtr dpy, ref XVisualInfo vis, IntPtr shareList, bool direct);

		[DllImport(Library, EntryPoint = "glXDestroyContext")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern void DestroyContext(IntPtr dpy, IntPtr context);

		[DllImport(Library, EntryPoint = "glXGetCurrentContext")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr GetCurrentContext();

		[DllImport(Library, EntryPoint = "glXMakeCurrent")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern bool MakeCurrent(IntPtr display, IntPtr drawable, IntPtr context);

		[DllImport(Library, EntryPoint = "glXSwapBuffers")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern void SwapBuffers(IntPtr display, IntPtr drawable);

		[DllImport(Library, EntryPoint = "glXGetProcAddress")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr GetProcAddress([MarshalAs(UnmanagedType.LPTStr)] string procName);

		[CLSCompliant(false)]
		[DllImport(Library, EntryPoint = "glXGetConfig")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern int GetConfig(IntPtr dpy, ref XVisualInfo vis, PixelFormatAttribute attrib, out int value);

		[DllImport(Library, EntryPoint = "glXChooseVisual")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static IntPtr ChooseVisual(IntPtr dpy, int screen, IntPtr attriblist);

		[DllImport(Library, EntryPoint = "glXChooseVisual")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static IntPtr ChooseVisual(IntPtr dpy, int screen, ref int attriblist);

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static IntPtr ChooseVisual(IntPtr dpy, int screen, int[] attriblist) {
			unsafe
			{
				fixed (int* attriblist_ptr = attriblist) {
					return ChooseVisual(dpy, screen, (IntPtr) attriblist_ptr);
				}
			}
		}

		// Returns an array of GLXFBConfig structures.
		[CLSCompliant(false)]
		[DllImport(Library, EntryPoint = "glXChooseFBConfig")]
		[Security.SuppressUnmanagedCodeSecurity]
		unsafe public extern static IntPtr* ChooseFBConfig(IntPtr dpy, int screen, int[] attriblist, out int fbount);

		// Returns a pointer to an XVisualInfo structure.
		[DllImport(Library, EntryPoint = "glXGetVisualFromFBConfig")]
		[Security.SuppressUnmanagedCodeSecurity]
		public unsafe extern static IntPtr GetVisualFromFBConfig(IntPtr dpy, IntPtr fbconfig);

		public static class Sgi {
			public static ErrorCode SwapInterval(int interval) {
				return (ErrorCode) Delegates.glXSwapIntervalSGI(interval);
			}
		}

		public static class Arb {
			[CLSCompliant(false)]
			unsafe public static IntPtr CreateContextAttribs(IntPtr display, IntPtr fbconfig, IntPtr share_context, bool direct, int* attribs) {
				return Delegates.glXCreateContextAttribsARB(display, fbconfig, share_context, direct, attribs);
			}

			public static IntPtr CreateContextAttribs(IntPtr display, IntPtr fbconfig, IntPtr share_context, bool direct, int[] attribs) {
				unsafe
				{
					fixed (int* attribs_ptr = attribs) {
						return Delegates.glXCreateContextAttribsARB(display, fbconfig, share_context, direct, attribs_ptr);
					}
				}
			}
		}

		/// <summary>
		/// Enlists extension methods.
		/// </summary>
		public static class Delegates {
			static Delegates() {
				Type coreClass = typeof(Glx);
				const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly;
				MethodInfo method;
				IntPtr address;
				IntPtr display = NativeApi.DefaultDisplay;
				NativeApi.XLockDisplay(display);
				foreach (FieldInfo f in typeof(Delegates).GetFields(flags)) {
					try {
						address = GetProcAddress(f.Name);
					} catch {
						address = IntPtr.Zero;
					}
					if (address.ToInt64() > 2L)
						f.SetValue(null, Marshal.GetDelegateForFunctionPointer(address, f.FieldType));
					else {
						method = coreClass.GetMethod(f.Name.Substring(3), flags);
						if (method != null)
							f.SetValue(null, Delegate.CreateDelegate(f.FieldType, method));
					}
				}
				NativeApi.XUnlockDisplay(display);
			}

			[SuppressUnmanagedCodeSecurity]
			public delegate int SwapIntervalSGI(int interval);
			public static readonly SwapIntervalSGI glXSwapIntervalSGI = null;

			[SuppressUnmanagedCodeSecurity]
			[CLSCompliant(false)]
			public unsafe delegate IntPtr CreateContextAttribsARB(IntPtr display, IntPtr fbconfig, IntPtr share_context, bool direct, int* attribs);
			[CLSCompliant(false)]
			public static readonly unsafe CreateContextAttribsARB glXCreateContextAttribsARB = null;
		}
	}
}

#pragma warning restore 1591