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

		length = Basics.NormalizeInc(length - 1, 1, 12);
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

	public class Options : ICloneable
	{
		private double _Compression;
		private double _YearLength;

		public Options()
		{
			YearType     = ToDate.DateType.SolarYear;
			_YearLength  = 360.0;
			_Compression = 0.0;
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
			get => _Compression;
			set
			{
				if (value >= 0.0)
				{
					_Compression = value;
				}
			}
		}

		[PGDisplayName("Year Length")]
		public double YearLength
		{
			get => _YearLength;
			set
			{
				if (value >= 0.0)
				{
					_YearLength = value;
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
			var o = new Options();
			o.YearLength    = YearLength;
			o.YearType      = YearType;
			o.Compression   = Compression;
			o.OffsetDays    = OffsetDays;
			o.OffsetHours   = OffsetHours;
			o.OffsetMinutes = OffsetMinutes;
			return o;
		}

		public void Copy(Options o)
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