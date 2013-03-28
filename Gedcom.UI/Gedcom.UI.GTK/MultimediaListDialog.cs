/*
 *  $Id: MultimediaListDialog.cs 183 2008-06-08 15:31:15Z davek $
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
using Gedcom.UI.GTK.Widgets;

namespace Gedcom.UI.GTK
{
	
	
	public partial class MultimediaListDialog : Gtk.Dialog
	{
		
		#region Constructors
		
		public MultimediaListDialog()
		{
			this.Build();
			
			ScrapBookView.ShowFileList = false;
		}
		
		#endregion
		
		#region Properties
		
		public GedcomDatabase Database
		{
			get { return ScrapBookView.Database; }
			set { ScrapBookView.Database = value; }
		}
		
		public GedcomRecord Record
		{
			get { return ScrapBookView.SelectedRecord; }
			set 
			{ 
				ScrapBookView.Record = value;
				
				// FIXME: major hack
				// create dummy record with all multimedia items in it
				GedcomRecord rec = new GedcomRecord();
				rec.Database = Database;
				foreach (GedcomMultimediaRecord media in Database.Media)
				{
					rec.Multimedia.Add(media.XRefID);
				}
				
				ScrapBookView.Record = rec;
			}
		}
		
		#endregion
		
		#region Event Handlers
		
		#endregion
	}
}
