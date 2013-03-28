/*
 *  $Id: IndividualMoreDialog.cs 183 2008-06-08 15:31:15Z davek $
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

using Gedcom.UI.GTK.Widgets;
using Gedcom.UI.Common;

namespace Gedcom.UI.GTK
{
	
	
	public partial class IndividualMoreDialog : Gtk.Dialog
	{
		#region Variables
				
		#endregion
		
		#region Constructors
		
		public IndividualMoreDialog()
		{
			this.Build();
			
			IndividualView.ViewTabs = Gedcom.UI.GTK.Widgets.IndividualView.ViewTabFlags.MoreDialog;
		}
	
		#endregion
		
		#region Properties
		
		public GedcomDatabase Database
		{
			get { return IndividualView.Database; }
			set { IndividualView.Database = value; }
		}
		
		public GedcomRecord Record
		{
			get { return IndividualView.Record; }
			set { IndividualView.Record = value; }
		}
		
		public IGedcomView View
		{
			get { return IndividualView; }	
		}
		
		#endregion
	
	
		#region Events
		
		public event EventHandler<SourceCitationArgs> ShowSourceCitation;
		public event EventHandler<ScrapBookArgs> ShowScrapBook;
		public event EventHandler<FactArgs> MoreFactInformation;
		public event EventHandler<NoteArgs> SelectNewNote;
		public event EventHandler<IndividualArgs> ShowName;
		public event EventHandler<IndividualArgs> DeleteIndividual;
		
		#endregion
	
		#region Event Handlers
		
		protected virtual void OnIndividualView_ShowSourceCitation (object sender, SourceCitationArgs e)
		{
			if (ShowSourceCitation != null)
			{
				ShowSourceCitation(this, e);
			}
		}

		protected virtual void OnIndividualView_ShowScrapBook (object sender, ScrapBookArgs e)
		{
			if (ShowScrapBook != null)
			{
				ShowScrapBook(this, e);
			}
		}

		protected virtual void OnIndividualView_ShowName (object sender, IndividualArgs e)
		{
			if (ShowName != null)
			{
				ShowName(this, e);
			}
		}

		protected virtual void OnIndividualView_DeleteIndividual (object sender, IndividualArgs e)
		{
			if (DeleteIndividual != null)
			{
				DeleteIndividual(this, e);
			}
		}

		protected virtual void OnIndividualView_MoreFactInformation (object sender, FactArgs e)
		{
			if (MoreFactInformation != null)
			{
				MoreFactInformation(this, e);
			}
		}

		protected virtual void OnIndividualView_SelectNewNote (object sender, NoteArgs e)
		{
			if (SelectNewNote != null)
			{
				SelectNewNote(this, e);
			}
		}
	
		#endregion
	}
}
