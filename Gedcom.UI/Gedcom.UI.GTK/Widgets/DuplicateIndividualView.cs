/*
 *  $Id: DuplicateIndividualView.cs 194 2008-11-10 20:39:37Z davek $
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

using Gedcom;

namespace Gedcom.UI.GTK.Widgets
{
	
	[System.ComponentModel.Category("Gedcom.UI.GTK.Widgets")]
	[System.ComponentModel.ToolboxItem(true)]
	public partial class DuplicateIndividualView : Gtk.Bin
	{
		#region Variables
		
		protected GedcomDatabase _database;
		protected GedcomRecord _record;
		
		protected GedcomIndividualRecord _indi;
		
		#endregion
		
		#region Constructors
		
		public DuplicateIndividualView()
		{
			this.Build();
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

				ClearView();
				
				if (_record != null)
				{		
					if (_record.RecordType == GedcomRecordType.Individual)
					{
						_indi = _record as GedcomIndividualRecord;
					}
					else
					{
						throw new Exception("Invalid record type given to DuplicateIndividualView");
					}
									
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
			
			HusbandDiedInEntry.Sensitive = enable;
			HusbandBornInEntry.Sensitive = enable;
			
			NameButton.Sensitive = enable;
		}
				
		public void ClearView()
		{
			HusbandNameEntry.Text = string.Empty;
		 	HusbandDateBornEntry.Text = string.Empty;
			HusbandDateDiedEntry.Text = string.Empty;
		 	HusbandBornInEntry.Text = string.Empty;
		 	HusbandDiedInEntry.Text = string.Empty;		
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

				if (_indi.ChildIn.Count == 0)
				{
					ParentsLabel.Text = "Parents Unknown";
				}
				else
				{
					GedcomFamilyLink link = _indi.ChildIn[0];
					
					GedcomFamilyRecord parentalFamily = _database[link.Family] as GedcomFamilyRecord;
					
					if (parentalFamily == null)
					{
						System.Diagnostics.Debug.WriteLine("Family link points to non family record");	
					}
					else
					{
						GedcomIndividualRecord husb = null;
						GedcomIndividualRecord wife = null;
						
						if (!string.IsNullOrEmpty(parentalFamily.Husband))
						{
							husb = _database[parentalFamily.Husband] as GedcomIndividualRecord;
						}
						if (!string.IsNullOrEmpty(parentalFamily.Wife))
						{
							wife = _database[parentalFamily.Wife] as GedcomIndividualRecord;
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
							sb.Append("Parents Unknown");
						}
						
						ParentsLabel.Text = sb.ToString();
					}
				}
			}
		}

		
		#endregion
	}
}
