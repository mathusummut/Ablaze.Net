namespace System {
	/// <summary>
	/// Holds a version number represented by a major version and a minor sub-version number.
	/// </summary>
	[Serializable]
	public struct MajorMinorVersion : IComparable<MajorMinorVersion>, IEquatable<MajorMinorVersion> {
		/// <summary>
		/// The major version.
		/// </summary>
		public int Major;
		/// <summary>
		/// The minor sub-version.
		/// </summary>
		public int Minor;

		/// <summary>
		/// Initializes a new version representation.
		/// </summary>
		/// <param name="major">The major version number.</param>
		public MajorMinorVersion(int major) {
			Major = major;
			Minor = 0;
		}

		/// <summary>
		/// Initializes a new version representation.
		/// </summary>
		/// <param name="major">The major version number.</param>
		/// <param name="minor">The minor sub-version.</param>
		public MajorMinorVersion(int major, int minor) {
			Major = major;
			Minor = minor;
		}

		/// <summary>
		/// Gets a hash code for this instance.
		/// </summary>
		public override int GetHashCode() {
			return unchecked(Major << 16 + Minor);
		}

		/// <summary>
		/// Gets a string that represents this instance.
		/// </summary>
		public override string ToString() {
			return "v" + Major + "." + Minor;
		}

		/// <summary>
		/// Returns whether the specified versions are considered equal.
		/// </summary>
		/// <param name="v1">The first version.</param>
		/// <param name="v2">The second version.</param>
		public static bool operator ==(MajorMinorVersion v1, MajorMinorVersion v2) {
			return v1.Major == v2.Major && v1.Minor == v2.Minor;
		}

		/// <summary>
		/// Returns whether the specified versions are considered unequal.
		/// </summary>
		/// <param name="v1">The first version.</param>
		/// <param name="v2">The second version.</param>
		public static bool operator !=(MajorMinorVersion v1, MajorMinorVersion v2) {
			return !(v1.Major == v2.Major && v1.Minor == v2.Minor);
		}

		/// <summary>
		/// Returns whether the first version is considered older than the second.
		/// </summary>
		/// <param name="v1">The first version.</param>
		/// <param name="v2">The second version.</param>
		public static bool operator <(MajorMinorVersion v1, MajorMinorVersion v2) {
			return v1.CompareTo(v2) < 0;
		}

		/// <summary>
		/// Returns whether the first version is considered older or equal to the second.
		/// </summary>
		/// <param name="v1">The first version.</param>
		/// <param name="v2">The second version.</param>
		public static bool operator <=(MajorMinorVersion v1, MajorMinorVersion v2) {
			return v1.CompareTo(v2) <= 0;
		}

		/// <summary>
		/// Returns whether the first version is considered more recent than the second.
		/// </summary>
		/// <param name="v1">The first version.</param>
		/// <param name="v2">The second version.</param>
		public static bool operator >(MajorMinorVersion v1, MajorMinorVersion v2) {
			return v1.CompareTo(v2) > 0;
		}

		/// <summary>
		/// Returns whether the first version is considered more recent or equal to the second.
		/// </summary>
		/// <param name="v1">The first version.</param>
		/// <param name="v2">The second version.</param>
		public static bool operator >=(MajorMinorVersion v1, MajorMinorVersion v2) {
			return v1.CompareTo(v2) >= 0;
		}

		/// <summary>
		/// Returns whether the version is equal to this version.
		/// </summary>
		/// <param name="version">The version to compare with.</param>
		public bool Equals(MajorMinorVersion version) {
			return Major == version.Major && Minor == version.Minor;
		}

		/// <summary>
		/// Returns whether the version is equal to this version.
		/// </summary>
		/// <param name="obj">The object to compare with.</param>
		public override bool Equals(object obj) {
			return obj is MajorMinorVersion ? Equals((MajorMinorVersion) obj) : false;
		}

		/// <summary>
		/// Compares the current version with the specified version.
		/// </summary>
		/// <param name="version">The version to compare with.</param>
		public int CompareTo(MajorMinorVersion version) {
			int comparison = Major.CompareTo(version.Major);
			return comparison == 0 ? Minor.CompareTo(version.Minor) : comparison;
		}
	}
}