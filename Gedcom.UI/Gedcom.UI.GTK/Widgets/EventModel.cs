/*
 *  $Id: EventModel.cs 199 2008-11-15 15:20:44Z davek $
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
using System.Collections.Generic;
using Gedcom;

namespace Gedcom.UI.GTK.Widgets
{
	
	
	public class EventModel : Gtk.ListStore
	{
		#region Enums
		
		public enum EventModelType
		{
			Individual = 0,
			Family,
			All
		};
		
		public enum Columns
		{
			Tag = 0,
			Readable = 1,
			Type = 2,
			Included = 3
		};
		
		#endregion
		
		#region Variables
		
		private EventModelType _modelType;
		
		private List<GedcomEvent.GedcomEventType> _includedEvents;
		
		#endregion
		
		#region Constructors
		
		private EventModel() : base(typeof(string), typeof(string), typeof(GedcomEvent.GedcomEventType), typeof(bool))
		{
		
		}
		
		public EventModel(EventModelType modelType) : this(modelType, GedcomEvent.GedcomEventType.GenericEvent, false)
		{
			
		}

		public EventModel(EventModelType modelType, GedcomEvent.GedcomEventType filterType, bool excludeFilter) :
			this(modelType, new List<GedcomEvent.GedcomEventType>() { filterType }, excludeFilter)
		{
		}
		
		public EventModel(EventModelType modelType, List<GedcomEvent.GedcomEventType> filterTypes, bool excludeFilter) : this()
		{
			_modelType = modelType;
			
			foreach (GedcomEvent.GedcomEventType eventType in Enum.GetValues(typeof(GedcomEvent.GedcomEventType)))
			{
				bool add = false;
				switch (_modelType)
				{
					case EventModelType.All:
						// even for all we don't want to include custom / generics
						if (eventType != GedcomEvent.GedcomEventType.Custom && 
							eventType != GedcomEvent.GedcomEventType.GenericEvent &&
							eventType != GedcomEvent.GedcomEventType.GenericFact)
						{
							add = true;
						}
						break;
					case EventModelType.Individual:
						// want individual events in the list
						if (eventType < GedcomEvent.GedcomEventType.ANUL || eventType > GedcomEvent.GedcomEventType.RESI)
						{
							add = true;
						}
						break;
					case EventModelType.Family:
						// want family events in the list
						if (eventType >= GedcomEvent.GedcomEventType.ANUL && eventType <= GedcomEvent.GedcomEventType.RESI)
						{
							add = true;
						}
						break;
				}
				if (add)
				{
					add = (filterTypes.Contains(eventType) || filterTypes.Contains(GedcomEvent.GedcomEventType.GenericEvent));
					
					if (excludeFilter)
					{
						add = !add;
					}
					if (add)
					{
						Gtk.TreeIter iter = Append();
						SetValue(iter, 0, GedcomEvent.TypeToTag(eventType));
						SetValue(iter, 1, GedcomEvent.TypeToReadable(eventType));
						SetValue(iter, 2, eventType);
						SetValue(iter, 3, false);
					}
				}
			}
		}
		
		#endregion
		
		#region Properties
		
		public EventModelType ModelType
		{
			get { return _modelType; }
		}
		
		public GedcomRecordList<GedcomEvent.GedcomEventType> IncludedEvents
		{
			get 
			{
				GedcomRecordList<GedcomEvent.GedcomEventType> includedEvents = new GedcomRecordList<GedcomEvent.GedcomEventType>();
				
				Gtk.TreeIter iter;
				if (IterChildren(out iter))
				{
					do
					{
						GedcomEvent.GedcomEventType type = (GedcomEvent.GedcomEventType)GetValue(iter, (int)Columns.Type);
						bool included = (bool)GetValue(iter, (int)Columns.Included);
						if (included)
						{
							includedEvents.Add(type);
						}
					} while (IterNext(ref iter));
				}
							
				return includedEvents; 
			}
			set 
			{ 
				_includedEvents = value;
				Gtk.TreeIter iter;
				if (IterChildren(out iter))
				{
					do
					{
						GedcomEvent.GedcomEventType type = (GedcomEvent.GedcomEventType)GetValue(iter, (int)Columns.Type);
						bool included = false;
						if (_includedEvents != null)
						{
							included = _includedEvents.Contains(type);
						}
												
						SetValue(iter, (int)Columns.Included, included);
					} while (IterNext(ref iter));
				}
			}
		}
		
		#endregion
	}
}
