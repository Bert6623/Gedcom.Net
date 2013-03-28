using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace SoftWx.Dna {
    /// <summary>
    /// Represents mapping of SNPs to associated data values.
    /// </summary>
    public class SnpMap<T> {
        private DnaCollection<T> values;
        private SnpCollection snps;

        private SnpMap() { }

        /// <summary>
        /// Create a new instance of SnpMap.
        /// </summary>
        /// <param name="firstChromosome">Chromosome number of the first chromosome included in the DnaList.</param>
        /// <param name="lastChromosome">Chromosome number of the last chromosome included in the DnaList.</param>
        public SnpMap(int firstChromosome, int lastChromosome) {
            this.snps = new SnpCollection(firstChromosome, lastChromosome);
            this.values = new DnaCollection<T>(firstChromosome, lastChromosome);
        }

        public SnpMap(SnpCollection snps) {
            if (!snps.IsReadOnly) throw new ArgumentException("An SnpCollection must be ReadOnly to be used to construct a new SnpMap.");
            this.snps = snps;
            this.values = new DnaCollection<T>(snps.FirstChromosome, snps.LastChromosome);
            foreach (Snp snp in snps) this.values.Add(default(T), snp.Chromosome);
        }

        public int FirstChromosome { get { return this.snps.FirstChromosome; } }

        public int LastChromosome { get { return this.snps.LastChromosome; } }

        /// <summary>
        /// Gets the count of SNPs contained in this Genome.
        /// </summary>
        public int Count { get { return values.Count; } }

        /// <summary>
        /// Gets the count of items in the DnaList for the specified chromosome.
        /// </summary>
        /// <param name="chromosome">The number of the chromosome for which an item count is desired.</param>
        /// <returns>The count of items for the specified chromosome.</returns>
        public int CountOnChromosome(int chromosome) {
            if ((chromosome < this.FirstChromosome) || (chromosome > this.LastChromosome)) {
                throw new ArgumentOutOfRangeException("chromosome not covered.");
            }
            return this.snps.CountOnChromosome(chromosome);
        }
        
        /// <summary>
        /// Gets the value for the specified Snp.
        /// </summary>
        /// <param name="snp">The Snp whose associated value is desired.</param>
        /// <returns>The value for the specified Snp.</returns>
        public T this[Snp snp] {
            get {
                if (snp == null) throw new ArgumentNullException("Snp cannot be null.");
                DnaIndex? dnaIndex = this.snps.GetSnpIndex(snp.RsId);
                if (dnaIndex.HasValue) {
                    return this.values[dnaIndex.Value];
                } else {
                    return default(T);
                }
            }
            set {
                if (snp == null) throw new ArgumentNullException("Snp cannot be null.");
                DnaIndex? dnaIndex = this.snps.GetSnpIndex(snp.RsId);
                if (!dnaIndex.HasValue) {
                    throw new ArgumentOutOfRangeException("SnpMap does not contain the Snp.");
                } else {
                    this.values[dnaIndex.Value] = value;
                }
            }
        }

        public T this[DnaIndex dnaIndex] {
            get { return this.values[dnaIndex]; }
            set { this.values[dnaIndex] = value; }
        }

        /// <summary>
        /// Gets the DnaIndex describing the location (chromosome number and ordinal
        /// index position) of the Snp having the specified rsId.
        /// </summary>
        /// <param name="rsId">The rsId of the Snp whose location is desired.</param>
        /// <returns>The SnpIndex for the given Snp RsId.</returns>
        public DnaIndex? GetDnaIndex(string rsId) {
            if (rsId == null) throw new ArgumentNullException("rsId cannot be null.");

            return this.snps.GetSnpIndex(rsId); 
        }

        public Snp SnpAt(DnaIndex dnaIndex) {
            return this.snps[dnaIndex];
        }

        /// <summary>
        /// Adds the specified value for the specified Snp to this Genome.
        /// </summary>
        /// <param name="snp">The Snp being added.</param>
        /// <param name="alleles">The value to associate with the specified Snp.</param>
        public virtual void Add(Snp snp, T value) {
            if (snp == null) throw new ArgumentNullException("Snp cannot be null.");

            DnaIndex dnaIndex = this.snps.Add(snp);
            this.values.Insert(value, dnaIndex);
        }

        /// <summary>
        /// Determines whether the specified Snp is contained in this Genome.
        /// </summary>
        /// <param name="snp">The Snp to find.</param>
        /// <returns>true if this Genome contains the specified Snp, otherwise false.</returns>
        public bool Contains(Snp snp) {
            return this.snps.Contains(snp);
        }

        /// <summary>
        /// Clears all Snp and Allele data from the Genome.
        /// </summary>
        public virtual void Clear() {
            this.snps.Clear();
            this.values.Clear();
        }

        public IEnumerable<Snp> Snps { get { return this.snps; } }
        ///// <summary>
        ///// Gets an SnpCollection of the SNPs contained in this Genome.
        ///// </summary>
        ///// <returns>An SnpCollection.</returns>
        //protected SnpCollection GetSnps() {
        //    return this.snps;
        //}
    }
}
