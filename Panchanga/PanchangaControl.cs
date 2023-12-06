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
using System.Threading;
using System.Windows.Forms;
using Mhora.Body;
using Mhora.Calculation;
using Mhora.Components;
using Mhora.Hora;
using Mhora.Settings;
using Mhora.SwissEph;
using Mhora.Varga;

namespace Mhora.Panchanga
{
    public class PanchangaControl : MhoraControl
    {
        private Button bCompute;
        private Button bOpts;

        private          bool                   bResultsInvalid = true;
        private readonly IContainer             components      = null;
        private          ContextMenu            contextMenu;
        private          PanchangaGlobalMoments globals = new PanchangaGlobalMoments();

        private readonly int[] gulika_kalas =
        {
            6,
            5,
            4,
            3,
            2,
            1,
            0
        };


        private ArrayList               locals = new ArrayList();
        public  DelegateComputeFinished m_DelegateComputeFinished;
        private MenuItem                menuItem1;
        private MenuItem                menuItem2;
        private MenuItem                menuItemFilePrintPreview;
        private MenuItem                menuItemPrintPanchanga;


        private ListView mList;

        //ProgressDialog fProgress = null;
        private          Mutex       mutexProgress;
        private readonly UserOptions opts;

        private readonly int[] rahu_kalas =
        {
            7,
            1,
            6,
            4,
            5,
            3,
            2
        };

        private double sunrise;
        private double ut_sr;

        private readonly int[] yama_kalas =
        {
            4,
            3,
            2,
            1,
            0,
            6,
            5
        };


        public PanchangaControl(Horoscope _h)
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();
            h                                      =  _h;
            h.Changed                              += OnRecalculate;
            MhoraGlobalOptions.DisplayPrefsChanged += OnRedisplay;
            opts                                   =  new UserOptions();
            AddViewsToContextMenu(contextMenu);
            mutexProgress = new Mutex(false);
            OnRedisplay(MhoraGlobalOptions.Instance.TableBackgroundColor);
            bCompute_Click(null, null);
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

#region Designer generated code

        /// <summary>
        ///     Required method for Designer support - do not modify
        ///     the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mList                    = new System.Windows.Forms.ListView();
            this.bOpts                    = new System.Windows.Forms.Button();
            this.bCompute                 = new System.Windows.Forms.Button();
            this.contextMenu              = new System.Windows.Forms.ContextMenu();
            this.menuItemPrintPanchanga   = new System.Windows.Forms.MenuItem();
            this.menuItemFilePrintPreview = new System.Windows.Forms.MenuItem();
            this.menuItem1                = new System.Windows.Forms.MenuItem();
            this.menuItem2                = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // mList
            // 
            this.mList.Anchor               =  ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.mList.FullRowSelect        =  true;
            this.mList.Location             =  new System.Drawing.Point(8, 40);
            this.mList.Name                 =  "mList";
            this.mList.Size                 =  new System.Drawing.Size(512, 272);
            this.mList.TabIndex             =  0;
            this.mList.View                 =  System.Windows.Forms.View.Details;
            this.mList.SelectedIndexChanged += new System.EventHandler(this.mList_SelectedIndexChanged);
            // 
            // bOpts
            // 
            this.bOpts.Location =  new System.Drawing.Point(16, 8);
            this.bOpts.Name     =  "bOpts";
            this.bOpts.TabIndex =  1;
            this.bOpts.Text     =  "Options";
            this.bOpts.Click    += new System.EventHandler(this.bOpts_Click);
            // 
            // bCompute
            // 
            this.bCompute.Location =  new System.Drawing.Point(104, 8);
            this.bCompute.Name     =  "bCompute";
            this.bCompute.TabIndex =  2;
            this.bCompute.Text     =  "Compute";
            this.bCompute.Click    += new System.EventHandler(this.bCompute_Click);
            // 
            // contextMenu
            // 
            this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[]
            {
                this.menuItemPrintPanchanga,
                this.menuItemFilePrintPreview,
                this.menuItem1,
                this.menuItem2
            });
            // 
            // menuItemPrintPanchanga
            // 
            this.menuItemPrintPanchanga.Index =  0;
            this.menuItemPrintPanchanga.Text  =  "Print Panchanga";
            this.menuItemPrintPanchanga.Click += new System.EventHandler(this.menuItemPrintPanchanga_Click);
            // 
            // menuItemFilePrintPreview
            // 
            this.menuItemFilePrintPreview.Index =  1;
            this.menuItemFilePrintPreview.Text  =  "Print Preview Panchanga";
            this.menuItemFilePrintPreview.Click += new System.EventHandler(this.menuItemFilePrintPreview_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 2;
            this.menuItem1.Text  = "-";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 3;
            this.menuItem2.Text  = "-";
            // 
            // PanchangaControl
            // 
            this.ContextMenu = this.contextMenu;
            this.Controls.Add(this.bCompute);
            this.Controls.Add(this.bOpts);
            this.Controls.Add(this.mList);
            this.Name =  "PanchangaControl";
            this.Size =  new System.Drawing.Size(528, 320);
            this.Load += new System.EventHandler(this.PanchangaControl_Load);
            this.ResumeLayout(false);
        }

#endregion

