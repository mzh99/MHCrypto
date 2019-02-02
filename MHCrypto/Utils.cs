using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OCSS.StringUtil;

namespace OCSS.MHCrypto {

   public class SingleByteXorResult {
      public readonly int OtherID;
      public readonly int Key;
      public readonly FrequencyAnalysis FAInst;

      public SingleByteXorResult(int id, int key, FrequencyAnalysis freq) {
         this.OtherID = id;
         this.Key = key;
         this.FAInst = freq;
      }
   }

   public static class Utils {

      public static readonly byte PUNCT_EXCLAMATION = 33;
      public static readonly byte PUNCT_DOUBLE_QUOTE = 34;
      public static readonly byte PUNCT_APOSTROPHE = 39;
      public static readonly byte PUNCT_COMMA = 44;
      public static readonly byte PUNCT_HYPHEN = 45;
      public static readonly byte PUNCT_PERIOD = 46;
      public static readonly byte PUNCT_COLON = 58;
      public static readonly byte PUNCT_SEMICOLON = 59;
      public static readonly byte PUNCT_QUESTION = 63;

      public static readonly byte ASCII_SPACE = 32;
      public static readonly byte ASCII_DELETE = 127;
      public static readonly byte START_UPPER_ASCII = 128;
      public static readonly byte NUM_ALPHA = 26;
      public static readonly byte ALPHA_START_UPPER = 65;
      public static readonly byte ALPHA_START_LOWER = 97;
      public static readonly byte ALPHA_UPPERLOWER_DIFF = (byte) (ALPHA_START_LOWER - ALPHA_START_UPPER);

      /// <summary>XOR two Ascii strings together</summary>
      /// <param name="str1">string 1</param>
      /// <param name="str2">string 2</param>
      /// <returns>byte array of XORed data</returns>
      public static byte[] XorStrings(string str1, string str2) {
         return XorArrays(Encoding.ASCII.GetBytes(str1), Encoding.ASCII.GetBytes(str2));
      }

      /// <summary>XOR two Ascii strings together</summary>
      /// <param name="str1">string 1</param>
      /// <param name="str2">string 2</param>
      /// <returns>ASCII string of XORed data</returns>
      public static string XorStringsAsAsciiString(string str1, string str2) {
         return Encoding.ASCII.GetString(XorStrings(str1, str2));
      }

      /// <summary>XOR two byte arrays of equal size</summary>
      /// <param name="buff1">buffer 1</param>
      /// <param name="buff2">buffer 2</param>
      /// <returns>byte array of XORed data</returns>
      public static byte[] XorArrays(byte[] buff1, byte[] buff2) {
         if (buff1.Length != buff2.Length)
            throw new ArgumentException("lengths must be equal.");
         var output = new byte[buff1.Length];
         for (int ndx = 0; ndx < buff1.Length; ndx++) {
            output[ndx] = (byte) (buff1[ndx] ^ buff2[ndx]);
         }
         return output;
      }

      /// <summary>XOR two byte arrays of equal size</summary>
      /// <param name="buff1">buffer 1</param>
      /// <param name="buff2">buffer 2</param>
      /// <returns>ASCII string of XORed data</returns>
      public static string XorArraysAsAsciiString(byte[] buff1, byte[] buff2) {
         return Encoding.ASCII.GetString(XorArrays(buff1, buff2));
      }

      /// <summary>XOR an array of bytes against a single byte</summary>
      /// <param name="buff1">buffer 1</param>
      /// <param name="val">byte to XOR with</param>
      /// <returns>byte array of XORed data</returns>
      public static byte[] XorArrayWithSingleByte(byte[] buff1, byte val) {
         var output = new byte[buff1.Length];
         for (int ndx = 0; ndx < buff1.Length; ndx++) {
            output[ndx] = (byte) (buff1[ndx] ^ val);
         }
         return output;
      }

      /// <summary>XOR an array of bytes against a single byte</summary>
      /// <param name="buff1">buffer 1</param>
      /// <param name="val">byte to XOR with</param>
      /// <returns>ASCII string of XORed data</returns>
      public static string XorArrayWithSingleByteAsAsciiString(byte[] buff1, byte val) {
         return Encoding.ASCII.GetString(XorArrayWithSingleByte(buff1, val));
      }

      /// <summary>XOR an array of bytes using a repeating key of bytes (wrapping if needed)</summary>
      /// <param name="buff1">array of bytes</param>
      /// <param name="keyBuff">array of key bytes</param>
      /// <returns>byte array of XORed data</returns>
      public static byte[] XORArrayWithRepeatingKey(byte[] buff1, byte[] keyBuff) {
         byte[] output = new byte[buff1.Length];
         int keyNdx = 0;
         for (int z = 0; z < buff1.Length; z++) {
            output[z] = (byte) (buff1[z] ^ keyBuff[keyNdx++]);
            if (keyNdx >= keyBuff.Length)  // wrap key index pointer
               keyNdx = 0;
         }
         return output;
      }

