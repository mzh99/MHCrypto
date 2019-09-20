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

      public static string Decrypt(IEnumerable<string> cipherNums, IEnumerable<string> words, LetterSelection letterSel = LetterSelection.FirstLetter) {
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
         return Decrypt(nums, words, letterSel);
      }

      public static string Decrypt(IEnumerable<int> cipherNums, IEnumerable<string> words, LetterSelection letterSel = LetterSelection.FirstLetter) {
         string[] wdArray = words.ToArray();
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

   }

}
