using SlimDX;
using SlimDX.Direct3D9;

namespace PhysicsEngine {
	public class State {
		public Vector3 pos;
		public Vector3 vel;
		public Quaternion orientation;
		public Vector3 rotation;

		public State(RigidBody b) {
			pos = b.Pos;
			vel = b.Vel;
			orientation = b.Orientation;
			rotation = b.Rotation;
		}
	}
}