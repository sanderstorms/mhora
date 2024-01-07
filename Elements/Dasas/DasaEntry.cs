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

using System.ComponentModel;
using Mhora.Components.Converter;
using Mhora.Components.Dasa;

namespace Mhora.Elements.Dasas;

[TypeConverter(typeof(DasaEntryConverter))]
public class DasaEntry
{
	public double           dasaLength; // 1 year = 360 days = 360 degrees is used internally!!!!
	public Body.BodyType        graha;
	public int              level;
	public string           shortDesc;
	public double           startUT;
	public ZodiacHouse.Rasi zodiacHouse;

	public DasaEntry(Body.BodyType _graha, double _startUT, double _dasaLength, int _level, string _shortDesc)
	{
		graha = _graha;
		Construct(_startUT, _dasaLength, _level, _shortDesc);
	}

	public DasaEntry(ZodiacHouse.Rasi _zodiacHouse, double _startUT, double _dasaLength, int _level, string _shortDesc)
	{
		zodiacHouse = _zodiacHouse;
		Construct(_startUT, _dasaLength, _level, _shortDesc);
	}

	public DasaEntry()
	{
		startUT     = dasaLength = 0.0;
		level       = 1;
		shortDesc   = "Jup";
		graha       = Body.BodyType.Jupiter;
		zodiacHouse = ZodiacHouse.Rasi.Ari;
	}

	public string DasaName
	{
		get => shortDesc;
		set => shortDesc = value;
	}

	public int DasaLevel
	{
		get => level;
		set => level = value;
	}

	public double StartUT
	{
		get => startUT;
		set => startUT = value;
	}

	public double DasaLength
	{
		get => dasaLength;
		set => dasaLength = value;
	}

	public Body.BodyType Graha
	{
		get => graha;
		set => graha = value;
	}

	public ZodiacHouse.Rasi ZHouse
	{
		get => zodiacHouse;
		set => zodiacHouse = value;
	}

	private void Construct(double _startUT, double _dasaLength, int _level, string _shortDesc)
	{
		startUT    = _startUT;
		dasaLength = _dasaLength;
		level      = _level;
		shortDesc  = _shortDesc;
	}
}