        private void PanchangaControl_Load(object sender, EventArgs e)
        {
        }

        public void OnRedisplay(object o)
        {
            mList.ForeColor = MhoraGlobalOptions.Instance.TableForegroundColor;
            mList.BackColor = MhoraGlobalOptions.Instance.TableBackgroundColor;
            mList.Font      = MhoraGlobalOptions.Instance.GeneralFont;
        }

        public void OnRecalculate(object _h)
        {
            if (bResultsInvalid)
            {
                return;
            }

            var h = (Horoscope)_h;

            var li = new ListViewItem();
            li.Text = "Results may be out of date. Click the Compute Button to recalculate the panchanga";
            mList.Items.Insert(0, li);
            mList.Items.Insert(1, string.Empty);
            bResultsInvalid = true;
        }

        public object SetOptions(object o)
        {
            return opts.CopyFrom(o);
        }

        private void bOpts_Click(object sender, EventArgs e)
        {
            //this.mutexProgress.WaitOne();
            //if (this.fProgress != null)
            //{
            //	MessageBox.Show("Cannot show options when calculation is in progress");
            //	this.mutexProgress.Close();
            //	return;
            //}
            new MhoraOptions(opts, SetOptions).ShowDialog();
            //this.mutexProgress.Close();
        }

        private void bCompute_Click(object sender, EventArgs e)
        {
            m_DelegateComputeFinished = ComputeFinished;
            var t = new Thread(ComputeStart);
            t.Start();
        }

        private void ComputeStart()
        {
            //this.mutexProgress.WaitOne();
            //if (fProgress != null)
            //{
            //	this.mutexProgress.Close();
            //	return;
            //}
            //fProgress = new ProgressDialog(opts.NumDays);
            //fProgress.setProgress(opts.NumDays/2);
            mhora.Log.Debug("Starting threaded computation");
            //fProgress.ShowDialog();
            //this.mutexProgress.Close();
            bCompute.Enabled = false;
            bOpts.Enabled    = false;
            ContextMenu      = null;
            Invoke((Action) ComputeEntries);
            Invoke(m_DelegateComputeFinished);
        }

        private void ComputeFinished()
        {
            mhora.Log.Debug("Thread finished execution");
            bResultsInvalid  = false;
            bCompute.Enabled = true;
            bOpts.Enabled    = true;
            ContextMenu      = contextMenu;
            //this.m_DelegateComputeFinished -= new DelegateComputeFinished(this.ComputeFinished);
            //this.mutexProgress.WaitOne();
            //fProgress.Close();
            //fProgress = null;
            //this.mutexProgress.Close();
        }


        private void ComputeEntries()
        {
            mList.Clear();
            mList.Columns.Add(string.Empty, -2, HorizontalAlignment.Left);

            if (false == opts.ShowUpdates)
            {
                mList.BeginUpdate();
            }

            var ut_start = Math.Floor(h.baseUT);
            double[] geopos =
            {
                h.info.lon.toDouble(),
                h.info.lat.toDouble(),
                h.info.alt
            };

            globals = new PanchangaGlobalMoments();
            locals  = new ArrayList();

            for (var i = 0; i < opts.NumDays; i++)
            {
                ComputeEntry(ut_start, geopos);
                ut_start               += 1;
                mList.Columns[0].Width =  -2;
            }

            mList.Columns[0].Width = -2;

            if (false == opts.ShowUpdates)
            {
                mList.EndUpdate();
            }
        }

