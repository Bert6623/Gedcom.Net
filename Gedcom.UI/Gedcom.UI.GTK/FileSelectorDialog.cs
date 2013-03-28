/*
 *  $Id: FileSelectorDialog.cs 189 2008-10-10 14:16:10Z davek $
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

namespace Gedcom.UI.GTK
{
	
	
	public partial class FileSelectorDialog : Gtk.FileChooserDialog
	{
		#region Variables
		
		protected object _userData;

		private Dictionary<string, string> _mimeTypeFilters;
		
		#endregion
		
		#region Constructors
		
		public FileSelectorDialog()
		{
			this.Build();
			
			this.BorderWidth = 12;
			
			(this as Gtk.FileChooser).FileActivated += new EventHandler(OnFileActivated);

			AddDefaultFilter();
		}
		
		#endregion
		
		#region Properties
		
		public bool SaveDialog
		{
			get { return Action == Gtk.FileChooserAction.Save; }
			set
			{
				if (value)
				{
					Action = Gtk.FileChooserAction.Save;
					ActionButton.Label = Gtk.Stock.Save;
				}
				else
				{
					Action = Gtk.FileChooserAction.Open;
					ActionButton.Label = Gtk.Stock.Open;
				}
			}
		}
		
		public new object UserData
		{
			get { return _userData; }
			set { _userData = value; }
		}

		public Dictionary<string, string> MimeTypeFilters
		{
			get { return _mimeTypeFilters; }
			set
			{
				_mimeTypeFilters = value;

				foreach (string mimeType in _mimeTypeFilters.Keys)
				{
					string name = _mimeTypeFilters[mimeType];
					
					AddFilter(mimeType, name);
				}
			}
		}
		
		#endregion
		
		#region Events
		
		public event EventHandler FileSelected;
		
		#endregion
		
		#region EventHandlers
		
		protected virtual void OnFileActivated(object sender, EventArgs e)
		{
			if (FileSelected != null)
			{
				FileSelected(this, EventArgs.Empty);	
			}
			
			this.Destroy();
		}
		
		protected virtual void OnResponse(object sender, Gtk.ResponseArgs args)
		{
			if (!string.IsNullOrEmpty(this.Filename))
			{
				if (FileSelected != null &&
				    ((int)args.ResponseId) == 0)
				{
					FileSelected(this, EventArgs.Empty);					
				}
			}
			this.Destroy();
		}

		#endregion

		#region Methods

		public void ClearFilters()
		{
			Gtk.FileFilter[] filters = Filters;
			foreach (Gtk.FileFilter filter in filters)
			{
				RemoveFilter(filter);
			}
		}

		public void AddFilter(string mimeType, string name)
		{
			Gtk.FileFilter mimeFilter = new Gtk.FileFilter();
			mimeFilter.Name = name;
			mimeFilter.AddMimeType(mimeType);
			AddFilter(mimeFilter);
		}

		private void AddDefaultFilter()
		{
			Gtk.FileFilter noneFilter = new Gtk.FileFilter();
			noneFilter.Name = "All Files";
			noneFilter.AddPattern("*");
			AddFilter(noneFilter);
		}
		
		#endregion
	}
}
