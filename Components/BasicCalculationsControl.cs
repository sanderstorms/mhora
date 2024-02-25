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
using Mhora.Components.Controls;
using Mhora.Components.Delegates;
using Mhora.Components.Property;
using Mhora.Components.Varga;
using Mhora.Database.Settings;
using Mhora.Definitions;
using Mhora.Elements;
using Mhora.Elements.Dasas;
using Mhora.Elements.Dasas.NakshatraDasa;
using Mhora.Tables;
using Mhora.Util;

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


	private ColumnHeader colBody;
	private ContextMenu  calculationsContextMenu;

	private ColumnHeader Longitude;
	private MenuItem     menu64Navamsa;
	private MenuItem     menuAstroInfo;
	private MenuItem     menuAvasthas;
	private MenuItem     menuBasicGrahas;
	private MenuItem     menuBhavaCusps;


	private MenuItem             menuChangeVarga;
	private MenuItem             menuCharaKarakas;
	private MenuItem             menuCharaKarakas7;
	private MenuItem             menuCopyLon;
	private MenuItem             menuItem1;
	private MenuItem             menuItem2;
	private MenuItem             menuMrityuLongitudes;
	private MenuItem             menuNakshatraAspects;
	private MenuItem             menuNonLonBodies;
	private MenuItem             menuOtherLongitudes;
	private MenuItem             menuSahamaLongitudes;
	private MenuItem             menuSpecialTaras;
	private MenuItem             menuSpecialTithis;
	private ListView             mList;
	private ColumnHeader         Nakshatra;
	private ColumnHeader         Pada;
	private ListViewColumnSorter lvwColumnSorter;

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
		options                                =  new UserOptions
		{
			DivisionType = new Division(DivisionType.Rasi)
		};
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
			this.colBody = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.Longitude = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.Nakshatra = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.Pada = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.calculationsContextMenu = new System.Windows.Forms.ContextMenu();
			this.menuChangeVarga = new System.Windows.Forms.MenuItem();
			this.menuCopyLon = new System.Windows.Forms.MenuItem();
			this.menuBasicGrahas = new System.Windows.Forms.MenuItem();
			this.menuOtherLongitudes = new System.Windows.Forms.MenuItem();
			this.menuMrityuLongitudes = new System.Windows.Forms.MenuItem();
			this.menuSahamaLongitudes = new System.Windows.Forms.MenuItem();
			this.menuNonLonBodies = new System.Windows.Forms.MenuItem();
			this.menuCharaKarakas = new System.Windows.Forms.MenuItem();
			this.menuCharaKarakas7 = new System.Windows.Forms.MenuItem();
			this.menu64Navamsa = new System.Windows.Forms.MenuItem();
			this.menuAstroInfo = new System.Windows.Forms.MenuItem();
			this.menuSpecialTithis = new System.Windows.Forms.MenuItem();
			this.menuSpecialTaras = new System.Windows.Forms.MenuItem();
			this.menuNakshatraAspects = new System.Windows.Forms.MenuItem();
			this.menuBhavaCusps = new System.Windows.Forms.MenuItem();
			this.menuAvasthas = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// mList
			// 
			this.mList.AllowDrop = true;
			this.mList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colBody,
            this.Longitude,
            this.Nakshatra,
            this.Pada});
			this.mList.ContextMenu = this.calculationsContextMenu;
			this.mList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.mList.ForeColor = System.Drawing.SystemColors.HotTrack;
			this.mList.FullRowSelect = true;
			this.mList.HideSelection = false;
			this.mList.Location = new System.Drawing.Point(0, 0);
			this.mList.Name = "mList";
			this.mList.Size = new System.Drawing.Size(496, 176);
			this.mList.TabIndex = 0;
			this.mList.UseCompatibleStateImageBehavior = false;
			this.mList.View = System.Windows.Forms.View.Details;
			this.mList.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.OnColumnclick);
			this.mList.SelectedIndexChanged += new System.EventHandler(this.mList_SelectedIndexChanged);
			this.mList.DragDrop += new System.Windows.Forms.DragEventHandler(this.mList_DragDrop);
			this.mList.DragEnter += new System.Windows.Forms.DragEventHandler(this.mList_DragEnter);
			this.mList.MouseHover += new System.EventHandler(this.mList_MouseHover);
			//
			// Create an instance of a ListView column sorter and assign it
			// to the ListView control.
			this.lvwColumnSorter          = new ListViewColumnSorter();
			this.mList.ListViewItemSorter = lvwColumnSorter;
			// 
			// Body
			// 
			this.colBody.Text = "Body";
			this.colBody.Width = 100;
			// 
			// Longitude
			// 
			this.Longitude.Text = "Longitude";
			this.Longitude.Width = 120;
			// 
			// Nakshatra
			// 
			this.Nakshatra.Text = "Nakshatra";
			this.Nakshatra.Width = 120;
			// 
			// Pada
			// 
			this.Pada.Text = "Pada";
			this.Pada.Width = 50;
			// 
			// calculationsContextMenu
			// 
			this.calculationsContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
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
            this.menuItem2});
			this.calculationsContextMenu.Popup += new System.EventHandler(this.calculationsContextMenu_Popup);
			// 
			// menuChangeVarga
			// 
			this.menuChangeVarga.Index = 0;
			this.menuChangeVarga.Text = "Options";
			this.menuChangeVarga.Click += new System.EventHandler(this.menuChangeVarga_Click);
			// 
			// menuCopyLon
			// 
			this.menuCopyLon.Index = 1;
			this.menuCopyLon.Text = "Copy Longitude";
			this.menuCopyLon.Click += new System.EventHandler(this.menuCopyLon_Click);
			// 
			// menuBasicGrahas
			// 
			this.menuBasicGrahas.Index = 2;
			this.menuBasicGrahas.Text = "Basic Longitudes";
			this.menuBasicGrahas.Click += new System.EventHandler(this.menuBasicGrahas_Click);
			// 
			// menuOtherLongitudes
			// 
			this.menuOtherLongitudes.Index = 3;
			this.menuOtherLongitudes.Text = "Other Longitudes";
			this.menuOtherLongitudes.Click += new System.EventHandler(this.menuOtherLongitudes_Click);
			// 
			// menuMrityuLongitudes
			// 
			this.menuMrityuLongitudes.Index = 4;
			this.menuMrityuLongitudes.Text = "Mrityu Longitudes";
			this.menuMrityuLongitudes.Click += new System.EventHandler(this.menuMrityuLongitudes_Click);
			// 
			// menuSahamaLongitudes
			// 
			this.menuSahamaLongitudes.Index = 5;
			this.menuSahamaLongitudes.Text = "Sahama Longitudes";
			this.menuSahamaLongitudes.Click += new System.EventHandler(this.menuSahamaLongitudes_Click);
			// 
			// menuNonLonBodies
			// 
			this.menuNonLonBodies.Index = 6;
			this.menuNonLonBodies.Text = "Non-Longitude Bodies";
			this.menuNonLonBodies.Click += new System.EventHandler(this.menuNonLonBodies_Click);
			// 
			// menuCharaKarakas
			// 
			this.menuCharaKarakas.Index = 7;
			this.menuCharaKarakas.Text = "Chara Karakas (8)";
			this.menuCharaKarakas.Click += new System.EventHandler(this.menuCharaKarakas_Click);
			// 
			// menuCharaKarakas7
			// 
			this.menuCharaKarakas7.Index = 8;
			this.menuCharaKarakas7.Text = "Chara Karakas (7)";
			this.menuCharaKarakas7.Click += new System.EventHandler(this.menuCharaKarakas7_Click);
			// 
			// menu64Navamsa
			// 
			this.menu64Navamsa.Index = 9;
			this.menu64Navamsa.Text = "64th Navamsa";
			this.menu64Navamsa.Click += new System.EventHandler(this.menu64Navamsa_Click);
			// 
			// menuAstroInfo
			// 
			this.menuAstroInfo.Index = 10;
			this.menuAstroInfo.Text = "Astronomical Info";
			this.menuAstroInfo.Click += new System.EventHandler(this.menuAstroInfo_Click);
			// 
			// menuSpecialTithis
			// 
			this.menuSpecialTithis.Index = 11;
			this.menuSpecialTithis.Text = "Special Tithis";
			this.menuSpecialTithis.Click += new System.EventHandler(this.menuSpecialTithis_Click);
			// 
			// menuSpecialTaras
			// 
			this.menuSpecialTaras.Index = 12;
			this.menuSpecialTaras.Text = "Special Nakshatras";
			this.menuSpecialTaras.Click += new System.EventHandler(this.menuSpecialTaras_Click);
			// 
			// menuNakshatraAspects
			// 
			this.menuNakshatraAspects.Index = 13;
			this.menuNakshatraAspects.Text = "Nakshatra Aspects";
			this.menuNakshatraAspects.Click += new System.EventHandler(this.menuNakshatraAspects_Click);
			// 
			// menuBhavaCusps
			// 
			this.menuBhavaCusps.Index = 14;
			this.menuBhavaCusps.Text = "Bhava Cusps";
			this.menuBhavaCusps.Click += new System.EventHandler(this.menuBhavaCusps_Click);
			// 
			// menuAvasthas
			// 
			this.menuAvasthas.Index = 15;
			this.menuAvasthas.Text = "Avasthas";
			this.menuAvasthas.Click += new System.EventHandler(this.menuAvasthas_Click);
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 16;
			this.menuItem1.Text = "-";
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 17;
			this.menuItem2.Text = "-";
			// 
			// BasicCalculationsControl
			// 
			this.ContextMenu = this.calculationsContextMenu;
			this.Controls.Add(this.mList);
			this.Name = "BasicCalculationsControl";
			this.Size = new System.Drawing.Size(496, 176);
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

		return str + " " + Tithis.Name[t - 1];
	}

	private void RepopulateSpecialTaras()
	{
		mList.Columns.Clear();
		mList.Items.Clear();
		mList.Columns.Add("Type", -1, HorizontalAlignment.Left);
		mList.Columns.Add("Nakshatra (27)", -1, HorizontalAlignment.Left);
		mList.Columns.Add("Nakshatra (28)", -1, HorizontalAlignment.Left);

		var nmoon   = h.GetPosition(Body.Moon).Longitude.ToNakshatra();
		var nmoon28 = h.GetPosition(Body.Moon).Longitude.ToNakshatra28();
		for (var i = 0; i < Taras.SpecialIndices.Length; i++)
		{
			var sn   = nmoon.Add(Taras.SpecialIndices[i]);
			var sn28 = nmoon28.Add(Taras.SpecialIndices[i]);

			var li = new ListViewItem
			{
				Text = string.Format("{0:00}  {1} Tara", Taras.SpecialIndices[i], Taras.SpecialNames[i])
			};
			li.SubItems.Add(sn.ToString());
			li.SubItems.Add(sn28.ToString());
			mList.Items.Add(li);
		}

		ColorAndFontRows(mList);
		ResizeColumns();
	}

	private void RepopulateCharaKarakas()
	{
		mList.Clear();
		mList.Columns.Add("Body", -1, HorizontalAlignment.Left);
		mList.Columns.Add("Karakas", -1, HorizontalAlignment.Left);
		mList.Columns.Add("Offset", -2, HorizontalAlignment.Left);

		var al  = new ArrayList();
		var max = 0;
		if (vt == ViewType.ViewCharaKarakas)
		{
			max = (int)Body.Rahu;
		}
		else
		{
			max = (int)Body.Saturn;
		}


		for (var i = (int)Body.Sun; i <= max; i++)
		{
			var b   = (Body) i;
			var bp  = h.GetPosition(b);
			var bkc = new KarakaComparer(bp);
			al.Add(bkc);
		}

		al.Sort();

		for (var i = 0; i < al.Count; i++)
		{
			var li = new ListViewItem();
			var bk = (KarakaComparer) al[i];
			li.Text = bk.GetPosition.Name.Name();
			if (vt == ViewType.ViewCharaKarakas)
			{
				li.SubItems.Add(Bodies.Karakas[i]);
			}
			else
			{
				li.SubItems.Add(Bodies.Karakas7[i]);
			}

			li.SubItems.Add(string.Format("{0:0.00}", bk.GetOffset()));
			mList.Items.Add(li);
		}


		ColorAndFontRows(mList);
		ResizeColumns();
	}


	private void Repopulate64NavamsaHelper(Body b, string name, Position bp, Division div)
	{
		var dp = bp.ToDivisionPosition(div);
		var li = new ListViewItem
		{
			Text = b.ToString()
		};
		li.SubItems.Add(name);
		li.SubItems.Add(dp.ZodiacHouse.ToString());
		li.SubItems.Add(h.LordOfZodiacHouse(dp.ZodiacHouse, div).Name());
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
			if (dp.Body == Body.Other)
			{
				desc = dp.Description;
			}
			else
			{
				desc = dp.Body.ToString();
			}

			var li = new ListViewItem(desc);
			li.SubItems.Add(dp.ZodiacHouse.ToString());
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

        Body[] bodyReferences =
		{
            Body.Lagna,
            Body.Moon,
            Body.Sun
		};

		foreach (var b in bodyReferences)
		{
			var bp    = h.GetPosition(b).Clone();
			var bpLon = bp.Longitude;

			bp.Longitude = bpLon.Add(30.0 / 9.0 * (64 - 1));
			Repopulate64NavamsaHelper(b, "64th Navamsa", bp, new Division(DivisionType.Navamsa));

			bp.Longitude = bpLon.Add(30.0 / 3.0 * (22 - 1));
			Repopulate64NavamsaHelper(b, "22nd Drekkana", bp, new Division(DivisionType.DrekkanaParasara));
			Repopulate64NavamsaHelper(b, "22nd Drekkana (Parivritti)", bp, new Division(DivisionType.DrekkanaParivrittitraya));
			Repopulate64NavamsaHelper(b, "22nd Drekkana (Somnath)", bp, new Division(DivisionType.DrekkanaSomnath));
			Repopulate64NavamsaHelper(b, "22nd Drekkana (Jagannath)", bp, new Division(DivisionType.DrekkanaJagannath));
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

		for (var i = (int)Body.Sun; i <= (int)Body.Ketu; i++)
		{
			var b  = (Body) i;
			var li = new ListViewItem
			{
				Text = b.Name()
			};
			var dp            = h.GetPosition(b).ToDivisionPosition(new Division(DivisionType.Panchamsa));
			var avastha_index = -1;
			switch (dp.ZodiacHouse.Index() % 2)
			{
				case 1:
					avastha_index = dp.Part;
					break;
				case 0:
					avastha_index = 6 - dp.Part;
					break;
			}

			li.SubItems.Add(Vargas.Avasthas[avastha_index - 1]);
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

		for (var i = (int)Body.Sun; i <= (int)Body.Rahu; i++)
		{
			var  b          = (Body) i;
			bool dirForward = (i % 2) == 0;

			var       bp = h.GetPosition(b);
			var       n  = bp.Longitude.ToNakshatra();
			Nakshatra l;

			if (dirForward)
			{
				l = n.Add(Bodies.LattaAspects[i]);
			}
			else
			{
				l = n.AddReverse(Bodies.LattaAspects[i]);
			}

			var nak_fmt = string.Empty;
			foreach (var j in Nakshatras.TaraAspects[i])
			{
				var na = n.Add(j);
				if (nak_fmt.Length > 0)
				{
					nak_fmt = string.Format("{0}, {1}-{2}", nak_fmt, j, na.Name());
				}
				else
				{
					nak_fmt = string.Format("{0}-{1}", j, na.Name());
				}
			}

			var li  = new ListViewItem(b.Name());
			var fmt = string.Format("{0:00}-{1}", Bodies.LattaAspects[i], l);
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

		for (var i = (int)Body.Sun; i <= (int)Body.Saturn; i++)
		{
			var b  = (Body) i;
			var bp = h.GetPosition(b);
			var li = new ListViewItem(b.Name());
			li.SubItems.Add(bp.Longitude.Value.ToString());
			li.SubItems.Add(bp.SpeedLongitude.ToString());
			li.SubItems.Add(bp.Latitude.ToString());
			li.SubItems.Add(bp.SpeedLatitude.ToString());
			li.SubItems.Add(bp.Distance.ToString());
			li.SubItems.Add(bp.SpeedDistance.ToString());
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
		mList.Columns.Add("Tithis", -1, HorizontalAlignment.Left);
		mList.Columns.Add("% Left", -1, HorizontalAlignment.Left);

		var spos      = h.GetPosition(Body.Sun).Longitude;
		var mpos      = h.GetPosition(Body.Moon).Longitude;
		var baseTithi = mpos.Sub(spos).Value;
		for (var i = 1; i <= 12; i++)
		{
			var    spTithiVal = new Longitude(baseTithi * i).Value;
			double tithi      = 0;
			double perc       = 0;
			var    li         = new ListViewItem();
			var    s1         = string.Format("{0:00}  {1} Tithis", i, Tithis.SpecialNames[i]);
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
			case Dasas.NakshatraLord.Vimsottari:
				id = new VimsottariDasa(h);
				break;
			case Dasas.NakshatraLord.Ashtottari:
				id = new AshtottariDasa(h);
				break;
			case Dasas.NakshatraLord.Yogini:
				id = new YoginiDasa(h);
				break;
			case Dasas.NakshatraLord.Shodashottari:
				id = new ShodashottariDasa(h);
				break;
			case Dasas.NakshatraLord.Dwadashottari:
				id = new DwadashottariDasa(h);
				break;
			case Dasas.NakshatraLord.Panchottari:
				id = new PanchottariDasa(h);
				break;
			case Dasas.NakshatraLord.Shatabdika:
				id = new ShatabdikaDasa(h);
				break;
			case Dasas.NakshatraLord.ChaturashitiSama:
				id = new ChaturashitiSamaDasa(h);
				break;
			case Dasas.NakshatraLord.DwisaptatiSama:
				id = new DwisaptatiSamaDasa(h);
				break;
			case Dasas.NakshatraLord.ShatTrimshaSama:
				id = new ShatTrimshaSamaDasa(h);
				break;
		}

		var b = id.LordOfNakshatra(l.ToNakshatra());
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
			var li = new ListViewItem
			{
				Text = string.Format("{0}", (char) h.SwephHouseSystem)
			};
			li.SubItems.Add(h.SwephHouseCusps[i].Value.ToString());
			li.SubItems.Add(h.SwephHouseCusps[i + 1].Value.ToString());
			mList.Items.Add(li);
		}

		ColorAndFontRows(mList);
		ResizeColumns();
	}

	private string longitudeToString(Longitude lon)
	{
		var rasi    = lon.ToZodiacHouse().ToString();
		var offset  = lon.ToZodiacHouseOffset();
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

		var lpos = h.GetPosition(Body.Lagna).Longitude;
		var bp   = new Position(h, Body.Lagna, BodyType.Lagna, lpos, 0, 0, 0, 0, 0);
		for (var i = 0; i < 12; i++)
		{
			var dp = bp.ToDivisionPosition(options.DivisionType);
			var li = new ListViewItem
			{
				Text = longitudeToString(new Longitude(dp.CuspLower))
			};
			li.SubItems.Add(longitudeToString(new Longitude(dp.CuspHigher)));
			li.SubItems.Add(dp.ZodiacHouse.ToString());
			bp.Longitude = new Longitude(dp.CuspHigher + 1);
			mList.Items.Add(li);
		}

		ColorAndFontRows(mList);
		ResizeColumns();
	}

	private string GetBodyString(Position bp)
	{
		var dir = bp.SpeedLongitude >= 0.0 ? string.Empty : " (R)";

		if (bp.Name == Body.Other || bp.Name == Body.MrityuPoint)
		{
			return bp.OtherString + dir;
		}

		return bp.Name.ToString();
	}

	private bool CheckBodyForCurrentView(Position bp)
	{
		switch (vt)
		{
			case ViewType.ViewMrityuLongitudes:
				if (bp.Name == Body.MrityuPoint)
				{
					return true;
				}

				return false;
			case ViewType.ViewOtherLongitudes:
				if (bp.Name == Body.Other && bp.BodyType != BodyType.Sahama)
				{
					return true;
				}

				return false;
			case ViewType.ViewSahamaLongitudes:
				if (bp.BodyType == BodyType.Sahama)
				{
					return true;
				}

				return false;
			case ViewType.ViewBasicGrahas:
				if (bp.Name == Body.MrityuPoint || bp.Name == Body.Other)
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
		for (var i = (int)Body.Sun; i <= (int)Body.Rahu; i++)
		{
			var b   = (Body) i;
			var bp  = h.GetPosition(b);
			var bkc = new KarakaComparer(bp);
			al.Add(bkc);
		}

		al.Sort();
		var karaka_indices = new int[9];
		for (var i = 0; i < al.Count; i++)
		{
			var bk = (KarakaComparer) al[i];
			karaka_indices[(int) bk.GetPosition.Name] = i;
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


		foreach (Position bp in h.PositionList)
		{
			if (false == CheckBodyForCurrentView(bp))
			{
				continue;
			}

			var li = new ListViewItem
			{
				Text = GetBodyString(bp)
			};

			if ((int) bp.Name >= (int)Body.Sun && (int) bp.Name <= (int)Body.Rahu)
			{
				li.Text = string.Format("{0}   {1}", li.Text, Bodies.KarakasS[karaka_indices[(int) bp.Name]]);
			}

			li.SubItems.Add(longitudeToString(bp.Longitude));
			li.SubItems.Add(bp.Longitude.ToNakshatra().ToString());
			li.SubItems.Add(bp.Longitude.ToNakshatraPada().ToString());
			li.SubItems.Add(getNakLord(bp.Longitude));

			var dp = bp.ToDivisionPosition(options.DivisionType);
			li.SubItems.Add(dp.ZodiacHouse.ToString());
			li.SubItems.Add(dp.Part.ToString());
			li.SubItems.Add(bp.AmsaRuler( options.DivisionType.MultipleDivisions[0].Varga, dp.RulerIndex));
			li.SubItems.Add(longitudeToString(new Longitude(dp.CuspLower)));
			li.SubItems.Add(longitudeToString(new Longitude(dp.CuspHigher)));

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

	private class UserOptions : ICloneable
	{
		[PGNotVisible]
		public Division DivisionType
		{
			get;
			set;
		}

		[PGDisplayName("Division Type")]
		public DivisionType UIDivisionType
		{
			get => DivisionType.MultipleDivisions[0].Varga;
			set => DivisionType = new Division(value);
		}

		public Dasas.NakshatraLord NakshatraLord
		{
			get;
			set;
		}

		public object Clone()
		{
			var uo = new UserOptions
			{
				DivisionType = DivisionType,
				NakshatraLord = NakshatraLord
			};
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