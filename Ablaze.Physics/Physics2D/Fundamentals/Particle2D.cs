using System.Windows.Media.Media3D;
using System;
using Ablaze.Physics.Physics2D.Exceptions;

namespace Ablaze.Physics.System {
	class Particle2D {
		protected double mass; //Mass of particle
		protected Vector3D linearAcceleration; //Acceleration of particle
		protected Vector3D center; //Centroid of particle
		protected double linearMomentum;

		public double Mass {
			get {
				return mass;
			}
		}

		public Vector3D Center {
			get {
				return center;
			}
		}

		//Instantiate particle with mass m position p
		public Particle2D(double m, Vector3D p) {
			if (m > 0) {
				mass = m;
				linearAcceleration = new Vector3D(0, 0, 0);
				center = p;
			} else throw new NegativeMassException();

		}

		//Assume default position
		public Particle2D(double m) {
			if (m > 0) {
				mass = m;
				linearAcceleration = new Vector3D(0, 0, 0);
				center = new Vector3D(0, 0, 0);
			} else throw new NegativeMassException();
		}

		//Constructor only callable via subclasses of Particle2D
		protected Particle2D() {}

		//Applies permanent force on particle (Won't stop unless perfectly balanced by other forces)
		//Force acting on a particle is considered translational
		//At any point in time, a net force is acting on a particle
		public void applyForce(Force2D F) {
			//Calculate direction angle
			double gradient = (F.Direction.X) / (F.Direction.Y);
			double angle = Math.Atan(gradient);

			//Obtain components
			double Xcomponent = F.Newtons * Math.Cos(angle);
			double Ycomponent = F.Newtons * Math.Sin(angle);

			//Add to total acceleration
			linearAcceleration += new Vector3D(Xcomponent, Ycomponent, 0);
		}
	}
}
