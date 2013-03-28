using System.Collections.Generic;
using System.Threading;
using System;
using System.IO;

namespace SoftWx.Dna {
    public class PhasedGenomeFile {
        private const string header = "# rsid\tchromosome\tposition\tgenotype";
        string filename;
        string name;
        PhasedGenome genome;
        List<string> comments = new List<string>();

        private PhasedGenomeFile() { }

        public PhasedGenomeFile(string filename) {
            this.filename = filename;
            this.name = Path.GetFileNameWithoutExtension(filename);
        }

        public string Filename { get { return this.filename; } }

        public string Extension {
            get { return Path.GetExtension(this.filename); }
            set { this.filename = Path.ChangeExtension(this.filename, value); }
        }

        public string Name { get { return this.name; } }

        public PhasedGenome PhasedGenome {
            get { return this.genome; }
            set { this.genome = value; }
        }

        public IEnumerable<string> Comments { get { return this.comments; } }

        public void ClearComments() {
            this.comments.Clear();
        }

        public void SetStandardComments() {
            ClearComments();
            AddComment("# " + this.name);
            AddComment("# allele values are phased - the first allele character of each SNP go");
            AddComment("# together, and the second allele character of each SNp go together.");
        }

        public void AddComment(string comment) {
            if (comment == null) comment = "";
            if ((comment.Length == 0) || (comment[0] != '#')) comment = "# " + comment;
            this.comments.Add(comment);
        }

        public void Read(PhasedGenome genome, SnpCollection snps, CancellationToken cancel, Progress progress) {
            this.PhasedGenome = genome;
            Read(snps, cancel, progress);
        }

        public void Read(SnpCollection snps, CancellationToken cancel, Progress progress) {
            if (snps == null) throw new ArgumentNullException("The SnpCollection cannot be null.");
            if (this.filename == null) throw new ArgumentNullException("filename cannot be null.");
            if (String.IsNullOrWhiteSpace(filename)) throw new ArgumentOutOfRangeException("filename cannot be empty.");

            if (this.genome == null) {
                this.genome = new PhasedGenome(1, 23);
            } else {
                this.genome.Clear();
            }
            this.comments.Clear();
            using (StreamReader reader = new StreamReader(this.filename)) {
                long length = 0;
                if (progress != null) length = reader.BaseStream.Length;
                string line;
                string[] columns = new string[4];
                while ((line = reader.ReadLine()) != null) {
                    cancel.ThrowIfCancellationRequested();
                    if ((line.Length > 0) && (line[0] != '#')) {
                        int colCount = line.FastSplit('\t', columns);
                        if ((colCount != 2) && (colCount != 4)) throw new ApplicationException("Not phased genome format.");
                        string rsId = columns[0];
                        Snp snp = snps[rsId];
                        if (snp != null) {
                            var alleles = columns[colCount - 1];
                            var phased = new PhasedGenome.Phasing(alleles[0].ToAllele(), alleles[1].ToAllele());
                            this.genome.Add(snp, phased);
                        }
                    } else if ((line.Length > 0) && (genome.Count == 0) && (line != header)) {
                        this.comments.Add(line);
                    }
                    if (progress != null) progress.Set(reader.BaseStream.Position, length);
                }
            }
            if (String.IsNullOrWhiteSpace(genome.Name)) this.genome.Name = Path.GetFileNameWithoutExtension(filename);
        }

        public void Write(bool includePositions, CancellationToken cancel, Progress progress) {
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
                    var phased = genome[snp];
                    line += phased.A.ToAlleleString() + phased.B.ToAlleleString();
                    writer.WriteLine(line);
                    if (progress != null) progress.Set(++count, genome.Count);
                }
            }
        }
    }
}
