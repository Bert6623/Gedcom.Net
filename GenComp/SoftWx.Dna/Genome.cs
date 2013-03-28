using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace SoftWx.Dna {
    /// <summary>
    /// Represents an individual's Genome as a collection of allele pair values
    /// for some set of SNPs.
    /// </summary>
    public class Genome : SnpMap<byte> {
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

        /// <summary>
        /// Create a new instance of Genome, with a null name
        /// and Unknown GenomeType.
        /// </summary>
        /// <param name="firstChromosome">Chromosome number of the first chromosome included in the DnaList.</param>
        /// <param name="lastChromosome">Chromosome number of the last chromosome included in the DnaList.</param>
        public Genome(int firstChromosome, int lastChromosome) : this(firstChromosome, lastChromosome, null, GenomeType.Unknown) { }

        /// <summary>
        /// Create a new instance of Genome with the specified
        /// name and GenomeType.
        /// </summary>
        /// <param name="firstChromosome">Chromosome number of the first chromosome included in the DnaList.</param>
        /// <param name="lastChromosome">Chromosome number of the last chromosome included in the DnaList.</param>
        /// <param name="name">The name of this Genome.</param>
        /// <param name="genomeType">The type of the genome test.</param>
        public Genome(int firstChromosome, int lastChromosome, string name, GenomeType genomeType) : base(firstChromosome, lastChromosome) {
            this.Name = name;
            this.GenomeTestType = genomeType;
        }

        /// <summary>
        /// Gets or sets the Name of this Genome.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the type of the genome test.
        /// </summary>
        public GenomeType GenomeTestType { get; set; }

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
        public IList<SegmentMatch> MatchingSegments(Genome other, SnpMap<MatchWeight> matchWeights, int minSnpCount, double minCm, 
            int minStitchSnpCount, double minStitchCm, bool onlyFull) {
            var result = new List<SegmentMatch>();
            int matchCount = 0;
            int phasedSnpCount = 0;
            int matchScore = 0;
            var prevChr = -1;
            Snp startSnp, endSnp;
            startSnp = endSnp = null;
            int snpCount = 0;
            int genomeSnpCount = this.Count;
            bool canJoinWithPrevious = false;
            foreach (var snp in this.Snps) {
                snpCount++;
                byte alleles = this[snp];
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
                //bool haveMatch = (haveNull && buildingMatch && sameChromosome) || ((alleles == otherAlleles) && !haveNull);
                bool haveMatch;
                if (haveNull) {
                    haveMatch = sameChromosome && buildingMatch;
                } else {
                    haveMatch = (alleles == otherAlleles);
                    if (!onlyFull) {
                        haveMatch = haveMatch || (!hoho && buildingMatch);
                    }
                }
                //bool haveMatch = ((!hoho) && buildingMatch && sameChromosome) || (hoho && (alleles == otherAlleles));
                //if (!onlyFull) haveMatch = haveMatch || ((alleles & otherAlleles) != 0);
                if (buildingMatch) {
                    if (haveMatch && sameChromosome && (snpCount != genomeSnpCount)) {
                        if (!haveNull) {
                            matchCount++;
                            if (hoho) {
                                endSnp = snp;
                                phasedSnpCount++;
                                matchScore += GetMatchWeight(snp, matchWeights, alleles);
                            }
                        }
                    } else { // building match but don't have match
                        if ((matchCount >= minStitchSnpCount) && (endSnp.cM - startSnp.cM >= minStitchCm)) {
                            // matching segment is big enough to keep
                            if (canJoinWithPrevious) {
                                var lastMatch = result[result.Count - 1];
                                result[result.Count - 1] = new SegmentMatch(lastMatch.StartSnp, endSnp,
                                    lastMatch.SnpCount + matchCount, lastMatch.PhasedSnpCount + phasedSnpCount, 
                                    lastMatch.Score + matchScore, PhasedGenome.Phasing.MatchKind.AxA, lastMatch.HalvesMatched);
                            } else {
                                result.Add(new SegmentMatch(startSnp, endSnp, matchCount, phasedSnpCount, matchScore, PhasedGenome.Phasing.MatchKind.AxA, onlyFull ? 2 : 1));
                            }
                            canJoinWithPrevious = sameChromosome;
                        } else {
//                            if ((!haveNull) && hoho) canJoinWithPrevious = false;
                            canJoinWithPrevious = false;
                        }
                        if (haveMatch && hoho) { //we're on next chromosome and it starts with a match
                            startSnp = endSnp = snp;
                            matchCount = 1;
                            if (hoho) {
                                phasedSnpCount = 1;
                                matchScore = GetMatchWeight(snp, matchWeights, alleles);
                            }
                        } else {
                            startSnp = endSnp = null;
                            matchCount = 0;
                            phasedSnpCount = 0;
                            matchScore = 0;
                        }
                    }
                } else { // !buildingMatch
                    if (haveMatch) {
                        if (hoho) {
                            startSnp = endSnp = snp;
                            matchCount = 1;
                            phasedSnpCount = 1;
                            matchScore = GetMatchWeight(snp, matchWeights, alleles);
                        }
                    } else {
                        if ((!haveNull) && hoho) canJoinWithPrevious = false;
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
        private int GetMatchWeight(Snp snp, SnpMap<MatchWeight> matchWeights, byte allele) {
            int result = 10;
            if (matchWeights.Contains(snp)) {
                byte snpAlleles = snp.Alleles;
                if ((allele & snpAlleles) == 0) allele = allele.ReverseAllele();
                if ((allele & snpAlleles.MajorAllele()) != 0) {
                    result = matchWeights[snp].Major;
                } else if ((allele & snpAlleles.MinorAllele()) != 0) {
                    result = matchWeights[snp].Minor;
                } else {
                    result = 10;
                }
            }
            if (result < 10) result = 0;
                //else if (result > 19) result = 100000;
                //            else if (result > 10) result = 10;
                //            else if (result > 15) result = 20;
            else result = 1;
            return result;
        }
        //public void Phase(SegmentMatch match, Genome other) {
        //    DnaIndex startIndex = this.snps.GetSnpIndex(match.StartSnp.RsId).Value;
        //    DnaIndex endIndex = this.snps.GetSnpIndex(match.EndSnp.RsId).Value;
        //    var phasedSegment = new PhasedSegment(startIndex, endIndex.Index - startIndex.Index + 1);
        //    int idx = startIndex.Index;
        //    while (idx <= endIndex.Index) {
        //        Snp snp = this.snps[startIndex.Chromosome, idx];
        //        byte alleleA = Allele.Null;
        //        byte alleleB = Allele.Null;
        //        byte thisAlleles = this[snp];
        //        byte thisAllelesBasic = (byte) (thisAlleles & Allele.AllAlleles);
        //        if (thisAllelesBasic != Allele.Null) {
        //            byte otherAlleles = other[snp];
        //            byte otherAllelesBasic = (byte)(otherAlleles & Allele.AllAlleles);
        //            if (thisAlleles.IsHomozygous()) {
        //                //// just split the homozygous alleles unless the other is not a match
        //                //if ((!otherAlleles.IsHomozygous())
        //                //    || ((((thisAllelesBasic & Allele.AT) != 0) == ((otherAllelesBasic & Allele.AT) != 0))
        //                //        && (((thisAllelesBasic & Allele.CG) != 0) == ((otherAllelesBasic & Allele.CG) != 0)))) {
        //                    alleleA = alleleB = thisAllelesBasic;
        //                //}
        //            } else {
        //                if ((otherAllelesBasic != Allele.Null) && otherAlleles.IsHomozygous()) {
        //                    byte matchAllele = (byte)(thisAllelesBasic & otherAllelesBasic);
        //                    if (matchAllele != Allele.Null) {
        //                        if (otherAllelesBasic == Allele.A) otherAllelesBasic = Allele.T;
        //                        else if (otherAllelesBasic == Allele.T) otherAllelesBasic = Allele.A;
        //                        else if (otherAllelesBasic == Allele.C) otherAllelesBasic = Allele.G;
        //                        else if (otherAllelesBasic == Allele.G) otherAllelesBasic = Allele.C;
        //                        else throw new ApplicationException();
        //                        matchAllele = (byte)(thisAllelesBasic & otherAllelesBasic);
        //                    }
        //                    alleleA = matchAllele;
        //                    alleleB = (byte)(thisAllelesBasic - matchAllele);
        //                }
        //            }
        //        }
        //        phasedSegment.PhaseA[idx - startIndex.Index] = alleleA;
        //        phasedSegment.PhaseB[idx - startIndex.Index] = alleleB;
        //        idx++;
        //    }
        //    this.phasedSegments.Add(phasedSegment);
        //}

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
