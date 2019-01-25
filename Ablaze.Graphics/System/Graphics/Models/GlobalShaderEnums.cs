namespace System.Graphics.Models {
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
