#pragma warning disable 1591
using System.Drawing.Imaging;
using System.Graphics.OGL;

namespace System.Graphics.Text {
	class TexturePage : IDisposable {
		int gLTexID;
		int width;
		int height;

		public int GLTexID {
			get {
				return gLTexID;
			}
		}
		public int Width {
			get {
				return width;
			}
		}
		public int Height {
			get {
				return height;
			}
		}

		public TexturePage(string filePath) {
			var bitmap = new QBitmap(filePath);
			CreateTexture(bitmap.bitmapData);
			bitmap.Free();
		}

		public TexturePage(BitmapData dataSource) {
			CreateTexture(dataSource);
		}

		private void CreateTexture(BitmapData dataSource) {
			width = dataSource.Width;
			height = dataSource.Height;

			Helper.SafeGLEnable(EnableCap.Texture2D, () => {
				GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

				GL.GenTextures(1, out gLTexID);
				GL.BindTexture(TextureTarget.Texture2D, gLTexID);

				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.ClampToBorder);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.ClampToBorder);

				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Linear);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Linear);

				GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0,
					System.Graphics.OGL.TargetPixelFormat.Bgra, PixelType.UnsignedByte, dataSource.Scan0);

				GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
			});
		}

		~TexturePage() {
			if (gLTexID != 0) {
				GraphicsContext.RaiseResourceLeakedEvent(this, LeakedWhile.Finalizing, new IntPtr(gLTexID));
				gLTexID = 0;
			}
		}

		public void Dispose() {
			GL.DeleteTexture(gLTexID);
			GC.SuppressFinalize(this);
		}
	}
}

#pragma warning restore 1591