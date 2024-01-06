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
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Mhora.Components.Dasa.Nakshatra;
using Mhora.Components.Delegates;
using Mhora.Components.Property;
using Mhora.Components.Varga;
using Mhora.Database.Settings;
using Mhora.Elements;
using Mhora.Elements.Calculation;
using Mhora.Tables;
using Tithi = Mhora.Tables.Tithi;

namespace Mhora.Components;

/// <summary>
///     Summary description for BasicCalculationsControl.
/// </summary>
public class BasicCalculationsControl : MhoraControl
{
	/// <summary>
	///     Required designer variable.
	/// </summary>
	private readonly Container components = null;


	private readonly UserOptions options;


	private ColumnHeader Body;
	private ContextMenu  calculationsContextMenu;

	private ColumnHeader Longitude;
	private MenuItem     menu64Navamsa;
	private MenuItem     menuAstroInfo;
	private MenuItem     menuAvasthas;
	private MenuItem     menuBasicGrahas;
	private MenuItem     menuBhavaCusps;


	private MenuItem     menuChangeVarga;
	private MenuItem     menuCharaKarakas;
	private MenuItem     menuCharaKarakas7;
	private MenuItem     menuCopyLon;
	private MenuItem     menuItem1;
	private MenuItem     menuItem2;
	private MenuItem     menuMrityuLongitudes;
	private MenuItem     menuNakshatraAspects;
	private MenuItem     menuNonLonBodies;
	private MenuItem     menuOtherLongitudes;
	private MenuItem     menuSahamaLongitudes;
	private MenuItem     menuSpecialTaras;
	private MenuItem     menuSpecialTithis;
	private ListView     mList;
	private ColumnHeader Nakshatra;
	private ColumnHeader Pada;

	private ViewType vt;

