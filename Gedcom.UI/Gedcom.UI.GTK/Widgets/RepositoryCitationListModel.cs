/*
 *  $Id: RepositoryCitationListModel.cs 183 2008-06-08 15:31:15Z davek $
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
	// FIXME: wrong name, inconsistant, really a list of call numbers in
	// a repo citation
	// FIXME: use a GenericListModel<T>
	public class RepositoryCitationListModel : Gtk.ListStore
	{	
		#region Variables
				
		protected GedcomDatabase _database;
		protected GedcomRecord _record;
		
		#endregion
		
		#region Constructors
		
		public RepositoryCitationListModel() : base(new Type[] { typeof(string),typeof(string) })
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
				
				if (_record.RecordType != GedcomRecordType.RepositoryCitation)
				{
					throw new Exception("Must set a repository citation record");	
				}
				
				DoFill();
			}
		}
		
				
		#endregion
		
		#region Methods
		
		public void DoFill()
		{
			if (_database == null)
			{
				throw new Exception("Database must be set before filling the model");	
			}
			
			this.Clear();
			
			// FIXME: this isn't nice or right
			
			GedcomRepositoryCitation repo = _record as GedcomRepositoryCitation;
			
			int i = 0;
			int j = 0;
			foreach (string callNumber in repo.CallNumbers)
			{
				Gtk.TreeIter iter = this.Append();
				this.SetValue(iter,0, callNumber);
				SourceMediaType type = repo.MediaTypes[i];
				string mediaType = "None";
				if (type == SourceMediaType.Other)
				{
					mediaType = repo.OtherMediaTypes[j++];
				}
				else
				{
					mediaType = type.ToString().Replace('_', ' ');
				}
				if (string.IsNullOrEmpty(mediaType))
				{
					mediaType = "None";	
				}
				this.SetValue(iter,1, mediaType);
				i ++;
			}
		}
		
		#endregion
	}
}
