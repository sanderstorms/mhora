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
using System.Runtime.Serialization;
using Mhora.Components.Property;
using Mhora.Elements;
using Mhora.Util;
using Newtonsoft.Json;

namespace Mhora.Database.Settings;

[JsonObject]
public class HoroscopeOptions : MhoraSerializableOptions, ICloneable
{
	public enum AyanamsaType
	{
		Fagan              = 0,
		Lahiri             = 1,
		Raman              = 3,
		Ushashashi         = 4,
		Krishnamurti       = 5,
		Suryasiddhanta     = 21,
		SuryasiddhantaMsun = 22,
		Citra              = 26,
		TrueCitra          = 27
	}

	[TypeConverter(typeof(EnumDescConverter))]
	public enum EBhavaType
	{
		[Description("Lagna is at the beginning of the bhava")]
		Start,

		[Description("Lagna is in the middle of the bhava")]
		Middle
	}

	public enum EGrahaPositionType
	{
		Apparent,
		True
	}

	[TypeConverter(typeof(EnumDescConverter))]
	public enum EHoraType
	{
		[Description("Day split into equal parts")]
		Sunriset,

		[Description("Daytime and Nighttime split into equal parts")]
		SunrisetEqual,

		[Description("Day (Local Mean Time) split into equal parts")]
		Lmt
	}

	[TypeConverter(typeof(EnumDescConverter))]
	public enum EMaandiType
	{
		[Description("Rises at the beginning of Saturn's portion")]
		SaturnBegin,

		[Description("Rises in the middle of Saturn's portion")]
		SaturnMid,

		[Description("Rises at the end of Saturn's portion")]
		SaturnEnd,

		[Description("Rises at the beginning of the lordless portion")]
		LordlessBegin,

		[Description("Rises in the middle of the lordless portion")]
		LordlessMid,

		[Description("Rises at the end of the lordless portion")]
		LordlessEnd
	}

	[TypeConverter(typeof(EnumDescConverter))]
	public enum ENodeType
	{
		[Description("Mean Positions of nodes")]
		Mean,

		[Description("True Positions of nodes")]
		True
	}

	[TypeConverter(typeof(EnumDescConverter))]
	public enum EUpagrahaType
	{
		[Description("Rises at the beginning of the grahas portion")]
		Begin,

		[Description("Rises in the middle of the grahas portion")]
		Mid,

		[Description("Rises at the end of the grahas portion")]
		End
	}

	[TypeConverter(typeof(EnumDescConverter))]
	public enum SunrisePositionType
	{
		[Description("Sun's center rises")]
		TrueDiscCenter,

		[Description("Sun's edge rises")]
		TrueDiscEdge,

		[Description("Sun's center appears to rise")]
		ApparentDiscCenter,

		[Description("Sun's edge apprears to rise")]
		ApparentDiscEdge,

		[Description("6am Local Mean Time")]
		Lmt
	}

	protected const string              CatGeneral  = "1: General Settings";
	protected const string              CatGraha    = "2: Graha Settings";
	protected const string              CatSunrise  = "3: Sunrise Settings";
	protected const string              CatUpagraha = "4: Upagraha Settings";
	public          EGrahaPositionType  GrahaPositionType;
	private         AyanamsaType        _mAyanamsa;
	private         Angle               _mAyanamsaOffset;
	private         EBhavaType          _mBhavaType;
	private         string              _mEphemPath;
	private         EMaandiType         _mGulikaType;
	private         EHoraType           _mHoraType;
	private         EHoraType           _mKalaType;
	private         EMaandiType         _mMaandiType;
	private         SunrisePositionType _mSunrisePosition;
	private         EUpagrahaType       _mUpagrahaType;
	private         Angle               _mUserLongitude;
	private         ENodeType           _nodeType;

	public HoroscopeOptions()
	{
		SunrisePosition   = SunrisePositionType.TrueDiscCenter;
		_mHoraType        = EHoraType.Lmt;
		_mKalaType        = EHoraType.Sunriset;
		_mBhavaType       = EBhavaType.Start;
		GrahaPositionType = EGrahaPositionType.True;
		_nodeType         = ENodeType.Mean;
		Ayanamsa          = AyanamsaType.TrueCitra;
		AyanamsaOffset    = Angle.Empty;
		_mUserLongitude   = Angle.Empty;
		MaandiType        = EMaandiType.SaturnBegin;
		GulikaType        = EMaandiType.SaturnMid;
		UpagrahaType      = EUpagrahaType.Mid;
		_mEphemPath       = GetExeDir() + "\\eph";
	}

