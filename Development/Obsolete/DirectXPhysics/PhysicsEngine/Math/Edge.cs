using SlimDX;

namespace PhysicsEngine {
	public class Edge {
		public Vertex[] v = new Vertex[2];
		public Face[] f = new Face[2];

		public Vector3 normal;

		public void Initialize() {
			normal = Vector3.Normalize(v[0].pos - v[1].pos);
		}

		public void Transform(Matrix m) {
			Vector3.TransformNormal(normal, m);
		}
	}
}