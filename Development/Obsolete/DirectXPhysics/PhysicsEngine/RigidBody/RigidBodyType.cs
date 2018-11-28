using SlimDX;
using SlimDX.Direct3D9;
using System;
using System.Runtime.InteropServices;

namespace PhysicsEngine {
	public class RigidBodyType : IDisposable {
		public string Name;

		private Mesh mesh;
		private Material[] materials;
		private Texture[] textures;

		public Polyhedron polyhedron;

		public SignedDistanceMap sdm;
		public float volume;
		public Matrix3 inertiaTensor;
		public float radius;

		public RigidBodyType(string meshFileName, Device device) {
			// TODO make this more general, so it doesn't rely on polyhedron, doesn't do the mesh loading, etc.

			/* load mesh from file, compute center of mass, moment
                 * of intertia, transform mesh so center of mass is 0
                 * and moments are on primary axis */
			ExtendedMaterial[] exMaterials;
			mesh = Mesh.FromFile(device, meshFileName, MeshFlags.Managed);
			exMaterials = mesh.GetMaterials();
			textures = new Texture[exMaterials.Length];
			materials = new Material[exMaterials.Length];
			for (int i = 0; i < exMaterials.Length; ++i) {
				if (exMaterials[i].TextureFileName != null && exMaterials[i].TextureFileName.Length != 0) {
					string texturePath =
						System.IO.Path.Combine(System.IO.Path.GetDirectoryName(meshFileName),
						exMaterials[i].TextureFileName);
					textures[i] = Texture.FromFile(device, texturePath);
				}
				materials[i] = exMaterials[i].MaterialD3D;
				materials[i].Ambient = materials[i].Diffuse;
			}
			polyhedron = new Polyhedron(mesh);

			Vector3 centerOfMass;
			polyhedron.Compute(1.0f, out volume, out centerOfMass, out inertiaTensor);

			// transform vertices in original mesh to make the center of mass (0,0,0)
			PositionNormalTextured[] vertList = mesh.LockVertexBuffer(LockFlags.None).ReadRange<PositionNormalTextured>(mesh.VertexCount);
			for (int i = 0; i < vertList.Length; i++) {
				vertList[i].Position -= centerOfMass;
			}
			mesh.UnlockVertexBuffer();

			// transform the polyhedron as well
			polyhedron.Transform(Matrix.Translation(-1 * centerOfMass));

			//sdm = new BruteForceSDM(polyhedron);
			sdm = new GridSignedDistanceMap(polyhedron, 0.1f);

			radius = 0;
			foreach (Vertex v in polyhedron.verts)
				radius = Math.Max(radius, v.pos.Length());
		}

		public void Render(Device device) {
			for (int i = 0; i < materials.Length; ++i) {
				if (textures != null && textures[i] != null) {
					device.SetTexture(0, textures[i]);
				}
				device.Material = materials[i];
				if (mesh != null)
					mesh.DrawSubset(i);
			}
		}

		~RigidBodyType() {
			Dispose();
		}

		public void Dispose() {
			if (mesh == null)
				return;
			mesh.Dispose();
			mesh = null;
			for (int i = 0; i < textures.Length; i++) {
				if (textures[i] != null)
					textures[i].Dispose();
			}
			textures = null;
			GC.SuppressFinalize(this);
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct PositionNormalTextured {
			public float X;

			public float Y;

			public float Z;

			public float Nx;

			public float Ny;

			public float Nz;

			public float Tu;

			public float Tv;

			public static readonly int StrideSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(PositionNormalTextured));

			public Vector3 Normal {
				get {
					Vector3 vector3 = new Vector3();
					vector3 = new Vector3(this.Nx, this.Ny, this.Nz);
					return vector3;
				}
				set {
					this.Nx = value.X;
					this.Ny = value.Y;
					this.Nz = value.Z;
				}
			}

			public Vector3 Position {
				get {
					Vector3 vector3 = new Vector3();
					vector3 = new Vector3(this.X, this.Y, this.Z);
					return vector3;
				}
				set {
					this.X = value.X;
					this.Y = value.Y;
					this.Z = value.Z;
				}
			}

			public PositionNormalTextured(Vector3 pos, Vector3 nor, float u, float v) : this() {
				this.Position = pos;
				this.Normal = nor;
				this.Tu = u;
				this.Tv = v;
			}

			public PositionNormalTextured(float xvalue, float yvalue, float zvalue, float nxvalue, float nyvalue, float nzvalue, float u, float v) {
				this.X = xvalue;
				this.Y = yvalue;
				this.Z = zvalue;
				this.Nx = nxvalue;
				this.Ny = nyvalue;
				this.Nz = nzvalue;
				this.Tu = u;
				this.Tv = v;
			}
		}
	}
}