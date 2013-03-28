using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftWx.Dna {
    public class PhasedGenome : SnpMap<PhasedGenome.Phasing> {
        public struct Phasing {
            public enum MatchKind {AxA=0, AxB=1, BxA=2, BxB=3}
            [Flags]
            public enum Match {None=0, AxA=1<<MatchKind.AxA, AxB=1<<MatchKind.AxB, 
                BxA=1<<MatchKind.BxA, BxB=1<<MatchKind.BxB}
            private readonly byte a;
            private readonly byte b;
            public Phasing(byte a, byte b) {
                this.a = a;
                this.b = b;
            }
            public byte A { get { return this.a; } }
            public byte B { get { return this.b; } }
            public byte this[int index] {
                get {
                    if (index == 0) return this.a;
                    else if (index == 1) return this.b;
                    else throw new ArgumentOutOfRangeException();
                }
            }
            public Match Compare(Phasing other) {
                Match result = Match.None;
                if (this.a == other.a) result |= Match.AxA;
                if (this.a == other.b) result |= Match.AxB;
                if (this.b == other.a) result |= Match.BxA;
                if (this.b == other.b) result |= Match.BxB;
                return result;
            }
            public override string ToString() {
                return a.ToAlleleString() + " " + b.ToAllelesString();
            }
        }

        /// <summary>
        /// Create a new instance of PhasedGenome, with a null name
        /// and Unknown GenomeType.
        /// </summary>
        /// <param name="firstChromosome">Chromosome number of the first chromosome included in the DnaList.</param>
        /// <param name="lastChromosome">Chromosome number of the last chromosome included in the DnaList.</param>
        public PhasedGenome(int firstChromosome, int lastChromosome) : this(firstChromosome, lastChromosome, null) { }

        /// <summary>
        /// Create a new instance of PhasedGenome with the specified
        /// name.
        /// </summary>
        /// <param name="firstChromosome">Chromosome number of the first chromosome included in the DnaList.</param>
        /// <param name="lastChromosome">Chromosome number of the last chromosome included in the DnaList.</param>
        /// <param name="name">The name of this Genome.</param>
        public PhasedGenome(int firstChromosome, int lastChromosome, string name) : base(firstChromosome, lastChromosome) {
            this.Name = name;
        }

        public PhasedGenome(Genome genome) : this(genome.FirstChromosome, genome.LastChromosome, "phased-" + genome.Name) {
            foreach (var snp in genome.Snps) {
                byte alleles = genome[snp];
                if (alleles.IsHomozygous()) {
                    alleles &= Allele.AllAlleles;
                    if (alleles != Allele.Null) {
                        this.Add(snp, new Phasing(alleles, alleles));
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the Name of this Genome.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Determines the matching segments shared by this and the specified PhasedGenome.
        /// </summary>
        /// <param name="other">The other Genome to compare.</param>
        /// <param name="minSnpCount">The minimum number of contiguous matching Snp allele values to be considered a valid match.</param>
        /// <param name="minCm">The minimum length of a segment in centiMorgans to be considered a valid match.</param>
        /// <param name="minStitchCm">The minimum length of each of two segments that may be stitched together when separated by a single mismatch.</param>
        /// <param name="minStitchSnpCount">The minimum number of contiguous matching SNP allele values in each of two segments that may be
        /// stitched together when separated by a single mismatch.</param>
        /// <param name="onlyFull">Specifies whether </param>
        /// <returns>An IList of SegmentMatch structs describing the matching segments that were identified.</returns>
        public IList<SegmentMatch> MatchingSegments(PhasedGenome other, int maxErrorsToStitch, int minSnpCount, double minCm,
                int minStitchSnpCount, double minStitchCm) {
            var result = new List<SegmentMatch>();
            var lastSegmentIdx = new int[4];
            for (int i = 0; i < lastSegmentIdx.Length; i++) lastSegmentIdx[i] = -1;
            var prevChr = -1;
            Snp[] start = new Snp[4];
            Snp[] end = new Snp[4];
            int[] matchCount = new int[4];
            int[] errorCount = new int[4];
            for(int i=0; i<start.Length; i++) start[i] = end[i] = null;
            for (int i = 0; i < errorCount.Length; i++) errorCount[i] = 999;
            Phasing.Match thisMatch;
            int genomeSnpCount = this.Count;
            int snpCount = 0;
            int sharedSnpCount = 0;
            foreach (var snp in this.Snps) {
                snpCount++;
                bool sameChromosome = (snp.Chromosome == prevChr);
                bool sharedSnp = other.Contains(snp);
                if (sharedSnp || !sameChromosome) {
                    if (sharedSnp) {
                        sharedSnpCount++;
                        thisMatch = this[snp].Compare(other[snp]);
                    } else {
                        thisMatch = Phasing.Match.None;
                    }
                    for (int i = 0; i < start.Length; i++) {
                        bool isMatch = ((int)thisMatch & (1 << i)) != 0;
                        if (!isMatch) {
                            switch (i) {
                                case 0: if ((this[snp].A == Allele.NoCall) || (other[snp].A == Allele.NoCall)) continue;
                                    break;
                                case 1: if ((this[snp].A == Allele.NoCall) || (other[snp].B == Allele.NoCall)) continue;
                                    break;
                                case 2: if ((this[snp].B == Allele.NoCall) || (other[snp].A == Allele.NoCall)) continue;
                                    break;
                                case 3: if ((this[snp].B == Allele.NoCall) || (other[snp].B == Allele.NoCall)) continue;
                                    break;
                            }
                        }
                        if ((start[i] != null) && ((!sameChromosome) || !isMatch)) {
                            // must end the segment being built
                            if ((matchCount[i] >= minStitchSnpCount) && (end[i].cM - start[i].cM >= minStitchCm)) {
                                // matching segment is big enough to keep
                                if (errorCount[i] <= maxErrorsToStitch) {
                                    // stitch to the previous segment
                                    var segment = result[lastSegmentIdx[i]];
                                    segment = new SegmentMatch(segment.StartSnp, end[i], segment.SnpCount + matchCount[i],
                                        segment.PhasedSnpCount + matchCount[i], 0, (Phasing.MatchKind) i, 1);
                                    result[lastSegmentIdx[i]] = segment;
                                } else {
                                    var segment = new SegmentMatch(start[i], end[i], matchCount[i], matchCount[i], 0, (Phasing.MatchKind)i, 1);
                                    result.Add(segment);
                                    lastSegmentIdx[i] = result.Count - 1;
                                }
                                errorCount[i] = 0; // will get incremented to 1 below
                            } else {
                                errorCount[i] += matchCount[i];
                            }
                            start[i] = end[i] = null;
                        }
                        if (start[i] != null) {
                            // building a segment 
                            if (isMatch) {
                                // this Snp is a match for this phase pairing
                                end[i] = snp;
                                matchCount[i]++;
                            } else {
                                // this Snp is not a match for this phase pairing
                                throw new ApplicationException("shouldn't get here");
                            }
                        } else {
                            // not building a segment
                            if (isMatch) {
                                // this Snp is a match for this phase pairing, start segment
                                start[i] = end[i] = snp;
                                matchCount[i] = 1;
                            } else {
                                // this Snp is not a match for this phase pairing
                                errorCount[i]++;
                            }
                        }
                    }
                }
                if (!sameChromosome) for (int i = 0; i < errorCount.Length; i++) errorCount[i] = 999;
                prevChr = snp.Chromosome;
            }
            // finish up any last segments on final chromosome
            for (int i = 0; i < start.Length; i++) {
                if (start[i] != null) {
                    // must end the segment being built
                    if ((matchCount[i] >= minStitchSnpCount) && (end[i].cM - start[i].cM >= minStitchCm)) {
                        // matching segment is big enough to keep
                        if (errorCount[i] <= 1) {
                            // stitch to the previous segmetn
                            var segment = result[lastSegmentIdx[i]];
                            segment = new SegmentMatch(segment.StartSnp, end[i], segment.SnpCount + matchCount[i],
                                segment.PhasedSnpCount + matchCount[i], 0, (Phasing.MatchKind) i, 1);
                            result[lastSegmentIdx[i]] = segment;
                        } else {
                            var segment = new SegmentMatch(start[i], end[i], matchCount[i], matchCount[i], 0, (Phasing.MatchKind) i, 1);
                            result.Add(segment);
                        }
                    }
                }
            }
            // prune segments that are too small
            List<SegmentMatch> filteredResult = new List<SegmentMatch>();
            if ((minSnpCount != minStitchSnpCount) || (minCm != minStitchCm)) {
                foreach (var match in result) {
                    if ((match.SnpCount >= minSnpCount) && (match.CmLength >= minCm)) {
                        filteredResult.Add(match);
                    }
                }
                result = filteredResult;
            }
            //// filter out duplicate matches
            //var ordered = result.OrderByDescending(s=>s.CmLength).ToList();
            //result = new List<SegmentMatch>();
            //for (int i = 0; i < ordered.Count; i++) {
            //    SegmentMatch segment = ordered[i];
            //    result.Add(segment);
            //    for (int j = ordered.Count - 1; j > i ; j--) {
            //        var segment2 = ordered[j];
            //        if (segment2.StartSnp.Chromosome == segment.StartSnp.Chromosome) {
            //            if ((segment2.StartSnp.Position <= segment.EndSnp.Position)
            //                && (segment2.EndSnp.Position >= segment.StartSnp.Position)) {
            //                bool segIsA = (segment.MatchKind == Phasing.MatchKind.AxA) || (segment.MatchKind == Phasing.MatchKind.AxB);
            //                bool seg2IsA = (segment2.MatchKind == Phasing.MatchKind.AxA) || (segment2.MatchKind == Phasing.MatchKind.AxB);
            //                if ((segIsA && seg2IsA) || ((!segIsA) && (!seg2IsA))) {
            //                    ordered.RemoveAt(j);
            //                }
            //            }
            //        }
            //    }
            //}
            return result;
        }

        public void AddPhasing(Genome thisFullGenome, Genome otherFullGenome, IList<SegmentMatch> sharedSegments, 
                int minSnpCount, int edgeWasteCount, Phasing.Match matchSides, bool fillNoCalls, List<Tuple<SegmentMatch, 
                SegmentMatch>> results) {
            var snpQuick = new HashSet<Snp>(this.Snps);
            var taboo = new HashSet<SegmentMatch>();
            var modifyList = new List<KeyValuePair<Snp, Phasing>>(50);
            var insertList = new List<KeyValuePair<Snp, Phasing>>(100);
            foreach (var segment in sharedSegments.OrderByDescending(s => s.CmLength)) {
                if ((((1 << (int) segment.MatchKind) & (int) matchSides) != 0) && (!taboo.Contains(segment))) {
                    DnaIndex? start = thisFullGenome.GetDnaIndex(segment.StartSnp.RsId);
                    DnaIndex? end = thisFullGenome.GetDnaIndex(segment.EndSnp.RsId);
                    if (start.HasValue && end.HasValue) {
                        byte chr = start.Value.Chromosome;
                        int startIdx = start.Value.Index + edgeWasteCount;
                        int endIdx = end.Value.Index - edgeWasteCount;
                        if (segment.CmLength >= 20) {
                            if (this.SnpAt(new DnaIndex(segment.StartSnp.Chromosome, 0)) == segment.StartSnp) {
                                startIdx -= edgeWasteCount;
                            }
                            if (this.SnpAt(new DnaIndex(segment.EndSnp.Chromosome, this.CountOnChromosome(segment.EndSnp.Chromosome) - 1)) == segment.EndSnp) {
                                endIdx += edgeWasteCount;
                            }
                        }
                        int compareCount = 0;
                        for (int i = startIdx; i < endIdx; i++) {
                            DnaIndex dnaIndex = new DnaIndex(chr, i);
                            Snp snp = thisFullGenome.SnpAt(dnaIndex);
                            if (otherFullGenome.Contains(snp)) {
                                byte otherAlleles = otherFullGenome[snp];
                                byte thisAlleles = thisFullGenome[dnaIndex];
                                if ((otherAlleles & Allele.AllAlleles) != Allele.Null) {
                                    compareCount++;
                                    if (otherAlleles.IsHomozygous()) { //&& (thisAlleles.IsHeterozygous() || ((thisAlleles & Allele.NoCall) != 0))
                                        bool sideA = ((segment.MatchKind == Phasing.MatchKind.AxA) || (segment.MatchKind == Phasing.MatchKind.AxB));
                                        byte commonAllele = (byte)(otherAlleles & Allele.AllAlleles);
                                        if (snpQuick.Contains(snp)) {
                                            Phasing phasing = this[snp];
                                            if ((phasing.A != Allele.NoCall) || (phasing.B != Allele.NoCall)) {
                                                byte target = (sideA) ? phasing.A : phasing.B;
                                                if (target == Allele.NoCall) {
                                                    if (fillNoCalls) {
                                                        if (sideA) {
                                                            phasing = new Phasing(commonAllele, phasing.B);
                                                        } else {
                                                            phasing = new Phasing(phasing.A, commonAllele);
                                                        }
                                                        modifyList.Add(new KeyValuePair<Snp, Phasing>(snp, phasing));
                                                    }
                                                } else {
                                                    // verify
                                                    if (target != commonAllele) {
                                                        modifyList.Add(new KeyValuePair<Snp, Phasing>(snp, new Phasing(Allele.NoCall, Allele.NoCall)));
                                                    }
                                                }
                                            }
                                        } else {
                                            if (fillNoCalls) {
                                                thisAlleles = (byte)((thisAlleles & Allele.NoCall) | (thisAlleles & Allele.AllAlleles));
                                            } else {
                                                thisAlleles = (byte)(thisAlleles & Allele.AllAlleles);
                                            }
                                            Phasing phasing;
                                            if (sideA) {
                                                phasing = new Phasing(commonAllele, (byte)(thisAlleles & ~commonAllele));
                                            } else {
                                                phasing = new Phasing((byte)(thisAlleles & ~commonAllele), commonAllele);
                                            }
                                            // add phased snp values
                                            insertList.Add(new KeyValuePair<Snp, Phasing>(snp, phasing));
                                            snpQuick.Add(snp);
                                        }
                                    }
                                }
                            }
                        }
                        if ((results != null) && (startIdx < endIdx)) {
                            Snp startSnp = thisFullGenome.SnpAt(new DnaIndex(chr, startIdx));
                            Snp endSnp = thisFullGenome.SnpAt(new DnaIndex(chr, endIdx));
                            var resultInfo = new SegmentMatch(startSnp, endSnp, compareCount, insertList.Count,0, Phasing.MatchKind.AxA, 0);
                            results.Add(new Tuple<SegmentMatch, SegmentMatch>(segment, resultInfo));
                        }
                        foreach (var seg2 in sharedSegments) {
                            if (seg2.StartSnp.Chromosome == segment.StartSnp.Chromosome) {
                                if ((segment.StartSnp.Position < seg2.EndSnp.Position)
                                    && (segment.EndSnp.Position > seg2.StartSnp.Position)) {
                                    taboo.Add(seg2);
                                }
                            }
                        }
                    } else {
                        throw new ApplicationException("shoudn't be here");
                    }
                }
            }
            foreach (var pair in insertList) {
                this.Add(pair.Key, pair.Value);
            }
            foreach (var pair in modifyList) {
                this[pair.Key] = pair.Value;
            }
        }

        public IList<Tuple<SegmentMatch, SegmentMatch>> MatchPhase(PhasedGenome other, Genome thisFullGenome, Genome otherFullGenome, 
                int maxErrorsToStitch, int minSnpCount, double minCm, int minStitchSnpCount, double minStitchCm,
                int edgeWasteCount, bool fillNoCalls) {
            var results = new List<Tuple<SegmentMatch, SegmentMatch>>();
            var segments = MatchingSegments(other, maxErrorsToStitch, minSnpCount, minCm, minStitchSnpCount, minStitchCm);
            double aCm, bCm;
            aCm = bCm = 0;
            SegmentMatch prevSegment = new SegmentMatch(null, null, 0, 0, 0, Phasing.MatchKind.AxA, 0);
            int uniqueCount = 0;
            foreach(var segment in segments) {
                if ((segment.MatchKind == Phasing.MatchKind.AxA) || (segment.MatchKind == Phasing.MatchKind.AxB)) {
                    aCm += segment.CmLength;
                } else if ((segment.MatchKind == Phasing.MatchKind.BxA) || (segment.MatchKind == Phasing.MatchKind.BxB)) {
                    bCm += segment.CmLength;
                }
                if ((segment.StartSnp != prevSegment.StartSnp) || (segment.EndSnp != prevSegment.EndSnp)) uniqueCount++;
                prevSegment = segment;
            }
            if (aCm >= bCm) {
                //segments = segments.Where(s=>(s.MatchKind == Phasing.MatchKind.AxA) || (s.MatchKind == Phasing.MatchKind.AxB)).ToList();
                AddPhasing(thisFullGenome, otherFullGenome, segments, minSnpCount, edgeWasteCount, Phasing.Match.AxA | Phasing.Match.AxB, fillNoCalls, results);
            } else {
                //segments = segments.Where(s=>(s.MatchKind == Phasing.MatchKind.BxA) || (s.MatchKind == Phasing.MatchKind.BxB)).ToList();
                AddPhasing(thisFullGenome, otherFullGenome, segments, minSnpCount, edgeWasteCount, Phasing.Match.BxA | Phasing.Match.BxB, fillNoCalls, results);
            }
            //if (uniqueCount * 4 != segments.Count) {
                segments = MatchingSegments(other, maxErrorsToStitch, minSnpCount, minCm, minStitchSnpCount, minStitchCm);
            //}
            if (aCm >= bCm) {
                //segments = segments.Where(s=>(s.MatchKind == Phasing.MatchKind.BxA) || (s.MatchKind == Phasing.MatchKind.BxB)).ToList();
                AddPhasing(thisFullGenome, otherFullGenome, segments, minSnpCount, edgeWasteCount, Phasing.Match.BxA | Phasing.Match.BxB, fillNoCalls, results);
            } else {
                //segments = segments.Where(s=>(s.MatchKind == Phasing.MatchKind.AxA) || (s.MatchKind == Phasing.MatchKind.AxB)).ToList();
                AddPhasing(thisFullGenome, otherFullGenome, segments, minSnpCount, edgeWasteCount, Phasing.Match.AxA | Phasing.Match.AxB, fillNoCalls, results);
            }
            return results;
        }

        public Tuple<Genome, Genome> Split(Genome full, bool clone) {
            var genomeA = new Genome(full.FirstChromosome, full.LastChromosome);
            var genomeB = new Genome(full.FirstChromosome, full.LastChromosome);
            var result = new Tuple<Genome, Genome>(genomeA, genomeB);
            byte a, b;
            foreach (var snp in full.Snps) {
                if (this.Contains(snp)) {
                    var phasing = this[snp];
                    a = phasing.A;
                    b = phasing.B;
                    if (clone) {
                        a |= Allele.Homozygous;
                        b |= Allele.Homozygous;
                    }
                } else {
                    byte fullAlleles = full[snp];
                    if (fullAlleles == (Allele.NoCall | Allele.Homozygous)) {
                        a = b = Allele.NoCall;
                    } else {
                        a = b = Allele.Null;
                    }
                }
                genomeA.Add(snp, a);
                genomeB.Add(snp, b);
            }
            return result;
        }

        public Tuple<Genome, Genome> Split(bool clone) {
            var genomeA = new Genome(FirstChromosome, LastChromosome);
            var genomeB = new Genome(FirstChromosome, LastChromosome);
            var result = new Tuple<Genome, Genome>(genomeA, genomeB);
            byte a, b;
            foreach (var snp in this.Snps) {
                var phasing = this[snp];
                a = phasing.A;
                b = phasing.B;
                if (clone) {
                    a |= Allele.Homozygous;
                    b |= Allele.Homozygous;
                }
                genomeA.Add(snp, a);
                genomeB.Add(snp, b);
            }
            return result;
        }
    }
}
