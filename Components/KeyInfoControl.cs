/******
Copyright (C) 2005 Ajit Krishnan (http://www.mudgala.com)

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
******/

using System;
using System.ComponentModel;
using System.Windows.Forms;
using Mhora.Calculation;
using Mhora.Components.Controls;
using Mhora.Dasas.NakshatraDasa;
using Mhora.Database.Settings;
using Mhora.Definitions;
using Mhora.Elements;
using Mhora.SwissEph;
using Mhora.Util;

namespace Mhora.Components;

/// <summary>
///     Summary description for KeyInfoControl.
/// </summary>
public class KeyInfoControl : MhoraControl
{
	/// <summary>
	///     Required designer variable.
	/// </summary>
	private readonly Container components = null;

	private ColumnHeader         Info;
	private ColumnHeader         Key;
	private MenuItem             menuItem1;
	private MenuItem             menuItem2;
	private ContextMenu          mKeyInfoMenu;
	private ListView             mList;
	private ListViewColumnSorter lvwColumnSorter;

	public KeyInfoControl(Horoscope _h)
	{
		// This call is required by the Windows.Forms Form Designer.
		InitializeComponent();

		// TODO: Add any initialization after the InitForm call
		h                                      =  _h;
		h.Changed                              += OnRecalculate;
		MhoraGlobalOptions.DisplayPrefsChanged += OnRedisplay;
		Repopulate();
		AddViewsToContextMenu(mKeyInfoMenu);
	}

	/// <summary>
	///     Clean up any resources being used.
	/// </summary>
	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			components?.Dispose();
		}

		base.Dispose(disposing);
	}

#region Component Designer generated code

	/// <summary>
	///     Required method for Designer support - do not modify
	///     the contents of this method with the code editor.
	/// </summary>
	private void InitializeComponent()
	{
			this.mList = new System.Windows.Forms.ListView();
			this.Key = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.Info = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.mKeyInfoMenu = new System.Windows.Forms.ContextMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// mList
			// 
			this.mList.AllowColumnReorder = true;
			this.mList.BackColor = System.Drawing.Color.Lavender;
			this.mList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Key,
            this.Info});
			this.mList.ContextMenu = this.mKeyInfoMenu;
			this.mList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.mList.ForeColor = System.Drawing.SystemColors.HotTrack;
			this.mList.FullRowSelect = true;
			this.mList.HideSelection = false;
			this.mList.Location = new System.Drawing.Point(0, 0);
			this.mList.Name = "mList";
			this.mList.Size = new System.Drawing.Size(496, 240);
			this.mList.TabIndex = 0;
			this.mList.UseCompatibleStateImageBehavior = false;
			this.mList.View = System.Windows.Forms.View.Details;
			this.mList.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.OnColumnclick);
			this.mList.SelectedIndexChanged += new System.EventHandler(this.mList_SelectedIndexChanged);
			//
			// Create an instance of a ListView column sorter and assign it
			// to the ListView control.
			this.lvwColumnSorter          = new ListViewColumnSorter();
			this.mList.ListViewItemSorter = lvwColumnSorter;
			// 
			// Key
			// 
			this.Key.Text = "Key";
			this.Key.Width = 136;
			// 
			// Info
			// 
			this.Info.Text = "Info";
			this.Info.Width = 350;
			// 
			// mKeyInfoMenu
			// 
			this.mKeyInfoMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem2});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.Text = "-";
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 1;
			this.menuItem2.Text = "-";
			// 
			// KeyInfoControl
			// 
			this.Controls.Add(this.mList);
			this.Name = "KeyInfoControl";
			this.Size = new System.Drawing.Size(496, 240);
			this.ResumeLayout(false);

	}

