#if NET20

namespace System.Security.Cryptography {
	/// <summary>
	/// Represents the abstract base class from which all implementations of the Advanced Encryption Standard (AES) must inherit.
	/// </summary>
	public abstract class Aes : SymmetricAlgorithm {
		private static KeySizes[] legalBlockSizes;
		private static KeySizes[] legalKeySizes;

		static Aes() {
			legalBlockSizes = new KeySizes[] { new KeySizes(128, 128, 0) };
			legalKeySizes = new KeySizes[] { new KeySizes(128, 256, 64) };
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Security.Cryptography.Aes" /> class.
		/// </summary>
		public Aes() {
			LegalBlockSizesValue = legalBlockSizes;
			LegalKeySizesValue = legalKeySizes;
			BlockSizeValue = 128;
			FeedbackSizeValue = 8;
			KeySizeValue = 256;
			ModeValue = CipherMode.CBC;
		}

		/// <summary>Creates a cryptographic object that is used to perform the symmetric algorithm.</summary>
		/// <returns>A cryptographic object that is used to perform the symmetric algorithm.</returns>
		public static new Aes Create() {
			return Create("AES");
		}

		/// <summary>Creates a cryptographic object that specifies the implementation of AES to use to perform the symmetric algorithm.</summary>
		/// <returns>A cryptographic object that is used to perform the symmetric algorithm.</returns>
		/// <param name="algorithmName">The name of the specific implementation of AES to use.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="algorithmName" /> parameter is null.</exception>
		public static new Aes Create(string algorithmName) {
			if (algorithmName == null)
				throw new ArgumentNullException(nameof(algorithmName));
			return CryptoConfig.CreateFromName(algorithmName) as Aes;
		}
	}
}

#endif