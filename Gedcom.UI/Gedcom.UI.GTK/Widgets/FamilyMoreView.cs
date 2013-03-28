/*
 *  $Id: FamilyMoreView.cs 189 2008-10-10 14:16:10Z davek $
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
using Gedcom.UI.Common;

namespace Gedcom.UI.GTK.Widgets
{
	
	[System.ComponentModel.Category("Gedcom.UI.GTK.Widgets")]
	[System.ComponentModel.ToolboxItem(true)]
	public partial class FamilyMoreView : Gtk.Bin, IGedcomView
	{
		#region Variables
		
		protected GedcomDatabase _database;
		protected GedcomRecord _record;
				
		#endregion
		
		#region Constructors
		
		public FamilyMoreView()
		{
			this.Build();
					
			MarriageView.ShowMoreButton = false;
			MarriageView.ShowSourceCitation += new EventHandler<SourceCitationArgs>(OnMarriageSourceButton_Clicked);
			MarriageView.ShowScrapBook += new EventHandler<ScrapBookArgs>(OnMarriageScrapbookButton_Clicked);
								
			Notebook.Page = 0;
		}
		
		#endregion
		
		#region IGedcomView
		
		#region Properties
		
		public GedcomDatabase Database
		{
			get { return _database; }
			set
			{
				_database = value;
				MarriageView.Database = value;
				FactView.Database = value;
				AddressView.Database = value;
				NotesView.Database = value;
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
				
				if (_record.RecordType != GedcomRecordType.Individual &&
					_record.RecordType != GedcomRecordType.Family)
				{
					throw new Exception("Invalid record type given to FamilyView");
				}
				
				ClearView();			
				FillView();
			}
		}
		
		public GedcomIndividualRecord Husband
		{
			get { return MarriageView.Husband; }
		}
		
		public GedcomIndividualRecord Wife
		{
			get { return MarriageView.Wife; }
		}
		
		#endregion
		
		#region Events
		
		public event EventHandler<IndividualArgs> MoreInformation;
		public event EventHandler<FamilyArgs> MoreFamilyInformation;
		public event EventHandler<SpouseSelectArgs> SpouseSelect;
		public event EventHandler<SourceCitationArgs> ShowSourceCitation;
		public event EventHandler<IndividualArgs> SelectNewChild;
		public event EventHandler<IndividualArgs> SelectNewSpouse;
		public event EventHandler<ScrapBookArgs> ShowScrapBook;
		public event EventHandler<IndividualArgs> ShowName;
		public event EventHandler<IndividualArgs> DeleteIndividual;
		public event EventHandler<FactArgs> MoreFactInformation;
		public event EventHandler<NoteArgs> SelectNewNote;
		
		#endregion
		
		#region Methods
		
		public void ClearView()
		{	
			MarriageView.Clear();
		 	
			FactView.Clear();
			AddressView.ClearView();
			NotesView.Clear();
		}
		
		public void SaveView()
		{
			MarriageView.Save();
			FactView.Save();
			AddressView.SaveView();
			NotesView.Save();
		}
		
		#endregion
		
		#endregion
		
		#region EventHandlers
		
		protected virtual void OnMarriageSourceButton_Clicked(object sender, SourceCitationArgs e)
		{
			if (ShowSourceCitation != null)
			{
				ShowSourceCitation(this,e);
			}
		}
			
		protected virtual void OnFactView_EventAdded(object sender, System.EventArgs e)
		{
			GedcomEvent ev = FactView.Event;
			
			if (ev != null)
			{
				switch (ev.EventType)
				{
					case GedcomEvent.GedcomEventType.MARR:
						break;
				}
			}
		}

		protected virtual void OnFactView_EventRemoved(object sender, System.EventArgs e)
		{
			GedcomEvent ev = FactView.Event;
			
			if (ev != null)
			{
				switch (ev.EventType)
				{
					case GedcomEvent.GedcomEventType.MARR:
						MarriageView.Clear();
						break;
				}
			}
		}
		
		protected virtual void OnFactView_ShowSourceCitation(object sender, SourceCitationArgs e)
		{
			if (ShowSourceCitation != null)
			{
				ShowSourceCitation(this, e);	
			}
		}
		
		protected virtual void OnMarriageScrapbookButton_Clicked(object sender, ScrapBookArgs e)
		{
			if (ShowScrapBook != null)
			{

				ShowScrapBook(this, e);
			}
		}
		
		
		
		protected virtual void OnAddressView_ShowSourceCitation (object sender, SourceCitationArgs e)
		{
			if (ShowSourceCitation != null)
			{
				ShowSourceCitation(this, e);
			}
		}

		protected virtual void OnAddressView_ShowScrapBook (object sender, ScrapBookArgs e)
		{
			if (ShowScrapBook != null)
			{
				ShowScrapBook(this, e);
			}
		}

		protected virtual void OnAddressView_MoreFactInformation (object sender, FactArgs e)
		{
			if (MoreFactInformation != null)
			{
				MoreFactInformation(this, e);
			}
		}

		protected virtual void OnFactView_ShowScrapBook (object sender, ScrapBookArgs e)
		{
			if (ShowScrapBook != null)
			{
				ShowScrapBook(this, e);
			}
		}

		protected virtual void OnFactView_MoreInformation (object sender, FactArgs e)
		{
			if (MoreFactInformation != null)
			{
				MoreFactInformation(this, e);
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
		
		private void FillView()
		{
			MarriageView.Record = _record;
			FactView.Record = _record;
			AddressView.Record = _record;
			NotesView.Record = _record;
		}
		
		#endregion
		
	}
}
