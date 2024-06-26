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
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using Mhora.Calculation;
using Mhora.Components.VargaControl;
using Mhora.Dasas;
using Mhora.Dasas.NakshatraDasa;
using Mhora.Dasas.RasiDasa;
using Mhora.Definitions;
using Mhora.Elements;
using Mhora.Elements.Extensions;
using Mhora.Util;

namespace Mhora.Components;

public class MhoraPrintDocument : PrintDocument
{
	private readonly int baseChanchaPage = 3;

	private readonly int baseDasaPage = 4;

	//int numDasaPages = 0;
	private readonly int  baseNavamsaPage = 2;
	private readonly int  baseVargaPage   = 8;
	private readonly Font f               = new("Microsoft Sans Serif", 10);

	private readonly Font   f_fix      = new("Courier New", 10);
	private readonly Font   f_fix_s    = new("Courier New", 8);
	private readonly Font   f_s        = new("Microsoft Sans Serif", 8);
	private readonly Font   f_sanskrit = new("Sanskrit 99", 15);
	private readonly Font   f_u        = new("Microsoft Sans Serif", 10, FontStyle.Underline);
	private readonly int    pad_height = 10;

	private readonly List<Division> alVargas  = [];


	private   Graphics  g;
	protected Horoscope h;

	private int iVarga;


	private int left;
	private int numVargaPages;

	private int pageNumber;
	private int top;
	private int width;

	public MhoraPrintDocument(Horoscope _h)
	{
		h = _h;
	}

	protected override void OnBeginPrint(PrintEventArgs e)
	{
		for (var i = (int) DivisionType.HoraParasara; i <= (int) DivisionType.DwadasamsaDwadasamsa; i++)
		{
			alVargas.Add(new Division((DivisionType) i));
		}

		numVargaPages = (int) (alVargas.Count / 6.0).Ceil();

		base.OnBeginPrint(e);
	}

	protected override void OnEndPrint(PrintEventArgs e)
	{
		base.OnEndPrint(e);
	}

	protected override void OnPrintPage(PrintPageEventArgs e)
	{
		base.OnPrintPage(e);
		e.HasMorePages = true;
		pageNumber++;


		PrintHeader(e);

		if (pageNumber == 1)
		{
			PrintCoverPage(e);
			return;
		}

		if (pageNumber == baseDasaPage)
		{
			PrintNarayanaDasa(e);
			return;
		}

		if (pageNumber == baseNavamsaPage)
		{
			PrintNavamsaChakra(e);
			return;
		}

		if (pageNumber == baseChanchaPage)
		{
			PrintChanchaChakra(e);
			return;
		}

		if (pageNumber == baseDasaPage + 1)
		{
			PrintSuDasa(e);
			return;
		}

		if (pageNumber == baseDasaPage + 2)
		{
			PrintShoolaDasa(e);
			return;
		}

		if (pageNumber == baseDasaPage + 3)
		{
			PrintDrigDasa(e);
			return;
		}

		if (pageNumber >= baseVargaPage && pageNumber < baseVargaPage + numVargaPages - 1)
		{
			PrintVargas(e);
			return;
		}

		if (pageNumber == baseVargaPage + numVargaPages - 1)
		{
			e.HasMorePages = false;
			PrintVargas(e);
		}
	}

	private void PrintHeader(PrintPageEventArgs e)
	{
		var s = ". �I ram jy<.";
		e.Graphics.ResetTransform();
		var sz = e.Graphics.MeasureString(s, f_sanskrit);
		e.Graphics.TranslateTransform(e.MarginBounds.Left, e.MarginBounds.Top);
		e.Graphics.DrawString(s, f_sanskrit, Brushes.Black, e.MarginBounds.Width / 2 - sz.Width / 2, -1 * f_sanskrit.Height);
		e.Graphics.ResetTransform();

		e.Graphics.ResetTransform();
		e.Graphics.TranslateTransform(e.PageBounds.Width / 2, e.MarginBounds.Bottom + f.Height * 2);
		s  = string.Format("Page {0}", pageNumber);
		sz = e.Graphics.MeasureString(s, f);
		e.Graphics.DrawString(s, f, Brushes.Black, -sz.Width / 2, 0);
		e.Graphics.ResetTransform();
	}

