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
using Mhora.Components.Delegates;
using Mhora.Components.File;
using Mhora.Components.Property;
using Mhora.Definitions;
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
public class StrengthOptions : MhoraSerializableOptions
{
	private GrahaStrength[] _mColord;
	private GrahaStrength[] _mKarakaKendradiGrahaDasaColord;
	private GrahaStrength[] _mKarakaKendradiGrahaDasaGraha;
	private RashiStrength[]  _mKarakaKendradiGrahaDasaRasi;
	private RashiStrength[]  _mMoolaDasaRasi;
	private GrahaStrength[] _mNaisargikaDasaGraha;
	private RashiStrength[]  _mNaisargikaDasaRasi;
	private RashiStrength[]  _mNarayanaDasaRasi;
	private RashiStrength[]  _mNavamsaDasaRasi;


	public StrengthOptions()
	{
		Colord = new[]
		{
			GrahaStrength.NotInOwnHouse,
			GrahaStrength.AspectsRasi,
			GrahaStrength.Exaltation,
			GrahaStrength.RasisNature,
			GrahaStrength.NarayanaDasaLength,
			GrahaStrength.Longitude
		};

		NaisargikaDasaGraha = new[]
		{
			GrahaStrength.Exaltation,
			GrahaStrength.LordInOwnHouse,
			GrahaStrength.MoolaTrikona,
			GrahaStrength.Longitude
		};

		KarakaKendradiGrahaDasaGraha = new[]
		{
			GrahaStrength.Exaltation,
			GrahaStrength.MoolaTrikona,
			GrahaStrength.OwnHouse,
			GrahaStrength.Longitude
		};

		KarakaKendradiGrahaDasaRasi = new[]
		{
			RashiStrength.Conjunction,
			RashiStrength.AspectsRasi,
			RashiStrength.Exaltation,
			RashiStrength.MoolaTrikona,
			RashiStrength.OwnHouse,
			RashiStrength.LordsNature,
			RashiStrength.AtmaKaraka,
			RashiStrength.Longitude,
			RashiStrength.LordInDifferentOddity,
			RashiStrength.KarakaKendradiGrahaDasaLength
		};
		KarakaKendradiGrahaDasaColord = new[]
		{
			GrahaStrength.NotInOwnHouse,
			GrahaStrength.Conjunction,
			GrahaStrength.AspectsRasi,
			GrahaStrength.Exaltation,
			GrahaStrength.MoolaTrikona,
			GrahaStrength.OwnHouse,
			GrahaStrength.LordsNature,
			GrahaStrength.AtmaKaraka,
			GrahaStrength.Longitude,
			GrahaStrength.LordInDifferentOddity,
			GrahaStrength.KarakaKendradiGrahaDasaLength
		};

		NavamsaDasaRasi = new[]
		{
			RashiStrength.AspectsRasi,
			RashiStrength.Conjunction,
			RashiStrength.Exaltation,
			RashiStrength.LordInDifferentOddity,
			RashiStrength.RasisNature,
			RashiStrength.LordsLongitude
		};

		MoolaDasaRasi = new[]
		{
			RashiStrength.Conjunction,
			RashiStrength.Exaltation,
			RashiStrength.MoolaTrikona,
			RashiStrength.OwnHouse,
			RashiStrength.RasisNature,
			RashiStrength.LordsLongitude
		};

		NarayanaDasaRasi = new[]
		{
			RashiStrength.Conjunction,
			RashiStrength.AspectsRasi,
			RashiStrength.Exaltation,
			RashiStrength.LordInDifferentOddity,
			RashiStrength.RasisNature,
			RashiStrength.LordsLongitude
		};

		NaisargikaDasaRasi = new[]
		{
			RashiStrength.Conjunction,
			RashiStrength.AspectsRasi,
			RashiStrength.Exaltation,
			RashiStrength.RasisNature,
			RashiStrength.LordIsAtmaKaraka,
			RashiStrength.LordInDifferentOddity,
			RashiStrength.Longitude
		};
	}

	[Category("Co-Lord Strengths")]
	[PGDisplayName("Graha Strength")]
	public GrahaStrength[] Colord
	{
		get => _mColord;
		set => _mColord = value;
	}

	[Category("Naisargika Dasa Strengths")]
	[PGDisplayName("Graha Strengths")]
	public GrahaStrength[] NaisargikaDasaGraha
	{
		get => _mNaisargikaDasaGraha;
		set => _mNaisargikaDasaGraha = value;
	}

