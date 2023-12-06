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
using System.Drawing;
using System.Drawing.Printing;
using Mhora.Calculation;
using Mhora.Hora;
using Mhora.Settings;
using Mhora.SwissEph;
using Mhora.Varga;

namespace Mhora.Panchanga
{
    /// <summary>
    ///     Summary description for PanchangaPrintDocument.
    /// </summary>
    public class PanchangaPrintDocument : PrintDocument
    {
        private readonly Brush b = Brushes.Black;
        public           bool  bPrintLagna;

        public  bool bPrintPanchanga = true;
        private int  day_offset;
        private int  day_width;
        private int  divisional_chart_size;

        private readonly Font f = MhoraGlobalOptions.Instance.GeneralFont;

        private readonly Font f_u = new Font(MhoraGlobalOptions.Instance.GeneralFont.FontFamily,
                                             MhoraGlobalOptions.Instance.GeneralFont.SizeInPoints,
                                             FontStyle.Underline);

        private readonly PanchangaGlobalMoments globals;
        private readonly Horoscope              h;
        private readonly int                    header_offset = 30;
        private          int                    karana_name_1_offset;
        private          int                    karana_name_2_offset;
        private          int                    karana_name_width;
        private          int                    karana_time_1_offset;
        private          int                    karana_time_2_offset;
        private          int                    karana_time_width;
        private          int                    local_index;
        private readonly ArrayList              locals;

        private readonly int                          margin_offset = 30;
        private          int                          nak_name_offset;
        private          int                          nak_name_width;
        private          int                          nak_time_offset;
        private          int                          nak_time_width;
        private readonly PanchangaControl.UserOptions opts;
        private readonly Pen                          p = Pens.Black;
        private          int                          rahu_kala_offset;
        private          int                          rahu_kala_width;
        private          int                          sm_name_offset;
        private          int                          sm_name_width;
        private          int                          sm_time_offset;
        private          int                          sm_time_width;
        private          int                          sunrise_offset;
        private          int                          sunrise_width;
        private          int                          sunset_offset;
        private          int                          sunset_width;

        private int time_width;
        private int tithi_name_offset;
        private int tithi_name_width;
        private int tithi_time_offset;
        private int tithi_time_width;
        private int wday_offset;
        private int wday_width;

        public PanchangaPrintDocument(
            PanchangaControl.UserOptions _opts,
            Horoscope                    _h,
            PanchangaGlobalMoments       _globals,
            ArrayList                    _locals)
        {
            h       = _h;
            opts    = _opts;
            globals = _globals;
            locals  = _locals;

            if (locals.Count                                       > 0 &&
                ((PanchangaLocalMoments)locals[0]).lagnas_ut.Count > 1)
            {
                bPrintLagna = true;
            }
        }

        protected override void OnBeginPrint(PrintEventArgs e)
        {
            base.OnBeginPrint(e);

            divisional_chart_size = 250;
            time_width            = 43;

            day_width         = 65;
            wday_width        = 25;
            sunrise_width     = time_width;
            sunset_width      = time_width;
            tithi_name_width  = 75;
            tithi_time_width  = time_width;
            karana_name_width = 85;
            karana_time_width = time_width;
            nak_name_width    = 70;
            nak_time_width    = time_width;
            sm_name_width     = 80;
            sm_time_width     = time_width;
            rahu_kala_width   = time_width * 2 + 10;

            day_offset     = 0;
            wday_offset    = day_width;
            sunrise_offset = wday_offset    + wday_width;
            sunset_offset  = sunrise_offset + sunrise_width;

            nak_name_offset      = sunset_offset        + sunset_width;
            nak_time_offset      = nak_name_offset      + nak_name_width;
            tithi_name_offset    = nak_time_offset      + nak_time_width;
            tithi_time_offset    = tithi_name_offset    + tithi_name_width;
            karana_name_1_offset = tithi_time_offset    + tithi_time_width;
            karana_time_1_offset = karana_name_1_offset + karana_name_width;
            karana_name_2_offset = karana_time_1_offset + karana_time_width;
            karana_time_2_offset = karana_name_2_offset + karana_name_width;
            sm_name_offset       = karana_time_2_offset + karana_time_width;
            sm_time_offset       = sm_name_offset       + sm_name_width;
            rahu_kala_offset     = sm_time_offset       + sm_time_width;
        }

