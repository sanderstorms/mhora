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
using Mhora.Elements.Ghataka;
using Mhora.Elements.Kuta;
using Mhora.Tables;
using mhora.Util;

namespace Mhora.Components;

public class KutaMatchingControl : MhoraControl
{
	private readonly IContainer   components = null;
	private          Button       bFemaleChange;
	private          Button       bMaleChange;
	protected        ColumnHeader Female;
	private          Horoscope    h2;
	private          ColumnHeader Kuta;
	private          Label        label1;
	private          Label        label2;

	private ListView     lView;
	private ColumnHeader Male;
	private ContextMenu  mContext;
	private MenuItem     menuItem1;
	private MenuItem     menuItem2;
	private ColumnHeader Score;
	private TextBox      tbHorFemale;
	private TextBox      tbHorMale;

	public KutaMatchingControl(Horoscope _h, Horoscope _h2)
	{
		// This call is required by the Windows Form Designer.
		InitializeComponent();
		h          =  _h;
		h2         =  _h2;
		h.Changed  += OnRecalculate;
		h2.Changed += OnRecalculate;
		AddViewsToContextMenu(mContext);
		populateTextBoxes();
		OnRecalculate(h);
	}

	private void populateTextBoxes()
	{
		tbHorMale.Text   = "Current Chart";
		tbHorFemale.Text = "Current Chart";
		foreach (var f in (MhoraGlobalOptions.MainControl).MdiChildren)
		{
			if (f is MhoraChild)
			{
				var mch = (MhoraChild) f;
				if (mch.getHoroscope() == h)
				{
					tbHorMale.Text = mch.Text;
				}

				if (mch.getHoroscope() == h2)
				{
					tbHorFemale.Text = mch.Text;
				}
			}
		}
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

#region Designer generated code

	/// <summary>
	///     Required method for Designer support - do not modify
	///     the contents of this method with the code editor.
	/// </summary>
	private void InitializeComponent()
	{
		this.tbHorMale     = new System.Windows.Forms.TextBox();
		this.tbHorFemale   = new System.Windows.Forms.TextBox();
		this.lView         = new System.Windows.Forms.ListView();
		this.Kuta          = new System.Windows.Forms.ColumnHeader();
		this.Male          = new System.Windows.Forms.ColumnHeader();
		this.Female        = new System.Windows.Forms.ColumnHeader();
		this.Score         = new System.Windows.Forms.ColumnHeader();
		this.bMaleChange   = new System.Windows.Forms.Button();
		this.bFemaleChange = new System.Windows.Forms.Button();
		this.mContext      = new System.Windows.Forms.ContextMenu();
		this.menuItem1     = new System.Windows.Forms.MenuItem();
		this.menuItem2     = new System.Windows.Forms.MenuItem();
		this.label1        = new System.Windows.Forms.Label();
		this.label2        = new System.Windows.Forms.Label();
		this.SuspendLayout();
		// 
		// tbHorMale
		// 
		this.tbHorMale.Anchor   = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
		this.tbHorMale.Location = new System.Drawing.Point(72, 8);
		this.tbHorMale.Name     = "tbHorMale";
		this.tbHorMale.ReadOnly = true;
		this.tbHorMale.Size     = new System.Drawing.Size(320, 20);
		this.tbHorMale.TabIndex = 0;
		this.tbHorMale.Text     = "";
		// 
		// tbHorFemale
		// 
		this.tbHorFemale.Anchor   = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
		this.tbHorFemale.Location = new System.Drawing.Point(72, 40);
		this.tbHorFemale.Name     = "tbHorFemale";
		this.tbHorFemale.ReadOnly = true;
		this.tbHorFemale.Size     = new System.Drawing.Size(320, 20);
		this.tbHorFemale.TabIndex = 1;
		this.tbHorFemale.Text     = "";
		// 
		// lView
		// 
		this.lView.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
		this.lView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[]
		{
			this.Kuta,
			this.Male,
			this.Female,
			this.Score
		});
		this.lView.FullRowSelect        =  true;
		this.lView.Location             =  new System.Drawing.Point(8, 72);
		this.lView.Name                 =  "lView";
		this.lView.Size                 =  new System.Drawing.Size(544, 264);
		this.lView.TabIndex             =  2;
		this.lView.View                 =  System.Windows.Forms.View.Details;
		this.lView.SelectedIndexChanged += new System.EventHandler(this.lView_SelectedIndexChanged);
		// 
		// Kuta
		// 
		this.Kuta.Text  = "Kuta";
		this.Kuta.Width = 163;
		// 
		// Male
		// 
		this.Male.Text  = "Male";
		this.Male.Width = 126;
		// 
		// Female
		// 
		this.Female.Text  = "Female";
		this.Female.Width = 119;
		// 
		// Score
		// 
		this.Score.Text  = "Score";
		this.Score.Width = 107;
		// 
		// bMaleChange
		// 
		this.bMaleChange.Anchor   =  ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
		this.bMaleChange.Location =  new System.Drawing.Point(464, 8);
		this.bMaleChange.Name     =  "bMaleChange";
		this.bMaleChange.Size     =  new System.Drawing.Size(64, 24);
		this.bMaleChange.TabIndex =  3;
		this.bMaleChange.Text     =  "Change";
		this.bMaleChange.Click    += new System.EventHandler(this.bMaleChange_Click);
		// 
		// bFemaleChange
		// 
		this.bFemaleChange.Anchor   =  ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
		this.bFemaleChange.Location =  new System.Drawing.Point(464, 40);
		this.bFemaleChange.Name     =  "bFemaleChange";
		this.bFemaleChange.Size     =  new System.Drawing.Size(64, 23);
		this.bFemaleChange.TabIndex =  4;
		this.bFemaleChange.Text     =  "Change";
		this.bFemaleChange.Click    += new System.EventHandler(this.bFemaleChange_Click);
		// 
		// mContext
		// 
		this.mContext.MenuItems.AddRange(new System.Windows.Forms.MenuItem[]
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
		// label1
		// 
		this.label1.Location  = new System.Drawing.Point(8, 8);
		this.label1.Name      = "label1";
		this.label1.Size      = new System.Drawing.Size(48, 23);
		this.label1.TabIndex  = 5;
		this.label1.Text      = "Male:";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		// 
		// label2
		// 
		this.label2.Location  = new System.Drawing.Point(8, 40);
		this.label2.Name      = "label2";
		this.label2.Size      = new System.Drawing.Size(64, 23);
		this.label2.TabIndex  = 6;
		this.label2.Text      = "Female:";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		// 
		// KutaMatchingControl
		// 
		this.ContextMenu = this.mContext;
		this.Controls.Add(this.label2);
		this.Controls.Add(this.label1);
		this.Controls.Add(this.bFemaleChange);
		this.Controls.Add(this.bMaleChange);
		this.Controls.Add(this.lView);
		this.Controls.Add(this.tbHorFemale);
		this.Controls.Add(this.tbHorMale);
		this.Name =  "KutaMatchingControl";
		this.Size =  new System.Drawing.Size(560, 344);
		this.Load += new System.EventHandler(this.KutaMatchingControl_Load);
		this.ResumeLayout(false);
	}

#endregion

	public string getGhatakaString(bool gh)
	{
		if (gh)
		{
			return "Ghataka";
		}

		return "-";
	}

	public void OnRecalculate(object o)
	{
		var dtype = new Division(Vargas.DivisionType.Rasi);

		var l1 = h.GetPosition(Body.BodyType.Lagna);
		var l2 = h2.GetPosition(Body.BodyType.Lagna);
		var m1 = h.GetPosition(Body.BodyType.Moon);
		var m2 = h2.GetPosition(Body.BodyType.Moon);
		var z1 = m1.ToDivisionPosition(dtype).ZodiacHouse;
		var z2 = m2.ToDivisionPosition(dtype).ZodiacHouse;
		var n1 = m1.Longitude.ToNakshatra();
		var n2 = m2.Longitude.ToNakshatra();

		lView.Items.Clear();

		{
			var li = new ListViewItem("Nakshatra Yoni");
			li.SubItems.Add(KutaNakshatraYoni.GetType(n1) + " (" + KutaNakshatraYoni.GetSex(n1) + ")");
			li.SubItems.Add(KutaNakshatraYoni.GetType(n2) + " (" + KutaNakshatraYoni.GetSex(n2) + ")");
			lView.Items.Add(li);
		}
		{
			var li = new ListViewItem("Rasi Yoni");
			li.SubItems.Add(KutaRasiYoni.GetType(z1).ToString());
			li.SubItems.Add(KutaRasiYoni.GetType(z2).ToString());
			lView.Items.Add(li);
		}
		{
			var li = new ListViewItem("Varna");
			li.SubItems.Add(KutaVarna.GetType(n1).ToString());
			li.SubItems.Add(KutaVarna.GetType(n2).ToString());
			li.SubItems.Add(KutaVarna.GetScore(n1, n2) + "/" + KutaVarna.GetMaxScore());
			lView.Items.Add(li);
		}
		{
			var li = new ListViewItem("Gana (Chandra)");
			li.SubItems.Add(KutaGana.GetType(n1).ToString());
			li.SubItems.Add(KutaGana.GetType(n2).ToString());
			li.SubItems.Add(KutaGana.GetScore(n1, n2) + "/" + KutaGana.GetMaxScore());

			lView.Items.Add(li);
		}
		{
			var li = new ListViewItem("Gana (Lagna)");
			li.SubItems.Add(KutaGana.GetType(l1.Longitude.ToNakshatra()).ToString());
			li.SubItems.Add(KutaGana.GetType(l2.Longitude.ToNakshatra()).ToString());
			li.SubItems.Add(KutaGana.GetScore(l1.Longitude.ToNakshatra(), l2.Longitude.ToNakshatra()) + "/" + KutaGana.GetMaxScore());
			lView.Items.Add(li);
		}
		{
			var li = new ListViewItem("Vedha");
			li.SubItems.Add(KutaVedha.GetType(n1).ToString());
			li.SubItems.Add(KutaVedha.GetType(n2).ToString());
			li.SubItems.Add(KutaVedha.GetScore(n1, n2) + "/" + KutaVedha.GetMaxScore());
			lView.Items.Add(li);
		}
		{
			var li = new ListViewItem("Rajju");
			li.SubItems.Add(KutaRajju.GetType(n1).ToString());
			li.SubItems.Add(KutaRajju.GetType(n2).ToString());
			li.SubItems.Add(KutaRajju.GetScore(n1, n2) + "/" + KutaRajju.GetMaxScore());
			lView.Items.Add(li);
		}
		{
			var li = new ListViewItem("Nadi");
			li.SubItems.Add(KutaNadi.GetType(n1).ToString());
			li.SubItems.Add(KutaNadi.GetType(n2).ToString());
			li.SubItems.Add(KutaNadi.GetScore(n1, n2) + "/" + KutaNadi.GetMaxScore());
			lView.Items.Add(li);
		}
		{
			var li = new ListViewItem("Gotra (TD:Abhi)");
			li.SubItems.Add(KutaGotra.GetType(n1).ToString());
			li.SubItems.Add(KutaGotra.GetType(n2).ToString());
			li.SubItems.Add(KutaGotra.GetScore(n1, n2) + "/" + KutaGotra.GetMaxScore());
			lView.Items.Add(li);
		}
		{
			var li = new ListViewItem("Vihanga");
			li.SubItems.Add(KutaVihanga.GetType(n1).ToString());
			li.SubItems.Add(KutaVihanga.GetType(n2).ToString());
			li.SubItems.Add(KutaVihanga.GetDominator(n1, n2).ToString());
			lView.Items.Add(li);
		}
		{
			var li = new ListViewItem("Bhuta (Nakshatra)");
			li.SubItems.Add(KutaBhutaNakshatra.GetType(n1).ToString());
			li.SubItems.Add(KutaBhutaNakshatra.GetType(n2).ToString());
			li.SubItems.Add(KutaBhutaNakshatra.GetScore(n1, n2) + "/" + KutaBhutaNakshatra.GetMaxScore());
			lView.Items.Add(li);
		}
		{
			var li        = new ListViewItem("Ghataka (Moon)");
			var ja        = h.GetPosition(Body.BodyType.Moon).ToDivisionPosition(dtype).ZodiacHouse;
			var ch        = h2.GetPosition(Body.BodyType.Moon).ToDivisionPosition(dtype).ZodiacHouse;
			var isGhataka = GhatakaMoon.CheckGhataka(ja, ch);
			li.SubItems.Add(ja.ToString());
			li.SubItems.Add(ch.ToString());
			li.SubItems.Add(getGhatakaString(isGhataka));
			lView.Items.Add(li);
		}
		{
			var li        = new ListViewItem("Ghataka (Tithis)");
			var ja        = h.GetPosition(Body.BodyType.Moon).ToDivisionPosition(dtype).ZodiacHouse;
			var ltithi    = h2.GetPosition(Body.BodyType.Moon).Longitude.Sub(h2.GetPosition(Body.BodyType.Sun).Longitude);
			var t         = ltithi.ToTithi();
			var isGhataka = GhatakaTithi.CheckTithi(ja, t);
			li.SubItems.Add(ja.ToString());
			li.SubItems.Add(t.ToString());
			li.SubItems.Add(getGhatakaString(isGhataka));
			lView.Items.Add(li);
		}
		{
			var li        = new ListViewItem("Ghataka (Day)");
			var ja        = h.GetPosition(Body.BodyType.Moon).ToDivisionPosition(dtype).ZodiacHouse;
			var wd        = h2.Wday;
			var isGhataka = GhatakaDay.CheckDay(ja, wd);
			li.SubItems.Add(ja.ToString());
			li.SubItems.Add(wd.ToString());
			li.SubItems.Add(getGhatakaString(isGhataka));
			lView.Items.Add(li);
		}
		{
			var li        = new ListViewItem("Ghataka (Star)");
			var ja        = h.GetPosition(Body.BodyType.Moon).ToDivisionPosition(dtype).ZodiacHouse;
			var na        = h2.GetPosition(Body.BodyType.Moon).Longitude.ToNakshatra();
			var isGhataka = GhatakaStar.CheckStar(ja, na);
			li.SubItems.Add(ja.ToString());
			li.SubItems.Add(na.Name());
			li.SubItems.Add(getGhatakaString(isGhataka));
			lView.Items.Add(li);
		}
		{
			var li        = new ListViewItem("Ghataka Lagna(S)");
			var ja        = h.GetPosition(Body.BodyType.Moon).ToDivisionPosition(dtype).ZodiacHouse;
			var sa        = h2.GetPosition(Body.BodyType.Lagna).ToDivisionPosition(dtype).ZodiacHouse;
			var isGhataka = GhatakaLagnaSame.CheckLagna(ja, sa);
			li.SubItems.Add(ja.ToString());
			li.SubItems.Add(sa.ToString());
			li.SubItems.Add(getGhatakaString(isGhataka));
			lView.Items.Add(li);
		}
		{
			var li        = new ListViewItem("Ghataka Lagna(O)");
			var ja        = h.GetPosition(Body.BodyType.Moon).ToDivisionPosition(dtype).ZodiacHouse;
			var op        = h2.GetPosition(Body.BodyType.Lagna).ToDivisionPosition(dtype).ZodiacHouse;
			var isGhataka = GhatakaLagnaOpp.CheckLagna(ja, op);
			li.SubItems.Add(ja.ToString());
			li.SubItems.Add(op.ToString());
			li.SubItems.Add(getGhatakaString(isGhataka));
			lView.Items.Add(li);
		}
		ColorAndFontRows(lView);
	}

	private void KutaMatchingControl_Load(object sender, EventArgs e)
	{
	}

	private void bMaleChange_Click(object sender, EventArgs e)
	{
		var f = new ChooseHoroscopeControl();
		f.ShowDialog();
		if (f.GetHorsocope() != null)
		{
			h.Changed      -= OnRecalculate;
			h              =  f.GetHorsocope();
			tbHorMale.Text =  f.GetHoroscopeName();
			h.Changed      += OnRecalculate;
			OnRecalculate(h);
		}

		f.Dispose();
	}

	private void lView_SelectedIndexChanged(object sender, EventArgs e)
	{
	}

	private void bFemaleChange_Click(object sender, EventArgs e)
	{
		var f = new ChooseHoroscopeControl();
		f.ShowDialog();
		if (f.GetHorsocope() != null)
		{
			h2.Changed       -= OnRecalculate;
			h2               =  f.GetHorsocope();
			tbHorFemale.Text =  f.GetHoroscopeName();
			h2.Changed       += OnRecalculate;
			OnRecalculate(h2);
		}

		f.Dispose();
	}
}