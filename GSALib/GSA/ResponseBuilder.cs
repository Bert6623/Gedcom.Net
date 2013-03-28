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
using System.IO;
using System.Xml;
using System.Collections;

namespace GSALib.GSA
{
    /// <summary>
    /// Class for building response supported in GSA
    /// <para>Author Albert Ghukasyan</para>
    /// </summary>
    internal sealed class ResponseBuilder
    {
        #region Constructor

        private Utils.XMLParser xmlParser = new GSALib.Utils.XMLParser();
        internal ResponseBuilder()
        {
        }

        #endregion

        #region buildResponse Methode

        /// <summary>
        /// Builds Response and saveing to file
        /// </summary>
        /// <param name="istream">Web Request returned Stream</param>
        /// <param name="GSAHostAddress">Not Used Yet</param>
        /// <param name="path">Path to file to where need to save data</param>
        /// <returns></returns>
        public Response buildResponse(StreamReader istream, String GSAHostAddress, String path)
        {
            xmlParser.Parse(istream);
            if (path != null)
            {
                try
                {
                    System.IO.StreamWriter sw = new StreamWriter(path);
                    sw.AutoFlush = true;
                    sw.Write(xmlParser.sb.ToString());
                    sw.Close(); 
                }
                catch(Exception ex)
                {
                    throw new Exceptions.GSAInvalidFileException("Invalide File Specified",ex.StackTrace);
                }
            }
            return xmlParser.getGSAResponse();
        }

        #endregion
    }

}
