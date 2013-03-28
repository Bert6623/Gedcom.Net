/*
 *  $Id: MarriageView.cs 197 2008-11-15 12:41:00Z davek $
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
	public partial class MarriageView : Gtk.Bin
	{
		#region Variables
		
		protected GedcomDatabase _database;
		protected GedcomRecord _record;
		
		protected GedcomFamilyRecord _famRecord;
		protected GedcomIndividualRecord _husband;
		protected GedcomIndividualRecord _wife;
		
		protected GedcomAssociation _husbandAssociation;
		protected GedcomAssociation _wifeAssociation;
				
		#endregion
		
		#region Constructors
		
		public MarriageView()
		{
			this.Build();
			
			MarriageButtonBox.NoShowAll = true;
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
				
				GedcomFamilyRecord fam = null;
				
				if (_record.RecordType == GedcomRecordType.Individual)
				{
					// indi record, find first family
					GedcomIndividualRecord indi = _record as GedcomIndividualRecord;
					
					if (indi.SpouseIn.Count > 0)
					{
						GedcomFamilyLink link = indi.SpouseIn[0];
						fam = _database[link.Family] as GedcomFamilyRecord;
					}
					else
					{
						// FIXME: not in a family as a spouse, create a new
						// family record					
						fam = new GedcomFamilyRecord(_database, indi, null);
					}
				}
				else if (_record.RecordType != GedcomRecordType.Family)
				{
					throw new Exception("Invalid record type given to FamilyView");
				}
				else
				{
					fam = _record as GedcomFamilyRecord;
				}
				
				// NOTE: _record will be whatever was passed in, family or
				// individual
				
				Clear();
				
				_famRecord = fam;
				
				FillView();
			}
		}
		
		public GedcomIndividualRecord Husband
		{
			get { return _husband; }
		}
		
		public GedcomIndividualRecord Wife
		{
			get { return _wife; }
		}
	
		public bool ShowMoreButton
		{
			set 
			{ 
				MarriageMoreButton.Visible = value;
			}
		}
		
		#endregion
		
		#region Events
		
		public event EventHandler<SourceCitationArgs> ShowSourceCitation;
		public event EventHandler<ScrapBookArgs> ShowScrapBook;
		public event EventHandler<FamilyArgs> MoreFamilyInformation;
		
		#endregion
		
		#region Event Handlers
		
		protected virtual void OnMarriageMoreButton_Clicked(object sender, System.EventArgs e)
		{
			FamilyArgs args = new FamilyArgs();
			args.Fam = _famRecord;
		
			Save();
		
			if (MoreFamilyInformation != null)
			{
				MoreFamilyInformation(this, args);	
			}
		}
		
		protected virtual void OnMarriageSourceButton_Clicked(object sender, System.EventArgs e)
		{
			Save();
			
			if (ShowSourceCitation != null)
			{
				SourceCitationArgs args = new SourceCitationArgs();
				args.Record = _famRecord.Marriage;
			
				ShowSourceCitation(this,args);
			}
		}
			
				
		protected virtual void OnMarriageScrapbookButton_Clicked(object sender, System.EventArgs e)
		{
			Save();
			
			if (_famRecord.Marriage != null)
			{
				if (ShowScrapBook != null)
				{
					ScrapBookArgs args = new ScrapBookArgs();
					args.Record = _famRecord.Marriage;
					ShowScrapBook(this, args);
				}
			}
		}
		
				
		protected virtual void Marriage_Changed(object sender, System.EventArgs e)
		{
			bool sensitive = ((!string.IsNullOrEmpty(MarriageLocationEntry.Text)) ||
							 (!string.IsNullOrEmpty(MarriageDateEntry.Text)));
			sensitive &= MarriageDateEntry.Sensitive;
							 
			MarriageSourceButton.Sensitive = sensitive;
			BeginingStatusComboBox.Sensitive = sensitive;
			MarriageButtonBox.Sensitive = sensitive;
		}
		
		#endregion
		
		#region Methods

		public void Clear()
		{	
		 	MarriageDateEntry.Text = string.Empty;
			MarriageLocationEntry.Text = string.Empty;
			
			BeginingStatusComboBox.Active = 4;
			
			Marriage_Changed(this, EventArgs.Empty);
		}
		
		private void FillView()
		{
			GedcomFamilyRecord fam = _famRecord;
			
			GedcomIndividualRecord husb = null;
			GedcomIndividualRecord wife = null;
		
			GedcomFamilyEvent marriage = fam.Marriage as GedcomFamilyEvent;

			if (marriage != null)
			{
				GedcomPlace place = marriage.Place;
				if (place != null)
				{
					MarriageLocationEntry.Text = place.Name;	
				}
				
				GedcomDate date = marriage.Date;
				if (date != null)
				{
					MarriageDateEntry.Text = date.DateString;
				}
			}

			BeginingStatusComboBox.Active = (int)fam.StartStatus;
			
			_husband = husb;
			_wife = wife;	
			
			Marriage_Changed(this, EventArgs.Empty);
		}

		
		public void Save()
		{
			if (_famRecord != null)
			{
				GedcomFamilyEvent marriage = _famRecord.Marriage;
				if (!string.IsNullOrEmpty(MarriageDateEntry.Text) ||
				    !string.IsNullOrEmpty(MarriageLocationEntry.Text))
				{
					if (marriage == null)
					{
						marriage = new GedcomFamilyEvent();
						marriage.Database = _database;
						marriage.EventType = GedcomEvent.GedcomEventType.MARR;
						marriage.Level = _famRecord.Level + 1;
						_famRecord.Events.Add(marriage);
					}
					if (marriage.Place == null)
					{
						marriage.Place = new GedcomPlace();
						marriage.Place.Level = marriage.Level + 1;
					}
					if (marriage.Date == null)
					{
						marriage.Date = new GedcomDate(_database);
						marriage.Date.Level = marriage.Level + 1;
					}
					marriage.Place.Database = _database;
					marriage.Place.Name = MarriageLocationEntry.Text;
					marriage.Date.ParseDateString(MarriageDateEntry.Text);
				}
				else if (marriage != null)
				{
					_famRecord.Events.Remove(marriage);
				}
				
				_famRecord.StartStatus = (MarriageStartStatus)BeginingStatusComboBox.Active;
			}
			
		}
		
		#endregion
	}
}
