using System.Drawing;

namespace System.Physics2D {
	public struct CollisionEventArgs {
		public static readonly CollisionEventArgs Empty = new CollisionEventArgs();
		public Particle P1, P2;
		public Vector2D Normal;
		public double Depth;

		public CollisionEventArgs(Particle p1, Particle p2, Vector2D normal, double depth) {
			P1 = p1;
			P2 = p2;
			Normal = normal;
			Depth = depth;
		}
	}
}