/*
 *  $Id: PropertiesDialog.cs 199 2008-11-15 15:20:44Z davek $
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
using System.Diagnostics;
using System.IO;
using Gedcom.UI.Common;
using Gedcom.UI.GTK.Widgets;

namespace Gedcom.UI.GTK
{
	
	
	public partial class PropertiesDialog : Gtk.Dialog
	{
		#region Constructors
		
		public PropertiesDialog()
		{
			this.Build();
		}

		#endregion

		#region Properties

		public GedcomDatabase Database
		{
			get { return HeaderView.Database; }
			set 
			{
				HeaderView.Database = value;
			}
		}
		
		#endregion

		#region Events
		
		public event EventHandler<SourceCitationArgs> ShowSourceCitation;
		public event EventHandler<NoteArgs> SelectNewNote;
		
		#endregion
		
		#region Event Handlers

		protected virtual void OnHeaderView_ShowSourceCitation (object sender, SourceCitationArgs e)
		{
			if (ShowSourceCitation != null)
			{
				ShowSourceCitation(this, e);
			}
		}

		protected virtual void OnHeaderView_SelectNewNote (object sender, NoteArgs e)
		{
			if (SelectNewNote != null)
			{
				SelectNewNote(this, e);
			}
		}

		protected virtual void OnHeaderView_AddFile (object sender, MultimediaFileArgs e)
		{
			FileSelectorDialog fileSelector = new FileSelectorDialog();
			fileSelector.Title = "Select Media File";
			fileSelector.AddFilter("image/*", "Images");
			fileSelector.AddFilter("video/*", "Videos");
			fileSelector.AddFilter("audio/*", "Audio");
			fileSelector.AddFilter("text/*", "Text Documents");
			
			fileSelector.FileSelected += new EventHandler(ScrapBookList_AddFileSelected);
			fileSelector.UserData = e;
			fileSelector.Modal = true;
					
			fileSelector.Run();
		}

		protected void ScrapBookList_AddFileSelected(object sender, EventArgs e)
		{
			FileSelectorDialog fileSelector = sender as FileSelectorDialog;	

			MultimediaFileArgs addFileArgs = (MultimediaFileArgs)fileSelector.UserData;
			
			addFileArgs.Filename = fileSelector.Filename;
		}
		
		protected virtual void OnHeaderView_OpenFile (object sender, MultimediaFileArgs e)
		{
			if (!string.IsNullOrEmpty(e.Filename))
			{
				Process process = new Process();
				
				process.StartInfo.FileName = e.Filename;
				process.StartInfo.UseShellExecute = true;
				
				FileInfo info = new FileInfo(Database.Name);
				process.StartInfo.WorkingDirectory = info.Directory.FullName;
				
				System.Console.WriteLine("Launch: " + e.Filename);
				
				if (!process.Start())
				{
					// FIXME: inform user
					System.Console.WriteLine("Failed to launch process");
				}
			}
			else
			{
				// FIXME: inform user
				System.Console.WriteLine("No Filename set for media file");
			}
		}
				
		#endregion
		
		#region Methods

		public void SaveView()
		{
			HeaderView.SaveView();
		}

		#endregion
	}
}
