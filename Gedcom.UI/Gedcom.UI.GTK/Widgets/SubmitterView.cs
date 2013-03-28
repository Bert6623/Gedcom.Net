/*
 *  $Id: SubmitterView.cs 199 2008-11-15 15:20:44Z davek $
 * 
 *  Copyright (C) 2008 David A Knight <david@ritter.demon.co.uk>
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
	
	
	[System.ComponentModel.ToolboxItem(true)]
	[System.ComponentModel.Category("Gedcom.UI.GTK.Widgets")]
	public partial class SubmitterView : Gtk.Bin
	{
		#region Variables

		private GedcomDatabase _database;		
		private GedcomSubmitterRecord _submitter;
		
		#endregion
		
		
		#region Constructors
		
		public SubmitterView()
		{
			this.Build();
			notebook1.CurrentPage = 0;
		}

		#endregion

		#region Properties

		public GedcomDatabase Database
		{
			get { return _database; }
			set
			{
				_database = value;

				_submitter = null;
				if (!string.IsNullOrEmpty(_database.Header.SubmitterXRefID))
				{
					_submitter = _database.Header.Submitter;
				}

				if (_submitter == null)
				{
					// create a dummy submitter
					_submitter = new GedcomSubmitterRecord(_database);
				}
				
				AddressView.Database = _database;
				AddressView.Record = _submitter;

				NotesView.Database = _database;
				NotesView.Record = _submitter;

				ScrapBookView.Database = _database;
				ScrapBookView.Record = _submitter;
				
				Clear();
				FillView();
			}
		}
		
		#endregion

		#region Events
		
		public event EventHandler<SourceCitationArgs> ShowSourceCitation;
		public event EventHandler<NoteArgs> SelectNewNote;

		public event EventHandler<MultimediaFileArgs> OpenFile;
		public event EventHandler<MultimediaFileArgs> AddFile;
				
		#endregion

		#region Event Handlers

		protected virtual void OnNotesViewShow_SourceCitation (object sender, SourceCitationArgs e)
		{
			if (ShowSourceCitation != null)
			{
				ShowSourceCitation(this, e);
			}
		}

		protected virtual void OnNotesView_SelectNewNote (object sender, NoteArgs e)
		{
			if (SelectNewNote != null)
			{
				SelectNewNote(this, e);
			}
		}

		protected virtual void OnScrapBookView_FileSelected (object sender, System.EventArgs e)
		{
			
		}

		protected virtual void OnScrapBookView_FileDeSelected (object sender, System.EventArgs e)
		{
		}

		protected virtual void OnScrapBookView_SelectNewNote (object sender, NoteArgs e)
		{
			if (SelectNewNote != null)
			{
				SelectNewNote(this, e);
			}
		}

		protected virtual void OnScrapBookView_AddFile (object sender, MultimediaFileArgs e)
		{
			if (AddFile != null)
			{
				AddFile(this, e);
			}
		}

		protected virtual void OnScrapBookView_ShowSourceCitation (object sender, SourceCitationArgs e)
		{
			if (ShowSourceCitation != null)
			{
				ShowSourceCitation(this, e);
			}
		}

		protected virtual void OnScrapBookView_OpenFile (object sender, MultimediaFileArgs e)
		{
			if (OpenFile != null)
			{
				OpenFile(this, e);
			}
		}
		
		#endregion
		
		#region Methods

		public void Clear()
		{
			NameEntry.Text = string.Empty;
			AddressView.ClearView();
			NotesView.Clear();
		}

		public void FillView()
		{
			if (!string.IsNullOrEmpty(_submitter.Name))
			{
				NameEntry.Text = _submitter.Name;
			}
			AddressView.FillView();
		}

		public void SaveView()
		{
			_submitter.Name = NameEntry.Text;
			AddressView.SaveView();
			NotesView.Save();

			if (!string.IsNullOrEmpty(_submitter.Name) ||
			    _submitter.Address != null)
			{
				_database.Header.Submitter = _submitter;
			}
			else
			{
				_database.Header.Submitter = null;
			}
		}

		#endregion
	}
}
