using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace System.Graphics.Models {
	/// <summary>
	/// Contains the algorithm to generate a height map from image.
	/// </summary>
	public static class HeightMapGenerator {
		/// <summary>
		/// Generates height map from the specified parameters.
		/// </summary>
		/// <param name="parameters">The height map parameters.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static MeshComponent GenerateHeightmap(ref HeightMapParams parameters) {
			return GenerateHeightmap(parameters.Image, parameters.Scale, parameters.ColorHeightMultiplier, parameters.BlurRadius, parameters.OptimizeIndices, parameters.FlushBuffer, parameters.BindAction);
		}

		/// <summary>
		/// Generates height map from the specified image.
		/// </summary>
		/// <param name="image">The image to use the to generate heights from.</param>
		/// <param name="scale">The X and Y scale of the base.</param>
		/// <param name="colorHeightMultiplier">The height multipler of each color component (RGB).</param>
		/// <param name="blurRadius">The smoothness of the generated vertices.</param>
		/// <param name="optimizeIndices">Whether to optimize the indices. Takes less space, but may take ages to generate if a large size is used.</param>
		/// <param name="flushBuffer">Whether to flush the buffer to VRAM on first render. This frees memory, but means that subsequent operations that
		/// manipulate the mesh vertices must be performed on an OpenGL thread.</param>
		/// <param name="bindAction">Determines what to do with the image after binding into GPU memory</param>
		public static MeshComponent GenerateHeightmap(Bitmap image, Vector2 scale, Vector3 colorHeightMultiplier, float blurRadius, bool optimizeIndices = false, bool flushBuffer = true, ImageParameterAction bindAction = ImageParameterAction.RemoveReference) {
			if (image == null)
				return MeshComponent.Empty;
			float[][] heights;
			int width, height;
			Size size;
			using (PixelWorker temp = PixelWorker.FromImage(image, true, false, bindAction)) {
				size = temp.Size;
				if (blurRadius >= 0f)
					ImageLib.BoxBlur(temp, blurRadius);
				else
					ImageLib.BoxSharpen(temp, blurRadius);
				width = temp.Width;
				height = temp.Height;
				heights = new float[width][];
				for (int i = 0; i < width; i++)
					heights[i] = new float[height];
				ParallelLoop.For(0, temp.PixelCount, delegate (int pixel) {
					BgraColor color = temp.GetPixelBgra(pixel * temp.ComponentCount);
					if (color.R == 0 && color.G == 0 && color.B == 0)
						color.R = 1;
					heights[pixel % width][pixel / width] = (color.R * colorHeightMultiplier.X + color.G * colorHeightMultiplier.Y + color.B * colorHeightMultiplier.Z) * color.A * 0.00392156863f;
				});
			}
			width--;
			height--;
			float texIncrX = 1f / width;
			float texIncrY = 1f / height;
			List<Vertex> vertices = new List<Vertex>();
			int x, y = 0;
			while (y < height) {
				x = 0;
				while (x < width) {
					if (heights[x][y] != 0f) {
						vertices.Add(new Vertex(new Vector3(x, heights[x][y], y), new Vector2((x % size.Width) * texIncrX, (y % size.Height) * texIncrY), x == 0 || y == 0 ? Vector3.UnitY : Vector3.Normalize(new Vector3(-(heights[x + 1][y] - heights[x - 1][y]), 2, heights[x][y + 1] - heights[x][y - 1]))));
						x++;
						vertices.Add(new Vertex(new Vector3(x, heights[x][y], y), new Vector2((x % size.Width) * texIncrX, (y % size.Height) * texIncrY), x == width || y == 0 ? Vector3.UnitY : Vector3.Normalize(new Vector3(-(heights[x + 1][y] - heights[x - 1][y]), 2, heights[x][y + 1] - heights[x][y - 1]))));
						y++;
						vertices.Add(new Vertex(new Vector3(x, heights[x][y], y), new Vector2((x % size.Width) * texIncrX, (y % size.Height) * texIncrY), x == width || y == height ? Vector3.UnitY : Vector3.Normalize(new Vector3(-(heights[x + 1][y] - heights[x - 1][y]), 2, heights[x][y + 1] - heights[x][y - 1]))));
						x--;
						vertices.Add(new Vertex(new Vector3(x, heights[x][y], y), new Vector2((x % size.Width) * texIncrX, (y % size.Height) * texIncrY), x == 0 || y == height ? Vector3.UnitY : Vector3.Normalize(new Vector3(-(heights[x + 1][y] - heights[x - 1][y]), 2, heights[x][y + 1] - heights[x][y - 1]))));
						y--;
					}
					x++;
				}
				y++;
			}
			MeshComponent component = new MeshComponent("HeightMap", null, MeshExtensions.TriangulateQuads(vertices.ToArray()), optimizeIndices, OGL.BufferUsageHint.StaticDraw);
			component.ScaleMesh(new Vector3(scale.X, 1f, scale.Y), Vector3.Zero);
			component.FlushBufferOnNextRender = flushBuffer;
			return component;
		}
	}

	/// <summary>
	/// Stores the height map parameters.
	/// </summary>
	[Serializable]
	public struct HeightMapParams {
		/// <summary>
		/// The image to generate the height map from.
		/// </summary>
		public Bitmap Image;
		/// <summary>
		/// The X and Y scale of the base.
		/// </summary>
		public Vector2 Scale;
		/// <summary>
		/// The height multipler of each color component (RGB).
		/// </summary>
		public Vector3 ColorHeightMultiplier;
		/// <summary>
		/// The smoothness of the generated vertices.
		/// </summary>
		public float BlurRadius;
		/// <summary>
		/// Whether to optimize the indices. Takes less space, but may take ages to generate if a large size is used.
		/// </summary>
		public bool OptimizeIndices;
		/// <summary>
		/// Whether to flush the buffer to VRAM on first render. This frees memory, but means that subsequent operations that
		/// manipulate the mesh vertices must be performed on an OpenGL thread.
		/// </summary>
		public bool FlushBuffer;
		/// <summary>
		/// Determines what to do with the image after binding into GPU memory
		/// </summary>
		public ImageParameterAction BindAction;

		/// <summary>
		/// Initializes the height map parameters
		/// </summary>
		/// <param name="image">The image to generate the height map from</param>
		/// <param name="scale">The X and Y scale of the base</param>
		/// <param name="colorHeightMultiplier">The height multipler of each color component (RGB)</param>
		/// <param name="blurRadius">The smoothness of the generated vertices</param>
		/// <param name="optimizeIndices">Whether to optimize the indices. Takes less space, but may take ages to generate if a large size is used</param>
		/// <param name="flushBuffer">Whether to flush the buffer to VRAM on first render. This frees memory, but means that subsequent operations that
		/// <param name="bindAction">Determines what to do with the image after binding into GPU memory</param>
		/// manipulate the mesh vertices must be performed on an OpenGL thread</param>
		public HeightMapParams(Bitmap image, Vector2 scale, Vector3 colorHeightMultiplier, float blurRadius, bool optimizeIndices = false, bool flushBuffer = true, ImageParameterAction bindAction = ImageParameterAction.RemoveReference) {
			Image = image;
			Scale = scale;
			ColorHeightMultiplier = colorHeightMultiplier;
			BlurRadius = blurRadius;
			OptimizeIndices = optimizeIndices;
			FlushBuffer = flushBuffer;
			BindAction = bindAction;
		}
	}
}