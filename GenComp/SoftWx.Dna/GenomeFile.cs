using System.Collections.Generic;
using System.Threading;
using System;
using System.IO;

namespace SoftWx.Dna {
    public class GenomeFile {
        private const string header = "# rsid\tchromosome\tposition\tgenotype";
        string filename;
        string name;
        Genome genome;
        List<string> comments = new List<string>();

        private GenomeFile() { }

        public GenomeFile(string filename) {
            this.filename = StripPhasedPrefix(filename);
            name = Path.GetFileNameWithoutExtension(filename);
        }

        public string Filename { get { return this.filename; } }

        public string Extension {
            get { return Path.GetExtension(this.filename); }
            set { this.filename = Path.ChangeExtension(this.filename, value); }
        }

        public string Name { get { return this.name; } }

        public Genome Genome {
            get { return this.genome; }
            set { this.genome = value; }
        }

        public IEnumerable<string> Comments { get { return this.comments; } }

        public void ClearComments() {
            this.comments.Clear();
        }

        public void AddComment(string comment) {
            if (comment == null) comment = "";
            if ((comment.Length == 0) || (comment[0] != '#')) comment = "# " + comment;
            this.comments.Add(comment);
        }

        public void Read(SnpCollection snps, CancellationToken cancel, Progress progress) {
            if (this.filename == null) throw new ArgumentNullException("filename cannot be null.");
            if (String.IsNullOrWhiteSpace(filename)) throw new ArgumentOutOfRangeException("filename cannot be empty.");

            string ext = Path.GetExtension(filename);
            if (ext.EndsWith("csv", StringComparison.InvariantCultureIgnoreCase)) {
                try {
                    Read(Genome.GenomeType.Ftdna, snps, cancel, progress);
                    return;
                }
                catch (Exception) { }
                genome.Clear();
                try {
                    Read(Genome.GenomeType.MeAnd23v2, snps, cancel, progress);
                    return;
                }
                catch (Exception) { }
            } else {
                try {
                    Read(Genome.GenomeType.MeAnd23v2, snps, cancel, progress);
                    return;
                }
                catch (Exception) { }
                genome.Clear();
                try {
                    Read(Genome.GenomeType.Ftdna, snps, cancel, progress);
                    return;
                }
                catch (Exception) { }
            }
            throw new ApplicationException("Could not read as either format.");
        }

        public void Read(Genome.GenomeType genomeType, SnpCollection snps, CancellationToken cancel, Progress progress) {
            if (snps == null) throw new ArgumentNullException("The SnpCollection cannot be null.");
            if (this.filename == null) throw new ArgumentNullException("filename cannot be null.");
            if (String.IsNullOrWhiteSpace(filename)) throw new ArgumentOutOfRangeException("filename cannot be empty.");

            this.comments.Clear();
            if (this.genome == null) {
                this.genome = new Genome(1, 23);
            } else {
                this.genome.Clear();
            }
            if (genomeType == Dna.Genome.GenomeType.Ftdna) {
                ReadFtdnaGenome(snps, cancel, progress);
            } else {
                Read23AndMeGenome(snps, cancel, progress);
            }
            if (String.IsNullOrWhiteSpace(this.genome.Name)) this.genome.Name = Path.GetFileNameWithoutExtension(this.filename);
        }

        public void Write(bool includePositions, char nullChar, CancellationToken cancel, Progress progress) {
            if (this.genome == null) throw new ArgumentNullException("The Genome cannot be null.");
            if (this.filename == null) throw new ArgumentNullException("filename cannot be null.");
            if (String.IsNullOrWhiteSpace(this.filename)) throw new ArgumentOutOfRangeException("filename cannot be empty.");

            using (StreamWriter writer = new StreamWriter(this.filename)) {
                if (this.comments != null) foreach (var str in this.comments) writer.WriteLine(str);
                writer.WriteLine(header);
                int count = 0;
                foreach (var snp in this.genome.Snps) {
                    cancel.ThrowIfCancellationRequested();
                    string line = snp.RsId + "\t";
                    if (includePositions) {
                        line += Snp.ChromosomeToString(snp.Chromosome) + "\t" + snp.Position + "\t";
                    }
                    byte alleles = genome[snp];
                    if (snp.Position == 892967) {
                        int xxx = 1;
                    }
                    if (alleles == Allele.Null) {
                        line += nullChar;
                    } else {
                        line += genome[snp].ToAllelesString();
                    }
                    writer.WriteLine(line);
                    if (progress != null) progress.Set(++count, genome.Count);
                }
            }
        }