	[Category("Naisargika Dasa Strengths")]
	[PGDisplayName("Rasi Strengths")]
	public RashiStrength[] NaisargikaDasaRasi
	{
		get => _mNaisargikaDasaRasi;
		set => _mNaisargikaDasaRasi = value;
	}

	[Category("Navamsa Dasa Strengths")]
	[PGDisplayName("Rasi Strengths")]
	public RashiStrength[] NavamsaDasaRasi
	{
		get => _mNavamsaDasaRasi;
		set => _mNavamsaDasaRasi = value;
	}

	[Category("Moola Dasa Strengths")]
	[PGDisplayName("Rasi Strengths")]
	public RashiStrength[] MoolaDasaRasi
	{
		get => _mMoolaDasaRasi;
		set => _mMoolaDasaRasi = value;
	}

	[Category("Narayana Dasa Strengths")]
	[PGDisplayName("Rasi Strengths")]
	public RashiStrength[] NarayanaDasaRasi
	{
		get => _mNarayanaDasaRasi;
		set => _mNarayanaDasaRasi = value;
	}

	[Category("Karakas Kendradi Graha Dasa")]
	[PGDisplayName("Rasi Strengths")]
	public RashiStrength[] KarakaKendradiGrahaDasaRasi
	{
		get => _mKarakaKendradiGrahaDasaRasi;
		set => _mKarakaKendradiGrahaDasaRasi = value;
	}

	[Category("Karakas Kendradi Graha Dasa")]
	[PGDisplayName("Graha Strengths")]
	public GrahaStrength[] KarakaKendradiGrahaDasaGraha
	{
		get => _mKarakaKendradiGrahaDasaGraha;
		set => _mKarakaKendradiGrahaDasaGraha = value;
	}

	[PGNotVisible]
	[Category("Karakas Kendradi Graha Dasa")]
	[PGDisplayName("CoLord Strengths")]
	[TypeConverter(typeof(MhoraArrayConverter))]
	public GrahaStrength[] KarakaKendradiGrahaDasaColord
	{
		get => _mKarakaKendradiGrahaDasaColord;
		set => _mKarakaKendradiGrahaDasaColord = value;
	}

	public static RashiStrength[] JaiminiFirstRasi
	{
		get
		{
			return new []
			{
				RashiStrength.AtmaKaraka,
				RashiStrength.Conjunction,
				RashiStrength.Exaltation,
				RashiStrength.MoolaTrikona,
				RashiStrength.OwnHouse,
				RashiStrength.RasisNature,
				RashiStrength.LordIsAtmaKaraka,
				RashiStrength.LordsLongitude,
				RashiStrength.LordInDifferentOddity
			};
		}
	}

	public static RashiStrength [] JaiminiSecondRasi
	{
		get
		{
			return new []
			{
				RashiStrength.AspectsRasi
			};
		}
	}

	public static GrahaStrength [] VimsottariGraha
	{
		get
		{
			return new []
			{
				GrahaStrength.KendraConjunction,
				GrahaStrength.First
			};
		}
	}


	public StrengthOptions Clone()
	{
		var opts = new StrengthOptions
		{
			Colord = (GrahaStrength[]) Colord.Clone(),
			NaisargikaDasaGraha = (GrahaStrength[]) NaisargikaDasaGraha.Clone(),
			NavamsaDasaRasi = (RashiStrength[]) NavamsaDasaRasi.Clone(),
			MoolaDasaRasi = (RashiStrength[]) MoolaDasaRasi.Clone(),
			NarayanaDasaRasi = (RashiStrength[]) NarayanaDasaRasi.Clone(),
			NaisargikaDasaRasi = (RashiStrength[]) NaisargikaDasaRasi.Clone()
		};
		return opts;
	}

	public object Copy(object o)
	{
		var so = (StrengthOptions) o;
		Colord              = (GrahaStrength[]) so.Colord.Clone();
		NaisargikaDasaGraha = (GrahaStrength[]) so.NaisargikaDasaGraha.Clone();
		NavamsaDasaRasi     = (RashiStrength[]) so.NavamsaDasaRasi.Clone();
		MoolaDasaRasi       = (RashiStrength[]) so.MoolaDasaRasi.Clone();
		NarayanaDasaRasi    = (RashiStrength[]) so.NarayanaDasaRasi.Clone();
		NaisargikaDasaRasi  = (RashiStrength[]) so.NaisargikaDasaRasi.Clone();
		return Clone();
	}
}