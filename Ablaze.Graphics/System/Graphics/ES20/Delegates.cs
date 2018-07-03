//Ported from OpenTK, and excellent library.

#pragma warning disable 0649
#pragma warning disable 1591

using System.Platforms;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace System.Graphics.ES20 {
	partial class GL {
		/// <summary>
		/// Lists all the available delegates (null if not available).
		/// </summary>
		[System.Security.SuppressUnmanagedCodeSecurity()]
		public static partial class Delegates {
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
			public delegate void ActiveTexture(System.Graphics.ES20.TextureUnit texture);
			public static ActiveTexture glActiveTexture;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void AttachShader(UInt32 program, UInt32 shader);
			[CLSCompliant(false)]
			public static AttachShader glAttachShader;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BeginPerfMonitorAMD(UInt32 monitor);
			[CLSCompliant(false)]
			public static BeginPerfMonitorAMD glBeginPerfMonitorAMD;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindAttribLocation(UInt32 program, UInt32 index, String name);
			[CLSCompliant(false)]
			public static BindAttribLocation glBindAttribLocation;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindBuffer(System.Graphics.ES20.BufferTarget target, UInt32 buffer);
			[CLSCompliant(false)]
			public static BindBuffer glBindBuffer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindFramebuffer(System.Graphics.ES20.FramebufferTarget target, UInt32 framebuffer);
			[CLSCompliant(false)]
			public static BindFramebuffer glBindFramebuffer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindRenderbuffer(System.Graphics.ES20.RenderbufferTarget target, UInt32 renderbuffer);
			[CLSCompliant(false)]
			public static BindRenderbuffer glBindRenderbuffer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindTexture(System.Graphics.ES20.TextureTarget target, UInt32 texture);
			[CLSCompliant(false)]
			public static BindTexture glBindTexture;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindVertexArrayOES(UInt32 array);
			[CLSCompliant(false)]
			public static BindVertexArrayOES glBindVertexArrayOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void BlendColor(Single red, Single green, Single blue, Single alpha);
			public static BlendColor glBlendColor;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void BlendEquation(System.Graphics.ES20.BlendEquationMode mode);
			public static BlendEquation glBlendEquation;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void BlendEquationSeparate(System.Graphics.ES20.BlendEquationMode modeRGB, System.Graphics.ES20.BlendEquationMode modeAlpha);
			public static BlendEquationSeparate glBlendEquationSeparate;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void BlendFunc(System.Graphics.ES20.BlendingFactorSrc sfactor, System.Graphics.ES20.BlendingFactorDest dfactor);
			public static BlendFunc glBlendFunc;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void BlendFuncSeparate(System.Graphics.ES20.BlendingFactorSrc srcRGB, System.Graphics.ES20.BlendingFactorDest dstRGB, System.Graphics.ES20.BlendingFactorSrc srcAlpha, System.Graphics.ES20.BlendingFactorDest dstAlpha);
			public static BlendFuncSeparate glBlendFuncSeparate;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BlitFramebufferANGLE(Int32 srcX0, Int32 srcY0, Int32 srcX1, Int32 srcY1, Int32 dstX0, Int32 dstY0, Int32 dstX1, Int32 dstY1, UInt32 mask, System.Graphics.ES20.All filter);
			[CLSCompliant(false)]
			public static BlitFramebufferANGLE glBlitFramebufferANGLE;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void BufferData(System.Graphics.ES20.BufferTarget target, IntPtr size, IntPtr data, System.Graphics.ES20.BufferUsage usage);
			public static BufferData glBufferData;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void BufferSubData(System.Graphics.ES20.BufferTarget target, IntPtr offset, IntPtr size, IntPtr data);
			public static BufferSubData glBufferSubData;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate System.Graphics.ES20.FramebufferErrorCode CheckFramebufferStatus(System.Graphics.ES20.FramebufferTarget target);
			public static CheckFramebufferStatus glCheckFramebufferStatus;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Clear(System.Graphics.ES20.ClearBufferMask mask);
			public static Clear glClear;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void ClearColor(Single red, Single green, Single blue, Single alpha);
			public static ClearColor glClearColor;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void ClearDepthf(Single depth);
			public static ClearDepthf glClearDepthf;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void ClearStencil(Int32 s);
			public static ClearStencil glClearStencil;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void ColorMask(bool red, bool green, bool blue, bool alpha);
			public static ColorMask glColorMask;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CompileShader(UInt32 shader);
			[CLSCompliant(false)]
			public static CompileShader glCompileShader;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void CompressedTexImage2D(System.Graphics.ES20.TextureTarget target, Int32 level, System.Graphics.ES20.PixelInternalFormat internalformat, Int32 width, Int32 height, Int32 border, Int32 imageSize, IntPtr data);
			public static CompressedTexImage2D glCompressedTexImage2D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void CompressedTexImage3DOES(System.Graphics.ES20.All target, Int32 level, System.Graphics.ES20.All internalformat, Int32 width, Int32 height, Int32 depth, Int32 border, Int32 imageSize, IntPtr data);
			public static CompressedTexImage3DOES glCompressedTexImage3DOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void CompressedTexSubImage2D(System.Graphics.ES20.TextureTarget target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 width, Int32 height, System.Graphics.ES20.PixelFormat format, Int32 imageSize, IntPtr data);
			public static CompressedTexSubImage2D glCompressedTexSubImage2D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void CompressedTexSubImage3DOES(System.Graphics.ES20.All target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 width, Int32 height, Int32 depth, System.Graphics.ES20.All format, Int32 imageSize, IntPtr data);
			public static CompressedTexSubImage3DOES glCompressedTexSubImage3DOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void CopyTexImage2D(System.Graphics.ES20.TextureTarget target, Int32 level, System.Graphics.ES20.PixelInternalFormat internalformat, Int32 x, Int32 y, Int32 width, Int32 height, Int32 border);
			public static CopyTexImage2D glCopyTexImage2D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void CopyTexSubImage2D(System.Graphics.ES20.TextureTarget target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 x, Int32 y, Int32 width, Int32 height);
			public static CopyTexSubImage2D glCopyTexSubImage2D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void CopyTexSubImage3DOES(System.Graphics.ES20.All target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 x, Int32 y, Int32 width, Int32 height);
			public static CopyTexSubImage3DOES glCopyTexSubImage3DOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void CoverageMaskNV(bool mask);
			public static CoverageMaskNV glCoverageMaskNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void CoverageOperationNV(System.Graphics.ES20.All operation);
			public static CoverageOperationNV glCoverageOperationNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate Int32 CreateProgram();
			public static CreateProgram glCreateProgram;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate Int32 CreateShader(System.Graphics.ES20.ShaderType type);
			public static CreateShader glCreateShader;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void CullFace(System.Graphics.ES20.CullFaceMode mode);
			public static CullFace glCullFace;
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
			public unsafe delegate void DeleteFramebuffers(Int32 n, UInt32* framebuffers);
			[CLSCompliant(false)]
			public unsafe static DeleteFramebuffers glDeleteFramebuffers;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DeletePerfMonitorsAMD(Int32 n, UInt32* monitors);
			[CLSCompliant(false)]
			public unsafe static DeletePerfMonitorsAMD glDeletePerfMonitorsAMD;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DeleteProgram(UInt32 program);
			[CLSCompliant(false)]
			public static DeleteProgram glDeleteProgram;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DeleteRenderbuffers(Int32 n, UInt32* renderbuffers);
			[CLSCompliant(false)]
			public unsafe static DeleteRenderbuffers glDeleteRenderbuffers;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DeleteShader(UInt32 shader);
			[CLSCompliant(false)]
			public static DeleteShader glDeleteShader;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DeleteTextures(Int32 n, UInt32* textures);
			[CLSCompliant(false)]
			public unsafe static DeleteTextures glDeleteTextures;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DeleteVertexArraysOES(Int32 n, UInt32* arrays);
			[CLSCompliant(false)]
			public unsafe static DeleteVertexArraysOES glDeleteVertexArraysOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void DepthFunc(System.Graphics.ES20.DepthFunction func);
			public static DepthFunc glDepthFunc;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void DepthMask(bool flag);
			public static DepthMask glDepthMask;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void DepthRangef(Single zNear, Single zFar);
			public static DepthRangef glDepthRangef;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DetachShader(UInt32 program, UInt32 shader);
			[CLSCompliant(false)]
			public static DetachShader glDetachShader;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Disable(System.Graphics.ES20.EnableCap cap);
			public static Disable glDisable;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DisableDriverControlQCOM(UInt32 driverControl);
			[CLSCompliant(false)]
			public static DisableDriverControlQCOM glDisableDriverControlQCOM;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DisableVertexAttribArray(UInt32 index);
			[CLSCompliant(false)]
			public static DisableVertexAttribArray glDisableVertexAttribArray;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DiscardFramebufferEXT(System.Graphics.ES20.All target, Int32 numAttachments, System.Graphics.ES20.All* attachments);
			[CLSCompliant(false)]
			public unsafe static DiscardFramebufferEXT glDiscardFramebufferEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void DrawArrays(System.Graphics.ES20.BeginMode mode, Int32 first, Int32 count);
			public static DrawArrays glDrawArrays;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void DrawElements(System.Graphics.ES20.BeginMode mode, Int32 count, System.Graphics.ES20.DrawElementsType type, IntPtr indices);
			public static DrawElements glDrawElements;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void EGLImageTargetRenderbufferStorageOES(System.Graphics.ES20.All target, IntPtr image);
			public static EGLImageTargetRenderbufferStorageOES glEGLImageTargetRenderbufferStorageOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void EGLImageTargetTexture2DOES(System.Graphics.ES20.All target, IntPtr image);
			public static EGLImageTargetTexture2DOES glEGLImageTargetTexture2DOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Enable(System.Graphics.ES20.EnableCap cap);
			public static Enable glEnable;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void EnableDriverControlQCOM(UInt32 driverControl);
			[CLSCompliant(false)]
			public static EnableDriverControlQCOM glEnableDriverControlQCOM;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void EnableVertexAttribArray(UInt32 index);
			[CLSCompliant(false)]
			public static EnableVertexAttribArray glEnableVertexAttribArray;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void EndPerfMonitorAMD(UInt32 monitor);
			[CLSCompliant(false)]
			public static EndPerfMonitorAMD glEndPerfMonitorAMD;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void EndTilingQCOM(UInt32 preserveMask);
			[CLSCompliant(false)]
			public static EndTilingQCOM glEndTilingQCOM;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void ExtGetBufferPointervQCOM(System.Graphics.ES20.All target, IntPtr @params);
			public static ExtGetBufferPointervQCOM glExtGetBufferPointervQCOM;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ExtGetBuffersQCOM(UInt32* buffers, Int32 maxBuffers, Int32* numBuffers);
			[CLSCompliant(false)]
			public unsafe static ExtGetBuffersQCOM glExtGetBuffersQCOM;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ExtGetFramebuffersQCOM(UInt32* framebuffers, Int32 maxFramebuffers, Int32* numFramebuffers);
			[CLSCompliant(false)]
			public unsafe static ExtGetFramebuffersQCOM glExtGetFramebuffersQCOM;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ExtGetProgramBinarySourceQCOM(UInt32 program, System.Graphics.ES20.All shadertype, String source, Int32* length);
			[CLSCompliant(false)]
			public unsafe static ExtGetProgramBinarySourceQCOM glExtGetProgramBinarySourceQCOM;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ExtGetProgramsQCOM(UInt32* programs, Int32 maxPrograms, Int32* numPrograms);
			[CLSCompliant(false)]
			public unsafe static ExtGetProgramsQCOM glExtGetProgramsQCOM;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ExtGetRenderbuffersQCOM(UInt32* renderbuffers, Int32 maxRenderbuffers, Int32* numRenderbuffers);
			[CLSCompliant(false)]
			public unsafe static ExtGetRenderbuffersQCOM glExtGetRenderbuffersQCOM;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ExtGetShadersQCOM(UInt32* shaders, Int32 maxShaders, Int32* numShaders);
			[CLSCompliant(false)]
			public unsafe static ExtGetShadersQCOM glExtGetShadersQCOM;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ExtGetTexLevelParameterivQCOM(UInt32 texture, System.Graphics.ES20.All face, Int32 level, System.Graphics.ES20.All pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static ExtGetTexLevelParameterivQCOM glExtGetTexLevelParameterivQCOM;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void ExtGetTexSubImageQCOM(System.Graphics.ES20.All target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 width, Int32 height, Int32 depth, System.Graphics.ES20.All format, System.Graphics.ES20.All type, IntPtr texels);
			public static ExtGetTexSubImageQCOM glExtGetTexSubImageQCOM;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ExtGetTexturesQCOM(UInt32* textures, Int32 maxTextures, Int32* numTextures);
			[CLSCompliant(false)]
			public unsafe static ExtGetTexturesQCOM glExtGetTexturesQCOM;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool ExtIsProgramBinaryQCOM(UInt32 program);
			[CLSCompliant(false)]
			public static ExtIsProgramBinaryQCOM glExtIsProgramBinaryQCOM;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void ExtTexObjectStateOverrideiQCOM(System.Graphics.ES20.All target, System.Graphics.ES20.All pname, Int32 param);
			public static ExtTexObjectStateOverrideiQCOM glExtTexObjectStateOverrideiQCOM;
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
			[CLSCompliant(false)]
			public delegate void FramebufferRenderbuffer(System.Graphics.ES20.FramebufferTarget target, System.Graphics.ES20.FramebufferSlot attachment, System.Graphics.ES20.RenderbufferTarget renderbuffertarget, UInt32 renderbuffer);
			[CLSCompliant(false)]
			public static FramebufferRenderbuffer glFramebufferRenderbuffer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FramebufferTexture2D(System.Graphics.ES20.FramebufferTarget target, System.Graphics.ES20.FramebufferSlot attachment, System.Graphics.ES20.TextureTarget textarget, UInt32 texture, Int32 level);
			[CLSCompliant(false)]
			public static FramebufferTexture2D glFramebufferTexture2D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void FramebufferTexture2DMultisampleIMG();
			public static FramebufferTexture2DMultisampleIMG glFramebufferTexture2DMultisampleIMG;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FramebufferTexture3DOES(System.Graphics.ES20.All target, System.Graphics.ES20.All attachment, System.Graphics.ES20.All textarget, UInt32 texture, Int32 level, Int32 zoffset);
			[CLSCompliant(false)]
			public static FramebufferTexture3DOES glFramebufferTexture3DOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void FrontFace(System.Graphics.ES20.FrontFaceDirection mode);
			public static FrontFace glFrontFace;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GenBuffers(Int32 n, [OutAttribute] UInt32* buffers);
			[CLSCompliant(false)]
			public unsafe static GenBuffers glGenBuffers;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void GenerateMipmap(System.Graphics.ES20.TextureTarget target);
			public static GenerateMipmap glGenerateMipmap;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GenFencesNV(Int32 n, [OutAttribute] UInt32* fences);
			[CLSCompliant(false)]
			public unsafe static GenFencesNV glGenFencesNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GenFramebuffers(Int32 n, [OutAttribute] UInt32* framebuffers);
			[CLSCompliant(false)]
			public unsafe static GenFramebuffers glGenFramebuffers;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GenPerfMonitorsAMD(Int32 n, [OutAttribute] UInt32* monitors);
			[CLSCompliant(false)]
			public unsafe static GenPerfMonitorsAMD glGenPerfMonitorsAMD;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GenRenderbuffers(Int32 n, [OutAttribute] UInt32* renderbuffers);
			[CLSCompliant(false)]
			public unsafe static GenRenderbuffers glGenRenderbuffers;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GenTextures(Int32 n, [OutAttribute] UInt32* textures);
			[CLSCompliant(false)]
			public unsafe static GenTextures glGenTextures;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GenVertexArraysOES(Int32 n, [OutAttribute] UInt32* arrays);
			[CLSCompliant(false)]
			public unsafe static GenVertexArraysOES glGenVertexArraysOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetActiveAttrib(UInt32 program, UInt32 index, Int32 bufsize, [OutAttribute] Int32* length, [OutAttribute] Int32* size, [OutAttribute] System.Graphics.ES20.ActiveAttribType* type, [OutAttribute] StringBuilder name);
			[CLSCompliant(false)]
			public unsafe static GetActiveAttrib glGetActiveAttrib;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetActiveUniform(UInt32 program, UInt32 index, Int32 bufsize, [OutAttribute] Int32* length, [OutAttribute] Int32* size, [OutAttribute] System.Graphics.ES20.ActiveUniformType* type, [OutAttribute] StringBuilder name);
			[CLSCompliant(false)]
			public unsafe static GetActiveUniform glGetActiveUniform;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetAttachedShaders(UInt32 program, Int32 maxcount, [OutAttribute] Int32* count, [OutAttribute] UInt32* shaders);
			[CLSCompliant(false)]
			public unsafe static GetAttachedShaders glGetAttachedShaders;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate int GetAttribLocation(UInt32 program, String name);
			[CLSCompliant(false)]
			public static GetAttribLocation glGetAttribLocation;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetBooleanv(System.Graphics.ES20.GetPName pname, [OutAttribute] bool* @params);
			[CLSCompliant(false)]
			public unsafe static GetBooleanv glGetBooleanv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetBufferParameteriv(System.Graphics.ES20.BufferTarget target, System.Graphics.ES20.BufferParameterName pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetBufferParameteriv glGetBufferParameteriv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void GetBufferPointervOES(System.Graphics.ES20.All target, System.Graphics.ES20.All pname, [OutAttribute] IntPtr @params);
			public static GetBufferPointervOES glGetBufferPointervOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetDriverControlsQCOM([OutAttribute] Int32* num, Int32 size, [OutAttribute] UInt32* driverControls);
			[CLSCompliant(false)]
			public unsafe static GetDriverControlsQCOM glGetDriverControlsQCOM;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetDriverControlStringQCOM(UInt32 driverControl, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] StringBuilder driverControlString);
			[CLSCompliant(false)]
			public unsafe static GetDriverControlStringQCOM glGetDriverControlStringQCOM;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate System.Graphics.ES20.ErrorCode GetError();
			public static GetError glGetError;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetFenceivNV(UInt32 fence, System.Graphics.ES20.All pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetFenceivNV glGetFenceivNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetFloatv(System.Graphics.ES20.GetPName pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetFloatv glGetFloatv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetFramebufferAttachmentParameteriv(System.Graphics.ES20.FramebufferTarget target, System.Graphics.ES20.FramebufferSlot attachment, System.Graphics.ES20.FramebufferParameterName pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetFramebufferAttachmentParameteriv glGetFramebufferAttachmentParameteriv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetIntegerv(System.Graphics.ES20.GetPName pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetIntegerv glGetIntegerv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetPerfMonitorCounterDataAMD(UInt32 monitor, System.Graphics.ES20.All pname, Int32 dataSize, [OutAttribute] UInt32* data, [OutAttribute] Int32* bytesWritten);
			[CLSCompliant(false)]
			public unsafe static GetPerfMonitorCounterDataAMD glGetPerfMonitorCounterDataAMD;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetPerfMonitorCounterInfoAMD(UInt32 group, UInt32 counter, System.Graphics.ES20.All pname, [OutAttribute] IntPtr data);
			[CLSCompliant(false)]
			public static GetPerfMonitorCounterInfoAMD glGetPerfMonitorCounterInfoAMD;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetPerfMonitorCountersAMD(UInt32 group, [OutAttribute] Int32* numCounters, [OutAttribute] Int32* maxActiveCounters, Int32 counterSize, [OutAttribute] UInt32* counters);
			[CLSCompliant(false)]
			public unsafe static GetPerfMonitorCountersAMD glGetPerfMonitorCountersAMD;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetPerfMonitorCounterStringAMD(UInt32 group, UInt32 counter, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] StringBuilder counterString);
			[CLSCompliant(false)]
			public unsafe static GetPerfMonitorCounterStringAMD glGetPerfMonitorCounterStringAMD;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetPerfMonitorGroupsAMD([OutAttribute] Int32* numGroups, Int32 groupsSize, [OutAttribute] UInt32* groups);
			[CLSCompliant(false)]
			public unsafe static GetPerfMonitorGroupsAMD glGetPerfMonitorGroupsAMD;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetPerfMonitorGroupStringAMD(UInt32 group, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] StringBuilder groupString);
			[CLSCompliant(false)]
			public unsafe static GetPerfMonitorGroupStringAMD glGetPerfMonitorGroupStringAMD;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetProgramBinaryOES(UInt32 program, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] System.Graphics.ES20.All* binaryFormat, [OutAttribute] IntPtr binary);
			[CLSCompliant(false)]
			public unsafe static GetProgramBinaryOES glGetProgramBinaryOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetProgramInfoLog(UInt32 program, Int32 bufsize, [OutAttribute] Int32* length, [OutAttribute] StringBuilder infolog);
			[CLSCompliant(false)]
			public unsafe static GetProgramInfoLog glGetProgramInfoLog;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetProgramiv(UInt32 program, System.Graphics.ES20.ProgramParameter pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetProgramiv glGetProgramiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetRenderbufferParameteriv(System.Graphics.ES20.RenderbufferTarget target, System.Graphics.ES20.RenderbufferParameterName pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetRenderbufferParameteriv glGetRenderbufferParameteriv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetShaderInfoLog(UInt32 shader, Int32 bufsize, [OutAttribute] Int32* length, [OutAttribute] StringBuilder infolog);
			[CLSCompliant(false)]
			public unsafe static GetShaderInfoLog glGetShaderInfoLog;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetShaderiv(UInt32 shader, System.Graphics.ES20.ShaderParameter pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetShaderiv glGetShaderiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetShaderPrecisionFormat(System.Graphics.ES20.ShaderType shadertype, System.Graphics.ES20.ShaderPrecision precisiontype, [OutAttribute] Int32* range, [OutAttribute] Int32* precision);
			[CLSCompliant(false)]
			public unsafe static GetShaderPrecisionFormat glGetShaderPrecisionFormat;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetShaderSource(UInt32 shader, Int32 bufsize, [OutAttribute] Int32* length, [OutAttribute] StringBuilder source);
			[CLSCompliant(false)]
			public unsafe static GetShaderSource glGetShaderSource;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public unsafe delegate System.IntPtr GetString(System.Graphics.ES20.StringName name);
			public unsafe static GetString glGetString;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetTexParameterfv(System.Graphics.ES20.TextureTarget target, System.Graphics.ES20.GetTextureParameter pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetTexParameterfv glGetTexParameterfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetTexParameteriv(System.Graphics.ES20.TextureTarget target, System.Graphics.ES20.GetTextureParameter pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetTexParameteriv glGetTexParameteriv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetUniformfv(UInt32 program, Int32 location, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetUniformfv glGetUniformfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetUniformiv(UInt32 program, Int32 location, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetUniformiv glGetUniformiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate int GetUniformLocation(UInt32 program, String name);
			[CLSCompliant(false)]
			public static GetUniformLocation glGetUniformLocation;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetVertexAttribfv(UInt32 index, System.Graphics.ES20.VertexAttribParameter pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetVertexAttribfv glGetVertexAttribfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetVertexAttribiv(UInt32 index, System.Graphics.ES20.VertexAttribParameter pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetVertexAttribiv glGetVertexAttribiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetVertexAttribPointerv(UInt32 index, System.Graphics.ES20.VertexAttribPointerParameter pname, [OutAttribute] IntPtr pointer);
			[CLSCompliant(false)]
			public static GetVertexAttribPointerv glGetVertexAttribPointerv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Hint(System.Graphics.ES20.HintTarget target, System.Graphics.ES20.HintMode mode);
			public static Hint glHint;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsBuffer(UInt32 buffer);
			[CLSCompliant(false)]
			public static IsBuffer glIsBuffer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate bool IsEnabled(System.Graphics.ES20.EnableCap cap);
			public static IsEnabled glIsEnabled;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsFenceNV(UInt32 fence);
			[CLSCompliant(false)]
			public static IsFenceNV glIsFenceNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsFramebuffer(UInt32 framebuffer);
			[CLSCompliant(false)]
			public static IsFramebuffer glIsFramebuffer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsProgram(UInt32 program);
			[CLSCompliant(false)]
			public static IsProgram glIsProgram;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsRenderbuffer(UInt32 renderbuffer);
			[CLSCompliant(false)]
			public static IsRenderbuffer glIsRenderbuffer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsShader(UInt32 shader);
			[CLSCompliant(false)]
			public static IsShader glIsShader;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsTexture(UInt32 texture);
			[CLSCompliant(false)]
			public static IsTexture glIsTexture;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsVertexArrayOES(UInt32 array);
			[CLSCompliant(false)]
			public static IsVertexArrayOES glIsVertexArrayOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void LineWidth(Single width);
			public static LineWidth glLineWidth;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void LinkProgram(UInt32 program);
			[CLSCompliant(false)]
			public static LinkProgram glLinkProgram;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public unsafe delegate System.IntPtr MapBufferOES(System.Graphics.ES20.All target, System.Graphics.ES20.All access);
			public unsafe static MapBufferOES glMapBufferOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiDrawArraysEXT(System.Graphics.ES20.All mode, Int32* first, Int32* count, Int32 primcount);
			[CLSCompliant(false)]
			public unsafe static MultiDrawArraysEXT glMultiDrawArraysEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiDrawElementsEXT(System.Graphics.ES20.All mode, Int32* first, System.Graphics.ES20.All type, IntPtr indices, Int32 primcount);
			[CLSCompliant(false)]
			public unsafe static MultiDrawElementsEXT glMultiDrawElementsEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void PixelStorei(System.Graphics.ES20.PixelStoreParameter pname, Int32 param);
			public static PixelStorei glPixelStorei;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void PolygonOffset(Single factor, Single units);
			public static PolygonOffset glPolygonOffset;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramBinaryOES(UInt32 program, System.Graphics.ES20.All binaryFormat, IntPtr binary, Int32 length);
			[CLSCompliant(false)]
			public static ProgramBinaryOES glProgramBinaryOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void ReadPixels(Int32 x, Int32 y, Int32 width, Int32 height, System.Graphics.ES20.PixelFormat format, System.Graphics.ES20.PixelType type, IntPtr pixels);
			public static ReadPixels glReadPixels;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void ReleaseShaderCompiler();
			public static ReleaseShaderCompiler glReleaseShaderCompiler;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void RenderbufferStorage(System.Graphics.ES20.RenderbufferTarget target, System.Graphics.ES20.RenderbufferInternalFormat internalformat, Int32 width, Int32 height);
			public static RenderbufferStorage glRenderbufferStorage;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void RenderbufferStorageMultisampleANGLE(System.Graphics.ES20.All target, Int32 samples, System.Graphics.ES20.All internalformat, Int32 width, Int32 height);
			public static RenderbufferStorageMultisampleANGLE glRenderbufferStorageMultisampleANGLE;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void RenderbufferStorageMultisampleAPPLE();
			public static RenderbufferStorageMultisampleAPPLE glRenderbufferStorageMultisampleAPPLE;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void RenderbufferStorageMultisampleIMG();
			public static RenderbufferStorageMultisampleIMG glRenderbufferStorageMultisampleIMG;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void ResolveMultisampleFramebufferAPPLE();
			public static ResolveMultisampleFramebufferAPPLE glResolveMultisampleFramebufferAPPLE;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void SampleCoverage(Single value, bool invert);
			public static SampleCoverage glSampleCoverage;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Scissor(Int32 x, Int32 y, Int32 width, Int32 height);
			public static Scissor glScissor;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void SelectPerfMonitorCountersAMD(UInt32 monitor, bool enable, UInt32 group, Int32 numCounters, UInt32* countersList);
			[CLSCompliant(false)]
			public unsafe static SelectPerfMonitorCountersAMD glSelectPerfMonitorCountersAMD;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SetFenceNV(UInt32 fence, System.Graphics.ES20.All condition);
			[CLSCompliant(false)]
			public static SetFenceNV glSetFenceNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ShaderBinary(Int32 n, UInt32* shaders, System.Graphics.ES20.ShaderBinaryFormat binaryformat, IntPtr binary, Int32 length);
			[CLSCompliant(false)]
			public unsafe static ShaderBinary glShaderBinary;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ShaderSource(UInt32 shader, Int32 count, String[] @string, Int32* length);
			[CLSCompliant(false)]
			public unsafe static ShaderSource glShaderSource;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void StartTilingQCOM(UInt32 x, UInt32 y, UInt32 width, UInt32 height, UInt32 preserveMask);
			[CLSCompliant(false)]
			public static StartTilingQCOM glStartTilingQCOM;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void StencilFunc(System.Graphics.ES20.StencilFunction func, Int32 @ref, UInt32 mask);
			[CLSCompliant(false)]
			public static StencilFunc glStencilFunc;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void StencilFuncSeparate(System.Graphics.ES20.CullFaceMode face, System.Graphics.ES20.StencilFunction func, Int32 @ref, UInt32 mask);
			[CLSCompliant(false)]
			public static StencilFuncSeparate glStencilFuncSeparate;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void StencilMask(UInt32 mask);
			[CLSCompliant(false)]
			public static StencilMask glStencilMask;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void StencilMaskSeparate(System.Graphics.ES20.CullFaceMode face, UInt32 mask);
			[CLSCompliant(false)]
			public static StencilMaskSeparate glStencilMaskSeparate;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void StencilOp(System.Graphics.ES20.StencilOp fail, System.Graphics.ES20.StencilOp zfail, System.Graphics.ES20.StencilOp zpass);
			public static StencilOp glStencilOp;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void StencilOpSeparate(System.Graphics.ES20.CullFaceMode face, System.Graphics.ES20.StencilOp fail, System.Graphics.ES20.StencilOp zfail, System.Graphics.ES20.StencilOp zpass);
			public static StencilOpSeparate glStencilOpSeparate;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool TestFenceNV(UInt32 fence);
			[CLSCompliant(false)]
			public static TestFenceNV glTestFenceNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void TexImage2D(System.Graphics.ES20.TextureTarget target, Int32 level, System.Graphics.ES20.PixelInternalFormat internalformat, Int32 width, Int32 height, Int32 border, System.Graphics.ES20.PixelFormat format, System.Graphics.ES20.PixelType type, IntPtr pixels);
			public static TexImage2D glTexImage2D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void TexImage3DOES(System.Graphics.ES20.All target, Int32 level, System.Graphics.ES20.All internalformat, Int32 width, Int32 height, Int32 depth, Int32 border, System.Graphics.ES20.All format, System.Graphics.ES20.All type, IntPtr pixels);
			public static TexImage3DOES glTexImage3DOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void TexParameterf(System.Graphics.ES20.TextureTarget target, System.Graphics.ES20.TextureParameterName pname, Single param);
			public static TexParameterf glTexParameterf;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexParameterfv(System.Graphics.ES20.TextureTarget target, System.Graphics.ES20.TextureParameterName pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static TexParameterfv glTexParameterfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void TexParameteri(System.Graphics.ES20.TextureTarget target, System.Graphics.ES20.TextureParameterName pname, Int32 param);
			public static TexParameteri glTexParameteri;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexParameteriv(System.Graphics.ES20.TextureTarget target, System.Graphics.ES20.TextureParameterName pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static TexParameteriv glTexParameteriv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void TexSubImage2D(System.Graphics.ES20.TextureTarget target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 width, Int32 height, System.Graphics.ES20.PixelFormat format, System.Graphics.ES20.PixelType type, IntPtr pixels);
			public static TexSubImage2D glTexSubImage2D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void TexSubImage3DOES(System.Graphics.ES20.All target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 width, Int32 height, Int32 depth, System.Graphics.ES20.All format, System.Graphics.ES20.All type, IntPtr pixels);
			public static TexSubImage3DOES glTexSubImage3DOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Uniform1f(Int32 location, Single x);
			public static Uniform1f glUniform1f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform1fv(Int32 location, Int32 count, Single* v);
			[CLSCompliant(false)]
			public unsafe static Uniform1fv glUniform1fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Uniform1i(Int32 location, Int32 x);
			public static Uniform1i glUniform1i;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform1iv(Int32 location, Int32 count, Int32* v);
			[CLSCompliant(false)]
			public unsafe static Uniform1iv glUniform1iv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Uniform2f(Int32 location, Single x, Single y);
			public static Uniform2f glUniform2f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform2fv(Int32 location, Int32 count, Single* v);
			[CLSCompliant(false)]
			public unsafe static Uniform2fv glUniform2fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Uniform2i(Int32 location, Int32 x, Int32 y);
			public static Uniform2i glUniform2i;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform2iv(Int32 location, Int32 count, Int32* v);
			[CLSCompliant(false)]
			public unsafe static Uniform2iv glUniform2iv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Uniform3f(Int32 location, Single x, Single y, Single z);
			public static Uniform3f glUniform3f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform3fv(Int32 location, Int32 count, Single* v);
			[CLSCompliant(false)]
			public unsafe static Uniform3fv glUniform3fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Uniform3i(Int32 location, Int32 x, Int32 y, Int32 z);
			public static Uniform3i glUniform3i;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform3iv(Int32 location, Int32 count, Int32* v);
			[CLSCompliant(false)]
			public unsafe static Uniform3iv glUniform3iv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Uniform4f(Int32 location, Single x, Single y, Single z, Single w);
			public static Uniform4f glUniform4f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform4fv(Int32 location, Int32 count, Single* v);
			[CLSCompliant(false)]
			public unsafe static Uniform4fv glUniform4fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Uniform4i(Int32 location, Int32 x, Int32 y, Int32 z, Int32 w);
			public static Uniform4i glUniform4i;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform4iv(Int32 location, Int32 count, Int32* v);
			[CLSCompliant(false)]
			public unsafe static Uniform4iv glUniform4iv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void UniformMatrix2fv(Int32 location, Int32 count, bool transpose, Single* value);
			[CLSCompliant(false)]
			public unsafe static UniformMatrix2fv glUniformMatrix2fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void UniformMatrix3fv(Int32 location, Int32 count, bool transpose, Single* value);
			[CLSCompliant(false)]
			public unsafe static UniformMatrix3fv glUniformMatrix3fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void UniformMatrix4fv(Int32 location, Int32 count, bool transpose, Single* value);
			[CLSCompliant(false)]
			public unsafe static UniformMatrix4fv glUniformMatrix4fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate bool UnmapBufferOES(System.Graphics.ES20.All target);
			public static UnmapBufferOES glUnmapBufferOES;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void UseProgram(UInt32 program);
			[CLSCompliant(false)]
			public static UseProgram glUseProgram;
			[CLSCompliant(false)]
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void ValidateProgram(UInt32 program);
			[CLSCompliant(false)]
			public static ValidateProgram glValidateProgram;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib1f(UInt32 indx, Single x);
			[CLSCompliant(false)]
			public static VertexAttrib1f glVertexAttrib1f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib1fv(UInt32 indx, Single* values);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib1fv glVertexAttrib1fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib2f(UInt32 indx, Single x, Single y);
			[CLSCompliant(false)]
			public static VertexAttrib2f glVertexAttrib2f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib2fv(UInt32 indx, Single* values);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib2fv glVertexAttrib2fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib3f(UInt32 indx, Single x, Single y, Single z);
			[CLSCompliant(false)]
			public static VertexAttrib3f glVertexAttrib3f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib3fv(UInt32 indx, Single* values);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib3fv glVertexAttrib3fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib4f(UInt32 indx, Single x, Single y, Single z, Single w);
			[CLSCompliant(false)]
			public static VertexAttrib4f glVertexAttrib4f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib4fv(UInt32 indx, Single* values);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib4fv glVertexAttrib4fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribPointer(UInt32 indx, Int32 size, System.Graphics.ES20.VertexAttribPointerType type, bool normalized, Int32 stride, IntPtr ptr);
			[CLSCompliant(false)]
			public static VertexAttribPointer glVertexAttribPointer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Viewport(Int32 x, Int32 y, Int32 width, Int32 height);
			public static Viewport glViewport;
		}
	}
}

#pragma warning restore 0649
#pragma warning restore 1591