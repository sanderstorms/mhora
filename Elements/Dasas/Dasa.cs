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
using Mhora.Components.Delegates;
using Mhora.Components.Property;
using Mhora.Definitions;
using Mhora.Util;

namespace Mhora.Elements.Dasas;

public abstract class Dasa
{
	public Recalculate RecalculateEvent;

	public static int NarayanaDasaLength(ZodiacHouse zh, DivisionPosition dp)
	{
		var length = 0;

		if (zh.IsOddFooted())
		{
			length = zh.NumHousesBetween(dp.ZodiacHouse);
		}
		else
		{
			length = zh.NumHousesBetweenReverse(dp.ZodiacHouse);
		}

		if (dp.IsExaltedPhalita())
		{
			length++;
		}
		else if (dp.IsDebilitatedPhalita())
		{
			length--;
		}

		length = (length - 1).NormalizeInc(1, 12);
		return length;
	}


	public event EvtChanged Changed;

	public void DivisionChanged(Division d)
	{
	}

	public void OnChanged()
	{
		Changed?.Invoke(this);
	}

	public string EntryDescription(DasaEntry de, DateTime start, DateTime end)
	{
		return string.Empty;
	}

	public class DasaOptions : ICloneable
	{
		private double _compression;
		private double _yearLength;

		public DasaOptions()
		{
			YearType     = ToDate.DateType.SolarYear;
			_yearLength  = 360.0;
			_compression = 0.0;
		}

		[PGDisplayName("Year Type")]
		public ToDate.DateType YearType
		{
			get;
			set;
		}

		[PGDisplayName("Dasa Compression")]
		public double Compression
		{
			get => _compression;
			set
			{
				if (value >= 0.0)
				{
					_compression = value;
				}
			}
		}

		[PGDisplayName("Year Length")]
		public double YearLength
		{
			get => _yearLength;
			set
			{
				if (value >= 0.0)
				{
					_yearLength = value;
				}
			}
		}

		[PGDisplayName("Offset Dates by Days")]
		public double OffsetDays
		{
			get;
			set;
		}

		[PGDisplayName("Offset Dates by Hours")]
		public double OffsetHours
		{
			get;
			set;
		}

		[PGDisplayName("Offset Dates by Minutes")]
		public double OffsetMinutes
		{
			get;
			set;
		}

		public object Clone()
		{
			var o = new DasaOptions();
			o.YearLength    = YearLength;
			o.YearType      = YearType;
			o.Compression   = Compression;
			o.OffsetDays    = OffsetDays;
			o.OffsetHours   = OffsetHours;
			o.OffsetMinutes = OffsetMinutes;
			return o;
		}

		public void Copy(DasaOptions o)
		{
			YearLength    = o.YearLength;
			YearType      = o.YearType;
			Compression   = o.Compression;
			OffsetDays    = o.OffsetDays;
			OffsetHours   = o.OffsetHours;
			OffsetMinutes = o.OffsetMinutes;
		}
	}
}