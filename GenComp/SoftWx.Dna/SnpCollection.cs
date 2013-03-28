using System;
using System.Collections.Generic;

namespace SoftWx.Dna {
    public class SnpCollection : ICollection<Snp>, IEnumerable<Snp> {
        private Dictionary<string, DnaIndex> snpLookup;
        private readonly DnaCollection<Snp> snps;
        private bool isReadOnly = false;

        /// <summary>
        /// Creates a new instance of SnpCollection using the specified range of chromosomes.
        /// </summary>
        /// <param name="firstChromosome">Chromosome number of the first chromosome included in the SnpCollection.</param>
        /// <param name="lastChromosome">Chromosome number of the last chromosome included in the SnpCollection.</param>
        public SnpCollection(int firstChromosome, int lastChromosome) {
            this.snps = new DnaCollection<Snp>(firstChromosome, lastChromosome);
            RebuildIndex();
        }

        /// <summary>
        /// Gets the number of the first chromosome contained in this DnaList.
        /// </summary>
        public byte FirstChromosome { get { return this.snps.FirstChromosome; } }

        /// <summary>
        /// Gets the number of the last chromosome contained in this DnaList.
        /// </summary>
        public byte LastChromosome { get { return this.snps.LastChromosome; } }

        /// <summary>
        /// Gets the total count of Snps across all chromosome covered by the SnpCollection.
        /// </summary>
        /// <returns>Total count of Snps in the SnpCollection.</returns>
        public int Count {
            get {
                if (this.snpLookup != null) {
                    return this.snpLookup.Count;
                } else {
                    return this.snps.Count;
                }
            }
        }

        /// <summary>
        /// Gets the count of items in the DnaList for the specified chromosome.
        /// </summary>
        /// <param name="chromosome">The number of the chromosome for which an item count is desired.</param>
        /// <returns>The count of items for the specified chromosome.</returns>
        public int CountOnChromosome(int chromosome) {
            return this.snps.CountOnChromosome(chromosome);
        }

        public int CountBetween(Snp start, Snp end) {
            DnaIndex? startIdx = GetSnpIndex(start.RsId);
            DnaIndex? endIdx = GetSnpIndex(end.RsId);
            if (startIdx.HasValue && endIdx.HasValue && (start.Chromosome == end.Chromosome)) {
                return endIdx.Value.Index - startIdx.Value.Index;
            } else {
                return 0;
            }
        }

        /// <summary>
        /// Gets the count of chromosomes covered by the DnaList.
        /// </summary>
        public int ChromosomeCount { get { return this.snps.ChromosomeCount; } }

        /// <summary>
        /// Gets the ReadOnly state of the SnpCollection.
        /// </summary>
        public bool IsReadOnly {
            get { return this.isReadOnly; }
        }

        /// <summary>
        /// Gets the Snp having the specified rsId.
        /// </summary>
        /// <param name="rsId">RsId of the desired Snp.</param>
        /// <returns>The Snp having the specified rsId.</returns>
        public Snp this[string rsId] {
            get {
                if (rsId == null) throw new ArgumentNullException("rsId cannot be null.");

                if (this.snpLookup == null) RebuildIndex();
                DnaIndex idx;                
                if (this.snpLookup.TryGetValue(rsId, out idx)) {
                    Snp snp = this.snps[idx.Chromosome, idx.Index];
                    if (snp.RsId != rsId) throw new ApplicationException("Inconsistant SnpCollection: " + rsId + " <> " + snp.RsId);
                    return snp;
                } else {
                    return null;
                }
            }
        }

        public Snp this[DnaIndex dnaIndex] {
            get {
                return this.snps[dnaIndex];
            }
        }

        public Snp this[int chromosome, int index] {
            get {
                return this.snps[chromosome, index];
            }
        }

        /// <summary>
        /// Gets the SnpIndex describing the location (chromosome number and ordinal
        /// index position) of the Snp having the specified rsId.
        /// </summary>
        /// <param name="rsId">The rsId of the Snp whose location is desired.</param>
        /// <returns>The SnpIndex for the given Snp RsId.</returns>
        public DnaIndex? GetSnpIndex(string rsId) {
            if (rsId == null) throw new ArgumentNullException("rsId cannot be null.");

            if (this.snpLookup == null) RebuildIndex();
            DnaIndex idx;
            if (this.snpLookup.TryGetValue(rsId, out idx)) {
                return idx;
            } else {
                return null;
            }
        }