        private Moment utToMoment(double found_ut)
        {
            // turn into horoscope
            int year  = 0,
                month = 0,
                day   = 0;
            double hour = 0;
            found_ut += h.info.tz.toDouble() / 24.0;
            sweph.RevJul(found_ut, ref year, ref month, ref day, ref hour);
            var m = new Moment(year, month, day, hour);
            return m;
        }

        private string utToString(double ut)
        {
            int year  = 0,
                month = 0,
                day   = 0;
            double time = 0;

            ut += h.info.tz.toDouble() / 24.0;
            sweph.RevJul(ut, ref year, ref month, ref day, ref time);
            return timeToString(time);
        }

        private string utTimeToString(double ut_event, double ut_sr, double sunrise)
        {
            var m   = utToMoment(ut_event);
            var hms = new HMSInfo(m.time);

            if (ut_event >= ut_sr - sunrise / 24.0 + 1.0)
            {
                if (false == opts.LargeHours)
                {
                    return string.Format("*{0:00}:{1:00}", hms.degree, hms.minute);
                }

                return string.Format("{0:00}:{1:00}", hms.degree + 24, hms.minute);
            }

            return string.Format("{0:00}:{1:00}", hms.degree, hms.minute);
        }

        private string timeToString(double time)
        {
            var hms = new HMSInfo(time);
            return string.Format("{0:00}:{1:00}",
                                 hms.degree,
                                 hms.minute,
                                 hms.second);
        }

