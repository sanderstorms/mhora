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
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using Mhora.Components.Delegates;
using Mhora.Components.Property;
using Mhora.Elements.Calculation;
using Mhora.Elements.Hora;
using Newtonsoft.Json;

namespace Mhora.Database.Settings;

[Serializable]
public class MhoraSerializableOptions
{
	public static string GetExeDir()
	{
		var oLocal   = Process.GetCurrentProcess();
		var oMain    = oLocal.MainModule;
		var fileName = Path.GetDirectoryName(oMain.FileName);
		if (fileName[fileName.Length - 1] == '\\')
		{
			fileName.Remove(fileName.Length - 1, 1);
		}

		//Debug.WriteLine( string.Format("Exe launched from {0}", fileName), "GlobalOptions");
		return fileName;
	}

	public static string GetOptsFilename()
	{
		var fileName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\MhoraOptions.json";
		//Debug.WriteLine( string.Format("Options stored at {0}", fileName), "GlobalOptions");
		return fileName;
	}
}

[JsonObject]
public class StrengthOptions : MhoraSerializableOptions, ICloneable
{
	private FindStronger.EGrahaStrength[] _mColord;
	private FindStronger.EGrahaStrength[] _mKarakaKendradiGrahaDasaColord;
	private FindStronger.EGrahaStrength[] _mKarakaKendradiGrahaDasaGraha;
	private FindStronger.ERasiStrength[]  _mKarakaKendradiGrahaDasaRasi;
	private FindStronger.ERasiStrength[]  _mMoolaDasaRasi;
	private FindStronger.EGrahaStrength[] _mNaisargikaDasaGraha;
	private FindStronger.ERasiStrength[]  _mNaisargikaDasaRasi;
	private FindStronger.ERasiStrength[]  _mNarayanaDasaRasi;
	private FindStronger.ERasiStrength[]  _mNavamsaDasaRasi;


	public StrengthOptions()
	{
		Colord = new[]
		{
			FindStronger.EGrahaStrength.NotInOwnHouse,
			FindStronger.EGrahaStrength.AspectsRasi,
			FindStronger.EGrahaStrength.Exaltation,
			FindStronger.EGrahaStrength.RasisNature,
			FindStronger.EGrahaStrength.NarayanaDasaLength,
			FindStronger.EGrahaStrength.Longitude
		};

		NaisargikaDasaGraha = new[]
		{
			FindStronger.EGrahaStrength.Exaltation,
			FindStronger.EGrahaStrength.LordInOwnHouse,
			FindStronger.EGrahaStrength.MoolaTrikona,
			FindStronger.EGrahaStrength.Longitude
		};

		KarakaKendradiGrahaDasaGraha = new[]
		{
			FindStronger.EGrahaStrength.Exaltation,
			FindStronger.EGrahaStrength.MoolaTrikona,
			FindStronger.EGrahaStrength.OwnHouse,
			FindStronger.EGrahaStrength.Longitude
		};

		KarakaKendradiGrahaDasaRasi = new[]
		{
			FindStronger.ERasiStrength.Conjunction,
			FindStronger.ERasiStrength.AspectsRasi,
			FindStronger.ERasiStrength.Exaltation,
			FindStronger.ERasiStrength.MoolaTrikona,
			FindStronger.ERasiStrength.OwnHouse,
			FindStronger.ERasiStrength.LordsNature,
			FindStronger.ERasiStrength.AtmaKaraka,
			FindStronger.ERasiStrength.Longitude,
			FindStronger.ERasiStrength.LordInDifferentOddity,
			FindStronger.ERasiStrength.KarakaKendradiGrahaDasaLength
		};
		KarakaKendradiGrahaDasaColord = new[]
		{
			FindStronger.EGrahaStrength.NotInOwnHouse,
			FindStronger.EGrahaStrength.Conjunction,
			FindStronger.EGrahaStrength.AspectsRasi,
			FindStronger.EGrahaStrength.Exaltation,
			FindStronger.EGrahaStrength.MoolaTrikona,
			FindStronger.EGrahaStrength.OwnHouse,
			FindStronger.EGrahaStrength.LordsNature,
			FindStronger.EGrahaStrength.AtmaKaraka,
			FindStronger.EGrahaStrength.Longitude,
			FindStronger.EGrahaStrength.LordInDifferentOddity,
			FindStronger.EGrahaStrength.KarakaKendradiGrahaDasaLength
		};

		NavamsaDasaRasi = new[]
		{
			FindStronger.ERasiStrength.AspectsRasi,
			FindStronger.ERasiStrength.Conjunction,
			FindStronger.ERasiStrength.Exaltation,
			FindStronger.ERasiStrength.LordInDifferentOddity,
			FindStronger.ERasiStrength.RasisNature,
			FindStronger.ERasiStrength.LordsLongitude
		};

		MoolaDasaRasi = new[]
		{
			FindStronger.ERasiStrength.Conjunction,
			FindStronger.ERasiStrength.Exaltation,
			FindStronger.ERasiStrength.MoolaTrikona,
			FindStronger.ERasiStrength.OwnHouse,
			FindStronger.ERasiStrength.RasisNature,
			FindStronger.ERasiStrength.LordsLongitude
		};

		NarayanaDasaRasi = new[]
		{
			FindStronger.ERasiStrength.Conjunction,
			FindStronger.ERasiStrength.AspectsRasi,
			FindStronger.ERasiStrength.Exaltation,
			FindStronger.ERasiStrength.LordInDifferentOddity,
			FindStronger.ERasiStrength.RasisNature,
			FindStronger.ERasiStrength.LordsLongitude
		};

		NaisargikaDasaRasi = new[]
		{
			FindStronger.ERasiStrength.Conjunction,
			FindStronger.ERasiStrength.AspectsRasi,
			FindStronger.ERasiStrength.Exaltation,
			FindStronger.ERasiStrength.RasisNature,
			FindStronger.ERasiStrength.LordIsAtmaKaraka,
			FindStronger.ERasiStrength.LordInDifferentOddity,
			FindStronger.ERasiStrength.Longitude
		};
	}

