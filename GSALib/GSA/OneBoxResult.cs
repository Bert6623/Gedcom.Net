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

namespace GSALib.GSA
{
    /// <summary>
    /// Provides metods to create OneBoxResult  objects
    /// <para>Author Albert Ghukasyan</para>
    /// </summary>
    [Serializable]
    public sealed class OneBoxResult
    {
        #region Varaibles

        private String url;
        private ArrayList fieldEntries;

        #endregion

        #region Contructor

        public OneBoxResult()
        {
            this.fieldEntries = new ArrayList();
        }

        #endregion

        #region Get/Set Properties

        public String getUrl()
        {
            return url;
        }

        public void setUrl(String url)
        {
            this.url = url;
        }

        public Entry[] getFieldEntries()
        {
            Entry[] fields = new Entry[fieldEntries.Count];
            for (int i = 0; i < fieldEntries.Count; i++)
            {
                fields[i] = (Entry)fieldEntries[i];
            }
            return fields;
        }

        internal void addFieldEntry(String key, String value)
        {
            this.fieldEntries.Add(new Entry(key, value));
        }

        #endregion
    }

    [Serializable]
    public sealed class Entry
    {
        #region Varaibles

        private Object key;
        private Object value;

        #endregion

        #region Contructor

        public Entry(Object key, Object value)
        {
            this.key = key;
            this.value = value;
        }

        #endregion

        #region Get/Set Properties

        public Object getKey()
        {
            return key;
        }

        public Object getValue()
        {
            return value;
        }

        public Object setValue(Object value)
        {
            Object retval = value;
            this.value = value;
            return retval;
        }

        #endregion
    }

}
