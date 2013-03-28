/*
 *  $Id: IndividualList.cs 194 2008-11-10 20:39:37Z davek $
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
using Gedcom.UI.Common;

namespace Gedcom.UI.GTK.Widgets
{
	
	
	[System.ComponentModel.Category("Gedcom.UI.GTK.Widgets")]
	[System.ComponentModel.ToolboxItem(true)]
	public partial class IndividualList : Gtk.Bin, IGedcomView
	{
		#region Variables
				
		protected GedcomDatabase _database;
		protected GedcomRecord _record;
			
		protected IndividualListModel _listModel;
		
		protected string _filterText;
		protected uint _refilterHandle = 0;

		private Gtk.TreeViewColumn _nameCol;
		private Gtk.TreeViewColumn _dobCol;
		private Gtk.TreeViewColumn _preferedCol;
		
		#endregion
		
		#region Constructors
		
		public IndividualList()
		{		
			this.Build();
			
			_nameCol = new Gtk.TreeViewColumn();
			Gtk.CellRenderer rend = new Gtk.CellRendererText();
			_nameCol.Title = "Name";
			_nameCol.Sizing = Gtk.TreeViewColumnSizing.Fixed;
			_nameCol.PackStart(rend,true);
			_nameCol.SetCellDataFunc(rend, new Gtk.TreeCellDataFunc(ListModelUtil.RenderIndividualName));
			_nameCol.Resizable = true;
			rend.Width = 200;
			_nameCol.MinWidth = 250;
			
			_dobCol = new Gtk.TreeViewColumn();
			rend = new Gtk.CellRendererText();
			_dobCol.Title = "Date of Birth";
			_dobCol.Sizing = Gtk.TreeViewColumnSizing.Fixed;
			_dobCol.PackStart(rend,false);
			_dobCol.SetCellDataFunc(rend, new Gtk.TreeCellDataFunc(ListModelUtil.RenderIndividualDob));
			_dobCol.Resizable = false;
			_dobCol.MinWidth = 150;

			_preferedCol = new Gtk.TreeViewColumn();
			rend = new Gtk.CellRendererToggle();
			((Gtk.CellRendererToggle)rend).Radio = true;
			((Gtk.CellRendererToggle)rend).Mode = Gtk.CellRendererMode.Activatable;
			((Gtk.CellRendererToggle)rend).Activatable = true;
			((Gtk.CellRendererToggle)rend).Toggled += new Gtk.ToggledHandler(Prefered_Toggled);
			_preferedCol.Title = "Prefered";
			_preferedCol.Sizing = Gtk.TreeViewColumnSizing.Fixed;
			_preferedCol.PackStart(rend,true);
			_preferedCol.Resizable = false;
			_preferedCol.MinWidth = 50;
			_preferedCol.SetCellDataFunc(rend, new Gtk.TreeCellDataFunc(ListModelUtil.RenderPreferedSpouse));
			_preferedCol.Visible = false;
			
			IndividualTreeView.AppendColumn(_nameCol);
			IndividualTreeView.AppendColumn(_dobCol);
			IndividualTreeView.AppendColumn(_preferedCol);
			
			IndividualTreeView.Model = null;
			_filterText = string.Empty;
			
			Gtk.TreeSelection selection = IndividualTreeView.Selection;
			selection.Changed += new EventHandler(OnSelection_Changed);
			
			SoundexCheckBox.Toggled += new EventHandler(OnSoundexCheckBox_Toggled);
		}
				
		#endregion
				
		#region Properties
		
		public IndividualListModel ListModel
		{
			get { return _listModel; }
			set 
			{ 
				_listModel = value;
				_preferedCol.Visible = (_listModel is SpouseListModel);
			}
		}
			
		#endregion
		
		#region IGedcomView
		
		#region Properties
		
		public GedcomDatabase Database
		{
			get { return _database; }
			set
			{
				_database = value;
				IndividualTreeView.Model = null;
				if (_listModel == null)
				{
					_listModel = new IndividualListModel();
					_listModel.FilterIndividual += new EventHandler<IndividualListModel.FilterArgs>(FilterIndividual);
				}
				_listModel.Database = _database;
				
				IndividualTreeView.Model = _listModel.Adapter;
				TotalLabel.Text = string.Format("({0})", _listModel.Count);
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
		
		public GedcomIndividualRecord Husband
		{
			get { return null; }
		}
		
		public GedcomIndividualRecord Wife
		{
			get { return null; }
		}
		
		#endregion
		
		#region Events
		
		public event EventHandler<IndividualArgs> MoreInformation;
		public event EventHandler<FamilyArgs> MoreFamilyInformation;
		public event EventHandler<SpouseSelectArgs> SpouseSelect;
		public event EventHandler<SourceCitationArgs> ShowSourceCitation;
		public event EventHandler<IndividualArgs> SelectNewChild;
		public event EventHandler<IndividualArgs> SelectNewSpouse;
		public event EventHandler<ScrapBookArgs> ShowScrapBook;
		public event EventHandler<IndividualArgs> ShowName;
		public event EventHandler<IndividualArgs> DeleteIndividual;
		public event EventHandler<FactArgs> MoreFactInformation;
		public event EventHandler<NoteArgs> SelectNewNote;
		
		#endregion
		
		#region Methods
			
		public void ClearView()
		{
			NameEntry.Text = string.Empty;
			FilterComboBox.Active = 0;
			IndividualTreeView.Selection.UnselectAll();
		}
		
		public void SaveView()
		{
			
		}
		
		#endregion
		
		#endregion
		
		#region Events
		
		public event EventHandler RecordChanged;
		public event EventHandler<IndividualListModel.FilterArgs> Filter;
		
		#endregion
		
		#region EventHandlers
		
		private void FilterIndividual(object sender, IndividualListModel.FilterArgs e)
		{
			if (Filter != null)
			{
				Filter(this, e);			
			}
		}
		
		private bool Refilter()
		{
			if (_listModel != null)
			{
				_listModel.FilterText = _filterText;
				_listModel.FilterType = (IndividualListModel.NameFilterType)FilterComboBox.Active;
				_listModel.Soundex = SoundexCheckBox.Active;
				_listModel.DoFill();
				TotalLabel.Text = string.Format("({0})", _listModel.Count);
			}	
			
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
			Gtk.TreeSelection selection = IndividualTreeView.Selection;
			
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

		protected void OnSoundexCheckBox_Toggled(object sender, EventArgs e)
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

		protected void Prefered_Toggled(object sender, Gtk.ToggledArgs e)
		{			
			SpouseListModel spouseList = (SpouseListModel)_listModel;

			Gtk.TreePath path = new Gtk.TreePath(e.Path);
			Gtk.TreeIter iter;
			if (_listModel.GetIter(out iter, path))
			{
				GedcomIndividualRecord indi = _listModel.GetValue(iter, 0);
				spouseList.SetPrefered(indi.XRefID);
			}
		}
		
		#endregion
		
		#region Methods
		
		#endregion
	}
}
