/*
 *  $Id: IGedcomView.cs 194 2008-11-10 20:39:37Z davek $
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

namespace Gedcom.UI.Common
{
	
	
	public interface IGedcomView
	{
		#region Properties
		
		GedcomDatabase Database
		{
			get;
			set;
		}
		
		GedcomRecord Record
		{
			get;
			set;
		}
		
		GedcomIndividualRecord Husband
		{
			get;	
		}
		
		GedcomIndividualRecord Wife
		{
			get;	
		}
		
		#endregion
		
		#region Events
		
		event EventHandler<IndividualArgs> MoreInformation;
		event EventHandler<FamilyArgs> MoreFamilyInformation;
		event EventHandler<SpouseSelectArgs> SpouseSelect;
		event EventHandler<SourceCitationArgs> ShowSourceCitation;
		event EventHandler<IndividualArgs> SelectNewChild;
		event EventHandler<IndividualArgs> SelectNewSpouse;
		event EventHandler<ScrapBookArgs> ShowScrapBook;
		event EventHandler<IndividualArgs> ShowName;
		
		event EventHandler<IndividualArgs> DeleteIndividual;
		
		event EventHandler<FactArgs> MoreFactInformation;
		
		event EventHandler<NoteArgs> SelectNewNote;
		
		#endregion
		
		#region Methods
		
		void ClearView();
		
		void SaveView();
		
		#endregion
	}
		
	public class IndividualArgs : EventArgs
	{
		public GedcomIndividualRecord Indi;
	}
	
	public class FamilyArgs : EventArgs
	{
		public GedcomFamilyRecord Fam;
	}
	
	public class SpouseSelectArgs : EventArgs
	{
		public GedcomIndividualRecord Indi;
		public GedcomIndividualRecord Spouse;
		public GedcomIndividualRecord SelectedSpouse;
		public GedcomFamilyRecord Family;
	}
	
	public class SourceCitationArgs : EventArgs
	{
		public GedcomRecord Record;
	}
	
	public class SourceArgs : EventArgs
	{
		public GedcomSourceRecord Source;	
	}
	
	public class ScrapBookArgs : EventArgs
	{
		public GedcomRecord Record;
	}
	
	public class FactArgs : EventArgs
	{
		public GedcomEvent Event;
	}
	
	public class NoteArgs : EventArgs
	{
		public GedcomNoteRecord Note;
	}

	public class MultimediaFileArgs : EventArgs
	{
		public string Filename;
	}
}