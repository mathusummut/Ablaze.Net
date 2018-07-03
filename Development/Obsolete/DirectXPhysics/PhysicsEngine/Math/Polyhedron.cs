using System;
using System.Collections;
using SlimDX;
using SlimDX.Direct3D9;
using System.Runtime.InteropServices;

namespace PhysicsEngine {
	public class Polyhedron {
		public ArrayList verts;
		public ArrayList edges;
		public ArrayList faces;

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

		private static float SQR(float x) {
			return x * x;
		}
		private static float CUBE(float x) {
			return x * x * x;
		}

		public void Compute(float density, out float mass, out Vector3 r, out Matrix3 J) {
			ComputeVolumeIntegrals();

			//                Console.WriteLine("T0 = " + T0);
			//                Console.WriteLine("T1 = ({0},{1},{2})", T1[0], T1[1], T1[2]);
			//                Console.WriteLine("T2 = ({0},{1},{2})", T2[0], T2[1], T2[2]);
			//                Console.WriteLine("TP = ({0},{1},{2})", TP[0], TP[1], TP[2]);

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

			float[] norm = new float[3];
			foreach (Face f in faces) {
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

			T1[X] /= 2;
			T1[Y] /= 2;
			T1[Z] /= 2;
			T2[X] /= 3;
			T2[Y] /= 3;
			T2[Z] /= 3;
			TP[X] /= 2;
			TP[Y] /= 2;
			TP[Z] /= 2;
		}

		private void ComputeFaceIntegrals(Face f, float[] n) {
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
			Fcc = k3 * (SQR(n[A]) * Paa + 2 * n[A] * n[B] * Pab + SQR(n[B]) * Pbb
				+ w * (2 * (n[A] * Pa + n[B] * Pb) + w * P1));

			Faaa = k1 * Paaa;
			Fbbb = k1 * Pbbb;
			Fccc = -k4 * (CUBE(n[A]) * Paaa + 3 * SQR(n[A]) * n[B] * Paab
				+ 3 * n[A] * SQR(n[B]) * Pabb + CUBE(n[B]) * Pbbb
				+ 3 * w * (SQR(n[A]) * Paa + 2 * n[A] * n[B] * Pab + SQR(n[B]) * Pbb)
				+ w * w * (3 * (n[A] * Pa + n[B] * Pb) + w * P1));

			Faab = k1 * Paab;
			Fbbc = -k2 * (n[A] * Pabb + n[B] * Pbbb + w * Pbb);
			Fcca = k3 * (SQR(n[A]) * Paaa + 2 * n[A] * n[B] * Paab + SQR(n[B]) * Pabb
				+ w * (2 * (n[A] * Paa + n[B] * Pab) + w * Pa));
		}

		private void ComputeProjectionIntegrals(Face f) {
			float a0, a1, da;
			float b0, b1, db;
			float a0_2, a0_3, a0_4, b0_2, b0_3, b0_4;
			float a1_2, a1_3, b1_2, b1_3;
			float C1, Ca, Caa, Caaa, Cb, Cbb, Cbbb;
			float Cab, Kab, Caab, Kaab, Cabb, Kabb;

			P1 = Pa = Pb = Paa = Pab = Pbb = Paaa = Paab = Pabb = Pbbb = 0.0f;

			float[,] v = new float[3, 3];
			for (int i = 0; i < 3; i++) {
				v[i, 0] = f.v[i].pos.X;
				v[i, 1] = f.v[i].pos.Y;
				v[i, 2] = f.v[i].pos.Z;
			}
			for (int i = 0; i < 3; i++) {
				a0 = v[i, A];
				b0 = v[i, B];
				a1 = v[(i + 1) % 3, A];
				b1 = v[(i + 1) % 3, B];
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

			P1 /= 2.0f;
			Pa /= 6.0f;
			Paa /= 12.0f;
			Paaa /= 20.0f;
			Pb /= -6.0f;
			Pbb /= -12.0f;
			Pbbb /= -20.0f;
			Pab /= 24.0f;
			Paab /= 60.0f;
			Pabb /= -60.0f;
		}

		public Polyhedron(Mesh mesh) {
			/* clone the mesh so as to get at it's faces and vertices */
			using (Mesh tempMesh = mesh.Clone(mesh.Device, MeshFlags.Managed, VertexFormat.Position)) {
				PositionOnly[] vertList = tempMesh.LockVertexBuffer(LockFlags.ReadOnly).ReadRange<PositionOnly>(tempMesh.VertexCount);
				short[] faceList = tempMesh.LockIndexBuffer(LockFlags.ReadOnly).ReadRange<short>(tempMesh.FaceCount * 3);

				verts = new ArrayList();
				for (int i = 0; i < tempMesh.VertexCount; i++) {
					Vertex v = new Vertex();
					v.pos = vertList[i].Position;
					verts.Add(v);
				}

				faces = new ArrayList();
				for (int i = 0; i < tempMesh.FaceCount; i++) {
					Face f = new Face();
					for (int j = 0; j < 3; j++)
						f.v[j] = (Vertex) verts[faceList[i * 3 + j]];
					faces.Add(f);
				}

				tempMesh.UnlockVertexBuffer();
				tempMesh.UnlockIndexBuffer();
			}

			Initialize();
		}

		private void Initialize() {
			// backlink each vertex to all the faces it is in
			foreach (Face f in faces)
				foreach (Vertex v in f.v)
					v.f.Add(f);

			// clean the mesh of duplicate vertices
			// for speed would want to sort the vertices in some way
			for (int i = 0; i < verts.Count; i++) {
				for (int j = i + 1; j < verts.Count;) {
					Vertex v1 = (Vertex) verts[i];
					Vertex v2 = (Vertex) verts[j];
					if (v1.pos.X == v2.pos.X && v1.pos.Y == v2.pos.Y && v1.pos.Z == v2.pos.Z) {
						foreach (Face f in v2.f) {
							// replace v2 in face f with v1
							for (int k = 0; k < 3; k++)
								if (f.v[k] == v2)
									f.v[k] = v1;

							// add f to v1's list of faces
							v1.f.Add(f);
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
			edges = new ArrayList();
			int numMissingEdges = 0;
			foreach (Face f1 in faces) {
				for (int i = 0; i < 3; i++) {
					// check if edge already created
					if (f1.e[i] != null)
						continue;

					foreach (Face f2 in f1.v[i].f) {
						if (f1 == f2)
							continue;

						for (int j = 0; j < 3; j++) {
							if (f1.v[i] == f2.v[(j + 1) % 3] && f1.v[(i + 1) % 3] == f2.v[j]) {
								Edge e = new Edge();
								e.v[0] = f1.v[i];
								e.v[1] = f1.v[(i + 1) % 3];
								e.f[0] = f1;
								e.f[1] = f2;
								f1.e[i] = e;
								f2.e[j] = e;
								f1.v[i].e.Add(e);
								f2.v[j].e.Add(e);
								edges.Add(e);
							}
						}
					}

					// make sure edge was found
					if (f1.e[i] == null) {
						numMissingEdges++;
					}
				}
			}

			//            if (numMissingEdges > 0)
			//                throw new Exception("Unable to construct polyhedron b/c of hole in mesh: " + numMissingEdges + " missing edges\n");
			//
			foreach (Face f in faces) {
				f.Initialize();
			}

			foreach (Edge e in edges) {
				e.Initialize();
			}

			foreach (Vertex v in verts) {
				v.Initialize();
			}
		}

		public void Transform(Matrix m) {
			foreach (Vertex v in verts) {
				v.Transform(m);
			}

			foreach (Face f in faces) {
				f.Transform(m);
			}

			foreach (Edge e in edges) {
				e.Transform(m);
			}
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

		[StructLayout(LayoutKind.Sequential)]
		private struct PositionOnly {
			public float X;

			public float Y;

			public float Z;

			public Vector3 Position {
				get {
					Vector3 vector3 = new Vector3();
					vector3 = new Vector3(this.X, this.Y, this.Z);
					return vector3;
				}
				set {
					this.X = value.X;
					this.Y = value.Y;
					this.Z = value.Z;
				}
			}

			public static readonly int StrideSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(PositionOnly));

			public PositionOnly(Vector3 value) : this() {
				Position = value;
			}

			public PositionOnly(float xvalue, float yvalue, float zvalue) {
				this.X = xvalue;
				this.Y = yvalue;
				this.Z = zvalue;
			}
		}
	}
}