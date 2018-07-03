//Ported from OpenTK, and excellent library.

#pragma warning disable 1572
#pragma warning disable 1573
#pragma warning disable 1591

using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Graphics.ES20 {
	/// <summary>
	/// An abstraction layer for all OpenGL|ES 2.0 functions.
	/// </summary>
	[Security.SuppressUnmanagedCodeSecurity]
	public static partial class GL {
		/// <summary>
		/// The name of the OpenGL|ES 2.0 library.
		/// </summary>
		public const string Library = "libGLESv2.dll";
		/// <summary>
		/// The native prefix of every GL function.
		/// </summary>
		public const string Prefix = "gl";

		// Note: Mono 1.9.1 truncates StringBuilder results (for 'out string' parameters).
		// We work around this issue by doubling the StringBuilder capacity.

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void ClearColor(Color color) {
			GL.ClearColor(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
		}

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void ClearColor(Color4 color) {
			GL.ClearColor(color.R, color.G, color.B, color.A);
		}





#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void BlendColor(Color color) {
			GL.BlendColor(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
		}

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void BlendColor(Color4 color) {
			GL.BlendColor(color.R, color.G, color.B, color.A);
		}





		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Uniform2(int location, ref Vector2 vector) {
			GL.Uniform2(location, vector.X, vector.Y);
		}

		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Uniform3(int location, ref Vector3 vector) {
			GL.Uniform3(location, vector.X, vector.Y, vector.Z);
		}

		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Uniform4(int location, ref Vector4 vector) {
			GL.Uniform4(location, vector.X, vector.Y, vector.Z, vector.W);
		}

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Uniform2(int location, Vector2 vector) {
			GL.Uniform2(location, vector.X, vector.Y);
		}

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Uniform3(int location, Vector3 vector) {
			GL.Uniform3(location, vector.X, vector.Y, vector.Z);
		}

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Uniform4(int location, Vector4 vector) {
			GL.Uniform4(location, vector.X, vector.Y, vector.Z, vector.W);
		}

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Uniform4(int location, Color4 color) {
			GL.Uniform4(location, color.R, color.G, color.B, color.A);
		}

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void UniformMatrix4(int location, bool transpose, ref Matrix4 matrix) {
			unsafe
			{
				fixed (float* matrix_ptr = &matrix.Row0.X) {
					GL.UniformMatrix4(location, 1, transpose, matrix_ptr);
				}
			}
		}

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static string GetActiveAttrib(int program, int index, out int size, out ActiveAttribType type) {
			int length;
			GetProgram(program, ProgramParameter.ActiveAttributeMaxLength, out length);
			StringBuilder sb = new StringBuilder(length == 0 ? 1 : length * 2);

			GetActiveAttrib(program, index, sb.Capacity, out length, out size, out type, sb);
			return sb.ToString();
		}





#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static string GetActiveUniform(int program, int uniformIndex, out int size, out ActiveUniformType type) {
			int length;
			GetProgram(program, ProgramParameter.ActiveUniformMaxLength, out length);

			StringBuilder sb = new StringBuilder(length == 0 ? 1 : length);
			GetActiveUniform(program, uniformIndex, sb.Capacity, out length, out size, out type, sb);
			return sb.ToString();
		}





#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void ShaderSource(Int32 shader, System.String @string) {
			unsafe
			{
				int length = @string.Length;
				GL.ShaderSource((UInt32) shader, 1, new string[] { @string }, &length);
			}
		}





#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static string GetShaderInfoLog(Int32 shader) {
			string info;
			GetShaderInfoLog(shader, out info);
			return info;
		}





#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void GetShaderInfoLog(Int32 shader, out string info) {
			unsafe
			{
				int length;
				GL.GetShader(shader, ShaderParameter.InfoLogLength, out length);
				if (length == 0) {
					info = String.Empty;
					return;
				}
				StringBuilder sb = new StringBuilder(length * 2);
				GL.GetShaderInfoLog((UInt32) shader, sb.Capacity, &length, sb);
				info = sb.ToString();
			}
		}





#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static string GetProgramInfoLog(Int32 program) {
			string info;
			GetProgramInfoLog(program, out info);
			return info;
		}





#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void GetProgramInfoLog(Int32 program, out string info) {
			unsafe
			{
				int length;
				GL.GetProgram(program, ProgramParameter.InfoLogLength, out length);
				if (length == 0) {
					info = String.Empty;
					return;
				}
				StringBuilder sb = new StringBuilder(length * 2);
				GL.GetProgramInfoLog((UInt32) program, sb.Capacity, &length, sb);
				info = sb.ToString();
			}
		}







		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void VertexAttrib2(Int32 index, ref Vector2 v) {
			GL.VertexAttrib2(index, v.X, v.Y);
		}





		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void VertexAttrib3(Int32 index, ref Vector3 v) {
			GL.VertexAttrib3(index, v.X, v.Y, v.Z);
		}





		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void VertexAttrib4(Int32 index, ref Vector4 v) {
			GL.VertexAttrib4(index, v.X, v.Y, v.Z, v.W);
		}





#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void VertexAttrib2(Int32 index, Vector2 v) {
			GL.VertexAttrib2(index, v.X, v.Y);
		}





#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void VertexAttrib3(Int32 index, Vector3 v) {
			GL.VertexAttrib3(index, v.X, v.Y, v.Z);
		}





#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void VertexAttrib4(Int32 index, Vector4 v) {
			GL.VertexAttrib4(index, v.X, v.Y, v.Z, v.W);
		}





#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void VertexAttribPointer(int index, int size, VertexAttribPointerType type, bool normalized, int stride, int offset) {
			VertexAttribPointer(index, size, type, normalized, stride, (IntPtr) offset);
		}

		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void VertexAttribPointer(uint index, int size, VertexAttribPointerType type, bool normalized, int stride, int offset) {
			VertexAttribPointer(index, size, type, normalized, stride, (IntPtr) offset);
		}





#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void DrawElements(BeginMode mode, int count, DrawElementsType type, int offset) {
			DrawElements(mode, count, type, new IntPtr(offset));
		}





#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static int GenTexture() {
			int id;
			GenTextures(1, out id);
			return id;
		}





#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void DeleteTexture(int id) {
			DeleteTextures(1, ref id);
		}





#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void GetFloat(GetPName pname, out Vector2 vector) {
			unsafe
			{
				fixed (Vector2* ptr = &vector)
					GetFloat(pname, (float*) ptr);
			}
		}

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void GetFloat(GetPName pname, out Vector3 vector) {
			unsafe
			{
				fixed (Vector3* ptr = &vector)
					GetFloat(pname, (float*) ptr);
			}
		}

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void GetFloat(GetPName pname, out Vector4 vector) {
			unsafe
			{
				fixed (Vector4* ptr = &vector)
					GetFloat(pname, (float*) ptr);
			}
		}

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void GetFloat(GetPName pname, out Matrix4 matrix) {
			unsafe
			{
				fixed (Matrix4* ptr = &matrix)
					GetFloat(pname, (float*) ptr);
			}
		}





#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Viewport(Size size) {
			GL.Viewport(0, 0, size.Width, size.Height);
		}

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Viewport(Point location, Size size) {
			GL.Viewport(location.X, location.Y, size.Width, size.Height);
		}

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Viewport(Rectangle rectangle) {
			GL.Viewport(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
		}

		/// <summary>
		/// AMD extensions
		/// </summary>
		public static class Amd {
			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glBeginPerfMonitorAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void BeginPerfMonitor(Int32 monitor) {
				Delegates.glBeginPerfMonitorAMD((UInt32) monitor);
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glBeginPerfMonitorAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void BeginPerfMonitor(UInt32 monitor) {
				Delegates.glBeginPerfMonitorAMD((UInt32) monitor);
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeletePerfMonitorsAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void DeletePerfMonitors(Int32 n, Int32[] monitors) {
				unsafe
				{
					fixed (Int32* monitors_ptr = monitors) {
						Delegates.glDeletePerfMonitorsAMD((Int32) n, (UInt32*) monitors_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeletePerfMonitorsAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void DeletePerfMonitors(Int32 n, ref Int32 monitors) {
				unsafe
				{
					fixed (Int32* monitors_ptr = &monitors) {
						Delegates.glDeletePerfMonitorsAMD((Int32) n, (UInt32*) monitors_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeletePerfMonitorsAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void DeletePerfMonitors(Int32 n, Int32* monitors) {
				Delegates.glDeletePerfMonitorsAMD((Int32) n, (UInt32*) monitors);
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeletePerfMonitorsAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void DeletePerfMonitors(Int32 n, UInt32[] monitors) {
				unsafe
				{
					fixed (UInt32* monitors_ptr = monitors) {
						Delegates.glDeletePerfMonitorsAMD((Int32) n, (UInt32*) monitors_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeletePerfMonitorsAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void DeletePerfMonitors(Int32 n, ref UInt32 monitors) {
				unsafe
				{
					fixed (UInt32* monitors_ptr = &monitors) {
						Delegates.glDeletePerfMonitorsAMD((Int32) n, (UInt32*) monitors_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeletePerfMonitorsAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void DeletePerfMonitors(Int32 n, UInt32* monitors) {
				Delegates.glDeletePerfMonitorsAMD((Int32) n, (UInt32*) monitors);
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glEndPerfMonitorAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void EndPerfMonitor(Int32 monitor) {
				Delegates.glEndPerfMonitorAMD((UInt32) monitor);
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glEndPerfMonitorAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void EndPerfMonitor(UInt32 monitor) {
				Delegates.glEndPerfMonitorAMD((UInt32) monitor);
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenPerfMonitorsAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void GenPerfMonitors(Int32 n, [OutAttribute] Int32[] monitors) {
				unsafe
				{
					fixed (Int32* monitors_ptr = monitors) {
						Delegates.glGenPerfMonitorsAMD((Int32) n, (UInt32*) monitors_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenPerfMonitorsAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void GenPerfMonitors(Int32 n, [OutAttribute] out Int32 monitors) {
				unsafe
				{
					fixed (Int32* monitors_ptr = &monitors) {
						Delegates.glGenPerfMonitorsAMD((Int32) n, (UInt32*) monitors_ptr);
						monitors = *monitors_ptr;
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenPerfMonitorsAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void GenPerfMonitors(Int32 n, [OutAttribute] Int32* monitors) {
				Delegates.glGenPerfMonitorsAMD((Int32) n, (UInt32*) monitors);
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenPerfMonitorsAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void GenPerfMonitors(Int32 n, [OutAttribute] UInt32[] monitors) {
				unsafe
				{
					fixed (UInt32* monitors_ptr = monitors) {
						Delegates.glGenPerfMonitorsAMD((Int32) n, (UInt32*) monitors_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenPerfMonitorsAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void GenPerfMonitors(Int32 n, [OutAttribute] out UInt32 monitors) {
				unsafe
				{
					fixed (UInt32* monitors_ptr = &monitors) {
						Delegates.glGenPerfMonitorsAMD((Int32) n, (UInt32*) monitors_ptr);
						monitors = *monitors_ptr;
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenPerfMonitorsAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void GenPerfMonitors(Int32 n, [OutAttribute] UInt32* monitors) {
				Delegates.glGenPerfMonitorsAMD((Int32) n, (UInt32*) monitors);
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetPerfMonitorCounterDataAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void GetPerfMonitorCounterData(Int32 monitor, System.Graphics.ES20.All pname, Int32 dataSize, [OutAttribute] Int32[] data, [OutAttribute] Int32[] bytesWritten) {
				unsafe
				{
					fixed (Int32* data_ptr = data)
					fixed (Int32* bytesWritten_ptr = bytesWritten) {
						Delegates.glGetPerfMonitorCounterDataAMD((UInt32) monitor, (System.Graphics.ES20.All) pname, (Int32) dataSize, (UInt32*) data_ptr, (Int32*) bytesWritten_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetPerfMonitorCounterDataAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void GetPerfMonitorCounterData(Int32 monitor, System.Graphics.ES20.All pname, Int32 dataSize, [OutAttribute] out Int32 data, [OutAttribute] out Int32 bytesWritten) {
				unsafe
				{
					fixed (Int32* data_ptr = &data)
					fixed (Int32* bytesWritten_ptr = &bytesWritten) {
						Delegates.glGetPerfMonitorCounterDataAMD((UInt32) monitor, (System.Graphics.ES20.All) pname, (Int32) dataSize, (UInt32*) data_ptr, (Int32*) bytesWritten_ptr);
						data = *data_ptr;
						bytesWritten = *bytesWritten_ptr;
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetPerfMonitorCounterDataAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void GetPerfMonitorCounterData(Int32 monitor, System.Graphics.ES20.All pname, Int32 dataSize, [OutAttribute] Int32* data, [OutAttribute] Int32* bytesWritten) {
				Delegates.glGetPerfMonitorCounterDataAMD((UInt32) monitor, (System.Graphics.ES20.All) pname, (Int32) dataSize, (UInt32*) data, (Int32*) bytesWritten);
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetPerfMonitorCounterDataAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void GetPerfMonitorCounterData(UInt32 monitor, System.Graphics.ES20.All pname, Int32 dataSize, [OutAttribute] UInt32[] data, [OutAttribute] Int32[] bytesWritten) {
				unsafe
				{
					fixed (UInt32* data_ptr = data)
					fixed (Int32* bytesWritten_ptr = bytesWritten) {
						Delegates.glGetPerfMonitorCounterDataAMD((UInt32) monitor, (System.Graphics.ES20.All) pname, (Int32) dataSize, (UInt32*) data_ptr, (Int32*) bytesWritten_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetPerfMonitorCounterDataAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void GetPerfMonitorCounterData(UInt32 monitor, System.Graphics.ES20.All pname, Int32 dataSize, [OutAttribute] out UInt32 data, [OutAttribute] out Int32 bytesWritten) {
				unsafe
				{
					fixed (UInt32* data_ptr = &data)
					fixed (Int32* bytesWritten_ptr = &bytesWritten) {
						Delegates.glGetPerfMonitorCounterDataAMD((UInt32) monitor, (System.Graphics.ES20.All) pname, (Int32) dataSize, (UInt32*) data_ptr, (Int32*) bytesWritten_ptr);
						data = *data_ptr;
						bytesWritten = *bytesWritten_ptr;
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetPerfMonitorCounterDataAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void GetPerfMonitorCounterData(UInt32 monitor, System.Graphics.ES20.All pname, Int32 dataSize, [OutAttribute] UInt32* data, [OutAttribute] Int32* bytesWritten) {
				Delegates.glGetPerfMonitorCounterDataAMD((UInt32) monitor, (System.Graphics.ES20.All) pname, (Int32) dataSize, (UInt32*) data, (Int32*) bytesWritten);
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetPerfMonitorCounterInfoAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void GetPerfMonitorCounterInfo(Int32 group, Int32 counter, System.Graphics.ES20.All pname, [OutAttribute] IntPtr data) {
				Delegates.glGetPerfMonitorCounterInfoAMD((UInt32) group, (UInt32) counter, (System.Graphics.ES20.All) pname, (IntPtr) data);
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetPerfMonitorCounterInfoAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void GetPerfMonitorCounterInfo<T3>(Int32 group, Int32 counter, System.Graphics.ES20.All pname, [InAttribute, OutAttribute] T3[] data)
							where T3 : struct {
				GCHandle data_ptr = GCHandle.Alloc(data, GCHandleType.Pinned);
				try {
					Delegates.glGetPerfMonitorCounterInfoAMD((UInt32) group, (UInt32) counter, (System.Graphics.ES20.All) pname, (IntPtr) data_ptr.AddrOfPinnedObject());
				} finally {
					data_ptr.Free();
				}
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetPerfMonitorCounterInfoAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void GetPerfMonitorCounterInfo<T3>(Int32 group, Int32 counter, System.Graphics.ES20.All pname, [InAttribute, OutAttribute] T3[,] data)
							where T3 : struct {
				GCHandle data_ptr = GCHandle.Alloc(data, GCHandleType.Pinned);
				try {
					Delegates.glGetPerfMonitorCounterInfoAMD((UInt32) group, (UInt32) counter, (System.Graphics.ES20.All) pname, (IntPtr) data_ptr.AddrOfPinnedObject());
				} finally {
					data_ptr.Free();
				}
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetPerfMonitorCounterInfoAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void GetPerfMonitorCounterInfo<T3>(Int32 group, Int32 counter, System.Graphics.ES20.All pname, [InAttribute, OutAttribute] T3[,,] data)
							where T3 : struct {
				GCHandle data_ptr = GCHandle.Alloc(data, GCHandleType.Pinned);
				try {
					Delegates.glGetPerfMonitorCounterInfoAMD((UInt32) group, (UInt32) counter, (System.Graphics.ES20.All) pname, (IntPtr) data_ptr.AddrOfPinnedObject());
				} finally {
					data_ptr.Free();
				}
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetPerfMonitorCounterInfoAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void GetPerfMonitorCounterInfo<T3>(Int32 group, Int32 counter, System.Graphics.ES20.All pname, [InAttribute, OutAttribute] ref T3 data)
							where T3 : struct {
				GCHandle data_ptr = GCHandle.Alloc(data, GCHandleType.Pinned);
				try {
					Delegates.glGetPerfMonitorCounterInfoAMD((UInt32) group, (UInt32) counter, (System.Graphics.ES20.All) pname, (IntPtr) data_ptr.AddrOfPinnedObject());
					data = (T3) data_ptr.Target;
				} finally {
					data_ptr.Free();
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetPerfMonitorCounterInfoAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void GetPerfMonitorCounterInfo(UInt32 group, UInt32 counter, System.Graphics.ES20.All pname, [OutAttribute] IntPtr data) {
				Delegates.glGetPerfMonitorCounterInfoAMD((UInt32) group, (UInt32) counter, (System.Graphics.ES20.All) pname, (IntPtr) data);
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetPerfMonitorCounterInfoAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void GetPerfMonitorCounterInfo<T3>(UInt32 group, UInt32 counter, System.Graphics.ES20.All pname, [InAttribute, OutAttribute] T3[] data)
				where T3 : struct {
				GCHandle data_ptr = GCHandle.Alloc(data, GCHandleType.Pinned);
				try {
					Delegates.glGetPerfMonitorCounterInfoAMD((UInt32) group, (UInt32) counter, (System.Graphics.ES20.All) pname, (IntPtr) data_ptr.AddrOfPinnedObject());
				} finally {
					data_ptr.Free();
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetPerfMonitorCounterInfoAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void GetPerfMonitorCounterInfo<T3>(UInt32 group, UInt32 counter, System.Graphics.ES20.All pname, [InAttribute, OutAttribute] T3[,] data)
				where T3 : struct {
				GCHandle data_ptr = GCHandle.Alloc(data, GCHandleType.Pinned);
				try {
					Delegates.glGetPerfMonitorCounterInfoAMD((UInt32) group, (UInt32) counter, (System.Graphics.ES20.All) pname, (IntPtr) data_ptr.AddrOfPinnedObject());
				} finally {
					data_ptr.Free();
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetPerfMonitorCounterInfoAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void GetPerfMonitorCounterInfo<T3>(UInt32 group, UInt32 counter, System.Graphics.ES20.All pname, [InAttribute, OutAttribute] T3[,,] data)
				where T3 : struct {
				GCHandle data_ptr = GCHandle.Alloc(data, GCHandleType.Pinned);
				try {
					Delegates.glGetPerfMonitorCounterInfoAMD((UInt32) group, (UInt32) counter, (System.Graphics.ES20.All) pname, (IntPtr) data_ptr.AddrOfPinnedObject());
				} finally {
					data_ptr.Free();
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetPerfMonitorCounterInfoAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void GetPerfMonitorCounterInfo<T3>(UInt32 group, UInt32 counter, System.Graphics.ES20.All pname, [InAttribute, OutAttribute] ref T3 data)
				where T3 : struct {
				GCHandle data_ptr = GCHandle.Alloc(data, GCHandleType.Pinned);
				try {
					Delegates.glGetPerfMonitorCounterInfoAMD((UInt32) group, (UInt32) counter, (System.Graphics.ES20.All) pname, (IntPtr) data_ptr.AddrOfPinnedObject());
					data = (T3) data_ptr.Target;
				} finally {
					data_ptr.Free();
				}
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetPerfMonitorCountersAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void GetPerfMonitorCounters(Int32 group, [OutAttribute] Int32[] numCounters, [OutAttribute] Int32[] maxActiveCounters, Int32 counterSize, [OutAttribute] Int32[] counters) {
				unsafe
				{
					fixed (Int32* numCounters_ptr = numCounters)
					fixed (Int32* maxActiveCounters_ptr = maxActiveCounters)
					fixed (Int32* counters_ptr = counters) {
						Delegates.glGetPerfMonitorCountersAMD((UInt32) group, (Int32*) numCounters_ptr, (Int32*) maxActiveCounters_ptr, (Int32) counterSize, (UInt32*) counters_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetPerfMonitorCountersAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void GetPerfMonitorCounters(Int32 group, [OutAttribute] out Int32 numCounters, [OutAttribute] out Int32 maxActiveCounters, Int32 counterSize, [OutAttribute] out Int32 counters) {
				unsafe
				{
					fixed (Int32* numCounters_ptr = &numCounters)
					fixed (Int32* maxActiveCounters_ptr = &maxActiveCounters)
					fixed (Int32* counters_ptr = &counters) {
						Delegates.glGetPerfMonitorCountersAMD((UInt32) group, (Int32*) numCounters_ptr, (Int32*) maxActiveCounters_ptr, (Int32) counterSize, (UInt32*) counters_ptr);
						numCounters = *numCounters_ptr;
						maxActiveCounters = *maxActiveCounters_ptr;
						counters = *counters_ptr;
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetPerfMonitorCountersAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void GetPerfMonitorCounters(Int32 group, [OutAttribute] Int32* numCounters, [OutAttribute] Int32* maxActiveCounters, Int32 counterSize, [OutAttribute] Int32* counters) {
				Delegates.glGetPerfMonitorCountersAMD((UInt32) group, (Int32*) numCounters, (Int32*) maxActiveCounters, (Int32) counterSize, (UInt32*) counters);
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetPerfMonitorCountersAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void GetPerfMonitorCounters(UInt32 group, [OutAttribute] Int32[] numCounters, [OutAttribute] Int32[] maxActiveCounters, Int32 counterSize, [OutAttribute] UInt32[] counters) {
				unsafe
				{
					fixed (Int32* numCounters_ptr = numCounters)
					fixed (Int32* maxActiveCounters_ptr = maxActiveCounters)
					fixed (UInt32* counters_ptr = counters) {
						Delegates.glGetPerfMonitorCountersAMD((UInt32) group, (Int32*) numCounters_ptr, (Int32*) maxActiveCounters_ptr, (Int32) counterSize, (UInt32*) counters_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetPerfMonitorCountersAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void GetPerfMonitorCounters(UInt32 group, [OutAttribute] out Int32 numCounters, [OutAttribute] out Int32 maxActiveCounters, Int32 counterSize, [OutAttribute] out UInt32 counters) {
				unsafe
				{
					fixed (Int32* numCounters_ptr = &numCounters)
					fixed (Int32* maxActiveCounters_ptr = &maxActiveCounters)
					fixed (UInt32* counters_ptr = &counters) {
						Delegates.glGetPerfMonitorCountersAMD((UInt32) group, (Int32*) numCounters_ptr, (Int32*) maxActiveCounters_ptr, (Int32) counterSize, (UInt32*) counters_ptr);
						numCounters = *numCounters_ptr;
						maxActiveCounters = *maxActiveCounters_ptr;
						counters = *counters_ptr;
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetPerfMonitorCountersAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void GetPerfMonitorCounters(UInt32 group, [OutAttribute] Int32* numCounters, [OutAttribute] Int32* maxActiveCounters, Int32 counterSize, [OutAttribute] UInt32* counters) {
				Delegates.glGetPerfMonitorCountersAMD((UInt32) group, (Int32*) numCounters, (Int32*) maxActiveCounters, (Int32) counterSize, (UInt32*) counters);
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetPerfMonitorCounterStringAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void GetPerfMonitorCounterString(Int32 group, Int32 counter, Int32 bufSize, [OutAttribute] Int32[] length, [OutAttribute] StringBuilder counterString) {
				unsafe
				{
					fixed (Int32* length_ptr = length) {
						Delegates.glGetPerfMonitorCounterStringAMD((UInt32) group, (UInt32) counter, (Int32) bufSize, (Int32*) length_ptr, (StringBuilder) counterString);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetPerfMonitorCounterStringAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void GetPerfMonitorCounterString(Int32 group, Int32 counter, Int32 bufSize, [OutAttribute] out Int32 length, [OutAttribute] StringBuilder counterString) {
				unsafe
				{
					fixed (Int32* length_ptr = &length) {
						Delegates.glGetPerfMonitorCounterStringAMD((UInt32) group, (UInt32) counter, (Int32) bufSize, (Int32*) length_ptr, (StringBuilder) counterString);
						length = *length_ptr;
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetPerfMonitorCounterStringAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void GetPerfMonitorCounterString(Int32 group, Int32 counter, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] StringBuilder counterString) {
				Delegates.glGetPerfMonitorCounterStringAMD((UInt32) group, (UInt32) counter, (Int32) bufSize, (Int32*) length, (StringBuilder) counterString);
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetPerfMonitorCounterStringAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void GetPerfMonitorCounterString(UInt32 group, UInt32 counter, Int32 bufSize, [OutAttribute] Int32[] length, [OutAttribute] StringBuilder counterString) {
				unsafe
				{
					fixed (Int32* length_ptr = length) {
						Delegates.glGetPerfMonitorCounterStringAMD((UInt32) group, (UInt32) counter, (Int32) bufSize, (Int32*) length_ptr, (StringBuilder) counterString);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetPerfMonitorCounterStringAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void GetPerfMonitorCounterString(UInt32 group, UInt32 counter, Int32 bufSize, [OutAttribute] out Int32 length, [OutAttribute] StringBuilder counterString) {
				unsafe
				{
					fixed (Int32* length_ptr = &length) {
						Delegates.glGetPerfMonitorCounterStringAMD((UInt32) group, (UInt32) counter, (Int32) bufSize, (Int32*) length_ptr, (StringBuilder) counterString);
						length = *length_ptr;
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetPerfMonitorCounterStringAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void GetPerfMonitorCounterString(UInt32 group, UInt32 counter, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] StringBuilder counterString) {
				Delegates.glGetPerfMonitorCounterStringAMD((UInt32) group, (UInt32) counter, (Int32) bufSize, (Int32*) length, (StringBuilder) counterString);
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetPerfMonitorGroupsAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void GetPerfMonitorGroup([OutAttribute] Int32[] numGroups, Int32 groupsSize, [OutAttribute] Int32[] groups) {
				unsafe
				{
					fixed (Int32* numGroups_ptr = numGroups)
					fixed (Int32* groups_ptr = groups) {
						Delegates.glGetPerfMonitorGroupsAMD((Int32*) numGroups_ptr, (Int32) groupsSize, (UInt32*) groups_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetPerfMonitorGroupsAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void GetPerfMonitorGroup([OutAttribute] Int32[] numGroups, Int32 groupsSize, [OutAttribute] UInt32[] groups) {
				unsafe
				{
					fixed (Int32* numGroups_ptr = numGroups)
					fixed (UInt32* groups_ptr = groups) {
						Delegates.glGetPerfMonitorGroupsAMD((Int32*) numGroups_ptr, (Int32) groupsSize, (UInt32*) groups_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetPerfMonitorGroupsAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void GetPerfMonitorGroup([OutAttribute] out Int32 numGroups, Int32 groupsSize, [OutAttribute] out Int32 groups) {
				unsafe
				{
					fixed (Int32* numGroups_ptr = &numGroups)
					fixed (Int32* groups_ptr = &groups) {
						Delegates.glGetPerfMonitorGroupsAMD((Int32*) numGroups_ptr, (Int32) groupsSize, (UInt32*) groups_ptr);
						numGroups = *numGroups_ptr;
						groups = *groups_ptr;
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetPerfMonitorGroupsAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void GetPerfMonitorGroup([OutAttribute] out Int32 numGroups, Int32 groupsSize, [OutAttribute] out UInt32 groups) {
				unsafe
				{
					fixed (Int32* numGroups_ptr = &numGroups)
					fixed (UInt32* groups_ptr = &groups) {
						Delegates.glGetPerfMonitorGroupsAMD((Int32*) numGroups_ptr, (Int32) groupsSize, (UInt32*) groups_ptr);
						numGroups = *numGroups_ptr;
						groups = *groups_ptr;
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetPerfMonitorGroupsAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void GetPerfMonitorGroup([OutAttribute] Int32* numGroups, Int32 groupsSize, [OutAttribute] Int32* groups) {
				Delegates.glGetPerfMonitorGroupsAMD((Int32*) numGroups, (Int32) groupsSize, (UInt32*) groups);
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetPerfMonitorGroupsAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void GetPerfMonitorGroup([OutAttribute] Int32* numGroups, Int32 groupsSize, [OutAttribute] UInt32* groups) {
				Delegates.glGetPerfMonitorGroupsAMD((Int32*) numGroups, (Int32) groupsSize, (UInt32*) groups);
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetPerfMonitorGroupStringAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void GetPerfMonitorGroupString(Int32 group, Int32 bufSize, [OutAttribute] Int32[] length, [OutAttribute] StringBuilder groupString) {
				unsafe
				{
					fixed (Int32* length_ptr = length) {
						Delegates.glGetPerfMonitorGroupStringAMD((UInt32) group, (Int32) bufSize, (Int32*) length_ptr, (StringBuilder) groupString);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetPerfMonitorGroupStringAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void GetPerfMonitorGroupString(Int32 group, Int32 bufSize, [OutAttribute] out Int32 length, [OutAttribute] StringBuilder groupString) {
				unsafe
				{
					fixed (Int32* length_ptr = &length) {
						Delegates.glGetPerfMonitorGroupStringAMD((UInt32) group, (Int32) bufSize, (Int32*) length_ptr, (StringBuilder) groupString);
						length = *length_ptr;
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetPerfMonitorGroupStringAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void GetPerfMonitorGroupString(Int32 group, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] StringBuilder groupString) {
				Delegates.glGetPerfMonitorGroupStringAMD((UInt32) group, (Int32) bufSize, (Int32*) length, (StringBuilder) groupString);
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetPerfMonitorGroupStringAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void GetPerfMonitorGroupString(UInt32 group, Int32 bufSize, [OutAttribute] Int32[] length, [OutAttribute] StringBuilder groupString) {
				unsafe
				{
					fixed (Int32* length_ptr = length) {
						Delegates.glGetPerfMonitorGroupStringAMD((UInt32) group, (Int32) bufSize, (Int32*) length_ptr, (StringBuilder) groupString);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetPerfMonitorGroupStringAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void GetPerfMonitorGroupString(UInt32 group, Int32 bufSize, [OutAttribute] out Int32 length, [OutAttribute] StringBuilder groupString) {
				unsafe
				{
					fixed (Int32* length_ptr = &length) {
						Delegates.glGetPerfMonitorGroupStringAMD((UInt32) group, (Int32) bufSize, (Int32*) length_ptr, (StringBuilder) groupString);
						length = *length_ptr;
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetPerfMonitorGroupStringAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void GetPerfMonitorGroupString(UInt32 group, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] StringBuilder groupString) {
				Delegates.glGetPerfMonitorGroupStringAMD((UInt32) group, (Int32) bufSize, (Int32*) length, (StringBuilder) groupString);
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glSelectPerfMonitorCountersAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void SelectPerfMonitorCounters(Int32 monitor, bool enable, Int32 group, Int32 numCounters, Int32[] countersList) {
				unsafe
				{
					fixed (Int32* countersList_ptr = countersList) {
						Delegates.glSelectPerfMonitorCountersAMD((UInt32) monitor, (bool) enable, (UInt32) group, (Int32) numCounters, (UInt32*) countersList_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glSelectPerfMonitorCountersAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void SelectPerfMonitorCounters(Int32 monitor, bool enable, Int32 group, Int32 numCounters, ref Int32 countersList) {
				unsafe
				{
					fixed (Int32* countersList_ptr = &countersList) {
						Delegates.glSelectPerfMonitorCountersAMD((UInt32) monitor, (bool) enable, (UInt32) group, (Int32) numCounters, (UInt32*) countersList_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glSelectPerfMonitorCountersAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void SelectPerfMonitorCounters(Int32 monitor, bool enable, Int32 group, Int32 numCounters, Int32* countersList) {
				Delegates.glSelectPerfMonitorCountersAMD((UInt32) monitor, (bool) enable, (UInt32) group, (Int32) numCounters, (UInt32*) countersList);
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glSelectPerfMonitorCountersAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void SelectPerfMonitorCounters(UInt32 monitor, bool enable, UInt32 group, Int32 numCounters, UInt32[] countersList) {
				unsafe
				{
					fixed (UInt32* countersList_ptr = countersList) {
						Delegates.glSelectPerfMonitorCountersAMD((UInt32) monitor, (bool) enable, (UInt32) group, (Int32) numCounters, (UInt32*) countersList_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glSelectPerfMonitorCountersAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void SelectPerfMonitorCounters(UInt32 monitor, bool enable, UInt32 group, Int32 numCounters, ref UInt32 countersList) {
				unsafe
				{
					fixed (UInt32* countersList_ptr = &countersList) {
						Delegates.glSelectPerfMonitorCountersAMD((UInt32) monitor, (bool) enable, (UInt32) group, (Int32) numCounters, (UInt32*) countersList_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glSelectPerfMonitorCountersAMD")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void SelectPerfMonitorCounters(UInt32 monitor, bool enable, UInt32 group, Int32 numCounters, UInt32* countersList) {
				Delegates.glSelectPerfMonitorCountersAMD((UInt32) monitor, (bool) enable, (UInt32) group, (Int32) numCounters, (UInt32*) countersList);
			}

		}

		/// <summary>
		/// Angle extensions
		/// </summary>
		public static class Angle {

			/// <summary>[requires: 2.0]
			/// Copy a block of pixels from the read framebuffer to the draw framebuffer
			/// </summary>
			/// <param name="srcX0">
			/// <para>
			/// Specify the bounds of the source rectangle within the read buffer of the read framebuffer.
			/// </para>
			/// </param>
			/// <param name="dstX0">
			/// <para>
			/// Specify the bounds of the destination rectangle within the write buffer of the write framebuffer.
			/// </para>
			/// </param>
			/// <param name="mask">
			/// <para>
			/// The bitwise OR of the type indicating which buffers are to be copied. The allowed type are GL_COLOR_BUFFER_BIT, GL_DEPTH_BUFFER_BIT and GL_STENCIL_BUFFER_BIT.
			/// </para>
			/// </param>
			/// <param name="filter">
			/// <para>
			/// Specifies the interpolation to be applied if the image is stretched. Must be GL_NEAREST or GL_LINEAR.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glBlitFramebufferANGLE")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void BlitFramebuffer(Int32 srcX0, Int32 srcY0, Int32 srcX1, Int32 srcY1, Int32 dstX0, Int32 dstY0, Int32 dstX1, Int32 dstY1, Int32 mask, System.Graphics.ES20.All filter) {
				Delegates.glBlitFramebufferANGLE((Int32) srcX0, (Int32) srcY0, (Int32) srcX1, (Int32) srcY1, (Int32) dstX0, (Int32) dstY0, (Int32) dstX1, (Int32) dstY1, (UInt32) mask, (System.Graphics.ES20.All) filter);
			}


			/// <summary>[requires: 2.0]
			/// Copy a block of pixels from the read framebuffer to the draw framebuffer
			/// </summary>
			/// <param name="srcX0">
			/// <para>
			/// Specify the bounds of the source rectangle within the read buffer of the read framebuffer.
			/// </para>
			/// </param>
			/// <param name="dstX0">
			/// <para>
			/// Specify the bounds of the destination rectangle within the write buffer of the write framebuffer.
			/// </para>
			/// </param>
			/// <param name="mask">
			/// <para>
			/// The bitwise OR of the type indicating which buffers are to be copied. The allowed type are GL_COLOR_BUFFER_BIT, GL_DEPTH_BUFFER_BIT and GL_STENCIL_BUFFER_BIT.
			/// </para>
			/// </param>
			/// <param name="filter">
			/// <para>
			/// Specifies the interpolation to be applied if the image is stretched. Must be GL_NEAREST or GL_LINEAR.
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glBlitFramebufferANGLE")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void BlitFramebuffer(Int32 srcX0, Int32 srcY0, Int32 srcX1, Int32 srcY1, Int32 dstX0, Int32 dstY0, Int32 dstX1, Int32 dstY1, UInt32 mask, System.Graphics.ES20.All filter) {
				Delegates.glBlitFramebufferANGLE((Int32) srcX0, (Int32) srcY0, (Int32) srcX1, (Int32) srcY1, (Int32) dstX0, (Int32) dstY0, (Int32) dstX1, (Int32) dstY1, (UInt32) mask, (System.Graphics.ES20.All) filter);
			}


			/// <summary>[requires: 2.0]
			/// Establish data storage, format, dimensions and sample count of a renderbuffer object's image
			/// </summary>
			/// <param name="target">
			/// <para>
			/// Specifies a binding to which the target of the allocation and must be GL_RENDERBUFFER.
			/// </para>
			/// </param>
			/// <param name="samples">
			/// <para>
			/// Specifies the number of samples to be used for the renderbuffer object's storage.
			/// </para>
			/// </param>
			/// <param name="internalformat">
			/// <para>
			/// Specifies the internal format to use for the renderbuffer object's image.
			/// </para>
			/// </param>
			/// <param name="width">
			/// <para>
			/// Specifies the width of the renderbuffer, in pixels.
			/// </para>
			/// </param>
			/// <param name="height">
			/// <para>
			/// Specifies the height of the renderbuffer, in pixels.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glRenderbufferStorageMultisampleANGLE")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void RenderbufferStorageMultisample(System.Graphics.ES20.All target, Int32 samples, System.Graphics.ES20.All internalformat, Int32 width, Int32 height) {
				Delegates.glRenderbufferStorageMultisampleANGLE((System.Graphics.ES20.All) target, (Int32) samples, (System.Graphics.ES20.All) internalformat, (Int32) width, (Int32) height);
			}

		}

		/// <summary>
		/// Apple extensions
		/// </summary>
		public static class Apple {

			/// <summary>[requires: 2.0]
			/// Establish data storage, format, dimensions and sample count of a renderbuffer object's image
			/// </summary>
			/// <param name="target">
			/// <para>
			/// Specifies a binding to which the target of the allocation and must be GL_RENDERBUFFER.
			/// </para>
			/// </param>
			/// <param name="samples">
			/// <para>
			/// Specifies the number of samples to be used for the renderbuffer object's storage.
			/// </para>
			/// </param>
			/// <param name="internalformat">
			/// <para>
			/// Specifies the internal format to use for the renderbuffer object's image.
			/// </para>
			/// </param>
			/// <param name="width">
			/// <para>
			/// Specifies the width of the renderbuffer, in pixels.
			/// </para>
			/// </param>
			/// <param name="height">
			/// <para>
			/// Specifies the height of the renderbuffer, in pixels.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glRenderbufferStorageMultisampleAPPLE")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void RenderbufferStorageMultisample() {
				Delegates.glRenderbufferStorageMultisampleAPPLE();
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glResolveMultisampleFramebufferAPPLE")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void ResolveMultisampleFramebuffer() {
				Delegates.glResolveMultisampleFramebufferAPPLE();
			}

		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Select active texture unit
		/// </summary>
		/// <param name="texture">
		/// <para>
		/// Specifies which texture unit to make active. The number of texture units is implementation dependent, but must be at least two. texture must be one of GL_TEXTUREi, where i ranges from 0 (GL_MAX_COMBINED_TEXTURE_IMAGE_UNITS - 1). The initial value is GL_TEXTURE0.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glActiveTexture")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void ActiveTexture(System.Graphics.ES20.TextureUnit texture) {
			Delegates.glActiveTexture((System.Graphics.ES20.TextureUnit) texture);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Attaches a shader object to a program object
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to which a shader object will be attached.
		/// </para>
		/// </param>
		/// <param name="shader">
		/// <para>
		/// Specifies the shader object that is to be attached.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glAttachShader")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void AttachShader(Int32 program, Int32 shader) {
			Delegates.glAttachShader((UInt32) program, (UInt32) shader);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Attaches a shader object to a program object
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to which a shader object will be attached.
		/// </para>
		/// </param>
		/// <param name="shader">
		/// <para>
		/// Specifies the shader object that is to be attached.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glAttachShader")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void AttachShader(UInt32 program, UInt32 shader) {
			Delegates.glAttachShader((UInt32) program, (UInt32) shader);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Associates a generic vertex attribute index with a named attribute variable
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the handle of the program object in which the association is to be made.
		/// </para>
		/// </param>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be bound.
		/// </para>
		/// </param>
		/// <param name="name">
		/// <para>
		/// Specifies a null terminated string containing the name of the vertex shader attribute variable to which index is to be bound.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glBindAttribLocation")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void BindAttribLocation(Int32 program, Int32 index, String name) {
			Delegates.glBindAttribLocation((UInt32) program, (UInt32) index, (String) name);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Associates a generic vertex attribute index with a named attribute variable
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the handle of the program object in which the association is to be made.
		/// </para>
		/// </param>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be bound.
		/// </para>
		/// </param>
		/// <param name="name">
		/// <para>
		/// Specifies a null terminated string containing the name of the vertex shader attribute variable to which index is to be bound.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glBindAttribLocation")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void BindAttribLocation(UInt32 program, UInt32 index, String name) {
			Delegates.glBindAttribLocation((UInt32) program, (UInt32) index, (String) name);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Bind a named buffer object
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target to which the buffer object is bound. The symbolic constant must be GL_ARRAY_BUFFER, GL_COPY_READ_BUFFER, GL_COPY_WRITE_BUFFER, GL_ELEMENT_ARRAY_BUFFER, GL_PIXEL_PACK_BUFFER, GL_PIXEL_UNPACK_BUFFER, GL_TEXTURE_BUFFER, GL_TRANSFORM_FEEDBACK_BUFFER, or GL_UNIFORM_BUFFER.
		/// </para>
		/// </param>
		/// <param name="buffer">
		/// <para>
		/// Specifies the name of a buffer object.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glBindBuffer")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void BindBuffer(System.Graphics.ES20.BufferTarget target, Int32 buffer) {
			Delegates.glBindBuffer((System.Graphics.ES20.BufferTarget) target, (UInt32) buffer);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Bind a named buffer object
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target to which the buffer object is bound. The symbolic constant must be GL_ARRAY_BUFFER, GL_COPY_READ_BUFFER, GL_COPY_WRITE_BUFFER, GL_ELEMENT_ARRAY_BUFFER, GL_PIXEL_PACK_BUFFER, GL_PIXEL_UNPACK_BUFFER, GL_TEXTURE_BUFFER, GL_TRANSFORM_FEEDBACK_BUFFER, or GL_UNIFORM_BUFFER.
		/// </para>
		/// </param>
		/// <param name="buffer">
		/// <para>
		/// Specifies the name of a buffer object.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glBindBuffer")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void BindBuffer(System.Graphics.ES20.BufferTarget target, UInt32 buffer) {
			Delegates.glBindBuffer((System.Graphics.ES20.BufferTarget) target, (UInt32) buffer);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Bind a framebuffer to a framebuffer target
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the framebuffer target of the binding operation.
		/// </para>
		/// </param>
		/// <param name="framebuffer">
		/// <para>
		/// Specifies the name of the framebuffer object to bind.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glBindFramebuffer")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void BindFramebuffer(System.Graphics.ES20.FramebufferTarget target, Int32 framebuffer) {
			Delegates.glBindFramebuffer((System.Graphics.ES20.FramebufferTarget) target, (UInt32) framebuffer);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Bind a framebuffer to a framebuffer target
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the framebuffer target of the binding operation.
		/// </para>
		/// </param>
		/// <param name="framebuffer">
		/// <para>
		/// Specifies the name of the framebuffer object to bind.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glBindFramebuffer")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void BindFramebuffer(System.Graphics.ES20.FramebufferTarget target, UInt32 framebuffer) {
			Delegates.glBindFramebuffer((System.Graphics.ES20.FramebufferTarget) target, (UInt32) framebuffer);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Bind a renderbuffer to a renderbuffer target
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the renderbuffer target of the binding operation. target must be GL_RENDERBUFFER.
		/// </para>
		/// </param>
		/// <param name="renderbuffer">
		/// <para>
		/// Specifies the name of the renderbuffer object to bind.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glBindRenderbuffer")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void BindRenderbuffer(System.Graphics.ES20.RenderbufferTarget target, Int32 renderbuffer) {
			Delegates.glBindRenderbuffer((System.Graphics.ES20.RenderbufferTarget) target, (UInt32) renderbuffer);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Bind a renderbuffer to a renderbuffer target
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the renderbuffer target of the binding operation. target must be GL_RENDERBUFFER.
		/// </para>
		/// </param>
		/// <param name="renderbuffer">
		/// <para>
		/// Specifies the name of the renderbuffer object to bind.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glBindRenderbuffer")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void BindRenderbuffer(System.Graphics.ES20.RenderbufferTarget target, UInt32 renderbuffer) {
			Delegates.glBindRenderbuffer((System.Graphics.ES20.RenderbufferTarget) target, (UInt32) renderbuffer);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Bind a named texture to a texturing target
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target to which the texture is bound. Must be either GL_TEXTURE_1D, GL_TEXTURE_2D, GL_TEXTURE_3D, or GL_TEXTURE_1D_ARRAY, GL_TEXTURE_2D_ARRAY, GL_TEXTURE_RECTANGLE, GL_TEXTURE_CUBE_MAP, GL_TEXTURE_2D_MULTISAMPLE or GL_TEXTURE_2D_MULTISAMPLE_ARRAY.
		/// </para>
		/// </param>
		/// <param name="texture">
		/// <para>
		/// Specifies the name of a texture.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glBindTexture")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void BindTexture(System.Graphics.ES20.TextureTarget target, Int32 texture) {
			Delegates.glBindTexture((System.Graphics.ES20.TextureTarget) target, (UInt32) texture);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Bind a named texture to a texturing target
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target to which the texture is bound. Must be either GL_TEXTURE_1D, GL_TEXTURE_2D, GL_TEXTURE_3D, or GL_TEXTURE_1D_ARRAY, GL_TEXTURE_2D_ARRAY, GL_TEXTURE_RECTANGLE, GL_TEXTURE_CUBE_MAP, GL_TEXTURE_2D_MULTISAMPLE or GL_TEXTURE_2D_MULTISAMPLE_ARRAY.
		/// </para>
		/// </param>
		/// <param name="texture">
		/// <para>
		/// Specifies the name of a texture.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glBindTexture")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void BindTexture(System.Graphics.ES20.TextureTarget target, UInt32 texture) {
			Delegates.glBindTexture((System.Graphics.ES20.TextureTarget) target, (UInt32) texture);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Set the blend color
		/// </summary>
		/// <param name="red">
		/// <para>
		/// specify the components of GL_BLEND_COLOR
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glBlendColor")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void BlendColor(Single red, Single green, Single blue, Single alpha) {
			Delegates.glBlendColor((Single) red, (Single) green, (Single) blue, (Single) alpha);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify the equation used for both the RGB blend equation and the Alpha blend equation
		/// </summary>
		/// <param name="mode">
		/// <para>
		/// specifies how source and destination colors are combined. It must be GL_FUNC_ADD, GL_FUNC_SUBTRACT, GL_FUNC_REVERSE_SUBTRACT, GL_MIN, GL_MAX.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glBlendEquation")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void BlendEquation(System.Graphics.ES20.BlendEquationMode mode) {
			Delegates.glBlendEquation((System.Graphics.ES20.BlendEquationMode) mode);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Set the RGB blend equation and the alpha blend equation separately
		/// </summary>
		/// <param name="modeRGB">
		/// <para>
		/// specifies the RGB blend equation, how the red, green, and blue components of the source and destination colors are combined. It must be GL_FUNC_ADD, GL_FUNC_SUBTRACT, GL_FUNC_REVERSE_SUBTRACT, GL_MIN, GL_MAX.
		/// </para>
		/// </param>
		/// <param name="modeAlpha">
		/// <para>
		/// specifies the alpha blend equation, how the alpha component of the source and destination colors are combined. It must be GL_FUNC_ADD, GL_FUNC_SUBTRACT, GL_FUNC_REVERSE_SUBTRACT, GL_MIN, GL_MAX.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glBlendEquationSeparate")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void BlendEquationSeparate(System.Graphics.ES20.BlendEquationMode modeRGB, System.Graphics.ES20.BlendEquationMode modeAlpha) {
			Delegates.glBlendEquationSeparate((System.Graphics.ES20.BlendEquationMode) modeRGB, (System.Graphics.ES20.BlendEquationMode) modeAlpha);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify pixel arithmetic
		/// </summary>
		/// <param name="sfactor">
		/// <para>
		/// Specifies how the red, green, blue, and alpha source blending factors are computed. The initial value is GL_ONE.
		/// </para>
		/// </param>
		/// <param name="dfactor">
		/// <para>
		/// Specifies how the red, green, blue, and alpha destination blending factors are computed. The following symbolic constants are accepted: GL_ZERO, GL_ONE, GL_SRC_COLOR, GL_ONE_MINUS_SRC_COLOR, GL_DST_COLOR, GL_ONE_MINUS_DST_COLOR, GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA, GL_DST_ALPHA, GL_ONE_MINUS_DST_ALPHA. GL_CONSTANT_COLOR, GL_ONE_MINUS_CONSTANT_COLOR, GL_CONSTANT_ALPHA, and GL_ONE_MINUS_CONSTANT_ALPHA. The initial value is GL_ZERO.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glBlendFunc")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void BlendFunc(System.Graphics.ES20.BlendingFactorSrc sfactor, System.Graphics.ES20.BlendingFactorDest dfactor) {
			Delegates.glBlendFunc((System.Graphics.ES20.BlendingFactorSrc) sfactor, (System.Graphics.ES20.BlendingFactorDest) dfactor);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify pixel arithmetic for RGB and alpha components separately
		/// </summary>
		/// <param name="srcRGB">
		/// <para>
		/// Specifies how the red, green, and blue blending factors are computed. The initial value is GL_ONE.
		/// </para>
		/// </param>
		/// <param name="dstRGB">
		/// <para>
		/// Specifies how the red, green, and blue destination blending factors are computed. The initial value is GL_ZERO.
		/// </para>
		/// </param>
		/// <param name="srcAlpha">
		/// <para>
		/// Specified how the alpha source blending factor is computed. The initial value is GL_ONE.
		/// </para>
		/// </param>
		/// <param name="dstAlpha">
		/// <para>
		/// Specified how the alpha destination blending factor is computed. The initial value is GL_ZERO.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glBlendFuncSeparate")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void BlendFuncSeparate(System.Graphics.ES20.BlendingFactorSrc srcRGB, System.Graphics.ES20.BlendingFactorDest dstRGB, System.Graphics.ES20.BlendingFactorSrc srcAlpha, System.Graphics.ES20.BlendingFactorDest dstAlpha) {
			Delegates.glBlendFuncSeparate((System.Graphics.ES20.BlendingFactorSrc) srcRGB, (System.Graphics.ES20.BlendingFactorDest) dstRGB, (System.Graphics.ES20.BlendingFactorSrc) srcAlpha, (System.Graphics.ES20.BlendingFactorDest) dstAlpha);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Creates and initializes a buffer object's data store
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target buffer object. The symbolic constant must be GL_ARRAY_BUFFER, GL_COPY_READ_BUFFER, GL_COPY_WRITE_BUFFER, GL_ELEMENT_ARRAY_BUFFER, GL_PIXEL_PACK_BUFFER, GL_PIXEL_UNPACK_BUFFER, GL_TEXTURE_BUFFER, GL_TRANSFORM_FEEDBACK_BUFFER, or GL_UNIFORM_BUFFER.
		/// </para>
		/// </param>
		/// <param name="size">
		/// <para>
		/// Specifies the size in bytes of the buffer object's new data store.
		/// </para>
		/// </param>
		/// <param name="data">
		/// <para>
		/// Specifies a pointer to data that will be copied into the data store for initialization, or NULL if no data is to be copied.
		/// </para>
		/// </param>
		/// <param name="usage">
		/// <para>
		/// Specifies the expected usage pattern of the data store. The symbolic constant must be GL_STREAM_DRAW, GL_STREAM_READ, GL_STREAM_COPY, GL_STATIC_DRAW, GL_STATIC_READ, GL_STATIC_COPY, GL_DYNAMIC_DRAW, GL_DYNAMIC_READ, or GL_DYNAMIC_COPY.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glBufferData")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void BufferData(System.Graphics.ES20.BufferTarget target, IntPtr size, IntPtr data, System.Graphics.ES20.BufferUsage usage) {
			Delegates.glBufferData((System.Graphics.ES20.BufferTarget) target, (IntPtr) size, (IntPtr) data, (System.Graphics.ES20.BufferUsage) usage);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Creates and initializes a buffer object's data store
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target buffer object. The symbolic constant must be GL_ARRAY_BUFFER, GL_COPY_READ_BUFFER, GL_COPY_WRITE_BUFFER, GL_ELEMENT_ARRAY_BUFFER, GL_PIXEL_PACK_BUFFER, GL_PIXEL_UNPACK_BUFFER, GL_TEXTURE_BUFFER, GL_TRANSFORM_FEEDBACK_BUFFER, or GL_UNIFORM_BUFFER.
		/// </para>
		/// </param>
		/// <param name="size">
		/// <para>
		/// Specifies the size in bytes of the buffer object's new data store.
		/// </para>
		/// </param>
		/// <param name="data">
		/// <para>
		/// Specifies a pointer to data that will be copied into the data store for initialization, or NULL if no data is to be copied.
		/// </para>
		/// </param>
		/// <param name="usage">
		/// <para>
		/// Specifies the expected usage pattern of the data store. The symbolic constant must be GL_STREAM_DRAW, GL_STREAM_READ, GL_STREAM_COPY, GL_STATIC_DRAW, GL_STATIC_READ, GL_STATIC_COPY, GL_DYNAMIC_DRAW, GL_DYNAMIC_READ, or GL_DYNAMIC_COPY.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glBufferData")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void BufferData<T2>(System.Graphics.ES20.BufferTarget target, IntPtr size, [InAttribute, OutAttribute] T2[] data, System.Graphics.ES20.BufferUsage usage)
					where T2 : struct {
			GCHandle data_ptr = GCHandle.Alloc(data, GCHandleType.Pinned);
			try {
				Delegates.glBufferData((System.Graphics.ES20.BufferTarget) target, (IntPtr) size, (IntPtr) data_ptr.AddrOfPinnedObject(), (System.Graphics.ES20.BufferUsage) usage);
			} finally {
				data_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Creates and initializes a buffer object's data store
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target buffer object. The symbolic constant must be GL_ARRAY_BUFFER, GL_COPY_READ_BUFFER, GL_COPY_WRITE_BUFFER, GL_ELEMENT_ARRAY_BUFFER, GL_PIXEL_PACK_BUFFER, GL_PIXEL_UNPACK_BUFFER, GL_TEXTURE_BUFFER, GL_TRANSFORM_FEEDBACK_BUFFER, or GL_UNIFORM_BUFFER.
		/// </para>
		/// </param>
		/// <param name="size">
		/// <para>
		/// Specifies the size in bytes of the buffer object's new data store.
		/// </para>
		/// </param>
		/// <param name="data">
		/// <para>
		/// Specifies a pointer to data that will be copied into the data store for initialization, or NULL if no data is to be copied.
		/// </para>
		/// </param>
		/// <param name="usage">
		/// <para>
		/// Specifies the expected usage pattern of the data store. The symbolic constant must be GL_STREAM_DRAW, GL_STREAM_READ, GL_STREAM_COPY, GL_STATIC_DRAW, GL_STATIC_READ, GL_STATIC_COPY, GL_DYNAMIC_DRAW, GL_DYNAMIC_READ, or GL_DYNAMIC_COPY.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glBufferData")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void BufferData<T2>(System.Graphics.ES20.BufferTarget target, IntPtr size, [InAttribute, OutAttribute] T2[,] data, System.Graphics.ES20.BufferUsage usage)
					where T2 : struct {
			GCHandle data_ptr = GCHandle.Alloc(data, GCHandleType.Pinned);
			try {
				Delegates.glBufferData((System.Graphics.ES20.BufferTarget) target, (IntPtr) size, (IntPtr) data_ptr.AddrOfPinnedObject(), (System.Graphics.ES20.BufferUsage) usage);
			} finally {
				data_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Creates and initializes a buffer object's data store
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target buffer object. The symbolic constant must be GL_ARRAY_BUFFER, GL_COPY_READ_BUFFER, GL_COPY_WRITE_BUFFER, GL_ELEMENT_ARRAY_BUFFER, GL_PIXEL_PACK_BUFFER, GL_PIXEL_UNPACK_BUFFER, GL_TEXTURE_BUFFER, GL_TRANSFORM_FEEDBACK_BUFFER, or GL_UNIFORM_BUFFER.
		/// </para>
		/// </param>
		/// <param name="size">
		/// <para>
		/// Specifies the size in bytes of the buffer object's new data store.
		/// </para>
		/// </param>
		/// <param name="data">
		/// <para>
		/// Specifies a pointer to data that will be copied into the data store for initialization, or NULL if no data is to be copied.
		/// </para>
		/// </param>
		/// <param name="usage">
		/// <para>
		/// Specifies the expected usage pattern of the data store. The symbolic constant must be GL_STREAM_DRAW, GL_STREAM_READ, GL_STREAM_COPY, GL_STATIC_DRAW, GL_STATIC_READ, GL_STATIC_COPY, GL_DYNAMIC_DRAW, GL_DYNAMIC_READ, or GL_DYNAMIC_COPY.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glBufferData")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void BufferData<T2>(System.Graphics.ES20.BufferTarget target, IntPtr size, [InAttribute, OutAttribute] T2[,,] data, System.Graphics.ES20.BufferUsage usage)
					where T2 : struct {
			GCHandle data_ptr = GCHandle.Alloc(data, GCHandleType.Pinned);
			try {
				Delegates.glBufferData((System.Graphics.ES20.BufferTarget) target, (IntPtr) size, (IntPtr) data_ptr.AddrOfPinnedObject(), (System.Graphics.ES20.BufferUsage) usage);
			} finally {
				data_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Creates and initializes a buffer object's data store
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target buffer object. The symbolic constant must be GL_ARRAY_BUFFER, GL_COPY_READ_BUFFER, GL_COPY_WRITE_BUFFER, GL_ELEMENT_ARRAY_BUFFER, GL_PIXEL_PACK_BUFFER, GL_PIXEL_UNPACK_BUFFER, GL_TEXTURE_BUFFER, GL_TRANSFORM_FEEDBACK_BUFFER, or GL_UNIFORM_BUFFER.
		/// </para>
		/// </param>
		/// <param name="size">
		/// <para>
		/// Specifies the size in bytes of the buffer object's new data store.
		/// </para>
		/// </param>
		/// <param name="data">
		/// <para>
		/// Specifies a pointer to data that will be copied into the data store for initialization, or NULL if no data is to be copied.
		/// </para>
		/// </param>
		/// <param name="usage">
		/// <para>
		/// Specifies the expected usage pattern of the data store. The symbolic constant must be GL_STREAM_DRAW, GL_STREAM_READ, GL_STREAM_COPY, GL_STATIC_DRAW, GL_STATIC_READ, GL_STATIC_COPY, GL_DYNAMIC_DRAW, GL_DYNAMIC_READ, or GL_DYNAMIC_COPY.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glBufferData")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void BufferData<T2>(System.Graphics.ES20.BufferTarget target, IntPtr size, [InAttribute, OutAttribute] ref T2 data, System.Graphics.ES20.BufferUsage usage)
					where T2 : struct {
			GCHandle data_ptr = GCHandle.Alloc(data, GCHandleType.Pinned);
			try {
				Delegates.glBufferData((System.Graphics.ES20.BufferTarget) target, (IntPtr) size, (IntPtr) data_ptr.AddrOfPinnedObject(), (System.Graphics.ES20.BufferUsage) usage);
				data = (T2) data_ptr.Target;
			} finally {
				data_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Updates a subset of a buffer object's data store
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target buffer object. The symbolic constant must be GL_ARRAY_BUFFER, GL_COPY_READ_BUFFER, GL_COPY_WRITE_BUFFER, GL_ELEMENT_ARRAY_BUFFER, GL_PIXEL_PACK_BUFFER, GL_PIXEL_UNPACK_BUFFER, GL_TEXTURE_BUFFER, GL_TRANSFORM_FEEDBACK_BUFFER, or GL_UNIFORM_BUFFER.
		/// </para>
		/// </param>
		/// <param name="offset">
		/// <para>
		/// Specifies the offset into the buffer object's data store where data replacement will begin, measured in bytes.
		/// </para>
		/// </param>
		/// <param name="size">
		/// <para>
		/// Specifies the size in bytes of the data store region being replaced.
		/// </para>
		/// </param>
		/// <param name="data">
		/// <para>
		/// Specifies a pointer to the new data that will be copied into the data store.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glBufferSubData")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void BufferSubData(System.Graphics.ES20.BufferTarget target, IntPtr offset, IntPtr size, IntPtr data) {
			Delegates.glBufferSubData((System.Graphics.ES20.BufferTarget) target, (IntPtr) offset, (IntPtr) size, (IntPtr) data);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Updates a subset of a buffer object's data store
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target buffer object. The symbolic constant must be GL_ARRAY_BUFFER, GL_COPY_READ_BUFFER, GL_COPY_WRITE_BUFFER, GL_ELEMENT_ARRAY_BUFFER, GL_PIXEL_PACK_BUFFER, GL_PIXEL_UNPACK_BUFFER, GL_TEXTURE_BUFFER, GL_TRANSFORM_FEEDBACK_BUFFER, or GL_UNIFORM_BUFFER.
		/// </para>
		/// </param>
		/// <param name="offset">
		/// <para>
		/// Specifies the offset into the buffer object's data store where data replacement will begin, measured in bytes.
		/// </para>
		/// </param>
		/// <param name="size">
		/// <para>
		/// Specifies the size in bytes of the data store region being replaced.
		/// </para>
		/// </param>
		/// <param name="data">
		/// <para>
		/// Specifies a pointer to the new data that will be copied into the data store.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glBufferSubData")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void BufferSubData<T3>(System.Graphics.ES20.BufferTarget target, IntPtr offset, IntPtr size, [InAttribute, OutAttribute] T3[] data)
					where T3 : struct {
			GCHandle data_ptr = GCHandle.Alloc(data, GCHandleType.Pinned);
			try {
				Delegates.glBufferSubData((System.Graphics.ES20.BufferTarget) target, (IntPtr) offset, (IntPtr) size, (IntPtr) data_ptr.AddrOfPinnedObject());
			} finally {
				data_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Updates a subset of a buffer object's data store
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target buffer object. The symbolic constant must be GL_ARRAY_BUFFER, GL_COPY_READ_BUFFER, GL_COPY_WRITE_BUFFER, GL_ELEMENT_ARRAY_BUFFER, GL_PIXEL_PACK_BUFFER, GL_PIXEL_UNPACK_BUFFER, GL_TEXTURE_BUFFER, GL_TRANSFORM_FEEDBACK_BUFFER, or GL_UNIFORM_BUFFER.
		/// </para>
		/// </param>
		/// <param name="offset">
		/// <para>
		/// Specifies the offset into the buffer object's data store where data replacement will begin, measured in bytes.
		/// </para>
		/// </param>
		/// <param name="size">
		/// <para>
		/// Specifies the size in bytes of the data store region being replaced.
		/// </para>
		/// </param>
		/// <param name="data">
		/// <para>
		/// Specifies a pointer to the new data that will be copied into the data store.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glBufferSubData")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void BufferSubData<T3>(System.Graphics.ES20.BufferTarget target, IntPtr offset, IntPtr size, [InAttribute, OutAttribute] T3[,] data)
					where T3 : struct {
			GCHandle data_ptr = GCHandle.Alloc(data, GCHandleType.Pinned);
			try {
				Delegates.glBufferSubData((System.Graphics.ES20.BufferTarget) target, (IntPtr) offset, (IntPtr) size, (IntPtr) data_ptr.AddrOfPinnedObject());
			} finally {
				data_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Updates a subset of a buffer object's data store
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target buffer object. The symbolic constant must be GL_ARRAY_BUFFER, GL_COPY_READ_BUFFER, GL_COPY_WRITE_BUFFER, GL_ELEMENT_ARRAY_BUFFER, GL_PIXEL_PACK_BUFFER, GL_PIXEL_UNPACK_BUFFER, GL_TEXTURE_BUFFER, GL_TRANSFORM_FEEDBACK_BUFFER, or GL_UNIFORM_BUFFER.
		/// </para>
		/// </param>
		/// <param name="offset">
		/// <para>
		/// Specifies the offset into the buffer object's data store where data replacement will begin, measured in bytes.
		/// </para>
		/// </param>
		/// <param name="size">
		/// <para>
		/// Specifies the size in bytes of the data store region being replaced.
		/// </para>
		/// </param>
		/// <param name="data">
		/// <para>
		/// Specifies a pointer to the new data that will be copied into the data store.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glBufferSubData")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void BufferSubData<T3>(System.Graphics.ES20.BufferTarget target, IntPtr offset, IntPtr size, [InAttribute, OutAttribute] T3[,,] data)
					where T3 : struct {
			GCHandle data_ptr = GCHandle.Alloc(data, GCHandleType.Pinned);
			try {
				Delegates.glBufferSubData((System.Graphics.ES20.BufferTarget) target, (IntPtr) offset, (IntPtr) size, (IntPtr) data_ptr.AddrOfPinnedObject());
			} finally {
				data_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Updates a subset of a buffer object's data store
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target buffer object. The symbolic constant must be GL_ARRAY_BUFFER, GL_COPY_READ_BUFFER, GL_COPY_WRITE_BUFFER, GL_ELEMENT_ARRAY_BUFFER, GL_PIXEL_PACK_BUFFER, GL_PIXEL_UNPACK_BUFFER, GL_TEXTURE_BUFFER, GL_TRANSFORM_FEEDBACK_BUFFER, or GL_UNIFORM_BUFFER.
		/// </para>
		/// </param>
		/// <param name="offset">
		/// <para>
		/// Specifies the offset into the buffer object's data store where data replacement will begin, measured in bytes.
		/// </para>
		/// </param>
		/// <param name="size">
		/// <para>
		/// Specifies the size in bytes of the data store region being replaced.
		/// </para>
		/// </param>
		/// <param name="data">
		/// <para>
		/// Specifies a pointer to the new data that will be copied into the data store.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glBufferSubData")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void BufferSubData<T3>(System.Graphics.ES20.BufferTarget target, IntPtr offset, IntPtr size, [InAttribute, OutAttribute] ref T3 data)
					where T3 : struct {
			GCHandle data_ptr = GCHandle.Alloc(data, GCHandleType.Pinned);
			try {
				Delegates.glBufferSubData((System.Graphics.ES20.BufferTarget) target, (IntPtr) offset, (IntPtr) size, (IntPtr) data_ptr.AddrOfPinnedObject());
				data = (T3) data_ptr.Target;
			} finally {
				data_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Check the completeness status of a framebuffer
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specify the target of the framebuffer completeness check.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glCheckFramebufferStatus")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				System.Graphics.ES20.FramebufferErrorCode CheckFramebufferStatus(System.Graphics.ES20.FramebufferTarget target) {
			return Delegates.glCheckFramebufferStatus((System.Graphics.ES20.FramebufferTarget) target);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Clear buffers to preset values
		/// </summary>
		/// <param name="mask">
		/// <para>
		/// Bitwise OR of masks that indicate the buffers to be cleared. The three masks are GL_COLOR_BUFFER_BIT, GL_DEPTH_BUFFER_BIT, and GL_STENCIL_BUFFER_BIT.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glClear")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void Clear(System.Graphics.ES20.ClearBufferMask mask) {
			Delegates.glClear((System.Graphics.ES20.ClearBufferMask) mask);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify clear values for the color buffers
		/// </summary>
		/// <param name="red">
		/// <para>
		/// Specify the red, green, blue, and alpha values used when the color buffers are cleared. The initial values are all 0.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glClearColor")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void ClearColor(Single red, Single green, Single blue, Single alpha) {
			Delegates.glClearColor((Single) red, (Single) green, (Single) blue, (Single) alpha);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify the clear value for the depth buffer
		/// </summary>
		/// <param name="depth">
		/// <para>
		/// Specifies the depth value used when the depth buffer is cleared. The initial value is 1.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glClearDepthf")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void ClearDepth(Single depth) {
			Delegates.glClearDepthf((Single) depth);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify the clear value for the stencil buffer
		/// </summary>
		/// <param name="s">
		/// <para>
		/// Specifies the index used when the stencil buffer is cleared. The initial value is 0.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glClearStencil")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void ClearStencil(Int32 s) {
			Delegates.glClearStencil((Int32) s);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Enable and disable writing of frame buffer color components
		/// </summary>
		/// <param name="red">
		/// <para>
		/// Specify whether red, green, blue, and alpha can or cannot be written into the frame buffer. The initial values are all GL_TRUE, indicating that the color components can be written.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glColorMask")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void ColorMask(bool red, bool green, bool blue, bool alpha) {
			Delegates.glColorMask((bool) red, (bool) green, (bool) blue, (bool) alpha);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Compiles a shader object
		/// </summary>
		/// <param name="shader">
		/// <para>
		/// Specifies the shader object to be compiled.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glCompileShader")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void CompileShader(Int32 shader) {
			Delegates.glCompileShader((UInt32) shader);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Compiles a shader object
		/// </summary>
		/// <param name="shader">
		/// <para>
		/// Specifies the shader object to be compiled.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glCompileShader")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void CompileShader(UInt32 shader) {
			Delegates.glCompileShader((UInt32) shader);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify a two-dimensional texture image in a compressed format
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target texture. Must be GL_TEXTURE_2D, GL_PROXY_TEXTURE_2D, GL_TEXTURE_1D_ARRAY, GL_PROXY_TEXTURE_1D_ARRAY, GL_TEXTURE_CUBE_MAP_POSITIVE_X, GL_TEXTURE_CUBE_MAP_NEGATIVE_X, GL_TEXTURE_CUBE_MAP_POSITIVE_Y, GL_TEXTURE_CUBE_MAP_NEGATIVE_Y, GL_TEXTURE_CUBE_MAP_POSITIVE_Z, GL_TEXTURE_CUBE_MAP_NEGATIVE_Z, or GL_PROXY_TEXTURE_CUBE_MAP.
		/// </para>
		/// </param>
		/// <param name="level">
		/// <para>
		/// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap reduction image.
		/// </para>
		/// </param>
		/// <param name="internalformat">
		/// <para>
		/// Specifies the format of the compressed image data stored at address data.
		/// </para>
		/// </param>
		/// <param name="width">
		/// <para>
		/// Specifies the width of the texture image. All implementations support 2D texture images that are at least 64 texels wide and cube-mapped texture images that are at least 16 texels wide.
		/// </para>
		/// </param>
		/// <param name="height">
		/// <para>
		/// Specifies the height of the texture image. All implementations support 2D texture images that are at least 64 texels high and cube-mapped texture images that are at least 16 texels high.
		/// </para>
		/// </param>
		/// <param name="border">
		/// <para>
		/// This value must be 0.
		/// </para>
		/// </param>
		/// <param name="imageSize">
		/// <para>
		/// Specifies the number of unsigned bytes of image data starting at the address specified by data.
		/// </para>
		/// </param>
		/// <param name="data">
		/// <para>
		/// Specifies a pointer to the compressed image data in memory.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glCompressedTexImage2D")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void CompressedTexImage2D(System.Graphics.ES20.TextureTarget target, Int32 level, System.Graphics.ES20.PixelInternalFormat internalformat, Int32 width, Int32 height, Int32 border, Int32 imageSize, IntPtr data) {
			Delegates.glCompressedTexImage2D((System.Graphics.ES20.TextureTarget) target, (Int32) level, (System.Graphics.ES20.PixelInternalFormat) internalformat, (Int32) width, (Int32) height, (Int32) border, (Int32) imageSize, (IntPtr) data);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify a two-dimensional texture image in a compressed format
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target texture. Must be GL_TEXTURE_2D, GL_PROXY_TEXTURE_2D, GL_TEXTURE_1D_ARRAY, GL_PROXY_TEXTURE_1D_ARRAY, GL_TEXTURE_CUBE_MAP_POSITIVE_X, GL_TEXTURE_CUBE_MAP_NEGATIVE_X, GL_TEXTURE_CUBE_MAP_POSITIVE_Y, GL_TEXTURE_CUBE_MAP_NEGATIVE_Y, GL_TEXTURE_CUBE_MAP_POSITIVE_Z, GL_TEXTURE_CUBE_MAP_NEGATIVE_Z, or GL_PROXY_TEXTURE_CUBE_MAP.
		/// </para>
		/// </param>
		/// <param name="level">
		/// <para>
		/// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap reduction image.
		/// </para>
		/// </param>
		/// <param name="internalformat">
		/// <para>
		/// Specifies the format of the compressed image data stored at address data.
		/// </para>
		/// </param>
		/// <param name="width">
		/// <para>
		/// Specifies the width of the texture image. All implementations support 2D texture images that are at least 64 texels wide and cube-mapped texture images that are at least 16 texels wide.
		/// </para>
		/// </param>
		/// <param name="height">
		/// <para>
		/// Specifies the height of the texture image. All implementations support 2D texture images that are at least 64 texels high and cube-mapped texture images that are at least 16 texels high.
		/// </para>
		/// </param>
		/// <param name="border">
		/// <para>
		/// This value must be 0.
		/// </para>
		/// </param>
		/// <param name="imageSize">
		/// <para>
		/// Specifies the number of unsigned bytes of image data starting at the address specified by data.
		/// </para>
		/// </param>
		/// <param name="data">
		/// <para>
		/// Specifies a pointer to the compressed image data in memory.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glCompressedTexImage2D")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void CompressedTexImage2D<T7>(System.Graphics.ES20.TextureTarget target, Int32 level, System.Graphics.ES20.PixelInternalFormat internalformat, Int32 width, Int32 height, Int32 border, Int32 imageSize, [InAttribute, OutAttribute] T7[] data)
					where T7 : struct {
			GCHandle data_ptr = GCHandle.Alloc(data, GCHandleType.Pinned);
			try {
				Delegates.glCompressedTexImage2D((System.Graphics.ES20.TextureTarget) target, (Int32) level, (System.Graphics.ES20.PixelInternalFormat) internalformat, (Int32) width, (Int32) height, (Int32) border, (Int32) imageSize, (IntPtr) data_ptr.AddrOfPinnedObject());
			} finally {
				data_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify a two-dimensional texture image in a compressed format
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target texture. Must be GL_TEXTURE_2D, GL_PROXY_TEXTURE_2D, GL_TEXTURE_1D_ARRAY, GL_PROXY_TEXTURE_1D_ARRAY, GL_TEXTURE_CUBE_MAP_POSITIVE_X, GL_TEXTURE_CUBE_MAP_NEGATIVE_X, GL_TEXTURE_CUBE_MAP_POSITIVE_Y, GL_TEXTURE_CUBE_MAP_NEGATIVE_Y, GL_TEXTURE_CUBE_MAP_POSITIVE_Z, GL_TEXTURE_CUBE_MAP_NEGATIVE_Z, or GL_PROXY_TEXTURE_CUBE_MAP.
		/// </para>
		/// </param>
		/// <param name="level">
		/// <para>
		/// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap reduction image.
		/// </para>
		/// </param>
		/// <param name="internalformat">
		/// <para>
		/// Specifies the format of the compressed image data stored at address data.
		/// </para>
		/// </param>
		/// <param name="width">
		/// <para>
		/// Specifies the width of the texture image. All implementations support 2D texture images that are at least 64 texels wide and cube-mapped texture images that are at least 16 texels wide.
		/// </para>
		/// </param>
		/// <param name="height">
		/// <para>
		/// Specifies the height of the texture image. All implementations support 2D texture images that are at least 64 texels high and cube-mapped texture images that are at least 16 texels high.
		/// </para>
		/// </param>
		/// <param name="border">
		/// <para>
		/// This value must be 0.
		/// </para>
		/// </param>
		/// <param name="imageSize">
		/// <para>
		/// Specifies the number of unsigned bytes of image data starting at the address specified by data.
		/// </para>
		/// </param>
		/// <param name="data">
		/// <para>
		/// Specifies a pointer to the compressed image data in memory.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glCompressedTexImage2D")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void CompressedTexImage2D<T7>(System.Graphics.ES20.TextureTarget target, Int32 level, System.Graphics.ES20.PixelInternalFormat internalformat, Int32 width, Int32 height, Int32 border, Int32 imageSize, [InAttribute, OutAttribute] T7[,] data)
					where T7 : struct {
			GCHandle data_ptr = GCHandle.Alloc(data, GCHandleType.Pinned);
			try {
				Delegates.glCompressedTexImage2D((System.Graphics.ES20.TextureTarget) target, (Int32) level, (System.Graphics.ES20.PixelInternalFormat) internalformat, (Int32) width, (Int32) height, (Int32) border, (Int32) imageSize, (IntPtr) data_ptr.AddrOfPinnedObject());
			} finally {
				data_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify a two-dimensional texture image in a compressed format
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target texture. Must be GL_TEXTURE_2D, GL_PROXY_TEXTURE_2D, GL_TEXTURE_1D_ARRAY, GL_PROXY_TEXTURE_1D_ARRAY, GL_TEXTURE_CUBE_MAP_POSITIVE_X, GL_TEXTURE_CUBE_MAP_NEGATIVE_X, GL_TEXTURE_CUBE_MAP_POSITIVE_Y, GL_TEXTURE_CUBE_MAP_NEGATIVE_Y, GL_TEXTURE_CUBE_MAP_POSITIVE_Z, GL_TEXTURE_CUBE_MAP_NEGATIVE_Z, or GL_PROXY_TEXTURE_CUBE_MAP.
		/// </para>
		/// </param>
		/// <param name="level">
		/// <para>
		/// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap reduction image.
		/// </para>
		/// </param>
		/// <param name="internalformat">
		/// <para>
		/// Specifies the format of the compressed image data stored at address data.
		/// </para>
		/// </param>
		/// <param name="width">
		/// <para>
		/// Specifies the width of the texture image. All implementations support 2D texture images that are at least 64 texels wide and cube-mapped texture images that are at least 16 texels wide.
		/// </para>
		/// </param>
		/// <param name="height">
		/// <para>
		/// Specifies the height of the texture image. All implementations support 2D texture images that are at least 64 texels high and cube-mapped texture images that are at least 16 texels high.
		/// </para>
		/// </param>
		/// <param name="border">
		/// <para>
		/// This value must be 0.
		/// </para>
		/// </param>
		/// <param name="imageSize">
		/// <para>
		/// Specifies the number of unsigned bytes of image data starting at the address specified by data.
		/// </para>
		/// </param>
		/// <param name="data">
		/// <para>
		/// Specifies a pointer to the compressed image data in memory.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glCompressedTexImage2D")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void CompressedTexImage2D<T7>(System.Graphics.ES20.TextureTarget target, Int32 level, System.Graphics.ES20.PixelInternalFormat internalformat, Int32 width, Int32 height, Int32 border, Int32 imageSize, [InAttribute, OutAttribute] T7[,,] data)
					where T7 : struct {
			GCHandle data_ptr = GCHandle.Alloc(data, GCHandleType.Pinned);
			try {
				Delegates.glCompressedTexImage2D((System.Graphics.ES20.TextureTarget) target, (Int32) level, (System.Graphics.ES20.PixelInternalFormat) internalformat, (Int32) width, (Int32) height, (Int32) border, (Int32) imageSize, (IntPtr) data_ptr.AddrOfPinnedObject());
			} finally {
				data_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify a two-dimensional texture image in a compressed format
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target texture. Must be GL_TEXTURE_2D, GL_PROXY_TEXTURE_2D, GL_TEXTURE_1D_ARRAY, GL_PROXY_TEXTURE_1D_ARRAY, GL_TEXTURE_CUBE_MAP_POSITIVE_X, GL_TEXTURE_CUBE_MAP_NEGATIVE_X, GL_TEXTURE_CUBE_MAP_POSITIVE_Y, GL_TEXTURE_CUBE_MAP_NEGATIVE_Y, GL_TEXTURE_CUBE_MAP_POSITIVE_Z, GL_TEXTURE_CUBE_MAP_NEGATIVE_Z, or GL_PROXY_TEXTURE_CUBE_MAP.
		/// </para>
		/// </param>
		/// <param name="level">
		/// <para>
		/// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap reduction image.
		/// </para>
		/// </param>
		/// <param name="internalformat">
		/// <para>
		/// Specifies the format of the compressed image data stored at address data.
		/// </para>
		/// </param>
		/// <param name="width">
		/// <para>
		/// Specifies the width of the texture image. All implementations support 2D texture images that are at least 64 texels wide and cube-mapped texture images that are at least 16 texels wide.
		/// </para>
		/// </param>
		/// <param name="height">
		/// <para>
		/// Specifies the height of the texture image. All implementations support 2D texture images that are at least 64 texels high and cube-mapped texture images that are at least 16 texels high.
		/// </para>
		/// </param>
		/// <param name="border">
		/// <para>
		/// This value must be 0.
		/// </para>
		/// </param>
		/// <param name="imageSize">
		/// <para>
		/// Specifies the number of unsigned bytes of image data starting at the address specified by data.
		/// </para>
		/// </param>
		/// <param name="data">
		/// <para>
		/// Specifies a pointer to the compressed image data in memory.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glCompressedTexImage2D")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void CompressedTexImage2D<T7>(System.Graphics.ES20.TextureTarget target, Int32 level, System.Graphics.ES20.PixelInternalFormat internalformat, Int32 width, Int32 height, Int32 border, Int32 imageSize, [InAttribute, OutAttribute] ref T7 data)
					where T7 : struct {
			GCHandle data_ptr = GCHandle.Alloc(data, GCHandleType.Pinned);
			try {
				Delegates.glCompressedTexImage2D((System.Graphics.ES20.TextureTarget) target, (Int32) level, (System.Graphics.ES20.PixelInternalFormat) internalformat, (Int32) width, (Int32) height, (Int32) border, (Int32) imageSize, (IntPtr) data_ptr.AddrOfPinnedObject());
				data = (T7) data_ptr.Target;
			} finally {
				data_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify a two-dimensional texture subimage in a compressed format
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target texture. Must be GL_TEXTURE_2D, GL_TEXTURE_CUBE_MAP_POSITIVE_X, GL_TEXTURE_CUBE_MAP_NEGATIVE_X, GL_TEXTURE_CUBE_MAP_POSITIVE_Y, GL_TEXTURE_CUBE_MAP_NEGATIVE_Y, GL_TEXTURE_CUBE_MAP_POSITIVE_Z, or GL_TEXTURE_CUBE_MAP_NEGATIVE_Z.
		/// </para>
		/// </param>
		/// <param name="level">
		/// <para>
		/// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap reduction image.
		/// </para>
		/// </param>
		/// <param name="xoffset">
		/// <para>
		/// Specifies a texel offset in the x direction within the texture array.
		/// </para>
		/// </param>
		/// <param name="yoffset">
		/// <para>
		/// Specifies a texel offset in the y direction within the texture array.
		/// </para>
		/// </param>
		/// <param name="width">
		/// <para>
		/// Specifies the width of the texture subimage.
		/// </para>
		/// </param>
		/// <param name="height">
		/// <para>
		/// Specifies the height of the texture subimage.
		/// </para>
		/// </param>
		/// <param name="format">
		/// <para>
		/// Specifies the format of the compressed image data stored at address data.
		/// </para>
		/// </param>
		/// <param name="imageSize">
		/// <para>
		/// Specifies the number of unsigned bytes of image data starting at the address specified by data.
		/// </para>
		/// </param>
		/// <param name="data">
		/// <para>
		/// Specifies a pointer to the compressed image data in memory.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glCompressedTexSubImage2D")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void CompressedTexSubImage2D(System.Graphics.ES20.TextureTarget target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 width, Int32 height, System.Graphics.ES20.PixelFormat format, Int32 imageSize, IntPtr data) {
			Delegates.glCompressedTexSubImage2D((System.Graphics.ES20.TextureTarget) target, (Int32) level, (Int32) xoffset, (Int32) yoffset, (Int32) width, (Int32) height, (System.Graphics.ES20.PixelFormat) format, (Int32) imageSize, (IntPtr) data);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify a two-dimensional texture subimage in a compressed format
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target texture. Must be GL_TEXTURE_2D, GL_TEXTURE_CUBE_MAP_POSITIVE_X, GL_TEXTURE_CUBE_MAP_NEGATIVE_X, GL_TEXTURE_CUBE_MAP_POSITIVE_Y, GL_TEXTURE_CUBE_MAP_NEGATIVE_Y, GL_TEXTURE_CUBE_MAP_POSITIVE_Z, or GL_TEXTURE_CUBE_MAP_NEGATIVE_Z.
		/// </para>
		/// </param>
		/// <param name="level">
		/// <para>
		/// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap reduction image.
		/// </para>
		/// </param>
		/// <param name="xoffset">
		/// <para>
		/// Specifies a texel offset in the x direction within the texture array.
		/// </para>
		/// </param>
		/// <param name="yoffset">
		/// <para>
		/// Specifies a texel offset in the y direction within the texture array.
		/// </para>
		/// </param>
		/// <param name="width">
		/// <para>
		/// Specifies the width of the texture subimage.
		/// </para>
		/// </param>
		/// <param name="height">
		/// <para>
		/// Specifies the height of the texture subimage.
		/// </para>
		/// </param>
		/// <param name="format">
		/// <para>
		/// Specifies the format of the compressed image data stored at address data.
		/// </para>
		/// </param>
		/// <param name="imageSize">
		/// <para>
		/// Specifies the number of unsigned bytes of image data starting at the address specified by data.
		/// </para>
		/// </param>
		/// <param name="data">
		/// <para>
		/// Specifies a pointer to the compressed image data in memory.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glCompressedTexSubImage2D")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void CompressedTexSubImage2D<T8>(System.Graphics.ES20.TextureTarget target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 width, Int32 height, System.Graphics.ES20.PixelFormat format, Int32 imageSize, [InAttribute, OutAttribute] T8[] data)
					where T8 : struct {
			GCHandle data_ptr = GCHandle.Alloc(data, GCHandleType.Pinned);
			try {
				Delegates.glCompressedTexSubImage2D((System.Graphics.ES20.TextureTarget) target, (Int32) level, (Int32) xoffset, (Int32) yoffset, (Int32) width, (Int32) height, (System.Graphics.ES20.PixelFormat) format, (Int32) imageSize, (IntPtr) data_ptr.AddrOfPinnedObject());
			} finally {
				data_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify a two-dimensional texture subimage in a compressed format
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target texture. Must be GL_TEXTURE_2D, GL_TEXTURE_CUBE_MAP_POSITIVE_X, GL_TEXTURE_CUBE_MAP_NEGATIVE_X, GL_TEXTURE_CUBE_MAP_POSITIVE_Y, GL_TEXTURE_CUBE_MAP_NEGATIVE_Y, GL_TEXTURE_CUBE_MAP_POSITIVE_Z, or GL_TEXTURE_CUBE_MAP_NEGATIVE_Z.
		/// </para>
		/// </param>
		/// <param name="level">
		/// <para>
		/// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap reduction image.
		/// </para>
		/// </param>
		/// <param name="xoffset">
		/// <para>
		/// Specifies a texel offset in the x direction within the texture array.
		/// </para>
		/// </param>
		/// <param name="yoffset">
		/// <para>
		/// Specifies a texel offset in the y direction within the texture array.
		/// </para>
		/// </param>
		/// <param name="width">
		/// <para>
		/// Specifies the width of the texture subimage.
		/// </para>
		/// </param>
		/// <param name="height">
		/// <para>
		/// Specifies the height of the texture subimage.
		/// </para>
		/// </param>
		/// <param name="format">
		/// <para>
		/// Specifies the format of the compressed image data stored at address data.
		/// </para>
		/// </param>
		/// <param name="imageSize">
		/// <para>
		/// Specifies the number of unsigned bytes of image data starting at the address specified by data.
		/// </para>
		/// </param>
		/// <param name="data">
		/// <para>
		/// Specifies a pointer to the compressed image data in memory.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glCompressedTexSubImage2D")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void CompressedTexSubImage2D<T8>(System.Graphics.ES20.TextureTarget target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 width, Int32 height, System.Graphics.ES20.PixelFormat format, Int32 imageSize, [InAttribute, OutAttribute] T8[,] data)
					where T8 : struct {
			GCHandle data_ptr = GCHandle.Alloc(data, GCHandleType.Pinned);
			try {
				Delegates.glCompressedTexSubImage2D((System.Graphics.ES20.TextureTarget) target, (Int32) level, (Int32) xoffset, (Int32) yoffset, (Int32) width, (Int32) height, (System.Graphics.ES20.PixelFormat) format, (Int32) imageSize, (IntPtr) data_ptr.AddrOfPinnedObject());
			} finally {
				data_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify a two-dimensional texture subimage in a compressed format
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target texture. Must be GL_TEXTURE_2D, GL_TEXTURE_CUBE_MAP_POSITIVE_X, GL_TEXTURE_CUBE_MAP_NEGATIVE_X, GL_TEXTURE_CUBE_MAP_POSITIVE_Y, GL_TEXTURE_CUBE_MAP_NEGATIVE_Y, GL_TEXTURE_CUBE_MAP_POSITIVE_Z, or GL_TEXTURE_CUBE_MAP_NEGATIVE_Z.
		/// </para>
		/// </param>
		/// <param name="level">
		/// <para>
		/// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap reduction image.
		/// </para>
		/// </param>
		/// <param name="xoffset">
		/// <para>
		/// Specifies a texel offset in the x direction within the texture array.
		/// </para>
		/// </param>
		/// <param name="yoffset">
		/// <para>
		/// Specifies a texel offset in the y direction within the texture array.
		/// </para>
		/// </param>
		/// <param name="width">
		/// <para>
		/// Specifies the width of the texture subimage.
		/// </para>
		/// </param>
		/// <param name="height">
		/// <para>
		/// Specifies the height of the texture subimage.
		/// </para>
		/// </param>
		/// <param name="format">
		/// <para>
		/// Specifies the format of the compressed image data stored at address data.
		/// </para>
		/// </param>
		/// <param name="imageSize">
		/// <para>
		/// Specifies the number of unsigned bytes of image data starting at the address specified by data.
		/// </para>
		/// </param>
		/// <param name="data">
		/// <para>
		/// Specifies a pointer to the compressed image data in memory.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glCompressedTexSubImage2D")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void CompressedTexSubImage2D<T8>(System.Graphics.ES20.TextureTarget target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 width, Int32 height, System.Graphics.ES20.PixelFormat format, Int32 imageSize, [InAttribute, OutAttribute] T8[,,] data)
					where T8 : struct {
			GCHandle data_ptr = GCHandle.Alloc(data, GCHandleType.Pinned);
			try {
				Delegates.glCompressedTexSubImage2D((System.Graphics.ES20.TextureTarget) target, (Int32) level, (Int32) xoffset, (Int32) yoffset, (Int32) width, (Int32) height, (System.Graphics.ES20.PixelFormat) format, (Int32) imageSize, (IntPtr) data_ptr.AddrOfPinnedObject());
			} finally {
				data_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify a two-dimensional texture subimage in a compressed format
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target texture. Must be GL_TEXTURE_2D, GL_TEXTURE_CUBE_MAP_POSITIVE_X, GL_TEXTURE_CUBE_MAP_NEGATIVE_X, GL_TEXTURE_CUBE_MAP_POSITIVE_Y, GL_TEXTURE_CUBE_MAP_NEGATIVE_Y, GL_TEXTURE_CUBE_MAP_POSITIVE_Z, or GL_TEXTURE_CUBE_MAP_NEGATIVE_Z.
		/// </para>
		/// </param>
		/// <param name="level">
		/// <para>
		/// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap reduction image.
		/// </para>
		/// </param>
		/// <param name="xoffset">
		/// <para>
		/// Specifies a texel offset in the x direction within the texture array.
		/// </para>
		/// </param>
		/// <param name="yoffset">
		/// <para>
		/// Specifies a texel offset in the y direction within the texture array.
		/// </para>
		/// </param>
		/// <param name="width">
		/// <para>
		/// Specifies the width of the texture subimage.
		/// </para>
		/// </param>
		/// <param name="height">
		/// <para>
		/// Specifies the height of the texture subimage.
		/// </para>
		/// </param>
		/// <param name="format">
		/// <para>
		/// Specifies the format of the compressed image data stored at address data.
		/// </para>
		/// </param>
		/// <param name="imageSize">
		/// <para>
		/// Specifies the number of unsigned bytes of image data starting at the address specified by data.
		/// </para>
		/// </param>
		/// <param name="data">
		/// <para>
		/// Specifies a pointer to the compressed image data in memory.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glCompressedTexSubImage2D")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void CompressedTexSubImage2D<T8>(System.Graphics.ES20.TextureTarget target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 width, Int32 height, System.Graphics.ES20.PixelFormat format, Int32 imageSize, [InAttribute, OutAttribute] ref T8 data)
					where T8 : struct {
			GCHandle data_ptr = GCHandle.Alloc(data, GCHandleType.Pinned);
			try {
				Delegates.glCompressedTexSubImage2D((System.Graphics.ES20.TextureTarget) target, (Int32) level, (Int32) xoffset, (Int32) yoffset, (Int32) width, (Int32) height, (System.Graphics.ES20.PixelFormat) format, (Int32) imageSize, (IntPtr) data_ptr.AddrOfPinnedObject());
				data = (T8) data_ptr.Target;
			} finally {
				data_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Copy pixels into a 2D texture image
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target texture. Must be GL_TEXTURE_2D, GL_TEXTURE_CUBE_MAP_POSITIVE_X, GL_TEXTURE_CUBE_MAP_NEGATIVE_X, GL_TEXTURE_CUBE_MAP_POSITIVE_Y, GL_TEXTURE_CUBE_MAP_NEGATIVE_Y, GL_TEXTURE_CUBE_MAP_POSITIVE_Z, or GL_TEXTURE_CUBE_MAP_NEGATIVE_Z.
		/// </para>
		/// </param>
		/// <param name="level">
		/// <para>
		/// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap reduction image.
		/// </para>
		/// </param>
		/// <param name="internalformat">
		/// <para>
		/// Specifies the internal format of the texture. Must be one of the following symbolic constants: GL_COMPRESSED_RED, GL_COMPRESSED_RG, GL_COMPRESSED_RGB, GL_COMPRESSED_RGBA. GL_COMPRESSED_SRGB, GL_COMPRESSED_SRGB_ALPHA. GL_DEPTH_COMPONENT, GL_DEPTH_COMPONENT16, GL_DEPTH_COMPONENT24, GL_DEPTH_COMPONENT32, GL_RED, GL_RG, GL_RGB, GL_R3_G3_B2, GL_RGB4, GL_RGB5, GL_RGB8, GL_RGB10, GL_RGB12, GL_RGB16, GL_RGBA, GL_RGBA2, GL_RGBA4, GL_RGB5_A1, GL_RGBA8, GL_RGB10_A2, GL_RGBA12, GL_RGBA16, GL_SRGB, GL_SRGB8, GL_SRGB_ALPHA, or GL_SRGB8_ALPHA8.
		/// </para>
		/// </param>
		/// <param name="x">
		/// <para>
		/// Specify the window coordinates of the lower left corner of the rectangular region of pixels to be copied.
		/// </para>
		/// </param>
		/// <param name="width">
		/// <para>
		/// Specifies the width of the texture image. Must be 0 or 2 sup n + 2 ( border ) for some integer .
		/// </para>
		/// </param>
		/// <param name="height">
		/// <para>
		/// Specifies the height of the texture image. Must be 0 or 2 sup m + 2 ( border ) for some integer .
		/// </para>
		/// </param>
		/// <param name="border">
		/// <para>
		/// Specifies the width of the border. Must be either 0 or 1.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glCopyTexImage2D")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void CopyTexImage2D(System.Graphics.ES20.TextureTarget target, Int32 level, System.Graphics.ES20.PixelInternalFormat internalformat, Int32 x, Int32 y, Int32 width, Int32 height, Int32 border) {
			Delegates.glCopyTexImage2D((System.Graphics.ES20.TextureTarget) target, (Int32) level, (System.Graphics.ES20.PixelInternalFormat) internalformat, (Int32) x, (Int32) y, (Int32) width, (Int32) height, (Int32) border);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Copy a two-dimensional texture subimage
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target texture. Must be GL_TEXTURE_2D, GL_TEXTURE_CUBE_MAP_POSITIVE_X, GL_TEXTURE_CUBE_MAP_NEGATIVE_X, GL_TEXTURE_CUBE_MAP_POSITIVE_Y, GL_TEXTURE_CUBE_MAP_NEGATIVE_Y, GL_TEXTURE_CUBE_MAP_POSITIVE_Z, or GL_TEXTURE_CUBE_MAP_NEGATIVE_Z.
		/// </para>
		/// </param>
		/// <param name="level">
		/// <para>
		/// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap reduction image.
		/// </para>
		/// </param>
		/// <param name="xoffset">
		/// <para>
		/// Specifies a texel offset in the x direction within the texture array.
		/// </para>
		/// </param>
		/// <param name="yoffset">
		/// <para>
		/// Specifies a texel offset in the y direction within the texture array.
		/// </para>
		/// </param>
		/// <param name="x">
		/// <para>
		/// Specify the window coordinates of the lower left corner of the rectangular region of pixels to be copied.
		/// </para>
		/// </param>
		/// <param name="width">
		/// <para>
		/// Specifies the width of the texture subimage.
		/// </para>
		/// </param>
		/// <param name="height">
		/// <para>
		/// Specifies the height of the texture subimage.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glCopyTexSubImage2D")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void CopyTexSubImage2D(System.Graphics.ES20.TextureTarget target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 x, Int32 y, Int32 width, Int32 height) {
			Delegates.glCopyTexSubImage2D((System.Graphics.ES20.TextureTarget) target, (Int32) level, (Int32) xoffset, (Int32) yoffset, (Int32) x, (Int32) y, (Int32) width, (Int32) height);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Creates a program object
		/// </summary>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glCreateProgram")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				Int32 CreateProgram() {
			return Delegates.glCreateProgram();
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Creates a shader object
		/// </summary>
		/// <param name="shaderType">
		/// <para>
		/// Specifies the type of shader to be created. Must be one of GL_VERTEX_SHADER, GL_TESS_CONTROL_SHADER, GL_TESS_EVALUATION_SHADER, GL_GEOMETRY_SHADER, or GL_FRAGMENT_SHADER.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glCreateShader")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				Int32 CreateShader(System.Graphics.ES20.ShaderType type) {
			return Delegates.glCreateShader((System.Graphics.ES20.ShaderType) type);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify whether front- or back-facing facets can be culled
		/// </summary>
		/// <param name="mode">
		/// <para>
		/// Specifies whether front- or back-facing facets are candidates for culling. Symbolic constants GL_FRONT, GL_BACK, and GL_FRONT_AND_BACK are accepted. The initial value is GL_BACK.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glCullFace")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void CullFace(System.Graphics.ES20.CullFaceMode mode) {
			Delegates.glCullFace((System.Graphics.ES20.CullFaceMode) mode);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Delete named buffer objects
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of buffer objects to be deleted.
		/// </para>
		/// </param>
		/// <param name="buffers">
		/// <para>
		/// Specifies an array of buffer objects to be deleted.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeleteBuffers")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void DeleteBuffers(Int32 n, Int32[] buffers) {
			unsafe
			{
				fixed (Int32* buffers_ptr = buffers) {
					Delegates.glDeleteBuffers((Int32) n, (UInt32*) buffers_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Delete named buffer objects
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of buffer objects to be deleted.
		/// </para>
		/// </param>
		/// <param name="buffers">
		/// <para>
		/// Specifies an array of buffer objects to be deleted.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeleteBuffers")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void DeleteBuffers(Int32 n, ref Int32 buffers) {
			unsafe
			{
				fixed (Int32* buffers_ptr = &buffers) {
					Delegates.glDeleteBuffers((Int32) n, (UInt32*) buffers_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Delete named buffer objects
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of buffer objects to be deleted.
		/// </para>
		/// </param>
		/// <param name="buffers">
		/// <para>
		/// Specifies an array of buffer objects to be deleted.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeleteBuffers")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void DeleteBuffers(Int32 n, Int32* buffers) {
			Delegates.glDeleteBuffers((Int32) n, (UInt32*) buffers);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Delete named buffer objects
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of buffer objects to be deleted.
		/// </para>
		/// </param>
		/// <param name="buffers">
		/// <para>
		/// Specifies an array of buffer objects to be deleted.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeleteBuffers")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void DeleteBuffers(Int32 n, UInt32[] buffers) {
			unsafe
			{
				fixed (UInt32* buffers_ptr = buffers) {
					Delegates.glDeleteBuffers((Int32) n, (UInt32*) buffers_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Delete named buffer objects
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of buffer objects to be deleted.
		/// </para>
		/// </param>
		/// <param name="buffers">
		/// <para>
		/// Specifies an array of buffer objects to be deleted.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeleteBuffers")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void DeleteBuffers(Int32 n, ref UInt32 buffers) {
			unsafe
			{
				fixed (UInt32* buffers_ptr = &buffers) {
					Delegates.glDeleteBuffers((Int32) n, (UInt32*) buffers_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Delete named buffer objects
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of buffer objects to be deleted.
		/// </para>
		/// </param>
		/// <param name="buffers">
		/// <para>
		/// Specifies an array of buffer objects to be deleted.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeleteBuffers")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void DeleteBuffers(Int32 n, UInt32* buffers) {
			Delegates.glDeleteBuffers((Int32) n, (UInt32*) buffers);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Delete framebuffer objects
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of framebuffer objects to be deleted.
		/// </para>
		/// </param>
		/// <param name="framebuffers">
		/// <para>
		/// A pointer to an array containing n framebuffer objects to be deleted.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeleteFramebuffers")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void DeleteFramebuffers(Int32 n, Int32[] framebuffers) {
			unsafe
			{
				fixed (Int32* framebuffers_ptr = framebuffers) {
					Delegates.glDeleteFramebuffers((Int32) n, (UInt32*) framebuffers_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Delete framebuffer objects
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of framebuffer objects to be deleted.
		/// </para>
		/// </param>
		/// <param name="framebuffers">
		/// <para>
		/// A pointer to an array containing n framebuffer objects to be deleted.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeleteFramebuffers")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void DeleteFramebuffers(Int32 n, ref Int32 framebuffers) {
			unsafe
			{
				fixed (Int32* framebuffers_ptr = &framebuffers) {
					Delegates.glDeleteFramebuffers((Int32) n, (UInt32*) framebuffers_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Delete framebuffer objects
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of framebuffer objects to be deleted.
		/// </para>
		/// </param>
		/// <param name="framebuffers">
		/// <para>
		/// A pointer to an array containing n framebuffer objects to be deleted.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeleteFramebuffers")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void DeleteFramebuffers(Int32 n, Int32* framebuffers) {
			Delegates.glDeleteFramebuffers((Int32) n, (UInt32*) framebuffers);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Delete framebuffer objects
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of framebuffer objects to be deleted.
		/// </para>
		/// </param>
		/// <param name="framebuffers">
		/// <para>
		/// A pointer to an array containing n framebuffer objects to be deleted.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeleteFramebuffers")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void DeleteFramebuffers(Int32 n, UInt32[] framebuffers) {
			unsafe
			{
				fixed (UInt32* framebuffers_ptr = framebuffers) {
					Delegates.glDeleteFramebuffers((Int32) n, (UInt32*) framebuffers_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Delete framebuffer objects
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of framebuffer objects to be deleted.
		/// </para>
		/// </param>
		/// <param name="framebuffers">
		/// <para>
		/// A pointer to an array containing n framebuffer objects to be deleted.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeleteFramebuffers")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void DeleteFramebuffers(Int32 n, ref UInt32 framebuffers) {
			unsafe
			{
				fixed (UInt32* framebuffers_ptr = &framebuffers) {
					Delegates.glDeleteFramebuffers((Int32) n, (UInt32*) framebuffers_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Delete framebuffer objects
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of framebuffer objects to be deleted.
		/// </para>
		/// </param>
		/// <param name="framebuffers">
		/// <para>
		/// A pointer to an array containing n framebuffer objects to be deleted.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeleteFramebuffers")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void DeleteFramebuffers(Int32 n, UInt32* framebuffers) {
			Delegates.glDeleteFramebuffers((Int32) n, (UInt32*) framebuffers);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Deletes a program object
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to be deleted.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeleteProgram")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void DeleteProgram(Int32 program) {
			Delegates.glDeleteProgram((UInt32) program);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Deletes a program object
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to be deleted.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeleteProgram")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void DeleteProgram(UInt32 program) {
			Delegates.glDeleteProgram((UInt32) program);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Delete renderbuffer objects
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of renderbuffer objects to be deleted.
		/// </para>
		/// </param>
		/// <param name="renderbuffers">
		/// <para>
		/// A pointer to an array containing n renderbuffer objects to be deleted.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeleteRenderbuffers")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void DeleteRenderbuffers(Int32 n, Int32[] renderbuffers) {
			unsafe
			{
				fixed (Int32* renderbuffers_ptr = renderbuffers) {
					Delegates.glDeleteRenderbuffers((Int32) n, (UInt32*) renderbuffers_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Delete renderbuffer objects
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of renderbuffer objects to be deleted.
		/// </para>
		/// </param>
		/// <param name="renderbuffers">
		/// <para>
		/// A pointer to an array containing n renderbuffer objects to be deleted.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeleteRenderbuffers")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void DeleteRenderbuffers(Int32 n, ref Int32 renderbuffers) {
			unsafe
			{
				fixed (Int32* renderbuffers_ptr = &renderbuffers) {
					Delegates.glDeleteRenderbuffers((Int32) n, (UInt32*) renderbuffers_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Delete renderbuffer objects
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of renderbuffer objects to be deleted.
		/// </para>
		/// </param>
		/// <param name="renderbuffers">
		/// <para>
		/// A pointer to an array containing n renderbuffer objects to be deleted.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeleteRenderbuffers")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void DeleteRenderbuffers(Int32 n, Int32* renderbuffers) {
			Delegates.glDeleteRenderbuffers((Int32) n, (UInt32*) renderbuffers);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Delete renderbuffer objects
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of renderbuffer objects to be deleted.
		/// </para>
		/// </param>
		/// <param name="renderbuffers">
		/// <para>
		/// A pointer to an array containing n renderbuffer objects to be deleted.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeleteRenderbuffers")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void DeleteRenderbuffers(Int32 n, UInt32[] renderbuffers) {
			unsafe
			{
				fixed (UInt32* renderbuffers_ptr = renderbuffers) {
					Delegates.glDeleteRenderbuffers((Int32) n, (UInt32*) renderbuffers_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Delete renderbuffer objects
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of renderbuffer objects to be deleted.
		/// </para>
		/// </param>
		/// <param name="renderbuffers">
		/// <para>
		/// A pointer to an array containing n renderbuffer objects to be deleted.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeleteRenderbuffers")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void DeleteRenderbuffers(Int32 n, ref UInt32 renderbuffers) {
			unsafe
			{
				fixed (UInt32* renderbuffers_ptr = &renderbuffers) {
					Delegates.glDeleteRenderbuffers((Int32) n, (UInt32*) renderbuffers_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Delete renderbuffer objects
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of renderbuffer objects to be deleted.
		/// </para>
		/// </param>
		/// <param name="renderbuffers">
		/// <para>
		/// A pointer to an array containing n renderbuffer objects to be deleted.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeleteRenderbuffers")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void DeleteRenderbuffers(Int32 n, UInt32* renderbuffers) {
			Delegates.glDeleteRenderbuffers((Int32) n, (UInt32*) renderbuffers);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Deletes a shader object
		/// </summary>
		/// <param name="shader">
		/// <para>
		/// Specifies the shader object to be deleted.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeleteShader")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void DeleteShader(Int32 shader) {
			Delegates.glDeleteShader((UInt32) shader);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Deletes a shader object
		/// </summary>
		/// <param name="shader">
		/// <para>
		/// Specifies the shader object to be deleted.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeleteShader")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void DeleteShader(UInt32 shader) {
			Delegates.glDeleteShader((UInt32) shader);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Delete named textures
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of textures to be deleted.
		/// </para>
		/// </param>
		/// <param name="textures">
		/// <para>
		/// Specifies an array of textures to be deleted.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeleteTextures")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void DeleteTextures(Int32 n, Int32[] textures) {
			unsafe
			{
				fixed (Int32* textures_ptr = textures) {
					Delegates.glDeleteTextures((Int32) n, (UInt32*) textures_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Delete named textures
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of textures to be deleted.
		/// </para>
		/// </param>
		/// <param name="textures">
		/// <para>
		/// Specifies an array of textures to be deleted.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeleteTextures")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void DeleteTextures(Int32 n, ref Int32 textures) {
			unsafe
			{
				fixed (Int32* textures_ptr = &textures) {
					Delegates.glDeleteTextures((Int32) n, (UInt32*) textures_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Delete named textures
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of textures to be deleted.
		/// </para>
		/// </param>
		/// <param name="textures">
		/// <para>
		/// Specifies an array of textures to be deleted.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeleteTextures")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void DeleteTextures(Int32 n, Int32* textures) {
			Delegates.glDeleteTextures((Int32) n, (UInt32*) textures);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Delete named textures
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of textures to be deleted.
		/// </para>
		/// </param>
		/// <param name="textures">
		/// <para>
		/// Specifies an array of textures to be deleted.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeleteTextures")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void DeleteTextures(Int32 n, UInt32[] textures) {
			unsafe
			{
				fixed (UInt32* textures_ptr = textures) {
					Delegates.glDeleteTextures((Int32) n, (UInt32*) textures_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Delete named textures
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of textures to be deleted.
		/// </para>
		/// </param>
		/// <param name="textures">
		/// <para>
		/// Specifies an array of textures to be deleted.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeleteTextures")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void DeleteTextures(Int32 n, ref UInt32 textures) {
			unsafe
			{
				fixed (UInt32* textures_ptr = &textures) {
					Delegates.glDeleteTextures((Int32) n, (UInt32*) textures_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Delete named textures
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of textures to be deleted.
		/// </para>
		/// </param>
		/// <param name="textures">
		/// <para>
		/// Specifies an array of textures to be deleted.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeleteTextures")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void DeleteTextures(Int32 n, UInt32* textures) {
			Delegates.glDeleteTextures((Int32) n, (UInt32*) textures);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify the value used for depth buffer comparisons
		/// </summary>
		/// <param name="func">
		/// <para>
		/// Specifies the depth comparison function. Symbolic constants GL_NEVER, GL_LESS, GL_EQUAL, GL_LEQUAL, GL_GREATER, GL_NOTEQUAL, GL_GEQUAL, and GL_ALWAYS are accepted. The initial value is GL_LESS.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDepthFunc")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void DepthFunc(System.Graphics.ES20.DepthFunction func) {
			Delegates.glDepthFunc((System.Graphics.ES20.DepthFunction) func);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Enable or disable writing into the depth buffer
		/// </summary>
		/// <param name="flag">
		/// <para>
		/// Specifies whether the depth buffer is enabled for writing. If flag is GL_FALSE, depth buffer writing is disabled. Otherwise, it is enabled. Initially, depth buffer writing is enabled.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDepthMask")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void DepthMask(bool flag) {
			Delegates.glDepthMask((bool) flag);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify mapping of depth values from normalized device coordinates to window coordinates
		/// </summary>
		/// <param name="nearVal">
		/// <para>
		/// Specifies the mapping of the near clipping plane to window coordinates. The initial value is 0.
		/// </para>
		/// </param>
		/// <param name="farVal">
		/// <para>
		/// Specifies the mapping of the far clipping plane to window coordinates. The initial value is 1.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDepthRangef")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void DepthRange(Single zNear, Single zFar) {
			Delegates.glDepthRangef((Single) zNear, (Single) zFar);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Detaches a shader object from a program object to which it is attached
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object from which to detach the shader object.
		/// </para>
		/// </param>
		/// <param name="shader">
		/// <para>
		/// Specifies the shader object to be detached.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDetachShader")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void DetachShader(Int32 program, Int32 shader) {
			Delegates.glDetachShader((UInt32) program, (UInt32) shader);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Detaches a shader object from a program object to which it is attached
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object from which to detach the shader object.
		/// </para>
		/// </param>
		/// <param name="shader">
		/// <para>
		/// Specifies the shader object to be detached.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDetachShader")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void DetachShader(UInt32 program, UInt32 shader) {
			Delegates.glDetachShader((UInt32) program, (UInt32) shader);
		}

		/// <summary>[requires: v2.0 and 2.0]</summary>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDisable")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void Disable(System.Graphics.ES20.EnableCap cap) {
			Delegates.glDisable((System.Graphics.ES20.EnableCap) cap);
		}

		/// <summary>[requires: v2.0 and 2.0]</summary>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDisableVertexAttribArray")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void DisableVertexAttribArray(Int32 index) {
			Delegates.glDisableVertexAttribArray((UInt32) index);
		}

		/// <summary>[requires: v2.0 and 2.0]</summary>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDisableVertexAttribArray")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void DisableVertexAttribArray(UInt32 index) {
			Delegates.glDisableVertexAttribArray((UInt32) index);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Render primitives from array data
		/// </summary>
		/// <param name="mode">
		/// <para>
		/// Specifies what kind of primitives to render. Symbolic constants GL_POINTS, GL_LINE_STRIP, GL_LINE_LOOP, GL_LINES, GL_LINE_STRIP_ADJACENCY, GL_LINES_ADJACENCY, GL_TRIANGLE_STRIP, GL_TRIANGLE_FAN, GL_TRIANGLES, GL_TRIANGLE_STRIP_ADJACENCY, GL_TRIANGLES_ADJACENCY and GL_PATCHES are accepted.
		/// </para>
		/// </param>
		/// <param name="first">
		/// <para>
		/// Specifies the starting index in the enabled arrays.
		/// </para>
		/// </param>
		/// <param name="count">
		/// <para>
		/// Specifies the number of indices to be rendered.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDrawArrays")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void DrawArrays(System.Graphics.ES20.BeginMode mode, Int32 first, Int32 count) {
			Delegates.glDrawArrays((System.Graphics.ES20.BeginMode) mode, (Int32) first, (Int32) count);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Render primitives from array data
		/// </summary>
		/// <param name="mode">
		/// <para>
		/// Specifies what kind of primitives to render. Symbolic constants GL_POINTS, GL_LINE_STRIP, GL_LINE_LOOP, GL_LINES, GL_LINE_STRIP_ADJACENCY, GL_LINES_ADJACENCY, GL_TRIANGLE_STRIP, GL_TRIANGLE_FAN, GL_TRIANGLES, GL_TRIANGLE_STRIP_ADJACENCY, GL_TRIANGLES_ADJACENCY and GL_PATCHES are accepted.
		/// </para>
		/// </param>
		/// <param name="count">
		/// <para>
		/// Specifies the number of elements to be rendered.
		/// </para>
		/// </param>
		/// <param name="type">
		/// <para>
		/// Specifies the type of the values in indices. Must be one of GL_UNSIGNED_BYTE, GL_UNSIGNED_SHORT, or GL_UNSIGNED_INT.
		/// </para>
		/// </param>
		/// <param name="indices">
		/// <para>
		/// Specifies a pointer to the location where the indices are stored.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDrawElements")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void DrawElements(System.Graphics.ES20.BeginMode mode, Int32 count, System.Graphics.ES20.DrawElementsType type, IntPtr indices) {
			Delegates.glDrawElements((System.Graphics.ES20.BeginMode) mode, (Int32) count, (System.Graphics.ES20.DrawElementsType) type, (IntPtr) indices);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Render primitives from array data
		/// </summary>
		/// <param name="mode">
		/// <para>
		/// Specifies what kind of primitives to render. Symbolic constants GL_POINTS, GL_LINE_STRIP, GL_LINE_LOOP, GL_LINES, GL_LINE_STRIP_ADJACENCY, GL_LINES_ADJACENCY, GL_TRIANGLE_STRIP, GL_TRIANGLE_FAN, GL_TRIANGLES, GL_TRIANGLE_STRIP_ADJACENCY, GL_TRIANGLES_ADJACENCY and GL_PATCHES are accepted.
		/// </para>
		/// </param>
		/// <param name="count">
		/// <para>
		/// Specifies the number of elements to be rendered.
		/// </para>
		/// </param>
		/// <param name="type">
		/// <para>
		/// Specifies the type of the values in indices. Must be one of GL_UNSIGNED_BYTE, GL_UNSIGNED_SHORT, or GL_UNSIGNED_INT.
		/// </para>
		/// </param>
		/// <param name="indices">
		/// <para>
		/// Specifies a pointer to the location where the indices are stored.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDrawElements")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void DrawElements<T3>(System.Graphics.ES20.BeginMode mode, Int32 count, System.Graphics.ES20.DrawElementsType type, [InAttribute, OutAttribute] T3[] indices)
					where T3 : struct {
			GCHandle indices_ptr = GCHandle.Alloc(indices, GCHandleType.Pinned);
			try {
				Delegates.glDrawElements((System.Graphics.ES20.BeginMode) mode, (Int32) count, (System.Graphics.ES20.DrawElementsType) type, (IntPtr) indices_ptr.AddrOfPinnedObject());
			} finally {
				indices_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Render primitives from array data
		/// </summary>
		/// <param name="mode">
		/// <para>
		/// Specifies what kind of primitives to render. Symbolic constants GL_POINTS, GL_LINE_STRIP, GL_LINE_LOOP, GL_LINES, GL_LINE_STRIP_ADJACENCY, GL_LINES_ADJACENCY, GL_TRIANGLE_STRIP, GL_TRIANGLE_FAN, GL_TRIANGLES, GL_TRIANGLE_STRIP_ADJACENCY, GL_TRIANGLES_ADJACENCY and GL_PATCHES are accepted.
		/// </para>
		/// </param>
		/// <param name="count">
		/// <para>
		/// Specifies the number of elements to be rendered.
		/// </para>
		/// </param>
		/// <param name="type">
		/// <para>
		/// Specifies the type of the values in indices. Must be one of GL_UNSIGNED_BYTE, GL_UNSIGNED_SHORT, or GL_UNSIGNED_INT.
		/// </para>
		/// </param>
		/// <param name="indices">
		/// <para>
		/// Specifies a pointer to the location where the indices are stored.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDrawElements")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void DrawElements<T3>(System.Graphics.ES20.BeginMode mode, Int32 count, System.Graphics.ES20.DrawElementsType type, [InAttribute, OutAttribute] T3[,] indices)
					where T3 : struct {
			GCHandle indices_ptr = GCHandle.Alloc(indices, GCHandleType.Pinned);
			try {
				Delegates.glDrawElements((System.Graphics.ES20.BeginMode) mode, (Int32) count, (System.Graphics.ES20.DrawElementsType) type, (IntPtr) indices_ptr.AddrOfPinnedObject());
			} finally {
				indices_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Render primitives from array data
		/// </summary>
		/// <param name="mode">
		/// <para>
		/// Specifies what kind of primitives to render. Symbolic constants GL_POINTS, GL_LINE_STRIP, GL_LINE_LOOP, GL_LINES, GL_LINE_STRIP_ADJACENCY, GL_LINES_ADJACENCY, GL_TRIANGLE_STRIP, GL_TRIANGLE_FAN, GL_TRIANGLES, GL_TRIANGLE_STRIP_ADJACENCY, GL_TRIANGLES_ADJACENCY and GL_PATCHES are accepted.
		/// </para>
		/// </param>
		/// <param name="count">
		/// <para>
		/// Specifies the number of elements to be rendered.
		/// </para>
		/// </param>
		/// <param name="type">
		/// <para>
		/// Specifies the type of the values in indices. Must be one of GL_UNSIGNED_BYTE, GL_UNSIGNED_SHORT, or GL_UNSIGNED_INT.
		/// </para>
		/// </param>
		/// <param name="indices">
		/// <para>
		/// Specifies a pointer to the location where the indices are stored.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDrawElements")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void DrawElements<T3>(System.Graphics.ES20.BeginMode mode, Int32 count, System.Graphics.ES20.DrawElementsType type, [InAttribute, OutAttribute] T3[,,] indices)
					where T3 : struct {
			GCHandle indices_ptr = GCHandle.Alloc(indices, GCHandleType.Pinned);
			try {
				Delegates.glDrawElements((System.Graphics.ES20.BeginMode) mode, (Int32) count, (System.Graphics.ES20.DrawElementsType) type, (IntPtr) indices_ptr.AddrOfPinnedObject());
			} finally {
				indices_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Render primitives from array data
		/// </summary>
		/// <param name="mode">
		/// <para>
		/// Specifies what kind of primitives to render. Symbolic constants GL_POINTS, GL_LINE_STRIP, GL_LINE_LOOP, GL_LINES, GL_LINE_STRIP_ADJACENCY, GL_LINES_ADJACENCY, GL_TRIANGLE_STRIP, GL_TRIANGLE_FAN, GL_TRIANGLES, GL_TRIANGLE_STRIP_ADJACENCY, GL_TRIANGLES_ADJACENCY and GL_PATCHES are accepted.
		/// </para>
		/// </param>
		/// <param name="count">
		/// <para>
		/// Specifies the number of elements to be rendered.
		/// </para>
		/// </param>
		/// <param name="type">
		/// <para>
		/// Specifies the type of the values in indices. Must be one of GL_UNSIGNED_BYTE, GL_UNSIGNED_SHORT, or GL_UNSIGNED_INT.
		/// </para>
		/// </param>
		/// <param name="indices">
		/// <para>
		/// Specifies a pointer to the location where the indices are stored.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDrawElements")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void DrawElements<T3>(System.Graphics.ES20.BeginMode mode, Int32 count, System.Graphics.ES20.DrawElementsType type, [InAttribute, OutAttribute] ref T3 indices)
					where T3 : struct {
			GCHandle indices_ptr = GCHandle.Alloc(indices, GCHandleType.Pinned);
			try {
				Delegates.glDrawElements((System.Graphics.ES20.BeginMode) mode, (Int32) count, (System.Graphics.ES20.DrawElementsType) type, (IntPtr) indices_ptr.AddrOfPinnedObject());
				indices = (T3) indices_ptr.Target;
			} finally {
				indices_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Enable or disable server-side GL capabilities
		/// </summary>
		/// <param name="cap">
		/// <para>
		/// Specifies a symbolic constant indicating a GL capability.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glEnable")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void Enable(System.Graphics.ES20.EnableCap cap) {
			Delegates.glEnable((System.Graphics.ES20.EnableCap) cap);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Enable or disable a generic vertex attribute array
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be enabled or disabled.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glEnableVertexAttribArray")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void EnableVertexAttribArray(Int32 index) {
			Delegates.glEnableVertexAttribArray((UInt32) index);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Enable or disable a generic vertex attribute array
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be enabled or disabled.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glEnableVertexAttribArray")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void EnableVertexAttribArray(UInt32 index) {
			Delegates.glEnableVertexAttribArray((UInt32) index);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Block until all GL execution is complete
		/// </summary>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glFinish")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void Finish() {
			Delegates.glFinish();
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Force execution of GL commands in finite time
		/// </summary>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glFlush")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void Flush() {
			Delegates.glFlush();
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Attach a renderbuffer as a logical buffer to the currently bound framebuffer object
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the framebuffer target. target must be GL_DRAW_FRAMEBUFFER, GL_READ_FRAMEBUFFER, or GL_FRAMEBUFFER. GL_FRAMEBUFFER is equivalent to GL_DRAW_FRAMEBUFFER.
		/// </para>
		/// </param>
		/// <param name="attachment">
		/// <para>
		/// Specifies the attachment point of the framebuffer.
		/// </para>
		/// </param>
		/// <param name="renderbuffertarget">
		/// <para>
		/// Specifies the renderbuffer target and must be GL_RENDERBUFFER.
		/// </para>
		/// </param>
		/// <param name="renderbuffer">
		/// <para>
		/// Specifies the name of an existing renderbuffer object of type renderbuffertarget to attach.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glFramebufferRenderbuffer")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void FramebufferRenderbuffer(System.Graphics.ES20.FramebufferTarget target, System.Graphics.ES20.FramebufferSlot attachment, System.Graphics.ES20.RenderbufferTarget renderbuffertarget, Int32 renderbuffer) {
			Delegates.glFramebufferRenderbuffer((System.Graphics.ES20.FramebufferTarget) target, (System.Graphics.ES20.FramebufferSlot) attachment, (System.Graphics.ES20.RenderbufferTarget) renderbuffertarget, (UInt32) renderbuffer);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Attach a renderbuffer as a logical buffer to the currently bound framebuffer object
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the framebuffer target. target must be GL_DRAW_FRAMEBUFFER, GL_READ_FRAMEBUFFER, or GL_FRAMEBUFFER. GL_FRAMEBUFFER is equivalent to GL_DRAW_FRAMEBUFFER.
		/// </para>
		/// </param>
		/// <param name="attachment">
		/// <para>
		/// Specifies the attachment point of the framebuffer.
		/// </para>
		/// </param>
		/// <param name="renderbuffertarget">
		/// <para>
		/// Specifies the renderbuffer target and must be GL_RENDERBUFFER.
		/// </para>
		/// </param>
		/// <param name="renderbuffer">
		/// <para>
		/// Specifies the name of an existing renderbuffer object of type renderbuffertarget to attach.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glFramebufferRenderbuffer")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void FramebufferRenderbuffer(System.Graphics.ES20.FramebufferTarget target, System.Graphics.ES20.FramebufferSlot attachment, System.Graphics.ES20.RenderbufferTarget renderbuffertarget, UInt32 renderbuffer) {
			Delegates.glFramebufferRenderbuffer((System.Graphics.ES20.FramebufferTarget) target, (System.Graphics.ES20.FramebufferSlot) attachment, (System.Graphics.ES20.RenderbufferTarget) renderbuffertarget, (UInt32) renderbuffer);
		}

		/// <summary>[requires: v2.0 and 2.0]</summary>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glFramebufferTexture2D")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void FramebufferTexture2D(System.Graphics.ES20.FramebufferTarget target, System.Graphics.ES20.FramebufferSlot attachment, System.Graphics.ES20.TextureTarget textarget, Int32 texture, Int32 level) {
			Delegates.glFramebufferTexture2D((System.Graphics.ES20.FramebufferTarget) target, (System.Graphics.ES20.FramebufferSlot) attachment, (System.Graphics.ES20.TextureTarget) textarget, (UInt32) texture, (Int32) level);
		}

		/// <summary>[requires: v2.0 and 2.0]</summary>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glFramebufferTexture2D")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void FramebufferTexture2D(System.Graphics.ES20.FramebufferTarget target, System.Graphics.ES20.FramebufferSlot attachment, System.Graphics.ES20.TextureTarget textarget, UInt32 texture, Int32 level) {
			Delegates.glFramebufferTexture2D((System.Graphics.ES20.FramebufferTarget) target, (System.Graphics.ES20.FramebufferSlot) attachment, (System.Graphics.ES20.TextureTarget) textarget, (UInt32) texture, (Int32) level);
		}

		/// <summary>[requires: v2.0 and 2.0]</summary>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glFramebufferTexture2DMultisampleIMG")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void FramebufferTexture2DMultisampleIMG() {
			Delegates.glFramebufferTexture2DMultisampleIMG();
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Define front- and back-facing polygons
		/// </summary>
		/// <param name="mode">
		/// <para>
		/// Specifies the orientation of front-facing polygons. GL_CW and GL_CCW are accepted. The initial value is GL_CCW.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glFrontFace")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void FrontFace(System.Graphics.ES20.FrontFaceDirection mode) {
			Delegates.glFrontFace((System.Graphics.ES20.FrontFaceDirection) mode);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Generate buffer object names
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of buffer object names to be generated.
		/// </para>
		/// </param>
		/// <param name="buffers">
		/// <para>
		/// Specifies an array in which the generated buffer object names are stored.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenBuffers")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GenBuffers(Int32 n, [OutAttribute] Int32[] buffers) {
			unsafe
			{
				fixed (Int32* buffers_ptr = buffers) {
					Delegates.glGenBuffers((Int32) n, (UInt32*) buffers_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Generate buffer object names
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of buffer object names to be generated.
		/// </para>
		/// </param>
		/// <param name="buffers">
		/// <para>
		/// Specifies an array in which the generated buffer object names are stored.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenBuffers")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GenBuffers(Int32 n, [OutAttribute] out Int32 buffers) {
			unsafe
			{
				fixed (Int32* buffers_ptr = &buffers) {
					Delegates.glGenBuffers((Int32) n, (UInt32*) buffers_ptr);
					buffers = *buffers_ptr;
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Generate buffer object names
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of buffer object names to be generated.
		/// </para>
		/// </param>
		/// <param name="buffers">
		/// <para>
		/// Specifies an array in which the generated buffer object names are stored.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenBuffers")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void GenBuffers(Int32 n, [OutAttribute] Int32* buffers) {
			Delegates.glGenBuffers((Int32) n, (UInt32*) buffers);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Generate buffer object names
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of buffer object names to be generated.
		/// </para>
		/// </param>
		/// <param name="buffers">
		/// <para>
		/// Specifies an array in which the generated buffer object names are stored.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenBuffers")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void GenBuffers(Int32 n, [OutAttribute] UInt32[] buffers) {
			unsafe
			{
				fixed (UInt32* buffers_ptr = buffers) {
					Delegates.glGenBuffers((Int32) n, (UInt32*) buffers_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Generate buffer object names
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of buffer object names to be generated.
		/// </para>
		/// </param>
		/// <param name="buffers">
		/// <para>
		/// Specifies an array in which the generated buffer object names are stored.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenBuffers")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void GenBuffers(Int32 n, [OutAttribute] out UInt32 buffers) {
			unsafe
			{
				fixed (UInt32* buffers_ptr = &buffers) {
					Delegates.glGenBuffers((Int32) n, (UInt32*) buffers_ptr);
					buffers = *buffers_ptr;
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Generate buffer object names
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of buffer object names to be generated.
		/// </para>
		/// </param>
		/// <param name="buffers">
		/// <para>
		/// Specifies an array in which the generated buffer object names are stored.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenBuffers")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void GenBuffers(Int32 n, [OutAttribute] UInt32* buffers) {
			Delegates.glGenBuffers((Int32) n, (UInt32*) buffers);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Generate mipmaps for a specified texture target
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target to which the texture whose mimaps to generate is bound. target must be GL_TEXTURE_1D, GL_TEXTURE_2D, GL_TEXTURE_3D, GL_TEXTURE_1D_ARRAY, GL_TEXTURE_2D_ARRAY or GL_TEXTURE_CUBE_MAP.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenerateMipmap")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GenerateMipmap(System.Graphics.ES20.TextureTarget target) {
			Delegates.glGenerateMipmap((System.Graphics.ES20.TextureTarget) target);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Generate framebuffer object names
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of framebuffer object names to generate.
		/// </para>
		/// </param>
		/// <param name="ids">
		/// <para>
		/// Specifies an array in which the generated framebuffer object names are stored.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenFramebuffers")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GenFramebuffers(Int32 n, [OutAttribute] Int32[] framebuffers) {
			unsafe
			{
				fixed (Int32* framebuffers_ptr = framebuffers) {
					Delegates.glGenFramebuffers((Int32) n, (UInt32*) framebuffers_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Generate framebuffer object names
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of framebuffer object names to generate.
		/// </para>
		/// </param>
		/// <param name="ids">
		/// <para>
		/// Specifies an array in which the generated framebuffer object names are stored.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenFramebuffers")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GenFramebuffers(Int32 n, [OutAttribute] out Int32 framebuffers) {
			unsafe
			{
				fixed (Int32* framebuffers_ptr = &framebuffers) {
					Delegates.glGenFramebuffers((Int32) n, (UInt32*) framebuffers_ptr);
					framebuffers = *framebuffers_ptr;
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Generate framebuffer object names
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of framebuffer object names to generate.
		/// </para>
		/// </param>
		/// <param name="ids">
		/// <para>
		/// Specifies an array in which the generated framebuffer object names are stored.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenFramebuffers")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void GenFramebuffers(Int32 n, [OutAttribute] Int32* framebuffers) {
			Delegates.glGenFramebuffers((Int32) n, (UInt32*) framebuffers);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Generate framebuffer object names
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of framebuffer object names to generate.
		/// </para>
		/// </param>
		/// <param name="ids">
		/// <para>
		/// Specifies an array in which the generated framebuffer object names are stored.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenFramebuffers")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void GenFramebuffers(Int32 n, [OutAttribute] UInt32[] framebuffers) {
			unsafe
			{
				fixed (UInt32* framebuffers_ptr = framebuffers) {
					Delegates.glGenFramebuffers((Int32) n, (UInt32*) framebuffers_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Generate framebuffer object names
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of framebuffer object names to generate.
		/// </para>
		/// </param>
		/// <param name="ids">
		/// <para>
		/// Specifies an array in which the generated framebuffer object names are stored.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenFramebuffers")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void GenFramebuffers(Int32 n, [OutAttribute] out UInt32 framebuffers) {
			unsafe
			{
				fixed (UInt32* framebuffers_ptr = &framebuffers) {
					Delegates.glGenFramebuffers((Int32) n, (UInt32*) framebuffers_ptr);
					framebuffers = *framebuffers_ptr;
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Generate framebuffer object names
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of framebuffer object names to generate.
		/// </para>
		/// </param>
		/// <param name="ids">
		/// <para>
		/// Specifies an array in which the generated framebuffer object names are stored.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenFramebuffers")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void GenFramebuffers(Int32 n, [OutAttribute] UInt32* framebuffers) {
			Delegates.glGenFramebuffers((Int32) n, (UInt32*) framebuffers);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Generate renderbuffer object names
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of renderbuffer object names to generate.
		/// </para>
		/// </param>
		/// <param name="renderbuffers">
		/// <para>
		/// Specifies an array in which the generated renderbuffer object names are stored.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenRenderbuffers")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GenRenderbuffers(Int32 n, [OutAttribute] Int32[] renderbuffers) {
			unsafe
			{
				fixed (Int32* renderbuffers_ptr = renderbuffers) {
					Delegates.glGenRenderbuffers((Int32) n, (UInt32*) renderbuffers_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Generate renderbuffer object names
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of renderbuffer object names to generate.
		/// </para>
		/// </param>
		/// <param name="renderbuffers">
		/// <para>
		/// Specifies an array in which the generated renderbuffer object names are stored.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenRenderbuffers")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GenRenderbuffers(Int32 n, [OutAttribute] out Int32 renderbuffers) {
			unsafe
			{
				fixed (Int32* renderbuffers_ptr = &renderbuffers) {
					Delegates.glGenRenderbuffers((Int32) n, (UInt32*) renderbuffers_ptr);
					renderbuffers = *renderbuffers_ptr;
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Generate renderbuffer object names
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of renderbuffer object names to generate.
		/// </para>
		/// </param>
		/// <param name="renderbuffers">
		/// <para>
		/// Specifies an array in which the generated renderbuffer object names are stored.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenRenderbuffers")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void GenRenderbuffers(Int32 n, [OutAttribute] Int32* renderbuffers) {
			Delegates.glGenRenderbuffers((Int32) n, (UInt32*) renderbuffers);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Generate renderbuffer object names
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of renderbuffer object names to generate.
		/// </para>
		/// </param>
		/// <param name="renderbuffers">
		/// <para>
		/// Specifies an array in which the generated renderbuffer object names are stored.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenRenderbuffers")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void GenRenderbuffers(Int32 n, [OutAttribute] UInt32[] renderbuffers) {
			unsafe
			{
				fixed (UInt32* renderbuffers_ptr = renderbuffers) {
					Delegates.glGenRenderbuffers((Int32) n, (UInt32*) renderbuffers_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Generate renderbuffer object names
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of renderbuffer object names to generate.
		/// </para>
		/// </param>
		/// <param name="renderbuffers">
		/// <para>
		/// Specifies an array in which the generated renderbuffer object names are stored.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenRenderbuffers")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void GenRenderbuffers(Int32 n, [OutAttribute] out UInt32 renderbuffers) {
			unsafe
			{
				fixed (UInt32* renderbuffers_ptr = &renderbuffers) {
					Delegates.glGenRenderbuffers((Int32) n, (UInt32*) renderbuffers_ptr);
					renderbuffers = *renderbuffers_ptr;
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Generate renderbuffer object names
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of renderbuffer object names to generate.
		/// </para>
		/// </param>
		/// <param name="renderbuffers">
		/// <para>
		/// Specifies an array in which the generated renderbuffer object names are stored.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenRenderbuffers")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void GenRenderbuffers(Int32 n, [OutAttribute] UInt32* renderbuffers) {
			Delegates.glGenRenderbuffers((Int32) n, (UInt32*) renderbuffers);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Generate texture names
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of texture names to be generated.
		/// </para>
		/// </param>
		/// <param name="textures">
		/// <para>
		/// Specifies an array in which the generated texture names are stored.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenTextures")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GenTextures(Int32 n, [OutAttribute] Int32[] textures) {
			unsafe
			{
				fixed (Int32* textures_ptr = textures) {
					Delegates.glGenTextures((Int32) n, (UInt32*) textures_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Generate texture names
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of texture names to be generated.
		/// </para>
		/// </param>
		/// <param name="textures">
		/// <para>
		/// Specifies an array in which the generated texture names are stored.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenTextures")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GenTextures(Int32 n, [OutAttribute] out Int32 textures) {
			unsafe
			{
				fixed (Int32* textures_ptr = &textures) {
					Delegates.glGenTextures((Int32) n, (UInt32*) textures_ptr);
					textures = *textures_ptr;
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Generate texture names
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of texture names to be generated.
		/// </para>
		/// </param>
		/// <param name="textures">
		/// <para>
		/// Specifies an array in which the generated texture names are stored.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenTextures")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void GenTextures(Int32 n, [OutAttribute] Int32* textures) {
			Delegates.glGenTextures((Int32) n, (UInt32*) textures);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Generate texture names
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of texture names to be generated.
		/// </para>
		/// </param>
		/// <param name="textures">
		/// <para>
		/// Specifies an array in which the generated texture names are stored.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenTextures")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void GenTextures(Int32 n, [OutAttribute] UInt32[] textures) {
			unsafe
			{
				fixed (UInt32* textures_ptr = textures) {
					Delegates.glGenTextures((Int32) n, (UInt32*) textures_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Generate texture names
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of texture names to be generated.
		/// </para>
		/// </param>
		/// <param name="textures">
		/// <para>
		/// Specifies an array in which the generated texture names are stored.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenTextures")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void GenTextures(Int32 n, [OutAttribute] out UInt32 textures) {
			unsafe
			{
				fixed (UInt32* textures_ptr = &textures) {
					Delegates.glGenTextures((Int32) n, (UInt32*) textures_ptr);
					textures = *textures_ptr;
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Generate texture names
		/// </summary>
		/// <param name="n">
		/// <para>
		/// Specifies the number of texture names to be generated.
		/// </para>
		/// </param>
		/// <param name="textures">
		/// <para>
		/// Specifies an array in which the generated texture names are stored.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenTextures")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void GenTextures(Int32 n, [OutAttribute] UInt32* textures) {
			Delegates.glGenTextures((Int32) n, (UInt32*) textures);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns information about an active attribute variable for the specified program object
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to be queried.
		/// </para>
		/// </param>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the attribute variable to be queried.
		/// </para>
		/// </param>
		/// <param name="bufSize">
		/// <para>
		/// Specifies the maximum number of characters OpenGL is allowed to write in the character buffer indicated by name.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Returns the number of characters actually written by OpenGL in the string indicated by name (excluding the null terminator) if a value other than NULL is passed.
		/// </para>
		/// </param>
		/// <param name="size">
		/// <para>
		/// Returns the size of the attribute variable.
		/// </para>
		/// </param>
		/// <param name="type">
		/// <para>
		/// Returns the data type of the attribute variable.
		/// </para>
		/// </param>
		/// <param name="name">
		/// <para>
		/// Returns a null terminated string containing the name of the attribute variable.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetActiveAttrib")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetActiveAttrib(Int32 program, Int32 index, Int32 bufsize, [OutAttribute] Int32[] length, [OutAttribute] Int32[] size, [OutAttribute] System.Graphics.ES20.ActiveAttribType[] type, [OutAttribute] StringBuilder name) {
			unsafe
			{
				fixed (Int32* length_ptr = length)
				fixed (Int32* size_ptr = size)
				fixed (System.Graphics.ES20.ActiveAttribType* type_ptr = type) {
					Delegates.glGetActiveAttrib((UInt32) program, (UInt32) index, (Int32) bufsize, (Int32*) length_ptr, (Int32*) size_ptr, (System.Graphics.ES20.ActiveAttribType*) type_ptr, (StringBuilder) name);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns information about an active attribute variable for the specified program object
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to be queried.
		/// </para>
		/// </param>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the attribute variable to be queried.
		/// </para>
		/// </param>
		/// <param name="bufSize">
		/// <para>
		/// Specifies the maximum number of characters OpenGL is allowed to write in the character buffer indicated by name.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Returns the number of characters actually written by OpenGL in the string indicated by name (excluding the null terminator) if a value other than NULL is passed.
		/// </para>
		/// </param>
		/// <param name="size">
		/// <para>
		/// Returns the size of the attribute variable.
		/// </para>
		/// </param>
		/// <param name="type">
		/// <para>
		/// Returns the data type of the attribute variable.
		/// </para>
		/// </param>
		/// <param name="name">
		/// <para>
		/// Returns a null terminated string containing the name of the attribute variable.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetActiveAttrib")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetActiveAttrib(Int32 program, Int32 index, Int32 bufsize, [OutAttribute] out Int32 length, [OutAttribute] out Int32 size, [OutAttribute] out System.Graphics.ES20.ActiveAttribType type, [OutAttribute] StringBuilder name) {
			unsafe
			{
				fixed (Int32* length_ptr = &length)
				fixed (Int32* size_ptr = &size)
				fixed (System.Graphics.ES20.ActiveAttribType* type_ptr = &type) {
					Delegates.glGetActiveAttrib((UInt32) program, (UInt32) index, (Int32) bufsize, (Int32*) length_ptr, (Int32*) size_ptr, (System.Graphics.ES20.ActiveAttribType*) type_ptr, (StringBuilder) name);
					length = *length_ptr;
					size = *size_ptr;
					type = *type_ptr;
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns information about an active attribute variable for the specified program object
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to be queried.
		/// </para>
		/// </param>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the attribute variable to be queried.
		/// </para>
		/// </param>
		/// <param name="bufSize">
		/// <para>
		/// Specifies the maximum number of characters OpenGL is allowed to write in the character buffer indicated by name.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Returns the number of characters actually written by OpenGL in the string indicated by name (excluding the null terminator) if a value other than NULL is passed.
		/// </para>
		/// </param>
		/// <param name="size">
		/// <para>
		/// Returns the size of the attribute variable.
		/// </para>
		/// </param>
		/// <param name="type">
		/// <para>
		/// Returns the data type of the attribute variable.
		/// </para>
		/// </param>
		/// <param name="name">
		/// <para>
		/// Returns a null terminated string containing the name of the attribute variable.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetActiveAttrib")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void GetActiveAttrib(Int32 program, Int32 index, Int32 bufsize, [OutAttribute] Int32* length, [OutAttribute] Int32* size, [OutAttribute] System.Graphics.ES20.ActiveAttribType* type, [OutAttribute] StringBuilder name) {
			Delegates.glGetActiveAttrib((UInt32) program, (UInt32) index, (Int32) bufsize, (Int32*) length, (Int32*) size, (System.Graphics.ES20.ActiveAttribType*) type, (StringBuilder) name);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns information about an active attribute variable for the specified program object
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to be queried.
		/// </para>
		/// </param>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the attribute variable to be queried.
		/// </para>
		/// </param>
		/// <param name="bufSize">
		/// <para>
		/// Specifies the maximum number of characters OpenGL is allowed to write in the character buffer indicated by name.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Returns the number of characters actually written by OpenGL in the string indicated by name (excluding the null terminator) if a value other than NULL is passed.
		/// </para>
		/// </param>
		/// <param name="size">
		/// <para>
		/// Returns the size of the attribute variable.
		/// </para>
		/// </param>
		/// <param name="type">
		/// <para>
		/// Returns the data type of the attribute variable.
		/// </para>
		/// </param>
		/// <param name="name">
		/// <para>
		/// Returns a null terminated string containing the name of the attribute variable.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetActiveAttrib")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void GetActiveAttrib(UInt32 program, UInt32 index, Int32 bufsize, [OutAttribute] Int32[] length, [OutAttribute] Int32[] size, [OutAttribute] System.Graphics.ES20.ActiveAttribType[] type, [OutAttribute] StringBuilder name) {
			unsafe
			{
				fixed (Int32* length_ptr = length)
				fixed (Int32* size_ptr = size)
				fixed (System.Graphics.ES20.ActiveAttribType* type_ptr = type) {
					Delegates.glGetActiveAttrib((UInt32) program, (UInt32) index, (Int32) bufsize, (Int32*) length_ptr, (Int32*) size_ptr, (System.Graphics.ES20.ActiveAttribType*) type_ptr, (StringBuilder) name);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns information about an active attribute variable for the specified program object
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to be queried.
		/// </para>
		/// </param>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the attribute variable to be queried.
		/// </para>
		/// </param>
		/// <param name="bufSize">
		/// <para>
		/// Specifies the maximum number of characters OpenGL is allowed to write in the character buffer indicated by name.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Returns the number of characters actually written by OpenGL in the string indicated by name (excluding the null terminator) if a value other than NULL is passed.
		/// </para>
		/// </param>
		/// <param name="size">
		/// <para>
		/// Returns the size of the attribute variable.
		/// </para>
		/// </param>
		/// <param name="type">
		/// <para>
		/// Returns the data type of the attribute variable.
		/// </para>
		/// </param>
		/// <param name="name">
		/// <para>
		/// Returns a null terminated string containing the name of the attribute variable.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetActiveAttrib")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void GetActiveAttrib(UInt32 program, UInt32 index, Int32 bufsize, [OutAttribute] out Int32 length, [OutAttribute] out Int32 size, [OutAttribute] out System.Graphics.ES20.ActiveAttribType type, [OutAttribute] StringBuilder name) {
			unsafe
			{
				fixed (Int32* length_ptr = &length)
				fixed (Int32* size_ptr = &size)
				fixed (System.Graphics.ES20.ActiveAttribType* type_ptr = &type) {
					Delegates.glGetActiveAttrib((UInt32) program, (UInt32) index, (Int32) bufsize, (Int32*) length_ptr, (Int32*) size_ptr, (System.Graphics.ES20.ActiveAttribType*) type_ptr, (StringBuilder) name);
					length = *length_ptr;
					size = *size_ptr;
					type = *type_ptr;
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns information about an active attribute variable for the specified program object
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to be queried.
		/// </para>
		/// </param>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the attribute variable to be queried.
		/// </para>
		/// </param>
		/// <param name="bufSize">
		/// <para>
		/// Specifies the maximum number of characters OpenGL is allowed to write in the character buffer indicated by name.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Returns the number of characters actually written by OpenGL in the string indicated by name (excluding the null terminator) if a value other than NULL is passed.
		/// </para>
		/// </param>
		/// <param name="size">
		/// <para>
		/// Returns the size of the attribute variable.
		/// </para>
		/// </param>
		/// <param name="type">
		/// <para>
		/// Returns the data type of the attribute variable.
		/// </para>
		/// </param>
		/// <param name="name">
		/// <para>
		/// Returns a null terminated string containing the name of the attribute variable.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetActiveAttrib")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void GetActiveAttrib(UInt32 program, UInt32 index, Int32 bufsize, [OutAttribute] Int32* length, [OutAttribute] Int32* size, [OutAttribute] System.Graphics.ES20.ActiveAttribType* type, [OutAttribute] StringBuilder name) {
			Delegates.glGetActiveAttrib((UInt32) program, (UInt32) index, (Int32) bufsize, (Int32*) length, (Int32*) size, (System.Graphics.ES20.ActiveAttribType*) type, (StringBuilder) name);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns information about an active uniform variable for the specified program object
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to be queried.
		/// </para>
		/// </param>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the uniform variable to be queried.
		/// </para>
		/// </param>
		/// <param name="bufSize">
		/// <para>
		/// Specifies the maximum number of characters OpenGL is allowed to write in the character buffer indicated by name.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Returns the number of characters actually written by OpenGL in the string indicated by name (excluding the null terminator) if a value other than NULL is passed.
		/// </para>
		/// </param>
		/// <param name="size">
		/// <para>
		/// Returns the size of the uniform variable.
		/// </para>
		/// </param>
		/// <param name="type">
		/// <para>
		/// Returns the data type of the uniform variable.
		/// </para>
		/// </param>
		/// <param name="name">
		/// <para>
		/// Returns a null terminated string containing the name of the uniform variable.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetActiveUniform")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetActiveUniform(Int32 program, Int32 index, Int32 bufsize, [OutAttribute] Int32[] length, [OutAttribute] Int32[] size, [OutAttribute] System.Graphics.ES20.ActiveUniformType[] type, [OutAttribute] StringBuilder name) {
			unsafe
			{
				fixed (Int32* length_ptr = length)
				fixed (Int32* size_ptr = size)
				fixed (System.Graphics.ES20.ActiveUniformType* type_ptr = type) {
					Delegates.glGetActiveUniform((UInt32) program, (UInt32) index, (Int32) bufsize, (Int32*) length_ptr, (Int32*) size_ptr, (System.Graphics.ES20.ActiveUniformType*) type_ptr, (StringBuilder) name);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns information about an active uniform variable for the specified program object
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to be queried.
		/// </para>
		/// </param>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the uniform variable to be queried.
		/// </para>
		/// </param>
		/// <param name="bufSize">
		/// <para>
		/// Specifies the maximum number of characters OpenGL is allowed to write in the character buffer indicated by name.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Returns the number of characters actually written by OpenGL in the string indicated by name (excluding the null terminator) if a value other than NULL is passed.
		/// </para>
		/// </param>
		/// <param name="size">
		/// <para>
		/// Returns the size of the uniform variable.
		/// </para>
		/// </param>
		/// <param name="type">
		/// <para>
		/// Returns the data type of the uniform variable.
		/// </para>
		/// </param>
		/// <param name="name">
		/// <para>
		/// Returns a null terminated string containing the name of the uniform variable.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetActiveUniform")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetActiveUniform(Int32 program, Int32 index, Int32 bufsize, [OutAttribute] out Int32 length, [OutAttribute] out Int32 size, [OutAttribute] out System.Graphics.ES20.ActiveUniformType type, [OutAttribute] StringBuilder name) {
			unsafe
			{
				fixed (Int32* length_ptr = &length)
				fixed (Int32* size_ptr = &size)
				fixed (System.Graphics.ES20.ActiveUniformType* type_ptr = &type) {
					Delegates.glGetActiveUniform((UInt32) program, (UInt32) index, (Int32) bufsize, (Int32*) length_ptr, (Int32*) size_ptr, (System.Graphics.ES20.ActiveUniformType*) type_ptr, (StringBuilder) name);
					length = *length_ptr;
					size = *size_ptr;
					type = *type_ptr;
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns information about an active uniform variable for the specified program object
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to be queried.
		/// </para>
		/// </param>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the uniform variable to be queried.
		/// </para>
		/// </param>
		/// <param name="bufSize">
		/// <para>
		/// Specifies the maximum number of characters OpenGL is allowed to write in the character buffer indicated by name.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Returns the number of characters actually written by OpenGL in the string indicated by name (excluding the null terminator) if a value other than NULL is passed.
		/// </para>
		/// </param>
		/// <param name="size">
		/// <para>
		/// Returns the size of the uniform variable.
		/// </para>
		/// </param>
		/// <param name="type">
		/// <para>
		/// Returns the data type of the uniform variable.
		/// </para>
		/// </param>
		/// <param name="name">
		/// <para>
		/// Returns a null terminated string containing the name of the uniform variable.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetActiveUniform")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void GetActiveUniform(Int32 program, Int32 index, Int32 bufsize, [OutAttribute] Int32* length, [OutAttribute] Int32* size, [OutAttribute] System.Graphics.ES20.ActiveUniformType* type, [OutAttribute] StringBuilder name) {
			Delegates.glGetActiveUniform((UInt32) program, (UInt32) index, (Int32) bufsize, (Int32*) length, (Int32*) size, (System.Graphics.ES20.ActiveUniformType*) type, (StringBuilder) name);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns information about an active uniform variable for the specified program object
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to be queried.
		/// </para>
		/// </param>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the uniform variable to be queried.
		/// </para>
		/// </param>
		/// <param name="bufSize">
		/// <para>
		/// Specifies the maximum number of characters OpenGL is allowed to write in the character buffer indicated by name.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Returns the number of characters actually written by OpenGL in the string indicated by name (excluding the null terminator) if a value other than NULL is passed.
		/// </para>
		/// </param>
		/// <param name="size">
		/// <para>
		/// Returns the size of the uniform variable.
		/// </para>
		/// </param>
		/// <param name="type">
		/// <para>
		/// Returns the data type of the uniform variable.
		/// </para>
		/// </param>
		/// <param name="name">
		/// <para>
		/// Returns a null terminated string containing the name of the uniform variable.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetActiveUniform")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void GetActiveUniform(UInt32 program, UInt32 index, Int32 bufsize, [OutAttribute] Int32[] length, [OutAttribute] Int32[] size, [OutAttribute] System.Graphics.ES20.ActiveUniformType[] type, [OutAttribute] StringBuilder name) {
			unsafe
			{
				fixed (Int32* length_ptr = length)
				fixed (Int32* size_ptr = size)
				fixed (System.Graphics.ES20.ActiveUniformType* type_ptr = type) {
					Delegates.glGetActiveUniform((UInt32) program, (UInt32) index, (Int32) bufsize, (Int32*) length_ptr, (Int32*) size_ptr, (System.Graphics.ES20.ActiveUniformType*) type_ptr, (StringBuilder) name);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns information about an active uniform variable for the specified program object
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to be queried.
		/// </para>
		/// </param>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the uniform variable to be queried.
		/// </para>
		/// </param>
		/// <param name="bufSize">
		/// <para>
		/// Specifies the maximum number of characters OpenGL is allowed to write in the character buffer indicated by name.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Returns the number of characters actually written by OpenGL in the string indicated by name (excluding the null terminator) if a value other than NULL is passed.
		/// </para>
		/// </param>
		/// <param name="size">
		/// <para>
		/// Returns the size of the uniform variable.
		/// </para>
		/// </param>
		/// <param name="type">
		/// <para>
		/// Returns the data type of the uniform variable.
		/// </para>
		/// </param>
		/// <param name="name">
		/// <para>
		/// Returns a null terminated string containing the name of the uniform variable.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetActiveUniform")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void GetActiveUniform(UInt32 program, UInt32 index, Int32 bufsize, [OutAttribute] out Int32 length, [OutAttribute] out Int32 size, [OutAttribute] out System.Graphics.ES20.ActiveUniformType type, [OutAttribute] StringBuilder name) {
			unsafe
			{
				fixed (Int32* length_ptr = &length)
				fixed (Int32* size_ptr = &size)
				fixed (System.Graphics.ES20.ActiveUniformType* type_ptr = &type) {
					Delegates.glGetActiveUniform((UInt32) program, (UInt32) index, (Int32) bufsize, (Int32*) length_ptr, (Int32*) size_ptr, (System.Graphics.ES20.ActiveUniformType*) type_ptr, (StringBuilder) name);
					length = *length_ptr;
					size = *size_ptr;
					type = *type_ptr;
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns information about an active uniform variable for the specified program object
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to be queried.
		/// </para>
		/// </param>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the uniform variable to be queried.
		/// </para>
		/// </param>
		/// <param name="bufSize">
		/// <para>
		/// Specifies the maximum number of characters OpenGL is allowed to write in the character buffer indicated by name.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Returns the number of characters actually written by OpenGL in the string indicated by name (excluding the null terminator) if a value other than NULL is passed.
		/// </para>
		/// </param>
		/// <param name="size">
		/// <para>
		/// Returns the size of the uniform variable.
		/// </para>
		/// </param>
		/// <param name="type">
		/// <para>
		/// Returns the data type of the uniform variable.
		/// </para>
		/// </param>
		/// <param name="name">
		/// <para>
		/// Returns a null terminated string containing the name of the uniform variable.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetActiveUniform")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void GetActiveUniform(UInt32 program, UInt32 index, Int32 bufsize, [OutAttribute] Int32* length, [OutAttribute] Int32* size, [OutAttribute] System.Graphics.ES20.ActiveUniformType* type, [OutAttribute] StringBuilder name) {
			Delegates.glGetActiveUniform((UInt32) program, (UInt32) index, (Int32) bufsize, (Int32*) length, (Int32*) size, (System.Graphics.ES20.ActiveUniformType*) type, (StringBuilder) name);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns the handles of the shader objects attached to a program object
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to be queried.
		/// </para>
		/// </param>
		/// <param name="maxCount">
		/// <para>
		/// Specifies the size of the array for storing the returned object names.
		/// </para>
		/// </param>
		/// <param name="count">
		/// <para>
		/// Returns the number of names actually returned in objects.
		/// </para>
		/// </param>
		/// <param name="shaders">
		/// <para>
		/// Specifies an array that is used to return the names of attached shader objects.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetAttachedShaders")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetAttachedShaders(Int32 program, Int32 maxcount, [OutAttribute] Int32[] count, [OutAttribute] Int32[] shaders) {
			unsafe
			{
				fixed (Int32* count_ptr = count)
				fixed (Int32* shaders_ptr = shaders) {
					Delegates.glGetAttachedShaders((UInt32) program, (Int32) maxcount, (Int32*) count_ptr, (UInt32*) shaders_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns the handles of the shader objects attached to a program object
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to be queried.
		/// </para>
		/// </param>
		/// <param name="maxCount">
		/// <para>
		/// Specifies the size of the array for storing the returned object names.
		/// </para>
		/// </param>
		/// <param name="count">
		/// <para>
		/// Returns the number of names actually returned in objects.
		/// </para>
		/// </param>
		/// <param name="shaders">
		/// <para>
		/// Specifies an array that is used to return the names of attached shader objects.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetAttachedShaders")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetAttachedShaders(Int32 program, Int32 maxcount, [OutAttribute] out Int32 count, [OutAttribute] out Int32 shaders) {
			unsafe
			{
				fixed (Int32* count_ptr = &count)
				fixed (Int32* shaders_ptr = &shaders) {
					Delegates.glGetAttachedShaders((UInt32) program, (Int32) maxcount, (Int32*) count_ptr, (UInt32*) shaders_ptr);
					count = *count_ptr;
					shaders = *shaders_ptr;
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns the handles of the shader objects attached to a program object
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to be queried.
		/// </para>
		/// </param>
		/// <param name="maxCount">
		/// <para>
		/// Specifies the size of the array for storing the returned object names.
		/// </para>
		/// </param>
		/// <param name="count">
		/// <para>
		/// Returns the number of names actually returned in objects.
		/// </para>
		/// </param>
		/// <param name="shaders">
		/// <para>
		/// Specifies an array that is used to return the names of attached shader objects.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetAttachedShaders")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void GetAttachedShaders(Int32 program, Int32 maxcount, [OutAttribute] Int32* count, [OutAttribute] Int32* shaders) {
			Delegates.glGetAttachedShaders((UInt32) program, (Int32) maxcount, (Int32*) count, (UInt32*) shaders);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns the handles of the shader objects attached to a program object
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to be queried.
		/// </para>
		/// </param>
		/// <param name="maxCount">
		/// <para>
		/// Specifies the size of the array for storing the returned object names.
		/// </para>
		/// </param>
		/// <param name="count">
		/// <para>
		/// Returns the number of names actually returned in objects.
		/// </para>
		/// </param>
		/// <param name="shaders">
		/// <para>
		/// Specifies an array that is used to return the names of attached shader objects.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetAttachedShaders")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void GetAttachedShaders(UInt32 program, Int32 maxcount, [OutAttribute] Int32[] count, [OutAttribute] UInt32[] shaders) {
			unsafe
			{
				fixed (Int32* count_ptr = count)
				fixed (UInt32* shaders_ptr = shaders) {
					Delegates.glGetAttachedShaders((UInt32) program, (Int32) maxcount, (Int32*) count_ptr, (UInt32*) shaders_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns the handles of the shader objects attached to a program object
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to be queried.
		/// </para>
		/// </param>
		/// <param name="maxCount">
		/// <para>
		/// Specifies the size of the array for storing the returned object names.
		/// </para>
		/// </param>
		/// <param name="count">
		/// <para>
		/// Returns the number of names actually returned in objects.
		/// </para>
		/// </param>
		/// <param name="shaders">
		/// <para>
		/// Specifies an array that is used to return the names of attached shader objects.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetAttachedShaders")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void GetAttachedShaders(UInt32 program, Int32 maxcount, [OutAttribute] out Int32 count, [OutAttribute] out UInt32 shaders) {
			unsafe
			{
				fixed (Int32* count_ptr = &count)
				fixed (UInt32* shaders_ptr = &shaders) {
					Delegates.glGetAttachedShaders((UInt32) program, (Int32) maxcount, (Int32*) count_ptr, (UInt32*) shaders_ptr);
					count = *count_ptr;
					shaders = *shaders_ptr;
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns the handles of the shader objects attached to a program object
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to be queried.
		/// </para>
		/// </param>
		/// <param name="maxCount">
		/// <para>
		/// Specifies the size of the array for storing the returned object names.
		/// </para>
		/// </param>
		/// <param name="count">
		/// <para>
		/// Returns the number of names actually returned in objects.
		/// </para>
		/// </param>
		/// <param name="shaders">
		/// <para>
		/// Specifies an array that is used to return the names of attached shader objects.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetAttachedShaders")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void GetAttachedShaders(UInt32 program, Int32 maxcount, [OutAttribute] Int32* count, [OutAttribute] UInt32* shaders) {
			Delegates.glGetAttachedShaders((UInt32) program, (Int32) maxcount, (Int32*) count, (UInt32*) shaders);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns the location of an attribute variable
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to be queried.
		/// </para>
		/// </param>
		/// <param name="name">
		/// <para>
		/// Points to a null terminated string containing the name of the attribute variable whose location is to be queried.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetAttribLocation")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				int GetAttribLocation(Int32 program, String name) {
			return Delegates.glGetAttribLocation((UInt32) program, (String) name);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns the location of an attribute variable
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to be queried.
		/// </para>
		/// </param>
		/// <param name="name">
		/// <para>
		/// Points to a null terminated string containing the name of the attribute variable whose location is to be queried.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetAttribLocation")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		int GetAttribLocation(UInt32 program, String name) {
			return Delegates.glGetAttribLocation((UInt32) program, (String) name);
		}

		/// <summary>[requires: v2.0 and 2.0]</summary>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetBooleanv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetBoolean(System.Graphics.ES20.GetPName pname, [OutAttribute] bool[] @params) {
			unsafe
			{
				fixed (bool* @params_ptr = @params) {
					Delegates.glGetBooleanv((System.Graphics.ES20.GetPName) pname, (bool*) @params_ptr);
				}
			}
		}

		/// <summary>[requires: v2.0 and 2.0]</summary>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetBooleanv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetBoolean(System.Graphics.ES20.GetPName pname, [OutAttribute] out bool @params) {
			unsafe
			{
				fixed (bool* @params_ptr = &@params) {
					Delegates.glGetBooleanv((System.Graphics.ES20.GetPName) pname, (bool*) @params_ptr);
					@params = *@params_ptr;
				}
			}
		}

		/// <summary>[requires: v2.0 and 2.0]</summary>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetBooleanv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void GetBoolean(System.Graphics.ES20.GetPName pname, [OutAttribute] bool* @params) {
			Delegates.glGetBooleanv((System.Graphics.ES20.GetPName) pname, (bool*) @params);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Return parameters of a buffer object
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target buffer object. The symbolic constant must be GL_ARRAY_BUFFER, GL_ELEMENT_ARRAY_BUFFER, GL_PIXEL_PACK_BUFFER, or GL_PIXEL_UNPACK_BUFFER.
		/// </para>
		/// </param>
		/// <param name="value">
		/// <para>
		/// Specifies the symbolic name of a buffer object parameter. Accepted values are GL_BUFFER_ACCESS, GL_BUFFER_MAPPED, GL_BUFFER_SIZE, or GL_BUFFER_USAGE.
		/// </para>
		/// </param>
		/// <param name="data">
		/// <para>
		/// Returns the requested parameter.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetBufferParameteriv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetBufferParameter(System.Graphics.ES20.BufferTarget target, System.Graphics.ES20.BufferParameterName pname, [OutAttribute] Int32[] @params) {
			unsafe
			{
				fixed (Int32* @params_ptr = @params) {
					Delegates.glGetBufferParameteriv((System.Graphics.ES20.BufferTarget) target, (System.Graphics.ES20.BufferParameterName) pname, (Int32*) @params_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Return parameters of a buffer object
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target buffer object. The symbolic constant must be GL_ARRAY_BUFFER, GL_ELEMENT_ARRAY_BUFFER, GL_PIXEL_PACK_BUFFER, or GL_PIXEL_UNPACK_BUFFER.
		/// </para>
		/// </param>
		/// <param name="value">
		/// <para>
		/// Specifies the symbolic name of a buffer object parameter. Accepted values are GL_BUFFER_ACCESS, GL_BUFFER_MAPPED, GL_BUFFER_SIZE, or GL_BUFFER_USAGE.
		/// </para>
		/// </param>
		/// <param name="data">
		/// <para>
		/// Returns the requested parameter.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetBufferParameteriv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetBufferParameter(System.Graphics.ES20.BufferTarget target, System.Graphics.ES20.BufferParameterName pname, [OutAttribute] out Int32 @params) {
			unsafe
			{
				fixed (Int32* @params_ptr = &@params) {
					Delegates.glGetBufferParameteriv((System.Graphics.ES20.BufferTarget) target, (System.Graphics.ES20.BufferParameterName) pname, (Int32*) @params_ptr);
					@params = *@params_ptr;
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Return parameters of a buffer object
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target buffer object. The symbolic constant must be GL_ARRAY_BUFFER, GL_ELEMENT_ARRAY_BUFFER, GL_PIXEL_PACK_BUFFER, or GL_PIXEL_UNPACK_BUFFER.
		/// </para>
		/// </param>
		/// <param name="value">
		/// <para>
		/// Specifies the symbolic name of a buffer object parameter. Accepted values are GL_BUFFER_ACCESS, GL_BUFFER_MAPPED, GL_BUFFER_SIZE, or GL_BUFFER_USAGE.
		/// </para>
		/// </param>
		/// <param name="data">
		/// <para>
		/// Returns the requested parameter.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetBufferParameteriv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void GetBufferParameter(System.Graphics.ES20.BufferTarget target, System.Graphics.ES20.BufferParameterName pname, [OutAttribute] Int32* @params) {
			Delegates.glGetBufferParameteriv((System.Graphics.ES20.BufferTarget) target, (System.Graphics.ES20.BufferParameterName) pname, (Int32*) @params);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Return error information
		/// </summary>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetError")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				System.Graphics.ES20.ErrorCode GetError() {
			return Delegates.glGetError();
		}

		/// <summary>[requires: v2.0 and 2.0]</summary>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetFloatv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetFloat(System.Graphics.ES20.GetPName pname, [OutAttribute] Single[] @params) {
			unsafe
			{
				fixed (Single* @params_ptr = @params) {
					Delegates.glGetFloatv((System.Graphics.ES20.GetPName) pname, (Single*) @params_ptr);
				}
			}
		}

		/// <summary>[requires: v2.0 and 2.0]</summary>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetFloatv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetFloat(System.Graphics.ES20.GetPName pname, [OutAttribute] out Single @params) {
			unsafe
			{
				fixed (Single* @params_ptr = &@params) {
					Delegates.glGetFloatv((System.Graphics.ES20.GetPName) pname, (Single*) @params_ptr);
					@params = *@params_ptr;
				}
			}
		}

		/// <summary>[requires: v2.0 and 2.0]</summary>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetFloatv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void GetFloat(System.Graphics.ES20.GetPName pname, [OutAttribute] Single* @params) {
			Delegates.glGetFloatv((System.Graphics.ES20.GetPName) pname, (Single*) @params);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Retrieve information about attachments of a bound framebuffer object
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target of the query operation.
		/// </para>
		/// </param>
		/// <param name="attachment">
		/// <para>
		/// Specifies the attachment within target
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the parameter of attachment to query.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Specifies the address of a variable receive the value of pname for attachment.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetFramebufferAttachmentParameteriv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetFramebufferAttachmentParameter(System.Graphics.ES20.FramebufferTarget target, System.Graphics.ES20.FramebufferSlot attachment, System.Graphics.ES20.FramebufferParameterName pname, [OutAttribute] Int32[] @params) {
			unsafe
			{
				fixed (Int32* @params_ptr = @params) {
					Delegates.glGetFramebufferAttachmentParameteriv((System.Graphics.ES20.FramebufferTarget) target, (System.Graphics.ES20.FramebufferSlot) attachment, (System.Graphics.ES20.FramebufferParameterName) pname, (Int32*) @params_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Retrieve information about attachments of a bound framebuffer object
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target of the query operation.
		/// </para>
		/// </param>
		/// <param name="attachment">
		/// <para>
		/// Specifies the attachment within target
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the parameter of attachment to query.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Specifies the address of a variable receive the value of pname for attachment.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetFramebufferAttachmentParameteriv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetFramebufferAttachmentParameter(System.Graphics.ES20.FramebufferTarget target, System.Graphics.ES20.FramebufferSlot attachment, System.Graphics.ES20.FramebufferParameterName pname, [OutAttribute] out Int32 @params) {
			unsafe
			{
				fixed (Int32* @params_ptr = &@params) {
					Delegates.glGetFramebufferAttachmentParameteriv((System.Graphics.ES20.FramebufferTarget) target, (System.Graphics.ES20.FramebufferSlot) attachment, (System.Graphics.ES20.FramebufferParameterName) pname, (Int32*) @params_ptr);
					@params = *@params_ptr;
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Retrieve information about attachments of a bound framebuffer object
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target of the query operation.
		/// </para>
		/// </param>
		/// <param name="attachment">
		/// <para>
		/// Specifies the attachment within target
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the parameter of attachment to query.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Specifies the address of a variable receive the value of pname for attachment.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetFramebufferAttachmentParameteriv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void GetFramebufferAttachmentParameter(System.Graphics.ES20.FramebufferTarget target, System.Graphics.ES20.FramebufferSlot attachment, System.Graphics.ES20.FramebufferParameterName pname, [OutAttribute] Int32* @params) {
			Delegates.glGetFramebufferAttachmentParameteriv((System.Graphics.ES20.FramebufferTarget) target, (System.Graphics.ES20.FramebufferSlot) attachment, (System.Graphics.ES20.FramebufferParameterName) pname, (Int32*) @params);
		}

		/// <summary>[requires: v2.0 and 2.0]</summary>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetIntegerv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetInteger(System.Graphics.ES20.GetPName pname, [OutAttribute] Int32[] @params) {
			unsafe
			{
				fixed (Int32* @params_ptr = @params) {
					Delegates.glGetIntegerv((System.Graphics.ES20.GetPName) pname, (Int32*) @params_ptr);
				}
			}
		}

		/// <summary>[requires: v2.0 and 2.0]</summary>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetIntegerv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetInteger(System.Graphics.ES20.GetPName pname, [OutAttribute] out Int32 @params) {
			unsafe
			{
				fixed (Int32* @params_ptr = &@params) {
					Delegates.glGetIntegerv((System.Graphics.ES20.GetPName) pname, (Int32*) @params_ptr);
					@params = *@params_ptr;
				}
			}
		}

		/// <summary>[requires: v2.0 and 2.0]</summary>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetIntegerv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void GetInteger(System.Graphics.ES20.GetPName pname, [OutAttribute] Int32* @params) {
			Delegates.glGetIntegerv((System.Graphics.ES20.GetPName) pname, (Int32*) @params);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns the information log for a program object
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object whose information log is to be queried.
		/// </para>
		/// </param>
		/// <param name="maxLength">
		/// <para>
		/// Specifies the size of the character buffer for storing the returned information log.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Returns the length of the string returned in infoLog (excluding the null terminator).
		/// </para>
		/// </param>
		/// <param name="infoLog">
		/// <para>
		/// Specifies an array of characters that is used to return the information log.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetProgramInfoLog")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetProgramInfoLog(Int32 program, Int32 bufsize, [OutAttribute] Int32[] length, [OutAttribute] StringBuilder infolog) {
			unsafe
			{
				fixed (Int32* length_ptr = length) {
					Delegates.glGetProgramInfoLog((UInt32) program, (Int32) bufsize, (Int32*) length_ptr, (StringBuilder) infolog);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns the information log for a program object
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object whose information log is to be queried.
		/// </para>
		/// </param>
		/// <param name="maxLength">
		/// <para>
		/// Specifies the size of the character buffer for storing the returned information log.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Returns the length of the string returned in infoLog (excluding the null terminator).
		/// </para>
		/// </param>
		/// <param name="infoLog">
		/// <para>
		/// Specifies an array of characters that is used to return the information log.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetProgramInfoLog")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetProgramInfoLog(Int32 program, Int32 bufsize, [OutAttribute] out Int32 length, [OutAttribute] StringBuilder infolog) {
			unsafe
			{
				fixed (Int32* length_ptr = &length) {
					Delegates.glGetProgramInfoLog((UInt32) program, (Int32) bufsize, (Int32*) length_ptr, (StringBuilder) infolog);
					length = *length_ptr;
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns the information log for a program object
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object whose information log is to be queried.
		/// </para>
		/// </param>
		/// <param name="maxLength">
		/// <para>
		/// Specifies the size of the character buffer for storing the returned information log.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Returns the length of the string returned in infoLog (excluding the null terminator).
		/// </para>
		/// </param>
		/// <param name="infoLog">
		/// <para>
		/// Specifies an array of characters that is used to return the information log.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetProgramInfoLog")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void GetProgramInfoLog(Int32 program, Int32 bufsize, [OutAttribute] Int32* length, [OutAttribute] StringBuilder infolog) {
			Delegates.glGetProgramInfoLog((UInt32) program, (Int32) bufsize, (Int32*) length, (StringBuilder) infolog);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns the information log for a program object
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object whose information log is to be queried.
		/// </para>
		/// </param>
		/// <param name="maxLength">
		/// <para>
		/// Specifies the size of the character buffer for storing the returned information log.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Returns the length of the string returned in infoLog (excluding the null terminator).
		/// </para>
		/// </param>
		/// <param name="infoLog">
		/// <para>
		/// Specifies an array of characters that is used to return the information log.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetProgramInfoLog")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void GetProgramInfoLog(UInt32 program, Int32 bufsize, [OutAttribute] Int32[] length, [OutAttribute] StringBuilder infolog) {
			unsafe
			{
				fixed (Int32* length_ptr = length) {
					Delegates.glGetProgramInfoLog((UInt32) program, (Int32) bufsize, (Int32*) length_ptr, (StringBuilder) infolog);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns the information log for a program object
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object whose information log is to be queried.
		/// </para>
		/// </param>
		/// <param name="maxLength">
		/// <para>
		/// Specifies the size of the character buffer for storing the returned information log.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Returns the length of the string returned in infoLog (excluding the null terminator).
		/// </para>
		/// </param>
		/// <param name="infoLog">
		/// <para>
		/// Specifies an array of characters that is used to return the information log.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetProgramInfoLog")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void GetProgramInfoLog(UInt32 program, Int32 bufsize, [OutAttribute] out Int32 length, [OutAttribute] StringBuilder infolog) {
			unsafe
			{
				fixed (Int32* length_ptr = &length) {
					Delegates.glGetProgramInfoLog((UInt32) program, (Int32) bufsize, (Int32*) length_ptr, (StringBuilder) infolog);
					length = *length_ptr;
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns the information log for a program object
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object whose information log is to be queried.
		/// </para>
		/// </param>
		/// <param name="maxLength">
		/// <para>
		/// Specifies the size of the character buffer for storing the returned information log.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Returns the length of the string returned in infoLog (excluding the null terminator).
		/// </para>
		/// </param>
		/// <param name="infoLog">
		/// <para>
		/// Specifies an array of characters that is used to return the information log.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetProgramInfoLog")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void GetProgramInfoLog(UInt32 program, Int32 bufsize, [OutAttribute] Int32* length, [OutAttribute] StringBuilder infolog) {
			Delegates.glGetProgramInfoLog((UInt32) program, (Int32) bufsize, (Int32*) length, (StringBuilder) infolog);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns a parameter from a program object
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to be queried.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the object parameter. Accepted symbolic names are GL_DELETE_STATUS, GL_LINK_STATUS, GL_VALIDATE_STATUS, GL_INFO_LOG_LENGTH, GL_ATTACHED_SHADERS, GL_ACTIVE_ATTRIBUTES, GL_ACTIVE_ATTRIBUTE_MAX_LENGTH, GL_ACTIVE_UNIFORMS, GL_ACTIVE_UNIFORM_BLOCKS, GL_ACTIVE_UNIFORM_BLOCK_MAX_NAME_LENGTH, GL_ACTIVE_UNIFORM_MAX_LENGTH, GL_PROGRAM_BINARY_LENGTH, GL_TRANSFORM_FEEDBACK_BUFFER_MODE, GL_TRANSFORM_FEEDBACK_VARYINGS, GL_TRANSFORM_FEEDBACK_VARYING_MAX_LENGTH, GL_GEOMETRY_VERTICES_OUT, GL_GEOMETRY_INPUT_TYPE, and GL_GEOMETRY_OUTPUT_TYPE.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Returns the requested object parameter.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetProgramiv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetProgram(Int32 program, System.Graphics.ES20.ProgramParameter pname, [OutAttribute] Int32[] @params) {
			unsafe
			{
				fixed (Int32* @params_ptr = @params) {
					Delegates.glGetProgramiv((UInt32) program, (System.Graphics.ES20.ProgramParameter) pname, (Int32*) @params_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns a parameter from a program object
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to be queried.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the object parameter. Accepted symbolic names are GL_DELETE_STATUS, GL_LINK_STATUS, GL_VALIDATE_STATUS, GL_INFO_LOG_LENGTH, GL_ATTACHED_SHADERS, GL_ACTIVE_ATTRIBUTES, GL_ACTIVE_ATTRIBUTE_MAX_LENGTH, GL_ACTIVE_UNIFORMS, GL_ACTIVE_UNIFORM_BLOCKS, GL_ACTIVE_UNIFORM_BLOCK_MAX_NAME_LENGTH, GL_ACTIVE_UNIFORM_MAX_LENGTH, GL_PROGRAM_BINARY_LENGTH, GL_TRANSFORM_FEEDBACK_BUFFER_MODE, GL_TRANSFORM_FEEDBACK_VARYINGS, GL_TRANSFORM_FEEDBACK_VARYING_MAX_LENGTH, GL_GEOMETRY_VERTICES_OUT, GL_GEOMETRY_INPUT_TYPE, and GL_GEOMETRY_OUTPUT_TYPE.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Returns the requested object parameter.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetProgramiv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetProgram(Int32 program, System.Graphics.ES20.ProgramParameter pname, [OutAttribute] out Int32 @params) {
			unsafe
			{
				fixed (Int32* @params_ptr = &@params) {
					Delegates.glGetProgramiv((UInt32) program, (System.Graphics.ES20.ProgramParameter) pname, (Int32*) @params_ptr);
					@params = *@params_ptr;
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns a parameter from a program object
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to be queried.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the object parameter. Accepted symbolic names are GL_DELETE_STATUS, GL_LINK_STATUS, GL_VALIDATE_STATUS, GL_INFO_LOG_LENGTH, GL_ATTACHED_SHADERS, GL_ACTIVE_ATTRIBUTES, GL_ACTIVE_ATTRIBUTE_MAX_LENGTH, GL_ACTIVE_UNIFORMS, GL_ACTIVE_UNIFORM_BLOCKS, GL_ACTIVE_UNIFORM_BLOCK_MAX_NAME_LENGTH, GL_ACTIVE_UNIFORM_MAX_LENGTH, GL_PROGRAM_BINARY_LENGTH, GL_TRANSFORM_FEEDBACK_BUFFER_MODE, GL_TRANSFORM_FEEDBACK_VARYINGS, GL_TRANSFORM_FEEDBACK_VARYING_MAX_LENGTH, GL_GEOMETRY_VERTICES_OUT, GL_GEOMETRY_INPUT_TYPE, and GL_GEOMETRY_OUTPUT_TYPE.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Returns the requested object parameter.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetProgramiv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void GetProgram(Int32 program, System.Graphics.ES20.ProgramParameter pname, [OutAttribute] Int32* @params) {
			Delegates.glGetProgramiv((UInt32) program, (System.Graphics.ES20.ProgramParameter) pname, (Int32*) @params);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns a parameter from a program object
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to be queried.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the object parameter. Accepted symbolic names are GL_DELETE_STATUS, GL_LINK_STATUS, GL_VALIDATE_STATUS, GL_INFO_LOG_LENGTH, GL_ATTACHED_SHADERS, GL_ACTIVE_ATTRIBUTES, GL_ACTIVE_ATTRIBUTE_MAX_LENGTH, GL_ACTIVE_UNIFORMS, GL_ACTIVE_UNIFORM_BLOCKS, GL_ACTIVE_UNIFORM_BLOCK_MAX_NAME_LENGTH, GL_ACTIVE_UNIFORM_MAX_LENGTH, GL_PROGRAM_BINARY_LENGTH, GL_TRANSFORM_FEEDBACK_BUFFER_MODE, GL_TRANSFORM_FEEDBACK_VARYINGS, GL_TRANSFORM_FEEDBACK_VARYING_MAX_LENGTH, GL_GEOMETRY_VERTICES_OUT, GL_GEOMETRY_INPUT_TYPE, and GL_GEOMETRY_OUTPUT_TYPE.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Returns the requested object parameter.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetProgramiv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void GetProgram(UInt32 program, System.Graphics.ES20.ProgramParameter pname, [OutAttribute] Int32[] @params) {
			unsafe
			{
				fixed (Int32* @params_ptr = @params) {
					Delegates.glGetProgramiv((UInt32) program, (System.Graphics.ES20.ProgramParameter) pname, (Int32*) @params_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns a parameter from a program object
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to be queried.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the object parameter. Accepted symbolic names are GL_DELETE_STATUS, GL_LINK_STATUS, GL_VALIDATE_STATUS, GL_INFO_LOG_LENGTH, GL_ATTACHED_SHADERS, GL_ACTIVE_ATTRIBUTES, GL_ACTIVE_ATTRIBUTE_MAX_LENGTH, GL_ACTIVE_UNIFORMS, GL_ACTIVE_UNIFORM_BLOCKS, GL_ACTIVE_UNIFORM_BLOCK_MAX_NAME_LENGTH, GL_ACTIVE_UNIFORM_MAX_LENGTH, GL_PROGRAM_BINARY_LENGTH, GL_TRANSFORM_FEEDBACK_BUFFER_MODE, GL_TRANSFORM_FEEDBACK_VARYINGS, GL_TRANSFORM_FEEDBACK_VARYING_MAX_LENGTH, GL_GEOMETRY_VERTICES_OUT, GL_GEOMETRY_INPUT_TYPE, and GL_GEOMETRY_OUTPUT_TYPE.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Returns the requested object parameter.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetProgramiv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void GetProgram(UInt32 program, System.Graphics.ES20.ProgramParameter pname, [OutAttribute] out Int32 @params) {
			unsafe
			{
				fixed (Int32* @params_ptr = &@params) {
					Delegates.glGetProgramiv((UInt32) program, (System.Graphics.ES20.ProgramParameter) pname, (Int32*) @params_ptr);
					@params = *@params_ptr;
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns a parameter from a program object
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to be queried.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the object parameter. Accepted symbolic names are GL_DELETE_STATUS, GL_LINK_STATUS, GL_VALIDATE_STATUS, GL_INFO_LOG_LENGTH, GL_ATTACHED_SHADERS, GL_ACTIVE_ATTRIBUTES, GL_ACTIVE_ATTRIBUTE_MAX_LENGTH, GL_ACTIVE_UNIFORMS, GL_ACTIVE_UNIFORM_BLOCKS, GL_ACTIVE_UNIFORM_BLOCK_MAX_NAME_LENGTH, GL_ACTIVE_UNIFORM_MAX_LENGTH, GL_PROGRAM_BINARY_LENGTH, GL_TRANSFORM_FEEDBACK_BUFFER_MODE, GL_TRANSFORM_FEEDBACK_VARYINGS, GL_TRANSFORM_FEEDBACK_VARYING_MAX_LENGTH, GL_GEOMETRY_VERTICES_OUT, GL_GEOMETRY_INPUT_TYPE, and GL_GEOMETRY_OUTPUT_TYPE.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Returns the requested object parameter.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetProgramiv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void GetProgram(UInt32 program, System.Graphics.ES20.ProgramParameter pname, [OutAttribute] Int32* @params) {
			Delegates.glGetProgramiv((UInt32) program, (System.Graphics.ES20.ProgramParameter) pname, (Int32*) @params);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Retrieve information about a bound renderbuffer object
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target of the query operation. target must be GL_RENDERBUFFER.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the parameter whose value to retrieve from the renderbuffer bound to target.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Specifies the address of an array to receive the value of the queried parameter.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetRenderbufferParameteriv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetRenderbufferParameter(System.Graphics.ES20.RenderbufferTarget target, System.Graphics.ES20.RenderbufferParameterName pname, [OutAttribute] Int32[] @params) {
			unsafe
			{
				fixed (Int32* @params_ptr = @params) {
					Delegates.glGetRenderbufferParameteriv((System.Graphics.ES20.RenderbufferTarget) target, (System.Graphics.ES20.RenderbufferParameterName) pname, (Int32*) @params_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Retrieve information about a bound renderbuffer object
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target of the query operation. target must be GL_RENDERBUFFER.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the parameter whose value to retrieve from the renderbuffer bound to target.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Specifies the address of an array to receive the value of the queried parameter.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetRenderbufferParameteriv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetRenderbufferParameter(System.Graphics.ES20.RenderbufferTarget target, System.Graphics.ES20.RenderbufferParameterName pname, [OutAttribute] out Int32 @params) {
			unsafe
			{
				fixed (Int32* @params_ptr = &@params) {
					Delegates.glGetRenderbufferParameteriv((System.Graphics.ES20.RenderbufferTarget) target, (System.Graphics.ES20.RenderbufferParameterName) pname, (Int32*) @params_ptr);
					@params = *@params_ptr;
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Retrieve information about a bound renderbuffer object
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target of the query operation. target must be GL_RENDERBUFFER.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the parameter whose value to retrieve from the renderbuffer bound to target.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Specifies the address of an array to receive the value of the queried parameter.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetRenderbufferParameteriv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void GetRenderbufferParameter(System.Graphics.ES20.RenderbufferTarget target, System.Graphics.ES20.RenderbufferParameterName pname, [OutAttribute] Int32* @params) {
			Delegates.glGetRenderbufferParameteriv((System.Graphics.ES20.RenderbufferTarget) target, (System.Graphics.ES20.RenderbufferParameterName) pname, (Int32*) @params);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns the information log for a shader object
		/// </summary>
		/// <param name="shader">
		/// <para>
		/// Specifies the shader object whose information log is to be queried.
		/// </para>
		/// </param>
		/// <param name="maxLength">
		/// <para>
		/// Specifies the size of the character buffer for storing the returned information log.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Returns the length of the string returned in infoLog (excluding the null terminator).
		/// </para>
		/// </param>
		/// <param name="infoLog">
		/// <para>
		/// Specifies an array of characters that is used to return the information log.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetShaderInfoLog")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetShaderInfoLog(Int32 shader, Int32 bufsize, [OutAttribute] Int32[] length, [OutAttribute] StringBuilder infolog) {
			unsafe
			{
				fixed (Int32* length_ptr = length) {
					Delegates.glGetShaderInfoLog((UInt32) shader, (Int32) bufsize, (Int32*) length_ptr, (StringBuilder) infolog);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns the information log for a shader object
		/// </summary>
		/// <param name="shader">
		/// <para>
		/// Specifies the shader object whose information log is to be queried.
		/// </para>
		/// </param>
		/// <param name="maxLength">
		/// <para>
		/// Specifies the size of the character buffer for storing the returned information log.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Returns the length of the string returned in infoLog (excluding the null terminator).
		/// </para>
		/// </param>
		/// <param name="infoLog">
		/// <para>
		/// Specifies an array of characters that is used to return the information log.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetShaderInfoLog")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetShaderInfoLog(Int32 shader, Int32 bufsize, [OutAttribute] out Int32 length, [OutAttribute] StringBuilder infolog) {
			unsafe
			{
				fixed (Int32* length_ptr = &length) {
					Delegates.glGetShaderInfoLog((UInt32) shader, (Int32) bufsize, (Int32*) length_ptr, (StringBuilder) infolog);
					length = *length_ptr;
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns the information log for a shader object
		/// </summary>
		/// <param name="shader">
		/// <para>
		/// Specifies the shader object whose information log is to be queried.
		/// </para>
		/// </param>
		/// <param name="maxLength">
		/// <para>
		/// Specifies the size of the character buffer for storing the returned information log.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Returns the length of the string returned in infoLog (excluding the null terminator).
		/// </para>
		/// </param>
		/// <param name="infoLog">
		/// <para>
		/// Specifies an array of characters that is used to return the information log.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetShaderInfoLog")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void GetShaderInfoLog(Int32 shader, Int32 bufsize, [OutAttribute] Int32* length, [OutAttribute] StringBuilder infolog) {
			Delegates.glGetShaderInfoLog((UInt32) shader, (Int32) bufsize, (Int32*) length, (StringBuilder) infolog);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns the information log for a shader object
		/// </summary>
		/// <param name="shader">
		/// <para>
		/// Specifies the shader object whose information log is to be queried.
		/// </para>
		/// </param>
		/// <param name="maxLength">
		/// <para>
		/// Specifies the size of the character buffer for storing the returned information log.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Returns the length of the string returned in infoLog (excluding the null terminator).
		/// </para>
		/// </param>
		/// <param name="infoLog">
		/// <para>
		/// Specifies an array of characters that is used to return the information log.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetShaderInfoLog")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void GetShaderInfoLog(UInt32 shader, Int32 bufsize, [OutAttribute] Int32[] length, [OutAttribute] StringBuilder infolog) {
			unsafe
			{
				fixed (Int32* length_ptr = length) {
					Delegates.glGetShaderInfoLog((UInt32) shader, (Int32) bufsize, (Int32*) length_ptr, (StringBuilder) infolog);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns the information log for a shader object
		/// </summary>
		/// <param name="shader">
		/// <para>
		/// Specifies the shader object whose information log is to be queried.
		/// </para>
		/// </param>
		/// <param name="maxLength">
		/// <para>
		/// Specifies the size of the character buffer for storing the returned information log.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Returns the length of the string returned in infoLog (excluding the null terminator).
		/// </para>
		/// </param>
		/// <param name="infoLog">
		/// <para>
		/// Specifies an array of characters that is used to return the information log.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetShaderInfoLog")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void GetShaderInfoLog(UInt32 shader, Int32 bufsize, [OutAttribute] out Int32 length, [OutAttribute] StringBuilder infolog) {
			unsafe
			{
				fixed (Int32* length_ptr = &length) {
					Delegates.glGetShaderInfoLog((UInt32) shader, (Int32) bufsize, (Int32*) length_ptr, (StringBuilder) infolog);
					length = *length_ptr;
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns the information log for a shader object
		/// </summary>
		/// <param name="shader">
		/// <para>
		/// Specifies the shader object whose information log is to be queried.
		/// </para>
		/// </param>
		/// <param name="maxLength">
		/// <para>
		/// Specifies the size of the character buffer for storing the returned information log.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Returns the length of the string returned in infoLog (excluding the null terminator).
		/// </para>
		/// </param>
		/// <param name="infoLog">
		/// <para>
		/// Specifies an array of characters that is used to return the information log.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetShaderInfoLog")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void GetShaderInfoLog(UInt32 shader, Int32 bufsize, [OutAttribute] Int32* length, [OutAttribute] StringBuilder infolog) {
			Delegates.glGetShaderInfoLog((UInt32) shader, (Int32) bufsize, (Int32*) length, (StringBuilder) infolog);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns a parameter from a shader object
		/// </summary>
		/// <param name="shader">
		/// <para>
		/// Specifies the shader object to be queried.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the object parameter. Accepted symbolic names are GL_SHADER_TYPE, GL_DELETE_STATUS, GL_COMPILE_STATUS, GL_INFO_LOG_LENGTH, GL_SHADER_SOURCE_LENGTH.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Returns the requested object parameter.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetShaderiv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetShader(Int32 shader, System.Graphics.ES20.ShaderParameter pname, [OutAttribute] Int32[] @params) {
			unsafe
			{
				fixed (Int32* @params_ptr = @params) {
					Delegates.glGetShaderiv((UInt32) shader, (System.Graphics.ES20.ShaderParameter) pname, (Int32*) @params_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns a parameter from a shader object
		/// </summary>
		/// <param name="shader">
		/// <para>
		/// Specifies the shader object to be queried.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the object parameter. Accepted symbolic names are GL_SHADER_TYPE, GL_DELETE_STATUS, GL_COMPILE_STATUS, GL_INFO_LOG_LENGTH, GL_SHADER_SOURCE_LENGTH.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Returns the requested object parameter.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetShaderiv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetShader(Int32 shader, System.Graphics.ES20.ShaderParameter pname, [OutAttribute] out Int32 @params) {
			unsafe
			{
				fixed (Int32* @params_ptr = &@params) {
					Delegates.glGetShaderiv((UInt32) shader, (System.Graphics.ES20.ShaderParameter) pname, (Int32*) @params_ptr);
					@params = *@params_ptr;
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns a parameter from a shader object
		/// </summary>
		/// <param name="shader">
		/// <para>
		/// Specifies the shader object to be queried.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the object parameter. Accepted symbolic names are GL_SHADER_TYPE, GL_DELETE_STATUS, GL_COMPILE_STATUS, GL_INFO_LOG_LENGTH, GL_SHADER_SOURCE_LENGTH.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Returns the requested object parameter.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetShaderiv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void GetShader(Int32 shader, System.Graphics.ES20.ShaderParameter pname, [OutAttribute] Int32* @params) {
			Delegates.glGetShaderiv((UInt32) shader, (System.Graphics.ES20.ShaderParameter) pname, (Int32*) @params);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns a parameter from a shader object
		/// </summary>
		/// <param name="shader">
		/// <para>
		/// Specifies the shader object to be queried.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the object parameter. Accepted symbolic names are GL_SHADER_TYPE, GL_DELETE_STATUS, GL_COMPILE_STATUS, GL_INFO_LOG_LENGTH, GL_SHADER_SOURCE_LENGTH.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Returns the requested object parameter.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetShaderiv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void GetShader(UInt32 shader, System.Graphics.ES20.ShaderParameter pname, [OutAttribute] Int32[] @params) {
			unsafe
			{
				fixed (Int32* @params_ptr = @params) {
					Delegates.glGetShaderiv((UInt32) shader, (System.Graphics.ES20.ShaderParameter) pname, (Int32*) @params_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns a parameter from a shader object
		/// </summary>
		/// <param name="shader">
		/// <para>
		/// Specifies the shader object to be queried.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the object parameter. Accepted symbolic names are GL_SHADER_TYPE, GL_DELETE_STATUS, GL_COMPILE_STATUS, GL_INFO_LOG_LENGTH, GL_SHADER_SOURCE_LENGTH.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Returns the requested object parameter.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetShaderiv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void GetShader(UInt32 shader, System.Graphics.ES20.ShaderParameter pname, [OutAttribute] out Int32 @params) {
			unsafe
			{
				fixed (Int32* @params_ptr = &@params) {
					Delegates.glGetShaderiv((UInt32) shader, (System.Graphics.ES20.ShaderParameter) pname, (Int32*) @params_ptr);
					@params = *@params_ptr;
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns a parameter from a shader object
		/// </summary>
		/// <param name="shader">
		/// <para>
		/// Specifies the shader object to be queried.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the object parameter. Accepted symbolic names are GL_SHADER_TYPE, GL_DELETE_STATUS, GL_COMPILE_STATUS, GL_INFO_LOG_LENGTH, GL_SHADER_SOURCE_LENGTH.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Returns the requested object parameter.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetShaderiv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void GetShader(UInt32 shader, System.Graphics.ES20.ShaderParameter pname, [OutAttribute] Int32* @params) {
			Delegates.glGetShaderiv((UInt32) shader, (System.Graphics.ES20.ShaderParameter) pname, (Int32*) @params);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Retrieve the range and precision for numeric formats supported by the shader compiler
		/// </summary>
		/// <param name="shaderType">
		/// <para>
		/// Specifies the type of shader whose precision to query. shaderType must be GL_VERTEX_SHADER or GL_FRAGMENT_SHADER.
		/// </para>
		/// </param>
		/// <param name="precisionType">
		/// <para>
		/// Specifies the numeric format whose precision and range to query.
		/// </para>
		/// </param>
		/// <param name="range">
		/// <para>
		/// Specifies the address of array of two integers into which encodings of the implementation's numeric range are returned.
		/// </para>
		/// </param>
		/// <param name="precision">
		/// <para>
		/// Specifies the address of an integer into which the numeric precision of the implementation is written.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetShaderPrecisionFormat")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetShaderPrecisionFormat(System.Graphics.ES20.ShaderType shadertype, System.Graphics.ES20.ShaderPrecision precisiontype, [OutAttribute] Int32[] range, [OutAttribute] Int32[] precision) {
			unsafe
			{
				fixed (Int32* range_ptr = range)
				fixed (Int32* precision_ptr = precision) {
					Delegates.glGetShaderPrecisionFormat((System.Graphics.ES20.ShaderType) shadertype, (System.Graphics.ES20.ShaderPrecision) precisiontype, (Int32*) range_ptr, (Int32*) precision_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Retrieve the range and precision for numeric formats supported by the shader compiler
		/// </summary>
		/// <param name="shaderType">
		/// <para>
		/// Specifies the type of shader whose precision to query. shaderType must be GL_VERTEX_SHADER or GL_FRAGMENT_SHADER.
		/// </para>
		/// </param>
		/// <param name="precisionType">
		/// <para>
		/// Specifies the numeric format whose precision and range to query.
		/// </para>
		/// </param>
		/// <param name="range">
		/// <para>
		/// Specifies the address of array of two integers into which encodings of the implementation's numeric range are returned.
		/// </para>
		/// </param>
		/// <param name="precision">
		/// <para>
		/// Specifies the address of an integer into which the numeric precision of the implementation is written.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetShaderPrecisionFormat")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetShaderPrecisionFormat(System.Graphics.ES20.ShaderType shadertype, System.Graphics.ES20.ShaderPrecision precisiontype, [OutAttribute] out Int32 range, [OutAttribute] out Int32 precision) {
			unsafe
			{
				fixed (Int32* range_ptr = &range)
				fixed (Int32* precision_ptr = &precision) {
					Delegates.glGetShaderPrecisionFormat((System.Graphics.ES20.ShaderType) shadertype, (System.Graphics.ES20.ShaderPrecision) precisiontype, (Int32*) range_ptr, (Int32*) precision_ptr);
					range = *range_ptr;
					precision = *precision_ptr;
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Retrieve the range and precision for numeric formats supported by the shader compiler
		/// </summary>
		/// <param name="shaderType">
		/// <para>
		/// Specifies the type of shader whose precision to query. shaderType must be GL_VERTEX_SHADER or GL_FRAGMENT_SHADER.
		/// </para>
		/// </param>
		/// <param name="precisionType">
		/// <para>
		/// Specifies the numeric format whose precision and range to query.
		/// </para>
		/// </param>
		/// <param name="range">
		/// <para>
		/// Specifies the address of array of two integers into which encodings of the implementation's numeric range are returned.
		/// </para>
		/// </param>
		/// <param name="precision">
		/// <para>
		/// Specifies the address of an integer into which the numeric precision of the implementation is written.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetShaderPrecisionFormat")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void GetShaderPrecisionFormat(System.Graphics.ES20.ShaderType shadertype, System.Graphics.ES20.ShaderPrecision precisiontype, [OutAttribute] Int32* range, [OutAttribute] Int32* precision) {
			Delegates.glGetShaderPrecisionFormat((System.Graphics.ES20.ShaderType) shadertype, (System.Graphics.ES20.ShaderPrecision) precisiontype, (Int32*) range, (Int32*) precision);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns the source code string from a shader object
		/// </summary>
		/// <param name="shader">
		/// <para>
		/// Specifies the shader object to be queried.
		/// </para>
		/// </param>
		/// <param name="bufSize">
		/// <para>
		/// Specifies the size of the character buffer for storing the returned source code string.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Returns the length of the string returned in source (excluding the null terminator).
		/// </para>
		/// </param>
		/// <param name="source">
		/// <para>
		/// Specifies an array of characters that is used to return the source code string.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetShaderSource")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetShaderSource(Int32 shader, Int32 bufsize, [OutAttribute] Int32[] length, [OutAttribute] StringBuilder source) {
			unsafe
			{
				fixed (Int32* length_ptr = length) {
					Delegates.glGetShaderSource((UInt32) shader, (Int32) bufsize, (Int32*) length_ptr, (StringBuilder) source);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns the source code string from a shader object
		/// </summary>
		/// <param name="shader">
		/// <para>
		/// Specifies the shader object to be queried.
		/// </para>
		/// </param>
		/// <param name="bufSize">
		/// <para>
		/// Specifies the size of the character buffer for storing the returned source code string.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Returns the length of the string returned in source (excluding the null terminator).
		/// </para>
		/// </param>
		/// <param name="source">
		/// <para>
		/// Specifies an array of characters that is used to return the source code string.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetShaderSource")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetShaderSource(Int32 shader, Int32 bufsize, [OutAttribute] out Int32 length, [OutAttribute] StringBuilder source) {
			unsafe
			{
				fixed (Int32* length_ptr = &length) {
					Delegates.glGetShaderSource((UInt32) shader, (Int32) bufsize, (Int32*) length_ptr, (StringBuilder) source);
					length = *length_ptr;
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns the source code string from a shader object
		/// </summary>
		/// <param name="shader">
		/// <para>
		/// Specifies the shader object to be queried.
		/// </para>
		/// </param>
		/// <param name="bufSize">
		/// <para>
		/// Specifies the size of the character buffer for storing the returned source code string.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Returns the length of the string returned in source (excluding the null terminator).
		/// </para>
		/// </param>
		/// <param name="source">
		/// <para>
		/// Specifies an array of characters that is used to return the source code string.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetShaderSource")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void GetShaderSource(Int32 shader, Int32 bufsize, [OutAttribute] Int32* length, [OutAttribute] StringBuilder source) {
			Delegates.glGetShaderSource((UInt32) shader, (Int32) bufsize, (Int32*) length, (StringBuilder) source);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns the source code string from a shader object
		/// </summary>
		/// <param name="shader">
		/// <para>
		/// Specifies the shader object to be queried.
		/// </para>
		/// </param>
		/// <param name="bufSize">
		/// <para>
		/// Specifies the size of the character buffer for storing the returned source code string.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Returns the length of the string returned in source (excluding the null terminator).
		/// </para>
		/// </param>
		/// <param name="source">
		/// <para>
		/// Specifies an array of characters that is used to return the source code string.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetShaderSource")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void GetShaderSource(UInt32 shader, Int32 bufsize, [OutAttribute] Int32[] length, [OutAttribute] StringBuilder source) {
			unsafe
			{
				fixed (Int32* length_ptr = length) {
					Delegates.glGetShaderSource((UInt32) shader, (Int32) bufsize, (Int32*) length_ptr, (StringBuilder) source);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns the source code string from a shader object
		/// </summary>
		/// <param name="shader">
		/// <para>
		/// Specifies the shader object to be queried.
		/// </para>
		/// </param>
		/// <param name="bufSize">
		/// <para>
		/// Specifies the size of the character buffer for storing the returned source code string.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Returns the length of the string returned in source (excluding the null terminator).
		/// </para>
		/// </param>
		/// <param name="source">
		/// <para>
		/// Specifies an array of characters that is used to return the source code string.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetShaderSource")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void GetShaderSource(UInt32 shader, Int32 bufsize, [OutAttribute] out Int32 length, [OutAttribute] StringBuilder source) {
			unsafe
			{
				fixed (Int32* length_ptr = &length) {
					Delegates.glGetShaderSource((UInt32) shader, (Int32) bufsize, (Int32*) length_ptr, (StringBuilder) source);
					length = *length_ptr;
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns the source code string from a shader object
		/// </summary>
		/// <param name="shader">
		/// <para>
		/// Specifies the shader object to be queried.
		/// </para>
		/// </param>
		/// <param name="bufSize">
		/// <para>
		/// Specifies the size of the character buffer for storing the returned source code string.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Returns the length of the string returned in source (excluding the null terminator).
		/// </para>
		/// </param>
		/// <param name="source">
		/// <para>
		/// Specifies an array of characters that is used to return the source code string.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetShaderSource")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void GetShaderSource(UInt32 shader, Int32 bufsize, [OutAttribute] Int32* length, [OutAttribute] StringBuilder source) {
			Delegates.glGetShaderSource((UInt32) shader, (Int32) bufsize, (Int32*) length, (StringBuilder) source);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Return a string describing the current GL connection
		/// </summary>
		/// <param name="name">
		/// <para>
		/// Specifies a symbolic constant, one of GL_VENDOR, GL_RENDERER, GL_VERSION, or GL_SHADING_LANGUAGE_VERSION. Additionally, glGetStringi accepts the GL_EXTENSIONS token.
		/// </para>
		/// </param>
		/// <param name="index">
		/// <para>
		/// For glGetStringi, specifies the index of the string to return.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetString")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe System.String GetString(System.Graphics.ES20.StringName name) {
			unsafe
			{
				return new string((sbyte*) Delegates.glGetString((System.Graphics.ES20.StringName) name));
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Return texture parameter values
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the symbolic name of the target texture. GL_TEXTURE_1D, GL_TEXTURE_2D, GL_TEXTURE_1D_ARRAY, GL_TEXTURE_2D_ARRAY, GL_TEXTURE_3D, GL_TEXTURE_RECTANGLE, and GL_TEXTURE_CUBE_MAP are accepted.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the symbolic name of a texture parameter. GL_TEXTURE_BASE_LEVEL, GL_TEXTURE_BORDER_COLOR, GL_TEXTURE_COMPARE_MODE, GL_TEXTURE_COMPARE_FUNC, GL_TEXTURE_LOD_BIAS, GL_TEXTURE_MAG_FILTER, GL_TEXTURE_MAX_LEVEL, GL_TEXTURE_MAX_LOD, GL_TEXTURE_MIN_FILTER, GL_TEXTURE_MIN_LOD, GL_TEXTURE_SWIZZLE_R, GL_TEXTURE_SWIZZLE_G, GL_TEXTURE_SWIZZLE_B, GL_TEXTURE_SWIZZLE_A, GL_TEXTURE_SWIZZLE_RGBA, GL_TEXTURE_WRAP_S, GL_TEXTURE_WRAP_T, and GL_TEXTURE_WRAP_R are accepted.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Returns the texture parameters.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetTexParameterfv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetTexParameter(System.Graphics.ES20.TextureTarget target, System.Graphics.ES20.GetTextureParameter pname, [OutAttribute] Single[] @params) {
			unsafe
			{
				fixed (Single* @params_ptr = @params) {
					Delegates.glGetTexParameterfv((System.Graphics.ES20.TextureTarget) target, (System.Graphics.ES20.GetTextureParameter) pname, (Single*) @params_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Return texture parameter values
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the symbolic name of the target texture. GL_TEXTURE_1D, GL_TEXTURE_2D, GL_TEXTURE_1D_ARRAY, GL_TEXTURE_2D_ARRAY, GL_TEXTURE_3D, GL_TEXTURE_RECTANGLE, and GL_TEXTURE_CUBE_MAP are accepted.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the symbolic name of a texture parameter. GL_TEXTURE_BASE_LEVEL, GL_TEXTURE_BORDER_COLOR, GL_TEXTURE_COMPARE_MODE, GL_TEXTURE_COMPARE_FUNC, GL_TEXTURE_LOD_BIAS, GL_TEXTURE_MAG_FILTER, GL_TEXTURE_MAX_LEVEL, GL_TEXTURE_MAX_LOD, GL_TEXTURE_MIN_FILTER, GL_TEXTURE_MIN_LOD, GL_TEXTURE_SWIZZLE_R, GL_TEXTURE_SWIZZLE_G, GL_TEXTURE_SWIZZLE_B, GL_TEXTURE_SWIZZLE_A, GL_TEXTURE_SWIZZLE_RGBA, GL_TEXTURE_WRAP_S, GL_TEXTURE_WRAP_T, and GL_TEXTURE_WRAP_R are accepted.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Returns the texture parameters.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetTexParameterfv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetTexParameter(System.Graphics.ES20.TextureTarget target, System.Graphics.ES20.GetTextureParameter pname, [OutAttribute] out Single @params) {
			unsafe
			{
				fixed (Single* @params_ptr = &@params) {
					Delegates.glGetTexParameterfv((System.Graphics.ES20.TextureTarget) target, (System.Graphics.ES20.GetTextureParameter) pname, (Single*) @params_ptr);
					@params = *@params_ptr;
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Return texture parameter values
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the symbolic name of the target texture. GL_TEXTURE_1D, GL_TEXTURE_2D, GL_TEXTURE_1D_ARRAY, GL_TEXTURE_2D_ARRAY, GL_TEXTURE_3D, GL_TEXTURE_RECTANGLE, and GL_TEXTURE_CUBE_MAP are accepted.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the symbolic name of a texture parameter. GL_TEXTURE_BASE_LEVEL, GL_TEXTURE_BORDER_COLOR, GL_TEXTURE_COMPARE_MODE, GL_TEXTURE_COMPARE_FUNC, GL_TEXTURE_LOD_BIAS, GL_TEXTURE_MAG_FILTER, GL_TEXTURE_MAX_LEVEL, GL_TEXTURE_MAX_LOD, GL_TEXTURE_MIN_FILTER, GL_TEXTURE_MIN_LOD, GL_TEXTURE_SWIZZLE_R, GL_TEXTURE_SWIZZLE_G, GL_TEXTURE_SWIZZLE_B, GL_TEXTURE_SWIZZLE_A, GL_TEXTURE_SWIZZLE_RGBA, GL_TEXTURE_WRAP_S, GL_TEXTURE_WRAP_T, and GL_TEXTURE_WRAP_R are accepted.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Returns the texture parameters.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetTexParameterfv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void GetTexParameter(System.Graphics.ES20.TextureTarget target, System.Graphics.ES20.GetTextureParameter pname, [OutAttribute] Single* @params) {
			Delegates.glGetTexParameterfv((System.Graphics.ES20.TextureTarget) target, (System.Graphics.ES20.GetTextureParameter) pname, (Single*) @params);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Return texture parameter values
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the symbolic name of the target texture. GL_TEXTURE_1D, GL_TEXTURE_2D, GL_TEXTURE_1D_ARRAY, GL_TEXTURE_2D_ARRAY, GL_TEXTURE_3D, GL_TEXTURE_RECTANGLE, and GL_TEXTURE_CUBE_MAP are accepted.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the symbolic name of a texture parameter. GL_TEXTURE_BASE_LEVEL, GL_TEXTURE_BORDER_COLOR, GL_TEXTURE_COMPARE_MODE, GL_TEXTURE_COMPARE_FUNC, GL_TEXTURE_LOD_BIAS, GL_TEXTURE_MAG_FILTER, GL_TEXTURE_MAX_LEVEL, GL_TEXTURE_MAX_LOD, GL_TEXTURE_MIN_FILTER, GL_TEXTURE_MIN_LOD, GL_TEXTURE_SWIZZLE_R, GL_TEXTURE_SWIZZLE_G, GL_TEXTURE_SWIZZLE_B, GL_TEXTURE_SWIZZLE_A, GL_TEXTURE_SWIZZLE_RGBA, GL_TEXTURE_WRAP_S, GL_TEXTURE_WRAP_T, and GL_TEXTURE_WRAP_R are accepted.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Returns the texture parameters.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetTexParameteriv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetTexParameter(System.Graphics.ES20.TextureTarget target, System.Graphics.ES20.GetTextureParameter pname, [OutAttribute] Int32[] @params) {
			unsafe
			{
				fixed (Int32* @params_ptr = @params) {
					Delegates.glGetTexParameteriv((System.Graphics.ES20.TextureTarget) target, (System.Graphics.ES20.GetTextureParameter) pname, (Int32*) @params_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Return texture parameter values
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the symbolic name of the target texture. GL_TEXTURE_1D, GL_TEXTURE_2D, GL_TEXTURE_1D_ARRAY, GL_TEXTURE_2D_ARRAY, GL_TEXTURE_3D, GL_TEXTURE_RECTANGLE, and GL_TEXTURE_CUBE_MAP are accepted.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the symbolic name of a texture parameter. GL_TEXTURE_BASE_LEVEL, GL_TEXTURE_BORDER_COLOR, GL_TEXTURE_COMPARE_MODE, GL_TEXTURE_COMPARE_FUNC, GL_TEXTURE_LOD_BIAS, GL_TEXTURE_MAG_FILTER, GL_TEXTURE_MAX_LEVEL, GL_TEXTURE_MAX_LOD, GL_TEXTURE_MIN_FILTER, GL_TEXTURE_MIN_LOD, GL_TEXTURE_SWIZZLE_R, GL_TEXTURE_SWIZZLE_G, GL_TEXTURE_SWIZZLE_B, GL_TEXTURE_SWIZZLE_A, GL_TEXTURE_SWIZZLE_RGBA, GL_TEXTURE_WRAP_S, GL_TEXTURE_WRAP_T, and GL_TEXTURE_WRAP_R are accepted.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Returns the texture parameters.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetTexParameteriv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetTexParameter(System.Graphics.ES20.TextureTarget target, System.Graphics.ES20.GetTextureParameter pname, [OutAttribute] out Int32 @params) {
			unsafe
			{
				fixed (Int32* @params_ptr = &@params) {
					Delegates.glGetTexParameteriv((System.Graphics.ES20.TextureTarget) target, (System.Graphics.ES20.GetTextureParameter) pname, (Int32*) @params_ptr);
					@params = *@params_ptr;
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Return texture parameter values
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the symbolic name of the target texture. GL_TEXTURE_1D, GL_TEXTURE_2D, GL_TEXTURE_1D_ARRAY, GL_TEXTURE_2D_ARRAY, GL_TEXTURE_3D, GL_TEXTURE_RECTANGLE, and GL_TEXTURE_CUBE_MAP are accepted.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the symbolic name of a texture parameter. GL_TEXTURE_BASE_LEVEL, GL_TEXTURE_BORDER_COLOR, GL_TEXTURE_COMPARE_MODE, GL_TEXTURE_COMPARE_FUNC, GL_TEXTURE_LOD_BIAS, GL_TEXTURE_MAG_FILTER, GL_TEXTURE_MAX_LEVEL, GL_TEXTURE_MAX_LOD, GL_TEXTURE_MIN_FILTER, GL_TEXTURE_MIN_LOD, GL_TEXTURE_SWIZZLE_R, GL_TEXTURE_SWIZZLE_G, GL_TEXTURE_SWIZZLE_B, GL_TEXTURE_SWIZZLE_A, GL_TEXTURE_SWIZZLE_RGBA, GL_TEXTURE_WRAP_S, GL_TEXTURE_WRAP_T, and GL_TEXTURE_WRAP_R are accepted.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Returns the texture parameters.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetTexParameteriv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void GetTexParameter(System.Graphics.ES20.TextureTarget target, System.Graphics.ES20.GetTextureParameter pname, [OutAttribute] Int32* @params) {
			Delegates.glGetTexParameteriv((System.Graphics.ES20.TextureTarget) target, (System.Graphics.ES20.GetTextureParameter) pname, (Int32*) @params);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns the value of a uniform variable
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to be queried.
		/// </para>
		/// </param>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be queried.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Returns the value of the specified uniform variable.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetUniformfv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetUniform(Int32 program, Int32 location, [OutAttribute] Single[] @params) {
			unsafe
			{
				fixed (Single* @params_ptr = @params) {
					Delegates.glGetUniformfv((UInt32) program, (Int32) location, (Single*) @params_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns the value of a uniform variable
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to be queried.
		/// </para>
		/// </param>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be queried.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Returns the value of the specified uniform variable.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetUniformfv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetUniform(Int32 program, Int32 location, [OutAttribute] out Single @params) {
			unsafe
			{
				fixed (Single* @params_ptr = &@params) {
					Delegates.glGetUniformfv((UInt32) program, (Int32) location, (Single*) @params_ptr);
					@params = *@params_ptr;
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns the value of a uniform variable
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to be queried.
		/// </para>
		/// </param>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be queried.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Returns the value of the specified uniform variable.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetUniformfv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void GetUniform(Int32 program, Int32 location, [OutAttribute] Single* @params) {
			Delegates.glGetUniformfv((UInt32) program, (Int32) location, (Single*) @params);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns the value of a uniform variable
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to be queried.
		/// </para>
		/// </param>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be queried.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Returns the value of the specified uniform variable.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetUniformfv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void GetUniform(UInt32 program, Int32 location, [OutAttribute] Single[] @params) {
			unsafe
			{
				fixed (Single* @params_ptr = @params) {
					Delegates.glGetUniformfv((UInt32) program, (Int32) location, (Single*) @params_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns the value of a uniform variable
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to be queried.
		/// </para>
		/// </param>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be queried.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Returns the value of the specified uniform variable.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetUniformfv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void GetUniform(UInt32 program, Int32 location, [OutAttribute] out Single @params) {
			unsafe
			{
				fixed (Single* @params_ptr = &@params) {
					Delegates.glGetUniformfv((UInt32) program, (Int32) location, (Single*) @params_ptr);
					@params = *@params_ptr;
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns the value of a uniform variable
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to be queried.
		/// </para>
		/// </param>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be queried.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Returns the value of the specified uniform variable.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetUniformfv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void GetUniform(UInt32 program, Int32 location, [OutAttribute] Single* @params) {
			Delegates.glGetUniformfv((UInt32) program, (Int32) location, (Single*) @params);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns the value of a uniform variable
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to be queried.
		/// </para>
		/// </param>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be queried.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Returns the value of the specified uniform variable.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetUniformiv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetUniform(Int32 program, Int32 location, [OutAttribute] Int32[] @params) {
			unsafe
			{
				fixed (Int32* @params_ptr = @params) {
					Delegates.glGetUniformiv((UInt32) program, (Int32) location, (Int32*) @params_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns the value of a uniform variable
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to be queried.
		/// </para>
		/// </param>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be queried.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Returns the value of the specified uniform variable.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetUniformiv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetUniform(Int32 program, Int32 location, [OutAttribute] out Int32 @params) {
			unsafe
			{
				fixed (Int32* @params_ptr = &@params) {
					Delegates.glGetUniformiv((UInt32) program, (Int32) location, (Int32*) @params_ptr);
					@params = *@params_ptr;
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns the value of a uniform variable
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to be queried.
		/// </para>
		/// </param>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be queried.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Returns the value of the specified uniform variable.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetUniformiv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void GetUniform(Int32 program, Int32 location, [OutAttribute] Int32* @params) {
			Delegates.glGetUniformiv((UInt32) program, (Int32) location, (Int32*) @params);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns the value of a uniform variable
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to be queried.
		/// </para>
		/// </param>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be queried.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Returns the value of the specified uniform variable.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetUniformiv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void GetUniform(UInt32 program, Int32 location, [OutAttribute] Int32[] @params) {
			unsafe
			{
				fixed (Int32* @params_ptr = @params) {
					Delegates.glGetUniformiv((UInt32) program, (Int32) location, (Int32*) @params_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns the value of a uniform variable
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to be queried.
		/// </para>
		/// </param>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be queried.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Returns the value of the specified uniform variable.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetUniformiv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void GetUniform(UInt32 program, Int32 location, [OutAttribute] out Int32 @params) {
			unsafe
			{
				fixed (Int32* @params_ptr = &@params) {
					Delegates.glGetUniformiv((UInt32) program, (Int32) location, (Int32*) @params_ptr);
					@params = *@params_ptr;
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns the value of a uniform variable
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to be queried.
		/// </para>
		/// </param>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be queried.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Returns the value of the specified uniform variable.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetUniformiv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void GetUniform(UInt32 program, Int32 location, [OutAttribute] Int32* @params) {
			Delegates.glGetUniformiv((UInt32) program, (Int32) location, (Int32*) @params);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns the location of a uniform variable
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to be queried.
		/// </para>
		/// </param>
		/// <param name="name">
		/// <para>
		/// Points to a null terminated string containing the name of the uniform variable whose location is to be queried.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetUniformLocation")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				int GetUniformLocation(Int32 program, String name) {
			return Delegates.glGetUniformLocation((UInt32) program, (String) name);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Returns the location of a uniform variable
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the program object to be queried.
		/// </para>
		/// </param>
		/// <param name="name">
		/// <para>
		/// Points to a null terminated string containing the name of the uniform variable whose location is to be queried.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetUniformLocation")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		int GetUniformLocation(UInt32 program, String name) {
			return Delegates.glGetUniformLocation((UInt32) program, (String) name);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Return a generic vertex attribute parameter
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the generic vertex attribute parameter to be queried.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the symbolic name of the vertex attribute parameter to be queried. Accepted values are GL_VERTEX_ATTRIB_ARRAY_BUFFER_BINDING, GL_VERTEX_ATTRIB_ARRAY_ENABLED, GL_VERTEX_ATTRIB_ARRAY_SIZE, GL_VERTEX_ATTRIB_ARRAY_STRIDE, GL_VERTEX_ATTRIB_ARRAY_TYPE, GL_VERTEX_ATTRIB_ARRAY_NORMALIZED, GL_VERTEX_ATTRIB_ARRAY_INTEGER, GL_VERTEX_ATTRIB_ARRAY_DIVISOR, or GL_CURRENT_VERTEX_ATTRIB.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Returns the requested data.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetVertexAttribfv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetVertexAttrib(Int32 index, System.Graphics.ES20.VertexAttribParameter pname, [OutAttribute] Single[] @params) {
			unsafe
			{
				fixed (Single* @params_ptr = @params) {
					Delegates.glGetVertexAttribfv((UInt32) index, (System.Graphics.ES20.VertexAttribParameter) pname, (Single*) @params_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Return a generic vertex attribute parameter
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the generic vertex attribute parameter to be queried.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the symbolic name of the vertex attribute parameter to be queried. Accepted values are GL_VERTEX_ATTRIB_ARRAY_BUFFER_BINDING, GL_VERTEX_ATTRIB_ARRAY_ENABLED, GL_VERTEX_ATTRIB_ARRAY_SIZE, GL_VERTEX_ATTRIB_ARRAY_STRIDE, GL_VERTEX_ATTRIB_ARRAY_TYPE, GL_VERTEX_ATTRIB_ARRAY_NORMALIZED, GL_VERTEX_ATTRIB_ARRAY_INTEGER, GL_VERTEX_ATTRIB_ARRAY_DIVISOR, or GL_CURRENT_VERTEX_ATTRIB.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Returns the requested data.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetVertexAttribfv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetVertexAttrib(Int32 index, System.Graphics.ES20.VertexAttribParameter pname, [OutAttribute] out Single @params) {
			unsafe
			{
				fixed (Single* @params_ptr = &@params) {
					Delegates.glGetVertexAttribfv((UInt32) index, (System.Graphics.ES20.VertexAttribParameter) pname, (Single*) @params_ptr);
					@params = *@params_ptr;
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Return a generic vertex attribute parameter
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the generic vertex attribute parameter to be queried.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the symbolic name of the vertex attribute parameter to be queried. Accepted values are GL_VERTEX_ATTRIB_ARRAY_BUFFER_BINDING, GL_VERTEX_ATTRIB_ARRAY_ENABLED, GL_VERTEX_ATTRIB_ARRAY_SIZE, GL_VERTEX_ATTRIB_ARRAY_STRIDE, GL_VERTEX_ATTRIB_ARRAY_TYPE, GL_VERTEX_ATTRIB_ARRAY_NORMALIZED, GL_VERTEX_ATTRIB_ARRAY_INTEGER, GL_VERTEX_ATTRIB_ARRAY_DIVISOR, or GL_CURRENT_VERTEX_ATTRIB.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Returns the requested data.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetVertexAttribfv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void GetVertexAttrib(Int32 index, System.Graphics.ES20.VertexAttribParameter pname, [OutAttribute] Single* @params) {
			Delegates.glGetVertexAttribfv((UInt32) index, (System.Graphics.ES20.VertexAttribParameter) pname, (Single*) @params);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Return a generic vertex attribute parameter
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the generic vertex attribute parameter to be queried.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the symbolic name of the vertex attribute parameter to be queried. Accepted values are GL_VERTEX_ATTRIB_ARRAY_BUFFER_BINDING, GL_VERTEX_ATTRIB_ARRAY_ENABLED, GL_VERTEX_ATTRIB_ARRAY_SIZE, GL_VERTEX_ATTRIB_ARRAY_STRIDE, GL_VERTEX_ATTRIB_ARRAY_TYPE, GL_VERTEX_ATTRIB_ARRAY_NORMALIZED, GL_VERTEX_ATTRIB_ARRAY_INTEGER, GL_VERTEX_ATTRIB_ARRAY_DIVISOR, or GL_CURRENT_VERTEX_ATTRIB.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Returns the requested data.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetVertexAttribfv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void GetVertexAttrib(UInt32 index, System.Graphics.ES20.VertexAttribParameter pname, [OutAttribute] Single[] @params) {
			unsafe
			{
				fixed (Single* @params_ptr = @params) {
					Delegates.glGetVertexAttribfv((UInt32) index, (System.Graphics.ES20.VertexAttribParameter) pname, (Single*) @params_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Return a generic vertex attribute parameter
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the generic vertex attribute parameter to be queried.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the symbolic name of the vertex attribute parameter to be queried. Accepted values are GL_VERTEX_ATTRIB_ARRAY_BUFFER_BINDING, GL_VERTEX_ATTRIB_ARRAY_ENABLED, GL_VERTEX_ATTRIB_ARRAY_SIZE, GL_VERTEX_ATTRIB_ARRAY_STRIDE, GL_VERTEX_ATTRIB_ARRAY_TYPE, GL_VERTEX_ATTRIB_ARRAY_NORMALIZED, GL_VERTEX_ATTRIB_ARRAY_INTEGER, GL_VERTEX_ATTRIB_ARRAY_DIVISOR, or GL_CURRENT_VERTEX_ATTRIB.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Returns the requested data.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetVertexAttribfv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void GetVertexAttrib(UInt32 index, System.Graphics.ES20.VertexAttribParameter pname, [OutAttribute] out Single @params) {
			unsafe
			{
				fixed (Single* @params_ptr = &@params) {
					Delegates.glGetVertexAttribfv((UInt32) index, (System.Graphics.ES20.VertexAttribParameter) pname, (Single*) @params_ptr);
					@params = *@params_ptr;
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Return a generic vertex attribute parameter
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the generic vertex attribute parameter to be queried.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the symbolic name of the vertex attribute parameter to be queried. Accepted values are GL_VERTEX_ATTRIB_ARRAY_BUFFER_BINDING, GL_VERTEX_ATTRIB_ARRAY_ENABLED, GL_VERTEX_ATTRIB_ARRAY_SIZE, GL_VERTEX_ATTRIB_ARRAY_STRIDE, GL_VERTEX_ATTRIB_ARRAY_TYPE, GL_VERTEX_ATTRIB_ARRAY_NORMALIZED, GL_VERTEX_ATTRIB_ARRAY_INTEGER, GL_VERTEX_ATTRIB_ARRAY_DIVISOR, or GL_CURRENT_VERTEX_ATTRIB.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Returns the requested data.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetVertexAttribfv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void GetVertexAttrib(UInt32 index, System.Graphics.ES20.VertexAttribParameter pname, [OutAttribute] Single* @params) {
			Delegates.glGetVertexAttribfv((UInt32) index, (System.Graphics.ES20.VertexAttribParameter) pname, (Single*) @params);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Return a generic vertex attribute parameter
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the generic vertex attribute parameter to be queried.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the symbolic name of the vertex attribute parameter to be queried. Accepted values are GL_VERTEX_ATTRIB_ARRAY_BUFFER_BINDING, GL_VERTEX_ATTRIB_ARRAY_ENABLED, GL_VERTEX_ATTRIB_ARRAY_SIZE, GL_VERTEX_ATTRIB_ARRAY_STRIDE, GL_VERTEX_ATTRIB_ARRAY_TYPE, GL_VERTEX_ATTRIB_ARRAY_NORMALIZED, GL_VERTEX_ATTRIB_ARRAY_INTEGER, GL_VERTEX_ATTRIB_ARRAY_DIVISOR, or GL_CURRENT_VERTEX_ATTRIB.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Returns the requested data.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetVertexAttribiv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetVertexAttrib(Int32 index, System.Graphics.ES20.VertexAttribParameter pname, [OutAttribute] Int32[] @params) {
			unsafe
			{
				fixed (Int32* @params_ptr = @params) {
					Delegates.glGetVertexAttribiv((UInt32) index, (System.Graphics.ES20.VertexAttribParameter) pname, (Int32*) @params_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Return a generic vertex attribute parameter
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the generic vertex attribute parameter to be queried.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the symbolic name of the vertex attribute parameter to be queried. Accepted values are GL_VERTEX_ATTRIB_ARRAY_BUFFER_BINDING, GL_VERTEX_ATTRIB_ARRAY_ENABLED, GL_VERTEX_ATTRIB_ARRAY_SIZE, GL_VERTEX_ATTRIB_ARRAY_STRIDE, GL_VERTEX_ATTRIB_ARRAY_TYPE, GL_VERTEX_ATTRIB_ARRAY_NORMALIZED, GL_VERTEX_ATTRIB_ARRAY_INTEGER, GL_VERTEX_ATTRIB_ARRAY_DIVISOR, or GL_CURRENT_VERTEX_ATTRIB.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Returns the requested data.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetVertexAttribiv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetVertexAttrib(Int32 index, System.Graphics.ES20.VertexAttribParameter pname, [OutAttribute] out Int32 @params) {
			unsafe
			{
				fixed (Int32* @params_ptr = &@params) {
					Delegates.glGetVertexAttribiv((UInt32) index, (System.Graphics.ES20.VertexAttribParameter) pname, (Int32*) @params_ptr);
					@params = *@params_ptr;
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Return a generic vertex attribute parameter
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the generic vertex attribute parameter to be queried.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the symbolic name of the vertex attribute parameter to be queried. Accepted values are GL_VERTEX_ATTRIB_ARRAY_BUFFER_BINDING, GL_VERTEX_ATTRIB_ARRAY_ENABLED, GL_VERTEX_ATTRIB_ARRAY_SIZE, GL_VERTEX_ATTRIB_ARRAY_STRIDE, GL_VERTEX_ATTRIB_ARRAY_TYPE, GL_VERTEX_ATTRIB_ARRAY_NORMALIZED, GL_VERTEX_ATTRIB_ARRAY_INTEGER, GL_VERTEX_ATTRIB_ARRAY_DIVISOR, or GL_CURRENT_VERTEX_ATTRIB.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Returns the requested data.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetVertexAttribiv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void GetVertexAttrib(Int32 index, System.Graphics.ES20.VertexAttribParameter pname, [OutAttribute] Int32* @params) {
			Delegates.glGetVertexAttribiv((UInt32) index, (System.Graphics.ES20.VertexAttribParameter) pname, (Int32*) @params);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Return a generic vertex attribute parameter
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the generic vertex attribute parameter to be queried.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the symbolic name of the vertex attribute parameter to be queried. Accepted values are GL_VERTEX_ATTRIB_ARRAY_BUFFER_BINDING, GL_VERTEX_ATTRIB_ARRAY_ENABLED, GL_VERTEX_ATTRIB_ARRAY_SIZE, GL_VERTEX_ATTRIB_ARRAY_STRIDE, GL_VERTEX_ATTRIB_ARRAY_TYPE, GL_VERTEX_ATTRIB_ARRAY_NORMALIZED, GL_VERTEX_ATTRIB_ARRAY_INTEGER, GL_VERTEX_ATTRIB_ARRAY_DIVISOR, or GL_CURRENT_VERTEX_ATTRIB.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Returns the requested data.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetVertexAttribiv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void GetVertexAttrib(UInt32 index, System.Graphics.ES20.VertexAttribParameter pname, [OutAttribute] Int32[] @params) {
			unsafe
			{
				fixed (Int32* @params_ptr = @params) {
					Delegates.glGetVertexAttribiv((UInt32) index, (System.Graphics.ES20.VertexAttribParameter) pname, (Int32*) @params_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Return a generic vertex attribute parameter
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the generic vertex attribute parameter to be queried.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the symbolic name of the vertex attribute parameter to be queried. Accepted values are GL_VERTEX_ATTRIB_ARRAY_BUFFER_BINDING, GL_VERTEX_ATTRIB_ARRAY_ENABLED, GL_VERTEX_ATTRIB_ARRAY_SIZE, GL_VERTEX_ATTRIB_ARRAY_STRIDE, GL_VERTEX_ATTRIB_ARRAY_TYPE, GL_VERTEX_ATTRIB_ARRAY_NORMALIZED, GL_VERTEX_ATTRIB_ARRAY_INTEGER, GL_VERTEX_ATTRIB_ARRAY_DIVISOR, or GL_CURRENT_VERTEX_ATTRIB.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Returns the requested data.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetVertexAttribiv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void GetVertexAttrib(UInt32 index, System.Graphics.ES20.VertexAttribParameter pname, [OutAttribute] out Int32 @params) {
			unsafe
			{
				fixed (Int32* @params_ptr = &@params) {
					Delegates.glGetVertexAttribiv((UInt32) index, (System.Graphics.ES20.VertexAttribParameter) pname, (Int32*) @params_ptr);
					@params = *@params_ptr;
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Return a generic vertex attribute parameter
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the generic vertex attribute parameter to be queried.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the symbolic name of the vertex attribute parameter to be queried. Accepted values are GL_VERTEX_ATTRIB_ARRAY_BUFFER_BINDING, GL_VERTEX_ATTRIB_ARRAY_ENABLED, GL_VERTEX_ATTRIB_ARRAY_SIZE, GL_VERTEX_ATTRIB_ARRAY_STRIDE, GL_VERTEX_ATTRIB_ARRAY_TYPE, GL_VERTEX_ATTRIB_ARRAY_NORMALIZED, GL_VERTEX_ATTRIB_ARRAY_INTEGER, GL_VERTEX_ATTRIB_ARRAY_DIVISOR, or GL_CURRENT_VERTEX_ATTRIB.
		/// </para>
		/// </param>
		/// <param name="params">
		/// <para>
		/// Returns the requested data.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetVertexAttribiv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void GetVertexAttrib(UInt32 index, System.Graphics.ES20.VertexAttribParameter pname, [OutAttribute] Int32* @params) {
			Delegates.glGetVertexAttribiv((UInt32) index, (System.Graphics.ES20.VertexAttribParameter) pname, (Int32*) @params);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Return the address of the specified generic vertex attribute pointer
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the generic vertex attribute parameter to be returned.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the symbolic name of the generic vertex attribute parameter to be returned. Must be GL_VERTEX_ATTRIB_ARRAY_POINTER.
		/// </para>
		/// </param>
		/// <param name="pointer">
		/// <para>
		/// Returns the pointer value.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetVertexAttribPointerv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetVertexAttribPointer(Int32 index, System.Graphics.ES20.VertexAttribPointerParameter pname, [OutAttribute] IntPtr pointer) {
			Delegates.glGetVertexAttribPointerv((UInt32) index, (System.Graphics.ES20.VertexAttribPointerParameter) pname, (IntPtr) pointer);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Return the address of the specified generic vertex attribute pointer
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the generic vertex attribute parameter to be returned.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the symbolic name of the generic vertex attribute parameter to be returned. Must be GL_VERTEX_ATTRIB_ARRAY_POINTER.
		/// </para>
		/// </param>
		/// <param name="pointer">
		/// <para>
		/// Returns the pointer value.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetVertexAttribPointerv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetVertexAttribPointer<T2>(Int32 index, System.Graphics.ES20.VertexAttribPointerParameter pname, [InAttribute, OutAttribute] T2[] pointer)
					where T2 : struct {
			GCHandle pointer_ptr = GCHandle.Alloc(pointer, GCHandleType.Pinned);
			try {
				Delegates.glGetVertexAttribPointerv((UInt32) index, (System.Graphics.ES20.VertexAttribPointerParameter) pname, (IntPtr) pointer_ptr.AddrOfPinnedObject());
			} finally {
				pointer_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Return the address of the specified generic vertex attribute pointer
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the generic vertex attribute parameter to be returned.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the symbolic name of the generic vertex attribute parameter to be returned. Must be GL_VERTEX_ATTRIB_ARRAY_POINTER.
		/// </para>
		/// </param>
		/// <param name="pointer">
		/// <para>
		/// Returns the pointer value.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetVertexAttribPointerv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetVertexAttribPointer<T2>(Int32 index, System.Graphics.ES20.VertexAttribPointerParameter pname, [InAttribute, OutAttribute] T2[,] pointer)
					where T2 : struct {
			GCHandle pointer_ptr = GCHandle.Alloc(pointer, GCHandleType.Pinned);
			try {
				Delegates.glGetVertexAttribPointerv((UInt32) index, (System.Graphics.ES20.VertexAttribPointerParameter) pname, (IntPtr) pointer_ptr.AddrOfPinnedObject());
			} finally {
				pointer_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Return the address of the specified generic vertex attribute pointer
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the generic vertex attribute parameter to be returned.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the symbolic name of the generic vertex attribute parameter to be returned. Must be GL_VERTEX_ATTRIB_ARRAY_POINTER.
		/// </para>
		/// </param>
		/// <param name="pointer">
		/// <para>
		/// Returns the pointer value.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetVertexAttribPointerv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetVertexAttribPointer<T2>(Int32 index, System.Graphics.ES20.VertexAttribPointerParameter pname, [InAttribute, OutAttribute] T2[,,] pointer)
					where T2 : struct {
			GCHandle pointer_ptr = GCHandle.Alloc(pointer, GCHandleType.Pinned);
			try {
				Delegates.glGetVertexAttribPointerv((UInt32) index, (System.Graphics.ES20.VertexAttribPointerParameter) pname, (IntPtr) pointer_ptr.AddrOfPinnedObject());
			} finally {
				pointer_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Return the address of the specified generic vertex attribute pointer
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the generic vertex attribute parameter to be returned.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the symbolic name of the generic vertex attribute parameter to be returned. Must be GL_VERTEX_ATTRIB_ARRAY_POINTER.
		/// </para>
		/// </param>
		/// <param name="pointer">
		/// <para>
		/// Returns the pointer value.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetVertexAttribPointerv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void GetVertexAttribPointer<T2>(Int32 index, System.Graphics.ES20.VertexAttribPointerParameter pname, [InAttribute, OutAttribute] ref T2 pointer)
					where T2 : struct {
			GCHandle pointer_ptr = GCHandle.Alloc(pointer, GCHandleType.Pinned);
			try {
				Delegates.glGetVertexAttribPointerv((UInt32) index, (System.Graphics.ES20.VertexAttribPointerParameter) pname, (IntPtr) pointer_ptr.AddrOfPinnedObject());
				pointer = (T2) pointer_ptr.Target;
			} finally {
				pointer_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Return the address of the specified generic vertex attribute pointer
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the generic vertex attribute parameter to be returned.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the symbolic name of the generic vertex attribute parameter to be returned. Must be GL_VERTEX_ATTRIB_ARRAY_POINTER.
		/// </para>
		/// </param>
		/// <param name="pointer">
		/// <para>
		/// Returns the pointer value.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetVertexAttribPointerv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void GetVertexAttribPointer(UInt32 index, System.Graphics.ES20.VertexAttribPointerParameter pname, [OutAttribute] IntPtr pointer) {
			Delegates.glGetVertexAttribPointerv((UInt32) index, (System.Graphics.ES20.VertexAttribPointerParameter) pname, (IntPtr) pointer);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Return the address of the specified generic vertex attribute pointer
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the generic vertex attribute parameter to be returned.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the symbolic name of the generic vertex attribute parameter to be returned. Must be GL_VERTEX_ATTRIB_ARRAY_POINTER.
		/// </para>
		/// </param>
		/// <param name="pointer">
		/// <para>
		/// Returns the pointer value.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetVertexAttribPointerv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void GetVertexAttribPointer<T2>(UInt32 index, System.Graphics.ES20.VertexAttribPointerParameter pname, [InAttribute, OutAttribute] T2[] pointer)
			where T2 : struct {
			GCHandle pointer_ptr = GCHandle.Alloc(pointer, GCHandleType.Pinned);
			try {
				Delegates.glGetVertexAttribPointerv((UInt32) index, (System.Graphics.ES20.VertexAttribPointerParameter) pname, (IntPtr) pointer_ptr.AddrOfPinnedObject());
			} finally {
				pointer_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Return the address of the specified generic vertex attribute pointer
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the generic vertex attribute parameter to be returned.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the symbolic name of the generic vertex attribute parameter to be returned. Must be GL_VERTEX_ATTRIB_ARRAY_POINTER.
		/// </para>
		/// </param>
		/// <param name="pointer">
		/// <para>
		/// Returns the pointer value.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetVertexAttribPointerv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void GetVertexAttribPointer<T2>(UInt32 index, System.Graphics.ES20.VertexAttribPointerParameter pname, [InAttribute, OutAttribute] T2[,] pointer)
			where T2 : struct {
			GCHandle pointer_ptr = GCHandle.Alloc(pointer, GCHandleType.Pinned);
			try {
				Delegates.glGetVertexAttribPointerv((UInt32) index, (System.Graphics.ES20.VertexAttribPointerParameter) pname, (IntPtr) pointer_ptr.AddrOfPinnedObject());
			} finally {
				pointer_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Return the address of the specified generic vertex attribute pointer
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the generic vertex attribute parameter to be returned.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the symbolic name of the generic vertex attribute parameter to be returned. Must be GL_VERTEX_ATTRIB_ARRAY_POINTER.
		/// </para>
		/// </param>
		/// <param name="pointer">
		/// <para>
		/// Returns the pointer value.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetVertexAttribPointerv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void GetVertexAttribPointer<T2>(UInt32 index, System.Graphics.ES20.VertexAttribPointerParameter pname, [InAttribute, OutAttribute] T2[,,] pointer)
			where T2 : struct {
			GCHandle pointer_ptr = GCHandle.Alloc(pointer, GCHandleType.Pinned);
			try {
				Delegates.glGetVertexAttribPointerv((UInt32) index, (System.Graphics.ES20.VertexAttribPointerParameter) pname, (IntPtr) pointer_ptr.AddrOfPinnedObject());
			} finally {
				pointer_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Return the address of the specified generic vertex attribute pointer
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the generic vertex attribute parameter to be returned.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the symbolic name of the generic vertex attribute parameter to be returned. Must be GL_VERTEX_ATTRIB_ARRAY_POINTER.
		/// </para>
		/// </param>
		/// <param name="pointer">
		/// <para>
		/// Returns the pointer value.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetVertexAttribPointerv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void GetVertexAttribPointer<T2>(UInt32 index, System.Graphics.ES20.VertexAttribPointerParameter pname, [InAttribute, OutAttribute] ref T2 pointer)
			where T2 : struct {
			GCHandle pointer_ptr = GCHandle.Alloc(pointer, GCHandleType.Pinned);
			try {
				Delegates.glGetVertexAttribPointerv((UInt32) index, (System.Graphics.ES20.VertexAttribPointerParameter) pname, (IntPtr) pointer_ptr.AddrOfPinnedObject());
				pointer = (T2) pointer_ptr.Target;
			} finally {
				pointer_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify implementation-specific hints
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies a symbolic constant indicating the behavior to be controlled. GL_LINE_SMOOTH_HINT, GL_POLYGON_SMOOTH_HINT, GL_TEXTURE_COMPRESSION_HINT, and GL_FRAGMENT_SHADER_DERIVATIVE_HINT are accepted.
		/// </para>
		/// </param>
		/// <param name="mode">
		/// <para>
		/// Specifies a symbolic constant indicating the desired behavior. GL_FASTEST, GL_NICEST, and GL_DONT_CARE are accepted.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glHint")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void Hint(System.Graphics.ES20.HintTarget target, System.Graphics.ES20.HintMode mode) {
			Delegates.glHint((System.Graphics.ES20.HintTarget) target, (System.Graphics.ES20.HintMode) mode);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Determine if a name corresponds to a buffer object
		/// </summary>
		/// <param name="buffer">
		/// <para>
		/// Specifies a value that may be the name of a buffer object.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glIsBuffer")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				bool IsBuffer(Int32 buffer) {
			return Delegates.glIsBuffer((UInt32) buffer);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Determine if a name corresponds to a buffer object
		/// </summary>
		/// <param name="buffer">
		/// <para>
		/// Specifies a value that may be the name of a buffer object.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glIsBuffer")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		bool IsBuffer(UInt32 buffer) {
			return Delegates.glIsBuffer((UInt32) buffer);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Test whether a capability is enabled
		/// </summary>
		/// <param name="cap">
		/// <para>
		/// Specifies a symbolic constant indicating a GL capability.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glIsEnabled")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				bool IsEnabled(System.Graphics.ES20.EnableCap cap) {
			return Delegates.glIsEnabled((System.Graphics.ES20.EnableCap) cap);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Determine if a name corresponds to a framebuffer object
		/// </summary>
		/// <param name="framebuffer">
		/// <para>
		/// Specifies a value that may be the name of a framebuffer object.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glIsFramebuffer")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				bool IsFramebuffer(Int32 framebuffer) {
			return Delegates.glIsFramebuffer((UInt32) framebuffer);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Determine if a name corresponds to a framebuffer object
		/// </summary>
		/// <param name="framebuffer">
		/// <para>
		/// Specifies a value that may be the name of a framebuffer object.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glIsFramebuffer")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		bool IsFramebuffer(UInt32 framebuffer) {
			return Delegates.glIsFramebuffer((UInt32) framebuffer);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Determines if a name corresponds to a program object
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies a potential program object.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glIsProgram")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				bool IsProgram(Int32 program) {
			return Delegates.glIsProgram((UInt32) program);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Determines if a name corresponds to a program object
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies a potential program object.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glIsProgram")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		bool IsProgram(UInt32 program) {
			return Delegates.glIsProgram((UInt32) program);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Determine if a name corresponds to a renderbuffer object
		/// </summary>
		/// <param name="renderbuffer">
		/// <para>
		/// Specifies a value that may be the name of a renderbuffer object.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glIsRenderbuffer")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				bool IsRenderbuffer(Int32 renderbuffer) {
			return Delegates.glIsRenderbuffer((UInt32) renderbuffer);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Determine if a name corresponds to a renderbuffer object
		/// </summary>
		/// <param name="renderbuffer">
		/// <para>
		/// Specifies a value that may be the name of a renderbuffer object.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glIsRenderbuffer")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		bool IsRenderbuffer(UInt32 renderbuffer) {
			return Delegates.glIsRenderbuffer((UInt32) renderbuffer);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Determines if a name corresponds to a shader object
		/// </summary>
		/// <param name="shader">
		/// <para>
		/// Specifies a potential shader object.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glIsShader")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				bool IsShader(Int32 shader) {
			return Delegates.glIsShader((UInt32) shader);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Determines if a name corresponds to a shader object
		/// </summary>
		/// <param name="shader">
		/// <para>
		/// Specifies a potential shader object.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glIsShader")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		bool IsShader(UInt32 shader) {
			return Delegates.glIsShader((UInt32) shader);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Determine if a name corresponds to a texture
		/// </summary>
		/// <param name="texture">
		/// <para>
		/// Specifies a value that may be the name of a texture.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glIsTexture")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				bool IsTexture(Int32 texture) {
			return Delegates.glIsTexture((UInt32) texture);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Determine if a name corresponds to a texture
		/// </summary>
		/// <param name="texture">
		/// <para>
		/// Specifies a value that may be the name of a texture.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glIsTexture")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		bool IsTexture(UInt32 texture) {
			return Delegates.glIsTexture((UInt32) texture);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify the width of rasterized lines
		/// </summary>
		/// <param name="width">
		/// <para>
		/// Specifies the width of rasterized lines. The initial value is 1.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glLineWidth")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void LineWidth(Single width) {
			Delegates.glLineWidth((Single) width);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Links a program object
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the handle of the program object to be linked.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glLinkProgram")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void LinkProgram(Int32 program) {
			Delegates.glLinkProgram((UInt32) program);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Links a program object
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the handle of the program object to be linked.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glLinkProgram")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void LinkProgram(UInt32 program) {
			Delegates.glLinkProgram((UInt32) program);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Set pixel storage modes
		/// </summary>
		/// <param name="pname">
		/// <para>
		/// Specifies the symbolic name of the parameter to be set. Six values affect the packing of pixel data into memory: GL_PACK_SWAP_BYTES, GL_PACK_LSB_FIRST, GL_PACK_ROW_LENGTH, GL_PACK_IMAGE_HEIGHT, GL_PACK_SKIP_PIXELS, GL_PACK_SKIP_ROWS, GL_PACK_SKIP_IMAGES, and GL_PACK_ALIGNMENT. Six more affect the unpacking of pixel data from memory: GL_UNPACK_SWAP_BYTES, GL_UNPACK_LSB_FIRST, GL_UNPACK_ROW_LENGTH, GL_UNPACK_IMAGE_HEIGHT, GL_UNPACK_SKIP_PIXELS, GL_UNPACK_SKIP_ROWS, GL_UNPACK_SKIP_IMAGES, and GL_UNPACK_ALIGNMENT.
		/// </para>
		/// </param>
		/// <param name="param">
		/// <para>
		/// Specifies the value that pname is set to.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glPixelStorei")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void PixelStore(System.Graphics.ES20.PixelStoreParameter pname, Int32 param) {
			Delegates.glPixelStorei((System.Graphics.ES20.PixelStoreParameter) pname, (Int32) param);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Set the scale and units used to calculate depth values
		/// </summary>
		/// <param name="factor">
		/// <para>
		/// Specifies a scale factor that is used to create a variable depth offset for each polygon. The initial value is 0.
		/// </para>
		/// </param>
		/// <param name="units">
		/// <para>
		/// Is multiplied by an implementation-specific value to create a constant depth offset. The initial value is 0.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glPolygonOffset")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void PolygonOffset(Single factor, Single units) {
			Delegates.glPolygonOffset((Single) factor, (Single) units);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Read a block of pixels from the frame buffer
		/// </summary>
		/// <param name="x">
		/// <para>
		/// Specify the window coordinates of the first pixel that is read from the frame buffer. This location is the lower left corner of a rectangular block of pixels.
		/// </para>
		/// </param>
		/// <param name="width">
		/// <para>
		/// Specify the dimensions of the pixel rectangle. width and height of one correspond to a single pixel.
		/// </para>
		/// </param>
		/// <param name="format">
		/// <para>
		/// Specifies the format of the pixel data. The following symbolic values are accepted: GL_STENCIL_INDEX, GL_DEPTH_COMPONENT, GL_DEPTH_STENCIL, GL_RED, GL_GREEN, GL_BLUE, GL_RGB, GL_BGR, GL_RGBA, and GL_BGRA.
		/// </para>
		/// </param>
		/// <param name="type">
		/// <para>
		/// Specifies the data type of the pixel data. Must be one of GL_UNSIGNED_BYTE, GL_BYTE, GL_UNSIGNED_SHORT, GL_SHORT, GL_UNSIGNED_INT, GL_INT, GL_HALF_FLOAT, GL_FLOAT, GL_UNSIGNED_BYTE_3_3_2, GL_UNSIGNED_BYTE_2_3_3_REV, GL_UNSIGNED_SHORT_5_6_5, GL_UNSIGNED_SHORT_5_6_5_REV, GL_UNSIGNED_SHORT_4_4_4_4, GL_UNSIGNED_SHORT_4_4_4_4_REV, GL_UNSIGNED_SHORT_5_5_5_1, GL_UNSIGNED_SHORT_1_5_5_5_REV, GL_UNSIGNED_INT_8_8_8_8, GL_UNSIGNED_INT_8_8_8_8_REV, GL_UNSIGNED_INT_10_10_10_2, GL_UNSIGNED_INT_2_10_10_10_REV, GL_UNSIGNED_INT_24_8, GL_UNSIGNED_INT_10F_11F_11F_REV, GL_UNSIGNED_INT_5_9_9_9_REV, or GL_FLOAT_32_UNSIGNED_INT_24_8_REV.
		/// </para>
		/// </param>
		/// <param name="data">
		/// <para>
		/// Returns the pixel data.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glReadPixels")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void ReadPixels(Int32 x, Int32 y, Int32 width, Int32 height, System.Graphics.ES20.PixelFormat format, System.Graphics.ES20.PixelType type, IntPtr pixels) {
			Delegates.glReadPixels((Int32) x, (Int32) y, (Int32) width, (Int32) height, (System.Graphics.ES20.PixelFormat) format, (System.Graphics.ES20.PixelType) type, (IntPtr) pixels);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Read a block of pixels from the frame buffer
		/// </summary>
		/// <param name="x">
		/// <para>
		/// Specify the window coordinates of the first pixel that is read from the frame buffer. This location is the lower left corner of a rectangular block of pixels.
		/// </para>
		/// </param>
		/// <param name="width">
		/// <para>
		/// Specify the dimensions of the pixel rectangle. width and height of one correspond to a single pixel.
		/// </para>
		/// </param>
		/// <param name="format">
		/// <para>
		/// Specifies the format of the pixel data. The following symbolic values are accepted: GL_STENCIL_INDEX, GL_DEPTH_COMPONENT, GL_DEPTH_STENCIL, GL_RED, GL_GREEN, GL_BLUE, GL_RGB, GL_BGR, GL_RGBA, and GL_BGRA.
		/// </para>
		/// </param>
		/// <param name="type">
		/// <para>
		/// Specifies the data type of the pixel data. Must be one of GL_UNSIGNED_BYTE, GL_BYTE, GL_UNSIGNED_SHORT, GL_SHORT, GL_UNSIGNED_INT, GL_INT, GL_HALF_FLOAT, GL_FLOAT, GL_UNSIGNED_BYTE_3_3_2, GL_UNSIGNED_BYTE_2_3_3_REV, GL_UNSIGNED_SHORT_5_6_5, GL_UNSIGNED_SHORT_5_6_5_REV, GL_UNSIGNED_SHORT_4_4_4_4, GL_UNSIGNED_SHORT_4_4_4_4_REV, GL_UNSIGNED_SHORT_5_5_5_1, GL_UNSIGNED_SHORT_1_5_5_5_REV, GL_UNSIGNED_INT_8_8_8_8, GL_UNSIGNED_INT_8_8_8_8_REV, GL_UNSIGNED_INT_10_10_10_2, GL_UNSIGNED_INT_2_10_10_10_REV, GL_UNSIGNED_INT_24_8, GL_UNSIGNED_INT_10F_11F_11F_REV, GL_UNSIGNED_INT_5_9_9_9_REV, or GL_FLOAT_32_UNSIGNED_INT_24_8_REV.
		/// </para>
		/// </param>
		/// <param name="data">
		/// <para>
		/// Returns the pixel data.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glReadPixels")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void ReadPixels<T6>(Int32 x, Int32 y, Int32 width, Int32 height, System.Graphics.ES20.PixelFormat format, System.Graphics.ES20.PixelType type, [InAttribute, OutAttribute] T6[] pixels)
					where T6 : struct {
			GCHandle pixels_ptr = GCHandle.Alloc(pixels, GCHandleType.Pinned);
			try {
				Delegates.glReadPixels((Int32) x, (Int32) y, (Int32) width, (Int32) height, (System.Graphics.ES20.PixelFormat) format, (System.Graphics.ES20.PixelType) type, (IntPtr) pixels_ptr.AddrOfPinnedObject());
			} finally {
				pixels_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Read a block of pixels from the frame buffer
		/// </summary>
		/// <param name="x">
		/// <para>
		/// Specify the window coordinates of the first pixel that is read from the frame buffer. This location is the lower left corner of a rectangular block of pixels.
		/// </para>
		/// </param>
		/// <param name="width">
		/// <para>
		/// Specify the dimensions of the pixel rectangle. width and height of one correspond to a single pixel.
		/// </para>
		/// </param>
		/// <param name="format">
		/// <para>
		/// Specifies the format of the pixel data. The following symbolic values are accepted: GL_STENCIL_INDEX, GL_DEPTH_COMPONENT, GL_DEPTH_STENCIL, GL_RED, GL_GREEN, GL_BLUE, GL_RGB, GL_BGR, GL_RGBA, and GL_BGRA.
		/// </para>
		/// </param>
		/// <param name="type">
		/// <para>
		/// Specifies the data type of the pixel data. Must be one of GL_UNSIGNED_BYTE, GL_BYTE, GL_UNSIGNED_SHORT, GL_SHORT, GL_UNSIGNED_INT, GL_INT, GL_HALF_FLOAT, GL_FLOAT, GL_UNSIGNED_BYTE_3_3_2, GL_UNSIGNED_BYTE_2_3_3_REV, GL_UNSIGNED_SHORT_5_6_5, GL_UNSIGNED_SHORT_5_6_5_REV, GL_UNSIGNED_SHORT_4_4_4_4, GL_UNSIGNED_SHORT_4_4_4_4_REV, GL_UNSIGNED_SHORT_5_5_5_1, GL_UNSIGNED_SHORT_1_5_5_5_REV, GL_UNSIGNED_INT_8_8_8_8, GL_UNSIGNED_INT_8_8_8_8_REV, GL_UNSIGNED_INT_10_10_10_2, GL_UNSIGNED_INT_2_10_10_10_REV, GL_UNSIGNED_INT_24_8, GL_UNSIGNED_INT_10F_11F_11F_REV, GL_UNSIGNED_INT_5_9_9_9_REV, or GL_FLOAT_32_UNSIGNED_INT_24_8_REV.
		/// </para>
		/// </param>
		/// <param name="data">
		/// <para>
		/// Returns the pixel data.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glReadPixels")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void ReadPixels<T6>(Int32 x, Int32 y, Int32 width, Int32 height, System.Graphics.ES20.PixelFormat format, System.Graphics.ES20.PixelType type, [InAttribute, OutAttribute] T6[,] pixels)
					where T6 : struct {
			GCHandle pixels_ptr = GCHandle.Alloc(pixels, GCHandleType.Pinned);
			try {
				Delegates.glReadPixels((Int32) x, (Int32) y, (Int32) width, (Int32) height, (System.Graphics.ES20.PixelFormat) format, (System.Graphics.ES20.PixelType) type, (IntPtr) pixels_ptr.AddrOfPinnedObject());
			} finally {
				pixels_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Read a block of pixels from the frame buffer
		/// </summary>
		/// <param name="x">
		/// <para>
		/// Specify the window coordinates of the first pixel that is read from the frame buffer. This location is the lower left corner of a rectangular block of pixels.
		/// </para>
		/// </param>
		/// <param name="width">
		/// <para>
		/// Specify the dimensions of the pixel rectangle. width and height of one correspond to a single pixel.
		/// </para>
		/// </param>
		/// <param name="format">
		/// <para>
		/// Specifies the format of the pixel data. The following symbolic values are accepted: GL_STENCIL_INDEX, GL_DEPTH_COMPONENT, GL_DEPTH_STENCIL, GL_RED, GL_GREEN, GL_BLUE, GL_RGB, GL_BGR, GL_RGBA, and GL_BGRA.
		/// </para>
		/// </param>
		/// <param name="type">
		/// <para>
		/// Specifies the data type of the pixel data. Must be one of GL_UNSIGNED_BYTE, GL_BYTE, GL_UNSIGNED_SHORT, GL_SHORT, GL_UNSIGNED_INT, GL_INT, GL_HALF_FLOAT, GL_FLOAT, GL_UNSIGNED_BYTE_3_3_2, GL_UNSIGNED_BYTE_2_3_3_REV, GL_UNSIGNED_SHORT_5_6_5, GL_UNSIGNED_SHORT_5_6_5_REV, GL_UNSIGNED_SHORT_4_4_4_4, GL_UNSIGNED_SHORT_4_4_4_4_REV, GL_UNSIGNED_SHORT_5_5_5_1, GL_UNSIGNED_SHORT_1_5_5_5_REV, GL_UNSIGNED_INT_8_8_8_8, GL_UNSIGNED_INT_8_8_8_8_REV, GL_UNSIGNED_INT_10_10_10_2, GL_UNSIGNED_INT_2_10_10_10_REV, GL_UNSIGNED_INT_24_8, GL_UNSIGNED_INT_10F_11F_11F_REV, GL_UNSIGNED_INT_5_9_9_9_REV, or GL_FLOAT_32_UNSIGNED_INT_24_8_REV.
		/// </para>
		/// </param>
		/// <param name="data">
		/// <para>
		/// Returns the pixel data.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glReadPixels")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void ReadPixels<T6>(Int32 x, Int32 y, Int32 width, Int32 height, System.Graphics.ES20.PixelFormat format, System.Graphics.ES20.PixelType type, [InAttribute, OutAttribute] T6[,,] pixels)
					where T6 : struct {
			GCHandle pixels_ptr = GCHandle.Alloc(pixels, GCHandleType.Pinned);
			try {
				Delegates.glReadPixels((Int32) x, (Int32) y, (Int32) width, (Int32) height, (System.Graphics.ES20.PixelFormat) format, (System.Graphics.ES20.PixelType) type, (IntPtr) pixels_ptr.AddrOfPinnedObject());
			} finally {
				pixels_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Read a block of pixels from the frame buffer
		/// </summary>
		/// <param name="x">
		/// <para>
		/// Specify the window coordinates of the first pixel that is read from the frame buffer. This location is the lower left corner of a rectangular block of pixels.
		/// </para>
		/// </param>
		/// <param name="width">
		/// <para>
		/// Specify the dimensions of the pixel rectangle. width and height of one correspond to a single pixel.
		/// </para>
		/// </param>
		/// <param name="format">
		/// <para>
		/// Specifies the format of the pixel data. The following symbolic values are accepted: GL_STENCIL_INDEX, GL_DEPTH_COMPONENT, GL_DEPTH_STENCIL, GL_RED, GL_GREEN, GL_BLUE, GL_RGB, GL_BGR, GL_RGBA, and GL_BGRA.
		/// </para>
		/// </param>
		/// <param name="type">
		/// <para>
		/// Specifies the data type of the pixel data. Must be one of GL_UNSIGNED_BYTE, GL_BYTE, GL_UNSIGNED_SHORT, GL_SHORT, GL_UNSIGNED_INT, GL_INT, GL_HALF_FLOAT, GL_FLOAT, GL_UNSIGNED_BYTE_3_3_2, GL_UNSIGNED_BYTE_2_3_3_REV, GL_UNSIGNED_SHORT_5_6_5, GL_UNSIGNED_SHORT_5_6_5_REV, GL_UNSIGNED_SHORT_4_4_4_4, GL_UNSIGNED_SHORT_4_4_4_4_REV, GL_UNSIGNED_SHORT_5_5_5_1, GL_UNSIGNED_SHORT_1_5_5_5_REV, GL_UNSIGNED_INT_8_8_8_8, GL_UNSIGNED_INT_8_8_8_8_REV, GL_UNSIGNED_INT_10_10_10_2, GL_UNSIGNED_INT_2_10_10_10_REV, GL_UNSIGNED_INT_24_8, GL_UNSIGNED_INT_10F_11F_11F_REV, GL_UNSIGNED_INT_5_9_9_9_REV, or GL_FLOAT_32_UNSIGNED_INT_24_8_REV.
		/// </para>
		/// </param>
		/// <param name="data">
		/// <para>
		/// Returns the pixel data.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glReadPixels")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void ReadPixels<T6>(Int32 x, Int32 y, Int32 width, Int32 height, System.Graphics.ES20.PixelFormat format, System.Graphics.ES20.PixelType type, [InAttribute, OutAttribute] ref T6 pixels)
					where T6 : struct {
			GCHandle pixels_ptr = GCHandle.Alloc(pixels, GCHandleType.Pinned);
			try {
				Delegates.glReadPixels((Int32) x, (Int32) y, (Int32) width, (Int32) height, (System.Graphics.ES20.PixelFormat) format, (System.Graphics.ES20.PixelType) type, (IntPtr) pixels_ptr.AddrOfPinnedObject());
				pixels = (T6) pixels_ptr.Target;
			} finally {
				pixels_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Release resources consumed by the implementation's shader compiler
		/// </summary>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glReleaseShaderCompiler")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void ReleaseShaderCompiler() {
			Delegates.glReleaseShaderCompiler();
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Establish data storage, format and dimensions of a renderbuffer object's image
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies a binding to which the target of the allocation and must be GL_RENDERBUFFER.
		/// </para>
		/// </param>
		/// <param name="internalformat">
		/// <para>
		/// Specifies the internal format to use for the renderbuffer object's image.
		/// </para>
		/// </param>
		/// <param name="width">
		/// <para>
		/// Specifies the width of the renderbuffer, in pixels.
		/// </para>
		/// </param>
		/// <param name="height">
		/// <para>
		/// Specifies the height of the renderbuffer, in pixels.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glRenderbufferStorage")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void RenderbufferStorage(System.Graphics.ES20.RenderbufferTarget target, System.Graphics.ES20.RenderbufferInternalFormat internalformat, Int32 width, Int32 height) {
			Delegates.glRenderbufferStorage((System.Graphics.ES20.RenderbufferTarget) target, (System.Graphics.ES20.RenderbufferInternalFormat) internalformat, (Int32) width, (Int32) height);
		}

		/// <summary>[requires: v2.0 and 2.0]</summary>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glRenderbufferStorageMultisampleIMG")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void RenderbufferStorageMultisampleIMG() {
			Delegates.glRenderbufferStorageMultisampleIMG();
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify multisample coverage parameters
		/// </summary>
		/// <param name="value">
		/// <para>
		/// Specify a single floating-point sample coverage value. The value is clamped to the range [0 ,1]. The initial value is 1.0.
		/// </para>
		/// </param>
		/// <param name="invert">
		/// <para>
		/// Specify a single boolean value representing if the coverage masks should be inverted. GL_TRUE and GL_FALSE are accepted. The initial value is GL_FALSE.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glSampleCoverage")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void SampleCoverage(Single value, bool invert) {
			Delegates.glSampleCoverage((Single) value, (bool) invert);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Define the scissor box
		/// </summary>
		/// <param name="x">
		/// <para>
		/// Specify the lower left corner of the scissor box. Initially (0, 0).
		/// </para>
		/// </param>
		/// <param name="width">
		/// <para>
		/// Specify the width and height of the scissor box. When a GL context is first attached to a window, width and height are set to the dimensions of that window.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glScissor")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void Scissor(Int32 x, Int32 y, Int32 width, Int32 height) {
			Delegates.glScissor((Int32) x, (Int32) y, (Int32) width, (Int32) height);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Load pre-compiled shader binaries
		/// </summary>
		/// <param name="count">
		/// <para>
		/// Specifies the number of shader object handles contained in shaders.
		/// </para>
		/// </param>
		/// <param name="shaders">
		/// <para>
		/// Specifies the address of an array of shader handles into which to load pre-compiled shader binaries.
		/// </para>
		/// </param>
		/// <param name="binaryFormat">
		/// <para>
		/// Specifies the format of the shader binaries contained in binary.
		/// </para>
		/// </param>
		/// <param name="binary">
		/// <para>
		/// Specifies the address of an array of bytes containing pre-compiled binary shader code.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Specifies the length of the array whose address is given in binary.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glShaderBinary")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void ShaderBinary(Int32 n, Int32[] shaders, System.Graphics.ES20.ShaderBinaryFormat binaryformat, IntPtr binary, Int32 length) {
			unsafe
			{
				fixed (Int32* shaders_ptr = shaders) {
					Delegates.glShaderBinary((Int32) n, (UInt32*) shaders_ptr, (System.Graphics.ES20.ShaderBinaryFormat) binaryformat, (IntPtr) binary, (Int32) length);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Load pre-compiled shader binaries
		/// </summary>
		/// <param name="count">
		/// <para>
		/// Specifies the number of shader object handles contained in shaders.
		/// </para>
		/// </param>
		/// <param name="shaders">
		/// <para>
		/// Specifies the address of an array of shader handles into which to load pre-compiled shader binaries.
		/// </para>
		/// </param>
		/// <param name="binaryFormat">
		/// <para>
		/// Specifies the format of the shader binaries contained in binary.
		/// </para>
		/// </param>
		/// <param name="binary">
		/// <para>
		/// Specifies the address of an array of bytes containing pre-compiled binary shader code.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Specifies the length of the array whose address is given in binary.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glShaderBinary")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void ShaderBinary<T3>(Int32 n, Int32[] shaders, System.Graphics.ES20.ShaderBinaryFormat binaryformat, [InAttribute, OutAttribute] T3[] binary, Int32 length)
					where T3 : struct {
			unsafe
			{
				fixed (Int32* shaders_ptr = shaders) {
					GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
					try {
						Delegates.glShaderBinary((Int32) n, (UInt32*) shaders_ptr, (System.Graphics.ES20.ShaderBinaryFormat) binaryformat, (IntPtr) binary_ptr.AddrOfPinnedObject(), (Int32) length);
					} finally {
						binary_ptr.Free();
					}
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Load pre-compiled shader binaries
		/// </summary>
		/// <param name="count">
		/// <para>
		/// Specifies the number of shader object handles contained in shaders.
		/// </para>
		/// </param>
		/// <param name="shaders">
		/// <para>
		/// Specifies the address of an array of shader handles into which to load pre-compiled shader binaries.
		/// </para>
		/// </param>
		/// <param name="binaryFormat">
		/// <para>
		/// Specifies the format of the shader binaries contained in binary.
		/// </para>
		/// </param>
		/// <param name="binary">
		/// <para>
		/// Specifies the address of an array of bytes containing pre-compiled binary shader code.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Specifies the length of the array whose address is given in binary.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glShaderBinary")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void ShaderBinary<T3>(Int32 n, Int32[] shaders, System.Graphics.ES20.ShaderBinaryFormat binaryformat, [InAttribute, OutAttribute] T3[,] binary, Int32 length)
					where T3 : struct {
			unsafe
			{
				fixed (Int32* shaders_ptr = shaders) {
					GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
					try {
						Delegates.glShaderBinary((Int32) n, (UInt32*) shaders_ptr, (System.Graphics.ES20.ShaderBinaryFormat) binaryformat, (IntPtr) binary_ptr.AddrOfPinnedObject(), (Int32) length);
					} finally {
						binary_ptr.Free();
					}
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Load pre-compiled shader binaries
		/// </summary>
		/// <param name="count">
		/// <para>
		/// Specifies the number of shader object handles contained in shaders.
		/// </para>
		/// </param>
		/// <param name="shaders">
		/// <para>
		/// Specifies the address of an array of shader handles into which to load pre-compiled shader binaries.
		/// </para>
		/// </param>
		/// <param name="binaryFormat">
		/// <para>
		/// Specifies the format of the shader binaries contained in binary.
		/// </para>
		/// </param>
		/// <param name="binary">
		/// <para>
		/// Specifies the address of an array of bytes containing pre-compiled binary shader code.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Specifies the length of the array whose address is given in binary.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glShaderBinary")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void ShaderBinary<T3>(Int32 n, Int32[] shaders, System.Graphics.ES20.ShaderBinaryFormat binaryformat, [InAttribute, OutAttribute] T3[,,] binary, Int32 length)
					where T3 : struct {
			unsafe
			{
				fixed (Int32* shaders_ptr = shaders) {
					GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
					try {
						Delegates.glShaderBinary((Int32) n, (UInt32*) shaders_ptr, (System.Graphics.ES20.ShaderBinaryFormat) binaryformat, (IntPtr) binary_ptr.AddrOfPinnedObject(), (Int32) length);
					} finally {
						binary_ptr.Free();
					}
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Load pre-compiled shader binaries
		/// </summary>
		/// <param name="count">
		/// <para>
		/// Specifies the number of shader object handles contained in shaders.
		/// </para>
		/// </param>
		/// <param name="shaders">
		/// <para>
		/// Specifies the address of an array of shader handles into which to load pre-compiled shader binaries.
		/// </para>
		/// </param>
		/// <param name="binaryFormat">
		/// <para>
		/// Specifies the format of the shader binaries contained in binary.
		/// </para>
		/// </param>
		/// <param name="binary">
		/// <para>
		/// Specifies the address of an array of bytes containing pre-compiled binary shader code.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Specifies the length of the array whose address is given in binary.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glShaderBinary")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void ShaderBinary<T3>(Int32 n, Int32[] shaders, System.Graphics.ES20.ShaderBinaryFormat binaryformat, [InAttribute, OutAttribute] ref T3 binary, Int32 length)
					where T3 : struct {
			unsafe
			{
				fixed (Int32* shaders_ptr = shaders) {
					GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
					try {
						Delegates.glShaderBinary((Int32) n, (UInt32*) shaders_ptr, (System.Graphics.ES20.ShaderBinaryFormat) binaryformat, (IntPtr) binary_ptr.AddrOfPinnedObject(), (Int32) length);
						binary = (T3) binary_ptr.Target;
					} finally {
						binary_ptr.Free();
					}
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Load pre-compiled shader binaries
		/// </summary>
		/// <param name="count">
		/// <para>
		/// Specifies the number of shader object handles contained in shaders.
		/// </para>
		/// </param>
		/// <param name="shaders">
		/// <para>
		/// Specifies the address of an array of shader handles into which to load pre-compiled shader binaries.
		/// </para>
		/// </param>
		/// <param name="binaryFormat">
		/// <para>
		/// Specifies the format of the shader binaries contained in binary.
		/// </para>
		/// </param>
		/// <param name="binary">
		/// <para>
		/// Specifies the address of an array of bytes containing pre-compiled binary shader code.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Specifies the length of the array whose address is given in binary.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glShaderBinary")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void ShaderBinary(Int32 n, ref Int32 shaders, System.Graphics.ES20.ShaderBinaryFormat binaryformat, IntPtr binary, Int32 length) {
			unsafe
			{
				fixed (Int32* shaders_ptr = &shaders) {
					Delegates.glShaderBinary((Int32) n, (UInt32*) shaders_ptr, (System.Graphics.ES20.ShaderBinaryFormat) binaryformat, (IntPtr) binary, (Int32) length);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Load pre-compiled shader binaries
		/// </summary>
		/// <param name="count">
		/// <para>
		/// Specifies the number of shader object handles contained in shaders.
		/// </para>
		/// </param>
		/// <param name="shaders">
		/// <para>
		/// Specifies the address of an array of shader handles into which to load pre-compiled shader binaries.
		/// </para>
		/// </param>
		/// <param name="binaryFormat">
		/// <para>
		/// Specifies the format of the shader binaries contained in binary.
		/// </para>
		/// </param>
		/// <param name="binary">
		/// <para>
		/// Specifies the address of an array of bytes containing pre-compiled binary shader code.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Specifies the length of the array whose address is given in binary.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glShaderBinary")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void ShaderBinary<T3>(Int32 n, ref Int32 shaders, System.Graphics.ES20.ShaderBinaryFormat binaryformat, [InAttribute, OutAttribute] T3[] binary, Int32 length)
					where T3 : struct {
			unsafe
			{
				fixed (Int32* shaders_ptr = &shaders) {
					GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
					try {
						Delegates.glShaderBinary((Int32) n, (UInt32*) shaders_ptr, (System.Graphics.ES20.ShaderBinaryFormat) binaryformat, (IntPtr) binary_ptr.AddrOfPinnedObject(), (Int32) length);
					} finally {
						binary_ptr.Free();
					}
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Load pre-compiled shader binaries
		/// </summary>
		/// <param name="count">
		/// <para>
		/// Specifies the number of shader object handles contained in shaders.
		/// </para>
		/// </param>
		/// <param name="shaders">
		/// <para>
		/// Specifies the address of an array of shader handles into which to load pre-compiled shader binaries.
		/// </para>
		/// </param>
		/// <param name="binaryFormat">
		/// <para>
		/// Specifies the format of the shader binaries contained in binary.
		/// </para>
		/// </param>
		/// <param name="binary">
		/// <para>
		/// Specifies the address of an array of bytes containing pre-compiled binary shader code.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Specifies the length of the array whose address is given in binary.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glShaderBinary")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void ShaderBinary<T3>(Int32 n, ref Int32 shaders, System.Graphics.ES20.ShaderBinaryFormat binaryformat, [InAttribute, OutAttribute] T3[,] binary, Int32 length)
					where T3 : struct {
			unsafe
			{
				fixed (Int32* shaders_ptr = &shaders) {
					GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
					try {
						Delegates.glShaderBinary((Int32) n, (UInt32*) shaders_ptr, (System.Graphics.ES20.ShaderBinaryFormat) binaryformat, (IntPtr) binary_ptr.AddrOfPinnedObject(), (Int32) length);
					} finally {
						binary_ptr.Free();
					}
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Load pre-compiled shader binaries
		/// </summary>
		/// <param name="count">
		/// <para>
		/// Specifies the number of shader object handles contained in shaders.
		/// </para>
		/// </param>
		/// <param name="shaders">
		/// <para>
		/// Specifies the address of an array of shader handles into which to load pre-compiled shader binaries.
		/// </para>
		/// </param>
		/// <param name="binaryFormat">
		/// <para>
		/// Specifies the format of the shader binaries contained in binary.
		/// </para>
		/// </param>
		/// <param name="binary">
		/// <para>
		/// Specifies the address of an array of bytes containing pre-compiled binary shader code.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Specifies the length of the array whose address is given in binary.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glShaderBinary")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void ShaderBinary<T3>(Int32 n, ref Int32 shaders, System.Graphics.ES20.ShaderBinaryFormat binaryformat, [InAttribute, OutAttribute] T3[,,] binary, Int32 length)
					where T3 : struct {
			unsafe
			{
				fixed (Int32* shaders_ptr = &shaders) {
					GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
					try {
						Delegates.glShaderBinary((Int32) n, (UInt32*) shaders_ptr, (System.Graphics.ES20.ShaderBinaryFormat) binaryformat, (IntPtr) binary_ptr.AddrOfPinnedObject(), (Int32) length);
					} finally {
						binary_ptr.Free();
					}
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Load pre-compiled shader binaries
		/// </summary>
		/// <param name="count">
		/// <para>
		/// Specifies the number of shader object handles contained in shaders.
		/// </para>
		/// </param>
		/// <param name="shaders">
		/// <para>
		/// Specifies the address of an array of shader handles into which to load pre-compiled shader binaries.
		/// </para>
		/// </param>
		/// <param name="binaryFormat">
		/// <para>
		/// Specifies the format of the shader binaries contained in binary.
		/// </para>
		/// </param>
		/// <param name="binary">
		/// <para>
		/// Specifies the address of an array of bytes containing pre-compiled binary shader code.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Specifies the length of the array whose address is given in binary.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glShaderBinary")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void ShaderBinary<T3>(Int32 n, ref Int32 shaders, System.Graphics.ES20.ShaderBinaryFormat binaryformat, [InAttribute, OutAttribute] ref T3 binary, Int32 length)
					where T3 : struct {
			unsafe
			{
				fixed (Int32* shaders_ptr = &shaders) {
					GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
					try {
						Delegates.glShaderBinary((Int32) n, (UInt32*) shaders_ptr, (System.Graphics.ES20.ShaderBinaryFormat) binaryformat, (IntPtr) binary_ptr.AddrOfPinnedObject(), (Int32) length);
						binary = (T3) binary_ptr.Target;
					} finally {
						binary_ptr.Free();
					}
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Load pre-compiled shader binaries
		/// </summary>
		/// <param name="count">
		/// <para>
		/// Specifies the number of shader object handles contained in shaders.
		/// </para>
		/// </param>
		/// <param name="shaders">
		/// <para>
		/// Specifies the address of an array of shader handles into which to load pre-compiled shader binaries.
		/// </para>
		/// </param>
		/// <param name="binaryFormat">
		/// <para>
		/// Specifies the format of the shader binaries contained in binary.
		/// </para>
		/// </param>
		/// <param name="binary">
		/// <para>
		/// Specifies the address of an array of bytes containing pre-compiled binary shader code.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Specifies the length of the array whose address is given in binary.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glShaderBinary")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void ShaderBinary(Int32 n, Int32* shaders, System.Graphics.ES20.ShaderBinaryFormat binaryformat, IntPtr binary, Int32 length) {
			Delegates.glShaderBinary((Int32) n, (UInt32*) shaders, (System.Graphics.ES20.ShaderBinaryFormat) binaryformat, (IntPtr) binary, (Int32) length);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Load pre-compiled shader binaries
		/// </summary>
		/// <param name="count">
		/// <para>
		/// Specifies the number of shader object handles contained in shaders.
		/// </para>
		/// </param>
		/// <param name="shaders">
		/// <para>
		/// Specifies the address of an array of shader handles into which to load pre-compiled shader binaries.
		/// </para>
		/// </param>
		/// <param name="binaryFormat">
		/// <para>
		/// Specifies the format of the shader binaries contained in binary.
		/// </para>
		/// </param>
		/// <param name="binary">
		/// <para>
		/// Specifies the address of an array of bytes containing pre-compiled binary shader code.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Specifies the length of the array whose address is given in binary.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glShaderBinary")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void ShaderBinary<T3>(Int32 n, Int32* shaders, System.Graphics.ES20.ShaderBinaryFormat binaryformat, [InAttribute, OutAttribute] T3[] binary, Int32 length)
			where T3 : struct {
			GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
			try {
				Delegates.glShaderBinary((Int32) n, (UInt32*) shaders, (System.Graphics.ES20.ShaderBinaryFormat) binaryformat, (IntPtr) binary_ptr.AddrOfPinnedObject(), (Int32) length);
			} finally {
				binary_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Load pre-compiled shader binaries
		/// </summary>
		/// <param name="count">
		/// <para>
		/// Specifies the number of shader object handles contained in shaders.
		/// </para>
		/// </param>
		/// <param name="shaders">
		/// <para>
		/// Specifies the address of an array of shader handles into which to load pre-compiled shader binaries.
		/// </para>
		/// </param>
		/// <param name="binaryFormat">
		/// <para>
		/// Specifies the format of the shader binaries contained in binary.
		/// </para>
		/// </param>
		/// <param name="binary">
		/// <para>
		/// Specifies the address of an array of bytes containing pre-compiled binary shader code.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Specifies the length of the array whose address is given in binary.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glShaderBinary")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void ShaderBinary<T3>(Int32 n, Int32* shaders, System.Graphics.ES20.ShaderBinaryFormat binaryformat, [InAttribute, OutAttribute] T3[,] binary, Int32 length)
			where T3 : struct {
			GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
			try {
				Delegates.glShaderBinary((Int32) n, (UInt32*) shaders, (System.Graphics.ES20.ShaderBinaryFormat) binaryformat, (IntPtr) binary_ptr.AddrOfPinnedObject(), (Int32) length);
			} finally {
				binary_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Load pre-compiled shader binaries
		/// </summary>
		/// <param name="count">
		/// <para>
		/// Specifies the number of shader object handles contained in shaders.
		/// </para>
		/// </param>
		/// <param name="shaders">
		/// <para>
		/// Specifies the address of an array of shader handles into which to load pre-compiled shader binaries.
		/// </para>
		/// </param>
		/// <param name="binaryFormat">
		/// <para>
		/// Specifies the format of the shader binaries contained in binary.
		/// </para>
		/// </param>
		/// <param name="binary">
		/// <para>
		/// Specifies the address of an array of bytes containing pre-compiled binary shader code.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Specifies the length of the array whose address is given in binary.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glShaderBinary")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void ShaderBinary<T3>(Int32 n, Int32* shaders, System.Graphics.ES20.ShaderBinaryFormat binaryformat, [InAttribute, OutAttribute] T3[,,] binary, Int32 length)
			where T3 : struct {
			GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
			try {
				Delegates.glShaderBinary((Int32) n, (UInt32*) shaders, (System.Graphics.ES20.ShaderBinaryFormat) binaryformat, (IntPtr) binary_ptr.AddrOfPinnedObject(), (Int32) length);
			} finally {
				binary_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Load pre-compiled shader binaries
		/// </summary>
		/// <param name="count">
		/// <para>
		/// Specifies the number of shader object handles contained in shaders.
		/// </para>
		/// </param>
		/// <param name="shaders">
		/// <para>
		/// Specifies the address of an array of shader handles into which to load pre-compiled shader binaries.
		/// </para>
		/// </param>
		/// <param name="binaryFormat">
		/// <para>
		/// Specifies the format of the shader binaries contained in binary.
		/// </para>
		/// </param>
		/// <param name="binary">
		/// <para>
		/// Specifies the address of an array of bytes containing pre-compiled binary shader code.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Specifies the length of the array whose address is given in binary.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glShaderBinary")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void ShaderBinary<T3>(Int32 n, Int32* shaders, System.Graphics.ES20.ShaderBinaryFormat binaryformat, [InAttribute, OutAttribute] ref T3 binary, Int32 length)
			where T3 : struct {
			GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
			try {
				Delegates.glShaderBinary((Int32) n, (UInt32*) shaders, (System.Graphics.ES20.ShaderBinaryFormat) binaryformat, (IntPtr) binary_ptr.AddrOfPinnedObject(), (Int32) length);
				binary = (T3) binary_ptr.Target;
			} finally {
				binary_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Load pre-compiled shader binaries
		/// </summary>
		/// <param name="count">
		/// <para>
		/// Specifies the number of shader object handles contained in shaders.
		/// </para>
		/// </param>
		/// <param name="shaders">
		/// <para>
		/// Specifies the address of an array of shader handles into which to load pre-compiled shader binaries.
		/// </para>
		/// </param>
		/// <param name="binaryFormat">
		/// <para>
		/// Specifies the format of the shader binaries contained in binary.
		/// </para>
		/// </param>
		/// <param name="binary">
		/// <para>
		/// Specifies the address of an array of bytes containing pre-compiled binary shader code.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Specifies the length of the array whose address is given in binary.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glShaderBinary")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void ShaderBinary(Int32 n, UInt32[] shaders, System.Graphics.ES20.ShaderBinaryFormat binaryformat, IntPtr binary, Int32 length) {
			unsafe
			{
				fixed (UInt32* shaders_ptr = shaders) {
					Delegates.glShaderBinary((Int32) n, (UInt32*) shaders_ptr, (System.Graphics.ES20.ShaderBinaryFormat) binaryformat, (IntPtr) binary, (Int32) length);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Load pre-compiled shader binaries
		/// </summary>
		/// <param name="count">
		/// <para>
		/// Specifies the number of shader object handles contained in shaders.
		/// </para>
		/// </param>
		/// <param name="shaders">
		/// <para>
		/// Specifies the address of an array of shader handles into which to load pre-compiled shader binaries.
		/// </para>
		/// </param>
		/// <param name="binaryFormat">
		/// <para>
		/// Specifies the format of the shader binaries contained in binary.
		/// </para>
		/// </param>
		/// <param name="binary">
		/// <para>
		/// Specifies the address of an array of bytes containing pre-compiled binary shader code.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Specifies the length of the array whose address is given in binary.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glShaderBinary")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void ShaderBinary<T3>(Int32 n, UInt32[] shaders, System.Graphics.ES20.ShaderBinaryFormat binaryformat, [InAttribute, OutAttribute] T3[] binary, Int32 length)
			where T3 : struct {
			unsafe
			{
				fixed (UInt32* shaders_ptr = shaders) {
					GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
					try {
						Delegates.glShaderBinary((Int32) n, (UInt32*) shaders_ptr, (System.Graphics.ES20.ShaderBinaryFormat) binaryformat, (IntPtr) binary_ptr.AddrOfPinnedObject(), (Int32) length);
					} finally {
						binary_ptr.Free();
					}
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Load pre-compiled shader binaries
		/// </summary>
		/// <param name="count">
		/// <para>
		/// Specifies the number of shader object handles contained in shaders.
		/// </para>
		/// </param>
		/// <param name="shaders">
		/// <para>
		/// Specifies the address of an array of shader handles into which to load pre-compiled shader binaries.
		/// </para>
		/// </param>
		/// <param name="binaryFormat">
		/// <para>
		/// Specifies the format of the shader binaries contained in binary.
		/// </para>
		/// </param>
		/// <param name="binary">
		/// <para>
		/// Specifies the address of an array of bytes containing pre-compiled binary shader code.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Specifies the length of the array whose address is given in binary.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glShaderBinary")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void ShaderBinary<T3>(Int32 n, UInt32[] shaders, System.Graphics.ES20.ShaderBinaryFormat binaryformat, [InAttribute, OutAttribute] T3[,] binary, Int32 length)
			where T3 : struct {
			unsafe
			{
				fixed (UInt32* shaders_ptr = shaders) {
					GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
					try {
						Delegates.glShaderBinary((Int32) n, (UInt32*) shaders_ptr, (System.Graphics.ES20.ShaderBinaryFormat) binaryformat, (IntPtr) binary_ptr.AddrOfPinnedObject(), (Int32) length);
					} finally {
						binary_ptr.Free();
					}
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Load pre-compiled shader binaries
		/// </summary>
		/// <param name="count">
		/// <para>
		/// Specifies the number of shader object handles contained in shaders.
		/// </para>
		/// </param>
		/// <param name="shaders">
		/// <para>
		/// Specifies the address of an array of shader handles into which to load pre-compiled shader binaries.
		/// </para>
		/// </param>
		/// <param name="binaryFormat">
		/// <para>
		/// Specifies the format of the shader binaries contained in binary.
		/// </para>
		/// </param>
		/// <param name="binary">
		/// <para>
		/// Specifies the address of an array of bytes containing pre-compiled binary shader code.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Specifies the length of the array whose address is given in binary.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glShaderBinary")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void ShaderBinary<T3>(Int32 n, UInt32[] shaders, System.Graphics.ES20.ShaderBinaryFormat binaryformat, [InAttribute, OutAttribute] T3[,,] binary, Int32 length)
			where T3 : struct {
			unsafe
			{
				fixed (UInt32* shaders_ptr = shaders) {
					GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
					try {
						Delegates.glShaderBinary((Int32) n, (UInt32*) shaders_ptr, (System.Graphics.ES20.ShaderBinaryFormat) binaryformat, (IntPtr) binary_ptr.AddrOfPinnedObject(), (Int32) length);
					} finally {
						binary_ptr.Free();
					}
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Load pre-compiled shader binaries
		/// </summary>
		/// <param name="count">
		/// <para>
		/// Specifies the number of shader object handles contained in shaders.
		/// </para>
		/// </param>
		/// <param name="shaders">
		/// <para>
		/// Specifies the address of an array of shader handles into which to load pre-compiled shader binaries.
		/// </para>
		/// </param>
		/// <param name="binaryFormat">
		/// <para>
		/// Specifies the format of the shader binaries contained in binary.
		/// </para>
		/// </param>
		/// <param name="binary">
		/// <para>
		/// Specifies the address of an array of bytes containing pre-compiled binary shader code.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Specifies the length of the array whose address is given in binary.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glShaderBinary")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void ShaderBinary<T3>(Int32 n, UInt32[] shaders, System.Graphics.ES20.ShaderBinaryFormat binaryformat, [InAttribute, OutAttribute] ref T3 binary, Int32 length)
			where T3 : struct {
			unsafe
			{
				fixed (UInt32* shaders_ptr = shaders) {
					GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
					try {
						Delegates.glShaderBinary((Int32) n, (UInt32*) shaders_ptr, (System.Graphics.ES20.ShaderBinaryFormat) binaryformat, (IntPtr) binary_ptr.AddrOfPinnedObject(), (Int32) length);
						binary = (T3) binary_ptr.Target;
					} finally {
						binary_ptr.Free();
					}
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Load pre-compiled shader binaries
		/// </summary>
		/// <param name="count">
		/// <para>
		/// Specifies the number of shader object handles contained in shaders.
		/// </para>
		/// </param>
		/// <param name="shaders">
		/// <para>
		/// Specifies the address of an array of shader handles into which to load pre-compiled shader binaries.
		/// </para>
		/// </param>
		/// <param name="binaryFormat">
		/// <para>
		/// Specifies the format of the shader binaries contained in binary.
		/// </para>
		/// </param>
		/// <param name="binary">
		/// <para>
		/// Specifies the address of an array of bytes containing pre-compiled binary shader code.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Specifies the length of the array whose address is given in binary.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glShaderBinary")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void ShaderBinary(Int32 n, ref UInt32 shaders, System.Graphics.ES20.ShaderBinaryFormat binaryformat, IntPtr binary, Int32 length) {
			unsafe
			{
				fixed (UInt32* shaders_ptr = &shaders) {
					Delegates.glShaderBinary((Int32) n, (UInt32*) shaders_ptr, (System.Graphics.ES20.ShaderBinaryFormat) binaryformat, (IntPtr) binary, (Int32) length);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Load pre-compiled shader binaries
		/// </summary>
		/// <param name="count">
		/// <para>
		/// Specifies the number of shader object handles contained in shaders.
		/// </para>
		/// </param>
		/// <param name="shaders">
		/// <para>
		/// Specifies the address of an array of shader handles into which to load pre-compiled shader binaries.
		/// </para>
		/// </param>
		/// <param name="binaryFormat">
		/// <para>
		/// Specifies the format of the shader binaries contained in binary.
		/// </para>
		/// </param>
		/// <param name="binary">
		/// <para>
		/// Specifies the address of an array of bytes containing pre-compiled binary shader code.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Specifies the length of the array whose address is given in binary.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glShaderBinary")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void ShaderBinary<T3>(Int32 n, ref UInt32 shaders, System.Graphics.ES20.ShaderBinaryFormat binaryformat, [InAttribute, OutAttribute] T3[] binary, Int32 length)
			where T3 : struct {
			unsafe
			{
				fixed (UInt32* shaders_ptr = &shaders) {
					GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
					try {
						Delegates.glShaderBinary((Int32) n, (UInt32*) shaders_ptr, (System.Graphics.ES20.ShaderBinaryFormat) binaryformat, (IntPtr) binary_ptr.AddrOfPinnedObject(), (Int32) length);
					} finally {
						binary_ptr.Free();
					}
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Load pre-compiled shader binaries
		/// </summary>
		/// <param name="count">
		/// <para>
		/// Specifies the number of shader object handles contained in shaders.
		/// </para>
		/// </param>
		/// <param name="shaders">
		/// <para>
		/// Specifies the address of an array of shader handles into which to load pre-compiled shader binaries.
		/// </para>
		/// </param>
		/// <param name="binaryFormat">
		/// <para>
		/// Specifies the format of the shader binaries contained in binary.
		/// </para>
		/// </param>
		/// <param name="binary">
		/// <para>
		/// Specifies the address of an array of bytes containing pre-compiled binary shader code.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Specifies the length of the array whose address is given in binary.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glShaderBinary")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void ShaderBinary<T3>(Int32 n, ref UInt32 shaders, System.Graphics.ES20.ShaderBinaryFormat binaryformat, [InAttribute, OutAttribute] T3[,] binary, Int32 length)
			where T3 : struct {
			unsafe
			{
				fixed (UInt32* shaders_ptr = &shaders) {
					GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
					try {
						Delegates.glShaderBinary((Int32) n, (UInt32*) shaders_ptr, (System.Graphics.ES20.ShaderBinaryFormat) binaryformat, (IntPtr) binary_ptr.AddrOfPinnedObject(), (Int32) length);
					} finally {
						binary_ptr.Free();
					}
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Load pre-compiled shader binaries
		/// </summary>
		/// <param name="count">
		/// <para>
		/// Specifies the number of shader object handles contained in shaders.
		/// </para>
		/// </param>
		/// <param name="shaders">
		/// <para>
		/// Specifies the address of an array of shader handles into which to load pre-compiled shader binaries.
		/// </para>
		/// </param>
		/// <param name="binaryFormat">
		/// <para>
		/// Specifies the format of the shader binaries contained in binary.
		/// </para>
		/// </param>
		/// <param name="binary">
		/// <para>
		/// Specifies the address of an array of bytes containing pre-compiled binary shader code.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Specifies the length of the array whose address is given in binary.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glShaderBinary")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void ShaderBinary<T3>(Int32 n, ref UInt32 shaders, System.Graphics.ES20.ShaderBinaryFormat binaryformat, [InAttribute, OutAttribute] T3[,,] binary, Int32 length)
			where T3 : struct {
			unsafe
			{
				fixed (UInt32* shaders_ptr = &shaders) {
					GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
					try {
						Delegates.glShaderBinary((Int32) n, (UInt32*) shaders_ptr, (System.Graphics.ES20.ShaderBinaryFormat) binaryformat, (IntPtr) binary_ptr.AddrOfPinnedObject(), (Int32) length);
					} finally {
						binary_ptr.Free();
					}
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Load pre-compiled shader binaries
		/// </summary>
		/// <param name="count">
		/// <para>
		/// Specifies the number of shader object handles contained in shaders.
		/// </para>
		/// </param>
		/// <param name="shaders">
		/// <para>
		/// Specifies the address of an array of shader handles into which to load pre-compiled shader binaries.
		/// </para>
		/// </param>
		/// <param name="binaryFormat">
		/// <para>
		/// Specifies the format of the shader binaries contained in binary.
		/// </para>
		/// </param>
		/// <param name="binary">
		/// <para>
		/// Specifies the address of an array of bytes containing pre-compiled binary shader code.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Specifies the length of the array whose address is given in binary.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glShaderBinary")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void ShaderBinary<T3>(Int32 n, ref UInt32 shaders, System.Graphics.ES20.ShaderBinaryFormat binaryformat, [InAttribute, OutAttribute] ref T3 binary, Int32 length)
			where T3 : struct {
			unsafe
			{
				fixed (UInt32* shaders_ptr = &shaders) {
					GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
					try {
						Delegates.glShaderBinary((Int32) n, (UInt32*) shaders_ptr, (System.Graphics.ES20.ShaderBinaryFormat) binaryformat, (IntPtr) binary_ptr.AddrOfPinnedObject(), (Int32) length);
						binary = (T3) binary_ptr.Target;
					} finally {
						binary_ptr.Free();
					}
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Load pre-compiled shader binaries
		/// </summary>
		/// <param name="count">
		/// <para>
		/// Specifies the number of shader object handles contained in shaders.
		/// </para>
		/// </param>
		/// <param name="shaders">
		/// <para>
		/// Specifies the address of an array of shader handles into which to load pre-compiled shader binaries.
		/// </para>
		/// </param>
		/// <param name="binaryFormat">
		/// <para>
		/// Specifies the format of the shader binaries contained in binary.
		/// </para>
		/// </param>
		/// <param name="binary">
		/// <para>
		/// Specifies the address of an array of bytes containing pre-compiled binary shader code.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Specifies the length of the array whose address is given in binary.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glShaderBinary")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void ShaderBinary(Int32 n, UInt32* shaders, System.Graphics.ES20.ShaderBinaryFormat binaryformat, IntPtr binary, Int32 length) {
			Delegates.glShaderBinary((Int32) n, (UInt32*) shaders, (System.Graphics.ES20.ShaderBinaryFormat) binaryformat, (IntPtr) binary, (Int32) length);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Load pre-compiled shader binaries
		/// </summary>
		/// <param name="count">
		/// <para>
		/// Specifies the number of shader object handles contained in shaders.
		/// </para>
		/// </param>
		/// <param name="shaders">
		/// <para>
		/// Specifies the address of an array of shader handles into which to load pre-compiled shader binaries.
		/// </para>
		/// </param>
		/// <param name="binaryFormat">
		/// <para>
		/// Specifies the format of the shader binaries contained in binary.
		/// </para>
		/// </param>
		/// <param name="binary">
		/// <para>
		/// Specifies the address of an array of bytes containing pre-compiled binary shader code.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Specifies the length of the array whose address is given in binary.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glShaderBinary")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void ShaderBinary<T3>(Int32 n, UInt32* shaders, System.Graphics.ES20.ShaderBinaryFormat binaryformat, [InAttribute, OutAttribute] T3[] binary, Int32 length)
			where T3 : struct {
			GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
			try {
				Delegates.glShaderBinary((Int32) n, (UInt32*) shaders, (System.Graphics.ES20.ShaderBinaryFormat) binaryformat, (IntPtr) binary_ptr.AddrOfPinnedObject(), (Int32) length);
			} finally {
				binary_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Load pre-compiled shader binaries
		/// </summary>
		/// <param name="count">
		/// <para>
		/// Specifies the number of shader object handles contained in shaders.
		/// </para>
		/// </param>
		/// <param name="shaders">
		/// <para>
		/// Specifies the address of an array of shader handles into which to load pre-compiled shader binaries.
		/// </para>
		/// </param>
		/// <param name="binaryFormat">
		/// <para>
		/// Specifies the format of the shader binaries contained in binary.
		/// </para>
		/// </param>
		/// <param name="binary">
		/// <para>
		/// Specifies the address of an array of bytes containing pre-compiled binary shader code.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Specifies the length of the array whose address is given in binary.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glShaderBinary")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void ShaderBinary<T3>(Int32 n, UInt32* shaders, System.Graphics.ES20.ShaderBinaryFormat binaryformat, [InAttribute, OutAttribute] T3[,] binary, Int32 length)
			where T3 : struct {
			GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
			try {
				Delegates.glShaderBinary((Int32) n, (UInt32*) shaders, (System.Graphics.ES20.ShaderBinaryFormat) binaryformat, (IntPtr) binary_ptr.AddrOfPinnedObject(), (Int32) length);
			} finally {
				binary_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Load pre-compiled shader binaries
		/// </summary>
		/// <param name="count">
		/// <para>
		/// Specifies the number of shader object handles contained in shaders.
		/// </para>
		/// </param>
		/// <param name="shaders">
		/// <para>
		/// Specifies the address of an array of shader handles into which to load pre-compiled shader binaries.
		/// </para>
		/// </param>
		/// <param name="binaryFormat">
		/// <para>
		/// Specifies the format of the shader binaries contained in binary.
		/// </para>
		/// </param>
		/// <param name="binary">
		/// <para>
		/// Specifies the address of an array of bytes containing pre-compiled binary shader code.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Specifies the length of the array whose address is given in binary.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glShaderBinary")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void ShaderBinary<T3>(Int32 n, UInt32* shaders, System.Graphics.ES20.ShaderBinaryFormat binaryformat, [InAttribute, OutAttribute] T3[,,] binary, Int32 length)
			where T3 : struct {
			GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
			try {
				Delegates.glShaderBinary((Int32) n, (UInt32*) shaders, (System.Graphics.ES20.ShaderBinaryFormat) binaryformat, (IntPtr) binary_ptr.AddrOfPinnedObject(), (Int32) length);
			} finally {
				binary_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Load pre-compiled shader binaries
		/// </summary>
		/// <param name="count">
		/// <para>
		/// Specifies the number of shader object handles contained in shaders.
		/// </para>
		/// </param>
		/// <param name="shaders">
		/// <para>
		/// Specifies the address of an array of shader handles into which to load pre-compiled shader binaries.
		/// </para>
		/// </param>
		/// <param name="binaryFormat">
		/// <para>
		/// Specifies the format of the shader binaries contained in binary.
		/// </para>
		/// </param>
		/// <param name="binary">
		/// <para>
		/// Specifies the address of an array of bytes containing pre-compiled binary shader code.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Specifies the length of the array whose address is given in binary.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glShaderBinary")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void ShaderBinary<T3>(Int32 n, UInt32* shaders, System.Graphics.ES20.ShaderBinaryFormat binaryformat, [InAttribute, OutAttribute] ref T3 binary, Int32 length)
			where T3 : struct {
			GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
			try {
				Delegates.glShaderBinary((Int32) n, (UInt32*) shaders, (System.Graphics.ES20.ShaderBinaryFormat) binaryformat, (IntPtr) binary_ptr.AddrOfPinnedObject(), (Int32) length);
				binary = (T3) binary_ptr.Target;
			} finally {
				binary_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Replaces the source code in a shader object
		/// </summary>
		/// <param name="shader">
		/// <para>
		/// Specifies the handle of the shader object whose source code is to be replaced.
		/// </para>
		/// </param>
		/// <param name="count">
		/// <para>
		/// Specifies the number of elements in the string and length arrays.
		/// </para>
		/// </param>
		/// <param name="string">
		/// <para>
		/// Specifies an array of pointers to strings containing the source code to be loaded into the shader.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Specifies an array of string lengths.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glShaderSource")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void ShaderSource(Int32 shader, Int32 count, String[] @string, Int32[] length) {
			unsafe
			{
				fixed (Int32* length_ptr = length) {
					Delegates.glShaderSource((UInt32) shader, (Int32) count, (String[]) @string, (Int32*) length_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Replaces the source code in a shader object
		/// </summary>
		/// <param name="shader">
		/// <para>
		/// Specifies the handle of the shader object whose source code is to be replaced.
		/// </para>
		/// </param>
		/// <param name="count">
		/// <para>
		/// Specifies the number of elements in the string and length arrays.
		/// </para>
		/// </param>
		/// <param name="string">
		/// <para>
		/// Specifies an array of pointers to strings containing the source code to be loaded into the shader.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Specifies an array of string lengths.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glShaderSource")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void ShaderSource(Int32 shader, Int32 count, String[] @string, ref Int32 length) {
			unsafe
			{
				fixed (Int32* length_ptr = &length) {
					Delegates.glShaderSource((UInt32) shader, (Int32) count, (String[]) @string, (Int32*) length_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Replaces the source code in a shader object
		/// </summary>
		/// <param name="shader">
		/// <para>
		/// Specifies the handle of the shader object whose source code is to be replaced.
		/// </para>
		/// </param>
		/// <param name="count">
		/// <para>
		/// Specifies the number of elements in the string and length arrays.
		/// </para>
		/// </param>
		/// <param name="string">
		/// <para>
		/// Specifies an array of pointers to strings containing the source code to be loaded into the shader.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Specifies an array of string lengths.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glShaderSource")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void ShaderSource(Int32 shader, Int32 count, String[] @string, Int32* length) {
			Delegates.glShaderSource((UInt32) shader, (Int32) count, (String[]) @string, (Int32*) length);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Replaces the source code in a shader object
		/// </summary>
		/// <param name="shader">
		/// <para>
		/// Specifies the handle of the shader object whose source code is to be replaced.
		/// </para>
		/// </param>
		/// <param name="count">
		/// <para>
		/// Specifies the number of elements in the string and length arrays.
		/// </para>
		/// </param>
		/// <param name="string">
		/// <para>
		/// Specifies an array of pointers to strings containing the source code to be loaded into the shader.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Specifies an array of string lengths.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glShaderSource")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void ShaderSource(UInt32 shader, Int32 count, String[] @string, Int32[] length) {
			unsafe
			{
				fixed (Int32* length_ptr = length) {
					Delegates.glShaderSource((UInt32) shader, (Int32) count, (String[]) @string, (Int32*) length_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Replaces the source code in a shader object
		/// </summary>
		/// <param name="shader">
		/// <para>
		/// Specifies the handle of the shader object whose source code is to be replaced.
		/// </para>
		/// </param>
		/// <param name="count">
		/// <para>
		/// Specifies the number of elements in the string and length arrays.
		/// </para>
		/// </param>
		/// <param name="string">
		/// <para>
		/// Specifies an array of pointers to strings containing the source code to be loaded into the shader.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Specifies an array of string lengths.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glShaderSource")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void ShaderSource(UInt32 shader, Int32 count, String[] @string, ref Int32 length) {
			unsafe
			{
				fixed (Int32* length_ptr = &length) {
					Delegates.glShaderSource((UInt32) shader, (Int32) count, (String[]) @string, (Int32*) length_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Replaces the source code in a shader object
		/// </summary>
		/// <param name="shader">
		/// <para>
		/// Specifies the handle of the shader object whose source code is to be replaced.
		/// </para>
		/// </param>
		/// <param name="count">
		/// <para>
		/// Specifies the number of elements in the string and length arrays.
		/// </para>
		/// </param>
		/// <param name="string">
		/// <para>
		/// Specifies an array of pointers to strings containing the source code to be loaded into the shader.
		/// </para>
		/// </param>
		/// <param name="length">
		/// <para>
		/// Specifies an array of string lengths.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glShaderSource")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void ShaderSource(UInt32 shader, Int32 count, String[] @string, Int32* length) {
			Delegates.glShaderSource((UInt32) shader, (Int32) count, (String[]) @string, (Int32*) length);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Set front and back function and reference value for stencil testing
		/// </summary>
		/// <param name="func">
		/// <para>
		/// Specifies the test function. Eight symbolic constants are valid: GL_NEVER, GL_LESS, GL_LEQUAL, GL_GREATER, GL_GEQUAL, GL_EQUAL, GL_NOTEQUAL, and GL_ALWAYS. The initial value is GL_ALWAYS.
		/// </para>
		/// </param>
		/// <param name="ref">
		/// <para>
		/// Specifies the reference value for the stencil test. ref is clamped to the range [0, 2 sup n - 1], where is the number of bitplanes in the stencil buffer. The initial value is 0.
		/// </para>
		/// </param>
		/// <param name="mask">
		/// <para>
		/// Specifies a mask that is ANDed with both the reference value and the stored stencil value when the test is done. The initial value is all 1's.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glStencilFunc")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void StencilFunc(System.Graphics.ES20.StencilFunction func, Int32 @ref, Int32 mask) {
			Delegates.glStencilFunc((System.Graphics.ES20.StencilFunction) func, (Int32) @ref, (UInt32) mask);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Set front and back function and reference value for stencil testing
		/// </summary>
		/// <param name="func">
		/// <para>
		/// Specifies the test function. Eight symbolic constants are valid: GL_NEVER, GL_LESS, GL_LEQUAL, GL_GREATER, GL_GEQUAL, GL_EQUAL, GL_NOTEQUAL, and GL_ALWAYS. The initial value is GL_ALWAYS.
		/// </para>
		/// </param>
		/// <param name="ref">
		/// <para>
		/// Specifies the reference value for the stencil test. ref is clamped to the range [0, 2 sup n - 1], where is the number of bitplanes in the stencil buffer. The initial value is 0.
		/// </para>
		/// </param>
		/// <param name="mask">
		/// <para>
		/// Specifies a mask that is ANDed with both the reference value and the stored stencil value when the test is done. The initial value is all 1's.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glStencilFunc")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void StencilFunc(System.Graphics.ES20.StencilFunction func, Int32 @ref, UInt32 mask) {
			Delegates.glStencilFunc((System.Graphics.ES20.StencilFunction) func, (Int32) @ref, (UInt32) mask);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Set front and/or back function and reference value for stencil testing
		/// </summary>
		/// <param name="face">
		/// <para>
		/// Specifies whether front and/or back stencil state is updated. Three symbolic constants are valid: GL_FRONT, GL_BACK, and GL_FRONT_AND_BACK.
		/// </para>
		/// </param>
		/// <param name="func">
		/// <para>
		/// Specifies the test function. Eight symbolic constants are valid: GL_NEVER, GL_LESS, GL_LEQUAL, GL_GREATER, GL_GEQUAL, GL_EQUAL, GL_NOTEQUAL, and GL_ALWAYS. The initial value is GL_ALWAYS.
		/// </para>
		/// </param>
		/// <param name="ref">
		/// <para>
		/// Specifies the reference value for the stencil test. ref is clamped to the range [0, 2 sup n - 1], where is the number of bitplanes in the stencil buffer. The initial value is 0.
		/// </para>
		/// </param>
		/// <param name="mask">
		/// <para>
		/// Specifies a mask that is ANDed with both the reference value and the stored stencil value when the test is done. The initial value is all 1's.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glStencilFuncSeparate")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void StencilFuncSeparate(System.Graphics.ES20.CullFaceMode face, System.Graphics.ES20.StencilFunction func, Int32 @ref, Int32 mask) {
			Delegates.glStencilFuncSeparate((System.Graphics.ES20.CullFaceMode) face, (System.Graphics.ES20.StencilFunction) func, (Int32) @ref, (UInt32) mask);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Set front and/or back function and reference value for stencil testing
		/// </summary>
		/// <param name="face">
		/// <para>
		/// Specifies whether front and/or back stencil state is updated. Three symbolic constants are valid: GL_FRONT, GL_BACK, and GL_FRONT_AND_BACK.
		/// </para>
		/// </param>
		/// <param name="func">
		/// <para>
		/// Specifies the test function. Eight symbolic constants are valid: GL_NEVER, GL_LESS, GL_LEQUAL, GL_GREATER, GL_GEQUAL, GL_EQUAL, GL_NOTEQUAL, and GL_ALWAYS. The initial value is GL_ALWAYS.
		/// </para>
		/// </param>
		/// <param name="ref">
		/// <para>
		/// Specifies the reference value for the stencil test. ref is clamped to the range [0, 2 sup n - 1], where is the number of bitplanes in the stencil buffer. The initial value is 0.
		/// </para>
		/// </param>
		/// <param name="mask">
		/// <para>
		/// Specifies a mask that is ANDed with both the reference value and the stored stencil value when the test is done. The initial value is all 1's.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glStencilFuncSeparate")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void StencilFuncSeparate(System.Graphics.ES20.CullFaceMode face, System.Graphics.ES20.StencilFunction func, Int32 @ref, UInt32 mask) {
			Delegates.glStencilFuncSeparate((System.Graphics.ES20.CullFaceMode) face, (System.Graphics.ES20.StencilFunction) func, (Int32) @ref, (UInt32) mask);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Control the front and back writing of individual bits in the stencil planes
		/// </summary>
		/// <param name="mask">
		/// <para>
		/// Specifies a bit mask to enable and disable writing of individual bits in the stencil planes. Initially, the mask is all 1's.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glStencilMask")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void StencilMask(Int32 mask) {
			Delegates.glStencilMask((UInt32) mask);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Control the front and back writing of individual bits in the stencil planes
		/// </summary>
		/// <param name="mask">
		/// <para>
		/// Specifies a bit mask to enable and disable writing of individual bits in the stencil planes. Initially, the mask is all 1's.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glStencilMask")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void StencilMask(UInt32 mask) {
			Delegates.glStencilMask((UInt32) mask);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Control the front and/or back writing of individual bits in the stencil planes
		/// </summary>
		/// <param name="face">
		/// <para>
		/// Specifies whether the front and/or back stencil writemask is updated. Three symbolic constants are valid: GL_FRONT, GL_BACK, and GL_FRONT_AND_BACK.
		/// </para>
		/// </param>
		/// <param name="mask">
		/// <para>
		/// Specifies a bit mask to enable and disable writing of individual bits in the stencil planes. Initially, the mask is all 1's.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glStencilMaskSeparate")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void StencilMaskSeparate(System.Graphics.ES20.CullFaceMode face, Int32 mask) {
			Delegates.glStencilMaskSeparate((System.Graphics.ES20.CullFaceMode) face, (UInt32) mask);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Control the front and/or back writing of individual bits in the stencil planes
		/// </summary>
		/// <param name="face">
		/// <para>
		/// Specifies whether the front and/or back stencil writemask is updated. Three symbolic constants are valid: GL_FRONT, GL_BACK, and GL_FRONT_AND_BACK.
		/// </para>
		/// </param>
		/// <param name="mask">
		/// <para>
		/// Specifies a bit mask to enable and disable writing of individual bits in the stencil planes. Initially, the mask is all 1's.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glStencilMaskSeparate")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void StencilMaskSeparate(System.Graphics.ES20.CullFaceMode face, UInt32 mask) {
			Delegates.glStencilMaskSeparate((System.Graphics.ES20.CullFaceMode) face, (UInt32) mask);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Set front and back stencil test actions
		/// </summary>
		/// <param name="sfail">
		/// <para>
		/// Specifies the action to take when the stencil test fails. Eight symbolic constants are accepted: GL_KEEP, GL_ZERO, GL_REPLACE, GL_INCR, GL_INCR_WRAP, GL_DECR, GL_DECR_WRAP, and GL_INVERT. The initial value is GL_KEEP.
		/// </para>
		/// </param>
		/// <param name="dpfail">
		/// <para>
		/// Specifies the stencil action when the stencil test passes, but the depth test fails. dpfail accepts the same symbolic constants as sfail. The initial value is GL_KEEP.
		/// </para>
		/// </param>
		/// <param name="dppass">
		/// <para>
		/// Specifies the stencil action when both the stencil test and the depth test pass, or when the stencil test passes and either there is no depth buffer or depth testing is not enabled. dppass accepts the same symbolic constants as sfail. The initial value is GL_KEEP.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glStencilOp")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void StencilOp(System.Graphics.ES20.StencilOp fail, System.Graphics.ES20.StencilOp zfail, System.Graphics.ES20.StencilOp zpass) {
			Delegates.glStencilOp((System.Graphics.ES20.StencilOp) fail, (System.Graphics.ES20.StencilOp) zfail, (System.Graphics.ES20.StencilOp) zpass);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Set front and/or back stencil test actions
		/// </summary>
		/// <param name="face">
		/// <para>
		/// Specifies whether front and/or back stencil state is updated. Three symbolic constants are valid: GL_FRONT, GL_BACK, and GL_FRONT_AND_BACK.
		/// </para>
		/// </param>
		/// <param name="sfail">
		/// <para>
		/// Specifies the action to take when the stencil test fails. Eight symbolic constants are accepted: GL_KEEP, GL_ZERO, GL_REPLACE, GL_INCR, GL_INCR_WRAP, GL_DECR, GL_DECR_WRAP, and GL_INVERT. The initial value is GL_KEEP.
		/// </para>
		/// </param>
		/// <param name="dpfail">
		/// <para>
		/// Specifies the stencil action when the stencil test passes, but the depth test fails. dpfail accepts the same symbolic constants as sfail. The initial value is GL_KEEP.
		/// </para>
		/// </param>
		/// <param name="dppass">
		/// <para>
		/// Specifies the stencil action when both the stencil test and the depth test pass, or when the stencil test passes and either there is no depth buffer or depth testing is not enabled. dppass accepts the same symbolic constants as sfail. The initial value is GL_KEEP.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glStencilOpSeparate")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void StencilOpSeparate(System.Graphics.ES20.CullFaceMode face, System.Graphics.ES20.StencilOp fail, System.Graphics.ES20.StencilOp zfail, System.Graphics.ES20.StencilOp zpass) {
			Delegates.glStencilOpSeparate((System.Graphics.ES20.CullFaceMode) face, (System.Graphics.ES20.StencilOp) fail, (System.Graphics.ES20.StencilOp) zfail, (System.Graphics.ES20.StencilOp) zpass);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify a two-dimensional texture image
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target texture. Must be GL_TEXTURE_2D, GL_PROXY_TEXTURE_2D, GL_TEXTURE_1D_ARRAY, GL_PROXY_TEXTURE_1D_ARRAY, GL_TEXTURE_RECTANGLE, GL_PROXY_TEXTURE_RECTANGLE, GL_TEXTURE_CUBE_MAP_POSITIVE_X, GL_TEXTURE_CUBE_MAP_NEGATIVE_X, GL_TEXTURE_CUBE_MAP_POSITIVE_Y, GL_TEXTURE_CUBE_MAP_NEGATIVE_Y, GL_TEXTURE_CUBE_MAP_POSITIVE_Z, GL_TEXTURE_CUBE_MAP_NEGATIVE_Z, or GL_PROXY_TEXTURE_CUBE_MAP.
		/// </para>
		/// </param>
		/// <param name="level">
		/// <para>
		/// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap reduction image. If target is GL_TEXTURE_RECTANGLE or GL_PROXY_TEXTURE_RECTANGLE, level must be 0.
		/// </para>
		/// </param>
		/// <param name="internalFormat">
		/// <para>
		/// Specifies the number of color components in the texture. Must be one of the following symbolic constants: GL_COMPRESSED_RED, GL_COMPRESSED_RG, GL_COMPRESSED_RGB, GL_COMPRESSED_RGBA, GL_COMPRESSED_SRGB, GL_COMPRESSED_SRGB_ALPHA, GL_DEPTH_COMPONENT, GL_DEPTH_COMPONENT16, GL_DEPTH_COMPONENT24, GL_DEPTH_COMPONENT32, GL_R3_G3_B2, GL_RED, GL_RG, GL_RGB, GL_RGB4, GL_RGB5, GL_RGB8, GL_RGB10, GL_RGB12, GL_RGB16, GL_RGBA, GL_RGBA2, GL_RGBA4, GL_RGB5_A1, GL_RGBA8, GL_RGB10_A2, GL_RGBA12, GL_RGBA16, GL_SRGB, GL_SRGB8, GL_SRGB_ALPHA, or GL_SRGB8_ALPHA8.
		/// </para>
		/// </param>
		/// <param name="width">
		/// <para>
		/// Specifies the width of the texture image. All implementations support texture images that are at least 1024 texels wide.
		/// </para>
		/// </param>
		/// <param name="height">
		/// <para>
		/// Specifies the height of the texture image, or the number of layers in a texture array, in the case of the GL_TEXTURE_1D_ARRAY and GL_PROXY_TEXTURE_1D_ARRAY targets. All implementations support 2D texture images that are at least 1024 texels high, and texture arrays that are at least 256 layers deep.
		/// </para>
		/// </param>
		/// <param name="border">
		/// <para>
		/// This value must be 0.
		/// </para>
		/// </param>
		/// <param name="format">
		/// <para>
		/// Specifies the format of the pixel data. The following symbolic values are accepted: GL_RED, GL_RG, GL_RGB, GL_BGR, GL_RGBA, and GL_BGRA.
		/// </para>
		/// </param>
		/// <param name="type">
		/// <para>
		/// Specifies the data type of the pixel data. The following symbolic values are accepted: GL_UNSIGNED_BYTE, GL_BYTE, GL_UNSIGNED_SHORT, GL_SHORT, GL_UNSIGNED_INT, GL_INT, GL_FLOAT, GL_UNSIGNED_BYTE_3_3_2, GL_UNSIGNED_BYTE_2_3_3_REV, GL_UNSIGNED_SHORT_5_6_5, GL_UNSIGNED_SHORT_5_6_5_REV, GL_UNSIGNED_SHORT_4_4_4_4, GL_UNSIGNED_SHORT_4_4_4_4_REV, GL_UNSIGNED_SHORT_5_5_5_1, GL_UNSIGNED_SHORT_1_5_5_5_REV, GL_UNSIGNED_INT_8_8_8_8, GL_UNSIGNED_INT_8_8_8_8_REV, GL_UNSIGNED_INT_10_10_10_2, and GL_UNSIGNED_INT_2_10_10_10_REV.
		/// </para>
		/// </param>
		/// <param name="data">
		/// <para>
		/// Specifies a pointer to the image data in memory.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glTexImage2D")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void TexImage2D(System.Graphics.ES20.TextureTarget target, Int32 level, System.Graphics.ES20.PixelInternalFormat internalformat, Int32 width, Int32 height, Int32 border, System.Graphics.ES20.PixelFormat format, System.Graphics.ES20.PixelType type, IntPtr pixels) {
			Delegates.glTexImage2D((System.Graphics.ES20.TextureTarget) target, (Int32) level, (System.Graphics.ES20.PixelInternalFormat) internalformat, (Int32) width, (Int32) height, (Int32) border, (System.Graphics.ES20.PixelFormat) format, (System.Graphics.ES20.PixelType) type, (IntPtr) pixels);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify a two-dimensional texture image
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target texture. Must be GL_TEXTURE_2D, GL_PROXY_TEXTURE_2D, GL_TEXTURE_1D_ARRAY, GL_PROXY_TEXTURE_1D_ARRAY, GL_TEXTURE_RECTANGLE, GL_PROXY_TEXTURE_RECTANGLE, GL_TEXTURE_CUBE_MAP_POSITIVE_X, GL_TEXTURE_CUBE_MAP_NEGATIVE_X, GL_TEXTURE_CUBE_MAP_POSITIVE_Y, GL_TEXTURE_CUBE_MAP_NEGATIVE_Y, GL_TEXTURE_CUBE_MAP_POSITIVE_Z, GL_TEXTURE_CUBE_MAP_NEGATIVE_Z, or GL_PROXY_TEXTURE_CUBE_MAP.
		/// </para>
		/// </param>
		/// <param name="level">
		/// <para>
		/// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap reduction image. If target is GL_TEXTURE_RECTANGLE or GL_PROXY_TEXTURE_RECTANGLE, level must be 0.
		/// </para>
		/// </param>
		/// <param name="internalFormat">
		/// <para>
		/// Specifies the number of color components in the texture. Must be one of the following symbolic constants: GL_COMPRESSED_RED, GL_COMPRESSED_RG, GL_COMPRESSED_RGB, GL_COMPRESSED_RGBA, GL_COMPRESSED_SRGB, GL_COMPRESSED_SRGB_ALPHA, GL_DEPTH_COMPONENT, GL_DEPTH_COMPONENT16, GL_DEPTH_COMPONENT24, GL_DEPTH_COMPONENT32, GL_R3_G3_B2, GL_RED, GL_RG, GL_RGB, GL_RGB4, GL_RGB5, GL_RGB8, GL_RGB10, GL_RGB12, GL_RGB16, GL_RGBA, GL_RGBA2, GL_RGBA4, GL_RGB5_A1, GL_RGBA8, GL_RGB10_A2, GL_RGBA12, GL_RGBA16, GL_SRGB, GL_SRGB8, GL_SRGB_ALPHA, or GL_SRGB8_ALPHA8.
		/// </para>
		/// </param>
		/// <param name="width">
		/// <para>
		/// Specifies the width of the texture image. All implementations support texture images that are at least 1024 texels wide.
		/// </para>
		/// </param>
		/// <param name="height">
		/// <para>
		/// Specifies the height of the texture image, or the number of layers in a texture array, in the case of the GL_TEXTURE_1D_ARRAY and GL_PROXY_TEXTURE_1D_ARRAY targets. All implementations support 2D texture images that are at least 1024 texels high, and texture arrays that are at least 256 layers deep.
		/// </para>
		/// </param>
		/// <param name="border">
		/// <para>
		/// This value must be 0.
		/// </para>
		/// </param>
		/// <param name="format">
		/// <para>
		/// Specifies the format of the pixel data. The following symbolic values are accepted: GL_RED, GL_RG, GL_RGB, GL_BGR, GL_RGBA, and GL_BGRA.
		/// </para>
		/// </param>
		/// <param name="type">
		/// <para>
		/// Specifies the data type of the pixel data. The following symbolic values are accepted: GL_UNSIGNED_BYTE, GL_BYTE, GL_UNSIGNED_SHORT, GL_SHORT, GL_UNSIGNED_INT, GL_INT, GL_FLOAT, GL_UNSIGNED_BYTE_3_3_2, GL_UNSIGNED_BYTE_2_3_3_REV, GL_UNSIGNED_SHORT_5_6_5, GL_UNSIGNED_SHORT_5_6_5_REV, GL_UNSIGNED_SHORT_4_4_4_4, GL_UNSIGNED_SHORT_4_4_4_4_REV, GL_UNSIGNED_SHORT_5_5_5_1, GL_UNSIGNED_SHORT_1_5_5_5_REV, GL_UNSIGNED_INT_8_8_8_8, GL_UNSIGNED_INT_8_8_8_8_REV, GL_UNSIGNED_INT_10_10_10_2, and GL_UNSIGNED_INT_2_10_10_10_REV.
		/// </para>
		/// </param>
		/// <param name="data">
		/// <para>
		/// Specifies a pointer to the image data in memory.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glTexImage2D")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void TexImage2D<T8>(System.Graphics.ES20.TextureTarget target, Int32 level, System.Graphics.ES20.PixelInternalFormat internalformat, Int32 width, Int32 height, Int32 border, System.Graphics.ES20.PixelFormat format, System.Graphics.ES20.PixelType type, [InAttribute, OutAttribute] T8[] pixels)
					where T8 : struct {
			GCHandle pixels_ptr = GCHandle.Alloc(pixels, GCHandleType.Pinned);
			try {
				Delegates.glTexImage2D((System.Graphics.ES20.TextureTarget) target, (Int32) level, (System.Graphics.ES20.PixelInternalFormat) internalformat, (Int32) width, (Int32) height, (Int32) border, (System.Graphics.ES20.PixelFormat) format, (System.Graphics.ES20.PixelType) type, (IntPtr) pixels_ptr.AddrOfPinnedObject());
			} finally {
				pixels_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify a two-dimensional texture image
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target texture. Must be GL_TEXTURE_2D, GL_PROXY_TEXTURE_2D, GL_TEXTURE_1D_ARRAY, GL_PROXY_TEXTURE_1D_ARRAY, GL_TEXTURE_RECTANGLE, GL_PROXY_TEXTURE_RECTANGLE, GL_TEXTURE_CUBE_MAP_POSITIVE_X, GL_TEXTURE_CUBE_MAP_NEGATIVE_X, GL_TEXTURE_CUBE_MAP_POSITIVE_Y, GL_TEXTURE_CUBE_MAP_NEGATIVE_Y, GL_TEXTURE_CUBE_MAP_POSITIVE_Z, GL_TEXTURE_CUBE_MAP_NEGATIVE_Z, or GL_PROXY_TEXTURE_CUBE_MAP.
		/// </para>
		/// </param>
		/// <param name="level">
		/// <para>
		/// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap reduction image. If target is GL_TEXTURE_RECTANGLE or GL_PROXY_TEXTURE_RECTANGLE, level must be 0.
		/// </para>
		/// </param>
		/// <param name="internalFormat">
		/// <para>
		/// Specifies the number of color components in the texture. Must be one of the following symbolic constants: GL_COMPRESSED_RED, GL_COMPRESSED_RG, GL_COMPRESSED_RGB, GL_COMPRESSED_RGBA, GL_COMPRESSED_SRGB, GL_COMPRESSED_SRGB_ALPHA, GL_DEPTH_COMPONENT, GL_DEPTH_COMPONENT16, GL_DEPTH_COMPONENT24, GL_DEPTH_COMPONENT32, GL_R3_G3_B2, GL_RED, GL_RG, GL_RGB, GL_RGB4, GL_RGB5, GL_RGB8, GL_RGB10, GL_RGB12, GL_RGB16, GL_RGBA, GL_RGBA2, GL_RGBA4, GL_RGB5_A1, GL_RGBA8, GL_RGB10_A2, GL_RGBA12, GL_RGBA16, GL_SRGB, GL_SRGB8, GL_SRGB_ALPHA, or GL_SRGB8_ALPHA8.
		/// </para>
		/// </param>
		/// <param name="width">
		/// <para>
		/// Specifies the width of the texture image. All implementations support texture images that are at least 1024 texels wide.
		/// </para>
		/// </param>
		/// <param name="height">
		/// <para>
		/// Specifies the height of the texture image, or the number of layers in a texture array, in the case of the GL_TEXTURE_1D_ARRAY and GL_PROXY_TEXTURE_1D_ARRAY targets. All implementations support 2D texture images that are at least 1024 texels high, and texture arrays that are at least 256 layers deep.
		/// </para>
		/// </param>
		/// <param name="border">
		/// <para>
		/// This value must be 0.
		/// </para>
		/// </param>
		/// <param name="format">
		/// <para>
		/// Specifies the format of the pixel data. The following symbolic values are accepted: GL_RED, GL_RG, GL_RGB, GL_BGR, GL_RGBA, and GL_BGRA.
		/// </para>
		/// </param>
		/// <param name="type">
		/// <para>
		/// Specifies the data type of the pixel data. The following symbolic values are accepted: GL_UNSIGNED_BYTE, GL_BYTE, GL_UNSIGNED_SHORT, GL_SHORT, GL_UNSIGNED_INT, GL_INT, GL_FLOAT, GL_UNSIGNED_BYTE_3_3_2, GL_UNSIGNED_BYTE_2_3_3_REV, GL_UNSIGNED_SHORT_5_6_5, GL_UNSIGNED_SHORT_5_6_5_REV, GL_UNSIGNED_SHORT_4_4_4_4, GL_UNSIGNED_SHORT_4_4_4_4_REV, GL_UNSIGNED_SHORT_5_5_5_1, GL_UNSIGNED_SHORT_1_5_5_5_REV, GL_UNSIGNED_INT_8_8_8_8, GL_UNSIGNED_INT_8_8_8_8_REV, GL_UNSIGNED_INT_10_10_10_2, and GL_UNSIGNED_INT_2_10_10_10_REV.
		/// </para>
		/// </param>
		/// <param name="data">
		/// <para>
		/// Specifies a pointer to the image data in memory.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glTexImage2D")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void TexImage2D<T8>(System.Graphics.ES20.TextureTarget target, Int32 level, System.Graphics.ES20.PixelInternalFormat internalformat, Int32 width, Int32 height, Int32 border, System.Graphics.ES20.PixelFormat format, System.Graphics.ES20.PixelType type, [InAttribute, OutAttribute] T8[,] pixels)
					where T8 : struct {
			GCHandle pixels_ptr = GCHandle.Alloc(pixels, GCHandleType.Pinned);
			try {
				Delegates.glTexImage2D((System.Graphics.ES20.TextureTarget) target, (Int32) level, (System.Graphics.ES20.PixelInternalFormat) internalformat, (Int32) width, (Int32) height, (Int32) border, (System.Graphics.ES20.PixelFormat) format, (System.Graphics.ES20.PixelType) type, (IntPtr) pixels_ptr.AddrOfPinnedObject());
			} finally {
				pixels_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify a two-dimensional texture image
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target texture. Must be GL_TEXTURE_2D, GL_PROXY_TEXTURE_2D, GL_TEXTURE_1D_ARRAY, GL_PROXY_TEXTURE_1D_ARRAY, GL_TEXTURE_RECTANGLE, GL_PROXY_TEXTURE_RECTANGLE, GL_TEXTURE_CUBE_MAP_POSITIVE_X, GL_TEXTURE_CUBE_MAP_NEGATIVE_X, GL_TEXTURE_CUBE_MAP_POSITIVE_Y, GL_TEXTURE_CUBE_MAP_NEGATIVE_Y, GL_TEXTURE_CUBE_MAP_POSITIVE_Z, GL_TEXTURE_CUBE_MAP_NEGATIVE_Z, or GL_PROXY_TEXTURE_CUBE_MAP.
		/// </para>
		/// </param>
		/// <param name="level">
		/// <para>
		/// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap reduction image. If target is GL_TEXTURE_RECTANGLE or GL_PROXY_TEXTURE_RECTANGLE, level must be 0.
		/// </para>
		/// </param>
		/// <param name="internalFormat">
		/// <para>
		/// Specifies the number of color components in the texture. Must be one of the following symbolic constants: GL_COMPRESSED_RED, GL_COMPRESSED_RG, GL_COMPRESSED_RGB, GL_COMPRESSED_RGBA, GL_COMPRESSED_SRGB, GL_COMPRESSED_SRGB_ALPHA, GL_DEPTH_COMPONENT, GL_DEPTH_COMPONENT16, GL_DEPTH_COMPONENT24, GL_DEPTH_COMPONENT32, GL_R3_G3_B2, GL_RED, GL_RG, GL_RGB, GL_RGB4, GL_RGB5, GL_RGB8, GL_RGB10, GL_RGB12, GL_RGB16, GL_RGBA, GL_RGBA2, GL_RGBA4, GL_RGB5_A1, GL_RGBA8, GL_RGB10_A2, GL_RGBA12, GL_RGBA16, GL_SRGB, GL_SRGB8, GL_SRGB_ALPHA, or GL_SRGB8_ALPHA8.
		/// </para>
		/// </param>
		/// <param name="width">
		/// <para>
		/// Specifies the width of the texture image. All implementations support texture images that are at least 1024 texels wide.
		/// </para>
		/// </param>
		/// <param name="height">
		/// <para>
		/// Specifies the height of the texture image, or the number of layers in a texture array, in the case of the GL_TEXTURE_1D_ARRAY and GL_PROXY_TEXTURE_1D_ARRAY targets. All implementations support 2D texture images that are at least 1024 texels high, and texture arrays that are at least 256 layers deep.
		/// </para>
		/// </param>
		/// <param name="border">
		/// <para>
		/// This value must be 0.
		/// </para>
		/// </param>
		/// <param name="format">
		/// <para>
		/// Specifies the format of the pixel data. The following symbolic values are accepted: GL_RED, GL_RG, GL_RGB, GL_BGR, GL_RGBA, and GL_BGRA.
		/// </para>
		/// </param>
		/// <param name="type">
		/// <para>
		/// Specifies the data type of the pixel data. The following symbolic values are accepted: GL_UNSIGNED_BYTE, GL_BYTE, GL_UNSIGNED_SHORT, GL_SHORT, GL_UNSIGNED_INT, GL_INT, GL_FLOAT, GL_UNSIGNED_BYTE_3_3_2, GL_UNSIGNED_BYTE_2_3_3_REV, GL_UNSIGNED_SHORT_5_6_5, GL_UNSIGNED_SHORT_5_6_5_REV, GL_UNSIGNED_SHORT_4_4_4_4, GL_UNSIGNED_SHORT_4_4_4_4_REV, GL_UNSIGNED_SHORT_5_5_5_1, GL_UNSIGNED_SHORT_1_5_5_5_REV, GL_UNSIGNED_INT_8_8_8_8, GL_UNSIGNED_INT_8_8_8_8_REV, GL_UNSIGNED_INT_10_10_10_2, and GL_UNSIGNED_INT_2_10_10_10_REV.
		/// </para>
		/// </param>
		/// <param name="data">
		/// <para>
		/// Specifies a pointer to the image data in memory.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glTexImage2D")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void TexImage2D<T8>(System.Graphics.ES20.TextureTarget target, Int32 level, System.Graphics.ES20.PixelInternalFormat internalformat, Int32 width, Int32 height, Int32 border, System.Graphics.ES20.PixelFormat format, System.Graphics.ES20.PixelType type, [InAttribute, OutAttribute] T8[,,] pixels)
					where T8 : struct {
			GCHandle pixels_ptr = GCHandle.Alloc(pixels, GCHandleType.Pinned);
			try {
				Delegates.glTexImage2D((System.Graphics.ES20.TextureTarget) target, (Int32) level, (System.Graphics.ES20.PixelInternalFormat) internalformat, (Int32) width, (Int32) height, (Int32) border, (System.Graphics.ES20.PixelFormat) format, (System.Graphics.ES20.PixelType) type, (IntPtr) pixels_ptr.AddrOfPinnedObject());
			} finally {
				pixels_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify a two-dimensional texture image
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target texture. Must be GL_TEXTURE_2D, GL_PROXY_TEXTURE_2D, GL_TEXTURE_1D_ARRAY, GL_PROXY_TEXTURE_1D_ARRAY, GL_TEXTURE_RECTANGLE, GL_PROXY_TEXTURE_RECTANGLE, GL_TEXTURE_CUBE_MAP_POSITIVE_X, GL_TEXTURE_CUBE_MAP_NEGATIVE_X, GL_TEXTURE_CUBE_MAP_POSITIVE_Y, GL_TEXTURE_CUBE_MAP_NEGATIVE_Y, GL_TEXTURE_CUBE_MAP_POSITIVE_Z, GL_TEXTURE_CUBE_MAP_NEGATIVE_Z, or GL_PROXY_TEXTURE_CUBE_MAP.
		/// </para>
		/// </param>
		/// <param name="level">
		/// <para>
		/// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap reduction image. If target is GL_TEXTURE_RECTANGLE or GL_PROXY_TEXTURE_RECTANGLE, level must be 0.
		/// </para>
		/// </param>
		/// <param name="internalFormat">
		/// <para>
		/// Specifies the number of color components in the texture. Must be one of the following symbolic constants: GL_COMPRESSED_RED, GL_COMPRESSED_RG, GL_COMPRESSED_RGB, GL_COMPRESSED_RGBA, GL_COMPRESSED_SRGB, GL_COMPRESSED_SRGB_ALPHA, GL_DEPTH_COMPONENT, GL_DEPTH_COMPONENT16, GL_DEPTH_COMPONENT24, GL_DEPTH_COMPONENT32, GL_R3_G3_B2, GL_RED, GL_RG, GL_RGB, GL_RGB4, GL_RGB5, GL_RGB8, GL_RGB10, GL_RGB12, GL_RGB16, GL_RGBA, GL_RGBA2, GL_RGBA4, GL_RGB5_A1, GL_RGBA8, GL_RGB10_A2, GL_RGBA12, GL_RGBA16, GL_SRGB, GL_SRGB8, GL_SRGB_ALPHA, or GL_SRGB8_ALPHA8.
		/// </para>
		/// </param>
		/// <param name="width">
		/// <para>
		/// Specifies the width of the texture image. All implementations support texture images that are at least 1024 texels wide.
		/// </para>
		/// </param>
		/// <param name="height">
		/// <para>
		/// Specifies the height of the texture image, or the number of layers in a texture array, in the case of the GL_TEXTURE_1D_ARRAY and GL_PROXY_TEXTURE_1D_ARRAY targets. All implementations support 2D texture images that are at least 1024 texels high, and texture arrays that are at least 256 layers deep.
		/// </para>
		/// </param>
		/// <param name="border">
		/// <para>
		/// This value must be 0.
		/// </para>
		/// </param>
		/// <param name="format">
		/// <para>
		/// Specifies the format of the pixel data. The following symbolic values are accepted: GL_RED, GL_RG, GL_RGB, GL_BGR, GL_RGBA, and GL_BGRA.
		/// </para>
		/// </param>
		/// <param name="type">
		/// <para>
		/// Specifies the data type of the pixel data. The following symbolic values are accepted: GL_UNSIGNED_BYTE, GL_BYTE, GL_UNSIGNED_SHORT, GL_SHORT, GL_UNSIGNED_INT, GL_INT, GL_FLOAT, GL_UNSIGNED_BYTE_3_3_2, GL_UNSIGNED_BYTE_2_3_3_REV, GL_UNSIGNED_SHORT_5_6_5, GL_UNSIGNED_SHORT_5_6_5_REV, GL_UNSIGNED_SHORT_4_4_4_4, GL_UNSIGNED_SHORT_4_4_4_4_REV, GL_UNSIGNED_SHORT_5_5_5_1, GL_UNSIGNED_SHORT_1_5_5_5_REV, GL_UNSIGNED_INT_8_8_8_8, GL_UNSIGNED_INT_8_8_8_8_REV, GL_UNSIGNED_INT_10_10_10_2, and GL_UNSIGNED_INT_2_10_10_10_REV.
		/// </para>
		/// </param>
		/// <param name="data">
		/// <para>
		/// Specifies a pointer to the image data in memory.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glTexImage2D")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void TexImage2D<T8>(System.Graphics.ES20.TextureTarget target, Int32 level, System.Graphics.ES20.PixelInternalFormat internalformat, Int32 width, Int32 height, Int32 border, System.Graphics.ES20.PixelFormat format, System.Graphics.ES20.PixelType type, [InAttribute, OutAttribute] ref T8 pixels)
					where T8 : struct {
			GCHandle pixels_ptr = GCHandle.Alloc(pixels, GCHandleType.Pinned);
			try {
				Delegates.glTexImage2D((System.Graphics.ES20.TextureTarget) target, (Int32) level, (System.Graphics.ES20.PixelInternalFormat) internalformat, (Int32) width, (Int32) height, (Int32) border, (System.Graphics.ES20.PixelFormat) format, (System.Graphics.ES20.PixelType) type, (IntPtr) pixels_ptr.AddrOfPinnedObject());
				pixels = (T8) pixels_ptr.Target;
			} finally {
				pixels_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Set texture parameters
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target texture, which must be either GL_TEXTURE_1D, GL_TEXTURE_2D, GL_TEXTURE_3D, GL_TEXTURE_1D_ARRAY, GL_TEXTURE_2D_ARRAY, GL_TEXTURE_RECTANGLE, or GL_TEXTURE_CUBE_MAP.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the symbolic name of a single-valued texture parameter. pname can be one of the following: GL_TEXTURE_BASE_LEVEL, GL_TEXTURE_COMPARE_FUNC, GL_TEXTURE_COMPARE_MODE, GL_TEXTURE_LOD_BIAS, GL_TEXTURE_MIN_FILTER, GL_TEXTURE_MAG_FILTER, GL_TEXTURE_MIN_LOD, GL_TEXTURE_MAX_LOD, GL_TEXTURE_MAX_LEVEL, GL_TEXTURE_SWIZZLE_R, GL_TEXTURE_SWIZZLE_G, GL_TEXTURE_SWIZZLE_B, GL_TEXTURE_SWIZZLE_A, GL_TEXTURE_WRAP_S, GL_TEXTURE_WRAP_T, or GL_TEXTURE_WRAP_R.
		/// </para>
		/// </param>
		/// <param name="param">
		/// <para>
		/// Specifies the value of pname.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glTexParameterf")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void TexParameter(System.Graphics.ES20.TextureTarget target, System.Graphics.ES20.TextureParameterName pname, Single param) {
			Delegates.glTexParameterf((System.Graphics.ES20.TextureTarget) target, (System.Graphics.ES20.TextureParameterName) pname, (Single) param);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Set texture parameters
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target texture, which must be either GL_TEXTURE_1D, GL_TEXTURE_2D, GL_TEXTURE_3D, GL_TEXTURE_1D_ARRAY, GL_TEXTURE_2D_ARRAY, GL_TEXTURE_RECTANGLE, or GL_TEXTURE_CUBE_MAP.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the symbolic name of a single-valued texture parameter. pname can be one of the following: GL_TEXTURE_BASE_LEVEL, GL_TEXTURE_COMPARE_FUNC, GL_TEXTURE_COMPARE_MODE, GL_TEXTURE_LOD_BIAS, GL_TEXTURE_MIN_FILTER, GL_TEXTURE_MAG_FILTER, GL_TEXTURE_MIN_LOD, GL_TEXTURE_MAX_LOD, GL_TEXTURE_MAX_LEVEL, GL_TEXTURE_SWIZZLE_R, GL_TEXTURE_SWIZZLE_G, GL_TEXTURE_SWIZZLE_B, GL_TEXTURE_SWIZZLE_A, GL_TEXTURE_WRAP_S, GL_TEXTURE_WRAP_T, or GL_TEXTURE_WRAP_R.
		/// </para>
		/// </param>
		/// <param name="param">
		/// <para>
		/// Specifies the value of pname.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glTexParameterfv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void TexParameter(System.Graphics.ES20.TextureTarget target, System.Graphics.ES20.TextureParameterName pname, Single[] @params) {
			unsafe
			{
				fixed (Single* @params_ptr = @params) {
					Delegates.glTexParameterfv((System.Graphics.ES20.TextureTarget) target, (System.Graphics.ES20.TextureParameterName) pname, (Single*) @params_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Set texture parameters
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target texture, which must be either GL_TEXTURE_1D, GL_TEXTURE_2D, GL_TEXTURE_3D, GL_TEXTURE_1D_ARRAY, GL_TEXTURE_2D_ARRAY, GL_TEXTURE_RECTANGLE, or GL_TEXTURE_CUBE_MAP.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the symbolic name of a single-valued texture parameter. pname can be one of the following: GL_TEXTURE_BASE_LEVEL, GL_TEXTURE_COMPARE_FUNC, GL_TEXTURE_COMPARE_MODE, GL_TEXTURE_LOD_BIAS, GL_TEXTURE_MIN_FILTER, GL_TEXTURE_MAG_FILTER, GL_TEXTURE_MIN_LOD, GL_TEXTURE_MAX_LOD, GL_TEXTURE_MAX_LEVEL, GL_TEXTURE_SWIZZLE_R, GL_TEXTURE_SWIZZLE_G, GL_TEXTURE_SWIZZLE_B, GL_TEXTURE_SWIZZLE_A, GL_TEXTURE_WRAP_S, GL_TEXTURE_WRAP_T, or GL_TEXTURE_WRAP_R.
		/// </para>
		/// </param>
		/// <param name="param">
		/// <para>
		/// Specifies the value of pname.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glTexParameterfv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void TexParameter(System.Graphics.ES20.TextureTarget target, System.Graphics.ES20.TextureParameterName pname, Single* @params) {
			Delegates.glTexParameterfv((System.Graphics.ES20.TextureTarget) target, (System.Graphics.ES20.TextureParameterName) pname, (Single*) @params);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Set texture parameters
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target texture, which must be either GL_TEXTURE_1D, GL_TEXTURE_2D, GL_TEXTURE_3D, GL_TEXTURE_1D_ARRAY, GL_TEXTURE_2D_ARRAY, GL_TEXTURE_RECTANGLE, or GL_TEXTURE_CUBE_MAP.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the symbolic name of a single-valued texture parameter. pname can be one of the following: GL_TEXTURE_BASE_LEVEL, GL_TEXTURE_COMPARE_FUNC, GL_TEXTURE_COMPARE_MODE, GL_TEXTURE_LOD_BIAS, GL_TEXTURE_MIN_FILTER, GL_TEXTURE_MAG_FILTER, GL_TEXTURE_MIN_LOD, GL_TEXTURE_MAX_LOD, GL_TEXTURE_MAX_LEVEL, GL_TEXTURE_SWIZZLE_R, GL_TEXTURE_SWIZZLE_G, GL_TEXTURE_SWIZZLE_B, GL_TEXTURE_SWIZZLE_A, GL_TEXTURE_WRAP_S, GL_TEXTURE_WRAP_T, or GL_TEXTURE_WRAP_R.
		/// </para>
		/// </param>
		/// <param name="param">
		/// <para>
		/// Specifies the value of pname.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glTexParameteri")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void TexParameter(System.Graphics.ES20.TextureTarget target, System.Graphics.ES20.TextureParameterName pname, Int32 param) {
			Delegates.glTexParameteri((System.Graphics.ES20.TextureTarget) target, (System.Graphics.ES20.TextureParameterName) pname, (Int32) param);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Set texture parameters
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target texture, which must be either GL_TEXTURE_1D, GL_TEXTURE_2D, GL_TEXTURE_3D, GL_TEXTURE_1D_ARRAY, GL_TEXTURE_2D_ARRAY, GL_TEXTURE_RECTANGLE, or GL_TEXTURE_CUBE_MAP.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the symbolic name of a single-valued texture parameter. pname can be one of the following: GL_TEXTURE_BASE_LEVEL, GL_TEXTURE_COMPARE_FUNC, GL_TEXTURE_COMPARE_MODE, GL_TEXTURE_LOD_BIAS, GL_TEXTURE_MIN_FILTER, GL_TEXTURE_MAG_FILTER, GL_TEXTURE_MIN_LOD, GL_TEXTURE_MAX_LOD, GL_TEXTURE_MAX_LEVEL, GL_TEXTURE_SWIZZLE_R, GL_TEXTURE_SWIZZLE_G, GL_TEXTURE_SWIZZLE_B, GL_TEXTURE_SWIZZLE_A, GL_TEXTURE_WRAP_S, GL_TEXTURE_WRAP_T, or GL_TEXTURE_WRAP_R.
		/// </para>
		/// </param>
		/// <param name="param">
		/// <para>
		/// Specifies the value of pname.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glTexParameteriv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void TexParameter(System.Graphics.ES20.TextureTarget target, System.Graphics.ES20.TextureParameterName pname, Int32[] @params) {
			unsafe
			{
				fixed (Int32* @params_ptr = @params) {
					Delegates.glTexParameteriv((System.Graphics.ES20.TextureTarget) target, (System.Graphics.ES20.TextureParameterName) pname, (Int32*) @params_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Set texture parameters
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target texture, which must be either GL_TEXTURE_1D, GL_TEXTURE_2D, GL_TEXTURE_3D, GL_TEXTURE_1D_ARRAY, GL_TEXTURE_2D_ARRAY, GL_TEXTURE_RECTANGLE, or GL_TEXTURE_CUBE_MAP.
		/// </para>
		/// </param>
		/// <param name="pname">
		/// <para>
		/// Specifies the symbolic name of a single-valued texture parameter. pname can be one of the following: GL_TEXTURE_BASE_LEVEL, GL_TEXTURE_COMPARE_FUNC, GL_TEXTURE_COMPARE_MODE, GL_TEXTURE_LOD_BIAS, GL_TEXTURE_MIN_FILTER, GL_TEXTURE_MAG_FILTER, GL_TEXTURE_MIN_LOD, GL_TEXTURE_MAX_LOD, GL_TEXTURE_MAX_LEVEL, GL_TEXTURE_SWIZZLE_R, GL_TEXTURE_SWIZZLE_G, GL_TEXTURE_SWIZZLE_B, GL_TEXTURE_SWIZZLE_A, GL_TEXTURE_WRAP_S, GL_TEXTURE_WRAP_T, or GL_TEXTURE_WRAP_R.
		/// </para>
		/// </param>
		/// <param name="param">
		/// <para>
		/// Specifies the value of pname.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glTexParameteriv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void TexParameter(System.Graphics.ES20.TextureTarget target, System.Graphics.ES20.TextureParameterName pname, Int32* @params) {
			Delegates.glTexParameteriv((System.Graphics.ES20.TextureTarget) target, (System.Graphics.ES20.TextureParameterName) pname, (Int32*) @params);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify a two-dimensional texture subimage
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target texture. Must be GL_TEXTURE_2D, GL_TEXTURE_CUBE_MAP_POSITIVE_X, GL_TEXTURE_CUBE_MAP_NEGATIVE_X, GL_TEXTURE_CUBE_MAP_POSITIVE_Y, GL_TEXTURE_CUBE_MAP_NEGATIVE_Y, GL_TEXTURE_CUBE_MAP_POSITIVE_Z, or GL_TEXTURE_CUBE_MAP_NEGATIVE_Z.
		/// </para>
		/// </param>
		/// <param name="level">
		/// <para>
		/// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap reduction image.
		/// </para>
		/// </param>
		/// <param name="xoffset">
		/// <para>
		/// Specifies a texel offset in the x direction within the texture array.
		/// </para>
		/// </param>
		/// <param name="yoffset">
		/// <para>
		/// Specifies a texel offset in the y direction within the texture array.
		/// </para>
		/// </param>
		/// <param name="width">
		/// <para>
		/// Specifies the width of the texture subimage.
		/// </para>
		/// </param>
		/// <param name="height">
		/// <para>
		/// Specifies the height of the texture subimage.
		/// </para>
		/// </param>
		/// <param name="format">
		/// <para>
		/// Specifies the format of the pixel data. The following symbolic values are accepted: GL_RED, GL_RG, GL_RGB, GL_BGR, GL_RGBA, and GL_BGRA.
		/// </para>
		/// </param>
		/// <param name="type">
		/// <para>
		/// Specifies the data type of the pixel data. The following symbolic values are accepted: GL_UNSIGNED_BYTE, GL_BYTE, GL_UNSIGNED_SHORT, GL_SHORT, GL_UNSIGNED_INT, GL_INT, GL_FLOAT, GL_UNSIGNED_BYTE_3_3_2, GL_UNSIGNED_BYTE_2_3_3_REV, GL_UNSIGNED_SHORT_5_6_5, GL_UNSIGNED_SHORT_5_6_5_REV, GL_UNSIGNED_SHORT_4_4_4_4, GL_UNSIGNED_SHORT_4_4_4_4_REV, GL_UNSIGNED_SHORT_5_5_5_1, GL_UNSIGNED_SHORT_1_5_5_5_REV, GL_UNSIGNED_INT_8_8_8_8, GL_UNSIGNED_INT_8_8_8_8_REV, GL_UNSIGNED_INT_10_10_10_2, and GL_UNSIGNED_INT_2_10_10_10_REV.
		/// </para>
		/// </param>
		/// <param name="data">
		/// <para>
		/// Specifies a pointer to the image data in memory.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glTexSubImage2D")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void TexSubImage2D(System.Graphics.ES20.TextureTarget target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 width, Int32 height, System.Graphics.ES20.PixelFormat format, System.Graphics.ES20.PixelType type, IntPtr pixels) {
			Delegates.glTexSubImage2D((System.Graphics.ES20.TextureTarget) target, (Int32) level, (Int32) xoffset, (Int32) yoffset, (Int32) width, (Int32) height, (System.Graphics.ES20.PixelFormat) format, (System.Graphics.ES20.PixelType) type, (IntPtr) pixels);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify a two-dimensional texture subimage
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target texture. Must be GL_TEXTURE_2D, GL_TEXTURE_CUBE_MAP_POSITIVE_X, GL_TEXTURE_CUBE_MAP_NEGATIVE_X, GL_TEXTURE_CUBE_MAP_POSITIVE_Y, GL_TEXTURE_CUBE_MAP_NEGATIVE_Y, GL_TEXTURE_CUBE_MAP_POSITIVE_Z, or GL_TEXTURE_CUBE_MAP_NEGATIVE_Z.
		/// </para>
		/// </param>
		/// <param name="level">
		/// <para>
		/// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap reduction image.
		/// </para>
		/// </param>
		/// <param name="xoffset">
		/// <para>
		/// Specifies a texel offset in the x direction within the texture array.
		/// </para>
		/// </param>
		/// <param name="yoffset">
		/// <para>
		/// Specifies a texel offset in the y direction within the texture array.
		/// </para>
		/// </param>
		/// <param name="width">
		/// <para>
		/// Specifies the width of the texture subimage.
		/// </para>
		/// </param>
		/// <param name="height">
		/// <para>
		/// Specifies the height of the texture subimage.
		/// </para>
		/// </param>
		/// <param name="format">
		/// <para>
		/// Specifies the format of the pixel data. The following symbolic values are accepted: GL_RED, GL_RG, GL_RGB, GL_BGR, GL_RGBA, and GL_BGRA.
		/// </para>
		/// </param>
		/// <param name="type">
		/// <para>
		/// Specifies the data type of the pixel data. The following symbolic values are accepted: GL_UNSIGNED_BYTE, GL_BYTE, GL_UNSIGNED_SHORT, GL_SHORT, GL_UNSIGNED_INT, GL_INT, GL_FLOAT, GL_UNSIGNED_BYTE_3_3_2, GL_UNSIGNED_BYTE_2_3_3_REV, GL_UNSIGNED_SHORT_5_6_5, GL_UNSIGNED_SHORT_5_6_5_REV, GL_UNSIGNED_SHORT_4_4_4_4, GL_UNSIGNED_SHORT_4_4_4_4_REV, GL_UNSIGNED_SHORT_5_5_5_1, GL_UNSIGNED_SHORT_1_5_5_5_REV, GL_UNSIGNED_INT_8_8_8_8, GL_UNSIGNED_INT_8_8_8_8_REV, GL_UNSIGNED_INT_10_10_10_2, and GL_UNSIGNED_INT_2_10_10_10_REV.
		/// </para>
		/// </param>
		/// <param name="data">
		/// <para>
		/// Specifies a pointer to the image data in memory.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glTexSubImage2D")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void TexSubImage2D<T8>(System.Graphics.ES20.TextureTarget target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 width, Int32 height, System.Graphics.ES20.PixelFormat format, System.Graphics.ES20.PixelType type, [InAttribute, OutAttribute] T8[] pixels)
					where T8 : struct {
			GCHandle pixels_ptr = GCHandle.Alloc(pixels, GCHandleType.Pinned);
			try {
				Delegates.glTexSubImage2D((System.Graphics.ES20.TextureTarget) target, (Int32) level, (Int32) xoffset, (Int32) yoffset, (Int32) width, (Int32) height, (System.Graphics.ES20.PixelFormat) format, (System.Graphics.ES20.PixelType) type, (IntPtr) pixels_ptr.AddrOfPinnedObject());
			} finally {
				pixels_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify a two-dimensional texture subimage
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target texture. Must be GL_TEXTURE_2D, GL_TEXTURE_CUBE_MAP_POSITIVE_X, GL_TEXTURE_CUBE_MAP_NEGATIVE_X, GL_TEXTURE_CUBE_MAP_POSITIVE_Y, GL_TEXTURE_CUBE_MAP_NEGATIVE_Y, GL_TEXTURE_CUBE_MAP_POSITIVE_Z, or GL_TEXTURE_CUBE_MAP_NEGATIVE_Z.
		/// </para>
		/// </param>
		/// <param name="level">
		/// <para>
		/// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap reduction image.
		/// </para>
		/// </param>
		/// <param name="xoffset">
		/// <para>
		/// Specifies a texel offset in the x direction within the texture array.
		/// </para>
		/// </param>
		/// <param name="yoffset">
		/// <para>
		/// Specifies a texel offset in the y direction within the texture array.
		/// </para>
		/// </param>
		/// <param name="width">
		/// <para>
		/// Specifies the width of the texture subimage.
		/// </para>
		/// </param>
		/// <param name="height">
		/// <para>
		/// Specifies the height of the texture subimage.
		/// </para>
		/// </param>
		/// <param name="format">
		/// <para>
		/// Specifies the format of the pixel data. The following symbolic values are accepted: GL_RED, GL_RG, GL_RGB, GL_BGR, GL_RGBA, and GL_BGRA.
		/// </para>
		/// </param>
		/// <param name="type">
		/// <para>
		/// Specifies the data type of the pixel data. The following symbolic values are accepted: GL_UNSIGNED_BYTE, GL_BYTE, GL_UNSIGNED_SHORT, GL_SHORT, GL_UNSIGNED_INT, GL_INT, GL_FLOAT, GL_UNSIGNED_BYTE_3_3_2, GL_UNSIGNED_BYTE_2_3_3_REV, GL_UNSIGNED_SHORT_5_6_5, GL_UNSIGNED_SHORT_5_6_5_REV, GL_UNSIGNED_SHORT_4_4_4_4, GL_UNSIGNED_SHORT_4_4_4_4_REV, GL_UNSIGNED_SHORT_5_5_5_1, GL_UNSIGNED_SHORT_1_5_5_5_REV, GL_UNSIGNED_INT_8_8_8_8, GL_UNSIGNED_INT_8_8_8_8_REV, GL_UNSIGNED_INT_10_10_10_2, and GL_UNSIGNED_INT_2_10_10_10_REV.
		/// </para>
		/// </param>
		/// <param name="data">
		/// <para>
		/// Specifies a pointer to the image data in memory.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glTexSubImage2D")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void TexSubImage2D<T8>(System.Graphics.ES20.TextureTarget target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 width, Int32 height, System.Graphics.ES20.PixelFormat format, System.Graphics.ES20.PixelType type, [InAttribute, OutAttribute] T8[,] pixels)
					where T8 : struct {
			GCHandle pixels_ptr = GCHandle.Alloc(pixels, GCHandleType.Pinned);
			try {
				Delegates.glTexSubImage2D((System.Graphics.ES20.TextureTarget) target, (Int32) level, (Int32) xoffset, (Int32) yoffset, (Int32) width, (Int32) height, (System.Graphics.ES20.PixelFormat) format, (System.Graphics.ES20.PixelType) type, (IntPtr) pixels_ptr.AddrOfPinnedObject());
			} finally {
				pixels_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify a two-dimensional texture subimage
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target texture. Must be GL_TEXTURE_2D, GL_TEXTURE_CUBE_MAP_POSITIVE_X, GL_TEXTURE_CUBE_MAP_NEGATIVE_X, GL_TEXTURE_CUBE_MAP_POSITIVE_Y, GL_TEXTURE_CUBE_MAP_NEGATIVE_Y, GL_TEXTURE_CUBE_MAP_POSITIVE_Z, or GL_TEXTURE_CUBE_MAP_NEGATIVE_Z.
		/// </para>
		/// </param>
		/// <param name="level">
		/// <para>
		/// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap reduction image.
		/// </para>
		/// </param>
		/// <param name="xoffset">
		/// <para>
		/// Specifies a texel offset in the x direction within the texture array.
		/// </para>
		/// </param>
		/// <param name="yoffset">
		/// <para>
		/// Specifies a texel offset in the y direction within the texture array.
		/// </para>
		/// </param>
		/// <param name="width">
		/// <para>
		/// Specifies the width of the texture subimage.
		/// </para>
		/// </param>
		/// <param name="height">
		/// <para>
		/// Specifies the height of the texture subimage.
		/// </para>
		/// </param>
		/// <param name="format">
		/// <para>
		/// Specifies the format of the pixel data. The following symbolic values are accepted: GL_RED, GL_RG, GL_RGB, GL_BGR, GL_RGBA, and GL_BGRA.
		/// </para>
		/// </param>
		/// <param name="type">
		/// <para>
		/// Specifies the data type of the pixel data. The following symbolic values are accepted: GL_UNSIGNED_BYTE, GL_BYTE, GL_UNSIGNED_SHORT, GL_SHORT, GL_UNSIGNED_INT, GL_INT, GL_FLOAT, GL_UNSIGNED_BYTE_3_3_2, GL_UNSIGNED_BYTE_2_3_3_REV, GL_UNSIGNED_SHORT_5_6_5, GL_UNSIGNED_SHORT_5_6_5_REV, GL_UNSIGNED_SHORT_4_4_4_4, GL_UNSIGNED_SHORT_4_4_4_4_REV, GL_UNSIGNED_SHORT_5_5_5_1, GL_UNSIGNED_SHORT_1_5_5_5_REV, GL_UNSIGNED_INT_8_8_8_8, GL_UNSIGNED_INT_8_8_8_8_REV, GL_UNSIGNED_INT_10_10_10_2, and GL_UNSIGNED_INT_2_10_10_10_REV.
		/// </para>
		/// </param>
		/// <param name="data">
		/// <para>
		/// Specifies a pointer to the image data in memory.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glTexSubImage2D")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void TexSubImage2D<T8>(System.Graphics.ES20.TextureTarget target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 width, Int32 height, System.Graphics.ES20.PixelFormat format, System.Graphics.ES20.PixelType type, [InAttribute, OutAttribute] T8[,,] pixels)
					where T8 : struct {
			GCHandle pixels_ptr = GCHandle.Alloc(pixels, GCHandleType.Pinned);
			try {
				Delegates.glTexSubImage2D((System.Graphics.ES20.TextureTarget) target, (Int32) level, (Int32) xoffset, (Int32) yoffset, (Int32) width, (Int32) height, (System.Graphics.ES20.PixelFormat) format, (System.Graphics.ES20.PixelType) type, (IntPtr) pixels_ptr.AddrOfPinnedObject());
			} finally {
				pixels_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify a two-dimensional texture subimage
		/// </summary>
		/// <param name="target">
		/// <para>
		/// Specifies the target texture. Must be GL_TEXTURE_2D, GL_TEXTURE_CUBE_MAP_POSITIVE_X, GL_TEXTURE_CUBE_MAP_NEGATIVE_X, GL_TEXTURE_CUBE_MAP_POSITIVE_Y, GL_TEXTURE_CUBE_MAP_NEGATIVE_Y, GL_TEXTURE_CUBE_MAP_POSITIVE_Z, or GL_TEXTURE_CUBE_MAP_NEGATIVE_Z.
		/// </para>
		/// </param>
		/// <param name="level">
		/// <para>
		/// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap reduction image.
		/// </para>
		/// </param>
		/// <param name="xoffset">
		/// <para>
		/// Specifies a texel offset in the x direction within the texture array.
		/// </para>
		/// </param>
		/// <param name="yoffset">
		/// <para>
		/// Specifies a texel offset in the y direction within the texture array.
		/// </para>
		/// </param>
		/// <param name="width">
		/// <para>
		/// Specifies the width of the texture subimage.
		/// </para>
		/// </param>
		/// <param name="height">
		/// <para>
		/// Specifies the height of the texture subimage.
		/// </para>
		/// </param>
		/// <param name="format">
		/// <para>
		/// Specifies the format of the pixel data. The following symbolic values are accepted: GL_RED, GL_RG, GL_RGB, GL_BGR, GL_RGBA, and GL_BGRA.
		/// </para>
		/// </param>
		/// <param name="type">
		/// <para>
		/// Specifies the data type of the pixel data. The following symbolic values are accepted: GL_UNSIGNED_BYTE, GL_BYTE, GL_UNSIGNED_SHORT, GL_SHORT, GL_UNSIGNED_INT, GL_INT, GL_FLOAT, GL_UNSIGNED_BYTE_3_3_2, GL_UNSIGNED_BYTE_2_3_3_REV, GL_UNSIGNED_SHORT_5_6_5, GL_UNSIGNED_SHORT_5_6_5_REV, GL_UNSIGNED_SHORT_4_4_4_4, GL_UNSIGNED_SHORT_4_4_4_4_REV, GL_UNSIGNED_SHORT_5_5_5_1, GL_UNSIGNED_SHORT_1_5_5_5_REV, GL_UNSIGNED_INT_8_8_8_8, GL_UNSIGNED_INT_8_8_8_8_REV, GL_UNSIGNED_INT_10_10_10_2, and GL_UNSIGNED_INT_2_10_10_10_REV.
		/// </para>
		/// </param>
		/// <param name="data">
		/// <para>
		/// Specifies a pointer to the image data in memory.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glTexSubImage2D")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void TexSubImage2D<T8>(System.Graphics.ES20.TextureTarget target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 width, Int32 height, System.Graphics.ES20.PixelFormat format, System.Graphics.ES20.PixelType type, [InAttribute, OutAttribute] ref T8 pixels)
					where T8 : struct {
			GCHandle pixels_ptr = GCHandle.Alloc(pixels, GCHandleType.Pinned);
			try {
				Delegates.glTexSubImage2D((System.Graphics.ES20.TextureTarget) target, (Int32) level, (Int32) xoffset, (Int32) yoffset, (Int32) width, (Int32) height, (System.Graphics.ES20.PixelFormat) format, (System.Graphics.ES20.PixelType) type, (IntPtr) pixels_ptr.AddrOfPinnedObject());
				pixels = (T8) pixels_ptr.Target;
			} finally {
				pixels_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify the value of a uniform variable for the current program object
		/// </summary>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified uniform variable.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUniform1f")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void Uniform1(Int32 location, Single x) {
			Delegates.glUniform1f((Int32) location, (Single) x);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify the value of a uniform variable for the current program object
		/// </summary>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified uniform variable.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUniform1fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void Uniform1(Int32 location, Int32 count, Single[] v) {
			unsafe
			{
				fixed (Single* v_ptr = v) {
					Delegates.glUniform1fv((Int32) location, (Int32) count, (Single*) v_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify the value of a uniform variable for the current program object
		/// </summary>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified uniform variable.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUniform1fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void Uniform1(Int32 location, Int32 count, ref Single v) {
			unsafe
			{
				fixed (Single* v_ptr = &v) {
					Delegates.glUniform1fv((Int32) location, (Int32) count, (Single*) v_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify the value of a uniform variable for the current program object
		/// </summary>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified uniform variable.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUniform1fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void Uniform1(Int32 location, Int32 count, Single* v) {
			Delegates.glUniform1fv((Int32) location, (Int32) count, (Single*) v);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify the value of a uniform variable for the current program object
		/// </summary>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified uniform variable.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUniform1i")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void Uniform1(Int32 location, Int32 x) {
			Delegates.glUniform1i((Int32) location, (Int32) x);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify the value of a uniform variable for the current program object
		/// </summary>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified uniform variable.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUniform1iv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void Uniform1(Int32 location, Int32 count, Int32[] v) {
			unsafe
			{
				fixed (Int32* v_ptr = v) {
					Delegates.glUniform1iv((Int32) location, (Int32) count, (Int32*) v_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify the value of a uniform variable for the current program object
		/// </summary>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified uniform variable.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUniform1iv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void Uniform1(Int32 location, Int32 count, ref Int32 v) {
			unsafe
			{
				fixed (Int32* v_ptr = &v) {
					Delegates.glUniform1iv((Int32) location, (Int32) count, (Int32*) v_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify the value of a uniform variable for the current program object
		/// </summary>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified uniform variable.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUniform1iv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void Uniform1(Int32 location, Int32 count, Int32* v) {
			Delegates.glUniform1iv((Int32) location, (Int32) count, (Int32*) v);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify the value of a uniform variable for the current program object
		/// </summary>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified uniform variable.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUniform2f")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void Uniform2(Int32 location, Single x, Single y) {
			Delegates.glUniform2f((Int32) location, (Single) x, (Single) y);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify the value of a uniform variable for the current program object
		/// </summary>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified uniform variable.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUniform2fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void Uniform2(Int32 location, Int32 count, Single[] v) {
			unsafe
			{
				fixed (Single* v_ptr = v) {
					Delegates.glUniform2fv((Int32) location, (Int32) count, (Single*) v_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify the value of a uniform variable for the current program object
		/// </summary>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified uniform variable.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUniform2fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void Uniform2(Int32 location, Int32 count, ref Single v) {
			unsafe
			{
				fixed (Single* v_ptr = &v) {
					Delegates.glUniform2fv((Int32) location, (Int32) count, (Single*) v_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify the value of a uniform variable for the current program object
		/// </summary>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified uniform variable.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUniform2fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void Uniform2(Int32 location, Int32 count, Single* v) {
			Delegates.glUniform2fv((Int32) location, (Int32) count, (Single*) v);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify the value of a uniform variable for the current program object
		/// </summary>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified uniform variable.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUniform2i")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void Uniform2(Int32 location, Int32 x, Int32 y) {
			Delegates.glUniform2i((Int32) location, (Int32) x, (Int32) y);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify the value of a uniform variable for the current program object
		/// </summary>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified uniform variable.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUniform2iv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void Uniform2(Int32 location, Int32 count, Int32[] v) {
			unsafe
			{
				fixed (Int32* v_ptr = v) {
					Delegates.glUniform2iv((Int32) location, (Int32) count, (Int32*) v_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify the value of a uniform variable for the current program object
		/// </summary>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified uniform variable.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUniform2iv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void Uniform2(Int32 location, Int32 count, Int32* v) {
			Delegates.glUniform2iv((Int32) location, (Int32) count, (Int32*) v);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify the value of a uniform variable for the current program object
		/// </summary>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified uniform variable.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUniform3f")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void Uniform3(Int32 location, Single x, Single y, Single z) {
			Delegates.glUniform3f((Int32) location, (Single) x, (Single) y, (Single) z);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify the value of a uniform variable for the current program object
		/// </summary>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified uniform variable.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUniform3fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void Uniform3(Int32 location, Int32 count, Single[] v) {
			unsafe
			{
				fixed (Single* v_ptr = v) {
					Delegates.glUniform3fv((Int32) location, (Int32) count, (Single*) v_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify the value of a uniform variable for the current program object
		/// </summary>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified uniform variable.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUniform3fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void Uniform3(Int32 location, Int32 count, ref Single v) {
			unsafe
			{
				fixed (Single* v_ptr = &v) {
					Delegates.glUniform3fv((Int32) location, (Int32) count, (Single*) v_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify the value of a uniform variable for the current program object
		/// </summary>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified uniform variable.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUniform3fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void Uniform3(Int32 location, Int32 count, Single* v) {
			Delegates.glUniform3fv((Int32) location, (Int32) count, (Single*) v);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify the value of a uniform variable for the current program object
		/// </summary>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified uniform variable.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUniform3i")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void Uniform3(Int32 location, Int32 x, Int32 y, Int32 z) {
			Delegates.glUniform3i((Int32) location, (Int32) x, (Int32) y, (Int32) z);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify the value of a uniform variable for the current program object
		/// </summary>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified uniform variable.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUniform3iv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void Uniform3(Int32 location, Int32 count, Int32[] v) {
			unsafe
			{
				fixed (Int32* v_ptr = v) {
					Delegates.glUniform3iv((Int32) location, (Int32) count, (Int32*) v_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify the value of a uniform variable for the current program object
		/// </summary>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified uniform variable.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUniform3iv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void Uniform3(Int32 location, Int32 count, ref Int32 v) {
			unsafe
			{
				fixed (Int32* v_ptr = &v) {
					Delegates.glUniform3iv((Int32) location, (Int32) count, (Int32*) v_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify the value of a uniform variable for the current program object
		/// </summary>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified uniform variable.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUniform3iv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void Uniform3(Int32 location, Int32 count, Int32* v) {
			Delegates.glUniform3iv((Int32) location, (Int32) count, (Int32*) v);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify the value of a uniform variable for the current program object
		/// </summary>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified uniform variable.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUniform4f")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void Uniform4(Int32 location, Single x, Single y, Single z, Single w) {
			Delegates.glUniform4f((Int32) location, (Single) x, (Single) y, (Single) z, (Single) w);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify the value of a uniform variable for the current program object
		/// </summary>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified uniform variable.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUniform4fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void Uniform4(Int32 location, Int32 count, Single[] v) {
			unsafe
			{
				fixed (Single* v_ptr = v) {
					Delegates.glUniform4fv((Int32) location, (Int32) count, (Single*) v_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify the value of a uniform variable for the current program object
		/// </summary>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified uniform variable.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUniform4fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void Uniform4(Int32 location, Int32 count, ref Single v) {
			unsafe
			{
				fixed (Single* v_ptr = &v) {
					Delegates.glUniform4fv((Int32) location, (Int32) count, (Single*) v_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify the value of a uniform variable for the current program object
		/// </summary>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified uniform variable.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUniform4fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void Uniform4(Int32 location, Int32 count, Single* v) {
			Delegates.glUniform4fv((Int32) location, (Int32) count, (Single*) v);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify the value of a uniform variable for the current program object
		/// </summary>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified uniform variable.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUniform4i")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void Uniform4(Int32 location, Int32 x, Int32 y, Int32 z, Int32 w) {
			Delegates.glUniform4i((Int32) location, (Int32) x, (Int32) y, (Int32) z, (Int32) w);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify the value of a uniform variable for the current program object
		/// </summary>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified uniform variable.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUniform4iv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void Uniform4(Int32 location, Int32 count, Int32[] v) {
			unsafe
			{
				fixed (Int32* v_ptr = v) {
					Delegates.glUniform4iv((Int32) location, (Int32) count, (Int32*) v_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify the value of a uniform variable for the current program object
		/// </summary>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified uniform variable.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUniform4iv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void Uniform4(Int32 location, Int32 count, ref Int32 v) {
			unsafe
			{
				fixed (Int32* v_ptr = &v) {
					Delegates.glUniform4iv((Int32) location, (Int32) count, (Int32*) v_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specify the value of a uniform variable for the current program object
		/// </summary>
		/// <param name="location">
		/// <para>
		/// Specifies the location of the uniform variable to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified uniform variable.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUniform4iv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void Uniform4(Int32 location, Int32 count, Int32* v) {
			Delegates.glUniform4iv((Int32) location, (Int32) count, (Int32*) v);
		}

		/// <summary>[requires: v2.0 and 2.0]</summary>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUniformMatrix2fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void UniformMatrix2(Int32 location, Int32 count, bool transpose, Single[] value) {
			unsafe
			{
				fixed (Single* value_ptr = value) {
					Delegates.glUniformMatrix2fv((Int32) location, (Int32) count, (bool) transpose, (Single*) value_ptr);
				}
			}
		}

		/// <summary>[requires: v2.0 and 2.0]</summary>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUniformMatrix2fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void UniformMatrix2(Int32 location, Int32 count, bool transpose, ref Single value) {
			unsafe
			{
				fixed (Single* value_ptr = &value) {
					Delegates.glUniformMatrix2fv((Int32) location, (Int32) count, (bool) transpose, (Single*) value_ptr);
				}
			}
		}

		/// <summary>[requires: v2.0 and 2.0]</summary>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUniformMatrix2fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void UniformMatrix2(Int32 location, Int32 count, bool transpose, Single* value) {
			Delegates.glUniformMatrix2fv((Int32) location, (Int32) count, (bool) transpose, (Single*) value);
		}

		/// <summary>[requires: v2.0 and 2.0]</summary>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUniformMatrix3fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void UniformMatrix3(Int32 location, Int32 count, bool transpose, Single[] value) {
			unsafe
			{
				fixed (Single* value_ptr = value) {
					Delegates.glUniformMatrix3fv((Int32) location, (Int32) count, (bool) transpose, (Single*) value_ptr);
				}
			}
		}

		/// <summary>[requires: v2.0 and 2.0]</summary>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUniformMatrix3fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void UniformMatrix3(Int32 location, Int32 count, bool transpose, ref Single value) {
			unsafe
			{
				fixed (Single* value_ptr = &value) {
					Delegates.glUniformMatrix3fv((Int32) location, (Int32) count, (bool) transpose, (Single*) value_ptr);
				}
			}
		}

		/// <summary>[requires: v2.0 and 2.0]</summary>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUniformMatrix3fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void UniformMatrix3(Int32 location, Int32 count, bool transpose, Single* value) {
			Delegates.glUniformMatrix3fv((Int32) location, (Int32) count, (bool) transpose, (Single*) value);
		}

		/// <summary>[requires: v2.0 and 2.0]</summary>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUniformMatrix4fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void UniformMatrix4(Int32 location, Int32 count, bool transpose, Single[] value) {
			unsafe
			{
				fixed (Single* value_ptr = value) {
					Delegates.glUniformMatrix4fv((Int32) location, (Int32) count, (bool) transpose, (Single*) value_ptr);
				}
			}
		}

		/// <summary>[requires: v2.0 and 2.0]</summary>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUniformMatrix4fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void UniformMatrix4(Int32 location, Int32 count, bool transpose, ref Single value) {
			unsafe
			{
				fixed (Single* value_ptr = &value) {
					Delegates.glUniformMatrix4fv((Int32) location, (Int32) count, (bool) transpose, (Single*) value_ptr);
				}
			}
		}

		/// <summary>[requires: v2.0 and 2.0]</summary>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUniformMatrix4fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void UniformMatrix4(Int32 location, Int32 count, bool transpose, Single* value) {
			Delegates.glUniformMatrix4fv((Int32) location, (Int32) count, (bool) transpose, (Single*) value);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Installs a program object as part of current rendering state
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the handle of the program object whose executables are to be used as part of current rendering state.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUseProgram")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void UseProgram(Int32 program) {
			Delegates.glUseProgram((UInt32) program);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Installs a program object as part of current rendering state
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the handle of the program object whose executables are to be used as part of current rendering state.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUseProgram")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void UseProgram(UInt32 program) {
			Delegates.glUseProgram((UInt32) program);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Validates a program object
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the handle of the program object to be validated.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glValidateProgram")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void ValidateProgram(Int32 program) {
			Delegates.glValidateProgram((UInt32) program);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Validates a program object
		/// </summary>
		/// <param name="program">
		/// <para>
		/// Specifies the handle of the program object to be validated.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glValidateProgram")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void ValidateProgram(UInt32 program) {
			Delegates.glValidateProgram((UInt32) program);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specifies the value of a generic vertex attribute
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified vertex attribute.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glVertexAttrib1f")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void VertexAttrib1(Int32 indx, Single x) {
			Delegates.glVertexAttrib1f((UInt32) indx, (Single) x);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specifies the value of a generic vertex attribute
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified vertex attribute.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glVertexAttrib1f")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void VertexAttrib1(UInt32 indx, Single x) {
			Delegates.glVertexAttrib1f((UInt32) indx, (Single) x);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specifies the value of a generic vertex attribute
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified vertex attribute.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glVertexAttrib1fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void VertexAttrib1(Int32 indx, Single[] values) {
			unsafe
			{
				fixed (Single* values_ptr = values) {
					Delegates.glVertexAttrib1fv((UInt32) indx, (Single*) values_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specifies the value of a generic vertex attribute
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified vertex attribute.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glVertexAttrib1fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void VertexAttrib1(Int32 indx, Single* values) {
			Delegates.glVertexAttrib1fv((UInt32) indx, (Single*) values);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specifies the value of a generic vertex attribute
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified vertex attribute.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glVertexAttrib1fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void VertexAttrib1(UInt32 indx, Single[] values) {
			unsafe
			{
				fixed (Single* values_ptr = values) {
					Delegates.glVertexAttrib1fv((UInt32) indx, (Single*) values_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specifies the value of a generic vertex attribute
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified vertex attribute.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glVertexAttrib1fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void VertexAttrib1(UInt32 indx, Single* values) {
			Delegates.glVertexAttrib1fv((UInt32) indx, (Single*) values);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specifies the value of a generic vertex attribute
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified vertex attribute.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glVertexAttrib2f")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void VertexAttrib2(Int32 indx, Single x, Single y) {
			Delegates.glVertexAttrib2f((UInt32) indx, (Single) x, (Single) y);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specifies the value of a generic vertex attribute
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified vertex attribute.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glVertexAttrib2f")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void VertexAttrib2(UInt32 indx, Single x, Single y) {
			Delegates.glVertexAttrib2f((UInt32) indx, (Single) x, (Single) y);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specifies the value of a generic vertex attribute
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified vertex attribute.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glVertexAttrib2fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void VertexAttrib2(Int32 indx, Single[] values) {
			unsafe
			{
				fixed (Single* values_ptr = values) {
					Delegates.glVertexAttrib2fv((UInt32) indx, (Single*) values_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specifies the value of a generic vertex attribute
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified vertex attribute.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glVertexAttrib2fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void VertexAttrib2(Int32 indx, ref Single values) {
			unsafe
			{
				fixed (Single* values_ptr = &values) {
					Delegates.glVertexAttrib2fv((UInt32) indx, (Single*) values_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specifies the value of a generic vertex attribute
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified vertex attribute.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glVertexAttrib2fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void VertexAttrib2(Int32 indx, Single* values) {
			Delegates.glVertexAttrib2fv((UInt32) indx, (Single*) values);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specifies the value of a generic vertex attribute
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified vertex attribute.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glVertexAttrib2fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void VertexAttrib2(UInt32 indx, Single[] values) {
			unsafe
			{
				fixed (Single* values_ptr = values) {
					Delegates.glVertexAttrib2fv((UInt32) indx, (Single*) values_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specifies the value of a generic vertex attribute
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified vertex attribute.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glVertexAttrib2fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void VertexAttrib2(UInt32 indx, ref Single values) {
			unsafe
			{
				fixed (Single* values_ptr = &values) {
					Delegates.glVertexAttrib2fv((UInt32) indx, (Single*) values_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specifies the value of a generic vertex attribute
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified vertex attribute.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glVertexAttrib2fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void VertexAttrib2(UInt32 indx, Single* values) {
			Delegates.glVertexAttrib2fv((UInt32) indx, (Single*) values);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specifies the value of a generic vertex attribute
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified vertex attribute.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glVertexAttrib3f")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void VertexAttrib3(Int32 indx, Single x, Single y, Single z) {
			Delegates.glVertexAttrib3f((UInt32) indx, (Single) x, (Single) y, (Single) z);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specifies the value of a generic vertex attribute
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified vertex attribute.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glVertexAttrib3f")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void VertexAttrib3(UInt32 indx, Single x, Single y, Single z) {
			Delegates.glVertexAttrib3f((UInt32) indx, (Single) x, (Single) y, (Single) z);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specifies the value of a generic vertex attribute
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified vertex attribute.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glVertexAttrib3fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void VertexAttrib3(Int32 indx, Single[] values) {
			unsafe
			{
				fixed (Single* values_ptr = values) {
					Delegates.glVertexAttrib3fv((UInt32) indx, (Single*) values_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specifies the value of a generic vertex attribute
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified vertex attribute.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glVertexAttrib3fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void VertexAttrib3(Int32 indx, ref Single values) {
			unsafe
			{
				fixed (Single* values_ptr = &values) {
					Delegates.glVertexAttrib3fv((UInt32) indx, (Single*) values_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specifies the value of a generic vertex attribute
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified vertex attribute.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glVertexAttrib3fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void VertexAttrib3(Int32 indx, Single* values) {
			Delegates.glVertexAttrib3fv((UInt32) indx, (Single*) values);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specifies the value of a generic vertex attribute
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified vertex attribute.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glVertexAttrib3fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void VertexAttrib3(UInt32 indx, Single[] values) {
			unsafe
			{
				fixed (Single* values_ptr = values) {
					Delegates.glVertexAttrib3fv((UInt32) indx, (Single*) values_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specifies the value of a generic vertex attribute
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified vertex attribute.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glVertexAttrib3fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void VertexAttrib3(UInt32 indx, ref Single values) {
			unsafe
			{
				fixed (Single* values_ptr = &values) {
					Delegates.glVertexAttrib3fv((UInt32) indx, (Single*) values_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specifies the value of a generic vertex attribute
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified vertex attribute.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glVertexAttrib3fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void VertexAttrib3(UInt32 indx, Single* values) {
			Delegates.glVertexAttrib3fv((UInt32) indx, (Single*) values);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specifies the value of a generic vertex attribute
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified vertex attribute.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glVertexAttrib4f")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void VertexAttrib4(Int32 indx, Single x, Single y, Single z, Single w) {
			Delegates.glVertexAttrib4f((UInt32) indx, (Single) x, (Single) y, (Single) z, (Single) w);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specifies the value of a generic vertex attribute
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified vertex attribute.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glVertexAttrib4f")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void VertexAttrib4(UInt32 indx, Single x, Single y, Single z, Single w) {
			Delegates.glVertexAttrib4f((UInt32) indx, (Single) x, (Single) y, (Single) z, (Single) w);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specifies the value of a generic vertex attribute
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified vertex attribute.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glVertexAttrib4fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void VertexAttrib4(Int32 indx, Single[] values) {
			unsafe
			{
				fixed (Single* values_ptr = values) {
					Delegates.glVertexAttrib4fv((UInt32) indx, (Single*) values_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specifies the value of a generic vertex attribute
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified vertex attribute.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glVertexAttrib4fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void VertexAttrib4(Int32 indx, ref Single values) {
			unsafe
			{
				fixed (Single* values_ptr = &values) {
					Delegates.glVertexAttrib4fv((UInt32) indx, (Single*) values_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specifies the value of a generic vertex attribute
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified vertex attribute.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glVertexAttrib4fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void VertexAttrib4(Int32 indx, Single* values) {
			Delegates.glVertexAttrib4fv((UInt32) indx, (Single*) values);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specifies the value of a generic vertex attribute
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified vertex attribute.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glVertexAttrib4fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void VertexAttrib4(UInt32 indx, Single[] values) {
			unsafe
			{
				fixed (Single* values_ptr = values) {
					Delegates.glVertexAttrib4fv((UInt32) indx, (Single*) values_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specifies the value of a generic vertex attribute
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified vertex attribute.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glVertexAttrib4fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void VertexAttrib4(UInt32 indx, ref Single values) {
			unsafe
			{
				fixed (Single* values_ptr = &values) {
					Delegates.glVertexAttrib4fv((UInt32) indx, (Single*) values_ptr);
				}
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Specifies the value of a generic vertex attribute
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be modified.
		/// </para>
		/// </param>
		/// <param name="v0">
		/// <para>
		/// Specifies the new values to be used for the specified vertex attribute.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glVertexAttrib4fv")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		unsafe void VertexAttrib4(UInt32 indx, Single* values) {
			Delegates.glVertexAttrib4fv((UInt32) indx, (Single*) values);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Define an array of generic vertex attribute data
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be modified.
		/// </para>
		/// </param>
		/// <param name="size">
		/// <para>
		/// Specifies the number of components per generic vertex attribute. Must be 1, 2, 3, 4. Additionally, the symbolic constant GL_BGRA is accepted by glVertexAttribPointer. The initial value is 4.
		/// </para>
		/// </param>
		/// <param name="type">
		/// <para>
		/// Specifies the data type of each component in the array. The symbolic constants GL_BYTE, GL_UNSIGNED_BYTE, GL_SHORT, GL_UNSIGNED_SHORT, GL_INT, and GL_UNSIGNED_INT are accepted by both functions. Additionally GL_HALF_FLOAT, GL_FLOAT, GL_DOUBLE, GL_FIXED, GL_INT_2_10_10_10_REV, and GL_UNSIGNED_INT_2_10_10_10_REV are accepted by glVertexAttribPointer. The initial value is GL_FLOAT.
		/// </para>
		/// </param>
		/// <param name="normalized">
		/// <para>
		/// For glVertexAttribPointer, specifies whether fixed-point data values should be normalized (GL_TRUE) or converted directly as fixed-point values (GL_FALSE) when they are accessed.
		/// </para>
		/// </param>
		/// <param name="stride">
		/// <para>
		/// Specifies the byte offset between consecutive generic vertex attributes. If stride is 0, the generic vertex attributes are understood to be tightly packed in the array. The initial value is 0.
		/// </para>
		/// </param>
		/// <param name="pointer">
		/// <para>
		/// Specifies a offset of the first component of the first generic vertex attribute in the array in the data store of the buffer currently bound to the GL_ARRAY_BUFFER target. The initial value is 0.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glVertexAttribPointer")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void VertexAttribPointer(Int32 indx, Int32 size, System.Graphics.ES20.VertexAttribPointerType type, bool normalized, Int32 stride, IntPtr ptr) {
			Delegates.glVertexAttribPointer((UInt32) indx, (Int32) size, (System.Graphics.ES20.VertexAttribPointerType) type, (bool) normalized, (Int32) stride, (IntPtr) ptr);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Define an array of generic vertex attribute data
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be modified.
		/// </para>
		/// </param>
		/// <param name="size">
		/// <para>
		/// Specifies the number of components per generic vertex attribute. Must be 1, 2, 3, 4. Additionally, the symbolic constant GL_BGRA is accepted by glVertexAttribPointer. The initial value is 4.
		/// </para>
		/// </param>
		/// <param name="type">
		/// <para>
		/// Specifies the data type of each component in the array. The symbolic constants GL_BYTE, GL_UNSIGNED_BYTE, GL_SHORT, GL_UNSIGNED_SHORT, GL_INT, and GL_UNSIGNED_INT are accepted by both functions. Additionally GL_HALF_FLOAT, GL_FLOAT, GL_DOUBLE, GL_FIXED, GL_INT_2_10_10_10_REV, and GL_UNSIGNED_INT_2_10_10_10_REV are accepted by glVertexAttribPointer. The initial value is GL_FLOAT.
		/// </para>
		/// </param>
		/// <param name="normalized">
		/// <para>
		/// For glVertexAttribPointer, specifies whether fixed-point data values should be normalized (GL_TRUE) or converted directly as fixed-point values (GL_FALSE) when they are accessed.
		/// </para>
		/// </param>
		/// <param name="stride">
		/// <para>
		/// Specifies the byte offset between consecutive generic vertex attributes. If stride is 0, the generic vertex attributes are understood to be tightly packed in the array. The initial value is 0.
		/// </para>
		/// </param>
		/// <param name="pointer">
		/// <para>
		/// Specifies a offset of the first component of the first generic vertex attribute in the array in the data store of the buffer currently bound to the GL_ARRAY_BUFFER target. The initial value is 0.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glVertexAttribPointer")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void VertexAttribPointer<T5>(Int32 indx, Int32 size, System.Graphics.ES20.VertexAttribPointerType type, bool normalized, Int32 stride, [InAttribute, OutAttribute] T5[] ptr)
					where T5 : struct {
			GCHandle ptr_ptr = GCHandle.Alloc(ptr, GCHandleType.Pinned);
			try {
				Delegates.glVertexAttribPointer((UInt32) indx, (Int32) size, (System.Graphics.ES20.VertexAttribPointerType) type, (bool) normalized, (Int32) stride, (IntPtr) ptr_ptr.AddrOfPinnedObject());
			} finally {
				ptr_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Define an array of generic vertex attribute data
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be modified.
		/// </para>
		/// </param>
		/// <param name="size">
		/// <para>
		/// Specifies the number of components per generic vertex attribute. Must be 1, 2, 3, 4. Additionally, the symbolic constant GL_BGRA is accepted by glVertexAttribPointer. The initial value is 4.
		/// </para>
		/// </param>
		/// <param name="type">
		/// <para>
		/// Specifies the data type of each component in the array. The symbolic constants GL_BYTE, GL_UNSIGNED_BYTE, GL_SHORT, GL_UNSIGNED_SHORT, GL_INT, and GL_UNSIGNED_INT are accepted by both functions. Additionally GL_HALF_FLOAT, GL_FLOAT, GL_DOUBLE, GL_FIXED, GL_INT_2_10_10_10_REV, and GL_UNSIGNED_INT_2_10_10_10_REV are accepted by glVertexAttribPointer. The initial value is GL_FLOAT.
		/// </para>
		/// </param>
		/// <param name="normalized">
		/// <para>
		/// For glVertexAttribPointer, specifies whether fixed-point data values should be normalized (GL_TRUE) or converted directly as fixed-point values (GL_FALSE) when they are accessed.
		/// </para>
		/// </param>
		/// <param name="stride">
		/// <para>
		/// Specifies the byte offset between consecutive generic vertex attributes. If stride is 0, the generic vertex attributes are understood to be tightly packed in the array. The initial value is 0.
		/// </para>
		/// </param>
		/// <param name="pointer">
		/// <para>
		/// Specifies a offset of the first component of the first generic vertex attribute in the array in the data store of the buffer currently bound to the GL_ARRAY_BUFFER target. The initial value is 0.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glVertexAttribPointer")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void VertexAttribPointer<T5>(Int32 indx, Int32 size, System.Graphics.ES20.VertexAttribPointerType type, bool normalized, Int32 stride, [InAttribute, OutAttribute] T5[,] ptr)
					where T5 : struct {
			GCHandle ptr_ptr = GCHandle.Alloc(ptr, GCHandleType.Pinned);
			try {
				Delegates.glVertexAttribPointer((UInt32) indx, (Int32) size, (System.Graphics.ES20.VertexAttribPointerType) type, (bool) normalized, (Int32) stride, (IntPtr) ptr_ptr.AddrOfPinnedObject());
			} finally {
				ptr_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Define an array of generic vertex attribute data
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be modified.
		/// </para>
		/// </param>
		/// <param name="size">
		/// <para>
		/// Specifies the number of components per generic vertex attribute. Must be 1, 2, 3, 4. Additionally, the symbolic constant GL_BGRA is accepted by glVertexAttribPointer. The initial value is 4.
		/// </para>
		/// </param>
		/// <param name="type">
		/// <para>
		/// Specifies the data type of each component in the array. The symbolic constants GL_BYTE, GL_UNSIGNED_BYTE, GL_SHORT, GL_UNSIGNED_SHORT, GL_INT, and GL_UNSIGNED_INT are accepted by both functions. Additionally GL_HALF_FLOAT, GL_FLOAT, GL_DOUBLE, GL_FIXED, GL_INT_2_10_10_10_REV, and GL_UNSIGNED_INT_2_10_10_10_REV are accepted by glVertexAttribPointer. The initial value is GL_FLOAT.
		/// </para>
		/// </param>
		/// <param name="normalized">
		/// <para>
		/// For glVertexAttribPointer, specifies whether fixed-point data values should be normalized (GL_TRUE) or converted directly as fixed-point values (GL_FALSE) when they are accessed.
		/// </para>
		/// </param>
		/// <param name="stride">
		/// <para>
		/// Specifies the byte offset between consecutive generic vertex attributes. If stride is 0, the generic vertex attributes are understood to be tightly packed in the array. The initial value is 0.
		/// </para>
		/// </param>
		/// <param name="pointer">
		/// <para>
		/// Specifies a offset of the first component of the first generic vertex attribute in the array in the data store of the buffer currently bound to the GL_ARRAY_BUFFER target. The initial value is 0.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glVertexAttribPointer")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void VertexAttribPointer<T5>(Int32 indx, Int32 size, System.Graphics.ES20.VertexAttribPointerType type, bool normalized, Int32 stride, [InAttribute, OutAttribute] T5[,,] ptr)
					where T5 : struct {
			GCHandle ptr_ptr = GCHandle.Alloc(ptr, GCHandleType.Pinned);
			try {
				Delegates.glVertexAttribPointer((UInt32) indx, (Int32) size, (System.Graphics.ES20.VertexAttribPointerType) type, (bool) normalized, (Int32) stride, (IntPtr) ptr_ptr.AddrOfPinnedObject());
			} finally {
				ptr_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Define an array of generic vertex attribute data
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be modified.
		/// </para>
		/// </param>
		/// <param name="size">
		/// <para>
		/// Specifies the number of components per generic vertex attribute. Must be 1, 2, 3, 4. Additionally, the symbolic constant GL_BGRA is accepted by glVertexAttribPointer. The initial value is 4.
		/// </para>
		/// </param>
		/// <param name="type">
		/// <para>
		/// Specifies the data type of each component in the array. The symbolic constants GL_BYTE, GL_UNSIGNED_BYTE, GL_SHORT, GL_UNSIGNED_SHORT, GL_INT, and GL_UNSIGNED_INT are accepted by both functions. Additionally GL_HALF_FLOAT, GL_FLOAT, GL_DOUBLE, GL_FIXED, GL_INT_2_10_10_10_REV, and GL_UNSIGNED_INT_2_10_10_10_REV are accepted by glVertexAttribPointer. The initial value is GL_FLOAT.
		/// </para>
		/// </param>
		/// <param name="normalized">
		/// <para>
		/// For glVertexAttribPointer, specifies whether fixed-point data values should be normalized (GL_TRUE) or converted directly as fixed-point values (GL_FALSE) when they are accessed.
		/// </para>
		/// </param>
		/// <param name="stride">
		/// <para>
		/// Specifies the byte offset between consecutive generic vertex attributes. If stride is 0, the generic vertex attributes are understood to be tightly packed in the array. The initial value is 0.
		/// </para>
		/// </param>
		/// <param name="pointer">
		/// <para>
		/// Specifies a offset of the first component of the first generic vertex attribute in the array in the data store of the buffer currently bound to the GL_ARRAY_BUFFER target. The initial value is 0.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glVertexAttribPointer")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void VertexAttribPointer<T5>(Int32 indx, Int32 size, System.Graphics.ES20.VertexAttribPointerType type, bool normalized, Int32 stride, [InAttribute, OutAttribute] ref T5 ptr)
					where T5 : struct {
			GCHandle ptr_ptr = GCHandle.Alloc(ptr, GCHandleType.Pinned);
			try {
				Delegates.glVertexAttribPointer((UInt32) indx, (Int32) size, (System.Graphics.ES20.VertexAttribPointerType) type, (bool) normalized, (Int32) stride, (IntPtr) ptr_ptr.AddrOfPinnedObject());
				ptr = (T5) ptr_ptr.Target;
			} finally {
				ptr_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Define an array of generic vertex attribute data
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be modified.
		/// </para>
		/// </param>
		/// <param name="size">
		/// <para>
		/// Specifies the number of components per generic vertex attribute. Must be 1, 2, 3, 4. Additionally, the symbolic constant GL_BGRA is accepted by glVertexAttribPointer. The initial value is 4.
		/// </para>
		/// </param>
		/// <param name="type">
		/// <para>
		/// Specifies the data type of each component in the array. The symbolic constants GL_BYTE, GL_UNSIGNED_BYTE, GL_SHORT, GL_UNSIGNED_SHORT, GL_INT, and GL_UNSIGNED_INT are accepted by both functions. Additionally GL_HALF_FLOAT, GL_FLOAT, GL_DOUBLE, GL_FIXED, GL_INT_2_10_10_10_REV, and GL_UNSIGNED_INT_2_10_10_10_REV are accepted by glVertexAttribPointer. The initial value is GL_FLOAT.
		/// </para>
		/// </param>
		/// <param name="normalized">
		/// <para>
		/// For glVertexAttribPointer, specifies whether fixed-point data values should be normalized (GL_TRUE) or converted directly as fixed-point values (GL_FALSE) when they are accessed.
		/// </para>
		/// </param>
		/// <param name="stride">
		/// <para>
		/// Specifies the byte offset between consecutive generic vertex attributes. If stride is 0, the generic vertex attributes are understood to be tightly packed in the array. The initial value is 0.
		/// </para>
		/// </param>
		/// <param name="pointer">
		/// <para>
		/// Specifies a offset of the first component of the first generic vertex attribute in the array in the data store of the buffer currently bound to the GL_ARRAY_BUFFER target. The initial value is 0.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glVertexAttribPointer")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void VertexAttribPointer(UInt32 indx, Int32 size, System.Graphics.ES20.VertexAttribPointerType type, bool normalized, Int32 stride, IntPtr ptr) {
			Delegates.glVertexAttribPointer((UInt32) indx, (Int32) size, (System.Graphics.ES20.VertexAttribPointerType) type, (bool) normalized, (Int32) stride, (IntPtr) ptr);
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Define an array of generic vertex attribute data
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be modified.
		/// </para>
		/// </param>
		/// <param name="size">
		/// <para>
		/// Specifies the number of components per generic vertex attribute. Must be 1, 2, 3, 4. Additionally, the symbolic constant GL_BGRA is accepted by glVertexAttribPointer. The initial value is 4.
		/// </para>
		/// </param>
		/// <param name="type">
		/// <para>
		/// Specifies the data type of each component in the array. The symbolic constants GL_BYTE, GL_UNSIGNED_BYTE, GL_SHORT, GL_UNSIGNED_SHORT, GL_INT, and GL_UNSIGNED_INT are accepted by both functions. Additionally GL_HALF_FLOAT, GL_FLOAT, GL_DOUBLE, GL_FIXED, GL_INT_2_10_10_10_REV, and GL_UNSIGNED_INT_2_10_10_10_REV are accepted by glVertexAttribPointer. The initial value is GL_FLOAT.
		/// </para>
		/// </param>
		/// <param name="normalized">
		/// <para>
		/// For glVertexAttribPointer, specifies whether fixed-point data values should be normalized (GL_TRUE) or converted directly as fixed-point values (GL_FALSE) when they are accessed.
		/// </para>
		/// </param>
		/// <param name="stride">
		/// <para>
		/// Specifies the byte offset between consecutive generic vertex attributes. If stride is 0, the generic vertex attributes are understood to be tightly packed in the array. The initial value is 0.
		/// </para>
		/// </param>
		/// <param name="pointer">
		/// <para>
		/// Specifies a offset of the first component of the first generic vertex attribute in the array in the data store of the buffer currently bound to the GL_ARRAY_BUFFER target. The initial value is 0.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glVertexAttribPointer")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void VertexAttribPointer<T5>(UInt32 indx, Int32 size, System.Graphics.ES20.VertexAttribPointerType type, bool normalized, Int32 stride, [InAttribute, OutAttribute] T5[] ptr)
			where T5 : struct {
			GCHandle ptr_ptr = GCHandle.Alloc(ptr, GCHandleType.Pinned);
			try {
				Delegates.glVertexAttribPointer((UInt32) indx, (Int32) size, (System.Graphics.ES20.VertexAttribPointerType) type, (bool) normalized, (Int32) stride, (IntPtr) ptr_ptr.AddrOfPinnedObject());
			} finally {
				ptr_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Define an array of generic vertex attribute data
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be modified.
		/// </para>
		/// </param>
		/// <param name="size">
		/// <para>
		/// Specifies the number of components per generic vertex attribute. Must be 1, 2, 3, 4. Additionally, the symbolic constant GL_BGRA is accepted by glVertexAttribPointer. The initial value is 4.
		/// </para>
		/// </param>
		/// <param name="type">
		/// <para>
		/// Specifies the data type of each component in the array. The symbolic constants GL_BYTE, GL_UNSIGNED_BYTE, GL_SHORT, GL_UNSIGNED_SHORT, GL_INT, and GL_UNSIGNED_INT are accepted by both functions. Additionally GL_HALF_FLOAT, GL_FLOAT, GL_DOUBLE, GL_FIXED, GL_INT_2_10_10_10_REV, and GL_UNSIGNED_INT_2_10_10_10_REV are accepted by glVertexAttribPointer. The initial value is GL_FLOAT.
		/// </para>
		/// </param>
		/// <param name="normalized">
		/// <para>
		/// For glVertexAttribPointer, specifies whether fixed-point data values should be normalized (GL_TRUE) or converted directly as fixed-point values (GL_FALSE) when they are accessed.
		/// </para>
		/// </param>
		/// <param name="stride">
		/// <para>
		/// Specifies the byte offset between consecutive generic vertex attributes. If stride is 0, the generic vertex attributes are understood to be tightly packed in the array. The initial value is 0.
		/// </para>
		/// </param>
		/// <param name="pointer">
		/// <para>
		/// Specifies a offset of the first component of the first generic vertex attribute in the array in the data store of the buffer currently bound to the GL_ARRAY_BUFFER target. The initial value is 0.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glVertexAttribPointer")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void VertexAttribPointer<T5>(UInt32 indx, Int32 size, System.Graphics.ES20.VertexAttribPointerType type, bool normalized, Int32 stride, [InAttribute, OutAttribute] T5[,] ptr)
			where T5 : struct {
			GCHandle ptr_ptr = GCHandle.Alloc(ptr, GCHandleType.Pinned);
			try {
				Delegates.glVertexAttribPointer((UInt32) indx, (Int32) size, (System.Graphics.ES20.VertexAttribPointerType) type, (bool) normalized, (Int32) stride, (IntPtr) ptr_ptr.AddrOfPinnedObject());
			} finally {
				ptr_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Define an array of generic vertex attribute data
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be modified.
		/// </para>
		/// </param>
		/// <param name="size">
		/// <para>
		/// Specifies the number of components per generic vertex attribute. Must be 1, 2, 3, 4. Additionally, the symbolic constant GL_BGRA is accepted by glVertexAttribPointer. The initial value is 4.
		/// </para>
		/// </param>
		/// <param name="type">
		/// <para>
		/// Specifies the data type of each component in the array. The symbolic constants GL_BYTE, GL_UNSIGNED_BYTE, GL_SHORT, GL_UNSIGNED_SHORT, GL_INT, and GL_UNSIGNED_INT are accepted by both functions. Additionally GL_HALF_FLOAT, GL_FLOAT, GL_DOUBLE, GL_FIXED, GL_INT_2_10_10_10_REV, and GL_UNSIGNED_INT_2_10_10_10_REV are accepted by glVertexAttribPointer. The initial value is GL_FLOAT.
		/// </para>
		/// </param>
		/// <param name="normalized">
		/// <para>
		/// For glVertexAttribPointer, specifies whether fixed-point data values should be normalized (GL_TRUE) or converted directly as fixed-point values (GL_FALSE) when they are accessed.
		/// </para>
		/// </param>
		/// <param name="stride">
		/// <para>
		/// Specifies the byte offset between consecutive generic vertex attributes. If stride is 0, the generic vertex attributes are understood to be tightly packed in the array. The initial value is 0.
		/// </para>
		/// </param>
		/// <param name="pointer">
		/// <para>
		/// Specifies a offset of the first component of the first generic vertex attribute in the array in the data store of the buffer currently bound to the GL_ARRAY_BUFFER target. The initial value is 0.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glVertexAttribPointer")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void VertexAttribPointer<T5>(UInt32 indx, Int32 size, System.Graphics.ES20.VertexAttribPointerType type, bool normalized, Int32 stride, [InAttribute, OutAttribute] T5[,,] ptr)
			where T5 : struct {
			GCHandle ptr_ptr = GCHandle.Alloc(ptr, GCHandleType.Pinned);
			try {
				Delegates.glVertexAttribPointer((UInt32) indx, (Int32) size, (System.Graphics.ES20.VertexAttribPointerType) type, (bool) normalized, (Int32) stride, (IntPtr) ptr_ptr.AddrOfPinnedObject());
			} finally {
				ptr_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Define an array of generic vertex attribute data
		/// </summary>
		/// <param name="index">
		/// <para>
		/// Specifies the index of the generic vertex attribute to be modified.
		/// </para>
		/// </param>
		/// <param name="size">
		/// <para>
		/// Specifies the number of components per generic vertex attribute. Must be 1, 2, 3, 4. Additionally, the symbolic constant GL_BGRA is accepted by glVertexAttribPointer. The initial value is 4.
		/// </para>
		/// </param>
		/// <param name="type">
		/// <para>
		/// Specifies the data type of each component in the array. The symbolic constants GL_BYTE, GL_UNSIGNED_BYTE, GL_SHORT, GL_UNSIGNED_SHORT, GL_INT, and GL_UNSIGNED_INT are accepted by both functions. Additionally GL_HALF_FLOAT, GL_FLOAT, GL_DOUBLE, GL_FIXED, GL_INT_2_10_10_10_REV, and GL_UNSIGNED_INT_2_10_10_10_REV are accepted by glVertexAttribPointer. The initial value is GL_FLOAT.
		/// </para>
		/// </param>
		/// <param name="normalized">
		/// <para>
		/// For glVertexAttribPointer, specifies whether fixed-point data values should be normalized (GL_TRUE) or converted directly as fixed-point values (GL_FALSE) when they are accessed.
		/// </para>
		/// </param>
		/// <param name="stride">
		/// <para>
		/// Specifies the byte offset between consecutive generic vertex attributes. If stride is 0, the generic vertex attributes are understood to be tightly packed in the array. The initial value is 0.
		/// </para>
		/// </param>
		/// <param name="pointer">
		/// <para>
		/// Specifies a offset of the first component of the first generic vertex attribute in the array in the data store of the buffer currently bound to the GL_ARRAY_BUFFER target. The initial value is 0.
		/// </para>
		/// </param>
		[System.CLSCompliant(false)]
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glVertexAttribPointer")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
		void VertexAttribPointer<T5>(UInt32 indx, Int32 size, System.Graphics.ES20.VertexAttribPointerType type, bool normalized, Int32 stride, [InAttribute, OutAttribute] ref T5 ptr)
			where T5 : struct {
			GCHandle ptr_ptr = GCHandle.Alloc(ptr, GCHandleType.Pinned);
			try {
				Delegates.glVertexAttribPointer((UInt32) indx, (Int32) size, (System.Graphics.ES20.VertexAttribPointerType) type, (bool) normalized, (Int32) stride, (IntPtr) ptr_ptr.AddrOfPinnedObject());
				ptr = (T5) ptr_ptr.Target;
			} finally {
				ptr_ptr.Free();
			}
		}


		/// <summary>[requires: v2.0 and 2.0]
		/// Set the viewport
		/// </summary>
		/// <param name="x">
		/// <para>
		/// Specify the lower left corner of the viewport rectangle, in pixels. The initial value is (0,0).
		/// </para>
		/// </param>
		/// <param name="width">
		/// <para>
		/// Specify the width and height of the viewport. When a GL context is first attached to a window, width and height are set to the dimensions of that window.
		/// </para>
		/// </param>
		//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glViewport")]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static
				void Viewport(Int32 x, Int32 y, Int32 width, Int32 height) {
			Delegates.glViewport((Int32) x, (Int32) y, (Int32) width, (Int32) height);
		}

		/// <summary>
		/// Extensions
		/// </summary>
		public static class Ext {
			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDiscardFramebufferEXT")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void DiscardFramebuffer(System.Graphics.ES20.All target, Int32 numAttachments, System.Graphics.ES20.All[] attachments) {
				unsafe
				{
					fixed (System.Graphics.ES20.All* attachments_ptr = attachments) {
						Delegates.glDiscardFramebufferEXT((System.Graphics.ES20.All) target, (Int32) numAttachments, (System.Graphics.ES20.All*) attachments_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDiscardFramebufferEXT")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void DiscardFramebuffer(System.Graphics.ES20.All target, Int32 numAttachments, ref System.Graphics.ES20.All attachments) {
				unsafe
				{
					fixed (System.Graphics.ES20.All* attachments_ptr = &attachments) {
						Delegates.glDiscardFramebufferEXT((System.Graphics.ES20.All) target, (Int32) numAttachments, (System.Graphics.ES20.All*) attachments_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDiscardFramebufferEXT")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void DiscardFramebuffer(System.Graphics.ES20.All target, Int32 numAttachments, System.Graphics.ES20.All* attachments) {
				Delegates.glDiscardFramebufferEXT((System.Graphics.ES20.All) target, (Int32) numAttachments, (System.Graphics.ES20.All*) attachments);
			}


			/// <summary>[requires: 2.0]
			/// Render multiple sets of primitives from array data
			/// </summary>
			/// <param name="mode">
			/// <para>
			/// Specifies what kind of primitives to render. Symbolic constants GL_POINTS, GL_LINE_STRIP, GL_LINE_LOOP, GL_LINES, GL_LINE_STRIP_ADJACENCY, GL_LINES_ADJACENCY, GL_TRIANGLE_STRIP, GL_TRIANGLE_FAN, GL_TRIANGLES, GL_TRIANGLE_STRIP_ADJACENCY, GL_TRIANGLES_ADJACENCY and GL_PATCHES are accepted.
			/// </para>
			/// </param>
			/// <param name="first">
			/// <para>
			/// Points to an array of starting indices in the enabled arrays.
			/// </para>
			/// </param>
			/// <param name="count">
			/// <para>
			/// Points to an array of the number of indices to be rendered.
			/// </para>
			/// </param>
			/// <param name="primcount">
			/// <para>
			/// Specifies the size of the first and count
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glMultiDrawArraysEXT")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void MultiDrawArrays(System.Graphics.ES20.All mode, Int32[] first, Int32[] count, Int32 primcount) {
				unsafe
				{
					fixed (Int32* first_ptr = first)
					fixed (Int32* count_ptr = count) {
						Delegates.glMultiDrawArraysEXT((System.Graphics.ES20.All) mode, (Int32*) first_ptr, (Int32*) count_ptr, (Int32) primcount);
					}
				}
			}


			/// <summary>[requires: 2.0]
			/// Render multiple sets of primitives from array data
			/// </summary>
			/// <param name="mode">
			/// <para>
			/// Specifies what kind of primitives to render. Symbolic constants GL_POINTS, GL_LINE_STRIP, GL_LINE_LOOP, GL_LINES, GL_LINE_STRIP_ADJACENCY, GL_LINES_ADJACENCY, GL_TRIANGLE_STRIP, GL_TRIANGLE_FAN, GL_TRIANGLES, GL_TRIANGLE_STRIP_ADJACENCY, GL_TRIANGLES_ADJACENCY and GL_PATCHES are accepted.
			/// </para>
			/// </param>
			/// <param name="first">
			/// <para>
			/// Points to an array of starting indices in the enabled arrays.
			/// </para>
			/// </param>
			/// <param name="count">
			/// <para>
			/// Points to an array of the number of indices to be rendered.
			/// </para>
			/// </param>
			/// <param name="primcount">
			/// <para>
			/// Specifies the size of the first and count
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glMultiDrawArraysEXT")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void MultiDrawArrays(System.Graphics.ES20.All mode, ref Int32 first, ref Int32 count, Int32 primcount) {
				unsafe
				{
					fixed (Int32* first_ptr = &first)
					fixed (Int32* count_ptr = &count) {
						Delegates.glMultiDrawArraysEXT((System.Graphics.ES20.All) mode, (Int32*) first_ptr, (Int32*) count_ptr, (Int32) primcount);
					}
				}
			}


			/// <summary>[requires: 2.0]
			/// Render multiple sets of primitives from array data
			/// </summary>
			/// <param name="mode">
			/// <para>
			/// Specifies what kind of primitives to render. Symbolic constants GL_POINTS, GL_LINE_STRIP, GL_LINE_LOOP, GL_LINES, GL_LINE_STRIP_ADJACENCY, GL_LINES_ADJACENCY, GL_TRIANGLE_STRIP, GL_TRIANGLE_FAN, GL_TRIANGLES, GL_TRIANGLE_STRIP_ADJACENCY, GL_TRIANGLES_ADJACENCY and GL_PATCHES are accepted.
			/// </para>
			/// </param>
			/// <param name="first">
			/// <para>
			/// Points to an array of starting indices in the enabled arrays.
			/// </para>
			/// </param>
			/// <param name="count">
			/// <para>
			/// Points to an array of the number of indices to be rendered.
			/// </para>
			/// </param>
			/// <param name="primcount">
			/// <para>
			/// Specifies the size of the first and count
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glMultiDrawArraysEXT")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void MultiDrawArrays(System.Graphics.ES20.All mode, Int32* first, Int32* count, Int32 primcount) {
				Delegates.glMultiDrawArraysEXT((System.Graphics.ES20.All) mode, (Int32*) first, (Int32*) count, (Int32) primcount);
			}


			/// <summary>[requires: 2.0]
			/// Render multiple sets of primitives by specifying indices of array data elements
			/// </summary>
			/// <param name="mode">
			/// <para>
			/// Specifies what kind of primitives to render. Symbolic constants GL_POINTS, GL_LINE_STRIP, GL_LINE_LOOP, GL_LINES, GL_LINE_STRIP_ADJACENCY, GL_LINES_ADJACENCY, GL_TRIANGLE_STRIP, GL_TRIANGLE_FAN, GL_TRIANGLES, GL_TRIANGLE_STRIP_ADJACENCY, GL_TRIANGLES_ADJACENCY and GL_PATCHES are accepted.
			/// </para>
			/// </param>
			/// <param name="count">
			/// <para>
			/// Points to an array of the elements counts.
			/// </para>
			/// </param>
			/// <param name="type">
			/// <para>
			/// Specifies the type of the values in indices. Must be one of GL_UNSIGNED_BYTE, GL_UNSIGNED_SHORT, or GL_UNSIGNED_INT.
			/// </para>
			/// </param>
			/// <param name="indices">
			/// <para>
			/// Specifies a pointer to the location where the indices are stored.
			/// </para>
			/// </param>
			/// <param name="primcount">
			/// <para>
			/// Specifies the size of the count array.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glMultiDrawElementsEXT")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void MultiDrawElements(System.Graphics.ES20.All mode, Int32[] first, System.Graphics.ES20.All type, IntPtr indices, Int32 primcount) {
				unsafe
				{
					fixed (Int32* first_ptr = first) {
						Delegates.glMultiDrawElementsEXT((System.Graphics.ES20.All) mode, (Int32*) first_ptr, (System.Graphics.ES20.All) type, (IntPtr) indices, (Int32) primcount);
					}
				}
			}


			/// <summary>[requires: 2.0]
			/// Render multiple sets of primitives by specifying indices of array data elements
			/// </summary>
			/// <param name="mode">
			/// <para>
			/// Specifies what kind of primitives to render. Symbolic constants GL_POINTS, GL_LINE_STRIP, GL_LINE_LOOP, GL_LINES, GL_LINE_STRIP_ADJACENCY, GL_LINES_ADJACENCY, GL_TRIANGLE_STRIP, GL_TRIANGLE_FAN, GL_TRIANGLES, GL_TRIANGLE_STRIP_ADJACENCY, GL_TRIANGLES_ADJACENCY and GL_PATCHES are accepted.
			/// </para>
			/// </param>
			/// <param name="count">
			/// <para>
			/// Points to an array of the elements counts.
			/// </para>
			/// </param>
			/// <param name="type">
			/// <para>
			/// Specifies the type of the values in indices. Must be one of GL_UNSIGNED_BYTE, GL_UNSIGNED_SHORT, or GL_UNSIGNED_INT.
			/// </para>
			/// </param>
			/// <param name="indices">
			/// <para>
			/// Specifies a pointer to the location where the indices are stored.
			/// </para>
			/// </param>
			/// <param name="primcount">
			/// <para>
			/// Specifies the size of the count array.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glMultiDrawElementsEXT")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void MultiDrawElements<T3>(System.Graphics.ES20.All mode, Int32[] first, System.Graphics.ES20.All type, [InAttribute, OutAttribute] T3[] indices, Int32 primcount)
							where T3 : struct {
				unsafe
				{
					fixed (Int32* first_ptr = first) {
						GCHandle indices_ptr = GCHandle.Alloc(indices, GCHandleType.Pinned);
						try {
							Delegates.glMultiDrawElementsEXT((System.Graphics.ES20.All) mode, (Int32*) first_ptr, (System.Graphics.ES20.All) type, (IntPtr) indices_ptr.AddrOfPinnedObject(), (Int32) primcount);
						} finally {
							indices_ptr.Free();
						}
					}
				}
			}


			/// <summary>[requires: 2.0]
			/// Render multiple sets of primitives by specifying indices of array data elements
			/// </summary>
			/// <param name="mode">
			/// <para>
			/// Specifies what kind of primitives to render. Symbolic constants GL_POINTS, GL_LINE_STRIP, GL_LINE_LOOP, GL_LINES, GL_LINE_STRIP_ADJACENCY, GL_LINES_ADJACENCY, GL_TRIANGLE_STRIP, GL_TRIANGLE_FAN, GL_TRIANGLES, GL_TRIANGLE_STRIP_ADJACENCY, GL_TRIANGLES_ADJACENCY and GL_PATCHES are accepted.
			/// </para>
			/// </param>
			/// <param name="count">
			/// <para>
			/// Points to an array of the elements counts.
			/// </para>
			/// </param>
			/// <param name="type">
			/// <para>
			/// Specifies the type of the values in indices. Must be one of GL_UNSIGNED_BYTE, GL_UNSIGNED_SHORT, or GL_UNSIGNED_INT.
			/// </para>
			/// </param>
			/// <param name="indices">
			/// <para>
			/// Specifies a pointer to the location where the indices are stored.
			/// </para>
			/// </param>
			/// <param name="primcount">
			/// <para>
			/// Specifies the size of the count array.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glMultiDrawElementsEXT")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void MultiDrawElements<T3>(System.Graphics.ES20.All mode, Int32[] first, System.Graphics.ES20.All type, [InAttribute, OutAttribute] T3[,] indices, Int32 primcount)
							where T3 : struct {
				unsafe
				{
					fixed (Int32* first_ptr = first) {
						GCHandle indices_ptr = GCHandle.Alloc(indices, GCHandleType.Pinned);
						try {
							Delegates.glMultiDrawElementsEXT((System.Graphics.ES20.All) mode, (Int32*) first_ptr, (System.Graphics.ES20.All) type, (IntPtr) indices_ptr.AddrOfPinnedObject(), (Int32) primcount);
						} finally {
							indices_ptr.Free();
						}
					}
				}
			}


			/// <summary>[requires: 2.0]
			/// Render multiple sets of primitives by specifying indices of array data elements
			/// </summary>
			/// <param name="mode">
			/// <para>
			/// Specifies what kind of primitives to render. Symbolic constants GL_POINTS, GL_LINE_STRIP, GL_LINE_LOOP, GL_LINES, GL_LINE_STRIP_ADJACENCY, GL_LINES_ADJACENCY, GL_TRIANGLE_STRIP, GL_TRIANGLE_FAN, GL_TRIANGLES, GL_TRIANGLE_STRIP_ADJACENCY, GL_TRIANGLES_ADJACENCY and GL_PATCHES are accepted.
			/// </para>
			/// </param>
			/// <param name="count">
			/// <para>
			/// Points to an array of the elements counts.
			/// </para>
			/// </param>
			/// <param name="type">
			/// <para>
			/// Specifies the type of the values in indices. Must be one of GL_UNSIGNED_BYTE, GL_UNSIGNED_SHORT, or GL_UNSIGNED_INT.
			/// </para>
			/// </param>
			/// <param name="indices">
			/// <para>
			/// Specifies a pointer to the location where the indices are stored.
			/// </para>
			/// </param>
			/// <param name="primcount">
			/// <para>
			/// Specifies the size of the count array.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glMultiDrawElementsEXT")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void MultiDrawElements<T3>(System.Graphics.ES20.All mode, Int32[] first, System.Graphics.ES20.All type, [InAttribute, OutAttribute] T3[,,] indices, Int32 primcount)
							where T3 : struct {
				unsafe
				{
					fixed (Int32* first_ptr = first) {
						GCHandle indices_ptr = GCHandle.Alloc(indices, GCHandleType.Pinned);
						try {
							Delegates.glMultiDrawElementsEXT((System.Graphics.ES20.All) mode, (Int32*) first_ptr, (System.Graphics.ES20.All) type, (IntPtr) indices_ptr.AddrOfPinnedObject(), (Int32) primcount);
						} finally {
							indices_ptr.Free();
						}
					}
				}
			}


			/// <summary>[requires: 2.0]
			/// Render multiple sets of primitives by specifying indices of array data elements
			/// </summary>
			/// <param name="mode">
			/// <para>
			/// Specifies what kind of primitives to render. Symbolic constants GL_POINTS, GL_LINE_STRIP, GL_LINE_LOOP, GL_LINES, GL_LINE_STRIP_ADJACENCY, GL_LINES_ADJACENCY, GL_TRIANGLE_STRIP, GL_TRIANGLE_FAN, GL_TRIANGLES, GL_TRIANGLE_STRIP_ADJACENCY, GL_TRIANGLES_ADJACENCY and GL_PATCHES are accepted.
			/// </para>
			/// </param>
			/// <param name="count">
			/// <para>
			/// Points to an array of the elements counts.
			/// </para>
			/// </param>
			/// <param name="type">
			/// <para>
			/// Specifies the type of the values in indices. Must be one of GL_UNSIGNED_BYTE, GL_UNSIGNED_SHORT, or GL_UNSIGNED_INT.
			/// </para>
			/// </param>
			/// <param name="indices">
			/// <para>
			/// Specifies a pointer to the location where the indices are stored.
			/// </para>
			/// </param>
			/// <param name="primcount">
			/// <para>
			/// Specifies the size of the count array.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glMultiDrawElementsEXT")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void MultiDrawElements<T3>(System.Graphics.ES20.All mode, Int32[] first, System.Graphics.ES20.All type, [InAttribute, OutAttribute] ref T3 indices, Int32 primcount)
							where T3 : struct {
				unsafe
				{
					fixed (Int32* first_ptr = first) {
						GCHandle indices_ptr = GCHandle.Alloc(indices, GCHandleType.Pinned);
						try {
							Delegates.glMultiDrawElementsEXT((System.Graphics.ES20.All) mode, (Int32*) first_ptr, (System.Graphics.ES20.All) type, (IntPtr) indices_ptr.AddrOfPinnedObject(), (Int32) primcount);
							indices = (T3) indices_ptr.Target;
						} finally {
							indices_ptr.Free();
						}
					}
				}
			}


			/// <summary>[requires: 2.0]
			/// Render multiple sets of primitives by specifying indices of array data elements
			/// </summary>
			/// <param name="mode">
			/// <para>
			/// Specifies what kind of primitives to render. Symbolic constants GL_POINTS, GL_LINE_STRIP, GL_LINE_LOOP, GL_LINES, GL_LINE_STRIP_ADJACENCY, GL_LINES_ADJACENCY, GL_TRIANGLE_STRIP, GL_TRIANGLE_FAN, GL_TRIANGLES, GL_TRIANGLE_STRIP_ADJACENCY, GL_TRIANGLES_ADJACENCY and GL_PATCHES are accepted.
			/// </para>
			/// </param>
			/// <param name="count">
			/// <para>
			/// Points to an array of the elements counts.
			/// </para>
			/// </param>
			/// <param name="type">
			/// <para>
			/// Specifies the type of the values in indices. Must be one of GL_UNSIGNED_BYTE, GL_UNSIGNED_SHORT, or GL_UNSIGNED_INT.
			/// </para>
			/// </param>
			/// <param name="indices">
			/// <para>
			/// Specifies a pointer to the location where the indices are stored.
			/// </para>
			/// </param>
			/// <param name="primcount">
			/// <para>
			/// Specifies the size of the count array.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glMultiDrawElementsEXT")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void MultiDrawElements(System.Graphics.ES20.All mode, ref Int32 first, System.Graphics.ES20.All type, IntPtr indices, Int32 primcount) {
				unsafe
				{
					fixed (Int32* first_ptr = &first) {
						Delegates.glMultiDrawElementsEXT((System.Graphics.ES20.All) mode, (Int32*) first_ptr, (System.Graphics.ES20.All) type, (IntPtr) indices, (Int32) primcount);
					}
				}
			}


			/// <summary>[requires: 2.0]
			/// Render multiple sets of primitives by specifying indices of array data elements
			/// </summary>
			/// <param name="mode">
			/// <para>
			/// Specifies what kind of primitives to render. Symbolic constants GL_POINTS, GL_LINE_STRIP, GL_LINE_LOOP, GL_LINES, GL_LINE_STRIP_ADJACENCY, GL_LINES_ADJACENCY, GL_TRIANGLE_STRIP, GL_TRIANGLE_FAN, GL_TRIANGLES, GL_TRIANGLE_STRIP_ADJACENCY, GL_TRIANGLES_ADJACENCY and GL_PATCHES are accepted.
			/// </para>
			/// </param>
			/// <param name="count">
			/// <para>
			/// Points to an array of the elements counts.
			/// </para>
			/// </param>
			/// <param name="type">
			/// <para>
			/// Specifies the type of the values in indices. Must be one of GL_UNSIGNED_BYTE, GL_UNSIGNED_SHORT, or GL_UNSIGNED_INT.
			/// </para>
			/// </param>
			/// <param name="indices">
			/// <para>
			/// Specifies a pointer to the location where the indices are stored.
			/// </para>
			/// </param>
			/// <param name="primcount">
			/// <para>
			/// Specifies the size of the count array.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glMultiDrawElementsEXT")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void MultiDrawElements<T3>(System.Graphics.ES20.All mode, ref Int32 first, System.Graphics.ES20.All type, [InAttribute, OutAttribute] T3[] indices, Int32 primcount)
							where T3 : struct {
				unsafe
				{
					fixed (Int32* first_ptr = &first) {
						GCHandle indices_ptr = GCHandle.Alloc(indices, GCHandleType.Pinned);
						try {
							Delegates.glMultiDrawElementsEXT((System.Graphics.ES20.All) mode, (Int32*) first_ptr, (System.Graphics.ES20.All) type, (IntPtr) indices_ptr.AddrOfPinnedObject(), (Int32) primcount);
						} finally {
							indices_ptr.Free();
						}
					}
				}
			}


			/// <summary>[requires: 2.0]
			/// Render multiple sets of primitives by specifying indices of array data elements
			/// </summary>
			/// <param name="mode">
			/// <para>
			/// Specifies what kind of primitives to render. Symbolic constants GL_POINTS, GL_LINE_STRIP, GL_LINE_LOOP, GL_LINES, GL_LINE_STRIP_ADJACENCY, GL_LINES_ADJACENCY, GL_TRIANGLE_STRIP, GL_TRIANGLE_FAN, GL_TRIANGLES, GL_TRIANGLE_STRIP_ADJACENCY, GL_TRIANGLES_ADJACENCY and GL_PATCHES are accepted.
			/// </para>
			/// </param>
			/// <param name="count">
			/// <para>
			/// Points to an array of the elements counts.
			/// </para>
			/// </param>
			/// <param name="type">
			/// <para>
			/// Specifies the type of the values in indices. Must be one of GL_UNSIGNED_BYTE, GL_UNSIGNED_SHORT, or GL_UNSIGNED_INT.
			/// </para>
			/// </param>
			/// <param name="indices">
			/// <para>
			/// Specifies a pointer to the location where the indices are stored.
			/// </para>
			/// </param>
			/// <param name="primcount">
			/// <para>
			/// Specifies the size of the count array.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glMultiDrawElementsEXT")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void MultiDrawElements<T3>(System.Graphics.ES20.All mode, ref Int32 first, System.Graphics.ES20.All type, [InAttribute, OutAttribute] T3[,] indices, Int32 primcount)
							where T3 : struct {
				unsafe
				{
					fixed (Int32* first_ptr = &first) {
						GCHandle indices_ptr = GCHandle.Alloc(indices, GCHandleType.Pinned);
						try {
							Delegates.glMultiDrawElementsEXT((System.Graphics.ES20.All) mode, (Int32*) first_ptr, (System.Graphics.ES20.All) type, (IntPtr) indices_ptr.AddrOfPinnedObject(), (Int32) primcount);
						} finally {
							indices_ptr.Free();
						}
					}
				}
			}


			/// <summary>[requires: 2.0]
			/// Render multiple sets of primitives by specifying indices of array data elements
			/// </summary>
			/// <param name="mode">
			/// <para>
			/// Specifies what kind of primitives to render. Symbolic constants GL_POINTS, GL_LINE_STRIP, GL_LINE_LOOP, GL_LINES, GL_LINE_STRIP_ADJACENCY, GL_LINES_ADJACENCY, GL_TRIANGLE_STRIP, GL_TRIANGLE_FAN, GL_TRIANGLES, GL_TRIANGLE_STRIP_ADJACENCY, GL_TRIANGLES_ADJACENCY and GL_PATCHES are accepted.
			/// </para>
			/// </param>
			/// <param name="count">
			/// <para>
			/// Points to an array of the elements counts.
			/// </para>
			/// </param>
			/// <param name="type">
			/// <para>
			/// Specifies the type of the values in indices. Must be one of GL_UNSIGNED_BYTE, GL_UNSIGNED_SHORT, or GL_UNSIGNED_INT.
			/// </para>
			/// </param>
			/// <param name="indices">
			/// <para>
			/// Specifies a pointer to the location where the indices are stored.
			/// </para>
			/// </param>
			/// <param name="primcount">
			/// <para>
			/// Specifies the size of the count array.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glMultiDrawElementsEXT")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void MultiDrawElements<T3>(System.Graphics.ES20.All mode, ref Int32 first, System.Graphics.ES20.All type, [InAttribute, OutAttribute] T3[,,] indices, Int32 primcount)
							where T3 : struct {
				unsafe
				{
					fixed (Int32* first_ptr = &first) {
						GCHandle indices_ptr = GCHandle.Alloc(indices, GCHandleType.Pinned);
						try {
							Delegates.glMultiDrawElementsEXT((System.Graphics.ES20.All) mode, (Int32*) first_ptr, (System.Graphics.ES20.All) type, (IntPtr) indices_ptr.AddrOfPinnedObject(), (Int32) primcount);
						} finally {
							indices_ptr.Free();
						}
					}
				}
			}


			/// <summary>[requires: 2.0]
			/// Render multiple sets of primitives by specifying indices of array data elements
			/// </summary>
			/// <param name="mode">
			/// <para>
			/// Specifies what kind of primitives to render. Symbolic constants GL_POINTS, GL_LINE_STRIP, GL_LINE_LOOP, GL_LINES, GL_LINE_STRIP_ADJACENCY, GL_LINES_ADJACENCY, GL_TRIANGLE_STRIP, GL_TRIANGLE_FAN, GL_TRIANGLES, GL_TRIANGLE_STRIP_ADJACENCY, GL_TRIANGLES_ADJACENCY and GL_PATCHES are accepted.
			/// </para>
			/// </param>
			/// <param name="count">
			/// <para>
			/// Points to an array of the elements counts.
			/// </para>
			/// </param>
			/// <param name="type">
			/// <para>
			/// Specifies the type of the values in indices. Must be one of GL_UNSIGNED_BYTE, GL_UNSIGNED_SHORT, or GL_UNSIGNED_INT.
			/// </para>
			/// </param>
			/// <param name="indices">
			/// <para>
			/// Specifies a pointer to the location where the indices are stored.
			/// </para>
			/// </param>
			/// <param name="primcount">
			/// <para>
			/// Specifies the size of the count array.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glMultiDrawElementsEXT")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void MultiDrawElements<T3>(System.Graphics.ES20.All mode, ref Int32 first, System.Graphics.ES20.All type, [InAttribute, OutAttribute] ref T3 indices, Int32 primcount)
							where T3 : struct {
				unsafe
				{
					fixed (Int32* first_ptr = &first) {
						GCHandle indices_ptr = GCHandle.Alloc(indices, GCHandleType.Pinned);
						try {
							Delegates.glMultiDrawElementsEXT((System.Graphics.ES20.All) mode, (Int32*) first_ptr, (System.Graphics.ES20.All) type, (IntPtr) indices_ptr.AddrOfPinnedObject(), (Int32) primcount);
							indices = (T3) indices_ptr.Target;
						} finally {
							indices_ptr.Free();
						}
					}
				}
			}


			/// <summary>[requires: 2.0]
			/// Render multiple sets of primitives by specifying indices of array data elements
			/// </summary>
			/// <param name="mode">
			/// <para>
			/// Specifies what kind of primitives to render. Symbolic constants GL_POINTS, GL_LINE_STRIP, GL_LINE_LOOP, GL_LINES, GL_LINE_STRIP_ADJACENCY, GL_LINES_ADJACENCY, GL_TRIANGLE_STRIP, GL_TRIANGLE_FAN, GL_TRIANGLES, GL_TRIANGLE_STRIP_ADJACENCY, GL_TRIANGLES_ADJACENCY and GL_PATCHES are accepted.
			/// </para>
			/// </param>
			/// <param name="count">
			/// <para>
			/// Points to an array of the elements counts.
			/// </para>
			/// </param>
			/// <param name="type">
			/// <para>
			/// Specifies the type of the values in indices. Must be one of GL_UNSIGNED_BYTE, GL_UNSIGNED_SHORT, or GL_UNSIGNED_INT.
			/// </para>
			/// </param>
			/// <param name="indices">
			/// <para>
			/// Specifies a pointer to the location where the indices are stored.
			/// </para>
			/// </param>
			/// <param name="primcount">
			/// <para>
			/// Specifies the size of the count array.
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glMultiDrawElementsEXT")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void MultiDrawElements(System.Graphics.ES20.All mode, Int32* first, System.Graphics.ES20.All type, IntPtr indices, Int32 primcount) {
				Delegates.glMultiDrawElementsEXT((System.Graphics.ES20.All) mode, (Int32*) first, (System.Graphics.ES20.All) type, (IntPtr) indices, (Int32) primcount);
			}


			/// <summary>[requires: 2.0]
			/// Render multiple sets of primitives by specifying indices of array data elements
			/// </summary>
			/// <param name="mode">
			/// <para>
			/// Specifies what kind of primitives to render. Symbolic constants GL_POINTS, GL_LINE_STRIP, GL_LINE_LOOP, GL_LINES, GL_LINE_STRIP_ADJACENCY, GL_LINES_ADJACENCY, GL_TRIANGLE_STRIP, GL_TRIANGLE_FAN, GL_TRIANGLES, GL_TRIANGLE_STRIP_ADJACENCY, GL_TRIANGLES_ADJACENCY and GL_PATCHES are accepted.
			/// </para>
			/// </param>
			/// <param name="count">
			/// <para>
			/// Points to an array of the elements counts.
			/// </para>
			/// </param>
			/// <param name="type">
			/// <para>
			/// Specifies the type of the values in indices. Must be one of GL_UNSIGNED_BYTE, GL_UNSIGNED_SHORT, or GL_UNSIGNED_INT.
			/// </para>
			/// </param>
			/// <param name="indices">
			/// <para>
			/// Specifies a pointer to the location where the indices are stored.
			/// </para>
			/// </param>
			/// <param name="primcount">
			/// <para>
			/// Specifies the size of the count array.
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glMultiDrawElementsEXT")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void MultiDrawElements<T3>(System.Graphics.ES20.All mode, Int32* first, System.Graphics.ES20.All type, [InAttribute, OutAttribute] T3[] indices, Int32 primcount)
				where T3 : struct {
				GCHandle indices_ptr = GCHandle.Alloc(indices, GCHandleType.Pinned);
				try {
					Delegates.glMultiDrawElementsEXT((System.Graphics.ES20.All) mode, (Int32*) first, (System.Graphics.ES20.All) type, (IntPtr) indices_ptr.AddrOfPinnedObject(), (Int32) primcount);
				} finally {
					indices_ptr.Free();
				}
			}


			/// <summary>[requires: 2.0]
			/// Render multiple sets of primitives by specifying indices of array data elements
			/// </summary>
			/// <param name="mode">
			/// <para>
			/// Specifies what kind of primitives to render. Symbolic constants GL_POINTS, GL_LINE_STRIP, GL_LINE_LOOP, GL_LINES, GL_LINE_STRIP_ADJACENCY, GL_LINES_ADJACENCY, GL_TRIANGLE_STRIP, GL_TRIANGLE_FAN, GL_TRIANGLES, GL_TRIANGLE_STRIP_ADJACENCY, GL_TRIANGLES_ADJACENCY and GL_PATCHES are accepted.
			/// </para>
			/// </param>
			/// <param name="count">
			/// <para>
			/// Points to an array of the elements counts.
			/// </para>
			/// </param>
			/// <param name="type">
			/// <para>
			/// Specifies the type of the values in indices. Must be one of GL_UNSIGNED_BYTE, GL_UNSIGNED_SHORT, or GL_UNSIGNED_INT.
			/// </para>
			/// </param>
			/// <param name="indices">
			/// <para>
			/// Specifies a pointer to the location where the indices are stored.
			/// </para>
			/// </param>
			/// <param name="primcount">
			/// <para>
			/// Specifies the size of the count array.
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glMultiDrawElementsEXT")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void MultiDrawElements<T3>(System.Graphics.ES20.All mode, Int32* first, System.Graphics.ES20.All type, [InAttribute, OutAttribute] T3[,] indices, Int32 primcount)
				where T3 : struct {
				GCHandle indices_ptr = GCHandle.Alloc(indices, GCHandleType.Pinned);
				try {
					Delegates.glMultiDrawElementsEXT((System.Graphics.ES20.All) mode, (Int32*) first, (System.Graphics.ES20.All) type, (IntPtr) indices_ptr.AddrOfPinnedObject(), (Int32) primcount);
				} finally {
					indices_ptr.Free();
				}
			}


			/// <summary>[requires: 2.0]
			/// Render multiple sets of primitives by specifying indices of array data elements
			/// </summary>
			/// <param name="mode">
			/// <para>
			/// Specifies what kind of primitives to render. Symbolic constants GL_POINTS, GL_LINE_STRIP, GL_LINE_LOOP, GL_LINES, GL_LINE_STRIP_ADJACENCY, GL_LINES_ADJACENCY, GL_TRIANGLE_STRIP, GL_TRIANGLE_FAN, GL_TRIANGLES, GL_TRIANGLE_STRIP_ADJACENCY, GL_TRIANGLES_ADJACENCY and GL_PATCHES are accepted.
			/// </para>
			/// </param>
			/// <param name="count">
			/// <para>
			/// Points to an array of the elements counts.
			/// </para>
			/// </param>
			/// <param name="type">
			/// <para>
			/// Specifies the type of the values in indices. Must be one of GL_UNSIGNED_BYTE, GL_UNSIGNED_SHORT, or GL_UNSIGNED_INT.
			/// </para>
			/// </param>
			/// <param name="indices">
			/// <para>
			/// Specifies a pointer to the location where the indices are stored.
			/// </para>
			/// </param>
			/// <param name="primcount">
			/// <para>
			/// Specifies the size of the count array.
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glMultiDrawElementsEXT")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void MultiDrawElements<T3>(System.Graphics.ES20.All mode, Int32* first, System.Graphics.ES20.All type, [InAttribute, OutAttribute] T3[,,] indices, Int32 primcount)
				where T3 : struct {
				GCHandle indices_ptr = GCHandle.Alloc(indices, GCHandleType.Pinned);
				try {
					Delegates.glMultiDrawElementsEXT((System.Graphics.ES20.All) mode, (Int32*) first, (System.Graphics.ES20.All) type, (IntPtr) indices_ptr.AddrOfPinnedObject(), (Int32) primcount);
				} finally {
					indices_ptr.Free();
				}
			}


			/// <summary>[requires: 2.0]
			/// Render multiple sets of primitives by specifying indices of array data elements
			/// </summary>
			/// <param name="mode">
			/// <para>
			/// Specifies what kind of primitives to render. Symbolic constants GL_POINTS, GL_LINE_STRIP, GL_LINE_LOOP, GL_LINES, GL_LINE_STRIP_ADJACENCY, GL_LINES_ADJACENCY, GL_TRIANGLE_STRIP, GL_TRIANGLE_FAN, GL_TRIANGLES, GL_TRIANGLE_STRIP_ADJACENCY, GL_TRIANGLES_ADJACENCY and GL_PATCHES are accepted.
			/// </para>
			/// </param>
			/// <param name="count">
			/// <para>
			/// Points to an array of the elements counts.
			/// </para>
			/// </param>
			/// <param name="type">
			/// <para>
			/// Specifies the type of the values in indices. Must be one of GL_UNSIGNED_BYTE, GL_UNSIGNED_SHORT, or GL_UNSIGNED_INT.
			/// </para>
			/// </param>
			/// <param name="indices">
			/// <para>
			/// Specifies a pointer to the location where the indices are stored.
			/// </para>
			/// </param>
			/// <param name="primcount">
			/// <para>
			/// Specifies the size of the count array.
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glMultiDrawElementsEXT")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void MultiDrawElements<T3>(System.Graphics.ES20.All mode, Int32* first, System.Graphics.ES20.All type, [InAttribute, OutAttribute] ref T3 indices, Int32 primcount)
				where T3 : struct {
				GCHandle indices_ptr = GCHandle.Alloc(indices, GCHandleType.Pinned);
				try {
					Delegates.glMultiDrawElementsEXT((System.Graphics.ES20.All) mode, (Int32*) first, (System.Graphics.ES20.All) type, (IntPtr) indices_ptr.AddrOfPinnedObject(), (Int32) primcount);
					indices = (T3) indices_ptr.Target;
				} finally {
					indices_ptr.Free();
				}
			}

		}

		/// <summary>
		/// Nvidia extensions
		/// </summary>
		public static class NV {
			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glCoverageMaskNV")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void CoverageMask(bool mask) {
				Delegates.glCoverageMaskNV((bool) mask);
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glCoverageOperationNV")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void CoverageOperation(System.Graphics.ES20.All operation) {
				Delegates.glCoverageOperationNV((System.Graphics.ES20.All) operation);
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeleteFencesNV")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void DeleteFences(Int32 n, Int32[] fences) {
				unsafe
				{
					fixed (Int32* fences_ptr = fences) {
						Delegates.glDeleteFencesNV((Int32) n, (UInt32*) fences_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeleteFencesNV")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void DeleteFences(Int32 n, ref Int32 fences) {
				unsafe
				{
					fixed (Int32* fences_ptr = &fences) {
						Delegates.glDeleteFencesNV((Int32) n, (UInt32*) fences_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeleteFencesNV")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void DeleteFences(Int32 n, Int32* fences) {
				Delegates.glDeleteFencesNV((Int32) n, (UInt32*) fences);
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeleteFencesNV")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void DeleteFences(Int32 n, UInt32[] fences) {
				unsafe
				{
					fixed (UInt32* fences_ptr = fences) {
						Delegates.glDeleteFencesNV((Int32) n, (UInt32*) fences_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeleteFencesNV")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void DeleteFences(Int32 n, ref UInt32 fences) {
				unsafe
				{
					fixed (UInt32* fences_ptr = &fences) {
						Delegates.glDeleteFencesNV((Int32) n, (UInt32*) fences_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeleteFencesNV")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void DeleteFences(Int32 n, UInt32* fences) {
				Delegates.glDeleteFencesNV((Int32) n, (UInt32*) fences);
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glFinishFenceNV")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void FinishFence(Int32 fence) {
				Delegates.glFinishFenceNV((UInt32) fence);
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glFinishFenceNV")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void FinishFence(UInt32 fence) {
				Delegates.glFinishFenceNV((UInt32) fence);
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenFencesNV")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void GenFences(Int32 n, [OutAttribute] Int32[] fences) {
				unsafe
				{
					fixed (Int32* fences_ptr = fences) {
						Delegates.glGenFencesNV((Int32) n, (UInt32*) fences_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenFencesNV")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void GenFences(Int32 n, [OutAttribute] out Int32 fences) {
				unsafe
				{
					fixed (Int32* fences_ptr = &fences) {
						Delegates.glGenFencesNV((Int32) n, (UInt32*) fences_ptr);
						fences = *fences_ptr;
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenFencesNV")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void GenFences(Int32 n, [OutAttribute] Int32* fences) {
				Delegates.glGenFencesNV((Int32) n, (UInt32*) fences);
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenFencesNV")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void GenFences(Int32 n, [OutAttribute] UInt32[] fences) {
				unsafe
				{
					fixed (UInt32* fences_ptr = fences) {
						Delegates.glGenFencesNV((Int32) n, (UInt32*) fences_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenFencesNV")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void GenFences(Int32 n, [OutAttribute] out UInt32 fences) {
				unsafe
				{
					fixed (UInt32* fences_ptr = &fences) {
						Delegates.glGenFencesNV((Int32) n, (UInt32*) fences_ptr);
						fences = *fences_ptr;
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenFencesNV")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void GenFences(Int32 n, [OutAttribute] UInt32* fences) {
				Delegates.glGenFencesNV((Int32) n, (UInt32*) fences);
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetFenceivNV")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void GetFence(Int32 fence, System.Graphics.ES20.All pname, [OutAttribute] Int32[] @params) {
				unsafe
				{
					fixed (Int32* @params_ptr = @params) {
						Delegates.glGetFenceivNV((UInt32) fence, (System.Graphics.ES20.All) pname, (Int32*) @params_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetFenceivNV")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void GetFence(Int32 fence, System.Graphics.ES20.All pname, [OutAttribute] out Int32 @params) {
				unsafe
				{
					fixed (Int32* @params_ptr = &@params) {
						Delegates.glGetFenceivNV((UInt32) fence, (System.Graphics.ES20.All) pname, (Int32*) @params_ptr);
						@params = *@params_ptr;
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetFenceivNV")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void GetFence(Int32 fence, System.Graphics.ES20.All pname, [OutAttribute] Int32* @params) {
				Delegates.glGetFenceivNV((UInt32) fence, (System.Graphics.ES20.All) pname, (Int32*) @params);
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetFenceivNV")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void GetFence(UInt32 fence, System.Graphics.ES20.All pname, [OutAttribute] Int32[] @params) {
				unsafe
				{
					fixed (Int32* @params_ptr = @params) {
						Delegates.glGetFenceivNV((UInt32) fence, (System.Graphics.ES20.All) pname, (Int32*) @params_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetFenceivNV")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void GetFence(UInt32 fence, System.Graphics.ES20.All pname, [OutAttribute] out Int32 @params) {
				unsafe
				{
					fixed (Int32* @params_ptr = &@params) {
						Delegates.glGetFenceivNV((UInt32) fence, (System.Graphics.ES20.All) pname, (Int32*) @params_ptr);
						@params = *@params_ptr;
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetFenceivNV")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void GetFence(UInt32 fence, System.Graphics.ES20.All pname, [OutAttribute] Int32* @params) {
				Delegates.glGetFenceivNV((UInt32) fence, (System.Graphics.ES20.All) pname, (Int32*) @params);
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glIsFenceNV")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						bool IsFence(Int32 fence) {
				return Delegates.glIsFenceNV((UInt32) fence);
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glIsFenceNV")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			bool IsFence(UInt32 fence) {
				return Delegates.glIsFenceNV((UInt32) fence);
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glSetFenceNV")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void SetFence(Int32 fence, System.Graphics.ES20.All condition) {
				Delegates.glSetFenceNV((UInt32) fence, (System.Graphics.ES20.All) condition);
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glSetFenceNV")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void SetFence(UInt32 fence, System.Graphics.ES20.All condition) {
				Delegates.glSetFenceNV((UInt32) fence, (System.Graphics.ES20.All) condition);
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glTestFenceNV")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						bool TestFence(Int32 fence) {
				return Delegates.glTestFenceNV((UInt32) fence);
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glTestFenceNV")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			bool TestFence(UInt32 fence) {
				return Delegates.glTestFenceNV((UInt32) fence);
			}

		}

		/// <summary>
		/// Oes extensions
		/// </summary>
		public static class Oes {

			/// <summary>[requires: 2.0]
			/// Bind a vertex array object
			/// </summary>
			/// <param name="array">
			/// <para>
			/// Specifies the name of the vertex array to bind.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glBindVertexArrayOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void BindVertexArray(Int32 array) {
				Delegates.glBindVertexArrayOES((UInt32) array);
			}


			/// <summary>[requires: 2.0]
			/// Bind a vertex array object
			/// </summary>
			/// <param name="array">
			/// <para>
			/// Specifies the name of the vertex array to bind.
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glBindVertexArrayOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void BindVertexArray(UInt32 array) {
				Delegates.glBindVertexArrayOES((UInt32) array);
			}


			/// <summary>[requires: 2.0]
			/// Specify a three-dimensional texture image in a compressed format
			/// </summary>
			/// <param name="target">
			/// <para>
			/// Specifies the target texture. Must be GL_TEXTURE_3D, GL_PROXY_TEXTURE_3D, GL_TEXTURE_2D_ARRAY or GL_PROXY_TEXTURE_2D_ARRAY.
			/// </para>
			/// </param>
			/// <param name="level">
			/// <para>
			/// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap reduction image.
			/// </para>
			/// </param>
			/// <param name="internalformat">
			/// <para>
			/// Specifies the format of the compressed image data stored at address data.
			/// </para>
			/// </param>
			/// <param name="width">
			/// <para>
			/// Specifies the width of the texture image. All implementations support 3D texture images that are at least 16 texels wide.
			/// </para>
			/// </param>
			/// <param name="height">
			/// <para>
			/// Specifies the height of the texture image. All implementations support 3D texture images that are at least 16 texels high.
			/// </para>
			/// </param>
			/// <param name="depth">
			/// <para>
			/// Specifies the depth of the texture image. All implementations support 3D texture images that are at least 16 texels deep.
			/// </para>
			/// </param>
			/// <param name="border">
			/// <para>
			/// This value must be 0.
			/// </para>
			/// </param>
			/// <param name="imageSize">
			/// <para>
			/// Specifies the number of unsigned bytes of image data starting at the address specified by data.
			/// </para>
			/// </param>
			/// <param name="data">
			/// <para>
			/// Specifies a pointer to the compressed image data in memory.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glCompressedTexImage3DOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void CompressedTexImage3D(System.Graphics.ES20.All target, Int32 level, System.Graphics.ES20.All internalformat, Int32 width, Int32 height, Int32 depth, Int32 border, Int32 imageSize, IntPtr data) {
				Delegates.glCompressedTexImage3DOES((System.Graphics.ES20.All) target, (Int32) level, (System.Graphics.ES20.All) internalformat, (Int32) width, (Int32) height, (Int32) depth, (Int32) border, (Int32) imageSize, (IntPtr) data);
			}


			/// <summary>[requires: 2.0]
			/// Specify a three-dimensional texture image in a compressed format
			/// </summary>
			/// <param name="target">
			/// <para>
			/// Specifies the target texture. Must be GL_TEXTURE_3D, GL_PROXY_TEXTURE_3D, GL_TEXTURE_2D_ARRAY or GL_PROXY_TEXTURE_2D_ARRAY.
			/// </para>
			/// </param>
			/// <param name="level">
			/// <para>
			/// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap reduction image.
			/// </para>
			/// </param>
			/// <param name="internalformat">
			/// <para>
			/// Specifies the format of the compressed image data stored at address data.
			/// </para>
			/// </param>
			/// <param name="width">
			/// <para>
			/// Specifies the width of the texture image. All implementations support 3D texture images that are at least 16 texels wide.
			/// </para>
			/// </param>
			/// <param name="height">
			/// <para>
			/// Specifies the height of the texture image. All implementations support 3D texture images that are at least 16 texels high.
			/// </para>
			/// </param>
			/// <param name="depth">
			/// <para>
			/// Specifies the depth of the texture image. All implementations support 3D texture images that are at least 16 texels deep.
			/// </para>
			/// </param>
			/// <param name="border">
			/// <para>
			/// This value must be 0.
			/// </para>
			/// </param>
			/// <param name="imageSize">
			/// <para>
			/// Specifies the number of unsigned bytes of image data starting at the address specified by data.
			/// </para>
			/// </param>
			/// <param name="data">
			/// <para>
			/// Specifies a pointer to the compressed image data in memory.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glCompressedTexImage3DOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void CompressedTexImage3D<T8>(System.Graphics.ES20.All target, Int32 level, System.Graphics.ES20.All internalformat, Int32 width, Int32 height, Int32 depth, Int32 border, Int32 imageSize, [InAttribute, OutAttribute] T8[] data)
							where T8 : struct {
				GCHandle data_ptr = GCHandle.Alloc(data, GCHandleType.Pinned);
				try {
					Delegates.glCompressedTexImage3DOES((System.Graphics.ES20.All) target, (Int32) level, (System.Graphics.ES20.All) internalformat, (Int32) width, (Int32) height, (Int32) depth, (Int32) border, (Int32) imageSize, (IntPtr) data_ptr.AddrOfPinnedObject());
				} finally {
					data_ptr.Free();
				}
			}


			/// <summary>[requires: 2.0]
			/// Specify a three-dimensional texture image in a compressed format
			/// </summary>
			/// <param name="target">
			/// <para>
			/// Specifies the target texture. Must be GL_TEXTURE_3D, GL_PROXY_TEXTURE_3D, GL_TEXTURE_2D_ARRAY or GL_PROXY_TEXTURE_2D_ARRAY.
			/// </para>
			/// </param>
			/// <param name="level">
			/// <para>
			/// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap reduction image.
			/// </para>
			/// </param>
			/// <param name="internalformat">
			/// <para>
			/// Specifies the format of the compressed image data stored at address data.
			/// </para>
			/// </param>
			/// <param name="width">
			/// <para>
			/// Specifies the width of the texture image. All implementations support 3D texture images that are at least 16 texels wide.
			/// </para>
			/// </param>
			/// <param name="height">
			/// <para>
			/// Specifies the height of the texture image. All implementations support 3D texture images that are at least 16 texels high.
			/// </para>
			/// </param>
			/// <param name="depth">
			/// <para>
			/// Specifies the depth of the texture image. All implementations support 3D texture images that are at least 16 texels deep.
			/// </para>
			/// </param>
			/// <param name="border">
			/// <para>
			/// This value must be 0.
			/// </para>
			/// </param>
			/// <param name="imageSize">
			/// <para>
			/// Specifies the number of unsigned bytes of image data starting at the address specified by data.
			/// </para>
			/// </param>
			/// <param name="data">
			/// <para>
			/// Specifies a pointer to the compressed image data in memory.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glCompressedTexImage3DOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void CompressedTexImage3D<T8>(System.Graphics.ES20.All target, Int32 level, System.Graphics.ES20.All internalformat, Int32 width, Int32 height, Int32 depth, Int32 border, Int32 imageSize, [InAttribute, OutAttribute] T8[,] data)
							where T8 : struct {
				GCHandle data_ptr = GCHandle.Alloc(data, GCHandleType.Pinned);
				try {
					Delegates.glCompressedTexImage3DOES((System.Graphics.ES20.All) target, (Int32) level, (System.Graphics.ES20.All) internalformat, (Int32) width, (Int32) height, (Int32) depth, (Int32) border, (Int32) imageSize, (IntPtr) data_ptr.AddrOfPinnedObject());
				} finally {
					data_ptr.Free();
				}
			}


			/// <summary>[requires: 2.0]
			/// Specify a three-dimensional texture image in a compressed format
			/// </summary>
			/// <param name="target">
			/// <para>
			/// Specifies the target texture. Must be GL_TEXTURE_3D, GL_PROXY_TEXTURE_3D, GL_TEXTURE_2D_ARRAY or GL_PROXY_TEXTURE_2D_ARRAY.
			/// </para>
			/// </param>
			/// <param name="level">
			/// <para>
			/// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap reduction image.
			/// </para>
			/// </param>
			/// <param name="internalformat">
			/// <para>
			/// Specifies the format of the compressed image data stored at address data.
			/// </para>
			/// </param>
			/// <param name="width">
			/// <para>
			/// Specifies the width of the texture image. All implementations support 3D texture images that are at least 16 texels wide.
			/// </para>
			/// </param>
			/// <param name="height">
			/// <para>
			/// Specifies the height of the texture image. All implementations support 3D texture images that are at least 16 texels high.
			/// </para>
			/// </param>
			/// <param name="depth">
			/// <para>
			/// Specifies the depth of the texture image. All implementations support 3D texture images that are at least 16 texels deep.
			/// </para>
			/// </param>
			/// <param name="border">
			/// <para>
			/// This value must be 0.
			/// </para>
			/// </param>
			/// <param name="imageSize">
			/// <para>
			/// Specifies the number of unsigned bytes of image data starting at the address specified by data.
			/// </para>
			/// </param>
			/// <param name="data">
			/// <para>
			/// Specifies a pointer to the compressed image data in memory.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glCompressedTexImage3DOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void CompressedTexImage3D<T8>(System.Graphics.ES20.All target, Int32 level, System.Graphics.ES20.All internalformat, Int32 width, Int32 height, Int32 depth, Int32 border, Int32 imageSize, [InAttribute, OutAttribute] T8[,,] data)
							where T8 : struct {
				GCHandle data_ptr = GCHandle.Alloc(data, GCHandleType.Pinned);
				try {
					Delegates.glCompressedTexImage3DOES((System.Graphics.ES20.All) target, (Int32) level, (System.Graphics.ES20.All) internalformat, (Int32) width, (Int32) height, (Int32) depth, (Int32) border, (Int32) imageSize, (IntPtr) data_ptr.AddrOfPinnedObject());
				} finally {
					data_ptr.Free();
				}
			}


			/// <summary>[requires: 2.0]
			/// Specify a three-dimensional texture image in a compressed format
			/// </summary>
			/// <param name="target">
			/// <para>
			/// Specifies the target texture. Must be GL_TEXTURE_3D, GL_PROXY_TEXTURE_3D, GL_TEXTURE_2D_ARRAY or GL_PROXY_TEXTURE_2D_ARRAY.
			/// </para>
			/// </param>
			/// <param name="level">
			/// <para>
			/// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap reduction image.
			/// </para>
			/// </param>
			/// <param name="internalformat">
			/// <para>
			/// Specifies the format of the compressed image data stored at address data.
			/// </para>
			/// </param>
			/// <param name="width">
			/// <para>
			/// Specifies the width of the texture image. All implementations support 3D texture images that are at least 16 texels wide.
			/// </para>
			/// </param>
			/// <param name="height">
			/// <para>
			/// Specifies the height of the texture image. All implementations support 3D texture images that are at least 16 texels high.
			/// </para>
			/// </param>
			/// <param name="depth">
			/// <para>
			/// Specifies the depth of the texture image. All implementations support 3D texture images that are at least 16 texels deep.
			/// </para>
			/// </param>
			/// <param name="border">
			/// <para>
			/// This value must be 0.
			/// </para>
			/// </param>
			/// <param name="imageSize">
			/// <para>
			/// Specifies the number of unsigned bytes of image data starting at the address specified by data.
			/// </para>
			/// </param>
			/// <param name="data">
			/// <para>
			/// Specifies a pointer to the compressed image data in memory.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glCompressedTexImage3DOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void CompressedTexImage3D<T8>(System.Graphics.ES20.All target, Int32 level, System.Graphics.ES20.All internalformat, Int32 width, Int32 height, Int32 depth, Int32 border, Int32 imageSize, [InAttribute, OutAttribute] ref T8 data)
							where T8 : struct {
				GCHandle data_ptr = GCHandle.Alloc(data, GCHandleType.Pinned);
				try {
					Delegates.glCompressedTexImage3DOES((System.Graphics.ES20.All) target, (Int32) level, (System.Graphics.ES20.All) internalformat, (Int32) width, (Int32) height, (Int32) depth, (Int32) border, (Int32) imageSize, (IntPtr) data_ptr.AddrOfPinnedObject());
					data = (T8) data_ptr.Target;
				} finally {
					data_ptr.Free();
				}
			}


			/// <summary>[requires: 2.0]
			/// Specify a three-dimensional texture subimage in a compressed format
			/// </summary>
			/// <param name="target">
			/// <para>
			/// Specifies the target texture. Must be GL_TEXTURE_3D.
			/// </para>
			/// </param>
			/// <param name="level">
			/// <para>
			/// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap reduction image.
			/// </para>
			/// </param>
			/// <param name="xoffset">
			/// <para>
			/// Specifies a texel offset in the x direction within the texture array.
			/// </para>
			/// </param>
			/// <param name="yoffset">
			/// <para>
			/// Specifies a texel offset in the y direction within the texture array.
			/// </para>
			/// </param>
			/// <param name="width">
			/// <para>
			/// Specifies the width of the texture subimage.
			/// </para>
			/// </param>
			/// <param name="height">
			/// <para>
			/// Specifies the height of the texture subimage.
			/// </para>
			/// </param>
			/// <param name="depth">
			/// <para>
			/// Specifies the depth of the texture subimage.
			/// </para>
			/// </param>
			/// <param name="format">
			/// <para>
			/// Specifies the format of the compressed image data stored at address data.
			/// </para>
			/// </param>
			/// <param name="imageSize">
			/// <para>
			/// Specifies the number of unsigned bytes of image data starting at the address specified by data.
			/// </para>
			/// </param>
			/// <param name="data">
			/// <para>
			/// Specifies a pointer to the compressed image data in memory.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glCompressedTexSubImage3DOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void CompressedTexSubImage3D(System.Graphics.ES20.All target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 width, Int32 height, Int32 depth, System.Graphics.ES20.All format, Int32 imageSize, IntPtr data) {
				Delegates.glCompressedTexSubImage3DOES((System.Graphics.ES20.All) target, (Int32) level, (Int32) xoffset, (Int32) yoffset, (Int32) zoffset, (Int32) width, (Int32) height, (Int32) depth, (System.Graphics.ES20.All) format, (Int32) imageSize, (IntPtr) data);
			}


			/// <summary>[requires: 2.0]
			/// Specify a three-dimensional texture subimage in a compressed format
			/// </summary>
			/// <param name="target">
			/// <para>
			/// Specifies the target texture. Must be GL_TEXTURE_3D.
			/// </para>
			/// </param>
			/// <param name="level">
			/// <para>
			/// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap reduction image.
			/// </para>
			/// </param>
			/// <param name="xoffset">
			/// <para>
			/// Specifies a texel offset in the x direction within the texture array.
			/// </para>
			/// </param>
			/// <param name="yoffset">
			/// <para>
			/// Specifies a texel offset in the y direction within the texture array.
			/// </para>
			/// </param>
			/// <param name="width">
			/// <para>
			/// Specifies the width of the texture subimage.
			/// </para>
			/// </param>
			/// <param name="height">
			/// <para>
			/// Specifies the height of the texture subimage.
			/// </para>
			/// </param>
			/// <param name="depth">
			/// <para>
			/// Specifies the depth of the texture subimage.
			/// </para>
			/// </param>
			/// <param name="format">
			/// <para>
			/// Specifies the format of the compressed image data stored at address data.
			/// </para>
			/// </param>
			/// <param name="imageSize">
			/// <para>
			/// Specifies the number of unsigned bytes of image data starting at the address specified by data.
			/// </para>
			/// </param>
			/// <param name="data">
			/// <para>
			/// Specifies a pointer to the compressed image data in memory.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glCompressedTexSubImage3DOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void CompressedTexSubImage3D<T10>(System.Graphics.ES20.All target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 width, Int32 height, Int32 depth, System.Graphics.ES20.All format, Int32 imageSize, [InAttribute, OutAttribute] T10[] data)
							where T10 : struct {
				GCHandle data_ptr = GCHandle.Alloc(data, GCHandleType.Pinned);
				try {
					Delegates.glCompressedTexSubImage3DOES((System.Graphics.ES20.All) target, (Int32) level, (Int32) xoffset, (Int32) yoffset, (Int32) zoffset, (Int32) width, (Int32) height, (Int32) depth, (System.Graphics.ES20.All) format, (Int32) imageSize, (IntPtr) data_ptr.AddrOfPinnedObject());
				} finally {
					data_ptr.Free();
				}
			}


			/// <summary>[requires: 2.0]
			/// Specify a three-dimensional texture subimage in a compressed format
			/// </summary>
			/// <param name="target">
			/// <para>
			/// Specifies the target texture. Must be GL_TEXTURE_3D.
			/// </para>
			/// </param>
			/// <param name="level">
			/// <para>
			/// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap reduction image.
			/// </para>
			/// </param>
			/// <param name="xoffset">
			/// <para>
			/// Specifies a texel offset in the x direction within the texture array.
			/// </para>
			/// </param>
			/// <param name="yoffset">
			/// <para>
			/// Specifies a texel offset in the y direction within the texture array.
			/// </para>
			/// </param>
			/// <param name="width">
			/// <para>
			/// Specifies the width of the texture subimage.
			/// </para>
			/// </param>
			/// <param name="height">
			/// <para>
			/// Specifies the height of the texture subimage.
			/// </para>
			/// </param>
			/// <param name="depth">
			/// <para>
			/// Specifies the depth of the texture subimage.
			/// </para>
			/// </param>
			/// <param name="format">
			/// <para>
			/// Specifies the format of the compressed image data stored at address data.
			/// </para>
			/// </param>
			/// <param name="imageSize">
			/// <para>
			/// Specifies the number of unsigned bytes of image data starting at the address specified by data.
			/// </para>
			/// </param>
			/// <param name="data">
			/// <para>
			/// Specifies a pointer to the compressed image data in memory.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glCompressedTexSubImage3DOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void CompressedTexSubImage3D<T10>(System.Graphics.ES20.All target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 width, Int32 height, Int32 depth, System.Graphics.ES20.All format, Int32 imageSize, [InAttribute, OutAttribute] T10[,] data)
							where T10 : struct {
				GCHandle data_ptr = GCHandle.Alloc(data, GCHandleType.Pinned);
				try {
					Delegates.glCompressedTexSubImage3DOES((System.Graphics.ES20.All) target, (Int32) level, (Int32) xoffset, (Int32) yoffset, (Int32) zoffset, (Int32) width, (Int32) height, (Int32) depth, (System.Graphics.ES20.All) format, (Int32) imageSize, (IntPtr) data_ptr.AddrOfPinnedObject());
				} finally {
					data_ptr.Free();
				}
			}


			/// <summary>[requires: 2.0]
			/// Specify a three-dimensional texture subimage in a compressed format
			/// </summary>
			/// <param name="target">
			/// <para>
			/// Specifies the target texture. Must be GL_TEXTURE_3D.
			/// </para>
			/// </param>
			/// <param name="level">
			/// <para>
			/// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap reduction image.
			/// </para>
			/// </param>
			/// <param name="xoffset">
			/// <para>
			/// Specifies a texel offset in the x direction within the texture array.
			/// </para>
			/// </param>
			/// <param name="yoffset">
			/// <para>
			/// Specifies a texel offset in the y direction within the texture array.
			/// </para>
			/// </param>
			/// <param name="width">
			/// <para>
			/// Specifies the width of the texture subimage.
			/// </para>
			/// </param>
			/// <param name="height">
			/// <para>
			/// Specifies the height of the texture subimage.
			/// </para>
			/// </param>
			/// <param name="depth">
			/// <para>
			/// Specifies the depth of the texture subimage.
			/// </para>
			/// </param>
			/// <param name="format">
			/// <para>
			/// Specifies the format of the compressed image data stored at address data.
			/// </para>
			/// </param>
			/// <param name="imageSize">
			/// <para>
			/// Specifies the number of unsigned bytes of image data starting at the address specified by data.
			/// </para>
			/// </param>
			/// <param name="data">
			/// <para>
			/// Specifies a pointer to the compressed image data in memory.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glCompressedTexSubImage3DOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void CompressedTexSubImage3D<T10>(System.Graphics.ES20.All target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 width, Int32 height, Int32 depth, System.Graphics.ES20.All format, Int32 imageSize, [InAttribute, OutAttribute] T10[,,] data)
							where T10 : struct {
				GCHandle data_ptr = GCHandle.Alloc(data, GCHandleType.Pinned);
				try {
					Delegates.glCompressedTexSubImage3DOES((System.Graphics.ES20.All) target, (Int32) level, (Int32) xoffset, (Int32) yoffset, (Int32) zoffset, (Int32) width, (Int32) height, (Int32) depth, (System.Graphics.ES20.All) format, (Int32) imageSize, (IntPtr) data_ptr.AddrOfPinnedObject());
				} finally {
					data_ptr.Free();
				}
			}


			/// <summary>[requires: 2.0]
			/// Specify a three-dimensional texture subimage in a compressed format
			/// </summary>
			/// <param name="target">
			/// <para>
			/// Specifies the target texture. Must be GL_TEXTURE_3D.
			/// </para>
			/// </param>
			/// <param name="level">
			/// <para>
			/// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap reduction image.
			/// </para>
			/// </param>
			/// <param name="xoffset">
			/// <para>
			/// Specifies a texel offset in the x direction within the texture array.
			/// </para>
			/// </param>
			/// <param name="yoffset">
			/// <para>
			/// Specifies a texel offset in the y direction within the texture array.
			/// </para>
			/// </param>
			/// <param name="width">
			/// <para>
			/// Specifies the width of the texture subimage.
			/// </para>
			/// </param>
			/// <param name="height">
			/// <para>
			/// Specifies the height of the texture subimage.
			/// </para>
			/// </param>
			/// <param name="depth">
			/// <para>
			/// Specifies the depth of the texture subimage.
			/// </para>
			/// </param>
			/// <param name="format">
			/// <para>
			/// Specifies the format of the compressed image data stored at address data.
			/// </para>
			/// </param>
			/// <param name="imageSize">
			/// <para>
			/// Specifies the number of unsigned bytes of image data starting at the address specified by data.
			/// </para>
			/// </param>
			/// <param name="data">
			/// <para>
			/// Specifies a pointer to the compressed image data in memory.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glCompressedTexSubImage3DOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void CompressedTexSubImage3D<T10>(System.Graphics.ES20.All target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 width, Int32 height, Int32 depth, System.Graphics.ES20.All format, Int32 imageSize, [InAttribute, OutAttribute] ref T10 data)
							where T10 : struct {
				GCHandle data_ptr = GCHandle.Alloc(data, GCHandleType.Pinned);
				try {
					Delegates.glCompressedTexSubImage3DOES((System.Graphics.ES20.All) target, (Int32) level, (Int32) xoffset, (Int32) yoffset, (Int32) zoffset, (Int32) width, (Int32) height, (Int32) depth, (System.Graphics.ES20.All) format, (Int32) imageSize, (IntPtr) data_ptr.AddrOfPinnedObject());
					data = (T10) data_ptr.Target;
				} finally {
					data_ptr.Free();
				}
			}


			/// <summary>[requires: 2.0]
			/// Copy a three-dimensional texture subimage
			/// </summary>
			/// <param name="target">
			/// <para>
			/// Specifies the target texture. Must be GL_TEXTURE_3D
			/// </para>
			/// </param>
			/// <param name="level">
			/// <para>
			/// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap reduction image.
			/// </para>
			/// </param>
			/// <param name="xoffset">
			/// <para>
			/// Specifies a texel offset in the x direction within the texture array.
			/// </para>
			/// </param>
			/// <param name="yoffset">
			/// <para>
			/// Specifies a texel offset in the y direction within the texture array.
			/// </para>
			/// </param>
			/// <param name="zoffset">
			/// <para>
			/// Specifies a texel offset in the z direction within the texture array.
			/// </para>
			/// </param>
			/// <param name="x">
			/// <para>
			/// Specify the window coordinates of the lower left corner of the rectangular region of pixels to be copied.
			/// </para>
			/// </param>
			/// <param name="width">
			/// <para>
			/// Specifies the width of the texture subimage.
			/// </para>
			/// </param>
			/// <param name="height">
			/// <para>
			/// Specifies the height of the texture subimage.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glCopyTexSubImage3DOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void CopyTexSubImage3D(System.Graphics.ES20.All target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 x, Int32 y, Int32 width, Int32 height) {
				Delegates.glCopyTexSubImage3DOES((System.Graphics.ES20.All) target, (Int32) level, (Int32) xoffset, (Int32) yoffset, (Int32) zoffset, (Int32) x, (Int32) y, (Int32) width, (Int32) height);
			}


			/// <summary>[requires: 2.0]
			/// Delete vertex array objects
			/// </summary>
			/// <param name="n">
			/// <para>
			/// Specifies the number of vertex array objects to be deleted.
			/// </para>
			/// </param>
			/// <param name="arrays">
			/// <para>
			/// Specifies the address of an array containing the n names of the objects to be deleted.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeleteVertexArraysOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void DeleteVertexArrays(Int32 n, Int32[] arrays) {
				unsafe
				{
					fixed (Int32* arrays_ptr = arrays) {
						Delegates.glDeleteVertexArraysOES((Int32) n, (UInt32*) arrays_ptr);
					}
				}
			}


			/// <summary>[requires: 2.0]
			/// Delete vertex array objects
			/// </summary>
			/// <param name="n">
			/// <para>
			/// Specifies the number of vertex array objects to be deleted.
			/// </para>
			/// </param>
			/// <param name="arrays">
			/// <para>
			/// Specifies the address of an array containing the n names of the objects to be deleted.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeleteVertexArraysOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void DeleteVertexArrays(Int32 n, ref Int32 arrays) {
				unsafe
				{
					fixed (Int32* arrays_ptr = &arrays) {
						Delegates.glDeleteVertexArraysOES((Int32) n, (UInt32*) arrays_ptr);
					}
				}
			}


			/// <summary>[requires: 2.0]
			/// Delete vertex array objects
			/// </summary>
			/// <param name="n">
			/// <para>
			/// Specifies the number of vertex array objects to be deleted.
			/// </para>
			/// </param>
			/// <param name="arrays">
			/// <para>
			/// Specifies the address of an array containing the n names of the objects to be deleted.
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeleteVertexArraysOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void DeleteVertexArrays(Int32 n, Int32* arrays) {
				Delegates.glDeleteVertexArraysOES((Int32) n, (UInt32*) arrays);
			}


			/// <summary>[requires: 2.0]
			/// Delete vertex array objects
			/// </summary>
			/// <param name="n">
			/// <para>
			/// Specifies the number of vertex array objects to be deleted.
			/// </para>
			/// </param>
			/// <param name="arrays">
			/// <para>
			/// Specifies the address of an array containing the n names of the objects to be deleted.
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeleteVertexArraysOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void DeleteVertexArrays(Int32 n, UInt32[] arrays) {
				unsafe
				{
					fixed (UInt32* arrays_ptr = arrays) {
						Delegates.glDeleteVertexArraysOES((Int32) n, (UInt32*) arrays_ptr);
					}
				}
			}


			/// <summary>[requires: 2.0]
			/// Delete vertex array objects
			/// </summary>
			/// <param name="n">
			/// <para>
			/// Specifies the number of vertex array objects to be deleted.
			/// </para>
			/// </param>
			/// <param name="arrays">
			/// <para>
			/// Specifies the address of an array containing the n names of the objects to be deleted.
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeleteVertexArraysOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void DeleteVertexArrays(Int32 n, ref UInt32 arrays) {
				unsafe
				{
					fixed (UInt32* arrays_ptr = &arrays) {
						Delegates.glDeleteVertexArraysOES((Int32) n, (UInt32*) arrays_ptr);
					}
				}
			}


			/// <summary>[requires: 2.0]
			/// Delete vertex array objects
			/// </summary>
			/// <param name="n">
			/// <para>
			/// Specifies the number of vertex array objects to be deleted.
			/// </para>
			/// </param>
			/// <param name="arrays">
			/// <para>
			/// Specifies the address of an array containing the n names of the objects to be deleted.
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDeleteVertexArraysOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void DeleteVertexArrays(Int32 n, UInt32* arrays) {
				Delegates.glDeleteVertexArraysOES((Int32) n, (UInt32*) arrays);
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glEGLImageTargetRenderbufferStorageOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void EGLImageTargetRenderbufferStorage(System.Graphics.ES20.All target, IntPtr image) {
				Delegates.glEGLImageTargetRenderbufferStorageOES((System.Graphics.ES20.All) target, (IntPtr) image);
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glEGLImageTargetTexture2DOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void EGLImageTargetTexture2D(System.Graphics.ES20.All target, IntPtr image) {
				Delegates.glEGLImageTargetTexture2DOES((System.Graphics.ES20.All) target, (IntPtr) image);
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glFramebufferTexture3DOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void FramebufferTexture3D(System.Graphics.ES20.All target, System.Graphics.ES20.All attachment, System.Graphics.ES20.All textarget, Int32 texture, Int32 level, Int32 zoffset) {
				Delegates.glFramebufferTexture3DOES((System.Graphics.ES20.All) target, (System.Graphics.ES20.All) attachment, (System.Graphics.ES20.All) textarget, (UInt32) texture, (Int32) level, (Int32) zoffset);
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glFramebufferTexture3DOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void FramebufferTexture3D(System.Graphics.ES20.All target, System.Graphics.ES20.All attachment, System.Graphics.ES20.All textarget, UInt32 texture, Int32 level, Int32 zoffset) {
				Delegates.glFramebufferTexture3DOES((System.Graphics.ES20.All) target, (System.Graphics.ES20.All) attachment, (System.Graphics.ES20.All) textarget, (UInt32) texture, (Int32) level, (Int32) zoffset);
			}


			/// <summary>[requires: 2.0]
			/// Generate vertex array object names
			/// </summary>
			/// <param name="n">
			/// <para>
			/// Specifies the number of vertex array object names to generate.
			/// </para>
			/// </param>
			/// <param name="arrays">
			/// <para>
			/// Specifies an array in which the generated vertex array object names are stored.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenVertexArraysOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void GenVertexArrays(Int32 n, [OutAttribute] Int32[] arrays) {
				unsafe
				{
					fixed (Int32* arrays_ptr = arrays) {
						Delegates.glGenVertexArraysOES((Int32) n, (UInt32*) arrays_ptr);
					}
				}
			}


			/// <summary>[requires: 2.0]
			/// Generate vertex array object names
			/// </summary>
			/// <param name="n">
			/// <para>
			/// Specifies the number of vertex array object names to generate.
			/// </para>
			/// </param>
			/// <param name="arrays">
			/// <para>
			/// Specifies an array in which the generated vertex array object names are stored.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenVertexArraysOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void GenVertexArrays(Int32 n, [OutAttribute] out Int32 arrays) {
				unsafe
				{
					fixed (Int32* arrays_ptr = &arrays) {
						Delegates.glGenVertexArraysOES((Int32) n, (UInt32*) arrays_ptr);
						arrays = *arrays_ptr;
					}
				}
			}


			/// <summary>[requires: 2.0]
			/// Generate vertex array object names
			/// </summary>
			/// <param name="n">
			/// <para>
			/// Specifies the number of vertex array object names to generate.
			/// </para>
			/// </param>
			/// <param name="arrays">
			/// <para>
			/// Specifies an array in which the generated vertex array object names are stored.
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenVertexArraysOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void GenVertexArrays(Int32 n, [OutAttribute] Int32* arrays) {
				Delegates.glGenVertexArraysOES((Int32) n, (UInt32*) arrays);
			}


			/// <summary>[requires: 2.0]
			/// Generate vertex array object names
			/// </summary>
			/// <param name="n">
			/// <para>
			/// Specifies the number of vertex array object names to generate.
			/// </para>
			/// </param>
			/// <param name="arrays">
			/// <para>
			/// Specifies an array in which the generated vertex array object names are stored.
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenVertexArraysOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void GenVertexArrays(Int32 n, [OutAttribute] UInt32[] arrays) {
				unsafe
				{
					fixed (UInt32* arrays_ptr = arrays) {
						Delegates.glGenVertexArraysOES((Int32) n, (UInt32*) arrays_ptr);
					}
				}
			}


			/// <summary>[requires: 2.0]
			/// Generate vertex array object names
			/// </summary>
			/// <param name="n">
			/// <para>
			/// Specifies the number of vertex array object names to generate.
			/// </para>
			/// </param>
			/// <param name="arrays">
			/// <para>
			/// Specifies an array in which the generated vertex array object names are stored.
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenVertexArraysOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void GenVertexArrays(Int32 n, [OutAttribute] out UInt32 arrays) {
				unsafe
				{
					fixed (UInt32* arrays_ptr = &arrays) {
						Delegates.glGenVertexArraysOES((Int32) n, (UInt32*) arrays_ptr);
						arrays = *arrays_ptr;
					}
				}
			}


			/// <summary>[requires: 2.0]
			/// Generate vertex array object names
			/// </summary>
			/// <param name="n">
			/// <para>
			/// Specifies the number of vertex array object names to generate.
			/// </para>
			/// </param>
			/// <param name="arrays">
			/// <para>
			/// Specifies an array in which the generated vertex array object names are stored.
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGenVertexArraysOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void GenVertexArrays(Int32 n, [OutAttribute] UInt32* arrays) {
				Delegates.glGenVertexArraysOES((Int32) n, (UInt32*) arrays);
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetBufferPointervOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void GetBufferPointer(System.Graphics.ES20.All target, System.Graphics.ES20.All pname, [OutAttribute] IntPtr @params) {
				Delegates.glGetBufferPointervOES((System.Graphics.ES20.All) target, (System.Graphics.ES20.All) pname, (IntPtr) @params);
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetBufferPointervOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void GetBufferPointer<T2>(System.Graphics.ES20.All target, System.Graphics.ES20.All pname, [InAttribute, OutAttribute] T2[] @params)
							where T2 : struct {
				GCHandle @params_ptr = GCHandle.Alloc(@params, GCHandleType.Pinned);
				try {
					Delegates.glGetBufferPointervOES((System.Graphics.ES20.All) target, (System.Graphics.ES20.All) pname, (IntPtr) @params_ptr.AddrOfPinnedObject());
				} finally {
					@params_ptr.Free();
				}
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetBufferPointervOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void GetBufferPointer<T2>(System.Graphics.ES20.All target, System.Graphics.ES20.All pname, [InAttribute, OutAttribute] T2[,] @params)
							where T2 : struct {
				GCHandle @params_ptr = GCHandle.Alloc(@params, GCHandleType.Pinned);
				try {
					Delegates.glGetBufferPointervOES((System.Graphics.ES20.All) target, (System.Graphics.ES20.All) pname, (IntPtr) @params_ptr.AddrOfPinnedObject());
				} finally {
					@params_ptr.Free();
				}
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetBufferPointervOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void GetBufferPointer<T2>(System.Graphics.ES20.All target, System.Graphics.ES20.All pname, [InAttribute, OutAttribute] T2[,,] @params)
							where T2 : struct {
				GCHandle @params_ptr = GCHandle.Alloc(@params, GCHandleType.Pinned);
				try {
					Delegates.glGetBufferPointervOES((System.Graphics.ES20.All) target, (System.Graphics.ES20.All) pname, (IntPtr) @params_ptr.AddrOfPinnedObject());
				} finally {
					@params_ptr.Free();
				}
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetBufferPointervOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void GetBufferPointer<T2>(System.Graphics.ES20.All target, System.Graphics.ES20.All pname, [InAttribute, OutAttribute] ref T2 @params)
							where T2 : struct {
				GCHandle @params_ptr = GCHandle.Alloc(@params, GCHandleType.Pinned);
				try {
					Delegates.glGetBufferPointervOES((System.Graphics.ES20.All) target, (System.Graphics.ES20.All) pname, (IntPtr) @params_ptr.AddrOfPinnedObject());
					@params = (T2) @params_ptr.Target;
				} finally {
					@params_ptr.Free();
				}
			}


			/// <summary>[requires: 2.0]
			/// Return a binary representation of a program object's compiled and linked executable source
			/// </summary>
			/// <param name="program">
			/// <para>
			/// Specifies the name of a program object whose binary representation to retrieve.
			/// </para>
			/// </param>
			/// <param name="bufSize">
			/// <para>
			/// Specifies the size of the buffer whose address is given by binary.
			/// </para>
			/// </param>
			/// <param name="length">
			/// <para>
			/// Specifies the address of a variable to receive the number of bytes written into binary.
			/// </para>
			/// </param>
			/// <param name="binaryFormat">
			/// <para>
			/// Specifies the address of a variable to receive a token indicating the format of the binary data returned by the GL.
			/// </para>
			/// </param>
			/// <param name="binary">
			/// <para>
			/// Specifies the address an array into which the GL will return program's binary representation.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetProgramBinaryOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void GetProgramBinary(Int32 program, Int32 bufSize, [OutAttribute] Int32[] length, [OutAttribute] System.Graphics.ES20.All[] binaryFormat, [OutAttribute] IntPtr binary) {
				unsafe
				{
					fixed (Int32* length_ptr = length)
					fixed (System.Graphics.ES20.All* binaryFormat_ptr = binaryFormat) {
						Delegates.glGetProgramBinaryOES((UInt32) program, (Int32) bufSize, (Int32*) length_ptr, (System.Graphics.ES20.All*) binaryFormat_ptr, (IntPtr) binary);
					}
				}
			}


			/// <summary>[requires: 2.0]
			/// Return a binary representation of a program object's compiled and linked executable source
			/// </summary>
			/// <param name="program">
			/// <para>
			/// Specifies the name of a program object whose binary representation to retrieve.
			/// </para>
			/// </param>
			/// <param name="bufSize">
			/// <para>
			/// Specifies the size of the buffer whose address is given by binary.
			/// </para>
			/// </param>
			/// <param name="length">
			/// <para>
			/// Specifies the address of a variable to receive the number of bytes written into binary.
			/// </para>
			/// </param>
			/// <param name="binaryFormat">
			/// <para>
			/// Specifies the address of a variable to receive a token indicating the format of the binary data returned by the GL.
			/// </para>
			/// </param>
			/// <param name="binary">
			/// <para>
			/// Specifies the address an array into which the GL will return program's binary representation.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetProgramBinaryOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void GetProgramBinary<T4>(Int32 program, Int32 bufSize, [OutAttribute] Int32[] length, [OutAttribute] System.Graphics.ES20.All[] binaryFormat, [InAttribute, OutAttribute] T4[] binary)
							where T4 : struct {
				unsafe
				{
					fixed (Int32* length_ptr = length)
					fixed (System.Graphics.ES20.All* binaryFormat_ptr = binaryFormat) {
						GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
						try {
							Delegates.glGetProgramBinaryOES((UInt32) program, (Int32) bufSize, (Int32*) length_ptr, (System.Graphics.ES20.All*) binaryFormat_ptr, (IntPtr) binary_ptr.AddrOfPinnedObject());
						} finally {
							binary_ptr.Free();
						}
					}
				}
			}


			/// <summary>[requires: 2.0]
			/// Return a binary representation of a program object's compiled and linked executable source
			/// </summary>
			/// <param name="program">
			/// <para>
			/// Specifies the name of a program object whose binary representation to retrieve.
			/// </para>
			/// </param>
			/// <param name="bufSize">
			/// <para>
			/// Specifies the size of the buffer whose address is given by binary.
			/// </para>
			/// </param>
			/// <param name="length">
			/// <para>
			/// Specifies the address of a variable to receive the number of bytes written into binary.
			/// </para>
			/// </param>
			/// <param name="binaryFormat">
			/// <para>
			/// Specifies the address of a variable to receive a token indicating the format of the binary data returned by the GL.
			/// </para>
			/// </param>
			/// <param name="binary">
			/// <para>
			/// Specifies the address an array into which the GL will return program's binary representation.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetProgramBinaryOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void GetProgramBinary<T4>(Int32 program, Int32 bufSize, [OutAttribute] Int32[] length, [OutAttribute] System.Graphics.ES20.All[] binaryFormat, [InAttribute, OutAttribute] T4[,] binary)
							where T4 : struct {
				unsafe
				{
					fixed (Int32* length_ptr = length)
					fixed (System.Graphics.ES20.All* binaryFormat_ptr = binaryFormat) {
						GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
						try {
							Delegates.glGetProgramBinaryOES((UInt32) program, (Int32) bufSize, (Int32*) length_ptr, (System.Graphics.ES20.All*) binaryFormat_ptr, (IntPtr) binary_ptr.AddrOfPinnedObject());
						} finally {
							binary_ptr.Free();
						}
					}
				}
			}


			/// <summary>[requires: 2.0]
			/// Return a binary representation of a program object's compiled and linked executable source
			/// </summary>
			/// <param name="program">
			/// <para>
			/// Specifies the name of a program object whose binary representation to retrieve.
			/// </para>
			/// </param>
			/// <param name="bufSize">
			/// <para>
			/// Specifies the size of the buffer whose address is given by binary.
			/// </para>
			/// </param>
			/// <param name="length">
			/// <para>
			/// Specifies the address of a variable to receive the number of bytes written into binary.
			/// </para>
			/// </param>
			/// <param name="binaryFormat">
			/// <para>
			/// Specifies the address of a variable to receive a token indicating the format of the binary data returned by the GL.
			/// </para>
			/// </param>
			/// <param name="binary">
			/// <para>
			/// Specifies the address an array into which the GL will return program's binary representation.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetProgramBinaryOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void GetProgramBinary<T4>(Int32 program, Int32 bufSize, [OutAttribute] Int32[] length, [OutAttribute] System.Graphics.ES20.All[] binaryFormat, [InAttribute, OutAttribute] T4[,,] binary)
							where T4 : struct {
				unsafe
				{
					fixed (Int32* length_ptr = length)
					fixed (System.Graphics.ES20.All* binaryFormat_ptr = binaryFormat) {
						GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
						try {
							Delegates.glGetProgramBinaryOES((UInt32) program, (Int32) bufSize, (Int32*) length_ptr, (System.Graphics.ES20.All*) binaryFormat_ptr, (IntPtr) binary_ptr.AddrOfPinnedObject());
						} finally {
							binary_ptr.Free();
						}
					}
				}
			}


			/// <summary>[requires: 2.0]
			/// Return a binary representation of a program object's compiled and linked executable source
			/// </summary>
			/// <param name="program">
			/// <para>
			/// Specifies the name of a program object whose binary representation to retrieve.
			/// </para>
			/// </param>
			/// <param name="bufSize">
			/// <para>
			/// Specifies the size of the buffer whose address is given by binary.
			/// </para>
			/// </param>
			/// <param name="length">
			/// <para>
			/// Specifies the address of a variable to receive the number of bytes written into binary.
			/// </para>
			/// </param>
			/// <param name="binaryFormat">
			/// <para>
			/// Specifies the address of a variable to receive a token indicating the format of the binary data returned by the GL.
			/// </para>
			/// </param>
			/// <param name="binary">
			/// <para>
			/// Specifies the address an array into which the GL will return program's binary representation.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetProgramBinaryOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void GetProgramBinary<T4>(Int32 program, Int32 bufSize, [OutAttribute] Int32[] length, [OutAttribute] System.Graphics.ES20.All[] binaryFormat, [InAttribute, OutAttribute] ref T4 binary)
							where T4 : struct {
				unsafe
				{
					fixed (Int32* length_ptr = length)
					fixed (System.Graphics.ES20.All* binaryFormat_ptr = binaryFormat) {
						GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
						try {
							Delegates.glGetProgramBinaryOES((UInt32) program, (Int32) bufSize, (Int32*) length_ptr, (System.Graphics.ES20.All*) binaryFormat_ptr, (IntPtr) binary_ptr.AddrOfPinnedObject());
							binary = (T4) binary_ptr.Target;
						} finally {
							binary_ptr.Free();
						}
					}
				}
			}


			/// <summary>[requires: 2.0]
			/// Return a binary representation of a program object's compiled and linked executable source
			/// </summary>
			/// <param name="program">
			/// <para>
			/// Specifies the name of a program object whose binary representation to retrieve.
			/// </para>
			/// </param>
			/// <param name="bufSize">
			/// <para>
			/// Specifies the size of the buffer whose address is given by binary.
			/// </para>
			/// </param>
			/// <param name="length">
			/// <para>
			/// Specifies the address of a variable to receive the number of bytes written into binary.
			/// </para>
			/// </param>
			/// <param name="binaryFormat">
			/// <para>
			/// Specifies the address of a variable to receive a token indicating the format of the binary data returned by the GL.
			/// </para>
			/// </param>
			/// <param name="binary">
			/// <para>
			/// Specifies the address an array into which the GL will return program's binary representation.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetProgramBinaryOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void GetProgramBinary(Int32 program, Int32 bufSize, [OutAttribute] out Int32 length, [OutAttribute] out System.Graphics.ES20.All binaryFormat, [OutAttribute] IntPtr binary) {
				unsafe
				{
					fixed (Int32* length_ptr = &length)
					fixed (System.Graphics.ES20.All* binaryFormat_ptr = &binaryFormat) {
						Delegates.glGetProgramBinaryOES((UInt32) program, (Int32) bufSize, (Int32*) length_ptr, (System.Graphics.ES20.All*) binaryFormat_ptr, (IntPtr) binary);
						length = *length_ptr;
						binaryFormat = *binaryFormat_ptr;
					}
				}
			}


			/// <summary>[requires: 2.0]
			/// Return a binary representation of a program object's compiled and linked executable source
			/// </summary>
			/// <param name="program">
			/// <para>
			/// Specifies the name of a program object whose binary representation to retrieve.
			/// </para>
			/// </param>
			/// <param name="bufSize">
			/// <para>
			/// Specifies the size of the buffer whose address is given by binary.
			/// </para>
			/// </param>
			/// <param name="length">
			/// <para>
			/// Specifies the address of a variable to receive the number of bytes written into binary.
			/// </para>
			/// </param>
			/// <param name="binaryFormat">
			/// <para>
			/// Specifies the address of a variable to receive a token indicating the format of the binary data returned by the GL.
			/// </para>
			/// </param>
			/// <param name="binary">
			/// <para>
			/// Specifies the address an array into which the GL will return program's binary representation.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetProgramBinaryOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void GetProgramBinary<T4>(Int32 program, Int32 bufSize, [OutAttribute] out Int32 length, [OutAttribute] out System.Graphics.ES20.All binaryFormat, [InAttribute, OutAttribute] T4[] binary)
							where T4 : struct {
				unsafe
				{
					fixed (Int32* length_ptr = &length)
					fixed (System.Graphics.ES20.All* binaryFormat_ptr = &binaryFormat) {
						GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
						try {
							Delegates.glGetProgramBinaryOES((UInt32) program, (Int32) bufSize, (Int32*) length_ptr, (System.Graphics.ES20.All*) binaryFormat_ptr, (IntPtr) binary_ptr.AddrOfPinnedObject());
							length = *length_ptr;
							binaryFormat = *binaryFormat_ptr;
						} finally {
							binary_ptr.Free();
						}
					}
				}
			}


			/// <summary>[requires: 2.0]
			/// Return a binary representation of a program object's compiled and linked executable source
			/// </summary>
			/// <param name="program">
			/// <para>
			/// Specifies the name of a program object whose binary representation to retrieve.
			/// </para>
			/// </param>
			/// <param name="bufSize">
			/// <para>
			/// Specifies the size of the buffer whose address is given by binary.
			/// </para>
			/// </param>
			/// <param name="length">
			/// <para>
			/// Specifies the address of a variable to receive the number of bytes written into binary.
			/// </para>
			/// </param>
			/// <param name="binaryFormat">
			/// <para>
			/// Specifies the address of a variable to receive a token indicating the format of the binary data returned by the GL.
			/// </para>
			/// </param>
			/// <param name="binary">
			/// <para>
			/// Specifies the address an array into which the GL will return program's binary representation.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetProgramBinaryOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void GetProgramBinary<T4>(Int32 program, Int32 bufSize, [OutAttribute] out Int32 length, [OutAttribute] out System.Graphics.ES20.All binaryFormat, [InAttribute, OutAttribute] T4[,] binary)
							where T4 : struct {
				unsafe
				{
					fixed (Int32* length_ptr = &length)
					fixed (System.Graphics.ES20.All* binaryFormat_ptr = &binaryFormat) {
						GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
						try {
							Delegates.glGetProgramBinaryOES((UInt32) program, (Int32) bufSize, (Int32*) length_ptr, (System.Graphics.ES20.All*) binaryFormat_ptr, (IntPtr) binary_ptr.AddrOfPinnedObject());
							length = *length_ptr;
							binaryFormat = *binaryFormat_ptr;
						} finally {
							binary_ptr.Free();
						}
					}
				}
			}


			/// <summary>[requires: 2.0]
			/// Return a binary representation of a program object's compiled and linked executable source
			/// </summary>
			/// <param name="program">
			/// <para>
			/// Specifies the name of a program object whose binary representation to retrieve.
			/// </para>
			/// </param>
			/// <param name="bufSize">
			/// <para>
			/// Specifies the size of the buffer whose address is given by binary.
			/// </para>
			/// </param>
			/// <param name="length">
			/// <para>
			/// Specifies the address of a variable to receive the number of bytes written into binary.
			/// </para>
			/// </param>
			/// <param name="binaryFormat">
			/// <para>
			/// Specifies the address of a variable to receive a token indicating the format of the binary data returned by the GL.
			/// </para>
			/// </param>
			/// <param name="binary">
			/// <para>
			/// Specifies the address an array into which the GL will return program's binary representation.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetProgramBinaryOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void GetProgramBinary<T4>(Int32 program, Int32 bufSize, [OutAttribute] out Int32 length, [OutAttribute] out System.Graphics.ES20.All binaryFormat, [InAttribute, OutAttribute] T4[,,] binary)
							where T4 : struct {
				unsafe
				{
					fixed (Int32* length_ptr = &length)
					fixed (System.Graphics.ES20.All* binaryFormat_ptr = &binaryFormat) {
						GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
						try {
							Delegates.glGetProgramBinaryOES((UInt32) program, (Int32) bufSize, (Int32*) length_ptr, (System.Graphics.ES20.All*) binaryFormat_ptr, (IntPtr) binary_ptr.AddrOfPinnedObject());
							length = *length_ptr;
							binaryFormat = *binaryFormat_ptr;
						} finally {
							binary_ptr.Free();
						}
					}
				}
			}


			/// <summary>[requires: 2.0]
			/// Return a binary representation of a program object's compiled and linked executable source
			/// </summary>
			/// <param name="program">
			/// <para>
			/// Specifies the name of a program object whose binary representation to retrieve.
			/// </para>
			/// </param>
			/// <param name="bufSize">
			/// <para>
			/// Specifies the size of the buffer whose address is given by binary.
			/// </para>
			/// </param>
			/// <param name="length">
			/// <para>
			/// Specifies the address of a variable to receive the number of bytes written into binary.
			/// </para>
			/// </param>
			/// <param name="binaryFormat">
			/// <para>
			/// Specifies the address of a variable to receive a token indicating the format of the binary data returned by the GL.
			/// </para>
			/// </param>
			/// <param name="binary">
			/// <para>
			/// Specifies the address an array into which the GL will return program's binary representation.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetProgramBinaryOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void GetProgramBinary<T4>(Int32 program, Int32 bufSize, [OutAttribute] out Int32 length, [OutAttribute] out System.Graphics.ES20.All binaryFormat, [InAttribute, OutAttribute] ref T4 binary)
							where T4 : struct {
				unsafe
				{
					fixed (Int32* length_ptr = &length)
					fixed (System.Graphics.ES20.All* binaryFormat_ptr = &binaryFormat) {
						GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
						try {
							Delegates.glGetProgramBinaryOES((UInt32) program, (Int32) bufSize, (Int32*) length_ptr, (System.Graphics.ES20.All*) binaryFormat_ptr, (IntPtr) binary_ptr.AddrOfPinnedObject());
							length = *length_ptr;
							binaryFormat = *binaryFormat_ptr;
							binary = (T4) binary_ptr.Target;
						} finally {
							binary_ptr.Free();
						}
					}
				}
			}


			/// <summary>[requires: 2.0]
			/// Return a binary representation of a program object's compiled and linked executable source
			/// </summary>
			/// <param name="program">
			/// <para>
			/// Specifies the name of a program object whose binary representation to retrieve.
			/// </para>
			/// </param>
			/// <param name="bufSize">
			/// <para>
			/// Specifies the size of the buffer whose address is given by binary.
			/// </para>
			/// </param>
			/// <param name="length">
			/// <para>
			/// Specifies the address of a variable to receive the number of bytes written into binary.
			/// </para>
			/// </param>
			/// <param name="binaryFormat">
			/// <para>
			/// Specifies the address of a variable to receive a token indicating the format of the binary data returned by the GL.
			/// </para>
			/// </param>
			/// <param name="binary">
			/// <para>
			/// Specifies the address an array into which the GL will return program's binary representation.
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetProgramBinaryOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void GetProgramBinary(Int32 program, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] System.Graphics.ES20.All* binaryFormat, [OutAttribute] IntPtr binary) {
				Delegates.glGetProgramBinaryOES((UInt32) program, (Int32) bufSize, (Int32*) length, (System.Graphics.ES20.All*) binaryFormat, (IntPtr) binary);
			}


			/// <summary>[requires: 2.0]
			/// Return a binary representation of a program object's compiled and linked executable source
			/// </summary>
			/// <param name="program">
			/// <para>
			/// Specifies the name of a program object whose binary representation to retrieve.
			/// </para>
			/// </param>
			/// <param name="bufSize">
			/// <para>
			/// Specifies the size of the buffer whose address is given by binary.
			/// </para>
			/// </param>
			/// <param name="length">
			/// <para>
			/// Specifies the address of a variable to receive the number of bytes written into binary.
			/// </para>
			/// </param>
			/// <param name="binaryFormat">
			/// <para>
			/// Specifies the address of a variable to receive a token indicating the format of the binary data returned by the GL.
			/// </para>
			/// </param>
			/// <param name="binary">
			/// <para>
			/// Specifies the address an array into which the GL will return program's binary representation.
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetProgramBinaryOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void GetProgramBinary<T4>(Int32 program, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] System.Graphics.ES20.All* binaryFormat, [InAttribute, OutAttribute] T4[] binary)
				where T4 : struct {
				GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
				try {
					Delegates.glGetProgramBinaryOES((UInt32) program, (Int32) bufSize, (Int32*) length, (System.Graphics.ES20.All*) binaryFormat, (IntPtr) binary_ptr.AddrOfPinnedObject());
				} finally {
					binary_ptr.Free();
				}
			}


			/// <summary>[requires: 2.0]
			/// Return a binary representation of a program object's compiled and linked executable source
			/// </summary>
			/// <param name="program">
			/// <para>
			/// Specifies the name of a program object whose binary representation to retrieve.
			/// </para>
			/// </param>
			/// <param name="bufSize">
			/// <para>
			/// Specifies the size of the buffer whose address is given by binary.
			/// </para>
			/// </param>
			/// <param name="length">
			/// <para>
			/// Specifies the address of a variable to receive the number of bytes written into binary.
			/// </para>
			/// </param>
			/// <param name="binaryFormat">
			/// <para>
			/// Specifies the address of a variable to receive a token indicating the format of the binary data returned by the GL.
			/// </para>
			/// </param>
			/// <param name="binary">
			/// <para>
			/// Specifies the address an array into which the GL will return program's binary representation.
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetProgramBinaryOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void GetProgramBinary<T4>(Int32 program, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] System.Graphics.ES20.All* binaryFormat, [InAttribute, OutAttribute] T4[,] binary)
				where T4 : struct {
				GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
				try {
					Delegates.glGetProgramBinaryOES((UInt32) program, (Int32) bufSize, (Int32*) length, (System.Graphics.ES20.All*) binaryFormat, (IntPtr) binary_ptr.AddrOfPinnedObject());
				} finally {
					binary_ptr.Free();
				}
			}


			/// <summary>[requires: 2.0]
			/// Return a binary representation of a program object's compiled and linked executable source
			/// </summary>
			/// <param name="program">
			/// <para>
			/// Specifies the name of a program object whose binary representation to retrieve.
			/// </para>
			/// </param>
			/// <param name="bufSize">
			/// <para>
			/// Specifies the size of the buffer whose address is given by binary.
			/// </para>
			/// </param>
			/// <param name="length">
			/// <para>
			/// Specifies the address of a variable to receive the number of bytes written into binary.
			/// </para>
			/// </param>
			/// <param name="binaryFormat">
			/// <para>
			/// Specifies the address of a variable to receive a token indicating the format of the binary data returned by the GL.
			/// </para>
			/// </param>
			/// <param name="binary">
			/// <para>
			/// Specifies the address an array into which the GL will return program's binary representation.
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetProgramBinaryOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void GetProgramBinary<T4>(Int32 program, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] System.Graphics.ES20.All* binaryFormat, [InAttribute, OutAttribute] T4[,,] binary)
				where T4 : struct {
				GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
				try {
					Delegates.glGetProgramBinaryOES((UInt32) program, (Int32) bufSize, (Int32*) length, (System.Graphics.ES20.All*) binaryFormat, (IntPtr) binary_ptr.AddrOfPinnedObject());
				} finally {
					binary_ptr.Free();
				}
			}


			/// <summary>[requires: 2.0]
			/// Return a binary representation of a program object's compiled and linked executable source
			/// </summary>
			/// <param name="program">
			/// <para>
			/// Specifies the name of a program object whose binary representation to retrieve.
			/// </para>
			/// </param>
			/// <param name="bufSize">
			/// <para>
			/// Specifies the size of the buffer whose address is given by binary.
			/// </para>
			/// </param>
			/// <param name="length">
			/// <para>
			/// Specifies the address of a variable to receive the number of bytes written into binary.
			/// </para>
			/// </param>
			/// <param name="binaryFormat">
			/// <para>
			/// Specifies the address of a variable to receive a token indicating the format of the binary data returned by the GL.
			/// </para>
			/// </param>
			/// <param name="binary">
			/// <para>
			/// Specifies the address an array into which the GL will return program's binary representation.
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetProgramBinaryOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void GetProgramBinary<T4>(Int32 program, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] System.Graphics.ES20.All* binaryFormat, [InAttribute, OutAttribute] ref T4 binary)
				where T4 : struct {
				GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
				try {
					Delegates.glGetProgramBinaryOES((UInt32) program, (Int32) bufSize, (Int32*) length, (System.Graphics.ES20.All*) binaryFormat, (IntPtr) binary_ptr.AddrOfPinnedObject());
					binary = (T4) binary_ptr.Target;
				} finally {
					binary_ptr.Free();
				}
			}


			/// <summary>[requires: 2.0]
			/// Return a binary representation of a program object's compiled and linked executable source
			/// </summary>
			/// <param name="program">
			/// <para>
			/// Specifies the name of a program object whose binary representation to retrieve.
			/// </para>
			/// </param>
			/// <param name="bufSize">
			/// <para>
			/// Specifies the size of the buffer whose address is given by binary.
			/// </para>
			/// </param>
			/// <param name="length">
			/// <para>
			/// Specifies the address of a variable to receive the number of bytes written into binary.
			/// </para>
			/// </param>
			/// <param name="binaryFormat">
			/// <para>
			/// Specifies the address of a variable to receive a token indicating the format of the binary data returned by the GL.
			/// </para>
			/// </param>
			/// <param name="binary">
			/// <para>
			/// Specifies the address an array into which the GL will return program's binary representation.
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetProgramBinaryOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void GetProgramBinary(UInt32 program, Int32 bufSize, [OutAttribute] Int32[] length, [OutAttribute] System.Graphics.ES20.All[] binaryFormat, [OutAttribute] IntPtr binary) {
				unsafe
				{
					fixed (Int32* length_ptr = length)
					fixed (System.Graphics.ES20.All* binaryFormat_ptr = binaryFormat) {
						Delegates.glGetProgramBinaryOES((UInt32) program, (Int32) bufSize, (Int32*) length_ptr, (System.Graphics.ES20.All*) binaryFormat_ptr, (IntPtr) binary);
					}
				}
			}


			/// <summary>[requires: 2.0]
			/// Return a binary representation of a program object's compiled and linked executable source
			/// </summary>
			/// <param name="program">
			/// <para>
			/// Specifies the name of a program object whose binary representation to retrieve.
			/// </para>
			/// </param>
			/// <param name="bufSize">
			/// <para>
			/// Specifies the size of the buffer whose address is given by binary.
			/// </para>
			/// </param>
			/// <param name="length">
			/// <para>
			/// Specifies the address of a variable to receive the number of bytes written into binary.
			/// </para>
			/// </param>
			/// <param name="binaryFormat">
			/// <para>
			/// Specifies the address of a variable to receive a token indicating the format of the binary data returned by the GL.
			/// </para>
			/// </param>
			/// <param name="binary">
			/// <para>
			/// Specifies the address an array into which the GL will return program's binary representation.
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetProgramBinaryOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void GetProgramBinary<T4>(UInt32 program, Int32 bufSize, [OutAttribute] Int32[] length, [OutAttribute] System.Graphics.ES20.All[] binaryFormat, [InAttribute, OutAttribute] T4[] binary)
				where T4 : struct {
				unsafe
				{
					fixed (Int32* length_ptr = length)
					fixed (System.Graphics.ES20.All* binaryFormat_ptr = binaryFormat) {
						GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
						try {
							Delegates.glGetProgramBinaryOES((UInt32) program, (Int32) bufSize, (Int32*) length_ptr, (System.Graphics.ES20.All*) binaryFormat_ptr, (IntPtr) binary_ptr.AddrOfPinnedObject());
						} finally {
							binary_ptr.Free();
						}
					}
				}
			}


			/// <summary>[requires: 2.0]
			/// Return a binary representation of a program object's compiled and linked executable source
			/// </summary>
			/// <param name="program">
			/// <para>
			/// Specifies the name of a program object whose binary representation to retrieve.
			/// </para>
			/// </param>
			/// <param name="bufSize">
			/// <para>
			/// Specifies the size of the buffer whose address is given by binary.
			/// </para>
			/// </param>
			/// <param name="length">
			/// <para>
			/// Specifies the address of a variable to receive the number of bytes written into binary.
			/// </para>
			/// </param>
			/// <param name="binaryFormat">
			/// <para>
			/// Specifies the address of a variable to receive a token indicating the format of the binary data returned by the GL.
			/// </para>
			/// </param>
			/// <param name="binary">
			/// <para>
			/// Specifies the address an array into which the GL will return program's binary representation.
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetProgramBinaryOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void GetProgramBinary<T4>(UInt32 program, Int32 bufSize, [OutAttribute] Int32[] length, [OutAttribute] System.Graphics.ES20.All[] binaryFormat, [InAttribute, OutAttribute] T4[,] binary)
				where T4 : struct {
				unsafe
				{
					fixed (Int32* length_ptr = length)
					fixed (System.Graphics.ES20.All* binaryFormat_ptr = binaryFormat) {
						GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
						try {
							Delegates.glGetProgramBinaryOES((UInt32) program, (Int32) bufSize, (Int32*) length_ptr, (System.Graphics.ES20.All*) binaryFormat_ptr, (IntPtr) binary_ptr.AddrOfPinnedObject());
						} finally {
							binary_ptr.Free();
						}
					}
				}
			}


			/// <summary>[requires: 2.0]
			/// Return a binary representation of a program object's compiled and linked executable source
			/// </summary>
			/// <param name="program">
			/// <para>
			/// Specifies the name of a program object whose binary representation to retrieve.
			/// </para>
			/// </param>
			/// <param name="bufSize">
			/// <para>
			/// Specifies the size of the buffer whose address is given by binary.
			/// </para>
			/// </param>
			/// <param name="length">
			/// <para>
			/// Specifies the address of a variable to receive the number of bytes written into binary.
			/// </para>
			/// </param>
			/// <param name="binaryFormat">
			/// <para>
			/// Specifies the address of a variable to receive a token indicating the format of the binary data returned by the GL.
			/// </para>
			/// </param>
			/// <param name="binary">
			/// <para>
			/// Specifies the address an array into which the GL will return program's binary representation.
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetProgramBinaryOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void GetProgramBinary<T4>(UInt32 program, Int32 bufSize, [OutAttribute] Int32[] length, [OutAttribute] System.Graphics.ES20.All[] binaryFormat, [InAttribute, OutAttribute] T4[,,] binary)
				where T4 : struct {
				unsafe
				{
					fixed (Int32* length_ptr = length)
					fixed (System.Graphics.ES20.All* binaryFormat_ptr = binaryFormat) {
						GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
						try {
							Delegates.glGetProgramBinaryOES((UInt32) program, (Int32) bufSize, (Int32*) length_ptr, (System.Graphics.ES20.All*) binaryFormat_ptr, (IntPtr) binary_ptr.AddrOfPinnedObject());
						} finally {
							binary_ptr.Free();
						}
					}
				}
			}


			/// <summary>[requires: 2.0]
			/// Return a binary representation of a program object's compiled and linked executable source
			/// </summary>
			/// <param name="program">
			/// <para>
			/// Specifies the name of a program object whose binary representation to retrieve.
			/// </para>
			/// </param>
			/// <param name="bufSize">
			/// <para>
			/// Specifies the size of the buffer whose address is given by binary.
			/// </para>
			/// </param>
			/// <param name="length">
			/// <para>
			/// Specifies the address of a variable to receive the number of bytes written into binary.
			/// </para>
			/// </param>
			/// <param name="binaryFormat">
			/// <para>
			/// Specifies the address of a variable to receive a token indicating the format of the binary data returned by the GL.
			/// </para>
			/// </param>
			/// <param name="binary">
			/// <para>
			/// Specifies the address an array into which the GL will return program's binary representation.
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetProgramBinaryOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void GetProgramBinary<T4>(UInt32 program, Int32 bufSize, [OutAttribute] Int32[] length, [OutAttribute] System.Graphics.ES20.All[] binaryFormat, [InAttribute, OutAttribute] ref T4 binary)
				where T4 : struct {
				unsafe
				{
					fixed (Int32* length_ptr = length)
					fixed (System.Graphics.ES20.All* binaryFormat_ptr = binaryFormat) {
						GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
						try {
							Delegates.glGetProgramBinaryOES((UInt32) program, (Int32) bufSize, (Int32*) length_ptr, (System.Graphics.ES20.All*) binaryFormat_ptr, (IntPtr) binary_ptr.AddrOfPinnedObject());
							binary = (T4) binary_ptr.Target;
						} finally {
							binary_ptr.Free();
						}
					}
				}
			}


			/// <summary>[requires: 2.0]
			/// Return a binary representation of a program object's compiled and linked executable source
			/// </summary>
			/// <param name="program">
			/// <para>
			/// Specifies the name of a program object whose binary representation to retrieve.
			/// </para>
			/// </param>
			/// <param name="bufSize">
			/// <para>
			/// Specifies the size of the buffer whose address is given by binary.
			/// </para>
			/// </param>
			/// <param name="length">
			/// <para>
			/// Specifies the address of a variable to receive the number of bytes written into binary.
			/// </para>
			/// </param>
			/// <param name="binaryFormat">
			/// <para>
			/// Specifies the address of a variable to receive a token indicating the format of the binary data returned by the GL.
			/// </para>
			/// </param>
			/// <param name="binary">
			/// <para>
			/// Specifies the address an array into which the GL will return program's binary representation.
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetProgramBinaryOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void GetProgramBinary(UInt32 program, Int32 bufSize, [OutAttribute] out Int32 length, [OutAttribute] out System.Graphics.ES20.All binaryFormat, [OutAttribute] IntPtr binary) {
				unsafe
				{
					fixed (Int32* length_ptr = &length)
					fixed (System.Graphics.ES20.All* binaryFormat_ptr = &binaryFormat) {
						Delegates.glGetProgramBinaryOES((UInt32) program, (Int32) bufSize, (Int32*) length_ptr, (System.Graphics.ES20.All*) binaryFormat_ptr, (IntPtr) binary);
						length = *length_ptr;
						binaryFormat = *binaryFormat_ptr;
					}
				}
			}


			/// <summary>[requires: 2.0]
			/// Return a binary representation of a program object's compiled and linked executable source
			/// </summary>
			/// <param name="program">
			/// <para>
			/// Specifies the name of a program object whose binary representation to retrieve.
			/// </para>
			/// </param>
			/// <param name="bufSize">
			/// <para>
			/// Specifies the size of the buffer whose address is given by binary.
			/// </para>
			/// </param>
			/// <param name="length">
			/// <para>
			/// Specifies the address of a variable to receive the number of bytes written into binary.
			/// </para>
			/// </param>
			/// <param name="binaryFormat">
			/// <para>
			/// Specifies the address of a variable to receive a token indicating the format of the binary data returned by the GL.
			/// </para>
			/// </param>
			/// <param name="binary">
			/// <para>
			/// Specifies the address an array into which the GL will return program's binary representation.
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetProgramBinaryOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void GetProgramBinary<T4>(UInt32 program, Int32 bufSize, [OutAttribute] out Int32 length, [OutAttribute] out System.Graphics.ES20.All binaryFormat, [InAttribute, OutAttribute] T4[] binary)
				where T4 : struct {
				unsafe
				{
					fixed (Int32* length_ptr = &length)
					fixed (System.Graphics.ES20.All* binaryFormat_ptr = &binaryFormat) {
						GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
						try {
							Delegates.glGetProgramBinaryOES((UInt32) program, (Int32) bufSize, (Int32*) length_ptr, (System.Graphics.ES20.All*) binaryFormat_ptr, (IntPtr) binary_ptr.AddrOfPinnedObject());
							length = *length_ptr;
							binaryFormat = *binaryFormat_ptr;
						} finally {
							binary_ptr.Free();
						}
					}
				}
			}


			/// <summary>[requires: 2.0]
			/// Return a binary representation of a program object's compiled and linked executable source
			/// </summary>
			/// <param name="program">
			/// <para>
			/// Specifies the name of a program object whose binary representation to retrieve.
			/// </para>
			/// </param>
			/// <param name="bufSize">
			/// <para>
			/// Specifies the size of the buffer whose address is given by binary.
			/// </para>
			/// </param>
			/// <param name="length">
			/// <para>
			/// Specifies the address of a variable to receive the number of bytes written into binary.
			/// </para>
			/// </param>
			/// <param name="binaryFormat">
			/// <para>
			/// Specifies the address of a variable to receive a token indicating the format of the binary data returned by the GL.
			/// </para>
			/// </param>
			/// <param name="binary">
			/// <para>
			/// Specifies the address an array into which the GL will return program's binary representation.
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetProgramBinaryOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void GetProgramBinary<T4>(UInt32 program, Int32 bufSize, [OutAttribute] out Int32 length, [OutAttribute] out System.Graphics.ES20.All binaryFormat, [InAttribute, OutAttribute] T4[,] binary)
				where T4 : struct {
				unsafe
				{
					fixed (Int32* length_ptr = &length)
					fixed (System.Graphics.ES20.All* binaryFormat_ptr = &binaryFormat) {
						GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
						try {
							Delegates.glGetProgramBinaryOES((UInt32) program, (Int32) bufSize, (Int32*) length_ptr, (System.Graphics.ES20.All*) binaryFormat_ptr, (IntPtr) binary_ptr.AddrOfPinnedObject());
							length = *length_ptr;
							binaryFormat = *binaryFormat_ptr;
						} finally {
							binary_ptr.Free();
						}
					}
				}
			}


			/// <summary>[requires: 2.0]
			/// Return a binary representation of a program object's compiled and linked executable source
			/// </summary>
			/// <param name="program">
			/// <para>
			/// Specifies the name of a program object whose binary representation to retrieve.
			/// </para>
			/// </param>
			/// <param name="bufSize">
			/// <para>
			/// Specifies the size of the buffer whose address is given by binary.
			/// </para>
			/// </param>
			/// <param name="length">
			/// <para>
			/// Specifies the address of a variable to receive the number of bytes written into binary.
			/// </para>
			/// </param>
			/// <param name="binaryFormat">
			/// <para>
			/// Specifies the address of a variable to receive a token indicating the format of the binary data returned by the GL.
			/// </para>
			/// </param>
			/// <param name="binary">
			/// <para>
			/// Specifies the address an array into which the GL will return program's binary representation.
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetProgramBinaryOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void GetProgramBinary<T4>(UInt32 program, Int32 bufSize, [OutAttribute] out Int32 length, [OutAttribute] out System.Graphics.ES20.All binaryFormat, [InAttribute, OutAttribute] T4[,,] binary)
				where T4 : struct {
				unsafe
				{
					fixed (Int32* length_ptr = &length)
					fixed (System.Graphics.ES20.All* binaryFormat_ptr = &binaryFormat) {
						GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
						try {
							Delegates.glGetProgramBinaryOES((UInt32) program, (Int32) bufSize, (Int32*) length_ptr, (System.Graphics.ES20.All*) binaryFormat_ptr, (IntPtr) binary_ptr.AddrOfPinnedObject());
							length = *length_ptr;
							binaryFormat = *binaryFormat_ptr;
						} finally {
							binary_ptr.Free();
						}
					}
				}
			}


			/// <summary>[requires: 2.0]
			/// Return a binary representation of a program object's compiled and linked executable source
			/// </summary>
			/// <param name="program">
			/// <para>
			/// Specifies the name of a program object whose binary representation to retrieve.
			/// </para>
			/// </param>
			/// <param name="bufSize">
			/// <para>
			/// Specifies the size of the buffer whose address is given by binary.
			/// </para>
			/// </param>
			/// <param name="length">
			/// <para>
			/// Specifies the address of a variable to receive the number of bytes written into binary.
			/// </para>
			/// </param>
			/// <param name="binaryFormat">
			/// <para>
			/// Specifies the address of a variable to receive a token indicating the format of the binary data returned by the GL.
			/// </para>
			/// </param>
			/// <param name="binary">
			/// <para>
			/// Specifies the address an array into which the GL will return program's binary representation.
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetProgramBinaryOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void GetProgramBinary<T4>(UInt32 program, Int32 bufSize, [OutAttribute] out Int32 length, [OutAttribute] out System.Graphics.ES20.All binaryFormat, [InAttribute, OutAttribute] ref T4 binary)
				where T4 : struct {
				unsafe
				{
					fixed (Int32* length_ptr = &length)
					fixed (System.Graphics.ES20.All* binaryFormat_ptr = &binaryFormat) {
						GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
						try {
							Delegates.glGetProgramBinaryOES((UInt32) program, (Int32) bufSize, (Int32*) length_ptr, (System.Graphics.ES20.All*) binaryFormat_ptr, (IntPtr) binary_ptr.AddrOfPinnedObject());
							length = *length_ptr;
							binaryFormat = *binaryFormat_ptr;
							binary = (T4) binary_ptr.Target;
						} finally {
							binary_ptr.Free();
						}
					}
				}
			}


			/// <summary>[requires: 2.0]
			/// Return a binary representation of a program object's compiled and linked executable source
			/// </summary>
			/// <param name="program">
			/// <para>
			/// Specifies the name of a program object whose binary representation to retrieve.
			/// </para>
			/// </param>
			/// <param name="bufSize">
			/// <para>
			/// Specifies the size of the buffer whose address is given by binary.
			/// </para>
			/// </param>
			/// <param name="length">
			/// <para>
			/// Specifies the address of a variable to receive the number of bytes written into binary.
			/// </para>
			/// </param>
			/// <param name="binaryFormat">
			/// <para>
			/// Specifies the address of a variable to receive a token indicating the format of the binary data returned by the GL.
			/// </para>
			/// </param>
			/// <param name="binary">
			/// <para>
			/// Specifies the address an array into which the GL will return program's binary representation.
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetProgramBinaryOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void GetProgramBinary(UInt32 program, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] System.Graphics.ES20.All* binaryFormat, [OutAttribute] IntPtr binary) {
				Delegates.glGetProgramBinaryOES((UInt32) program, (Int32) bufSize, (Int32*) length, (System.Graphics.ES20.All*) binaryFormat, (IntPtr) binary);
			}


			/// <summary>[requires: 2.0]
			/// Return a binary representation of a program object's compiled and linked executable source
			/// </summary>
			/// <param name="program">
			/// <para>
			/// Specifies the name of a program object whose binary representation to retrieve.
			/// </para>
			/// </param>
			/// <param name="bufSize">
			/// <para>
			/// Specifies the size of the buffer whose address is given by binary.
			/// </para>
			/// </param>
			/// <param name="length">
			/// <para>
			/// Specifies the address of a variable to receive the number of bytes written into binary.
			/// </para>
			/// </param>
			/// <param name="binaryFormat">
			/// <para>
			/// Specifies the address of a variable to receive a token indicating the format of the binary data returned by the GL.
			/// </para>
			/// </param>
			/// <param name="binary">
			/// <para>
			/// Specifies the address an array into which the GL will return program's binary representation.
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetProgramBinaryOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void GetProgramBinary<T4>(UInt32 program, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] System.Graphics.ES20.All* binaryFormat, [InAttribute, OutAttribute] T4[] binary)
				where T4 : struct {
				GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
				try {
					Delegates.glGetProgramBinaryOES((UInt32) program, (Int32) bufSize, (Int32*) length, (System.Graphics.ES20.All*) binaryFormat, (IntPtr) binary_ptr.AddrOfPinnedObject());
				} finally {
					binary_ptr.Free();
				}
			}


			/// <summary>[requires: 2.0]
			/// Return a binary representation of a program object's compiled and linked executable source
			/// </summary>
			/// <param name="program">
			/// <para>
			/// Specifies the name of a program object whose binary representation to retrieve.
			/// </para>
			/// </param>
			/// <param name="bufSize">
			/// <para>
			/// Specifies the size of the buffer whose address is given by binary.
			/// </para>
			/// </param>
			/// <param name="length">
			/// <para>
			/// Specifies the address of a variable to receive the number of bytes written into binary.
			/// </para>
			/// </param>
			/// <param name="binaryFormat">
			/// <para>
			/// Specifies the address of a variable to receive a token indicating the format of the binary data returned by the GL.
			/// </para>
			/// </param>
			/// <param name="binary">
			/// <para>
			/// Specifies the address an array into which the GL will return program's binary representation.
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetProgramBinaryOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void GetProgramBinary<T4>(UInt32 program, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] System.Graphics.ES20.All* binaryFormat, [InAttribute, OutAttribute] T4[,] binary)
				where T4 : struct {
				GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
				try {
					Delegates.glGetProgramBinaryOES((UInt32) program, (Int32) bufSize, (Int32*) length, (System.Graphics.ES20.All*) binaryFormat, (IntPtr) binary_ptr.AddrOfPinnedObject());
				} finally {
					binary_ptr.Free();
				}
			}


			/// <summary>[requires: 2.0]
			/// Return a binary representation of a program object's compiled and linked executable source
			/// </summary>
			/// <param name="program">
			/// <para>
			/// Specifies the name of a program object whose binary representation to retrieve.
			/// </para>
			/// </param>
			/// <param name="bufSize">
			/// <para>
			/// Specifies the size of the buffer whose address is given by binary.
			/// </para>
			/// </param>
			/// <param name="length">
			/// <para>
			/// Specifies the address of a variable to receive the number of bytes written into binary.
			/// </para>
			/// </param>
			/// <param name="binaryFormat">
			/// <para>
			/// Specifies the address of a variable to receive a token indicating the format of the binary data returned by the GL.
			/// </para>
			/// </param>
			/// <param name="binary">
			/// <para>
			/// Specifies the address an array into which the GL will return program's binary representation.
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetProgramBinaryOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void GetProgramBinary<T4>(UInt32 program, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] System.Graphics.ES20.All* binaryFormat, [InAttribute, OutAttribute] T4[,,] binary)
				where T4 : struct {
				GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
				try {
					Delegates.glGetProgramBinaryOES((UInt32) program, (Int32) bufSize, (Int32*) length, (System.Graphics.ES20.All*) binaryFormat, (IntPtr) binary_ptr.AddrOfPinnedObject());
				} finally {
					binary_ptr.Free();
				}
			}


			/// <summary>[requires: 2.0]
			/// Return a binary representation of a program object's compiled and linked executable source
			/// </summary>
			/// <param name="program">
			/// <para>
			/// Specifies the name of a program object whose binary representation to retrieve.
			/// </para>
			/// </param>
			/// <param name="bufSize">
			/// <para>
			/// Specifies the size of the buffer whose address is given by binary.
			/// </para>
			/// </param>
			/// <param name="length">
			/// <para>
			/// Specifies the address of a variable to receive the number of bytes written into binary.
			/// </para>
			/// </param>
			/// <param name="binaryFormat">
			/// <para>
			/// Specifies the address of a variable to receive a token indicating the format of the binary data returned by the GL.
			/// </para>
			/// </param>
			/// <param name="binary">
			/// <para>
			/// Specifies the address an array into which the GL will return program's binary representation.
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetProgramBinaryOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void GetProgramBinary<T4>(UInt32 program, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] System.Graphics.ES20.All* binaryFormat, [InAttribute, OutAttribute] ref T4 binary)
				where T4 : struct {
				GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
				try {
					Delegates.glGetProgramBinaryOES((UInt32) program, (Int32) bufSize, (Int32*) length, (System.Graphics.ES20.All*) binaryFormat, (IntPtr) binary_ptr.AddrOfPinnedObject());
					binary = (T4) binary_ptr.Target;
				} finally {
					binary_ptr.Free();
				}
			}


			/// <summary>[requires: 2.0]
			/// Determine if a name corresponds to a vertex array object
			/// </summary>
			/// <param name="array">
			/// <para>
			/// Specifies a value that may be the name of a vertex array object.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glIsVertexArrayOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						bool IsVertexArray(Int32 array) {
				return Delegates.glIsVertexArrayOES((UInt32) array);
			}


			/// <summary>[requires: 2.0]
			/// Determine if a name corresponds to a vertex array object
			/// </summary>
			/// <param name="array">
			/// <para>
			/// Specifies a value that may be the name of a vertex array object.
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glIsVertexArrayOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			bool IsVertexArray(UInt32 array) {
				return Delegates.glIsVertexArrayOES((UInt32) array);
			}


			/// <summary>[requires: 2.0]
			/// Map a buffer object's data store
			/// </summary>
			/// <param name="target">
			/// <para>
			/// Specifies the target buffer object being mapped. The symbolic constant must be GL_ARRAY_BUFFER, GL_COPY_READ_BUFFER, GL_COPY_WRITE_BUFFER, GL_ELEMENT_ARRAY_BUFFER, GL_PIXEL_PACK_BUFFER, GL_PIXEL_UNPACK_BUFFER, GL_TEXTURE_BUFFER, GL_TRANSFORM_FEEDBACK_BUFFER or GL_UNIFORM_BUFFER.
			/// </para>
			/// </param>
			/// <param name="access">
			/// <para>
			/// Specifies the access policy, indicating whether it will be possible to read from, write to, or both read from and write to the buffer object's mapped data store. The symbolic constant must be GL_READ_ONLY, GL_WRITE_ONLY, or GL_READ_WRITE.
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glMapBufferOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe System.IntPtr MapBuffer(System.Graphics.ES20.All target, System.Graphics.ES20.All access) {
				return Delegates.glMapBufferOES((System.Graphics.ES20.All) target, (System.Graphics.ES20.All) access);
			}


			/// <summary>[requires: 2.0]
			/// Load a program object with a program binary
			/// </summary>
			/// <param name="program">
			/// <para>
			/// Specifies the name of a program object into which to load a program binary.
			/// </para>
			/// </param>
			/// <param name="binaryFormat">
			/// <para>
			/// Specifies the format of the binary data in binary.
			/// </para>
			/// </param>
			/// <param name="binary">
			/// <para>
			/// Specifies the address an array containing the binary to be loaded into program.
			/// </para>
			/// </param>
			/// <param name="length">
			/// <para>
			/// Specifies the number of bytes contained in binary.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glProgramBinaryOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void ProgramBinary(Int32 program, System.Graphics.ES20.All binaryFormat, IntPtr binary, Int32 length) {
				Delegates.glProgramBinaryOES((UInt32) program, (System.Graphics.ES20.All) binaryFormat, (IntPtr) binary, (Int32) length);
			}


			/// <summary>[requires: 2.0]
			/// Load a program object with a program binary
			/// </summary>
			/// <param name="program">
			/// <para>
			/// Specifies the name of a program object into which to load a program binary.
			/// </para>
			/// </param>
			/// <param name="binaryFormat">
			/// <para>
			/// Specifies the format of the binary data in binary.
			/// </para>
			/// </param>
			/// <param name="binary">
			/// <para>
			/// Specifies the address an array containing the binary to be loaded into program.
			/// </para>
			/// </param>
			/// <param name="length">
			/// <para>
			/// Specifies the number of bytes contained in binary.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glProgramBinaryOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void ProgramBinary<T2>(Int32 program, System.Graphics.ES20.All binaryFormat, [InAttribute, OutAttribute] T2[] binary, Int32 length)
							where T2 : struct {
				GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
				try {
					Delegates.glProgramBinaryOES((UInt32) program, (System.Graphics.ES20.All) binaryFormat, (IntPtr) binary_ptr.AddrOfPinnedObject(), (Int32) length);
				} finally {
					binary_ptr.Free();
				}
			}


			/// <summary>[requires: 2.0]
			/// Load a program object with a program binary
			/// </summary>
			/// <param name="program">
			/// <para>
			/// Specifies the name of a program object into which to load a program binary.
			/// </para>
			/// </param>
			/// <param name="binaryFormat">
			/// <para>
			/// Specifies the format of the binary data in binary.
			/// </para>
			/// </param>
			/// <param name="binary">
			/// <para>
			/// Specifies the address an array containing the binary to be loaded into program.
			/// </para>
			/// </param>
			/// <param name="length">
			/// <para>
			/// Specifies the number of bytes contained in binary.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glProgramBinaryOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void ProgramBinary<T2>(Int32 program, System.Graphics.ES20.All binaryFormat, [InAttribute, OutAttribute] T2[,] binary, Int32 length)
							where T2 : struct {
				GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
				try {
					Delegates.glProgramBinaryOES((UInt32) program, (System.Graphics.ES20.All) binaryFormat, (IntPtr) binary_ptr.AddrOfPinnedObject(), (Int32) length);
				} finally {
					binary_ptr.Free();
				}
			}


			/// <summary>[requires: 2.0]
			/// Load a program object with a program binary
			/// </summary>
			/// <param name="program">
			/// <para>
			/// Specifies the name of a program object into which to load a program binary.
			/// </para>
			/// </param>
			/// <param name="binaryFormat">
			/// <para>
			/// Specifies the format of the binary data in binary.
			/// </para>
			/// </param>
			/// <param name="binary">
			/// <para>
			/// Specifies the address an array containing the binary to be loaded into program.
			/// </para>
			/// </param>
			/// <param name="length">
			/// <para>
			/// Specifies the number of bytes contained in binary.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glProgramBinaryOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void ProgramBinary<T2>(Int32 program, System.Graphics.ES20.All binaryFormat, [InAttribute, OutAttribute] T2[,,] binary, Int32 length)
							where T2 : struct {
				GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
				try {
					Delegates.glProgramBinaryOES((UInt32) program, (System.Graphics.ES20.All) binaryFormat, (IntPtr) binary_ptr.AddrOfPinnedObject(), (Int32) length);
				} finally {
					binary_ptr.Free();
				}
			}


			/// <summary>[requires: 2.0]
			/// Load a program object with a program binary
			/// </summary>
			/// <param name="program">
			/// <para>
			/// Specifies the name of a program object into which to load a program binary.
			/// </para>
			/// </param>
			/// <param name="binaryFormat">
			/// <para>
			/// Specifies the format of the binary data in binary.
			/// </para>
			/// </param>
			/// <param name="binary">
			/// <para>
			/// Specifies the address an array containing the binary to be loaded into program.
			/// </para>
			/// </param>
			/// <param name="length">
			/// <para>
			/// Specifies the number of bytes contained in binary.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glProgramBinaryOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void ProgramBinary<T2>(Int32 program, System.Graphics.ES20.All binaryFormat, [InAttribute, OutAttribute] ref T2 binary, Int32 length)
							where T2 : struct {
				GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
				try {
					Delegates.glProgramBinaryOES((UInt32) program, (System.Graphics.ES20.All) binaryFormat, (IntPtr) binary_ptr.AddrOfPinnedObject(), (Int32) length);
					binary = (T2) binary_ptr.Target;
				} finally {
					binary_ptr.Free();
				}
			}


			/// <summary>[requires: 2.0]
			/// Load a program object with a program binary
			/// </summary>
			/// <param name="program">
			/// <para>
			/// Specifies the name of a program object into which to load a program binary.
			/// </para>
			/// </param>
			/// <param name="binaryFormat">
			/// <para>
			/// Specifies the format of the binary data in binary.
			/// </para>
			/// </param>
			/// <param name="binary">
			/// <para>
			/// Specifies the address an array containing the binary to be loaded into program.
			/// </para>
			/// </param>
			/// <param name="length">
			/// <para>
			/// Specifies the number of bytes contained in binary.
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glProgramBinaryOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void ProgramBinary(UInt32 program, System.Graphics.ES20.All binaryFormat, IntPtr binary, Int32 length) {
				Delegates.glProgramBinaryOES((UInt32) program, (System.Graphics.ES20.All) binaryFormat, (IntPtr) binary, (Int32) length);
			}


			/// <summary>[requires: 2.0]
			/// Load a program object with a program binary
			/// </summary>
			/// <param name="program">
			/// <para>
			/// Specifies the name of a program object into which to load a program binary.
			/// </para>
			/// </param>
			/// <param name="binaryFormat">
			/// <para>
			/// Specifies the format of the binary data in binary.
			/// </para>
			/// </param>
			/// <param name="binary">
			/// <para>
			/// Specifies the address an array containing the binary to be loaded into program.
			/// </para>
			/// </param>
			/// <param name="length">
			/// <para>
			/// Specifies the number of bytes contained in binary.
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glProgramBinaryOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void ProgramBinary<T2>(UInt32 program, System.Graphics.ES20.All binaryFormat, [InAttribute, OutAttribute] T2[] binary, Int32 length)
				where T2 : struct {
				GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
				try {
					Delegates.glProgramBinaryOES((UInt32) program, (System.Graphics.ES20.All) binaryFormat, (IntPtr) binary_ptr.AddrOfPinnedObject(), (Int32) length);
				} finally {
					binary_ptr.Free();
				}
			}


			/// <summary>[requires: 2.0]
			/// Load a program object with a program binary
			/// </summary>
			/// <param name="program">
			/// <para>
			/// Specifies the name of a program object into which to load a program binary.
			/// </para>
			/// </param>
			/// <param name="binaryFormat">
			/// <para>
			/// Specifies the format of the binary data in binary.
			/// </para>
			/// </param>
			/// <param name="binary">
			/// <para>
			/// Specifies the address an array containing the binary to be loaded into program.
			/// </para>
			/// </param>
			/// <param name="length">
			/// <para>
			/// Specifies the number of bytes contained in binary.
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glProgramBinaryOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void ProgramBinary<T2>(UInt32 program, System.Graphics.ES20.All binaryFormat, [InAttribute, OutAttribute] T2[,] binary, Int32 length)
				where T2 : struct {
				GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
				try {
					Delegates.glProgramBinaryOES((UInt32) program, (System.Graphics.ES20.All) binaryFormat, (IntPtr) binary_ptr.AddrOfPinnedObject(), (Int32) length);
				} finally {
					binary_ptr.Free();
				}
			}


			/// <summary>[requires: 2.0]
			/// Load a program object with a program binary
			/// </summary>
			/// <param name="program">
			/// <para>
			/// Specifies the name of a program object into which to load a program binary.
			/// </para>
			/// </param>
			/// <param name="binaryFormat">
			/// <para>
			/// Specifies the format of the binary data in binary.
			/// </para>
			/// </param>
			/// <param name="binary">
			/// <para>
			/// Specifies the address an array containing the binary to be loaded into program.
			/// </para>
			/// </param>
			/// <param name="length">
			/// <para>
			/// Specifies the number of bytes contained in binary.
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glProgramBinaryOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void ProgramBinary<T2>(UInt32 program, System.Graphics.ES20.All binaryFormat, [InAttribute, OutAttribute] T2[,,] binary, Int32 length)
				where T2 : struct {
				GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
				try {
					Delegates.glProgramBinaryOES((UInt32) program, (System.Graphics.ES20.All) binaryFormat, (IntPtr) binary_ptr.AddrOfPinnedObject(), (Int32) length);
				} finally {
					binary_ptr.Free();
				}
			}


			/// <summary>[requires: 2.0]
			/// Load a program object with a program binary
			/// </summary>
			/// <param name="program">
			/// <para>
			/// Specifies the name of a program object into which to load a program binary.
			/// </para>
			/// </param>
			/// <param name="binaryFormat">
			/// <para>
			/// Specifies the format of the binary data in binary.
			/// </para>
			/// </param>
			/// <param name="binary">
			/// <para>
			/// Specifies the address an array containing the binary to be loaded into program.
			/// </para>
			/// </param>
			/// <param name="length">
			/// <para>
			/// Specifies the number of bytes contained in binary.
			/// </para>
			/// </param>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glProgramBinaryOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void ProgramBinary<T2>(UInt32 program, System.Graphics.ES20.All binaryFormat, [InAttribute, OutAttribute] ref T2 binary, Int32 length)
				where T2 : struct {
				GCHandle binary_ptr = GCHandle.Alloc(binary, GCHandleType.Pinned);
				try {
					Delegates.glProgramBinaryOES((UInt32) program, (System.Graphics.ES20.All) binaryFormat, (IntPtr) binary_ptr.AddrOfPinnedObject(), (Int32) length);
					binary = (T2) binary_ptr.Target;
				} finally {
					binary_ptr.Free();
				}
			}


			/// <summary>[requires: 2.0]
			/// Specify a three-dimensional texture image
			/// </summary>
			/// <param name="target">
			/// <para>
			/// Specifies the target texture. Must be one of GL_TEXTURE_3D, GL_PROXY_TEXTURE_3D, GL_TEXTURE_2D_ARRAY or GL_PROXY_TEXTURE_2D_ARRAY.
			/// </para>
			/// </param>
			/// <param name="level">
			/// <para>
			/// Specifies the level-of-detail number. Level 0 is the base image level. Level is the n sup th mipmap reduction image.
			/// </para>
			/// </param>
			/// <param name="internalFormat">
			/// <para>
			/// Specifies the number of color components in the texture. Must be one of the following symbolic constants: GL_RGBA32F, GL_RGBA32I, GL_RGBA32UI, GL_RGBA16, GL_RGBA16F, GL_RGBA16I, GL_RGBA16UI, GL_RGBA8, GL_RGBA8UI, GL_SRGB8_ALPHA8, GL_RGB10_A2, GL_RGBA10_A2UI, GL_R11_G11_B10F, GL_RG32F, GL_RG32I, GL_RG32UI, GL_RG16, GL_RG16F, GL_RGB16I, GL_RGB16UI, GL_RG8, GL_RG8I, GL_RG8UI, GL_R23F, GL_R32I, GL_R32UI, GL_R16F, GL_R16I, GL_R16UI, GL_R8, GL_R8I, GL_R8UI, GL_RGBA16_UNORM, GL_RGBA8_SNORM, GL_RGB32F, GL_RGB32I, GL_RGB32UI, GL_RGB16_SNORM, GL_RGB16F, GL_RGB16I, GL_RGB16UI, GL_RGB16, GL_RGB8_SNORM, GL_RGB8, GL_RGB8I, GL_RGB8UI, GL_SRGB8, GL_RGB9_E5, GL_RG16_SNORM, GL_RG8_SNORM, GL_COMPRESSED_RG_RGTC2, GL_COMPRESSED_SIGNED_RG_RGTC2, GL_R16_SNORM, GL_R8_SNORM, GL_COMPRESSED_RED_RGTC1, GL_COMPRESSED_SIGNED_RED_RGTC1, GL_DEPTH_COMPONENT32F, GL_DEPTH_COMPONENT24, GL_DEPTH_COMPONENT16, GL_DEPTH32F_STENCIL8, GL_DEPTH24_STENCIL8.
			/// </para>
			/// </param>
			/// <param name="width">
			/// <para>
			/// Specifies the width of the texture image. All implementations support 3D texture images that are at least 16 texels wide.
			/// </para>
			/// </param>
			/// <param name="height">
			/// <para>
			/// Specifies the height of the texture image. All implementations support 3D texture images that are at least 256 texels high.
			/// </para>
			/// </param>
			/// <param name="depth">
			/// <para>
			/// Specifies the depth of the texture image, or the number of layers in a texture array. All implementations support 3D texture images that are at least 256 texels deep, and texture arrays that are at least 256 layers deep.
			/// </para>
			/// </param>
			/// <param name="border">
			/// <para>
			/// This value must be 0.
			/// </para>
			/// </param>
			/// <param name="format">
			/// <para>
			/// Specifies the format of the pixel data. The following symbolic values are accepted: GL_RED, GL_RG, GL_RGB, GL_BGR, GL_RGBA, and GL_BGRA.
			/// </para>
			/// </param>
			/// <param name="type">
			/// <para>
			/// Specifies the data type of the pixel data. The following symbolic values are accepted: GL_UNSIGNED_BYTE, GL_BYTE, GL_UNSIGNED_SHORT, GL_SHORT, GL_UNSIGNED_INT, GL_INT, GL_FLOAT, GL_UNSIGNED_BYTE_3_3_2, GL_UNSIGNED_BYTE_2_3_3_REV, GL_UNSIGNED_SHORT_5_6_5, GL_UNSIGNED_SHORT_5_6_5_REV, GL_UNSIGNED_SHORT_4_4_4_4, GL_UNSIGNED_SHORT_4_4_4_4_REV, GL_UNSIGNED_SHORT_5_5_5_1, GL_UNSIGNED_SHORT_1_5_5_5_REV, GL_UNSIGNED_INT_8_8_8_8, GL_UNSIGNED_INT_8_8_8_8_REV, GL_UNSIGNED_INT_10_10_10_2, and GL_UNSIGNED_INT_2_10_10_10_REV.
			/// </para>
			/// </param>
			/// <param name="data">
			/// <para>
			/// Specifies a pointer to the image data in memory.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glTexImage3DOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void TexImage3D(System.Graphics.ES20.All target, Int32 level, System.Graphics.ES20.All internalformat, Int32 width, Int32 height, Int32 depth, Int32 border, System.Graphics.ES20.All format, System.Graphics.ES20.All type, IntPtr pixels) {
				Delegates.glTexImage3DOES((System.Graphics.ES20.All) target, (Int32) level, (System.Graphics.ES20.All) internalformat, (Int32) width, (Int32) height, (Int32) depth, (Int32) border, (System.Graphics.ES20.All) format, (System.Graphics.ES20.All) type, (IntPtr) pixels);
			}


			/// <summary>[requires: 2.0]
			/// Specify a three-dimensional texture image
			/// </summary>
			/// <param name="target">
			/// <para>
			/// Specifies the target texture. Must be one of GL_TEXTURE_3D, GL_PROXY_TEXTURE_3D, GL_TEXTURE_2D_ARRAY or GL_PROXY_TEXTURE_2D_ARRAY.
			/// </para>
			/// </param>
			/// <param name="level">
			/// <para>
			/// Specifies the level-of-detail number. Level 0 is the base image level. Level is the n sup th mipmap reduction image.
			/// </para>
			/// </param>
			/// <param name="internalFormat">
			/// <para>
			/// Specifies the number of color components in the texture. Must be one of the following symbolic constants: GL_RGBA32F, GL_RGBA32I, GL_RGBA32UI, GL_RGBA16, GL_RGBA16F, GL_RGBA16I, GL_RGBA16UI, GL_RGBA8, GL_RGBA8UI, GL_SRGB8_ALPHA8, GL_RGB10_A2, GL_RGBA10_A2UI, GL_R11_G11_B10F, GL_RG32F, GL_RG32I, GL_RG32UI, GL_RG16, GL_RG16F, GL_RGB16I, GL_RGB16UI, GL_RG8, GL_RG8I, GL_RG8UI, GL_R23F, GL_R32I, GL_R32UI, GL_R16F, GL_R16I, GL_R16UI, GL_R8, GL_R8I, GL_R8UI, GL_RGBA16_UNORM, GL_RGBA8_SNORM, GL_RGB32F, GL_RGB32I, GL_RGB32UI, GL_RGB16_SNORM, GL_RGB16F, GL_RGB16I, GL_RGB16UI, GL_RGB16, GL_RGB8_SNORM, GL_RGB8, GL_RGB8I, GL_RGB8UI, GL_SRGB8, GL_RGB9_E5, GL_RG16_SNORM, GL_RG8_SNORM, GL_COMPRESSED_RG_RGTC2, GL_COMPRESSED_SIGNED_RG_RGTC2, GL_R16_SNORM, GL_R8_SNORM, GL_COMPRESSED_RED_RGTC1, GL_COMPRESSED_SIGNED_RED_RGTC1, GL_DEPTH_COMPONENT32F, GL_DEPTH_COMPONENT24, GL_DEPTH_COMPONENT16, GL_DEPTH32F_STENCIL8, GL_DEPTH24_STENCIL8.
			/// </para>
			/// </param>
			/// <param name="width">
			/// <para>
			/// Specifies the width of the texture image. All implementations support 3D texture images that are at least 16 texels wide.
			/// </para>
			/// </param>
			/// <param name="height">
			/// <para>
			/// Specifies the height of the texture image. All implementations support 3D texture images that are at least 256 texels high.
			/// </para>
			/// </param>
			/// <param name="depth">
			/// <para>
			/// Specifies the depth of the texture image, or the number of layers in a texture array. All implementations support 3D texture images that are at least 256 texels deep, and texture arrays that are at least 256 layers deep.
			/// </para>
			/// </param>
			/// <param name="border">
			/// <para>
			/// This value must be 0.
			/// </para>
			/// </param>
			/// <param name="format">
			/// <para>
			/// Specifies the format of the pixel data. The following symbolic values are accepted: GL_RED, GL_RG, GL_RGB, GL_BGR, GL_RGBA, and GL_BGRA.
			/// </para>
			/// </param>
			/// <param name="type">
			/// <para>
			/// Specifies the data type of the pixel data. The following symbolic values are accepted: GL_UNSIGNED_BYTE, GL_BYTE, GL_UNSIGNED_SHORT, GL_SHORT, GL_UNSIGNED_INT, GL_INT, GL_FLOAT, GL_UNSIGNED_BYTE_3_3_2, GL_UNSIGNED_BYTE_2_3_3_REV, GL_UNSIGNED_SHORT_5_6_5, GL_UNSIGNED_SHORT_5_6_5_REV, GL_UNSIGNED_SHORT_4_4_4_4, GL_UNSIGNED_SHORT_4_4_4_4_REV, GL_UNSIGNED_SHORT_5_5_5_1, GL_UNSIGNED_SHORT_1_5_5_5_REV, GL_UNSIGNED_INT_8_8_8_8, GL_UNSIGNED_INT_8_8_8_8_REV, GL_UNSIGNED_INT_10_10_10_2, and GL_UNSIGNED_INT_2_10_10_10_REV.
			/// </para>
			/// </param>
			/// <param name="data">
			/// <para>
			/// Specifies a pointer to the image data in memory.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glTexImage3DOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void TexImage3D<T9>(System.Graphics.ES20.All target, Int32 level, System.Graphics.ES20.All internalformat, Int32 width, Int32 height, Int32 depth, Int32 border, System.Graphics.ES20.All format, System.Graphics.ES20.All type, [InAttribute, OutAttribute] T9[] pixels)
							where T9 : struct {
				GCHandle pixels_ptr = GCHandle.Alloc(pixels, GCHandleType.Pinned);
				try {
					Delegates.glTexImage3DOES((System.Graphics.ES20.All) target, (Int32) level, (System.Graphics.ES20.All) internalformat, (Int32) width, (Int32) height, (Int32) depth, (Int32) border, (System.Graphics.ES20.All) format, (System.Graphics.ES20.All) type, (IntPtr) pixels_ptr.AddrOfPinnedObject());
				} finally {
					pixels_ptr.Free();
				}
			}


			/// <summary>[requires: 2.0]
			/// Specify a three-dimensional texture image
			/// </summary>
			/// <param name="target">
			/// <para>
			/// Specifies the target texture. Must be one of GL_TEXTURE_3D, GL_PROXY_TEXTURE_3D, GL_TEXTURE_2D_ARRAY or GL_PROXY_TEXTURE_2D_ARRAY.
			/// </para>
			/// </param>
			/// <param name="level">
			/// <para>
			/// Specifies the level-of-detail number. Level 0 is the base image level. Level is the n sup th mipmap reduction image.
			/// </para>
			/// </param>
			/// <param name="internalFormat">
			/// <para>
			/// Specifies the number of color components in the texture. Must be one of the following symbolic constants: GL_RGBA32F, GL_RGBA32I, GL_RGBA32UI, GL_RGBA16, GL_RGBA16F, GL_RGBA16I, GL_RGBA16UI, GL_RGBA8, GL_RGBA8UI, GL_SRGB8_ALPHA8, GL_RGB10_A2, GL_RGBA10_A2UI, GL_R11_G11_B10F, GL_RG32F, GL_RG32I, GL_RG32UI, GL_RG16, GL_RG16F, GL_RGB16I, GL_RGB16UI, GL_RG8, GL_RG8I, GL_RG8UI, GL_R23F, GL_R32I, GL_R32UI, GL_R16F, GL_R16I, GL_R16UI, GL_R8, GL_R8I, GL_R8UI, GL_RGBA16_UNORM, GL_RGBA8_SNORM, GL_RGB32F, GL_RGB32I, GL_RGB32UI, GL_RGB16_SNORM, GL_RGB16F, GL_RGB16I, GL_RGB16UI, GL_RGB16, GL_RGB8_SNORM, GL_RGB8, GL_RGB8I, GL_RGB8UI, GL_SRGB8, GL_RGB9_E5, GL_RG16_SNORM, GL_RG8_SNORM, GL_COMPRESSED_RG_RGTC2, GL_COMPRESSED_SIGNED_RG_RGTC2, GL_R16_SNORM, GL_R8_SNORM, GL_COMPRESSED_RED_RGTC1, GL_COMPRESSED_SIGNED_RED_RGTC1, GL_DEPTH_COMPONENT32F, GL_DEPTH_COMPONENT24, GL_DEPTH_COMPONENT16, GL_DEPTH32F_STENCIL8, GL_DEPTH24_STENCIL8.
			/// </para>
			/// </param>
			/// <param name="width">
			/// <para>
			/// Specifies the width of the texture image. All implementations support 3D texture images that are at least 16 texels wide.
			/// </para>
			/// </param>
			/// <param name="height">
			/// <para>
			/// Specifies the height of the texture image. All implementations support 3D texture images that are at least 256 texels high.
			/// </para>
			/// </param>
			/// <param name="depth">
			/// <para>
			/// Specifies the depth of the texture image, or the number of layers in a texture array. All implementations support 3D texture images that are at least 256 texels deep, and texture arrays that are at least 256 layers deep.
			/// </para>
			/// </param>
			/// <param name="border">
			/// <para>
			/// This value must be 0.
			/// </para>
			/// </param>
			/// <param name="format">
			/// <para>
			/// Specifies the format of the pixel data. The following symbolic values are accepted: GL_RED, GL_RG, GL_RGB, GL_BGR, GL_RGBA, and GL_BGRA.
			/// </para>
			/// </param>
			/// <param name="type">
			/// <para>
			/// Specifies the data type of the pixel data. The following symbolic values are accepted: GL_UNSIGNED_BYTE, GL_BYTE, GL_UNSIGNED_SHORT, GL_SHORT, GL_UNSIGNED_INT, GL_INT, GL_FLOAT, GL_UNSIGNED_BYTE_3_3_2, GL_UNSIGNED_BYTE_2_3_3_REV, GL_UNSIGNED_SHORT_5_6_5, GL_UNSIGNED_SHORT_5_6_5_REV, GL_UNSIGNED_SHORT_4_4_4_4, GL_UNSIGNED_SHORT_4_4_4_4_REV, GL_UNSIGNED_SHORT_5_5_5_1, GL_UNSIGNED_SHORT_1_5_5_5_REV, GL_UNSIGNED_INT_8_8_8_8, GL_UNSIGNED_INT_8_8_8_8_REV, GL_UNSIGNED_INT_10_10_10_2, and GL_UNSIGNED_INT_2_10_10_10_REV.
			/// </para>
			/// </param>
			/// <param name="data">
			/// <para>
			/// Specifies a pointer to the image data in memory.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glTexImage3DOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void TexImage3D<T9>(System.Graphics.ES20.All target, Int32 level, System.Graphics.ES20.All internalformat, Int32 width, Int32 height, Int32 depth, Int32 border, System.Graphics.ES20.All format, System.Graphics.ES20.All type, [InAttribute, OutAttribute] T9[,] pixels)
							where T9 : struct {
				GCHandle pixels_ptr = GCHandle.Alloc(pixels, GCHandleType.Pinned);
				try {
					Delegates.glTexImage3DOES((System.Graphics.ES20.All) target, (Int32) level, (System.Graphics.ES20.All) internalformat, (Int32) width, (Int32) height, (Int32) depth, (Int32) border, (System.Graphics.ES20.All) format, (System.Graphics.ES20.All) type, (IntPtr) pixels_ptr.AddrOfPinnedObject());
				} finally {
					pixels_ptr.Free();
				}
			}


			/// <summary>[requires: 2.0]
			/// Specify a three-dimensional texture image
			/// </summary>
			/// <param name="target">
			/// <para>
			/// Specifies the target texture. Must be one of GL_TEXTURE_3D, GL_PROXY_TEXTURE_3D, GL_TEXTURE_2D_ARRAY or GL_PROXY_TEXTURE_2D_ARRAY.
			/// </para>
			/// </param>
			/// <param name="level">
			/// <para>
			/// Specifies the level-of-detail number. Level 0 is the base image level. Level is the n sup th mipmap reduction image.
			/// </para>
			/// </param>
			/// <param name="internalFormat">
			/// <para>
			/// Specifies the number of color components in the texture. Must be one of the following symbolic constants: GL_RGBA32F, GL_RGBA32I, GL_RGBA32UI, GL_RGBA16, GL_RGBA16F, GL_RGBA16I, GL_RGBA16UI, GL_RGBA8, GL_RGBA8UI, GL_SRGB8_ALPHA8, GL_RGB10_A2, GL_RGBA10_A2UI, GL_R11_G11_B10F, GL_RG32F, GL_RG32I, GL_RG32UI, GL_RG16, GL_RG16F, GL_RGB16I, GL_RGB16UI, GL_RG8, GL_RG8I, GL_RG8UI, GL_R23F, GL_R32I, GL_R32UI, GL_R16F, GL_R16I, GL_R16UI, GL_R8, GL_R8I, GL_R8UI, GL_RGBA16_UNORM, GL_RGBA8_SNORM, GL_RGB32F, GL_RGB32I, GL_RGB32UI, GL_RGB16_SNORM, GL_RGB16F, GL_RGB16I, GL_RGB16UI, GL_RGB16, GL_RGB8_SNORM, GL_RGB8, GL_RGB8I, GL_RGB8UI, GL_SRGB8, GL_RGB9_E5, GL_RG16_SNORM, GL_RG8_SNORM, GL_COMPRESSED_RG_RGTC2, GL_COMPRESSED_SIGNED_RG_RGTC2, GL_R16_SNORM, GL_R8_SNORM, GL_COMPRESSED_RED_RGTC1, GL_COMPRESSED_SIGNED_RED_RGTC1, GL_DEPTH_COMPONENT32F, GL_DEPTH_COMPONENT24, GL_DEPTH_COMPONENT16, GL_DEPTH32F_STENCIL8, GL_DEPTH24_STENCIL8.
			/// </para>
			/// </param>
			/// <param name="width">
			/// <para>
			/// Specifies the width of the texture image. All implementations support 3D texture images that are at least 16 texels wide.
			/// </para>
			/// </param>
			/// <param name="height">
			/// <para>
			/// Specifies the height of the texture image. All implementations support 3D texture images that are at least 256 texels high.
			/// </para>
			/// </param>
			/// <param name="depth">
			/// <para>
			/// Specifies the depth of the texture image, or the number of layers in a texture array. All implementations support 3D texture images that are at least 256 texels deep, and texture arrays that are at least 256 layers deep.
			/// </para>
			/// </param>
			/// <param name="border">
			/// <para>
			/// This value must be 0.
			/// </para>
			/// </param>
			/// <param name="format">
			/// <para>
			/// Specifies the format of the pixel data. The following symbolic values are accepted: GL_RED, GL_RG, GL_RGB, GL_BGR, GL_RGBA, and GL_BGRA.
			/// </para>
			/// </param>
			/// <param name="type">
			/// <para>
			/// Specifies the data type of the pixel data. The following symbolic values are accepted: GL_UNSIGNED_BYTE, GL_BYTE, GL_UNSIGNED_SHORT, GL_SHORT, GL_UNSIGNED_INT, GL_INT, GL_FLOAT, GL_UNSIGNED_BYTE_3_3_2, GL_UNSIGNED_BYTE_2_3_3_REV, GL_UNSIGNED_SHORT_5_6_5, GL_UNSIGNED_SHORT_5_6_5_REV, GL_UNSIGNED_SHORT_4_4_4_4, GL_UNSIGNED_SHORT_4_4_4_4_REV, GL_UNSIGNED_SHORT_5_5_5_1, GL_UNSIGNED_SHORT_1_5_5_5_REV, GL_UNSIGNED_INT_8_8_8_8, GL_UNSIGNED_INT_8_8_8_8_REV, GL_UNSIGNED_INT_10_10_10_2, and GL_UNSIGNED_INT_2_10_10_10_REV.
			/// </para>
			/// </param>
			/// <param name="data">
			/// <para>
			/// Specifies a pointer to the image data in memory.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glTexImage3DOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void TexImage3D<T9>(System.Graphics.ES20.All target, Int32 level, System.Graphics.ES20.All internalformat, Int32 width, Int32 height, Int32 depth, Int32 border, System.Graphics.ES20.All format, System.Graphics.ES20.All type, [InAttribute, OutAttribute] T9[,,] pixels)
							where T9 : struct {
				GCHandle pixels_ptr = GCHandle.Alloc(pixels, GCHandleType.Pinned);
				try {
					Delegates.glTexImage3DOES((System.Graphics.ES20.All) target, (Int32) level, (System.Graphics.ES20.All) internalformat, (Int32) width, (Int32) height, (Int32) depth, (Int32) border, (System.Graphics.ES20.All) format, (System.Graphics.ES20.All) type, (IntPtr) pixels_ptr.AddrOfPinnedObject());
				} finally {
					pixels_ptr.Free();
				}
			}


			/// <summary>[requires: 2.0]
			/// Specify a three-dimensional texture image
			/// </summary>
			/// <param name="target">
			/// <para>
			/// Specifies the target texture. Must be one of GL_TEXTURE_3D, GL_PROXY_TEXTURE_3D, GL_TEXTURE_2D_ARRAY or GL_PROXY_TEXTURE_2D_ARRAY.
			/// </para>
			/// </param>
			/// <param name="level">
			/// <para>
			/// Specifies the level-of-detail number. Level 0 is the base image level. Level is the n sup th mipmap reduction image.
			/// </para>
			/// </param>
			/// <param name="internalFormat">
			/// <para>
			/// Specifies the number of color components in the texture. Must be one of the following symbolic constants: GL_RGBA32F, GL_RGBA32I, GL_RGBA32UI, GL_RGBA16, GL_RGBA16F, GL_RGBA16I, GL_RGBA16UI, GL_RGBA8, GL_RGBA8UI, GL_SRGB8_ALPHA8, GL_RGB10_A2, GL_RGBA10_A2UI, GL_R11_G11_B10F, GL_RG32F, GL_RG32I, GL_RG32UI, GL_RG16, GL_RG16F, GL_RGB16I, GL_RGB16UI, GL_RG8, GL_RG8I, GL_RG8UI, GL_R23F, GL_R32I, GL_R32UI, GL_R16F, GL_R16I, GL_R16UI, GL_R8, GL_R8I, GL_R8UI, GL_RGBA16_UNORM, GL_RGBA8_SNORM, GL_RGB32F, GL_RGB32I, GL_RGB32UI, GL_RGB16_SNORM, GL_RGB16F, GL_RGB16I, GL_RGB16UI, GL_RGB16, GL_RGB8_SNORM, GL_RGB8, GL_RGB8I, GL_RGB8UI, GL_SRGB8, GL_RGB9_E5, GL_RG16_SNORM, GL_RG8_SNORM, GL_COMPRESSED_RG_RGTC2, GL_COMPRESSED_SIGNED_RG_RGTC2, GL_R16_SNORM, GL_R8_SNORM, GL_COMPRESSED_RED_RGTC1, GL_COMPRESSED_SIGNED_RED_RGTC1, GL_DEPTH_COMPONENT32F, GL_DEPTH_COMPONENT24, GL_DEPTH_COMPONENT16, GL_DEPTH32F_STENCIL8, GL_DEPTH24_STENCIL8.
			/// </para>
			/// </param>
			/// <param name="width">
			/// <para>
			/// Specifies the width of the texture image. All implementations support 3D texture images that are at least 16 texels wide.
			/// </para>
			/// </param>
			/// <param name="height">
			/// <para>
			/// Specifies the height of the texture image. All implementations support 3D texture images that are at least 256 texels high.
			/// </para>
			/// </param>
			/// <param name="depth">
			/// <para>
			/// Specifies the depth of the texture image, or the number of layers in a texture array. All implementations support 3D texture images that are at least 256 texels deep, and texture arrays that are at least 256 layers deep.
			/// </para>
			/// </param>
			/// <param name="border">
			/// <para>
			/// This value must be 0.
			/// </para>
			/// </param>
			/// <param name="format">
			/// <para>
			/// Specifies the format of the pixel data. The following symbolic values are accepted: GL_RED, GL_RG, GL_RGB, GL_BGR, GL_RGBA, and GL_BGRA.
			/// </para>
			/// </param>
			/// <param name="type">
			/// <para>
			/// Specifies the data type of the pixel data. The following symbolic values are accepted: GL_UNSIGNED_BYTE, GL_BYTE, GL_UNSIGNED_SHORT, GL_SHORT, GL_UNSIGNED_INT, GL_INT, GL_FLOAT, GL_UNSIGNED_BYTE_3_3_2, GL_UNSIGNED_BYTE_2_3_3_REV, GL_UNSIGNED_SHORT_5_6_5, GL_UNSIGNED_SHORT_5_6_5_REV, GL_UNSIGNED_SHORT_4_4_4_4, GL_UNSIGNED_SHORT_4_4_4_4_REV, GL_UNSIGNED_SHORT_5_5_5_1, GL_UNSIGNED_SHORT_1_5_5_5_REV, GL_UNSIGNED_INT_8_8_8_8, GL_UNSIGNED_INT_8_8_8_8_REV, GL_UNSIGNED_INT_10_10_10_2, and GL_UNSIGNED_INT_2_10_10_10_REV.
			/// </para>
			/// </param>
			/// <param name="data">
			/// <para>
			/// Specifies a pointer to the image data in memory.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glTexImage3DOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void TexImage3D<T9>(System.Graphics.ES20.All target, Int32 level, System.Graphics.ES20.All internalformat, Int32 width, Int32 height, Int32 depth, Int32 border, System.Graphics.ES20.All format, System.Graphics.ES20.All type, [InAttribute, OutAttribute] ref T9 pixels)
							where T9 : struct {
				GCHandle pixels_ptr = GCHandle.Alloc(pixels, GCHandleType.Pinned);
				try {
					Delegates.glTexImage3DOES((System.Graphics.ES20.All) target, (Int32) level, (System.Graphics.ES20.All) internalformat, (Int32) width, (Int32) height, (Int32) depth, (Int32) border, (System.Graphics.ES20.All) format, (System.Graphics.ES20.All) type, (IntPtr) pixels_ptr.AddrOfPinnedObject());
					pixels = (T9) pixels_ptr.Target;
				} finally {
					pixels_ptr.Free();
				}
			}


			/// <summary>[requires: 2.0]
			/// Specify a three-dimensional texture subimage
			/// </summary>
			/// <param name="target">
			/// <para>
			/// Specifies the target texture. Must be GL_TEXTURE_3D.
			/// </para>
			/// </param>
			/// <param name="level">
			/// <para>
			/// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap reduction image.
			/// </para>
			/// </param>
			/// <param name="xoffset">
			/// <para>
			/// Specifies a texel offset in the x direction within the texture array.
			/// </para>
			/// </param>
			/// <param name="yoffset">
			/// <para>
			/// Specifies a texel offset in the y direction within the texture array.
			/// </para>
			/// </param>
			/// <param name="zoffset">
			/// <para>
			/// Specifies a texel offset in the z direction within the texture array.
			/// </para>
			/// </param>
			/// <param name="width">
			/// <para>
			/// Specifies the width of the texture subimage.
			/// </para>
			/// </param>
			/// <param name="height">
			/// <para>
			/// Specifies the height of the texture subimage.
			/// </para>
			/// </param>
			/// <param name="depth">
			/// <para>
			/// Specifies the depth of the texture subimage.
			/// </para>
			/// </param>
			/// <param name="format">
			/// <para>
			/// Specifies the format of the pixel data. The following symbolic values are accepted: GL_RED, GL_RG, GL_RGB, GL_BGR, GL_RGBA, and GL_BGRA.
			/// </para>
			/// </param>
			/// <param name="type">
			/// <para>
			/// Specifies the data type of the pixel data. The following symbolic values are accepted: GL_UNSIGNED_BYTE, GL_BYTE, GL_UNSIGNED_SHORT, GL_SHORT, GL_UNSIGNED_INT, GL_INT, GL_FLOAT, GL_UNSIGNED_BYTE_3_3_2, GL_UNSIGNED_BYTE_2_3_3_REV, GL_UNSIGNED_SHORT_5_6_5, GL_UNSIGNED_SHORT_5_6_5_REV, GL_UNSIGNED_SHORT_4_4_4_4, GL_UNSIGNED_SHORT_4_4_4_4_REV, GL_UNSIGNED_SHORT_5_5_5_1, GL_UNSIGNED_SHORT_1_5_5_5_REV, GL_UNSIGNED_INT_8_8_8_8, GL_UNSIGNED_INT_8_8_8_8_REV, GL_UNSIGNED_INT_10_10_10_2, and GL_UNSIGNED_INT_2_10_10_10_REV.
			/// </para>
			/// </param>
			/// <param name="data">
			/// <para>
			/// Specifies a pointer to the image data in memory.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glTexSubImage3DOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void TexSubImage3D(System.Graphics.ES20.All target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 width, Int32 height, Int32 depth, System.Graphics.ES20.All format, System.Graphics.ES20.All type, IntPtr pixels) {
				Delegates.glTexSubImage3DOES((System.Graphics.ES20.All) target, (Int32) level, (Int32) xoffset, (Int32) yoffset, (Int32) zoffset, (Int32) width, (Int32) height, (Int32) depth, (System.Graphics.ES20.All) format, (System.Graphics.ES20.All) type, (IntPtr) pixels);
			}


			/// <summary>[requires: 2.0]
			/// Specify a three-dimensional texture subimage
			/// </summary>
			/// <param name="target">
			/// <para>
			/// Specifies the target texture. Must be GL_TEXTURE_3D.
			/// </para>
			/// </param>
			/// <param name="level">
			/// <para>
			/// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap reduction image.
			/// </para>
			/// </param>
			/// <param name="xoffset">
			/// <para>
			/// Specifies a texel offset in the x direction within the texture array.
			/// </para>
			/// </param>
			/// <param name="yoffset">
			/// <para>
			/// Specifies a texel offset in the y direction within the texture array.
			/// </para>
			/// </param>
			/// <param name="zoffset">
			/// <para>
			/// Specifies a texel offset in the z direction within the texture array.
			/// </para>
			/// </param>
			/// <param name="width">
			/// <para>
			/// Specifies the width of the texture subimage.
			/// </para>
			/// </param>
			/// <param name="height">
			/// <para>
			/// Specifies the height of the texture subimage.
			/// </para>
			/// </param>
			/// <param name="depth">
			/// <para>
			/// Specifies the depth of the texture subimage.
			/// </para>
			/// </param>
			/// <param name="format">
			/// <para>
			/// Specifies the format of the pixel data. The following symbolic values are accepted: GL_RED, GL_RG, GL_RGB, GL_BGR, GL_RGBA, and GL_BGRA.
			/// </para>
			/// </param>
			/// <param name="type">
			/// <para>
			/// Specifies the data type of the pixel data. The following symbolic values are accepted: GL_UNSIGNED_BYTE, GL_BYTE, GL_UNSIGNED_SHORT, GL_SHORT, GL_UNSIGNED_INT, GL_INT, GL_FLOAT, GL_UNSIGNED_BYTE_3_3_2, GL_UNSIGNED_BYTE_2_3_3_REV, GL_UNSIGNED_SHORT_5_6_5, GL_UNSIGNED_SHORT_5_6_5_REV, GL_UNSIGNED_SHORT_4_4_4_4, GL_UNSIGNED_SHORT_4_4_4_4_REV, GL_UNSIGNED_SHORT_5_5_5_1, GL_UNSIGNED_SHORT_1_5_5_5_REV, GL_UNSIGNED_INT_8_8_8_8, GL_UNSIGNED_INT_8_8_8_8_REV, GL_UNSIGNED_INT_10_10_10_2, and GL_UNSIGNED_INT_2_10_10_10_REV.
			/// </para>
			/// </param>
			/// <param name="data">
			/// <para>
			/// Specifies a pointer to the image data in memory.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glTexSubImage3DOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void TexSubImage3D<T10>(System.Graphics.ES20.All target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 width, Int32 height, Int32 depth, System.Graphics.ES20.All format, System.Graphics.ES20.All type, [InAttribute, OutAttribute] T10[] pixels)
							where T10 : struct {
				GCHandle pixels_ptr = GCHandle.Alloc(pixels, GCHandleType.Pinned);
				try {
					Delegates.glTexSubImage3DOES((System.Graphics.ES20.All) target, (Int32) level, (Int32) xoffset, (Int32) yoffset, (Int32) zoffset, (Int32) width, (Int32) height, (Int32) depth, (System.Graphics.ES20.All) format, (System.Graphics.ES20.All) type, (IntPtr) pixels_ptr.AddrOfPinnedObject());
				} finally {
					pixels_ptr.Free();
				}
			}


			/// <summary>[requires: 2.0]
			/// Specify a three-dimensional texture subimage
			/// </summary>
			/// <param name="target">
			/// <para>
			/// Specifies the target texture. Must be GL_TEXTURE_3D.
			/// </para>
			/// </param>
			/// <param name="level">
			/// <para>
			/// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap reduction image.
			/// </para>
			/// </param>
			/// <param name="xoffset">
			/// <para>
			/// Specifies a texel offset in the x direction within the texture array.
			/// </para>
			/// </param>
			/// <param name="yoffset">
			/// <para>
			/// Specifies a texel offset in the y direction within the texture array.
			/// </para>
			/// </param>
			/// <param name="zoffset">
			/// <para>
			/// Specifies a texel offset in the z direction within the texture array.
			/// </para>
			/// </param>
			/// <param name="width">
			/// <para>
			/// Specifies the width of the texture subimage.
			/// </para>
			/// </param>
			/// <param name="height">
			/// <para>
			/// Specifies the height of the texture subimage.
			/// </para>
			/// </param>
			/// <param name="depth">
			/// <para>
			/// Specifies the depth of the texture subimage.
			/// </para>
			/// </param>
			/// <param name="format">
			/// <para>
			/// Specifies the format of the pixel data. The following symbolic values are accepted: GL_RED, GL_RG, GL_RGB, GL_BGR, GL_RGBA, and GL_BGRA.
			/// </para>
			/// </param>
			/// <param name="type">
			/// <para>
			/// Specifies the data type of the pixel data. The following symbolic values are accepted: GL_UNSIGNED_BYTE, GL_BYTE, GL_UNSIGNED_SHORT, GL_SHORT, GL_UNSIGNED_INT, GL_INT, GL_FLOAT, GL_UNSIGNED_BYTE_3_3_2, GL_UNSIGNED_BYTE_2_3_3_REV, GL_UNSIGNED_SHORT_5_6_5, GL_UNSIGNED_SHORT_5_6_5_REV, GL_UNSIGNED_SHORT_4_4_4_4, GL_UNSIGNED_SHORT_4_4_4_4_REV, GL_UNSIGNED_SHORT_5_5_5_1, GL_UNSIGNED_SHORT_1_5_5_5_REV, GL_UNSIGNED_INT_8_8_8_8, GL_UNSIGNED_INT_8_8_8_8_REV, GL_UNSIGNED_INT_10_10_10_2, and GL_UNSIGNED_INT_2_10_10_10_REV.
			/// </para>
			/// </param>
			/// <param name="data">
			/// <para>
			/// Specifies a pointer to the image data in memory.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glTexSubImage3DOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void TexSubImage3D<T10>(System.Graphics.ES20.All target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 width, Int32 height, Int32 depth, System.Graphics.ES20.All format, System.Graphics.ES20.All type, [InAttribute, OutAttribute] T10[,] pixels)
							where T10 : struct {
				GCHandle pixels_ptr = GCHandle.Alloc(pixels, GCHandleType.Pinned);
				try {
					Delegates.glTexSubImage3DOES((System.Graphics.ES20.All) target, (Int32) level, (Int32) xoffset, (Int32) yoffset, (Int32) zoffset, (Int32) width, (Int32) height, (Int32) depth, (System.Graphics.ES20.All) format, (System.Graphics.ES20.All) type, (IntPtr) pixels_ptr.AddrOfPinnedObject());
				} finally {
					pixels_ptr.Free();
				}
			}


			/// <summary>[requires: 2.0]
			/// Specify a three-dimensional texture subimage
			/// </summary>
			/// <param name="target">
			/// <para>
			/// Specifies the target texture. Must be GL_TEXTURE_3D.
			/// </para>
			/// </param>
			/// <param name="level">
			/// <para>
			/// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap reduction image.
			/// </para>
			/// </param>
			/// <param name="xoffset">
			/// <para>
			/// Specifies a texel offset in the x direction within the texture array.
			/// </para>
			/// </param>
			/// <param name="yoffset">
			/// <para>
			/// Specifies a texel offset in the y direction within the texture array.
			/// </para>
			/// </param>
			/// <param name="zoffset">
			/// <para>
			/// Specifies a texel offset in the z direction within the texture array.
			/// </para>
			/// </param>
			/// <param name="width">
			/// <para>
			/// Specifies the width of the texture subimage.
			/// </para>
			/// </param>
			/// <param name="height">
			/// <para>
			/// Specifies the height of the texture subimage.
			/// </para>
			/// </param>
			/// <param name="depth">
			/// <para>
			/// Specifies the depth of the texture subimage.
			/// </para>
			/// </param>
			/// <param name="format">
			/// <para>
			/// Specifies the format of the pixel data. The following symbolic values are accepted: GL_RED, GL_RG, GL_RGB, GL_BGR, GL_RGBA, and GL_BGRA.
			/// </para>
			/// </param>
			/// <param name="type">
			/// <para>
			/// Specifies the data type of the pixel data. The following symbolic values are accepted: GL_UNSIGNED_BYTE, GL_BYTE, GL_UNSIGNED_SHORT, GL_SHORT, GL_UNSIGNED_INT, GL_INT, GL_FLOAT, GL_UNSIGNED_BYTE_3_3_2, GL_UNSIGNED_BYTE_2_3_3_REV, GL_UNSIGNED_SHORT_5_6_5, GL_UNSIGNED_SHORT_5_6_5_REV, GL_UNSIGNED_SHORT_4_4_4_4, GL_UNSIGNED_SHORT_4_4_4_4_REV, GL_UNSIGNED_SHORT_5_5_5_1, GL_UNSIGNED_SHORT_1_5_5_5_REV, GL_UNSIGNED_INT_8_8_8_8, GL_UNSIGNED_INT_8_8_8_8_REV, GL_UNSIGNED_INT_10_10_10_2, and GL_UNSIGNED_INT_2_10_10_10_REV.
			/// </para>
			/// </param>
			/// <param name="data">
			/// <para>
			/// Specifies a pointer to the image data in memory.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glTexSubImage3DOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void TexSubImage3D<T10>(System.Graphics.ES20.All target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 width, Int32 height, Int32 depth, System.Graphics.ES20.All format, System.Graphics.ES20.All type, [InAttribute, OutAttribute] T10[,,] pixels)
							where T10 : struct {
				GCHandle pixels_ptr = GCHandle.Alloc(pixels, GCHandleType.Pinned);
				try {
					Delegates.glTexSubImage3DOES((System.Graphics.ES20.All) target, (Int32) level, (Int32) xoffset, (Int32) yoffset, (Int32) zoffset, (Int32) width, (Int32) height, (Int32) depth, (System.Graphics.ES20.All) format, (System.Graphics.ES20.All) type, (IntPtr) pixels_ptr.AddrOfPinnedObject());
				} finally {
					pixels_ptr.Free();
				}
			}


			/// <summary>[requires: 2.0]
			/// Specify a three-dimensional texture subimage
			/// </summary>
			/// <param name="target">
			/// <para>
			/// Specifies the target texture. Must be GL_TEXTURE_3D.
			/// </para>
			/// </param>
			/// <param name="level">
			/// <para>
			/// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap reduction image.
			/// </para>
			/// </param>
			/// <param name="xoffset">
			/// <para>
			/// Specifies a texel offset in the x direction within the texture array.
			/// </para>
			/// </param>
			/// <param name="yoffset">
			/// <para>
			/// Specifies a texel offset in the y direction within the texture array.
			/// </para>
			/// </param>
			/// <param name="zoffset">
			/// <para>
			/// Specifies a texel offset in the z direction within the texture array.
			/// </para>
			/// </param>
			/// <param name="width">
			/// <para>
			/// Specifies the width of the texture subimage.
			/// </para>
			/// </param>
			/// <param name="height">
			/// <para>
			/// Specifies the height of the texture subimage.
			/// </para>
			/// </param>
			/// <param name="depth">
			/// <para>
			/// Specifies the depth of the texture subimage.
			/// </para>
			/// </param>
			/// <param name="format">
			/// <para>
			/// Specifies the format of the pixel data. The following symbolic values are accepted: GL_RED, GL_RG, GL_RGB, GL_BGR, GL_RGBA, and GL_BGRA.
			/// </para>
			/// </param>
			/// <param name="type">
			/// <para>
			/// Specifies the data type of the pixel data. The following symbolic values are accepted: GL_UNSIGNED_BYTE, GL_BYTE, GL_UNSIGNED_SHORT, GL_SHORT, GL_UNSIGNED_INT, GL_INT, GL_FLOAT, GL_UNSIGNED_BYTE_3_3_2, GL_UNSIGNED_BYTE_2_3_3_REV, GL_UNSIGNED_SHORT_5_6_5, GL_UNSIGNED_SHORT_5_6_5_REV, GL_UNSIGNED_SHORT_4_4_4_4, GL_UNSIGNED_SHORT_4_4_4_4_REV, GL_UNSIGNED_SHORT_5_5_5_1, GL_UNSIGNED_SHORT_1_5_5_5_REV, GL_UNSIGNED_INT_8_8_8_8, GL_UNSIGNED_INT_8_8_8_8_REV, GL_UNSIGNED_INT_10_10_10_2, and GL_UNSIGNED_INT_2_10_10_10_REV.
			/// </para>
			/// </param>
			/// <param name="data">
			/// <para>
			/// Specifies a pointer to the image data in memory.
			/// </para>
			/// </param>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glTexSubImage3DOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void TexSubImage3D<T10>(System.Graphics.ES20.All target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 width, Int32 height, Int32 depth, System.Graphics.ES20.All format, System.Graphics.ES20.All type, [InAttribute, OutAttribute] ref T10 pixels)
							where T10 : struct {
				GCHandle pixels_ptr = GCHandle.Alloc(pixels, GCHandleType.Pinned);
				try {
					Delegates.glTexSubImage3DOES((System.Graphics.ES20.All) target, (Int32) level, (Int32) xoffset, (Int32) yoffset, (Int32) zoffset, (Int32) width, (Int32) height, (Int32) depth, (System.Graphics.ES20.All) format, (System.Graphics.ES20.All) type, (IntPtr) pixels_ptr.AddrOfPinnedObject());
					pixels = (T10) pixels_ptr.Target;
				} finally {
					pixels_ptr.Free();
				}
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glUnmapBufferOES")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						bool UnmapBuffer(System.Graphics.ES20.All target) {
				return Delegates.glUnmapBufferOES((System.Graphics.ES20.All) target);
			}

		}

		/// <summary>
		/// Qualcomm extensions
		/// </summary>
		public static class Qcom {
			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDisableDriverControlQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void DisableDriverControl(Int32 driverControl) {
				Delegates.glDisableDriverControlQCOM((UInt32) driverControl);
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glDisableDriverControlQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void DisableDriverControl(UInt32 driverControl) {
				Delegates.glDisableDriverControlQCOM((UInt32) driverControl);
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glEnableDriverControlQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void EnableDriverControl(Int32 driverControl) {
				Delegates.glEnableDriverControlQCOM((UInt32) driverControl);
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glEnableDriverControlQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void EnableDriverControl(UInt32 driverControl) {
				Delegates.glEnableDriverControlQCOM((UInt32) driverControl);
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glEndTilingQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void EndTiling(Int32 preserveMask) {
				Delegates.glEndTilingQCOM((UInt32) preserveMask);
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glEndTilingQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void EndTiling(UInt32 preserveMask) {
				Delegates.glEndTilingQCOM((UInt32) preserveMask);
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetBufferPointervQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void ExtGetBufferPointer(System.Graphics.ES20.All target, IntPtr @params) {
				Delegates.glExtGetBufferPointervQCOM((System.Graphics.ES20.All) target, (IntPtr) @params);
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetBufferPointervQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void ExtGetBufferPointer<T1>(System.Graphics.ES20.All target, [InAttribute, OutAttribute] T1[] @params)
							where T1 : struct {
				GCHandle @params_ptr = GCHandle.Alloc(@params, GCHandleType.Pinned);
				try {
					Delegates.glExtGetBufferPointervQCOM((System.Graphics.ES20.All) target, (IntPtr) @params_ptr.AddrOfPinnedObject());
				} finally {
					@params_ptr.Free();
				}
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetBufferPointervQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void ExtGetBufferPointer<T1>(System.Graphics.ES20.All target, [InAttribute, OutAttribute] T1[,] @params)
							where T1 : struct {
				GCHandle @params_ptr = GCHandle.Alloc(@params, GCHandleType.Pinned);
				try {
					Delegates.glExtGetBufferPointervQCOM((System.Graphics.ES20.All) target, (IntPtr) @params_ptr.AddrOfPinnedObject());
				} finally {
					@params_ptr.Free();
				}
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetBufferPointervQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void ExtGetBufferPointer<T1>(System.Graphics.ES20.All target, [InAttribute, OutAttribute] T1[,,] @params)
							where T1 : struct {
				GCHandle @params_ptr = GCHandle.Alloc(@params, GCHandleType.Pinned);
				try {
					Delegates.glExtGetBufferPointervQCOM((System.Graphics.ES20.All) target, (IntPtr) @params_ptr.AddrOfPinnedObject());
				} finally {
					@params_ptr.Free();
				}
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetBufferPointervQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void ExtGetBufferPointer<T1>(System.Graphics.ES20.All target, [InAttribute, OutAttribute] ref T1 @params)
							where T1 : struct {
				GCHandle @params_ptr = GCHandle.Alloc(@params, GCHandleType.Pinned);
				try {
					Delegates.glExtGetBufferPointervQCOM((System.Graphics.ES20.All) target, (IntPtr) @params_ptr.AddrOfPinnedObject());
					@params = (T1) @params_ptr.Target;
				} finally {
					@params_ptr.Free();
				}
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetBuffersQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void ExtGetBuffers(Int32[] buffers, Int32 maxBuffers, Int32[] numBuffers) {
				unsafe
				{
					fixed (Int32* buffers_ptr = buffers)
					fixed (Int32* numBuffers_ptr = numBuffers) {
						Delegates.glExtGetBuffersQCOM((UInt32*) buffers_ptr, (Int32) maxBuffers, (Int32*) numBuffers_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetBuffersQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void ExtGetBuffers(ref Int32 buffers, Int32 maxBuffers, ref Int32 numBuffers) {
				unsafe
				{
					fixed (Int32* buffers_ptr = &buffers)
					fixed (Int32* numBuffers_ptr = &numBuffers) {
						Delegates.glExtGetBuffersQCOM((UInt32*) buffers_ptr, (Int32) maxBuffers, (Int32*) numBuffers_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetBuffersQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void ExtGetBuffers(Int32* buffers, Int32 maxBuffers, Int32* numBuffers) {
				Delegates.glExtGetBuffersQCOM((UInt32*) buffers, (Int32) maxBuffers, (Int32*) numBuffers);
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetBuffersQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void ExtGetBuffers(UInt32[] buffers, Int32 maxBuffers, Int32[] numBuffers) {
				unsafe
				{
					fixed (UInt32* buffers_ptr = buffers)
					fixed (Int32* numBuffers_ptr = numBuffers) {
						Delegates.glExtGetBuffersQCOM((UInt32*) buffers_ptr, (Int32) maxBuffers, (Int32*) numBuffers_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetBuffersQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void ExtGetBuffers(ref UInt32 buffers, Int32 maxBuffers, ref Int32 numBuffers) {
				unsafe
				{
					fixed (UInt32* buffers_ptr = &buffers)
					fixed (Int32* numBuffers_ptr = &numBuffers) {
						Delegates.glExtGetBuffersQCOM((UInt32*) buffers_ptr, (Int32) maxBuffers, (Int32*) numBuffers_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetBuffersQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void ExtGetBuffers(UInt32* buffers, Int32 maxBuffers, Int32* numBuffers) {
				Delegates.glExtGetBuffersQCOM((UInt32*) buffers, (Int32) maxBuffers, (Int32*) numBuffers);
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetFramebuffersQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void ExtGetFramebuffers(Int32[] framebuffers, Int32 maxFramebuffers, Int32[] numFramebuffers) {
				unsafe
				{
					fixed (Int32* framebuffers_ptr = framebuffers)
					fixed (Int32* numFramebuffers_ptr = numFramebuffers) {
						Delegates.glExtGetFramebuffersQCOM((UInt32*) framebuffers_ptr, (Int32) maxFramebuffers, (Int32*) numFramebuffers_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetFramebuffersQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void ExtGetFramebuffers(ref Int32 framebuffers, Int32 maxFramebuffers, ref Int32 numFramebuffers) {
				unsafe
				{
					fixed (Int32* framebuffers_ptr = &framebuffers)
					fixed (Int32* numFramebuffers_ptr = &numFramebuffers) {
						Delegates.glExtGetFramebuffersQCOM((UInt32*) framebuffers_ptr, (Int32) maxFramebuffers, (Int32*) numFramebuffers_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetFramebuffersQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void ExtGetFramebuffers(Int32* framebuffers, Int32 maxFramebuffers, Int32* numFramebuffers) {
				Delegates.glExtGetFramebuffersQCOM((UInt32*) framebuffers, (Int32) maxFramebuffers, (Int32*) numFramebuffers);
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetFramebuffersQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void ExtGetFramebuffers(UInt32[] framebuffers, Int32 maxFramebuffers, Int32[] numFramebuffers) {
				unsafe
				{
					fixed (UInt32* framebuffers_ptr = framebuffers)
					fixed (Int32* numFramebuffers_ptr = numFramebuffers) {
						Delegates.glExtGetFramebuffersQCOM((UInt32*) framebuffers_ptr, (Int32) maxFramebuffers, (Int32*) numFramebuffers_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetFramebuffersQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void ExtGetFramebuffers(ref UInt32 framebuffers, Int32 maxFramebuffers, ref Int32 numFramebuffers) {
				unsafe
				{
					fixed (UInt32* framebuffers_ptr = &framebuffers)
					fixed (Int32* numFramebuffers_ptr = &numFramebuffers) {
						Delegates.glExtGetFramebuffersQCOM((UInt32*) framebuffers_ptr, (Int32) maxFramebuffers, (Int32*) numFramebuffers_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetFramebuffersQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void ExtGetFramebuffers(UInt32* framebuffers, Int32 maxFramebuffers, Int32* numFramebuffers) {
				Delegates.glExtGetFramebuffersQCOM((UInt32*) framebuffers, (Int32) maxFramebuffers, (Int32*) numFramebuffers);
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetProgramBinarySourceQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void ExtGetProgramBinarySource(Int32 program, System.Graphics.ES20.All shadertype, String source, Int32[] length) {
				unsafe
				{
					fixed (Int32* length_ptr = length) {
						Delegates.glExtGetProgramBinarySourceQCOM((UInt32) program, (System.Graphics.ES20.All) shadertype, (String) source, (Int32*) length_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetProgramBinarySourceQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void ExtGetProgramBinarySource(Int32 program, System.Graphics.ES20.All shadertype, String source, ref Int32 length) {
				unsafe
				{
					fixed (Int32* length_ptr = &length) {
						Delegates.glExtGetProgramBinarySourceQCOM((UInt32) program, (System.Graphics.ES20.All) shadertype, (String) source, (Int32*) length_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetProgramBinarySourceQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void ExtGetProgramBinarySource(Int32 program, System.Graphics.ES20.All shadertype, String source, Int32* length) {
				Delegates.glExtGetProgramBinarySourceQCOM((UInt32) program, (System.Graphics.ES20.All) shadertype, (String) source, (Int32*) length);
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetProgramBinarySourceQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void ExtGetProgramBinarySource(UInt32 program, System.Graphics.ES20.All shadertype, String source, Int32[] length) {
				unsafe
				{
					fixed (Int32* length_ptr = length) {
						Delegates.glExtGetProgramBinarySourceQCOM((UInt32) program, (System.Graphics.ES20.All) shadertype, (String) source, (Int32*) length_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetProgramBinarySourceQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void ExtGetProgramBinarySource(UInt32 program, System.Graphics.ES20.All shadertype, String source, ref Int32 length) {
				unsafe
				{
					fixed (Int32* length_ptr = &length) {
						Delegates.glExtGetProgramBinarySourceQCOM((UInt32) program, (System.Graphics.ES20.All) shadertype, (String) source, (Int32*) length_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetProgramBinarySourceQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void ExtGetProgramBinarySource(UInt32 program, System.Graphics.ES20.All shadertype, String source, Int32* length) {
				Delegates.glExtGetProgramBinarySourceQCOM((UInt32) program, (System.Graphics.ES20.All) shadertype, (String) source, (Int32*) length);
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetProgramsQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void ExtGetProgram(Int32[] programs, Int32 maxPrograms, Int32[] numPrograms) {
				unsafe
				{
					fixed (Int32* programs_ptr = programs)
					fixed (Int32* numPrograms_ptr = numPrograms) {
						Delegates.glExtGetProgramsQCOM((UInt32*) programs_ptr, (Int32) maxPrograms, (Int32*) numPrograms_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetProgramsQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void ExtGetProgram(ref Int32 programs, Int32 maxPrograms, ref Int32 numPrograms) {
				unsafe
				{
					fixed (Int32* programs_ptr = &programs)
					fixed (Int32* numPrograms_ptr = &numPrograms) {
						Delegates.glExtGetProgramsQCOM((UInt32*) programs_ptr, (Int32) maxPrograms, (Int32*) numPrograms_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetProgramsQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void ExtGetProgram(Int32* programs, Int32 maxPrograms, Int32* numPrograms) {
				Delegates.glExtGetProgramsQCOM((UInt32*) programs, (Int32) maxPrograms, (Int32*) numPrograms);
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetProgramsQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void ExtGetProgram(UInt32[] programs, Int32 maxPrograms, Int32[] numPrograms) {
				unsafe
				{
					fixed (UInt32* programs_ptr = programs)
					fixed (Int32* numPrograms_ptr = numPrograms) {
						Delegates.glExtGetProgramsQCOM((UInt32*) programs_ptr, (Int32) maxPrograms, (Int32*) numPrograms_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetProgramsQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void ExtGetProgram(ref UInt32 programs, Int32 maxPrograms, ref Int32 numPrograms) {
				unsafe
				{
					fixed (UInt32* programs_ptr = &programs)
					fixed (Int32* numPrograms_ptr = &numPrograms) {
						Delegates.glExtGetProgramsQCOM((UInt32*) programs_ptr, (Int32) maxPrograms, (Int32*) numPrograms_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetProgramsQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void ExtGetProgram(UInt32* programs, Int32 maxPrograms, Int32* numPrograms) {
				Delegates.glExtGetProgramsQCOM((UInt32*) programs, (Int32) maxPrograms, (Int32*) numPrograms);
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetRenderbuffersQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void ExtGetRenderbuffers(Int32[] renderbuffers, Int32 maxRenderbuffers, Int32[] numRenderbuffers) {
				unsafe
				{
					fixed (Int32* renderbuffers_ptr = renderbuffers)
					fixed (Int32* numRenderbuffers_ptr = numRenderbuffers) {
						Delegates.glExtGetRenderbuffersQCOM((UInt32*) renderbuffers_ptr, (Int32) maxRenderbuffers, (Int32*) numRenderbuffers_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetRenderbuffersQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void ExtGetRenderbuffers(ref Int32 renderbuffers, Int32 maxRenderbuffers, ref Int32 numRenderbuffers) {
				unsafe
				{
					fixed (Int32* renderbuffers_ptr = &renderbuffers)
					fixed (Int32* numRenderbuffers_ptr = &numRenderbuffers) {
						Delegates.glExtGetRenderbuffersQCOM((UInt32*) renderbuffers_ptr, (Int32) maxRenderbuffers, (Int32*) numRenderbuffers_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetRenderbuffersQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void ExtGetRenderbuffers(Int32* renderbuffers, Int32 maxRenderbuffers, Int32* numRenderbuffers) {
				Delegates.glExtGetRenderbuffersQCOM((UInt32*) renderbuffers, (Int32) maxRenderbuffers, (Int32*) numRenderbuffers);
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetRenderbuffersQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void ExtGetRenderbuffers(UInt32[] renderbuffers, Int32 maxRenderbuffers, Int32[] numRenderbuffers) {
				unsafe
				{
					fixed (UInt32* renderbuffers_ptr = renderbuffers)
					fixed (Int32* numRenderbuffers_ptr = numRenderbuffers) {
						Delegates.glExtGetRenderbuffersQCOM((UInt32*) renderbuffers_ptr, (Int32) maxRenderbuffers, (Int32*) numRenderbuffers_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetRenderbuffersQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void ExtGetRenderbuffers(ref UInt32 renderbuffers, Int32 maxRenderbuffers, ref Int32 numRenderbuffers) {
				unsafe
				{
					fixed (UInt32* renderbuffers_ptr = &renderbuffers)
					fixed (Int32* numRenderbuffers_ptr = &numRenderbuffers) {
						Delegates.glExtGetRenderbuffersQCOM((UInt32*) renderbuffers_ptr, (Int32) maxRenderbuffers, (Int32*) numRenderbuffers_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetRenderbuffersQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void ExtGetRenderbuffers(UInt32* renderbuffers, Int32 maxRenderbuffers, Int32* numRenderbuffers) {
				Delegates.glExtGetRenderbuffersQCOM((UInt32*) renderbuffers, (Int32) maxRenderbuffers, (Int32*) numRenderbuffers);
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetShadersQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void ExtGetShaders(Int32[] shaders, Int32 maxShaders, Int32[] numShaders) {
				unsafe
				{
					fixed (Int32* shaders_ptr = shaders)
					fixed (Int32* numShaders_ptr = numShaders) {
						Delegates.glExtGetShadersQCOM((UInt32*) shaders_ptr, (Int32) maxShaders, (Int32*) numShaders_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetShadersQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void ExtGetShaders(ref Int32 shaders, Int32 maxShaders, ref Int32 numShaders) {
				unsafe
				{
					fixed (Int32* shaders_ptr = &shaders)
					fixed (Int32* numShaders_ptr = &numShaders) {
						Delegates.glExtGetShadersQCOM((UInt32*) shaders_ptr, (Int32) maxShaders, (Int32*) numShaders_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetShadersQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void ExtGetShaders(Int32* shaders, Int32 maxShaders, Int32* numShaders) {
				Delegates.glExtGetShadersQCOM((UInt32*) shaders, (Int32) maxShaders, (Int32*) numShaders);
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetShadersQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void ExtGetShaders(UInt32[] shaders, Int32 maxShaders, Int32[] numShaders) {
				unsafe
				{
					fixed (UInt32* shaders_ptr = shaders)
					fixed (Int32* numShaders_ptr = numShaders) {
						Delegates.glExtGetShadersQCOM((UInt32*) shaders_ptr, (Int32) maxShaders, (Int32*) numShaders_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetShadersQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void ExtGetShaders(ref UInt32 shaders, Int32 maxShaders, ref Int32 numShaders) {
				unsafe
				{
					fixed (UInt32* shaders_ptr = &shaders)
					fixed (Int32* numShaders_ptr = &numShaders) {
						Delegates.glExtGetShadersQCOM((UInt32*) shaders_ptr, (Int32) maxShaders, (Int32*) numShaders_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetShadersQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void ExtGetShaders(UInt32* shaders, Int32 maxShaders, Int32* numShaders) {
				Delegates.glExtGetShadersQCOM((UInt32*) shaders, (Int32) maxShaders, (Int32*) numShaders);
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetTexLevelParameterivQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void ExtGetTexLevelParameter(Int32 texture, System.Graphics.ES20.All face, Int32 level, System.Graphics.ES20.All pname, Int32[] @params) {
				unsafe
				{
					fixed (Int32* @params_ptr = @params) {
						Delegates.glExtGetTexLevelParameterivQCOM((UInt32) texture, (System.Graphics.ES20.All) face, (Int32) level, (System.Graphics.ES20.All) pname, (Int32*) @params_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetTexLevelParameterivQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void ExtGetTexLevelParameter(Int32 texture, System.Graphics.ES20.All face, Int32 level, System.Graphics.ES20.All pname, ref Int32 @params) {
				unsafe
				{
					fixed (Int32* @params_ptr = &@params) {
						Delegates.glExtGetTexLevelParameterivQCOM((UInt32) texture, (System.Graphics.ES20.All) face, (Int32) level, (System.Graphics.ES20.All) pname, (Int32*) @params_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetTexLevelParameterivQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void ExtGetTexLevelParameter(Int32 texture, System.Graphics.ES20.All face, Int32 level, System.Graphics.ES20.All pname, Int32* @params) {
				Delegates.glExtGetTexLevelParameterivQCOM((UInt32) texture, (System.Graphics.ES20.All) face, (Int32) level, (System.Graphics.ES20.All) pname, (Int32*) @params);
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetTexLevelParameterivQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void ExtGetTexLevelParameter(UInt32 texture, System.Graphics.ES20.All face, Int32 level, System.Graphics.ES20.All pname, Int32[] @params) {
				unsafe
				{
					fixed (Int32* @params_ptr = @params) {
						Delegates.glExtGetTexLevelParameterivQCOM((UInt32) texture, (System.Graphics.ES20.All) face, (Int32) level, (System.Graphics.ES20.All) pname, (Int32*) @params_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetTexLevelParameterivQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void ExtGetTexLevelParameter(UInt32 texture, System.Graphics.ES20.All face, Int32 level, System.Graphics.ES20.All pname, ref Int32 @params) {
				unsafe
				{
					fixed (Int32* @params_ptr = &@params) {
						Delegates.glExtGetTexLevelParameterivQCOM((UInt32) texture, (System.Graphics.ES20.All) face, (Int32) level, (System.Graphics.ES20.All) pname, (Int32*) @params_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetTexLevelParameterivQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void ExtGetTexLevelParameter(UInt32 texture, System.Graphics.ES20.All face, Int32 level, System.Graphics.ES20.All pname, Int32* @params) {
				Delegates.glExtGetTexLevelParameterivQCOM((UInt32) texture, (System.Graphics.ES20.All) face, (Int32) level, (System.Graphics.ES20.All) pname, (Int32*) @params);
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetTexSubImageQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void ExtGetTexSubImage(System.Graphics.ES20.All target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 width, Int32 height, Int32 depth, System.Graphics.ES20.All format, System.Graphics.ES20.All type, IntPtr texels) {
				Delegates.glExtGetTexSubImageQCOM((System.Graphics.ES20.All) target, (Int32) level, (Int32) xoffset, (Int32) yoffset, (Int32) zoffset, (Int32) width, (Int32) height, (Int32) depth, (System.Graphics.ES20.All) format, (System.Graphics.ES20.All) type, (IntPtr) texels);
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetTexSubImageQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void ExtGetTexSubImage<T10>(System.Graphics.ES20.All target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 width, Int32 height, Int32 depth, System.Graphics.ES20.All format, System.Graphics.ES20.All type, [InAttribute, OutAttribute] T10[] texels)
							where T10 : struct {
				GCHandle texels_ptr = GCHandle.Alloc(texels, GCHandleType.Pinned);
				try {
					Delegates.glExtGetTexSubImageQCOM((System.Graphics.ES20.All) target, (Int32) level, (Int32) xoffset, (Int32) yoffset, (Int32) zoffset, (Int32) width, (Int32) height, (Int32) depth, (System.Graphics.ES20.All) format, (System.Graphics.ES20.All) type, (IntPtr) texels_ptr.AddrOfPinnedObject());
				} finally {
					texels_ptr.Free();
				}
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetTexSubImageQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void ExtGetTexSubImage<T10>(System.Graphics.ES20.All target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 width, Int32 height, Int32 depth, System.Graphics.ES20.All format, System.Graphics.ES20.All type, [InAttribute, OutAttribute] T10[,] texels)
							where T10 : struct {
				GCHandle texels_ptr = GCHandle.Alloc(texels, GCHandleType.Pinned);
				try {
					Delegates.glExtGetTexSubImageQCOM((System.Graphics.ES20.All) target, (Int32) level, (Int32) xoffset, (Int32) yoffset, (Int32) zoffset, (Int32) width, (Int32) height, (Int32) depth, (System.Graphics.ES20.All) format, (System.Graphics.ES20.All) type, (IntPtr) texels_ptr.AddrOfPinnedObject());
				} finally {
					texels_ptr.Free();
				}
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetTexSubImageQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void ExtGetTexSubImage<T10>(System.Graphics.ES20.All target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 width, Int32 height, Int32 depth, System.Graphics.ES20.All format, System.Graphics.ES20.All type, [InAttribute, OutAttribute] T10[,,] texels)
							where T10 : struct {
				GCHandle texels_ptr = GCHandle.Alloc(texels, GCHandleType.Pinned);
				try {
					Delegates.glExtGetTexSubImageQCOM((System.Graphics.ES20.All) target, (Int32) level, (Int32) xoffset, (Int32) yoffset, (Int32) zoffset, (Int32) width, (Int32) height, (Int32) depth, (System.Graphics.ES20.All) format, (System.Graphics.ES20.All) type, (IntPtr) texels_ptr.AddrOfPinnedObject());
				} finally {
					texels_ptr.Free();
				}
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetTexSubImageQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void ExtGetTexSubImage<T10>(System.Graphics.ES20.All target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 width, Int32 height, Int32 depth, System.Graphics.ES20.All format, System.Graphics.ES20.All type, [InAttribute, OutAttribute] ref T10 texels)
							where T10 : struct {
				GCHandle texels_ptr = GCHandle.Alloc(texels, GCHandleType.Pinned);
				try {
					Delegates.glExtGetTexSubImageQCOM((System.Graphics.ES20.All) target, (Int32) level, (Int32) xoffset, (Int32) yoffset, (Int32) zoffset, (Int32) width, (Int32) height, (Int32) depth, (System.Graphics.ES20.All) format, (System.Graphics.ES20.All) type, (IntPtr) texels_ptr.AddrOfPinnedObject());
					texels = (T10) texels_ptr.Target;
				} finally {
					texels_ptr.Free();
				}
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetTexturesQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void ExtGetTextures(Int32[] textures, Int32 maxTextures, Int32[] numTextures) {
				unsafe
				{
					fixed (Int32* textures_ptr = textures)
					fixed (Int32* numTextures_ptr = numTextures) {
						Delegates.glExtGetTexturesQCOM((UInt32*) textures_ptr, (Int32) maxTextures, (Int32*) numTextures_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetTexturesQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void ExtGetTextures(ref Int32 textures, Int32 maxTextures, ref Int32 numTextures) {
				unsafe
				{
					fixed (Int32* textures_ptr = &textures)
					fixed (Int32* numTextures_ptr = &numTextures) {
						Delegates.glExtGetTexturesQCOM((UInt32*) textures_ptr, (Int32) maxTextures, (Int32*) numTextures_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetTexturesQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void ExtGetTextures(Int32* textures, Int32 maxTextures, Int32* numTextures) {
				Delegates.glExtGetTexturesQCOM((UInt32*) textures, (Int32) maxTextures, (Int32*) numTextures);
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetTexturesQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void ExtGetTextures(UInt32[] textures, Int32 maxTextures, Int32[] numTextures) {
				unsafe
				{
					fixed (UInt32* textures_ptr = textures)
					fixed (Int32* numTextures_ptr = numTextures) {
						Delegates.glExtGetTexturesQCOM((UInt32*) textures_ptr, (Int32) maxTextures, (Int32*) numTextures_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetTexturesQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void ExtGetTextures(ref UInt32 textures, Int32 maxTextures, ref Int32 numTextures) {
				unsafe
				{
					fixed (UInt32* textures_ptr = &textures)
					fixed (Int32* numTextures_ptr = &numTextures) {
						Delegates.glExtGetTexturesQCOM((UInt32*) textures_ptr, (Int32) maxTextures, (Int32*) numTextures_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtGetTexturesQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void ExtGetTextures(UInt32* textures, Int32 maxTextures, Int32* numTextures) {
				Delegates.glExtGetTexturesQCOM((UInt32*) textures, (Int32) maxTextures, (Int32*) numTextures);
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtIsProgramBinaryQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						bool ExtIsProgramBinary(Int32 program) {
				return Delegates.glExtIsProgramBinaryQCOM((UInt32) program);
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtIsProgramBinaryQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			bool ExtIsProgramBinary(UInt32 program) {
				return Delegates.glExtIsProgramBinaryQCOM((UInt32) program);
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glExtTexObjectStateOverrideiQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void ExtTexObjectStateOverride(System.Graphics.ES20.All target, System.Graphics.ES20.All pname, Int32 param) {
				Delegates.glExtTexObjectStateOverrideiQCOM((System.Graphics.ES20.All) target, (System.Graphics.ES20.All) pname, (Int32) param);
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetDriverControlsQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void GetDriverControl([OutAttribute] Int32[] num, Int32 size, [OutAttribute] Int32[] driverControls) {
				unsafe
				{
					fixed (Int32* num_ptr = num)
					fixed (Int32* driverControls_ptr = driverControls) {
						Delegates.glGetDriverControlsQCOM((Int32*) num_ptr, (Int32) size, (UInt32*) driverControls_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetDriverControlsQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void GetDriverControl([OutAttribute] Int32[] num, Int32 size, [OutAttribute] UInt32[] driverControls) {
				unsafe
				{
					fixed (Int32* num_ptr = num)
					fixed (UInt32* driverControls_ptr = driverControls) {
						Delegates.glGetDriverControlsQCOM((Int32*) num_ptr, (Int32) size, (UInt32*) driverControls_ptr);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetDriverControlsQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void GetDriverControl([OutAttribute] out Int32 num, Int32 size, [OutAttribute] out Int32 driverControls) {
				unsafe
				{
					fixed (Int32* num_ptr = &num)
					fixed (Int32* driverControls_ptr = &driverControls) {
						Delegates.glGetDriverControlsQCOM((Int32*) num_ptr, (Int32) size, (UInt32*) driverControls_ptr);
						num = *num_ptr;
						driverControls = *driverControls_ptr;
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetDriverControlsQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void GetDriverControl([OutAttribute] out Int32 num, Int32 size, [OutAttribute] out UInt32 driverControls) {
				unsafe
				{
					fixed (Int32* num_ptr = &num)
					fixed (UInt32* driverControls_ptr = &driverControls) {
						Delegates.glGetDriverControlsQCOM((Int32*) num_ptr, (Int32) size, (UInt32*) driverControls_ptr);
						num = *num_ptr;
						driverControls = *driverControls_ptr;
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetDriverControlsQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void GetDriverControl([OutAttribute] Int32* num, Int32 size, [OutAttribute] Int32* driverControls) {
				Delegates.glGetDriverControlsQCOM((Int32*) num, (Int32) size, (UInt32*) driverControls);
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetDriverControlsQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void GetDriverControl([OutAttribute] Int32* num, Int32 size, [OutAttribute] UInt32* driverControls) {
				Delegates.glGetDriverControlsQCOM((Int32*) num, (Int32) size, (UInt32*) driverControls);
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetDriverControlStringQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void GetDriverControlString(Int32 driverControl, Int32 bufSize, [OutAttribute] Int32[] length, [OutAttribute] StringBuilder driverControlString) {
				unsafe
				{
					fixed (Int32* length_ptr = length) {
						Delegates.glGetDriverControlStringQCOM((UInt32) driverControl, (Int32) bufSize, (Int32*) length_ptr, (StringBuilder) driverControlString);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetDriverControlStringQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void GetDriverControlString(Int32 driverControl, Int32 bufSize, [OutAttribute] out Int32 length, [OutAttribute] StringBuilder driverControlString) {
				unsafe
				{
					fixed (Int32* length_ptr = &length) {
						Delegates.glGetDriverControlStringQCOM((UInt32) driverControl, (Int32) bufSize, (Int32*) length_ptr, (StringBuilder) driverControlString);
						length = *length_ptr;
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetDriverControlStringQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void GetDriverControlString(Int32 driverControl, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] StringBuilder driverControlString) {
				Delegates.glGetDriverControlStringQCOM((UInt32) driverControl, (Int32) bufSize, (Int32*) length, (StringBuilder) driverControlString);
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetDriverControlStringQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void GetDriverControlString(UInt32 driverControl, Int32 bufSize, [OutAttribute] Int32[] length, [OutAttribute] StringBuilder driverControlString) {
				unsafe
				{
					fixed (Int32* length_ptr = length) {
						Delegates.glGetDriverControlStringQCOM((UInt32) driverControl, (Int32) bufSize, (Int32*) length_ptr, (StringBuilder) driverControlString);
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetDriverControlStringQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void GetDriverControlString(UInt32 driverControl, Int32 bufSize, [OutAttribute] out Int32 length, [OutAttribute] StringBuilder driverControlString) {
				unsafe
				{
					fixed (Int32* length_ptr = &length) {
						Delegates.glGetDriverControlStringQCOM((UInt32) driverControl, (Int32) bufSize, (Int32*) length_ptr, (StringBuilder) driverControlString);
						length = *length_ptr;
					}
				}
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glGetDriverControlStringQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			unsafe void GetDriverControlString(UInt32 driverControl, Int32 bufSize, [OutAttribute] Int32* length, [OutAttribute] StringBuilder driverControlString) {
				Delegates.glGetDriverControlStringQCOM((UInt32) driverControl, (Int32) bufSize, (Int32*) length, (StringBuilder) driverControlString);
			}

			/// <summary>[requires: 2.0]</summary>
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glStartTilingQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
						void StartTiling(Int32 x, Int32 y, Int32 width, Int32 height, Int32 preserveMask) {
				Delegates.glStartTilingQCOM((UInt32) x, (UInt32) y, (UInt32) width, (UInt32) height, (UInt32) preserveMask);
			}

			/// <summary>[requires: 2.0]</summary>
			[System.CLSCompliant(false)]
			//[AutoGenerated(Category = "2.0", Version = "2.0", EntryPoint = "glStartTilingQCOM")]
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			public static
			void StartTiling(UInt32 x, UInt32 y, UInt32 width, UInt32 height, UInt32 preserveMask) {
				Delegates.glStartTilingQCOM((UInt32) x, (UInt32) y, (UInt32) width, (UInt32) height, (UInt32) preserveMask);
			}

		}


	}
}

#pragma warning restore 1572
#pragma warning restore 1573
#pragma warning restore 1591