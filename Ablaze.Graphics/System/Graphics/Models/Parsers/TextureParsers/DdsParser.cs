using System.Graphics.OGL;
using System.IO;

namespace System.Graphics.Models.Parsers.TextureParsers {
	/// <summary>
	/// Contains methods for parsing DDS files.
	/// </summary>
	[TextureParser("dds,Parse")]
	public static class DdsParser {
		private const byte HeaderSizeInBytes = 128; // all non-image data together is 128 Bytes
		private const uint BitMask = 0x00000007; // bits = 00 00 01 11
		private static NotImplementedException Unfinished = new NotImplementedException("ERROR: Only 2 Dimensional DXT1/3/5 compressed images for now. 1D/3D Textures may not be compressed according to spec.");

		/// <summary>
		/// Parses textures.
		/// </summary>
		/// <param name="source">The location of the file to parse the texture from.</param>
		/// <returns>A list of the textures parsed.</returns>
		public static ITexture[] Parse(Stream source) {
			// invalidate whatever it was before
			TextureTarget dimension = TextureTarget.Texture2D;
			int texturehandle = 0;
			int _Width = 0;
			int _Height = 0;
			int _Depth = 0;
			int _MipMapCount = 0;
			byte _BytesPerBlock = 0;
			PixelInternalFormat _PixelInternalFormat = PixelInternalFormat.Rgba8;
			byte[] _RawDataFromFile;
			using (BinaryReader reader = new BinaryReader(source)) {
				using (MemoryStream ms = new MemoryStream()) {
					byte[] buffer = new byte[4096];
					int count;
					while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
						ms.Write(buffer, 0, count);
					_RawDataFromFile = ms.ToArray();
				}
			}
			UInt32 offset = 0;
			string idString = GetString(ref _RawDataFromFile, offset);
			offset += 4;
			uint dwSize = GetUInt32(ref _RawDataFromFile, offset);
			offset += 4;
			uint dwFlags = GetUInt32(ref _RawDataFromFile, offset);
			offset += 4;
			uint dwHeight = GetUInt32(ref _RawDataFromFile, offset);
			offset += 4;
			uint dwWidth = GetUInt32(ref _RawDataFromFile, offset);
			offset += 8;
			uint dwDepth = GetUInt32(ref _RawDataFromFile, offset);
			offset += 4;
			uint dwMipMapCount = GetUInt32(ref _RawDataFromFile, offset);
			offset += 48;
			uint pfSize = GetUInt32(ref _RawDataFromFile, offset);
			offset += 4;
			uint pfFlags = GetUInt32(ref _RawDataFromFile, offset);
			offset += 4;
			uint pfFourCC = GetUInt32(ref _RawDataFromFile, offset);
			offset += 24;
			uint dwCaps1 = GetUInt32(ref _RawDataFromFile, offset);
			offset += 4;
			uint dwCaps2 = GetUInt32(ref _RawDataFromFile, offset);
			offset += 16;
			// start by checking if all forced flags are present. Flags indicate valid fields, but aren't written by every tool .....
			if (idString != "DDS " || // magic key
				 dwSize != 124 || // constant size of struct, never reused
				 pfSize != 32 || // constant size of struct, never reused
				 !CheckFlag(dwFlags, (uint) eDDSD.CAPS) ||        // must know it's caps
				 !CheckFlag(dwFlags, (uint) eDDSD.PIXELFORMAT) || // must know it's format
				 !CheckFlag(dwCaps1, (uint) eDDSCAPS.TEXTURE)     // must be a Texture
				)
				throw new ArgumentException("ERROR: File has invalid signature or missing Flags.");

			if (CheckFlag(dwFlags, (uint) eDDSD.WIDTH))
				_Width = (int) dwWidth;
			else
				throw new ArgumentException("ERROR: Flag for Width not set.");

			if (CheckFlag(dwFlags, (uint) eDDSD.HEIGHT))
				_Height = (int) dwHeight;
			else
				throw new ArgumentException("ERROR: Flag for Height not set.");

			if (CheckFlag(dwFlags, (uint) eDDSD.DEPTH) && CheckFlag(dwCaps2, (uint) eDDSCAPS2.VOLUME)) {
				dimension = TextureTarget.Texture3D; // image is 3D Volume
				_Depth = (int) dwDepth;
				throw Unfinished;
			} else {// image is 2D or Cube
				if (CheckFlag(dwCaps2, (uint) eDDSCAPS2.CUBEMAP)) {
					dimension = TextureTarget.TextureCubeMap;
					_Depth = 6;
				} else {
					dimension = TextureTarget.Texture2D;
					_Depth = 1;
				}
			}

			// these flags must be set for mipmaps to be included
			if (CheckFlag(dwCaps1, (uint) eDDSCAPS.MIPMAP) && CheckFlag(dwFlags, (uint) eDDSD.MIPMAPCOUNT))
				_MipMapCount = (int) dwMipMapCount; // image contains MipMaps
			else
				_MipMapCount = 1; // only 1 main image

			// Should never happen
			if (CheckFlag(dwFlags, (uint) eDDSD.PITCH) && CheckFlag(dwFlags, (uint) eDDSD.LINEARSIZE))
				throw new ArgumentException("INVALID: Pitch AND Linear Flags both set. Image cannot be uncompressed and DTXn compressed at the same time.");

			// This flag is set if format is uncompressed RGB RGBA etc.
			if (CheckFlag(dwFlags, (uint) eDDSD.PITCH)) {
				throw Unfinished;
			}

			if (CheckFlag(pfFlags, (uint) eDDPF.FOURCC))
				switch ((eFOURCC) pfFourCC) {
					case eFOURCC.DXT1:
						_PixelInternalFormat = (PixelInternalFormat) ExtTextureCompressionS3tc.CompressedRgbS3tcDxt1Ext;
						_BytesPerBlock = 8;
						break;
					//case eFOURCC.DXT2:
					case eFOURCC.DXT3:
						_PixelInternalFormat = (PixelInternalFormat) ExtTextureCompressionS3tc.CompressedRgbaS3tcDxt3Ext;
						_BytesPerBlock = 16;
						break;
					//case eFOURCC.DXT4:
					case eFOURCC.DXT5:
						_PixelInternalFormat = (PixelInternalFormat) ExtTextureCompressionS3tc.CompressedRgbaS3tcDxt5Ext;
						_BytesPerBlock = 16;
						break;
					default:
						throw Unfinished; // handle uncompressed formats 
				} else
				throw Unfinished;
			GL.GenTextures(1, out texturehandle);
			GL.BindTexture(dimension, texturehandle);

			int Cursor = HeaderSizeInBytes;
			// foreach face in the cubemap, get all it's mipmaps levels. Only one iteration for Texture2D
			for (int Slices = 0; Slices < _Depth; Slices++) {
				int trueMipMapCount = _MipMapCount - 1; // TODO: triplecheck correctness
				int Width = _Width;
				int Height = _Height;
				for (int Level = 0; Level < _MipMapCount; Level++) // start at base image
				{
					int BlocksPerRow = (Width + 3) >> 2;
					int BlocksPerColumn = (Height + 3) >> 2;
					int SurfaceBlockCount = BlocksPerRow * BlocksPerColumn; //   // DXTn stores Texels in 4x4 blocks, a Color block is 8 Bytes, an Alpha block is 8 Bytes for DXT3/5
					int SurfaceSizeInBytes = SurfaceBlockCount * _BytesPerBlock;

					// skip mipmaps smaller than a 4x4 Pixels block, which is the smallest DXTn unit.
					if (Width > 2 && Height > 2) { // Note: there could be a potential problem with non-power-of-two cube maps
						byte[] RawDataOfSurface = new byte[SurfaceSizeInBytes];
						for (int sourceColumn = 0; sourceColumn < BlocksPerColumn; sourceColumn++) {
							int targetColumn = BlocksPerColumn - sourceColumn - 1;
							for (int row = 0; row < BlocksPerRow; row++) {
								int target = (targetColumn * BlocksPerRow + row) * _BytesPerBlock;
								int sourceData = (sourceColumn * BlocksPerRow + row) * _BytesPerBlock + Cursor;
								switch (_PixelInternalFormat) {
									case (PixelInternalFormat) ExtTextureCompressionS3tc.CompressedRgbS3tcDxt1Ext:
										// Color only
										RawDataOfSurface[target + 0] = _RawDataFromFile[sourceData + 0];
										RawDataOfSurface[target + 1] = _RawDataFromFile[sourceData + 1];
										RawDataOfSurface[target + 2] = _RawDataFromFile[sourceData + 2];
										RawDataOfSurface[target + 3] = _RawDataFromFile[sourceData + 3];
										RawDataOfSurface[target + 4] = _RawDataFromFile[sourceData + 7];
										RawDataOfSurface[target + 5] = _RawDataFromFile[sourceData + 6];
										RawDataOfSurface[target + 6] = _RawDataFromFile[sourceData + 5];
										RawDataOfSurface[target + 7] = _RawDataFromFile[sourceData + 4];
										break;
									case (PixelInternalFormat) ExtTextureCompressionS3tc.CompressedRgbaS3tcDxt3Ext:
										// Alpha
										RawDataOfSurface[target + 0] = _RawDataFromFile[sourceData + 6];
										RawDataOfSurface[target + 1] = _RawDataFromFile[sourceData + 7];
										RawDataOfSurface[target + 2] = _RawDataFromFile[sourceData + 4];
										RawDataOfSurface[target + 3] = _RawDataFromFile[sourceData + 5];
										RawDataOfSurface[target + 4] = _RawDataFromFile[sourceData + 2];
										RawDataOfSurface[target + 5] = _RawDataFromFile[sourceData + 3];
										RawDataOfSurface[target + 6] = _RawDataFromFile[sourceData + 0];
										RawDataOfSurface[target + 7] = _RawDataFromFile[sourceData + 1];

										// Color
										RawDataOfSurface[target + 8] = _RawDataFromFile[sourceData + 8];
										RawDataOfSurface[target + 9] = _RawDataFromFile[sourceData + 9];
										RawDataOfSurface[target + 10] = _RawDataFromFile[sourceData + 10];
										RawDataOfSurface[target + 11] = _RawDataFromFile[sourceData + 11];
										RawDataOfSurface[target + 12] = _RawDataFromFile[sourceData + 15];
										RawDataOfSurface[target + 13] = _RawDataFromFile[sourceData + 14];
										RawDataOfSurface[target + 14] = _RawDataFromFile[sourceData + 13];
										RawDataOfSurface[target + 15] = _RawDataFromFile[sourceData + 12];
										break;
									case (PixelInternalFormat) ExtTextureCompressionS3tc.CompressedRgbaS3tcDxt5Ext:
										// Alpha, the first 2 bytes remain 
										RawDataOfSurface[target + 0] = _RawDataFromFile[sourceData + 0];
										RawDataOfSurface[target + 1] = _RawDataFromFile[sourceData + 1];

										// extract 3 bits each and flip them
										GetBytesFromUInt24(ref RawDataOfSurface, (uint) target + 5, FlipUInt24(GetUInt24(ref _RawDataFromFile, (uint) sourceData + 2)));
										GetBytesFromUInt24(ref RawDataOfSurface, (uint) target + 2, FlipUInt24(GetUInt24(ref _RawDataFromFile, (uint) sourceData + 5)));

										// Color
										RawDataOfSurface[target + 8] = _RawDataFromFile[sourceData + 8];
										RawDataOfSurface[target + 9] = _RawDataFromFile[sourceData + 9];
										RawDataOfSurface[target + 10] = _RawDataFromFile[sourceData + 10];
										RawDataOfSurface[target + 11] = _RawDataFromFile[sourceData + 11];
										RawDataOfSurface[target + 12] = _RawDataFromFile[sourceData + 15];
										RawDataOfSurface[target + 13] = _RawDataFromFile[sourceData + 14];
										RawDataOfSurface[target + 14] = _RawDataFromFile[sourceData + 13];
										RawDataOfSurface[target + 15] = _RawDataFromFile[sourceData + 12];
										break;
									default:
										throw new ArgumentException("ERROR: Should have never arrived here! Bad _PixelInternalFormat! Should have been dealt with much earlier.");
								}
							}
						}
						switch (dimension) {
							case TextureTarget.Texture2D:
								GL.CompressedTexImage2D(TextureTarget.Texture2D,
														 Level,
														 _PixelInternalFormat,
														 Width,
														 Height,
														0,
														 SurfaceSizeInBytes,
														 RawDataOfSurface);
								break;
							case TextureTarget.TextureCubeMap:
								GL.CompressedTexImage2D(TextureTarget.TextureCubeMapPositiveX + Slices,
														 Level,
														 _PixelInternalFormat,
														 Width,
														 Height,
														 0,
														 SurfaceSizeInBytes,
														 RawDataOfSurface);
								break;
							case TextureTarget.Texture1D: // Untested
							case TextureTarget.Texture3D: // Untested
							default:
								throw new ArgumentException("ERROR: Use DXT for 2D Images only. Cannot evaluate " + dimension);
						}
						int width, height, internalformat, compressed;
						switch (dimension) {
							case TextureTarget.Texture1D:
							case TextureTarget.Texture2D:
							case TextureTarget.Texture3D:
								GL.GetTexLevelParameter(dimension, Level, GetTextureParameter.TextureWidth, out width);
								GL.GetTexLevelParameter(dimension, Level, GetTextureParameter.TextureHeight, out height);
								GL.GetTexLevelParameter(dimension, Level, GetTextureParameter.TextureInternalFormat, out internalformat);
								GL.GetTexLevelParameter(dimension, Level, GetTextureParameter.TextureCompressed, out compressed);
								break;
							case TextureTarget.TextureCubeMap:
								GL.GetTexLevelParameter(TextureTarget.TextureCubeMapPositiveX + Slices, Level, GetTextureParameter.TextureWidth, out width);
								GL.GetTexLevelParameter(TextureTarget.TextureCubeMapPositiveX + Slices, Level, GetTextureParameter.TextureHeight, out height);
								GL.GetTexLevelParameter(TextureTarget.TextureCubeMapPositiveX + Slices, Level, GetTextureParameter.TextureInternalFormat, out internalformat);
								GL.GetTexLevelParameter(TextureTarget.TextureCubeMapPositiveX + Slices, Level, GetTextureParameter.TextureCompressed, out compressed);
								break;
							default:
								throw Unfinished;
						}
					} else {
						if (trueMipMapCount > Level)
							trueMipMapCount = Level - 1; // The current Level is invalid
					}

					Width /= 2;
					if (Width < 1)
						Width = 1;
					Height /= 2;
					if (Height < 1)
						Height = 1;
					Cursor += SurfaceSizeInBytes;
				}
				GL.TexParameter(dimension, (TextureParameterName) All.TextureBaseLevel, 0);
				GL.TexParameter(dimension, (TextureParameterName) All.TextureMaxLevel, trueMipMapCount);

				int TexMaxLevel;
				GL.GetTexParameter(dimension, GetTextureParameter.TextureMaxLevel, out TexMaxLevel);
			}

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.ClampToEdge);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.ClampToEdge);

			GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (int) TextureEnvMode.Modulate);
			return new ITexture[] { new Texture2D(texturehandle) };
		}

		[Flags] // Surface Description
		[Serializable]
		private enum eDDSD : uint {
			CAPS = 0x00000001, // is always present
			HEIGHT = 0x00000002, // is always present
			WIDTH = 0x00000004, // is always present
			PITCH = 0x00000008, // is set if the image is uncompressed
			PIXELFORMAT = 0x00001000, // is always present
			MIPMAPCOUNT = 0x00020000, // is set if the image contains MipMaps
			LINEARSIZE = 0x00080000, // is set if the image is compressed
			DEPTH = 0x00800000 // is set for 3D Volume Textures
		}

		[Flags] // Pixelformat 
		[Serializable]
		private enum eDDPF : uint {
			NONE = 0x00000000, // not part of DX, added for convenience
			ALPHAPIXELS = 0x00000001,
			FOURCC = 0x00000004,
			RGB = 0x00000040,
			RGBA = 0x00000041
		}

		/// <summary>This list was derived from nVidia OpenGL SDK</summary>
		[Flags] // Texture types
		[Serializable]
		private enum eFOURCC : uint {
			UNKNOWN = 0,
			R8G8B8 = 20,
			A8R8G8B8 = 21,
			X8R8G8B8 = 22,
			R5G6B5 = 23,
			X1R5G5B5 = 24,
			A1R5G5B5 = 25,
			A4R4G4B4 = 26,
			R3G3B2 = 27,
			A8 = 28,
			A8R3G3B2 = 29,
			X4R4G4B4 = 30,
			A2B10G10R10 = 31,
			A8B8G8R8 = 32,
			X8B8G8R8 = 33,
			G16R16 = 34,
			A2R10G10B10 = 35,
			A16B16G16R16 = 36,

			L8 = 50,
			A8L8 = 51,
			A4L4 = 52,

			D16_LOCKABLE = 70,
			D32 = 71,
			D24X8 = 77,
			D16 = 80,

			D32F_LOCKABLE = 82,
			L16 = 81,

			// s10e5 formats (16-bits per channel)
			R16F = 111,
			G16R16F = 112,
			A16B16G16R16F = 113,

			// IEEE s23e8 formats (32-bits per channel)
			R32F = 114,
			G32R32F = 115,
			A32B32G32R32F = 116,
			DXT1 = 0x31545844,
			DXT2 = 0x32545844,
			DXT3 = 0x33545844,
			DXT4 = 0x34545844,
			DXT5 = 0x35545844,
		}

		[Flags] // dwCaps1
		[Serializable]
		private enum eDDSCAPS : uint {
			NONE = 0x00000000, // not part of DX, added for convenience
			COMPLEX = 0x00000008, // should be set for any DDS file with more than one main surface
			TEXTURE = 0x00001000, // should always be set
			MIPMAP = 0x00400000 // only for files with MipMaps
		}

		[Flags]  // dwCaps2
		[Serializable]
		private enum eDDSCAPS2 : uint {
			NONE = 0x00000000, // not part of DX, added for convenience
			CUBEMAP = 0x00000200,
			CUBEMAP_POSITIVEX = 0x00000400,
			CUBEMAP_NEGATIVEX = 0x00000800,
			CUBEMAP_POSITIVEY = 0x00001000,
			CUBEMAP_NEGATIVEY = 0x00002000,
			CUBEMAP_POSITIVEZ = 0x00004000,
			CUBEMAP_NEGATIVEZ = 0x00008000,
			CUBEMAP_ALL_FACES = 0x0000FC00,
			VOLUME = 0x00200000 // for 3D Textures
		}

		/// <summary> Returns true if the flag is set, false otherwise</summary>
		private static bool CheckFlag(uint variable, uint flag) {
			return (variable & flag) > 0 ? true : false;
		}

		private static string GetString(ref byte[] input, uint offset) {
			return string.Empty + (char) input[offset + 0] + (char) input[offset + 1] + (char) input[offset + 2] + (char) input[offset + 3];
		}

		private static uint GetUInt32(ref byte[] input, uint offset) {
			return (uint) (((input[offset + 3] * 256 + input[offset + 2]) * 256 + input[offset + 1]) * 256 + input[offset + 0]);
		}

		private static uint GetUInt24(ref byte[] input, uint offset) {
			return (uint) ((input[offset + 2] * 256 + input[offset + 1]) * 256 + input[offset + 0]);
		}

		private static void GetBytesFromUInt24(ref byte[] input, uint offset, uint splitme) {
			input[offset + 0] = (byte) (splitme & 0x000000ff);
			input[offset + 1] = (byte) ((splitme & 0x0000ff00) >> 8);
			input[offset + 2] = (byte) ((splitme & 0x00ff0000) >> 16);
			return;
		}

		/// <summary>DXT5 Alpha block flipping, inspired by code from Evan Hart (nVidia SDK)</summary>
		private static uint FlipUInt24(uint inputUInt24) {
			byte[][] ThreeBits = new byte[2][];
			for (int i = 0; i < 2; i++)
				ThreeBits[i] = new byte[4];

			// extract 3 bits each into the array
			ThreeBits[0][0] = (byte) (inputUInt24 & BitMask);
			inputUInt24 >>= 3;
			ThreeBits[0][1] = (byte) (inputUInt24 & BitMask);
			inputUInt24 >>= 3;
			ThreeBits[0][2] = (byte) (inputUInt24 & BitMask);
			inputUInt24 >>= 3;
			ThreeBits[0][3] = (byte) (inputUInt24 & BitMask);
			inputUInt24 >>= 3;
			ThreeBits[1][0] = (byte) (inputUInt24 & BitMask);
			inputUInt24 >>= 3;
			ThreeBits[1][1] = (byte) (inputUInt24 & BitMask);
			inputUInt24 >>= 3;
			ThreeBits[1][2] = (byte) (inputUInt24 & BitMask);
			inputUInt24 >>= 3;
			ThreeBits[1][3] = (byte) (inputUInt24 & BitMask);

			// stuff 8x 3bits into 3 bytes
			uint Result = 0;
			Result = Result | (uint) (ThreeBits[1][0] << 0);
			Result = Result | (uint) (ThreeBits[1][1] << 3);
			Result = Result | (uint) (ThreeBits[1][2] << 6);
			Result = Result | (uint) (ThreeBits[1][3] << 9);
			Result = Result | (uint) (ThreeBits[0][0] << 12);
			Result = Result | (uint) (ThreeBits[0][1] << 15);
			Result = Result | (uint) (ThreeBits[0][2] << 18);
			Result = Result | (uint) (ThreeBits[0][3] << 21);
			return Result;
		}
	}
}