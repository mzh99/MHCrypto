using System;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OCSS.MHCrypto;

namespace MHCrypto.Tests {

   [TestClass]
   public class CaesarTests {

      [TestMethod]
      [ExpectedException(typeof(ArgumentException))]
      public void CaesarWithHighShiftThrows() {
         Caesar.EncryptASCII("test", 26);
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void CaesarWithNullParmThrows() {
         Caesar.EncryptASCII((string) null, 1);
      }

      [TestMethod]
      public void CaesarCryptIsSuccessful() {
         const string THE_QBF = "THE QUICK BROWN FOX JUMPS OVER THE LAZY DOG";
         const string THE_QBF_SHIFT23 = "QEB NRFZH YOLTK CLU GRJMP LSBO QEB IXWV ALD";

         string cipherText, plainText;

         cipherText = Caesar.EncryptASCII("ABC", 1);
         Assert.AreEqual("BCD", cipherText, "ABC not encrypted correctly");

         cipherText = Caesar.EncryptASCII("abc", 1);
         Assert.AreEqual("bcd", cipherText, "abc not encrypted correctly");

         cipherText = Caesar.EncryptASCII("XYZ", 1);
         Assert.AreEqual("YZA", cipherText, "XYZ not encrypted correctly");

         cipherText = Caesar.EncryptASCII("xyz", 1);
         Assert.AreEqual("yza", cipherText, "xyz not encrypted correctly");

         cipherText = Caesar.EncryptASCII(THE_QBF, 23);
         Assert.AreEqual(THE_QBF_SHIFT23, cipherText, "THE QUICK BROWN FOX FAILED AND FLOPPED.");

         plainText = Caesar.DecryptASCII("BCD", 1);
         Assert.AreEqual("ABC", plainText, "BCD not decrypted correctly");

         plainText = Caesar.DecryptASCII("bcd", 1);
         Assert.AreEqual("abc", plainText, "bcd not decrypted correctly");

         plainText = Caesar.DecryptASCII(THE_QBF_SHIFT23, 23);
         Assert.AreEqual(THE_QBF, plainText, "Fail: QEB NRFZH YOLTK CLU GRJMP LSBO QEB IXWV ALD");
      }
   }

   [TestClass]
   public class VigenereTests {

      // expected values taken from: https://en.wikipedia.org/wiki/Vigen%C3%A8re_cipher
      [TestMethod]
      public void CryptShortIsSuccessful() {
         const string KEY1 = "LEMON";
         const string MSG1 = "ATTACKATDAWN";

         string cipherText = Vigenere.EncryptASCII(MSG1, KEY1);
         Assert.AreEqual("LXFOPVEFRNHR", cipherText, $"Encrypt of {MSG1} failed");
         string plainText = Vigenere.DecryptASCII(cipherText, KEY1);
         Assert.AreEqual(MSG1, plainText, $"Decrypted message doesn't equal {MSG1}");
      }

      [TestMethod]
      public void CryptLowerIsSuccessful() {
         const string KEY1 = "lemon";
         const string MSG1 = "attackatdawn";

         string cipherText = Vigenere.EncryptASCII(MSG1, KEY1);
         Assert.AreEqual("lxfopvefrnhr", cipherText, $"Encrypt of {MSG1} failed");
         //string plainText = Vigenere.DecryptASCII(cipherText, KEY1);
         //Assert.AreEqual(MSG1, plainText, $"Decrypted message doesn't equal {MSG1}");
      }

      [TestMethod]
      public void CryptEqualLenKeyMsgIsSuccessful() {
         const string KEY1 = "ABCDABCDABCDABCDABCDABCDABCD";
         const string MSG1 = "CRYPTOISSHORTFORCRYPTOGRAPHY";

         string cipherText = Vigenere.EncryptASCII(MSG1, KEY1);
         Assert.AreEqual("CSASTPKVSIQUTGQUCSASTPIUAQJB", cipherText, $"Encrypt of {MSG1} failed");
         string plainText = Vigenere.DecryptASCII(cipherText, KEY1);
         Assert.AreEqual(MSG1, plainText, $"Decrypted message doesn't equal {MSG1}");
      }

      [TestMethod]
      public void DoubleCryptIsSuccessful() {
         const string KEY1 = "GO";
         const string KEY2 = "CAT";
         const string MSG1 = "ATTACKATDAWN";;
         string cipherText = Vigenere.EncryptASCII(MSG1, KEY1);
         string finalText = Vigenere.EncryptASCII(cipherText, KEY2);
         Assert.AreEqual("IHSQIRIHCQCU", finalText, $"Double Encryption of {MSG1} failed");
      }

   }

   [TestClass]
   public class FreqTests {

      const string THE_QBF = "THE QUICK BROWN FOX JUMPS OVER THE LAZY DOG";
      readonly int[] QBF_COUNTS = new int[] { 8, 1, 1, 1, 1, 3, 1, 1, 2, 1, 1, 1, 1, 1, 1, 4, 1, 1, 2, 1, 2, 2, 1, 1, 1, 1, 1 };

      [TestMethod]
      public void FreqGeneratesCorrectly() {
         var bytes = Encoding.ASCII.GetBytes(THE_QBF);
         var freq = new FrequencyAnalysis(bytes, FreqTableType.AlphaOnly);
         var list = freq.GetFrequencyCounts().ToArray();
         Assert.AreEqual(1, list[97], "# A incorrect");
         Assert.AreEqual(4, list[111], "# O incorrect");
         var vals = list.Where(n => n > 0).ToArray();
         CollectionAssert.AreEqual(QBF_COUNTS, vals, "Final counts are incorrect");
         double letterAPct = freq.GetFrequencyPctByChar('a');
         double letterAPct2 = freq.GetFrequencyPctByChar('A');
         Assert.AreEqual(letterAPct, letterAPct2, "Letter A and letter a not equal");
         Assert.AreEqual(Math.Round(100.0 / (double) THE_QBF.Length, 5), letterAPct, "Letter A not 1/43 freq");
      }

   }


 [TestClass]
   public class BookCipherTests {
      private static readonly string[] TestWords = { "four", "score", "and", "seven", "years"};

      [TestMethod]
      public void BookCipherDecryptBasicIsSuccessful() {
         var result = BookCipher.Decrypt(new int[] { 1, 2, 3, 4 }, TestWords);
         Assert.AreEqual("fsas", result, "simple cipher should be fsas");
      }

      [TestMethod]
      public void BookCipherDecryptWithIntAsStringIsSuccessful() {
         var result = BookCipher.Decrypt(new string[] { "1", "2", "3", "4" }, TestWords);
         Assert.AreEqual("fsas", result, "IntAsString should be fsas");
      }

      [TestMethod]
      [ExpectedException(typeof(FormatException))]
      public void BookCipherWithBadIntFormatStringThrows() {
         BookCipher.Decrypt(new string[] { "1", "2", "3", "abc" }, TestWords);
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentOutOfRangeException))]
      public void BookCipherWithWordIndexOutOfRangeThrows() {
         BookCipher.Decrypt(new int[] { 1, 2, 8 }, TestWords);
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentException))]
      public void BookCipherWithEmptyWordThrows() {
         BookCipher.Decrypt(new int[] { 1, 2 }, new string[] { "1", "" });
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentOutOfRangeException))]
      public void BookCipherWithZeroWordIndexThrows() {
         BookCipher.Decrypt(new int[] { 1, 2, 0 }, TestWords);
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentOutOfRangeException))]
      public void BookCipherWithNegativeWordIndexThrows() {
         BookCipher.Decrypt(new int[] { 1, 2, -1 }, TestWords);
      }



   }

}
