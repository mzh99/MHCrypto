using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCSS.MHCrypto {

   public enum FreqTableType { AlphaOnly, AlphaAndSpace, AlphaAndSpaceAndPunctuation }

   public static class EngFrequencyTables {

      public static readonly double[] ENG_FREQ_ALPHA_ONLY = {
         0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,  // 11-31
         0.0,  // space
         0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,    // 33-62
         0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,    // 63-92
         0.0, 0.0, 0.0, 0.0,     // 93-96
         8.167, 1.492, 2.782, 4.253, 13.00, 2.228,    // a-f
         2.015, 6.094, 6.966, 0.153, 0.772, 4.025,    // g-l
         2.406, 6.749, 7.507, 1.929, 0.095, 5.987,    // m-r
         6.327, 9.056, 2.758, 0.978, 2.360, 0.150, 1.974, 0.074,   // 115-122 s-z
         0.0, 0.0, 0.0, 0.0, 0.0 // 123-127
      };

      public static readonly double[] ENG_FREQ_ALPHA_AND_SPACE = {
         0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,  // 11-31
         19.18182,         // space
         0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,    // 33-62
         0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,    // 63-92
         0.0, 0.0, 0.0, 0.0,     // 93-96
         6.51738, 1.24248, 2.17339, 3.49835, 10.41442, 1.97881,   // 97-102 (a-f)
         1.58610, 4.92888, 5.58094, 0.09033, 0.50529, 3.31490,    // 103-108 (g-l)
         2.02124, 5.64513, 5.96302, 1.37645, 0.08606, 4.97563,    // 109-114 (m-r)
         5.15760, 7.29357, 2.25134, 0.82903, 1.71272, 0.13692, 1.45984, 0.07836,   // 115-122     (s-z)
         0.0, 0.0, 0.0, 0.0, 0.0 // 123-127
      };

      // blended into a new list based on ENG_FREQ_ALPHA_AND_SPACE and frequencies of punctuation from: https://en.wikipedia.org/wiki/Punctuation_of_English
      // Use caution when using as it's not been proven against any corpus, but should be accurate enough for general usage
      public static readonly double[] ENG_FREQ_ALPHA_AND_SPACE_AND_PUNCTUATION = {
         0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,  // 11-31
         15.18433, // space
         0.33, 2.67,  // 33-34 (exclamation, double-quote)
         0.0, 0.0, 0.0, 0.0,  // 35-38
         2.43,   // 39 (apostrophe/single quote)
         0.0, 0.0, 0.0, 0.0,  // 40-43
         6.13, 1.53, 6.53,    // 44-46 (comma, hyphen, period)
         0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,   // 47-57
         0.34, 0.32, // 58-59 (colon, semicolon)
         0.0, 0.0, 0.0,
         0.56,  // 63 (question mark)
         0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,   // 64-80
         0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,     // 81-96
         5.15916, 0.98355, 1.72046, 2.76929, 8.24405, 1.56643,
         1.25556, 3.90170, 4.41787, 0.07151, 0.39999, 2.62407,
         1.60001, 4.46868, 4.72033, 1.08960, 0.06813, 3.93871,
         4.08276, 5.77359, 1.78216, 0.65626, 1.35579, 0.10839, 1.15561, 0.06203,
         0.0, 0.0, 0.0, 0.0, 0.0 // 123-127
      };

   }

   public class FreqPair {
      public int Index {  get; private set; }
      public int FreqCnt { get; private set; }
      public double FreqPct { get; private set; }

      public FreqPair(int ndx, int cnt, double pct) {
         this.Index = ndx;
         this.FreqCnt = cnt;
         this.FreqPct = pct;
      }
   }

   public class FrequencyAnalysis {
      private readonly byte[] textBytes;
      private readonly int[] freqCnt = new int[256];
      private readonly double[] freqPct = new double[256];
      private readonly double[] alphaFreq;
      private readonly FreqTableType freqTableType;

      public double StatisticalDifference { get; private set; }
      public byte[] OrigBytes { get { return textBytes; } }

      public FrequencyAnalysis(byte[] byteArray, FreqTableType freqTableType = FreqTableType.AlphaOnly) {
         // copy incoming bytes to our copy
         textBytes = new byte[byteArray.Length];
         Buffer.BlockCopy(byteArray, 0, textBytes, 0, byteArray.Length);
         //for (int ndx = 0; ndx < byteArray.Length; ndx++) {
         //   textBytes[ndx] = Utils.ConvertAsciiToLower(byteArray[ndx]);
         //}
         this.freqTableType = freqTableType;
         switch (freqTableType) {
            case FreqTableType.AlphaAndSpace:
               alphaFreq = EngFrequencyTables.ENG_FREQ_ALPHA_AND_SPACE;
               break;
            case FreqTableType.AlphaAndSpaceAndPunctuation:
               alphaFreq = EngFrequencyTables.ENG_FREQ_ALPHA_AND_SPACE_AND_PUNCTUATION;
               break;
            default:    // alpha only default
               alphaFreq = EngFrequencyTables.ENG_FREQ_ALPHA_ONLY;
               break;
         }
         CalcFreq();
      }

      public FreqPair GetFreqPair(byte ndx) {
         return new FreqPair(ndx, freqCnt[ndx], freqPct[ndx]);
      }

      public IEnumerable<FreqPair> GetFreqPairs() {
         for (int z = 0; z < 256; z++) {
            var ndx = (byte) z;
            yield return new FreqPair(ndx, freqCnt[ndx], freqPct[ndx]);
         }
      }

      public int GetFrequencyCount(byte ndx) {
         return freqCnt[ndx];
      }

      public int GetFrequencyCountByChar(char c) {
         var bytes = Encoding.ASCII.GetBytes(new char[] { c });
         bytes[0] = Utils.ConvertAsciiToLower(bytes[0]);
         return GetFrequencyCount(bytes[0]);
      }

      public double GetFrequencyPct(byte ndx) {
         return freqPct[ndx];
      }

      public double GetFrequencyPctByChar(char c) {
         var bytes = Encoding.ASCII.GetBytes(new char[] { c });
         bytes[0] = Utils.ConvertAsciiToLower(bytes[0]);
         return GetFrequencyPct(bytes[0]);
      }

      public IEnumerable<int> GetFrequencyCounts() {
         for (int z = 0; z < 256; z++) {
            yield return GetFrequencyCount((byte) z);
         }
      }

      public IEnumerable<double> GetFrequenciesPcts() {
         for (int z = 0; z < 256; z++) {
            yield return GetFrequencyPct((byte) z);
         }
      }

      public bool HasAnyUnprintable() {
         for (int z = 0; z < freqCnt.Length; z++) {
            if (((z < Utils.ASCII_SPACE) || (z == Utils.ASCII_DELETE)) && (freqCnt[z] > 0))
               return true;
         }
         return false;
      }

      public bool HasAnyExtendedAscii() {
         for (int z = Utils.START_UPPER_ASCII; z < freqCnt.Length; z++) {
            if (freqCnt[z] > 0)
               return true;
         }
         return false;
      }

      //public int GetTotalFreqCountsOfMatchingBytes(byte[] matchList) {
      //   int cnt = 0;
      //   for (int z = 0; z < matchList.Length; z++) {
      //      byte ndx = Utils.ConvertAsciiToUpper(matchList[z]);
      //      cnt += freqCnt[ndx];
      //   }
      //   return cnt;
      //}

      private void CalcFreq() {
         for (int z = 0; z < textBytes.Length; z++) {
            byte ndx = Utils.ConvertAsciiToLower(textBytes[z]);
            freqCnt[ndx]++;
         }
         int sumOfCounts = freqCnt.Sum();
         for (int z = 0; z < 256; z++) {
            freqPct[z] = Math.Round((freqCnt[z] * 100.0) / sumOfCounts, 5);
         }
         // Calculate absolute difference between statistical and actual as a deviation value
         StatisticalDifference = 0;
         for (int z = 0; z < 256; z++) {
            // we don't collect frequencies above 127, so assume zero for those
            var val = (z < Utils.START_UPPER_ASCII) ? alphaFreq[z] : 0.0;
            StatisticalDifference += Math.Abs(freqPct[z] - val);
         }
         //switch (freqTableType) {
         //   case FreqTableType.AlphaAndSpace:
         //      StatisticalDifference += Math.Abs(freqPct[Utils.ASCII_SPACE] - alphaFreq[Utils.ASCII_SPACE]);
         //      for (int z = Utils.ALPHA_START_LOWER; z < Utils.ALPHA_START_LOWER + Utils.NUM_ALPHA; z++) {
         //         StatisticalDifference += Math.Abs(freqPct[z] - alphaFreq[z]);
         //      }
         //      break;
         //   case FreqTableType.AlphaAndSpaceAndPunctuation:
         //      StatisticalDifference += Math.Abs(freqPct[Utils.ASCII_SPACE] - alphaFreq[Utils.ASCII_SPACE]);
         //      StatisticalDifference += Math.Abs(freqPct[Utils.PUNCT_EXCLAMATION] - alphaFreq[Utils.PUNCT_EXCLAMATION]);
         //      StatisticalDifference += Math.Abs(freqPct[Utils.PUNCT_DOUBLE_QUOTE] - alphaFreq[Utils.PUNCT_DOUBLE_QUOTE]);
         //      StatisticalDifference += Math.Abs(freqPct[Utils.PUNCT_APOSTROPHE] - alphaFreq[Utils.PUNCT_APOSTROPHE]);
         //      StatisticalDifference += Math.Abs(freqPct[Utils.PUNCT_COMMA] - alphaFreq[Utils.PUNCT_COMMA]);
         //      StatisticalDifference += Math.Abs(freqPct[Utils.PUNCT_HYPHEN] - alphaFreq[Utils.PUNCT_HYPHEN]);
         //      StatisticalDifference += Math.Abs(freqPct[Utils.PUNCT_PERIOD] - alphaFreq[Utils.PUNCT_PERIOD]);
         //      StatisticalDifference += Math.Abs(freqPct[Utils.PUNCT_COLON] - alphaFreq[Utils.PUNCT_COLON]);
         //      StatisticalDifference += Math.Abs(freqPct[Utils.PUNCT_SEMICOLON] - alphaFreq[Utils.PUNCT_SEMICOLON]);
         //      StatisticalDifference += Math.Abs(freqPct[Utils.PUNCT_QUESTION] - alphaFreq[Utils.PUNCT_QUESTION]);
         //      for (int z = Utils.ALPHA_START_LOWER; z < Utils.ALPHA_START_LOWER + Utils.NUM_ALPHA; z++) {
         //         StatisticalDifference += Math.Abs(freqPct[z] - alphaFreq[z]);
         //      }
         //      break;
         //   default:    // alpha only is default
         //      for (int z = Utils.ALPHA_START_LOWER; z < Utils.ALPHA_START_LOWER + Utils.NUM_ALPHA; z++) {
         //         StatisticalDifference += Math.Abs(freqPct[z] - alphaFreq[z]);
         //      }
         //      break;
         //}
         StatisticalDifference = Math.Round(StatisticalDifference / sumOfCounts, 3);
      }
   }

}
