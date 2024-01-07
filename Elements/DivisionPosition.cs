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

namespace Mhora.Elements;

/// <summary>
///     Specifies a DivisionPosition, i.e. a position in a varga chart like Rasi
///     or Navamsa. It has no notion of "longitude".
/// </summary>
public class DivisionPosition
{
	private string      _description;
	private Longitude   _longitude;
	public  double      cusp_higher;
	public  double      cusp_lower;
	public  Body.BodyType   name;
	public  int         part;
	public  int         ruler_index;
	public  Body.Type   type;
	public  ZodiacHouse zodiac_house;

	public DivisionPosition(Body.BodyType _name, Body.Type _type, ZodiacHouse _zodiac_house, double _cusp_lower, double _cusp_higher, int _part)
	{
		name         = _name;
		type         = _type;
		zodiac_house = _zodiac_house;
		cusp_lower   = _cusp_lower;
		cusp_higher  = _cusp_higher;
		part         = _part;
		ruler_index  = 0;
		_longitude   = _zodiac_house.Origin;
	}

	public string Description
	{
		get
		{
			if (string.IsNullOrEmpty(_description) == false)
			{
				return _description;
			}

			return name.ToShortString();
		}
		set => _description = value;
	}

	public Longitude Longitude
	{
		get => _longitude;
		set
		{
			_longitude = value;
			if (type == Body.Type.Graha || type == Body.Type.Lagna)
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

	public override string ToString()
	{
		return string.Format("{0} ({1})", Description, zodiac_house);
	}

	public bool isInMoolaTrikona()
	{
		switch (name)
		{
			case Body.BodyType.Sun:
				if (zodiac_house.Sign == ZodiacHouse.Rasi.Leo)
				{
					return true;
				}

				break;
			case Body.BodyType.Moon:
				if (zodiac_house.Sign == ZodiacHouse.Rasi.Tau)
				{
					return true;
				}

				break;
			case Body.BodyType.Mars:
				if (zodiac_house.Sign == ZodiacHouse.Rasi.Ari)
				{
					return true;
				}

				break;
			case Body.BodyType.Mercury:
				if (zodiac_house.Sign == ZodiacHouse.Rasi.Vir)
				{
					return true;
				}

				break;
			case Body.BodyType.Jupiter:
				if (zodiac_house.Sign == ZodiacHouse.Rasi.Sag)
				{
					return true;
				}

				break;
			case Body.BodyType.Venus:
				if (zodiac_house.Sign == ZodiacHouse.Rasi.Lib)
				{
					return true;
				}

				break;
			case Body.BodyType.Saturn:
				if (zodiac_house.Sign == ZodiacHouse.Rasi.Aqu)
				{
					return true;
				}

				break;
			case Body.BodyType.Rahu:
				if (zodiac_house.Sign == ZodiacHouse.Rasi.Vir)
				{
					return true;
				}

				break;
			case Body.BodyType.Ketu:
				if (zodiac_house.Sign == ZodiacHouse.Rasi.Pis)
				{
					return true;
				}

				break;
		}

		return false;
	}

	public bool isInOwnHouse()
	{
		var zh = zodiac_house.Sign;
		switch (name)
		{
			case Body.BodyType.Sun:
				if (zh == ZodiacHouse.Rasi.Leo)
				{
					return true;
				}

				break;
			case Body.BodyType.Moon:
				if (zh == ZodiacHouse.Rasi.Tau)
				{
					return true;
				}

				break;
			case Body.BodyType.Mars:
				if (zh == ZodiacHouse.Rasi.Ari || zh == ZodiacHouse.Rasi.Sco)
				{
					return true;
				}

				break;
			case Body.BodyType.Mercury:
				if (zh == ZodiacHouse.Rasi.Gem || zh == ZodiacHouse.Rasi.Vir)
				{
					return true;
				}

				break;
			case Body.BodyType.Jupiter:
				if (zh == ZodiacHouse.Rasi.Sag || zh == ZodiacHouse.Rasi.Pis)
				{
					return true;
				}

				break;
			case Body.BodyType.Venus:
				if (zh == ZodiacHouse.Rasi.Tau || zh == ZodiacHouse.Rasi.Lib)
				{
					return true;
				}

				break;
			case Body.BodyType.Saturn:
				if (zh == ZodiacHouse.Rasi.Cap || zh == ZodiacHouse.Rasi.Aqu)
				{
					return true;
				}

				break;
			case Body.BodyType.Rahu:
				if (zh == ZodiacHouse.Rasi.Aqu)
				{
					return true;
				}

				break;
			case Body.BodyType.Ketu:
				if (zh == ZodiacHouse.Rasi.Sco)
				{
					return true;
				}

				break;
		}

		return false;
	}

	public bool isExaltedPhalita()
	{
		switch (name)
		{
			case Body.BodyType.Sun:
				if (zodiac_house.Sign == ZodiacHouse.Rasi.Ari)
				{
					return true;
				}

				break;
			case Body.BodyType.Moon:
				if (zodiac_house.Sign == ZodiacHouse.Rasi.Tau)
				{
					return true;
				}

				break;
			case Body.BodyType.Mars:
				if (zodiac_house.Sign == ZodiacHouse.Rasi.Cap)
				{
					return true;
				}

				break;
			case Body.BodyType.Mercury:
				if (zodiac_house.Sign == ZodiacHouse.Rasi.Vir)
				{
					return true;
				}

				break;
			case Body.BodyType.Jupiter:
				if (zodiac_house.Sign == ZodiacHouse.Rasi.Can)
				{
					return true;
				}

				break;
			case Body.BodyType.Venus:
				if (zodiac_house.Sign == ZodiacHouse.Rasi.Pis)
				{
					return true;
				}

				break;
			case Body.BodyType.Saturn:
				if (zodiac_house.Sign == ZodiacHouse.Rasi.Lib)
				{
					return true;
				}

				break;
			case Body.BodyType.Rahu:
				if (zodiac_house.Sign == ZodiacHouse.Rasi.Gem)
				{
					return true;
				}

				break;
			case Body.BodyType.Ketu:
				if (zodiac_house.Sign == ZodiacHouse.Rasi.Sag)
				{
					return true;
				}

				break;
		}

		return false;
	}

	public bool isDebilitatedPhalita()
	{
		switch (name)
		{
			case Body.BodyType.Sun:
				if (zodiac_house.Sign == ZodiacHouse.Rasi.Lib)
				{
					return true;
				}

				break;
			case Body.BodyType.Moon:
				if (zodiac_house.Sign == ZodiacHouse.Rasi.Sco)
				{
					return true;
				}

				break;
			case Body.BodyType.Mars:
				if (zodiac_house.Sign == ZodiacHouse.Rasi.Can)
				{
					return true;
				}

				break;
			case Body.BodyType.Mercury:
				if (zodiac_house.Sign == ZodiacHouse.Rasi.Pis)
				{
					return true;
				}

				break;
			case Body.BodyType.Jupiter:
				if (zodiac_house.Sign == ZodiacHouse.Rasi.Cap)
				{
					return true;
				}

				break;
			case Body.BodyType.Venus:
				if (zodiac_house.Sign == ZodiacHouse.Rasi.Vir)
				{
					return true;
				}

				break;
			case Body.BodyType.Saturn:
				if (zodiac_house.Sign == ZodiacHouse.Rasi.Ari)
				{
					return true;
				}

				break;
			case Body.BodyType.Rahu:
				if (zodiac_house.Sign == ZodiacHouse.Rasi.Sag)
				{
					return true;
				}

				break;
			case Body.BodyType.Ketu:
				if (zodiac_house.Sign == ZodiacHouse.Rasi.Gem)
				{
					return true;
				}

				break;
		}

		return false;
	}

	public bool GrahaDristi(ZodiacHouse h)
	{
		var num = zodiac_house.NumHousesBetween(h);
		if (num == 7)
		{
			return true;
		}

		if (name == Body.BodyType.Jupiter && (num == 5 || num == 9))
		{
			return true;
		}

		if (name == Body.BodyType.Rahu && (num == 5 || num == 9 || num == 2))
		{
			return true;
		}

		if (name == Body.BodyType.Mars && (num == 4 || num == 8))
		{
			return true;
		}

		if (name == Body.BodyType.Saturn && (num == 3 || num == 10))
		{
			return true;
		}

		return false;
	}
}