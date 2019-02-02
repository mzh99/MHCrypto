using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace OCSS.MHCrypto {

   /// <summary>Vigenère cipher encryption and decryption. (An interwoven Caesar cipher)</summary>
   /// <remarks>
   /// See: https://en.wikipedia.org/wiki/Vigen%C3%A8re_cipher
   /// This implementation preserves the non-alpha characters in the text as well as the case.
   /// </remarks>
   public static class Vigenere {

      public static string EncryptASCII(string plainText, string key) {
         return EncryptASCII(Encoding.ASCII.GetBytes(plainText), Encoding.ASCII.GetBytes(key));
      }

      public static string EncryptASCII(byte[] plainText, byte[] key) {
         if (plainText == null)
            throw new ArgumentNullException("plaintext");
         if (key == null)
            throw new ArgumentNullException("key");
         // select all alpha, convert to upper case, and turn characters into individual shift values for Caesar byte-by-byte encryption
         var convKey = KeyToShiftValues(key).ToArray();
         if (convKey.Length == 0)
            throw new ArgumentException("Converted key is empty. Please use alphabetic characters.");
         // copy byte array into work buffer for manipulation
         byte[] buff = new byte[plainText.Length];
         Buffer.BlockCopy(plainText, 0, buff, 0, plainText.Length);
         int keyNdx = 0;
         for (int z = 0; z < buff.Length; z++) {
            if (Utils.IsAsciiByteUpper(buff[z]) || Utils.IsAsciiByteLower(buff[z])) {
               buff[z] = Caesar.EncryptOneByteCaesarASCII(buff[z], convKey[keyNdx++]);
               if (keyNdx >= convKey.Length)
                  keyNdx = 0;    // wrap key back to beginning
            }
         }
         return Encoding.ASCII.GetString(buff);
      }

      public static string DecryptASCII(string cipherText, string key) {
         return DecryptASCII(Encoding.ASCII.GetBytes(cipherText), Encoding.ASCII.GetBytes(key));
      }

      public static string DecryptASCII(byte[] cipherText, byte[] key) {
         if (cipherText == null)
            throw new ArgumentNullException("cipherText");
         if (key == null)
            throw new ArgumentNullException("key");
         var convKey = KeyToShiftValues(key).ToArray();
         if (convKey.Length == 0)
            throw new ArgumentException("Converted key is empty. Please use alphabetic characters.");
         // copy byte array into work buffer for manipulation
         byte[] buff = new byte[cipherText.Length];
         Buffer.BlockCopy(cipherText, 0, buff, 0, cipherText.Length);
         int keyNdx = 0;
         for (int z = 0; z < buff.Length; z++) {
            if (Utils.IsAsciiByteUpper(buff[z]) || Utils.IsAsciiByteLower(buff[z])) {
               buff[z] = Caesar.DecryptOneByteCaesarASCII(buff[z], convKey[keyNdx++]);
               if (keyNdx >= convKey.Length)
                  keyNdx = 0;    // wrap key back to beginning
            }
         }
         return Encoding.ASCII.GetString(buff);
      }

      private static IEnumerable<byte> KeyToShiftValues(IEnumerable<byte> key) {
         return key.Where(b => Utils.IsAsciiByteLower(b) || Utils.IsAsciiByteUpper(b)).Select(b => Utils.ConvertAsciiToUpper(b)).Select(b => (byte) (b - Utils.ALPHA_START_UPPER));
      }
   }

}
