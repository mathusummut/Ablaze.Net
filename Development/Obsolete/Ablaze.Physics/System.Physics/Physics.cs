using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace System.Physics {
	public class Physics : IDisposable {
		public delegate void CollisionHandle(CollisionEventArgs e);
		public readonly List<Particle> Particles = new List<Particle>();
		private ManualResetEventSlim resetEvent = new ManualResetEventSlim();
		private PreciseStopwatch stopwatch = new PreciseStopwatch();
		private Action<int, ParallelLoopState> updateParticles, updateSprings, solveCollisions;
		private bool clearing;
		private int itemIndex;
		public event CollisionHandle Collision;
		public event WaitCallback Updated;
		public double StepInMilliseconds;
		

		public bool Updating {
			get;
			private set;
		}

		public bool Running {
			get {
				return stopwatch.Running;
			}
			set {
				stopwatch.Running = value;
			}
		}

		public Physics(double stepInMilliseconds) {
			updateParticles = UpdateParticles;
			updateSprings = UpdateSprings;
			solveCollisions = SolveCollisions;
			StepInMilliseconds = stepInMilliseconds;
		}

		public void Update() {
			Updating = true;
			itemIndex = -1;
			Parallel.For(0, int.MaxValue, updateParticles);
			itemIndex = -1;
			Parallel.For(0, int.MaxValue, updateSprings);
			stopwatch.ElapsedTicks = 0.0;
			Particle p;
			int i;
			for (i = 0; i < Particles.Count; i++) {
				p = Particles[i];
				if (p != null)
					p.Damping = 1.0;
			}
			itemIndex = -1;
			Parallel.For(0, int.MaxValue, solveCollisions);
			Updating = false;
			if (Updated != null)
				ThreadPool.UnsafeQueueUserWorkItem(Updated, this);
			if (clearing) {
				ClearAll();
				resetEvent.Set();
			}
		}

		private void UpdateParticles(int index, ParallelLoopState state) {
			int i = ++itemIndex;
			if (i < Particles.Count) {
				Particle p = Particles[i];
				int j;
				if (p == null)
					state.Stop();
				else if (p.IsDisposed)
					Particles.RemoveAt(i);
				else {
					for (j = 0; j < p.toAdd.Count; j++)
						Particles.Add(p.toAdd[j]);
					p.toAdd.Clear();
					if (p.IsSpring)
						return;
					double elapsed = stopwatch.ElapsedMilliseconds;
					p.Update(elapsed, elapsed / StepInMilliseconds);
				}
			} else
				state.Stop();
		}

		private void UpdateSprings(int index, ParallelLoopState state) {
			int i = ++itemIndex;
			if (i < Particles.Count) {
				Particle p = Particles[i];
				if (p == null)
					state.Stop();
				else if (p.IsDisposed)
					Particles.RemoveAt(i);
				else if (p.IsSpring) {
					double elapsed = stopwatch.ElapsedMilliseconds;
					p.Update(elapsed, elapsed / StepInMilliseconds);
				}
			} else
				state.Stop();
		}

		private void SolveCollisions(int index, ParallelLoopState state) {
			int i = ++itemIndex;
			if (i < Particles.Count) {
				Particle pa = Particles[i];
				if (pa == null)
					state.Stop();
				else if (pa.IsCollidable && !pa.IsSpring) {
					Particle pb;
					CollisionEventArgs collision;
					for (int j = 0; j < Particles.Count; j++) {
						pb = Particles[j];
						if (!(i == j || pb == null) && pb.IsCollidable && !(pa.IsFixed && pb.IsFixed) && (pa.CollisionFlag == pb.CollisionFlag || pa.CollisionFlag == 0 || pb.CollisionFlag == 0) && !(pa.IsFluid && pb.IsFluid) && !(pb.IsSpring && pb.springComponent.IsConnectedTo(pa))) {
							collision = SolvePossibleCollision(pa, pb);
							if (collision.Depth > 0.0 && Collision != null)
								Collision(new CollisionEventArgs(collision.P1, collision.P2, collision.Normal, collision.Depth));
						}
					}
				}
			} else
				state.Stop();
		}

		public void Render() {
			Particle p;
			for (int i = 0; i < Particles.Count; i++) {
				p = Particles[i];
				if (!(p == null || p.Model.HasAlpha))
					p.Render();
			}
			for (int i = 0; i < Particles.Count; i++) {
				p = Particles[i];
				if (p != null && p.Model.HasAlpha)
					p.Render();
			}
		}

		public static CollisionEventArgs SolvePossibleCollision(Particle objA, Particle objB) {
			if (objA == objB)
				return CollisionEventArgs.Empty;
			PolygonParticle shapeA = objA as PolygonParticle;
			PolygonParticle shapeB = objB as PolygonParticle;
			if (shapeA == null)
				return shapeB == null ? SolvePossibleCollision((ElliseParticle) objA, (ElliseParticle) objB) : SolvePossibleCollision(shapeB, (ElliseParticle) objA);
			else
				return shapeB == null ? SolvePossibleCollision(shapeA, (ElliseParticle) objB) : SolvePossibleCollision(shapeA, shapeB);
		}

		public static CollisionEventArgs SolvePossibleCollision(PolygonParticle ra, PolygonParticle rb) {
			Vector2D collisionNormal = Vector2D.Zero;
			double collisionDepth = int.MaxValue;
			Vector2D axis = Vector2D.Zero;
			axis = ra.Axis;
			double depth = TestIntervals(ra.Project(axis), rb.Project(axis));
			if (depth == 0.0)
				return CollisionEventArgs.Empty;
			if (Math.Abs(depth) < Math.Abs(collisionDepth)) {
				collisionNormal = axis;
				collisionDepth = depth;
			}
			axis = axis.PerpendicularLeft;
			depth = TestIntervals(ra.Project(axis), rb.Project(axis));
			if (depth == 0.0)
				return CollisionEventArgs.Empty;
			if (Math.Abs(depth) < Math.Abs(collisionDepth)) {
				collisionNormal = axis;
				collisionDepth = depth;
			}
			axis = rb.Axis;
			depth = TestIntervals(ra.Project(axis), rb.Project(axis));
			if (depth == 0.0)
				return CollisionEventArgs.Empty;
			if (Math.Abs(depth) < Math.Abs(collisionDepth)) {
				collisionNormal = axis;
				collisionDepth = depth;
			}
			axis = axis.PerpendicularLeft;
			depth = TestIntervals(ra.Project(axis), rb.Project(axis));
			if (depth == 0.0)
				return CollisionEventArgs.Empty;
			if (Math.Abs(depth) < Math.Abs(collisionDepth)) {
				collisionNormal = axis;
				collisionDepth = depth;
			}
			CollisionEventArgs resultant = new CollisionEventArgs(ra, rb, collisionNormal, collisionDepth);
			if (ra.IsFluid) {
				for (int i = 0; i < ra.fluid.Forces.Count; i++)
					rb.ApplyForce(ra.fluid.Forces[i]);
				rb.ApplyForce(new Force(new Vector2D(0.0, ra.Density / -rb.Density), 1.0, ra.ForceFlag, true, true, false));
				rb.Damping *= 1.0 - ra.Friction;
			} else if (rb.IsFluid) {
				for (int i = 0; i < rb.fluid.Forces.Count; i++)
					ra.ApplyForce(rb.fluid.Forces[i]);
				ra.ApplyForce(new Force(new Vector2D(0.0, rb.Density / -ra.Density), 1.0, rb.ForceFlag, true, true, false));
				ra.Damping *= 1.0 - rb.Friction;
			} else {
				Vector2D mtd = collisionNormal * collisionDepth;
				double te = ra.Elasticity + rb.Elasticity;
				double tf = 1.0 - (ra.Friction + rb.Friction);
				double ma = ra.IsFixed ? int.MaxValue : ra.Mass;
				double mb = rb.IsFixed ? int.MaxValue : rb.Mass;
				double tm = ma + mb;
				Vector2D caVn = collisionNormal * Vector2D.Dot(collisionNormal, ra.Velocity);
				Vector2D caVt = caVt = (ra.Velocity - caVn) * tf;
				Vector2D cbVn = collisionNormal * Vector2D.Dot(collisionNormal, rb.Velocity);
				Vector2D cbVt = (rb.Velocity - cbVn) * tf;
				Vector2D vnA = (cbVn * ((te + 1.0) * mb) + (caVn * (ma - (te * mb)))) / tm;
				Vector2D vnB = (caVn * ((te + 1.0) * ma) + (cbVn * (mb - (te * ma)))) / tm;
				Vector2D mtdA = mtd * (mb / tm);
				Vector2D mtdB = mtd * (-ma / tm);
				if (!ra.IsFixed)
					ra.SolveCollision(mtdA, vnA + caVt, collisionNormal);
				if (!rb.IsFixed)
					rb.SolveCollision(mtdB, vnB + cbVt, collisionNormal);
			}
			return resultant;
        }

		public static CollisionEventArgs SolvePossibleCollision(PolygonParticle ra, ElliseParticle ca) {
			Vector2D collisionNormal = Vector2D.Zero;
			double collisionDepth = int.MaxValue;
			Vector2D boxAxis = Vector2D.Zero;
			double r = (Vector2D.GetPointOnEllipseFromAngle(ca.Location, ca.Radius, ca.Rotation, ra.Location.GetRotationFrom(ca.Location)) - ca.Location).Length;
			bool temp = true;
			boxAxis = ra.Axis;
			double depth = TestIntervals(ra.Project(boxAxis), ca.Project(boxAxis));
			if (depth == 0.0)
				return CollisionEventArgs.Empty;
			if (Math.Abs(depth) < Math.Abs(collisionDepth)) {
				collisionNormal = boxAxis;
				collisionDepth = depth;
			}
			if (Math.Abs(depth) >= r)
				temp = false;
			boxAxis = boxAxis.PerpendicularLeft;
			depth = TestIntervals(ra.Project(boxAxis), ca.Project(boxAxis));
			if (depth == 0.0)
				return CollisionEventArgs.Empty;
			if (Math.Abs(depth) < Math.Abs(collisionDepth)) {
				collisionNormal = boxAxis;
				collisionDepth = depth;
			}
			if (Math.Abs(depth) >= r)
				temp = false;
			if (temp) {
				Vector2D d = ca.Location - ra.Location;
				Vector2D q = ra.Location;
				if (Vector2D.Dot(d, ra.Axis) >= 0.0)
					q += ra.Axis * ra.Size.X;
				else
					q += ra.Axis * -ra.Size.X;
				if (Vector2D.Dot(d, ra.Axis.PerpendicularLeft) >= 0.0)
					collisionNormal = q + (ra.Axis.PerpendicularLeft * ra.Size.Y) - ca.Location;
				else
					collisionNormal = q + (ra.Axis.PerpendicularLeft * -ra.Size.Y) - ca.Location;
				double mag = collisionNormal.Length;
				collisionNormal /= mag;
				collisionDepth = r - mag;
				if (collisionDepth <= 0.0)
					return CollisionEventArgs.Empty;
			}
			CollisionEventArgs resultant = new CollisionEventArgs(ra, ca, collisionNormal, collisionDepth);
			if (ra.IsFluid) {
				for (int i = 0; i < ra.fluid.Forces.Count; i++)
					ca.ApplyForce(ra.fluid.Forces[i]);
				ca.ApplyForce(new Force(new Vector2D(0.0, ra.Density / -ca.Density), 1.0, ra.ForceFlag, true, true, false));
				ca.Damping *= 1.0 - ra.Friction;
				ca.SolveRotation(collisionNormal, ra.fluid.Traction);
			} else if (ca.IsFluid) {
				for (int i = 0; i < ca.fluid.Forces.Count; i++)
					ra.ApplyForce(ca.fluid.Forces[i]);
				ra.ApplyForce(new Force(new Vector2D(0.0, ca.Density / -ra.Density), 1.0, ca.ForceFlag, true, true, false));
				ra.Damping *= 1.0 - ca.Friction;
			} else {
				Vector2D mtd = collisionNormal * collisionDepth;
				double te = ra.Elasticity + ca.Elasticity;
				double tf = 1.0 - (ra.Friction + ca.Friction);
				double ma = ra.IsFixed ? int.MaxValue : ra.Mass;
				double mb = ca.IsFixed ? int.MaxValue : ca.Mass;
				double tm = ma + mb;
				Vector2D caVn = collisionNormal * Vector2D.Dot(collisionNormal, ra.Velocity);
				Vector2D caVt = (ra.Velocity - caVn) * tf;
				Vector2D cbVn = collisionNormal * Vector2D.Dot(collisionNormal, ca.Velocity);
				Vector2D cbVt = (ca.Velocity - cbVn) * tf;
				Vector2D vnA = (cbVn * ((te + 1.0) * mb) + (caVn * (ma - (te * mb)))) / tm;
				Vector2D vnB = (caVn * ((te + 1.0) * ma) + (cbVn * (mb - (te * ma)))) / tm;
				Vector2D mtdA = mtd * (mb / tm);
				Vector2D mtdB = mtd * (-ma / tm);
				if (!ra.IsFixed)
					ra.SolveCollision(mtdA, vnA + caVt, collisionNormal);
				if (!ca.IsFixed) {
					if (collisionDepth < 0.0)
						collisionNormal = -collisionNormal;
					ca.SolveCollision(mtdB, vnB + cbVt, collisionNormal);
				}
			}
			return resultant;
        }

		public static CollisionEventArgs SolvePossibleCollision(ElliseParticle ca, ElliseParticle cb) {
			Vector2D collisionNormal = ca.Location - cb.Location;
			double mag = collisionNormal.Length;
			double collisionDepth = (Vector2D.GetPointOnEllipseFromAngle(ca.Location, ca.Radius, ca.Rotation, cb.Location.GetRotationFrom(ca.Location)) - ca.Location).Length + (Vector2D.GetPointOnEllipseFromAngle(cb.Location, cb.Radius, cb.Rotation, ca.Location.GetRotationFrom(cb.Location)) - cb.Location).Length - mag;
			if (collisionDepth <= 0.0)
				return CollisionEventArgs.Empty;
			collisionNormal /= mag;
			CollisionEventArgs resultant = new CollisionEventArgs(ca, cb, collisionNormal, collisionDepth);
			if (ca.IsFluid) {
				for (int i = 0; i < ca.fluid.Forces.Count; i++)
					cb.ApplyForce(ca.fluid.Forces[i]);
				cb.ApplyForce(new Force(new Vector2D(0.0, ca.Density / -cb.Density), 1.0, ca.ForceFlag, true, true, false));
				cb.Damping *= 1.0 - ca.Friction;
				cb.SolveRotation(collisionNormal, 1.0 - ca.fluid.Traction);
			} else if (cb.IsFluid) {
				for (int i = 0; i < cb.fluid.Forces.Count; i++)
					ca.ApplyForce(cb.fluid.Forces[i]);
				ca.ApplyForce(new Force(new Vector2D(0.0, cb.Density / -ca.Density), 1.0, cb.ForceFlag, true, true, false));
				ca.Damping *= 1.0 - cb.Friction;
				ca.SolveRotation(collisionNormal, 1.0 - cb.fluid.Traction);
			} else {
				Vector2D mtd = collisionNormal * collisionDepth;
				double te = ca.Elasticity + cb.Elasticity;
				double tf = 1.0 - (ca.Friction + cb.Friction);
				double ma = ca.IsFixed ? int.MaxValue : ca.Mass;
				double mb = cb.IsFixed ? int.MaxValue : cb.Mass;
				double tm = ma + mb;
				Vector2D caVn = collisionNormal * Vector2D.Dot(collisionNormal, ca.Velocity);
				Vector2D caVt = (ca.Velocity - caVn) * tf;
				Vector2D cbVn = collisionNormal * Vector2D.Dot(collisionNormal, cb.Velocity);
				Vector2D cbVt = (cb.Velocity - cbVn) * tf;
				Vector2D vnA = (cbVn * ((te + 1.0) * mb) + (caVn * (ma - (te * mb)))) / tm;
				Vector2D vnB = (caVn * ((te + 1.0) * ma) + (cbVn * (mb - (te * ma)))) / tm;
				Vector2D mtdA = mtd * (mb / tm);
				Vector2D mtdB = mtd * (-ma / tm);
				if (!ca.IsFixed)
					ca.SolveCollision(mtdA, vnA + caVt, -collisionNormal);
				if (!cb.IsFixed)
					cb.SolveCollision(mtdB, vnB + cbVt, collisionNormal);
			}
			return resultant;
        }

		public static double TestIntervals(Vector2D intervalA, Vector2D intervalB) {
			if (intervalA.Y < intervalB.X || intervalB.Y < intervalA.X)
				return 0.0;
			double lenA = intervalB.Y - intervalA.X;
			double lenB = intervalB.X - intervalA.Y;
			return Math.Abs(lenA) < Math.Abs(lenB) ? lenA : lenB;
		}

		public void ClearAll() {
			ClearAll(true);
		}

		private void ClearAll(bool stayAlive) {
			if (Particles.Count == 0)
				return;
			if (Updating) {
				clearing = true;
				resetEvent.Reset();
				resetEvent.Wait();
			} else {
				bool oldRunning = Running;
				Running = false;
				Particle p;
				for (int i = 0; i < Particles.Count; i++) {
					p = Particles[i];
					if (p != null)
						p.Dispose();
				}
				Particles.Clear();
				if (stayAlive)
					Running = oldRunning;
			}
		}

		~Physics() {
			ClearAll(false);
		}

		public virtual void Dispose() {
			ClearAll(false);
		}
	}
}