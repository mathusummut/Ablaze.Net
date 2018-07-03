using System.Platforms;
using System.Runtime.CompilerServices;

namespace System.Graphics {
	/// <summary>
	/// The graphics component that creates and manages all the graphics contexts assigned to a window.
	/// </summary>
	public class GraphicsComponent : IDisposable {
		/// <summary>
		/// The window that is associated with this graphics component.
		/// </summary>
		public readonly Window Window;
		/// <summary>
		/// The requested GraphicsMode of the graphics component.
		/// </summary>
		public GraphicsMode GraphicsMode;
		/// <summary>
		/// The requested OpenGL version.
		/// </summary>
		public MajorMinorVersion Version;

		/// <summary>
		/// Gets the handle of the control to which the graphics component is assigned.
		/// </summary>
		public IntPtr Control {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return Window.Handle;
			}
		}

		/// <summary>
		/// Gets whether the graphics component is disposed.
		/// </summary>
		public bool IsDisposed {
			get;
			private set;
		}

		/// <summary>
		/// Creates a new GraphicsComponent on the specified control with the default graphics mode.
		/// </summary>
		/// <param name="control">The handle of a control to create a GraphicsComponent for.</param>
		public GraphicsComponent(IntPtr control) : this(control, GraphicsMode.Default) {
		}

		/// <summary>
		/// Creates a new GraphicsComponent on the specified control.
		/// </summary>
		/// <param name="control">The handle of a control to create a GraphicsComponent for.</param>
		/// <param name="mode">The graphics mode of the component.</param>
		public GraphicsComponent(IntPtr control, GraphicsMode mode) {
			if (mode == GraphicsMode.Empty)
				mode = GraphicsMode.Default;
			GraphicsMode = mode;
			Window = GraphicsPlatform.Factory.GetWindowInfo(control, mode);
			Version = new MajorMinorVersion(1);
		}

		/// <summary>
		/// Creates a new GraphicsComponent on the specified control.
		/// </summary>
		/// <param name="control">The handle of a control to create a GraphicsComponent for.</param>
		/// <param name="mode">The graphics mode of the component.</param>
		/// <param name="version">The requested OpenGL version.</param>
		public GraphicsComponent(IntPtr control, GraphicsMode mode, MajorMinorVersion version) {
			if (mode == GraphicsMode.Empty)
				mode = GraphicsMode.Default;
			GraphicsMode = mode;
			Window = GraphicsPlatform.Factory.GetWindowInfo(control, mode);
			if (version.Major < 1)
				version.Major = 1;
			if (version.Minor < 0)
				version.Minor = 0;
			Version = version;
		}

		/// <summary>
		/// Creates a GraphicsContext on the control.
		/// </summary>
		/// <param name="shareContext">The graphics context to share resources with.</param>
		public virtual GraphicsContext CreateContext(GraphicsContext shareContext = null) {
			return GraphicsPlatform.Factory.CreateGLContext(GraphicsMode, Window, Version, shareContext);
		}

		/// <summary>
		/// Disposes of the graphics component.
		/// </summary>
		~GraphicsComponent() {
			Dispose();
		}

		/// <summary>
		/// Disposes of the graphics component. Beware, this may cause the actual window to be destroyed. Call only when the window is being closed.
		/// </summary>
		public void Dispose() {
			if (IsDisposed)
				return;
			if (Window != null)
				Window.Dispose();
			IsDisposed = true;
			GC.SuppressFinalize(this);
		}
	}
}