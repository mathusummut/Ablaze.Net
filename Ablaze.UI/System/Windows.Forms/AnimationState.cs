namespace System.Windows.Forms {
	/// <summary>
	/// Represents the current state of an animation.
	/// </summary>
	public enum AnimationState {
		/// <summary>
		/// The default value before the animation has even started.
		/// </summary>
		NotYetBegun = 0,
		/// <summary>
		/// The field was modified unexpectedly during the animation process.
		/// </summary>
		ValueDidntUpdateAsExpected,
		/// <summary>
		/// The field was updated successfully to the next value (not used by the AnimationFinished event).
		/// </summary>
		UpdateSuccess,
		/// <summary>
		/// The animation is completed successfully.
		/// </summary>
		Completed,
		/// <summary>
		/// The animation was halted, either because Halt() was called or possibly because the value was modified from
		/// outside the animation or the value stayed unchanged after update.
		/// </summary>
		Halted
	}
}