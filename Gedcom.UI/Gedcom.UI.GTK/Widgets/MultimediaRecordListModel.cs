/*
 *  $Id: MultimediaRecordListModel.cs 183 2008-06-08 15:31:15Z davek $
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
	
	
	public class MultimediaRecordListModel : GenericListModel<GedcomMultimediaRecord>
	{
		#region Variables
				
		protected GedcomDatabase _database;
		protected GedcomRecord _record;
				
		#endregion
		
		#region Constructors
		
		public MultimediaRecordListModel()
		{
			
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

				List<GedcomMultimediaRecord> records = null;
				
				if (_record != null)
				{
					records = new List<GedcomMultimediaRecord>(_record.Multimedia.Count);
					foreach (string multimediaId in _record.Multimedia)
					{
						GedcomMultimediaRecord multimedia = _database[multimediaId] as GedcomMultimediaRecord;
						if (multimedia != null)
						{
							records.Add(multimedia);
						}
						else
						{
							throw new Exception("Multimedia reference points to non multimedia record");
						}
					}
				}
				
				List = records;
			}
		}
		
		#endregion
		
				
		#region Methods
		
		
		#endregion
	}
}
