using System.Drawing;
using System.Graphics.Models;

namespace System.Physics2D {
	public class Spring : IDisposable {
		private double maxCompression, maxStretch;
		internal bool stretchToSize;
		public readonly Particle Component, P1, P2;
		public bool IsCompressible, IsStretchable;
		public double Softness, RestLength, WidthScale;

		public double MaxCompression {
			get {
				return maxCompression;
			}
			set {
				if (value > maxStretch)
					value = maxStretch;
				maxCompression = value;
			}
		}
		
		public double MaxStretch {
			get {
				return maxStretch;
			}
			set {
				if (value < maxCompression)
					value = maxCompression;
				maxStretch = value;
			}
		}

		/// <summary>
		/// Whether the spring resizes when elastically stretched or compressed.
		/// </summary>
		public bool StretchToSize {
			get {
				return stretchToSize;
			}
			set {
				stretchToSize = value;
				if (!stretchToSize)
					Component.Size.X = RestLength * WidthScale;
			}
		}
		/// <summary>
		/// Gets the centre of the spring.
		/// </summary>
		public Vector2D Centre {
			get {
				return (P1.Location + P2.Location) * 0.5;
			}
		}

		/// <summary>
		/// Gets the current length of the spring.
		/// </summary>
		public double CurrentLength {
			get {
				return P1.Location.DistanceFrom(P2.Location);
			}
		}

		public double Rotation {
			get {
				return P1.Location.GetRotationFrom(P2.Location);
			}
		}

		public Vector2D Velocity {
			get {
				return (P1.Velocity + P2.Velocity) * 0.5;
			}
		}

		public Spring(Particle p1, Particle p2, Vector2D size, double softness, double mass, double elasticity, double friction, bool isCollidable, bool isVisible, float z, int collisionFlag, int forceFlag, Texture fillTexture, Vector2D initialVelocity, double lifeSpan, double maxCompression, double maxStretch, bool stretchToSize, bool isCompressible, bool isStretchable) {
			this.maxStretch = maxStretch;
			MaxCompression = maxCompression;
			IsCompressible = isCompressible;
			IsStretchable = isStretchable;
			Softness = softness;
			P1 = p1;
			P2 = p2;
			RestLength = p1.Location.DistanceFrom(p2.Location);
			WidthScale = size.X;
			StretchToSize = stretchToSize;
			Component = new PolygonParticle(Centre, size, mass, elasticity, friction, false, isCollidable, isVisible, z, collisionFlag, forceFlag, fillTexture, initialVelocity, (float) Math.Atan2(p1.Location.Y - p2.Location.Y, p1.Location.X - p2.Location.X), 0f, 0f, lifeSpan);
			Component.springComponent = this;
			Component.OnSpringComponentChangedInternal();
		}

		public static implicit operator Particle(Spring s) {
			return s.Component;
		}

		/// <summary>
		/// Returns whether the spring is connected to the specified particle.
		/// </summary>
		/// <param name="p"></param>
		public bool IsConnectedTo(Particle p) {
			return p == P1 || p == P2;
		}

		public void SolveCollision(Vector2D movement, Vector2D vel) {
			if (!P1.IsFixed) {
				P1.Location += movement;
				P1.Velocity = vel;
			}
			if (!P2.IsFixed) {
				P2.Location += movement;
				P2.Velocity = vel;
			}
		}

		public void Update(double multiplier) {
			double DeltaLength = CurrentLength;
			if ((DeltaLength <= RestLength && !IsCompressible) || (DeltaLength >= RestLength && !IsStretchable))
				return;
			Vector2D p1Location = P1.Location, p2Location = P2.Location;
			Vector2D dmd = (p1Location - p2Location) * ((DeltaLength - RestLength) / DeltaLength);
			double invM1 = 1.0 / P1.Mass;
			double invM2 = 1.0 / P2.Mass;
			double sumInvMass = invM1 + invM2 + Softness;
			if (!P1.IsFixed)
				p1Location -= dmd * (invM1 / sumInvMass);
			if (!P2.IsFixed)
				p2Location += dmd * (invM2 / sumInvMass);
			if (CurrentLength < maxCompression) {
				DeltaLength = CurrentLength;
				dmd = (p1Location - p2Location) * ((DeltaLength - maxCompression) / DeltaLength);
				if (!P1.IsFixed)
					p1Location -= dmd;
				if (!P2.IsFixed)
					p2Location += dmd;
			} else if (CurrentLength > maxStretch) {
				DeltaLength = CurrentLength;
				dmd = (p1Location - p2Location) * ((DeltaLength - maxStretch) / DeltaLength);
				if (!P1.IsFixed)
					p1Location -= dmd;
				if (!P2.IsFixed)
					p2Location += dmd;
			}
			if (!P1.IsFixed)
				P1.Location += (p1Location - P1.Location) * multiplier;
			if (!P2.IsFixed)
				P2.Location += (p2Location - P2.Location) * multiplier;
		}

		~Spring() {
			Dispose(true);
		}

		public void Dispose() {
			Dispose(false);
		}

		public void Dispose(bool disposeConnectedPartricles) {
			Component.Dispose();
			if (disposeConnectedPartricles) {
				P1.Dispose();
				P2.Dispose();
			} else {
				P1.springComponent = null;
				P2.springComponent = null;
				P1.OnSpringComponentChangedInternal();
				P2.OnSpringComponentChangedInternal();
			}
			GC.SuppressFinalize(this);
		}
	}
}