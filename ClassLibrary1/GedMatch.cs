using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using HtmlAgilityPack;
using Gedcom;
using ThreadFactory;
//using Gedcom.Reports;

namespace GedMatch
{
    public class GedMatch
    {
        private static string htmlfile = String.Empty;
        private static string gedfile = String.Empty;
        public string WorkDir { get; set; }
        public string WorkIndiDir { get; set; }
        private GedcomDatabase _gd1;
        System.Collections.Hashtable htIndi;
        TaskFactory _factory;
        TaskFactory _gedfactory;
        public bool DoMatch { get; set; }
        public bool DoGed { get; set; }

        public GedMatch()
        {
            DoGed = false;
            DoMatch = false;
        }

        public void loadGedMatch( string kitNbr, Boolean recurse )
        {
            int retry = 0;
            if (recurse)
            {
                LimitedConcurrencyLevelTaskScheduler lcts = new LimitedConcurrencyLevelTaskScheduler(3);
                _factory = new TaskFactory(lcts);
                LimitedConcurrencyLevelTaskScheduler gedlcts = new LimitedConcurrencyLevelTaskScheduler(1);
                _gedfactory = new TaskFactory(gedlcts);
            }
            StreamWriter writer;

            if (WorkIndiDir == null || WorkIndiDir.Equals(string.Empty))
                WorkIndiDir = "C:\\Temp\\INDI\\";

            if (!Directory.Exists( WorkIndiDir ))
                Directory.CreateDirectory(WorkIndiDir);
            string pre = string.Empty;
            if (kitNbr.Trim().Length == 0)
                return;
            
            pre = "http://ww2.gedmatch.com:8007/autosomal/r-list2.php?kit_num=" + kitNbr + "&cm_limit=7&x_cm_limit=3&xsubmit=Display+Results";

            Recreate:

            if (!File.Exists(WorkDir + "\\GEDMATCH" + kitNbr + ".html"))
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(pre);

                request.ContentType = "application/x-www-form-urlencoded";
                request.Method = "Get";
                request.Timeout = 600000;
                request.Referer = "http://ww2.gedmatch.com:8007/autosomal/r_list1.php";
                request.Pipelined = false;
                request.KeepAlive = true;
                request.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.Default);
                request.UserAgent = "Mozilla/4.0 (Compatible; Windows NT 5.1; MSIE 6.0)" + " (compatible; MSIE 6.0; Windows NT 5.1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                //string Parameters = "kit_num=F169551&cm_limit=7&x_cm_limit=3&xsubmit=Display+Results";
                //request.ContentLength = Parameters.Length;

                //// We write the parameters into the request
                //StreamWriter sw = new StreamWriter(request.GetRequestStream());
                //sw.Write(Parameters);
                //sw.Close();
                StreamReader reader;
                lock (htmlfile)
                {
                    htmlfile = kitNbr;
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    reader = new StreamReader(response.GetResponseStream());
                }
                writer = new StreamWriter(WorkDir + "\\GEDMATCH" + kitNbr + ".html");
                string line = reader.ReadLine();
                while (line != null)
                {
                    line = reader.ReadLine();
                    writer.WriteLine(line);
                }
                reader.Close();
                writer.Close();
            }

