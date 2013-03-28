/*
 *  $Id: PedigreeBox.cs 194 2008-11-10 20:39:37Z davek $
 * 
 *  Copyright (C) 2007 David A Knight <david@ritter.demon.co.uk>
 *  Copyright (C) 2001-2006  Donald N. Allingham, Martin Hawlisch
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
using Gedcom.UI.Common;

namespace Gedcom.UI.GTK.Widgets
{
	[System.ComponentModel.Category("Gedcom.UI.GTK.Widgets")]
	[System.ComponentModel.ToolboxItem(true)]
	public partial class PedigreeBox : Gtk.DrawingArea
	{
		#region Variables
		
		GedcomIndividualRecord _person;
		bool _force_mouse_over;
		
		const int personDragType = 1;
		
		uint _maxLines;
		bool _alive;
		
		bool _highlight;
		
		string _text;
		
		Gdk.Color _bgColour;
		Gdk.Color _borderColour;

		Pango.Layout _textLayout;
		
		#endregion
		
		#region Constructors
		
		public PedigreeBox(GedcomIndividualRecord person, uint maxLines, object image)
		{
			_person = person;
			_force_mouse_over = false;
			
			AddEvents((int)Gdk.EventMask.ButtonPressMask);
			AddEvents((int)Gdk.EventMask.EnterNotifyMask);
			AddEvents((int)Gdk.EventMask.LeaveNotifyMask);
			
			_maxLines = maxLines;
			_alive = false; 
			
			_highlight = false;
			
			_text = FormatPerson(_maxLines, true);
			
			if (_person != null)
			{
				_alive = !person.Dead;
				
				if (_alive)
				{
					switch (_person.Sex)
					{
						case GedcomSex.Male:
							_bgColour = new Gdk.Color(185,207,231);
							_borderColour = new Gdk.Color(32,74,135);
							break;
						case GedcomSex.Female:
							_bgColour = new Gdk.Color(255,205,241);
							_borderColour = new Gdk.Color(135,32,106);
							break;
						default:
							_bgColour = new Gdk.Color(244,220,183);
							_borderColour = new Gdk.Color(143,89,2);
							break;
					}
				}
				else
				{
					switch (_person.Sex)
					{
						case GedcomSex.Male:
							_bgColour = new Gdk.Color(185,207,231);
							_borderColour = new Gdk.Color(0, 0, 0);
							break;
						case GedcomSex.Female:
							_bgColour = new Gdk.Color(255,205,241);
							_borderColour = new Gdk.Color(0, 0, 0);
							break;
						default:
							_bgColour = new Gdk.Color(244,220,183);
							_borderColour = new Gdk.Color(0, 0, 0);
							break;
					}	
				}
			}
			else
			{
				_bgColour = new Gdk.Color(211,215,207);
			}
			
			SetSizeRequest(120,95);
		}
		
		#endregion
		
		#region Properties
		
		public bool ForceMouseOver
		{
			get { return _force_mouse_over; }
			set { _force_mouse_over = value; }
		}
		
		public string Text
		{
			get { return _text; }	
		}
		
		#endregion
		
		#region Events
		
		public event EventHandler<IndividualArgs> SelectIndividual;
		
		#endregion
		
		#region Event Handlers
		
		
		#endregion
		
		#region Methods

		public string FormatPerson(uint maxLines, bool markup)
		{
			string text = string.Empty;
			
			if (_person != null)
			{
				string name = _person.GetName().Name;
				
				GedcomIndividualEvent birth = _person.Birth;
				string bDate = string.Empty;
				string bPlace = string.Empty;
				if (birth != null)
				{
					if (birth.Date != null)
					{
						bDate = birth.Date.DateString;
					}
					if (birth.Place != null)
					{
						bPlace = birth.Place.Name;
					}
				}
				
				GedcomIndividualEvent death = _person.Death;
				string dDate = string.Empty;
				string dPlace = string.Empty;
				if (death != null)
				{
					if (death.Date != null)
					{
						dDate = death.Date.DateString;
					}
					if (death.Place != null)
					{
						dPlace = death.Place.Name;
					}
				}
				
				bDate = bDate.Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;");
				bPlace = bPlace.Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;");
				dDate = dDate.Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;");
				dPlace = dPlace.Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;");
				
				if (maxLines < 5)
				{
					if (markup)
					{
						text = string.Format("{0}\n* <i>{1}</i>\n+ <i>{2}</i>", name, bDate, dDate);
					}
					else
					{
						text = string.Format("{0}\n* {1}\n+ {2}", name, bDate, dDate);
					}
				}
				else
				{
					if (markup)
					{
						text = string.Format("{0}\n* <i>{1}</i>\n  <i>{3}</i>\n+ <i>{2}</i>\n  <i>{4}</i>", name, bDate, dDate, bPlace, dPlace);
					}
					else
					{
						text = string.Format("{0}\n* {1}\n  {3}\n+ {2}\n  {4}", name, bDate, dDate, bPlace, dPlace);
					}
				}
			}
			
			return text;
		}
		
		protected override bool OnEnterNotifyEvent (Gdk.EventCrossing evnt)
		{
			if (_person != null || _force_mouse_over)
			{
				_highlight = true;
				QueueDraw();
			}
			
			return base.OnEnterNotifyEvent (evnt);
		}

		protected override bool OnLeaveNotifyEvent (Gdk.EventCrossing evnt)
		{
			_highlight = false;
			QueueDraw();
			
			return base.OnLeaveNotifyEvent (evnt);
		}

		protected override void OnRealized ()
		{
			base.OnRealized ();
			
			Cairo.Context context = Gdk.CairoHelper.Create(GdkWindow);
			
			_textLayout = Pango.CairoHelper.CreateLayout(context);
			_textLayout.FontDescription = Pango.FontDescription.FromString(Style.FontDesc.ToString());
			_textLayout.SetMarkup(_text);
			
			int w = 0;
			int h = 0;
			_textLayout.GetPixelSize(out w, out h);
			
			int xmin = (int)w + 12;
			int ymin = (int)h + 11;
			SetSizeRequest(Math.Max(xmin, 120), Math.Max(ymin, 25));
			
			((IDisposable)context.Target).Dispose();
			((IDisposable)context).Dispose();
		}

		protected override bool OnExposeEvent (Gdk.EventExpose evnt)
		{
			bool ret = base.OnExposeEvent (evnt);
			
			Gdk.Rectangle alloc = Allocation;
			
			Cairo.Context context = Gdk.CairoHelper.Create(GdkWindow);
			// box shape + path
			context.MoveTo(0, 5);
			context.CurveTo(0,2,2,0,5,0);
			context.LineTo(alloc.Width - 8, 0);
			context.CurveTo(alloc.Width - 5, 0, alloc.Width - 3, 2, alloc.Width - 3, 5);
			context.LineTo(alloc.Width - 3, alloc.Height - 8);
			context.CurveTo(alloc.Width - 3, alloc.Height - 5, alloc.Width - 5, alloc.Height - 3, alloc.Width - 8, alloc.Height - 3);
			context.LineTo(5, alloc.Height - 3);
			context.CurveTo(2, alloc.Height - 3, 0, alloc.Height - 5, 0, alloc.Height - 8);
			context.ClosePath();
			
			Cairo.Path path = context.CopyPath();
							                
			// shadow
			context.Save();
			context.Translate(3, 3);
			context.NewPath();
			context.AppendPath(path);
			context.SetSourceRGBA(_borderColour.Red / 65535, _borderColour.Green / 65535, _borderColour.Blue / 65535, 0.4);
			context.FillPreserve();
			context.LineWidth = 0;
			context.Stroke();
			context.Restore();
			
			// box shape for clipping
			context.AppendPath(path);
			context.Clip();
			
			// background
			context.AppendPath(path);
			context.SetSourceRGB(_bgColour.Red / 65535.0, _bgColour.Green / 65535.0, _bgColour.Blue / 65535.0);
			context.FillPreserve();
			context.Stroke();
			
			if (Direction != Gtk.TextDirection.Rtl)
			{
				context.MoveTo(5, 4);
				context.SetSourceRGB(0,0,0);
				Pango.CairoHelper.ShowLayout(context, _textLayout);
			}
			else
			{			
				int w = 0;
				int h = 0;
				_textLayout.GetPixelSize(out w, out h);
				context.MoveTo(alloc.Width - w - 7, 4);
				context.SetSourceRGB(0,0,0);
				Pango.CairoHelper.ShowLayout(context, _textLayout);
			}
			
			// border
			if (_highlight)
			{
				context.LineWidth = 5;
			}
			else
			{
				context.LineWidth = 2;
			}
			context.AppendPath(path);
			context.SetSourceRGB(_borderColour.Red / 65535, _borderColour.Green / 65535, _borderColour.Blue / 65535);
			context.Stroke();
			
			((IDisposable)context.Target).Dispose();
			((IDisposable)context).Dispose();
			
			return ret;
		}

		protected override bool OnButtonPressEvent (Gdk.EventButton evnt)
		{
			bool ret = base.OnButtonPressEvent (evnt);
			
			if (evnt.Button == 1 && evnt.Type == Gdk.EventType.TwoButtonPress && _person != null)
			{
				IndividualArgs args = new IndividualArgs();
				args.Indi = _person;
				
				if (SelectIndividual != null)
				{
					SelectIndividual(this, args);
				}
				
				ret = false;
			}
			
			return ret;
		}

		
		protected override void OnDragDataGet (Gdk.DragContext context, Gtk.SelectionData selData, uint info, uint time_)
		{
			base.OnDragDataGet (context, selData, info, time_);
					
		}

		
		protected override void OnDragBegin (Gdk.DragContext context)
		{
			base.OnDragBegin (context);
		}
		
		#endregion
	}
	
}
