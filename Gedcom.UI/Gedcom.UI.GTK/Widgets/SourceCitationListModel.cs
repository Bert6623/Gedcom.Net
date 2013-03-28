/*
 *  $Id: SourceCitationListModel.cs 183 2008-06-08 15:31:15Z davek $
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

using Gedcom;

namespace Gedcom.UI.GTK.Widgets
{
	
	public class SourceCitationListModel : GenericListModel<GedcomSourceCitation>
	{
		#region Variables
				
		protected GedcomDatabase _database;
		protected GedcomRecord _record;
		
		protected string _noIndividualLabel = string.Empty;
		
		public const string UnknownName = "unknown";
		
		#endregion
		
		#region Constructors
		
		public SourceCitationListModel()
		{
			
		}
				
		#endregion
		
		#region Properties
		
		public virtual GedcomDatabase Database
		{
			get { return _database; }
			set
			{
				_database = value;
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
				
				DoFill();
			}
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
			
			if (_record == null)
			{
				throw new Exception("Record must be set before filling the model");	
			}
						
			List = _record.Sources;
		}

		#endregion
	}
}