#endregion

	private void mList_SelectedIndexChanged(object sender, EventArgs e)
	{
	}

	protected override void copyToClipboard()
	{
		var iMaxDescLength = 0;
		for (var i = 0; i < mList.Items.Count; i++)
		{
			iMaxDescLength = Math.Max(mList.Items[i].Text.Length, iMaxDescLength);
		}

		iMaxDescLength += 2;

		var s = "Key Info: " + "\r\n\r\n";

		for (var i = 0; i < mList.Items.Count; i++)
		{
			var li = mList.Items[i];
			s += li.Text.PadRight(iMaxDescLength, ' ');
			s += "- ";
			for (var j = 1; j < li.SubItems.Count; j++)
			{
				s += li.SubItems[j].Text;
			}

			s += "\r\n";
		}

		Clipboard.SetDataObject(s);
	}

	private void Repopulate()
	{
		mList.Items.Clear();

		var li = new ListViewItem("Date of Birth");
		li.SubItems.Add(h.Info.DateOfBirth.ToString());
		mList.Items.Add(li);

		li = new ListViewItem("Time Zone");
		li.SubItems.Add(h.Info.City.Country.TimeZone.offsets[0]);
		mList.Items.Add(li);

		li = new ListViewItem("Latitude");
		li.SubItems.Add(h.Info.Latitude.ToString());
		mList.Items.Add(li);

		li = new ListViewItem("Longitude");
		li.SubItems.Add(h.Info.Longitude.ToString());
		mList.Items.Add(li);

		li = new ListViewItem("Altitude");
		li.SubItems.Add(h.Info.Altitude.ToString());
		mList.Items.Add(li);

		{
			var hms_srise = (TimeSpan) h.Sunrise;
			li = new ListViewItem("Sunrise");
			var fmt = string.Format("{0:00}:{1:00}:{2:00}", hms_srise.Hours, hms_srise.Minutes, hms_srise.Seconds);
			li.SubItems.Add(fmt);
			mList.Items.Add(li);
		}
		{
			var hms_sset = (TimeSpan)h.Sunset;
			li = new ListViewItem("Sunset");
			var fmt = string.Format("{0:00}:{1:00}:{2:00}", hms_sset.Hours, hms_sset.Minutes, hms_sset.Seconds);
			li.SubItems.Add(fmt);
			mList.Items.Add(li);
		}
		{
			li = new ListViewItem("Weekday");
			var fmt = string.Format("{0}", h.Wday);
			li.SubItems.Add(fmt);
			mList.Items.Add(li);
		}
		{
			var ltithi = h.GetPosition(Body.Moon).Longitude.Sub(h.GetPosition(Body.Sun).Longitude);
			var offset = 360.0 / 30.0 - ltithi.ToTithiOffset();
			var ti     = ltithi.ToTithi();
			var tiLord = ti.GetLord();
			li = new ListViewItem("Tithis");
			var fmt = string.Format("{0} ({1}) {2:N}% left", ti.GetEnumDescription(), tiLord, offset / 12.0 * 100);
			li.SubItems.Add(fmt);
			mList.Items.Add(li);
		}
		{
			var lmoon     = h.GetPosition(Body.Moon).Longitude;
			var nmoon     = lmoon.ToNakshatra();
			var nmoonLord = VimsottariDasa.NakshatraLord(nmoon);
			var offset    = 360.0 / 27.0 - lmoon.NakshatraOffset();
			var pada      = lmoon.NakshatraPada();
			var fmt       = string.Format("{0} {1} ({2}) {3:N}% left", nmoon.Name(), pada, nmoonLord, offset / (360.0 / 27.0) * 100);
			li = new ListViewItem("Nakshatra");
			li.SubItems.Add(fmt);
			mList.Items.Add(li);
		}
		{
			li = new ListViewItem("Karana");
			var lkarana = h.GetPosition(Body.Moon).Longitude.Sub(h.GetPosition(Body.Sun).Longitude);
			var koffset = 360.0 / 60.0 - lkarana.ToKaranaOffset();
			var k       = lkarana.ToKarana();
			var kLord   = k.GetLord();
			var fmt     = string.Format("{0} ({1}) {2:N}% left", k, kLord, koffset / 6.0 * 100);
			li.SubItems.Add(fmt);
			mList.Items.Add(li);
		}
		{
			li = new ListViewItem("Yoga");
			var smLon  = h.GetPosition(Body.Sun).Longitude.Add(h.GetPosition(Body.Moon).Longitude);
			var offset = 360.0 / 27.0 - smLon.ToSunMoonYogaOffset();
			var smYoga = smLon.ToSunMoonYoga();
			var smLord = smYoga.Lord();
			var fmt    = string.Format("{0} ({1}) {2:N}% left", smYoga, smLord, offset / (360.0 / 27.0) * 100);
			li.SubItems.Add(fmt);
			mList.Items.Add(li);
		}
		{
			li = new ListViewItem("Hora");
			var b   = h.CalculateHora();
			var fmt = string.Format("{0}", b);
			li.SubItems.Add(fmt);
			mList.Items.Add(li);
		}
		{
			li = new ListViewItem("Kala");
			var b   = h.CalculateKala();
			var fmt = string.Format("{0}", b);
			li.SubItems.Add(fmt);
			mList.Items.Add(li);
		}
		{
			li = new ListViewItem("Muhurta");
			var mIndex = (int) (Math.Floor(h.HoursAfterSunrise() / h.LengthOfDay() * 30.0) + 1);
			var m      = (Muhurta) mIndex;
			var fmt    = string.Format("{0} ({1})", m, m.NakLordOfMuhurta());
			li.SubItems.Add(fmt);
			mList.Items.Add(li);
		}
		{
			var ghatisSr = h.HoursAfterSunrise();
			var ghatisSs = h.HoursAfterSunRiseSet();
			li = new ListViewItem("Ghatis");
			var fmt = string.Format("{0:0.0000} / {1:0.0000}", ghatisSr.Ghati, ghatisSs.Ghati);
			li.SubItems.Add(fmt);
			mList.Items.Add(li);
		}
		{
			var vgOff = (int) Math.Ceiling(h.HoursAfterSunRiseSet() * 150.0);
			vgOff = vgOff % 9;
			if (vgOff == 0)
			{
				vgOff = 9;
			}

			var b = (Body) ((int) Body.Sun + vgOff - 1);
			li = new ListViewItem("Vighatika Graha");
			var fmt = string.Format("{0}", b);
			li.SubItems.Add(fmt);
			mList.Items.Add(li);
		}
		{
			li = new ListViewItem("LMT Offset");
			var e      = h.LmtOffset;
			var orig_e = e;
			e =  e < 0 ? -e : e;
			e *= 24.0;
			var hour = (int) Math.Floor(e);
			e = (e - Math.Floor(e)) * 60.0;
			var min = (int) Math.Floor(e);
			e = (e - Math.Floor(e)) * 60.0;
			var prefix = string.Empty;
			if (orig_e < 0)
			{
				prefix = "-";
			}

			var fmt  = string.Format("{0}{1:00}:{2:00}:{3:00.00}", prefix, hour, min, (double) e);
			var fmt2 = string.Format(" ({0:00.00} minutes)", (double) h.LmtOffset * 24.0 * 60.0);
			li.SubItems.Add(fmt + fmt2);
			mList.Items.Add(li);
		}
		{
			li = new ListViewItem("Ayanamsa");
			var aya      = sweph.GetAyanamsaUT(h.Info.Jd);
			var aya_hour = (int) Math.Floor(aya);
			aya = (aya - Math.Floor(aya)) * 60.0;
			var aya_min = (int) Math.Floor(aya);
			aya = (aya - Math.Floor(aya)) * 60.0;
			var fmt = string.Format("{0:00}-{1:00}-{2:00.00}", aya_hour, aya_min, aya);
			li.SubItems.Add(fmt);
			mList.Items.Add(li);
		}
		{
			li = new ListViewItem("Universal Time");
			li.SubItems.Add(h.Info.Jd.ToString());
			mList.Items.Add(li);
		}


		ColorAndFontRows(mList);
	}

	private void OnRedisplay(object o)
	{
		ColorAndFontRows(mList);
	}

	private void OnRecalculate(object o)
	{
		Repopulate();
	}

	private void OnColumnclick(object sender, ColumnClickEventArgs e)
	{
		// Determine if clicked column is already the column that is being sorted.
		if (e.Column == lvwColumnSorter.SortColumn)
		{
			// Reverse the current sort direction for this column.
			if (lvwColumnSorter.Order == SortOrder.Ascending)
			{
				lvwColumnSorter.Order = SortOrder.Descending;
			}
			else
			{
				lvwColumnSorter.Order = SortOrder.Ascending;
			}
		}
		else
		{
			// Set the column number that is to be sorted; default to ascending.
			lvwColumnSorter.SortColumn = e.Column;
			lvwColumnSorter.Order      = SortOrder.Ascending;
		}

		// Perform the sort with these new sort options.
		mList.Sort();
	}
}