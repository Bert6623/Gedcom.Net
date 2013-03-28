/*
 *  $Id: SiblingsListModel.cs 183 2008-06-08 15:31:15Z davek $
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
using System.Collections.Generic;

using Gedcom;

namespace Gedcom.UI.GTK.Widgets
{
	
	
	public class SiblingsListModel : IndividualListModel
	{
		#region Variables
		
		private List<string> _siblings;		
		
		#endregion
		
		#region Constructors
		
		public SiblingsListModel()
		{
			_siblings = new List<string>();
		}
		
		#endregion
		
				
		#region Properties
			
		public override GedcomDatabase Database
		{
			get { return _database; }
			set	{ _database = value; }
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
							
				if (_record is GedcomIndividualRecord)
				{
					_siblings.Clear();
					
					
					GedcomIndividualRecord indi = _record as GedcomIndividualRecord;
				
					foreach (GedcomFamilyLink link in indi.ChildIn)
					{
						GedcomFamilyRecord famRec = _database[link.Family] as GedcomFamilyRecord;
						if (famRec != null)
						{
							foreach (string childID in famRec.Children)
							{
								if (childID != indi.XRefID)
								{
									_siblings.Add(childID);
								}
							}
						}
					}				
				}
				else
				{
					throw new Exception("Must provide an individual record");	
				}
								
				DoFill();
			}
		}
		
		#endregion
		
				
		#region Methods
		
		protected override bool Filter(GedcomIndividualRecord indi)
		{
			return ((indi == null) || _siblings.Contains(indi.XRefID));	
		}
		
		
		#endregion
	}
}
