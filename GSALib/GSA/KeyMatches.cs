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

namespace GSALib.GSA
{
    /// <summary>
    /// KeyMatches Class returned from GSA Server
    /// <para>Author Albert Ghukasyan</para>
    /// </summary>
    [Serializable]
    public sealed class KeyMatches
    {
        #region Variables

        private string URL = "";
        private string Description = "";

        #endregion

        #region Constructor

        internal KeyMatches()
        {

        }

        #endregion

        #region Methods

        public void setURL(string _URL)
        {
            this.URL = _URL;
        }

        public void setDescription(string _Description)
        {
            this.Description = _Description;
        }

        public string getURL()
        {
            return this.URL;
        }

        public string getDescription()
        {
            return this.Description;
        }

        #endregion
    }
}
