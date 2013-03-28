// NoteListDialog.cs
//
//  Copyright (C) 2008 David A Knight <david@ritter.demon.co.uk>
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
//
//

using System;
using Gedcom;

namespace Gedcom.UI.GTK
{
	
	
	public partial class NoteListDialog : Gtk.Dialog
	{
		
		#region Constructors
		
		public NoteListDialog()
		{
			this.Build();
			
			NotesListView.ListOnly = true;
		}
		
		#endregion
		
		#region Properties
		
		public GedcomDatabase Database
		{
			get { return NotesListView.Database; }
			set { NotesListView.Database = value; }
		}
		
		public GedcomRecord Record
		{
			get { return NotesListView.Record; }
			set 
			{ 
				NotesListView.Record = value; 
				
				// FIXME: major hack
				// create dummy record with all notes in it
				GedcomRecord rec = new GedcomRecord();
				rec.Database = Database;
				foreach (GedcomNoteRecord note in Database.Notes)
				{
					rec.Notes.Add(note.XRefID);
				}
				
				NotesListView.Record = rec;
			}
		}
		
		public GedcomNoteRecord Note
		{
			get { return NotesListView.Note; }
		}
		
		#endregion
	}
}
