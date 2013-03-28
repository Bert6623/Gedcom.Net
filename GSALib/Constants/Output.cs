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
    /// Class provides Methods/Members to set Output format supported by GSA
    /// <para>Author Albert Ghukasyan</para>    
    /// </summary>
    public sealed class Output
    {
        #region Variables

        /// <summary>
        /// "XML" Format Output  (with DTD)
        /// </summary>
        public static Output XML = new Output("xml");

        /// <summary>
        /// "HTML Custom Output" Format or XML Output format (without DTD)
        /// </summary>
        public static Output XML_NO_DTD = new Output("xml_no_dtd");

        private String value;

        #endregion

        #region Constructor

        private Output(String value)
        {   
            this.value = (value == null) ? "" : value;
        }

        #endregion

        #region Methods

        public int hashCode()
        {
            return (value == null) ? 0 : value.GetHashCode();
        }

        public bool equals(Object o)
        {
            bool retval = false;
            if (o != null && o is Output)
            {
                Output other = (Output)o;
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
