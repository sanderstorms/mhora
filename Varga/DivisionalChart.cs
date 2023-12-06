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
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Mhora.Body;
using Mhora.Calculation;
using Mhora.Chart;
using Mhora.Components;
using Mhora.Components.Property;
using Mhora.Settings;
using Mhora.Util;

namespace Mhora.Varga
{
    /// <summary>
    ///     Summary description for DivisionalChart.
    /// </summary>
    [Serializable]
    public class DivisionalChart : MhoraControl //System.Windows.Forms.UserControl
    {
        private ArrayList arudha_pos;
        private Brush     b = new SolidBrush(Color.Black);

        private bool             bInnerMode;
        public  HoroscopeOptions calculation_options;

        /// <summary>
        ///     Required designer variable.
        /// </summary>
        private Container components = null;

        public  ContextMenu contextMenu;
        private IDrawChart  dc;
        private ArrayList   div_pos;

        private Font fBase = new Font(MhoraGlobalOptions.Instance.VargaFont.FontFamily,
                                      MhoraGlobalOptions.Instance.VargaFont.SizeInPoints);

        private ArrayList graha_arudha_pos;

        private DivisionalChart innerControl;

        private string[] karakas_s =
        {
            "AK",
            "AmK",
            "BK",
            "MK",
            "PiK",
            "PuK",
            "JK",
            "DK"
        };

        private string[] karakas_s7 =
        {
            "AK",
            "AmK",
            "BK",
            "MK",
            "PiK",
            "JK",
            "DK"
        };

        private MenuItem    mAkshavedamsa;
        private MenuItem    mAshtamsa;
        private MenuItem    mAshtamsaRaman;
        private MenuItem    mAshtottaramsa;
        private MenuItem    mBhava;
        private MenuItem    mBhavaEqual;
        private MenuItem    mBhavaKoch;
        private MenuItem    mBhavaPlacidus;
        private MenuItem    mBhavaSripati;
        private MenuItem    mChaturamsa;
        private MenuItem    mChaturvimsamsa;
        private MenuItem    mDasamsa;
        private MenuItem    mDrekkanaJagannath;
        private MenuItem    mDrekkanaParasara;
        private MenuItem    mDrekkanaParivrittitraya;
        private MenuItem    mDrekkanaSomnath;
        private MenuItem    mDwadasamsa;
        private MenuItem    mDwadasamsaDwadasamsa;
        private MenuItem    menuBhavaAlcabitus;
        private MenuItem    menuBhavaAxial;
        private MenuItem    menuBhavaCampanus;
        private MenuItem    menuBhavaRegiomontanus;
        private MenuItem    menuItem1;
        private MenuItem    menuItem10;
        private MenuItem    menuItem11;
        private MenuItem    menuItem12;
        private MenuItem    menuItem18;
        private MenuItem    menuItem2;
        private MenuItem    menuItem3;
        private MenuItem    menuItem4;
        private MenuItem    menuItem5;
        private MenuItem    menuItem6;
        private MenuItem    menuItem7;
        private MenuItem    menuItem8;
        private MenuItem    menuItem9;
        private MenuItem    mExtrapolate;
        private MenuItem    mHoraJagannath;
        private MenuItem    mHoraKashinath;
        private MenuItem    mHoraParasara;
        private MenuItem    mHoraParivritti;
        private MenuItem    mKhavedamsa;
        private MenuItem    mLagnaChange;
        private MenuItem    mNadiamsa;
        private MenuItem    mNadiamsaCKN;
        private MenuItem    mNakshatramsa;
        private MenuItem    mNavamsa;
        private MenuItem    mNavamsaDwadasamsa;
        private MenuItem    mOptions;
        private MenuItem    mPanchamsa;
        private MenuItem    mRasi;
        private MenuItem    mRegularDasamsaBased;
        private MenuItem    mRegularFromHouse;
        private MenuItem    mRegularKendraChaturthamsa;
        private MenuItem    mRegularNakshatramsaBased;
        private MenuItem    mRegularParivritti;
        private MenuItem    mRegularSaptamsaBased;
        private MenuItem    mRegularShashthamsaBased;
        private MenuItem    mRegularShodasamsaBased;
        private MenuItem    mRegularTrikona;
        private MenuItem    mRegularVimsamsaBased;
        private MenuItem    mRudramsaRaman;
        private MenuItem    mRudramsaRath;
        private MenuItem    mSaptamsa;
        private MenuItem    mShashtamsa;
        private MenuItem    mShashtyamsa;
        private MenuItem    mShodasamsa;
        private MenuItem    mTrimsamsa;
        private MenuItem    mTrimsamsaParivritti;
        private MenuItem    mTrimsamsaSimple;
        private MenuItem    mViewCharaKarakas;
        private MenuItem    mViewCharaKarakas7;
        private MenuItem    mViewDualGrahaArudha;
        private MenuItem    mViewNormal;
        private MenuItem    mViewVarnada;
        private MenuItem    mVimsamsa;
        public  UserOptions options;

        private Pen pn_black = new Pen(Color.Black, (float)0.01);

        public  bool      PrintMode = false;
        private int[]     sav_bindus;
        private ArrayList varnada_pos;

        public DivisionalChart(Horoscope _h)
        {
            Constructor(_h);
            bInnerMode = false;
        }

        public DivisionalChart(Horoscope _h, bool bInner)
        {
            Constructor(_h);
            bInnerMode = true;
        }


        public void AddInnerControl()
        {
            innerControl = new DivisionalChart(h, true);
            Controls.Add(innerControl);
        }

