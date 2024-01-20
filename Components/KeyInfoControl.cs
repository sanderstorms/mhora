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
using Mhora.Database.Settings;
using Mhora.Elements;
using Mhora.Elements.Calculation;
using Mhora.Elements.Dasas.Nakshatra;
using Mhora.Elements.Hora;
using Mhora.SwissEph;
using Mhora.Tables;
using mhora.Util;

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

	private ColumnHeader Info;
	private ColumnHeader Key;
	private MenuItem     menuItem1;
	private MenuItem     menuItem2;
	private ContextMenu  mKeyInfoMenu;
	private ListView     mList;

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
			if (components != null)
			{
				components.Dispose();
			}
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
		this.mList        = new System.Windows.Forms.ListView();
		this.Key          = new System.Windows.Forms.ColumnHeader();
		this.Info         = new System.Windows.Forms.ColumnHeader();
		this.mKeyInfoMenu = new System.Windows.Forms.ContextMenu();
		this.menuItem1    = new System.Windows.Forms.MenuItem();
		this.menuItem2    = new System.Windows.Forms.MenuItem();
		this.SuspendLayout();
		// 
		// mList
		// 
		this.mList.AllowColumnReorder = true;
		this.mList.BackColor          = System.Drawing.Color.Lavender;
		this.mList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[]
		{
			this.Key,
			this.Info
		});
		this.mList.ContextMenu          =  this.mKeyInfoMenu;
		this.mList.Dock                 =  System.Windows.Forms.DockStyle.Fill;
		this.mList.Font                 =  new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
		this.mList.ForeColor            =  System.Drawing.SystemColors.HotTrack;
		this.mList.FullRowSelect        =  true;
		this.mList.Location             =  new System.Drawing.Point(0, 0);
		this.mList.Name                 =  "mList";
		this.mList.Size                 =  new System.Drawing.Size(496, 240);
		this.mList.TabIndex             =  0;
		this.mList.View                 =  System.Windows.Forms.View.Details;
		this.mList.SelectedIndexChanged += new System.EventHandler(this.mList_SelectedIndexChanged);
		// 
		// Key
		// 
		this.Key.Text  = "Key";
		this.Key.Width = 136;
		// 
		// Info
		// 
		this.Info.Text  = "Info";
		this.Info.Width = 350;
		// 
		// mKeyInfoMenu
		// 
		this.mKeyInfoMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[]
		{
			this.menuItem1,
			this.menuItem2
		});
		// 
		// menuItem1
		// 
		this.menuItem1.Index = 0;
		this.menuItem1.Text  = "-";
		// 
		// menuItem2
		// 
		this.menuItem2.Index = 1;
		this.menuItem2.Text  = "-";
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

		ListViewItem li;

		li = new ListViewItem("Date of Birth");
		li.SubItems.Add(h.info.DateOfBirth.ToString());
		mList.Items.Add(li);

		li = new ListViewItem("Time Zone");
		li.SubItems.Add(h.info.City.Country.TimeZone.offsets[0]);
		mList.Items.Add(li);

		li = new ListViewItem("Latitude");
		li.SubItems.Add(h.info.Latitude.ToString());
		mList.Items.Add(li);

		li = new ListViewItem("Longitude");
		li.SubItems.Add(h.info.Longitude.ToString());
		mList.Items.Add(li);

		li = new ListViewItem("Altitude");
		li.SubItems.Add(h.info.Altitude.ToString());
		mList.Items.Add(li);

		{
			var hms_srise = new HMSInfo(h.sunrise);
			li = new ListViewItem("Sunrise");
			var fmt = string.Format("{0:00}:{1:00}:{2:00}", hms_srise.degree, hms_srise.minute, hms_srise.second);
			li.SubItems.Add(fmt);
			mList.Items.Add(li);
		}
		{
			var hms_sset = new HMSInfo(h.sunset);
			li = new ListViewItem("Sunset");
			var fmt = string.Format("{0:00}:{1:00}:{2:00}", hms_sset.degree, hms_sset.minute, hms_sset.second);
			li.SubItems.Add(fmt);
			mList.Items.Add(li);
		}
		{
			li = new ListViewItem("Weekday");
			var fmt = string.Format("{0}", h.wday);
			li.SubItems.Add(fmt);
			mList.Items.Add(li);
		}
		{
			var ltithi = h.getPosition(Body.BodyType.Moon).longitude.sub(h.getPosition(Body.BodyType.Sun).longitude);
			var offset = 360.0 / 30.0 - ltithi.toTithiOffset();
			var ti     = ltithi.toTithi();
			var tiLord = ti.GetLord();
			li = new ListViewItem("Tithis");
			var fmt = string.Format("{0} ({1}) {2:N}% left", ti.GetEnumDescription(), tiLord, offset / 12.0 * 100);
			li.SubItems.Add(fmt);
			mList.Items.Add(li);
		}
		{
			var lmoon     = h.getPosition(Body.BodyType.Moon).longitude;
			var nmoon     = lmoon.toNakshatra();
			var nmoonLord = VimsottariDasa.LordOfNakshatra(nmoon);
			var offset    = 360.0 / 27.0 - lmoon.toNakshatraOffset();
			var pada      = lmoon.toNakshatraPada();
			var fmt       = string.Format("{0} {1} ({2}) {3:N}% left", nmoon.Name(), pada, nmoonLord, offset / (360.0 / 27.0) * 100);
			li = new ListViewItem("Nakshatra");
			li.SubItems.Add(fmt);
			mList.Items.Add(li);
		}
		{
			li = new ListViewItem("Karana");
			var lkarana = h.getPosition(Body.BodyType.Moon).longitude.sub(h.getPosition(Body.BodyType.Sun).longitude);
			var koffset = 360.0 / 60.0 - lkarana.toKaranaOffset();
			var k       = lkarana.toKarana();
			var kLord   = k.getLord();
			var fmt     = string.Format("{0} ({1}) {2:N}% left", k, kLord, koffset / 6.0 * 100);
			li.SubItems.Add(fmt);
			mList.Items.Add(li);
		}
		{
			li = new ListViewItem("Yoga");
			var smLon  = h.getPosition(Body.BodyType.Sun).longitude.add(h.getPosition(Body.BodyType.Moon).longitude);
			var offset = 360.0 / 27.0 - smLon.toSunMoonYogaOffset();
			var smYoga = smLon.toSunMoonYoga();
			var smLord = smYoga.getLord();
			var fmt    = string.Format("{0} ({1}) {2:N}% left", smYoga, smLord, offset / (360.0 / 27.0) * 100);
			li.SubItems.Add(fmt);
			mList.Items.Add(li);
		}
		{
			li = new ListViewItem("Hora");
			var b   = h.calculateHora();
			var fmt = string.Format("{0}", b);
			li.SubItems.Add(fmt);
			mList.Items.Add(li);
		}
		{
			li = new ListViewItem("Kala");
			var b   = h.calculateKala();
			var fmt = string.Format("{0}", b);
			li.SubItems.Add(fmt);
			mList.Items.Add(li);
		}
		{
			li = new ListViewItem("Muhurta");
			var mIndex = (int) (Math.Floor(h.hoursAfterSunrise() / h.lengthOfDay() * 30.0) + 1);
			var m      = (Muhurtas.Muhurta) mIndex;
			var fmt    = string.Format("{0} ({1})", m, m.NakLordOfMuhurta());
			li.SubItems.Add(fmt);
			mList.Items.Add(li);
		}
		{
			var ghatisSr = h.hoursAfterSunrise()    * 2.5;
			var ghatisSs = h.hoursAfterSunRiseSet() * 2.5;
			li = new ListViewItem("Ghatis");
			var fmt = string.Format("{0:0.0000} / {1:0.0000}", ghatisSr, ghatisSs);
			li.SubItems.Add(fmt);
			mList.Items.Add(li);
		}
		{
			var vgOff = (int) Math.Ceiling(h.hoursAfterSunRiseSet() * 150.0);
			vgOff = vgOff % 9;
			if (vgOff == 0)
			{
				vgOff = 9;
			}

			var b = (Body.BodyType) ((int) Body.BodyType.Sun + vgOff - 1);
			li = new ListViewItem("Vighatika Graha");
			var fmt = string.Format("{0}", b);
			li.SubItems.Add(fmt);
			mList.Items.Add(li);
		}
		{
			li = new ListViewItem("LMT Offset");
			var e      = h.lmt_offset;
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
			var fmt2 = string.Format(" ({0:00.00} minutes)", (double) h.lmt_offset * 24.0 * 60.0);
			li.SubItems.Add(fmt + fmt2);
			mList.Items.Add(li);
		}
		{
			li = new ListViewItem("Ayanamsa");
			var aya      = sweph.GetAyanamsaUT(h.info.Jd);
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
			li.SubItems.Add(h.info.Jd.ToString());
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
}