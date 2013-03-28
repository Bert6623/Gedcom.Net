/*
 *  $Id: RepositoryList.cs 189 2008-10-10 14:16:10Z davek $
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

namespace Gedcom.UI.GTK.Widgets
{
	
	[System.ComponentModel.Category("Gedcom.UI.GTK.Widgets")]
	[System.ComponentModel.ToolboxItem(true)]
	public partial class RepositoryList : Gtk.Bin
	{
		#region Variables
				
		protected GedcomDatabase _database;
		protected GedcomRecord _record;
			
		protected RepositoryListModel _listModel;
		
		protected string _filterText;
		protected uint _refilterHandle = 0;
		
		#endregion
		
		#region Constructors
		
		public RepositoryList()
		{
			_listModel = new RepositoryListModel();
			
			this.Build();
			
			Gtk.TreeViewColumn nameCol = new Gtk.TreeViewColumn();
			Gtk.CellRenderer rend = new Gtk.CellRendererText();
			nameCol.Title = "Name";
			nameCol.Sizing = Gtk.TreeViewColumnSizing.Fixed;
			nameCol.PackStart(rend,true);
			nameCol.SetCellDataFunc(rend, new Gtk.TreeCellDataFunc(ListModelUtil.RenderRepositoryName));
								
			RepositoryTreeView.AppendColumn(nameCol);
			
			RepositoryTreeView.Model = null;
			_filterText = string.Empty;
			
			Gtk.TreeSelection selection = RepositoryTreeView.Selection;
			selection.Changed += new EventHandler(OnSelection_Changed);
		}
		
		#endregion
		
		#region Properties
		
		public GedcomDatabase Database
		{
			get { return _database; }
			set
			{
				_database = value;
				RepositoryTreeView.Model = null;
				_listModel.Database = _database;
				
				RepositoryTreeView.Model = _listModel.Adapter;
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
			}
		}
		
		public RepositoryListModel ListModel
		{
			get { return _listModel; }
			set { _listModel = value; }
		}
		
		#endregion
		
		#region Events
		
		public event EventHandler RecordChanged;
		
		#endregion
		
		#region EventHandlers
		
		private bool Refilter()
		{
			_listModel.FilterText = _filterText;
			_listModel.FilterType = (RepositoryListModel.RepositoryFilterType)FilterComboBox.Active;
			_listModel.DoFill();
			
			//_filter.Refilter();
			_refilterHandle = 0;
						
			return false;
		}
				
		protected virtual void OnNameEntry_Changed(object sender, System.EventArgs e)
		{
			_filterText = NameEntry.Text.ToLower();
			
			if (_refilterHandle != 0)
			{
				GLib.Source.Remove(_refilterHandle);
			}
		
			_refilterHandle = GLib.Timeout.Add(1000,new GLib.TimeoutHandler(Refilter));
		}
		
		
		protected virtual void OnFilterComboBox_Changed(object sender, System.EventArgs e)
		{
			_filterText = NameEntry.Text.ToLower();
			if (!string.IsNullOrEmpty(_filterText))
			{
				if (_refilterHandle != 0)
				{
					GLib.Source.Remove(_refilterHandle);
				}
			
				_refilterHandle = GLib.Timeout.Add(1000,new GLib.TimeoutHandler(Refilter));
			}
		}
		
		protected void OnSelection_Changed(object sender, System.EventArgs e)
		{
			Gtk.TreeSelection selection = RepositoryTreeView.Selection;
			
			Gtk.TreeModel model;
			Gtk.TreeIter iter;
			if (selection.GetSelected(out model, out iter))
			{
				_record = (GedcomRecord)model.GetValue(iter, 0);
			}
			else
			{
				_record = null;	
			}
			
			if (RecordChanged != null)
			{
				RecordChanged(this, EventArgs.Empty);	
			}
		}
		
		#endregion
		
		#region Methods
		
		#endregion
	}
}
