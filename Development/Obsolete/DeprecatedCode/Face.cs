using System.Numerics;
using System.Runtime.CompilerServices;

namespace System.Drawing {
	public class Face {
		public PolyhedronVertex v0, v1, v2;
		public Edge e0, e1, e2, e3;
		public Plane plane;

		// for distance calculations
		private Vector3 edge0, edge1;
		private float fA00, fA01, fA11, fDet;

		public void Initialize() {
			plane = Plane.CreateFromVertices(v0.pos, v1.pos, v2.pos);

			// for distance calculations
			edge0 = v1.pos - v0.pos;
			edge1 = v2.pos - v0.pos;
			fA00 = edge0.LengthSquared();
			fA01 = Vector3.Dot(edge0, edge1);
			fA11 = edge1.LengthSquared();
			fDet = Math.Abs(fA00 * fA11 - fA01 * fA01);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3 PointFromST(float s, float t) {
			return (1 - (s + t)) * v0.pos + s * v1.pos + t * v2.pos;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3 NormalFromST(float s, float t) {
			return (1 - (s + t)) * v0.normal + s * v1.normal + t * v2.normal;
		}

		public void Transform(ref Matrix4 m) {
			plane = plane.Transform(ref m);
			edge0 = edge0.TransformNormal(ref m);
			edge1 = edge1.TransformNormal(ref m);
		}

		// this code was adapted from Magic Software's point-triangle distance
		// code which can be found at:
		// http://www.magic-software.com
		public void SqrDistance(Vector3 pt, out float fSqrDist, out float fS, out float fT) {
			Vector3 kDiff = v0.pos - pt;
			float fB0 = Vector3.Dot(kDiff, edge0);
			float fB1 = Vector3.Dot(kDiff, edge1);
			float fC = kDiff.LengthSquared();
			fS = fA01 * fB1 - fA11 * fB0;
			fT = fA01 * fB0 - fA00 * fB1;
			fSqrDist = (float) 0.0;

			if (fS + fT <= fDet) {
				if (fS < (float) 0.0) {
					if (fT < (float) 0.0)  // region 4
					{
						if (fB0 < (float) 0.0) {
							fT = (float) 0.0;
							if (-fB0 >= fA00) {
								fS = (float) 1.0;
								fSqrDist = fA00 + ((float) 2.0) * fB0 + fC;
							} else {
								fS = -fB0 / fA00;
								fSqrDist = fB0 * fS + fC;
							}
						} else {
							fS = (float) 0.0;
							if (fB1 >= (float) 0.0) {
								fT = (float) 0.0;
								fSqrDist = fC;
							} else if (-fB1 >= fA11) {
								fT = (float) 1.0;
								fSqrDist = fA11 + ((float) 2.0) * fB1 + fC;
							} else {
								fT = -fB1 / fA11;
								fSqrDist = fB1 * fT + fC;
							}
						}
					} else  // region 3
					  {
						fS = (float) 0.0;
						if (fB1 >= (float) 0.0) {
							fT = (float) 0.0;
							fSqrDist = fC;
						} else if (-fB1 >= fA11) {
							fT = (float) 1.0;
							fSqrDist = fA11 + ((float) 2.0) * fB1 + fC;
						} else {
							fT = -fB1 / fA11;
							fSqrDist = fB1 * fT + fC;
						}
					}
				} else if (fT < (float) 0.0)  // region 5
				  {
					fT = (float) 0.0;
					if (fB0 >= (float) 0.0) {
						fS = (float) 0.0;
						fSqrDist = fC;
					} else if (-fB0 >= fA00) {
						fS = (float) 1.0;
						fSqrDist = fA00 + ((float) 2.0) * fB0 + fC;
					} else {
						fS = -fB0 / fA00;
						fSqrDist = fB0 * fS + fC;
					}
				} else  // region 0
				  {
					// minimum at interior point
					float fInvDet = ((float) 1.0) / fDet;
					fS *= fInvDet;
					fT *= fInvDet;
					fSqrDist = fS * (fA00 * fS + fA01 * fT + ((float) 2.0) * fB0) +
						fT * (fA01 * fS + fA11 * fT + ((float) 2.0) * fB1) + fC;
				}
			} else {
				float fTmp0, fTmp1, fNumer, fDenom;

				if (fS < (float) 0.0)  // region 2
				{
					fTmp0 = fA01 + fB0;
					fTmp1 = fA11 + fB1;
					if (fTmp1 > fTmp0) {
						fNumer = fTmp1 - fTmp0;
						fDenom = fA00 - 2f * fA01 + fA11;
						if (fNumer >= fDenom) {
							fS = (float) 1.0;
							fT = (float) 0.0;
							fSqrDist = fA00 + ((float) 2.0) * fB0 + fC;
						} else {
							fS = fNumer / fDenom;
							fT = (float) 1.0 - fS;
							fSqrDist = fS * (fA00 * fS + fA01 * fT + 2f * fB0) +
								fT * (fA01 * fS + fA11 * fT + ((float) 2.0) * fB1) + fC;
						}
					} else {
						fS = (float) 0.0;
						if (fTmp1 <= (float) 0.0) {
							fT = (float) 1.0;
							fSqrDist = fA11 + ((float) 2.0) * fB1 + fC;
						} else if (fB1 >= (float) 0.0) {
							fT = (float) 0.0;
							fSqrDist = fC;
						} else {
							fT = -fB1 / fA11;
							fSqrDist = fB1 * fT + fC;
						}
					}
				} else if (fT < (float) 0.0)  // region 6
				  {
					fTmp0 = fA01 + fB1;
					fTmp1 = fA00 + fB0;
					if (fTmp1 > fTmp0) {
						fNumer = fTmp1 - fTmp0;
						fDenom = fA00 - ((float) 2.0) * fA01 + fA11;
						if (fNumer >= fDenom) {
							fT = (float) 1.0;
							fS = (float) 0.0;
							fSqrDist = fA11 + ((float) 2.0) * fB1 + fC;
						} else {
							fT = fNumer / fDenom;
							fS = (float) 1.0 - fT;
							fSqrDist = fS * (fA00 * fS + fA01 * fT + ((float) 2.0) * fB0) +
								fT * (fA01 * fS + fA11 * fT + ((float) 2.0) * fB1) + fC;
						}
					} else {
						fT = (float) 0.0;
						if (fTmp1 <= (float) 0.0) {
							fS = (float) 1.0;
							fSqrDist = fA00 + ((float) 2.0) * fB0 + fC;
						} else if (fB0 >= (float) 0.0) {
							fS = (float) 0.0;
							fSqrDist = fC;
						} else {
							fS = -fB0 / fA00;
							fSqrDist = fB0 * fS + fC;
						}
					}
				} else  // region 1
				  {
					fNumer = fA11 + fB1 - fA01 - fB0;
					if (fNumer <= (float) 0.0) {
						fS = (float) 0.0;
						fT = (float) 1.0;
						fSqrDist = fA11 + ((float) 2.0) * fB1 + fC;
					} else {
						fDenom = fA00 - 2f * fA01 + fA11;
						if (fNumer >= fDenom) {
							fS = (float) 1.0;
							fT = (float) 0.0;
							fSqrDist = fA00 + ((float) 2.0) * fB0 + fC;
						} else {
							fS = fNumer / fDenom;
							fT = (float) 1.0 - fS;
							fSqrDist = fS * (fA00 * fS + fA01 * fT + ((float) 2.0) * fB0) +
								fT * (fA01 * fS + fA11 * fT + ((float) 2.0) * fB1) + fC;
						}
					}
				}
			}

			fSqrDist = Math.Abs(fSqrDist);
		}
	}
}