        public void Constructor(Horoscope _h)
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitForm call
            h                                      =  _h;
            options                                =  new UserOptions();
            calculation_options                    =  h.options;
            h.Changed                              += OnRecalculate;
            MhoraGlobalOptions.DisplayPrefsChanged += OnRedisplay;
            OnRecalculate(h);
            SetChartStyle(options.ChartStyle);
            //dc = new SouthIndianChart();
            //dc = new EastIndianChart();
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
            this.contextMenu                = new System.Windows.Forms.ContextMenu();
            this.mLagnaChange               = new System.Windows.Forms.MenuItem();
            this.mOptions                   = new System.Windows.Forms.MenuItem();
            this.menuItem8                  = new System.Windows.Forms.MenuItem();
            this.mViewNormal                = new System.Windows.Forms.MenuItem();
            this.mViewDualGrahaArudha       = new System.Windows.Forms.MenuItem();
            this.mViewVarnada               = new System.Windows.Forms.MenuItem();
            this.mViewCharaKarakas          = new System.Windows.Forms.MenuItem();
            this.mViewCharaKarakas7         = new System.Windows.Forms.MenuItem();
            this.menuItem1                  = new System.Windows.Forms.MenuItem();
            this.mRasi                      = new System.Windows.Forms.MenuItem();
            this.mNavamsa                   = new System.Windows.Forms.MenuItem();
            this.menuItem7                  = new System.Windows.Forms.MenuItem();
            this.mBhava                     = new System.Windows.Forms.MenuItem();
            this.mBhavaEqual                = new System.Windows.Forms.MenuItem();
            this.mBhavaSripati              = new System.Windows.Forms.MenuItem();
            this.menuItem9                  = new System.Windows.Forms.MenuItem();
            this.menuBhavaAlcabitus         = new System.Windows.Forms.MenuItem();
            this.menuBhavaAxial             = new System.Windows.Forms.MenuItem();
            this.menuBhavaCampanus          = new System.Windows.Forms.MenuItem();
            this.mBhavaKoch                 = new System.Windows.Forms.MenuItem();
            this.mBhavaPlacidus             = new System.Windows.Forms.MenuItem();
            this.menuBhavaRegiomontanus     = new System.Windows.Forms.MenuItem();
            this.menuItem5                  = new System.Windows.Forms.MenuItem();
            this.mHoraParasara              = new System.Windows.Forms.MenuItem();
            this.mDrekkanaParasara          = new System.Windows.Forms.MenuItem();
            this.mChaturamsa                = new System.Windows.Forms.MenuItem();
            this.mPanchamsa                 = new System.Windows.Forms.MenuItem();
            this.mShashtamsa                = new System.Windows.Forms.MenuItem();
            this.mSaptamsa                  = new System.Windows.Forms.MenuItem();
            this.mAshtamsa                  = new System.Windows.Forms.MenuItem();
            this.mDasamsa                   = new System.Windows.Forms.MenuItem();
            this.mDwadasamsa                = new System.Windows.Forms.MenuItem();
            this.mShodasamsa                = new System.Windows.Forms.MenuItem();
            this.mVimsamsa                  = new System.Windows.Forms.MenuItem();
            this.mChaturvimsamsa            = new System.Windows.Forms.MenuItem();
            this.menuItem18                 = new System.Windows.Forms.MenuItem();
            this.mHoraParivritti            = new System.Windows.Forms.MenuItem();
            this.mHoraJagannath             = new System.Windows.Forms.MenuItem();
            this.mHoraKashinath             = new System.Windows.Forms.MenuItem();
            this.mDrekkanaParivrittitraya   = new System.Windows.Forms.MenuItem();
            this.mDrekkanaJagannath         = new System.Windows.Forms.MenuItem();
            this.mDrekkanaSomnath           = new System.Windows.Forms.MenuItem();
            this.mAshtamsaRaman             = new System.Windows.Forms.MenuItem();
            this.mRudramsaRath              = new System.Windows.Forms.MenuItem();
            this.mRudramsaRaman             = new System.Windows.Forms.MenuItem();
            this.mTrimsamsaParivritti       = new System.Windows.Forms.MenuItem();
            this.mTrimsamsaSimple           = new System.Windows.Forms.MenuItem();
            this.menuItem6                  = new System.Windows.Forms.MenuItem();
            this.mNakshatramsa              = new System.Windows.Forms.MenuItem();
            this.mTrimsamsa                 = new System.Windows.Forms.MenuItem();
            this.mKhavedamsa                = new System.Windows.Forms.MenuItem();
            this.mAkshavedamsa              = new System.Windows.Forms.MenuItem();
            this.mShashtyamsa               = new System.Windows.Forms.MenuItem();
            this.mAshtottaramsa             = new System.Windows.Forms.MenuItem();
            this.mNadiamsa                  = new System.Windows.Forms.MenuItem();
            this.mNadiamsaCKN               = new System.Windows.Forms.MenuItem();
            this.menuItem10                 = new System.Windows.Forms.MenuItem();
            this.mNavamsaDwadasamsa         = new System.Windows.Forms.MenuItem();
            this.mDwadasamsaDwadasamsa      = new System.Windows.Forms.MenuItem();
            this.mExtrapolate               = new System.Windows.Forms.MenuItem();
            this.menuItem11                 = new System.Windows.Forms.MenuItem();
            this.mRegularParivritti         = new System.Windows.Forms.MenuItem();
            this.mRegularFromHouse          = new System.Windows.Forms.MenuItem();
            this.mRegularSaptamsaBased      = new System.Windows.Forms.MenuItem();
            this.mRegularDasamsaBased       = new System.Windows.Forms.MenuItem();
            this.mRegularShashthamsaBased   = new System.Windows.Forms.MenuItem();
            this.menuItem12                 = new System.Windows.Forms.MenuItem();
            this.mRegularTrikona            = new System.Windows.Forms.MenuItem();
            this.mRegularShodasamsaBased    = new System.Windows.Forms.MenuItem();
            this.mRegularVimsamsaBased      = new System.Windows.Forms.MenuItem();
            this.mRegularKendraChaturthamsa = new System.Windows.Forms.MenuItem();
            this.mRegularNakshatramsaBased  = new System.Windows.Forms.MenuItem();
            this.menuItem3                  = new System.Windows.Forms.MenuItem();
            this.menuItem4                  = new System.Windows.Forms.MenuItem();
            this.menuItem2                  = new System.Windows.Forms.MenuItem();
            // 
            // contextMenu
            // 
            this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[]
            {
                this.mLagnaChange,
                this.mOptions,
                this.menuItem8,
                this.menuItem1,
                this.mRasi,
                this.mNavamsa,
                this.menuItem7,
                this.menuItem5,
                this.mHoraParasara,
                this.mDrekkanaParasara,
                this.mChaturamsa,
                this.mPanchamsa,
                this.mShashtamsa,
                this.mSaptamsa,
                this.mAshtamsa,
                this.mDasamsa,
                this.mDwadasamsa,
                this.mShodasamsa,
                this.mVimsamsa,
                this.mChaturvimsamsa,
                this.menuItem18,
                this.menuItem6,
                this.menuItem11,
                this.menuItem3,
                this.menuItem4
            });
            // 
            // mLagnaChange
            // 
            this.mLagnaChange.Index =  0;
            this.mLagnaChange.Text  =  "Lagna Change";
            this.mLagnaChange.Click += new System.EventHandler(this.mLagnaChange_Click);
            // 
            // mOptions
            // 
            this.mOptions.Index =  1;
            this.mOptions.Text  =  "&Options";
            this.mOptions.Click += new System.EventHandler(this.mOptions_Click);
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 2;
            this.menuItem8.MenuItems.AddRange(new System.Windows.Forms.MenuItem[]
            {
                this.mViewNormal,
                this.mViewDualGrahaArudha,
                this.mViewVarnada,
                this.mViewCharaKarakas,
                this.mViewCharaKarakas7
            });
            this.menuItem8.Text = "View";
            // 
            // mViewNormal
            // 
            this.mViewNormal.Index =  0;
            this.mViewNormal.Text  =  "Normal";
            this.mViewNormal.Click += new System.EventHandler(this.mViewNormal_Click);
            // 
            // mViewDualGrahaArudha
            // 
            this.mViewDualGrahaArudha.Index =  1;
            this.mViewDualGrahaArudha.Text  =  "Graha Arudha";
            this.mViewDualGrahaArudha.Click += new System.EventHandler(this.mViewDualGrahaArudha_Click);
            // 
            // mViewVarnada
            // 
            this.mViewVarnada.Index =  2;
            this.mViewVarnada.Text  =  "Varnada";
            this.mViewVarnada.Click += new System.EventHandler(this.mViewVarnada_Click);
            // 
            // mViewCharaKarakas
            // 
            this.mViewCharaKarakas.Index =  3;
            this.mViewCharaKarakas.Text  =  "Chara Karakas (8)";
            this.mViewCharaKarakas.Click += new System.EventHandler(this.mViewCharaKarakas_Click);
            // 
            // mViewCharaKarakas7
            // 
            this.mViewCharaKarakas7.Index =  4;
            this.mViewCharaKarakas7.Text  =  "Chara Karakas (7)";
            this.mViewCharaKarakas7.Click += new System.EventHandler(this.mViewCharaKarakas7_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 3;
            this.menuItem1.Text  = "-";
            // 
            // mRasi
            // 
            this.mRasi.Index =  4;
            this.mRasi.Text  =  "Rasi";
            this.mRasi.Click += new System.EventHandler(this.mRasi_Click);
            // 
            // mNavamsa
            // 
            this.mNavamsa.Index =  5;
            this.mNavamsa.Text  =  "Navamsa";
            this.mNavamsa.Click += new System.EventHandler(this.mNavamsa_Click);
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 6;
            this.menuItem7.MenuItems.AddRange(new System.Windows.Forms.MenuItem[]
            {
                this.mBhava,
                this.mBhavaEqual,
                this.mBhavaSripati,
                this.menuItem9,
                this.menuBhavaAlcabitus,
                this.menuBhavaAxial,
                this.menuBhavaCampanus,
                this.mBhavaKoch,
                this.mBhavaPlacidus,
                this.menuBhavaRegiomontanus
            });
            this.menuItem7.Text = "Bhavas";
            // 
            // mBhava
            // 
            this.mBhava.Index =  0;
            this.mBhava.Text  =  "Equal houses (9 padas)";
            this.mBhava.Click += new System.EventHandler(this.mBhava_Click);
            // 
            // mBhavaEqual
            // 
            this.mBhavaEqual.Index =  1;
            this.mBhavaEqual.Text  =  "Equal houses (30 degrees)";
            this.mBhavaEqual.Click += new System.EventHandler(this.mBhavaEqual_Click);
            // 
            // mBhavaSripati
            // 
            this.mBhavaSripati.Index =  2;
            this.mBhavaSripati.Text  =  "Sripati (Poryphory)";
            this.mBhavaSripati.Click += new System.EventHandler(this.mBhavaSripati_Click);
            // 
            // menuItem9
            // 
            this.menuItem9.Index = 3;
            this.menuItem9.Text  = "-";
            // 
            // menuBhavaAlcabitus
            // 
            this.menuBhavaAlcabitus.Index =  4;
            this.menuBhavaAlcabitus.Text  =  "Alcabitus";
            this.menuBhavaAlcabitus.Click += new System.EventHandler(this.menuBhavaAlcabitus_Click);
            // 
            // menuBhavaAxial
            // 
            this.menuBhavaAxial.Index =  5;
            this.menuBhavaAxial.Text  =  "Axial";
            this.menuBhavaAxial.Click += new System.EventHandler(this.menuBhavaAxial_Click);
            // 
            // menuBhavaCampanus
            // 
            this.menuBhavaCampanus.Index =  6;
            this.menuBhavaCampanus.Text  =  "Campanus";
            this.menuBhavaCampanus.Click += new System.EventHandler(this.menuBhavaCampanus_Click);
            // 
            // mBhavaKoch
            // 
            this.mBhavaKoch.Index =  7;
            this.mBhavaKoch.Text  =  "Koch";
            this.mBhavaKoch.Click += new System.EventHandler(this.mBhavaKoch_Click);
            // 
            // mBhavaPlacidus
            // 
            this.mBhavaPlacidus.Index =  8;
            this.mBhavaPlacidus.Text  =  "Placidus";
            this.mBhavaPlacidus.Click += new System.EventHandler(this.mBhavaPlacidus_Click);
            // 
            // menuBhavaRegiomontanus
            // 
            this.menuBhavaRegiomontanus.Index =  9;
            this.menuBhavaRegiomontanus.Text  =  "Regiomontanus";
            this.menuBhavaRegiomontanus.Click += new System.EventHandler(this.menuBhavaRegiomontanus_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 7;
            this.menuItem5.Text  = "-";
            // 
            // mHoraParasara
            // 
            this.mHoraParasara.Index =  8;
            this.mHoraParasara.Text  =  "D-2: Hora";
            this.mHoraParasara.Click += new System.EventHandler(this.mHoraParasara_Click);
            // 
            // mDrekkanaParasara
            // 
            this.mDrekkanaParasara.Index =  9;
            this.mDrekkanaParasara.Text  =  "D-3: Drekkana";
            this.mDrekkanaParasara.Click += new System.EventHandler(this.mDrekkanaParasara_Click);
            // 
            // mChaturamsa
            // 
            this.mChaturamsa.Index =  10;
            this.mChaturamsa.Text  =  "D-4: Chaturamsa";
            this.mChaturamsa.Click += new System.EventHandler(this.mChaturamsa_Click);
            // 
            // mPanchamsa
            // 
            this.mPanchamsa.Index =  11;
            this.mPanchamsa.Text  =  "D-5: Panchamsa";
            this.mPanchamsa.Click += new System.EventHandler(this.mPanchamsa_Click);
            // 
            // mShashtamsa
            // 
            this.mShashtamsa.Index =  12;
            this.mShashtamsa.Text  =  "D-6: Sashtamsa";
            this.mShashtamsa.Click += new System.EventHandler(this.mShashtamsa_Click);
            // 
            // mSaptamsa
            // 
            this.mSaptamsa.Index =  13;
            this.mSaptamsa.Text  =  "D-7: Saptamsa";
            this.mSaptamsa.Click += new System.EventHandler(this.mSaptamsa_Click);
            // 
            // mAshtamsa
            // 
            this.mAshtamsa.Index =  14;
            this.mAshtamsa.Text  =  "D-8: Ashtamsa";
            this.mAshtamsa.Click += new System.EventHandler(this.mAshtamsa_Click);
            // 
            // mDasamsa
            // 
            this.mDasamsa.Index =  15;
            this.mDasamsa.Text  =  "D-10: Dasamsa";
            this.mDasamsa.Click += new System.EventHandler(this.mDasamsa_Click);
            // 
            // mDwadasamsa
            // 
            this.mDwadasamsa.Index =  16;
            this.mDwadasamsa.Text  =  "D-12: Dwadasamsa";
            this.mDwadasamsa.Click += new System.EventHandler(this.mDwadasamsa_Click);
            // 
            // mShodasamsa
            // 
            this.mShodasamsa.Index =  17;
            this.mShodasamsa.Text  =  "D-16: Shodasamsa";
            this.mShodasamsa.Click += new System.EventHandler(this.mShodasamsa_Click);
            // 
            // mVimsamsa
            // 
            this.mVimsamsa.Index =  18;
            this.mVimsamsa.Text  =  "D-20: Vimsamsa";
            this.mVimsamsa.Click += new System.EventHandler(this.mVimsamsa_Click);
            // 
            // mChaturvimsamsa
            // 
            this.mChaturvimsamsa.Index =  19;
            this.mChaturvimsamsa.Text  =  "D-24: Chaturvimsamsa";
            this.mChaturvimsamsa.Click += new System.EventHandler(this.mChaturvimsamsa_Click);
            // 
            // menuItem18
            // 
            this.menuItem18.Index = 20;
            this.menuItem18.MenuItems.AddRange(new System.Windows.Forms.MenuItem[]
            {
                this.mHoraParivritti,
                this.mHoraJagannath,
                this.mHoraKashinath,
                this.mDrekkanaParivrittitraya,
                this.mDrekkanaJagannath,
                this.mDrekkanaSomnath,
                this.mAshtamsaRaman,
                this.mRudramsaRath,
                this.mRudramsaRaman,
                this.mTrimsamsaParivritti,
                this.mTrimsamsaSimple
            });
            this.menuItem18.Text = "Other Vargas";
            // 
            // mHoraParivritti
            // 
            this.mHoraParivritti.Index =  0;
            this.mHoraParivritti.Text  =  "D-2: Parivritti Dvaya Hora";
            this.mHoraParivritti.Click += new System.EventHandler(this.mHoraParivritti_Click);
            // 
            // mHoraJagannath
            // 
            this.mHoraJagannath.Enabled =  false;
            this.mHoraJagannath.Index   =  1;
            this.mHoraJagannath.Text    =  "D-2: Jagannath Hora";
            this.mHoraJagannath.Click   += new System.EventHandler(this.mHoraJagannath_Click);
            // 
            // mHoraKashinath
            // 
            this.mHoraKashinath.Index =  2;
            this.mHoraKashinath.Text  =  "D-2: Kashinath Hora";
            this.mHoraKashinath.Click += new System.EventHandler(this.mHoraKashinath_Click);
            // 
            // mDrekkanaParivrittitraya
            // 
            this.mDrekkanaParivrittitraya.Index =  3;
            this.mDrekkanaParivrittitraya.Text  =  "D-3: Parivritti Traya Drekkana";
            this.mDrekkanaParivrittitraya.Click += new System.EventHandler(this.mDrekkanaParivrittitraya_Click);
            // 
            // mDrekkanaJagannath
            // 
            this.mDrekkanaJagannath.Index =  4;
            this.mDrekkanaJagannath.Text  =  "D-3: Jagannath Drekkana";
            this.mDrekkanaJagannath.Click += new System.EventHandler(this.mDrekkanaJagannath_Click);
            // 
            // mDrekkanaSomnath
            // 
            this.mDrekkanaSomnath.Index =  5;
            this.mDrekkanaSomnath.Text  =  "D-3: Somnath Drekkana";
            this.mDrekkanaSomnath.Click += new System.EventHandler(this.mDrekkanaSomnath_Click);
            // 
            // mAshtamsaRaman
            // 
            this.mAshtamsaRaman.Index =  6;
            this.mAshtamsaRaman.Text  =  "D-8: Raman Ashtamsa";
            this.mAshtamsaRaman.Click += new System.EventHandler(this.mAshtamsaRaman_Click);
            // 
            // mRudramsaRath
            // 
            this.mRudramsaRath.Index =  7;
            this.mRudramsaRath.Text  =  "D-11: Rath Rudramsa";
            this.mRudramsaRath.Click += new System.EventHandler(this.mRudramsaRath_Click);
            // 
            // mRudramsaRaman
            // 
            this.mRudramsaRaman.Index =  8;
            this.mRudramsaRaman.Text  =  "D-11: Raman Rudramsa";
            this.mRudramsaRaman.Click += new System.EventHandler(this.mRudramsaRaman_Click);
            // 
            // mTrimsamsaParivritti
            // 
            this.mTrimsamsaParivritti.Index =  9;
            this.mTrimsamsaParivritti.Text  =  "D-30: Parivritti Trimsa Trimsamasa";
            this.mTrimsamsaParivritti.Click += new System.EventHandler(this.mTrimsamsaParivritti_Click);
            // 
            // mTrimsamsaSimple
            // 
            this.mTrimsamsaSimple.Index =  10;
            this.mTrimsamsaSimple.Text  =  "D-30: Zodiacal Trimsamsa";
            this.mTrimsamsaSimple.Click += new System.EventHandler(this.mTrimsamsaSimple_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 21;
            this.menuItem6.MenuItems.AddRange(new System.Windows.Forms.MenuItem[]
            {
                this.mNakshatramsa,
                this.mTrimsamsa,
                this.mKhavedamsa,
                this.mAkshavedamsa,
                this.mShashtyamsa,
                this.mAshtottaramsa,
                this.mNadiamsa,
                this.mNadiamsaCKN,
                this.menuItem10,
                this.mNavamsaDwadasamsa,
                this.mDwadasamsaDwadasamsa,
                this.mExtrapolate
            });
            this.menuItem6.Text = "Higher Vargas";
            // 
            // mNakshatramsa
            // 
            this.mNakshatramsa.Index =  0;
            this.mNakshatramsa.Text  =  "D-27: Nakshatramsa";
            this.mNakshatramsa.Click += new System.EventHandler(this.mNakshatramsa_Click);
            // 
            // mTrimsamsa
            // 
            this.mTrimsamsa.Index =  1;
            this.mTrimsamsa.Text  =  "D-30: Trimsamsa";
            this.mTrimsamsa.Click += new System.EventHandler(this.mTrimsamsa_Click);
            // 
            // mKhavedamsa
            // 
            this.mKhavedamsa.Index =  2;
            this.mKhavedamsa.Text  =  "D-40: Khavedamsa";
            this.mKhavedamsa.Click += new System.EventHandler(this.mKhavedamsa_Click);
            // 
            // mAkshavedamsa
            // 
            this.mAkshavedamsa.Index =  3;
            this.mAkshavedamsa.Text  =  "D-45: Akshavedamsa";
            this.mAkshavedamsa.Click += new System.EventHandler(this.mAkshavedamsa_Click_1);
            // 
            // mShashtyamsa
            // 
            this.mShashtyamsa.Index =  4;
            this.mShashtyamsa.Text  =  "D-60: Shashtyamsa";
            this.mShashtyamsa.Click += new System.EventHandler(this.mShashtyamsa_Click_1);
            // 
            // mAshtottaramsa
            // 
            this.mAshtottaramsa.Index =  5;
            this.mAshtottaramsa.Text  =  "D-108: Parivritti Ashtottaramsa";
            this.mAshtottaramsa.Click += new System.EventHandler(this.menuItem7_Click);
            // 
            // mNadiamsa
            // 
            this.mNadiamsa.Index =  6;
            this.mNadiamsa.Text  =  "D-150: Nadiamsa";
            this.mNadiamsa.Click += new System.EventHandler(this.mNadiamsa_Click);
            // 
            // mNadiamsaCKN
            // 
            this.mNadiamsaCKN.Index =  7;
            this.mNadiamsaCKN.Text  =  "D-150: Nadiamsa (Variable)";
            this.mNadiamsaCKN.Click += new System.EventHandler(this.mNadiamsaCKN_Click);
            // 
            // menuItem10
            // 
            this.menuItem10.Index = 8;
            this.menuItem10.Text  = "-";
            // 
            // mNavamsaDwadasamsa
            // 
            this.mNavamsaDwadasamsa.Index =  9;
            this.mNavamsaDwadasamsa.Text  =  "D-108: Navamsa Dwadasamsa";
            this.mNavamsaDwadasamsa.Click += new System.EventHandler(this.mNavamsaDwadasamsa_Click);
            // 
            // mDwadasamsaDwadasamsa
            // 
            this.mDwadasamsaDwadasamsa.Index =  10;
            this.mDwadasamsaDwadasamsa.Text  =  "D-144: Dwadasamsa Dwadasamsa";
            this.mDwadasamsaDwadasamsa.Click += new System.EventHandler(this.mDwadasamsaDwadasamsa_Click);
            // 
            // mExtrapolate
            // 
            this.mExtrapolate.Index =  11;
            this.mExtrapolate.Text  =  "Extrapolate Horoscope";
            this.mExtrapolate.Click += new System.EventHandler(this.mExtrapolate_Click);
            // 
            // menuItem11
            // 
            this.menuItem11.Index = 22;
            this.menuItem11.MenuItems.AddRange(new System.Windows.Forms.MenuItem[]
            {
                this.mRegularParivritti,
                this.mRegularFromHouse,
                this.mRegularSaptamsaBased,
                this.mRegularDasamsaBased,
                this.mRegularShashthamsaBased,
                this.menuItem12,
                this.mRegularTrikona,
                this.mRegularShodasamsaBased,
                this.mRegularVimsamsaBased,
                this.mRegularKendraChaturthamsa,
                this.mRegularNakshatramsaBased
            });
            this.menuItem11.Text = "Custom Varga Variations";
            // 
            // mRegularParivritti
            // 
            this.mRegularParivritti.Index =  0;
            this.mRegularParivritti.Text  =  "Regular: Parivritti";
            this.mRegularParivritti.Click += new System.EventHandler(this.mRegularParivritti_Click);
            // 
            // mRegularFromHouse
            // 
            this.mRegularFromHouse.Index =  1;
            this.mRegularFromHouse.Text  =  "Regular: 1: (d-12,d-60 based)";
            this.mRegularFromHouse.Click += new System.EventHandler(this.mRegularFromHouse_Click);
            // 
            // mRegularSaptamsaBased
            // 
            this.mRegularSaptamsaBased.Index =  2;
            this.mRegularSaptamsaBased.Text  =  "Regular: 1,7:  (d-7 based)";
            this.mRegularSaptamsaBased.Click += new System.EventHandler(this.mRegularSaptamsaBased_Click);
            // 
            // mRegularDasamsaBased
            // 
            this.mRegularDasamsaBased.Index =  3;
            this.mRegularDasamsaBased.Text  =  "Regular: 1,9: (d-10 based)";
            this.mRegularDasamsaBased.Click += new System.EventHandler(this.mRegularDasamsaBased_Click);
            // 
            // mRegularShashthamsaBased
            // 
            this.mRegularShashthamsaBased.Index =  4;
            this.mRegularShashthamsaBased.Text  =  "Regular: Ari,Lib:  (d-6, d-40 based)";
            this.mRegularShashthamsaBased.Click += new System.EventHandler(this.mRegularShashthamsaBased_Click);
            // 
            // menuItem12
            // 
            this.menuItem12.Index =  5;
            this.menuItem12.Text  =  "Regular: Leo,Can: (d-24 based)";
            this.menuItem12.Click += new System.EventHandler(this.menuItem12_Click);
            // 
            // mRegularTrikona
            // 
            this.mRegularTrikona.Index =  6;
            this.mRegularTrikona.Text  =  "Trikona: 1,5,9: (d-3 based)";
            this.mRegularTrikona.Click += new System.EventHandler(this.mRegularTrikona_Click);
            // 
            // mRegularShodasamsaBased
            // 
            this.mRegularShodasamsaBased.Index =  7;
            this.mRegularShodasamsaBased.Text  =  "Trikona: Ari,Leo,Sag: (d-16, d-45 based)";
            this.mRegularShodasamsaBased.Click += new System.EventHandler(this.mRegularShodasamsaBased_Click);
            // 
            // mRegularVimsamsaBased
            // 
            this.mRegularVimsamsaBased.Index =  8;
            this.mRegularVimsamsaBased.Text  =  "Trikona: Ari,Sag,Leo: (d-8, d-20 based)";
            this.mRegularVimsamsaBased.Click += new System.EventHandler(this.mRegularVimsamsaBased_Click);
            // 
            // mRegularKendraChaturthamsa
            // 
            this.mRegularKendraChaturthamsa.Index =  9;
            this.mRegularKendraChaturthamsa.Text  =  "Kendra: 1,4,7,10: (d-4 based)";
            this.mRegularKendraChaturthamsa.Click += new System.EventHandler(this.mRegularKendraChaturthamsa_Click);
            // 
            // mRegularNakshatramsaBased
            // 
            this.mRegularNakshatramsaBased.Index =  10;
            this.mRegularNakshatramsaBased.Text  =  "Kendra: Ari,Can,Lib,Cap: (d-27 based)";
            this.mRegularNakshatramsaBased.Click += new System.EventHandler(this.mRegularNakshatramsaBased_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 23;
            this.menuItem3.Text  = "-";
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 24;
            this.menuItem4.Text  = "-";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = -1;
            this.menuItem2.Text  = "Change view 2";
            // 
            // DivisionalChart
            // 
            this.AllowDrop   =  true;
            this.ContextMenu =  this.contextMenu;
            this.Font        =  new System.Drawing.Font("Times New Roman", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.Name        =  "DivisionalChart";
            this.Size        =  new System.Drawing.Size(504, 312);
            this.DragEnter   += new System.Windows.Forms.DragEventHandler(this.DivisionalChart_DragEnter);
            this.Resize      += new System.EventHandler(this.DivisionalChart_Resize);
            this.Load        += new System.EventHandler(this.DivisionalChart_Load);
            this.DragLeave   += new System.EventHandler(this.DivisionalChart_DragLeave);
            this.Paint       += new System.Windows.Forms.PaintEventHandler(this.DivisionalChart_Paint);
            this.DragDrop    += new System.Windows.Forms.DragEventHandler(this.DivisionalChart_DragDrop);
            this.MouseLeave  += new System.EventHandler(this.DivisionalChart_MouseLeave);
            this.MouseDown   += new System.Windows.Forms.MouseEventHandler(this.DivisionalChart_MouseDown);
        }

#endregion

        private void DivisionalChart_Load(object sender, EventArgs e)
        {
            AddViewsToContextMenu(contextMenu);
        }

        private Point GetOffset(ZodiacHouse zh, int item)
        {
            return dc.GetSmallItemOffset(zh, item);
        }

        private void AddItem(Graphics g, ZodiacHouse zh, int item, DivisionPosition dp, bool large)
        {
            string s = null;

            if (dp.type == BodyType.Name.GrahaArudha ||
                dp.type == BodyType.Name.Varnada)
            {
                s = dp.otherString;
            }
            else
            {
                s = Body.Body.toShortString(dp.name);
            }

            AddItem(g, zh, item, dp, large, s);
        }

        private void AddItem(Graphics g, ZodiacHouse zh, int item, DivisionPosition dp, bool large, string s)
        {
            Point p;
            Font  f;

            if (large)
            {
                p = dc.GetItemOffset(zh, item);
                f = fBase;
                if (dp.type == BodyType.Name.Graha)
                {
                    var bp = h.getPosition(dp.name);
                    if (bp.speed_longitude < 0.0 && bp.name != Body.Body.Name.Rahu && bp.name != Body.Body.Name.Ketu)
                    {
                        f = new Font(fBase.Name, fBase.Size, FontStyle.Underline);
                    }
                }
                else if (dp.name == Body.Body.Name.Lagna)
                {
                    f = new Font(fBase.Name, fBase.Size, FontStyle.Bold);
                }
            }
            else
            {
                var fs = FontStyle.Regular;
                if (dp.type == BodyType.Name.BhavaArudhaSecondary)
                {
                    fs = FontStyle.Italic;
                }

                p = dc.GetSmallItemOffset(zh, item);
                f = new Font(fBase.Name, fBase.SizeInPoints - 1, fs);
            }

            if (dp.type == BodyType.Name.GrahaArudha)
            {
                f = new Font(fBase.Name, fBase.SizeInPoints - 1);
            }

            switch (dp.type)
            {
                case BodyType.Name.Graha:
                case BodyType.Name.GrahaArudha:
                    b = new SolidBrush(MhoraGlobalOptions.Instance.VargaGrahaColor);
                    break;
                case BodyType.Name.SpecialLagna:
                    b = new SolidBrush(MhoraGlobalOptions.Instance.VargaSpecialLagnaColor);
                    break;
                case BodyType.Name.BhavaArudha:
                case BodyType.Name.Varnada:
                case BodyType.Name.BhavaArudhaSecondary:
                    b = new SolidBrush(MhoraGlobalOptions.Instance.VargaSecondaryColor);
                    break;
                case BodyType.Name.Lagna:
                    b = new SolidBrush(MhoraGlobalOptions.Instance.VargaLagnaColor);
                    break;
            }

            Font f2 = null;
            if (bInnerMode)
            {
                f2 = new Font(f.FontFamily, f.SizeInPoints + 1, f.Style);
            }
            else
            {
                f2 = f;
            }

            //SizeF sf = g.MeasureString (s, f, this.Width);
            //g.FillRectangle(r, p.X, p.Y, sf.Width, sf.Height);
            g.DrawString(s, f2, b, p.X, p.Y);

            if (options.Varga.MultipleDivisions.Length   == 1                        &&
                options.Varga.MultipleDivisions[0].Varga == Basics.DivisionType.Rasi &&
                PrintMode                                == false                    &&
                (dp.type == BodyType.Name.Graha ||
                 dp.type == BodyType.Name.Lagna))
            {
                var   pLon = dc.GetDegreeOffset(h.getPosition(dp.name).longitude);
                var   pn   = new Pen(MhoraGlobalOptions.Instance.getBinduColor(dp.name), (float)0.01);
                Brush br   = new SolidBrush(MhoraGlobalOptions.Instance.getBinduColor(dp.name));
                g.FillEllipse(br, pLon.X - 1, pLon.Y - 1, 4, 4);
                //g.DrawEllipse(pn, pLon.X-1, pLon.Y-1, 2, 2);
                g.DrawEllipse(new Pen(Color.Gray), pLon.X - 1, pLon.Y - 1, 4, 4);
            }
        }


        private void PaintObjects(Graphics g)
        {
            switch (options.ViewStyle)
            {
                case UserOptions.EViewStyle.Panchanga:
                case UserOptions.EViewStyle.Normal:
                case UserOptions.EViewStyle.Varnada:
                    PaintNormalView(g);
                    break;
                case UserOptions.EViewStyle.DualGrahaArudha:
                    PaintDualGrahaArudhasView(g);
                    break;
                case UserOptions.EViewStyle.CharaKarakas7:
                case UserOptions.EViewStyle.CharaKarakas8:
                    PaintCharaKarakas(g);
                    break;
            }
        }


        private void PaintCharaKarakas(Graphics g)
        {
            var nItems = new int[13]
            {
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            };
            var al = new ArrayList();

            // number of karakas to display
            var max = 0;
            if (options.ViewStyle == UserOptions.EViewStyle.CharaKarakas7)
            {
                max = (int)Body.Body.Name.Saturn;
            }
            else
            {
                max = (int)Body.Body.Name.Rahu;
            }

            // determine karakas
            for (var i = (int)Body.Body.Name.Sun; i <= max; i++)
            {
                var b   = (Body.Body.Name)i;
                var bp  = h.getPosition(b);
                var bkc = new KarakaComparer(bp);
                al.Add(bkc);
            }

            al.Sort();

            var kindex = new int[max + 1];
            for (var i = 0; i <= max; i++)
            {
                var bp = ((KarakaComparer)al[i]).GetPosition;
                kindex[(int)bp.name] = i;
            }


            // display bodies
            for (var i = 0; i <= max; i++)
            {
                var b  = (Body.Body.Name)i;
                var dp = (DivisionPosition)div_pos[i];
                var zh = dp.zodiac_house;
                var j  = (int)zh.value;
                nItems[j]++;
                if (options.ViewStyle == UserOptions.EViewStyle.CharaKarakas7)
                {
                    AddItem(g, zh, nItems[j], dp, true, karakas_s7[kindex[i]]);
                }
                else
                {
                    AddItem(g, zh, nItems[j], dp, true, karakas_s[kindex[i]]);
                }
            }

            var dp2 = (DivisionPosition)div_pos[(int)Body.Body.Name.Lagna];
            var zh2 = dp2.zodiac_house;
            var j2  = (int)zh2.value;
            nItems[j2]++;
            AddItem(g, zh2, nItems[j2], dp2, true);
        }

        private void PaintDualGrahaArudhasView(Graphics g)
        {
            var nItems = new int[13]
            {
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            };

            DivisionPosition dpo;
            int              i;
            dpo = h.getPosition(Body.Body.Name.Lagna).toDivisionPosition(options.Varga);
            i   = (int)dpo.zodiac_house.value;
            nItems[i]++;
            AddItem(g, dpo.zodiac_house, nItems[i], dpo, true);


            foreach (DivisionPosition dp in graha_arudha_pos)
            {
                i = (int)dp.zodiac_house.value;
                nItems[i]++;
                AddItem(g, dp.zodiac_house, nItems[i], dp, true);
            }
        }

        private void PaintSAV(Graphics g)
        {
            if (PrintMode)
            {
                return;
            }

            if (false == MhoraGlobalOptions.Instance.VargaShowSAVVarga &&
                false == MhoraGlobalOptions.Instance.VargaShowSAVRasi)
            {
                return;
            }

            var   zh = new ZodiacHouse(ZodiacHouse.Name.Ari);
            Brush b  = new SolidBrush(MhoraGlobalOptions.Instance.VargaSAVColor);
            var   f  = MhoraGlobalOptions.Instance.GeneralFont;
            for (var i = 1; i <= 12; i++)
            {
                var zhi = zh.add(i);
                var p   = dc.GetSingleItemOffset(zhi);
                g.DrawString(sav_bindus[i - 1].ToString(), f, b, p.X, p.Y);
            }
        }

        private void PaintNormalView(Graphics g)
        {
            var nItems = new int[13]
            {
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            };
            var bItems = new Body.Body.Name[10]
            {
                Body.Body.Name.Lagna,
                Body.Body.Name.Sun,
                Body.Body.Name.Moon,
                Body.Body.Name.Mars,
                Body.Body.Name.Mercury,
                Body.Body.Name.Jupiter,
                Body.Body.Name.Venus,
                Body.Body.Name.Saturn,
                Body.Body.Name.Rahu,
                Body.Body.Name.Ketu
            };

        #if DDD
			foreach (ZodiacHouse.Name _zh in ZodiacHouse.AllNames)
			{
				ZodiacHouse zh = new ZodiacHouse(_zh);
				for (int i = 1; i<9; i++)
				{
					DivisionPosition dp = new DivisionPosition(Body.Name.Jupiter,
						BodyType.Name.Graha, zh, 0.0, 0.0);
					AddItem (g, zh, i, dp, true);
				}
				for (int i = 1; i<=6; i++)
				{
					DivisionPosition dp = new DivisionPosition(Body.Name.A11, 
											BodyType.Name.BhavaArudha, zh, 0.0, 0.0);
					AddItem (g, zh, i, dp, false);
				}

			}
        #endif

            foreach (DivisionPosition dp in div_pos)
            {
                if (options.ViewStyle == UserOptions.EViewStyle.Panchanga &&
                    dp.type           != BodyType.Name.Graha)
                {
                    continue;
                }

                if (dp.type != BodyType.Name.Graha && dp.type != BodyType.Name.Lagna)
                {
                    continue;
                }

                var zh = dp.zodiac_house;
                var i  = (int)zh.value;
                nItems[i]++;
                AddItem(g, zh, nItems[i], dp, true);
            }


            if (options.ViewStyle == UserOptions.EViewStyle.Panchanga)
            {
                return;
            }

            foreach (DivisionPosition dp in div_pos)
            {
                if (dp.type != BodyType.Name.SpecialLagna)
                {
                    continue;
                }

                var zh = dp.zodiac_house;
                var i  = (int)zh.value;
                nItems[i]++;
                AddItem(g, zh, nItems[i], dp, true);
            }

            for (var k = 1; k < 13; k++)
            {
                nItems[k] = 0;
            }

            ArrayList secondary_pos = null;

            if (options.ViewStyle == UserOptions.EViewStyle.Normal)
            {
                secondary_pos = arudha_pos;
            }
            else
            {
                secondary_pos = varnada_pos;
            }

            foreach (DivisionPosition dp in secondary_pos)
            {
                var zh = dp.zodiac_house;
                var i  = (int)zh.value;
                nItems[i]++;
                AddItem(g, zh, nItems[i], dp, false);
            }
        }

        private void DivisionalChart_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            DrawChart(g);
        }

        public void DrawChart(Graphics g)
        {
            DrawChart(g, Width, Height);
        }

        public void DrawChart(Graphics g, int width, int height)
        {
            DrawChart(g, width, height, false);
        }

        public void DrawChart(Graphics g, int width, int height, bool bDrawInner)
        {
            var f = MhoraGlobalOptions.Instance.VargaFont;
            //this.BackColor = System.Drawing.Color.White;
            if (width == 0 || height == 0)
            {
                return;
            }


            var xw = dc.GetLength();
            var yw = dc.GetLength();

            var off = 5;

            if (bInnerMode)
            {
                off = 0;
            }

            var m_wh    = Math.Min(height, width) - 2 * off - off;
            var scale_x = ((float)width  - 2 * off) / xw;
            var scale_y = ((float)height - 2 * off) / yw;
            var scale   = m_wh                      / (float)xw;

            if (false == PrintMode && false == bDrawInner)
            {
                g.Clear(BackColor);
            }

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TranslateTransform(off, off);
            if (MhoraGlobalOptions.Instance.VargaChartIsSquare)
            {
                scale_x = scale;
                scale_y = scale;
            }

            g.ScaleTransform(scale_x, scale_y);

            if (false == PrintMode)
            {
                Brush bg = new SolidBrush(MhoraGlobalOptions.Instance.VargaBackgroundColor);
                g.FillRectangle(bg, 0, 0, xw, yw);
            }

            dc.DrawOutline(g);
            if (innerControl != null)
            {
                var ptInner = dc.GetInnerSquareOffset();
                var length  = dc.GetLength() - ptInner.X * 2;
                innerControl.Left   = (int)(ptInner.X    * scale_x) + off;
                innerControl.Top    = (int)(ptInner.X    * scale_y) + off;
                innerControl.Width  = (int)(length * scale_x);
                innerControl.Height = (int)(length * scale_y);
                innerControl.Anchor = AnchorStyles.Left;
            }

            PaintSAV(g);

            PaintObjects(g);

            var s_dtype = Basics.numPartsInDivisionString(options.Varga);
            //string.Format("D-{0}", Basics.numPartsInDivision(options.Varga));
            var hint = g.MeasureString(s_dtype, f);
            g.DrawString(s_dtype, f, Brushes.Black, xw * 2 / 4 - hint.Width / 2, yw * 2 / 4 - hint.Height / 2);

            s_dtype = Basics.variationNameOfDivision(options.Varga);
            hint    = g.MeasureString(s_dtype, f);
            g.DrawString(s_dtype, f, Brushes.Black, xw * 2 / 4 - hint.Width / 2, yw * 2 / 4 - f.Height - hint.Height / 2);

            if (options.ChartStyle == UserOptions.EChartStyle.SouthIndian &&
                MhoraGlobalOptions.Instance.VargaShowDob                  &&
                false == PrintMode                                        &&
                false == bDrawInner)
            {
                var tob = h.info.tob.ToString();
                hint = g.MeasureString(tob, f);
                g.DrawString(tob, f, Brushes.Black, xw * 2 / 4 - hint.Width / 2, (float)(yw * 2 / 4 - hint.Height / 2 + f.Height * 1.5));

                var latlon = h.info.lat + " " + h.info.lon;
                hint = g.MeasureString(latlon, f);
                g.DrawString(latlon, f, Brushes.Black, xw * 2 / 4 - hint.Width / 2, (float)(yw * 2 / 4 - hint.Height / 2 + f.Height * 2.5));

                hint = g.MeasureString(h.info.name, f);
                g.DrawString(h.info.name, f, Brushes.Black, xw * 2 / 4 - hint.Width / 2, (float)(yw * 2 / 4 - hint.Height / 2 - f.Height * 1.5));
            }


            /*
			ZodiacHouse zh = new ZodiacHouse(ZodiacHouse.Name.Sco);
			for (int i=1; i<9; i++)
				AddItem(g, zh, i, new D, true);

			for (int i=1; i<=12; i++) 
			{
				ZodiacHouse zh = new ZodiacHouse((ZodiacHouse.Name)i);
				AddItem (g, zh, 9, zh.value.ToString());
			}
			*/
            Update();
        }

        private void DivisionalChart_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void SetChartStyle(UserOptions.EChartStyle cs)
        {
            switch (cs)
            {
                case UserOptions.EChartStyle.EastIndian:
                    dc = new EastIndianChart();
                    return;
                case UserOptions.EChartStyle.SouthIndian:
                default:
                    dc = new SouthIndianChart();
                    return;
            }
        }

        public object SetOptions(object o)
        {
            var uo = (UserOptions)o;
            if (uo.ChartStyle != options.ChartStyle)
            {
                SetChartStyle(uo.ChartStyle);
            }

            options = uo;
            OnRecalculate(h);

            return options.Clone();
        }

        private void mOptions_Click(object sender, EventArgs e)
        {
            Form f = new MhoraOptions(options.Clone(), SetOptions);
            f.ShowDialog();
        }

        private void CalculateBindus()
        {
            if (MhoraGlobalOptions.Instance.VargaShowSAVVarga)
            {
                sav_bindus = new Ashtakavarga(h, options.Varga).getSav();
            }
            else if (MhoraGlobalOptions.Instance.VargaShowSAVRasi)
            {
                sav_bindus = new Ashtakavarga(h, new Division(Basics.DivisionType.Rasi)).getSav();
            }
        }

        private void OnRedisplay(object o)
        {
            SetChartStyle(MhoraGlobalOptions.Instance.VargaStyle);
            options.ChartStyle = MhoraGlobalOptions.Instance.VargaStyle;
            fBase = new Font(MhoraGlobalOptions.Instance.VargaFont.FontFamily,
                             MhoraGlobalOptions.Instance.VargaFont.SizeInPoints);
            CalculateBindus();
            Invalidate();
        }

        private void OnRecalculate(object o)
        {
            div_pos          = h.CalculateDivisionPositions(options.Varga);
            arudha_pos       = h.CalculateArudhaDivisionPositions(options.Varga);
            varnada_pos      = h.CalculateVarnadaDivisionPositions(options.Varga);
            graha_arudha_pos = h.CalculateGrahaArudhaDivisionPositions(options.Varga);
            CalculateBindus();
            Invalidate();
        }

        private void DivisionalChart_DragDrop(object sender, DragEventArgs e)
        {
            if (options.ShowInner &&
                e.Data.GetDataPresent(typeof(DivisionalChart)))
            {
                var div = Division.CopyFromClipboard();
                if (null == div)
                {
                    return;
                }

                if (innerControl == null)
                {
                    AddInnerControl();
                }

                innerControl.options.Varga = div;
                innerControl.OnRecalculate(innerControl.h);
                Invalidate();
            }


        #if DND
			if (MhoraGlobalOptions.Reference is DivisionalChart) 
			{	
				DivisionalChart dc = (DivisionalChart)MhoraGlobalOptions.Reference;
				h.Changed -= new EvtChanged(OnRecalculate);
				dc.ControlHoroscope.Changed += new EvtChanged(OnRecalculate);
				h = dc.ControlHoroscope;
				DivisionalChart.UserOptions uo = (UserOptions)this.options.Clone();
				uo.Division = dc.options.Division;
				this.SetOptions(uo);
				this.OnRecalculate(h);
			}
        #endif
        }

        private void DivisionalChart_DragEnter(object sender, DragEventArgs e)
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

        private void mRasi_Click(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.Rasi);
            OnRecalculate(h);
            Invalidate();
        }

        private void mNavamsa_Click(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.Navamsa);
            OnRecalculate(h);
            Invalidate();
        }

        private void mBhava_Click(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.BhavaPada);
            OnRecalculate(h);
            Invalidate();
        }

