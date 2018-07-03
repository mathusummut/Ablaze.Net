namespace System.Windows.Forms {
	/// <summary>
	/// Specifies which child controls to skip when searching for controls at a point.
	/// </summary>
	[Flags]
	public enum ChildSkip {
		/// <summary>
		/// Do not skip any child controls.
		/// </summary>
		None = 0,
		/// <summary>
		/// Skip invisible child windows.
		/// </summary>
		Invisible = 1,
		/// <summary>
		/// Skip disabled child windows.
		/// </summary>
		Disabled = 2,
		/// <summary>
		/// Skip transparent child windows.
		/// </summary>
		Transparent = 4,
		/// <summary>
		/// Skip controls that don't respond to mouse clicks (return HTTRANPARENT when sent WM_NCHITTEST).
		/// </summary>
		DontRespondToMouse = 8
	}

	/// <summary>
	/// Specifies the dispose options for the form.
	/// </summary>
	public enum DisposeOptions {
		/// <summary>
		/// Does not dispose anything.
		/// </summary>
		None = 0,
		/// <summary>
		/// Disposes of the form only.
		/// </summary>
		FormOnly = 1,
		/// <summary>
		/// Disposes of every resource used by the form as well. Only use this if you want to kill the form completely.
		/// </summary>
		FullDisposal = 2
	}

	/// <summary>
	/// Contains the options flag that is used to specify form disposal mode.
	/// </summary>
	public class DisposeFormEventArgs : EventArgs {
		/// <summary>
		/// The options flag that is used to specify form disposal mode.
		/// </summary>
		public DisposeOptions DisposeMode;

		/// <summary>
		/// Initializes a new DisposeFormEventArgs instance.
		/// </summary>
		/// <param name="disposeMode">The options flag that is used to specify form disposal mode.</param>
		public DisposeFormEventArgs(DisposeOptions disposeMode) {
			DisposeMode = disposeMode;
		}
	}
}