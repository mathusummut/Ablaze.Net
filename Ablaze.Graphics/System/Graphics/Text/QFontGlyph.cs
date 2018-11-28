using System.Drawing;

namespace System.Graphics.Text {
	/// <summary>
	/// Represents a glyph.
	/// </summary>
	public class QFontGlyph {
		/// <summary>
		/// Which texture page the glyph is on
		/// </summary>
		public int page;

		/// <summary>
		/// The rectangle defining the glyphs position on the page
		/// </summary>
		public Rectangle rect;

		/// <summary>
		/// How far the glyph would need to be vertically offset to be vertically in line with the tallest glyph in the set of all glyphs
		/// </summary>
		public int yOffset;

		/// <summary>
		/// Which character this glyph represents
		/// </summary>
		public char character;

		/// <summary>
		/// Initializes a new glyph representation.
		/// </summary>
		/// <param name="page">Which texture page the glyph is on</param>
		/// <param name="rect">The rectangle defining the glyphs position on the page</param>
		/// <param name="yOffset">How far the glyph would need to be vertically offset to be vertically in line with the tallest glyph in the set of all glyphs</param>
		/// <param name="character">Which character this glyph represents</param>
		public QFontGlyph(int page, Rectangle rect, int yOffset, char character) {
			this.page = page;
			this.rect = rect;
			this.yOffset = yOffset;
			this.character = character;
		}
	}
}