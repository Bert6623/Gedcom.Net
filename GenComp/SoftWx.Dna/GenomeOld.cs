using System;
using System.Collections.Generic;
using System.Linq;

namespace SoftWx.Dna {
    /// <summary>
    /// Represents an individual's Genome as a collection of allele pair values
    /// for some set of SNPs.
    /// </summary>
    public class GenomeOld {
        /// <summary>Type of genome test.</summary>
        public enum GenomeType : byte {
            /// <summary>Unknwn genome test.</summary>
            Unknown,
            /// <summary>23AndMe version 2 genome test.</summary>
            MeAnd23v2,
            /// <summary>23AndMe version 3 genome test.</summary>
            MeAnd23v3,
            /// <summary>FTDNA (Family Tree DNA) Illumina genome test.</summary>
            Ftdna,
        }
        private Dictionary<Snp, byte> genotypes = new Dictionary<Snp, byte>();
        private SnpCollection snps = null;
        private object lockObj = new object();

        /// <summary>
        /// Create a new instance of Genome, with a null name
        /// and Unknown GenomeType.
        /// </summary>
        public GenomeOld() : this(null, GenomeType.Unknown) { }

        /// <summary>
        /// Create a new instance of Genome with the specified
        /// name and GenomeType.
        /// </summary>
        /// <param name="name">The name of this Genome.</param>
        /// <param name="genomeType">The type of the genome test.</param>
        public GenomeOld(string name, GenomeType genomeType) {
            this.Name = name;
            this.GenomeTestType = genomeType;
        }

        /// <summary>
        /// Gets the Name of this Genome.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the type of the genome test.
        /// </summary>
        public GenomeType GenomeTestType { get; private set; }

        /// <summary>
        /// Gets the count of SNPs contained in this Genome.
        /// </summary>
        public int Count { get { return genotypes.Count; } }

        /// <summary>
        /// Gets the byte Alleles value for the specified Snp.
        /// </summary>
        /// <param name="snp">The Snp whose alleles value is desired.</param>
        /// <returns>The byte Alleles value for the specified Snp.</returns>
        public byte this[Snp snp] {
            get {
                if (snp == null) throw new ArgumentNullException("Snp cannot be null.");
                return this.genotypes[snp];
            }
        }

        /// <summary>
        /// Adds the specified byte Alleles value for the specified Snp to this Genome.
        /// </summary>
        /// <param name="snp">The Snp being added.</param>
        /// <param name="alleles">The byte Alleles value to add for the specified Snp.</param>
        public virtual void Add(Snp snp, byte alleles) {
            if (snp == null) throw new ArgumentNullException("Snp cannot be null.");

            this.genotypes.Add(snp, alleles);
        }

        /// <summary>
        /// Determines whether the specified Snp is contained in this Genome.
        /// </summary>
        /// <param name="snp">The Snp to find.</param>
        /// <returns>true if this Genome contains the specified Snp, otherwise false.</returns>
        public bool ContainsKey(Snp snp) {
            return this.genotypes.ContainsKey(snp);
        }

        /// <summary>
        /// Clears all Snp and Allele data from the Genome.
        /// </summary>
        public virtual void Clear() {
            this.genotypes.Clear();
        }

        /// <summary>
        /// Struct used to convey segment information for segments of contiguous
        /// Snps within a Genome.
        /// </summary>
        public struct SegmentMatch {
            private readonly Snp startSnp;
            private readonly Snp endSnp;
            private readonly int snpCount;
            private readonly int hoHoSnpCount;
            private readonly int halvesMatched;

