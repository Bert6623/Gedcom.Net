using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;

using System.Linq;
using Newtonsoft.Json;
using System.Xml;


namespace FamilyTreeDNA
{
    public class FamilyTreeDNA
    {
        public string WorkDir { get; set; }
        public string WorkIndiDir { get; set; }
        public bool zip { get; set; }
        CookieContainer s_cc;
        StreamWriter swMatch;
        public bool gatherGedcom { get; set; }
        //StreamWriter swSurname;
        //private GedcomDatabase _gd1;
        //System.Collections.Hashtable htIndi;
        List<string> lstMatch = new List<string>();
        List<string> icwMatch = new List<string>();
        System.Collections.Hashtable hshMatch = new System.Collections.Hashtable();


        public FamilyTreeDNA()
        {
            s_cc = new CookieContainer();
            gatherGedcom = false;
        }

        public void Login(string userName, string password)
        {
            StreamWriter swlog = new StreamWriter(WorkDir + "../" + userName + ".log", true);
            swlog.AutoFlush = true;

            HttpWebRequest request = CreateRequest("https://my.familytreedna.com/login.aspx");
            request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(userName + ":" + password)));
            HttpWebResponse resp = (HttpWebResponse)request.GetResponse();
            string respHtml = new StreamReader(resp.GetResponseStream()).ReadToEnd();
            string viewState = ExtractViewState(respHtml);
           
            try { swlog.WriteLine("Login " + userName + "/" + password); }
            catch { }

            request = CreateRequest("https://my.familytreedna.com/login.aspx");
            request.Method = "POST";
            request.Headers.Add("X-MicrosoftAjax", "Delta=true");
            request.ContentType = "application/x-www-form-urlencoded";
            Cookie c = new Cookie("__utma", "168171965.1690722681.1337792889.1337792889.1337792889.1", "//", ".familytreedna.com");
            c.HttpOnly = true;
            request.CookieContainer.Add(c);
            c = new Cookie("__utmb", "168171965.1.10.1337792889", "//", ".familytreedna.com");
            c.HttpOnly = true;
            request.CookieContainer.Add(c);
            c = new Cookie("__utmc", "168171965", "//", ".familytreedna.com");
            c.HttpOnly = true;
            request.CookieContainer.Add(c);
            c = new Cookie("__utmz", "168171965.1337792889.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none)", "//", ".familytreedna.com");
            c.HttpOnly = true; 
            request.CookieContainer.Add(c);
            
            //__EventValidation
            string eventValidationFlag = "id=\"__EVENTVALIDATION\" value=\"";
            int i = respHtml.IndexOf(eventValidationFlag) + eventValidationFlag.Length;
            int j = respHtml.IndexOf("\"", i);
            string eventValidation = respHtml.Substring(i, j - i);
            eventValidation = System.Web.HttpUtility.UrlEncode(eventValidation);

            using (StreamWriter w = new StreamWriter(request.GetRequestStream()))
            {
                string payload = "__EVENTTARGET=&__EVENTARGUMENT=&__EVENTVALIDATION=" + eventValidation + "&__VIEWSTATE=" + viewState + "&LoginView1%24Login1%24UserName=" + userName + "&" + "LoginView1%24Login1%24Password=" + password + "&LoginView1%24Login1%24LoginButton=Log+In&LoginView1%24txtKit=&LoginView1%24txtEmail";
                w.Write(payload);
                w.Flush();
                w.Close();
            }
            resp = (HttpWebResponse)request.GetResponse();
            respHtml = new StreamReader(resp.GetResponseStream()).ReadToEnd();

            //Now get the real screen
            //System.Threading.Thread.Sleep(1500);
            request = CreateRequest("https://my.familytreedna.com/");
            resp = (HttpWebResponse)request.GetResponse();
            respHtml = new StreamReader(resp.GetResponseStream()).ReadToEnd();

            try { swlog.WriteLine("Family Finder"); }
            catch { }

            swMatch = new StreamWriter(WorkDir + userName + "_Family_Finder_Matches.csv");
            swMatch.WriteLine("Full Name,Match Date,Relationship Range,Suggested Relationship,Shared cM,Longest Block,Known Relationship,E-mail,Ancestral Surnames (Bolded names match your surnames),notes");
            WriteMatchFile(userName);
            swMatch.Flush();
            swMatch.Close();

