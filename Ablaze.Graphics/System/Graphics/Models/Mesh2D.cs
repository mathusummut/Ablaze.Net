using System.Drawing;
using System.Graphics.OGL;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace System.Graphics.Models {
	/// <summary>
	/// Offers simple methods to create and manage 2D meshes
	/// </summary>
	public static class Mesh2D {
		/// <summary>
		/// Creates a 2D orthographic mesh from the specified quadrilateral
		/// </summary>
		/// <param name="texture">The texture of the quad mesh from (can be null)</param>
		/// <param name="location">The location on initialize the mesh at</param>
		/// <param name="size">The size of the mesh</param>
		/// <param name="flipVertically">Whether to render the texture upside down</param>
		/// <param name="render2D">True if the mesh is meant to be rendered on an orthographic projection</param>
		public static MeshComponent MeshFromQuad(TextureCollection texture, Vector3 location, Vector2 size, bool flipVertically = false, bool render2D = true) {
			if (flipVertically) {
				return new MeshComponent(texture, MeshExtensions.TriangulateQuads(new Vertex[] {
					new Vertex(location, Vector2.UnitY),
					new Vertex(new Vector3(location.X + size.X, location.Y, location.Z), Vector2.One),
					new Vertex(new Vector3(location.X + size.X, location.Y + size.Y, location.Z), Vector2.UnitX),
					new Vertex(new Vector3(location.X, location.Y + size.Y, location.Z), Vector2.Zero)
				}), false, BufferUsageHint.StaticDraw, false) {
					LowOpacity = render2D
				};
			} else {
				return new MeshComponent(texture, MeshExtensions.TriangulateQuads(new Vertex[] {
					new Vertex(location, Vector2.Zero),
					new Vertex(new Vector3(location.X + size.X, location.Y, location.Z), Vector2.UnitX),
					new Vertex(new Vector3(location.X + size.X, location.Y + size.Y, location.Z), Vector2.One),
					new Vertex(new Vector3(location.X, location.Y + size.Y, location.Z), Vector2.UnitY)
				}), false, BufferUsageHint.StaticDraw, false) {
					LowOpacity = render2D
				};
			}
		}

		/// <summary>
		/// Updates the texture coordinates of the specified mesh so that the texture is repeated instead of stretched
		/// </summary>
		/// <param name="mesh2D">The mesh whose texture coordinates to update</param>
		/// <param name="texture">The texture to tile</param>
		/// <param name="quadSize">The size of the target area that will contain the tiles</param>
		public static void UpdateTextureCoordinatesForRepeat(MeshComponent mesh2D, Texture2D texture, Vector2 quadSize) {
			UpdateTextureCoordinatesForRepeat(mesh2D, texture.BitmapSize.ToVector2(), quadSize);
		}

		/// <summary>
		/// Updates the texture coordinates of the specified mesh so that the texture is repeated using the specified tile size instead of stretched
		/// </summary>
		/// <param name="mesh2D">The mesh whose texture coordinates to update</param>
		/// <param name="tileSize">The size of the texture quad tiles</param>
		/// <param name="quadSize">The size of the target area that will contain the tiles</param>
		public static void UpdateTextureCoordinatesForRepeat(MeshComponent mesh2D, Vector2 tileSize, Vector2 quadSize) {
			mesh2D.BeginUpdateMesh();
			Vertex[] bufferData = mesh2D.BufferData;
			tileSize.X = quadSize.X / tileSize.X;
			tileSize.Y = quadSize.Y / tileSize.Y;
			Vector2 texPos;
			for (int i = 0; i < bufferData.Length; ++i) {
				texPos = bufferData[i].TexPos;
				if (texPos.X != 0)
					texPos.X = tileSize.X;
				if (texPos.Y != 0)
					texPos.Y = tileSize.Y;
				bufferData[i].TexPos = texPos;
			}
			mesh2D.EndUpdateMesh();
		}

		/// <summary>
		/// Resets the mesh texture coordinates if they have been modified by the UpdateTextureCoordinatesForRepeat() function
		/// </summary>
		/// <param name="mesh2D">The rectangular mesh to reset</param>
		public static void ResetMeshTexture(MeshComponent mesh2D) {
			UpdateTextureCoordinatesForRepeat(mesh2D, Vector2.One, Vector2.One);
		}

		/// <summary>
		/// Creates a blank 2D quadrilateral mesh
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static MeshComponent CreateShared2DMeshRect() {
			return MeshFromQuad(null, Vector3.Zero, Vector2.One, false, true);
		}

		/// <summary>
		/// Draws the specified quad on screen on a 2D orthographic projection. Any additional customization can be done using the sharedRectMesh object.
		/// </summary>
		/// <param name="texture">The texture of the quad (can be null)</param>
		/// <param name="origin">The origin coordinate in the current 2D orthographic projection (usually {0, 0, 0}), relative to which the location is specified</param>
		/// <param name="location">The location to render the quad at. It is the target location of the top-left coordinate of the texture to be drawn</param>
		/// <param name="size">The size to render the quad at</param>
		/// <param name="sharedRectMesh">The 2D quad mesh to use for rendering, that must be created with CreateShared2DMeshRect(). Can be null</param>
		public static void DrawQuad2D(ITexture texture, Vector3 origin, Vector3 location, Vector2 size, MeshComponent sharedRectMesh = null) {
			DrawQuad2D(texture, origin, location, size, Vector3.Zero, ColorF.White, sharedRectMesh);
		}

		/// <summary>
		/// Draws the specified quad on screen on a 2D orthographic projection. Any additional customization can be done using the sharedRectMesh object.
		/// </summary>
		/// <param name="texture">The texture of the quad (can be null)</param>
		/// <param name="origin">The origin coordinate in the current 2D orthographic projection (usually {0, 0, 0}), relative to which the location is specified</param>
		/// <param name="location">The location to render the quad at. It is the target location of the top-left coordinate of the texture to be drawn</param>
		/// <param name="size">The size to render the quad at</param>
		/// <param name="rotation">The rotation of the quad</param>
		/// <param name="opacity">The opacity multiplier for the texture (1 leaves the texture intact, 0 means transparent, can also be any value inbetween)</param>
		/// <param name="sharedRectMesh">The 2D quad mesh to use for rendering, that must be created with CreateShared2DMeshRect(). Can be null</param>
		public static void DrawQuad2D(ITexture texture, Vector3 origin, Vector3 location, Vector2 size, Vector3 rotation, float opacity = 1f, MeshComponent sharedRectMesh = null) {
			DrawQuad2D(texture, origin, location, size, rotation, new ColorF(opacity, 1f, 1f, 1f), sharedRectMesh);
		}

		/// <summary>
		/// Draws the specified quad on screen on a 2D orthographic projection. Any additional customization can be done using the sharedRectMesh object.
		/// </summary>
		/// <param name="texture">The texture of the quad (can be null)</param>
		/// <param name="origin">The origin coordinate in the current 2D orthographic projection (usually {0, 0, 0}), relative to which the location is specified</param>
		/// <param name="location">The location to render the texture at. It is the target location of the top-left coordinate of the texture to be drawn</param>
		/// <param name="size">The size to render the texture at</param>
		/// <param name="rotation">The rotation of the texture</param>
		/// <param name="multiplier">The colour and opacity multiplier for the quad</param>
		/// <param name="sharedRectMesh">The 2D quad mesh to use for rendering, that must be created with CreateShared2DMeshRect(). Can be null</param>
		public static void DrawQuad2D(ITexture texture, Vector3 origin, Vector3 location, Vector2 size, Vector3 rotation, ColorF multiplier, MeshComponent sharedRectMesh = null) {
			if (size.X <= 0 || size.Y <= 0 || multiplier.A == 0f)
				return;
			bool dispose;
			if (sharedRectMesh == null) {
				sharedRectMesh = CreateShared2DMeshRect();
				dispose = true;
			} else
				dispose = false;
			sharedRectMesh.Location = new Vector3(origin.X + (location.X - origin.X) / size.X, origin.Y + (location.Y - origin.Y) / size.Y, 0f);
			sharedRectMesh.Scale = new Vector3(size, 0f);
			sharedRectMesh.Rotation = rotation;
			sharedRectMesh.MaterialHue = multiplier;
			if (texture == null)
				texture = Texture2D.Empty;
			sharedRectMesh.Render(texture);
			if (dispose)
				sharedRectMesh.Dispose();
		}

		/// <summary>
		/// Sets the camera projection to orthographic at the specified size
		/// </summary>
		/// <param name="viewport">The view port size (to map to relative pixel coordinates)</param>
		/// <param name="maxZ">The maximum Z-depth</param>
		/// <param name="disableDepth">Whether to render consequent polygons as 2D</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Setup2D(Size viewport, float maxZ = 1f, bool disableDepth = true) {
			Setup2D(new Vector2(viewport.Width, viewport.Height), maxZ, disableDepth);
		}

		/// <summary>
		/// Sets the camera projection to orthographic at the specified size
		/// </summary>
		/// <param name="viewport">The view port size (to map to relative pixel coordinates)</param>
		/// <param name="maxZ">The maximum Z-depth</param>
		/// <param name="disableDepth">Whether to render consequent polygons as 2D</param>
		public static void Setup2D(Vector2 viewport, float maxZ, bool disableDepth = true) {
			Matrix4 matrix = Matrix4.CreateOrthographicOffCenter(0f, viewport.X, viewport.Y, 0f, -maxZ, maxZ);
			Setup2D(ref matrix, disableDepth);
		}

		/// <summary>
		/// Sets the camera projection to orthographic using the specified matrix
		/// </summary>
		/// <param name="ortho">The orthographic transform matrix (usually using Matrix4.CreateOrthographicOffCenter())</param>
		/// <param name="disableDepth">Whether to render consequent polygons as 2D</param>
		public static void Setup2D(ref Matrix4 ortho, bool disableDepth = true) {
			if (disableDepth)
				GL.Disable(EnableCap.DepthTest);
			GL.Enable(EnableCap.CullFace);
			GlobalShader globalShader = (GlobalShader) Shader.CurrentShader;
			globalShader.SetUniformValue(GlobalShaderParams.ProjectionMatrix.ToString(), ref ortho, ShaderSetMode.SetImmediately);
			Matrix4 modelView = Matrix4.Identity;
			globalShader.SetUniformValue(GlobalShaderParams.ModelViewMatrix.ToString(), ref modelView, ShaderSetMode.SetImmediately);
			globalShader.SetUniformValue(GlobalShaderParams.LightingEnabled.ToString(), 0f, ShaderSetMode.SetImmediately);
		}

		/// <summary>
		/// Converts the specified 3D coordinate to 2D using the specified projection properties
		/// </summary>
		/// <param name="pos">The coordinate to project</param>
		/// <param name="viewMatrix">The matrix that transforms the coordinate (world matrix)</param>
		/// <param name="projectionMatrix">The matrix that represents the camera</param>
		/// <param name="viewWidth">The width of the viewport</param>
		/// <param name="viewHeight">The height of the viewport</param>
		public static Vector2 ProjectTo2D(this Vector3 pos, ref Matrix4 viewMatrix, ref Matrix4 projectionMatrix, int viewWidth, int viewHeight) {
			pos = pos.Transform(ref viewMatrix);
			pos = pos.Transform(ref projectionMatrix);
			Vector2 resultant = new Vector2(pos.X / pos.Z, pos.Y / pos.Z);
			return new Vector2((resultant.X + 1f) * viewWidth * 0.5f, (resultant.Y + 1f) * viewHeight * 0.5f);
		}
	}
}