	[Category("Co-Lord Strengths")]
	[PGDisplayName("Graha Strength")]
	public FindStronger.EGrahaStrength[] Colord
	{
		get => _mColord;
		set => _mColord = value;
	}

	[Category("Naisargika Dasa Strengths")]
	[PGDisplayName("Graha Strengths")]
	public FindStronger.EGrahaStrength[] NaisargikaDasaGraha
	{
		get => _mNaisargikaDasaGraha;
		set => _mNaisargikaDasaGraha = value;
	}

	[Category("Naisargika Dasa Strengths")]
	[PGDisplayName("Rasi Strengths")]
	public FindStronger.ERasiStrength[] NaisargikaDasaRasi
	{
		get => _mNaisargikaDasaRasi;
		set => _mNaisargikaDasaRasi = value;
	}

	[Category("Navamsa Dasa Strengths")]
	[PGDisplayName("Rasi Strengths")]
	public FindStronger.ERasiStrength[] NavamsaDasaRasi
	{
		get => _mNavamsaDasaRasi;
		set => _mNavamsaDasaRasi = value;
	}

	[Category("Moola Dasa Strengths")]
	[PGDisplayName("Rasi Strengths")]
	public FindStronger.ERasiStrength[] MoolaDasaRasi
	{
		get => _mMoolaDasaRasi;
		set => _mMoolaDasaRasi = value;
	}

	[Category("Narayana Dasa Strengths")]
	[PGDisplayName("Rasi Strengths")]
	public FindStronger.ERasiStrength[] NarayanaDasaRasi
	{
		get => _mNarayanaDasaRasi;
		set => _mNarayanaDasaRasi = value;
	}

	[Category("Karakas Kendradi Graha Dasa")]
	[PGDisplayName("Rasi Strengths")]
	public FindStronger.ERasiStrength[] KarakaKendradiGrahaDasaRasi
	{
		get => _mKarakaKendradiGrahaDasaRasi;
		set => _mKarakaKendradiGrahaDasaRasi = value;
	}

	[Category("Karakas Kendradi Graha Dasa")]
	[PGDisplayName("Graha Strengths")]
	public FindStronger.EGrahaStrength[] KarakaKendradiGrahaDasaGraha
	{
		get => _mKarakaKendradiGrahaDasaGraha;
		set => _mKarakaKendradiGrahaDasaGraha = value;
	}

	[PGNotVisible]
	[Category("Karakas Kendradi Graha Dasa")]
	[PGDisplayName("CoLord Strengths")]
	[TypeConverter(typeof(MhoraArrayConverter))]
	public FindStronger.EGrahaStrength[] KarakaKendradiGrahaDasaColord
	{
		get => _mKarakaKendradiGrahaDasaColord;
		set => _mKarakaKendradiGrahaDasaColord = value;
	}

	public object Clone()
	{
		var opts = new StrengthOptions();
		opts.Colord              = (FindStronger.EGrahaStrength[]) Colord.Clone();
		opts.NaisargikaDasaGraha = (FindStronger.EGrahaStrength[]) NaisargikaDasaGraha.Clone();
		opts.NavamsaDasaRasi     = (FindStronger.ERasiStrength[]) NavamsaDasaRasi.Clone();
		opts.MoolaDasaRasi       = (FindStronger.ERasiStrength[]) MoolaDasaRasi.Clone();
		opts.NarayanaDasaRasi    = (FindStronger.ERasiStrength[]) NarayanaDasaRasi.Clone();
		opts.NaisargikaDasaRasi  = (FindStronger.ERasiStrength[]) NaisargikaDasaRasi.Clone();
		return opts;
	}

	public object Copy(object o)
	{
		var so = (StrengthOptions) o;
		Colord              = (FindStronger.EGrahaStrength[]) so.Colord.Clone();
		NaisargikaDasaGraha = (FindStronger.EGrahaStrength[]) so.NaisargikaDasaGraha.Clone();
		NavamsaDasaRasi     = (FindStronger.ERasiStrength[]) so.NavamsaDasaRasi.Clone();
		MoolaDasaRasi       = (FindStronger.ERasiStrength[]) so.MoolaDasaRasi.Clone();
		NarayanaDasaRasi    = (FindStronger.ERasiStrength[]) so.NarayanaDasaRasi.Clone();
		NaisargikaDasaRasi  = (FindStronger.ERasiStrength[]) so.NaisargikaDasaRasi.Clone();
		return Clone();
	}
}