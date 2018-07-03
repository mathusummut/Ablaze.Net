using SlimDX;
using SlimDX.Direct3D9;

namespace PhysicsEngine {
	public struct CollisionPoint {
		public Vector3 p; // point
		public Vector3 n; // normal at point
		public float d; // penetration depth at point

		public CollisionPoint(Vector3 p, Vector3 n, float d) {
			this.p = p;
			this.n = n;
			this.d = d;
		}

		public void Transform(Matrix m) {
			Vector3.TransformCoordinate(p, m);
			Vector3.TransformNormal(n, m);
		}
	}
}
