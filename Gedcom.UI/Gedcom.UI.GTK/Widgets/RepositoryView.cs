/*
 *  $Id: RepositoryView.cs 189 2008-10-10 14:16:10Z davek $
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
	public partial class RepositoryView : Gtk.Bin, IGedcomView
	{
		#region Variables
				
		protected GedcomDatabase _database;
		protected GedcomRecord _record;
		
		#endregion
				
		#region Constructors
				
		public RepositoryView()
		{
			this.Build();
			
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
	
				Sensitive = (_record != null);
	
				if (_record.RecordType != GedcomRecordType.Repository)
				{
					throw new Exception("Can only set a GedcomRepositoryRecord");	
				}
						
				ClearView();
				
				NotesView.Record = value;
				
				GedcomRepositoryRecord repo  = _record as GedcomRepositoryRecord;
								
				if (!string.IsNullOrEmpty(repo.Name))
				{
					NameEntry.Text = repo.Name;	
				}
				
				if (repo.Address != null)
				{
					GedcomAddress address = repo.Address;
					
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
					if (!string.IsNullOrEmpty(address.Email1))
					{
							EmailEntry.Text = address.Email1;
					}
					if (!string.IsNullOrEmpty(address.Www1))
					{
						WebSiteEntry.Text = address.Www1;
					}
						
				}
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

		protected virtual void OnNotebook_SwitchPage (object o, Gtk.SwitchPageArgs args)
		{
			SaveView();
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
		
		public void ClearView()
		{
			NameEntry.Text = string.Empty;
			
			Street1Entry.Text = string.Empty;
			Street2Entry.Text = string.Empty;
			CityEntry.Text = string.Empty;
			StateEntry.Text = string.Empty;
			PostCodeEntry.Text = string.Empty;
			CountryEntry.Text = string.Empty;
			PhoneNumberEntry.Text = string.Empty;
			EmailEntry.Text = string.Empty;
			WebSiteEntry.Text = string.Empty;
			
			NotesView.Clear();
		}
		
		public void SaveView()
		{
			GedcomRepositoryRecord repo  = _record as GedcomRepositoryRecord;
			
			if (repo != null)
			{
				repo.Name = NameEntry.Text;
				
				if (!string.IsNullOrEmpty(Street1Entry.Text) ||
			       !string.IsNullOrEmpty(Street2Entry.Text) ||
			       !string.IsNullOrEmpty(CityEntry.Text) ||
			       !string.IsNullOrEmpty(StateEntry.Text) ||
			       !string.IsNullOrEmpty(PostCodeEntry.Text) ||
			       !string.IsNullOrEmpty(CountryEntry.Text) ||
			       !string.IsNullOrEmpty(PhoneNumberEntry.Text) ||
			       !string.IsNullOrEmpty(EmailEntry.Text) ||
			       !string.IsNullOrEmpty(WebSiteEntry.Text))
		        {
			       	if (repo.Address == null)
			       	{
						repo.Address = new GedcomAddress();		
						repo.Address.Database = Database;
			       	}
			       			       	
			       	repo.Address.AddressLine1 = Street1Entry.Text;
					repo.Address.AddressLine2 = Street2Entry.Text;
					repo.Address.City = CityEntry.Text;
					repo.Address.State = StateEntry.Text;
					repo.Address.PostCode = PostCodeEntry.Text;
					repo.Address.Country = CountryEntry.Text;
					repo.Address.Phone1 = PhoneNumberEntry.Text;
					repo.Address.Email1 = EmailEntry.Text;
					repo.Address.Www1 = WebSiteEntry.Text;
		        }
		        
		        NotesView.Save();
		    }
		}

		#endregion
	}
}
