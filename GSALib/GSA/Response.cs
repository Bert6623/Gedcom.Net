/********************************************************************************
 *  
 *  Product: GSALib
 *  Description: A C# API for accessing the Google Search Appliance.
 *
 *  (c) Copyright 2008 Michael Cizmar + Associates Ltd.  (MC+A)
 *  
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using GSALib.Constants;

namespace GSALib.GSA
{
    /// <summary>
    /// Class for creating Response object based on returned results of GSA
    /// <para>Author Albert Ghukasyan</para>
    /// </summary>
    [Serializable]
    public sealed class Response
    {
        #region Variables

        private double searchTime;
        private String query;
        private Hashtable parameters = new Hashtable();
        private long startIndex;
        private long endIndex;
        private long numResults;
        private bool isFiltered;
        private String previousResponseUrl;
        private String nextResponseUrl;
        private String searchComments;
        private Spelling spelling;
        private ArrayList synonyms = new ArrayList();
        private ArrayList keyMatches = new ArrayList();
        private ArrayList results = new ArrayList();
        private ArrayList oneboxResponses = new ArrayList();
        private ArrayList parametricNavigation = new ArrayList();
        private bool isParametricEstimated;

        #endregion

        #region Constructor

        internal Response()
        {
        }

        #endregion

        #region Get/Set Properties

        public long getEndIndex()
        {
            return endIndex;
        }

        public void setEndIndex(long endIndex)
        {
            this.endIndex = endIndex;
        }

        public bool IsFiltered()
        {
            return isFiltered;
        }

        public void setFiltered(bool filtered)
        {
            isFiltered = filtered;
        }

        public void setResults(ArrayList results)
        {
            this.results = results;
        }

        public void setKeyMatches(ArrayList _keyMatches)
        {
            this.keyMatches = _keyMatches;
        }

        public ArrayList getKeyMatches()
        {
            return this.keyMatches;
        }

        public String getPreviousResponseUrl()
        {
            return previousResponseUrl;
        }

        public void setPreviousResponseUrl(String previousResponseUrl)
        {
            this.previousResponseUrl = previousResponseUrl;
        }

        public String getNextResponseUrl()
        {
            return nextResponseUrl;
        }

        public void setNextResponseUrl(String nextResponseUrl)
        {
            this.nextResponseUrl = nextResponseUrl;
        }

        public String getSearchComments()
        {
            return searchComments;
        }

        public void setSearchComments(String SearchComments)
        {
            this.searchComments = SearchComments;
        }
        
        public ArrayList getOneBoxResponses()
        {
            return oneboxResponses;
        }

        public void setOneBoxResponses(ArrayList oneboxResponses)
        {
            this.oneboxResponses = oneboxResponses;
        }

        public void addOneBoxResponse(OneBoxResponse oneboxResponse)
        {
            this.oneboxResponses.Add(oneboxResponse);
        }

        public bool getIsParametricEstimated()
        {
            return this.isParametricEstimated;
        }

        public void setIsParametricEstimated(bool isParametricEstimated)
        {
            this.isParametricEstimated = isParametricEstimated;
        }

        public ArrayList getParametricResponses()
        {
            return parametricNavigation;
        }

        public void setParametricResponses(ArrayList parametricResponses)
        {
            this.parametricNavigation = parametricResponses;
        }

        public void addParametricResponse(ParametricNavigationResponse parametricResponse)
        {
            this.parametricNavigation.Add(parametricResponse);
        }

        public Spelling getSpelling()
        {
            return spelling;
        }

        public void setSpelling(Spelling spelling)
        {
            this.spelling = spelling;
        }

        public ArrayList getSynonymsWithMarkup()
        {
            return synonyms;
        }

        public void addSynonymWithMarkup(String synonym)
        {
            synonyms.Add(synonym);
        }

        public String getQuery()
        {
            return query;
        }

        public void setQuery(String query)
        {
            this.query = query;
        }

        public long getNumResults()
        {
            return numResults;
        }

        public void setNumResults(long numResults)
        {
            this.numResults = numResults;
        }

        public Hashtable getParams()
        {
            return this.parameters;
        }

        public void setParams(Hashtable parameters)
        {
            this.parameters = parameters;
        }

        public void putParam(String name, String value)
        {
            parameters.Add(name, value);
        }

        public double getSearchTime()
        {
            return searchTime;
        }

        public void setSearchTime(double searchTime)
        {
            this.searchTime = searchTime;
        }

        public long getStartIndex()
        {
            return startIndex;
        }

        public void setStartIndex(long startIndex)
        {
            this.startIndex = startIndex;
        }
       
        public ArrayList getResults()
        {
            return results;
        }

        #endregion

        #region ToString

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            
            String indent = "";
            buf.Append(indent).Append("searchTime=").Append(searchTime).Append("\n");
            buf.Append(indent).Append("query=").Append(query).Append("\n");
            buf.Append(indent).Append("startIndex=").Append(startIndex).Append("\n");
            buf.Append(indent).Append("endIndex=").Append(endIndex).Append("\n");
            buf.Append(indent).Append("numResults=").Append(numResults).Append("\n");
            buf.Append(indent).Append("isFiltered=").Append(isFiltered).Append("\n");
            buf.Append(indent).Append("nextResponseUrl=").Append(nextResponseUrl).Append("\n");
            buf.Append(indent).Append("results=").Append(results).Append("\n");
            buf.Append(indent).Append("params=").Append(parameters).Append("\n");

            return buf.ToString();
        }

        #endregion
    }
}
