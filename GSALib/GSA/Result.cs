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
    /// Class for creating object that contains resuls of Request
    /// <para>Author Albert Ghukasyan</para>
    /// </summary>
    [Serializable]
    public sealed class Result
    {
        #region Variables

        private String mimeType;
        private int indentation;
        private String url;
        private String escapedUrl;
        private String title;
        private int rating;
        private IList<KeyValuePair<string,string>> metas;
        private Hashtable fields;
        private String summary;
        private String language;
        private String cacheDocId;
        private String cacheDocEncoding;
        private String cacheDocSize;
        private String crawelDate;

        #endregion

        #region Constructor

        internal Result()
        {
            metas = new List<KeyValuePair<string,string>>();
            fields = new Hashtable();
        }

        #endregion

        #region Get/Set Properties

        public void setIndentation(int indentation)
        {
            this.indentation = indentation;
        }

        public void setLanguage(String language)
        {
            this.language = language;
        }

        public void setCrawelDate(String crawelDate)
        {
            this.crawelDate = crawelDate;
        }

        public void setMetas(IList<KeyValuePair<string,string>> metas)
        {
            this.metas = metas;
        }

        public void addMeta(String key, String value)
        {
            this.metas.Add(new KeyValuePair<string,string>(key,value));
        }

        public void setFields(Hashtable fields)
        {
            this.fields = fields;
        }

        public void addField(String key, String value)
        {
            this.fields.Add(key, value);
        }

        public void setMimeType(String mimeType)
        {
            this.mimeType = mimeType;
        }

        public void setRating(int rating)
        {
            this.rating = rating;
        }

        public void setSummary(String summary)
        {
            this.summary = summary;
        }

        public void setTitle(String title)
        {
            this.title = title;
        }

        public void setUrl(String url)
        {
            this.url = url;
        }

        public int getIndentation()
        {
            return indentation;
        }

        public String getLanguage()
        {
            return language;
        }

        public String getCrawelDate()
        {
            return crawelDate;
        }

        public IList<KeyValuePair<string,string>> getMetas()
        {
            return metas;
        }

        public IList<string> getMeta(String name)
        {
            IList<string> metaReturn = new List<string>();
            foreach(KeyValuePair<string,string> item in metas)
            {
                if (item.Key.Equals(name))
                {
                    metaReturn.Add(item.Value);
                }
            }
            return metaReturn;
        }

        public Hashtable getFields()
        {
            return fields;
        }

        public String getField(String name)
        {
            return Util.getString(fields[name], null, false);
        }

        public String getMimeType()
        {
            return mimeType;
        }

        public int getRating()
        {
            return rating;
        }

        public String getSummary()
        {
            return summary;
        }

        public String getTitle()
        {
            return title;
        }

        public String getUrl()
        {
            return url;
        }

        public String getCacheDocEncoding()
        {
            return cacheDocEncoding;
        }

        public void setCacheDocEncoding(String cacheDocEncoding)
        {
            this.cacheDocEncoding = cacheDocEncoding;
        }

        public String getCacheDocId()
        {
            return cacheDocId;
        }

        public void setCacheDocId(String cacheDocId)
        {
            this.cacheDocId = cacheDocId;
        }

        public String getCacheDocSize()
        {
            return cacheDocSize;
        }

        public void setCacheDocSize(String cacheDocSize)
        {
            this.cacheDocSize = cacheDocSize;
        }

        public String getEscapedUrl()
        {
            return escapedUrl;
        }

        public void setEscapedUrl(String escapedUrl)
        {
            this.escapedUrl = escapedUrl;
        }

        #endregion

        #region ToString

        public override string ToString()
        {
            StringBuilder buffer = new StringBuilder();

            buffer.Append("GSAResult[");
            buffer.Append("mimeType = ").Append(mimeType);
            buffer.Append(", indentation = ").Append(indentation);
            buffer.Append(", url = ").Append(url);
            buffer.Append(", escapedUrl = ").Append(escapedUrl);
            buffer.Append(", title = ").Append(title);
            buffer.Append(", rating = ").Append(rating);
            buffer.Append(", metas = ").Append(metas);
            buffer.Append(", fields = ").Append(fields);
            buffer.Append(", summary = ").Append(summary);
            buffer.Append(", language = ").Append(language);
            buffer.Append(", cacheDocId = ").Append(cacheDocId);
            buffer.Append(", cacheDocEncoding = ").Append(cacheDocEncoding);
            buffer.Append(", cacheDocSize = ").Append(cacheDocSize);
            buffer.Append("]");

            return buffer.ToString();
        }

        #endregion
    }
}