//Ported from OpenTK, and excellent library.

#pragma warning disable 0649
#pragma warning disable 1591

using System.Platforms;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace System.Graphics.OGL {
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
				const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly;
				MethodInfo method;
				IntPtr address;
				long addr;
				Func<string, IntPtr> GetAddress = new Func<string, IntPtr>(GraphicsPlatform.Factory.GetAddress);
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
			[CLSCompliant(false)]
			public delegate void Accum(System.Graphics.OGL.AccumOp op, Single value);
			[CLSCompliant(false)]
			public static Accum glAccum;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ActiveProgramEXT(UInt32 program);
			[CLSCompliant(false)]
			public static ActiveProgramEXT glActiveProgramEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ActiveShaderProgram(UInt32 pipeline, UInt32 program);
			[CLSCompliant(false)]
			public static ActiveShaderProgram glActiveShaderProgram;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ActiveStencilFaceEXT(System.Graphics.OGL.ExtStencilTwoSide face);
			[CLSCompliant(false)]
			public static ActiveStencilFaceEXT glActiveStencilFaceEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ActiveTexture(System.Graphics.OGL.TextureUnit texture);
			[CLSCompliant(false)]
			public static ActiveTexture glActiveTexture;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ActiveTextureARB(System.Graphics.OGL.TextureUnit texture);
			[CLSCompliant(false)]
			public static ActiveTextureARB glActiveTextureARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ActiveVaryingNV(UInt32 program, String name);
			[CLSCompliant(false)]
			public static ActiveVaryingNV glActiveVaryingNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void AlphaFragmentOp1ATI(System.Graphics.OGL.AtiFragmentShader op, UInt32 dst, UInt32 dstMod, UInt32 arg1, UInt32 arg1Rep, UInt32 arg1Mod);
			[CLSCompliant(false)]
			public static AlphaFragmentOp1ATI glAlphaFragmentOp1ATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void AlphaFragmentOp2ATI(System.Graphics.OGL.AtiFragmentShader op, UInt32 dst, UInt32 dstMod, UInt32 arg1, UInt32 arg1Rep, UInt32 arg1Mod, UInt32 arg2, UInt32 arg2Rep, UInt32 arg2Mod);
			[CLSCompliant(false)]
			public static AlphaFragmentOp2ATI glAlphaFragmentOp2ATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void AlphaFragmentOp3ATI(System.Graphics.OGL.AtiFragmentShader op, UInt32 dst, UInt32 dstMod, UInt32 arg1, UInt32 arg1Rep, UInt32 arg1Mod, UInt32 arg2, UInt32 arg2Rep, UInt32 arg2Mod, UInt32 arg3, UInt32 arg3Rep, UInt32 arg3Mod);
			[CLSCompliant(false)]
			public static AlphaFragmentOp3ATI glAlphaFragmentOp3ATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void AlphaFunc(System.Graphics.OGL.AlphaFunction func, Single @ref);
			[CLSCompliant(false)]
			public static AlphaFunc glAlphaFunc;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ApplyTextureEXT(System.Graphics.OGL.ExtLightTexture mode);
			[CLSCompliant(false)]
			public static ApplyTextureEXT glApplyTextureEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate bool AreProgramsResidentNV(Int32 n, UInt32* programs, [OutAttribute] bool* residences);
			[CLSCompliant(false)]
			public unsafe static AreProgramsResidentNV glAreProgramsResidentNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate bool AreTexturesResident(Int32 n, UInt32* textures, [OutAttribute] bool* residences);
			[CLSCompliant(false)]
			public unsafe static AreTexturesResident glAreTexturesResident;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate bool AreTexturesResidentEXT(Int32 n, UInt32* textures, [OutAttribute] bool* residences);
			[CLSCompliant(false)]
			public unsafe static AreTexturesResidentEXT glAreTexturesResidentEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ArrayElement(Int32 i);
			[CLSCompliant(false)]
			public static ArrayElement glArrayElement;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ArrayElementEXT(Int32 i);
			[CLSCompliant(false)]
			public static ArrayElementEXT glArrayElementEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ArrayObjectATI(System.Graphics.OGL.EnableCap array, Int32 size, System.Graphics.OGL.AtiVertexArrayObject type, Int32 stride, UInt32 buffer, UInt32 offset);
			[CLSCompliant(false)]
			public static ArrayObjectATI glArrayObjectATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void AsyncMarkerSGIX(UInt32 marker);
			[CLSCompliant(false)]
			public static AsyncMarkerSGIX glAsyncMarkerSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void AttachObjectARB(UInt32 containerObj, UInt32 obj);
			[CLSCompliant(false)]
			public static AttachObjectARB glAttachObjectARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void AttachShader(UInt32 program, UInt32 shader);
			[CLSCompliant(false)]
			public static AttachShader glAttachShader;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Begin(System.Graphics.OGL.BeginMode mode);
			[CLSCompliant(false)]
			public static Begin glBegin;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BeginConditionalRender(UInt32 id, System.Graphics.OGL.ConditionalRenderType mode);
			[CLSCompliant(false)]
			public static BeginConditionalRender glBeginConditionalRender;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BeginConditionalRenderNV(UInt32 id, System.Graphics.OGL.NvConditionalRender mode);
			[CLSCompliant(false)]
			public static BeginConditionalRenderNV glBeginConditionalRenderNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BeginFragmentShaderATI();
			[CLSCompliant(false)]
			public static BeginFragmentShaderATI glBeginFragmentShaderATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BeginOcclusionQueryNV(UInt32 id);
			[CLSCompliant(false)]
			public static BeginOcclusionQueryNV glBeginOcclusionQueryNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BeginPerfMonitorAMD(UInt32 monitor);
			[CLSCompliant(false)]
			public static BeginPerfMonitorAMD glBeginPerfMonitorAMD;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BeginQuery(System.Graphics.OGL.QueryTarget target, UInt32 id);
			[CLSCompliant(false)]
			public static BeginQuery glBeginQuery;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BeginQueryARB(System.Graphics.OGL.ArbOcclusionQuery target, UInt32 id);
			[CLSCompliant(false)]
			public static BeginQueryARB glBeginQueryARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BeginQueryIndexed(System.Graphics.OGL.QueryTarget target, UInt32 index, UInt32 id);
			[CLSCompliant(false)]
			public static BeginQueryIndexed glBeginQueryIndexed;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BeginTransformFeedback(System.Graphics.OGL.BeginFeedbackMode primitiveMode);
			[CLSCompliant(false)]
			public static BeginTransformFeedback glBeginTransformFeedback;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BeginTransformFeedbackEXT(System.Graphics.OGL.ExtTransformFeedback primitiveMode);
			[CLSCompliant(false)]
			public static BeginTransformFeedbackEXT glBeginTransformFeedbackEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BeginTransformFeedbackNV(System.Graphics.OGL.NvTransformFeedback primitiveMode);
			[CLSCompliant(false)]
			public static BeginTransformFeedbackNV glBeginTransformFeedbackNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BeginVertexShaderEXT();
			[CLSCompliant(false)]
			public static BeginVertexShaderEXT glBeginVertexShaderEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BeginVideoCaptureNV(UInt32 video_capture_slot);
			[CLSCompliant(false)]
			public static BeginVideoCaptureNV glBeginVideoCaptureNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindAttribLocation(UInt32 program, UInt32 index, String name);
			[CLSCompliant(false)]
			public static BindAttribLocation glBindAttribLocation;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindAttribLocationARB(UInt32 programObj, UInt32 index, String name);
			[CLSCompliant(false)]
			public static BindAttribLocationARB glBindAttribLocationARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindBuffer(System.Graphics.OGL.BufferTarget target, UInt32 buffer);
			[CLSCompliant(false)]
			public static BindBuffer glBindBuffer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindBufferARB(System.Graphics.OGL.BufferTargetArb target, UInt32 buffer);
			[CLSCompliant(false)]
			public static BindBufferARB glBindBufferARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindBufferBase(System.Graphics.OGL.BufferTarget target, UInt32 index, UInt32 buffer);
			[CLSCompliant(false)]
			public static BindBufferBase glBindBufferBase;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindBufferBaseEXT(System.Graphics.OGL.ExtTransformFeedback target, UInt32 index, UInt32 buffer);
			[CLSCompliant(false)]
			public static BindBufferBaseEXT glBindBufferBaseEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindBufferBaseNV(System.Graphics.OGL.NvTransformFeedback target, UInt32 index, UInt32 buffer);
			[CLSCompliant(false)]
			public static BindBufferBaseNV glBindBufferBaseNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindBufferOffsetEXT(System.Graphics.OGL.ExtTransformFeedback target, UInt32 index, UInt32 buffer, IntPtr offset);
			[CLSCompliant(false)]
			public static BindBufferOffsetEXT glBindBufferOffsetEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindBufferOffsetNV(System.Graphics.OGL.NvTransformFeedback target, UInt32 index, UInt32 buffer, IntPtr offset);
			[CLSCompliant(false)]
			public static BindBufferOffsetNV glBindBufferOffsetNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindBufferRange(System.Graphics.OGL.BufferTarget target, UInt32 index, UInt32 buffer, IntPtr offset, IntPtr size);
			[CLSCompliant(false)]
			public static BindBufferRange glBindBufferRange;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindBufferRangeEXT(System.Graphics.OGL.ExtTransformFeedback target, UInt32 index, UInt32 buffer, IntPtr offset, IntPtr size);
			[CLSCompliant(false)]
			public static BindBufferRangeEXT glBindBufferRangeEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindBufferRangeNV(System.Graphics.OGL.NvTransformFeedback target, UInt32 index, UInt32 buffer, IntPtr offset, IntPtr size);
			[CLSCompliant(false)]
			public static BindBufferRangeNV glBindBufferRangeNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindFragDataLocation(UInt32 program, UInt32 color, String name);
			[CLSCompliant(false)]
			public static BindFragDataLocation glBindFragDataLocation;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindFragDataLocationEXT(UInt32 program, UInt32 color, String name);
			[CLSCompliant(false)]
			public static BindFragDataLocationEXT glBindFragDataLocationEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindFragDataLocationIndexed(UInt32 program, UInt32 colorNumber, UInt32 index, String name);
			[CLSCompliant(false)]
			public static BindFragDataLocationIndexed glBindFragDataLocationIndexed;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindFragmentShaderATI(UInt32 id);
			[CLSCompliant(false)]
			public static BindFragmentShaderATI glBindFragmentShaderATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindFramebuffer(System.Graphics.OGL.FramebufferTarget target, UInt32 framebuffer);
			[CLSCompliant(false)]
			public static BindFramebuffer glBindFramebuffer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindFramebufferEXT(System.Graphics.OGL.FramebufferTarget target, UInt32 framebuffer);
			[CLSCompliant(false)]
			public static BindFramebufferEXT glBindFramebufferEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindImageTextureEXT(UInt32 index, UInt32 texture, Int32 level, bool layered, Int32 layer, System.Graphics.OGL.ExtShaderImageLoadStore access, Int32 format);
			[CLSCompliant(false)]
			public static BindImageTextureEXT glBindImageTextureEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Int32 BindLightParameterEXT(System.Graphics.OGL.LightName light, System.Graphics.OGL.LightParameter value);
			[CLSCompliant(false)]
			public static BindLightParameterEXT glBindLightParameterEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Int32 BindMaterialParameterEXT(System.Graphics.OGL.MaterialFace face, System.Graphics.OGL.MaterialParameter value);
			[CLSCompliant(false)]
			public static BindMaterialParameterEXT glBindMaterialParameterEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindMultiTextureEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureTarget target, UInt32 texture);
			[CLSCompliant(false)]
			public static BindMultiTextureEXT glBindMultiTextureEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Int32 BindParameterEXT(System.Graphics.OGL.ExtVertexShader value);
			[CLSCompliant(false)]
			public static BindParameterEXT glBindParameterEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindProgramARB(System.Graphics.OGL.AssemblyProgramTargetArb target, UInt32 program);
			[CLSCompliant(false)]
			public static BindProgramARB glBindProgramARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindProgramNV(System.Graphics.OGL.AssemblyProgramTargetArb target, UInt32 id);
			[CLSCompliant(false)]
			public static BindProgramNV glBindProgramNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindProgramPipeline(UInt32 pipeline);
			[CLSCompliant(false)]
			public static BindProgramPipeline glBindProgramPipeline;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindRenderbuffer(System.Graphics.OGL.RenderbufferTarget target, UInt32 renderbuffer);
			[CLSCompliant(false)]
			public static BindRenderbuffer glBindRenderbuffer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindRenderbufferEXT(System.Graphics.OGL.RenderbufferTarget target, UInt32 renderbuffer);
			[CLSCompliant(false)]
			public static BindRenderbufferEXT glBindRenderbufferEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindSampler(UInt32 unit, UInt32 sampler);
			[CLSCompliant(false)]
			public static BindSampler glBindSampler;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Int32 BindTexGenParameterEXT(System.Graphics.OGL.TextureUnit unit, System.Graphics.OGL.TextureCoordName coord, System.Graphics.OGL.TextureGenParameter value);
			[CLSCompliant(false)]
			public static BindTexGenParameterEXT glBindTexGenParameterEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindTexture(System.Graphics.OGL.TextureTarget target, UInt32 texture);
			[CLSCompliant(false)]
			public static BindTexture glBindTexture;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindTextureEXT(System.Graphics.OGL.TextureTarget target, UInt32 texture);
			[CLSCompliant(false)]
			public static BindTextureEXT glBindTextureEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Int32 BindTextureUnitParameterEXT(System.Graphics.OGL.TextureUnit unit, System.Graphics.OGL.ExtVertexShader value);
			[CLSCompliant(false)]
			public static BindTextureUnitParameterEXT glBindTextureUnitParameterEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindTransformFeedback(System.Graphics.OGL.TransformFeedbackTarget target, UInt32 id);
			[CLSCompliant(false)]
			public static BindTransformFeedback glBindTransformFeedback;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindTransformFeedbackNV(System.Graphics.OGL.NvTransformFeedback2 target, UInt32 id);
			[CLSCompliant(false)]
			public static BindTransformFeedbackNV glBindTransformFeedbackNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindVertexArray(UInt32 array);
			[CLSCompliant(false)]
			public static BindVertexArray glBindVertexArray;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindVertexArrayAPPLE(UInt32 array);
			[CLSCompliant(false)]
			public static BindVertexArrayAPPLE glBindVertexArrayAPPLE;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindVertexShaderEXT(UInt32 id);
			[CLSCompliant(false)]
			public static BindVertexShaderEXT glBindVertexShaderEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindVideoCaptureStreamBufferNV(UInt32 video_capture_slot, UInt32 stream, System.Graphics.OGL.NvVideoCapture frame_region, IntPtr offset);
			[CLSCompliant(false)]
			public static BindVideoCaptureStreamBufferNV glBindVideoCaptureStreamBufferNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BindVideoCaptureStreamTextureNV(UInt32 video_capture_slot, UInt32 stream, System.Graphics.OGL.NvVideoCapture frame_region, System.Graphics.OGL.NvVideoCapture target, UInt32 texture);
			[CLSCompliant(false)]
			public static BindVideoCaptureStreamTextureNV glBindVideoCaptureStreamTextureNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Binormal3bEXT(SByte bx, SByte by, SByte bz);
			[CLSCompliant(false)]
			public static Binormal3bEXT glBinormal3bEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Binormal3bvEXT(SByte* v);
			[CLSCompliant(false)]
			public unsafe static Binormal3bvEXT glBinormal3bvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Binormal3dEXT(Double bx, Double by, Double bz);
			[CLSCompliant(false)]
			public static Binormal3dEXT glBinormal3dEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Binormal3dvEXT(Double* v);
			[CLSCompliant(false)]
			public unsafe static Binormal3dvEXT glBinormal3dvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Binormal3fEXT(Single bx, Single by, Single bz);
			[CLSCompliant(false)]
			public static Binormal3fEXT glBinormal3fEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Binormal3fvEXT(Single* v);
			[CLSCompliant(false)]
			public unsafe static Binormal3fvEXT glBinormal3fvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Binormal3iEXT(Int32 bx, Int32 by, Int32 bz);
			[CLSCompliant(false)]
			public static Binormal3iEXT glBinormal3iEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Binormal3ivEXT(Int32* v);
			[CLSCompliant(false)]
			public unsafe static Binormal3ivEXT glBinormal3ivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Binormal3sEXT(Int16 bx, Int16 by, Int16 bz);
			[CLSCompliant(false)]
			public static Binormal3sEXT glBinormal3sEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Binormal3svEXT(Int16* v);
			[CLSCompliant(false)]
			public unsafe static Binormal3svEXT glBinormal3svEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BinormalPointerEXT(System.Graphics.OGL.NormalPointerType type, Int32 stride, IntPtr pointer);
			[CLSCompliant(false)]
			public static BinormalPointerEXT glBinormalPointerEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Bitmap(Int32 width, Int32 height, Single xorig, Single yorig, Single xmove, Single ymove, Byte* bitmap);
			[CLSCompliant(false)]
			public unsafe static Bitmap glBitmap;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BlendColor(Single red, Single green, Single blue, Single alpha);
			[CLSCompliant(false)]
			public static BlendColor glBlendColor;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BlendColorEXT(Single red, Single green, Single blue, Single alpha);
			[CLSCompliant(false)]
			public static BlendColorEXT glBlendColorEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BlendEquation(System.Graphics.OGL.BlendEquationMode mode);
			[CLSCompliant(false)]
			public static BlendEquation glBlendEquation;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BlendEquationEXT(System.Graphics.OGL.ExtBlendMinmax mode);
			[CLSCompliant(false)]
			public static BlendEquationEXT glBlendEquationEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BlendEquationi(UInt32 buf, System.Graphics.OGL.Version40 mode);
			[CLSCompliant(false)]
			public static BlendEquationi glBlendEquationi;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BlendEquationiARB(UInt32 buf, System.Graphics.OGL.ArbDrawBuffersBlend mode);
			[CLSCompliant(false)]
			public static BlendEquationiARB glBlendEquationiARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BlendEquationIndexedAMD(UInt32 buf, System.Graphics.OGL.AmdDrawBuffersBlend mode);
			[CLSCompliant(false)]
			public static BlendEquationIndexedAMD glBlendEquationIndexedAMD;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BlendEquationSeparate(System.Graphics.OGL.BlendEquationMode modeRGB, System.Graphics.OGL.BlendEquationMode modeAlpha);
			[CLSCompliant(false)]
			public static BlendEquationSeparate glBlendEquationSeparate;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BlendEquationSeparateEXT(System.Graphics.OGL.ExtBlendEquationSeparate modeRGB, System.Graphics.OGL.ExtBlendEquationSeparate modeAlpha);
			[CLSCompliant(false)]
			public static BlendEquationSeparateEXT glBlendEquationSeparateEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BlendEquationSeparatei(UInt32 buf, System.Graphics.OGL.BlendEquationMode modeRGB, System.Graphics.OGL.BlendEquationMode modeAlpha);
			[CLSCompliant(false)]
			public static BlendEquationSeparatei glBlendEquationSeparatei;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BlendEquationSeparateiARB(UInt32 buf, System.Graphics.OGL.ArbDrawBuffersBlend modeRGB, System.Graphics.OGL.ArbDrawBuffersBlend modeAlpha);
			[CLSCompliant(false)]
			public static BlendEquationSeparateiARB glBlendEquationSeparateiARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BlendEquationSeparateIndexedAMD(UInt32 buf, System.Graphics.OGL.AmdDrawBuffersBlend modeRGB, System.Graphics.OGL.AmdDrawBuffersBlend modeAlpha);
			[CLSCompliant(false)]
			public static BlendEquationSeparateIndexedAMD glBlendEquationSeparateIndexedAMD;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BlendFunc(System.Graphics.OGL.BlendingFactorSrc sfactor, System.Graphics.OGL.BlendingFactorDest dfactor);
			[CLSCompliant(false)]
			public static BlendFunc glBlendFunc;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BlendFunci(UInt32 buf, System.Graphics.OGL.Version40 src, System.Graphics.OGL.Version40 dst);
			[CLSCompliant(false)]
			public static BlendFunci glBlendFunci;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BlendFunciARB(UInt32 buf, System.Graphics.OGL.ArbDrawBuffersBlend src, System.Graphics.OGL.ArbDrawBuffersBlend dst);
			[CLSCompliant(false)]
			public static BlendFunciARB glBlendFunciARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BlendFuncIndexedAMD(UInt32 buf, System.Graphics.OGL.AmdDrawBuffersBlend src, System.Graphics.OGL.AmdDrawBuffersBlend dst);
			[CLSCompliant(false)]
			public static BlendFuncIndexedAMD glBlendFuncIndexedAMD;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BlendFuncSeparate(System.Graphics.OGL.BlendingFactorSrc sfactorRGB, System.Graphics.OGL.BlendingFactorDest dfactorRGB, System.Graphics.OGL.BlendingFactorSrc sfactorAlpha, System.Graphics.OGL.BlendingFactorDest dfactorAlpha);
			[CLSCompliant(false)]
			public static BlendFuncSeparate glBlendFuncSeparate;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BlendFuncSeparateEXT(System.Graphics.OGL.ExtBlendFuncSeparate sfactorRGB, System.Graphics.OGL.ExtBlendFuncSeparate dfactorRGB, System.Graphics.OGL.ExtBlendFuncSeparate sfactorAlpha, System.Graphics.OGL.ExtBlendFuncSeparate dfactorAlpha);
			[CLSCompliant(false)]
			public static BlendFuncSeparateEXT glBlendFuncSeparateEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BlendFuncSeparatei(UInt32 buf, System.Graphics.OGL.Version40 srcRGB, System.Graphics.OGL.Version40 dstRGB, System.Graphics.OGL.Version40 srcAlpha, System.Graphics.OGL.Version40 dstAlpha);
			[CLSCompliant(false)]
			public static BlendFuncSeparatei glBlendFuncSeparatei;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BlendFuncSeparateiARB(UInt32 buf, System.Graphics.OGL.ArbDrawBuffersBlend srcRGB, System.Graphics.OGL.ArbDrawBuffersBlend dstRGB, System.Graphics.OGL.ArbDrawBuffersBlend srcAlpha, System.Graphics.OGL.ArbDrawBuffersBlend dstAlpha);
			[CLSCompliant(false)]
			public static BlendFuncSeparateiARB glBlendFuncSeparateiARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BlendFuncSeparateIndexedAMD(UInt32 buf, System.Graphics.OGL.AmdDrawBuffersBlend srcRGB, System.Graphics.OGL.AmdDrawBuffersBlend dstRGB, System.Graphics.OGL.AmdDrawBuffersBlend srcAlpha, System.Graphics.OGL.AmdDrawBuffersBlend dstAlpha);
			[CLSCompliant(false)]
			public static BlendFuncSeparateIndexedAMD glBlendFuncSeparateIndexedAMD;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BlendFuncSeparateINGR(System.Graphics.OGL.All sfactorRGB, System.Graphics.OGL.All dfactorRGB, System.Graphics.OGL.All sfactorAlpha, System.Graphics.OGL.All dfactorAlpha);
			[CLSCompliant(false)]
			public static BlendFuncSeparateINGR glBlendFuncSeparateINGR;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BlitFramebuffer(Int32 srcX0, Int32 srcY0, Int32 srcX1, Int32 srcY1, Int32 dstX0, Int32 dstY0, Int32 dstX1, Int32 dstY1, System.Graphics.OGL.ClearBufferMask mask, System.Graphics.OGL.BlitFramebufferFilter filter);
			[CLSCompliant(false)]
			public static BlitFramebuffer glBlitFramebuffer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BlitFramebufferEXT(Int32 srcX0, Int32 srcY0, Int32 srcX1, Int32 srcY1, Int32 dstX0, Int32 dstY0, Int32 dstX1, Int32 dstY1, System.Graphics.OGL.ClearBufferMask mask, System.Graphics.OGL.ExtFramebufferBlit filter);
			[CLSCompliant(false)]
			public static BlitFramebufferEXT glBlitFramebufferEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BufferAddressRangeNV(System.Graphics.OGL.NvVertexBufferUnifiedMemory pname, UInt32 index, UInt64 address, IntPtr length);
			[CLSCompliant(false)]
			public static BufferAddressRangeNV glBufferAddressRangeNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BufferData(System.Graphics.OGL.BufferTarget target, IntPtr size, IntPtr data, System.Graphics.OGL.BufferUsageHint usage);
			[CLSCompliant(false)]
			public static BufferData glBufferData;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BufferDataARB(System.Graphics.OGL.BufferTargetArb target, IntPtr size, IntPtr data, System.Graphics.OGL.BufferUsageArb usage);
			[CLSCompliant(false)]
			public static BufferDataARB glBufferDataARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BufferParameteriAPPLE(System.Graphics.OGL.BufferTarget target, System.Graphics.OGL.BufferParameterApple pname, Int32 param);
			[CLSCompliant(false)]
			public static BufferParameteriAPPLE glBufferParameteriAPPLE;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BufferSubData(System.Graphics.OGL.BufferTarget target, IntPtr offset, IntPtr size, IntPtr data);
			[CLSCompliant(false)]
			public static BufferSubData glBufferSubData;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void BufferSubDataARB(System.Graphics.OGL.BufferTargetArb target, IntPtr offset, IntPtr size, IntPtr data);
			[CLSCompliant(false)]
			public static BufferSubDataARB glBufferSubDataARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CallList(UInt32 list);
			[CLSCompliant(false)]
			public static CallList glCallList;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CallLists(Int32 n, System.Graphics.OGL.ListNameType type, IntPtr lists);
			[CLSCompliant(false)]
			public static CallLists glCallLists;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate System.Graphics.OGL.FramebufferErrorCode CheckFramebufferStatus(System.Graphics.OGL.FramebufferTarget target);
			[CLSCompliant(false)]
			public static CheckFramebufferStatus glCheckFramebufferStatus;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate System.Graphics.OGL.FramebufferErrorCode CheckFramebufferStatusEXT(System.Graphics.OGL.FramebufferTarget target);
			[CLSCompliant(false)]
			public static CheckFramebufferStatusEXT glCheckFramebufferStatusEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate System.Graphics.OGL.ExtDirectStateAccess CheckNamedFramebufferStatusEXT(UInt32 framebuffer, System.Graphics.OGL.FramebufferTarget target);
			[CLSCompliant(false)]
			public static CheckNamedFramebufferStatusEXT glCheckNamedFramebufferStatusEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ClampColor(System.Graphics.OGL.ClampColorTarget target, System.Graphics.OGL.ClampColorMode clamp);
			[CLSCompliant(false)]
			public static ClampColor glClampColor;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ClampColorARB(System.Graphics.OGL.ArbColorBufferFloat target, System.Graphics.OGL.ArbColorBufferFloat clamp);
			[CLSCompliant(false)]
			public static ClampColorARB glClampColorARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Clear(System.Graphics.OGL.ClearBufferMask mask);
			[CLSCompliant(false)]
			public static Clear glClear;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ClearAccum(Single red, Single green, Single blue, Single alpha);
			[CLSCompliant(false)]
			public static ClearAccum glClearAccum;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ClearBufferfi(System.Graphics.OGL.ClearBuffer buffer, Int32 drawbuffer, Single depth, Int32 stencil);
			[CLSCompliant(false)]
			public static ClearBufferfi glClearBufferfi;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ClearBufferfv(System.Graphics.OGL.ClearBuffer buffer, Int32 drawbuffer, Single* value);
			[CLSCompliant(false)]
			public unsafe static ClearBufferfv glClearBufferfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ClearBufferiv(System.Graphics.OGL.ClearBuffer buffer, Int32 drawbuffer, Int32* value);
			[CLSCompliant(false)]
			public unsafe static ClearBufferiv glClearBufferiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ClearBufferuiv(System.Graphics.OGL.ClearBuffer buffer, Int32 drawbuffer, UInt32* value);
			[CLSCompliant(false)]
			public unsafe static ClearBufferuiv glClearBufferuiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ClearColor(Single red, Single green, Single blue, Single alpha);
			[CLSCompliant(false)]
			public static ClearColor glClearColor;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ClearColorIiEXT(Int32 red, Int32 green, Int32 blue, Int32 alpha);
			[CLSCompliant(false)]
			public static ClearColorIiEXT glClearColorIiEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ClearColorIuiEXT(UInt32 red, UInt32 green, UInt32 blue, UInt32 alpha);
			[CLSCompliant(false)]
			public static ClearColorIuiEXT glClearColorIuiEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ClearDepth(Double depth);
			[CLSCompliant(false)]
			public static ClearDepth glClearDepth;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ClearDepthdNV(Double depth);
			[CLSCompliant(false)]
			public static ClearDepthdNV glClearDepthdNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ClearDepthf(Single d);
			[CLSCompliant(false)]
			public static ClearDepthf glClearDepthf;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ClearIndex(Single c);
			[CLSCompliant(false)]
			public static ClearIndex glClearIndex;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ClearStencil(Int32 s);
			[CLSCompliant(false)]
			public static ClearStencil glClearStencil;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ClientActiveTexture(System.Graphics.OGL.TextureUnit texture);
			[CLSCompliant(false)]
			public static ClientActiveTexture glClientActiveTexture;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ClientActiveTextureARB(System.Graphics.OGL.TextureUnit texture);
			[CLSCompliant(false)]
			public static ClientActiveTextureARB glClientActiveTextureARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ClientActiveVertexStreamATI(System.Graphics.OGL.AtiVertexStreams stream);
			[CLSCompliant(false)]
			public static ClientActiveVertexStreamATI glClientActiveVertexStreamATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ClientAttribDefaultEXT(System.Graphics.OGL.ClientAttribMask mask);
			[CLSCompliant(false)]
			public static ClientAttribDefaultEXT glClientAttribDefaultEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate System.Graphics.OGL.ArbSync ClientWaitSync(IntPtr sync, UInt32 flags, UInt64 timeout);
			[CLSCompliant(false)]
			public static ClientWaitSync glClientWaitSync;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ClipPlane(System.Graphics.OGL.ClipPlaneName plane, Double* equation);
			[CLSCompliant(false)]
			public unsafe static ClipPlane glClipPlane;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Color3b(SByte red, SByte green, SByte blue);
			[CLSCompliant(false)]
			public static Color3b glColor3b;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Color3bv(SByte* v);
			[CLSCompliant(false)]
			public unsafe static Color3bv glColor3bv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Color3d(Double red, Double green, Double blue);
			[CLSCompliant(false)]
			public static Color3d glColor3d;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Color3dv(Double* v);
			[CLSCompliant(false)]
			public unsafe static Color3dv glColor3dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Color3f(Single red, Single green, Single blue);
			[CLSCompliant(false)]
			public static Color3f glColor3f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Color3fv(Single* v);
			[CLSCompliant(false)]
			public unsafe static Color3fv glColor3fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Color3fVertex3fSUN(Single r, Single g, Single b, Single x, Single y, Single z);
			[CLSCompliant(false)]
			public static Color3fVertex3fSUN glColor3fVertex3fSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Color3fVertex3fvSUN(Single* c, Single* v);
			[CLSCompliant(false)]
			public unsafe static Color3fVertex3fvSUN glColor3fVertex3fvSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Color3hNV(Half red, Half green, Half blue);
			[CLSCompliant(false)]
			public static Color3hNV glColor3hNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Color3hvNV(Half* v);
			[CLSCompliant(false)]
			public unsafe static Color3hvNV glColor3hvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Color3i(Int32 red, Int32 green, Int32 blue);
			[CLSCompliant(false)]
			public static Color3i glColor3i;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Color3iv(Int32* v);
			[CLSCompliant(false)]
			public unsafe static Color3iv glColor3iv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Color3s(Int16 red, Int16 green, Int16 blue);
			[CLSCompliant(false)]
			public static Color3s glColor3s;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Color3sv(Int16* v);
			[CLSCompliant(false)]
			public unsafe static Color3sv glColor3sv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Color3ub(Byte red, Byte green, Byte blue);
			[CLSCompliant(false)]
			public static Color3ub glColor3ub;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Color3ubv(Byte* v);
			[CLSCompliant(false)]
			public unsafe static Color3ubv glColor3ubv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Color3ui(UInt32 red, UInt32 green, UInt32 blue);
			[CLSCompliant(false)]
			public static Color3ui glColor3ui;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Color3uiv(UInt32* v);
			[CLSCompliant(false)]
			public unsafe static Color3uiv glColor3uiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Color3us(UInt16 red, UInt16 green, UInt16 blue);
			[CLSCompliant(false)]
			public static Color3us glColor3us;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Color3usv(UInt16* v);
			[CLSCompliant(false)]
			public unsafe static Color3usv glColor3usv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Color4b(SByte red, SByte green, SByte blue, SByte alpha);
			[CLSCompliant(false)]
			public static Color4b glColor4b;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Color4bv(SByte* v);
			[CLSCompliant(false)]
			public unsafe static Color4bv glColor4bv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Color4d(Double red, Double green, Double blue, Double alpha);
			[CLSCompliant(false)]
			public static Color4d glColor4d;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Color4dv(Double* v);
			[CLSCompliant(false)]
			public unsafe static Color4dv glColor4dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Color4f(Single red, Single green, Single blue, Single alpha);
			[CLSCompliant(false)]
			public static Color4f glColor4f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Color4fNormal3fVertex3fSUN(Single r, Single g, Single b, Single a, Single nx, Single ny, Single nz, Single x, Single y, Single z);
			[CLSCompliant(false)]
			public static Color4fNormal3fVertex3fSUN glColor4fNormal3fVertex3fSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Color4fNormal3fVertex3fvSUN(Single* c, Single* n, Single* v);
			[CLSCompliant(false)]
			public unsafe static Color4fNormal3fVertex3fvSUN glColor4fNormal3fVertex3fvSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Color4fv(Single* v);
			[CLSCompliant(false)]
			public unsafe static Color4fv glColor4fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Color4hNV(Half red, Half green, Half blue, Half alpha);
			[CLSCompliant(false)]
			public static Color4hNV glColor4hNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Color4hvNV(Half* v);
			[CLSCompliant(false)]
			public unsafe static Color4hvNV glColor4hvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Color4i(Int32 red, Int32 green, Int32 blue, Int32 alpha);
			[CLSCompliant(false)]
			public static Color4i glColor4i;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Color4iv(Int32* v);
			[CLSCompliant(false)]
			public unsafe static Color4iv glColor4iv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Color4s(Int16 red, Int16 green, Int16 blue, Int16 alpha);
			[CLSCompliant(false)]
			public static Color4s glColor4s;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Color4sv(Int16* v);
			[CLSCompliant(false)]
			public unsafe static Color4sv glColor4sv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Color4ub(Byte red, Byte green, Byte blue, Byte alpha);
			[CLSCompliant(false)]
			public static Color4ub glColor4ub;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Color4ubv(Byte* v);
			[CLSCompliant(false)]
			public unsafe static Color4ubv glColor4ubv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Color4ubVertex2fSUN(Byte r, Byte g, Byte b, Byte a, Single x, Single y);
			[CLSCompliant(false)]
			public static Color4ubVertex2fSUN glColor4ubVertex2fSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Color4ubVertex2fvSUN(Byte* c, Single* v);
			[CLSCompliant(false)]
			public unsafe static Color4ubVertex2fvSUN glColor4ubVertex2fvSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Color4ubVertex3fSUN(Byte r, Byte g, Byte b, Byte a, Single x, Single y, Single z);
			[CLSCompliant(false)]
			public static Color4ubVertex3fSUN glColor4ubVertex3fSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Color4ubVertex3fvSUN(Byte* c, Single* v);
			[CLSCompliant(false)]
			public unsafe static Color4ubVertex3fvSUN glColor4ubVertex3fvSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Color4ui(UInt32 red, UInt32 green, UInt32 blue, UInt32 alpha);
			[CLSCompliant(false)]
			public static Color4ui glColor4ui;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Color4uiv(UInt32* v);
			[CLSCompliant(false)]
			public unsafe static Color4uiv glColor4uiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Color4us(UInt16 red, UInt16 green, UInt16 blue, UInt16 alpha);
			[CLSCompliant(false)]
			public static Color4us glColor4us;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Color4usv(UInt16* v);
			[CLSCompliant(false)]
			public unsafe static Color4usv glColor4usv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ColorFormatNV(Int32 size, System.Graphics.OGL.NvVertexBufferUnifiedMemory type, Int32 stride);
			[CLSCompliant(false)]
			public static ColorFormatNV glColorFormatNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ColorFragmentOp1ATI(System.Graphics.OGL.AtiFragmentShader op, UInt32 dst, UInt32 dstMask, UInt32 dstMod, UInt32 arg1, UInt32 arg1Rep, UInt32 arg1Mod);
			[CLSCompliant(false)]
			public static ColorFragmentOp1ATI glColorFragmentOp1ATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ColorFragmentOp2ATI(System.Graphics.OGL.AtiFragmentShader op, UInt32 dst, UInt32 dstMask, UInt32 dstMod, UInt32 arg1, UInt32 arg1Rep, UInt32 arg1Mod, UInt32 arg2, UInt32 arg2Rep, UInt32 arg2Mod);
			[CLSCompliant(false)]
			public static ColorFragmentOp2ATI glColorFragmentOp2ATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ColorFragmentOp3ATI(System.Graphics.OGL.AtiFragmentShader op, UInt32 dst, UInt32 dstMask, UInt32 dstMod, UInt32 arg1, UInt32 arg1Rep, UInt32 arg1Mod, UInt32 arg2, UInt32 arg2Rep, UInt32 arg2Mod, UInt32 arg3, UInt32 arg3Rep, UInt32 arg3Mod);
			[CLSCompliant(false)]
			public static ColorFragmentOp3ATI glColorFragmentOp3ATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ColorMask(bool red, bool green, bool blue, bool alpha);
			[CLSCompliant(false)]
			public static ColorMask glColorMask;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ColorMaski(UInt32 index, bool r, bool g, bool b, bool a);
			[CLSCompliant(false)]
			public static ColorMaski glColorMaski;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ColorMaskIndexedEXT(UInt32 index, bool r, bool g, bool b, bool a);
			[CLSCompliant(false)]
			public static ColorMaskIndexedEXT glColorMaskIndexedEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ColorMaterial(System.Graphics.OGL.MaterialFace face, System.Graphics.OGL.ColorMaterialParameter mode);
			[CLSCompliant(false)]
			public static ColorMaterial glColorMaterial;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ColorP3ui(System.Graphics.OGL.PackedPointerType type, UInt32 color);
			[CLSCompliant(false)]
			public static ColorP3ui glColorP3ui;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ColorP3uiv(System.Graphics.OGL.PackedPointerType type, UInt32* color);
			[CLSCompliant(false)]
			public unsafe static ColorP3uiv glColorP3uiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ColorP4ui(System.Graphics.OGL.PackedPointerType type, UInt32 color);
			[CLSCompliant(false)]
			public static ColorP4ui glColorP4ui;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ColorP4uiv(System.Graphics.OGL.PackedPointerType type, UInt32* color);
			[CLSCompliant(false)]
			public unsafe static ColorP4uiv glColorP4uiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ColorPointer(Int32 size, System.Graphics.OGL.ColorPointerType type, Int32 stride, IntPtr pointer);
			[CLSCompliant(false)]
			public static ColorPointer glColorPointer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ColorPointerEXT(Int32 size, System.Graphics.OGL.ColorPointerType type, Int32 stride, Int32 count, IntPtr pointer);
			[CLSCompliant(false)]
			public static ColorPointerEXT glColorPointerEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ColorPointerListIBM(Int32 size, System.Graphics.OGL.ColorPointerType type, Int32 stride, IntPtr pointer, Int32 ptrstride);
			[CLSCompliant(false)]
			public static ColorPointerListIBM glColorPointerListIBM;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ColorPointervINTEL(Int32 size, System.Graphics.OGL.VertexPointerType type, IntPtr pointer);
			[CLSCompliant(false)]
			public static ColorPointervINTEL glColorPointervINTEL;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ColorSubTable(System.Graphics.OGL.ColorTableTarget target, Int32 start, Int32 count, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, IntPtr data);
			[CLSCompliant(false)]
			public static ColorSubTable glColorSubTable;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ColorSubTableEXT(System.Graphics.OGL.ColorTableTarget target, Int32 start, Int32 count, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, IntPtr data);
			[CLSCompliant(false)]
			public static ColorSubTableEXT glColorSubTableEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ColorTable(System.Graphics.OGL.ColorTableTarget target, System.Graphics.OGL.PixelInternalFormat internalformat, Int32 width, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, IntPtr table);
			[CLSCompliant(false)]
			public static ColorTable glColorTable;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ColorTableEXT(System.Graphics.OGL.ColorTableTarget target, System.Graphics.OGL.PixelInternalFormat internalFormat, Int32 width, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, IntPtr table);
			[CLSCompliant(false)]
			public static ColorTableEXT glColorTableEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ColorTableParameterfv(System.Graphics.OGL.ColorTableTarget target, System.Graphics.OGL.ColorTableParameterPName pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static ColorTableParameterfv glColorTableParameterfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ColorTableParameterfvSGI(System.Graphics.OGL.SgiColorTable target, System.Graphics.OGL.SgiColorTable pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static ColorTableParameterfvSGI glColorTableParameterfvSGI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ColorTableParameteriv(System.Graphics.OGL.ColorTableTarget target, System.Graphics.OGL.ColorTableParameterPName pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static ColorTableParameteriv glColorTableParameteriv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ColorTableParameterivSGI(System.Graphics.OGL.SgiColorTable target, System.Graphics.OGL.SgiColorTable pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static ColorTableParameterivSGI glColorTableParameterivSGI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ColorTableSGI(System.Graphics.OGL.SgiColorTable target, System.Graphics.OGL.PixelInternalFormat internalformat, Int32 width, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, IntPtr table);
			[CLSCompliant(false)]
			public static ColorTableSGI glColorTableSGI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CombinerInputNV(System.Graphics.OGL.NvRegisterCombiners stage, System.Graphics.OGL.NvRegisterCombiners portion, System.Graphics.OGL.NvRegisterCombiners variable, System.Graphics.OGL.NvRegisterCombiners input, System.Graphics.OGL.NvRegisterCombiners mapping, System.Graphics.OGL.NvRegisterCombiners componentUsage);
			[CLSCompliant(false)]
			public static CombinerInputNV glCombinerInputNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CombinerOutputNV(System.Graphics.OGL.NvRegisterCombiners stage, System.Graphics.OGL.NvRegisterCombiners portion, System.Graphics.OGL.NvRegisterCombiners abOutput, System.Graphics.OGL.NvRegisterCombiners cdOutput, System.Graphics.OGL.NvRegisterCombiners sumOutput, System.Graphics.OGL.NvRegisterCombiners scale, System.Graphics.OGL.NvRegisterCombiners bias, bool abDotProduct, bool cdDotProduct, bool muxSum);
			[CLSCompliant(false)]
			public static CombinerOutputNV glCombinerOutputNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CombinerParameterfNV(System.Graphics.OGL.NvRegisterCombiners pname, Single param);
			[CLSCompliant(false)]
			public static CombinerParameterfNV glCombinerParameterfNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void CombinerParameterfvNV(System.Graphics.OGL.NvRegisterCombiners pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static CombinerParameterfvNV glCombinerParameterfvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CombinerParameteriNV(System.Graphics.OGL.NvRegisterCombiners pname, Int32 param);
			[CLSCompliant(false)]
			public static CombinerParameteriNV glCombinerParameteriNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void CombinerParameterivNV(System.Graphics.OGL.NvRegisterCombiners pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static CombinerParameterivNV glCombinerParameterivNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void CombinerStageParameterfvNV(System.Graphics.OGL.NvRegisterCombiners2 stage, System.Graphics.OGL.NvRegisterCombiners2 pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static CombinerStageParameterfvNV glCombinerStageParameterfvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CompileShader(UInt32 shader);
			[CLSCompliant(false)]
			public static CompileShader glCompileShader;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CompileShaderARB(UInt32 shaderObj);
			[CLSCompliant(false)]
			public static CompileShaderARB glCompileShaderARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void CompileShaderIncludeARB(UInt32 shader, Int32 count, String[] path, Int32* length);
			[CLSCompliant(false)]
			public unsafe static CompileShaderIncludeARB glCompileShaderIncludeARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CompressedMultiTexImage1DEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureTarget target, Int32 level, System.Graphics.OGL.ExtDirectStateAccess internalformat, Int32 width, Int32 border, Int32 imageSize, IntPtr bits);
			[CLSCompliant(false)]
			public static CompressedMultiTexImage1DEXT glCompressedMultiTexImage1DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CompressedMultiTexImage2DEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureTarget target, Int32 level, System.Graphics.OGL.ExtDirectStateAccess internalformat, Int32 width, Int32 height, Int32 border, Int32 imageSize, IntPtr bits);
			[CLSCompliant(false)]
			public static CompressedMultiTexImage2DEXT glCompressedMultiTexImage2DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CompressedMultiTexImage3DEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureTarget target, Int32 level, System.Graphics.OGL.ExtDirectStateAccess internalformat, Int32 width, Int32 height, Int32 depth, Int32 border, Int32 imageSize, IntPtr bits);
			[CLSCompliant(false)]
			public static CompressedMultiTexImage3DEXT glCompressedMultiTexImage3DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CompressedMultiTexSubImage1DEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureTarget target, Int32 level, Int32 xoffset, Int32 width, System.Graphics.OGL.TargetPixelFormat format, Int32 imageSize, IntPtr bits);
			[CLSCompliant(false)]
			public static CompressedMultiTexSubImage1DEXT glCompressedMultiTexSubImage1DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CompressedMultiTexSubImage2DEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureTarget target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 width, Int32 height, System.Graphics.OGL.TargetPixelFormat format, Int32 imageSize, IntPtr bits);
			[CLSCompliant(false)]
			public static CompressedMultiTexSubImage2DEXT glCompressedMultiTexSubImage2DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CompressedMultiTexSubImage3DEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureTarget target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 width, Int32 height, Int32 depth, System.Graphics.OGL.TargetPixelFormat format, Int32 imageSize, IntPtr bits);
			[CLSCompliant(false)]
			public static CompressedMultiTexSubImage3DEXT glCompressedMultiTexSubImage3DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CompressedTexImage1D(System.Graphics.OGL.TextureTarget target, Int32 level, System.Graphics.OGL.PixelInternalFormat internalformat, Int32 width, Int32 border, Int32 imageSize, IntPtr data);
			[CLSCompliant(false)]
			public static CompressedTexImage1D glCompressedTexImage1D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CompressedTexImage1DARB(System.Graphics.OGL.TextureTarget target, Int32 level, System.Graphics.OGL.PixelInternalFormat internalformat, Int32 width, Int32 border, Int32 imageSize, IntPtr data);
			[CLSCompliant(false)]
			public static CompressedTexImage1DARB glCompressedTexImage1DARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CompressedTexImage2D(System.Graphics.OGL.TextureTarget target, Int32 level, System.Graphics.OGL.PixelInternalFormat internalformat, Int32 width, Int32 height, Int32 border, Int32 imageSize, IntPtr data);
			[CLSCompliant(false)]
			public static CompressedTexImage2D glCompressedTexImage2D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CompressedTexImage2DARB(System.Graphics.OGL.TextureTarget target, Int32 level, System.Graphics.OGL.PixelInternalFormat internalformat, Int32 width, Int32 height, Int32 border, Int32 imageSize, IntPtr data);
			[CLSCompliant(false)]
			public static CompressedTexImage2DARB glCompressedTexImage2DARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CompressedTexImage3D(System.Graphics.OGL.TextureTarget target, Int32 level, System.Graphics.OGL.PixelInternalFormat internalformat, Int32 width, Int32 height, Int32 depth, Int32 border, Int32 imageSize, IntPtr data);
			[CLSCompliant(false)]
			public static CompressedTexImage3D glCompressedTexImage3D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CompressedTexImage3DARB(System.Graphics.OGL.TextureTarget target, Int32 level, System.Graphics.OGL.PixelInternalFormat internalformat, Int32 width, Int32 height, Int32 depth, Int32 border, Int32 imageSize, IntPtr data);
			[CLSCompliant(false)]
			public static CompressedTexImage3DARB glCompressedTexImage3DARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CompressedTexSubImage1D(System.Graphics.OGL.TextureTarget target, Int32 level, Int32 xoffset, Int32 width, System.Graphics.OGL.TargetPixelFormat format, Int32 imageSize, IntPtr data);
			[CLSCompliant(false)]
			public static CompressedTexSubImage1D glCompressedTexSubImage1D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CompressedTexSubImage1DARB(System.Graphics.OGL.TextureTarget target, Int32 level, Int32 xoffset, Int32 width, System.Graphics.OGL.TargetPixelFormat format, Int32 imageSize, IntPtr data);
			[CLSCompliant(false)]
			public static CompressedTexSubImage1DARB glCompressedTexSubImage1DARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CompressedTexSubImage2D(System.Graphics.OGL.TextureTarget target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 width, Int32 height, System.Graphics.OGL.TargetPixelFormat format, Int32 imageSize, IntPtr data);
			[CLSCompliant(false)]
			public static CompressedTexSubImage2D glCompressedTexSubImage2D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CompressedTexSubImage2DARB(System.Graphics.OGL.TextureTarget target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 width, Int32 height, System.Graphics.OGL.TargetPixelFormat format, Int32 imageSize, IntPtr data);
			[CLSCompliant(false)]
			public static CompressedTexSubImage2DARB glCompressedTexSubImage2DARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CompressedTexSubImage3D(System.Graphics.OGL.TextureTarget target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 width, Int32 height, Int32 depth, System.Graphics.OGL.TargetPixelFormat format, Int32 imageSize, IntPtr data);
			[CLSCompliant(false)]
			public static CompressedTexSubImage3D glCompressedTexSubImage3D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CompressedTexSubImage3DARB(System.Graphics.OGL.TextureTarget target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 width, Int32 height, Int32 depth, System.Graphics.OGL.TargetPixelFormat format, Int32 imageSize, IntPtr data);
			[CLSCompliant(false)]
			public static CompressedTexSubImage3DARB glCompressedTexSubImage3DARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CompressedTextureImage1DEXT(UInt32 texture, System.Graphics.OGL.TextureTarget target, Int32 level, System.Graphics.OGL.ExtDirectStateAccess internalformat, Int32 width, Int32 border, Int32 imageSize, IntPtr bits);
			[CLSCompliant(false)]
			public static CompressedTextureImage1DEXT glCompressedTextureImage1DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CompressedTextureImage2DEXT(UInt32 texture, System.Graphics.OGL.TextureTarget target, Int32 level, System.Graphics.OGL.ExtDirectStateAccess internalformat, Int32 width, Int32 height, Int32 border, Int32 imageSize, IntPtr bits);
			[CLSCompliant(false)]
			public static CompressedTextureImage2DEXT glCompressedTextureImage2DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CompressedTextureImage3DEXT(UInt32 texture, System.Graphics.OGL.TextureTarget target, Int32 level, System.Graphics.OGL.ExtDirectStateAccess internalformat, Int32 width, Int32 height, Int32 depth, Int32 border, Int32 imageSize, IntPtr bits);
			[CLSCompliant(false)]
			public static CompressedTextureImage3DEXT glCompressedTextureImage3DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CompressedTextureSubImage1DEXT(UInt32 texture, System.Graphics.OGL.TextureTarget target, Int32 level, Int32 xoffset, Int32 width, System.Graphics.OGL.TargetPixelFormat format, Int32 imageSize, IntPtr bits);
			[CLSCompliant(false)]
			public static CompressedTextureSubImage1DEXT glCompressedTextureSubImage1DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CompressedTextureSubImage2DEXT(UInt32 texture, System.Graphics.OGL.TextureTarget target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 width, Int32 height, System.Graphics.OGL.TargetPixelFormat format, Int32 imageSize, IntPtr bits);
			[CLSCompliant(false)]
			public static CompressedTextureSubImage2DEXT glCompressedTextureSubImage2DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CompressedTextureSubImage3DEXT(UInt32 texture, System.Graphics.OGL.TextureTarget target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 width, Int32 height, Int32 depth, System.Graphics.OGL.TargetPixelFormat format, Int32 imageSize, IntPtr bits);
			[CLSCompliant(false)]
			public static CompressedTextureSubImage3DEXT glCompressedTextureSubImage3DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ConvolutionFilter1D(System.Graphics.OGL.ConvolutionTarget target, System.Graphics.OGL.PixelInternalFormat internalformat, Int32 width, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, IntPtr image);
			[CLSCompliant(false)]
			public static ConvolutionFilter1D glConvolutionFilter1D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ConvolutionFilter1DEXT(System.Graphics.OGL.ExtConvolution target, System.Graphics.OGL.PixelInternalFormat internalformat, Int32 width, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, IntPtr image);
			[CLSCompliant(false)]
			public static ConvolutionFilter1DEXT glConvolutionFilter1DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ConvolutionFilter2D(System.Graphics.OGL.ConvolutionTarget target, System.Graphics.OGL.PixelInternalFormat internalformat, Int32 width, Int32 height, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, IntPtr image);
			[CLSCompliant(false)]
			public static ConvolutionFilter2D glConvolutionFilter2D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ConvolutionFilter2DEXT(System.Graphics.OGL.ExtConvolution target, System.Graphics.OGL.PixelInternalFormat internalformat, Int32 width, Int32 height, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, IntPtr image);
			[CLSCompliant(false)]
			public static ConvolutionFilter2DEXT glConvolutionFilter2DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ConvolutionParameterf(System.Graphics.OGL.ConvolutionTarget target, System.Graphics.OGL.ConvolutionParameter pname, Single @params);
			[CLSCompliant(false)]
			public static ConvolutionParameterf glConvolutionParameterf;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ConvolutionParameterfEXT(System.Graphics.OGL.ExtConvolution target, System.Graphics.OGL.ExtConvolution pname, Single @params);
			[CLSCompliant(false)]
			public static ConvolutionParameterfEXT glConvolutionParameterfEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ConvolutionParameterfv(System.Graphics.OGL.ConvolutionTarget target, System.Graphics.OGL.ConvolutionParameter pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static ConvolutionParameterfv glConvolutionParameterfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ConvolutionParameterfvEXT(System.Graphics.OGL.ExtConvolution target, System.Graphics.OGL.ExtConvolution pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static ConvolutionParameterfvEXT glConvolutionParameterfvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ConvolutionParameteri(System.Graphics.OGL.ConvolutionTarget target, System.Graphics.OGL.ConvolutionParameter pname, Int32 @params);
			[CLSCompliant(false)]
			public static ConvolutionParameteri glConvolutionParameteri;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ConvolutionParameteriEXT(System.Graphics.OGL.ExtConvolution target, System.Graphics.OGL.ExtConvolution pname, Int32 @params);
			[CLSCompliant(false)]
			public static ConvolutionParameteriEXT glConvolutionParameteriEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ConvolutionParameteriv(System.Graphics.OGL.ConvolutionTarget target, System.Graphics.OGL.ConvolutionParameter pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static ConvolutionParameteriv glConvolutionParameteriv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ConvolutionParameterivEXT(System.Graphics.OGL.ExtConvolution target, System.Graphics.OGL.ExtConvolution pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static ConvolutionParameterivEXT glConvolutionParameterivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CopyBufferSubData(System.Graphics.OGL.BufferTarget readTarget, System.Graphics.OGL.BufferTarget writeTarget, IntPtr readOffset, IntPtr writeOffset, IntPtr size);
			[CLSCompliant(false)]
			public static CopyBufferSubData glCopyBufferSubData;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CopyColorSubTable(System.Graphics.OGL.ColorTableTarget target, Int32 start, Int32 x, Int32 y, Int32 width);
			[CLSCompliant(false)]
			public static CopyColorSubTable glCopyColorSubTable;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CopyColorSubTableEXT(System.Graphics.OGL.ColorTableTarget target, Int32 start, Int32 x, Int32 y, Int32 width);
			[CLSCompliant(false)]
			public static CopyColorSubTableEXT glCopyColorSubTableEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CopyColorTable(System.Graphics.OGL.ColorTableTarget target, System.Graphics.OGL.PixelInternalFormat internalformat, Int32 x, Int32 y, Int32 width);
			[CLSCompliant(false)]
			public static CopyColorTable glCopyColorTable;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CopyColorTableSGI(System.Graphics.OGL.SgiColorTable target, System.Graphics.OGL.PixelInternalFormat internalformat, Int32 x, Int32 y, Int32 width);
			[CLSCompliant(false)]
			public static CopyColorTableSGI glCopyColorTableSGI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CopyConvolutionFilter1D(System.Graphics.OGL.ConvolutionTarget target, System.Graphics.OGL.PixelInternalFormat internalformat, Int32 x, Int32 y, Int32 width);
			[CLSCompliant(false)]
			public static CopyConvolutionFilter1D glCopyConvolutionFilter1D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CopyConvolutionFilter1DEXT(System.Graphics.OGL.ExtConvolution target, System.Graphics.OGL.PixelInternalFormat internalformat, Int32 x, Int32 y, Int32 width);
			[CLSCompliant(false)]
			public static CopyConvolutionFilter1DEXT glCopyConvolutionFilter1DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CopyConvolutionFilter2D(System.Graphics.OGL.ConvolutionTarget target, System.Graphics.OGL.PixelInternalFormat internalformat, Int32 x, Int32 y, Int32 width, Int32 height);
			[CLSCompliant(false)]
			public static CopyConvolutionFilter2D glCopyConvolutionFilter2D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CopyConvolutionFilter2DEXT(System.Graphics.OGL.ExtConvolution target, System.Graphics.OGL.PixelInternalFormat internalformat, Int32 x, Int32 y, Int32 width, Int32 height);
			[CLSCompliant(false)]
			public static CopyConvolutionFilter2DEXT glCopyConvolutionFilter2DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CopyImageSubDataNV(UInt32 srcName, System.Graphics.OGL.NvCopyImage srcTarget, Int32 srcLevel, Int32 srcX, Int32 srcY, Int32 srcZ, UInt32 dstName, System.Graphics.OGL.NvCopyImage dstTarget, Int32 dstLevel, Int32 dstX, Int32 dstY, Int32 dstZ, Int32 width, Int32 height, Int32 depth);
			[CLSCompliant(false)]
			public static CopyImageSubDataNV glCopyImageSubDataNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CopyMultiTexImage1DEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureTarget target, Int32 level, System.Graphics.OGL.ExtDirectStateAccess internalformat, Int32 x, Int32 y, Int32 width, Int32 border);
			[CLSCompliant(false)]
			public static CopyMultiTexImage1DEXT glCopyMultiTexImage1DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CopyMultiTexImage2DEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureTarget target, Int32 level, System.Graphics.OGL.ExtDirectStateAccess internalformat, Int32 x, Int32 y, Int32 width, Int32 height, Int32 border);
			[CLSCompliant(false)]
			public static CopyMultiTexImage2DEXT glCopyMultiTexImage2DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CopyMultiTexSubImage1DEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureTarget target, Int32 level, Int32 xoffset, Int32 x, Int32 y, Int32 width);
			[CLSCompliant(false)]
			public static CopyMultiTexSubImage1DEXT glCopyMultiTexSubImage1DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CopyMultiTexSubImage2DEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureTarget target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 x, Int32 y, Int32 width, Int32 height);
			[CLSCompliant(false)]
			public static CopyMultiTexSubImage2DEXT glCopyMultiTexSubImage2DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CopyMultiTexSubImage3DEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureTarget target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 x, Int32 y, Int32 width, Int32 height);
			[CLSCompliant(false)]
			public static CopyMultiTexSubImage3DEXT glCopyMultiTexSubImage3DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CopyPixels(Int32 x, Int32 y, Int32 width, Int32 height, System.Graphics.OGL.PixelCopyType type);
			[CLSCompliant(false)]
			public static CopyPixels glCopyPixels;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CopyTexImage1D(System.Graphics.OGL.TextureTarget target, Int32 level, System.Graphics.OGL.PixelInternalFormat internalformat, Int32 x, Int32 y, Int32 width, Int32 border);
			[CLSCompliant(false)]
			public static CopyTexImage1D glCopyTexImage1D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CopyTexImage1DEXT(System.Graphics.OGL.TextureTarget target, Int32 level, System.Graphics.OGL.PixelInternalFormat internalformat, Int32 x, Int32 y, Int32 width, Int32 border);
			[CLSCompliant(false)]
			public static CopyTexImage1DEXT glCopyTexImage1DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CopyTexImage2D(System.Graphics.OGL.TextureTarget target, Int32 level, System.Graphics.OGL.PixelInternalFormat internalformat, Int32 x, Int32 y, Int32 width, Int32 height, Int32 border);
			[CLSCompliant(false)]
			public static CopyTexImage2D glCopyTexImage2D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CopyTexImage2DEXT(System.Graphics.OGL.TextureTarget target, Int32 level, System.Graphics.OGL.PixelInternalFormat internalformat, Int32 x, Int32 y, Int32 width, Int32 height, Int32 border);
			[CLSCompliant(false)]
			public static CopyTexImage2DEXT glCopyTexImage2DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CopyTexSubImage1D(System.Graphics.OGL.TextureTarget target, Int32 level, Int32 xoffset, Int32 x, Int32 y, Int32 width);
			[CLSCompliant(false)]
			public static CopyTexSubImage1D glCopyTexSubImage1D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CopyTexSubImage1DEXT(System.Graphics.OGL.TextureTarget target, Int32 level, Int32 xoffset, Int32 x, Int32 y, Int32 width);
			[CLSCompliant(false)]
			public static CopyTexSubImage1DEXT glCopyTexSubImage1DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CopyTexSubImage2D(System.Graphics.OGL.TextureTarget target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 x, Int32 y, Int32 width, Int32 height);
			[CLSCompliant(false)]
			public static CopyTexSubImage2D glCopyTexSubImage2D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CopyTexSubImage2DEXT(System.Graphics.OGL.TextureTarget target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 x, Int32 y, Int32 width, Int32 height);
			[CLSCompliant(false)]
			public static CopyTexSubImage2DEXT glCopyTexSubImage2DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CopyTexSubImage3D(System.Graphics.OGL.TextureTarget target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 x, Int32 y, Int32 width, Int32 height);
			[CLSCompliant(false)]
			public static CopyTexSubImage3D glCopyTexSubImage3D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CopyTexSubImage3DEXT(System.Graphics.OGL.TextureTarget target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 x, Int32 y, Int32 width, Int32 height);
			[CLSCompliant(false)]
			public static CopyTexSubImage3DEXT glCopyTexSubImage3DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CopyTextureImage1DEXT(UInt32 texture, System.Graphics.OGL.TextureTarget target, Int32 level, System.Graphics.OGL.ExtDirectStateAccess internalformat, Int32 x, Int32 y, Int32 width, Int32 border);
			[CLSCompliant(false)]
			public static CopyTextureImage1DEXT glCopyTextureImage1DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CopyTextureImage2DEXT(UInt32 texture, System.Graphics.OGL.TextureTarget target, Int32 level, System.Graphics.OGL.ExtDirectStateAccess internalformat, Int32 x, Int32 y, Int32 width, Int32 height, Int32 border);
			[CLSCompliant(false)]
			public static CopyTextureImage2DEXT glCopyTextureImage2DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CopyTextureSubImage1DEXT(UInt32 texture, System.Graphics.OGL.TextureTarget target, Int32 level, Int32 xoffset, Int32 x, Int32 y, Int32 width);
			[CLSCompliant(false)]
			public static CopyTextureSubImage1DEXT glCopyTextureSubImage1DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CopyTextureSubImage2DEXT(UInt32 texture, System.Graphics.OGL.TextureTarget target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 x, Int32 y, Int32 width, Int32 height);
			[CLSCompliant(false)]
			public static CopyTextureSubImage2DEXT glCopyTextureSubImage2DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CopyTextureSubImage3DEXT(UInt32 texture, System.Graphics.OGL.TextureTarget target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 x, Int32 y, Int32 width, Int32 height);
			[CLSCompliant(false)]
			public static CopyTextureSubImage3DEXT glCopyTextureSubImage3DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Int32 CreateProgram();
			[CLSCompliant(false)]
			public static CreateProgram glCreateProgram;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Int32 CreateProgramObjectARB();
			[CLSCompliant(false)]
			public static CreateProgramObjectARB glCreateProgramObjectARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Int32 CreateShader(System.Graphics.OGL.ShaderType type);
			[CLSCompliant(false)]
			public static CreateShader glCreateShader;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Int32 CreateShaderObjectARB(System.Graphics.OGL.ArbShaderObjects shaderType);
			[CLSCompliant(false)]
			public static CreateShaderObjectARB glCreateShaderObjectARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Int32 CreateShaderProgramEXT(System.Graphics.OGL.ExtSeparateShaderObjects type, String @string);
			[CLSCompliant(false)]
			public static CreateShaderProgramEXT glCreateShaderProgramEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Int32 CreateShaderProgramv(System.Graphics.OGL.ShaderType type, Int32 count, String[] strings);
			[CLSCompliant(false)]
			public static CreateShaderProgramv glCreateShaderProgramv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate IntPtr CreateSyncFromCLeventARB(IntPtr context, IntPtr @event, UInt32 flags);
			[CLSCompliant(false)]
			public static CreateSyncFromCLeventARB glCreateSyncFromCLeventARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CullFace(System.Graphics.OGL.CullFaceMode mode);
			[CLSCompliant(false)]
			public static CullFace glCullFace;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void CullParameterdvEXT(System.Graphics.OGL.ExtCullVertex pname, [OutAttribute] Double* @params);
			[CLSCompliant(false)]
			public unsafe static CullParameterdvEXT glCullParameterdvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void CullParameterfvEXT(System.Graphics.OGL.ExtCullVertex pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static CullParameterfvEXT glCullParameterfvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void CurrentPaletteMatrixARB(Int32 index);
			[CLSCompliant(false)]
			public static CurrentPaletteMatrixARB glCurrentPaletteMatrixARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DebugMessageCallbackAMD(DebugProcAmd callback, [OutAttribute] IntPtr userParam);
			[CLSCompliant(false)]
			public static DebugMessageCallbackAMD glDebugMessageCallbackAMD;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DebugMessageCallbackARB(DebugProcArb callback, IntPtr userParam);
			[CLSCompliant(false)]
			public static DebugMessageCallbackARB glDebugMessageCallbackARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DebugMessageControlARB(System.Graphics.OGL.ArbDebugOutput source, System.Graphics.OGL.ArbDebugOutput type, System.Graphics.OGL.ArbDebugOutput severity, Int32 count, UInt32* ids, bool enabled);
			[CLSCompliant(false)]
			public unsafe static DebugMessageControlARB glDebugMessageControlARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DebugMessageEnableAMD(System.Graphics.OGL.AmdDebugOutput category, System.Graphics.OGL.AmdDebugOutput severity, Int32 count, UInt32* ids, bool enabled);
			[CLSCompliant(false)]
			public unsafe static DebugMessageEnableAMD glDebugMessageEnableAMD;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DebugMessageInsertAMD(System.Graphics.OGL.AmdDebugOutput category, System.Graphics.OGL.AmdDebugOutput severity, UInt32 id, Int32 length, String buf);
			[CLSCompliant(false)]
			public static DebugMessageInsertAMD glDebugMessageInsertAMD;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DebugMessageInsertARB(System.Graphics.OGL.ArbDebugOutput source, System.Graphics.OGL.ArbDebugOutput type, UInt32 id, System.Graphics.OGL.ArbDebugOutput severity, Int32 length, String buf);
			[CLSCompliant(false)]
			public static DebugMessageInsertARB glDebugMessageInsertARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DeformationMap3dSGIX(System.Graphics.OGL.SgixPolynomialFfd target, Double u1, Double u2, Int32 ustride, Int32 uorder, Double v1, Double v2, Int32 vstride, Int32 vorder, Double w1, Double w2, Int32 wstride, Int32 worder, Double* points);
			[CLSCompliant(false)]
			public unsafe static DeformationMap3dSGIX glDeformationMap3dSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DeformationMap3fSGIX(System.Graphics.OGL.SgixPolynomialFfd target, Single u1, Single u2, Int32 ustride, Int32 uorder, Single v1, Single v2, Int32 vstride, Int32 vorder, Single w1, Single w2, Int32 wstride, Int32 worder, Single* points);
			[CLSCompliant(false)]
			public unsafe static DeformationMap3fSGIX glDeformationMap3fSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DeformSGIX(UInt32 mask);
			[CLSCompliant(false)]
			public static DeformSGIX glDeformSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DeleteAsyncMarkersSGIX(UInt32 marker, Int32 range);
			[CLSCompliant(false)]
			public static DeleteAsyncMarkersSGIX glDeleteAsyncMarkersSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DeleteBuffers(Int32 n, UInt32* buffers);
			[CLSCompliant(false)]
			public unsafe static DeleteBuffers glDeleteBuffers;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DeleteBuffersARB(Int32 n, UInt32* buffers);
			[CLSCompliant(false)]
			public unsafe static DeleteBuffersARB glDeleteBuffersARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DeleteFencesAPPLE(Int32 n, UInt32* fences);
			[CLSCompliant(false)]
			public unsafe static DeleteFencesAPPLE glDeleteFencesAPPLE;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DeleteFencesNV(Int32 n, UInt32* fences);
			[CLSCompliant(false)]
			public unsafe static DeleteFencesNV glDeleteFencesNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DeleteFragmentShaderATI(UInt32 id);
			[CLSCompliant(false)]
			public static DeleteFragmentShaderATI glDeleteFragmentShaderATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DeleteFramebuffers(Int32 n, UInt32* framebuffers);
			[CLSCompliant(false)]
			public unsafe static DeleteFramebuffers glDeleteFramebuffers;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DeleteFramebuffersEXT(Int32 n, UInt32* framebuffers);
			[CLSCompliant(false)]
			public unsafe static DeleteFramebuffersEXT glDeleteFramebuffersEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DeleteLists(UInt32 list, Int32 range);
			[CLSCompliant(false)]
			public static DeleteLists glDeleteLists;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DeleteNamedStringARB(Int32 namelen, String name);
			[CLSCompliant(false)]
			public static DeleteNamedStringARB glDeleteNamedStringARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DeleteNamesAMD(System.Graphics.OGL.AmdNameGenDelete identifier, UInt32 num, UInt32* names);
			[CLSCompliant(false)]
			public unsafe static DeleteNamesAMD glDeleteNamesAMD;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DeleteObjectARB(UInt32 obj);
			[CLSCompliant(false)]
			public static DeleteObjectARB glDeleteObjectARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DeleteOcclusionQueriesNV(Int32 n, UInt32* ids);
			[CLSCompliant(false)]
			public unsafe static DeleteOcclusionQueriesNV glDeleteOcclusionQueriesNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DeletePerfMonitorsAMD(Int32 n, [OutAttribute] UInt32* monitors);
			[CLSCompliant(false)]
			public unsafe static DeletePerfMonitorsAMD glDeletePerfMonitorsAMD;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DeleteProgram(UInt32 program);
			[CLSCompliant(false)]
			public static DeleteProgram glDeleteProgram;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DeleteProgramPipelines(Int32 n, UInt32* pipelines);
			[CLSCompliant(false)]
			public unsafe static DeleteProgramPipelines glDeleteProgramPipelines;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DeleteProgramsARB(Int32 n, UInt32* programs);
			[CLSCompliant(false)]
			public unsafe static DeleteProgramsARB glDeleteProgramsARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DeleteProgramsNV(Int32 n, UInt32* programs);
			[CLSCompliant(false)]
			public unsafe static DeleteProgramsNV glDeleteProgramsNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DeleteQueries(Int32 n, UInt32* ids);
			[CLSCompliant(false)]
			public unsafe static DeleteQueries glDeleteQueries;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DeleteQueriesARB(Int32 n, UInt32* ids);
			[CLSCompliant(false)]
			public unsafe static DeleteQueriesARB glDeleteQueriesARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DeleteRenderbuffers(Int32 n, UInt32* renderbuffers);
			[CLSCompliant(false)]
			public unsafe static DeleteRenderbuffers glDeleteRenderbuffers;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DeleteRenderbuffersEXT(Int32 n, UInt32* renderbuffers);
			[CLSCompliant(false)]
			public unsafe static DeleteRenderbuffersEXT glDeleteRenderbuffersEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DeleteSamplers(Int32 count, UInt32* samplers);
			[CLSCompliant(false)]
			public unsafe static DeleteSamplers glDeleteSamplers;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DeleteShader(UInt32 shader);
			[CLSCompliant(false)]
			public static DeleteShader glDeleteShader;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DeleteSync(IntPtr sync);
			[CLSCompliant(false)]
			public static DeleteSync glDeleteSync;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DeleteTextures(Int32 n, UInt32* textures);
			[CLSCompliant(false)]
			public unsafe static DeleteTextures glDeleteTextures;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DeleteTexturesEXT(Int32 n, UInt32* textures);
			[CLSCompliant(false)]
			public unsafe static DeleteTexturesEXT glDeleteTexturesEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DeleteTransformFeedbacks(Int32 n, UInt32* ids);
			[CLSCompliant(false)]
			public unsafe static DeleteTransformFeedbacks glDeleteTransformFeedbacks;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DeleteTransformFeedbacksNV(Int32 n, UInt32* ids);
			[CLSCompliant(false)]
			public unsafe static DeleteTransformFeedbacksNV glDeleteTransformFeedbacksNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DeleteVertexArrays(Int32 n, UInt32* arrays);
			[CLSCompliant(false)]
			public unsafe static DeleteVertexArrays glDeleteVertexArrays;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DeleteVertexArraysAPPLE(Int32 n, UInt32* arrays);
			[CLSCompliant(false)]
			public unsafe static DeleteVertexArraysAPPLE glDeleteVertexArraysAPPLE;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DeleteVertexShaderEXT(UInt32 id);
			[CLSCompliant(false)]
			public static DeleteVertexShaderEXT glDeleteVertexShaderEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DepthBoundsdNV(Double zmin, Double zmax);
			[CLSCompliant(false)]
			public static DepthBoundsdNV glDepthBoundsdNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DepthBoundsEXT(Double zmin, Double zmax);
			[CLSCompliant(false)]
			public static DepthBoundsEXT glDepthBoundsEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DepthFunc(System.Graphics.OGL.DepthFunction func);
			[CLSCompliant(false)]
			public static DepthFunc glDepthFunc;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DepthMask(bool flag);
			[CLSCompliant(false)]
			public static DepthMask glDepthMask;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DepthRange(Double near, Double far);
			[CLSCompliant(false)]
			public static DepthRange glDepthRange;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DepthRangeArrayv(UInt32 first, Int32 count, Double* v);
			[CLSCompliant(false)]
			public unsafe static DepthRangeArrayv glDepthRangeArrayv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DepthRangedNV(Double zNear, Double zFar);
			[CLSCompliant(false)]
			public static DepthRangedNV glDepthRangedNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DepthRangef(Single n, Single f);
			[CLSCompliant(false)]
			public static DepthRangef glDepthRangef;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DepthRangeIndexed(UInt32 index, Double n, Double f);
			[CLSCompliant(false)]
			public static DepthRangeIndexed glDepthRangeIndexed;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DetachObjectARB(UInt32 containerObj, UInt32 attachedObj);
			[CLSCompliant(false)]
			public static DetachObjectARB glDetachObjectARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DetachShader(UInt32 program, UInt32 shader);
			[CLSCompliant(false)]
			public static DetachShader glDetachShader;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DetailTexFuncSGIS(System.Graphics.OGL.TextureTarget target, Int32 n, Single* points);
			[CLSCompliant(false)]
			public unsafe static DetailTexFuncSGIS glDetailTexFuncSGIS;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Disable(System.Graphics.OGL.EnableCap cap);
			[CLSCompliant(false)]
			public static Disable glDisable;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DisableClientState(System.Graphics.OGL.ArrayCap array);
			[CLSCompliant(false)]
			public static DisableClientState glDisableClientState;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DisableClientStateIndexedEXT(System.Graphics.OGL.EnableCap array, UInt32 index);
			[CLSCompliant(false)]
			public static DisableClientStateIndexedEXT glDisableClientStateIndexedEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Disablei(System.Graphics.OGL.IndexedEnableCap target, UInt32 index);
			[CLSCompliant(false)]
			public static Disablei glDisablei;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DisableIndexedEXT(System.Graphics.OGL.IndexedEnableCap target, UInt32 index);
			[CLSCompliant(false)]
			public static DisableIndexedEXT glDisableIndexedEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DisableVariantClientStateEXT(UInt32 id);
			[CLSCompliant(false)]
			public static DisableVariantClientStateEXT glDisableVariantClientStateEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DisableVertexAttribAPPLE(UInt32 index, System.Graphics.OGL.AppleVertexProgramEvaluators pname);
			[CLSCompliant(false)]
			public static DisableVertexAttribAPPLE glDisableVertexAttribAPPLE;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DisableVertexAttribArray(UInt32 index);
			[CLSCompliant(false)]
			public static DisableVertexAttribArray glDisableVertexAttribArray;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DisableVertexAttribArrayARB(UInt32 index);
			[CLSCompliant(false)]
			public static DisableVertexAttribArrayARB glDisableVertexAttribArrayARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DrawArrays(System.Graphics.OGL.BeginMode mode, Int32 first, Int32 count);
			[CLSCompliant(false)]
			public static DrawArrays glDrawArrays;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DrawArraysEXT(System.Graphics.OGL.BeginMode mode, Int32 first, Int32 count);
			[CLSCompliant(false)]
			public static DrawArraysEXT glDrawArraysEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DrawArraysIndirect(System.Graphics.OGL.ArbDrawIndirect mode, IntPtr indirect);
			[CLSCompliant(false)]
			public static DrawArraysIndirect glDrawArraysIndirect;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DrawArraysInstanced(System.Graphics.OGL.BeginMode mode, Int32 first, Int32 count, Int32 primcount);
			[CLSCompliant(false)]
			public static DrawArraysInstanced glDrawArraysInstanced;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DrawArraysInstancedARB(System.Graphics.OGL.BeginMode mode, Int32 first, Int32 count, Int32 primcount);
			[CLSCompliant(false)]
			public static DrawArraysInstancedARB glDrawArraysInstancedARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DrawArraysInstancedEXT(System.Graphics.OGL.BeginMode mode, Int32 start, Int32 count, Int32 primcount);
			[CLSCompliant(false)]
			public static DrawArraysInstancedEXT glDrawArraysInstancedEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DrawBuffer(System.Graphics.OGL.DrawBufferMode mode);
			[CLSCompliant(false)]
			public static DrawBuffer glDrawBuffer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DrawBuffers(Int32 n, System.Graphics.OGL.DrawBuffersEnum* bufs);
			[CLSCompliant(false)]
			public unsafe static DrawBuffers glDrawBuffers;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DrawBuffersARB(Int32 n, System.Graphics.OGL.ArbDrawBuffers* bufs);
			[CLSCompliant(false)]
			public unsafe static DrawBuffersARB glDrawBuffersARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void DrawBuffersATI(Int32 n, System.Graphics.OGL.AtiDrawBuffers* bufs);
			[CLSCompliant(false)]
			public unsafe static DrawBuffersATI glDrawBuffersATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DrawElementArrayAPPLE(System.Graphics.OGL.BeginMode mode, Int32 first, Int32 count);
			[CLSCompliant(false)]
			public static DrawElementArrayAPPLE glDrawElementArrayAPPLE;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DrawElementArrayATI(System.Graphics.OGL.BeginMode mode, Int32 count);
			[CLSCompliant(false)]
			public static DrawElementArrayATI glDrawElementArrayATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DrawElements(System.Graphics.OGL.BeginMode mode, Int32 count, System.Graphics.OGL.DrawElementsType type, IntPtr indices);
			[CLSCompliant(false)]
			public static DrawElements glDrawElements;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DrawElementsBaseVertex(System.Graphics.OGL.BeginMode mode, Int32 count, System.Graphics.OGL.DrawElementsType type, IntPtr indices, Int32 basevertex);
			[CLSCompliant(false)]
			public static DrawElementsBaseVertex glDrawElementsBaseVertex;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DrawElementsIndirect(System.Graphics.OGL.ArbDrawIndirect mode, System.Graphics.OGL.ArbDrawIndirect type, IntPtr indirect);
			[CLSCompliant(false)]
			public static DrawElementsIndirect glDrawElementsIndirect;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DrawElementsInstanced(System.Graphics.OGL.BeginMode mode, Int32 count, System.Graphics.OGL.DrawElementsType type, IntPtr indices, Int32 primcount);
			[CLSCompliant(false)]
			public static DrawElementsInstanced glDrawElementsInstanced;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DrawElementsInstancedARB(System.Graphics.OGL.BeginMode mode, Int32 count, System.Graphics.OGL.DrawElementsType type, IntPtr indices, Int32 primcount);
			[CLSCompliant(false)]
			public static DrawElementsInstancedARB glDrawElementsInstancedARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DrawElementsInstancedBaseVertex(System.Graphics.OGL.BeginMode mode, Int32 count, System.Graphics.OGL.DrawElementsType type, IntPtr indices, Int32 primcount, Int32 basevertex);
			[CLSCompliant(false)]
			public static DrawElementsInstancedBaseVertex glDrawElementsInstancedBaseVertex;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DrawElementsInstancedEXT(System.Graphics.OGL.BeginMode mode, Int32 count, System.Graphics.OGL.DrawElementsType type, IntPtr indices, Int32 primcount);
			[CLSCompliant(false)]
			public static DrawElementsInstancedEXT glDrawElementsInstancedEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DrawMeshArraysSUN(System.Graphics.OGL.BeginMode mode, Int32 first, Int32 count, Int32 width);
			[CLSCompliant(false)]
			public static DrawMeshArraysSUN glDrawMeshArraysSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DrawPixels(Int32 width, Int32 height, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, IntPtr pixels);
			[CLSCompliant(false)]
			public static DrawPixels glDrawPixels;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DrawRangeElementArrayAPPLE(System.Graphics.OGL.BeginMode mode, UInt32 start, UInt32 end, Int32 first, Int32 count);
			[CLSCompliant(false)]
			public static DrawRangeElementArrayAPPLE glDrawRangeElementArrayAPPLE;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DrawRangeElementArrayATI(System.Graphics.OGL.BeginMode mode, UInt32 start, UInt32 end, Int32 count);
			[CLSCompliant(false)]
			public static DrawRangeElementArrayATI glDrawRangeElementArrayATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DrawRangeElements(System.Graphics.OGL.BeginMode mode, UInt32 start, UInt32 end, Int32 count, System.Graphics.OGL.DrawElementsType type, IntPtr indices);
			[CLSCompliant(false)]
			public static DrawRangeElements glDrawRangeElements;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DrawRangeElementsBaseVertex(System.Graphics.OGL.BeginMode mode, UInt32 start, UInt32 end, Int32 count, System.Graphics.OGL.DrawElementsType type, IntPtr indices, Int32 basevertex);
			[CLSCompliant(false)]
			public static DrawRangeElementsBaseVertex glDrawRangeElementsBaseVertex;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DrawRangeElementsEXT(System.Graphics.OGL.BeginMode mode, UInt32 start, UInt32 end, Int32 count, System.Graphics.OGL.DrawElementsType type, IntPtr indices);
			[CLSCompliant(false)]
			public static DrawRangeElementsEXT glDrawRangeElementsEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DrawTransformFeedback(System.Graphics.OGL.BeginMode mode, UInt32 id);
			[CLSCompliant(false)]
			public static DrawTransformFeedback glDrawTransformFeedback;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DrawTransformFeedbackNV(System.Graphics.OGL.NvTransformFeedback2 mode, UInt32 id);
			[CLSCompliant(false)]
			public static DrawTransformFeedbackNV glDrawTransformFeedbackNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void DrawTransformFeedbackStream(System.Graphics.OGL.BeginMode mode, UInt32 id, UInt32 stream);
			[CLSCompliant(false)]
			public static DrawTransformFeedbackStream glDrawTransformFeedbackStream;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void EdgeFlag(bool flag);
			[CLSCompliant(false)]
			public static EdgeFlag glEdgeFlag;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void EdgeFlagFormatNV(Int32 stride);
			[CLSCompliant(false)]
			public static EdgeFlagFormatNV glEdgeFlagFormatNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void EdgeFlagPointer(Int32 stride, IntPtr pointer);
			[CLSCompliant(false)]
			public static EdgeFlagPointer glEdgeFlagPointer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void EdgeFlagPointerEXT(Int32 stride, Int32 count, bool* pointer);
			[CLSCompliant(false)]
			public unsafe static EdgeFlagPointerEXT glEdgeFlagPointerEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void EdgeFlagPointerListIBM(Int32 stride, bool* pointer, Int32 ptrstride);
			[CLSCompliant(false)]
			public unsafe static EdgeFlagPointerListIBM glEdgeFlagPointerListIBM;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void EdgeFlagv(bool* flag);
			[CLSCompliant(false)]
			public unsafe static EdgeFlagv glEdgeFlagv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ElementPointerAPPLE(System.Graphics.OGL.AppleElementArray type, IntPtr pointer);
			[CLSCompliant(false)]
			public static ElementPointerAPPLE glElementPointerAPPLE;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ElementPointerATI(System.Graphics.OGL.AtiElementArray type, IntPtr pointer);
			[CLSCompliant(false)]
			public static ElementPointerATI glElementPointerATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Enable(System.Graphics.OGL.EnableCap cap);
			[CLSCompliant(false)]
			public static Enable glEnable;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void EnableClientState(System.Graphics.OGL.ArrayCap array);
			[CLSCompliant(false)]
			public static EnableClientState glEnableClientState;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void EnableClientStateIndexedEXT(System.Graphics.OGL.EnableCap array, UInt32 index);
			[CLSCompliant(false)]
			public static EnableClientStateIndexedEXT glEnableClientStateIndexedEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Enablei(System.Graphics.OGL.IndexedEnableCap target, UInt32 index);
			[CLSCompliant(false)]
			public static Enablei glEnablei;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void EnableIndexedEXT(System.Graphics.OGL.IndexedEnableCap target, UInt32 index);
			[CLSCompliant(false)]
			public static EnableIndexedEXT glEnableIndexedEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void EnableVariantClientStateEXT(UInt32 id);
			[CLSCompliant(false)]
			public static EnableVariantClientStateEXT glEnableVariantClientStateEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void EnableVertexAttribAPPLE(UInt32 index, System.Graphics.OGL.AppleVertexProgramEvaluators pname);
			[CLSCompliant(false)]
			public static EnableVertexAttribAPPLE glEnableVertexAttribAPPLE;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void EnableVertexAttribArray(UInt32 index);
			[CLSCompliant(false)]
			public static EnableVertexAttribArray glEnableVertexAttribArray;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void EnableVertexAttribArrayARB(UInt32 index);
			[CLSCompliant(false)]
			public static EnableVertexAttribArrayARB glEnableVertexAttribArrayARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void End();
			[CLSCompliant(false)]
			public static End glEnd;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void EndConditionalRender();
			[CLSCompliant(false)]
			public static EndConditionalRender glEndConditionalRender;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void EndConditionalRenderNV();
			[CLSCompliant(false)]
			public static EndConditionalRenderNV glEndConditionalRenderNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void EndFragmentShaderATI();
			[CLSCompliant(false)]
			public static EndFragmentShaderATI glEndFragmentShaderATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void EndList();
			[CLSCompliant(false)]
			public static EndList glEndList;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void EndOcclusionQueryNV();
			[CLSCompliant(false)]
			public static EndOcclusionQueryNV glEndOcclusionQueryNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void EndPerfMonitorAMD(UInt32 monitor);
			[CLSCompliant(false)]
			public static EndPerfMonitorAMD glEndPerfMonitorAMD;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void EndQuery(System.Graphics.OGL.QueryTarget target);
			[CLSCompliant(false)]
			public static EndQuery glEndQuery;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void EndQueryARB(System.Graphics.OGL.ArbOcclusionQuery target);
			[CLSCompliant(false)]
			public static EndQueryARB glEndQueryARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void EndQueryIndexed(System.Graphics.OGL.QueryTarget target, UInt32 index);
			[CLSCompliant(false)]
			public static EndQueryIndexed glEndQueryIndexed;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void EndTransformFeedback();
			[CLSCompliant(false)]
			public static EndTransformFeedback glEndTransformFeedback;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void EndTransformFeedbackEXT();
			[CLSCompliant(false)]
			public static EndTransformFeedbackEXT glEndTransformFeedbackEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void EndTransformFeedbackNV();
			[CLSCompliant(false)]
			public static EndTransformFeedbackNV glEndTransformFeedbackNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void EndVertexShaderEXT();
			[CLSCompliant(false)]
			public static EndVertexShaderEXT glEndVertexShaderEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void EndVideoCaptureNV(UInt32 video_capture_slot);
			[CLSCompliant(false)]
			public static EndVideoCaptureNV glEndVideoCaptureNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void EvalCoord1d(Double u);
			[CLSCompliant(false)]
			public static EvalCoord1d glEvalCoord1d;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void EvalCoord1dv(Double* u);
			[CLSCompliant(false)]
			public unsafe static EvalCoord1dv glEvalCoord1dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void EvalCoord1f(Single u);
			[CLSCompliant(false)]
			public static EvalCoord1f glEvalCoord1f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void EvalCoord1fv(Single* u);
			[CLSCompliant(false)]
			public unsafe static EvalCoord1fv glEvalCoord1fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void EvalCoord2d(Double u, Double v);
			[CLSCompliant(false)]
			public static EvalCoord2d glEvalCoord2d;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void EvalCoord2dv(Double* u);
			[CLSCompliant(false)]
			public unsafe static EvalCoord2dv glEvalCoord2dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void EvalCoord2f(Single u, Single v);
			[CLSCompliant(false)]
			public static EvalCoord2f glEvalCoord2f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void EvalCoord2fv(Single* u);
			[CLSCompliant(false)]
			public unsafe static EvalCoord2fv glEvalCoord2fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void EvalMapsNV(System.Graphics.OGL.NvEvaluators target, System.Graphics.OGL.NvEvaluators mode);
			[CLSCompliant(false)]
			public static EvalMapsNV glEvalMapsNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void EvalMesh1(System.Graphics.OGL.MeshMode1 mode, Int32 i1, Int32 i2);
			[CLSCompliant(false)]
			public static EvalMesh1 glEvalMesh1;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void EvalMesh2(System.Graphics.OGL.MeshMode2 mode, Int32 i1, Int32 i2, Int32 j1, Int32 j2);
			[CLSCompliant(false)]
			public static EvalMesh2 glEvalMesh2;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void EvalPoint1(Int32 i);
			[CLSCompliant(false)]
			public static EvalPoint1 glEvalPoint1;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void EvalPoint2(Int32 i, Int32 j);
			[CLSCompliant(false)]
			public static EvalPoint2 glEvalPoint2;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ExecuteProgramNV(System.Graphics.OGL.AssemblyProgramTargetArb target, UInt32 id, Single* @params);
			[CLSCompliant(false)]
			public unsafe static ExecuteProgramNV glExecuteProgramNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ExtractComponentEXT(UInt32 res, UInt32 src, UInt32 num);
			[CLSCompliant(false)]
			public static ExtractComponentEXT glExtractComponentEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void FeedbackBuffer(Int32 size, System.Graphics.OGL.FeedbackType type, [OutAttribute] Single* buffer);
			[CLSCompliant(false)]
			public unsafe static FeedbackBuffer glFeedbackBuffer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate IntPtr FenceSync(System.Graphics.OGL.ArbSync condition, UInt32 flags);
			[CLSCompliant(false)]
			public static FenceSync glFenceSync;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FinalCombinerInputNV(System.Graphics.OGL.NvRegisterCombiners variable, System.Graphics.OGL.NvRegisterCombiners input, System.Graphics.OGL.NvRegisterCombiners mapping, System.Graphics.OGL.NvRegisterCombiners componentUsage);
			[CLSCompliant(false)]
			public static FinalCombinerInputNV glFinalCombinerInputNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Finish();
			[CLSCompliant(false)]
			public static Finish glFinish;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate Int32 FinishAsyncSGIX([OutAttribute] UInt32* markerp);
			[CLSCompliant(false)]
			public unsafe static FinishAsyncSGIX glFinishAsyncSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FinishFenceAPPLE(UInt32 fence);
			[CLSCompliant(false)]
			public static FinishFenceAPPLE glFinishFenceAPPLE;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FinishFenceNV(UInt32 fence);
			[CLSCompliant(false)]
			public static FinishFenceNV glFinishFenceNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FinishObjectAPPLE(System.Graphics.OGL.AppleFence @object, Int32 name);
			[CLSCompliant(false)]
			public static FinishObjectAPPLE glFinishObjectAPPLE;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FinishTextureSUNX();
			[CLSCompliant(false)]
			public static FinishTextureSUNX glFinishTextureSUNX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Flush();
			[CLSCompliant(false)]
			public static Flush glFlush;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FlushMappedBufferRange(System.Graphics.OGL.BufferTarget target, IntPtr offset, IntPtr length);
			[CLSCompliant(false)]
			public static FlushMappedBufferRange glFlushMappedBufferRange;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FlushMappedBufferRangeAPPLE(System.Graphics.OGL.BufferTarget target, IntPtr offset, IntPtr size);
			[CLSCompliant(false)]
			public static FlushMappedBufferRangeAPPLE glFlushMappedBufferRangeAPPLE;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FlushMappedNamedBufferRangeEXT(UInt32 buffer, IntPtr offset, IntPtr length);
			[CLSCompliant(false)]
			public static FlushMappedNamedBufferRangeEXT glFlushMappedNamedBufferRangeEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FlushPixelDataRangeNV(System.Graphics.OGL.NvPixelDataRange target);
			[CLSCompliant(false)]
			public static FlushPixelDataRangeNV glFlushPixelDataRangeNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FlushRasterSGIX();
			[CLSCompliant(false)]
			public static FlushRasterSGIX glFlushRasterSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FlushVertexArrayRangeAPPLE(Int32 length, [OutAttribute] IntPtr pointer);
			[CLSCompliant(false)]
			public static FlushVertexArrayRangeAPPLE glFlushVertexArrayRangeAPPLE;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FlushVertexArrayRangeNV();
			[CLSCompliant(false)]
			public static FlushVertexArrayRangeNV glFlushVertexArrayRangeNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FogCoordd(Double coord);
			[CLSCompliant(false)]
			public static FogCoordd glFogCoordd;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FogCoorddEXT(Double coord);
			[CLSCompliant(false)]
			public static FogCoorddEXT glFogCoorddEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void FogCoorddv(Double* coord);
			[CLSCompliant(false)]
			public unsafe static FogCoorddv glFogCoorddv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void FogCoorddvEXT(Double* coord);
			[CLSCompliant(false)]
			public unsafe static FogCoorddvEXT glFogCoorddvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FogCoordf(Single coord);
			[CLSCompliant(false)]
			public static FogCoordf glFogCoordf;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FogCoordfEXT(Single coord);
			[CLSCompliant(false)]
			public static FogCoordfEXT glFogCoordfEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FogCoordFormatNV(System.Graphics.OGL.NvVertexBufferUnifiedMemory type, Int32 stride);
			[CLSCompliant(false)]
			public static FogCoordFormatNV glFogCoordFormatNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void FogCoordfv(Single* coord);
			[CLSCompliant(false)]
			public unsafe static FogCoordfv glFogCoordfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void FogCoordfvEXT(Single* coord);
			[CLSCompliant(false)]
			public unsafe static FogCoordfvEXT glFogCoordfvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FogCoordhNV(Half fog);
			[CLSCompliant(false)]
			public static FogCoordhNV glFogCoordhNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void FogCoordhvNV(Half* fog);
			[CLSCompliant(false)]
			public unsafe static FogCoordhvNV glFogCoordhvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FogCoordPointer(System.Graphics.OGL.FogPointerType type, Int32 stride, IntPtr pointer);
			[CLSCompliant(false)]
			public static FogCoordPointer glFogCoordPointer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FogCoordPointerEXT(System.Graphics.OGL.ExtFogCoord type, Int32 stride, IntPtr pointer);
			[CLSCompliant(false)]
			public static FogCoordPointerEXT glFogCoordPointerEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FogCoordPointerListIBM(System.Graphics.OGL.IbmVertexArrayLists type, Int32 stride, IntPtr pointer, Int32 ptrstride);
			[CLSCompliant(false)]
			public static FogCoordPointerListIBM glFogCoordPointerListIBM;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Fogf(System.Graphics.OGL.FogParameter pname, Single param);
			[CLSCompliant(false)]
			public static Fogf glFogf;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void FogFuncSGIS(Int32 n, Single* points);
			[CLSCompliant(false)]
			public unsafe static FogFuncSGIS glFogFuncSGIS;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Fogfv(System.Graphics.OGL.FogParameter pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static Fogfv glFogfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Fogi(System.Graphics.OGL.FogParameter pname, Int32 param);
			[CLSCompliant(false)]
			public static Fogi glFogi;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Fogiv(System.Graphics.OGL.FogParameter pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static Fogiv glFogiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FragmentColorMaterialSGIX(System.Graphics.OGL.MaterialFace face, System.Graphics.OGL.MaterialParameter mode);
			[CLSCompliant(false)]
			public static FragmentColorMaterialSGIX glFragmentColorMaterialSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FragmentLightfSGIX(System.Graphics.OGL.SgixFragmentLighting light, System.Graphics.OGL.SgixFragmentLighting pname, Single param);
			[CLSCompliant(false)]
			public static FragmentLightfSGIX glFragmentLightfSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void FragmentLightfvSGIX(System.Graphics.OGL.SgixFragmentLighting light, System.Graphics.OGL.SgixFragmentLighting pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static FragmentLightfvSGIX glFragmentLightfvSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FragmentLightiSGIX(System.Graphics.OGL.SgixFragmentLighting light, System.Graphics.OGL.SgixFragmentLighting pname, Int32 param);
			[CLSCompliant(false)]
			public static FragmentLightiSGIX glFragmentLightiSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void FragmentLightivSGIX(System.Graphics.OGL.SgixFragmentLighting light, System.Graphics.OGL.SgixFragmentLighting pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static FragmentLightivSGIX glFragmentLightivSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FragmentLightModelfSGIX(System.Graphics.OGL.SgixFragmentLighting pname, Single param);
			[CLSCompliant(false)]
			public static FragmentLightModelfSGIX glFragmentLightModelfSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void FragmentLightModelfvSGIX(System.Graphics.OGL.SgixFragmentLighting pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static FragmentLightModelfvSGIX glFragmentLightModelfvSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FragmentLightModeliSGIX(System.Graphics.OGL.SgixFragmentLighting pname, Int32 param);
			[CLSCompliant(false)]
			public static FragmentLightModeliSGIX glFragmentLightModeliSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void FragmentLightModelivSGIX(System.Graphics.OGL.SgixFragmentLighting pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static FragmentLightModelivSGIX glFragmentLightModelivSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FragmentMaterialfSGIX(System.Graphics.OGL.MaterialFace face, System.Graphics.OGL.MaterialParameter pname, Single param);
			[CLSCompliant(false)]
			public static FragmentMaterialfSGIX glFragmentMaterialfSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void FragmentMaterialfvSGIX(System.Graphics.OGL.MaterialFace face, System.Graphics.OGL.MaterialParameter pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static FragmentMaterialfvSGIX glFragmentMaterialfvSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FragmentMaterialiSGIX(System.Graphics.OGL.MaterialFace face, System.Graphics.OGL.MaterialParameter pname, Int32 param);
			[CLSCompliant(false)]
			public static FragmentMaterialiSGIX glFragmentMaterialiSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void FragmentMaterialivSGIX(System.Graphics.OGL.MaterialFace face, System.Graphics.OGL.MaterialParameter pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static FragmentMaterialivSGIX glFragmentMaterialivSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FramebufferDrawBufferEXT(UInt32 framebuffer, System.Graphics.OGL.DrawBufferMode mode);
			[CLSCompliant(false)]
			public static FramebufferDrawBufferEXT glFramebufferDrawBufferEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void FramebufferDrawBuffersEXT(UInt32 framebuffer, Int32 n, System.Graphics.OGL.DrawBufferMode* bufs);
			[CLSCompliant(false)]
			public unsafe static FramebufferDrawBuffersEXT glFramebufferDrawBuffersEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FramebufferReadBufferEXT(UInt32 framebuffer, System.Graphics.OGL.ReadBufferMode mode);
			[CLSCompliant(false)]
			public static FramebufferReadBufferEXT glFramebufferReadBufferEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FramebufferRenderbuffer(System.Graphics.OGL.FramebufferTarget target, System.Graphics.OGL.FramebufferAttachment attachment, System.Graphics.OGL.RenderbufferTarget renderbuffertarget, UInt32 renderbuffer);
			[CLSCompliant(false)]
			public static FramebufferRenderbuffer glFramebufferRenderbuffer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FramebufferRenderbufferEXT(System.Graphics.OGL.FramebufferTarget target, System.Graphics.OGL.FramebufferAttachment attachment, System.Graphics.OGL.RenderbufferTarget renderbuffertarget, UInt32 renderbuffer);
			[CLSCompliant(false)]
			public static FramebufferRenderbufferEXT glFramebufferRenderbufferEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FramebufferTexture(System.Graphics.OGL.FramebufferTarget target, System.Graphics.OGL.FramebufferAttachment attachment, UInt32 texture, Int32 level);
			[CLSCompliant(false)]
			public static FramebufferTexture glFramebufferTexture;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FramebufferTexture1D(System.Graphics.OGL.FramebufferTarget target, System.Graphics.OGL.FramebufferAttachment attachment, System.Graphics.OGL.TextureTarget textarget, UInt32 texture, Int32 level);
			[CLSCompliant(false)]
			public static FramebufferTexture1D glFramebufferTexture1D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FramebufferTexture1DEXT(System.Graphics.OGL.FramebufferTarget target, System.Graphics.OGL.FramebufferAttachment attachment, System.Graphics.OGL.TextureTarget textarget, UInt32 texture, Int32 level);
			[CLSCompliant(false)]
			public static FramebufferTexture1DEXT glFramebufferTexture1DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FramebufferTexture2D(System.Graphics.OGL.FramebufferTarget target, System.Graphics.OGL.FramebufferAttachment attachment, System.Graphics.OGL.TextureTarget textarget, UInt32 texture, Int32 level);
			[CLSCompliant(false)]
			public static FramebufferTexture2D glFramebufferTexture2D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FramebufferTexture2DEXT(System.Graphics.OGL.FramebufferTarget target, System.Graphics.OGL.FramebufferAttachment attachment, System.Graphics.OGL.TextureTarget textarget, UInt32 texture, Int32 level);
			[CLSCompliant(false)]
			public static FramebufferTexture2DEXT glFramebufferTexture2DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FramebufferTexture3D(System.Graphics.OGL.FramebufferTarget target, System.Graphics.OGL.FramebufferAttachment attachment, System.Graphics.OGL.TextureTarget textarget, UInt32 texture, Int32 level, Int32 zoffset);
			[CLSCompliant(false)]
			public static FramebufferTexture3D glFramebufferTexture3D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FramebufferTexture3DEXT(System.Graphics.OGL.FramebufferTarget target, System.Graphics.OGL.FramebufferAttachment attachment, System.Graphics.OGL.TextureTarget textarget, UInt32 texture, Int32 level, Int32 zoffset);
			[CLSCompliant(false)]
			public static FramebufferTexture3DEXT glFramebufferTexture3DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FramebufferTextureARB(System.Graphics.OGL.FramebufferTarget target, System.Graphics.OGL.FramebufferAttachment attachment, UInt32 texture, Int32 level);
			[CLSCompliant(false)]
			public static FramebufferTextureARB glFramebufferTextureARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FramebufferTextureEXT(System.Graphics.OGL.FramebufferTarget target, System.Graphics.OGL.FramebufferAttachment attachment, UInt32 texture, Int32 level);
			[CLSCompliant(false)]
			public static FramebufferTextureEXT glFramebufferTextureEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FramebufferTextureFaceARB(System.Graphics.OGL.FramebufferTarget target, System.Graphics.OGL.FramebufferAttachment attachment, UInt32 texture, Int32 level, System.Graphics.OGL.TextureTarget face);
			[CLSCompliant(false)]
			public static FramebufferTextureFaceARB glFramebufferTextureFaceARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FramebufferTextureFaceEXT(System.Graphics.OGL.FramebufferTarget target, System.Graphics.OGL.FramebufferAttachment attachment, UInt32 texture, Int32 level, System.Graphics.OGL.TextureTarget face);
			[CLSCompliant(false)]
			public static FramebufferTextureFaceEXT glFramebufferTextureFaceEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FramebufferTextureLayer(System.Graphics.OGL.FramebufferTarget target, System.Graphics.OGL.FramebufferAttachment attachment, UInt32 texture, Int32 level, Int32 layer);
			[CLSCompliant(false)]
			public static FramebufferTextureLayer glFramebufferTextureLayer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FramebufferTextureLayerARB(System.Graphics.OGL.FramebufferTarget target, System.Graphics.OGL.FramebufferAttachment attachment, UInt32 texture, Int32 level, Int32 layer);
			[CLSCompliant(false)]
			public static FramebufferTextureLayerARB glFramebufferTextureLayerARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FramebufferTextureLayerEXT(System.Graphics.OGL.FramebufferTarget target, System.Graphics.OGL.FramebufferAttachment attachment, UInt32 texture, Int32 level, Int32 layer);
			[CLSCompliant(false)]
			public static FramebufferTextureLayerEXT glFramebufferTextureLayerEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FrameTerminatorGREMEDY();
			[CLSCompliant(false)]
			public static FrameTerminatorGREMEDY glFrameTerminatorGREMEDY;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FrameZoomSGIX(Int32 factor);
			[CLSCompliant(false)]
			public static FrameZoomSGIX glFrameZoomSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FreeObjectBufferATI(UInt32 buffer);
			[CLSCompliant(false)]
			public static FreeObjectBufferATI glFreeObjectBufferATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void FrontFace(System.Graphics.OGL.FrontFaceDirection mode);
			[CLSCompliant(false)]
			public static FrontFace glFrontFace;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Frustum(Double left, Double right, Double bottom, Double top, Double zNear, Double zFar);
			[CLSCompliant(false)]
			public static Frustum glFrustum;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Int32 GenAsyncMarkersSGIX(Int32 range);
			[CLSCompliant(false)]
			public static GenAsyncMarkersSGIX glGenAsyncMarkersSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GenBuffers(Int32 n, [OutAttribute] UInt32* buffers);
			[CLSCompliant(false)]
			public unsafe static GenBuffers glGenBuffers;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GenBuffersARB(Int32 n, [OutAttribute] UInt32* buffers);
			[CLSCompliant(false)]
			public unsafe static GenBuffersARB glGenBuffersARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GenerateMipmap(System.Graphics.OGL.GenerateMipmapTarget target);
			[CLSCompliant(false)]
			public static GenerateMipmap glGenerateMipmap;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GenerateMipmapEXT(System.Graphics.OGL.GenerateMipmapTarget target);
			[CLSCompliant(false)]
			public static GenerateMipmapEXT glGenerateMipmapEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GenerateMultiTexMipmapEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureTarget target);
			[CLSCompliant(false)]
			public static GenerateMultiTexMipmapEXT glGenerateMultiTexMipmapEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GenerateTextureMipmapEXT(UInt32 texture, System.Graphics.OGL.TextureTarget target);
			[CLSCompliant(false)]
			public static GenerateTextureMipmapEXT glGenerateTextureMipmapEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GenFencesAPPLE(Int32 n, [OutAttribute] UInt32* fences);
			[CLSCompliant(false)]
			public unsafe static GenFencesAPPLE glGenFencesAPPLE;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GenFencesNV(Int32 n, [OutAttribute] UInt32* fences);
			[CLSCompliant(false)]
			public unsafe static GenFencesNV glGenFencesNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Int32 GenFragmentShadersATI(UInt32 range);
			[CLSCompliant(false)]
			public static GenFragmentShadersATI glGenFragmentShadersATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GenFramebuffers(Int32 n, [OutAttribute] UInt32* framebuffers);
			[CLSCompliant(false)]
			public unsafe static GenFramebuffers glGenFramebuffers;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GenFramebuffersEXT(Int32 n, [OutAttribute] UInt32* framebuffers);
			[CLSCompliant(false)]
			public unsafe static GenFramebuffersEXT glGenFramebuffersEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Int32 GenLists(Int32 range);
			[CLSCompliant(false)]
			public static GenLists glGenLists;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GenNamesAMD(System.Graphics.OGL.AmdNameGenDelete identifier, UInt32 num, [OutAttribute] UInt32* names);
			[CLSCompliant(false)]
			public unsafe static GenNamesAMD glGenNamesAMD;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GenOcclusionQueriesNV(Int32 n, [OutAttribute] UInt32* ids);
			[CLSCompliant(false)]
			public unsafe static GenOcclusionQueriesNV glGenOcclusionQueriesNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GenPerfMonitorsAMD(Int32 n, [OutAttribute] UInt32* monitors);
			[CLSCompliant(false)]
			public unsafe static GenPerfMonitorsAMD glGenPerfMonitorsAMD;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GenProgramPipelines(Int32 n, [OutAttribute] UInt32* pipelines);
			[CLSCompliant(false)]
			public unsafe static GenProgramPipelines glGenProgramPipelines;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GenProgramsARB(Int32 n, [OutAttribute] UInt32* programs);
			[CLSCompliant(false)]
			public unsafe static GenProgramsARB glGenProgramsARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GenProgramsNV(Int32 n, [OutAttribute] UInt32* programs);
			[CLSCompliant(false)]
			public unsafe static GenProgramsNV glGenProgramsNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GenQueries(Int32 n, [OutAttribute] UInt32* ids);
			[CLSCompliant(false)]
			public unsafe static GenQueries glGenQueries;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GenQueriesARB(Int32 n, [OutAttribute] UInt32* ids);
			[CLSCompliant(false)]
			public unsafe static GenQueriesARB glGenQueriesARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GenRenderbuffers(Int32 n, [OutAttribute] UInt32* renderbuffers);
			[CLSCompliant(false)]
			public unsafe static GenRenderbuffers glGenRenderbuffers;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GenRenderbuffersEXT(Int32 n, [OutAttribute] UInt32* renderbuffers);
			[CLSCompliant(false)]
			public unsafe static GenRenderbuffersEXT glGenRenderbuffersEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GenSamplers(Int32 count, [OutAttribute] UInt32* samplers);
			[CLSCompliant(false)]
			public unsafe static GenSamplers glGenSamplers;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Int32 GenSymbolsEXT(System.Graphics.OGL.ExtVertexShader datatype, System.Graphics.OGL.ExtVertexShader storagetype, System.Graphics.OGL.ExtVertexShader range, UInt32 components);
			[CLSCompliant(false)]
			public static GenSymbolsEXT glGenSymbolsEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GenTextures(Int32 n, [OutAttribute] UInt32* textures);
			[CLSCompliant(false)]
			public unsafe static GenTextures glGenTextures;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GenTexturesEXT(Int32 n, [OutAttribute] UInt32* textures);
			[CLSCompliant(false)]
			public unsafe static GenTexturesEXT glGenTexturesEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GenTransformFeedbacks(Int32 n, [OutAttribute] UInt32* ids);
			[CLSCompliant(false)]
			public unsafe static GenTransformFeedbacks glGenTransformFeedbacks;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GenTransformFeedbacksNV(Int32 n, [OutAttribute] UInt32* ids);
			[CLSCompliant(false)]
			public unsafe static GenTransformFeedbacksNV glGenTransformFeedbacksNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GenVertexArrays(Int32 n, [OutAttribute] UInt32* arrays);
			[CLSCompliant(false)]
			public unsafe static GenVertexArrays glGenVertexArrays;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GenVertexArraysAPPLE(Int32 n, [OutAttribute] UInt32* arrays);
			[CLSCompliant(false)]
			public unsafe static GenVertexArraysAPPLE glGenVertexArraysAPPLE;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Int32 GenVertexShadersEXT(UInt32 range);
			[CLSCompliant(false)]
			public static GenVertexShadersEXT glGenVertexShadersEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetActiveAttrib(UInt32 program, UInt32 index, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] Int32* size, [OutAttribute] System.Graphics.OGL.ActiveAttribType* type, [OutAttribute] StringBuilder name);
			[CLSCompliant(false)]
			public unsafe static GetActiveAttrib glGetActiveAttrib;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetActiveAttribARB(UInt32 programObj, UInt32 index, Int32 maxLength, [OutAttribute] Int32* length, [OutAttribute] Int32* size, [OutAttribute] System.Graphics.OGL.ArbVertexShader* type, [OutAttribute] StringBuilder name);
			[CLSCompliant(false)]
			public unsafe static GetActiveAttribARB glGetActiveAttribARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetActiveSubroutineName(UInt32 program, System.Graphics.OGL.ShaderType shadertype, UInt32 index, Int32 bufsize, [OutAttribute] Int32* length, [OutAttribute] StringBuilder name);
			[CLSCompliant(false)]
			public unsafe static GetActiveSubroutineName glGetActiveSubroutineName;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetActiveSubroutineUniformiv(UInt32 program, System.Graphics.OGL.ShaderType shadertype, UInt32 index, System.Graphics.OGL.ActiveSubroutineUniformParameter pname, [OutAttribute] Int32* values);
			[CLSCompliant(false)]
			public unsafe static GetActiveSubroutineUniformiv glGetActiveSubroutineUniformiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetActiveSubroutineUniformName(UInt32 program, System.Graphics.OGL.ShaderType shadertype, UInt32 index, Int32 bufsize, [OutAttribute] Int32* length, [OutAttribute] StringBuilder name);
			[CLSCompliant(false)]
			public unsafe static GetActiveSubroutineUniformName glGetActiveSubroutineUniformName;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetActiveUniform(UInt32 program, UInt32 index, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] Int32* size, [OutAttribute] System.Graphics.OGL.ActiveUniformType* type, [OutAttribute] StringBuilder name);
			[CLSCompliant(false)]
			public unsafe static GetActiveUniform glGetActiveUniform;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetActiveUniformARB(UInt32 programObj, UInt32 index, Int32 maxLength, [OutAttribute] Int32* length, [OutAttribute] Int32* size, [OutAttribute] System.Graphics.OGL.ArbShaderObjects* type, [OutAttribute] StringBuilder name);
			[CLSCompliant(false)]
			public unsafe static GetActiveUniformARB glGetActiveUniformARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetActiveUniformBlockiv(UInt32 program, UInt32 uniformBlockIndex, System.Graphics.OGL.ActiveUniformBlockParameter pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetActiveUniformBlockiv glGetActiveUniformBlockiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetActiveUniformBlockName(UInt32 program, UInt32 uniformBlockIndex, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] StringBuilder uniformBlockName);
			[CLSCompliant(false)]
			public unsafe static GetActiveUniformBlockName glGetActiveUniformBlockName;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetActiveUniformName(UInt32 program, UInt32 uniformIndex, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] StringBuilder uniformName);
			[CLSCompliant(false)]
			public unsafe static GetActiveUniformName glGetActiveUniformName;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetActiveUniformsiv(UInt32 program, Int32 uniformCount, UInt32* uniformIndices, System.Graphics.OGL.ActiveUniformParameter pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetActiveUniformsiv glGetActiveUniformsiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetActiveVaryingNV(UInt32 program, UInt32 index, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] Int32* size, [OutAttribute] System.Graphics.OGL.NvTransformFeedback* type, [OutAttribute] StringBuilder name);
			[CLSCompliant(false)]
			public unsafe static GetActiveVaryingNV glGetActiveVaryingNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetArrayObjectfvATI(System.Graphics.OGL.EnableCap array, System.Graphics.OGL.AtiVertexArrayObject pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetArrayObjectfvATI glGetArrayObjectfvATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetArrayObjectivATI(System.Graphics.OGL.EnableCap array, System.Graphics.OGL.AtiVertexArrayObject pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetArrayObjectivATI glGetArrayObjectivATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetAttachedObjectsARB(UInt32 containerObj, Int32 maxCount, [OutAttribute] Int32* count, [OutAttribute] UInt32* obj);
			[CLSCompliant(false)]
			public unsafe static GetAttachedObjectsARB glGetAttachedObjectsARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetAttachedShaders(UInt32 program, Int32 maxCount, [OutAttribute] Int32* count, [OutAttribute] UInt32* obj);
			[CLSCompliant(false)]
			public unsafe static GetAttachedShaders glGetAttachedShaders;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Int32 GetAttribLocation(UInt32 program, String name);
			[CLSCompliant(false)]
			public static GetAttribLocation glGetAttribLocation;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Int32 GetAttribLocationARB(UInt32 programObj, String name);
			[CLSCompliant(false)]
			public static GetAttribLocationARB glGetAttribLocationARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetBooleani_v(System.Graphics.OGL.GetIndexedPName target, UInt32 index, [OutAttribute] bool* data);
			[CLSCompliant(false)]
			public unsafe static GetBooleani_v glGetBooleani_v;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetBooleanIndexedvEXT(System.Graphics.OGL.ExtDrawBuffers2 target, UInt32 index, [OutAttribute] bool* data);
			[CLSCompliant(false)]
			public unsafe static GetBooleanIndexedvEXT glGetBooleanIndexedvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetBooleanv(System.Graphics.OGL.GetPName pname, [OutAttribute] bool* @params);
			[CLSCompliant(false)]
			public unsafe static GetBooleanv glGetBooleanv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetBufferParameteri64v(System.Graphics.OGL.BufferTarget target, System.Graphics.OGL.BufferParameterName pname, [OutAttribute] Int64* @params);
			[CLSCompliant(false)]
			public unsafe static GetBufferParameteri64v glGetBufferParameteri64v;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetBufferParameteriv(System.Graphics.OGL.BufferTarget target, System.Graphics.OGL.BufferParameterName pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetBufferParameteriv glGetBufferParameteriv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetBufferParameterivARB(System.Graphics.OGL.ArbVertexBufferObject target, System.Graphics.OGL.BufferParameterNameArb pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetBufferParameterivARB glGetBufferParameterivARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetBufferParameterui64vNV(System.Graphics.OGL.NvShaderBufferLoad target, System.Graphics.OGL.NvShaderBufferLoad pname, [OutAttribute] UInt64* @params);
			[CLSCompliant(false)]
			public unsafe static GetBufferParameterui64vNV glGetBufferParameterui64vNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetBufferPointerv(System.Graphics.OGL.BufferTarget target, System.Graphics.OGL.BufferPointer pname, [OutAttribute] IntPtr @params);
			[CLSCompliant(false)]
			public static GetBufferPointerv glGetBufferPointerv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetBufferPointervARB(System.Graphics.OGL.ArbVertexBufferObject target, System.Graphics.OGL.BufferPointerNameArb pname, [OutAttribute] IntPtr @params);
			[CLSCompliant(false)]
			public static GetBufferPointervARB glGetBufferPointervARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetBufferSubData(System.Graphics.OGL.BufferTarget target, IntPtr offset, IntPtr size, [OutAttribute] IntPtr data);
			[CLSCompliant(false)]
			public static GetBufferSubData glGetBufferSubData;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetBufferSubDataARB(System.Graphics.OGL.BufferTargetArb target, IntPtr offset, IntPtr size, [OutAttribute] IntPtr data);
			[CLSCompliant(false)]
			public static GetBufferSubDataARB glGetBufferSubDataARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetClipPlane(System.Graphics.OGL.ClipPlaneName plane, [OutAttribute] Double* equation);
			[CLSCompliant(false)]
			public unsafe static GetClipPlane glGetClipPlane;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetColorTable(System.Graphics.OGL.ColorTableTarget target, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, [OutAttribute] IntPtr table);
			[CLSCompliant(false)]
			public static GetColorTable glGetColorTable;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetColorTableEXT(System.Graphics.OGL.ColorTableTarget target, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, [OutAttribute] IntPtr data);
			[CLSCompliant(false)]
			public static GetColorTableEXT glGetColorTableEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetColorTableParameterfv(System.Graphics.OGL.ColorTableTarget target, System.Graphics.OGL.GetColorTableParameterPName pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetColorTableParameterfv glGetColorTableParameterfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetColorTableParameterfvEXT(System.Graphics.OGL.ColorTableTarget target, System.Graphics.OGL.GetColorTableParameterPName pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetColorTableParameterfvEXT glGetColorTableParameterfvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetColorTableParameterfvSGI(System.Graphics.OGL.SgiColorTable target, System.Graphics.OGL.SgiColorTable pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetColorTableParameterfvSGI glGetColorTableParameterfvSGI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetColorTableParameteriv(System.Graphics.OGL.ColorTableTarget target, System.Graphics.OGL.GetColorTableParameterPName pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetColorTableParameteriv glGetColorTableParameteriv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetColorTableParameterivEXT(System.Graphics.OGL.ColorTableTarget target, System.Graphics.OGL.GetColorTableParameterPName pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetColorTableParameterivEXT glGetColorTableParameterivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetColorTableParameterivSGI(System.Graphics.OGL.SgiColorTable target, System.Graphics.OGL.SgiColorTable pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetColorTableParameterivSGI glGetColorTableParameterivSGI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetColorTableSGI(System.Graphics.OGL.SgiColorTable target, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, [OutAttribute] IntPtr table);
			[CLSCompliant(false)]
			public static GetColorTableSGI glGetColorTableSGI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetCombinerInputParameterfvNV(System.Graphics.OGL.NvRegisterCombiners stage, System.Graphics.OGL.NvRegisterCombiners portion, System.Graphics.OGL.NvRegisterCombiners variable, System.Graphics.OGL.NvRegisterCombiners pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetCombinerInputParameterfvNV glGetCombinerInputParameterfvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetCombinerInputParameterivNV(System.Graphics.OGL.NvRegisterCombiners stage, System.Graphics.OGL.NvRegisterCombiners portion, System.Graphics.OGL.NvRegisterCombiners variable, System.Graphics.OGL.NvRegisterCombiners pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetCombinerInputParameterivNV glGetCombinerInputParameterivNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetCombinerOutputParameterfvNV(System.Graphics.OGL.NvRegisterCombiners stage, System.Graphics.OGL.NvRegisterCombiners portion, System.Graphics.OGL.NvRegisterCombiners pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetCombinerOutputParameterfvNV glGetCombinerOutputParameterfvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetCombinerOutputParameterivNV(System.Graphics.OGL.NvRegisterCombiners stage, System.Graphics.OGL.NvRegisterCombiners portion, System.Graphics.OGL.NvRegisterCombiners pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetCombinerOutputParameterivNV glGetCombinerOutputParameterivNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetCombinerStageParameterfvNV(System.Graphics.OGL.NvRegisterCombiners2 stage, System.Graphics.OGL.NvRegisterCombiners2 pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetCombinerStageParameterfvNV glGetCombinerStageParameterfvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetCompressedMultiTexImageEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureTarget target, Int32 lod, [OutAttribute] IntPtr img);
			[CLSCompliant(false)]
			public static GetCompressedMultiTexImageEXT glGetCompressedMultiTexImageEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetCompressedTexImage(System.Graphics.OGL.TextureTarget target, Int32 level, [OutAttribute] IntPtr img);
			[CLSCompliant(false)]
			public static GetCompressedTexImage glGetCompressedTexImage;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetCompressedTexImageARB(System.Graphics.OGL.TextureTarget target, Int32 level, [OutAttribute] IntPtr img);
			[CLSCompliant(false)]
			public static GetCompressedTexImageARB glGetCompressedTexImageARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetCompressedTextureImageEXT(UInt32 texture, System.Graphics.OGL.TextureTarget target, Int32 lod, [OutAttribute] IntPtr img);
			[CLSCompliant(false)]
			public static GetCompressedTextureImageEXT glGetCompressedTextureImageEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetConvolutionFilter(System.Graphics.OGL.ConvolutionTarget target, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, [OutAttribute] IntPtr image);
			[CLSCompliant(false)]
			public static GetConvolutionFilter glGetConvolutionFilter;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetConvolutionFilterEXT(System.Graphics.OGL.ExtConvolution target, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, [OutAttribute] IntPtr image);
			[CLSCompliant(false)]
			public static GetConvolutionFilterEXT glGetConvolutionFilterEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetConvolutionParameterfv(System.Graphics.OGL.ConvolutionTarget target, System.Graphics.OGL.GetConvolutionParameterPName pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetConvolutionParameterfv glGetConvolutionParameterfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetConvolutionParameterfvEXT(System.Graphics.OGL.ExtConvolution target, System.Graphics.OGL.ExtConvolution pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetConvolutionParameterfvEXT glGetConvolutionParameterfvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetConvolutionParameteriv(System.Graphics.OGL.ConvolutionTarget target, System.Graphics.OGL.GetConvolutionParameterPName pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetConvolutionParameteriv glGetConvolutionParameteriv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetConvolutionParameterivEXT(System.Graphics.OGL.ExtConvolution target, System.Graphics.OGL.ExtConvolution pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetConvolutionParameterivEXT glGetConvolutionParameterivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate Int32 GetDebugMessageLogAMD(UInt32 count, Int32 bufsize, [OutAttribute] System.Graphics.OGL.AmdDebugOutput* categories, [OutAttribute] UInt32* severities, [OutAttribute] UInt32* ids, [OutAttribute] Int32* lengths, [OutAttribute] StringBuilder message);
			[CLSCompliant(false)]
			public unsafe static GetDebugMessageLogAMD glGetDebugMessageLogAMD;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate Int32 GetDebugMessageLogARB(UInt32 count, Int32 bufsize, [OutAttribute] System.Graphics.OGL.ArbDebugOutput* sources, [OutAttribute] System.Graphics.OGL.ArbDebugOutput* types, [OutAttribute] UInt32* ids, [OutAttribute] System.Graphics.OGL.ArbDebugOutput* severities, [OutAttribute] Int32* lengths, [OutAttribute] StringBuilder messageLog);
			[CLSCompliant(false)]
			public unsafe static GetDebugMessageLogARB glGetDebugMessageLogARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetDetailTexFuncSGIS(System.Graphics.OGL.TextureTarget target, [OutAttribute] Single* points);
			[CLSCompliant(false)]
			public unsafe static GetDetailTexFuncSGIS glGetDetailTexFuncSGIS;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetDoublei_v(System.Graphics.OGL.GetIndexedPName target, UInt32 index, [OutAttribute] Double* data);
			[CLSCompliant(false)]
			public unsafe static GetDoublei_v glGetDoublei_v;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetDoubleIndexedvEXT(System.Graphics.OGL.ExtDirectStateAccess target, UInt32 index, [OutAttribute] Double* data);
			[CLSCompliant(false)]
			public unsafe static GetDoubleIndexedvEXT glGetDoubleIndexedvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetDoublev(System.Graphics.OGL.GetPName pname, [OutAttribute] Double* @params);
			[CLSCompliant(false)]
			public unsafe static GetDoublev glGetDoublev;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate System.Graphics.OGL.ErrorCode GetError();
			[CLSCompliant(false)]
			public static GetError glGetError;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetFenceivNV(UInt32 fence, System.Graphics.OGL.NvFence pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetFenceivNV glGetFenceivNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetFinalCombinerInputParameterfvNV(System.Graphics.OGL.NvRegisterCombiners variable, System.Graphics.OGL.NvRegisterCombiners pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetFinalCombinerInputParameterfvNV glGetFinalCombinerInputParameterfvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetFinalCombinerInputParameterivNV(System.Graphics.OGL.NvRegisterCombiners variable, System.Graphics.OGL.NvRegisterCombiners pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetFinalCombinerInputParameterivNV glGetFinalCombinerInputParameterivNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetFloati_v(System.Graphics.OGL.GetIndexedPName target, UInt32 index, [OutAttribute] Single* data);
			[CLSCompliant(false)]
			public unsafe static GetFloati_v glGetFloati_v;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetFloatIndexedvEXT(System.Graphics.OGL.ExtDirectStateAccess target, UInt32 index, [OutAttribute] Single* data);
			[CLSCompliant(false)]
			public unsafe static GetFloatIndexedvEXT glGetFloatIndexedvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetFloatv(System.Graphics.OGL.GetPName pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetFloatv glGetFloatv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetFogFuncSGIS([OutAttribute] Single* points);
			[CLSCompliant(false)]
			public unsafe static GetFogFuncSGIS glGetFogFuncSGIS;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Int32 GetFragDataIndex(UInt32 program, String name);
			[CLSCompliant(false)]
			public static GetFragDataIndex glGetFragDataIndex;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Int32 GetFragDataLocation(UInt32 program, String name);
			[CLSCompliant(false)]
			public static GetFragDataLocation glGetFragDataLocation;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Int32 GetFragDataLocationEXT(UInt32 program, String name);
			[CLSCompliant(false)]
			public static GetFragDataLocationEXT glGetFragDataLocationEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetFragmentLightfvSGIX(System.Graphics.OGL.SgixFragmentLighting light, System.Graphics.OGL.SgixFragmentLighting pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetFragmentLightfvSGIX glGetFragmentLightfvSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetFragmentLightivSGIX(System.Graphics.OGL.SgixFragmentLighting light, System.Graphics.OGL.SgixFragmentLighting pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetFragmentLightivSGIX glGetFragmentLightivSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetFragmentMaterialfvSGIX(System.Graphics.OGL.MaterialFace face, System.Graphics.OGL.MaterialParameter pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetFragmentMaterialfvSGIX glGetFragmentMaterialfvSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetFragmentMaterialivSGIX(System.Graphics.OGL.MaterialFace face, System.Graphics.OGL.MaterialParameter pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetFragmentMaterialivSGIX glGetFragmentMaterialivSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetFramebufferAttachmentParameteriv(System.Graphics.OGL.FramebufferTarget target, System.Graphics.OGL.FramebufferAttachment attachment, System.Graphics.OGL.FramebufferParameterName pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetFramebufferAttachmentParameteriv glGetFramebufferAttachmentParameteriv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetFramebufferAttachmentParameterivEXT(System.Graphics.OGL.FramebufferTarget target, System.Graphics.OGL.FramebufferAttachment attachment, System.Graphics.OGL.FramebufferParameterName pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetFramebufferAttachmentParameterivEXT glGetFramebufferAttachmentParameterivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetFramebufferParameterivEXT(UInt32 framebuffer, System.Graphics.OGL.ExtDirectStateAccess pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetFramebufferParameterivEXT glGetFramebufferParameterivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate System.Graphics.OGL.ArbRobustness GetGraphicsResetStatusARB();
			[CLSCompliant(false)]
			public static GetGraphicsResetStatusARB glGetGraphicsResetStatusARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Int32 GetHandleARB(System.Graphics.OGL.ArbShaderObjects pname);
			[CLSCompliant(false)]
			public static GetHandleARB glGetHandleARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetHistogram(System.Graphics.OGL.HistogramTarget target, bool reset, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, [OutAttribute] IntPtr values);
			[CLSCompliant(false)]
			public static GetHistogram glGetHistogram;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetHistogramEXT(System.Graphics.OGL.ExtHistogram target, bool reset, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, [OutAttribute] IntPtr values);
			[CLSCompliant(false)]
			public static GetHistogramEXT glGetHistogramEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetHistogramParameterfv(System.Graphics.OGL.HistogramTarget target, System.Graphics.OGL.GetHistogramParameterPName pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetHistogramParameterfv glGetHistogramParameterfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetHistogramParameterfvEXT(System.Graphics.OGL.ExtHistogram target, System.Graphics.OGL.ExtHistogram pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetHistogramParameterfvEXT glGetHistogramParameterfvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetHistogramParameteriv(System.Graphics.OGL.HistogramTarget target, System.Graphics.OGL.GetHistogramParameterPName pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetHistogramParameteriv glGetHistogramParameteriv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetHistogramParameterivEXT(System.Graphics.OGL.ExtHistogram target, System.Graphics.OGL.ExtHistogram pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetHistogramParameterivEXT glGetHistogramParameterivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetImageTransformParameterfvHP(System.Graphics.OGL.HpImageTransform target, System.Graphics.OGL.HpImageTransform pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetImageTransformParameterfvHP glGetImageTransformParameterfvHP;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetImageTransformParameterivHP(System.Graphics.OGL.HpImageTransform target, System.Graphics.OGL.HpImageTransform pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetImageTransformParameterivHP glGetImageTransformParameterivHP;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetInfoLogARB(UInt32 obj, Int32 maxLength, [OutAttribute] Int32* length, [OutAttribute] StringBuilder infoLog);
			[CLSCompliant(false)]
			public unsafe static GetInfoLogARB glGetInfoLogARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Int32 GetInstrumentsSGIX();
			[CLSCompliant(false)]
			public static GetInstrumentsSGIX glGetInstrumentsSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetInteger64i_v(System.Graphics.OGL.GetIndexedPName target, UInt32 index, [OutAttribute] Int64* data);
			[CLSCompliant(false)]
			public unsafe static GetInteger64i_v glGetInteger64i_v;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetInteger64v(System.Graphics.OGL.ArbSync pname, [OutAttribute] Int64* @params);
			[CLSCompliant(false)]
			public unsafe static GetInteger64v glGetInteger64v;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetIntegeri_v(System.Graphics.OGL.GetIndexedPName target, UInt32 index, [OutAttribute] Int32* data);
			[CLSCompliant(false)]
			public unsafe static GetIntegeri_v glGetIntegeri_v;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetIntegerIndexedvEXT(System.Graphics.OGL.GetIndexedPName target, UInt32 index, [OutAttribute] Int32* data);
			[CLSCompliant(false)]
			public unsafe static GetIntegerIndexedvEXT glGetIntegerIndexedvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetIntegerui64i_vNV(System.Graphics.OGL.NvVertexBufferUnifiedMemory value, UInt32 index, [OutAttribute] UInt64* result);
			[CLSCompliant(false)]
			public unsafe static GetIntegerui64i_vNV glGetIntegerui64i_vNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetIntegerui64vNV(System.Graphics.OGL.NvShaderBufferLoad value, [OutAttribute] UInt64* result);
			[CLSCompliant(false)]
			public unsafe static GetIntegerui64vNV glGetIntegerui64vNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetIntegerv(System.Graphics.OGL.GetPName pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetIntegerv glGetIntegerv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetInvariantBooleanvEXT(UInt32 id, System.Graphics.OGL.ExtVertexShader value, [OutAttribute] bool* data);
			[CLSCompliant(false)]
			public unsafe static GetInvariantBooleanvEXT glGetInvariantBooleanvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetInvariantFloatvEXT(UInt32 id, System.Graphics.OGL.ExtVertexShader value, [OutAttribute] Single* data);
			[CLSCompliant(false)]
			public unsafe static GetInvariantFloatvEXT glGetInvariantFloatvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetInvariantIntegervEXT(UInt32 id, System.Graphics.OGL.ExtVertexShader value, [OutAttribute] Int32* data);
			[CLSCompliant(false)]
			public unsafe static GetInvariantIntegervEXT glGetInvariantIntegervEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetLightfv(System.Graphics.OGL.LightName light, System.Graphics.OGL.LightParameter pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetLightfv glGetLightfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetLightiv(System.Graphics.OGL.LightName light, System.Graphics.OGL.LightParameter pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetLightiv glGetLightiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetListParameterfvSGIX(UInt32 list, System.Graphics.OGL.ListParameterName pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetListParameterfvSGIX glGetListParameterfvSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetListParameterivSGIX(UInt32 list, System.Graphics.OGL.ListParameterName pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetListParameterivSGIX glGetListParameterivSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetLocalConstantBooleanvEXT(UInt32 id, System.Graphics.OGL.ExtVertexShader value, [OutAttribute] bool* data);
			[CLSCompliant(false)]
			public unsafe static GetLocalConstantBooleanvEXT glGetLocalConstantBooleanvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetLocalConstantFloatvEXT(UInt32 id, System.Graphics.OGL.ExtVertexShader value, [OutAttribute] Single* data);
			[CLSCompliant(false)]
			public unsafe static GetLocalConstantFloatvEXT glGetLocalConstantFloatvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetLocalConstantIntegervEXT(UInt32 id, System.Graphics.OGL.ExtVertexShader value, [OutAttribute] Int32* data);
			[CLSCompliant(false)]
			public unsafe static GetLocalConstantIntegervEXT glGetLocalConstantIntegervEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetMapAttribParameterfvNV(System.Graphics.OGL.NvEvaluators target, UInt32 index, System.Graphics.OGL.NvEvaluators pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetMapAttribParameterfvNV glGetMapAttribParameterfvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetMapAttribParameterivNV(System.Graphics.OGL.NvEvaluators target, UInt32 index, System.Graphics.OGL.NvEvaluators pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetMapAttribParameterivNV glGetMapAttribParameterivNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetMapControlPointsNV(System.Graphics.OGL.NvEvaluators target, UInt32 index, System.Graphics.OGL.NvEvaluators type, Int32 ustride, Int32 vstride, bool packed, [OutAttribute] IntPtr points);
			[CLSCompliant(false)]
			public static GetMapControlPointsNV glGetMapControlPointsNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetMapdv(System.Graphics.OGL.MapTarget target, System.Graphics.OGL.GetMapQuery query, [OutAttribute] Double* v);
			[CLSCompliant(false)]
			public unsafe static GetMapdv glGetMapdv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetMapfv(System.Graphics.OGL.MapTarget target, System.Graphics.OGL.GetMapQuery query, [OutAttribute] Single* v);
			[CLSCompliant(false)]
			public unsafe static GetMapfv glGetMapfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetMapiv(System.Graphics.OGL.MapTarget target, System.Graphics.OGL.GetMapQuery query, [OutAttribute] Int32* v);
			[CLSCompliant(false)]
			public unsafe static GetMapiv glGetMapiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetMapParameterfvNV(System.Graphics.OGL.NvEvaluators target, System.Graphics.OGL.NvEvaluators pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetMapParameterfvNV glGetMapParameterfvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetMapParameterivNV(System.Graphics.OGL.NvEvaluators target, System.Graphics.OGL.NvEvaluators pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetMapParameterivNV glGetMapParameterivNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetMaterialfv(System.Graphics.OGL.MaterialFace face, System.Graphics.OGL.MaterialParameter pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetMaterialfv glGetMaterialfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetMaterialiv(System.Graphics.OGL.MaterialFace face, System.Graphics.OGL.MaterialParameter pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetMaterialiv glGetMaterialiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetMinmax(System.Graphics.OGL.MinmaxTarget target, bool reset, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, [OutAttribute] IntPtr values);
			[CLSCompliant(false)]
			public static GetMinmax glGetMinmax;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetMinmaxEXT(System.Graphics.OGL.ExtHistogram target, bool reset, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, [OutAttribute] IntPtr values);
			[CLSCompliant(false)]
			public static GetMinmaxEXT glGetMinmaxEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetMinmaxParameterfv(System.Graphics.OGL.MinmaxTarget target, System.Graphics.OGL.GetMinmaxParameterPName pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetMinmaxParameterfv glGetMinmaxParameterfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetMinmaxParameterfvEXT(System.Graphics.OGL.ExtHistogram target, System.Graphics.OGL.ExtHistogram pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetMinmaxParameterfvEXT glGetMinmaxParameterfvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetMinmaxParameteriv(System.Graphics.OGL.MinmaxTarget target, System.Graphics.OGL.GetMinmaxParameterPName pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetMinmaxParameteriv glGetMinmaxParameteriv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetMinmaxParameterivEXT(System.Graphics.OGL.ExtHistogram target, System.Graphics.OGL.ExtHistogram pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetMinmaxParameterivEXT glGetMinmaxParameterivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetMultisamplefv(System.Graphics.OGL.GetMultisamplePName pname, UInt32 index, [OutAttribute] Single* val);
			[CLSCompliant(false)]
			public unsafe static GetMultisamplefv glGetMultisamplefv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetMultisamplefvNV(System.Graphics.OGL.NvExplicitMultisample pname, UInt32 index, [OutAttribute] Single* val);
			[CLSCompliant(false)]
			public unsafe static GetMultisamplefvNV glGetMultisamplefvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetMultiTexEnvfvEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureEnvTarget target, System.Graphics.OGL.TextureEnvParameter pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetMultiTexEnvfvEXT glGetMultiTexEnvfvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetMultiTexEnvivEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureEnvTarget target, System.Graphics.OGL.TextureEnvParameter pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetMultiTexEnvivEXT glGetMultiTexEnvivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetMultiTexGendvEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureCoordName coord, System.Graphics.OGL.TextureGenParameter pname, [OutAttribute] Double* @params);
			[CLSCompliant(false)]
			public unsafe static GetMultiTexGendvEXT glGetMultiTexGendvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetMultiTexGenfvEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureCoordName coord, System.Graphics.OGL.TextureGenParameter pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetMultiTexGenfvEXT glGetMultiTexGenfvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetMultiTexGenivEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureCoordName coord, System.Graphics.OGL.TextureGenParameter pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetMultiTexGenivEXT glGetMultiTexGenivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetMultiTexImageEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureTarget target, Int32 level, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, [OutAttribute] IntPtr pixels);
			[CLSCompliant(false)]
			public static GetMultiTexImageEXT glGetMultiTexImageEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetMultiTexLevelParameterfvEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureTarget target, Int32 level, System.Graphics.OGL.GetTextureParameter pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetMultiTexLevelParameterfvEXT glGetMultiTexLevelParameterfvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetMultiTexLevelParameterivEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureTarget target, Int32 level, System.Graphics.OGL.GetTextureParameter pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetMultiTexLevelParameterivEXT glGetMultiTexLevelParameterivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetMultiTexParameterfvEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureTarget target, System.Graphics.OGL.GetTextureParameter pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetMultiTexParameterfvEXT glGetMultiTexParameterfvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetMultiTexParameterIivEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureTarget target, System.Graphics.OGL.GetTextureParameter pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetMultiTexParameterIivEXT glGetMultiTexParameterIivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetMultiTexParameterIuivEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureTarget target, System.Graphics.OGL.GetTextureParameter pname, [OutAttribute] UInt32* @params);
			[CLSCompliant(false)]
			public unsafe static GetMultiTexParameterIuivEXT glGetMultiTexParameterIuivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetMultiTexParameterivEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureTarget target, System.Graphics.OGL.GetTextureParameter pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetMultiTexParameterivEXT glGetMultiTexParameterivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetNamedBufferParameterivEXT(UInt32 buffer, System.Graphics.OGL.ExtDirectStateAccess pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetNamedBufferParameterivEXT glGetNamedBufferParameterivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetNamedBufferParameterui64vNV(UInt32 buffer, System.Graphics.OGL.NvShaderBufferLoad pname, [OutAttribute] UInt64* @params);
			[CLSCompliant(false)]
			public unsafe static GetNamedBufferParameterui64vNV glGetNamedBufferParameterui64vNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetNamedBufferPointervEXT(UInt32 buffer, System.Graphics.OGL.ExtDirectStateAccess pname, [OutAttribute] IntPtr @params);
			[CLSCompliant(false)]
			public static GetNamedBufferPointervEXT glGetNamedBufferPointervEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetNamedBufferSubDataEXT(UInt32 buffer, IntPtr offset, IntPtr size, [OutAttribute] IntPtr data);
			[CLSCompliant(false)]
			public static GetNamedBufferSubDataEXT glGetNamedBufferSubDataEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetNamedFramebufferAttachmentParameterivEXT(UInt32 framebuffer, System.Graphics.OGL.FramebufferAttachment attachment, System.Graphics.OGL.ExtDirectStateAccess pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetNamedFramebufferAttachmentParameterivEXT glGetNamedFramebufferAttachmentParameterivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetNamedProgramivEXT(UInt32 program, System.Graphics.OGL.ExtDirectStateAccess target, System.Graphics.OGL.ExtDirectStateAccess pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetNamedProgramivEXT glGetNamedProgramivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetNamedProgramLocalParameterdvEXT(UInt32 program, System.Graphics.OGL.ExtDirectStateAccess target, UInt32 index, [OutAttribute] Double* @params);
			[CLSCompliant(false)]
			public unsafe static GetNamedProgramLocalParameterdvEXT glGetNamedProgramLocalParameterdvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetNamedProgramLocalParameterfvEXT(UInt32 program, System.Graphics.OGL.ExtDirectStateAccess target, UInt32 index, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetNamedProgramLocalParameterfvEXT glGetNamedProgramLocalParameterfvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetNamedProgramLocalParameterIivEXT(UInt32 program, System.Graphics.OGL.ExtDirectStateAccess target, UInt32 index, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetNamedProgramLocalParameterIivEXT glGetNamedProgramLocalParameterIivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetNamedProgramLocalParameterIuivEXT(UInt32 program, System.Graphics.OGL.ExtDirectStateAccess target, UInt32 index, [OutAttribute] UInt32* @params);
			[CLSCompliant(false)]
			public unsafe static GetNamedProgramLocalParameterIuivEXT glGetNamedProgramLocalParameterIuivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetNamedProgramStringEXT(UInt32 program, System.Graphics.OGL.ExtDirectStateAccess target, System.Graphics.OGL.ExtDirectStateAccess pname, [OutAttribute] IntPtr @string);
			[CLSCompliant(false)]
			public static GetNamedProgramStringEXT glGetNamedProgramStringEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetNamedRenderbufferParameterivEXT(UInt32 renderbuffer, System.Graphics.OGL.RenderbufferParameterName pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetNamedRenderbufferParameterivEXT glGetNamedRenderbufferParameterivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetNamedStringARB(Int32 namelen, String name, Int32 bufSize, [OutAttribute] Int32* stringlen, [OutAttribute] StringBuilder @string);
			[CLSCompliant(false)]
			public unsafe static GetNamedStringARB glGetNamedStringARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetNamedStringivARB(Int32 namelen, String name, System.Graphics.OGL.ArbShadingLanguageInclude pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetNamedStringivARB glGetNamedStringivARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetnColorTableARB(System.Graphics.OGL.ArbRobustness target, System.Graphics.OGL.ArbRobustness format, System.Graphics.OGL.ArbRobustness type, Int32 bufSize, [OutAttribute] IntPtr table);
			[CLSCompliant(false)]
			public static GetnColorTableARB glGetnColorTableARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetnCompressedTexImageARB(System.Graphics.OGL.ArbRobustness target, Int32 lod, Int32 bufSize, [OutAttribute] IntPtr img);
			[CLSCompliant(false)]
			public static GetnCompressedTexImageARB glGetnCompressedTexImageARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetnConvolutionFilterARB(System.Graphics.OGL.ArbRobustness target, System.Graphics.OGL.ArbRobustness format, System.Graphics.OGL.ArbRobustness type, Int32 bufSize, [OutAttribute] IntPtr image);
			[CLSCompliant(false)]
			public static GetnConvolutionFilterARB glGetnConvolutionFilterARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetnHistogramARB(System.Graphics.OGL.ArbRobustness target, bool reset, System.Graphics.OGL.ArbRobustness format, System.Graphics.OGL.ArbRobustness type, Int32 bufSize, [OutAttribute] IntPtr values);
			[CLSCompliant(false)]
			public static GetnHistogramARB glGetnHistogramARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetnMapdvARB(System.Graphics.OGL.ArbRobustness target, System.Graphics.OGL.ArbRobustness query, Int32 bufSize, [OutAttribute] Double* v);
			[CLSCompliant(false)]
			public unsafe static GetnMapdvARB glGetnMapdvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetnMapfvARB(System.Graphics.OGL.ArbRobustness target, System.Graphics.OGL.ArbRobustness query, Int32 bufSize, [OutAttribute] Single* v);
			[CLSCompliant(false)]
			public unsafe static GetnMapfvARB glGetnMapfvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetnMapivARB(System.Graphics.OGL.ArbRobustness target, System.Graphics.OGL.ArbRobustness query, Int32 bufSize, [OutAttribute] Int32* v);
			[CLSCompliant(false)]
			public unsafe static GetnMapivARB glGetnMapivARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetnMinmaxARB(System.Graphics.OGL.ArbRobustness target, bool reset, System.Graphics.OGL.ArbRobustness format, System.Graphics.OGL.ArbRobustness type, Int32 bufSize, [OutAttribute] IntPtr values);
			[CLSCompliant(false)]
			public static GetnMinmaxARB glGetnMinmaxARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetnPixelMapfvARB(System.Graphics.OGL.ArbRobustness map, Int32 bufSize, [OutAttribute] Single* values);
			[CLSCompliant(false)]
			public unsafe static GetnPixelMapfvARB glGetnPixelMapfvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetnPixelMapuivARB(System.Graphics.OGL.ArbRobustness map, Int32 bufSize, [OutAttribute] UInt32* values);
			[CLSCompliant(false)]
			public unsafe static GetnPixelMapuivARB glGetnPixelMapuivARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetnPixelMapusvARB(System.Graphics.OGL.ArbRobustness map, Int32 bufSize, [OutAttribute] UInt16* values);
			[CLSCompliant(false)]
			public unsafe static GetnPixelMapusvARB glGetnPixelMapusvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetnPolygonStippleARB(Int32 bufSize, [OutAttribute] Byte* pattern);
			[CLSCompliant(false)]
			public unsafe static GetnPolygonStippleARB glGetnPolygonStippleARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetnSeparableFilterARB(System.Graphics.OGL.ArbRobustness target, System.Graphics.OGL.ArbRobustness format, System.Graphics.OGL.ArbRobustness type, Int32 rowBufSize, [OutAttribute] IntPtr row, Int32 columnBufSize, [OutAttribute] IntPtr column, [OutAttribute] IntPtr span);
			[CLSCompliant(false)]
			public static GetnSeparableFilterARB glGetnSeparableFilterARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetnTexImageARB(System.Graphics.OGL.ArbRobustness target, Int32 level, System.Graphics.OGL.ArbRobustness format, System.Graphics.OGL.ArbRobustness type, Int32 bufSize, [OutAttribute] IntPtr img);
			[CLSCompliant(false)]
			public static GetnTexImageARB glGetnTexImageARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetnUniformdvARB(UInt32 program, Int32 location, Int32 bufSize, [OutAttribute] Double* @params);
			[CLSCompliant(false)]
			public unsafe static GetnUniformdvARB glGetnUniformdvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetnUniformfvARB(UInt32 program, Int32 location, Int32 bufSize, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetnUniformfvARB glGetnUniformfvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetnUniformivARB(UInt32 program, Int32 location, Int32 bufSize, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetnUniformivARB glGetnUniformivARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetnUniformuivARB(UInt32 program, Int32 location, Int32 bufSize, [OutAttribute] UInt32* @params);
			[CLSCompliant(false)]
			public unsafe static GetnUniformuivARB glGetnUniformuivARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetObjectBufferfvATI(UInt32 buffer, System.Graphics.OGL.AtiVertexArrayObject pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetObjectBufferfvATI glGetObjectBufferfvATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetObjectBufferivATI(UInt32 buffer, System.Graphics.OGL.AtiVertexArrayObject pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetObjectBufferivATI glGetObjectBufferivATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetObjectParameterfvARB(UInt32 obj, System.Graphics.OGL.ArbShaderObjects pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetObjectParameterfvARB glGetObjectParameterfvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetObjectParameterivAPPLE(System.Graphics.OGL.AppleObjectPurgeable objectType, UInt32 name, System.Graphics.OGL.AppleObjectPurgeable pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetObjectParameterivAPPLE glGetObjectParameterivAPPLE;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetObjectParameterivARB(UInt32 obj, System.Graphics.OGL.ArbShaderObjects pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetObjectParameterivARB glGetObjectParameterivARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetOcclusionQueryivNV(UInt32 id, System.Graphics.OGL.NvOcclusionQuery pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetOcclusionQueryivNV glGetOcclusionQueryivNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetOcclusionQueryuivNV(UInt32 id, System.Graphics.OGL.NvOcclusionQuery pname, [OutAttribute] UInt32* @params);
			[CLSCompliant(false)]
			public unsafe static GetOcclusionQueryuivNV glGetOcclusionQueryuivNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetPerfMonitorCounterDataAMD(UInt32 monitor, System.Graphics.OGL.AmdPerformanceMonitor pname, Int32 dataSize, [OutAttribute] UInt32* data, [OutAttribute] Int32* bytesWritten);
			[CLSCompliant(false)]
			public unsafe static GetPerfMonitorCounterDataAMD glGetPerfMonitorCounterDataAMD;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetPerfMonitorCounterInfoAMD(UInt32 group, UInt32 counter, System.Graphics.OGL.AmdPerformanceMonitor pname, [OutAttribute] IntPtr data);
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
			public unsafe delegate void GetPixelMapfv(System.Graphics.OGL.PixelMap map, [OutAttribute] Single* values);
			[CLSCompliant(false)]
			public unsafe static GetPixelMapfv glGetPixelMapfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetPixelMapuiv(System.Graphics.OGL.PixelMap map, [OutAttribute] UInt32* values);
			[CLSCompliant(false)]
			public unsafe static GetPixelMapuiv glGetPixelMapuiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetPixelMapusv(System.Graphics.OGL.PixelMap map, [OutAttribute] UInt16* values);
			[CLSCompliant(false)]
			public unsafe static GetPixelMapusv glGetPixelMapusv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetPixelTexGenParameterfvSGIS(System.Graphics.OGL.SgisPixelTexture pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetPixelTexGenParameterfvSGIS glGetPixelTexGenParameterfvSGIS;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetPixelTexGenParameterivSGIS(System.Graphics.OGL.SgisPixelTexture pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetPixelTexGenParameterivSGIS glGetPixelTexGenParameterivSGIS;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetPointerIndexedvEXT(System.Graphics.OGL.ExtDirectStateAccess target, UInt32 index, [OutAttribute] IntPtr data);
			[CLSCompliant(false)]
			public static GetPointerIndexedvEXT glGetPointerIndexedvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetPointerv(System.Graphics.OGL.GetPointervPName pname, [OutAttribute] IntPtr @params);
			[CLSCompliant(false)]
			public static GetPointerv glGetPointerv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetPointervEXT(System.Graphics.OGL.GetPointervPName pname, [OutAttribute] IntPtr @params);
			[CLSCompliant(false)]
			public static GetPointervEXT glGetPointervEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetPolygonStipple([OutAttribute] Byte* mask);
			[CLSCompliant(false)]
			public unsafe static GetPolygonStipple glGetPolygonStipple;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetProgramBinary(UInt32 program, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] System.Graphics.OGL.BinaryFormat* binaryFormat, [OutAttribute] IntPtr binary);
			[CLSCompliant(false)]
			public unsafe static GetProgramBinary glGetProgramBinary;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetProgramEnvParameterdvARB(System.Graphics.OGL.ArbVertexProgram target, UInt32 index, [OutAttribute] Double* @params);
			[CLSCompliant(false)]
			public unsafe static GetProgramEnvParameterdvARB glGetProgramEnvParameterdvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetProgramEnvParameterfvARB(System.Graphics.OGL.ArbVertexProgram target, UInt32 index, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetProgramEnvParameterfvARB glGetProgramEnvParameterfvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetProgramEnvParameterIivNV(System.Graphics.OGL.NvGpuProgram4 target, UInt32 index, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetProgramEnvParameterIivNV glGetProgramEnvParameterIivNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetProgramEnvParameterIuivNV(System.Graphics.OGL.NvGpuProgram4 target, UInt32 index, [OutAttribute] UInt32* @params);
			[CLSCompliant(false)]
			public unsafe static GetProgramEnvParameterIuivNV glGetProgramEnvParameterIuivNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetProgramInfoLog(UInt32 program, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] StringBuilder infoLog);
			[CLSCompliant(false)]
			public unsafe static GetProgramInfoLog glGetProgramInfoLog;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetProgramiv(UInt32 program, System.Graphics.OGL.ProgramParameter pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetProgramiv glGetProgramiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetProgramivARB(System.Graphics.OGL.AssemblyProgramTargetArb target, System.Graphics.OGL.AssemblyProgramParameterArb pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetProgramivARB glGetProgramivARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetProgramivNV(UInt32 id, System.Graphics.OGL.NvVertexProgram pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetProgramivNV glGetProgramivNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetProgramLocalParameterdvARB(System.Graphics.OGL.ArbVertexProgram target, UInt32 index, [OutAttribute] Double* @params);
			[CLSCompliant(false)]
			public unsafe static GetProgramLocalParameterdvARB glGetProgramLocalParameterdvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetProgramLocalParameterfvARB(System.Graphics.OGL.ArbVertexProgram target, UInt32 index, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetProgramLocalParameterfvARB glGetProgramLocalParameterfvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetProgramLocalParameterIivNV(System.Graphics.OGL.NvGpuProgram4 target, UInt32 index, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetProgramLocalParameterIivNV glGetProgramLocalParameterIivNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetProgramLocalParameterIuivNV(System.Graphics.OGL.NvGpuProgram4 target, UInt32 index, [OutAttribute] UInt32* @params);
			[CLSCompliant(false)]
			public unsafe static GetProgramLocalParameterIuivNV glGetProgramLocalParameterIuivNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetProgramNamedParameterdvNV(UInt32 id, Int32 len, Byte* name, [OutAttribute] Double* @params);
			[CLSCompliant(false)]
			public unsafe static GetProgramNamedParameterdvNV glGetProgramNamedParameterdvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetProgramNamedParameterfvNV(UInt32 id, Int32 len, Byte* name, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetProgramNamedParameterfvNV glGetProgramNamedParameterfvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetProgramParameterdvNV(System.Graphics.OGL.AssemblyProgramTargetArb target, UInt32 index, System.Graphics.OGL.AssemblyProgramParameterArb pname, [OutAttribute] Double* @params);
			[CLSCompliant(false)]
			public unsafe static GetProgramParameterdvNV glGetProgramParameterdvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetProgramParameterfvNV(System.Graphics.OGL.AssemblyProgramTargetArb target, UInt32 index, System.Graphics.OGL.AssemblyProgramParameterArb pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetProgramParameterfvNV glGetProgramParameterfvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetProgramPipelineInfoLog(UInt32 pipeline, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] StringBuilder infoLog);
			[CLSCompliant(false)]
			public unsafe static GetProgramPipelineInfoLog glGetProgramPipelineInfoLog;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetProgramPipelineiv(UInt32 pipeline, System.Graphics.OGL.ProgramPipelineParameter pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetProgramPipelineiv glGetProgramPipelineiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetProgramStageiv(UInt32 program, System.Graphics.OGL.ShaderType shadertype, System.Graphics.OGL.ProgramStageParameter pname, [OutAttribute] Int32* values);
			[CLSCompliant(false)]
			public unsafe static GetProgramStageiv glGetProgramStageiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetProgramStringARB(System.Graphics.OGL.AssemblyProgramTargetArb target, System.Graphics.OGL.AssemblyProgramParameterArb pname, [OutAttribute] IntPtr @string);
			[CLSCompliant(false)]
			public static GetProgramStringARB glGetProgramStringARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetProgramStringNV(UInt32 id, System.Graphics.OGL.NvVertexProgram pname, [OutAttribute] Byte* program);
			[CLSCompliant(false)]
			public unsafe static GetProgramStringNV glGetProgramStringNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetProgramSubroutineParameteruivNV(System.Graphics.OGL.NvGpuProgram5 target, UInt32 index, [OutAttribute] UInt32* param);
			[CLSCompliant(false)]
			public unsafe static GetProgramSubroutineParameteruivNV glGetProgramSubroutineParameteruivNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetQueryIndexediv(System.Graphics.OGL.QueryTarget target, UInt32 index, System.Graphics.OGL.GetQueryParam pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetQueryIndexediv glGetQueryIndexediv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetQueryiv(System.Graphics.OGL.QueryTarget target, System.Graphics.OGL.GetQueryParam pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetQueryiv glGetQueryiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetQueryivARB(System.Graphics.OGL.ArbOcclusionQuery target, System.Graphics.OGL.ArbOcclusionQuery pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetQueryivARB glGetQueryivARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetQueryObjecti64v(UInt32 id, System.Graphics.OGL.GetQueryObjectParam pname, [OutAttribute] Int64* @params);
			[CLSCompliant(false)]
			public unsafe static GetQueryObjecti64v glGetQueryObjecti64v;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetQueryObjecti64vEXT(UInt32 id, System.Graphics.OGL.ExtTimerQuery pname, [OutAttribute] Int64* @params);
			[CLSCompliant(false)]
			public unsafe static GetQueryObjecti64vEXT glGetQueryObjecti64vEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetQueryObjectiv(UInt32 id, System.Graphics.OGL.GetQueryObjectParam pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetQueryObjectiv glGetQueryObjectiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetQueryObjectivARB(UInt32 id, System.Graphics.OGL.ArbOcclusionQuery pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetQueryObjectivARB glGetQueryObjectivARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetQueryObjectui64v(UInt32 id, System.Graphics.OGL.GetQueryObjectParam pname, [OutAttribute] UInt64* @params);
			[CLSCompliant(false)]
			public unsafe static GetQueryObjectui64v glGetQueryObjectui64v;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetQueryObjectui64vEXT(UInt32 id, System.Graphics.OGL.ExtTimerQuery pname, [OutAttribute] UInt64* @params);
			[CLSCompliant(false)]
			public unsafe static GetQueryObjectui64vEXT glGetQueryObjectui64vEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetQueryObjectuiv(UInt32 id, System.Graphics.OGL.GetQueryObjectParam pname, [OutAttribute] UInt32* @params);
			[CLSCompliant(false)]
			public unsafe static GetQueryObjectuiv glGetQueryObjectuiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetQueryObjectuivARB(UInt32 id, System.Graphics.OGL.ArbOcclusionQuery pname, [OutAttribute] UInt32* @params);
			[CLSCompliant(false)]
			public unsafe static GetQueryObjectuivARB glGetQueryObjectuivARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetRenderbufferParameteriv(System.Graphics.OGL.RenderbufferTarget target, System.Graphics.OGL.RenderbufferParameterName pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetRenderbufferParameteriv glGetRenderbufferParameteriv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetRenderbufferParameterivEXT(System.Graphics.OGL.RenderbufferTarget target, System.Graphics.OGL.RenderbufferParameterName pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetRenderbufferParameterivEXT glGetRenderbufferParameterivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetSamplerParameterfv(UInt32 sampler, System.Graphics.OGL.SamplerParameter pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetSamplerParameterfv glGetSamplerParameterfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetSamplerParameterIiv(UInt32 sampler, System.Graphics.OGL.ArbSamplerObjects pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetSamplerParameterIiv glGetSamplerParameterIiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetSamplerParameterIuiv(UInt32 sampler, System.Graphics.OGL.ArbSamplerObjects pname, [OutAttribute] UInt32* @params);
			[CLSCompliant(false)]
			public unsafe static GetSamplerParameterIuiv glGetSamplerParameterIuiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetSamplerParameteriv(UInt32 sampler, System.Graphics.OGL.SamplerParameter pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetSamplerParameteriv glGetSamplerParameteriv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetSeparableFilter(System.Graphics.OGL.SeparableTarget target, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, [OutAttribute] IntPtr row, [OutAttribute] IntPtr column, [OutAttribute] IntPtr span);
			[CLSCompliant(false)]
			public static GetSeparableFilter glGetSeparableFilter;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetSeparableFilterEXT(System.Graphics.OGL.ExtConvolution target, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, [OutAttribute] IntPtr row, [OutAttribute] IntPtr column, [OutAttribute] IntPtr span);
			[CLSCompliant(false)]
			public static GetSeparableFilterEXT glGetSeparableFilterEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetShaderInfoLog(UInt32 shader, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] StringBuilder infoLog);
			[CLSCompliant(false)]
			public unsafe static GetShaderInfoLog glGetShaderInfoLog;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetShaderiv(UInt32 shader, System.Graphics.OGL.ShaderParameter pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetShaderiv glGetShaderiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetShaderPrecisionFormat(System.Graphics.OGL.ShaderType shadertype, System.Graphics.OGL.ShaderPrecisionType precisiontype, [OutAttribute] Int32* range, [OutAttribute] Int32* precision);
			[CLSCompliant(false)]
			public unsafe static GetShaderPrecisionFormat glGetShaderPrecisionFormat;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetShaderSource(UInt32 shader, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] StringBuilder source);
			[CLSCompliant(false)]
			public unsafe static GetShaderSource glGetShaderSource;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetShaderSourceARB(UInt32 obj, Int32 maxLength, [OutAttribute] Int32* length, [OutAttribute] StringBuilder source);
			[CLSCompliant(false)]
			public unsafe static GetShaderSourceARB glGetShaderSourceARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetSharpenTexFuncSGIS(System.Graphics.OGL.TextureTarget target, [OutAttribute] Single* points);
			[CLSCompliant(false)]
			public unsafe static GetSharpenTexFuncSGIS glGetSharpenTexFuncSGIS;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate IntPtr GetString(System.Graphics.OGL.StringName name);
			[CLSCompliant(false)]
			public static GetString glGetString;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate IntPtr GetStringi(System.Graphics.OGL.StringName name, UInt32 index);
			[CLSCompliant(false)]
			public static GetStringi glGetStringi;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Int32 GetSubroutineIndex(UInt32 program, System.Graphics.OGL.ShaderType shadertype, String name);
			[CLSCompliant(false)]
			public static GetSubroutineIndex glGetSubroutineIndex;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Int32 GetSubroutineUniformLocation(UInt32 program, System.Graphics.OGL.ShaderType shadertype, String name);
			[CLSCompliant(false)]
			public static GetSubroutineUniformLocation glGetSubroutineUniformLocation;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetSynciv(IntPtr sync, System.Graphics.OGL.ArbSync pname, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] Int32* values);
			[CLSCompliant(false)]
			public unsafe static GetSynciv glGetSynciv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetTexBumpParameterfvATI(System.Graphics.OGL.AtiEnvmapBumpmap pname, [OutAttribute] Single* param);
			[CLSCompliant(false)]
			public unsafe static GetTexBumpParameterfvATI glGetTexBumpParameterfvATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetTexBumpParameterivATI(System.Graphics.OGL.AtiEnvmapBumpmap pname, [OutAttribute] Int32* param);
			[CLSCompliant(false)]
			public unsafe static GetTexBumpParameterivATI glGetTexBumpParameterivATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetTexEnvfv(System.Graphics.OGL.TextureEnvTarget target, System.Graphics.OGL.TextureEnvParameter pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetTexEnvfv glGetTexEnvfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetTexEnviv(System.Graphics.OGL.TextureEnvTarget target, System.Graphics.OGL.TextureEnvParameter pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetTexEnviv glGetTexEnviv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetTexFilterFuncSGIS(System.Graphics.OGL.TextureTarget target, System.Graphics.OGL.SgisTextureFilter4 filter, [OutAttribute] Single* weights);
			[CLSCompliant(false)]
			public unsafe static GetTexFilterFuncSGIS glGetTexFilterFuncSGIS;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetTexGendv(System.Graphics.OGL.TextureCoordName coord, System.Graphics.OGL.TextureGenParameter pname, [OutAttribute] Double* @params);
			[CLSCompliant(false)]
			public unsafe static GetTexGendv glGetTexGendv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetTexGenfv(System.Graphics.OGL.TextureCoordName coord, System.Graphics.OGL.TextureGenParameter pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetTexGenfv glGetTexGenfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetTexGeniv(System.Graphics.OGL.TextureCoordName coord, System.Graphics.OGL.TextureGenParameter pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetTexGeniv glGetTexGeniv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetTexImage(System.Graphics.OGL.TextureTarget target, Int32 level, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, [OutAttribute] IntPtr pixels);
			[CLSCompliant(false)]
			public static GetTexImage glGetTexImage;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetTexLevelParameterfv(System.Graphics.OGL.TextureTarget target, Int32 level, System.Graphics.OGL.GetTextureParameter pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetTexLevelParameterfv glGetTexLevelParameterfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetTexLevelParameteriv(System.Graphics.OGL.TextureTarget target, Int32 level, System.Graphics.OGL.GetTextureParameter pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetTexLevelParameteriv glGetTexLevelParameteriv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetTexParameterfv(System.Graphics.OGL.TextureTarget target, System.Graphics.OGL.GetTextureParameter pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetTexParameterfv glGetTexParameterfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetTexParameterIiv(System.Graphics.OGL.TextureTarget target, System.Graphics.OGL.GetTextureParameter pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetTexParameterIiv glGetTexParameterIiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetTexParameterIivEXT(System.Graphics.OGL.TextureTarget target, System.Graphics.OGL.GetTextureParameter pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetTexParameterIivEXT glGetTexParameterIivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetTexParameterIuiv(System.Graphics.OGL.TextureTarget target, System.Graphics.OGL.GetTextureParameter pname, [OutAttribute] UInt32* @params);
			[CLSCompliant(false)]
			public unsafe static GetTexParameterIuiv glGetTexParameterIuiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetTexParameterIuivEXT(System.Graphics.OGL.TextureTarget target, System.Graphics.OGL.GetTextureParameter pname, [OutAttribute] UInt32* @params);
			[CLSCompliant(false)]
			public unsafe static GetTexParameterIuivEXT glGetTexParameterIuivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetTexParameteriv(System.Graphics.OGL.TextureTarget target, System.Graphics.OGL.GetTextureParameter pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetTexParameteriv glGetTexParameteriv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetTexParameterPointervAPPLE(System.Graphics.OGL.AppleTextureRange target, System.Graphics.OGL.AppleTextureRange pname, [OutAttribute] IntPtr @params);
			[CLSCompliant(false)]
			public static GetTexParameterPointervAPPLE glGetTexParameterPointervAPPLE;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetTextureImageEXT(UInt32 texture, System.Graphics.OGL.TextureTarget target, Int32 level, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, [OutAttribute] IntPtr pixels);
			[CLSCompliant(false)]
			public static GetTextureImageEXT glGetTextureImageEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetTextureLevelParameterfvEXT(UInt32 texture, System.Graphics.OGL.TextureTarget target, Int32 level, System.Graphics.OGL.GetTextureParameter pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetTextureLevelParameterfvEXT glGetTextureLevelParameterfvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetTextureLevelParameterivEXT(UInt32 texture, System.Graphics.OGL.TextureTarget target, Int32 level, System.Graphics.OGL.GetTextureParameter pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetTextureLevelParameterivEXT glGetTextureLevelParameterivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetTextureParameterfvEXT(UInt32 texture, System.Graphics.OGL.TextureTarget target, System.Graphics.OGL.GetTextureParameter pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetTextureParameterfvEXT glGetTextureParameterfvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetTextureParameterIivEXT(UInt32 texture, System.Graphics.OGL.TextureTarget target, System.Graphics.OGL.GetTextureParameter pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetTextureParameterIivEXT glGetTextureParameterIivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetTextureParameterIuivEXT(UInt32 texture, System.Graphics.OGL.TextureTarget target, System.Graphics.OGL.GetTextureParameter pname, [OutAttribute] UInt32* @params);
			[CLSCompliant(false)]
			public unsafe static GetTextureParameterIuivEXT glGetTextureParameterIuivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetTextureParameterivEXT(UInt32 texture, System.Graphics.OGL.TextureTarget target, System.Graphics.OGL.GetTextureParameter pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetTextureParameterivEXT glGetTextureParameterivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetTrackMatrixivNV(System.Graphics.OGL.AssemblyProgramTargetArb target, UInt32 address, System.Graphics.OGL.AssemblyProgramParameterArb pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetTrackMatrixivNV glGetTrackMatrixivNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetTransformFeedbackVarying(UInt32 program, UInt32 index, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] Int32* size, [OutAttribute] System.Graphics.OGL.ActiveAttribType* type, [OutAttribute] StringBuilder name);
			[CLSCompliant(false)]
			public unsafe static GetTransformFeedbackVarying glGetTransformFeedbackVarying;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetTransformFeedbackVaryingEXT(UInt32 program, UInt32 index, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] Int32* size, [OutAttribute] System.Graphics.OGL.ExtTransformFeedback* type, [OutAttribute] StringBuilder name);
			[CLSCompliant(false)]
			public unsafe static GetTransformFeedbackVaryingEXT glGetTransformFeedbackVaryingEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetTransformFeedbackVaryingNV(UInt32 program, UInt32 index, [OutAttribute] Int32* location);
			[CLSCompliant(false)]
			public unsafe static GetTransformFeedbackVaryingNV glGetTransformFeedbackVaryingNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Int32 GetUniformBlockIndex(UInt32 program, String uniformBlockName);
			[CLSCompliant(false)]
			public static GetUniformBlockIndex glGetUniformBlockIndex;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Int32 GetUniformBufferSizeEXT(UInt32 program, Int32 location);
			[CLSCompliant(false)]
			public static GetUniformBufferSizeEXT glGetUniformBufferSizeEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetUniformdv(UInt32 program, Int32 location, [OutAttribute] Double* @params);
			[CLSCompliant(false)]
			public unsafe static GetUniformdv glGetUniformdv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetUniformfv(UInt32 program, Int32 location, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetUniformfv glGetUniformfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetUniformfvARB(UInt32 programObj, Int32 location, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetUniformfvARB glGetUniformfvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetUniformi64vNV(UInt32 program, Int32 location, [OutAttribute] Int64* @params);
			[CLSCompliant(false)]
			public unsafe static GetUniformi64vNV glGetUniformi64vNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetUniformIndices(UInt32 program, Int32 uniformCount, String[] uniformNames, [OutAttribute] UInt32* uniformIndices);
			[CLSCompliant(false)]
			public unsafe static GetUniformIndices glGetUniformIndices;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetUniformiv(UInt32 program, Int32 location, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetUniformiv glGetUniformiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetUniformivARB(UInt32 programObj, Int32 location, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetUniformivARB glGetUniformivARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Int32 GetUniformLocation(UInt32 program, String name);
			[CLSCompliant(false)]
			public static GetUniformLocation glGetUniformLocation;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Int32 GetUniformLocationARB(UInt32 programObj, String name);
			[CLSCompliant(false)]
			public static GetUniformLocationARB glGetUniformLocationARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate IntPtr GetUniformOffsetEXT(UInt32 program, Int32 location);
			[CLSCompliant(false)]
			public static GetUniformOffsetEXT glGetUniformOffsetEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetUniformSubroutineuiv(System.Graphics.OGL.ShaderType shadertype, Int32 location, [OutAttribute] UInt32* @params);
			[CLSCompliant(false)]
			public unsafe static GetUniformSubroutineuiv glGetUniformSubroutineuiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetUniformui64vNV(UInt32 program, Int32 location, [OutAttribute] UInt64* @params);
			[CLSCompliant(false)]
			public unsafe static GetUniformui64vNV glGetUniformui64vNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetUniformuiv(UInt32 program, Int32 location, [OutAttribute] UInt32* @params);
			[CLSCompliant(false)]
			public unsafe static GetUniformuiv glGetUniformuiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetUniformuivEXT(UInt32 program, Int32 location, [OutAttribute] UInt32* @params);
			[CLSCompliant(false)]
			public unsafe static GetUniformuivEXT glGetUniformuivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetVariantArrayObjectfvATI(UInt32 id, System.Graphics.OGL.AtiVertexArrayObject pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetVariantArrayObjectfvATI glGetVariantArrayObjectfvATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetVariantArrayObjectivATI(UInt32 id, System.Graphics.OGL.AtiVertexArrayObject pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetVariantArrayObjectivATI glGetVariantArrayObjectivATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetVariantBooleanvEXT(UInt32 id, System.Graphics.OGL.ExtVertexShader value, [OutAttribute] bool* data);
			[CLSCompliant(false)]
			public unsafe static GetVariantBooleanvEXT glGetVariantBooleanvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetVariantFloatvEXT(UInt32 id, System.Graphics.OGL.ExtVertexShader value, [OutAttribute] Single* data);
			[CLSCompliant(false)]
			public unsafe static GetVariantFloatvEXT glGetVariantFloatvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetVariantIntegervEXT(UInt32 id, System.Graphics.OGL.ExtVertexShader value, [OutAttribute] Int32* data);
			[CLSCompliant(false)]
			public unsafe static GetVariantIntegervEXT glGetVariantIntegervEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetVariantPointervEXT(UInt32 id, System.Graphics.OGL.ExtVertexShader value, [OutAttribute] IntPtr data);
			[CLSCompliant(false)]
			public static GetVariantPointervEXT glGetVariantPointervEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Int32 GetVaryingLocationNV(UInt32 program, String name);
			[CLSCompliant(false)]
			public static GetVaryingLocationNV glGetVaryingLocationNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetVertexAttribArrayObjectfvATI(UInt32 index, System.Graphics.OGL.AtiVertexAttribArrayObject pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetVertexAttribArrayObjectfvATI glGetVertexAttribArrayObjectfvATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetVertexAttribArrayObjectivATI(UInt32 index, System.Graphics.OGL.AtiVertexAttribArrayObject pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetVertexAttribArrayObjectivATI glGetVertexAttribArrayObjectivATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetVertexAttribdv(UInt32 index, System.Graphics.OGL.VertexAttribParameter pname, [OutAttribute] Double* @params);
			[CLSCompliant(false)]
			public unsafe static GetVertexAttribdv glGetVertexAttribdv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetVertexAttribdvARB(UInt32 index, System.Graphics.OGL.VertexAttribParameterArb pname, [OutAttribute] Double* @params);
			[CLSCompliant(false)]
			public unsafe static GetVertexAttribdvARB glGetVertexAttribdvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetVertexAttribdvNV(UInt32 index, System.Graphics.OGL.NvVertexProgram pname, [OutAttribute] Double* @params);
			[CLSCompliant(false)]
			public unsafe static GetVertexAttribdvNV glGetVertexAttribdvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetVertexAttribfv(UInt32 index, System.Graphics.OGL.VertexAttribParameter pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetVertexAttribfv glGetVertexAttribfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetVertexAttribfvARB(UInt32 index, System.Graphics.OGL.VertexAttribParameterArb pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetVertexAttribfvARB glGetVertexAttribfvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetVertexAttribfvNV(UInt32 index, System.Graphics.OGL.NvVertexProgram pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetVertexAttribfvNV glGetVertexAttribfvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetVertexAttribIiv(UInt32 index, System.Graphics.OGL.VertexAttribParameter pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetVertexAttribIiv glGetVertexAttribIiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetVertexAttribIivEXT(UInt32 index, System.Graphics.OGL.NvVertexProgram4 pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetVertexAttribIivEXT glGetVertexAttribIivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetVertexAttribIuiv(UInt32 index, System.Graphics.OGL.VertexAttribParameter pname, [OutAttribute] UInt32* @params);
			[CLSCompliant(false)]
			public unsafe static GetVertexAttribIuiv glGetVertexAttribIuiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetVertexAttribIuivEXT(UInt32 index, System.Graphics.OGL.NvVertexProgram4 pname, [OutAttribute] UInt32* @params);
			[CLSCompliant(false)]
			public unsafe static GetVertexAttribIuivEXT glGetVertexAttribIuivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetVertexAttribiv(UInt32 index, System.Graphics.OGL.VertexAttribParameter pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetVertexAttribiv glGetVertexAttribiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetVertexAttribivARB(UInt32 index, System.Graphics.OGL.VertexAttribParameterArb pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetVertexAttribivARB glGetVertexAttribivARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetVertexAttribivNV(UInt32 index, System.Graphics.OGL.NvVertexProgram pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetVertexAttribivNV glGetVertexAttribivNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetVertexAttribLdv(UInt32 index, System.Graphics.OGL.VertexAttribParameter pname, [OutAttribute] Double* @params);
			[CLSCompliant(false)]
			public unsafe static GetVertexAttribLdv glGetVertexAttribLdv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetVertexAttribLdvEXT(UInt32 index, System.Graphics.OGL.ExtVertexAttrib64bit pname, [OutAttribute] Double* @params);
			[CLSCompliant(false)]
			public unsafe static GetVertexAttribLdvEXT glGetVertexAttribLdvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetVertexAttribLi64vNV(UInt32 index, System.Graphics.OGL.NvVertexAttribInteger64bit pname, [OutAttribute] Int64* @params);
			[CLSCompliant(false)]
			public unsafe static GetVertexAttribLi64vNV glGetVertexAttribLi64vNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetVertexAttribLui64vNV(UInt32 index, System.Graphics.OGL.NvVertexAttribInteger64bit pname, [OutAttribute] UInt64* @params);
			[CLSCompliant(false)]
			public unsafe static GetVertexAttribLui64vNV glGetVertexAttribLui64vNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetVertexAttribPointerv(UInt32 index, System.Graphics.OGL.VertexAttribPointerParameter pname, [OutAttribute] IntPtr pointer);
			[CLSCompliant(false)]
			public static GetVertexAttribPointerv glGetVertexAttribPointerv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetVertexAttribPointervARB(UInt32 index, System.Graphics.OGL.VertexAttribPointerParameterArb pname, [OutAttribute] IntPtr pointer);
			[CLSCompliant(false)]
			public static GetVertexAttribPointervARB glGetVertexAttribPointervARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GetVertexAttribPointervNV(UInt32 index, System.Graphics.OGL.NvVertexProgram pname, [OutAttribute] IntPtr pointer);
			[CLSCompliant(false)]
			public static GetVertexAttribPointervNV glGetVertexAttribPointervNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetVideoCaptureivNV(UInt32 video_capture_slot, System.Graphics.OGL.NvVideoCapture pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetVideoCaptureivNV glGetVideoCaptureivNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetVideoCaptureStreamdvNV(UInt32 video_capture_slot, UInt32 stream, System.Graphics.OGL.NvVideoCapture pname, [OutAttribute] Double* @params);
			[CLSCompliant(false)]
			public unsafe static GetVideoCaptureStreamdvNV glGetVideoCaptureStreamdvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetVideoCaptureStreamfvNV(UInt32 video_capture_slot, UInt32 stream, System.Graphics.OGL.NvVideoCapture pname, [OutAttribute] Single* @params);
			[CLSCompliant(false)]
			public unsafe static GetVideoCaptureStreamfvNV glGetVideoCaptureStreamfvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetVideoCaptureStreamivNV(UInt32 video_capture_slot, UInt32 stream, System.Graphics.OGL.NvVideoCapture pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetVideoCaptureStreamivNV glGetVideoCaptureStreamivNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetVideoi64vNV(UInt32 video_slot, System.Graphics.OGL.NvPresentVideo pname, [OutAttribute] Int64* @params);
			[CLSCompliant(false)]
			public unsafe static GetVideoi64vNV glGetVideoi64vNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetVideoivNV(UInt32 video_slot, System.Graphics.OGL.NvPresentVideo pname, [OutAttribute] Int32* @params);
			[CLSCompliant(false)]
			public unsafe static GetVideoivNV glGetVideoivNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetVideoui64vNV(UInt32 video_slot, System.Graphics.OGL.NvPresentVideo pname, [OutAttribute] UInt64* @params);
			[CLSCompliant(false)]
			public unsafe static GetVideoui64vNV glGetVideoui64vNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void GetVideouivNV(UInt32 video_slot, System.Graphics.OGL.NvPresentVideo pname, [OutAttribute] UInt32* @params);
			[CLSCompliant(false)]
			public unsafe static GetVideouivNV glGetVideouivNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GlobalAlphaFactorbSUN(SByte factor);
			[CLSCompliant(false)]
			public static GlobalAlphaFactorbSUN glGlobalAlphaFactorbSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GlobalAlphaFactordSUN(Double factor);
			[CLSCompliant(false)]
			public static GlobalAlphaFactordSUN glGlobalAlphaFactordSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GlobalAlphaFactorfSUN(Single factor);
			[CLSCompliant(false)]
			public static GlobalAlphaFactorfSUN glGlobalAlphaFactorfSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GlobalAlphaFactoriSUN(Int32 factor);
			[CLSCompliant(false)]
			public static GlobalAlphaFactoriSUN glGlobalAlphaFactoriSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GlobalAlphaFactorsSUN(Int16 factor);
			[CLSCompliant(false)]
			public static GlobalAlphaFactorsSUN glGlobalAlphaFactorsSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GlobalAlphaFactorubSUN(Byte factor);
			[CLSCompliant(false)]
			public static GlobalAlphaFactorubSUN glGlobalAlphaFactorubSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GlobalAlphaFactoruiSUN(UInt32 factor);
			[CLSCompliant(false)]
			public static GlobalAlphaFactoruiSUN glGlobalAlphaFactoruiSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void GlobalAlphaFactorusSUN(UInt16 factor);
			[CLSCompliant(false)]
			public static GlobalAlphaFactorusSUN glGlobalAlphaFactorusSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Hint(System.Graphics.OGL.HintTarget target, System.Graphics.OGL.HintMode mode);
			[CLSCompliant(false)]
			public static Hint glHint;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void HintPGI(System.Graphics.OGL.PgiMiscHints target, Int32 mode);
			[CLSCompliant(false)]
			public static HintPGI glHintPGI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Histogram(System.Graphics.OGL.HistogramTarget target, Int32 width, System.Graphics.OGL.PixelInternalFormat internalformat, bool sink);
			[CLSCompliant(false)]
			public static Histogram glHistogram;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void HistogramEXT(System.Graphics.OGL.ExtHistogram target, Int32 width, System.Graphics.OGL.PixelInternalFormat internalformat, bool sink);
			[CLSCompliant(false)]
			public static HistogramEXT glHistogramEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void IglooInterfaceSGIX(System.Graphics.OGL.All pname, IntPtr @params);
			[CLSCompliant(false)]
			public static IglooInterfaceSGIX glIglooInterfaceSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ImageTransformParameterfHP(System.Graphics.OGL.HpImageTransform target, System.Graphics.OGL.HpImageTransform pname, Single param);
			[CLSCompliant(false)]
			public static ImageTransformParameterfHP glImageTransformParameterfHP;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ImageTransformParameterfvHP(System.Graphics.OGL.HpImageTransform target, System.Graphics.OGL.HpImageTransform pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static ImageTransformParameterfvHP glImageTransformParameterfvHP;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ImageTransformParameteriHP(System.Graphics.OGL.HpImageTransform target, System.Graphics.OGL.HpImageTransform pname, Int32 param);
			[CLSCompliant(false)]
			public static ImageTransformParameteriHP glImageTransformParameteriHP;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ImageTransformParameterivHP(System.Graphics.OGL.HpImageTransform target, System.Graphics.OGL.HpImageTransform pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static ImageTransformParameterivHP glImageTransformParameterivHP;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate IntPtr ImportSyncEXT(System.Graphics.OGL.ExtX11SyncObject external_sync_type, IntPtr external_sync, UInt32 flags);
			[CLSCompliant(false)]
			public static ImportSyncEXT glImportSyncEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Indexd(Double c);
			[CLSCompliant(false)]
			public static Indexd glIndexd;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Indexdv(Double* c);
			[CLSCompliant(false)]
			public unsafe static Indexdv glIndexdv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Indexf(Single c);
			[CLSCompliant(false)]
			public static Indexf glIndexf;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void IndexFormatNV(System.Graphics.OGL.NvVertexBufferUnifiedMemory type, Int32 stride);
			[CLSCompliant(false)]
			public static IndexFormatNV glIndexFormatNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void IndexFuncEXT(System.Graphics.OGL.ExtIndexFunc func, Single @ref);
			[CLSCompliant(false)]
			public static IndexFuncEXT glIndexFuncEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Indexfv(Single* c);
			[CLSCompliant(false)]
			public unsafe static Indexfv glIndexfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Indexi(Int32 c);
			[CLSCompliant(false)]
			public static Indexi glIndexi;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Indexiv(Int32* c);
			[CLSCompliant(false)]
			public unsafe static Indexiv glIndexiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void IndexMask(UInt32 mask);
			[CLSCompliant(false)]
			public static IndexMask glIndexMask;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void IndexMaterialEXT(System.Graphics.OGL.MaterialFace face, System.Graphics.OGL.ExtIndexMaterial mode);
			[CLSCompliant(false)]
			public static IndexMaterialEXT glIndexMaterialEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void IndexPointer(System.Graphics.OGL.IndexPointerType type, Int32 stride, IntPtr pointer);
			[CLSCompliant(false)]
			public static IndexPointer glIndexPointer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void IndexPointerEXT(System.Graphics.OGL.IndexPointerType type, Int32 stride, Int32 count, IntPtr pointer);
			[CLSCompliant(false)]
			public static IndexPointerEXT glIndexPointerEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void IndexPointerListIBM(System.Graphics.OGL.IndexPointerType type, Int32 stride, IntPtr pointer, Int32 ptrstride);
			[CLSCompliant(false)]
			public static IndexPointerListIBM glIndexPointerListIBM;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Indexs(Int16 c);
			[CLSCompliant(false)]
			public static Indexs glIndexs;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Indexsv(Int16* c);
			[CLSCompliant(false)]
			public unsafe static Indexsv glIndexsv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Indexub(Byte c);
			[CLSCompliant(false)]
			public static Indexub glIndexub;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Indexubv(Byte* c);
			[CLSCompliant(false)]
			public unsafe static Indexubv glIndexubv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void InitNames();
			[CLSCompliant(false)]
			public static InitNames glInitNames;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void InsertComponentEXT(UInt32 res, UInt32 src, UInt32 num);
			[CLSCompliant(false)]
			public static InsertComponentEXT glInsertComponentEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void InstrumentsBufferSGIX(Int32 size, [OutAttribute] Int32* buffer);
			[CLSCompliant(false)]
			public unsafe static InstrumentsBufferSGIX glInstrumentsBufferSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void InterleavedArrays(System.Graphics.OGL.InterleavedArrayFormat format, Int32 stride, IntPtr pointer);
			[CLSCompliant(false)]
			public static InterleavedArrays glInterleavedArrays;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsAsyncMarkerSGIX(UInt32 marker);
			[CLSCompliant(false)]
			public static IsAsyncMarkerSGIX glIsAsyncMarkerSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsBuffer(UInt32 buffer);
			[CLSCompliant(false)]
			public static IsBuffer glIsBuffer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsBufferARB(UInt32 buffer);
			[CLSCompliant(false)]
			public static IsBufferARB glIsBufferARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsBufferResidentNV(System.Graphics.OGL.NvShaderBufferLoad target);
			[CLSCompliant(false)]
			public static IsBufferResidentNV glIsBufferResidentNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsEnabled(System.Graphics.OGL.EnableCap cap);
			[CLSCompliant(false)]
			public static IsEnabled glIsEnabled;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsEnabledi(System.Graphics.OGL.IndexedEnableCap target, UInt32 index);
			[CLSCompliant(false)]
			public static IsEnabledi glIsEnabledi;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsEnabledIndexedEXT(System.Graphics.OGL.IndexedEnableCap target, UInt32 index);
			[CLSCompliant(false)]
			public static IsEnabledIndexedEXT glIsEnabledIndexedEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsFenceAPPLE(UInt32 fence);
			[CLSCompliant(false)]
			public static IsFenceAPPLE glIsFenceAPPLE;
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
			public delegate bool IsFramebufferEXT(UInt32 framebuffer);
			[CLSCompliant(false)]
			public static IsFramebufferEXT glIsFramebufferEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsList(UInt32 list);
			[CLSCompliant(false)]
			public static IsList glIsList;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsNameAMD(System.Graphics.OGL.AmdNameGenDelete identifier, UInt32 name);
			[CLSCompliant(false)]
			public static IsNameAMD glIsNameAMD;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsNamedBufferResidentNV(UInt32 buffer);
			[CLSCompliant(false)]
			public static IsNamedBufferResidentNV glIsNamedBufferResidentNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsNamedStringARB(Int32 namelen, String name);
			[CLSCompliant(false)]
			public static IsNamedStringARB glIsNamedStringARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsObjectBufferATI(UInt32 buffer);
			[CLSCompliant(false)]
			public static IsObjectBufferATI glIsObjectBufferATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsOcclusionQueryNV(UInt32 id);
			[CLSCompliant(false)]
			public static IsOcclusionQueryNV glIsOcclusionQueryNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsProgram(UInt32 program);
			[CLSCompliant(false)]
			public static IsProgram glIsProgram;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsProgramARB(UInt32 program);
			[CLSCompliant(false)]
			public static IsProgramARB glIsProgramARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsProgramNV(UInt32 id);
			[CLSCompliant(false)]
			public static IsProgramNV glIsProgramNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsProgramPipeline(UInt32 pipeline);
			[CLSCompliant(false)]
			public static IsProgramPipeline glIsProgramPipeline;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsQuery(UInt32 id);
			[CLSCompliant(false)]
			public static IsQuery glIsQuery;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsQueryARB(UInt32 id);
			[CLSCompliant(false)]
			public static IsQueryARB glIsQueryARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsRenderbuffer(UInt32 renderbuffer);
			[CLSCompliant(false)]
			public static IsRenderbuffer glIsRenderbuffer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsRenderbufferEXT(UInt32 renderbuffer);
			[CLSCompliant(false)]
			public static IsRenderbufferEXT glIsRenderbufferEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsSampler(UInt32 sampler);
			[CLSCompliant(false)]
			public static IsSampler glIsSampler;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsShader(UInt32 shader);
			[CLSCompliant(false)]
			public static IsShader glIsShader;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsSync(IntPtr sync);
			[CLSCompliant(false)]
			public static IsSync glIsSync;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsTexture(UInt32 texture);
			[CLSCompliant(false)]
			public static IsTexture glIsTexture;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsTextureEXT(UInt32 texture);
			[CLSCompliant(false)]
			public static IsTextureEXT glIsTextureEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsTransformFeedback(UInt32 id);
			[CLSCompliant(false)]
			public static IsTransformFeedback glIsTransformFeedback;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsTransformFeedbackNV(UInt32 id);
			[CLSCompliant(false)]
			public static IsTransformFeedbackNV glIsTransformFeedbackNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsVariantEnabledEXT(UInt32 id, System.Graphics.OGL.ExtVertexShader cap);
			[CLSCompliant(false)]
			public static IsVariantEnabledEXT glIsVariantEnabledEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsVertexArray(UInt32 array);
			[CLSCompliant(false)]
			public static IsVertexArray glIsVertexArray;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsVertexArrayAPPLE(UInt32 array);
			[CLSCompliant(false)]
			public static IsVertexArrayAPPLE glIsVertexArrayAPPLE;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool IsVertexAttribEnabledAPPLE(UInt32 index, System.Graphics.OGL.AppleVertexProgramEvaluators pname);
			[CLSCompliant(false)]
			public static IsVertexAttribEnabledAPPLE glIsVertexAttribEnabledAPPLE;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void LightEnviSGIX(System.Graphics.OGL.SgixFragmentLighting pname, Int32 param);
			[CLSCompliant(false)]
			public static LightEnviSGIX glLightEnviSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Lightf(System.Graphics.OGL.LightName light, System.Graphics.OGL.LightParameter pname, Single param);
			[CLSCompliant(false)]
			public static Lightf glLightf;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Lightfv(System.Graphics.OGL.LightName light, System.Graphics.OGL.LightParameter pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static Lightfv glLightfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Lighti(System.Graphics.OGL.LightName light, System.Graphics.OGL.LightParameter pname, Int32 param);
			[CLSCompliant(false)]
			public static Lighti glLighti;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Lightiv(System.Graphics.OGL.LightName light, System.Graphics.OGL.LightParameter pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static Lightiv glLightiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void LightModelf(System.Graphics.OGL.LightModelParameter pname, Single param);
			[CLSCompliant(false)]
			public static LightModelf glLightModelf;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void LightModelfv(System.Graphics.OGL.LightModelParameter pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static LightModelfv glLightModelfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void LightModeli(System.Graphics.OGL.LightModelParameter pname, Int32 param);
			[CLSCompliant(false)]
			public static LightModeli glLightModeli;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void LightModeliv(System.Graphics.OGL.LightModelParameter pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static LightModeliv glLightModeliv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void LineStipple(Int32 factor, UInt16 pattern);
			[CLSCompliant(false)]
			public static LineStipple glLineStipple;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void LineWidth(Single width);
			[CLSCompliant(false)]
			public static LineWidth glLineWidth;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void LinkProgram(UInt32 program);
			[CLSCompliant(false)]
			public static LinkProgram glLinkProgram;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void LinkProgramARB(UInt32 programObj);
			[CLSCompliant(false)]
			public static LinkProgramARB glLinkProgramARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ListBase(UInt32 @base);
			[CLSCompliant(false)]
			public static ListBase glListBase;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ListParameterfSGIX(UInt32 list, System.Graphics.OGL.ListParameterName pname, Single param);
			[CLSCompliant(false)]
			public static ListParameterfSGIX glListParameterfSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ListParameterfvSGIX(UInt32 list, System.Graphics.OGL.ListParameterName pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static ListParameterfvSGIX glListParameterfvSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ListParameteriSGIX(UInt32 list, System.Graphics.OGL.ListParameterName pname, Int32 param);
			[CLSCompliant(false)]
			public static ListParameteriSGIX glListParameteriSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ListParameterivSGIX(UInt32 list, System.Graphics.OGL.ListParameterName pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static ListParameterivSGIX glListParameterivSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void LoadIdentity();
			[CLSCompliant(false)]
			public static LoadIdentity glLoadIdentity;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void LoadIdentityDeformationMapSGIX(UInt32 mask);
			[CLSCompliant(false)]
			public static LoadIdentityDeformationMapSGIX glLoadIdentityDeformationMapSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void LoadMatrixd(Double* m);
			[CLSCompliant(false)]
			public unsafe static LoadMatrixd glLoadMatrixd;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void LoadMatrixf(Single* m);
			[CLSCompliant(false)]
			public unsafe static LoadMatrixf glLoadMatrixf;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void LoadName(UInt32 name);
			[CLSCompliant(false)]
			public static LoadName glLoadName;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void LoadProgramNV(System.Graphics.OGL.AssemblyProgramTargetArb target, UInt32 id, Int32 len, Byte* program);
			[CLSCompliant(false)]
			public unsafe static LoadProgramNV glLoadProgramNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void LoadTransposeMatrixd(Double* m);
			[CLSCompliant(false)]
			public unsafe static LoadTransposeMatrixd glLoadTransposeMatrixd;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void LoadTransposeMatrixdARB(Double* m);
			[CLSCompliant(false)]
			public unsafe static LoadTransposeMatrixdARB glLoadTransposeMatrixdARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void LoadTransposeMatrixf(Single* m);
			[CLSCompliant(false)]
			public unsafe static LoadTransposeMatrixf glLoadTransposeMatrixf;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void LoadTransposeMatrixfARB(Single* m);
			[CLSCompliant(false)]
			public unsafe static LoadTransposeMatrixfARB glLoadTransposeMatrixfARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void LockArraysEXT(Int32 first, Int32 count);
			[CLSCompliant(false)]
			public static LockArraysEXT glLockArraysEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void LogicOp(System.Graphics.OGL.LogicOp opcode);
			[CLSCompliant(false)]
			public static LogicOp glLogicOp;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MakeBufferNonResidentNV(System.Graphics.OGL.NvShaderBufferLoad target);
			[CLSCompliant(false)]
			public static MakeBufferNonResidentNV glMakeBufferNonResidentNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MakeBufferResidentNV(System.Graphics.OGL.NvShaderBufferLoad target, System.Graphics.OGL.NvShaderBufferLoad access);
			[CLSCompliant(false)]
			public static MakeBufferResidentNV glMakeBufferResidentNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MakeNamedBufferNonResidentNV(UInt32 buffer);
			[CLSCompliant(false)]
			public static MakeNamedBufferNonResidentNV glMakeNamedBufferNonResidentNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MakeNamedBufferResidentNV(UInt32 buffer, System.Graphics.OGL.NvShaderBufferLoad access);
			[CLSCompliant(false)]
			public static MakeNamedBufferResidentNV glMakeNamedBufferResidentNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Map1d(System.Graphics.OGL.MapTarget target, Double u1, Double u2, Int32 stride, Int32 order, Double* points);
			[CLSCompliant(false)]
			public unsafe static Map1d glMap1d;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Map1f(System.Graphics.OGL.MapTarget target, Single u1, Single u2, Int32 stride, Int32 order, Single* points);
			[CLSCompliant(false)]
			public unsafe static Map1f glMap1f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Map2d(System.Graphics.OGL.MapTarget target, Double u1, Double u2, Int32 ustride, Int32 uorder, Double v1, Double v2, Int32 vstride, Int32 vorder, Double* points);
			[CLSCompliant(false)]
			public unsafe static Map2d glMap2d;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Map2f(System.Graphics.OGL.MapTarget target, Single u1, Single u2, Int32 ustride, Int32 uorder, Single v1, Single v2, Int32 vstride, Int32 vorder, Single* points);
			[CLSCompliant(false)]
			public unsafe static Map2f glMap2f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate IntPtr MapBuffer(System.Graphics.OGL.BufferTarget target, System.Graphics.OGL.BufferAccess access);
			[CLSCompliant(false)]
			public unsafe static MapBuffer glMapBuffer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate IntPtr MapBufferARB(System.Graphics.OGL.BufferTargetArb target, System.Graphics.OGL.ArbVertexBufferObject access);
			[CLSCompliant(false)]
			public unsafe static MapBufferARB glMapBufferARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate IntPtr MapBufferRange(System.Graphics.OGL.BufferTarget target, IntPtr offset, IntPtr length, System.Graphics.OGL.BufferAccessMask access);
			[CLSCompliant(false)]
			public unsafe static MapBufferRange glMapBufferRange;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MapControlPointsNV(System.Graphics.OGL.NvEvaluators target, UInt32 index, System.Graphics.OGL.NvEvaluators type, Int32 ustride, Int32 vstride, Int32 uorder, Int32 vorder, bool packed, IntPtr points);
			[CLSCompliant(false)]
			public static MapControlPointsNV glMapControlPointsNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MapGrid1d(Int32 un, Double u1, Double u2);
			[CLSCompliant(false)]
			public static MapGrid1d glMapGrid1d;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MapGrid1f(Int32 un, Single u1, Single u2);
			[CLSCompliant(false)]
			public static MapGrid1f glMapGrid1f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MapGrid2d(Int32 un, Double u1, Double u2, Int32 vn, Double v1, Double v2);
			[CLSCompliant(false)]
			public static MapGrid2d glMapGrid2d;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MapGrid2f(Int32 un, Single u1, Single u2, Int32 vn, Single v1, Single v2);
			[CLSCompliant(false)]
			public static MapGrid2f glMapGrid2f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate IntPtr MapNamedBufferEXT(UInt32 buffer, System.Graphics.OGL.ExtDirectStateAccess access);
			[CLSCompliant(false)]
			public unsafe static MapNamedBufferEXT glMapNamedBufferEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate IntPtr MapNamedBufferRangeEXT(UInt32 buffer, IntPtr offset, IntPtr length, System.Graphics.OGL.BufferAccessMask access);
			[CLSCompliant(false)]
			public unsafe static MapNamedBufferRangeEXT glMapNamedBufferRangeEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate IntPtr MapObjectBufferATI(UInt32 buffer);
			[CLSCompliant(false)]
			public unsafe static MapObjectBufferATI glMapObjectBufferATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MapParameterfvNV(System.Graphics.OGL.NvEvaluators target, System.Graphics.OGL.NvEvaluators pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static MapParameterfvNV glMapParameterfvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MapParameterivNV(System.Graphics.OGL.NvEvaluators target, System.Graphics.OGL.NvEvaluators pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static MapParameterivNV glMapParameterivNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MapVertexAttrib1dAPPLE(UInt32 index, UInt32 size, Double u1, Double u2, Int32 stride, Int32 order, Double* points);
			[CLSCompliant(false)]
			public unsafe static MapVertexAttrib1dAPPLE glMapVertexAttrib1dAPPLE;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MapVertexAttrib1fAPPLE(UInt32 index, UInt32 size, Single u1, Single u2, Int32 stride, Int32 order, Single* points);
			[CLSCompliant(false)]
			public unsafe static MapVertexAttrib1fAPPLE glMapVertexAttrib1fAPPLE;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MapVertexAttrib2dAPPLE(UInt32 index, UInt32 size, Double u1, Double u2, Int32 ustride, Int32 uorder, Double v1, Double v2, Int32 vstride, Int32 vorder, Double* points);
			[CLSCompliant(false)]
			public unsafe static MapVertexAttrib2dAPPLE glMapVertexAttrib2dAPPLE;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MapVertexAttrib2fAPPLE(UInt32 index, UInt32 size, Single u1, Single u2, Int32 ustride, Int32 uorder, Single v1, Single v2, Int32 vstride, Int32 vorder, Single* points);
			[CLSCompliant(false)]
			public unsafe static MapVertexAttrib2fAPPLE glMapVertexAttrib2fAPPLE;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Materialf(System.Graphics.OGL.MaterialFace face, System.Graphics.OGL.MaterialParameter pname, Single param);
			[CLSCompliant(false)]
			public static Materialf glMaterialf;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Materialfv(System.Graphics.OGL.MaterialFace face, System.Graphics.OGL.MaterialParameter pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static Materialfv glMaterialfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Materiali(System.Graphics.OGL.MaterialFace face, System.Graphics.OGL.MaterialParameter pname, Int32 param);
			[CLSCompliant(false)]
			public static Materiali glMateriali;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Materialiv(System.Graphics.OGL.MaterialFace face, System.Graphics.OGL.MaterialParameter pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static Materialiv glMaterialiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MatrixFrustumEXT(System.Graphics.OGL.MatrixMode mode, Double left, Double right, Double bottom, Double top, Double zNear, Double zFar);
			[CLSCompliant(false)]
			public static MatrixFrustumEXT glMatrixFrustumEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MatrixIndexPointerARB(Int32 size, System.Graphics.OGL.ArbMatrixPalette type, Int32 stride, IntPtr pointer);
			[CLSCompliant(false)]
			public static MatrixIndexPointerARB glMatrixIndexPointerARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MatrixIndexubvARB(Int32 size, Byte* indices);
			[CLSCompliant(false)]
			public unsafe static MatrixIndexubvARB glMatrixIndexubvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MatrixIndexuivARB(Int32 size, UInt32* indices);
			[CLSCompliant(false)]
			public unsafe static MatrixIndexuivARB glMatrixIndexuivARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MatrixIndexusvARB(Int32 size, UInt16* indices);
			[CLSCompliant(false)]
			public unsafe static MatrixIndexusvARB glMatrixIndexusvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MatrixLoaddEXT(System.Graphics.OGL.MatrixMode mode, Double* m);
			[CLSCompliant(false)]
			public unsafe static MatrixLoaddEXT glMatrixLoaddEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MatrixLoadfEXT(System.Graphics.OGL.MatrixMode mode, Single* m);
			[CLSCompliant(false)]
			public unsafe static MatrixLoadfEXT glMatrixLoadfEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MatrixLoadIdentityEXT(System.Graphics.OGL.MatrixMode mode);
			[CLSCompliant(false)]
			public static MatrixLoadIdentityEXT glMatrixLoadIdentityEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MatrixLoadTransposedEXT(System.Graphics.OGL.MatrixMode mode, Double* m);
			[CLSCompliant(false)]
			public unsafe static MatrixLoadTransposedEXT glMatrixLoadTransposedEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MatrixLoadTransposefEXT(System.Graphics.OGL.MatrixMode mode, Single* m);
			[CLSCompliant(false)]
			public unsafe static MatrixLoadTransposefEXT glMatrixLoadTransposefEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MatrixMode(System.Graphics.OGL.MatrixMode mode);
			[CLSCompliant(false)]
			public static MatrixMode glMatrixMode;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MatrixMultdEXT(System.Graphics.OGL.MatrixMode mode, Double* m);
			[CLSCompliant(false)]
			public unsafe static MatrixMultdEXT glMatrixMultdEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MatrixMultfEXT(System.Graphics.OGL.MatrixMode mode, Single* m);
			[CLSCompliant(false)]
			public unsafe static MatrixMultfEXT glMatrixMultfEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MatrixMultTransposedEXT(System.Graphics.OGL.MatrixMode mode, Double* m);
			[CLSCompliant(false)]
			public unsafe static MatrixMultTransposedEXT glMatrixMultTransposedEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MatrixMultTransposefEXT(System.Graphics.OGL.MatrixMode mode, Single* m);
			[CLSCompliant(false)]
			public unsafe static MatrixMultTransposefEXT glMatrixMultTransposefEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MatrixOrthoEXT(System.Graphics.OGL.MatrixMode mode, Double left, Double right, Double bottom, Double top, Double zNear, Double zFar);
			[CLSCompliant(false)]
			public static MatrixOrthoEXT glMatrixOrthoEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MatrixPopEXT(System.Graphics.OGL.MatrixMode mode);
			[CLSCompliant(false)]
			public static MatrixPopEXT glMatrixPopEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MatrixPushEXT(System.Graphics.OGL.MatrixMode mode);
			[CLSCompliant(false)]
			public static MatrixPushEXT glMatrixPushEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MatrixRotatedEXT(System.Graphics.OGL.MatrixMode mode, Double angle, Double x, Double y, Double z);
			[CLSCompliant(false)]
			public static MatrixRotatedEXT glMatrixRotatedEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MatrixRotatefEXT(System.Graphics.OGL.MatrixMode mode, Single angle, Single x, Single y, Single z);
			[CLSCompliant(false)]
			public static MatrixRotatefEXT glMatrixRotatefEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MatrixScaledEXT(System.Graphics.OGL.MatrixMode mode, Double x, Double y, Double z);
			[CLSCompliant(false)]
			public static MatrixScaledEXT glMatrixScaledEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MatrixScalefEXT(System.Graphics.OGL.MatrixMode mode, Single x, Single y, Single z);
			[CLSCompliant(false)]
			public static MatrixScalefEXT glMatrixScalefEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MatrixTranslatedEXT(System.Graphics.OGL.MatrixMode mode, Double x, Double y, Double z);
			[CLSCompliant(false)]
			public static MatrixTranslatedEXT glMatrixTranslatedEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MatrixTranslatefEXT(System.Graphics.OGL.MatrixMode mode, Single x, Single y, Single z);
			[CLSCompliant(false)]
			public static MatrixTranslatefEXT glMatrixTranslatefEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MemoryBarrierEXT(UInt32 barriers);
			[CLSCompliant(false)]
			public static MemoryBarrierEXT glMemoryBarrierEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Minmax(System.Graphics.OGL.MinmaxTarget target, System.Graphics.OGL.PixelInternalFormat internalformat, bool sink);
			[CLSCompliant(false)]
			public static Minmax glMinmax;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MinmaxEXT(System.Graphics.OGL.ExtHistogram target, System.Graphics.OGL.PixelInternalFormat internalformat, bool sink);
			[CLSCompliant(false)]
			public static MinmaxEXT glMinmaxEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MinSampleShading(Single value);
			[CLSCompliant(false)]
			public static MinSampleShading glMinSampleShading;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MinSampleShadingARB(Single value);
			[CLSCompliant(false)]
			public static MinSampleShadingARB glMinSampleShadingARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiDrawArrays(System.Graphics.OGL.BeginMode mode, Int32* first, Int32* count, Int32 primcount);
			[CLSCompliant(false)]
			public unsafe static MultiDrawArrays glMultiDrawArrays;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiDrawArraysEXT(System.Graphics.OGL.BeginMode mode, Int32* first, Int32* count, Int32 primcount);
			[CLSCompliant(false)]
			public unsafe static MultiDrawArraysEXT glMultiDrawArraysEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiDrawArraysIndirectAMD(System.Graphics.OGL.AmdMultiDrawIndirect mode, IntPtr indirect, Int32 primcount, Int32 stride);
			[CLSCompliant(false)]
			public static MultiDrawArraysIndirectAMD glMultiDrawArraysIndirectAMD;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiDrawElementArrayAPPLE(System.Graphics.OGL.BeginMode mode, Int32* first, Int32* count, Int32 primcount);
			[CLSCompliant(false)]
			public unsafe static MultiDrawElementArrayAPPLE glMultiDrawElementArrayAPPLE;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiDrawElements(System.Graphics.OGL.BeginMode mode, Int32* count, System.Graphics.OGL.DrawElementsType type, IntPtr indices, Int32 primcount);
			[CLSCompliant(false)]
			public unsafe static MultiDrawElements glMultiDrawElements;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiDrawElementsBaseVertex(System.Graphics.OGL.BeginMode mode, Int32* count, System.Graphics.OGL.DrawElementsType type, IntPtr indices, Int32 primcount, Int32* basevertex);
			[CLSCompliant(false)]
			public unsafe static MultiDrawElementsBaseVertex glMultiDrawElementsBaseVertex;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiDrawElementsEXT(System.Graphics.OGL.BeginMode mode, Int32* count, System.Graphics.OGL.DrawElementsType type, IntPtr indices, Int32 primcount);
			[CLSCompliant(false)]
			public unsafe static MultiDrawElementsEXT glMultiDrawElementsEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiDrawElementsIndirectAMD(System.Graphics.OGL.AmdMultiDrawIndirect mode, System.Graphics.OGL.AmdMultiDrawIndirect type, IntPtr indirect, Int32 primcount, Int32 stride);
			[CLSCompliant(false)]
			public static MultiDrawElementsIndirectAMD glMultiDrawElementsIndirectAMD;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiDrawRangeElementArrayAPPLE(System.Graphics.OGL.BeginMode mode, UInt32 start, UInt32 end, Int32* first, Int32* count, Int32 primcount);
			[CLSCompliant(false)]
			public unsafe static MultiDrawRangeElementArrayAPPLE glMultiDrawRangeElementArrayAPPLE;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiModeDrawArraysIBM(System.Graphics.OGL.BeginMode* mode, Int32* first, Int32* count, Int32 primcount, Int32 modestride);
			[CLSCompliant(false)]
			public unsafe static MultiModeDrawArraysIBM glMultiModeDrawArraysIBM;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiModeDrawElementsIBM(System.Graphics.OGL.BeginMode* mode, Int32* count, System.Graphics.OGL.DrawElementsType type, IntPtr indices, Int32 primcount, Int32 modestride);
			[CLSCompliant(false)]
			public unsafe static MultiModeDrawElementsIBM glMultiModeDrawElementsIBM;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexBufferEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureTarget target, System.Graphics.OGL.ExtDirectStateAccess internalformat, UInt32 buffer);
			[CLSCompliant(false)]
			public static MultiTexBufferEXT glMultiTexBufferEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexCoord1d(System.Graphics.OGL.TextureUnit target, Double s);
			[CLSCompliant(false)]
			public static MultiTexCoord1d glMultiTexCoord1d;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexCoord1dARB(System.Graphics.OGL.TextureUnit target, Double s);
			[CLSCompliant(false)]
			public static MultiTexCoord1dARB glMultiTexCoord1dARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexCoord1dv(System.Graphics.OGL.TextureUnit target, Double* v);
			[CLSCompliant(false)]
			public unsafe static MultiTexCoord1dv glMultiTexCoord1dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexCoord1dvARB(System.Graphics.OGL.TextureUnit target, Double* v);
			[CLSCompliant(false)]
			public unsafe static MultiTexCoord1dvARB glMultiTexCoord1dvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexCoord1f(System.Graphics.OGL.TextureUnit target, Single s);
			[CLSCompliant(false)]
			public static MultiTexCoord1f glMultiTexCoord1f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexCoord1fARB(System.Graphics.OGL.TextureUnit target, Single s);
			[CLSCompliant(false)]
			public static MultiTexCoord1fARB glMultiTexCoord1fARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexCoord1fv(System.Graphics.OGL.TextureUnit target, Single* v);
			[CLSCompliant(false)]
			public unsafe static MultiTexCoord1fv glMultiTexCoord1fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexCoord1fvARB(System.Graphics.OGL.TextureUnit target, Single* v);
			[CLSCompliant(false)]
			public unsafe static MultiTexCoord1fvARB glMultiTexCoord1fvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexCoord1hNV(System.Graphics.OGL.TextureUnit target, Half s);
			[CLSCompliant(false)]
			public static MultiTexCoord1hNV glMultiTexCoord1hNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexCoord1hvNV(System.Graphics.OGL.TextureUnit target, Half* v);
			[CLSCompliant(false)]
			public unsafe static MultiTexCoord1hvNV glMultiTexCoord1hvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexCoord1i(System.Graphics.OGL.TextureUnit target, Int32 s);
			[CLSCompliant(false)]
			public static MultiTexCoord1i glMultiTexCoord1i;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexCoord1iARB(System.Graphics.OGL.TextureUnit target, Int32 s);
			[CLSCompliant(false)]
			public static MultiTexCoord1iARB glMultiTexCoord1iARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexCoord1iv(System.Graphics.OGL.TextureUnit target, Int32* v);
			[CLSCompliant(false)]
			public unsafe static MultiTexCoord1iv glMultiTexCoord1iv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexCoord1ivARB(System.Graphics.OGL.TextureUnit target, Int32* v);
			[CLSCompliant(false)]
			public unsafe static MultiTexCoord1ivARB glMultiTexCoord1ivARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexCoord1s(System.Graphics.OGL.TextureUnit target, Int16 s);
			[CLSCompliant(false)]
			public static MultiTexCoord1s glMultiTexCoord1s;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexCoord1sARB(System.Graphics.OGL.TextureUnit target, Int16 s);
			[CLSCompliant(false)]
			public static MultiTexCoord1sARB glMultiTexCoord1sARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexCoord1sv(System.Graphics.OGL.TextureUnit target, Int16* v);
			[CLSCompliant(false)]
			public unsafe static MultiTexCoord1sv glMultiTexCoord1sv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexCoord1svARB(System.Graphics.OGL.TextureUnit target, Int16* v);
			[CLSCompliant(false)]
			public unsafe static MultiTexCoord1svARB glMultiTexCoord1svARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexCoord2d(System.Graphics.OGL.TextureUnit target, Double s, Double t);
			[CLSCompliant(false)]
			public static MultiTexCoord2d glMultiTexCoord2d;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexCoord2dARB(System.Graphics.OGL.TextureUnit target, Double s, Double t);
			[CLSCompliant(false)]
			public static MultiTexCoord2dARB glMultiTexCoord2dARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexCoord2dv(System.Graphics.OGL.TextureUnit target, Double* v);
			[CLSCompliant(false)]
			public unsafe static MultiTexCoord2dv glMultiTexCoord2dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexCoord2dvARB(System.Graphics.OGL.TextureUnit target, Double* v);
			[CLSCompliant(false)]
			public unsafe static MultiTexCoord2dvARB glMultiTexCoord2dvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexCoord2f(System.Graphics.OGL.TextureUnit target, Single s, Single t);
			[CLSCompliant(false)]
			public static MultiTexCoord2f glMultiTexCoord2f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexCoord2fARB(System.Graphics.OGL.TextureUnit target, Single s, Single t);
			[CLSCompliant(false)]
			public static MultiTexCoord2fARB glMultiTexCoord2fARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexCoord2fv(System.Graphics.OGL.TextureUnit target, Single* v);
			[CLSCompliant(false)]
			public unsafe static MultiTexCoord2fv glMultiTexCoord2fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexCoord2fvARB(System.Graphics.OGL.TextureUnit target, Single* v);
			[CLSCompliant(false)]
			public unsafe static MultiTexCoord2fvARB glMultiTexCoord2fvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexCoord2hNV(System.Graphics.OGL.TextureUnit target, Half s, Half t);
			[CLSCompliant(false)]
			public static MultiTexCoord2hNV glMultiTexCoord2hNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexCoord2hvNV(System.Graphics.OGL.TextureUnit target, Half* v);
			[CLSCompliant(false)]
			public unsafe static MultiTexCoord2hvNV glMultiTexCoord2hvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexCoord2i(System.Graphics.OGL.TextureUnit target, Int32 s, Int32 t);
			[CLSCompliant(false)]
			public static MultiTexCoord2i glMultiTexCoord2i;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexCoord2iARB(System.Graphics.OGL.TextureUnit target, Int32 s, Int32 t);
			[CLSCompliant(false)]
			public static MultiTexCoord2iARB glMultiTexCoord2iARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexCoord2iv(System.Graphics.OGL.TextureUnit target, Int32* v);
			[CLSCompliant(false)]
			public unsafe static MultiTexCoord2iv glMultiTexCoord2iv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexCoord2ivARB(System.Graphics.OGL.TextureUnit target, Int32* v);
			[CLSCompliant(false)]
			public unsafe static MultiTexCoord2ivARB glMultiTexCoord2ivARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexCoord2s(System.Graphics.OGL.TextureUnit target, Int16 s, Int16 t);
			[CLSCompliant(false)]
			public static MultiTexCoord2s glMultiTexCoord2s;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexCoord2sARB(System.Graphics.OGL.TextureUnit target, Int16 s, Int16 t);
			[CLSCompliant(false)]
			public static MultiTexCoord2sARB glMultiTexCoord2sARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexCoord2sv(System.Graphics.OGL.TextureUnit target, Int16* v);
			[CLSCompliant(false)]
			public unsafe static MultiTexCoord2sv glMultiTexCoord2sv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexCoord2svARB(System.Graphics.OGL.TextureUnit target, Int16* v);
			[CLSCompliant(false)]
			public unsafe static MultiTexCoord2svARB glMultiTexCoord2svARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexCoord3d(System.Graphics.OGL.TextureUnit target, Double s, Double t, Double r);
			[CLSCompliant(false)]
			public static MultiTexCoord3d glMultiTexCoord3d;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexCoord3dARB(System.Graphics.OGL.TextureUnit target, Double s, Double t, Double r);
			[CLSCompliant(false)]
			public static MultiTexCoord3dARB glMultiTexCoord3dARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexCoord3dv(System.Graphics.OGL.TextureUnit target, Double* v);
			[CLSCompliant(false)]
			public unsafe static MultiTexCoord3dv glMultiTexCoord3dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexCoord3dvARB(System.Graphics.OGL.TextureUnit target, Double* v);
			[CLSCompliant(false)]
			public unsafe static MultiTexCoord3dvARB glMultiTexCoord3dvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexCoord3f(System.Graphics.OGL.TextureUnit target, Single s, Single t, Single r);
			[CLSCompliant(false)]
			public static MultiTexCoord3f glMultiTexCoord3f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexCoord3fARB(System.Graphics.OGL.TextureUnit target, Single s, Single t, Single r);
			[CLSCompliant(false)]
			public static MultiTexCoord3fARB glMultiTexCoord3fARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexCoord3fv(System.Graphics.OGL.TextureUnit target, Single* v);
			[CLSCompliant(false)]
			public unsafe static MultiTexCoord3fv glMultiTexCoord3fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexCoord3fvARB(System.Graphics.OGL.TextureUnit target, Single* v);
			[CLSCompliant(false)]
			public unsafe static MultiTexCoord3fvARB glMultiTexCoord3fvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexCoord3hNV(System.Graphics.OGL.TextureUnit target, Half s, Half t, Half r);
			[CLSCompliant(false)]
			public static MultiTexCoord3hNV glMultiTexCoord3hNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexCoord3hvNV(System.Graphics.OGL.TextureUnit target, Half* v);
			[CLSCompliant(false)]
			public unsafe static MultiTexCoord3hvNV glMultiTexCoord3hvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexCoord3i(System.Graphics.OGL.TextureUnit target, Int32 s, Int32 t, Int32 r);
			[CLSCompliant(false)]
			public static MultiTexCoord3i glMultiTexCoord3i;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexCoord3iARB(System.Graphics.OGL.TextureUnit target, Int32 s, Int32 t, Int32 r);
			[CLSCompliant(false)]
			public static MultiTexCoord3iARB glMultiTexCoord3iARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexCoord3iv(System.Graphics.OGL.TextureUnit target, Int32* v);
			[CLSCompliant(false)]
			public unsafe static MultiTexCoord3iv glMultiTexCoord3iv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexCoord3ivARB(System.Graphics.OGL.TextureUnit target, Int32* v);
			[CLSCompliant(false)]
			public unsafe static MultiTexCoord3ivARB glMultiTexCoord3ivARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexCoord3s(System.Graphics.OGL.TextureUnit target, Int16 s, Int16 t, Int16 r);
			[CLSCompliant(false)]
			public static MultiTexCoord3s glMultiTexCoord3s;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexCoord3sARB(System.Graphics.OGL.TextureUnit target, Int16 s, Int16 t, Int16 r);
			[CLSCompliant(false)]
			public static MultiTexCoord3sARB glMultiTexCoord3sARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexCoord3sv(System.Graphics.OGL.TextureUnit target, Int16* v);
			[CLSCompliant(false)]
			public unsafe static MultiTexCoord3sv glMultiTexCoord3sv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexCoord3svARB(System.Graphics.OGL.TextureUnit target, Int16* v);
			[CLSCompliant(false)]
			public unsafe static MultiTexCoord3svARB glMultiTexCoord3svARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexCoord4d(System.Graphics.OGL.TextureUnit target, Double s, Double t, Double r, Double q);
			[CLSCompliant(false)]
			public static MultiTexCoord4d glMultiTexCoord4d;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexCoord4dARB(System.Graphics.OGL.TextureUnit target, Double s, Double t, Double r, Double q);
			[CLSCompliant(false)]
			public static MultiTexCoord4dARB glMultiTexCoord4dARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexCoord4dv(System.Graphics.OGL.TextureUnit target, Double* v);
			[CLSCompliant(false)]
			public unsafe static MultiTexCoord4dv glMultiTexCoord4dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexCoord4dvARB(System.Graphics.OGL.TextureUnit target, Double* v);
			[CLSCompliant(false)]
			public unsafe static MultiTexCoord4dvARB glMultiTexCoord4dvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexCoord4f(System.Graphics.OGL.TextureUnit target, Single s, Single t, Single r, Single q);
			[CLSCompliant(false)]
			public static MultiTexCoord4f glMultiTexCoord4f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexCoord4fARB(System.Graphics.OGL.TextureUnit target, Single s, Single t, Single r, Single q);
			[CLSCompliant(false)]
			public static MultiTexCoord4fARB glMultiTexCoord4fARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexCoord4fv(System.Graphics.OGL.TextureUnit target, Single* v);
			[CLSCompliant(false)]
			public unsafe static MultiTexCoord4fv glMultiTexCoord4fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexCoord4fvARB(System.Graphics.OGL.TextureUnit target, Single* v);
			[CLSCompliant(false)]
			public unsafe static MultiTexCoord4fvARB glMultiTexCoord4fvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexCoord4hNV(System.Graphics.OGL.TextureUnit target, Half s, Half t, Half r, Half q);
			[CLSCompliant(false)]
			public static MultiTexCoord4hNV glMultiTexCoord4hNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexCoord4hvNV(System.Graphics.OGL.TextureUnit target, Half* v);
			[CLSCompliant(false)]
			public unsafe static MultiTexCoord4hvNV glMultiTexCoord4hvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexCoord4i(System.Graphics.OGL.TextureUnit target, Int32 s, Int32 t, Int32 r, Int32 q);
			[CLSCompliant(false)]
			public static MultiTexCoord4i glMultiTexCoord4i;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexCoord4iARB(System.Graphics.OGL.TextureUnit target, Int32 s, Int32 t, Int32 r, Int32 q);
			[CLSCompliant(false)]
			public static MultiTexCoord4iARB glMultiTexCoord4iARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexCoord4iv(System.Graphics.OGL.TextureUnit target, Int32* v);
			[CLSCompliant(false)]
			public unsafe static MultiTexCoord4iv glMultiTexCoord4iv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexCoord4ivARB(System.Graphics.OGL.TextureUnit target, Int32* v);
			[CLSCompliant(false)]
			public unsafe static MultiTexCoord4ivARB glMultiTexCoord4ivARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexCoord4s(System.Graphics.OGL.TextureUnit target, Int16 s, Int16 t, Int16 r, Int16 q);
			[CLSCompliant(false)]
			public static MultiTexCoord4s glMultiTexCoord4s;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexCoord4sARB(System.Graphics.OGL.TextureUnit target, Int16 s, Int16 t, Int16 r, Int16 q);
			[CLSCompliant(false)]
			public static MultiTexCoord4sARB glMultiTexCoord4sARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexCoord4sv(System.Graphics.OGL.TextureUnit target, Int16* v);
			[CLSCompliant(false)]
			public unsafe static MultiTexCoord4sv glMultiTexCoord4sv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexCoord4svARB(System.Graphics.OGL.TextureUnit target, Int16* v);
			[CLSCompliant(false)]
			public unsafe static MultiTexCoord4svARB glMultiTexCoord4svARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexCoordP1ui(System.Graphics.OGL.TextureUnit texture, System.Graphics.OGL.PackedPointerType type, UInt32 coords);
			[CLSCompliant(false)]
			public static MultiTexCoordP1ui glMultiTexCoordP1ui;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexCoordP1uiv(System.Graphics.OGL.TextureUnit texture, System.Graphics.OGL.PackedPointerType type, UInt32* coords);
			[CLSCompliant(false)]
			public unsafe static MultiTexCoordP1uiv glMultiTexCoordP1uiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexCoordP2ui(System.Graphics.OGL.TextureUnit texture, System.Graphics.OGL.PackedPointerType type, UInt32 coords);
			[CLSCompliant(false)]
			public static MultiTexCoordP2ui glMultiTexCoordP2ui;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexCoordP2uiv(System.Graphics.OGL.TextureUnit texture, System.Graphics.OGL.PackedPointerType type, UInt32* coords);
			[CLSCompliant(false)]
			public unsafe static MultiTexCoordP2uiv glMultiTexCoordP2uiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexCoordP3ui(System.Graphics.OGL.TextureUnit texture, System.Graphics.OGL.PackedPointerType type, UInt32 coords);
			[CLSCompliant(false)]
			public static MultiTexCoordP3ui glMultiTexCoordP3ui;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexCoordP3uiv(System.Graphics.OGL.TextureUnit texture, System.Graphics.OGL.PackedPointerType type, UInt32* coords);
			[CLSCompliant(false)]
			public unsafe static MultiTexCoordP3uiv glMultiTexCoordP3uiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexCoordP4ui(System.Graphics.OGL.TextureUnit texture, System.Graphics.OGL.PackedPointerType type, UInt32 coords);
			[CLSCompliant(false)]
			public static MultiTexCoordP4ui glMultiTexCoordP4ui;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexCoordP4uiv(System.Graphics.OGL.TextureUnit texture, System.Graphics.OGL.PackedPointerType type, UInt32* coords);
			[CLSCompliant(false)]
			public unsafe static MultiTexCoordP4uiv glMultiTexCoordP4uiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexCoordPointerEXT(System.Graphics.OGL.TextureUnit texunit, Int32 size, System.Graphics.OGL.TexCoordPointerType type, Int32 stride, IntPtr pointer);
			[CLSCompliant(false)]
			public static MultiTexCoordPointerEXT glMultiTexCoordPointerEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexEnvfEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureEnvTarget target, System.Graphics.OGL.TextureEnvParameter pname, Single param);
			[CLSCompliant(false)]
			public static MultiTexEnvfEXT glMultiTexEnvfEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexEnvfvEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureEnvTarget target, System.Graphics.OGL.TextureEnvParameter pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static MultiTexEnvfvEXT glMultiTexEnvfvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexEnviEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureEnvTarget target, System.Graphics.OGL.TextureEnvParameter pname, Int32 param);
			[CLSCompliant(false)]
			public static MultiTexEnviEXT glMultiTexEnviEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexEnvivEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureEnvTarget target, System.Graphics.OGL.TextureEnvParameter pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static MultiTexEnvivEXT glMultiTexEnvivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexGendEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureCoordName coord, System.Graphics.OGL.TextureGenParameter pname, Double param);
			[CLSCompliant(false)]
			public static MultiTexGendEXT glMultiTexGendEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexGendvEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureCoordName coord, System.Graphics.OGL.TextureGenParameter pname, Double* @params);
			[CLSCompliant(false)]
			public unsafe static MultiTexGendvEXT glMultiTexGendvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexGenfEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureCoordName coord, System.Graphics.OGL.TextureGenParameter pname, Single param);
			[CLSCompliant(false)]
			public static MultiTexGenfEXT glMultiTexGenfEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexGenfvEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureCoordName coord, System.Graphics.OGL.TextureGenParameter pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static MultiTexGenfvEXT glMultiTexGenfvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexGeniEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureCoordName coord, System.Graphics.OGL.TextureGenParameter pname, Int32 param);
			[CLSCompliant(false)]
			public static MultiTexGeniEXT glMultiTexGeniEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexGenivEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureCoordName coord, System.Graphics.OGL.TextureGenParameter pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static MultiTexGenivEXT glMultiTexGenivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexImage1DEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureTarget target, Int32 level, System.Graphics.OGL.ExtDirectStateAccess internalformat, Int32 width, Int32 border, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, IntPtr pixels);
			[CLSCompliant(false)]
			public static MultiTexImage1DEXT glMultiTexImage1DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexImage2DEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureTarget target, Int32 level, System.Graphics.OGL.ExtDirectStateAccess internalformat, Int32 width, Int32 height, Int32 border, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, IntPtr pixels);
			[CLSCompliant(false)]
			public static MultiTexImage2DEXT glMultiTexImage2DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexImage3DEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureTarget target, Int32 level, System.Graphics.OGL.ExtDirectStateAccess internalformat, Int32 width, Int32 height, Int32 depth, Int32 border, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, IntPtr pixels);
			[CLSCompliant(false)]
			public static MultiTexImage3DEXT glMultiTexImage3DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexParameterfEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureTarget target, System.Graphics.OGL.TextureParameterName pname, Single param);
			[CLSCompliant(false)]
			public static MultiTexParameterfEXT glMultiTexParameterfEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexParameterfvEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureTarget target, System.Graphics.OGL.TextureParameterName pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static MultiTexParameterfvEXT glMultiTexParameterfvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexParameteriEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureTarget target, System.Graphics.OGL.TextureParameterName pname, Int32 param);
			[CLSCompliant(false)]
			public static MultiTexParameteriEXT glMultiTexParameteriEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexParameterIivEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureTarget target, System.Graphics.OGL.TextureParameterName pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static MultiTexParameterIivEXT glMultiTexParameterIivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexParameterIuivEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureTarget target, System.Graphics.OGL.TextureParameterName pname, UInt32* @params);
			[CLSCompliant(false)]
			public unsafe static MultiTexParameterIuivEXT glMultiTexParameterIuivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultiTexParameterivEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureTarget target, System.Graphics.OGL.TextureParameterName pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static MultiTexParameterivEXT glMultiTexParameterivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexRenderbufferEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureTarget target, UInt32 renderbuffer);
			[CLSCompliant(false)]
			public static MultiTexRenderbufferEXT glMultiTexRenderbufferEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexSubImage1DEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureTarget target, Int32 level, Int32 xoffset, Int32 width, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, IntPtr pixels);
			[CLSCompliant(false)]
			public static MultiTexSubImage1DEXT glMultiTexSubImage1DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexSubImage2DEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureTarget target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 width, Int32 height, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, IntPtr pixels);
			[CLSCompliant(false)]
			public static MultiTexSubImage2DEXT glMultiTexSubImage2DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void MultiTexSubImage3DEXT(System.Graphics.OGL.TextureUnit texunit, System.Graphics.OGL.TextureTarget target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 width, Int32 height, Int32 depth, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, IntPtr pixels);
			[CLSCompliant(false)]
			public static MultiTexSubImage3DEXT glMultiTexSubImage3DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultMatrixd(Double* m);
			[CLSCompliant(false)]
			public unsafe static MultMatrixd glMultMatrixd;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultMatrixf(Single* m);
			[CLSCompliant(false)]
			public unsafe static MultMatrixf glMultMatrixf;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultTransposeMatrixd(Double* m);
			[CLSCompliant(false)]
			public unsafe static MultTransposeMatrixd glMultTransposeMatrixd;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultTransposeMatrixdARB(Double* m);
			[CLSCompliant(false)]
			public unsafe static MultTransposeMatrixdARB glMultTransposeMatrixdARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultTransposeMatrixf(Single* m);
			[CLSCompliant(false)]
			public unsafe static MultTransposeMatrixf glMultTransposeMatrixf;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void MultTransposeMatrixfARB(Single* m);
			[CLSCompliant(false)]
			public unsafe static MultTransposeMatrixfARB glMultTransposeMatrixfARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void NamedBufferDataEXT(UInt32 buffer, IntPtr size, IntPtr data, System.Graphics.OGL.ExtDirectStateAccess usage);
			[CLSCompliant(false)]
			public static NamedBufferDataEXT glNamedBufferDataEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void NamedBufferSubDataEXT(UInt32 buffer, IntPtr offset, IntPtr size, IntPtr data);
			[CLSCompliant(false)]
			public static NamedBufferSubDataEXT glNamedBufferSubDataEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void NamedCopyBufferSubDataEXT(UInt32 readBuffer, UInt32 writeBuffer, IntPtr readOffset, IntPtr writeOffset, IntPtr size);
			[CLSCompliant(false)]
			public static NamedCopyBufferSubDataEXT glNamedCopyBufferSubDataEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void NamedFramebufferRenderbufferEXT(UInt32 framebuffer, System.Graphics.OGL.FramebufferAttachment attachment, System.Graphics.OGL.RenderbufferTarget renderbuffertarget, UInt32 renderbuffer);
			[CLSCompliant(false)]
			public static NamedFramebufferRenderbufferEXT glNamedFramebufferRenderbufferEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void NamedFramebufferTexture1DEXT(UInt32 framebuffer, System.Graphics.OGL.FramebufferAttachment attachment, System.Graphics.OGL.TextureTarget textarget, UInt32 texture, Int32 level);
			[CLSCompliant(false)]
			public static NamedFramebufferTexture1DEXT glNamedFramebufferTexture1DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void NamedFramebufferTexture2DEXT(UInt32 framebuffer, System.Graphics.OGL.FramebufferAttachment attachment, System.Graphics.OGL.TextureTarget textarget, UInt32 texture, Int32 level);
			[CLSCompliant(false)]
			public static NamedFramebufferTexture2DEXT glNamedFramebufferTexture2DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void NamedFramebufferTexture3DEXT(UInt32 framebuffer, System.Graphics.OGL.FramebufferAttachment attachment, System.Graphics.OGL.TextureTarget textarget, UInt32 texture, Int32 level, Int32 zoffset);
			[CLSCompliant(false)]
			public static NamedFramebufferTexture3DEXT glNamedFramebufferTexture3DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void NamedFramebufferTextureEXT(UInt32 framebuffer, System.Graphics.OGL.FramebufferAttachment attachment, UInt32 texture, Int32 level);
			[CLSCompliant(false)]
			public static NamedFramebufferTextureEXT glNamedFramebufferTextureEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void NamedFramebufferTextureFaceEXT(UInt32 framebuffer, System.Graphics.OGL.FramebufferAttachment attachment, UInt32 texture, Int32 level, System.Graphics.OGL.TextureTarget face);
			[CLSCompliant(false)]
			public static NamedFramebufferTextureFaceEXT glNamedFramebufferTextureFaceEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void NamedFramebufferTextureLayerEXT(UInt32 framebuffer, System.Graphics.OGL.FramebufferAttachment attachment, UInt32 texture, Int32 level, Int32 layer);
			[CLSCompliant(false)]
			public static NamedFramebufferTextureLayerEXT glNamedFramebufferTextureLayerEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void NamedProgramLocalParameter4dEXT(UInt32 program, System.Graphics.OGL.ExtDirectStateAccess target, UInt32 index, Double x, Double y, Double z, Double w);
			[CLSCompliant(false)]
			public static NamedProgramLocalParameter4dEXT glNamedProgramLocalParameter4dEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void NamedProgramLocalParameter4dvEXT(UInt32 program, System.Graphics.OGL.ExtDirectStateAccess target, UInt32 index, Double* @params);
			[CLSCompliant(false)]
			public unsafe static NamedProgramLocalParameter4dvEXT glNamedProgramLocalParameter4dvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void NamedProgramLocalParameter4fEXT(UInt32 program, System.Graphics.OGL.ExtDirectStateAccess target, UInt32 index, Single x, Single y, Single z, Single w);
			[CLSCompliant(false)]
			public static NamedProgramLocalParameter4fEXT glNamedProgramLocalParameter4fEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void NamedProgramLocalParameter4fvEXT(UInt32 program, System.Graphics.OGL.ExtDirectStateAccess target, UInt32 index, Single* @params);
			[CLSCompliant(false)]
			public unsafe static NamedProgramLocalParameter4fvEXT glNamedProgramLocalParameter4fvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void NamedProgramLocalParameterI4iEXT(UInt32 program, System.Graphics.OGL.ExtDirectStateAccess target, UInt32 index, Int32 x, Int32 y, Int32 z, Int32 w);
			[CLSCompliant(false)]
			public static NamedProgramLocalParameterI4iEXT glNamedProgramLocalParameterI4iEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void NamedProgramLocalParameterI4ivEXT(UInt32 program, System.Graphics.OGL.ExtDirectStateAccess target, UInt32 index, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static NamedProgramLocalParameterI4ivEXT glNamedProgramLocalParameterI4ivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void NamedProgramLocalParameterI4uiEXT(UInt32 program, System.Graphics.OGL.ExtDirectStateAccess target, UInt32 index, UInt32 x, UInt32 y, UInt32 z, UInt32 w);
			[CLSCompliant(false)]
			public static NamedProgramLocalParameterI4uiEXT glNamedProgramLocalParameterI4uiEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void NamedProgramLocalParameterI4uivEXT(UInt32 program, System.Graphics.OGL.ExtDirectStateAccess target, UInt32 index, UInt32* @params);
			[CLSCompliant(false)]
			public unsafe static NamedProgramLocalParameterI4uivEXT glNamedProgramLocalParameterI4uivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void NamedProgramLocalParameters4fvEXT(UInt32 program, System.Graphics.OGL.ExtDirectStateAccess target, UInt32 index, Int32 count, Single* @params);
			[CLSCompliant(false)]
			public unsafe static NamedProgramLocalParameters4fvEXT glNamedProgramLocalParameters4fvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void NamedProgramLocalParametersI4ivEXT(UInt32 program, System.Graphics.OGL.ExtDirectStateAccess target, UInt32 index, Int32 count, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static NamedProgramLocalParametersI4ivEXT glNamedProgramLocalParametersI4ivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void NamedProgramLocalParametersI4uivEXT(UInt32 program, System.Graphics.OGL.ExtDirectStateAccess target, UInt32 index, Int32 count, UInt32* @params);
			[CLSCompliant(false)]
			public unsafe static NamedProgramLocalParametersI4uivEXT glNamedProgramLocalParametersI4uivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void NamedProgramStringEXT(UInt32 program, System.Graphics.OGL.ExtDirectStateAccess target, System.Graphics.OGL.ExtDirectStateAccess format, Int32 len, IntPtr @string);
			[CLSCompliant(false)]
			public static NamedProgramStringEXT glNamedProgramStringEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void NamedRenderbufferStorageEXT(UInt32 renderbuffer, System.Graphics.OGL.PixelInternalFormat internalformat, Int32 width, Int32 height);
			[CLSCompliant(false)]
			public static NamedRenderbufferStorageEXT glNamedRenderbufferStorageEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void NamedRenderbufferStorageMultisampleCoverageEXT(UInt32 renderbuffer, Int32 coverageSamples, Int32 colorSamples, System.Graphics.OGL.PixelInternalFormat internalformat, Int32 width, Int32 height);
			[CLSCompliant(false)]
			public static NamedRenderbufferStorageMultisampleCoverageEXT glNamedRenderbufferStorageMultisampleCoverageEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void NamedRenderbufferStorageMultisampleEXT(UInt32 renderbuffer, Int32 samples, System.Graphics.OGL.PixelInternalFormat internalformat, Int32 width, Int32 height);
			[CLSCompliant(false)]
			public static NamedRenderbufferStorageMultisampleEXT glNamedRenderbufferStorageMultisampleEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void NamedStringARB(System.Graphics.OGL.ArbShadingLanguageInclude type, Int32 namelen, String name, Int32 stringlen, String @string);
			[CLSCompliant(false)]
			public static NamedStringARB glNamedStringARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void NewList(UInt32 list, System.Graphics.OGL.ListMode mode);
			[CLSCompliant(false)]
			public static NewList glNewList;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Int32 NewObjectBufferATI(Int32 size, IntPtr pointer, System.Graphics.OGL.AtiVertexArrayObject usage);
			[CLSCompliant(false)]
			public static NewObjectBufferATI glNewObjectBufferATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Normal3b(SByte nx, SByte ny, SByte nz);
			[CLSCompliant(false)]
			public static Normal3b glNormal3b;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Normal3bv(SByte* v);
			[CLSCompliant(false)]
			public unsafe static Normal3bv glNormal3bv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Normal3d(Double nx, Double ny, Double nz);
			[CLSCompliant(false)]
			public static Normal3d glNormal3d;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Normal3dv(Double* v);
			[CLSCompliant(false)]
			public unsafe static Normal3dv glNormal3dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Normal3f(Single nx, Single ny, Single nz);
			[CLSCompliant(false)]
			public static Normal3f glNormal3f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Normal3fv(Single* v);
			[CLSCompliant(false)]
			public unsafe static Normal3fv glNormal3fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Normal3fVertex3fSUN(Single nx, Single ny, Single nz, Single x, Single y, Single z);
			[CLSCompliant(false)]
			public static Normal3fVertex3fSUN glNormal3fVertex3fSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Normal3fVertex3fvSUN(Single* n, Single* v);
			[CLSCompliant(false)]
			public unsafe static Normal3fVertex3fvSUN glNormal3fVertex3fvSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Normal3hNV(Half nx, Half ny, Half nz);
			[CLSCompliant(false)]
			public static Normal3hNV glNormal3hNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Normal3hvNV(Half* v);
			[CLSCompliant(false)]
			public unsafe static Normal3hvNV glNormal3hvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Normal3i(Int32 nx, Int32 ny, Int32 nz);
			[CLSCompliant(false)]
			public static Normal3i glNormal3i;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Normal3iv(Int32* v);
			[CLSCompliant(false)]
			public unsafe static Normal3iv glNormal3iv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Normal3s(Int16 nx, Int16 ny, Int16 nz);
			[CLSCompliant(false)]
			public static Normal3s glNormal3s;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Normal3sv(Int16* v);
			[CLSCompliant(false)]
			public unsafe static Normal3sv glNormal3sv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void NormalFormatNV(System.Graphics.OGL.NvVertexBufferUnifiedMemory type, Int32 stride);
			[CLSCompliant(false)]
			public static NormalFormatNV glNormalFormatNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void NormalP3ui(System.Graphics.OGL.PackedPointerType type, UInt32 coords);
			[CLSCompliant(false)]
			public static NormalP3ui glNormalP3ui;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void NormalP3uiv(System.Graphics.OGL.PackedPointerType type, UInt32* coords);
			[CLSCompliant(false)]
			public unsafe static NormalP3uiv glNormalP3uiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void NormalPointer(System.Graphics.OGL.NormalPointerType type, Int32 stride, IntPtr pointer);
			[CLSCompliant(false)]
			public static NormalPointer glNormalPointer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void NormalPointerEXT(System.Graphics.OGL.NormalPointerType type, Int32 stride, Int32 count, IntPtr pointer);
			[CLSCompliant(false)]
			public static NormalPointerEXT glNormalPointerEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void NormalPointerListIBM(System.Graphics.OGL.NormalPointerType type, Int32 stride, IntPtr pointer, Int32 ptrstride);
			[CLSCompliant(false)]
			public static NormalPointerListIBM glNormalPointerListIBM;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void NormalPointervINTEL(System.Graphics.OGL.NormalPointerType type, IntPtr pointer);
			[CLSCompliant(false)]
			public static NormalPointervINTEL glNormalPointervINTEL;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void NormalStream3bATI(System.Graphics.OGL.AtiVertexStreams stream, SByte nx, SByte ny, SByte nz);
			[CLSCompliant(false)]
			public static NormalStream3bATI glNormalStream3bATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void NormalStream3bvATI(System.Graphics.OGL.AtiVertexStreams stream, SByte* coords);
			[CLSCompliant(false)]
			public unsafe static NormalStream3bvATI glNormalStream3bvATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void NormalStream3dATI(System.Graphics.OGL.AtiVertexStreams stream, Double nx, Double ny, Double nz);
			[CLSCompliant(false)]
			public static NormalStream3dATI glNormalStream3dATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void NormalStream3dvATI(System.Graphics.OGL.AtiVertexStreams stream, Double* coords);
			[CLSCompliant(false)]
			public unsafe static NormalStream3dvATI glNormalStream3dvATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void NormalStream3fATI(System.Graphics.OGL.AtiVertexStreams stream, Single nx, Single ny, Single nz);
			[CLSCompliant(false)]
			public static NormalStream3fATI glNormalStream3fATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void NormalStream3fvATI(System.Graphics.OGL.AtiVertexStreams stream, Single* coords);
			[CLSCompliant(false)]
			public unsafe static NormalStream3fvATI glNormalStream3fvATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void NormalStream3iATI(System.Graphics.OGL.AtiVertexStreams stream, Int32 nx, Int32 ny, Int32 nz);
			[CLSCompliant(false)]
			public static NormalStream3iATI glNormalStream3iATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void NormalStream3ivATI(System.Graphics.OGL.AtiVertexStreams stream, Int32* coords);
			[CLSCompliant(false)]
			public unsafe static NormalStream3ivATI glNormalStream3ivATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void NormalStream3sATI(System.Graphics.OGL.AtiVertexStreams stream, Int16 nx, Int16 ny, Int16 nz);
			[CLSCompliant(false)]
			public static NormalStream3sATI glNormalStream3sATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void NormalStream3svATI(System.Graphics.OGL.AtiVertexStreams stream, Int16* coords);
			[CLSCompliant(false)]
			public unsafe static NormalStream3svATI glNormalStream3svATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate System.Graphics.OGL.AppleObjectPurgeable ObjectPurgeableAPPLE(System.Graphics.OGL.AppleObjectPurgeable objectType, UInt32 name, System.Graphics.OGL.AppleObjectPurgeable option);
			[CLSCompliant(false)]
			public static ObjectPurgeableAPPLE glObjectPurgeableAPPLE;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate System.Graphics.OGL.AppleObjectPurgeable ObjectUnpurgeableAPPLE(System.Graphics.OGL.AppleObjectPurgeable objectType, UInt32 name, System.Graphics.OGL.AppleObjectPurgeable option);
			[CLSCompliant(false)]
			public static ObjectUnpurgeableAPPLE glObjectUnpurgeableAPPLE;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Ortho(Double left, Double right, Double bottom, Double top, Double zNear, Double zFar);
			[CLSCompliant(false)]
			public static Ortho glOrtho;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void PassTexCoordATI(UInt32 dst, UInt32 coord, System.Graphics.OGL.AtiFragmentShader swizzle);
			[CLSCompliant(false)]
			public static PassTexCoordATI glPassTexCoordATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void PassThrough(Single token);
			[CLSCompliant(false)]
			public static PassThrough glPassThrough;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void PatchParameterfv(System.Graphics.OGL.PatchParameterFloat pname, Single* values);
			[CLSCompliant(false)]
			public unsafe static PatchParameterfv glPatchParameterfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void PatchParameteri(System.Graphics.OGL.PatchParameterInt pname, Int32 value);
			[CLSCompliant(false)]
			public static PatchParameteri glPatchParameteri;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void PauseTransformFeedback();
			[CLSCompliant(false)]
			public static PauseTransformFeedback glPauseTransformFeedback;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void PauseTransformFeedbackNV();
			[CLSCompliant(false)]
			public static PauseTransformFeedbackNV glPauseTransformFeedbackNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void PixelDataRangeNV(System.Graphics.OGL.NvPixelDataRange target, Int32 length, [OutAttribute] IntPtr pointer);
			[CLSCompliant(false)]
			public static PixelDataRangeNV glPixelDataRangeNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void PixelMapfv(System.Graphics.OGL.PixelMap map, Int32 mapsize, Single* values);
			[CLSCompliant(false)]
			public unsafe static PixelMapfv glPixelMapfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void PixelMapuiv(System.Graphics.OGL.PixelMap map, Int32 mapsize, UInt32* values);
			[CLSCompliant(false)]
			public unsafe static PixelMapuiv glPixelMapuiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void PixelMapusv(System.Graphics.OGL.PixelMap map, Int32 mapsize, UInt16* values);
			[CLSCompliant(false)]
			public unsafe static PixelMapusv glPixelMapusv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void PixelStoref(System.Graphics.OGL.PixelStoreParameter pname, Single param);
			[CLSCompliant(false)]
			public static PixelStoref glPixelStoref;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void PixelStorei(System.Graphics.OGL.PixelStoreParameter pname, Int32 param);
			[CLSCompliant(false)]
			public static PixelStorei glPixelStorei;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void PixelTexGenParameterfSGIS(System.Graphics.OGL.SgisPixelTexture pname, Single param);
			[CLSCompliant(false)]
			public static PixelTexGenParameterfSGIS glPixelTexGenParameterfSGIS;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void PixelTexGenParameterfvSGIS(System.Graphics.OGL.SgisPixelTexture pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static PixelTexGenParameterfvSGIS glPixelTexGenParameterfvSGIS;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void PixelTexGenParameteriSGIS(System.Graphics.OGL.SgisPixelTexture pname, Int32 param);
			[CLSCompliant(false)]
			public static PixelTexGenParameteriSGIS glPixelTexGenParameteriSGIS;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void PixelTexGenParameterivSGIS(System.Graphics.OGL.SgisPixelTexture pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static PixelTexGenParameterivSGIS glPixelTexGenParameterivSGIS;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void PixelTexGenSGIX(System.Graphics.OGL.SgixPixelTexture mode);
			[CLSCompliant(false)]
			public static PixelTexGenSGIX glPixelTexGenSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void PixelTransferf(System.Graphics.OGL.PixelTransferParameter pname, Single param);
			[CLSCompliant(false)]
			public static PixelTransferf glPixelTransferf;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void PixelTransferi(System.Graphics.OGL.PixelTransferParameter pname, Int32 param);
			[CLSCompliant(false)]
			public static PixelTransferi glPixelTransferi;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void PixelTransformParameterfEXT(System.Graphics.OGL.ExtPixelTransform target, System.Graphics.OGL.ExtPixelTransform pname, Single param);
			[CLSCompliant(false)]
			public static PixelTransformParameterfEXT glPixelTransformParameterfEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void PixelTransformParameterfvEXT(System.Graphics.OGL.ExtPixelTransform target, System.Graphics.OGL.ExtPixelTransform pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static PixelTransformParameterfvEXT glPixelTransformParameterfvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void PixelTransformParameteriEXT(System.Graphics.OGL.ExtPixelTransform target, System.Graphics.OGL.ExtPixelTransform pname, Int32 param);
			[CLSCompliant(false)]
			public static PixelTransformParameteriEXT glPixelTransformParameteriEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void PixelTransformParameterivEXT(System.Graphics.OGL.ExtPixelTransform target, System.Graphics.OGL.ExtPixelTransform pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static PixelTransformParameterivEXT glPixelTransformParameterivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void PixelZoom(Single xfactor, Single yfactor);
			[CLSCompliant(false)]
			public static PixelZoom glPixelZoom;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void PNTrianglesfATI(System.Graphics.OGL.AtiPnTriangles pname, Single param);
			[CLSCompliant(false)]
			public static PNTrianglesfATI glPNTrianglesfATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void PNTrianglesiATI(System.Graphics.OGL.AtiPnTriangles pname, Int32 param);
			[CLSCompliant(false)]
			public static PNTrianglesiATI glPNTrianglesiATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void PointParameterf(System.Graphics.OGL.PointParameterName pname, Single param);
			[CLSCompliant(false)]
			public static PointParameterf glPointParameterf;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void PointParameterfARB(System.Graphics.OGL.ArbPointParameters pname, Single param);
			[CLSCompliant(false)]
			public static PointParameterfARB glPointParameterfARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void PointParameterfEXT(System.Graphics.OGL.ExtPointParameters pname, Single param);
			[CLSCompliant(false)]
			public static PointParameterfEXT glPointParameterfEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void PointParameterfSGIS(System.Graphics.OGL.SgisPointParameters pname, Single param);
			[CLSCompliant(false)]
			public static PointParameterfSGIS glPointParameterfSGIS;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void PointParameterfv(System.Graphics.OGL.PointParameterName pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static PointParameterfv glPointParameterfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void PointParameterfvARB(System.Graphics.OGL.ArbPointParameters pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static PointParameterfvARB glPointParameterfvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void PointParameterfvEXT(System.Graphics.OGL.ExtPointParameters pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static PointParameterfvEXT glPointParameterfvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void PointParameterfvSGIS(System.Graphics.OGL.SgisPointParameters pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static PointParameterfvSGIS glPointParameterfvSGIS;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void PointParameteri(System.Graphics.OGL.PointParameterName pname, Int32 param);
			[CLSCompliant(false)]
			public static PointParameteri glPointParameteri;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void PointParameteriNV(System.Graphics.OGL.NvPointSprite pname, Int32 param);
			[CLSCompliant(false)]
			public static PointParameteriNV glPointParameteriNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void PointParameteriv(System.Graphics.OGL.PointParameterName pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static PointParameteriv glPointParameteriv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void PointParameterivNV(System.Graphics.OGL.NvPointSprite pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static PointParameterivNV glPointParameterivNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void PointSize(Single size);
			[CLSCompliant(false)]
			public static PointSize glPointSize;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate Int32 PollAsyncSGIX([OutAttribute] UInt32* markerp);
			[CLSCompliant(false)]
			public unsafe static PollAsyncSGIX glPollAsyncSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate Int32 PollInstrumentsSGIX([OutAttribute] Int32* marker_p);
			[CLSCompliant(false)]
			public unsafe static PollInstrumentsSGIX glPollInstrumentsSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void PolygonMode(System.Graphics.OGL.MaterialFace face, System.Graphics.OGL.PolygonMode mode);
			[CLSCompliant(false)]
			public static PolygonMode glPolygonMode;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void PolygonOffset(Single factor, Single units);
			[CLSCompliant(false)]
			public static PolygonOffset glPolygonOffset;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void PolygonOffsetEXT(Single factor, Single bias);
			[CLSCompliant(false)]
			public static PolygonOffsetEXT glPolygonOffsetEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void PolygonStipple(Byte* mask);
			[CLSCompliant(false)]
			public unsafe static PolygonStipple glPolygonStipple;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void PopAttrib();
			[CLSCompliant(false)]
			public static PopAttrib glPopAttrib;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void PopClientAttrib();
			[CLSCompliant(false)]
			public static PopClientAttrib glPopClientAttrib;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void PopMatrix();
			[CLSCompliant(false)]
			public static PopMatrix glPopMatrix;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void PopName();
			[CLSCompliant(false)]
			public static PopName glPopName;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void PresentFrameDualFillNV(UInt32 video_slot, UInt64 minPresentTime, UInt32 beginPresentTimeId, UInt32 presentDurationId, System.Graphics.OGL.NvPresentVideo type, System.Graphics.OGL.NvPresentVideo target0, UInt32 fill0, System.Graphics.OGL.NvPresentVideo target1, UInt32 fill1, System.Graphics.OGL.NvPresentVideo target2, UInt32 fill2, System.Graphics.OGL.NvPresentVideo target3, UInt32 fill3);
			[CLSCompliant(false)]
			public static PresentFrameDualFillNV glPresentFrameDualFillNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void PresentFrameKeyedNV(UInt32 video_slot, UInt64 minPresentTime, UInt32 beginPresentTimeId, UInt32 presentDurationId, System.Graphics.OGL.NvPresentVideo type, System.Graphics.OGL.NvPresentVideo target0, UInt32 fill0, UInt32 key0, System.Graphics.OGL.NvPresentVideo target1, UInt32 fill1, UInt32 key1);
			[CLSCompliant(false)]
			public static PresentFrameKeyedNV glPresentFrameKeyedNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void PrimitiveRestartIndex(UInt32 index);
			[CLSCompliant(false)]
			public static PrimitiveRestartIndex glPrimitiveRestartIndex;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void PrimitiveRestartIndexNV(UInt32 index);
			[CLSCompliant(false)]
			public static PrimitiveRestartIndexNV glPrimitiveRestartIndexNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void PrimitiveRestartNV();
			[CLSCompliant(false)]
			public static PrimitiveRestartNV glPrimitiveRestartNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void PrioritizeTextures(Int32 n, UInt32* textures, Single* priorities);
			[CLSCompliant(false)]
			public unsafe static PrioritizeTextures glPrioritizeTextures;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void PrioritizeTexturesEXT(Int32 n, UInt32* textures, Single* priorities);
			[CLSCompliant(false)]
			public unsafe static PrioritizeTexturesEXT glPrioritizeTexturesEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramBinary(UInt32 program, System.Graphics.OGL.BinaryFormat binaryFormat, IntPtr binary, Int32 length);
			[CLSCompliant(false)]
			public static ProgramBinary glProgramBinary;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramBufferParametersfvNV(System.Graphics.OGL.NvParameterBufferObject target, UInt32 buffer, UInt32 index, Int32 count, Single* @params);
			[CLSCompliant(false)]
			public unsafe static ProgramBufferParametersfvNV glProgramBufferParametersfvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramBufferParametersIivNV(System.Graphics.OGL.NvParameterBufferObject target, UInt32 buffer, UInt32 index, Int32 count, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static ProgramBufferParametersIivNV glProgramBufferParametersIivNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramBufferParametersIuivNV(System.Graphics.OGL.NvParameterBufferObject target, UInt32 buffer, UInt32 index, Int32 count, UInt32* @params);
			[CLSCompliant(false)]
			public unsafe static ProgramBufferParametersIuivNV glProgramBufferParametersIuivNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramEnvParameter4dARB(System.Graphics.OGL.AssemblyProgramTargetArb target, UInt32 index, Double x, Double y, Double z, Double w);
			[CLSCompliant(false)]
			public static ProgramEnvParameter4dARB glProgramEnvParameter4dARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramEnvParameter4dvARB(System.Graphics.OGL.AssemblyProgramTargetArb target, UInt32 index, Double* @params);
			[CLSCompliant(false)]
			public unsafe static ProgramEnvParameter4dvARB glProgramEnvParameter4dvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramEnvParameter4fARB(System.Graphics.OGL.AssemblyProgramTargetArb target, UInt32 index, Single x, Single y, Single z, Single w);
			[CLSCompliant(false)]
			public static ProgramEnvParameter4fARB glProgramEnvParameter4fARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramEnvParameter4fvARB(System.Graphics.OGL.AssemblyProgramTargetArb target, UInt32 index, Single* @params);
			[CLSCompliant(false)]
			public unsafe static ProgramEnvParameter4fvARB glProgramEnvParameter4fvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramEnvParameterI4iNV(System.Graphics.OGL.NvGpuProgram4 target, UInt32 index, Int32 x, Int32 y, Int32 z, Int32 w);
			[CLSCompliant(false)]
			public static ProgramEnvParameterI4iNV glProgramEnvParameterI4iNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramEnvParameterI4ivNV(System.Graphics.OGL.NvGpuProgram4 target, UInt32 index, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static ProgramEnvParameterI4ivNV glProgramEnvParameterI4ivNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramEnvParameterI4uiNV(System.Graphics.OGL.NvGpuProgram4 target, UInt32 index, UInt32 x, UInt32 y, UInt32 z, UInt32 w);
			[CLSCompliant(false)]
			public static ProgramEnvParameterI4uiNV glProgramEnvParameterI4uiNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramEnvParameterI4uivNV(System.Graphics.OGL.NvGpuProgram4 target, UInt32 index, UInt32* @params);
			[CLSCompliant(false)]
			public unsafe static ProgramEnvParameterI4uivNV glProgramEnvParameterI4uivNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramEnvParameters4fvEXT(System.Graphics.OGL.ExtGpuProgramParameters target, UInt32 index, Int32 count, Single* @params);
			[CLSCompliant(false)]
			public unsafe static ProgramEnvParameters4fvEXT glProgramEnvParameters4fvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramEnvParametersI4ivNV(System.Graphics.OGL.NvGpuProgram4 target, UInt32 index, Int32 count, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static ProgramEnvParametersI4ivNV glProgramEnvParametersI4ivNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramEnvParametersI4uivNV(System.Graphics.OGL.NvGpuProgram4 target, UInt32 index, Int32 count, UInt32* @params);
			[CLSCompliant(false)]
			public unsafe static ProgramEnvParametersI4uivNV glProgramEnvParametersI4uivNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramLocalParameter4dARB(System.Graphics.OGL.AssemblyProgramTargetArb target, UInt32 index, Double x, Double y, Double z, Double w);
			[CLSCompliant(false)]
			public static ProgramLocalParameter4dARB glProgramLocalParameter4dARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramLocalParameter4dvARB(System.Graphics.OGL.AssemblyProgramTargetArb target, UInt32 index, Double* @params);
			[CLSCompliant(false)]
			public unsafe static ProgramLocalParameter4dvARB glProgramLocalParameter4dvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramLocalParameter4fARB(System.Graphics.OGL.AssemblyProgramTargetArb target, UInt32 index, Single x, Single y, Single z, Single w);
			[CLSCompliant(false)]
			public static ProgramLocalParameter4fARB glProgramLocalParameter4fARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramLocalParameter4fvARB(System.Graphics.OGL.AssemblyProgramTargetArb target, UInt32 index, Single* @params);
			[CLSCompliant(false)]
			public unsafe static ProgramLocalParameter4fvARB glProgramLocalParameter4fvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramLocalParameterI4iNV(System.Graphics.OGL.NvGpuProgram4 target, UInt32 index, Int32 x, Int32 y, Int32 z, Int32 w);
			[CLSCompliant(false)]
			public static ProgramLocalParameterI4iNV glProgramLocalParameterI4iNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramLocalParameterI4ivNV(System.Graphics.OGL.NvGpuProgram4 target, UInt32 index, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static ProgramLocalParameterI4ivNV glProgramLocalParameterI4ivNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramLocalParameterI4uiNV(System.Graphics.OGL.NvGpuProgram4 target, UInt32 index, UInt32 x, UInt32 y, UInt32 z, UInt32 w);
			[CLSCompliant(false)]
			public static ProgramLocalParameterI4uiNV glProgramLocalParameterI4uiNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramLocalParameterI4uivNV(System.Graphics.OGL.NvGpuProgram4 target, UInt32 index, UInt32* @params);
			[CLSCompliant(false)]
			public unsafe static ProgramLocalParameterI4uivNV glProgramLocalParameterI4uivNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramLocalParameters4fvEXT(System.Graphics.OGL.ExtGpuProgramParameters target, UInt32 index, Int32 count, Single* @params);
			[CLSCompliant(false)]
			public unsafe static ProgramLocalParameters4fvEXT glProgramLocalParameters4fvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramLocalParametersI4ivNV(System.Graphics.OGL.NvGpuProgram4 target, UInt32 index, Int32 count, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static ProgramLocalParametersI4ivNV glProgramLocalParametersI4ivNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramLocalParametersI4uivNV(System.Graphics.OGL.NvGpuProgram4 target, UInt32 index, Int32 count, UInt32* @params);
			[CLSCompliant(false)]
			public unsafe static ProgramLocalParametersI4uivNV glProgramLocalParametersI4uivNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramNamedParameter4dNV(UInt32 id, Int32 len, Byte* name, Double x, Double y, Double z, Double w);
			[CLSCompliant(false)]
			public unsafe static ProgramNamedParameter4dNV glProgramNamedParameter4dNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramNamedParameter4dvNV(UInt32 id, Int32 len, Byte* name, Double* v);
			[CLSCompliant(false)]
			public unsafe static ProgramNamedParameter4dvNV glProgramNamedParameter4dvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramNamedParameter4fNV(UInt32 id, Int32 len, Byte* name, Single x, Single y, Single z, Single w);
			[CLSCompliant(false)]
			public unsafe static ProgramNamedParameter4fNV glProgramNamedParameter4fNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramNamedParameter4fvNV(UInt32 id, Int32 len, Byte* name, Single* v);
			[CLSCompliant(false)]
			public unsafe static ProgramNamedParameter4fvNV glProgramNamedParameter4fvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramParameter4dNV(System.Graphics.OGL.AssemblyProgramTargetArb target, UInt32 index, Double x, Double y, Double z, Double w);
			[CLSCompliant(false)]
			public static ProgramParameter4dNV glProgramParameter4dNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramParameter4dvNV(System.Graphics.OGL.AssemblyProgramTargetArb target, UInt32 index, Double* v);
			[CLSCompliant(false)]
			public unsafe static ProgramParameter4dvNV glProgramParameter4dvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramParameter4fNV(System.Graphics.OGL.AssemblyProgramTargetArb target, UInt32 index, Single x, Single y, Single z, Single w);
			[CLSCompliant(false)]
			public static ProgramParameter4fNV glProgramParameter4fNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramParameter4fvNV(System.Graphics.OGL.AssemblyProgramTargetArb target, UInt32 index, Single* v);
			[CLSCompliant(false)]
			public unsafe static ProgramParameter4fvNV glProgramParameter4fvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramParameteri(UInt32 program, System.Graphics.OGL.AssemblyProgramParameterArb pname, Int32 value);
			[CLSCompliant(false)]
			public static ProgramParameteri glProgramParameteri;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramParameteriARB(UInt32 program, System.Graphics.OGL.AssemblyProgramParameterArb pname, Int32 value);
			[CLSCompliant(false)]
			public static ProgramParameteriARB glProgramParameteriARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramParameteriEXT(UInt32 program, System.Graphics.OGL.AssemblyProgramParameterArb pname, Int32 value);
			[CLSCompliant(false)]
			public static ProgramParameteriEXT glProgramParameteriEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramParameters4dvNV(System.Graphics.OGL.AssemblyProgramTargetArb target, UInt32 index, Int32 count, Double* v);
			[CLSCompliant(false)]
			public unsafe static ProgramParameters4dvNV glProgramParameters4dvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramParameters4fvNV(System.Graphics.OGL.AssemblyProgramTargetArb target, UInt32 index, Int32 count, Single* v);
			[CLSCompliant(false)]
			public unsafe static ProgramParameters4fvNV glProgramParameters4fvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramStringARB(System.Graphics.OGL.AssemblyProgramTargetArb target, System.Graphics.OGL.ArbVertexProgram format, Int32 len, IntPtr @string);
			[CLSCompliant(false)]
			public static ProgramStringARB glProgramStringARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramSubroutineParametersuivNV(System.Graphics.OGL.NvGpuProgram5 target, Int32 count, UInt32* @params);
			[CLSCompliant(false)]
			public unsafe static ProgramSubroutineParametersuivNV glProgramSubroutineParametersuivNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramUniform1d(UInt32 program, Int32 location, Double v0);
			[CLSCompliant(false)]
			public static ProgramUniform1d glProgramUniform1d;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramUniform1dEXT(UInt32 program, Int32 location, Double x);
			[CLSCompliant(false)]
			public static ProgramUniform1dEXT glProgramUniform1dEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniform1dv(UInt32 program, Int32 location, Int32 count, Double* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniform1dv glProgramUniform1dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniform1dvEXT(UInt32 program, Int32 location, Int32 count, Double* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniform1dvEXT glProgramUniform1dvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramUniform1f(UInt32 program, Int32 location, Single v0);
			[CLSCompliant(false)]
			public static ProgramUniform1f glProgramUniform1f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramUniform1fEXT(UInt32 program, Int32 location, Single v0);
			[CLSCompliant(false)]
			public static ProgramUniform1fEXT glProgramUniform1fEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniform1fv(UInt32 program, Int32 location, Int32 count, Single* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniform1fv glProgramUniform1fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniform1fvEXT(UInt32 program, Int32 location, Int32 count, Single* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniform1fvEXT glProgramUniform1fvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramUniform1i(UInt32 program, Int32 location, Int32 v0);
			[CLSCompliant(false)]
			public static ProgramUniform1i glProgramUniform1i;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramUniform1i64NV(UInt32 program, Int32 location, Int64 x);
			[CLSCompliant(false)]
			public static ProgramUniform1i64NV glProgramUniform1i64NV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniform1i64vNV(UInt32 program, Int32 location, Int32 count, Int64* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniform1i64vNV glProgramUniform1i64vNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramUniform1iEXT(UInt32 program, Int32 location, Int32 v0);
			[CLSCompliant(false)]
			public static ProgramUniform1iEXT glProgramUniform1iEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniform1iv(UInt32 program, Int32 location, Int32 count, Int32* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniform1iv glProgramUniform1iv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniform1ivEXT(UInt32 program, Int32 location, Int32 count, Int32* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniform1ivEXT glProgramUniform1ivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramUniform1ui(UInt32 program, Int32 location, UInt32 v0);
			[CLSCompliant(false)]
			public static ProgramUniform1ui glProgramUniform1ui;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramUniform1ui64NV(UInt32 program, Int32 location, UInt64 x);
			[CLSCompliant(false)]
			public static ProgramUniform1ui64NV glProgramUniform1ui64NV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniform1ui64vNV(UInt32 program, Int32 location, Int32 count, UInt64* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniform1ui64vNV glProgramUniform1ui64vNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramUniform1uiEXT(UInt32 program, Int32 location, UInt32 v0);
			[CLSCompliant(false)]
			public static ProgramUniform1uiEXT glProgramUniform1uiEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniform1uiv(UInt32 program, Int32 location, Int32 count, UInt32* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniform1uiv glProgramUniform1uiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniform1uivEXT(UInt32 program, Int32 location, Int32 count, UInt32* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniform1uivEXT glProgramUniform1uivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramUniform2d(UInt32 program, Int32 location, Double v0, Double v1);
			[CLSCompliant(false)]
			public static ProgramUniform2d glProgramUniform2d;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramUniform2dEXT(UInt32 program, Int32 location, Double x, Double y);
			[CLSCompliant(false)]
			public static ProgramUniform2dEXT glProgramUniform2dEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniform2dv(UInt32 program, Int32 location, Int32 count, Double* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniform2dv glProgramUniform2dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniform2dvEXT(UInt32 program, Int32 location, Int32 count, Double* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniform2dvEXT glProgramUniform2dvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramUniform2f(UInt32 program, Int32 location, Single v0, Single v1);
			[CLSCompliant(false)]
			public static ProgramUniform2f glProgramUniform2f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramUniform2fEXT(UInt32 program, Int32 location, Single v0, Single v1);
			[CLSCompliant(false)]
			public static ProgramUniform2fEXT glProgramUniform2fEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniform2fv(UInt32 program, Int32 location, Int32 count, Single* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniform2fv glProgramUniform2fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniform2fvEXT(UInt32 program, Int32 location, Int32 count, Single* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniform2fvEXT glProgramUniform2fvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramUniform2i(UInt32 program, Int32 location, Int32 v0, Int32 v1);
			[CLSCompliant(false)]
			public static ProgramUniform2i glProgramUniform2i;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramUniform2i64NV(UInt32 program, Int32 location, Int64 x, Int64 y);
			[CLSCompliant(false)]
			public static ProgramUniform2i64NV glProgramUniform2i64NV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniform2i64vNV(UInt32 program, Int32 location, Int32 count, Int64* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniform2i64vNV glProgramUniform2i64vNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramUniform2iEXT(UInt32 program, Int32 location, Int32 v0, Int32 v1);
			[CLSCompliant(false)]
			public static ProgramUniform2iEXT glProgramUniform2iEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniform2iv(UInt32 program, Int32 location, Int32 count, Int32* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniform2iv glProgramUniform2iv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniform2ivEXT(UInt32 program, Int32 location, Int32 count, Int32* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniform2ivEXT glProgramUniform2ivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramUniform2ui(UInt32 program, Int32 location, UInt32 v0, UInt32 v1);
			[CLSCompliant(false)]
			public static ProgramUniform2ui glProgramUniform2ui;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramUniform2ui64NV(UInt32 program, Int32 location, UInt64 x, UInt64 y);
			[CLSCompliant(false)]
			public static ProgramUniform2ui64NV glProgramUniform2ui64NV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniform2ui64vNV(UInt32 program, Int32 location, Int32 count, UInt64* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniform2ui64vNV glProgramUniform2ui64vNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramUniform2uiEXT(UInt32 program, Int32 location, UInt32 v0, UInt32 v1);
			[CLSCompliant(false)]
			public static ProgramUniform2uiEXT glProgramUniform2uiEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniform2uiv(UInt32 program, Int32 location, Int32 count, UInt32* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniform2uiv glProgramUniform2uiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniform2uivEXT(UInt32 program, Int32 location, Int32 count, UInt32* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniform2uivEXT glProgramUniform2uivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramUniform3d(UInt32 program, Int32 location, Double v0, Double v1, Double v2);
			[CLSCompliant(false)]
			public static ProgramUniform3d glProgramUniform3d;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramUniform3dEXT(UInt32 program, Int32 location, Double x, Double y, Double z);
			[CLSCompliant(false)]
			public static ProgramUniform3dEXT glProgramUniform3dEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniform3dv(UInt32 program, Int32 location, Int32 count, Double* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniform3dv glProgramUniform3dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniform3dvEXT(UInt32 program, Int32 location, Int32 count, Double* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniform3dvEXT glProgramUniform3dvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramUniform3f(UInt32 program, Int32 location, Single v0, Single v1, Single v2);
			[CLSCompliant(false)]
			public static ProgramUniform3f glProgramUniform3f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramUniform3fEXT(UInt32 program, Int32 location, Single v0, Single v1, Single v2);
			[CLSCompliant(false)]
			public static ProgramUniform3fEXT glProgramUniform3fEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniform3fv(UInt32 program, Int32 location, Int32 count, Single* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniform3fv glProgramUniform3fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniform3fvEXT(UInt32 program, Int32 location, Int32 count, Single* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniform3fvEXT glProgramUniform3fvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramUniform3i(UInt32 program, Int32 location, Int32 v0, Int32 v1, Int32 v2);
			[CLSCompliant(false)]
			public static ProgramUniform3i glProgramUniform3i;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramUniform3i64NV(UInt32 program, Int32 location, Int64 x, Int64 y, Int64 z);
			[CLSCompliant(false)]
			public static ProgramUniform3i64NV glProgramUniform3i64NV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniform3i64vNV(UInt32 program, Int32 location, Int32 count, Int64* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniform3i64vNV glProgramUniform3i64vNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramUniform3iEXT(UInt32 program, Int32 location, Int32 v0, Int32 v1, Int32 v2);
			[CLSCompliant(false)]
			public static ProgramUniform3iEXT glProgramUniform3iEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniform3iv(UInt32 program, Int32 location, Int32 count, Int32* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniform3iv glProgramUniform3iv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniform3ivEXT(UInt32 program, Int32 location, Int32 count, Int32* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniform3ivEXT glProgramUniform3ivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramUniform3ui(UInt32 program, Int32 location, UInt32 v0, UInt32 v1, UInt32 v2);
			[CLSCompliant(false)]
			public static ProgramUniform3ui glProgramUniform3ui;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramUniform3ui64NV(UInt32 program, Int32 location, UInt64 x, UInt64 y, UInt64 z);
			[CLSCompliant(false)]
			public static ProgramUniform3ui64NV glProgramUniform3ui64NV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniform3ui64vNV(UInt32 program, Int32 location, Int32 count, UInt64* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniform3ui64vNV glProgramUniform3ui64vNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramUniform3uiEXT(UInt32 program, Int32 location, UInt32 v0, UInt32 v1, UInt32 v2);
			[CLSCompliant(false)]
			public static ProgramUniform3uiEXT glProgramUniform3uiEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniform3uiv(UInt32 program, Int32 location, Int32 count, UInt32* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniform3uiv glProgramUniform3uiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniform3uivEXT(UInt32 program, Int32 location, Int32 count, UInt32* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniform3uivEXT glProgramUniform3uivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramUniform4d(UInt32 program, Int32 location, Double v0, Double v1, Double v2, Double v3);
			[CLSCompliant(false)]
			public static ProgramUniform4d glProgramUniform4d;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramUniform4dEXT(UInt32 program, Int32 location, Double x, Double y, Double z, Double w);
			[CLSCompliant(false)]
			public static ProgramUniform4dEXT glProgramUniform4dEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniform4dv(UInt32 program, Int32 location, Int32 count, Double* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniform4dv glProgramUniform4dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniform4dvEXT(UInt32 program, Int32 location, Int32 count, Double* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniform4dvEXT glProgramUniform4dvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramUniform4f(UInt32 program, Int32 location, Single v0, Single v1, Single v2, Single v3);
			[CLSCompliant(false)]
			public static ProgramUniform4f glProgramUniform4f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramUniform4fEXT(UInt32 program, Int32 location, Single v0, Single v1, Single v2, Single v3);
			[CLSCompliant(false)]
			public static ProgramUniform4fEXT glProgramUniform4fEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniform4fv(UInt32 program, Int32 location, Int32 count, Single* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniform4fv glProgramUniform4fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniform4fvEXT(UInt32 program, Int32 location, Int32 count, Single* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniform4fvEXT glProgramUniform4fvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramUniform4i(UInt32 program, Int32 location, Int32 v0, Int32 v1, Int32 v2, Int32 v3);
			[CLSCompliant(false)]
			public static ProgramUniform4i glProgramUniform4i;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramUniform4i64NV(UInt32 program, Int32 location, Int64 x, Int64 y, Int64 z, Int64 w);
			[CLSCompliant(false)]
			public static ProgramUniform4i64NV glProgramUniform4i64NV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniform4i64vNV(UInt32 program, Int32 location, Int32 count, Int64* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniform4i64vNV glProgramUniform4i64vNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramUniform4iEXT(UInt32 program, Int32 location, Int32 v0, Int32 v1, Int32 v2, Int32 v3);
			[CLSCompliant(false)]
			public static ProgramUniform4iEXT glProgramUniform4iEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniform4iv(UInt32 program, Int32 location, Int32 count, Int32* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniform4iv glProgramUniform4iv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniform4ivEXT(UInt32 program, Int32 location, Int32 count, Int32* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniform4ivEXT glProgramUniform4ivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramUniform4ui(UInt32 program, Int32 location, UInt32 v0, UInt32 v1, UInt32 v2, UInt32 v3);
			[CLSCompliant(false)]
			public static ProgramUniform4ui glProgramUniform4ui;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramUniform4ui64NV(UInt32 program, Int32 location, UInt64 x, UInt64 y, UInt64 z, UInt64 w);
			[CLSCompliant(false)]
			public static ProgramUniform4ui64NV glProgramUniform4ui64NV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniform4ui64vNV(UInt32 program, Int32 location, Int32 count, UInt64* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniform4ui64vNV glProgramUniform4ui64vNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramUniform4uiEXT(UInt32 program, Int32 location, UInt32 v0, UInt32 v1, UInt32 v2, UInt32 v3);
			[CLSCompliant(false)]
			public static ProgramUniform4uiEXT glProgramUniform4uiEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniform4uiv(UInt32 program, Int32 location, Int32 count, UInt32* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniform4uiv glProgramUniform4uiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniform4uivEXT(UInt32 program, Int32 location, Int32 count, UInt32* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniform4uivEXT glProgramUniform4uivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniformMatrix2dv(UInt32 program, Int32 location, Int32 count, bool transpose, Double* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniformMatrix2dv glProgramUniformMatrix2dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniformMatrix2dvEXT(UInt32 program, Int32 location, Int32 count, bool transpose, Double* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniformMatrix2dvEXT glProgramUniformMatrix2dvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniformMatrix2fv(UInt32 program, Int32 location, Int32 count, bool transpose, Single* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniformMatrix2fv glProgramUniformMatrix2fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniformMatrix2fvEXT(UInt32 program, Int32 location, Int32 count, bool transpose, Single* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniformMatrix2fvEXT glProgramUniformMatrix2fvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniformMatrix2x3dv(UInt32 program, Int32 location, Int32 count, bool transpose, Double* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniformMatrix2x3dv glProgramUniformMatrix2x3dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniformMatrix2x3dvEXT(UInt32 program, Int32 location, Int32 count, bool transpose, Double* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniformMatrix2x3dvEXT glProgramUniformMatrix2x3dvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniformMatrix2x3fv(UInt32 program, Int32 location, Int32 count, bool transpose, Single* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniformMatrix2x3fv glProgramUniformMatrix2x3fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniformMatrix2x3fvEXT(UInt32 program, Int32 location, Int32 count, bool transpose, Single* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniformMatrix2x3fvEXT glProgramUniformMatrix2x3fvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniformMatrix2x4dv(UInt32 program, Int32 location, Int32 count, bool transpose, Double* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniformMatrix2x4dv glProgramUniformMatrix2x4dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniformMatrix2x4dvEXT(UInt32 program, Int32 location, Int32 count, bool transpose, Double* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniformMatrix2x4dvEXT glProgramUniformMatrix2x4dvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniformMatrix2x4fv(UInt32 program, Int32 location, Int32 count, bool transpose, Single* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniformMatrix2x4fv glProgramUniformMatrix2x4fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniformMatrix2x4fvEXT(UInt32 program, Int32 location, Int32 count, bool transpose, Single* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniformMatrix2x4fvEXT glProgramUniformMatrix2x4fvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniformMatrix3dv(UInt32 program, Int32 location, Int32 count, bool transpose, Double* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniformMatrix3dv glProgramUniformMatrix3dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniformMatrix3dvEXT(UInt32 program, Int32 location, Int32 count, bool transpose, Double* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniformMatrix3dvEXT glProgramUniformMatrix3dvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniformMatrix3fv(UInt32 program, Int32 location, Int32 count, bool transpose, Single* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniformMatrix3fv glProgramUniformMatrix3fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniformMatrix3fvEXT(UInt32 program, Int32 location, Int32 count, bool transpose, Single* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniformMatrix3fvEXT glProgramUniformMatrix3fvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniformMatrix3x2dv(UInt32 program, Int32 location, Int32 count, bool transpose, Double* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniformMatrix3x2dv glProgramUniformMatrix3x2dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniformMatrix3x2dvEXT(UInt32 program, Int32 location, Int32 count, bool transpose, Double* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniformMatrix3x2dvEXT glProgramUniformMatrix3x2dvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniformMatrix3x2fv(UInt32 program, Int32 location, Int32 count, bool transpose, Single* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniformMatrix3x2fv glProgramUniformMatrix3x2fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniformMatrix3x2fvEXT(UInt32 program, Int32 location, Int32 count, bool transpose, Single* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniformMatrix3x2fvEXT glProgramUniformMatrix3x2fvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniformMatrix3x4dv(UInt32 program, Int32 location, Int32 count, bool transpose, Double* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniformMatrix3x4dv glProgramUniformMatrix3x4dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniformMatrix3x4dvEXT(UInt32 program, Int32 location, Int32 count, bool transpose, Double* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniformMatrix3x4dvEXT glProgramUniformMatrix3x4dvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniformMatrix3x4fv(UInt32 program, Int32 location, Int32 count, bool transpose, Single* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniformMatrix3x4fv glProgramUniformMatrix3x4fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniformMatrix3x4fvEXT(UInt32 program, Int32 location, Int32 count, bool transpose, Single* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniformMatrix3x4fvEXT glProgramUniformMatrix3x4fvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniformMatrix4dv(UInt32 program, Int32 location, Int32 count, bool transpose, Double* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniformMatrix4dv glProgramUniformMatrix4dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniformMatrix4dvEXT(UInt32 program, Int32 location, Int32 count, bool transpose, Double* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniformMatrix4dvEXT glProgramUniformMatrix4dvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniformMatrix4fv(UInt32 program, Int32 location, Int32 count, bool transpose, Single* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniformMatrix4fv glProgramUniformMatrix4fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniformMatrix4fvEXT(UInt32 program, Int32 location, Int32 count, bool transpose, Single* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniformMatrix4fvEXT glProgramUniformMatrix4fvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniformMatrix4x2dv(UInt32 program, Int32 location, Int32 count, bool transpose, Double* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniformMatrix4x2dv glProgramUniformMatrix4x2dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniformMatrix4x2dvEXT(UInt32 program, Int32 location, Int32 count, bool transpose, Double* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniformMatrix4x2dvEXT glProgramUniformMatrix4x2dvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniformMatrix4x2fv(UInt32 program, Int32 location, Int32 count, bool transpose, Single* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniformMatrix4x2fv glProgramUniformMatrix4x2fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniformMatrix4x2fvEXT(UInt32 program, Int32 location, Int32 count, bool transpose, Single* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniformMatrix4x2fvEXT glProgramUniformMatrix4x2fvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniformMatrix4x3dv(UInt32 program, Int32 location, Int32 count, bool transpose, Double* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniformMatrix4x3dv glProgramUniformMatrix4x3dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniformMatrix4x3dvEXT(UInt32 program, Int32 location, Int32 count, bool transpose, Double* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniformMatrix4x3dvEXT glProgramUniformMatrix4x3dvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniformMatrix4x3fv(UInt32 program, Int32 location, Int32 count, bool transpose, Single* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniformMatrix4x3fv glProgramUniformMatrix4x3fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniformMatrix4x3fvEXT(UInt32 program, Int32 location, Int32 count, bool transpose, Single* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniformMatrix4x3fvEXT glProgramUniformMatrix4x3fvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramUniformui64NV(UInt32 program, Int32 location, UInt64 value);
			[CLSCompliant(false)]
			public static ProgramUniformui64NV glProgramUniformui64NV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ProgramUniformui64vNV(UInt32 program, Int32 location, Int32 count, UInt64* value);
			[CLSCompliant(false)]
			public unsafe static ProgramUniformui64vNV glProgramUniformui64vNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProgramVertexLimitNV(System.Graphics.OGL.NvGeometryProgram4 target, Int32 limit);
			[CLSCompliant(false)]
			public static ProgramVertexLimitNV glProgramVertexLimitNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProvokingVertex(System.Graphics.OGL.ProvokingVertexMode mode);
			[CLSCompliant(false)]
			public static ProvokingVertex glProvokingVertex;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ProvokingVertexEXT(System.Graphics.OGL.ExtProvokingVertex mode);
			[CLSCompliant(false)]
			public static ProvokingVertexEXT glProvokingVertexEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void PushAttrib(System.Graphics.OGL.AttribMask mask);
			[CLSCompliant(false)]
			public static PushAttrib glPushAttrib;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void PushClientAttrib(System.Graphics.OGL.ClientAttribMask mask);
			[CLSCompliant(false)]
			public static PushClientAttrib glPushClientAttrib;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void PushClientAttribDefaultEXT(System.Graphics.OGL.ClientAttribMask mask);
			[CLSCompliant(false)]
			public static PushClientAttribDefaultEXT glPushClientAttribDefaultEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void PushMatrix();
			[CLSCompliant(false)]
			public static PushMatrix glPushMatrix;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void PushName(UInt32 name);
			[CLSCompliant(false)]
			public static PushName glPushName;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void QueryCounter(UInt32 id, System.Graphics.OGL.QueryCounterTarget target);
			[CLSCompliant(false)]
			public static QueryCounter glQueryCounter;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void RasterPos2d(Double x, Double y);
			[CLSCompliant(false)]
			public static RasterPos2d glRasterPos2d;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void RasterPos2dv(Double* v);
			[CLSCompliant(false)]
			public unsafe static RasterPos2dv glRasterPos2dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void RasterPos2f(Single x, Single y);
			[CLSCompliant(false)]
			public static RasterPos2f glRasterPos2f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void RasterPos2fv(Single* v);
			[CLSCompliant(false)]
			public unsafe static RasterPos2fv glRasterPos2fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void RasterPos2i(Int32 x, Int32 y);
			[CLSCompliant(false)]
			public static RasterPos2i glRasterPos2i;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void RasterPos2iv(Int32* v);
			[CLSCompliant(false)]
			public unsafe static RasterPos2iv glRasterPos2iv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void RasterPos2s(Int16 x, Int16 y);
			[CLSCompliant(false)]
			public static RasterPos2s glRasterPos2s;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void RasterPos2sv(Int16* v);
			[CLSCompliant(false)]
			public unsafe static RasterPos2sv glRasterPos2sv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void RasterPos3d(Double x, Double y, Double z);
			[CLSCompliant(false)]
			public static RasterPos3d glRasterPos3d;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void RasterPos3dv(Double* v);
			[CLSCompliant(false)]
			public unsafe static RasterPos3dv glRasterPos3dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void RasterPos3f(Single x, Single y, Single z);
			[CLSCompliant(false)]
			public static RasterPos3f glRasterPos3f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void RasterPos3fv(Single* v);
			[CLSCompliant(false)]
			public unsafe static RasterPos3fv glRasterPos3fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void RasterPos3i(Int32 x, Int32 y, Int32 z);
			[CLSCompliant(false)]
			public static RasterPos3i glRasterPos3i;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void RasterPos3iv(Int32* v);
			[CLSCompliant(false)]
			public unsafe static RasterPos3iv glRasterPos3iv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void RasterPos3s(Int16 x, Int16 y, Int16 z);
			[CLSCompliant(false)]
			public static RasterPos3s glRasterPos3s;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void RasterPos3sv(Int16* v);
			[CLSCompliant(false)]
			public unsafe static RasterPos3sv glRasterPos3sv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void RasterPos4d(Double x, Double y, Double z, Double w);
			[CLSCompliant(false)]
			public static RasterPos4d glRasterPos4d;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void RasterPos4dv(Double* v);
			[CLSCompliant(false)]
			public unsafe static RasterPos4dv glRasterPos4dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void RasterPos4f(Single x, Single y, Single z, Single w);
			[CLSCompliant(false)]
			public static RasterPos4f glRasterPos4f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void RasterPos4fv(Single* v);
			[CLSCompliant(false)]
			public unsafe static RasterPos4fv glRasterPos4fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void RasterPos4i(Int32 x, Int32 y, Int32 z, Int32 w);
			[CLSCompliant(false)]
			public static RasterPos4i glRasterPos4i;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void RasterPos4iv(Int32* v);
			[CLSCompliant(false)]
			public unsafe static RasterPos4iv glRasterPos4iv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void RasterPos4s(Int16 x, Int16 y, Int16 z, Int16 w);
			[CLSCompliant(false)]
			public static RasterPos4s glRasterPos4s;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void RasterPos4sv(Int16* v);
			[CLSCompliant(false)]
			public unsafe static RasterPos4sv glRasterPos4sv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ReadBuffer(System.Graphics.OGL.ReadBufferMode mode);
			[CLSCompliant(false)]
			public static ReadBuffer glReadBuffer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ReadInstrumentsSGIX(Int32 marker);
			[CLSCompliant(false)]
			public static ReadInstrumentsSGIX glReadInstrumentsSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ReadnPixelsARB(Int32 x, Int32 y, Int32 width, Int32 height, System.Graphics.OGL.ArbRobustness format, System.Graphics.OGL.ArbRobustness type, Int32 bufSize, [OutAttribute] IntPtr data);
			[CLSCompliant(false)]
			public static ReadnPixelsARB glReadnPixelsARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ReadPixels(Int32 x, Int32 y, Int32 width, Int32 height, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, [OutAttribute] IntPtr pixels);
			[CLSCompliant(false)]
			public static ReadPixels glReadPixels;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Rectd(Double x1, Double y1, Double x2, Double y2);
			[CLSCompliant(false)]
			public static Rectd glRectd;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Rectdv(Double* v1, Double* v2);
			[CLSCompliant(false)]
			public unsafe static Rectdv glRectdv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Rectf(Single x1, Single y1, Single x2, Single y2);
			[CLSCompliant(false)]
			public static Rectf glRectf;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Rectfv(Single* v1, Single* v2);
			[CLSCompliant(false)]
			public unsafe static Rectfv glRectfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Recti(Int32 x1, Int32 y1, Int32 x2, Int32 y2);
			[CLSCompliant(false)]
			public static Recti glRecti;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Rectiv(Int32* v1, Int32* v2);
			[CLSCompliant(false)]
			public unsafe static Rectiv glRectiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Rects(Int16 x1, Int16 y1, Int16 x2, Int16 y2);
			[CLSCompliant(false)]
			public static Rects glRects;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Rectsv(Int16* v1, Int16* v2);
			[CLSCompliant(false)]
			public unsafe static Rectsv glRectsv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ReferencePlaneSGIX(Double* equation);
			[CLSCompliant(false)]
			public unsafe static ReferencePlaneSGIX glReferencePlaneSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ReleaseShaderCompiler();
			[CLSCompliant(false)]
			public static ReleaseShaderCompiler glReleaseShaderCompiler;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void RenderbufferStorage(System.Graphics.OGL.RenderbufferTarget target, System.Graphics.OGL.RenderbufferStorage internalformat, Int32 width, Int32 height);
			[CLSCompliant(false)]
			public static RenderbufferStorage glRenderbufferStorage;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void RenderbufferStorageEXT(System.Graphics.OGL.RenderbufferTarget target, System.Graphics.OGL.RenderbufferStorage internalformat, Int32 width, Int32 height);
			[CLSCompliant(false)]
			public static RenderbufferStorageEXT glRenderbufferStorageEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void RenderbufferStorageMultisample(System.Graphics.OGL.RenderbufferTarget target, Int32 samples, System.Graphics.OGL.RenderbufferStorage internalformat, Int32 width, Int32 height);
			[CLSCompliant(false)]
			public static RenderbufferStorageMultisample glRenderbufferStorageMultisample;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void RenderbufferStorageMultisampleCoverageNV(System.Graphics.OGL.RenderbufferTarget target, Int32 coverageSamples, Int32 colorSamples, System.Graphics.OGL.PixelInternalFormat internalformat, Int32 width, Int32 height);
			[CLSCompliant(false)]
			public static RenderbufferStorageMultisampleCoverageNV glRenderbufferStorageMultisampleCoverageNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void RenderbufferStorageMultisampleEXT(System.Graphics.OGL.ExtFramebufferMultisample target, Int32 samples, System.Graphics.OGL.ExtFramebufferMultisample internalformat, Int32 width, Int32 height);
			[CLSCompliant(false)]
			public static RenderbufferStorageMultisampleEXT glRenderbufferStorageMultisampleEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate Int32 RenderMode(System.Graphics.OGL.RenderingMode mode);
			[CLSCompliant(false)]
			public static RenderMode glRenderMode;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ReplacementCodePointerSUN(System.Graphics.OGL.SunTriangleList type, Int32 stride, IntPtr pointer);
			[CLSCompliant(false)]
			public static ReplacementCodePointerSUN glReplacementCodePointerSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ReplacementCodeubSUN(Byte code);
			[CLSCompliant(false)]
			public static ReplacementCodeubSUN glReplacementCodeubSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ReplacementCodeubvSUN(Byte* code);
			[CLSCompliant(false)]
			public unsafe static ReplacementCodeubvSUN glReplacementCodeubvSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ReplacementCodeuiColor3fVertex3fSUN(UInt32 rc, Single r, Single g, Single b, Single x, Single y, Single z);
			[CLSCompliant(false)]
			public static ReplacementCodeuiColor3fVertex3fSUN glReplacementCodeuiColor3fVertex3fSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ReplacementCodeuiColor3fVertex3fvSUN(UInt32* rc, Single* c, Single* v);
			[CLSCompliant(false)]
			public unsafe static ReplacementCodeuiColor3fVertex3fvSUN glReplacementCodeuiColor3fVertex3fvSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ReplacementCodeuiColor4fNormal3fVertex3fSUN(UInt32 rc, Single r, Single g, Single b, Single a, Single nx, Single ny, Single nz, Single x, Single y, Single z);
			[CLSCompliant(false)]
			public static ReplacementCodeuiColor4fNormal3fVertex3fSUN glReplacementCodeuiColor4fNormal3fVertex3fSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ReplacementCodeuiColor4fNormal3fVertex3fvSUN(UInt32* rc, Single* c, Single* n, Single* v);
			[CLSCompliant(false)]
			public unsafe static ReplacementCodeuiColor4fNormal3fVertex3fvSUN glReplacementCodeuiColor4fNormal3fVertex3fvSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ReplacementCodeuiColor4ubVertex3fSUN(UInt32 rc, Byte r, Byte g, Byte b, Byte a, Single x, Single y, Single z);
			[CLSCompliant(false)]
			public static ReplacementCodeuiColor4ubVertex3fSUN glReplacementCodeuiColor4ubVertex3fSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ReplacementCodeuiColor4ubVertex3fvSUN(UInt32* rc, Byte* c, Single* v);
			[CLSCompliant(false)]
			public unsafe static ReplacementCodeuiColor4ubVertex3fvSUN glReplacementCodeuiColor4ubVertex3fvSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ReplacementCodeuiNormal3fVertex3fSUN(UInt32 rc, Single nx, Single ny, Single nz, Single x, Single y, Single z);
			[CLSCompliant(false)]
			public static ReplacementCodeuiNormal3fVertex3fSUN glReplacementCodeuiNormal3fVertex3fSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ReplacementCodeuiNormal3fVertex3fvSUN(UInt32* rc, Single* n, Single* v);
			[CLSCompliant(false)]
			public unsafe static ReplacementCodeuiNormal3fVertex3fvSUN glReplacementCodeuiNormal3fVertex3fvSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ReplacementCodeuiSUN(UInt32 code);
			[CLSCompliant(false)]
			public static ReplacementCodeuiSUN glReplacementCodeuiSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ReplacementCodeuiTexCoord2fColor4fNormal3fVertex3fSUN(UInt32 rc, Single s, Single t, Single r, Single g, Single b, Single a, Single nx, Single ny, Single nz, Single x, Single y, Single z);
			[CLSCompliant(false)]
			public static ReplacementCodeuiTexCoord2fColor4fNormal3fVertex3fSUN glReplacementCodeuiTexCoord2fColor4fNormal3fVertex3fSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ReplacementCodeuiTexCoord2fColor4fNormal3fVertex3fvSUN(UInt32* rc, Single* tc, Single* c, Single* n, Single* v);
			[CLSCompliant(false)]
			public unsafe static ReplacementCodeuiTexCoord2fColor4fNormal3fVertex3fvSUN glReplacementCodeuiTexCoord2fColor4fNormal3fVertex3fvSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ReplacementCodeuiTexCoord2fNormal3fVertex3fSUN(UInt32 rc, Single s, Single t, Single nx, Single ny, Single nz, Single x, Single y, Single z);
			[CLSCompliant(false)]
			public static ReplacementCodeuiTexCoord2fNormal3fVertex3fSUN glReplacementCodeuiTexCoord2fNormal3fVertex3fSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ReplacementCodeuiTexCoord2fNormal3fVertex3fvSUN(UInt32* rc, Single* tc, Single* n, Single* v);
			[CLSCompliant(false)]
			public unsafe static ReplacementCodeuiTexCoord2fNormal3fVertex3fvSUN glReplacementCodeuiTexCoord2fNormal3fVertex3fvSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ReplacementCodeuiTexCoord2fVertex3fSUN(UInt32 rc, Single s, Single t, Single x, Single y, Single z);
			[CLSCompliant(false)]
			public static ReplacementCodeuiTexCoord2fVertex3fSUN glReplacementCodeuiTexCoord2fVertex3fSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ReplacementCodeuiTexCoord2fVertex3fvSUN(UInt32* rc, Single* tc, Single* v);
			[CLSCompliant(false)]
			public unsafe static ReplacementCodeuiTexCoord2fVertex3fvSUN glReplacementCodeuiTexCoord2fVertex3fvSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ReplacementCodeuiVertex3fSUN(UInt32 rc, Single x, Single y, Single z);
			[CLSCompliant(false)]
			public static ReplacementCodeuiVertex3fSUN glReplacementCodeuiVertex3fSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ReplacementCodeuiVertex3fvSUN(UInt32* rc, Single* v);
			[CLSCompliant(false)]
			public unsafe static ReplacementCodeuiVertex3fvSUN glReplacementCodeuiVertex3fvSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ReplacementCodeuivSUN(UInt32* code);
			[CLSCompliant(false)]
			public unsafe static ReplacementCodeuivSUN glReplacementCodeuivSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ReplacementCodeusSUN(UInt16 code);
			[CLSCompliant(false)]
			public static ReplacementCodeusSUN glReplacementCodeusSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ReplacementCodeusvSUN(UInt16* code);
			[CLSCompliant(false)]
			public unsafe static ReplacementCodeusvSUN glReplacementCodeusvSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void RequestResidentProgramsNV(Int32 n, UInt32* programs);
			[CLSCompliant(false)]
			public unsafe static RequestResidentProgramsNV glRequestResidentProgramsNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ResetHistogram(System.Graphics.OGL.HistogramTarget target);
			[CLSCompliant(false)]
			public static ResetHistogram glResetHistogram;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ResetHistogramEXT(System.Graphics.OGL.ExtHistogram target);
			[CLSCompliant(false)]
			public static ResetHistogramEXT glResetHistogramEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ResetMinmax(System.Graphics.OGL.MinmaxTarget target);
			[CLSCompliant(false)]
			public static ResetMinmax glResetMinmax;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ResetMinmaxEXT(System.Graphics.OGL.ExtHistogram target);
			[CLSCompliant(false)]
			public static ResetMinmaxEXT glResetMinmaxEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ResizeBuffersMESA();
			[CLSCompliant(false)]
			public static ResizeBuffersMESA glResizeBuffersMESA;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ResumeTransformFeedback();
			[CLSCompliant(false)]
			public static ResumeTransformFeedback glResumeTransformFeedback;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ResumeTransformFeedbackNV();
			[CLSCompliant(false)]
			public static ResumeTransformFeedbackNV glResumeTransformFeedbackNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Rotated(Double angle, Double x, Double y, Double z);
			[CLSCompliant(false)]
			public static Rotated glRotated;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Rotatef(Single angle, Single x, Single y, Single z);
			[CLSCompliant(false)]
			public static Rotatef glRotatef;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SampleCoverage(Single value, bool invert);
			[CLSCompliant(false)]
			public static SampleCoverage glSampleCoverage;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SampleCoverageARB(Single value, bool invert);
			[CLSCompliant(false)]
			public static SampleCoverageARB glSampleCoverageARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SampleMapATI(UInt32 dst, UInt32 interp, System.Graphics.OGL.AtiFragmentShader swizzle);
			[CLSCompliant(false)]
			public static SampleMapATI glSampleMapATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SampleMaskEXT(Single value, bool invert);
			[CLSCompliant(false)]
			public static SampleMaskEXT glSampleMaskEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SampleMaski(UInt32 index, UInt32 mask);
			[CLSCompliant(false)]
			public static SampleMaski glSampleMaski;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SampleMaskIndexedNV(UInt32 index, UInt32 mask);
			[CLSCompliant(false)]
			public static SampleMaskIndexedNV glSampleMaskIndexedNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SampleMaskSGIS(Single value, bool invert);
			[CLSCompliant(false)]
			public static SampleMaskSGIS glSampleMaskSGIS;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SamplePatternEXT(System.Graphics.OGL.ExtMultisample pattern);
			[CLSCompliant(false)]
			public static SamplePatternEXT glSamplePatternEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SamplePatternSGIS(System.Graphics.OGL.SgisMultisample pattern);
			[CLSCompliant(false)]
			public static SamplePatternSGIS glSamplePatternSGIS;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SamplerParameterf(UInt32 sampler, System.Graphics.OGL.SamplerParameter pname, Single param);
			[CLSCompliant(false)]
			public static SamplerParameterf glSamplerParameterf;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void SamplerParameterfv(UInt32 sampler, System.Graphics.OGL.SamplerParameter pname, Single* param);
			[CLSCompliant(false)]
			public unsafe static SamplerParameterfv glSamplerParameterfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SamplerParameteri(UInt32 sampler, System.Graphics.OGL.SamplerParameter pname, Int32 param);
			[CLSCompliant(false)]
			public static SamplerParameteri glSamplerParameteri;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void SamplerParameterIiv(UInt32 sampler, System.Graphics.OGL.ArbSamplerObjects pname, Int32* param);
			[CLSCompliant(false)]
			public unsafe static SamplerParameterIiv glSamplerParameterIiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void SamplerParameterIuiv(UInt32 sampler, System.Graphics.OGL.ArbSamplerObjects pname, UInt32* param);
			[CLSCompliant(false)]
			public unsafe static SamplerParameterIuiv glSamplerParameterIuiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void SamplerParameteriv(UInt32 sampler, System.Graphics.OGL.SamplerParameter pname, Int32* param);
			[CLSCompliant(false)]
			public unsafe static SamplerParameteriv glSamplerParameteriv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Scaled(Double x, Double y, Double z);
			[CLSCompliant(false)]
			public static Scaled glScaled;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Scalef(Single x, Single y, Single z);
			[CLSCompliant(false)]
			public static Scalef glScalef;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Scissor(Int32 x, Int32 y, Int32 width, Int32 height);
			[CLSCompliant(false)]
			public static Scissor glScissor;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ScissorArrayv(UInt32 first, Int32 count, Int32* v);
			[CLSCompliant(false)]
			public unsafe static ScissorArrayv glScissorArrayv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ScissorIndexed(UInt32 index, Int32 left, Int32 bottom, Int32 width, Int32 height);
			[CLSCompliant(false)]
			public static ScissorIndexed glScissorIndexed;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ScissorIndexedv(UInt32 index, Int32* v);
			[CLSCompliant(false)]
			public unsafe static ScissorIndexedv glScissorIndexedv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SecondaryColor3b(SByte red, SByte green, SByte blue);
			[CLSCompliant(false)]
			public static SecondaryColor3b glSecondaryColor3b;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SecondaryColor3bEXT(SByte red, SByte green, SByte blue);
			[CLSCompliant(false)]
			public static SecondaryColor3bEXT glSecondaryColor3bEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void SecondaryColor3bv(SByte* v);
			[CLSCompliant(false)]
			public unsafe static SecondaryColor3bv glSecondaryColor3bv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void SecondaryColor3bvEXT(SByte* v);
			[CLSCompliant(false)]
			public unsafe static SecondaryColor3bvEXT glSecondaryColor3bvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SecondaryColor3d(Double red, Double green, Double blue);
			[CLSCompliant(false)]
			public static SecondaryColor3d glSecondaryColor3d;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SecondaryColor3dEXT(Double red, Double green, Double blue);
			[CLSCompliant(false)]
			public static SecondaryColor3dEXT glSecondaryColor3dEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void SecondaryColor3dv(Double* v);
			[CLSCompliant(false)]
			public unsafe static SecondaryColor3dv glSecondaryColor3dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void SecondaryColor3dvEXT(Double* v);
			[CLSCompliant(false)]
			public unsafe static SecondaryColor3dvEXT glSecondaryColor3dvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SecondaryColor3f(Single red, Single green, Single blue);
			[CLSCompliant(false)]
			public static SecondaryColor3f glSecondaryColor3f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SecondaryColor3fEXT(Single red, Single green, Single blue);
			[CLSCompliant(false)]
			public static SecondaryColor3fEXT glSecondaryColor3fEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void SecondaryColor3fv(Single* v);
			[CLSCompliant(false)]
			public unsafe static SecondaryColor3fv glSecondaryColor3fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void SecondaryColor3fvEXT(Single* v);
			[CLSCompliant(false)]
			public unsafe static SecondaryColor3fvEXT glSecondaryColor3fvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SecondaryColor3hNV(Half red, Half green, Half blue);
			[CLSCompliant(false)]
			public static SecondaryColor3hNV glSecondaryColor3hNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void SecondaryColor3hvNV(Half* v);
			[CLSCompliant(false)]
			public unsafe static SecondaryColor3hvNV glSecondaryColor3hvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SecondaryColor3i(Int32 red, Int32 green, Int32 blue);
			[CLSCompliant(false)]
			public static SecondaryColor3i glSecondaryColor3i;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SecondaryColor3iEXT(Int32 red, Int32 green, Int32 blue);
			[CLSCompliant(false)]
			public static SecondaryColor3iEXT glSecondaryColor3iEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void SecondaryColor3iv(Int32* v);
			[CLSCompliant(false)]
			public unsafe static SecondaryColor3iv glSecondaryColor3iv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void SecondaryColor3ivEXT(Int32* v);
			[CLSCompliant(false)]
			public unsafe static SecondaryColor3ivEXT glSecondaryColor3ivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SecondaryColor3s(Int16 red, Int16 green, Int16 blue);
			[CLSCompliant(false)]
			public static SecondaryColor3s glSecondaryColor3s;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SecondaryColor3sEXT(Int16 red, Int16 green, Int16 blue);
			[CLSCompliant(false)]
			public static SecondaryColor3sEXT glSecondaryColor3sEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void SecondaryColor3sv(Int16* v);
			[CLSCompliant(false)]
			public unsafe static SecondaryColor3sv glSecondaryColor3sv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void SecondaryColor3svEXT(Int16* v);
			[CLSCompliant(false)]
			public unsafe static SecondaryColor3svEXT glSecondaryColor3svEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SecondaryColor3ub(Byte red, Byte green, Byte blue);
			[CLSCompliant(false)]
			public static SecondaryColor3ub glSecondaryColor3ub;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SecondaryColor3ubEXT(Byte red, Byte green, Byte blue);
			[CLSCompliant(false)]
			public static SecondaryColor3ubEXT glSecondaryColor3ubEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void SecondaryColor3ubv(Byte* v);
			[CLSCompliant(false)]
			public unsafe static SecondaryColor3ubv glSecondaryColor3ubv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void SecondaryColor3ubvEXT(Byte* v);
			[CLSCompliant(false)]
			public unsafe static SecondaryColor3ubvEXT glSecondaryColor3ubvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SecondaryColor3ui(UInt32 red, UInt32 green, UInt32 blue);
			[CLSCompliant(false)]
			public static SecondaryColor3ui glSecondaryColor3ui;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SecondaryColor3uiEXT(UInt32 red, UInt32 green, UInt32 blue);
			[CLSCompliant(false)]
			public static SecondaryColor3uiEXT glSecondaryColor3uiEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void SecondaryColor3uiv(UInt32* v);
			[CLSCompliant(false)]
			public unsafe static SecondaryColor3uiv glSecondaryColor3uiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void SecondaryColor3uivEXT(UInt32* v);
			[CLSCompliant(false)]
			public unsafe static SecondaryColor3uivEXT glSecondaryColor3uivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SecondaryColor3us(UInt16 red, UInt16 green, UInt16 blue);
			[CLSCompliant(false)]
			public static SecondaryColor3us glSecondaryColor3us;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SecondaryColor3usEXT(UInt16 red, UInt16 green, UInt16 blue);
			[CLSCompliant(false)]
			public static SecondaryColor3usEXT glSecondaryColor3usEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void SecondaryColor3usv(UInt16* v);
			[CLSCompliant(false)]
			public unsafe static SecondaryColor3usv glSecondaryColor3usv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void SecondaryColor3usvEXT(UInt16* v);
			[CLSCompliant(false)]
			public unsafe static SecondaryColor3usvEXT glSecondaryColor3usvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SecondaryColorFormatNV(Int32 size, System.Graphics.OGL.NvVertexBufferUnifiedMemory type, Int32 stride);
			[CLSCompliant(false)]
			public static SecondaryColorFormatNV glSecondaryColorFormatNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SecondaryColorP3ui(System.Graphics.OGL.PackedPointerType type, UInt32 color);
			[CLSCompliant(false)]
			public static SecondaryColorP3ui glSecondaryColorP3ui;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void SecondaryColorP3uiv(System.Graphics.OGL.PackedPointerType type, UInt32* color);
			[CLSCompliant(false)]
			public unsafe static SecondaryColorP3uiv glSecondaryColorP3uiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SecondaryColorPointer(Int32 size, System.Graphics.OGL.ColorPointerType type, Int32 stride, IntPtr pointer);
			[CLSCompliant(false)]
			public static SecondaryColorPointer glSecondaryColorPointer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SecondaryColorPointerEXT(Int32 size, System.Graphics.OGL.ColorPointerType type, Int32 stride, IntPtr pointer);
			[CLSCompliant(false)]
			public static SecondaryColorPointerEXT glSecondaryColorPointerEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SecondaryColorPointerListIBM(Int32 size, System.Graphics.OGL.IbmVertexArrayLists type, Int32 stride, IntPtr pointer, Int32 ptrstride);
			[CLSCompliant(false)]
			public static SecondaryColorPointerListIBM glSecondaryColorPointerListIBM;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void SelectBuffer(Int32 size, [OutAttribute] UInt32* buffer);
			[CLSCompliant(false)]
			public unsafe static SelectBuffer glSelectBuffer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void SelectPerfMonitorCountersAMD(UInt32 monitor, bool enable, UInt32 group, Int32 numCounters, [OutAttribute] UInt32* counterList);
			[CLSCompliant(false)]
			public unsafe static SelectPerfMonitorCountersAMD glSelectPerfMonitorCountersAMD;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SeparableFilter2D(System.Graphics.OGL.SeparableTarget target, System.Graphics.OGL.PixelInternalFormat internalformat, Int32 width, Int32 height, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, IntPtr row, IntPtr column);
			[CLSCompliant(false)]
			public static SeparableFilter2D glSeparableFilter2D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SeparableFilter2DEXT(System.Graphics.OGL.ExtConvolution target, System.Graphics.OGL.PixelInternalFormat internalformat, Int32 width, Int32 height, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, IntPtr row, IntPtr column);
			[CLSCompliant(false)]
			public static SeparableFilter2DEXT glSeparableFilter2DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SetFenceAPPLE(UInt32 fence);
			[CLSCompliant(false)]
			public static SetFenceAPPLE glSetFenceAPPLE;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SetFenceNV(UInt32 fence, System.Graphics.OGL.NvFence condition);
			[CLSCompliant(false)]
			public static SetFenceNV glSetFenceNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void SetFragmentShaderConstantATI(UInt32 dst, Single* value);
			[CLSCompliant(false)]
			public unsafe static SetFragmentShaderConstantATI glSetFragmentShaderConstantATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SetInvariantEXT(UInt32 id, System.Graphics.OGL.ExtVertexShader type, IntPtr addr);
			[CLSCompliant(false)]
			public static SetInvariantEXT glSetInvariantEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SetLocalConstantEXT(UInt32 id, System.Graphics.OGL.ExtVertexShader type, IntPtr addr);
			[CLSCompliant(false)]
			public static SetLocalConstantEXT glSetLocalConstantEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void SetMultisamplefvAMD(System.Graphics.OGL.AmdSamplePositions pname, UInt32 index, Single* val);
			[CLSCompliant(false)]
			public unsafe static SetMultisamplefvAMD glSetMultisamplefvAMD;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ShadeModel(System.Graphics.OGL.ShadingModel mode);
			[CLSCompliant(false)]
			public static ShadeModel glShadeModel;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ShaderBinary(Int32 count, UInt32* shaders, System.Graphics.OGL.BinaryFormat binaryformat, IntPtr binary, Int32 length);
			[CLSCompliant(false)]
			public unsafe static ShaderBinary glShaderBinary;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ShaderOp1EXT(System.Graphics.OGL.ExtVertexShader op, UInt32 res, UInt32 arg1);
			[CLSCompliant(false)]
			public static ShaderOp1EXT glShaderOp1EXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ShaderOp2EXT(System.Graphics.OGL.ExtVertexShader op, UInt32 res, UInt32 arg1, UInt32 arg2);
			[CLSCompliant(false)]
			public static ShaderOp2EXT glShaderOp2EXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ShaderOp3EXT(System.Graphics.OGL.ExtVertexShader op, UInt32 res, UInt32 arg1, UInt32 arg2, UInt32 arg3);
			[CLSCompliant(false)]
			public static ShaderOp3EXT glShaderOp3EXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ShaderSource(UInt32 shader, Int32 count, String[] @string, Int32* length);
			[CLSCompliant(false)]
			public unsafe static ShaderSource glShaderSource;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ShaderSourceARB(UInt32 shaderObj, Int32 count, String[] @string, Int32* length);
			[CLSCompliant(false)]
			public unsafe static ShaderSourceARB glShaderSourceARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void SharpenTexFuncSGIS(System.Graphics.OGL.TextureTarget target, Int32 n, Single* points);
			[CLSCompliant(false)]
			public unsafe static SharpenTexFuncSGIS glSharpenTexFuncSGIS;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SpriteParameterfSGIX(System.Graphics.OGL.SgixSprite pname, Single param);
			[CLSCompliant(false)]
			public static SpriteParameterfSGIX glSpriteParameterfSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void SpriteParameterfvSGIX(System.Graphics.OGL.SgixSprite pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static SpriteParameterfvSGIX glSpriteParameterfvSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SpriteParameteriSGIX(System.Graphics.OGL.SgixSprite pname, Int32 param);
			[CLSCompliant(false)]
			public static SpriteParameteriSGIX glSpriteParameteriSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void SpriteParameterivSGIX(System.Graphics.OGL.SgixSprite pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static SpriteParameterivSGIX glSpriteParameterivSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void StartInstrumentsSGIX();
			[CLSCompliant(false)]
			public static StartInstrumentsSGIX glStartInstrumentsSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void StencilClearTagEXT(Int32 stencilTagBits, UInt32 stencilClearTag);
			[CLSCompliant(false)]
			public static StencilClearTagEXT glStencilClearTagEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void StencilFunc(System.Graphics.OGL.StencilFunction func, Int32 @ref, UInt32 mask);
			[CLSCompliant(false)]
			public static StencilFunc glStencilFunc;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void StencilFuncSeparate(System.Graphics.OGL.Version20 face, System.Graphics.OGL.StencilFunction func, Int32 @ref, UInt32 mask);
			[CLSCompliant(false)]
			public static StencilFuncSeparate glStencilFuncSeparate;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void StencilFuncSeparateATI(System.Graphics.OGL.StencilFunction frontfunc, System.Graphics.OGL.StencilFunction backfunc, Int32 @ref, UInt32 mask);
			[CLSCompliant(false)]
			public static StencilFuncSeparateATI glStencilFuncSeparateATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void StencilMask(UInt32 mask);
			[CLSCompliant(false)]
			public static StencilMask glStencilMask;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void StencilMaskSeparate(System.Graphics.OGL.StencilFace face, UInt32 mask);
			[CLSCompliant(false)]
			public static StencilMaskSeparate glStencilMaskSeparate;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void StencilOp(System.Graphics.OGL.StencilOp fail, System.Graphics.OGL.StencilOp zfail, System.Graphics.OGL.StencilOp zpass);
			[CLSCompliant(false)]
			public static StencilOp glStencilOp;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void StencilOpSeparate(System.Graphics.OGL.StencilFace face, System.Graphics.OGL.StencilOp sfail, System.Graphics.OGL.StencilOp dpfail, System.Graphics.OGL.StencilOp dppass);
			[CLSCompliant(false)]
			public static StencilOpSeparate glStencilOpSeparate;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void StencilOpSeparateATI(System.Graphics.OGL.AtiSeparateStencil face, System.Graphics.OGL.StencilOp sfail, System.Graphics.OGL.StencilOp dpfail, System.Graphics.OGL.StencilOp dppass);
			[CLSCompliant(false)]
			public static StencilOpSeparateATI glStencilOpSeparateATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void StopInstrumentsSGIX(Int32 marker);
			[CLSCompliant(false)]
			public static StopInstrumentsSGIX glStopInstrumentsSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void StringMarkerGREMEDY(Int32 len, IntPtr @string);
			[CLSCompliant(false)]
			public static StringMarkerGREMEDY glStringMarkerGREMEDY;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void SwizzleEXT(UInt32 res, UInt32 @in, System.Graphics.OGL.ExtVertexShader outX, System.Graphics.OGL.ExtVertexShader outY, System.Graphics.OGL.ExtVertexShader outZ, System.Graphics.OGL.ExtVertexShader outW);
			[CLSCompliant(false)]
			public static SwizzleEXT glSwizzleEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TagSampleBufferSGIX();
			[CLSCompliant(false)]
			public static TagSampleBufferSGIX glTagSampleBufferSGIX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Tangent3bEXT(SByte tx, SByte ty, SByte tz);
			[CLSCompliant(false)]
			public static Tangent3bEXT glTangent3bEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Tangent3bvEXT(SByte* v);
			[CLSCompliant(false)]
			public unsafe static Tangent3bvEXT glTangent3bvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Tangent3dEXT(Double tx, Double ty, Double tz);
			[CLSCompliant(false)]
			public static Tangent3dEXT glTangent3dEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Tangent3dvEXT(Double* v);
			[CLSCompliant(false)]
			public unsafe static Tangent3dvEXT glTangent3dvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Tangent3fEXT(Single tx, Single ty, Single tz);
			[CLSCompliant(false)]
			public static Tangent3fEXT glTangent3fEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Tangent3fvEXT(Single* v);
			[CLSCompliant(false)]
			public unsafe static Tangent3fvEXT glTangent3fvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Tangent3iEXT(Int32 tx, Int32 ty, Int32 tz);
			[CLSCompliant(false)]
			public static Tangent3iEXT glTangent3iEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Tangent3ivEXT(Int32* v);
			[CLSCompliant(false)]
			public unsafe static Tangent3ivEXT glTangent3ivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Tangent3sEXT(Int16 tx, Int16 ty, Int16 tz);
			[CLSCompliant(false)]
			public static Tangent3sEXT glTangent3sEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Tangent3svEXT(Int16* v);
			[CLSCompliant(false)]
			public unsafe static Tangent3svEXT glTangent3svEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TangentPointerEXT(System.Graphics.OGL.NormalPointerType type, Int32 stride, IntPtr pointer);
			[CLSCompliant(false)]
			public static TangentPointerEXT glTangentPointerEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TbufferMask3DFX(UInt32 mask);
			[CLSCompliant(false)]
			public static TbufferMask3DFX glTbufferMask3DFX;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TessellationFactorAMD(Single factor);
			[CLSCompliant(false)]
			public static TessellationFactorAMD glTessellationFactorAMD;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TessellationModeAMD(System.Graphics.OGL.AmdVertexShaderTesselator mode);
			[CLSCompliant(false)]
			public static TessellationModeAMD glTessellationModeAMD;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool TestFenceAPPLE(UInt32 fence);
			[CLSCompliant(false)]
			public static TestFenceAPPLE glTestFenceAPPLE;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool TestFenceNV(UInt32 fence);
			[CLSCompliant(false)]
			public static TestFenceNV glTestFenceNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool TestObjectAPPLE(System.Graphics.OGL.AppleFence @object, UInt32 name);
			[CLSCompliant(false)]
			public static TestObjectAPPLE glTestObjectAPPLE;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexBuffer(System.Graphics.OGL.TextureBufferTarget target, System.Graphics.OGL.SizedInternalFormat internalformat, UInt32 buffer);
			[CLSCompliant(false)]
			public static TexBuffer glTexBuffer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexBufferARB(System.Graphics.OGL.TextureTarget target, System.Graphics.OGL.ArbTextureBufferObject internalformat, UInt32 buffer);
			[CLSCompliant(false)]
			public static TexBufferARB glTexBufferARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexBufferEXT(System.Graphics.OGL.TextureTarget target, System.Graphics.OGL.ExtTextureBufferObject internalformat, UInt32 buffer);
			[CLSCompliant(false)]
			public static TexBufferEXT glTexBufferEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexBumpParameterfvATI(System.Graphics.OGL.AtiEnvmapBumpmap pname, Single* param);
			[CLSCompliant(false)]
			public unsafe static TexBumpParameterfvATI glTexBumpParameterfvATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexBumpParameterivATI(System.Graphics.OGL.AtiEnvmapBumpmap pname, Int32* param);
			[CLSCompliant(false)]
			public unsafe static TexBumpParameterivATI glTexBumpParameterivATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexCoord1d(Double s);
			[CLSCompliant(false)]
			public static TexCoord1d glTexCoord1d;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexCoord1dv(Double* v);
			[CLSCompliant(false)]
			public unsafe static TexCoord1dv glTexCoord1dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexCoord1f(Single s);
			[CLSCompliant(false)]
			public static TexCoord1f glTexCoord1f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexCoord1fv(Single* v);
			[CLSCompliant(false)]
			public unsafe static TexCoord1fv glTexCoord1fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexCoord1hNV(Half s);
			[CLSCompliant(false)]
			public static TexCoord1hNV glTexCoord1hNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexCoord1hvNV(Half* v);
			[CLSCompliant(false)]
			public unsafe static TexCoord1hvNV glTexCoord1hvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexCoord1i(Int32 s);
			[CLSCompliant(false)]
			public static TexCoord1i glTexCoord1i;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexCoord1iv(Int32* v);
			[CLSCompliant(false)]
			public unsafe static TexCoord1iv glTexCoord1iv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexCoord1s(Int16 s);
			[CLSCompliant(false)]
			public static TexCoord1s glTexCoord1s;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexCoord1sv(Int16* v);
			[CLSCompliant(false)]
			public unsafe static TexCoord1sv glTexCoord1sv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexCoord2d(Double s, Double t);
			[CLSCompliant(false)]
			public static TexCoord2d glTexCoord2d;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexCoord2dv(Double* v);
			[CLSCompliant(false)]
			public unsafe static TexCoord2dv glTexCoord2dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexCoord2f(Single s, Single t);
			[CLSCompliant(false)]
			public static TexCoord2f glTexCoord2f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexCoord2fColor3fVertex3fSUN(Single s, Single t, Single r, Single g, Single b, Single x, Single y, Single z);
			[CLSCompliant(false)]
			public static TexCoord2fColor3fVertex3fSUN glTexCoord2fColor3fVertex3fSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexCoord2fColor3fVertex3fvSUN(Single* tc, Single* c, Single* v);
			[CLSCompliant(false)]
			public unsafe static TexCoord2fColor3fVertex3fvSUN glTexCoord2fColor3fVertex3fvSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexCoord2fColor4fNormal3fVertex3fSUN(Single s, Single t, Single r, Single g, Single b, Single a, Single nx, Single ny, Single nz, Single x, Single y, Single z);
			[CLSCompliant(false)]
			public static TexCoord2fColor4fNormal3fVertex3fSUN glTexCoord2fColor4fNormal3fVertex3fSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexCoord2fColor4fNormal3fVertex3fvSUN(Single* tc, Single* c, Single* n, Single* v);
			[CLSCompliant(false)]
			public unsafe static TexCoord2fColor4fNormal3fVertex3fvSUN glTexCoord2fColor4fNormal3fVertex3fvSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexCoord2fColor4ubVertex3fSUN(Single s, Single t, Byte r, Byte g, Byte b, Byte a, Single x, Single y, Single z);
			[CLSCompliant(false)]
			public static TexCoord2fColor4ubVertex3fSUN glTexCoord2fColor4ubVertex3fSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexCoord2fColor4ubVertex3fvSUN(Single* tc, Byte* c, Single* v);
			[CLSCompliant(false)]
			public unsafe static TexCoord2fColor4ubVertex3fvSUN glTexCoord2fColor4ubVertex3fvSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexCoord2fNormal3fVertex3fSUN(Single s, Single t, Single nx, Single ny, Single nz, Single x, Single y, Single z);
			[CLSCompliant(false)]
			public static TexCoord2fNormal3fVertex3fSUN glTexCoord2fNormal3fVertex3fSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexCoord2fNormal3fVertex3fvSUN(Single* tc, Single* n, Single* v);
			[CLSCompliant(false)]
			public unsafe static TexCoord2fNormal3fVertex3fvSUN glTexCoord2fNormal3fVertex3fvSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexCoord2fv(Single* v);
			[CLSCompliant(false)]
			public unsafe static TexCoord2fv glTexCoord2fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexCoord2fVertex3fSUN(Single s, Single t, Single x, Single y, Single z);
			[CLSCompliant(false)]
			public static TexCoord2fVertex3fSUN glTexCoord2fVertex3fSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexCoord2fVertex3fvSUN(Single* tc, Single* v);
			[CLSCompliant(false)]
			public unsafe static TexCoord2fVertex3fvSUN glTexCoord2fVertex3fvSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexCoord2hNV(Half s, Half t);
			[CLSCompliant(false)]
			public static TexCoord2hNV glTexCoord2hNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexCoord2hvNV(Half* v);
			[CLSCompliant(false)]
			public unsafe static TexCoord2hvNV glTexCoord2hvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexCoord2i(Int32 s, Int32 t);
			[CLSCompliant(false)]
			public static TexCoord2i glTexCoord2i;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexCoord2iv(Int32* v);
			[CLSCompliant(false)]
			public unsafe static TexCoord2iv glTexCoord2iv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexCoord2s(Int16 s, Int16 t);
			[CLSCompliant(false)]
			public static TexCoord2s glTexCoord2s;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexCoord2sv(Int16* v);
			[CLSCompliant(false)]
			public unsafe static TexCoord2sv glTexCoord2sv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexCoord3d(Double s, Double t, Double r);
			[CLSCompliant(false)]
			public static TexCoord3d glTexCoord3d;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexCoord3dv(Double* v);
			[CLSCompliant(false)]
			public unsafe static TexCoord3dv glTexCoord3dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexCoord3f(Single s, Single t, Single r);
			[CLSCompliant(false)]
			public static TexCoord3f glTexCoord3f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexCoord3fv(Single* v);
			[CLSCompliant(false)]
			public unsafe static TexCoord3fv glTexCoord3fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexCoord3hNV(Half s, Half t, Half r);
			[CLSCompliant(false)]
			public static TexCoord3hNV glTexCoord3hNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexCoord3hvNV(Half* v);
			[CLSCompliant(false)]
			public unsafe static TexCoord3hvNV glTexCoord3hvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexCoord3i(Int32 s, Int32 t, Int32 r);
			[CLSCompliant(false)]
			public static TexCoord3i glTexCoord3i;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexCoord3iv(Int32* v);
			[CLSCompliant(false)]
			public unsafe static TexCoord3iv glTexCoord3iv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexCoord3s(Int16 s, Int16 t, Int16 r);
			[CLSCompliant(false)]
			public static TexCoord3s glTexCoord3s;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexCoord3sv(Int16* v);
			[CLSCompliant(false)]
			public unsafe static TexCoord3sv glTexCoord3sv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexCoord4d(Double s, Double t, Double r, Double q);
			[CLSCompliant(false)]
			public static TexCoord4d glTexCoord4d;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexCoord4dv(Double* v);
			[CLSCompliant(false)]
			public unsafe static TexCoord4dv glTexCoord4dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexCoord4f(Single s, Single t, Single r, Single q);
			[CLSCompliant(false)]
			public static TexCoord4f glTexCoord4f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexCoord4fColor4fNormal3fVertex4fSUN(Single s, Single t, Single p, Single q, Single r, Single g, Single b, Single a, Single nx, Single ny, Single nz, Single x, Single y, Single z, Single w);
			[CLSCompliant(false)]
			public static TexCoord4fColor4fNormal3fVertex4fSUN glTexCoord4fColor4fNormal3fVertex4fSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexCoord4fColor4fNormal3fVertex4fvSUN(Single* tc, Single* c, Single* n, Single* v);
			[CLSCompliant(false)]
			public unsafe static TexCoord4fColor4fNormal3fVertex4fvSUN glTexCoord4fColor4fNormal3fVertex4fvSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexCoord4fv(Single* v);
			[CLSCompliant(false)]
			public unsafe static TexCoord4fv glTexCoord4fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexCoord4fVertex4fSUN(Single s, Single t, Single p, Single q, Single x, Single y, Single z, Single w);
			[CLSCompliant(false)]
			public static TexCoord4fVertex4fSUN glTexCoord4fVertex4fSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexCoord4fVertex4fvSUN(Single* tc, Single* v);
			[CLSCompliant(false)]
			public unsafe static TexCoord4fVertex4fvSUN glTexCoord4fVertex4fvSUN;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexCoord4hNV(Half s, Half t, Half r, Half q);
			[CLSCompliant(false)]
			public static TexCoord4hNV glTexCoord4hNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexCoord4hvNV(Half* v);
			[CLSCompliant(false)]
			public unsafe static TexCoord4hvNV glTexCoord4hvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexCoord4i(Int32 s, Int32 t, Int32 r, Int32 q);
			[CLSCompliant(false)]
			public static TexCoord4i glTexCoord4i;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexCoord4iv(Int32* v);
			[CLSCompliant(false)]
			public unsafe static TexCoord4iv glTexCoord4iv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexCoord4s(Int16 s, Int16 t, Int16 r, Int16 q);
			[CLSCompliant(false)]
			public static TexCoord4s glTexCoord4s;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexCoord4sv(Int16* v);
			[CLSCompliant(false)]
			public unsafe static TexCoord4sv glTexCoord4sv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexCoordFormatNV(Int32 size, System.Graphics.OGL.NvVertexBufferUnifiedMemory type, Int32 stride);
			[CLSCompliant(false)]
			public static TexCoordFormatNV glTexCoordFormatNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexCoordP1ui(System.Graphics.OGL.PackedPointerType type, UInt32 coords);
			[CLSCompliant(false)]
			public static TexCoordP1ui glTexCoordP1ui;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexCoordP1uiv(System.Graphics.OGL.PackedPointerType type, UInt32* coords);
			[CLSCompliant(false)]
			public unsafe static TexCoordP1uiv glTexCoordP1uiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexCoordP2ui(System.Graphics.OGL.PackedPointerType type, UInt32 coords);
			[CLSCompliant(false)]
			public static TexCoordP2ui glTexCoordP2ui;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexCoordP2uiv(System.Graphics.OGL.PackedPointerType type, UInt32* coords);
			[CLSCompliant(false)]
			public unsafe static TexCoordP2uiv glTexCoordP2uiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexCoordP3ui(System.Graphics.OGL.PackedPointerType type, UInt32 coords);
			[CLSCompliant(false)]
			public static TexCoordP3ui glTexCoordP3ui;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexCoordP3uiv(System.Graphics.OGL.PackedPointerType type, UInt32* coords);
			[CLSCompliant(false)]
			public unsafe static TexCoordP3uiv glTexCoordP3uiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexCoordP4ui(System.Graphics.OGL.PackedPointerType type, UInt32 coords);
			[CLSCompliant(false)]
			public static TexCoordP4ui glTexCoordP4ui;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexCoordP4uiv(System.Graphics.OGL.PackedPointerType type, UInt32* coords);
			[CLSCompliant(false)]
			public unsafe static TexCoordP4uiv glTexCoordP4uiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexCoordPointer(Int32 size, System.Graphics.OGL.TexCoordPointerType type, Int32 stride, IntPtr pointer);
			[CLSCompliant(false)]
			public static TexCoordPointer glTexCoordPointer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexCoordPointerEXT(Int32 size, System.Graphics.OGL.TexCoordPointerType type, Int32 stride, Int32 count, IntPtr pointer);
			[CLSCompliant(false)]
			public static TexCoordPointerEXT glTexCoordPointerEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexCoordPointerListIBM(Int32 size, System.Graphics.OGL.TexCoordPointerType type, Int32 stride, IntPtr pointer, Int32 ptrstride);
			[CLSCompliant(false)]
			public static TexCoordPointerListIBM glTexCoordPointerListIBM;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexCoordPointervINTEL(Int32 size, System.Graphics.OGL.VertexPointerType type, IntPtr pointer);
			[CLSCompliant(false)]
			public static TexCoordPointervINTEL glTexCoordPointervINTEL;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexEnvf(System.Graphics.OGL.TextureEnvTarget target, System.Graphics.OGL.TextureEnvParameter pname, Single param);
			[CLSCompliant(false)]
			public static TexEnvf glTexEnvf;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexEnvfv(System.Graphics.OGL.TextureEnvTarget target, System.Graphics.OGL.TextureEnvParameter pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static TexEnvfv glTexEnvfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexEnvi(System.Graphics.OGL.TextureEnvTarget target, System.Graphics.OGL.TextureEnvParameter pname, Int32 param);
			[CLSCompliant(false)]
			public static TexEnvi glTexEnvi;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexEnviv(System.Graphics.OGL.TextureEnvTarget target, System.Graphics.OGL.TextureEnvParameter pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static TexEnviv glTexEnviv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexFilterFuncSGIS(System.Graphics.OGL.TextureTarget target, System.Graphics.OGL.SgisTextureFilter4 filter, Int32 n, Single* weights);
			[CLSCompliant(false)]
			public unsafe static TexFilterFuncSGIS glTexFilterFuncSGIS;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexGend(System.Graphics.OGL.TextureCoordName coord, System.Graphics.OGL.TextureGenParameter pname, Double param);
			[CLSCompliant(false)]
			public static TexGend glTexGend;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexGendv(System.Graphics.OGL.TextureCoordName coord, System.Graphics.OGL.TextureGenParameter pname, Double* @params);
			[CLSCompliant(false)]
			public unsafe static TexGendv glTexGendv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexGenf(System.Graphics.OGL.TextureCoordName coord, System.Graphics.OGL.TextureGenParameter pname, Single param);
			[CLSCompliant(false)]
			public static TexGenf glTexGenf;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexGenfv(System.Graphics.OGL.TextureCoordName coord, System.Graphics.OGL.TextureGenParameter pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static TexGenfv glTexGenfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexGeni(System.Graphics.OGL.TextureCoordName coord, System.Graphics.OGL.TextureGenParameter pname, Int32 param);
			[CLSCompliant(false)]
			public static TexGeni glTexGeni;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexGeniv(System.Graphics.OGL.TextureCoordName coord, System.Graphics.OGL.TextureGenParameter pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static TexGeniv glTexGeniv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexImage1D(System.Graphics.OGL.TextureTarget target, Int32 level, System.Graphics.OGL.PixelInternalFormat internalformat, Int32 width, Int32 border, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, IntPtr pixels);
			[CLSCompliant(false)]
			public static TexImage1D glTexImage1D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexImage2D(System.Graphics.OGL.TextureTarget target, Int32 level, System.Graphics.OGL.PixelInternalFormat internalformat, Int32 width, Int32 height, Int32 border, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, IntPtr pixels);
			[CLSCompliant(false)]
			public static TexImage2D glTexImage2D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexImage2DMultisample(System.Graphics.OGL.TextureTargetMultisample target, Int32 samples, System.Graphics.OGL.PixelInternalFormat internalformat, Int32 width, Int32 height, bool fixedsamplelocations);
			[CLSCompliant(false)]
			public static TexImage2DMultisample glTexImage2DMultisample;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexImage2DMultisampleCoverageNV(System.Graphics.OGL.NvTextureMultisample target, Int32 coverageSamples, Int32 colorSamples, Int32 internalFormat, Int32 width, Int32 height, bool fixedSampleLocations);
			[CLSCompliant(false)]
			public static TexImage2DMultisampleCoverageNV glTexImage2DMultisampleCoverageNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexImage3D(System.Graphics.OGL.TextureTarget target, Int32 level, System.Graphics.OGL.PixelInternalFormat internalformat, Int32 width, Int32 height, Int32 depth, Int32 border, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, IntPtr pixels);
			[CLSCompliant(false)]
			public static TexImage3D glTexImage3D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexImage3DEXT(System.Graphics.OGL.TextureTarget target, Int32 level, System.Graphics.OGL.PixelInternalFormat internalformat, Int32 width, Int32 height, Int32 depth, Int32 border, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, IntPtr pixels);
			[CLSCompliant(false)]
			public static TexImage3DEXT glTexImage3DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexImage3DMultisample(System.Graphics.OGL.TextureTargetMultisample target, Int32 samples, System.Graphics.OGL.PixelInternalFormat internalformat, Int32 width, Int32 height, Int32 depth, bool fixedsamplelocations);
			[CLSCompliant(false)]
			public static TexImage3DMultisample glTexImage3DMultisample;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexImage3DMultisampleCoverageNV(System.Graphics.OGL.NvTextureMultisample target, Int32 coverageSamples, Int32 colorSamples, Int32 internalFormat, Int32 width, Int32 height, Int32 depth, bool fixedSampleLocations);
			[CLSCompliant(false)]
			public static TexImage3DMultisampleCoverageNV glTexImage3DMultisampleCoverageNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexImage4DSGIS(System.Graphics.OGL.TextureTarget target, Int32 level, System.Graphics.OGL.PixelInternalFormat internalformat, Int32 width, Int32 height, Int32 depth, Int32 size4d, Int32 border, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, IntPtr pixels);
			[CLSCompliant(false)]
			public static TexImage4DSGIS glTexImage4DSGIS;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexParameterf(System.Graphics.OGL.TextureTarget target, System.Graphics.OGL.TextureParameterName pname, Single param);
			[CLSCompliant(false)]
			public static TexParameterf glTexParameterf;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexParameterfv(System.Graphics.OGL.TextureTarget target, System.Graphics.OGL.TextureParameterName pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static TexParameterfv glTexParameterfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexParameteri(System.Graphics.OGL.TextureTarget target, System.Graphics.OGL.TextureParameterName pname, Int32 param);
			[CLSCompliant(false)]
			public static TexParameteri glTexParameteri;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexParameterIiv(System.Graphics.OGL.TextureTarget target, System.Graphics.OGL.TextureParameterName pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static TexParameterIiv glTexParameterIiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexParameterIivEXT(System.Graphics.OGL.TextureTarget target, System.Graphics.OGL.TextureParameterName pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static TexParameterIivEXT glTexParameterIivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexParameterIuiv(System.Graphics.OGL.TextureTarget target, System.Graphics.OGL.TextureParameterName pname, UInt32* @params);
			[CLSCompliant(false)]
			public unsafe static TexParameterIuiv glTexParameterIuiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexParameterIuivEXT(System.Graphics.OGL.TextureTarget target, System.Graphics.OGL.TextureParameterName pname, UInt32* @params);
			[CLSCompliant(false)]
			public unsafe static TexParameterIuivEXT glTexParameterIuivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TexParameteriv(System.Graphics.OGL.TextureTarget target, System.Graphics.OGL.TextureParameterName pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static TexParameteriv glTexParameteriv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexRenderbufferNV(System.Graphics.OGL.TextureTarget target, UInt32 renderbuffer);
			[CLSCompliant(false)]
			public static TexRenderbufferNV glTexRenderbufferNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexSubImage1D(System.Graphics.OGL.TextureTarget target, Int32 level, Int32 xoffset, Int32 width, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, IntPtr pixels);
			[CLSCompliant(false)]
			public static TexSubImage1D glTexSubImage1D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexSubImage1DEXT(System.Graphics.OGL.TextureTarget target, Int32 level, Int32 xoffset, Int32 width, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, IntPtr pixels);
			[CLSCompliant(false)]
			public static TexSubImage1DEXT glTexSubImage1DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexSubImage2D(System.Graphics.OGL.TextureTarget target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 width, Int32 height, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, IntPtr pixels);
			[CLSCompliant(false)]
			public static TexSubImage2D glTexSubImage2D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexSubImage2DEXT(System.Graphics.OGL.TextureTarget target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 width, Int32 height, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, IntPtr pixels);
			[CLSCompliant(false)]
			public static TexSubImage2DEXT glTexSubImage2DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexSubImage3D(System.Graphics.OGL.TextureTarget target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 width, Int32 height, Int32 depth, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, IntPtr pixels);
			[CLSCompliant(false)]
			public static TexSubImage3D glTexSubImage3D;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexSubImage3DEXT(System.Graphics.OGL.TextureTarget target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 width, Int32 height, Int32 depth, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, IntPtr pixels);
			[CLSCompliant(false)]
			public static TexSubImage3DEXT glTexSubImage3DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TexSubImage4DSGIS(System.Graphics.OGL.TextureTarget target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 woffset, Int32 width, Int32 height, Int32 depth, Int32 size4d, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, IntPtr pixels);
			[CLSCompliant(false)]
			public static TexSubImage4DSGIS glTexSubImage4DSGIS;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TextureBarrierNV();
			[CLSCompliant(false)]
			public static TextureBarrierNV glTextureBarrierNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TextureBufferEXT(UInt32 texture, System.Graphics.OGL.TextureTarget target, System.Graphics.OGL.ExtDirectStateAccess internalformat, UInt32 buffer);
			[CLSCompliant(false)]
			public static TextureBufferEXT glTextureBufferEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TextureColorMaskSGIS(bool red, bool green, bool blue, bool alpha);
			[CLSCompliant(false)]
			public static TextureColorMaskSGIS glTextureColorMaskSGIS;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TextureImage1DEXT(UInt32 texture, System.Graphics.OGL.TextureTarget target, Int32 level, System.Graphics.OGL.ExtDirectStateAccess internalformat, Int32 width, Int32 border, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, IntPtr pixels);
			[CLSCompliant(false)]
			public static TextureImage1DEXT glTextureImage1DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TextureImage2DEXT(UInt32 texture, System.Graphics.OGL.TextureTarget target, Int32 level, System.Graphics.OGL.ExtDirectStateAccess internalformat, Int32 width, Int32 height, Int32 border, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, IntPtr pixels);
			[CLSCompliant(false)]
			public static TextureImage2DEXT glTextureImage2DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TextureImage2DMultisampleCoverageNV(UInt32 texture, System.Graphics.OGL.NvTextureMultisample target, Int32 coverageSamples, Int32 colorSamples, Int32 internalFormat, Int32 width, Int32 height, bool fixedSampleLocations);
			[CLSCompliant(false)]
			public static TextureImage2DMultisampleCoverageNV glTextureImage2DMultisampleCoverageNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TextureImage2DMultisampleNV(UInt32 texture, System.Graphics.OGL.NvTextureMultisample target, Int32 samples, Int32 internalFormat, Int32 width, Int32 height, bool fixedSampleLocations);
			[CLSCompliant(false)]
			public static TextureImage2DMultisampleNV glTextureImage2DMultisampleNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TextureImage3DEXT(UInt32 texture, System.Graphics.OGL.TextureTarget target, Int32 level, System.Graphics.OGL.ExtDirectStateAccess internalformat, Int32 width, Int32 height, Int32 depth, Int32 border, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, IntPtr pixels);
			[CLSCompliant(false)]
			public static TextureImage3DEXT glTextureImage3DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TextureImage3DMultisampleCoverageNV(UInt32 texture, System.Graphics.OGL.NvTextureMultisample target, Int32 coverageSamples, Int32 colorSamples, Int32 internalFormat, Int32 width, Int32 height, Int32 depth, bool fixedSampleLocations);
			[CLSCompliant(false)]
			public static TextureImage3DMultisampleCoverageNV glTextureImage3DMultisampleCoverageNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TextureImage3DMultisampleNV(UInt32 texture, System.Graphics.OGL.NvTextureMultisample target, Int32 samples, Int32 internalFormat, Int32 width, Int32 height, Int32 depth, bool fixedSampleLocations);
			[CLSCompliant(false)]
			public static TextureImage3DMultisampleNV glTextureImage3DMultisampleNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TextureLightEXT(System.Graphics.OGL.ExtLightTexture pname);
			[CLSCompliant(false)]
			public static TextureLightEXT glTextureLightEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TextureMaterialEXT(System.Graphics.OGL.MaterialFace face, System.Graphics.OGL.MaterialParameter mode);
			[CLSCompliant(false)]
			public static TextureMaterialEXT glTextureMaterialEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TextureNormalEXT(System.Graphics.OGL.ExtTexturePerturbNormal mode);
			[CLSCompliant(false)]
			public static TextureNormalEXT glTextureNormalEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TextureParameterfEXT(UInt32 texture, System.Graphics.OGL.TextureTarget target, System.Graphics.OGL.TextureParameterName pname, Single param);
			[CLSCompliant(false)]
			public static TextureParameterfEXT glTextureParameterfEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TextureParameterfvEXT(UInt32 texture, System.Graphics.OGL.TextureTarget target, System.Graphics.OGL.TextureParameterName pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static TextureParameterfvEXT glTextureParameterfvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TextureParameteriEXT(UInt32 texture, System.Graphics.OGL.TextureTarget target, System.Graphics.OGL.TextureParameterName pname, Int32 param);
			[CLSCompliant(false)]
			public static TextureParameteriEXT glTextureParameteriEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TextureParameterIivEXT(UInt32 texture, System.Graphics.OGL.TextureTarget target, System.Graphics.OGL.TextureParameterName pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static TextureParameterIivEXT glTextureParameterIivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TextureParameterIuivEXT(UInt32 texture, System.Graphics.OGL.TextureTarget target, System.Graphics.OGL.TextureParameterName pname, UInt32* @params);
			[CLSCompliant(false)]
			public unsafe static TextureParameterIuivEXT glTextureParameterIuivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TextureParameterivEXT(UInt32 texture, System.Graphics.OGL.TextureTarget target, System.Graphics.OGL.TextureParameterName pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static TextureParameterivEXT glTextureParameterivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TextureRangeAPPLE(System.Graphics.OGL.AppleTextureRange target, Int32 length, IntPtr pointer);
			[CLSCompliant(false)]
			public static TextureRangeAPPLE glTextureRangeAPPLE;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TextureRenderbufferEXT(UInt32 texture, System.Graphics.OGL.TextureTarget target, UInt32 renderbuffer);
			[CLSCompliant(false)]
			public static TextureRenderbufferEXT glTextureRenderbufferEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TextureSubImage1DEXT(UInt32 texture, System.Graphics.OGL.TextureTarget target, Int32 level, Int32 xoffset, Int32 width, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, IntPtr pixels);
			[CLSCompliant(false)]
			public static TextureSubImage1DEXT glTextureSubImage1DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TextureSubImage2DEXT(UInt32 texture, System.Graphics.OGL.TextureTarget target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 width, Int32 height, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, IntPtr pixels);
			[CLSCompliant(false)]
			public static TextureSubImage2DEXT glTextureSubImage2DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TextureSubImage3DEXT(UInt32 texture, System.Graphics.OGL.TextureTarget target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 width, Int32 height, Int32 depth, System.Graphics.OGL.TargetPixelFormat format, System.Graphics.OGL.PixelType type, IntPtr pixels);
			[CLSCompliant(false)]
			public static TextureSubImage3DEXT glTextureSubImage3DEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TrackMatrixNV(System.Graphics.OGL.AssemblyProgramTargetArb target, UInt32 address, System.Graphics.OGL.NvVertexProgram matrix, System.Graphics.OGL.NvVertexProgram transform);
			[CLSCompliant(false)]
			public static TrackMatrixNV glTrackMatrixNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TransformFeedbackAttribsNV(UInt32 count, Int32* attribs, System.Graphics.OGL.NvTransformFeedback bufferMode);
			[CLSCompliant(false)]
			public unsafe static TransformFeedbackAttribsNV glTransformFeedbackAttribsNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TransformFeedbackStreamAttribsNV(Int32 count, Int32* attribs, Int32 nbuffers, Int32* bufstreams, System.Graphics.OGL.NvTransformFeedback bufferMode);
			[CLSCompliant(false)]
			public unsafe static TransformFeedbackStreamAttribsNV glTransformFeedbackStreamAttribsNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TransformFeedbackVaryings(UInt32 program, Int32 count, String[] varyings, System.Graphics.OGL.TransformFeedbackMode bufferMode);
			[CLSCompliant(false)]
			public static TransformFeedbackVaryings glTransformFeedbackVaryings;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void TransformFeedbackVaryingsEXT(UInt32 program, Int32 count, String[] varyings, System.Graphics.OGL.ExtTransformFeedback bufferMode);
			[CLSCompliant(false)]
			public static TransformFeedbackVaryingsEXT glTransformFeedbackVaryingsEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void TransformFeedbackVaryingsNV(UInt32 program, Int32 count, Int32* locations, System.Graphics.OGL.NvTransformFeedback bufferMode);
			[CLSCompliant(false)]
			public unsafe static TransformFeedbackVaryingsNV glTransformFeedbackVaryingsNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Translated(Double x, Double y, Double z);
			[CLSCompliant(false)]
			public static Translated glTranslated;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Translatef(Single x, Single y, Single z);
			[CLSCompliant(false)]
			public static Translatef glTranslatef;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Uniform1d(Int32 location, Double x);
			[CLSCompliant(false)]
			public static Uniform1d glUniform1d;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform1dv(Int32 location, Int32 count, Double* value);
			[CLSCompliant(false)]
			public unsafe static Uniform1dv glUniform1dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Uniform1f(Int32 location, Single v0);
			[CLSCompliant(false)]
			public static Uniform1f glUniform1f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Uniform1fARB(Int32 location, Single v0);
			[CLSCompliant(false)]
			public static Uniform1fARB glUniform1fARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform1fv(Int32 location, Int32 count, Single* value);
			[CLSCompliant(false)]
			public unsafe static Uniform1fv glUniform1fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform1fvARB(Int32 location, Int32 count, Single* value);
			[CLSCompliant(false)]
			public unsafe static Uniform1fvARB glUniform1fvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Uniform1i(Int32 location, Int32 v0);
			[CLSCompliant(false)]
			public static Uniform1i glUniform1i;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Uniform1i64NV(Int32 location, Int64 x);
			[CLSCompliant(false)]
			public static Uniform1i64NV glUniform1i64NV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform1i64vNV(Int32 location, Int32 count, Int64* value);
			[CLSCompliant(false)]
			public unsafe static Uniform1i64vNV glUniform1i64vNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Uniform1iARB(Int32 location, Int32 v0);
			[CLSCompliant(false)]
			public static Uniform1iARB glUniform1iARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform1iv(Int32 location, Int32 count, Int32* value);
			[CLSCompliant(false)]
			public unsafe static Uniform1iv glUniform1iv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform1ivARB(Int32 location, Int32 count, Int32* value);
			[CLSCompliant(false)]
			public unsafe static Uniform1ivARB glUniform1ivARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Uniform1ui(Int32 location, UInt32 v0);
			[CLSCompliant(false)]
			public static Uniform1ui glUniform1ui;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Uniform1ui64NV(Int32 location, UInt64 x);
			[CLSCompliant(false)]
			public static Uniform1ui64NV glUniform1ui64NV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform1ui64vNV(Int32 location, Int32 count, UInt64* value);
			[CLSCompliant(false)]
			public unsafe static Uniform1ui64vNV glUniform1ui64vNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Uniform1uiEXT(Int32 location, UInt32 v0);
			[CLSCompliant(false)]
			public static Uniform1uiEXT glUniform1uiEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform1uiv(Int32 location, Int32 count, UInt32* value);
			[CLSCompliant(false)]
			public unsafe static Uniform1uiv glUniform1uiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform1uivEXT(Int32 location, Int32 count, UInt32* value);
			[CLSCompliant(false)]
			public unsafe static Uniform1uivEXT glUniform1uivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Uniform2d(Int32 location, Double x, Double y);
			[CLSCompliant(false)]
			public static Uniform2d glUniform2d;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform2dv(Int32 location, Int32 count, Double* value);
			[CLSCompliant(false)]
			public unsafe static Uniform2dv glUniform2dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Uniform2f(Int32 location, Single v0, Single v1);
			[CLSCompliant(false)]
			public static Uniform2f glUniform2f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Uniform2fARB(Int32 location, Single v0, Single v1);
			[CLSCompliant(false)]
			public static Uniform2fARB glUniform2fARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform2fv(Int32 location, Int32 count, Single* value);
			[CLSCompliant(false)]
			public unsafe static Uniform2fv glUniform2fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform2fvARB(Int32 location, Int32 count, Single* value);
			[CLSCompliant(false)]
			public unsafe static Uniform2fvARB glUniform2fvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Uniform2i(Int32 location, Int32 v0, Int32 v1);
			[CLSCompliant(false)]
			public static Uniform2i glUniform2i;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Uniform2i64NV(Int32 location, Int64 x, Int64 y);
			[CLSCompliant(false)]
			public static Uniform2i64NV glUniform2i64NV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform2i64vNV(Int32 location, Int32 count, Int64* value);
			[CLSCompliant(false)]
			public unsafe static Uniform2i64vNV glUniform2i64vNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Uniform2iARB(Int32 location, Int32 v0, Int32 v1);
			[CLSCompliant(false)]
			public static Uniform2iARB glUniform2iARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform2iv(Int32 location, Int32 count, Int32* value);
			[CLSCompliant(false)]
			public unsafe static Uniform2iv glUniform2iv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform2ivARB(Int32 location, Int32 count, Int32* value);
			[CLSCompliant(false)]
			public unsafe static Uniform2ivARB glUniform2ivARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Uniform2ui(Int32 location, UInt32 v0, UInt32 v1);
			[CLSCompliant(false)]
			public static Uniform2ui glUniform2ui;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Uniform2ui64NV(Int32 location, UInt64 x, UInt64 y);
			[CLSCompliant(false)]
			public static Uniform2ui64NV glUniform2ui64NV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform2ui64vNV(Int32 location, Int32 count, UInt64* value);
			[CLSCompliant(false)]
			public unsafe static Uniform2ui64vNV glUniform2ui64vNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Uniform2uiEXT(Int32 location, UInt32 v0, UInt32 v1);
			[CLSCompliant(false)]
			public static Uniform2uiEXT glUniform2uiEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform2uiv(Int32 location, Int32 count, UInt32* value);
			[CLSCompliant(false)]
			public unsafe static Uniform2uiv glUniform2uiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform2uivEXT(Int32 location, Int32 count, UInt32* value);
			[CLSCompliant(false)]
			public unsafe static Uniform2uivEXT glUniform2uivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Uniform3d(Int32 location, Double x, Double y, Double z);
			[CLSCompliant(false)]
			public static Uniform3d glUniform3d;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform3dv(Int32 location, Int32 count, Double* value);
			[CLSCompliant(false)]
			public unsafe static Uniform3dv glUniform3dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Uniform3f(Int32 location, Single v0, Single v1, Single v2);
			[CLSCompliant(false)]
			public static Uniform3f glUniform3f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Uniform3fARB(Int32 location, Single v0, Single v1, Single v2);
			[CLSCompliant(false)]
			public static Uniform3fARB glUniform3fARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform3fv(Int32 location, Int32 count, Single* value);
			[CLSCompliant(false)]
			public unsafe static Uniform3fv glUniform3fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform3fvARB(Int32 location, Int32 count, Single* value);
			[CLSCompliant(false)]
			public unsafe static Uniform3fvARB glUniform3fvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Uniform3i(Int32 location, Int32 v0, Int32 v1, Int32 v2);
			[CLSCompliant(false)]
			public static Uniform3i glUniform3i;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Uniform3i64NV(Int32 location, Int64 x, Int64 y, Int64 z);
			[CLSCompliant(false)]
			public static Uniform3i64NV glUniform3i64NV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform3i64vNV(Int32 location, Int32 count, Int64* value);
			[CLSCompliant(false)]
			public unsafe static Uniform3i64vNV glUniform3i64vNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Uniform3iARB(Int32 location, Int32 v0, Int32 v1, Int32 v2);
			[CLSCompliant(false)]
			public static Uniform3iARB glUniform3iARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform3iv(Int32 location, Int32 count, Int32* value);
			[CLSCompliant(false)]
			public unsafe static Uniform3iv glUniform3iv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform3ivARB(Int32 location, Int32 count, Int32* value);
			[CLSCompliant(false)]
			public unsafe static Uniform3ivARB glUniform3ivARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Uniform3ui(Int32 location, UInt32 v0, UInt32 v1, UInt32 v2);
			[CLSCompliant(false)]
			public static Uniform3ui glUniform3ui;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Uniform3ui64NV(Int32 location, UInt64 x, UInt64 y, UInt64 z);
			[CLSCompliant(false)]
			public static Uniform3ui64NV glUniform3ui64NV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform3ui64vNV(Int32 location, Int32 count, UInt64* value);
			[CLSCompliant(false)]
			public unsafe static Uniform3ui64vNV glUniform3ui64vNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Uniform3uiEXT(Int32 location, UInt32 v0, UInt32 v1, UInt32 v2);
			[CLSCompliant(false)]
			public static Uniform3uiEXT glUniform3uiEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform3uiv(Int32 location, Int32 count, UInt32* value);
			[CLSCompliant(false)]
			public unsafe static Uniform3uiv glUniform3uiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform3uivEXT(Int32 location, Int32 count, UInt32* value);
			[CLSCompliant(false)]
			public unsafe static Uniform3uivEXT glUniform3uivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Uniform4d(Int32 location, Double x, Double y, Double z, Double w);
			[CLSCompliant(false)]
			public static Uniform4d glUniform4d;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform4dv(Int32 location, Int32 count, Double* value);
			[CLSCompliant(false)]
			public unsafe static Uniform4dv glUniform4dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Uniform4f(Int32 location, Single v0, Single v1, Single v2, Single v3);
			[CLSCompliant(false)]
			public static Uniform4f glUniform4f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Uniform4fARB(Int32 location, Single v0, Single v1, Single v2, Single v3);
			[CLSCompliant(false)]
			public static Uniform4fARB glUniform4fARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform4fv(Int32 location, Int32 count, Single* value);
			[CLSCompliant(false)]
			public unsafe static Uniform4fv glUniform4fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform4fvARB(Int32 location, Int32 count, Single* value);
			[CLSCompliant(false)]
			public unsafe static Uniform4fvARB glUniform4fvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Uniform4i(Int32 location, Int32 v0, Int32 v1, Int32 v2, Int32 v3);
			[CLSCompliant(false)]
			public static Uniform4i glUniform4i;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Uniform4i64NV(Int32 location, Int64 x, Int64 y, Int64 z, Int64 w);
			[CLSCompliant(false)]
			public static Uniform4i64NV glUniform4i64NV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform4i64vNV(Int32 location, Int32 count, Int64* value);
			[CLSCompliant(false)]
			public unsafe static Uniform4i64vNV glUniform4i64vNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Uniform4iARB(Int32 location, Int32 v0, Int32 v1, Int32 v2, Int32 v3);
			[CLSCompliant(false)]
			public static Uniform4iARB glUniform4iARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform4iv(Int32 location, Int32 count, Int32* value);
			[CLSCompliant(false)]
			public unsafe static Uniform4iv glUniform4iv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform4ivARB(Int32 location, Int32 count, Int32* value);
			[CLSCompliant(false)]
			public unsafe static Uniform4ivARB glUniform4ivARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Uniform4ui(Int32 location, UInt32 v0, UInt32 v1, UInt32 v2, UInt32 v3);
			[CLSCompliant(false)]
			public static Uniform4ui glUniform4ui;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Uniform4ui64NV(Int32 location, UInt64 x, UInt64 y, UInt64 z, UInt64 w);
			[CLSCompliant(false)]
			public static Uniform4ui64NV glUniform4ui64NV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform4ui64vNV(Int32 location, Int32 count, UInt64* value);
			[CLSCompliant(false)]
			public unsafe static Uniform4ui64vNV glUniform4ui64vNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Uniform4uiEXT(Int32 location, UInt32 v0, UInt32 v1, UInt32 v2, UInt32 v3);
			[CLSCompliant(false)]
			public static Uniform4uiEXT glUniform4uiEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform4uiv(Int32 location, Int32 count, UInt32* value);
			[CLSCompliant(false)]
			public unsafe static Uniform4uiv glUniform4uiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniform4uivEXT(Int32 location, Int32 count, UInt32* value);
			[CLSCompliant(false)]
			public unsafe static Uniform4uivEXT glUniform4uivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void UniformBlockBinding(UInt32 program, UInt32 uniformBlockIndex, UInt32 uniformBlockBinding);
			[CLSCompliant(false)]
			public static UniformBlockBinding glUniformBlockBinding;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void UniformBufferEXT(UInt32 program, Int32 location, UInt32 buffer);
			[CLSCompliant(false)]
			public static UniformBufferEXT glUniformBufferEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void UniformMatrix2dv(Int32 location, Int32 count, bool transpose, Double* value);
			[CLSCompliant(false)]
			public unsafe static UniformMatrix2dv glUniformMatrix2dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void UniformMatrix2fv(Int32 location, Int32 count, bool transpose, Single* value);
			[CLSCompliant(false)]
			public unsafe static UniformMatrix2fv glUniformMatrix2fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void UniformMatrix2fvARB(Int32 location, Int32 count, bool transpose, Single* value);
			[CLSCompliant(false)]
			public unsafe static UniformMatrix2fvARB glUniformMatrix2fvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void UniformMatrix2x3dv(Int32 location, Int32 count, bool transpose, Double* value);
			[CLSCompliant(false)]
			public unsafe static UniformMatrix2x3dv glUniformMatrix2x3dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void UniformMatrix2x3fv(Int32 location, Int32 count, bool transpose, Single* value);
			[CLSCompliant(false)]
			public unsafe static UniformMatrix2x3fv glUniformMatrix2x3fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void UniformMatrix2x4dv(Int32 location, Int32 count, bool transpose, Double* value);
			[CLSCompliant(false)]
			public unsafe static UniformMatrix2x4dv glUniformMatrix2x4dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void UniformMatrix2x4fv(Int32 location, Int32 count, bool transpose, Single* value);
			[CLSCompliant(false)]
			public unsafe static UniformMatrix2x4fv glUniformMatrix2x4fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void UniformMatrix3dv(Int32 location, Int32 count, bool transpose, Double* value);
			[CLSCompliant(false)]
			public unsafe static UniformMatrix3dv glUniformMatrix3dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void UniformMatrix3fv(Int32 location, Int32 count, bool transpose, Single* value);
			[CLSCompliant(false)]
			public unsafe static UniformMatrix3fv glUniformMatrix3fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void UniformMatrix3fvARB(Int32 location, Int32 count, bool transpose, Single* value);
			[CLSCompliant(false)]
			public unsafe static UniformMatrix3fvARB glUniformMatrix3fvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void UniformMatrix3x2dv(Int32 location, Int32 count, bool transpose, Double* value);
			[CLSCompliant(false)]
			public unsafe static UniformMatrix3x2dv glUniformMatrix3x2dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void UniformMatrix3x2fv(Int32 location, Int32 count, bool transpose, Single* value);
			[CLSCompliant(false)]
			public unsafe static UniformMatrix3x2fv glUniformMatrix3x2fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void UniformMatrix3x4dv(Int32 location, Int32 count, bool transpose, Double* value);
			[CLSCompliant(false)]
			public unsafe static UniformMatrix3x4dv glUniformMatrix3x4dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void UniformMatrix3x4fv(Int32 location, Int32 count, bool transpose, Single* value);
			[CLSCompliant(false)]
			public unsafe static UniformMatrix3x4fv glUniformMatrix3x4fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void UniformMatrix4dv(Int32 location, Int32 count, bool transpose, Double* value);
			[CLSCompliant(false)]
			public unsafe static UniformMatrix4dv glUniformMatrix4dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void UniformMatrix4fv(Int32 location, Int32 count, bool transpose, Single* value);
			[CLSCompliant(false)]
			public unsafe static UniformMatrix4fv glUniformMatrix4fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void UniformMatrix4fvARB(Int32 location, Int32 count, bool transpose, Single* value);
			[CLSCompliant(false)]
			public unsafe static UniformMatrix4fvARB glUniformMatrix4fvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void UniformMatrix4x2dv(Int32 location, Int32 count, bool transpose, Double* value);
			[CLSCompliant(false)]
			public unsafe static UniformMatrix4x2dv glUniformMatrix4x2dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void UniformMatrix4x2fv(Int32 location, Int32 count, bool transpose, Single* value);
			[CLSCompliant(false)]
			public unsafe static UniformMatrix4x2fv glUniformMatrix4x2fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void UniformMatrix4x3dv(Int32 location, Int32 count, bool transpose, Double* value);
			[CLSCompliant(false)]
			public unsafe static UniformMatrix4x3dv glUniformMatrix4x3dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void UniformMatrix4x3fv(Int32 location, Int32 count, bool transpose, Single* value);
			[CLSCompliant(false)]
			public unsafe static UniformMatrix4x3fv glUniformMatrix4x3fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void UniformSubroutinesuiv(System.Graphics.OGL.ShaderType shadertype, Int32 count, UInt32* indices);
			[CLSCompliant(false)]
			public unsafe static UniformSubroutinesuiv glUniformSubroutinesuiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Uniformui64NV(Int32 location, UInt64 value);
			[CLSCompliant(false)]
			public static Uniformui64NV glUniformui64NV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Uniformui64vNV(Int32 location, Int32 count, UInt64* value);
			[CLSCompliant(false)]
			public unsafe static Uniformui64vNV glUniformui64vNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void UnlockArraysEXT();
			[CLSCompliant(false)]
			public static UnlockArraysEXT glUnlockArraysEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool UnmapBuffer(System.Graphics.OGL.BufferTarget target);
			[CLSCompliant(false)]
			public static UnmapBuffer glUnmapBuffer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool UnmapBufferARB(System.Graphics.OGL.BufferTargetArb target);
			[CLSCompliant(false)]
			public static UnmapBufferARB glUnmapBufferARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate bool UnmapNamedBufferEXT(UInt32 buffer);
			[CLSCompliant(false)]
			public static UnmapNamedBufferEXT glUnmapNamedBufferEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void UnmapObjectBufferATI(UInt32 buffer);
			[CLSCompliant(false)]
			public static UnmapObjectBufferATI glUnmapObjectBufferATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void UpdateObjectBufferATI(UInt32 buffer, UInt32 offset, Int32 size, IntPtr pointer, System.Graphics.OGL.AtiVertexArrayObject preserve);
			[CLSCompliant(false)]
			public static UpdateObjectBufferATI glUpdateObjectBufferATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void UseProgram(UInt32 program);
			[CLSCompliant(false)]
			public static UseProgram glUseProgram;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void UseProgramObjectARB(UInt32 programObj);
			[CLSCompliant(false)]
			public static UseProgramObjectARB glUseProgramObjectARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void UseProgramStages(UInt32 pipeline, System.Graphics.OGL.ProgramStageMask stages, UInt32 program);
			[CLSCompliant(false)]
			public static UseProgramStages glUseProgramStages;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void UseShaderProgramEXT(System.Graphics.OGL.ExtSeparateShaderObjects type, UInt32 program);
			[CLSCompliant(false)]
			public static UseShaderProgramEXT glUseShaderProgramEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ValidateProgram(UInt32 program);
			[CLSCompliant(false)]
			public static ValidateProgram glValidateProgram;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ValidateProgramARB(UInt32 programObj);
			[CLSCompliant(false)]
			public static ValidateProgramARB glValidateProgramARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ValidateProgramPipeline(UInt32 pipeline);
			[CLSCompliant(false)]
			public static ValidateProgramPipeline glValidateProgramPipeline;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VariantArrayObjectATI(UInt32 id, System.Graphics.OGL.AtiVertexArrayObject type, Int32 stride, UInt32 buffer, UInt32 offset);
			[CLSCompliant(false)]
			public static VariantArrayObjectATI glVariantArrayObjectATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VariantbvEXT(UInt32 id, SByte* addr);
			[CLSCompliant(false)]
			public unsafe static VariantbvEXT glVariantbvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VariantdvEXT(UInt32 id, Double* addr);
			[CLSCompliant(false)]
			public unsafe static VariantdvEXT glVariantdvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VariantfvEXT(UInt32 id, Single* addr);
			[CLSCompliant(false)]
			public unsafe static VariantfvEXT glVariantfvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VariantivEXT(UInt32 id, Int32* addr);
			[CLSCompliant(false)]
			public unsafe static VariantivEXT glVariantivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VariantPointerEXT(UInt32 id, System.Graphics.OGL.ExtVertexShader type, UInt32 stride, IntPtr addr);
			[CLSCompliant(false)]
			public static VariantPointerEXT glVariantPointerEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VariantsvEXT(UInt32 id, Int16* addr);
			[CLSCompliant(false)]
			public unsafe static VariantsvEXT glVariantsvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VariantubvEXT(UInt32 id, Byte* addr);
			[CLSCompliant(false)]
			public unsafe static VariantubvEXT glVariantubvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VariantuivEXT(UInt32 id, UInt32* addr);
			[CLSCompliant(false)]
			public unsafe static VariantuivEXT glVariantuivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VariantusvEXT(UInt32 id, UInt16* addr);
			[CLSCompliant(false)]
			public unsafe static VariantusvEXT glVariantusvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VDPAUFiniNV();
			[CLSCompliant(false)]
			public static VDPAUFiniNV glVDPAUFiniNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VDPAUGetSurfaceivNV(IntPtr surface, System.Graphics.OGL.NvVdpauInterop pname, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] Int32* values);
			[CLSCompliant(false)]
			public unsafe static VDPAUGetSurfaceivNV glVDPAUGetSurfaceivNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VDPAUInitNV(IntPtr vdpDevice, IntPtr getProcAddress);
			[CLSCompliant(false)]
			public static VDPAUInitNV glVDPAUInitNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VDPAUIsSurfaceNV(IntPtr surface);
			[CLSCompliant(false)]
			public static VDPAUIsSurfaceNV glVDPAUIsSurfaceNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VDPAUMapSurfacesNV(Int32 numSurfaces, IntPtr* surfaces);
			[CLSCompliant(false)]
			public unsafe static VDPAUMapSurfacesNV glVDPAUMapSurfacesNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate IntPtr VDPAURegisterOutputSurfaceNV([OutAttribute] IntPtr vdpSurface, System.Graphics.OGL.NvVdpauInterop target, Int32 numTextureNames, UInt32* textureNames);
			[CLSCompliant(false)]
			public unsafe static VDPAURegisterOutputSurfaceNV glVDPAURegisterOutputSurfaceNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate IntPtr VDPAURegisterVideoSurfaceNV([OutAttribute] IntPtr vdpSurface, System.Graphics.OGL.NvVdpauInterop target, Int32 numTextureNames, UInt32* textureNames);
			[CLSCompliant(false)]
			public unsafe static VDPAURegisterVideoSurfaceNV glVDPAURegisterVideoSurfaceNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VDPAUSurfaceAccessNV(IntPtr surface, System.Graphics.OGL.NvVdpauInterop access);
			[CLSCompliant(false)]
			public static VDPAUSurfaceAccessNV glVDPAUSurfaceAccessNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VDPAUUnmapSurfacesNV(Int32 numSurface, IntPtr* surfaces);
			[CLSCompliant(false)]
			public unsafe static VDPAUUnmapSurfacesNV glVDPAUUnmapSurfacesNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VDPAUUnregisterSurfaceNV(IntPtr surface);
			[CLSCompliant(false)]
			public static VDPAUUnregisterSurfaceNV glVDPAUUnregisterSurfaceNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Vertex2d(Double x, Double y);
			[CLSCompliant(false)]
			public static Vertex2d glVertex2d;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Vertex2dv(Double* v);
			[CLSCompliant(false)]
			public unsafe static Vertex2dv glVertex2dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Vertex2f(Single x, Single y);
			[CLSCompliant(false)]
			public static Vertex2f glVertex2f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Vertex2fv(Single* v);
			[CLSCompliant(false)]
			public unsafe static Vertex2fv glVertex2fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Vertex2hNV(Half x, Half y);
			[CLSCompliant(false)]
			public static Vertex2hNV glVertex2hNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Vertex2hvNV(Half* v);
			[CLSCompliant(false)]
			public unsafe static Vertex2hvNV glVertex2hvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Vertex2i(Int32 x, Int32 y);
			[CLSCompliant(false)]
			public static Vertex2i glVertex2i;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Vertex2iv(Int32* v);
			[CLSCompliant(false)]
			public unsafe static Vertex2iv glVertex2iv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Vertex2s(Int16 x, Int16 y);
			[CLSCompliant(false)]
			public static Vertex2s glVertex2s;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Vertex2sv(Int16* v);
			[CLSCompliant(false)]
			public unsafe static Vertex2sv glVertex2sv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Vertex3d(Double x, Double y, Double z);
			[CLSCompliant(false)]
			public static Vertex3d glVertex3d;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Vertex3dv(Double* v);
			[CLSCompliant(false)]
			public unsafe static Vertex3dv glVertex3dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Vertex3f(Single x, Single y, Single z);
			[CLSCompliant(false)]
			public static Vertex3f glVertex3f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Vertex3fv(Single* v);
			[CLSCompliant(false)]
			public unsafe static Vertex3fv glVertex3fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Vertex3hNV(Half x, Half y, Half z);
			[CLSCompliant(false)]
			public static Vertex3hNV glVertex3hNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Vertex3hvNV(Half* v);
			[CLSCompliant(false)]
			public unsafe static Vertex3hvNV glVertex3hvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Vertex3i(Int32 x, Int32 y, Int32 z);
			[CLSCompliant(false)]
			public static Vertex3i glVertex3i;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Vertex3iv(Int32* v);
			[CLSCompliant(false)]
			public unsafe static Vertex3iv glVertex3iv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Vertex3s(Int16 x, Int16 y, Int16 z);
			[CLSCompliant(false)]
			public static Vertex3s glVertex3s;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Vertex3sv(Int16* v);
			[CLSCompliant(false)]
			public unsafe static Vertex3sv glVertex3sv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Vertex4d(Double x, Double y, Double z, Double w);
			[CLSCompliant(false)]
			public static Vertex4d glVertex4d;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Vertex4dv(Double* v);
			[CLSCompliant(false)]
			public unsafe static Vertex4dv glVertex4dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Vertex4f(Single x, Single y, Single z, Single w);
			[CLSCompliant(false)]
			public static Vertex4f glVertex4f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Vertex4fv(Single* v);
			[CLSCompliant(false)]
			public unsafe static Vertex4fv glVertex4fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Vertex4hNV(Half x, Half y, Half z, Half w);
			[CLSCompliant(false)]
			public static Vertex4hNV glVertex4hNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Vertex4hvNV(Half* v);
			[CLSCompliant(false)]
			public unsafe static Vertex4hvNV glVertex4hvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Vertex4i(Int32 x, Int32 y, Int32 z, Int32 w);
			[CLSCompliant(false)]
			public static Vertex4i glVertex4i;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Vertex4iv(Int32* v);
			[CLSCompliant(false)]
			public unsafe static Vertex4iv glVertex4iv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Vertex4s(Int16 x, Int16 y, Int16 z, Int16 w);
			[CLSCompliant(false)]
			public static Vertex4s glVertex4s;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void Vertex4sv(Int16* v);
			[CLSCompliant(false)]
			public unsafe static Vertex4sv glVertex4sv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexArrayParameteriAPPLE(System.Graphics.OGL.AppleVertexArrayRange pname, Int32 param);
			[CLSCompliant(false)]
			public static VertexArrayParameteriAPPLE glVertexArrayParameteriAPPLE;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexArrayRangeAPPLE(Int32 length, [OutAttribute] IntPtr pointer);
			[CLSCompliant(false)]
			public static VertexArrayRangeAPPLE glVertexArrayRangeAPPLE;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexArrayRangeNV(Int32 length, IntPtr pointer);
			[CLSCompliant(false)]
			public static VertexArrayRangeNV glVertexArrayRangeNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexArrayVertexAttribLOffsetEXT(UInt32 vaobj, UInt32 buffer, UInt32 index, Int32 size, System.Graphics.OGL.ExtVertexAttrib64bit type, Int32 stride, IntPtr offset);
			[CLSCompliant(false)]
			public static VertexArrayVertexAttribLOffsetEXT glVertexArrayVertexAttribLOffsetEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib1d(UInt32 index, Double x);
			[CLSCompliant(false)]
			public static VertexAttrib1d glVertexAttrib1d;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib1dARB(UInt32 index, Double x);
			[CLSCompliant(false)]
			public static VertexAttrib1dARB glVertexAttrib1dARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib1dNV(UInt32 index, Double x);
			[CLSCompliant(false)]
			public static VertexAttrib1dNV glVertexAttrib1dNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib1dv(UInt32 index, Double* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib1dv glVertexAttrib1dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib1dvARB(UInt32 index, Double* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib1dvARB glVertexAttrib1dvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib1dvNV(UInt32 index, Double* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib1dvNV glVertexAttrib1dvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib1f(UInt32 index, Single x);
			[CLSCompliant(false)]
			public static VertexAttrib1f glVertexAttrib1f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib1fARB(UInt32 index, Single x);
			[CLSCompliant(false)]
			public static VertexAttrib1fARB glVertexAttrib1fARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib1fNV(UInt32 index, Single x);
			[CLSCompliant(false)]
			public static VertexAttrib1fNV glVertexAttrib1fNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib1fv(UInt32 index, Single* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib1fv glVertexAttrib1fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib1fvARB(UInt32 index, Single* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib1fvARB glVertexAttrib1fvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib1fvNV(UInt32 index, Single* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib1fvNV glVertexAttrib1fvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib1hNV(UInt32 index, Half x);
			[CLSCompliant(false)]
			public static VertexAttrib1hNV glVertexAttrib1hNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib1hvNV(UInt32 index, Half* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib1hvNV glVertexAttrib1hvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib1s(UInt32 index, Int16 x);
			[CLSCompliant(false)]
			public static VertexAttrib1s glVertexAttrib1s;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib1sARB(UInt32 index, Int16 x);
			[CLSCompliant(false)]
			public static VertexAttrib1sARB glVertexAttrib1sARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib1sNV(UInt32 index, Int16 x);
			[CLSCompliant(false)]
			public static VertexAttrib1sNV glVertexAttrib1sNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib1sv(UInt32 index, Int16* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib1sv glVertexAttrib1sv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib1svARB(UInt32 index, Int16* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib1svARB glVertexAttrib1svARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib1svNV(UInt32 index, Int16* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib1svNV glVertexAttrib1svNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib2d(UInt32 index, Double x, Double y);
			[CLSCompliant(false)]
			public static VertexAttrib2d glVertexAttrib2d;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib2dARB(UInt32 index, Double x, Double y);
			[CLSCompliant(false)]
			public static VertexAttrib2dARB glVertexAttrib2dARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib2dNV(UInt32 index, Double x, Double y);
			[CLSCompliant(false)]
			public static VertexAttrib2dNV glVertexAttrib2dNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib2dv(UInt32 index, Double* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib2dv glVertexAttrib2dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib2dvARB(UInt32 index, Double* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib2dvARB glVertexAttrib2dvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib2dvNV(UInt32 index, Double* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib2dvNV glVertexAttrib2dvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib2f(UInt32 index, Single x, Single y);
			[CLSCompliant(false)]
			public static VertexAttrib2f glVertexAttrib2f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib2fARB(UInt32 index, Single x, Single y);
			[CLSCompliant(false)]
			public static VertexAttrib2fARB glVertexAttrib2fARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib2fNV(UInt32 index, Single x, Single y);
			[CLSCompliant(false)]
			public static VertexAttrib2fNV glVertexAttrib2fNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib2fv(UInt32 index, Single* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib2fv glVertexAttrib2fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib2fvARB(UInt32 index, Single* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib2fvARB glVertexAttrib2fvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib2fvNV(UInt32 index, Single* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib2fvNV glVertexAttrib2fvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib2hNV(UInt32 index, Half x, Half y);
			[CLSCompliant(false)]
			public static VertexAttrib2hNV glVertexAttrib2hNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib2hvNV(UInt32 index, Half* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib2hvNV glVertexAttrib2hvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib2s(UInt32 index, Int16 x, Int16 y);
			[CLSCompliant(false)]
			public static VertexAttrib2s glVertexAttrib2s;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib2sARB(UInt32 index, Int16 x, Int16 y);
			[CLSCompliant(false)]
			public static VertexAttrib2sARB glVertexAttrib2sARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib2sNV(UInt32 index, Int16 x, Int16 y);
			[CLSCompliant(false)]
			public static VertexAttrib2sNV glVertexAttrib2sNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib2sv(UInt32 index, Int16* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib2sv glVertexAttrib2sv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib2svARB(UInt32 index, Int16* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib2svARB glVertexAttrib2svARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib2svNV(UInt32 index, Int16* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib2svNV glVertexAttrib2svNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib3d(UInt32 index, Double x, Double y, Double z);
			[CLSCompliant(false)]
			public static VertexAttrib3d glVertexAttrib3d;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib3dARB(UInt32 index, Double x, Double y, Double z);
			[CLSCompliant(false)]
			public static VertexAttrib3dARB glVertexAttrib3dARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib3dNV(UInt32 index, Double x, Double y, Double z);
			[CLSCompliant(false)]
			public static VertexAttrib3dNV glVertexAttrib3dNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib3dv(UInt32 index, Double* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib3dv glVertexAttrib3dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib3dvARB(UInt32 index, Double* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib3dvARB glVertexAttrib3dvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib3dvNV(UInt32 index, Double* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib3dvNV glVertexAttrib3dvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib3f(UInt32 index, Single x, Single y, Single z);
			[CLSCompliant(false)]
			public static VertexAttrib3f glVertexAttrib3f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib3fARB(UInt32 index, Single x, Single y, Single z);
			[CLSCompliant(false)]
			public static VertexAttrib3fARB glVertexAttrib3fARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib3fNV(UInt32 index, Single x, Single y, Single z);
			[CLSCompliant(false)]
			public static VertexAttrib3fNV glVertexAttrib3fNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib3fv(UInt32 index, Single* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib3fv glVertexAttrib3fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib3fvARB(UInt32 index, Single* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib3fvARB glVertexAttrib3fvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib3fvNV(UInt32 index, Single* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib3fvNV glVertexAttrib3fvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib3hNV(UInt32 index, Half x, Half y, Half z);
			[CLSCompliant(false)]
			public static VertexAttrib3hNV glVertexAttrib3hNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib3hvNV(UInt32 index, Half* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib3hvNV glVertexAttrib3hvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib3s(UInt32 index, Int16 x, Int16 y, Int16 z);
			[CLSCompliant(false)]
			public static VertexAttrib3s glVertexAttrib3s;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib3sARB(UInt32 index, Int16 x, Int16 y, Int16 z);
			[CLSCompliant(false)]
			public static VertexAttrib3sARB glVertexAttrib3sARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib3sNV(UInt32 index, Int16 x, Int16 y, Int16 z);
			[CLSCompliant(false)]
			public static VertexAttrib3sNV glVertexAttrib3sNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib3sv(UInt32 index, Int16* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib3sv glVertexAttrib3sv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib3svARB(UInt32 index, Int16* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib3svARB glVertexAttrib3svARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib3svNV(UInt32 index, Int16* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib3svNV glVertexAttrib3svNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib4bv(UInt32 index, SByte* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib4bv glVertexAttrib4bv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib4bvARB(UInt32 index, SByte* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib4bvARB glVertexAttrib4bvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib4d(UInt32 index, Double x, Double y, Double z, Double w);
			[CLSCompliant(false)]
			public static VertexAttrib4d glVertexAttrib4d;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib4dARB(UInt32 index, Double x, Double y, Double z, Double w);
			[CLSCompliant(false)]
			public static VertexAttrib4dARB glVertexAttrib4dARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib4dNV(UInt32 index, Double x, Double y, Double z, Double w);
			[CLSCompliant(false)]
			public static VertexAttrib4dNV glVertexAttrib4dNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib4dv(UInt32 index, Double* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib4dv glVertexAttrib4dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib4dvARB(UInt32 index, Double* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib4dvARB glVertexAttrib4dvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib4dvNV(UInt32 index, Double* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib4dvNV glVertexAttrib4dvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib4f(UInt32 index, Single x, Single y, Single z, Single w);
			[CLSCompliant(false)]
			public static VertexAttrib4f glVertexAttrib4f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib4fARB(UInt32 index, Single x, Single y, Single z, Single w);
			[CLSCompliant(false)]
			public static VertexAttrib4fARB glVertexAttrib4fARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib4fNV(UInt32 index, Single x, Single y, Single z, Single w);
			[CLSCompliant(false)]
			public static VertexAttrib4fNV glVertexAttrib4fNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib4fv(UInt32 index, Single* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib4fv glVertexAttrib4fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib4fvARB(UInt32 index, Single* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib4fvARB glVertexAttrib4fvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib4fvNV(UInt32 index, Single* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib4fvNV glVertexAttrib4fvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib4hNV(UInt32 index, Half x, Half y, Half z, Half w);
			[CLSCompliant(false)]
			public static VertexAttrib4hNV glVertexAttrib4hNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib4hvNV(UInt32 index, Half* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib4hvNV glVertexAttrib4hvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib4iv(UInt32 index, Int32* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib4iv glVertexAttrib4iv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib4ivARB(UInt32 index, Int32* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib4ivARB glVertexAttrib4ivARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib4Nbv(UInt32 index, SByte* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib4Nbv glVertexAttrib4Nbv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib4NbvARB(UInt32 index, SByte* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib4NbvARB glVertexAttrib4NbvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib4Niv(UInt32 index, Int32* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib4Niv glVertexAttrib4Niv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib4NivARB(UInt32 index, Int32* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib4NivARB glVertexAttrib4NivARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib4Nsv(UInt32 index, Int16* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib4Nsv glVertexAttrib4Nsv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib4NsvARB(UInt32 index, Int16* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib4NsvARB glVertexAttrib4NsvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib4Nub(UInt32 index, Byte x, Byte y, Byte z, Byte w);
			[CLSCompliant(false)]
			public static VertexAttrib4Nub glVertexAttrib4Nub;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib4NubARB(UInt32 index, Byte x, Byte y, Byte z, Byte w);
			[CLSCompliant(false)]
			public static VertexAttrib4NubARB glVertexAttrib4NubARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib4Nubv(UInt32 index, Byte* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib4Nubv glVertexAttrib4Nubv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib4NubvARB(UInt32 index, Byte* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib4NubvARB glVertexAttrib4NubvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib4Nuiv(UInt32 index, UInt32* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib4Nuiv glVertexAttrib4Nuiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib4NuivARB(UInt32 index, UInt32* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib4NuivARB glVertexAttrib4NuivARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib4Nusv(UInt32 index, UInt16* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib4Nusv glVertexAttrib4Nusv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib4NusvARB(UInt32 index, UInt16* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib4NusvARB glVertexAttrib4NusvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib4s(UInt32 index, Int16 x, Int16 y, Int16 z, Int16 w);
			[CLSCompliant(false)]
			public static VertexAttrib4s glVertexAttrib4s;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib4sARB(UInt32 index, Int16 x, Int16 y, Int16 z, Int16 w);
			[CLSCompliant(false)]
			public static VertexAttrib4sARB glVertexAttrib4sARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib4sNV(UInt32 index, Int16 x, Int16 y, Int16 z, Int16 w);
			[CLSCompliant(false)]
			public static VertexAttrib4sNV glVertexAttrib4sNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib4sv(UInt32 index, Int16* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib4sv glVertexAttrib4sv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib4svARB(UInt32 index, Int16* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib4svARB glVertexAttrib4svARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib4svNV(UInt32 index, Int16* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib4svNV glVertexAttrib4svNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttrib4ubNV(UInt32 index, Byte x, Byte y, Byte z, Byte w);
			[CLSCompliant(false)]
			public static VertexAttrib4ubNV glVertexAttrib4ubNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib4ubv(UInt32 index, Byte* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib4ubv glVertexAttrib4ubv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib4ubvARB(UInt32 index, Byte* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib4ubvARB glVertexAttrib4ubvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib4ubvNV(UInt32 index, Byte* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib4ubvNV glVertexAttrib4ubvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib4uiv(UInt32 index, UInt32* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib4uiv glVertexAttrib4uiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib4uivARB(UInt32 index, UInt32* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib4uivARB glVertexAttrib4uivARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib4usv(UInt32 index, UInt16* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib4usv glVertexAttrib4usv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttrib4usvARB(UInt32 index, UInt16* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttrib4usvARB glVertexAttrib4usvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribArrayObjectATI(UInt32 index, Int32 size, System.Graphics.OGL.AtiVertexAttribArrayObject type, bool normalized, Int32 stride, UInt32 buffer, UInt32 offset);
			[CLSCompliant(false)]
			public static VertexAttribArrayObjectATI glVertexAttribArrayObjectATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribDivisor(UInt32 index, UInt32 divisor);
			[CLSCompliant(false)]
			public static VertexAttribDivisor glVertexAttribDivisor;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribDivisorARB(UInt32 index, UInt32 divisor);
			[CLSCompliant(false)]
			public static VertexAttribDivisorARB glVertexAttribDivisorARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribFormatNV(UInt32 index, Int32 size, System.Graphics.OGL.NvVertexBufferUnifiedMemory type, bool normalized, Int32 stride);
			[CLSCompliant(false)]
			public static VertexAttribFormatNV glVertexAttribFormatNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribI1i(UInt32 index, Int32 x);
			[CLSCompliant(false)]
			public static VertexAttribI1i glVertexAttribI1i;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribI1iEXT(UInt32 index, Int32 x);
			[CLSCompliant(false)]
			public static VertexAttribI1iEXT glVertexAttribI1iEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribI1iv(UInt32 index, Int32* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribI1iv glVertexAttribI1iv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribI1ivEXT(UInt32 index, Int32* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribI1ivEXT glVertexAttribI1ivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribI1ui(UInt32 index, UInt32 x);
			[CLSCompliant(false)]
			public static VertexAttribI1ui glVertexAttribI1ui;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribI1uiEXT(UInt32 index, UInt32 x);
			[CLSCompliant(false)]
			public static VertexAttribI1uiEXT glVertexAttribI1uiEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribI1uiv(UInt32 index, UInt32* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribI1uiv glVertexAttribI1uiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribI1uivEXT(UInt32 index, UInt32* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribI1uivEXT glVertexAttribI1uivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribI2i(UInt32 index, Int32 x, Int32 y);
			[CLSCompliant(false)]
			public static VertexAttribI2i glVertexAttribI2i;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribI2iEXT(UInt32 index, Int32 x, Int32 y);
			[CLSCompliant(false)]
			public static VertexAttribI2iEXT glVertexAttribI2iEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribI2iv(UInt32 index, Int32* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribI2iv glVertexAttribI2iv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribI2ivEXT(UInt32 index, Int32* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribI2ivEXT glVertexAttribI2ivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribI2ui(UInt32 index, UInt32 x, UInt32 y);
			[CLSCompliant(false)]
			public static VertexAttribI2ui glVertexAttribI2ui;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribI2uiEXT(UInt32 index, UInt32 x, UInt32 y);
			[CLSCompliant(false)]
			public static VertexAttribI2uiEXT glVertexAttribI2uiEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribI2uiv(UInt32 index, UInt32* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribI2uiv glVertexAttribI2uiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribI2uivEXT(UInt32 index, UInt32* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribI2uivEXT glVertexAttribI2uivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribI3i(UInt32 index, Int32 x, Int32 y, Int32 z);
			[CLSCompliant(false)]
			public static VertexAttribI3i glVertexAttribI3i;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribI3iEXT(UInt32 index, Int32 x, Int32 y, Int32 z);
			[CLSCompliant(false)]
			public static VertexAttribI3iEXT glVertexAttribI3iEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribI3iv(UInt32 index, Int32* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribI3iv glVertexAttribI3iv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribI3ivEXT(UInt32 index, Int32* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribI3ivEXT glVertexAttribI3ivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribI3ui(UInt32 index, UInt32 x, UInt32 y, UInt32 z);
			[CLSCompliant(false)]
			public static VertexAttribI3ui glVertexAttribI3ui;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribI3uiEXT(UInt32 index, UInt32 x, UInt32 y, UInt32 z);
			[CLSCompliant(false)]
			public static VertexAttribI3uiEXT glVertexAttribI3uiEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribI3uiv(UInt32 index, UInt32* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribI3uiv glVertexAttribI3uiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribI3uivEXT(UInt32 index, UInt32* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribI3uivEXT glVertexAttribI3uivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribI4bv(UInt32 index, SByte* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribI4bv glVertexAttribI4bv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribI4bvEXT(UInt32 index, SByte* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribI4bvEXT glVertexAttribI4bvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribI4i(UInt32 index, Int32 x, Int32 y, Int32 z, Int32 w);
			[CLSCompliant(false)]
			public static VertexAttribI4i glVertexAttribI4i;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribI4iEXT(UInt32 index, Int32 x, Int32 y, Int32 z, Int32 w);
			[CLSCompliant(false)]
			public static VertexAttribI4iEXT glVertexAttribI4iEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribI4iv(UInt32 index, Int32* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribI4iv glVertexAttribI4iv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribI4ivEXT(UInt32 index, Int32* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribI4ivEXT glVertexAttribI4ivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribI4sv(UInt32 index, Int16* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribI4sv glVertexAttribI4sv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribI4svEXT(UInt32 index, Int16* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribI4svEXT glVertexAttribI4svEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribI4ubv(UInt32 index, Byte* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribI4ubv glVertexAttribI4ubv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribI4ubvEXT(UInt32 index, Byte* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribI4ubvEXT glVertexAttribI4ubvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribI4ui(UInt32 index, UInt32 x, UInt32 y, UInt32 z, UInt32 w);
			[CLSCompliant(false)]
			public static VertexAttribI4ui glVertexAttribI4ui;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribI4uiEXT(UInt32 index, UInt32 x, UInt32 y, UInt32 z, UInt32 w);
			[CLSCompliant(false)]
			public static VertexAttribI4uiEXT glVertexAttribI4uiEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribI4uiv(UInt32 index, UInt32* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribI4uiv glVertexAttribI4uiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribI4uivEXT(UInt32 index, UInt32* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribI4uivEXT glVertexAttribI4uivEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribI4usv(UInt32 index, UInt16* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribI4usv glVertexAttribI4usv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribI4usvEXT(UInt32 index, UInt16* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribI4usvEXT glVertexAttribI4usvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribIFormatNV(UInt32 index, Int32 size, System.Graphics.OGL.NvVertexBufferUnifiedMemory type, Int32 stride);
			[CLSCompliant(false)]
			public static VertexAttribIFormatNV glVertexAttribIFormatNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribIPointer(UInt32 index, Int32 size, System.Graphics.OGL.VertexAttribIPointerType type, Int32 stride, IntPtr pointer);
			[CLSCompliant(false)]
			public static VertexAttribIPointer glVertexAttribIPointer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribIPointerEXT(UInt32 index, Int32 size, System.Graphics.OGL.NvVertexProgram4 type, Int32 stride, IntPtr pointer);
			[CLSCompliant(false)]
			public static VertexAttribIPointerEXT glVertexAttribIPointerEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribL1d(UInt32 index, Double x);
			[CLSCompliant(false)]
			public static VertexAttribL1d glVertexAttribL1d;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribL1dEXT(UInt32 index, Double x);
			[CLSCompliant(false)]
			public static VertexAttribL1dEXT glVertexAttribL1dEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribL1dv(UInt32 index, Double* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribL1dv glVertexAttribL1dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribL1dvEXT(UInt32 index, Double* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribL1dvEXT glVertexAttribL1dvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribL1i64NV(UInt32 index, Int64 x);
			[CLSCompliant(false)]
			public static VertexAttribL1i64NV glVertexAttribL1i64NV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribL1i64vNV(UInt32 index, Int64* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribL1i64vNV glVertexAttribL1i64vNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribL1ui64NV(UInt32 index, UInt64 x);
			[CLSCompliant(false)]
			public static VertexAttribL1ui64NV glVertexAttribL1ui64NV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribL1ui64vNV(UInt32 index, UInt64* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribL1ui64vNV glVertexAttribL1ui64vNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribL2d(UInt32 index, Double x, Double y);
			[CLSCompliant(false)]
			public static VertexAttribL2d glVertexAttribL2d;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribL2dEXT(UInt32 index, Double x, Double y);
			[CLSCompliant(false)]
			public static VertexAttribL2dEXT glVertexAttribL2dEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribL2dv(UInt32 index, Double* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribL2dv glVertexAttribL2dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribL2dvEXT(UInt32 index, Double* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribL2dvEXT glVertexAttribL2dvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribL2i64NV(UInt32 index, Int64 x, Int64 y);
			[CLSCompliant(false)]
			public static VertexAttribL2i64NV glVertexAttribL2i64NV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribL2i64vNV(UInt32 index, Int64* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribL2i64vNV glVertexAttribL2i64vNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribL2ui64NV(UInt32 index, UInt64 x, UInt64 y);
			[CLSCompliant(false)]
			public static VertexAttribL2ui64NV glVertexAttribL2ui64NV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribL2ui64vNV(UInt32 index, UInt64* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribL2ui64vNV glVertexAttribL2ui64vNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribL3d(UInt32 index, Double x, Double y, Double z);
			[CLSCompliant(false)]
			public static VertexAttribL3d glVertexAttribL3d;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribL3dEXT(UInt32 index, Double x, Double y, Double z);
			[CLSCompliant(false)]
			public static VertexAttribL3dEXT glVertexAttribL3dEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribL3dv(UInt32 index, Double* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribL3dv glVertexAttribL3dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribL3dvEXT(UInt32 index, Double* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribL3dvEXT glVertexAttribL3dvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribL3i64NV(UInt32 index, Int64 x, Int64 y, Int64 z);
			[CLSCompliant(false)]
			public static VertexAttribL3i64NV glVertexAttribL3i64NV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribL3i64vNV(UInt32 index, Int64* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribL3i64vNV glVertexAttribL3i64vNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribL3ui64NV(UInt32 index, UInt64 x, UInt64 y, UInt64 z);
			[CLSCompliant(false)]
			public static VertexAttribL3ui64NV glVertexAttribL3ui64NV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribL3ui64vNV(UInt32 index, UInt64* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribL3ui64vNV glVertexAttribL3ui64vNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribL4d(UInt32 index, Double x, Double y, Double z, Double w);
			[CLSCompliant(false)]
			public static VertexAttribL4d glVertexAttribL4d;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribL4dEXT(UInt32 index, Double x, Double y, Double z, Double w);
			[CLSCompliant(false)]
			public static VertexAttribL4dEXT glVertexAttribL4dEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribL4dv(UInt32 index, Double* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribL4dv glVertexAttribL4dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribL4dvEXT(UInt32 index, Double* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribL4dvEXT glVertexAttribL4dvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribL4i64NV(UInt32 index, Int64 x, Int64 y, Int64 z, Int64 w);
			[CLSCompliant(false)]
			public static VertexAttribL4i64NV glVertexAttribL4i64NV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribL4i64vNV(UInt32 index, Int64* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribL4i64vNV glVertexAttribL4i64vNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribL4ui64NV(UInt32 index, UInt64 x, UInt64 y, UInt64 z, UInt64 w);
			[CLSCompliant(false)]
			public static VertexAttribL4ui64NV glVertexAttribL4ui64NV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribL4ui64vNV(UInt32 index, UInt64* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribL4ui64vNV glVertexAttribL4ui64vNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribLFormatNV(UInt32 index, Int32 size, System.Graphics.OGL.NvVertexAttribInteger64bit type, Int32 stride);
			[CLSCompliant(false)]
			public static VertexAttribLFormatNV glVertexAttribLFormatNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribLPointer(UInt32 index, Int32 size, System.Graphics.OGL.VertexAttribDPointerType type, Int32 stride, IntPtr pointer);
			[CLSCompliant(false)]
			public static VertexAttribLPointer glVertexAttribLPointer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribLPointerEXT(UInt32 index, Int32 size, System.Graphics.OGL.ExtVertexAttrib64bit type, Int32 stride, IntPtr pointer);
			[CLSCompliant(false)]
			public static VertexAttribLPointerEXT glVertexAttribLPointerEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribP1ui(UInt32 index, System.Graphics.OGL.PackedPointerType type, bool normalized, UInt32 value);
			[CLSCompliant(false)]
			public static VertexAttribP1ui glVertexAttribP1ui;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribP1uiv(UInt32 index, System.Graphics.OGL.PackedPointerType type, bool normalized, UInt32* value);
			[CLSCompliant(false)]
			public unsafe static VertexAttribP1uiv glVertexAttribP1uiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribP2ui(UInt32 index, System.Graphics.OGL.PackedPointerType type, bool normalized, UInt32 value);
			[CLSCompliant(false)]
			public static VertexAttribP2ui glVertexAttribP2ui;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribP2uiv(UInt32 index, System.Graphics.OGL.PackedPointerType type, bool normalized, UInt32* value);
			[CLSCompliant(false)]
			public unsafe static VertexAttribP2uiv glVertexAttribP2uiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribP3ui(UInt32 index, System.Graphics.OGL.PackedPointerType type, bool normalized, UInt32 value);
			[CLSCompliant(false)]
			public static VertexAttribP3ui glVertexAttribP3ui;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribP3uiv(UInt32 index, System.Graphics.OGL.PackedPointerType type, bool normalized, UInt32* value);
			[CLSCompliant(false)]
			public unsafe static VertexAttribP3uiv glVertexAttribP3uiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribP4ui(UInt32 index, System.Graphics.OGL.PackedPointerType type, bool normalized, UInt32 value);
			[CLSCompliant(false)]
			public static VertexAttribP4ui glVertexAttribP4ui;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribP4uiv(UInt32 index, System.Graphics.OGL.PackedPointerType type, bool normalized, UInt32* value);
			[CLSCompliant(false)]
			public unsafe static VertexAttribP4uiv glVertexAttribP4uiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribPointer(UInt32 index, Int32 size, System.Graphics.OGL.VertexAttribPointerType type, bool normalized, Int32 stride, IntPtr pointer);
			[CLSCompliant(false)]
			public static VertexAttribPointer glVertexAttribPointer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribPointerARB(UInt32 index, Int32 size, System.Graphics.OGL.VertexAttribPointerTypeArb type, bool normalized, Int32 stride, IntPtr pointer);
			[CLSCompliant(false)]
			public static VertexAttribPointerARB glVertexAttribPointerARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexAttribPointerNV(UInt32 index, Int32 fsize, System.Graphics.OGL.VertexAttribParameterArb type, Int32 stride, IntPtr pointer);
			[CLSCompliant(false)]
			public static VertexAttribPointerNV glVertexAttribPointerNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribs1dvNV(UInt32 index, Int32 count, Double* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribs1dvNV glVertexAttribs1dvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribs1fvNV(UInt32 index, Int32 count, Single* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribs1fvNV glVertexAttribs1fvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribs1hvNV(UInt32 index, Int32 n, Half* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribs1hvNV glVertexAttribs1hvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribs1svNV(UInt32 index, Int32 count, Int16* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribs1svNV glVertexAttribs1svNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribs2dvNV(UInt32 index, Int32 count, Double* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribs2dvNV glVertexAttribs2dvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribs2fvNV(UInt32 index, Int32 count, Single* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribs2fvNV glVertexAttribs2fvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribs2hvNV(UInt32 index, Int32 n, Half* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribs2hvNV glVertexAttribs2hvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribs2svNV(UInt32 index, Int32 count, Int16* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribs2svNV glVertexAttribs2svNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribs3dvNV(UInt32 index, Int32 count, Double* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribs3dvNV glVertexAttribs3dvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribs3fvNV(UInt32 index, Int32 count, Single* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribs3fvNV glVertexAttribs3fvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribs3hvNV(UInt32 index, Int32 n, Half* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribs3hvNV glVertexAttribs3hvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribs3svNV(UInt32 index, Int32 count, Int16* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribs3svNV glVertexAttribs3svNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribs4dvNV(UInt32 index, Int32 count, Double* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribs4dvNV glVertexAttribs4dvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribs4fvNV(UInt32 index, Int32 count, Single* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribs4fvNV glVertexAttribs4fvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribs4hvNV(UInt32 index, Int32 n, Half* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribs4hvNV glVertexAttribs4hvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribs4svNV(UInt32 index, Int32 count, Int16* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribs4svNV glVertexAttribs4svNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexAttribs4ubvNV(UInt32 index, Int32 count, Byte* v);
			[CLSCompliant(false)]
			public unsafe static VertexAttribs4ubvNV glVertexAttribs4ubvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexBlendARB(Int32 count);
			[CLSCompliant(false)]
			public static VertexBlendARB glVertexBlendARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexBlendEnvfATI(System.Graphics.OGL.AtiVertexStreams pname, Single param);
			[CLSCompliant(false)]
			public static VertexBlendEnvfATI glVertexBlendEnvfATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexBlendEnviATI(System.Graphics.OGL.AtiVertexStreams pname, Int32 param);
			[CLSCompliant(false)]
			public static VertexBlendEnviATI glVertexBlendEnviATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexFormatNV(Int32 size, System.Graphics.OGL.NvVertexBufferUnifiedMemory type, Int32 stride);
			[CLSCompliant(false)]
			public static VertexFormatNV glVertexFormatNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexP2ui(System.Graphics.OGL.PackedPointerType type, UInt32 value);
			[CLSCompliant(false)]
			public static VertexP2ui glVertexP2ui;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexP2uiv(System.Graphics.OGL.PackedPointerType type, UInt32* value);
			[CLSCompliant(false)]
			public unsafe static VertexP2uiv glVertexP2uiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexP3ui(System.Graphics.OGL.PackedPointerType type, UInt32 value);
			[CLSCompliant(false)]
			public static VertexP3ui glVertexP3ui;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexP3uiv(System.Graphics.OGL.PackedPointerType type, UInt32* value);
			[CLSCompliant(false)]
			public unsafe static VertexP3uiv glVertexP3uiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexP4ui(System.Graphics.OGL.PackedPointerType type, UInt32 value);
			[CLSCompliant(false)]
			public static VertexP4ui glVertexP4ui;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexP4uiv(System.Graphics.OGL.PackedPointerType type, UInt32* value);
			[CLSCompliant(false)]
			public unsafe static VertexP4uiv glVertexP4uiv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexPointer(Int32 size, System.Graphics.OGL.VertexPointerType type, Int32 stride, IntPtr pointer);
			[CLSCompliant(false)]
			public static VertexPointer glVertexPointer;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexPointerEXT(Int32 size, System.Graphics.OGL.VertexPointerType type, Int32 stride, Int32 count, IntPtr pointer);
			[CLSCompliant(false)]
			public static VertexPointerEXT glVertexPointerEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexPointerListIBM(Int32 size, System.Graphics.OGL.VertexPointerType type, Int32 stride, IntPtr pointer, Int32 ptrstride);
			[CLSCompliant(false)]
			public static VertexPointerListIBM glVertexPointerListIBM;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexPointervINTEL(Int32 size, System.Graphics.OGL.VertexPointerType type, IntPtr pointer);
			[CLSCompliant(false)]
			public static VertexPointervINTEL glVertexPointervINTEL;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexStream1dATI(System.Graphics.OGL.AtiVertexStreams stream, Double x);
			[CLSCompliant(false)]
			public static VertexStream1dATI glVertexStream1dATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexStream1dvATI(System.Graphics.OGL.AtiVertexStreams stream, Double* coords);
			[CLSCompliant(false)]
			public unsafe static VertexStream1dvATI glVertexStream1dvATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexStream1fATI(System.Graphics.OGL.AtiVertexStreams stream, Single x);
			[CLSCompliant(false)]
			public static VertexStream1fATI glVertexStream1fATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexStream1fvATI(System.Graphics.OGL.AtiVertexStreams stream, Single* coords);
			[CLSCompliant(false)]
			public unsafe static VertexStream1fvATI glVertexStream1fvATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexStream1iATI(System.Graphics.OGL.AtiVertexStreams stream, Int32 x);
			[CLSCompliant(false)]
			public static VertexStream1iATI glVertexStream1iATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexStream1ivATI(System.Graphics.OGL.AtiVertexStreams stream, Int32* coords);
			[CLSCompliant(false)]
			public unsafe static VertexStream1ivATI glVertexStream1ivATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexStream1sATI(System.Graphics.OGL.AtiVertexStreams stream, Int16 x);
			[CLSCompliant(false)]
			public static VertexStream1sATI glVertexStream1sATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexStream1svATI(System.Graphics.OGL.AtiVertexStreams stream, Int16* coords);
			[CLSCompliant(false)]
			public unsafe static VertexStream1svATI glVertexStream1svATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexStream2dATI(System.Graphics.OGL.AtiVertexStreams stream, Double x, Double y);
			[CLSCompliant(false)]
			public static VertexStream2dATI glVertexStream2dATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexStream2dvATI(System.Graphics.OGL.AtiVertexStreams stream, Double* coords);
			[CLSCompliant(false)]
			public unsafe static VertexStream2dvATI glVertexStream2dvATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexStream2fATI(System.Graphics.OGL.AtiVertexStreams stream, Single x, Single y);
			[CLSCompliant(false)]
			public static VertexStream2fATI glVertexStream2fATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexStream2fvATI(System.Graphics.OGL.AtiVertexStreams stream, Single* coords);
			[CLSCompliant(false)]
			public unsafe static VertexStream2fvATI glVertexStream2fvATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexStream2iATI(System.Graphics.OGL.AtiVertexStreams stream, Int32 x, Int32 y);
			[CLSCompliant(false)]
			public static VertexStream2iATI glVertexStream2iATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexStream2ivATI(System.Graphics.OGL.AtiVertexStreams stream, Int32* coords);
			[CLSCompliant(false)]
			public unsafe static VertexStream2ivATI glVertexStream2ivATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexStream2sATI(System.Graphics.OGL.AtiVertexStreams stream, Int16 x, Int16 y);
			[CLSCompliant(false)]
			public static VertexStream2sATI glVertexStream2sATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexStream2svATI(System.Graphics.OGL.AtiVertexStreams stream, Int16* coords);
			[CLSCompliant(false)]
			public unsafe static VertexStream2svATI glVertexStream2svATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexStream3dATI(System.Graphics.OGL.AtiVertexStreams stream, Double x, Double y, Double z);
			[CLSCompliant(false)]
			public static VertexStream3dATI glVertexStream3dATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexStream3dvATI(System.Graphics.OGL.AtiVertexStreams stream, Double* coords);
			[CLSCompliant(false)]
			public unsafe static VertexStream3dvATI glVertexStream3dvATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexStream3fATI(System.Graphics.OGL.AtiVertexStreams stream, Single x, Single y, Single z);
			[CLSCompliant(false)]
			public static VertexStream3fATI glVertexStream3fATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexStream3fvATI(System.Graphics.OGL.AtiVertexStreams stream, Single* coords);
			[CLSCompliant(false)]
			public unsafe static VertexStream3fvATI glVertexStream3fvATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexStream3iATI(System.Graphics.OGL.AtiVertexStreams stream, Int32 x, Int32 y, Int32 z);
			[CLSCompliant(false)]
			public static VertexStream3iATI glVertexStream3iATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexStream3ivATI(System.Graphics.OGL.AtiVertexStreams stream, Int32* coords);
			[CLSCompliant(false)]
			public unsafe static VertexStream3ivATI glVertexStream3ivATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexStream3sATI(System.Graphics.OGL.AtiVertexStreams stream, Int16 x, Int16 y, Int16 z);
			[CLSCompliant(false)]
			public static VertexStream3sATI glVertexStream3sATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexStream3svATI(System.Graphics.OGL.AtiVertexStreams stream, Int16* coords);
			[CLSCompliant(false)]
			public unsafe static VertexStream3svATI glVertexStream3svATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexStream4dATI(System.Graphics.OGL.AtiVertexStreams stream, Double x, Double y, Double z, Double w);
			[CLSCompliant(false)]
			public static VertexStream4dATI glVertexStream4dATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexStream4dvATI(System.Graphics.OGL.AtiVertexStreams stream, Double* coords);
			[CLSCompliant(false)]
			public unsafe static VertexStream4dvATI glVertexStream4dvATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexStream4fATI(System.Graphics.OGL.AtiVertexStreams stream, Single x, Single y, Single z, Single w);
			[CLSCompliant(false)]
			public static VertexStream4fATI glVertexStream4fATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexStream4fvATI(System.Graphics.OGL.AtiVertexStreams stream, Single* coords);
			[CLSCompliant(false)]
			public unsafe static VertexStream4fvATI glVertexStream4fvATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexStream4iATI(System.Graphics.OGL.AtiVertexStreams stream, Int32 x, Int32 y, Int32 z, Int32 w);
			[CLSCompliant(false)]
			public static VertexStream4iATI glVertexStream4iATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexStream4ivATI(System.Graphics.OGL.AtiVertexStreams stream, Int32* coords);
			[CLSCompliant(false)]
			public unsafe static VertexStream4ivATI glVertexStream4ivATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexStream4sATI(System.Graphics.OGL.AtiVertexStreams stream, Int16 x, Int16 y, Int16 z, Int16 w);
			[CLSCompliant(false)]
			public static VertexStream4sATI glVertexStream4sATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexStream4svATI(System.Graphics.OGL.AtiVertexStreams stream, Int16* coords);
			[CLSCompliant(false)]
			public unsafe static VertexStream4svATI glVertexStream4svATI;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexWeightfEXT(Single weight);
			[CLSCompliant(false)]
			public static VertexWeightfEXT glVertexWeightfEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexWeightfvEXT(Single* weight);
			[CLSCompliant(false)]
			public unsafe static VertexWeightfvEXT glVertexWeightfvEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexWeighthNV(Half weight);
			[CLSCompliant(false)]
			public static VertexWeighthNV glVertexWeighthNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VertexWeighthvNV(Half* weight);
			[CLSCompliant(false)]
			public unsafe static VertexWeighthvNV glVertexWeighthvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void VertexWeightPointerEXT(Int32 size, System.Graphics.OGL.ExtVertexWeighting type, Int32 stride, IntPtr pointer);
			[CLSCompliant(false)]
			public static VertexWeightPointerEXT glVertexWeightPointerEXT;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate System.Graphics.OGL.NvVideoCapture VideoCaptureNV(UInt32 video_capture_slot, [OutAttribute] UInt32* sequence_num, [OutAttribute] UInt64* capture_time);
			[CLSCompliant(false)]
			public unsafe static VideoCaptureNV glVideoCaptureNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VideoCaptureStreamParameterdvNV(UInt32 video_capture_slot, UInt32 stream, System.Graphics.OGL.NvVideoCapture pname, Double* @params);
			[CLSCompliant(false)]
			public unsafe static VideoCaptureStreamParameterdvNV glVideoCaptureStreamParameterdvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VideoCaptureStreamParameterfvNV(UInt32 video_capture_slot, UInt32 stream, System.Graphics.OGL.NvVideoCapture pname, Single* @params);
			[CLSCompliant(false)]
			public unsafe static VideoCaptureStreamParameterfvNV glVideoCaptureStreamParameterfvNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void VideoCaptureStreamParameterivNV(UInt32 video_capture_slot, UInt32 stream, System.Graphics.OGL.NvVideoCapture pname, Int32* @params);
			[CLSCompliant(false)]
			public unsafe static VideoCaptureStreamParameterivNV glVideoCaptureStreamParameterivNV;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void Viewport(Int32 x, Int32 y, Int32 width, Int32 height);
			[CLSCompliant(false)]
			public static Viewport glViewport;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ViewportArrayv(UInt32 first, Int32 count, Single* v);
			[CLSCompliant(false)]
			public unsafe static ViewportArrayv glViewportArrayv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void ViewportIndexedf(UInt32 index, Single x, Single y, Single w, Single h);
			[CLSCompliant(false)]
			public static ViewportIndexedf glViewportIndexedf;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void ViewportIndexedfv(UInt32 index, Single* v);
			[CLSCompliant(false)]
			public unsafe static ViewportIndexedfv glViewportIndexedfv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void WaitSync(IntPtr sync, UInt32 flags, UInt64 timeout);
			[CLSCompliant(false)]
			public static WaitSync glWaitSync;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void WeightbvARB(Int32 size, SByte* weights);
			[CLSCompliant(false)]
			public unsafe static WeightbvARB glWeightbvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void WeightdvARB(Int32 size, Double* weights);
			[CLSCompliant(false)]
			public unsafe static WeightdvARB glWeightdvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void WeightfvARB(Int32 size, Single* weights);
			[CLSCompliant(false)]
			public unsafe static WeightfvARB glWeightfvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void WeightivARB(Int32 size, Int32* weights);
			[CLSCompliant(false)]
			public unsafe static WeightivARB glWeightivARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void WeightPointerARB(Int32 size, System.Graphics.OGL.ArbVertexBlend type, Int32 stride, IntPtr pointer);
			[CLSCompliant(false)]
			public static WeightPointerARB glWeightPointerARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void WeightsvARB(Int32 size, Int16* weights);
			[CLSCompliant(false)]
			public unsafe static WeightsvARB glWeightsvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void WeightubvARB(Int32 size, Byte* weights);
			[CLSCompliant(false)]
			public unsafe static WeightubvARB glWeightubvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void WeightuivARB(Int32 size, UInt32* weights);
			[CLSCompliant(false)]
			public unsafe static WeightuivARB glWeightuivARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void WeightusvARB(Int32 size, UInt16* weights);
			[CLSCompliant(false)]
			public unsafe static WeightusvARB glWeightusvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void WindowPos2d(Double x, Double y);
			[CLSCompliant(false)]
			public static WindowPos2d glWindowPos2d;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void WindowPos2dARB(Double x, Double y);
			[CLSCompliant(false)]
			public static WindowPos2dARB glWindowPos2dARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void WindowPos2dMESA(Double x, Double y);
			[CLSCompliant(false)]
			public static WindowPos2dMESA glWindowPos2dMESA;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void WindowPos2dv(Double* v);
			[CLSCompliant(false)]
			public unsafe static WindowPos2dv glWindowPos2dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void WindowPos2dvARB(Double* v);
			[CLSCompliant(false)]
			public unsafe static WindowPos2dvARB glWindowPos2dvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void WindowPos2dvMESA(Double* v);
			[CLSCompliant(false)]
			public unsafe static WindowPos2dvMESA glWindowPos2dvMESA;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void WindowPos2f(Single x, Single y);
			[CLSCompliant(false)]
			public static WindowPos2f glWindowPos2f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void WindowPos2fARB(Single x, Single y);
			[CLSCompliant(false)]
			public static WindowPos2fARB glWindowPos2fARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void WindowPos2fMESA(Single x, Single y);
			[CLSCompliant(false)]
			public static WindowPos2fMESA glWindowPos2fMESA;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void WindowPos2fv(Single* v);
			[CLSCompliant(false)]
			public unsafe static WindowPos2fv glWindowPos2fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void WindowPos2fvARB(Single* v);
			[CLSCompliant(false)]
			public unsafe static WindowPos2fvARB glWindowPos2fvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void WindowPos2fvMESA(Single* v);
			[CLSCompliant(false)]
			public unsafe static WindowPos2fvMESA glWindowPos2fvMESA;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void WindowPos2i(Int32 x, Int32 y);
			[CLSCompliant(false)]
			public static WindowPos2i glWindowPos2i;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void WindowPos2iARB(Int32 x, Int32 y);
			[CLSCompliant(false)]
			public static WindowPos2iARB glWindowPos2iARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void WindowPos2iMESA(Int32 x, Int32 y);
			[CLSCompliant(false)]
			public static WindowPos2iMESA glWindowPos2iMESA;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void WindowPos2iv(Int32* v);
			[CLSCompliant(false)]
			public unsafe static WindowPos2iv glWindowPos2iv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void WindowPos2ivARB(Int32* v);
			[CLSCompliant(false)]
			public unsafe static WindowPos2ivARB glWindowPos2ivARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void WindowPos2ivMESA(Int32* v);
			[CLSCompliant(false)]
			public unsafe static WindowPos2ivMESA glWindowPos2ivMESA;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void WindowPos2s(Int16 x, Int16 y);
			[CLSCompliant(false)]
			public static WindowPos2s glWindowPos2s;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void WindowPos2sARB(Int16 x, Int16 y);
			[CLSCompliant(false)]
			public static WindowPos2sARB glWindowPos2sARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void WindowPos2sMESA(Int16 x, Int16 y);
			[CLSCompliant(false)]
			public static WindowPos2sMESA glWindowPos2sMESA;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void WindowPos2sv(Int16* v);
			[CLSCompliant(false)]
			public unsafe static WindowPos2sv glWindowPos2sv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void WindowPos2svARB(Int16* v);
			[CLSCompliant(false)]
			public unsafe static WindowPos2svARB glWindowPos2svARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void WindowPos2svMESA(Int16* v);
			[CLSCompliant(false)]
			public unsafe static WindowPos2svMESA glWindowPos2svMESA;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void WindowPos3d(Double x, Double y, Double z);
			[CLSCompliant(false)]
			public static WindowPos3d glWindowPos3d;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void WindowPos3dARB(Double x, Double y, Double z);
			[CLSCompliant(false)]
			public static WindowPos3dARB glWindowPos3dARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void WindowPos3dMESA(Double x, Double y, Double z);
			[CLSCompliant(false)]
			public static WindowPos3dMESA glWindowPos3dMESA;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void WindowPos3dv(Double* v);
			[CLSCompliant(false)]
			public unsafe static WindowPos3dv glWindowPos3dv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void WindowPos3dvARB(Double* v);
			[CLSCompliant(false)]
			public unsafe static WindowPos3dvARB glWindowPos3dvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void WindowPos3dvMESA(Double* v);
			[CLSCompliant(false)]
			public unsafe static WindowPos3dvMESA glWindowPos3dvMESA;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void WindowPos3f(Single x, Single y, Single z);
			[CLSCompliant(false)]
			public static WindowPos3f glWindowPos3f;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void WindowPos3fARB(Single x, Single y, Single z);
			[CLSCompliant(false)]
			public static WindowPos3fARB glWindowPos3fARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void WindowPos3fMESA(Single x, Single y, Single z);
			[CLSCompliant(false)]
			public static WindowPos3fMESA glWindowPos3fMESA;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void WindowPos3fv(Single* v);
			[CLSCompliant(false)]
			public unsafe static WindowPos3fv glWindowPos3fv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void WindowPos3fvARB(Single* v);
			[CLSCompliant(false)]
			public unsafe static WindowPos3fvARB glWindowPos3fvARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void WindowPos3fvMESA(Single* v);
			[CLSCompliant(false)]
			public unsafe static WindowPos3fvMESA glWindowPos3fvMESA;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void WindowPos3i(Int32 x, Int32 y, Int32 z);
			[CLSCompliant(false)]
			public static WindowPos3i glWindowPos3i;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void WindowPos3iARB(Int32 x, Int32 y, Int32 z);
			[CLSCompliant(false)]
			public static WindowPos3iARB glWindowPos3iARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void WindowPos3iMESA(Int32 x, Int32 y, Int32 z);
			[CLSCompliant(false)]
			public static WindowPos3iMESA glWindowPos3iMESA;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void WindowPos3iv(Int32* v);
			[CLSCompliant(false)]
			public unsafe static WindowPos3iv glWindowPos3iv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void WindowPos3ivARB(Int32* v);
			[CLSCompliant(false)]
			public unsafe static WindowPos3ivARB glWindowPos3ivARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void WindowPos3ivMESA(Int32* v);
			[CLSCompliant(false)]
			public unsafe static WindowPos3ivMESA glWindowPos3ivMESA;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void WindowPos3s(Int16 x, Int16 y, Int16 z);
			[CLSCompliant(false)]
			public static WindowPos3s glWindowPos3s;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void WindowPos3sARB(Int16 x, Int16 y, Int16 z);
			[CLSCompliant(false)]
			public static WindowPos3sARB glWindowPos3sARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void WindowPos3sMESA(Int16 x, Int16 y, Int16 z);
			[CLSCompliant(false)]
			public static WindowPos3sMESA glWindowPos3sMESA;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void WindowPos3sv(Int16* v);
			[CLSCompliant(false)]
			public unsafe static WindowPos3sv glWindowPos3sv;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void WindowPos3svARB(Int16* v);
			[CLSCompliant(false)]
			public unsafe static WindowPos3svARB glWindowPos3svARB;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void WindowPos3svMESA(Int16* v);
			[CLSCompliant(false)]
			public unsafe static WindowPos3svMESA glWindowPos3svMESA;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void WindowPos4dMESA(Double x, Double y, Double z, Double w);
			[CLSCompliant(false)]
			public static WindowPos4dMESA glWindowPos4dMESA;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void WindowPos4dvMESA(Double* v);
			[CLSCompliant(false)]
			public unsafe static WindowPos4dvMESA glWindowPos4dvMESA;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void WindowPos4fMESA(Single x, Single y, Single z, Single w);
			[CLSCompliant(false)]
			public static WindowPos4fMESA glWindowPos4fMESA;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void WindowPos4fvMESA(Single* v);
			[CLSCompliant(false)]
			public unsafe static WindowPos4fvMESA glWindowPos4fvMESA;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void WindowPos4iMESA(Int32 x, Int32 y, Int32 z, Int32 w);
			[CLSCompliant(false)]
			public static WindowPos4iMESA glWindowPos4iMESA;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void WindowPos4ivMESA(Int32* v);
			[CLSCompliant(false)]
			public unsafe static WindowPos4ivMESA glWindowPos4ivMESA;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void WindowPos4sMESA(Int16 x, Int16 y, Int16 z, Int16 w);
			[CLSCompliant(false)]
			public static WindowPos4sMESA glWindowPos4sMESA;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public unsafe delegate void WindowPos4svMESA(Int16* v);
			[CLSCompliant(false)]
			public unsafe static WindowPos4svMESA glWindowPos4svMESA;
			[System.Security.SuppressUnmanagedCodeSecurity()]
			[CLSCompliant(false)]
			public delegate void WriteMaskEXT(UInt32 res, UInt32 @in, System.Graphics.OGL.ExtVertexShader outX, System.Graphics.OGL.ExtVertexShader outY, System.Graphics.OGL.ExtVertexShader outZ, System.Graphics.OGL.ExtVertexShader outW);
			[CLSCompliant(false)]
			public static WriteMaskEXT glWriteMaskEXT;
		}
	}
}

#pragma warning restore 0649
#pragma warning restore 1591