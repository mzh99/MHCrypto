using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCSS.MHCrypto {

   public enum LetterSelection { FirstLetter, LastLetter }

   /// <summary>Implements a homophonic substitution cipher similar to the Beale cipher.</summary>
   /// <remarks>
   /// Note: I added a feature to allow last letter of word instead of always using the first.
   /// See: https://en.wikipedia.org/wiki/Book_cipher for more details.
   /// </remarks>
   public static class BookCipher {

      /// <summary>Decrypt a Book Cipher using a string representation of integer cipher numbers and a set of key words.</summary>
      /// <param name="cipherNums">an ordered list of 1-based cipher numbers (word indexes) in string format</param>
      /// <param name="keyWords">an ordered list of key words used to encrypt the document.</param>
      /// <param name="letterSel">a choice of first letter or last letter from key word to encrypt document</param>
      /// <returns>a string representation of decrypted text without any punctuation or spacing</returns>
      public static string Decrypt(IEnumerable<string> cipherNums, IEnumerable<string> keyWords, LetterSelection letterSel = LetterSelection.FirstLetter) {
         int num = 0;
         var numArray = cipherNums.ToArray();
         int[] nums = new int[numArray.Length];
         // validation of cipher numbers being valid string representations of integers
         for (int ndx = 0; ndx < numArray.Length; ndx++) {
            var trimmedNum = numArray[ndx].Trim();
            if (int.TryParse(trimmedNum, out num) == false)
               throw new FormatException($"Cipher number {ndx + 1} has an invalid number ({trimmedNum}).");
            nums[ndx] = num;
         }
         return Decrypt(nums, keyWords, letterSel);
      }

      /// <summary>Decrypt a Book Cipher using a list of integer cipher numbers and a set of key words.</summary>
      /// <param name="cipherNums">an ordered list of 1-based cipher numbers (word indexes)</param>
      /// <param name="keyWords">an ordered list of key words used to encrypt the document.</param>
      /// <param name="letterSel">a choice of first letter or last letter from key word to encrypt document</param>
      /// <returns>a string representation of decrypted text without any punctuation or spacing</returns>
      public static string Decrypt(IEnumerable<int> cipherNums, IEnumerable<string> keyWords, LetterSelection letterSel = LetterSelection.FirstLetter) {
         string[] wdArray = keyWords.ToArray();
         char[] charList = new char[wdArray.Length];
         for (int ndx = 0; ndx < wdArray.Length; ndx++) {
            if (wdArray[ndx] == null)
               throw new ArgumentException($"Word {ndx + 1} is null.");
            wdArray[ndx] = wdArray[ndx].Trim();
            if (wdArray[ndx] == string.Empty)
               throw new ArgumentException($"Word {ndx + 1} is empty.");
            int letterNdx = (letterSel == LetterSelection.FirstLetter) ? 0 : wdArray[ndx].Length - 1;
            charList[ndx] = (wdArray[ndx])[letterNdx];
         }
         return Decrypt(cipherNums, charList);
      }

      /// <summary>/// <summary>Decrypt a Book Cipher using a list of integer cipher numbers and a list of characters used to encrypt.</summary>
      /// <param name="cipherNums">an ordered list of 1-based cipher numbers (word indexes)</param>
      /// <param name="letters">a list of characters used to encrypt the text.</param>
      /// <returns>a string representation of decrypted text without any punctuation or spacing</returns>
      public static string Decrypt(IEnumerable<int> cipherNums, IEnumerable<char> letters) {
         var letterLookup = letters.ToArray();
         int cnt = 0;
         StringBuilder sb = new StringBuilder();
         foreach (var num in cipherNums) {
            if (num <= 0)
               throw new ArgumentOutOfRangeException($"Entry {cnt + 1} has a number ({num}) less than or equal to zero.");
            if (num > letterLookup.Length)
               throw new ArgumentOutOfRangeException($"Entry {cnt + 1} has a number ({num}) exceeding word count in key ({letterLookup.Length})");
            sb.Append(letterLookup[num - 1]);
            cnt++;
         }
         return sb.ToString();
      }

      /// <summary>Encrypt text using a set of key words from a book or document</summary>
      /// <param name="plainTextWords">an ordered list of text to encrypt</param>
      /// <param name="bookText">an ordered list of words used to encrypt; repeats are okay and recommended.</param>
      /// <param name="delimiter">the delimeter to use for separating numbers; typical is comma or space</param>
      /// <param name="letterSel">a choice of first letter or last letter of key word to encrypt document</param>
      /// <returns>a string of encrypted text</returns>
      public static string Encrypt(string plainText, IEnumerable<string> bookText, char delimiter, LetterSelection letterSel = LetterSelection.FirstLetter) {
         Random rnd = new Random();
         var lookup = new Dictionary<char, List<int>>();
         int cnt = 0;
         foreach (var wd in bookText) {
            if (string.IsNullOrEmpty(wd))
               throw new ArgumentException($"Word {cnt + 1} is null or empty.");
            var oneWord = wd.Trim().ToLower();
            if (oneWord == string.Empty)
               throw new ArgumentException($"Word {cnt + 1} is whitespace.");
            int letterNdx = (letterSel == LetterSelection.FirstLetter) ? 0 : oneWord.Length - 1;
            char c = oneWord[letterNdx];
            if (lookup.ContainsKey(c)) {
               lookup[c].Add(cnt + 1);    // add to list
            }
            else {
               var intList = new List<int> { cnt + 1 };
               lookup.Add(c, intList);
            }
            cnt++;
         }
         // now each letter has an associated list of word indexes (which can be chosen at random) to encrypt each letter of the plaintext
         var sb = new StringBuilder();
         bool firstNum = true;
         for (int ndx = 0; ndx < plainText.Length; ndx++) {
            char c = (plainText.Substring(ndx, 1).ToLower())[0];
            if (char.IsLetter(c)) {
               if (lookup.ContainsKey(c) == false)
                  throw new ArgumentOutOfRangeException($"letter {c} is not found in any words of book text.");
               var intList = lookup[c];
               var numChoices = intList.Count;
               // todo: replace Random with crypto-quality PNG
               int wdChoiceNdx = (numChoices == 1) ? 0 : rnd.Next(intList.Count);
               if (firstNum == false)
                  sb.Append($"{delimiter}");
               sb.Append($"{intList[wdChoiceNdx]}");
               firstNum = false;
            }
         }
         return sb.ToString();
      }

   }

}
