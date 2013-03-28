/********************************************************************************
 *  
 *  Product: GSALib
 *  Description: A C# API for accessing the Google Search Appliance.
 *
 *  (c) Copyright 2008 Michael Cizmar + Associates Ltd.  (MC+A)
 *  
********************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using GSALib.Utils;

namespace GSALib.GSA
{
    /// <summary>
    /// Class for creating query string for submitting http requests to GSA
    /// <para>Author Albert Ghukasyan</para>
    /// </summary>
    internal sealed class QueryBuilder
    {
        #region Variables

        private char as_dt; // {i, e}:{include [as_sitesearch], exclude [as_sitesearch]}
        private String as_epq; // additional query terms
        private String as_eq; // exclude terms
        private String as_lq; // pages linking to this url
        private String as_occt; // {any, title, URL}:{search anywhere on page, search in title, search in url}
        private String[] as_oq; // any of these
        private String[] as_q;

        private String q; // *REQUIRED when sitesearch specified* Search query

        private String as_sitesearch; // *MAX 118 after URL encoding* set the value for this into "site"
        private String sitesearch; // needs q to be supplied
        private String[] sites; // *REQUIRED* (OR-ed) collection of "collections" ()

        private String client; // *REQUIRED* string specifying valid frontend      (default -needs to be speficied- is default_frontend)
        private String output; // *REQUIRED* format for search results      (default -needs to be specified- is xml_no_dtd)
        private String proxycustom; // {<HOME/>,<ADVANCED/>,<TEST/>}
        private bool proxyreload; // {1} force reload of serverside stylesheet (else default reloaded after 15 mins)
        private String proxystylesheet; // if output==xml_no_dtd then {Omitted,}

        private char access; // {p, s, a}:{public, secure, all}
        private char filter; // 0,1,p,s  (default is 1)
        private String lr; // Language restrict

        private String ie; // Input encoding      (default is latin1)
        private String oe; // Output encoding     (default is UTF8)

        private long start; // {0..999} scroll into the search results (constraint: start+num <= 1000)
        private int num; // {1..100} max results per request     (default is 10)
        private byte numgm; // {0..5} max num of keymatches per result     (default is 3)

        private String[] getfields; // get meta tags
        private Hashtable partialfields; // meta tag names and partial-values
        private bool partialFieldsOr = true; // used in conjunction with partialFields

        private Hashtable requiredfields; // meta tag names and complete-values
        private bool requiredFieldsOr = true; // used in conjunction with requiredFields

        private String sort; // Only date is currently supported

        private char AS_DT_INCLUDE = 'i';
        private char AS_DT_EXCLUDE = 'e';
        private char FILTER_DUP_SNIPPET_AND_DIRECTORY = '1';
        private char FILTER_OFF = '0';
        private char FILTER_DUP_SNIPPET = 'p';
        private char FILTER_DUP_DIRECTORY = 's';
        private char _DEFAULT_ACCESS = 'p';
        private String _DEFAULT_OUTPUT = "xml_no_dtd";

        #endregion

        #region Constructor

        internal QueryBuilder(String query, String output)
        {
            this.q = query;
            this.output = output == null ? _DEFAULT_OUTPUT : output;
            this.access = _DEFAULT_ACCESS;
            this.requiredfields = new Hashtable();
            this.partialfields = new Hashtable();
        }

        #endregion

        #region Set Properties

        public void setAccess(char access)
        {
            this.access = access;
        }

        public char getAccess()
        {
            return this.access;
        }

        public void setAs_dt(char as_dt)
        {
            this.as_dt = as_dt;
        }

        public void setAs_epq(String as_epq)
        {
            this.as_epq = as_epq;
        }

        public void setAs_eq(String as_eq)
        {
            this.as_eq = as_eq;
        }

        public void setAs_lq(String as_lq)
        {
            this.as_lq = as_lq;
        }

        public void setAs_occt(String as_occt)
        {
            this.as_occt = as_occt;
        }

        public void setAs_oq(String[] as_oq)
        {
            this.as_oq = as_oq;
        }

        public void setAs_q(String[] as_q)
        {
            this.as_q = as_q;
        }

        public void setAs_sitesearch(String as_sitesearch)
        {
            this.as_sitesearch = as_sitesearch;
        }

        public void setClient(String client)
        {
            this.client = client;
        }

        public void setFilter(char filter)
        {
            this.filter = filter;
        }

        public void setGetfields(String[] getfields)
        {
            this.getfields = getfields;
        }

        public void setPartialfields(Hashtable partialfields, bool orIfTrueAndIfFalse)
        {
            this.partialfields = partialfields;
            this.partialFieldsOr = orIfTrueAndIfFalse;
        }

        public void setRequiredfields(Hashtable requiredfields, bool orIfTrueAndIfFalse)
        {
            this.requiredfields = requiredfields;
            this.requiredFieldsOr = orIfTrueAndIfFalse;
        }

        public void setIe(String ie)
        {
            this.ie = ie;
        }

        public void setLr(String lr)
        {
            this.lr = lr;
        }

        public void setNum(int num)
        {
            this.num = num;
        }

        public void setNumgm(byte numgm)
        {
            this.numgm = numgm;
        }

        public void setOe(String oe)
        {
            this.oe = oe;
        }

        public void setOutput(String output)
        {
            this.output = output;
        }

        public void setProxycustom(String proxycustom)
        {
            this.proxycustom = proxycustom;
        }

        public void setProxyreload(bool proxyreload)
        {
            this.proxyreload = proxyreload;
        }

        public void setProxystylesheet(String proxystylesheet)
        {
            this.proxystylesheet = proxystylesheet;
        }

        public void setQ(String q)
        {
            this.q = q;
        }

        public void setSites(String[] sites)
        {
            this.sites = sites;
        }

        public void setSitesearch(String sitesearch)
        {
            this.sitesearch = sitesearch;
        }

        public void setSort(String sort)
        {
            this.sort = sort;
        }

        public void setStart(long start)
        {
            this.start = start;
        }

        #endregion

        #region GetValue

        /// <summary>
        /// Builds Query via appending string to  String builder ( refer to Util.appendQueryParam)
        /// </summary>
        /// <returns>Builded Query String</returns>
        public String getValue()
        {
            StringBuilder sbuf = new StringBuilder();
            Util.appendQueryParam(sbuf, "access", access.ToString());
            if (output != null) Util.appendQueryParam(sbuf, "output", output);
            if (sort != null) Util.appendQueryParam(sbuf, "sort", sort);
            if (ie != null) Util.appendQueryParam(sbuf, "ie", ie);
            if (oe != null) Util.appendQueryParam(sbuf, "oe", oe);
            Util.appendQueryParam(sbuf, "client", client);
            if (start > 0) Util.appendQueryParam(sbuf, "start", start.ToString());
            if (q != null) Util.appendQueryParam(sbuf, "q", q);
            if (as_dt == AS_DT_INCLUDE || as_dt == AS_DT_EXCLUDE)
                Util.appendQueryParam(sbuf, "as_dt", as_dt.ToString());
            if (as_epq != null) Util.appendQueryParam(sbuf, "as_epq", as_epq);
            if (as_eq != null) Util.appendQueryParam(sbuf, "as_eq", as_eq);
            if (as_lq != null) Util.appendQueryParam(sbuf, "as_lq", as_lq);
            if (as_occt != null) Util.appendQueryParam(sbuf, "as_occt", as_occt);
            if (as_oq != null)
            {
                String temp = Util.SeparatedString(as_oq, null, " ");
                Util.appendQueryParam(sbuf, "as_oq", temp);
            }
            if (as_q != null)
            {
                String temp = Util.SeparatedString(as_q, null, " ");
                Util.appendQueryParam(sbuf, "as_q", temp);
            }

            if (as_sitesearch != null) Util.appendQueryParam(sbuf, "as_sitesearch", as_sitesearch);
            if (filter == FILTER_DUP_DIRECTORY || filter == FILTER_DUP_SNIPPET
                    || filter == FILTER_DUP_SNIPPET_AND_DIRECTORY || filter == FILTER_OFF)
                Util.appendQueryParam(sbuf, "filter", filter.ToString());
            if (lr != null) Util.appendQueryParam(sbuf, "lr", lr);
            if (num > 0) Util.appendQueryParam(sbuf, "num", num.ToString());
            if (numgm > 0) Util.appendQueryParam(sbuf, "numgm", numgm.ToString());
            if (proxycustom != null) Util.appendQueryParam(sbuf, "proxycustom", proxycustom);
            if (proxyreload) Util.appendQueryParam(sbuf, "proxyreload", "1");
            if (proxystylesheet != null) Util.appendQueryParam(sbuf, "proxystylesheet", proxystylesheet);
            if (sitesearch != null) Util.appendQueryParam(sbuf, "sitesearch", sitesearch);
            if (requiredfields != null && requiredfields.Count > 0)
            {
                Util.appendMappedQueryParams(
                        sbuf,
                        "requiredfields",
                        requiredfields,
                        requiredFieldsOr ? "|" : ".");
            }
            if (partialfields != null && partialfields.Count > 0)
            {
                Util.appendMappedQueryParams(
                        sbuf,
                        "partialfields",
                        partialfields,
                        partialFieldsOr ? "|" : ".");
            }
            if (getfields != null && getfields.Length > 0)
            {
                String allFields = Util.SeparatedString(getfields, "", ".");
                Util.appendQueryParam(sbuf, "getfields", allFields);
            }
            if (sites != null && sites.Length > 0)
            {
                String allSites = Util.SeparatedString(sites, "", "|");
                Util.appendQueryParam(sbuf, "site", allSites);
            }
            return sbuf.ToString();
        }

        #endregion
    }
}