﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Gedcom;
using Gedcom.Reports;
using System.Net;
using System.Text.RegularExpressions;
using GedMatch;
//using System.Web.Script.Serialization;


namespace GedComCommand
{
    class Program
    {

        TextWriter sw;
        String swFile1;
        String swFile2;

        Program()
        {
        }

        public void controller(string[] args)
        {
            if (args.Length >= 2 && args[0].Equals("-FTDNA"))
            {
                FamilyTreeDNA.FamilyTreeDNA ftd = new FamilyTreeDNA.FamilyTreeDNA();
                ftd.Login("169551", "D3742");
            }
            if (args.Length >= 2 && args[0].Equals("-compare"))
            {
                GedcomDatabase gd1 = new GedcomDatabase();
                GedcomIndividualRecord record = new GedcomIndividualRecord(gd1, "Robert Warthen");
                record.Sex = GedcomSex.Male;
                //record.Birth = new GedcomIndividualEvent().Date.;

                GedcomParser.GedcomRecordWriter grw = new GedcomParser.GedcomRecordWriter();
                grw.WriteGedcom(gd1, "C:\\Temp\\Rob.ged");

            }
            if (args.Length >= 2 && args[0].Equals("-compdir"))
            {
                sw = new StreamWriter(args[1] + "\\match.csv");
                sw.WriteLine("File1,File2,Exact Match,Given1,Surname1,Birth Date1,Birth Place1,Death Date1,Death Place1,Given2,Surname2,Birth Date2,Birth Place2,Death Date2,Death Place2");
                string[] files = Directory.GetFiles(args[1], "*.*", SearchOption.TopDirectoryOnly);
                string[] files2 = Directory.GetFiles(args[1], "*.*", SearchOption.TopDirectoryOnly);

                if (args.Length >= 3 && args[2].Equals("-file"))
                {
                    files = new string[1];
                    files[0] = args[3];
                }

                if (args.Length >= 3 && args[2].Equals("-dir"))
                {
                    files2 = Directory.GetFiles(args[3], "*.*", SearchOption.TopDirectoryOnly);
                }

                foreach (string gdf1 in files)
                {
                    string gedcomFile1 = gdf1.ToUpper();
                    if (!gedcomFile1.EndsWith(".GED"))
                        continue;

                    swFile1 = Path.GetFileName(gedcomFile1);
                    //Console.WriteLine("Loading " + );
                    GedcomParser.GedcomRecordReader grr1 = new GedcomParser.GedcomRecordReader();
                    grr1.GedcomFile = gedcomFile1;
                    try { grr1.ReadGedcom(); }
                    catch { continue; }
                    GedcomDatabase gd1 = grr1.Database;
                    foreach (string gdf2 in files2)
                    {
                        string gedcomFile2 = gdf2.ToUpper();
                        if (!gedcomFile2.EndsWith(".GED"))
                            continue;
                        if (gedcomFile2.Equals(gedcomFile1))
                            continue;
                        swFile2 = Path.GetFileName(gedcomFile2);
                        //Console.WriteLine("Comparing " + gedcomFile1 + " and " + gedcomFile2);
                        GedcomParser.GedcomRecordReader grr2 = new GedcomParser.GedcomRecordReader();
                        grr2.GedcomFile = gedcomFile2;
                        try { grr2.ReadGedcom(); }
                        catch { continue; }
                        GedcomDatabase gd2 = grr2.Database;

                        GedcomDuplicate.DuplicateFoundFunc found = new GedcomDuplicate.DuplicateFoundFunc(FoundDuplicate);
                        float matchThreshold = 95;

                        GedcomDuplicate.FindDuplicates(gd1, gd2, matchThreshold, found);
                    }
                    sw.Flush();
                }
                sw.Close();
            }
            if (args.Length >= 2 && args[0].Equals("-surname"))
            {
                string[] files = Directory.GetFiles(args[1], "*.*", SearchOption.AllDirectories);
                string surname = args[2].ToUpper();

                foreach (string gedcomFile1 in files)
                {
                    if (!gedcomFile1.ToUpper().EndsWith(".GED"))
                        continue;

                    //Console.WriteLine("Comparing " + gedcomFile1 + " for surname " + surname);
                    GedcomParser.GedcomRecordReader grr1 = new GedcomParser.GedcomRecordReader();
                    grr1.GedcomFile = gedcomFile1;
                    grr1.ReadGedcom();
                    GedcomDatabase gd1 = grr1.Database;
                    bool foundfile = false;

                    foreach (GedcomIndividualRecord rec in gd1.Individuals)
                    {
                        try
                        {
                            if (rec.Names.Count > 0 &&  rec.Names[0].Surname.ToUpper().Equals(surname))
                            {
                                if (!foundfile)
                                {
                                    Console.WriteLine("File " + gedcomFile1);
                                    foundfile = true;
                                }
                                Console.Write("  Found " + rec.Names[0].Given + " " + rec.Names[0].Surname);
                                try { Console.Write(" Born: " + rec.Birth.Date.DateString); }
                                catch { }
                                try { Console.Write(" in " + rec.Birth.Place.Name); }
                                catch { }
                                try { Console.Write(" Died: " + rec.Death.Date.DateString); }
                                catch { }
                                try { Console.Write(" at " + rec.Death.Place.Name); }
                                catch { }
                                Console.WriteLine();
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Can't read");
                        }
                    }

                }
            }

            if (args.Length >= 2 && args[0].Equals("-soundex"))
            {
                string[] files = Directory.GetFiles(args[1], "*.*", SearchOption.AllDirectories);
                string surname = args[2].ToUpper();
                string sunameSoundex = Util.GenerateSoundex(surname);

                foreach (string gedcomFile1 in files)
                {
                    if (!gedcomFile1.ToUpper().EndsWith(".GED"))
                        continue;

                    //Console.WriteLine("Comparing " + gedcomFile1 + " for surname " + surname);
                    GedcomParser.GedcomRecordReader grr1 = new GedcomParser.GedcomRecordReader();
                    grr1.GedcomFile = gedcomFile1;
                    grr1.ReadGedcom();
                    GedcomDatabase gd1 = grr1.Database;
                    bool foundfile = false;

                    foreach (GedcomIndividualRecord rec in gd1.Individuals)
                    {
                        if (rec.Names[0].SurnameSoundex.Equals(sunameSoundex))
                        {
                            if (!foundfile)
                            {
                                Console.WriteLine("File " + gedcomFile1);
                                foundfile = true;
                            }
                            Console.Write("  Found " + rec.Names[0].Given + " " + rec.Names[0].Surname);
                            try { Console.Write(" Born: " + rec.Birth.Date.DateString); }
                            catch { }
                            try { Console.Write(" in " + rec.Birth.Place.Name); }
                            catch { }
                            try { Console.Write(" Died: " + rec.Death.Date.DateString); }
                            catch { }
                            try { Console.Write(" at " + rec.Death.Place.Name); }
                            catch { }
                            Console.WriteLine();
                        }
                    }

                }
            }

            if (args.Length >= 2 && args[0].Equals("-loaddir"))
            {
                string[] files = Directory.GetFiles(args[1], "*.*", SearchOption.AllDirectories);

                foreach (string gedcomFile1 in files)
                {
                    Console.WriteLine("Loading " + gedcomFile1);
                    GedcomParser.GedcomRecordReader grr1 = new GedcomParser.GedcomRecordReader();
                    grr1.GedcomFile = gedcomFile1;
                    grr1.ReadGedcom();
                    GedcomDatabase gd1 = grr1.Database;

                    foreach (GedcomIndividualRecord matchIndi in gd1.Individuals)
                    {
                        SaveDatabase(matchIndi);
                    }
                }
            }

            if (args.Length >= 2 && args[0].ToUpper().Equals("-GEDMATCH"))
            {
                GedMatch.GedMatch gm = new GedMatch.GedMatch();
                gm.WorkDir = args[1];
                gm.DoMatch = true;
                gm.DoGed = true;

                gm.loadGedMatch(args[2],true);
            }
            
            if (args.Length >= 2 && args[0].ToUpper().Equals("-GETGEDMATCH"))
            {
                GedMatch.GedMatch gm = new GedMatch.GedMatch();
                gm.WorkDir = args[1];
                gm.DoGed = true;

                gm.loadGedMatch(args[2], true);
            }
        }

        private void client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
        }
        private void SaveDatabase(GedcomIndividualRecord matchIndi)
        {
        }