	public BasicCalculationsControl(Horoscope _h)
	{
		// This call is required by the Windows.Forms Form Designer.
		InitializeComponent();

		// TODO: Add any initialization after the InitForm call
		h                                      =  _h;
		vt                                     =  ViewType.ViewBasicGrahas;
		menuBasicGrahas.Checked                =  true;
		h.Changed                              += OnRecalculate;
		MhoraGlobalOptions.DisplayPrefsChanged += OnRedisplay;
		options                                =  new UserOptions();
		options.DivisionType                   =  new Division(Basics.DivisionType.Rasi);
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
		this.mList                   = new System.Windows.Forms.ListView();
		this.Body                    = new System.Windows.Forms.ColumnHeader();
		this.Longitude               = new System.Windows.Forms.ColumnHeader();
		this.Nakshatra               = new System.Windows.Forms.ColumnHeader();
		this.Pada                    = new System.Windows.Forms.ColumnHeader();
		this.calculationsContextMenu = new System.Windows.Forms.ContextMenu();
		this.menuChangeVarga         = new System.Windows.Forms.MenuItem();
		this.menuCopyLon             = new System.Windows.Forms.MenuItem();
		this.menuBasicGrahas         = new System.Windows.Forms.MenuItem();
		this.menuOtherLongitudes     = new System.Windows.Forms.MenuItem();
		this.menuMrityuLongitudes    = new System.Windows.Forms.MenuItem();
		this.menuSahamaLongitudes    = new System.Windows.Forms.MenuItem();
		this.menuNonLonBodies        = new System.Windows.Forms.MenuItem();
		this.menuCharaKarakas        = new System.Windows.Forms.MenuItem();
		this.menuCharaKarakas7       = new System.Windows.Forms.MenuItem();
		this.menu64Navamsa           = new System.Windows.Forms.MenuItem();
		this.menuAstroInfo           = new System.Windows.Forms.MenuItem();
		this.menuSpecialTithis       = new System.Windows.Forms.MenuItem();
		this.menuSpecialTaras        = new System.Windows.Forms.MenuItem();
		this.menuNakshatraAspects    = new System.Windows.Forms.MenuItem();
		this.menuBhavaCusps          = new System.Windows.Forms.MenuItem();
		this.menuAvasthas            = new System.Windows.Forms.MenuItem();
		this.menuItem1               = new System.Windows.Forms.MenuItem();
		this.menuItem2               = new System.Windows.Forms.MenuItem();
		this.SuspendLayout();
		// 
		// mList
		// 
		this.mList.AllowDrop = true;
		this.mList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[]
		{
			this.Body,
			this.Longitude,
			this.Nakshatra,
			this.Pada
		});
		this.mList.ContextMenu          =  this.calculationsContextMenu;
		this.mList.Dock                 =  System.Windows.Forms.DockStyle.Fill;
		this.mList.Font                 =  new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
		this.mList.ForeColor            =  System.Drawing.SystemColors.HotTrack;
		this.mList.FullRowSelect        =  true;
		this.mList.Location             =  new System.Drawing.Point(0, 0);
		this.mList.Name                 =  "mList";
		this.mList.Size                 =  new System.Drawing.Size(496, 176);
		this.mList.TabIndex             =  0;
		this.mList.View                 =  System.Windows.Forms.View.Details;
		this.mList.MouseHover           += new System.EventHandler(this.mList_MouseHover);
		this.mList.DragDrop             += new System.Windows.Forms.DragEventHandler(this.mList_DragDrop);
		this.mList.DragEnter            += new System.Windows.Forms.DragEventHandler(this.mList_DragEnter);
		this.mList.SelectedIndexChanged += new System.EventHandler(this.mList_SelectedIndexChanged);
		// 
		// Body
		// 
		this.Body.Text  = "Body";
		this.Body.Width = 100;
		// 
		// Longitude
		// 
		this.Longitude.Text  = "Longitude";
		this.Longitude.Width = 120;
		// 
		// Nakshatra
		// 
		this.Nakshatra.Text  = "Nakshatra";
		this.Nakshatra.Width = 120;
		// 
		// Pada
		// 
		this.Pada.Text  = "Pada";
		this.Pada.Width = 50;
		// 
		// calculationsContextMenu
		// 
		this.calculationsContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[]
		{
			this.menuChangeVarga,
			this.menuCopyLon,
			this.menuBasicGrahas,
			this.menuOtherLongitudes,
			this.menuMrityuLongitudes,
			this.menuSahamaLongitudes,
			this.menuNonLonBodies,
			this.menuCharaKarakas,
			this.menuCharaKarakas7,
			this.menu64Navamsa,
			this.menuAstroInfo,
			this.menuSpecialTithis,
			this.menuSpecialTaras,
			this.menuNakshatraAspects,
			this.menuBhavaCusps,
			this.menuAvasthas,
			this.menuItem1,
			this.menuItem2
		});
		this.calculationsContextMenu.Popup += new System.EventHandler(this.calculationsContextMenu_Popup);
		// 
		// menuChangeVarga
		// 
		this.menuChangeVarga.Index =  0;
		this.menuChangeVarga.Text  =  "Options";
		this.menuChangeVarga.Click += new System.EventHandler(this.menuChangeVarga_Click);
		// 
		// menuCopyLon
		// 
		this.menuCopyLon.Index =  1;
		this.menuCopyLon.Text  =  "Copy Longitude";
		this.menuCopyLon.Click += new System.EventHandler(this.menuCopyLon_Click);
		// 
		// menuBasicGrahas
		// 
		this.menuBasicGrahas.Index =  2;
		this.menuBasicGrahas.Text  =  "Basic Longitudes";
		this.menuBasicGrahas.Click += new System.EventHandler(this.menuBasicGrahas_Click);
		// 
		// menuOtherLongitudes
		// 
		this.menuOtherLongitudes.Index =  3;
		this.menuOtherLongitudes.Text  =  "Other Longitudes";
		this.menuOtherLongitudes.Click += new System.EventHandler(this.menuOtherLongitudes_Click);
		// 
		// menuMrityuLongitudes
		// 
		this.menuMrityuLongitudes.Index =  4;
		this.menuMrityuLongitudes.Text  =  "Mrityu Longitudes";
		this.menuMrityuLongitudes.Click += new System.EventHandler(this.menuMrityuLongitudes_Click);
		// 
		// menuSahamaLongitudes
		// 
		this.menuSahamaLongitudes.Index =  5;
		this.menuSahamaLongitudes.Text  =  "Sahama Longitudes";
		this.menuSahamaLongitudes.Click += new System.EventHandler(this.menuSahamaLongitudes_Click);
		// 
		// menuNonLonBodies
		// 
		this.menuNonLonBodies.Index =  6;
		this.menuNonLonBodies.Text  =  "Non-Longitude Bodies";
		this.menuNonLonBodies.Click += new System.EventHandler(this.menuNonLonBodies_Click);
		// 
		// menuCharaKarakas
		// 
		this.menuCharaKarakas.Index =  7;
		this.menuCharaKarakas.Text  =  "Chara Karakas (8)";
		this.menuCharaKarakas.Click += new System.EventHandler(this.menuCharaKarakas_Click);
		// 
		// menuCharaKarakas7
		// 
		this.menuCharaKarakas7.Index =  8;
		this.menuCharaKarakas7.Text  =  "Chara Karakas (7)";
		this.menuCharaKarakas7.Click += new System.EventHandler(this.menuCharaKarakas7_Click);
		// 
		// menu64Navamsa
		// 
		this.menu64Navamsa.Index =  9;
		this.menu64Navamsa.Text  =  "64th Navamsa";
		this.menu64Navamsa.Click += new System.EventHandler(this.menu64Navamsa_Click);
		// 
		// menuAstroInfo
		// 
		this.menuAstroInfo.Index =  10;
		this.menuAstroInfo.Text  =  "Astronomical Info";
		this.menuAstroInfo.Click += new System.EventHandler(this.menuAstroInfo_Click);
		// 
		// menuSpecialTithis
		// 
		this.menuSpecialTithis.Index =  11;
		this.menuSpecialTithis.Text  =  "Special Tithis";
		this.menuSpecialTithis.Click += new System.EventHandler(this.menuSpecialTithis_Click);
		// 
		// menuSpecialTaras
		// 
		this.menuSpecialTaras.Index =  12;
		this.menuSpecialTaras.Text  =  "Special Nakshatras";
		this.menuSpecialTaras.Click += new System.EventHandler(this.menuSpecialTaras_Click);
		// 
		// menuNakshatraAspects
		// 
		this.menuNakshatraAspects.Index =  13;
		this.menuNakshatraAspects.Text  =  "Nakshatra Aspects";
		this.menuNakshatraAspects.Click += new System.EventHandler(this.menuNakshatraAspects_Click);
		// 
		// menuBhavaCusps
		// 
		this.menuBhavaCusps.Index =  14;
		this.menuBhavaCusps.Text  =  "Bhava Cusps";
		this.menuBhavaCusps.Click += new System.EventHandler(this.menuBhavaCusps_Click);
		// 
		// menuAvasthas
		// 
		this.menuAvasthas.Index =  15;
		this.menuAvasthas.Text  =  "Avasthas";
		this.menuAvasthas.Click += new System.EventHandler(this.menuAvasthas_Click);
		// 
		// menuItem1
		// 
		this.menuItem1.Index = 16;
		this.menuItem1.Text  = "-";
		// 
		// menuItem2
		// 
		this.menuItem2.Index = 17;
		this.menuItem2.Text  = "-";
		// 
		// BasicCalculationsControl
		// 
		this.ContextMenu = this.calculationsContextMenu;
		this.Controls.Add(this.mList);
		this.Name =  "BasicCalculationsControl";
		this.Size =  new System.Drawing.Size(496, 176);
		this.Load += new System.EventHandler(this.BasicCalculationsControl_Load);
		this.ResumeLayout(false);
	}