        /// <summary>
        /// Determines whether this SnpCollection contains an Snp with the
        /// specified RsId.
        /// </summary>
        /// <param name="rsId">The RsId string to check.</param>
        /// <returns>True if the SnpCollection contains the RsId, otherwise false.</returns>
        public bool Contains(string rsId) {
            if (rsId == null) throw new ArgumentNullException("rsId cannot be null.");

            if (this.snpLookup == null) RebuildIndex();
            return this.snpLookup.ContainsKey(rsId);
        }

        /// <summary>
        /// Determines whether this SnpCollection contains an Snp with the
        /// same RsId as the specified Snp.
        /// </summary>
        /// <param name="snp">The Snp to check.</param>
        /// <returns>True if the SnpCollection contains an Snp with the same RsId, otherwise false.</returns>
        public bool Contains(Snp snp) {
            if (snp == null) throw new ArgumentNullException("snp cannot be null.");

            if (this.snpLookup == null) RebuildIndex();
            return snpLookup.ContainsKey(snp.RsId);
        }

        /// <summary>
        /// Extrapolate the centiMorgan position of the specified position on the specified
        /// chromosome, using the centiMorgan values of nearby Snps within this SnpCollection.
        /// </summary>
        /// <param name="chromosome">The chromosome where the position lies.</param>
        /// <param name="position">The position whose centiMorgan position is desired.</param>
        /// <returns>The extrapolated centiMorgan position of the position.</returns>
        public float ExtrapolateCentiMorganPosition(int chromosome, int position) {
            Snp snp = new Snp("temp", (byte)chromosome, position);
            Snp snp1 = null;
            Snp snp2 = null;
            int chrCount = this.snps.CountOnChromosome(chromosome);
            if (chrCount > 0) {
                var idx = this.snps.BinarySearch(chromosome, snp);
                if (idx < 0) idx = ~idx;
                if (idx < chrCount) {
                    snp2 = this.snps[chromosome, idx];
                    if (snp2.Position == position) return snp2.cM;
                }
                if (chrCount > 1) {
                    if (idx == chrCount) {
                        snp1 = this.snps[chromosome, chrCount - 2];
                        snp2 = this.snps[chromosome, chrCount - 1];
                    } else {
                        if (idx == 0) {
                            snp1 = this.snps[chromosome, 0];
                            snp2 = this.snps[chromosome, 1];
                        } else {
                            snp1 = this.snps[chromosome, idx - 1];
                            snp2 = this.snps[chromosome, idx];
                        }
                    }
                }
            }
            if ((snp1 == null) || (snp2 == null) || (snp1.cM < 0) || (snp2.cM < 0)) {
                return -1;
            } else {
                double ratio = (snp2.cM - snp1.cM) / (snp2.Position - snp1.Position);
                return (float)(snp1.cM + (position - snp1.Position) * ratio);
            }
        }

        /// <summary>
        /// Copies the Snps in this SnpCollection to the specified array, starting at
        /// the specified arrayIndex.
        /// </summary>
        /// <param name="array">The array to be copied to.</param>
        /// <param name="arrayIndex">The staring index to begin copying to.</param>
        public void CopyTo(Snp[] array, int arrayIndex) {
            if (array == null) throw new ArgumentNullException("array cannot be null.");
            if (arrayIndex < 0) throw new ArgumentOutOfRangeException("arrayIndex must be >= 0");
            if (array.Length - arrayIndex > this.Count) throw new ArgumentException("array is not large enough.");

            foreach (Snp snp in this.snps) {
                array[arrayIndex++] = snp;
            }
        }

        /// <summary>
        /// Clears the contents of this SnpCollection.
        /// </summary>
        public void Clear() {
            if (IsReadOnly) throw new InvalidOperationException("SnpCollection is ReadOnly.");

            this.snps.Clear();
            RebuildIndex();
        }

