/*
 *  $Id: GedcomRecordReaderTest.cs 192 2008-11-01 21:36:29Z davek $
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
	public class GedcomRecordReaderTest
	{
		private GedcomRecordReader _reader;
		private int _individuals;
		private int _families;
		
		public GedcomRecordReaderTest()
		{
		}
		
		private void Read(string file)
		{
			string dir = "/home/david/Projects/Gedcom.NET/Data/tests";
			string gedcomFile = Path.Combine(dir,file);

			long start = DateTime.Now.Ticks;
			_reader = new GedcomRecordReader();
			bool success = _reader.ReadGedcom(gedcomFile);
			long end = DateTime.Now.Ticks;
			
			System.Console.WriteLine("Read time: " + TimeSpan.FromTicks(end - start).TotalSeconds + " seconds");
			
			NUnit.Framework.Assert.AreEqual(true, success, "Failed to read " + gedcomFile);
			
			_individuals = 0;
			_families = 0;
			
			NUnit.Framework.Assert.Greater(_reader.Database.Count,0,"No records read");
						
			foreach (DictionaryEntry entry in _reader.Database)
			{
				GedcomRecord record = entry.Value as GedcomRecord;
							
				if (record.RecordType == GedcomRecordType.Individual)
				{
					_individuals ++;	
				}
				else if (record.RecordType == GedcomRecordType.Family)
				{
					_families ++;
				}
			}

			System.Console.WriteLine(gedcomFile + " contains " + _individuals + " individuals");
		}
			
		[Test]
		public void Test1()
		{
			Read("test1.ged");
			
			// file has 91 INDI, 1 is  HEAD/_SCHEMA/INDI though
			
			NUnit.Framework.Assert.AreEqual(90,_individuals,"Not read all individuals");
			NUnit.Framework.Assert.AreEqual(15,_families,"Not read all families");
		}
		
		[Test]
		public void Test2()
		{
			Read("test2.ged");
			
			NUnit.Framework.Assert.AreEqual(4,_individuals,"Not read all individuals");
			NUnit.Framework.Assert.AreEqual(2,_families,"Not read all families");
		}
		
		[Test]
		public void Test3()
		{
			Read("test3.ged");
			
			NUnit.Framework.Assert.AreEqual(4,_individuals,"Not read all individuals");
			NUnit.Framework.Assert.AreEqual(2,_families,"Not read all families");
		}
		
		[Test]
		public void Presidents()
		{
			Read("presidents.ged");
			
			NUnit.Framework.Assert.AreEqual(2145,_individuals,"Not read all individuals");
			NUnit.Framework.Assert.AreEqual(1042,_families,"Not read all families");
		}
		
		[Test]
		public void Werrett()
		{
			Read("werrett.ged");
			
			NUnit.Framework.Assert.AreEqual(12338,_individuals,"Not read all individuals");
			NUnit.Framework.Assert.AreEqual(4206,_families,"Not read all families");
		}
		
		[Test]
		public void Whereat()
		{
			Read("whereat.ged");
			
			NUnit.Framework.Assert.AreEqual(263,_individuals,"Not read all individuals");
			NUnit.Framework.Assert.AreEqual(78,_families,"Not read all families");
		}
		
		[Test]
		public void Database1()
		{
			Read("Database1.ged");
			
			// file has 24963 INDI, 1 is in a CONT
			
			NUnit.Framework.Assert.AreEqual(24962,_individuals,"Not read all individuals");
			NUnit.Framework.Assert.AreEqual(8217,_families,"Not read all families");
		}
		
		[Test]
		public void TGC551LF()
		{
			Read("TGC551LF.ged");

			GedcomHeader header = _reader.Database.Header;
		 
			NUnit.Framework.Assert.IsNotNull(header.Copyright, "Missing copyright");
			NUnit.Framework.Assert.IsNotEmpty(header.Copyright, "Missing copyright");

			NUnit.Framework.Assert.AreEqual("John A. Nairn", header.Submitter.Name, "Submitter not correctly read");

			NUnit.Framework.Assert.AreNotEqual(null, header.ContentDescription, "Missing content description");

			NUnit.Framework.Assert.AreNotEqual(null, header.CorporationAddress, "Missing corporation address");
			
			NUnit.Framework.Assert.AreEqual(15,_individuals,"Not read all individuals");
			NUnit.Framework.Assert.AreEqual(7,_families,"Not read all families");
		}
		
		[Test]
		public void Durand1()
		{
			Read("FAM_DD_4_2noms.ged");
			
			NUnit.Framework.Assert.AreEqual(5,_individuals,"Not read all individuals");
			NUnit.Framework.Assert.AreEqual(2,_families,"Not read all families");
		}
		
		[Test]
		public void Durand2()
		{
			Read("TOUT200801_unicode.ged");
		}

		[Test]
		public void Durand3()
		{
			Read("test_gedcom-net.ged");
		}

		[Test]
		public void Kollmann()
		{
			Read("Kollmann.ged");

			NUnit.Framework.Assert.AreEqual(408,_individuals,"Not read all individuals");
			NUnit.Framework.Assert.AreEqual(156,_families,"Not read all families");
		}
	}
}

