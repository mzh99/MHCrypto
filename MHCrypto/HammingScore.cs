namespace OCSS.MHCrypto {

   public class HammingScore {
      public readonly int Keysize;
      public readonly int NumSamples;
      public readonly double AvgScore;

      public HammingScore(int keySize, int numSamples, double score) {
         this.Keysize = keySize;
         this.NumSamples = numSamples;
         this.AvgScore = score;
      }
   }

}
