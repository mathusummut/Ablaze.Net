using System.Drawing;

namespace System.Physics2D {
	public class Lift {
		public readonly Particle Component;
		public LiftPath Path;
		private Vector2D previousLocation = new Vector2D(double.NaN, double.NaN);
		private bool isMoving;

		public Lift(Particle p, LiftPath path) {
			isMoving = true;
			Path = path;
			Component = p;
			p.liftComponent = this;
		}

		public static implicit operator Particle(Lift l) {
			return l.Component;
		}

		public void Update() {
			if (isMoving) {
				if (!double.IsNaN(previousLocation.X) && Component.Location.DistanceFrom(previousLocation) < float.Epsilon)
					Path.stuckCounter++;
				Component.Velocity = Path.NextVector(Component.Location);
				previousLocation = Component.Location;
			}
		}

		~Lift() {
			Dispose();
		}

		public void Dispose() {
			Component.Dispose();
			GC.SuppressFinalize(this);
		}
	}
}