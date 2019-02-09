using System;
using System.Text;

namespace OCSS.MHCrypto {

   /// <summary>Classic Caesar encryption and decryption</summary>
   /// <remarks>
   /// See: https://en.wikipedia.org/wiki/Caesar_cipher
   /// This implementation preserves the non-alpha characters in the text as well as the case.
   ///
   /// Notes:
   /// The encryption can be represented using modular arithmetic by first transforming the letters into numbers, according to the scheme, A → 0, B → 1, ..., Z → 25
   /// Encryption of a letter x by a shift n can be described mathematically as
   ///   En(x) = (x + n) mod 26
   ///
   /// Decryption is performed similarly,
   ///   Dn(x) = (x - n) mod 26
   ///   There are different definitions for the modulo operation. In the above, the result is in the range 0 to 25
   ///   If x + n or x − n are not in the range 0 to 25, subtract or add 26 as needed.
   /// </remarks>
   public static class Caesar {

      /// <summary>Decryption for ASCII string</summary>
      /// <param name="cipherText">cipher text</param>
      /// <param name="shift">number of positions to shift. ex) with 1 as shift A=B</param>
      /// <returns>string of plain-text</returns>
      public static string DecryptASCII(string cipherText, byte shift) {
         return DecryptASCII(Encoding.ASCII.GetBytes(cipherText), shift);
      }

      /// <summary>Decryption for ASCII byte array</summary>
      /// <param name="cipherText">cipher text</param>
      /// <param name="shift">number of positions to shift. ex) with 1 as shift A=B</param>
      /// <returns>string of plain-text</returns>
      public static string DecryptASCII(byte[] cipherText, byte shift) {
         if (shift < 0 || shift >= Utils.NUM_ALPHA)
            throw new ArgumentException($"shift must be between 0 and {Utils.NUM_ALPHA - 1}");
         if (cipherText == null)
            throw new ArgumentNullException("cipherText");
         if (shift == 0)   // no shift, return as-is
            return Encoding.ASCII.GetString(cipherText);
         // copy byte array into work buffer for manipulation
         byte[] buff = new byte[cipherText.Length];
         Buffer.BlockCopy(cipherText, 0, buff, 0, cipherText.Length);
         for (int z = 0; z < buff.Length; z++) {
            buff[z] = DecryptOneByteCaesarASCII(buff[z], shift);
         }
         return Encoding.ASCII.GetString(buff);
      }

      /// <summary>Decrypt a single byte of the ciphertext</summary>
      /// <param name="b">byte to decrypt</param>
      /// <param name="shift">number of positions to shift</param>
      /// <returns>the decrypted byte</returns>
      /// <remarks>
      /// If the passed byte in non-alpha, the original byte is returned untouched.
      /// This makes it safe to pass in non-encoded bytes.
      /// Also, this method can be used in companion ciphers like Vigenere.
      /// </remarks>
      public static byte DecryptOneByteCaesarASCII(byte b, byte shift) {
         byte val;
         if (Utils.IsAsciiByteUpper(b)) {
            // check for negative result as c# modulo can be negative
            int intermed = b - Utils.ALPHA_START_UPPER - shift;
            if (intermed < 0)
               intermed += Utils.NUM_ALPHA;
            val = (byte) ((intermed % Utils.NUM_ALPHA) + Utils.ALPHA_START_UPPER);
         }
         else {
            if (Utils.IsAsciiByteLower(b)) {
               // check for negative as c# modulo can be negative
               int intermed = b - Utils.ALPHA_START_LOWER - shift;
               if (intermed < 0)
                  intermed += Utils.NUM_ALPHA;
               val = (byte) ((intermed % Utils.NUM_ALPHA) + Utils.ALPHA_START_LOWER);
            }
            else {
               val = b;    // don't touch as it's not alpha
            }
         }
         return val;
      }

      /// <summary>Encryption for ASCII string</summary>
      /// <param name="plainText">Plain-text</param>
      /// <param name="shift">number of positions to shift. ex) with 1 as shift A=B</param>
      /// <returns>string of encrypted data</returns>
      public static string EncryptASCII(string plainText, byte shift) {
         return EncryptASCII(Encoding.ASCII.GetBytes(plainText), shift);
      }
      /// <summary>Encryption for ASCII byte array</summary>
      /// <param name="plainText">Plain-text</param>
      /// <param name="shift">number of positions to shift. ex) with 1 as shift A=B</param>
      /// <returns>string of encrypted data</returns>
      public static string EncryptASCII(byte[] plainText, byte shift) {
         if (shift < 0 || shift >= Utils.NUM_ALPHA)
            throw new ArgumentException($"shift must be between 0 and {Utils.NUM_ALPHA - 1}");
         if (plainText == null)
            throw new ArgumentNullException("plainText");
         if (shift == 0)   // return as-is for no shift
            return Encoding.ASCII.GetString(plainText);
         // copy byte array into work buffer for manipulation
         byte[] buff = new byte[plainText.Length];
         Buffer.BlockCopy(plainText, 0, buff, 0, plainText.Length);
         for (int z = 0; z < buff.Length; z++) {
            buff[z] = EncryptOneByteCaesarASCII(buff[z], shift);
         }
         return Encoding.ASCII.GetString(buff);
      }

      /// <summary>Encrypt a single byte of the plaintext</summary>
      /// <param name="b">byte to encrypt</param>
      /// <param name="shift">number of positions to shift</param>
      /// <returns>the encrypted byte</returns>
      /// <remarks>
      /// If the passed byte in non-alpha, the original byte is returned untouched.
      /// This makes it safe to pass in non-encoded bytes.
      /// Also, this method can be used in companion ciphers like Vigenere.
      /// </remarks>
      public static byte EncryptOneByteCaesarASCII(byte b, byte shift) {
         byte val;
         if (Utils.IsAsciiByteUpper(b)) {
            val = (byte) (((b - Utils.ALPHA_START_UPPER + shift) % Utils.NUM_ALPHA) + Utils.ALPHA_START_UPPER);
         }
         else {
            if (Utils.IsAsciiByteLower(b)) {
               val = (byte) (((b - Utils.ALPHA_START_LOWER + shift) % Utils.NUM_ALPHA) + Utils.ALPHA_START_LOWER);
            }
            else {
               val = b;    // leave alone, not alpha
            }
         }
         return val;
      }
   }

}
