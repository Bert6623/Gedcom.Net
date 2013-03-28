/********************************************************************************
 *  
 *  Product: GSALib
 *  Description: A C# API for accessing the Google Search Appliance.
 *
 *  (c) Copyright 2011 Michael Cizmar + Associates Ltd.  (MC+A)
 *  
********************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace GSALib.GSA
{
    /// <summary>
    /// Provides metods to collect Dymnamic Navigation results returned from GSA Server
    /// <para>Author Michael Cizmar</para>
    /// </summary>
    [Serializable]
    public sealed class ParametricNavigationResponse
    {
        #region Variables

        private String name;
        private String displayName;
        private bool isRange;
        private String type;
        private ArrayList parameticResults;
        
        #endregion

        #region Constructor

        public ParametricNavigationResponse()
        {
            this.parameticResults = new ArrayList();
        }

        #endregion

        #region Get/Set Properties

        public String getName()
        {
            return name;
        }

        public String getDisplayName()
        {
            return displayName;
        }

        public String getType()
        {
            return type;
        }

        public bool getIsRange()
        {
            return isRange;
        }

        public ArrayList getParametericResults()
        {
            return parameticResults;
        }

        
        public void setName(String name)
        {
            this.name = name;
        }

        public void setDisplayName(String displayName)
        {
            this.displayName = displayName;
        }

        public void setIsRange(bool isRange)
        {
            this.isRange = isRange;
        }

        public void setType(String type)
        {
            this.type = type;
        }

        public void setParametricResults(ArrayList parameticResults)
        {
            this.parameticResults = parameticResults;
        }

        public void addResult(ParametricResult parametricResult)
        {
            this.parameticResults.Add(parametricResult);
        }

        

        #endregion
    }
}