	private void PrintBody(Position bp)
	{
		var b = Brushes.Black;
		g.ResetTransform();
		g.TranslateTransform(left, top);
		g.DrawString(bp.Name.ToString(), f, b, 0, 0);
		g.DrawString(bp.Longitude.ToString(), f_fix, b, width / 6, 0);

		var s        = string.Empty;
		var nak      = bp.Longitude.ToNakshatra();
		var nak_pada = bp.Longitude.NakshatraPada();
		s = string.Format("{0} {1}", nak.ToShortString(), nak_pada);
		g.DrawString(s, f, b, (float) (width / 6 * 2.5), 0);

		top += f.Height;
	}

	private void PrintString(string s)
	{
		g.ResetTransform();
		g.TranslateTransform(left, top);
		g.DrawString(s, f, Brushes.Black, 0, 0);
		top += f.Height;
	}


	private string GetDasaString(ToDate td, DasaEntry deAntar, bool bGraha)
	{
		string s;
		if (bGraha)
		{
			s = string.Format("{0} {1}", deAntar.Graha.ToShortString(), td.AddYears(deAntar.Start).ToDateString());
		}
		else
		{
			s = string.Format("{0} {1}", deAntar.ZHouse.ToShortString(), td.AddYears(deAntar.Start).ToDateString());
		}

		return s;
	}

	private void PrintDasa(IDasa id, bool bGraha)
	{
		var b      = Brushes.Black;
		var alDasa = id.Dasa(0);
		var td     = new ToDate(h.Info.Jd, 360, 0, h);

		var num_entries_per_line = 6;
		var entry_width          = width / 6;

		g.ResetTransform();
		g.TranslateTransform(left, top);
		g.DrawString(id.Description(), f_u, b, 0, 0);
		top += f.Height * 2;

		foreach (DasaEntry de in alDasa)
		{
			g.ResetTransform();
			g.TranslateTransform(left, top);
			var s = string.Empty;
			if (bGraha)
			{
				s = de.Graha.ToString();
			}
			else
			{
				s = de.ZHouse.ToString();
			}

			g.DrawString(s, f, b, 0, 0);
			var alAntar = id.AntarDasa(de);
			for (var j = 0; j < (int) (alAntar.Count / (double)num_entries_per_line).Ceil(); j++)
			{
				g.ResetTransform();
				g.TranslateTransform(left, top);
				for (var i = 0; i < num_entries_per_line; i++)
				{
					if (j * num_entries_per_line + i >= alAntar.Count)
					{
						continue;
					}

					var deAntar = alAntar[j * num_entries_per_line + i];
					s = GetDasaString(td, deAntar, bGraha);
					g.DrawString(s, f_fix_s, b, (i + 1) * entry_width - (float) (entry_width * .5), 0);
				}

				top += f_fix_s.Height;
			}

			top += 5;
		}
	}

	private void PrintNarayanaDasa(PrintPageEventArgs e)
	{
		left  = e.MarginBounds.Left;
		top   = e.MarginBounds.Top;
		width = e.MarginBounds.Width;

		IDasa id = new NarayanaDasa(h);
		g = e.Graphics;
		g.ResetTransform();
		g.TranslateTransform(left, top);
		PrintDasa(id, false);
	}

	private void PrintDrigDasa(PrintPageEventArgs e)
	{
		left  = e.MarginBounds.Left;
		top   = e.MarginBounds.Top;
		width = e.MarginBounds.Width;

		IDasa id = new DrigDasa(h);
		g = e.Graphics;
		g.ResetTransform();
		g.TranslateTransform(left, top);
		PrintDasa(id, false);

		var vd = new VimsottariDasa(h)
		{
			Options =
			{
				SeedBody = VimsottariDasa.UserOptions.StartBodyType.Lagna
			}
		};
		vd.SetOptions(vd.Options);
		id = vd;
		PrintDasa(id, true);
	}

	private void PrintShoolaDasa(PrintPageEventArgs e)
	{
		left  = e.MarginBounds.Left;
		top   = e.MarginBounds.Top;
		width = e.MarginBounds.Width;

		IDasa id = new ShoolaDasa(h);
		g = e.Graphics;
		g.ResetTransform();
		g.TranslateTransform(left, top);
		PrintDasa(id, false);

		id = new NirayaanaShoolaDasa(h);
		PrintDasa(id, false);
	}

	private void PrintSuDasa(PrintPageEventArgs e)
	{
		left  = e.MarginBounds.Left;
		top   = e.MarginBounds.Top;
		width = e.MarginBounds.Width;

		IDasa id = new SuDasa(h);
		g = e.Graphics;
		g.ResetTransform();
		g.TranslateTransform(left, top);
		PrintDasa(id, false);
	}


	private string GetVimAntarString(ToDate td, DasaEntry de)
	{
		var mStart = td.AddYears(de.Start);
		return string.Format("{0} {1}", de.Graha.ToShortString(), mStart.ToDateString());
	}

