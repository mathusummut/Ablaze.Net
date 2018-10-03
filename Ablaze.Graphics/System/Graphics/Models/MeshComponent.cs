using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Graphics.OGL;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace System.Graphics.Models {
	/// <summary>
	/// Loads and holds the resources that make up the model, and uses OpenGL for rendering render.
	/// </summary>
	public class MeshComponent : IModel, IEnumerable<Vertex>, IEnumerable {
		/// <summary>
		/// The size of a Vector2 instance.
		/// </summary>
		public static readonly int Vector2Size = Marshal.SizeOf(typeof(Vector2));
		/// <summary>
		/// The size of a Vector3 instance.
		/// </summary>
		public static readonly int Vector3Size = Marshal.SizeOf(typeof(Vector3));
		/// <summary>
		/// The size of a Vector4 instance.
		/// </summary>
		public static readonly int Vector4Size = Marshal.SizeOf(typeof(Vector4));
		/// <summary>
		/// Called when a model is about to be rendered.
		/// </summary>
		public event Action RenderBegin;
		/// <summary>
		/// Called when a model has been rendered.
		/// </summary>
		public event Action RenderEnd;
		/// <summary>
		/// Called when the location of the model has changed.
		/// </summary>
		public event Action LocationChanged;
		/// <summary>
		/// Called when the scale of the model has changed.
		/// </summary>
		public event Action ScaleChanged;
		/// <summary>
		/// Called when the rotation of the model has changed.
		/// </summary>
		public event Action RotationChanged;
		private ITexture defaultTexture = Texture2D.Empty;
		/// <summary>
		/// The vertex data of the mesh.
		/// </summary>
		private Vertex[] bufferData;
		private IntPtr BufferSize;
		private Vector3 oldLoc, currentLoc, oldRot, currentRot, oldScale = Vector3.One, currentScale = Vector3.One;
		private bool isClone, isDisposed, updateBuffer;
		private Matrix4 generatedMatrix = Matrix4.Identity;
		private MeshComponent lastNextModel;
		private VertexArrayBuffer VertexArrayBuffer;
		private DataBuffer DataBuffer;
		private IndexBuffer indexBuffer;
		private ITexture texture;
		private IModel parent;
		private int triangles, vertices;
		/// <summary>
		/// The transformation matrix that is used if 'UseCustomTransformation' is set to true. The same as TransformationMatrix.
		/// </summary>
		public Matrix4 Transformation = Matrix4.Identity;

		/// <summary>
		/// Gets or sets whether to keep a copy of the vertices in memory even after loading the vertices into GPU memory (allows faster and multithreaded vertex retrieval)
		/// </summary>
		public bool KeepCopyInMemory {
			get {
				return indexBuffer.KeepCopyInMemory;
			}
			set {
				indexBuffer.KeepCopyInMemory = value;
			}
		}

		/// <summary>
		/// The buffer that contains the index order of the vertices of the mesh.
		/// </summary>
		public IndexBuffer IndexBuffer {
			get {
				return indexBuffer;
			}
		}

		/// <summary>
		/// Treats the component as if it is see-through. Set to true if the texture for it is not opaque.
		/// </summary>
		public bool LowOpacity {
			get;
			set;
		}

		/// <summary>
		/// If true, the 'Transformation' matrix is used instead of 'Location', 'Scale' and 'Rotation'.
		/// </summary>
		public bool UseCustomTransformation {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the component's hue and opacity.
		/// </summary>
		public ColorF MaterialHue {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the hue of the ambient light that hits the object.
		/// </summary>
		public ColorF AmbientHue {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the hue of the reflective shine of the object.
		/// </summary>
		public ColorF ShineHue {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the shininess exponent of the material.
		/// </summary>
		public float Shininess {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the transformation matrix that is used if 'UseCustomTransformation' is set to true.
		/// </summary>
		public Matrix4 TransformationMatrix {
			get {
				return Transformation;
			}
			set {
				Transformation = value;
			}
		}

		/// <summary>
		/// Configures the way textures are wrapped to polygons.
		/// </summary>
		public TextureWrapMode WrapMode {
			get;
			set;
		}

		/// <summary>
		/// Gets the number of components in the model.
		/// </summary>
		public int Count {
			get {
				return 0;
			}
		}

		/// <summary>
		/// Set to true if you want the vertex buffer to be flushed on next render.
		/// </summary>
		public bool FlushBufferOnNextRender {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the parent model (can be null, not guaranteed to be accurate)
		/// </summary>
		public IModel Parent {
			get {
				return parent;
			}
			set {
				parent = value;
			}
		}

		/// <summary>
		/// Gets or sets the name of the component.
		/// </summary>
		public string Name {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets an object that contains properties relevant to this mesh.
		/// </summary>
		public object Tag {
			get;
			set;
		}

		/// <summary>
		/// Gets the vertex data of the mesh. If the mesh is cloned, it may be shared unless BeginUpdateMesh() was called.
		/// </summary>
		public Vertex[] BufferData {
			get {
				Vertex[] buffer = bufferData;
				if (buffer != null)
					return buffer;
				if (vertices == 0)
					return new Vertex[0];
				DataBuffer dataBuffer = DataBuffer;
				if (dataBuffer == null)
					return null;
				DataBuffer.Bind(BufferTarget.ArrayBuffer);
				unsafe
				{
					buffer = new Vertex[vertices];
					fixed (Vertex* ptr = buffer)
						GL.GetBufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, BufferSize, new IntPtr(ptr));
					bufferData = buffer;
					return buffer;
				}
			}
		}

		/// <summary>
		/// Gets the number of triangles in the model.
		/// </summary>
		public int Triangles {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return triangles;
			}
		}

		/// <summary>
		/// Creates a new empty mesh component.
		/// </summary>
		public static MeshComponent Empty {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return new MeshComponent(Texture2D.Empty, null, null, BufferUsageHint.StaticDraw, false);
			}
		}

		/// <summary>
		/// Gets or sets whether the model is visible (rendered) or not.
		/// </summary>
		public bool Visible {
			get;
			set;
		}

		/// <summary>
		/// If true, the backs of mesh components are not rendered.
		/// </summary>
		public bool Cull {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the rendering mode of the polygons of the mesh.
		/// </summary>
		public MeshMode MeshMode {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the transparency of the mesh material (1 is opaque, 0 is transparent).
		/// </summary>
		public float Alpha {
			get {
				return MaterialHue.A;
			}
			set {
				ColorF materialHue = MaterialHue;
				materialHue.A = value;
				MaterialHue = materialHue;
			}
		}

		/// <summary>
		/// Gets or sets the type of the model for optimization.
		/// </summary>
		public BufferUsageHint Optimization {
			get;
			set;
		}

		/// <summary>
		/// Gets the number of vertices in the model.
		/// </summary>
		public int Vertices {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return vertices;
			}
		}

		/// <summary>
		/// Gets or sets the default texture of the model when none is specified.
		/// </summary>
		public ITexture[] DefaultTextures {
			get {
				return new ITexture[] { defaultTexture };
			}
			set {
				if (value == null || value.Length == 0)
					value = Texture2D.EmptyTexture;
				if (value[0] == defaultTexture)
					return;
				defaultTexture.Dispose();
				defaultTexture = value[0];
				defaultTexture.AddReference();
				if (Texture2D.Empty.Equals(texture))
					texture = defaultTexture;
			}
		}

		/// <summary>
		/// Gets or sets the texture used by the model (only the texture at index 0 will be used).
		/// </summary>
		public ITexture[] Textures {
			get {
				return new ITexture[] { texture };
			}
			set {
				if (value == null || value.Length == 0)
					value = DefaultTextures;
				if (value[0] == texture)
					return;
				texture.Dispose();
				texture = value[0];
				texture.AddReference();
			}
		}

		/// <summary>
		/// Gets whether the model is disposed.
		/// </summary>
		public bool IsDisposed {
			get {
				return isDisposed;
			}
		}

		/// <summary>
		/// Gets the location of the centre of the model.
		/// </summary>
		public Vector3 Center {
			get {
				Vertex[] buffer = BufferData;
				if (buffer.Length == 0)
					return currentLoc;
				Vector3 total = buffer[0].Pos;
				for (int i = 1; i < buffer.Length; i++)
					total += buffer[i].Pos;
				return total / vertices;
			}
		}

		/// <summary>
		/// Gets or sets the relative location of the model.
		/// </summary>
		public Vector3 Location {
			get {
				return currentLoc;
			}
			set {
				if (value == currentLoc)
					return;
				currentLoc = value;
				RaiseLocationChanged();
			}
		}

		/// <summary>
		/// Gets or sets the relative scale of the model.
		/// </summary>
		public Vector3 Scale {
			get {
				return currentScale;
			}
			set {
				if (value == currentScale)
					return;
				currentScale = value;
				RaiseScaleChanged();
			}
		}

		/// <summary>
		/// Gets or sets the relative rotation of the model in radians (angle X, angle Y, angle Z).
		/// </summary>
		public Vector3 Rotation {
			get {
				return currentRot;
			}
			set {
				if (value == currentRot)
					return;
				currentRot = value;
				RaiseRotationChanged();
			}
		}

		/// <summary>
		/// Gets the width of the bounding box of the model.
		/// </summary>
		public float Width {
			get {
				Vertex[] buffer = BufferData;
				if (buffer.Length == 0)
					return 0f;
				float maxX = buffer[0].Pos.X;
				float minX = maxX;
				float temp;
				for (int i = 1; i < buffer.Length; i++) {
					temp = buffer[i].Pos.X;
					if (temp > maxX)
						maxX = temp;
					else if (temp < minX)
						minX = temp;
				}
				return maxX - minX;
			}
		}

		/// <summary>
		/// Gets the height of the bounding box of the model.
		/// </summary>
		public float Height {
			get {
				Vertex[] buffer = BufferData;
				if (buffer.Length == 0)
					return 0f;
				float maxY = buffer[0].Pos.Y;
				float minY = maxY;
				float temp;
				for (int i = 1; i < buffer.Length; i++) {
					temp = buffer[i].Pos.Y;
					if (temp > maxY)
						maxY = temp;
					else if (temp < minY)
						minY = temp;
				}
				return maxY - minY;
			}
		}

		/// <summary>
		/// Gets the depth of the bounding box of the model.
		/// </summary>
		public float Depth {
			get {
				Vertex[] buffer = BufferData;
				if (buffer.Length == 0)
					return 0f;
				float maxZ = buffer[0].Pos.Z;
				float minZ = maxZ;
				float temp;
				for (int i = 1; i < buffer.Length; i++) {
					temp = buffer[i].Pos.Z;
					if (temp > maxZ)
						maxZ = temp;
					else if (temp < minZ)
						minZ = temp;
				}
				return maxZ - minZ;
			}
		}

		/// <summary>
		/// Gets the smallest X coordinate.
		/// </summary>
		public float MinX {
			get {
				Vertex[] buffer = BufferData;
				if (buffer.Length == 0)
					return currentLoc.X;
				float minX = buffer[0].Pos.X;
				float temp;
				for (int i = 1; i < buffer.Length; i++) {
					temp = buffer[i].Pos.X;
					if (temp < minX)
						minX = temp;
				}
				return minX;
			}
		}

		/// <summary>
		/// Gets the smallest Y coordinate.
		/// </summary>
		public float MinY {
			get {
				Vertex[] buffer = BufferData;
				if (buffer.Length == 0)
					return currentLoc.Y;
				float minY = buffer[0].Pos.Y;
				float temp;
				for (int i = 1; i < buffer.Length; i++) {
					temp = buffer[i].Pos.Y;
					if (temp < minY)
						minY = temp;
				}
				return minY;
			}
		}

		/// <summary>
		/// Gets the smallest Z coordinate.
		/// </summary>
		public float MinZ {
			get {
				Vertex[] buffer = BufferData;
				if (buffer.Length == 0)
					return currentLoc.Z;
				float minZ = buffer[0].Pos.Z;
				float temp;
				for (int i = 1; i < buffer.Length; i++) {
					temp = buffer[i].Pos.Z;
					if (temp < minZ)
						minZ = temp;
				}
				return minZ;
			}
		}

		/// <summary>
		/// Gets the largest X coordinate.
		/// </summary>
		public float MaxX {
			get {
				Vertex[] buffer = BufferData;
				if (buffer.Length == 0)
					return currentLoc.X;
				float maxX = buffer[0].Pos.X;
				float temp;
				for (int i = 1; i < buffer.Length; i++) {
					temp = buffer[i].Pos.X;
					if (temp > maxX)
						maxX = temp;
				}
				return maxX;
			}
		}

		/// <summary>
		/// Gets the largest Y coordinate.
		/// </summary>
		public float MaxY {
			get {
				Vertex[] buffer = BufferData;
				if (buffer.Length == 0)
					return currentLoc.Y;
				float maxY = buffer[0].Pos.Y;
				float temp;
				for (int i = 1; i < buffer.Length; i++) {
					temp = buffer[i].Pos.Y;
					if (temp > maxY)
						maxY = temp;
				}
				return maxY;
			}
		}

		/// <summary>
		/// Gets the largest Z coordinate.
		/// </summary>
		public float MaxZ {
			get {
				Vertex[] buffer = BufferData;
				if (buffer.Length == 0)
					return currentLoc.Z;
				float maxZ = buffer[0].Pos.Z;
				float temp;
				for (int i = 1; i < buffer.Length; i++) {
					temp = buffer[i].Pos.Z;
					if (temp > maxZ)
						maxZ = temp;
				}
				return maxZ;
			}
		}

		/// <summary>
		/// Gets the bounding box sizes of the model.
		/// </summary>
		public Vector3 Bounds {
			get {
				Vertex[] buffer = BufferData;
				if (buffer.Length == 0)
					return Vector3.Zero;
				Vector3 max = buffer[0].Pos;
				Vector3 min = max;
				Vector3 temp;
				for (int i = 1; i < buffer.Length; i++) {
					temp = buffer[i].Pos;
					if (temp.X > max.X)
						max.X = temp.X;
					else if (temp.X < min.X)
						min.X = temp.X;
					if (temp.Y > max.Y)
						max.Y = temp.Y;
					else if (temp.Y < min.Y)
						min.Y = temp.Y;
					if (temp.Z > max.Z)
						max.Z = temp.Z;
					else if (temp.Z < min.Z)
						min.Z = temp.Z;
				}
				return max - min;
			}
		}

		/// <summary>
		/// Creates a copy of an existing model.
		/// </summary>
		/// <param name="modelComponent">The model to copy.</param>
		public MeshComponent(MeshComponent modelComponent) {
			if (modelComponent == null)
				return;
			bufferData = modelComponent.bufferData;
			AmbientHue = modelComponent.AmbientHue;
			Cull = modelComponent.Cull;
			generatedMatrix = modelComponent.generatedMatrix;
			Tag = modelComponent.Tag;
			isDisposed = modelComponent.isDisposed;
			LowOpacity = modelComponent.LowOpacity;
			MaterialHue = modelComponent.MaterialHue;
			MeshMode = modelComponent.MeshMode;
			Name = modelComponent.Name;
			Shininess = modelComponent.Shininess;
			oldLoc = modelComponent.oldLoc;
			oldRot = modelComponent.oldRot;
			oldScale = modelComponent.oldScale;
			ShineHue = modelComponent.ShineHue;
			Transformation = modelComponent.Transformation;
			UseCustomTransformation = modelComponent.UseCustomTransformation;
			WrapMode = modelComponent.WrapMode;
			Optimization = modelComponent.Optimization;
			currentLoc = modelComponent.currentLoc;
			currentRot = modelComponent.currentRot;
			currentScale = modelComponent.currentScale;
			BufferSize = modelComponent.BufferSize;
			Visible = modelComponent.Visible;
			triangles = modelComponent.triangles;
			vertices = modelComponent.vertices;
			defaultTexture = modelComponent.defaultTexture;
			if (defaultTexture != null)
				defaultTexture.AddReference();
			texture = modelComponent.texture;
			if (texture == null)
				texture = Texture2D.Empty;
			else
				texture.AddReference();
			indexBuffer = modelComponent.indexBuffer;
			if (indexBuffer != null)
				indexBuffer.AddReference();
			DataBuffer = modelComponent.DataBuffer;
			if (DataBuffer != null)
				DataBuffer.AddReference();
			VertexArrayBuffer = modelComponent.VertexArrayBuffer;
			if (VertexArrayBuffer != null)
				VertexArrayBuffer.AddReference();
			FlushBufferOnNextRender = true;
			isClone = true;
		}

		/// <summary>
		/// Initializes a new mesh component.
		/// </summary>
		/// <param name="texture">The texture to use with the model.</param>
		/// <param name="optimization">Configures what the mesh is optimized for.</param>
		/// <param name="vertices">A sequential array of all the vertices of the triangles of the mesh.</param>
		/// <param name="optimizeDuplicates">Whether to remove duplicates from the array (may take a long time to perform).</param>
		/// <param name="flushBuffer">Whether to flush the buffer to VRAM on first render. This frees memory, but means that subsequent operations that
		/// manipulate the mesh vertices must be performed on an OpenGL thread.</param>
		public MeshComponent(ITexture texture, Vertex[] vertices, bool optimizeDuplicates, BufferUsageHint optimization = BufferUsageHint.StaticDraw, bool flushBuffer = true) : this(texture, MeshExtensions.GenerateIndices(vertices, optimizeDuplicates), optimization, flushBuffer) {
		}

		/// <summary>
		/// Initializes a new mesh component.
		/// </summary>
		/// <param name="texture">The texture to use with the model.</param>
		/// <param name="bufferData">A tuple where the first element is sequential array of all the vertices of the triangles of the mesh (can't be null),
		/// and the second element contains indices that represent the vertex order (can be byte[], ushort[] or uint[])..</param>
		/// <param name="optimization">Configures what the mesh is optimized for.</param>
		/// <param name="flushBuffer">Whether to flush the buffer to VRAM on first render. This frees memory, but means that subsequent operations that
		/// manipulate the mesh vertices must be performed on an OpenGL thread.</param>
		public MeshComponent(ITexture texture, Tuple<Vertex[], Array> bufferData, BufferUsageHint optimization = BufferUsageHint.StaticDraw, bool flushBuffer = true) : this(texture, bufferData == null ? null : bufferData.Item1, bufferData == null ? null : bufferData.Item2, optimization, flushBuffer) {
		}

		/// <summary>
		/// Initializes a new mesh component.
		/// </summary>
		/// <param name="texture">The texture to use with the model.</param>
		/// <param name="optimization">Configures what the mesh is optimized for.</param>
		/// <param name="vertices">A sequential array of all the vertices of the triangles of the mesh.</param>
		/// <param name="indices">The indices that represent the vertex order (can be byte[], ushort[] or uint[]).</param>
		/// <param name="flushBuffer">Whether to flush the buffer to VRAM on first render. This frees memory, but means that subsequent operations that
		/// manipulate the mesh vertices must be performed on an OpenGL thread.</param>
		public MeshComponent(ITexture texture, Vertex[] vertices, Array indices, BufferUsageHint optimization = BufferUsageHint.StaticDraw, bool flushBuffer = true) {
			if (vertices == null)
				bufferData = new Vertex[0];
			else
				bufferData = vertices;
			if (texture == null)
				this.texture = Texture2D.Empty;
			else {
				this.texture = texture;
				texture.AddReference();
			}
			Optimization = optimization;
			Visible = true;
			Cull = true;
			DataBuffer = new DataBuffer();
			if (GL.Delegates.glGenVertexArrays != null)
				VertexArrayBuffer = new VertexArrayBuffer();
			triangles = indices.Length / 3;
			indexBuffer = IndexBuffer.FromArray(indices);
			this.vertices = bufferData.Length;
			BufferSize = new IntPtr(bufferData.Length * Vertex.SizeOfVertex);
			updateBuffer = true;
			generatedMatrix = Matrix4.CreateTranslation(ref currentLoc) * Matrix4.CreateRotationZ(currentRot.Z) * Matrix4.CreateRotationY(currentRot.Y) * Matrix4.CreateRotationX(currentRot.X) * Matrix4.CreateScale(ref currentScale);
			FlushBufferOnNextRender = flushBuffer;
			RestoreDefaults();
		}

		/// <summary>
		/// Resets components to the defaults.
		/// </summary>
		public void RestoreDefaults() {
			LowOpacity = false;
			UseCustomTransformation = false;
			currentLoc = Vector3.Zero;
			currentRot = Vector3.Zero;
			currentScale = Vector3.One;
			MaterialHue = Light.DefaultMaterialHue;
			AmbientHue = Light.DefaultAmbientHue;
			ShineHue = Light.DefaultShineHue;
			Shininess = Light.DefaultShininess;
			Transformation = Matrix4.Identity;
			WrapMode = TextureWrapMode.Repeat;
		}

		/// <summary>
		/// Flushes the buffer to VRAM. This *MUST* be called on an OpenGL rendering thread.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void FlushBuffer() {
			FlushBufferOnNextRender = false;
			Vertex[] buffer = bufferData;
			if (buffer == null)
				return;
			bufferData = null;
			if (updateBuffer) {
				updateBuffer = false;
				DataBuffer.Bind(BufferTarget.ArrayBuffer);
				GL.BufferData(BufferTarget.ArrayBuffer, BufferSize, buffer, Optimization);
			}
		}

		/// <summary>
		/// Raises the RenderBegin event.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		protected void RaiseRenderBegin() {
			Action handler = RenderBegin;
			if (handler != null)
				handler();
		}

		/// <summary>
		/// Raises the RenderEnd event.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		protected void RaiseRenderEnd() {
			Action handler = RenderEnd;
			if (handler != null)
				handler();
		}

		/// <summary>
		/// Raises the LocationChanged event.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		protected void RaiseLocationChanged() {
			Action handler = LocationChanged;
			if (handler != null)
				handler();
		}

		/// <summary>
		/// Raises the ScaleChanged event.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		protected void RaiseScaleChanged() {
			Action handler = ScaleChanged;
			if (handler != null)
				handler();
		}

		/// <summary>
		/// Raises the RotationChanged event.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		protected void RaiseRotationChanged() {
			Action handler = RotationChanged;
			if (handler != null)
				handler();
		}

		/// <summary>
		/// (Optional) Checks whether the model view has been updated and if so recalculates the transformation matrix.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void CheckMatrixUpdate() {
			if (UseCustomTransformation || (currentLoc == oldLoc && currentScale == oldScale && currentRot == oldRot))
				return;
			oldLoc = currentLoc;
			oldScale = currentScale;
			oldRot = currentRot;
			generatedMatrix = Matrix4.CreateTranslation(ref oldLoc) * Matrix4.CreateRotationZ(oldRot.Z) * Matrix4.CreateRotationX(oldRot.X) * Matrix4.CreateRotationY(oldRot.Y) * Matrix4.CreateScale(ref oldScale);
		}

		/// <summary>
		/// Sets up the initial settings for an OpenGL context. Call this before anything is initialized or drawn.
		/// </summary>
		public static void SetupGLEnvironment() {
			GL.Enable(EnableCap.Texture2D);
			GL.Enable(EnableCap.Blend);
			GL.Enable(EnableCap.AlphaTest);
			GL.Enable(EnableCap.Multisample);
			GL.Enable(EnableCap.DepthClamp);
			GL.DepthFunc(DepthFunction.Lequal);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.Hint(HintTarget.GenerateMipmapHint, HintMode.Nicest);
			GL.Hint(HintTarget.FogHint, HintMode.Fastest);
			GL.Hint(HintTarget.LineSmoothHint, HintMode.Fastest);
			GL.Hint(HintTarget.PointSmoothHint, HintMode.Fastest);
			GL.Hint(HintTarget.PolygonSmoothHint, HintMode.Fastest);
			GL.Hint(HintTarget.TextureCompressionHint, HintMode.Fastest);
			GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Fastest);
			GL.CullFace(CullFaceMode.Back);
			GL.EnableClientState(ArrayCap.VertexArray);
			GL.EnableClientState(ArrayCap.TextureCoordArray);
			GL.EnableClientState(ArrayCap.NormalArray);
		}

		/// <summary>
		/// Sets the camera projection to orthographic at the specified size.
		/// </summary>
		/// <param name="viewport">The view port size (to map to relative pixel coordinates).</param>
		/// <param name="maxZ">The maximum Z-depth.</param>
		/// <param name="disableDepth">Whether to render consequent polygons as 2D.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Setup2D(Size viewport, float maxZ = 1f, bool disableDepth = true) {
			Mesh2D.Setup2D(new Vector2(viewport.Width, viewport.Height), maxZ, disableDepth);
		}

		/// <summary>
		/// Sets the camera projection to orthographic at the specified size.
		/// </summary>
		/// <param name="viewport">The view port size (to map to relative pixel coordinates).</param>
		/// <param name="maxZ">The maximum Z-depth.</param>
		/// <param name="disableDepth">Whether to render consequent polygons as 2D.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Setup2D(Vector2 viewport, float maxZ, bool disableDepth = true) {
			Mesh2D.Setup2D(viewport, maxZ, disableDepth);
		}

		/// <summary>
		/// Sets the camera projection to orthographic using the specified matrix.
		/// </summary>
		/// <param name="ortho">The orthographic transform matrix (usually using Matrix4.CreateOrthographicOffCenter()).</param>
		/// <param name="disableDepth">Whether to render consequent polygons as 2D.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Setup2D(ref Matrix4 ortho, bool disableDepth = true) {
			Mesh2D.Setup2D(ref ortho, disableDepth);
		}

		/// <summary>
		/// Sets up the required OpenGL settings so as to create the necessary environment for the models to work (must be called on a thread on which an OpenGL context is set up, preferably on the thread on which the model was loaded).
		/// </summary>
		/// <param name="viewPort">The size of the OpenGL viewport.</param>
		/// <param name="camera">The camera matrix.</param>
		/// <param name="shortestDistance">The closest an object can get to a camera before disappearing.
		/// Too close (ie. smaller than 2.5f) may result in depth test issues and some far away polygons may flicker in some situations.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Setup3D(Size viewPort, ref Matrix4 camera, float shortestDistance) {
			Setup3D(new Vector2(viewPort.Width, viewPort.Height), ref camera, shortestDistance);
		}

		/// <summary>
		/// Sets up the required OpenGL settings so as to create the necessary environment for the models to work (must be called on a thread on which an OpenGL context is set up, preferably on the thread on which the model was loaded).
		/// </summary>
		/// <param name="viewPort">The size of the OpenGL viewport.</param>
		/// <param name="camera">The camera matrix.</param>
		/// <param name="shortestDistance">The closest an object can get to a camera before disappearing.
		/// Too close (ie. smaller than 2.5f) may result in depth test issues and some far away polygons may flicker in some situations.</param>
		public static void Setup3D(Vector2 viewPort, ref Matrix4 camera, float shortestDistance) {
			Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(Maths.PiOver4F, viewPort.Y == 0f ? 1f : viewPort.X / viewPort.Y, shortestDistance, 80000f);
			GlobalShader globalShader = (GlobalShader) Shader.CurrentShader;
			globalShader.SetUniformValue(GlobalShaderParams.ProjectionMatrix.ToString(), ref projection, ShaderSetMode.SetImmediately);
			globalShader.SetUniformValue(GlobalShaderParams.ModelViewMatrix.ToString(), ref camera, ShaderSetMode.SetImmediately);
			globalShader.SetUniformValue(GlobalShaderParams.LightingEnabled.ToString(), 1f, ShaderSetMode.SetImmediately);
			GL.Enable(EnableCap.DepthTest);
		}

		/// <summary>
		/// Renders the mesh (must be called on a thread on which an OpenGL context is set up, preferably on the thread on which the model was loaded)
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void Render() {
			Render(texture);
		}

		/// <summary>
		/// Renders the mesh with the specified texture (must be called on a thread on which an OpenGL context is set up, preferably on the thread on which the model was loaded)
		/// </summary>
		/// <param name="texture">The texture to bind instead of the default (can be null)</param>
		public void Render(ITexture texture) {
			Render(texture, null);
		}

		/// <summary>
		/// Renders the mesh with the specified texture (must be called on a thread on which an OpenGL context is set up, preferably on the thread on which the model was loaded)
		/// </summary>
		/// <param name="nextModel">The next mesh component to interpolate with (can be null)</param>
		public void Render(IModel nextModel) {
			Render(null, (MeshComponent) nextModel);
		}

		/// <summary>
		/// Renders the mesh with the specified texture (must be called on a thread on which an OpenGL context is set up, preferably on the thread on which the model was loaded)
		/// </summary>
		/// <param name="texture">The texture to bind instead of the default</param>
		/// <param name="nextModel">The next mesh component to interpolate with (can be null)</param>
		public virtual void Render(ITexture texture, MeshComponent nextModel) {
			if (!Visible || vertices == 0 || DataBuffer == null)
				return;
			float alpha = MaterialHue.A;
			IModel parent = this.parent;
			while (parent != null) {
				alpha *= parent.Alpha;
				parent = parent.Parent;
			}
			if (alpha <= 0f)
				return;
			RaiseRenderBegin();
			bool lowOpacity = LowOpacity || alpha < 0.97f;
			if (lowOpacity) {
				GL.DepthMask(false);
				GL.Disable(EnableCap.CullFace);
			} else if (Cull)
				GL.Enable(EnableCap.CullFace);
			else
				GL.Disable(EnableCap.CullFace);
			MeshMode meshMode = MeshMode;
			if (meshMode == MeshMode.WireFrame)
				GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
			ColorF materialHue = MaterialHue;
			materialHue.A = alpha;
			GlobalShader shader = (GlobalShader) Shader.CurrentShader;
			if (UseCustomTransformation)
				shader.SetUniformValue(GlobalShaderParams.WorldViewMatrix.ToString(), ref Transformation, ShaderSetMode.SetImmediately);
			else {
				CheckMatrixUpdate();
				shader.SetUniformValue(GlobalShaderParams.WorldViewMatrix.ToString(), ref generatedMatrix, ShaderSetMode.SetImmediately);
			}
			shader.SetUniformValue(GlobalShaderParams.MaterialHue.ToString(), materialHue, ShaderSetMode.SetImmediately);
			bool premultiplied = false;
			if (texture == null)
				texture = this.texture;
			if (texture == null) {
				GL.BindTexture(TextureTarget.Texture2D, 0);
				shader.SetUniformValue(GlobalShaderParams.UseTexture.ToString(), 0f, ShaderSetMode.SetImmediately);
			} else {
				premultiplied = texture.Premultiplied;
				if (premultiplied)
					GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);
				texture.Bind(WrapMode);
				shader.SetUniformValue(GlobalShaderParams.UseTexture.ToString(), texture.Name == 0 ? 0f : 1f, ShaderSetMode.SetImmediately);
			}
			shader.SetUniformValue(GlobalShaderParams.AmbientHue.ToString(), AmbientHue, ShaderSetMode.SetImmediately);
			shader.SetUniformValue(GlobalShaderParams.ShineHue.ToString(), ShineHue, ShaderSetMode.SetImmediately);
			shader.SetUniformValue(GlobalShaderParams.Shininess.ToString(), Shininess, ShaderSetMode.SetImmediately);
			bool hasBufferUpdated = false;
			VertexArrayBuffer vab = VertexArrayBuffer;
			if (vab == null) {
				if (GL.Delegates.glGenVertexArrays != null) {
					vab = new VertexArrayBuffer();
					VertexArrayBuffer = vab;
					vab.Bind();
					hasBufferUpdated = true;
				}
			} else {
				ErrorCode error = GL.GetError();
				if (error != ErrorCode.NoError)
					Console.WriteLine("OpenGL error found before binding VAO: " + error);
				vab.Bind();
				error = GL.GetError();
				if (error != ErrorCode.NoError) {
					vab.Dispose();
					vab = new VertexArrayBuffer();
					VertexArrayBuffer = vab;
					vab.Bind();
					hasBufferUpdated = true;
					error = GL.GetError();
					if (error != ErrorCode.NoError)
						Console.WriteLine("OpenGL error found before after binding VAO: " + error);
				}
			}
			hasBufferUpdated |= BindAndUpdateDataBuffer();
			if (vab == null || hasBufferUpdated || nextModel != lastNextModel) {
				lastNextModel = nextModel;
				GL.EnableVertexAttribArray(0);
				GL.EnableVertexAttribArray(1);
				GL.EnableVertexAttribArray(2);
				GL.EnableVertexAttribArray(3);
				GL.EnableVertexAttribArray(4);
				GL.VertexAttribPointer(shader.GetAttributeIndex(GlobalShaderAttribs.Position.ToString(), true), 3, VertexAttribPointerType.Float, false, Vertex.SizeOfVertex, IntPtr.Zero);
				GL.VertexAttribPointer(shader.GetAttributeIndex(GlobalShaderAttribs.TexCoord.ToString(), true), 2, VertexAttribPointerType.Float, false, Vertex.SizeOfVertex, new IntPtr(Vector3Size));
				GL.VertexAttribPointer(shader.GetAttributeIndex(GlobalShaderAttribs.Normal.ToString(), true), 3, VertexAttribPointerType.Float, false, Vertex.SizeOfVertex, new IntPtr(Vector3Size + Vector2Size));
				if (nextModel != null)
					nextModel.BindAndUpdateDataBuffer();
				GL.VertexAttribPointer(shader.GetAttributeIndex(GlobalShaderAttribs.Position2.ToString(), true), 3, VertexAttribPointerType.Float, false, Vertex.SizeOfVertex, IntPtr.Zero);
				GL.VertexAttribPointer(shader.GetAttributeIndex(GlobalShaderAttribs.Normal2.ToString(), true), 3, VertexAttribPointerType.Float, false, Vertex.SizeOfVertex, new IntPtr(Vector3Size + Vector2Size));
			}
			if (FlushBufferOnNextRender && !KeepCopyInMemory)
				FlushBuffer();
			indexBuffer.Bind();
			GL.DrawElements(BeginMode.Triangles, indexBuffer.Count, indexBuffer.Format, IntPtr.Zero);
			if (vab == null) {
				GL.DisableVertexAttribArray(0);
				GL.DisableVertexAttribArray(1);
				GL.DisableVertexAttribArray(2);
				GL.DisableVertexAttribArray(3);
				GL.DisableVertexAttribArray(4);
			} else
				VertexArrayBuffer.Unbind();
			if (premultiplied)
				GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			if (lowOpacity)
				GL.DepthMask(true);
			GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
			if (nextModel != null)
				shader.SetUniformValue(GlobalShaderParams.Interpolate.ToString(), 0f, ShaderSetMode.SetImmediately);
			RaiseRenderEnd();
		}

		private bool BindAndUpdateDataBuffer() {
			DataBuffer.Bind(BufferTarget.ArrayBuffer);
			bool hasBufferUpdated = false;
			if (updateBuffer) {
				updateBuffer = false;
				hasBufferUpdated = true;
				Vertex[] newBuffer = bufferData;
				if (newBuffer != null) {
					GL.BufferData(BufferTarget.ArrayBuffer, BufferSize, IntPtr.Zero, Optimization);
					GL.BufferData(BufferTarget.ArrayBuffer, BufferSize, newBuffer, Optimization);
				}
			}
			return hasBufferUpdated;
		}

		/// <summary>
		/// Signals that the buffer is about to be updated.
		/// </summary>
		public void BeginUpdateMesh() {
			if (isClone) {
				isClone = false;
				Vertex[] v = bufferData;
				Vertex[] newBuffer = new Vertex[v.Length];
				Array.Copy(v, newBuffer, v.Length);
				bufferData = newBuffer;
				if (DataBuffer != null)
					DataBuffer.Dispose();
				DataBuffer = new DataBuffer();
				VertexArrayBuffer vab = VertexArrayBuffer;
				if (vab != null)
					vab.Dispose();
				if (GL.Delegates.glGenVertexArrays != null)
					VertexArrayBuffer = new VertexArrayBuffer();
				updateBuffer = true;
			}
		}

		/// <summary>
		/// Signals that the buffer should be updated on next rendering.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public void EndUpdateMesh() {
			updateBuffer = true;
		}

		/// <summary>
		/// Translate the model by the specified vector. CALL BeginUpdateMesh() BEFORE CALLING THIS METHOD.
		/// </summary>
		/// <param name="vector">The vector to translate at.</param>
		public void TranslateMesh(Vector3 vector) {
			if (vector == Vector3.Zero)
				return;
			Vertex[] buffer = BufferData;
			for (int i = 0; i < buffer.Length; i++)
				buffer[i].Pos += vector;
			EndUpdateMesh();
		}

		/// <summary>
		/// Enlarges the model to the specified scale. CALL BeginUpdateMesh() BEFORE CALLING THIS METHOD.
		/// </summary>
		/// <param name="scale">The scale to enlarge by.</param>
		/// <param name="centre">The centre to enlarge against.</param>
		public void ScaleMesh(float scale, Vector3 centre) {
			if (scale == 1f)
				return;
			Vertex[] buffer = BufferData;
			ParallelLoop.For(0, buffer.Length, i => buffer[i].Pos = ((buffer[i].Pos - centre) * scale) + centre);
			EndUpdateMesh();
		}

		/// <summary>
		/// Enlarges the model to the specified scale. CALL BeginUpdateMesh() BEFORE CALLING THIS METHOD.
		/// </summary>
		/// <param name="scale">Contains the scale of the X, Y and Z dimension individually.</param>
		/// <param name="centre">The centre to enlarge against.</param>
		public void ScaleMesh(Vector3 scale, Vector3 centre) {
			if (scale == Vector3.One)
				return;
			Vertex[] buffer = BufferData;
			ParallelLoop.For(0, buffer.Length, i => buffer[i].Pos = (Vector3.Multiply((buffer[i].Pos - centre), scale)) + centre);
			EndUpdateMesh();
		}

		/// <summary>
		/// Transforms the model using the specified matrix. CALL BeginUpdateMesh() BEFORE CALLING THIS METHOD.
		/// </summary>
		/// <param name="matrix">The matrix to multiply transform vertices with.</param>
		public void ApplyMatrixTransformation(ref Matrix4 matrix) {
			Matrix4 temp = matrix;
			Vertex[] buffer = BufferData;
			ParallelLoop.For(0, buffer.Length, i => buffer[i].Pos = buffer[i].Pos.Transform(ref temp));
			EndUpdateMesh();
		}

		/// <summary>
		/// Rotates the model.
		/// </summary>
		/// <param name="rotation">Contains the rotation of the X, Y and Z axis individually.</param>
		public void RotateMesh(Vector3 rotation) {
			if (rotation == Vector3.Zero)
				return;
			Matrix4 temp = Matrix4.CreateRotationZ(rotation.Z) * Matrix4.CreateRotationY(rotation.Y) * Matrix4.CreateRotationX(rotation.X);
			ApplyMatrixTransformation(ref temp);
		}

		/// <summary>
		/// Gets a string that describes some aspects of the mesh component.
		/// </summary>
		public override string ToString() {
			string name = Name == null ? null : Name.Trim();
			if (name == null || name.Length == 0)
				return "{ Vertices: " + Vertices + " }";
			else
				return "{ Vertices: " + Vertices + ", Name: " + name + " }";
		}

		/// <summary>
		/// Returns an enumerator that iterates through the vertices of the model.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public IEnumerator<Vertex> GetEnumerator() {
			return ((IEnumerable<Vertex>) BufferData).GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through the vertices of the model.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		IEnumerator IEnumerable.GetEnumerator() {
			return BufferData.GetEnumerator();
		}

		/// <summary>
		/// Creates a copy of the model.
		/// </summary>
		/// <returns>A copy of the model.</returns>
		public object Clone() {
			return new MeshComponent(this);
		}

		/// <summary>
		/// Creates a copy of the model.
		/// </summary>
		/// <param name="components">Ignored.</param>
		/// <returns>A copy of the model.</returns>
		public IModel Clone(bool components) {
			return new MeshComponent(this);
		}

		/// <summary>
		/// If this is called, then there is a memory leak because Dispose() is not called.
		/// Disposes of the OpenGL resources used by the model.
		/// </summary>
		~MeshComponent() {
			GraphicsContext.IsFinalizer = true;
			try {
				Dispose();
			} finally {
				GraphicsContext.IsFinalizer = false;
			}
		}

		/// <summary>
		/// Called when the mesh component is being disposed.
		/// </summary>
		protected virtual void OnDisposing() {
		}

		/// <summary>
		/// Disposes of the OpenGL resources used by the model.
		/// </summary>
		public void Dispose() {
			Dispose(true, true);
		}

		/// <summary>
		/// Disposes of the resources used by the model.
		/// </summary>
		/// <param name="disposeChildren">Whether to dispose of the child components of the model.</param>
		public void Dispose(bool disposeChildren) {
			Dispose(disposeChildren, true);
		}

		/// <summary>
		/// Disposes of the resources used by the model
		/// </summary>
		/// <param name="disposeChildren">Whether to dispose of the child components of the model</param>
		/// <param name="removeFromParent">Always set to true. Except in the Dispose() function of the parent, as all child components are removed automatically</param>
		public void Dispose(bool disposeChildren, bool removeFromParent) {
			if (isDisposed)
				return;
			isDisposed = true;
			OnDisposing();
			if (removeFromParent) {
				Model tentativeParent = parent as Model;
				if (tentativeParent != null)
					tentativeParent.Remove(this);
			}
			parent = null;
			lastNextModel = null;
			if (texture != null) {
				texture.Dispose();
				texture = null;
			}
			if (defaultTexture != null) {
				defaultTexture.Dispose();
				defaultTexture = null;
			}
			if (DataBuffer != null) {
				DataBuffer.Dispose();
				DataBuffer = null;
			}
			if (indexBuffer != null) {
				indexBuffer.Dispose();
				indexBuffer = null;
			}
			if (VertexArrayBuffer != null) {
				VertexArrayBuffer.Dispose();
				VertexArrayBuffer = null;
			}
			bufferData = null;
			GC.SuppressFinalize(this);
		}
	}
}