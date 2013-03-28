/*
 *  $Id: SummaryView.cs 189 2008-10-10 14:16:10Z davek $
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
	public partial class SummaryView : Gtk.Bin, IGedcomView
	{
		#region Variables
		
		protected GedcomDatabase _database;
		protected GedcomRecord _record;
				
		#endregion
		
		#region Constructors
		
		public SummaryView()
		{
			this.Build();
		}
		
		#endregion
		
		#region Properties
		
		#endregion
		
		#region IGedcomView
		
		#region Properties
		
		public GedcomDatabase Database
		{
			get { return _database; }
			set
			{
				_database = value;
				
				DatabaseNameLabel.Text = _database.Name;
				IndividualsCountLabel.Text = _database.Individuals.Count.ToString();
				FamiliesCountLabel.Text = _database.Families.Count.ToString();
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
		
		#region Methods
		
		public void ClearView()
		{
			
		}
		
		public void SaveView()
		{
			
		}
		
		#endregion
		
		#endregion
	}
}
