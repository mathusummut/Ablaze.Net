using System.Platforms.Windows;
using System.Runtime.CompilerServices;

namespace System.Windows.Forms {
	/// <summary>
	/// Represents a global hotkey.
	/// </summary>
	[Serializable]
	public struct Hotkey : IEquatable<Hotkey> {
		/// <summary>
		/// Represents an empty hotkey.
		/// </summary>
		public static readonly Hotkey Empty = new Hotkey();
		/// <summary>
		/// The key that triggers the hotkey if accompanied by the assigned modifiers.
		/// </summary>
		public readonly Keys Key;
		/// <summary>
		/// The modifiers that must accompany the key in order to trigger the hotkey.
		/// </summary>
		public readonly KeyModifiers Modifiers;
		/// <summary>
		/// Used for assigning an ID to the hotkey (or leave 0 if not used). Hotkeys that are
		/// the same should have the same ID.
		/// </summary>
		public int ID;

		/// <summary>
		/// Gets the true hash of the hotkey.
		/// </summary>
		[CLSCompliant(false)]
		public uint TrueHash {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return ((uint) Key << 16) | (uint) Modifiers;
			}
		}

		/// <summary>
		/// Initializes a hotkey from the specified parameters.
		/// </summary>
		/// <param name="key">The key that triggers the hotkey if accompanied by the assigned modifiers.</param>
		/// <param name="modifiers">The modifiers that must accompany the key in order to trigger the hotkey.</param>
		public Hotkey(Keys key, KeyModifiers modifiers) {
			Key = key;
			Modifiers = modifiers;
			ID = 0;
		}

		/// <summary>
		/// Initializes a hotkey from the specified true hash.
		/// </summary>
		/// <param name="trueHash">The true hash of the hotkey.</param>
		[CLSCompliant(false)]
		public Hotkey(uint trueHash) {
			Key = (Keys) ((trueHash & 0xffff0000) >> 16);
			Modifiers = (KeyModifiers) (trueHash & 0x0000ffff);
			ID = 0;
		}

		/// <summary>
		/// Clones the specified hotkey.
		/// </summary>
		/// <param name="key">The hotkey to clone.</param>
		public Hotkey(Hotkey key) {
			Key = key.Key;
			Modifiers = key.Modifiers;
			ID = key.ID;
		}

		/// <summary>
		/// Returns whether this hotkey instance and the object are considered equal.
		/// </summary>
		/// <param name="obj">The object to compare with.</param>
		public override bool Equals(object obj) {
			return obj is Hotkey ? Equals((Hotkey) obj) : false;
		}

		/// <summary>
		/// Returns whether this hotkey is equal to the specified hotkey.
		/// </summary>
		/// <param name="key">The hotkey to compare with.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool Equals(Hotkey key) {
			return ID == 0 || key.ID == 0 ? (Key == key.Key && (Modifiers | KeyModifiers.NoRepeat) == (key.Modifiers | KeyModifiers.NoRepeat)) : (ID == key.ID);
		}

		/// <summary>
		/// Returns whether two hotkeys are considered equal.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator ==(Hotkey a, Hotkey b) {
			return a.Equals(b);
		}

		/// <summary>
		/// Returns whether two hotkeys are considered different.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator !=(Hotkey a, Hotkey b) {
			return !a.Equals(b);
		}

		/// <summary>
		/// Gets the hash code of the hotkey.
		/// </summary>
		public override int GetHashCode() {
			return ID == 0 ? unchecked((int) TrueHash) : ID;
		}

		/// <summary>
		/// Gets a string that represents the hotkey.
		/// </summary>
		public override string ToString() {
			return Modifiers.ToString().Replace(" ", string.Empty).Replace(',', '+').Replace("+NoRepeat", string.Empty) + "+" + Key.ToString().Replace(" ", string.Empty).Replace(',', '+');
		}
	}
}