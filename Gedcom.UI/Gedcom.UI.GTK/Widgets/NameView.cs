/*
 *  $Id: NameView.cs 194 2008-11-10 20:39:37Z davek $
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
	public partial class NameView : Gtk.Bin
	{
		#region Variables
		
		protected GedcomDatabase _database;
		protected GedcomRecord _record;
		
		protected GedcomName _name;
		
		protected Gtk.ListStore _names;
		
		protected bool _loading;
		
		#endregion
		
		#region Constructors
		
		public NameView()
		{
			this.Build();
		
			_names = new Gtk.ListStore(new Type[] { typeof(GedcomName) });
			
			Gtk.TreeViewColumn nameCol = new Gtk.TreeViewColumn();
			Gtk.CellRenderer rend = new Gtk.CellRendererText();
			nameCol.Title = "Name";
			nameCol.Sizing = Gtk.TreeViewColumnSizing.Fixed;
			nameCol.PackStart(rend,true);
			nameCol.SetCellDataFunc(rend, new Gtk.TreeCellDataFunc(ListModelUtil.RenderName));
			nameCol.Resizable = true;
			nameCol.MinWidth = 200;
			
			NamesTreeView.AppendColumn(nameCol);
			
			Gtk.TreeSelection selection = NamesTreeView.Selection;
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
				NotesView.Database = value;
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

				ClearView();

				if (_record.RecordType != GedcomRecordType.Individual)
				{
					throw new Exception("Only Individual Records can be used");
				}
				
				_loading = true;
				
				GedcomIndividualRecord indi = _record as GedcomIndividualRecord;
				
				_names.Clear();
				foreach (GedcomName name in indi.Names)
				{
					Gtk.TreeIter iter = _names.Append();
					_names.SetValue(iter, 0, (object)name);
				}
				
				if (indi.Names.Count > 0)
				{
					NamesTreeView.Model = _names;
					NamesTreeView.Selection.SelectPath(new Gtk.TreePath("0"));
				}
				
				_loading = false;
				
				notebook1.Page = 0;
			}
		}
		
		#endregion
		
		#region Events
		
		public event EventHandler<SourceCitationArgs> ShowSourceCitation;
		public event EventHandler<NoteArgs> SelectNewNote;
		
		#endregion
		
		#region Event Handlers
		
		protected void OnSelection_Changed(object sender, System.EventArgs e)
		{
			Gtk.TreeSelection selection = NamesTreeView.Selection;

			SaveView();
			
			Gtk.TreeModel model;
			Gtk.TreeIter iter;
			if (selection.GetSelected(out model, out iter))
			{
				_name = (GedcomName)model.GetValue(iter, 0);
			}
			else
			{
				_name = null;
			}
			
			ClearView();
			FillView();
		}
		
		protected virtual void OnNameSourceButton_Clicked (object sender, System.EventArgs e)
		{
			if (ShowSourceCitation != null)
			{
				SourceCitationArgs args = new SourceCitationArgs();
				args.Record = _name;
				ShowSourceCitation(this,args);
			}
		}
		
		protected virtual void OnNotesView_ShowSourceCitation (object sender, SourceCitationArgs e)
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
				
		#endregion
		
		#region Methods
		
		protected void ClearView()
		{
			PrefixComboBoxEntry.Entry.Text = string.Empty;
			GivenEntry.Text = string.Empty;
			SurnamePrefixComboBoxEntry.Entry.Text = string.Empty;
			SurnameEntry.Text = string.Empty;
			SuffixComboBoxEntry.Entry.Text = string.Empty;
			NicknameEntry.Text = string.Empty;
			PreferedCheckbox.Active = false;
			
			NotesView.Clear();
		}
		
		protected void FillView()
		{
			if (_name != null)
			{
				if (!string.IsNullOrEmpty(_name.Prefix))
				{
					PrefixComboBoxEntry.Entry.Text = _name.Prefix;
				}
				if (!string.IsNullOrEmpty(_name.Given))
				{
					GivenEntry.Text = _name.Given;
				}
				if (!string.IsNullOrEmpty(_name.SurnamePrefix))
				{
					SurnamePrefixComboBoxEntry.Entry.Text = _name.SurnamePrefix;
				}
				if (!string.IsNullOrEmpty(_name.Surname))
				{
					SurnameEntry.Text = _name.Surname;
				}
				if (!string.IsNullOrEmpty(_name.Suffix))
				{
					SuffixComboBoxEntry.Entry.Text = _name.Suffix;
				}
				if (!string.IsNullOrEmpty(_name.Nick))
				{
					NicknameEntry.Text = _name.Nick;
				}
				
				NotesView.Record = _name;

				GedcomIndividualRecord indi = _record as GedcomIndividualRecord;

				int count = indi.Names.Count;
				PreferedCheckbox.Sensitive = (count > 1);
				PreferedCheckbox.Active = _name.PreferedName || (count == 1);
			}
		}
		
		public void SaveView()
		{
			if (!_loading)
			{
				GedcomIndividualRecord indi = _record as GedcomIndividualRecord;

				if (_name == null)
				{				
					_name = new GedcomName();
					indi.Names.Add(_name);
				}			
				
				int count = indi.Names.Count;
				
				_name.Prefix = PrefixComboBoxEntry.Entry.Text;
				_name.Given = GivenEntry.Text;
				_name.SurnamePrefix = SurnamePrefixComboBoxEntry.Entry.Text;
				_name.Surname = SurnameEntry.Text;
				_name.Suffix = SuffixComboBoxEntry.Entry.Text;
				_name.Nick = NicknameEntry.Text;
				
				if (PreferedCheckbox.Active || count == 1)
				{
					indi.SetPreferedName(_name);
				}
			}
		}

		#endregion
	}
}
