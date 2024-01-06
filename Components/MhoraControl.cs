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
using System.Drawing;
using System.Windows.Forms;
using Mhora.Calculation;
using Mhora.Settings;
using Mhora.Varga;

namespace Mhora.Components;

/// <summary>
///     Summary description for MhoraControl.
/// </summary>
public class MhoraControl : UserControl
{
	/// <summary>
	///     Required designer variable.
	/// </summary>
	private readonly Container components = null;


	protected Horoscope h;

	protected Splitter sp;

	/*public MhoraControl(Horoscope _h)
	{
	    // This call is required by the Windows.Forms Form Designer.
	    InitializeComponent();
	    _h = h;

	    // TODO: Add any initialization after the InitForm call

	}*/
	public MhoraControl()
	{
		// This call is required by the Windows.Forms Form Designer.
		InitializeComponent();

		// TODO: Add any initialization after the InitForm call
	}

	public Horoscope ControlHoroscope
	{
		get => h;
		set => h = value;
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
		// 
		// MhoraControl
		// 
		this.AutoScroll =  true;
		this.Name       =  "MhoraControl";
		this.Size       =  new System.Drawing.Size(360, 216);
		this.Load       += new System.EventHandler(this.MhoraControl_Load);
	}

#endregion


	public void ViewControl(MhoraControlContainer.BaseUserOptions.ViewType vt)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(vt);
	}

	protected void ViewVimsottariDasa(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaVimsottari);
	}

	protected void ViewYogaVimsottariDasa(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaYogaVimsottari);
	}

	protected void ViewKaranaChaturashitiSamaDasa(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaKaranaChaturashitiSama);
	}

	protected void ViewAshtottariDasa(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaAshtottari);
	}

	protected void ViewTithiPraveshAshtottariDasaTithi(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaTithiPraveshAshtottariCompressedTithi);
	}

	protected void ViewTithiPraveshAshtottariDasaSolar(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaTithiPraveshAshtottariCompressedSolar);
	}

	protected void ViewTithiPraveshAshtottariDasaFixed(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaTithiPraveshAshtottariCompressedFixed);
	}

	protected void ViewTithiAshtottariDasa(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaTithiAshtottari);
	}

	protected void ViewYogaPraveshVimsottariDasaYoga(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaYogaPraveshVimsottariCompressedYoga);
	}

	protected void ViewShodashottariDasa(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaShodashottari);
	}

	protected void ViewDwadashottariDasa(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaDwadashottari);
	}

	protected void ViewPanchottariDasa(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaPanchottari);
	}

	protected void ViewShatabdikaDasa(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaShatabdika);
	}

	protected void ViewChaturashitiSamaDasa(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaChaturashitiSama);
	}

	protected void ViewDwisaptatiSamaDasa(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaDwisaptatiSama);
	}

	protected void ViewShatTrimshaSamaDasa(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaShatTrimshaSama);
	}

	protected void ViewYoginiDasa(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaYogini);
	}

	protected void ViewKalachakraDasa(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaKalachakra);
	}

	protected void ViewNaisargikaGrahaDasa(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.NaisargikaGrahaDasa);
	}

	protected void ViewKarakaKendradiGrahaDasa(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaKarakaKendradiGraha);
	}

	protected void ViewMoolaDasa(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaMoola);
	}

	protected void ViewNaisargikaRasiDasa(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.NaisargikaRasiDasa);
	}

	protected void ViewNarayanaDasa(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaNarayana);
	}

	protected void ViewNarayanaSamaDasa(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaNarayanaSama);
	}

	protected void ViewShoolaDasa(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaShoola);
	}

	protected void ViewNiryaanaShoolaDasa(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaNiryaanaShoola);
	}

	protected void ViewSuDasa(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaSu);
	}

	protected void ViewNavamsaDasa(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaNavamsa);
	}

	protected void ViewMandookaDasa(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaMandooka);
	}

	protected void ViewCharaDasa(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaChara);
	}

	protected void ViewTrikonaDasa(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaTrikona);
	}

	protected void ViewDrigDasa(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaDrig);
	}

	protected void ViewSudarshanaChakraDasa(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaSudarshanaChakra);
	}

	protected void ViewLagnaKendradiRasiDasa(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaLagnaKendradiRasi);
	}

	protected void ViewSudarshanaChakraDasaCompressed(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaSudarshanaChakraCompressed);
	}

	protected void ViewMuddaDasa(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaMudda);
	}

	protected void ViewTajakaDasa(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaTajaka);
	}

	protected void ViewTattwaDasa(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaTattwa);
	}

	protected void ControlCopyToClipboard(object sender, EventArgs e)
	{
		copyToClipboard();
	}

	protected virtual void copyToClipboard()
	{
	}

	protected void ViewTithiPraveshDasa(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaTithiPravesh);
	}

	protected void ViewYogaPraveshDasa(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaYogaPravesh);
	}

	protected void ViewNakshatraPraveshDasa(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaNakshatraPravesh);
	}

	protected void ViewKaranaPraveshDasa(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaKaranaPravesh);
	}

	protected void ViewKeyInfo(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.KeyInfo);
	}

	protected void ViewBasicCalculations(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.BasicCalculations);
	}

	protected void ViewDivisionalChart(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DivisionalChart);
	}

	protected void ViewBalas(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.Balas);
	}

	protected void ViewAshtakavarga(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.Ashtakavarga);
	}

	protected void ViewKutaMatching(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.KutaMatching);
	}

	protected void ViewNavamsaCircle(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.NavamsaCircle);
	}

	protected void ViewVaraChakra(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.VaraChakra);
	}

	protected void ViewSarvatobhadraChakra(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.ChakraSarvatobhadra81);
	}

	protected void ViewTransitsSearch(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.TransitSearch);
	}

	protected void ViewPanchanga(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.Panchanga);
	}

	protected void SplitViewHorizontal(object sender, EventArgs e)
	{
		var c_this  = (MhoraControlContainer) Parent;
		var c_grand = (MhoraSplitContainer) c_this.Parent;

		var dc1   = new DivisionalChart(h);
		var c_dc1 = new MhoraControlContainer(dc1);

		var dc2   = new DivisionalChart(h);
		var c_cd2 = new MhoraControlContainer(dc2);

		var ns = new MhoraSplitContainer(c_dc1);
		ns.Control1      = c_cd2;
		ns.DrawDock      = MhoraSplitContainer.DrawStyle.UpDown;
		c_grand.Control2 = ns;

		//c_grand.Control2 = c_dc;
		//c_dc.Dock = DockStyle.Fill;
		//return;

		/*
		MhoraSplitContainer new_split = new MhoraSplitContainer (c_this);
		new_split.Control2 = c_this;
		new_split.DrawDock = MhoraSplitContainer.DrawStyle.UpDown;
		new_split.Dock = DockStyle.Fill;

		c_grand.Control2 = new_split;
		c_grand.Show();
		*/
	}

	protected void AddViewsToContextMenu(ContextMenu cmenu)
	{
		var mBasicsMenu = new MenuItem("Basic Info");
		mBasicsMenu.MenuItems.Add("Key Info", ViewKeyInfo);
		mBasicsMenu.MenuItems.Add("Calculations", ViewBasicCalculations);
		mBasicsMenu.MenuItems.Add("Divisional Chart", ViewDivisionalChart);
		mBasicsMenu.MenuItems.Add("Balas", ViewBalas);
		mBasicsMenu.MenuItems.Add("Ashtakavarga", ViewAshtakavarga);

		var mChakrasMenu = new MenuItem("Chakras");
		mChakrasMenu.MenuItems.Add("Navamsa Chakra", ViewNavamsaCircle);
		mChakrasMenu.MenuItems.Add("Vara Chakra", ViewVaraChakra);
		mChakrasMenu.MenuItems.Add("Sarvatobhadra Chakra", ViewSarvatobhadraChakra);

		var mNakDasaMenu = new MenuItem("Nakshatra Dasa");
		mNakDasaMenu.MenuItems.Add("Vimsottari Dasa", ViewVimsottariDasa);
		mNakDasaMenu.MenuItems.Add("Ashottari Dasa", ViewAshtottariDasa);
		mNakDasaMenu.MenuItems.Add("-");
		mNakDasaMenu.MenuItems.Add("Panchottari Dasa", ViewPanchottariDasa);
		mNakDasaMenu.MenuItems.Add("Dwadashottari Dasa", ViewDwadashottariDasa);
		mNakDasaMenu.MenuItems.Add("Shodashottari Dasa", ViewShodashottariDasa);
		mNakDasaMenu.MenuItems.Add("Chaturashiti Sama Dasa", ViewChaturashitiSamaDasa);
		mNakDasaMenu.MenuItems.Add("Dwisaptati Sama Dasa", ViewDwisaptatiSamaDasa);
		mNakDasaMenu.MenuItems.Add("ShatTrimsha Sama Dasa", ViewShatTrimshaSamaDasa);
		mNakDasaMenu.MenuItems.Add("Shatabdika Dasa", ViewShatabdikaDasa);
		mNakDasaMenu.MenuItems.Add("Kalachakra Dasa", ViewKalachakraDasa);
		mNakDasaMenu.MenuItems.Add("Yogini Dasa", ViewYoginiDasa);
		mNakDasaMenu.MenuItems.Add("-");
		mNakDasaMenu.MenuItems.Add("Tithi Ashtottari Dasa", ViewTithiAshtottariDasa);
		mNakDasaMenu.MenuItems.Add("Yoga Vimsottari Dasa", ViewYogaVimsottariDasa);
		mNakDasaMenu.MenuItems.Add("Karana Chaturashiti Sama Dasa", ViewKaranaChaturashitiSamaDasa);

		var mGrahaDasaMenu = new MenuItem("Graha Dasa");
		mGrahaDasaMenu.MenuItems.Add("Naisargika Dasa", ViewNaisargikaGrahaDasa);
		mGrahaDasaMenu.MenuItems.Add("Moola Dasa", ViewMoolaDasa);
		mGrahaDasaMenu.MenuItems.Add("Karaka Kendradi Dasa", ViewKarakaKendradiGrahaDasa);

		var mRasiDasaMenu = new MenuItem("Rasi Dasa");
		mRasiDasaMenu.MenuItems.Add("Naisargika Dasa", ViewNaisargikaRasiDasa);
		mRasiDasaMenu.MenuItems.Add("Narayana Dasa", ViewNarayanaDasa);
		mRasiDasaMenu.MenuItems.Add("Narayana Sama Dasa", ViewNarayanaSamaDasa);
		mRasiDasaMenu.MenuItems.Add("Shoola Dasa", ViewShoolaDasa);
		mRasiDasaMenu.MenuItems.Add("Niryaana Shoola Dasa", ViewNiryaanaShoolaDasa);
		mRasiDasaMenu.MenuItems.Add("Drig Dasa", ViewDrigDasa);
		mRasiDasaMenu.MenuItems.Add("Su Dasa", ViewSuDasa);
		mRasiDasaMenu.MenuItems.Add("Sudarshana Chakra Dasa", ViewSudarshanaChakraDasa);
		mRasiDasaMenu.MenuItems.Add("Lagna Kendradi Rasi Dasa", ViewLagnaKendradiRasiDasa);
		mRasiDasaMenu.MenuItems.Add("Navamsa Ayur Dasa", ViewNavamsaDasa);
		mRasiDasaMenu.MenuItems.Add("Mandooka Dasa", ViewMandookaDasa);
		mRasiDasaMenu.MenuItems.Add("Chara Dasa", ViewCharaDasa);
		mRasiDasaMenu.MenuItems.Add("Trikona Dasa", ViewTrikonaDasa);

		var mRelatedChartMenu = new MenuItem("Yearly Charts");
		mRelatedChartMenu.MenuItems.Add("Tajaka Chart", ViewTajakaDasa);
		mRelatedChartMenu.MenuItems.Add("Sudarshana Chakra Dasa (Solar Year)", ViewSudarshanaChakraDasaCompressed);
		mRelatedChartMenu.MenuItems.Add("Mudda Dasa (Solar Year)", ViewMuddaDasa);
		mRelatedChartMenu.MenuItems.Add("-");
		mRelatedChartMenu.MenuItems.Add("Tithi Pravesh Chart", ViewTithiPraveshDasa);
		mRelatedChartMenu.MenuItems.Add("Tithi Pravesh Ashtottari Dasa (Tithi Year)", ViewTithiPraveshAshtottariDasaTithi);
		mRelatedChartMenu.MenuItems.Add("Tithi Pravesh Ashtottari Dasa (Solar Year)", ViewTithiPraveshAshtottariDasaSolar);
		mRelatedChartMenu.MenuItems.Add("Tithi Pravesh Ashtottari Dasa (Fixed Year)", ViewTithiPraveshAshtottariDasaFixed);
		mRelatedChartMenu.MenuItems.Add("-");
		mRelatedChartMenu.MenuItems.Add("Yoga Pravesh Chart", ViewYogaPraveshDasa);
		mRelatedChartMenu.MenuItems.Add("Yoga Pravesh Vimsottari Dasa (Yoga Year)", ViewYogaPraveshVimsottariDasaYoga);
		mRelatedChartMenu.MenuItems.Add("-");
		mRelatedChartMenu.MenuItems.Add("Nakshatra Pravesh Chart", ViewNakshatraPraveshDasa);
		mRelatedChartMenu.MenuItems.Add("-");
		mRelatedChartMenu.MenuItems.Add("Karana Pravesh Chart", ViewKaranaPraveshDasa);
		mRelatedChartMenu.MenuItems.Add("-");
		mRelatedChartMenu.MenuItems.Add("Tattwa Dasa", ViewTattwaDasa);


		var mOtherMenu = new MenuItem("Other");
		mOtherMenu.MenuItems.Add("Kuta Matching", ViewKutaMatching);
		mOtherMenu.MenuItems.Add("Transit Search", ViewTransitsSearch);
		mOtherMenu.MenuItems.Add("Panchanga", ViewPanchanga);
		//MenuItem mSplitMenu = new MenuItem ("Split View");
		//mSplitMenu.MenuItems.Add ("Split Horizontal", new EventHandler(SplitViewHorizontal));


		cmenu.MenuItems.Add(mBasicsMenu);
		cmenu.MenuItems.Add(mChakrasMenu);
		cmenu.MenuItems.Add(mNakDasaMenu);
		cmenu.MenuItems.Add(mGrahaDasaMenu);
		cmenu.MenuItems.Add(mRasiDasaMenu);
		cmenu.MenuItems.Add(mOtherMenu);
		cmenu.MenuItems.Add(mRelatedChartMenu);

		cmenu.MenuItems.Add("Copy To clipboard", ControlCopyToClipboard);
		//cmenu.MenuItems.Add (mSplitMenu);
	}

	protected void MhoraControl_Load(object sender, EventArgs e)
	{
	}

	protected virtual void FontRows(ListView mList)
	{
		mList.ForeColor = MhoraGlobalOptions.Instance.TableForegroundColor;
		var f = MhoraGlobalOptions.Instance.GeneralFont;
		//new Font ("Courier New", 10);
		mList.Font = f;
		foreach (ListViewItem li in mList.Items)
		{
			li.Font = f;
		}
	}

	protected virtual void ColorAndFontRows(ListView mList)
	{
		ColorRows(mList);
		FontRows(mList);
	}

	protected virtual void ColorRows(ListView mList)
	{
		var cList = new Color[2];
		cList[1] = MhoraGlobalOptions.Instance.TableOddRowColor;
		cList[0] = MhoraGlobalOptions.Instance.TableEvenRowColor;
		//cList[1] = Color.WhiteSmoke;

		for (var i = 0; i < mList.Items.Count; i++)
		{
			if (i % 2 == 1)
			{
				mList.Items[i].BackColor = cList[0];
			}
			else
			{
				mList.Items[i].BackColor = cList[1];
			}
		}

		mList.BackColor = MhoraGlobalOptions.Instance.TableBackgroundColor;
	}

	private void DoNothing(object o)
	{
	}
	/*protected void mChangeView_Click(object sender, System.EventArgs e)
	{
	    MhoraControlContainer cont = (MhoraControlContainer)this.Parent;
	    MhoraControlContainer.BaseUserOptions options = cont.options;
	    cont.h = this.h;
	    Form f = new MhoraOptions(options.Clone(), new ApplyOptions(cont.SetBaseOptions));
	    f.Show();
	}*/

	private void mViewDasaVimsottari_Click(object sender, EventArgs e)
	{
	}

	private void mDasa_Click(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaVimsottari);
	}

	private void mDivisionalChart_Click(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DivisionalChart);
	}
}