      /// <summary>Extract every xth byte from a byte array</summary>
      /// <param name="buffer">byte array to extract from</param>
      /// <param name="startNdx">starting index to extract; zero = start</param>
      /// <param name="offset">The offset number to use. 4=every 4th byte</param>
      /// <returns></returns>
      public static byte[] ExtractEveryXthByte(byte[] buffer, int startNdx, int offset) {
         if (offset <= 0)
            throw new ArgumentException("offset must be greater than zero.");
         if (startNdx < 0)
            throw new ArgumentException("startIndex must be zero or greater.");

         int numBytes = buffer.Length - startNdx;
         int len = numBytes / offset;
         if (numBytes % offset > 0)
            len++;
         byte[] newBuffer = new byte[len];
         int cnt = 0;
         for (int z = startNdx; z < buffer.Length; z += offset) {
            newBuffer[cnt++] = buffer[z];
         }
         return newBuffer;

      }

      public static bool IsAsciiByteUpper(byte b) {
         return (b >= ALPHA_START_UPPER && b < ALPHA_START_UPPER + NUM_ALPHA);
      }

      public static bool IsAsciiByteLower(byte b) {
         return (b >= ALPHA_START_LOWER && b < ALPHA_START_LOWER + NUM_ALPHA);
      }

      public static byte ConvertAsciiToLower(byte b) {
         return IsAsciiByteUpper(b) ?  (byte) (b + ALPHA_UPPERLOWER_DIFF) : (byte) (b + 0);
      }

      public static byte ConvertAsciiToUpper(byte b) {
         return IsAsciiByteLower(b) ?  (byte) (b - ALPHA_UPPERLOWER_DIFF) : (byte) (b + 0);
      }

      // find lowest Hamming Distance for each possible keysize from MIN_KEYSIZE to MAX_KEYSIZE
      // Note: With minimal samples, you may not get the correct lowest calculated value. (using 1 or 2 for example)
      public static IEnumerable<HammingScore> FindProbableKeySize(byte[] cipherBytes, int minKeySize = 1, int maxKeySize = 40, int numSamples = 6, int topCnt = 3) {
         var hamList = new List<HammingScore>();
         if (numSamples < 1)
            throw new ArgumentException("Number of samples must be one or greater");
         int minCipherSizeReq = (numSamples + 1) * maxKeySize;
         if (minCipherSizeReq > cipherBytes.Length)
            throw new ArgumentException($"Cipher bytes must be at least {minCipherSizeReq} bytes to accomodate number of samples.");
         for (int keySize = minKeySize; keySize <= maxKeySize; keySize++) {
            uint runTot = 0;
            for (int sampleNum = 0; sampleNum < numSamples; sampleNum++) {
               int startNdx = sampleNum * keySize;
               runTot += MHString.HammingDistance(cipherBytes, startNdx, startNdx + keySize, keySize);
            }
            double avg = Math.Round(((double) runTot / (double) (keySize * numSamples)),5);
            hamList.Add(new HammingScore(keySize, numSamples, avg));
         }
         return hamList.OrderBy(h => h.AvgScore).Take(topCnt).ToList();
      }

      public static IEnumerable<SingleByteXorResult> TestAllSingleXorKeys(byte[] testBlock, int id, int numTop) {
         // find most likely key based on freq analysis
         var list = new List<SingleByteXorResult>();
         // try all 256 single byte keys
         for (int k = 0; k < 256; k++) {
            var buffer = Utils.XorArrayWithSingleByte(testBlock, (byte) k);
            list.Add(new SingleByteXorResult(id, k, new FrequencyAnalysis(buffer, FreqTableType.AlphaAndSpace)));
         }
         return list.OrderBy(f => f.FAInst.StatisticalDifference).Take(numTop);
      }

      public static byte[] ProbeForPossibleKeys(byte[] cipherBytes, int id, int keySize) {
         var likelyKey = new byte[keySize];
         for (int offset = 0; offset < keySize; offset++) {
            // create a byte array made up of every keySize bytes starting at position offset
            byte[] testBlock = ExtractEveryXthByte(cipherBytes, offset, keySize);
            var tops = TestAllSingleXorKeys(testBlock, id, 2).ToList();
            likelyKey[offset] = (byte) tops[0].Key;
         }
         return likelyKey;
      }
   }

}
