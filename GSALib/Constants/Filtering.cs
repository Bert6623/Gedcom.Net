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
    /// Class provides Filtering options in search results supported by GSA
    /// <para>Author Albert Ghukasyan</para>    
    /// </summary>
    public sealed class Filtering
    {
        #region Variables

        /// <summary>
        /// No Filter
        /// </summary>
        public static Filtering NO_FILTER = new Filtering('0');

        /// <summary>
        /// "Duplicate directory" and "Duplicate snippet" filtering 
        /// </summary>
        public static Filtering FULL_FILTER = new Filtering('1');

        /// <summary>
        /// "Duplicate snippet" filtering 
        /// </summary>
        public static Filtering DUPLICATE_SNIPPET_FILTER = new Filtering('s');

        /// <summary>
        /// "Duplicate directory" filtering 
        /// </summary>      
        public static Filtering DUPLICATE_DIRECTORY_FILTER = new Filtering('p');

        private char value;

        #endregion

        #region Constructor

        private Filtering(char value)
        {
            this.value = value;
        }

        #endregion

        #region Methods

        public int hashCode()
        {
            return value;
        }

        public bool equals(Object o)
        {
            bool retval = false;
            if (o != null && o is Filtering)
            {
                Filtering other = (Filtering)o;
                retval = other.value == this.value;
            }
            return retval;
        }

        public char getValue()
        {
            return value;
        }

        #endregion
    }
}