        protected override void OnEndPrint(PrintEventArgs e)
        {
            base.OnEndPrint(e);
        }

        private void checkForMorePages(PrintPageEventArgs e)
        {
            e.HasMorePages = true;
            if (bPrintLagna     == false &&
                bPrintPanchanga == false)
            {
                e.HasMorePages = false;
            }
        }

        protected override void OnPrintPage(PrintPageEventArgs e)
        {
            if (bPrintPanchanga)
            {
                PrintFirstPage(e);
            }
            else if (bPrintLagna)
            {
                PrintLagna(e);
            }

            checkForMorePages(e);
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

        private string utTimeToString(double ut_event, double ut_sr, double sunrise)
        {
            var m   = utToMoment(ut_event);
            var hms = new HMSInfo(m.time);

            if (ut_event >= ut_sr - sunrise / 24.0 + 1.0)
            {
                if (false == opts.LargeHours)
                {
                    return string.Format("*{0}:{1:00}", hms.degree, hms.minute);
                }

                return string.Format("{0:00}:{1:00}", hms.degree + 24, hms.minute);
            }

            return string.Format("{0:00}:{1:00}", hms.degree, hms.minute);
        }

        private void PrintLagna(PrintPageEventArgs e)
        {
            e.HasMorePages = true;
            var g = e.Graphics;
            g.ResetTransform();
            g.TranslateTransform(100, header_offset);

            for (var j = 1; j <= 12; j++)
            {
                var zh = new ZodiacHouse((ZodiacHouse.Name)j);
                g.DrawString(zh.value.ToString(),
                             f,
                             b,
                             day_offset + 100 + (int)zh.value * time_width,
                             0);
            }

            g.TranslateTransform(0, f.Height);

            var i = local_index;
            while (i < locals.Count)
            {
                var local     = (PanchangaLocalMoments)locals[i];
                var m_sunrise = new Moment(local.sunrise_ut, h);
                g.DrawString(m_sunrise.ToString(), f, b, day_offset, 0);

                for (var j = 0; j < local.lagnas_ut.Count; j++)
                {
                    var pmi = (PanchangaMomentInfo)local.lagnas_ut[j];
                    //Moment m_lagna = new Moment(pmi.ut, h);
                    var zh = new ZodiacHouse((ZodiacHouse.Name)pmi.info);
                    zh = zh.add(12);
                    var _f = f;

                    if (local.lagna_zh == zh.value)
                    {
                        _f = f_u;
                    }

                    g.DrawString(utTimeToString(pmi.ut, local.sunrise_ut, local.sunrise),
                                 _f,
                                 b,
                                 day_offset + 100 + (int)zh.value * time_width,
                                 0);
                }

                local_index = ++i;
                g.TranslateTransform(0, f.Height);
                if (g.Transform.OffsetY > e.PageBounds.Height - header_offset)
                {
                    return;
                }
            }

            bPrintLagna    = false;
            e.HasMorePages = false;
        }

        private void PrintTitle(Graphics g, int left, int right, string s)
        {
            var sz = g.MeasureString(s, f);
            g.DrawString(s, f, b, left + (right - left - sz.Width) / 2, 0);
        }

        private void PrintFirstPage(PrintPageEventArgs e)
        {
            e.HasMorePages = true;
            var g = e.Graphics;
            g.ResetTransform();
            g.TranslateTransform(margin_offset, header_offset);

            PrintTitle(g, 0, wday_offset                             + wday_width, "Date/Day");
            PrintTitle(g, sunrise_offset, sunset_offset              + sunset_width, "Sunrise/set");
            PrintTitle(g, nak_name_offset, nak_time_offset           + nak_time_width, "Nakshatra");
            PrintTitle(g, tithi_name_offset, tithi_time_offset       + tithi_time_width, "Tithi");
            PrintTitle(g, karana_name_1_offset, karana_time_2_offset + karana_time_width, "Karana");
            PrintTitle(g, sm_name_offset, sm_time_offset             + sm_time_width, "SM-Yoga");

            g.TranslateTransform(0, (float)(f.Height * 1.5));

            var iStart = local_index;
            var i      = local_index;
            while (i < locals.Count)
            {
                var numLines  = 1;
                var local     = (PanchangaLocalMoments)locals[i];
                var m_sunrise = new Moment(local.sunrise_ut, h);
                var m_sunset  = new Moment(0, 0, 0, local.sunset);

                g.DrawString(m_sunrise.ToShortDateString(), f, b, day_offset, 0);
                g.DrawString(Basics.weekdayToShortString(local.wday), f, b, wday_offset, 0);

                if (opts.ShowSunriset)
                {
                    g.DrawString(m_sunrise.ToTimeString(), f, b, sunrise_offset, 0);
                    g.DrawString(m_sunset.ToTimeString(), f, b, sunset_offset, 0);
                }

                var numTithis  = local.tithi_index_end     - local.tithi_index_start;
                var numNaks    = local.nakshatra_index_end - local.nakshatra_index_start;
                var numSMYogas = local.smyoga_index_end    - local.smyoga_index_start;
                var numKaranas = local.karana_index_end    - local.karana_index_start;

                if (opts.CalcTithiCusps)
                {
                    numLines = Math.Max(numLines, numTithis);
                    for (var j = 0; j < numTithis; j++)
                    {
                        var pmi    = (PanchangaMomentInfo)globals.tithis_ut[local.tithi_index_start + 1 + j];
                        var t      = new Tithi((Tithi.Name)pmi.info);
                        var mTithi = new Moment(pmi.ut, h);
                        g.DrawString(t.ToUnqualifiedString(), f, b, tithi_name_offset, j * f.Height);
                        g.DrawString(utTimeToString(pmi.ut, local.sunrise_ut, local.sunrise),
                                     f,
                                     b,
                                     tithi_time_offset,
                                     j * f.Height);
                    }
                }

                if (opts.CalcKaranaCusps)
                {
                    numLines = Math.Max(numLines, (int)Math.Ceiling(numKaranas / 2.0));
                    for (var j = 0; j < numKaranas; j++)
                    {
                        var pmi         = (PanchangaMomentInfo)globals.karanas_ut[local.karana_index_start + 1 + j];
                        var k           = new Karana((Karana.Name)pmi.info);
                        var mKarana     = new Moment(pmi.ut, h);
                        var jRow        = (int)Math.Floor((decimal)j / 2);
                        var name_offset = karana_name_1_offset;
                        var time_offset = karana_time_1_offset;
                        if (j % 2 == 1)
                        {
                            name_offset = karana_name_2_offset;
                            time_offset = karana_time_2_offset;
                        }

                        g.DrawString(k.value.ToString(), f, b, name_offset, jRow * f.Height);
                        g.DrawString(utTimeToString(pmi.ut, local.sunrise_ut, local.sunrise),
                                     f,
                                     b,
                                     time_offset,
                                     jRow * f.Height);
                    }
                }

                if (opts.CalcNakCusps)
                {
                    numLines = Math.Max(numLines, numNaks);
                    for (var j = 0; j < numNaks; j++)
                    {
                        var pmi  = (PanchangaMomentInfo)globals.nakshatras_ut[local.nakshatra_index_start + 1 + j];
                        var n    = new Nakshatra((Nakshatra.Name)pmi.info);
                        var mNak = new Moment(pmi.ut, h);
                        g.DrawString(n.ToString(), f, b, nak_name_offset, j * f.Height);
                        g.DrawString(utTimeToString(pmi.ut, local.sunrise_ut, local.sunrise),
                                     f,
                                     b,
                                     nak_time_offset,
                                     j * f.Height);
                    }
                }

                if (opts.CalcSMYogaCusps)
                {
                    numLines = Math.Max(numLines, numSMYogas);
                    for (var j = 0; j < numSMYogas; j++)
                    {
                        var pmi     = (PanchangaMomentInfo)globals.smyogas_ut[local.smyoga_index_start + 1 + j];
                        var sm      = new SunMoonYoga((SunMoonYoga.Name)pmi.info);
                        var mSMYoga = new Moment(pmi.ut, h);
                        g.DrawString(sm.value.ToString(), f, b, sm_name_offset, j * f.Height);
                        g.DrawString(utTimeToString(pmi.ut, local.sunrise_ut, local.sunrise),
                                     f,
                                     b,
                                     sm_time_offset,
                                     j * f.Height);
                    }
                }


            #if DND
				string s_rahu_kala = string.Format("{0} - {1}", 
					this.utTimeToString(local.kalas_ut[local.rahu_kala_index], local.sunrise_ut, local.sunrise),
					this.utTimeToString(local.kalas_ut[local.rahu_kala_index+1], local.sunrise_ut, local.sunrise));
				g.DrawString(s_rahu_kala, f, b, rahu_kala_offset, 0);
            #endif

                g.TranslateTransform(0, f.Height * numLines);

                local_index = ++i;

                if (g.Transform.OffsetY > e.PageBounds.Height - header_offset - divisional_chart_size)
                {
                    goto first_done;
                }
            }

            bPrintPanchanga = false;
            local_index     = 0;

        first_done:
            var   offsetY = g.Transform.OffsetY;
            float offsetX = margin_offset + sm_time_offset + sm_time_width;

            var mCurr  = new Moment(((PanchangaLocalMoments)locals[iStart]).sunrise_ut, h);
            var hiCurr = new HoraInfo(mCurr, h.info.lat, h.info.lon, h.info.tz);
            var hCurr  = new Horoscope(hiCurr, h.options);
            var dc     = new DivisionalChart(hCurr);
            dc.PrintMode         = true;
            dc.options.ViewStyle = DivisionalChart.UserOptions.EViewStyle.Panchanga;
            dc.SetOptions(dc.options);
            dc.DrawChart(g, divisional_chart_size, divisional_chart_size);

            g.ResetTransform();
            // horizontal top & bottom
            g.DrawLine(p, margin_offset - 5, header_offset     - 5, margin_offset                     + sm_time_offset + sm_time_width + 5, header_offset     - 5);
            g.DrawLine(p, margin_offset - 5, header_offset - 5 + f.Height * (float)1.5, margin_offset + sm_time_offset + sm_time_width + 5, header_offset - 5 + f.Height * (float)1.5);
            g.DrawLine(p, margin_offset - 5, offsetY           + 5, offsetX                           + 5, offsetY     + 5);
            // vertical left and right
            g.DrawLine(p, margin_offset - 5, header_offset - 5, margin_offset - 5, offsetY + 5);
            g.DrawLine(p, offsetX       + 5, header_offset - 5, offsetX       + 5, offsetY + 5);

            g.DrawLine(p,
                       margin_offset + sunset_offset + sunset_width - 2,
                       header_offset                                - 5,
                       margin_offset + sunset_offset + sunset_width - 2,
                       offsetY                                      + 5);

            g.DrawLine(p,
                       margin_offset + tithi_time_offset + tithi_time_width - 2,
                       header_offset                                        - 5,
                       margin_offset + tithi_time_offset + tithi_time_width - 2,
                       offsetY                                              + 5);

            g.DrawLine(p,
                       margin_offset + nak_time_offset + nak_time_width - 2,
                       header_offset                                    - 5,
                       margin_offset + nak_time_offset + nak_time_width - 2,
                       offsetY                                          + 5);

            g.DrawLine(p,
                       margin_offset + karana_time_2_offset + karana_time_width - 2,
                       header_offset                                            - 5,
                       margin_offset + karana_time_2_offset + karana_time_width - 2,
                       offsetY                                                  + 5);
        }
    }
}