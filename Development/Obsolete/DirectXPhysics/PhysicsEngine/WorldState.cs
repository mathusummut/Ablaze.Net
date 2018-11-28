using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using SlimDX;
using SlimDX.Direct3D9;

namespace PhysicsEngine {
	public class WorldState : IDisposable {
		private object SyncRoot = new object();
		public List<RigidBody> BodyList;
		public float Resolution = 0.66f;

		public WorldState(List<RigidBody> bodies) {
			BodyList = bodies;
		}

		public void Render(Device device) {
			if (BodyList == null)
				return;
			for (int i = 0; i < BodyList.Count; i++)
				BodyList[i].Render(device);
		}

		private CollisionPair CheckCollision(RigidBody[] b) {
			// if both bodies have infinite mass ignore collision
			if (b[0].Mass == float.PositiveInfinity && b[1].Mass == float.PositiveInfinity)
				return null;

			// TODO: better macro collision detection than bounding spheres

			// check bounding spheres
			if ((b[0].Pos - b[1].Pos).LengthSquared() > (b[0].Type.radius + b[1].Type.radius) * (b[0].Type.radius + b[1].Type.radius))
				return null;


			List<Vector3>[] cp = new List<Vector3>[2];
			cp[0] = new List<Vector3>();
			cp[1] = new List<Vector3>();
			Vector3 p, edge_vector, inc;
			float edge_length;
			int n;
			for (int i = 0; i < 2; i++) {
				foreach (Vertex v in b[i].Type.polyhedron.verts) {
					p = b[1 - i].PointWorldToBody(b[i].PointBodyToWorld(v.pos));
					if (b[1 - i].EvaluateSDM(p, false).d <= 0)
						cp[i].Add(v.pos);
				}
				// find the deepest point of penetration along each edge
				foreach (Edge e in b[i].Type.polyhedron.edges) {
					edge_vector = b[1 - i].VectorWorldToBody(b[i].VectorBodyToWorld(e.v[1].pos - e.v[0].pos));
					edge_length = edge_vector.Length();
					inc = edge_vector * (Resolution / edge_length);
					p = b[1 - i].PointWorldToBody(b[i].PointBodyToWorld(e.v[0].pos));
					n = (int) (edge_length / Resolution);
					//Console.Out.WriteLine("n = {0}", n);    

					CollisionPoint bestCP = new CollisionPoint(new Vector3(), new Vector3(), float.PositiveInfinity);
					while (n > 0) {
						p += inc;
						CollisionPoint curCP = b[1 - i].EvaluateSDM(p, false);
						if (curCP.d < bestCP.d && curCP.d <= 0)
							bestCP = curCP;
						n--;
					}
					if (bestCP.d != float.PositiveInfinity)
						cp[i].Add(b[i].PointWorldToBody(b[1 - i].PointBodyToWorld(bestCP.p)));
				}
			}

			if (cp[0].Count > 0 || cp[1].Count > 0)
				return new CollisionPair(b, cp);
			else
				return null;
		}

		private void ProcessAllCollisions(float dt) {
			SyncedList<CollisionPair> collisionPairs = new SyncedList<CollisionPair>();

			// collision processing
			State[] oldState = new State[BodyList.Count];
			// temporarily update vel then pos for each body
			ParallelLoop.For(0, BodyList.Count, i => {
				oldState[i] = new State(BodyList[i]);
				BodyList[i].UpdateVel(dt);
				BodyList[i].UpdatePos(dt);
			});

			// find all collision pairs
			collisionPairs.Clear();
			ParallelLoop.For(0, BodyList.Count, i => {
				for (int j = i + 1; j < BodyList.Count; j++) {
					// TODO: better macro collision detection
					CollisionPair cp = CheckCollision(new RigidBody[] { BodyList[i], BodyList[j] });
					if (cp != null)
						collisionPairs.Add(cp);
				}
			});

			// restore old states
			for (int i = 0; i < BodyList.Count; i++) {
				BodyList[i].SetState(oldState[i]);
			}

			// process all collision pairs
			foreach (CollisionPair cp in collisionPairs) {
				cp.ResolveCollision(dt);
			}
		}