        private void FoundDuplicate(GedcomIndividualRecord indi, List<GedcomIndividualRecord> matches)
        {
            sw.Write(swFile1 + "," + swFile2 + ",");
            if (indi.Names[0].Surname.Equals(matches[0].Names[0].Surname))
            {
                try
                {
                    if (indi.Birth.Date.DateString.Equals(matches[0].Birth.Date.DateString))
                    {
                        Console.WriteLine("Exact Match");
                        sw.Write("Yes");
                    }
                }
                catch { }
            }
            sw.Write(",");
            Console.Write("  Found " + indi.Names[0].Given + " " + indi.Names[0].Surname);
            sw.Write(indi.Names[0].Given + "," + indi.Names[0].Surname+",");
            try { Console.Write(" Born: " + indi.Birth.Date.DateString); sw.Write(indi.Birth.Date.DateString); }
            catch { }
            sw.Write(",");
            try { Console.Write(" in " + indi.Birth.Place.Name); sw.Write(indi.Birth.Place.Name); }
            catch { }
            sw.Write(",");
            try { Console.Write(" Died: " + indi.Death.Date.DateString); sw.Write(indi.Death.Date.DateString); }
            catch { }
            sw.Write(",");
            try { Console.Write(" at " + indi.Death.Place.Name); sw.Write(indi.Death.Place.Name);}
            catch { }
            sw.Write(",");
            Console.WriteLine();

            Console.Write("  Found " + matches[0].Names[0].Given + " " + matches[0].Names[0].Surname);
            sw.Write(matches[0].Names[0].Given + "," + matches[0].Names[0].Surname + ",");
            try { Console.Write(" Born: " + matches[0].Birth.Date.DateString); sw.Write(matches[0].Birth.Date.DateString); }
            catch { }
            sw.Write(",");
            try { Console.Write(" in " + matches[0].Birth.Place.Name); sw.Write(matches[0].Birth.Place.Name); }
            catch { }
            sw.Write(",");
            try { Console.Write(" Died: " + matches[0].Death.Date.DateString); sw.Write(matches[0].Death.Date.DateString); }
            catch { }
            sw.Write(",");
            try { Console.Write(" at " + matches[0].Death.Place.Name); sw.Write(matches[0].Death.Place.Name); }
            catch { }
            Console.WriteLine();
            sw.WriteLine();
        }
        
        static void Main(string[] args)
        {
            Program p = new Program();
            p.controller(args);
        }
    }
}
