using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCSS.MHCrypto {
   /// <summary>Implements a homophonic substitution cipher similar to the Beale cipher.</summary>
   /// <remarks>See: https://en.wikipedia.org/wiki/Book_cipher for more details.</remarks>
   public static class BookCipher {

      public static string Decrypt(IEnumerable<string> cipherNums, IEnumerable<string> words) {
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
         return Decrypt(nums, words);
      }

      public static string Decrypt(IEnumerable<int> cipherNums, IEnumerable<string> words) {
         int cnt = 0;
         foreach (var wd in words) {
            if (string.IsNullOrEmpty(wd.Trim()))
               throw new ArgumentException($"Word {cnt + 1} is null or empty.");
            cnt++;
         }
         return Decrypt(cipherNums, words.Select(w => (w.Trim()[0])));
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
