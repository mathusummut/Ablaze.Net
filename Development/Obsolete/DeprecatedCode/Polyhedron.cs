using System.Collections.Generic;
using System.Graphics.Models;
using System.Numerics;

namespace System.Drawing {
	public class Polyhedron {
		public List<PolyhedronVertex> verts;
		public List<Edge> edges;
		public List<Face> faces;

		private const int X = 0;
		private const int Y = 1;
		private const int Z = 2;

		private int A;   /* alpha */
		private int B;   /* beta */
		private int C;   /* gamma */

		/* projection integrals */
		private float P1, Pa, Pb, Paa, Pab, Pbb, Paaa, Paab, Pabb, Pbbb;

		/* face integrals */
		private float Fa, Fb, Fc, Faa, Fbb, Fcc, Faaa, Fbbb, Fccc, Faab, Fbbc, Fcca;

		/* volume integrals */
		private float T0;
		private float[] T1 = new float[3];
		private float[] T2 = new float[3];
		private float[] TP = new float[3];

		public void Compute(float density, out float mass, out Vector3 r, out Matrix3 J) {
			ComputeVolumeIntegrals();

			mass = density * T0;

			/* compute center of mass */
			r.X = T1[X] / T0;
			r.Y = T1[Y] / T0;
			r.Z = T1[Z] / T0;

			/* compute inertia tensor */
			J = Matrix3.Zero;
			J.M11 = density * (T2[Y] + T2[Z]);
			J.M22 = density * (T2[Z] + T2[X]);
			J.M33 = density * (T2[X] + T2[Y]);
			J.M12 = J.M21 = -density * TP[X];
			J.M23 = J.M32 = -density * TP[Y];
			J.M31 = J.M13 = -density * TP[Z];

			/* translate inertia tensor to center of mass */
			J.M11 -= mass * (r.Y * r.Y + r.Z * r.Z);
			J.M22 -= mass * (r.Z * r.Z + r.X * r.X);
			J.M33 -= mass * (r.X * r.X + r.Y * r.Y);
			J.M12 = J.M21 += mass * r.X * r.Y;
			J.M23 = J.M32 += mass * r.Y * r.Z;
			J.M31 = J.M13 += mass * r.Z * r.X;
		}

		private void ComputeVolumeIntegrals() {
			T0 = T1[X] = T1[Y] = T1[Z]
				= T2[X] = T2[Y] = T2[Z]
				= TP[X] = TP[Y] = TP[Z] = 0;
			unsafe
			{
				float* norm = stackalloc float[3];
				Face f;
				for (int i = 0; i < faces.Count; i++) {
					f = faces[i];
					norm[X] = f.plane.Normal.X;
					norm[Y] = f.plane.Normal.Y;
					norm[Z] = f.plane.Normal.Z;

					float nx = Math.Abs(norm[X]);
					float ny = Math.Abs(norm[Y]);
					float nz = Math.Abs(norm[Z]);
					if (nx > ny && nx > nz)
						C = X;
					else
						C = (ny > nz) ? Y : Z;
					A = (C + 1) % 3;
					B = (A + 1) % 3;

					ComputeFaceIntegrals(f, norm);

					T0 += norm[X] * ((A == X) ? Fa : ((B == X) ? Fb : Fc));

					T1[A] += norm[A] * Faa;
					T1[B] += norm[B] * Fbb;
					T1[C] += norm[C] * Fcc;
					T2[A] += norm[A] * Faaa;
					T2[B] += norm[B] * Fbbb;
					T2[C] += norm[C] * Fccc;
					TP[A] += norm[A] * Faab;
					TP[B] += norm[B] * Fbbc;
					TP[C] += norm[C] * Fcca;
				}
			}

			T1[X] *= 0.5F;
			T1[Y] *= 0.5F;
			T1[Z] *= 0.5F;
			T2[X] /= 3F;
			T2[Y] /= 3F;
			T2[Z] /= 3F;
			TP[X] *= 0.5F;
			TP[Y] *= 0.5F;
			TP[Z] *= 0.5F;
		}