	private void PrintVimDasa(VimsottariDasa vd)
	{
		var b       = Brushes.Black;
		var al_dasa = vd.Dasa(0);
		var td      = new ToDate(h.Info.Jd, 360, 0, h);
		var s       = string.Empty;

		g.ResetTransform();
		g.TranslateTransform(left, top);
		s = vd.Description();
		g.DrawString(s, f_u, b, 0, 0);
		top += f.Height + pad_height;

		foreach (DasaEntry de in al_dasa)
		{
			g.ResetTransform();
			g.TranslateTransform(left, top);
			var mStart = td.AddYears(de.Start);
			g.DrawString(de.Graha.Name(), f, b, 0, 0);
			//s = string.Format("{0} ", mStart.ToDateString());
			//g.DrawString(s, f_fix, b, width / 6, 0);

			var deAntar = vd.AntarDasa(de);

			var aw  = width / 7;
			var off = -40;
			g.DrawString(GetVimAntarString(td, deAntar[0]), f_s, b, off + aw, 0);
			g.DrawString(GetVimAntarString(td, deAntar[1]), f_s, b, off + aw * 2, 0);
			g.DrawString(GetVimAntarString(td, deAntar[2]), f_s, b, off + aw * 3, 0);

			g.DrawString(GetVimAntarString(td, deAntar[3]), f_s, b, off + aw, f.Height);
			g.DrawString(GetVimAntarString(td, deAntar[4]), f_s, b, off + aw * 2, f.Height);
			g.DrawString(GetVimAntarString(td, deAntar[5]), f_s, b, off + aw * 3, f.Height);

			g.DrawString(GetVimAntarString(td, deAntar[6]), f_s, b, off + aw, f.Height     * 2);
			g.DrawString(GetVimAntarString(td, deAntar[7]), f_s, b, off + aw * 2, f.Height * 2);
			g.DrawString(GetVimAntarString(td, deAntar[8]), f_s, b, off + aw * 3, f.Height * 2);

			top += f.Height * 3 + 4;
		}
	}

	private bool GetNextVarga(ref Division dtype)
	{
		if (iVarga >= alVargas.Count)
		{
			return false;
		}

		dtype = alVargas[iVarga++];
		return true;
	}

	private void PrintVargas(PrintPageEventArgs e)
	{
		g = e.Graphics;

		left  = e.MarginBounds.Left;
		top   = e.MarginBounds.Top;
		width = e.MarginBounds.Width;

		var dtype = new Division(DivisionType.Rasi);
		var dc    = new DivisionalChart(h);
		dc.PrintMode = true;

		var dc_size = Math.Min(width / 2, e.MarginBounds.Height / 3);

		if (false == GetNextVarga(ref dtype))
		{
			return;
		}

		dc.options.Varga = dtype;
		dc.SetOptions(dc.options);
		g.TranslateTransform(left, top);
		dc.DrawChart(g, dc_size, dc_size);

		g.ResetTransform();
		if (false == GetNextVarga(ref dtype))
		{
			return;
		}

		dc.options.Varga = dtype;
		dc.SetOptions(dc.options);
		g.TranslateTransform(left + dc_size, top);
		dc.DrawChart(g, dc_size, dc_size);

		g.ResetTransform();
		if (false == GetNextVarga(ref dtype))
		{
			return;
		}

		dc.options.Varga = dtype;
		dc.SetOptions(dc.options);
		g.TranslateTransform(left, top + dc_size);
		dc.DrawChart(g, dc_size, dc_size);

		g.ResetTransform();
		if (false == GetNextVarga(ref dtype))
		{
			return;
		}

		dc.options.Varga = dtype;
		dc.SetOptions(dc.options);
		g.TranslateTransform(left + dc_size, top + dc_size);
		dc.DrawChart(g, dc_size, dc_size);

		g.ResetTransform();
		if (false == GetNextVarga(ref dtype))
		{
			return;
		}

		dc.options.Varga = dtype;
		dc.SetOptions(dc.options);
		g.TranslateTransform(left, top + dc_size * 2);
		dc.DrawChart(g, dc_size, dc_size);

		g.ResetTransform();
		if (false == GetNextVarga(ref dtype))
		{
			return;
		}

		dc.options.Varga = dtype;
		dc.SetOptions(dc.options);
		g.TranslateTransform(left + dc_size, top + dc_size * 2);
		dc.DrawChart(g, dc_size, dc_size);
	}

