/*
 *  $Id: PedigreeView.cs 194 2008-11-10 20:39:37Z davek $
 * 
 *  Copyright (C) 2007 David A Knight <david@ritter.demon.co.uk>
 *  Copyright (C) 2001-2006  Donald N. Allingham, Martin Hawlisch
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
using System.Collections;
using System.Collections.Generic;

using Gedcom;
using Gedcom.UI.Common;
using Utility;

namespace Gedcom.UI.GTK.Widgets
{
	[System.ComponentModel.Category("Gedcom.UI.GTK.Widgets")]
	[System.ComponentModel.ToolboxItem(true)]
	public partial class PedigreeView : Gtk.Bin, IGedcomView
	{
		#region Variables
		
		GedcomDatabase _database;
		GedcomRecord _record;
		
		int _forceSize;
		bool _showImages;
		bool _showMarriageData;
		
		Gtk.Notebook _notebook;
		Gtk.Table _table2;
		Gtk.Table _table3;
		Gtk.Table _table4;
		Gtk.Table _table5;
				
		Hashtable  _lines;
		
		GedcomFamilyRecord _dummyFam;
		
		string _fatherID;
		string _motherID;
		
		#endregion
		
		#region Constructors
		
		public PedigreeView()
		{
			this.Build();
			
			_forceSize = 0;
			_showImages = false;
			_showMarriageData = false;
		
			_lines = new Hashtable();
			
			_notebook = new Gtk.Notebook();
			Add(_notebook);
			
			_notebook.ShowBorder = false;
			_notebook.ShowTabs = false;
			
			_table2 = new Gtk.Table(1, 1, false);
			AddTableToNotebook(_table2);
			
			_table3 = new Gtk.Table(1, 1, false);
			AddTableToNotebook(_table3);
			
			_table4 = new Gtk.Table(1, 1, false);
			AddTableToNotebook(_table4);
			
			_table5 = new Gtk.Table(1, 1, false);
			AddTableToNotebook(_table5);
			
			_notebook.CurrentPage = 0;
			_notebook.ShowAll();
			
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
					throw new Exception("Invalid record type given to Pedigree: " + _record.RecordType.ToString());
				}
				
				
				GedcomIndividualRecord indi = (GedcomIndividualRecord)_record;
				_dummyFam = indi.GetAllChildren();
				
				BuildTrees();
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
			
		}
		
		public void SaveView()
		{
		}
		
		#endregion
		
		#endregion

		#region Event Handlers

		private void Line_Expose(object sender, Gtk.ExposeEventArgs e)
		{
			Gtk.DrawingArea line = (Gtk.DrawingArea)sender;
			
			Gdk.GC gc = new Gdk.GC(line.GdkWindow);
			
			Gdk.Rectangle alloc = line.Allocation;
			
			int rightSide = 0;
			if (line.Direction != Gtk.TextDirection.Rtl)
			{
				rightSide = alloc.Width;
			}
			
			Utility.Pair<int, bool> lineData = (Utility.Pair<int, bool>)_lines[line];
			
			if (lineData != null)
			{
				int i = lineData.First;
				bool rela = lineData.Second;
				
				Gdk.LineStyle lineStyle = Gdk.LineStyle.Solid;
				if (!rela)
				{
					lineStyle = Gdk.LineStyle.OnOffDash;
				}
				
				int lineWidth = 3;
				
				gc.SetLineAttributes(lineWidth, lineStyle, Gdk.CapStyle.Butt, Gdk.JoinStyle.Bevel);
				
				if (i % 2 == 0)
				{
					line.GdkWindow.DrawLine(gc, rightSide, alloc.Height / 2, alloc.Width / 2, alloc.Height / 2);
					line.GdkWindow.DrawLine(gc, alloc.Width / 2, 0, alloc.Width / 2, alloc.Height / 2);
				}
				else
				{
					line.GdkWindow.DrawLine(gc, rightSide, alloc.Height / 2, alloc.Width / 2, alloc.Height / 2);
					line.GdkWindow.DrawLine(gc, alloc.Width / 2, alloc.Height, alloc.Width / 2, alloc.Height / 2);
				}
			}
		}
		
		private void PedigreeBox_SelectIndividual(object sender, IndividualArgs e)
		{
			Record = e.Indi;
		}
		
		private void ArrowButton_Click(object sender, EventArgs e)
		{
			Gtk.Menu menu = new Gtk.Menu();
			
			foreach (string childId in _dummyFam.Children)
			{
				GedcomIndividualRecord child = (GedcomIndividualRecord)_database[childId];
				
				string name = child.GetName().Name;
				
				Gtk.ImageMenuItem menuItem = new Gtk.ImageMenuItem(name);
				menuItem.Image = new Gtk.Image(Gtk.Stock.JumpTo, Gtk.IconSize.Menu);
				menuItem.ShowAll();
				
				menu.Append(menuItem);
								
				menuItem.Activated += ChildMenu_Activated;
			}
			
			menu.Popup();
		}
		
		private void ChildMenu_Activated(object sender, EventArgs e)
		{
			Gtk.MenuItem item = (Gtk.MenuItem)sender;
			
			Gtk.Menu menu = (Gtk.Menu)item.Parent;
			
			int i = 0;
			foreach (Gtk.Widget childItem in menu.Children)
			{
				if (childItem == item)
				{
					string childId = _dummyFam.Children[i];
					GedcomIndividualRecord child = (GedcomIndividualRecord)_database[childId];
					
					Record = child;
					break;
				}
				i ++;
			}
		}
		
		private void FatherButton_Click(object sender, EventArgs e)
		{
			GedcomIndividualRecord father = (GedcomIndividualRecord) _database[_fatherID];
			
			Record = father;
		}
		
		private void  MotherButton_Click(object sender, EventArgs e)
		{
			GedcomIndividualRecord mother = (GedcomIndividualRecord) _database[_motherID];
			
			Record = mother;
		}
		
		#endregion

		#region Methods
		
		private void AddTableToNotebook(Gtk.Table table)
		{
			Gtk.ScrolledWindow frame = new Gtk.ScrolledWindow();
			frame.ShadowType = Gtk.ShadowType.None;
			frame.SetPolicy(Gtk.PolicyType.Automatic, Gtk.PolicyType.Automatic);
			frame.AddWithViewport(table);
			table.RowSpacing = 1;
			table.ColumnSpacing = 0;
		
			_notebook.AppendPage(frame, new Gtk.Label(string.Empty));
		}
		
		protected override void OnSizeRequested (ref Gtk.Requisition requisition)
		{
			base.OnSizeRequested (ref requisition);
			
			if (_forceSize == 0)
			{
				Gdk.Rectangle v = Allocation;
				int pages = _notebook.NPages - 1;
				while (pages >= 0)
				{
					Gtk.Widget page = _notebook.GetNthPage(pages);
					
					page = ((Gtk.Bin)page).Child;
					page = ((Gtk.Bin)page).Child;
					
					Gdk.Rectangle p = page.Allocation;
					
					if (v.Width >= p.Width && v.Height > p.Height)
					{
						_notebook.CurrentPage = pages;
						break;
					}
					pages --;
				}
				
			}
			else
			{
				_notebook.CurrentPage = _forceSize - 2;
			}
		}
		
		private void BuildTrees()
		{
			uint[][][] pos2 = new uint[][][] {
				new uint[][] { new uint[] { 0, 3, 3, 3 }, new uint[] { 1, 0, 3 }, new uint[] { 1, 6, 4 }, new uint[] { 3, 3, 2, 3 } },
				new uint[][] { new uint[] { 2, 0, 3, 3 } },
				new uint[][] { new uint[] { 2, 6, 3, 3 } }
			};
			
			uint[][][] pos3 = new uint[][][] {
				new uint[][] { new uint[] { 0, 4, 3, 5 }, new uint[] { 1, 1, 3 }, new uint[] { 1, 9, 4 }, new uint[] { 3, 5, 2, 3 } },
				new uint[][] { new uint[] { 2, 1, 3, 3 }, new uint[] { 3, 0, 1 }, new uint[] { 3, 4, 1 }, new uint[] { 5, 1, 2, 3 } },
				new uint[][] { new uint[] { 2, 9, 3, 3 }, new uint[] { 3, 8, 1 }, new uint[] { 3, 12, 1 }, new uint[] { 5, 9, 2, 3 } },
				new uint[][] { new uint[] { 4, 0, 3, 1 } },
				new uint[][] { new uint[] { 4, 4, 3, 1 } },
				new uint[][] { new uint[] { 4, 8, 3, 1 } },
				new uint[][] { new uint[] { 4, 12, 3, 1 } }
			};
			
			uint[][][] pos4 = new uint[][][] {
				new uint[][] { new uint[] { 0, 5, 3, 5 }, new uint[] { 1, 2, 3 }, new uint[] { 1, 10, 4 }, new uint[] { 3, 6, 2, 3 } },
				new uint[][] { new uint[] { 2, 2, 3, 3 }, new uint[] { 3, 1, 1 }, new uint[] { 3, 5, 1 }, new uint[] { 5, 3, 2, 1 } },
				new uint[][] { new uint[] { 2, 10, 3, 3 }, new uint[] { 3, 9, 1 }, new uint[] { 3, 13, 1 }, new uint[] { 5, 11, 2, 1 } },
				new uint[][] { new uint[] { 4, 1, 3, 1 }, new uint[] { 5, 0, 1 }, new uint[] { 5, 2, 1 }, new uint[] { 7, 1, 2, 1 } },
				new uint[][] { new uint[] { 4, 5, 3, 1 }, new uint[] { 5, 4, 1 }, new uint[] { 5, 6, 1 }, new uint[] { 7, 5, 2, 1 } },
				new uint[][] { new uint[] { 4, 9, 3, 1 }, new uint[] { 5, 8, 1 }, new uint[] { 5, 10, 1 }, new uint[] { 7, 9, 2, 1 } },
				new uint[][] { new uint[] { 4, 13, 3, 1 }, new uint[] { 5, 12, 1 }, new uint[] { 5, 14, 1 }, new uint[] { 7, 13, 2, 1 } },
				new uint[][] { new uint[] { 6, 0, 3, 1 } },
				new uint[][] { new uint[] { 6, 2, 3, 1 } },
				new uint[][] { new uint[] { 6, 4, 3, 1 } },
				new uint[][] { new uint[] { 6, 6, 3, 1 } },
				new uint[][] { new uint[] { 6, 8, 3, 1 } },
				new uint[][] { new uint[] { 6, 10, 3, 1 } },
				new uint[][] { new uint[] { 6, 12, 3, 1 } },
				new uint[][] { new uint[] { 6, 14, 3, 1 } }
			};
			
			uint[][][] pos5 = new uint[][][] {
				new uint[][] { new uint[] {0, 10, 3, 11},  new uint[] {1, 5, 5},  new uint[] {1, 21, 5},  new uint[] {3, 13, 2, 5} }, 
				new uint[][] { new uint[] {2, 5, 3, 5},  new uint[] {3, 2, 3},  new uint[] {3, 10, 3},  new uint[] {5,  6, 2, 3} }, 
				new uint[][] { new uint[] {2, 21, 3, 5},  new uint[] {3, 18, 3},  new uint[] {3, 26, 3},  new uint[] {5, 22, 2, 3} }, 
				new uint[][] { new uint[] {4,  2, 3, 3},  new uint[] {5, 1, 1},  new uint[] {5, 5, 1},  new uint[] {7, 3, 2, 1} }, 
				new uint[][] { new uint[] {4, 10, 3, 3},  new uint[] {5, 9, 1},  new uint[] {5, 13, 1},  new uint[] {7, 11, 2, 1} }, 
				new uint[][] { new uint[] {4, 18, 3, 3},  new uint[] {5, 17, 1},  new uint[] {5, 21, 1},  new uint[] {7, 19, 2, 1} }, 
				new uint[][] { new uint[] {4, 26, 3, 3},  new uint[] {5, 25, 1},  new uint[] {5, 29, 1},  new uint[] {7, 27, 2, 1} }, 
				new uint[][] { new uint[] {6,  1, 3, 1},  new uint[] {7, 0, 1},  new uint[] {7, 2, 1},  new uint[] {9, 1, 2, 1} }, 
				new uint[][] { new uint[] {6,  5, 3, 1},  new uint[] {7, 4, 1},  new uint[] {7, 6, 1},  new uint[] {9, 5, 2, 1} }, 
				new uint[][] { new uint[] {6,  9, 3, 1},  new uint[] {7, 8, 1},  new uint[] {7, 10, 1},  new uint[] {9, 9, 2, 1} }, 
				new uint[][] { new uint[] {6, 13, 3, 1},  new uint[] {7, 12, 1},  new uint[] {7, 14, 1},  new uint[] {9, 13, 2, 1} }, 
				new uint[][] { new uint[] {6, 17, 3, 1},  new uint[] {7, 16, 1},  new uint[] {7, 18, 1},  new uint[] {9, 17, 2, 1} }, 
				new uint[][] { new uint[] {6, 21, 3, 1},  new uint[] {7, 20, 1},  new uint[] {7, 22, 1},  new uint[] {9, 21, 2, 1} }, 
				new uint[][] { new uint[] {6, 25, 3, 1},  new uint[] {7, 24, 1},  new uint[] {7, 26, 1},  new uint[] {9, 25, 2, 1} }, 
				new uint[][] { new uint[] {6, 29, 3, 1},  new uint[] {7, 28, 1},  new uint[] {7, 30, 1},  new uint[] {9, 29, 2, 1} }, 
				new uint[][] { new uint[] {8,  0, 3, 1}}, 
				new uint[][] { new uint[] {8,  2, 3, 1}}, 
				new uint[][] { new uint[] {8,  4, 3, 1}}, 
				new uint[][] { new uint[] {8,  6, 3, 1}}, 
				new uint[][] { new uint[] {8,  8, 3, 1}}, 
				new uint[][] { new uint[] {8, 10, 3, 1}}, 
				new uint[][] { new uint[] {8, 12, 3, 1}}, 
				new uint[][] { new uint[] {8, 14, 3, 1}}, 
				new uint[][] { new uint[] {8, 16, 3, 1}}, 
				new uint[][] { new uint[] {8, 18, 3, 1}}, 
				new uint[][] { new uint[] {8, 20, 3, 1}}, 
				new uint[][] { new uint[] {8, 22, 3, 1}}, 
				new uint[][] { new uint[] {8, 24, 3, 1}}, 
				new uint[][] { new uint[] {8, 26, 3, 1}}, 
				new uint[][] { new uint[] {8, 28, 3, 1}}, 
				new uint[][] { new uint[] {8, 30, 3, 1}}
			};
		
			GedcomFamilyLink[] lst = new GedcomFamilyLink[32];
			GedcomIndividualRecord indi = (GedcomIndividualRecord)_record;
			FindTree(indi, 0, 1, lst);
			
			_lines.Clear();
			_fatherID = string.Empty;
			_motherID = string.Empty;
			
			if (!string.IsNullOrEmpty(lst[0].Family))
			{
				GedcomFamilyRecord fam = (GedcomFamilyRecord)_database[lst[0].Family];
				
				_fatherID = fam.Husband;
				_motherID = fam.Wife;
			}
			
			Rebuild(_table2, pos2, indi, lst);
			Rebuild(_table3, pos3, indi, lst);
			Rebuild(_table4, pos4, indi, lst);
			Rebuild(_table5, pos5, indi, lst);
			
			this.ShowAll();
		}
		
		private void FindTree(GedcomIndividualRecord indi, int i, int depth, GedcomFamilyLink[] lst)
		{
			if (depth <= 5 && indi != null)
			{				
				List<GedcomFamilyLink> families = indi.ChildIn;
				if (families == null || families.Count == 0)
				{
					// indi doesn't exist as a child in any family, create
					// a dummy link record
					GedcomFamilyLink famLink = new GedcomFamilyLink();
					famLink.Indi = indi.XRefID;
					famLink.Pedigree = PedegreeLinkageType.Unknown;
					famLink.Database = _database;
					lst[i] = famLink;
				}
				else
				{
					GedcomFamilyLink famLink = families[0];
										
					lst[i] = famLink;
					
					GedcomFamilyRecord famRec = (GedcomFamilyRecord)_database[famLink.Family];
					
					if (!string.IsNullOrEmpty(famRec.Husband))
					{
						indi = (GedcomIndividualRecord)_database[famRec.Husband];
						FindTree(indi, (2*i)+1, depth + 1, lst);
					}
					if (!string.IsNullOrEmpty(famRec.Wife))
					{
						indi = (GedcomIndividualRecord)_database[famRec.Wife];
						FindTree(indi, (2*i)+2, depth + 1, lst);
					}
				}
			}
		}
		
		private void Rebuild(Gtk.Table table, uint[][][] positions, GedcomIndividualRecord activePerson, GedcomFamilyLink[] lst)
		{
			foreach (Gtk.Widget child in table.Children)
			{
				child.Destroy();
			}
			table.Resize(1,1);
			
			uint xmax = 0;
			uint ymax = 0;
			
			for (int i = 0; i < positions.Length; i ++)
			{
				uint x = positions[i][0][0] + 1;
				uint y = positions[i][0][1] + 1;
				uint w = positions[i][0][2];
				uint h = positions[i][0][3];
				
				GedcomFamilyLink famLink = (GedcomFamilyLink)lst[i];
				if (famLink == null)
				{	
					PedigreeBox pw = new PedigreeBox(null, 0, null);
					
					if (i > 0 && lst[((i+1)/2)-1] != null)
					{
						GedcomFamilyLink missingFamLink = (GedcomFamilyLink)lst[((i+1)/2)-1];
						
						// missing parent button
						pw.ForceMouseOver = true;
					}
					// FIXME: both conditions do the same thing, double checking
					// the gramps code it doesn't appear to be a mistake in porting
					if (positions[i][0][2] > 1)
					{
						table.Attach(pw,x, x+w, y, y+h, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 0, 0);
					}
					else
					{
						table.Attach(pw,x, x+w, y, y+h, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 0, 0);
					}
					
					xmax = (uint)Math.Max(xmax, x + w);
					ymax = (uint)Math.Max(ymax, y + h);
				}
				else
				{
					GedcomIndividualRecord indi = (GedcomIndividualRecord)_database[famLink.Indi];
					
					if (_showImages && i < ((positions.Length - 1) / 2) && positions[i][0][3] > 1)
					{
						
					}
					
					PedigreeBox pw = new PedigreeBox(indi, positions[i][0][3], null);
					pw.SelectIndividual += PedigreeBox_SelectIndividual;
					
					if (positions[i][0][3] < 7)
					{
						pw.TooltipMarkup = pw.FormatPerson(11, true);
					}
					
					if (positions[i][0][2] > 1)
					{
						table.Attach(pw, x, x+w, y, y+h, Gtk.AttachOptions.Expand | Gtk.AttachOptions.Fill, Gtk.AttachOptions.Expand | Gtk.AttachOptions.Fill, 0, 0);
					}
					else
					{
						table.Attach(pw, x, x+w, y, y+h, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 0, 0);
					}
					
					xmax = (uint)Math.Max(xmax, x + w);
					ymax = (uint)Math.Max(ymax, y + h);
				}
				
				// connection lines
				if (positions[i].Length > 1)
				{
					// separate boxes for father and mother
					x = positions[i][1][0] + 1;
					y = positions[i][1][1] + 1;
					w = 1;
					h = positions[i][1][2];
					
					Gtk.DrawingArea line = new Gtk.DrawingArea();
					line.ExposeEvent += Line_Expose;
					bool rela = false;
					if (famLink != null && (famLink.Pedigree == PedegreeLinkageType.Birth || famLink.Pedigree == PedegreeLinkageType.Unknown))
					{
						line.AddEvents((int)Gdk.EventMask.ButtonPressMask);
						rela = true;
					}
					Utility.Pair<int, bool> lineData = new Pair<int,bool>();
					lineData.First = i * 2 + 1;
					lineData.Second = rela;
					_lines[line] = lineData;
					
					table.Attach(line, x, x + w, y, y + h, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 0, 0);
					
					xmax = (uint)Math.Max(xmax, x + w);
					ymax = (uint)Math.Max(ymax, y + h);
					
					x = positions[i][2][0] + 1;
					y = positions[i][2][1] + 1;
					w = 1;
					h = positions[i][2][2];
					
					line = new Gtk.DrawingArea();
					line.ExposeEvent += Line_Expose;
					rela = false;
					if (famLink != null && (famLink.Pedigree == PedegreeLinkageType.Birth || famLink.Pedigree == PedegreeLinkageType.Unknown))
					{
						line.AddEvents((int)Gdk.EventMask.ButtonPressMask);
						rela = true;
					}
					lineData = new Pair<int,bool>();
					lineData.First = i * 2 + 2;
					lineData.Second = rela;
					_lines[line] = lineData;
					
					table.Attach(line, x, x + w, y, y + h, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 0, 0);
					
					xmax = (uint)Math.Max(xmax, x + w);
					ymax = (uint)Math.Max(ymax, y + h);
				}
				
				// marriage data
				if (_showMarriageData && positions[i].Length > 3)
				{
					string  text = string.Empty;
					if (famLink != null && (famLink.Pedigree == PedegreeLinkageType.Birth || famLink.Pedigree == PedegreeLinkageType.Unknown))
					{
						text = "foo";
					}
					Gtk.Label label = new Gtk.Label(text);
					label.Justify = Gtk.Justification.Left;
					label.LineWrap = true;
					label.SetAlignment(0.1F, 0.5F);
					
					x = positions[i][3][0] + 1;
					y = positions[i][3][1] + 1;
					w = positions[i][3][2];
					h = positions[i][3][3];
					
					table.Attach(label, x, x + w, y, y + h, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 0, 0);
				}
			}
			
			// nav arrows
			if (lst[0] != null)
			{
				Gtk.Button arrowButton = new Gtk.Button();
				arrowButton.Add(new Gtk.Arrow(Gtk.ArrowType.Left, Gtk.ShadowType.In));
				
				arrowButton.Sensitive = (_dummyFam.Children.Count > 0);
				arrowButton.Clicked += ArrowButton_Click;
				if (arrowButton.Sensitive)
				{
					arrowButton.TooltipText = "Jump to child...";
				}
				
				uint ymid = (uint)Math.Floor(ymax / 2.0F);
				table.Attach(arrowButton, 0, 1, ymid, ymid + 1, 0, 0, 0, 0);
				
				// father
				arrowButton = new Gtk.Button();
				arrowButton.Add(new Gtk.Arrow(Gtk.ArrowType.Right, Gtk.ShadowType.In));
				arrowButton.Sensitive = (lst[1] != null);
				arrowButton.Clicked += FatherButton_Click;
				if (arrowButton.Sensitive)
				{
					arrowButton.TooltipText = "Jump to father";
				}
				
				ymid = (uint)Math.Floor(ymax / 4.0F);
				table.Attach(arrowButton, xmax, xmax + 1, ymid - 1, ymid + 2, 0, 0, 0, 0);
				
				// mother
				arrowButton = new Gtk.Button();
				arrowButton.Add(new Gtk.Arrow(Gtk.ArrowType.Right, Gtk.ShadowType.In));
				arrowButton.Sensitive = (lst[2] != null);
				arrowButton.Clicked += MotherButton_Click;
				if (arrowButton.Sensitive)
				{
					arrowButton.TooltipText = "Jump to mother";
				}
				
				ymid = (uint)Math.Floor(ymax / 4.0F * 3);
				table.Attach(arrowButton, xmax, xmax + 1, ymid - 1, ymid + 2, 0, 0, 0, 0);
				
				
				// dummy widgets to allow pedigree to be centred
				Gtk.Label l = new Gtk.Label(string.Empty);
				table.Attach(l, 0, 1, 0, 1, Gtk.AttachOptions.Expand | Gtk.AttachOptions.Fill, Gtk.AttachOptions.Expand | Gtk.AttachOptions.Fill, 0, 0);
				l = new Gtk.Label(string.Empty);
				table.Attach(l, xmax, xmax + 1, ymax, ymax + 1, Gtk.AttachOptions.Expand | Gtk.AttachOptions.Fill, Gtk.AttachOptions.Expand | Gtk.AttachOptions.Fill, 0, 0);
	
				table.ShowAll();
			}
		}	
			
		#endregion
	}
	
	
}
