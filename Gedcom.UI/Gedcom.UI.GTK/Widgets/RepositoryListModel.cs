/*
 *  $Id: RepositoryListModel.cs 183 2008-06-08 15:31:15Z davek $
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
	
	public class RepositoryListModel : GenericListModel<GedcomRepositoryRecord>
	{
		#region Enums
		
		public enum RepositoryFilterType
		{
			Name = 0
		};
		
		#endregion
		
		#region Variables
				
		protected GedcomDatabase _database;
		protected GedcomRecord _record;
		
		protected string _filterText;
		protected RepositoryFilterType _filterType; 
		
		protected string _noIndividualLabel = string.Empty;
		
		public const string UnknownName = "unknown";
		
		#endregion
		
		#region Constructors
		
		public RepositoryListModel()
		{
			_filterText = string.Empty;
			_filterType = RepositoryFilterType.Name;
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
		
		public RepositoryFilterType FilterType
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
			
			List = _database.Repositories;
		}
			
		protected override bool Filter(GedcomRepositoryRecord repo)
		{
			bool show = true;
								
			if (repo != null)
			{
				string name = string.Empty;
				
				switch (_filterType)
				{
					case RepositoryFilterType.Name:
						name = repo.Name;
						break;
				}
								
				show = name.StartsWith(_filterText, true, CultureInfo.CurrentCulture);
			}
			
			return show;
		}
		
		#endregion
	}
}
