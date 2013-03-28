/*
 *  $Id: DuplicateView.cs 189 2008-10-10 14:16:10Z davek $
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
using System.Collections;
using System.Collections.Generic;
using Gedcom;
using Gedcom.Reports;

namespace Gedcom.UI.GTK.Widgets
{
	
	
	[System.ComponentModel.Category("Gedcom.UI.GTK.Widgets")]
	[System.ComponentModel.ToolboxItem(true)]
	public partial class DuplicateView : Gtk.Bin
	{
		#region Variables
		
		protected GedcomDatabase _databaseA;
		protected GedcomDatabase _databaseB;
		
		protected float _threshold = 80;
		
		protected Hashtable _duplicates;

		protected List<GedcomIndividualRecord> _matches = null;
		protected int _currentMatch = 0;
		
		protected GedcomIndividualRecord _indi;
		
		#endregion
		
		#region Constructors
		
		public DuplicateView()
		{
			this.Build();
			
			DuplicateList.Filter += new EventHandler<IndividualListModel.FilterArgs>(FilterIndividual);
		}
		
		#endregion
		
		#region Properties
		
		public GedcomDatabase DatabaseA
		{
			get { return _databaseA; }
			set 
			{ 
				_databaseA = value;
				PersonADuplicateView.Database = _databaseA;
			}
		}
		
		public GedcomDatabase DatabaseB
		{
			get { return _databaseB; }
			set 
			{ 
				if (_databaseA == null)
				{
					throw new Exception("DatabaseA must be set prior to setting DatabaseB");
				}
				
				_databaseB = value;
				PersonBDuplicateView.Database = _databaseB;
				
				Fill();
			}
		}
		
		public float Threshold
		{
			get { return _threshold; }
			set { _threshold = value; }
		}
		
		#endregion
		
		#region Event Handlers
		
		private void FilterIndividual(object sender, IndividualListModel.FilterArgs e)
		{
			if (_duplicates == null)
			{
				e.Include = false;
			}
			else
			{
				e.Include = _duplicates.Contains(e.Individual.XRefID);
			}
		}
		
		protected virtual void OnDuplicateList_RecordChanged(object sender, EventArgs e)
		{
			_indi = (GedcomIndividualRecord)DuplicateList.Record;
			
			DetailsBox.Sensitive = (_indi != null);
			
			int matches = 0;
			_matches = null;
			_currentMatch = 0;
			
			float percent = 0;
			
			if (_indi != null)
			{
				_matches = (List<GedcomIndividualRecord>)_duplicates[_indi.XRefID];
				matches = _matches.Count;
				_currentMatch = 1;
				PersonBDuplicateView.Record = _matches[0];
				
				percent = _indi.IsMatch(_matches[0]); 
				
				BackButton.Sensitive = false;
				ForwardButton.Sensitive = (matches > 1);
			}
			
			DuplicatesLabel.Text = string.Format("Potential Duplicate {0} of {1}: {2}%", _currentMatch, matches, percent);
			
			PersonADuplicateView.Record = _indi;
		}
		
		protected virtual void OnBackButton_Clicked (object sender, EventArgs e)
		{
			_currentMatch --;
			
			BackButton.Sensitive = (_currentMatch > 1);
			ForwardButton.Sensitive = (_currentMatch < _matches.Count);
			
			PersonBDuplicateView.Record = _matches[_currentMatch - 1];
			
			float percent = _indi.IsMatch(_matches[_currentMatch - 1]); 
			DuplicatesLabel.Text = string.Format("Potential Duplicate {0} of {1}: {2}%", _currentMatch, _matches.Count, percent);
		}

		protected virtual void OnForwardButton_Clicked (object sender, EventArgs e)
		{
			_currentMatch ++;
			
			BackButton.Sensitive = (_currentMatch > 1);
			ForwardButton.Sensitive = (_currentMatch < _matches.Count);
			
			PersonBDuplicateView.Record = _matches[_currentMatch - 1];
			
			float percent = _indi.IsMatch(_matches[_currentMatch - 1]); 
			DuplicatesLabel.Text = string.Format("Potential Duplicate {0} of {1}: {2}%", _currentMatch, _matches.Count, percent);
		}
		
		#endregion
		
		#region Methods
		
		public void Fill()
		{
			_duplicates = new Hashtable();
			
			GedcomDuplicate.DuplicateFoundFunc found = new GedcomDuplicate.DuplicateFoundFunc(FoundDuplicate);
			
			GedcomDuplicate.FindDuplicates(_databaseA, _databaseB, _threshold, found);
			
			// merges are into databaseA, populate the list
			DuplicateList.Database = _databaseA;
		}
		
		private void FoundDuplicate(GedcomIndividualRecord indi, List<GedcomIndividualRecord> matches)
		{
			_duplicates.Add(indi.XRefID, matches);
		}

		protected virtual void OnMergeButton_Clicked (object sender, System.EventArgs e)
		{
			GedcomIndividualRecord indi2 = (GedcomIndividualRecord)PersonBDuplicateView.Record;
		
			// copy sex if undetermined
			if (_indi.Sex == GedcomSex.Undetermined)
			{
				_indi.Sex = indi2.Sex;
			}
			
			// copy names
			
			foreach (GedcomName indiName in indi2.Names)
			{
				bool matched = false;
				
				foreach (GedcomName name in _indi.Names)
				{
					if (indiName.IsMatch(name) == 100.0F)
					{
						matched = true;
						break;
					}
				}
			
				// doesn't match any existing, add as a new name
				if (!matched)
				{
					indiName.Database = _databaseA;
					_indi.Names.Add(indiName);
				}
			}
			
			// FIXME: aliases need copying, but that is merging
			// separate individuals
			
			// FIXME: copy ancestors
			
			// FIXME: copy decendants
			
			// FIXME: copy associations
			
			// copy attributes
			foreach (GedcomIndividualEvent indiAttribute in indi2.Attributes)
			{
				bool matched = false;
				
				foreach (GedcomIndividualEvent attribute in _indi.Attributes)
				{
					if (indiAttribute.IsMatch(attribute) == 100.0F)
					{
						matched = true;
						break;
					}
				}
				
				if (!matched)
				{
					indiAttribute.Database = _indi.Database;
					_indi.Attributes.Add(indiAttribute);
				}
			}
			
			// FIXME: copy child in
			
			// copy events
			foreach (GedcomIndividualEvent indiEvent in indi2.Events)
			{
				bool matched = false;
				
				foreach (GedcomIndividualEvent ev in _indi.Events)
				{
					if (indiEvent.IsMatch(ev) == 100.0F)
					{
						matched = true;
						break;
					}
				}
				
				if (!matched)
				{
					indiEvent.Database = _indi.Database;
					_indi.Events.Add(indiEvent);
				}
			}
			
			// FIXME: copy spouse in
			
			// FIXME: copy submitter records
			
			// copy notes
			foreach (string noteID in indi2.Notes)
			{
				GedcomNoteRecord indiNote = _databaseB[noteID] as GedcomNoteRecord;
				if (indiNote != null)
				{
					bool matched = false;
					
					foreach (string noteID2 in _indi.Notes)
					{
						GedcomNoteRecord note = _databaseA[noteID2] as GedcomNoteRecord;
						if (note != null)
						{
							if (note.Text == indiNote.Text)
							{
								matched = true;
							}
						}
						else
						{
							System.Diagnostics.Debug.WriteLine("Missing NOTE record " + noteID2);
						}
					}
					
					if (!matched)
					{
						_indi.Database.Add(indiNote.XRefID, indiNote);
						_indi.Notes.Add(indiNote.XRefID);
					}
				}
				else
				{
					System.Diagnostics.Debug.WriteLine("Missing NOTE record " + noteID);
				}
			}
			
			// FIXME: copy sources
			
			// FIXME: copy multimedia
		
		}
		
		#endregion
	}
}
