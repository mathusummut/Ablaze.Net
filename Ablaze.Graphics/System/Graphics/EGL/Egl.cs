//Ported from OpenTK, and excellent library.

#pragma warning disable 1591

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Graphics.EGL {
	/// <summary>
	/// EGL Library
	/// </summary>
	[Security.SuppressUnmanagedCodeSecurity]
	public static class Egl {
		/// <summary>
		/// The EGL library
		/// </summary>
		public const string Library = "libEGL.dll";

		[DllImportAttribute("libEGL.dll", EntryPoint = "eglGetError")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern int GetError();

		[DllImportAttribute("libEGL.dll", EntryPoint = "eglGetDisplay")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr GetDisplay(IntPtr display_id);

		[DllImportAttribute("libEGL.dll", EntryPoint = "eglInitialize")]
		[Security.SuppressUnmanagedCodeSecurity]
		//[return: MarshalAsAttribute(UnmanagedType.I1)]
		public static extern bool Initialize(IntPtr dpy, out int major, out int minor);

		[DllImportAttribute("libEGL.dll", EntryPoint = "eglTerminate")]
		[Security.SuppressUnmanagedCodeSecurity]
		//[return: MarshalAsAttribute(UnmanagedType.I1)]
		public static extern bool Terminate(IntPtr dpy);

		[DllImportAttribute("libEGL.dll", EntryPoint = "eglQueryString")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr QueryString(IntPtr dpy, int name);

		[DllImportAttribute("libEGL.dll", EntryPoint = "eglGetConfigs")]
		[Security.SuppressUnmanagedCodeSecurity]
		[return: MarshalAsAttribute(UnmanagedType.I1)]
		public static extern bool GetConfigs(IntPtr dpy, IntPtr[] configs, int config_size, out int num_config);

		[DllImportAttribute("libEGL.dll", EntryPoint = "eglChooseConfig")]
		[Security.SuppressUnmanagedCodeSecurity]
		[return: MarshalAsAttribute(UnmanagedType.I1)]
		public static extern bool ChooseConfig(IntPtr dpy, int[] attrib_list, [In, Out] IntPtr[] configs, int config_size, out int num_config);

		[DllImportAttribute("libEGL.dll", EntryPoint = "eglGetConfigAttrib")]
		[Security.SuppressUnmanagedCodeSecurity]
		[return: MarshalAsAttribute(UnmanagedType.I1)]
		public static extern bool GetConfigAttrib(IntPtr dpy, IntPtr config, int attribute, out int value);

		[DllImportAttribute("libEGL.dll", EntryPoint = "eglCreateWindowSurface")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr CreateWindowSurface(IntPtr dpy, IntPtr config, IntPtr win, int[] attrib_list);

		[DllImportAttribute("libEGL.dll", EntryPoint = "eglCreatePbufferSurface")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr CreatePbufferSurface(IntPtr dpy, IntPtr config, int[] attrib_list);

		[DllImportAttribute("libEGL.dll", EntryPoint = "eglCreatePixmapSurface")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr CreatePixmapSurface(IntPtr dpy, IntPtr config, IntPtr pixmap, int[] attrib_list);

		[DllImportAttribute("libEGL.dll", EntryPoint = "eglDestroySurface")]
		[Security.SuppressUnmanagedCodeSecurity]
		[return: MarshalAsAttribute(UnmanagedType.I1)]
		public static extern bool DestroySurface(IntPtr dpy, IntPtr surface);

		[DllImportAttribute("libEGL.dll", EntryPoint = "eglQuerySurface")]
		[Security.SuppressUnmanagedCodeSecurity]
		[return: MarshalAsAttribute(UnmanagedType.I1)]
		public static extern bool QuerySurface(IntPtr dpy, IntPtr surface, int attribute, out int value);

		[DllImportAttribute("libEGL.dll", EntryPoint = "eglBindAPI")]
		[Security.SuppressUnmanagedCodeSecurity]
		[return: MarshalAsAttribute(UnmanagedType.I1)]
		public static extern bool BindAPI(int api);

		[DllImportAttribute("libEGL.dll", EntryPoint = "eglQueryAPI")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern int QueryAPI();

		[DllImportAttribute("libEGL.dll", EntryPoint = "eglWaitClient")]
		[Security.SuppressUnmanagedCodeSecurity]
		[return: MarshalAsAttribute(UnmanagedType.I1)]
		public static extern bool WaitClient();

		[DllImportAttribute("libEGL.dll", EntryPoint = "eglReleaseThread")]
		[Security.SuppressUnmanagedCodeSecurity]
		[return: MarshalAsAttribute(UnmanagedType.I1)]
		public static extern bool ReleaseThread();

		[DllImportAttribute("libEGL.dll", EntryPoint = "eglCreatePbufferFromClientBuffer")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr CreatePbufferFromClientBuffer(IntPtr dpy, int buftype, IntPtr buffer, IntPtr config, int[] attrib_list);

		[DllImportAttribute("libEGL.dll", EntryPoint = "eglSurfaceAttrib")]
		[Security.SuppressUnmanagedCodeSecurity]
		[return: MarshalAsAttribute(UnmanagedType.I1)]
		public static extern bool SurfaceAttrib(IntPtr dpy, IntPtr surface, int attribute, int value);

		[DllImportAttribute("libEGL.dll", EntryPoint = "eglBindTexImage")]
		[Security.SuppressUnmanagedCodeSecurity]
		[return: MarshalAsAttribute(UnmanagedType.I1)]
		public static extern bool BindTexImage(IntPtr dpy, IntPtr surface, int buffer);

		[DllImportAttribute("libEGL.dll", EntryPoint = "eglReleaseTexImage")]
		[Security.SuppressUnmanagedCodeSecurity]
		[return: MarshalAsAttribute(UnmanagedType.I1)]
		public static extern bool ReleaseTexImage(IntPtr dpy, IntPtr surface, int buffer);

		[DllImportAttribute("libEGL.dll", EntryPoint = "eglSwapInterval")]
		[Security.SuppressUnmanagedCodeSecurity]
		[return: MarshalAsAttribute(UnmanagedType.I1)]
		public static extern bool SwapInterval(IntPtr dpy, int interval);

		[DllImportAttribute("libEGL.dll", EntryPoint = "eglCreateContext")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr CreateContext(IntPtr dpy, IntPtr config, IntPtr share_context, int[] attrib_list);

		[DllImportAttribute("libEGL.dll", EntryPoint = "eglDestroyContext")]
		[Security.SuppressUnmanagedCodeSecurity]
		[return: MarshalAsAttribute(UnmanagedType.I1)]
		public static extern bool DestroyContext(IntPtr dpy, IntPtr ctx);

		[DllImportAttribute("libEGL.dll", EntryPoint = "eglMakeCurrent")]
		[Security.SuppressUnmanagedCodeSecurity]
		[return: MarshalAsAttribute(UnmanagedType.I1)]
		public static extern bool MakeCurrent(IntPtr dpy, IntPtr draw, IntPtr read, IntPtr ctx);

		[DllImportAttribute("libEGL.dll", EntryPoint = "eglGetCurrentContext")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr GetCurrentContext();

		[DllImportAttribute("libEGL.dll", EntryPoint = "eglGetCurrentSurface")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr GetCurrentSurface(int readdraw);

		[DllImportAttribute("libEGL.dll", EntryPoint = "eglGetCurrentDisplay")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr GetCurrentDisplay();

		[DllImportAttribute("libEGL.dll", EntryPoint = "eglQueryContext")]
		[Security.SuppressUnmanagedCodeSecurity]
		[return: MarshalAsAttribute(UnmanagedType.I1)]
		public static extern bool QueryContext(IntPtr dpy, IntPtr ctx, int attribute, out int value);

		[DllImportAttribute("libEGL.dll", EntryPoint = "eglWaitGL")]
		[Security.SuppressUnmanagedCodeSecurity]
		[return: MarshalAsAttribute(UnmanagedType.I1)]
		public static extern bool WaitGL();

		[DllImportAttribute("libEGL.dll", EntryPoint = "eglWaitNative")]
		[Security.SuppressUnmanagedCodeSecurity]
		[return: MarshalAsAttribute(UnmanagedType.I1)]
		public static extern bool WaitNative(int engine);

		[DllImportAttribute("libEGL.dll", EntryPoint = "eglSwapBuffers")]
		[Security.SuppressUnmanagedCodeSecurity]
		[return: MarshalAsAttribute(UnmanagedType.I1)]
		public static extern bool SwapBuffers(IntPtr dpy, IntPtr surface);

		[DllImportAttribute("libEGL.dll", EntryPoint = "eglCopyBuffers")]
		[Security.SuppressUnmanagedCodeSecurity]
		[return: MarshalAsAttribute(UnmanagedType.I1)]
		public static extern bool CopyBuffers(IntPtr dpy, IntPtr surface, IntPtr target);

		[DllImportAttribute("libEGL.dll", EntryPoint = "eglGetProcAddress")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr GetProcAddress(string funcname);

		/// <summary>
		/// Gets whether EGL is supported.
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
	}
}

#pragma warning restore 1591