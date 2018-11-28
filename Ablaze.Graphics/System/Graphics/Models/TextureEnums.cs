namespace System.Graphics.Models {
	/// <summary>
	/// Specifies which operation to perform on Non-Power-Of-Two-sized textures
	/// </summary>
	public enum NPotTextureScaleMode {
		/// <summary>
		/// Performs no operation (allows non-power of two textures to be used)
		/// </summary>
		None = 0,
		/// <summary>
		/// Stretches the texture to dimensions that are a power of two and larger or equal than the current texture dimensions
		/// </summary>
		ScaleUp,
		/// <summary>
		/// Stretches the texture to dimensions that are a power of two and smaller or equal than the current texture dimensions
		/// </summary>
		ScaleDown,
		/// <summary>
		/// Pads the texture with zeros until its dimensions become a power of two
		/// </summary>
		Pad
	}
}