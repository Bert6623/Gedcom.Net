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
    /// Provides metods to collect OneBox results returned from GSA Server
    /// <para>Author Albert Ghukasyan</para>
    /// </summary>
    [Serializable]
    public sealed class OneBoxResponse
    {
        #region Variables

        private String titleText;
        private String titleLink;
        private String imageSource;
        private String providerName;
        private ArrayList moduleResults;
        private String moduleName;

        #endregion

        #region Constructor

        public OneBoxResponse()
        {
            this.moduleResults = new ArrayList();
        }

        #endregion

        #region Get/Set Properties

        public String getModuleName()
        {
            return moduleName;
        }

        public String getImageSource()
        {
            return imageSource;
        }

        public ArrayList getModuleResults()
        {
            return moduleResults;
        }

        public String getProviderName()
        {
            return providerName;
        }

        public String getTitleLink()
        {
            return titleLink;
        }

        public String getTitleText()
        {
            return titleText;
        }

        public void setImageSource(String imageSource)
        {
            this.imageSource = imageSource;
        }

        public void setModuleName(String moduleName)
        {
            this.moduleName = moduleName;
        }

        public void setModuleResults(ArrayList moduleResults)
        {
            this.moduleResults = moduleResults;
        }

        public void addResult(OneBoxResult oneboxResult)
        {
            this.moduleResults.Add(oneboxResult);
        }

        public void setProviderName(String providerName)
        {
            this.providerName = providerName;
        }

        public void setTitleLink(String titleLink)
        {
            this.titleLink = titleLink;
        }

        public void setTitleText(String titleText)
        {
            this.titleText = titleText;
        }

        #endregion
    }
}
