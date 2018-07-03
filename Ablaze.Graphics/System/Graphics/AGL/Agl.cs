//Ported from OpenTK, and excellent library.

#pragma warning disable 1591

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Graphics.AGL {
	/// <summary>
	/// Handles AGL graphics.
	/// </summary>
	[Security.SuppressUnmanagedCodeSecurity]
	public static class Agl {
		/// <summary>
		/// The AGL library
		/// </summary>
		public const string Library = "/System/Library/Frameworks/AGL.framework/Versions/Current/AGL";

		/// <summary>
		/// Gets whether AGL is supported.
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

		[DllImport(Library, EntryPoint = "aglChoosePixelFormat")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr ChoosePixelFormat(ref IntPtr gdevs, int ndev, int[] attribs);

		[DllImport(Library, EntryPoint = "aglDestroyPixelFormat")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern void DestroyPixelFormat(IntPtr pix);

		[DllImport(Library, EntryPoint = "aglNextPixelFormat")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr NextPixelFormat(IntPtr pix);

		[DllImport(Library, EntryPoint = "aglDescribePixelFormat")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern bool DescribePixelFormat(IntPtr pix, PixelFormatAttribute attrib, out int value);

		[DllImport(Library, EntryPoint = "aglQueryRendererInfo")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr QueryRendererInfo(IntPtr[] gdevs, int ndev);

		[DllImport(Library, EntryPoint = "aglDestroyRendererInfo")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern void DestroyRendererInfo(IntPtr rend);

		[DllImport(Library, EntryPoint = "aglNextRendererInfo")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr NextRendererInfo(IntPtr rend);

		[DllImport(Library, EntryPoint = "aglDescribeRenderer")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern byte DescribeRenderer(IntPtr rend, int prop, out int value);

		[DllImport(Library, EntryPoint = "aglCreateContext")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr CreateContext(IntPtr pix, IntPtr share);

		[DllImport(Library)]
		[Security.SuppressUnmanagedCodeSecurity]
		static extern byte aglDestroyContext(IntPtr ctx);

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool DestroyContext(IntPtr context) {
			return aglDestroyContext(context) != 0;
		}

		[CLSCompliant(false)]
		[DllImport(Library, EntryPoint = "aglCopyContext")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern byte CopyContext(IntPtr src, IntPtr dst, uint mask);

		[DllImport(Library, EntryPoint = "aglUpdateContext")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern byte UpdateContext(IntPtr ctx);

		[DllImport(Library)]
		[Security.SuppressUnmanagedCodeSecurity]
		static extern byte aglSetCurrentContext(IntPtr ctx);

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool SetCurrentContext(IntPtr context) {
			return aglSetCurrentContext(context) != 0;
		}

		[DllImport(Library, EntryPoint = "aglGetCurrentContext")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr GetCurrentContext();

		[DllImport(Library, EntryPoint = "aglSetDrawable")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern byte SetDrawable(IntPtr ctx, IntPtr draw);

		[DllImport(Library, EntryPoint = "aglSetOffScreen")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern byte SetOffScreen(IntPtr ctx, int width, int height, int rowbytes, IntPtr baseaddr);

		[DllImport(Library, EntryPoint = "aglGetDrawable")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr GetDrawable(IntPtr ctx);

		[DllImport(Library, EntryPoint = "aglSetFullScreen")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern byte SetFullScreen(IntPtr ctx, int width, int height, int freq, int device);

		[DllImport(Library, EntryPoint = "aglSetVirtualScreen")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern byte SetVirtualScreen(IntPtr ctx, int screen);

		[DllImport(Library, EntryPoint = "aglGetVirtualScreen")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern int GetVirtualScreen(IntPtr ctx);

		[CLSCompliant(false)]
		[DllImport(Library, EntryPoint = "aglGetVersion")]
		[Security.SuppressUnmanagedCodeSecurity]
		public unsafe static extern void GetVersion(int* major, int* minor);

		[CLSCompliant(false)]
		[DllImport(Library, EntryPoint = "aglConfigure")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern byte Configure(UInt32 pname, uint param);

		[DllImport(Library, EntryPoint = "aglSwapBuffers")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern void SwapBuffers(IntPtr ctx);

		[DllImport(Library, EntryPoint = "aglEnable")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern bool Enable(IntPtr ctx, ParameterNames pname);

		[DllImport(Library, EntryPoint = "aglDisable")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern bool Disable(IntPtr ctx, ParameterNames pname);

		[CLSCompliant(false)]
		[DllImport(Library, EntryPoint = "aglIsEnabled")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern bool IsEnabled(IntPtr ctx, UInt32 pname);

		[DllImport(Library, EntryPoint = "aglSetInteger")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern bool SetInteger(IntPtr ctx, ParameterNames pname, ref int param);

		[DllImport(Library, EntryPoint = "aglSetInteger")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern bool SetInteger(IntPtr ctx, ParameterNames pname, int[] param);

		[CLSCompliant(false)]
		[DllImport(Library, EntryPoint = "aglGetInteger")]
		[Security.SuppressUnmanagedCodeSecurity]
		public unsafe static extern bool GetInteger(IntPtr ctx, ParameterNames pname, int* @params);

		[DllImport(Library, EntryPoint = "aglGetInteger")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern bool GetInteger(IntPtr ctx, ParameterNames pname, out int param);

		[DllImport(Library, EntryPoint = "aglUseFont")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern byte UseFont(IntPtr ctx, int fontID, int face, int size, int first, int count, int @base);

		[DllImport(Library, EntryPoint = "aglGetError")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern AglError GetError();

		[Security.SuppressUnmanagedCodeSecurity]
		[DllImport(Library)]
		public static extern IntPtr aglErrorString(AglError code);

		[Security.SuppressUnmanagedCodeSecurity]
		public static string ErrorString(AglError code) {
			return Marshal.PtrToStringAnsi(aglErrorString(code));
		}

		[DllImport(Library, EntryPoint = "aglResetLibrary")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern void ResetLibrary();

		[CLSCompliant(false)]
		[DllImport(Library, EntryPoint = "aglSurfaceTexture")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern void SurfaceTexture(IntPtr context, UInt32 target, UInt32 internalformat, IntPtr surfacecontext);

		[CLSCompliant(false)]
		[DllImport(Library, EntryPoint = "aglCreatePBuffer")]
		[Security.SuppressUnmanagedCodeSecurity]
		public unsafe static extern byte CreatePBuffer(int width, int height, UInt32 target, UInt32 internalFormat, long max_level, IntPtr* pbuffer);

		[DllImport(Library, EntryPoint = "aglDestroyPBuffer")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern byte DestroyPBuffer(IntPtr pbuffer);

		[CLSCompliant(false)]
		[DllImport(Library, EntryPoint = "aglDescribePBuffer")]
		[Security.SuppressUnmanagedCodeSecurity]
		public unsafe static extern byte DescribePBuffer(IntPtr pbuffer, int* width, int* height, UInt32* target, UInt32* internalFormat, int* max_level);

		[DllImport(Library, EntryPoint = "aglTexImagePBuffer")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern byte TexImagePBuffer(IntPtr ctx, IntPtr pbuffer, int source);

		[DllImport(Library, EntryPoint = "aglSetPBuffer")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern byte SetPBuffer(IntPtr ctx, IntPtr pbuffer, int face, int level, int screen);

		[CLSCompliant(false)]
		[DllImport(Library, EntryPoint = "aglGetPBuffer")]
		[Security.SuppressUnmanagedCodeSecurity]
		public unsafe static extern byte GetPBuffer(IntPtr ctx, IntPtr* pbuffer, int* face, int* level, int* screen);

		[CLSCompliant(false)]
		[DllImport(Library, EntryPoint = "aglGetCGLContext")]
		[Security.SuppressUnmanagedCodeSecurity]
		public unsafe static extern byte GetCGLContext(IntPtr ctx, void** cgl_ctx);

		[CLSCompliant(false)]
		[DllImport(Library, EntryPoint = "aglGetCGLPixelFormat")]
		[Security.SuppressUnmanagedCodeSecurity]
		public unsafe static extern byte GetCGLPixelFormat(IntPtr pix, void** cgl_pix);

		[DllImport("libdl.dylib", EntryPoint = "NSIsSymbolNameDefined")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern bool IsSymbolNameDefined(string s);

		[DllImport("libdl.dylib", EntryPoint = "NSLookupAndBindSymbol")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr LookUpAndBindSymbol(string s);

		[DllImport("libdl.dylib", EntryPoint = "NSAddressOfSymbol")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr AddressOfSymbol(IntPtr symbol);

		/// <summary>
		/// Gets the address of the specified function.
		/// </summary>
		/// <param name="function">The function to look for.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static IntPtr GetAddress(string function) {
			string fname = "_" + function;
			if (!IsSymbolNameDefined(fname))
				return IntPtr.Zero;
			IntPtr symbol = LookUpAndBindSymbol(fname);
			if (symbol != IntPtr.Zero)
				symbol = AddressOfSymbol(symbol);
			return symbol;
		}
	}
}

#pragma warning restore 1591