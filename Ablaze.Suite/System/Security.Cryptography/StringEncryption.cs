using System.IO;
using System.Linq;
using System.Text;

namespace System.Security.Cryptography {
	/// <summary>
	/// An enumeration of the supported symmetric encryption algorithms.
	/// </summary>
	public enum SymmetricEncryption {
		/// <summary>
		/// AES - Advanced Encryption Standard (supports 16 bytes length)
		/// </summary>
		Advanced,
		/// <summary>
		/// DES - Data Encryption Standard (supports 8 bytes length)
		/// </summary>
		Data,
		/// <summary>
		/// Rijndael Encryption Algorithm (supports 16, 24, or 32 bytes length)
		/// </summary>
		Rijndael,
		/// <summary>
		/// RC2 - Rivest Cipher 2 (supports 8 bytes length)
		/// </summary>
		Rivest,
		/// <summary>
		/// TDES - Triple Data Encryption Standard (supports 8 bytes length)
		/// </summary>
		TripleData,
	}

	/// <summary>
	/// Provides simple methods to securely encrypt a string.
	/// </summary>
	public static class StringEncryption {
		/// <summary>
		/// Encrypts the plain text using the specified key.
		/// </summary>
		/// <param name="text">The text to encrypt.</param>
		/// <param name="key">The key to encrypt with.</param>
		/// <param name="method">The symmetric encryption method to use to encrypt.</param>
		/// <param name="bytes">The length in bytes to encrypt with.</param>
		/// <param name="iterations">The number of iterations to repeat the encryption.</param>
		/// <returns>The encrypted string.</returns>
		public static string Encrypt(this string text, string key, SymmetricEncryption method = SymmetricEncryption.Rijndael, int bytes = 32, int iterations = 1000) {
			if (string.IsNullOrEmpty(text) || key == null || key.Length == 0)
				return string.Empty;
			// Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
			// so that the same Salt and IV values can be used when decrypting.  
			byte[] saltStringBytes = GenerateRandomEntropy(bytes);
			byte[] ivStringBytes = GenerateRandomEntropy(bytes);
			byte[] plainTextBytes = Encoding.UTF8.GetBytes(text);
#if !NET35
			using (
#endif
			Rfc2898DeriveBytes password = new Rfc2898DeriveBytes(key, saltStringBytes, iterations)
#if NET35
				;
#else
			) {
#endif
				using (SymmetricAlgorithm symmetricKey = GetAlgorithm(method)) {
					symmetricKey.BlockSize = bytes * 8;
					symmetricKey.Mode = CipherMode.CBC;
					symmetricKey.Padding = PaddingMode.PKCS7;
					using (ICryptoTransform encryptor = symmetricKey.CreateEncryptor(password.GetBytes(bytes), ivStringBytes)) {
						using (MemoryStream memoryStream = new MemoryStream()) {
							using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write)) {
								cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
								cryptoStream.FlushFinalBlock();
								// Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
								saltStringBytes = saltStringBytes.Concat(ivStringBytes).ToArray();
								saltStringBytes = saltStringBytes.Concat(memoryStream.ToArray()).ToArray();
								memoryStream.Close();
								cryptoStream.Close();
								return Convert.ToBase64String(saltStringBytes);
							}
						}
					}
				}
#if !NET35
			}
#endif
		}

		/// <summary>
		/// Decrypts the encrypted cipher text using the specified key.
		/// </summary>
		/// <param name="cipherText">The encrypted text to decrypt.</param>
		/// <param name="key">The key to decrypt with.</param>
		/// <param name="method">The symmetric encryption method to use to decrypt.</param>
		/// <param name="bytes">The length in bytes to decrypt with.</param>
		/// <param name="iterations">The number of iterations to repeat the decryption.</param>
		/// <returns>The decrypted string.</returns>
		public static string Decrypt(this string cipherText, string key, SymmetricEncryption method = SymmetricEncryption.Rijndael, int bytes = 32, int iterations = 1000) {
			if (string.IsNullOrEmpty(cipherText) || key == null || key.Length == 0)
				return string.Empty;
			// Get the complete stream of bytes that represent:
			// [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
			byte[] cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
			// Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
			byte[] cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip(bytes * 2).Take(cipherTextBytesWithSaltAndIv.Length - (bytes * 2)).ToArray();
#if !NET35
			using (
#endif
			Rfc2898DeriveBytes password = new Rfc2898DeriveBytes(key, cipherTextBytesWithSaltAndIv.Take(bytes).ToArray(), iterations)
#if NET35
				;
#else
			) {
#endif
				using (SymmetricAlgorithm symmetricKey = GetAlgorithm(method)) {
					symmetricKey.BlockSize = bytes * 8;
					symmetricKey.Mode = CipherMode.CBC;
					symmetricKey.Padding = PaddingMode.PKCS7;
					using (ICryptoTransform decryptor = symmetricKey.CreateDecryptor(password.GetBytes(bytes), cipherTextBytesWithSaltAndIv.Skip(bytes).Take(bytes).ToArray())) {
						using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes)) {
							using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read)) {
								byte[] plainTextBytes = new byte[cipherTextBytes.Length];
								int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
								memoryStream.Close();
								cryptoStream.Close();
								return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
							}
						}
					}
				}
#if !NET35
			}
#endif
		}

		/// <summary>
		/// Generates a cryptographically random array of bytes.
		/// </summary>
		/// <param name="bytes">The number of bytes to return.</param>
		/// <returns>A cryptographically random array of bytes.</returns>
		public static byte[] GenerateRandomEntropy(int bytes) {
			byte[] randomBytes = new byte[bytes]; // 32 Bytes will give us 256 bits.
#if !NET35
			using (
#endif
			RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider()
#if NET35
				;
#else
			) {
#endif
				// Fill the array with cryptographically secure random bytes.
				rngCsp.GetBytes(randomBytes);
#if !NET35
			}
#endif
			return randomBytes;
		}

		/// <summary>
		/// Gets the encryptor instance that uses the specified encryption algorithm.
		/// </summary>
		/// <param name="algorithm">The algorithm whose instance to return.</param>
		public static SymmetricAlgorithm GetAlgorithm(this SymmetricEncryption algorithm) {
			switch (algorithm) {
				case SymmetricEncryption.Advanced:
					return Aes.Create();
				case SymmetricEncryption.Rijndael:
					return Rijndael.Create();
				case SymmetricEncryption.Rivest:
					return RC2.Create();
				case SymmetricEncryption.TripleData:
					return TripleDES.Create();
				default:
					return DES.Create();
			}
		}
	}
}