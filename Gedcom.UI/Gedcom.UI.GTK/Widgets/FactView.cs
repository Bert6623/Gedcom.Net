/*
 *  $Id: FactView.cs 196 2008-11-12 22:55:27Z davek $
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
using Gedcom.UI.Common;

namespace Gedcom.UI.GTK.Widgets
{
	
	[System.ComponentModel.Category("Gedcom.UI.GTK.Widgets")]
	[System.ComponentModel.ToolboxItem(true)]
	public partial class FactView : Gtk.Bin
	{
		#region Enums
		
		public enum FactViewType
		{
			Individual = 0,
			Family
		};
		
		#endregion
		
		#region Variables
		
		protected GedcomDatabase _database;
		protected GedcomRecord _record;
		
		protected GedcomEvent _event;
		
		protected bool _loading;
		
		private FactArgs _moreInfoArgs;
		
		private EventListModel _eventListModel;
		
		#endregion
		
		#region Constructors
		
		public FactView()
		{
			this.Build();
					
			FactTypeCombo.Clear();
			
			Gtk.CellRendererText rend = new Gtk.CellRendererText();
			rend.Ellipsize = Pango.EllipsizeMode.End;
			FactTypeCombo.PackStart(rend, true);
			FactTypeCombo.SetAttributes(rend, "text", EventModel.Columns.Readable);
									
			Gtk.TreeViewColumn buttonCol = new Gtk.TreeViewColumn();
			GtkCellRendererButton butRend = new GtkCellRendererButton();
			butRend.StockId = "gtk-index";
			buttonCol.PackStart(butRend,true);
			
			butRend = new GtkCellRendererButton();
			butRend.StockId = "gtk-remove";
			buttonCol.PackStart(butRend,true);
			
			butRend = new GtkCellRendererButton();
			butRend.StockId = "gtk-edit";
			buttonCol.PackStart(butRend,true);
			
			Gtk.TreeViewColumn nameCol = new Gtk.TreeViewColumn();
			rend = new Gtk.CellRendererText();
			nameCol.Title = "Type";
			nameCol.PackStart(rend,true);
			nameCol.SetCellDataFunc(rend, new Gtk.TreeCellDataFunc(ListModelUtil.RenderEventName));
			
			Gtk.TreeViewColumn dateCol = new Gtk.TreeViewColumn();
			rend = new Gtk.CellRendererText();
			dateCol.Title = "Date";
			dateCol.PackStart(rend,true);
			dateCol.SetCellDataFunc(rend, new Gtk.TreeCellDataFunc(ListModelUtil.RenderEventDate));
			dateCol.SortColumnId = 0;
			
			FactsTreeView.AppendColumn(buttonCol);
			FactsTreeView.AppendColumn(nameCol);
			FactsTreeView.AppendColumn(dateCol);
			
			_eventListModel = new EventListModel();
			// we need more complicated filtering than just the event type so
			// hookup to the event
			_eventListModel.FilterGedcomEvent += new EventHandler<EventListModel.FilterArgs>(FilterEvents);
			
			FactsTreeView.Model = _eventListModel;
			
			Gtk.TreeSelection selection = FactsTreeView.Selection;
			selection.Mode = Gtk.SelectionMode.Browse;
			selection.Changed += new EventHandler(OnFactSelection_Changed);
			
			_moreInfoArgs = new FactArgs();
		}
		
		#endregion
		
		#region Properties
		
		public GedcomDatabase Database
		{
			get { return _database; }
			set { _database = value; }
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
				
				_loading = true;
								
				if (_record.RecordType == GedcomRecordType.Individual)
				{
					_eventListModel.FilterType =  GedcomEvent.GedcomEventType.RESIFact;
					SetFactCombo(FactViewType.Individual);
					
					GedcomIndividualRecord indi = (GedcomIndividualRecord)_record;
					Enable((indi.Events.Count > 0));
				}
				else
				{
					_eventListModel.FilterType =  GedcomEvent.GedcomEventType.RESI;
					SetFactCombo(FactViewType.Family);
					
					GedcomFamilyRecord fam = (GedcomFamilyRecord)_record;
					Enable((fam.Events.Count > 0));
				}
								
				_eventListModel.Database = _database;
				_eventListModel.Record = _record;
				FactsTreeView.Model = _eventListModel;
				
				if (FactTypeCombo.Sensitive)
				{
					Gtk.TreeSelection selection = FactsTreeView.Selection;
					selection.SelectPath(Gtk.TreePath.NewFirst());
				}
				
				_loading = false;
			}
		}
		
		public GedcomEvent Event
		{
			get { return _event; }		
		}
		
		#endregion
		
		#region Events
		
		public event EventHandler EventRemoved;
		public event EventHandler EventAdded;
		public event EventHandler<SourceCitationArgs> ShowSourceCitation;
		public event EventHandler<ScrapBookArgs> ShowScrapBook;
		public event EventHandler<FactArgs> MoreInformation;
		
		#endregion
		
		#region EventHandlers
		
		protected void OnFactSelection_Changed(object sender, System.EventArgs e)
		{
			SetEventFromSelected();
		}
		
		protected virtual void OnFactSourceButton_Clicked(object sender, System.EventArgs e)
		{
			SourceCitationArgs args = new SourceCitationArgs();
			args.Record = _event;
			ShowSourceCitation(this,args);
		}
		
		protected virtual void OnEventScrapbookButton_Clicked(object sender, System.EventArgs e)
		{
			if (ShowScrapBook != null)
			{
				ScrapBookArgs args = new ScrapBookArgs();
				args.Record = _event;
				ShowScrapBook(this, args);
			}
		}	
		
		protected virtual void OnNewFactButton_Clicked(object sender, System.EventArgs e)
		{
			Save();
			
			Gtk.TreeIter iter;
			
			GedcomIndividualRecord indi = _record as GedcomIndividualRecord;
			GedcomFamilyRecord fam = _record as GedcomFamilyRecord;

			GedcomEvent ev = null;
			
			if (indi != null)
			{
				GedcomIndividualEvent individualEvent = new GedcomIndividualEvent();
				individualEvent.EventType = GedcomEvent.GedcomEventType.GenericFact;
				individualEvent.Database = _database;
				individualEvent.IndiRecord = indi;
												
				ev = individualEvent;
				indi.Attributes.Add(individualEvent);
			}
			else if (fam != null)
			{
				GedcomFamilyEvent famEvent = new GedcomFamilyEvent();
				famEvent.EventType = GedcomEvent.GedcomEventType.GenericEvent;
				famEvent.Database = _database;
				famEvent.FamRecord = fam;
								
				ev = famEvent;
				fam.Events.Add(famEvent);
			}
			
			iter = _eventListModel.Append();
			_eventListModel.SetValue(iter, 0, ev);
			
			_event = null;
			Gtk.TreeSelection selection = FactsTreeView.Selection;
			selection.SelectIter(iter);
			
			if (EventAdded != null)
			{
				EventAdded(this, EventArgs.Empty);	
			}
		}
		
		// GTK# being annoying this time, need this so the
		// handler is connected before the widget handles the event
		[GLib.ConnectBefore]
		protected virtual void OnFactsTreeView_ButtonPressEvent(object sender, Gtk.ButtonPressEventArgs e)
		{
			Gdk.EventButton eb = e.Event;
			
			if (eb.Button == 1)
			{
				int x = (int)eb.X;
				int y = (int)eb.Y;
				Gtk.TreePath path;
								
				if (FactsTreeView.GetPathAtPos(x, y, out path))
				{
					Gtk.TreeViewColumn buttonCol = FactsTreeView.Columns[0];
					if (x < buttonCol.Width)
					{
						Gtk.TreeIter iter;
						Gtk.CellRenderer[] rends = buttonCol.CellRenderers;
						
						EventListModel events = FactsTreeView.Model as EventListModel;
						
						if (events.GetIter(out iter, path))
						{
							GedcomEvent ev = _event;
							
							_event = events.GetValue(iter, 0) as GedcomEvent;
							
							int i = 0;
							bool buttonClicked = false;
							foreach (GtkCellRendererButton rend in rends)
							{
								if (x >= rend.X && x <= rend.X + rend.Width)
								{
									buttonClicked = true;
									break;
								}
								i ++;
							}
							if (buttonClicked)
							{
								switch (i)
								{
									// source button
									case 0:
										if (ShowSourceCitation != null)
										{
											SourceCitationArgs args = new SourceCitationArgs();
											args.Record = _event;
											ShowSourceCitation(this,args);
										}
										break;
									// remove button
									case 1:
										GedcomIndividualRecord indi = _record as GedcomIndividualRecord;
										GedcomFamilyRecord fam = _record as GedcomFamilyRecord;
									
										if (EventRemoved != null)
										{
											EventRemoved(this, EventArgs.Empty);
										}
													
										events.Remove(ref iter);
							
										if (indi != null)
										{
											if (indi.Events.Contains((GedcomIndividualEvent)_event))
											{
												indi.Events.Remove((GedcomIndividualEvent)_event);
											}
											else if (indi.Attributes.Contains((GedcomIndividualEvent)_event))
											{
												indi.Attributes.Remove((GedcomIndividualEvent)_event);
											}
										}
										else if (fam != null)
										{
											fam.Events.Remove((GedcomFamilyEvent)_event);	
										}
										
										if (ev == _event)
										{
											ev = null;	
										}
										
										break;
									// more button
									case 2:
										if (_event == ev)
										{
											Save();
										}
																				
										if (MoreInformation != null)
										{
											_moreInfoArgs.Event = _event;
											_event = ev;
											MoreInformation(this, _moreInfoArgs);
										}
										break;
								}
							}
							SetEventFromSelected();
						}
					}
				}
			}
		}
				
		protected virtual void OnFactMoreButton_Clicked (object sender, System.EventArgs e)
		{
			Save();
			
			if (MoreInformation != null)
			{
				_moreInfoArgs.Event = _event;
				MoreInformation(this, _moreInfoArgs);
			}
		}

		private void FilterEvents(object sender, EventListModel.FilterArgs e)
		{
			bool include = true;
			switch (e.Event.EventType) 
			{
				case GedcomEvent.GedcomEventType.RESIFact:
					include = false;
					break;
				case GedcomEvent.GedcomEventType.GenericFact:
					// don't include height fact
					if (string.Compare(e.Event.EventName, "Height") == 0) 
					{
						include = false;
					}
					// or weight fact
					else if (string.Compare(e.Event.EventName, "Weight") == 0) 
					{
						include = false;
					}
					// or medical
					else if (string.Compare(e.Event.EventName, "Medical") == 0) 
					{
						include = false;
					}
					break;
			}

			e.Include = include;
		}
		
		#endregion
		
		#region Methods
		
		private void Disable()
		{
			Enable(false);
		}
		
		private void Enable()
		{
			Enable(true);
		}
		
		private void Enable(bool enable)
		{
			FactTypeCombo.Sensitive = enable;
			FactSourceButton.Sensitive = enable;
			EventScrapbookButton.Sensitive = enable;
			FactDateEntry.Sensitive = enable;
			CommentTextView.Sensitive = enable;
			FactMoreButton.Sensitive = enable;
		}
		
		private void ClearDetails()
		{
			FactTypeCombo.Active = -1;
			FactNameEntry.Text = string.Empty;
			FactDateEntry.Text = string.Empty;
			CommentTextView.Buffer.Clear();
		}
		
		public void Clear()
		{
			_event = null;
		
			ClearDetails();
		
			EventListModel eventListModel = FactsTreeView.Model as EventListModel;
			if (eventListModel != null)
			{
				eventListModel.Clear();	
			}
		}

		private void SetEventFromSelected()
		{
			Gtk.TreeSelection selection = FactsTreeView.Selection;
			
			if (_event != null && !_loading)
			{
				Save();
			}
			
			ClearDetails();
			
			Gtk.TreeModel model;
			Gtk.TreeIter iter;
			if (selection.GetSelected(out model, out iter))
			{
				GedcomEvent ev = (GedcomEvent)model.GetValue(iter, 0);
				SetEventView(ev);
				Enable();
			}
			else
			{
				_event = null;
				Disable();
			}
		}
		
		protected void SetEventView(GedcomEvent e)
		{
			Gtk.ListStore eventTypes = FactTypeCombo.Model as Gtk.ListStore;
			
			Gtk.TreeIter iter;
			
			if (eventTypes.GetIterFirst(out iter))
			{
				int i = 0;
				do
				{
					GedcomEvent.GedcomEventType type = (GedcomEvent.GedcomEventType)eventTypes.GetValue(iter, 2);
					if (type == e.EventType)
					{
						FactTypeCombo.Active = i;
						break;	
					}
					i ++;
				}
				while (eventTypes.IterNext(ref iter));
			}
					
			if (!string.IsNullOrEmpty(e.EventName))
			{
				FactNameEntry.Text = e.EventName;
			}
			
			if (e.Date != null)
			{
				FactDateEntry.Text = e.Date.DateString;	
			}
			
			GedcomPlace place = e.Place;
			if (place != null)
			{
				CommentTextView.Buffer.Text = place.Name;	
			}
			
			_event = e;
		}
		
		public void Save()
		{
			if ((!_loading) && (_record != null))
			{
				
				if (_event != null)
				{
					GedcomEvent ev = _event;

					GedcomEvent.GedcomEventType newType = ev.EventType;
					
					if (FactTypeCombo.Active != -1)
					{
						Gtk.TreeIter iter;
						Gtk.ListStore eventTypes = FactTypeCombo.Model as Gtk.ListStore;
						if (eventTypes.GetIterFromString(out iter, FactTypeCombo.Active.ToString()))
						{
							newType = (GedcomEvent.GedcomEventType)eventTypes.GetValue(iter,2);							
						}
					}

					// need to remove event from indi.Events/indi.Attributes
					// and add to the correct one if the event type has changed
					// (and we are dealing with an individual rather than a family
					if (_record.RecordType == GedcomRecordType.Individual && newType != ev.EventType)
					{
						GedcomIndividualEvent indiEv = (GedcomIndividualEvent)ev;
						GedcomIndividualRecord indi = (GedcomIndividualRecord)_record;
						if (indi.Events.Contains(indiEv))
						{
							indi.Events.Remove(indiEv);
						}
						else if (indi.Attributes.Contains(indiEv))
						{
							indi.Attributes.Remove(indiEv);
						}
						if (newType == Gedcom.GedcomEvent.GedcomEventType.GenericEvent ||
						    (newType >= Gedcom.GedcomEvent.GedcomEventType.BIRT &&
						    newType <= Gedcom.GedcomEvent.GedcomEventType.RETI))
						{
							indi.Events.Add(indiEv);
						}
						else
						{
							indi.Attributes.Add(indiEv);
						}
					}
					
					ev.EventName = FactNameEntry.Text;
					
					if (ev.Date == null)
					{
						ev.Date = new GedcomDate(_database);
						ev.Date.Level = ev.Level + 1; 
					}
					ev.Date.ParseDateString(FactDateEntry.Text);
					
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
						
				}
			}
			
		}
		
		private void SetFactCombo(FactViewType viewType)
		{
			EventModel eventTypes = null;
			
			if (viewType == FactViewType.Individual)
			{
				// don't show RESI facts, they are handled separately
				eventTypes = new EventModel(EventModel.EventModelType.Individual, GedcomEvent.GedcomEventType.RESIFact, true);
			}
			else
			{
				// don't show RESI facts, they are handled separately
				eventTypes = new EventModel(EventModel.EventModelType.Family, GedcomEvent.GedcomEventType.RESI, true);
			}
			
			FactTypeCombo.Model = eventTypes;
			
		}

		#endregion
		
	}
}
