// $Id: FactDetailView.cs 199 2008-11-15 15:20:44Z davek $
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
using Gedcom.UI.Common;

namespace Gedcom.UI.GTK.Widgets
{
	
	
	[System.ComponentModel.Category("Gedcom.UI.GTK.Widgets")]
	[System.ComponentModel.ToolboxItem(true)]
	public partial class FactDetailView : Gtk.Bin
	{
		#region Variables
		
		private GedcomDatabase _database;
		private GedcomRecord _record;
		
		#endregion 
		
		#region Constructors
		
		public FactDetailView()
		{
			this.Build();
		}
		
		#endregion
		
		#region Properties
		
		public GedcomDatabase Database
		{
			get { return _database; }
			set 
			{ 
				_database = value; 
				AddressView.Database = _database;
				NotesView.Database = _database;
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
				
				
				Clear();
				
				_record = value;
				
				if ( _record == null ||
				    (_record.RecordType != GedcomRecordType.Event &&
				    _record.RecordType != GedcomRecordType.FamilyEvent &&
				    _record.RecordType != GedcomRecordType.IndividualEvent))
				{
					throw new Exception("Record must be an event/fact");
				}
				
				FillView();
			}
		}
		
		#endregion
		
		#region Events
		
		public event EventHandler<SourceCitationArgs> ShowSourceCitation;
		public event EventHandler<ScrapBookArgs> ShowScrapBook;
		public event EventHandler<NoteArgs> SelectNewNote;
		
		#endregion

		#region Event Handlers
		
		protected virtual void OnFactSourceButton_Clicked (object sender, System.EventArgs e)
		{
			if (ShowSourceCitation != null)
			{
				SourceCitationArgs args = new SourceCitationArgs();
				args.Record = _record;
				ShowSourceCitation(this, args);
			}
		}

		protected virtual void OnEventScrapbookButton_Clicked (object sender, System.EventArgs e)
		{
			if (ShowScrapBook != null)
			{
				ScrapBookArgs args = new ScrapBookArgs();
				args.Record = _record;
				ShowScrapBook(this, args);
			}
		}
		
		protected virtual void OnNotesView_ShowSourceCitation (object sender, SourceCitationArgs e)
		{
			if (ShowSourceCitation != null)
			{
				ShowSourceCitation(this, e);
			}
		}
	
		protected virtual void OnNotesView_SelectNewNote (object sender, NoteArgs e)
		{
			if (SelectNewNote != null)
			{
				SelectNewNote(this, e);
			}
		}
		
		#endregion
		
		#region Methods
		
		private void Clear()
		{
			EventNameLabel.Text = "Unknown Event";
			
			EventTypeEntry.Text = string.Empty;
			FactDateEntry.Text = string.Empty;
			CommentTextView.Buffer.Clear();
			CauseEntry.Text = string.Empty;
			AgencyEntry.Text = string.Empty;
			ReligiousEntry.Text = string.Empty;
			
			AddressView.ClearView();
						
			NotesView.Clear();
		}
		
		public void SaveView()
		{
			GedcomEvent ev = (GedcomEvent)_record;

			if (!string.IsNullOrEmpty(EventExtraEntry.Text))
			{
				ev.EventName = EventExtraEntry.Text;
			}
			
			if (!string.IsNullOrEmpty(EventTypeEntry.Text))
			{
				ev.Classification = EventTypeEntry.Text;
			}
			
			if (!string.IsNullOrEmpty(FactDateEntry.Text))
			{
				if (ev.Date == null)
				{
					ev.Date = new GedcomDate(_database);
					ev.Date.Level = ev.Level + 1; 
				}
				ev.Date.ParseDateString(FactDateEntry.Text);
			}
			else
			{
				ev.Date = null;
			}
			
			string place = CommentTextView.Buffer.Text;
			if (!string.IsNullOrEmpty(place))
			{
				if (ev.Place == null)
				{
					ev.Place = new GedcomPlace();
					ev.Place.Level = ev.Level + 1;
					ev.Place.Database = _database;
				}
				ev.Place.Name = place;	
			}
			else
			{
				ev.Place = null;
			}
			
			ev.Cause = CauseEntry.Text;
			ev.ResponsibleAgency = AgencyEntry.Text;
			ev.ReligiousAffiliation = ReligiousEntry.Text;
			
			AddressView.SaveView();
						
			NotesView.Save();
		}
		
		private void FillView()
		{
			Notebook.Page = 0;
			
			GedcomEvent ev = (GedcomEvent)_record;
			
			EventNameLabel.Text = GedcomEvent.TypeToReadable(ev.EventType);
			
			if (!string.IsNullOrEmpty(ev.EventName))
			{
				EventExtraEntry.Text = ev.EventName;
			}
			
			if (!string.IsNullOrEmpty(ev.Classification))
			{
				EventTypeEntry.Text = ev.Classification;
			}
			
			GedcomDate date = ev.Date;
			if (date != null)
			{
				FactDateEntry.Text = date.DateString;
			}
			GedcomPlace place = ev.Place;
			if (place != null)
			{
				CommentTextView.Buffer.Text = place.Name;
			}
			
			if (!string.IsNullOrEmpty(ev.Cause))
			{
				CauseEntry.Text = ev.Cause;
			}
			if (!string.IsNullOrEmpty(ev.ResponsibleAgency))
			{
				AgencyEntry.Text = ev.ResponsibleAgency;
			}
			if (!string.IsNullOrEmpty(ev.ReligiousAffiliation))
			{
				ReligiousEntry.Text = ev.ReligiousAffiliation;
			}
			
			AddressView.Record = _record;
			NotesView.Record = _record;		
		}

		#endregion
	}
}