        private void ComputeEntry(double ut, double[] geopos)
        {
            int year  = 0,
                month = 0,
                day   = 0;
            double sunset = 0,
                   hour   = 0;
            sweph.obtainLock(h);
            h.populateSunrisetCacheHelper(ut - 0.5, ref sunrise, ref sunset, ref ut_sr);
            sweph.releaseLock(h);

            sweph.RevJul(ut_sr, ref year, ref month, ref day, ref hour);
            var moment_sr = new Moment(year, month, day, hour);
            var moment_ut = new Moment(ut, h);
            var infoCurr  = new HoraInfo(moment_ut, h.info.lat, h.info.lon, h.info.tz);
            var hCurr     = new Horoscope(infoCurr, h.options);

            ListViewItem li = null;

            var local = new PanchangaLocalMoments();
            local.sunrise    = hCurr.sunrise;
            local.sunset     = sunset;
            local.sunrise_ut = ut_sr;
            sweph.RevJul(ut, ref year, ref month, ref day, ref hour);
            local.wday = (Basics.Weekday)sweph.DayOfWeek(ut);


            local.kalas_ut = hCurr.getKalaCuspsUt();
            if (opts.CalcSpecialKalas)
            {
                var bStart = Basics.weekdayRuler(hCurr.wday);
                if (hCurr.options.KalaType == HoroscopeOptions.EHoraType.Lmt)
                {
                    bStart = Basics.weekdayRuler(hCurr.lmt_wday);
                }

                local.rahu_kala_index   = rahu_kalas[(int)bStart];
                local.gulika_kala_index = gulika_kalas[(int)bStart];
                local.yama_kala_index   = yama_kalas[(int)bStart];
            }

            if (opts.CalcLagnaCusps)
            {
                li = new ListViewItem();
                sweph.obtainLock(h);
                var bp_lagna_sr = Basics.CalculateSingleBodyPosition(ut_sr, sweph.BodyNameToSweph(Body.Body.Name.Lagna), Body.Body.Name.Lagna, BodyType.Name.Lagna, h);
                var dp_lagna_sr = bp_lagna_sr.toDivisionPosition(new Division(Basics.DivisionType.Rasi));
                local.lagna_zh = dp_lagna_sr.zodiac_house.value;

                var bp_lagna_base = new Longitude(bp_lagna_sr.longitude.toZodiacHouseBase());
                var ut_transit    = ut_sr;
                for (var i = 1; i <= 12; i++)
                {
                    var r = new Retrogression(h, Body.Body.Name.Lagna);
                    ut_transit = r.GetLagnaTransitForward(ut_transit, bp_lagna_base.add(i * 30.0));

                    var pmi = new PanchangaMomentInfo(ut_transit, (int)bp_lagna_sr.longitude.toZodiacHouse().add(i + 1).value);
                    local.lagnas_ut.Add(pmi);
                }

                sweph.releaseLock(h);
            }

            if (opts.CalcTithiCusps)
            {
                var t = new Transit(h);
                sweph.obtainLock(h);
                var tithi_start = t.LongitudeOfTithi(ut_sr).toTithi();
                var tithi_end   = t.LongitudeOfTithi(ut_sr + 1.0).toTithi();

                var tithi_curr = tithi_start.add(1);
                local.tithi_index_start = globals.tithis_ut.Count - 1;
                local.tithi_index_end   = globals.tithis_ut.Count - 1;

                while (tithi_start.value != tithi_end.value &&
                       tithi_curr.value  != tithi_end.value)
                {
                    tithi_curr = tithi_curr.add(2);
                    var dLonToFind = ((double)(int)tithi_curr.value - 1) * (360.0 / 30.0);
                    var ut_found = t.LinearSearchBinary(ut_sr,
                                                        ut_sr + 1.0,
                                                        new Longitude(dLonToFind),
                                                        t.LongitudeOfTithiDir);

                    globals.tithis_ut.Add(new PanchangaMomentInfo(ut_found, (int)tithi_curr.value));
                    local.tithi_index_end++;
                }

                sweph.releaseLock(h);
            }


            if (opts.CalcKaranaCusps)
            {
                var t = new Transit(h);
                sweph.obtainLock(h);
                var karana_start = t.LongitudeOfTithi(ut_sr).toKarana();
                var karana_end   = t.LongitudeOfTithi(ut_sr + 1.0).toKarana();

                var karana_curr = karana_start.add(1);
                local.karana_index_start = globals.karanas_ut.Count - 1;
                local.karana_index_end   = globals.karanas_ut.Count - 1;

                while (karana_start.value != karana_end.value &&
                       karana_curr.value  != karana_end.value)
                {
                    karana_curr = karana_curr.add(2);
                    var dLonToFind = ((double)(int)karana_curr.value - 1) * (360.0 / 60.0);
                    var ut_found = t.LinearSearchBinary(ut_sr,
                                                        ut_sr + 1.0,
                                                        new Longitude(dLonToFind),
                                                        t.LongitudeOfTithiDir);

                    globals.karanas_ut.Add(new PanchangaMomentInfo(ut_found, (int)karana_curr.value));
                    local.karana_index_end++;
                }

                sweph.releaseLock(h);
            }

            if (opts.CalcSMYogaCusps)
            {
                var t = new Transit(h);
                sweph.obtainLock(h);
                var sm_start = t.LongitudeOfSunMoonYoga(ut_sr).toSunMoonYoga();
                var sm_end   = t.LongitudeOfSunMoonYoga(ut_sr + 1.0).toSunMoonYoga();

                var sm_curr = sm_start.add(1);
                local.smyoga_index_start = globals.smyogas_ut.Count - 1;
                local.smyoga_index_end   = globals.smyogas_ut.Count - 1;

                while (sm_start.value != sm_end.value &&
                       sm_curr.value  != sm_end.value)
                {
                    sm_curr = sm_curr.add(2);
                    var dLonToFind = ((double)(int)sm_curr.value - 1) * (360.0 / 27);
                    var ut_found = t.LinearSearchBinary(ut_sr,
                                                        ut_sr + 1.0,
                                                        new Longitude(dLonToFind),
                                                        t.LongitudeOfSunMoonYogaDir);

                    globals.smyogas_ut.Add(new PanchangaMomentInfo(ut_found, (int)sm_curr.value));
                    local.smyoga_index_end++;
                }

                sweph.releaseLock(h);
            }


            if (opts.CalcNakCusps)
            {
                var bDiscard = true;
                var t        = new Transit(h, Body.Body.Name.Moon);
                sweph.obtainLock(h);
                var nak_start = t.GenericLongitude(ut_sr, ref bDiscard).toNakshatra();
                var nak_end   = t.GenericLongitude(ut_sr + 1.0, ref bDiscard).toNakshatra();

                local.nakshatra_index_start = globals.nakshatras_ut.Count - 1;
                local.nakshatra_index_end   = globals.nakshatras_ut.Count - 1;

                var nak_curr = nak_start.add(1);

                while (nak_start.value != nak_end.value &&
                       nak_curr.value  != nak_end.value)
                {
                    nak_curr = nak_curr.add(2);
                    var dLonToFind = ((int)nak_curr.value - 1) * (360.0 / 27.0);
                    var ut_found = t.LinearSearchBinary(ut_sr,
                                                        ut_sr + 1.0,
                                                        new Longitude(dLonToFind),
                                                        t.GenericLongitude);

                    globals.nakshatras_ut.Add(new PanchangaMomentInfo(ut_found, (int)nak_curr.value));
                    mhora.Log.Debug("Found nakshatra {0}", nak_curr.value);
                    local.nakshatra_index_end++;
                }

                sweph.releaseLock(h);
            }

            if (opts.CalcHoraCusps)
            {
                local.horas_ut = hCurr.getHoraCuspsUt();
                hCurr.calculateHora(ut_sr + 1.0 / 24.0, ref local.hora_base);
            }

            if (opts.CalcKalaCusps)
            {
                hCurr.calculateKala(ref local.kala_base);
            }


            locals.Add(local);
            Invoke((Action) (() => DisplayEntry(local)));
        }

