using System;
using System.Collections;
using SlimDX;
using SlimDX.Direct3D9;

namespace PhysicsEngine {
	public class Vertex {
		public ArrayList e = new ArrayList();
		public ArrayList f = new ArrayList();

		public Vector3 pos;
		public Vector3 normal;

		public void Initialize() {
			// compute the interpolated normal
			foreach (Face g in f) {
				for (int i = 0; i < 3; i++) {
					if (g.v[i] == this) {
						// weight the contribution of each face's normal by the angle that face
						// subtends with respect this vertex
						if (g.e[i] != null && g.e[(i + 2) % 3] != null) // HACK to make holey meshes work
						{
							float angle = (float) Math.Acos(Math.Abs(Vector3.Dot(g.e[i].normal, g.e[(i + 2) % 3].normal)));
							normal += g.plane.Normal * angle;
						}
						break;
					}
				}
			}

			normal.Normalize();
		}

		public void Transform(Matrix m) {
			Vector3.TransformCoordinate(pos, m);
			Vector3.TransformNormal(normal, m);
		}
	}
}