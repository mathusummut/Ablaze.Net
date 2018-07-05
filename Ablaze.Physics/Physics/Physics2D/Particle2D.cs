using System.Windows;

namespace Ablaze.Physics.System {
	class Particle2D {
		private float translationalInertia; //Resistance to change in translational momentum
		private float rotationalInertia; //Resistance to change in rotational momentum

		private translationalForce2D translationalForce; //The translational force acting on particle
		private rotationalForce2D rotationalForce; //The rotational force acting on particle

		private float mass; //Mass of particle
		private float acceleration; //Acceleration of particle
		private Vector centre; //Centroid of particle

	}
}
