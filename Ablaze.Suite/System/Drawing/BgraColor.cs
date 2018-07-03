using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Drawing {
	/// <summary>
	/// Represents a color with 4 byte components (B, G, R, A).
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct BgraColor : IEquatable<BgraColor> {
		/// <summary>
		/// The blue component of this BgraColor structure.
		/// </summary>
		public byte B;
		/// <summary>
		/// The green component of this BgraColor structure.
		/// </summary>
		public byte G;
		/// <summary>
		/// The red component of this BgraColor structure.
		/// </summary>
		public byte R;
		/// <summary>
		/// The alpha component of this BgraColor structure.
		/// </summary>
		public byte A;

		/// <summary>
		/// Empty transparent color (ARGB: 0, 0, 0, 0)
		/// </summary>
		public static readonly BgraColor Empty = new BgraColor();
		/// <summary>
		/// Transparent color (ARGB: 0, 255, 255, 255)
		/// </summary>
		public static readonly BgraColor Transparent = new BgraColor(0, 255, 255, 255);

		/// <summary>
		/// RGB: 240, 248, 255
		/// </summary>
		public static readonly BgraColor AliceBlue = new BgraColor(240, 248, 255);
		/// <summary>
		/// RGB: 250, 235, 215
		/// </summary>
		public static readonly BgraColor AntiqueWhite = new BgraColor(250, 235, 215);
		/// <summary>
		/// RGB: 127, 255, 212
		/// </summary>
		public static readonly BgraColor AquaMarine = new BgraColor(127, 255, 212);
		/// <summary>
		/// RGB: 240, 255, 255
		/// </summary>
		public static readonly BgraColor Azure = new BgraColor(240, 255, 255);
		/// <summary>
		/// RGB: 245, 245, 220
		/// </summary>
		public static readonly BgraColor Beige = new BgraColor(245, 245, 220);
		/// <summary>
		/// RGB: 255, 228, 196
		/// </summary>
		public static readonly BgraColor Bisque = new BgraColor(255, 228, 196);
		/// <summary>
		/// RGB: 0, 0, 0
		/// </summary>
		public static readonly BgraColor Black = new BgraColor(0, 0, 0);
		/// <summary>
		/// RGB: 255, 235, 205
		/// </summary>
		public static readonly BgraColor BlanchedAlmond = new BgraColor(255, 235, 205);
		/// <summary>
		/// RGB: 0, 0, 255
		/// </summary>
		public static readonly BgraColor Blue = new BgraColor(0, 0, 255);
		/// <summary>
		/// RGB: 138, 43, 226
		/// </summary>
		public static readonly BgraColor BlueViolet = new BgraColor(138, 43, 226);
		/// <summary>
		/// RGB: 165, 42, 42
		/// </summary>
		public static readonly BgraColor Brown = new BgraColor(165, 42, 42);
		/// <summary>
		/// RGB: 222, 184, 135
		/// </summary>
		public static readonly BgraColor BurlyWood = new BgraColor(222, 184, 135);
		/// <summary>
		/// RGB: 95, 158, 160
		/// </summary>
		public static readonly BgraColor CadetBlue = new BgraColor(95, 158, 160);
		/// <summary>
		/// RGB: 127, 255, 0
		/// </summary>
		public static readonly BgraColor Chartreuse = new BgraColor(127, 255, 0);
		/// <summary>
		/// RGB: 210, 105, 30
		/// </summary>
		public static readonly BgraColor Chocolate = new BgraColor(210, 105, 30);
		/// <summary>
		/// RGB: 255, 127, 80
		/// </summary>
		public static readonly BgraColor Coral = new BgraColor(255, 127, 80);
		/// <summary>
		/// RGB: 100, 149, 237
		/// </summary>
		public static readonly BgraColor CornFlowerBlue = new BgraColor(100, 149, 237);
		/// <summary>
		/// RGB: 255, 248, 220
		/// </summary>
		public static readonly BgraColor Cornsilk = new BgraColor(255, 248, 220);
		/// <summary>
		/// RGB: 220, 20, 60
		/// </summary>
		public static readonly BgraColor Crimson = new BgraColor(220, 20, 60);
		/// <summary>
		/// RGB: 0, 255, 255
		/// </summary>
		public static readonly BgraColor Cyan = new BgraColor(0, 255, 255);
		/// <summary>
		/// RGB: 0, 0, 139
		/// </summary>
		public static readonly BgraColor DarkBlue = new BgraColor(0, 0, 139);
		/// <summary>
		/// RGB: 0, 139, 139
		/// </summary>
		public static readonly BgraColor DarkCyan = new BgraColor(0, 139, 139);
		/// <summary>
		/// RGB: 184, 134, 11
		/// </summary>
		public static readonly BgraColor DarkGoldenRod = new BgraColor(184, 134, 11);
		/// <summary>
		/// RGB: 169, 169, 169
		/// </summary>
		public static readonly BgraColor DarkGray = new BgraColor(169, 169, 169);
		/// <summary>
		/// RGB: 0, 100, 0
		/// </summary>
		public static readonly BgraColor DarkGreen = new BgraColor(0, 100, 0);
		/// <summary>
		/// RGB: 189, 183, 107
		/// </summary>
		public static readonly BgraColor DarkKhaki = new BgraColor(189, 183, 107);
		/// <summary>
		/// RGB: 139, 0, 139
		/// </summary>
		public static readonly BgraColor DarkMagenta = new BgraColor(139, 0, 139);
		/// <summary>
		/// RGB: 85, 107, 47
		/// </summary>
		public static readonly BgraColor DarkOliveGreen = new BgraColor(85, 107, 47);
		/// <summary>
		/// RGB: 255, 140, 0
		/// </summary>
		public static readonly BgraColor DarkOrange = new BgraColor(255, 140, 0);
		/// <summary>
		/// RGB: 153, 50, 204
		/// </summary>
		public static readonly BgraColor DarkOrchid = new BgraColor(153, 50, 204);
		/// <summary>
		/// RGB: 139, 0, 0
		/// </summary>
		public static readonly BgraColor DarkRed = new BgraColor(139, 0, 0);
		/// <summary>
		/// RGB: 233, 150, 122
		/// </summary>
		public static readonly BgraColor DarkSalmon = new BgraColor(233, 150, 122);
		/// <summary>
		/// RGB: 143, 188, 139
		/// </summary>
		public static readonly BgraColor DarkSeaGreen = new BgraColor(143, 188, 139);
		/// <summary>
		/// RGB: 72, 61, 139
		/// </summary>
		public static readonly BgraColor DarkSlateBlue = new BgraColor(72, 61, 139);
		/// <summary>
		/// RGB: 47, 79, 79
		/// </summary>
		public static readonly BgraColor DarkSlateGray = new BgraColor(47, 79, 79);
		/// <summary>
		/// RGB: 0, 206, 209
		/// </summary>
		public static readonly BgraColor DarkTurquoise = new BgraColor(0, 206, 209);
		/// <summary>
		/// RGB: 148, 0, 211
		/// </summary>
		public static readonly BgraColor DarkViolet = new BgraColor(148, 0, 211);
		/// <summary>
		/// RGB: 255, 20, 147
		/// </summary>
		public static readonly BgraColor DeepPink = new BgraColor(255, 20, 147);
		/// <summary>
		/// RGB: 0, 191, 255
		/// </summary>
		public static readonly BgraColor DeepSkyBlue = new BgraColor(0, 191, 255);
		/// <summary>
		/// RGB: 105, 105, 105
		/// </summary>
		public static readonly BgraColor DimGray = new BgraColor(105, 105, 105);
		/// <summary>
		/// RGB: 30, 144, 255
		/// </summary>
		public static readonly BgraColor DodgerBlue = new BgraColor(30, 144, 255);
		/// <summary>
		/// RGB: 178, 34, 34
		/// </summary>
		public static readonly BgraColor FireBrick = new BgraColor(178, 34, 34);
		/// <summary>
		/// RGB: 255, 250, 240
		/// </summary>
		public static readonly BgraColor FloralWhite = new BgraColor(255, 250, 240);
		/// <summary>
		/// RGB: 34, 139, 34
		/// </summary>
		public static readonly BgraColor ForestGreen = new BgraColor(34, 139, 34);
		/// <summary>
		/// RGB: 255, 0, 255
		/// </summary>
		public static readonly BgraColor Fuchsia = new BgraColor(255, 0, 255);
		/// <summary>
		/// RGB: 220, 220, 220
		/// </summary>
		public static readonly BgraColor Gainsboro = new BgraColor(220, 220, 220);
		/// <summary>
		/// RGB: 248, 248, 255
		/// </summary>
		public static readonly BgraColor GhostWhite = new BgraColor(248, 248, 255);
		/// <summary>
		/// RGB: 255, 215, 0
		/// </summary>
		public static readonly BgraColor Gold = new BgraColor(255, 215, 0);
		/// <summary>
		/// RGB: 218, 165, 32
		/// </summary>
		public static readonly BgraColor GoldenRod = new BgraColor(218, 165, 32);
		/// <summary>
		/// RGB: 128, 128, 128
		/// </summary>
		public static readonly BgraColor Gray = new BgraColor(128, 128, 128);
		/// <summary>
		/// RGB: 0, 128, 0
		/// </summary>
		public static readonly BgraColor Green = new BgraColor(0, 128, 0);
		/// <summary>
		/// RGB: 173, 255, 47
		/// </summary>
		public static readonly BgraColor GreenYellow = new BgraColor(173, 255, 47);
		/// <summary>
		/// RGB: 240, 255, 240
		/// </summary>
		public static readonly BgraColor HoneyDew = new BgraColor(240, 255, 240);
		/// <summary>
		/// RGB: 255, 105, 180
		/// </summary>
		public static readonly BgraColor HotPink = new BgraColor(255, 105, 180);
		/// <summary>
		/// RGB: 205, 92, 92
		/// </summary>
		public static readonly BgraColor IndianRed = new BgraColor(205, 92, 92);
		/// <summary>
		/// RGB: 75, 0, 130
		/// </summary>
		public static readonly BgraColor Indigo = new BgraColor(75, 0, 130);
		/// <summary>
		/// RGB: 255, 255, 240
		/// </summary>
		public static readonly BgraColor Ivory = new BgraColor(255, 255, 240);
		/// <summary>
		/// RGB: 240, 230, 140
		/// </summary>
		public static readonly BgraColor Khaki = new BgraColor(240, 230, 140);
		/// <summary>
		/// RGB: 230, 230, 250
		/// </summary>
		public static readonly BgraColor Lavender = new BgraColor(230, 230, 250);
		/// <summary>
		/// RGB: 255, 240, 245
		/// </summary>
		public static readonly BgraColor LavenderBlush = new BgraColor(255, 240, 245);
		/// <summary>
		/// RGB: 124, 252, 0
		/// </summary>
		public static readonly BgraColor LawnGreen = new BgraColor(124, 252, 0);
		/// <summary>
		/// RGB: 255, 250, 205
		/// </summary>
		public static readonly BgraColor LemonChiffon = new BgraColor(255, 250, 205);
		/// <summary>
		/// RGB: 173, 216, 230
		/// </summary>
		public static readonly BgraColor LightBlue = new BgraColor(173, 216, 230);
		/// <summary>
		/// RGB: 240, 128, 128
		/// </summary>
		public static readonly BgraColor LightCoral = new BgraColor(240, 128, 128);
		/// <summary>
		/// RGB: 224, 255, 255
		/// </summary>
		public static readonly BgraColor LightCyan = new BgraColor(224, 255, 255);
		/// <summary>
		/// RGB: 250, 250, 210
		/// </summary>
		public static readonly BgraColor LightGoldenRodYellow = new BgraColor(250, 250, 210);
		/// <summary>
		/// RGB: 211, 211, 211
		/// </summary>
		public static readonly BgraColor LightGray = new BgraColor(211, 211, 211);
		/// <summary>
		/// RGB: 144, 238, 144
		/// </summary>
		public static readonly BgraColor LightGreen = new BgraColor(144, 238, 144);
		/// <summary>
		/// RGB: 255, 182, 193
		/// </summary>
		public static readonly BgraColor LightPink = new BgraColor(255, 182, 193);
		/// <summary>
		/// RGB: 255, 160, 122
		/// </summary>
		public static readonly BgraColor LightSalmon = new BgraColor(255, 160, 122);
		/// <summary>
		/// RGB: 32, 178, 170
		/// </summary>
		public static readonly BgraColor LightSeaGreen = new BgraColor(32, 178, 170);
		/// <summary>
		/// RGB: 135, 206, 250
		/// </summary>
		public static readonly BgraColor LightSkyBlue = new BgraColor(135, 206, 250);
		/// <summary>
		/// RGB: 119, 136, 153
		/// </summary>
		public static readonly BgraColor LightSlateGray = new BgraColor(119, 136, 153);
		/// <summary>
		/// RGB: 176, 196, 222
		/// </summary>
		public static readonly BgraColor LightSteelBlue = new BgraColor(176, 196, 222);
		/// <summary>
		/// RGB: 255, 255, 224
		/// </summary>
		public static readonly BgraColor LightYellow = new BgraColor(255, 255, 224);
		/// <summary>
		/// RGB: 0, 255, 0
		/// </summary>
		public static readonly BgraColor Lime = new BgraColor(0, 255, 0);
		/// <summary>
		/// RGB: 50, 205, 50
		/// </summary>
		public static readonly BgraColor LimeGreen = new BgraColor(50, 205, 50);
		/// <summary>
		/// RGB: 250, 240, 230
		/// </summary>
		public static readonly BgraColor Linen = new BgraColor(250, 240, 230);
		/// <summary>
		/// RGB: 255, 0, 255
		/// </summary>
		public static readonly BgraColor Magenta = new BgraColor(255, 0, 255);
		/// <summary>
		/// RGB: 128, 0, 0
		/// </summary>
		public static readonly BgraColor Maroon = new BgraColor(128, 0, 0);
		/// <summary>
		/// RGB: 102, 205, 170
		/// </summary>
		public static readonly BgraColor MediumAquaMarine = new BgraColor(102, 205, 170);
		/// <summary>
		/// RGB: 0, 0, 205
		/// </summary>
		public static readonly BgraColor MediumBlue = new BgraColor(0, 0, 205);
		/// <summary>
		/// RGB: 186, 85, 211
		/// </summary>
		public static readonly BgraColor MediumOrchid = new BgraColor(186, 85, 211);
		/// <summary>
		/// RGB: 147, 112, 219
		/// </summary>
		public static readonly BgraColor MediumPurple = new BgraColor(147, 112, 219);
		/// <summary>
		/// RGB: 60, 179, 113
		/// </summary>
		public static readonly BgraColor MediumSeaGreen = new BgraColor(60, 179, 113);
		/// <summary>
		/// RGB: 123, 104, 238
		/// </summary>
		public static readonly BgraColor MediumSlateBlue = new BgraColor(123, 104, 238);
		/// <summary>
		/// RGB: 0, 250, 154
		/// </summary>
		public static readonly BgraColor MediumSpringGreen = new BgraColor(0, 250, 154);
		/// <summary>
		/// RGB: 72, 209, 204
		/// </summary>
		public static readonly BgraColor MediumTurquoise = new BgraColor(72, 209, 204);
		/// <summary>
		/// RGB: 199, 21, 133
		/// </summary>
		public static readonly BgraColor MediumVioletRed = new BgraColor(199, 21, 133);
		/// <summary>
		/// RGB: 25, 25, 112
		/// </summary>
		public static readonly BgraColor MidnightBlue = new BgraColor(25, 25, 112);
		/// <summary>
		/// RGB: 245, 255, 250
		/// </summary>
		public static readonly BgraColor MintCream = new BgraColor(245, 255, 250);
		/// <summary>
		/// RGB: 255, 228, 225
		/// </summary>
		public static readonly BgraColor MistyRose = new BgraColor(255, 228, 225);
		/// <summary>
		/// RGB: 255, 228, 181
		/// </summary>
		public static readonly BgraColor Moccasin = new BgraColor(255, 228, 181);
		/// <summary>
		/// RGB: 255, 222, 173
		/// </summary>
		public static readonly BgraColor NavajoWhite = new BgraColor(255, 222, 173);
		/// <summary>
		/// RGB: 0, 0, 128
		/// </summary>
		public static readonly BgraColor NavyBlue = new BgraColor(0, 0, 128);
		/// <summary>
		/// RGB: 253, 245, 230
		/// </summary>
		public static readonly BgraColor OldLace = new BgraColor(253, 245, 230);
		/// <summary>
		/// RGB: 128, 128, 0
		/// </summary>
		public static readonly BgraColor Olive = new BgraColor(128, 128, 0);
		/// <summary>
		/// RGB: 107, 142, 35
		/// </summary>
		public static readonly BgraColor OliveDrab = new BgraColor(107, 142, 35);
		/// <summary>
		/// RGB: 255, 165, 0
		/// </summary>
		public static readonly BgraColor Orange = new BgraColor(255, 165, 0);
		/// <summary>
		/// RGB: 255, 69, 0
		/// </summary>
		public static readonly BgraColor OrangeRed = new BgraColor(255, 69, 0);
		/// <summary>
		/// RGB: 218, 112, 214
		/// </summary>
		public static readonly BgraColor Orchid = new BgraColor(218, 112, 214);
		/// <summary>
		/// RGB: 250, 240, 230
		/// </summary>
		public static readonly BgraColor PaleGoldenRod = new BgraColor(238, 232, 170);
		/// <summary>
		/// RGB: 152, 251, 152
		/// </summary>
		public static readonly BgraColor PaleGreen = new BgraColor(152, 251, 152);
		/// <summary>
		/// RGB: 175, 238, 238
		/// </summary>
		public static readonly BgraColor PaleTurquoise = new BgraColor(175, 238, 238);
		/// <summary>
		/// RGB: 219, 112, 147
		/// </summary>
		public static readonly BgraColor PaleVioletRed = new BgraColor(219, 112, 147);
		/// <summary>
		/// RGB: 255, 239, 213
		/// </summary>
		public static readonly BgraColor PapayaWhip = new BgraColor(255, 239, 213);
		/// <summary>
		/// RGB: 255, 218, 185
		/// </summary>
		public static readonly BgraColor PeachPuff = new BgraColor(255, 218, 185);
		/// <summary>
		/// RGB: 205, 133, 65
		/// </summary>
		public static readonly BgraColor Peru = new BgraColor(205, 133, 65);
		/// <summary>
		/// RGB: 255, 192, 203
		/// </summary>
		public static readonly BgraColor Pink = new BgraColor(255, 192, 203);
		/// <summary>
		/// RGB: 221, 160, 221
		/// </summary>
		public static readonly BgraColor Plum = new BgraColor(221, 160, 221);
		/// <summary>
		/// RGB: 176, 224, 230
		/// </summary>
		public static readonly BgraColor PowderBlue = new BgraColor(176, 224, 230);
		/// <summary>
		/// RGB: 128, 0, 128
		/// </summary>
		public static readonly BgraColor Purple = new BgraColor(128, 0, 128);
		/// <summary>
		/// RGB: 255, 0, 0
		/// </summary>
		public static readonly BgraColor Red = new BgraColor(255, 0, 0);
		/// <summary>
		/// RGB: 188, 143, 143
		/// </summary>
		public static readonly BgraColor RosyBrown = new BgraColor(188, 143, 143);
		/// <summary>
		/// RGB: 65, 105, 225
		/// </summary>
		public static readonly BgraColor RoyalBlue = new BgraColor(65, 105, 225);
		/// <summary>
		/// RGB: 139, 69, 19
		/// </summary>
		public static readonly BgraColor SaddleBrown = new BgraColor(139, 69, 19);
		/// <summary>
		/// RGB: 250, 128, 114
		/// </summary>
		public static readonly BgraColor Salmon = new BgraColor(250, 128, 114);
		/// <summary>
		/// RGB: 244, 164, 96
		/// </summary>
		public static readonly BgraColor SandyBrown = new BgraColor(244, 164, 96);
		/// <summary>
		/// RGB: 46, 139, 87
		/// </summary>
		public static readonly BgraColor SeaGreen = new BgraColor(46, 139, 87);
		/// <summary>
		/// RGB: 255, 245, 238
		/// </summary>
		public static readonly BgraColor SeaShell = new BgraColor(255, 245, 238);
		/// <summary>
		/// RGB: 160, 82, 45
		/// </summary>
		public static readonly BgraColor Sienna = new BgraColor(160, 82, 45);
		/// <summary>
		/// RGB: 192, 192, 192
		/// </summary>
		public static readonly BgraColor Silver = new BgraColor(192, 192, 192);
		/// <summary>
		/// RGB: 135, 206, 235
		/// </summary>
		public static readonly BgraColor SkyBlue = new BgraColor(135, 206, 235);
		/// <summary>
		/// RGB: 106, 90, 205
		/// </summary>
		public static readonly BgraColor SlateBlue = new BgraColor(106, 90, 205);
		/// <summary>
		/// RGB: 112, 128, 144
		/// </summary>
		public static readonly BgraColor SlateGray = new BgraColor(112, 128, 144);
		/// <summary>
		/// RGB: 255, 250, 250
		/// </summary>
		public static readonly BgraColor Snow = new BgraColor(255, 250, 250);
		/// <summary>
		/// RGB: 0, 255, 127
		/// </summary>
		public static readonly BgraColor SpringGreen = new BgraColor(0, 255, 127);
		/// <summary>
		/// RGB: 70, 130, 180
		/// </summary>
		public static readonly BgraColor SteelBlue = new BgraColor(70, 130, 180);
		/// <summary>
		/// RGB: 210, 180, 140
		/// </summary>
		public static readonly BgraColor Tan = new BgraColor(210, 180, 140);
		/// <summary>
		/// RGB: 0, 128, 128
		/// </summary>
		public static readonly BgraColor Teal = new BgraColor(0, 128, 128);
		/// <summary>
		/// RGB: 216, 191, 216
		/// </summary>
		public static readonly BgraColor Thistle = new BgraColor(216, 191, 216);
		/// <summary>
		/// RGB: 255, 99, 71
		/// </summary>
		public static readonly BgraColor Tomato = new BgraColor(255, 99, 71);
		/// <summary>
		/// RGB: 64, 224, 208
		/// </summary>
		public static readonly BgraColor Turquoise = new BgraColor(64, 224, 208);
		/// <summary>
		/// RGB: 238, 130, 238
		/// </summary>
		public static readonly BgraColor Violet = new BgraColor(238, 130, 238);
		/// <summary>
		/// RGB: 245, 222, 179
		/// </summary>
		public static readonly BgraColor Wheat = new BgraColor(245, 222, 179);
		/// <summary>
		/// RGB: 255, 255, 255
		/// </summary>
		public static readonly BgraColor White = new BgraColor(255, 255, 255);
		/// <summary>
		/// RGB: 245, 245, 245
		/// </summary>
		public static readonly BgraColor WhiteSmoke = new BgraColor(245, 245, 245);
		/// <summary>
		/// RGB: 255, 255, 0
		/// </summary>
		public static readonly BgraColor Yellow = new BgraColor(255, 255, 0);
		/// <summary>
		/// RGB: 154, 205, 50
		/// </summary>
		public static readonly BgraColor YellowGreen = new BgraColor(154, 205, 50);

		/// <summary>
		/// Gets the component specified by the index.
		/// </summary>
		/// <param name="index">0 for B, 1 for G, 2 for R, 3 for A.</param>
		public byte this[int index] {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				switch (index) {
					case 0:
						return B;
					case 1:
						return G;
					case 2:
						return R;
					case 3:
						return A;
					default:
						throw new IndexOutOfRangeException("Component index cannot be smaller than 0 or greater than 3.");
				}
			}
		}

		/// <summary>
		/// Gets the luminance of this color.
		/// </summary>
		public byte Luminance {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return (byte) ((0.299f * R + 0.587f * G + 0.114f * B) * A * 0.00392156863f);
			}
		}

		/// <summary>
		/// Gets the Chrominance U.
		/// </summary>
		public byte ChrominanceU {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return ImageLib.Clamp((127.5f + R * 0.5f - G * 0.418688f - B * 0.081312f) * A * 0.00392156863f);
			}
		}

		/// <summary>
		/// Gets the Chrominance V.
		/// </summary>
		public byte ChrominanceV {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return ImageLib.Clamp((127.5f - R * 0.168736f - G * 0.331264f + B * 0.5f) * A * 0.00392156863f);
			}
		}

		/// <summary>
		/// Constructs a new BgraColor structure from the specified components.
		/// </summary>
		/// <param name="val">The value of R, G and B of the new BgraColor structure (A is 255).</param>
		public BgraColor(byte val) {
			B = val;
			G = val;
			R = val;
			A = 255;
		}

		/// <summary>
		/// Constructs a new BgraColor structure from the specified components.
		/// </summary>
		/// <param name="alpha">The new alpha component.</param>
		/// <param name="val">The value of R, G and B of the new BgraColor structure.</param>
		public BgraColor(byte alpha, byte val) {
			B = val;
			G = val;
			R = val;
			A = alpha;
		}

		/// <summary>
		/// Constructs a new BgraColor structure from the specified components.
		/// </summary>
		/// <param name="a">The alpha component of the new BgraColor structure.</param>
		/// <param name="r">The red component of the new BgraColor structure.</param>
		/// <param name="g">The green component of the new BgraColor structure.</param>
		/// <param name="b">The blue component of the new BgraColor structure.</param>
		public BgraColor(byte a, byte r, byte g, byte b) {
			B = b;
			G = g;
			R = r;
			A = a;
		}

		/// <summary>
		/// Constructs a new BgraColor structure from the specified components with alpha fully opaque.
		/// </summary>
		/// <param name="r">The red component of the new BgraColor structure.</param>
		/// <param name="g">The green component of the new BgraColor structure.</param>
		/// <param name="b">The blue component of the new BgraColor structure.</param>
		public BgraColor(byte r, byte g, byte b) {
			B = b;
			G = g;
			R = r;
			A = 255;
		}

		/// <summary>
		/// Constructs a new BgraColor structure from the specified components.
		/// </summary>
		/// <param name="color">The values to copy into this new BgraColor structure.</param>
		public BgraColor(Color color) {
			B = color.B;
			G = color.G;
			R = color.R;
			A = color.A;
		}

		/// <summary>
		/// Constructs a new BgraColor structure from the specified components, replacing alpha with the given value.
		/// </summary>
		/// <param name="alpha">The new alpha component.</param>
		/// <param name="baseColor">The BGR values to copy into this new BgraColor structure.</param>
		public BgraColor(byte alpha, Color baseColor) {
			B = baseColor.B;
			G = baseColor.G;
			R = baseColor.R;
			A = alpha;
		}

		/// <summary>
		/// Constructs a new BgraColor structure from the specified components, replacing alpha with the given value.
		/// </summary>
		/// <param name="alpha">The new alpha component.</param>
		/// <param name="baseColor">The BGR values to copy into this new BgraColor structure.</param>
		public BgraColor(byte alpha, BgraColor baseColor) {
			B = baseColor.B;
			G = baseColor.G;
			R = baseColor.R;
			A = alpha;
		}

		/// <summary>
		/// Constructs a new BgraColor structure from the specified components.
		/// </summary>
		/// <param name="argb">The ARGB integer that represents the color.</param>
		public BgraColor(int argb) {
			A = (byte) ((argb >> 24) & 0xFF);
			R = (byte) ((argb >> 16) & 0xFF);
			G = (byte) ((argb >> 8) & 0xFF);
			B = (byte) (argb & 0xFF);
		}

		/// <summary>
		/// Converts this color to an ARGB integer representation with 8 bits per channel.
		/// </summary>
		/// <returns>A <see cref="System.Int32"/> that represents this instance.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public int ToArgb() {
			return unchecked((int) ((uint) A << 24 | (uint) R << 16 | (uint) G << 8 | B));
		}

		/// <summary>
		/// Converts this color to an ARGB unsigned integer representation with 8 bits per channel.
		/// </summary>
		/// <returns>A <see cref="System.UInt32"/> that represents this instance.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public uint ToUInt() {
			return (uint) A << 24 | (uint) R << 16 | (uint) G << 8 | B;
		}

		/// <summary>
		/// Gets a BgraColor structure from the specified pointer to a Bgra color.
		/// </summary>
		/// <param name="ptr">The pointer to the structure.</param>
		/// <param name="componentCount">The number of colors represented.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public static unsafe BgraColor FromPointer(byte* ptr, int componentCount) {
			if (componentCount >= 4)
				return *((BgraColor*) ptr);
			else if (componentCount == 3) {
				return new BgraColor() {
					B = *ptr,
					G = *(ptr + 1),
					R = *(ptr + 2),
					A = 255
				};
			} else if (componentCount == 1) {
				byte val = *ptr;
				return new BgraColor() {
					B = val,
					G = val,
					R = val,
					A = 255
				};
			} else if (componentCount == 2) {
				return new BgraColor() {
					B = *ptr,
					G = *(ptr + 1),
					R = 0,
					A = 255
				};
			} else
				return new BgraColor();
		}

		/// <summary>
		/// Gets a BgraColor structure from the specified pointer to a Bgra color.
		/// </summary>
		/// <param name="pointer">The pointer to the structure.</param>
		/// <param name="componentCount">The number of colors represented.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static BgraColor FromPointer(IntPtr pointer, int componentCount) {
			unsafe
			{
				if (componentCount >= 4)
					return *((BgraColor*) pointer);
				else if (componentCount == 3) {
					byte* ptr = (byte*) pointer;
					return new BgraColor() {
						B = *ptr,
						G = *(ptr + 1),
						R = *(ptr + 2),
						A = 255
					};
				} else if (componentCount == 1) {
					byte val = *((byte*) pointer);
					return new BgraColor() {
						B = val,
						G = val,
						R = val,
						A = 255
					};
				} else if (componentCount == 2) {
					byte* ptr = (byte*) pointer;
					return new BgraColor() {
						B = *ptr,
						G = *(ptr + 1),
						R = 0,
						A = 255
					};
				} else
					return new BgraColor();
			}
		}

		/// <summary>
		/// Sets the BgraColor structure pointed to by the pointer to the specified Bgra color.
		/// </summary>
		/// <param name="pointer">The pointer to the structure.</param>
		/// <param name="componentCount">The number of colors represented.</param>
		/// <param name="targetColor">The target color value to set it to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void SetColor(IntPtr pointer, int componentCount, BgraColor targetColor) {
			unsafe
			{
				if (componentCount >= 4)
					*((BgraColor*) pointer) = targetColor;
				else if (componentCount == 3) {
					byte* ptr = (byte*) pointer;
					*ptr = targetColor.B;
					*(ptr + 1) = targetColor.G;
					*(ptr + 2) = targetColor.R;
				} else if (componentCount == 1)
					*((byte*) pointer) = targetColor.B;
				else if (componentCount == 2) {
					byte* ptr = (byte*) pointer;
					*ptr = targetColor.B;
					*(ptr + 1) = targetColor.G;
				}
			}
		}

		/// <summary>
		/// Sets the BgraColor structure pointed to by the pointer to the specified Bgra color.
		/// </summary>
		/// <param name="pointer">The pointer to the structure.</param>
		/// <param name="componentCount">The number of colors represented.</param>
		/// <param name="targetColor">The target color value to set it to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public static void SetColor(IntPtr pointer, int componentCount, ref BgraColor targetColor) {
			unsafe
			{
				if (componentCount >= 4)
					*((BgraColor*) pointer) = targetColor;
				else if (componentCount == 3) {
					byte* ptr = (byte*) pointer;
					*ptr = targetColor.B;
					*(ptr + 1) = targetColor.G;
					*(ptr + 2) = targetColor.R;
				} else if (componentCount == 1)
					*((byte*) pointer) = targetColor.B;
				else if (componentCount == 2) {
					byte* ptr = (byte*) pointer;
					*ptr = targetColor.B;
					*(ptr + 1) = targetColor.G;
				}
			}
		}

		/// <summary>
		/// Sets the BgraColor structure pointed to by the pointer to the specified Bgra color.
		/// </summary>
		/// <param name="pointer">The pointer to the structure.</param>
		/// <param name="componentCount">The number of colors represented.</param>
		/// <param name="targetColor">The target color value to set it to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public static unsafe void SetColor(byte* pointer, int componentCount, BgraColor targetColor) {
			if (componentCount >= 4)
				*((BgraColor*) pointer) = targetColor;
			else if (componentCount == 3) {
				*pointer = targetColor.B;
				*(pointer + 1) = targetColor.G;
				*(pointer + 2) = targetColor.R;
			} else if (componentCount == 1)
				*pointer = targetColor.B;
			else if (componentCount == 2) {
				*pointer = targetColor.B;
				*(pointer + 1) = targetColor.G;
			}
		}

		/// <summary>
		/// Sets the BgraColor structure pointed to by the pointer to the specified Bgra color.
		/// </summary>
		/// <param name="pointer">The pointer to the structure.</param>
		/// <param name="componentCount">The number of colors represented.</param>
		/// <param name="targetColor">The target color value to set it to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public static unsafe void SetColor(byte* pointer, int componentCount, ref BgraColor targetColor) {
			if (componentCount >= 4)
				*((BgraColor*) pointer) = targetColor;
			else if (componentCount == 3) {
				*pointer = targetColor.B;
				*(pointer + 1) = targetColor.G;
				*(pointer + 2) = targetColor.R;
			} else if (componentCount == 1)
				*pointer = targetColor.B;
			else if (componentCount == 2) {
				*pointer = targetColor.B;
				*(pointer + 1) = targetColor.G;
			}
		}

		/// <summary>
		/// Sets the BgraColor structure pointed to by the pointer to the specified Bgra color.
		/// </summary>
		/// <param name="ptr">The pointer to the structure.</param>
		/// <param name="componentCount">The number of colors represented.</param>
		/// <param name="targetPtr">A pointer to the target color value to set it to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		[CLSCompliant(false)]
		public static unsafe void SetColor(byte* ptr, int componentCount, byte* targetPtr) {
			if (componentCount >= 4)
				*((BgraColor*) ptr) = *((BgraColor*) targetPtr);
			else if (componentCount == 3) {
				*ptr = *targetPtr;
				*(ptr + 1) = *(targetPtr + 1);
				*(ptr + 2) = *(targetPtr + 2);
			} else if (componentCount == 1)
				*ptr = *targetPtr;
			else if (componentCount == 2) {
				*ptr = *targetPtr;
				*(ptr + 1) = *(targetPtr + 1);
			}
		}

		/// <summary>
		/// Sets the BgraColor structure pointed to by the pointer to the specified Bgra color.
		/// </summary>
		/// <param name="pointer">The pointer to the structure.</param>
		/// <param name="componentCount">The number of colors represented.</param>
		/// <param name="targetColor">A pointer to the target color value to set it to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void SetColor(IntPtr pointer, int componentCount, IntPtr targetColor) {
			unsafe
			{
				if (componentCount >= 4)
					*((BgraColor*) pointer) = *((BgraColor*) targetColor);
				else if (componentCount == 3) {
					byte* ptr = (byte*) pointer;
					byte* targetPtr = (byte*) targetColor;
					*ptr = *targetPtr;
					*(ptr + 1) = *(targetPtr + 1);
					*(ptr + 2) = *(targetPtr + 2);
				} else if (componentCount == 1)
					*((byte*) pointer) = *((byte*) targetColor);
				else if (componentCount == 2) {
					byte* ptr = (byte*) pointer;
					byte* targetPtr = (byte*) targetColor;
					*ptr = *targetPtr;
					*(ptr + 1) = *(targetPtr + 1);
				}
			}
		}

		/// <summary>
		/// Compares the specified BgraColor structures for equality.
		/// </summary>
		/// <param name="left">The left-hand side of the comparison.</param>
		/// <param name="right">The right-hand side of the comparison.</param>
		/// <returns>True if left is equal to right; false otherwise.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator ==(BgraColor left, BgraColor right) {
			return left.Equals(right);
		}

		/// <summary>
		/// Compares the specified BgraColor structures for inequality.
		/// </summary>
		/// <param name="left">The left-hand side of the comparison.</param>
		/// <param name="right">The right-hand side of the comparison.</param>
		/// <returns>True if left is not equal to right; false otherwise.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator !=(BgraColor left, BgraColor right) {
			return !left.Equals(right);
		}

		/// <summary>
		/// Converts the specified System.Drawing.Color to a BgraColor structure.
		/// </summary>
		/// <param name="color">The System.Drawing.Color to convert.</param>
		/// <returns>A new BgraColor structure containing the converted components.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static explicit operator BgraColor(Color color) {
			return new BgraColor(color.A, color.R, color.G, color.B);
		}

		/// <summary>
		/// Converts the specified BgraColor to a System.Drawing.Color structure.
		/// </summary>
		/// <param name="color">The BgraColor to convert.</param>
		/// <returns>A new System.Drawing.Color structure containing the converted components.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static explicit operator Color(BgraColor color) {
			return Color.FromArgb(color.A, color.R, color.G, color.B);
		}

		/// <summary>
		/// Compares whether this BgraColor structure is equal to the specified object.
		/// </summary>
		/// <param name="obj">An object to compare to.</param>
		/// <returns>True obj is a BgraColor structure with the same components as this BgraColor; false otherwise.</returns>
		public override bool Equals(object obj) {
			if (obj is BgraColor)
				return Equals((BgraColor) obj);
			else
				return false;
		}

		/// <summary>
		/// Calculates the hash code for this BgraColor structure.
		/// </summary>
		/// <returns>A System.Int32 containing the hashcode of this BgraColor structure.</returns>
		public override int GetHashCode() {
			return ToArgb();
		}

		/// <summary>
		/// Creates a System.String that describes this BgraColor structure.
		/// </summary>
		/// <returns>A System.String that describes this BgraColor structure.</returns>
		public override string ToString() {
			return string.Format("(A: {0}, R: {1}, G: {2}, B: {3})", A.ToString(), R.ToString(), G.ToString(), B.ToString());
		}

		/// <summary>
		/// Compares whether this BgraColor structure is equal to the specified BgraColor.
		/// </summary>
		/// <param name="other">The BgraColor structure to compare to.</param>
		/// <returns>True if both BgraColor structures contain the same components; false otherwise.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool Equals(BgraColor other) {
			return R == other.R && G == other.G && B == other.B && A == other.A;
		}
	}
}