            if (!File.Exists(WorkDir + "\\GEDMATCH" + kitNbr + ".csv") || recurse)
            {
                System.Threading.Tasks.Task[] tasks = new System.Threading.Tasks.Task[9999];
                int taskid = 0;
                Console.WriteLine("Loading " + kitNbr);
                writer = new StreamWriter(WorkDir + "\\GEDMATCH" + kitNbr + ".csv");
                HtmlDocument doc = new HtmlDocument();
                try
                {
                    doc.Load(WorkDir + "\\GEDMATCH" + kitNbr + ".html");
                }
                catch (Exception ex)
                {
                    doc = null;
                    File.Delete(WorkDir + "\\GEDMATCH" + kitNbr + ".html");
                    if (retry++ < 2)
                        goto Recreate;
                }
                HtmlNode hdn = doc.DocumentNode;
                IEnumerable<HtmlNode> nodes = System.Linq.Enumerable.Empty<HtmlNode>();
                nodes = hdn.SelectNodes("/body[1]/font[1]/table[1]");
                int i = 0;
                foreach (var node in nodes)
                {
                    int row = 0;
                    foreach (var rnode in node.ChildNodes)
                    {
                        string sex = "U";
                        int pos = 0;
                        string nKitNbr = string.Empty;
                        if (row++ < 3 || rnode.ChildNodes.Count == 0)
                            continue;
                        foreach (var tnode in rnode.ChildNodes)
                        {
                            pos++;
                            if (pos == 2)
                            {
                                nKitNbr = tnode.InnerText.Trim();
                                Console.WriteLine("Gathering kit number " + nKitNbr);
                                if (nKitNbr.Length == 0)
                                    continue;
                                writer.Write(kitNbr + "," + nKitNbr + ",");
                            }
                            //Type
                            if (pos == 4)
                            {
                                writer.Write(tnode.InnerText.Trim() + ",");
                            }
                            //6 Triangulate
                            //7 GedCom
                            if (pos == 7 && nKitNbr.Length > 0)
                            {
                                if (DoGed)
                                {
                                    try
                                    {
                                        string html = tnode.InnerHtml.Trim();
                                        if (!html.Equals("&nbsp;") && !html.Equals(String.Empty) && (html.IndexOf("a href") > 0))
                                        {
                                            var parent = _gedfactory.StartNew(() =>
                                                       {
                                                           GetGedMatch(nKitNbr, html, pre, sex);
                                                       });
                                            tasks[taskid++] = parent;
                                        }
                                    }
                                    catch { }
                                }
                            }
                            //8 List
                            if (pos == 8)
                            {
                            }
                            //Sex
                            if (pos == 12)
                            {
                                sex = tnode.InnerText.Trim();
                                writer.Write(tnode.InnerText.Trim() + ",");
                            }
                            //Details
                            if (pos == 17)
                            {
                                writer.Write(tnode.InnerText.Trim() + ",");
                            }
                            //Total cM
                            if (pos == 19)
                            {
                                writer.Write(tnode.InnerText.Trim() + ",");
                            }
                            //Largest cM
                            if (pos == 21)
                            {
                                writer.Write(tnode.InnerText.Trim() + ",");
                            }
                            //Generation
                            if (pos == 23)
                            {
                                writer.Write(tnode.InnerText.Trim().Replace("&nbsp;", "") + ",");
                            }
                            //Has X
                            if (pos == 25)
                            {
                                writer.Write(tnode.InnerText.Trim().Replace("&nbsp;", "") + ",");
                            }
                            //X Adjusted cM
                            if (pos == 26)
                            {
                                writer.Write(tnode.InnerText.Trim().Replace("&nbsp;", "") + ",");
                            }
                            //X Total cM
                            if (pos == 28)
                            {
                                writer.Write(tnode.InnerText.Trim() + ",");
                            }
                            //X Largest cM
                            if (pos == 30)
                            {
                                writer.Write(tnode.InnerText.Trim() + ",");
                            }
                            //e-mail address
                            if (pos == 33)
                            {
                                string email = tnode.InnerHtml;
                                email = email.Replace("<font face=\"courier\" size=\"+1\"><b>", "");
                                email = email.Replace("</b></font><img src='./gifs/Eaddr_", "");
                                email = email.Replace(".gif'>", "");
                                writer.Write(email);
                            }
                        }
                        if (nKitNbr.Length > 0)
                        {
                            writer.WriteLine();
                            if (DoMatch)
                            {
                                try
                                {
                                    var parent = _factory.StartNew(() =>
                                               { loadGedMatch(nKitNbr, false); });
                                    tasks[taskid++] = parent;
                                }
                                catch { }
                            }
                        }
                    }
                }
                writer.Close();
                tasks = (Task[])ResizeArray(tasks, taskid);
                Task.WaitAll(tasks);
            }
        }
        
        private static System.Array ResizeArray(System.Array oldArray, int newSize)
        {
            int oldSize = oldArray.Length;
            System.Type elementType = oldArray.GetType().GetElementType();
            System.Array newArray = System.Array.CreateInstance(elementType, newSize);
            int preserveLength = System.Math.Min(oldSize, newSize);
            if (preserveLength > 0)
                System.Array.Copy(oldArray, newArray, preserveLength);
            return newArray;
        }

        private void GetGedMatch(string nKitNbr, string html, string pre, string sex, int attempt = 0)
        {
            if (!html.Equals("&nbsp;") && !html.Equals(String.Empty) && (html.IndexOf("<a href") >= 0))
            {
                Console.WriteLine("Loading GED Kit number " + nKitNbr);
                _gd1 = new GedcomDatabase();
                GedcomHeader gh = new GedcomHeader();
                string GedName = String.Empty;
                gh.Filename = "GM_" + nKitNbr + ".ged";
                _gd1.Header = gh;

                htIndi = new System.Collections.Hashtable(50);

                html = html.Replace("&nbsp;", " ");
                html = html.Replace("<a href='", "");
                string geds = String.Empty;
                try { geds = html.Trim().Substring(0, html.IndexOf(" title") - 2); }
                catch { }

                int ifam = html.IndexOf("id_family=");
                int ifame = html.IndexOf("' title");
                string nfamily = html.Substring(ifam + 10,ifame-ifam-10);
                GetHTML(geds, WorkDir + "\\GM_SURNAME" + nKitNbr + ".html", pre);

                StreamReader reader = new StreamReader(WorkDir + "\\GM_SURNAME" + nKitNbr + ".html");
                string line = reader.ReadLine();
                bool found = false;
                while (line != null)
                {
                    if (line.StartsWith("ANCESTORS OF: "))
                    {
                        int ig = line.IndexOf("&id_ged");
                        int ige = line.IndexOf("'>");
                        int ine = line.IndexOf("</a>");
                        try
                        {
                            GedName = line.Substring(ige + 2, ine - ige - 2).Replace("&nbsp;", " ").Replace("<b>", "").Replace("</b>", "");
                        }
                        catch { }
                        try
                        {
                            string nindi = line.Substring(ig + 8, ige - ig - 8);

                            if (!Directory.Exists(WorkIndiDir + nfamily))
                                Directory.CreateDirectory(WorkIndiDir + nfamily);

                            GedcomIndividualRecord gir = GetGedIndi(nindi, nfamily, nKitNbr, geds, sex);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message + " - " + ex.StackTrace);
                        }
                        found = true;
                        break;
                    }
                    line = reader.ReadLine();
                }
                reader.Close();

                if (!found && attempt++ < 2)
                {
                    GetHTML("http://www.gedmatch.com/surname.php?id_family=" + nfamily, WorkDir + "\\GM_SURNAME_LIST_" + nKitNbr + ".html", geds);
                    StreamReader freader = new StreamReader(WorkDir + "\\GM_SURNAME_LIST_" + nKitNbr + ".html");
                    string fline = freader.ReadLine();
                    {
                        //Try http://www.gedmatch.com/surname_detail.php?id_familyname=243182&id_family=5884506
                        try
                        {
                            GetHTML("http://www.gedmatch.com/surname.php?initial=A&id_family=" + nfamily, WorkDir + "\\GM_SURNAME_LIST_A_" + nKitNbr + ".html", geds);

                            StreamReader f1reader = new StreamReader(WorkDir + "\\GM_SURNAME_LIST_A_" + nKitNbr + ".html");
                            string f1line = f1reader.ReadLine();
                            while (f1line != null)
                            {
                                if (f1line.IndexOf("id_familyname=") > 0)
                                {
                                    int ifamname = f1line.IndexOf("id_familyname=");
                                    int ige = f1line.IndexOf("&");
                                    string nfamilyname = f1line.Substring(ifamname + 14, ige - ifamname - 14);
                                    //Get http://www.gedmatch.com/surname_detail.php?id_familyname=243182&id_family=5884506
                                    GetHTML("http://www.gedmatch.com/surname_detail.php?id_familyname=" + nfamilyname + "&id_family=" + nfamily, WorkDir + "\\GM_SURNAME_DETAIL_" + nKitNbr + ".html", geds);

                                    StreamReader dreader = new StreamReader(WorkDir + "\\GM_SURNAME_DETAIL_" + nKitNbr + ".html");
                                    string dline = dreader.ReadLine();
                                    while (dline != null)
                                    {
                                        if (dline.IndexOf("id_ged=") > 0)
                                        {
                                            int iged = dline.IndexOf("id_ged=");
                                            int ie = dline.IndexOf("'>");
                                            string nindi = dline.Substring(iged + 7,ie-7-iged);
                                            html = dline.Substring(iged, ie + 4 - iged);
                                            //Call 
                                            if (!Directory.Exists(WorkIndiDir + nfamily))
                                                Directory.CreateDirectory(WorkIndiDir + nfamily);
                                            GetGedIndi(nindi, nfamily, nKitNbr, geds, sex);
                                            found = true;
                                        }
                                        dline = dreader.ReadLine();
                                    }
                                }
                                f1line = f1reader.ReadLine();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message + " - " + ex.StackTrace);
                        }

                    }
                    reader.Close();

                }

                if (found)
                lock (gedfile)
                {
                    gedfile = WorkDir + "\\GM_" + GedName + nKitNbr + ".ged";
                    Console.WriteLine("Writting " + gedfile);
                    GedcomParser.GedcomRecordWriter grw = new GedcomParser.GedcomRecordWriter();
                    try
                    {
                        grw.WriteGedcom(_gd1, gedfile);
                    }
                    catch
                    {   
                        try
                        {
                            gedfile = WorkDir + "\\GM_" + nKitNbr + ".ged";
                            grw.WriteGedcom(_gd1, gedfile);
                        }
                        catch (Exception ex) { Console.WriteLine("Couldn't write out " + gedfile + ": " + ex.Message + " - " + ex.StackTrace); }
                    }
                    gedfile = string.Empty;
                }

            }
        }

        private GedcomIndividualRecord GetGedIndi(string indi, string family, string nKitNbr, string pre, string sex, GedcomIndividualRecord sindi=null,GedcomIndividualRecord schild=null)
        {
            if (htIndi.Contains(indi))
            {
                foreach (GedcomIndividualRecord ggir in _gd1.Individuals)
                {
                    try
                    {
                        if (ggir.XRefID.Equals(indi))
                            return ggir;
                    }
                    catch { }
                }
                return null;
            }
            else
                htIndi.Add(indi, indi);

            string fatherid = String.Empty;
            string motherid = String.Empty;
            string npre = "http://www.gedmatch.com/individual_detail.php?id_family=" + family + "&id_ged=" + indi;
            foreach (char ichar in Path.GetInvalidFileNameChars())
                indi.Replace(ichar, '_');
            GetHTML(npre, WorkIndiDir + family + "\\GM_INDI_" + nKitNbr + "_" + indi + ".html", pre);
            StreamReader reader = new StreamReader(WorkIndiDir + family + "\\GM_INDI_" + nKitNbr + "_" + indi + ".html");
            string line = reader.ReadLine();
            GedcomIndividualRecord record=null;
            GedcomFamilyRecord ngfr = null;

            bool union = false;
            while (line != null)
            {

                //Check for Name
                if (line.StartsWith("<br><font size=+3><b>"))
                {
                    int ine = line.IndexOf("</b>");
                    int ige = line.IndexOf("'>");
                    int iborn = line.IndexOf("Born:");
                    int ib = line.IndexOf("</b>");
                    int ideath = line.IndexOf("Died:");
                    string name = line.Substring(21, ine - 21).Replace("&nbsp;", " ").Trim();

                    record = new GedcomIndividualRecord(_gd1);
                    GedcomName gn = new GedcomName();
                    gn.Database = _gd1;
                    gn.Split(name);
                    gn.Level = 1;
                    gn.PreferedName = true; 
                    record.Names.Add(gn);
                    record.XRefID = indi;
                    record.Sex = GedcomSex.Undetermined;
                    if (sex.Equals("M"))
                    {
                        record.Sex = GedcomSex.Male;
                    }
                    if (sex.Equals("F"))
                    {
                        record.Sex = GedcomSex.Female;
                    }
                    try
                    {
                        string born = line.Substring(iborn + 11).Replace("<br>", "").Replace("&nbsp;", " ").Trim();
                        string death = string.Empty;
                        if (ideath > 0)
                        {
                            born = line.Substring(iborn + 11, ideath - iborn - 11).Replace("<br>", "").Replace("&nbsp;", " ").Trim();
                            death = line.Substring(ideath);
                        }

                        try
                        {
                            GedcomDate bd = new GedcomDate(born);
                            bd.Database = _gd1;
                            bd.Date1 = born.Substring(0, born.IndexOf(","));
                            bd.Level = record.Level + 2;
                            GedcomIndividualEvent gieb = new GedcomIndividualEvent();
                            gieb.Database = _gd1;
                            gieb.Date = bd;
                            //record.Birth.Date = bd;
                            GedcomAddress gab = new GedcomAddress();
                            gab.Database = _gd1;
                            gab.AddressLine = born.Substring(born.IndexOf(",") + 1);
                            gieb.Address = gab;
                            //record.Birth.Address = gab;
                            gieb.EventType = GedcomEvent.GedcomEventType.BIRT;
                            gieb.IndiRecord = record;
                            gieb.Level = record.Level + 1;
                            record.Events.Add(gieb);
                        }
                        catch
                        {
                        }

                        if (death.Equals(string.Empty))
                        {
                            GedcomDate dd = new GedcomDate(death);
                            dd.Database = _gd1;
                            dd.Date1 = death.Substring(0, death.IndexOf(","));
                            dd.Level = record.Level + 2;
                            GedcomIndividualEvent gieb = new GedcomIndividualEvent();
                            gieb.Database = _gd1;
                            gieb.Date = dd;
                            //record.Birth.Date = bd;
                            GedcomAddress gab = new GedcomAddress();
                            gab.Database = _gd1;
                            gab.AddressLine = born.Substring(death.IndexOf(",") + 1);
                            gieb.Address = gab;
                            //record.Birth.Address = gab;
                            gieb.EventType = GedcomEvent.GedcomEventType.DEAT;
                            gieb.IndiRecord = record;
                            gieb.Level = record.Level+1;
                            record.Events.Add(gieb);
                        }
                    }
                    catch {}
                    //GedcomFamilyRecord ngfr=null;
                    //if (sindi != null)
                    //{
                    //    ngfr = new GedcomFamilyRecord(_gd1, record, sindi);
                    //    //sindi.SpouseIn.Add(
                    //}
                    //if (schild != null)
                    //{
                    //    //GedcomFamilyLink gfl = new GedcomFamilyLink();
                    //    //gfl.Database = _gd1;
                    //    if (ngfr != null)
                    //    {
                    //        //gfl.XRefID = ngfr.XRefID;
                    //        ngfr.AddChild(schild);
                    //    }
                    //    else
                    //    {
                    //        ngfr = new GedcomFamilyRecord(_gd1, record, null);
                    //        ngfr.AddChild(schild);
                    //    }
                    //}
                }

                //Check for
                if (line.StartsWith("<br>Father: "))
                {
                    try
                    {
                        int ifam = line.IndexOf("id_family=");
                        int ig = line.IndexOf("&id_ged");
                        int ige = line.IndexOf("'>");
                        if (ifam > 0 && ig > 0)
                        {
                            string nfamily = line.Substring(ifam + 10, ig - ifam - 10);
                            string nindi = line.Substring(ig + 8, ige - ig - 8);
                            GetGedIndi(nindi, nfamily, nKitNbr, npre, "M", null, record);
                        }
                    }
                    catch { }
                }
                if (line.StartsWith("<br>Mother: "))
                {
                    try
                    {
                        int ifam = line.IndexOf("id_family=");
                        int ig = line.IndexOf("&id_ged");
                        int ige = line.IndexOf("'>");
                        if (ifam > 0 && ig > 0)
                        {
                            string nfamily = line.Substring(ifam + 10, ig - ifam - 10);
                            string nindi = line.Substring(ig + 8, ige - ig - 8);
                            GetGedIndi(nindi, nfamily, nKitNbr, npre, "F", null, record);
                        }
                        int iss = line.IndexOf("<br>Union with:");
                        if (iss > 0)
                            line = line.Substring(iss);
                    }
                    catch { }
                }

                //Check for Spouse
                if (line.StartsWith("<br>Union with:") || line.StartsWith("<br><br>Union with: ") || union)
                {
                    int ifam = line.IndexOf("id_family=");
                    int ig = line.IndexOf("&id_ged");
                    int ige = line.IndexOf("'>");
                    if (ifam > 0 && ig > 0)
                    {
                        string nfamily = line.Substring(ifam + 10, ig - ifam - 10);
                        string nindi = line.Substring(ig + 8, ige - ig - 8);
                        string nsex = "U";
                        if (sex.Equals("M"))
                        {
                            nsex = "F";
                        }
                        if (sex.Equals("F"))
                        {
                            nsex = "M";
                        }
                        GedcomIndividualRecord spouse = GetGedIndi(nindi, nfamily, nKitNbr, npre, nsex);
                        //ToDo Figure out if we already created the family with OTHER spouse
                        ngfr = new GedcomFamilyRecord(_gd1, record, spouse);
                        union = false;
                    }
                    else
                        union = true;
                }

                bool gotchild = false;
                while (line.StartsWith("<br>Children: ") || (gotchild && (line.StartsWith("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href='./individual")||line.StartsWith("&nbsp;&nbsp;&nbsp;&nbsp;+<a href='./individual"))))
                {
                    union = false;
                    int ifam = line.IndexOf("id_family=");
                    int ig = line.IndexOf("&id_ged");
                    int ige = line.IndexOf("'>");
                    if (ifam > 0 && ig > 0)
                    {
                        string nfamily = line.Substring(ifam + 10, ig - ifam - 10);
                        string nindi = line.Substring(ig + 8, ige - ig - 8);
                        string nsex = "U";
                        GedcomIndividualRecord child = GetGedIndi(nindi, nfamily, nKitNbr, npre, nsex);
                        if (ngfr == null)
                        {
                            //Add child to Group
                        }
                        else
                        {
                            ngfr.AddChild(child);
                        }
                        line = reader.ReadLine();
                    }
                    gotchild = true;
                }
                //Add Children
                line = reader.ReadLine();
            }
            reader.Close();

            return record;
        }

        private void GetHTML(string html, string fn, string pre)
        {
            if (!File.Exists(fn))
            {
                StreamWriter writer;

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(html);
                request.ContentType = "application/x-www-form-urlencoded";
                request.Method = "Get";
                request.Timeout = 600000;
                request.Referer = pre;
                request.Pipelined = false;
                request.KeepAlive = true;
                request.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
                request.UserAgent = "Mozilla/4.0 (Compatible; Windows NT 5.1; MSIE 6.0)" + " (compatible; MSIE 6.0; Windows NT 5.1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                //request.ContentLength = Parameters.Length;

                StreamReader reader;
                //lock (htmlfile)
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    reader = new StreamReader(response.GetResponseStream());
                }
                writer = new StreamWriter(fn);
                string line = reader.ReadLine();
                while (line != null)
                {
                    line = reader.ReadLine();
                    writer.WriteLine(line);
                }
                reader.Close();
                writer.Close();
            }
        }


    }
}
