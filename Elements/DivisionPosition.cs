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

using Mhora.Definitions;
using Mhora.Elements.Extensions;

namespace Mhora.Elements;

/// <summary>
///     Specifies a DivisionPosition, i.e. a position in a varga chart like Rasi
///     or Navamsa. It has no notion of "longitude".
/// </summary>
public class DivisionPosition
{
	private string      _description;
	private Longitude   _longitude;

	public Body        Body        { get; }
	public int         RulerIndex  { get; set; }
	public BodyType    BodyType    { get;}
	public ZodiacHouse ZodiacHouse { get; }
	public Cusp        Cusp        { get; }

	public DivisionPosition(Body body, BodyType bodyType, ZodiacHouse zodiacHouse, Cusp cusp)
	{
		Body        = body;
		BodyType    = bodyType;
		ZodiacHouse = zodiacHouse;
		Cusp        = cusp;
		RulerIndex  = 0;
		_longitude  = zodiacHouse.Origin();
	}

	public string Description
	{
		get
		{
			if (string.IsNullOrEmpty(_description) == false)
			{
				return _description;
			}

			return Body.ToShortString();
		}
		set => _description = value;
	}

	public Longitude Longitude
	{
		get => _longitude;
		set
		{
			_longitude = value;
			if (BodyType == BodyType.Graha || BodyType == BodyType.Lagna)
			{
				HasLongitude = true;
			}
		}
	}

	public bool HasLongitude
	{
		get;
		private set;
	}

	public override string ToString() => string.Format("{0} ({1})", Description, ZodiacHouse);

	public bool IsInMoolaTrikona()
	{
		switch (Body)
		{
			case Body.Sun:
				if (ZodiacHouse == ZodiacHouse.Leo)
				{
					return true;
				}

				break;
			case Body.Moon:
				if (ZodiacHouse == ZodiacHouse.Tau)
				{
					return true;
				}

				break;
			case Body.Mars:
				if (ZodiacHouse == ZodiacHouse.Ari)
				{
					return true;
				}

				break;
			case Body.Mercury:
				if (ZodiacHouse == ZodiacHouse.Vir)
				{
					return true;
				}

				break;
			case Body.Jupiter:
				if (ZodiacHouse == ZodiacHouse.Sag)
				{
					return true;
				}

				break;
			case Body.Venus:
				if (ZodiacHouse == ZodiacHouse.Lib)
				{
					return true;
				}

				break;
			case Body.Saturn:
				if (ZodiacHouse == ZodiacHouse.Aqu)
				{
					return true;
				}

				break;
			case Body.Rahu:
				if (ZodiacHouse == ZodiacHouse.Vir)
				{
					return true;
				}

				break;
			case Body.Ketu:
				if (ZodiacHouse == ZodiacHouse.Pis)
				{
					return true;
				}

				break;
		}

		return false;
	}

	public bool IsInOwnHouse()
	{
		var zh = ZodiacHouse;
		switch (Body)
		{
			case Body.Sun:
				if (zh == ZodiacHouse.Leo)
				{
					return true;
				}

				break;
			case Body.Moon:
				if (zh == ZodiacHouse.Tau)
				{
					return true;
				}

				break;
			case Body.Mars:
				if (zh == ZodiacHouse.Ari || zh == ZodiacHouse.Sco)
				{
					return true;
				}

				break;
			case Body.Mercury:
				if (zh == ZodiacHouse.Gem || zh == ZodiacHouse.Vir)
				{
					return true;
				}

				break;
			case Body.Jupiter:
				if (zh == ZodiacHouse.Sag || zh == ZodiacHouse.Pis)
				{
					return true;
				}

				break;
			case Body.Venus:
				if (zh == ZodiacHouse.Tau || zh == ZodiacHouse.Lib)
				{
					return true;
				}

				break;
			case Body.Saturn:
				if (zh == ZodiacHouse.Cap || zh == ZodiacHouse.Aqu)
				{
					return true;
				}

				break;
			case Body.Rahu:
				if (zh == ZodiacHouse.Aqu)
				{
					return true;
				}

				break;
			case Body.Ketu:
				if (zh == ZodiacHouse.Sco)
				{
					return true;
				}

				break;
		}

		return false;
	}

	public bool IsExaltedPhalita()
	{
		switch (Body)
		{
			case Body.Sun:
				if (ZodiacHouse == ZodiacHouse.Ari)
				{
					return true;
				}

				break;
			case Body.Moon:
				if (ZodiacHouse == ZodiacHouse.Tau)
				{
					return true;
				}

				break;
			case Body.Mars:
				if (ZodiacHouse == ZodiacHouse.Cap)
				{
					return true;
				}

				break;
			case Body.Mercury:
				if (ZodiacHouse == ZodiacHouse.Vir)
				{
					return true;
				}

				break;
			case Body.Jupiter:
				if (ZodiacHouse == ZodiacHouse.Can)
				{
					return true;
				}

				break;
			case Body.Venus:
				if (ZodiacHouse == ZodiacHouse.Pis)
				{
					return true;
				}

				break;
			case Body.Saturn:
				if (ZodiacHouse == ZodiacHouse.Lib)
				{
					return true;
				}

				break;
			case Body.Rahu:
				if (ZodiacHouse == ZodiacHouse.Gem)
				{
					return true;
				}

				break;
			case Body.Ketu:
				if (ZodiacHouse == ZodiacHouse.Sag)
				{
					return true;
				}

				break;
		}

		return false;
	}

	public bool IsDebilitatedPhalita()
	{
		switch (Body)
		{
			case Body.Sun:
				if (ZodiacHouse == ZodiacHouse.Lib)
				{
					return true;
				}

				break;
			case Body.Moon:
				if (ZodiacHouse == ZodiacHouse.Sco)
				{
					return true;
				}

				break;
			case Body.Mars:
				if (ZodiacHouse == ZodiacHouse.Can)
				{
					return true;
				}

				break;
			case Body.Mercury:
				if (ZodiacHouse == ZodiacHouse.Pis)
				{
					return true;
				}

				break;
			case Body.Jupiter:
				if (ZodiacHouse == ZodiacHouse.Cap)
				{
					return true;
				}

				break;
			case Body.Venus:
				if (ZodiacHouse == ZodiacHouse.Vir)
				{
					return true;
				}

				break;
			case Body.Saturn:
				if (ZodiacHouse == ZodiacHouse.Ari)
				{
					return true;
				}

				break;
			case Body.Rahu:
				if (ZodiacHouse == ZodiacHouse.Sag)
				{
					return true;
				}

				break;
			case Body.Ketu:
				if (ZodiacHouse == ZodiacHouse.Gem)
				{
					return true;
				}

				break;
		}

		return false;
	}

	public bool GrahaDristi(ZodiacHouse h)
	{
		var num = ZodiacHouse.NumHousesBetween(h);
		if (num == 7)
		{
			return true;
		}

		if (Body == Body.Jupiter && (num == 5 || num == 9))
		{
			return true;
		}

		if (Body == Body.Rahu && (num == 5 || num == 9 || num == 2))
		{
			return true;
		}

		if (Body == Body.Mars && (num == 4 || num == 8))
		{
			return true;
		}

		if (Body == Body.Saturn && (num == 3 || num == 10))
		{
			return true;
		}

		return false;
	}
}