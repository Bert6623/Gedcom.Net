using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace SoftWx.Dna {
    /// <summary>
    /// Static class containing methods for reading and writing to file formats
    /// supported by the SoftWx.Dna library.
    /// </summary>
    public static class SnpFile {
        #region SnpCollection Files
        // SnpCollection files are a custom text file format defined by the SoftWx.Dna library. 
        // The format uses one text line for each Snp element in the SnpCollection where each
        // line is CSV (comma delimited) text of the Snp properties.

        /// <summary>
        /// Read data into the specified SnpCollection from the SnpCollection
        /// file having the specified filename (which may include a path).
        /// </summary>
        /// <param name="snps">The SnpCollection to receive the read data.</param>
        /// <param name="filename">The path and filename of the SnpCollection file.</param>
        public static void Read(SnpCollection snps, string filename) {
            if (snps == null) throw new ArgumentNullException("The SnpCollection cannot be null.");
            if (filename == null) throw new ArgumentNullException("filename cannot be null.");
            if (String.IsNullOrWhiteSpace(filename)) throw new ArgumentOutOfRangeException("filename cannot be empty.");

            Read(snps, filename, new CancellationToken(false) , null);
        }

        /// <summary>
        /// Read data into the specified SnpCollection from the SnpCollection
        /// file having the specified filename (which may include a path). 
        /// The specified CancellationToken can be used to abort the read. 
        /// The progress parameter will be updated by this method with a 
        /// value of 0-100 to reflect the percent progress of the read.
        /// </summary>
        /// <param name="snps">The SnpCollection to receive the read data.</param>
        /// <param name="filename">The path and filename of the SnpCollection file.</param>
        /// <param name="cancel">The CancellationToken that can be used to abort the read.</param>
        /// <param name="progress">The progress parameter that will be updated to reflect 
        /// the percent progress of the read.</param>
        public static void Read(SnpCollection snps, string filename, CancellationToken cancel, Progress progress) {
            if (snps == null) throw new ArgumentNullException("The SnpCollection cannot be null.");
            if (filename == null) throw new ArgumentNullException("filename cannot be null.");
            if (String.IsNullOrWhiteSpace(filename)) throw new ArgumentOutOfRangeException("filename cannot be empty.");

            using (StreamReader reader = new StreamReader(filename)) {
                long length = 0;
                if (progress != null) length = reader.BaseStream.Length;
                string[] columns = new string[6];
                string line;
                while ((line = reader.ReadLine()) != null) {
                    cancel.ThrowIfCancellationRequested();
                    line.FastSplit(',', columns);
                    byte chr = Convert.ToByte(columns[2]);
                    if (chr == 0) chr = 23; // handles legacy use of 0 for X 
                    Snp snp;
                    if ((!String.IsNullOrWhiteSpace(columns[3]) && Char.IsDigit(columns[3][0]))) {
                        // new format snp file
                        snp = new Snp(columns[0], chr, Convert.ToInt32(columns[3]), Convert.ToSingle(columns[4]), columns[1], columns[5]);
                    } else {
                        // old SnpMap format snp file
                        snp = new Snp(columns[0], chr, -1, -1, columns[1], columns[3]);
                    }
                    snps.Add(snp);
                    if (progress != null) progress.Set(reader.BaseStream.Position, length);

                }
            }
        }

        /// <summary>
        /// Write all data from the specified SnpCollection into the SnpCollection
        /// file having the specified filename (which may include a path). Any
        /// existing contents of the file will be overwritten.
        /// </summary>
        /// <param name="snps">The SnpCollection containing the data to be written.</param>
        /// <param name="filename">The path and filename of the SnpCollection file.</param>
        public static void Write(SnpCollection snps, string filename) {
            if (snps == null) throw new ArgumentNullException("The SnpCollection cannot be null.");
            if (filename == null) throw new ArgumentNullException("filename cannot be null.");
            if (String.IsNullOrWhiteSpace(filename)) throw new ArgumentOutOfRangeException("filename cannot be empty.");

            Write(snps, filename, new CancellationToken(false), null);
        }

        /// <summary>
        /// Write all data from the specified SnpCollection into the SnpCollection
        /// file having the specified filename (which may include a path). Any
        /// existing contents of the file will be overwritten.
        /// The specified CancellationToken can be used to abort the read. 
        /// The progress parameter will be updated by this method with a 
        /// value of 0-100 to reflect the percent progress of the read.
        /// </summary>
        /// <param name="snps">The SnpCollection containing the data to be written.</param>
        /// <param name="filename">The path and filename of the SnpCollection file.</param>
        /// <param name="cancel">The CancellationToken that can be used to abort the write.</param>
        /// <param name="progress">The progress parameter that will be updated to reflect 
        /// the percent progress of the write.</param>
        public static void Write(SnpCollection snps, string filename, CancellationToken cancel, Progress progress) {
            if (snps == null) throw new ArgumentNullException("The SnpCollection cannot be null.");
            if (filename == null) throw new ArgumentNullException("filename cannot be null.");
            if (String.IsNullOrWhiteSpace(filename)) throw new ArgumentOutOfRangeException("filename cannot be empty.");
         
            using (StreamWriter writer = new StreamWriter(filename)) {
                int count = 0;
                foreach (var snp in snps) {
                    string rsid = snp.RsId;
                    cancel.ThrowIfCancellationRequested();
                    writer.WriteLine(rsid + "," + snp.AlfredId + "," + Snp.ChromosomeToString(snp.Chromosome)
                        + "," + snp.Position + "," + snp.cM.ToString("0.######") + "," + snp.Alleles.ToAllelesString());
                    count++;
                    if (progress != null) progress.Set(count, snps.Count);
                }
            }
        }

        /// <summary>
        /// Read data into the specified SnpCollection from the Rutgers SNP
        /// map file having the specified filename (which may include a path). 
        /// The specified CancellationToken can be used to abort the read. 
        /// The progress parameter will be updated by this method with a 
        /// value of 0-100 to reflect the percent progress of the read.
        /// </summary>
        /// </summary>
        /// <remarks>See http://compgen.rutgers.edu/RutgersMap/AffymetrixIllumina.aspx </remarks>
        /// <param name="snps">The SnpCollection to receive the read data.</param>
        /// <param name="filename">The path and filename of the Rutgers SNP map file.</param>
        /// <param name="cancel">The CancellationToken that can be used to abort the read.</param>
        /// <param name="progress">The progress parameter that will be updated to reflect 
        /// the percent progress of the read.</param>
        public static void ReadRutgers(SnpCollection snps, string filename, CancellationToken cancel, Progress progress) {
            if (snps == null) throw new ArgumentNullException("The SnpCollection cannot be null.");
            if (filename == null) throw new ArgumentNullException("filename cannot be null.");
            if (String.IsNullOrWhiteSpace(filename)) throw new ArgumentOutOfRangeException("filename cannot be empty.");

            using (StreamReader reader = new StreamReader(filename)) {
                long length = 0;
                if (progress != null) length = reader.BaseStream.Length;
                string[] columns = new string[4];
                string line;
                reader.ReadLine(); // skip header
                while ((line = reader.ReadLine()) != null) {
                    cancel.ThrowIfCancellationRequested();
                    line.FastSplit(',', columns);
                    byte? chr = Snp.ChromosomeToByte(columns[1]);
                    if (chr.HasValue && (chr.Value >= 1) && (chr.Value <= 23)) {
                        float cM;
                        Snp snp;
                        if (float.TryParse(columns[3], out cM)) {
                            snp = new Snp(columns[0], chr.Value, Convert.ToInt32(columns[2]), cM, null, null);
                        } else {
                            snp = new Snp(columns[0], chr.Value, Convert.ToInt32(columns[2]));
                        }
                        snps.Add(snp);
                    }
                    if (progress != null) progress.Set(reader.BaseStream.Position, length);
                }
            }
        }
        #endregion

        public static void ReadMatchWeights(SnpMap<MatchWeight> matchWeights, SnpCollection snps, string filename, 
            CancellationToken cancel, Progress progress) {
            if (matchWeights == null) throw new ArgumentNullException("The MatchWeights cannot be null.");
            if (snps == null) throw new ArgumentNullException("The SnpCollection cannot be null.");
            if (filename == null) throw new ArgumentNullException("filename cannot be null.");
            if (String.IsNullOrWhiteSpace(filename)) throw new ArgumentOutOfRangeException("filename cannot be empty.");

            using (StreamReader reader = new StreamReader(filename)) {
                long length = 0;
                if (progress != null) length = reader.BaseStream.Length;
                string line;
                string[] columns = new string[5];
                while ((line = reader.ReadLine()) != null) {
                    cancel.ThrowIfCancellationRequested();
                    if (line.Length > 0) {
                        line.FastSplit('\t', columns);
                        string rsId = columns[0];
                        string majorAllele = columns[1];
                        double majorWeight, minorWeight;
                        double.TryParse(columns[2], out majorWeight);
                        double.TryParse(columns[4], out minorWeight);
                        if (snps.Contains(rsId)) {
                            Snp snp = snps[rsId];
                            MatchWeight matchWeight = new MatchWeight(Convert.ToInt32(10*majorWeight), Convert.ToInt32(10*minorWeight));
                            matchWeights.Add(snp, matchWeight);
                        }
                    }
                    if (progress != null) progress.Set(reader.BaseStream.Position, length);
                }
            }
        }
    }
}
