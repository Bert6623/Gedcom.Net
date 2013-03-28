using System;
using SoftWx.Dna;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace GenComp {
    // The way the program processes the filenames in the first will
    // be read in groups, and then all matchings between that group
    // and the files in filenames2 will be processed. In general, if
    // processing gets interrupted, you should be able to tell from 
    // the results which group in filenames1 was the last to complete, 
    // so that you can remove them from filenames1 and start the 
    // program up again without having to rematch the whole enchilada.
    // There might be a little interleaving of the last matches of
    // one group and the first matches of the next group because of
    // the indeterminate nature of task scheduling.
    class Program {
        static void Main(string[] args) {
            if (args.Length == 0) {
                Console.WriteLine("No command was given.");
                ShowHelp();
                return;
            }
            var command = args[0].ToLower();
            try {
                if (command == "simplephase") {
                    SimplePhase(args);
                } else if (command == "matchphase") {
                    MatchPhase(args, false);
                } else if (command == "matchphaseboth") {
                    MatchPhase(args, true);
                } else if (command == "match") {
                    Match(args);
                } else if (command == "split") {
                    Split(args);
                } else if (command == "splitclone") {
                    SplitClone(args);
                } else if (command == "merge") {
                    Merge(args);
                } else if (command == "updatecentimorgans") {
                    UpdateCentimorgans(args);
                } else {
                    Console.WriteLine("Command \"" + args[0] + "\" not a recognized command.");
                    ShowHelp();
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
#if DEBUG
            Console.WriteLine("press any key");
            Console.ReadKey();
#endif
        }

        static void SimplePhase(string[] args) {
            if (args.Length <= 1) {
                Console.WriteLine("invalid arguments");
                ShowHelpSimplePhase();
                return;
            }
            SnpCollection refSnps = ReadRefSnps();
            int count = 0;
            for (int i = 1; i < args.Length; i++) {
                IEnumerable<string> filenames;
                filenames = Directory.EnumerateFiles(Configuration.GenomePath, args[i]);
                foreach(var filename in filenames) {
                    GenomeFile gfile = ReadGenome(refSnps, filename);
                    WritePhasedGenome(SimplePhase(filename, gfile, refSnps));
                    count++;
                }
            }
            if (count == 0) Console.WriteLine("no file found");
        }

        static PhasedGenomeFile SimplePhase(string filename, GenomeFile gfile, SnpCollection refSnps) {
            Console.Write("Creating simple phased genome...");
            PhasedGenome phased = new PhasedGenome(gfile.Genome);
            Console.WriteLine("completed");
            PhasedGenomeFile pfile = new PhasedGenomeFile(GetPhasedFilename(filename));
            pfile.SetStandardComments();
            pfile.AddComment("## history");
            pfile.AddComment("## " + DateTime.Now.ToString() + " SimplePhase of " + phased.Count.ToString("#,##0") 
                + " homozygous SNPs (of " + gfile.Genome.Count.ToString("#,##0") + " total).");
            pfile.PhasedGenome = phased;
            return pfile;
        }

        static void MatchPhase(string[] args, bool both) {
            if (args.Length != 3) {
                Console.WriteLine("invalid arguments");
                ShowHelpMatchPhase();
                return;
            }
            SnpCollection refSnps = ReadRefSnps();
            string path = Configuration.GenomePath;
            string file1 = Path.Combine(path, args[1]);
            string file2 = Path.Combine(path, args[2]);
            PhasedGenomeFile phased1, phased2;
            var full1 = ReadGenome(refSnps, file1);
            phased1 = ReadPhasedGenome(refSnps, file1);
            if (phased1 == null) {
                phased1 = SimplePhase(file1, full1, refSnps);
            }
            var full2 = ReadGenome(refSnps, file2);
            phased2 = ReadPhasedGenome(refSnps, file2);
            if (phased2 == null) {
                phased2 = SimplePhase(file2, full2, refSnps);
                if (!both) WritePhasedGenome(phased2);
            }

            Console.WriteLine("match phasing...");
            int oldCount = phased1.PhasedGenome.Count;
            var segs = phased1.PhasedGenome.MatchPhase(phased2.PhasedGenome, full1.Genome, full2.Genome, Configuration.MaxErrorsToStitch, 
                Configuration.PhaseSegmentMinSnpCount, Configuration.SegmentMinCmLength, Configuration.StitchMinPhasedSnpCount, 
                Configuration.StitchMinCmLength, Configuration.PhaseSegmentEdgeWaste, Configuration.FillNoCalls);
            AddHistory(phased1, full2, phased1.PhasedGenome.Count - oldCount, segs);
            Console.WriteLine("completed");
            WritePhasedGenome(phased1);

            if (both) {
                Console.WriteLine("match phasing...");
                oldCount = phased2.PhasedGenome.Count;
                segs = phased2.PhasedGenome.MatchPhase(phased1.PhasedGenome, full2.Genome, full1.Genome, Configuration.MaxErrorsToStitch,
                    Configuration.PhaseSegmentMinSnpCount, Configuration.SegmentMinCmLength, Configuration.StitchMinPhasedSnpCount,
                    Configuration.StitchMinCmLength, Configuration.PhaseSegmentEdgeWaste, Configuration.FillNoCalls);
                AddHistory(phased2, full1, phased2.PhasedGenome.Count - oldCount, segs);
                Console.WriteLine("completed");
                WritePhasedGenome(phased2);
            }
        }

        static void Match(string[] args) {
            if (args.Length < 4) {
                Console.WriteLine("invalid arguments");
                ShowHelpMatch();
                return;
            }
            SnpCollection refSnps = ReadRefSnps();
            string path = Configuration.GenomePath;
            string lastFilename = Path.Combine(path, args[args.Length - 1]);
            var testGenomeFile = new GenomeFile(lastFilename);
            if (File.Exists(testGenomeFile.Filename)) {
                try {
                    testGenomeFile.Read(refSnps, new System.Threading.CancellationToken(), null);
                    if (testGenomeFile.Genome.Count > 0) {
                        int i = 1;
                        while (File.Exists(Path.Combine(path, "MatchResults(" + i + ").csv"))) i++;
                        string filename = Path.Combine(path, "MatchResults(" + i + ").csv");
                        Console.WriteLine("** no filename was specified for the results");
                        Console.WriteLine("** results will be written to " + filename);
                        string[] args2 = new string[args.Length + 1];
                        for (int k = 0; k < args.Length; k++) args2[k] = args[k];
                        args = args2;
                        args[args.Length - 1] = filename;
                    }
                }
                catch { }
            }
            string[] filenames = new string[args.Length - 2];
            for (int i = 0; i < filenames.Length; i++) {
                filenames[i] = args[i + 1];
            }
            List<Tuple<string, string, SegmentMatch>> results = new List<Tuple<string, string, SegmentMatch>>();
            for (int i = 0; i < filenames.Length -1; i++) {
                string file1 = Path.Combine(path, filenames[i]);
                var phased1 = ReadPhasedGenome(refSnps, file1);
                if (phased1 == null) {
                    var full = ReadGenome(refSnps, file1);
                    phased1 = SimplePhase(file1, full, refSnps);
                    WritePhasedGenome(phased1);
                }
                for (int j = i + 1; j < filenames.Length; j++) {
                    string file2 = Path.Combine(path, filenames[j]);
                    var phased2 = ReadPhasedGenome(refSnps, file2);
                    if (phased2 == null) {
                        var full = ReadGenome(refSnps, file2);
                        phased2 = SimplePhase(file2, full, refSnps);
                        WritePhasedGenome(phased2);
                    }

                    Console.Write("Finding matching segments...");
                    var result = phased1.PhasedGenome.MatchingSegments(phased2.PhasedGenome, Configuration.MaxErrorsToStitch, 
                        Configuration.SegmentMinPhasedSnpCount, Configuration.SegmentMinCmLength, 
                        Configuration.StitchMinPhasedSnpCount, Configuration.StitchMinCmLength);
                    if (result.Count > 0) {
                        foreach (var seg in result) results.Add(new Tuple<string, string, SegmentMatch>(phased1.Name, phased2.Name, seg));
                    } else {
                        results.Add(new Tuple<string, string, SegmentMatch>(phased1.Name, phased2.Name, default(SegmentMatch)));
                    }
                    Console.WriteLine("completed");
                }
            }
            Console.Write("Writing match results to " + args[3] + "...");
            WriteSegmentMatches(results, Path.Combine(path, args[args.Length - 1]));
            Console.WriteLine("completed");
        }

        public static void Split(string[] args) {
            if (args.Length != 2) {
                Console.WriteLine("invalid arguments");
                ShowHelpSplit();
                return;
            }
            SnpCollection refSnps = ReadRefSnps();
            string path = Configuration.GenomePath;
            string file1 = Path.Combine(path, args[1]);
            var full = ReadGenome(refSnps, file1);
            var phased1 = ReadPhasedGenome(refSnps, file1);
            if (phased1 == null) {
                phased1 = SimplePhase(file1, full, refSnps);
                WritePhasedGenome(phased1);
            }

            Console.Write("Splitting phased genome...");
            var result = phased1.PhasedGenome.Split(full.Genome, false);
            Console.WriteLine("completed");

            var split = new GenomeFile(phased1.Filename.Replace("phased-", "phasedA-"));
            split.Genome = result.Item1;
            split.AddComment("# this file is the phased A side split from " + phased1.Name);
            WriteGenone(split);
            split = new GenomeFile(phased1.Filename.Replace("phased-", "phasedB-"));
            split.Genome = result.Item2;
            split.AddComment("# this file is the phased B side split from " + phased1.Name);
            WriteGenone(split);            
        }

        public static void SplitClone(string[] args) {
            if (args.Length != 2) {
                Console.WriteLine("invalid arguments");
                ShowHelpSplitClone();
                return;
            }
            SnpCollection refSnps = ReadRefSnps();
            string path = Configuration.GenomePath;
            string file1 = Path.Combine(path, args[1]);
            var full = ReadGenome(refSnps, file1);
            var phased1 = ReadPhasedGenome(refSnps, file1);
            if (phased1 == null) {
                phased1 = SimplePhase(file1, full, refSnps);
                WritePhasedGenome(phased1);
            }

            Console.Write("Splitting and cloning phased genome...");
            var result = phased1.PhasedGenome.Split(true);
            Console.WriteLine("completed");

            var split = new GenomeFile(phased1.Filename.Replace("phased-", "phasedAA-"));
            split.Genome = result.Item1;
            split.AddComment("# this file is the phased A side split and cloned from " + phased1.Name);
            WriteGenone(split);
            split = new GenomeFile(phased1.Filename.Replace("phased-", "phasedBB-"));
            split.Genome = result.Item2;
            split.AddComment("# this file is the phased B side split and cloned from " + phased1.Name);
            WriteGenone(split);            
        }

        public static void Merge(string[] args) {
            Console.WriteLine("not yet implemented");
        }

        public static void UpdateCentimorgans(string[] args) {
            SnpCollection snps = new SnpCollection(1, 23);
            string filename = args[1];
            SnpFile.ReadRutgers(snps, filename, new System.Threading.CancellationToken(), null);
            for (int i = 1; i <= 23; i++) snps.Add(new Snp("fake" + i, (byte) i, 0, 0, null, null));
            SnpCollection refSnps = new SnpCollection(1, 23);
            SnpFile.Read(refSnps, "RefSnps.csv");
            Snp prevSnp = null;
            foreach(Snp snp in refSnps) {
                Snp rutgerSnp = null;
                if (snps.Contains(snp.RsId)) {
                    rutgerSnp = snps[snp.RsId];
                    //if ((rutgerSnp.Chromosome != snp.Chromosome) || (Math.Abs(rutgerSnp.Position-snp.Position) > 10)) {
                    //    Console.WriteLine("mismatched pos for " + snp.RsId + " - " + snp.Chromosome + ":" + snp.Position + " vs. " + rutgerSnp.Chromosome + ":" + rutgerSnp.Position);
                    //    snp.cM = snps.ExtrapolateCentiMorganPosition(snp.Chromosome, snp.Position);
                    //} else {
                    //    snp.cM = snps[snp.RsId].cM;
                    //}
                    snp.Chromosome = rutgerSnp.Chromosome;
                    snp.Position = rutgerSnp.Position;
                } 
                if ((rutgerSnp != null) && (rutgerSnp.cM > 0)) {
                    snp.cM = rutgerSnp.cM;
                } else {
                    snp.cM = snps.ExtrapolateCentiMorganPosition(snp.Chromosome, snp.Position);
                }
                if ((prevSnp != null) && (prevSnp.Chromosome == snp.Chromosome)
                    && (prevSnp.cM > snp.cM)) {
                    Console.WriteLine("cM out of sequence " + prevSnp.RsId + "-" + snp.RsId);
                }
                prevSnp = snp;
            }
            SnpFile.Write(refSnps, "RefSnps2.csv");
        }

        public static string GetPhasedFilename(string genomeFilename) {
            string path = Path.GetDirectoryName(genomeFilename);
            string name = Path.GetFileNameWithoutExtension(genomeFilename);
            if (!name.StartsWith("phased-")) name = "phased-" + name;
            return Path.Combine(path, name + ".txt");
        }

        public static SnpCollection ReadRefSnps() {
            SnpCollection refSnps = new SnpCollection(1, 23);
            Console.Write("Reading reference SNPs...");
            SnpFile.Read(refSnps, "RefSnps.csv");
            Console.WriteLine("completed");
            return refSnps;
        }

        public static GenomeFile ReadGenome(SnpCollection refSnps, string filename) {
            GenomeFile result = new GenomeFile(filename);
            result.Genome = new Genome(1, 23);
            Console.Write("Reading genome " + result.Name + "...");
            result.Read(refSnps, new System.Threading.CancellationToken(), null);
            Console.WriteLine("completed");
            return result;
        }

        public static void WriteGenone(GenomeFile genomeFile) {
            genomeFile.Extension = "txt";
            Console.Write("Writing genome " + genomeFile.Name + "...");
            genomeFile.Write(true, Configuration.UnphasedChar, new System.Threading.CancellationToken(), null);
            Console.WriteLine("completed");
        }

        public static PhasedGenomeFile ReadPhasedGenome(SnpCollection refSnps, string filename) {
            string phasedFilename = GetPhasedFilename(filename);
            if (!File.Exists(phasedFilename)) return null;
            PhasedGenomeFile result = new PhasedGenomeFile(phasedFilename);
            result.PhasedGenome = new PhasedGenome(1, 23);
            Console.Write("Reading phased genome " + result.Name + "...");
            result.Read(refSnps, new System.Threading.CancellationToken(), null);
            Console.WriteLine("completed");
            return result;
        }

        public static void WritePhasedGenome(PhasedGenomeFile genomeFile) {
            genomeFile.Extension = "txt";
            Console.Write("Writing phased genome " + genomeFile.Name + "...");
            genomeFile.Write(true, new System.Threading.CancellationToken(), null);
            Console.WriteLine("completed");
        }

        public static void AddHistory(PhasedGenomeFile pfile, GenomeFile gfile, int count, IList<Tuple<SegmentMatch, SegmentMatch>> segs) {
            DateTime now = DateTime.Now;
            pfile.AddComment("## " + DateTime.Now.ToString() + " MatchPhase added " + count.ToString("#,##0") + " phased heterozygous SNPs using " + gfile.Name + ".");
            if ((segs != null) && (segs.Count > 0)) {
                string filename = Path.Combine(Path.GetDirectoryName(pfile.Filename), pfile.Name + "HISTORY.csv");
                List<string> lines = new List<string>();
                if (!File.Exists(filename)) {
                    lines.Add("date,time,source,chromosome,origStart,origEnd,origLen(cM),sides,usedStart,usedEnd,usedLen(cM),newlyPhasedCount");
                }
                foreach (var tup in segs) {
                    var match = tup.Item1;
                    var phased = tup.Item2;
                    string line = "\"" + now.ToShortDateString() + "\",\"" + now.ToShortTimeString() + "\",\"" + gfile.Name + "\","
                        + Snp.ChromosomeToString(match.StartSnp.Chromosome) + "," + match.StartSnp.Position.ToString() + ","
                        + match.EndSnp.Position.ToString() + "," + match.CmLength.ToString("#0.00") + "," 
                        + match.MatchKind.ToString() + "," + phased.StartSnp.Position.ToString() + "," 
                        + phased.EndSnp.Position.ToString() + "," + phased.CmLength.ToString("#0.00") + "," 
                        + phased.PhasedSnpCount.ToString();
                    lines.Add(line);
                }
                File.AppendAllLines(filename, lines);
            }
        }

        public static void WriteSegmentMatches(IList<Tuple<string, string, SegmentMatch>> segList, string outFile) {
            List<string> lines = new List<string>();
            if (segList.Count > 0) {
                string header = "file1\tfile2\tchromosome\tstart\tend\tphasedCount\tcM\tphaseSide";
                lines.Add(header);
                int si = 1;
                string prevline = null;
                foreach (var tuple in segList.OrderBy(s=>s.Item1).ThenBy(s=>s.Item2).ThenBy(s=>s.Item3.StartSnp).ThenBy(s=>s.Item3.EndSnp).ThenBy(s=>s.Item3.MatchKind)) {
                    var segment = tuple.Item3;
                    int score = (segment.Score + 4) / 10;
                    string str = tuple.Item1 + "\t"
                        + tuple.Item2 + "\t";
                    if (segment.StartSnp == null) {
                        str += "\t\t\t\t\t";
                        prevline = str;
                        lines.Add(str);
                    } else {
                        str += Snp.ChromosomeToString(segment.StartSnp.Chromosome) + "\t"
                            + segment.StartSnp.Position.ToString() + "\t"
                            + segment.EndSnp.Position.ToString() + "\t"
                            + segment.PhasedSnpCount.ToString() + "\t"
                            + (segment.EndSnp.cM - segment.StartSnp.cM).ToString("0.000") + "\t";
                        if (str != prevline) {
                            prevline = str;
                            str += segment.MatchKind.ToString();
                            lines.Add(str);
                            si++;
                        } else {
                            lines[lines.Count - 1] += " " + segment.MatchKind.ToString();
                        }
                    }
                }
            } else {
                // there were no matching segments - write the pairing out to results to show it was processed
                lines.Add("no matching segments");
            }
            // append to results
            File.WriteAllLines(outFile, lines.AsEnumerable());
        }

        static void ShowHelp() {
            Console.WriteLine("usage: GenComp {command} [command dependent parameters]...");
            Console.WriteLine("The following commands (case not important) are supported:");
            Console.WriteLine("  Merge - not implemented yet. Merges two genome files for the same person"
                          + "\n          into a single file. Will merge files from two testing companies,"
                          + "\n          or merge autosomal file with x chromosome file.");
            Console.WriteLine("  SimplePhase - this creates a file containing a simple phasing consisting"
                          + "\n          of just the homozygous alleles.");
            Console.WriteLine("  Match - this performs segment matching of two or more genomes, and outputs"
                          + "\n          the results in a csv file. Will create the simple phased genome"
                          + "\n          files it uses to perform the matching if they do not already exist."
                          + "\n          If more than 2 files are specified, than all possible pairings of"
                          + "\n          two genome files are matched.");
            Console.WriteLine("  MatchPhase - this performs deeper phasing of the first genome using the"
                          + "\n          results of matching against a second genome of a related person.");
            Console.WriteLine("  MatchPhaseBoth - just like MatchPhase, but does deeper phasing of the"
                          + "\n          second genome as well, using matches with the first genome.");
            Console.WriteLine("  Split - Splits a phased genome into two separate genome files that contain"
                          + "\n          the allele contained in their respective sides.");
            Console.WriteLine("  SplitClone - Splits a phased genome into two separate genome files that"
                          + "\n          are homozygous for all alleles the side the split contains.");
            Console.WriteLine("");
            ShowHelpMerge();
            ShowHelpSimplePhase();
            ShowHelpMatch();
            ShowHelpMatchPhase();
            ShowHelpSplit();
            ShowHelpSplitClone();
        }

        static void ShowHelpMerge() { }

        static void ShowHelpSimplePhase() {
            Console.WriteLine(" usage: GenComp SimplePhase filename1 {filename2...}");
            Console.WriteLine(" ex)");
            Console.WriteLine(" GenComp SimplePhase SamJones.csv BonnieSmith.txt");
            Console.WriteLine(" GenComp SimplePhase *.csv");
        }

        static void ShowHelpMatch() {
            Console.WriteLine(" usage: GenComp Match file1 file2 [file3...] outputFile");
            Console.WriteLine(" ex)");
            Console.WriteLine(" GenComp Match SamJones.csv BonnieSmith.txt \"Sam x Bonnie.csv\"");
        }

        static void ShowHelpMatchPhase() {
            Console.WriteLine(" usage: GenComp MatchPhase filename1 filename2");
            Console.WriteLine(" ex)");
            Console.WriteLine(" GenComp MatchPhase SamJones.csv BonnieSmith.txt");
            Console.WriteLine(" GenComp MatchPhaseBoth SamJones.csv BonnieSmith.txt");
        }

        static void ShowHelpSplit() {
            Console.WriteLine(" usage: GenComp Split filename");
            Console.WriteLine(" ex)");
            Console.WriteLine(" GenComp Split SameJones.csv");
        }

        static void ShowHelpSplitClone() {
            Console.WriteLine(" usage: GenComp SplitClone filename");
            Console.WriteLine(" ex)");
            Console.WriteLine(" GenComp SplitClone SameJones.csv");
        }
    }
}