	[Category(CatGeneral)]
	[PropertyOrder(1)]
	[PGDisplayName("Full Ephemeris Path")]
	public string EphemerisPath
	{
		get => _mEphemPath;
		set => _mEphemPath = value;
	}

	[PropertyOrder(2)]
	[Category(CatGeneral)]
	public AyanamsaType Ayanamsa
	{
		get => _mAyanamsa;
		set => _mAyanamsa = value;
	}

	[PropertyOrder(4)]
	[Category(CatGeneral)]
	[PGDisplayName("Custom Longitude")]
	public Angle CustomBodyLongitude
	{
		get => _mUserLongitude;
		set => _mUserLongitude = value;
	}

	[Category(CatGeneral)]
	[PropertyOrder(3)]
	[PGDisplayName("Ayanamsa Offset")]
	public Angle AyanamsaOffset
	{
		get => _mAyanamsaOffset;
		set => _mAyanamsaOffset = value;
	}

	[Category(CatUpagraha)]
	[PropertyOrder(1)]
	[PGDisplayName("Upagraha")]
	public EUpagrahaType UpagrahaType
	{
		get => _mUpagrahaType;
		set => _mUpagrahaType = _mUpagrahaType;
	}

	[Category(CatUpagraha)]
	[PropertyOrder(2)]
	[PGDisplayName("Maandi")]
	public EMaandiType MaandiType
	{
		get => _mMaandiType;
		set => _mMaandiType = value;
	}

	[Category(CatUpagraha)]
	[PropertyOrder(3)]
	[PGDisplayName("Gulika")]
	public EMaandiType GulikaType
	{
		get => _mGulikaType;
		set => _mGulikaType = value;
	}

	[Category(CatSunrise)]
	[PropertyOrder(1)]
	[PGDisplayName("Sunrise")]
	public SunrisePositionType SunrisePosition
	{
		get => _mSunrisePosition;
		set => _mSunrisePosition = value;
	}

	[Category(CatSunrise)]
	[PropertyOrder(2)]
	[PGDisplayName("Hora")]
	public EHoraType HoraType
	{
		get => _mHoraType;
		set => _mHoraType = value;
	}

	[Category(CatSunrise)]
	[PropertyOrder(3)]
	[PGDisplayName("Kala")]
	public EHoraType KalaType
	{
		get => _mKalaType;
		set => _mKalaType = value;
	}

	//public EGrahaPositionType GrahaPositionType
	//{
	//	get { return grahaPositionType; }
	//	set { grahaPositionType = value; }
	//}
	[Category(CatGraha)]
	[PropertyOrder(1)]
	[PGDisplayName("Rahu / Ketu")]
	public ENodeType NodeType
	{
		get => _nodeType;
		set => _nodeType = value;
	}

	[Category(CatGraha)]
	[PropertyOrder(2)]
	[PGDisplayName("Bhava")]
	public EBhavaType BhavaType
	{
		get => _mBhavaType;
		set => _mBhavaType = value;
	}

	public object Clone()
	{
		var o = new HoroscopeOptions();
		o.SunrisePosition   = SunrisePosition;
		o.GrahaPositionType = GrahaPositionType;
		o._nodeType          = _nodeType;
		o.Ayanamsa          = Ayanamsa;
		o.AyanamsaOffset    = AyanamsaOffset;
		o.HoraType          = HoraType;
		o.KalaType          = KalaType;
		o.BhavaType         = BhavaType;
		o._mUserLongitude    = _mUserLongitude;
		o.MaandiType        = MaandiType;
		o.GulikaType        = GulikaType;
		o.UpagrahaType      = UpagrahaType;
		o.EphemerisPath     = EphemerisPath;
		return o;
	}

	public void Copy(HoroscopeOptions o)
	{
		SunrisePosition   = o.SunrisePosition;
		GrahaPositionType = o.GrahaPositionType;
		_nodeType          = o._nodeType;
		Ayanamsa          = o.Ayanamsa;
		AyanamsaOffset    = o.AyanamsaOffset;
		HoraType          = o.HoraType;
		KalaType          = o.KalaType;
		BhavaType         = o.BhavaType;
		_mUserLongitude    = o._mUserLongitude;
		MaandiType        = o.MaandiType;
		GulikaType        = o.GulikaType;
		UpagrahaType      = o.UpagrahaType;
		EphemerisPath     = o.EphemerisPath;
	}
}