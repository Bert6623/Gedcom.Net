using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftWx.Dna {
    /// <summary>
    /// A compact struct (designed to occupy 4 bytes) to store the position
    /// of a base pair as a chromosome number and position within a list of
    /// chromosome data. The chromosome is kept as a byte, and the index 
    /// position as an unsigned 24-bit integer.
    /// </summary>
    
    public struct DnaIndex {
        private readonly byte chromosome;
        private readonly byte hIndex;    // most significant byte of unsigned 24 bit integer
        private readonly ushort lIndex;  // least significant word of unsigned 24 bit integer

        /// <summary>
        /// Creates a new instance of DnaListIndex struct.
        /// </summary>
        /// <param name="chromosome">The chromosome number.</param>
        /// <param name="index">The ordinal index position within the chromosome.</param>
        public DnaIndex(byte chromosome, int index) {
            this.chromosome = chromosome;
            this.hIndex = (byte)(index >> 16);
            this.lIndex = (ushort)(index & 0xffff);
        }

        /// <summary>
        /// Gets the chromosome number.
        /// </summary>
        public byte Chromosome { get { return this.chromosome; } }

        /// <summary>
        /// Gets the ordinal index position within the chromosome.
        /// </summary>
        public int Index { get { return (hIndex << 16) + lIndex; } }

        /// <summary>
        /// Converts the value of the current DnaListIndex struct to its equivalent string representation.
        /// </summary>
        /// <returns>The string representation of the SnpIndex.</returns>
        public override string ToString() {
            return chromosome.ToString() + ":" + Index;
        }
    }
}
