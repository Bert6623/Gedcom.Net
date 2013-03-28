/*
 *  $Id: SourceCitationList.cs 189 2008-10-10 14:16:10Z davek $
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
	public partial class SourceCitationList : Gtk.Bin
	{
		#region Variables
				
		protected GedcomDatabase _database;
		protected GedcomRecord _record;
			
		protected SourceCitationListModel _listModel;
				
		#endregion
		
		#region Constructors
		
		public SourceCitationList()
		{
			this.Build();
			
			_listModel = new SourceCitationListModel();
			
			Gtk.TreeViewColumn buttonCol = new Gtk.TreeViewColumn();
			GtkCellRendererButton butRend = new GtkCellRendererButton();
			butRend.StockId = "gtk-remove";
			buttonCol.PackStart(butRend,true);
			
			Gtk.TreeViewColumn citationCountCol = new Gtk.TreeViewColumn();
			Gtk.CellRenderer rend = new Gtk.CellRendererText();
			citationCountCol.Title = "No.";
			citationCountCol.PackStart(rend,true);
			citationCountCol.SetCellDataFunc(rend, new Gtk.TreeCellDataFunc(ListModelUtil.RenderCitationCount));
			
			Gtk.TreeViewColumn citationCol = new Gtk.TreeViewColumn();
			rend = new Gtk.CellRendererText();
			citationCol.Title = "Text";
			citationCol.PackStart(rend,true);
			citationCol.SetCellDataFunc(rend, new Gtk.TreeCellDataFunc(ListModelUtil.RenderSourceCitation));
			
			SourceCitationTreeView.AppendColumn(buttonCol);
			SourceCitationTreeView.AppendColumn(citationCountCol);
			SourceCitationTreeView.AppendColumn(citationCol);
					
			SourceCitationTreeView.Model = _listModel.Adapter;
			
			Gtk.TreeSelection selection = SourceCitationTreeView.Selection;
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
				SourceCitationTreeView.Model = null;
				_listModel.Database = _database;
				
				SourceCitationTreeView.Model = _listModel.Adapter;
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
				
				//SourceCitationTreeView.Model = null;
								
				_record = value;
				_listModel.Record = _record;
				
				//SourceCitationTreeView.Model = _listModel.Adapter;
				
				SelectFirst();
			}
		}
		
		public SourceCitationListModel ListModel
		{
			get { return _listModel; }
			set { _listModel = value; }
		}
		
		#endregion
		
		#region Events
		
		public event EventHandler RecordChanged;
		
		#endregion
		
		#region EventHandlers
				
		protected void OnSelection_Changed(object sender, System.EventArgs e)
		{
			Gtk.TreeSelection selection = SourceCitationTreeView.Selection;
			
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
		
		// GTK# being annoying this time, need this so the
		// handler is connected before the widget handles the event
		[GLib.ConnectBefore]
		protected virtual void OnSourceCitationTreeView_ButtonPressEvent(object sender, Gtk.ButtonPressEventArgs e)
		{
			Gdk.EventButton ev = e.Event;
			
			if (ev.Button == 1)
			{
				int x = (int)ev.X;
				int y = (int)ev.Y;
				Gtk.TreePath path;
								
				if (SourceCitationTreeView.GetPathAtPos(x, y, out path))
				{
					Gtk.TreeViewColumn buttonCol = SourceCitationTreeView.Columns[0];
					if (x < buttonCol.Width)
					{
						Gtk.TreeIter iter;
						
						if (_listModel.GetIter(out iter, path))
						{
							GedcomSourceCitation citation = _listModel.GetValue(iter, 0) as GedcomSourceCitation;
							
							_listModel.Record.Sources.Remove(citation);
				
							_listModel.ItemRemoved(path);
						}
					}
				}
			}
		}
		
		protected virtual void OnNewSourceCitationButton_Clicked(object sender, System.EventArgs e)
		{
			GedcomSourceCitation citation = new GedcomSourceCitation();
			
			citation.Level = _listModel.Record.Level + 1;
			citation.Database = _database;
			
			_listModel.Record.Sources.Add(citation);
			_listModel.ItemInserted();
			
			Gtk.TreeIter iter;
			int i = _listModel.List.Count - 1;
			if (_listModel.GetIter(out iter, i))
			{
				SourceCitationTreeView.Selection.SelectIter(iter);
			}
			
		}
		
		#endregion
		
		#region Methods
		
		private void SelectFirst()
		{
			if (_record != null && _record.Sources.Count > 0)
			{
				Gtk.TreeIter iter;
				if (_listModel.GetIter(out iter, 0))
				{
					Gtk.TreeSelection selection = SourceCitationTreeView.Selection;
					selection.SelectIter(iter);
				}
			}	
		}
		
		#endregion
	}
}
