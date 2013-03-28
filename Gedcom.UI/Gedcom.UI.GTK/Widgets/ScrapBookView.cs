/*
 *  $Id: ScrapBookView.cs 194 2008-11-10 20:39:37Z davek $
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
using System.IO;
using Gedcom.UI.Common;

namespace Gedcom.UI.GTK.Widgets
{
	
	
	[System.ComponentModel.Category("Gedcom.UI.GTK.Widgets")]
	[System.ComponentModel.ToolboxItem(true)]
	public partial class ScrapBookView : Gtk.Bin
	{
		#region Variables
		
		protected MultimediaRecordListModel _recListModel;
		protected MultimediaFileListModel _listModel;
		
		#endregion
		
		#region Constructors
		
		public ScrapBookView()
		{
			_recListModel = new MultimediaRecordListModel();
			_listModel = new MultimediaFileListModel();
			
			this.Build();
			
			
			Gtk.TreeViewColumn titleCol = new Gtk.TreeViewColumn();
			Gtk.CellRenderer rend = new Gtk.CellRendererText();
			titleCol.Title = "Record Title";
			titleCol.PackStart(rend,true);
			titleCol.SetCellDataFunc(rend, new Gtk.TreeCellDataFunc(ListModelUtil.RenderMultimediaRecordTitle));
								
			RecordTreeView.AppendColumn(titleCol);
			
			RecordTreeView.Model = null;
			
			Gtk.TreeSelection selection = RecordTreeView.Selection;
			selection.Changed += new EventHandler(OnRecordSelection_Changed);

			Gtk.TreeViewColumn buttonCol = new Gtk.TreeViewColumn();
			GtkCellRendererButton butRend = new GtkCellRendererButton();
			butRend.StockId = "gtk-open";
			buttonCol.PackStart(butRend,true);
							
			Gtk.TreeViewColumn filenameCol = new Gtk.TreeViewColumn();
			rend = new Gtk.CellRendererText();
			filenameCol.Title = "Filename";
			filenameCol.PackStart(rend,true);
			filenameCol.SetCellDataFunc(rend, new Gtk.TreeCellDataFunc(ListModelUtil.RenderMultimediaFilename));

			Gtk.TreeViewColumn typeCol = new Gtk.TreeViewColumn();
			rend = new Gtk.CellRendererText();
			typeCol.Title = "Type";
			typeCol.PackStart(rend,true);
			typeCol.SetCellDataFunc(rend, new Gtk.TreeCellDataFunc(ListModelUtil.RenderMultimediaType));
			
			Gtk.TreeViewColumn formatCol = new Gtk.TreeViewColumn();
			rend = new Gtk.CellRendererText();
			formatCol.Title = "Format";
			formatCol.PackStart(rend,true);
			formatCol.SetCellDataFunc(rend, new Gtk.TreeCellDataFunc(ListModelUtil.RenderMultimediaFormat));
									
			FileTreeView.AppendColumn(buttonCol);
			FileTreeView.AppendColumn(filenameCol);
			FileTreeView.AppendColumn(typeCol);
			FileTreeView.AppendColumn(formatCol);
			
			FileTreeView.Model = null;
			
			selection = FileTreeView.Selection;
			selection.Changed += new EventHandler(OnSelection_Changed);
		}
		
		#endregion
		
		#region Properties
		
		public GedcomDatabase Database
		{
			get { return _recListModel.Database; }
			set 
			{ 
				_recListModel.Database = value;
				_listModel.Database = value;
				NotesView.Database = value;
			}
		}
						
		public GedcomRecord Record
		{
			get 
			{
				GedcomRecord rec;
				
				if (ShowRecordList)
				{
					rec = _recListModel.Record;
				}
				else
				{
					rec = _listModel.Record;
				}
				
				return rec;
			}
			set 
			{ 
				GedcomMultimediaRecord multi;
				
				Sensitive = (value != null);

				TitleEntry.Text = string.Empty;
			
				DetailsNotebook.Page = 0;
				
				if (ShowRecordList)
				{
					DetailsNotebook.Sensitive = false;
					RemoveRecordButton.Sensitive = false;
				
					_recListModel.Record = value;
					if (value != null && _recListModel.Record.Multimedia.Count > 0)
					{
						multi = (GedcomMultimediaRecord)Database[_recListModel.Record.Multimedia[0]];
						
						_listModel.Record = multi;
						TitleEntry.Text = multi.Title;
						TitleEntry.Sensitive = true;
						NotesView.Record = multi;
					}
					else
					{
						_listModel.Record = null;
						TitleEntry.Sensitive = false;
						NotesView.Record = null;
					}
					
					RecordTreeView.Model = _recListModel.Adapter;
					FileTreeView.Model = _listModel.Adapter;
				}
				else
				{
					multi = value as GedcomMultimediaRecord;
					
					_listModel.Record = value;
					FileTreeView.Model = _listModel.Adapter;
					
					if (multi != null)
					{
						TitleEntry.Text = multi.Title;
						TitleEntry.Sensitive = true;
					}
					NotesView.Record = multi;
				}
			}
		}
		
		
		public GedcomMultimediaRecord SelectedRecord
		{
			get
			{
				return (GedcomMultimediaRecord)_listModel.Record;
			}
		}
		
		public GedcomMultimediaFile SelectedFile
		{
			get 
			{
				GedcomMultimediaFile file = null;
				
				Gtk.TreeSelection selection = FileTreeView.Selection;
			
				Gtk.TreeModel model;
				Gtk.TreeIter iter;
				if (selection.GetSelected(out model, out iter))
				{
					file = (GedcomMultimediaFile)model.GetValue(iter, 0);
				
				}
			
				return file;
			}
		}
		
		public bool ShowRecordList
		{
			get { return vbox2.Visible; }
			set
			{
				vbox2.Visible = value;
				vseparator1.Visible = (value & ShowFileList);
			}
		}
		
		public bool ShowFileList
		{
			get { return DetailsNotebook.Visible; }
			set
			{
				DetailsNotebook.Visible = value;
				vseparator1.Visible = (value & ShowRecordList);
				
				// FIXME: hide buttons on record list if files aren't shown,
				// this is a hack
				hbuttonbox2.Visible = value;
				
				// vbox2 needs to expand/not expand
				bool expand = !value;
				hbox1.SetChildPacking(vbox2, expand, true, 0, Gtk.PackType.Start);
			}
		}
		
		#endregion
		
		#region Events
		
		public event EventHandler FileSelected;
		public event EventHandler FileDeSelected;
		public event EventHandler<MultimediaFileArgs> OpenFile;
		
		public event EventHandler<SourceCitationArgs> ShowSourceCitation;
		
		public event EventHandler<MultimediaFileArgs> AddFile;
		
		public event EventHandler<NoteArgs> SelectNewNote;
		
		#endregion
		
		#region Event Handlers
			
		protected void OnRecordSelection_Changed(object sender, System.EventArgs e)
		{
			Gtk.TreeSelection selection = RecordTreeView.Selection;
			
			Gtk.TreeModel model;
			Gtk.TreeIter iter;
			
			TitleEntry.Text = string.Empty;
			
			if (selection.GetSelected(out model, out iter))
			{
				GedcomMultimediaRecord multi = (GedcomMultimediaRecord)model.GetValue(iter, 0);
				
				_listModel.Record = multi;
				
				DetailsNotebook.Sensitive = true;
				RemoveRecordButton.Sensitive = true;
				TitleEntry.Text = multi.Title;
				TitleEntry.Sensitive = true;
				NotesView.Record = multi;
			}
			else
			{
				_listModel.Record = null;
				TitleEntry.Sensitive = false;
				NotesView.Record = null;
				
				DetailsNotebook.Sensitive = false;
				RemoveRecordButton.Sensitive = false;
			}
			
		}
		
		protected void OnSelection_Changed(object sender, System.EventArgs e)
		{
			DoRecordSelectionChange();
		}

		protected virtual void OnAddFileButton_Clicked (object sender, System.EventArgs e)
		{
			if (AddFile != null)
			{
				MultimediaFileArgs args = new MultimediaFileArgs();
				
				AddFile(this, args);
				
				string filename = args.Filename;
				
				if (!string.IsNullOrEmpty(filename))
				{
					GedcomMultimediaRecord obje = (GedcomMultimediaRecord)SelectedRecord;
					
					obje.AddMultimediaFile(filename);
				
					_listModel.Record = _listModel.Record;
				
					DoRecordSelectionChange();
				}
			}
			
		}

		protected virtual void OnRemoveFileButton_Clicked (object sender, System.EventArgs e)
		{
			GedcomMultimediaRecord obje = SelectedRecord;
			GedcomMultimediaFile file = SelectedFile;
			
			obje.Files.Remove(file);
			
			_listModel.Record = _listModel.Record;
			
			DoRecordSelectionChange();
		}

		protected virtual void OnAddRecordButton_Clicked (object sender, System.EventArgs e)
		{
			GedcomMultimediaRecord obje = new GedcomMultimediaRecord(Database);
			
			Record.Multimedia.Add(obje.XRefID);
			
			// force refresh
			Record = Record;
		}

		protected virtual void OnRemoveRecordButton_Clicked (object sender, System.EventArgs e)
		{
			GedcomMultimediaRecord obje = SelectedRecord;
			
			Record.Multimedia.Remove(obje.XRefID);
			
			// force refresh
			Record = Record;
		}
		
		// GTK# being annoying this time, need this so the
		// handler is connected before the widget handles the event
		[GLib.ConnectBefore]
		protected virtual void OnFileTreeView_ButtonPressEvent(object sender, Gtk.ButtonPressEventArgs e)
		{
			Gdk.EventButton ev = e.Event;
			
			if (ev.Button == 1)
			{
				int x = (int)ev.X;
				int y = (int)ev.Y;
				Gtk.TreePath path;
								
				if (FileTreeView.GetPathAtPos(x, y, out path))
				{
					Gtk.TreeViewColumn buttonCol = FileTreeView.Columns[0];
					if (x < buttonCol.Width)
					{
						Gtk.TreeIter iter;
						Gtk.CellRenderer[] rends = buttonCol.CellRenderers;
						
						if (_listModel.GetIter(out iter, path))
						{
							GedcomMultimediaFile file = (GedcomMultimediaFile)_listModel.GetValue(iter, 0);
							
							int butX = 0;
							int i = 0;
							foreach (GtkCellRendererButton rend in rends)
							{
								if (x < (butX + rend.Width))
								{
									break;
								}
								butX += rend.Width;
								i ++;
							}
							// open column
							if (i == 0 && OpenFile != null)
							{
								MultimediaFileArgs args = new MultimediaFileArgs();
								args.Filename = file.Filename;
								OpenFile(this,args);
							}
						}
					}
				}
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
		
		protected void DoRecordSelectionChange()
		{		
			Gtk.TreeSelection selection = FileTreeView.Selection;
			
			Gtk.TreeModel model;
			Gtk.TreeIter iter;
			if (selection.GetSelected(out model, out iter))
			{
				//GedcomMultimediaFile file = (GedcomMultimediaFile)model.GetValue(iter, 0);
				
				if (FileSelected != null)
				{
					FileSelected(this, EventArgs.Empty);
				}
			}
			else if (FileDeSelected != null)
			{
				FileDeSelected(this, EventArgs.Empty);
			}
		}

		public void SaveView()
		{
			if (ShowFileList && TitleEntry.Sensitive)
			{
				GedcomMultimediaRecord multi = (GedcomMultimediaRecord)_listModel.Record;
				if (multi != null)
				{
					multi.Title = TitleEntry.Text;
				}
				
				NotesView.Save();
			}
		}
		
		#endregion
	}
}