        //public static void Write(Genome genome, string filename, IList<string> comments, bool includePositions, char nullChar, CancellationToken cancel, Progress progress) {
        //    Write23AndMeGenome(genome, filename, comments, includePositions, nullChar, cancel, progress);
        //}

        //public void Write(bool includePositions, char nullChar, CancellationToken cancel, Progress progress) {
        //    if (this.genome == null) throw new ApplicationException("The Genome cannot be null.");
        //    if (this.filename == null) throw new ApplicationException("filename cannot be null.");
        //    if (String.IsNullOrWhiteSpace(this.filename)) throw new ApplicationException("filename cannot be empty.");
        //}

        private string StripPhasedPrefix(string filename) {
            if (filename.StartsWith("phased-", StringComparison.InvariantCultureIgnoreCase)) {
                return filename.Substring(7);
            } else {
                return filename;
            }
        }

        #region 23AndMe Files
        // 23AndMe files are are made available by the 23AndMe DNA testing service to
        // their customers.

        /// <summary>
        /// Read data into the specified Genome from an individual's
        /// 23AndMe personal genome file, using the specified filename (which 
        /// may include a path). The specified SnpCollection does not need
        /// to contain all the SNPs that may be present in the genome file.
        /// The SnpCollection is used as the source of physical and cM
        /// positional information. Centimorgan values for SNPs present in
        /// the genome but not in the SnpCollection are extrapolated.
        /// The specified CancellationToken can be used to abort the read. 
        /// The progress parameter will be updated by this method with a 
        /// value of 0-100 to reflect the percent progress of the read.
        /// </summary>
        /// <param name="genome">The Genome to receive the read data.</param>
        /// <param name="snps">The SnpCollection containing reference SNP positional data.</param>
        /// <param name="filename">The path and filename of the 23AndMe genome file.</param>
        /// <param name="cancel">The CancellationToken that can be used to abort the read.</param>
        /// <param name="progress">The progress parameter that will be updated to reflect 
        /// the percent progress of the read.</param>
        private void Read23AndMeGenome(SnpCollection snps, CancellationToken cancel, Progress progress) {
            using (StreamReader reader = new StreamReader(this.filename)) {
                long length = 0;
                if (progress != null) length = reader.BaseStream.Length;
                string line;
                string[] columns = new string[4];
                while ((line = reader.ReadLine()) != null) {
                    cancel.ThrowIfCancellationRequested();
                    if ((line.Length > 0) && (line[0] != '#') && (line[0] != '-')) {
                        int colCount = line.FastSplit('\t', columns);
                        if (colCount <= 1) throw new ApplicationException("Not 23andMe format.");
                        string rsId = columns[0];
                        Snp snp = snps[rsId];
                        if ((snp == null) && (colCount == 4)) {
                            byte chr = Snp.ChromosomeToByte(columns[1]).Value;
                            if (chr <= 23) {
                                int position = Convert.ToInt32(columns[2]);
                                snp = new Snp(rsId, chr, position, snps.ExtrapolateCentiMorganPosition(chr, position), null, "");
                            }
                        }
                        if (snp != null) {
                            var alleles = columns[colCount - 1];
                            if ((alleles != null) && (snp.Chromosome == 23) && (alleles.Length == 1)) alleles += alleles;
                            this.genome.Add(snp, Allele.ToAlleles(alleles));
                        }
                    } else if ((line.Length > 0) && (genome.Count == 0) && !line.StartsWith("# rsid\t")) {
                        this.comments.Add(line);
                    }
                    if (progress != null) progress.Set(reader.BaseStream.Position, length);
                }
            }
            if (this.genome.Count < 700000) {
                this.genome.GenomeTestType = Genome.GenomeType.MeAnd23v2;
            } else {
                this.genome.GenomeTestType = Genome.GenomeType.MeAnd23v3;
            }
        }

