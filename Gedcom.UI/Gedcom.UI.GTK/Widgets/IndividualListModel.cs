/*
 *  $Id: IndividualListModel.cs 183 2008-06-08 15:31:15Z davek $
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
using System.Globalization;

using Gedcom;

namespace Gedcom.UI.GTK.Widgets
{
	
	public class IndividualListModel : GenericListModel<GedcomIndividualRecord>
	{
		#region Enums
		
		public enum NameFilterType
		{
			Surname = 0,
			Firstname = 1
		};
		
		#endregion
		
		#region Variables
				
		protected GedcomDatabase _database;
		protected GedcomRecord _record;
		
		protected string _filterText;
		protected NameFilterType _filterType;
		protected string _filterSoundex;
		
		protected string _noIndividualLabel = string.Empty;
		
		protected bool _soundex = false;
		
		protected int _count = 0;
		
		public const string UnknownName = "unknown";
		public const string UnknownSoundex = "u525";
		
		private FilterArgs _args;
		
		#endregion
		
		#region Constructors
		
		public IndividualListModel()
		{
			_filterText = string.Empty;
			_filterType = NameFilterType.Surname;	
			_args = new FilterArgs();
			_applyFilter = true;
		}

		#endregion
		
		#region Properties
		
		public virtual GedcomDatabase Database
		{
			get { return _database; }
			set
			{
				_database = value;
				DoFill();
			}
		}
		
		public virtual GedcomRecord Record
		{
			get { return _record; }
			set
			{
				if (_database == null)
				{
					throw new Exception("Database must be set before Record");	
				}
				
				_record = value;
			}
		}
		
		public string FilterText
		{
			get { return _filterText; }
			set 
			{ 
				_filterText = value;
				_filterSoundex = Util.GenerateSoundex(value);
			}
		}
		
		public NameFilterType FilterType
		{
			get { return _filterType; }
			set { _filterType = value; }
		}
		
		public bool Soundex
		{
			get { return _soundex; }
			set { _soundex = value; }
		}
	
		#region IGedcomIndividualList
		
		public string NoIndividualLabel
		{
			get { return _noIndividualLabel; }
			set { _noIndividualLabel = value; }
		}
		
		#endregion
		
		#endregion
		
		#region Events
		
		public class FilterArgs : EventArgs
		{
			public GedcomIndividualRecord Individual;
			public bool Include;
		}
		
		public EventHandler<FilterArgs> FilterIndividual; 
		
		#endregion
		
		#region Methods
		
		public virtual void DoFill()
		{
			if (_database == null)
			{
				throw new Exception("Database must be set before filling the model");	
			}
			
			List = _database.Individuals;
		}
		
		protected override bool Filter(GedcomIndividualRecord indi)
		{
			bool show = true;
								
			if (indi != null && FilterIndividual != null)
			{
				_args.Individual = indi;
				_args.Include = true;
				FilterIndividual(this, _args);
				
				show = _args.Include;
			}
								
			if (show && indi != null)
			{	
				string txt = _filterText;
				if (_soundex)
				{
					txt = _filterSoundex;
				}
				switch (_filterType)
				{
					case NameFilterType.Surname:
						show = indi.MatchSurname(txt, _soundex);
						break;
					case NameFilterType.Firstname:
						show = indi.MatchFirstname(txt, _soundex);
						break;
					default:
						show = false;
						break;
				}				
			}
			
			return show;
		}
		
		#endregion
	}
}
