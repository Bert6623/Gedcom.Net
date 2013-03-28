/*
 *  $Id: MarriageListModel.cs 183 2008-06-08 15:31:15Z davek $
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

using Gedcom;

namespace Gedcom.UI.GTK.Widgets
{
	
	// FIXME: use a GenericListModel<T>
	public class MarriageListModel : Gtk.ListStore
	{
		#region Variables
				
		protected GedcomDatabase _database;
		protected GedcomRecord _record;
		
		public const string UnknownName = "unknown /unknown/";
		
		#endregion
		
		#region Constructors
		
		public MarriageListModel() : base(typeof(GedcomIndividualEvent))
		{
			this.DefaultSortFunc = new Gtk.TreeIterCompareFunc(Compare);
			this.SetSortFunc(0, new Gtk.TreeIterCompareFunc(Compare));
		}
		
		#endregion
		
				
		#region Properties
		
		public GedcomDatabase Database
		{
			get { return _database; }
			set
			{
				_database = value;			
			}
		}
		
		public GedcomRecord Record
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
				
				GedcomIndividualRecord indi = _record as GedcomIndividualRecord;
				
					
				foreach (GedcomFamilyLink spouseIn in indi.SpouseIn)
				{
					string famID = spouseIn.Family;
					GedcomFamilyRecord fam = _database[famID] as GedcomFamilyRecord;
					
					if (fam != null)
					{
						GedcomFamilyEvent marriage = fam.Marriage;
						if (marriage != null)
						{
							Gtk.TreeIter iter = this.Append();
							this.SetValue(iter, 0, (object) marriage);
						}
					}
					else
					{
						System.Diagnostics.Debug.WriteLine("Family link points to non family record");	
					}
				}
			}
		}
		
		#endregion
		
				
		#region Methods
		
		public int Compare(Gtk.TreeModel model, Gtk.TreeIter a, Gtk.TreeIter b)
		{
			int ret = -1;
			
			GedcomIndividualEvent eventA = (GedcomIndividualEvent) model.GetValue(a,0);
			GedcomIndividualEvent eventB = (GedcomIndividualEvent) model.GetValue(b,0);
			
			if (eventA != null && eventB != null)
			{
				GedcomDate dateA = eventA.Date;
				GedcomDate dateB = eventB.Date;
				
				if (dateA != null && dateB != null)
				{
					DateTime dateTimeA;
					DateTime dateTimeB;
					
					if (DateTime.TryParse(dateA.Date1, out dateTimeA) && DateTime.TryParse(dateB.Date1, out dateTimeB))
					{
						ret = DateTime.Compare(dateTimeA, dateTimeB);
					}
				}
				else if (dateA != null)
				{
					ret = 1;	
				}
			}
			else if (eventA != null)
			{
				ret = 1;	
			}
			
			return ret;
		}
		
		#endregion
	}
}
