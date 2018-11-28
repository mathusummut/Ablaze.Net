using System.Drawing;

namespace System.Physics2D {
	public class Cannon : IDisposable {
		private double elapsed;
		public delegate void FireHandler(Cannon sender);
		public readonly Particle Component;
		/// <summary>
		/// The particle like which all projectiles of the cannon will be.
		/// </summary>
		public Particle Projectile;
		public Vector2D Offset;
		/// <summary>
		/// The direction at which to shoot.
		/// </summary>
		public Vector2D Direction;
		public double Strength, ShootInterval;
		public event FireHandler Fire;

		public Cannon(Particle p, Particle projectile, Vector2D direction, double strength, double shootInterval, Vector2D offset) {
			Direction = direction;
			Strength = strength;
			Projectile = projectile;
			ShootInterval = shootInterval;
			Offset = offset;
			Component = p;
			p.cannonComponent.Add(this);
		}

		public static implicit operator Particle(Cannon c) {
			return c.Component;
		}

		public void Update(double elapsedMilliseconds, double multiplier) {
			if (ShootInterval == 0.0)
				return;
			elapsed += elapsedMilliseconds;
			if (elapsed >= ShootInterval) {
				elapsed -= ShootInterval;
				Particle p = (Particle) Projectile.Clone();
				p.Location = Component.Location + Offset;
				Vector2D force = Direction * Strength;
				p.Velocity = force / p.Mass;
				Component.Velocity += -force * multiplier / Component.Mass;
				Component.toAdd.Add(p);
				FireHandler handler = Fire;
				if (handler != null)
					handler(this);
			}
		}

		~Cannon() {
			Dispose(true);
		}

		public void Dispose() {
			Dispose(false);
		}

		public void Dispose(bool disposeProjectile) {
			Component.Dispose();
			if (Projectile != null)
				Projectile.Dispose();
			GC.SuppressFinalize(this);
		}
	}
}