	private void PrintNavamsaChakra(PrintPageEventArgs e)
	{
		g = e.Graphics;

		left  = e.MarginBounds.Left;
		top   = e.MarginBounds.Top;
		width = e.MarginBounds.Width;

		var nc = new NavamsaControl(h);
		nc.PrintMode = true;
		var bmp = nc.DrawToBitmap(Math.Min(e.MarginBounds.Width, e.MarginBounds.Height));

		g.ResetTransform();
		g.DrawImage(bmp, left, top);
	}

	private void PrintChanchaChakra(PrintPageEventArgs e)
	{
		g = e.Graphics;

		left  = e.MarginBounds.Left;
		top   = e.MarginBounds.Top;
		width = e.MarginBounds.Width;
		var iChanchaSize = Math.Min(e.MarginBounds.Width, e.MarginBounds.Height) - 100;
		var iNavaSize    = 350;

		var ac = new AshtakavargaControl(h);
		ac.PrintMode = true;
		Image iChancha = ac.DrawChanchaToImage(iChanchaSize);
		Image iNava    = ac.DrawNavaChakrasToImage(iNavaSize);

		g.ResetTransform();
		g.DrawImage(iChancha, e.PageBounds.Width / 2 - iChanchaSize / 2, top);
		g.DrawImage(iNava, e.PageBounds.Width    / 2 - iNavaSize    / 2, e.PageBounds.Height - iNavaSize - 80);
	}

	private void PrintCoverPage(PrintPageEventArgs e)
	{
		g = e.Graphics;

		left  = e.MarginBounds.Left;
		top   = e.MarginBounds.Top;
		width = e.MarginBounds.Width;


		var dc_rasi = new DivisionalChart(h);
		dc_rasi.PrintMode = true;

		var dc_nav = new DivisionalChart(h);
		dc_nav.options.Varga = new Division(DivisionType.Navamsa);
		dc_nav.PrintMode     = true;
		dc_nav.SetOptions(dc_nav.options);

		// Rasi & Navamsa charts
		g.TranslateTransform(left, top);
		dc_rasi.DrawChart(g, width / 2, width / 2);
		g.ResetTransform();
		g.TranslateTransform(left + width / 2, top);
		dc_nav.DrawChart(g, width / 2, width / 2);
		top += width / 2 + pad_height;

		Time dstOffset = h.Info.DstOffset;
		if (h.Info.UseDst == false)
		{
			dstOffset -= h.Info.DstCorrection;
		}
		// Birth Details
		PrintString(string.Format("{0} {1}. {2}. {3}, {4}.", h.Vara.WeekDay, h.Info.DateOfBirth, dstOffset, h.Info.Latitude, h.Info.Longitude));

		// Tithis
		var ltithi = h.GetPosition(Body.Moon).Longitude.Sub(h.GetPosition(Body.Sun).Longitude);
		var offset = 360.0 / 30.0 - ltithi.ToTithiOffset();
		var ti     = ltithi.ToTithi();
		PrintString(string.Format("Tithis: {0} {1:N}% left", ti.GetEnumDescription(), offset / 12.0 * 100));

		// Nakshatra
		var lmoon = h.GetPosition(Body.Moon).Longitude;
		var nmoon = lmoon.ToNakshatra();
		offset = 360.0 / 27.0 - lmoon.NakshatraOffset();
		var pada = lmoon.NakshatraPada();
		PrintString(string.Format("Nakshatra: {0} {1}  {2:N}% left", nmoon.Name(), pada, offset / (360.0 / 27.0) * 100));

		// Yoga, Hora
		var smLon  = h.GetPosition(Body.Sun).Longitude.Add(h.GetPosition(Body.Moon).Longitude);
		var smYoga = smLon.ToSunMoonYoga();
		var bHora  = h.Vara.HoraLord;
		PrintString(string.Format("{0} Yoga, {1} Hora", smYoga, bHora));


		top += pad_height;

		// Calculation Details
		foreach (Position bp in h.PositionList)
		{
			switch (bp.BodyType)
			{
				case BodyType.Graha:
				case BodyType.Lagna:
				case BodyType.SpecialLagna:
				case BodyType.Upagraha:
					PrintBody(bp);
					break;
			}
		}

		top  = e.MarginBounds.Top  + width / 2 + pad_height + f.Height;
		left = e.MarginBounds.Left + width / 2;
		// Vimsottari Dasa
		var vd = new VimsottariDasa(h)
		{
			Options =
			{
				SeedBody = VimsottariDasa.UserOptions.StartBodyType.Moon
			}
		};
		PrintVimDasa(vd);
	}
}