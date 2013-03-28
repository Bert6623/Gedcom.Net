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
using System.Configuration;
using System.Text;
using System.IO;
using System.Net;

namespace GSALib.GSA
{
    /// <summary>
    /// Class provides Methods to access to GSA by specified host
    /// <para>Author Albert Ghukasyan</para>    
    /// </summary>
    public sealed class ClientAccess
    {
        #region Variables

        private String protocol;
        private String host;
        private String path;
        private int port;
        public String GSAHostAddress = "";

        public const int DEFAULT_PORT = 80;
        public const String DEFAULT_PATH = "/search";
        public const String DEFAULT_PROTOCOL = "http";
        private bool NeedCredentails = false;
        public NetworkCredential credentails;

        #endregion

        #region Contructors

        /// <summary>
        /// Constructor wich Creates ClientAccess Object with Specified Host,Port,Protocol and path
        /// </summary>
        /// <param name="protocol">Protocol (Default is http:// )</param>
        /// <param name="host">Host of GSA Server</param>
        /// <param name="port">Port of GSA Server(Default is 80)</param>        
        /// <param name="path">Path(Default is "/Search" )</param>
        public ClientAccess(String protocol, String host, int port, String path)
        {
            InitializeHost(protocol, host, port, path);
        }

        /// <summary>
        /// Constructor wich Creates ClientAccess Object with DEFAULT_PROTOCOL
        /// </summary>
        /// <param name="host">Host of GSA Server</param>
        /// <param name="port">Port of GSA Server(Default is 80)</param>        
        /// <param name="path">Path(Default is "/Search" )</param>
        public ClientAccess(String host, int port, String path)
        {
            InitializeHost(DEFAULT_PROTOCOL, host, port, path);
        }

        /// <summary>
        /// Constructor wich Creates ClientAccess Object with DEFAULT_PROTOCOL and PORT
        /// </summary>
        /// <param name="host">Host of GSA Server</param>
        /// <param name="path">Path(Default is "/Search" )</param>
        public ClientAccess(String host, String path)
        {
            InitializeHost(DEFAULT_PROTOCOL, host, DEFAULT_PORT, path);
        }

        /// <summary>
        /// Constructor wich Creates ClientAccess Object with DEFAULT_PROTOCOL,PORT and Search Path
        /// </summary>
        /// <param name="host">Host of GSA Server</param>
        public ClientAccess(String host)
        {
            InitializeHost(DEFAULT_PROTOCOL, host, DEFAULT_PORT, DEFAULT_PATH);
        }
        
        /// <summary>
        /// Default Contructor
        /// </summary>
        public ClientAccess()
        {
            InitializeHost(DEFAULT_PROTOCOL, null, DEFAULT_PORT, DEFAULT_PATH);
        }

        /// <summary>
        /// Initializing of Members
        /// </summary>       
        private void InitializeHost(String protocol, String host, int port, String path)
        {
            if (host != null)
            {
                this.protocol = protocol;
                this.host = host;
                this.port = port;
                this.path = path;
            }

            try { this.GSAHostAddress = this.GetConfig(); }
            catch (GSALib.Exceptions.GSAHostNotFoundInAppSettingsException ex) { throw ex; }
            catch (FileNotFoundException ex) { throw ex; }

        }

        #endregion

        #region Read host data from .cofig file

        /// <summary>
        /// Reads GSAHostAddress from the Application/Web config file
        /// </summary>
        /// <returns>GSAHostAddress specified in Configuration file</returns>
        private string GetConfig()
        {
            string Result = "";
            
            try
            {
                Result = ConfigurationManager.AppSettings["GSAHostAddress"];

                if (Result.Length <= 0)
                    throw new GSALib.Exceptions.GSAHostNotFoundInAppSettingsException("GSAHostAddress key not found in appSettings section", "");
            }
            catch (Exception ex) { throw ex; }

            return Result;
        }

        #endregion

        #region Get Properties
       
        public String getHost()
        {
            return host;
        }

        public String getPath()
        {
            return path;
        }

        public int getPort()
        {
            return port;
        }

        public String getProtocol()
        {
            return protocol;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Default Search
        /// </summary>
        /// <param name="query">Query class object</param>
        /// <returns></returns>
        public StreamReader search(Query query)
        {
            if (query.getAccess() != 'p')
                this.NeedCredentails = true;

            return search(query.getValue());
        }

        /// <summary>
        /// Getting full URL
        /// </summary>
        /// <param name="rawQuery">Query to append</param>
        /// <returns></returns>
        public string GetFullPath(string rawQuery)
        {
            String fullUrl = rawQuery;
            if (rawQuery.IndexOf("://") < 0)
            {
                // build full url
                fullUrl = this.host == null ?
                    GSAHostAddress + DEFAULT_PATH + (rawQuery.StartsWith("?") ? rawQuery : ("?" + rawQuery))
                    :
                    protocol + "://" + host + ':' + port
                        + (path.StartsWith("/") ? path : ("/" + path))
                        + (rawQuery.StartsWith("?") ? rawQuery : ("?" + rawQuery));
            }

            return fullUrl;
        }

        /// <summary>
        /// Search in WEB by specified Query
        /// </summary>        
        /// <returns> Stream wich contains response Steram of results from GSA</returns>
        private StreamReader search(String rawQuery)
        {
            StreamReader retval = null;
            
            if (rawQuery != null)
            {
                WebRequest rew = WebRequest.Create(GetFullPath(rawQuery));
                if (this.NeedCredentails && credentails != null)
                {
                    rew.Credentials = credentails;
                }
                else if (this.NeedCredentails)
                {
                    throw new Exceptions.GSANeedNetworkCredentailsException("Please specify NTLM credentails", "");
                }

                retval = new StreamReader(rew.GetResponse().GetResponseStream(), true);
            }
            return retval;
        }
        
        /// <summary>
        /// Sending Response to gsa host and data retrieving
        /// </summary>
        /// <param name="query"></param>
        /// <param name="path">Set to null if you dont want to save in file</param>
        /// <returns></returns>
        public Response getGSAResponse(Query query,String path)
        {
            StreamReader readStream = search(query);

            ResponseBuilder rsp = new ResponseBuilder();//(readStream, GSAHostAddress);

            return rsp.buildResponse(readStream, GSAHostAddress, path);// ResponseBuilder.buildResponse(search(rawQuery), xmlSystemId);
        }

        #endregion
    }
}
