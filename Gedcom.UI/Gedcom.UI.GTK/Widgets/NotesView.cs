/*
 *  $Id: NotesView.cs 189 2008-10-10 14:16:10Z davek $
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
using Gedcom.UI.Common;

namespace Gedcom.UI.GTK.Widgets
{
	
	[System.ComponentModel.Category("Gedcom.UI.GTK.Widgets")]
	[System.ComponentModel.ToolboxItem(true)]
	public partial class NotesView : Gtk.Bin
	{
		#region Variables
		
		protected NoteListModel _model;
		
		protected GedcomDatabase _database;
		protected GedcomRecord _record;
		protected GedcomNoteRecord _note;
		
		// bit of a hack, GedcomSourceRecord has an extra set
		// of notes for the DATA
		protected bool _dataNotes;
		
		private bool _loading = false;
		
		#endregion
		
		#region Constructors
		
		public NotesView()
		{
			this.Build();
			
			Gtk.TreeViewColumn buttonCol = new Gtk.TreeViewColumn();
			GtkCellRendererButton butRend = new GtkCellRendererButton();
			butRend.StockId = "gtk-index";
			buttonCol.PackStart(butRend,true);
			
			butRend = new GtkCellRendererButton();
			butRend.StockId = "gtk-remove";
			buttonCol.PackStart(butRend,true);
			
			Gtk.TreeViewColumn noteCountCol = new Gtk.TreeViewColumn();
			Gtk.CellRenderer rend = new Gtk.CellRendererText();
			noteCountCol.Title = "No.";
			noteCountCol.PackStart(rend,true);
			noteCountCol.SetCellDataFunc(rend, new Gtk.TreeCellDataFunc(ListModelUtil.RenderNoteCount));
			
			Gtk.TreeViewColumn noteCol = new Gtk.TreeViewColumn();
			rend = new Gtk.CellRendererText();
			noteCol.Title = "Text";
			noteCol.PackStart(rend,true);
			noteCol.SetCellDataFunc(rend, new Gtk.TreeCellDataFunc(ListModelUtil.RenderNote));
			
			NotesTreeView.AppendColumn(buttonCol);
			NotesTreeView.AppendColumn(noteCountCol);
			NotesTreeView.AppendColumn(noteCol);
			
			Model = new NoteListModel();
			
			Gtk.TreeSelection selection = NotesTreeView.Selection;
			selection.Changed += new EventHandler(OnNoteSelection_Changed);
		}
		
		#endregion
		
		#region Properties
		
		public GedcomDatabase Database
		{
			get { return _database; }
			set
			{
				_database = value;
				_model.Database = value;
			}
		}
		
		public GedcomRecord Record
		{
			get { return _record; }
			set
			{
				_record = value;
				
				_loading = true;
				
				Clear();
				
				_model.Record = _record;
				
				if (_record != null)
				{			
					Gtk.TreeIter iter;
					
					if (_model.GetIterFirst(out iter))
					{
						NotesTreeView.Selection.SelectIter(iter);	
					}
				}
				
				_loading = false;
			}
		}
		
		public GedcomNoteRecord Note
		{
			get { return _note; }
			set
			{
				_note = value;
				
				NotesTextView.Editable = (_note != null);
				
				if (NotesTextView.Editable)
				{
					NotesTextView.Buffer.Text = _note.Text;
				}
				else
				{
					NotesTextView.Buffer.Text = string.Empty;
				}
				
			}
		}
		
		public NoteListModel Model
		{
			get { return _model; }
			set
			{
				_model = value;
				NotesTreeView.Model = _model;
			}
		}
		
		public bool DataNotes
		{
			get { return _dataNotes; }
			set 
			{ 
				_dataNotes = value;
				_model.DataNotes = value;
			}
		}
		
		public bool ListOnly
		{
			get { return !scrolledwindow1.Visible; }
			set
			{
				scrolledwindow1.Visible = !value;
				vbuttonbox2.Visible = !value;
			}
		}
		
		public bool NoteOnly
		{
			get { return !hbox1.Visible; }
			set
			{
				scrolledwindow1.Visible = value;
				hbox1.Visible = ! value;
			}
		}
		
		#endregion
		
		#region Events
		
		public event EventHandler<SourceCitationArgs> ShowSourceCitation;
		public event EventHandler<NoteArgs> SelectNewNote;
		
		#endregion
		
		#region EventHandlers
		
		protected virtual void OnNewNoteButton_Clicked(object sender, System.EventArgs e)
		{
			GedcomNoteRecord note = new GedcomNoteRecord(_database);
			
			DoAddNote(note);
		}
				
		protected void OnNoteSelection_Changed(object sender, System.EventArgs e)
		{
			Gtk.TreeSelection selection = NotesTreeView.Selection;
			
			if (_note != null)
			{
				Save();
			}
			
			Gtk.TreeModel model;
			Gtk.TreeIter iter;
			
			GedcomNoteRecord note = null;
			
			if (selection.GetSelected(out model, out iter))
			{
				note = (GedcomNoteRecord)model.GetValue(iter, 0);
			}
						
			Note = note;
		}
		
		// GTK# being annoying this time, need this so the
		// handler is connected before the widget handles the event
		[GLib.ConnectBefore]
		protected virtual void OnNotesTreeView_ButtonPressEvent(object sender, Gtk.ButtonPressEventArgs e)
		{
			Gdk.EventButton ev = e.Event;
			
			if (ev.Button == 1)
			{
				int x = (int)ev.X;
				int y = (int)ev.Y;
				Gtk.TreePath path;
								
				if (NotesTreeView.GetPathAtPos(x, y, out path))
				{
					Gtk.TreeViewColumn buttonCol = NotesTreeView.Columns[0];
					if (x < buttonCol.Width)
					{
						Gtk.TreeIter iter;
						Gtk.CellRenderer[] rends = buttonCol.CellRenderers;
						
						if (_model.GetIter(out iter, path))
						{
							GedcomNoteRecord note = _model.GetValue(iter, 0) as GedcomNoteRecord;
									
							int i = 0;
							bool buttonClicked = false;
							foreach (GtkCellRendererButton rend in rends)
							{
								if (x >= rend.X && x <= rend.X + rend.Width)
								{
									buttonClicked = true;
									break;
								}
								i ++;
							}
							if (buttonClicked)
							{
								switch (i)
								{
									// source citation column
									case 0:
										if (ShowSourceCitation != null)
										{
											SourceCitationArgs args = new SourceCitationArgs();
											args.Record = note;
											ShowSourceCitation(this,args);
										}
										break;
										
									// remove column
									case 1:
										if (_record.RecordType != GedcomRecordType.Source || !_dataNotes)
										{
											_record.Notes.Remove(note.XRefID);
										}
										else
										{
											GedcomSourceRecord sourceRecord = (GedcomSourceRecord)_record;
											sourceRecord.DataNotes.Remove(note.XRefID);
										}
										
										note.Delete();
										
										_model.Remove(ref iter);
										break;
								}
							}
							
						}
					}
				}
			}
		}
				
		protected virtual void OnAddNoteButton_Clicked (object sender, System.EventArgs e)
		{
			if (SelectNewNote != null)
			{
				NoteArgs args = new NoteArgs();
				
				SelectNewNote(this, args);
				
				if (args.Note != null)
				{
					DoAddNote(args.Note);
				}
			}
		}
		
		#endregion
		
		#region Methods
		
		public void Clear()
		{
			Note = null;
			if (_model != null)
			{
				_model.Clear();	
			}
		}
		
		public void Save()
		{
		    if ((!_loading) && (_note != null))
			{				
				_note.Database = _database;
				_note.Text = NotesTextView.Buffer.Text;
			}	
		}

		private void DoAddNote(GedcomNoteRecord note)
		{
			if (_record.RecordType != GedcomRecordType.Source || !_dataNotes)
			{
				_record.Notes.Add(note.XRefID);
			}
			else
			{
				GedcomSourceRecord sourceRecord = (GedcomSourceRecord)_record;
				sourceRecord.DataNotes.Add(note.XRefID);
			}
						
			Gtk.TreeIter iter = _model.Append();
			_model.SetValue(iter, 0, note);
			
			NotesTreeView.Selection.SelectIter(iter);	
		}
		
		#endregion
		
	}
}