		private unsafe void ComputeFaceIntegrals(Face f, float* n) {
			float w = f.plane.D;
			float k1, k2, k3, k4;

			ComputeProjectionIntegrals(f);

			k1 = 1 / n[C];
			k2 = k1 * k1;
			k3 = k2 * k1;
			k4 = k3 * k1;

			Fa = k1 * Pa;
			Fb = k1 * Pb;
			Fc = -k2 * (n[A] * Pa + n[B] * Pb + w * P1);

			Faa = k1 * Paa;
			Fbb = k1 * Pbb;
			Fcc = k3 * (Maths.Square(n[A]) * Paa + 2 * n[A] * n[B] * Pab + Maths.Square(n[B]) * Pbb
				+ w * (2 * (n[A] * Pa + n[B] * Pb) + w * P1));

			Faaa = k1 * Paaa;
			Fbbb = k1 * Pbbb;
			Fccc = -k4 * (Maths.Cube(n[A]) * Paaa + 3 * Maths.Square(n[A]) * n[B] * Paab
				+ 3 * n[A] * Maths.Square(n[B]) * Pabb + Maths.Cube(n[B]) * Pbbb
				+ 3 * w * (Maths.Square(n[A]) * Paa + 2 * n[A] * n[B] * Pab + Maths.Square(n[B]) * Pbb)
				+ w * w * (3 * (n[A] * Pa + n[B] * Pb) + w * P1));

			Faab = k1 * Paab;
			Fbbc = -k2 * (n[A] * Pabb + n[B] * Pbbb + w * Pbb);
			Fcca = k3 * (Maths.Square(n[A]) * Paaa + 2 * n[A] * n[B] * Paab + Maths.Square(n[B]) * Pabb
				+ w * (2 * (n[A] * Paa + n[B] * Pab) + w * Pa));
		}

		private void ComputeProjectionIntegrals(Face f) {
			float a0, a1, da;
			float b0, b1, db;
			float a0_2, a0_3, a0_4, b0_2, b0_3, b0_4;
			float a1_2, a1_3, b1_2, b1_3;
			float C1, Ca, Caa, Caaa, Cb, Cbb, Cbbb;
			float Cab, Kab, Caab, Kaab, Cabb, Kabb;

			P1 = Pa = Pb = Paa = Pab = Pbb = Paaa = Paab = Pabb = Pbbb = 0f;

			float[][] v = new float[3][];
			v[0] = new float[3];
			v[1] = new float[3];
			v[2] = new float[3];

			v[0][0] = f.v0.pos.X;
			v[0][1] = f.v0.pos.Y;
			v[0][2] = f.v0.pos.Z;

			v[1][0] = f.v1.pos.X;
			v[1][1] = f.v1.pos.Y;
			v[1][2] = f.v1.pos.Z;

			v[2][0] = f.v2.pos.X;
			v[2][1] = f.v2.pos.Y;
			v[2][2] = f.v2.pos.Z;

			for (int i = 0; i < 3; i++) {
				a0 = v[i][A];
				b0 = v[i][B];
				a1 = v[(i + 1) % 3][A];
				b1 = v[(i + 1) % 3][B];
				da = a1 - a0;
				db = b1 - b0;
				a0_2 = a0 * a0;
				a0_3 = a0_2 * a0;
				a0_4 = a0_3 * a0;
				b0_2 = b0 * b0;
				b0_3 = b0_2 * b0;
				b0_4 = b0_3 * b0;
				a1_2 = a1 * a1;
				a1_3 = a1_2 * a1;
				b1_2 = b1 * b1;
				b1_3 = b1_2 * b1;

				C1 = a1 + a0;
				Ca = a1 * C1 + a0_2;
				Caa = a1 * Ca + a0_3;
				Caaa = a1 * Caa + a0_4;
				Cb = b1 * (b1 + b0) + b0_2;
				Cbb = b1 * Cb + b0_3;
				Cbbb = b1 * Cbb + b0_4;
				Cab = 3 * a1_2 + 2 * a1 * a0 + a0_2;
				Kab = a1_2 + 2 * a1 * a0 + 3 * a0_2;
				Caab = a0 * Cab + 4 * a1_3;
				Kaab = a1 * Kab + 4 * a0_3;
				Cabb = 4 * b1_3 + 3 * b1_2 * b0 + 2 * b1 * b0_2 + b0_3;
				Kabb = b1_3 + 2 * b1_2 * b0 + 3 * b1 * b0_2 + 4 * b0_3;

				P1 += db * C1;
				Pa += db * Ca;
				Paa += db * Caa;
				Paaa += db * Caaa;
				Pb += da * Cb;
				Pbb += da * Cbb;
				Pbbb += da * Cbbb;
				Pab += db * (b1 * Cab + b0 * Kab);
				Paab += db * (b1 * Caab + b0 * Kaab);
				Pabb += da * (a1 * Cabb + a0 * Kabb);
			}

			P1 *= 0.5F;
			Pa /= 6f;
			Paa /= 12f;
			Paaa *= 0.05F;
			Pb /= -6f;
			Pbb /= -12f;
			Pbbb *= -0.05f;
			Pab /= 24f;
			Paab /= 60f;
			Pabb /= -60f;
		}

