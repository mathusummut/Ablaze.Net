// AForge Video Library
// AForge.NET framework
//
// Copyright © Andrew Kirillov, 2007-2008
// andrew.kirillov@gmail.com
//

namespace AForge.Video {
	using System;

	/// <summary>
	/// Some internal utilities for handling arrays.
	/// </summary>
	/// 
	public static class ByteArrayUtils {
		/// <summary>
		/// Check if the array contains needle at specified position.
		/// </summary>
		/// 
		/// <param name="array">Source array to check for needle.</param>
		/// <param name="needle">Needle we are searching for.</param>
		/// <param name="startIndex">Start index in source array.</param>
		/// 
		/// <returns>Returns <b>true</b> if the source array contains the needle at
		/// the specified index. Otherwise it returns <b>false</b>.</returns>
		/// 
		public static bool Compare(byte[] array, byte[] needle, int startIndex) {
			int needleLen = needle.Length;
			// compare
			for (int i = 0, p = startIndex; i < needleLen; i++, p++) {
				if (array[p] != needle[i]) {
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Find subarray in the source array.
		/// </summary>
		/// 
		/// <param name="array">Source array to search for needle.</param>
		/// <param name="needle">Needle we are searching for.</param>
		/// <param name="startIndex">Start index in source array.</param>
		/// <param name="sourceLength">Number of bytes in source array, where the needle is searched for.</param>
		/// 
		/// <returns>Returns starting position of the needle if it was found or <b>-1</b> otherwise.</returns>
		/// 
		public static int Find(byte[] array, byte[] needle, int startIndex, int sourceLength) {
			int needleLen = needle.Length;
			int index;

			while (sourceLength >= needleLen) {
				// find needle's starting element
				index = Array.IndexOf(array, needle[0], startIndex, sourceLength - needleLen + 1);

				// if we did not find even the first element of the needls, then the search is failed
				if (index == -1)
					return -1;

				int i, p;
				// check for needle
				for (i = 0, p = index; i < needleLen; i++, p++) {
					if (array[p] != needle[i]) {
						break;
					}
				}

				if (i == needleLen) {
					// needle was found
					return index;
				}

				// continue to search for needle
				sourceLength -= (index - startIndex + 1);
				startIndex = index + 1;
			}
			return -1;
		}

		/// <summary>
		/// Copies the specified number of bytes from the source to the destination memory region.
		/// </summary>
		/// <param name="src">The source to copy from.</param>
		/// <param name="dest">The destination to copy to.</param>
		/// <param name="bytesToCopy">The number of bytes to copy.</param>
		public static unsafe void MemoryCopy(byte* src, byte* dest, uint bytesToCopy) {
			if (sizeof(IntPtr) == 8) {
				switch (bytesToCopy) {
					case 0u:
						return;
					case 1u:
						*dest = *src;
						return;
					case 2u:
						*(short*) dest = *(short*) src;
						return;
					case 3u:
						*(short*) dest = *(short*) src;
						*(dest + 2) = *(src + 2);
						return;
					case 4u:
						*(int*) dest = *(int*) src;
						return;
					case 5u:
						*(int*) dest = *(int*) src;
						*(dest + 4) = *(src + 4);
						return;
					case 6u:
						*(int*) dest = *(int*) src;
						*(short*) (dest + 4) = *(short*) (src + 4);
						return;
					case 7u:
						*(int*) dest = *(int*) src;
						*(short*) (dest + 4) = *(short*) (src + 4);
						*(dest + 6) = *(src + 6);
						return;
					case 8u:
						*(long*) dest = *(long*) src;
						return;
					case 9u:
						*(long*) dest = *(long*) src;
						*(dest + 8) = *(src + 8);
						return;
					case 10u:
						*(long*) dest = *(long*) src;
						*(short*) (dest + 8) = *(short*) (src + 8);
						return;
					case 11u:
						*(long*) dest = *(long*) src;
						*(short*) (dest + 8) = *(short*) (src + 8);
						*(dest + 10) = *(src + 10);
						return;
					case 12u:
						*(long*) dest = *(long*) src;
						*(int*) (dest + 8) = *(int*) (src + 8);
						return;
					case 13u:
						*(long*) dest = *(long*) src;
						*(int*) (dest + 8) = *(int*) (src + 8);
						*(dest + 12) = *(src + 12);
						return;
					case 14u:
						*(long*) dest = *(long*) src;
						*(int*) (dest + 8) = *(int*) (src + 8);
						*(short*) (dest + 12) = *(short*) (src + 12);
						return;
					case 15u:
						*(long*) dest = *(long*) src;
						*(int*) (dest + 8) = *(int*) (src + 8);
						*(short*) (dest + 12) = *(short*) (src + 12);
						*(dest + 14) = *(src + 14);
						return;
					case 16u:
						*(long*) dest = *(long*) src;
						*(long*) (dest + 8) = *(long*) (src + 8);
						return;
					default:
						break;
				}
				if (((int) dest & 3) != 0) {
					if (((int) dest & 1) != 0) {
						*dest = *src;
						src++;
						dest++;
						bytesToCopy--;
						if (((int) dest & 2) == 0)
							goto Aligned;
					}
					*(short*) dest = *(short*) src;
					src += 2;
					dest += 2;
					bytesToCopy -= 2;
					Aligned:
					;
				}

				if (((int) dest & 4) != 0) {
					*(int*) dest = *(int*) src;
					src += 4;
					dest += 4;
					bytesToCopy -= 4;
				}
				uint count = bytesToCopy / 16;
				while (count > 0) {
					((long*) dest)[0] = ((long*) src)[0];
					((long*) dest)[1] = ((long*) src)[1];
					dest += 16;
					src += 16;
					count--;
				}

				if ((bytesToCopy & 8u) != 0) {
					((long*) dest)[0] = ((long*) src)[0];
					dest += 8;
					src += 8;
				}
				if ((bytesToCopy & 4u) != 0) {
					((int*) dest)[0] = ((int*) src)[0];
					dest += 4;
					src += 4;
				}
				if ((bytesToCopy & 2u) != 0) {
					((short*) dest)[0] = ((short*) src)[0];
					dest += 2;
					src += 2;
				}
				if ((bytesToCopy & 1u) != 0)
					*dest = *src;

			} else {
				switch (bytesToCopy) {
					case 0u:
						return;
					case 1u:
						*dest = *src;
						return;
					case 2u:
						*(short*) dest = *(short*) src;
						return;
					case 3u:
						*(short*) dest = *(short*) src;
						*(dest + 2) = *(src + 2);
						return;
					case 4u:
						*(int*) dest = *(int*) src;
						return;
					case 5u:
						*(int*) dest = *(int*) src;
						*(dest + 4) = *(src + 4);
						return;
					case 6u:
						*(int*) dest = *(int*) src;
						*(short*) (dest + 4) = *(short*) (src + 4);
						return;
					case 7u:
						*(int*) dest = *(int*) src;
						*(short*) (dest + 4) = *(short*) (src + 4);
						*(dest + 6) = *(src + 6);
						return;
					case 8u:
						*(int*) dest = *(int*) src;
						*(int*) (dest + 4) = *(int*) (src + 4);
						return;
					case 9u:
						*(int*) dest = *(int*) src;
						*(int*) (dest + 4) = *(int*) (src + 4);
						*(dest + 8) = *(src + 8);
						return;
					case 10u:
						*(int*) dest = *(int*) src;
						*(int*) (dest + 4) = *(int*) (src + 4);
						*(short*) (dest + 8) = *(short*) (src + 8);
						return;
					case 11u:
						*(int*) dest = *(int*) src;
						*(int*) (dest + 4) = *(int*) (src + 4);
						*(short*) (dest + 8) = *(short*) (src + 8);
						*(dest + 10) = *(src + 10);
						return;
					case 12u:
						*(int*) dest = *(int*) src;
						*(int*) (dest + 4) = *(int*) (src + 4);
						*(int*) (dest + 8) = *(int*) (src + 8);
						return;
					case 13u:
						*(int*) dest = *(int*) src;
						*(int*) (dest + 4) = *(int*) (src + 4);
						*(int*) (dest + 8) = *(int*) (src + 8);
						*(dest + 12) = *(src + 12);
						return;
					case 14u:
						*(int*) dest = *(int*) src;
						*(int*) (dest + 4) = *(int*) (src + 4);
						*(int*) (dest + 8) = *(int*) (src + 8);
						*(short*) (dest + 12) = *(short*) (src + 12);
						return;
					case 15u:
						*(int*) dest = *(int*) src;
						*(int*) (dest + 4) = *(int*) (src + 4);
						*(int*) (dest + 8) = *(int*) (src + 8);
						*(short*) (dest + 12) = *(short*) (src + 12);
						*(dest + 14) = *(src + 14);
						return;
					case 16u:
						*(int*) dest = *(int*) src;
						*(int*) (dest + 4) = *(int*) (src + 4);
						*(int*) (dest + 8) = *(int*) (src + 8);
						*(int*) (dest + 12) = *(int*) (src + 12);
						return;
					default:
						break;
				}
				if (((int) dest & 3) != 0) {
					if (((int) dest & 1) != 0) {
						*dest = *src;
						src++;
						dest++;
						bytesToCopy--;
						if (((int) dest & 2) == 0)
							goto Aligned;
					}
					*(short*) dest = *(short*) src;
					src += 2;
					dest += 2;
					bytesToCopy -= 2;
					Aligned:
					;
				}
				uint count = bytesToCopy / 16;
				while (count > 0) {
					((int*) dest)[0] = ((int*) src)[0];
					((int*) dest)[1] = ((int*) src)[1];
					((int*) dest)[2] = ((int*) src)[2];
					((int*) dest)[3] = ((int*) src)[3];
					dest += 16;
					src += 16;
					count--;
				}

				if ((bytesToCopy & 8u) != 0) {
					((int*) dest)[0] = ((int*) src)[0];
					((int*) dest)[1] = ((int*) src)[1];
					dest += 8;
					src += 8;
				}
				if ((bytesToCopy & 4u) != 0) {
					((int*) dest)[0] = ((int*) src)[0];
					dest += 4;
					src += 4;
				}
				if ((bytesToCopy & 2u) != 0) {
					((short*) dest)[0] = ((short*) src)[0];
					dest += 2;
					src += 2;
				}
				if ((bytesToCopy & 1u) != 0)
					*dest = *src;
			}
		}
	}
}