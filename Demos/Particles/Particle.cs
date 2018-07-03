using System;
using System.Graphics.Models;
using System.Numerics;

namespace Particles {
	public class Particle : MeshComponent {
		public Particle(MeshComponent component) : base(component) {
			component.Dispose();
		}

		public void Update(Particle[] particles) {
			Vector3 dist;
			//System.Random random = new System.Random();
			//do {
			//retry = false;
			float length;
			for (int i = 0; i < particles.Length; i++) {
				dist = Location - particles[i].Location;
				length = dist.LengthSquared();
				if (length < 0.1f) {
					Location += new Vector3(Vector2.Normalize(Vector2.One), 0f);
					//Position = new Vector2(((float) random.NextDouble()) * 13.333f - 6.667f, ((float) random.NextDouble()) * 10f - 5f);
					//retry = true;
				}
				//	Position = ParticleManager.Approach(Position, particles[i].Position, 0.5f, -0.01f, 0.5f);
			}
			//} while (retry);
			Location = new Vector3(Vector2.Clamp(Location.ToVector2(), new Vector2(-1, -1), new Vector2(1, 1)), 0f);
		}
	}
}