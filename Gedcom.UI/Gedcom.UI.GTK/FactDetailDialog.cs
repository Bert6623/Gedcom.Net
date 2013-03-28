// FactDetailDialog.cs
//
//  Copyright (C) 2008 [name of author]
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
//
//

using System;
using Gedcom;
using Gedcom.UI.GTK.Widgets;
using Gedcom.UI.Common;

namespace Gedcom.UI.GTK
{
	
	
	public partial class FactDetailDialog : Gtk.Dialog
	{
		#region Variables
		
		#endregion
		
		#region Constructors
		
		public FactDetailDialog()
		{
			this.Build();
		}

		#endregion
		
		#region Properties
		
		public GedcomDatabase Database
		{
			get { return FactDetails.Database; }
			set { FactDetails.Database = value; }
		}
		
		public GedcomRecord Record
		{
			get { return FactDetails.Record; }
			set { FactDetails.Record = value; }
		}
		
		public FactDetailView View
		{
			get { return FactDetails; }
		}
		
		#endregion
		
		#region Events
		
		public event EventHandler<SourceCitationArgs> ShowSourceCitation;
		public event EventHandler<ScrapBookArgs> ShowScrapBook;
		public event EventHandler<NoteArgs> SelectNewNote;
				
		#endregion
		
		#region Event Handlers
		
		protected virtual void OnFactDetails_ShowSourceCitation (object sender, SourceCitationArgs e)
		{
			if (ShowSourceCitation != null)
			{
				ShowSourceCitation(this, e);
			}
		}

		protected virtual void OnFactDetails_ShowScrapBook (object sender, ScrapBookArgs e)
		{
			if (ShowScrapBook != null)
			{
				ShowScrapBook(this, e);
			}
		}

		protected virtual void OnFactDetails_SelectNewNote (object sender, NoteArgs e)
		{
			if (SelectNewNote != null)
			{
				SelectNewNote(this, e);
			}
		}
		
		#endregion
	}
}
