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

namespace GSALib.Constants
{
    /// <summary>
    /// Class provides XML tags structure supported in GSA XML Output Format
    /// <para>Author Albert Ghukasyan</para>    
    /// </summary>
    internal sealed class XMLTags
    {
        #region Varables

        public Hashtable tags;
                      
        /// <summary>
        /// This tag contains HTML data in the encoding format that is specified in the attribute.        
        /// <para>The data is BASE64 encoded to preserve the data integrity of cached results that are encoded in a different encoding scheme than the requested results .</para>
        /// Attributes { "encoding" } .
        /// </summary>
        public const int BLOB = 1;

        /// <summary>
        /// Indicates that the "cache:" special query term is supported for this search result URL.
        /// <para> Attributes { "SZ-Provides the size of the cached version of the search result in kilobytes " , "CID-Identifier of a document in the Google Search Appliance cache." } .</para>
        /// </summary>
        public const int C = 2;

        /// <summary>
        /// Encapsulates the cached version of a search result.        
        /// </summary>
        public const int CACHE = 3;

        /// <summary>
        /// MIME type of the cached result, as specified in the HTTP header that is returned when the document is crawled.       
        /// </summary>
        public const int CACHE_CONTENT_TYPE = 4;

        /// <summary>
        /// The cached version of the search result. All search results are stored in HTML format.
        /// </summary>
        public const int CACHE_HTML = 5;

        /// <summary>
        /// The encoding scheme of the cached result, as specified in the HTTP header that is returned when the document is crawled.
        /// </summary>
        public const int CACHE_ENCODING = 6;

        /// <summary>
        /// The language of the cached result as determined by Google's automatic language classification algorithm. The value of this tag is the same as the values used for the automatic language collections without the "lang_" prefix.
        /// </summary>
        public const int CACHE_LANGUAGE = 7;

        /// <summary>
        /// Date that the document was crawled, as specified in the Date HTTP header when the document was crawled for this index.
        /// </summary>
        public const int CACHE_LAST_MODIFIED = 8;

        /// <summary>
        /// Encapsulates query terms that are found in the visible text of the cached result returned.
        /// </summary>
        public const int CACHE_LEGEND_FOUND = 9;

        /// <summary>
        /// Details of any query terms that are not visible in the cached result returned.
        /// </summary>
        public const int CACHE_LEGEND_NOTFOUND = 10;

        /// <summary>
        /// Details of a query term that is visible in the cached result. Query terms found in the cached result are automatically highlighted using the colors described in the attributes of this tag.
        /// <para> Attributes { "fgcolor-The foreground color of the query term in the cached result." , "bgcolor-The background color of the query term in the cached result." } .</para>
        /// </summary>
        public const int CACHE_LEGEND_TEXT = 11;

        /// <summary>
        /// Final URL of cached result after all redirects are resolved.
        /// </summary>
        public const int CACHE_REDIR_URL = 12;

        /// <summary>
        /// Initial URL of cached result.
        /// </summary>
        public const int CACHE_URL = 13;

        /// <summary>
        /// An optional element that shows the date when the page was crawled. It is shown only for pages that have been crawled within the past two days.
        /// </summary>
        public const int CRAWLDATE = 14;

        /// <summary>
        /// Search comments.
        /// <para> Example comment: Sorry, no content found for this URL </para> 
        /// </summary>
        public const int CT = 15;

        /// <summary>
        /// Encapsulates custom XML tags that are specified in the "proxycustom" input parameter.
        /// </summary>
        public const int CUSTOM = 16;

        /// <summary>
        /// Encapsulates the results returned by OneBox modules. (Applies to version 4.6 and newer.)
        /// </summary>
        public const int ENTOBRESULTS = 17;

        /// <summary>
        /// Indicates that document filtering was performed during this search.
        /// </summary>
        public const int FI = 18;        

        /// <summary>
        /// Additional details about the search result.
        /// <para> Attributes { "NAME-Name of the result descriptor." , "VALUE-Value of the result descriptor." } .</para>
        /// </summary>
        public const int FS = 19;

        /// <summary>
        /// Contains the description of a KeyMatch result.
        /// </summary>
        public const int GD = 20;

        /// <summary>
        /// Contains the URL of a KeyMatch result.
        /// </summary>
        public const int GL = 21;

        /// <summary>
        /// Encapsulates a single KeyMatch result.
        /// </summary>
        public const int GM = 22;