		private void ProcessAllContacts(float dt) {
			// first, clear level and the collisions for each body
			for (int i = 0; i < BodyList.Count; i++) {
				RigidBody a = BodyList[i];
				a.levelSet = false;
				a.collisions.Clear();
			}

			ConcurrentQueue<RigidBody> queue = new ConcurrentQueue<RigidBody>();
			ParallelLoop.For(0, BodyList.Count, i => {
				RigidBody a = BodyList[i];

				if (a.Mass == float.PositiveInfinity) {
					// keep track of bodies w/ infinite mass for later
					a.level = -1;
					a.levelSet = true;
					queue.Enqueue(a);

					// these guys don't fall
					return;
				}

				// let each body fall due to gravity, check for collisions with all other bodies
				State savedState = new State(a);
				a.UpdatePos(dt);

				for (int j = 0; j < BodyList.Count; j++) {
					if (i == j)
						continue;
					RigidBody b = BodyList[j];
					CollisionPair cp = CheckCollision(new RigidBody[] { a, b });
					if (cp != null) {
						//Console.Out.WriteLine("contact between '{0}' and '{1}'", a.Name, b.Name);
						a.collisions.Add(cp);
						b.collisions.Add(cp);
					}
				}

				a.SetState(savedState);
			});

			// topological sort (bfs)
			// TODO check this
			int max_level = -1;
			while (queue.Count > 0) {
				RigidBody a;
				queue.TryDequeue(out a);
				//Console.Out.WriteLine("considering collisions with '{0}'", a.Name);
				if (a.level > max_level)
					max_level = a.level;
				foreach (CollisionPair cp in a.collisions) {
					RigidBody b = (cp.body[0] == a ? cp.body[1] : cp.body[0]);
					//Console.Out.WriteLine("considering collision between '{0}' and '{1}'", a.Name, b.Name);
					if (!b.levelSet) {
						b.level = a.level + 1;
						b.levelSet = true;
						queue.Enqueue(b);
						//Console.Out.WriteLine("found body '{0}' in level {1}", b.Name, b.level);
					}
				}
			}

			int num_levels = max_level + 1;

			//Console.WriteLine("num_levels = {0}", num_levels);

			List<RigidBody>[] bodiesAtLevel = new List<RigidBody>[num_levels];
			List<CollisionPair>[] collisionsAtLevel = new List<CollisionPair>[num_levels];
			for (int i = 0; i < num_levels; i++) {
				bodiesAtLevel[i] = new List<RigidBody>();
				collisionsAtLevel[i] = new List<CollisionPair>();
			}

			ParallelLoop.For(0, BodyList.Count, i => {
				RigidBody a = BodyList[i];
				if (!a.levelSet || a.level < 0)
					return; // either a static body or no contacts

				// add a to a's level
				bodiesAtLevel[a.level].Add(a);

				// add collisions involving a to a's level
				foreach (CollisionPair cp in a.collisions) {
					RigidBody b = (cp.body[0] == a ? cp.body[1] : cp.body[0]);
					if (b.level <= a.level) // contact with object at or below the same level as a
					{
						// make sure not to add duplicate collisions
						bool found = false;
						foreach (CollisionPair cp2 in collisionsAtLevel[a.level])
							if (cp == cp2)
								found = true;

						if (!found)
							collisionsAtLevel[a.level].Add(cp);
					}
				}
			});
			for (int level = 0; level < num_levels; level++) {
				// process all contacts
				foreach (CollisionPair cp in collisionsAtLevel[level]) {
					cp.ResolveContact(0.5f);
				}
			}
			for (int level = 0; level < num_levels; level++) {
				// process all contacts
				foreach (CollisionPair cp in collisionsAtLevel[level]) {
					cp.ResolveContact(0f);
				}
			}

			for (int level = 0; level < num_levels; level++) {
				// process all contacts
				foreach (CollisionPair cp in collisionsAtLevel[level]) {
					cp.ResolveContact(0);
				}

				// freeze each body in this level
				foreach (RigidBody body in bodiesAtLevel[level]) {
					body.frozen = true;
				}
			}

			// unfreeze all bodies
			for (int level = 0; level < num_levels; level++) {
				foreach (RigidBody body in bodiesAtLevel[level]) {
					body.frozen = false;
				}
			}
		}


		public void Update(float dt) {
			if (dt == 0 || BodyList == null)
				return;
			lock (SyncRoot) {
				ProcessAllCollisions(dt);
				// update vel for each body
				for (int i = 0; i < BodyList.Count; i++)
					BodyList[i].UpdateVel(dt);
				ProcessAllContacts(dt);
				// update pos for each body
				ParallelLoop.For(0, BodyList.Count, i => {
					BodyList[i].UpdatePos(dt);
				});
			}
		}

		~WorldState() {
			Dispose();
		}

		public void Dispose() {
			for (int i = 0; i < BodyList.Count; i++) {
				BodyList[i].Dispose();
			}
			BodyList = new List<RigidBody>();
			GC.SuppressFinalize(this);
		}
	}	
}