using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Drawing {
	/// <summary>
	/// Represents a color with 4 floating-point components (R, G, B, A).
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct ColorF : IEquatable<ColorF> {
		/// <summary>
		/// The RGBA components of the ColorF struct, use for vectorized calculations.
		/// </summary>
		public Vector4 Components;

		/// <summary>
		/// Empty transparent color (ARGB: 0, 0, 0, 0)
		/// </summary>
		public static readonly ColorF Empty = new ColorF();
		/// <summary>
		/// Transparent color (ARGB: 0, 255, 255, 255)
		/// </summary>
		public static readonly ColorF Transparent = new ColorF(0, 255, 255, 255);

		/// <summary>
		/// RGB: 240, 248, 255
		/// </summary>
		public static readonly ColorF AliceBlue = new ColorF(240, 248, 255);
		/// <summary>
		/// RGB: 250, 235, 215
		/// </summary>
		public static readonly ColorF AntiqueWhite = new ColorF(250, 235, 215);
		/// <summary>
		/// RGB: 127, 255, 212
		/// </summary>
		public static readonly ColorF AquaMarine = new ColorF(127, 255, 212);
		/// <summary>
		/// RGB: 240, 255, 255
		/// </summary>
		public static readonly ColorF Azure = new ColorF(240, 255, 255);
		/// <summary>
		/// RGB: 245, 245, 220
		/// </summary>
		public static readonly ColorF Beige = new ColorF(245, 245, 220);
		/// <summary>
		/// RGB: 255, 228, 196
		/// </summary>
		public static readonly ColorF Bisque = new ColorF(255, 228, 196);
		/// <summary>
		/// RGB: 0, 0, 0
		/// </summary>
		public static readonly ColorF Black = new ColorF(0, 0, 0);
		/// <summary>
		/// RGB: 255, 235, 205
		/// </summary>
		public static readonly ColorF BlanchedAlmond = new ColorF(255, 235, 205);
		/// <summary>
		/// RGB: 0, 0, 255
		/// </summary>
		public static readonly ColorF Blue = new ColorF(0, 0, 255);
		/// <summary>
		/// RGB: 138, 43, 226
		/// </summary>
		public static readonly ColorF BlueViolet = new ColorF(138, 43, 226);
		/// <summary>
		/// RGB: 165, 42, 42
		/// </summary>
		public static readonly ColorF Brown = new ColorF(165, 42, 42);
		/// <summary>
		/// RGB: 222, 184, 135
		/// </summary>
		public static readonly ColorF BurlyWood = new ColorF(222, 184, 135);
		/// <summary>
		/// RGB: 95, 158, 160
		/// </summary>
		public static readonly ColorF CadetBlue = new ColorF(95, 158, 160);
		/// <summary>
		/// RGB: 127, 255, 0
		/// </summary>
		public static readonly ColorF Chartreuse = new ColorF(127, 255, 0);
		/// <summary>
		/// RGB: 210, 105, 30
		/// </summary>
		public static readonly ColorF Chocolate = new ColorF(210, 105, 30);
		/// <summary>
		/// RGB: 255, 127, 80
		/// </summary>
		public static readonly ColorF Coral = new ColorF(255, 127, 80);
		/// <summary>
		/// RGB: 100, 149, 237
		/// </summary>
		public static readonly ColorF CornFlowerBlue = new ColorF(100, 149, 237);
		/// <summary>
		/// RGB: 255, 248, 220
		/// </summary>
		public static readonly ColorF Cornsilk = new ColorF(255, 248, 220);
		/// <summary>
		/// RGB: 220, 20, 60
		/// </summary>
		public static readonly ColorF Crimson = new ColorF(220, 20, 60);
		/// <summary>
		/// RGB: 0, 255, 255
		/// </summary>
		public static readonly ColorF Cyan = new ColorF(0, 255, 255);
		/// <summary>
		/// RGB: 0, 0, 139
		/// </summary>
		public static readonly ColorF DarkBlue = new ColorF(0, 0, 139);
		/// <summary>
		/// RGB: 0, 139, 139
		/// </summary>
		public static readonly ColorF DarkCyan = new ColorF(0, 139, 139);
		/// <summary>
		/// RGB: 184, 134, 11
		/// </summary>
		public static readonly ColorF DarkGoldenRod = new ColorF(184, 134, 11);
		/// <summary>
		/// RGB: 169, 169, 169
		/// </summary>
		public static readonly ColorF DarkGray = new ColorF(169, 169, 169);
		/// <summary>
		/// RGB: 0, 100, 0
		/// </summary>
		public static readonly ColorF DarkGreen = new ColorF(0, 100, 0);
		/// <summary>
		/// RGB: 189, 183, 107
		/// </summary>
		public static readonly ColorF DarkKhaki = new ColorF(189, 183, 107);
		/// <summary>
		/// RGB: 139, 0, 139
		/// </summary>
		public static readonly ColorF DarkMagenta = new ColorF(139, 0, 139);
		/// <summary>
		/// RGB: 85, 107, 47
		/// </summary>
		public static readonly ColorF DarkOliveGreen = new ColorF(85, 107, 47);
		/// <summary>
		/// RGB: 255, 140, 0
		/// </summary>
		public static readonly ColorF DarkOrange = new ColorF(255, 140, 0);
		/// <summary>
		/// RGB: 153, 50, 204
		/// </summary>
		public static readonly ColorF DarkOrchid = new ColorF(153, 50, 204);
		/// <summary>
		/// RGB: 139, 0, 0
		/// </summary>
		public static readonly ColorF DarkRed = new ColorF(139, 0, 0);
		/// <summary>
		/// RGB: 233, 150, 122
		/// </summary>
		public static readonly ColorF DarkSalmon = new ColorF(233, 150, 122);
		/// <summary>
		/// RGB: 143, 188, 139
		/// </summary>
		public static readonly ColorF DarkSeaGreen = new ColorF(143, 188, 139);
		/// <summary>
		/// RGB: 72, 61, 139
		/// </summary>
		public static readonly ColorF DarkSlateBlue = new ColorF(72, 61, 139);
		/// <summary>
		/// RGB: 47, 79, 79
		/// </summary>
		public static readonly ColorF DarkSlateGray = new ColorF(47, 79, 79);
		/// <summary>
		/// RGB: 0, 206, 209
		/// </summary>
		public static readonly ColorF DarkTurquoise = new ColorF(0, 206, 209);
		/// <summary>
		/// RGB: 148, 0, 211
		/// </summary>
		public static readonly ColorF DarkViolet = new ColorF(148, 0, 211);
		/// <summary>
		/// RGB: 255, 20, 147
		/// </summary>
		public static readonly ColorF DeepPink = new ColorF(255, 20, 147);
		/// <summary>
		/// RGB: 0, 191, 255
		/// </summary>
		public static readonly ColorF DeepSkyBlue = new ColorF(0, 191, 255);
		/// <summary>
		/// RGB: 105, 105, 105
		/// </summary>
		public static readonly ColorF DimGray = new ColorF(105, 105, 105);
		/// <summary>
		/// RGB: 30, 144, 255
		/// </summary>
		public static readonly ColorF DodgerBlue = new ColorF(30, 144, 255);
		/// <summary>
		/// RGB: 178, 34, 34
		/// </summary>
		public static readonly ColorF FireBrick = new ColorF(178, 34, 34);
		/// <summary>
		/// RGB: 255, 250, 240
		/// </summary>
		public static readonly ColorF FloralWhite = new ColorF(255, 250, 240);
		/// <summary>
		/// RGB: 34, 139, 34
		/// </summary>
		public static readonly ColorF ForestGreen = new ColorF(34, 139, 34);
		/// <summary>
		/// RGB: 255, 0, 255
		/// </summary>
		public static readonly ColorF Fuchsia = new ColorF(255, 0, 255);
		/// <summary>
		/// RGB: 220, 220, 220
		/// </summary>
		public static readonly ColorF Gainsboro = new ColorF(220, 220, 220);
		/// <summary>
		/// RGB: 248, 248, 255
		/// </summary>
		public static readonly ColorF GhostWhite = new ColorF(248, 248, 255);
		/// <summary>
		/// RGB: 255, 215, 0
		/// </summary>
		public static readonly ColorF Gold = new ColorF(255, 215, 0);
		/// <summary>
		/// RGB: 218, 165, 32
		/// </summary>
		public static readonly ColorF GoldenRod = new ColorF(218, 165, 32);
		/// <summary>
		/// RGB: 128, 128, 128
		/// </summary>
		public static readonly ColorF Gray = new ColorF(128, 128, 128);
		/// <summary>
		/// RGB: 0, 128, 0
		/// </summary>
		public static readonly ColorF Green = new ColorF(0, 128, 0);
		/// <summary>
		/// RGB: 173, 255, 47
		/// </summary>
		public static readonly ColorF GreenYellow = new ColorF(173, 255, 47);
		/// <summary>
		/// RGB: 240, 255, 240
		/// </summary>
		public static readonly ColorF HoneyDew = new ColorF(240, 255, 240);
		/// <summary>
		/// RGB: 255, 105, 180
		/// </summary>
		public static readonly ColorF HotPink = new ColorF(255, 105, 180);
		/// <summary>
		/// RGB: 205, 92, 92
		/// </summary>
		public static readonly ColorF IndianRed = new ColorF(205, 92, 92);
		/// <summary>
		/// RGB: 75, 0, 130
		/// </summary>
		public static readonly ColorF Indigo = new ColorF(75, 0, 130);
		/// <summary>
		/// RGB: 255, 255, 240
		/// </summary>
		public static readonly ColorF Ivory = new ColorF(255, 255, 240);
		/// <summary>
		/// RGB: 240, 230, 140
		/// </summary>
		public static readonly ColorF Khaki = new ColorF(240, 230, 140);
		/// <summary>
		/// RGB: 230, 230, 250
		/// </summary>
		public static readonly ColorF Lavender = new ColorF(230, 230, 250);
		/// <summary>
		/// RGB: 255, 240, 245
		/// </summary>
		public static readonly ColorF LavenderBlush = new ColorF(255, 240, 245);
		/// <summary>
		/// RGB: 124, 252, 0
		/// </summary>
		public static readonly ColorF LawnGreen = new ColorF(124, 252, 0);
		/// <summary>
		/// RGB: 255, 250, 205
		/// </summary>
		public static readonly ColorF LemonChiffon = new ColorF(255, 250, 205);
		/// <summary>
		/// RGB: 173, 216, 230
		/// </summary>
		public static readonly ColorF LightBlue = new ColorF(173, 216, 230);
		/// <summary>
		/// RGB: 240, 128, 128
		/// </summary>
		public static readonly ColorF LightCoral = new ColorF(240, 128, 128);
		/// <summary>
		/// RGB: 224, 255, 255
		/// </summary>
		public static readonly ColorF LightCyan = new ColorF(224, 255, 255);
		/// <summary>
		/// RGB: 250, 250, 210
		/// </summary>
		public static readonly ColorF LightGoldenRodYellow = new ColorF(250, 250, 210);
		/// <summary>
		/// RGB: 211, 211, 211
		/// </summary>
		public static readonly ColorF LightGray = new ColorF(211, 211, 211);
		/// <summary>
		/// RGB: 144, 238, 144
		/// </summary>
		public static readonly ColorF LightGreen = new ColorF(144, 238, 144);
		/// <summary>
		/// RGB: 255, 182, 193
		/// </summary>
		public static readonly ColorF LightPink = new ColorF(255, 182, 193);
		/// <summary>
		/// RGB: 255, 160, 122
		/// </summary>
		public static readonly ColorF LightSalmon = new ColorF(255, 160, 122);
		/// <summary>
		/// RGB: 32, 178, 170
		/// </summary>
		public static readonly ColorF LightSeaGreen = new ColorF(32, 178, 170);
		/// <summary>
		/// RGB: 135, 206, 250
		/// </summary>
		public static readonly ColorF LightSkyBlue = new ColorF(135, 206, 250);
		/// <summary>
		/// RGB: 119, 136, 153
		/// </summary>
		public static readonly ColorF LightSlateGray = new ColorF(119, 136, 153);
		/// <summary>
		/// RGB: 176, 196, 222
		/// </summary>
		public static readonly ColorF LightSteelBlue = new ColorF(176, 196, 222);
		/// <summary>
		/// RGB: 255, 255, 224
		/// </summary>
		public static readonly ColorF LightYellow = new ColorF(255, 255, 224);
		/// <summary>
		/// RGB: 0, 255, 0
		/// </summary>
		public static readonly ColorF Lime = new ColorF(0, 255, 0);
		/// <summary>
		/// RGB: 50, 205, 50
		/// </summary>
		public static readonly ColorF LimeGreen = new ColorF(50, 205, 50);
		/// <summary>
		/// RGB: 250, 240, 230
		/// </summary>
		public static readonly ColorF Linen = new ColorF(250, 240, 230);
		/// <summary>
		/// RGB: 255, 0, 255
		/// </summary>
		public static readonly ColorF Magenta = new ColorF(255, 0, 255);
		/// <summary>
		/// RGB: 128, 0, 0
		/// </summary>
		public static readonly ColorF Maroon = new ColorF(128, 0, 0);
		/// <summary>
		/// RGB: 102, 205, 170
		/// </summary>
		public static readonly ColorF MediumAquaMarine = new ColorF(102, 205, 170);
		/// <summary>
		/// RGB: 0, 0, 205
		/// </summary>
		public static readonly ColorF MediumBlue = new ColorF(0, 0, 205);
		/// <summary>
		/// RGB: 186, 85, 211
		/// </summary>
		public static readonly ColorF MediumOrchid = new ColorF(186, 85, 211);
		/// <summary>
		/// RGB: 147, 112, 219
		/// </summary>
		public static readonly ColorF MediumPurple = new ColorF(147, 112, 219);
		/// <summary>
		/// RGB: 60, 179, 113
		/// </summary>
		public static readonly ColorF MediumSeaGreen = new ColorF(60, 179, 113);
		/// <summary>
		/// RGB: 123, 104, 238
		/// </summary>
		public static readonly ColorF MediumSlateBlue = new ColorF(123, 104, 238);
		/// <summary>
		/// RGB: 0, 250, 154
		/// </summary>
		public static readonly ColorF MediumSpringGreen = new ColorF(0, 250, 154);
		/// <summary>
		/// RGB: 72, 209, 204
		/// </summary>
		public static readonly ColorF MediumTurquoise = new ColorF(72, 209, 204);
		/// <summary>
		/// RGB: 199, 21, 133
		/// </summary>
		public static readonly ColorF MediumVioletRed = new ColorF(199, 21, 133);
		/// <summary>
		/// RGB: 25, 25, 112
		/// </summary>
		public static readonly ColorF MidnightBlue = new ColorF(25, 25, 112);
		/// <summary>
		/// RGB: 245, 255, 250
		/// </summary>
		public static readonly ColorF MintCream = new ColorF(245, 255, 250);
		/// <summary>
		/// RGB: 255, 228, 225
		/// </summary>
		public static readonly ColorF MistyRose = new ColorF(255, 228, 225);
		/// <summary>
		/// RGB: 255, 228, 181
		/// </summary>
		public static readonly ColorF Moccasin = new ColorF(255, 228, 181);
		/// <summary>
		/// RGB: 255, 222, 173
		/// </summary>
		public static readonly ColorF NavajoWhite = new ColorF(255, 222, 173);
		/// <summary>
		/// RGB: 0, 0, 128
		/// </summary>
		public static readonly ColorF NavyBlue = new ColorF(0, 0, 128);
		/// <summary>
		/// RGB: 253, 245, 230
		/// </summary>
		public static readonly ColorF OldLace = new ColorF(253, 245, 230);
		/// <summary>
		/// RGB: 128, 128, 0
		/// </summary>
		public static readonly ColorF Olive = new ColorF(128, 128, 0);
		/// <summary>
		/// RGB: 107, 142, 35
		/// </summary>
		public static readonly ColorF OliveDrab = new ColorF(107, 142, 35);
		/// <summary>
		/// RGB: 255, 165, 0
		/// </summary>
		public static readonly ColorF Orange = new ColorF(255, 165, 0);
		/// <summary>
		/// RGB: 255, 69, 0
		/// </summary>
		public static readonly ColorF OrangeRed = new ColorF(255, 69, 0);
		/// <summary>
		/// RGB: 218, 112, 214
		/// </summary>
		public static readonly ColorF Orchid = new ColorF(218, 112, 214);
		/// <summary>
		/// RGB: 250, 240, 230
		/// </summary>
		public static readonly ColorF PaleGoldenRod = new ColorF(238, 232, 170);
		/// <summary>
		/// RGB: 152, 251, 152
		/// </summary>
		public static readonly ColorF PaleGreen = new ColorF(152, 251, 152);
		/// <summary>
		/// RGB: 175, 238, 238
		/// </summary>
		public static readonly ColorF PaleTurquoise = new ColorF(175, 238, 238);
		/// <summary>
		/// RGB: 219, 112, 147
		/// </summary>
		public static readonly ColorF PaleVioletRed = new ColorF(219, 112, 147);
		/// <summary>
		/// RGB: 255, 239, 213
		/// </summary>
		public static readonly ColorF PapayaWhip = new ColorF(255, 239, 213);
		/// <summary>
		/// RGB: 255, 218, 185
		/// </summary>
		public static readonly ColorF PeachPuff = new ColorF(255, 218, 185);
		/// <summary>
		/// RGB: 205, 133, 65
		/// </summary>
		public static readonly ColorF Peru = new ColorF(205, 133, 65);
		/// <summary>
		/// RGB: 255, 192, 203
		/// </summary>
		public static readonly ColorF Pink = new ColorF(255, 192, 203);
		/// <summary>
		/// RGB: 221, 160, 221
		/// </summary>
		public static readonly ColorF Plum = new ColorF(221, 160, 221);
		/// <summary>
		/// RGB: 176, 224, 230
		/// </summary>
		public static readonly ColorF PowderBlue = new ColorF(176, 224, 230);
		/// <summary>
		/// RGB: 128, 0, 128
		/// </summary>
		public static readonly ColorF Purple = new ColorF(128, 0, 128);
		/// <summary>
		/// RGB: 255, 0, 0
		/// </summary>
		public static readonly ColorF Red = new ColorF(255, 0, 0);
		/// <summary>
		/// RGB: 188, 143, 143
		/// </summary>
		public static readonly ColorF RosyBrown = new ColorF(188, 143, 143);
		/// <summary>
		/// RGB: 65, 105, 225
		/// </summary>
		public static readonly ColorF RoyalBlue = new ColorF(65, 105, 225);
		/// <summary>
		/// RGB: 139, 69, 19
		/// </summary>
		public static readonly ColorF SaddleBrown = new ColorF(139, 69, 19);
		/// <summary>
		/// RGB: 250, 128, 114
		/// </summary>
		public static readonly ColorF Salmon = new ColorF(250, 128, 114);
		/// <summary>
		/// RGB: 244, 164, 96
		/// </summary>
		public static readonly ColorF SandyBrown = new ColorF(244, 164, 96);
		/// <summary>
		/// RGB: 46, 139, 87
		/// </summary>
		public static readonly ColorF SeaGreen = new ColorF(46, 139, 87);
		/// <summary>
		/// RGB: 255, 245, 238
		/// </summary>
		public static readonly ColorF SeaShell = new ColorF(255, 245, 238);
		/// <summary>
		/// RGB: 160, 82, 45
		/// </summary>
		public static readonly ColorF Sienna = new ColorF(160, 82, 45);
		/// <summary>
		/// RGB: 192, 192, 192
		/// </summary>
		public static readonly ColorF Silver = new ColorF(192, 192, 192);
		/// <summary>
		/// RGB: 135, 206, 235
		/// </summary>
		public static readonly ColorF SkyBlue = new ColorF(135, 206, 235);
		/// <summary>
		/// RGB: 106, 90, 205
		/// </summary>
		public static readonly ColorF SlateBlue = new ColorF(106, 90, 205);
		/// <summary>
		/// RGB: 112, 128, 144
		/// </summary>
		public static readonly ColorF SlateGray = new ColorF(112, 128, 144);
		/// <summary>
		/// RGB: 255, 250, 250
		/// </summary>
		public static readonly ColorF Snow = new ColorF(255, 250, 250);
		/// <summary>
		/// RGB: 0, 255, 127
		/// </summary>
		public static readonly ColorF SpringGreen = new ColorF(0, 255, 127);
		/// <summary>
		/// RGB: 70, 130, 180
		/// </summary>
		public static readonly ColorF SteelBlue = new ColorF(70, 130, 180);
		/// <summary>
		/// RGB: 210, 180, 140
		/// </summary>
		public static readonly ColorF Tan = new ColorF(210, 180, 140);
		/// <summary>
		/// RGB: 0, 128, 128
		/// </summary>
		public static readonly ColorF Teal = new ColorF(0, 128, 128);
		/// <summary>
		/// RGB: 216, 191, 216
		/// </summary>
		public static readonly ColorF Thistle = new ColorF(216, 191, 216);
		/// <summary>
		/// RGB: 255, 99, 71
		/// </summary>
		public static readonly ColorF Tomato = new ColorF(255, 99, 71);
		/// <summary>
		/// RGB: 64, 224, 208
		/// </summary>
		public static readonly ColorF Turquoise = new ColorF(64, 224, 208);
		/// <summary>
		/// RGB: 238, 130, 238
		/// </summary>
		public static readonly ColorF Violet = new ColorF(238, 130, 238);
		/// <summary>
		/// RGB: 245, 222, 179
		/// </summary>
		public static readonly ColorF Wheat = new ColorF(245, 222, 179);
		/// <summary>
		/// RGB: 255, 255, 255
		/// </summary>
		public static readonly ColorF White = new ColorF(255, 255, 255);
		/// <summary>
		/// RGB: 245, 245, 245
		/// </summary>
		public static readonly ColorF WhiteSmoke = new ColorF(245, 245, 245);
		/// <summary>
		/// RGB: 255, 255, 0
		/// </summary>
		public static readonly ColorF Yellow = new ColorF(255, 255, 0);
		/// <summary>
		/// RGB: 154, 205, 50
		/// </summary>
		public static readonly ColorF YellowGreen = new ColorF(154, 205, 50);

		/// <summary>
		/// Gets the component specified by the index.
		/// </summary>
		/// <param name="index">0 for R, 1 for G, 2 for B, 3 for A.</param>
		public float this[int index] {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				switch (index) {
					case 0:
						return Components.X;
					case 1:
						return Components.Y;
					case 2:
						return Components.Z;
					case 3:
						return Components.W;
					default:
						throw new IndexOutOfRangeException("Component index cannot be smaller than 0 or greater than 3.");
				}
			}
		}

		/// <summary>
		/// The red component of this ColorF structure (0 to 1).
		/// </summary>
		public float R {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return Components.X;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				Components.X = value;
			}
		}

		/// <summary>
		/// The green component of this ColorF structure (0 to 1).
		/// </summary>
		public float G {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return Components.Y;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				Components.Y = value;
			}
		}

		/// <summary>
		/// The blue component of this ColorF structure (0 to 1).
		/// </summary>
		public float B {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return Components.Z;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				Components.Z = value;
			}
		}

		/// <summary>
		/// The alpha component of this ColorF structure (0 to 1).
		/// </summary>
		public float A {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return Components.W;
			}
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			set {
				Components.W = value;
			}
		}

		/// <summary>
		/// Constructs a new ColorF structure directly from the specified RGBA components.
		/// </summary>
		public ColorF(Vector4 components) {
			Components = components;
		}

		/// <summary>
		/// Constructs a new ColorF structure from the specified components.
		/// </summary>
		/// <param name="a">The alpha component of the new ColorF structure.</param>
		/// <param name="r">The red component of the new ColorF structure.</param>
		/// <param name="g">The green component of the new ColorF structure.</param>
		/// <param name="b">The blue component of the new ColorF structure.</param>
		public ColorF(float a, float r, float g, float b) {
			Components = new Vector4(Clamp(r), Clamp(g), Clamp(b), Clamp(a));
		}

		/// <summary>
		/// Constructs a new ColorF structure from the specified components.
		/// </summary>
		/// <param name="a">The alpha component of the new ColorF structure.</param>
		/// <param name="r">The red component of the new ColorF structure.</param>
		/// <param name="g">The green component of the new ColorF structure.</param>
		/// <param name="b">The blue component of the new ColorF structure.</param>
		public ColorF(byte a, byte r, byte g, byte b) {
			Components = new Vector4(r, g, b, a) * 0.00392156863f;
		}

		/// <summary>
		/// Constructs a new ColorF structure from the specified components with alpha fully opaque.
		/// </summary>
		/// <param name="r">The red component of the new ColorF structure.</param>
		/// <param name="g">The green component of the new ColorF structure.</param>
		/// <param name="b">The blue component of the new ColorF structure.</param>
		public ColorF(byte r, byte g, byte b) {
			Components = new Vector4(new Vector3(r, g, b) * 0.00392156863f, 1f);
		}

		/// <summary>
		/// Constructs a new ColorF structure from the specified components with alpha fully opaque.
		/// </summary>
		/// <param name="r">The red component of the new ColorF structure.</param>
		/// <param name="g">The green component of the new ColorF structure.</param>
		/// <param name="b">The blue component of the new ColorF structure.</param>
		public ColorF(float r, float g, float b) {
			Components = new Vector4(Clamp(r), Clamp(g), Clamp(b), 1f);
		}

		/// <summary>
		/// Constructs a new ColorF structure from the specified components, replacing alpha with the given value.
		/// </summary>
		/// <param name="alpha">The new alpha component.</param>
		/// <param name="baseColor">The RGB values to copy into this new ColorF structure.</param>
		public ColorF(float alpha, ColorF baseColor) {
			Components = new Vector4(baseColor.R, baseColor.G, baseColor.B, Clamp(alpha));
		}

		/// <summary>
		/// Constructs a new ColorF structure from the specified components.
		/// </summary>
		/// <param name="argb">The ARGB integer that represents the color.</param>
		public ColorF(int argb) {
			Components = new Vector4((byte) ((argb >> 16) & 0xFF), (byte) ((argb >> 8) & 0xFF), (byte) (argb & 0xFF), (byte) ((argb >> 24) & 0xFF));
		}

		/// <summary>
		/// Converts this color to an integer representation with 8 bits per channel.
		/// </summary>
		/// <returns>A <see cref="System.Int32"/> that represents this instance.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public int ToArgb() {
			return unchecked((int) ((uint) (A * 255f) << 24 | (uint) (R * 255f) << 16 | (uint) (G * 255f) << 8 | (uint) (B * 255f)));
		}

		/// <summary>
		/// Compares the specified ColorF structures for equality.
		/// </summary>
		/// <param name="left">The left-hand side of the comparison.</param>
		/// <param name="right">The right-hand side of the comparison.</param>
		/// <returns>True if left is equal to right; false otherwise.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator ==(ColorF left, ColorF right) {
			return left.Equals(right);
		}

		/// <summary>
		/// Compares the specified ColorF structures for inequality.
		/// </summary>
		/// <param name="left">The left-hand side of the comparison.</param>
		/// <param name="right">The right-hand side of the comparison.</param>
		/// <returns>True if left is not equal to right; false otherwise.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator !=(ColorF left, ColorF right) {
			return !left.Equals(right);
		}

		/// <summary>
		/// Converts the specified System.Drawing.Color to a ColorF structure.
		/// </summary>
		/// <param name="color">The System.Drawing.Color to convert.</param>
		/// <returns>A new ColorF structure containing the converted components.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static explicit operator ColorF(Color color) {
			return new ColorF(color.A, color.R, color.G, color.B);
		}

		/// <summary>
		/// Converts the specified ColorF to a System.Drawing.Color structure.
		/// </summary>
		/// <param name="color">The ColorF to convert.</param>
		/// <returns>A new System.Drawing.Color structure containing the converted components.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static explicit operator Color(ColorF color) {
			return Color.FromArgb((int) (color.A * 255f), (int) (color.R * 255f), (int) (color.G * 255f), (int) (color.B * 255f));
		}

		/// <summary>
		/// Converts the specified Vector4 to a ColorF structure.
		/// </summary>
		/// <param name="components">The Vector4 to convert.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static implicit operator ColorF(Vector4 components) {
			return new ColorF() {
				Components = components
			};
		}

		/// <summary>
		/// Converts the specified ColorF to a Vector4 structure.
		/// </summary>
		/// <param name="color">The ColorF to convert.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static implicit operator Vector4(ColorF color) {
			return color.Components;
		}

		/// <summary>
		/// Converts the specified BgraColor to a ColorF structure.
		/// </summary>
		/// <param name="color">The BgraColor to convert.</param>
		/// <returns>A new ColorF structure containing the converted components.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static explicit operator ColorF(BgraColor color) {
			return new ColorF(color.A, color.R, color.G, color.B);
		}

		/// <summary>
		/// Converts the specified ColorF to a BgraColor structure.
		/// </summary>
		/// <param name="color">The ColorF to convert.</param>
		/// <returns>A new BgraColor structure containing the converted components.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static explicit operator BgraColor(ColorF color) {
			return new BgraColor((byte) (color.A * 255f), (byte) (color.R * 255f), (byte) (color.G * 255f), (byte) (color.B * 255f));
		}

		/// <summary>
		/// Clamps the value to the range [0, 1].
		/// </summary>
		/// <param name="value">The value to clamp.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static float Clamp(float value) {
			return value < 1f ? (value > 0f ? value : 0f) : 1f;
		}

		/// <summary>
		/// Compares whether this ColorF structure is equal to the specified object.
		/// </summary>
		/// <param name="obj">An object to compare to.</param>
		/// <returns>True obj is a ColorF structure with the same components as this ColorF; false otherwise.</returns>
		public override bool Equals(object obj) {
			if (obj is ColorF)
				return Equals((ColorF) obj);
			else
				return false;
		}

		/// <summary>
		/// Calculates the hash code for this ColorF structure.
		/// </summary>
		/// <returns>A System.Int32 containing the hashcode of this ColorF structure.</returns>
		public override int GetHashCode() {
			return ToArgb();
		}

		/// <summary>
		/// Creates a System.String that describes this ColorF structure.
		/// </summary>
		/// <returns>A System.String that describes this ColorF structure.</returns>
		public override string ToString() {
			return string.Format("(A: {0}, R: {1}, G: {2}, B: {3})", A.ToString(), R.ToString(), G.ToString(), B.ToString());
		}

		/// <summary>
		/// Compares whether this ColorF structure is equal to the specified ColorF.
		/// </summary>
		/// <param name="other">The ColorF structure to compare to.</param>
		/// <returns>True if both ColorF structures contain the same components; false otherwise.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool Equals(ColorF other) {
			return R == other.R && G == other.G && B == other.B && A == other.A;
		}
	}
}