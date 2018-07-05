using System.Collections.Generic;
using System.Drawing;
using System.Graphics.GL;
using System.Graphics.Models;
using System.Numerics.Vectors;

namespace System.Physics2D {
	public class PolygonParticle : Particle {
		private double currentRotationSpeed, rotation;
		private List<Particle> cornerParticles = new List<Particle>();

		public override double Rotation {
			get {
				return IsSpring ? springComponent.Rotation : rotation;
			}
			set {
				if (IsSpring)
					value = springComponent.Rotation;
				rotation = value;
				Axis = new Vector2D(Math.Cos(rotation), Math.Sin(rotation));
			}
		}

		public override Vector2D Radius {
			get {
				return new Vector2D(Vertex0.DistanceFrom(Vertex2) * 0.5);
			}
			set {
			}
		}

		internal Vector2D Axis {
			get;
			set;
		}

		public Vector2D Vertex0 {
			get {
				Vector2D axis = Axis;
				return Location - ((axis * Size.X) + (axis.PerpendicularLeft * Size.Y));
			}
		}

		public Vector2D Vertex1 {
			get {
				Vector2D axis = Axis;
				return Location + ((axis * Size.X) - (axis.PerpendicularLeft * Size.Y));
			}
		}

		public Vector2D Vertex2 {
			get {
				Vector2D axis = Axis;
				return Location + ((axis * Size.X) + (axis.PerpendicularLeft * Size.Y));
			}
		}

		public Vector2D Vertex3 {
			get {
				Vector2D axis = Axis;
				return Location - ((axis * Size.X) - (axis.PerpendicularLeft * Size.Y));
			}
		}

		public PolygonParticle(Vector2D location, Vector2D size, double mass, double elasticity, double friction, bool isFixed, bool isCollidable, bool isVisible, float z, int collisionFlag, int forceFlag, Texture fillTexture, Vector2D initialVelocity, double initialRotation = 0.0, double rotationalAcceleration = 0.0, double maxTorque = 0.0, double lifeSpan = 0.0, bool isCorner0Enabled = false, bool isCorner1Enabled = false, bool isCorner2Enabled = false, bool isCorner3Enabled = false) :
			base(location, size, mass, elasticity, friction, isFixed, isCollidable, z, collisionFlag, forceFlag, initialVelocity, initialRotation, rotationalAcceleration, maxTorque, lifeSpan) {
			IsCorner0Enabled = isCorner0Enabled;
			IsCorner1Enabled = isCorner1Enabled;
			IsCorner2Enabled = isCorner2Enabled;
			IsCorner3Enabled = isCorner3Enabled;
			v0 = new Vertex(new Vector3((Vector2) Vertex0, z), Vector2.Zero);
			v1 = new Vertex(new Vector3((Vector2) Vertex0, z), Vector2.UnitX);
			v2 = new Vertex(new Vector3((Vector2) Vertex0, z), Vector2.One);
			v3 = new Vertex(new Vector3((Vector2) Vertex0, z), Vector2.UnitY);
			Model = new MeshComponent(fillTexture, isFixed ? BufferUsageHint.StaticDraw : BufferUsageHint.StreamDraw, Vector3.Zero, isVisible, MeshComponent.TriangulateQuads(new Vertex[] { v0, v1, v2, v3 }), true);
			double rot = Rotation;
			Axis = new Vector2D(Math.Cos(rot), Math.Sin(rot));
		}

		public override object Clone() {
			return new PolygonParticle(Location, Size, Mass, Elasticity, Friction, IsFixed, IsCollidable, IsVisible, Z, CollisionFlag, ForceFlag, FillTexture, Velocity, Rotation, RotationalAcceleration, MaxTorque, LifeSpan, IsCorner0Enabled, IsCorner1Enabled, IsCorner2Enabled, IsCorner3Enabled);
		}

		/// <summary>
		/// Gets or sets whether the specified corner index is 
		/// </summary>
		/// <param name="index"></param>
		public bool this[int index] {
			get {
				return index < cornerParticles.Count Corner0 != null;
			}
			set {
				if (IsCorner0Enabled == value)
					return;
				else if (value)
					Corner0 = new ElliseParticle(Vertex0, Vector2D.Zero, 1.0, 0.0, 0.0, true, false, false, Z, CollisionFlag, ForceFlag, null, Vector2D.Zero);
				else
					Corner0 = null;
			}
		}

		public override void Render() {
			if (!IsVisible)
				return;
			else if (IsSpring) {
				Location = springComponent.Centre;
				if (springComponent.stretchToSize)
					Scale = new Drawing.Vector2(springComponent.CurrentLength * springComponent.WidthScale, Scale.Y);
				Rotation = springComponent.Rotation;
			}
			v0.Pos = new Vector3((Vector2) Vertex0, Z);
			if (v0.Pos != Model.BufferData[0].Pos) {
				Model.UpdateBuffer = true;
                Model.BufferData[0] = v0;
			}
			v1.Pos = new Vector3((Vector2) Vertex1, Z);
			if (v1.Pos != Model.BufferData[1].Pos) {
				Model.UpdateBuffer = true;
				Model.BufferData[1] = v1;
			}
			v2.Pos = new Vector3((Vector2) Vertex2, Z);
			if (v2.Pos != Model.BufferData[2].Pos) {
				Model.UpdateBuffer = true;
				Model.BufferData[2] = v2;
			}
			v3.Pos = new Vector3((Vector2) Vertex3, Z);
			if (v3.Pos != Model.BufferData[3].Pos) {
				Model.UpdateBuffer = true;
				Model.BufferData[3] = v3;
			}
			Model.Render();
		}

		public override void Update(double elapsed, double multiplier) {
			base.Update(elapsed, multiplier);
			currentRotationSpeed += DampenedTorque;
			if (currentRotationSpeed > MaxTorque)
				currentRotationSpeed = MaxTorque;
			else if (currentRotationSpeed < -MaxTorque)
				currentRotationSpeed = -MaxTorque;
			Rotation += currentRotationSpeed * multiplier;
			if (IsCorner0Enabled)
				Corner0.Location = Vertex0;
			if (IsCorner1Enabled)
				Corner1.Location = Vertex1;
			if (IsCorner2Enabled)
				Corner2.Location = Vertex2;
			if (IsCorner3Enabled)
				Corner3.Location = Vertex3;
		}

		public override Vector2D Project(Vector2D axis) {
			if (IsSpring)
				Rotation = springComponent.Rotation;
			Vector2D currentAxis = Axis;
			double radius = Size.X * Math.Abs(Vector2D.Dot(axis, currentAxis)) + Size.Y * Math.Abs(Vector2D.Dot(axis, currentAxis.PerpendicularLeft));
			double c = Vector2D.Dot(Location, axis);
			return new Vector2D(c - radius, c + radius);
		}
	}
}