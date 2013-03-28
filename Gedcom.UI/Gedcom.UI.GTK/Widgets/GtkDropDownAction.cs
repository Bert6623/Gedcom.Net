/*
 *  $Id: GtkDropDownAction.cs 197 2008-11-15 12:41:00Z davek $
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

namespace Gedcom.UI.GTK.Widgets
{
	
	public class GtkDropDownAction : Gtk.Action
	{
		#region Variables
		
		protected string _arrowTooltip;
		protected string _menuPath;
		protected Gtk.UIManager _uiManager;
		
		protected Gtk.Widget _menu;
		
		#endregion

		#region Constructors
			
		public GtkDropDownAction(IntPtr raw) : base(raw)
		{
		
		}
		
		public GtkDropDownAction(string name, string label) : base(name, label)
		{
		
		}
		
		public GtkDropDownAction(string name, string label, string tooltip, string stockid) : base(name, label, tooltip, stockid)
		{
		
		}
		
		#endregion
		
		#region Properties
		
		public string ArrowTooltip
		{
			get { return _arrowTooltip; }
			set { _arrowTooltip = value; }
		}
		
		public string MenuPath
		{
			get { return _menuPath; }
			set { _menuPath = value; }
		}
	
		public Gtk.UIManager UIManager
		{
			get { return _uiManager; }
			set { _uiManager = value; }
		}
		
		public Gtk.Widget Menu
		{
			get { return _menu; }
			set { _menu = value; }
		}
		
		#endregion
		
		#region Event Handlers
		
		private void Menu_Activated(object sender, Gtk.PopupMenuArgs e)
		{
			
		}
		
		private void Tooltip_Set(object sender, Gtk.TooltipSetArgs e)
		{
			Gtk.MenuToolButton button = (Gtk.MenuToolButton)sender;
			
			button.ArrowTooltipText = e.TipText;
		}
		
		#endregion
		
		#region Methods
		
		public new Gtk.Widget CreateMenuItem()
		{
			return base.CreateMenuItem();
		}
		
		public new Gtk.Widget CreateToolItem()
		{
			Gtk.MenuToolButton button = new Gtk.MenuToolButton(this.StockId);
						
			button.TooltipSet += new Gtk.TooltipSetHandler(Tooltip_Set);
			
			return button;
		}
				
		public new void ConnectProxy(Gtk.Widget proxy)
		{
			if (proxy is Gtk.MenuToolButton)
			{
				if (_menu != null || !string.IsNullOrEmpty(_menuPath))
				{
					if (_menu == null)
					{
						_menu = _uiManager.GetWidget(_menuPath);
					}
					
					((Gtk.MenuToolButton)proxy).Menu = _menu;
				}
							
				proxy.PopupMenu += new Gtk.PopupMenuHandler(Menu_Activated);
			}
			
			base.ConnectProxy(proxy);
		}
		
		#endregion
	}
}
