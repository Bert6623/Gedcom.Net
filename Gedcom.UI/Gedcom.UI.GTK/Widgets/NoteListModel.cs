/*
 *  $Id: NoteListModel.cs 183 2008-06-08 15:31:15Z davek $
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
	
	// FIXME: use a GenericListModel<T>
	public class NoteListModel : Gtk.ListStore
	{
		#region Variables
				
		protected GedcomDatabase _database;
		protected GedcomRecord _record;
				
		// bit of a hack, GedcomSourceRecord has an extra set
		// of notes for the DATA
		protected bool _dataNotes;
				
		#endregion
		
		#region Constructors
		
		public NoteListModel() : base(typeof(GedcomNoteRecord))
		{
			
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
				
				Clear();
				
				if (_record != null)
				{
					List<string> notes = null;
					if (_record.RecordType != GedcomRecordType.Source || !_dataNotes)
					{
						notes = _record.Notes;
					}
					else
					{
						GedcomSourceRecord sourceRecord = (GedcomSourceRecord)_record;
						notes = sourceRecord.DataNotes;
					}
					
					foreach (string noteID in notes)
					{
						GedcomNoteRecord note = _database[noteID] as GedcomNoteRecord;
						if (note != null)
						{
							Gtk.TreeIter iter = this.Append();
							this.SetValue(iter, 0, (object) note);	
						}
						else
						{
							System.Diagnostics.Debug.WriteLine("Note link points to non note record");	
						}
					}
				}
			}
		}
		
		public bool DataNotes
		{
			get { return _dataNotes; }
			set { _dataNotes = value; }
		}
		
		#endregion
		
				
		#region Methods
		
		
		#endregion
	}
}
