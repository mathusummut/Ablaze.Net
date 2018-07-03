using SlimDX;
using SlimDX.Direct3D9;

namespace PhysicsEngine {
	public interface SignedDistanceMap {
		CollisionPoint Evaluate(Vector3 pt, bool compute_normal);
	}
}