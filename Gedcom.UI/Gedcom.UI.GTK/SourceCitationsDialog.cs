/*
 *  $Id: SourceCitationsDialog.cs 183 2008-06-08 15:31:15Z davek $
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
using Gedcom.UI.GTK.Widgets;
using Gedcom.UI.Common;

namespace Gedcom.UI.GTK
{
	
	
	public partial class SourceCitationsDialog : Gtk.Dialog
	{
		#region Variables
				
		#endregion
				
		#region Constructors
		
		public SourceCitationsDialog()
		{
			this.Build();
			
			SourceCitationList.RecordChanged += new EventHandler(OnRecordChanged);
			
		
			SourceCitationView.Sensitive = false;
		}
		
		#endregion
		
		#region Properties
		
		public GedcomDatabase Database
		{
			get { return SourceCitationList.Database; }
			set
			{
				SourceCitationList.Database = value;
				SourceCitationView.Database = value;
			}
		}
		
		public GedcomRecord Record
		{
			get { return SourceCitationList.Record; }
			set { SourceCitationList.Record = value;  }
		}
		
		public Gedcom.UI.GTK.Widgets.SourceCitationList List
		{
			get { return SourceCitationList; }
		}
		
		public GedcomSourceRecord MasterSource
		{
			get { return SourceCitationView.MasterSource; }	
		}
		
		#endregion
		
		#region Events
		
		public event EventHandler ViewMasterSource;
		public event EventHandler<ScrapBookArgs> ShowScrapBook;
		public event EventHandler<SourceCitationArgs> ShowSourceCitation;
		public event EventHandler<NoteArgs> SelectNewNote;
		
		#endregion
		
		#region EventHandlers
		
		protected void OnRecordChanged(object sender, EventArgs e)
		{
			GedcomRecord citation = SourceCitationList.Record;
			
			SourceCitationView.Sensitive = (citation != null);
			
			if (citation != null)
			{
				SourceCitationView.Record = citation;	
			}
		}
		
		protected void OnViewMasterSource(object sender, EventArgs e)
		{
			if (ViewMasterSource != null)
			{
				ViewMasterSource(this,EventArgs.Empty);	
			}
		}
		
		protected void OnSelectMasterSource(object sender, SourceArgs e)
		{
			SourceListDialog listDialog = new SourceListDialog();
		
			listDialog.Database = Database;
			listDialog.Record = Record;
			
			int response = listDialog.Run();
			
			switch (response)
			{
				case (int)Gtk.ResponseType.Apply:
					e.Source = listDialog.Record as GedcomSourceRecord;
					break;
				case (int)Gtk.ResponseType.Ok:
					// Create new source
					e.Source = new GedcomSourceRecord(Database);								
					break;
			}
			
			listDialog.Destroy();
		}
		
		protected void OnScrapBookButtonClicked(object sender, EventArgs e)
		{					
			if (ShowScrapBook != null)
			{
				ScrapBookArgs args = new ScrapBookArgs();
				args.Record = Record;
				ShowScrapBook(this, args);
			}
		}
		
		protected virtual void OnSourceCitationView_SelectNewNote (object sender, NoteArgs e)
		{
			if (SelectNewNote != null)
			{
				SelectNewNote(this, e);
			}
		}
		
		#endregion
		
		#region Methods
		
		public void SaveView()
		{
			SourceCitationView.SaveView();	
		}

		protected virtual void OnSourceCitationView_ShowSourceCitation (object sender, SourceCitationArgs e)
		{
			if (ShowSourceCitation != null)
			{
				ShowSourceCitation(this, e);
			}
		}

		#endregion
		
	}
}
