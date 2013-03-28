/*
 *  $Id: MainWindow.cs 199 2008-11-15 15:20:44Z davek $
 * 
 * Copyright (C) 2008 David A Knight <david@ritter.demon.co.uk>
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GedcomParser;
using Gedcom.UI.Common;

namespace Gedcom.UI.SWF
{
	public partial class MainWindow : Form
	{
		#region Variables

		private AboutBox _aboutDialog = null;

		private GedcomDatabase _database;
		private GedcomRecord _record;
		private string _gedcomFile = string.Empty;

		private IGedcomView _currentView;

		#endregion

		#region Constructors

		public MainWindow()
		{
			InitializeComponent();
		}

		#endregion

		#region Properties

		public GedcomRecord Record
		{
			get { return _record; }
			set
			{
				if (_record != null && _currentView != null)
				{
					_currentView.SaveView();
				}
				_record = value;

				if (_currentView != null)
				{
					_currentView.Record = _record;
				}
			}
		}

		#endregion

		#region Event Handlers

		private void quitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
		{
			Application.Exit();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_aboutDialog == null)
			{
				_aboutDialog = new AboutBox();
				_aboutDialog.FormClosed += new FormClosedEventHandler(_aboutDialog_FormClosed);
				_aboutDialog.Show(this);
			}

			_aboutDialog.BringToFront();
		}

		private void _aboutDialog_FormClosed(object sender, FormClosedEventArgs e)
		{
			_aboutDialog = null;
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DialogResult result = openFileDialog1.ShowDialog(this);

			if (result == DialogResult.OK)
			{
				DoReadGedcom(openFileDialog1.FileName, false);
			}
		}


		private void DoReadGedcom_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			GedcomRecordReader reader = (GedcomRecordReader)sender;

			toolStripProgressBar1.ProgressBar.Value = (int)e.ProgressPercentage;
		}

		private void DoMergeGedcom_Completed(object sender, RunWorkerCompletedEventArgs e)
		{
			GedcomRecordReader reader = (GedcomRecordReader)sender;

			if ((!e.Cancelled) && (e.Error == null))
			{
				// FIXME: show ui to remove duplicates prior to combining
				_database.Combine(reader.Database);
			}
			toolStripStatusLabel1.Text = string.Empty;
			toolStripProgressBar1.Value = 0;
		}

		private void DoReadGedcom_Completed(object sender, RunWorkerCompletedEventArgs e)
		{
			GedcomRecordReader reader = (GedcomRecordReader)sender;

			if ((!e.Cancelled) && (e.Error == null))
			{
				SetGedcomDatabase(reader.Database);
			}
			toolStripStatusLabel1.Text = string.Empty;
			toolStripProgressBar1.Value = 0;
		}

		#endregion

		#region Methods

		private void DoReadGedcom(string gedcomFile, bool merge)
		{
			toolStripStatusLabel1.Text = "Reading " + gedcomFile;

			BackgroundGedcomRecordReader reader = new BackgroundGedcomRecordReader();

			if (!merge)
			{
				reader.Completed += new RunWorkerCompletedEventHandler(DoReadGedcom_Completed);
			}
			else
			{
				reader.Completed += new RunWorkerCompletedEventHandler(DoMergeGedcom_Completed);
			}

			reader.ProgressChanged += new ProgressChangedEventHandler(DoReadGedcom_ProgressChanged);

			reader.ReadGedcom(gedcomFile);
		}

		private void SetGedcomDatabase(GedcomDatabase database)
		{
			_database = null;
			_record = null;

			_database = database;

			_record = _database.Individuals[0];

			if (_currentView != null)
			{
				_currentView.Database = _database;
				_currentView.Record = _record;
			}
		}

		private void DoSaveGedcom(string gedcomFile)
		{
			if (_currentView != null)
			{
				_currentView.SaveView();
			}

			GedcomRecordWriter writer = new GedcomRecordWriter();
			writer.Database = _database;
			writer.GedcomFile = gedcomFile;
			writer.WriteGedcom();

			_gedcomFile = gedcomFile;

			_database.Name = _gedcomFile;
		}

		private void RefreshView()
		{
			// force refresh of current, FIXME: yuk
			GedcomRecord rec;

			if (_currentView != null)
			{
				rec = _currentView.Record;
			}
			else
			{
				rec = _record;
			}
			
			_record = null;
			Record = rec;
		}

		#endregion

	}
}
