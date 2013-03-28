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
using System.Text;

namespace GSALib.Constants
{
    /// <summary>
    /// Class provides Methods/Members to set Search Region
    /// <para>Author Albert Ghukasyan</para>    
    /// </summary>
    public sealed class SearchRegion
    {
        #region Variables

        /// <summary>
        /// Search Everywhere
        /// </summary>     
        public static SearchRegion ANY = new SearchRegion("any");

        /// <summary>
        /// Search in Titles
        /// </summary>
        public static SearchRegion TITLE = new SearchRegion("title");

        /// <summary>
        /// Search in URL
        /// </summary>
        public static SearchRegion URL = new SearchRegion("url");

        private String value;

        #endregion

        #region Constructor

        private SearchRegion(String value)
        {
            this.value = (value == null) ? "" : value;
        }

         #endregion

        #region Methods

        public int hashCode()
        {
            return value == null ? 0 : value.GetHashCode();
        }

        public bool equals(Object o)
        {
            bool retval = false;
            if (o != null && o is SearchRegion)
            {
                SearchRegion other = (SearchRegion)o;
                retval = other.value.Equals(this.value);
            }
            return retval;
        }

        public String getValue()
        {
            return value;
        }

         #endregion
    }
}
