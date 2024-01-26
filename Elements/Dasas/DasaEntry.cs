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

namespace Mhora.Elements.Dasas;

[TypeConverter(typeof(DasaEntryConverter))]
public class DasaEntry
{
	private double           _dasaLength; // 1 year = 360 days = 360 degrees is used internally!!!!
	private Body.BodyType    _graha;
	private int              _level;
	private string           _shortDesc;
	private double           _startUt;
	private ZodiacHouse.Rasi _zodiacHouse;

	public DasaEntry(Body.BodyType graha, double startUt, double dasaLength, int level, string name)
	{
		_graha = graha;
		Construct(startUt,dasaLength, level, name);
	}

	public DasaEntry(ZodiacHouse.Rasi zh, double startUt, double dasaLength, int level, string name)
	{
		_zodiacHouse = zh;
		Construct(startUt, dasaLength, level, name);
	}

	public DasaEntry()
	{
		_startUt     = _dasaLength = 0.0;
		_level       = 1;
		_shortDesc   = "Jup";
		_graha       = Body.BodyType.Jupiter;
		_zodiacHouse = ZodiacHouse.Rasi.Ari;
	}

	public string DasaName
	{
		get => _shortDesc;
		set => _shortDesc = value;
	}

	public int Level
	{
		get => _level;
		set => _level = value;
	}

	public double StartUt
	{
		get => _startUt;
		set => _startUt = value;
	}

	public double DasaLength
	{
		get => _dasaLength;
		set => _dasaLength = value;
	}

	public Body.BodyType Graha
	{
		get => _graha;
		set => _graha = value;
	}

	public ZodiacHouse.Rasi ZHouse
	{
		get => _zodiacHouse;
		set => _zodiacHouse = value;
	}

	private void Construct(double startUt, double dasaLength, int level, string shortDesc)
	{
		_startUt    = startUt;
		_dasaLength = dasaLength;
		_level      = level;
		_shortDesc  = shortDesc;
	}
}