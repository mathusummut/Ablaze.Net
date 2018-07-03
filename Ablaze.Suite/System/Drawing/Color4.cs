using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Drawing {
	/// <summary>
	/// Represents a color with 4 floating-point components (R, G, B, A).
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct Color4 : IEquatable<Color4> {
		/// <summary>
		/// The RGBA components of the Color4 struct, use for vectorized calculations.
		/// </summary>
		public Vector4 Components;

		/// <summary>
		/// Empty transparent color (ARGB: 0, 0, 0, 0)
		/// </summary>
		public static readonly Color4 Empty = new Color4();
		/// <summary>
		/// Transparent color (ARGB: 0, 255, 255, 255)
		/// </summary>
		public static readonly Color4 Transparent = new Color4(0, 255, 255, 255);

		/// <summary>
		/// RGB: 240, 248, 255
		/// </summary>
		public static readonly Color4 AliceBlue = new Color4(240, 248, 255);
		/// <summary>
		/// RGB: 250, 235, 215
		/// </summary>
		public static readonly Color4 AntiqueWhite = new Color4(250, 235, 215);
		/// <summary>
		/// RGB: 127, 255, 212
		/// </summary>
		public static readonly Color4 AquaMarine = new Color4(127, 255, 212);
		/// <summary>
		/// RGB: 240, 255, 255
		/// </summary>
		public static readonly Color4 Azure = new Color4(240, 255, 255);
		/// <summary>
		/// RGB: 245, 245, 220
		/// </summary>
		public static readonly Color4 Beige = new Color4(245, 245, 220);
		/// <summary>
		/// RGB: 255, 228, 196
		/// </summary>
		public static readonly Color4 Bisque = new Color4(255, 228, 196);
		/// <summary>
		/// RGB: 0, 0, 0
		/// </summary>
		public static readonly Color4 Black = new Color4(0, 0, 0);
		/// <summary>
		/// RGB: 255, 235, 205
		/// </summary>
		public static readonly Color4 BlanchedAlmond = new Color4(255, 235, 205);
		/// <summary>
		/// RGB: 0, 0, 255
		/// </summary>
		public static readonly Color4 Blue = new Color4(0, 0, 255);
		/// <summary>
		/// RGB: 138, 43, 226
		/// </summary>
		public static readonly Color4 BlueViolet = new Color4(138, 43, 226);
		/// <summary>
		/// RGB: 165, 42, 42
		/// </summary>
		public static readonly Color4 Brown = new Color4(165, 42, 42);
		/// <summary>
		/// RGB: 222, 184, 135
		/// </summary>
		public static readonly Color4 BurlyWood = new Color4(222, 184, 135);
		/// <summary>
		/// RGB: 95, 158, 160
		/// </summary>
		public static readonly Color4 CadetBlue = new Color4(95, 158, 160);
		/// <summary>
		/// RGB: 127, 255, 0
		/// </summary>
		public static readonly Color4 Chartreuse = new Color4(127, 255, 0);
		/// <summary>
		/// RGB: 210, 105, 30
		/// </summary>
		public static readonly Color4 Chocolate = new Color4(210, 105, 30);
		/// <summary>
		/// RGB: 255, 127, 80
		/// </summary>
		public static readonly Color4 Coral = new Color4(255, 127, 80);
		/// <summary>
		/// RGB: 100, 149, 237
		/// </summary>
		public static readonly Color4 CornFlowerBlue = new Color4(100, 149, 237);
		/// <summary>
		/// RGB: 255, 248, 220
		/// </summary>
		public static readonly Color4 Cornsilk = new Color4(255, 248, 220);
		/// <summary>
		/// RGB: 220, 20, 60
		/// </summary>
		public static readonly Color4 Crimson = new Color4(220, 20, 60);
		/// <summary>
		/// RGB: 0, 255, 255
		/// </summary>
		public static readonly Color4 Cyan = new Color4(0, 255, 255);
		/// <summary>
		/// RGB: 0, 0, 139
		/// </summary>
		public static readonly Color4 DarkBlue = new Color4(0, 0, 139);
		/// <summary>
		/// RGB: 0, 139, 139
		/// </summary>
		public static readonly Color4 DarkCyan = new Color4(0, 139, 139);
		/// <summary>
		/// RGB: 184, 134, 11
		/// </summary>
		public static readonly Color4 DarkGoldenRod = new Color4(184, 134, 11);
		/// <summary>
		/// RGB: 169, 169, 169
		/// </summary>
		public static readonly Color4 DarkGray = new Color4(169, 169, 169);
		/// <summary>
		/// RGB: 0, 100, 0
		/// </summary>
		public static readonly Color4 DarkGreen = new Color4(0, 100, 0);
		/// <summary>
		/// RGB: 189, 183, 107
		/// </summary>
		public static readonly Color4 DarkKhaki = new Color4(189, 183, 107);
		/// <summary>
		/// RGB: 139, 0, 139
		/// </summary>
		public static readonly Color4 DarkMagenta = new Color4(139, 0, 139);
		/// <summary>
		/// RGB: 85, 107, 47
		/// </summary>
		public static readonly Color4 DarkOliveGreen = new Color4(85, 107, 47);
		/// <summary>
		/// RGB: 255, 140, 0
		/// </summary>
		public static readonly Color4 DarkOrange = new Color4(255, 140, 0);
		/// <summary>
		/// RGB: 153, 50, 204
		/// </summary>
		public static readonly Color4 DarkOrchid = new Color4(153, 50, 204);
		/// <summary>
		/// RGB: 139, 0, 0
		/// </summary>
		public static readonly Color4 DarkRed = new Color4(139, 0, 0);
		/// <summary>
		/// RGB: 233, 150, 122
		/// </summary>
		public static readonly Color4 DarkSalmon = new Color4(233, 150, 122);
		/// <summary>
		/// RGB: 143, 188, 139
		/// </summary>
		public static readonly Color4 DarkSeaGreen = new Color4(143, 188, 139);
		/// <summary>
		/// RGB: 72, 61, 139
		/// </summary>
		public static readonly Color4 DarkSlateBlue = new Color4(72, 61, 139);
		/// <summary>
		/// RGB: 47, 79, 79
		/// </summary>
		public static readonly Color4 DarkSlateGray = new Color4(47, 79, 79);
		/// <summary>
		/// RGB: 0, 206, 209
		/// </summary>
		public static readonly Color4 DarkTurquoise = new Color4(0, 206, 209);
		/// <summary>
		/// RGB: 148, 0, 211
		/// </summary>
		public static readonly Color4 DarkViolet = new Color4(148, 0, 211);
		/// <summary>
		/// RGB: 255, 20, 147
		/// </summary>
		public static readonly Color4 DeepPink = new Color4(255, 20, 147);
		/// <summary>
		/// RGB: 0, 191, 255
		/// </summary>
		public static readonly Color4 DeepSkyBlue = new Color4(0, 191, 255);
		/// <summary>
		/// RGB: 105, 105, 105
		/// </summary>
		public static readonly Color4 DimGray = new Color4(105, 105, 105);
		/// <summary>
		/// RGB: 30, 144, 255
		/// </summary>
		public static readonly Color4 DodgerBlue = new Color4(30, 144, 255);
		/// <summary>
		/// RGB: 178, 34, 34
		/// </summary>
		public static readonly Color4 FireBrick = new Color4(178, 34, 34);
		/// <summary>
		/// RGB: 255, 250, 240
		/// </summary>
		public static readonly Color4 FloralWhite = new Color4(255, 250, 240);
		/// <summary>
		/// RGB: 34, 139, 34
		/// </summary>
		public static readonly Color4 ForestGreen = new Color4(34, 139, 34);
		/// <summary>
		/// RGB: 255, 0, 255
		/// </summary>
		public static readonly Color4 Fuchsia = new Color4(255, 0, 255);
		/// <summary>
		/// RGB: 220, 220, 220
		/// </summary>
		public static readonly Color4 Gainsboro = new Color4(220, 220, 220);
		/// <summary>
		/// RGB: 248, 248, 255
		/// </summary>
		public static readonly Color4 GhostWhite = new Color4(248, 248, 255);
		/// <summary>
		/// RGB: 255, 215, 0
		/// </summary>
		public static readonly Color4 Gold = new Color4(255, 215, 0);
		/// <summary>
		/// RGB: 218, 165, 32
		/// </summary>
		public static readonly Color4 GoldenRod = new Color4(218, 165, 32);
		/// <summary>
		/// RGB: 128, 128, 128
		/// </summary>
		public static readonly Color4 Gray = new Color4(128, 128, 128);
		/// <summary>
		/// RGB: 0, 128, 0
		/// </summary>
		public static readonly Color4 Green = new Color4(0, 128, 0);
		/// <summary>
		/// RGB: 173, 255, 47
		/// </summary>
		public static readonly Color4 GreenYellow = new Color4(173, 255, 47);
		/// <summary>
		/// RGB: 240, 255, 240
		/// </summary>
		public static readonly Color4 HoneyDew = new Color4(240, 255, 240);
		/// <summary>
		/// RGB: 255, 105, 180
		/// </summary>
		public static readonly Color4 HotPink = new Color4(255, 105, 180);
		/// <summary>
		/// RGB: 205, 92, 92
		/// </summary>
		public static readonly Color4 IndianRed = new Color4(205, 92, 92);
		/// <summary>
		/// RGB: 75, 0, 130
		/// </summary>
		public static readonly Color4 Indigo = new Color4(75, 0, 130);
		/// <summary>
		/// RGB: 255, 255, 240
		/// </summary>
		public static readonly Color4 Ivory = new Color4(255, 255, 240);
		/// <summary>
		/// RGB: 240, 230, 140
		/// </summary>
		public static readonly Color4 Khaki = new Color4(240, 230, 140);
		/// <summary>
		/// RGB: 230, 230, 250
		/// </summary>
		public static readonly Color4 Lavender = new Color4(230, 230, 250);
		/// <summary>
		/// RGB: 255, 240, 245
		/// </summary>
		public static readonly Color4 LavenderBlush = new Color4(255, 240, 245);
		/// <summary>
		/// RGB: 124, 252, 0
		/// </summary>
		public static readonly Color4 LawnGreen = new Color4(124, 252, 0);
		/// <summary>
		/// RGB: 255, 250, 205
		/// </summary>
		public static readonly Color4 LemonChiffon = new Color4(255, 250, 205);
		/// <summary>
		/// RGB: 173, 216, 230
		/// </summary>
		public static readonly Color4 LightBlue = new Color4(173, 216, 230);
		/// <summary>
		/// RGB: 240, 128, 128
		/// </summary>
		public static readonly Color4 LightCoral = new Color4(240, 128, 128);
		/// <summary>
		/// RGB: 224, 255, 255
		/// </summary>
		public static readonly Color4 LightCyan = new Color4(224, 255, 255);
		/// <summary>
		/// RGB: 250, 250, 210
		/// </summary>
		public static readonly Color4 LightGoldenRodYellow = new Color4(250, 250, 210);
		/// <summary>
		/// RGB: 211, 211, 211
		/// </summary>
		public static readonly Color4 LightGray = new Color4(211, 211, 211);
		/// <summary>
		/// RGB: 144, 238, 144
		/// </summary>
		public static readonly Color4 LightGreen = new Color4(144, 238, 144);
		/// <summary>
		/// RGB: 255, 182, 193
		/// </summary>
		public static readonly Color4 LightPink = new Color4(255, 182, 193);
		/// <summary>
		/// RGB: 255, 160, 122
		/// </summary>
		public static readonly Color4 LightSalmon = new Color4(255, 160, 122);
		/// <summary>
		/// RGB: 32, 178, 170
		/// </summary>
		public static readonly Color4 LightSeaGreen = new Color4(32, 178, 170);
		/// <summary>
		/// RGB: 135, 206, 250
		/// </summary>
		public static readonly Color4 LightSkyBlue = new Color4(135, 206, 250);
		/// <summary>
		/// RGB: 119, 136, 153
		/// </summary>
		public static readonly Color4 LightSlateGray = new Color4(119, 136, 153);
		/// <summary>
		/// RGB: 176, 196, 222
		/// </summary>
		public static readonly Color4 LightSteelBlue = new Color4(176, 196, 222);
		/// <summary>
		/// RGB: 255, 255, 224
		/// </summary>
		public static readonly Color4 LightYellow = new Color4(255, 255, 224);
		/// <summary>
		/// RGB: 0, 255, 0
		/// </summary>
		public static readonly Color4 Lime = new Color4(0, 255, 0);
		/// <summary>
		/// RGB: 50, 205, 50
		/// </summary>
		public static readonly Color4 LimeGreen = new Color4(50, 205, 50);
		/// <summary>
		/// RGB: 250, 240, 230
		/// </summary>
		public static readonly Color4 Linen = new Color4(250, 240, 230);
		/// <summary>
		/// RGB: 255, 0, 255
		/// </summary>
		public static readonly Color4 Magenta = new Color4(255, 0, 255);
		/// <summary>
		/// RGB: 128, 0, 0
		/// </summary>
		public static readonly Color4 Maroon = new Color4(128, 0, 0);
		/// <summary>
		/// RGB: 102, 205, 170
		/// </summary>
		public static readonly Color4 MediumAquaMarine = new Color4(102, 205, 170);
		/// <summary>
		/// RGB: 0, 0, 205
		/// </summary>
		public static readonly Color4 MediumBlue = new Color4(0, 0, 205);
		/// <summary>
		/// RGB: 186, 85, 211
		/// </summary>
		public static readonly Color4 MediumOrchid = new Color4(186, 85, 211);
		/// <summary>
		/// RGB: 147, 112, 219
		/// </summary>
		public static readonly Color4 MediumPurple = new Color4(147, 112, 219);
		/// <summary>
		/// RGB: 60, 179, 113
		/// </summary>
		public static readonly Color4 MediumSeaGreen = new Color4(60, 179, 113);
		/// <summary>
		/// RGB: 123, 104, 238
		/// </summary>
		public static readonly Color4 MediumSlateBlue = new Color4(123, 104, 238);
		/// <summary>
		/// RGB: 0, 250, 154
		/// </summary>
		public static readonly Color4 MediumSpringGreen = new Color4(0, 250, 154);
		/// <summary>
		/// RGB: 72, 209, 204
		/// </summary>
		public static readonly Color4 MediumTurquoise = new Color4(72, 209, 204);
		/// <summary>
		/// RGB: 199, 21, 133
		/// </summary>
		public static readonly Color4 MediumVioletRed = new Color4(199, 21, 133);
		/// <summary>
		/// RGB: 25, 25, 112
		/// </summary>
		public static readonly Color4 MidnightBlue = new Color4(25, 25, 112);
		/// <summary>
		/// RGB: 245, 255, 250
		/// </summary>
		public static readonly Color4 MintCream = new Color4(245, 255, 250);
		/// <summary>
		/// RGB: 255, 228, 225
		/// </summary>
		public static readonly Color4 MistyRose = new Color4(255, 228, 225);
		/// <summary>
		/// RGB: 255, 228, 181
		/// </summary>
		public static readonly Color4 Moccasin = new Color4(255, 228, 181);
		/// <summary>
		/// RGB: 255, 222, 173
		/// </summary>
		public static readonly Color4 NavajoWhite = new Color4(255, 222, 173);
		/// <summary>
		/// RGB: 0, 0, 128
		/// </summary>
		public static readonly Color4 NavyBlue = new Color4(0, 0, 128);
		/// <summary>
		/// RGB: 253, 245, 230
		/// </summary>
		public static readonly Color4 OldLace = new Color4(253, 245, 230);
		/// <summary>
		/// RGB: 128, 128, 0
		/// </summary>
		public static readonly Color4 Olive = new Color4(128, 128, 0);
		/// <summary>
		/// RGB: 107, 142, 35
		/// </summary>
		public static readonly Color4 OliveDrab = new Color4(107, 142, 35);
		/// <summary>
		/// RGB: 255, 165, 0
		/// </summary>
		public static readonly Color4 Orange = new Color4(255, 165, 0);
		/// <summary>
		/// RGB: 255, 69, 0
		/// </summary>
		public static readonly Color4 OrangeRed = new Color4(255, 69, 0);
		/// <summary>
		/// RGB: 218, 112, 214
		/// </summary>
		public static readonly Color4 Orchid = new Color4(218, 112, 214);
		/// <summary>
		/// RGB: 250, 240, 230
		/// </summary>
		public static readonly Color4 PaleGoldenRod = new Color4(238, 232, 170);
		/// <summary>
		/// RGB: 152, 251, 152
		/// </summary>
		public static readonly Color4 PaleGreen = new Color4(152, 251, 152);
		/// <summary>
		/// RGB: 175, 238, 238
		/// </summary>
		public static readonly Color4 PaleTurquoise = new Color4(175, 238, 238);
		/// <summary>
		/// RGB: 219, 112, 147
		/// </summary>
		public static readonly Color4 PaleVioletRed = new Color4(219, 112, 147);
		/// <summary>
		/// RGB: 255, 239, 213
		/// </summary>
		public static readonly Color4 PapayaWhip = new Color4(255, 239, 213);
		/// <summary>
		/// RGB: 255, 218, 185
		/// </summary>
		public static readonly Color4 PeachPuff = new Color4(255, 218, 185);
		/// <summary>
		/// RGB: 205, 133, 65
		/// </summary>
		public static readonly Color4 Peru = new Color4(205, 133, 65);
		/// <summary>
		/// RGB: 255, 192, 203
		/// </summary>
		public static readonly Color4 Pink = new Color4(255, 192, 203);
		/// <summary>
		/// RGB: 221, 160, 221
		/// </summary>
		public static readonly Color4 Plum = new Color4(221, 160, 221);
		/// <summary>
		/// RGB: 176, 224, 230
		/// </summary>
		public static readonly Color4 PowderBlue = new Color4(176, 224, 230);
		/// <summary>
		/// RGB: 128, 0, 128
		/// </summary>
		public static readonly Color4 Purple = new Color4(128, 0, 128);
		/// <summary>
		/// RGB: 255, 0, 0
		/// </summary>
		public static readonly Color4 Red = new Color4(255, 0, 0);
		/// <summary>
		/// RGB: 188, 143, 143
		/// </summary>
		public static readonly Color4 RosyBrown = new Color4(188, 143, 143);
		/// <summary>
		/// RGB: 65, 105, 225
		/// </summary>
		public static readonly Color4 RoyalBlue = new Color4(65, 105, 225);
		/// <summary>
		/// RGB: 139, 69, 19
		/// </summary>
		public static readonly Color4 SaddleBrown = new Color4(139, 69, 19);
		/// <summary>
		/// RGB: 250, 128, 114
		/// </summary>
		public static readonly Color4 Salmon = new Color4(250, 128, 114);
		/// <summary>
		/// RGB: 244, 164, 96
		/// </summary>
		public static readonly Color4 SandyBrown = new Color4(244, 164, 96);
		/// <summary>
		/// RGB: 46, 139, 87
		/// </summary>
		public static readonly Color4 SeaGreen = new Color4(46, 139, 87);
		/// <summary>
		/// RGB: 255, 245, 238
		/// </summary>
		public static readonly Color4 SeaShell = new Color4(255, 245, 238);
		/// <summary>
		/// RGB: 160, 82, 45
		/// </summary>
		public static readonly Color4 Sienna = new Color4(160, 82, 45);
		/// <summary>
		/// RGB: 192, 192, 192
		/// </summary>
		public static readonly Color4 Silver = new Color4(192, 192, 192);
		/// <summary>
		/// RGB: 135, 206, 235
		/// </summary>
		public static readonly Color4 SkyBlue = new Color4(135, 206, 235);
		/// <summary>
		/// RGB: 106, 90, 205
		/// </summary>
		public static readonly Color4 SlateBlue = new Color4(106, 90, 205);
		/// <summary>
		/// RGB: 112, 128, 144
		/// </summary>
		public static readonly Color4 SlateGray = new Color4(112, 128, 144);
		/// <summary>
		/// RGB: 255, 250, 250
		/// </summary>
		public static readonly Color4 Snow = new Color4(255, 250, 250);
		/// <summary>
		/// RGB: 0, 255, 127
		/// </summary>
		public static readonly Color4 SpringGreen = new Color4(0, 255, 127);
		/// <summary>
		/// RGB: 70, 130, 180
		/// </summary>
		public static readonly Color4 SteelBlue = new Color4(70, 130, 180);
		/// <summary>
		/// RGB: 210, 180, 140
		/// </summary>
		public static readonly Color4 Tan = new Color4(210, 180, 140);
		/// <summary>
		/// RGB: 0, 128, 128
		/// </summary>
		public static readonly Color4 Teal = new Color4(0, 128, 128);
		/// <summary>
		/// RGB: 216, 191, 216
		/// </summary>
		public static readonly Color4 Thistle = new Color4(216, 191, 216);
		/// <summary>
		/// RGB: 255, 99, 71
		/// </summary>
		public static readonly Color4 Tomato = new Color4(255, 99, 71);
		/// <summary>
		/// RGB: 64, 224, 208
		/// </summary>
		public static readonly Color4 Turquoise = new Color4(64, 224, 208);
		/// <summary>
		/// RGB: 238, 130, 238
		/// </summary>
		public static readonly Color4 Violet = new Color4(238, 130, 238);
		/// <summary>
		/// RGB: 245, 222, 179
		/// </summary>
		public static readonly Color4 Wheat = new Color4(245, 222, 179);
		/// <summary>
		/// RGB: 255, 255, 255
		/// </summary>
		public static readonly Color4 White = new Color4(255, 255, 255);
		/// <summary>
		/// RGB: 245, 245, 245
		/// </summary>
		public static readonly Color4 WhiteSmoke = new Color4(245, 245, 245);
		/// <summary>
		/// RGB: 255, 255, 0
		/// </summary>
		public static readonly Color4 Yellow = new Color4(255, 255, 0);
		/// <summary>
		/// RGB: 154, 205, 50
		/// </summary>
		public static readonly Color4 YellowGreen = new Color4(154, 205, 50);

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
		/// The red component of this Color4 structure (0 to 1).
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
		/// The green component of this Color4 structure (0 to 1).
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
		/// The blue component of this Color4 structure (0 to 1).
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
		/// The alpha component of this Color4 structure (0 to 1).
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
		/// Constructs a new Color4 structure directly from the specified RGBA components.
		/// </summary>
		public Color4(Vector4 components) {
			Components = components;
		}

		/// <summary>
		/// Constructs a new Color4 structure from the specified components.
		/// </summary>
		/// <param name="a">The alpha component of the new Color4 structure.</param>
		/// <param name="r">The red component of the new Color4 structure.</param>
		/// <param name="g">The green component of the new Color4 structure.</param>
		/// <param name="b">The blue component of the new Color4 structure.</param>
		public Color4(float a, float r, float g, float b) {
			Components = new Vector4(Clamp(r), Clamp(g), Clamp(b), Clamp(a));
		}

		/// <summary>
		/// Constructs a new Color4 structure from the specified components.
		/// </summary>
		/// <param name="a">The alpha component of the new Color4 structure.</param>
		/// <param name="r">The red component of the new Color4 structure.</param>
		/// <param name="g">The green component of the new Color4 structure.</param>
		/// <param name="b">The blue component of the new Color4 structure.</param>
		public Color4(byte a, byte r, byte g, byte b) {
			Components = new Vector4(r, g, b, a) * 0.00392156863f;
		}

		/// <summary>
		/// Constructs a new Color4 structure from the specified components with alpha fully opaque.
		/// </summary>
		/// <param name="r">The red component of the new Color4 structure.</param>
		/// <param name="g">The green component of the new Color4 structure.</param>
		/// <param name="b">The blue component of the new Color4 structure.</param>
		public Color4(byte r, byte g, byte b) {
			Components = new Vector4(new Vector3(r, g, b) * 0.00392156863f, 1f);
		}

		/// <summary>
		/// Constructs a new Color4 structure from the specified components with alpha fully opaque.
		/// </summary>
		/// <param name="r">The red component of the new Color4 structure.</param>
		/// <param name="g">The green component of the new Color4 structure.</param>
		/// <param name="b">The blue component of the new Color4 structure.</param>
		public Color4(float r, float g, float b) {
			Components = new Vector4(Clamp(r), Clamp(g), Clamp(b), 1f);
		}

		/// <summary>
		/// Constructs a new Color4 structure from the specified components, replacing alpha with the given value.
		/// </summary>
		/// <param name="alpha">The new alpha component.</param>
		/// <param name="baseColor">The RGB values to copy into this new Color4 structure.</param>
		public Color4(float alpha, Color4 baseColor) {
			Components = new Vector4(baseColor.R, baseColor.G, baseColor.B, Clamp(alpha));
		}

		/// <summary>
		/// Constructs a new Color4 structure from the specified components.
		/// </summary>
		/// <param name="argb">The ARGB integer that represents the color.</param>
		public Color4(int argb) {
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
		/// Compares the specified Color4 structures for equality.
		/// </summary>
		/// <param name="left">The left-hand side of the comparison.</param>
		/// <param name="right">The right-hand side of the comparison.</param>
		/// <returns>True if left is equal to right; false otherwise.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator ==(Color4 left, Color4 right) {
			return left.Equals(right);
		}

		/// <summary>
		/// Compares the specified Color4 structures for inequality.
		/// </summary>
		/// <param name="left">The left-hand side of the comparison.</param>
		/// <param name="right">The right-hand side of the comparison.</param>
		/// <returns>True if left is not equal to right; false otherwise.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool operator !=(Color4 left, Color4 right) {
			return !left.Equals(right);
		}

		/// <summary>
		/// Converts the specified System.Drawing.Color to a Color4 structure.
		/// </summary>
		/// <param name="color">The System.Drawing.Color to convert.</param>
		/// <returns>A new Color4 structure containing the converted components.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static explicit operator Color4(Color color) {
			return new Color4(color.A, color.R, color.G, color.B);
		}

		/// <summary>
		/// Converts the specified Color4 to a System.Drawing.Color structure.
		/// </summary>
		/// <param name="color">The Color4 to convert.</param>
		/// <returns>A new System.Drawing.Color structure containing the converted components.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static explicit operator Color(Color4 color) {
			return Color.FromArgb((int) (color.A * 255f), (int) (color.R * 255f), (int) (color.G * 255f), (int) (color.B * 255f));
		}

		/// <summary>
		/// Converts the specified Vector4 to a Color4 structure.
		/// </summary>
		/// <param name="components">The Vector4 to convert.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static implicit operator Color4(Vector4 components) {
			return new Color4() {
				Components = components
			};
		}

		/// <summary>
		/// Converts the specified Color4 to a Vector4 structure.
		/// </summary>
		/// <param name="color">The Color4 to convert.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static implicit operator Vector4(Color4 color) {
			return color.Components;
		}

		/// <summary>
		/// Converts the specified BgraColor to a Color4 structure.
		/// </summary>
		/// <param name="color">The BgraColor to convert.</param>
		/// <returns>A new Color4 structure containing the converted components.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static explicit operator Color4(BgraColor color) {
			return new Color4(color.A, color.R, color.G, color.B);
		}

		/// <summary>
		/// Converts the specified Color4 to a BgraColor structure.
		/// </summary>
		/// <param name="color">The Color4 to convert.</param>
		/// <returns>A new BgraColor structure containing the converted components.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static explicit operator BgraColor(Color4 color) {
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
		/// Compares whether this Color4 structure is equal to the specified object.
		/// </summary>
		/// <param name="obj">An object to compare to.</param>
		/// <returns>True obj is a Color4 structure with the same components as this Color4; false otherwise.</returns>
		public override bool Equals(object obj) {
			if (obj is Color4)
				return Equals((Color4) obj);
			else
				return false;
		}

		/// <summary>
		/// Calculates the hash code for this Color4 structure.
		/// </summary>
		/// <returns>A System.Int32 containing the hashcode of this Color4 structure.</returns>
		public override int GetHashCode() {
			return ToArgb();
		}

		/// <summary>
		/// Creates a System.String that describes this Color4 structure.
		/// </summary>
		/// <returns>A System.String that describes this Color4 structure.</returns>
		public override string ToString() {
			return string.Format("(A: {0}, R: {1}, G: {2}, B: {3})", A.ToString(), R.ToString(), G.ToString(), B.ToString());
		}

		/// <summary>
		/// Compares whether this Color4 structure is equal to the specified Color4.
		/// </summary>
		/// <param name="other">The Color4 structure to compare to.</param>
		/// <returns>True if both Color4 structures contain the same components; false otherwise.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public bool Equals(Color4 other) {
			return R == other.R && G == other.G && B == other.B && A == other.A;
		}
	}
}