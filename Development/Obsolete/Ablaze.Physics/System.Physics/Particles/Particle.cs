using System.Collections.Generic;
using System.Drawing;
using System.Graphics.GL;
using System.Graphics.Models;

namespace System.Physics2D {
	public class Particle : ICloneable, IDisposable {
		private Vector2 previous;
		private bool isFixed;
		private double maxTorque, angularVelocity, elapsed;
		internal List<Cannon> cannonComponent = new List<Cannon>();
		internal List<Particle> toAdd = new List<Particle>();
		internal Fluid fluid;
		internal Spring springComponent;
		internal Lift liftComponent;
		private MeshComponent Model;
		public double Friction, Mass, Elasticity, LifeSpan;
		public bool IsCollidable;
		public int CollisionFlag, ForceFlag;

		public virtual Vector3 Location {
			get {
				return Model.Location;
			}
			set {
				Model.Location = value;
				Model.UpdateBuffer = true;
			}
		}

		public virtual Vector2 Scale {
			get {
				return Model.Scale.Xy;
			}
			set {
				Model.Scale = new Vector3(value, 1f);
				Model.UpdateBuffer = true;
			}
		}

		public virtual float Rotation {
			get {
				return Model.Rotation.Z;
			}
			set {
				Model.Rotation = new Vector3(Model.Rotation.Xy, value);
				Model.UpdateBuffer = true;
			}
		}

		public double MaxTorque {
			get {
				return maxTorque;
			}
			set {
				maxTorque = value;
				if (angularVelocity > maxTorque)
					angularVelocity = maxTorque;
				else if (angularVelocity < -maxTorque)
					angularVelocity = -maxTorque;
			}
		}

		public double Damping {
			get;
			internal set;
		}

		public double DampenedTorque {
			get {
				return angularVelocity * Damping;
			}
		}

		public double RotationalAcceleration {
			get {
				return angularVelocity;
			}
			set {
				if (value > maxTorque)
					value = maxTorque;
				else if (value < -maxTorque)
					value = -maxTorque;
				angularVelocity = value;
			}
		}

		public bool IsVisible {
			get {
				return Model == null ? false : Model.IsVisible;
			}
			set {
				if (Model != null)
					Model.IsVisible = value;
			}
		}

		public bool IsFixed {
			get {
				return isFixed;
			}
			set {
				isFixed = value;
				if (Model == null)
					return;
				else if (value)
					Model.Optimization = BufferUsageHint.StaticDraw;
				else
					Model.Optimization = BufferUsageHint.StreamDraw;
			}
		}

		public TextureWrapMode WrapMode {
			get {
				return Model == null ? TextureWrapMode.ClampToEdge : Model.WrapMode;
			}
			set {
				if (Model != null)
					Model.WrapMode = value;
			}
		}

		public Texture FillTexture {
			get {
				return Model == null || Model.Textures.Length == 0 ? Texture.Empty : Model.Textures[0];
			}
			set {
				if (!(Model == null || Model.Textures.Length == 0))
					Model.Textures[0] = value;
			}
		}

		public bool IsFluid {
			get {
				return fluid != null;
			}
		}

		public bool IsSpring {
			get {
				return springComponent != null;
			}
		}

		public bool IsCannon {
			get {
				return cannonComponent.Count != 0;
			}
		}

		public bool IsLift {
			get {
				return liftComponent != null;
			}
		}

		public bool IsDisposed {
			get {
				return Model == null ? true : Model.IsDisposed;
			}
		}

		public virtual Vector2D Radius {
			get;
			set;
		}

		public Vector2D Velocity {
			get {
				if (IsSpring)
					return springComponent.Velocity;
				else
					return Location - previous;
			}
			set {
				previous = Location - value;
			}
		}

		public Vector2D DampenedVelocity {
			get {
				return Velocity * Damping;
			}
		}

		public virtual double Area {
			get {
				return Size.X * Size.Y;
			}
		}

