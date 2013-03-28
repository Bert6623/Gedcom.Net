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
using GSALib.Constants;

namespace GSALib.GSA
{
    /// <summary>
    /// Class creates Query object that will be used for submitting http requests to GSA
    /// <para>Author Albert Ghukasyan</para>
    /// </summary>
    [Serializable]
    public sealed class Query
    {
        #region Variables

        public static int MAX_RESULTS = 1000;
        public static int MAX_RESULTS_PER_QUERY = 100;
        private QueryBuilder query;
        private QueryTerm queryTerm;

        #endregion

        #region Constructor

        public Query()
        {
            query = new QueryBuilder("", null);
            query.setOutput(Output.XML_NO_DTD.getValue());            
        }

        #endregion

        #region Get/Set Properties

        public void setSiteCollections(String[] siteCollections)
        {
            query.setSites(siteCollections);
        }
        
        public void setFrontend(String frontend)
        {
            query.setClient(frontend);
        }

        public void setOutputFormat(Output of)
        {
            query.setOutput(of.getValue());
        }

        public void setMaxResults(int maxResults)
        {
            query.setNum(Math.Min(maxResults, MAX_RESULTS));
        }

        public void setNumKeyMatches(byte keyMatches)
        {
            query.setNumgm(keyMatches);
        }

        public void setSearchScope(SearchRegion searchRgn)
        {
            query.setAs_occt(searchRgn.getValue());
        }

        public void setFilter(Filtering filtering)
        {
            query.setFilter(filtering.getValue());
        }

        public void setQueryTerm(QueryTerm queryTerm)
        {
            this.queryTerm = queryTerm;
            query.setQ(queryTerm.getValue());
        }

        public void setOrQueryTerms(String[] orTerms)
        {
            query.setAs_oq(orTerms);
        }

        public void setAndQueryTerms(String[] andTerms)
        {
            query.setAs_q(andTerms);
        }

        public void setExactPhraseQueryTerm(String phrase)
        {
            query.setAs_epq(phrase);
        }

        public void setInputEncoding(String inputEncoding)
        {
            query.setIe(inputEncoding);
        }

        public void setOutputEncoding(String outputEncoding)
        {
            query.setOe(outputEncoding);
        }

        public void setLanguage(String language)
        {
            query.setLr(language);
        }

        public void setFetchMetaFields(String[] fields)
        {
            query.setGetfields(fields);
        }

        public void setRequiredMetaFields(Hashtable requiredFields)
        {
            query.setRequiredfields(requiredFields, true);
        }

        public void setRequiredMetaFields(Hashtable requiredFields, bool orIfTrueAndIfFalse)
        {
            query.setRequiredfields(requiredFields, orIfTrueAndIfFalse);
        }

        public void setPartialMetaFields(Hashtable partialFields)
        {
            query.setPartialfields(partialFields, true);
        }

        public void setPartialMetaFields(Hashtable partialFields, bool orIfTrueAndIfFalse)
        {
            query.setPartialfields(partialFields, orIfTrueAndIfFalse);
        }

        public void setSortByDate(bool asc, char mode)
        {
            query.setSort("date:" + (asc
                    ? "A:"
                    : "D:") + mode + ":d1");
        }
        
        public void unsetSortByDate()
        {
            query.setSort(null);
        }

        public void setScrollAhead(int n)
        {
            query.setStart(n);
        }

        public void setAccess(Access access)
        {
            query.setAccess(access.getValue());
        }

        public char getAccess()
        {
            return query.getAccess();
        }

        public void setProxycustom(String proxycustom)
        {
            query.setProxycustom(proxycustom);
        }

        public void setProxystylesheet(String proxystylesheet)
        {
            query.setProxystylesheet(proxystylesheet);
        }

        public void setProxyReload(bool force)
        {
            query.setProxyreload(force);
        }

        public String getValue()
        {
            return query.getValue();
        }

        public String getQueryString()
        {
            return queryTerm.getValue();
        }

        #endregion
    }

    /// <summary>
    /// Class creates QueryTerm object that will be used for submitting http requests to GSA
    /// <para> Class Object should be used to specify additional options of Query</para>
    /// Author Albert Ghukasyan
    /// </summary>
    [Serializable]
    public sealed class QueryTerm
    {
        #region Variables

        private ArrayList inTitleTerms;
        private ArrayList notInTitleTerms;
        private ArrayList inUrlTerms;
        private ArrayList notInUrlTerms;
        private ArrayList includeFiletype;
        private ArrayList excludeFiletype;
        private ArrayList allInTitleTerms;
        private ArrayList allInUrlTerms;
        private String site;
        private bool includeSite;
        private String dateRange;
        private String webDocLocation;
        private String cacheDocLocation;
        private String link;
        private String queryString;

        private const String _IN_TITLE = "intitle:";
        private const String _NOT_IN_TITLE = "-intitle:";
        private const String _IN_URL = "inurl:";
        private const String _NOT_IN_URL = "-inurl:";
        private const String _INCLUDE_FILETYPE = "filetype:";
        private const String _EXCLUDE_FILETYPE = "-filetype:";
        private const String _INCLUDE_SITE = "site:";
        private const String _EXCLUDE_SITE = "-site:";
        private const String _DATERANGE = "daterange:";
        private const String _ALL_IN_TITLE = "allintitle:";
        private const String _ALL_IN_URL = "allinurl:";
        private const String _INFO = "info:";
        private const String _CACHE = "cache:";
        private const String _LINK = "link:";
        private const String _OR = " OR ";
        private const String _SP = " ";

