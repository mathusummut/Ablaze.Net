using System.Collections.Generic;
using System.Drawing;

namespace System.Physics2D {
	public class LinearPath : LiftPath {
		public readonly List<Vector2D> Points;
		public bool GoToNextPointIfStuck;
		public int Index;

		public LinearPath(double speed, bool goToNextPointIfStuck, params Vector2D[] points) {
			Points = points == null ? new List<Vector2D>() : new List<Vector2D>(points);
			Speed = speed;
			GoToNextPointIfStuck = goToNextPointIfStuck;
		}

		public override Vector2D NextVector(Vector2D currentLocation) {
			Index %= Points.Count;
			int originalIndex = Index;
			while (Points[Index].DistanceFrom(currentLocation) < Speed) {
				Index = (Index + 1) % Points.Count;
				if (Index == originalIndex)
					return currentLocation;
			}
			if (stuckCounter > 3) {
				stuckCounter = 0;
				if (GoToNextPointIfStuck)
					Index = (Index + 1) % Points.Count;
			}
			return Vector2D.Multiply(Vector2D.Normalize(Points[Index] - currentLocation), Speed);
		}
	}
}