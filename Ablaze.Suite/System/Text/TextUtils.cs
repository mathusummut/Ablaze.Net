using System.Text.RegularExpressions;

namespace System.Text {
	/// <summary>
	/// A collection of text processing tools.
	/// </summary>
	public static class TextUtils {
		/// <summary>
		/// Function to detect the encoding for UTF-7, UTF-8/16/32 (bom, no bom, little and big endian),
		/// and local default codepage, and potentially other codepages.
		/// </summary>
		/// <param name="bytes">The byte array to parse text encoding from.</param>
		/// <param name="text">The text output as a string.</param>
		public static Encoding DetectTextEncoding(this byte[] bytes, out string text) {
			if (bytes == null) {
				text = string.Empty;
				return Encoding.Default;
			}

			//////////////// First check the low hanging fruit by checking if a
			//////////////// BOM/signature exists (sourced from http://www.unicode.org/faq/utf_bom.html#bom4)
			if (bytes.Length >= 4 && bytes[0] == 0x00 && bytes[1] == 0x00 && bytes[2] == 0xFE && bytes[3] == 0xFF) {
				text = Encoding.GetEncoding("utf-32BE").GetString(bytes, 4, bytes.Length - 4);
				return Encoding.GetEncoding("utf-32BE");
			}  // UTF-32, big-endian 
			else if (bytes.Length >= 4 && bytes[0] == 0xFF && bytes[1] == 0xFE && bytes[2] == 0x00 && bytes[3] == 0x00) {
				text = Encoding.UTF32.GetString(bytes, 4, bytes.Length - 4);
				return Encoding.UTF32;
			}    // UTF-32, little-endian
			else if (bytes.Length >= 2 && bytes[0] == 0xFE && bytes[1] == 0xFF) {
				text = Encoding.BigEndianUnicode.GetString(bytes, 2, bytes.Length - 2);
				return Encoding.BigEndianUnicode;
			}     // UTF-16, big-endian
			else if (bytes.Length >= 2 && bytes[0] == 0xFF && bytes[1] == 0xFE) {
				text = Encoding.Unicode.GetString(bytes, 2, bytes.Length - 2);
				return Encoding.Unicode;
			}              // UTF-16, little-endian
			else if (bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF) {
				text = Encoding.UTF8.GetString(bytes, 3, bytes.Length - 3);
				return Encoding.UTF8;
			} // UTF-8
			else if (bytes.Length >= 3 && bytes[0] == 0x2b && bytes[1] == 0x2f && bytes[2] == 0x76) {
				text = Encoding.UTF7.GetString(bytes, 3, bytes.Length - 3);
				return Encoding.UTF7;
			} // UTF-7

			// Some text files are encoded in UTF8, but have no BOM/signature. Hence
			// the below manually checks for a UTF8 pattern. This code is based off
			// the top answer at: http://stackoverflow.com/questions/6555015/check-for-invalid-utf8
			// For our purposes, an unnecessarily strict (and terser/slower)
			// implementation is shown at: http://stackoverflow.com/questions/1031645/how-to-detect-utf-8-in-plain-c
			// For the below, false positives should be exceedingly rare (and would
			// be either slightly malformed UTF-8 (which would suit our purposes
			// anyway) or 8-bit extended ASCII/UTF-16/32 at a vanishingly long shot).
			int i = 0;
			bool utf8 = false;
			while (i < bytes.Length - 4) {
				if (bytes[i] <= 0x7F) {
					i += 1;
					continue;
				}     // If all characters are below 0x80, then it is valid UTF8, but UTF8 is not 'required' (and therefore the text is more desirable to be treated as the default codepage of the computer). Hence, there's no "utf8 = true;" code unlike the next three checks.
				if (bytes[i] >= 0xC2 && bytes[i] <= 0xDF && bytes[i + 1] >= 0x80 && bytes[i + 1] < 0xC0) {
					i += 2;
					utf8 = true;
					continue;
				}
				if (bytes[i] >= 0xE0 && bytes[i] <= 0xF0 && bytes[i + 1] >= 0x80 && bytes[i + 1] < 0xC0 && bytes[i + 2] >= 0x80 && bytes[i + 2] < 0xC0) {
					i += 3;
					utf8 = true;
					continue;
				}
				if (bytes[i] >= 0xF0 && bytes[i] <= 0xF4 && bytes[i + 1] >= 0x80 && bytes[i + 1] < 0xC0 && bytes[i + 2] >= 0x80 && bytes[i + 2] < 0xC0 && bytes[i + 3] >= 0x80 && bytes[i + 3] < 0xC0) {
					i += 4;
					utf8 = true;
					continue;
				}
				utf8 = false;
				break;
			}
			if (utf8) {
				text = Encoding.UTF8.GetString(bytes);
				return Encoding.UTF8;
			}


			// The next check is a heuristic attempt to detect UTF-16 without a BOM.
			// We simply look for zeroes in odd or even byte places, and if a certain
			// threshold is reached, the code is 'probably' UF-16.          
			const double threshold = 0.1; // proportion of chars step 2 which must be zeroed to be diagnosed as utf-16. 0.1 = 10%
			int count = 0;
			for (int n = 0; n < bytes.Length; n += 2)
				if (bytes[n] == 0)
					count++;
			if (((double) count) / bytes.Length > threshold) {
				text = Encoding.BigEndianUnicode.GetString(bytes);
				return Encoding.BigEndianUnicode;
			}
			count = 0;
			for (int n = 1; n < bytes.Length; n += 2)
				if (bytes[n] == 0)
					count++;
			if (((double) count) / bytes.Length > threshold) {
				text = Encoding.Unicode.GetString(bytes);
				return Encoding.Unicode;
			} // (little-endian)


			// Finally, a long shot - let's see if we can find "charset=xyz" or
			// "encoding=xyz" to identify the encoding:
			for (int n = 0; n < bytes.Length - 9; n++) {
				if (
					((bytes[n + 0] == 'c' || bytes[n + 0] == 'C') && (bytes[n + 1] == 'h' || bytes[n + 1] == 'H') && (bytes[n + 2] == 'a' || bytes[n + 2] == 'A') && (bytes[n + 3] == 'r' || bytes[n + 3] == 'R') && (bytes[n + 4] == 's' || bytes[n + 4] == 'S') && (bytes[n + 5] == 'e' || bytes[n + 5] == 'E') && (bytes[n + 6] == 't' || bytes[n + 6] == 'T') && (bytes[n + 7] == '=')) ||
					((bytes[n + 0] == 'e' || bytes[n + 0] == 'E') && (bytes[n + 1] == 'n' || bytes[n + 1] == 'N') && (bytes[n + 2] == 'c' || bytes[n + 2] == 'C') && (bytes[n + 3] == 'o' || bytes[n + 3] == 'O') && (bytes[n + 4] == 'd' || bytes[n + 4] == 'D') && (bytes[n + 5] == 'i' || bytes[n + 5] == 'I') && (bytes[n + 6] == 'n' || bytes[n + 6] == 'N') && (bytes[n + 7] == 'g' || bytes[n + 7] == 'G') && (bytes[n + 8] == '='))
					) {
					if (bytes[n + 0] == 'c' || bytes[n + 0] == 'C')
						n += 8;
					else
						n += 9;
					if (bytes[n] == '"' || bytes[n] == '\'')
						n++;
					int oldn = n;
					while (n < bytes.Length && (bytes[n] == '_' || bytes[n] == '-' || (bytes[n] >= '0' && bytes[n] <= '9') || (bytes[n] >= 'a' && bytes[n] <= 'z') || (bytes[n] >= 'A' && bytes[n] <= 'Z'))) {
						n++;
					}
					byte[] nb = new byte[n - oldn];
					Array.Copy(bytes, oldn, nb, 0, n - oldn);
					try {
						string internalEnc = Encoding.ASCII.GetString(nb);
						text = Encoding.GetEncoding(internalEnc).GetString(bytes);
						return Encoding.GetEncoding(internalEnc);
					} catch { break; }    // If C# doesn't recognize the name of the encoding, break.
				}
			}


			// If all else fails, the encoding is probably (though certainly not
			// definitely) the user's local codepage! One might present to the user a
			// list of alternative encodings as shown here: http://stackoverflow.com/questions/8509339/what-is-the-most-common-encoding-of-each-language
			// A full list can be found using Encoding.GetEncodings();
			text = Encoding.Default.GetString(bytes);
			return Encoding.Default;
		}

