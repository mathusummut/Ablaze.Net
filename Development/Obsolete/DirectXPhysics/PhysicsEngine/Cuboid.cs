using System;
using SlimDX;
using SlimDX.Direct3D9;

namespace PhysicsEngine {
	/// <summary>
	/// Summary description for AABB.
	/// </summary>
	public class Cuboid {
		public Vector3 p0, p1;

		public Cuboid() {
			p0 = new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);
			p1 = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
		}

		public void Expand(Vector3 p) {
			p0.X = Math.Max(p0.X, p.X);
			p0.Y = Math.Max(p0.Y, p.Y);
			p0.Z = Math.Max(p0.Z, p.Z);
			p1.X = Math.Min(p1.X, p.X);
			p1.Y = Math.Min(p1.Y, p.Y);
			p1.Z = Math.Min(p1.Z, p.Z);
		}

		public bool Contains(Vector3 p) {
			return
				p0.X >= p.X &&
				p0.Y >= p.Y &&
				p0.Z >= p.Z &&
				p1.X <= p.X &&
				p1.Y <= p.Y &&
				p1.Z <= p.Z;
		}
	}
}