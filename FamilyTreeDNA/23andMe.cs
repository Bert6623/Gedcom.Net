using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using Newtonsoft.Json;
using System.Xml.Serialization;
using HtmlAgilityPack;
//using Gedcom;


namespace MeAnd23
{
    public class MeAnd23
    {
        private static readonly object lckMe = new object();
        public string WorkDir { get; set; }
        public string WorkIndiDir { get; set; }
        public string WebProfile { get; set; }
        public bool runFIA { get; set; }
        public bool runAF { get; set; }
        public bool runRF { get; set; }
        public bool runAC { get; set; }
        CookieContainer s_cc;

        List<string> lstProfile = new List<string>();
        string profileName = "Unknown";

        List<string> lstMatch = new List<string>();
        Hashtable htMatch = new Hashtable();

        public MeAnd23()
        {
            s_cc = new CookieContainer();
            WebProfile = "";
            runFIA = true;
            runAF = true;
            runRF = true;
            runAC = true;
        }

        public void Login(string userName, string password)
        {
            StreamWriter swlog = new StreamWriter(WorkDir + "../" + userName.Substring(0, userName.IndexOf('@')) + ".log",true);
            swlog.AutoFlush = true;
            WebProfile = WebProfile.ToLower();
            StreamWriter swulog = new StreamWriter(WorkDir + userName.Substring(0, userName.IndexOf('@')) + ".inprogress", true);
            swulog.Close();
            try
            {
                HttpWebRequest request = CreateRequest("https://www.23andme.com/");
                HttpWebResponse resp = null;
                string respHtml = "";
                lock (lckMe)
                {

                    resp = (HttpWebResponse)request.GetResponse();
                    ProcessCookies(resp);
                    respHtml = new StreamReader(resp.GetResponseStream()).ReadToEnd();
                }

                //Cookie c = new Cookie("NSC_xxx-wjq", "445226223660", "//", ".23andMe.com");
                //request.CookieContainer.Add(c);
                //c = new Cookie("optimizelyEndUserId", "oeu1345470154549r0.9720360543578863", "//", ".23andMe.com");
                //request.CookieContainer.Add(c);
                //c = new Cookie("ki_u", "ef627737-bd22-cafa-b9a6-fcfa4b45ea64", "//", ".23andMe.com");
                //request.CookieContainer.Add(c);
                //c = new Cookie("ki_t", "1345470155073%3B1345470155073%3B1345470163268%3B1%3B2", "//", ".23andMe.com");
                //request.CookieContainer.Add(c);
                //c = new Cookie("optimizelyBuckets", "%7B%2250099471%22%3A%2250120626%22%2C%2284955555%22%3A%2284897752%22%2C%2284976393%22%3A%2284996094%22%2C%2289363959%22%3A%2289400562%22%7D", "//", ".23andMe.com");
                //request.CookieContainer.Add(c);
                //c = new Cookie("optimizelyPendingLogEvents", "%5B%5D", "//", ".23andMe.com");
                //request.CookieContainer.Add(c);
                //c = new Cookie("WRUID", "0", "//", ".23andMe.com");
                //request.CookieContainer.Add(c);

                lock (lckMe)
                {
                    request = CreateRequest("https://www.23andme.com/user/signin/");
                    resp = (HttpWebResponse)request.GetResponse();
                    respHtml = new StreamReader(resp.GetResponseStream()).ReadToEnd();
                }

                request = CreateRequest("https://www.23andme.com/user/signin/");
                request.Method = "POST";
                request.Headers.Add("X-MicrosoftAjax", "Delta=true");
                request.ContentType = "application/x-www-form-urlencoded";
                using (StreamWriter w = new StreamWriter(request.GetRequestStream()))
                {
                    string payload = "username=" + userName + "&" + "password=" + password.Replace("&", "%26") + "&redirect=&source_flow=&__source_node__=start&__context__=QO6yqOAVbaD-OSN1P9PcroaOymz8yokN_By_fI9tYFVXNfblzmBx0jFosOXw9puKpeYvkl64JbYL1mdlcakgjRxiuvmfrYUCCrnqJXES3efUucDsoIQ1TGCXfSVHidvuSGR8X8AKaD0M2z8y_c3Pug%3D%3D&__form__=login&redirect=&button=Log+In";
                    w.Write(payload);
                    w.Flush();
                }
                lock (lckMe)
                {
                    resp = (HttpWebResponse)request.GetResponse();
                    respHtml = new StreamReader(resp.GetResponseStream()).ReadToEnd();
                }
                try { swlog.WriteLine("Login " + userName + " / " + password + " - " + WebProfile + " " + runFIA.ToString() + "/" + runAF.ToString()); }
                catch { }
                //Go in first with default.
                request = CreateRequest("https://www.23andme.com/you/");
                request.Referer = "https://www.23andme.com/user/signin/";
                lstProfile = new List<string>();
                profileName = "unknown";
                lock (lckMe)
                {
                    resp = (HttpWebResponse)request.GetResponse();
                    //List has     <div class="top_menu_item profile_names first_item"> and <div id="profile_dropdown" class="dropdown">
                    //No LIst has  <div class="top_menu_item profile_names first_item"> and immediatly <a href
                    //Read list <div id="profile_dropdown" class="dropdown">
                    //      <ul>    
                    //          <li><a id="profile_option_ea893fefef0d4a8b" class="profile_option" href="#">Brian Ware</a></li>
                    //Use /user/profile/stick/global with profile_id=3597e8b1fd7b2eb0&redirect=https%3A%2F%2Fwww.23andme.com%2Fyou%2F
                    //Add ability to free up memory more quickly.
                    {
                        HtmlAgilityPack.HtmlDocument htmlDoc2 = new HtmlAgilityPack.HtmlDocument();
                        htmlDoc2.OptionFixNestedTags = true;
                        htmlDoc2.Load(resp.GetResponseStream());
                        // <li id="nav-profile-select" class="has-flyout">
                        //<i class="ttam-icon-multi-user nav-icon"></i></a>
                        //<div class="flyout-content multi-user-flyout">
                        //    <ul>
                        //        <li class="current-profile"><a href="javascript:void(0)">Janis Moore</a></li>
                        //        <li class="divider"></li>
                        //    </ul>
                        //    <ul class="profile-list">
                        //        <li>
                        try
                        {
                            profileName = htmlDoc2.DocumentNode.SelectSingleNode("//li[@class='current-profile']").InnerText;
                            foreach (HtmlNode link in htmlDoc2.DocumentNode.SelectNodes("//ul[@class='profile-list']/li/a"))
                            {
                                if (WebProfile.Length == 0 && link.InnerText.ToLower().Contains(WebProfile))
                                    lstProfile.Add(link.Attributes["id"].Value.Substring(15));
                            }
                        }
                        catch
                        {
                            try
                            {
                                profileName = htmlDoc2.DocumentNode.SelectSingleNode("//a[@id='toggle_profile_dropdown']").InnerText;
                                foreach (HtmlNode link in htmlDoc2.DocumentNode.SelectNodes("//div[@id='profile_dropdown']/ul/li/a"))
                                {
                                    if (WebProfile.Length == 0 || link.InnerText.ToLower().Contains(WebProfile))
                                        lstProfile.Add(link.Attributes["id"].Value.Substring(15));
                                }
                            }
                            catch
                            {
                                //Old Format
                                try
                                {
                                    profileName = htmlDoc2.DocumentNode.SelectSingleNode("//div[@id='topright_nav']//a[@href='/you/']").InnerText;
                                }
                                catch
                                {
                                    profileName = "Unknown";
                                }
                            }
                        }
                        htmlDoc2 = null;
                        GC.Collect();

                    }
                }

                bool repeat = true;

                while ((WebProfile.Length > 0 && !profileName.ToLower().Contains(WebProfile) && repeat))
                {
                    repeat = getNextProfile();
                }

                do
                {
                    htMatch = new Hashtable();
                    try { swlog.WriteLine("Profile  " + profileName); }
                    catch { }

                    request = CreateRequest("https://www.23andme.com/you/relfinder/");
                    request.Referer = "https://www.23andme.com/you/";
                    request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                    lock (lckMe)
                    {

                        resp = (HttpWebResponse)request.GetResponse();
                        respHtml = new StreamReader(resp.GetResponseStream()).ReadToEnd();
                    }

                    //Make sure we have data.
                    if (!respHtml.Contains("data is not yet available") && !respHtml.Contains("Sample not received yet"))
                    {
                        //request = CreateRequest("https://cdn.optimizely.com/js/27075387.js");
                        //request.Referer = "https://www.23andme.com/you/relfinder/";
                        //resp = (HttpWebResponse)request.GetResponse();
                        //respHtml = new StreamReader(resp.GetResponseStream()).ReadToEnd();

                        //request = CreateRequest("https://log3.optimizely.com/event?a=27075387&d=27075387&y=false&x50099471=50093564&x81281830=81347246&x84955555=84897752&x84976393=84996094&x89363959=89400562&f=27031044,28251938,50099471,65179583,84955555,84976393,89363959,93736311&n=https%3A%2F%2Fwww.23andme.com%2Fyou%2Frelfinder%2F&u=oeu1339759658119r0.9431997842621058&wxhr=true&t=1345407177537");
                        //request.Referer = "https://www.23andme.com/you/relfinder/";
                        //resp = (HttpWebResponse)request.GetResponse();
                        //respHtml = new StreamReader(resp.GetResponseStream()).ReadToEnd();

                        request = CreateRequest("https://www.23andme.com/you/relfinder/fetch/");
                        request.Referer = "https://www.23andme.com/you/relfinder/";
                        request.Accept = "*/*";
                        //request.SendChunked = true;
                        //request.TransferEncoding = "gzip,deflate,sdch";
                        request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                        //request.Accept
                        lock (lckMe)
                        {

                            resp = (HttpWebResponse)request.GetResponse();
                            respHtml = new StreamReader(resp.GetResponseStream()).ReadToEnd();
                        }
                        //List<relfinder> rfl = (List<relfinder>)JsonConvert.DeserializeObject(respHtml, typeof(List<relfinder>));
                        relfinder rf = (relfinder)JsonConvert.DeserializeObject(respHtml, typeof(relfinder));

                        StreamWriter sw = new StreamWriter(WorkDir + userName.Substring(0, userName.IndexOf('@')) + "_" + profileName + "_23andMe_RF.csv");
                        sw.Write("Name,Sex,Birth Year,Relationship,Predicted Relationship,Predicted Range,DNA Shared,Segments Shared,Maternal Haplogroup,Paternal Haplogroup,Birthplace,Residence,Ancestry,Family Surnames,Family Locations,Notes,Sharing Status,Introduction Status");
                        sw.Write(",can_resend");
                        sw.Write(",resend_date");
                        sw.Write(",added");
                        sw.Write(",patsid");
                        sw.Write(",matside");
                        sw.Write(",rel_user");
                        sw.Write(",rel_alg");
                        sw.Write(",img");
                        sw.Write(",eiid");
                        sw.Write(",ehid");
                        sw.Write(",eid");
                        sw.Write(",first");
                        sw.Write(",last");
                        sw.Write(",visible");
                        sw.Write(",url");
                        sw.Write(",disc");
                        sw.WriteLine();
                        foreach (relfinderm rfm in rf.matches)
                        {
                            sw.Write(rfm.full); //Name
                            sw.Write("," + rfm.sex); //Sex
                            sw.Write("," + rfm.year); // Birth Year
                            sw.Write("," + rfm.rel_label); //Relationship
                            sw.Write("," + rfm.rel_alg_label); //Predicted Relationship
                            sw.Write("," + rfm.rel_range); //Predicted Range
                            sw.Write("," + rfm.pct); //DNA Shared
                            sw.Write("," + rfm.segs); //Segments Shared
                            sw.Write("," + rfm.mat);
                            sw.Write("," + rfm.pat);
                            sw.Write("," + rfm.birth); //Birthplace
                            sw.Write("," + rfm.res); //Residence
                            sw.Write(",\"" + rfm.anc + "\""); //Ancestry
                            sw.Write(",\"" + rfm.surs + "\""); //Family Surnames
                            sw.Write(",\"" + rfm.locs + "\"");//Family Locations
                            sw.Write(",\"" + rfm.desc + "\""); //Notes
                            sw.Write("," + rfm.share_status); //Sharing Status
                            sw.Write("," + rfm.intro_status); //Introduction Status
                            sw.Write("," + rfm.can_resend);
                            sw.Write("," + rfm.resend_date);
                            sw.Write("," + rfm.added);
                            sw.Write("," + rfm.patsid);
                            sw.Write("," + rfm.matside);
                            sw.Write("," + rfm.rel_user);
                            sw.Write("," + rfm.rel_alg);
                            sw.Write("," + rfm.img);
                            sw.Write("," + rfm.eiid);
                            sw.Write("," + rfm.ehid);
                            sw.Write("," + rfm.eid);
                            sw.Write("," + rfm.first);
                            sw.Write("," + rfm.last);
                            sw.Write("," + rfm.visible);
                            sw.Write("," + rfm.url);
                            sw.Write("," + rfm.disc);

                            if (rfm.ehid != null && rfm.ehid.Length > 0)
                            {
                                if (!lstMatch.Contains(rfm.ehid))
                                    lstMatch.Add(rfm.ehid);
                                htMatch.Add(rfm.ehid, rfm.full);
                            }

                            sw.WriteLine();
                        }
                        sw.Close();

                        try { swlog.WriteLine("FIA"); }
                        catch { }

                        // /you/labs/multi_ibd/results
                        request = CreateRequest("https://www.23andme.com/you/labs/multi_ibd/");
                        request.Referer = "https://www.23andme.com/you/labs/ancestry/";
                        request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                        List<string> lstFIA = new List<string>();
                        string first = string.Empty;
                        string last = "https://www.23andme.com/you/labs/multi_ibd/";
                        lock (lckMe)
                        {
                            resp = (HttpWebResponse)request.GetResponse();
                            //respHtml = new StreamReader(resp.GetResponseStream()).ReadToEnd();

                            //Add ability to free up memory more quickly.
                            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                            htmlDoc.OptionFixNestedTags = true;
                            htmlDoc.Load(resp.GetResponseStream());

                            foreach (HtmlNode link in htmlDoc.DocumentNode.SelectNodes("//li[@data-first]//span"))
                            {
                                if (link.Attributes["class"].Value.Equals("ps_id"))
                                {
                                    if (first.Length == 0) first = link.InnerText;
                                    if (!lstFIA.Contains(link.InnerText))
                                        lstFIA.Add(link.InnerText);
                                }
                            }
                            htmlDoc = null;
                            GC.Collect();

                        }

                        if (runAC)
                        {
                            string next = "https://www.23andme.com/you/ancestry/composition/fetch/?profile_id=" + first + "&threshold=0.51";
                            request = CreateRequest(next);
                            request.Referer = last;
                            request.Accept = "*/*";
                            lock (lckMe)
                            {
                                resp = (HttpWebResponse)request.GetResponse();
                                respHtml = new StreamReader(resp.GetResponseStream()).ReadToEnd();
                            }
                            sw = new StreamWriter(WorkDir + userName.Substring(0, userName.IndexOf('@')) + "_" + profileName + "_23andMe_AC.csv");
                            HeaderOutputAC(sw, respHtml);
                            sw.Close();
                        }

                        if (runFIA)
                        {
                            sw = new StreamWriter(WorkDir + userName.Substring(0, userName.IndexOf('@')) + "_" + profileName + "_23andMe_FIA.csv");
                            sw.Write("Comparison,Chromosome,Start point,End point,Genetic Distance,# SNPs");
                            sw.WriteLine();
                            // /you/labs/ancestry_finder/lookup/?profile_id_encrypted=
                            Random rm = new Random();
                            string next = "https://www.23andme.com/you/labs/multi_ibd/download/?default_id_1=" + first + "&default_id_2=";
                            int cnt = 1;
                            int totcnt = 0;
                            swlog.WriteLine("FIA - " + lstFIA.Count.ToString());
                            foreach (string ehid in lstFIA)
                            {
                                if (ehid.Length <= 0 || ehid.Equals(first))
                                    continue;
                                totcnt++;
                                if (totcnt % 25 == 0)
                                    swlog.WriteLine("FIA Count " + totcnt.ToString() + " out of " + lstFIA.Count.ToString());
                                cnt++;
                                if (cnt == 2)
                                {
                                    next = "https://www.23andme.com/you/labs/multi_ibd/results/?default_id_1=" + first + "&default_id_2=" + ehid;
                                    continue;
                                }
                                else if (cnt == 3)
                                {
                                    next = next + "&default_id_3=" + ehid;
                                    continue;
                                }
                                else if (cnt == 4)
                                    next = next + "&default_id_4=" + ehid;

                                //System.Threading.Thread.Sleep(rm.Next(000, 12000));

                                //request = CreateRequest(next);
                                //request.Referer = last;
                                //request.Accept = "*/*";
                                //request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                                //resp = (HttpWebResponse)request.GetResponse();
                                //respHtml = new StreamReader(resp.GetResponseStream()).ReadToEnd();

                                next = next.Replace("/results/?", "/download/?");
                                request = CreateRequest(next);
                                request.Referer = last;
                                request.Accept = "*/*";
                                request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                                try
                                {
                                    lock (lckMe)
                                    {
                                        resp = (HttpWebResponse)request.GetResponse();
                                        respHtml = new StreamReader(resp.GetResponseStream()).ReadToEnd();
                                    }
                                    if (!respHtml.StartsWith("Comparison"))
                                    {
                                        request = CreateRequest("https://www.23andme.com/you/labs/multi_ibd/");
                                        request.Referer = "https://www.23andme.com/you/labs/ancestry/";
                                        request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                                        lock (lckMe)
                                        {
                                            resp = (HttpWebResponse)request.GetResponse();
                                            respHtml = new StreamReader(resp.GetResponseStream()).ReadToEnd();
                                        }
                                        last = "https://www.23andme.com/you/labs/multi_ibd/";
                                        continue;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("FIA Error:" + ex.Message + " - " + ex.StackTrace);
                                    try { swlog.WriteLine("FIA Error:" + ex.Message + " - " + ex.StackTrace); }
                                    catch { }

                                    break;
                                    //continue;
                                }
                                HeaderOutputFIA(sw, respHtml);
                                last = next;
                                if (cnt == 4) { cnt = 1; }
                            }
                            if (cnt > 1)
                            {
                                if (cnt == 2)
                                    next = next + "&default_id_3=&default_id_4=";
                                if (cnt == 3)
                                    next = next + "&default_id_4=";
                                next = next.Replace("/results/?", "/download/?");
                                request = (HttpWebRequest)HttpWebRequest.Create(next);
                                request.CookieContainer = s_cc; // reuse cookie contianer across requests
                                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.1 (KHTML, like Gecko) Chrome/21.0.1180.79 Safari/537.1";
                                request.Referer = last;
                                request.Accept = "*/*";
                                request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                                try
                                {
                                    lock (lckMe)
                                    {
                                        resp = (HttpWebResponse)request.GetResponse();
                                        respHtml = new StreamReader(resp.GetResponseStream()).ReadToEnd();
                                    }
                                    if (!respHtml.StartsWith("Comparison"))
                                    {
                                        request = CreateRequest("https://www.23andme.com/you/labs/multi_ibd/");
                                        request.Referer = "https://www.23andme.com/you/labs/ancestry/";
                                        request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                                        lock (lckMe)
                                        {

                                            resp = (HttpWebResponse)request.GetResponse();
                                            respHtml = new StreamReader(resp.GetResponseStream()).ReadToEnd();
                                        }
                                        last = "https://www.23andme.com/you/labs/multi_ibd/";
                                        HeaderOutputFIA(sw, respHtml);
                                    }
                                    request = null;
                                    GC.Collect();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("FIA Error:" + ex.Message + " - " + ex.StackTrace);
                                    try { swlog.WriteLine("FIA Error:" + ex.Message + " - " + ex.StackTrace); }
                                    catch { }
                                }
                            }
                            sw.Close();
                            swlog.WriteLine("Completed FIA");
                        }
                    }
                    repeat = getNextProfile();
                } while (repeat);

                if (runAF)
                {
                    try { swlog.WriteLine("RunAF"); }
                    catch { }
                    //---------------------------------
                    // /you/labs/ancestry_finder/
                    request = CreateRequest("https://www.23andme.com/you/labs/ancestry_finder/");
                    request.Referer = "https://www.23andme.com/you/relfinder/";
                    request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                    List<string> lstAF = new List<string>();
                    lock (lckMe)
                    {
                        resp = (HttpWebResponse)request.GetResponse();
                        //respHtml = new StreamReader(resp.GetResponseStream()).ReadToEnd();

                        {
                            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                            htmlDoc.OptionFixNestedTags = true;
                            htmlDoc.Load(resp.GetResponseStream());


                            foreach (HtmlNode link in htmlDoc.DocumentNode.SelectNodes("//select[@id='id_profile_id_select']/option"))
                            {
                                string vl = link.GetAttributeValue("value", "");
                                if (vl.Length > 0 && !lstAF.Contains(vl))
                                    lstAF.Add(vl);
                                if (!htMatch.Contains(vl))
                                {
                                    if (link.NextSibling.InnerText.Length > 0)
                                        htMatch.Add(vl, link.NextSibling.InnerText);
                                    else
                                        htMatch.Add(vl, link.InnerText);
                                }
                            }
                            request = null;
                            GC.Collect();
                        }
                    }
                    StreamWriter sw = new StreamWriter(WorkDir + userName.Substring(0, userName.IndexOf('@')) + "_23andMe_AF.csv");
                    sw.Write("Name,MatchName,MaternalGrandmotherBirthCountry,MaternalGrandfatherBirthCountry,PaternalGrandmotherBirthCountry,PaternalGrandfatherBirthCountry,MaternalGrandmotherDeclaredAshkenazi,MaternalGrandfatherDeclaredAshkenazi,PaternalGrandmotherDeclaredAshkenazi,PaternalGrandfatherDeclaredAshkenazi,Chromosome,SegmentStartInMegaBasePairs,SegmentEndInMegaBasePairs,SegmentLengthInMegaBasePairs,SegmentLengthInCentiMorgans");
                    sw.WriteLine();
                    string last = "https://www.23andme.com/labs/ancestry_finder/";
                    // /you/labs/ancestry_finder/lookup/?profile_id_encrypted=
                    Random rm = new Random();
                    foreach (string ehid in lstAF)
                    {
                        if (ehid.Length <= 0)
                            continue;

                        //System.Threading.Thread.Sleep(rm.Next(1000, 3000));
                        string next = "https://www.23andme.com/you/labs/ancestry_finder/export/?profile_id_encrypted=" + ehid + "&download_submit=Download+Ancestry+Finder+matches+%28CSV%29";
                        request = (HttpWebRequest)HttpWebRequest.Create(next);
                        request.CookieContainer = s_cc; // reuse cookie contianer across requests
                        request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.1 (KHTML, like Gecko) Chrome/21.0.1180.79 Safari/537.1";
                        request.Referer = last;
                        request.Accept = "*/*";
                        //request.SendChunked = true;
                        //request.TransferEncoding = "gzip,deflate,sdch";
                        request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                        //request.Accept
                        try
                        {
                            lock (lckMe)
                            {

                                resp = (HttpWebResponse)request.GetResponse();
                                respHtml = new StreamReader(resp.GetResponseStream()).ReadToEnd();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("AF Error:" + ex.Message + " - " + ex.StackTrace);
                            try { swlog.WriteLine("AF Error:" + ex.Message + " - " + ex.StackTrace); }
                            catch { }
                            continue;
                        }
                        string dnamatch = htMatch[ehid].ToString();
                        HeaderOutputAF(sw, respHtml, dnamatch);
                        request = null;
                        GC.Collect();
                    }
                    sw.Close();
                }
                System.IO.File.Delete(WorkDir + userName.Substring(0, userName.IndexOf('@')) + ".inprogress");
            }
            catch (Exception ex)
            {
                try { swlog.WriteLine("Overall Error:" + ex.Message + " - " + ex.StackTrace); }
                catch { }
            }
            finally { swlog.WriteLine("Finished");  swlog.Close(); }

        }

        private bool getNextProfile()
        {
            bool repeat = false;
            if (lstProfile.Count > 0)
            {
                //Set next id
                string profileId;
                do
                {
                    profileId = lstProfile[0];
                    lstProfile.Remove(profileId);
                }
                while (profileId.Length == 0);

                if (profileId.Length > 0)
                {
                    // /user/profile/stick/global with profile_id=3597e8b1fd7b2eb0&redirect=https%3A%2F%2Fwww.23andme.com%2Fyou%2F
                    //Get data
                    HttpWebRequest request = CreateRequest("https://www.23andme.com/user/profile/stick/global/");
                    request.Referer = "https://www.23andme.com/you";
                    request.Method = "POST";
                    request.Headers.Add("X-MicrosoftAjax", "Delta=true");
                    request.ContentType = "application/x-www-form-urlencoded";
                    using (StreamWriter w = new StreamWriter(request.GetRequestStream()))
                    {
                        string payload = "profile_id=" + profileId + "&redirect=https%3A%2F%2Fwww.23andme.com%2Fyou%2F";
                        w.Write(payload);
                        w.Flush();
                    }
                    lock (lckMe)
                    {
                        HttpWebResponse resp = (HttpWebResponse)request.GetResponse();
                        {
                            HtmlAgilityPack.HtmlDocument htmlDoc2 = new HtmlAgilityPack.HtmlDocument();
                            htmlDoc2.OptionFixNestedTags = true;
                            htmlDoc2.Load(resp.GetResponseStream());
                            try
                            {
                                profileName = htmlDoc2.DocumentNode.SelectSingleNode("//li[@class='current-profile']").InnerText;
                            }
                            catch
                            {
                                try { profileName = htmlDoc2.DocumentNode.SelectSingleNode("//a[@id='toggle_profile_dropdown']").InnerText; }
                                catch { repeat = false; htmlDoc2 = null; GC.Collect();  return repeat; }
                            }
                            htmlDoc2 = null;

                            GC.Collect();
                        }
                    }
                    repeat = true;
                }
                else
                    repeat = false;
            }
            else
                repeat = false;
            return repeat;
        }

        private void HeaderOutputFIA(StreamWriter sw, string respHtml)
        {
            //sw.Write(respHtml.Replace("Comparison,Chromosome,Start point,End point,Genetic Distance,# SNPs\r\n", ""));
            string[] lines = respHtml.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            int i = 0;
            foreach (string line in lines)
            {
                if (i != 0 && line.Trim().Length > 0) //Skip first line with the header info
                {
                    //Want to split out the VS part
                    sw.WriteLine(line);
                }
                i++;
            }
        }

        private void HeaderOutputAF(StreamWriter sw, string respHtml, string match)
        {
            string[] lines = respHtml.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            int i = 0;
            foreach (string line in lines)
            {
                if (i != 0 && line.Trim().Length > 0)
                {
                    sw.WriteLine("\"" + match + "\"," + line);
                }
                i++;
            }
        }

        private void HeaderOutputAC(StreamWriter sw, string respHtml)
        {
            try
            {
                System.Xml.XmlDocument doc = (System.Xml.XmlDocument)JsonConvert.DeserializeXmlNode(respHtml,"root");
                XmlNode rootNode = doc.FirstChild;
                XmlNode segNode = doc.FirstChild;
                foreach (XmlNode tstNode in rootNode.ChildNodes)
                {
                    if (tstNode.Name.Equals("segments"))
                        segNode = tstNode;
                }
                if (segNode == rootNode)
                    throw new Exception();

                foreach (XmlNode subNode in segNode)
                {
                    string cluster = "Unknown";
                    string subcluster = "Unknown";
                    string subgroup = String.Empty;

                    subgroup = subNode.Name;

                    foreach (XmlNode hapNode in subNode.ChildNodes)
                    {
                        string hap = hapNode.Name.Substring(3, 1);
                        foreach (XmlNode chrNode in hapNode.ChildNodes)
                        {
                            string chromosome = chrNode.Name;
                            //foreach (XmlNode element in chrNode.ChildNodes)
                            {
                                string start = chrNode.FirstChild.InnerText;
                                string end = chrNode.LastChild.InnerText;

                                if (hap.Equals("1"))
                                {
                                    sw.WriteLine(chromosome.ToString() + "," + start.ToString() + "," + end.ToString() + "," + cluster + "," + subcluster + "," + subgroup + ",,,,,");

                                }
                                else
                                {
                                    sw.WriteLine(chromosome.ToString() + ",,,,,," + start.ToString() + "," + end.ToString() + "," + cluster + "," + subcluster + "," + subgroup);
                                }
                            }
                        }
                    }
                }
            }
            catch  //If we can't parse it, just write out the original...
            {
                sw.WriteLine(respHtml);
            }
        }

        private static T ConvertNode<T>(XmlNode node) where T : class
        {
            MemoryStream stm = new MemoryStream();

            StreamWriter stw = new StreamWriter(stm);
            stw.Write("<afr>");
            stw.Write(node.InnerXml);
            stw.Write("</afr>");
            stw.Flush();

            stm.Position = 0;

            XmlSerializer ser = new XmlSerializer(typeof(T),"afr");
            T result = (ser.Deserialize(stm) as T);

            return result;
        }

        public class relfinder
        {
            public List<relfinderm> matches;
            public string user_to_user_messaging_disabled_admin;
            public string relfinder_intros_blocked_admin;
            public string action_session;
            public string changes;
        }
        public class relfinderm
        {
            public relfinderm()
            {
                anc = String.Empty;
                surs = String.Empty;
                locs = String.Empty;
                desc = String.Empty;
            }

            public string resend_date;
            public string can_resend;
            public string anc; 
            public string rel_alg; 
            public string segs; 
            public string sex;
            public string locs; 
            public string added; 
            public string year; 
            public string patsid; 
            public string rel_label; 
            public string pat; 
            public string img; 
            public string res; 
            public string pct; 
            public string intro_status; 
            public string full; 
            public string mat; 
            public string matside; 
            public string eiid; 
            public string ehid; 
            public string surs; 
            public string birth; 
            public string rel_user; 
            public string rel_alg_label; 
            public string desc; 
            public string last; 
            public string visible; 
            public string url; 
            public string disc; 
            public string eid; 
            public string share_status;
            public string rel_range; 
            public string first; 
        }

        public class af_main
        {
            public string msg;
            public List<af_result> ancfinder_result;
            public string code;
            public string null_record;
        }
        
        public class af_result
        {
            public string scaling;
            public afr af_record;
        }
        
        public class afr
        {
            public aancestry ancestry;
            public List<asegments> segments;
            public apublic public_data;
        }
        public class asegments
        {
            public int st;
            public int nd;
            public int cm;
            public float sl;
        }

        public class aancestry
        {
            public atype pgf;
            public atype pgm;
            public atype mgf;
            public atype mgm;
        }

        public class atype
        {
            public string bc;
            public bool ashk;
        }

        public class apublic
        {
            public string picture_url;
            public string profile_url;
            public string label;
        }
        
        private void ProcessCookies(HttpWebResponse resp)
        {
            //s_cc = resp.Cookies;
        }

        public HttpWebRequest CreateRequest(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.CookieContainer = s_cc; // reuse cookie contianer across requests
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.1 (KHTML, like Gecko) Chrome/21.0.1180.79 Safari/537.1";
            return request;
        }
    }
}
