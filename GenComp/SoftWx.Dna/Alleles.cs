using System;

namespace SoftWx.Dna {
    /// <summary>
    /// Provides static methods that support the use of the byte type 
    /// to contain a standard representation of allele and allele
    /// pair values. The various values are specified as bit flags,
    /// allowing the combining of more than one value. The methods
    /// of this class are implemented as byte extension methods.
    /// Although this could have been implemented using a Flags enum 
    /// declaration, this seems more flexible and convenient.
    /// </summary>
    public static class Allele {
        /// <summary>Value for unset Allele.</summary>
        public const byte Null = 0;
        /// <summary>Value for the Adenine Allele.</summary>
        public const byte A = 1 << 0;
        /// <summary>Value for the Thymine Allele.</summary>
        public const byte T = 1 << 1;
        /// <summary>Value for the Cytosine Allele.</summary>
        public const byte C = 1 << 2;
        /// <summary>Value for the Guanine Allele.</summary>
        public const byte G = 1 << 3;
        /// <summary>Value for the Insertion Allele marker.</summary>
        public const byte I = 1 << 4;
        /// <summary>Value for the Deletion Allele marker.</summary>
        public const byte D = 1 << 5;
        /// <summary>Value for the Homozyous Allele pair.</summary>
        public const byte Homozygous = 1 << 6;
        /// <summary>Value for the No Call Allele marker.</summary>
        public const byte NoCall = 1 << 7;

        // standard SNP reference Allele pairs
        /// <summary>Value for the AC Allele pair.</summary>
        public const byte AC = A | C;
        /// <summary>Value for the AG Allele pair.</summary>
        public const byte AG = A | G;
        /// <summary>Value for the CT Allele pair.</summary>
        public const byte CT = C | T;
        /// <summary>Value for the GT Allele pair.</summary>
        public const byte GT = G | T;
        /// <summary>Value for the CG Allele pair.</summary>
        public const byte CG = C | G;
        /// <summary>Value for the AT Allele pair.</summary>
        public const byte AT = A | T;
        /// <summary>Value for all allele values (A, T, C, G).</summary>
        public const byte AllAlleles = AT | CG;
        /// <summary>
        /// Returns a byte representation of the specified Allele Char.
        /// </summary>
        /// <param name="source">The Char Allele.</param>
        /// <returns>A byte value representation of the specified Char Allele.</returns>
        public static byte ToAllele(this char source) {
            switch (source) {
                case 'A' : case 'a' : return A;
                case 'T' : case 't' : return T;
                case 'C' : case 'c' : return C;
                case 'G' : case 'g' : return G;
                case 'I' : case 'i' : return I;
                case 'D' : case 'd' : return D;
                case '-': return NoCall;
                default: return Null;
            }
        }

        /// <summary>
        /// Returns a byte representation of the specified Alleles String.
        /// </summary>
        /// <param name="source">The String Alleles.</param>
        /// <returns>A byte value represenation of the specified String Alleles.</returns>
        public static byte ToAlleles(this string source) {
            if (source == null) throw new ArgumentNullException("source cannot be null.");
            //if (source.Length != 2) throw new ArgumentException("source must be a 2 character string.");

            byte first = source[0].ToAllele();
            if (source.Length == 2) {
                byte second = source[1].ToAllele();
                return first.CombineAlleles(second);
            } else if (first != Allele.Null) {
                return first.CombineAlleles(Allele.NoCall);
            } else {
                return first;
            }
        }

        /// <summary>
        /// Determines if the Alleles value is Heterozygous.
        /// </summary>
        /// <param name="value">The Alleles value.</param>
        /// <returns>true if the Alleles is heterozygous, otherwise false.</returns>
        public static bool IsHeterozygous(this byte value) {
            return ((value != Allele.Homozygous) 
                    && ((((value & (Allele.AT)) != 0) && ((value & (Allele.CG)) != 0))
                        || (value == Allele.AT) || (value == Allele.CG)));
        }

        /// <summary>
        /// Determines if the Alleles value is Homozygous.
        /// </summary>
        /// <param name="value">The Alleles value.</param>
        /// <returns>true if the Alleles is homozygous, otherwise false.</returns>
        public static bool IsHomozygous(this byte value) {
            return (value & Allele.Homozygous) != 0;
        }

        /// <summary>
        /// Determines if the specified Allele source byte contains an other 
        /// specified Allele byte value. 
        /// </summary>
        /// <param name="source">The source Allele byte.</param>
        /// <param name="other">The other Allele byte.</param>
        /// <returns>True if the other Allele byte value is contained in the 
        /// source Allele byte value.</returns>
        public static bool HasAllele(this byte source, byte other) {
            if (other == Allele.Null) throw new ArgumentException("The other Allele value cannot be Allele.Null.");
            return ((source & other) == other);
        }