        void ICollection<Snp>.Add(Snp item) {
            Add(item);
        }

        /// <summary>
        /// Adds the specified Snp to the SnpCollection.
        /// </summary>
        /// <param name="snp">The Snp to be added.</param>
        public DnaIndex Add(Snp snp) {
            if (IsReadOnly) throw new InvalidOperationException("SnpCollection is ReadOnly.");

            byte chromosome = snp.Chromosome;
            int lastIndex = CountOnChromosome(chromosome) - 1;

            // assuming many lists are built from pre-ordered inputs, first do
            // quick check for that case - only doing a binarySearch if needed
            bool okToAdd = true;
            if (lastIndex >= 0) {
                Snp lastSnp = this.snps[chromosome, lastIndex];
                okToAdd = (snp.Position >= lastSnp.Position);
            }
            DnaIndex dnaIndex;
            if (okToAdd) {
                dnaIndex = this.snps.Add(snp, chromosome);
                if (this.snpLookup != null) this.snpLookup.Add(snp.RsId, dnaIndex);
            } else {
                // we're going to insert, which will mess up the dnaIndexes we have stored
                // in the snpLookup dictionary. So invalidate the lookup dictionary, and it
                // will be rebuilt next time it's needed.
                this.snpLookup = null;
                //while ((lastIndex >= 0) && (this.snps[chromosome, lastIndex].Position > snp.Position)) lastIndex--;
                //dnaIndex = new DnaIndex(chromosome, lastIndex + 1);
                lastIndex = this.snps.BinarySearch(chromosome, snp);
                if (lastIndex < 0) lastIndex = ~lastIndex; else lastIndex++;
                dnaIndex = new DnaIndex(chromosome, lastIndex);
                this.snps.Insert(snp, dnaIndex);
            }
            return dnaIndex;
        }

