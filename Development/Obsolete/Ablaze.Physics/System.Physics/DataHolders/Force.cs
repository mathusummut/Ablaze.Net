using System.Drawing;

namespace System.Physics2D {
	public struct Force : IEquatable<Force> {
		public Vector2D Direction;
		public int Flag;
		public double Strength;
		public bool IsRelativeToMass, IsTowardsPoint, IsDirectional;

		public Force(Vector2D direction, double strength, int flag, bool isRelativeToMass, bool isDirectional, bool isTowardsPoint) {
			Direction = direction;
			Flag = flag;
			IsRelativeToMass = isRelativeToMass;
			IsTowardsPoint = isTowardsPoint;
			IsDirectional = isDirectional;
			Strength = strength;
		}

		/// <summary>
		/// Compares the specified instances for equality.
		/// </summary>
		/// <param name="left">Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True if both instances are equal; false otherwise.</returns>
		public static bool operator ==(Force left, Force right) {
			return left.Equals(right);
		}

		/// <summary>
		/// Compares the specified instances for inequality.
		/// </summary>
		/// <param name="left">Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True if both instances are not equal; false otherwise.</returns>
		public static bool operator !=(Force left, Force right) {
			return !left.Equals(right);
		}

		/// <summary>
		/// Indicates whether this instance and a specified object are equal.
		/// </summary>
		/// <param name="obj">The object to compare to.</param>
		/// <returns>True if the instances are equal; false otherwise.</returns>
		public override bool Equals(object obj) {
			if (!(obj is Vector2))
				return false;
			return Equals((Vector2) obj);
		}

		/// <summary>Indicates whether the current vector is equal to another vector.</summary>
		/// <param name="other">A vector to compare with this vector.</param>
		/// <returns>true if the current vector is equal to the vector parameter; otherwise, false.</returns>
		public bool Equals(Force other) {
			return Direction == other.Direction && Flag == other.Flag && Strength == other.Strength && IsRelativeToMass == other.IsRelativeToMass && IsTowardsPoint == other.IsTowardsPoint && IsDirectional == other.IsDirectional;
	}

		public override int GetHashCode() {
			return Direction.GetHashCode() ^ Flag ^ (int) Strength;
	}
	}
}