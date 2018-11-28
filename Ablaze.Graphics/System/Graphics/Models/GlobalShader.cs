using System.Collections.Generic;
using System.Graphics.OGL;
using System.Numerics;

namespace System.Graphics.Models {
	/// <summary>
	/// Represents the default global OpenGL shader composed of a vertex shader and/or fragment shader.
	/// </summary>
	public class GlobalShader : Shader {
		private static List<string> GLSLVersions = new List<string>(new string[] { "110", "120", "130", "140", "150", "330", "400", "410", "420", "430", "440", "450", "460" });
		internal static string VersionQualifier;

		/// <summary>
		/// Gets the current GLSL version qualifier that is used (can be an empty string).
		/// </summary>
		public static string GLSLVersionQualifier {
			get {
				string versionString = VersionQualifier;
				return versionString == null ? string.Empty : versionString;
			}
		}

		/// <summary>
		/// The global vertex shader source code.
		/// </summary>
		public static readonly string VertexShaderSource =
@"uniform mat4 ProjectionMatrix;
uniform mat4 ModelViewMatrix;
uniform mat4 WorldViewMatrix;
uniform float Interpolate;
#if __VERSION__ >= 130
	precision mediump float;
	layout(location=0) in vec3 Position;
	layout(location=1) in vec2 TexCoord;
	layout(location=2) in vec3 Normal;
	layout(location=3) in vec3 Position2;
	layout(location=4) in vec3 Normal2;
	out vec3 worldPos;
	out vec2 textureCoord;
	out vec3 vertNormal;
#else
	attribute vec3 Position;
	attribute vec2 TexCoord;
	attribute vec3 Normal;
	attribute vec3 Position2;
	attribute vec3 Normal2;
	varying vec3 worldPos;
	varying vec2 textureCoord;
	varying vec3 vertNormal;

	mat3 transpose(mat3 m) {
		return mat3(m[0][0], m[1][0], m[2][0],
				  m[0][1], m[1][1], m[2][1],
				  m[0][2], m[1][2], m[2][2]);
	}

	mat3 inverse(mat3 m) {
		float a00 = m[0][0], a01 = m[0][1], a02 = m[0][2],
		      a10 = m[1][0], a11 = m[1][1], a12 = m[1][2],
		      a20 = m[2][0], a21 = m[2][1], a22 = m[2][2];
		float b01 = a22 * a11 - a12 * a21;
		float b11 = -a22 * a10 + a12 * a20;
		float b21 = a21 * a10 - a11 * a20;
		float det = a00 * b01 + a01 * b11 + a02 * b21;
		return mat3(b01, (-a22 * a01 + a02 * a21), (a12 * a01 - a02 * a11),
					b11, (a22 * a00 - a02 * a20), (-a12 * a00 + a02 * a10),
					b21, (-a21 * a00 + a01 * a20), (a11 * a00 - a01 * a10)) / det;
	}

