using System;
using System.Collections.Generic;
using SlimDX;

namespace PhysicsEngine {
	public class CollisionPair {
		public RigidBody[] body;

		// collisionPoints[i] are in the reference frame of body[i]
		public List<Vector3>[] collisionPoints;

		public CollisionPair(RigidBody[] b, List<Vector3>[] cp) {
			body = b;
			collisionPoints = cp;
		}

		public void ResolveCollision(float dt) {
			// save body state, then update vel and pos for interference detection
			do {
				State[] curState = new State[2];
				for (int i = 0; i < 2; i++) {
					curState[i] = new State(body[i]);
					body[i].UpdateVel(dt);
					body[i].UpdatePos(dt);
				}
				// find the deepest penetrating point
				CollisionPoint bestCP = new CollisionPoint(new Vector3(), new Vector3(), float.PositiveInfinity);
				int j;
				for (int i = 0; i < 2; i++) {
					j = 0;
					while (j < collisionPoints[i].Count) {
						CollisionPoint cp = body[1 - i].EvaluateSDM(body[1 - i].PointWorldToBody(body[i].PointBodyToWorld((Vector3) collisionPoints[i][j])), true);
						if (cp.d > 0)
							collisionPoints[i].RemoveAt(j);
						else {

							cp.p = body[1 - i].PointBodyToWorld(cp.p);
							cp.n = body[1 - i].VectorBodyToWorld(cp.n);
							if (i == 1)
								cp.n *= -1; // so don't have to worry about direction of normal
											// remove any non-penetrating or separating points
											//Vector3 vWorldRelative = curState[0].WorldVelocity(cp.p) - curState[1].WorldVelocity(cp.p);
							Vector3 vWorldRelative = curState[0].vel + Vector3.Cross(curState[0].rotation, cp.p - curState[0].pos)
								- (curState[1].vel + Vector3.Cross(curState[1].rotation, cp.p - curState[1].pos));
							float vWorldRelativeN = Vector3.Dot(vWorldRelative, cp.n);
							if (vWorldRelativeN >= -0.001) // HACK
							{
								collisionPoints[i].RemoveAt(j);
							} else {

								if (cp.d < bestCP.d) {
									bestCP = cp;
								}

								j++;
							}
						}
					}
				}

				// restore original body state to do collision processing
				for (int i = 0; i < 2; i++) {
					body[i].SetState(curState[i]);
				}
				if (collisionPoints[0].Count == 0 && collisionPoints[1].Count == 0)
					break;
				//p is collision point, N is normal, r is constant of restitution (bounciness)
				GenerateImpulse(bestCP.p, bestCP.n, Math.Min(body[0].Bounciness, body[1].Bounciness), body[0], body[1]);
				//while (; //true if completed collisions
			} while (true);
		}

		public void ResolveContact(float r) {
			// find the deepest penetrating point
			do {
				CollisionPoint bestCP = new CollisionPoint(new Vector3(), new Vector3(), float.PositiveInfinity);
				for (int i = 0; i < 2; i++) {
					for (int j = 0; j < collisionPoints[i].Count;) {
						CollisionPoint cp = body[1 - i].EvaluateSDM(body[1 - i].PointWorldToBody(body[i].PointBodyToWorld((Vector3) collisionPoints[i][j])), true);
						if (cp.d > 0) {
							collisionPoints[i].RemoveAt(j);
							continue;
						}

						cp.p = body[1 - i].PointBodyToWorld(cp.p);
						cp.n = body[1 - i].VectorBodyToWorld(cp.n);
						if (i == 1)
							cp.n *= -1; // so don't have to worry about direction of normal
										// remove any non-penetrating points

						Vector3 vWorldRelative = body[0].WorldVelocity(cp.p) - body[1].WorldVelocity(cp.p);
						float vWorldRelativeN = Vector3.Dot(vWorldRelative, cp.n);
						if (cp.d < bestCP.d && vWorldRelativeN < -0.001) // HACK
						{ // only choose points with non-separating velocity
							bestCP = cp;
						}

						j++;
					}
				}
				if (bestCP.d == float.PositiveInfinity)
					return;
				GenerateImpulse(bestCP.p, bestCP.n, r, body[0], body[1]);
				// all points are separating
			} while (true);
			//true if all points are seperating and collisions are completed. while (bestCP.d != float.PositiveInfinity);
		}


		private void GenerateImpulse(Vector3 p, Vector3 N, float r, RigidBody b0, RigidBody b1) {
			// frictional coefficient
			float u = Math.Max(body[0].Friction, body[1].Friction);

			Vector3 vOldWorldRelative = body[0].WorldVelocity(p) - body[1].WorldVelocity(p);
			float vOldWorldRelativeN = Vector3.Dot(vOldWorldRelative, N);
			Vector3 vNewWorldRelative = r * vOldWorldRelativeN * N;

			Matrix3 K = body[0].ComputeK(p) + body[1].ComputeK(p);
			Matrix3 kInv;
			K.Invert(out kInv);
			Vector3 J = -1 * kInv * (vNewWorldRelative - vOldWorldRelative);
			float Jn = Vector3.Dot(J, N);
			Vector3 Jt = J - (Jn * N);

			if (Jt.Length() >= u * Math.Abs(Jn)) {
				// impulse not in friction cone; need to do sliding friction

				Vector3 vOldWorldRelativeT = vOldWorldRelative - (vOldWorldRelativeN * N);
				float TLength = vOldWorldRelativeT.Length();
				Vector3 T = (TLength == 0 ? Vector3.Zero : vOldWorldRelativeT * (1 / TLength));

				Jn = ((r + 1) * vOldWorldRelativeN) / (Vector3.Dot(K * (N - (u * T)), N));
				J = Jn * (N - (u * T));
			}

			// XXX HACK
			//if (Math.Abs(Jn) < 0.001) break;

			// apply impulse to both bodies
			body[0].ApplyImpulse(p, -1 * J);
			body[1].ApplyImpulse(p, J);
		}

	}
}