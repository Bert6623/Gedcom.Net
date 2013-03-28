/*
 *  $Id: AddressView.cs 199 2008-11-15 15:20:44Z davek $
 * 
 *  Copyright (C) 2008 David A Knight <david@ritter.demon.co.uk>
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
	public partial class AddressView : Gtk.Bin
	{
		
		#region Variables
				
		protected GedcomDatabase _database;
		protected GedcomRecord _record;
		
		private EventListModel _eventListModel;
		
		private GedcomEvent _resi;

		private GedcomSubmitterRecord _submitter;
		private GedcomHeader _header;
		
		#endregion
		
		#region Constructors
		
		public AddressView()
		{
			this.Build();
			
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
			Gtk.CellRenderer rend = new Gtk.CellRendererText();
			nameCol.Title = "Type";
			nameCol.PackStart(rend,true);
			nameCol.SetCellDataFunc(rend, new Gtk.TreeCellDataFunc(ListModelUtil.RenderEventName));
			
			Gtk.TreeViewColumn dateCol = new Gtk.TreeViewColumn();
			rend = new Gtk.CellRendererText();
			dateCol.Title = "Date";
			dateCol.PackStart(rend,true);
			dateCol.SetCellDataFunc(rend, new Gtk.TreeCellDataFunc(ListModelUtil.RenderEventDate));
			dateCol.SortColumnId = 0;
			
			ResiTreeView.AppendColumn(buttonCol);
			ResiTreeView.AppendColumn(nameCol);
			ResiTreeView.AppendColumn(dateCol);
			
			Gtk.TreeSelection selection = ResiTreeView.Selection;
			selection.Changed += new EventHandler(OnResiSelection_Changed);
		
			// RESI event list for addresses
			_eventListModel = new EventListModel();
			_eventListModel.FilterType = GedcomEvent.GedcomEventType.RESIFact;
			
		}
		
		#endregion
		
		#region Properties	
		
		public GedcomDatabase Database
		{
			get { return _database; }
			set
			{
				_database = value;
				_eventListModel.Database = _database;
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
				
				if (_record != null)
				{
					bool singleRec = false;
					
					switch(_record.RecordType)
					{
						case GedcomRecordType.Submitter:
							_submitter = (GedcomSubmitterRecord)_record;
							singleRec = true;
							break;
						case GedcomRecordType.Header:
							_header = (GedcomHeader)_record;
							singleRec = true;
							break;
						case GedcomRecordType.Event:
						case GedcomRecordType.FamilyEvent:
						case GedcomRecordType.IndividualEvent:
							singleRec = true;
							break;
					}
					
					if (singleRec)
					{
						if (_submitter == null && _header == null)
						{
							_resi = (GedcomEvent)_record;
						}
					
						hbox5.Visible = false;
						hseparator1.Visible = false;
						hseparator2.Visible = false;
						hbox7.Visible = false;
						
						FillView();
					}
				}
				
				if (_resi == null && _submitter == null && _header == null)
				{
					_eventListModel.Record = _record;
					ResiTreeView.Model = _eventListModel;
					
					hbox5.Visible = true;
					hseparator1.Visible = true;
					hseparator2.Visible = true;
					hbox7.Visible = true;
				}
			}
		}
			
		#endregion
		
		#region Events
		
		public event EventHandler EventRemoved;
		public event EventHandler EventAdded;
		public event EventHandler<SourceCitationArgs> ShowSourceCitation;
		public event EventHandler<ScrapBookArgs> ShowScrapBook;
		public event EventHandler<FactArgs> MoreFactInformation;
		
		#endregion
		
		#region Event Handlers
				
		protected void OnResiSelection_Changed(object sender, System.EventArgs e)
		{
			if (_submitter == null && _header == null)
			{
				Gtk.TreeSelection selection = ResiTreeView.Selection;
				
				SaveView();
				
				Gtk.TreeModel model;
				Gtk.TreeIter iter;
				
				ClearView();
				
				if (selection.GetSelected(out model, out iter))
				{
					_resi = (GedcomEvent)model.GetValue(iter, 0);
					
					FillView();
				}
			}
		}
		
		protected virtual void OnAddressSourceButton_Clicked(object sender, System.EventArgs e)
		{
			if (ShowSourceCitation != null && _resi != null)
			{
				SourceCitationArgs args = new SourceCitationArgs();
				args.Record = _resi;
				
				ShowSourceCitation(this, args);
			}
		}

		protected virtual void OnAddressMoreButton_Clicked(object sender, System.EventArgs e)
		{
			if (MoreFactInformation != null && _resi != null)
			{
				FactArgs args = new FactArgs();
				args.Event = _resi;
				
				MoreFactInformation(this, args);
			}
		}

		protected virtual void OnAddressScrapbookButton_Clicked(object sender, System.EventArgs e)
		{
			if (ShowScrapBook != null && _resi != null)
			{
				ScrapBookArgs args = new ScrapBookArgs();
				args.Record = _resi;
				
				ShowScrapBook(this, args);
			}
		}
		
		protected virtual void OnNewAddressButton_Clicked(object sender, System.EventArgs e)
		{
			Gtk.TreeIter iter;
			GedcomEvent ev;
			
			switch (_record.RecordType)
			{
				case GedcomRecordType.Individual:
					GedcomIndividualRecord indi = (GedcomIndividualRecord)_record;
			
					GedcomIndividualEvent individualEvent = new GedcomIndividualEvent();
					individualEvent.EventType = GedcomEvent.GedcomEventType.RESIFact;
					individualEvent.Database = _database;
					individualEvent.Level = indi.Level + 1;
					individualEvent.IndiRecord = indi;
					indi.Events.Add(individualEvent);	
					ev = individualEvent;
				
					break;
				case GedcomRecordType.Family:
					GedcomFamilyRecord fam = (GedcomFamilyRecord)_record;
			
					GedcomFamilyEvent famEvent = new GedcomFamilyEvent ();
					famEvent.EventType = GedcomEvent.GedcomEventType.RESI;
					famEvent.Database = _database;
					famEvent.Level = fam.Level + 1;
					famEvent.FamRecord = fam;
					fam.Events.Add(famEvent);	
					ev = famEvent;
			
					break;
				default:
					throw new Exception("Invalid record type, AddressView can only handle individuals or families");
			}
			
			iter = _eventListModel.Append();
			_eventListModel.SetValue(iter, 0, ev);
			
			Gtk.TreeSelection selection = ResiTreeView.Selection;
			selection.SelectIter(iter);
				
			if (EventAdded != null)
			{
				EventAdded(this, EventArgs.Empty);
			}
			
		}
		
		
		// GTK# being annoying this time, need this so the
		// handler is connected before the widget handles the event
		[GLib.ConnectBefore]
		protected virtual void OnResiTreeView_ButtonPressEvent (object o, Gtk.ButtonPressEventArgs e)
		{
			Gdk.EventButton ev = e.Event;
			
			if (ev.Button == 1)
			{
				int x = (int)ev.X;
				int y = (int)ev.Y;
				Gtk.TreePath path;
				
				if (ResiTreeView.GetPathAtPos(x, y, out path))
				{
					Gtk.TreeViewColumn buttonCol = ResiTreeView.Columns[0];
					if (x < buttonCol.Width)
					{
						Gtk.TreeIter iter;
						Gtk.CellRenderer[] rends = buttonCol.CellRenderers;
						
						if (_eventListModel.GetIter(out iter, path))
						{
							GedcomEvent resiEv = (GedcomEvent)_eventListModel.GetValue(iter, 0);
														
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
									// source citation column
									case 0:
										if (ShowSourceCitation != null)
										{
											SourceCitationArgs args = new SourceCitationArgs();
											args.Record = resiEv;
											ShowSourceCitation(this,args);
										}
										break;
									// remove column
									case 1:
										if (resiEv == _resi)
										{
											ClearView();
										}
										
										if (resiEv is Gedcom.GedcomIndividualEvent)
										{
											GedcomIndividualEvent indiEv = (GedcomIndividualEvent)resiEv;
											
											GedcomIndividualRecord indi = indiEv.IndiRecord;
											indi.Events.Remove(indiEv);
										}
										else if (resiEv is Gedcom.GedcomFamilyEvent)
										{
											GedcomFamilyEvent famEv = (GedcomFamilyEvent )resiEv;
											
											GedcomFamilyRecord fam = famEv.FamRecord;
											fam.Events.Remove(famEv);	
										}
										
										if (EventRemoved != null)
										{
											EventRemoved(this, EventArgs.Empty);
										}
													
										_eventListModel.Remove(ref iter);
																										
										break;
									// more info column
									case 2:
										FactArgs args = new FactArgs();
										args.Event = resiEv;
									
										SaveView();
									
										if (MoreFactInformation != null)
										{
											MoreFactInformation(this, args);	
										}
										break;
								}
							}
						}
					}
				}
			}
		}
		
		#endregion
		
		#region Methods
		
		public void Disable()
		{
			Enable(false);
		}
		public void Enable()
		{
			Enable(true);
		}
		
		private void Enable(bool enable)
		{
			AddressDateEntry.Sensitive = enable;
			AddressSourceButton.Sensitive = enable;
			AddressMoreButton.Sensitive = enable;
			AddressScrapbookButton.Sensitive = enable;
			Street1Entry.Sensitive = enable;
			Street2Entry.Sensitive = enable;
			CityEntry.Sensitive = enable;
			StateEntry.Sensitive = enable;
			PostCodeEntry.Sensitive = enable;
			CountryEntry.Sensitive = enable;
			PhoneNumberEntry.Sensitive = enable;
			PhoneNumber2Entry.Sensitive = enable;
			PhoneNumber3Entry.Sensitive = enable;
			EmailEntry.Sensitive = enable;
			Email2Entry.Sensitive = enable;
			Email3Entry.Sensitive = enable;
			WebSiteEntry.Sensitive = enable;
			WebSite2Entry.Sensitive = enable;
			WebSite3Entry.Sensitive = enable;
		}

		private void Editable(bool editable)
		{
			AddressDateEntry.IsEditable = editable;
			Street1Entry.IsEditable = editable;
			Street2Entry.IsEditable = editable;
			CityEntry.IsEditable = editable;
			StateEntry.IsEditable = editable;
			PostCodeEntry.IsEditable = editable;
			CountryEntry.IsEditable = editable;
			PhoneNumberEntry.IsEditable = editable;
			PhoneNumber2Entry.IsEditable = editable;
			PhoneNumber3Entry.IsEditable = editable;
			EmailEntry.IsEditable = editable;
			Email2Entry.IsEditable = editable;
			Email3Entry.IsEditable = editable;
			WebSiteEntry.IsEditable = editable;
			WebSite2Entry.IsEditable = editable;
			WebSite3Entry.IsEditable = editable;
		}
		
		public void ClearView()
		{
			_resi = null;
			
			AddressDateEntry.Text = string.Empty;
			Street1Entry.Text = string.Empty;
			Street2Entry.Text = string.Empty;
			CityEntry.Text = string.Empty;
			StateEntry.Text = string.Empty;
			PostCodeEntry.Text = string.Empty;
			CountryEntry.Text = string.Empty;
			PhoneNumberEntry.Text = string.Empty;
			PhoneNumber2Entry.Text = string.Empty;
			PhoneNumber3Entry.Text = string.Empty;
			EmailEntry.Text = string.Empty;
			Email2Entry.Text = string.Empty;
			Email3Entry.Text = string.Empty;
			WebSiteEntry.Text = string.Empty;
			WebSite2Entry.Text = string.Empty;
			WebSite3Entry.Text = string.Empty;
			
			Disable();
		}
		
		public void FillView()
		{			
			if (_resi != null || _submitter != null || _header != null)
			{
				Enable();
				Editable((_header == null)); // header address isn't editable

				GedcomAddress address;

				if (_submitter != null)
				{
					address = _submitter.Address;
				}
				else if (_header != null)
				{
					address = _header.CorporationAddress;
				}
				else
				{
					if (_resi.Date != null)
					{
						AddressDateEntry.Text = _resi.Date.DateString;
					}
					
					address = _resi.Address;
				}
								
				if (address != null)
				{
					if (!string.IsNullOrEmpty(address.AddressLine))
					{
						Street1Entry.Text = address.AddressLine;
					}
					else if (!string.IsNullOrEmpty(address.AddressLine1))
					{
						Street1Entry.Text = address.AddressLine1;
					}
					if (!string.IsNullOrEmpty(address.AddressLine2))
					{
						Street2Entry.Text = address.AddressLine2;
					}
					if (!string.IsNullOrEmpty(address.City))
					{
						CityEntry.Text = address.City;
					}
					if (!string.IsNullOrEmpty(address.State))
					{
						StateEntry.Text = address.State;
					}
					if (!string.IsNullOrEmpty(address.PostCode))
					{
						PostCodeEntry.Text = address.PostCode;
					}
					if (!string.IsNullOrEmpty(address.Country))
					{
						CountryEntry.Text = address.Country;
					}
					if (!string.IsNullOrEmpty(address.Phone1))
					{
						PhoneNumberEntry.Text = address.Phone1;
					}
					if (!string.IsNullOrEmpty(address.Phone2))
					{
						PhoneNumber2Entry.Text = address.Phone2;
					}
					if (!string.IsNullOrEmpty(address.Phone3))
					{
						PhoneNumber3Entry.Text = address.Phone3;
					}
					if (!string.IsNullOrEmpty(address.Email1))
					{
						EmailEntry.Text = address.Email1;
					}
					if (!string.IsNullOrEmpty(address.Email2))
					{
						Email2Entry.Text = address.Email2;
					}
					if (!string.IsNullOrEmpty(address.Email3))
					{
						Email3Entry.Text = address.Email3;
					}
					if (!string.IsNullOrEmpty(address.Www1))
					{
						WebSiteEntry.Text = address.Www1;
					}
					if (!string.IsNullOrEmpty(address.Www2))
					{
						WebSite2Entry.Text = address.Www2;
					}
					if (!string.IsNullOrEmpty(address.Www3))
					{
						WebSite3Entry.Text = address.Www3;
					}
				}
			}
		}
	
		public void SaveView()
		{
			if (_resi != null || _submitter != null || _header != null)
			{
						
				if (!string.IsNullOrEmpty(AddressDateEntry.Text) ||
				    !string.IsNullOrEmpty(Street1Entry.Text) ||
			        !string.IsNullOrEmpty(Street2Entry.Text) ||
			        !string.IsNullOrEmpty(CityEntry.Text) ||
			        !string.IsNullOrEmpty(StateEntry.Text) ||
			        !string.IsNullOrEmpty(PostCodeEntry.Text) ||
			        !string.IsNullOrEmpty(CountryEntry.Text) ||
			        !string.IsNullOrEmpty(PhoneNumberEntry.Text) ||
				    !string.IsNullOrEmpty(PhoneNumber2Entry.Text) ||
				    !string.IsNullOrEmpty(PhoneNumber3Entry.Text) ||
			        !string.IsNullOrEmpty(EmailEntry.Text) ||
				    !string.IsNullOrEmpty(Email2Entry.Text) ||
				    !string.IsNullOrEmpty(Email3Entry.Text) ||
			        !string.IsNullOrEmpty(WebSiteEntry.Text) ||
				    !string.IsNullOrEmpty(WebSite2Entry.Text) ||
				    !string.IsNullOrEmpty(WebSite3Entry.Text))
			    {
					GedcomAddress address;
					
					if (_submitter != null)
					{
						if (_submitter.Address == null)
						{
							_submitter.Address = new GedcomAddress();
							_submitter.Address.Database = Database;
						}
						address = _submitter.Address;
					}
					else if (_header != null)
					{
						if (_header.CorporationAddress == null)
						{
							_header.CorporationAddress = new GedcomAddress();
							_header.CorporationAddress.Database = Database;
						}
						address = _header.CorporationAddress;
					}
					else 
					{
						if (_resi.Address == null)
						{
							_resi.Address = new GedcomAddress();
							_resi.Address.Database = Database;
						}
						address = _resi.Address;
					}
						
					if (_resi != null)
					{
						if (!hbox5.Visible)
						{
							// no date, TODO: should it be the event date?
							
						}
						else if (string.IsNullOrEmpty(AddressDateEntry.Text))
						{
							_resi.Date = null;
						}
						else
						{
							if (_resi.Date == null)
							{
								_resi.Date = new Gedcom.GedcomDate(_database);
							}
							_resi.Date.ParseDateString(AddressDateEntry.Text);
						}
					}
					
					address.AddressLine1 = Street1Entry.Text;
			    	address.AddressLine2 = Street2Entry.Text;
					address.City = CityEntry.Text;
					address.State = StateEntry.Text;
					address.PostCode = PostCodeEntry.Text;
					address.Country = CountryEntry.Text;
					address.Phone1 = PhoneNumberEntry.Text;
					address.Phone2 = PhoneNumber2Entry.Text;
					address.Phone3 = PhoneNumber3Entry.Text;
					address.Email1 = EmailEntry.Text;
					address.Email2 = Email2Entry.Text;
					address.Email3 = Email3Entry.Text;
					address.Www1 = WebSiteEntry.Text;
					address.Www2 = WebSite2Entry.Text;
					address.Www3 = WebSite3Entry.Text;
			    }
			    else if (_resi != null)
			    {
			    	_resi.Address = null;
			    }
				else if (_submitter != null)
				{
					_submitter.Address = null;
				}
				else
				{
					_header.CorporationAddress = null;
				}
			}
		}

		
		#endregion
	}
}
