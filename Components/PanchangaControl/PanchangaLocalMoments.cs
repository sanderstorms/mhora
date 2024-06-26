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

using System.Collections;
using System.Collections.Generic;
using Mhora.Definitions;
using Mhora.Util;

namespace Mhora.Components.PanchangaControl;

public class PanchangaLocalMoments
{
	public int                       gulika_kala_index;
	public JulianDate[]              horas_ut;
	public JulianDate[]              kalas_ut;
	public int                       karana_index_end;
	public int                       karana_index_start;
	public ZodiacHouse               lagna_zh;
	public List<PanchangaMomentInfo> lagnas_ut = [];
	public int                       nakshatra_index_end;
	public int                       nakshatra_index_start;
	public int                       rahu_kala_index;
	public int                       smyoga_index_end;
	public int                       smyoga_index_start;
	public Time                      sunrise;
	public JulianDate                sunrise_ut;
	public Time                      sunset;
	public int                       tithi_index_end;
	public int                       tithi_index_start;
	public Weekday                   wday;
	public int                       yama_kala_index;
}