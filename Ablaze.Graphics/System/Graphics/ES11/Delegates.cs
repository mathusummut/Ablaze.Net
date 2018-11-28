//Ported from OpenTK, and excellent library.

#pragma warning disable 0649
#pragma warning disable 1591

using System.Platforms;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace System.Graphics.ES11 {
	partial class GL {
		/// <summary>
		/// Lists all the available delegates (null if not available).
		/// </summary>
		[System.Security.SuppressUnmanagedCodeSecurity()]
		public static class Delegates {
			static Delegates() {
				ReloadEntryPoints();
			}

			internal static void ReloadEntryPoints() {
				Type coreClass = typeof(Core);
				GraphicsContext context = null;
				DrawableForm control = null;
				if (GraphicsContext.CurrentContext == null) {
					control = new DrawableForm();
					using (Window windowInfo = GraphicsPlatform.Factory.GetWindowInfo(control.Handle, GraphicsMode.Default)) {
						context = GraphicsPlatform.Factory.CreateGLContext(GraphicsMode.Default, windowInfo, new MajorMinorVersion(1));
						context.MakeCurrent(windowInfo);
					}
				}
				Func<string, IntPtr> GetAddress = new Func<string, IntPtr>(GraphicsPlatform.Factory.GetAddress);
				const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly;
				MethodInfo method;
				IntPtr address;
				long addr;
				foreach (FieldInfo f in typeof(Delegates).GetFields(flags)) {
					address = GetAddress(f.Name);
					addr = address.ToInt64();
					if (addr > 3L || addr < -1L)
						f.SetValue(null, Marshal.GetDelegateForFunctionPointer(address, f.FieldType));
					else {
						method = coreClass.GetMethod(f.Name.Substring(2), flags);
						if (method != null)
							f.SetValue(null, Delegate.CreateDelegate(f.FieldType, method));
					}
				}
				if (control != null) {
					context.MakeCurrent(null as Window);
					context.Dispose();
					context = null;
					control.Dispose();
					control = null;
				}
			}

			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void ActiveTexture(System.Graphics.ES11.All texture);
			public static ActiveTexture glActiveTexture;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void AlphaFunc(System.Graphics.ES11.All func, Single @ref);
			public static AlphaFunc glAlphaFunc;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void AlphaFuncx(System.Graphics.ES11.All func, int @ref);
			public static AlphaFuncx glAlphaFuncx;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void AlphaFuncxOES(System.Graphics.ES11.All func, int @ref);
			public static AlphaFuncxOES glAlphaFuncxOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindBuffer(System.Graphics.ES11.All target, UInt32 buffer);
			[CLSCompliant(false)]
			public static BindBuffer glBindBuffer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindFramebufferOES(System.Graphics.ES11.All target, UInt32 framebuffer);
			[CLSCompliant(false)]
			public static BindFramebufferOES glBindFramebufferOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindRenderbufferOES(System.Graphics.ES11.All target, UInt32 renderbuffer);
			[CLSCompliant(false)]
			public static BindRenderbufferOES glBindRenderbufferOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindTexture(System.Graphics.ES11.All target, UInt32 texture);
			[CLSCompliant(false)]
			public static BindTexture glBindTexture;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void BlendEquationOES(System.Graphics.ES11.All mode);
			public static BlendEquationOES glBlendEquationOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void BlendEquationSeparateOES(System.Graphics.ES11.All modeRGB, System.Graphics.ES11.All modeAlpha);
			public static BlendEquationSeparateOES glBlendEquationSeparateOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void BlendFunc(System.Graphics.ES11.All sfactor, System.Graphics.ES11.All dfactor);
			public static BlendFunc glBlendFunc;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void BlendFuncSeparateOES(System.Graphics.ES11.All srcRGB, System.Graphics.ES11.All dstRGB, System.Graphics.ES11.All srcAlpha, System.Graphics.ES11.All dstAlpha);
			public static BlendFuncSeparateOES glBlendFuncSeparateOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void BufferData(System.Graphics.ES11.All target, IntPtr size, IntPtr data, System.Graphics.ES11.All usage);
			public static BufferData glBufferData;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void BufferSubData(System.Graphics.ES11.All target, IntPtr offset, IntPtr size, IntPtr data);
			public static BufferSubData glBufferSubData;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate System.Graphics.ES11.All CheckFramebufferStatusOES(System.Graphics.ES11.All target);
			public static CheckFramebufferStatusOES glCheckFramebufferStatusOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Clear(UInt32 mask);
			[CLSCompliant(false)]
			public static Clear glClear;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void ClearColor(Single red, Single green, Single blue, Single alpha);
			public static ClearColor glClearColor;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void ClearColorx(int red, int green, int blue, int alpha);
			public static ClearColorx glClearColorx;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void ClearColorxOES(int red, int green, int blue, int alpha);
			public static ClearColorxOES glClearColorxOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void ClearDepthf(Single depth);
			public static ClearDepthf glClearDepthf;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void ClearDepthfOES(Single depth);
			public static ClearDepthfOES glClearDepthfOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void ClearDepthx(int depth);
			public static ClearDepthx glClearDepthx;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void ClearDepthxOES(int depth);
			public static ClearDepthxOES glClearDepthxOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void ClearStencil(Int32 s);
			public static ClearStencil glClearStencil;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void ClientActiveTexture(System.Graphics.ES11.All texture);
			public static ClientActiveTexture glClientActiveTexture;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ClipPlanef(System.Graphics.ES11.All plane, Single* equation);
			[CLSCompliant(false)]
			public unsafe static ClipPlanef glClipPlanef;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ClipPlanefIMG(System.Graphics.ES11.All p, Single* eqn);
			[CLSCompliant(false)]
			public unsafe static ClipPlanefIMG glClipPlanefIMG;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ClipPlanefOES(System.Graphics.ES11.All plane, Single* equation);
			[CLSCompliant(false)]
			public unsafe static ClipPlanefOES glClipPlanefOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ClipPlanex(System.Graphics.ES11.All plane, int* equation);
			[CLSCompliant(false)]
			public unsafe static ClipPlanex glClipPlanex;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ClipPlanexIMG(System.Graphics.ES11.All p, int* eqn);
			[CLSCompliant(false)]
			public unsafe static ClipPlanexIMG glClipPlanexIMG;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ClipPlanexOES(System.Graphics.ES11.All plane, int* equation);
			[CLSCompliant(false)]
			public unsafe static ClipPlanexOES glClipPlanexOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Color4f(Single red, Single green, Single blue, Single alpha);
			public static Color4f glColor4f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Color4ub(Byte red, Byte green, Byte blue, Byte alpha);
			public static Color4ub glColor4ub;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Color4x(int red, int green, int blue, int alpha);
			public static Color4x glColor4x;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Color4xOES(int red, int green, int blue, int alpha);
			public static Color4xOES glColor4xOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void ColorMask(bool red, bool green, bool blue, bool alpha);
			public static ColorMask glColorMask;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void ColorPointer(Int32 size, System.Graphics.ES11.All type, Int32 stride, IntPtr pointer);
			public static ColorPointer glColorPointer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void CompressedTexImage2D(System.Graphics.ES11.All target, Int32 level, System.Graphics.ES11.All internalformat, Int32 width, Int32 height, Int32 border, Int32 imageSize, IntPtr data);
			public static CompressedTexImage2D glCompressedTexImage2D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void CompressedTexSubImage2D(System.Graphics.ES11.All target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 width, Int32 height, System.Graphics.ES11.All format, Int32 imageSize, IntPtr data);
			public static CompressedTexSubImage2D glCompressedTexSubImage2D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void CopyTexImage2D(System.Graphics.ES11.All target, Int32 level, System.Graphics.ES11.All internalformat, Int32 x, Int32 y, Int32 width, Int32 height, Int32 border);
			public static CopyTexImage2D glCopyTexImage2D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void CopyTexSubImage2D(System.Graphics.ES11.All target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 x, Int32 y, Int32 width, Int32 height);
			public static CopyTexSubImage2D glCopyTexSubImage2D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void CullFace(System.Graphics.ES11.All mode);
			public static CullFace glCullFace;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CurrentPaletteMatrixOES(UInt32 matrixpaletteindex);
			[CLSCompliant(false)]
			public static CurrentPaletteMatrixOES glCurrentPaletteMatrixOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DeleteBuffers(Int32 n, UInt32* buffers);
			[CLSCompliant(false)]
			public unsafe static DeleteBuffers glDeleteBuffers;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DeleteFencesNV(Int32 n, UInt32* fences);
			[CLSCompliant(false)]
			public unsafe static DeleteFencesNV glDeleteFencesNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DeleteFramebuffersOES(Int32 n, UInt32* framebuffers);
			[CLSCompliant(false)]
			public unsafe static DeleteFramebuffersOES glDeleteFramebuffersOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DeleteRenderbuffersOES(Int32 n, UInt32* renderbuffers);
			[CLSCompliant(false)]
			public unsafe static DeleteRenderbuffersOES glDeleteRenderbuffersOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DeleteTextures(Int32 n, UInt32* textures);
			[CLSCompliant(false)]
			public unsafe static DeleteTextures glDeleteTextures;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void DepthFunc(System.Graphics.ES11.All func);
			public static DepthFunc glDepthFunc;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void DepthMask(bool flag);
			public static DepthMask glDepthMask;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void DepthRangef(Single zNear, Single zFar);
			public static DepthRangef glDepthRangef;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void DepthRangefOES(Single zNear, Single zFar);
			public static DepthRangefOES glDepthRangefOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void DepthRangex(int zNear, int zFar);
			public static DepthRangex glDepthRangex;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void DepthRangexOES(int zNear, int zFar);
			public static DepthRangexOES glDepthRangexOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Disable(System.Graphics.ES11.All cap);
			public static Disable glDisable;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void DisableClientState(System.Graphics.ES11.All array);
			public static DisableClientState glDisableClientState;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DisableDriverControlQCOM(UInt32 driverControl);
			[CLSCompliant(false)]
			public static DisableDriverControlQCOM glDisableDriverControlQCOM;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void DrawArrays(System.Graphics.ES11.All mode, Int32 first, Int32 count);
			public static DrawArrays glDrawArrays;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void DrawElements(System.Graphics.ES11.All mode, Int32 count, System.Graphics.ES11.All type, IntPtr indices);
			public static DrawElements glDrawElements;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void DrawTexfOES(Single x, Single y, Single z, Single width, Single height);
			public static DrawTexfOES glDrawTexfOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DrawTexfvOES(Single* coords);
			[CLSCompliant(false)]
			public unsafe static DrawTexfvOES glDrawTexfvOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void DrawTexiOES(Int32 x, Int32 y, Int32 z, Int32 width, Int32 height);
			public static DrawTexiOES glDrawTexiOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DrawTexivOES(Int32* coords);
			[CLSCompliant(false)]
			public unsafe static DrawTexivOES glDrawTexivOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void DrawTexsOES(Int16 x, Int16 y, Int16 z, Int16 width, Int16 height);
			public static DrawTexsOES glDrawTexsOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DrawTexsvOES(Int16* coords);
			[CLSCompliant(false)]
			public unsafe static DrawTexsvOES glDrawTexsvOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void DrawTexxOES(int x, int y, int z, int width, int height);
			public static DrawTexxOES glDrawTexxOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DrawTexxvOES(int* coords);
			[CLSCompliant(false)]
			public unsafe static DrawTexxvOES glDrawTexxvOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void EGLImageTargetRenderbufferStorageOES(System.Graphics.ES11.All target, IntPtr image);
			public static EGLImageTargetRenderbufferStorageOES glEGLImageTargetRenderbufferStorageOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void EGLImageTargetTexture2DOES(System.Graphics.ES11.All target, IntPtr image);
			public static EGLImageTargetTexture2DOES glEGLImageTargetTexture2DOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Enable(System.Graphics.ES11.All cap);
			public static Enable glEnable;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void EnableClientState(System.Graphics.ES11.All array);
			public static EnableClientState glEnableClientState;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void EnableDriverControlQCOM(UInt32 driverControl);
			[CLSCompliant(false)]
			public static EnableDriverControlQCOM glEnableDriverControlQCOM;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Finish();
			public static Finish glFinish;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FinishFenceNV(UInt32 fence);
			[CLSCompliant(false)]
			public static FinishFenceNV glFinishFenceNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Flush();
			public static Flush glFlush;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Fogf(System.Graphics.ES11.All pname, Single param);
			public static Fogf glFogf;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Fogfv(System.Graphics.ES11.All pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static Fogfv glFogfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Fogx(System.Graphics.ES11.All pname, int param);
			public static Fogx glFogx;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void FogxOES(System.Graphics.ES11.All pname, int param);
			public static FogxOES glFogxOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Fogxv(System.Graphics.ES11.All pname, int* @params);
			[CLSCompliant(false)]
			public unsafe static Fogxv glFogxv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void FogxvOES(System.Graphics.ES11.All pname, int* @params);
			[CLSCompliant(false)]
			public unsafe static FogxvOES glFogxvOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FramebufferRenderbufferOES(System.Graphics.ES11.All target, System.Graphics.ES11.All attachment, System.Graphics.ES11.All renderbuffertarget, UInt32 renderbuffer);
			[CLSCompliant(false)]
			public static FramebufferRenderbufferOES glFramebufferRenderbufferOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FramebufferTexture2DOES(System.Graphics.ES11.All target, System.Graphics.ES11.All attachment, System.Graphics.ES11.All textarget, UInt32 texture, Int32 level);
			[CLSCompliant(false)]
			public static FramebufferTexture2DOES glFramebufferTexture2DOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void FrontFace(System.Graphics.ES11.All mode);
			public static FrontFace glFrontFace;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Frustumf(Single left, Single right, Single bottom, Single top, Single zNear, Single zFar);
			public static Frustumf glFrustumf;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void FrustumfOES(Single left, Single right, Single bottom, Single top, Single zNear, Single zFar);
			public static FrustumfOES glFrustumfOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Frustumx(int left, int right, int bottom, int top, int zNear, int zFar);
			public static Frustumx glFrustumx;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void FrustumxOES(int left, int right, int bottom, int top, int zNear, int zFar);
			public static FrustumxOES glFrustumxOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GenBuffers(Int32 n, UInt32* buffers);
			[CLSCompliant(false)]
			public unsafe static GenBuffers glGenBuffers;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void GenerateMipmapOES(System.Graphics.ES11.All target);
			public static GenerateMipmapOES glGenerateMipmapOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GenFencesNV(Int32 n, UInt32* fences);
			[CLSCompliant(false)]
			public unsafe static GenFencesNV glGenFencesNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GenFramebuffersOES(Int32 n, UInt32* framebuffers);
			[CLSCompliant(false)]
			public unsafe static GenFramebuffersOES glGenFramebuffersOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GenRenderbuffersOES(Int32 n, UInt32* renderbuffers);
			[CLSCompliant(false)]
			public unsafe static GenRenderbuffersOES glGenRenderbuffersOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GenTextures(Int32 n, UInt32* textures);
			[CLSCompliant(false)]
			public unsafe static GenTextures glGenTextures;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetBooleanv(System.Graphics.ES11.All pname, bool* @params);
			[CLSCompliant(false)]
			public unsafe static GetBooleanv glGetBooleanv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetBufferParameteriv(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetBufferParameteriv glGetBufferParameteriv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void GetBufferPointervOES(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, IntPtr @params);
			public static GetBufferPointervOES glGetBufferPointervOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetClipPlanef(System.Graphics.ES11.All pname, Single* eqn);
			[CLSCompliant(false)]
			public unsafe static GetClipPlanef glGetClipPlanef;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetClipPlanefOES(System.Graphics.ES11.All pname, Single* eqn);
			[CLSCompliant(false)]
			public unsafe static GetClipPlanefOES glGetClipPlanefOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetClipPlanex(System.Graphics.ES11.All pname, int* eqn);
			[CLSCompliant(false)]
			public unsafe static GetClipPlanex glGetClipPlanex;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetClipPlanexOES(System.Graphics.ES11.All pname, int* eqn);
			[CLSCompliant(false)]
			public unsafe static GetClipPlanexOES glGetClipPlanexOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetDriverControlsQCOM(Int32* num, Int32 size, UInt32* driverControls);
			[CLSCompliant(false)]
			public unsafe static GetDriverControlsQCOM glGetDriverControlsQCOM;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetDriverControlStringQCOM(UInt32 driverControl, Int32 bufSize, Int32* length, String driverControlString);
			[CLSCompliant(false)]
			public unsafe static GetDriverControlStringQCOM glGetDriverControlStringQCOM;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate System.Graphics.ES11.All GetError();
			public static GetError glGetError;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetFenceivNV(UInt32 fence, System.Graphics.ES11.All pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetFenceivNV glGetFenceivNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetFixedv(System.Graphics.ES11.All pname, int* @params);
			[CLSCompliant(false)]
			public unsafe static GetFixedv glGetFixedv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetFixedvOES(System.Graphics.ES11.All pname, int* @params);
			[CLSCompliant(false)]
			public unsafe static GetFixedvOES glGetFixedvOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetFloatv(System.Graphics.ES11.All pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetFloatv glGetFloatv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetFramebufferAttachmentParameterivOES(System.Graphics.ES11.All target, System.Graphics.ES11.All attachment, System.Graphics.ES11.All pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetFramebufferAttachmentParameterivOES glGetFramebufferAttachmentParameterivOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetIntegerv(System.Graphics.ES11.All pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetIntegerv glGetIntegerv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetLightfv(System.Graphics.ES11.All light, System.Graphics.ES11.All pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetLightfv glGetLightfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetLightxv(System.Graphics.ES11.All light, System.Graphics.ES11.All pname, int* @params);
			[CLSCompliant(false)]
			public unsafe static GetLightxv glGetLightxv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetLightxvOES(System.Graphics.ES11.All light, System.Graphics.ES11.All pname, int* @params);
			[CLSCompliant(false)]
			public unsafe static GetLightxvOES glGetLightxvOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetMaterialfv(System.Graphics.ES11.All face, System.Graphics.ES11.All pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetMaterialfv glGetMaterialfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetMaterialxv(System.Graphics.ES11.All face, System.Graphics.ES11.All pname, int* @params);
			[CLSCompliant(false)]
			public unsafe static GetMaterialxv glGetMaterialxv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetMaterialxvOES(System.Graphics.ES11.All face, System.Graphics.ES11.All pname, int* @params);
			[CLSCompliant(false)]
			public unsafe static GetMaterialxvOES glGetMaterialxvOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void GetPointerv(System.Graphics.ES11.All pname, IntPtr @params);
			public static GetPointerv glGetPointerv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetRenderbufferParameterivOES(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetRenderbufferParameterivOES glGetRenderbufferParameterivOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public unsafe delegate System.IntPtr GetString(System.Graphics.ES11.All name);
			public unsafe static GetString glGetString;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetTexEnvfv(System.Graphics.ES11.All env, System.Graphics.ES11.All pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetTexEnvfv glGetTexEnvfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetTexEnviv(System.Graphics.ES11.All env, System.Graphics.ES11.All pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetTexEnviv glGetTexEnviv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetTexEnvxv(System.Graphics.ES11.All env, System.Graphics.ES11.All pname, int* @params);
			[CLSCompliant(false)]
			public unsafe static GetTexEnvxv glGetTexEnvxv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetTexEnvxvOES(System.Graphics.ES11.All env, System.Graphics.ES11.All pname, int* @params);
			[CLSCompliant(false)]
			public unsafe static GetTexEnvxvOES glGetTexEnvxvOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetTexGenfvOES(System.Graphics.ES11.All coord, System.Graphics.ES11.All pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetTexGenfvOES glGetTexGenfvOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetTexGenivOES(System.Graphics.ES11.All coord, System.Graphics.ES11.All pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetTexGenivOES glGetTexGenivOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetTexGenxvOES(System.Graphics.ES11.All coord, System.Graphics.ES11.All pname, int* @params);
			[CLSCompliant(false)]
			public unsafe static GetTexGenxvOES glGetTexGenxvOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetTexParameterfv(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetTexParameterfv glGetTexParameterfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetTexParameteriv(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetTexParameteriv glGetTexParameteriv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetTexParameterxv(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, int* @params);
			[CLSCompliant(false)]
			public unsafe static GetTexParameterxv glGetTexParameterxv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetTexParameterxvOES(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, int* @params);
			[CLSCompliant(false)]
			public unsafe static GetTexParameterxvOES glGetTexParameterxvOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Hint(System.Graphics.ES11.All target, System.Graphics.ES11.All mode);
			public static Hint glHint;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsBuffer(UInt32 buffer);
			[CLSCompliant(false)]
			public static IsBuffer glIsBuffer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate bool IsEnabled(System.Graphics.ES11.All cap);
			public static IsEnabled glIsEnabled;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsFenceNV(UInt32 fence);
			[CLSCompliant(false)]
			public static IsFenceNV glIsFenceNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsFramebufferOES(UInt32 framebuffer);
			[CLSCompliant(false)]
			public static IsFramebufferOES glIsFramebufferOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsRenderbufferOES(UInt32 renderbuffer);
			[CLSCompliant(false)]
			public static IsRenderbufferOES glIsRenderbufferOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsTexture(UInt32 texture);
			[CLSCompliant(false)]
			public static IsTexture glIsTexture;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Lightf(System.Graphics.ES11.All light, System.Graphics.ES11.All pname, Single param);
			public static Lightf glLightf;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Lightfv(System.Graphics.ES11.All light, System.Graphics.ES11.All pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static Lightfv glLightfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void LightModelf(System.Graphics.ES11.All pname, Single param);
			public static LightModelf glLightModelf;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void LightModelfv(System.Graphics.ES11.All pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static LightModelfv glLightModelfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void LightModelx(System.Graphics.ES11.All pname, int param);
			public static LightModelx glLightModelx;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void LightModelxOES(System.Graphics.ES11.All pname, int param);
			public static LightModelxOES glLightModelxOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void LightModelxv(System.Graphics.ES11.All pname, int* @params);
			[CLSCompliant(false)]
			public unsafe static LightModelxv glLightModelxv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void LightModelxvOES(System.Graphics.ES11.All pname, int* @params);
			[CLSCompliant(false)]
			public unsafe static LightModelxvOES glLightModelxvOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Lightx(System.Graphics.ES11.All light, System.Graphics.ES11.All pname, int param);
			public static Lightx glLightx;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void LightxOES(System.Graphics.ES11.All light, System.Graphics.ES11.All pname, int param);
			public static LightxOES glLightxOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Lightxv(System.Graphics.ES11.All light, System.Graphics.ES11.All pname, int* @params);
			[CLSCompliant(false)]
			public unsafe static Lightxv glLightxv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void LightxvOES(System.Graphics.ES11.All light, System.Graphics.ES11.All pname, int* @params);
			[CLSCompliant(false)]
			public unsafe static LightxvOES glLightxvOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void LineWidth(Single width);
			public static LineWidth glLineWidth;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void LineWidthx(int width);
			public static LineWidthx glLineWidthx;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void LineWidthxOES(int width);
			public static LineWidthxOES glLineWidthxOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void LoadIdentity();
			public static LoadIdentity glLoadIdentity;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void LoadMatrixf(Single* m);
			[CLSCompliant(false)]
			public unsafe static LoadMatrixf glLoadMatrixf;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void LoadMatrixx(int* m);
			[CLSCompliant(false)]
			public unsafe static LoadMatrixx glLoadMatrixx;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void LoadMatrixxOES(int* m);
			[CLSCompliant(false)]
			public unsafe static LoadMatrixxOES glLoadMatrixxOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void LoadPaletteFromModelViewMatrixOES();
			public static LoadPaletteFromModelViewMatrixOES glLoadPaletteFromModelViewMatrixOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void LogicOp(System.Graphics.ES11.All opcode);
			public static LogicOp glLogicOp;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public unsafe delegate System.IntPtr MapBufferOES(System.Graphics.ES11.All target, System.Graphics.ES11.All access);
			public unsafe static MapBufferOES glMapBufferOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Materialf(System.Graphics.ES11.All face, System.Graphics.ES11.All pname, Single param);
			public static Materialf glMaterialf;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Materialfv(System.Graphics.ES11.All face, System.Graphics.ES11.All pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static Materialfv glMaterialfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Materialx(System.Graphics.ES11.All face, System.Graphics.ES11.All pname, int param);
			public static Materialx glMaterialx;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void MaterialxOES(System.Graphics.ES11.All face, System.Graphics.ES11.All pname, int param);
			public static MaterialxOES glMaterialxOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Materialxv(System.Graphics.ES11.All face, System.Graphics.ES11.All pname, int* @params);
			[CLSCompliant(false)]
			public unsafe static Materialxv glMaterialxv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MaterialxvOES(System.Graphics.ES11.All face, System.Graphics.ES11.All pname, int* @params);
			[CLSCompliant(false)]
			public unsafe static MaterialxvOES glMaterialxvOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void MatrixIndexPointerOES(Int32 size, System.Graphics.ES11.All type, Int32 stride, IntPtr pointer);
			public static MatrixIndexPointerOES glMatrixIndexPointerOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void MatrixMode(System.Graphics.ES11.All mode);
			public static MatrixMode glMatrixMode;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void MultiTexCoord4f(System.Graphics.ES11.All target, Single s, Single t, Single r, Single q);
			public static MultiTexCoord4f glMultiTexCoord4f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void MultiTexCoord4x(System.Graphics.ES11.All target, int s, int t, int r, int q);
			public static MultiTexCoord4x glMultiTexCoord4x;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void MultiTexCoord4xOES(System.Graphics.ES11.All target, int s, int t, int r, int q);
			public static MultiTexCoord4xOES glMultiTexCoord4xOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultMatrixf(Single* m);
			[CLSCompliant(false)]
			public unsafe static MultMatrixf glMultMatrixf;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultMatrixx(int* m);
			[CLSCompliant(false)]
			public unsafe static MultMatrixx glMultMatrixx;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultMatrixxOES(int* m);
			[CLSCompliant(false)]
			public unsafe static MultMatrixxOES glMultMatrixxOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Normal3f(Single nx, Single ny, Single nz);
			public static Normal3f glNormal3f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Normal3x(int nx, int ny, int nz);
			public static Normal3x glNormal3x;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Normal3xOES(int nx, int ny, int nz);
			public static Normal3xOES glNormal3xOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void NormalPointer(System.Graphics.ES11.All type, Int32 stride, IntPtr pointer);
			public static NormalPointer glNormalPointer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Orthof(Single left, Single right, Single bottom, Single top, Single zNear, Single zFar);
			public static Orthof glOrthof;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void OrthofOES(Single left, Single right, Single bottom, Single top, Single zNear, Single zFar);
			public static OrthofOES glOrthofOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Orthox(int left, int right, int bottom, int top, int zNear, int zFar);
			public static Orthox glOrthox;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void OrthoxOES(int left, int right, int bottom, int top, int zNear, int zFar);
			public static OrthoxOES glOrthoxOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void PixelStorei(System.Graphics.ES11.All pname, Int32 param);
			public static PixelStorei glPixelStorei;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void PointParameterf(System.Graphics.ES11.All pname, Single param);
			public static PointParameterf glPointParameterf;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void PointParameterfv(System.Graphics.ES11.All pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static PointParameterfv glPointParameterfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void PointParameterx(System.Graphics.ES11.All pname, int param);
			public static PointParameterx glPointParameterx;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void PointParameterxOES(System.Graphics.ES11.All pname, int param);
			public static PointParameterxOES glPointParameterxOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void PointParameterxv(System.Graphics.ES11.All pname, int* @params);
			[CLSCompliant(false)]
			public unsafe static PointParameterxv glPointParameterxv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void PointParameterxvOES(System.Graphics.ES11.All pname, int* @params);
			[CLSCompliant(false)]
			public unsafe static PointParameterxvOES glPointParameterxvOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void PointSize(Single size);
			public static PointSize glPointSize;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void PointSizePointerOES(System.Graphics.ES11.All type, Int32 stride, IntPtr pointer);
			public static PointSizePointerOES glPointSizePointerOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void PointSizex(int size);
			public static PointSizex glPointSizex;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void PointSizexOES(int size);
			public static PointSizexOES glPointSizexOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void PolygonOffset(Single factor, Single units);
			public static PolygonOffset glPolygonOffset;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void PolygonOffsetx(int factor, int units);
			public static PolygonOffsetx glPolygonOffsetx;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void PolygonOffsetxOES(int factor, int units);
			public static PolygonOffsetxOES glPolygonOffsetxOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void PopMatrix();
			public static PopMatrix glPopMatrix;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void PushMatrix();
			public static PushMatrix glPushMatrix;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate Int32 QueryMatrixxOES(int* mantissa, Int32* exponent);
			[CLSCompliant(false)]
			public unsafe static QueryMatrixxOES glQueryMatrixxOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void ReadPixels(Int32 x, Int32 y, Int32 width, Int32 height, System.Graphics.ES11.All format, System.Graphics.ES11.All type, IntPtr pixels);
			public static ReadPixels glReadPixels;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void RenderbufferStorageOES(System.Graphics.ES11.All target, System.Graphics.ES11.All internalformat, Int32 width, Int32 height);
			public static RenderbufferStorageOES glRenderbufferStorageOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Rotatef(Single angle, Single x, Single y, Single z);
			public static Rotatef glRotatef;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Rotatex(int angle, int x, int y, int z);
			public static Rotatex glRotatex;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void RotatexOES(int angle, int x, int y, int z);
			public static RotatexOES glRotatexOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void SampleCoverage(Single value, bool invert);
			public static SampleCoverage glSampleCoverage;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void SampleCoveragex(int value, bool invert);
			public static SampleCoveragex glSampleCoveragex;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void SampleCoveragexOES(int value, bool invert);
			public static SampleCoveragexOES glSampleCoveragexOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Scalef(Single x, Single y, Single z);
			public static Scalef glScalef;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Scalex(int x, int y, int z);
			public static Scalex glScalex;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void ScalexOES(int x, int y, int z);
			public static ScalexOES glScalexOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Scissor(Int32 x, Int32 y, Int32 width, Int32 height);
			public static Scissor glScissor;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SetFenceNV(UInt32 fence, System.Graphics.ES11.All condition);
			[CLSCompliant(false)]
			public static SetFenceNV glSetFenceNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void ShadeModel(System.Graphics.ES11.All mode);
			public static ShadeModel glShadeModel;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void StencilFunc(System.Graphics.ES11.All func, Int32 @ref, UInt32 mask);
			[CLSCompliant(false)]
			public static StencilFunc glStencilFunc;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void StencilMask(UInt32 mask);
			[CLSCompliant(false)]
			public static StencilMask glStencilMask;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void StencilOp(System.Graphics.ES11.All fail, System.Graphics.ES11.All zfail, System.Graphics.ES11.All zpass);
			public static StencilOp glStencilOp;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool TestFenceNV(UInt32 fence);
			[CLSCompliant(false)]
			public static TestFenceNV glTestFenceNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void TexCoordPointer(Int32 size, System.Graphics.ES11.All type, Int32 stride, IntPtr pointer);
			public static TexCoordPointer glTexCoordPointer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void TexEnvf(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, Single param);
			public static TexEnvf glTexEnvf;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexEnvfv(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static TexEnvfv glTexEnvfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void TexEnvi(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, Int32 param);
			public static TexEnvi glTexEnvi;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexEnviv(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static TexEnviv glTexEnviv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void TexEnvx(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, int param);
			public static TexEnvx glTexEnvx;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void TexEnvxOES(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, int param);
			public static TexEnvxOES glTexEnvxOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexEnvxv(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, int* @params);
			[CLSCompliant(false)]
			public unsafe static TexEnvxv glTexEnvxv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexEnvxvOES(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, int* @params);
			[CLSCompliant(false)]
			public unsafe static TexEnvxvOES glTexEnvxvOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void TexGenfOES(System.Graphics.ES11.All coord, System.Graphics.ES11.All pname, Single param);
			public static TexGenfOES glTexGenfOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexGenfvOES(System.Graphics.ES11.All coord, System.Graphics.ES11.All pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static TexGenfvOES glTexGenfvOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void TexGeniOES(System.Graphics.ES11.All coord, System.Graphics.ES11.All pname, Int32 param);
			public static TexGeniOES glTexGeniOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexGenivOES(System.Graphics.ES11.All coord, System.Graphics.ES11.All pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static TexGenivOES glTexGenivOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void TexGenxOES(System.Graphics.ES11.All coord, System.Graphics.ES11.All pname, int param);
			public static TexGenxOES glTexGenxOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexGenxvOES(System.Graphics.ES11.All coord, System.Graphics.ES11.All pname, int* @params);
			[CLSCompliant(false)]
			public unsafe static TexGenxvOES glTexGenxvOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void TexImage2D(System.Graphics.ES11.All target, Int32 level, Int32 internalformat, Int32 width, Int32 height, Int32 border, System.Graphics.ES11.All format, System.Graphics.ES11.All type, IntPtr pixels);
			public static TexImage2D glTexImage2D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void TexParameterf(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, Single param);
			public static TexParameterf glTexParameterf;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexParameterfv(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static TexParameterfv glTexParameterfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void TexParameteri(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, Int32 param);
			public static TexParameteri glTexParameteri;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexParameteriv(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static TexParameteriv glTexParameteriv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void TexParameterx(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, int param);
			public static TexParameterx glTexParameterx;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void TexParameterxOES(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, int param);
			public static TexParameterxOES glTexParameterxOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexParameterxv(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, int* @params);
			[CLSCompliant(false)]
			public unsafe static TexParameterxv glTexParameterxv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexParameterxvOES(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, int* @params);
			[CLSCompliant(false)]
			public unsafe static TexParameterxvOES glTexParameterxvOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void TexSubImage2D(System.Graphics.ES11.All target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 width, Int32 height, System.Graphics.ES11.All format, System.Graphics.ES11.All type, IntPtr pixels);
			public static TexSubImage2D glTexSubImage2D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Translatef(Single x, Single y, Single z);
			public static Translatef glTranslatef;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Translatex(int x, int y, int z);
			public static Translatex glTranslatex;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void TranslatexOES(int x, int y, int z);
			public static TranslatexOES glTranslatexOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate bool UnmapBufferOES(System.Graphics.ES11.All target);
			public static UnmapBufferOES glUnmapBufferOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void VertexPointer(Int32 size, System.Graphics.ES11.All type, Int32 stride, IntPtr pointer);
			public static VertexPointer glVertexPointer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Viewport(Int32 x, Int32 y, Int32 width, Int32 height);
			public static Viewport glViewport;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void WeightPointerOES(Int32 size, System.Graphics.ES11.All type, Int32 stride, IntPtr pointer);
			public static WeightPointerOES glWeightPointerOES;
		}
	}
}

#pragma warning restore 0649
#pragma warning restore 1591