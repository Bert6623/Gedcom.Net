/*
 *  $Id: SourceCitationView.cs 189 2008-10-10 14:16:10Z davek $
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
using Gedcom.UI.Common;

namespace Gedcom.UI.GTK.Widgets
{
	
	[System.ComponentModel.Category("Gedcom.UI.GTK.Widgets")]
	[System.ComponentModel.ToolboxItem(true)]
	public partial class SourceCitationView : Gtk.Bin
	{
				
		#region Variables
				
		protected GedcomDatabase _database;
		protected GedcomRecord _record;
		
		protected GedcomSourceRecord _masterSource;
		
		#endregion
				
		#region Constructors
				
		public SourceCitationView()
		{
			this.Build();
			
			Gtk.ListStore certaintyTypes = new Gtk.ListStore(typeof(string));
			foreach (GedcomCertainty certainty in Enum.GetValues(typeof(GedcomCertainty)))
			{
				Gtk.TreeIter iter = certaintyTypes.Append();
				certaintyTypes.SetValue(iter, 0, certainty.ToString());
			}
			Gtk.CellRenderer rend = new Gtk.CellRendererText();
			CertaintyComboBox.PackStart(rend,true);
			CertaintyComboBox.AddAttribute(rend, "text", 0);
			CertaintyComboBox.Model = certaintyTypes;
			
			Notebook.Page = 0;
		}
		
		#endregion
		
		#region Properties
		
		public GedcomDatabase Database
		{
			get { return _database; }
			set
			{
				_database = value;
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
								
				ClearView();
				
				if (_record == null)
				{
					return;	
				}
				
				if (_record.RecordType != GedcomRecordType.SourceCitation)
				{
					throw new Exception("Can only set a GedcomSourceCitation");	
				}
			
				FillView();
			}
		}
		
		public GedcomSourceRecord MasterSource
		{
			get { return _masterSource; }	
		}
		
		#endregion
		
		#region Events
		
		public event EventHandler ViewMasterSource;
		public event EventHandler<SourceArgs> SelectMasterSource;
		public event EventHandler ScrapBookButtonClicked;
		public event EventHandler<SourceCitationArgs> ShowSourceCitation;
		public event EventHandler<NoteArgs> SelectNewNote;
		
		#endregion
		
		#region EventHandlers
				
		protected virtual void OnViewMasterButton_Clicked(object sender, System.EventArgs e)
		{
			if (ViewMasterSource != null && _masterSource != null)
			{
				ViewMasterSource(this,EventArgs.Empty);
			}
		}
		
		protected virtual void OnSelectMasterSourceButton_Click(object sender, System.EventArgs e)
		{
			if (SelectMasterSource != null)
			{
				SourceArgs args = new SourceArgs();
				SelectMasterSource(this, args);
				if (args.Source != null)
				{
					if (_masterSource != null)
					{
						_masterSource.Citations.Remove((GedcomSourceCitation)_record);
					}
					_masterSource = args.Source;
					_masterSource.Citations.Add((GedcomSourceCitation)_record);
					MasterSourceEntry.Text = _masterSource.Title;
				}
			}
		}
		
		
		protected virtual void OnScrapBookButton_Clicked(object sender, System.EventArgs e)
		{
			SaveView();	
			
			if (ScrapBookButtonClicked != null)
			{
				ScrapBookButtonClicked(this, EventArgs.Empty);
			}
		}
		
		protected virtual void OnNotesView_ShowSourceCitation(object sender, SourceCitationArgs e)
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
		
		public void ClearView()
		{
			MasterSourceEntry.Text = string.Empty;
			PageEntry.Text = string.Empty;
			EventTypeEntry.Text = string.Empty;
			RoleEntry.Text = string.Empty;
			DateEntry.Text = string.Empty;
			CertaintyComboBox.Active = 0;
			TextTextView.Buffer.Clear();
			
			NotesView.Clear();
		}

		protected void FillView()
		{
			GedcomSourceCitation citation = _record as GedcomSourceCitation;
				
			NotesView.Record = citation;
							
			if (!string.IsNullOrEmpty(citation.Source))
			{
				GedcomSourceRecord source = _database[citation.Source] as GedcomSourceRecord;
				_masterSource = source;
				if (source != null)
				{
					MasterSourceEntry.Text = source.Title;
				}
				else
				{
					MasterSourceEntry.Text = "<source missing>";
				}
			}
			
			if (!string.IsNullOrEmpty(citation.Page))
			{
				PageEntry.Text = citation.Page;
			}

			if (!string.IsNullOrEmpty(citation.EventType))
			{
				EventTypeEntry.Text = citation.EventType;
			}
			
			if (!string.IsNullOrEmpty(citation.Role))
			{
				RoleEntry.Text = citation.Role;
			}
			
			if (citation.Date != null)
			{
				DateEntry.Text = citation.Date.DateString;
			}
			
			CertaintyComboBox.Active = (int)citation.Certainty;
			
			if (!string.IsNullOrEmpty(citation.Text))
			{
				TextTextView.Buffer.Text = citation.Text;	
			}
		}
					
		public void SaveView()
		{
			GedcomSourceCitation citation = _record as GedcomSourceCitation;
			
			if (citation != null)
			{		
				if (_masterSource != null)
				{
					citation.Source = _masterSource.XRefID;
				}
				
				citation.Page = PageEntry.Text;
				citation.Role = RoleEntry.Text;
				if (!string.IsNullOrEmpty(DateEntry.Text))
				{
					if (citation.Date == null)
					{
						citation.Date = new GedcomDate(_database);
						citation.Date.Level = citation.Level + 1;
					}
					citation.Date.ParseDateString(DateEntry.Text);				
				}
				else
				{
					citation.Date = null;	
				}
					
				citation.Certainty = (GedcomCertainty)CertaintyComboBox.Active;
				
				citation.Text = TextTextView.Buffer.Text;
				
				NotesView.Save();
			}
		}

		#endregion
	}
}