		public Polyhedron(MeshComponent mesh) {
			Array faceList = mesh.IndexBuffer.Indices;

			verts = new List<PolyhedronVertex>();
			for (int i = 0; i < mesh.BufferData.Length; i++) {
				PolyhedronVertex v = new PolyhedronVertex();
				v.pos = mesh.BufferData[i].Pos;
				verts.Add(v);
			}

			faces = new List<Face>(mesh.Triangles);
			for (int i = 0; i < mesh.Triangles; i++) {
				faces.Add(new Face() {
					v0 = verts[Convert.ToInt32(faceList.GetValue(i * 3))],
					v1 = verts[Convert.ToInt32(faceList.GetValue(i * 3 + 1))],
					v2 = verts[Convert.ToInt32(faceList.GetValue(i * 3 + 2))]
				});
			}
			Initialize();
		}

		private void Initialize() {
			// backlink each vertex to all the faces it is in
			Face f;
			for (int i = 0; i < faces.Count; i++) {
				f = faces[i];
				f.v0.f.Add(f);
				f.v1.f.Add(f);
				f.v2.f.Add(f);
			}

			// clean the mesh of duplicate vertices
			// for speed would want to sort the vertices in some way
			for (int i = 0; i < verts.Count; i++) {
				for (int j = i + 1; j < verts.Count;) {
					PolyhedronVertex v1 = verts[i];
					PolyhedronVertex v2 = verts[j];
					if (v1.pos.X == v2.pos.X && v1.pos.Y == v2.pos.Y && v1.pos.Z == v2.pos.Z) {
						foreach (Face g in v2.f) {
							// replace v2 in face f with v1
							if (g.v0 == v2)
								g.v0 = v1;
							if (g.v1 == v2)
								g.v1 = v1;
							if (g.v2 == v2)
								g.v2 = v1;

							// add f to v1's list of faces
							v1.f.Add(g);
						}

						// eliminate v2 from the list of vertices
						verts.RemoveAt(j);
					} else {
						// only move on when didn't delete something
						j++;
					}
				}
			}

			// identify all edges
			edges = new List<Edge>();
			int numMissingEdges = 0;
			PolyhedronVertex[] vertices;
			Edge[] currentEdges;
			foreach (Face f1 in faces) {
				vertices = new PolyhedronVertex[] { f1.v0, f1.v1, f1.v2 };
				currentEdges = new Edge[] { f1.e0, f1.e1, f1.e2 };
				for (int i = 0; i < 3; i++) {
					// check if edge already created
					if (currentEdges[i] != null)
						continue;

					foreach (Face f2 in vertices[i].f) {
						if (f1 == f2)
							continue;

						for (int j = 0; j < 3; j++) {
							if (vertices[i] == vertices[(j + 1) % 3] && vertices[(i + 1) % 3] == vertices[j]) {
								Edge e = new Edge() {
									v0 = vertices[i],
									v1 = vertices[(i + 1) % 3],
									f0 = f1,
									f1 = f2
								};
								switch (i) {
									case 0:
										f1.e0 = e;
										break;
									case 1:
										f1.e1 = e;
										break;
									case 2:
										f1.e2 = e;
										break;
									case 3:
										f1.e3 = e;
										break;
								}
								switch (j) {
									case 0:
										f1.e0 = e;
										break;
									case 1:
										f1.e1 = e;
										break;
									case 2:
										f1.e2 = e;
										break;
									case 3:
										f1.e3 = e;
										break;
								}
								edges.Add(e);
							}
						}
					}

					// make sure edge was found
					if (currentEdges[i] == null) {
						numMissingEdges++;
					}
				}
			}

			//            if (numMissingEdges > 0)
			//                throw new Exception("Unable to construct polyhedron b/c of hole in mesh: " + numMissingEdges + " missing edges\n");
			//
			foreach (Face t in faces) {
				t.Initialize();
			}

			foreach (Edge e in edges) {
				e.Initialize();
			}

			foreach (PolyhedronVertex v in verts) {
				v.Initialize();
			}
		}

		public void Transform(ref Matrix4 m) {
			foreach (PolyhedronVertex v in verts)
				v.Transform(ref m);
			foreach (Face f in faces)
				f.Transform(ref m);
			foreach (Edge e in edges)
				e.Transform(ref m);
		}

		// determines the squared distance from the given point to this polyhedron
		// also gives the closest point on the polyhedron and the interpolated normal at the point
		public void SqrDistance(Vector3 pt, out float sqrDist, out Vector3 closestPt, out Vector3 normal) {
			float bestDist = float.PositiveInfinity;
			float bestS = 0, bestT = 0;
			Face bestFace = null;

			foreach (Face curFace in faces) {
				float curDist;
				float curS, curT;

				curFace.SqrDistance(pt, out curDist, out curS, out curT);
				if (curDist < bestDist) {
					bestDist = curDist;
					bestS = curS;
					bestT = curT;
					bestFace = curFace;
				}
			}

			sqrDist = bestDist;
			closestPt = bestFace.PointFromST(bestS, bestT);
			normal = bestFace.NormalFromST(bestS, bestT);
		}
	}
}