            //System.Threading.Thread.Sleep(1500);
            request = CreateRequest("https://my.familytreedna.com/family-finder-chromosome-browser_v2.aspx");
            resp = (HttpWebResponse)request.GetResponse();
            respHtml = new StreamReader(resp.GetResponseStream()).ReadToEnd();

            bool repeat = true;
            int rownum = 0;
            int totalcount = 0;
            int page = 1;
            string name = String.Empty;
            string resultid2 = String.Empty;
            //fswSurname = new StreamWriter(userName + "_SurnameList.csv");
            //swSurname.WriteLine("Full Name,Match Date,Relationship Range,Suggested Relationship,Shared cM,Longest Block,Known Relationship,E-mail,Ancestral Surnames (Bolded names match your surnames),notes");
            StreamWriter swchrom = new StreamWriter(WorkDir + userName + "_ChromsomeBrowser.csv");
            while (repeat)
            {
                request = CreateRequest("https://my.familytreedna.com/ftdnawebservice/FamilyFinderOmni700.asmx/getMatchdata");
                request.Method = "POST";
                request.ContentType = "application/json; charset=utf-8";
                string postData = "{'ekit': 'SX67iaoAkkw%3d','page': '" + page.ToString() + "','filter': '0','hide3rdparty': 'false', name: ''}";
                using (Stream s = request.GetRequestStream())
                {
                    using (StreamWriter sw = new StreamWriter(s))
                        sw.Write(postData);
                }

                //get response-stream, and use a streamReader to read the content
                using (Stream s = request.GetResponse().GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(s))
                    {
                        string jsonData = sr.ReadToEnd();
                        jsonData.Replace("{\"d\":[", "").Replace("]}", "");
                        try
                        {
                            foreach (string rec in jsonData.Split('{'))
                            {
                                foreach (string field in rec.Split(','))
                                {
                                    if (field.StartsWith("\"name\":"))
                                    {
                                        name = field.Substring(8, field.Length - 8);
                                    }
                                    if (field.StartsWith("\"resultid2\":"))
                                    {
                                        resultid2 = field.Substring(12, field.Length - 12);
                                        if (!lstMatch.Contains(resultid2))
                                            lstMatch.Add(resultid2);

                                    }
                                    if (field.StartsWith("\"totalcount\":"))
                                    {
                                        totalcount = int.Parse(field.Substring(13, field.Length - 13));
                                        //Console.WriteLine(field.Substring(13, field.Length - 13));
                                    }
                                    if (field.StartsWith("\"rownum\":"))
                                    {
                                        rownum = int.Parse(field.Substring(9, field.Length - 9));
                                        //Console.WriteLine(field.Substring(7, field.Length - 7));
                                        if (rownum == totalcount)
                                            repeat = false;
                                    }
                                }

                                if (resultid2.Trim().Length > 0)
                                    WriteChromosome(swchrom, name, resultid2);
                                swchrom.Flush();
                                resultid2 = String.Empty;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message + ": " + ex.StackTrace);
                        }

                        page++;
                    }
                }
            }
            swchrom.Close();
            try { swlog.WriteLine("ICW"); }
            catch { }


            WriteICWFile(userName);

            try { swlog.WriteLine("Finished"); swlog.Close(); }
            catch { }

        }

        private void WriteMatchFile(string userName)
        {
            HttpWebRequest request = CreateRequest("https://my.familytreedna.com/family-finder-matches.aspx");
            HttpWebResponse resp = (HttpWebResponse)request.GetResponse();
            string respHtml = new StreamReader(resp.GetResponseStream()).ReadToEnd();
            string viewState = ExtractViewState(respHtml);

            request = CreateRequest("https://my.familytreedna.com/family-finder-matches.aspx");
            request.Method = "POST";
            request.Headers.Add("X-MicrosoftAjax", "Delta=true");
            request.ContentType = "application/x-www-form-urlencoded";
            //__EventValidation
            string eventValidationFlag = "id=\"__EVENTVALIDATION\" value=\"";
            int i = respHtml.IndexOf(eventValidationFlag) + eventValidationFlag.Length;
            int j = respHtml.IndexOf("\"", i);
            string eventValidation = respHtml.Substring(i, j - i);
            eventValidation = System.Web.HttpUtility.UrlEncode(eventValidation);

            Cookie c = new Cookie("genealogyMsg", "False", "//", ".familytreedna.com");
            c.HttpOnly = true;
            request.CookieContainer.Add(c);
            c = new Cookie("intYourResults", "False", "//", ".familytreedna.com");
            c.HttpOnly = true;
            request.CookieContainer.Add(c);
            c = new Cookie("relNotesMsg", "40", "//", ".familytreedna.com");
            c.HttpOnly = true;
            request.CookieContainer.Add(c);

            using (StreamWriter w = new StreamWriter(request.GetRequestStream()))
            {
                string payload = "__EVENTTARGET=&__EVENTARGUMENT=&__LASTFOCUS=&__VIEWSTATE=" + viewState + "&__VIEWSTATEENCRYPTED=&__EVENTVALIDATION=" + eventValidation + "&ctl00%24hfKitNum="+userName+"&ctl00%24hfProceduresID=51&ctl00%24MainContent%24hfFilterText=&ctl00%24MainContent%24ddlFilterMatches=0&ctl00%24MainContent%24tbName=&ctl00%24MainContent%24tbSurnames=&ctl00%24MainContent%24btnCsvDld=CSV";
                w.Write(payload);
                w.Flush();
                w.Close();
            }
            resp = (HttpWebResponse)request.GetResponse();
            StreamReader sr = new StreamReader(resp.GetResponseStream());
            //First line has the header information
            sr.ReadLine();
            while (!sr.EndOfStream)
            {
                String respLine;
                respLine = sr.ReadLine();
                swMatch.WriteLine(respLine);
                int pos = 0;
                string name=String.Empty;
                foreach (string col in respLine.Split(','))
                {
                    pos++;
                    if (pos==1 && col.Length > 0) 
                    {
                        name = col.Replace("\"", "");
                    }
                    if (pos == 7)//Known relationship
                    {
                        Console.WriteLine("Checking " + name + " - " + col);
                        if (col.Trim().Length > 3 && !col.Trim().ToLower().Equals("(pending)") && !col.Trim().Equals("\"(Pending)\""))
                        {
                            icwMatch.Add(name.Replace(" ",""));
                        }

                    }
                    if (pos == 8)
                    {
                        
                    }
                }
            }

            //respHtml = new StreamReader(resp.GetResponseStream()).ReadToEnd();
            //swMatch.Write(respHtml);
    }


        private void WriteICWFile(string userName)
        {
            HttpWebRequest request = CreateRequest("https://my.familytreedna.com/family-finder-matches.aspx");
            HttpWebResponse resp = (HttpWebResponse)request.GetResponse();
            string respHtml = new StreamReader(resp.GetResponseStream()).ReadToEnd();
            string viewState = ExtractViewState(respHtml);

            StreamWriter swICW = new StreamWriter(WorkDir + userName + "_ICW.csv");

            try
            {
                //Go to In Common Quickly
                request = CreateRequest("https://my.familytreedna.com/family-finder-matches.aspx");
                request.Method = "POST";
                //request.Headers.Add("X-MicrosoftAjax", "Delta=true");
                request.ContentType = "application/x-www-form-urlencoded";
                request.KeepAlive = true;
                request.Referer = "https://my.familytreedna.com/family-finder-matches.aspx";
                //request.Accept = "gzip,deflate,sdch";

                //__EventValidation
                string eventValidationFlag = "id=\"__EVENTVALIDATION\" value=\"";
                int i = respHtml.IndexOf(eventValidationFlag) + eventValidationFlag.Length;
                int j = respHtml.IndexOf("\"", i);
                string eventValidation = respHtml.Substring(i, j - i);
                eventValidation = System.Web.HttpUtility.UrlEncode(eventValidation);

                Cookie c = new Cookie("__utma", "168171965.1690722681.1337792889.1337792889.1337792889.1", "//", ".familytreedna.com");
                c.HttpOnly = true;
                request.CookieContainer.Add(c);
                c = new Cookie("__utmb", "168171965.1.10.1337792889", "//", ".familytreedna.com");
                c.HttpOnly = true;
                request.CookieContainer.Add(c);
                c = new Cookie("__utmc", "168171965", "//", ".familytreedna.com");
                c.HttpOnly = true;
                request.CookieContainer.Add(c);
                c = new Cookie("__utmz", "168171965.1337792889.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none)", "//", ".familytreedna.com");
                c.HttpOnly = true;
                request.CookieContainer.Add(c);
                c = new Cookie("genealogyMsg", "False", "//", ".familytreedna.com");
                c.HttpOnly = true;
                request.CookieContainer.Add(c);
                c = new Cookie("intYourResults", "False", "//", ".familytreedna.com");
                c.HttpOnly = true;
                request.CookieContainer.Add(c);
                c = new Cookie("relNotesMsg", "31", "//", ".familytreedna.com");
                c.HttpOnly = true;
                request.CookieContainer.Add(c);

                using (StreamWriter w = new StreamWriter(request.GetRequestStream()))
                {
                    //                string payload = "__EVENTTARGET=ctl00%24MainContent%24gvMatchResults%24ctl01%24timer2&__EVENTARGUMENT=&__LASTFOCUS=&__VIEWSTATE=" + viewState + "&__VIEWSTATEENCRYPTED=&__EVENTVALIDATION=" + eventValidation + "&ctl00%24hfKitNum=" + userName + "&ctl00%24hfProceduresID=51&ctl00%24MainContent%24hfFilterText=&ctl00%24MainContent%24ddlFilterMatches=7&ctl00%24MainContent%24tbName=&ctl00%24MainContent%24tbSurnames=";
                    string payload = "__EVENTTARGET=ctl00%24MainContent%24ddlFilterMatches&__EVENTARGUMENT=&__LASTFOCUS=&__VIEWSTATE=" + viewState + "&__VIEWSTATEENCRYPTED=&__EVENTVALIDATION=" + eventValidation + "&ctl00%24hfKitNum=" + userName + "&ctl00%24hfProceduresID=51&ctl00%24MainContent%24hfFilterText=&ctl00%24MainContent%24ddlFilterMatches=5&ctl00%24MainContent%24tbName=&ctl00%24MainContent%24tbSurnames=";
                    w.Write(payload);
                    w.Flush();
                    w.Close();
                }
                resp = (HttpWebResponse)request.GetResponse();
                respHtml = (new StreamReader(resp.GetResponseStream())).ReadToEnd();

                viewState = ExtractViewState(respHtml);


                foreach (string relativeName in lstMatch)
                {
                    if (!hshMatch.Contains(relativeName))
                    {
                        Console.WriteLine("Skipping HASH ICW for " + relativeName);
                        continue;
                    }
                    string hashName = hshMatch[relativeName].ToString();

                    if (!icwMatch.Contains(hashName.Replace(" ", "")))
                    {
                        Console.WriteLine("Skipping ICW for " + hashName);
                        continue;
                    }

                    Console.WriteLine("Running ICW for " + hashName);
                    request = CreateRequest("https://my.familytreedna.com/family-finder-matches.aspx");
                    request.Method = "POST";
                    //request.Headers.Add("X-MicrosoftAjax", "Delta=true");
                    request.ContentType = "application/x-www-form-urlencoded";

                    //__EventValidation
                    eventValidationFlag = "id=\"__EVENTVALIDATION\" value=\"";
                    i = respHtml.IndexOf(eventValidationFlag) + eventValidationFlag.Length;
                    j = respHtml.IndexOf("\"", i);
                    eventValidation = respHtml.Substring(i, j - i);
                    eventValidation = System.Web.HttpUtility.UrlEncode(eventValidation);

                    c = new Cookie("genealogyMsg", "False", "//", ".familytreedna.com");
                    c.HttpOnly = true;
                    request.CookieContainer.Add(c);
                    c = new Cookie("intYourResults", "False", "//", ".familytreedna.com");
                    c.HttpOnly = true;
                    request.CookieContainer.Add(c);
                    c = new Cookie("relNotesMsg", "31", "//", ".familytreedna.com");
                    c.HttpOnly = true;
                    request.CookieContainer.Add(c);

                    using (StreamWriter w = new StreamWriter(request.GetRequestStream()))
                    {
                        string payload = "__EVENTTARGET=ctl00%24MainContent%24ddlRelativeName&__EVENTARGUMENT=&__LASTFOCUS=&__VIEWSTATE=" + viewState + "&__VIEWSTATEENCRYPTED=&__EVENTVALIDATION=" + eventValidation + "&ctl00%24hfKitNum=" + userName + "&ctl00%24hfProceduresID=51&ctl00%24MainContent%24hfFilterText=&ctl00%24MainContent%24ddlFilterMatches=5&ctl00%24MainContent%24ddlRelativeName=" + relativeName + "&ctl00%24MainContent%24tbName=&ctl00%24MainContent%24tbSurnames=";
                        w.Write(payload);
                        w.Flush();
                        w.Close();
                    }
                    resp = (HttpWebResponse)request.GetResponse();
                    StreamReader sr = new StreamReader(resp.GetResponseStream());
                    respHtml = sr.ReadToEnd();
                    viewState = ExtractViewState(respHtml);

                    //Get CSV
                    request = CreateRequest("https://my.familytreedna.com/family-finder-matches.aspx");
                    request.Method = "POST";
                    //request.Headers.Add("X-MicrosoftAjax", "Delta=true");
                    request.ContentType = "application/x-www-form-urlencoded";

                    //__EventValidation
                    eventValidationFlag = "id=\"__EVENTVALIDATION\" value=\"";
                    i = respHtml.IndexOf(eventValidationFlag) + eventValidationFlag.Length;
                    j = respHtml.IndexOf("\"", i);
                    eventValidation = respHtml.Substring(i, j - i);
                    eventValidation = System.Web.HttpUtility.UrlEncode(eventValidation);

                    c = new Cookie("genealogyMsg", "False", "//", ".familytreedna.com");
                    c.HttpOnly = true;
                    request.CookieContainer.Add(c);
                    c = new Cookie("intYourResults", "False", "//", ".familytreedna.com");
                    c.HttpOnly = true;
                    request.CookieContainer.Add(c);
                    c = new Cookie("relNotesMsg", "31", "//", ".familytreedna.com");
                    c.HttpOnly = true;
                    request.CookieContainer.Add(c);

                    using (StreamWriter w = new StreamWriter(request.GetRequestStream()))
                    {
                        string payload = "__EVENTTARGET=&__EVENTARGUMENT=&__LASTFOCUS=&__VIEWSTATE=" + viewState + "&__VIEWSTATEENCRYPTED=&__EVENTVALIDATION=" + eventValidation + "&ctl00%24hfKitNum=" + userName + "&ctl00%24hfProceduresID=51&ctl00%24MainContent%24hfFilterText=&ctl00%24MainContent%24ddlFilterMatches=5&ctl00%24MainContent%24ddlRelativeName=" + relativeName + "&ctl00%24MainContent%24tbName=&ctl00%24MainContent%24tbSurnames=&ctl00%24MainContent%24btnCsvDld=CSV";
                        w.Write(payload);
                        w.Flush();
                        w.Close();
                    }
                    resp = (HttpWebResponse)request.GetResponse();
                    sr = new StreamReader(resp.GetResponseStream());
                    //First line has the header information
                    sr.ReadLine();
                    while (!sr.EndOfStream)
                    {
                        String respLine;
                        respLine = sr.ReadLine();
                        if (respLine.Trim().Length > 0 && !respLine.Equals("\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\""))
                            swICW.WriteLine("\"" + hashName + "\"," + respLine);
                    }
                    swICW.Flush();


                }
            }
            finally
            {
                if (swICW != null)
                    swICW.Close();
            }
        }

        private void SetDistant(string userName)
        {
            HttpWebRequest request = CreateRequest("https://my.familytreedna.com/family-finder-matches.aspx");
            HttpWebResponse resp = (HttpWebResponse)request.GetResponse();
            string respHtml = new StreamReader(resp.GetResponseStream()).ReadToEnd();
            string viewState = ExtractViewState(respHtml);

            //Go to In Common Quickly
            request = CreateRequest("https://my.familytreedna.com/family-finder-matches.aspx");
            request.Method = "POST";
            //request.Headers.Add("X-MicrosoftAjax", "Delta=true");
            request.ContentType = "application/x-www-form-urlencoded";
            request.KeepAlive = true;
            request.Referer = "https://my.familytreedna.com/family-finder-matches.aspx";
            //request.Accept = "gzip,deflate,sdch";

            //__EventValidation
            string eventValidationFlag = "id=\"__EVENTVALIDATION\" value=\"";
            int i = respHtml.IndexOf(eventValidationFlag) + eventValidationFlag.Length;
            int j = respHtml.IndexOf("\"", i);
            string eventValidation = respHtml.Substring(i, j - i);
            eventValidation = System.Web.HttpUtility.UrlEncode(eventValidation);

            Cookie c = new Cookie("__utma", "168171965.1690722681.1337792889.1337792889.1337792889.1", "//", ".familytreedna.com");
            c.HttpOnly = true;
            request.CookieContainer.Add(c);
            c = new Cookie("__utmb", "168171965.1.10.1337792889", "//", ".familytreedna.com");
            c.HttpOnly = true;
            request.CookieContainer.Add(c);
            c = new Cookie("__utmc", "168171965", "//", ".familytreedna.com");
            c.HttpOnly = true;
            request.CookieContainer.Add(c);
            c = new Cookie("__utmz", "168171965.1337792889.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none)", "//", ".familytreedna.com");
            c.HttpOnly = true;
            request.CookieContainer.Add(c);
            c = new Cookie("genealogyMsg", "False", "//", ".familytreedna.com");
            c.HttpOnly = true;
            request.CookieContainer.Add(c);
            c = new Cookie("intYourResults", "False", "//", ".familytreedna.com");
            c.HttpOnly = true;
            request.CookieContainer.Add(c);
            c = new Cookie("relNotesMsg", "31", "//", ".familytreedna.com");
            c.HttpOnly = true;
            request.CookieContainer.Add(c);

            using (StreamWriter w = new StreamWriter(request.GetRequestStream()))
            {
                //                string payload = "__EVENTTARGET=ctl00%24MainContent%24gvMatchResults%24ctl01%24timer2&__EVENTARGUMENT=&__LASTFOCUS=&__VIEWSTATE=" + viewState + "&__VIEWSTATEENCRYPTED=&__EVENTVALIDATION=" + eventValidation + "&ctl00%24hfKitNum=" + userName + "&ctl00%24hfProceduresID=51&ctl00%24MainContent%24hfFilterText=&ctl00%24MainContent%24ddlFilterMatches=7&ctl00%24MainContent%24tbName=&ctl00%24MainContent%24tbSurnames=";
                string payload = "__EVENTTARGET=ctl00%24MainContent%24ddlFilterMatches&__EVENTARGUMENT=&__LASTFOCUS=&__VIEWSTATE=" + viewState + "&__VIEWSTATEENCRYPTED=&__EVENTVALIDATION=" + eventValidation + "&ctl00%24hfKitNum=" + userName + "&ctl00%24hfProceduresID=51&ctl00%24MainContent%24hfFilterText=&ctl00%24MainContent%24ddlFilterMatches=5&ctl00%24MainContent%24tbName=&ctl00%24MainContent%24tbSurnames=";
                w.Write(payload);
                w.Flush();
                w.Close();
            }
            resp = (HttpWebResponse)request.GetResponse();
            respHtml = (new StreamReader(resp.GetResponseStream())).ReadToEnd();

            viewState = ExtractViewState(respHtml);


            foreach (string relativeName in lstMatch)
            {
                //Get Data
                request = CreateRequest("https://my.familytreedna.com/family-finder-matches.aspx");
                request.Method = "POST";
                //request.Headers.Add("X-MicrosoftAjax", "Delta=true");
                request.ContentType = "application/x-www-form-urlencoded";

                //__EventValidation
                eventValidationFlag = "id=\"__EVENTVALIDATION\" value=\"";
                i = respHtml.IndexOf(eventValidationFlag) + eventValidationFlag.Length;
                j = respHtml.IndexOf("\"", i);
                eventValidation = respHtml.Substring(i, j - i);
                eventValidation = System.Web.HttpUtility.UrlEncode(eventValidation);

                c = new Cookie("genealogyMsg", "False", "//", ".familytreedna.com");
                c.HttpOnly = true;
                request.CookieContainer.Add(c);
                c = new Cookie("intYourResults", "False", "//", ".familytreedna.com");
                c.HttpOnly = true;
                request.CookieContainer.Add(c);
                c = new Cookie("relNotesMsg", "31", "//", ".familytreedna.com");
                c.HttpOnly = true;
                request.CookieContainer.Add(c);

                using (StreamWriter w = new StreamWriter(request.GetRequestStream()))
                {
                    string payload = "__EVENTTARGET=ctl00%24MainContent%24ddlRelativeName&__EVENTARGUMENT=&__LASTFOCUS=&__VIEWSTATE=" + viewState + "&__VIEWSTATEENCRYPTED=&__EVENTVALIDATION=" + eventValidation + "&ctl00%24hfKitNum=" + userName + "&ctl00%24hfProceduresID=51&ctl00%24MainContent%24hfFilterText=&ctl00%24MainContent%24ddlFilterMatches=5&ctl00%24MainContent%24ddlRelativeName=" + relativeName + "&ctl00%24MainContent%24tbName=&ctl00%24MainContent%24tbSurnames=&ctl00%24MainContent%24gvMatchResults%24ctl10%24assignRelationshipBtn=Assign";
                    w.Write(payload);
                    w.Flush();
                    w.Close();
                }
                resp = (HttpWebResponse)request.GetResponse();
                StreamReader sr = new StreamReader(resp.GetResponseStream());
                respHtml = sr.ReadToEnd();
                viewState = ExtractViewState(respHtml);

                //Save Data
                request = CreateRequest("https://my.familytreedna.com/family-finder-matches.aspx");
                request.Method = "POST";
                //request.Headers.Add("X-MicrosoftAjax", "Delta=true");
                request.ContentType = "application/x-www-form-urlencoded";

                //__EventValidation
                eventValidationFlag = "id=\"__EVENTVALIDATION\" value=\"";
                i = respHtml.IndexOf(eventValidationFlag) + eventValidationFlag.Length;
                j = respHtml.IndexOf("\"", i);
                eventValidation = respHtml.Substring(i, j - i);
                eventValidation = System.Web.HttpUtility.UrlEncode(eventValidation);

                c = new Cookie("genealogyMsg", "False", "//", ".familytreedna.com");
                c.HttpOnly = true;
                request.CookieContainer.Add(c);
                c = new Cookie("intYourResults", "False", "//", ".familytreedna.com");
                c.HttpOnly = true;
                request.CookieContainer.Add(c);
                c = new Cookie("relNotesMsg", "31", "//", ".familytreedna.com");
                c.HttpOnly = true;
                request.CookieContainer.Add(c);

                using (StreamWriter w = new StreamWriter(request.GetRequestStream()))
                {
                    string payload = "__EVENTTARGET=&__EVENTARGUMENT=&__LASTFOCUS=&__VIEWSTATE=" + viewState + "&__VIEWSTATEENCRYPTED=&__EVENTVALIDATION=" + eventValidation + "&ctl00%24hfKitNum=" + userName + "&ctl00%24hfProceduresID=51&ctl00%24MainContent%24hfFilterText=&ctl00%24MainContent%24ddlFilterMatches=0&ctl00%24MainContent%24tbName=&ctl00%24MainContent%24tbSurnames=&ctl00%24MainContent%24gvMatchResults%24ctl10%24assignRelationshipDdl=15&ctl00%24MainContent%24gvMatchResults%24ctl10%24saveRelationshipBtn=Save";
                    w.Write(payload);
                    w.Flush();
                    w.Close();
                }
                resp = (HttpWebResponse)request.GetResponse();
                respHtml = new StreamReader(resp.GetResponseStream()).ReadToEnd();


            }
        }


        private void WriteChromosome(StreamWriter swchrom, string name, string resultid2)
        {
            HttpWebRequest request = CreateRequest("https://my.familytreedna.com/ftdnawebservice/FamilyFinderOmni700.asmx/getUserMatchdataNew");
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";
            string postData = "{'ekit': 'SX67iaoAkkw%3d','resultid2': '" + resultid2 + "'}";
            using (Stream s = request.GetRequestStream())
            {
                using (StreamWriter sw = new StreamWriter(s))
                    sw.Write(postData);
            }

            //get response-stream, and use a streamReader to read the content
            using (Stream s = request.GetResponse().GetResponseStream())
            {
                using (StreamReader sr = new StreamReader(s))
                {
                    string jsonData = sr.ReadToEnd();
                    System.Xml.XmlDocument doc = (System.Xml.XmlDocument)JsonConvert.DeserializeXmlNode(jsonData);
                    XmlNode xn = doc.SelectSingleNode("/d");
                    name = xn["name"].InnerText.Replace("  ", " ").Replace("  ", " ");

                    if (!hshMatch.Contains(resultid2) )
                        hshMatch.Add(resultid2, name);

                    //Full Name	Match Date	Relationship Range	Suggested Relationship	Shared cM	Longest Block	Known Relationship	E-mail	Ancestral Surnames (Bolded names match your surnames)	notes

                    //Get Name
                    //Get ID
                    //Get Predicted
                    //Get Range
                    //Get cm
                    //Get segments

                    //Get cm list
                    int loc = jsonData.IndexOf("[[");
                    jsonData = jsonData.Substring(loc, jsonData.Length-loc-3);
                   try
                    {
                        int chromosome = -2;
                        foreach (string chromData in jsonData.Split('['))
                        {
                            chromosome++;
                            foreach (string rec in chromData.Split('{'))
                            {
                                string cm = String.Empty;
                                string snps = String.Empty;
                                string p1 = String.Empty;
                                string p2 = String.Empty;
                                foreach (string field in rec.Split(','))
                                {
                                    if (field.StartsWith("\"cm\":"))
                                    {
                                        cm = field.Substring(5, field.Length - 5);
                                    }
                                    if (field.StartsWith("\"snps\":"))
                                    {
                                        snps = field.Substring(7, field.Length - 7);
                                    }
                                    if (field.StartsWith("\"p1\":"))
                                    {
                                        p1 = field.Substring(5, field.Length - 5);
                                    }
                                    if (field.StartsWith("\"p2\":"))
                                    {
                                        p2 = field.Substring(5, field.Length - 5).Replace("}]", "").Replace("}", "");
                                    }
                                }
                                if (cm.Length > 0)
                                    swchrom.WriteLine(",\"" + name + "\"," + chromosome.ToString() + "," + p1 + "," + p2 +  "," + cm + "," + snps + "," + resultid2);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + ": " + ex.StackTrace);
                    }

                }
            }

        }

        public string GetGedcom(string gedInfo)
        {
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create("http://kelleyllc.com/ftdna-gedcom/");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/536.11 (KHTML, like Gecko) Chrome/20.0.1132.57 Safari/536.11";

            using (StreamWriter w = new StreamWriter(request.GetRequestStream()))
            {
                string payload = "gedInfo=" + System.Web.HttpUtility.UrlEncode(gedInfo);
                w.Write(payload);
                w.Flush();
                w.Close();
            }
            System.Net.HttpWebResponse resp = (System.Net.HttpWebResponse)request.GetResponse();
            string respHtml = new StreamReader(resp.GetResponseStream()).ReadToEnd();
            return respHtml;
        }

        private string ExtractViewState(string s)
        {
            string viewStateNameDelimiter = "__VIEWSTATE";
            string valueDelimiter = "value=\"";

            int viewStateNamePosition = s.IndexOf(viewStateNameDelimiter);
            int viewStateValuePosition = s.IndexOf(
                  valueDelimiter, viewStateNamePosition
               );

            int viewStateStartPosition = viewStateValuePosition +
                                         valueDelimiter.Length;
            int viewStateEndPosition = s.IndexOf("\"", viewStateStartPosition);

            return System.Web.HttpUtility.UrlEncodeUnicode(
                     s.Substring(
                        viewStateStartPosition,
                        viewStateEndPosition - viewStateStartPosition
                     )
                  );
        }

        public HttpWebRequest CreateRequest(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.CookieContainer = s_cc; // reuse cookie contianer across requests
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/536.11 (KHTML, like Gecko) Chrome/20.0.1132.57 Safari/536.11";
            return request;
        }

    }
}
