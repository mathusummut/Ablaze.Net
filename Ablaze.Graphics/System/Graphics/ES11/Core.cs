//Ported from OpenTK, and excellent library.

namespace System.Graphics.ES11 {
	partial class GL {
		[System.Security.SuppressUnmanagedCodeSecurity()]
		internal static class Core {
			/*[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glActiveTexture", ExactSpelling = true)]
			internal extern static void ActiveTexture(System.Graphics.ES11.All texture);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glAlphaFunc", ExactSpelling = true)]
			internal extern static void AlphaFunc(System.Graphics.ES11.All func, Single @ref);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glAlphaFuncx", ExactSpelling = true)]
			internal extern static void AlphaFuncx(System.Graphics.ES11.All func, int @ref);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glAlphaFuncxOES", ExactSpelling = true)]
			internal extern static void AlphaFuncxOES(System.Graphics.ES11.All func, int @ref);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glBindBuffer", ExactSpelling = true)]
			internal extern static void BindBuffer(System.Graphics.ES11.All target, UInt32 buffer);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glBindFramebufferOES", ExactSpelling = true)]
			internal extern static void BindFramebufferOES(System.Graphics.ES11.All target, UInt32 framebuffer);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glBindRenderbufferOES", ExactSpelling = true)]
			internal extern static void BindRenderbufferOES(System.Graphics.ES11.All target, UInt32 renderbuffer);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glBindTexture", ExactSpelling = true)]
			internal extern static void BindTexture(System.Graphics.ES11.All target, UInt32 texture);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glBlendEquationOES", ExactSpelling = true)]
			internal extern static void BlendEquationOES(System.Graphics.ES11.All mode);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glBlendEquationSeparateOES", ExactSpelling = true)]
			internal extern static void BlendEquationSeparateOES(System.Graphics.ES11.All modeRGB, System.Graphics.ES11.All modeAlpha);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glBlendFunc", ExactSpelling = true)]
			internal extern static void BlendFunc(System.Graphics.ES11.All sfactor, System.Graphics.ES11.All dfactor);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glBlendFuncSeparateOES", ExactSpelling = true)]
			internal extern static void BlendFuncSeparateOES(System.Graphics.ES11.All srcRGB, System.Graphics.ES11.All dstRGB, System.Graphics.ES11.All srcAlpha, System.Graphics.ES11.All dstAlpha);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glBufferData", ExactSpelling = true)]
			internal extern static void BufferData(System.Graphics.ES11.All target, IntPtr size, IntPtr data, System.Graphics.ES11.All usage);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glBufferSubData", ExactSpelling = true)]
			internal extern static void BufferSubData(System.Graphics.ES11.All target, IntPtr offset, IntPtr size, IntPtr data);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glCheckFramebufferStatusOES", ExactSpelling = true)]
			internal extern static System.Graphics.ES11.All CheckFramebufferStatusOES(System.Graphics.ES11.All target);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glClear", ExactSpelling = true)]
			internal extern static void Clear(UInt32 mask);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glClearColor", ExactSpelling = true)]
			internal extern static void ClearColor(Single red, Single green, Single blue, Single alpha);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glClearColorx", ExactSpelling = true)]
			internal extern static void ClearColorx(int red, int green, int blue, int alpha);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glClearColorxOES", ExactSpelling = true)]
			internal extern static void ClearColorxOES(int red, int green, int blue, int alpha);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glClearDepthf", ExactSpelling = true)]
			internal extern static void ClearDepthf(Single depth);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glClearDepthfOES", ExactSpelling = true)]
			internal extern static void ClearDepthfOES(Single depth);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glClearDepthx", ExactSpelling = true)]
			internal extern static void ClearDepthx(int depth);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glClearDepthxOES", ExactSpelling = true)]
			internal extern static void ClearDepthxOES(int depth);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glClearStencil", ExactSpelling = true)]
			internal extern static void ClearStencil(Int32 s);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glClientActiveTexture", ExactSpelling = true)]
			internal extern static void ClientActiveTexture(System.Graphics.ES11.All texture);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glClipPlanef", ExactSpelling = true)]
			internal extern static unsafe void ClipPlanef(System.Graphics.ES11.All plane, Single* equation);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glClipPlanefIMG", ExactSpelling = true)]
			internal extern static unsafe void ClipPlanefIMG(System.Graphics.ES11.All p, Single* eqn);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glClipPlanefOES", ExactSpelling = true)]
			internal extern static unsafe void ClipPlanefOES(System.Graphics.ES11.All plane, Single* equation);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glClipPlanex", ExactSpelling = true)]
			internal extern static unsafe void ClipPlanex(System.Graphics.ES11.All plane, int* equation);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glClipPlanexIMG", ExactSpelling = true)]
			internal extern static unsafe void ClipPlanexIMG(System.Graphics.ES11.All p, int* eqn);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glClipPlanexOES", ExactSpelling = true)]
			internal extern static unsafe void ClipPlanexOES(System.Graphics.ES11.All plane, int* equation);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glColor4f", ExactSpelling = true)]
			internal extern static void Color4f(Single red, Single green, Single blue, Single alpha);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glColor4ub", ExactSpelling = true)]
			internal extern static void Color4ub(Byte red, Byte green, Byte blue, Byte alpha);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glColor4x", ExactSpelling = true)]
			internal extern static void Color4x(int red, int green, int blue, int alpha);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glColor4xOES", ExactSpelling = true)]
			internal extern static void Color4xOES(int red, int green, int blue, int alpha);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glColorMask", ExactSpelling = true)]
			internal extern static void ColorMask(bool red, bool green, bool blue, bool alpha);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glColorPointer", ExactSpelling = true)]
			internal extern static void ColorPointer(Int32 size, System.Graphics.ES11.All type, Int32 stride, IntPtr pointer);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glCompressedTexImage2D", ExactSpelling = true)]
			internal extern static void CompressedTexImage2D(System.Graphics.ES11.All target, Int32 level, System.Graphics.ES11.All internalformat, Int32 width, Int32 height, Int32 border, Int32 imageSize, IntPtr data);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glCompressedTexSubImage2D", ExactSpelling = true)]
			internal extern static void CompressedTexSubImage2D(System.Graphics.ES11.All target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 width, Int32 height, System.Graphics.ES11.All format, Int32 imageSize, IntPtr data);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glCopyTexImage2D", ExactSpelling = true)]
			internal extern static void CopyTexImage2D(System.Graphics.ES11.All target, Int32 level, System.Graphics.ES11.All internalformat, Int32 x, Int32 y, Int32 width, Int32 height, Int32 border);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glCopyTexSubImage2D", ExactSpelling = true)]
			internal extern static void CopyTexSubImage2D(System.Graphics.ES11.All target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 x, Int32 y, Int32 width, Int32 height);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glCullFace", ExactSpelling = true)]
			internal extern static void CullFace(System.Graphics.ES11.All mode);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glCurrentPaletteMatrixOES", ExactSpelling = true)]
			internal extern static void CurrentPaletteMatrixOES(UInt32 matrixpaletteindex);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glDeleteBuffers", ExactSpelling = true)]
			internal extern static unsafe void DeleteBuffers(Int32 n, UInt32* buffers);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glDeleteFencesNV", ExactSpelling = true)]
			internal extern static unsafe void DeleteFencesNV(Int32 n, UInt32* fences);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glDeleteFramebuffersOES", ExactSpelling = true)]
			internal extern static unsafe void DeleteFramebuffersOES(Int32 n, UInt32* framebuffers);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glDeleteRenderbuffersOES", ExactSpelling = true)]
			internal extern static unsafe void DeleteRenderbuffersOES(Int32 n, UInt32* renderbuffers);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glDeleteTextures", ExactSpelling = true)]
			internal extern static unsafe void DeleteTextures(Int32 n, UInt32* textures);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glDepthFunc", ExactSpelling = true)]
			internal extern static void DepthFunc(System.Graphics.ES11.All func);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glDepthMask", ExactSpelling = true)]
			internal extern static void DepthMask(bool flag);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glDepthRangef", ExactSpelling = true)]
			internal extern static void DepthRangef(Single zNear, Single zFar);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glDepthRangefOES", ExactSpelling = true)]
			internal extern static void DepthRangefOES(Single zNear, Single zFar);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glDepthRangex", ExactSpelling = true)]
			internal extern static void DepthRangex(int zNear, int zFar);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glDepthRangexOES", ExactSpelling = true)]
			internal extern static void DepthRangexOES(int zNear, int zFar);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glDisable", ExactSpelling = true)]
			internal extern static void Disable(System.Graphics.ES11.All cap);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glDisableClientState", ExactSpelling = true)]
			internal extern static void DisableClientState(System.Graphics.ES11.All array);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glDisableDriverControlQCOM", ExactSpelling = true)]
			internal extern static void DisableDriverControlQCOM(UInt32 driverControl);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glDrawArrays", ExactSpelling = true)]
			internal extern static void DrawArrays(System.Graphics.ES11.All mode, Int32 first, Int32 count);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glDrawElements", ExactSpelling = true)]
			internal extern static void DrawElements(System.Graphics.ES11.All mode, Int32 count, System.Graphics.ES11.All type, IntPtr indices);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glDrawTexfOES", ExactSpelling = true)]
			internal extern static void DrawTexfOES(Single x, Single y, Single z, Single width, Single height);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glDrawTexfvOES", ExactSpelling = true)]
			internal extern static unsafe void DrawTexfvOES(Single* coords);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glDrawTexiOES", ExactSpelling = true)]
			internal extern static void DrawTexiOES(Int32 x, Int32 y, Int32 z, Int32 width, Int32 height);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glDrawTexivOES", ExactSpelling = true)]
			internal extern static unsafe void DrawTexivOES(Int32* coords);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glDrawTexsOES", ExactSpelling = true)]
			internal extern static void DrawTexsOES(Int16 x, Int16 y, Int16 z, Int16 width, Int16 height);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glDrawTexsvOES", ExactSpelling = true)]
			internal extern static unsafe void DrawTexsvOES(Int16* coords);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glDrawTexxOES", ExactSpelling = true)]
			internal extern static void DrawTexxOES(int x, int y, int z, int width, int height);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glDrawTexxvOES", ExactSpelling = true)]
			internal extern static unsafe void DrawTexxvOES(int* coords);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glEGLImageTargetRenderbufferStorageOES", ExactSpelling = true)]
			internal extern static void EGLImageTargetRenderbufferStorageOES(System.Graphics.ES11.All target, IntPtr image);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glEGLImageTargetTexture2DOES", ExactSpelling = true)]
			internal extern static void EGLImageTargetTexture2DOES(System.Graphics.ES11.All target, IntPtr image);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glEnable", ExactSpelling = true)]
			internal extern static void Enable(System.Graphics.ES11.All cap);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glEnableClientState", ExactSpelling = true)]
			internal extern static void EnableClientState(System.Graphics.ES11.All array);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glEnableDriverControlQCOM", ExactSpelling = true)]
			internal extern static void EnableDriverControlQCOM(UInt32 driverControl);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glFinish", ExactSpelling = true)]
			internal extern static void Finish();
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glFinishFenceNV", ExactSpelling = true)]
			internal extern static void FinishFenceNV(UInt32 fence);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glFlush", ExactSpelling = true)]
			internal extern static void Flush();
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glFogf", ExactSpelling = true)]
			internal extern static void Fogf(System.Graphics.ES11.All pname, Single param);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glFogfv", ExactSpelling = true)]
			internal extern static unsafe void Fogfv(System.Graphics.ES11.All pname, Single* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glFogx", ExactSpelling = true)]
			internal extern static void Fogx(System.Graphics.ES11.All pname, int param);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glFogxOES", ExactSpelling = true)]
			internal extern static void FogxOES(System.Graphics.ES11.All pname, int param);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glFogxv", ExactSpelling = true)]
			internal extern static unsafe void Fogxv(System.Graphics.ES11.All pname, int* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glFogxvOES", ExactSpelling = true)]
			internal extern static unsafe void FogxvOES(System.Graphics.ES11.All pname, int* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glFramebufferRenderbufferOES", ExactSpelling = true)]
			internal extern static void FramebufferRenderbufferOES(System.Graphics.ES11.All target, System.Graphics.ES11.All attachment, System.Graphics.ES11.All renderbuffertarget, UInt32 renderbuffer);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glFramebufferTexture2DOES", ExactSpelling = true)]
			internal extern static void FramebufferTexture2DOES(System.Graphics.ES11.All target, System.Graphics.ES11.All attachment, System.Graphics.ES11.All textarget, UInt32 texture, Int32 level);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glFrontFace", ExactSpelling = true)]
			internal extern static void FrontFace(System.Graphics.ES11.All mode);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glFrustumf", ExactSpelling = true)]
			internal extern static void Frustumf(Single left, Single right, Single bottom, Single top, Single zNear, Single zFar);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glFrustumfOES", ExactSpelling = true)]
			internal extern static void FrustumfOES(Single left, Single right, Single bottom, Single top, Single zNear, Single zFar);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glFrustumx", ExactSpelling = true)]
			internal extern static void Frustumx(int left, int right, int bottom, int top, int zNear, int zFar);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glFrustumxOES", ExactSpelling = true)]
			internal extern static void FrustumxOES(int left, int right, int bottom, int top, int zNear, int zFar);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glGenBuffers", ExactSpelling = true)]
			internal extern static unsafe void GenBuffers(Int32 n, UInt32* buffers);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glGenerateMipmapOES", ExactSpelling = true)]
			internal extern static void GenerateMipmapOES(System.Graphics.ES11.All target);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glGenFencesNV", ExactSpelling = true)]
			internal extern static unsafe void GenFencesNV(Int32 n, UInt32* fences);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glGenFramebuffersOES", ExactSpelling = true)]
			internal extern static unsafe void GenFramebuffersOES(Int32 n, UInt32* framebuffers);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glGenRenderbuffersOES", ExactSpelling = true)]
			internal extern static unsafe void GenRenderbuffersOES(Int32 n, UInt32* renderbuffers);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glGenTextures", ExactSpelling = true)]
			internal extern static unsafe void GenTextures(Int32 n, UInt32* textures);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glGetBooleanv", ExactSpelling = true)]
			internal extern static unsafe void GetBooleanv(System.Graphics.ES11.All pname, bool* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glGetBufferParameteriv", ExactSpelling = true)]
			internal extern static unsafe void GetBufferParameteriv(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, Int32* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glGetBufferPointervOES", ExactSpelling = true)]
			internal extern static void GetBufferPointervOES(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, IntPtr @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glGetClipPlanef", ExactSpelling = true)]
			internal extern static unsafe void GetClipPlanef(System.Graphics.ES11.All pname, Single* eqn);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glGetClipPlanefOES", ExactSpelling = true)]
			internal extern static unsafe void GetClipPlanefOES(System.Graphics.ES11.All pname, Single* eqn);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glGetClipPlanex", ExactSpelling = true)]
			internal extern static unsafe void GetClipPlanex(System.Graphics.ES11.All pname, int* eqn);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glGetClipPlanexOES", ExactSpelling = true)]
			internal extern static unsafe void GetClipPlanexOES(System.Graphics.ES11.All pname, int* eqn);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glGetDriverControlsQCOM", ExactSpelling = true)]
			internal extern static unsafe void GetDriverControlsQCOM(Int32* num, Int32 size, UInt32* driverControls);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glGetDriverControlStringQCOM", ExactSpelling = true)]
			internal extern static unsafe void GetDriverControlStringQCOM(UInt32 driverControl, Int32 bufSize, Int32* length, String driverControlString);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glGetError", ExactSpelling = true)]
			internal extern static System.Graphics.ES11.All GetError();
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glGetFenceivNV", ExactSpelling = true)]
			internal extern static unsafe void GetFenceivNV(UInt32 fence, System.Graphics.ES11.All pname, Int32* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glGetFixedv", ExactSpelling = true)]
			internal extern static unsafe void GetFixedv(System.Graphics.ES11.All pname, int* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glGetFixedvOES", ExactSpelling = true)]
			internal extern static unsafe void GetFixedvOES(System.Graphics.ES11.All pname, int* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glGetFloatv", ExactSpelling = true)]
			internal extern static unsafe void GetFloatv(System.Graphics.ES11.All pname, Single* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glGetFramebufferAttachmentParameterivOES", ExactSpelling = true)]
			internal extern static unsafe void GetFramebufferAttachmentParameterivOES(System.Graphics.ES11.All target, System.Graphics.ES11.All attachment, System.Graphics.ES11.All pname, Int32* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glGetIntegerv", ExactSpelling = true)]
			internal extern static unsafe void GetIntegerv(System.Graphics.ES11.All pname, Int32* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glGetLightfv", ExactSpelling = true)]
			internal extern static unsafe void GetLightfv(System.Graphics.ES11.All light, System.Graphics.ES11.All pname, Single* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glGetLightxv", ExactSpelling = true)]
			internal extern static unsafe void GetLightxv(System.Graphics.ES11.All light, System.Graphics.ES11.All pname, int* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glGetLightxvOES", ExactSpelling = true)]
			internal extern static unsafe void GetLightxvOES(System.Graphics.ES11.All light, System.Graphics.ES11.All pname, int* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glGetMaterialfv", ExactSpelling = true)]
			internal extern static unsafe void GetMaterialfv(System.Graphics.ES11.All face, System.Graphics.ES11.All pname, Single* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glGetMaterialxv", ExactSpelling = true)]
			internal extern static unsafe void GetMaterialxv(System.Graphics.ES11.All face, System.Graphics.ES11.All pname, int* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glGetMaterialxvOES", ExactSpelling = true)]
			internal extern static unsafe void GetMaterialxvOES(System.Graphics.ES11.All face, System.Graphics.ES11.All pname, int* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glGetPointerv", ExactSpelling = true)]
			internal extern static void GetPointerv(System.Graphics.ES11.All pname, IntPtr @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glGetRenderbufferParameterivOES", ExactSpelling = true)]
			internal extern static unsafe void GetRenderbufferParameterivOES(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, Int32* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glGetString", ExactSpelling = true)]
			internal extern static unsafe System.IntPtr GetString(System.Graphics.ES11.All name);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glGetTexEnvfv", ExactSpelling = true)]
			internal extern static unsafe void GetTexEnvfv(System.Graphics.ES11.All env, System.Graphics.ES11.All pname, Single* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glGetTexEnviv", ExactSpelling = true)]
			internal extern static unsafe void GetTexEnviv(System.Graphics.ES11.All env, System.Graphics.ES11.All pname, Int32* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glGetTexEnvxv", ExactSpelling = true)]
			internal extern static unsafe void GetTexEnvxv(System.Graphics.ES11.All env, System.Graphics.ES11.All pname, int* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glGetTexEnvxvOES", ExactSpelling = true)]
			internal extern static unsafe void GetTexEnvxvOES(System.Graphics.ES11.All env, System.Graphics.ES11.All pname, int* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glGetTexGenfvOES", ExactSpelling = true)]
			internal extern static unsafe void GetTexGenfvOES(System.Graphics.ES11.All coord, System.Graphics.ES11.All pname, Single* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glGetTexGenivOES", ExactSpelling = true)]
			internal extern static unsafe void GetTexGenivOES(System.Graphics.ES11.All coord, System.Graphics.ES11.All pname, Int32* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glGetTexGenxvOES", ExactSpelling = true)]
			internal extern static unsafe void GetTexGenxvOES(System.Graphics.ES11.All coord, System.Graphics.ES11.All pname, int* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glGetTexParameterfv", ExactSpelling = true)]
			internal extern static unsafe void GetTexParameterfv(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, Single* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glGetTexParameteriv", ExactSpelling = true)]
			internal extern static unsafe void GetTexParameteriv(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, Int32* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glGetTexParameterxv", ExactSpelling = true)]
			internal extern static unsafe void GetTexParameterxv(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, int* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glGetTexParameterxvOES", ExactSpelling = true)]
			internal extern static unsafe void GetTexParameterxvOES(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, int* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glHint", ExactSpelling = true)]
			internal extern static void Hint(System.Graphics.ES11.All target, System.Graphics.ES11.All mode);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glIsBuffer", ExactSpelling = true)]
			internal extern static bool IsBuffer(UInt32 buffer);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glIsEnabled", ExactSpelling = true)]
			internal extern static bool IsEnabled(System.Graphics.ES11.All cap);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glIsFenceNV", ExactSpelling = true)]
			internal extern static bool IsFenceNV(UInt32 fence);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glIsFramebufferOES", ExactSpelling = true)]
			internal extern static bool IsFramebufferOES(UInt32 framebuffer);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glIsRenderbufferOES", ExactSpelling = true)]
			internal extern static bool IsRenderbufferOES(UInt32 renderbuffer);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glIsTexture", ExactSpelling = true)]
			internal extern static bool IsTexture(UInt32 texture);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glLightf", ExactSpelling = true)]
			internal extern static void Lightf(System.Graphics.ES11.All light, System.Graphics.ES11.All pname, Single param);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glLightfv", ExactSpelling = true)]
			internal extern static unsafe void Lightfv(System.Graphics.ES11.All light, System.Graphics.ES11.All pname, Single* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glLightModelf", ExactSpelling = true)]
			internal extern static void LightModelf(System.Graphics.ES11.All pname, Single param);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glLightModelfv", ExactSpelling = true)]
			internal extern static unsafe void LightModelfv(System.Graphics.ES11.All pname, Single* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glLightModelx", ExactSpelling = true)]
			internal extern static void LightModelx(System.Graphics.ES11.All pname, int param);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glLightModelxOES", ExactSpelling = true)]
			internal extern static void LightModelxOES(System.Graphics.ES11.All pname, int param);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glLightModelxv", ExactSpelling = true)]
			internal extern static unsafe void LightModelxv(System.Graphics.ES11.All pname, int* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glLightModelxvOES", ExactSpelling = true)]
			internal extern static unsafe void LightModelxvOES(System.Graphics.ES11.All pname, int* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glLightx", ExactSpelling = true)]
			internal extern static void Lightx(System.Graphics.ES11.All light, System.Graphics.ES11.All pname, int param);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glLightxOES", ExactSpelling = true)]
			internal extern static void LightxOES(System.Graphics.ES11.All light, System.Graphics.ES11.All pname, int param);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glLightxv", ExactSpelling = true)]
			internal extern static unsafe void Lightxv(System.Graphics.ES11.All light, System.Graphics.ES11.All pname, int* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glLightxvOES", ExactSpelling = true)]
			internal extern static unsafe void LightxvOES(System.Graphics.ES11.All light, System.Graphics.ES11.All pname, int* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glLineWidth", ExactSpelling = true)]
			internal extern static void LineWidth(Single width);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glLineWidthx", ExactSpelling = true)]
			internal extern static void LineWidthx(int width);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glLineWidthxOES", ExactSpelling = true)]
			internal extern static void LineWidthxOES(int width);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glLoadIdentity", ExactSpelling = true)]
			internal extern static void LoadIdentity();
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glLoadMatrixf", ExactSpelling = true)]
			internal extern static unsafe void LoadMatrixf(Single* m);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glLoadMatrixx", ExactSpelling = true)]
			internal extern static unsafe void LoadMatrixx(int* m);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glLoadMatrixxOES", ExactSpelling = true)]
			internal extern static unsafe void LoadMatrixxOES(int* m);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glLoadPaletteFromModelViewMatrixOES", ExactSpelling = true)]
			internal extern static void LoadPaletteFromModelViewMatrixOES();
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glLogicOp", ExactSpelling = true)]
			internal extern static void LogicOp(System.Graphics.ES11.All opcode);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glMapBufferOES", ExactSpelling = true)]
			internal extern static unsafe System.IntPtr MapBufferOES(System.Graphics.ES11.All target, System.Graphics.ES11.All access);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glMaterialf", ExactSpelling = true)]
			internal extern static void Materialf(System.Graphics.ES11.All face, System.Graphics.ES11.All pname, Single param);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glMaterialfv", ExactSpelling = true)]
			internal extern static unsafe void Materialfv(System.Graphics.ES11.All face, System.Graphics.ES11.All pname, Single* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glMaterialx", ExactSpelling = true)]
			internal extern static void Materialx(System.Graphics.ES11.All face, System.Graphics.ES11.All pname, int param);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glMaterialxOES", ExactSpelling = true)]
			internal extern static void MaterialxOES(System.Graphics.ES11.All face, System.Graphics.ES11.All pname, int param);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glMaterialxv", ExactSpelling = true)]
			internal extern static unsafe void Materialxv(System.Graphics.ES11.All face, System.Graphics.ES11.All pname, int* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glMaterialxvOES", ExactSpelling = true)]
			internal extern static unsafe void MaterialxvOES(System.Graphics.ES11.All face, System.Graphics.ES11.All pname, int* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glMatrixIndexPointerOES", ExactSpelling = true)]
			internal extern static void MatrixIndexPointerOES(Int32 size, System.Graphics.ES11.All type, Int32 stride, IntPtr pointer);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glMatrixMode", ExactSpelling = true)]
			internal extern static void MatrixMode(System.Graphics.ES11.All mode);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glMultiTexCoord4f", ExactSpelling = true)]
			internal extern static void MultiTexCoord4f(System.Graphics.ES11.All target, Single s, Single t, Single r, Single q);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glMultiTexCoord4x", ExactSpelling = true)]
			internal extern static void MultiTexCoord4x(System.Graphics.ES11.All target, int s, int t, int r, int q);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glMultiTexCoord4xOES", ExactSpelling = true)]
			internal extern static void MultiTexCoord4xOES(System.Graphics.ES11.All target, int s, int t, int r, int q);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glMultMatrixf", ExactSpelling = true)]
			internal extern static unsafe void MultMatrixf(Single* m);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glMultMatrixx", ExactSpelling = true)]
			internal extern static unsafe void MultMatrixx(int* m);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glMultMatrixxOES", ExactSpelling = true)]
			internal extern static unsafe void MultMatrixxOES(int* m);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glNormal3f", ExactSpelling = true)]
			internal extern static void Normal3f(Single nx, Single ny, Single nz);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glNormal3x", ExactSpelling = true)]
			internal extern static void Normal3x(int nx, int ny, int nz);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glNormal3xOES", ExactSpelling = true)]
			internal extern static void Normal3xOES(int nx, int ny, int nz);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glNormalPointer", ExactSpelling = true)]
			internal extern static void NormalPointer(System.Graphics.ES11.All type, Int32 stride, IntPtr pointer);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glOrthof", ExactSpelling = true)]
			internal extern static void Orthof(Single left, Single right, Single bottom, Single top, Single zNear, Single zFar);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glOrthofOES", ExactSpelling = true)]
			internal extern static void OrthofOES(Single left, Single right, Single bottom, Single top, Single zNear, Single zFar);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glOrthox", ExactSpelling = true)]
			internal extern static void Orthox(int left, int right, int bottom, int top, int zNear, int zFar);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glOrthoxOES", ExactSpelling = true)]
			internal extern static void OrthoxOES(int left, int right, int bottom, int top, int zNear, int zFar);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glPixelStorei", ExactSpelling = true)]
			internal extern static void PixelStorei(System.Graphics.ES11.All pname, Int32 param);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glPointParameterf", ExactSpelling = true)]
			internal extern static void PointParameterf(System.Graphics.ES11.All pname, Single param);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glPointParameterfv", ExactSpelling = true)]
			internal extern static unsafe void PointParameterfv(System.Graphics.ES11.All pname, Single* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glPointParameterx", ExactSpelling = true)]
			internal extern static void PointParameterx(System.Graphics.ES11.All pname, int param);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glPointParameterxOES", ExactSpelling = true)]
			internal extern static void PointParameterxOES(System.Graphics.ES11.All pname, int param);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glPointParameterxv", ExactSpelling = true)]
			internal extern static unsafe void PointParameterxv(System.Graphics.ES11.All pname, int* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glPointParameterxvOES", ExactSpelling = true)]
			internal extern static unsafe void PointParameterxvOES(System.Graphics.ES11.All pname, int* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glPointSize", ExactSpelling = true)]
			internal extern static void PointSize(Single size);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glPointSizePointerOES", ExactSpelling = true)]
			internal extern static void PointSizePointerOES(System.Graphics.ES11.All type, Int32 stride, IntPtr pointer);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glPointSizex", ExactSpelling = true)]
			internal extern static void PointSizex(int size);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glPointSizexOES", ExactSpelling = true)]
			internal extern static void PointSizexOES(int size);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glPolygonOffset", ExactSpelling = true)]
			internal extern static void PolygonOffset(Single factor, Single units);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glPolygonOffsetx", ExactSpelling = true)]
			internal extern static void PolygonOffsetx(int factor, int units);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glPolygonOffsetxOES", ExactSpelling = true)]
			internal extern static void PolygonOffsetxOES(int factor, int units);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glPopMatrix", ExactSpelling = true)]
			internal extern static void PopMatrix();
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glPushMatrix", ExactSpelling = true)]
			internal extern static void PushMatrix();
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glQueryMatrixxOES", ExactSpelling = true)]
			internal extern static unsafe Int32 QueryMatrixxOES(int* mantissa, Int32* exponent);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glReadPixels", ExactSpelling = true)]
			internal extern static void ReadPixels(Int32 x, Int32 y, Int32 width, Int32 height, System.Graphics.ES11.All format, System.Graphics.ES11.All type, IntPtr pixels);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glRenderbufferStorageOES", ExactSpelling = true)]
			internal extern static void RenderbufferStorageOES(System.Graphics.ES11.All target, System.Graphics.ES11.All internalformat, Int32 width, Int32 height);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glRotatef", ExactSpelling = true)]
			internal extern static void Rotatef(Single angle, Single x, Single y, Single z);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glRotatex", ExactSpelling = true)]
			internal extern static void Rotatex(int angle, int x, int y, int z);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glRotatexOES", ExactSpelling = true)]
			internal extern static void RotatexOES(int angle, int x, int y, int z);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glSampleCoverage", ExactSpelling = true)]
			internal extern static void SampleCoverage(Single value, bool invert);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glSampleCoveragex", ExactSpelling = true)]
			internal extern static void SampleCoveragex(int value, bool invert);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glSampleCoveragexOES", ExactSpelling = true)]
			internal extern static void SampleCoveragexOES(int value, bool invert);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glScalef", ExactSpelling = true)]
			internal extern static void Scalef(Single x, Single y, Single z);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glScalex", ExactSpelling = true)]
			internal extern static void Scalex(int x, int y, int z);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glScalexOES", ExactSpelling = true)]
			internal extern static void ScalexOES(int x, int y, int z);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glScissor", ExactSpelling = true)]
			internal extern static void Scissor(Int32 x, Int32 y, Int32 width, Int32 height);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glSetFenceNV", ExactSpelling = true)]
			internal extern static void SetFenceNV(UInt32 fence, System.Graphics.ES11.All condition);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glShadeModel", ExactSpelling = true)]
			internal extern static void ShadeModel(System.Graphics.ES11.All mode);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glStencilFunc", ExactSpelling = true)]
			internal extern static void StencilFunc(System.Graphics.ES11.All func, Int32 @ref, UInt32 mask);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glStencilMask", ExactSpelling = true)]
			internal extern static void StencilMask(UInt32 mask);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glStencilOp", ExactSpelling = true)]
			internal extern static void StencilOp(System.Graphics.ES11.All fail, System.Graphics.ES11.All zfail, System.Graphics.ES11.All zpass);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glTestFenceNV", ExactSpelling = true)]
			internal extern static bool TestFenceNV(UInt32 fence);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glTexCoordPointer", ExactSpelling = true)]
			internal extern static void TexCoordPointer(Int32 size, System.Graphics.ES11.All type, Int32 stride, IntPtr pointer);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glTexEnvf", ExactSpelling = true)]
			internal extern static void TexEnvf(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, Single param);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glTexEnvfv", ExactSpelling = true)]
			internal extern static unsafe void TexEnvfv(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, Single* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glTexEnvi", ExactSpelling = true)]
			internal extern static void TexEnvi(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, Int32 param);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glTexEnviv", ExactSpelling = true)]
			internal extern static unsafe void TexEnviv(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, Int32* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glTexEnvx", ExactSpelling = true)]
			internal extern static void TexEnvx(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, int param);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glTexEnvxOES", ExactSpelling = true)]
			internal extern static void TexEnvxOES(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, int param);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glTexEnvxv", ExactSpelling = true)]
			internal extern static unsafe void TexEnvxv(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, int* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glTexEnvxvOES", ExactSpelling = true)]
			internal extern static unsafe void TexEnvxvOES(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, int* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glTexGenfOES", ExactSpelling = true)]
			internal extern static void TexGenfOES(System.Graphics.ES11.All coord, System.Graphics.ES11.All pname, Single param);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glTexGenfvOES", ExactSpelling = true)]
			internal extern static unsafe void TexGenfvOES(System.Graphics.ES11.All coord, System.Graphics.ES11.All pname, Single* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glTexGeniOES", ExactSpelling = true)]
			internal extern static void TexGeniOES(System.Graphics.ES11.All coord, System.Graphics.ES11.All pname, Int32 param);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glTexGenivOES", ExactSpelling = true)]
			internal extern static unsafe void TexGenivOES(System.Graphics.ES11.All coord, System.Graphics.ES11.All pname, Int32* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glTexGenxOES", ExactSpelling = true)]
			internal extern static void TexGenxOES(System.Graphics.ES11.All coord, System.Graphics.ES11.All pname, int param);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glTexGenxvOES", ExactSpelling = true)]
			internal extern static unsafe void TexGenxvOES(System.Graphics.ES11.All coord, System.Graphics.ES11.All pname, int* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glTexImage2D", ExactSpelling = true)]
			internal extern static void TexImage2D(System.Graphics.ES11.All target, Int32 level, Int32 internalformat, Int32 width, Int32 height, Int32 border, System.Graphics.ES11.All format, System.Graphics.ES11.All type, IntPtr pixels);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glTexParameterf", ExactSpelling = true)]
			internal extern static void TexParameterf(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, Single param);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glTexParameterfv", ExactSpelling = true)]
			internal extern static unsafe void TexParameterfv(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, Single* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glTexParameteri", ExactSpelling = true)]
			internal extern static void TexParameteri(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, Int32 param);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glTexParameteriv", ExactSpelling = true)]
			internal extern static unsafe void TexParameteriv(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, Int32* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glTexParameterx", ExactSpelling = true)]
			internal extern static void TexParameterx(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, int param);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glTexParameterxOES", ExactSpelling = true)]
			internal extern static void TexParameterxOES(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, int param);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glTexParameterxv", ExactSpelling = true)]
			internal extern static unsafe void TexParameterxv(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, int* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glTexParameterxvOES", ExactSpelling = true)]
			internal extern static unsafe void TexParameterxvOES(System.Graphics.ES11.All target, System.Graphics.ES11.All pname, int* @params);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glTexSubImage2D", ExactSpelling = true)]
			internal extern static void TexSubImage2D(System.Graphics.ES11.All target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 width, Int32 height, System.Graphics.ES11.All format, System.Graphics.ES11.All type, IntPtr pixels);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glTranslatef", ExactSpelling = true)]
			internal extern static void Translatef(Single x, Single y, Single z);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glTranslatex", ExactSpelling = true)]
			internal extern static void Translatex(int x, int y, int z);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glTranslatexOES", ExactSpelling = true)]
			internal extern static void TranslatexOES(int x, int y, int z);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glUnmapBufferOES", ExactSpelling = true)]
			internal extern static bool UnmapBufferOES(System.Graphics.ES11.All target);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glVertexPointer", ExactSpelling = true)]
			internal extern static void VertexPointer(Int32 size, System.Graphics.ES11.All type, Int32 stride, IntPtr pointer);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glViewport", ExactSpelling = true)]
			internal extern static void Viewport(Int32 x, Int32 y, Int32 width, Int32 height);
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[System.Runtime.InteropServices.DllImport(GL.Library, EntryPoint = "glWeightPointerOES", ExactSpelling = true)]
			internal extern static void WeightPointerOES(Int32 size, System.Graphics.ES11.All type, Int32 stride, IntPtr pointer);
			*/
		}
	}
}