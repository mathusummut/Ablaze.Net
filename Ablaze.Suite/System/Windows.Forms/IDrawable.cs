using System.Drawing;

namespace System.Windows.Forms {
	/// <summary>
	/// Represents a control that can be drawn using different technologies
	/// </summary>
	public interface IDrawable {
		/// <summary>
		/// Gets whether OpenGL rendering is supported
		/// </summary>
		bool SupportsGL {
			get;
		}

		/// <summary>
		/// Draws the control with its children inside onto the specified Graphics canvas at the current location
		/// </summary>
		/// <param name="g">The graphics object to draw on</param>
		void DrawGdi(Graphics g);

		/// <summary>
		/// Draws the control with its children inside onto the specified Graphics canvas at the specified location
		/// </summary>
		/// <param name="g">The graphics object to draw on</param>
		/// <param name="location">The location to draw at</param>
		/// <param name="drawChildren">Whether to draw the child controls</param>
		void DrawGdi(Graphics g, Point location, bool drawChildren = true);

		/// <summary>
		/// Draws the control with its children in the current OpenGL context (assumes the GL matrix is set to orthographic and maps to pixel coordinates).
		/// If not implemented, please throw NotImplementedException.
		/// </summary>
		/// <param name="location">The location to draw at</param>
		/// <param name="drawChildren">Whether to draw the child controls</param>
		void DrawGL(Point location, bool drawChildren);

		/// <summary>
		/// Renders the control and its children onto the specified image
		/// </summary>
		/// <param name="bitmap">The image to draw onto</param>
		void DrawToBitmap(Bitmap bitmap);

		/// <summary>
		/// Renders the control and its children onto the specified image
		/// </summary>
		/// <param name="bitmap">The image to draw onto</param>
		/// <param name="targetBounds">The bounds within which the form is rendered</param>
		void DrawToBitmap(Bitmap bitmap, Rectangle targetBounds);

		/// <summary>
		/// Renders the control onto the specified image
		/// </summary>
		/// <param name="bitmap">The image to draw onto</param>
		/// <param name="targetBounds">The bounds within which the form is rendered</param>
		/// <param name="drawChildren">Whether to draw the child controls</param>
		void DrawToBitmap(Bitmap bitmap, Rectangle targetBounds, bool drawChildren);
	}
}