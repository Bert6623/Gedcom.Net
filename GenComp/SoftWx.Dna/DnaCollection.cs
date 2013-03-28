using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftWx.Dna {
    public class DnaCollection<T> : IEnumerable<T> {
        private byte firstChromosome;
        private byte lastChromosome;
        private readonly List<T>[] chromosomes;
        private bool isReadOnly = false;

        private DnaCollection() { }

        /// <summary>
        /// Creates a new instance of DnaList using the specified range of chromosomes.
        /// </summary>
        /// <param name="firstChromosome">Chromosome number of the first chromosome included in the DnaList.</param>
        /// <param name="lastChromosome">Chromosome number of the last chromosome included in the DnaList.</param>
        public DnaCollection(int firstChromosome, int lastChromosome) {
            if ((firstChromosome < 0) || (firstChromosome > 255)) throw new ArgumentOutOfRangeException("firstChromosome must be 0 to 255.");
            if ((lastChromosome < 0) || (lastChromosome > 255)) throw new ArgumentOutOfRangeException("lastChromosome must be 0 to 255.");
            if (firstChromosome > lastChromosome) throw new ArgumentOutOfRangeException("firstChromsome must be <= lastChromosome.");

            this.firstChromosome = (byte) firstChromosome;
            this.lastChromosome = (byte) lastChromosome;
            this.chromosomes = new List<T>[lastChromosome - firstChromosome + 1];
            for (int i = 0; i < chromosomes.Length; i++) {
                chromosomes[i] = new List<T>();
            }
        }
        /// <summary>
        /// Gets the number of the first chromosome contained in this DnaList.
        /// </summary>
        public byte FirstChromosome { get { return this.firstChromosome; } }

        /// <summary>
        /// Gets the number of the last chromosome contained in this DnaList.
        /// </summary>
        public byte LastChromosome { get { return this.lastChromosome; } }

        /// <summary>
        /// Gets the item at the chromosome and ordinal index specified by a DnaListIndex.
        /// </summary>
        /// <param name="idx">DnaListIndex specifying the chromosome and ordinal index of the desired item.</param>
        /// <returns>The item at the chromosome and ordinal index specified by the DnaListIndex.</returns>
        public T this[DnaIndex idx] {
            get { return this[idx.Chromosome, idx.Index]; }
            set { this[idx.Chromosome, idx.Index] = value; }
        }

        /// <summary>
        /// Gets the item at the specified ordinal index of the specified chromosome.
        /// </summary>
        /// <param name="chromosome">Number of the chromosome containing the desired item.</param>
        /// <param name="index">Ordinal index position of the desired item within the collection 
        /// of items for the specified chromosome.</param>
        /// <returns>The item at the specified index of the specified chromosome.</returns>
        public T this[int chromosome, int index] {
            get {
                if ((chromosome < this.firstChromosome) || (chromosome > this.lastChromosome)) throw new ArgumentOutOfRangeException("chromosome is not covered by the DnaList.");
                var chrList = this.chromosomes[chromosome - this.firstChromosome];
                if ((index < 0) || (index >= chrList.Count)) throw new ArgumentOutOfRangeException("index is outside the bounds of the chromosome.");

                return chrList[index];
            }
            set {
                if ((chromosome < this.firstChromosome) || (chromosome > this.lastChromosome)) throw new ArgumentOutOfRangeException("chromosome is not covered by the DnaList.");
                var chrList = this.chromosomes[chromosome - this.firstChromosome];
                if ((index < 0) || (index >= chrList.Count)) throw new ArgumentOutOfRangeException("index is outside the bounds of the chromosome.");

                chrList[index] = value;
            }
        }

        /// <summary>
        /// Gets the total count of items across all chromosome covered by the DnaList.
        /// </summary>
        /// <returns>Total count of items in the DnaList.</returns>
        public int Count {
            get {
                int count = 0;
                foreach (var chromosome in this.chromosomes) count += chromosome.Count;
                return count;
            }
        }

        /// <summary>
        /// Gets the count of items in the DnaList for the specified chromosome.
        /// </summary>
        /// <param name="chromosome">The number of the chromosome for which an item count is desired.</param>
        /// <returns>The count of items for the specified chromosome.</returns>
        public int CountOnChromosome(int chromosome) {
            if ((chromosome < this.firstChromosome) || (chromosome > this.lastChromosome)) {
                throw new ArgumentOutOfRangeException("chromosome not covered by the DnaList.");
            }
            return this.chromosomes[chromosome - this.firstChromosome].Count;
        }

        /// <summary>
        /// Gets the count of chromosomes covered by the DnaList.
        /// </summary>
        public int ChromosomeCount { get { return this.lastChromosome - this.firstChromosome + 1; } }

        /// <summary>
        /// Gets the ReadOnly state of the SnpCollection.
        /// </summary>
        public bool IsReadOnly {
            get { return this.isReadOnly; }
        }

        /// <summary>
        /// Searches the items in the specified chromosome for index of the specified item. Assumes the items
        /// are in sorted order.
        /// </summary>
        /// <param name="chromosome">The chromosome to search.</param>
        /// <param name="target">The item to search for.</param>
        /// <returns>The zero-based index of item if item is found; otherwise, a negative number that is the 
        /// bitwise complement of the index of the next element that is larger than item or, if there is no \
        /// larger element, the bitwise complement of Count.</returns>
        public int BinarySearch(int chromosome, T target) {
            if ((chromosome < this.firstChromosome) || (chromosome > this.lastChromosome)) {
                throw new ArgumentOutOfRangeException("chromosome not covered by the DnaList.");
            }

            return this.chromosomes[chromosome - this.firstChromosome].BinarySearch(target);
        }

        /// <summary>
        /// Make this DnaCollection read only (Clear, Adds, Removes, and item sets will no
        /// longer be allowed). Once made read only, the collection can not be set back to
        /// being not read only.
        /// </summary>
        public void MakeReadOnly() {
            this.isReadOnly = true;
        }

        /// <summary>
        /// Clears the contents of this DnaList.
        /// </summary>
        public virtual void Clear() {
            if (IsReadOnly) throw new InvalidOperationException("DnaCollection is ReadOnly.");
            for (int i = 0; i < this.chromosomes.Length; i++) {
                chromosomes[i].Clear();
            }
        }

        /// <summary>
        /// Adds the specified item to the DnaList.
        /// </summary>
        /// <param name="snp">The item to be added.</param>
        public DnaIndex Add(T item, int chromosome) {
            if (IsReadOnly) throw new InvalidOperationException("DnaCollection is ReadOnly.");
            if ((chromosome < this.firstChromosome) || (chromosome > this.lastChromosome)) {
                throw new ArgumentOutOfRangeException("chromosome not covered by the DnaList.");
            }
            List<T> list = chromosomes[chromosome - this.firstChromosome];
            list.Add(item);
            return new DnaIndex((byte) chromosome, list.Count - 1);
        }

        /// <summary>
        /// Inserts the specified item into the DnaList at the chromosome and
        /// position indicated by the specified DnaListIndex.
        /// </summary>
        /// <param name="idx">DnaListIndex specifying the chromosome and ordinal index where
        /// the item will be inserted.</param>
        public void Insert(T item, DnaIndex idx) {
            Insert(item, idx.Chromosome, idx.Index);
        }

        /// <summary>
        /// Inserts the specified item into the DnaList at the specified chromosome
        /// and position.
        /// </summary>
        /// <param name="item">The item to be inserted.</param>
        /// <param name="chromosome">Number of the chromosome where the item will be inserted.</param>
        /// <param name="index">Ordinal index position within the collection of items for the
        public void Insert(T item, int chromosome, int index) {
            if (IsReadOnly) throw new InvalidOperationException("DnaCollection is ReadOnly.");
            if ((chromosome < this.firstChromosome) || (chromosome > this.lastChromosome)) {
                throw new ArgumentOutOfRangeException("chromosome not covered by the DnaList.");
            }
            List<T> list = this.chromosomes[chromosome - this.firstChromosome];
            list.Insert(index, item);
        }

        /// <summary>
        /// Remove the item at the specified DnaIndex.
        /// </summary>
        /// <param name="idx">DnaIndex specifying the location of the item to be removed.</param>
        public void RemoveAt(DnaIndex idx) {
            RemoveAt(idx.Chromosome, idx.Index);
        }

        /// <summary>
        /// Remove the item at the specified chromosome and index on that chromosome.
        /// </summary>
        /// <param name="chromosome">The chromosome where the item to be removed is located.</param>
        /// <param name="index">The index on the chromosome of the item to be removed.</param>
        public void RemoveAt(int chromosome, int index) {
            if (IsReadOnly) throw new InvalidOperationException("DnaCollection is ReadOnly.");
            if ((chromosome < this.firstChromosome) || (chromosome > this.lastChromosome)) {
                throw new ArgumentOutOfRangeException("chromosome not covered by the DnaList.");
            }
            this.chromosomes[chromosome - this.firstChromosome].RemoveAt(index);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the SnpCollection.
        /// </summary>
        /// <returns>An IEnumerator<Snp>.</returns>
        public IEnumerator<T> GetEnumerator() {
            foreach (var chrList in this.chromosomes) {
                foreach (T item in chrList) {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the SnpCollection.
        /// </summary>
        /// <returns>An IEnumerator.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        protected List<T> this[int chromosome] {
            get {
                if ((chromosome < this.firstChromosome) || (chromosome > this.lastChromosome)) {
                    throw new ArgumentOutOfRangeException("chromosome not covered by the DnaList.");
                }
                return this.chromosomes[chromosome - this.firstChromosome]; 
            } 
        }
    }
}