        private void mBhavaEqual_Click(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.BhavaEqual);
            OnRecalculate(h);
            Invalidate();
        }

        private void mBhavaSripati_Click(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.BhavaSripati);
            OnRecalculate(h);
            Invalidate();
        }

        private void mBhavaKoch_Click(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.BhavaKoch);
            OnRecalculate(h);
            Invalidate();
        }

        private void mBhavaPlacidus_Click(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.BhavaPlacidus);
            OnRecalculate(h);
            Invalidate();
        }

        private void menuBhavaAlcabitus_Click(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.BhavaAlcabitus);
            OnRecalculate(h);
            Invalidate();
        }

        private void menuBhavaCampanus_Click(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.BhavaCampanus);
            OnRecalculate(h);
            Invalidate();
        }

        private void menuBhavaRegiomontanus_Click(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.BhavaRegiomontanus);
            OnRecalculate(h);
            Invalidate();
        }

        private void menuBhavaAxial_Click(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.BhavaAxial);
            OnRecalculate(h);
            Invalidate();
        }

        private void mDrekkanaParasara_Click(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.DrekkanaParasara);
            OnRecalculate(h);
            Invalidate();
        }

        private void mChaturamsa_Click(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.Chaturthamsa);
            OnRecalculate(h);
            Invalidate();
        }

        private void mPanchamsa_Click(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.Panchamsa);
            OnRecalculate(h);
            Invalidate();
        }

        private void mShashtamsa_Click(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.Shashthamsa);
            OnRecalculate(h);
            Invalidate();
        }

        private void mSaptamsa_Click(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.Saptamsa);
            OnRecalculate(h);
            Invalidate();
        }

        private void mAshtamsa_Click(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.Ashtamsa);
            OnRecalculate(h);
            Invalidate();
        }

        private void mAshtamsaRaman_Click(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.AshtamsaRaman);
            OnRecalculate(h);
            Invalidate();
        }

        private void mDasamsa_Click(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.Dasamsa);
            OnRecalculate(h);
            Invalidate();
        }

        private void mDwadasamsa_Click(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.Dwadasamsa);
            OnRecalculate(h);
            Invalidate();
        }

        private void mShodasamsa_Click(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.Shodasamsa);
            OnRecalculate(h);
            Invalidate();
        }

        private void mVimsamsa_Click(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.Vimsamsa);
            OnRecalculate(h);
            Invalidate();
        }

        private void mChaturvimsamsa_Click(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.Chaturvimsamsa);
            OnRecalculate(h);
            Invalidate();
        }

        private void mNakshatramsa_Click(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.Nakshatramsa);
            OnRecalculate(h);
            Invalidate();
        }

        private void mTrimsamsa_Click(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.Trimsamsa);
            OnRecalculate(h);
            Invalidate();
        }

        private void mKhavedamsa_Click(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.Khavedamsa);
            OnRecalculate(h);
            Invalidate();
        }

        private void mDrekkanaJagannath_Click(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.DrekkanaJagannath);
            OnRecalculate(h);
            Invalidate();
        }

        private void mDrekkanaSomnath_Click(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.DrekkanaSomnath);
            OnRecalculate(h);
            Invalidate();
        }

        private void mDrekkanaParivrittitraya_Click(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.DrekkanaParivrittitraya);
            OnRecalculate(h);
            Invalidate();
        }

        private void mHoraKashinath_Click(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.HoraKashinath);
            OnRecalculate(h);
            Invalidate();
        }

        private void mHoraParivritti_Click(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.HoraParivrittiDwaya);
            OnRecalculate(h);
            Invalidate();
        }

        private void mHoraParasara_Click(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.HoraParasara);
            OnRecalculate(h);
            Invalidate();
        }

        private void mHoraJagannath_Click(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.HoraJagannath);
            OnRecalculate(h);
            Invalidate();
        }

        private void mTrimsamsaParivritti_Click(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.TrimsamsaParivritti);
            OnRecalculate(h);
            Invalidate();
        }

        private void mTrimsamsaSimple_Click(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.TrimsamsaSimple);
            OnRecalculate(h);
            Invalidate();
        }


        private void mLagnaChange_Click(object sender, EventArgs e)
        {
            var vf = new VargaRectificationForm(h, this, options.Varga);
            vf.Show();
        }

        private void mExtrapolate_Click(object sender, EventArgs e)
        {
            foreach (Position bp in h.positionList)
            {
                var dp      = bp.toDivisionPosition(options.Varga);
                var lLower  = new Longitude(dp.cusp_lower);
                var lOffset = bp.longitude.sub(lLower);
                var lRange  = new Longitude(dp.cusp_higher).sub(lLower);
                Trace.Assert(lOffset.value <= lRange.value, "Extrapolation internal error: Slice smaller than range. Weird.");

                var newOffset = lOffset.value / lRange.value     * 30.0;
                var newBase   = ((int)dp.zodiac_house.value - 1) * 30.0;
                bp.longitude = new Longitude(newOffset + newBase);
            }

            h.OnlySignalChanged();
        }

        private void mAkshavedamsa_Click_1(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.Akshavedamsa);
            OnRecalculate(h);
            Invalidate();
        }

        private void mShashtyamsa_Click_1(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.Shashtyamsa);
            OnRecalculate(h);
            Invalidate();
        }

        private void mRudramsaRath_Click(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.Rudramsa);
            OnRecalculate(h);
            Invalidate();
        }

        private void mRudramsaRaman_Click(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.RudramsaRaman);
            OnRecalculate(h);
            Invalidate();
        }

        private void mNadiamsa_Click(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.Nadiamsa);
            OnRecalculate(h);
            Invalidate();
        }

        private void mNadiamsaCKN_Click(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.NadiamsaCKN);
            OnRecalculate(h);
            Invalidate();
        }

        private void mNavamsaDwadasamsa_Click(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.NavamsaDwadasamsa);
            OnRecalculate(h);
            Invalidate();
        }

        private void mDwadasamsaDwadasamsa_Click(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.DwadasamsaDwadasamsa);
            OnRecalculate(h);
            Invalidate();
        }

        private void menuItem7_Click(object sender, EventArgs e)
        {
            options.Varga = new Division(Basics.DivisionType.Ashtottaramsa);
            OnRecalculate(h);
            Invalidate();
        }

        private void mRegularParivritti_Click(object sender, EventArgs e)
        {
            var single = new Division.SingleDivision(Basics.DivisionType.GenericParivritti,
                                                     Basics.numPartsInDivision(options.Varga));
            options.Varga = new Division(single);

            OnRecalculate(h);
            Invalidate();
        }

        private void mRegularFromHouse_Click(object sender, EventArgs e)
        {
            var single = new Division.SingleDivision(Basics.DivisionType.GenericDwadasamsa,
                                                     Basics.numPartsInDivision(options.Varga));
            options.Varga = new Division(single);

            OnRecalculate(h);
            Invalidate();
        }


        private void mRegularTrikona_Click(object sender, EventArgs e)
        {
            var single = new Division.SingleDivision(Basics.DivisionType.GenericDrekkana,
                                                     Basics.numPartsInDivision(options.Varga));
            options.Varga = new Division(single);
            OnRecalculate(h);
            Invalidate();
        }

        protected override void copyToClipboard()
        {
            var displayGraphics = CreateGraphics();
            var size            = Math.Min(Width, Height);
            var bmpBuffer       = new Bitmap(size, size, displayGraphics);
            var imageGraphics   = Graphics.FromImage(bmpBuffer);
            DrawChart(imageGraphics);
            displayGraphics.Dispose();
            Clipboard.SetDataObject(bmpBuffer, true);
            imageGraphics.Dispose();
        }

        private void DivisionalChart_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DoDragDrop(this, DragDropEffects.Copy);
                Clipboard.SetDataObject(string.Empty);
            }
            else
            {
                contextMenu.Show(this, new Point(e.X, e.Y));
                ;
            }
        }

        private void mViewNormal_Click(object sender, EventArgs e)
        {
            options.ViewStyle = UserOptions.EViewStyle.Normal;
            Invalidate();
        }

        private void mViewDualGrahaArudha_Click(object sender, EventArgs e)
        {
            options.ViewStyle = UserOptions.EViewStyle.DualGrahaArudha;
            Invalidate();
        }

        private void mViewCharaKarakas_Click(object sender, EventArgs e)
        {
            options.ViewStyle = UserOptions.EViewStyle.CharaKarakas8;
            Invalidate();
        }

        private void mViewCharaKarakas7_Click(object sender, EventArgs e)
        {
            options.ViewStyle = UserOptions.EViewStyle.CharaKarakas7;
            Invalidate();
        }

        private void mViewVarnada_Click(object sender, EventArgs e)
        {
            options.ViewStyle = UserOptions.EViewStyle.Varnada;
            Invalidate();
        }

        private void mRegularKendraChaturthamsa_Click(object sender, EventArgs e)
        {
            var single = new Division.SingleDivision(Basics.DivisionType.GenericChaturthamsa,
                                                     Basics.numPartsInDivision(options.Varga));
            options.Varga = new Division(single);
            OnRecalculate(h);
            Invalidate();
        }

        private void mRegularSaptamsaBased_Click(object sender, EventArgs e)
        {
            var single = new Division.SingleDivision(Basics.DivisionType.GenericSaptamsa,
                                                     Basics.numPartsInDivision(options.Varga));
            options.Varga = new Division(single);
            OnRecalculate(h);
            Invalidate();
        }

        private void mRegularDasamsaBased_Click(object sender, EventArgs e)
        {
            var single = new Division.SingleDivision(Basics.DivisionType.GenericDasamsa,
                                                     Basics.numPartsInDivision(options.Varga));
            options.Varga = new Division(single);
            OnRecalculate(h);
            Invalidate();
        }

        private void mRegularShashthamsaBased_Click(object sender, EventArgs e)
        {
            var single = new Division.SingleDivision(Basics.DivisionType.GenericShashthamsa,
                                                     Basics.numPartsInDivision(options.Varga));
            options.Varga = new Division(single);
            OnRecalculate(h);
            Invalidate();
        }

        private void mRegularShodasamsaBased_Click(object sender, EventArgs e)
        {
            var single = new Division.SingleDivision(Basics.DivisionType.GenericShodasamsa,
                                                     Basics.numPartsInDivision(options.Varga));
            options.Varga = new Division(single);
            OnRecalculate(h);
            Invalidate();
        }

        private void mRegularVimsamsaBased_Click(object sender, EventArgs e)
        {
            var single = new Division.SingleDivision(Basics.DivisionType.GenericVimsamsa,
                                                     Basics.numPartsInDivision(options.Varga));
            options.Varga = new Division(single);
            OnRecalculate(h);
            Invalidate();
        }

        private void mRegularNakshatramsaBased_Click(object sender, EventArgs e)
        {
            var single = new Division.SingleDivision(Basics.DivisionType.GenericNakshatramsa,
                                                     Basics.numPartsInDivision(options.Varga));
            options.Varga = new Division(single);
            OnRecalculate(h);
            Invalidate();
        }

        private void menuItem12_Click(object sender, EventArgs e)
        {
            var single = new Division.SingleDivision(Basics.DivisionType.GenericChaturvimsamsa,
                                                     Basics.numPartsInDivision(options.Varga));
            options.Varga = new Division(single);
            OnRecalculate(h);
            Invalidate();
        }

        private void DivisionalChart_MouseLeave(object sender, EventArgs e)
        {
            //if (e
            //Division.CopyToClipboard(this.options.Varga);
            //this.DoDragDrop(this, DragDropEffects.Copy);
        }

        private void DivisionalChart_DragLeave(object sender, EventArgs e)
        {
            Division.CopyToClipboard(options.Varga);
        }

        public class UserOptions : ICloneable
        {
            [TypeConverter(typeof(EnumDescConverter))]
            public enum EChartStyle
            {
                [Description("South Indian Square (Jupiter)")]
                SouthIndian,

                [Description("East Indian Square (Sun)")]
                EastIndian
            }

            [TypeConverter(typeof(EnumDescConverter))]
            public enum EViewStyle
            {
                [Description("Regular")]
                Normal,

                [Description("Dual Graha Arudhas")]
                DualGrahaArudha,

                [Description("8 Chara Karakas (regular)")]
                CharaKarakas8,

                [Description("7 Chara Karakas (mundane)")]
                CharaKarakas7,

                [Description("Varnada Lagna")]
                Varnada,

                [Description("Panchanga Print View")]
                Panchanga
            }

            private Division innerVarga;

            public UserOptions()
            {
                Varga      = new Division(Basics.DivisionType.Rasi);
                ViewStyle  = EViewStyle.Normal;
                ChartStyle = MhoraGlobalOptions.Instance.VargaStyle;
                Varga      = new Division(Basics.DivisionType.Rasi);
                innerVarga = new Division(Basics.DivisionType.Rasi);
                ShowInner  = false;
            }

            [Category("Options")]
            [PGDisplayName("Varga")]
            public Division Varga
            {
                get;
                set;
            }


            [PGDisplayName("Dual Chart View")]
            public bool ShowInner
            {
                get;
                set;
            }

            [PGDisplayName("View Type")]
            public EViewStyle ViewStyle
            {
                get;
                set;
            }

            [Category("Options")]
            [PGDisplayName("Chart style")]
            public EChartStyle ChartStyle
            {
                get;
                set;
            }

            public object Clone()
            {
                var uo = new UserOptions();
                uo.Varga      = Varga;
                uo.ChartStyle = ChartStyle;
                uo.ViewStyle  = ViewStyle;
                uo.ShowInner  = ShowInner;
                return uo;
            }

            public object Copy(object o)
            {
                var uo = (UserOptions)o;
                ChartStyle = uo.ChartStyle;
                ViewStyle  = uo.ViewStyle;
                Varga      = uo.Varga;
                ShowInner  = uo.ShowInner;
                return uo;
            }
        }
    }
}