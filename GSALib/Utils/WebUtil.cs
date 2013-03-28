using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;

namespace GSALib.Utils
{
    public class WebUtil
    {
        public static int getStart(HttpRequest request, string queryStringName)
        {
            int startNum = 0;
            if (request[queryStringName] != null)
            {
                if ((request[queryStringName] != null) && (request[queryStringName].Length>0))
                {
                    startNum = Int32.Parse(request[queryStringName]);
                }
                //else it will use the default
            }
            return startNum;
        }
        public static int getPageSize(HttpRequest request, string pageStringName)
        {
            int num = 25;//Default is 25 for Dorman
            if (request[pageStringName] != null)
            {
                num = Int32.Parse(request[pageStringName]);
            }
            return num;
        }
        public static int getCurrent(int start,int pageSize)
        {
            int num = 0;
            num = (start + pageSize) / pageSize;

            return num;
        }
        public static bool isFirstPage(int start, int pageSize)
        {
            if (start < pageSize)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool isLastPage(int start, int pageSize,int numResults)
        {
            if (((start + pageSize)>numResults) || ((start+pageSize)>=1000))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string createBaseUrl(HttpRequest request)
        {
            StringBuilder baseUrl = new StringBuilder();
            baseUrl.Append(request.Path + "?");
            foreach (string key in request.QueryString.Keys)
            {
                if ((key=="start")  ||
                    (key=="num"))
                {
                    //do nothing
                } else 
                {
                    if (key != null)
                    {
                        baseUrl.Append("&" + key + "=" + request[key]);
                    }
                }
            }

            return baseUrl.ToString();
        }

        public static string createClearUrl(HttpRequest request, string filterName, string filterValue)
        {
            StringBuilder baseUrl = new StringBuilder();
            baseUrl.Append(request.Path + "?");
            string finaldnavs = string.Empty;
            string q = string.Empty;
            foreach (string key in request.QueryString.Keys)
            {
                if ((key == "start") ||
                    (key == "num"))
                {
                    //do nothing
                }
                else if (key == "dnavs")
                {
                    //dnavss=inmeta:make%3DHONDA+inmeta:model%3DACCORD%253A%2520LX
                    IList<KeyValuePair<string,string>> dnavsParm = getSelectedFilters(request);
                    foreach (KeyValuePair<string,string> singleParam in dnavsParm)
                    {
                        if (singleParam.Key!=filterName) {
                            finaldnavs += "+inmeta:" + singleParam.Key + "%3D" + singleParam.Value;
                        }
                    }
                }
                else if (key == "q")
                {
                    if (request[key].IndexOf("inmeta") > 0)
                        q = request[key].Substring(0,request[key].IndexOf("inmeta"));
                }
                else
                {
                    if (key != null)
                    {
                        baseUrl.Append("&" + key + "=" + request[key]);
                    }
                }
            }
            if (filterName != "")
            {
                baseUrl.Append("&q=" + q.Trim() + encodeStringForGSA(finaldnavs));
            }
            else
            {
                baseUrl.Append("&q=" + encodeStringForGSA(finaldnavs));
            }
            baseUrl.Append("&dnavs=" + encodeStringForGSA(finaldnavs));
            return baseUrl.ToString();
        }

        public static IList<KeyValuePair<string,string>> getSelectedFilters(HttpRequest request)
        {
            string[] delimiter = new string[]{" inmeta:"};
            string meta = string.Empty;
            IList<KeyValuePair<string, string>> filters = new List<KeyValuePair<string,string>>();
            if (request["dnavs"]!=null)
            {
                foreach (string filter in request["dnavs"].Split(delimiter, StringSplitOptions.RemoveEmptyEntries))
                {

                    if (filter.IndexOf("meta:") > 0)
                    {
                        meta = filter.Substring(filter.IndexOf("meta:") + 5);
                    }
                    else
                    {
                        meta = filter;
                    }
                    if (meta.Equals("inmeta"))
                    {
                        //ignore
                    }
                    else
                    {
                        string[] keys = meta.Split('=');
                        KeyValuePair<string, string> keyset = new KeyValuePair<string, string>(keys[0], keys[1]);
                        filters.Add(keyset);
                    }
                }
            }

            return filters;
        }
        public static string createFilterUrl(HttpRequest request, string filterName, string filterValue)
        {
            StringBuilder baseUrl = new StringBuilder();
            baseUrl.Append(request.Path + "?");
            bool dnavsFound = false;
            string finaldnavs = string.Empty;
            string q = string.Empty;

            foreach (string key in request.QueryString.Keys)
            {
                if ((key == "start") ||
                    (key == "num"))
                {
                    //do nothing
                }
                else if (key == "dnavs")
                {
                    //dnavss=inmeta:make%3DHONDA+inmeta:model%3DACCORD%253A%2520LX
                    finaldnavs =  request[key];
                    dnavsFound = true;
                }
                else if (key == "q")
                {
                    if (request[key].IndexOf("inmeta") > 0)
                    {
                        q = request[key].Substring(0, request[key].IndexOf("inmeta"));
                    }
                    else
                    {
                        q = request[key];
                    }
                }
                else
                {
                    if (key != null)
                    {
                        baseUrl.Append("&" + key + "=" + request[key]);
                    }
                }
            }

            if (!dnavsFound)
            {
                //No previous filter
                baseUrl.Append("&q=");
                baseUrl.Append(q.Trim());
                
                baseUrl.Append("+inmeta:" + filterName + "%3D" + encodeStringForGSA(filterValue));
                baseUrl.Append("&dnavs=");
                baseUrl.Append("+inmeta:" + filterName + "%3D" + encodeStringForGSA(filterValue));
            } else {
                baseUrl.Append("&q=");
                baseUrl.Append(q.Trim());
                baseUrl.Append("+");
                baseUrl.Append(finaldnavs);
                baseUrl.Append("+inmeta:" + filterName + "%3D" + encodeStringForGSA(filterValue));
                baseUrl.Append("&dnavs=");
                baseUrl.Append(finaldnavs);
                baseUrl.Append("+inmeta:" + filterName + "%3D" + encodeStringForGSA(filterValue));
            }

            return baseUrl.ToString();
        }

        public static string encodeStringForGSA(string value)
        {
            value = value.Replace(" ", "%2520").Replace("-", "%252D").Replace(".", "%252E");
            value = value.Replace(".","%252E").Replace("(","%2528").Replace("!","%2521");
		    value = value.Replace(")", "%2529").Replace(":", "%253A");
            return value;
        }

       
    }
}
