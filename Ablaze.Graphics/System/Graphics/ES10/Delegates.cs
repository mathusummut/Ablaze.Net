//Ported from OpenTK, and excellent library.

#pragma warning disable 0649
#pragma warning disable 1591

using System.Platforms;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace System.Graphics.ES10 {
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
			public delegate void ActiveTexture(System.Graphics.ES10.All texture);
			public static ActiveTexture glActiveTexture;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void AlphaFunc(System.Graphics.ES10.All func, Single @ref);
			public static AlphaFunc glAlphaFunc;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void AlphaFuncx(System.Graphics.ES10.All func, int @ref);
			public static AlphaFuncx glAlphaFuncx;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindTexture(System.Graphics.ES10.All target, UInt32 texture);
			[CLSCompliant(false)]
			public static BindTexture glBindTexture;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void BlendFunc(System.Graphics.ES10.All sfactor, System.Graphics.ES10.All dfactor);
			public static BlendFunc glBlendFunc;
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
			public delegate void ClearDepthf(Single depth);
			public static ClearDepthf glClearDepthf;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void ClearDepthx(int depth);
			public static ClearDepthx glClearDepthx;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void ClearStencil(Int32 s);
			public static ClearStencil glClearStencil;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void ClientActiveTexture(System.Graphics.ES10.All texture);
			public static ClientActiveTexture glClientActiveTexture;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Color4f(Single red, Single green, Single blue, Single alpha);
			public static Color4f glColor4f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Color4x(int red, int green, int blue, int alpha);
			public static Color4x glColor4x;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void ColorMask(bool red, bool green, bool blue, bool alpha);
			public static ColorMask glColorMask;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void ColorPointer(Int32 size, System.Graphics.ES10.All type, Int32 stride, IntPtr pointer);
			public static ColorPointer glColorPointer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void CompressedTexImage2D(System.Graphics.ES10.All target, Int32 level, System.Graphics.ES10.All internalformat, Int32 width, Int32 height, Int32 border, Int32 imageSize, IntPtr data);
			public static CompressedTexImage2D glCompressedTexImage2D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void CompressedTexSubImage2D(System.Graphics.ES10.All target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 width, Int32 height, System.Graphics.ES10.All format, Int32 imageSize, IntPtr data);
			public static CompressedTexSubImage2D glCompressedTexSubImage2D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void CopyTexImage2D(System.Graphics.ES10.All target, Int32 level, System.Graphics.ES10.All internalformat, Int32 x, Int32 y, Int32 width, Int32 height, Int32 border);
			public static CopyTexImage2D glCopyTexImage2D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void CopyTexSubImage2D(System.Graphics.ES10.All target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 x, Int32 y, Int32 width, Int32 height);
			public static CopyTexSubImage2D glCopyTexSubImage2D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void CullFace(System.Graphics.ES10.All mode);
			public static CullFace glCullFace;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DeleteTextures(Int32 n, UInt32* textures);
			[CLSCompliant(false)]
			public unsafe static DeleteTextures glDeleteTextures;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void DepthFunc(System.Graphics.ES10.All func);
			public static DepthFunc glDepthFunc;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void DepthMask(bool flag);
			public static DepthMask glDepthMask;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void DepthRangef(Single zNear, Single zFar);
			public static DepthRangef glDepthRangef;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void DepthRangex(int zNear, int zFar);
			public static DepthRangex glDepthRangex;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Disable(System.Graphics.ES10.All cap);
			public static Disable glDisable;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void DisableClientState(System.Graphics.ES10.All array);
			public static DisableClientState glDisableClientState;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void DrawArrays(System.Graphics.ES10.All mode, Int32 first, Int32 count);
			public static DrawArrays glDrawArrays;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void DrawElements(System.Graphics.ES10.All mode, Int32 count, System.Graphics.ES10.All type, IntPtr indices);
			public static DrawElements glDrawElements;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Enable(System.Graphics.ES10.All cap);
			public static Enable glEnable;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void EnableClientState(System.Graphics.ES10.All array);
			public static EnableClientState glEnableClientState;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Finish();
			public static Finish glFinish;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Flush();
			public static Flush glFlush;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Fogf(System.Graphics.ES10.All pname, Single param);
			public static Fogf glFogf;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Fogfv(System.Graphics.ES10.All pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static Fogfv glFogfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Fogx(System.Graphics.ES10.All pname, int param);
			public static Fogx glFogx;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Fogxv(System.Graphics.ES10.All pname, int* @params);
			[CLSCompliant(false)]
			public unsafe static Fogxv glFogxv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void FrontFace(System.Graphics.ES10.All mode);
			public static FrontFace glFrontFace;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Frustumf(Single left, Single right, Single bottom, Single top, Single zNear, Single zFar);
			public static Frustumf glFrustumf;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Frustumx(int left, int right, int bottom, int top, int zNear, int zFar);
			public static Frustumx glFrustumx;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GenTextures(Int32 n, UInt32* textures);
			[CLSCompliant(false)]
			public unsafe static GenTextures glGenTextures;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate System.Graphics.ES10.All GetError();
			public static GetError glGetError;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetIntegerv(System.Graphics.ES10.All pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetIntegerv glGetIntegerv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public unsafe delegate System.IntPtr GetString(System.Graphics.ES10.All name);
			public unsafe static GetString glGetString;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Hint(System.Graphics.ES10.All target, System.Graphics.ES10.All mode);
			public static Hint glHint;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Lightf(System.Graphics.ES10.All light, System.Graphics.ES10.All pname, Single param);
			public static Lightf glLightf;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Lightfv(System.Graphics.ES10.All light, System.Graphics.ES10.All pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static Lightfv glLightfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void LightModelf(System.Graphics.ES10.All pname, Single param);
			public static LightModelf glLightModelf;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void LightModelfv(System.Graphics.ES10.All pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static LightModelfv glLightModelfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void LightModelx(System.Graphics.ES10.All pname, int param);
			public static LightModelx glLightModelx;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void LightModelxv(System.Graphics.ES10.All pname, int* @params);
			[CLSCompliant(false)]
			public unsafe static LightModelxv glLightModelxv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Lightx(System.Graphics.ES10.All light, System.Graphics.ES10.All pname, int param);
			public static Lightx glLightx;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Lightxv(System.Graphics.ES10.All light, System.Graphics.ES10.All pname, int* @params);
			[CLSCompliant(false)]
			public unsafe static Lightxv glLightxv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void LineWidth(Single width);
			public static LineWidth glLineWidth;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void LineWidthx(int width);
			public static LineWidthx glLineWidthx;
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
			public delegate void LogicOp(System.Graphics.ES10.All opcode);
			public static LogicOp glLogicOp;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Materialf(System.Graphics.ES10.All face, System.Graphics.ES10.All pname, Single param);
			public static Materialf glMaterialf;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Materialfv(System.Graphics.ES10.All face, System.Graphics.ES10.All pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static Materialfv glMaterialfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Materialx(System.Graphics.ES10.All face, System.Graphics.ES10.All pname, int param);
			public static Materialx glMaterialx;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Materialxv(System.Graphics.ES10.All face, System.Graphics.ES10.All pname, int* @params);
			[CLSCompliant(false)]
			public unsafe static Materialxv glMaterialxv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void MatrixMode(System.Graphics.ES10.All mode);
			public static MatrixMode glMatrixMode;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void MultiTexCoord4f(System.Graphics.ES10.All target, Single s, Single t, Single r, Single q);
			public static MultiTexCoord4f glMultiTexCoord4f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void MultiTexCoord4x(System.Graphics.ES10.All target, int s, int t, int r, int q);
			public static MultiTexCoord4x glMultiTexCoord4x;
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
			public delegate void Normal3f(Single nx, Single ny, Single nz);
			public static Normal3f glNormal3f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Normal3x(int nx, int ny, int nz);
			public static Normal3x glNormal3x;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void NormalPointer(System.Graphics.ES10.All type, Int32 stride, IntPtr pointer);
			public static NormalPointer glNormalPointer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Orthof(Single left, Single right, Single bottom, Single top, Single zNear, Single zFar);
			public static Orthof glOrthof;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Orthox(int left, int right, int bottom, int top, int zNear, int zFar);
			public static Orthox glOrthox;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void PixelStorei(System.Graphics.ES10.All pname, Int32 param);
			public static PixelStorei glPixelStorei;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void PointSize(Single size);
			public static PointSize glPointSize;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void PointSizex(int size);
			public static PointSizex glPointSizex;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void PolygonOffset(Single factor, Single units);
			public static PolygonOffset glPolygonOffset;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void PolygonOffsetx(int factor, int units);
			public static PolygonOffsetx glPolygonOffsetx;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void PopMatrix();
			public static PopMatrix glPopMatrix;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void PushMatrix();
			public static PushMatrix glPushMatrix;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void ReadPixels(Int32 x, Int32 y, Int32 width, Int32 height, System.Graphics.ES10.All format, System.Graphics.ES10.All type, IntPtr pixels);
			public static ReadPixels glReadPixels;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Rotatef(Single angle, Single x, Single y, Single z);
			public static Rotatef glRotatef;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Rotatex(int angle, int x, int y, int z);
			public static Rotatex glRotatex;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void SampleCoverage(Single value, bool invert);
			public static SampleCoverage glSampleCoverage;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void SampleCoveragex(int value, bool invert);
			public static SampleCoveragex glSampleCoveragex;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Scalef(Single x, Single y, Single z);
			public static Scalef glScalef;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Scalex(int x, int y, int z);
			public static Scalex glScalex;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Scissor(Int32 x, Int32 y, Int32 width, Int32 height);
			public static Scissor glScissor;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void ShadeModel(System.Graphics.ES10.All mode);
			public static ShadeModel glShadeModel;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void StencilFunc(System.Graphics.ES10.All func, Int32 @ref, UInt32 mask);
			[CLSCompliant(false)]
			public static StencilFunc glStencilFunc;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void StencilMask(UInt32 mask);
			[CLSCompliant(false)]
			public static StencilMask glStencilMask;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void StencilOp(System.Graphics.ES10.All fail, System.Graphics.ES10.All zfail, System.Graphics.ES10.All zpass);
			public static StencilOp glStencilOp;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void TexCoordPointer(Int32 size, System.Graphics.ES10.All type, Int32 stride, IntPtr pointer);
			public static TexCoordPointer glTexCoordPointer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void TexEnvf(System.Graphics.ES10.All target, System.Graphics.ES10.All pname, Single param);
			public static TexEnvf glTexEnvf;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexEnvfv(System.Graphics.ES10.All target, System.Graphics.ES10.All pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static TexEnvfv glTexEnvfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void TexEnvx(System.Graphics.ES10.All target, System.Graphics.ES10.All pname, int param);
			public static TexEnvx glTexEnvx;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexEnvxv(System.Graphics.ES10.All target, System.Graphics.ES10.All pname, int* @params);
			[CLSCompliant(false)]
			public unsafe static TexEnvxv glTexEnvxv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void TexImage2D(System.Graphics.ES10.All target, Int32 level, Int32 internalformat, Int32 width, Int32 height, Int32 border, System.Graphics.ES10.All format, System.Graphics.ES10.All type, IntPtr pixels);
			public static TexImage2D glTexImage2D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void TexParameterf(System.Graphics.ES10.All target, System.Graphics.ES10.All pname, Single param);
			public static TexParameterf glTexParameterf;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void TexParameterx(System.Graphics.ES10.All target, System.Graphics.ES10.All pname, int param);
			public static TexParameterx glTexParameterx;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void TexSubImage2D(System.Graphics.ES10.All target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 width, Int32 height, System.Graphics.ES10.All format, System.Graphics.ES10.All type, IntPtr pixels);
			public static TexSubImage2D glTexSubImage2D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Translatef(Single x, Single y, Single z);
			public static Translatef glTranslatef;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Translatex(int x, int y, int z);
			public static Translatex glTranslatex;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void VertexPointer(Int32 size, System.Graphics.ES10.All type, Int32 stride, IntPtr pointer);
			public static VertexPointer glVertexPointer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			public delegate void Viewport(Int32 x, Int32 y, Int32 width, Int32 height);
			public static Viewport glViewport;
		}
	}
}

#pragma warning restore 0649
#pragma warning restore 1591