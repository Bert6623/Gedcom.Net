/*
 *  $Id: Main.cs 200 2008-11-30 14:34:07Z davek $
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
using System.Diagnostics;
using System.Reflection;
using Gtk;

namespace Gedcom.UI.GTK
{
	class MainClass
	{
		public readonly static string AppName = Process.GetCurrentProcess().ProcessName;
		public readonly static string AppVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
		public readonly static string AppDisplayName = "Gedcom.NET";
		public readonly static string[] AppAuthors = new string[]
		{
			"David A Knight <david@ritter.demon.co.uk>"
		};
		public readonly static GedcomAddress AppAddress = null;
				
		public static void Main (string[] args)
		{
			Gdk.Threads.Init();
			Application.Init();
			MainWindow win = new MainWindow();
			win.Show ();
			
			Gdk.Threads.Enter();
			try
			{
				string lastOpened = AppSettings.Instance.LastOpenedFile;
				if (AppSettings.Instance.ReloadLastOpenFile && !string.IsNullOrEmpty(lastOpened))
				{
					win.DoReadGedcom(lastOpened, false);
				}
				Application.Run();
			}
			finally
			{
				Gdk.Threads.Leave();
			}
		}
	}
}