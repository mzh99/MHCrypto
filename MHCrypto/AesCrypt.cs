using System;
using System.IO;
using System.Security.Cryptography;

namespace OCSS.MHCrypto {

   public static class AesCrypt {

      public static string DecryptStringFromBytes_Aes(byte[] cipherBytes, byte[] key, byte[] iv, int blockSize, CipherMode mode) {
         if (cipherBytes == null || cipherBytes.Length <= 0)
            throw new ArgumentNullException("cipherText");
         if (key == null || key.Length <= 0)
            throw new ArgumentNullException("Key");
         if (iv == null || iv.Length <= 0)
            throw new ArgumentNullException("Key");

         // Declare the string used to hold the decrypted text.
         string plainText = null;
         // Create an AesManaged object with the specified key and IV.
         using (AesManaged aesAlg = new AesManaged()) {
            aesAlg.Key = key;
            aesAlg.IV = iv;
            aesAlg.BlockSize = blockSize;
            aesAlg.Mode = mode;

            // Create a decrytor to perform the stream transform.
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            // Create the streams used for decryption.
            using (MemoryStream msDecrypt = new MemoryStream(cipherBytes)) {
               using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read)) {
                  using (StreamReader srDecrypt = new StreamReader(csDecrypt)) {
                     // Read the decrypted bytes from the decrypting stream and place them in a string.
                     plainText = srDecrypt.ReadToEnd();
                  }
               }
            }

         }

         return plainText;
      }

      public static string DecryptStringFromBytes_Aes(byte[] cipherBytes, byte[] key, int blockSize, CipherMode mode) {
         if (cipherBytes == null || cipherBytes.Length <= 0)
            throw new ArgumentNullException("cipherText");
         if (key == null || key.Length <= 0)
            throw new ArgumentNullException("Key");

         // Declare the string used to hold the decrypted text.
         string plainText = null;

         // Create an AesManaged object with the specified key and IV.
         using (AesManaged aesAlg = new AesManaged()) {
            aesAlg.Key = key;
            aesAlg.BlockSize = blockSize;
            aesAlg.Mode = mode;

            // Create a decrytor to perform the stream transform.
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            // Create the streams used for decryption.
            using (MemoryStream msDecrypt = new MemoryStream(cipherBytes)) {
               using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read)) {
                  using (StreamReader srDecrypt = new StreamReader(csDecrypt)) {
                     // Read the decrypted bytes from the decrypting stream and place them in a string.
                     plainText = srDecrypt.ReadToEnd();
                  }
               }
            }

         }

         return plainText;
      }
   }

}
