/*
 *  $Id: global.asax.cs 183 2008-06-08 15:31:15Z davek $
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
using System.Configuration;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Xml.XPath;


public partial class Global : HttpApplication
{
	protected void Application_Start(object sender, EventArgs e)
	{
		if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["gedcomDirectory"]))
		{
			Application["GedcomDirectory"] = ConfigurationManager.AppSettings["gedcomDirectory"].Trim();		
		}
		else
		{
			throw new Exception("gedcomDirectory required in site.config");	
		}
		
		if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["gedcomFile"]))
		{
			Application["GedcomFile"] = ConfigurationManager.AppSettings["gedcomFile"].Trim();		
		}
		else
		{
			throw new Exception("gedcomFile required in site.config");	
		}
		
		if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["xslDirectory"]))
		{
			Application["XSLDirectory"] = ConfigurationManager.AppSettings["xslDirectory"].Trim();		
		}
		else
		{
			throw new Exception("xslDirectory required in site.config");	
		}
		
		string xmlFile = Path.Combine((string)Application["GedcomDirectory"], (string)Application["GedcomFile"]);
		
		XPathDocument doc = new XPathDocument(xmlFile);
			
		
		Application["XMLDoc"] = doc;
	}
	
	protected void Session_Start(object sender, EventArgs e)
	{
	
	}
	
	protected void Application_Error(object sender, EventArgs e)
	{
	
	}
	
	protected void Session_End(object sender, EventArgs e)
	{
	
	}
	
	protected void Application_End(object sender, EventArgs e)
	{
	
	}
	
	public void Application_BeginRequest(object sender, EventArgs e)
	{
		HttpRequest request = HttpContext.Current.Request;
		
		string path = request.Path;
		
		if (!string.IsNullOrEmpty(request.ApplicationPath))
		{
			path = path.Substring(request.ApplicationPath.Length);
		}
		
		// URL Formats:
		// 				/individual/surname/rest of name
		
		path = path.Trim("/".ToCharArray());	
		
		string[] parts = path.Split('/');
		
		string handler = VirtualPathUtility.ToAbsolute("~/GedcomWebHandler.ashx");
		
		NameValueCollection newQuery = new NameValueCollection();
		
		bool rewrite = false;
		
		if (parts.Length == 0 || parts[0] == string.Empty)
		{
			// Summary + list sections: family, individuals, sources, repos
			
			rewrite = true;
		}
		else if (parts.Length == 1 && parts[0] == "individual")
		{
			// List surnames
			
			newQuery.Add("listsurnames", "1");
			
			rewrite = true;
		}
		else if (parts.Length == 2 && parts[0] == "individual")
		{
			// list all individuals with matching surname
			
			string surname = parts[1];
			
			newQuery.Add("surname", surname);
			
			rewrite = true;
		}
		else if (parts.Length == 3 && parts[0] == "individual")
		{
			// show individuals with matching name
			
			string surname = parts[1];
			string restOfName = parts[2];
			
			newQuery.Add("surname", surname);
			newQuery.Add("restOfName", restOfName);
			
			rewrite = true;
		}
		else
		{
			
		}
		
		if (rewrite)
		{
			StringBuilder sb = new StringBuilder();
			
			sb.Append(handler);
			if (newQuery.Count > 0)
			{
				bool first = true;
				foreach (string key in newQuery.Keys)
				{
					if (first)
					{
						sb.Append("?");
						first = false;
					}
					else
					{
						sb.Append("&");	
					}
					sb.Append(key);
					sb.Append("=");
					sb.Append(newQuery[key]);
				}
			}
						
			HttpContext.Current.RewritePath(sb.ToString());
		}
	}


	public void Application_AuthenticateRequest(object sender, EventArgs e)
	{
	
	}
	
	
	public void Application_PostAuthenticateRequest(object sender, EventArgs e)
	{
	
	}


	public void Application_AuthorizeRequest(object sender, EventArgs e)
	{
	
	}


	public void Application_PostAuthorizeRequest(object sender, EventArgs e)
	{
	
	}
	

	public void Application_ResolveRequestCache(object sender, EventArgs e)
	{
	
	}


	public void Application_PostResolveRequestCache(object sender, EventArgs e)
	{
	
	}
	
	public void Application_PostMapRequestHandler(object sender, EventArgs e)
	{
	
	}
	

	public void Application_AcquireRequestState(object sender, EventArgs e)
	{

	}

	public void Application_PostAcquireRequestState(object sender, EventArgs e)
	{
	
	}

	public void Application_PreRequestHandlerExecute(object sender, EventArgs e)
	{
	
	}


	public void Application_PostRequestHandlerExecute(object sender, EventArgs e)
	{
	
	}


	public void Application_ReleaseRequestState(object sender, EventArgs e)
	{
	
	}


	public void Application_PostReleaseRequestState(object sender, EventArgs e)
	{
	
	}


	public void Application_UpdateRequestCache(object sender, EventArgs e)
	{
	
	}

	public void Application_PostUpdateRequestCache(object sender, EventArgs e)
	{
	
	}


	public void Application_EndRequest(object sender, EventArgs e)
	{
	
	}

	
}
