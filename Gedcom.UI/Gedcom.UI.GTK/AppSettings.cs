/*
 *  $Id: AppSettings.cs 200 2008-11-30 14:34:07Z davek $
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
using System.Configuration;

namespace Gedcom.UI.GTK
{
	public class AppSettings : ApplicationSettingsBase
	{
		#region Constructors
		
		private AppSettings()
		{
			
		}

		#endregion

		public static AppSettings Instance = new AppSettings();

		#region Properties
		
		[UserScopedSetting]
		[DefaultSettingValue("false")]
		public bool ReloadLastOpenFile 
		{ 
			get { return (bool)this["ReloadLastOpenFile"]; }
			set { this["ReloadLastOpenFile"] = value; }
		}

		[UserScopedSetting]
		[DefaultSettingValue("")]
		public string LastOpenedFile 
		{ 
			get { return (string)this["LastOpenedFile"]; }
			set { this["LastOpenedFile"] = value; }
		}

		[UserScopedSetting]
		[DefaultSettingValue("true")]
		public bool AllowHypenAndUnderscore 
		{ 
			get { return (bool)this["AllowHypenAndUnderscore"]; }
			set { this["AllowHypenAndUnderscore"] = value; }
		}

		[UserScopedSetting]
		[DefaultSettingValue("true")]
		public bool AllowInformationSeparatorOneLoad
		{ 
			get { return (bool)this["AllowInformationSeparatorOneLoad"]; }
			set { this["AllowInformationSeparatorOneLoad"] = value; }
		}

		[UserScopedSetting]
		[DefaultSettingValue("false")]
		public bool AllowInformationSeparatorOneSave 
		{ 
			get { return (bool)this["AllowInformationSeparatorOneSave"]; }
			set { this["AllowInformationSeparatorOneSave"] = value; }
		}

		[UserScopedSetting]
		[DefaultSettingValue("true")]
		public bool AllowLineTabsLoad 
		{ 
			get { return (bool)this["AllowLineTabsLoad"]; }
			set { this["AllowLineTabsLoad"] = value; }
		}

		[UserScopedSetting]
		[DefaultSettingValue("false")]
		public bool AllowLineTabsSave 
		{ 
			get { return (bool)this["AllowLineTabsSave"]; }
			set { this["AllowLineTabsSave"] = value; }
		}

		[UserScopedSetting]
		[DefaultSettingValue("true")]
		public bool AllowTabsLoad 
		{ 
			get { return (bool)this["AllowTabsLoad"]; }
			set { this["AllowTabsLoad"] = value; }
		}

		[UserScopedSetting]
		[DefaultSettingValue("false")]
		public bool AllowTabsSave 
		{ 
			get { return (bool)this["AllowTabsSave"]; }
			set { this["AllowTabsSave"] = value; }
		}

		
		[UserScopedSetting]
		[DefaultSettingValue("true")]
		public bool ApplyConcContOnNewLineHack 
		{ 
			get { return (bool)this["ApplyConcContOnNewLineHack"]; }
			set { this["ApplyConcContOnNewLineHack"] = value; }
		}

		[UserScopedSetting]
		[DefaultSettingValue("true")]
		public bool IgnoreInvalidDelimeter 
		{ 
			get { return (bool)this["IgnoreInvalidDelimeter"]; }
			set { this["IgnoreInvalidDelimeter"] = value; }
		}

		[UserScopedSetting]
		[DefaultSettingValue("true")]
		public bool IgnoreMissingLineTerminator 
		{ 
			get { return (bool)this["IgnoreMissingLineTerminator"]; }
			set { this["IgnoreMissingLineTerminator"] = value; }
		}

		[UserScopedSetting]
		[DefaultSettingValue("")]
		public string SubmitterName 
		{ 
			get { return (string)this["SubmitterName"]; }
			set { this["SubmitterName"] = value; }
		}

		[UserScopedSetting]
		[DefaultSettingValue("")]
		public string AddressLine1
		{ 
			get { return (string)this["AddressLine1"]; }
			set { this["AddressLine1"] = value; }
		}

		[UserScopedSetting]
		[DefaultSettingValue("")]
		public string AddressLine2
		{ 
			get { return (string)this["AddressLine2"]; }
			set { this["AddressLine2"] = value; }
		}

		[UserScopedSetting]
		[DefaultSettingValue("")]
		public string AddressLine3
		{ 
			get { return (string)this["AddressLine3"]; }
			set { this["AddressLine3"] = value; }
		}

		[UserScopedSetting]
		[DefaultSettingValue("")]
		public string City
		{ 
			get { return (string)this["City"]; }
			set { this["City"] = value; }
		}

		[UserScopedSetting]
		[DefaultSettingValue("")]
		public string State
		{ 
			get { return (string)this["State"]; }
			set { this["State"] = value; }
		}

		[UserScopedSetting]
		[DefaultSettingValue("")]
		public string PostCode
		{ 
			get { return (string)this["PostCode"]; }
			set { this["PostCode"] = value; }
		}

		[UserScopedSetting]
		[DefaultSettingValue("")]
		public string Country
		{ 
			get { return (string)this["Country"]; }
			set { this["Country"] = value; }
		}

		[UserScopedSetting]
		[DefaultSettingValue("")]
		public string Phone1
		{ 
			get { return (string)this["Phone1"]; }
			set { this["Phone1"] = value; }
		}

		[UserScopedSetting]
		[DefaultSettingValue("")]
		public string Phone2
		{ 
			get { return (string)this["Phone2"]; }
			set { this["Phone2"] = value; }
		}

		[UserScopedSetting]
		[DefaultSettingValue("")]
		public string Phone3
		{ 
			get { return (string)this["Phone3"]; }
			set { this["Phone3"] = value; }
		}

		[UserScopedSetting]
		[DefaultSettingValue("")]
		public string Fax1
		{ 
			get { return (string)this["Fax1"]; }
			set { this["Fax1"] = value; }
		}

		[UserScopedSetting]
		[DefaultSettingValue("")]
		public string Fax2
		{ 
			get { return (string)this["Fax2"]; }
			set { this["Fax2"] = value; }
		}

		[UserScopedSetting]
		[DefaultSettingValue("")]
		public string Fax3
		{ 
			get { return (string)this["Fax3"]; }
			set { this["Fax3"] = value; }
		}

		[UserScopedSetting]
		[DefaultSettingValue("")]
		public string Email1
		{ 
			get { return (string)this["Email1"]; }
			set { this["Email1"] = value; }
		}

		[UserScopedSetting]
		[DefaultSettingValue("")]
		public string Email2
		{ 
			get { return (string)this["Email2"]; }
			set { this["Email2"] = value; }
		}

		[UserScopedSetting]
		[DefaultSettingValue("")]
		public string Email3
		{ 
			get { return (string)this["Email3"]; }
			set { this["Email3"] = value; }
		}

		[UserScopedSetting]
		[DefaultSettingValue("")]
		public string Www1
		{ 
			get { return (string)this["Www1"]; }
			set { this["Www1"] = value; }
		}

		[UserScopedSetting]
		[DefaultSettingValue("")]
		public string Www2
		{ 
			get { return (string)this["Www2"]; }
			set { this["Www2"] = value; }
		}

		[UserScopedSetting]
		[DefaultSettingValue("")]
		public string Www3
		{ 
			get { return (string)this["Www3"]; }
			set { this["Www3"] = value; }
		}

		#endregion

		#region Methods

		protected override void OnPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnPropertyChanged (sender, e);
			
			Save();
		}
		
		#endregion
	}
}
