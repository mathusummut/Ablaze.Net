using System.Drawing;

namespace System.Physics2D {
	public abstract class LiftPath {
		internal int stuckCounter;
		public double Speed;

		public abstract Vector2D NextVector(Vector2D currentLocation);
	}
}