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
using System.ComponentModel;
using Mhora.Components.Delegates;
using Mhora.Components.Property;
using Mhora.Definitions;
using Mhora.Elements;

namespace Mhora.Components.TransitControl;

public class TransitSearchOptions : ICloneable
{
	public enum EForward
	{
		Before,
		After
	}

	public TransitSearchOptions()
	{
		StartDate    = DateTime.Now;
		SearchBody   = Body.Sun;
		TransitPoint = new Longitude(0.0);
		Forward      = true;
		Division     = DivisionType.Rasi;
	}


	[Category("Transit Search")]
	[PropertyOrder(1)]
	[PGDisplayName("In Vargas")]
	public DivisionType Division
	{
		get;
		set;
	}

	[Category("Transit Search")]
	[PropertyOrder(2)]
	[PGDisplayName("Search")]
	public EForward UIForward
	{
		get
		{
			if (Forward)
			{
				return EForward.After;
			}

			return EForward.Before;
		}
		set
		{
			if (value == EForward.After)
			{
				Forward = true;
			}
			else
			{
				Forward = false;
			}
		}
	}

	[PGNotVisible]
	public bool Forward
	{
		get;
		set;
	}

	[Category("Transit Search")]
	[PropertyOrder(3)]
	[PGDisplayName("Date")]
	public DateTime StartDate
	{
		get;
		set;
	}

	[Category("Transit Search")]
	[PropertyOrder(4)]
	[PGDisplayName("When Body")]
	public Body SearchBody
	{
		get;
		set;
	}

	[Category("Transit Search")]
	[PropertyOrder(5)]
	[PGDisplayName("Transits")]
	public Longitude TransitPoint
	{
		get;
		set;
	}

	[Category("Transit Search")]
	[PropertyOrder(6)]
	[PGDisplayName("Apply Locally")]
	public bool Apply
	{
		get;
		set;
	}

#region ICloneable Members

	public object Clone()
	{
		// TODO:  Add TransitSearchOptions.Clone implementation
		var ret = new TransitSearchOptions
		{
			StartDate = StartDate,
			Forward = Forward,
			SearchBody = SearchBody,
			TransitPoint = TransitPoint
		};
		return ret;
	}

	public object CopyFrom(object o)
	{
		var nopt = (TransitSearchOptions) o;
		StartDate    = nopt.StartDate;
		Forward      = nopt.Forward;
		SearchBody   = nopt.SearchBody;
		TransitPoint = nopt.TransitPoint;
		return Clone();
	}

#endregion
}