        private void DisplayEntry(PanchangaLocalMoments local)
        {
            string s;
            int day   = 0,
                month = 0,
                year  = 0;
            double time = 0;

            sweph.RevJul(local.sunrise_ut, ref year, ref month, ref day, ref time);
            var m = new Moment(year, month, day, time);
            mList.Items.Add(string.Format("{0}, {1}", local.wday, m.ToDateString()));

            if (opts.ShowSunriset)
            {
                s = string.Format("Sunrise at {0}. Sunset at {1}",
                                  timeToString(local.sunrise),
                                  timeToString(local.sunset));
                mList.Items.Add(s);
            }

            if (opts.CalcSpecialKalas)
            {
                var s_rahu = string.Format("Rahu Kala from {0} to {1}",
                                           new Moment(local.kalas_ut[local.rahu_kala_index], h).ToTimeString(),
                                           new Moment(local.kalas_ut[local.rahu_kala_index + 1], h).ToTimeString());
                var s_gulika = string.Format("Gulika Kala from {0} to {1}",
                                             new Moment(local.kalas_ut[local.gulika_kala_index], h).ToTimeString(),
                                             new Moment(local.kalas_ut[local.gulika_kala_index + 1], h).ToTimeString());
                var s_yama = string.Format("Yama Kala from {0} to {1}",
                                           new Moment(local.kalas_ut[local.yama_kala_index], h).ToTimeString(),
                                           new Moment(local.kalas_ut[local.yama_kala_index + 1], h).ToTimeString());

                if (opts.OneEntryPerLine)
                {
                    mList.Items.Add(s_rahu);
                    mList.Items.Add(s_gulika);
                    mList.Items.Add(s_yama);
                }
                else
                {
                    mList.Items.Add(string.Format("{0}. {1}. {2}.", s_rahu, s_gulika, s_yama));
                }
            }

            if (opts.CalcTithiCusps)
            {
                var s_tithi = string.Empty;

                if (local.tithi_index_start == local.tithi_index_end &&
                    local.tithi_index_start >= 0)
                {
                    var pmi = (PanchangaMomentInfo)globals.tithis_ut[local.tithi_index_start];
                    var t   = new Tithi((Tithi.Name)pmi.info);
                    mList.Items.Add(string.Format("{0} - full.", t.value));
                }
                else
                {
                    for (var i = local.tithi_index_start + 1; i <= local.tithi_index_end; i++)
                    {
                        if (i < 0)
                        {
                            continue;
                        }

                        var pmi = (PanchangaMomentInfo)globals.tithis_ut[i];
                        var t   = new Tithi((Tithi.Name)pmi.info).addReverse(2);
                        s_tithi += string.Format("{0} until {1}",
                                                 t.value,
                                                 utTimeToString(pmi.ut, local.sunrise_ut, local.sunrise));

                        if (opts.OneEntryPerLine)
                        {
                            mList.Items.Add(s_tithi);
                            s_tithi = string.Empty;
                        }
                        else
                        {
                            s_tithi += ". ";
                        }
                    }

                    if (false == opts.OneEntryPerLine)
                    {
                        mList.Items.Add(s_tithi);
                    }
                }
            }


            if (opts.CalcKaranaCusps)
            {
                var s_karana = string.Empty;

                if (local.karana_index_start == local.karana_index_end &&
                    local.karana_index_start >= 0)
                {
                    var pmi = (PanchangaMomentInfo)globals.karanas_ut[local.karana_index_start];
                    var k   = new Karana((Karana.Name)pmi.info);
                    mList.Items.Add(string.Format("{0} karana - full.", k.value));
                }
                else
                {
                    for (var i = local.karana_index_start + 1; i <= local.karana_index_end; i++)
                    {
                        if (i < 0)
                        {
                            continue;
                        }

                        var pmi = (PanchangaMomentInfo)globals.karanas_ut[i];
                        var k   = new Karana((Karana.Name)pmi.info).addReverse(2);
                        s_karana += string.Format("{0} karana until {1}",
                                                  k.value,
                                                  utTimeToString(pmi.ut, local.sunrise_ut, local.sunrise));

                        if (opts.OneEntryPerLine)
                        {
                            mList.Items.Add(s_karana);
                            s_karana = string.Empty;
                        }
                        else
                        {
                            s_karana += ". ";
                        }
                    }

                    if (false == opts.OneEntryPerLine)
                    {
                        mList.Items.Add(s_karana);
                    }
                }
            }


            if (opts.CalcSMYogaCusps)
            {
                var s_smyoga = string.Empty;

                if (local.smyoga_index_start == local.smyoga_index_end &&
                    local.smyoga_index_start >= 0)
                {
                    var pmi = (PanchangaMomentInfo)globals.smyogas_ut[local.smyoga_index_start];
                    var sm  = new SunMoonYoga((SunMoonYoga.Name)pmi.info);
                    mList.Items.Add(string.Format("{0} yoga - full.", sm.value));
                }
                else
                {
                    for (var i = local.smyoga_index_start + 1; i <= local.smyoga_index_end; i++)
                    {
                        if (i < 0)
                        {
                            continue;
                        }

                        var pmi = (PanchangaMomentInfo)globals.smyogas_ut[i];
                        var sm  = new SunMoonYoga((SunMoonYoga.Name)pmi.info).addReverse(2);
                        s_smyoga += string.Format("{0} yoga until {1}",
                                                  sm.value,
                                                  utTimeToString(pmi.ut, local.sunrise_ut, local.sunrise));

                        if (opts.OneEntryPerLine)
                        {
                            mList.Items.Add(s_smyoga);
                            s_smyoga = string.Empty;
                        }
                        else
                        {
                            s_smyoga += ". ";
                        }
                    }

                    if (false == opts.OneEntryPerLine)
                    {
                        mList.Items.Add(s_smyoga);
                    }
                }
            }


            if (opts.CalcNakCusps)
            {
                var s_nak = string.Empty;

                if (local.nakshatra_index_start == local.nakshatra_index_end &&
                    local.nakshatra_index_start >= 0)
                {
                    var pmi = (PanchangaMomentInfo)globals.nakshatras_ut[local.nakshatra_index_start];
                    var n   = new Nakshatra((Nakshatra.Name)pmi.info);
                    mList.Items.Add(string.Format("{0} - full.", n.value));
                }
                else
                {
                    for (var i = local.nakshatra_index_start + 1; i <= local.nakshatra_index_end; i++)
                    {
                        if (i < 0)
                        {
                            continue;
                        }

                        var pmi = (PanchangaMomentInfo)globals.nakshatras_ut[i];
                        var n   = new Nakshatra((Nakshatra.Name)pmi.info).addReverse(2);
                        s_nak += string.Format("{0} until {1}",
                                               n.value,
                                               utTimeToString(pmi.ut, local.sunrise_ut, local.sunrise));
                        if (opts.OneEntryPerLine)
                        {
                            mList.Items.Add(s_nak);
                            s_nak = string.Empty;
                        }
                        else
                        {
                            s_nak += ". ";
                        }
                    }

                    if (false == opts.OneEntryPerLine)
                    {
                        mList.Items.Add(s_nak);
                    }
                }
            }

            if (opts.CalcLagnaCusps)
            {
                var sLagna = "    ";
                var zBase  = new ZodiacHouse(local.lagna_zh);
                for (var i = 0; i < 12; i++)
                {
                    var pmi   = (PanchangaMomentInfo)local.lagnas_ut[i];
                    var zCurr = new ZodiacHouse((ZodiacHouse.Name)pmi.info);
                    zCurr = zCurr.add(12);
                    sLagna = string.Format("{0}{1} Lagna until {2}. ",
                                           sLagna,
                                           zCurr.value,
                                           utTimeToString(pmi.ut, local.sunrise_ut, local.sunrise));
                    if (opts.OneEntryPerLine || i % 4 == 3)
                    {
                        mList.Items.Add(sLagna);
                        sLagna = string.Empty;
                    }
                }
            }

            if (opts.CalcHoraCusps)
            {
                var sHora = "    ";
                for (var i = 0; i < 24; i++)
                {
                    var ib    = (int)Basics.normalize_exc_lower(0, 7, local.hora_base + i);
                    var bHora = h.horaOrder[ib];
                    sHora = string.Format("{0}{1} hora until {2}. ",
                                          sHora,
                                          bHora,
                                          utTimeToString(local.horas_ut[i + 1], local.sunrise_ut, local.sunrise));
                    if (opts.OneEntryPerLine || i % 4 == 3)
                    {
                        mList.Items.Add(sHora);
                        sHora = string.Empty;
                    }
                }
            }

            if (opts.CalcKalaCusps)
            {
                var sKala = "    ";
                for (var i = 0; i < 16; i++)
                {
                    var ib    = (int)Basics.normalize_exc_lower(0, 8, local.kala_base + i);
                    var bKala = h.kalaOrder[ib];
                    sKala = string.Format("{0}{1} kala until {2}. ",
                                          sKala,
                                          bKala,
                                          utTimeToString(local.kalas_ut[i + 1], local.sunrise_ut, local.sunrise));
                    if (opts.OneEntryPerLine || i % 4 == 3)
                    {
                        mList.Items.Add(sKala);
                        sKala = string.Empty;
                    }
                }
            }

            mList.Items.Add(string.Empty);
        }

