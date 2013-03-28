using System;

namespace SoftWx.Dna {
    /// <summary>
    /// The Snp class encapsulate information about an SNP (Single Nucleotide
    /// Polymorphism).
    /// </summary>
    public class Snp : IComparable {
        /// <summary>The reference Id of the Snp, typically the letters "rs" followed by a unique number.</summary>
        public string RsId { get; private set; }
        /// <summary>The Id assigned to the Snp by the Alfred website.</summary>
        public string AlfredId { get; set; }
        /// <summary>The reference alleles for the Snp.</summary>
        public byte Alleles { get; set; }
        /// <summary>The number of the chromosome where the Snp is located.</summary>
        public byte Chromosome { get; set; }
        /// <summary>The logical centiMorgan location of the Snp on the chromosome.</summary>
        public float cM { get; set; }
        /// <summary>The physical location of the Snp on the chromosome.</summary>
        public int Position { get; set; }
        /// <summary>The SoftWx.Dna assigned Id of the Snp, typically the number following the "rs" from the RsId.</summary>
        public int Id { get; private set; }

        private Snp() { }

        /// <summary>
        /// Create a new instance of Snp using the specified values.
        /// </summary>
        /// <param name="rsId">The RsId.</param>
        /// <param name="chromosome">The chromosome where the Snp is located.</param>
        /// <param name="position">The Snp's physical position on the chromosome.</param>
        public Snp(string rsId, byte chromosome, int position) : this(rsId, chromosome, position, -1, null, null) { }

        /// <summary>
        /// Create a new instance of Snp using the specified values.
        /// </summary>
        /// <param name="rsId">The RsId.</param>
        /// <param name="chromosome">The chromosome where the Snp is located.</param>
        /// <param name="position">The Snp's physical position on the chromosome.</param>
        /// <param name="cM">The Snp's logical centiMorgan position on the chromosome.</param>
        /// <param name="alfredId">The AlfredId.</param>
        /// <param name="alleles">The Alleles byte.</param>
        public Snp(string rsId, byte chromosome, int position, float cM, string alfredId, string alleles) {
            if (rsId == null) throw new ArgumentNullException("rsId cannot be null.");

            this.RsId = rsId;
            this.Chromosome = chromosome;
            this.Position = position;
            this.AlfredId = alfredId;
            this.cM = cM;
            SetAlleles(alleles);
            ComputeId();
        }

        /// <summary>
        /// Returns the byte representation of a string chromosome number.
        /// In addtion to the straightforward converstion of "1" through "22" 
        /// into 1-22, "X" returns 23, "Y" returns 24, and "MT" returns 25.
        /// </summary>
        /// <param name="chromosome">The chromosome number as a string.</param>
        /// <returns>The chromosome number as a byte? if it can be converted, otherwise null.</returns>
        public static byte? ChromosomeToByte(string chromosome) {
            if (chromosome == null) throw new ArgumentNullException("chromosome cannot be null.");

            byte? chr = null;
            if (chromosome == "X") {
                chr = 23;
            } else if (chromosome == "XY") {
                chr = 24;
            } else if (chromosome == "Y") {
                chr = 25;
            } else if (chromosome == "MT") {
                chr = 26;
            } else {
                for (int i = 0; i < chromosome.Length; i++) {
                    if (Char.IsDigit(chromosome[i])) {
                        chr = Convert.ToByte(chromosome.Substring(i));
                        break;
                    }
                }
            }
            return chr;
        }

        /// <summary>
        /// Returns the byte representation of a string chromosome number.
        /// In addtion to the straightforward converstion of "1" through "22" 
        /// into 1-22, "X" returns 23, "Y" returns 24, and "MT" returns 25.
        /// </summary>
        /// <param name="chromosome">The chromosome number as a string.</param>
        /// <returns>The chromosome number as a byte? if it can be converted, otherwise null.</returns>
        public static string ChromosomeToString(int chromosome) {
            if (chromosome < 1) throw new ArgumentOutOfRangeException("chromosome must be greater than 0.");

            string result = null;
            if ((chromosome > 0) && (chromosome <= 22)) {
                result = chromosome.ToString();
            } else if (chromosome == 23) {
                result = "X";
            } else if (chromosome == 24) {
                result = "XY";
            } else if (chromosome == 25) {
                result = "Y";
            } else if (chromosome == 26) {
                result = "MT";
            }
            return result;
        }

        /// <summary>
        /// Returns the hash code for this Snp.
        /// </summary>
        /// <returns>The int hash code.</returns>
        public override int GetHashCode() {
            return this.Id;
        }

        /// <summary>
        /// Determines whether the specified Object is equal to this Snp.
        /// </summary>
        /// <param name="obj">The object to compare with this Snp.</param>
        /// <returns>true if the specified Object is equal to this Snp; otherwise, false.</returns>
        public override bool Equals(object obj) {
            if (obj == null) return false;
            Snp other = obj as Snp;
            return ((other != null) && this.Equals(other));
        }

        /// <summary>
        /// Determines whether the specified Snp is equal to this Snp.
        /// </summary>
        /// <param name="other">The Snp to compare with this Snp.</param>
        /// <returns>true if the specified Snp is equal to this Snp; otherwise, false.</returns>
        public bool Equals(Snp other) {
            return ((other != null) && (this.Chromosome == other.Chromosome)
                && (this.Position == other.Position) && (this.Id == other.Id));
        }

        /// <summary>
        /// Returns the string representation of this Snp.
        /// </summary>
        /// <returns>The string representation of this Snp.</returns>
        public override string ToString() {
            return RsId + " @ " + Chromosome + ":" + Position.ToString("#,###");
        }

        //public static bool operator ==(Snp lhs, Snp rhs) {
        //    return ((object)rhs != null) && lhs.Equals(rhs);
        //}

        //public static bool operator !=(Snp lhs, Snp rhs) {
        //    return ((object)rhs == null) || !lhs.Equals(rhs);
        //}

        /// <summary>
        /// Compares an Snp to another by comparing the chromosome and physical position of the Snps.
        /// </summary>
        /// <param name="obj">An object to compare.</param>
        /// <returns>0 if both Snps have the same chromome and position, -1 if this Snp is 
        /// at a lower chromosome:position, 1 if this Snp is at a higher chromosome:position.</returns>
        int IComparable.CompareTo(object obj) {
            if (obj is Snp) {
                Snp snp = (Snp)obj;
                int result = this.Chromosome.CompareTo(snp.Chromosome);
                if (result == 0) {
                    result = this.Position.CompareTo(snp.Position);
                    if (result == 0) {
                        result = this.RsId.CompareTo(snp.RsId);
                    }
                }
                return result;
            } else {
                throw new ArgumentException("Object is not an Snp.");
            }
        }

        private void SetAlleles(string alleles) {
            Alleles = Allele.Null;
            if (alleles != null) {
                alleles = alleles.Replace("/", "").ToUpper();
                if (alleles.Length > 0) {
                    Alleles = alleles[0].ToAllele();
                    if (alleles.Length > 1) {
                        RefineAllele(alleles[1]);
                    }
                }
            }
        }

        private void ComputeId() {
            if ((RsId.Length > 2) && RsId.StartsWith("rs", StringComparison.OrdinalIgnoreCase)) {
                Id = Int32.Parse(RsId.Substring(2));
            } else {
                Id = RsId.GetHashCode();
                if (Id > 0) {
                    Id = -Id;
                } else if (Id == 0) {
                    Id = -1;
                }
            }
        }

        private void RefineAllele(char allele) {
            byte allele2 = allele.ToAllele();
            Alleles = Alleles.CombineAlleles(allele2);
        }
    }
}