        private void Write23AndMe(bool includePositions, char nullChar, CancellationToken cancel, Progress progress) {
            using (StreamWriter writer = new StreamWriter(this.filename)) {
                int count = 0;
                if (includePositions) {
                    writer.WriteLine("# rsid\tchromosome\tposition\tgenotype");
                } else {
                    writer.WriteLine("# rsid\tgenotype");
                }
                foreach (var snp in genome.Snps) {
                    cancel.ThrowIfCancellationRequested();
                    string line = snp.RsId + "\t";
                    if (includePositions) {
                        line += Snp.ChromosomeToString(snp.Chromosome) + "\t" + snp.Position + "\t";
                    }
                    byte alleles = genome[snp];
                    if (alleles == Allele.Null) {
                        line += nullChar;
                    } else {
                        line += genome[snp].ToAllelesString();
                    }
                    writer.WriteLine(line);
                    if (progress != null) progress.Set(++count, genome.Count);
                }
            }
        }
        #endregion
        #region Family Tree DNA (FTDNA) Files
        // FTDNA files are are made available by the Family Tree DNA testing 
        // service to their customers.

        /// <summary>
        /// Read data into the specified Genome from an individual's
        /// FTDNA personal genome file, using the specified filename (which 
        /// may include a path). The specified SnpCollection does not need
        /// to contain all the SNPs that may be present in the genome file.
        /// The SnpCollection is used as the source of physical and cM
        /// positional information. Centimorgan values for SNPs present in
        /// the genome but not in the SnpCollection are extrapolated.
        /// The specified CancellationToken can be used to abort the read. 
        /// The progress parameter will be updated by this method with a 
        /// value of 0-100 to reflect the percent progress of the read.
        /// </summary>
        /// <param name="genome">The Genome to receive the read data.</param>
        /// <param name="snps">The SnpCollection containing reference SNP positional data.</param>
        /// <param name="filename">The path and filename of the FTDNA genome file.</param>
        /// <param name="cancel">The CancellationToken that can be used to abort the read.</param>
        /// <param name="progress">The progress parameter that will be updated to reflect 
        /// the percent progress of the read.</param>
        private void ReadFtdnaGenome(SnpCollection snps, CancellationToken cancel, Progress progress) {
            using (StreamReader reader = new StreamReader(this.filename)) {
                long length = 0;
                if (progress != null) length = reader.BaseStream.Length;
                string[] columns = new string[4];
                string line;
                bool started = false;
                while ((line = reader.ReadLine()) != null) {
                    cancel.ThrowIfCancellationRequested();
                    if ((line.Length > 0) && (line[0] != '#') && (line[0] != '-')
                        && (started || !line.StartsWith("RSID,", StringComparison.Ordinal))) {
                        started = true;
                        line.Replace("\"", "").FastSplit(',', columns);
                        string rsId = columns[0];
                        Snp snp = snps[rsId];
                        if (snp == null) {
                            byte chr = Snp.ChromosomeToByte(columns[1]).Value;
                            if (chr > 23) continue;
                            int position = Convert.ToInt32(columns[2]);
                            snp = new Snp(rsId, chr, position, snps.ExtrapolateCentiMorganPosition(chr, position), null, "");
                        }
                        var alleles = columns[3];
                        if ((snp.Chromosome == 23) && (alleles.Length == 1)) alleles += alleles;
                        this.genome.Add(snp, Allele.ToAlleles(alleles));
                    } else if ((line.Length > 0) && (this.genome.Count == 0) && !line.StartsWith("# rsid\t")) {
                        this.comments.Add(line);
                    }
                    if (progress != null) progress.Set(reader.BaseStream.Position, length);
                }
            }
        }
        #endregion
    }
}
