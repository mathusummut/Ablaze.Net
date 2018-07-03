using System;
using SlimDX;
using SlimDX.Direct3D9;

namespace PhysicsEngine {
	public class BruteForceSignedDistanceMap : SignedDistanceMap {
		Polyhedron p;

		public BruteForceSignedDistanceMap(Polyhedron p) {
			this.p = p;
		}

		public CollisionPoint Evaluate(Vector3 pt, bool compute_normal) {
			float sqrDist;
			Vector3 closestPt, normal;
			p.SqrDistance(pt, out sqrDist, out closestPt, out normal);

			CollisionPoint v;
			v.p = pt;
			if (Vector3.Dot((pt - closestPt), normal) < 0)
				v.d = -(float) Math.Sqrt(sqrDist);
			else
				v.d = (float) Math.Sqrt(sqrDist);

			v.n = normal;

			return v;
		}
	}
}