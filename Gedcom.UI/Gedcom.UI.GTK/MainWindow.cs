/*
 *  $Id: MainWindow.cs 202 2008-12-10 11:42:10Z davek $
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
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics; 
using System.IO;
using Gtk;

using Gedcom;
using GedcomParser;

using Gedcom.UI.GTK;
using Gedcom.UI.GTK.Widgets;
using Gedcom.UI.Common;

using Gedcom.Reports;

public partial class MainWindow: Gtk.Window
{
	#region Variables
		
	protected Gtk.AboutDialog _aboutDialog = null;
	
	protected IGedcomView[] _views;
	protected Gtk.Action[] _viewActions;
	
	protected GedcomDatabase _database;
	protected GedcomRecord _record;
	protected string _gedcomFile = string.Empty;
	
	protected IGedcomView _currentView;
	
	const int SourcesPage = 3;
	const int MultimediaPage = 4;
	const int RepositoryPage = 5;
	const int PedigreePage = 6;
	const int NotePage = 7;
	const int DuplicatesPage = 8;
	

	private Gtk.ActionGroup _viewGroup;
	private Gtk.ActionGroup _toolsGroup;
	private Gtk.ActionGroup _reportsGroup;
	private Gtk.ActionGroup _defaultGroup;

	private Gtk.RecentManager _recentManager; 
	private Gtk.RecentChooserMenu _toolbarRecentMenu;
	
	#endregion
	
	#region Constructors
	
	public MainWindow (): base (MainClass.AppName)
	{
		this.Build();
				
		_views = new IGedcomView[]
		{
			SummaryViewView,
			FamilyViewView,
			IndividualViewView
		};
		_viewActions = new Gtk.Action[]
		{
			SummaryViewAction,
			FamilyViewAction,
			IndividualViewAction,
			SourceViewAction,
			MultimediaViewAction,
			RepositoryViewAction,
			PedigreeViewAction,
			NoteViewAction
		};
		
		foreach (IGedcomView view in _views)
		{
			view.MoreInformation += new EventHandler<IndividualArgs>(OnMoreInformation);
			view.MoreFamilyInformation += new EventHandler<FamilyArgs>(OnMoreFamilyInformation);
		    view.SpouseSelect += new EventHandler<SpouseSelectArgs>(OnSpouseSelect);
		    view.ShowSourceCitation += new EventHandler<SourceCitationArgs>(OnShowSourceCitation);
			view.SelectNewChild += new EventHandler<IndividualArgs>(OnSelectNewChild);
			view.SelectNewSpouse += new EventHandler<IndividualArgs>(OnSelectNewSpouse);
			view.SelectNewNote += new EventHandler<NoteArgs>(OnSelectNewNote);
			view.ShowScrapBook += new EventHandler<ScrapBookArgs>(OnShowScrapBook);
			view.ShowName += new EventHandler<IndividualArgs>(OnShowName);
			view.DeleteIndividual += new EventHandler<IndividualArgs>(OnDeleteIndividual);
			view.MoreFactInformation += new EventHandler<FactArgs>(OnMoreFactInformation);
		}

		SourceViewView.ShowScrapBook += new EventHandler<ScrapBookArgs>(OnShowScrapBook);
		SourceViewView.ShowSourceCitation += new EventHandler<SourceCitationArgs>(OnShowSourceCitation);
		MultimediaViewView.AddFile += new EventHandler<MultimediaFileArgs>(OnMultimediaView_AddFile);
		MultimediaViewView.OpenFile += new EventHandler<MultimediaFileArgs>(OnMultimediaView_OpenFile);
		MultimediaViewView.ShowSourceCitation += new EventHandler<SourceCitationArgs>(OnShowSourceCitation);
		RepositoryViewView.ShowSourceCitation += new EventHandler<SourceCitationArgs>(OnShowSourceCitation);
		NotesViewView.ShowSourceCitation += new EventHandler<SourceCitationArgs>(OnShowSourceCitation);
		
		// FIXME: not working via property in designer show all kicking in?
		NotesViewView.NoteOnly = true;
		
		ViewNotebook.ShowTabs = false;		
		ViewNotebook.Page = 0;
		_currentView = _views[0];
		
		ViewNotebook.Sensitive = false;
		Merge.Sensitive = false;
		Save.Sensitive = false;
		SaveAs.Sensitive = false;
		PropertiesAction.Sensitive = false;


		_viewGroup = View.ActionGroup;
		_toolsGroup = Tools.ActionGroup;
		_reportsGroup = Reports.ActionGroup;
		_defaultGroup = New.ActionGroup;		
		
		// get all report related actions, make insensitive
		foreach (Gtk.Action action in _reportsGroup.ListActions())
		{
			action.Sensitive = false;
		}
		
		// get all tools related actions, make insensitive
		foreach (Gtk.Action action in _toolsGroup.ListActions())
		{
			action.Sensitive = false;
		}
		
		// get all view related actions, make insensitive
		foreach (Gtk.Action action in _viewGroup.ListActions())
		{
			action.Sensitive = false;
		}
		SummaryViewAction.ActionGroup.Sensitive = true;

		NewIndividual.Sensitive = false;
		NewMedia.Sensitive = false;
		NewRepository.Sensitive = false;
		NewSource.Sensitive = false;
		NewNote.Sensitive = false;	

		// dropdown actions, not supported by stetic
		Gtk.UIManager manager = new Gtk.UIManager();
		GtkDropDownAction newDropDown = new GtkDropDownAction("New", "New", string.Empty, Gtk.Stock.New);
		newDropDown.Activated += OnNew_Activated;
		newDropDown.UIManager = manager;
		newDropDown.MenuPath = "/newpopup";
		manager.InsertActionGroup(_defaultGroup, 0);
		manager.AddUiFromString("<ui><popup name='newpopup'><menuitem action='NewDatabase'/><menuitem action='NewIndividual'/><menuitem action='NewMedia'/><menuitem action='NewRepository'/><menuitem action='NewSource'/><menuitem action='NewNote'/></popup></ui>");
		Gtk.Widget newButton = newDropDown.CreateToolItem();
		newDropDown.ConnectProxy(newButton);
		
		toolbar1.Insert((Gtk.ToolItem)newButton, 0);
		
		// gtk recent actions, not supported by stetic or gtk# 2.10
		_recentManager = Gtk.RecentManager.Default;
		//_recentManager.Limit = 4; // FIXME: don't hardcode max recent file count
		
		Gtk.RecentChooserMenu menu = new RecentChooserMenu(_recentManager);
		menu.LocalOnly = true;
		menu.SortType = RecentSortType.Mru;
		
		Gtk.RecentFilter filter = new RecentFilter();
		filter.Name = "GEDCOM files";
		filter.AddMimeType("application/x-gedcom");
		menu.AddFilter(filter);		
		menu.Filter = filter;
		menu.ItemActivated += RecentMenu_ItemActivated;
		_toolbarRecentMenu = menu;
		
		GtkDropDownAction recentDropDown = new GtkDropDownAction("Open", "Open", "Open File", Gtk.Stock.Open);
		recentDropDown.Menu = menu;
		recentDropDown.Activated += OnOpen_Activated;
		Gtk.Widget openButton = recentDropDown.CreateToolItem();
		recentDropDown.ConnectProxy(openButton);
		
		toolbar1.Insert((Gtk.ToolItem)openButton, 1);
	}
	
	#endregion
	
	#region Properties
	
	public GedcomRecord Record
	{
		get { return _record; }
		set
		{
			if (_record != null && _currentView != null)
			{
				_currentView.SaveView();	
			}
			
			_record = value;
			if (_currentView != null)
			{
				_currentView.Record = value;	
			}
		}
	}
	
	#endregion
	
	#region EventHandlers
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected virtual void OnQuit(object sender, System.EventArgs e)
	{
		Application.Quit();
	}

	protected virtual void OnAbout(object sender, System.EventArgs e)
	{
		if (_aboutDialog == null)
		{
			_aboutDialog = new Gtk.AboutDialog();
			_aboutDialog.ProgramName = MainClass.AppDisplayName;
			_aboutDialog.Version = MainClass.AppVersion;
			_aboutDialog.Authors = MainClass.AppAuthors;
			_aboutDialog.Comments = "Gedcom.NET GTK based UI";
			_aboutDialog.Copyright = "© 2007-2008 David A Knight";
			_aboutDialog.License = @"This program is free software; you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation; either version 2 of the License, or
 (at your option) any later version.
 
 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU General Public License for more details.
 
 You should have received a copy of the GNU General Public License
 along with this program; if not, write to the Free Software
 Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA";			
			
			_aboutDialog.DeleteEvent += new Gtk.DeleteEventHandler(OnAbout_DeleteEvent);
			_aboutDialog.Destroyed += new EventHandler(OnAbout_Destroyed);
			_aboutDialog.Response += new Gtk.ResponseHandler(OnAbout_Response);
		}
		
		_aboutDialog.Show();
		_aboutDialog.GdkWindow.Raise();
	}
	
	private void OnAbout_DeleteEvent(object sender, Gtk.DeleteEventArgs e)
	{
		_aboutDialog.Hide();	
	}
	
	private void OnAbout_Destroyed(object sender, EventArgs e)
	{
		_aboutDialog = null;	
	}
	
	private void OnAbout_Response(object sender, Gtk.ResponseArgs e)
	{
		_aboutDialog.Hide();	
	}

	protected virtual void OnSwitchPage(object o, Gtk.SwitchPageArgs args)
	{
		DoSwitchPage(args.PageNum, true);
	}

	protected virtual void OnListIndividuals_Activated(object sender, System.EventArgs e)
	{
		if (_currentView != null)
		{
			_currentView.SaveView();
		}
		
		IndividualListDialog listDialog = new IndividualListDialog();
		
		if (sender is Gtk.Dialog)
		{
			listDialog.TransientFor = (Gtk.Window)sender;
		}
		else
		{
			listDialog.TransientFor = this;
		}
		
		listDialog.Database = _database;
		listDialog.Record = _record;
		
		listDialog.Show();
		
		listDialog.Response += new Gtk.ResponseHandler(OnListIndividuals_Response);
	}
	
	protected void OnListIndividuals_Response(object sender, Gtk.ResponseArgs e)
	{
		IndividualListDialog listDialog = sender as IndividualListDialog;
		bool selected = false;
		
		if (e.ResponseId == Gtk.ResponseType.Apply)
		{
			if (listDialog.Record != null)
			{
				Record = listDialog.Record;
				selected = true;
			}
			
		}
		else if (e.ResponseId == Gtk.ResponseType.Ok)
		{
			// Create new indi
			GedcomIndividualRecord indi = new GedcomIndividualRecord(_database);
			
			Record = indi;
			selected = true;
		}
		
		listDialog.Destroy();
		
		if (_currentView == SummaryViewView && selected)
		{
			Gtk.RadioAction action = _viewActions[1] as Gtk.RadioAction;
			action.Activate();
		}
	}
	
	protected virtual void OnListMediaItems_Activated(object sender, System.EventArgs e)
	{
		if (_currentView != null)
		{
			_currentView.SaveView();
		}
		MultimediaViewView.SaveView();
		
		MultimediaListDialog listDialog = new MultimediaListDialog();
		
		if (sender is Gtk.Dialog)
		{
			listDialog.TransientFor = (Gtk.Window)sender;
		}
		else
		{
			listDialog.TransientFor = this;
		}
		
		listDialog.Database = _database;
		listDialog.Record = _record;
		
		listDialog.Show();
		
		listDialog.Response += new Gtk.ResponseHandler(OnListMediaItems_Response);
	}
	
	protected void OnListMediaItems_Response(object sender, Gtk.ResponseArgs e)
	{
		MultimediaListDialog listDialog = sender as MultimediaListDialog;
		
		if (e.ResponseId == Gtk.ResponseType.Apply)
		{
			if (listDialog.Record != null)
			{
				MultimediaViewView.Database = _database;
				MultimediaViewView.Record = listDialog.Record;
				ViewNotebook.Page = MultimediaPage;
			}
		}
		else if (e.ResponseId == Gtk.ResponseType.Ok)
		{
			// Create new multimedia item
			GedcomMultimediaRecord media = new GedcomMultimediaRecord(_database);
			
			MultimediaViewView.Database = _database;
			MultimediaViewView.Record = media;
			ViewNotebook.Page = MultimediaPage;
		}
	
		listDialog.Destroy();
	}

	
	protected virtual void OnListRepositories_Activated(object sender, System.EventArgs e)
	{
		if (_currentView != null)
		{
			_currentView.SaveView();
		}
		
		RepositoryListDialog listDialog = new RepositoryListDialog();
		
		if (sender is Gtk.Dialog)
		{
			listDialog.TransientFor = (Gtk.Window)sender;
		}
		else
		{
			listDialog.TransientFor = this;
		}
		
		listDialog.Database = _database;
		listDialog.Record = _record;
		
		listDialog.Show();
		
		listDialog.Response += new Gtk.ResponseHandler(OnListRepositories_Response);
	}
	
	protected void OnListRepositories_Response(object sender, Gtk.ResponseArgs e)
	{
		RepositoryListDialog listDialog = sender as RepositoryListDialog;
				
		if (e.ResponseId == Gtk.ResponseType.Apply)
		{
			if (listDialog.Record != null)
			{
				RepositoryViewView.Database = _database;
				RepositoryViewView.Record = listDialog.Record;
				ViewNotebook.Page = RepositoryPage;
			}
		}
		else if (e.ResponseId == Gtk.ResponseType.Ok)
		{
			// Create new repository
			GedcomRepositoryRecord repo = new GedcomRepositoryRecord(_database);
			
			RepositoryViewView.Database = _database;
			RepositoryViewView.Record = repo;
			ViewNotebook.Page = RepositoryPage;
		}
		listDialog.Destroy();
	}
	
	protected virtual void OnListSources_Activated(object sender, System.EventArgs e)
	{
		if (_currentView != null)
		{
			_currentView.SaveView();
		}
		
		SourceListDialog listDialog = new SourceListDialog();
		
		if (sender is Gtk.Dialog)
		{
			listDialog.TransientFor = (Gtk.Window)sender;
		}
		else
		{
			listDialog.TransientFor = this;
		}
		
		listDialog.Database = _database;
		listDialog.Record = _record;
		
		listDialog.Show();
		
		listDialog.Response += new Gtk.ResponseHandler(OnListSources_Response);
	}
	
	protected void OnListSources_Response(object sender, Gtk.ResponseArgs e)
	{
		SourceListDialog listDialog = sender as SourceListDialog;
				
		if (e.ResponseId == Gtk.ResponseType.Apply)
		{
			if (listDialog.Record != null)
			{
				SourceViewView.Database = _database;
				SourceViewView.Record = listDialog.Record;
				ViewNotebook.Page = SourcesPage;
			}
			
		}
		else if (e.ResponseId == Gtk.ResponseType.Ok)
		{
			// Create new source
			GedcomSourceRecord source = new GedcomSourceRecord(_database);
			
			SourceViewView.Database = _database;
			SourceViewView.Record = source;
			ViewNotebook.Page = SourcesPage;
		}
		listDialog.Destroy();
	}
	
	protected void OnMoreInformation(object sender, IndividualArgs e)
	{
		GedcomIndividualRecord indi = e.Indi;
		
		IndividualMoreDialog moreDialog = new IndividualMoreDialog();
		
		if (sender is Gtk.Dialog)
		{
			moreDialog.TransientFor = (Gtk.Window)sender;
		}
		else
		{
			moreDialog.TransientFor = this;
		}
		
		moreDialog.Database = _database;
		moreDialog.Record = indi;
		
		moreDialog.ShowSourceCitation += new EventHandler<SourceCitationArgs>(OnShowSourceCitation);
		moreDialog.ShowScrapBook += new EventHandler<ScrapBookArgs>(OnShowScrapBook);
		moreDialog.ShowName += new EventHandler<IndividualArgs>(OnShowName);
		moreDialog.DeleteIndividual += new EventHandler<IndividualArgs>(OnDeleteIndividual);
		moreDialog.MoreFactInformation += new EventHandler<FactArgs>(OnMoreFactInformation);
		moreDialog.SelectNewNote += new System.EventHandler<NoteArgs>(OnSelectNewNote);
		
		moreDialog.Response += new Gtk.ResponseHandler(OnMoreInformation_Response);
		
		moreDialog.ShowAll();
	}
	
	protected void OnMoreInformation_Response(object sender, Gtk.ResponseArgs e)
	{
		IndividualMoreDialog moreDialog = sender as IndividualMoreDialog;
		
		moreDialog.View.SaveView();
		
		RefreshView();
				
		moreDialog.Destroy();
	}
	
	protected void OnMoreFamilyInformation(object sender, FamilyArgs e)
	{
		GedcomFamilyRecord fam = e.Fam;
		
		FamilyMoreDialog moreDialog = new FamilyMoreDialog();
		
		if (sender is Gtk.Dialog)
		{
			moreDialog.TransientFor = (Gtk.Window)sender;
		}
		else
		{
			moreDialog.TransientFor = this;
		}
				
		moreDialog.Database = _database;
		moreDialog.Record = fam;
		
		moreDialog.ShowSourceCitation += new EventHandler<SourceCitationArgs>(OnShowSourceCitation);
		moreDialog.ShowScrapBook += new EventHandler<ScrapBookArgs>(OnShowScrapBook);
		moreDialog.MoreFactInformation += new EventHandler<FactArgs>(OnMoreFactInformation);
		moreDialog.SelectNewNote += new System.EventHandler<NoteArgs>(OnSelectNewNote);
		
		moreDialog.Response += new Gtk.ResponseHandler(OnMoreFamilyInformation_Response);
		
		moreDialog.ShowAll();
	}
	
	protected void OnMoreFamilyInformation_Response(object sender, Gtk.ResponseArgs e)
	{
		FamilyMoreDialog moreDialog = sender as FamilyMoreDialog;
		
		moreDialog.View.SaveView();
		
		RefreshView();
				
		moreDialog.Destroy();
	}
	
	protected void OnSpouseSelect(object sender, SpouseSelectArgs e)
	{
		IndividualListDialog listDialog = new IndividualListDialog();
		
		SpouseListModel listModel = new SpouseListModel();
		
		if (sender is Gtk.Dialog)
		{
			listDialog.TransientFor = (Gtk.Window)sender;
		}
		else
		{
			listDialog.TransientFor = this;
		}
				
		listModel.Database = _database;
		listModel.Record = e.Indi;
		
		listDialog.Title = "Select Spouse";
		listDialog.List.ListModel = listModel;
		listDialog.Database = _database;
		listDialog.Record = e.Indi;
		
		listDialog.Modal = true;
		
		int response = listDialog.Run();
		
		if (response == (int)Gtk.ResponseType.Apply)
		{
			if (listDialog.Record != e.Indi)
			{
				e.SelectedSpouse = listDialog.Record as GedcomIndividualRecord;
				e.Family = listModel.GetFamily(e.SelectedSpouse.XRefID);
			}
		}
		else if (response == (int)Gtk.ResponseType.Ok)
		{
			// Create new indi
			GedcomIndividualRecord indi = new GedcomIndividualRecord(_database);
			GedcomFamilyRecord fam = new GedcomFamilyRecord(_database, e.Indi, indi);
	
			e.SelectedSpouse = indi;
			e.Family = fam;
		}
		
		listDialog.Destroy();
	}
	
	protected void OnShowSourceCitation(object sender, SourceCitationArgs e)
	{
		if (e.Record == null)
		{
			// FIXME: do something
		}
		else
		{
			SourceCitationsDialog citationDialog = new SourceCitationsDialog();
					
			if (sender is Gtk.Dialog)
			{
				citationDialog.TransientFor = (Gtk.Window)sender;
			}
			else
			{
				citationDialog.TransientFor = this;
			}
			
			citationDialog.Database = _database;
			citationDialog.Record = e.Record;
			
			citationDialog.Response += new Gtk.ResponseHandler(OnShowSourceCitation_Response);
			citationDialog.ViewMasterSource += new EventHandler(OnViewMasterSource);
			citationDialog.ShowScrapBook += new EventHandler<ScrapBookArgs>(OnShowScrapBook);
			citationDialog.ShowSourceCitation += new EventHandler<SourceCitationArgs>(OnShowSourceCitation);
			citationDialog.SelectNewNote += new System.EventHandler<Gedcom.UI.Common.NoteArgs>(OnSelectNewNote);

			citationDialog.Show();
		}
	}
	
	protected void OnShowSourceCitation_Response(object sender, Gtk.ResponseArgs e)
	{
		SourceCitationsDialog citationDialog = sender as SourceCitationsDialog;
		
		citationDialog.SaveView();
		
		citationDialog.Destroy();
	}
	
	protected void OnSelectNewChild(object sender, IndividualArgs e)
	{
		IndividualListDialog listDialog = new IndividualListDialog();
		
		IndividualListModel listModel = new IndividualListModel();
		
		if (sender is Gtk.Dialog)
		{
			listDialog.TransientFor = (Gtk.Window)sender;
		}
		else
		{
			listDialog.TransientFor = this;
		}
		
		listModel.Database = _database;
		
		listDialog.Title = "Select Child";
		listDialog.List.ListModel = listModel;
		listDialog.Database = _database;
		listDialog.Record = e.Indi;
		
		listDialog.Modal = true;
		
		int response = listDialog.Run();
		
		if (response == (int)Gtk.ResponseType.Apply)
		{
			e.Indi = listDialog.Record as GedcomIndividualRecord;	
		}
		else if (response == (int)Gtk.ResponseType.Ok)
		{
			// Create new indi
			GedcomIndividualRecord indi = new GedcomIndividualRecord(_database);
			
			e.Indi = indi;
		}
		
		listDialog.Destroy();
	}
	
	protected void OnSelectNewSpouse(object sender, IndividualArgs e)
	{
		IndividualListDialog listDialog = new IndividualListDialog();
		
		IndividualListModel listModel = new IndividualListModel();
		
		if (sender is Gtk.Dialog)
		{
			listDialog.TransientFor = (Gtk.Window)sender;
		}
		else
		{
			listDialog.TransientFor = this;
		}
		
		listModel.Database = _database;
		
		listDialog.Title = "Select Spouse";
		listDialog.List.ListModel = listModel;
		listDialog.Database = _database;
		listDialog.Record = e.Indi;
		
		listDialog.Modal = true;
		
		int response = listDialog.Run();
		
		if (response == (int)Gtk.ResponseType.Apply)
		{
			e.Indi = listDialog.Record as GedcomIndividualRecord;	
		}
		else if (response == (int)Gtk.ResponseType.Ok)
		{
			// Create new indi
			GedcomIndividualRecord indi = new GedcomIndividualRecord(_database);
			
			e.Indi = indi;
		}
		
		listDialog.Destroy();
	}
	
	protected void OnSelectNewNote(object sender, NoteArgs e)
	{
		NoteListDialog listDialog = new NoteListDialog();
		
		if (sender is Gtk.Dialog)
		{
			listDialog.TransientFor = (Gtk.Window)sender;
		}
		else
		{
			listDialog.TransientFor = this;
		}
		
		listDialog.Database = _database;
		listDialog.Record = null;
		
		listDialog.Modal = true;
		
		int response = listDialog.Run();
		
		if (response == (int)Gtk.ResponseType.Apply)
		{
			e.Note = listDialog.Note;
		}
		else if (response == (int)Gtk.ResponseType.Ok)
		{
			// Create new note
			GedcomNoteRecord note = new GedcomNoteRecord(_database);
			
			e.Note = note;
		}
		
		listDialog.Destroy();
	}
	
	protected void OnShowScrapBook(object sender, ScrapBookArgs e)
	{
		ScrapBookDialog scrapBookDialog = new ScrapBookDialog();
		
		if (sender is Gtk.Dialog)
		{
			scrapBookDialog.TransientFor = (Gtk.Window)sender;
		}
		else
		{
			scrapBookDialog.TransientFor = this;
		}
		
		scrapBookDialog.Database = _database;
		scrapBookDialog.Record = e.Record;
		scrapBookDialog.Title = "Scrapbook";
		
		scrapBookDialog.Response += OnShowScrapBook_Response;
		scrapBookDialog.ShowSourceCitation += OnShowSourceCitation;
		scrapBookDialog.SelectNewNote += OnSelectNewNote;
				
		scrapBookDialog.Run();
	}
	
	protected void OnShowScrapBook_Response(object sender, Gtk.ResponseArgs e)
	{
		ScrapBookDialog scrapBookDialog = sender as ScrapBookDialog;
		
		if (e.ResponseId != 0)
		{
			scrapBookDialog.Destroy();
		}
	}
	
	protected void OnShowName(object sender, IndividualArgs e)
	{
		NameDialog nameDialog = new NameDialog();
		
		if (sender is Gtk.Dialog)
		{
			nameDialog.TransientFor = (Gtk.Window)sender;
		}
		else
		{
			nameDialog.TransientFor = this;
		}
		
		nameDialog.Database = _database;
		nameDialog.Record = e.Indi;
		nameDialog.Title = "Name(s)";
		
		nameDialog.Response += OnShowName_Response;
		nameDialog.ShowSourceCitation += new EventHandler<SourceCitationArgs>(OnShowSourceCitation);
		nameDialog.SelectNewNote += new System.EventHandler<NoteArgs>(OnSelectNewNote);
		
		nameDialog.Run();
	}
	
	protected void OnShowName_Response(object sender, Gtk.ResponseArgs e)
	{
		NameDialog nameDialog = sender as NameDialog;
		
		nameDialog.View.SaveView();
		
		if (e.ResponseId != 0)
		{
			nameDialog.Destroy();
			RefreshView();
		}
	}
	
	protected void OnDeleteIndividual(object sender, IndividualArgs e)
	{
		DeleteIndividualDialog deleteDialog = new DeleteIndividualDialog(e.Indi);
		
		if (sender is Gtk.Dialog)
		{
			deleteDialog.TransientFor = (Gtk.Window)sender;
		}
		else
		{
			deleteDialog.TransientFor = this;
		}
		
		deleteDialog.Modal = true;
		
		switch (deleteDialog.Run())
		{
			case (int)Gtk.ResponseType.Ok:
				e.Indi.Delete();
				
				// FIXME: select new individual
				if (e.Indi == _record)
				{
					_record = _database.Individuals[0];
					if (_currentView != null)
					{
						_currentView.Record = _record;
					}
				}
				// or just refresh the current view
				else
				{
					RefreshView();
				}
				break;
		}
		deleteDialog.Destroy();
	}
	
	protected void OnMultimediaView_AddFile(object sender, MultimediaFileArgs e)
	{
		FileSelectorDialog fileSelector = new FileSelectorDialog();
		fileSelector.Title = "Select Media File";
		fileSelector.AddFilter("image/*", "Images");
		fileSelector.AddFilter("video/*", "Videos");
		fileSelector.AddFilter("audio/*", "Audio");
		fileSelector.AddFilter("text/*", "Text Documents");
		fileSelector.FileSelected += new EventHandler(MultimediaView_AddFileSelected);
		fileSelector.UserData = e;
		fileSelector.Modal = true;
				
		fileSelector.Run();
	}
	
	protected void MultimediaView_AddFileSelected(object sender, EventArgs e)
	{
		FileSelectorDialog fileSelector = sender as FileSelectorDialog;	

		MultimediaFileArgs addFileArgs = (MultimediaFileArgs)fileSelector.UserData;
		
		addFileArgs.Filename = fileSelector.Filename;
	}
	
	protected void OnMultimediaView_OpenFile(object sender, MultimediaFileArgs e)
	{
		if (!string.IsNullOrEmpty(e.Filename))
		{
			Process process = new Process();
			
			process.StartInfo.FileName = e.Filename;
			process.StartInfo.UseShellExecute = true;
			
			FileInfo info = new FileInfo(_database.Name);
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
	
	protected void OnViewMasterSource(object sender, EventArgs e)
	{
		SourceCitationsDialog citationDialog = sender as SourceCitationsDialog;
		
		SourceViewView.Database = _database;
		SourceViewView.Record = citationDialog.MasterSource;
		ViewNotebook.Page = SourcesPage;	
	}
	
	protected void OnMoreFactInformation(object sender, FactArgs e)
	{
		FactDetailDialog factDetailDialog = new FactDetailDialog();
		
		if (sender is Gtk.Dialog)
		{
			factDetailDialog.TransientFor = (Gtk.Window)sender;
		}
		else
		{
			factDetailDialog.TransientFor = this;
		}
		
		factDetailDialog.Database = _database;
		factDetailDialog.Record = e.Event;
		
		factDetailDialog.Response += OnMoreFactInformation_Response;
		factDetailDialog.ShowSourceCitation += new EventHandler<SourceCitationArgs>(OnShowSourceCitation);
		factDetailDialog.ShowScrapBook += new EventHandler<ScrapBookArgs>(OnShowScrapBook);
		factDetailDialog.SelectNewNote += new System.EventHandler<Gedcom.UI.Common.NoteArgs>(OnSelectNewNote);
		
		factDetailDialog.Run();
	}
	
	protected void OnMoreFactInformation_Response(object sender, Gtk.ResponseArgs e)
	{
		FactDetailDialog factDetailDialog = (FactDetailDialog)sender;
		
		factDetailDialog.View.SaveView();
		
		if (e.ResponseId != 0)
		{
			factDetailDialog.Destroy();
			RefreshView();
		}
	}
	
	protected virtual void OnHusbandsParents_Activated(object sender, System.EventArgs e)
	{
		GedcomIndividualRecord indi = _currentView.Husband;
		
		if (indi != null && indi.ChildIn.Count > 0)
		{
			GedcomFamilyLink link = indi.ChildIn[0];
			GedcomFamilyRecord fam = _database[link.Family] as GedcomFamilyRecord;
			if (fam != null)
			{
				if (!string.IsNullOrEmpty(fam.Husband))
				{
					indi = _database[fam.Husband] as GedcomIndividualRecord;		
				}
				else if (!string.IsNullOrEmpty(fam.Wife))
				{
					indi = _database[fam.Wife] as GedcomIndividualRecord;
				}
				else
				{
					// FIXME: shouldn't be here, or should we?  why can't there
					// be a family record with children but parents not known?
					// create a parent as unknown /unknown/  ?
					
					throw new Exception("Family with no husband or wife");
				}
				
				if (indi != null)
				{
					Record = indi;	
				}
				else
				{
					System.Diagnostics.Debug.WriteLine("Husband/Wife in family points to non individual record");
				}
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("Family link points to non family record");	
			}
		}
	}

	protected virtual void OnWifesParents_Activated(object sender, System.EventArgs e)
	{
		GedcomIndividualRecord indi = _currentView.Wife;
		
		if (indi != null && indi.ChildIn.Count > 0)
		{
			GedcomFamilyLink link = indi.ChildIn[0];
			GedcomFamilyRecord fam = _database[link.Family] as GedcomFamilyRecord;
			if (fam != null)
			{
				if (!string.IsNullOrEmpty(fam.Husband))
				{
					indi = _database[fam.Husband] as GedcomIndividualRecord;		
				}
				else if (!string.IsNullOrEmpty(fam.Wife))
				{
					indi = _database[fam.Husband] as GedcomIndividualRecord;
				}
				else
				{
					// shouldn't be here
					throw new Exception("Family with no husband or wife");
				}
				
				if (indi != null)
				{
					Record = indi;	
				}
				else
				{
					System.Diagnostics.Debug.WriteLine("Husband/Wife in family points to non individual record");
				}
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("Family link points to non family record");	
			}
		}
	}
	
	protected virtual void OnNew_Activated (object sender, System.EventArgs e)
	{
		OnNewDatabase_Activated(sender, e);
	}
	
	protected virtual void OnNewDatabase_Activated(object sender, System.EventArgs e)
	{
		GedcomDatabase database;
		
		database = new GedcomDatabase();
		
		// create an initial person as we need one to work properly
		GedcomIndividualRecord indi = new GedcomIndividualRecord(database);
		
		database.Name = "Unsaved";
		
		SetGedcomDatabase(database);
	}
	
	protected virtual void OnOpen_Activated(object sender, System.EventArgs e)
	{
		FileSelectorDialog fileSelector = new FileSelectorDialog();
		fileSelector.Title = "Select File to Open";
		fileSelector.AddFilter("application/x-gedcom", "GEDCOM Files");
		
		fileSelector.FileSelected += new EventHandler(Open_FileSelected);
		fileSelector.Show();
	}
	
	protected void Open_FileSelected(object sender, EventArgs e)
	{
		FileSelectorDialog fileSelector = sender as FileSelectorDialog;	
		
		string filename = fileSelector.Filename;
		FileInfo info = new FileInfo(filename);
		
		// recent manager wants a uri
		string uri = string.Format("file://{0}", filename);
		
		Gtk.RecentData data = new RecentData();
		data.AppName = MainClass.AppName;
		data.AppExec = string.Format("{0} %f", MainClass.AppName);
		data.DisplayName = info.Name;
		data.IsPrivate = false;
		data.MimeType = "application/x-gedcom";
		
		_recentManager.AddFull(uri, data);
		
		DoReadGedcom(filename, false);		
	}
	
	protected virtual void OnMerge_Activated (object sender, System.EventArgs e)
	{
		FileSelectorDialog fileSelector = new FileSelectorDialog();
		fileSelector.Title = "Select File to Merge";
		fileSelector.AddFilter("application/x-gedcom", "GEDCOM Files");
		
		fileSelector.FileSelected += new EventHandler(Merge_FileSelected);
		fileSelector.Show();
	}
	
	protected void Merge_FileSelected(object sender, EventArgs e)
	{
		FileSelectorDialog fileSelector = sender as FileSelectorDialog;	
		
		DoReadGedcom(fileSelector.Filename, true);		
	}

	
	protected virtual void OnSave_Activated(object sender, System.EventArgs e)
	{
		if (!string.IsNullOrEmpty(_gedcomFile))
		{
			DoSaveGedcom(_gedcomFile);		
		}
		else
		{
			SaveAs.Activate();
		}
	}
	
	protected virtual void OnSaveAs_Activated(object sender, System.EventArgs e)
	{
		FileSelectorDialog fileSelector = new FileSelectorDialog();
		fileSelector.Title = "Select File to Save As";
		fileSelector.AddFilter("application/x-gedcom", "GEDCOM Files");
		fileSelector.SaveDialog = true;
		
		fileSelector.FileSelected += new EventHandler(SaveAs_FileSelected);
		fileSelector.Run();
	}
	
	protected void SaveAs_FileSelected(object sender, EventArgs e)
	{
		FileSelectorDialog fileSelector = sender as FileSelectorDialog;	
		
		DoSaveGedcom(fileSelector.Filename);		
	}
	
	protected virtual void OnFamilyViewActionChanged(object sender, Gtk.ChangedArgs e)
	{
		Gtk.RadioAction action = e.Current;
		if (ViewNotebook.Page != action.Value)
		{
			DoSwitchPage((uint)ViewNotebook.Page, false);
			ViewNotebook.Page = action.Value;
		}
	}
	
	protected virtual void OnIndividualReportWebPage_Activated(object sender, System.EventArgs e)
	{
		if (_currentView != null)
		{
			_currentView.SaveView();
			
			// view may have changed the record so get what the view thinks
			// the current record is
			_record = _currentView.Record;
		}
		
		GedcomIndividualReport report = new GedcomIndividualReport();
		
		report.Database = _database;
		report.Record = _record;
		report.BaseXsltName = "IndividualReport";
		
		string filename = report.CreateReport();
		
		FileSelectorDialog fileSelector = new FileSelectorDialog();
		fileSelector.Title = "Save Individual Report As...";
		fileSelector.AddFilter("text/html", "HTML Files");
		fileSelector.AddFilter("application/pdf", "PDF Files");
		fileSelector.AddFilter("image/svg+xml", "SVG Files");
		fileSelector.SaveDialog = true;
		fileSelector.FileSelected += new EventHandler(ReportSave_FileSelected);
		fileSelector.UserData = filename;
		
		fileSelector.Run();
	}

	protected void ReportSave_FileSelected(object sender, EventArgs e)
	{
		FileSelectorDialog fileSelector = sender as FileSelectorDialog;
		
		FileInfo info = new FileInfo((string)fileSelector.UserData);
		info.CopyTo(fileSelector.Filename, true);
		info.Delete();
		
		// FIXME: report status
	}

	protected virtual void OnDecendantsReportWebPage_Activated(object sender, System.EventArgs e)
	{
		if (_currentView != null)
		{
			_currentView.SaveView();
				
			// view may have changed the record so get what the view thinks
			// the current record is
			_record = _currentView.Record;
		}
		
		GedcomIndividualReport report = new GedcomIndividualReport();
		
		report.Database = _database;
		report.Record = _record;
		report.DecendantGenerations = - int.MaxValue;
		report.BaseXsltName = "Descendants";
		
		string filename = report.CreateReport();
		
		FileSelectorDialog fileSelector = new FileSelectorDialog();
		fileSelector.Title = "Save Decendants Report As...";
		fileSelector.AddFilter("text/html", "HTML Files");
		fileSelector.AddFilter("application/pdf", "PDF Files");
		fileSelector.AddFilter("image/svg+xml", "SVG Files");
		fileSelector.SaveDialog = true;
		fileSelector.FileSelected += new EventHandler(ReportSave_FileSelected);
		fileSelector.UserData = filename;
		
		fileSelector.Run();
	}
	
	protected virtual void OnFamilyGroupSheetReportWebPage_Activated(object sender, System.EventArgs e)
	{
		if (_currentView != null)
		{
			_currentView.SaveView();
				
			// view may have changed the record so get what the view thinks
			// the current record is
			_record = _currentView.Record;
		}
		
		GedcomIndividualReport report = new GedcomIndividualReport();
		
		report.Database = _database;
		report.Record = _record;
		report.AncestorGenerations = 1; // need to get the parents of the father/mother
		report.DecendantGenerations = - 2; // need 2 rather than 1 to pull in spouse for children
		report.BaseXsltName = "FamilyGroupReport";
		
		GedcomIndividualRecord indi = _record as GedcomIndividualRecord;
		if (indi != null)
		{
			GedcomFamilyRecord fam = indi.GetFamily();
			if (fam != null)
			{
				report.XrefID = fam.XRefID;
			}
		}
		
		string filename = report.CreateReport();
		
		FileSelectorDialog fileSelector = new FileSelectorDialog();
		fileSelector.Title = "Save Family Group Sheet Report As...";
		fileSelector.AddFilter("text/html", "HTML Files");
		fileSelector.AddFilter("application/pdf", "PDF Files");
		fileSelector.AddFilter("image/svg+xml", "SVG Files");
		fileSelector.SaveDialog = true;
		fileSelector.FileSelected += new EventHandler(ReportSave_FileSelected);
		fileSelector.UserData = filename;
		
		fileSelector.Run();
	}

	protected virtual void OnFindDuplicates_Activated (object sender, System.EventArgs e)
	{
		ViewNotebook.Page = DuplicatesPage;
	}
	
	protected virtual void OnNewIndividual_Activated (object sender, System.EventArgs e)
	{
		// Create new indi
		GedcomIndividualRecord indi = new GedcomIndividualRecord(_database);
			
		Record = indi;
		
		if (_currentView == SummaryViewView)
		{
			Gtk.RadioAction action = _viewActions[1] as Gtk.RadioAction;
			action.Activate();
		}
	}

	protected virtual void OnNewMedia_Activated (object sender, System.EventArgs e)
	{
		// Create new multimedia item
		GedcomMultimediaRecord media = new GedcomMultimediaRecord(_database);
		
		MultimediaViewView.Database = _database;
		MultimediaViewView.Record = media;
		ViewNotebook.Page = MultimediaPage;
	}

	protected virtual void OnNewRepository_Activated (object sender, System.EventArgs e)
	{
		// Create new repository
		GedcomRepositoryRecord repo = new GedcomRepositoryRecord(_database);
		
		RepositoryViewView.Database = _database;
		RepositoryViewView.Record = repo;
		ViewNotebook.Page = RepositoryPage;
	}

	protected virtual void OnNewSource_Activated (object sender, System.EventArgs e)
	{
		// Create new source
		GedcomSourceRecord source = new GedcomSourceRecord(_database);
		
		SourceViewView.Database = _database;
		SourceViewView.Record = source;
		ViewNotebook.Page = SourcesPage;
	}
	
	protected virtual void OnNewNote_Activated (object sender, System.EventArgs e)
	{
		// Create new note
		GedcomNoteRecord note = new GedcomNoteRecord(_database);
		
		NotesViewView.Database = _database;
		NotesViewView.Record = note;
		NotesViewView.Note = note;
		ViewNotebook.Page = NotePage;
	}
	
	private void DoReadGedcom_Completed(object sender, RunWorkerCompletedEventArgs e)
	{	
		Gtk.Application.Invoke(delegate
		                       {
			GedcomRecordReader reader = (GedcomRecordReader)sender;
			
			if ((!e.Cancelled) && (e.Error == null))
			{
				SetGedcomDatabase(reader.Database);
			}
			StatusBar.Pop(0);
			ProgressBar.Fraction = 0;
			ProgressBar.Text = string.Empty;

			AppSettings.Instance.LastOpenedFile = reader.GedcomFile;
		});
	}
	
	private void DoMergeGedcom_Completed(object sender, RunWorkerCompletedEventArgs e)
	{
		Gtk.Application.Invoke(delegate
		                       {
			GedcomRecordReader reader = (GedcomRecordReader)sender;
		
			if ((!e.Cancelled) && (e.Error == null))
			{
				// FIXME: show ui to remove duplicates prior
				// to combining
				_database.Combine(reader.Database);
			}			
			StatusBar.Pop(0);
			ProgressBar.Fraction = 0;
			ProgressBar.Text = string.Empty;
		});
	}
	
	private void DoReadGedcom_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		GedcomRecordReader reader = (GedcomRecordReader)sender;

		Gtk.Application.Invoke(delegate
		                       {
			ProgressBar.Text = string.Format("{0} %", reader.Progress);
			ProgressBar.Fraction = (e.ProgressPercentage / 100.0);
		});
	}
	
	private void RecentMenu_ItemActivated(object sender, EventArgs e)
	{
		Gtk.RecentInfo info = _toolbarRecentMenu.CurrentItem;
		
		string filename = info.Uri;
		if (filename.StartsWith("file://"))
		{
			filename = filename.Substring("file://".Length);
		}
		
		DoReadGedcom(filename, false);
	}
	
	
	protected virtual void OnListOfNotesAction_Activated (object sender, System.EventArgs e)
	{
		if (_currentView != null)
		{
			_currentView.SaveView();
		}
		//NotesViewView.SaveView();
		
		NoteListDialog listDialog = new NoteListDialog();
		
		if (sender is Gtk.Dialog)
		{
			listDialog.TransientFor = (Gtk.Window)sender;
		}
		else
		{
			listDialog.TransientFor = this;
		}
		
		listDialog.Database = _database;
		listDialog.Record = _record;
		
		listDialog.Show();
		
		listDialog.Response += new Gtk.ResponseHandler(OnListNotes_Response);
	}
	
	protected void OnListNotes_Response(object sender, Gtk.ResponseArgs e)
	{
		NoteListDialog listDialog = sender as NoteListDialog;
		
		if (e.ResponseId == Gtk.ResponseType.Apply)
		{
			if (listDialog.Record != null)
			{
				NotesViewView.Database = _database;
				NotesViewView.Record = listDialog.Note;
				NotesViewView.Note = listDialog.Note;
				ViewNotebook.Page = NotePage;
			}
		}
		else if (e.ResponseId == Gtk.ResponseType.Ok)
		{
			// Create new multimedia item
			GedcomNoteRecord note = new GedcomNoteRecord(_database);
			
			NotesViewView.Database = _database;
			NotesViewView.Record = note;
			ViewNotebook.Page = NotePage;
		}
	
		listDialog.Destroy();
	}


	protected virtual void OnPropertiesAction_Activated (object sender, System.EventArgs e)
	{
		PropertiesDialog propertiesDialog = new PropertiesDialog();
		propertiesDialog.Database = _database;
		propertiesDialog.Response += new Gtk.ResponseHandler(OnPropertiesAction_Response);
		propertiesDialog.ShowSourceCitation += OnShowSourceCitation;
		propertiesDialog.SelectNewNote += OnSelectNewNote;
		
		propertiesDialog.TransientFor = this;
		
		propertiesDialog.Show();
	}

	protected void OnPropertiesAction_Response(object sender, Gtk.ResponseArgs e)
	{
		PropertiesDialog propertiesDialog = sender as PropertiesDialog;

		propertiesDialog.SaveView();
		
		propertiesDialog.Destroy();
	}

	protected void OnPreferences_Activated (object sender, System.EventArgs e)
	{
		PreferencesDialog preferencesDialog = new PreferencesDialog();
		preferencesDialog.Response += new Gtk.ResponseHandler(OnPreferencesAction_Response);
		
		preferencesDialog.TransientFor = this;
		
		preferencesDialog.Show();
	}

	protected void OnPreferencesAction_Response(object sender, Gtk.ResponseArgs e)
	{
		PreferencesDialog preferencesDialog = sender as PreferencesDialog;

		preferencesDialog.Destroy();
	}
	
	#endregion
	
	#region Methods

	private void DoSwitchPage(uint pageNum, bool toggleAction)
	{
		if (pageNum >= 0)
		{
			if (_currentView != null)
			{
				_currentView.SaveView();
				
				// view may have changed the record so get what the view thinks
				// the current record is
				_record = _currentView.Record;
			}
			
			// make views that don't go off the current individual record
			// to update when we switch tabs
			SourceViewView.SaveView();
			RepositoryViewView.SaveView();
			// FIXME: as multimedia objects can be edited via the scrapbook dialog
			// saving all the time here isn't right as the record may have changed
			MultimediaViewView.SaveView();
		
			// same as above but for notes
			NotesViewView.Save();
			
			// _currentView is only for gedcom views
			// we have other views such as source view, multimedia view etc.
			// which while part of the gedcom file don't represent an
			// individual or family
			if (pageNum < _views.Length)
			{
				IGedcomView view = _views[pageNum];
				
				view.Database = _database;
				view.Record = _record;
				
				_currentView = view;			
			}
			
			switch (pageNum)
			{
			
				case PedigreePage:
					// FIXME: yuk
					
					PedigreeViewView.Database = _database;
					PedigreeViewView.Record = _record;
					_currentView = PedigreeViewView;
					break;
				case SourcesPage:
					SourceViewView.Sensitive = (SourceViewView.Record != null); 
					break;
				case RepositoryPage:
					RepositoryViewView.Sensitive = (RepositoryViewView.Record != null);
					break;
				case MultimediaPage:
					MultimediaViewView.Sensitive = (MultimediaViewView.Record != null);
					break;
				case DuplicatesPage:
					// page 7 is for duplicates, it doesn't
					// take a record at all, so set _currentView to null
					this.DuplicateViewView.DatabaseA = _database;
					this.DuplicateViewView.DatabaseB = _database;
					_currentView = null;
					break;
				case NotePage:
					NotesViewView.Sensitive = (NotesViewView.Record != null);
					break;
			}
			
			// FIXME: yuk
			if (pageNum != DuplicatesPage && toggleAction)
			{
				Gtk.RadioAction action = _viewActions[pageNum] as Gtk.RadioAction;
				action.Activate();
			}
		}
	}

	// don't really want this to be public, but allows us to load the last opened
	// file on startup
	public void DoReadGedcom(string gedcomFile, bool merge)
	{
	
		StatusBar.Push(0, "Reading " + gedcomFile);
		
		BackgroundGedcomRecordReader reader = new BackgroundGedcomRecordReader();

		reader.Parser.AllowHyphenOrUnderscoreInTag = AppSettings.Instance.AllowHypenAndUnderscore;
		reader.Parser.AllowInformationSeparatorOne = AppSettings.Instance.AllowInformationSeparatorOneLoad;
		reader.Parser.AllowLineTabs = AppSettings.Instance.AllowLineTabsLoad;
		reader.Parser.AllowTabs = AppSettings.Instance.AllowTabsLoad;
		reader.Parser.ApplyConcContOnNewLineHack = AppSettings.Instance.ApplyConcContOnNewLineHack;
		reader.Parser.IgnoreInvalidDelim = AppSettings.Instance.IgnoreInvalidDelimeter;
		reader.Parser.IgnoreMissingTerms = AppSettings.Instance.IgnoreMissingLineTerminator;
		
		if (!merge)
		{
			reader.Completed += new RunWorkerCompletedEventHandler(DoReadGedcom_Completed);
		}
		else
		{
			reader.Completed += new RunWorkerCompletedEventHandler(DoMergeGedcom_Completed);
		}
		reader.ProgressChanged += new ProgressChangedEventHandler(DoReadGedcom_ProgressChanged);

		reader.ReadGedcom(gedcomFile);
	}
		
	private void SetGedcomDatabase(GedcomDatabase database)
	{
		_database = null;
		_record = null;
	
		_database = database;
			
		_record = _database.Individuals[0];
				
		if (_currentView != null)
		{
			_currentView.Database = _database;
					
			_currentView.Record = _record;
		}
		
		ViewNotebook.Sensitive = true;
		Merge.Sensitive = true;
		Save.Sensitive = true;
		SaveAs.Sensitive = true;
		PropertiesAction.Sensitive = true;
		
		// get all report related actions, make sensitive
		foreach (Gtk.Action action in _reportsGroup.ListActions())
		{
			action.Sensitive = true;
		}
		
		// get all tools related actions, make sensitive
		foreach (Gtk.Action action in _toolsGroup.ListActions())
		{
			action.Sensitive = true;
		}
		
		// get all view related actions, make sensitive
		foreach (Gtk.Action action in _viewGroup.ListActions())
		{
			action.Sensitive = true;	
		}		
		
		NewIndividual.Sensitive = true;
		NewMedia.Sensitive = true;
		NewRepository.Sensitive = true;
		NewSource.Sensitive = true;
		NewNote.Sensitive = true;
	}
	
	private void DoSaveGedcom(string gedcomFile)
	{
		if (_currentView != null)
		{
			_currentView.SaveView();	
		}
		
		GedcomRecordWriter writer = new GedcomRecordWriter();
		writer.Database = _database;
		writer.GedcomFile = gedcomFile;

		writer.ApplicationName = MainClass.AppDisplayName;
		writer.ApplicationSystemID = MainClass.AppDisplayName;
		writer.ApplicationVersion = MainClass.AppVersion;
		writer.Corporation = MainClass.AppAuthors[0];
		writer.CorporationAddress = MainClass.AppAddress;

		writer.AllowInformationSeparatorOneSave = AppSettings.Instance.AllowInformationSeparatorOneSave;
		writer.AllowLineTabsSave = AppSettings.Instance.AllowLineTabsSave;
		writer.AllowTabsSave = AppSettings.Instance.AllowTabsSave;
		
		writer.WriteGedcom();
		
		_gedcomFile = gedcomFile;
		
		_database.Name = _gedcomFile;
	}
	
	private void RefreshView()
	{
		// force refresh of current, FIXME: yuk
		GedcomRecord rec;
		
		// current view may have switch record
		if (_currentView != null)
		{
			rec = _currentView.Record;	
		}
		else
		{
			rec = _record;	
		}
		
		_record = null;
		Record = rec;	
	}

	#endregion
}
