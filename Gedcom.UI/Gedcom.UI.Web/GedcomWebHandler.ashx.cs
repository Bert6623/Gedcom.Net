/*
 *  $Id: GedcomWebHandler.ashx.cs 183 2008-06-08 15:31:15Z davek $
 * 
 *  Copyright (C) 2007 David A Knight <david@ritter.demon.co.uk>
 *
 *  This program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program; if not, write to the Free Software
 *  Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA
 *
 */

using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;

namespace Gedcom.UI.Web
{
	
	
	public partial class GedcomWebHandler : IHttpHandler
	{
		public virtual bool IsReusable
		{
			get { return true; }	
		}
		
		public virtual void ProcessRequest(HttpContext context)
		{
			HttpRequest request = context.Request;
			HttpResponse response = context.Response;
			HttpApplicationState application = context.Application;
			
			string xslFile = string.Empty;
			
			XsltArgumentList args = new XsltArgumentList();
			
			if (request.QueryString.Count == 0)
			{
				// summary + list page
				
				xslFile = "Summary.xsl";
			}
			else if (request.QueryString["listsurnames"] != null)
			{
				// list all surnames
				
				xslFile = "Surnames.xsl";
			}
			else if (request.QueryString["surname"] != null)
			{
				string surname = HttpUtility.UrlDecode(request.QueryString["surname"]);
				
				args.AddParam("Surname", string.Empty, surname);
				
				if (request.QueryString["restOfName"] == null)
				{
					// list of individuals with surname
					
					xslFile = "Individuals.xsl";
				}
				else
				{
					// list of individuals with requested name
					
					string restOfName = HttpUtility.UrlDecode(request.QueryString["restOfName"]);
					
					args.AddParam("GivenName", string.Empty, restOfName);
					
					xslFile = "Individual.xsl";
				}
			}
			else
			{
				response.StatusCode = 404;
				response.StatusDescription = "Unknown mapping";
			}
			
			if (response.StatusCode != 404)
			{
				xslFile = Path.Combine((string)application["XSLDirectory"], xslFile);
				
				if (File.Exists(xslFile))
				{			
					XPathDocument doc = (XPathDocument)application["XMLDoc"];
					
					XslCompiledTransform transform = new XslCompiledTransform();
					transform.Load(xslFile);
					
					response.ContentType = "text/html";
					response.Charset = "utf-8";
					
					System.Console.WriteLine("Transform begin: " + DateTime.Now.ToString());
					
					transform.Transform(doc.CreateNavigator(), args, response.OutputStream);
					
					System.Console.WriteLine("Transform end: " + DateTime.Now.ToString());
				}
				else
				{
					response.StatusCode = 404;
					response.StatusDescription = "Unable to find xsl file";
				}
			}
		}
	}
}