        #endregion

        #region Constructor

        public QueryTerm(String queryString)
        {
            this.queryString = queryString;
        }

        #endregion

        #region Get/Set Properties

        public void setQueryString(String queryString)
        {
            this.queryString = queryString;
        }

        public void setInTitle(ArrayList inTitleTerms)
        {
            this.inTitleTerms = inTitleTerms;
        }

        public void setNotInTitle(ArrayList notInTitleTerms)
        {
            this.notInTitleTerms = notInTitleTerms;
        }

        public QueryTerm addInTitle(String term, bool include)
        {
            if (include) inTitleTerms.Add(term);
            else notInTitleTerms.Add(term);
            return this;
        }

        public void setAllInTitle(ArrayList allInTitleTerms)
        {
            this.allInTitleTerms = allInTitleTerms;
        }
               
        public void setInUrl(ArrayList inUrlTerms)
        {
            this.inUrlTerms = inUrlTerms;
        }

        public void setNotInUrl(ArrayList notInUrlTerms)
        {
            this.notInUrlTerms = notInUrlTerms;
        }

        public QueryTerm addInUrl(String term, bool include)
        {
            if (include) inUrlTerms.Add(term);
            else notInUrlTerms.Add(term);
            return this;
        }

        public void setAllInUrl(ArrayList allInUrlTerms)
        {
            this.allInUrlTerms = allInUrlTerms;
        }

        public void setIncludeFileType(ArrayList filetype)
        {
            this.includeFiletype = filetype;
        }

        public void setExcludeFileType(ArrayList filetype)
        {
            this.excludeFiletype = filetype;
        }
       
        public QueryTerm addFileType(String term, bool include)
        {
            if (include)
            {
                if (null == includeFiletype) includeFiletype = new ArrayList();
                includeFiletype.Add(term);
            }
            else
            {
                if (null == excludeFiletype) excludeFiletype = new ArrayList();
                excludeFiletype.Add(term);
            }
            return this;
        }

        public void setSite(String site, bool include)
        {
            this.includeSite = include;
            this.site = site;
        }

        public void setWebDocument(String docLocation)
        {
            this.webDocLocation = docLocation;
        }

        public void setCachedDocument(String docLocation)
        {
            this.cacheDocLocation = docLocation;
        }

        public void setWithLinksTo(String link)
        {
            this.link = link;
        }

        public void setDateRange(DateTime fromDate, DateTime toDate)
        {
            StringBuilder dateRange = new StringBuilder(fromDate.ToString("YYYY-MM-DD"));
            dateRange.Append("..");
            dateRange.Append(toDate.ToString("YYYY-MM-DD"));
            this.dateRange = dateRange.ToString();
        }

        #endregion

        #region getValue

        public String getValue()
        {
            String retval = null;
            StringBuilder qbuf = new StringBuilder();
            if (allInTitleTerms != null && allInTitleTerms.Count  > 0)
            {
                qbuf.Append(_ALL_IN_TITLE).Append(Util.SeparatedString(allInTitleTerms, null, _SP)).Append(' ');
            }
            if (allInUrlTerms != null && allInUrlTerms.Count > 0)
            {
                qbuf.Append(_ALL_IN_URL).Append(Util.SeparatedString(allInUrlTerms, null, _SP)).Append(' ');
            }

            if (webDocLocation != null && webDocLocation.Length  > 0)
            {
                qbuf.Append(_INFO).Append(webDocLocation).Append(' ');
            }

            if (cacheDocLocation != null && cacheDocLocation.Length > 0)
            {
                qbuf.Append(_CACHE).Append(cacheDocLocation).Append(' ');
            }
            if (link != null && link.Length > 0)
            {
                qbuf.Append(_LINK).Append(link);
            }

            if (inTitleTerms != null && inTitleTerms.Count > 0)
            {
                qbuf.Append(Util.SeparatedString(inTitleTerms, _IN_TITLE, _SP)).Append(' ');
            }
            if (notInTitleTerms != null && notInTitleTerms.Count > 0)
            {
                qbuf.Append(Util.SeparatedString(notInTitleTerms, _NOT_IN_TITLE, _SP)).Append(' ');
            }

            if (inUrlTerms != null && inUrlTerms.Count > 0)
            {
                qbuf.Append(Util.SeparatedString(inUrlTerms, _IN_URL, _SP)).Append(' ');
            }
            if (notInUrlTerms != null && notInUrlTerms.Count > 0)
            {
                qbuf.Append(Util.SeparatedString(notInUrlTerms, _NOT_IN_URL, _SP)).Append(' ');
            }

            if (site != null && site.Length > 0)
            {
                qbuf.Append(includeSite
                        ? _INCLUDE_SITE
                        : _EXCLUDE_SITE).Append(site).Append(' ');
            }

            if (includeFiletype != null && includeFiletype.Count > 0)
            {
                qbuf.Append(Util.SeparatedString(includeFiletype, _INCLUDE_FILETYPE, _OR)).Append(' ');
            }
            if (excludeFiletype != null && excludeFiletype.Count > 0)
            {
                qbuf.Append(Util.SeparatedString(excludeFiletype, _EXCLUDE_FILETYPE, _SP)).Append(' ');
            }
            if (dateRange != null)
            {
                qbuf.Append(_DATERANGE).Append(dateRange).Append(' ');
            }

            if (queryString != null) qbuf.Append(queryString);

            retval = qbuf.ToString();
            return retval;
        }

        #endregion       
    }
}
