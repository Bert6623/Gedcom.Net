/*
 *  $Id: SourceListModel.cs 183 2008-06-08 15:31:15Z davek $
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
using System.Globalization;

using Gedcom;

namespace Gedcom.UI.GTK.Widgets
{
	
	public class SourceListModel : GenericListModel<GedcomSourceRecord>
	{
		#region Enums
		
		public enum SourceFilterType
		{
			Title = 0
		};
		
		#endregion
		
		#region Variables
				
		protected GedcomDatabase _database;
		protected GedcomRecord _record;
		
		protected string _filterText;
		protected SourceFilterType _filterType; 
		
		protected string _noIndividualLabel = string.Empty;
		
		public const string UnknownName = "unknown";
		
		#endregion
		
		#region Constructors
		
		public SourceListModel()
		{
			_filterText = string.Empty;
			_filterType = SourceFilterType.Title;	
			_applyFilter = true;
		}
				
		#endregion
		
		#region Properties
		
		public virtual GedcomDatabase Database
		{
			get { return _database; }
			set
			{
				_database = value;
				DoFill();
			}
		}
		
		public virtual GedcomRecord Record
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
		
		public string FilterText
		{
			get { return _filterText; }
			set { _filterText = value; }
		}
		
		public SourceFilterType FilterType
		{
			get { return _filterType; }
			set { _filterType = value; }
		}
		
		public string NoIndividualLabel
		{
			get { return _noIndividualLabel; }
			set { _noIndividualLabel = value; }
		}
				
		#endregion
		
		#region Methods
		
		public void DoFill()
		{
			if (_database == null)
			{
				throw new Exception("Database must be set before filling the model");	
			}
			
			List = _database.Sources;
		}
	
		protected override bool Filter(GedcomSourceRecord source)
		{
			bool show = true;
								
			if (source != null)
			{
				string name = string.Empty;
				
				switch (_filterType)
				{
					case SourceFilterType.Title:
						name = source.Title;
						break;
				}
								
				show = name.StartsWith(_filterText, true, CultureInfo.CurrentCulture);
			}
			
			return show;
		}
		
		#endregion
	}
}
