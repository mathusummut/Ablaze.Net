using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace System.Drawing {
	public class PolyhedronVertex {
		public List<Face> f = new List<Face>();

		public Vector3 pos;
		public Vector3 normal;

		public void Initialize() {
			// compute the interpolated normal
			PolyhedronVertex[] vertices;
			Edge[] edges;
			foreach (Face g in f) {
				vertices = new PolyhedronVertex[] { g.v0, g.v1, g.v2 };
				edges = new Edge[] { g.e0, g.e1, g.e2 };
				for (int i = 0; i < 3; i++) {
					if (vertices[i] == this) {
						// weight the contribution of each face's normal by the angle that face
						// subtends with respect this vertex
						if (edges[i] != null && edges[(i + 2) % 3] != null) // HACK to make holey meshes work
						{
							float angle = (float) Math.Acos(Math.Abs(Vector3.Dot(edges[i].normal, edges[(i + 2) % 3].normal)));
							normal += g.plane.Normal * angle;
						}
						break;
					}
				}
			}

			normal = Vector3.Normalize(normal);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Transform(ref Matrix4 m) {
			pos = pos.TransformPosition(ref m);
			normal = normal.TransformNormal(ref m);
		}
	}
}