        /// <summary>
        /// GSP = "Google Search Protocol" Encapsulates all data that is returned in the Google XML search results.
        /// <para> Attributes { "VER-Indicates version of the search results output. The current output version is "3.2" ." } .</para>
        /// </summary>
        public const int GSP = 23;

        /// <summary>
        /// Encapsulates special features that are included for this search result.
        /// </summary>
        public const int HAS = 24;

        /// <summary>
        /// Indicates that filtering has occurred and that additional results are available from the directory where this search result was found. The value of this tag is ready to be used with the site:" special query term.
        /// <para> Attributes { "U-Server and path components of the directory's URL." } .</para>
        /// </summary>
        public const int HN = 25;

        /// <summary>
        /// Indicates that the "link:" special query term is supported for this search result URL.
        /// </summary>
        public const int L = 26;

        /// <summary>
        /// Indicates the language of the search result. The LANG element contains a two-letter language code.
        /// </summary>
        public const int LANG = 27;

        /// <summary>
        /// The estimated total number of results for the search.The estimate of the total number of results for a search can be too high or too low.
        /// </summary>
        public const int M = 28;

        /// <summary>
        /// Meta tag name and value pairs obtained from the search result.Only meta tags that are requested in the search request are returned.
        /// <para> Attributes { "N-Name of the meta tag." ,  "V-Value of the meta tag." } .</para>
        /// </summary>
        public const int MT = 29;

        /// <summary>
        /// Encapsulates the navigation information for the result set. The NB tag is present only if either the previous or additional results are available.
        /// </summary>
        public const int NB = 30;

        /// <summary>
        /// Contains a relative URL pointing to the next results page.The NU tag is present only when more results are available. 
        /// </summary>
        public const int NU = 31;

        /// <summary>
        /// Encapsulates a result returned by a OneBox module.
        /// </summary>
        public const int OBRES = 32;

        /// <summary>
        /// A synonym suggestion for the submitted query, in HTML format.
        /// <para> Attributes { "q-The URL-encoded version of the synonym suggestion." } .</para>
        /// </summary>
        public const int OneSynonym = 33;

        /// <summary>
        /// A synonym suggestion for the submitted query, in HTML format.
        /// <para> Attributes { "name-Name of the input parameter." , "value-HTML formatted version of the input parameter value" , "original_value-Original URL encoded version of the input parameter value" } .</para>        
        /// </summary>
        public const int PARAM = 34;

        /// <summary>
        /// Contains relative URL to the previous results page.The PU tag is present only if previous results are available.        
        /// </summary>
        public const int PU = 35;

        /// <summary>
        /// The search query terms submitted to the Google search engine to generate these results.
        /// </summary>
        public const int Q = 36;

        /// <summary>
        /// Encapsulates the details of an individual search result.
        /// <para> Attributes { "N-The index number (1-based) of this search result." , "L-The recommended indentation level of the results." , "MIME-The MIME type of the search result." } .</para>
        /// </summary>
        public const int R = 37;

        /// <summary>
        /// Encapsulates the set of all search results.
        /// <para> Attributes { "SN-The index (1-based) of the first search result returned in this result set." , "EN-Indicates the index (1-based) of the last search result returned in this result set." } .</para>
        /// </summary>
        public const int RES = 38;

        /// <summary>
        /// Provides a general rating of the relevance of the search result.
        /// </summary>
        public const int RK = 39;

        /// <summary>
        /// The snippet for the search result.
        /// </summary>
        public const int S = 40;

        /// <summary>
        /// Encapsulates alternate spelling suggestions for the submitted query. Only one spelling suggestion is returned at this time.
        /// </summary>
        public const int Spelling = 41;

        /// <summary>
        /// An alternate spelling suggestion for the submitted query, in HTML format.
        /// <para> Attributes { "q-The URL-encoded version of the spelling suggestion." ,"qe -Internal-only attribute for the URL-encoded version of the spelling suggestion." } .</para>
        /// </summary>
        public const int Suggestion = 42;

        /// <summary>
        /// Encapsulates the synonym suggestions for the submitted query. Up to 20 synonym suggestions may be returned, depending on the synonym list that is associated with the front end.        
        /// </summary>
        public const int Synonyms = 43;

        /// <summary>
        /// The title of the search result.
        /// </summary>
        public const int T = 44;

