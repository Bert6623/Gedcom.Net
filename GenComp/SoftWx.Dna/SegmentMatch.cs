using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftWx.Dna {
    /// <summary>
    /// Struct used to convey segment information for segments of contiguous
    /// Snps within a Genome.
    /// </summary>
    public struct SegmentMatch {
        private readonly Snp startSnp;
        private readonly Snp endSnp;
        private readonly int snpCount;
        private readonly int phasedSnpCount;
        private readonly int score;
        private readonly PhasedGenome.Phasing.MatchKind matchKind;
        private readonly int halvesMatched;

        /// <summary>
        /// Create a new SegmentMatch struct with the specified values.
        /// </summary>
        /// <param name="startSnp">The Snp that is the start of the segment.</param>
        /// <param name="endSnp">The Snp that is the end of the segment (inclusive).</param>
        /// <param name="snpCount">The number of matching Snps within the segment.</param>
        /// <param name="halvesMatched">The number of halves matched; either 1 for a half 
        /// matching segment, or 2 for a full matching segment.</param>
        public SegmentMatch(Snp startSnp, Snp endSnp, int snpCount, int phasedSnpCount, int score,
                            PhasedGenome.Phasing.MatchKind matchKind, int halvesMatched) {
            this.startSnp = startSnp;
            this.endSnp = endSnp;
            this.snpCount = snpCount;
            this.phasedSnpCount = phasedSnpCount;
            this.score = score;
            this.matchKind = matchKind;
            this.halvesMatched = halvesMatched;
        }
        /// <summary>Gets the start Snp for this SegemntMatch.</summary>
        public Snp StartSnp { get { return this.startSnp; } }
        /// <summary>Gets the end Snp (inclusive) for this SegemntMatch.</summary>
        public Snp EndSnp { get { return this.endSnp; } }
        /// <summary>Gets the count of matching Snps in this SegemntMatch.</summary>
        public int SnpCount { get { return this.snpCount; } }
        /// <summary>Gets the length of matching segment in centimorgans.</summary>
        public double CmLength { get { return this.endSnp.cM - this.startSnp.cM; } }
        /// <summary>Gets the count of matching Snps in this SegemntMatch where both individuals are homozygous.
        /// These are important, because they are the only SNPs where a mismatch is possible.</summary>
        public int PhasedSnpCount { get { return this.phasedSnpCount; } }
        /// <summary></summary>
        public int Score { get { return this.score; } }
        /// <summary></summary>
        public PhasedGenome.Phasing.MatchKind MatchKind { get { return this.matchKind; } }
        /// <summary>Gets the halves matched value for this SegemntMatch; either 1 for a half 
        /// matching segment, or 2 for a full matching segment.</summary>
        public int HalvesMatched { get { return this.halvesMatched; } }

        /// <summary>
        /// Returns a string representation of this SegmentMatch.
        /// </summary>
        /// <returns>The string representation of this SegmentMatch.</returns>
        public override string ToString() {
            string type;
            if (halvesMatched == 1) {
                type = "Half";
            } else {
                type = "Full";
            }
            return type + " " + startSnp + " - " + endSnp + " (" + snpCount + ":" + (endSnp.cM - startSnp.cM).ToString("0.00");
        }
    }
}
