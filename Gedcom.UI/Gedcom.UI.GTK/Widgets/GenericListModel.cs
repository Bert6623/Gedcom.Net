/*
 *  $Id: GenericListModel.cs 199 2008-11-15 15:20:44Z davek $
 * 
 *  Copyright (C) 2007-2008 David A Knight <david@ritter.demon.co.uk>
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

namespace Gedcom.UI.GTK.Widgets
{
	
	
	public class GenericListModel<T> : GLib.Object, Gtk.TreeModelImplementor
	{
		private List<T> _data;
		
		private Gtk.TreeModelAdapter _adapter;
			
		protected bool _applyFilter;
		
		#region Constructors
		
		public GenericListModel()
		{
			_adapter = new Gtk.TreeModelAdapter(this);
			_applyFilter = false;
		}
		
		#endregion

		#region Gtk.TreeModelImplementor
		
		private int _columns = 1;
		
		public Gtk.TreeModelFlags Flags 
		{
			get
			{
				return Gtk.TreeModelFlags.ListOnly;
			}
		}
		
		public int NColumns
		{
			get
			{
				return _columns;
			}
		}
		
		public GLib.GType GetColumnType(int col)
		{
			GLib.GType result = (GLib.GType)typeof(T);
			return result;
		}
		
		public bool GetIterFirst(out Gtk.TreeIter iter)
		{
			bool ret = false;
			
			iter = Gtk.TreeIter.Zero;
			
			if (_data != null && _data.Count > 0)
			{
				iter = new Gtk.TreeIter();
				iter.UserData = (IntPtr)0;
				ret = true;
			}

			return ret;
		}
		
		public bool GetIter(out Gtk.TreeIter iter, Gtk.TreePath path)
		{
			bool ret = false;
		
			iter = Gtk.TreeIter.Zero;

			if (path != null && _data != null)
			{
				int i = path.Indices[0];
				if (i < _data.Count)
				{
					iter = new Gtk.TreeIter();	
					iter.UserData = (IntPtr)i;
					ret = true;
				}
			}
	
			return ret;
		}
		
		public Gtk.TreePath GetPath(Gtk.TreeIter iter)
		{
			Gtk.TreePath ret = null;
			
			if (_data != null)
			{
				int i = (int)iter.UserData;
				
				if (i < _data.Count)
				{
					ret = new Gtk.TreePath();
					ret.PrependIndex(i);
					
					ret.Owned = false;
				}
			}
			
			return ret;
		}
		
		public void GetValue(Gtk.TreeIter iter, int col, ref GLib.Value val)
		{
			int i = (int)iter.UserData;
			
			T item = default(T);
			if (_data != null && i < _data.Count)
			{
				item = _data[i];
			}
			
			val = new GLib.Value(typeof(T));
			val.Val = item;
		}
		
		public bool IterNext(ref Gtk.TreeIter iter)
		{
			bool ret = false;
			
			if (_data != null)
			{
				int i = (int)iter.UserData;
				i ++;
				if (i < _data.Count)
				{
					ret = true;
					iter.UserData = (IntPtr)i;
				}
			}
			
			return ret;
		}
				
		public bool IterChildren(out Gtk.TreeIter child, Gtk.TreeIter parent)
		{
			child = Gtk.TreeIter.Zero;
			
			return false;
		}
		
		public bool IterHasChild(Gtk.TreeIter iter)
		{
			return false;
		}
		
		public int IterNChildren(Gtk.TreeIter iter)
		{
			return 0;
		}
		
		public bool IterNthChild(out Gtk.TreeIter child, Gtk.TreeIter parent, int n)
		{
			child = Gtk.TreeIter.Zero;
			
			return false;
		}
		
		public bool IterParent(out Gtk.TreeIter parent, Gtk.TreeIter child)
		{
			parent = Gtk.TreeIter.Zero;
			
			return false;
		}
		
		public void RefNode(Gtk.TreeIter iter)
		{
		}
		
		public void UnrefNode(Gtk.TreeIter iter)
		{
		}
		
		#endregion
		
		#region Properties
		
		public int Count
		{
			get 
			{ 
				int count = 0;
				if (_data != null)
				{
					count = _data.Count;	
				}
				return count;
			}
		}
		
		public bool ApplyFilter
		{
			get { return _applyFilter; }
			set { _applyFilter = value; }
		}
		
		public List<T> List
		{
			get { return _data; }
			set
			{
				if (_data != null)
				{
					Clear();
				}
					
				if (value == null)
				{
					_data = null;
				}
				else
				{
					if (_applyFilter)
					{
						_data = value.FindAll(Filter);
					}
					else
					{
						_data = value;
					}
				
					if (_data != null)
					{
						Gtk.TreeIter iter = new Gtk.TreeIter();
						using (Gtk.TreePath path = new Gtk.TreePath())
						{
							path.AppendIndex(0);
							path.Owned = false;
							for (int i = 0; i < _data.Count; i ++)
							{
								iter.UserData = (IntPtr)i;
								_adapter.EmitRowInserted(path, iter);
								path.Next();
							}
						}
					}
				}
			}
		}
		
		public Gtk.TreeModelAdapter Adapter
		{
			get { return _adapter; }
		}
		
		#endregion
		
		#region Methods

		public void Clear()
		{
			if (_data != null && _data.Count > 0)
			{
				List<T> data = _data;
				_data = null;
				using (Gtk.TreePath path = new Gtk.TreePath())
				{
					path.AppendIndex(data.Count - 1);
					path.Owned = false;
					for (int i = data.Count - 1; i >= 0; i --)
					{
						_adapter.EmitRowDeleted(path);
						path.Prev();
					}
				}
			}
		}
		
		public virtual int Compare(Gtk.TreeModel model, Gtk.TreeIter a, Gtk.TreeIter b)
		{
			int ret = -1;

			int i = (int)a.UserData;
			int j = (int)b.UserData;
			
			T itemA = default(T);
			T itemB = default(T);
			
			if (_data != null && i < _data.Count)
			{
				itemA = _data[i];
			}
			
			if (_data != null && j < _data.Count)
			{
				itemB = _data[j];
			}
			
			if (itemA != null && itemB != null)
			{
				if (itemA is IComparable)
				{
					ret = ((IComparable)itemA).CompareTo(itemB);
				}
			}
			else if (itemA != null)
			{
				ret = 1;	
			}
			
			return ret;
		}
		
		protected virtual bool Filter(T item)
		{
			return true;
		}
		
		public T GetValue(Gtk.TreeIter iter, int col)
		{
			T item = default(T);
			
			if (_data != null)
			{
				int i = (int)iter.UserData;
				if (i < _data.Count)
				{
					item = _data[i];
				}
			}
		
			return item;
		}
		
		public void ItemInserted()
		{
			ItemInserted(_data.Count - 1);
		}
		
		public void ItemInserted(int i)
		{
			T item = _data[i];
			
			Gtk.TreeIter iter = new Gtk.TreeIter();
			iter.UserData = (IntPtr)i;
			using (Gtk.TreePath path = GetPath(iter))
			{
				_adapter.EmitRowInserted(path, iter);
			}
		}
		
		public void ItemRemoved(int i)
		{
			using (Gtk.TreePath path = new Gtk.TreePath())
			{
				path.AppendIndex(i);
				path.Owned = false;
				
				ItemRemoved(path);
			}
		}
		
		public void ItemRemoved(Gtk.TreeIter iter)
		{
			int i = (int)iter.UserData;
			ItemRemoved(i);			
		}
		
		public void ItemRemoved(Gtk.TreePath path)
		{	
			_adapter.EmitRowDeleted(path);
		}

		
		public bool GetIter(out Gtk.TreeIter iter, int i)
		{
			bool ret = false;
			
			iter = Gtk.TreeIter.Zero;

			if (_data != null)
			{
				if (i < _data.Count)
				{
					iter = new Gtk.TreeIter();	
					iter.UserData = (IntPtr)i;
					ret = true;
				}
			}
			
			return ret;
		}
		
		#endregion
	}
}
