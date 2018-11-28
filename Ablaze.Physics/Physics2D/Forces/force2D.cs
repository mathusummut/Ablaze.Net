using System.Windows.Media.Media3D;
using Ablaze.Physics.Physics2D.Exceptions;
using System;

namespace Ablaze.Physics.System {
	class Force2D {
		//Translational Force acting on a particle or rigidbody

		private Vector3D direction;
		private double newtons;

		public Vector3D Direction {
			get {
				return direction;
			}

			set {
				direction = Vector3D.Multiply(1 / value.Length, value);
			}
		}

		public double Newtons {
			get {
				return newtons;
			}

			set {
				newtons = value;
			}
		}

		public Vector3D Resultant {
			get {
				return Vector3D.Multiply(direction, newtons);
			}
		}

		public Force2D(double N, Vector3D d) {
			newtons = N;
			Direction = d;

			if (d.X == 0 && d.Y == 0)
				throw new NoDirection();
		}

		public Force2D(Force2D F) {
			newtons = F.newtons;
			direction = F.direction;
		}

		public static Force2D operator +(Force2D b, Force2D c) {
			Force2D resultant = new Force2D(b);

			resultant.Direction = b.direction + c.direction;
			resultant.newtons = Math.Sqrt(Math.Pow(b.newtons, 2) + Math.Pow(c.newtons, 2) + b.newtons * c.newtons * Math.Cos(Vector3D.AngleBetween(b.direction, c.direction)));

			return resultant;
		}
	}
}
