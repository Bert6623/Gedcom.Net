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

namespace GSALib.Utils
{
    /// <summary>
    /// Class provides Methods to set Filter in GAS Query
    /// <para>Author Albert Ghukasyan</para>    
    /// </summary>
    public sealed class QueryFilter
    {
        #region Variables

        private ArrayList paramSet = new ArrayList();

        #endregion

        #region Contructor

        public QueryFilter(String[] retainedParams)
        {
            for (int i = 0, iSize = retainedParams.Length; i < iSize; i++)
            {
                paramSet.Add(retainedParams[i]);
            }
        }

        #endregion
       
        #region Methods

        public String filter(String queryString)
        {
            StringBuilder sbuf = new StringBuilder();
            String[] parameters = queryString.Split('&');
            bool firstTime = true;
            for (int i = 0, iSize = parameters.Length; i < iSize; i++, firstTime = false)
            {
                if (null != parameters[i])
                {
                    String[] keyValue = parameters[i].Split('=');
                    if (paramSet.Contains(keyValue[0]))
                    {
                        if (!firstTime)
                        {
                            sbuf.Append('&');
                        }
                        sbuf.Append(keyValue[0]).Append('=').Append(keyValue[1]);
                    }
                }
            }

            return sbuf.ToString();
        }

        #endregion
    }
}