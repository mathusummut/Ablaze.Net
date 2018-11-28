#pragma warning disable 0649
#pragma warning disable 1591

using System.Platforms.Windows;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace System.Graphics.WGL {
	partial class Wgl {
		/// <summary>
		/// Enlists all available delegates (null if not available).
		/// </summary>
		[System.Security.SuppressUnmanagedCodeSecurity()]
		public static partial class Delegates {
			static Delegates() {
				wglCreateContext = Core.CreateContext;
				wglMakeCurrent = Core.MakeCurrent;
			}

			/// <summary>
			/// Windows is fucking annoying sometimes. You need a GL context to be able to load a GL context. Fucking hell.
			/// </summary>
			internal static void ReloadDelegates() {
				using (DrawableForm ctrl = new DrawableForm()) {
					using (WinWindow window = new WinWindow(ctrl.Handle)) {
						using (WinGraphicsContext context = new WinGraphicsContext(new GraphicsMode(new IntPtr(1), GraphicsMode.Default.ColorFormat,
							GraphicsMode.Default.Depth, GraphicsMode.Default.Stencil, GraphicsMode.Default.Samples, GraphicsMode.Default.AccumFormat,
							GraphicsMode.Default.DoubleBuffering, GraphicsMode.Default.Stereo), window, new MajorMinorVersion(1))) {
							context.MakeCurrent(window);
							ReloadEntryPoints();
							OGL.GL.Delegates.ReloadEntryPoints();
							context.MakeCurrent(null as WinWindow);
						}
					}
				}
			}

			internal static void ReloadEntryPoints() {
				Type coreClass = typeof(Core);
				const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly;
				MethodInfo method;
				IntPtr address;
				long addr;
				foreach (FieldInfo f in typeof(Delegates).GetFields(flags)) {
					try {
						address = Core.GetProcAddress(f.Name);
					} catch {
						address = IntPtr.Zero;
					}
					addr = address.ToInt64();
					if (addr > 3L || addr < -1L)
						f.SetValue(null, Marshal.GetDelegateForFunctionPointer(address, f.FieldType));
					else {
						method = coreClass.GetMethod(f.Name.Substring(3), flags);
						if (method != null)
							f.SetValue(null, Delegate.CreateDelegate(f.FieldType, method));
					}
				}
			}

			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate IntPtr CreateContext(IntPtr hDc);
			[CLSCompliant(false)]
			public static CreateContext wglCreateContext;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Boolean DeleteContext(IntPtr oldContext);
			[CLSCompliant(false)]
			public static DeleteContext wglDeleteContext;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate IntPtr GetCurrentContext();
			[CLSCompliant(false)]
			public static GetCurrentContext wglGetCurrentContext;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Boolean MakeCurrent(IntPtr hDc, IntPtr newContext);
			[CLSCompliant(false)]
			public static MakeCurrent wglMakeCurrent;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Boolean CopyContext(IntPtr hglrcSrc, IntPtr hglrcDst, UInt32 mask);
			[CLSCompliant(false)]
			public static CopyContext wglCopyContext;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate int ChoosePixelFormat(IntPtr hDc, PixelFormatDescriptor* pPfd);
			[CLSCompliant(false)]
			public unsafe static ChoosePixelFormat wglChoosePixelFormat;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate int DescribePixelFormat(IntPtr hdc, int ipfd, UInt32 cjpfd, PixelFormatDescriptor* ppfd);
			[CLSCompliant(false)]
			public unsafe static DescribePixelFormat wglDescribePixelFormat;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate IntPtr GetCurrentDC();
			[CLSCompliant(false)]
			public static GetCurrentDC wglGetCurrentDC;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate IntPtr GetDefaultProcAddress(String lpszProc);
			[CLSCompliant(false)]
			public static GetDefaultProcAddress wglGetDefaultProcAddress;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate IntPtr GetProcAddress(String lpszProc);
			[CLSCompliant(false)]
			public static GetProcAddress wglGetProcAddress;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate int GetPixelFormat(IntPtr hdc);
			[CLSCompliant(false)]
			public static GetPixelFormat wglGetPixelFormat;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate Boolean SetPixelFormat(IntPtr hdc, int ipfd, PixelFormatDescriptor* ppfd);
			[CLSCompliant(false)]
			public unsafe static SetPixelFormat wglSetPixelFormat;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Boolean SwapBuffers(IntPtr hdc);
			[CLSCompliant(false)]
			public static SwapBuffers wglSwapBuffers;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Boolean ShareLists(IntPtr hrcSrvShare, IntPtr hrcSrvSource);
			[CLSCompliant(false)]
			public static ShareLists wglShareLists;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate IntPtr CreateLayerContext(IntPtr hDc, int level);
			[CLSCompliant(false)]
			public static CreateLayerContext wglCreateLayerContext;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate Boolean DescribeLayerPlane(IntPtr hDc, int pixelFormat, int layerPlane, UInt32 nBytes, LayerPlaneDescriptor* plpd);
			[CLSCompliant(false)]
			public unsafe static DescribeLayerPlane wglDescribeLayerPlane;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate int SetLayerPaletteEntries(IntPtr hdc, int iLayerPlane, int iStart, int cEntries, Int32* pcr);
			[CLSCompliant(false)]
			public unsafe static SetLayerPaletteEntries wglSetLayerPaletteEntries;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate int GetLayerPaletteEntries(IntPtr hdc, int iLayerPlane, int iStart, int cEntries, Int32* pcr);
			[CLSCompliant(false)]
			public unsafe static GetLayerPaletteEntries wglGetLayerPaletteEntries;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Boolean RealizeLayerPalette(IntPtr hdc, int iLayerPlane, Boolean bRealize);
			[CLSCompliant(false)]
			public static RealizeLayerPalette wglRealizeLayerPalette;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Boolean SwapLayerBuffers(IntPtr hdc, UInt32 fuFlags);
			[CLSCompliant(false)]
			public static SwapLayerBuffers wglSwapLayerBuffers;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Boolean UseFontBitmapsA(IntPtr hDC, Int32 first, Int32 count, Int32 listBase);
			[CLSCompliant(false)]
			public static UseFontBitmapsA wglUseFontBitmapsA;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Boolean UseFontBitmapsW(IntPtr hDC, Int32 first, Int32 count, Int32 listBase);
			[CLSCompliant(false)]
			public static UseFontBitmapsW wglUseFontBitmapsW;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate Boolean UseFontOutlinesA(IntPtr hDC, Int32 first, Int32 count, Int32 listBase, float thickness, float deviation, Int32 fontMode, GlyphMetricsFloat* glyphMetrics);
			[CLSCompliant(false)]
			public unsafe static UseFontOutlinesA wglUseFontOutlinesA;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate Boolean UseFontOutlinesW(IntPtr hDC, Int32 first, Int32 count, Int32 listBase, float thickness, float deviation, Int32 fontMode, GlyphMetricsFloat* glyphMetrics);
			[CLSCompliant(false)]
			public unsafe static UseFontOutlinesW wglUseFontOutlinesW;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate IntPtr CreateContextAttribsARB(IntPtr hDC, IntPtr hShareContext, int* attribList);
			[CLSCompliant(false)]
			public unsafe static CreateContextAttribsARB wglCreateContextAttribsARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate IntPtr CreateBufferRegionARB(IntPtr hDC, int iLayerPlane, UInt32 uType);
			[CLSCompliant(false)]
			public static CreateBufferRegionARB wglCreateBufferRegionARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DeleteBufferRegionARB(IntPtr hRegion);
			[CLSCompliant(false)]
			public static DeleteBufferRegionARB wglDeleteBufferRegionARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Boolean SaveBufferRegionARB(IntPtr hRegion, int x, int y, int width, int height);
			[CLSCompliant(false)]
			public static SaveBufferRegionARB wglSaveBufferRegionARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Boolean RestoreBufferRegionARB(IntPtr hRegion, int x, int y, int width, int height, int xSrc, int ySrc);
			[CLSCompliant(false)]
			public static RestoreBufferRegionARB wglRestoreBufferRegionARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate IntPtr GetExtensionsStringARB(IntPtr hdc);
			[CLSCompliant(false)]
			public static GetExtensionsStringARB wglGetExtensionsStringARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate Boolean GetPixelFormatAttribivARB(IntPtr hdc, int iPixelFormat, int iLayerPlane, UInt32 nAttributes, int* piAttributes, [Out] int* piValues);
			[CLSCompliant(false)]
			public unsafe static GetPixelFormatAttribivARB wglGetPixelFormatAttribivARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate Boolean GetPixelFormatAttribfvARB(IntPtr hdc, int iPixelFormat, int iLayerPlane, UInt32 nAttributes, int* piAttributes, [Out] Single* pfValues);
			[CLSCompliant(false)]
			public unsafe static GetPixelFormatAttribfvARB wglGetPixelFormatAttribfvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate Boolean ChoosePixelFormatARB(IntPtr hdc, int* piAttribIList, Single* pfAttribFList, UInt32 nMaxFormats, [Out] int* piFormats, [Out] UInt32* nNumFormats);
			[CLSCompliant(false)]
			public unsafe static ChoosePixelFormatARB wglChoosePixelFormatARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Boolean MakeContextCurrentARB(IntPtr hDrawDC, IntPtr hReadDC, IntPtr hglrc);
			[CLSCompliant(false)]
			public static MakeContextCurrentARB wglMakeContextCurrentARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate IntPtr GetCurrentReadDCARB();
			[CLSCompliant(false)]
			public static GetCurrentReadDCARB wglGetCurrentReadDCARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate IntPtr CreatePbufferARB(IntPtr hDC, int iPixelFormat, int iWidth, int iHeight, int* piAttribList);
			[CLSCompliant(false)]
			public unsafe static CreatePbufferARB wglCreatePbufferARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate IntPtr GetPbufferDCARB(IntPtr hPbuffer);
			[CLSCompliant(false)]
			public static GetPbufferDCARB wglGetPbufferDCARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate int ReleasePbufferDCARB(IntPtr hPbuffer, IntPtr hDC);
			[CLSCompliant(false)]
			public static ReleasePbufferDCARB wglReleasePbufferDCARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Boolean DestroyPbufferARB(IntPtr hPbuffer);
			[CLSCompliant(false)]
			public static DestroyPbufferARB wglDestroyPbufferARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate Boolean QueryPbufferARB(IntPtr hPbuffer, int iAttribute, [Out] int* piValue);
			[CLSCompliant(false)]
			public unsafe static QueryPbufferARB wglQueryPbufferARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Boolean BindTexImageARB(IntPtr hPbuffer, int iBuffer);
			[CLSCompliant(false)]
			public static BindTexImageARB wglBindTexImageARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Boolean ReleaseTexImageARB(IntPtr hPbuffer, int iBuffer);
			[CLSCompliant(false)]
			public static ReleaseTexImageARB wglReleaseTexImageARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate Boolean SetPbufferAttribARB(IntPtr hPbuffer, int* piAttribList);
			[CLSCompliant(false)]
			public unsafe static SetPbufferAttribARB wglSetPbufferAttribARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool CreateDisplayColorTableEXT(UInt16 id);
			[CLSCompliant(false)]
			public static CreateDisplayColorTableEXT wglCreateDisplayColorTableEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate bool LoadDisplayColorTableEXT(UInt16* table, UInt32 length);
			[CLSCompliant(false)]
			public unsafe static LoadDisplayColorTableEXT wglLoadDisplayColorTableEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool BindDisplayColorTableEXT(UInt16 id);
			[CLSCompliant(false)]
			public static BindDisplayColorTableEXT wglBindDisplayColorTableEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DestroyDisplayColorTableEXT(UInt16 id);
			[CLSCompliant(false)]
			public static DestroyDisplayColorTableEXT wglDestroyDisplayColorTableEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate IntPtr GetExtensionsStringEXT();
			[CLSCompliant(false)]
			public static GetExtensionsStringEXT wglGetExtensionsStringEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Boolean MakeContextCurrentEXT(IntPtr hDrawDC, IntPtr hReadDC, IntPtr hglrc);
			[CLSCompliant(false)]
			public static MakeContextCurrentEXT wglMakeContextCurrentEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate IntPtr GetCurrentReadDCEXT();
			[CLSCompliant(false)]
			public static GetCurrentReadDCEXT wglGetCurrentReadDCEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate IntPtr CreatePbufferEXT(IntPtr hDC, int iPixelFormat, int iWidth, int iHeight, int* piAttribList);
			[CLSCompliant(false)]
			public unsafe static CreatePbufferEXT wglCreatePbufferEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate IntPtr GetPbufferDCEXT(IntPtr hPbuffer);
			[CLSCompliant(false)]
			public static GetPbufferDCEXT wglGetPbufferDCEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate int ReleasePbufferDCEXT(IntPtr hPbuffer, IntPtr hDC);
			[CLSCompliant(false)]
			public static ReleasePbufferDCEXT wglReleasePbufferDCEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Boolean DestroyPbufferEXT(IntPtr hPbuffer);
			[CLSCompliant(false)]
			public static DestroyPbufferEXT wglDestroyPbufferEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate Boolean QueryPbufferEXT(IntPtr hPbuffer, int iAttribute, [Out] int* piValue);
			[CLSCompliant(false)]
			public unsafe static QueryPbufferEXT wglQueryPbufferEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate Boolean GetPixelFormatAttribivEXT(IntPtr hdc, int iPixelFormat, int iLayerPlane, UInt32 nAttributes, [Out] int* piAttributes, [Out] int* piValues);
			[CLSCompliant(false)]
			public unsafe static GetPixelFormatAttribivEXT wglGetPixelFormatAttribivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate Boolean GetPixelFormatAttribfvEXT(IntPtr hdc, int iPixelFormat, int iLayerPlane, UInt32 nAttributes, [Out] int* piAttributes, [Out] Single* pfValues);
			[CLSCompliant(false)]
			public unsafe static GetPixelFormatAttribfvEXT wglGetPixelFormatAttribfvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate Boolean ChoosePixelFormatEXT(IntPtr hdc, int* piAttribIList, Single* pfAttribFList, UInt32 nMaxFormats, [Out] int* piFormats, [Out] UInt32* nNumFormats);
			[CLSCompliant(false)]
			public unsafe static ChoosePixelFormatEXT wglChoosePixelFormatEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Boolean SwapIntervalEXT(int interval);
			[CLSCompliant(false)]
			public static SwapIntervalEXT wglSwapIntervalEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate int GetSwapIntervalEXT();
			[CLSCompliant(false)]
			public static GetSwapIntervalEXT wglGetSwapIntervalEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate IntPtr AllocateMemoryNV(Int32 size, Single readfreq, Single writefreq, Single priority);
			[CLSCompliant(false)]
			public unsafe static AllocateMemoryNV wglAllocateMemoryNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FreeMemoryNV([Out] IntPtr pointer);
			[CLSCompliant(false)]
			public static FreeMemoryNV wglFreeMemoryNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate Boolean GetSyncValuesOML(IntPtr hdc, [Out] Int64* ust, [Out] Int64* msc, [Out] Int64* sbc);
			[CLSCompliant(false)]
			public unsafe static GetSyncValuesOML wglGetSyncValuesOML;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate Boolean GetMscRateOML(IntPtr hdc, [Out] Int32* numerator, [Out] Int32* denominator);
			[CLSCompliant(false)]
			public unsafe static GetMscRateOML wglGetMscRateOML;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Int64 SwapBuffersMscOML(IntPtr hdc, Int64 target_msc, Int64 divisor, Int64 remainder);
			[CLSCompliant(false)]
			public static SwapBuffersMscOML wglSwapBuffersMscOML;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Int64 SwapLayerBuffersMscOML(IntPtr hdc, int fuPlanes, Int64 target_msc, Int64 divisor, Int64 remainder);
			[CLSCompliant(false)]
			public static SwapLayerBuffersMscOML wglSwapLayerBuffersMscOML;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate Boolean WaitForMscOML(IntPtr hdc, Int64 target_msc, Int64 divisor, Int64 remainder, [Out] Int64* ust, [Out] Int64* msc, [Out] Int64* sbc);
			[CLSCompliant(false)]
			public unsafe static WaitForMscOML wglWaitForMscOML;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate Boolean WaitForSbcOML(IntPtr hdc, Int64 target_sbc, [Out] Int64* ust, [Out] Int64* msc, [Out] Int64* sbc);
			[CLSCompliant(false)]
			public unsafe static WaitForSbcOML wglWaitForSbcOML;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate Boolean GetDigitalVideoParametersI3D(IntPtr hDC, int iAttribute, [Out] int* piValue);
			[CLSCompliant(false)]
			public unsafe static GetDigitalVideoParametersI3D wglGetDigitalVideoParametersI3D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate Boolean SetDigitalVideoParametersI3D(IntPtr hDC, int iAttribute, int* piValue);
			[CLSCompliant(false)]
			public unsafe static SetDigitalVideoParametersI3D wglSetDigitalVideoParametersI3D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate Boolean GetGammaTableParametersI3D(IntPtr hDC, int iAttribute, [Out] int* piValue);
			[CLSCompliant(false)]
			public unsafe static GetGammaTableParametersI3D wglGetGammaTableParametersI3D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate Boolean SetGammaTableParametersI3D(IntPtr hDC, int iAttribute, int* piValue);
			[CLSCompliant(false)]
			public unsafe static SetGammaTableParametersI3D wglSetGammaTableParametersI3D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate Boolean GetGammaTableI3D(IntPtr hDC, int iEntries, [Out] UInt16* puRed, [Out] UInt16* puGreen, [Out] UInt16* puBlue);
			[CLSCompliant(false)]
			public unsafe static GetGammaTableI3D wglGetGammaTableI3D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate Boolean SetGammaTableI3D(IntPtr hDC, int iEntries, UInt16* puRed, UInt16* puGreen, UInt16* puBlue);
			[CLSCompliant(false)]
			public unsafe static SetGammaTableI3D wglSetGammaTableI3D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Boolean EnableGenlockI3D(IntPtr hDC);
			[CLSCompliant(false)]
			public static EnableGenlockI3D wglEnableGenlockI3D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Boolean DisableGenlockI3D(IntPtr hDC);
			[CLSCompliant(false)]
			public static DisableGenlockI3D wglDisableGenlockI3D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate Boolean IsEnabledGenlockI3D(IntPtr hDC, [Out] Boolean* pFlag);
			[CLSCompliant(false)]
			public unsafe static IsEnabledGenlockI3D wglIsEnabledGenlockI3D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Boolean GenlockSourceI3D(IntPtr hDC, UInt32 uSource);
			[CLSCompliant(false)]
			public static GenlockSourceI3D wglGenlockSourceI3D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate Boolean GetGenlockSourceI3D(IntPtr hDC, [Out] UInt32* uSource);
			[CLSCompliant(false)]
			public unsafe static GetGenlockSourceI3D wglGetGenlockSourceI3D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Boolean GenlockSourceEdgeI3D(IntPtr hDC, UInt32 uEdge);
			[CLSCompliant(false)]
			public static GenlockSourceEdgeI3D wglGenlockSourceEdgeI3D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate Boolean GetGenlockSourceEdgeI3D(IntPtr hDC, [Out] UInt32* uEdge);
			[CLSCompliant(false)]
			public unsafe static GetGenlockSourceEdgeI3D wglGetGenlockSourceEdgeI3D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Boolean GenlockSampleRateI3D(IntPtr hDC, UInt32 uRate);
			[CLSCompliant(false)]
			public static GenlockSampleRateI3D wglGenlockSampleRateI3D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate Boolean GetGenlockSampleRateI3D(IntPtr hDC, [Out] UInt32* uRate);
			[CLSCompliant(false)]
			public unsafe static GetGenlockSampleRateI3D wglGetGenlockSampleRateI3D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Boolean GenlockSourceDelayI3D(IntPtr hDC, UInt32 uDelay);
			[CLSCompliant(false)]
			public static GenlockSourceDelayI3D wglGenlockSourceDelayI3D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate Boolean GetGenlockSourceDelayI3D(IntPtr hDC, [Out] UInt32* uDelay);
			[CLSCompliant(false)]
			public unsafe static GetGenlockSourceDelayI3D wglGetGenlockSourceDelayI3D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate Boolean QueryGenlockMaxSourceDelayI3D(IntPtr hDC, [Out] UInt32* uMaxLineDelay, [Out] UInt32* uMaxPixelDelay);
			[CLSCompliant(false)]
			public unsafe static QueryGenlockMaxSourceDelayI3D wglQueryGenlockMaxSourceDelayI3D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate IntPtr CreateImageBufferI3D(IntPtr hDC, Int32 dwSize, UInt32 uFlags);
			[CLSCompliant(false)]
			public unsafe static CreateImageBufferI3D wglCreateImageBufferI3D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Boolean DestroyImageBufferI3D(IntPtr hDC, IntPtr pAddress);
			[CLSCompliant(false)]
			public static DestroyImageBufferI3D wglDestroyImageBufferI3D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate Boolean AssociateImageBufferEventsI3D(IntPtr hDC, IntPtr* pEvent, IntPtr pAddress, Int32* pSize, UInt32 count);
			[CLSCompliant(false)]
			public unsafe static AssociateImageBufferEventsI3D wglAssociateImageBufferEventsI3D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Boolean ReleaseImageBufferEventsI3D(IntPtr hDC, IntPtr pAddress, UInt32 count);
			[CLSCompliant(false)]
			public static ReleaseImageBufferEventsI3D wglReleaseImageBufferEventsI3D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Boolean EnableFrameLockI3D();
			[CLSCompliant(false)]
			public static EnableFrameLockI3D wglEnableFrameLockI3D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Boolean DisableFrameLockI3D();
			[CLSCompliant(false)]
			public static DisableFrameLockI3D wglDisableFrameLockI3D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate Boolean IsEnabledFrameLockI3D([Out] Boolean* pFlag);
			[CLSCompliant(false)]
			public unsafe static IsEnabledFrameLockI3D wglIsEnabledFrameLockI3D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate Boolean QueryFrameLockMasterI3D([Out] Boolean* pFlag);
			[CLSCompliant(false)]
			public unsafe static QueryFrameLockMasterI3D wglQueryFrameLockMasterI3D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate Boolean GetFrameUsageI3D([Out] float* pUsage);
			[CLSCompliant(false)]
			public unsafe static GetFrameUsageI3D wglGetFrameUsageI3D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Boolean BeginFrameTrackingI3D();
			[CLSCompliant(false)]
			public static BeginFrameTrackingI3D wglBeginFrameTrackingI3D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Boolean EndFrameTrackingI3D();
			[CLSCompliant(false)]
			public static EndFrameTrackingI3D wglEndFrameTrackingI3D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate Boolean QueryFrameTrackingI3D([Out] Int32* pFrameCount, [Out] Int32* pMissedFrames, [Out] float* pLastMissedUsage);
			[CLSCompliant(false)]
			public unsafe static QueryFrameTrackingI3D wglQueryFrameTrackingI3D;
		}
	}
}

#pragma warning restore 0649
#pragma warning restore 1591