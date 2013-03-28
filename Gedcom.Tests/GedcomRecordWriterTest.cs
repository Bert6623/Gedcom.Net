/*
 *  $Id: GedcomRecordWriterTest.cs 192 2008-11-01 21:36:29Z davek $
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
using System.Collections;
using System.IO;
using NUnit.Framework;
using GedcomParser;

using Gedcom;

namespace GedcomParser
{
	[TestFixture()]
	public class GedcomRecordWriterTest
	{
		private GedcomRecordWriter _writer;
		
		public GedcomRecordWriterTest()
		{
		}
		
		private void Write(string file)
		{
			string dir = "/home/david/Projects/Gedcom.NET/Data/tests";
			string gedcomFile = Path.Combine(dir,file);
			
			string outputDir = Path.Combine(dir,"Output");
			string expectedDir = Path.Combine(dir,"Expected");
			
			GedcomRecordReader reader = new GedcomRecordReader();
			reader.ReadGedcom(gedcomFile);
			
			NUnit.Framework.Assert.Greater(reader.Database.Count,0,"No records read");
			
			_writer = new GedcomRecordWriter();
			_writer.Test = true;
			_writer.Database = reader.Database;
			_writer.GedcomFile = Path.Combine(outputDir,file);

			_writer.ApplicationName = "Gedcom.NET";
			_writer.ApplicationSystemID = "Gedcom.NET";
			_writer.ApplicationVersion = "Test Suite";
			_writer.Corporation = "David A Knight";
			
			_writer.WriteGedcom();
			
			string expectedOutput = Path.Combine(expectedDir,file);
			if (!File.Exists(expectedOutput))
			{
				File.Copy(_writer.GedcomFile,expectedOutput);	
			}
			
			string written = File.ReadAllText(_writer.GedcomFile);
			string expected = File.ReadAllText(expectedOutput);
								
			NUnit.Framework.Assert.IsTrue(written == expected, "Output differs from expected");
			
		}
		
		[Test]
		public void Test1()
		{
			Write("test1.ged");
		}
		
		[Test]
		public void Test2()
		{
			Write("test2.ged");
		}
		
		[Test]
		public void Presidents()
		{
			Write("presidents.ged");
		}
		
		[Test]
		public void Werrett()
		{
			Write("werrett.ged");
		}
		
		[Test]
		public void Whereat()
		{
			Write("whereat.ged");
		}
		
		[Test]
		public void Database1()
		{
			Write("Database1.ged");
		}
		
		[Test]
		public void Durand1()
		{
			Write("FAM_DD_4_2noms.ged");
		}
		
		[Test]
		public void Durand2()
		{
			Write("TOUT200801_unicode.ged");
		}
		
		[Test]
		public void Durand3()
		{
			Write("test_gedcom-net.ged");
		}

		[Test]
		public void Kollmann()
		{
			Write("Kollmann.ged");
		}

		[Test]
		public void TGC551LF()
		{
			Write("TGC551LF.ged");
		}
	}
}

