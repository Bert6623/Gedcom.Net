<%@ WebHandler Language="C#" Class="FileSystemHandler" %>

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.IO;
using System.Collections.Generic;
using System.Xml;

public class FileSystemHandler : IHttpHandler
{
    #region IHttpHandler Members

    string pathToConfigFile = "~/App_Code/MappingFile.mapping";
    private HttpContext _context;
    private HttpContext Context
    {
        get
        {
            return _context;
        }
        set
        {
            _context = value;
        }
    }
    string _itemHandlerPath;
    Dictionary<string, string> mappedPathsInConfigFile;

    private void Initialize()
    {
        XmlDocument configFile = new XmlDocument();
        string physicalPathToConfigFile = Context.Server.MapPath(this.pathToConfigFile);
        configFile.Load(physicalPathToConfigFile);// Load the configuration file
        XmlElement rootElement = configFile.DocumentElement;

        XmlNode handlerPathSection = rootElement.GetElementsByTagName("genericHandlerPath")[0]; // get all mappings ;
        this._itemHandlerPath = handlerPathSection.InnerText;

        this.mappedPathsInConfigFile = new Dictionary<string, string>();
        XmlNode mappingsSection = rootElement.GetElementsByTagName("Mappings")[0]; // get all mappings ;
        foreach (XmlNode mapping in mappingsSection.ChildNodes)
        {
            XmlNode virtualPathAsNode = mapping.SelectSingleNode("child::VirtualPath");
            XmlNode physicalPathAsNode = mapping.SelectSingleNode("child::PhysicalPath");
            this.mappedPathsInConfigFile.Add(PathHelper.RemoveEndingSlash(virtualPathAsNode.InnerText, '/'), PathHelper.RemoveEndingSlash(physicalPathAsNode.InnerText, '\\'));
        }
    }


    public void ProcessRequest(HttpContext context)
    {
        Context = context;
        if (Context.Request.QueryString["path"] == null)
        {
            return;
        }

        Initialize();


        string virtualPathToFile = Context.Server.HtmlDecode(Context.Request.QueryString["path"]);
        string physicalPathToFile = "";
        foreach (KeyValuePair<string, string> mappedPath in mappedPathsInConfigFile)
        {
            if (virtualPathToFile.ToLower().StartsWith(mappedPath.Key.ToLower()))
            {
                // Build the physical path to the file ;
                physicalPathToFile = virtualPathToFile.Replace(mappedPath.Key, mappedPath.Value).Replace("/", "\\");

                break;// Brak the foreach loop ;
            }
        }

        // The docx files are downloaded ;
        if (Path.GetExtension(physicalPathToFile).Equals(".docx", StringComparison.CurrentCultureIgnoreCase))
        {// Handle .docx files ;

            this.WriteFile(physicalPathToFile, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", Context.Response);
        }

        if (Path.GetExtension(physicalPathToFile).Equals(".jpg", StringComparison.CurrentCultureIgnoreCase))
        {// Handle .jpg files ;
            WriteFile(physicalPathToFile, "image/jpeg", Context.Response);
        }

        // "txt/html" is the default valuse for Response.ContentType property;
        // do not download the file. Open in the window ;
        Context.Response.WriteFile(physicalPathToFile);
    }


    /// <summary>
    /// Forces browser to download the file
    /// </summary>
    /// <param name="physicalPathToFile">Physical path to the file on the server </param>
    /// <param name="contentType">The file content type</param>
    private void WriteFile(string physicalPathToFile, string contentType, HttpResponse response)
    {
        response.Buffer = true;
        response.Clear();
        string ct = response.ContentType;
        response.ContentType = contentType == "" ? response.ContentType : contentType;
        string extension = Path.GetExtension(physicalPathToFile);

        if (extension != ".htm" && extension != ".html" && extension != ".xml")
        {
            response.AddHeader("content-disposition", "attachment; filename=" + Path.GetFileName(physicalPathToFile));
        }
        response.WriteFile(physicalPathToFile);
        response.Flush();
        response.End();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
    
    #endregion
}