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

namespace GSALib.Exceptions
{
    /// <summary>
    /// "Invalid File" Exception class ,throws when File to where need to save XML data not correctly specified
    /// <para>Author Albert Ghukasyan</para>    
    /// </summary>    
    public sealed class GSAInvalidFileException: Exception
    {
        #region Variables

        private string _message;
        private string _StackTrace;

        #endregion

        #region Constructor

        internal GSAInvalidFileException(string Message, string StackTrace)
        {
            this._message = Message;
            this._StackTrace = StackTrace;
        }

        #endregion

        #region System.Exception class override Methods

        public override string Message
        {
            get
            {
                return this._message;
            }
        }

        public override string StackTrace
        {
            get
            {
                return this._StackTrace;
            }
        }

        #endregion
        
    }
}