		/// <summary>
		/// Keeps only the textual content from the specified HTML string.
		/// </summary>
		/// <param name="source">The HTML string to strip down.</param>
		public static string StripHTML(this string source) {
			string result = source.Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty);
			result = Regex.Replace(result, @"( )+", " ");
			// Remove the header (prepare first by clearing attributes)
			result = Regex.Replace(result, @"<( )*head([^>])*>", "<head>", RegexOptions.IgnoreCase);
			result = Regex.Replace(result, @"(<( )*(/)( )*head( )*>)", "</head>", RegexOptions.IgnoreCase);
			result = Regex.Replace(result, "(<head>).*(</head>)", string.Empty, RegexOptions.IgnoreCase);
			// remove all scripts (prepare first by clearing attributes)
			result = Regex.Replace(result, @"<( )*script([^>])*>", "<script>", RegexOptions.IgnoreCase);
			result = Regex.Replace(result, @"(<( )*(/)( )*script( )*>)", "</script>", RegexOptions.IgnoreCase);
			result = Regex.Replace(result, @"(<script>).*(</script>)", string.Empty, RegexOptions.IgnoreCase);
			// remove all styles (prepare first by clearing attributes)
			result = Regex.Replace(result, @"<( )*style([^>])*>", "<style>", RegexOptions.IgnoreCase);
			result = Regex.Replace(result, @"(<( )*(/)( )*style( )*>)", "</style>", RegexOptions.IgnoreCase);
			result = Regex.Replace(result, "(<style>).*(</style>)", string.Empty, RegexOptions.IgnoreCase);
			// insert tabs in spaces of <td> tags
			result = Regex.Replace(result, @"<( )*td([^>])*>", "\t", RegexOptions.IgnoreCase);
			// insert line breaks in places of <BR> and <LI> tags
			result = Regex.Replace(result, @"<( )*br( )*>", "\r", RegexOptions.IgnoreCase);
			result = Regex.Replace(result, @"<( )*li( )*>", "\r", RegexOptions.IgnoreCase);
			// insert line paragraphs (double line breaks) in place if <P>, <DIV> and <TR> tags
			result = Regex.Replace(result, @"<( )*div([^>])*>", "\r\r", RegexOptions.IgnoreCase);
			result = Regex.Replace(result, @"<( )*tr([^>])*>", "\r\r", RegexOptions.IgnoreCase);
			result = Regex.Replace(result, @"<( )*p([^>])*>", "\r\r", RegexOptions.IgnoreCase);
			// Remove remaining tags like <a>, links, images, comments etc - anything that's enclosed inside < >
			result = Regex.Replace(result, @"<[^>]*>", string.Empty, RegexOptions.IgnoreCase);
			// replace special characters:
			result = Regex.Replace(result, @"&bull;", " * ", RegexOptions.IgnoreCase);
			result = Regex.Replace(result, @"&lsaquo;", "<", RegexOptions.IgnoreCase);
			result = Regex.Replace(result, @"&rsaquo;", ">", RegexOptions.IgnoreCase);
			result = Regex.Replace(result, @"&trade;", "(tm)", RegexOptions.IgnoreCase);
			result = Regex.Replace(result, @"&frasl;", "/", RegexOptions.IgnoreCase);
			result = Regex.Replace(result, @"&lt;", "<", RegexOptions.IgnoreCase);
			result = Regex.Replace(result, @"&gt;", ">", RegexOptions.IgnoreCase);
			result = Regex.Replace(result, @"&copy;", "(c)", RegexOptions.IgnoreCase);
			result = Regex.Replace(result, @"&reg;", "(r)", RegexOptions.IgnoreCase);
			// Remove all others.
			result = Regex.Replace(result, @"&(.{2,6});", string.Empty, RegexOptions.IgnoreCase);
			// make line breaking consistent
			result = result.Replace('\n', '\r');
			// Remove extra line breaks and tabs:
			// replace over 2 breaks with 2 and over 4 tabs with 4.
			// Prepare first to remove any whitespaces in between
			// the escaped characters and remove redundant tabs in between line breaks
			result = Regex.Replace(result, "(\r)( )+(\r)", "\r\r", RegexOptions.IgnoreCase);
			result = Regex.Replace(result, "(\t)( )+(\t)", "\t\t", RegexOptions.IgnoreCase);
			result = Regex.Replace(result, "(\t)( )+(\r)", "\t\r", RegexOptions.IgnoreCase);
			result = Regex.Replace(result, "(\r)( )+(\t)", "\r\t", RegexOptions.IgnoreCase);
			// Remove redundant tabs
			result = Regex.Replace(result, "(\r)(\t)+(\r)", "\r\r", RegexOptions.IgnoreCase);
			// Remove multiple tabs following a line break with just one tab
			result = Regex.Replace(result, "(\r)(\t)+", "\r\t", RegexOptions.IgnoreCase);
			// Initial replacement target string for line breaks
			string breaks = "\r\r\r";
			// Initial replacement target string for tabs
			string tabs = "\t\t\t\t\t";
			for (int index = 0; index < result.Length; index++) {
				result = result.Replace(breaks, "\r\r");
				result = result.Replace(tabs, "\t\t\t\t");
				breaks += "\r";
				tabs += "\t";
			}
			return result.Trim();
		}
	}
}