        /// <summary>
        /// Removes the specified Snp from the SnpCollection.
        /// </summary>
        /// <param name="snp">The Snp to be removed.</param>
        public bool Remove(Snp snp) {
            if (IsReadOnly) throw new InvalidOperationException("SnpCollection is ReadOnly.");
            if (snp == null) throw new ArgumentNullException("snp cannot be null.");

            if (this.snpLookup == null) RebuildIndex();
            DnaIndex dnaIndex;
            if (snpLookup.TryGetValue(snp.RsId, out dnaIndex)) {
                this.snps.RemoveAt(dnaIndex.Chromosome, dnaIndex.Index);
                this.snpLookup = null;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the SnpCollection.
        /// </summary>
        /// <returns>An IEnumerator<Snp>.</returns>
        public IEnumerator<Snp> GetEnumerator() {
            return this.snps.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the SnpCollection.
        /// </summary>
        /// <returns>An IEnumerator.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return this.snps.GetEnumerator();
        }

        private void RebuildIndex() {
            this.snpLookup = new Dictionary<string, DnaIndex>(this.Count, StringComparer.Ordinal);
            for (byte chr = this.FirstChromosome; chr <= this.LastChromosome; chr++) {
                for (int idx = 0; idx < this.CountOnChromosome(chr); idx++) {
                    DnaIndex dnaIndex = new DnaIndex(chr, idx);
                    Snp snp = snps[dnaIndex];
                    this.snpLookup.Add(snp.RsId, dnaIndex);
                }
            }
        }
    }
////*******************************************************************
////*******************************************************************
//    public class SnpCollectionOld : DnaCollection<Snp>, ICollection<Snp> {
//        private readonly Dictionary<string, DnaIndex> snpLookup;
//        private bool isReadonly = false;

//        /// <summary>
//        /// Creates a new instance of SnpCollection using the specified range of chromosomes.
//        /// </summary>
//        /// <param name="firstChromosome">Chromosome number of the first chromosome included in the SnpCollection.</param>
//        /// <param name="lastChromosome">Chromosome number of the last chromosome included in the SnpCollection.</param>
//        public SnpCollectionOld(byte firstChromosome, byte lastChromosome) : base(firstChromosome, lastChromosome) {
//            RebuildIndex();
//        }

//        /// <summary>
//        /// Gets the Snp having the specified rsId.
//        /// </summary>
//        /// <param name="rsId">RsId of the desired Snp.</param>
//        /// <returns>The Snp having the specified rsId.</returns>
//        public Snp this[string rsId] {
//            get {
//                if (rsId == null) throw new ArgumentNullException("rsId cannot be null.");

//                if (this.snpLookup == null) RebuildIndex();
//                DnaIndex idx;
//                if (snpLookup.TryGetValue(rsId, out idx)) {
//                    Snp snp = this[idx.Chromosome, idx.Index];
//                    if (snp.RsId != rsId) throw new ApplicationException("Inconsistant SnpCollection: " + rsId + " <> " + snp.RsId);
//                    return snp;
//                } else {
//                    return null;
//                }
//            }
//        }

//        /// <summary>
//        /// Gets the SnpIndex describing the location (chromosome number and ordinal
//        /// index position) of the Snp having the specified rsId.
//        /// </summary>
//        /// <param name="rsId">The rsId of the Snp whose location is desired.</param>
//        /// <returns>The SnpIndex for the given Snp RsId.</returns>
//        public DnaIndex? GetSnpIndex(string rsId) {
//            if (rsId == null) throw new ArgumentNullException("rsId cannot be null.");

//            if (this.snpLookup == null) RebuildIndex();
//            DnaIndex idx;
//            if (snpLookup.TryGetValue(rsId, out idx)) {
//                return idx;
//            } else {
//                return null;
//            }
//        }

//        /// <summary>
//        /// Determines whether this SnpCollection contains an Snp with the
//        /// specified RsId.
//        /// </summary>
//        /// <param name="rsId">The RsId string to check.</param>
//        /// <returns>True if the SnpCollection contains the RsId, otherwise false.</returns>
//        public bool Contains(string rsId) {
//            if (rsId == null) throw new ArgumentNullException("rsId cannot be null.");

//            if (this.snpLookup == null) RebuildIndex();
//            return snpLookup.ContainsKey(rsId);
//        }

//        /// <summary>
//        /// Determines whether this SnpCollection contains an Snp with the
//        /// same RsId as the specified Snp.
//        /// </summary>
//        /// <param name="snp">The Snp to check.</param>
//        /// <returns>True if the SnpCollection contains an Snp with the same RsId, otherwise false.</returns>
//        public bool Contains(Snp snp) {
//            if (snp == null) throw new ArgumentNullException("snp cannot be null.");

//            if (this.snpLookup == null) RebuildIndex();
//            return snpLookup.ContainsKey(snp.RsId);
//        }

//        /// <summary>
//        /// Clears the contents of this SnpCollection.
//        /// </summary>
//        public override void Clear() {
//            if (IsReadOnly) throw new InvalidOperationException("SnpCollection is ReadOnly.");

//            if (snpLookup != null) snpLookup.Clear();
//            base.Clear();
//            RebuildIndex();
//        }

//        /// <summary>
//        /// Adds the specified Snp to the SnpCollection.
//        /// </summary>
//        /// <param name="snp">The Snp to be added.</param>
//        public void Add(Snp snp) {
//            if (IsReadOnly) throw new InvalidOperationException("SnpCollection is ReadOnly.");

//            byte chromosome = snp.Chromosome;
//            int lastIndex = CountOnChromosome(chromosome) - 1;

//            // assuming many lists are built from pre-ordered inputs, first do
//            // quick check for that case - only doing a binarySearch if needed
//            bool okToAdd = true;
//            if (lastIndex >= 0) {
//                Snp lastSnp = this[chromosome, lastIndex];
//                okToAdd = (snp.Position >= lastSnp.Position);
//            }
//            DnaIndex dnaIndex;
//            if (okToAdd) {
//                dnaIndex = Add(snp, chromosome);
//            } else {
//                var list = this[chromosome];
//                int idx = list.BinarySearch(snp);
//                //if (idx >= 0) throw new ApplicationException();
//                if (idx < 0) idx = ~idx;
//                while ((idx < list.Count) && (list[idx].Position == snp.Position)) idx++;
//                dnaIndex = new DnaIndex(chromosome, idx);
//                Insert(snp, dnaIndex);
//                idx++;
//                while (idx < list.Count) {
//                    Snp movedSnp = list[idx];
//                    DnaIndex oldIndex = snpLookup[movedSnp.RsId];
//                    snpLookup[movedSnp.RsId] = new DnaIndex(oldIndex.Chromosome, idx);
//                    idx++;
//                }
//            }
//            snpLookup.Add(snp.RsId, dnaIndex);
//        }

//        /// <summary>
//        /// Extrapolate the centiMorgan position of the specified position on the specified
//        /// chromosome, using the centiMorgan values of nearby Snps within this SnpCollection.
//        /// </summary>
//        /// <param name="chromosome">The chromosome where the position lies.</param>
//        /// <param name="position">The position whose centiMorgan position is desired.</param>
//        /// <returns>The extrapolated centiMorgan position of the position.</returns>
//        public float ExtrapolateCentiMorganPosition(int chromosome, int position) {
//            var list = this[chromosome];
//            Snp snp = new Snp("temp", (byte)chromosome, position);
//            Snp snp1 = null;
//            Snp snp2 = null;
//            if (list.Count > 0) {
//                var idx = list.BinarySearch(snp);
//                if (idx < 0) idx = ~idx;
//                if ((idx < list.Count) && (list[idx].Position == position)) return list[idx].cM;
//                if (list.Count > 1) {
//                    if (idx == list.Count) {
//                        snp1 = list[list.Count - 2];
//                        snp2 = list[list.Count - 1];
//                    } else {
//                        if (idx == 0) {
//                            snp1 = list[0];
//                            snp2 = list[1];
//                        } else {
//                            snp1 = list[idx - 1];
//                            snp2 = list[idx];
//                        }
//                    }
//                }
//            }
//            if ((snp1 == null) || (snp2 == null)) {
//                return 0;
//            } else {
//                double ratio = (snp2.cM - snp1.cM) / (snp2.Position - snp1.Position);
//                return (float)(snp1.cM + (position - snp1.Position) * ratio);
//            }
//        }

//        /// <summary>
//        /// Removes the specified Snp from the SnpCollection.
//        /// </summary>
//        /// <param name="snp">The Snp to be removed.</param>
//        public bool Remove(Snp snp) {
//            if (IsReadOnly) throw new InvalidOperationException("SnpCollection is ReadOnly.");
//            if (snp == null) throw new ArgumentNullException("snp cannot be null.");

//            DnaIndex dnaIndex;
//            if (snpLookup.TryGetValue(snp.RsId, out dnaIndex)) {
//                snpLookup.Remove(snp.RsId);
//                RemoveAt(dnaIndex.Chromosome, dnaIndex.Index);
//                return true;
//            }
//            return false;
//        }

//        /// <summary>
//        /// Copies the Snps in this SnpCollection to the specified array, starting at
//        /// the specified arrayIndex.
//        /// </summary>
//        /// <param name="array">The array to be copied to.</param>
//        /// <param name="arrayIndex">The staring index to begin copying to.</param>
//        public void CopyTo(Snp[] array, int arrayIndex) {
//            if (array == null) throw new ArgumentNullException("array cannot be null.");
//            if (arrayIndex < 0) throw new ArgumentOutOfRangeException("arrayIndex must be >= 0");
//            if (array.Length - arrayIndex > this.Count) throw new ArgumentException("array is not large enough.");

//            foreach (Snp snp in this) {
//                array[arrayIndex++] = snp;
//            }
//        }

//        private void RebuildIndex() {
//            //this.snpLookup = new Dictionary<string, DnaIndex>(this.Count, StringComparer.Ordinal);
//            //for (byte chr = this.StartChromosome; chr <= this.EndChromosome; chr++) {
//            //    for (int idx = 0; idx < this.CountOnChromosome(chr); idx++) {
//            //        DnaIndex dnaIndex = new DnaIndex(chr, idx);
//            //        Snp snp = this[dnaIndex];
//            //        this.snpLookup.Add(snp.RsId, dnaIndex);
//            //    }
//            //}
//        }
//    }
}
