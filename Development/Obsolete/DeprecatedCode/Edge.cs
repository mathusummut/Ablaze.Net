using System.Numerics;
using System.Runtime.CompilerServices;

namespace System.Drawing {
	public class Edge {
		public PolyhedronVertex v0, v1;
		public Face f0, f1;
		public Vector3 normal;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Initialize() {
			normal = Vector3.Normalize(v0.pos - v1.pos);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Transform(ref Matrix4 m) {
			normal = normal.TransformNormal(ref m);
		}
	}
}