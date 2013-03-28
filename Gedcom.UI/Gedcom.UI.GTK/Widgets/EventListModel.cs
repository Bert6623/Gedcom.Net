/*
 *  $Id: EventListModel.cs 196 2008-11-12 22:55:27Z davek $
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
	
	
	public class EventListModel : Gtk.ListStore
	{
		#region Variables
				
		protected GedcomDatabase _database;
		protected GedcomRecord _record;
		
		public const string UnknownName = "unknown /unknown/";
		
		protected GedcomEvent.GedcomEventType _filterType;	
		protected bool _excludeFilter = false;

		private FilterArgs _args;
		
		#endregion
		
		#region Constructors
		
		public EventListModel() : base(typeof(GedcomEvent))
		{
			this.DefaultSortFunc = new Gtk.TreeIterCompareFunc(Compare);
			this.SetSortFunc(0, new Gtk.TreeIterCompareFunc(Compare));

			_args = new FilterArgs();
		}
		
		#endregion
		
				
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
				
				Clear();
				
				if (_record.RecordType == GedcomRecordType.Individual)
				{
					GedcomIndividualRecord indi = _record as GedcomIndividualRecord;
				
					foreach (GedcomIndividualEvent e in indi.Events)
					{
						if (Filter(e))
						{
							Gtk.TreeIter iter = this.Append();
							this.SetValue(iter, 0, (object) e);
						}
					}
					foreach (GedcomIndividualEvent e in indi.Attributes)
					{
						if (Filter(e))
						{
							Gtk.TreeIter iter = this.Append();
							this.SetValue(iter, 0, (object) e);
						}
					}
				}
				else if (_record.RecordType == GedcomRecordType.Family)
				{
					GedcomFamilyRecord fam = _record as GedcomFamilyRecord;
					
					foreach (GedcomFamilyEvent e in fam.Events)
					{
						if (Filter(e))
						{
							Gtk.TreeIter iter = this.Append();
							this.SetValue(iter, 0, (object) e);
						}
					}
				}
				else
				{
					throw new Exception("Must provide an individual or family record record");	
				}
			}
		}
		
		public GedcomEvent.GedcomEventType FilterType
		{
			get { return _filterType; }
			set { _filterType = value; }
		}
		
		public bool ExcludeFilter
		{
			get { return _excludeFilter; }
			set { _excludeFilter = value; }
		}
		
		#endregion

		#region Events

		public class FilterArgs : EventArgs
		{
			public GedcomEvent Event;
			public bool Include;
		}

		public event EventHandler<FilterArgs> FilterGedcomEvent;
		
		#endregion
				
		#region Methods
		
		public int Compare(Gtk.TreeModel model, Gtk.TreeIter a, Gtk.TreeIter b)
		{
			GedcomEvent eventA = (GedcomEvent) model.GetValue(a,0);
			GedcomEvent eventB = (GedcomEvent) model.GetValue(b,0);
			
			return GedcomEvent.CompareByDate(eventA, eventB);
		}
		
		private bool Filter(GedcomEvent e)
		{
			bool ret = true;

			if (FilterGedcomEvent != null) 
			{
				_args.Event = e;
				FilterGedcomEvent(this, _args);

				ret = _args.Include;
			}
			else
			{
				ret = (_filterType == GedcomEvent.GedcomEventType.GenericEvent ||
				            e.EventType == _filterType);
				
				if (_excludeFilter)
				{
					ret = !ret;
				}
			}
			
			return ret;
		}
		
		#endregion
	}
}
