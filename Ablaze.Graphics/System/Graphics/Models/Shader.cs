using System.Collections.Generic;
using System.Graphics.OGL;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Graphics.Models {
	/// <summary>
	/// Represents a shader compilation state
	/// </summary>
	public enum ShaderState {
		/// <summary>
		/// The shader compiled and linked successfully, and was bound
		/// </summary>
		Bound,
		/// <summary>
		/// The shader source code did not compile successfully
		/// </summary>
		CompilationFailed,
		/// <summary>
		/// The shader components failed to link
		/// </summary>
		LinkingFailed
	}

	/// <summary>
	/// Contains a shader source
	/// </summary>
	public struct ShaderSource {
		/// <summary>
		/// The type of shader component the source code represents
		/// </summary>
		public ShaderType Type;
		/// <summary>
		/// The shader component source code
		/// </summary>
		public string SourceCode;

		/// <summary>
		/// Initializes a new shader source.
		/// </summary>
		/// <param name="type">The type of shader component the source code represents</param>
		/// <param name="sourceCode">The shader component source code. DO NOT INCLUDE VERSION QUALIFIER IN SOURCE</param>
		public ShaderSource(ShaderType type, string sourceCode) {
			Type = type;
			SourceCode = sourceCode;
		}
	}

	/// <summary>
	/// Represents the mode used to set the value to the shader variable
	/// </summary>
	public enum ShaderSetMode {
		/// <summary>
		/// The value is set immediately. This assumes that the shader is currently bound
		/// </summary>
		SetImmediately = 0,
		/// <summary>
		/// Sets the value immediately if the shader is currently bound on a graphics context that is available on this thread,
		/// otherwise the modification is queued to be performed on next bind. Probably slow
		/// </summary>
		SetIfContextAvailable,
		/// <summary>
		/// Queues the modification to be performed on next bind
		/// </summary>
		SetOnNextBind
	}

	/// <summary>
	/// Represents an OpenGL shader composed of a vertex shader and/or fragment shader
	/// </summary>
	public class Shader : IDisposable {
		/// <summary>
		/// An empty shader program
		/// </summary>
		public static readonly Shader Empty = new Shader() {
			Tag = "Empty"
		};
		/// <summary>
		/// If true, shader compilation logs are printed to console
		/// </summary>
		public static bool DebugMode;
		private static Dictionary<Type, Action<int, object>> uniformSetters = new Dictionary<Type, Action<int, object>>(),
			attributeSetters = new Dictionary<Type, Action<int, object>>();
		private static ThreadLocal<Shader> boundShader = new ThreadLocal<Shader>();
		private Dictionary<string, Parameters> uniforms = new Dictionary<string, Parameters>(StringComparer.Ordinal),
			attributes = new Dictionary<string, Parameters>(StringComparer.Ordinal);
		private object uniformsSyncRoot = new object(), attributesSyncRoot = new object();
		private HashSet<string> uniformAssignments = new HashSet<string>(), attributeAssignments = new HashSet<string>();
		/// <summary>
		/// The source codes of the shaders. Upon first binding, they are set to null
		/// </summary>
		protected ShaderSource[] ShaderSources;
		private bool boundAtLeastOnce;
		private int id;
		/// <summary>
		/// The name of the shader
		/// </summary>
		public string Name;
		/// <summary>
		/// Available for your personal use
		/// </summary>
		public object Tag;

		/// <summary>
		/// Gets the currently bound shader
		/// </summary>
		public static Shader CurrentShader {
			get {
				if (boundShader.IsValueCreated) {
					Shader value = boundShader.Value;
					return value == null ? Empty : value;
				} else
					return Empty;
			}
		}

		/// <summary>
		/// Gets the native OpenGL name of the shader
		/// </summary>
		public int ID {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return id;
			}
		}

		/// <summary>
		/// Gets whether the shader program is disposed
		/// </summary>
		public bool IsDisposed {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return id == 0;
			}
		}

		private bool IsBound {
			get {
				if (GraphicsContext.IsGraphicsContextAvailable) {
					int prog;
					GL.GetInteger(GetPName.CurrentProgram, out prog);
					return prog == id;
				} else
					return false;
			}
		}

		static Shader() {
			Type uniformSetter = typeof(UniformSetterAttribute), attributeSetter = typeof(AttributeSetterAttribute);
			object[] attributes;
			UniformSetterAttribute uniformType;
			AttributeSetterAttribute attributeType;
			Type delegateType = typeof(Action<int, object>);
			foreach (MethodInfo method in typeof(Shader).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)) {
				attributes = method.GetCustomAttributes(uniformSetter, false);
				if (!(attributes == null || attributes.Length == 0)) {
					uniformType = attributes[0] as UniformSetterAttribute;
					uniformSetters.Add(uniformType.TypeHandled, (Action<int, object>) Delegate.CreateDelegate(delegateType, null, method));
				}
				attributes = method.GetCustomAttributes(attributeSetter, false);
				if (!(attributes == null || attributes.Length == 0)) {
					attributeType = attributes[0] as AttributeSetterAttribute;
					attributeSetters.Add(attributeType.TypeHandled, (Action<int, object>) Delegate.CreateDelegate(delegateType, null, method));
				}
			}
		}

		/// <summary>
		/// Initializes a new shader. The shader sources are in order of compilation
		/// </summary>
		/// <param name="sources">The shader sources</param>
		public Shader(params ShaderSource[] sources) {
			if (!(sources == null || sources.Length == 0))
				ShaderSources = sources;
		}

		/// <summary>
		/// Gets the uniform variable location in the shader. If the uniform variable is not found and throwIfNotFound is false, then -1 is returned
		/// </summary>
		/// <param name="varName">The uniform variable name</param>
		/// <param name="throwIfNotFound">If true and the uniform variable is not found, then an ArgumentException is thrown</param>
		public int GetUniformIndex(string varName, bool throwIfNotFound = false) {
			if (string.IsNullOrEmpty(varName)) {
				if (throwIfNotFound)
					throw new ArgumentException("The uniform variable name cannot be null or empty.", nameof(varName));
				else
					return -1;
			}
			lock (uniformsSyncRoot) {
				Parameters param;
				bool contains = uniforms.TryGetValue(varName, out param);
				if (!contains)
					param = new Parameters();
				else if (param.Location != -1)
					return param.Location;
				int location = GL.GetUniformLocation(id, varName);
				if (location == -1) {
					if (throwIfNotFound)
						throw new ArgumentException("The uniform variable '" + varName + "' was not found in the shader.", nameof(varName));
				} else {
					param.Location = location;
					if (!contains)
						uniforms[varName] = param;
				}
				return location;
			}
		}

		/// <summary>
		/// Gets the attribute location in the shader. If the attribute is not found and throwIfNotFound is false, then -1 is returned
		/// </summary>
		/// <param name="varName">The attribute name</param>
		/// <param name="throwIfNotFound">If true and the attribute is not found, then an ArgumentException is thrown</param>
		public int GetAttributeIndex(string varName, bool throwIfNotFound = false) {
			if (string.IsNullOrEmpty(varName)) {
				if (throwIfNotFound)
					throw new ArgumentException("The attribute name cannot be null or empty.", nameof(varName));
				else
					return -1;
			}
			lock (attributesSyncRoot) {
				Parameters param;
				bool contains = attributes.TryGetValue(varName, out param);
				if (!contains)
					param = new Parameters();
				else if (param.Location != -1)
					return param.Location;
				int location = GL.GetAttribLocation(id, varName);
				if (location == -1) {
					if (throwIfNotFound)
						throw new ArgumentException("The attribute '" + varName + "' was not found in the shader.", nameof(varName));
				} else {
					param.Location = location;
					if (!contains)
						attributes[varName] = param;
				}
				return location;
			}
		}

		/// <summary>
		/// Gets the uniform variable value in the shader. Null is returned if the value is not found
		/// </summary>
		/// <param name="varName">The uniform variable name</param>
		public T? GetUniformValue<T>(string varName) where T : struct {
			if (string.IsNullOrEmpty(varName))
				throw new ArgumentException("The uniform variable name cannot be null or empty.", nameof(varName));
			lock (uniformsSyncRoot) {
				Parameters param;
				bool contains = uniforms.TryGetValue(varName, out param);
				if (!contains)
					param = new Parameters();
				else if (param.Value != null)
					return (T) param.Value;
				int location = GetUniformIndex(varName, false);
				if (location == -1)
					return null;
				else {
					unsafe
					{
						//did not use stackalloc in case the user uses the wrong struct and unbalances the stack
						int size = Marshal.SizeOf(typeof(T));
						float[] result = new float[((size - 1) / 4) + 1];
						fixed (float* output = result) {
							GL.GetUniform(id, location, output);
							T returnValue = default(T);
							TypedReference valueref = __makeref(returnValue);
							byte* valuePtr = (byte*) *((IntPtr*) &valueref);
							byte* ptr = (byte*) output;
							for (int i = 0; i < size; ++i)
								valuePtr[i] = ptr[i];
							param.Value = returnValue;
							if (!contains)
								uniforms[varName] = param;
							return returnValue;
						}
					}
				}
			}
		}

		/// <summary>
		/// Gets the attribute value in the shader. Null is returned if the value is not found
		/// </summary>
		/// <param name="varName">The attribute name</param>
		public T? GetAttributeValue<T>(string varName) where T : struct {
			if (string.IsNullOrEmpty(varName))
				throw new ArgumentException("The attribute name cannot be null or empty.", nameof(varName));
			lock (attributesSyncRoot) {
				Parameters param;
				if (attributes.TryGetValue(varName, out param) && param.Value != null)
					return (T) param.Value;
				else
					return null;
			}
		}

		/// <summary>
		/// Sets the value of a uniform variable within the shader
		/// </summary>
		/// <param name="varName">The uniform variable name</param>
		/// <param name="value">The target value</param>
		/// <param name="mode">The method to use to set the value</param>
		public virtual void SetUniformValue(string varName, int value, ShaderSetMode mode) {
			if (id == 0)
				return;
			else if (boundAtLeastOnce && mode != ShaderSetMode.SetOnNextBind && (mode == ShaderSetMode.SetImmediately || IsBound))
				GL.Uniform1(GetUniformIndex(varName, true), value);
			else {
				lock (uniformsSyncRoot) {
					Parameters param;
					bool contains = uniforms.TryGetValue(varName, out param);
					if (!contains)
						param = new Parameters();
					param.Value = value;
					if (!contains)
						uniforms[varName] = param;
					uniformAssignments.Add(varName);
				}
			}
		}

		/// <summary>
		/// Sets the value of a uniform variable within the shader
		/// </summary>
		/// <param name="varName">The uniform variable name</param>
		/// <param name="value">The target value</param>
		/// <param name="mode">The method to use to set the value</param>
		public virtual void SetUniformValue(string varName, float value, ShaderSetMode mode) {
			if (id == 0)
				return;
			else if (boundAtLeastOnce && mode != ShaderSetMode.SetOnNextBind && (mode == ShaderSetMode.SetImmediately || IsBound))
				GL.Uniform1(GetUniformIndex(varName, true), value);
			else {
				lock (uniformsSyncRoot) {
					Parameters param;
					bool contains = uniforms.TryGetValue(varName, out param);
					if (!contains)
						param = new Parameters();
					param.Value = value;
					if (!contains)
						uniforms[varName] = param;
					uniformAssignments.Add(varName);
				}
			}
		}

		/// <summary>
		/// Sets the value of a uniform variable within the shader
		/// </summary>
		/// <param name="varName">The uniform variable name</param>
		/// <param name="value">The target value</param>
		/// <param name="mode">The method to use to set the value</param>
		public virtual void SetUniformValue(string varName, double value, ShaderSetMode mode) {
			if (id == 0)
				return;
			else if (boundAtLeastOnce && mode != ShaderSetMode.SetOnNextBind && (mode == ShaderSetMode.SetImmediately || IsBound))
				GL.Uniform1(GetUniformIndex(varName, true), value);
			else {
				lock (uniformsSyncRoot) {
					Parameters param;
					bool contains = uniforms.TryGetValue(varName, out param);
					if (!contains)
						param = new Parameters();
					param.Value = value;
					if (!contains)
						uniforms[varName] = param;
					uniformAssignments.Add(varName);
				}
			}
		}

		/// <summary>
		/// Sets the value of a uniform variable within the shader
		/// </summary>
		/// <param name="varName">The uniform variable name</param>
		/// <param name="value">The target value</param>
		/// <param name="mode">The method to use to set the value</param>
		public virtual void SetUniformValue(string varName, Vector2 value, ShaderSetMode mode) {
			if (id == 0)
				return;
			else if (boundAtLeastOnce && mode != ShaderSetMode.SetOnNextBind && (mode == ShaderSetMode.SetImmediately || IsBound))
				GL.Uniform2(GetUniformIndex(varName, true), value);
			else {
				lock (uniformsSyncRoot) {
					Parameters param;
					bool contains = uniforms.TryGetValue(varName, out param);
					if (!contains)
						param = new Parameters();
					param.Value = value;
					if (!contains)
						uniforms[varName] = param;
					uniformAssignments.Add(varName);
				}
			}
		}

		/// <summary>
		/// Sets the value of a uniform variable within the shader
		/// </summary>
		/// <param name="varName">The uniform variable name</param>
		/// <param name="value">The target value</param>
		/// <param name="mode">The method to use to set the value</param>
		public virtual void SetUniformValue(string varName, Vector3 value, ShaderSetMode mode) {
			if (id == 0)
				return;
			else if (boundAtLeastOnce && mode != ShaderSetMode.SetOnNextBind && (mode == ShaderSetMode.SetImmediately || IsBound))
				GL.Uniform3(GetUniformIndex(varName, true), ref value);
			else {
				lock (uniformsSyncRoot) {
					Parameters param;
					bool contains = uniforms.TryGetValue(varName, out param);
					if (!contains)
						param = new Parameters();
					param.Value = value;
					if (!contains)
						uniforms[varName] = param;
					uniformAssignments.Add(varName);
				}
			}
		}

		/// <summary>
		/// Sets the value of a uniform variable within the shader
		/// </summary>
		/// <param name="varName">The uniform variable name</param>
		/// <param name="value">The target value</param>
		/// <param name="mode">The method to use to set the value</param>
		public virtual void SetUniformValue(string varName, Vector4 value, ShaderSetMode mode) {
			if (id == 0)
				return;
			else if (boundAtLeastOnce && mode != ShaderSetMode.SetOnNextBind && (mode == ShaderSetMode.SetImmediately || IsBound))
				GL.Uniform4(GetUniformIndex(varName, true), ref value);
			else {
				lock (uniformsSyncRoot) {
					Parameters param;
					bool contains = uniforms.TryGetValue(varName, out param);
					if (!contains)
						param = new Parameters();
					param.Value = value;
					if (!contains)
						uniforms[varName] = param;
					uniformAssignments.Add(varName);
				}
			}
		}

		/// <summary>
		/// Sets the value of a uniform variable within the shader
		/// </summary>
		/// <param name="varName">The uniform variable name</param>
		/// <param name="value">The target value</param>
		/// <param name="mode">The method to use to set the value</param>
		public virtual void SetUniformValue(string varName, ref Matrix4 value, ShaderSetMode mode) {
			if (id == 0)
				return;
			else if (boundAtLeastOnce && mode != ShaderSetMode.SetOnNextBind && (mode == ShaderSetMode.SetImmediately || IsBound))
				GL.UniformMatrix4(GetUniformIndex(varName, true), false, ref value);
			else {
				lock (uniformsSyncRoot) {
					Parameters param;
					bool contains = uniforms.TryGetValue(varName, out param);
					if (!contains)
						param = new Parameters();
					param.Value = value;
					if (!contains)
						uniforms[varName] = param;
					uniformAssignments.Add(varName);
				}
			}
		}

		/// <summary>
		/// Sets the value of an attribute variable within the shader
		/// </summary>
		/// <param name="varName">The attribute variable name</param>
		/// <param name="value">The target value</param>
		/// <param name="mode">The method to use to set the value</param>
		public void SetAttributeValue(string varName, int value, ShaderSetMode mode) {
			if (id == 0)
				return;
			else if (boundAtLeastOnce && mode != ShaderSetMode.SetOnNextBind && (mode == ShaderSetMode.SetImmediately || IsBound))
				GL.VertexAttrib1(GetAttributeIndex(varName, true), value);
			else {
				lock (attributesSyncRoot) {
					Parameters param;
					bool contains = attributes.TryGetValue(varName, out param);
					if (!contains)
						param = new Parameters();
					param.Value = value;
					if (!contains)
						attributes[varName] = param;
					attributeAssignments.Add(varName);
				}
			}
		}

		/// <summary>
		/// Sets the value of an attribute variable within the shader
		/// </summary>
		/// <param name="varName">The attribute variable name</param>
		/// <param name="value">The target value</param>
		/// <param name="mode">The method to use to set the value</param>
		public void SetAttributeValue(string varName, float value, ShaderSetMode mode) {
			if (id == 0)
				return;
			else if (boundAtLeastOnce && mode != ShaderSetMode.SetOnNextBind && (mode == ShaderSetMode.SetImmediately || IsBound))
				GL.VertexAttrib1(GetAttributeIndex(varName, true), value);
			else {
				lock (attributesSyncRoot) {
					Parameters param;
					bool contains = attributes.TryGetValue(varName, out param);
					if (!contains)
						param = new Parameters();
					param.Value = value;
					if (!contains)
						attributes[varName] = param;
					attributeAssignments.Add(varName);
				}
			}
		}

		/// <summary>
		/// Sets the value of an attribute variable within the shader
		/// </summary>
		/// <param name="varName">The attribute variable name</param>
		/// <param name="value">The target value</param>
		/// <param name="mode">The method to use to set the value</param>
		public void SetAttributeValue(string varName, double value, ShaderSetMode mode) {
			if (id == 0)
				return;
			else if (boundAtLeastOnce && mode != ShaderSetMode.SetOnNextBind && (mode == ShaderSetMode.SetImmediately || IsBound))
				GL.VertexAttrib1(GetAttributeIndex(varName, true), value);
			else {
				lock (attributesSyncRoot) {
					Parameters param;
					bool contains = attributes.TryGetValue(varName, out param);
					if (!contains)
						param = new Parameters();
					param.Value = value;
					if (!contains)
						attributes[varName] = param;
					attributeAssignments.Add(varName);
				}
			}
		}

		/// <summary>
		/// Sets the value of an attribute variable within the shader
		/// </summary>
		/// <param name="varName">The attribute variable name</param>
		/// <param name="value">The target value</param>
		/// <param name="mode">The method to use to set the value</param>
		public void SetAttributeValue(string varName, Vector2 value, ShaderSetMode mode) {
			if (id == 0)
				return;
			else if (boundAtLeastOnce && mode != ShaderSetMode.SetOnNextBind && (mode == ShaderSetMode.SetImmediately || IsBound))
				GL.VertexAttrib2(GetAttributeIndex(varName, true), value);
			else {
				lock (attributesSyncRoot) {
					Parameters param;
					bool contains = attributes.TryGetValue(varName, out param);
					if (!contains)
						param = new Parameters();
					param.Value = value;
					if (!contains)
						attributes[varName] = param;
					attributeAssignments.Add(varName);
				}
			}
		}

		/// <summary>
		/// Sets the value of an attribute variable within the shader
		/// </summary>
		/// <param name="varName">The attribute variable name</param>
		/// <param name="value">The target value</param>
		/// <param name="mode">The method to use to set the value</param>
		public void SetAttributeValue(string varName, Vector3 value, ShaderSetMode mode) {
			if (id == 0)
				return;
			else if (boundAtLeastOnce && mode != ShaderSetMode.SetOnNextBind && (mode == ShaderSetMode.SetImmediately || IsBound))
				GL.VertexAttrib3(GetAttributeIndex(varName, true), ref value);
			else {
				lock (attributesSyncRoot) {
					Parameters param;
					bool contains = attributes.TryGetValue(varName, out param);
					if (!contains)
						param = new Parameters();
					param.Value = value;
					if (!contains)
						attributes[varName] = param;
					attributeAssignments.Add(varName);
				}
			}
		}

		/// <summary>
		/// Sets the value of an attribute variable within the shader
		/// </summary>
		/// <param name="varName">The attribute variable name</param>
		/// <param name="value">The target value</param>
		/// <param name="mode">The method to use to set the value</param>
		public void SetAttributeValue(string varName, Vector4 value, ShaderSetMode mode) {
			if (id == 0)
				return;
			else if (boundAtLeastOnce && mode != ShaderSetMode.SetOnNextBind && (mode == ShaderSetMode.SetImmediately || IsBound))
				GL.VertexAttrib4(GetAttributeIndex(varName, true), ref value);
			else {
				lock (attributesSyncRoot) {
					Parameters param;
					bool contains = attributes.TryGetValue(varName, out param);
					if (!contains)
						param = new Parameters();
					param.Value = value;
					if (!contains)
						attributes[varName] = param;
					attributeAssignments.Add(varName);
				}
			}
		}

		/// <summary>
		/// Bind the shader for rendering
		/// </summary>
		public void Bind() {
			Bind(true);
		}

		/// <summary>
		/// Bind the shader for rendering, and returns whether binding was successful
		/// </summary>
		/// <param name="throwOnError">Whether to throw on compilation error</param>
		public virtual ShaderState Bind(bool throwOnError) {
			int status;
			bool needsLink = false;
			if (ShaderSources != null) {
				if (GlobalShader.VersionQualifier == null) {
					using (GlobalShader temp = new GlobalShader()) {
						temp.Bind(throwOnError);
						Unbind();
					}
				}
				ShaderSource source;
				string info;
				int shader;
				for (int i = 0; i < ShaderSources.Length; i++) {
					source = ShaderSources[i];
					if (source.Type != 0 && !string.IsNullOrEmpty(source.SourceCode)) {
						if (!(GlobalShader.VersionQualifier.Length == 0 || this is GlobalShader))
							source.SourceCode = GlobalShader.VersionQualifier + source.SourceCode;
						if (id == 0)
							id = GL.CreateProgram();
						shader = GL.CreateShader(source.Type);
						GL.ShaderSource(shader, source.SourceCode);
						try {
							GL.CompileShader(shader);
						} catch {
						}
						GL.GetShader(shader, ShaderParameter.CompileStatus, out status);
						if (status != 1 || DebugMode) {
							GL.GetShaderInfoLog(shader, out info);
							if (info == null)
								info = string.Empty;
							else
								info = info.Trim();
							if (status == 1) {
								if (info.Length != 0)
									Console.WriteLine(info);
							} else {
								GL.DeleteShader(shader);
								GL.DeleteProgram(id);
								id = 0;
								if (throwOnError)
									throw new ArgumentException("The specified " + source.Type + " failed to compile.\n" + info, new InvalidProgramException(source.SourceCode));
								else
									return ShaderState.CompilationFailed;
							}
						}
						GL.AttachShader(id, shader);
						GL.DeleteShader(shader);
						needsLink = true;
					}
				}
				ShaderSources = null;
			}
			if (needsLink) {
				GL.LinkProgram(id);
				GL.GetProgram(id, ProgramParameter.LinkStatus, out status);
				if (status != 1 || DebugMode) {
					string info;
					GL.GetProgramInfoLog(id, out info);
					if (info == null)
						info = string.Empty;
					else
						info = info.Trim();
					if (status == 1) {
						if (info.Length != 0)
							Console.WriteLine(info);
					} else {
						GL.DeleteProgram(id);
						id = 0;
						if (throwOnError)
							throw new InvalidProgramException("The specified shaders failed to link.\n" + info);
						else
							return ShaderState.LinkingFailed;
					}
				}
			}
			boundAtLeastOnce = true;
			GL.UseProgram(id);
			boundShader.Value = this;
			if (id != 0) {
				object value;
				if (uniformAssignments.Count != 0) {
					lock (uniformsSyncRoot) {
						foreach (string variable in uniformAssignments) {
							value = uniforms[variable].Value;
							uniformSetters[value.GetType()](GetUniformIndex(variable, true), value);
						}
					}
				}
				if (attributeAssignments.Count != 0) {
					lock (attributesSyncRoot) {
						foreach (string variable in attributeAssignments) {
							value = attributes[variable].Value;
							attributeSetters[value.GetType()](GetAttributeIndex(variable, true), value);
						}
					}
				}
			}
			return ShaderState.Bound;
		}

		[UniformSetter(typeof(int))]
		private static void SetUniformInt(int location, object value) {
			GL.Uniform1(location, (int) value);
		}

		[UniformSetter(typeof(float))]
		private static void SetUniformFloat(int location, object value) {
			GL.Uniform1(location, (float) value);
		}

		[UniformSetter(typeof(double))]
		private static void SetUniformDouble(int location, object value) {
			GL.Uniform1(location, (double) value);
		}

		[UniformSetter(typeof(Vector2))]
		private static void SetUniformVector2(int location, object value) {
			GL.Uniform2(location, (Vector2) value);
		}

		[UniformSetter(typeof(Vector3))]
		private static void SetUniformVector3(int location, object value) {
			GL.Uniform3(location, (Vector3) value);
		}

		[UniformSetter(typeof(Vector4))]
		private static void SetUniformVector4(int location, object value) {
			GL.Uniform4(location, (Vector4) value);
		}

		[UniformSetter(typeof(Matrix4))]
		private static void SetUniformMatrix4(int location, object value) {
			Matrix4 val = (Matrix4) value;
			GL.UniformMatrix4(location, false, ref val);
		}

		[AttributeSetter(typeof(int))]
		private static void SetAttributeInt(int location, object value) {
			GL.VertexAttrib1(location, (int) value);
		}

		[AttributeSetter(typeof(float))]
		private static void SetAttributeFloat(int location, object value) {
			GL.VertexAttrib1(location, (float) value);
		}

		[AttributeSetter(typeof(double))]
		private static void SetAttributeDouble(int location, object value) {
			GL.VertexAttrib1(location, (double) value);
		}

		[AttributeSetter(typeof(Vector2))]
		private static void SetAttributeVector2(int location, object value) {
			GL.VertexAttrib2(location, (Vector2) value);
		}

		[AttributeSetter(typeof(Vector3))]
		private static void SetAttributeVector3(int location, object value) {
			GL.VertexAttrib3(location, (Vector3) value);
		}

		[AttributeSetter(typeof(Vector4))]
		private static void SetAttributeVector4(int location, object value) {
			GL.VertexAttrib4(location, (Vector4) value);
		}

		/// <summary>
		/// Unbinds any shader
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Unbind() {
			GL.UseProgram(0);
			boundShader.Value = Empty;
		}

		/// <summary>
		/// Returns the buffer name
		/// </summary>
		/// <returns>The buffer name</returns>
		public override int GetHashCode() {
			return id;
		}

		/// <summary>
		/// Creates a System.String that describes this Shader
		/// </summary>
		/// <returns>A System.String that describes this Shader</returns>
		public override string ToString() {
			return "Shader (handle " + id + ")";
		}

		/// <summary>
		/// Compares whether this Shader is equal to the specified object
		/// </summary>
		/// <param name="obj">An object to compare to</param>
		public override bool Equals(object obj) {
			return Equals(obj as Shader);
		}

		/// <summary>
		/// Compares whether this Shader is equal to the specified object
		/// </summary>
		/// <param name="other">The object to compare to</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool Equals(Shader other) {
			return other == null ? false : (id == other.id);
		}

		/// <summary>
		/// Disposes of the shader program
		/// </summary>
		~Shader() {
			if (id != 0) {
				GraphicsContext.RaiseResourceLeakedEvent(this, LeakedWhile.Finalizing, new IntPtr(id));
				id = 0;
			}
		}

		/// <summary>
		/// Disposes of the shader program
		/// </summary>
		public void Dispose() {
			if (id == 0)
				return;
			else if (boundShader.Value == this)
				boundShader.Value = Empty;
			try {
				GL.DeleteProgram(id);
			} catch {
				GraphicsContext.RaiseResourceLeakedEvent(this, LeakedWhile.Disposing, new IntPtr(id));
			}
			id = 0;
			GC.SuppressFinalize(this);
		}

		private sealed class Parameters {
			public int Location = -1;
			public object Value;
		}

		private sealed class UniformSetterAttribute : Attribute {
			public Type TypeHandled;

			public UniformSetterAttribute(Type typeHandled) {
				TypeHandled = typeHandled;
			}
		}

		private sealed class AttributeSetterAttribute : Attribute {
			public Type TypeHandled;

			public AttributeSetterAttribute(Type typeHandled) {
				TypeHandled = typeHandled;
			}
		}
	}
}