/*                         MIT License
                 Copyright (c) 2017 TGASharpLib

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

BIG THANKS TO ALEXGREENALEX AT https://github.com/ALEXGREENALEX/TGASharpLib
*/

#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Drawing {
	#region Enums
	/// <summary>
	/// <para>The first 128 Color Map Type codes are reserved for use by Truevision,
	/// while the second set of 128 Color Map Type codes(128 to 255) may be used for
	/// developer applications.</para>
	/// True-Color images do not normally make use of the color map field, but some current
	/// applications store palette information or developer-defined information in this field.
	/// It is best to check Field 3, Image Type, to make sure you have a file which can use the
	/// data stored in the Color Map Field.
	/// Otherwise ignore the information. When saving or creating files for True-Color
	/// images do not use this field and set it to Zero to ensure compatibility. Please refer
	/// to the Developer Area specification for methods of storing developer defined information.
	/// </summary>
	public enum TgaColorMapType : byte {
		NoColorMap = 0,
		ColorMap = 1,
		Truevision_2,
		Truevision_3,
		Truevision_4,
		Truevision_5,
		Truevision_6,
		Truevision_7,
		Truevision_8,
		Truevision_9,
		Truevision_10,
		Truevision_11,
		Truevision_12,
		Truevision_13,
		Truevision_14,
		Truevision_15,
		Truevision_16,
		Truevision_17,
		Truevision_18,
		Truevision_19,
		Truevision_20,
		Truevision_21,
		Truevision_22,
		Truevision_23,
		Truevision_24,
		Truevision_25,
		Truevision_26,
		Truevision_27,
		Truevision_28,
		Truevision_29,
		Truevision_30,
		Truevision_31,
		Truevision_32,
		Truevision_33,
		Truevision_34,
		Truevision_35,
		Truevision_36,
		Truevision_37,
		Truevision_38,
		Truevision_39,
		Truevision_40,
		Truevision_41,
		Truevision_42,
		Truevision_43,
		Truevision_44,
		Truevision_45,
		Truevision_46,
		Truevision_47,
		Truevision_48,
		Truevision_49,
		Truevision_50,
		Truevision_51,
		Truevision_52,
		Truevision_53,
		Truevision_54,
		Truevision_55,
		Truevision_56,
		Truevision_57,
		Truevision_58,
		Truevision_59,
		Truevision_60,
		Truevision_61,
		Truevision_62,
		Truevision_63,
		Truevision_64,
		Truevision_65,
		Truevision_66,
		Truevision_67,
		Truevision_68,
		Truevision_69,
		Truevision_70,
		Truevision_71,
		Truevision_72,
		Truevision_73,
		Truevision_74,
		Truevision_75,
		Truevision_76,
		Truevision_77,
		Truevision_78,
		Truevision_79,
		Truevision_80,
		Truevision_81,
		Truevision_82,
		Truevision_83,
		Truevision_84,
		Truevision_85,
		Truevision_86,
		Truevision_87,
		Truevision_88,
		Truevision_89,
		Truevision_90,
		Truevision_91,
		Truevision_92,
		Truevision_93,
		Truevision_94,
		Truevision_95,
		Truevision_96,
		Truevision_97,
		Truevision_98,
		Truevision_99,
		Truevision_100,
		Truevision_101,
		Truevision_102,
		Truevision_103,
		Truevision_104,
		Truevision_105,
		Truevision_106,
		Truevision_107,
		Truevision_108,
		Truevision_109,
		Truevision_110,
		Truevision_111,
		Truevision_112,
		Truevision_113,
		Truevision_114,
		Truevision_115,
		Truevision_116,
		Truevision_117,
		Truevision_118,
		Truevision_119,
		Truevision_120,
		Truevision_121,
		Truevision_122,
		Truevision_123,
		Truevision_124,
		Truevision_125,
		Truevision_126,
		Truevision_127,
		Other_128,
		Other_129,
		Other_130,
		Other_131,
		Other_132,
		Other_133,
		Other_134,
		Other_135,
		Other_136,
		Other_137,
		Other_138,
		Other_139,
		Other_140,
		Other_141,
		Other_142,
		Other_143,
		Other_144,
		Other_145,
		Other_146,
		Other_147,
		Other_148,
		Other_149,
		Other_150,
		Other_151,
		Other_152,
		Other_153,
		Other_154,
		Other_155,
		Other_156,
		Other_157,
		Other_158,
		Other_159,
		Other_160,
		Other_161,
		Other_162,
		Other_163,
		Other_164,
		Other_165,
		Other_166,
		Other_167,
		Other_168,
		Other_169,
		Other_170,
		Other_171,
		Other_172,
		Other_173,
		Other_174,
		Other_175,
		Other_176,
		Other_177,
		Other_178,
		Other_179,
		Other_180,
		Other_181,
		Other_182,
		Other_183,
		Other_184,
		Other_185,
		Other_186,
		Other_187,
		Other_188,
		Other_189,
		Other_190,
		Other_191,
		Other_192,
		Other_193,
		Other_194,
		Other_195,
		Other_196,
		Other_197,
		Other_198,
		Other_199,
		Other_200,
		Other_201,
		Other_202,
		Other_203,
		Other_204,
		Other_205,
		Other_206,
		Other_207,
		Other_208,
		Other_209,
		Other_210,
		Other_211,
		Other_212,
		Other_213,
		Other_214,
		Other_215,
		Other_216,
		Other_217,
		Other_218,
		Other_219,
		Other_220,
		Other_221,
		Other_222,
		Other_223,
		Other_224,
		Other_225,
		Other_226,
		Other_227,
		Other_228,
		Other_229,
		Other_230,
		Other_231,
		Other_232,
		Other_233,
		Other_234,
		Other_235,
		Other_236,
		Other_237,
		Other_238,
		Other_239,
		Other_240,
		Other_241,
		Other_242,
		Other_243,
		Other_244,
		Other_245,
		Other_246,
		Other_247,
		Other_248,
		Other_249,
		Other_250,
		Other_251,
		Other_252,
		Other_253,
		Other_254,
		Other_255
	}

	/// <summary>
	/// Establishes the number of bits per entry. Typically 15, 16, 24 or 32-bit values are used.
	/// <para>When working with VDA or VDA/D cards it is preferred that you select 16 bits(5 bits
	/// per primary with 1 bit to select interrupt control) and set the 16th bit to 0 so that the
	/// interrupt bit is disabled. Even if this field is set to 15 bits(5 bits per primary) you
	/// must still parse the color map data 16 bits at a time and ignore the 16th bit.</para>
	/// <para>When working with a TARGA M8 card you would select 24 bits (8 bits per primary)
	/// since the color map is defined as 256 entries of 24 bit color values.</para>
	/// When working with a TrueVista card(ATVista or NuVista) you would select 24-bit(8 bits per
	/// primary) or 32-bit(8 bits per primary including Alpha channel) depending on your
	/// application’s use of look-up tables. It is suggested that when working with 16-bit and
	/// 32-bit color images, you store them as True-Color images and do not use the color map 
	/// field to store look-up tables. Please refer to the TGA Extensions for fields better suited
	/// to storing look-up table information.
	/// </summary>
	public enum TgaColorMapEntrySize : byte {
		Other = 0,
		X1R5G5B5 = 15,
		A1R5G5B5 = 16,
		R8G8B8 = 24,
		A8R8G8B8 = 32
	}

	/// <summary>
	/// Truevision has currently defined seven image types:
	/// <para>0 - No Image Data Included;</para>
	/// <para>1 - Uncompressed, Color-mapped Image;</para>
	/// <para>2 - Uncompressed, True-color Image;</para>
	/// <para>3 - Uncompressed, Black-and-white Image;</para>
	/// <para>9 - Run-length encoded, Color-mapped Image;</para>
	/// <para>10 - Run-length encoded, True-color Image;</para>
	/// <para>11 - Run-length encoded, Black-and-white Image.</para>
	/// Image Data Type codes 0 to 127 are reserved for use by Truevision for general applications.
	/// Image Data Type codes 128 to 255 may be used for developer applications.
	/// </summary>
	public enum TgaImageType : byte {
		NoImageData = 0,
		Uncompressed_ColorMapped = 1,
		Uncompressed_TrueColor,
		Uncompressed_BlackWhite,
		Truevision_4,
		Truevision_5,
		Truevision_6,
		Truevision_7,
		Truevision_8,
		RLE_ColorMapped = 9,
		RLE_TrueColor,
		RLE_BlackWhite,
		Truevision_12,
		Truevision_13,
		Truevision_14,
		Truevision_15,
		Truevision_16,
		Truevision_17,
		Truevision_18,
		Truevision_19,
		Truevision_20,
		Truevision_21,
		Truevision_22,
		Truevision_23,
		Truevision_24,
		Truevision_25,
		Truevision_26,
		Truevision_27,
		Truevision_28,
		Truevision_29,
		Truevision_30,
		Truevision_31,
		Truevision_32,
		Truevision_33,
		Truevision_34,
		Truevision_35,
		Truevision_36,
		Truevision_37,
		Truevision_38,
		Truevision_39,
		Truevision_40,
		Truevision_41,
		Truevision_42,
		Truevision_43,
		Truevision_44,
		Truevision_45,
		Truevision_46,
		Truevision_47,
		Truevision_48,
		Truevision_49,
		Truevision_50,
		Truevision_51,
		Truevision_52,
		Truevision_53,
		Truevision_54,
		Truevision_55,
		Truevision_56,
		Truevision_57,
		Truevision_58,
		Truevision_59,
		Truevision_60,
		Truevision_61,
		Truevision_62,
		Truevision_63,
		Truevision_64,
		Truevision_65,
		Truevision_66,
		Truevision_67,
		Truevision_68,
		Truevision_69,
		Truevision_70,
		Truevision_71,
		Truevision_72,
		Truevision_73,
		Truevision_74,
		Truevision_75,
		Truevision_76,
		Truevision_77,
		Truevision_78,
		Truevision_79,
		Truevision_80,
		Truevision_81,
		Truevision_82,
		Truevision_83,
		Truevision_84,
		Truevision_85,
		Truevision_86,
		Truevision_87,
		Truevision_88,
		Truevision_89,
		Truevision_90,
		Truevision_91,
		Truevision_92,
		Truevision_93,
		Truevision_94,
		Truevision_95,
		Truevision_96,
		Truevision_97,
		Truevision_98,
		Truevision_99,
		Truevision_100,
		Truevision_101,
		Truevision_102,
		Truevision_103,
		Truevision_104,
		Truevision_105,
		Truevision_106,
		Truevision_107,
		Truevision_108,
		Truevision_109,
		Truevision_110,
		Truevision_111,
		Truevision_112,
		Truevision_113,
		Truevision_114,
		Truevision_115,
		Truevision_116,
		Truevision_117,
		Truevision_118,
		Truevision_119,
		Truevision_120,
		Truevision_121,
		Truevision_122,
		Truevision_123,
		Truevision_124,
		Truevision_125,
		Truevision_126,
		Truevision_127,
		Other_128,
		Other_129,
		Other_130,
		Other_131,
		Other_132,
		Other_133,
		Other_134,
		Other_135,
		Other_136,
		Other_137,
		Other_138,
		Other_139,
		Other_140,
		Other_141,
		Other_142,
		Other_143,
		Other_144,
		Other_145,
		Other_146,
		Other_147,
		Other_148,
		Other_149,
		Other_150,
		Other_151,
		Other_152,
		Other_153,
		Other_154,
		Other_155,
		Other_156,
		Other_157,
		Other_158,
		Other_159,
		Other_160,
		Other_161,
		Other_162,
		Other_163,
		Other_164,
		Other_165,
		Other_166,
		Other_167,
		Other_168,
		Other_169,
		Other_170,
		Other_171,
		Other_172,
		Other_173,
		Other_174,
		Other_175,
		Other_176,
		Other_177,
		Other_178,
		Other_179,
		Other_180,
		Other_181,
		Other_182,
		Other_183,
		Other_184,
		Other_185,
		Other_186,
		Other_187,
		Other_188,
		Other_189,
		Other_190,
		Other_191,
		Other_192,
		Other_193,
		Other_194,
		Other_195,
		Other_196,
		Other_197,
		Other_198,
		Other_199,
		Other_200,
		Other_201,
		Other_202,
		Other_203,
		Other_204,
		Other_205,
		Other_206,
		Other_207,
		Other_208,
		Other_209,
		Other_210,
		Other_211,
		Other_212,
		Other_213,
		Other_214,
		Other_215,
		Other_216,
		Other_217,
		Other_218,
		Other_219,
		Other_220,
		Other_221,
		Other_222,
		Other_223,
		Other_224,
		Other_225,
		Other_226,
		Other_227,
		Other_228,
		Other_229,
		Other_230,
		Other_231,
		Other_232,
		Other_233,
		Other_234,
		Other_235,
		Other_236,
		Other_237,
		Other_238,
		Other_239,
		Other_240,
		Other_241,
		Other_242,
		Other_243,
		Other_244,
		Other_245,
		Other_246,
		Other_247,
		Other_248,
		Other_249,
		Other_250,
		Other_251,
		Other_252,
		Other_253,
		Other_254,
		Other_255
	}

	/// <summary>
	/// Number of bits per pixel. This number includes the Attribute or Alpha channel bits.
	/// Common values are 8, 16, 24 and 32 but other pixel depths could be used.
	/// </summary>
	public enum TgaPixelDepth : byte {
		Other = 0,
		Bpp8 = 8,
		Bpp16 = 16,
		Bpp24 = 24,
		Bpp32 = 32
	}

	/// <summary>
	/// Used to indicate the order in which pixel data is transferred from the file to the screen.
	/// (Bit 4 (bit 0 in enum) is for left-to-right ordering and bit 5 (bit 1 in enum) is for
	/// topto-bottom ordering as shown below.)
	/// </summary>
	public enum TgaImgOrigin : byte {
		BottomLeft = 0,
		BottomRight,
		TopLeft,
		TopRight
	}

	/// <summary>
	/// Contains a value which specifies the type of Alpha channel
	/// data contained in the file. Value Meaning:
	/// <para>0: no Alpha data included (bits 3-0 of field 5.6 should also be set to zero)</para>
	/// <para>1: undefined data in the Alpha field, can be ignored</para>
	/// <para>2: undefined data in the Alpha field, but should be retained</para>
	/// <para>3: useful Alpha channel data is present</para>
	/// <para>4: pre-multiplied Alpha(see description below)</para>
	/// <para>5 -127: RESERVED</para>
	/// <para>128-255: Un-assigned</para>
	/// <para>Pre-multiplied Alpha Example: Suppose the Alpha channel data is being used to specify the
	/// opacity of each pixel(for use when the image is overlayed on another image), where 0 indicates
	/// that the pixel is completely transparent and a value of 1 indicates that the pixel is
	/// completely opaque(assume all component values have been normalized).</para>
	/// <para>A quadruple(a, r, g, b) of( 0.5, 1, 0, 0) would indicate that the pixel is pure red with a
	/// transparency of one-half. For numerous reasons(including image compositing) is is better to
	/// pre-multiply the individual color components with the value in the Alpha channel.</para>
	/// A pre-multiplication of the above would produce a quadruple(0.5, 0.5, 0, 0).
	/// A value of 3 in the Attributes Type Field(field 23) would indicate that the color components
	/// of the pixel have already been scaled by the value in the Alpha channel.
	/// </summary>
	public enum TgaAttrType : byte {
		NoAlpha = 0,
		UndefinedAlphaCanBeIgnored,
		UndefinedAlphaButShouldBeRetained,
		UsefulAlpha,
		PreMultipliedAlpha,
		Reserved_5,
		Reserved_6,
		Reserved_7,
		Reserved_8,
		Reserved_9,
		Reserved_10,
		Reserved_11,
		Reserved_12,
		Reserved_13,
		Reserved_14,
		Reserved_15,
		Reserved_16,
		Reserved_17,
		Reserved_18,
		Reserved_19,
		Reserved_20,
		Reserved_21,
		Reserved_22,
		Reserved_23,
		Reserved_24,
		Reserved_25,
		Reserved_26,
		Reserved_27,
		Reserved_28,
		Reserved_29,
		Reserved_30,
		Reserved_31,
		Reserved_32,
		Reserved_33,
		Reserved_34,
		Reserved_35,
		Reserved_36,
		Reserved_37,
		Reserved_38,
		Reserved_39,
		Reserved_40,
		Reserved_41,
		Reserved_42,
		Reserved_43,
		Reserved_44,
		Reserved_45,
		Reserved_46,
		Reserved_47,
		Reserved_48,
		Reserved_49,
		Reserved_50,
		Reserved_51,
		Reserved_52,
		Reserved_53,
		Reserved_54,
		Reserved_55,
		Reserved_56,
		Reserved_57,
		Reserved_58,
		Reserved_59,
		Reserved_60,
		Reserved_61,
		Reserved_62,
		Reserved_63,
		Reserved_64,
		Reserved_65,
		Reserved_66,
		Reserved_67,
		Reserved_68,
		Reserved_69,
		Reserved_70,
		Reserved_71,
		Reserved_72,
		Reserved_73,
		Reserved_74,
		Reserved_75,
		Reserved_76,
		Reserved_77,
		Reserved_78,
		Reserved_79,
		Reserved_80,
		Reserved_81,
		Reserved_82,
		Reserved_83,
		Reserved_84,
		Reserved_85,
		Reserved_86,
		Reserved_87,
		Reserved_88,
		Reserved_89,
		Reserved_90,
		Reserved_91,
		Reserved_92,
		Reserved_93,
		Reserved_94,
		Reserved_95,
		Reserved_96,
		Reserved_97,
		Reserved_98,
		Reserved_99,
		Reserved_100,
		Reserved_101,
		Reserved_102,
		Reserved_103,
		Reserved_104,
		Reserved_105,
		Reserved_106,
		Reserved_107,
		Reserved_108,
		Reserved_109,
		Reserved_110,
		Reserved_111,
		Reserved_112,
		Reserved_113,
		Reserved_114,
		Reserved_115,
		Reserved_116,
		Reserved_117,
		Reserved_118,
		Reserved_119,
		Reserved_120,
		Reserved_121,
		Reserved_122,
		Reserved_123,
		Reserved_124,
		Reserved_125,
		Reserved_126,
		Reserved_127,
		UnAssigned_128,
		UnAssigned_129,
		UnAssigned_130,
		UnAssigned_131,
		UnAssigned_132,
		UnAssigned_133,
		UnAssigned_134,
		UnAssigned_135,
		UnAssigned_136,
		UnAssigned_137,
		UnAssigned_138,
		UnAssigned_139,
		UnAssigned_140,
		UnAssigned_141,
		UnAssigned_142,
		UnAssigned_143,
		UnAssigned_144,
		UnAssigned_145,
		UnAssigned_146,
		UnAssigned_147,
		UnAssigned_148,
		UnAssigned_149,
		UnAssigned_150,
		UnAssigned_151,
		UnAssigned_152,
		UnAssigned_153,
		UnAssigned_154,
		UnAssigned_155,
		UnAssigned_156,
		UnAssigned_157,
		UnAssigned_158,
		UnAssigned_159,
		UnAssigned_160,
		UnAssigned_161,
		UnAssigned_162,
		UnAssigned_163,
		UnAssigned_164,
		UnAssigned_165,
		UnAssigned_166,
		UnAssigned_167,
		UnAssigned_168,
		UnAssigned_169,
		UnAssigned_170,
		UnAssigned_171,
		UnAssigned_172,
		UnAssigned_173,
		UnAssigned_174,
		UnAssigned_175,
		UnAssigned_176,
		UnAssigned_177,
		UnAssigned_178,
		UnAssigned_179,
		UnAssigned_180,
		UnAssigned_181,
		UnAssigned_182,
		UnAssigned_183,
		UnAssigned_184,
		UnAssigned_185,
		UnAssigned_186,
		UnAssigned_187,
		UnAssigned_188,
		UnAssigned_189,
		UnAssigned_190,
		UnAssigned_191,
		UnAssigned_192,
		UnAssigned_193,
		UnAssigned_194,
		UnAssigned_195,
		UnAssigned_196,
		UnAssigned_197,
		UnAssigned_198,
		UnAssigned_199,
		UnAssigned_200,
		UnAssigned_201,
		UnAssigned_202,
		UnAssigned_203,
		UnAssigned_204,
		UnAssigned_205,
		UnAssigned_206,
		UnAssigned_207,
		UnAssigned_208,
		UnAssigned_209,
		UnAssigned_210,
		UnAssigned_211,
		UnAssigned_212,
		UnAssigned_213,
		UnAssigned_214,
		UnAssigned_215,
		UnAssigned_216,
		UnAssigned_217,
		UnAssigned_218,
		UnAssigned_219,
		UnAssigned_220,
		UnAssigned_221,
		UnAssigned_222,
		UnAssigned_223,
		UnAssigned_224,
		UnAssigned_225,
		UnAssigned_226,
		UnAssigned_227,
		UnAssigned_228,
		UnAssigned_229,
		UnAssigned_230,
		UnAssigned_231,
		UnAssigned_232,
		UnAssigned_233,
		UnAssigned_234,
		UnAssigned_235,
		UnAssigned_236,
		UnAssigned_237,
		UnAssigned_238,
		UnAssigned_239,
		UnAssigned_240,
		UnAssigned_241,
		UnAssigned_242,
		UnAssigned_243,
		UnAssigned_244,
		UnAssigned_245,
		UnAssigned_246,
		UnAssigned_247,
		UnAssigned_248,
		UnAssigned_249,
		UnAssigned_250,
		UnAssigned_251,
		UnAssigned_252,
		UnAssigned_253,
		UnAssigned_254,
		UnAssigned_255
	}
	#endregion

	#region Classes
	public class TgaColorKey : ICloneable {
		byte a = 0;
		byte r = 0;
		byte g = 0;
		byte b = 0;

		public TgaColorKey() {
		}

		/// <summary>
		/// Make <see cref="TgaColorKey"/> from ARGB bytes.
		/// </summary>
		/// <param name="A">Alpha value.</param>
		/// <param name="R">Red value.</param>
		/// <param name="G">Green value.</param>
		/// <param name="B">Blue value.</param>
		public TgaColorKey(byte A, byte R, byte G, byte B) {
			a = A;
			r = R;
			g = G;
			b = B;
		}

		/// <summary>
		/// Make <see cref="TgaColorKey"/> from ARGB bytes.
		/// </summary>
		/// <param name="Bytes">Array of bytes(byte[4]).</param>
		public TgaColorKey(byte[] Bytes) {
			if (Bytes == null)
				throw new ArgumentNullException(nameof(Bytes) + " = null!");
			if (Bytes.Length != Size)
				throw new ArgumentOutOfRangeException(nameof(Bytes.Length) + " must be equal " + Size + "!");

			Color color = Color.FromArgb(BitConverter.ToInt32(Bytes, 0));
			a = color.A;
			r = color.R;
			g = color.G;
			b = color.B;
		}

		/// <summary>
		/// Make <see cref="TgaColorKey"/> from <see cref="int"/>.
		/// </summary>
		/// <param name="ARGB">32bit ARGB integer color value.</param>
		public TgaColorKey(int ARGB) {
			Color ColorARGB = Color.FromArgb(ARGB);
			a = ColorARGB.A;
			r = ColorARGB.R;
			g = ColorARGB.G;
			b = ColorARGB.B;
		}

		/// <summary>
		/// Make <see cref="TgaColorKey"/> from <see cref="Color"/>.
		/// </summary>
		/// <param name="color">GDI+ <see cref="Color"/> value.</param>
		public TgaColorKey(Color color) {
			a = color.A;
			r = color.R;
			g = color.G;
			b = color.B;
		}

		/// <summary>
		/// Gets or sets alpha color value.
		/// </summary>
		public byte A {
			get { return a; }
			set { a = value; }
		}

		/// <summary>
		/// Gets or sets red color value.
		/// </summary>
		public byte R {
			get { return r; }
			set { r = value; }
		}

		/// <summary>
		/// Gets or sets green color value.
		/// </summary>
		public byte G {
			get { return g; }
			set { g = value; }
		}

		/// <summary>
		/// Gets or sets blue color value.
		/// </summary>
		public byte B {
			get { return b; }
			set { b = value; }
		}

		/// <summary>
		/// Gets TGA Field size in bytes.
		/// </summary>
		public const int Size = 4;

		/// <summary>
		/// Make full independed copy of <see cref="TgaColorKey"/>.
		/// </summary>
		/// <returns>Copy of <see cref="TgaColorKey"/></returns>
		public TgaColorKey Clone() {
			return new TgaColorKey(a, r, g, b);
		}

		/// <summary>
		/// Make full independed copy of <see cref="TgaColorKey"/>.
		/// </summary>
		/// <returns>Copy of <see cref="TgaColorKey"/></returns>
		object ICloneable.Clone() {
			return Clone();
		}

		public override bool Equals(object obj) {
			return ((obj is TgaColorKey) ? Equals((TgaColorKey) obj) : false);
		}

		public bool Equals(TgaColorKey item) {
			return (a == item.a && r == item.r && g == item.g && b == item.b);
		}

		public static bool operator ==(TgaColorKey item1, TgaColorKey item2) {
			if (ReferenceEquals(item1, null))
				return ReferenceEquals(item2, null);

			if (ReferenceEquals(item2, null))
				return ReferenceEquals(item1, null);

			return item1.Equals(item2);
		}

		public static bool operator !=(TgaColorKey item1, TgaColorKey item2) {
			return !(item1 == item2);
		}

		public override int GetHashCode() {
			return ToInt().GetHashCode();
		}

		/// <summary>
		/// Gets <see cref="TgaColorKey"/> like string.
		/// </summary>
		/// <returns>String in ARGB format.</returns>
		public override string ToString() {
			return String.Format("{0}={1}, {2}={3}, {4}={5}, {6}={7}",
				nameof(A), a, nameof(R), r, nameof(G), g, nameof(B), b);
		}

		/// <summary>
		/// Convert <see cref="TgaColorKey"/> to byte array.
		/// </summary>
		/// <returns>Byte array with length = 4.</returns>
		public byte[] ToBytes() {
			return BitConverter.GetBytes(ToInt());
		}

		/// <summary>
		/// Gets <see cref="TgaColorKey"/> like GDI+ <see cref="Color"/>.
		/// </summary>
		/// <returns><see cref="Color"/> value of <see cref="TgaColorKey"/>.</returns>
		public Color ToColor() {
			return Color.FromArgb(a, r, g, b);
		}

		/// <summary>
		/// Gets <see cref="TgaColorKey"/> like ARGB <see cref="int"/>.
		/// </summary>
		/// <returns>ARGB <see cref="int"/> value of <see cref="TgaColorKey"/>.</returns>
		public int ToInt() {
			return ToColor().ToArgb();
		}
	}

	/// <summary>
	/// This field (5 bytes) and its sub-fields describe the color map (if any) used for the image.
	/// If the Color Map Type field is set to zero, indicating that no color map exists, then
	/// these 5 bytes should be set to zero. These bytes always must be written to the file.
	/// </summary>
	public class TgaColorMapSpec : ICloneable {
		ushort firstEntryIndex = 0;
		ushort colorMapLength = 0;
		TgaColorMapEntrySize colorMapEntrySize = TgaColorMapEntrySize.Other;

		/// <summary>
		/// Make new <see cref="TgaColorMapSpec"/>.
		/// </summary>
		public TgaColorMapSpec() {
		}

		/// <summary>
		/// Make <see cref="TgaColorMapSpec"/> from bytes.
		/// </summary>
		/// <param name="Bytes">Array of bytes(byte[5]).</param>
		public TgaColorMapSpec(byte[] Bytes) {
			if (Bytes == null)
				throw new ArgumentNullException(nameof(Bytes) + " = null!");
			if (Bytes.Length != Size)
				throw new ArgumentOutOfRangeException(nameof(Bytes.Length) + " must be equal " + Size + "!");

			firstEntryIndex = BitConverter.ToUInt16(Bytes, 0);
			colorMapLength = BitConverter.ToUInt16(Bytes, 2);
			colorMapEntrySize = (TgaColorMapEntrySize) Bytes[4];
		}

		/// <summary>
		/// Field 4.1 (2 bytes):
		/// Index of the first color map entry. Index refers to the starting entry in loading
		/// the color map.
		/// <para>Example: If you would have 1024 entries in the entire color map but you only
		/// need to store 72 of those entries, this field allows you to start in the middle of
		/// the color-map (e.g., position 342).</para>
		/// </summary>
		[CLSCompliant(false)]
		public ushort FirstEntryIndex {
			get { return firstEntryIndex; }
			set { firstEntryIndex = value; }
		}

		/// <summary>
		/// Field 4.2 (2 bytes):
		/// Total number of color map entries included.
		/// </summary>
		[CLSCompliant(false)]
		public ushort ColorMapLength {
			get { return colorMapLength; }
			set { colorMapLength = value; }
		}

		/// <summary>
		/// Field 4.3 (1 byte):
		/// Establishes the number of bits per entry. Typically 15, 16, 24 or 32-bit values are used.
		/// <para>When working with VDA or VDA/D cards it is preferred that you select 16 bits(5 bits
		/// per primary with 1 bit to select interrupt control) and set the 16th bit to 0 so that the
		/// interrupt bit is disabled. Even if this field is set to 15 bits(5 bits per primary) you
		/// must still parse the color map data 16 bits at a time and ignore the 16th bit.</para>
		/// <para>When working with a TARGA M8 card you would select 24 bits (8 bits per primary)
		/// since the color map is defined as 256 entries of 24 bit color values.</para>
		/// When working with a TrueVista card(ATVista or NuVista) you would select 24-bit(8 bits per
		/// primary) or 32-bit(8 bits per primary including Alpha channel) depending on your
		/// application’s use of look-up tables. It is suggested that when working with 16-bit and
		/// 32-bit color images, you store them as True-Color images and do not use the color map 
		/// field to store look-up tables. Please refer to the TGA Extensions for fields better suited
		/// to storing look-up table information.
		/// </summary>
		public TgaColorMapEntrySize ColorMapEntrySize {
			get { return colorMapEntrySize; }
			set { colorMapEntrySize = value; }
		}

		/// <summary>
		/// Gets TGA Field size in bytes.
		/// </summary>
		public const int Size = 5;

		/// <summary>
		/// Make full independed copy of <see cref="TgaColorMapSpec"/>.
		/// </summary>
		/// <returns>Copy of <see cref="TgaColorMapSpec"/></returns>
		public TgaColorMapSpec Clone() {
			return new TgaColorMapSpec(ToBytes());
		}

		/// <summary>
		/// Make full independed copy of <see cref="TgaColorMapSpec"/>.
		/// </summary>
		/// <returns>Copy of <see cref="TgaColorMapSpec"/></returns>
		object ICloneable.Clone() {
			return Clone();
		}

		public override bool Equals(object obj) {
			return ((obj is TgaColorMapSpec) ? Equals((TgaColorMapSpec) obj) : false);
		}

		public bool Equals(TgaColorMapSpec item) {
			return (firstEntryIndex == item.firstEntryIndex &&
				colorMapLength == item.colorMapLength &&
				colorMapEntrySize == item.colorMapEntrySize);
		}

		public static bool operator ==(TgaColorMapSpec item1, TgaColorMapSpec item2) {
			if (ReferenceEquals(item1, null))
				return ReferenceEquals(item2, null);

			if (ReferenceEquals(item2, null))
				return ReferenceEquals(item1, null);

			return item1.Equals(item2);
		}

		public static bool operator !=(TgaColorMapSpec item1, TgaColorMapSpec item2) {
			return !(item1 == item2);
		}

		public override int GetHashCode() {
			unchecked {
				return (firstEntryIndex << 16 | colorMapLength).GetHashCode() ^ colorMapEntrySize.GetHashCode();
			}
		}

		public override string ToString() {
			return String.Format("{0}={1}, {2}={3}, {4}={5}", nameof(FirstEntryIndex), FirstEntryIndex,
				nameof(ColorMapLength), ColorMapLength, nameof(ColorMapEntrySize), ColorMapEntrySize);
		}

		/// <summary>
		/// Convert ColorMapSpec to byte array.
		/// </summary>
		/// <returns>Byte array with length = 5.</returns>
		public byte[] ToBytes() {
			return BitConverterExt.ToBytes(firstEntryIndex, colorMapLength, (byte) colorMapEntrySize);
		}
	}

	public class TgaComment : ICloneable {
		const int StrNLen = 80; //80 ASCII chars + 1 '\0' = 81 per SrtN!
		string origString = String.Empty;
		char blankSpaceChar = TgaString.DefaultBlankSpaceChar;

		public TgaComment() {
		}

		public TgaComment(string Str, char BlankSpaceChar = '\0') {
			if (Str == null)
				throw new ArgumentNullException(nameof(Str) + " = null!");

			origString = Str;
			blankSpaceChar = BlankSpaceChar;
		}

		public TgaComment(byte[] Bytes) {
			if (Bytes == null)
				throw new ArgumentNullException(nameof(Bytes) + " = null!");
			if (Bytes.Length != Size)
				throw new ArgumentOutOfRangeException(nameof(Bytes.Length) + " must be equal " + Size + "!");

			string s = Encoding.ASCII.GetString(Bytes, 0, StrNLen);
			s += Encoding.ASCII.GetString(Bytes, 81, StrNLen);
			s += Encoding.ASCII.GetString(Bytes, 162, StrNLen);
			s += Encoding.ASCII.GetString(Bytes, 243, StrNLen);

			switch (s[s.Length - 1]) {
				case '\0':
				case ' ':
					blankSpaceChar = s[s.Length - 1];
					origString = s.TrimEnd(new char[] { s[s.Length - 1] });
					break;
				default:
					origString = s;
					break;
			}
		}

		/// <summary>
		/// Gets TGA Field size in bytes.
		/// </summary>
		public const int Size = 81 * 4;

		public string OriginalString {
			get { return origString; }
			set { origString = value; }
		}

		public char BlankSpaceChar {
			get { return blankSpaceChar; }
			set { blankSpaceChar = value; }
		}

		/// <summary>
		/// Make full independed copy of <see cref="TgaComment"/>.
		/// </summary>
		/// <returns>Copy of <see cref="TgaComment"/></returns>
		public TgaComment Clone() {
			return new TgaComment(origString, blankSpaceChar);
		}

		/// <summary>
		/// Make full independed copy of <see cref="TgaComment"/>.
		/// </summary>
		/// <returns>Copy of <see cref="TgaComment"/></returns>
		object ICloneable.Clone() {
			return Clone();
		}

		public override bool Equals(object obj) {
			return ((obj is TgaComment) ? Equals((TgaComment) obj) : false);
		}

		public bool Equals(TgaComment item) {
			return (origString == item.origString && blankSpaceChar == item.blankSpaceChar);
		}

		public static bool operator ==(TgaComment item1, TgaComment item2) {
			if (ReferenceEquals(item1, null))
				return ReferenceEquals(item2, null);

			if (ReferenceEquals(item2, null))
				return ReferenceEquals(item1, null);

			return item1.Equals(item2);
		}

		public static bool operator !=(TgaComment item1, TgaComment item2) {
			return !(item1 == item2);
		}

		public override int GetHashCode() {
			return origString.GetHashCode() ^ blankSpaceChar.GetHashCode();
		}

		/// <summary>
		/// Get ASCII-Like string with string-terminators, example: "Line1 \0\0 Line2 \0\0\0".
		/// </summary>
		/// <returns>String with replaced string-terminators to "\0".</returns>
		public override string ToString() {
			return Encoding.ASCII.GetString(ToBytes()).Replace("\0", @"\0");
		}

		/// <summary>
		/// Get ASCII-Like string to first string-terminator, example:
		/// "Some string \0 Some Data \0" - > "Some string".
		/// </summary>
		/// <returns>String to first string-terminator.</returns>
		public string GetString() {
			String Str = Encoding.ASCII.GetString(ToBytes());
			for (int i = 1; i < 4; i++)
				Str = Str.Insert((StrNLen + 1) * i + i - 1, "\n");
			return Str.Replace("\0", String.Empty).TrimEnd(new char[] { '\n' });
		}

		/// <summary>
		/// Convert <see cref="TgaComment"/> to byte array.
		/// </summary>
		/// <returns>Byte array, every byte is ASCII symbol.</returns>
		public byte[] ToBytes() {
			return ToBytes(origString, blankSpaceChar);
		}

		/// <summary>
		/// Convert <see cref="TgaComment"/> to byte array.
		/// </summary>
		/// <param name="Str">Input string.</param>
		/// <param name="BlankSpaceChar">Char for filling blank space in string.</param>
		/// <returns>Byte array, every byte is ASCII symbol.</returns>
		public static byte[] ToBytes(string Str, char BlankSpaceChar = '\0') {
			char[] C = new char[81 * 4];

			for (int i = 0; i < C.Length; i++) {
				if ((i + 82) % 81 == 0)
					C[i] = TgaString.DefaultEndingChar;
				else {
					int Index = i - i / 81;
					C[i] = (Index < Str.Length ? Str[Index] : BlankSpaceChar);
				}
			}
			return Encoding.ASCII.GetBytes(C);
		}
	}

	public class TgaDateTime : ICloneable {
		ushort month = 0;
		ushort day = 0;
		ushort year = 0;
		ushort hour = 0;
		ushort minute = 0;
		ushort second = 0;

		/// <summary>
		/// Make empty <see cref="TgaDateTime"/>.
		/// </summary>
		public TgaDateTime() {
		}

		/// <summary>
		/// Make <see cref="TgaDateTime"/> from <see cref="DateTime"/>.
		/// </summary>
		/// <param name="DateAndTime">Some <see cref="DateTime"/> variable.</param>
		public TgaDateTime(DateTime DateAndTime) {
			month = (ushort) DateAndTime.Month;
			day = (ushort) DateAndTime.Day;
			year = (ushort) DateAndTime.Year;
			hour = (ushort) DateAndTime.Hour;
			minute = (ushort) DateAndTime.Minute;
			second = (ushort) DateAndTime.Second;
		}

		/// <summary>
		/// Make <see cref="TgaDateTime"/> from ushort values.
		/// </summary>
		/// <param name="Month">Month (1 - 12).</param>
		/// <param name="Day">Day (1 - 31).</param>
		/// <param name="Year">Year (4 digit, ie. 1989).</param>
		/// <param name="Hour">Hour (0 - 23).</param>
		/// <param name="Minute">Minute (0 - 59).</param>
		/// <param name="Second">Second (0 - 59).</param>
		[CLSCompliant(false)]
		public TgaDateTime(ushort Month, ushort Day, ushort Year, ushort Hour, ushort Minute, ushort Second) {
			month = Month;
			day = Day;
			year = Year;
			hour = Hour;
			minute = Minute;
			second = Second;
		}

		/// <summary>
		/// Make <see cref="TgaDateTime"/> from bytes.
		/// </summary>
		/// <param name="Bytes">Array of bytes(byte[12]).</param>
		public TgaDateTime(byte[] Bytes) {
			if (Bytes == null)
				throw new ArgumentNullException(nameof(Bytes) + " = null!");
			else if (Bytes.Length != Size)
				throw new ArgumentOutOfRangeException(nameof(Bytes) + " must be equal " + Size + "!");

			month = BitConverter.ToUInt16(Bytes, 0);
			day = BitConverter.ToUInt16(Bytes, 2);
			year = BitConverter.ToUInt16(Bytes, 4);
			hour = BitConverter.ToUInt16(Bytes, 6);
			minute = BitConverter.ToUInt16(Bytes, 8);
			second = BitConverter.ToUInt16(Bytes, 10);
		}

		/// <summary>
		/// Gets or Sets month (1 - 12).
		/// </summary>
		[CLSCompliant(false)]
		public ushort Month {
			get { return month; }
			set { month = value; }
		}

		/// <summary>
		/// Gets or Sets day (1 - 31).
		/// </summary>
		[CLSCompliant(false)]
		public ushort Day {
			get { return day; }
			set { day = value; }
		}

		/// <summary>
		/// Gets or Sets year (4 digit, ie. 1989).
		/// </summary>
		[CLSCompliant(false)]
		public ushort Year {
			get { return year; }
			set { year = value; }
		}

		/// <summary>
		/// Gets or Sets hour (0 - 23).
		/// </summary>
		[CLSCompliant(false)]
		public ushort Hour {
			get { return hour; }
			set { hour = value; }
		}

		/// <summary>
		/// Gets or Sets minute (0 - 59).
		/// </summary>
		[CLSCompliant(false)]
		public ushort Minute {
			get { return minute; }
			set { minute = value; }
		}

		/// <summary>
		/// Gets or Sets second (0 - 59).
		/// </summary>
		[CLSCompliant(false)]
		public ushort Second {
			get { return second; }
			set { second = value; }
		}

		/// <summary>
		/// Gets TGA Field size in bytes.
		/// </summary>
		public const int Size = 12;

		/// <summary>
		/// Make full independed copy of <see cref="TgaDateTime"/>.
		/// </summary>
		/// <returns>Copy of <see cref="TgaDateTime"/></returns>
		public TgaDateTime Clone() {
			return new TgaDateTime(month, day, year, hour, minute, second);
		}

		/// <summary>
		/// Make full independed copy of <see cref="TgaDateTime"/>.
		/// </summary>
		/// <returns>Copy of <see cref="TgaDateTime"/></returns>
		object ICloneable.Clone() {
			return Clone();
		}

		public override bool Equals(object obj) {
			return ((obj is TgaDateTime) ? Equals((TgaDateTime) obj) : false);
		}

		public bool Equals(TgaDateTime item) {
			return (
				month == item.month &&
				day == item.day &&
				year == item.year &&
				hour == item.hour &&
				minute == item.minute &&
				second == item.second);
		}

		public static bool operator ==(TgaDateTime item1, TgaDateTime item2) {
			if (ReferenceEquals(item1, null))
				return ReferenceEquals(item2, null);

			if (ReferenceEquals(item2, null))
				return ReferenceEquals(item1, null);

			return item1.Equals(item2);
		}

		public static bool operator !=(TgaDateTime item1, TgaDateTime item2) {
			return !(item1 == item2);
		}

		public override int GetHashCode() {
			unchecked {
				int hash = 17;
				hash = hash * 23 + (month << 16 | hour).GetHashCode();
				hash = hash * 23 + (day << 16 | minute).GetHashCode();
				hash = hash * 23 + (year << 16 | second).GetHashCode();
				return hash;
			}
		}

		/// <summary>
		/// Gets <see cref="TgaDateTime"/> like string.
		/// </summary>
		/// <returns>String in "1990.01.23 1:02:03" format.</returns>
		public override string ToString() {
			return String.Format("{0:D4}.{1:D2}.{2:D2} {3}:{4:D2}:{5:D2}", year, month, day, hour, minute, second);
		}

		/// <summary>
		/// Convert <see cref="TgaDateTime"/> to byte array.
		/// </summary>
		/// <returns>Byte array with length = 12.</returns>
		public byte[] ToBytes() {
			return BitConverterExt.ToBytes(month, day, year, hour, minute, second);
		}

		/// <summary>
		/// Gets <see cref="TgaDateTime"/> like <see cref="DateTime"/>.
		/// </summary>
		/// <returns><see cref="DateTime"/> value of <see cref="TgaDateTime"/>.</returns>
		public DateTime ToDateTime() {
			return new DateTime(year, month, day, hour, minute, second);
		}
	}

	public class TgaDevEntry : ICloneable {
		// Directory
		ushort fieldTag = 0;
		uint fieldFileOffset = 0;
		// Field
		byte[] data = null;

		/// <summary>
		/// Make empty <see cref="TgaDevEntry"/>.
		/// </summary>
		public TgaDevEntry() {
		}

		/// <summary>
		/// Make <see cref="TgaDevEntry"/> from other <see cref="TgaDevEntry"/>.
		/// </summary>
		/// <param name="Entry">Some <see cref="TgaDevEntry"/> variable.</param>
		public TgaDevEntry(TgaDevEntry Entry) {
			if (Entry == null)
				throw new ArgumentNullException();

			fieldTag = Entry.fieldTag;
			fieldFileOffset = Entry.fieldFileOffset;
			data = BitConverterExt.ToBytes(Entry.data);
		}

		/// <summary>
		/// Make <see cref="TgaDevEntry"/> from <see cref="Tag"/>, <see cref="Offset"/> and <see cref="FieldSize"/>.
		/// </summary>
		/// <param name="Tag">TAG ID (0 - 65535). See <see cref="Tag"/>.</param>
		/// <param name="Offset">TAG file offset in bytes. See <see cref="Offset"/>.</param>
		/// <param name="Data">This is DevEntry Field Data. See <see cref="Data"/>.</param>
		[CLSCompliant(false)]
		public TgaDevEntry(ushort Tag, uint Offset, byte[] Data = null) {
			fieldTag = Tag;
			fieldFileOffset = Offset;
			data = Data;
		}

		/// <summary>
		/// Make <see cref="TgaDevEntry"/> from bytes.
		/// </summary>
		/// <param name="Bytes">Array of bytes(byte[6] or bigger, if <see cref="Data"/> exist).</param>
		public TgaDevEntry(byte[] Bytes) {
			if (Bytes == null)
				throw new ArgumentNullException(nameof(Bytes) + " = null!");
			else if (Bytes.Length < 6)
				throw new ArgumentOutOfRangeException(nameof(Bytes) + " must be >= 6!");

			fieldTag = BitConverter.ToUInt16(Bytes, 0);
			fieldFileOffset = BitConverter.ToUInt32(Bytes, 2);

			if (Bytes.Length > 6)
				data = BitConverterExt.GetElements(Bytes, 6, Bytes.Length - 6);
		}

		/// <summary>
		/// Each TAG is a value in the range of 0 to 65535. Values from 0 - 32767 are available for developer use,
		/// while values from 32768 - 65535 are reserved for Truevision.
		/// </summary>
		[CLSCompliant(false)]
		public ushort Tag {
			get { return fieldTag; }
			set { fieldTag = value; }
		}

		/// <summary>
		/// This OFFSET is a number of bytes from the beginning of the file to the start of the field
		/// referenced by the tag.
		/// </summary>
		[CLSCompliant(false)]
		public uint Offset {
			get { return fieldFileOffset; }
			set { fieldFileOffset = value; }
		}

		/// <summary>
		/// Field DATA.
		/// Although the size and format of the actual Developer Area fields are totally up to the developer,
		/// please define your formats to address future considerations you might have concerning your fields.
		/// This means that if you anticipate changing a field, build flexibility into the format to make these
		/// changes easy on other developers.Major changes to an existing TAG’s definition should never happen.
		/// </summary>
		public byte[] Data {
			get { return data; }
			set { data = value; }
		}

		/// <summary>
		/// The FIELD SIZE is a number of bytes in the field. Same like: Data.Length,
		/// if <see cref="Data"/> is null, return -1.
		/// </summary>
		public int FieldSize {
			get {
				if (Data == null)
					return -1;

				return Data.Length;
			}
		}

		/// <summary>
		/// Gets TGA <see cref="TgaDevEntry"/> size in bytes (Always constant and equal 10!).
		/// It is not <see cref="FieldSize"/>! It is just size of entry sizeof(ushort + uint + uint).
		/// </summary>
		public const int Size = 10;

		/// <summary>
		/// Make full independed copy of <see cref="TgaDevEntry"/>.
		/// </summary>
		/// <returns>Copy of <see cref="TgaDevEntry"/></returns>
		public TgaDevEntry Clone() {
			return new TgaDevEntry(this);
		}

		/// <summary>
		/// Make full independed copy of <see cref="TgaDevEntry"/>.
		/// </summary>
		/// <returns>Copy of <see cref="TgaDevEntry"/></returns>
		object ICloneable.Clone() {
			return Clone();
		}

		public override bool Equals(object obj) {
			return ((obj is TgaDevEntry) ? Equals((TgaDevEntry) obj) : false);
		}

		public bool Equals(TgaDevEntry item) {
			return (fieldTag == item.fieldTag &&
				fieldFileOffset == item.fieldFileOffset &&
				BitConverterExt.IsArraysEqual(data, item.data));
		}

		public static bool operator ==(TgaDevEntry item1, TgaDevEntry item2) {
			if (ReferenceEquals(item1, null))
				return ReferenceEquals(item2, null);

			if (ReferenceEquals(item2, null))
				return ReferenceEquals(item1, null);

			return item1.Equals(item2);
		}

		public static bool operator !=(TgaDevEntry item1, TgaDevEntry item2) {
			return !(item1 == item2);
		}

		public override int GetHashCode() {
			unchecked {
				int hash = 17;
				hash = hash * 23 + fieldTag.GetHashCode();
				hash = hash * 23 + fieldFileOffset.GetHashCode();

				if (data != null)
					for (int i = 0; i < data.Length; i++)
						hash = hash * 23 + data[i].GetHashCode();

				return hash;
			}
		}

		/// <summary>
		/// Gets <see cref="TgaDevEntry"/> like string.
		/// </summary>
		/// <returns>String in "Tag={0}, Offset={1}, FieldSize={2}" format.</returns>
		public override string ToString() {
			return String.Format("{0}={1}, {2}={3}, {4}={5}", nameof(Tag), fieldTag,
				nameof(Offset), fieldFileOffset, nameof(FieldSize), FieldSize);
		}

		/// <summary>
		/// Convert <see cref="TgaDevEntry"/> to byte array. (Not include <see cref="Data"/>!).
		/// </summary>
		/// <returns>Byte array with length = 10.</returns>
		public byte[] ToBytes() {
			return BitConverterExt.ToBytes(fieldTag, fieldFileOffset, (data == null ? 0 : data.Length));
		}
	} //Not full ToBytes()

	public class TgaFraction : ICloneable {
		ushort numerator = 0;
		ushort denominator = 0;

		/// <summary>
		/// Make <see cref="TgaFraction"/> from <see cref="Numerator"/> and <see cref="Denominator"/>.
		/// </summary>
		/// <param name="Numerator">Numerator value.</param>
		/// <param name="Denominator">Denominator value.</param>
		[CLSCompliant(false)]
		public TgaFraction(ushort Numerator = 0, ushort Denominator = 0) {
			numerator = Numerator;
			denominator = Denominator;
		}

		/// <summary>
		/// Make <see cref="TgaFraction"/> from bytes.
		/// </summary>
		/// <param name="Bytes">Array of bytes(byte[4]).</param>
		public TgaFraction(byte[] Bytes) {
			if (Bytes == null)
				throw new ArgumentNullException(nameof(Bytes) + " = null!");
			if (Bytes.Length != Size)
				throw new ArgumentOutOfRangeException(nameof(Bytes.Length) + " must be equal " + Size + "!");

			numerator = BitConverter.ToUInt16(Bytes, 0);
			denominator = BitConverter.ToUInt16(Bytes, 2);
		}

		/// <summary>
		/// Gets or sets numerator value.
		/// </summary>
		[CLSCompliant(false)]
		public ushort Numerator {
			get { return numerator; }
			set { numerator = value; }
		}

		/// <summary>
		/// Gets or sets denominator value.
		/// </summary>
		[CLSCompliant(false)]
		public ushort Denominator {
			get { return denominator; }
			set { denominator = value; }
		}

		/// <summary>
		/// Get aspect ratio = <see cref="Numerator"/> / <see cref="Denominator"/>.
		/// </summary>
		public float AspectRatio {
			get {
				if (numerator == denominator)
					return 1f;

				return numerator / (float) denominator;
			}
		}

		/// <summary>
		/// Gets Empty <see cref="TgaFraction"/>, all values are 0.
		/// </summary>
		public static readonly TgaFraction Empty = new TgaFraction();

		/// <summary>
		/// Gets One <see cref="TgaFraction"/>, all values are 1 (ones, 1 / 1 = 1).
		/// </summary>
		public static readonly TgaFraction One = new TgaFraction(1, 1);

		/// <summary>
		/// Gets TGA Field size in bytes.
		/// </summary>
		public const int Size = 4;

		/// <summary>
		/// Make full independed copy of <see cref="TgaFraction"/>.
		/// </summary>
		/// <returns>Copy of <see cref="TgaFraction"/></returns>
		public TgaFraction Clone() {
			return new TgaFraction(numerator, denominator);
		}

		/// <summary>
		/// Make full independed copy of <see cref="TgaFraction"/>.
		/// </summary>
		/// <returns>Copy of <see cref="TgaFraction"/></returns>
		object ICloneable.Clone() {
			return Clone();
		}

		public override bool Equals(object obj) {
			return ((obj is TgaFraction) ? Equals((TgaFraction) obj) : false);
		}

		public bool Equals(TgaFraction item) {
			return (numerator == item.numerator && denominator == item.denominator);
		}

		public static bool operator ==(TgaFraction item1, TgaFraction item2) {
			if (ReferenceEquals(item1, null))
				return ReferenceEquals(item2, null);

			if (ReferenceEquals(item2, null))
				return ReferenceEquals(item1, null);

			return item1.Equals(item2);
		}

		public static bool operator !=(TgaFraction item1, TgaFraction item2) {
			return !(item1 == item2);
		}

		public override int GetHashCode() {
			return (numerator << 16 | denominator).GetHashCode();
		}

		/// <summary>
		/// Gets <see cref="TgaFraction"/> like string.
		/// </summary>
		/// <returns>String in "Numerator=1, Denominator=2" format.</returns>
		public override string ToString() {
			return String.Format("{0}={1}, {2}={3}", nameof(Numerator), numerator,
				nameof(Denominator), denominator);
		}

		/// <summary>
		/// Convert <see cref="TgaFraction"/> to byte array.
		/// </summary>
		/// <returns>Byte array with length = 4.</returns>
		public byte[] ToBytes() {
			return BitConverterExt.ToBytes(numerator, denominator);
		}
	}

	/// <summary>
	/// Contains image origin bits and alpha channel bits(or number of overlay bits)
	/// </summary>
	public class TgaImageDescriptor : ICloneable {
		TgaImgOrigin imageOrigin = 0; //bits 5-4
		byte alphaChannelBits = 0; //bits 3-0

		/// <summary>
		/// Make empty <see cref="TgaImageDescriptor"/>.
		/// </summary>
		public TgaImageDescriptor() {
		}

		/// <summary>
		/// Make <see cref="TgaImageDescriptor"/> from bytes.
		/// </summary>
		/// <param name="b">ImageDescriptor byte with reserved 7-6 bits, bits 5-4 used for
		/// <see cref="ImageOrigin"/>, 3-0 used as alpha channel bits or number of overlay bits.</param>
		public TgaImageDescriptor(byte b) {
			imageOrigin = (TgaImgOrigin) ((b & 0x30) >> 4);
			alphaChannelBits = (byte) (b & 0x0F);
		}

		/// <summary>
		/// Gets or Sets Image Origin bits (select from enum only, don'n use 5-4 bits!).
		/// </summary>
		public TgaImgOrigin ImageOrigin {
			get { return imageOrigin; }
			set { imageOrigin = value; }
		}

		/// <summary>
		/// Gets or Sets alpha channel bits or number of overlay bits.
		/// </summary>
		public byte AlphaChannelBits {
			get { return alphaChannelBits; }
			set { alphaChannelBits = value; }
		}

		/// <summary>
		/// Gets TGA Field size in bytes.
		/// </summary>
		public const int Size = 1;

		/// <summary>
		/// Make full copy of <see cref="TgaImageDescriptor"/>.
		/// </summary>
		/// <returns>Full independent copy of <see cref="TgaImageDescriptor"/>.</returns>
		public TgaImageDescriptor Clone() {
			return new TgaImageDescriptor(ToByte());
		}

		/// <summary>
		/// Make full copy of <see cref="TgaImageDescriptor"/>.
		/// </summary>
		/// <returns>Full independent copy of <see cref="TgaImageDescriptor"/>.</returns>
		object ICloneable.Clone() {
			return Clone();
		}

		public override bool Equals(object obj) {
			return ((obj is TgaImageDescriptor) ? Equals((TgaImageDescriptor) obj) : false);
		}

		public bool Equals(TgaImageDescriptor item) {
			return (imageOrigin == item.imageOrigin && alphaChannelBits == item.alphaChannelBits);
		}

		public static bool operator ==(TgaImageDescriptor item1, TgaImageDescriptor item2) {
			if (ReferenceEquals(item1, null))
				return ReferenceEquals(item2, null);

			if (ReferenceEquals(item2, null))
				return ReferenceEquals(item1, null);

			return item1.Equals(item2);
		}

		public static bool operator !=(TgaImageDescriptor item1, TgaImageDescriptor item2) {
			return !(item1 == item2);
		}

		public override int GetHashCode() {
			unchecked {
				return ((int) ImageOrigin << 4 | alphaChannelBits).GetHashCode();
			}
		}

		public override string ToString() {
			return String.Format("{0}={1}, {2}={3}, ImageDescriptor_AsByte={4}", nameof(ImageOrigin),
				imageOrigin, nameof(AlphaChannelBits), alphaChannelBits, ToByte());
		}

		/// <summary>
		/// Gets ImageDescriptor byte.
		/// </summary>
		/// <returns>ImageDescriptor byte with reserved 7-6 bits, bits 5-4 used for imageOrigin,
		/// 3-0 used as alpha channel bits or number of overlay bits.</returns>
		public byte ToByte() {
			return (byte) (((int) imageOrigin << 4) | alphaChannelBits);
		}
	}

	/// <summary>
	/// Image Specification - Field 5 (10 bytes):
	/// <para>This field and its sub-fields describe the image screen location, size and pixel depth.
	/// These information is always written to the file.</para>
	/// </summary>
	public class TgaImageSpec : ICloneable {
		ushort x_Origin = 0;
		ushort y_Origin = 0;
		ushort imageWidth = 0;
		ushort imageHeight = 0;
		TgaPixelDepth pixelDepth = TgaPixelDepth.Other;
		TgaImageDescriptor imageDescriptor = new TgaImageDescriptor();

		public TgaImageSpec() {
		}

		/// <summary>
		/// Make ImageSpec from values.
		/// </summary>
		/// <param name="X_Origin">These specify the absolute horizontal coordinate for the lower
		/// left corner of the image as it is positioned on a display device having an origin at
		/// the lower left of the screen(e.g., the TARGA series).</param>
		/// <param name="Y_Origin">These specify the absolute vertical coordinate for the lower
		/// left corner of the image as it is positioned on a display device having an origin at
		/// the lower left of the screen(e.g., the TARGA series).</param>
		/// <param name="ImageWidth">This field specifies the width of the image in pixels.</param>
		/// <param name="ImageHeight">This field specifies the height of the image in pixels.</param>
		/// <param name="PixelDepth">This field indicates the number of bits per pixel. This number
		/// includes the Attribute or Alpha channel bits. Common values are 8, 16, 24 and 32 but
		/// other pixel depths could be used.</param>
		/// <param name="ImageDescriptor">Contains image origin bits and alpha channel bits
		/// (or number of overlay bits).</param>
		[CLSCompliant(false)]
		public TgaImageSpec(ushort X_Origin, ushort Y_Origin, ushort ImageWidth, ushort ImageHeight,
			TgaPixelDepth PixelDepth, TgaImageDescriptor ImageDescriptor) {
			x_Origin = X_Origin;
			y_Origin = Y_Origin;
			imageWidth = ImageWidth;
			imageHeight = ImageHeight;
			pixelDepth = PixelDepth;
			imageDescriptor = ImageDescriptor;
		}

		/// <summary>
		/// Make ImageSpec from bytes.
		/// </summary>
		/// <param name="Bytes">Array of bytes(byte[10]).</param>
		public TgaImageSpec(byte[] Bytes) {
			if (Bytes == null)
				throw new ArgumentNullException(nameof(Bytes) + " = null!");
			if (Bytes.Length != Size)
				throw new ArgumentOutOfRangeException(nameof(Bytes.Length) + " must be equal " + Size + "!");

			x_Origin = BitConverter.ToUInt16(Bytes, 0);
			y_Origin = BitConverter.ToUInt16(Bytes, 2);
			imageWidth = BitConverter.ToUInt16(Bytes, 4);
			imageHeight = BitConverter.ToUInt16(Bytes, 6);
			pixelDepth = (TgaPixelDepth) Bytes[8];
			imageDescriptor = new TgaImageDescriptor(Bytes[9]);
		}

		/// <summary>
		/// These specify the absolute horizontal coordinate for the lower left corner of the image
		/// as it is positioned on a display device having an origin at the lower left of the
		/// screen(e.g., the TARGA series).
		/// </summary>
		[CLSCompliant(false)]
		public ushort X_Origin {
			get { return x_Origin; }
			set { x_Origin = value; }
		}

		/// <summary>
		/// These specify the absolute vertical coordinate for the lower left corner of the image
		/// as it is positioned on a display device having an origin at the lower left of the
		/// screen(e.g., the TARGA series).
		/// </summary>
		[CLSCompliant(false)]
		public ushort Y_Origin {
			get { return y_Origin; }
			set { y_Origin = value; }
		}

		/// <summary>
		/// This field specifies the width of the image in pixels.
		/// </summary>
		[CLSCompliant(false)]
		public ushort ImageWidth {
			get { return imageWidth; }
			set { imageWidth = value; }
		}

		/// <summary>
		/// This field specifies the height of the image in pixels.
		/// </summary>
		[CLSCompliant(false)]
		public ushort ImageHeight {
			get { return imageHeight; }
			set { imageHeight = value; }
		}

		/// <summary>
		/// This field indicates the number of bits per pixel. This number includes the Attribute or
		/// Alpha channel bits. Common values are 8, 16, 24 and 32 but other pixel depths could be used.
		/// </summary>
		public TgaPixelDepth PixelDepth {
			get { return pixelDepth; }
			set { pixelDepth = value; }
		}

		/// <summary>
		/// Contains image origin bits and alpha channel bits(or number of overlay bits).
		/// </summary>
		public TgaImageDescriptor ImageDescriptor {
			get { return imageDescriptor; }
			set { imageDescriptor = value; }
		}

		/// <summary>
		/// Gets TGA Field size in bytes.
		/// </summary>
		public const int Size = 10;

		/// <summary>
		/// Make full copy of <see cref="TgaImageDescriptor"/>.
		/// </summary>
		/// <returns></returns>
		public TgaImageSpec Clone() {
			return new TgaImageSpec(ToBytes());
		}

		/// <summary>
		/// Make full copy of <see cref="TgaImageDescriptor"/>.
		/// </summary>
		/// <returns></returns>
		object ICloneable.Clone() {
			return Clone();
		}

		public override bool Equals(object obj) {
			return ((obj is TgaImageSpec) ? Equals((TgaImageSpec) obj) : false);
		}

		public bool Equals(TgaImageSpec item) {
			return (
				x_Origin == item.x_Origin &&
				y_Origin == item.y_Origin &&
				imageWidth == item.imageWidth &&
				imageHeight == item.imageHeight &&
				pixelDepth == item.pixelDepth &&
				imageDescriptor == item.imageDescriptor);
		}

		public static bool operator ==(TgaImageSpec item1, TgaImageSpec item2) {
			if (ReferenceEquals(item1, null))
				return ReferenceEquals(item2, null);

			if (ReferenceEquals(item2, null))
				return ReferenceEquals(item1, null);

			return item1.Equals(item2);
		}

		public static bool operator !=(TgaImageSpec item1, TgaImageSpec item2) {
			return !(item1 == item2);
		}

		public override int GetHashCode() {
			unchecked {
				int hash = 17;
				hash = hash * 23 + x_Origin.GetHashCode();
				hash = hash * 23 + y_Origin.GetHashCode();
				hash = hash * 23 + imageWidth.GetHashCode();
				hash = hash * 23 + imageHeight.GetHashCode();
				hash = hash * 23 + pixelDepth.GetHashCode();

				if (imageDescriptor != null)
					hash = hash * 23 + imageDescriptor.GetHashCode();

				return hash;
			}
		}

		public override string ToString() {
			return String.Format("{0}={1}, {2}={3}, {4}={5}, {6}={7}, {8}={9}, {10}={11}",
				nameof(X_Origin), x_Origin,
				nameof(Y_Origin), y_Origin,
				nameof(ImageWidth), imageWidth,
				nameof(ImageHeight), imageHeight,
				nameof(PixelDepth), pixelDepth,
				nameof(ImageDescriptor), imageDescriptor);
		}

		/// <summary>
		/// Convert <see cref="TgaImageSpec"/> to byte array.
		/// </summary>
		/// <returns>Byte array with length = 10.</returns>
		public byte[] ToBytes() {
			return BitConverterExt.ToBytes(x_Origin, y_Origin, imageWidth, imageHeight,
				(byte) pixelDepth, (imageDescriptor == null ? byte.MinValue : imageDescriptor.ToByte()));
		}
	}

	/// <summary>
	/// Postage Stamp Image (MaxSize 64x64, uncompressed, PixelDepth like in full image).
	/// </summary>
	public class TgaPostageStampImage : ICloneable {
		byte width = 0;
		byte height = 0;
		byte[] data = null;

		public TgaPostageStampImage() {
		}

		/// <summary>
		/// Make <see cref="TgaPostageStampImage"/> from bytes array.
		/// </summary>
		/// <param name="Bytes">Bytes array, first 2 bytes are <see cref="Width"/> and <see cref="Height"/>,
		/// next bytes - image data.</param>
		public TgaPostageStampImage(byte[] Bytes) {
			if (Bytes == null)
				throw new ArgumentNullException(nameof(Bytes) + " = null!");
			if (Bytes.Length < 2)
				throw new ArgumentOutOfRangeException(nameof(Bytes.Length) + " must be >= " + 2 + "!");

			width = Bytes[0];
			height = Bytes[1];

			if (Bytes.Length > 2)
				data = BitConverterExt.GetElements(Bytes, 2, Bytes.Length - 2);
		}

		/// <summary>
		/// Make <see cref="TgaPostageStampImage"/> from bytes and size.
		/// </summary>
		/// <param name="Width">Image Width.</param>
		/// <param name="Height">Image Height.</param>
		/// <param name="Bytes">Postage Stamp Image Data.</param>
		public TgaPostageStampImage(byte Width, byte Height, byte[] Bytes) {
			if (Bytes == null)
				throw new ArgumentNullException(nameof(Bytes) + " = null!");

			width = Width;
			height = Height;
			data = Bytes;
		}

		/// <summary>
		/// Postage Stamp Image Data
		/// </summary>
		public byte[] Data {
			get { return data; }
			set { data = value; }
		}

		/// <summary>
		/// Postage Stamp Image Width (maximum = 64).
		/// </summary>
		public byte Width {
			get { return width; }
			set { width = value; }
		}

		/// <summary>
		/// Postage Stamp Image Height (maximum = 64).
		/// </summary>
		public byte Height {
			get { return height; }
			set { height = value; }
		}

		/// <summary>
		/// Make full copy of <see cref="TgaPostageStampImage"/>.
		/// </summary>
		/// <returns>Full independent copy of <see cref="TgaPostageStampImage"/>.</returns>
		public TgaPostageStampImage Clone() {
			return new TgaPostageStampImage(width, height, BitConverterExt.ToBytes(data));
		}

		/// <summary>
		/// Make full copy of <see cref="TgaPostageStampImage"/>.
		/// </summary>
		/// <returns>Full independent copy of <see cref="TgaPostageStampImage"/>.</returns>
		object ICloneable.Clone() {
			return Clone();
		}

		public override bool Equals(object obj) {
			return ((obj is TgaPostageStampImage) ? Equals((TgaPostageStampImage) obj) : false);
		}

		public bool Equals(TgaPostageStampImage item) {
			return width == item.width && height == item.height && BitConverterExt.IsArraysEqual(data, item.data);
		}

		public static bool operator ==(TgaPostageStampImage item1, TgaPostageStampImage item2) {
			if (ReferenceEquals(item1, null))
				return ReferenceEquals(item2, null);

			if (ReferenceEquals(item2, null))
				return ReferenceEquals(item1, null);

			return item1.Equals(item2);
		}

		public static bool operator !=(TgaPostageStampImage item1, TgaPostageStampImage item2) {
			return !(item1 == item2);
		}

		public override int GetHashCode() {
			unchecked {
				int hash = 27;
				hash = (13 * hash) + width.GetHashCode();
				hash = (13 * hash) + height.GetHashCode();
				if (data != null)
					for (int i = 0; i < data.Length; i++)
						hash = (13 * hash) + data[i].GetHashCode();
				return hash;
			}
		}

		public override string ToString() {
			return String.Format("{0}={1}, {2}={3}, DataLength={4}",
				nameof(Width), width, nameof(Height), height, (data == null ? -1 : data.Length));
		}

		/// <summary>
		/// Convert <see cref="TgaPostageStampImage"/> to byte array.
		/// </summary>
		/// <returns>Byte array.</returns>
		public byte[] ToBytes() {
			return BitConverterExt.ToBytes(width, height, data);
		}
	}

	public class TgaSoftVersion : ICloneable {
		ushort versionNumber = 0;
		char versionLetter = ' ';

		/// <summary>
		/// Gets Empty <see cref="TgaSoftVersion"/>, <see cref="VersionLetter"/> = ' ' (space).
		/// </summary>
		public TgaSoftVersion() {
		}

		/// <summary>
		/// Make <see cref="TgaSoftVersion"/> from string.
		/// </summary>
		/// <param name="Str">Input string, example: "123d".</param>
		public TgaSoftVersion(string Str) {
			if (Str == null)
				throw new ArgumentNullException();
			if (Str.Length < 3 || Str.Length > 4)
				throw new ArgumentOutOfRangeException(nameof(Str.Length) + " must be equal 3 or 4!");

			bool Res = ushort.TryParse(Str.Substring(0, 3), out versionNumber);
			if (Res && Str.Length == 4)
				versionLetter = Str[3];
		}

		/// <summary>
		/// Make <see cref="TgaSoftVersion"/> from bytes.
		/// </summary>
		/// <param name="Bytes">Bytes array (byte[3]).</param>
		public TgaSoftVersion(byte[] Bytes) {
			if (Bytes == null)
				throw new ArgumentNullException(nameof(Bytes) + " = null!");
			if (Bytes.Length != Size)
				throw new ArgumentOutOfRangeException(nameof(Bytes.Length) + " must be equal " + Size + "!");

			versionNumber = BitConverter.ToUInt16(Bytes, 0);
			versionLetter = Encoding.ASCII.GetString(Bytes, 2, 1)[0];
		}

		[CLSCompliant(false)]
		public TgaSoftVersion(ushort VersionNumber, char VersionLetter = ' ') {
			versionNumber = VersionNumber;
			versionLetter = VersionLetter;
		}

		[CLSCompliant(false)]
		public ushort VersionNumber {
			get { return versionNumber; }
			set { versionNumber = value; }
		}

		public char VersionLetter {
			get { return versionLetter; }
			set { versionLetter = value; }
		}

		/// <summary>
		/// Gets TGA Field size in bytes.
		/// </summary>
		public const int Size = 3;

		/// <summary>
		/// Make full copy of <see cref="TgaSoftVersion"/>.
		/// </summary>
		/// <returns></returns>
		public TgaSoftVersion Clone() {
			return new TgaSoftVersion(versionNumber, versionLetter);
		}

		/// <summary>
		/// Make full copy of <see cref="TgaSoftVersion"/>.
		/// </summary>
		/// <returns></returns>
		object ICloneable.Clone() {
			return Clone();
		}

		public override bool Equals(object obj) {
			return ((obj is TgaSoftVersion) ? Equals((TgaSoftVersion) obj) : false);
		}

		public bool Equals(TgaSoftVersion item) {
			return (versionNumber == item.versionNumber && versionLetter == item.versionLetter);
		}

		public static bool operator ==(TgaSoftVersion item1, TgaSoftVersion item2) {
			if (ReferenceEquals(item1, null))
				return ReferenceEquals(item2, null);

			if (ReferenceEquals(item2, null))
				return ReferenceEquals(item1, null);

			return item1.Equals(item2);
		}

		public static bool operator !=(TgaSoftVersion item1, TgaSoftVersion item2) {
			return !(item1 == item2);
		}

		public override int GetHashCode() {
			return versionNumber.GetHashCode() ^ versionLetter.GetHashCode();
		}

		public override string ToString() {
			return (versionNumber.ToString("000") + versionLetter).TrimEnd(new char[] { ' ', '\0' });
		}

		/// <summary>
		/// Convert <see cref="TgaSoftVersion"/> to byte array.
		/// </summary>
		/// <returns>Byte array, <see cref="VersionNumber"/> (2 bytes) and
		/// <see cref="VersionLetter"/> (ASCII symbol).</returns>
		public byte[] ToBytes() {
			return ToBytes(versionNumber, versionLetter);
		}

		/// <summary>
		/// Convert <see cref="TgaSoftVersion"/> to byte array.
		/// </summary>
		/// <param name="VersionNumber">Set 123 for 1.23 version.</param>
		/// <param name="VersionLetter">Version letter, example: for 'a' - "1.23a".</param>
		/// <returns>Byte array, <see cref="VersionNumber"/> (2 bytes) and <see cref="VersionLetter"/> (ASCII symbol).</returns>
		[CLSCompliant(false)]
		public static byte[] ToBytes(ushort VersionNumber, char VersionLetter = ' ') {
			return BitConverterExt.ToBytes(VersionNumber, Encoding.ASCII.GetBytes(VersionLetter.ToString()));
		}
	}

	/// <summary>
	/// Use it for working with ASCII strings in TGA files.
	/// </summary>
	public class TgaString : ICloneable {
		public const string XFileSignatuteConst = "TRUEVISION-XFILE";
		public const string DotSymbolConst = ".";

		string origString = String.Empty;
		int length = 0;
		char blankSpaceChar = DefaultBlankSpaceChar;
		bool useEnding = false;

		public TgaString(bool UseEnding = false) {
			useEnding = UseEnding;
		}

		public TgaString(byte[] Bytes, bool UseEnding = false) {
			if (Bytes == null)
				throw new ArgumentNullException(nameof(Bytes) + " = null!");

			length = Bytes.Length;
			useEnding = UseEnding;
			string s = Encoding.ASCII.GetString(Bytes, 0, Bytes.Length - (useEnding ? 1 : 0));

			if (s.Length > 0)
				switch (s[s.Length - 1]) {
					case '\0':
					case ' ':
						blankSpaceChar = s[s.Length - 1];
						origString = s.TrimEnd(new char[] { s[s.Length - 1] });
						break;
					default:
						origString = s;
						break;
				}
		}

		public TgaString(int Length, bool UseEnding = false) {
			length = Length;
			useEnding = UseEnding;
		}

		public TgaString(string Str, int Length, bool UseEnding = false, char BlankSpaceChar = '\0') {
			if (Str == null)
				throw new ArgumentNullException(nameof(Str) + " = null!");

			origString = Str;
			length = Length;
			blankSpaceChar = BlankSpaceChar;
			useEnding = UseEnding;
		}

		public string OriginalString {
			get { return origString; }
			set { origString = value; }
		}

		public int Length {
			get { return length; }
			set { length = value; }
		}

		public char BlankSpaceChar {
			get { return blankSpaceChar; }
			set { blankSpaceChar = value; }
		}

		public bool UseEndingChar {
			get { return useEnding; }
			set { useEnding = value; }
		}

		/// <summary>
		/// Gets ending char, default '\0'.
		/// </summary>
		public static readonly char DefaultEndingChar = '\0';

		/// <summary>
		/// Gets blank space char, value = '\0'.
		/// </summary>
		public static readonly char DefaultBlankSpaceChar = '\0';

		/// <summary>
		/// Gets Empty <see cref="TgaString"/>.
		/// </summary>
		public static readonly TgaString Empty = new TgaString();

		/// <summary>
		/// Gets <see cref="TgaString"/> with <see cref="DefaultEndingChar"/> = '\0' and <see cref="UseEndingChar"/> = true.
		/// </summary>
		public static readonly TgaString ZeroTerminator = new TgaString(true);

		/// <summary>
		/// Gets "." <see cref="TgaString"/> with dot (period) symbol.
		/// </summary>
		public static readonly TgaString DotSymbol = new TgaString(DotSymbolConst, DotSymbolConst.Length);

		/// <summary>
		/// Gets "TRUEVISION-XFILE" <see cref="TgaString"/> (TGA File Format Version 2.0 signatute).
		/// </summary>
		public static readonly TgaString XFileSignatute = new TgaString(XFileSignatuteConst, XFileSignatuteConst.Length);

		/// <summary>
		/// Make full independed copy of <see cref="TgaString"/>.
		/// </summary>
		/// <returns>Copy of <see cref="TgaString"/></returns>
		public TgaString Clone() {
			return new TgaString(origString, length, useEnding, blankSpaceChar);
		}

		/// <summary>
		/// Make full independed copy of <see cref="TgaString"/>.
		/// </summary>
		/// <returns>Copy of <see cref="TgaString"/></returns>
		object ICloneable.Clone() {
			return Clone();
		}

		public override bool Equals(object obj) {
			return ((obj is TgaString) ? Equals((TgaString) obj) : false);
		}

		public bool Equals(TgaString item) {
			return (
				origString == item.origString &&
				length == item.length &&
				blankSpaceChar == item.blankSpaceChar &&
				useEnding == item.useEnding);
		}

		public static bool operator ==(TgaString item1, TgaString item2) {
			if (ReferenceEquals(item1, null))
				return ReferenceEquals(item2, null);

			if (ReferenceEquals(item2, null))
				return ReferenceEquals(item1, null);

			return item1.Equals(item2);
		}

		public static bool operator !=(TgaString item1, TgaString item2) {
			return !(item1 == item2);
		}

		public static TgaString operator +(TgaString item1, TgaString item2) {
			if (ReferenceEquals(item1, null) || ReferenceEquals(item2, null))
				throw new ArgumentNullException();

			return new TgaString(BitConverterExt.ToBytes(item1.ToBytes(), item2.ToBytes()));
		}

		public override int GetHashCode() {
			unchecked {
				int hash = 17;
				hash = hash * 23 + origString.GetHashCode();
				hash = hash * 23 + length.GetHashCode();
				hash = hash * 23 + blankSpaceChar.GetHashCode();
				hash = hash * 23 + useEnding.GetHashCode();
				return hash;
			}
		}

		/// <summary>
		/// Get ASCII-Like string with string-terminators, example: "Some string\0\0\0\0\0".
		/// </summary>
		/// <returns>String with replaced string-terminators to "\0".</returns>
		public override string ToString() {
			return Encoding.ASCII.GetString(ToBytes()).Replace("\0", @"\0");
		}

		/// <summary>
		/// Get ASCII-Like string to first string-terminator, example:
		/// "Some string \0 Some Data \0" - > "Some string".
		/// </summary>
		/// <returns>String to first string-terminator.</returns>
		public string GetString() {
			String Str = Encoding.ASCII.GetString(ToBytes());
			int EndIndex = Str.IndexOf('\0');
			if (EndIndex != -1)
				Str = Str.Substring(0, EndIndex);
			return Str;
		}

		/// <summary>
		/// Convert <see cref="TgaString"/> to byte array.
		/// </summary>
		/// <returns>Byte array, every byte is ASCII symbol.</returns>
		public byte[] ToBytes() {
			return ToBytes(origString, length, useEnding, blankSpaceChar);
		}

		/// <summary>
		/// Convert <see cref="TgaString"/> to byte array.
		/// </summary>
		/// <param name="str">Input string.</param>
		/// <param name="Length">Length of output ASCII string with Ending char (if used).</param>
		/// <param name="UseEnding">Add EndingChr to string or not?</param>
		/// <param name="BlankSpaceChar">Char for filling blank space in string. If this char is '-' (only for example!),
		/// for string "ABC" with <see cref="Length"/> = 7, with UseEnding = true,
		/// <see cref="DefaultEndingChar"/> is '\0', result string is "ABC---\0".</param>
		/// <returns>Byte array, every byte is ASCII symbol.</returns>
		public static byte[] ToBytes(string str, int Length, bool UseEnding = true, char BlankSpaceChar = '\0') {
			char[] C = new char[Math.Max(Length, (UseEnding ? 1 : 0))];

			for (int i = 0; i < C.Length; i++)
				C[i] = (i < str.Length ? str[i] : BlankSpaceChar);

			if (UseEnding)
				C[C.Length - 1] = DefaultEndingChar;

			return Encoding.ASCII.GetBytes(C);
		}
	}

	public class TgaTime : ICloneable {
		ushort hours = 0;
		ushort minutes = 0;
		ushort seconds = 0;

		/// <summary>
		/// Make empty <see cref="TgaTime"/>.
		/// </summary>
		public TgaTime() {
		}

		/// <summary>
		/// Make <see cref="TgaTime"/> from <see cref="TimeSpan"/>.
		/// </summary>
		/// <param name="Time">Some <see cref="TimeSpan"/> variable.</param>
		public TgaTime(TimeSpan Time) {
			hours = (ushort) Time.TotalHours;
			minutes = (ushort) Time.Minutes;
			seconds = (ushort) Time.Seconds;
		}

		/// <summary>
		/// Make <see cref="TgaTime"/> from ushort values.
		/// </summary>
		/// <param name="Hours">Hour (0 - 65535).</param>
		/// <param name="Minutes">Minute (0 - 59).</param>
		/// <param name="Seconds">Second (0 - 59).</param>
		[CLSCompliant(false)]
		public TgaTime(ushort Hours, ushort Minutes, ushort Seconds) {
			hours = Hours;
			minutes = Minutes;
			seconds = Seconds;
		}

		/// <summary>
		/// Make <see cref="TgaTime"/> from bytes.
		/// </summary>
		/// <param name="Bytes">Array of bytes(byte[6]).</param>
		public TgaTime(byte[] Bytes) {
			if (Bytes == null)
				throw new ArgumentNullException(nameof(Bytes) + " = null!");
			else if (Bytes.Length != Size)
				throw new ArgumentOutOfRangeException(nameof(Bytes) + " must be equal " + Size + "!");

			hours = BitConverter.ToUInt16(Bytes, 0);
			minutes = BitConverter.ToUInt16(Bytes, 2);
			seconds = BitConverter.ToUInt16(Bytes, 4);
		}

		/// <summary>
		/// Gets or Sets hour (0 - 65535).
		/// </summary>
		[CLSCompliant(false)]
		public ushort Hours {
			get { return hours; }
			set { hours = value; }
		}

		/// <summary>
		/// Gets or Sets minute (0 - 59).
		/// </summary>
		[CLSCompliant(false)]
		public ushort Minutes {
			get { return minutes; }
			set { minutes = value; }
		}

		/// <summary>
		/// Gets or Sets second (0 - 59).
		/// </summary>
		[CLSCompliant(false)]
		public ushort Seconds {
			get { return seconds; }
			set { seconds = value; }
		}

		/// <summary>
		/// Gets TGA Field size in bytes.
		/// </summary>
		public const int Size = 6;

		/// <summary>
		/// Make full independed copy of <see cref="TgaTime"/>.
		/// </summary>
		/// <returns>Copy of <see cref="TgaTime"/></returns>
		public TgaTime Clone() {
			return new TgaTime(hours, minutes, seconds);
		}

		/// <summary>
		/// Make full independed copy of <see cref="TgaTime"/>.
		/// </summary>
		/// <returns>Copy of <see cref="TgaTime"/></returns>
		object ICloneable.Clone() {
			return Clone();
		}

		public override bool Equals(object obj) {
			return ((obj is TgaTime) ? Equals((TgaTime) obj) : false);
		}

		public bool Equals(TgaTime item) {
			return (hours == item.hours && minutes == item.minutes && seconds == item.seconds);
		}

		public static bool operator ==(TgaTime item1, TgaTime item2) {
			if (ReferenceEquals(item1, null))
				return ReferenceEquals(item2, null);

			if (ReferenceEquals(item2, null))
				return ReferenceEquals(item1, null);

			return item1.Equals(item2);
		}

		public static bool operator !=(TgaTime item1, TgaTime item2) {
			return !(item1 == item2);
		}

		public override int GetHashCode() {
			unchecked {
				int hash = 17;
				hash = hash * 23 + hours.GetHashCode();
				hash = hash * 23 + (minutes << 16 | seconds).GetHashCode();
				return hash;
			}
		}

		/// <summary>
		/// Gets <see cref="TgaTime"/> like string.
		/// </summary>
		/// <returns>String in "H:M:S" format.</returns>
		public override string ToString() {
			return String.Format("{0}:{1}:{2}", hours, minutes, seconds);
		}

		/// <summary>
		/// Convert <see cref="TgaTime"/> to byte array.
		/// </summary>
		/// <returns>Byte array with length = 6.</returns>
		public byte[] ToBytes() {
			return BitConverterExt.ToBytes(hours, minutes, seconds);
		}

		/// <summary>
		/// Gets <see cref="TgaTime"/> like <see cref="TimeSpan"/>.
		/// </summary>
		/// <returns><see cref="TimeSpan"/> value of <see cref="TgaTime"/>.</returns>
		public TimeSpan ToTimeSpan() {
			return new TimeSpan(hours, minutes, seconds);
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// File Header Area (18 bytes)
	/// </summary>
	public class TgaHeader : ICloneable {
		byte idLength = 0;
		TgaColorMapType colorMapType = TgaColorMapType.NoColorMap;
		TgaImageType imageType = TgaImageType.NoImageData;
		TgaColorMapSpec colorMapSpec = new TgaColorMapSpec();
		TgaImageSpec imageSpec = new TgaImageSpec();

		/// <summary>
		/// Make empty <see cref="TgaHeader"/>.
		/// </summary>
		public TgaHeader() {
		}

		/// <summary>
		/// Make <see cref="TgaHeader"/> from bytes.
		/// </summary>
		/// <param name="Bytes">Bytes array (byte[18]).</param>
		public TgaHeader(byte[] Bytes) {
			if (Bytes == null)
				throw new ArgumentNullException(nameof(Bytes) + " = null!");
			if (Bytes.Length != Size)
				throw new ArgumentOutOfRangeException(nameof(Bytes.Length) + " must be equal " + Size + "!");

			idLength = Bytes[0];
			colorMapType = (TgaColorMapType) Bytes[1];
			imageType = (TgaImageType) Bytes[2];
			colorMapSpec = new TgaColorMapSpec(BitConverterExt.GetElements(Bytes, 3, TgaColorMapSpec.Size));
			imageSpec = new TgaImageSpec(BitConverterExt.GetElements(Bytes, 8, TgaImageSpec.Size));
		}

		/// <summary>
		/// ID Length - Field 1 (1 byte):
		/// This field identifies the number of bytes contained in the ImageID Field.
		/// The maximum number of characters is 255. A value of zero indicates that no Image ID
		/// field is included with the image.
		/// </summary>
		public byte IDLength {
			get { return idLength; }
			set { idLength = value; }
		}

		/// <summary>
		/// Color Map Type - Field 2 (1 byte):
		/// This field indicates the type of color map (if any) included with the image.
		/// There are currently 2 defined values for this field:
		/// <para>0 - indicates that no color-map data is included with this image;</para>
		/// <para>1 - indicates that a color-map is included with this image.</para>
		/// </summary>
		public TgaColorMapType ColorMapType {
			get { return colorMapType; }
			set { colorMapType = value; }
		}

		/// <summary>
		/// Image Type - Field 3 (1 byte):
		/// <para>The TGA File Format can be used to store Pseudo-Color, True-Color and Direct-Color images
		/// of various pixel depths.</para>
		/// </summary>
		public TgaImageType ImageType {
			get { return imageType; }
			set { imageType = value; }
		}

		/// <summary>
		/// Color Map Specification - Field 4 (5 bytes):
		/// <para>This field and its sub-fields describe the color map (if any) used for the image.
		/// If the Color Map Type field is set to zero, indicating that no color map exists, then
		/// these 5 bytes should be set to zero. These bytes always must be written to the file.</para>
		/// </summary>
		public TgaColorMapSpec ColorMapSpec {
			get { return colorMapSpec; }
			set { colorMapSpec = value; }
		}

		/// <summary>
		/// Image Specification - Field 5 (10 bytes):
		/// <para>This field and its sub-fields describe the image screen location, size and pixel depth.
		/// These information is always written to the file.</para>
		/// </summary>
		public TgaImageSpec ImageSpec {
			get { return imageSpec; }
			set { imageSpec = value; }
		}

		/// <summary>
		/// Gets TGA Header Section size in bytes.
		/// </summary>
		public const int Size = 18;

		/// <summary>
		/// Make full copy of <see cref="TgaHeader"/>.
		/// </summary>
		/// <returns>Full independent copy of <see cref="TgaHeader"/>.</returns>
		public TgaHeader Clone() {
			return new TgaHeader(ToBytes());
		}

		/// <summary>
		/// Make full copy of <see cref="TgaHeader"/>.
		/// </summary>
		/// <returns>Full independent copy of <see cref="TgaHeader"/>.</returns>
		object ICloneable.Clone() {
			return Clone();
		}

		public override bool Equals(object obj) {
			return ((obj is TgaHeader) ? Equals((TgaHeader) obj) : false);
		}

		public bool Equals(TgaHeader item) {
			return (idLength == item.idLength &&
				colorMapType == item.colorMapType &&
				imageType == item.imageType &&
				colorMapSpec == item.colorMapSpec &&
				imageSpec == item.imageSpec);
		}

		public static bool operator ==(TgaHeader item1, TgaHeader item2) {
			if (ReferenceEquals(item1, null))
				return ReferenceEquals(item2, null);

			if (ReferenceEquals(item2, null))
				return ReferenceEquals(item1, null);

			return item1.Equals(item2);
		}

		public static bool operator !=(TgaHeader item1, TgaHeader item2) {
			return !(item1 == item2);
		}

		public override int GetHashCode() {
			unchecked {
				int hash = 17;
				hash = hash * 23 + (idLength << 24 | (byte) colorMapType << 8 | (byte) imageType).GetHashCode();

				if (colorMapSpec != null)
					hash = hash * 23 + colorMapSpec.GetHashCode();

				if (imageSpec != null)
					hash = hash * 23 + imageSpec.GetHashCode();

				return hash;
			}
		}

		public override string ToString() {
			return String.Format("{0}={1}, {2}={3}, {4}={5}, {6}={7}, {8}={9}",
				nameof(IDLength), idLength,
				nameof(ColorMapType), colorMapType,
				nameof(ImageType), imageType,
				nameof(ColorMapSpec), colorMapSpec,
				nameof(ImageSpec), imageSpec);
		}

		/// <summary>
		/// Convert <see cref="TgaHeader"/> to byte array.
		/// </summary>
		/// <returns>Byte array with size equal <see cref="Size"/>.</returns>
		public byte[] ToBytes() {
			return BitConverterExt.ToBytes(idLength, (byte) colorMapType, (byte) imageType,
				(colorMapSpec == null ? new byte[TgaColorMapSpec.Size] : colorMapSpec.ToBytes()),
				(imageSpec == null ? new byte[TgaImageSpec.Size] : imageSpec.ToBytes()));
		}
	}

	/// <summary>
	/// Image Or ColorMap Area
	/// </summary>
	public class TgaImgOrColMap : ICloneable {
		TgaString imageID = null;
		byte[] colorMapData = null;
		byte[] imageData = null;

		/// <summary>
		/// Make empty <see cref="TgaImgOrColMap"/>.
		/// </summary>
		public TgaImgOrColMap() {
		}

		/// <summary>
		/// Make <see cref="TgaImgOrColMap"/> from arrays.
		/// </summary>
		/// <param name="ImageID">This optional field contains identifying information about the image.
		/// The maximum length for this field is 255 bytes. Refer to <see cref="TgaHeader.IDLength"/>
		/// for the length of this field. If field 1 is set to Zero indicating that no Image ID exists
		/// then these bytes are not written to the file.</param>
		/// <param name="ColorMapData">Color Map Data, see <see cref="ColorMapData"/> description.</param>
		/// <param name="ImageData">Image Data, see <see cref="ImageData"/> description.</param>
		public TgaImgOrColMap(TgaString ImageID, byte[] ColorMapData, byte[] ImageData) {
			imageID = ImageID;
			colorMapData = ColorMapData;
			imageData = ImageData;
		}

		/// <summary>
		/// Image ID - Field 6 (variable):
		/// <para>This optional field contains identifying information about the image. The maximum length
		/// for this field is 255 bytes. Refer to <see cref="TgaHeader.IDLength"/> for the length of this
		/// field. If field 1 is set to Zero indicating that no Image ID exists then these bytes are not
		/// written to the file. Can have text inside (ASCII).</para>
		/// </summary>
		public TgaString ImageID {
			get { return imageID; }
			set { imageID = value; }
		}

		/// <summary>
		/// Color Map Data - Field 7 (variable):
		/// <para>If the Color Map Type(field 2) field is set to zero indicating that no Color-Map
		/// exists then this field will not be present (i.e., no bytes written to the file).</para>
		/// <para>This variable-length field contains the actual color map information (LUT data).
		/// Field 4.3 specifies the width in bits of each color map entry while Field 4.2 specifies
		/// the number of color map entries in this field. These two fields together are used to
		/// determine the number of bytes contained in field 7.</para>
		/// <para>Each color map entry is stored using an integral number of bytes.The RGB specification
		/// for each color map entry is stored in successive bit-fields in the multi-byte entries.
		/// Each color bit-field is assumed to be MIN(Field4.3/3, 8) bits in length. If Field 4.3
		/// contains 24, then each color specification is 8 bits in length; if Field 4.3 contains 32,
		/// then each color specification is also 8 bits (32/3 gives 10, but 8 is smaller).
		/// Unused bit(s) in the multi-byte entries are assumed to specify attribute bits. The
		/// attribute bit field is often called the Alpha Channel, Overlay Bit(s) or Interrupt Bit(s).</para>
		/// For the TARGA M-8, ATVista and NuVista, the number of bits in a color map specification is
		/// 24 (or 32). The red, green, and blue components are each represented by one byte.
		/// </summary>
		public byte[] ColorMapData {
			get { return colorMapData; }
			set { colorMapData = value; }
		}

		/// <summary>
		/// Image Data - Field 8 (variable):
		/// <para>This field contains (Width)x(Height) pixels. Each pixel specifies image data in one
		/// of the following formats:</para>
		/// <para>a single color-map index for Pseudo-Color;
		/// Attribute, Red, Green and Blue ordered data for True-Color;
		/// and independent color-map indices for Direct-Color.</para>
		/// <para>The values for Width and Height are specified in Fields 5.3 and 5.4 respectively.
		/// The number of attribute and color-definition bits for each pixel are defined in Fields 5.6
		/// and 5.5, respectively.Each pixel is stored as an integral number of bytes.</para>
		/// </summary>
		public byte[] ImageData {
			get { return imageData; }
			set { imageData = value; }
		}

		/// <summary>
		/// Make full copy of <see cref="TgaImgOrColMap"/>.
		/// </summary>
		/// <returns>Full independed copy of <see cref="TgaImgOrColMap"/>.</returns>
		public TgaImgOrColMap Clone() {
			return new TgaImgOrColMap(
				(imageID == null ? null : imageID.Clone()),
				(colorMapData == null ? null : (byte[]) colorMapData.Clone()),
				(imageData == null ? null : (byte[]) imageData.Clone()));
		}

		/// <summary>
		/// Make full copy of <see cref="TgaImgOrColMap"/>.
		/// </summary>
		/// <returns>Full independed copy of <see cref="TgaImgOrColMap"/>.</returns>
		object ICloneable.Clone() {
			return Clone();
		}

		public override bool Equals(object obj) {
			return ((obj is TgaImgOrColMap) ? Equals((TgaImgOrColMap) obj) : false);
		}

		public bool Equals(TgaImgOrColMap item) {
			return imageID == item.imageID &&
				BitConverterExt.IsArraysEqual(colorMapData, item.colorMapData) &&
				BitConverterExt.IsArraysEqual(imageData, item.imageData);
		}

		public static bool operator ==(TgaImgOrColMap item1, TgaImgOrColMap item2) {
			if (ReferenceEquals(item1, null))
				return ReferenceEquals(item2, null);

			if (ReferenceEquals(item2, null))
				return ReferenceEquals(item1, null);

			return item1.Equals(item2);
		}

		public static bool operator !=(TgaImgOrColMap item1, TgaImgOrColMap item2) {
			return !(item1 == item2);
		}

		public override int GetHashCode() {
			unchecked {
				int hash = 27;

				if (imageID != null)
					hash = (13 * hash) + imageID.GetHashCode();
				if (colorMapData != null)
					for (int i = 0; i < colorMapData.Length; i++)
						hash = (13 * hash) + colorMapData[i].GetHashCode();
				if (imageData != null)
					for (int i = 0; i < imageData.Length; i++)
						hash = (13 * hash) + imageData[i].GetHashCode();

				return hash;
			}
		}
	} //No ToBytes()

	/// <summary>
	/// Developer Area
	/// </summary> //?
	public class TgaDevArea : ICloneable {
		List<TgaDevEntry> entries = new List<TgaDevEntry>();

		public TgaDevArea() {
		}

		public TgaDevArea(List<TgaDevEntry> Entries) {
			if (Entries == null)
				throw new ArgumentNullException(nameof(Entries) + " = null!");

			entries = Entries;
		}

		/// <summary>
		/// Developer Data - Field 9 (variable):
		/// </summary>
		public List<TgaDevEntry> Entries {
			get { return entries; }
			set { entries = value; }
		}

		public int Count {
			get { return entries.Count; }
		}

		public TgaDevEntry this[int index] {
			get { return entries[index]; }
			set { entries[index] = value; }
		}

		/// <summary>
		/// Make full copy of <see cref="TgaDevArea"/>.
		/// </summary>
		/// <returns>Full independent copy of <see cref="TgaDevArea"/>.</returns>
		public TgaDevArea Clone() {
			if (entries == null)
				return new TgaDevArea(null);

			List<TgaDevEntry> L = new List<TgaDevEntry>();
			for (int i = 0; i < entries.Count; i++)
				L.Add(entries[i].Clone());

			return new TgaDevArea(L);
		}

		/// <summary>
		/// Make full copy of <see cref="TgaDevArea"/>.
		/// </summary>
		/// <returns>Full independent copy of <see cref="TgaDevArea"/>.</returns>
		object ICloneable.Clone() {
			return Clone();
		}

		public override bool Equals(object obj) {
			return ((obj is TgaDevArea) ? Equals((TgaDevArea) obj) : false);
		}

		public bool Equals(TgaDevArea item) {
			return BitConverterExt.IsListsEqual(entries, item.entries);
		}

		public static bool operator ==(TgaDevArea item1, TgaDevArea item2) {
			if (ReferenceEquals(item1, null))
				return ReferenceEquals(item2, null);

			if (ReferenceEquals(item2, null))
				return ReferenceEquals(item1, null);

			return item1.Equals(item2);
		}

		public static bool operator !=(TgaDevArea item1, TgaDevArea item2) {
			return !(item1 == item2);
		}

		public override int GetHashCode() {
			unchecked {
				int hash = 27;
				if (entries != null)
					for (int i = 0; i < entries.Count; i++)
						hash = (13 * hash) + entries[i].GetHashCode();
				return hash;
			}
		}

		/// <summary>
		/// Convert <see cref="TgaDevArea"/> (without Fields Data, only Directory!) to byte array.
		/// </summary>
		/// <returns>Byte array, Len = (NUMBER_OF_TAGS_IN_THE_DIRECTORY * 10) + 2 bytes in size.
		/// The "+ 2" includes the 2 bytes for the number of tags in the directory.</returns>
		public byte[] ToBytes() {
			if (entries == null)
				throw new Exception(nameof(Entries) + " = null!");

			ushort NumberOfEntries = (ushort) Math.Min(ushort.MaxValue, entries.Count);
			List<byte> DevDir = new List<byte>(BitConverter.GetBytes(NumberOfEntries));

			for (int i = 0; i < entries.Count; i++) {
				DevDir.AddRange(BitConverter.GetBytes(entries[i].Tag));
				DevDir.AddRange(BitConverter.GetBytes(entries[i].Offset));
				DevDir.AddRange(BitConverter.GetBytes(entries[i].FieldSize));
			}

			return DevDir.ToArray();
		}
	} //Not full ToBytes()

	/// <summary>
	/// Extension Area
	/// </summary>
	public class TgaExtArea : ICloneable {
		public const int MinSize = 495; //bytes

		ushort extensionSize = MinSize;
		TgaString authorName = new TgaString(41, true);
		TgaComment authorComments = new TgaComment();
		TgaDateTime dateTimeStamp = new TgaDateTime();
		TgaString jobNameOrID = new TgaString(41, true);
		TgaTime jobTime = new TgaTime();
		TgaString softwareID = new TgaString(41, true);
		TgaSoftVersion softVersion = new TgaSoftVersion();
		TgaColorKey keyColor = new TgaColorKey();
		TgaFraction pixelAspectRatio = TgaFraction.Empty;
		TgaFraction gammaValue = TgaFraction.Empty;
		uint colorCorrectionOffset = 0;
		uint postageStampOffset = 0;
		uint scanLineOffset = 0;
		TgaAttrType attributesType = TgaAttrType.NoAlpha;
		uint[] scanLineTable = null;
		TgaPostageStampImage postageStampImage = null;
		ushort[] colorCorrectionTable = null;
		byte[] otherDataInExtensionArea = null;

		public TgaExtArea() {
		}

		/// <summary>
		/// Make <see cref="TgaExtArea"/> from bytes. Warning: <see cref="ScanLineTable"/>,
		/// <see cref="PostageStampImage"/>, <see cref="ColorCorrectionTable"/> not included,
		/// because thea are can be not in the Extension Area of TGA file!
		/// </summary>
		/// <param name="Bytes">Bytes of <see cref="TgaExtArea"/>.</param>
		/// <param name="SLT">Scan Line Table.</param>
		/// <param name="PostImg">Postage Stamp Image.</param>
		/// <param name="CCT">Color Correction Table.</param>
		[CLSCompliant(false)]
		public TgaExtArea(byte[] Bytes, uint[] SLT = null, TgaPostageStampImage PostImg = null, ushort[] CCT = null) {
			if (Bytes == null)
				throw new ArgumentNullException(nameof(Bytes) + " = null!");
			if (Bytes.Length < MinSize)
				throw new ArgumentOutOfRangeException(nameof(Bytes.Length) + " must be >= " + MinSize + "!");

			extensionSize = BitConverter.ToUInt16(Bytes, 0);
			authorName = new TgaString(BitConverterExt.GetElements(Bytes, 2, 41), true);
			authorComments = new TgaComment(BitConverterExt.GetElements(Bytes, 43, TgaComment.Size));
			dateTimeStamp = new TgaDateTime(BitConverterExt.GetElements(Bytes, 367, TgaDateTime.Size));
			jobNameOrID = new TgaString(BitConverterExt.GetElements(Bytes, 379, 41), true);
			jobTime = new TgaTime(BitConverterExt.GetElements(Bytes, 420, TgaTime.Size));
			softwareID = new TgaString(BitConverterExt.GetElements(Bytes, 426, 41), true);
			softVersion = new TgaSoftVersion(BitConverterExt.GetElements(Bytes, 467, TgaSoftVersion.Size));
			keyColor = new TgaColorKey(BitConverterExt.GetElements(Bytes, 470, TgaColorKey.Size));
			pixelAspectRatio = new TgaFraction(BitConverterExt.GetElements(Bytes, 474, TgaFraction.Size));
			gammaValue = new TgaFraction(BitConverterExt.GetElements(Bytes, 478, TgaFraction.Size));
			colorCorrectionOffset = BitConverter.ToUInt32(Bytes, 482);
			postageStampOffset = BitConverter.ToUInt32(Bytes, 486);
			scanLineOffset = BitConverter.ToUInt32(Bytes, 490);
			attributesType = (TgaAttrType) Bytes[494];

			if (extensionSize > MinSize)
				otherDataInExtensionArea = BitConverterExt.GetElements(Bytes, 495, Bytes.Length - MinSize);

			scanLineTable = SLT;
			postageStampImage = PostImg;
			colorCorrectionTable = CCT;
		}

		#region Properties
		/// <summary>
		/// Extension Size - Field 10 (2 Bytes):
		/// This field is a SHORT field which specifies the number of BYTES in the fixedlength portion of
		/// the Extension Area. For Version 2.0 of the TGA File Format, this number should be set to 495.
		/// If the number found in this field is not 495, then the file will be assumed to be of a
		/// version other than 2.0. If it ever becomes necessary to alter this number, the change
		/// will be controlled by Truevision, and will be accompanied by a revision to the TGA File
		/// Format with an accompanying change in the version number.
		/// </summary>
		[CLSCompliant(false)]
		public ushort ExtensionSize {
			get { return extensionSize; }
			set { extensionSize = value; }
		}

		/// <summary>
		/// Author Name - Field 11 (41 Bytes):
		/// Bytes 2-42 - This field is an ASCII field of 41 bytes where the last byte must be a null
		/// (binary zero). This gives a total of 40 ASCII characters for the name. If the field is used,
		/// it should contain the name of the person who created the image (author). If the field is not
		/// used, you may fill it with nulls or a series of blanks(spaces) terminated by a null.
		/// The 41st byte must always be a null.
		/// </summary>
		public TgaString AuthorName {
			get { return authorName; }
			set { authorName = value; }
		}

		/// <summary>
		/// Author Comments - Field 12 (324 Bytes):
		/// Bytes 43-366 - This is an ASCII field consisting of 324 bytes which are organized as four lines
		/// of 80 characters, each followed by a null terminator.This field is provided, in addition to the
		/// original IMAGE ID field(in the original TGA format), because it was determined that a few
		/// developers had used the IMAGE ID field for their own purposes.This field gives the developer
		/// four lines of 80 characters each, to use as an Author Comment area. Each line is fixed to 81
		/// bytes which makes access to the four lines easy.Each line must be terminated by a null.
		/// If you do not use all 80 available characters in the line, place the null after the last
		/// character and blank or null fill the rest of the line. The 81st byte of each of the four
		/// lines must be null.
		/// </summary>
		public TgaComment AuthorComments {
			get { return authorComments; }
			set { authorComments = value; }
		}

		/// <summary>
		/// Date/Time Stamp - Field 13 (12 Bytes):
		/// Bytes 367-378 - This field contains a series of 6 SHORT values which define the integer
		/// value for the date and time that the image was saved. This data is formatted as follows:
		/// <para>SHORT 0: Month(1 - 12)</para>
		/// <para>SHORT 1: Day(1 - 31)</para>
		/// <para>SHORT 2: Year(4 digit, ie. 1989)</para>
		/// <para>SHORT 3: Hour(0 - 23)</para>
		/// <para>SHORT 4: Minute(0 - 59)</para>
		/// <para>SHORT 5: Second(0 - 59)</para>
		/// Even though operating systems typically time- and date-stamp files, this feature is
		/// provided because the operating system may change the time and date stamp if the file is
		/// copied. By using this area, you are guaranteed an unmodified region for date and time
		/// recording. If the fields are not used, you should fill them with binary zeros (0).
		/// </summary>
		public TgaDateTime DateTimeStamp {
			get { return dateTimeStamp; }
			set { dateTimeStamp = value; }
		}

		/// <summary>
		/// Job Name/ID - Field 14 (41 Bytes):
		/// Bytes 379-419 - This field is an ASCII field of 41 bytes where the last byte must be 
		/// a binary zero. This gives a total of 40 ASCII characters for the job name or the ID.
		/// If the field is used, it should contain a name or id tag which refers to the job with
		/// which the image was associated.This allows production companies (and others) to tie
		/// images with jobs by using this field as a job name (i.e., CITY BANK) or job id number
		/// (i.e., CITY023). If the field is not used, you may fill it with a null terminated series
		/// of blanks (spaces) or nulls. In any case, the 41st byte must be a null.
		/// </summary>
		public TgaString JobNameOrID {
			get { return jobNameOrID; }
			set { jobNameOrID = value; }
		}

		/// <summary>
		/// Job Time - Field 15 (6 Bytes):
		/// Bytes 420-425 - This field contains a series of 3 SHORT values which define the integer
		/// value for the job elapsed time when the image was saved.This data is formatted as follows:
		/// <para>SHORT 0: Hours(0 - 65535)</para>
		/// <para>SHORT 1: Minutes(0 - 59)</para>
		/// <para>SHORT 2: Seconds(0 - 59)</para>
		/// The purpose of this field is to allow production houses (and others) to keep a running total
		/// of the amount of time invested in a particular image. This may be useful for billing, costing,
		/// and time estimating. If the fields are not used, you should fill them with binary zeros (0).
		/// </summary>
		public TgaTime JobTime {
			get { return jobTime; }
			set { jobTime = value; }
		}

		/// <summary>
		/// Software ID - Field 16 (41 Bytes):
		/// Bytes 426-466 - This field is an ASCII field of 41 bytes where the last byte must be
		/// a binary zero (null). This gives a total of 40 ASCII characters for the Software ID.
		/// The purpose of this field is to allow software to determine and record with what program
		/// a particular image was created.If the field is not used, you may fill it with a
		/// null terminated series of blanks (spaces) or nulls. The 41st byte must always be a null.
		/// </summary>
		public TgaString SoftwareID {
			get { return softwareID; }
			set { softwareID = value; }
		}

		/// <summary>
		/// Software Version - Field 17 (3 Bytes):
		/// Bytes 467-469 - This field consists of two sub-fields, a SHORT and an ASCII BYTE.
		/// The purpose of this field is to define the version of software defined by the
		/// “Software ID” field above. The SHORT contains the version number as a binary
		/// integer times 100.
		/// <para>Therefore, software version 4.17 would be the integer value 417.This allows for
		/// two decimal positions of sub-version.The ASCII BYTE supports developers who also
		/// tag a release letter to the end. For example, if the version number is 1.17b, then
		/// the SHORT would contain 117. and the ASCII BYTE would contain “b”.
		/// The organization is as follows:</para>
		/// <para>SHORT (Bytes 0 - 1): Version Number * 100</para>
		/// <para>BYTE(Byte 2): Version Letter</para>
		/// If you do not use this field, set the SHORT to binary zero, and the BYTE to a space(“ “)
		/// </summary>
		public TgaSoftVersion SoftVersion {
			get { return softVersion; }
			set { softVersion = value; }
		}

		/// <summary>
		/// Key Color - Field 18 (4 Bytes):
		/// Bytes 470-473 - This field contains a long value which is the key color in effect at
		/// the time the image is saved. The format is in A:R:G:B where ‘A’ (most significant byte)
		/// is the alpha channel key color(if you don’t have an alpha channel in your application,
		/// keep this byte zero [0]).
		/// <para>The Key Color can be thought of as the ‘background color’ or ‘transparent color’.
		/// This is the color of the ‘non image’ area of the screen, and the same color that the
		/// screen would be cleared to if erased in the application. If you don’t use this field,
		/// set it to all zeros (0). Setting the field to all zeros is the same as selecting a key
		/// color of black.</para>
		/// A good example of a key color is the ‘transparent color’ used in TIPS™ for WINDOW loading/saving.
		/// </summary>
		public TgaColorKey KeyColor {
			get { return keyColor; }
			set { keyColor = value; }
		}

		/// <summary>
		/// Pixel Aspect Ratio - Field 19 (4 Bytes):
		/// Bytes 474-477 - This field contains two SHORT sub-fields, which when taken together
		/// specify a pixel size ratio.The format is as follows:
		/// <para>SHORT 0: Pixel Ratio Numerator(pixel width)</para>
		/// <para>SHORT 1: Pixel Ratio Denominator(pixel height)</para>
		/// These sub-fields may be used to determine the aspect ratio of a pixel. This is useful when
		/// it is important to preserve the proper aspect ratio of the saved image. If the two values
		/// are set to the same non-zero value, then the image is composed of square pixels. A zero
		/// in the second sub-field (denominator) indicates that no pixel aspect ratio is specified.
		/// </summary>
		public TgaFraction PixelAspectRatio {
			get { return pixelAspectRatio; }
			set { pixelAspectRatio = value; }
		}

		/// <summary>
		/// Gamma Value - Field 20 (4 Bytes):
		/// Bytes 478-481 - This field contains two SHORT sub-fields, which when taken together in a ratio,
		/// provide a fractional gamma value.The format is as follows:
		/// <para>SHORT 0: Gamma Numerator</para>
		/// <para>SHORT 1: Gamma Denominator</para>
		/// The resulting value should be in the range of 0.0 to 10.0, with only one decimal place of
		/// precision necessary. An uncorrected image (an image with no gamma) should have the value 1.0 as
		/// the result.This may be accomplished by placing thesame, non-zero values in both positions
		/// (i.e., 1/1). If you decide to totally ignore this field, please set the denominator (the second
		/// SHORT) to the value zero. This will indicate that the Gamma Value field is not being used.
		/// </summary>
		public TgaFraction GammaValue {
			get { return gammaValue; }
			set { gammaValue = value; }
		}

		/// <summary>
		/// Color Correction Offset - Field 21 (4 Bytes):
		/// Bytes 482-485 - This field is a 4-byte field containing a single offset value. This is an offset
		/// from the beginning of the file to the start of the Color Correction table. This table may be
		/// written anywhere between the end of the Image Data field (field 8) and the start of the TGA
		/// File Footer. If the image has no Color Correction Table or if the Gamma Value setting is
		/// sufficient, set this value to zero and do not write a Correction Table anywhere.
		/// </summary>
		[CLSCompliant(false)]
		public uint ColorCorrectionTableOffset {
			get { return colorCorrectionOffset; }
			set { colorCorrectionOffset = value; }
		}

		/// <summary>
		/// Postage Stamp Offset - Field 22 (4 Bytes):
		/// Bytes 486-489 - This field is a 4-byte field containing a single offset value. This is an offset
		/// from the beginning of the file to the start of the Postage Stamp Image. The Postage Stamp Image
		/// must be written after Field 25 (Scan Line Table) but before the start of the TGA File Footer.
		/// If no postage stamp is stored, set this field to the value zero (0).
		/// </summary>
		[CLSCompliant(false)]
		public uint PostageStampOffset {
			get { return postageStampOffset; }
			set { postageStampOffset = value; }
		}

		/// <summary>
		/// Scan Line Offset - Field 23 (4 Bytes):
		/// Bytes 490-493 - This field is a 4-byte field containing a single offset value. This is an
		/// offset from the beginning of the file to the start of the Scan Line Table.
		/// </summary>
		[CLSCompliant(false)]
		public uint ScanLineOffset {
			get { return scanLineOffset; }
			set { scanLineOffset = value; }
		}

		/// <summary>
		/// Attributes Type - Field 24 (1 Byte):
		/// Byte 494 - This single byte field contains a value which specifies the type of Alpha channel
		/// data contained in the file. Value Meaning:
		/// <para>0: no Alpha data included (bits 3-0 of field 5.6 should also be set to zero)</para>
		/// <para>1: undefined data in the Alpha field, can be ignored</para>
		/// <para>2: undefined data in the Alpha field, but should be retained</para>
		/// <para>3: useful Alpha channel data is present</para>
		/// <para>4: pre-multiplied Alpha(see description below)</para>
		/// <para>5 -127: RESERVED</para>
		/// <para>128-255: Un-assigned</para>
		/// <para>Pre-multiplied Alpha Example: Suppose the Alpha channel data is being used to specify the
		/// opacity of each pixel(for use when the image is overlayed on another image), where 0 indicates
		/// that the pixel is completely transparent and a value of 1 indicates that the pixel is
		/// completely opaque(assume all component values have been normalized).</para>
		/// <para>A quadruple(a, r, g, b) of( 0.5, 1, 0, 0) would indicate that the pixel is pure red with a
		/// transparency of one-half. For numerous reasons(including image compositing) is is better to
		/// pre-multiply the individual color components with the value in the Alpha channel.</para>
		/// A pre-multiplication of the above would produce a quadruple(0.5, 0.5, 0, 0).
		/// A value of 3 in the Attributes Type Field(field 23) would indicate that the color components
		/// of the pixel have already been scaled by the value in the Alpha channel.
		/// </summary>
		public TgaAttrType AttributesType {
			get { return attributesType; }
			set { attributesType = value; }
		}

		/// <summary>
		/// Scan Line Table - Field 25 (Variable):
		/// This information is provided, at the developers’ request, for two purposes:
		/// <para>1) To make random access of compressed images easy.</para>
		/// <para>2) To allow “giant picture” access in smaller “chunks”.</para>
		/// This table should contain a series of 4-byte offsets.Each offset you write should point to the
		/// start of the next scan line, in the order that the image was saved (i.e., top down or bottom up).
		/// The offset should be from the start of the file.Therefore, you will have a four byte value for
		/// each scan line in your image. This means that if your image is 768 pixels tall, you will have 768,
		/// 4-byte offset pointers (for a total of 3072 bytes). This size is not extreme, and thus this table
		/// can be built and maintained in memory, and then written out at the proper time.
		/// </summary>
		[CLSCompliant(false)]
		public uint[] ScanLineTable {
			get { return scanLineTable; }
			set { scanLineTable = value; }
		}

		/// <summary>
		/// Postage Stamp Image - Field 26 (Variable):
		/// The Postage Stamp area is a smaller representation of the original image. This is useful for
		/// “browsing” a collection of image files. If your application can deal with a postage stamp image,
		/// it is recommended that you create one using sub-sampling techniques to create the best
		/// representation possible. The postage stamp image must be stored in the same format as the normal
		/// image specified in the file, but without any compression. The first byte of the postage stamp
		/// image specifies the X size of the stamp in pixels, the second byte of the stamp image specifies the
		/// Y size of the stamp in pixels. Truevision does not recommend stamps larger than 64x64 pixels, and
		/// suggests that any stamps stored larger be clipped. Obviously, the storage of the postage stamp
		/// relies heavily on the storage of the image. The two images are stored using the same format under
		/// the assumption that if you can read the image, then you can read the postage stamp. If the original
		/// image is color mapped, DO NOT average the postage stamp, as you will create new colors not in your map.
		/// </summary>
		public TgaPostageStampImage PostageStampImage {
			get { return postageStampImage; }
			set { postageStampImage = value; }
		}

		/// <summary>
		/// Color Correction Table - Field 27 (2K Bytes):
		/// The Color Correction Table is a block of 256 x 4 SHORT values, where each set of
		/// four contiguous values are the desired A:R:G:B correction for that entry. This
		/// allows the user to store a correction table for image remapping or LUT driving.
		/// Since each color in the block is a SHORT, the maximum value for a color gun (i.e.,
		/// A, R, G, B) is 65535, and the minimum value is zero.Therefore, BLACK maps to
		/// 0, 0, 0, 0 and WHITE maps to 65535, 65535, 65535, 65535.
		/// </summary>
		[CLSCompliant(false)]
		public ushort[] ColorCorrectionTable {
			get { return colorCorrectionTable; }
			set { colorCorrectionTable = value; }
		}

		/// <summary>
		/// Other Data In Extension Area (if <see cref="ExtensionSize"/> > 495).
		/// </summary>
		public byte[] OtherDataInExtensionArea {
			get { return otherDataInExtensionArea; }
			set { otherDataInExtensionArea = value; }
		}
		#endregion

		/// <summary>
		/// Make full copy of <see cref="TgaExtArea"/>.
		/// </summary>
		/// <returns>Full independent copy of <see cref="TgaExtArea"/>.</returns>
		public TgaExtArea Clone() {
			TgaExtArea NewExtArea = new TgaExtArea();
			NewExtArea.extensionSize = extensionSize;
			NewExtArea.authorName = authorName.Clone();
			NewExtArea.authorComments = authorComments.Clone();
			NewExtArea.dateTimeStamp = dateTimeStamp.Clone();
			NewExtArea.jobNameOrID = jobNameOrID.Clone();
			NewExtArea.jobTime = jobTime.Clone();
			NewExtArea.softwareID = softwareID.Clone();
			NewExtArea.softVersion = softVersion.Clone();
			NewExtArea.keyColor = keyColor.Clone();
			NewExtArea.pixelAspectRatio = pixelAspectRatio.Clone();
			NewExtArea.gammaValue = gammaValue.Clone();
			NewExtArea.colorCorrectionOffset = colorCorrectionOffset;
			NewExtArea.postageStampOffset = postageStampOffset;
			NewExtArea.scanLineOffset = scanLineOffset;
			NewExtArea.attributesType = attributesType;

			if (scanLineTable != null)
				NewExtArea.scanLineTable = (uint[]) scanLineTable.Clone();
			if (postageStampImage != null)
				NewExtArea.postageStampImage = new TgaPostageStampImage(postageStampImage.ToBytes());
			if (colorCorrectionTable != null)
				NewExtArea.colorCorrectionTable = (ushort[]) colorCorrectionTable.Clone();

			if (otherDataInExtensionArea != null)
				NewExtArea.otherDataInExtensionArea = (byte[]) otherDataInExtensionArea.Clone();

			return NewExtArea;
		}

		/// <summary>
		/// Make full copy of <see cref="TgaExtArea"/>.
		/// </summary>
		/// <returns>Full independent copy of <see cref="TgaExtArea"/>.</returns>
		object ICloneable.Clone() {
			return Clone();
		}

		public override bool Equals(object obj) {
			return ((obj is TgaExtArea) ? Equals((TgaExtArea) obj) : false);
		}

		public bool Equals(TgaExtArea item) {
			return (extensionSize == item.extensionSize &&
				authorName == item.authorName &&
				authorComments == item.authorComments &&
				dateTimeStamp == item.dateTimeStamp &&
				jobNameOrID == item.jobNameOrID &&
				jobTime == item.jobTime &&
				softwareID == item.softwareID &&
				softVersion == item.softVersion &&
				keyColor == item.keyColor &&
				pixelAspectRatio == item.pixelAspectRatio &&
				gammaValue == item.gammaValue &&
				colorCorrectionOffset == item.colorCorrectionOffset &&
				postageStampOffset == item.postageStampOffset &&
				scanLineOffset == item.scanLineOffset &&
				attributesType == item.attributesType &&

				BitConverterExt.IsArraysEqual(scanLineTable, item.scanLineTable) &&
				postageStampImage == item.postageStampImage &&
				BitConverterExt.IsArraysEqual(colorCorrectionTable, item.colorCorrectionTable) &&

				BitConverterExt.IsArraysEqual(otherDataInExtensionArea, item.otherDataInExtensionArea));
		}

		public static bool operator ==(TgaExtArea item1, TgaExtArea item2) {
			if (ReferenceEquals(item1, null))
				return ReferenceEquals(item2, null);

			if (ReferenceEquals(item2, null))
				return ReferenceEquals(item1, null);

			return item1.Equals(item2);
		}

		public static bool operator !=(TgaExtArea item1, TgaExtArea item2) {
			return !(item1 == item2);
		}

		public override int GetHashCode() {
			unchecked {
				int hash = 27;
				hash = (13 * hash) + extensionSize.GetHashCode();
				hash = (13 * hash) + authorName.GetHashCode();
				hash = (13 * hash) + authorComments.GetHashCode();
				hash = (13 * hash) + dateTimeStamp.GetHashCode();
				hash = (13 * hash) + jobNameOrID.GetHashCode();
				hash = (13 * hash) + jobTime.GetHashCode();
				hash = (13 * hash) + softwareID.GetHashCode();
				hash = (13 * hash) + softVersion.GetHashCode();
				hash = (13 * hash) + keyColor.GetHashCode();
				hash = (13 * hash) + pixelAspectRatio.GetHashCode();
				hash = (13 * hash) + gammaValue.GetHashCode();
				hash = (13 * hash) + colorCorrectionOffset.GetHashCode();
				hash = (13 * hash) + postageStampOffset.GetHashCode();
				hash = (13 * hash) + scanLineOffset.GetHashCode();
				hash = (13 * hash) + attributesType.GetHashCode();

				if (scanLineTable != null)
					for (int i = 0; i < scanLineTable.Length; i++)
						hash = (13 * hash) + scanLineTable[i].GetHashCode();

				if (postageStampImage != null)
					hash = (13 * hash) + postageStampImage.GetHashCode();

				if (colorCorrectionTable != null)
					for (int i = 0; i < colorCorrectionTable.Length; i++)
						hash = (13 * hash) + colorCorrectionTable[i].GetHashCode();

				if (otherDataInExtensionArea != null)
					for (int i = 0; i < otherDataInExtensionArea.Length; i++)
						hash = (13 * hash) + otherDataInExtensionArea[i].GetHashCode();

				return hash;
			}
		}

		/// <summary>
		/// Convert <see cref="TgaExtArea"/> to byte array. Warning: <see cref="ScanLineTable"/>,
		/// <see cref="PostageStampImage"/>, <see cref="ColorCorrectionTable"/> not included,
		/// because thea are can be not in the Extension Area of TGA file!
		/// </summary>
		/// <returns>Byte array.</returns>
		public byte[] ToBytes() {
			#region Exceptions check
			if (authorName == null)
				authorName = new TgaString(41, true);

			if (authorComments == null)
				authorComments = new TgaComment();

			if (dateTimeStamp == null)
				dateTimeStamp = new TgaDateTime(DateTime.UtcNow);

			if (jobNameOrID == null)
				jobNameOrID = new TgaString(41, true);

			if (jobTime == null)
				jobTime = new TgaTime();

			if (softwareID == null)
				softwareID = new TgaString(41, true);

			if (softVersion == null)
				softVersion = new TgaSoftVersion();

			if (keyColor == null)
				keyColor = new TgaColorKey();

			if (pixelAspectRatio == null)
				pixelAspectRatio = new TgaFraction();

			if (gammaValue == null)
				gammaValue = new TgaFraction();
			#endregion

			return BitConverterExt.ToBytes(
				extensionSize,
				authorName.ToBytes(),
				authorComments.ToBytes(),
				dateTimeStamp.ToBytes(),
				jobNameOrID.ToBytes(),
				jobTime.ToBytes(),
				softwareID.ToBytes(),
				softVersion.ToBytes(),
				keyColor.ToBytes(),
				pixelAspectRatio.ToBytes(),
				gammaValue.ToBytes(),
				colorCorrectionOffset,
				postageStampOffset,
				scanLineOffset,
				(byte) attributesType,
				otherDataInExtensionArea);
		}
	} //Not full ToBytes()

	/// <summary>
	/// File Footer Area
	/// </summary>
	public class TgaFooter : ICloneable {
		uint extAreaOffset = 0;
		uint devDirOffset = 0;
		TgaString signature = TgaString.XFileSignatute;
		TgaString reservedChar = TgaString.DotSymbol;
		TgaString zeroStrTerminator = TgaString.ZeroTerminator;

		/// <summary>
		/// Make NewXFile format TGA Footer with <see cref="ExtensionAreaOffset"/> = 0 and
		/// <see cref="DeveloperDirectoryOffset"/> = 0.
		/// </summary>
		public TgaFooter() {
		}

		/// <summary>
		/// Make <see cref="TgaFooter"/> from values.
		/// </summary>
		/// <param name="ExtOff">Extension Area Offset, offset from the beginning of the file.</param>
		/// <param name="DevDirOff">Developer Directory Offset, offset from the beginning of the file.</param>
		/// <param name="Sign">New TGA format signature.</param>
		/// <param name="ReservChr">Reserved Character - ASCII character “.” (period).</param>
		/// <param name="Termin">Binary Zero Terminator, a binary zero which acts as a final terminator.</param>
		[CLSCompliant(false)]
		public TgaFooter(uint ExtOff, uint DevDirOff, TgaString Sign, TgaString ReservChr, TgaString Termin) {
			extAreaOffset = ExtOff;
			devDirOffset = DevDirOff;
			signature = Sign;
			reservedChar = ReservChr;
			zeroStrTerminator = Termin;
		}

		/// <summary>
		/// Make <see cref="TgaFooter"/> from bytes (if signature is right).
		/// </summary>
		/// <param name="Bytes">Bytes array (byte[26]).</param>
		public TgaFooter(byte[] Bytes) {
			if (Bytes == null)
				throw new ArgumentNullException(nameof(Bytes) + " = null!");
			if (Bytes.Length != Size)
				throw new ArgumentOutOfRangeException(nameof(Bytes.Length) + " must be equal " + Size + "!");

			extAreaOffset = BitConverter.ToUInt32(Bytes, 0);
			devDirOffset = BitConverter.ToUInt32(Bytes, 4);
			signature = new TgaString(BitConverterExt.GetElements(Bytes, 8, TgaString.XFileSignatuteConst.Length));
			reservedChar = new TgaString(new byte[] { Bytes[24] });
			zeroStrTerminator = new TgaString(new byte[] { Bytes[25] });
		}

		/// <summary>
		/// Byte 0-3 - Extension Area Offset - Field 28
		/// The first four bytes (bytes 0-3, the first LONG) of the TGA File Footer contain an
		/// offset from the beginning of the file to the start of the Extension Area. Simply
		/// SEEK to this location to position to the start of the Extension Area. If the
		/// Extension Area Offset is zero, no Extension Area exists in the file.
		/// </summary>
		[CLSCompliant(false)]
		public uint ExtensionAreaOffset {
			get { return extAreaOffset; }
			set { extAreaOffset = value; }
		}

		/// <summary>
		/// Byte 4-7 - Developer Directory Offset - Field 29
		/// The next four bytes(bytes 4-7, the second LONG) contain an offset from the
		/// beginning of the file to the start of the Developer Directory. If the Developer
		/// Directory Offset is zero, then the Developer Area does not exist.
		/// </summary>
		[CLSCompliant(false)]
		public uint DeveloperDirectoryOffset {
			get { return devDirOffset; }
			set { devDirOffset = value; }
		}

		/// <summary>
		/// Byte 8-23 - Signature - Field 30
		/// This string is exactly 16 bytes long and is formatted exactly as shown below
		/// capital letters), with a hyphen between “TRUEVISION” and “XFILE.” If the
		/// signature is detected, the file is assumed to be of the New TGA format and MAY,
		/// therefore, contain the Developer Area and/or the Extension Area fields.If the
		/// signature is not found, then the file is assumed to be in the Original TGA format.
		/// </summary>
		public TgaString Signature {
			get { return signature; }
			set { signature = value; }
		}

		/// <summary>
		/// Byte 24 - Reserved Character - Field 31
		/// Byte 24 is an ASCII character “.” (period). This character MUST BE a period or
		/// the file is not considered a proper TGA file.
		/// </summary>
		public TgaString ReservedCharacter {
			get { return reservedChar; }
			set { reservedChar = value; }
		}

		/// <summary>
		/// Byte 25 - Binary Zero String Terminator - Field 32
		/// Byte 25 is a binary zero which acts as a final terminator and allows the entire TGA
		/// File Footer to be read and utilized as a “C” string.
		/// </summary>
		public TgaString BinaryZeroStringTerminator {
			get { return zeroStrTerminator; }
			set { zeroStrTerminator = value; }
		}

		/// <summary>
		/// Make full copy of <see cref="TgaFooter"/>.
		/// </summary>
		/// <returns></returns>
		public TgaFooter Clone() {
			return new TgaFooter(extAreaOffset, devDirOffset, signature.Clone(),
				reservedChar.Clone(), zeroStrTerminator.Clone());
		}

		/// <summary>
		/// Make full copy of <see cref="TgaFooter"/>.
		/// </summary>
		/// <returns></returns>
		object ICloneable.Clone() {
			return Clone();
		}

		/// <summary>
		/// Gets TGA Footer Section size in bytes.
		/// </summary>
		public const int Size = 26;

		public override bool Equals(object obj) {
			return ((obj is TgaFooter) ? Equals((TgaFooter) obj) : false);
		}

		public bool Equals(TgaFooter item) {
			return (extAreaOffset == item.extAreaOffset &&
				devDirOffset == item.devDirOffset &&
				signature == item.signature &&
				reservedChar == item.reservedChar &&
				zeroStrTerminator == item.zeroStrTerminator);
		}

		public static bool operator ==(TgaFooter item1, TgaFooter item2) {
			if (ReferenceEquals(item1, null))
				return ReferenceEquals(item2, null);

			if (ReferenceEquals(item2, null))
				return ReferenceEquals(item1, null);

			return item1.Equals(item2);
		}

		public static bool operator !=(TgaFooter item1, TgaFooter item2) {
			return !(item1 == item2);
		}

		public override int GetHashCode() {
			unchecked {
				int hash = 17;
				hash = hash * 23 + extAreaOffset.GetHashCode();
				hash = hash * 23 + devDirOffset.GetHashCode();

				if (signature != null)
					hash = hash * 23 + signature.GetHashCode();

				if (reservedChar != null)
					hash = hash * 23 + reservedChar.GetHashCode();

				if (zeroStrTerminator != null)
					hash = hash * 23 + zeroStrTerminator.GetHashCode();

				return hash;
			}
		}

		public override string ToString() {
			return String.Format("{0}={1}, {2}={3}, FullSignature={4}",
				nameof(ExtensionAreaOffset), extAreaOffset, nameof(DeveloperDirectoryOffset), devDirOffset,
				(signature + reservedChar + zeroStrTerminator).ToString());
		}

		/// <summary>
		/// Convert <see cref="TgaFooter"/> to byte array.
		/// </summary>
		/// <returns>Byte array with size equal <see cref="Size"/>.</returns>
		public byte[] ToBytes() {
			return BitConverterExt.ToBytes(extAreaOffset, devDirOffset,
				signature.ToBytes(), reservedChar.ToBytes(), zeroStrTerminator.ToBytes());
		}

		/// <summary>
		/// Is footer is real footer of TGA File Format Version 2.0?
		/// Checking by <see cref="TgaString.XFileSignatute"/>.
		/// </summary>
		public bool IsFooterCorrect {
			get { return signature == TgaString.XFileSignatute; }
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Simplify ByteConversion operations, like concatination of byte arrays, comparing and other.
	/// </summary>
	public static class BitConverterExt {
		/// <summary>
		/// Combine byte, byte[], (u)short, (u)int, (u)long values to byte[] array.
		/// </summary>
		/// <param name="obj">Array of byte, byte[], (u)short, (u)int, (u)long values.</param>
		/// <returns>Array of bytes, null when some object is null.</returns>
		public static byte[] ToBytes(params object[] obj) {
			if (obj == null)
				return null;

			List<byte> BytesList = new List<byte>();

			for (int i = 0; i < obj.Length; i++) {
				if (obj[i] == null)
					continue;
				else if (obj[i] is byte)
					BytesList.Add((byte) obj[i]);
				else if (obj[i] is byte[])
					BytesList.AddRange((byte[]) obj[i]);
				else if (obj[i] is short)
					BytesList.AddRange(BitConverter.GetBytes((short) obj[i]));
				else if (obj[i] is ushort)
					BytesList.AddRange(BitConverter.GetBytes((ushort) obj[i]));
				else if (obj[i] is int)
					BytesList.AddRange(BitConverter.GetBytes((int) obj[i]));
				else if (obj[i] is uint)
					BytesList.AddRange(BitConverter.GetBytes((uint) obj[i]));
				else if (obj[i] is long)
					BytesList.AddRange(BitConverter.GetBytes((long) obj[i]));
				else if (obj[i] is ulong)
					BytesList.AddRange(BitConverter.GetBytes((ulong) obj[i]));
			}
			return BytesList.ToArray();
		}

		/// <summary>
		/// Copies a range of elements from an Array starting at the specified source index.
		/// The length and the index are specified as 32-bit integers.
		/// </summary>
		/// <param name="SrcArray">The <see cref="Array"/> that contains the data to copy.</param>
		/// <param name="Offset">A 32-bit integer that represents the index in the
		/// SrcArray at which copying begins.</param>
		/// <param name="Count">A 32-bit integer that represents the number of elements to copy.</param>
		/// <returns></returns>
		public static T[] GetElements<T>(T[] SrcArray, int Offset, int Count) {
			if (SrcArray == null)
				throw new ArgumentNullException(nameof(SrcArray) + " is null!");

			if (Offset >= SrcArray.Length || Offset < 0)
				throw new ArgumentOutOfRangeException(nameof(Offset) + " has wrong value!");

			if (Count <= 0 || Offset + Count > SrcArray.Length)
				throw new ArgumentOutOfRangeException(nameof(Count) + " has wrong value!");

			T[] Buff = new T[Count];
			Array.Copy(SrcArray, Offset, Buff, 0, Buff.Length);
			return Buff;
		}

		/// <summary>
		/// Compare N-dimensional Arrays.
		/// </summary>
		/// <typeparam name="T">Arrays Type.</typeparam>
		/// <param name="item1">First Array.</param>
		/// <param name="item2">Second Array.</param>
		/// <returns>True, if Arrays are equal.</returns>
		public static bool IsArraysEqual<T>(T[] item1, T[] item2) {
			if (ReferenceEquals(item1, item2))
				return true;

			if (item1 == null || item2 == null)
				return false;

			if (item1.Length != item2.Length)
				return false;

			EqualityComparer<T> comparer = EqualityComparer<T>.Default;
			for (int i = 0; i < item1.Length; i++)
				if (!comparer.Equals(item1[i], item2[i]))
					return false;
			return true;
		}

		/// <summary>
		/// Compare Lists.
		/// </summary>
		/// <typeparam name="T">List Type.</typeparam>
		/// <param name="item1">First List.</param>
		/// <param name="item2">Second List.</param>
		/// <returns>True, if Lists are equal.</returns>
		public static bool IsListsEqual<T>(List<T> item1, List<T> item2) {
			if (ReferenceEquals(item1, item2))
				return true;

			if (item1 == null || item2 == null)
				return false;

			if (item1.Count != item2.Count)
				return false;

			for (int i = 0; i < item1.Count; i++)
				if (!item1[i].Equals(item2[i]))
					return false;
			return true;
		}

		/// <summary>
		/// Compare elements in one Array with different offsets.
		/// </summary>
		/// <typeparam name="T">Array type.</typeparam>
		/// <param name="Arr">Some Array.</param>
		/// <param name="Offset1">First offset.</param>
		/// <param name="Offset2">Second offset.</param>
		/// <param name="Count">Elements count which must be compared.</param>
		/// <returns></returns>
		public static bool IsElementsEqual<T>(T[] Arr, int Offset1, int Offset2, int Count) {
			if (Arr == null)
				throw new ArgumentNullException(nameof(Arr) + " is null!");

			if (Offset1 >= Arr.Length || Offset1 < 0)
				throw new ArgumentOutOfRangeException(nameof(Offset1) + " has wrong value!");

			if (Offset2 >= Arr.Length || Offset2 < 0)
				throw new ArgumentOutOfRangeException(nameof(Offset2) + " has wrong value!");

			if (Count <= 0 || Offset1 + Count > Arr.Length || Offset2 + Count > Arr.Length)
				throw new ArgumentOutOfRangeException(nameof(Count) + " has wrong value!");

			if (Offset1 == Offset2)
				return true;

			for (int i = 0; i < Count; i++)
				if (!Arr[Offset1 + i].Equals(Arr[Offset2 + i]))
					return false;

			return true;
		}
	}
	#endregion

	public class TargaImage : ICloneable {
		public TgaHeader Header = new TgaHeader();
		public TgaImgOrColMap ImageOrColorMapArea = new TgaImgOrColMap();
		public TgaDevArea DevArea = null;
		public TgaExtArea ExtArea = null;
		public TgaFooter Footer = null;

		#region TGA Creation, Loading, Saving (all are public, have reference to private metods).
		/// <summary>
		/// Create new empty <see cref="TargaImage"/> istance.
		/// </summary>
		public TargaImage() {
		}

		/// <summary>
		/// Create <see cref="TargaImage"/> instance with some params. If it must have ColorMap,
		/// check all ColorMap fields and settings after.
		/// </summary>
		/// <param name="Width">Image Width.</param>
		/// <param name="Height">Image Height.</param>
		/// <param name="PixDepth">Image Pixel Depth (bits / pixel), set ColorMap bpp after, if needed!</param>
		/// <param name="ImgType">Image Type (is RLE compressed, ColorMapped or GrayScaled).</param>
		/// <param name="AttrBits">Set numder of Attrbute bits (Alpha channel bits), default: 0, 1, 8.</param>
		/// <param name="NewFormat">Use new 2.0 TGA XFile format?</param>
		[CLSCompliant(false)]
		public TargaImage(ushort Width, ushort Height, TgaPixelDepth PixDepth = TgaPixelDepth.Bpp24,
			TgaImageType ImgType = TgaImageType.Uncompressed_TrueColor, byte AttrBits = 0, bool NewFormat = true) {
			if (Width <= 0 || Height <= 0 || PixDepth == TgaPixelDepth.Other) {
				Width = Height = 0;
				PixDepth = TgaPixelDepth.Other;
				ImgType = TgaImageType.NoImageData;
				AttrBits = 0;
			} else {
				int BytesPerPixel = (int) Math.Ceiling((double) PixDepth / 8.0);
				ImageOrColorMapArea.ImageData = new byte[Width * Height * BytesPerPixel];

				if (ImgType == TgaImageType.Uncompressed_ColorMapped || ImgType == TgaImageType.RLE_ColorMapped) {
					Header.ColorMapType = TgaColorMapType.ColorMap;
					Header.ColorMapSpec.FirstEntryIndex = 0;
					Header.ColorMapSpec.ColorMapEntrySize = (TgaColorMapEntrySize) Math.Ceiling((double) PixDepth / 8);
				}
			}

			Header.ImageType = ImgType;
			Header.ImageSpec.ImageWidth = Width;
			Header.ImageSpec.ImageHeight = Height;
			Header.ImageSpec.PixelDepth = PixDepth;
			Header.ImageSpec.ImageDescriptor.AlphaChannelBits = AttrBits;

			if (NewFormat) {
				Footer = new TgaFooter();
				ExtArea = new TgaExtArea();
				ExtArea.DateTimeStamp = new TgaDateTime(DateTime.UtcNow);
				ExtArea.AttributesType = (AttrBits > 0 ? TgaAttrType.UsefulAlpha : TgaAttrType.NoAlpha);
			}
		}

		/// <summary>
		/// Make <see cref="TargaImage"/> from some <see cref="TargaImage"/> instance.
		/// Equal to <see cref="TargaImage.Clone()"/> function.
		/// </summary>
		/// <param name="tga">Original <see cref="TargaImage"/> instance.</param>
		public TargaImage(TargaImage tga) {
			Header = tga.Header.Clone();
			ImageOrColorMapArea = tga.ImageOrColorMapArea.Clone();
			DevArea = tga.DevArea.Clone();
			ExtArea = tga.ExtArea.Clone();
			Footer = tga.Footer.Clone();
		}

		/// <summary>
		/// Load <see cref="TargaImage"/> from file.
		/// </summary>
		/// <param name="filename">Full path to TGA file.</param>
		/// <returns>Loaded <see cref="TargaImage"/> file.</returns>
		public TargaImage(string filename) {
			LoadFunc(filename);
		}

		/// <summary>
		/// Make <see cref="TargaImage"/> from bytes array.
		/// </summary>
		/// <param name="bytes">Bytes array (same like TGA File).</param>
		public TargaImage(byte[] bytes) {
			LoadFunc(bytes);
		}

		/// <summary>
		/// Make <see cref="TargaImage"/> from <see cref="Stream"/>.
		/// For file opening better use <see cref="FromFile(string)"/>.
		/// </summary>
		/// <param name="stream">Some stream. You can use a lot of Stream types, but Stream must support:
		/// <see cref="Stream.CanSeek"/> and <see cref="Stream.CanRead"/>.</param>
		public TargaImage(Stream stream) {
			LoadFunc(stream);
		}

		/// <summary>
		/// Make <see cref="TargaImage"/> from <see cref="Bitmap"/>.
		/// </summary>
		/// <param name="bmp">Input Bitmap, supported a lot of bitmaps types: 8/15/16/24/32 Bpp's.</param>
		/// <param name="UseRLE">Use RLE Compression?</param>
		/// <param name="NewFormat">Use new 2.0 TGA XFile format?</param>
		/// <param name="ColorMap2BytesEntry">Is Color Map Entry size equal 15 or 16 Bpp, else - 24 or 32.</param>
		public TargaImage(Bitmap bmp, bool UseRLE = false, bool NewFormat = true, bool ColorMap2BytesEntry = false) {
			LoadFunc(bmp, UseRLE, NewFormat, ColorMap2BytesEntry);
		}

		/// <summary>
		/// Load <see cref="TargaImage"/> from file.
		/// </summary>
		/// <param name="filename">Full path to TGA file.</param>
		/// <returns>Loaded <see cref="TargaImage"/> file.</returns>
		public static TargaImage FromFile(string filename) {
			return new TargaImage(filename);
		}

		/// <summary>
		/// Make <see cref="TargaImage"/> from bytes array.
		/// </summary>
		/// <param name="bytes">Bytes array (same like TGA File).</param>
		public static TargaImage FromBytes(byte[] bytes) {
			return new TargaImage(bytes);
		}

		/// <summary>
		/// Make <see cref="TargaImage"/> from <see cref="Stream"/>.
		/// For file opening better use <see cref="FromFile(string)"/>.
		/// </summary>
		/// <param name="stream">Some stream. You can use a lot of Stream types, but Stream must support:
		/// <see cref="Stream.CanSeek"/> and <see cref="Stream.CanRead"/>.</param>
		public static TargaImage FromStream(Stream stream) {
			return new TargaImage(stream);
		}

		/// <summary>
		/// Make <see cref="TargaImage"/> from <see cref="Bitmap"/>.
		/// </summary>
		/// <param name="bmp">Input Bitmap, supported a lot of bitmaps types: 8/15/16/24/32 Bpp's.</param>
		/// <param name="UseRLE">Use RLE Compression?</param>
		/// <param name="NewFormat">Use new 2.0 TGA XFile format?</param>
		/// <param name="ColorMap2BytesEntry">Is Color Map Entry size equal 15 or 16 Bpp, else - 24 or 32.</param>
		public static TargaImage FromBitmap(Bitmap bmp, bool UseRLE = false,
			bool NewFormat = true, bool ColorMap2BytesEntry = false) {
			return new TargaImage(bmp, UseRLE, NewFormat, ColorMap2BytesEntry);
		}

		/// <summary>
		/// Save <see cref="TargaImage"/> to file.
		/// </summary>
		/// <param name="filename">Full path to file.</param>
		/// <returns>Return "true", if all done or "false", if failed.</returns>
		public bool Save(string filename) {
			try {
				bool Result = false;
				using (FileStream Fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None)) {
					using (MemoryStream Ms = new MemoryStream()) {
						Result = SaveFunc(Ms);
						Ms.WriteTo(Fs);
						Fs.Flush();
					}
				}
				return Result;
			} catch {
				return false;
			}
		}

		/// <summary>
		/// Save <see cref="TargaImage"/> to <see cref="Stream"/>.
		/// </summary>
		/// <param name="stream">Some stream, it must support: <see cref="Stream.CanWrite"/>.</param>
		/// <returns>Return "true", if all done or "false", if failed.</returns>
		public bool Save(Stream stream) {
			return SaveFunc(stream);
		}
		#endregion

		/// <summary>
		/// Gets or Sets Image Width Header.ImageSpec.ImageWidth
		/// </summary>
		[CLSCompliant(false)]
		public ushort Width {
			get { return Header.ImageSpec.ImageWidth; }
			set { Header.ImageSpec.ImageWidth = value; }
		}

		/// <summary>
		/// Gets or Sets Image Height Header.ImageSpec.ImageHeight
		/// </summary>
		[CLSCompliant(false)]
		public ushort Height {
			get { return Header.ImageSpec.ImageHeight; }
			set { Header.ImageSpec.ImageHeight = value; }
		}

		/// <summary>
		/// Gets or Sets <see cref="TargaImage"/> image Size.
		/// </summary>
		public Size Size {
			get { return new Size(Header.ImageSpec.ImageWidth, Header.ImageSpec.ImageHeight); }
			set {
				Header.ImageSpec.ImageWidth = (ushort) value.Width;
				Header.ImageSpec.ImageHeight = (ushort) value.Height;
			}
		}

		/// <summary>
		/// Make full independed copy of <see cref="TargaImage"/>.
		/// </summary>
		/// <returns>Full independed copy of <see cref="TargaImage"/>.</returns>
		public TargaImage Clone() {
			return new TargaImage(this);
		}

		object ICloneable.Clone() {
			return Clone();
		}

		/// <summary>
		/// Flip <see cref="TargaImage"/> directions, for more info see <see cref="TgaImgOrigin"/>.
		/// </summary>
		/// <param name="Horizontal">Flip horizontal.</param>
		/// <param name="Vertical">Flip vertical.</param>
		public void Flip(bool Horizontal = false, bool Vertical = false) {
			int NewOrigin = (int) Header.ImageSpec.ImageDescriptor.ImageOrigin;
			NewOrigin = NewOrigin ^ ((Vertical ? 0x20 : 0) | (Horizontal ? 0x10 : 0));
			Header.ImageSpec.ImageDescriptor.ImageOrigin = (TgaImgOrigin) NewOrigin;
		}

		/// <summary>
		/// Get information from TGA image.
		/// </summary>
		/// <returns>MultiLine string with info fields (one per line).</returns>
		public string GetInfo() {
			StringBuilder SB = new StringBuilder();

			SB.AppendLine("Header:");
			SB.AppendLine("\tID Length = " + Header.IDLength);
			SB.AppendLine("\tImage Type = " + Header.ImageType);
			SB.AppendLine("\tHeader -> ImageSpec:");
			SB.AppendLine("\t\tImage Width = " + Header.ImageSpec.ImageWidth);
			SB.AppendLine("\t\tImage Height = " + Header.ImageSpec.ImageHeight);
			SB.AppendLine("\t\tPixel Depth = " + Header.ImageSpec.PixelDepth);
			SB.AppendLine("\t\tImage Descriptor (AsByte) = " + Header.ImageSpec.ImageDescriptor.ToByte());
			SB.AppendLine("\t\tImage Descriptor -> AttributeBits = " + Header.ImageSpec.ImageDescriptor.AlphaChannelBits);
			SB.AppendLine("\t\tImage Descriptor -> ImageOrigin = " + Header.ImageSpec.ImageDescriptor.ImageOrigin);
			SB.AppendLine("\t\tX_Origin = " + Header.ImageSpec.X_Origin);
			SB.AppendLine("\t\tY_Origin = " + Header.ImageSpec.Y_Origin);
			SB.AppendLine("\tColorMap Type = " + Header.ColorMapType);
			SB.AppendLine("\tHeader -> ColorMapSpec:");
			SB.AppendLine("\t\tColorMap Entry Size = " + Header.ColorMapSpec.ColorMapEntrySize);
			SB.AppendLine("\t\tColorMap Length = " + Header.ColorMapSpec.ColorMapLength);
			SB.AppendLine("\t\tFirstEntry Index = " + Header.ColorMapSpec.FirstEntryIndex);

			SB.AppendLine("\nImage / Color Map Area:");
			if (Header.IDLength > 0 && ImageOrColorMapArea.ImageID != null)
				SB.AppendLine("\tImage ID = \"" + ImageOrColorMapArea.ImageID.GetString() + "\"");
			else
				SB.AppendLine("\tImage ID = null");

			if (ImageOrColorMapArea.ImageData != null)
				SB.AppendLine("\tImage Data Length = " + ImageOrColorMapArea.ImageData.Length);
			else
				SB.AppendLine("\tImage Data = null");

			if (ImageOrColorMapArea.ColorMapData != null)
				SB.AppendLine("\tColorMap Data Length = " + ImageOrColorMapArea.ColorMapData.Length);
			else
				SB.AppendLine("\tColorMap Data = null");

			SB.AppendLine("\nDevelopers Area:");
			if (DevArea != null)
				SB.AppendLine("\tCount = " + DevArea.Count);
			else
				SB.AppendLine("\tDevArea = null");

			SB.AppendLine("\nExtension Area:");
			if (ExtArea != null) {
				SB.AppendLine("\tExtension Size = " + ExtArea.ExtensionSize);
				SB.AppendLine("\tAuthor Name = \"" + ExtArea.AuthorName.GetString() + "\"");
				SB.AppendLine("\tAuthor Comments = \"" + ExtArea.AuthorComments.GetString() + "\"");
				SB.AppendLine("\tDate / Time Stamp = " + ExtArea.DateTimeStamp);
				SB.AppendLine("\tJob Name / ID = \"" + ExtArea.JobNameOrID.GetString() + "\"");
				SB.AppendLine("\tJob Time = " + ExtArea.JobTime);
				SB.AppendLine("\tSoftware ID = \"" + ExtArea.SoftwareID.GetString() + "\"");
				SB.AppendLine("\tSoftware Version = \"" + ExtArea.SoftVersion + "\"");
				SB.AppendLine("\tKey Color = " + ExtArea.KeyColor);
				SB.AppendLine("\tPixel Aspect Ratio = " + ExtArea.PixelAspectRatio);
				SB.AppendLine("\tGamma Value = " + ExtArea.GammaValue);
				SB.AppendLine("\tColor Correction Table Offset = " + ExtArea.ColorCorrectionTableOffset);
				SB.AppendLine("\tPostage Stamp Offset = " + ExtArea.PostageStampOffset);
				SB.AppendLine("\tScan Line Offset = " + ExtArea.ScanLineOffset);
				SB.AppendLine("\tAttributes Type = " + ExtArea.AttributesType);

				if (ExtArea.ScanLineTable != null)
					SB.AppendLine("\tScan Line Table = " + ExtArea.ScanLineTable.Length);
				else
					SB.AppendLine("\tScan Line Table = null");

				if (ExtArea.PostageStampImage != null)
					SB.AppendLine("\tPostage Stamp Image: " + ExtArea.PostageStampImage.ToString());
				else
					SB.AppendLine("\tPostage Stamp Image = null");

				SB.AppendLine("\tColor Correction Table = " + (ExtArea.ColorCorrectionTable != null));
			} else
				SB.AppendLine("\tExtArea = null");

			SB.AppendLine("\nFooter:");
			if (Footer != null) {
				SB.AppendLine("\tExtension Area Offset = " + Footer.ExtensionAreaOffset);
				SB.AppendLine("\tDeveloper Directory Offset = " + Footer.DeveloperDirectoryOffset);
				SB.AppendLine("\tSignature (Full) = \"" + Footer.Signature.ToString() +
					Footer.ReservedCharacter.ToString() + Footer.BinaryZeroStringTerminator.ToString() + "\"");
			} else
				SB.AppendLine("\tFooter = null");

			return SB.ToString();
		}

		/// <summary>
		/// Check and update all fields with data length and offsets.
		/// </summary>
		/// <returns>Return "true", if all OK or "false", if checking failed.</returns>
		public bool CheckAndUpdateOffsets(out string ErrorStr) {
			ErrorStr = String.Empty;

			if (Header == null) {
				ErrorStr = "Header = null";
				return false;
			}

			if (ImageOrColorMapArea == null) {
				ErrorStr = "ImageOrColorMapArea = null";
				return false;
			}

			uint Offset = TgaHeader.Size; // Virtual Offset

			#region Header
			if (ImageOrColorMapArea.ImageID != null) {
				int StrMaxLen = 255;
				if (ImageOrColorMapArea.ImageID.UseEndingChar)
					StrMaxLen--;

				Header.IDLength = (byte) Math.Min(ImageOrColorMapArea.ImageID.OriginalString.Length, StrMaxLen);
				ImageOrColorMapArea.ImageID.Length = Header.IDLength;
				Offset += Header.IDLength;
			} else
				Header.IDLength = 0;
			#endregion

			#region ColorMap
			if (Header.ColorMapType != TgaColorMapType.NoColorMap) {
				if (Header.ColorMapSpec == null) {
					ErrorStr = "Header.ColorMapSpec = null";
					return false;
				}

				if (Header.ColorMapSpec.ColorMapLength == 0) {
					ErrorStr = "Header.ColorMapSpec.ColorMapLength = 0";
					return false;
				}

				if (ImageOrColorMapArea.ColorMapData == null) {
					ErrorStr = "ImageOrColorMapArea.ColorMapData = null";
					return false;
				}

				int CmBytesPerPixel = (int) Math.Ceiling((double) Header.ColorMapSpec.ColorMapEntrySize / 8.0);
				int LenBytes = Header.ColorMapSpec.ColorMapLength * CmBytesPerPixel;

				if (LenBytes != ImageOrColorMapArea.ColorMapData.Length) {
					ErrorStr = "ImageOrColorMapArea.ColorMapData.Length has wrong size!";
					return false;
				}

				Offset += (uint) ImageOrColorMapArea.ColorMapData.Length;
			}
			#endregion

			#region Image Data
			int BytesPerPixel = 0;
			if (Header.ImageType != TgaImageType.NoImageData) {
				if (Header.ImageSpec == null) {
					ErrorStr = "Header.ImageSpec = null";
					return false;
				}

				if (Header.ImageSpec.ImageWidth == 0 || Header.ImageSpec.ImageHeight == 0) {
					ErrorStr = "Header.ImageSpec.ImageWidth = 0 or Header.ImageSpec.ImageHeight = 0";
					return false;
				}

				if (ImageOrColorMapArea.ImageData == null) {
					ErrorStr = "ImageOrColorMapArea.ImageData = null";
					return false;
				}

				BytesPerPixel = (int) Math.Ceiling((double) Header.ImageSpec.PixelDepth / 8.0);
				if (Width * Height * BytesPerPixel != ImageOrColorMapArea.ImageData.Length) {
					ErrorStr = "ImageOrColorMapArea.ImageData.Length has wrong size!";
					return false;
				}

				if (Header.ImageType >= TgaImageType.RLE_ColorMapped &&
					Header.ImageType <= TgaImageType.RLE_BlackWhite) {
					byte[] RLE = RLE_Encode(ImageOrColorMapArea.ImageData, Width, Height);
					if (RLE == null) {
						ErrorStr = "RLE Compressing error! Check Image Data size.";
						return false;
					}

					Offset += (uint) RLE.Length;
					RLE = null;
				} else
					Offset += (uint) ImageOrColorMapArea.ImageData.Length;
			}
			#endregion

			#region Footer, DevArea, ExtArea
			if (Footer != null) {
				#region DevArea
				if (DevArea != null) {
					int DevAreaCount = DevArea.Count;
					for (int i = 0; i < DevAreaCount; i++)
						if (DevArea[i] == null || DevArea[i].FieldSize <= 0) //Del Empty Entries
						{
							DevArea.Entries.RemoveAt(i);
							DevAreaCount--;
							i--;
						}

					if (DevArea.Count <= 0)
						Footer.DeveloperDirectoryOffset = 0;

					if (DevArea.Count > 2) {
						DevArea.Entries.Sort((a, b) => { return a.Tag.CompareTo(b.Tag); });
						for (int i = 0; i < DevArea.Count - 1; i++)
							if (DevArea[i].Tag == DevArea[i + 1].Tag) {
								ErrorStr = "DevArea Enties has same Tags!";
								return false;
							}
					}

					for (int i = 0; i < DevArea.Count; i++) {
						DevArea[i].Offset = Offset;
						Offset += (uint) DevArea[i].FieldSize;
					}

					Footer.DeveloperDirectoryOffset = Offset;
					Offset += (uint) (DevArea.Count * 10 + 2);
				} else
					Footer.DeveloperDirectoryOffset = 0;
				#endregion

				#region ExtArea
				if (ExtArea != null) {
					ExtArea.ExtensionSize = TgaExtArea.MinSize;
					if (ExtArea.OtherDataInExtensionArea != null)
						ExtArea.ExtensionSize += (ushort) ExtArea.OtherDataInExtensionArea.Length;

					ExtArea.DateTimeStamp = new TgaDateTime(DateTime.UtcNow);

					Footer.ExtensionAreaOffset = Offset;
					Offset += ExtArea.ExtensionSize;

					#region ScanLineTable
					if (ExtArea.ScanLineTable == null)
						ExtArea.ScanLineOffset = 0;
					else {
						if (ExtArea.ScanLineTable.Length != Height) {
							ErrorStr = "ExtArea.ScanLineTable.Length != Height";
							return false;
						}

						ExtArea.ScanLineOffset = Offset;
						Offset += (uint) (ExtArea.ScanLineTable.Length * 4);
					}
					#endregion

					#region PostageStampImage
					if (ExtArea.PostageStampImage == null)
						ExtArea.PostageStampOffset = 0;
					else {
						if (ExtArea.PostageStampImage.Width == 0 || ExtArea.PostageStampImage.Height == 0) {
							ErrorStr = "ExtArea.PostageStampImage Width or Height is equal 0!";
							return false;
						}

						if (ExtArea.PostageStampImage.Data == null) {
							ErrorStr = "ExtArea.PostageStampImage.Data == null";
							return false;
						}

						int PImgSB = ExtArea.PostageStampImage.Width * ExtArea.PostageStampImage.Height * BytesPerPixel;
						if (Header.ImageType != TgaImageType.NoImageData &&
							ExtArea.PostageStampImage.Data.Length != PImgSB) {
							ErrorStr = "ExtArea.PostageStampImage.Data.Length is wrong!";
							return false;
						}


						ExtArea.PostageStampOffset = Offset;
						Offset += (uint) (ExtArea.PostageStampImage.Data.Length);
					}
					#endregion

					#region ColorCorrectionTable
					if (ExtArea.ColorCorrectionTable == null)
						ExtArea.ColorCorrectionTableOffset = 0;
					else {
						if (ExtArea.ColorCorrectionTable.Length != 1024) {
							ErrorStr = "ExtArea.ColorCorrectionTable.Length != 256 * 4";
							return false;
						}

						ExtArea.ColorCorrectionTableOffset = Offset;
						Offset += (uint) (ExtArea.ColorCorrectionTable.Length * 2);
					}
					#endregion
				} else
					Footer.ExtensionAreaOffset = 0;
				#endregion

				#region Footer
				if (Footer.ToBytes().Length != TgaFooter.Size) {
					ErrorStr = "Footer.Length is wrong!";
					return false;
				}

				Offset += TgaFooter.Size;
				#endregion
			}
			#endregion

			return true;
		}

		#region Convert
		/// <summary>
		/// Convert <see cref="TargaImage"/> to <see cref="Bitmap"/>.
		/// </summary>
		/// <param name="ForceUseAlpha">Force use alpha channel.</param>
		/// <returns>Bitmap or null, on error.</returns>
		public Bitmap ToBitmap(bool ForceUseAlpha = false) {
			return ToBitmapFunc(ForceUseAlpha, false);
		}

		/// <summary>
		/// Convert <see cref="TargaImage"/> to bytes array.
		/// </summary>
		/// <returns>Bytes array, (equal to saved file, but in memory) or null (on error).</returns>
		public byte[] ToBytes() {
			try {
				byte[] Bytes;
				using (MemoryStream ms = new MemoryStream()) {
					Save(ms);
					Bytes = ms.ToArray();
					ms.Flush();
				}
				return Bytes;
			} catch {
				return null;
			}
		}

		/// <summary>
		/// Convert TGA Image to new XFile format (v2.0).
		/// </summary>
		public void ToNewFormat() {
			if (Footer == null)
				Footer = new TgaFooter();

			if (ExtArea == null) {
				ExtArea = new TgaExtArea();

				ExtArea.DateTimeStamp = new TgaDateTime(DateTime.UtcNow);

				if (Header.ImageSpec.ImageDescriptor.AlphaChannelBits > 0)
					ExtArea.AttributesType = TgaAttrType.UsefulAlpha;
				else
					ExtArea.AttributesType = TgaAttrType.NoAlpha;
			}
		}
		#endregion

		#region Private functions
		bool LoadFunc(string filename) {
			if (!File.Exists(filename))
				throw new FileNotFoundException("File: \"" + filename + "\" not found!");

			try {
				using (FileStream FS = new FileStream(filename, FileMode.Open))
					return LoadFunc(FS);
			} catch {
				return false;
			}
		}

		bool LoadFunc(byte[] bytes) {
			if (bytes == null)
				throw new ArgumentNullException();

			try {
				using (MemoryStream FS = new MemoryStream(bytes, false))
					return LoadFunc(FS);
			} catch {
				return false;
			}
		}

		bool LoadFunc(Stream stream) {
			if (stream == null)
				throw new ArgumentNullException();
			if (!(stream.CanRead && stream.CanSeek))
				throw new FileLoadException("Stream reading or seeking is not avaiable!");

			try {
				stream.Seek(0, SeekOrigin.Begin);
				BinaryReader Br = new BinaryReader(stream);

				Header = new TgaHeader(Br.ReadBytes(TgaHeader.Size));

				if (Header.IDLength > 0)
					ImageOrColorMapArea.ImageID = new TgaString(Br.ReadBytes(Header.IDLength));

				if (Header.ColorMapSpec.ColorMapLength > 0) {
					int CmBytesPerPixel = (int) Math.Ceiling((double) Header.ColorMapSpec.ColorMapEntrySize / 8.0);
					int LenBytes = Header.ColorMapSpec.ColorMapLength * CmBytesPerPixel;
					ImageOrColorMapArea.ColorMapData = Br.ReadBytes(LenBytes);
				}

				#region Read Image Data
				int BytesPerPixel = (int) Math.Ceiling((double) Header.ImageSpec.PixelDepth / 8.0);
				if (Header.ImageType != TgaImageType.NoImageData) {
					int ImageDataSize = Width * Height * BytesPerPixel;
					switch (Header.ImageType) {
						case TgaImageType.RLE_ColorMapped:
						case TgaImageType.RLE_TrueColor:
						case TgaImageType.RLE_BlackWhite:

							int DataOffset = 0;
							byte PacketInfo;
							int PacketCount;
							byte[] RLE_Bytes, RLE_Part;
							ImageOrColorMapArea.ImageData = new byte[ImageDataSize];

							do {
								PacketInfo = Br.ReadByte(); //1 type bit and 7 count bits. Len = Count + 1.
								PacketCount = (PacketInfo & 127) + 1;

								if (PacketInfo >= 128) // bit7 = 1, RLE
								{
									RLE_Bytes = new byte[PacketCount * BytesPerPixel];
									RLE_Part = Br.ReadBytes(BytesPerPixel);
									for (int i = 0; i < RLE_Bytes.Length; i++)
										RLE_Bytes[i] = RLE_Part[i % BytesPerPixel];
								} else // RAW format
									RLE_Bytes = Br.ReadBytes(PacketCount * BytesPerPixel);

								Buffer.BlockCopy(RLE_Bytes, 0, ImageOrColorMapArea.ImageData, DataOffset, RLE_Bytes.Length);
								DataOffset += RLE_Bytes.Length;
							}
							while (DataOffset < ImageDataSize);
							RLE_Bytes = null;
							break;

						case TgaImageType.Uncompressed_ColorMapped:
						case TgaImageType.Uncompressed_TrueColor:
						case TgaImageType.Uncompressed_BlackWhite:
							ImageOrColorMapArea.ImageData = Br.ReadBytes(ImageDataSize);
							break;
					}
				}
				#endregion

				#region Try parse Footer
				stream.Seek(-TgaFooter.Size, SeekOrigin.End);
				uint FooterOffset = (uint) stream.Position;
				TgaFooter MbFooter = new TgaFooter(Br.ReadBytes(TgaFooter.Size));
				if (MbFooter.IsFooterCorrect) {
					Footer = MbFooter;
					uint DevDirOffset = Footer.DeveloperDirectoryOffset;
					uint ExtAreaOffset = Footer.ExtensionAreaOffset;

					#region If Dev Area exist, read it.
					if (DevDirOffset != 0) {
						stream.Seek(DevDirOffset, SeekOrigin.Begin);
						DevArea = new TgaDevArea();
						uint NumberOfTags = Br.ReadUInt16();

						ushort[] Tags = new ushort[NumberOfTags];
						uint[] TagOffsets = new uint[NumberOfTags];
						uint[] TagSizes = new uint[NumberOfTags];

						for (int i = 0; i < NumberOfTags; i++) {
							Tags[i] = Br.ReadUInt16();
							TagOffsets[i] = Br.ReadUInt32();
							TagSizes[i] = Br.ReadUInt32();
						}

						for (int i = 0; i < NumberOfTags; i++) {
							stream.Seek(TagOffsets[i], SeekOrigin.Begin);
							var Ent = new TgaDevEntry(Tags[i], TagOffsets[i], Br.ReadBytes((int) TagSizes[i]));
							DevArea.Entries.Add(Ent);
						}

						Tags = null;
						TagOffsets = null;
						TagSizes = null;
					}
					#endregion

					#region If Ext Area exist, read it.
					if (ExtAreaOffset != 0) {
						stream.Seek(ExtAreaOffset, SeekOrigin.Begin);
						ushort ExtAreaSize = Math.Max((ushort) TgaExtArea.MinSize, Br.ReadUInt16());
						stream.Seek(ExtAreaOffset, SeekOrigin.Begin);
						ExtArea = new TgaExtArea(Br.ReadBytes(ExtAreaSize));

						if (ExtArea.ScanLineOffset > 0) {
							stream.Seek(ExtArea.ScanLineOffset, SeekOrigin.Begin);
							ExtArea.ScanLineTable = new uint[Height];
							for (int i = 0; i < ExtArea.ScanLineTable.Length; i++)
								ExtArea.ScanLineTable[i] = Br.ReadUInt32();
						}

						if (ExtArea.PostageStampOffset > 0) {
							stream.Seek(ExtArea.PostageStampOffset, SeekOrigin.Begin);
							byte W = Br.ReadByte();
							byte H = Br.ReadByte();
							int ImgDataSize = W * H * BytesPerPixel;
							if (ImgDataSize > 0)
								ExtArea.PostageStampImage = new TgaPostageStampImage(W, H, Br.ReadBytes(ImgDataSize));
						}

						if (ExtArea.ColorCorrectionTableOffset > 0) {
							stream.Seek(ExtArea.ColorCorrectionTableOffset, SeekOrigin.Begin);
							ExtArea.ColorCorrectionTable = new ushort[256 * 4];
							for (int i = 0; i < ExtArea.ColorCorrectionTable.Length; i++)
								ExtArea.ColorCorrectionTable[i] = Br.ReadUInt16();
						}
					}
					#endregion
				}
				#endregion

				Br.Close();
				return true;
			} catch {
				return false;
			}
		}

		bool LoadFunc(Bitmap bmp, bool UseRLE = false, bool NewFormat = true, bool ColorMap2BytesEntry = false) {
			if (bmp == null)
				throw new ArgumentNullException();

			try {
				Header.ImageSpec.ImageWidth = (ushort) bmp.Width;
				Header.ImageSpec.ImageHeight = (ushort) bmp.Height;
				Header.ImageSpec.ImageDescriptor.ImageOrigin = TgaImgOrigin.TopLeft;

				switch (bmp.PixelFormat) {
					case PixelFormat.Indexed:
					case PixelFormat.Gdi:
					case PixelFormat.Alpha:
					case PixelFormat.Undefined:
					case PixelFormat.PAlpha:
					case PixelFormat.Extended:
					case PixelFormat.Max:
					case PixelFormat.Canonical:
					case PixelFormat.Format16bppRgb565:
					default:
						throw new FormatException(nameof(PixelFormat) + " is not supported!");

					case PixelFormat.Format1bppIndexed:
					case PixelFormat.Format4bppIndexed:
					case PixelFormat.Format8bppIndexed:
					case PixelFormat.Format16bppGrayScale:
					case PixelFormat.Format16bppRgb555:
					case PixelFormat.Format16bppArgb1555:
					case PixelFormat.Format24bppRgb:
					case PixelFormat.Format32bppRgb:
					case PixelFormat.Format32bppArgb:
					case PixelFormat.Format32bppPArgb:
					case PixelFormat.Format48bppRgb:
					case PixelFormat.Format64bppArgb:
					case PixelFormat.Format64bppPArgb:

						int bpp = Math.Max(8, Image.GetPixelFormatSize(bmp.PixelFormat));
						int BytesPP = bpp / 8;

						if (bmp.PixelFormat == PixelFormat.Format16bppRgb555)
							bpp = 15;

						bool IsAlpha = Image.IsAlphaPixelFormat(bmp.PixelFormat);
						bool IsPreAlpha = IsAlpha && bmp.PixelFormat.ToString().EndsWith("PArgb");
						bool IsColorMapped = bmp.PixelFormat.ToString().EndsWith("Indexed");

						Header.ImageSpec.PixelDepth = (TgaPixelDepth) (BytesPP * 8);

						if (IsAlpha) {
							Header.ImageSpec.ImageDescriptor.AlphaChannelBits = (byte) (BytesPP * 2);

							if (bmp.PixelFormat == PixelFormat.Format16bppArgb1555)
								Header.ImageSpec.ImageDescriptor.AlphaChannelBits = 1;
						}

						#region ColorMap
						bool IsGrayImage = (bmp.PixelFormat == PixelFormat.Format16bppGrayScale | IsColorMapped);

						if (IsColorMapped && bmp.Palette != null) {
							Color[] Colors = bmp.Palette.Entries;

							#region Analyze ColorMapType
							int AlphaSum = 0;
							bool ColorMapUseAlpha = false;

							for (int i = 0; i < Colors.Length; i++) {
								IsGrayImage &= (Colors[i].R == Colors[i].G && Colors[i].G == Colors[i].B);
								ColorMapUseAlpha |= (Colors[i].A < 248);
								AlphaSum |= Colors[i].A;
							}
							ColorMapUseAlpha &= (AlphaSum > 0);

							int CMapBpp = (ColorMap2BytesEntry ? 15 : 24) + (ColorMapUseAlpha ? (ColorMap2BytesEntry ? 1 : 8) : 0);
							int CMBytesPP = (int) Math.Ceiling(CMapBpp / 8.0);
							#endregion

							Header.ColorMapSpec.ColorMapLength = Math.Min((ushort) Colors.Length, ushort.MaxValue);
							Header.ColorMapSpec.ColorMapEntrySize = (TgaColorMapEntrySize) CMapBpp;
							ImageOrColorMapArea.ColorMapData = new byte[Header.ColorMapSpec.ColorMapLength * CMBytesPP];

							byte[] CMapEntry = new byte[CMBytesPP];

							const float To5Bit = 32f / 256f; // Scale value from 8 to 5 bits.
							for (int i = 0; i < Colors.Length; i++) {
								switch (Header.ColorMapSpec.ColorMapEntrySize) {
									case TgaColorMapEntrySize.A1R5G5B5:
									case TgaColorMapEntrySize.X1R5G5B5:
										int R = (int) (Colors[i].R * To5Bit);
										int G = (int) (Colors[i].G * To5Bit) << 5;
										int B = (int) (Colors[i].B * To5Bit) << 10;
										int A = 0;

										if (Header.ColorMapSpec.ColorMapEntrySize == TgaColorMapEntrySize.A1R5G5B5)
											A = ((Colors[i].A & 0x80) << 15);

										CMapEntry = BitConverter.GetBytes(A | R | G | B);
										break;

									case TgaColorMapEntrySize.R8G8B8:
										CMapEntry[0] = Colors[i].B;
										CMapEntry[1] = Colors[i].G;
										CMapEntry[2] = Colors[i].R;
										break;

									case TgaColorMapEntrySize.A8R8G8B8:
										CMapEntry[0] = Colors[i].B;
										CMapEntry[1] = Colors[i].G;
										CMapEntry[2] = Colors[i].R;
										CMapEntry[3] = Colors[i].A;
										break;

									case TgaColorMapEntrySize.Other:
									default:
										break;
								}

								Buffer.BlockCopy(CMapEntry, 0, ImageOrColorMapArea.ColorMapData, i * CMBytesPP, CMBytesPP);
							}
						}
						#endregion

						#region ImageType
						if (UseRLE) {
							if (IsGrayImage)
								Header.ImageType = TgaImageType.RLE_BlackWhite;
							else if (IsColorMapped)
								Header.ImageType = TgaImageType.RLE_ColorMapped;
							else
								Header.ImageType = TgaImageType.RLE_TrueColor;
						} else {
							if (IsGrayImage)
								Header.ImageType = TgaImageType.Uncompressed_BlackWhite;
							else if (IsColorMapped)
								Header.ImageType = TgaImageType.Uncompressed_ColorMapped;
							else
								Header.ImageType = TgaImageType.Uncompressed_TrueColor;
						}

						Header.ColorMapType = (IsColorMapped ? TgaColorMapType.ColorMap : TgaColorMapType.NoColorMap);
						#endregion

						#region NewFormat
						if (NewFormat) {
							Footer = new TgaFooter();
							ExtArea = new TgaExtArea();
							ExtArea.DateTimeStamp = new TgaDateTime(DateTime.UtcNow);

							if (IsAlpha) {
								ExtArea.AttributesType = TgaAttrType.UsefulAlpha;

								if (IsPreAlpha)
									ExtArea.AttributesType = TgaAttrType.PreMultipliedAlpha;
							} else {
								ExtArea.AttributesType = TgaAttrType.NoAlpha;

								if (Header.ImageSpec.ImageDescriptor.AlphaChannelBits > 0)
									ExtArea.AttributesType = TgaAttrType.UndefinedAlphaButShouldBeRetained;
							}
						}
						#endregion

						#region Bitmap width is aligned by 32 bits = 4 bytes! Delete it.
						int StrideBytes = bmp.Width * BytesPP;
						int PaddingBytes = (int) Math.Ceiling(StrideBytes / 4.0) * 4 - StrideBytes;

						byte[] ImageData = new byte[(StrideBytes + PaddingBytes) * bmp.Height];

						Rectangle Re = new Rectangle(0, 0, bmp.Width, bmp.Height);
						BitmapData BmpData = bmp.LockBits(Re, ImageLockMode.ReadOnly, bmp.PixelFormat);
						Marshal.Copy(BmpData.Scan0, ImageData, 0, ImageData.Length);
						bmp.UnlockBits(BmpData);
						BmpData = null;

						if (PaddingBytes > 0) //Need delete bytes align
						{
							ImageOrColorMapArea.ImageData = new byte[StrideBytes * bmp.Height];
							for (int i = 0; i < bmp.Height; i++)
								Buffer.BlockCopy(ImageData, i * (StrideBytes + PaddingBytes),
									ImageOrColorMapArea.ImageData, i * StrideBytes, StrideBytes);
						} else
							ImageOrColorMapArea.ImageData = ImageData;

						ImageData = null;

						// Not official supported, but works (tested on 16bpp GrayScale test images)!
						if (bmp.PixelFormat == PixelFormat.Format16bppGrayScale) {
							for (long i = 0; i < ImageOrColorMapArea.ImageData.Length; i++)
								ImageOrColorMapArea.ImageData[i] ^= byte.MaxValue;
						}
						#endregion

						break;
				}

				return true;
			} catch {
				return false;
			}
		}

		bool SaveFunc(Stream stream) {
			try {
				if (stream == null)
					throw new ArgumentNullException();
				if (!(stream.CanWrite && stream.CanSeek))
					throw new FileLoadException("Stream writing or seeking is not avaiable!");

				string CheckResult;
				if (!CheckAndUpdateOffsets(out CheckResult))
					return false;

				BinaryWriter Bw = new BinaryWriter(stream);
				Bw.Write(Header.ToBytes());

				if (ImageOrColorMapArea.ImageID != null)
					Bw.Write(ImageOrColorMapArea.ImageID.ToBytes());

				if (Header.ColorMapType != TgaColorMapType.NoColorMap)
					Bw.Write(ImageOrColorMapArea.ColorMapData);

				#region ImageData
				if (Header.ImageType != TgaImageType.NoImageData) {
					if (Header.ImageType >= TgaImageType.RLE_ColorMapped &&
						Header.ImageType <= TgaImageType.RLE_BlackWhite)
						Bw.Write(RLE_Encode(ImageOrColorMapArea.ImageData, Width, Height));
					else
						Bw.Write(ImageOrColorMapArea.ImageData);
				}
				#endregion

				#region Footer
				if (Footer != null) {
					#region DevArea
					if (DevArea != null) {
						for (int i = 0; i < DevArea.Count; i++)
							Bw.Write(DevArea[i].Data);

						Bw.Write((ushort) DevArea.Count);

						for (int i = 0; i < DevArea.Count; i++) {
							Bw.Write(DevArea[i].Tag);
							Bw.Write(DevArea[i].Offset);
							Bw.Write(DevArea[i].FieldSize);
						}
					}
					#endregion

					#region ExtArea
					if (ExtArea != null) {
						Bw.Write(ExtArea.ToBytes());

						if (ExtArea.ScanLineTable != null)
							for (int i = 0; i < ExtArea.ScanLineTable.Length; i++)
								Bw.Write(ExtArea.ScanLineTable[i]);

						if (ExtArea.PostageStampImage != null)
							Bw.Write(ExtArea.PostageStampImage.ToBytes());

						if (ExtArea.ColorCorrectionTable != null)
							for (int i = 0; i < ExtArea.ColorCorrectionTable.Length; i++)
								Bw.Write(ExtArea.ColorCorrectionTable[i]);
					}
					#endregion

					Bw.Write(Footer.ToBytes());
				}
				#endregion

				Bw.Flush();
				stream.Flush();
				return true;
			} catch {
				return false;
			}
		}

		/// <summary>
		/// Encode image with RLE compression (used RLE per line)!
		/// </summary>
		/// <param name="ImageData">Image data, bytes array with size = Width * Height * BytesPerPixel.</param>
		/// <param name="Width">Image Width, must be > 0.</param>
		/// <param name="Height">Image Height, must be > 0.</param>
		/// <returns>Bytes array with RLE compressed image data.</returns>
		static byte[] RLE_Encode(byte[] ImageData, int Width, int Height) {
			if (ImageData == null)
				throw new ArgumentNullException(nameof(ImageData) + "in null!");

			if (Width <= 0 || Height <= 0)
				throw new ArgumentOutOfRangeException(nameof(Width) + " and " + nameof(Height) + " must be > 0!");

			int Bpp = ImageData.Length / Width / Height; // Bytes per pixel
			int ScanLineSize = Width * Bpp;

			if (ScanLineSize * Height != ImageData.Length)
				throw new ArgumentOutOfRangeException("ImageData has wrong Length!");

			try {
				int Count = 0;
				int Pos = 0;
				bool IsRLE = false;
				List<byte> Encoded = new List<byte>();
				byte[] RowData = new byte[ScanLineSize];

				for (int y = 0; y < Height; y++) {
					Pos = 0;
					Buffer.BlockCopy(ImageData, y * ScanLineSize, RowData, 0, ScanLineSize);

					while (Pos < ScanLineSize) {
						if (Pos >= ScanLineSize - Bpp) {
							Encoded.Add(0);
							Encoded.AddRange(BitConverterExt.GetElements(RowData, Pos, Bpp));
							Pos += Bpp;
							break;
						}

						Count = 0; //1
						IsRLE = BitConverterExt.IsElementsEqual(RowData, Pos, Pos + Bpp, Bpp);

						for (int i = Pos + Bpp; i < Math.Min(Pos + 128 * Bpp, ScanLineSize) - Bpp; i += Bpp) {
							if (IsRLE ^ BitConverterExt.IsElementsEqual(RowData, (IsRLE ? Pos : i), i + Bpp, Bpp)) {
								//Count--;
								break;
							} else
								Count++;
						}

						int CountBpp = (Count + 1) * Bpp;
						Encoded.Add((byte) (IsRLE ? Count | 128 : Count));
						Encoded.AddRange(BitConverterExt.GetElements(RowData, Pos, (IsRLE ? Bpp : CountBpp)));
						Pos += CountBpp;
					}
				}

				return Encoded.ToArray();
			} catch {
				return null;
			}
		}

		/// <summary>
		/// Convert <see cref="TargaImage"/> to <see cref="Bitmap"/>.
		/// </summary>
		/// <param name="ForceUseAlpha">Force use alpha channel.</param>
		/// <param name="PostageStampImage">Get Postage Stamp Image (Thumb) or get main image?</param>
		/// <returns>Bitmap or null, on error.</returns>
		Bitmap ToBitmapFunc(bool ForceUseAlpha = false, bool PostageStampImage = false) {
			try {
				#region UseAlpha?
				bool UseAlpha = true;
				if (ExtArea != null) {
					switch (ExtArea.AttributesType) {
						case TgaAttrType.NoAlpha:
						case TgaAttrType.UndefinedAlphaCanBeIgnored:
						case TgaAttrType.UndefinedAlphaButShouldBeRetained:
							UseAlpha = false;
							break;
						case TgaAttrType.UsefulAlpha:
						case TgaAttrType.PreMultipliedAlpha:
						default:
							break;
					}
				}
				UseAlpha = (Header.ImageSpec.ImageDescriptor.AlphaChannelBits > 0 && UseAlpha) | ForceUseAlpha;
				#endregion

				#region IsGrayImage
				bool IsGrayImage = Header.ImageType == TgaImageType.RLE_BlackWhite ||
					Header.ImageType == TgaImageType.Uncompressed_BlackWhite;
				#endregion

				#region Get PixelFormat
				PixelFormat PixFormat = PixelFormat.Format24bppRgb;

				switch (Header.ImageSpec.PixelDepth) {
					case TgaPixelDepth.Bpp8:
						PixFormat = PixelFormat.Format8bppIndexed;
						break;

					case TgaPixelDepth.Bpp16:
						if (IsGrayImage)
							PixFormat = PixelFormat.Format16bppGrayScale;
						else
							PixFormat = (UseAlpha ? PixelFormat.Format16bppArgb1555 : PixelFormat.Format16bppRgb555);
						break;

					case TgaPixelDepth.Bpp24:
						PixFormat = PixelFormat.Format24bppRgb;
						break;

					case TgaPixelDepth.Bpp32:
						if (UseAlpha) {
							var f = Footer;
							if (ExtArea?.AttributesType == TgaAttrType.PreMultipliedAlpha)
								PixFormat = PixelFormat.Format32bppPArgb;
							else
								PixFormat = PixelFormat.Format32bppArgb;
						} else
							PixFormat = PixelFormat.Format32bppRgb;
						break;

					default:
						PixFormat = PixelFormat.Undefined;
						break;
				}
				#endregion

				ushort BMP_Width = (PostageStampImage ? ExtArea.PostageStampImage.Width : Width);
				ushort BMP_Height = (PostageStampImage ? ExtArea.PostageStampImage.Height : Height);
				Bitmap BMP = new Bitmap(BMP_Width, BMP_Height, PixFormat);

				#region ColorMap and GrayPalette
				if (Header.ColorMapType == TgaColorMapType.ColorMap &&
				   (Header.ImageType == TgaImageType.RLE_ColorMapped ||
					Header.ImageType == TgaImageType.Uncompressed_ColorMapped)) {

					ColorPalette ColorMap = BMP.Palette;
					Color[] CMapColors = ColorMap.Entries;

					switch (Header.ColorMapSpec.ColorMapEntrySize) {
						case TgaColorMapEntrySize.X1R5G5B5:
						case TgaColorMapEntrySize.A1R5G5B5:
							const float To8Bit = 255f / 31f; // Scale value from 5 to 8 bits.
							for (int i = 0; i < Math.Min(CMapColors.Length, Header.ColorMapSpec.ColorMapLength); i++) {
								ushort A1R5G5B5 = BitConverter.ToUInt16(ImageOrColorMapArea.ColorMapData, i * 2);
								int A = (UseAlpha ? (A1R5G5B5 & 0x8000) >> 15 : 1) * 255; // (0 or 1) * 255
								int R = (int) (((A1R5G5B5 & 0x7C00) >> 10) * To8Bit);
								int G = (int) (((A1R5G5B5 & 0x3E0) >> 5) * To8Bit);
								int B = (int) ((A1R5G5B5 & 0x1F) * To8Bit);
								CMapColors[i] = Color.FromArgb(A, R, G, B);
							}
							break;

						case TgaColorMapEntrySize.R8G8B8:
							for (int i = 0; i < Math.Min(CMapColors.Length, Header.ColorMapSpec.ColorMapLength); i++) {
								int Index = i * 3; //RGB = 3 bytes
								int R = ImageOrColorMapArea.ColorMapData[Index + 2];
								int G = ImageOrColorMapArea.ColorMapData[Index + 1];
								int B = ImageOrColorMapArea.ColorMapData[Index];
								CMapColors[i] = Color.FromArgb(R, G, B);
							}
							break;

						case TgaColorMapEntrySize.A8R8G8B8:
							for (int i = 0; i < Math.Min(CMapColors.Length, Header.ColorMapSpec.ColorMapLength); i++) {
								int ARGB = BitConverter.ToInt32(ImageOrColorMapArea.ColorMapData, i * 4);
								CMapColors[i] = Color.FromArgb(UseAlpha ? ARGB | (0xFF << 24) : ARGB);
							}
							break;

						default:
							ColorMap = null;
							break;
					}

					if (ColorMap != null)
						BMP.Palette = ColorMap;
				}

				if (PixFormat == PixelFormat.Format8bppIndexed && IsGrayImage) {
					ColorPalette GrayPalette = BMP.Palette;
					Color[] GrayColors = GrayPalette.Entries;
					for (int i = 0; i < GrayColors.Length; i++)
						GrayColors[i] = Color.FromArgb(i, i, i);
					BMP.Palette = GrayPalette;
				}
				#endregion

				#region Bitmap width must by aligned (align value = 32 bits = 4 bytes)!
				byte[] ImageData;
				int BytesPerPixel = (int) Math.Ceiling((double) Header.ImageSpec.PixelDepth / 8.0);
				int StrideBytes = BMP.Width * BytesPerPixel;
				int PaddingBytes = (int) Math.Ceiling(StrideBytes / 4.0) * 4 - StrideBytes;

				if (PaddingBytes > 0) //Need bytes align
				{
					ImageData = new byte[(StrideBytes + PaddingBytes) * BMP.Height];
					for (int i = 0; i < BMP.Height; i++)
						Buffer.BlockCopy(PostageStampImage ? ExtArea.PostageStampImage.Data :
							ImageOrColorMapArea.ImageData, i * StrideBytes, ImageData,
							i * (StrideBytes + PaddingBytes), StrideBytes);
				} else
					ImageData = BitConverterExt.ToBytes(PostageStampImage ? ExtArea.PostageStampImage.Data :
						ImageOrColorMapArea.ImageData);

				// Not official supported, but works (tested on 2 test images)!
				if (PixFormat == PixelFormat.Format16bppGrayScale) {
					for (long i = 0; i < ImageData.Length; i++)
						ImageData[i] ^= byte.MaxValue;
				}
				#endregion

				Rectangle Re = new Rectangle(0, 0, BMP.Width, BMP.Height);
				BitmapData BmpData = BMP.LockBits(Re, ImageLockMode.WriteOnly, BMP.PixelFormat);
				Marshal.Copy(ImageData, 0, BmpData.Scan0, ImageData.Length);
				BMP.UnlockBits(BmpData);
				ImageData = null;
				BmpData = null;

				if (ExtArea != null && ExtArea.KeyColor.ToInt() != 0)
					BMP.MakeTransparent(ExtArea.KeyColor.ToColor());

				#region Flip Image
				switch (Header.ImageSpec.ImageDescriptor.ImageOrigin) {
					case TgaImgOrigin.BottomLeft:
						BMP.RotateFlip(RotateFlipType.RotateNoneFlipY);
						break;
					case TgaImgOrigin.BottomRight:
						BMP.RotateFlip(RotateFlipType.RotateNoneFlipXY);
						break;
					case TgaImgOrigin.TopLeft:
					default:
						break;
					case TgaImgOrigin.TopRight:
						BMP.RotateFlip(RotateFlipType.RotateNoneFlipX);
						break;
				}
				#endregion

				return BMP;
			} catch {
				return null;
			}
		}
		#endregion

		#region Explicit
		public static explicit operator Bitmap(TargaImage tga) {
			return tga.ToBitmap();
		}

		public static explicit operator TargaImage(Bitmap bmp) {
			return FromBitmap(bmp);
		}
		#endregion

		#region PostageStamp Image
		/// <summary>
		/// Convert <see cref="TgaPostageStampImage"/> to <see cref="Bitmap"/>.
		/// </summary>
		/// <param name="ForceUseAlpha">Force use alpha channel.</param>
		/// <returns>Bitmap or null.</returns>
		public Bitmap GetPostageStampImage(bool ForceUseAlpha = false) {
			if (ExtArea == null || ExtArea.PostageStampImage == null || ExtArea.PostageStampImage.Data == null ||
				ExtArea.PostageStampImage.Width <= 0 || ExtArea.PostageStampImage.Height <= 0)
				return null;

			return ToBitmapFunc(ForceUseAlpha, true);
		}

		/// <summary>
		/// Update Postage Stamp Image or set it.
		/// </summary>
		public void UpdatePostageStampImage() {
			if (Header.ImageType == TgaImageType.NoImageData) {
				if (ExtArea != null)
					ExtArea.PostageStampImage = null;
				return;
			}

			ToNewFormat();
			if (ExtArea.PostageStampImage == null)
				ExtArea.PostageStampImage = new TgaPostageStampImage();

			int PS_Width = Header.ImageSpec.ImageWidth;
			int PS_Height = Header.ImageSpec.ImageHeight;

			if (Width > 64 || Height > 64) {
				float AspectRatio = Width / (float) Height;
				PS_Width = (byte) (64f * (AspectRatio < 1f ? AspectRatio : 1f));
				PS_Height = (byte) (64f / (AspectRatio > 1f ? AspectRatio : 1f));
			}
			PS_Width = Math.Max(PS_Width, 4);
			PS_Height = Math.Max(PS_Height, 4);

			ExtArea.PostageStampImage.Width = (byte) PS_Width;
			ExtArea.PostageStampImage.Height = (byte) PS_Height;

			int BytesPerPixel = (int) Math.Ceiling((double) Header.ImageSpec.PixelDepth / 8.0);
			ExtArea.PostageStampImage.Data = new byte[PS_Width * PS_Height * BytesPerPixel];

			float WidthCoef = Width / (float) PS_Width;
			float HeightCoef = Height / (float) PS_Height;

			for (int y = 0; y < PS_Height; y++) {
				int Y_Offset = (int) (y * HeightCoef) * Width * BytesPerPixel;
				int y_Offset = y * PS_Width * BytesPerPixel;

				for (int x = 0; x < PS_Width; x++) {
					Buffer.BlockCopy(ImageOrColorMapArea.ImageData, Y_Offset + (int) (x * WidthCoef) * BytesPerPixel,
						ExtArea.PostageStampImage.Data, y_Offset + x * BytesPerPixel, BytesPerPixel);
				}
			}
		}

		public void DeletePostageStampImage() {
			if (ExtArea != null)
				ExtArea.PostageStampImage = null;
		}
		#endregion
	}
}

#pragma warning restore 1591