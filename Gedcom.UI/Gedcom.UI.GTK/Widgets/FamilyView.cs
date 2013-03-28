/*
 *  $Id: FamilyView.cs 194 2008-11-10 20:39:37Z davek $
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
	public partial class FamilyView : Gtk.Bin, IGedcomView
	{
		#region Variables
			
		protected GedcomDatabase _database;
		protected GedcomRecord _record;
		
		
		protected GedcomFamilyRecord _famRecord;
		protected GedcomIndividualRecord _husband;
		protected GedcomIndividualRecord _wife;
		
		protected GedcomAssociation _husbandAssociation;
		protected GedcomAssociation _wifeAssociation;
		
		
		private ChildrenListModel _childrenListModel;
		#endregion
		
		#region Constructors
		
		public FamilyView()
		{
			this.Build();
						
			Gtk.TreeViewColumn buttonCol = new Gtk.TreeViewColumn();
			GtkCellRendererButton butRend = new GtkCellRendererButton();
			butRend.StockId = "gtk-index";
			buttonCol.PackStart(butRend,true);
			butRend = new GtkCellRendererButton();
			butRend.StockId = "gtk-remove";
			buttonCol.PackStart(butRend,true);
			butRend = new GtkCellRendererButton();
			butRend.StockId = "gtk-edit";
			buttonCol.PackStart(butRend,true);
						
			Gtk.TreeViewColumn nameCol = new Gtk.TreeViewColumn();
			Gtk.CellRenderer rend = new Gtk.CellRendererText();
			nameCol.Title = "Name";
			nameCol.PackStart(rend,true);
			nameCol.SetCellDataFunc(rend, new Gtk.TreeCellDataFunc(ListModelUtil.RenderIndividualName));
			nameCol.SortColumnId = 0;
			
			Gtk.TreeViewColumn sexCol = new Gtk.TreeViewColumn();
			rend = new Gtk.CellRendererText();
			sexCol.Title = "Sex";
			sexCol.PackStart(rend,true);
			sexCol.SetCellDataFunc(rend, new Gtk.TreeCellDataFunc(ListModelUtil.RenderIndividualSex));
			
			Gtk.TreeViewColumn dobCol = new Gtk.TreeViewColumn();
			rend = new Gtk.CellRendererText();
			dobCol.Title = "Date of Birth";
			dobCol.PackStart(rend,true);
			dobCol.SetCellDataFunc(rend, new Gtk.TreeCellDataFunc(ListModelUtil.RenderIndividualDob));
			
			Gtk.TreeViewColumn birthPlaceCol = new Gtk.TreeViewColumn();
			rend = new Gtk.CellRendererText();
			birthPlaceCol.Title = "Birth Place";
			birthPlaceCol.PackStart(rend,true);
			birthPlaceCol.SetCellDataFunc(rend, new Gtk.TreeCellDataFunc(ListModelUtil.RenderIndividualBirthPlace));
			
			Gtk.TreeViewColumn dodCol = new Gtk.TreeViewColumn();
			rend = new Gtk.CellRendererText();
			dodCol.Title = "Date of Death";
			dodCol.PackStart(rend,true);
			dodCol.SetCellDataFunc(rend, new Gtk.TreeCellDataFunc(ListModelUtil.RenderIndividualDod));
			
			Gtk.TreeViewColumn deathPlaceCol = new Gtk.TreeViewColumn();
			rend = new Gtk.CellRendererText();
			deathPlaceCol.Title = "Death Place";
			deathPlaceCol.PackStart(rend,true);
			deathPlaceCol.SetCellDataFunc(rend, new Gtk.TreeCellDataFunc(ListModelUtil.RenderIndividualDeathPlace));
			
			ChildrenTreeView.AppendColumn(buttonCol);
			ChildrenTreeView.AppendColumn(nameCol);
			ChildrenTreeView.AppendColumn(sexCol);
			ChildrenTreeView.AppendColumn(dobCol);
			ChildrenTreeView.AppendColumn(birthPlaceCol);
			ChildrenTreeView.AppendColumn(dodCol);
			ChildrenTreeView.AppendColumn(deathPlaceCol);
			
			HusbandSpouseView.AddButtonClicked += this.OnAddHusbandButton_Clicked;
			HusbandSpouseView.RemoveButtonClicked += this.OnRemoveHusbandButton_Clicked;
			HusbandSpouseView.DeleteButtonClicked += this.OnDeleteHusbandButton_Clicked;
			HusbandSpouseView.NameSourceButtonClicked += this.OnHusbandNameSourceButton_Clicked;
			HusbandSpouseView.DateBornSourceButtonClicked += this.OnHusbandDateBornSourceButton_Clicked;
			HusbandSpouseView.DateDiedSourceButtonClicked += this.OnHusbandDateDiedSourceButton_Clicked;
			HusbandSpouseView.MoreButtonClicked += this.OnHusbandMoreButton_Clicked;
			HusbandSpouseView.FamiliesButtonClicked += this.OnHusbandFamiliesButton_Clicked;
			HusbandSpouseView.ParentsButtonClicked += this.OnHusbandParentsButtonClicked;
			HusbandSpouseView.ScrapBookButtonClicked += this.OnHusbandScrapBookButtonClicked;
			HusbandSpouseView.NameButtonClicked += this.OnHusbandNameButtonClicked;
			
			WifeSpouseView.AddButtonClicked += this.OnAddWifeButton_Clicked;
			WifeSpouseView.RemoveButtonClicked += this.OnRemoveWifeButton_Clicked;
			WifeSpouseView.DeleteButtonClicked += this.OnDeleteWifeButton_Clicked;
			WifeSpouseView.NameSourceButtonClicked += this.OnWifeNameSourceButton_Clicked;
			WifeSpouseView.DateBornSourceButtonClicked += this.OnWifeDateBornSourceButton_Clicked;
			WifeSpouseView.DateDiedSourceButtonClicked += this.OnWifeDateDiedSourceButton_Clicked;
			WifeSpouseView.MoreButtonClicked += this.OnWifeMoreButton_Clicked;
			WifeSpouseView.FamiliesButtonClicked += this.OnWifeFamiliesButton_Clicked;
			WifeSpouseView.ParentsButtonClicked += this.OnWifeParentsButtonClicked;
			WifeSpouseView.ScrapBookButtonClicked += this.OnWifeScrapBookButtonClicked;
			WifeSpouseView.NameButtonClicked += this.OnWifeNameButtonClicked;
			
			MarriageView.ShowScrapBook += new EventHandler<ScrapBookArgs>(OnMarriageScrapbookButton_Clicked);
			MarriageView.ShowSourceCitation += new EventHandler<SourceCitationArgs>(OnMarriageSourceButton_Clicked);
			MarriageView.MoreFamilyInformation += new EventHandler<FamilyArgs>(OnMarriageMoreButton_Clicked);
		}
		
		#endregion
		
		#region Properties
	
		#endregion
				
		#region IGedcomView
		
		#region Properties
		
		public GedcomDatabase Database
		{
			get { return _database; }
			set 
			{ 
				_database = value;
				
				HusbandSpouseView.Database = _database;
				WifeSpouseView.Database = _database;
				MarriageView.Database = _database;
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
				
				GedcomFamilyRecord fam = null;
				
				if (_record.RecordType == GedcomRecordType.Individual)
				{
					// indi record, find first family
					GedcomIndividualRecord indi = _record as GedcomIndividualRecord;
					
					fam = indi.GetFamily();
					
					if (fam == null)
					{
						// FIXME: not in a family as a spouse, create a new
						// family record					
						fam = new GedcomFamilyRecord(_database, indi, null);
					}
				}
				else if (_record.RecordType != GedcomRecordType.Family)
				{
					throw new Exception("Invalid record type given to FamilyView");
				}
				
				// NOTE: _record will be whatever was passed in, family or
				// individual, FIXME: should always be individual passed in
				
				ClearView();
				
				_famRecord = fam;
				
				FillView();
			}
		}
		
		public GedcomIndividualRecord Husband
		{
			get { return _husband; }
		}
		
		public GedcomIndividualRecord Wife
		{
			get { return _wife; }
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
			HusbandSpouseView.ClearView();
			
			WifeSpouseView.ClearView();
			
			MarriageView.Clear();
			
			if (_childrenListModel != null)
			{
				_childrenListModel.Clear();
			}
		}
		
		public void SaveView()
		{
			if (_husband != null)
			{
				HusbandSpouseView.SaveView();
			}
			
			if (_wife != null)
			{
				WifeSpouseView.SaveView();
			}
				
			if (_famRecord != null)
			{
				MarriageView.Save();
			}
		}
		
		#endregion
		
		#endregion
		
		#region Event Handlers
					
		protected virtual void OnChildrenTreeView_RowActivated(object sender, Gtk.RowActivatedArgs e)
		{
			Gtk.TreePath path = e.Path;
			
			if (_childrenListModel != null)
			{
				Gtk.TreeIter iter;
				if (_childrenListModel.GetIter(out iter, path))
				{
					if (e.Column != ChildrenTreeView.Columns[0])
					{
						SaveView();
						
						Record = (GedcomRecord)_childrenListModel.GetValue(iter, 0);
					}
				}
			}		
		}
				
		protected virtual void OnHusbandMoreButton_Clicked(object sender, System.EventArgs e)
		{
			IndividualArgs args = new IndividualArgs();
			args.Indi = _husband;
			
			SaveView();
			
			if (MoreInformation != null)
			{
				MoreInformation(this, args);	
			}
		}

		protected virtual void OnWifeMoreButton_Clicked(object sender, System.EventArgs e)
		{
			IndividualArgs args = new IndividualArgs();
			args.Indi = _wife;
			
			SaveView();
			
			if (MoreInformation != null)
			{
				MoreInformation(this, args);	
			}
		}

		protected virtual void OnMarriageMoreButton_Clicked(object sender, FamilyArgs e)
		{
			SaveView();
			
			if (MoreFamilyInformation != null)
			{
				MoreFamilyInformation(this, e);	
			}
		}

		protected virtual void OnHusbandFamiliesButton_Clicked(object sender, System.EventArgs e)
		{
			SpouseSelectArgs args = new SpouseSelectArgs();
			args.Indi = _husband;
			args.Spouse = _wife;
			
			SaveView();
			
			if (SpouseSelect != null)
			{
				SpouseSelect(this,args);
				
				if (args.SelectedSpouse != null)
				{			
					if (_record == _wife)
					{
						_record = args.SelectedSpouse;	
					}
					
					_famRecord = args.Family;
					_wife = args.SelectedSpouse;
					FillView();
				}
			}
		}

		protected virtual void OnWifeFamiliesButton_Clicked(object sender, System.EventArgs e)
		{
			SpouseSelectArgs args = new SpouseSelectArgs();
			args.Indi = _wife;
			args.Spouse = _husband;
			
			SaveView();
			
			if (SpouseSelect != null)
			{
				SpouseSelect(this,args);
				
				if (args.SelectedSpouse != null)
				{
					if (_record == _husband)
					{
						_record = args.SelectedSpouse;	
					}
					
					_famRecord = args.Family;
					_husband = args.SelectedSpouse;
					FillView();
				}
			}
		}
		
		// GTK# being annoying this time, need this so the
		// handler is connected before the widget handles the event
		[GLib.ConnectBefore]
		protected virtual void OnChildrenTreeView_ButtonPressEvent(object sender, Gtk.ButtonPressEventArgs e)
		{
			Gdk.EventButton ev = e.Event;
			
			if (ev.Button == 1)
			{
				int x = (int)ev.X;
				int y = (int)ev.Y;
				Gtk.TreePath path;
												
				if (ChildrenTreeView.GetPathAtPos(x, y, out path))
				{
					Gtk.TreeViewColumn buttonCol = ChildrenTreeView.Columns[0];
					if (x < buttonCol.Width)
					{
						Gtk.TreeIter iter;
						Gtk.CellRenderer[] rends = buttonCol.CellRenderers;
						
						if (_childrenListModel.GetIter(out iter, path))
						{
							GedcomIndividualRecord child = _childrenListModel.GetValue(iter, 0) as GedcomIndividualRecord;
							
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
											args.Record = child.Birth;
											ShowSourceCitation(this,args);
										}
										break;
									// remove column
									case 1:
										RemoveChild(iter);	
										break;
									// more info column
									case 2:
										IndividualArgs args = new IndividualArgs();
										args.Indi = child;
									
										SaveView();
									
										if (MoreInformation != null)
										{
											MoreInformation(this, args);	
										}
										break;
								}
							}
						}
					}
				}
			}
		}

		protected virtual void OnHusbandNameSourceButton_Clicked(object sender, System.EventArgs e)
		{
			SaveView();
			
			if (ShowSourceCitation != null)
			{
				SourceCitationArgs args = new SourceCitationArgs();
				args.Record = _husband.GetName();
				ShowSourceCitation(this,args);
			}
		}

		protected virtual void OnHusbandDateBornSourceButton_Clicked(object sender, System.EventArgs e)
		{
			SaveView();
			
			if (ShowSourceCitation != null)
			{
				SourceCitationArgs args = new SourceCitationArgs();
				args.Record = _husband.Birth;
				ShowSourceCitation(this,args);
			}
		}

		protected virtual void OnHusbandDateDiedSourceButton_Clicked(object sender, System.EventArgs e)
		{
			SaveView();
			
			if (ShowSourceCitation != null)
			{
				SourceCitationArgs args = new SourceCitationArgs();
				args.Record = _husband.Death;
				ShowSourceCitation(this,args);
			}
		}

		protected virtual void OnWifeNameSourceButton_Clicked(object sender, System.EventArgs e)
		{
			SaveView();
			
			if (ShowSourceCitation != null)
			{
				SourceCitationArgs args = new SourceCitationArgs();
				args.Record = _wife.GetName();
				ShowSourceCitation(this,args);
			}
		}

		protected virtual void OnWifeDateBornSourceButton_Clicked(object sender, System.EventArgs e)
		{
			SaveView();
			
			if (ShowSourceCitation != null)
			{
				SourceCitationArgs args = new SourceCitationArgs();
				args.Record = _wife.Birth;
				ShowSourceCitation(this,args);
			}
		}

		protected virtual void OnWifeDateDiedSourceButton_Clicked(object sender, System.EventArgs e)
		{
			SaveView();
			
			if (ShowSourceCitation != null)
			{
				SourceCitationArgs args = new SourceCitationArgs();
				args.Record = _wife.Death;
				ShowSourceCitation(this,args);
			}
		}

		protected virtual void OnMarriageSourceButton_Clicked(object sender, SourceCitationArgs e)
		{
			SaveView();
			
			if (ShowSourceCitation != null)
			{
				ShowSourceCitation(this, e);
			}
		}

		protected virtual void OnAddChildButton_Clicked(object sender, System.EventArgs e)
		{
			SaveView();
			
			if (SelectNewChild != null)
			{
				IndividualArgs args = new IndividualArgs();
				SelectNewChild(this, args);
				
				GedcomIndividualRecord indi = args.Indi;
				
				if (_famRecord.AddChild(indi))
				{
					// GedcomFamilyRecord contains a list of children ids,
					// _childrenListModel is a list of GedcomIndividualRecords built from that,
					// so we still need to update the list
					_childrenListModel.List.Add(indi);
					_childrenListModel.ItemInserted();
				}
			}
		}
		
		protected virtual void OnNewChildButton_Clicked(object sender, System.EventArgs e)
		{
			// make sure husband/wife names are up to date
			SaveView();
			
			GedcomIndividualRecord indi = _famRecord.AddNewChild();
			
			// GedcomFamilyRecord contains a list of children ids,
			// _childrenListModel is a list of GedcomIndividualRecords built from that,
			// so we still need to update the list
			_childrenListModel.List.Add(indi);
			_childrenListModel.ItemInserted();
		}
		
		protected virtual void OnRemoveHusbandButton_Clicked(object sender, System.EventArgs e)
		{
			if (_husband != null)
			{
				SaveView();
				
				_famRecord.RemoveHusband(_husband);
				
				HusbandSpouseView.Record = null;
				
				_husband = null;
				
				// FIXME: check if there is anyone left in the family
			}
		}

		protected virtual void OnRemoveWifeButton_Clicked(object sender, System.EventArgs e)
		{
			if (_wife != null)
			{
				SaveView();
				
				_famRecord.RemoveWife(_wife);
				
				WifeSpouseView.Record = null;	
				
				_wife = null;
				
				// FIXME: check if there is anyone left in the family	
			}
		}
		
		protected virtual void OnAddHusbandButton_Clicked(object sender, System.EventArgs e)
		{
			if (SelectNewSpouse != null)
			{
			
				SaveView();
					
				IndividualArgs args = new IndividualArgs();
				SelectNewSpouse(this, args);
				
				if (args.Indi != null)
				{
					if (_husband == _record)
					{
						_record = args.Indi;	
					}
					
					_famRecord.ChangeHusband(args.Indi);
				}
							
				FillView();
			}
		}

		protected virtual void OnAddWifeButton_Clicked(object sender, System.EventArgs e)
		{
			if (SelectNewSpouse != null)
			{
				SaveView();
								
				IndividualArgs args = new IndividualArgs();
				SelectNewSpouse(this, args);
				
				if (args.Indi != null)
				{				
					if (_wife == _record)
					{
						_record = args.Indi;	
					}
					
					_famRecord.ChangeWife(args.Indi);
				}
								
				FillView();
			}
		}

		protected virtual void OnDeleteHusbandButton_Clicked(object sender, System.EventArgs e)
		{
			if (_husband != null)
			{
				GedcomIndividualRecord indi = _husband;
				
				if (DeleteIndividual != null)
				{
					IndividualArgs args = new IndividualArgs();
					args.Indi = indi;
					DeleteIndividual(this, args);
				}
			}
		}

		protected virtual void OnDeleteWifeButton_Clicked(object sender, System.EventArgs e)
		{
			if (_wife != null)
			{
				GedcomIndividualRecord indi = _wife;
				
				if (DeleteIndividual != null)
				{
					IndividualArgs args = new IndividualArgs();
					args.Indi = indi;
					DeleteIndividual(this, args);
				}
			}
		}

		protected virtual void OnHusbandParentsButtonClicked(object sender, EventArgs e)
		{
			GotoParents(HusbandSpouseView.ParentalFamily);
		}
		
		protected virtual void OnWifeParentsButtonClicked(object sender, EventArgs e)
		{
			GotoParents(WifeSpouseView.ParentalFamily);
		}
		
		protected virtual void OnHusbandScrapBookButtonClicked(object sender, EventArgs e)
		{
			SaveView();
			
			if (ShowScrapBook != null)
			{
				ScrapBookArgs args = new ScrapBookArgs();
				args.Record = _husband;
				
				ShowScrapBook(this, args);
			}
		}
		
		protected virtual void OnWifeScrapBookButtonClicked(object sender, EventArgs e)
		{
			SaveView();
			
			if (ShowScrapBook != null)
			{
				ScrapBookArgs args = new ScrapBookArgs();
				args.Record = _wife;
				ShowScrapBook(this, args);
			}
		}

		protected virtual void OnMarriageScrapbookButton_Clicked(object sender, ScrapBookArgs e)
		{
			SaveView();
			
			if (_famRecord.Marriage != null)
			{
				if (ShowScrapBook != null)
				{
					ShowScrapBook(this, e);
				}
			}
		}
				
		protected virtual void OnHusbandNameButtonClicked(object sender, EventArgs e)
		{
			SaveView();
			
			if (ShowName != null)
			{
				IndividualArgs args = new IndividualArgs();
				args.Indi = _husband;
				ShowName(this, args);
			}
		}
		
		protected virtual void OnWifeNameButtonClicked(object sender, EventArgs e)
		{
			SaveView();
			
			if (ShowName != null)
			{
				IndividualArgs args = new IndividualArgs();
				args.Indi = _wife;
				ShowName(this, args);
			}
		}
		
		#endregion
		
		#region Methods
		
		private void RemoveChild(Gtk.TreeIter iter)
		{			
			GedcomIndividualRecord child = _childrenListModel.GetValue(iter, 0) as GedcomIndividualRecord;
			
			_famRecord.RemoveChild(child);
			
			// GedcomFamilyRecord contains a list of children ids,
			// _childrenListModel is a list of GedcomIndividualRecords built from that,
			// so we still need to update the list
			_childrenListModel.List.Remove(child);
			_childrenListModel.ItemRemoved(iter);
		}
		
		private void FillView()
		{
			GedcomFamilyRecord fam = _famRecord;
			
			GedcomIndividualRecord husb = null;
			GedcomIndividualRecord wife = null;
			
			if (!string.IsNullOrEmpty(fam.Husband))
			{
				husb = _database[fam.Husband] as GedcomIndividualRecord;
				
				HusbandSpouseView.Record = husb;
			}
			else
			{
				HusbandSpouseView.Record = null;
			}
			
			if (!string.IsNullOrEmpty(fam.Wife))
			{
				wife = _database[fam.Wife] as GedcomIndividualRecord;
				
				WifeSpouseView.Record = wife;
			}
			else
			{
				WifeSpouseView.Record = null;
			}
			
			MarriageView.Record = fam;
			
			_childrenListModel = new ChildrenListModel();
			_childrenListModel.Database = _database;
			_childrenListModel.Record = fam;
			
			ChildrenTreeView.Model = _childrenListModel.Adapter;
			
			_husband = husb;
			_wife = wife;			
		}

		private void GotoParents(GedcomFamilyRecord fam)
		{
			if (fam != null)
			{
				SaveView();
				
				GedcomIndividualRecord husb = null;
				GedcomIndividualRecord wife = null;
	
				ClearView();
	
				if (!string.IsNullOrEmpty(fam.Husband))
				{
					husb = _database[fam.Husband] as GedcomIndividualRecord;
				}
				if (!string.IsNullOrEmpty(fam.Wife))
				{
					wife = _database[fam.Wife] as GedcomIndividualRecord;
				}
				
				if (husb != null)
				{
					_record = husb;
					_famRecord = fam;
					_wife = wife;
					
					FillView();
				}
				else if (wife != null)
				{
					_record = wife;
					_famRecord = fam;
					_husband = husb;
					
					FillView();
				}
				else
				{
					// FIXME hmm, no parents set, but got family, do what?
				}
			}	
		}
		
		#endregion
	}
}