#endregion

	private void BasicCalculationsControl_Load(object sender, EventArgs e)
	{
		AddViewsToContextMenu(calculationsContextMenu);
		Repopulate();
	}

	private void ResizeColumns()
	{
		for (var i = 0; i < mList.Columns.Count; i++)
		{
			mList.Columns[i].Width = -1;
		}

		mList.Columns[mList.Columns.Count - 1].Width = -2;
	}

	private string getTithiName(double val, ref double tithi, ref double perc)
	{
		var offset = val;
		while (offset >= 12.0)
		{
			offset -= 12.0;
		}

		var t = (int) Math.Floor(val / 12.0) + 1;
		tithi = t;
		perc  = 100 - offset / 12.0 * 100;
		if (t == 15)
		{
			return "Pournima";
		}

		if (t == 30)
		{
			return "Amavasya";
		}

		string str;
		if (t > 15)
		{
			str =  "Krishna ";
			t   -= 15;
		}
		else
		{
			str = "Shukla ";
		}

		return str + " " + Tithi.Name[t - 1];
	}

	private void RepopulateSpecialTaras()
	{
		mList.Columns.Clear();
		mList.Items.Clear();
		mList.Columns.Add("Type", -1, HorizontalAlignment.Left);
		mList.Columns.Add("Nakshatra (27)", -1, HorizontalAlignment.Left);
		mList.Columns.Add("Nakshatra (28)", -1, HorizontalAlignment.Left);

		var nmoon   = h.getPosition(Elements.Body.Name.Moon).longitude.toNakshatra();
		var nmoon28 = h.getPosition(Elements.Body.Name.Moon).longitude.toNakshatra28();
		for (var i = 0; i < Tara.specialIndices.Length; i++)
		{
			var sn   = nmoon.add(Tara.specialIndices[i]);
			var sn28 = nmoon28.add(Tara.specialIndices[i]);

			var li = new ListViewItem();
			li.Text = string.Format("{0:00}  {1} Tara", Tara.specialIndices[i], Tara.specialNames[i]);
			li.SubItems.Add(sn.value.ToString());
			li.SubItems.Add(sn28.value.ToString());
			mList.Items.Add(li);
		}

		ColorAndFontRows(mList);
		ResizeColumns();
	}

	private void RepopulateCharaKarakas()
	{
		mList.Clear();
		mList.Columns.Add("Body", -1, HorizontalAlignment.Left);
		mList.Columns.Add("Karaka", -1, HorizontalAlignment.Left);
		mList.Columns.Add("Offset", -2, HorizontalAlignment.Left);

		var al  = new ArrayList();
		var max = 0;
		if (vt == ViewType.ViewCharaKarakas)
		{
			max = (int) Elements.Body.Name.Rahu;
		}
		else
		{
			max = (int) Elements.Body.Name.Saturn;
		}


		for (var i = (int) Elements.Body.Name.Sun; i <= max; i++)
		{
			var b   = (Body.Name) i;
			var bp  = h.getPosition(b);
			var bkc = new KarakaComparer(bp);
			al.Add(bkc);
		}

		al.Sort();

		for (var i = 0; i < al.Count; i++)
		{
			var li = new ListViewItem();
			var bk = (KarakaComparer) al[i];
			li.Text = Elements.Body.toString(bk.GetPosition.name);
			if (vt == ViewType.ViewCharaKarakas)
			{
				li.SubItems.Add(Karaka.karakas[i]);
			}
			else
			{
				li.SubItems.Add(Karaka.karakas7[i]);
			}

			li.SubItems.Add(string.Format("{0:0.00}", bk.getOffset()));
			mList.Items.Add(li);
		}


		ColorAndFontRows(mList);
		ResizeColumns();
	}


	private void Repopulate64NavamsaHelper(Body.Name b, string name, Position bp, Division div)
	{
		var dp = bp.toDivisionPosition(div);
		var li = new ListViewItem();
		li.Text = b.ToString();
		li.SubItems.Add(name);
		li.SubItems.Add(dp.zodiac_house.value.ToString());
		li.SubItems.Add(Elements.Body.toString(h.LordOfZodiacHouse(dp.zodiac_house, div)));
		mList.Items.Add(li);
	}

	private void RepopulateNonLonBodies()
	{
		mList.Clear();
		mList.Columns.Add("Non Longitudinal Body", -1, HorizontalAlignment.Left);
		mList.Columns.Add("Zodiac House", -2, HorizontalAlignment.Left);

		var al = h.CalculateArudhaDivisionPositions(options.DivisionType);
		al.AddRange(h.CalculateVarnadaDivisionPositions(options.DivisionType));

		foreach (DivisionPosition dp in al)
		{
			var desc = string.Empty;
			if (dp.name == Elements.Body.Name.Other)
			{
				desc = dp.Description;
			}
			else
			{
				desc = dp.name.ToString();
			}

			var li = new ListViewItem(desc);
			li.SubItems.Add(dp.zodiac_house.value.ToString());
			mList.Items.Add(li);
		}

		ColorAndFontRows(mList);
		ResizeColumns();
	}

	private void Repopulate64Navamsa()
	{
		mList.Clear();
		mList.Columns.Add("Reference", -1, HorizontalAlignment.Left);
		mList.Columns.Add("Count", -1, HorizontalAlignment.Left);
		mList.Columns.Add("Rasi", -1, HorizontalAlignment.Left);
		mList.Columns.Add("Lord", -2, HorizontalAlignment.Left);

		Body.Name[] bodyReferences =
		{
			Elements.Body.Name.Lagna,
			Elements.Body.Name.Moon,
			Elements.Body.Name.Sun
		};

		foreach (var b in bodyReferences)
		{
			var bp    = (Position) h.getPosition(b).Clone();
			var bpLon = bp.longitude.add(0);

			bp.longitude = bpLon.add(30.0 / 9.0 * (64 - 1));
			Repopulate64NavamsaHelper(b, "64th Navamsa", bp, new Division(Basics.DivisionType.Navamsa));

			bp.longitude = bpLon.add(30.0 / 3.0 * (22 - 1));
			Repopulate64NavamsaHelper(b, "22nd Drekkana", bp, new Division(Basics.DivisionType.DrekkanaParasara));
			Repopulate64NavamsaHelper(b, "22nd Drekkana (Parivritti)", bp, new Division(Basics.DivisionType.DrekkanaParivrittitraya));
			Repopulate64NavamsaHelper(b, "22nd Drekkana (Somnath)", bp, new Division(Basics.DivisionType.DrekkanaSomnath));
			Repopulate64NavamsaHelper(b, "22nd Drekkana (Jagannath)", bp, new Division(Basics.DivisionType.DrekkanaJagannath));
		}

		ColorAndFontRows(mList);
		ResizeColumns();
	}

	private void RepopulateAvasthas()
	{
		mList.Clear();
		mList.Columns.Add("Body", -1, HorizontalAlignment.Left);
		mList.Columns.Add("Age", -1, HorizontalAlignment.Left);
		mList.Columns.Add("Alertness", -1, HorizontalAlignment.Left);
		mList.Columns.Add("Mood", -2, HorizontalAlignment.Left);

		for (var i = (int) Elements.Body.Name.Sun; i <= (int) Elements.Body.Name.Ketu; i++)
		{
			var b  = (Body.Name) i;
			var li = new ListViewItem();
			li.Text = Elements.Body.toString(b);
			var dp            = h.getPosition(b).toDivisionPosition(new Division(Basics.DivisionType.Panchamsa));
			var avastha_index = -1;
			switch ((int) dp.zodiac_house.value % 2)
			{
				case 1:
					avastha_index = dp.part;
					break;
				case 0:
					avastha_index = 6 - dp.part;
					break;
			}

			li.SubItems.Add(Tables.Varga.Avasthas[avastha_index - 1]);
			mList.Items.Add(li);
		}

		ColorAndFontRows(mList);
		ResizeColumns();
	}

	private void RepopulateNakshatraAspects()
	{
		mList.Clear();
		mList.Columns.Add("Body", -1, HorizontalAlignment.Left);
		mList.Columns.Add("Latta", -1, HorizontalAlignment.Left);
		mList.Columns.Add("Aspected", -2, HorizontalAlignment.Left);

		for (var i = (int) Elements.Body.Name.Sun; i <= (int) Elements.Body.Name.Rahu; i++)
		{
			var b          = (Body.Name) i;
			var dirForward = true;
			if (i % 2 == 1)
			{
				dirForward = false;
			}

			var       bp = h.getPosition(b);
			var       n  = bp.longitude.toNakshatra();
			Nakshatra l  = null;

			if (dirForward)
			{
				l = n.add(Karaka.latta_aspects[i]);
			}
			else
			{
				l = n.addReverse(Karaka.latta_aspects[i]);
			}

			var nak_fmt = string.Empty;
			foreach (var j in Karaka.tara_aspects[i])
			{
				var na = n.add(j);
				if (nak_fmt.Length > 0)
				{
					nak_fmt = string.Format("{0}, {1}-{2}", nak_fmt, j, na.value);
				}
				else
				{
					nak_fmt = string.Format("{0}-{1}", j, na.value);
				}
			}

			var li  = new ListViewItem(Elements.Body.toString(b));
			var fmt = string.Format("{0:00}-{1}", Karaka.latta_aspects[i], l.value);
			li.SubItems.Add(fmt);
			li.SubItems.Add(nak_fmt);
			mList.Items.Add(li);
		}

		ColorAndFontRows(mList);
		ResizeColumns();
	}

	private void RepopulateAstronomicalInfo()
	{
		mList.Clear();
		mList.Columns.Add("Body", -1, HorizontalAlignment.Left);
		mList.Columns.Add("Lon (deg)", -1, HorizontalAlignment.Left);
		mList.Columns.Add("/ day", -1, HorizontalAlignment.Left);
		mList.Columns.Add("Lat (deb)", -1, HorizontalAlignment.Left);
		mList.Columns.Add("/ day", -1, HorizontalAlignment.Left);
		mList.Columns.Add("Distance (AU)", -1, HorizontalAlignment.Left);
		mList.Columns.Add("/ day", -1, HorizontalAlignment.Left);

		for (var i = (int) Elements.Body.Name.Sun; i <= (int) Elements.Body.Name.Saturn; i++)
		{
			var b  = (Body.Name) i;
			var bp = h.getPosition(b);
			var li = new ListViewItem(Elements.Body.toString(b));
			li.SubItems.Add(bp.longitude.value.ToString());
			li.SubItems.Add(bp.speed_longitude.ToString());
			li.SubItems.Add(bp.latitude.ToString());
			li.SubItems.Add(bp.speed_latitude.ToString());
			li.SubItems.Add(bp.distance.ToString());
			li.SubItems.Add(bp.speed_distance.ToString());
			mList.Items.Add(li);
		}

		ColorAndFontRows(mList);
		ResizeColumns();
	}

	private void RepopulateSpecialTithis()
	{
		mList.Columns.Clear();
		mList.Items.Clear();
		mList.Columns.Add("Type", -1, HorizontalAlignment.Left);
		mList.Columns.Add("Tithi", -1, HorizontalAlignment.Left);
		mList.Columns.Add("% Left", -1, HorizontalAlignment.Left);

		var spos      = h.getPosition(Elements.Body.Name.Sun).longitude;
		var mpos      = h.getPosition(Elements.Body.Name.Moon).longitude;
		var baseTithi = mpos.sub(spos).value;
		for (var i = 1; i <= 12; i++)
		{
			var    spTithiVal = new Longitude(baseTithi * i).value;
			double tithi      = 0;
			double perc       = 0;
			var    li         = new ListViewItem();
			var    s1         = string.Format("{0:00}  {1} Tithi", i, Tithi.specialNames[i]);
			li.Text = s1;
			var s2 = getTithiName(spTithiVal, ref tithi, ref perc);
			li.SubItems.Add(s2);
			var s3 = string.Format("{0:###.##}%", perc);
			li.SubItems.Add(s3);
			mList.Items.Add(li);
		}

		ColorAndFontRows(mList);
		ResizeColumns();
	}

	private string getNakLord(Longitude l)
	{
		INakshatraDasa id = null;
		switch (options.NakshatraLord)
		{
			default:
			case ENakshatraLord.Vimsottari:
				id = new VimsottariDasa(h);
				break;
			case ENakshatraLord.Ashtottari:
				id = new AshtottariDasa(h);
				break;
			case ENakshatraLord.Yogini:
				id = new YoginiDasa(h);
				break;
			case ENakshatraLord.Shodashottari:
				id = new ShodashottariDasa(h);
				break;
			case ENakshatraLord.Dwadashottari:
				id = new DwadashottariDasa(h);
				break;
			case ENakshatraLord.Panchottari:
				id = new PanchottariDasa(h);
				break;
			case ENakshatraLord.Shatabdika:
				id = new ShatabdikaDasa(h);
				break;
			case ENakshatraLord.ChaturashitiSama:
				id = new ChaturashitiSamaDasa(h);
				break;
			case ENakshatraLord.DwisaptatiSama:
				id = new DwisaptatiSamaDasa(h);
				break;
			case ENakshatraLord.ShatTrimshaSama:
				id = new ShatTrimshaSamaDasa(h);
				break;
		}

		var b = id.lordOfNakshatra(l.toNakshatra());
		return b.ToString();
	}

	private void RepopulateHouseCusps()
	{
		mList.Clear();
		mList.Columns.Add("System", -1, HorizontalAlignment.Left);
		mList.Columns.Add("House", -1, HorizontalAlignment.Left);
		mList.Columns.Add("Start", -1, HorizontalAlignment.Left);
		mList.Columns.Add("Stop", -1, HorizontalAlignment.Left);

		for (var i = 0; i < 12; i++)
		{
			var li = new ListViewItem();
			li.Text = string.Format("{0}", (char) h.swephHouseSystem);
			li.SubItems.Add(h.swephHouseCusps[i].value.ToString());
			li.SubItems.Add(h.swephHouseCusps[i + 1].value.ToString());
			mList.Items.Add(li);
		}

		ColorAndFontRows(mList);
		ResizeColumns();
	}

	private string longitudeToString(Longitude lon)
	{
		var rasi    = lon.toZodiacHouse().value.ToString();
		var offset  = lon.toZodiacHouseOffset();
		var minutes = Math.Floor(offset);
		offset = (offset - minutes) * 60.0;
		var seconds = Math.Floor(offset);
		offset = (offset - seconds) * 60.0;
		var subsecs = Math.Floor(offset);
		offset = (offset - subsecs) * 60.0;
		var subsubsecs = Math.Floor(offset);

		return string.Format("{0:00} {1} {2:00}:{3:00}:{4:00}", minutes, rasi, seconds, subsecs, subsubsecs);
	}

	private void RepopulateBhavaCusps()
	{
		mList.Clear();
		mList.Columns.Add("Cusp Start", -1, HorizontalAlignment.Left);
		mList.Columns.Add("Cusp End", -1, HorizontalAlignment.Left);
		mList.Columns.Add("Rasi", -2, HorizontalAlignment.Left);

		var lpos = h.getPosition(Elements.Body.Name.Lagna).longitude.add(0);
		var bp   = new Position(h, Elements.Body.Name.Lagna, Elements.Body.Type.Lagna, lpos, 0, 0, 0, 0, 0);
		for (var i = 0; i < 12; i++)
		{
			var dp = bp.toDivisionPosition(options.DivisionType);
			var li = new ListViewItem();
			li.Text = longitudeToString(new Longitude(dp.cusp_lower));
			li.SubItems.Add(longitudeToString(new Longitude(dp.cusp_higher)));
			li.SubItems.Add(dp.zodiac_house.value.ToString());
			bp.longitude = new Longitude(dp.cusp_higher + 1);
			mList.Items.Add(li);
		}

		ColorAndFontRows(mList);
		ResizeColumns();
	}

	private string AmsaRuler(Position bp, DivisionPosition dp)
	{
		if (dp.ruler_index == 0)
		{
			return string.Empty;
		}

		var ri = dp.ruler_index - 1;

		if (options.DivisionType.MultipleDivisions.Length == 1)
		{
			switch (options.DivisionType.MultipleDivisions[0].Varga)
			{
				case Basics.DivisionType.HoraParasara:     return Tables.Varga.Rulers.Hora[ri];
				case Basics.DivisionType.DrekkanaParasara: return Tables.Varga.Rulers.Drekkana[ri];
				case Basics.DivisionType.Chaturthamsa:     return Tables.Varga.Rulers.Chaturthamsa[ri];
				case Basics.DivisionType.Saptamsa:         return Tables.Varga.Rulers.Saptamsa[ri];
				case Basics.DivisionType.Navamsa:          return Tables.Varga.Rulers.Navamsa[ri];
				case Basics.DivisionType.Dasamsa:          return Tables.Varga.Rulers.Dasamsa[ri];
				case Basics.DivisionType.Dwadasamsa:       return Tables.Varga.Rulers.Dwadasamsa[ri];
				case Basics.DivisionType.Shodasamsa:       return Tables.Varga.Rulers.Shodasamsa[ri];
				case Basics.DivisionType.Vimsamsa:         return Tables.Varga.Rulers.Vimsamsa[ri];
				case Basics.DivisionType.Chaturvimsamsa:   return Tables.Varga.Rulers.Chaturvimsamsa[ri];
				case Basics.DivisionType.Nakshatramsa:     return Tables.Varga.Rulers.Nakshatramsa[ri];
				case Basics.DivisionType.Trimsamsa:        return Tables.Varga.Rulers.Trimsamsa[ri];
				case Basics.DivisionType.Khavedamsa:       return Tables.Varga.Rulers.Khavedamsa[ri];
				case Basics.DivisionType.Akshavedamsa:     return Tables.Varga.Rulers.Akshavedamsa[ri];
				case Basics.DivisionType.Shashtyamsa:      return Tables.Varga.Rulers.Shashtyamsa[ri];
				case Basics.DivisionType.Nadiamsa:         return Tables.Varga.Rulers.NadiamsaCKN[ri];
				case Basics.DivisionType.NadiamsaCKN:      return Tables.Varga.Rulers.NadiamsaCKN[ri];
			}
		}

		return string.Empty;
	}

	private string GetBodyString(Position bp)
	{
		var dir = bp.speed_longitude >= 0.0 ? string.Empty : " (R)";

		if (bp.name == Elements.Body.Name.Other || bp.name == Elements.Body.Name.MrityuPoint)
		{
			return bp.otherString + dir;
		}

		return bp.name.ToString();
	}

	private bool CheckBodyForCurrentView(Position bp)
	{
		switch (vt)
		{
			case ViewType.ViewMrityuLongitudes:
				if (bp.name == Elements.Body.Name.MrityuPoint)
				{
					return true;
				}

				return false;
			case ViewType.ViewOtherLongitudes:
				if (bp.name == Elements.Body.Name.Other && bp.type != Elements.Body.Type.Sahama)
				{
					return true;
				}

				return false;
			case ViewType.ViewSahamaLongitudes:
				if (bp.type == Elements.Body.Type.Sahama)
				{
					return true;
				}

				return false;
			case ViewType.ViewBasicGrahas:
				if (bp.name == Elements.Body.Name.MrityuPoint || bp.name == Elements.Body.Name.Other)
				{
					return false;
				}

				return true;
		}

		return true;
	}

	private void RepopulateBasicGrahas()
	{
		mList.Columns.Clear();
		mList.Items.Clear();

		var al = new ArrayList();
		for (var i = (int) Elements.Body.Name.Sun; i <= (int) Elements.Body.Name.Rahu; i++)
		{
			var b   = (Body.Name) i;
			var bp  = h.getPosition(b);
			var bkc = new KarakaComparer(bp);
			al.Add(bkc);
		}

		al.Sort();
		var karaka_indices = new int[9];
		for (var i = 0; i < al.Count; i++)
		{
			var bk = (KarakaComparer) al[i];
			karaka_indices[(int) bk.GetPosition.name] = i;
		}


		mList.Columns.Add("Body", -1, HorizontalAlignment.Left);
		mList.Columns.Add("Longitude", -1, HorizontalAlignment.Left);
		mList.Columns.Add("Nakshatra", -1, HorizontalAlignment.Left);
		mList.Columns.Add("Pada", -1, HorizontalAlignment.Left);
		mList.Columns.Add("NakLord", -1, HorizontalAlignment.Left);
		mList.Columns.Add(options.DivisionType.ToString(), 100, HorizontalAlignment.Left);
		mList.Columns.Add("Part", -1, HorizontalAlignment.Left);
		mList.Columns.Add("Ruler", -1, HorizontalAlignment.Left);
		mList.Columns.Add("Cusp Start", -1, HorizontalAlignment.Left);
		mList.Columns.Add("Cusp End", -2, HorizontalAlignment.Left);


		foreach (Position bp in h.positionList)
		{
			if (false == CheckBodyForCurrentView(bp))
			{
				continue;
			}

			var li = new ListViewItem();
			li.Text = GetBodyString(bp);

			if ((int) bp.name >= (int) Elements.Body.Name.Sun && (int) bp.name <= (int) Elements.Body.Name.Rahu)
			{
				li.Text = string.Format("{0}   {1}", li.Text, Karaka.karakas_s[karaka_indices[(int) bp.name]]);
			}

			li.SubItems.Add(longitudeToString(bp.longitude));
			li.SubItems.Add(bp.longitude.toNakshatra().value.ToString());
			li.SubItems.Add(bp.longitude.toNakshatraPada().ToString());
			li.SubItems.Add(getNakLord(bp.longitude));

			var dp = bp.toDivisionPosition(options.DivisionType);
			li.SubItems.Add(dp.zodiac_house.value.ToString());
			li.SubItems.Add(dp.part.ToString());
			li.SubItems.Add(AmsaRuler(bp, dp));
			li.SubItems.Add(longitudeToString(new Longitude(dp.cusp_lower)));
			li.SubItems.Add(longitudeToString(new Longitude(dp.cusp_higher)));

			mList.Items.Add(li);
		}

		ColorAndFontRows(mList);
		ResizeColumns();
	}

	private void Repopulate()
	{
		mList.BeginUpdate();
		switch (vt)
		{
			case ViewType.ViewBasicGrahas:
				RepopulateBasicGrahas();
				break;
			case ViewType.ViewOtherLongitudes:
				RepopulateBasicGrahas();
				break;
			case ViewType.ViewMrityuLongitudes:
				RepopulateBasicGrahas();
				break;
			case ViewType.ViewSahamaLongitudes:
				RepopulateBasicGrahas();
				break;
			case ViewType.ViewSpecialTithis:
				RepopulateSpecialTithis();
				break;
			case ViewType.ViewSpecialTaras:
				RepopulateSpecialTaras();
				break;
			case ViewType.ViewBhavaCusps:
				RepopulateBhavaCusps();
				break;
			case ViewType.ViewAstronomicalInfo:
				RepopulateAstronomicalInfo();
				break;
			case ViewType.ViewNakshatraAspects:
				RepopulateNakshatraAspects();
				break;
			case ViewType.ViewCharaKarakas:
				RepopulateCharaKarakas();
				break;
			case ViewType.ViewCharaKarakas7:
				RepopulateCharaKarakas();
				break;
			case ViewType.ViewAvasthas:
				RepopulateAvasthas();
				break;
			case ViewType.View64Navamsa:
				Repopulate64Navamsa();
				break;
			case ViewType.ViewNonLonBodies:
				RepopulateNonLonBodies();
				break;
		}

		mList.EndUpdate();
	}

	private void OnRecalculate(object o)
	{
		Repopulate();
	}

	private void OnRedisplay(object o)
	{
		ColorAndFontRows(mList);
	}

	protected override void copyToClipboard()
	{
	}

	private void mList_SelectedIndexChanged(object sender, EventArgs e)
	{
	}

	private void menuItem1_Click(object sender, EventArgs e)
	{
		//mChangeView_Click (sender, e);
	}

	private void mList_MouseHover(object sender, EventArgs e)
	{
	}

	private void ResetMenuItems()
	{
		foreach (MenuItem mi in ContextMenu.MenuItems)
		{
			mi.Checked = false;
		}

		menuChangeVarga.Enabled = false;
		menuCopyLon.Enabled     = false;
	}

	private void menuBasicGrahas_Click(object sender, EventArgs e)
	{
		ResetMenuItems();
		menuChangeVarga.Enabled = true;
		menuCopyLon.Enabled     = true;
		menuBasicGrahas.Checked = true;
		vt                      = ViewType.ViewBasicGrahas;
		Repopulate();
	}

	private void menuSpecialTithis_Click(object sender, EventArgs e)
	{
		ResetMenuItems();
		menuSpecialTithis.Checked = true;
		vt                        = ViewType.ViewSpecialTithis;
		Repopulate();
	}

	private void menuSpecialTaras_Click(object sender, EventArgs e)
	{
		ResetMenuItems();
		menuSpecialTaras.Checked = true;
		vt                       = ViewType.ViewSpecialTaras;
		Repopulate();
	}


	public object SetOptions(object o)
	{
		options.Copy(o);
		Repopulate();
		return options.Clone();
	}

	private void menuChangeVarga_Click(object sender, EventArgs e)
	{
		var opts = new MhoraOptions(options.Clone(), SetOptions);
		opts.ShowDialog();
	}

	private void menuBhavaCusps_Click(object sender, EventArgs e)
	{
		ResetMenuItems();
		menuBhavaCusps.Checked  = true;
		menuChangeVarga.Enabled = true;

		vt = ViewType.ViewBhavaCusps;
		Repopulate();
	}

	private void menuOtherLongitudes_Click(object sender, EventArgs e)
	{
		ResetMenuItems();
		menuChangeVarga.Enabled     = true;
		menuOtherLongitudes.Checked = true;
		menuCopyLon.Enabled         = true;
		vt                          = ViewType.ViewOtherLongitudes;
		Repopulate();
	}

	private void menuMrityuLongitudes_Click(object sender, EventArgs e)
	{
		ResetMenuItems();
		menuChangeVarga.Enabled      = true;
		menuCopyLon.Enabled          = true;
		menuMrityuLongitudes.Checked = true;
		vt                           = ViewType.ViewMrityuLongitudes;
		Repopulate();
	}

	private void menuAstroInfo_Click(object sender, EventArgs e)
	{
		ResetMenuItems();
		menuAstroInfo.Checked = true;
		vt                    = ViewType.ViewAstronomicalInfo;
		Repopulate();
	}

	private void menuNakshatraAspects_Click(object sender, EventArgs e)
	{
		ResetMenuItems();
		menuNakshatraAspects.Checked = true;
		vt                           = ViewType.ViewNakshatraAspects;
		Repopulate();
	}

	private void menuCharaKarakas_Click(object sender, EventArgs e)
	{
		ResetMenuItems();
		menuCharaKarakas.Checked = true;
		vt                       = ViewType.ViewCharaKarakas;
		Repopulate();
	}

	private void menuSahamaLongitudes_Click(object sender, EventArgs e)
	{
		ResetMenuItems();
		menuChangeVarga.Enabled      = true;
		menuCopyLon.Enabled          = true;
		menuSahamaLongitudes.Checked = true;
		vt                           = ViewType.ViewSahamaLongitudes;
		Repopulate();
	}

	private void menuAvasthas_Click(object sender, EventArgs e)
	{
		ResetMenuItems();
		menuChangeVarga.Enabled = true;
		menuAvasthas.Checked    = true;
		vt                      = ViewType.ViewAvasthas;
		Repopulate();
	}

	private void calculationsContextMenu_Popup(object sender, EventArgs e)
	{
	}

	private void menuCharaKarakas7_Click(object sender, EventArgs e)
	{
		ResetMenuItems();
		menuCharaKarakas7.Checked = true;
		vt                        = ViewType.ViewCharaKarakas7;
		Repopulate();
	}

	private void menu64Navamsa_Click(object sender, EventArgs e)
	{
		ResetMenuItems();
		menu64Navamsa.Checked = true;
		vt                    = ViewType.View64Navamsa;
		Repopulate();
	}

	private void menuCopyLon_Click(object sender, EventArgs e)
	{
		if (mList.SelectedItems.Count <= 0)
		{
			return;
		}

		var li = mList.SelectedItems[0];
		Clipboard.SetDataObject(li.SubItems[1].Text, false);
	}

	private void menuNonLonBodies_Click(object sender, EventArgs e)
	{
		ResetMenuItems();
		menuChangeVarga.Enabled  = true;
		menuNonLonBodies.Checked = true;
		vt                       = ViewType.ViewNonLonBodies;
		Repopulate();
	}

	private void mList_DragEnter(object sender, DragEventArgs e)
	{
		if (e.Data.GetDataPresent(typeof(DivisionalChart)))
		{
			e.Effect = DragDropEffects.Copy;
		}
		else
		{
			e.Effect = DragDropEffects.None;
		}
	}

	private void mList_DragDrop(object sender, DragEventArgs e)
	{
		if (e.Data.GetDataPresent(typeof(DivisionalChart)))
		{
			var div = Division.CopyFromClipboard();
			if (null == div)
			{
				return;
			}

			options.DivisionType = div;
			OnRecalculate(options);
		}
	}

	private enum ENakshatraLord
	{
		Vimsottari,
		Ashtottari,
		Yogini,
		Shodashottari,
		Dwadashottari,
		Panchottari,
		Shatabdika,
		ChaturashitiSama,
		DwisaptatiSama,
		ShatTrimshaSama
	}

	private class UserOptions : ICloneable
	{
		[PGNotVisible]
		public Division DivisionType
		{
			get;
			set;
		}

		[PGDisplayName("Division Type")]
		public Basics.DivisionType UIDivisionType
		{
			get => DivisionType.MultipleDivisions[0].Varga;
			set => DivisionType = new Division(value);
		}

		public ENakshatraLord NakshatraLord
		{
			get;
			set;
		}

		public object Clone()
		{
			var uo = new UserOptions();
			uo.DivisionType  = DivisionType;
			uo.NakshatraLord = NakshatraLord;
			return uo;
		}

		public object Copy(object o)
		{
			var uo = (UserOptions) o;
			DivisionType  = uo.DivisionType;
			NakshatraLord = uo.NakshatraLord;
			return Clone();
		}
	}

	private enum ViewType
	{
		ViewBasicGrahas,
		ViewOtherLongitudes,
		ViewMrityuLongitudes,
		ViewSahamaLongitudes,
		ViewAvasthas,
		ViewSpecialTithis,
		ViewSpecialTaras,
		ViewBhavaCusps,
		ViewAstronomicalInfo,
		ViewNakshatraAspects,
		ViewCharaKarakas,
		ViewCharaKarakas7,
		View64Navamsa,
		ViewNonLonBodies
	}
}