	#if __VERSION__ < 120
	mat3 m3(mat4 m) {
		mat3 result;
		result[0][0] = m[0][0]; 
		result[0][1] = m[0][1]; 
		result[0][2] = m[0][2]; 
		result[1][0] = m[1][0]; 
		result[1][1] = m[1][1]; 
		result[1][2] = m[1][2]; 
		result[2][0] = m[2][0]; 
		result[2][1] = m[2][1]; 
		result[2][2] = m[2][2]; 
		return result;
	}
	#endif
#endif

void main() {
	textureCoord = TexCoord;
	mat4 worldModelViewMatrix = ModelViewMatrix * WorldViewMatrix;
	vec4 pos4 = vec4(mix(Position, Position2, Interpolate), 1);
	vec4 worldPos4 = worldModelViewMatrix * pos4;
	worldPos = worldPos4.xyz / worldPos4.w;
#if __VERSION__ >= 120
	vertNormal = transpose(inverse(mat3(worldModelViewMatrix))) * mix(Normal, Normal2, Interpolate);
#else
	vertNormal = transpose(inverse(m3(worldModelViewMatrix))) * mix(Normal, Normal2, Interpolate);
#endif
    gl_Position = ProjectionMatrix * worldPos4;
}";
		/// <summary>
		/// The global fragment shader source code (uses Blinn-Phong lighting).
		/// </summary>
		public static readonly string FragmentShaderSource =
@"uniform mat4 ModelViewMatrix;
uniform sampler2D TextureUnit;
uniform vec4 MaterialHue;
uniform vec4 AmbientHue;
uniform vec4 ShineHue;
uniform float Shininess;
uniform vec4 LightHue;
uniform vec3 LightPosition;
uniform float PointLight;
uniform float LightingEnabled;
uniform float UseTexture;
#if __VERSION__ >= 130
	precision mediump float;
	#define TEXTURE2D texture
	in vec3 worldPos;
	in vec2 textureCoord;
	in vec3 vertNormal;
	out vec4 gl_FragColor;
#else
	#define TEXTURE2D texture2D
	varying vec3 worldPos;
	varying vec2 textureCoord;
	varying vec3 vertNormal;
#endif

void main() {
	if (LightingEnabled > 0.5) {
		vec4 accum = vec4(0.0, 0.0, 0.0, 0.0);
		vec3 normal = normalize(vertNormal);
		vec3 lightDir;
		float diffuse;
		if (PointLight > 0.5) {
			vec4 transform = ModelViewMatrix * vec4(LightPosition, 1.0);
			vec3 temp = transform.xyz / transform.w;
			lightDir = normalize(temp - worldPos);
			diffuse = dot(lightDir, normal);
		} else {
			lightDir = normalize(LightPosition - worldPos);
			diffuse = dot(lightDir, normal);
			if (diffuse < 0.0) {
				diffuse = -diffuse;
				normal = -normal;
			}
		}
		float specular = pow(dot(normalize(lightDir + normalize(-worldPos)), normal), Shininess);
		accum += diffuse * LightHue + specular * ShineHue;
		accum += AmbientHue;
		accum.xyz *= clamp(accum.w, 0.0, 1.0);
		accum.w = 1.0;
		if (UseTexture > 0.5)
			gl_FragColor = TEXTURE2D(TextureUnit, textureCoord) * MaterialHue * accum;
		else
			gl_FragColor = MaterialHue * accum;
	} else if (UseTexture > 0.5)
		gl_FragColor = TEXTURE2D(TextureUnit, textureCoord) * MaterialHue;
	else
		gl_FragColor = MaterialHue;
}";

		/// <summary>
		/// Initializes a new global shader.
		/// </summary>
		public GlobalShader() : base(new ShaderSource(ShaderType.VertexShader, VertexShaderSource), new ShaderSource(ShaderType.FragmentShader, FragmentShaderSource)) {
			Tag = nameof(GlobalShader);
			ResetShader(ShaderSetMode.SetOnNextBind);
		}

		/// <summary>
		/// Bind the shader for rendering, and returns whether binding was successful.
		/// </summary>
		/// <param name="throwOnError">Whether to throw on compilation error.</param>
		public override ShaderState Bind(bool throwOnError) {
			if (!(ShaderSources == null || ShaderSources.Length == 0)) {
				if (VersionQualifier == null) {
					VersionQualifier = string.Empty;
					int major;
					GL.GetInteger(GetPName.MajorVersion, out major);
					int minor;
					GL.GetInteger(GetPName.MajorVersion, out minor);
					string glslVersion = string.Empty;
					switch (major) {
						case 2:
							if (minor == 0)
								glslVersion = "110";
							else if (minor == 1)
								glslVersion = "120";
							break;
						case 3:
							switch (minor) {
								case 0:
									glslVersion = "130";
									break;
								case 1:
									glslVersion = "140";
									break;
								case 2:
									glslVersion = "150";
									break;
								case 3:
									glslVersion = "330";
									break;
							}
							break;
						case 4:
							glslVersion = "4" + minor + "0";
							break;
					}
					if (glslVersion.Length == 0)
						return base.Bind(throwOnError);
					else if (!GLSLVersions.Contains(glslVersion))
						GLSLVersions.Add(glslVersion);
					string[] sources = new string[ShaderSources.Length];
					int i;
					for (i = 0; i < sources.Length; i++)
						sources[i] = ShaderSources[i].SourceCode;
					for (i = GLSLVersions.Count - 1; i >= 0; i--) {
						if (glslVersion == GLSLVersions[i]) {
							int j;
							do {
								VersionQualifier = "#version " + glslVersion + "\n";
								for (j = 0; j < sources.Length; j++)
									ShaderSources[j].SourceCode = VersionQualifier + sources[j];
								switch (base.Bind(false)) {
									case ShaderState.Bound:
										return ShaderState.Bound;
									case ShaderState.LinkingFailed:
										throw new InvalidProgramException("The global shader failed to link");
									default:
										if (i == 0)
											goto Failed;
										else {
											i--;
											glslVersion = GLSLVersions[i];
										}
										break;
								}
							} while (true);
						}
					}
					Failed:
					VersionQualifier = string.Empty;
					for (i = 0; i < sources.Length; i++)
						ShaderSources[i].SourceCode = sources[i];
				} else if (VersionQualifier.Length != 0) {
					for (int i = 0; i < ShaderSources.Length; i++)
						ShaderSources[i].SourceCode = VersionQualifier + ShaderSources[i].SourceCode;
				}
			}
			return base.Bind(throwOnError);
		}