        /// <summary>
        /// Total server time to return search results, measured in seconds.
        /// </summary>
        public const int TM = 45;

        /// <summary>
        /// The URL of the search result.
        /// </summary>
        public const int U = 46;

        /// <summary>
        /// The URL string to display when the URL that is in the U parameter is non-ASCII. Displays UTF-8 characters and IDNA domain names properly.
        /// </summary>
        public const int UD = 47;

        /// <summary>
        /// The URL encoded version of the URL that is in the U parameter.
        /// </summary>
        public const int UE = 48;

        /// <summary>
        /// Indicates that the estimated total number of results specified in this search result is exact.
        /// </summary>
        public const int XT = 49;

         /// <summary>
        /// The name of the tag that holds a single field of a One Box
        /// </summary>
        public const int FIELD = 50;

        /// <summary>
        /// The name of the tag that holds a One Box
        /// </summary>
        public const int MODULE_RESULT = 51;

        /// <summary>
        /// The name of the tag that holds if parametric is estimated
        /// </summary>
        public const int PC = 52;

        /// <summary>
        /// The name of the tag that holds the parametric name
        /// </summary>
        public const int PMT = 53;

        /// <summary>
        /// The name of the tag that holds the parametric attribute
        /// </summary>
        public const int PV = 54;

        /// <summary>
        /// The name of the tag that holds the parametric attribute
        /// </summary>
        public const int PARM = 55;


        #endregion

        #region Contructor

        internal XMLTags()
        {
            tags = new Hashtable();

            tags.Add("BLOB", BLOB);
            tags.Add("C", C);
            tags.Add("CACHE", CACHE);
            tags.Add("CACHE_CONTENT_TYPE", CACHE_CONTENT_TYPE);
            tags.Add("CACHE_HTML", CACHE_HTML);
            tags.Add("CACHE_ENCODING", CACHE_ENCODING);
            tags.Add("CACHE_LANGUAGE", CACHE_LANGUAGE);
            tags.Add("CACHE_LAST_MODIFIED", CACHE_LAST_MODIFIED);
            tags.Add("CACHE_LEGEND_FOUND", CACHE_LEGEND_FOUND);
            tags.Add("CACHE_LEGEND_NOTFOUND", CACHE_LEGEND_NOTFOUND);
            tags.Add("CACHE_LEGEND_TEXT", CACHE_LEGEND_TEXT);
            tags.Add("CACHE_REDIR_URL", CACHE_REDIR_URL);
            tags.Add("CACHE_URL", CACHE_URL);
            tags.Add("CRAWLDATE", CRAWLDATE);
            tags.Add("CT", CT);
            tags.Add("CUSTOM", CUSTOM);
            tags.Add("ENTOBRESULTS", ENTOBRESULTS);
            tags.Add("Field", FIELD);
            tags.Add("MODULE_RESULT", MODULE_RESULT);
            tags.Add("FI", FI);
            tags.Add("FS", FS);
            tags.Add("GD", GD);
            tags.Add("GL", GL);
            tags.Add("GM", GM);
            tags.Add("GSP", GSP);
            tags.Add("HAS", HAS);
            tags.Add("HN", HN);
            tags.Add("L", L);
            tags.Add("LANG", LANG);
            tags.Add("M", M);
            tags.Add("MT", MT);
            tags.Add("NB", NB);
            tags.Add("NU", NU);
            tags.Add("OBRES", OBRES);
            tags.Add("OneSynonym", OneSynonym);
            tags.Add("PARAM", PARAM);
            tags.Add("PARM", PARM);
            tags.Add("PC", PC);
            tags.Add("PMT", PMT);
            tags.Add("PU", PU);
            tags.Add("PV", PV);
            tags.Add("Q", Q);
            tags.Add("R", R);
            tags.Add("RES", RES);
            tags.Add("RK", RK);
            tags.Add("S", S);
            tags.Add("Spelling", Spelling);
            tags.Add("Suggestion", Suggestion);
            tags.Add("Synonyms", Synonyms);
            tags.Add("T", T);
            tags.Add("TM", TM);
            tags.Add("U", U);
            tags.Add("UD", UD);
            tags.Add("UE", UE);
            tags.Add("XT", XT);
        }

        #endregion
        
        #region Methods

        public int getTag(String tagName)
        {
            return Convert.ToInt32( tags[tagName] ?? 0 );
        }

        #endregion

    }
}
