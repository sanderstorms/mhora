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
using Mhora.Calculation;
using Mhora.Components.Delegates;
using Mhora.Components.Property;
using Mhora.Definitions;
using Mhora.Elements;
using Mhora.Elements.Extensions;
using Mhora.Util;

namespace Mhora.Dasas;

public abstract class Dasa
{
	public Recalculate RecalculateEvent;

	public static int NarayanaDasaLength(ZodiacHouse zh, Graha graha)
	{
		var length = 0;

		if (zh.IsOddFooted())
		{
			length = zh.NumHousesBetween(graha.Rashi);
		}
		else
		{
			length = zh.NumHousesBetweenReverse(graha.Rashi);
		}

		if (graha.IsExalted)
		{
			length++;
		}
		else if (graha.IsDebilitated)
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

	public string EntryDescription(DasaEntry de, DateTime start, DateTime end) => string.Empty;

	public class DasaOptions : ICloneable
	{
		private double _compression;
		private double _yearLength;

		public DasaOptions()
		{
			YearType     = DateType.SolarYear;
			_yearLength  = 360.0;
			_compression = 0.0;
		}

		[PGDisplayName("Year Type")]
		public DateType YearType
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
				if (_yearLength >= 0.0)
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


		[PGDisplayName("Offset (hh:mm:ss)")]
		public TimeSpan Offset
		{
			get;
			set;
		}

		public object Clone()
		{
			var o = new DasaOptions
			{
				YearLength = YearLength,
				YearType = YearType,
				Compression = Compression,
				OffsetDays = OffsetDays,
				Offset = Offset
			};
			return o;
		}

		public void Copy(DasaOptions o)
		{
			YearLength   = o.YearLength;
			YearType     = o.YearType;
			Compression  = o.Compression;
			OffsetDays   = o.OffsetDays;
			Offset       = o.Offset;
		}
	}
}