/*
 *  $Id: GtkCellRendererButton.cs 183 2008-06-08 15:31:15Z davek $
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
	
	
	public class GtkCellRendererButton : Gtk.CellRenderer
	{
		#region Variables
		
		private bool _active = false;
		private bool _prelight = false;
		
		private string _stockId = "gtk-missing-image";
	
		private int _x;
		private int _y;
		
		#endregion
		
		#region Constructors
		
		public GtkCellRendererButton()
		{
			this.Mode = Gtk.CellRendererMode.Activatable;
			this.Width = 24 + (int)this.Xpad;
			this.Height = 24 + (int)this.Ypad;
		}
		
		#endregion
		
		#region Properties
		
		public bool Active
		{
			get { return _active; }
			set { _active = false; }
		}
		
		public bool Prelight
		{
			get { return _prelight; }
			set { _prelight = value; }
		}
		
		public string StockId
		{
			get { return _stockId; }
			set { _stockId = value; }
		}
		
		public int X
		{
			get { return _x;}	
		}
		
		public int Y
		{
			get { return _y; }
		}
		
		#endregion
		
		#region Methods
				 
		public override void GetSize (Gtk.Widget widget, ref Gdk.Rectangle cell_area, out int x_offset, out int y_offset, out int width, out int height)
		{
			width = 24;
			height = 24;
			
			x_offset = 2;
			y_offset = 2;
						
			width += (2 * (int)Xpad) - 2;
			height += (2 * (int)Ypad) - 2;
		}
		
		protected override void Render (Gdk.Drawable window, Gtk.Widget widget, Gdk.Rectangle background_area, Gdk.Rectangle cell_area, Gdk.Rectangle expose_area, Gtk.CellRendererState flags)
		{
			int x;
			int y;
			int width;
			int height;
			
			Gdk.Pixbuf buf = widget.RenderIcon(_stockId, Gtk.IconSize.Menu, string.Empty);
			
			Gtk.StateType state = Gtk.StateType.Normal;
			
			if (Prelight)
			{
				state = Gtk.StateType.Prelight;
			}
			if (Active)
			{
				state = Gtk.StateType.Active;	
			}
			
			GetSize(widget, ref cell_area, out x, out y, out width, out height); 
			Gtk.Style.PaintBox(widget.Style, window, state,
			                      Gtk.ShadowType.In, cell_area, widget, "buttondefault",
			                      cell_area.X + x, cell_area.Y + y,
			                      width, height);
			
			Gdk.GC gc = new Gdk.GC(window);
			
			if (buf != null)
			{
				int xOffset = 2 + ((width - buf.Width) / 2) + cell_area.X;
				int yOffset = 2 + ((height - buf.Height) / 2) + cell_area.Y;
								
				window.DrawPixbuf(gc, buf, 0, 0, xOffset, yOffset, buf.Width, buf.Height, Gdk.RgbDither.None, 0, 0);	
			}
			
			_x = cell_area.X + x;
			_y = cell_area.Y + y;
		}
		
		#endregion
	}
}
