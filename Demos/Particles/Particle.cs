using System;
using System.Drawing;
using System.Graphics.Models;
using System.Numerics;

namespace Particles {
	public class Particle : MeshComponent {
		public Particle(Texture2D texture, Vector2 location, Vector2 size) : base(texture, MeshExtensions.TriangulateQuads(new Vertex[] {
			new Vertex(new Vector3(location, 0f), Vector2.UnitY),
			new Vertex(new Vector3(location.X + size.X, location.Y, 0f), Vector2.One),
			new Vertex(new Vector3(location.X + size.X, location.Y + size.Y, 0f), Vector2.UnitX),
			new Vertex(new Vector3(location.X, location.Y + size.Y, 0f), Vector2.Zero)
		}), false) {
			LowOpacity = true;
		}

		public void Update(Particle[] particles) {
			Vector3 dist;
			for (int i = 0; i < particles.Length; i++) {
				dist = Location - particles[i].Location;
				if (dist.LengthSquared() < 0.03f)
					Location += Vector3.Normalize(new Vector3(UniformRandom.RandomFloat, UniformRandom.RandomFloat, UniformRandom.RandomFloat)) * 2f;
			}
			Location = new Vector3(Vector2.Clamp(Location.ToVector2(), new Vector2(-1, -1), new Vector2(1, 1)), 0f);
		}

		public void Render2D(Graphics g, Size canvasSize) {
			Bitmap image = ((Texture2D) Textures[0]).Image;
			g.DrawImageUnscaled(image, new Point((int) ((Location.X * 0.5f + 0.5f) * canvasSize.Width) - image.Width / 2, (int) ((Location.Y * 0.5f + 0.5f) * canvasSize.Height) - image.Height / 2));
		}
	}
}