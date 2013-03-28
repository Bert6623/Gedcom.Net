/*
 *  $Id: PreferencesDialog.cs 201 2008-12-01 20:00:26Z davek $
 * 
 *  Copyright (C) 2008 David A Knight <david@ritter.demon.co.uk>
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

namespace Gedcom.UI.GTK
{
	
	
	public partial class PreferencesDialog : Gtk.Dialog
	{
		#region Constructors
		
		public PreferencesDialog()
		{
			this.Build();
			
			Fill();
		}

		#endregion

		#region Event Handlers
		
		protected void LoadLastOpenedCheckbutton_Toggled(object sender, System.EventArgs e)
		{
			AppSettings.Instance.ReloadLastOpenFile = LoadLastOpenedCheckbutton.Active;
		}

		protected void AllowHypenAndUnderscoreLoadCheckbutton_Toggled(object sender, System.EventArgs e)
		{
			AppSettings.Instance.AllowHypenAndUnderscore = AllowHypenAndUnderscoreLoadCheckbutton.Active;
		}

		protected void AllowInformationSeparatorOneLoadCheckbutton_Toggled(object sender, System.EventArgs e)
		{
			AppSettings.Instance.AllowInformationSeparatorOneLoad = AllowInformationSeparatorOneLoadCheckbutton.Active;
		}

		protected void AllowInformationSeparatorOneSaveCheckbutton_Toggled(object sender, System.EventArgs e)
		{
			AppSettings.Instance.AllowInformationSeparatorOneSave = AllowInformationSeparatorOneSaveCheckbutton.Active;
		}

		protected void AllowLineTabsLoadCheckbutton_Toggled(object sender, System.EventArgs e)
		{
			AppSettings.Instance.AllowLineTabsLoad = AllowLineTabsLoadCheckbutton.Active;
		}

		protected void AllowLineTabsSaveCheckbutton_Toggled (object sender, System.EventArgs e)
		{
			AppSettings.Instance.AllowLineTabsSave = AllowLineTabsSaveCheckbutton.Active;
		}

		protected void AllowTabsLoadCheckbutton_Toggled (object sender, System.EventArgs e)
		{
			AppSettings.Instance.AllowTabsLoad = AllowTabsLoadCheckbutton.Active;
		}

		protected void AllowTabsSaveCheckbutton_Toggled (object sender, System.EventArgs e)
		{
			AppSettings.Instance.AllowTabsSave = AllowTabsSaveCheckbutton.Active;
		}

		protected void ApplyConcContOnNewLineHackCheckbutton_Toggled(object sender, System.EventArgs e)
		{
			AppSettings.Instance.ApplyConcContOnNewLineHack = ApplyConcContOnNewLineHackCheckbutton.Active;
		}

		protected void IgnoreInvalidDelimeterCheckbutton_Toggled(object sender, System.EventArgs e)
		{
			AppSettings.Instance.IgnoreInvalidDelimeter = IgnoreInvalidDelimeterCheckbutton.Active;
		}

		protected void IgnoreMissingLineTerminatorCheckbutton_Toggled(object sender, System.EventArgs e)
		{
			AppSettings.Instance.IgnoreMissingLineTerminator = IgnoreMissingLineTerminatorCheckbutton.Active;
		}
		
		#endregion
		
		#region Methods

		private void Fill()
		{
			LoadLastOpenedCheckbutton.Active = AppSettings.Instance.ReloadLastOpenFile;

			NameEntry.Text = AppSettings.Instance.SubmitterName;

			IgnoreMissingLineTerminatorCheckbutton.Active = AppSettings.Instance.IgnoreMissingLineTerminator;
			IgnoreInvalidDelimeterCheckbutton.Active = AppSettings.Instance.IgnoreInvalidDelimeter;
			ApplyConcContOnNewLineHackCheckbutton.Active = AppSettings.Instance.ApplyConcContOnNewLineHack;
			AllowLineTabsLoadCheckbutton.Active = AppSettings.Instance.AllowLineTabsLoad;
			AllowLineTabsSaveCheckbutton.Active = AppSettings.Instance.AllowLineTabsSave;
			AllowTabsLoadCheckbutton.Active = AppSettings.Instance.AllowTabsLoad;
			AllowTabsSaveCheckbutton.Active = AppSettings.Instance.AllowTabsSave;
			AllowInformationSeparatorOneLoadCheckbutton.Active = AppSettings.Instance.AllowInformationSeparatorOneLoad;
			AllowInformationSeparatorOneSaveCheckbutton.Active = AppSettings.Instance.AllowInformationSeparatorOneSave;
			AllowHypenAndUnderscoreLoadCheckbutton.Active = AppSettings.Instance.AllowHypenAndUnderscore;
		}

		#endregion

	}
}
