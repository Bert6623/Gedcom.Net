/*
 *  $Id: ChildrenListModel.cs 183 2008-06-08 15:31:15Z davek $
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
	public class ChildrenListModel : IndividualListModel
	{
		#region Variables
		
		private List<string> _children;
		
		#endregion
		
		#region Constructors
		
		public ChildrenListModel()
		{
			_children = new List<string>();	
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
				
				GedcomFamilyRecord fam = null;
				
				if (_record is GedcomIndividualRecord)
				{
					// get all family records for the individual, create
					// a dummy family with all children in it.
					
					GedcomIndividualRecord indi = _record as GedcomIndividualRecord;
					fam = indi.GetAllChildren();
				}
				else if (!(_record is GedcomFamilyRecord))
				{
					throw new Exception("Must provide a family record");	
				}
				else
				{
					fam = _record as GedcomFamilyRecord;	
				}
								
				_children.Clear();
				
				foreach (string childID in fam.Children)
				{
					_children.Add(childID);
				}
				
				DoFill();
			}
		}
		
		#endregion
		
		#region Methods
		
		protected override bool Filter(GedcomIndividualRecord indi)
		{
			return ((indi == null) || _children.Contains(indi.XRefID));	
		}
		
		#endregion
	}
}
