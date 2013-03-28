// IndividualListComboBox.cs
//
//  Copyright (C) 2008 David A Knight <david@ritter.demon.co.uk>
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
//
//

using System;

using Gedcom;

namespace Gedcom.UI.GTK.Widgets
{
	
	[System.ComponentModel.Category("Gedcom.UI.GTK.Widgets")]
	[System.ComponentModel.ToolboxItem(true)]
	public partial class IndividualListComboBox : Gtk.Bin
	{
		#region Variables
		
		private string _noIndiText;
		private IndividualListModel _model;
		
		#endregion
		
		#region Constructors
		
		public IndividualListComboBox()
		{
			this.Build();
						
			Combo.Clear();
			
			
			Gtk.CellRendererText rend = new Gtk.CellRendererText();
			rend.Ellipsize = Pango.EllipsizeMode.End;
			Combo.PackStart(rend, true);
			Combo.SetCellDataFunc(rend, new Gtk.CellLayoutDataFunc(RenderIndividualNameCell));
						
			
		}
		
		#endregion
		
		#region Properties
		
		public int Active
		{
			get { return Combo.Active; }
			set { Combo.Active = value; }
		}
		
		public string NoIndividualText
		{
			get { return _noIndiText; }
			set { _noIndiText = value; }
		}
		
		public IndividualListModel ListModel
		{
			get { return _model; }
			set 
			{ 
				_model = value; 
				Combo.Model = _model.Adapter;
				Combo.Active = 0;
			}
		}
		
		#endregion
		
		#region Events
		
		public event EventHandler Changed;
		
		#endregion
		
		#region Event Handlers
		
		protected virtual void OnCombo_Changed(object sender, System.EventArgs e)
		{
			if (Changed != null)
			{
				Changed(this, EventArgs.Empty);
			}
		}
		
		#endregion
		
		#region Methods
		
		public bool GetActiveIter(out Gtk.TreeIter iter)
		{
			iter = Gtk.TreeIter.Zero;
			
			return Combo.GetActiveIter(out iter);
		}
		
		private void RenderIndividualNameCell(Gtk.CellLayout layout, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			object o = model.GetValue(iter,0);
			if (o == null || o is IntPtr && ((IntPtr)o) == IntPtr.Zero)
			{
				Gtk.CellRendererText rend = (Gtk.CellRendererText)cell;
				rend.Text = _noIndiText;
			}
			else
			{
				ListModelUtil.RenderIndividualName(null, cell, model, iter);	
			}
		}
		
		#endregion
		 
	}
}
