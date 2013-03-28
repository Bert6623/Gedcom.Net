/*
 *  $Id: GedcomDeleteTest.cs 199 2008-11-15 15:20:44Z davek $
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

namespace Gedcom
{
	
	
	[TestFixture()]
	public class GedcomDeleteTest
	{
		private GedcomRecordReader _reader;
		private int _individuals;
		private int _families;
		
		private void Read(string file)
		{
			string dir = "/home/david/Projects/Gedcom.NET/Data/tests";
			string gedcomFile = Path.Combine(dir,file);

			long start = DateTime.Now.Ticks;
			_reader = new GedcomRecordReader();
			bool success = _reader.ReadGedcom(gedcomFile);
			long end = DateTime.Now.Ticks;
					
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
		}
		
		
		[Test]
		public void Test1()
		{
			Read("test1.ged");
			
			NUnit.Framework.Assert.AreEqual(90,_individuals,"Not read all individuals");
			NUnit.Framework.Assert.AreEqual(15,_families,"Not read all families");
			
			string id = _reader.Parser.XrefCollection["I0145"];
			string sourceID = _reader.Parser.XrefCollection["S01668"];
			string sourceID2 = _reader.Parser.XrefCollection["S10021"];
			
			GedcomIndividualRecord indi = (GedcomIndividualRecord)_reader.Database[id];
			
			NUnit.Framework.Assert.AreNotEqual(null, _reader.Database[sourceID], "Unable to find expected source");
			NUnit.Framework.Assert.AreNotEqual(null, _reader.Database[sourceID2], "Unable to find expected source 2");
			
			indi.Delete();
			
			
			NUnit.Framework.Assert.AreEqual(89, _reader.Database.Individuals.Count,"Failed to delete individual");
			NUnit.Framework.Assert.AreEqual(15, _reader.Database.Families.Count,"Incorrectly erased family");		
	
			NUnit.Framework.Assert.AreNotEqual(null, _reader.Database[sourceID2], "Source incorrectly deleted when deleting individual");
			
			// source should still have a count of 1, the initial ref, we don't want to delete just because all citations
			// have gone, leave the source in the database.
			NUnit.Framework.Assert.AreNotEqual(null, _reader.Database[sourceID], "Source incorrectly deleted when only used by the deleted individual");
		}
		
		[Test]
		public void Test2()
		{
			Read("test2.ged");
			
			NUnit.Framework.Assert.AreEqual(4,_individuals,"Not read all individuals");
			NUnit.Framework.Assert.AreEqual(2,_families,"Not read all families");
			
			string id = _reader.Parser.XrefCollection["I04"];
			
			GedcomIndividualRecord indi = (GedcomIndividualRecord)_reader.Database[id];
			
			indi.Delete();
			
			
			NUnit.Framework.Assert.AreEqual(3, _reader.Database.Individuals.Count,"Failed to delete individual");
			NUnit.Framework.Assert.AreEqual(2, _reader.Database.Families.Count,"Incorrectly erased family");
			
			id = _reader.Parser.XrefCollection["I01"];
			
			indi = (GedcomIndividualRecord)_reader.Database[id];
			
			indi.Delete();
			
			NUnit.Framework.Assert.AreEqual(2, _reader.Database.Individuals.Count,"Failed to delete individual");
			NUnit.Framework.Assert.AreEqual(1, _reader.Database.Families.Count,"Incorrectly erased family");
			
			id = _reader.Parser.XrefCollection["I02"];
			
			indi = (GedcomIndividualRecord)_reader.Database[id];
			
			indi.Delete();
			
			NUnit.Framework.Assert.AreEqual(1, _reader.Database.Individuals.Count,"Failed to delete individual");
			NUnit.Framework.Assert.AreEqual(1, _reader.Database.Families.Count,"Incorrectly erased family");
			
			GedcomFamilyRecord famRec = _reader.Database.Families[0];
			string noteID = famRec.Notes[0];
			
			NUnit.Framework.Assert.AreNotEqual(null, _reader.Database[noteID], "Couldn't find expected note on family");
			
			id = _reader.Parser.XrefCollection["I03"];
			
			indi = (GedcomIndividualRecord)_reader.Database[id];
			
			indi.Delete();
			
			NUnit.Framework.Assert.AreEqual(0, _reader.Database.Individuals.Count,"Failed to delete individual");
			NUnit.Framework.Assert.AreEqual(0, _reader.Database.Families.Count,"Incorrectly erased family");
			
			NUnit.Framework.Assert.AreEqual(null, _reader.Database[noteID], "Incorrectly erased note from family");
		}
	
		[Test]
		public void Test3()
		{
			Read("test3.ged");
			
			NUnit.Framework.Assert.AreEqual(4,_individuals,"Not read all individuals");
			NUnit.Framework.Assert.AreEqual(2,_families,"Not read all families");
			
			string id = _reader.Parser.XrefCollection["I04"];
			
			GedcomIndividualRecord indi = (GedcomIndividualRecord)_reader.Database[id];
			
			indi.Delete();
			
			
			NUnit.Framework.Assert.AreEqual(3, _reader.Database.Individuals.Count,"Failed to delete individual");
			NUnit.Framework.Assert.AreEqual(2, _reader.Database.Families.Count,"Incorrectly erased family");
			
			id = _reader.Parser.XrefCollection["I01"];
			
			indi = (GedcomIndividualRecord)_reader.Database[id];
			
			indi.Delete();
			
			NUnit.Framework.Assert.AreEqual(2, _reader.Database.Individuals.Count,"Failed to delete individual");
			NUnit.Framework.Assert.AreEqual(1, _reader.Database.Families.Count,"Incorrectly erased family");
			
			id = _reader.Parser.XrefCollection["I02"];
			
			indi = (GedcomIndividualRecord)_reader.Database[id];
			
			indi.Delete();
			
			NUnit.Framework.Assert.AreEqual(1, _reader.Database.Individuals.Count,"Failed to delete individual");
			NUnit.Framework.Assert.AreEqual(1, _reader.Database.Families.Count,"Incorrectly erased family");
			
			GedcomFamilyRecord famRec = _reader.Database.Families[0];
			string noteID = famRec.Notes[0];
			
			NUnit.Framework.Assert.AreNotEqual(null, _reader.Database[noteID], "Couldn't find expected note on family");
			
			id = _reader.Parser.XrefCollection["I03"];
			
			indi = (GedcomIndividualRecord)_reader.Database[id];
			
			indi.Delete();
			
			NUnit.Framework.Assert.AreEqual(0, _reader.Database.Individuals.Count,"Failed to delete individual");
			NUnit.Framework.Assert.AreEqual(0, _reader.Database.Families.Count,"Incorrectly erased family");
			
			NUnit.Framework.Assert.AreNotEqual(null, _reader.Database[noteID], "Incorrectly erased note linked from family");
		}
	}
}
