/*
 *  $Id: SpouseView.cs 194 2008-11-10 20:39:37Z davek $
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
using System.Text;

namespace Gedcom.UI.GTK.Widgets
{
	
	
	[System.ComponentModel.Category("Gedcom.UI.GTK.Widgets")]
	[System.ComponentModel.ToolboxItem(true)]
	public partial class SpouseView : Gtk.Bin
	{
		#region Variables
		
		protected GedcomDatabase _database;
		protected GedcomRecord _record;
		
		protected GedcomIndividualRecord _indi;
		
		
		protected GedcomFamilyRecord _parentalFamily;
		
		#endregion

		#region Constructors
		
		public SpouseView()
		{
			this.Build();
		}
		
		#endregion
		
		#region IGedcomView
		
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

				ClearView();
				
				if (_record != null)
				{		
					if (_record.RecordType == GedcomRecordType.Individual)
					{
						_indi = _record as GedcomIndividualRecord;
					}
					else
					{
						throw new Exception("Invalid record type given to SpouseView");
					}
					
					_parentalFamily = null;
					
					Enable();
					FillView();
				}
				else
				{
					Disable();
					_indi = null;
				}
			}
		}
		
		#endregion
		
		#endregion
		
		#region Properties
		
		public string NameLabel
		{
			get { return SpouseNameLabel.Text; }
			set { SpouseNameLabel.Text = value; }
		}
		
		public GedcomFamilyRecord ParentalFamily
		{
			get { return _parentalFamily; }
			set { _parentalFamily = value; }
		}
		
		#endregion
		
		#region Events
		
		public event EventHandler AddButtonClicked;
		public event EventHandler RemoveButtonClicked;
		public event EventHandler DeleteButtonClicked;
		
		public event EventHandler MoreButtonClicked;
		public event EventHandler ScrapBookButtonClicked;
		public event EventHandler FamiliesButtonClicked;
		
		public event EventHandler NameSourceButtonClicked;
		public event EventHandler DateBornSourceButtonClicked;
		public event EventHandler DateDiedSourceButtonClicked;
		
		public event EventHandler ParentsButtonClicked;
		
		public event EventHandler NameButtonClicked;
		
		#endregion
		
		#region Event Handlers
		
		protected virtual void OnAddButton_Clicked(object sender, System.EventArgs e)
		{
			if (AddButtonClicked != null)
			{
				AddButtonClicked(this, EventArgs.Empty);
			}
		}

		
		protected virtual void OnRemoveButton_Clicked(object sender, System.EventArgs e)
		{
			if (_indi != null)
			{
				if (RemoveButtonClicked != null)
				{
					RemoveButtonClicked(this, EventArgs.Empty);
				}
			}
		}
		
		protected virtual void OnDeleteButton_Clicked(object sender, System.EventArgs e)
		{
			if (_indi != null)
			{
				if (DeleteButtonClicked != null)
				{
					DeleteButtonClicked(this, EventArgs.Empty);
				}
			}
		}
		
		protected virtual void OnMoreButton_Clicked(object sender, System.EventArgs e)
		{
			SaveView();

			if (MoreButtonClicked != null)
			{
				MoreButtonClicked(this, EventArgs.Empty);
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
		
		protected virtual void OnFamiliesButton_Clicked(object sender, System.EventArgs e)
		{
			SaveView();
		
			if (FamiliesButtonClicked != null)
			{
				FamiliesButtonClicked(this, EventArgs.Empty);
			}
		}
		
		protected virtual void OnNameSourceButton_Clicked(object sender, System.EventArgs e)
		{
			SaveView();
			
			if (NameSourceButtonClicked != null)
			{
				NameSourceButtonClicked(this, EventArgs.Empty);
			}
		}

		protected virtual void OnDateBornSourceButton_Clicked(object sender, System.EventArgs e)
		{
			SaveView();
			
			if (DateBornSourceButtonClicked != null)
			{
				DateBornSourceButtonClicked(this, EventArgs.Empty);
			}
		}

		protected virtual void OnDateDiedSourceButton_Clicked(object sender, System.EventArgs e)
		{
			SaveView();
			
			if (DateDiedSourceButtonClicked != null)
			{
				DateDiedSourceButtonClicked(this, EventArgs.Empty);
			}
		}
	
		protected virtual void OnParentsButton_Click(object sender, System.EventArgs e)
		{
			SaveView();
			
			if (ParentsButtonClicked != null)
			{
				ParentsButtonClicked(this, EventArgs.Empty);
			}
		}
		
		
		protected virtual void OnNameButton_Clicked(object sender, System.EventArgs e)
		{
			SaveView();
			
			if (NameButtonClicked != null)
			{
				NameButtonClicked(this, EventArgs.Empty);
			}
		}
		
		protected virtual void HusbandBorn_Changed(object sender, System.EventArgs e)
		{
			bool sensitive = ((!string.IsNullOrEmpty(HusbandBornInEntry.Text)) ||
							 (!string.IsNullOrEmpty(HusbandDateBornEntry.Text)));
			sensitive &= HusbandBornInEntry.Sensitive;
							 
			HusbandDateBornSourceButton.Sensitive = sensitive;
		}

		protected virtual void HusbandDied_Changed(object sender, System.EventArgs e)
		{
			bool sensitive = ((!string.IsNullOrEmpty(HusbandDiedInEntry.Text)) ||
							 (!string.IsNullOrEmpty(HusbandDateDiedEntry.Text)));
			sensitive &= HusbandDiedInEntry.Sensitive;
							 
			HusbandDateDiedSourceButton.Sensitive = sensitive;
		}
		
		#endregion
		
		#region Methods
		
		public void Enable()
		{
			Enable(true);
		}
		
		public void Disable()
		{
			Enable(false);
		}
		
		private void Enable(bool enable)
		{		
			HusbandNameSourceButton.Sensitive = enable;
			HusbandNameEntry.Sensitive = enable;
			HusbandDateDiedSourceButton.Sensitive = enable;
			HusbandDateDiedEntry.Sensitive = enable;
			HusbandDateBornSourceButton.Sensitive = enable;
			HusbandDateBornEntry.Sensitive = enable;
			
			HusbandMoreButton.Sensitive = enable;
			ScrapBookButton.Sensitive = enable;
			HusbandFamiliesButton.Sensitive = enable;
			
			HusbandDiedInEntry.Sensitive = enable;
			HusbandBornInEntry.Sensitive = enable;
			
			AddHusbandButton.Sensitive = !enable;
			RemoveHusbandButton.Sensitive = enable;
			DeleteButton.Sensitive = enable;
			ParentsButton.Sensitive = enable;
			
			NameButton.Sensitive = enable;
			
			// set sensitivity on source buttons for birth/death
			HusbandBorn_Changed(this, EventArgs.Empty);
			HusbandDied_Changed(this, EventArgs.Empty);
		}
				
		public void ClearView()
		{
			HusbandNameEntry.Text = string.Empty;
		 	HusbandDateBornEntry.Text = string.Empty;
			HusbandDateDiedEntry.Text = string.Empty;
		 	HusbandBornInEntry.Text = string.Empty;
		 	HusbandDiedInEntry.Text = string.Empty;
		 	
		 	// set sensitivity on source buttons for birth/death
			HusbandBorn_Changed(this, EventArgs.Empty);
			HusbandDied_Changed(this, EventArgs.Empty);
		}
		
		private void FillView()
		{
			if (_indi != null)
			{
				GedcomName name = _indi.GetName();
				
				HusbandNameEntry.Text = name.Name;
				
				GedcomIndividualEvent birth = _indi.Birth;
				if (birth != null)
				{
					GedcomPlace place = birth.Place;
					if (place != null)
					{
						HusbandBornInEntry.Text = place.Name;	
					}
					
					GedcomDate date = birth.Date;
					if (date != null)
					{
						HusbandDateBornEntry.Text = date.DateString;
					}
				}
				
				GedcomIndividualEvent death = _indi.Death;
				if (death != null)
				{
					GedcomPlace place = death.Place;
					if (place != null)
					{
						HusbandDiedInEntry.Text = place.Name;	
					}
					
					GedcomDate date = death.Date;
					if (date != null)
					{
						HusbandDateDiedEntry.Text = date.DateString;
					}
				}

				ParentsButton.Sensitive = false;
				if (_indi.ChildIn.Count == 0)
				{
					ParentsButton.Label = "Parents";
				    ParentsButton.Image = Gtk.Image.NewFromIconName(Gtk.Stock.GoUp, Gtk.IconSize.Button);
				}
				else
				{
					GedcomFamilyLink link = _indi.ChildIn[0];
					
					_parentalFamily = _database[link.Family] as GedcomFamilyRecord;
					
					if (_parentalFamily == null)
					{
						System.Diagnostics.Debug.WriteLine("Family link points to non family record");	
					}
					else
					{
						GedcomIndividualRecord husb = null;
						GedcomIndividualRecord wife = null;
						
						if (!string.IsNullOrEmpty(_parentalFamily.Husband))
						{
							husb = _database[_parentalFamily.Husband] as GedcomIndividualRecord;
						}
						if (!string.IsNullOrEmpty(_parentalFamily.Wife))
						{
							wife = _database[_parentalFamily.Wife] as GedcomIndividualRecord;
						}
						
						StringBuilder sb = new StringBuilder();

						GedcomName husbandName = null;
						
						if (husb != null)
						{
							husbandName = husb.GetName();
						}

						if (husbandName != null)
						{
							sb.Append(husbandName.Given);
						}

						GedcomName wifeName = null;
						
						if (wife != null)
						{
							wifeName = wife.GetName();
						}

						if (wifeName != null)
						{
							if (sb.Length > 0)
							{
								sb.Append(" & ");
							}
							sb.Append(wifeName.Given);
						}
						
						if (husbandName != null)
						{
							if (sb.Length > 0)
							{
								sb.Append(" ");
							}
							sb.Append(husbandName.Surname);
						}
						
						if (sb.Length == 0)
						{
							sb.Append("Parents");
						}
						
						ParentsButton.Label = sb.ToString();
						ParentsButton.Image = Gtk.Image.NewFromIconName(Gtk.Stock.GoUp, Gtk.IconSize.Button);
						ParentsButton.Sensitive = true;
					}
				}
				
				HusbandFamiliesButton.Image = Gtk.Image.NewFromIconName(Gtk.Stock.DialogInfo, Gtk.IconSize.Button);
				HusbandFamiliesButton.Label = string.Format("Families ({0})", _indi.SpouseIn.Count);
				
				// set sensitivity on source buttons for birth/death
				HusbandBorn_Changed(this, EventArgs.Empty);
				HusbandDied_Changed(this, EventArgs.Empty);
			}
		}

		public void SaveView()
		{
			if (_indi != null)
			{
				GedcomName name = null;
				
				if (_indi.Names.Count > 0)
				{
					name = _indi.GetName();
				}
				else
				{
					name = new GedcomName();
					name.Database = _database;
					name.Level = _indi.Level + 1;
					name.PreferedName = true;
					_indi.Names.Add(name);
				}
				
				// don't care if the name is empty, set it anyway
				name.Name = HusbandNameEntry.Text;
				
				GedcomIndividualEvent birth = _indi.Birth;
				if (!string.IsNullOrEmpty(HusbandBornInEntry.Text) ||
				    !string.IsNullOrEmpty(HusbandDateBornEntry.Text))
				{
					if (birth == null)
					{
						birth = new GedcomIndividualEvent();
						birth.Database = _database;
						birth.EventType = GedcomEvent.GedcomEventType.BIRT;
						birth.Level = _indi.Level + 1;
						birth.IndiRecord = _indi;
						_indi.Events.Add(birth);
					}
					if (birth.Place == null)
					{
						birth.Place = new GedcomPlace();
						birth.Place.Level = birth.Level + 1;
					}
					if (birth.Date == null)
					{
						birth.Date = new GedcomDate(_database);
						birth.Date.Level = birth.Level + 1;
					}
					
					birth.Place.Database = _database;
					birth.Place.Name = HusbandBornInEntry.Text;
					birth.Date.ParseDateString(HusbandDateBornEntry.Text);
				}
				else if (birth != null)
				{
					_indi.Events.Remove(birth);	
				}
				
				GedcomIndividualEvent death = _indi.Death;
				if (!string.IsNullOrEmpty(HusbandDiedInEntry.Text) ||
				    !string.IsNullOrEmpty(HusbandDateDiedEntry.Text))
				{
					if (death == null)
					{
						death = new GedcomIndividualEvent();
						death.Database = _database;
						death.EventType = GedcomEvent.GedcomEventType.DEAT;
						death.Level = _indi.Level + 1;
						death.IndiRecord = _indi;
						_indi.Events.Add(death);
					}
					if (death.Place == null)
					{
						death.Place = new GedcomPlace();
						death.Place.Level = death.Level + 1;
					}
					if (death.Date == null)
					{
						death.Date = new GedcomDate(_database);
						death.Date.Level = death.Level + 1;
					}
					
					death.Place.Database = _database;
					death.Place.Name = HusbandDiedInEntry.Text;
					death.Date.ParseDateString(HusbandDateDiedEntry.Text);
				}
				else if (death != null)
				{
					_indi.Events.Remove(death);	
				}
			}
			
		}

		#endregion
	}
}
