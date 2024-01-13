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
using Mhora.Components.Dasa;
using Mhora.Components.Varga;
using Mhora.Elements;
using Mhora.Elements.Calculation;
using Mhora.Elements.Dasas;
using Mhora.Elements.Dasas.Nakshatra;
using Mhora.Elements.Dasas.Rasi;
using Mhora.Tables;
using mhora.Util;
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

	private readonly Font      f_fix      = new("Courier New", 10);
	private readonly Font      f_fix_s    = new("Courier New", 8);
	private readonly Font      f_s        = new("Microsoft Sans Serif", 8);
	private readonly Font      f_sanskrit = new("Sanskrit 99", 15);
	private readonly Font      f_u        = new("Microsoft Sans Serif", 10, FontStyle.Underline);
	private readonly int       pad_height = 10;
	private          ArrayList alVargas;


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
		alVargas = new ArrayList();
		for (var i = (int) Vargas.DivisionType.HoraParasara; i <= (int) Vargas.DivisionType.DwadasamsaDwadasamsa; i++)
		{
			alVargas.Add(new Division((Vargas.DivisionType) i));
		}

		numVargaPages = (int) Math.Ceiling(alVargas.Count / 6.0);

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
		var s = ". ïI ram jy<.";
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
		g.DrawString(bp.name.ToString(), f, b, 0, 0);
		g.DrawString(bp.longitude.ToString(), f_fix, b, width / 6, 0);

		var s        = string.Empty;
		var nak      = bp.longitude.toNakshatra();
		var nak_pada = bp.longitude.toNakshatraPada();
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
			s = string.Format("{0} {1}", deAntar.graha.ToShortString(), td.AddYears(deAntar.startUT).ToDateString());
		}
		else
		{
			s = string.Format("{0} {1}", ZodiacHouse.ToShortString(deAntar.zodiacHouse), td.AddYears(deAntar.startUT).ToDateString());
		}

		return s;
	}

	private void PrintDasa(IDasa id, bool bGraha)
	{
		var b      = Brushes.Black;
		var alDasa = id.Dasa(0);
		var td     = new ToDate(h.baseUT, 360, 0, h);

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
				s = de.graha.ToString();
			}
			else
			{
				s = de.zodiacHouse.ToString();
			}

			g.DrawString(s, f, b, 0, 0);
			var alAntar = id.AntarDasa(de);
			for (var j = 0; j < (int) Math.Ceiling(alAntar.Count / (double) num_entries_per_line); j++)
			{
				g.ResetTransform();
				g.TranslateTransform(left, top);
				for (var i = 0; i < num_entries_per_line; i++)
				{
					if (j * num_entries_per_line + i >= alAntar.Count)
					{
						continue;
					}

					var deAntar = (DasaEntry) alAntar[j * num_entries_per_line + i];
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

		var vd = new VimsottariDasa(h);
		vd.options.SeedBody = VimsottariDasa.UserOptions.StartBodyType.Lagna;
		vd.SetOptions(vd.options);
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
		var mStart = td.AddYears(de.startUT);
		return string.Format("{0} {1}", de.graha.ToShortString(), mStart.ToDateString());
	}

	private void PrintVimDasa(VimsottariDasa vd)
	{
		var b       = Brushes.Black;
		var al_dasa = vd.Dasa(0);
		var td      = new ToDate(h.baseUT, 360, 0, h);
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
			var mStart = td.AddYears(de.StartUT);
			g.DrawString(de.graha.Name(), f, b, 0, 0);
			//s = string.Format("{0} ", mStart.ToDateString());
			//g.DrawString(s, f_fix, b, width / 6, 0);

			var al_antar = vd.AntarDasa(de);
			var deAntar  = (DasaEntry[]) al_antar.ToArray(typeof(DasaEntry));

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

		dtype = (Division) alVargas[iVarga++];
		return true;
	}

	private void PrintVargas(PrintPageEventArgs e)
	{
		g = e.Graphics;

		left  = e.MarginBounds.Left;
		top   = e.MarginBounds.Top;
		width = e.MarginBounds.Width;

		var dtype = new Division(Vargas.DivisionType.Rasi);
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
		dc_nav.options.Varga = new Division(Vargas.DivisionType.Navamsa);
		dc_nav.PrintMode     = true;
		dc_nav.SetOptions(dc_nav.options);

		// Rasi & Navamsa charts
		g.TranslateTransform(left, top);
		dc_rasi.DrawChart(g, width / 2, width / 2);
		g.ResetTransform();
		g.TranslateTransform(left + width / 2, top);
		dc_nav.DrawChart(g, width / 2, width / 2);
		top += width / 2 + pad_height;

		// Birth Details
		PrintString(string.Format("{0} {1}. {2}. {3}, {4}.", h.wday, h.info.tob, h.info.Timezone, h.info.Latitude, h.info.Longitude));

		// Tithis
		var ltithi = h.getPosition(Body.BodyType.Moon).longitude.sub(h.getPosition(Body.BodyType.Sun).longitude);
		var offset = 360.0 / 30.0 - ltithi.toTithiOffset();
		var ti     = ltithi.toTithi();
		PrintString(string.Format("Tithis: {0} {1:N}% left", ti.GetEnumDescription(), offset / 12.0 * 100));

		// Nakshatra
		var lmoon = h.getPosition(Body.BodyType.Moon).longitude;
		var nmoon = lmoon.toNakshatra();
		offset = 360.0 / 27.0 - lmoon.toNakshatraOffset();
		var pada = lmoon.toNakshatraPada();
		PrintString(string.Format("Nakshatra: {0} {1}  {2:N}% left", nmoon.Name(), pada, offset / (360.0 / 27.0) * 100));

		// Yoga, Hora
		var smLon  = h.getPosition(Body.BodyType.Sun).longitude.add(h.getPosition(Body.BodyType.Moon).longitude);
		var smYoga = smLon.toSunMoonYoga();
		var bHora  = h.calculateHora();
		PrintString(string.Format("{0} Yoga, {1} Hora", smYoga.value, bHora));


		top += pad_height;

		// Calculation Details
		foreach (Position bp in h.positionList)
		{
			switch (bp.type)
			{
				case Body.Type.Graha:
				case Body.Type.Lagna:
				case Body.Type.SpecialLagna:
				case Body.Type.Upagraha:
					PrintBody(bp);
					break;
			}
		}

		top  = e.MarginBounds.Top  + width / 2 + pad_height + f.Height;
		left = e.MarginBounds.Left + width / 2;
		// Vimsottari Dasa
		var vd = new VimsottariDasa(h);
		vd.options.SeedBody = VimsottariDasa.UserOptions.StartBodyType.Moon;
		PrintVimDasa(vd);
	}
}