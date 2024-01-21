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

	public double        CuspHigher  { get; set; }
	public double        CuspLower   { get; set; }
	public Body.BodyType Name        { get; set; }
	public int           Part        { get; set; }
	public int           RulerIndex  { get; set; }
	public Body.Type     Type        { get; set; }
	public ZodiacHouse   ZodiacHouse { get; set; }

	public DivisionPosition(Body.BodyType name, Body.Type type, ZodiacHouse zodiacHouse, double cuspLower, double cuspHigher, int part)
	{
		Name         = name;
		Type         = type;
		ZodiacHouse = zodiacHouse;
		CuspLower   = cuspLower;
		CuspHigher  = cuspHigher;
		Part         = part;
		RulerIndex  = 0;
		_longitude   = zodiacHouse.Origin;
	}

	public string Description
	{
		get
		{
			if (string.IsNullOrEmpty(_description) == false)
			{
				return _description;
			}

			return Name.ToShortString();
		}
		set => _description = value;
	}

	public Longitude Longitude
	{
		get => _longitude;
		set
		{
			_longitude = value;
			if (Type == Body.Type.Graha || Type == Body.Type.Lagna)
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
		return string.Format("{0} ({1})", Description, ZodiacHouse);
	}

	public bool IsInMoolaTrikona()
	{
		switch (Name)
		{
			case Body.BodyType.Sun:
				if (ZodiacHouse.Sign == ZodiacHouse.Rasi.Leo)
				{
					return true;
				}

				break;
			case Body.BodyType.Moon:
				if (ZodiacHouse.Sign == ZodiacHouse.Rasi.Tau)
				{
					return true;
				}

				break;
			case Body.BodyType.Mars:
				if (ZodiacHouse.Sign == ZodiacHouse.Rasi.Ari)
				{
					return true;
				}

				break;
			case Body.BodyType.Mercury:
				if (ZodiacHouse.Sign == ZodiacHouse.Rasi.Vir)
				{
					return true;
				}

				break;
			case Body.BodyType.Jupiter:
				if (ZodiacHouse.Sign == ZodiacHouse.Rasi.Sag)
				{
					return true;
				}

				break;
			case Body.BodyType.Venus:
				if (ZodiacHouse.Sign == ZodiacHouse.Rasi.Lib)
				{
					return true;
				}

				break;
			case Body.BodyType.Saturn:
				if (ZodiacHouse.Sign == ZodiacHouse.Rasi.Aqu)
				{
					return true;
				}

				break;
			case Body.BodyType.Rahu:
				if (ZodiacHouse.Sign == ZodiacHouse.Rasi.Vir)
				{
					return true;
				}

				break;
			case Body.BodyType.Ketu:
				if (ZodiacHouse.Sign == ZodiacHouse.Rasi.Pis)
				{
					return true;
				}

				break;
		}

		return false;
	}

	public bool IsInOwnHouse()
	{
		var zh = ZodiacHouse.Sign;
		switch (Name)
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

	public bool IsExaltedPhalita()
	{
		switch (Name)
		{
			case Body.BodyType.Sun:
				if (ZodiacHouse.Sign == ZodiacHouse.Rasi.Ari)
				{
					return true;
				}

				break;
			case Body.BodyType.Moon:
				if (ZodiacHouse.Sign == ZodiacHouse.Rasi.Tau)
				{
					return true;
				}

				break;
			case Body.BodyType.Mars:
				if (ZodiacHouse.Sign == ZodiacHouse.Rasi.Cap)
				{
					return true;
				}

				break;
			case Body.BodyType.Mercury:
				if (ZodiacHouse.Sign == ZodiacHouse.Rasi.Vir)
				{
					return true;
				}

				break;
			case Body.BodyType.Jupiter:
				if (ZodiacHouse.Sign == ZodiacHouse.Rasi.Can)
				{
					return true;
				}

				break;
			case Body.BodyType.Venus:
				if (ZodiacHouse.Sign == ZodiacHouse.Rasi.Pis)
				{
					return true;
				}

				break;
			case Body.BodyType.Saturn:
				if (ZodiacHouse.Sign == ZodiacHouse.Rasi.Lib)
				{
					return true;
				}

				break;
			case Body.BodyType.Rahu:
				if (ZodiacHouse.Sign == ZodiacHouse.Rasi.Gem)
				{
					return true;
				}

				break;
			case Body.BodyType.Ketu:
				if (ZodiacHouse.Sign == ZodiacHouse.Rasi.Sag)
				{
					return true;
				}

				break;
		}

		return false;
	}

	public bool IsDebilitatedPhalita()
	{
		switch (Name)
		{
			case Body.BodyType.Sun:
				if (ZodiacHouse.Sign == ZodiacHouse.Rasi.Lib)
				{
					return true;
				}

				break;
			case Body.BodyType.Moon:
				if (ZodiacHouse.Sign == ZodiacHouse.Rasi.Sco)
				{
					return true;
				}

				break;
			case Body.BodyType.Mars:
				if (ZodiacHouse.Sign == ZodiacHouse.Rasi.Can)
				{
					return true;
				}

				break;
			case Body.BodyType.Mercury:
				if (ZodiacHouse.Sign == ZodiacHouse.Rasi.Pis)
				{
					return true;
				}

				break;
			case Body.BodyType.Jupiter:
				if (ZodiacHouse.Sign == ZodiacHouse.Rasi.Cap)
				{
					return true;
				}

				break;
			case Body.BodyType.Venus:
				if (ZodiacHouse.Sign == ZodiacHouse.Rasi.Vir)
				{
					return true;
				}

				break;
			case Body.BodyType.Saturn:
				if (ZodiacHouse.Sign == ZodiacHouse.Rasi.Ari)
				{
					return true;
				}

				break;
			case Body.BodyType.Rahu:
				if (ZodiacHouse.Sign == ZodiacHouse.Rasi.Sag)
				{
					return true;
				}

				break;
			case Body.BodyType.Ketu:
				if (ZodiacHouse.Sign == ZodiacHouse.Rasi.Gem)
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

		if (Name == Body.BodyType.Jupiter && (num == 5 || num == 9))
		{
			return true;
		}

		if (Name == Body.BodyType.Rahu && (num == 5 || num == 9 || num == 2))
		{
			return true;
		}

		if (Name == Body.BodyType.Mars && (num == 4 || num == 8))
		{
			return true;
		}

		if (Name == Body.BodyType.Saturn && (num == 3 || num == 10))
		{
			return true;
		}

		return false;
	}
}