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
using Mhora.Definitions;
using Mhora.Util;

namespace Mhora.Dasas;

[TypeConverter(typeof(DasaEntryConverter))]
public class DasaEntry
{
	private Body        _graha;
	private int         _level;
	private string      _shortDesc;
	private TimeOffset  _start;
	private TimeOffset  _length; // 1 year = 360 days = 360 degrees is used internally!!!!
	private ZodiacHouse _zodiacHouse;

	public DasaEntry(Body graha, TimeOffset startUt, TimeOffset dasaLength, int level, string name)
	{
		_graha = graha;
		Construct(startUt,dasaLength, level, name);
	}

	public DasaEntry(ZodiacHouse zh, TimeOffset startUt, TimeOffset dasaLength, int level, string name)
	{
		_zodiacHouse = zh;
		Construct(startUt, dasaLength, level, name);
	}

	public DasaEntry()
	{
		_length  = 0;
		_start     = new TimeOffset();
		_level       = 1;
		_shortDesc   = "Jup";
		_graha       = Body.Jupiter;
		_zodiacHouse = ZodiacHouse.Ari;
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

	public TimeOffset Start
	{
		get => _start;
		set => _start = value;
	}

	public TimeOffset DasaLength
	{
		get => _length;
		set => _length = value;
	}

	public Body Graha
	{
		get => _graha;
		set => _graha = value;
	}

	public ZodiacHouse ZHouse
	{
		get => _zodiacHouse;
		set => _zodiacHouse = value;
	}

	private void Construct(TimeOffset startUt, TimeOffset dasaLength, int level, string shortDesc)
	{
		_start    = startUt;
		_length = dasaLength;
		_level      = level;
		_shortDesc  = shortDesc;
	}
}