using System.Windows;

namespace Ablaze.Physics.System {
	class Force2D {
		//Force acting on a PARTICLE - provides the basis for cumulative rigidbody forces

		private Vector direction;
		private float newtons;

		public Vector Direction {
			get {
				return direction;
			}

			set {
				direction = value;
			}
		}

		public float Newtons {
			get {
				return newtons;
			}

			set {
				newtons = value;
			}
		}
	}
}
