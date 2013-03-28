/*
 *  $Id: SpouseListModel.cs 194 2008-11-10 20:39:37Z davek $
 * 
 *  Copyright (C) 2007-2008 David A Knight <david@ritter.demon.co.uk>
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
using System.Collections.Generic;

using Gedcom;

namespace Gedcom.UI.GTK.Widgets
{
	
	
	public class SpouseListModel : IndividualListModel
	{

		#region Variables
		
		private List<string> _spouses;		
		private List<string> _families;
		private List<bool> _prefered;

		private GedcomIndividualRecord _indi;
		
		#endregion
		
		#region Constructors
		
		public SpouseListModel()
		{
			_spouses = new List<string>();
			_families = new List<string>();
			_prefered = new List<bool>();
			_applyFilter = true;
		}
		
		#endregion
		
				
		#region Properties
				
		public override GedcomDatabase Database
		{
			get { return _database; }
			set { _database = value; }
		}
		
		public override GedcomRecord Record
		{
			get { return _record; }
			set
			{
				if (_database == null)
				{
					throw new Exception("Database must be set before Record");	
				}
								
				_record = value;
				
				if (!(_record is GedcomIndividualRecord))
				{
					throw new Exception("Must provide an individual record");	
				}
				
				_spouses.Clear();
				_families.Clear();
				
				_indi = _record as GedcomIndividualRecord;
				
				foreach (GedcomFamilyLink spouseIn in _indi.SpouseIn)
				{
					string famID = spouseIn.Family;
					GedcomFamilyRecord fam = _database[famID] as GedcomFamilyRecord;
					if (fam != null)
					{
						string spouseID = string.Empty;
						
						if (fam.Husband == _indi.XRefID)
						{
							spouseID = fam.Wife;	
						}
						else
						{
							spouseID = fam.Husband;	
						}
						
						if (!string.IsNullOrEmpty(spouseID))
						{
							_spouses.Add(spouseID);
							_families.Add(famID);
							_prefered.Add(spouseIn.PreferedSpouse);
						}
					}
					else
					{
						System.Diagnostics.Debug.WriteLine("Family link points to non family record");	
					}
				}
				
				DoFill();
			}
		}
		
		#endregion
		
				
		#region Methods
			
		protected override bool Filter(GedcomIndividualRecord indi)
		{
			return ((indi == null) || _spouses.Contains(indi.XRefID));	
		}
		
		public GedcomFamilyRecord GetFamily(string xRefID)
		{
			int i = _spouses.IndexOf(xRefID);
			string famID = _families[i];
					
			return _database[famID] as GedcomFamilyRecord;
		}

		public bool Prefered(string xRefID)
		{
			int i = _spouses.IndexOf(xRefID);

			return _prefered[i];
		}

		public void SetPrefered(string xRefID)
		{
			int i = _spouses.IndexOf(xRefID);
			string spouseXrefID = _spouses[i];
			
			_indi.SetPreferedSpouse(spouseXrefID);
			for (int j = 0; j < _prefered.Count; j ++)
			{
				_prefered[j] = (j == i);
			}
				
		}
		
		#endregion
	}
}
