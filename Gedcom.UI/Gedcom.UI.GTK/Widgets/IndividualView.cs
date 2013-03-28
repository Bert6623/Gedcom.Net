/*
 *  $Id: IndividualView.cs 196 2008-11-12 22:55:27Z davek $
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
	public partial class IndividualView : Gtk.Bin, IGedcomView
	{
		#region Enums
		
		[Flags]
		public enum ViewTabFlags
		{
			Facts = 1,
			Marriage = 2,
			Address = 4,
			Medical = 8,
			Lineage = 16,
			Notes = 32,
			MoreDialog = (Facts | Address | Medical | Lineage | Notes),
			All = (Facts | Marriage | Address | Medical | Lineage | Notes)
		}
		
		#endregion
		
		#region Variables
				
		protected GedcomDatabase _database;
		protected GedcomRecord _record;
				
		protected ViewTabFlags _viewTabs;
		
		
		protected GedcomIndividualRecord _indi;
		protected GedcomEvent _event;
		
		protected GedcomAssociation _association;
		
		private bool _loading = false;
		
		private ParentsListModel _parents;
		private SpouseListModel _spouses;
		private ChildrenListModel _children;
		private SiblingsListModel _siblings;
		
		#endregion
		
		#region Constructors
		
		public IndividualView()
		{
			this.Build();
			
			FactView.EventAdded += new EventHandler(OnFactView_EventAdded);
			FactView.EventRemoved += new EventHandler(OnFactView_EventRemoved);
			FactView.ShowSourceCitation += new EventHandler<SourceCitationArgs>(OnFactView_ShowSourceCitation);
			FactView.ShowScrapBook += new EventHandler<ScrapBookArgs>(OnEventScrapbookButton_Clicked);
			
			Gtk.TreeViewColumn placeCol = new Gtk.TreeViewColumn();
			Gtk.CellRenderer rend = new Gtk.CellRendererText();
			placeCol.Title = "Place";
			placeCol.PackStart(rend,true);
			placeCol.SetCellDataFunc(rend, new Gtk.TreeCellDataFunc(ListModelUtil.RenderEventPlace));
			
			Gtk.TreeViewColumn dateCol = new Gtk.TreeViewColumn();
			rend = new Gtk.CellRendererText();
			dateCol.Title = "Date";
			dateCol.PackStart(rend,true);
			dateCol.SetCellDataFunc(rend, new Gtk.TreeCellDataFunc(ListModelUtil.RenderEventDate));
			dateCol.SortColumnId = 0;
			
			MarriageView.ShowScrapBook += new EventHandler<ScrapBookArgs>(OnMarriageScrapbookButton_Clicked);
			MarriageView.ShowSourceCitation += new EventHandler<SourceCitationArgs>(OnMarriageSourceButton_Clicked);
			MarriageView.MoreFamilyInformation += new EventHandler<FamilyArgs>(OnMarriageMoreButton_Clicked);
			
			Gtk.TreeViewColumn marriageToCol = new Gtk.TreeViewColumn();
			rend = new Gtk.CellRendererText();
			marriageToCol.Title = "To";
			marriageToCol.PackStart(rend,true);

			marriageToCol.SetCellDataFunc(rend, new Gtk.TreeCellDataFunc(ListModelUtil.RenderMarriageTo));
			
			MarriageTreeView.AppendColumn(placeCol);
			MarriageTreeView.AppendColumn(dateCol);
			MarriageTreeView.AppendColumn(marriageToCol);
			
			MarriageListModel marriageListModel = new MarriageListModel();
			MarriageTreeView.Model = marriageListModel;
			
			Gtk.TreeSelection selection = MarriageTreeView.Selection;
			selection.Changed += new EventHandler(OnMarriageSelection_Changed);
			
			_parents = new ParentsListModel();
			ParentsCombo.ListModel = _parents;
			
			_children = new ChildrenListModel();
			ChildrenCombo.ListModel = _children;
			
			_spouses = new SpouseListModel();
			SpousesCombo.ListModel = _spouses;
			
			_siblings = new SiblingsListModel();
			SiblingsCombo.ListModel = _siblings;
						
			Notebook.Page = 0;
			
		}
		
		#endregion
		
		#region Properties
		
		public ViewTabFlags ViewTabs
		{
			get { return _viewTabs; }
			set
			{
				_viewTabs = value;
				
				if ( (_viewTabs & ViewTabFlags.Facts) == 0)
				{
					Notebook.Children[0].Hide();
				}
				if ( (_viewTabs & ViewTabFlags.Marriage) == 0)
				{
					Notebook.Children[1].Hide();
				}
				if ( (_viewTabs & ViewTabFlags.Address) == 0)
				{
					Notebook.Children[2].Hide();
				}
				if ( (_viewTabs & ViewTabFlags.Medical) == 0)
				{
					Notebook.Children[3].Hide();
				}
				if ( (_viewTabs & ViewTabFlags.Lineage) == 0)
				{
					Notebook.Children[4].Hide();
				}
				if ( (_viewTabs & ViewTabFlags.Notes) == 0)
				{
					Notebook.Children[5].Hide();
				}
				
				if (_viewTabs != ViewTabFlags.All)
				{
					SwitchBox.Hide();		
				}
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
				MarriageView.Database = value;
				NotesView.Database = value;
				FactView.Database = value;
				AddressView.Database = value;
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
				
				if (_record.RecordType != GedcomRecordType.Individual)
				{
					throw new Exception("Invalid record type given to IndividualView: " + _record.RecordType.ToString());
				}
				
				_loading = true;
								
				ClearView();
				
				GedcomIndividualRecord indi = _record as GedcomIndividualRecord;			
				_indi = indi;
				
				GedcomName name = indi.GetName();
				
				NameEntry.Text = name.Name;
				
				GedcomIndividualEvent birth = indi.Birth;
				SetBirth(birth);
								
				GedcomIndividualEvent death = indi.Death;
				SetDeath(death);
				
				Gtk.TreeIter iter;
				Gtk.TreeModel model = SexComboBox.Model;
				int i = 0;
				if (model.GetIterFirst(out iter))
				{
					string sex = indi.SexChar;
					do
					{
						object o = model.GetValue(iter,0);
						if (((string)o)[0] == sex[0])
						{
							SexComboBox.Active = i;
							break;
						}
						i ++;
					}
					while (model.IterNext(ref iter));
				}
				
				FactView.Record = indi;
								
				if (indi.SpouseIn.Count > 0)
				{
					GedcomFamilyLink spouseIn = indi.SpouseIn[0];
					GedcomFamilyRecord fam = _database[spouseIn.Family] as GedcomFamilyRecord;
					if (fam != null)
					{
						MarriageView.Record = fam;
					}
					else
					{
						System.Diagnostics.Debug.WriteLine("Family link points to non family record");	
					}
				}
				MarriageListModel marriageListModel = new MarriageListModel();
				marriageListModel.Database = _database;
				marriageListModel.Record = indi;
				
				MarriageTreeView.Model = marriageListModel;
				
				AddressView.Record = indi;
				
				if (death != null && !string.IsNullOrEmpty(death.Cause))
				{
					CauseOfDeathTextView.Buffer.Text = death.Cause;
				}

				GedcomIndividualEvent heightFact = indi.Height;
				if (heightFact != null) 
				{
					HeightEntry.Text = heightFact.Classification;
				}
				GedcomIndividualEvent weightFact = indi.Weight;
				if (weightFact != null) 
				{
					WeightEntry.Text = weightFact.Classification;
				}
				GedcomIndividualEvent medicalFact = indi.Medical;
				if (medicalFact != null) 
				{
					MedicalInformationTextView.Buffer.Text = medicalFact.Classification;	
				}
				
				NotesView.Record = indi;
											
				_parents.Database = _database;
				_parents.Record = _record;
				_parents.NoIndividualLabel = "Parents";
				_parents.List.Insert(0, null);
				_parents.ItemInserted(0);
				ParentsCombo.ListModel = _parents;
				
				
				_children.Database = _database;
				_children.Record = _record;
				_children.NoIndividualLabel = "Children";
				_children.List.Insert(0, null);
				_children.ItemInserted(0);
				ChildrenCombo.ListModel = _children;
				
				_spouses.Database = _database;
				_spouses.Record = _record;
				_spouses.NoIndividualLabel = "Spouses";
				_spouses.List.Insert(0, null);
				_spouses.ItemInserted(0);
				SpousesCombo.ListModel = _spouses;
				
				_siblings.Database = _database;
				_siblings.Record = _record;
				_siblings.NoIndividualLabel = "Siblings";
				_siblings.List.Insert(0, null);
				_siblings.ItemInserted(0);
				SiblingsCombo.ListModel = _siblings;
				
				Born_Changed(this, EventArgs.Empty);
				Died_Changed(this, EventArgs.Empty);
				
				_loading = false;
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
			DateBornEntry.Text = string.Empty;
			DateDiedEntry.Text = string.Empty;
			BornInEntry.Text = string.Empty;
			DiedInEntry.Text = string.Empty;
			SexComboBox.Active = 0;
			
			ClearMarriageView();
			
			FactView.Clear();
		
			AddressView.ClearView();

			HeightEntry.Text = string.Empty;
			WeightEntry.Text = string.Empty;
			CauseOfDeathTextView.Buffer.Clear();
			MedicalInformationTextView.Buffer.Clear();
			
			NotesView.Clear();
								
			if (_parents != null)
			{
				_parents.Clear();
			}
			
			if (_children != null)
			{
				_children.Clear();
			}
			
			if (_spouses != null)
			{
				_spouses.Clear();
			}
			
			if (_siblings != null)
			{
				_siblings.Clear();
			}
			
			Born_Changed(this, EventArgs.Empty);
			Died_Changed(this, EventArgs.Empty);
		}
		
		public void SaveView()
		{
			if ((!_loading) && (_indi != null))
			{
				GedcomName name = null;
				
				if (_indi.Names.Count > 0)
				{
					name = _indi.GetName();
				}
				else
				{
					name = new GedcomName();
					name.Database = _database;
					name.Level = _indi.Level + 1;
					name.PreferedName = true;
					_indi.Names.Add(name);
				}
				
				// don't care if the name is empty, set it anyway
				name.Name = NameEntry.Text;
				
				GedcomIndividualEvent birth = _indi.Birth;
				if (!string.IsNullOrEmpty(BornInEntry.Text) ||
				    !string.IsNullOrEmpty(DateBornEntry.Text))
				{
					if (birth == null)
					{
						birth = new GedcomIndividualEvent();
						birth.Database = _database;
						birth.EventType = GedcomEvent.GedcomEventType.BIRT;
						birth.Level = _indi.Level + 1;
						birth.IndiRecord = _indi;
						_indi.Events.Add(birth);
					}
					if (birth.Place == null)
					{
						birth.Place = new GedcomPlace();
						birth.Place.Level = birth.Level + 1;
					}
					if (birth.Date == null)
					{
						birth.Date = new GedcomDate(_database);
						birth.Date.Level = birth.Level + 1;
					}
					
					birth.Place.Database = _database;
					birth.Place.Name = BornInEntry.Text;
					birth.Date.ParseDateString(DateBornEntry.Text);
				}
				else if (birth != null)
				{
					_indi.Events.Remove(birth);	
				}
				
				GedcomIndividualEvent death = _indi.Death;
				if (!string.IsNullOrEmpty(DiedInEntry.Text) ||
				    !string.IsNullOrEmpty(DateDiedEntry.Text))
				{
					if (death == null)
					{
						death = new GedcomIndividualEvent();
						death.Database = _database;
						death.EventType = GedcomEvent.GedcomEventType.DEAT;
						death.Level = _indi.Level + 1;
						death.IndiRecord = _indi;
						_indi.Events.Add(death);
					}
					if (death.Place == null)
					{
						death.Place = new GedcomPlace();
						death.Place.Level = death.Level + 1;
					}
					if (death.Date == null)
					{
						death.Date = new GedcomDate(_database);
						death.Date.Level = death.Level + 1;
					}
					
					death.Place.Database = _database;
					death.Place.Name = DiedInEntry.Text;
					death.Date.ParseDateString(DateDiedEntry.Text);
				}
				else if (death != null)
				{
					_indi.Events.Remove(death);	
				}
				
				_indi.Sex = (GedcomSex)SexComboBox.Active;
				
				FactView.Save();
				
				MarriageView.Save();
				
				AddressView.SaveView();

				GedcomIndividualEvent heightFact = _indi.Height;
				if (string.IsNullOrEmpty(HeightEntry.Text))
				{
					if (heightFact != null) 
					{
						_indi.Attributes.Remove(heightFact);
					}
				}
				else
				{
					if (heightFact == null) 
					{
						heightFact = new GedcomIndividualEvent();
						heightFact.Database = _database;
						heightFact.EventType = GedcomEvent.GedcomEventType.GenericFact;
						heightFact.Level = 1;
						heightFact.IndiRecord = _indi;
						heightFact.EventName = "Height";
						_indi.Attributes.Add(heightFact);
					}
					heightFact.Classification = HeightEntry.Text;
				}

				GedcomIndividualEvent weightFact = _indi.Weight;
				if (string.IsNullOrEmpty(WeightEntry.Text))
				{
					if (weightFact != null) 
					{
						_indi.Attributes.Remove(weightFact);
					}
				}
				else
				{
					if (weightFact == null) 
					{
						weightFact = new GedcomIndividualEvent();
						weightFact.Database = _database;
						weightFact.EventType = GedcomEvent.GedcomEventType.GenericFact;
						weightFact.Level = 1;
						weightFact.IndiRecord = _indi;
						weightFact.EventName = "Weight";
						_indi.Attributes.Add(weightFact);
					}
					weightFact.Classification = WeightEntry.Text;
				}
				
			    if (_indi.Death != null)
			    {
			    	_indi.Death.Cause = CauseOfDeathTextView.Buffer.Text;	
			    }

				GedcomIndividualEvent medicalFact = _indi.Medical;
				string medical = MedicalInformationTextView.Buffer.Text;
				if (string.IsNullOrEmpty(medical)) 
				{
					if (medicalFact != null)
					{
						_indi.Attributes.Remove(medicalFact);
					}
				}
				else
				{
					if (medicalFact == null) 
					{
						medicalFact = new GedcomIndividualEvent();
						medicalFact.Database = _database;
						medicalFact.EventType = GedcomEvent.GedcomEventType.GenericFact;
						medicalFact.Level = 1;
						medicalFact.IndiRecord = _indi;
						medicalFact.EventName = "Medical";
						_indi.Attributes.Add(medicalFact);
					}
					medicalFact.Classification = medical;
				}
				
			    NotesView.Save();
			}
			
		}
		
		#endregion
		
		#endregion
				
		#region EventHandlers
				
		protected void OnMarriageSelection_Changed(object sender, System.EventArgs e)
		{
			Gtk.TreeSelection selection = MarriageTreeView.Selection;
			
			SaveView();
			
			Gtk.TreeModel model;
			Gtk.TreeIter iter;
			if (selection.GetSelected(out model, out iter))
			{
				GedcomFamilyEvent ev = (GedcomFamilyEvent)model.GetValue(iter, 0);
				ClearMarriageView();
				
				GedcomIndividualRecord indi = _record as GedcomIndividualRecord;
								
				SetMarriageView(indi, ev);
			}
		}
				
		protected virtual void OnParentsCombo_Changed(object sender, System.EventArgs e)
		{
			if (ParentsCombo.Active > 0)
			{
				Gtk.TreeIter iter;
				if (ParentsCombo.GetActiveIter(out iter))
				{
					GedcomIndividualRecord parent = _parents.GetValue(iter, 0) as GedcomIndividualRecord;
					
					SaveView();
					
					Record = parent;
				}
			}
		}
		
		protected virtual void OnChildrenCombo_Changed(object sender, System.EventArgs e)
		{
			if (ChildrenCombo.Active > 0)
			{
				Gtk.TreeIter iter;
				if (ChildrenCombo.GetActiveIter(out iter))
				{
					GedcomIndividualRecord child = _children.GetValue(iter, 0) as GedcomIndividualRecord;
					
					SaveView();
					
					Record = child;
				}
			}
		}
		
		protected virtual void OnSpousesCombo_Changed(object sender, System.EventArgs e)
		{
			if (SpousesCombo.Active > 0)
			{
				Gtk.TreeIter iter;
				if (SpousesCombo.GetActiveIter(out iter))
				{
					GedcomIndividualRecord spouse = _spouses.GetValue(iter, 0) as GedcomIndividualRecord;
					
					SaveView();
					
					Record = spouse;
				}
			}
		}
		
		protected virtual void OnSiblingsCombo_Changed(object sender, System.EventArgs e)
		{
			if (SiblingsCombo.Active > 0)
			{
				Gtk.TreeIter iter;
				if (SiblingsCombo.GetActiveIter(out iter))
				{
					GedcomIndividualRecord sibling = _siblings.GetValue(iter, 0) as GedcomIndividualRecord;
					
					SaveView();
					
					Record = sibling;
				}
			}
		}

		protected virtual void OnNameSourceButton_Clicked(object sender, System.EventArgs e)
		{
			SaveView();
			
			if (ShowSourceCitation != null)
			{
				SourceCitationArgs args = new SourceCitationArgs();
				args.Record = _indi.GetName();
				ShowSourceCitation(this,args);
			}
		}

		protected virtual void OnDateBornSourceButton_Clicked(object sender, System.EventArgs e)
		{
			SaveView();
			
			if (ShowSourceCitation != null)
			{
				SourceCitationArgs args = new SourceCitationArgs();
				args.Record = _indi.Birth;
				ShowSourceCitation(this,args);
			}
		}

		protected virtual void OnDateDiedSourceButton_Clicked(object sender, System.EventArgs e)
		{
			SaveView();
			
			if (ShowSourceCitation != null)
			{
				SourceCitationArgs args = new SourceCitationArgs();
				args.Record = _indi.Death;
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
		
		protected virtual void OnMarriageMoreButton_Clicked(object sender, FamilyArgs e)
		{
			SaveView();
		
			if (MoreFamilyInformation != null)
			{
				MoreFamilyInformation(this, e);	
			}
		}
		
		protected virtual void OnFactView_EventAdded(object sender, System.EventArgs e)
		{
			GedcomEvent ev = FactView.Event;
			
			if (ev != null)
			{
				switch (ev.EventType)
				{
					case GedcomEvent.GedcomEventType.BIRT:
						SetBirth(ev);
						break;
					case GedcomEvent.GedcomEventType.DEAT:
						SetDeath(ev);
						break;
					case GedcomEvent.GedcomEventType.MARR:
						Gtk.TreeSelection selection = MarriageTreeView.Selection;
					
						MarriageListModel marriages = MarriageTreeView.Model as MarriageListModel;
						Gtk.TreeIter iter = marriages.Append();
						marriages.SetValue(iter, 0, ev);
						selection.SelectIter(iter);
						break;
				}
			}
		}

		protected virtual void OnFactView_EventRemoved(object sender, System.EventArgs e)
		{
			GedcomEvent ev = FactView.Event;
			
			if (ev != null)
			{
				switch (ev.EventType)
				{
					case GedcomEvent.GedcomEventType.BIRT:
						BornInEntry.Text = string.Empty;
						DateBornEntry.Text = string.Empty;
						break;
					case GedcomEvent.GedcomEventType.DEAT:
						DiedInEntry.Text = string.Empty;
						DateDiedEntry.Text = string.Empty;
						CauseOfDeathTextView.Buffer.Text = string.Empty;
						break;
					case GedcomEvent.GedcomEventType.MARR:
						Gtk.TreeSelection selection = MarriageTreeView.Selection;
						Gtk.TreeModel model;
						Gtk.TreeIter iter;
					
						if (selection.GetSelected(out model, out iter))
						{
							MarriageListModel marriages = model as MarriageListModel;
							
							marriages.Remove(ref iter);
						}
						break;
				}
			}
		}
		
		protected virtual void OnFactView_ShowSourceCitation(object sender, SourceCitationArgs e)
		{
			SaveView();
			
			if (ShowSourceCitation != null)
			{
				ShowSourceCitation(this, e);	
			}
		}
	
		protected virtual void OnIndiScrapbookButton_Clicked(object sender, System.EventArgs e)
		{
			SaveView();
			
			if (ShowScrapBook != null)
			{
				ScrapBookArgs args = new ScrapBookArgs();
				args.Record = _indi;
				ShowScrapBook(this, args);
			}
		}		

		protected virtual void OnMarriageScrapbookButton_Clicked(object sender, ScrapBookArgs e)
		{
			SaveView();
			
			if (ShowScrapBook != null)
			{
				ShowScrapBook(this, e);
			}
		}
		
		protected virtual void OnEventScrapbookButton_Clicked(object sender, ScrapBookArgs e)
		{
			SaveView();
			
			if (ShowScrapBook != null)
			{
				ShowScrapBook(this, e);
			}
		}
	
		protected virtual void OnNameButton_Clicked(object sender, System.EventArgs e)
		{
			SaveView();
			
			if (ShowName != null)
			{
				IndividualArgs args = new IndividualArgs();
				args.Indi = (GedcomIndividualRecord)_record;
				
				ShowName(this, args);
			}
		}

		protected virtual void Born_Changed(object sender, System.EventArgs e)
		{
			bool sensitive = ((!string.IsNullOrEmpty(BornInEntry.Text)) ||
							 (!string.IsNullOrEmpty(DateBornEntry.Text)));
			sensitive &= BornInEntry.Sensitive;
							 
			DateBornSourceButton.Sensitive = sensitive;
		}

		protected virtual void Died_Changed(object sender, System.EventArgs e)
		{
			bool sensitive = ((!string.IsNullOrEmpty(DiedInEntry.Text)) ||
							 (!string.IsNullOrEmpty(DateDiedEntry.Text)));
			sensitive &= DiedInEntry.Sensitive;
							 
			DateDiedSourceButton.Sensitive = sensitive;
		}


		protected virtual void OnDeleteButton_Clicked(object sender, System.EventArgs e)
		{
			SaveView();
			
			if (_record != null)
			{
				if (DeleteIndividual != null)
				{
					IndividualArgs args = new IndividualArgs();
					args.Indi = (GedcomIndividualRecord)_record;
					DeleteIndividual(this, args);
				}
			}
		}		
		
		protected virtual void OnFactView_MoreInformation (object sender, FactArgs e)
		{
			SaveView();
			
			if (MoreFactInformation != null)
			{
				MoreFactInformation(this, e);
			}
		}
		
		protected virtual void OnAddressView_ShowSourceCitation (object sender, SourceCitationArgs e)
		{
			SaveView();
			
			if (ShowSourceCitation != null)
			{
				ShowSourceCitation(this, e);
			}
		}

		protected virtual void OnAddressView_ShowScrapBook (object sender, ScrapBookArgs e)
		{
			SaveView();
			
			if (ShowScrapBook != null)
			{
				ShowScrapBook(this, e);
			}
		}

		protected virtual void OnAddressView_MoreFactInformation (object sender, FactArgs e)
		{
			SaveView();
			
			if (MoreFactInformation != null)
			{
				MoreFactInformation(this, e);
			}
		}
		
		protected virtual void OnNotesView_ShowSourceCitation (object sender, SourceCitationArgs e)
		{
			SaveView();
			
			if (ShowSourceCitation != null)
			{
				ShowSourceCitation(this, e);
			}
		}
		
		protected virtual void OnNotesView_SelectNewNote (object sender, NoteArgs e)
		{
			SaveView();
			
			if (SelectNewNote != null)
			{
				SelectNewNote(this, e);
			}
		}
		
		#endregion
		
		#region Methods
			
		protected void ClearMarriageView()
		{
			MarriageView.Clear();
		}
		
		protected void SetMarriageView(GedcomIndividualRecord indi, GedcomFamilyEvent marriage)
		{			
			if (marriage != null)
			{
				MarriageView.Record = marriage.FamRecord;
			}
			else
			{
				MarriageView.Record = null;
			}
		}
			
		private void SetBirth(GedcomEvent birth)
		{
			if (birth != null)
			{
				GedcomPlace place = birth.Place;
				if (place != null)
				{
					BornInEntry.Text = place.Name;	
				}
			
				GedcomDate date = birth.Date;
				if (date != null)
				{
					DateBornEntry.Text = date.DateString;
				}
			}
		}
		
		private void SetDeath(GedcomEvent death)
		{
			if (death != null)
			{
				GedcomPlace place = death.Place;
				if (place != null)
				{
					DiedInEntry.Text = place.Name;	
				}
				
				GedcomDate date = death.Date;
				if (date != null)
				{
					DateDiedEntry.Text = date.DateString;
				}
			}			
		}

		#endregion
	}
}