		/// <summary>
		/// Enables or disables lighting.
		/// </summary>
		/// <param name="enable">Whether to enable of disable lighting.</param>
		/// <param name="mode">The method to use to set the value.</param>
		public void ConfigureLighting(bool enable, ShaderSetMode mode) {
			SetUniformValue(GlobalShaderParams.LightingEnabled.ToString(), enable ? 1f : 0f, mode);
		}

		/// <summary>
		/// Resets the shader values to the default values.
		/// </summary>
		/// <param name="mode">The method to use to set the value.</param>
		public void ResetShader(ShaderSetMode mode) {
			Matrix4 identity = Matrix4.Identity;
			SetUniformValue(GlobalShaderParams.ProjectionMatrix.ToString(), ref identity, mode);
			SetUniformValue(GlobalShaderParams.ModelViewMatrix.ToString(), ref identity, mode);
			SetUniformValue(GlobalShaderParams.WorldViewMatrix.ToString(), ref identity, mode);
			SetUniformValue(GlobalShaderParams.MaterialHue.ToString(), Light.DefaultMaterialHue, mode);
			SetUniformValue(GlobalShaderParams.AmbientHue.ToString(), Light.DefaultAmbientHue, mode);
			SetUniformValue(GlobalShaderParams.ShineHue.ToString(), Light.DefaultShineHue, mode);
			SetUniformValue(GlobalShaderParams.Shininess.ToString(), Light.DefaultShininess, mode);
			SetUniformValue(GlobalShaderParams.LightHue.ToString(), Light.DefaultLightHue, mode);
			SetUniformValue(GlobalShaderParams.LightPosition.ToString(), Vector3.Zero, mode);
			SetUniformValue(GlobalShaderParams.LightingEnabled.ToString(), 1f, mode);
			SetUniformValue(GlobalShaderParams.UseTexture.ToString(), 1f, mode);
			SetUniformValue(GlobalShaderParams.Interpolate.ToString(), 0f, mode);
		}
	}

	/// <summary>
	/// A list of configurable parameters (uniforms) in the global shader
	/// </summary>
	public enum GlobalShaderParams {
		/// <summary>
		/// The projection matrix (perspective correction) (mat4)
		/// </summary>
		ProjectionMatrix,
		/// <summary>
		/// The model view matrix (camera matrix) (mat4)
		/// </summary>
		ModelViewMatrix,
		/// <summary>
		/// The transformation matrix of the model to be rendered (mat4)
		/// </summary>
		WorldViewMatrix,
		/// <summary>
		/// The component's hue and opacity (diffused) (vec4)
		/// </summary>
		MaterialHue,
		/// <summary>
		/// The hue of the ambient light that hits the object (vec4)
		/// </summary>
		AmbientHue,
		/// <summary>
		/// The hue of the reflective shine of the object (specular) (vec4)
		/// </summary>
		ShineHue,
		/// <summary>
		/// The shininess exponent of the material (float)
		/// </summary>
		Shininess,
		/// <summary>
		/// The color of the light emitted by the source (emitted) (vec4)
		/// </summary>
		LightHue,
		/// <summary>
		/// The position of the light source (vec3)
		/// </summary>
		LightPosition,
		/// <summary>
		/// Whether the light source is a point light source. If 0, it is an ambient light source, otherwise use 1 (float)
		/// </summary>
		PointLight,
		/// <summary>
		/// Whether lighting is enabled. 0 if disabled, 1 if enabled (float)
		/// </summary>
		LightingEnabled,
		/// <summary>
		/// If 1, the texture is used, otherwise 0 (float)
		/// </summary>
		UseTexture,
		/// <summary>
		/// How much to interpolate with the secondary position and normal buffers between 0 and 1 (0 means no interpolation, 1 means only the second buffer is used)
		/// </summary>
		Interpolate
	}

	/// <summary>
	/// A list of vertex attributes in the global shader.
	/// </summary>
	public enum GlobalShaderAttribs {
		/// <summary>
		/// The position of the vertex (vec3)
		/// </summary>
		Position,
		/// <summary>
		/// The coordinate of the corresponding pixel in the texture (0 to 1) (vec2)
		/// </summary>
		TexCoord,
		/// <summary>
		/// The normal of the vertex for lighting (vec3)
		/// </summary>
		Normal,
		/// <summary>
		/// The position of the vertex to interpolate with (if enabled) (vec3)
		/// </summary>
		Position2,
		/// <summary>
		/// The normal of the vertex for lighting to interpolate with (if enabled) (vec3)
		/// </summary>
		Normal2,
	}
}