        /// <summary>
        /// Returns the major Allele of the specified Allele byte value.
        /// </summary>
        /// <param name="value">The Allele value to be checked.</param>
        /// <returns>The Allele byte value of the major Allele.</returns>
        public static byte MajorAllele(this byte value) {
            if ((value & A) != 0) return A;
            if ((value & C) != 0) return C;
            if ((value & G) != 0) return G;
            return Null;
        }

        /// <summary>
        /// Returns the minor Allele of the specified Allele byte value.
        /// </summary>
        /// <param name="value">The Allele value to be checked.</param>
        /// <returns>The Allele byte value of the minor Allele.</returns>
        public static byte MinorAllele(this byte value) {
            if ((value & T) != 0) return T;
            if ((value & G) != 0) return G;
            if ((value & C) != 0) return C;
            return Null;
        }

        /// <summary>
        /// Combines the two specified Allele byte values into a single
        /// Allele byte value.
        /// </summary>
        /// <param name="allele">An Allele byte value to be conbmined.</param>
        /// <param name="allele2">An Allele byte value to be combined.</param>
        /// <returns>The combined Allele byte value.</returns>
        public static byte CombineAlleles(this byte allele, byte allele2) {
            if (allele == Null) return allele;
            if (allele2 == Null) return allele2;
            if (allele == allele2) return (byte)(allele | Homozygous);
            return (byte)(allele | allele2);
        }

        ///// <summary>
        ///// Return a standardized Allele byte representation of the specified 
        ///// value, using the specified refValue as reference Allele possibilities.
        ///// </summary>
        ///// <param name="value">The Allele byte value to be standardized.</param>
        ///// <param name="refValue">The Alleles reference value.</param>
        ///// <returns>The standardized Allele byte value.</returns>
        //public static byte StandardizeAlleles(this byte value, byte refValue) {
        //    int result = value;
        //    if ((refValue != AT) && (refValue != CG)) {
        //        if (((refValue & A) == 0) && ((result & A) != 0)) {
        //            result = (result ^ A) | T;
        //        } else if (((refValue & T) == 0) && ((result & T) != 0)) {
        //            result = (result ^ T) | A;
        //        }
        //        if (((refValue & C) == 0) && ((result & C) != 0)) {
        //            result = (result ^ C) | G;
        //        } else if (((refValue & G) == 0) && ((result & G) != 0)) {
        //            result = (result ^ G) | C;
        //        }
        //    }
        //    return (byte) result;
        //}

        /// <summary>
        /// Returns the reverse the specified Allele byte value by substituting the opposite
        /// base pair Allele.
        /// </summary>
        /// <param name="value">The Allele byte value to reverse.</param>
        /// <returns>The reverse Allele(s) byte value.</returns>
        public static byte ReverseAllele(this byte value) {
            byte result = value;
            if ((value & A) != 0) result = (byte)((result ^ A) | T);
            if ((value & T) != 0) result = (byte)((result ^ T) | A);
            if ((value & C) != 0) result = (byte)((result ^ C) | G);
            if ((value & G) != 0) result = (byte)((result ^ G) | C);
            return result;
        }

        /// <summary>
        /// Returns an artificially normalized representation of the Alleles value to make
        /// comparisons easier. T is converted to A unless the value is AT, and G is converted
        /// to C unless the the value is CG. 
        /// </summary>
        /// <param name="value">The Alleles byte value to normalize.</param>
        /// <returns></returns>
        public static byte Normalize(this byte value) {
            byte result = value;
            if ((value != Allele.AT) && ((value & Allele.T) != 0)) result ^= Allele.AT;
            if ((value != Allele.CG) && ((value & Allele.G) != 0)) result ^= Allele.CG;
            return result;
        }

        /// <summary>
        /// Returns the specified Alleles byte value as a String.
        /// </summary>
        /// <param name="value">The Alleles pair byte value.</param>
        /// <returns>The String representation of the specified Alleles byte.</returns>
        public static string ToAllelesString(this byte value) {
            string s = value.ToAlleleString();
            if (s != null) {
                if ((value & Allele.Homozygous) != 0) {
                    s += s;
                } else {
                value = (byte) (value ^ s[0].ToAllele());
                s += value.ToAlleleString();
                }
            }
            return s;
        }

        /// <summary>
        /// Returns the specified Allele byte value as a String.
        /// </summary>
        /// <param name="value">The Allele byte value.</param>
        /// <returns>The String representation of the specified Allele byte.</returns>
        public static string ToAlleleString(this byte value) {
            if ((value & Allele.A) != 0) return "A";
            if ((value & Allele.C) != 0) return "C";
            if ((value & Allele.G) != 0) return "G";
            if ((value & Allele.T) != 0) return "T";
            if ((value & Allele.I) != 0) return "I";
            if ((value & Allele.D) != 0) return "D";
            if ((value & Allele.NoCall) != 0) return "-";
            return null;
        }
    }
}