		public double Density {
			get {
				return Mass / Area;
			}
		}

		public Particle(Vector2D location, Vector2D size, double mass, double elasticity, double friction, bool isFixed, bool isCollidable, float z, int collisionFlag, int forceFlag, Vector2D initialVelocity, double initialRotation = 0.0, double rotationalAcceleration = 0.0, double maxTorque = int.MaxValue, double lifeSpan = 0.0) {
			Scale = 1f;
			Size = size;
			LifeSpan = lifeSpan;
			Z = z;
			CollisionFlag = collisionFlag;
			Location = location;
			Velocity = initialVelocity;
			this.isFixed = isFixed;
			Mass = mass;
			Elasticity = elasticity;
			Friction = friction;
			IsCollidable = isCollidable;
			ForceFlag = forceFlag;
			Rotation = initialRotation;
			this.maxTorque = maxTorque;
			RotationalAcceleration = rotationalAcceleration;
		}

		public virtual object Clone() {
			return new Particle(Location, Size, Mass, Elasticity, Friction, isFixed, IsCollidable, Z, CollisionFlag, ForceFlag, Velocity, Rotation, RotationalAcceleration, maxTorque, LifeSpan);
		}

		public void ApplyForce(Force f) {
			if (!(f.Flag == ForceFlag || f.Flag == 0 || ForceFlag == 0))
				return;
			if (f.IsDirectional)
				Velocity += f.IsRelativeToMass ? (f.Strength / Mass) * f.Direction : f.Strength * f.Direction;
			else {
				Vector2D toMove = f.IsTowardsPoint ? Location.DirectionTowards(f.Direction, f.Strength) : Location.DirectionAwayFrom(f.Direction, f.Strength);
				Velocity += f.IsRelativeToMass ? toMove / Mass : toMove;
			}
		}

		internal void OnSpringComponentChangedInternal() {
			OnSpringComponentChanged();
		}

		protected virtual void OnSpringComponentChanged() {
		}

		public virtual void Update(double elapsedMilliseconds, double multiplier) {
            if (IsLift)
				liftComponent.Update();
			for (int i = 0; i < cannonComponent.Count; i++)
				cannonComponent[i].Update(elapsedMilliseconds, multiplier);
			elapsed += elapsedMilliseconds;
			if (LifeSpan > 0.0 && elapsed >= LifeSpan) {
				elapsed -= LifeSpan;
				Dispose();
			} else {
				if (!IsFixed) {
					Vector2D toAdd = Velocity * Damping;
					Location += toAdd * multiplier;
					Velocity = toAdd;
				}
				if (IsSpring)
					springComponent.Update(multiplier);
			}
		}

		public virtual void SolveCollision(Vector2D movement, Vector2D vel, Vector2D normal) {
			if (IsSpring)
				springComponent.SolveCollision(movement, vel);
			else if (!isFixed) {
				Location += movement;
				Velocity = vel;
			}
		}

		public virtual Vector2D Project(Vector2D axis) {
			double c = Vector2D.Dot(Location, axis);
			Vector2D radius = Radius;
			return new Vector2D(c - radius.X, c + radius.Y);
		}

		public virtual void Render() {
			if (!(Model == null || Model.IsDisposed))
				Model.Render();
		}

		~Particle() {
			Dispose(true);
		}

		public virtual void Dispose() {
			Dispose(false);
		}

		public virtual void Dispose(bool disposeConnectedComponents) {
			if (IsDisposed)
				return;
			Model.Dispose();
			if (disposeConnectedComponents) {
				if (IsSpring)
					springComponent.Dispose(true);
				int i;
				if (IsCannon) {
					for (i = 0; i < cannonComponent.Count; i++)
						cannonComponent[i].Dispose(true);
				}
				for (i = 0; i < toAdd.Count; i++)
					toAdd[i].Dispose(true);
			}
			GC.SuppressFinalize(this);
		}
	}
}