        private void mList_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected override void copyToClipboard()
        {
            var s = string.Empty;
            foreach (ListViewItem li in mList.Items)
            {
                s += li.Text + "\r\n";
            }

            Clipboard.SetDataObject(s, false);
        }

        private void menuCopyToClipboard_Click(object sender, EventArgs e)
        {
            copyToClipboard();
        }

        private void checkPrintReqs()
        {
            if (opts.CalcKalaCusps    == false ||
                opts.CalcNakCusps     == false ||
                opts.CalcTithiCusps   == false ||
                opts.CalcSpecialKalas == false ||
                opts.CalcSMYogaCusps  == false ||
                opts.CalcKaranaCusps  == false)
            {
                MessageBox.Show("Not enough information calculated to show panchanga");
                throw new Exception();
            }
        }

        private void menuItemPrintPanchanga_Click(object sender, EventArgs e)
        {
            try
            {
                //this.checkPrintReqs();
                var mdoc     = new PanchangaPrintDocument(opts, h, globals, locals);
                var dlgPrint = new PrintDialog();
                dlgPrint.Document = mdoc;

                if (dlgPrint.ShowDialog() == DialogResult.OK)
                {
                    mdoc.Print();
                }
            }
            catch
            {
            }
        }

        private void menuItemFilePrintPreview_Click(object sender, EventArgs e)
        {
            try
            {
                //this.checkPrintReqs();
                var mdoc       = new PanchangaPrintDocument(opts, h, globals, locals);
                var dlgPreview = new PrintPreviewDialog();
                dlgPreview.Document = mdoc;
                dlgPreview.ShowDialog();
            }
            catch
            {
            }
        }