            /// <summary>
            /// Create a new SegmentMatch struct with the specified values.
            /// </summary>
            /// <param name="startSnp">The Snp that is the start of the segment.</param>
            /// <param name="endSnp">The Snp that is the end of the segment (inclusive).</param>
            /// <param name="snpCount">The number of matching Snps within the segment.</param>
            /// <param name="halvesMatched">The number of halves matched; either 1 for a half 
            /// matching segment, or 2 for a full matching segment.</param>
            public SegmentMatch(Snp startSnp, Snp endSnp, int snpCount, int hoHoSnpCount, int halvesMatched) {
                this.startSnp = startSnp;
                this.endSnp = endSnp;
                this.snpCount = snpCount;
                this.hoHoSnpCount = hoHoSnpCount;
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
            public int HoHoSnpCount { get { return this.hoHoSnpCount; } }
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

        /// <summary>
        /// Determines the matching segments shared by this and the specified Genome.
        /// </summary>
        /// <param name="other">The other Genome to compare.</param>
        /// <param name="minSnpCount">The minimum number of contiguous matching Snp allele values to be considered a valid match.</param>
        /// <param name="minCm">The minimum length of a segment in centiMorgans to be considered a valid match.</param>
        /// <param name="minStitchCm">The minimum length of each of two segments that may be stitched together when separated by a single mismatch.</param>
        /// <param name="minStitchSnpCount">The minimum number of contiguous matching SNP allele values in each of two segments that may be
        /// stitched together when separated by a single mismatch.</param>
        /// <param name="onlyFull">Specifies whether </param>
        /// <returns>An IList of SegmentMatch structs describing the matching segments that were identified.</returns>
        public IList<SegmentMatch> MatchingSegments(Genome other, int minSnpCount, double minCm,
            int minStitchSnpCount, double minStitchCm, bool onlyFull) {
            var snps = this.GetSnps();
            var result = new List<SegmentMatch>();
            int matchCount = 0;
            int hoHoSnpCount = 0;
            var prevChr = -1;
            Snp startSnp, endSnp;
            startSnp = endSnp = null;
            int snpCount = 0;
            int genomeSnpCount = snps.Count;
            bool canJoinWithPrevious = false;
            foreach (var snp in snps) {
                snpCount++;
                byte alleles = this.genotypes[snp];
                bool hoho = alleles.IsHomozygous();
                alleles &= Allele.AllAlleles;
                if ((alleles != Allele.AT) && ((alleles & Allele.T) != 0)) alleles ^= Allele.AT;
                if ((alleles != Allele.CG) && ((alleles & Allele.G) != 0)) alleles ^= Allele.CG;
                byte otherAlleles = Allele.Null;
                if (other.Contains(snp)) otherAlleles = other[snp];
                hoho &= otherAlleles.IsHomozygous();
                otherAlleles &= Allele.AllAlleles;
                if ((otherAlleles != Allele.AT) && ((otherAlleles & Allele.T) != 0)) otherAlleles ^= Allele.AT;
                if ((otherAlleles != Allele.CG) && ((otherAlleles & Allele.G) != 0)) otherAlleles ^= Allele.CG;
                bool haveNull = ((alleles == Allele.Null) || (otherAlleles == Allele.Null));
                hoho &= !haveNull;
                bool sameChromosome = (snp.Chromosome == prevChr);
                bool buildingMatch = (matchCount > 0);
                bool haveMatch = (haveNull && buildingMatch && sameChromosome) || ((alleles == otherAlleles) && !haveNull);
                if (!onlyFull) haveMatch = haveMatch || ((alleles & otherAlleles) != 0);
                if (buildingMatch) {
                    if (haveMatch && sameChromosome && (snpCount != genomeSnpCount)) {
                        endSnp = snp;
                        if (!haveNull) {
                            matchCount++;
                            if (hoho) hoHoSnpCount++;
                        }
                    } else {
                        if ((matchCount >= minStitchSnpCount) && (endSnp.cM - startSnp.cM >= minStitchCm)) {
                            // matching segment is big enough to keep
                            if (canJoinWithPrevious) {
                                var lastMatch = result[result.Count - 1];
                                result[result.Count - 1] = new SegmentMatch(lastMatch.StartSnp, endSnp,
                                    lastMatch.SnpCount + matchCount, lastMatch.HoHoSnpCount + hoHoSnpCount,
                                    lastMatch.HalvesMatched);
                            } else {
                                result.Add(new SegmentMatch(startSnp, endSnp, matchCount, hoHoSnpCount, onlyFull ? 2 : 1));
                            }
                            canJoinWithPrevious = sameChromosome;
                        } else {
                            if (!haveNull) canJoinWithPrevious = false;
                        }
                        if (haveMatch) { //we're on next chromosome and it starts with a match
                            startSnp = endSnp = snp;
                            matchCount = 1;
                            if (hoho) hoHoSnpCount = 1;
                        } else {
                            startSnp = endSnp = null;
                            matchCount = 0;
                            hoHoSnpCount = 0;
                        }
                    }
                } else { // !buildingMatch
                    if (haveMatch) {
                        startSnp = endSnp = snp;
                        matchCount = 1;
                        if (hoho) hoHoSnpCount = 1;
                    } else {
                        if (!haveNull) canJoinWithPrevious = false;
                    }
                }
                prevChr = snp.Chromosome;
            }
            List<SegmentMatch> filteredResult = new List<SegmentMatch>();
            if ((minSnpCount != minStitchSnpCount) || (minCm != minStitchCm)) {
                foreach (var match in result) {
                    if ((match.SnpCount >= minSnpCount) && (match.CmLength >= minCm)) {
                        filteredResult.Add(match);
                    }
                }
                return filteredResult;
            } else {
                return result;
            }
        }

        /// <summary>
        /// Gets an SnpCollection of the SNPs contained in this Genome.
        /// </summary>
        /// <returns>An SnpCollection.</returns>
        protected SnpCollection GetSnps() {
            lock (lockObj) {
                if (this.snps == null) {
                    var looseSnps = this.genotypes.Keys.ToArray<Snp>();
                    //Array.Sort(looseSnps);
                    ushort minChr = ushort.MaxValue;
                    ushort maxChr = ushort.MinValue;
                    foreach (var snp in looseSnps) {
                        if (snp.Chromosome < minChr) minChr = snp.Chromosome;
                        if (snp.Chromosome > maxChr) maxChr = snp.Chromosome;
                    }
                    snps = new SnpCollection(minChr, maxChr);
                    foreach (var snp in looseSnps) snps.Add(snp);
                }
                return this.snps;
            }
        }

        // this has a bug in it
        //private IList<SegmentMatch> InterleaveSegments(IList<SegmentMatch> allSegments, IList<SegmentMatch> fullSegments, int minSnpCount, double minCm,
        //    int minStitchSnpCount, double minStitchCm) {
        //    if (fullSegments.Count == 0) return allSegments;
        //    if (allSegments.Count == 0) return fullSegments;
        //    // merge the two
        //    var result = new List<SegmentMatch>();
        //    int aIdx = 0;
        //    int fIdx = 0;
        //    while ((aIdx < allSegments.Count) || (fIdx < fullSegments.Count)) {
        //        if (fIdx == fullSegments.Count) {
        //            result.Add(allSegments[aIdx++]);
        //        } else if (aIdx == allSegments.Count) {
        //            result.Add(fullSegments[fIdx++]);
        //        } else {
        //            var half = allSegments[aIdx];
        //            var full = fullSegments[fIdx];
        //            if (half.StartSnp.Chromosome < full.StartSnp.Chromosome) {
        //                result.Add(half);
        //                aIdx++;
        //            } else if (full.StartSnp.Chromosome < half.StartSnp.Chromosome) {
        //                result.Add(full);
        //                fIdx++;
        //            } else { // on same chromosome
        //                if (half.EndSnp.Position < full.StartSnp.Position) {
        //                    result.Add(half);
        //                    aIdx++;
        //                } else if (full.EndSnp.Position < half.StartSnp.Position) {
        //                    result.Add(full);
        //                    fIdx++;
        //                } else {
        //                    if (half.StartSnp.Position < full.StartSnp.Position) {
        //                        var snpIndex = snps.GetSnpIndex(full.StartSnp.RsId);
        //                        if (snpIndex.Value.Index > 0) {
        //                            var newEnd = snps[half.StartSnp.Chromosome, snpIndex.Value.Index - 1];
        //                            var newHalf = new SegmentMatch(half.StartSnp, newEnd,
        //                                //snpIndex.Value.Index - snps.GetSnpIndex(half.StartSnp.RsId).Value.Index, 1);
        //                                half.SnpCount - full.SnpCount, half.HoHoSnpCount - full.HoHoSnpCount, 1);
        //                            if ((newHalf.SnpCount >= minSnpCount) && (newHalf.EndSnp.cM - newHalf.StartSnp.cM >= minCm)) {
        //                                result.Add(newHalf);
        //                            }
        //                        }
        //                    }
        //                    result.Add(full);
        //                    if (half.EndSnp.Position > full.EndSnp.Position) {
        //                        var snpIndex = snps.GetSnpIndex(full.EndSnp.RsId);
        //                        var newStart = snps[half.StartSnp.Chromosome, snpIndex.Value.Index + 1];
        //                        var newHalf = new SegmentMatch(newStart, half.EndSnp,
        //                            //                                    snps.GetSnpIndex(half.EndSnp.RsId).Value.Index - snpIndex.Value.Index, 1);
        //                                half.SnpCount - full.SnpCount, half.HoHoSnpCount - full.HoHoSnpCount, 1);
        //                        if ((newHalf.SnpCount >= minSnpCount) && (newHalf.EndSnp.cM - newHalf.StartSnp.cM >= minCm)) {
        //                            allSegments[aIdx] = newHalf;
        //                            aIdx--;
        //                        }
        //                    }
        //                    aIdx++;
        //                    fIdx++;
        //                }
        //            }
        //        }
        //    }
        //    return result;
        //}
    }
}
