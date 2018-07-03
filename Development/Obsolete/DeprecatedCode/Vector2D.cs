using System.Runtime.InteropServices;

namespace System.Drawing {
	/// <summary>Represents a 2D vector using two double-precision floating-point numbers.</summary>
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector2D : IEquatable<Vector2D> {
		#region Fields

		/// <summary>The X coordinate of this instance.</summary>
		public double X;

		/// <summary>The Y coordinate of this instance.</summary>
		public double Y;

		/// <summary>
		/// Defines a unit-length Vector2d that points towards the X-axis.
		/// </summary>
		public static readonly Vector2D UnitX = new Vector2D(1D, 0D);

		/// <summary>
		/// Defines a unit-length Vector2d that points towards the Y-axis.
		/// </summary>
		public static readonly Vector2D UnitY = new Vector2D(0D, 1D);

		/// <summary>
		/// Defines a zero-length Vector2d.
		/// </summary>
		public static readonly Vector2D Zero = new Vector2D();

		/// <summary>
		/// Defines an instance with all components set to 1.
		/// </summary>
		public static readonly Vector2D One = new Vector2D(1D);

		/// <summary>
		/// Defines the size of the Vector2d struct in bytes.
		/// </summary>
		public static readonly int SizeInBytes = Marshal.SizeOf(new Vector2D());

		#endregion

		#region Constructors

		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		/// <param name="value">The value that will initialize this instance.</param>
		public Vector2D(double value) {
			X = value;
			Y = value;
		}

		/// <summary>Constructs left vector with the given coordinates.</summary>
		/// <param name="x">The X coordinate.</param>
		/// <param name="y">The Y coordinate.</param>
		public Vector2D(double x, double y) {
			this.X = x;
			this.Y = y;
		}

		#endregion

		#region Public Members

		#region Instance

		#region public double Length

		/// <summary>
		/// Gets the length (magnitude) of the vector.
		/// </summary>
		/// <seealso cref="LengthSquared"/>
		public double Length {
			get {
				return System.Math.Sqrt(X * X + Y * Y);
			}
		}

		#endregion

		#region public double LengthSquared

		/// <summary>
		/// Gets the square of the vector length (magnitude).
		/// </summary>
		/// <remarks>
		/// This property avoids the costly square root operation required by the Length property. This makes it more suitable
		/// for comparisons.
		/// </remarks>
		/// <see cref="Length"/>
		public double LengthSquared {
			get {
				return X * X + Y * Y;
			}
		}

		#endregion

		#region public Vector2d PerpendicularRight

		/// <summary>
		/// Gets the perpendicular vector on the right side of this vector.
		/// </summary>
		public Vector2D PerpendicularRight {
			get {
				return new Vector2D(Y, -X);
			}
		}

		#endregion

		#region public Vector2d PerpendicularLeft

		/// <summary>
		/// Gets the perpendicular vector on the left side of this vector.
		/// </summary>
		public Vector2D PerpendicularLeft {
			get {
				return new Vector2D(-Y, X);
			}
		}

		#endregion

		#region public void Normalize()

		/// <summary>
		/// Scales the Vector2 to unit length.
		/// </summary>
		public void Normalize() {
			if (X == 0.0 && Y == 0.0)
				return;
			double scale = 1.0 / Length;
			X *= scale;
			Y *= scale;
		}

		#endregion

		#endregion

		#region Static

		/// <summary>
		/// Adds a scalar to a vector.
		/// </summary>
		/// <param name="a">Left operand.</param>
		/// <param name="b">Right operand.</param>
		/// <returns>Result of operation.</returns>
		public static Vector2D AddScalar(Vector2D a, double b) {
			a.X += b;
			a.Y += b;
			return a;
		}

		/// <summary>
		/// Adds a scalar to a vector.
		/// </summary>
		/// <param name="a">Left operand.</param>
		/// <param name="b">Right operand.</param>
		/// <returns>Result of operation.</returns>
		public static Vector2D AddScalar(double b, Vector2D a) {
			a.X += b;
			a.Y += b;
			return a;
		}

		/// <summary>
		/// Multiplies a scalar to a vector.
		/// </summary>
		/// <param name="a">Left operand.</param>
		/// <param name="b">Right operand.</param>
		/// <returns>Result of operation.</returns>
		public static Vector2D MultScalar(Vector2D a, double b) {
			a.X *= b;
			a.Y *= b;
			return a;
		}

		/// <summary>
		/// Multiplies a scalar to a vector.
		/// </summary>
		/// <param name="a">Left operand.</param>
		/// <param name="b">Right operand.</param>
		/// <returns>Result of operation.</returns>
		public static Vector2D MultScalar(double b, Vector2D a) {
			a.X *= b;
			a.Y *= b;
			return a;
		}

		/// <summary>
		/// Divides a vector by a scalar.
		/// </summary>
		/// <param name="a">Left operand.</param>
		/// <param name="b">Right operand.</param>
		/// <returns>Result of operation.</returns>
		public static Vector2D DivScalar(Vector2D a, double b) {
			a.X /= b;
			a.Y /= b;
			return a;
		}

		#region Add

		/// <summary>
		/// Adds two vectors.
		/// </summary>
		/// <param name="a">Left operand.</param>
		/// <param name="b">Right operand.</param>
		/// <returns>Result of operation.</returns>
		public static Vector2D Add(Vector2D a, Vector2D b) {
			Add(ref a, ref b, out a);
			return a;
		}

		/// <summary>
		/// Adds two vectors.
		/// </summary>
		/// <param name="a">Left operand.</param>
		/// <param name="b">Right operand.</param>
		/// <param name="result">Result of operation.</param>
		public static void Add(ref Vector2D a, ref Vector2D b, out Vector2D result) {
			result = new Vector2D(a.X + b.X, a.Y + b.Y);
		}

		#endregion

		#region Subtract

		/// <summary>
		/// Subtract one Vector2D from another
		/// </summary>
		/// <param name="a">First operand</param>
		/// <param name="b">Second operand</param>
		/// <returns>Result of subtraction</returns>
		public static Vector2D Subtract(Vector2D a, Vector2D b) {
			Subtract(ref a, ref b, out a);
			return a;
		}

		/// <summary>
		/// Subtract one Vector2D from another
		/// </summary>
		/// <param name="a">First operand</param>
		/// <param name="b">Second operand</param>
		/// <param name="result">Result of subtraction</param>
		public static void Subtract(ref Vector2D a, ref Vector2D b, out Vector2D result) {
			result = new Vector2D(a.X - b.X, a.Y - b.Y);
		}

		#endregion

		#region Multiply

		/// <summary>
		/// Multiplies a vector by a scalar.
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <returns>Result of the operation.</returns>
		public static Vector2D Multiply(Vector2D vector, double scale) {
			Multiply(ref vector, scale, out vector);
			return vector;
		}

		/// <summary>
		/// Multiplies a vector by a scalar.
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <param name="result">Result of the operation.</param>
		public static void Multiply(ref Vector2D vector, double scale, out Vector2D result) {
			result = new Vector2D(vector.X * scale, vector.Y * scale);
		}

		/// <summary>
		/// Multiplies a vector by the components a vector (scale).
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <returns>Result of the operation.</returns>
		public static Vector2D Multiply(Vector2D vector, Vector2D scale) {
			Multiply(ref vector, ref scale, out vector);
			return vector;
		}

		/// <summary>
		/// Multiplies a vector by the components of a vector (scale).
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <param name="result">Result of the operation.</param>
		public static void Multiply(ref Vector2D vector, ref Vector2D scale, out Vector2D result) {
			result = new Vector2D(vector.X * scale.X, vector.Y * scale.Y);
		}

		#endregion

		#region Divide

		/// <summary>
		/// Divides a vector by a scalar.
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <returns>Result of the operation.</returns>
		public static Vector2D Divide(Vector2D vector, double scale) {
			Divide(ref vector, scale, out vector);
			return vector;
		}

		/// <summary>
		/// Divides a vector by a scalar.
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <param name="result">Result of the operation.</param>
		public static void Divide(ref Vector2D vector, double scale, out Vector2D result) {
			Multiply(ref vector, 1 / scale, out result);
		}

		/// <summary>
		/// Divides a vector by the components of a vector (scale).
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <returns>Result of the operation.</returns>
		public static Vector2D Divide(Vector2D vector, Vector2D scale) {
			Divide(ref vector, ref scale, out vector);
			return vector;
		}

		/// <summary>
		/// Divide a vector by the components of a vector (scale).
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <param name="result">Result of the operation.</param>
		public static void Divide(ref Vector2D vector, ref Vector2D scale, out Vector2D result) {
			result = new Vector2D(vector.X / scale.X, vector.Y / scale.Y);
		}

		#endregion

		#region Min

		/// <summary>
		/// Calculate the component-wise minimum of two vectors
		/// </summary>
		/// <param name="a">First operand</param>
		/// <param name="b">Second operand</param>
		/// <returns>The component-wise minimum</returns>
		public static Vector2D Min(Vector2D a, Vector2D b) {
			a.X = a.X < b.X ? a.X : b.X;
			a.Y = a.Y < b.Y ? a.Y : b.Y;
			return a;
		}

		/// <summary>
		/// Calculate the component-wise minimum of two vectors
		/// </summary>
		/// <param name="a">First operand</param>
		/// <param name="b">Second operand</param>
		/// <param name="result">The component-wise minimum</param>
		public static void Min(ref Vector2D a, ref Vector2D b, out Vector2D result) {
			result.X = a.X < b.X ? a.X : b.X;
			result.Y = a.Y < b.Y ? a.Y : b.Y;
		}

		#endregion

		#region Max

		/// <summary>
		/// Calculate the component-wise maximum of two vectors
		/// </summary>
		/// <param name="a">First operand</param>
		/// <param name="b">Second operand</param>
		/// <returns>The component-wise maximum</returns>
		public static Vector2D Max(Vector2D a, Vector2D b) {
			a.X = a.X > b.X ? a.X : b.X;
			a.Y = a.Y > b.Y ? a.Y : b.Y;
			return a;
		}

		/// <summary>
		/// Calculate the component-wise maximum of two vectors
		/// </summary>
		/// <param name="a">First operand</param>
		/// <param name="b">Second operand</param>
		/// <param name="result">The component-wise maximum</param>
		public static void Max(ref Vector2D a, ref Vector2D b, out Vector2D result) {
			result.X = a.X > b.X ? a.X : b.X;
			result.Y = a.Y > b.Y ? a.Y : b.Y;
		}

		#endregion

		#region Clamp

		/// <summary>
		/// Clamp a vector to the given minimum and maximum vectors
		/// </summary>
		/// <param name="vec">Input vector</param>
		/// <param name="min">Minimum vector</param>
		/// <param name="max">Maximum vector</param>
		/// <returns>The clamped vector</returns>
		public static Vector2D Clamp(Vector2D vec, Vector2D min, Vector2D max) {
			vec.X = vec.X < min.X ? min.X : vec.X > max.X ? max.X : vec.X;
			vec.Y = vec.Y < min.Y ? min.Y : vec.Y > max.Y ? max.Y : vec.Y;
			return vec;
		}

		/// <summary>
		/// Clamp a vector to the given minimum and maximum vectors
		/// </summary>
		/// <param name="vec">Input vector</param>
		/// <param name="min">Minimum vector</param>
		/// <param name="max">Maximum vector</param>
		/// <param name="result">The clamped vector</param>
		public static void Clamp(ref Vector2D vec, ref Vector2D min, ref Vector2D max, out Vector2D result) {
			result.X = vec.X < min.X ? min.X : vec.X > max.X ? max.X : vec.X;
			result.Y = vec.Y < min.Y ? min.Y : vec.Y > max.Y ? max.Y : vec.Y;
		}

		#endregion

		#region Normalize

		/// <summary>
		/// Scale a vector to unit length
		/// </summary>
		/// <param name="vec">The input vector</param>
		/// <returns>The normalized vector</returns>
		public static Vector2D Normalize(Vector2D vec) {
			if (vec == Zero)
				return Zero;
			double scale = 1.0 / vec.Length;
			vec.X *= scale;
			vec.Y *= scale;
			return vec;
		}

		#endregion

		#region Dot

		/// <summary>
		/// Calculate the dot (scalar) product of two vectors
		/// </summary>
		/// <param name="left">First operand</param>
		/// <param name="right">Second operand</param>
		/// <returns>The dot product of the two inputs</returns>
		public static double Dot(Vector2D left, Vector2D right) {
			return left.X * right.X + left.Y * right.Y;
		}

		/// <summary>
		/// Calculate the dot (scalar) product of two vectors
		/// </summary>
		/// <param name="left">First operand</param>
		/// <param name="right">Second operand</param>
		/// <param name="result">The dot product of the two inputs</param>
		public static void Dot(ref Vector2D left, ref Vector2D right, out double result) {
			result = left.X * right.X + left.Y * right.Y;
		}

		#endregion

		#region Lerp

		/// <summary>
		/// Returns a new Vector2D that is the linear blend of the 2 given Vectors
		/// </summary>
		/// <param name="a">First input vector</param>
		/// <param name="b">Second input vector</param>
		/// <param name="blend">The blend factor. a when blend=0, b when blend=1.</param>
		/// <returns>a when blend=0, b when blend=1, and a linear combination otherwise</returns>
		public static Vector2D Lerp(Vector2D a, Vector2D b, double blend) {
			a.X = blend * (b.X - a.X) + a.X;
			a.Y = blend * (b.Y - a.Y) + a.Y;
			return a;
		}

		/// <summary>
		/// Returns a new Vector2D that is the linear blend of the 2 given Vectors
		/// </summary>
		/// <param name="a">First input vector</param>
		/// <param name="b">Second input vector</param>
		/// <param name="blend">The blend factor. a when blend=0, b when blend=1.</param>
		/// <param name="result">a when blend=0, b when blend=1, and a linear combination otherwise</param>
		public static void Lerp(ref Vector2D a, ref Vector2D b, double blend, out Vector2D result) {
			result.X = blend * (b.X - a.X) + a.X;
			result.Y = blend * (b.Y - a.Y) + a.Y;
		}

		#endregion

		#region Barycentric

		/// <summary>
		/// Interpolate 3 Vectors using Barycentric coordinates
		/// </summary>
		/// <param name="a">First input Vector2D</param>
		/// <param name="b">Second input Vector2D</param>
		/// <param name="c">Third input Vector2D</param>
		/// <param name="u">First Barycentric Coordinate</param>
		/// <param name="v">Second Barycentric Coordinate</param>
		/// <returns>a when u=v=0, b when u=1,v=0, c when u=0,v=1, and a linear combination of a,b,c otherwise</returns>
		public static Vector2D BaryCentric(Vector2D a, Vector2D b, Vector2D c, double u, double v) {
			return a + u * (b - a) + v * (c - a);
		}

		/// <summary>Interpolate 3 Vectors using Barycentric coordinates</summary>
		/// <param name="a">First input Vector2D.</param>
		/// <param name="b">Second input Vector2D.</param>
		/// <param name="c">Third input Vector2D.</param>
		/// <param name="u">First Barycentric Coordinate.</param>
		/// <param name="v">Second Barycentric Coordinate.</param>
		/// <param name="result">Output Vector2D. a when u=v=0, b when u=1,v=0, c when u=0,v=1, and a linear combination of a,b,c otherwise</param>
		public static void BaryCentric(ref Vector2D a, ref Vector2D b, ref Vector2D c, double u, double v, out Vector2D result) {
			result = a; // copy

			Vector2D temp = b; // copy
			Subtract(ref temp, ref a, out temp);
			Multiply(ref temp, u, out temp);
			Add(ref result, ref temp, out result);

			temp = c; // copy
			Subtract(ref temp, ref a, out temp);
			Multiply(ref temp, v, out temp);
			Add(ref result, ref temp, out result);
		}

		#endregion

		#region Transform

		/// <summary>
		/// Returns the cross product of the specified vector or point.
		/// </summary>
		/// <param name="v">The vector to calculate the cross-product with.</param>
		public double CrossProduct(Vector2D v) {
			return (X * v.Y) - (Y * v.X);
		}

		/// <summary>
		/// Returns the distance between two points.
		/// </summary>
		/// <param name="v">The vector to calculate the distance from.</param>
		public double DistanceFrom(Vector2D v) {
			return (this - v).Length;
		}

		/// <summary>
		/// Returns the cross product of the specified vector or point.
		/// </summary>
		/// <param name="vector">The vector to calculate the cross-product with.</param>
		/// <param name="v">The vector to calculate the cross-product with.</param>
		public static double CrossProduct(Vector2D vector, Vector2D v) {
			return (vector.X * v.Y) - (vector.Y * v.X);
		}

		/// <summary>
		/// Returns the distance between two points.
		/// </summary>
		/// <param name="a">The starting point.</param>
		/// <param name="b">The vector to calculate the distance from.</param>
		public static double DistanceBetween(Vector2D a, Vector2D b) {
			return (a - b).Length;
		}

		/// <summary>
		/// Places the point about the specified axis at the specified angle.
		/// </summary>
		/// <param name="vector">The vector to set the angle with respect to it.</param>
		/// <param name="axis">The center point to use as origin.</param>
		/// <param name="angle">The angle to set in relation to the vector (not cumulative).</param>
		public static Vector2D SetAngleFrom(Vector2D vector, Vector2D axis, double angle) {
			return axis + ToMoveAt(vector.DistanceFrom(axis), angle);
		}

		/// <summary>
		/// Rotates the point about the specified axis at the specified angle.
		/// </summary>
		/// <param name="vector">The vector to rotate.</param>
		/// <param name="axis">The center point of rotation.</param>
		/// <param name="angle">The cumulative angle of rotation.</param>
		public static Vector2D RotateAbout(Vector2D vector, Vector2D axis, double angle) {
			return axis + ToMoveAt(vector.DistanceFrom(axis), axis.GetAngleFrom(vector) + angle);
		}

		/// <summary>
		/// Gets the current angle from another point in Radians.
		/// </summary>
		/// <param name="v">The point to get angle with respect to it.</param>
		public double GetAngleFrom(Vector2D v) {
			return Math.Atan2(v.Y - Y, v.X - X);
		}

		/// <summary>
		/// Gets the current angle from another point in Radians.
		/// </summary>
		/// <param name="v">A point.</param>
		/// <param name="axis">The axis point to get rotation from.</param>
		public static double GetAngleFrom(Vector2D v, Vector2D axis) {
			return Math.Atan2(v.Y - axis.Y, v.X - axis.X);
		}

		/// <summary>
		/// Moves point at the specified angle (measured in Radians) at the specified distance.
		/// </summary>
		/// <param name="distance">The distance to move.</param>
		/// <param name="angle">The angle to move.</param>
		public void MoveAt(double distance, double angle) {
			Vector2D v = ToMoveAt(distance, angle);
			X += v.X;
			Y += v.Y;
		}

		/// <summary>
		/// Moves point at the specified angle (measured in Radians) at the specified distance.
		/// </summary>
		/// <param name="vector">The point to move.</param>
		/// <param name="distance">The distance to move.</param>
		/// <param name="angle">The angle to move.</param>
		public static Vector2D MoveAt(Vector2D vector, double distance, double angle) {
			Vector2D v = ToMoveAt(distance, angle);
			return new Vector2D(vector.X + v.X, vector.Y + v.Y);
		}

		/// <summary>
		/// Returns the movement required for the point to move at the specified distance and angle.
		/// </summary>
		/// <param name="distance">The distance to move at.</param>
		/// <param name="angle">The angle to move at</param>
		public static Vector2D ToMoveAt(double distance, double angle) {
			return new Vector2D(Math.Cos(angle) * distance, Math.Sin(angle) * distance);
		}

		/// <summary>
		/// Moves the point towards the specified point for the specified distance.
		/// </summary>
		/// <param name="v">The point to move towards.</param>
		/// <param name="distance">The distance to move.</param>
		public void MoveTowards(Vector2D v, double distance) {
			MoveAt(distance, GetAngleFrom(v));
		}

		/// <summary>
		/// Moves the point towards the specified point for the specified distance.
		/// </summary>
		/// <param name="vector">The vector to move.</param>
		/// <param name="point">The point to move towards.</param>
		/// <param name="distance">The distance to move.</param>
		public static Vector2D MoveTowards(Vector2D vector, Vector2D point, double distance) {
			vector.MoveAt(distance, vector.GetAngleFrom(point));
			return vector;
		}

		/// <summary>
		/// Returns the movement required to move the point the specified distance towards the specified point.
		/// </summary>
		/// <param name="v">The point to move towards.</param>
		/// <param name="distance">The distance to move at.</param>
		public Vector2D DirectionTowards(Vector2D v, double distance) {
			return ToMoveAt(distance, GetAngleFrom(v));
		}

		/// <summary>
		/// Moves the point away from the specified point at the specified distance.
		/// </summary>
		/// <param name="v">The point to move away from.</param>
		/// <param name="distance">The distance to move.</param>
		public void MoveAwayFrom(Vector2D v, double distance) {
			MoveAt(distance, v.GetAngleFrom(this));
		}

		/// <summary>
		/// Moves the point away from the specified point at the specified distance.
		/// </summary>
		/// <param name="vector">The vector to move.</param>
		/// <param name="point">The point to move away from.</param>
		/// <param name="distance">The distance to move.</param>
		public static Vector2D MoveAway(Vector2D vector, Vector2D point, double distance) {
			vector.MoveAt(distance, point.GetAngleFrom(vector));
			return vector;
		}

		/// <summary>
		/// Returns the movement required to move the point the specified distance away from the specified point.
		/// </summary>
		/// <param name="v">The point to move towards.</param>
		/// <param name="distance">The distance to move at.</param>
		public Vector2D DirectionAwayFrom(Vector2D v, double distance) {
			return ToMoveAt(distance, v.GetAngleFrom(this));
		}

		/// <summary>
		/// Gets the point on the circumference of an ellipse that is located at the specified angle from the center.
		/// </summary>
		/// <param name="center">The center point of the ellipse.</param>
		/// <param name="radii">The radii of the ellipse (width and height).</param>
		/// <param name="ellipseRotation">The angle of rotation of the ellipse itself.</param>
		/// <param name="angle">The angle the point to obtain on the circumference makes with the center.</param>
		public static Vector2D GetPointOnEllipseFromAngle(Vector2D center, Vector2D radii, double ellipseRotation, double angle) {
			if (Math.Abs(radii.X - radii.Y) <= float.Epsilon)
				return center + SetAngleFrom(radii, Zero, angle);
			else {
				angle = angle % MathHelper.TwoPiD;
				if (angle >= Math.PI)
					angle -= MathHelper.TwoPiD;
				double t = Math.Atan(radii.X * Math.Tan(angle) / radii.Y);
				if (angle >= MathHelper.PiOver2D && angle < Math.PI)
					t += Math.PI;
				else if (angle >= -Math.PI && angle < -MathHelper.PiOver2D)
					t -= Math.PI;
				Vector2D resultant = new Vector2D(radii.X * Math.Cos(t), radii.Y * Math.Sin(t));
				return center + (ellipseRotation % MathHelper.TwoPiD == 0.0 ? resultant : RotateAbout(resultant, Zero, ellipseRotation));
			}
		}

		#endregion

		#endregion

		#region Operators

		/// <summary>
		/// Adds two instances.
		/// </summary>
		/// <param name="left">The left instance.</param>
		/// <param name="right">The right instance.</param>
		/// <returns>The result of the operation.</returns>
		public static Vector2D operator +(Vector2D left, Vector2D right) {
			left.X += right.X;
			left.Y += right.Y;
			return left;
		}

		/// <summary>
		/// Subtracts two instances.
		/// </summary>
		/// <param name="left">The left instance.</param>
		/// <param name="right">The right instance.</param>
		/// <returns>The result of the operation.</returns>
		public static Vector2D operator -(Vector2D left, Vector2D right) {
			left.X -= right.X;
			left.Y -= right.Y;
			return left;
		}

		/// <summary>
		/// Negates an instance.
		/// </summary>
		/// <param name="vec">The instance.</param>
		/// <returns>The result of the operation.</returns>
		public static Vector2D operator -(Vector2D vec) {
			vec.X = -vec.X;
			vec.Y = -vec.Y;
			return vec;
		}

		/// <summary>
		/// Multiplies an instance by a scalar.
		/// </summary>
		/// <param name="vec">The instance.</param>
		/// <param name="f">The scalar.</param>
		/// <returns>The result of the operation.</returns>
		public static Vector2D operator *(Vector2D vec, double f) {
			vec.X *= f;
			vec.Y *= f;
			return vec;
		}

		/// <summary>
		/// Multiply an instance by a scalar.
		/// </summary>
		/// <param name="f">The scalar.</param>
		/// <param name="vec">The instance.</param>
		/// <returns>The result of the operation.</returns>
		public static Vector2D operator *(double f, Vector2D vec) {
			vec.X *= f;
			vec.Y *= f;
			return vec;
		}

		/// <summary>
		/// Divides an instance by a scalar.
		/// </summary>
		/// <param name="vec">The instance.</param>
		/// <param name="f">The scalar.</param>
		/// <returns>The result of the operation.</returns>
		public static Vector2D operator /(Vector2D vec, double f) {
			double mult = 1.0 / f;
			vec.X *= mult;
			vec.Y *= mult;
			return vec;
		}

		/// <summary>
		/// Compares two instances for equality.
		/// </summary>
		/// <param name="left">The left instance.</param>
		/// <param name="right">The right instance.</param>
		/// <returns>True, if both instances are equal; false otherwise.</returns>
		public static bool operator ==(Vector2D left, Vector2D right) {
			return left.Equals(right);
		}

		/// <summary>
		/// Compares two instances for ienquality.
		/// </summary>
		/// <param name="left">The left instance.</param>
		/// <param name="right">The right instance.</param>
		/// <returns>True, if the instances are not equal; false otherwise.</returns>
		public static bool operator !=(Vector2D left, Vector2D right) {
			return !left.Equals(right);
		}


		/// <summary>
		/// Casts a Vector2D to PointF
		/// </summary>
		/// <param name="v">The vector to cast</param>
		public static explicit operator PointF(Vector2D v) {
			return new PointF((float) v.X, (float) v.Y);
		}

		/// <summary>
		/// Casts a PointF to Vector2D
		/// </summary>
		/// <param name="v">The point to cast</param>
		public static explicit operator Vector2D(PointF v) {
			return new Vector2D(v.X, v.Y);
		}

		/// <summary>
		/// Casts a Vector2D to Point
		/// </summary>
		/// <param name="v">The vector to cast</param>
		public static explicit operator Point(Vector2D v) {
			return new Point((int) v.X, (int) v.Y);
		}

		/// <summary>
		/// Casts a Point to Vector2D
		/// </summary>
		/// <param name="v">The point to cast</param>
		public static explicit operator Vector2D(Point v) {
			return new Vector2D(v.X, v.Y);
		}

		#endregion

		#region Overrides

		#region public override string ToString()

		/// <summary>
		/// Returns a System.String that represents the current instance.
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return String.Format("({0}, {1})", X, Y);
		}

		#endregion

		#region public override int GetHashCode()

		/// <summary>
		/// Returns the hashcode for this instance.
		/// </summary>
		/// <returns>A System.Int32 containing the unique hashcode for this instance.</returns>
		public override int GetHashCode() {
			return X.GetHashCode() ^ Y.GetHashCode();
		}

		#endregion

		#region public override bool Equals(object obj)

		/// <summary>
		/// Indicates whether this instance and a specified object are equal.
		/// </summary>
		/// <param name="obj">The object to compare to.</param>
		/// <returns>True if the instances are equal; false otherwise.</returns>
		public override bool Equals(object obj) {
			if (!(obj is Vector2D))
				return false;

			return this.Equals((Vector2D) obj);
		}

		#endregion

		#endregion

		#endregion

		#region IEquatable<Vector2d> Members

		/// <summary>Indicates whether the current vector is equal to another vector.</summary>
		/// <param name="other">A vector to compare with this vector.</param>
		/// <returns>true if the current vector is equal to the vector parameter; otherwise, false.</returns>
		public bool Equals(Vector2D other) {
			return
				X == other.X &&
				Y == other.Y;
		}

		#endregion
	}
}