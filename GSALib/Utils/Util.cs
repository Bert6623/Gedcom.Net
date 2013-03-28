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
using System.Globalization;
using System.Web;

namespace GSALib.Utils
{
    /// <summary>
    /// Class provides Util Methods to get/set and appending new parameters into GSA Query
    /// <para>Author Albert Ghukasyan</para>    
    /// </summary>
    internal static class Util
    {
        #region Methods

        public static int getStart(HttpRequest request,string startName)
        {
            int startNum = 0;
            if (request[startName] != null)
            {
                startNum = Int32.Parse(request[startName]);
            }
            return startNum;
        }

        /// <summary>
        /// Returns obj.ToString() if obj not null or Default if is null
        /// </summary>
        /// <param name="obj">Object From wich need to get String</param>
        /// <param name="Default">Returns when Obj is null</param>        
        /// <returns></returns>
        public static String getString(Object obj, String Default, bool nullstrict)
        {
            String retval = Default;
            if (obj != null && (!nullstrict || !"".Equals(obj.ToString())))
            {
                retval = obj.ToString();
            }
            return retval;
        }

        /// <summary>
        /// Allows String Separation
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="prefix"></param>
        /// <param name="delim"></param>
        /// <returns></returns>
        public static String SeparatedString(ArrayList tokens, String prefix, String delim)
        {
            StringBuilder sbuf = new StringBuilder();

            if (tokens != null)
            {
                for (int i = 0, iSize = tokens.Count; i < iSize; i++)
                {
                    sbuf.Append(i <= 0 || i >= iSize ? "" : delim);
                    sbuf.Append(prefix == null ? "" : prefix).Append(tokens[i].ToString());
                }
            }
            return sbuf.ToString();
        }

        /// <summary>
        /// Allows String Separation
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="prefix"></param>
        /// <param name="delim"></param>
        /// <returns></returns>
        public static String SeparatedString(String[] tokens, String prefix, String delim)
        {
            StringBuilder sbuf = new StringBuilder();

            if (tokens != null)
            {
                for (int i = 0, iSize = tokens.Length; i < iSize; i++)
                {
                    sbuf.Append(i <= 0 || i >= iSize ? "" : delim);
                    sbuf.Append(prefix == null ? "" : prefix).Append(tokens[i]);
                }
            }
            return sbuf.ToString();
        }

        /// <summary>
        /// Appends New Parameter to Query
        /// </summary>
        /// <param name="sbuf"></param>
        /// <param name="param"></param>
        /// <param name="value"></param>
        public static void appendQueryParam(StringBuilder sbuf, String param, String value)
        {
            if (sbuf.Length > 0 && "&" != sbuf.ToString().Substring(sbuf.Length - 1, 1))
            {
                sbuf.Append('&');
            }
            sbuf.Append(param).Append('=').Append(encode(value));
        }

        /// <summary>
        /// Appends New Parameter to Query
        /// </summary>
        /// <param name="sbuf"></param>
        /// <param name="param"></param>
        /// <param name="values"></param>
        public static void appendQueryParam(StringBuilder sbuf, String param, String[] values)
        {
            if (param != null && values != null && values.Length > 0)
            {
                for (int i = 0, iSize = values.Length; i < iSize; i++)
                {
                    appendQueryParam(sbuf, param, values[i]);
                }
            }
        }

        /// <summary>
        /// Appends New Maped Parameter to Query
        /// </summary>
        /// <param name="sbuf"></param>
        /// <param name="param"></param>
        /// <param name="fieldsMap"></param>
        /// <param name="delimiter"></param>
        public static void appendMappedQueryParams(StringBuilder sbuf, String param, Hashtable fieldsMap, String delimiter)
        {
            if (sbuf.Length > 0 && "&" != sbuf.ToString().Substring(sbuf.Length - 1, 1))
            {
                sbuf.Append("&");
            }
            sbuf.Append(param).Append("=");

            bool firstTime = true;
            foreach (object _key in fieldsMap.Keys)
            {
                String key = _key.ToString();
                String value = fieldsMap[_key].ToString();
                if (!firstTime)
                {
                    sbuf.Append(encode(delimiter));
                }

                String encvalue = encode(key + (value == null ? "" : ":" + value));
                sbuf.Append(encvalue);
                firstTime = false;
            }
        }

        /// <summary>
        /// Encodes string to URL string
        /// </summary>
        /// <param name="value">Value to encode</param>
        /// <returns></returns>
        public static String encode(String value)
        {
            String retval = "";
            try
            {
                if (value != null)
                {
                    retval = System.Web.HttpUtility.UrlEncode(value);
                }
            }
            catch (Exception ex)
            {
                throw new GSALib.Exceptions.GSAInvalidURLException("Invalid Url specified in parameter 'value' ", ex.StackTrace);
            }
            return retval;
        }

        #endregion
    }
}
