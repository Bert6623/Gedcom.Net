/*
 *  $Id: SourceView.cs 189 2008-10-10 14:16:10Z davek $
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
	public partial class SourceView : Gtk.Bin, IGedcomView
	{
			
		#region Variables
				
		protected GedcomDatabase _database;
		protected GedcomRecord _record;
				
		protected EventModel _eventModel;
		protected GedcomRecordedEvent _recordedEvent;
				
		protected GenericListModel<GedcomRecordedEvent> _eventGroups;				
				
		#endregion
				
		#region Constructors
		
		public SourceView()
		{
			this.Build();
			
			_eventModel = new EventModel(EventModel.EventModelType.All);
			Gtk.TreeViewColumn eventNameCol = new Gtk.TreeViewColumn();
			Gtk.CellRenderer rend = new Gtk.CellRendererToggle();
			eventNameCol.Title = "Event";
			eventNameCol.PackStart(rend, false);
			eventNameCol.AddAttribute(rend, "active", (int)EventModel.Columns.Included);
			((Gtk.CellRendererToggle)rend).Activatable = true;
			((Gtk.CellRendererToggle)rend).Toggled += new Gtk.ToggledHandler(OnIncluded_Toggle);
			rend = new Gtk.CellRendererText();
			eventNameCol.PackStart(rend, true);
			eventNameCol.AddAttribute(rend, "text", (int)EventModel.Columns.Readable);

			EventTypeTreeView.AppendColumn(eventNameCol);		
			EventTypeTreeView.Model = _eventModel;

			
			Gtk.TreeViewColumn buttonCol = new Gtk.TreeViewColumn();
			GtkCellRendererButton butRend = new GtkCellRendererButton();
			butRend.StockId = "gtk-remove";
			buttonCol.PackStart(butRend,true);
			
			Gtk.TreeViewColumn noteCountCol = new Gtk.TreeViewColumn();
			rend = new Gtk.CellRendererText();
			noteCountCol.Title = "No.";
			noteCountCol.PackStart(rend,true);
			noteCountCol.SetCellDataFunc(rend, new Gtk.TreeCellDataFunc(ListModelUtil.RenderEventRecordedCount));
		
			EventGroupTreeView.AppendColumn(buttonCol);
			EventGroupTreeView.AppendColumn(noteCountCol);

			Gtk.TreeSelection selection = EventGroupTreeView.Selection;
			selection.Changed += new EventHandler(OnEventGroupSelection_Changed);
			
			_eventGroups = new GenericListModel<GedcomRecordedEvent>();
			EventGroupTreeView.Model = _eventGroups.Adapter;
			
			Gtk.TreeViewColumn callNumberCol = new Gtk.TreeViewColumn();
			rend = new Gtk.CellRendererText();
			callNumberCol.Title = "Call Number";
			callNumberCol.PackStart(rend,true);
			callNumberCol.AddAttribute(rend, "text", 0);
			
			Gtk.TreeViewColumn mediaTypeCol = new Gtk.TreeViewColumn();
			rend = new Gtk.CellRendererText();
			mediaTypeCol.Title = "Media Type";
			mediaTypeCol.PackStart(rend,true);
			mediaTypeCol.AddAttribute(rend, "text", 1);
			
			CallNumberTreeView.AppendColumn(callNumberCol);
			CallNumberTreeView.AppendColumn(mediaTypeCol);
			
			RepositoryCitationListModel repoCitationListModel = new RepositoryCitationListModel();
			CallNumberTreeView.Model = repoCitationListModel;
			
			selection = CallNumberTreeView.Selection;
			selection.Changed += new EventHandler(OnCallNumberSelection_Changed);
					
			// How to handle SourceMediaType.Other ?
			// don't include in initial and if the select one is SourceMediaType.Other
			// add an item into the dropdown for its value?
			// as other isn't really valid this seems like a reasonable idea
			Gtk.ListStore mediaTypes = new Gtk.ListStore(typeof(string));
			foreach (SourceMediaType mediaType in Enum.GetValues(typeof(SourceMediaType)))
			{
				if (mediaType != SourceMediaType.Other)
				{
					Gtk.TreeIter iter = mediaTypes.Append();
					mediaTypes.SetValue(iter, 0, mediaType.ToString());
				}
			}
			rend = new Gtk.CellRendererText();
			MediaTypeCombo.PackStart(rend,true);
			MediaTypeCombo.AddAttribute(rend, "text", 0);
			MediaTypeCombo.Model = mediaTypes;
			
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
				DataNotesView.Database = value;
				RepoNotesView.Database = value;
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
				
				Sensitive = (_record != null);
				
				if (_record.RecordType != GedcomRecordType.Source)
				{
					throw new Exception("Can only set a GedcomSourceRecord");	
				}
						
				ClearView();
				
				FillView();
			}
		}
		
		public GedcomIndividualRecord Husband
		{
			get { return null; }	
		}
		
		public GedcomIndividualRecord Wife
		{
			get { return null; }	
		}
		
		public GedcomRecordedEvent RecordedEvent
		{
			get { return _recordedEvent; }
			set 
			{ 
				_recordedEvent = value;
				
				if (_recordedEvent != null)
				{
					_eventModel.IncludedEvents = _recordedEvent.Types;
					if (_recordedEvent.Date != null)
					{
						DateRecordedEntry.Text = _recordedEvent.Date.DateString;
					}
					else
					{
						DateRecordedEntry.Text = string.Empty;
					}
					if (_recordedEvent.Place != null)
					{
						PlaceRecordedEntry.Text = _recordedEvent.Place.Name;
					}
					else
					{
						PlaceRecordedEntry.Text = string.Empty;
					}
					
					EnableEventView(true);
				}
				else
				{
					EnableEventView(false);
				}
			}
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
		
		#region EventHandlers
		
		protected virtual void OnScrapbookButton_Clicked(object sender, System.EventArgs e)
		{
			SaveView();
			
			if (_record != null)
			{
				if (ShowScrapBook != null)
				{
					ScrapBookArgs args = new ScrapBookArgs();
					args.Record = _record;
					ShowScrapBook(this, args);
				}
			}
		}
		
		protected void OnEventGroupSelection_Changed(object sender, System.EventArgs e)
		{
			Gtk.TreeSelection selection = EventGroupTreeView.Selection;
			
			if (_recordedEvent != null)
			{
				SaveView();
			}
			
			Gtk.TreeModel model;
			Gtk.TreeIter iter;
			
			GedcomRecordedEvent recordedEvent = null;
			
			if (selection.GetSelected(out model, out iter))
			{
				recordedEvent = (GedcomRecordedEvent)model.GetValue(iter, 0);
			}
						
			RecordedEvent = recordedEvent;
		}
		
		protected virtual void OnNewEventGroupButton_Clicked(object sender, System.EventArgs e)
		{
			GedcomRecordedEvent recordedEvent = new GedcomRecordedEvent();
			recordedEvent.Database = Database;
			
			GedcomSourceRecord source = (GedcomSourceRecord)_record;
			source.EventsRecorded.Add(recordedEvent);
			
			_eventGroups.ItemInserted();
		}
		
		// GTK# being annoying this time, need this so the
		// handler is connected before the widget handles the event
		[GLib.ConnectBefore]
		protected virtual void OnEventGroupTreeView_ButtonPressEvent(object sender, Gtk.ButtonPressEventArgs e)
		{
			Gdk.EventButton ev = e.Event;
			
			if (ev.Button == 1)
			{
				int x = (int)ev.X;
				int y = (int)ev.Y;
				Gtk.TreePath path;
								
				if (EventGroupTreeView.GetPathAtPos(x, y, out path))
				{
					Gtk.TreeViewColumn buttonCol = EventGroupTreeView.Columns[0];
					if (x < buttonCol.Width)
					{
						Gtk.TreeIter iter;
						
						if (_eventModel.GetIter(out iter, path))
						{
							GedcomRecordedEvent recordedEvent = (GedcomRecordedEvent)_eventModel.GetValue(iter, 0);
							
							GedcomSourceRecord source = (GedcomSourceRecord)_record;
							source.EventsRecorded.Remove(recordedEvent);
							
							_eventModel.Remove(ref iter);
						}
					}
				}
			}
		}
		
		protected void OnCallNumberSelection_Changed(object sender, System.EventArgs e)
		{
			Gtk.TreeSelection selection = CallNumberTreeView.Selection;
			
			Gtk.TreeModel model;
			Gtk.TreeIter iter;
			if (selection.GetSelected(out model, out iter))
			{
				CallNumberEntry.Text = (string)model.GetValue(iter, 0);
				string mediaType = (string)model.GetValue(iter, 1);
				
				int i = 0;
				foreach (SourceMediaType sourceMediaType in Enum.GetValues(typeof(SourceMediaType)))
				{
					if (sourceMediaType.ToString() == mediaType)
					{
						break;	
					}
					i ++;
				}
				if (i > (int)SourceMediaType.Other)
				{
					i = (int)SourceMediaType.Other;	
				}
				
				if (i == (int)SourceMediaType.Other)
				{
					Gtk.ListStore list = (Gtk.ListStore)MediaTypeCombo.Model;
					
					// remove existing other item
					if (list.IterNthChild(out iter, i))
					{
						list.Remove(ref iter);
					}
					
					// add new other item
					iter = list.Append();
					list.SetValue(iter, 0, mediaType);					
				}
				
				MediaTypeCombo.Active = i;
			}
		}

		protected virtual void OnNotebook_SwitchPage (object o, Gtk.SwitchPageArgs args)
		{
			SaveView();
		}

		protected void OnIncluded_Toggle(object sender, Gtk.ToggledArgs e)
		{
			Gtk.TreeIter iter;
			
			if (_eventModel.GetIter(out iter, new Gtk.TreePath(e.Path)))
			{
				bool included = (bool)_eventModel.GetValue(iter, (int)EventModel.Columns.Included);
				_eventModel.SetValue(iter, (int)EventModel.Columns.Included, !included);
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

		private void EnableEventView(bool enable)
		{
			DateRecordedEntry.Sensitive = enable;
			PlaceRecordedEntry.Sensitive = enable;
			EventTypeTreeView.Sensitive = enable;
		}
		
		public void ClearView()
		{
			TitleEntry.Text = string.Empty;
			OriginatorEntry.Text = string.Empty;
			FiledByEntry.Text = string.Empty;
			AgencyTextBox.Text = string.Empty;
			PublicationFactsTextView.Buffer.Clear();
			TextTextView.Buffer.Clear();
			
			DateRecordedEntry.Text = string.Empty;
			PlaceRecordedEntry.Text = string.Empty;
			
			NotesView.Clear();
			DataNotesView.Clear();
			RepoNotesView.Clear();
						
			CallNumberEntry.Text = string.Empty;
			MediaTypeCombo.Active = 0;
			
			EnableEventView(false);
		}
					
		public void FillView()
		{
			GedcomSourceRecord source = _record as GedcomSourceRecord;
				
			NotesView.Record = source;
			DataNotesView.Record = source;
			RepositoryCitationListModel repoCitationListModel = new RepositoryCitationListModel();
			repoCitationListModel.Database = _database;
			CallNumberTreeView.Model = repoCitationListModel;
						
			if (source != null)
			{
				if (!string.IsNullOrEmpty(source.Title))
				{
					TitleEntry.Text = source.Title;
				}
				if (!string.IsNullOrEmpty(source.Originator))
				{
					OriginatorEntry.Text = source.Originator;
				}
				if (!string.IsNullOrEmpty(source.FiledBy))
				{
					FiledByEntry.Text = source.FiledBy;
				}
				if (!string.IsNullOrEmpty(source.Agency))
				{
					AgencyTextBox.Text = source.Agency;
				}
				if (!string.IsNullOrEmpty(source.PublicationFacts))
				{
					PublicationFactsTextView.Buffer.Text = source.PublicationFacts;
				}
							
				if (!string.IsNullOrEmpty(source.Text))
				{
					TextTextView.Buffer.Text = source.Text;	
				}
				
				if (source.RepositoryCitations.Count > 0)
				{
					repoCitationListModel.Record = source.RepositoryCitations[0];
					
					RepoNotesView.Record = source.RepositoryCitations[0];
				}
				
				if (_recordedEvent != null)
				{
					_eventModel.IncludedEvents = _recordedEvent.Types;
				}
				else
				{
					_eventModel.IncludedEvents = null;
				}
				
				_eventGroups.List = source.EventsRecorded;
			}
			else
			{
				_eventModel.IncludedEvents = null;
				_eventGroups.Clear();
			}
		}
					
		public void SaveView()
		{
			GedcomSourceRecord source = _record as GedcomSourceRecord;
			
			if (source != null)
			{
				source.Title = TitleEntry.Text;
				source.Originator = OriginatorEntry.Text;
				source.FiledBy = FiledByEntry.Text;
				source.Agency = AgencyTextBox.Text;
				source.PublicationFacts = PublicationFactsTextView.Buffer.Text;
				
				if (_recordedEvent != null)
				{
					_recordedEvent.Types = _eventModel.IncludedEvents;
				
					if (!string.IsNullOrEmpty(DateRecordedEntry.Text))
					{
						if (_recordedEvent.Date == null)
						{
							GedcomDate date = new GedcomDate(_database);
							date.Level = _record.Level + 3; // SOUR / DATA / EVEN / DATE
							_recordedEvent.Date = date;
						}
						
						_recordedEvent.Date.ParseDateString(DateRecordedEntry.Text);
					}
					else
					{
						_recordedEvent.Date = null;
					}
					
					if (!string.IsNullOrEmpty(PlaceRecordedEntry.Text))
					{
						if (_recordedEvent.Place == null)
						{
							GedcomPlace place = new GedcomPlace();
							place.Database = _database;
							place.Level = _record.Level + 3; // SOUR / DATA / EVEN / PLAC
							_recordedEvent.Place = place;
						}
						_recordedEvent.Place.Name = PlaceRecordedEntry.Text;
					}
					else
					{
						_recordedEvent.Place = null;
					}
				}
				
				source.Text = TextTextView.Buffer.Text;
				
				NotesView.Save();
				
				// FIXME: repository citations
				
				RepoNotesView.Save();
			}
		}

		#endregion
	}
}
