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
    /// Class provides access for searching in public, secure, or both contents supported by GSA
    /// <para>Author Albert Ghukasyan</para>    
    /// </summary>
    public sealed class Access
    {
        #region Variables

        /// <summary>
        /// returns PUBLIC results
        /// </summary>
        public static Access PUBLIC = new Access('p');

        /// <summary>
        /// returns SECURE results
        /// </summary>
        public static Access SECURE = new Access('s');
               
        /// <summary>
        /// returns ALL results
        /// </summary>
        public static Access ALL = new Access('a');

        private char value;

        #endregion

        #region Constructor

        private Access(char value)
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
            if (o != null && o is Access)
            {
                Access other = (Access)o;
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
