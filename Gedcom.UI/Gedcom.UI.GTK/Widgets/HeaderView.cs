/*
 *  $Id: HeaderView.cs 199 2008-11-15 15:20:44Z davek $
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
	
	[System.ComponentModel.Category("Gedcom.UI.GTK.Widgets")]
	[System.ComponentModel.ToolboxItem(true)]
	public partial class HeaderView : Gtk.Bin
	{
		#region Variables

		private GedcomDatabase _database;
		private GedcomHeader _header;
		
		#endregion
		
		#region Constructors
		
		public HeaderView()
		{
			this.Build();
		}

		#endregion

		#region Properties

		public GedcomDatabase Database
		{
			get { return _database; }
			set
			{
				_database = value;

				if (_database.Header == null)
				{
					throw new Exception("Database is missing GedcomHeader");
				}

				_header = _database.Header;
				
				SubmitterView.Database = _database;
				AddressView.Database = _database;

				AddressView.Record = _header;

				ClearView();

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

		protected virtual void OnSubmitterView_ShowSourceCitation (object sender, SourceCitationArgs e)
		{
			if (ShowSourceCitation != null)
			{
				ShowSourceCitation(this, e);
			}
		}

		protected virtual void OnSubmitterView_SelectNewNote (object sender, NoteArgs e)
		{
			if (SelectNewNote != null)
			{
				SelectNewNote(this, e);
			}
		}

		protected virtual void OnSubmitterView_AddFile (object sender, MultimediaFileArgs e)
		{
			if (AddFile != null)
			{
				AddFile(this, e);
			}
		}

		protected virtual void OnSubmitterView_OpenFile (object sender, MultimediaFileArgs e)
		{
			if (OpenFile != null)
			{
				OpenFile(this, e);
			}
		}
		
		#endregion
		
		#region Methods

		public void ClearView()
		{
			SourceNameEntry.Text = string.Empty;
			SourceDateEntry.Text = string.Empty;
			CopyrightEntry.Text = string.Empty;
			LanguageEntry.Text = string.Empty;
			DescriptionTextView.Buffer.Clear();

			ApplicationNameLabel.Text = string.Empty;
			VersionLabel.Text = string.Empty;
			CorporationLabel.Text = string.Empty;

			AddressView.ClearView();
			
			SubmitterView.Clear();
		}

		public void FillView()
		{
			SourceNameEntry.Text = _header.SourceName;
			if (_header.SourceDate != null)
			{
				SourceDateEntry.Text = _header.SourceDate.DateString;
			}

			if (!string.IsNullOrEmpty(_header.Copyright))
			{
				CopyrightEntry.Text = _header.Copyright;
			}

			if (!string.IsNullOrEmpty(_header.Language))
			{
				LanguageEntry.Text = _header.Language;
			}

			if (_header.ContentDescription != null)
			{
				DescriptionTextView.Buffer.Text = _header.ContentDescription.Text;
			}

			if (!string.IsNullOrEmpty(_header.ApplicationName))
			{
				ApplicationNameLabel.Text = _header.ApplicationName;
			}

			if (!string.IsNullOrEmpty(_header.ApplicationVersion))
			{
				VersionLabel.Text = _header.ApplicationVersion;
			}

			if (!string.IsNullOrEmpty(_header.Corporation))
			{
				CorporationLabel.Text = _header.Corporation;
			}

			AddressView.FillView();

			SubmitterView.FillView();
		}

		public void SaveView()
		{
			_header.SourceName = SourceNameEntry.Text;
			if (string.IsNullOrEmpty(SourceDateEntry.Text))
			{
				_header.SourceDate = null;
			}
			else
			{
				if (_header.SourceDate == null)
				{
					_header.SourceDate = new GedcomDate(_database);
					_header.SourceDate.Level = _header.Level + 2;
				}
				_header.SourceDate.ParseDateString(SourceDateEntry.Text);
			}
			
			_header.Copyright = CopyrightEntry.Text;
			_header.Language = LanguageEntry.Text;
			
			if (string.IsNullOrEmpty(DescriptionTextView.Buffer.Text))
			{
				_header.ContentDescription = null;
			}
			else
			{
				if (_header.ContentDescription == null)
				{
					_header.ContentDescription = new GedcomNoteRecord();
					_header.ContentDescription.Level = _header.Level + 1;
					_header.ContentDescription.Database = _database;
				}
				_header.ContentDescription.Text = DescriptionTextView.Buffer.Text;
			}

			SubmitterView.SaveView();
		}

		#endregion
	}
}
