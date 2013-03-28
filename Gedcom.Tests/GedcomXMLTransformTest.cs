/*
 *  $Id: GedcomXMLTransformTest.cs 183 2008-06-08 15:31:15Z davek $
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
using NUnit.Framework;
using System.Xml.XPath;
using System.Xml.Xsl;

using Gedcom;
using GedcomParser;

namespace Gedcom.Reports
{
	
	
	[TestFixture()]
	public class GedcomXMLTransformTest
	{
				
		private void DumpXML(string file)
		{
			string dir = "/home/david/Projects/Gedcom.NET/Data/tests";
			
			string xmlOutput = Path.Combine(dir, "XmlOutput");
			string xmlFile = Path.Combine(xmlOutput, file + ".xml");
			
			string xslFile = "/home/david/Projects/Gedcom.NET/Data/tests/Xsl/Surnames.xsl";
			
			XPathDocument doc = new XPathDocument(xmlFile);
				
			XslTransform transform = new XslTransform();
			transform.Load(xslFile);
									
			transform.Transform(doc, null, System.Console.Out);
			System.Console.Out.Flush();
		}
		
		[Test]
		public void Test1()
		{
			DumpXML("test1.ged");
		}
		
		[Test]
		public void Test2()
		{
			DumpXML("test2.ged");
		}
		
		[Test]
		public void Presidents()
		{
			DumpXML("presidents.ged");
		}
		
		[Test]
		public void Werrett()
		{
			DumpXML("werrett.ged");
		}
		
		[Test]
		public void Whereat()
		{
			DumpXML("whereat.ged");
		}
		
		[Test]
		public void Database1()
		{
			DumpXML("Database1.ged");
		}
	}
}
