using System;
using System.Collections.Generic;
using SlimDX;
using SlimDX.Direct3D9;

namespace PhysicsEngine {
	public class RigidBody : IDisposable {
		public string Name;
		private RigidBodyType type;
		private float bounciness;
		private float friction;
		private Vector3 pos;
		public Vector3 Vel;
		private Quaternion orientation;
		public Vector3 Rotation;
		private bool moved;
		private Matrix3 orientationMatrix;
		private Matrix3 inverseOrientationMatrix;
		private Matrix3 inverseWorldInertiaMatrix;
		private float mass;
		private Matrix3 inverseInertiaTensor;
		private Matrix3 inverseMassMatrix;

		public int level;
		public bool levelSet;
		public List<CollisionPair> collisions;

		// used in shock propagation
		public bool frozen;

		public Vector3 Pos {
			get {
				return pos;
			}
			set {
				pos = value;
				moved = true;
			}
		}

		public Quaternion Orientation {
			get {
				return orientation;
			}
			set {
				// XXX inefficient (but safer) to renormalize every time
				if (value.LengthSquared() == 0)
					orientation = Quaternion.Identity;
				else
					orientation = Quaternion.Normalize(value);
				moved = true;
			}
		}

		public Matrix3 OrientationMatrix {
			get {
				if (moved)
					RebuildMatrices();
				return orientationMatrix;
			}
		}

		public Matrix3 InverseOrientationMatrix {
			get {
				if (moved)
					RebuildMatrices();
				return inverseOrientationMatrix;
			}
		}

		public Matrix3 InverseWorldInertiaMatrix {
			get {
				if (moved)
					RebuildMatrices();
				return inverseWorldInertiaMatrix;
			}
		}

		private void RebuildMatrices() {
			moved = false;
			orientationMatrix = FromMatrix4(Matrix.RotationQuaternion(orientation));
			inverseOrientationMatrix = orientationMatrix.Transpose();
			inverseWorldInertiaMatrix = orientationMatrix * inverseInertiaTensor * inverseOrientationMatrix;
		}

		/* computes the velocity in world space of the
                 * point x in object space */
		public Vector3 WorldVelocity(Vector3 x) {
			// v_world = v + cross(omega, R*x_body)
			return Vel + Vector3.Cross(Rotation, x - Pos);
		}

		public Vector3 PointWorldToBody(Vector3 p) {
			return InverseOrientationMatrix * (p - Pos);
		}

		public Vector3 PointBodyToWorld(Vector3 p) {
			return OrientationMatrix * p + Pos;
		}

		public Vector3 VectorWorldToBody(Vector3 v) {
			return InverseOrientationMatrix * v;
		}

		public Vector3 VectorBodyToWorld(Vector3 v) {
			return OrientationMatrix * v;
		}

		public RigidBody(RigidBodyType type, Vector3 pos, Vector3 vel, Quaternion orientation, Vector3 rotation, float bounciness, float friction, float density) {
			this.type = type;
			Pos = pos;
			Vel = vel;
			Orientation = orientation;
			Rotation = rotation;
			this.bounciness = bounciness;
			this.friction = friction;
			this.collisions = new List<CollisionPair>();
			this.mass = density * type.volume;
			Matrix3 inertiaTensor = density * type.inertiaTensor;
			inertiaTensor.Invert(out inverseInertiaTensor);
			inverseMassMatrix = Matrix3.Identity * (1 / mass);
		}

		public void SetState(State state) {
			Pos = state.pos;
			Orientation = state.orientation;
			Vel = state.vel;
			Rotation = state.rotation;
		}

		public RigidBodyType Type {
			get {
				return type;
			}
		}

		public float Bounciness {
			get {
				return bounciness;
			}
		}

		public float Friction {
			get {
				return friction;
			}
		}

		public float Mass {
			get {
				return mass;
			}
		}

		// evaluates the SDM at point p in world space
		public CollisionPoint EvaluateSDM(Vector3 p, bool compute_normal) {
			return type.sdm.Evaluate(p, compute_normal);
		}

		/* computes the K matrix given a point x in world space
             * (used in collision response) */
		public Matrix3 ComputeK(Vector3 x) {
			if (frozen || mass == float.PositiveInfinity)
				return Matrix3.Zero;
			x -= Pos;
			Matrix3 Rx = Matrix3.CrossProductMatrix(ref x);
			return inverseMassMatrix - Rx * InverseWorldInertiaMatrix * Rx;
		}

		// applies an impulse J at point x (both in world coords) to the body
		public void ApplyImpulse(Vector3 x, Vector3 J) {
			if (frozen || mass == float.PositiveInfinity)
				return;

			Vector3 Rx = x - Pos;
			Vector3 vOld = Vel;
			Vector3 vNew = vOld + J * (1 / mass);
			Vector3 wOld = Rotation;
			Vector3 wNew = wOld + InverseWorldInertiaMatrix * Vector3.Cross(Rx, J);

			Vel = vNew;
			Rotation = wNew;
		}

		public void Render(Device device) {
			device.SetTransform(TransformState.World, ToMatrix4(OrientationMatrix) * Matrix.Translation(Pos));
			type.Render(device);
		}

		private static Matrix ToMatrix4(Matrix3 m3) {
			Matrix m4 = Matrix.Identity;
			m4.M11 = m3.M11;
			m4.M21 = m3.M12;
			m4.M31 = m3.M13;
			m4.M12 = m3.M21;
			m4.M22 = m3.M22;
			m4.M32 = m3.M23;
			m4.M13 = m3.M31;
			m4.M23 = m3.M32;
			m4.M33 = m3.M33;
			return m4;
		}

		private static Matrix3 FromMatrix4(Matrix m4) {
			Matrix3 m3 = Matrix3.Zero;
			m3.M11 = m4.M11;
			m3.M21 = m4.M12;
			m3.M31 = m4.M13;
			m3.M12 = m4.M21;
			m3.M22 = m4.M22;
			m3.M32 = m4.M23;
			m3.M13 = m4.M31;
			m3.M23 = m4.M32;
			m3.M33 = m4.M33;
			return m3;
		}

		public void UpdatePos(float dt) {
			/* beware of NaNs and infinity */
			Pos += Vel * dt;
			float rotSpeed = Rotation.Length();
			if (rotSpeed != 0)
				Orientation *= Quaternion.RotationAxis(Rotation * (1 / rotSpeed), rotSpeed * dt);
		}

		public void UpdateVel(float dt) {
			// integrate forces over dt

			// gravity (for non-infinite objects
			if (mass != float.PositiveInfinity)
				Vel += dt * new Vector3(0, -9.8f, 0);
		}

		~RigidBody() {
			Dispose();
		}

		public void Dispose() {
			type.Dispose();
			GC.SuppressFinalize(this);
		}
	}
}