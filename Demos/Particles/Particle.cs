using System;
using System.Drawing;
using System.Graphics.Models;
using System.Numerics;

namespace Particles {
	public class Particle : MeshComponent {
		public Particle(MeshComponent component) : base(component) {
			component.Dispose();
		}

		public void Update(Particle[] particles) {
			Vector3 dist;
			bool retry;
			do {
				retry = false;
				float length;
				for (int i = 0; i < particles.Length; i++) {
					dist = Location - particles[i].Location;
					length = dist.LengthSquared();
					if (length < 0.005f) {
						Location = new Vector3(((float) UniformRandom.RandomDouble) * 13.333f - 6.667f, ((float) UniformRandom.RandomDouble) * 10f - 5f, 0f);
						retry = true;
					}
					Location = new Vector3(ParticleManager.Approach(Location.ToVector2(), particles[i].Location.ToVector2(), 0.005f, -0.01f, 0.5f), 0f);
				}
			} while (retry);
			Location = new Vector3(Vector2.Clamp(Location.ToVector2(), new Vector2(-1, -1), new Vector2(1, 1)), 0f);
		}

		public void Render2D(Graphics g, Size canvasSize) {
			Bitmap image = ((Texture2D) Textures[0]).Image;
			g.DrawImageUnscaled(image, new Point((int) ((Location.X * 0.5f + 0.5f) * canvasSize.Width) - image.Width / 2, (int) ((Location.Y * 0.5f + 0.5f) * canvasSize.Height) - image.Height / 2));
		}
	}
}