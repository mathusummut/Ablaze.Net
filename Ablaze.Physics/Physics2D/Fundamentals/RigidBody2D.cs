using System.Windows.Media.Media3D;
using System.Collections.Generic;
using Ablaze.Physics.Physics2D.Exceptions;

namespace Ablaze.Physics.System {
	class RigidBody2D : Particle2D {
		//A collection of minimal particles necessary to form a boundary
		//Susceptible only to rotational and linear 2D forces
		//Not affected by Force2D - that is reserved for particles only

		//How it will work : Several particles can be added/removed from this rigidbody
		//In doing so the center and mass update to keep calculations correct
		//The particles will ideally be placed at the same position as the vertices of the models

		//Fundamental Values
		private double angularAcceleration; //dw/dt
		private double angularFrequency; //Revolutions/s
		private List<Particle2D> boundary;

		//Momentum Values
		private double angularMomentum;
		

		public RigidBody2D(Vector3D c, List<Particle2D> b) : base() {
			center = new Vector3D(0, 0, 0);

			for (int i = 0; i < boundary.Count; i++) {
				mass += b[i].Mass;
				center += b[i].Center;
			}

			if (mass <= 0)
				throw new MassOverflow();

			//Calculate center, and offset by c
			center /= b.Count;
			center += c;
		}

		public void applyRotationalForce(Force2D F, double distance, bool clockwise) {
			if (clockwise) {
				angularAcceleration += F.Newtons / (mass * distance); //F = ma = mr alpha
			} else {
				angularAcceleration -= F.Newtons / (mass * distance);
			}
		}
	}
}