        public class UserOptions : ICloneable
        {
            public UserOptions()
            {
                NumDays = 3;
            }

            [Description("Number of days to compute information for")]
            public int NumDays
            {
                get;
                set;
            }

            [Description("Include sunriset / sunset in the output?")]
            public bool ShowSunriset
            {
                get;
                set;
            } = true;

            [Description("Calculate and include Lagna cusp changes?")]
            public bool CalcLagnaCusps
            {
                get;
                set;
            }

            [Description("Calculate and include Tithi cusp information?")]
            public bool CalcTithiCusps
            {
                get;
                set;
            } = true;

            [Description("Calculate and include Karana cusp information?")]
            public bool CalcKaranaCusps
            {
                get;
                set;
            } = true;

            [Description("Calculate and include Sun-Moon yoga cusp information?")]
            public bool CalcSMYogaCusps
            {
                get;
                set;
            } = true;

            [Description("Calculate and include Nakshatra cusp information?")]
            public bool CalcNakCusps
            {
                get;
                set;
            } = true;

            [Description("Calculate and include Hora cusp information?")]
            public bool CalcHoraCusps
            {
                get;
                set;
            } = true;

            [Description("Calculate and include special Kalas?")]
            public bool CalcSpecialKalas
            {
                get;
                set;
            } = true;

