/*
 *  $Id: ListModelUtil.cs 194 2008-11-10 20:39:37Z davek $
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
	public class ListModelUtil
	{
		public const string UnknownName = "unknown /unknown/";
		
		public ListModelUtil()
		{
		}
		
		public static void RenderIndividualNameCell(Gtk.CellLayout layout, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			RenderIndividualName(null, cell, model, iter);	
		}
		
		public static void RenderIndividualName(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Gtk.CellRendererText rend = (Gtk.CellRendererText)cell;
				
			object o = model.GetValue(iter,0);
				
			Gtk.TreeModelImplementor impl = null;
			
			if (model is Gtk.TreeModelAdapter)
			{
				impl = ((Gtk.TreeModelAdapter)model).Implementor;
			}
			
			if (o is GedcomIndividualRecord)
			{
				GedcomIndividualRecord indi = o as GedcomIndividualRecord;
			
				if (indi.Names.Count > 0)
				{
					GedcomName name = indi.GetName();
					rend.Text = name.Name;
				}
				else
				{
					rend.Text = UnknownName;
				}
			}
			else if (impl is IGedcomIndividualList)
			{
				IGedcomIndividualList indiList = impl as IGedcomIndividualList;
				
				(cell as Gtk.CellRendererText).Text = indiList.NoIndividualLabel;
			}
			else if (model is IGedcomIndividualList)
			{
				IGedcomIndividualList indiList = model as IGedcomIndividualList;
				
				rend.Text = indiList.NoIndividualLabel;
			}
			else
			{
				rend.Text = "not set";
			}
		}
		
		public static void RenderIndividualSex(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Gtk.CellRendererText rend = (Gtk.CellRendererText)cell;

			object o = model.GetValue(iter,0);
			if (o is GedcomIndividualRecord)
			{
				GedcomIndividualRecord indi = o as GedcomIndividualRecord;
				rend.Text = indi.SexChar;
			}
		}
		
		public static void RenderIndividualDob(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Gtk.CellRendererText rend = (Gtk.CellRendererText)cell;

			object o = model.GetValue(iter,0);
			if (o is GedcomIndividualRecord)
			{
				GedcomIndividualRecord indi = o as GedcomIndividualRecord;
			
				GedcomIndividualEvent birth = indi.Birth;
				
				if (birth != null && birth.Date != null)
				{
					rend.Text = birth.Date.Period;
				}
				else
				{
					rend.Text = "<unknown>";
				}
			}
		}
		
		public static void RenderIndividualBirthPlace(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Gtk.CellRendererText rend = (Gtk.CellRendererText)cell;
			
			object o = model.GetValue(iter,0);
			if (o is GedcomIndividualRecord)
			{
				GedcomIndividualRecord indi = o as GedcomIndividualRecord;
			
				GedcomIndividualEvent birth = indi.Birth;
				
				if (birth != null && birth.Place != null)
				{
					rend.Text = birth.Place.Name;
				}
				else
				{
					rend.Text = "<unknown>";
				}
			}
		}
		
		public static void RenderIndividualDod(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Gtk.CellRendererText rend = (Gtk.CellRendererText)cell;

			object o = model.GetValue(iter,0);
			if (o is GedcomIndividualRecord)
			{
				GedcomIndividualRecord indi = o as GedcomIndividualRecord;
			
				GedcomIndividualEvent death = indi.Death;
				
				if (death != null && death.Date != null)
				{
					rend.Text = death.Date.Period;
				}
				else
				{
					rend.Text = "<unknown>";
				}
			}
		}
		
		public static void RenderIndividualDeathPlace(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Gtk.CellRendererText rend = (Gtk.CellRendererText)cell;

			object o = model.GetValue(iter,0);
			if (o is GedcomIndividualRecord)
			{
				GedcomIndividualRecord indi = o as GedcomIndividualRecord;
			
				GedcomIndividualEvent death = indi.Death;
				
				if (death != null && death.Place != null)
				{
					rend.Text = death.Place.Name;
				}
				else
				{
					rend.Text = "<unknown>";
				}
			}
		}
				
		public static void RenderEventName(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Gtk.CellRendererText rend = (Gtk.CellRendererText)cell;

			GedcomEvent e = (GedcomEvent) model.GetValue(iter,0);
			
			if (e != null)
			{
				try
				{
				string evType = GedcomEvent.TypeToReadable(e.EventType);
				
				if (string.IsNullOrEmpty(e.EventName) && string.IsNullOrEmpty(e.Classification))
				{
					rend.Text = evType;
				}
				else if (string.IsNullOrEmpty(e.Classification))
				{
					
					rend.Text = string.Format("{0} {1}", evType, e.EventName);
				}
				else
				{
					rend.Text = string.Format("{0} {1} {2}", evType, e.EventName, e.Classification);
				}
				}
				catch (Exception ex) { System.Console.WriteLine((int)e.EventType + " " + ex.Message + " " + e.EventType);
				
				System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace();
				foreach (System.Diagnostics.StackFrame f in trace.GetFrames())
				{
					System.Console.WriteLine(f);
				}
				}
			}
		}
		
		public static void RenderEventDate(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Gtk.CellRendererText rend = (Gtk.CellRendererText)cell;

			GedcomEvent e = (GedcomEvent) model.GetValue(iter,0);
			
			if (e != null)
			{
				GedcomDate date = e.Date;
				
				if (date != null)
				{
					rend.Text = date.Period;
				}
				else
				{
					rend.Text = "<unknown>";
				}
			}
		}
		
		public static void RenderEventPlace(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Gtk.CellRendererText rend = (Gtk.CellRendererText)cell;

			GedcomEvent e = (GedcomEvent) model.GetValue(iter,0);
			
			if (e != null)
			{
				GedcomPlace place = e.Place;
				
				if (place != null)
				{
					rend.Text = place.Name;
				}
				else
				{
					rend.Text = string.Empty;
				}
			}
		}
		
		public static void RenderEventRecordedCount(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Gtk.CellRendererText rend = (Gtk.CellRendererText)cell;
			
			GedcomRecordedEvent recordedEvent = (GedcomRecordedEvent) model.GetValue(iter,0);
			
			if (recordedEvent != null)
			{
				int num = int.Parse(model.GetStringFromIter(iter)) + 1;
				rend.Text = num.ToString();
			}
		}
		
		
		public static void RenderMarriageTo(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Gtk.CellRendererText rend = (Gtk.CellRendererText)cell;
			
			GedcomFamilyEvent e = (GedcomFamilyEvent) model.GetValue(iter,0);
			
			if (e != null)
			{
				GedcomFamilyRecord fam = e.FamRecord;
				MarriageListModel marriageListModel = model as MarriageListModel;
				GedcomIndividualRecord indi = marriageListModel.Record as GedcomIndividualRecord;
				
				if (fam != null && indi != null)
				{
					if (fam.Husband == indi.XRefID)
					{
						indi = indi.Database[fam.Wife] as GedcomIndividualRecord;
					}
					else
					{
						indi = indi.Database[fam.Husband] as GedcomIndividualRecord;
					}
					
					string name;
					
					if (indi.Names.Count > 0)
					{
						name = indi.GetName().Name;	
					}
					else
					{
						name = UnknownName;	
					}
					
					rend.Text = name;
				}
				else
				{
					rend.Text = string.Empty;
				}
			}
		}
		
		public static void RenderNoteCount(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Gtk.CellRendererText rend = (Gtk.CellRendererText)cell;
			
			GedcomNoteRecord note = (GedcomNoteRecord) model.GetValue(iter,0);
			
			if (note != null)
			{
				int num = int.Parse(model.GetStringFromIter(iter)) + 1;
				rend.Text = num.ToString();
			}
		}
		
		public static void RenderNote(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Gtk.CellRendererText rend = (Gtk.CellRendererText)cell;
			
			GedcomNoteRecord note = (GedcomNoteRecord) model.GetValue(iter,0);
			
			if (note != null && note.Text != null)
			{
				int snipetLength = 40;
				
				if (note.Text.Length < snipetLength)
				{
					snipetLength = note.Text.Length;	
				}
				
				string snipet = note.Text.Substring(0,snipetLength).Trim();
				
				int newline = snipet.IndexOfAny(new char[] { '\n', '\r' });
				if (newline != -1)
				{
					snipet = snipet.Substring(0,newline - 1);	
				}
				if (snipet.Length > snipetLength)
				{
					snipet += " ...";
				}
				
				rend.Text = snipet;
			}
			else
			{
				rend.Text = string.Empty;	
			}
		}
		
		public static void RenderSourceTitle(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Gtk.CellRendererText rend = (Gtk.CellRendererText)cell;
			
			object o = model.GetValue(iter,0);
			if (o is GedcomSourceRecord)
			{
				GedcomSourceRecord source = o as GedcomSourceRecord;
				
				rend.Text = source.Title;
			}
			else
			{
				rend.Text = string.Empty;	
			}
		}
		
//		public static void RenderSourceDate(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
//		{
//			GedcomSourceRecord source = (GedcomSourceRecord) model.GetValue(iter,0);
//			
//			if (source != null)
//			{
//				GedcomDate date = source.Date;
//				
//				if (date != null)
//				{
//					(cell as Gtk.CellRendererText).Text = date.Period;
//				}
//				else
//				{
//					(cell as Gtk.CellRendererText).Text = "<unknown>";
//				}
//			}
//			else
//			{
//				(cell as Gtk.CellRendererText).Text = string.Empty;	
//			}
//		}
		
		public static void RenderRepositoryName(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Gtk.CellRendererText rend = (Gtk.CellRendererText)cell;
			
			object o = model.GetValue(iter,0);
			if (o is GedcomRepositoryRecord)
			{
				GedcomRepositoryRecord repo = o as GedcomRepositoryRecord;
				
				rend.Text = repo.Name;
			}
			else
			{
				rend.Text = string.Empty;	
			}
		}
		
		
		public static void RenderCitationCount(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Gtk.CellRendererText rend = (Gtk.CellRendererText)cell;
			
			GedcomSourceCitation citation = (GedcomSourceCitation) model.GetValue(iter,0);
			
			if (citation != null)
			{
				int num = int.Parse(model.GetStringFromIter(iter)) + 1;
				rend.Text = num.ToString();
			}
			else
			{
				rend.Text = string.Empty;	
			}
		}
		
		public static void RenderSourceCitation(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Gtk.CellRendererText rend = (Gtk.CellRendererText)cell;
			
			GedcomSourceCitation citation = (GedcomSourceCitation) model.GetValue(iter,0);
			
			if (citation != null && citation.Text != null)
			{
				int snipetLength = 40;
				
				if (citation.Text.Length < snipetLength)
				{
					snipetLength = citation.Text.Length;	
				}
				
				string snipet = citation.Text.Substring(0,snipetLength).Trim();
				
				int newline = snipet.IndexOfAny(new char[] { '\n', '\r' });
				if (newline != -1)
				{
					snipet = snipet.Substring(0,newline - 1);	
				}
				if (snipet.Length > snipetLength)
				{
					snipet += " ...";
				}
				
				rend.Text = snipet;
			}
			else
			{
				rend.Text = string.Empty;	
			}
		}

		public static void RenderMultimediaRecordTitle(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Gtk.CellRendererText rend = (Gtk.CellRendererText)cell;
			
			GedcomMultimediaRecord record = (GedcomMultimediaRecord) model.GetValue(iter,0);
						
			if (record != null)
			{
				string title = record.Title;
				if (string.IsNullOrEmpty(title))
				{
					title = "Untitled";
				}
				rend.Text = title;
			}
			else
			{
				rend.Text = string.Empty;	
			}
		}

		public static void RenderMultimediaFilename(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Gtk.CellRendererText rend = (Gtk.CellRendererText)cell;
			
			GedcomMultimediaFile file = (GedcomMultimediaFile) model.GetValue(iter,0);
			
			if (file != null)
			{
				rend.Text = file.Filename;
			}
			else
			{
				rend.Text = string.Empty;	
			}
		}
		
		public static void RenderMultimediaType(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Gtk.CellRendererText rend = (Gtk.CellRendererText)cell;
			
			GedcomMultimediaFile file = (GedcomMultimediaFile) model.GetValue(iter,0);
			
			if (file != null)
			{
				rend.Text = file.SourceMediaType;
			}
			else
			{
				rend.Text = string.Empty;	
			}
		}
		
		public static void RenderMultimediaFormat(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Gtk.CellRendererText rend = (Gtk.CellRendererText)cell;
			
			GedcomMultimediaFile file = (GedcomMultimediaFile) model.GetValue(iter,0);
			
			if (file != null)
			{
				rend.Text = file.Format;
			}
			else
			{
				rend.Text = string.Empty;	
			}
		}
		
		public static void RenderName(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Gtk.CellRendererText rend = (Gtk.CellRendererText)cell;
			
			object o = model.GetValue(iter,0);
			
			if (o is GedcomName)
			{
				GedcomName name = (GedcomName)o;
				string str = name.Name;
				if (string.IsNullOrEmpty(str))
				{
					str = UnknownName;
				}
				rend.Text = str;
			}
		}

		public static void RenderPreferedSpouse(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Gtk.CellRendererToggle rend = (Gtk.CellRendererToggle)cell;

			Gtk.TreeModelImplementor impl = null;
			
			if (model is Gtk.TreeModelAdapter)
			{
				impl = ((Gtk.TreeModelAdapter)model).Implementor;
			}
			
			if (impl is SpouseListModel)
			{
				SpouseListModel spouseList = impl as SpouseListModel;
				GedcomIndividualRecord indi = (GedcomIndividualRecord)model.GetValue(iter,0);
				
				rend.Active = spouseList.Prefered(indi.XRefID);
			}			
		}
	}
}
