namespace System.IO.Compression {
	/// <summary>
	/// Contains utilities for handling Zip compression.
	/// </summary>
	public static class CompressionUtils {
		/// <summary>
		/// Compresses the byte array using a Zip algorithm.
		/// </summary>
		/// <param name="bytes">The byte array to compress.</param>
		public static byte[] Compress(byte[] bytes) {
			using (MemoryStream memStream = new MemoryStream()) {
				using (GZipStream zipStream = new GZipStream(memStream, CompressionMode.Compress, true))
					zipStream.Write(bytes, 0, bytes.Length);
				return memStream.ToArray();
			}
		}

		/// <summary>
		/// Compresses the byte array onto a stream in memory.
		/// </summary>
		/// <param name="bytes">The byte array to compress.</param>
		public static MemoryStream CompressToStream(byte[] bytes) {
			MemoryStream memStream = new MemoryStream();
			using (GZipStream zipStream = new GZipStream(memStream, CompressionMode.Compress, true))
				zipStream.Write(bytes, 0, bytes.Length);
			memStream.Position = 0;
			return memStream;
		}

		/// <summary>
		/// Compresses the stream into a byte array.
		/// </summary>
		/// <param name="stream">The stream of data to compress.</param>
		public static byte[] Compress(Stream stream) {
			using (MemoryStream memStream = new MemoryStream()) {
				using (GZipStream zipStream = new GZipStream(memStream, CompressionMode.Compress, true)) {
					long position = stream.Position;
					byte[] bytes = new byte[stream.Length - position];
					stream.Read(bytes, 0, bytes.Length);
					zipStream.Write(bytes, 0, bytes.Length);
					stream.Position = position;
				}
				return memStream.ToArray();
			}
		}

		/// <summary>
		/// Compresses the stream onto a memory stream.
		/// </summary>
		/// <param name="stream">The stream of data to compress.</param>
		public static MemoryStream CompressToStream(Stream stream) {
			MemoryStream memStream = new MemoryStream();
			using (GZipStream zipStream = new GZipStream(memStream, CompressionMode.Compress, true)) {
				long position = stream.Position;
				byte[] bytes = new byte[stream.Length - position];
				stream.Read(bytes, 0, bytes.Length);
				zipStream.Write(bytes, 0, bytes.Length);
				stream.Position = position;
			}
			memStream.Position = 0;
			return memStream;
		}

		/// <summary>
		/// Deompresses the byte array using a Zip algorithm.
		/// </summary>
		/// <param name="bytes">The byte array of compressed data.</param>
		public static byte[] Decompress(byte[] bytes) {
			using (MemoryStream compressedStream = new MemoryStream(bytes)) {
				using (MemoryStream uncompressedStream = new MemoryStream()) {
					using (GZipStream zipStream = new GZipStream(compressedStream, CompressionMode.Decompress, true))
						zipStream.CopyTo(uncompressedStream);
					return uncompressedStream.ToArray();
				}
			}
		}

		/// <summary>
		/// Decompresses the byte array onto a stream in memory.
		/// </summary>
		/// <param name="bytes">The byte array of compressed data.</param>
		public static MemoryStream DecompressToStream(byte[] bytes) {
			MemoryStream uncompressedStream = new MemoryStream();
			using (MemoryStream compressedStream = new MemoryStream(bytes)) {
				using (GZipStream zipStream = new GZipStream(compressedStream, CompressionMode.Decompress, true))
					zipStream.CopyTo(uncompressedStream);
			}
			uncompressedStream.Position = 0;
			return uncompressedStream;
		}

		/// <summary>
		/// Decompresses the stream into a byte array.
		/// </summary>
		/// <param name="stream">The stream of compressed data.</param>
		public static byte[] Decompress(Stream stream) {
			long position = stream.Position;
			using (MemoryStream uncompressedStream = new MemoryStream()) {
				using (GZipStream zipStream = new GZipStream(stream, CompressionMode.Decompress, true))
					zipStream.CopyTo(uncompressedStream);
				stream.Position = position;
				return uncompressedStream.ToArray();
			}
		}

		/// <summary>
		/// Decompresses the stream onto a memory stream.
		/// </summary>
		/// <param name="stream">The stream of compressed data.</param>
		public static MemoryStream DecompressToStream(Stream stream) {
			long position = stream.Position;
			MemoryStream uncompressedStream = new MemoryStream();
			using (GZipStream zipStream = new GZipStream(stream, CompressionMode.Decompress, true))
				zipStream.CopyTo(uncompressedStream);
			stream.Position = position;
			uncompressedStream.Position = 0;
			return uncompressedStream;
		}
	}
}