            [Description("Calculate and include Kala cusp information?")]
            public bool CalcKalaCusps
            {
                get;
                set;
            } = true;

            [Description("Display 02:00 after midnight as 26:00 or *02:00?")]
            public bool LargeHours
            {
                get;
                set;
            }

            [Description("Display incremental updates?")]
            public bool ShowUpdates
            {
                get;
                set;
            } = true;

            [Description("Display only one entry / line?")]
            public bool OneEntryPerLine
            {
                get;
                set;
            }

            public object Clone()
            {
                var uo = new UserOptions();
                uo.NumDays          = NumDays;
                uo.CalcLagnaCusps   = CalcLagnaCusps;
                uo.CalcNakCusps     = CalcNakCusps;
                uo.CalcTithiCusps   = CalcTithiCusps;
                uo.CalcKaranaCusps  = CalcKaranaCusps;
                uo.CalcHoraCusps    = CalcHoraCusps;
                uo.CalcKalaCusps    = CalcKalaCusps;
                uo.CalcSpecialKalas = CalcSpecialKalas;
                uo.LargeHours       = LargeHours;
                uo.ShowUpdates      = ShowUpdates;
                uo.ShowSunriset     = ShowSunriset;
                uo.OneEntryPerLine  = OneEntryPerLine;
                uo.CalcSMYogaCusps  = CalcSMYogaCusps;
                return uo;
            }

            public object CopyFrom(object _uo)
            {
                var uo = (UserOptions)_uo;
                NumDays          = uo.NumDays;
                CalcLagnaCusps   = uo.CalcLagnaCusps;
                CalcNakCusps     = uo.CalcNakCusps;
                CalcTithiCusps   = uo.CalcTithiCusps;
                CalcKaranaCusps  = uo.CalcKaranaCusps;
                CalcHoraCusps    = uo.CalcHoraCusps;
                CalcKalaCusps    = uo.CalcKalaCusps;
                CalcSpecialKalas = uo.CalcSpecialKalas;
                LargeHours       = uo.LargeHours;
                ShowUpdates      = uo.ShowUpdates;
                ShowSunriset     = uo.ShowSunriset;
                CalcSMYogaCusps  = uo.CalcSMYogaCusps;
                OneEntryPerLine  = uo.OneEntryPerLine;
                return Clone();
            }
        }
    }
}