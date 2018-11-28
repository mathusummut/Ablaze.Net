using System.Drawing;
using System.Graphics.GL;
using System.Graphics.Models;

namespace System.Physics2D {
	public class ElliseParticle : Particle {
		private double previousRotation, rotation;
		private Vertex[] vertices;
		private Texture temp;
		private double increment;
		private int sides;
		public double Traction;
		public bool IsTextureFixed;

		public override double Rotation {
			get {
				return rotation;
			}
			set {
				value %= MathHelper.TwoPiD;
				if (value == rotation % MathHelper.TwoPiD)
					return;
				previousRotation %= MathHelper.TwoPiD;
				rotation = value;
				UpdateVertices();
			}
		}

		public override double Area {
			get {
				Vector2D radius = base.Radius;
				return Math.PI * radius.X * radius.Y;
			}
		}

		public override double Scale {
			get {
				return base.Scale;
			}
			set {
				Radius *= value / base.Scale;
				base.Scale = value;
			}
		}

		public override Vector2D Radius {
			get {
				return base.Radius;
			}
			set {
				if (value == base.Radius)
					return;
				base.Radius = value;
				UpdateVertices();
			}
		}

		public int RenderedSides {
			get {
				return sides;
			}
			set {
				if (value == sides)
					return;
				sides = value;
				increment = MathHelper.TwoPiD / sides;
				UpdateVertices();
				MeshComponent previousModel = Model;
				if (previousModel == null) {
					Model = new MeshComponent(temp, IsFixed ? BufferUsageHint.StaticDraw : BufferUsageHint.StreamDraw, Vector3.Zero, true, MeshComponent.TriangulatePolygon(vertices), true);
					temp = null;
				} else {
					Model = new MeshComponent(previousModel.Textures.Length == 0 ? null : previousModel.Textures[0], IsFixed ? BufferUsageHint.StaticDraw : BufferUsageHint.StreamDraw, Vector3.Zero, previousModel.IsVisible, MeshComponent.TriangulatePolygon(vertices), true);
					previousModel.Dispose();
				}
			}
		}

		public ElliseParticle(Vector2D location, Vector2D radius, double mass, double elasticity, double friction, bool isFixed, bool isCollidable, bool isVisible, float z, int collisionFlag, int forceFlag, Texture fillTexture, Vector2D initialVelocity, double initialRotation = 0.0, double rotationalAcceleration = 0.0, double maxTorque = 0.0, double traction = 0.0, double lifeSpan = 0.0, int renderedSides = 16)
			: base(location, radius, mass, elasticity, friction, isFixed, isCollidable, z, collisionFlag, forceFlag, initialVelocity, initialRotation, rotationalAcceleration, maxTorque, lifeSpan) {
			Radius = radius;
			rotation = initialRotation;
			previousRotation = initialRotation;
			Traction = traction;
			temp = fillTexture;
			RenderedSides = renderedSides;
			IsVisible = isVisible;
		}

		public override object Clone() {
			return new ElliseParticle(Location, Radius, Mass, Elasticity, Friction, IsFixed, IsCollidable, IsVisible, Z, CollisionFlag, ForceFlag, FillTexture, Velocity, Rotation, RotationalAcceleration, MaxTorque, Traction, LifeSpan, sides);
		}

		public override void Update(double elapsed, double multiplier) {
			base.Update(elapsed, multiplier);
			if (IsFixed)
				return;
			double oldRotation = rotation;
			double currentRotationSpeed = DampenedTorque + rotation - previousRotation;
			if (currentRotationSpeed > MaxTorque)
				currentRotationSpeed = MaxTorque;
			else if (currentRotationSpeed < -MaxTorque)
				currentRotationSpeed = -MaxTorque;
			rotation += currentRotationSpeed * multiplier;
			previousRotation = oldRotation;
			UpdateVertices();
		}

		public override Vector2D Project(Vector2D axis) {
			double c = Vector2D.Dot(Location, axis);
			double radius = Radius.X == Radius.Y ? Radius.X : Vector2D.GetPointOnEllipseFromAngle(Vector2D.Zero, Radius, rotation, axis.GetRotationFrom(Vector2D.Zero)).Length;
			return new Vector2D(c - radius, c + radius);
		}

		public override void SolveCollision(Vector2D movement, Vector2D vel, Vector2D normal) {
			base.SolveCollision(movement, vel, normal);
			SolveRotation(normal, Damping);
		}

		public override void Render() {
			if (!IsVisible)
				return;
			for (int i = 0; i < sides; i++) {
				if (vertices[i] != Model.BufferData[i]) {
					Model.UpdateBuffer = true;
					Model.BufferData[i] = vertices[i];
				}
			}
			Model.Render();
		}

		public void SolveRotation(Vector2D normal, double damping) {
			if (IsFixed)
				return;
			double slipSpeed = Traction * RotationalAcceleration * damping;
			Vector2D force = new Vector2D(slipSpeed * normal.Y, slipSpeed * normal.X);
			Velocity += force;
		}

		private void UpdateVertices() {
			Vector2D radius = Radius;
			double x = Location.X;
			double y = Location.Y;
			double rotation = this.rotation;
			double step;
			vertices = new Vertex[sides];
			int i;
			if (IsTextureFixed) {
				if (radius.X == radius.Y) {
					for (i = 0; i < sides; i++) {
						step = i * increment + rotation;
						vertices[i] = new Vertex(new Vector3((float) (x + Math.Cos(step) * radius.X), (float) (y + Math.Sin(step) * radius.Y), Z), new Vector2((float) (Math.Cos(step) * 0.5 + 0.5), (float) (Math.Sin(step) * 0.5 + 0.5)));
					}
				} else {
					for (i = 0; i < sides; i++) {
						step = i * increment + rotation;
						vertices[i] = new Vertex(new Vector3((Vector2) Vector2D.GetPointOnEllipseFromAngle(Location, radius, rotation, step), Z), new Vector2((float) (Math.Cos(step) * 0.5 + 0.5), (float) (Math.Sin(step) * 0.5 + 0.5)));
					}
				}
			} else {
				double stepPlusRotation;
				if (radius.X == radius.Y) {
					for (i = 0; i < sides; i++) {
						step = i * increment;
						stepPlusRotation = step + rotation;
						vertices[i] = new Vertex(new Vector3((float) (x + Math.Cos(stepPlusRotation) * radius.X), (float) (y + Math.Sin(stepPlusRotation) * radius.Y), Z), new Vector2((float) (Math.Cos(step) * 0.5 + 0.5), (float) (Math.Sin(step) * 0.5 + 0.5)));
					}
				} else {
					for (i = 0; i < sides; i++) {
						step = i * increment;
						stepPlusRotation = step + rotation;
						vertices[i] = new Vertex(new Vector3((Vector2) Vector2D.GetPointOnEllipseFromAngle(Location, radius, rotation, stepPlusRotation), Z), new Vector2((float) (Math.Cos(step) * 0.5 + 0.5), (float) (Math.Sin(step) * 0.5 + 0.5)));
					}
				}
			}
		}
	}
}