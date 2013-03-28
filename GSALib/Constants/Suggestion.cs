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

namespace GSALib.Constants
{
    /// <summary>
    /// Class provides Methods/Members to set Suggestion
    /// <para>Author Albert Ghukasyan</para>    
    /// </summary>
    [Serializable]
    public sealed class Suggestion
    {
        #region Variables

        /// <summary>
        /// Text of the suggestion
        /// </summary>
        private String text;

        /// <summary>
        /// Markup Text of the suggestion
        /// </summary>
        private String textMarkup;

        #endregion

        #region Methods

        public void setText(String text)
        {
            this.text = text;
        }

        public String getText()
        {
            return text;
        }

        public void setTextMarkup(String textMarkup)
        {
            this.textMarkup = textMarkup;
        }

        public String getTextMarkup()
        {
            return textMarkup;
        }

        #endregion
    }

    /// <summary>
    /// Class provides Methods/Members to set Spelling
    /// <para>Author Albert Ghukasyan</para>    
    /// </summary>
    [Serializable]
    public class Spelling
    {
        #region Variables

        public ArrayList suggestions = new ArrayList();

        #endregion

        #region Variables

        public void addSuggestion(Suggestion suggestion)
        {
            suggestions.Add(suggestion);
        }

        public ArrayList getSuggestions()
        {
            return suggestions;
        }

        #endregion
    }
}
