using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace System.Graphics.Models {
	/// <summary>
	/// Represents a light source.
	/// </summary>
	[Serializable]
	public struct Light : IEquatable<Light> {
		/// <summary>
		/// The default color of the light emitted by the source.
		/// </summary>
		public static readonly Color4 DefaultLightHue = Color4.White;
		/// <summary>
		/// The default material hue and opacity.
		/// </summary>
		public static readonly Color4 DefaultMaterialHue = Color4.White;
		/// <summary>
		/// The default hue of the ambient light that hits the object.
		/// </summary>
		public static readonly Color4 DefaultAmbientHue = new Color4(1f, 0.15f, 0.15f, 0.15f);
		/// <summary>
		/// The default hue of the reflective shine of the object.
		/// </summary>
		public static readonly Color4 DefaultShineHue = new Color4(1f, 0.15f, 0.15f, 0.15f);
		/// <summary>
		/// The default shininess exponent of the material.
		/// </summary>
		public static readonly float DefaultShininess = 15f;

		/// <summary>
		/// Gets or sets the position of the light source.
		/// </summary>
		public Vector3 Position {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the color of the light emitted by the source.
		/// </summary>
		public Color4 Hue {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets whether the light source is a point light source, otherwise it is ambient.
		/// </summary>
		public bool PointLight {
			get;
			set;
		}

		/// <summary>
		/// Configures the light source.
		/// </summary>
		/// <param name="position">The position of the point light source.</param>
		public Light(Vector3 position) : this(position, DefaultLightHue, false) {
		}

		/// <summary>
		/// Configures the light source.
		/// </summary>
		/// <param name="position">The position of the point light source.</param>
		/// <param name="hue">The hue of the light emitted by the source.</param>
		public Light(Vector3 position, Color4 hue) : this(position, hue, false) {
		}

		/// <summary>
		/// Configures the light source.
		/// </summary>
		/// <param name="position">The position of the point light source.</param>
		/// <param name="hue">The hue of the light emitted by the source.</param>
		/// <param name="pointLight">Whether the light source is a point light source, otherwise it is ambient.</param>
		public Light(Vector3 position, Color4 hue, bool pointLight) {
			Position = position;
			Hue = hue;
			PointLight = pointLight;
		}

		/// <summary>
		/// Returns whether two light sources are equal.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator ==(Light left, Light right) {
			return left.Equals(right);
		}

		/// <summary>
		/// Returns whether two light sources are not equal.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator !=(Light left, Light right) {
			return !left.Equals(right);
		}

		/// <summary>
		/// Returns whether the light sources are equal to another.
		/// </summary>
		/// <param name="obj">The light source to check for equality to.</param>
		/// <returns>Whether the light sources are equal to one another.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool Equals(Light obj) {
			return Position == obj.Position && Hue == obj.Hue;
		}

		/// <summary>
		/// Returns whether the light sources are equal to another.
		/// </summary>
		/// <param name="obj">The light source to check for equality to.</param>
		/// <returns>Whether the light sources are equal to one another.</returns>
		public override bool Equals(object obj) {
			return obj is Light ? Equals((Light) obj) : false;
		}

		/// <summary>
		/// Gets a hash code for this instance.
		/// </summary>
		public override int GetHashCode() {
			return unchecked(Position.GetHashCode() << 9 ^ Hue.GetHashCode());
		}

		/// <summary>
		/// Gets a string that represents this light source.
		/// </summary>
		public override string ToString() {
			return "{" + Hue + ", " + Position.ToString() + "}";
		}
	}
}