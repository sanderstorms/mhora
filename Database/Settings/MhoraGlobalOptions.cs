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
using System.Drawing;
using System.IO;
using Mhora.Components.Delegates;
using Mhora.Components.Property;
using Mhora.Components.Varga;
using Mhora.Elements;
using Mhora.Util;
using Newtonsoft.Json;

namespace Mhora.Database.Settings;

/// <summary>
///     Summary description for GlobalOptions.
/// </summary>
[JsonObject]
public class MhoraGlobalOptions : MhoraSerializableOptions
{
	protected const string CatGeneral   = "1: General Settings";
	protected const string CatLocation  = "2: Default Location";
	protected const string CatLfGen    = "3: Look and Feel";
	protected const string CatLfDasa   = "3: Look and Feel: Dasa";
	protected const string CatLfDiv    = "4: Look and Feel: Vargas";
	protected const string CatLfTable  = "5: Look and Feel: Tabular Charts";
	protected const string CatLfChakra = "6: Look and Feel: Chakras";

	protected const string CatLfBindus = "7: Look and Feel: Bindus";

	//[NonSerialized]	public static object Reference = null;
	[NonSerialized]
	public static MainForm MainControl;

	public static MhoraGlobalOptions Instance;

	// Dasa Control
	private bool _bDasaHoverSelect;
	private bool _bDasaMoveSelect;
	private bool _bDasaShowEvents;
	private bool _bVargaShowDob;
	private bool _bVargaShowSavRasi;
	private bool _bVargaShowSavVarga;

	// Vargas charts
	private bool _bVargaSquare;

	// Form Widths
	public Size GrahaStrengthsFormSize = new(0, 0);

	public  HoroscopeOptions HOptions;
	private bool             _mbSavePrefsOnExit;

	// General
	private bool  _mbShowSplashScreeen;
	private Color _mcBodyJupiter;
	private Color _mcBodyKetu;

	// Body Colors
	private Color _mcBodyLagna;
	private Color _mcBodyMars;
	private Color _mcBodyMercury;
	private Color _mcBodyMoon;
	private Color _mcBodyOther;
	private Color _mcBodyRahu;
	private Color _mcBodySaturn;
	private Color _mcBodySun;
	private Color _mcBodyVenus;
	// Chakra Displays
	private Color _mcChakraBackground;
	private Color _mcDasaBackColor;
	private Color _mcDasaDateColor;
	private Color _mcDasaHighlightColor;
	private Color _mcDasaPeriodColor;

	private DivisionalChart.UserOptions.EChartStyle _mChartStyle;

	// Tabular Displays
	private Color _mcTableBackground;
	private Color _mcTableForeground;
	private Color _mcTableInterleaveFirst;
	private Color _mcTableInterleaveSecond;
	private Color _mcVargaBackground;
	private Color _mcVargaGraha;
	private Color _mcVargaLagna;
	private Color _mcVargaSav;
	private Color _mcVargaSecondary;
	private Color _mcVargaSpecialLagna;
	private Font  _mfFixedWidth;

	// General Font families
	private Font _mfGeneral;
	private Font _mfVarga;
	private int  _miDasaShowEventsLevel;

	private Angle           _mLat;
	private Angle           _mLon;
	private string          _msNotesExtension;
	public  Size            RasiStrengthsFormSize = new(0, 0);
	public  StrengthOptions SOptions;
	public  Size            VargaRectificationFormSize = new(0, 0);

	public MhoraGlobalOptions()
	{
		HOptions = new HoroscopeOptions();
		SOptions = new StrengthOptions();

		_mfFixedWidth = new Font("Courier New", 10);
		_mfGeneral    = new Font("Microsoft Sans Serif", 10);

		_bDasaHoverSelect      = false;
		_bDasaMoveSelect       = true;
		_bDasaShowEvents       = true;
		_miDasaShowEventsLevel = 2;
		_mcDasaBackColor       = Color.Lavender;
		_mcDasaDateColor       = Color.DarkRed;
		_mcDasaPeriodColor     = Color.DarkBlue;
		_mcDasaHighlightColor  = Color.White;

		_mbShowSplashScreeen = true;
		_mbSavePrefsOnExit   = true;
		_msNotesExtension    = "txt";

		_mcBodyLagna   = Color.BlanchedAlmond;
		_mcBodySun     = Color.Orange;
		_mcBodyMoon    = Color.LightSkyBlue;
		_mcBodyMars    = Color.Red;
		_mcBodyMercury = Color.Green;
		_mcBodyJupiter = Color.Yellow;
		_mcBodyVenus   = Color.Violet;
		_mcBodySaturn  = Color.DarkBlue;
		_mcBodyRahu    = Color.LightBlue;
		_mcBodyKetu    = Color.LightPink;
		_mcBodyOther   = Color.Black;

		_mcVargaBackground   = Color.AliceBlue;
		_mcVargaSecondary    = Color.CadetBlue;
		_mcVargaGraha        = Color.DarkRed;
		_mcVargaLagna        = Color.DarkViolet;
		_mcVargaSav          = Color.Gainsboro;
		_mcVargaSpecialLagna = Color.Gray;
		_mChartStyle         = DivisionalChart.UserOptions.EChartStyle.SouthIndian;
		_mfVarga             = new Font("Times New Roman", 7);
		_bVargaSquare        = true;
		_bVargaShowDob       = true;
		_bVargaShowSavVarga  = true;
		_bVargaShowSavRasi   = false;

		_mcTableBackground       = Color.Lavender;
		_mcTableForeground       = Color.Black;
		_mcTableInterleaveFirst  = Color.AliceBlue;
		_mcTableInterleaveSecond = Color.Lavender;

		_mcChakraBackground = Color.AliceBlue;
	}

	[Category(CatGeneral)]
	[PropertyOrder(1)]
	[PGDisplayName("Show splash screen")]
	public bool ShowSplashScreen
	{
		get => _mbShowSplashScreeen;
		set => _mbShowSplashScreeen = value;
	}

	[Category(CatGeneral)]
	[PropertyOrder(2)]
	[PGDisplayName("Save Preferences on Exit")]
	public bool SavePrefsOnExit
	{
		get => _mbSavePrefsOnExit;
		set => _mbSavePrefsOnExit = value;
	}

	[Category(CatGeneral)]
	[PropertyOrder(3)]
	[PGDisplayName("Notes file type")]
	public string ChartNotesFileExtension
	{
		get => _msNotesExtension;
		set => _msNotesExtension = value;
	}

	[Category(CatGeneral)]
	[PropertyOrder(4)]
	[PGDisplayName("Yogas file name")]
	public string YogasFileName => GetExeDir() + "\\" + "yogas.mhr";

	public string City
	{
		get;
		set;
	} = "Maastricht";

	[PropertyOrder(1)]
	[Category(CatLocation)]
	public Angle Latitude
	{
		get => _mLat;
		set => _mLat = value;
	}

	[PropertyOrder(2)]
	[Category(CatLocation)]
	public Angle Longitude
	{
		get => _mLon;
		set => _mLon = value;
	}

	[Category(CatLfGen)]
	[PGDisplayName("Font")]
	public Font GeneralFont
	{
		get => _mfGeneral;
		set => _mfGeneral = value;
	}

	[Category(CatLfGen)]
	[PGDisplayName("Fixed width font")]
	public Font FixedWidthFont
	{
		get => _mfFixedWidth;
		set => _mfFixedWidth = value;
	}

	[PropertyOrder(1)]
	[Category(CatLfDasa)]
	[PGDisplayName("Select by Mouse Hover")]
	public bool DasaHoverSelect
	{
		get => _bDasaHoverSelect;
		set => _bDasaHoverSelect = value;
	}

	[PropertyOrder(1)]
	[Category(CatLfDasa)]
	[PGDisplayName("Select by Mouse Move")]
	public bool DasaMoveSelect
	{
		get => _bDasaMoveSelect;
		set => _bDasaMoveSelect = value;
	}

	[PropertyOrder(2)]
	[Category(CatLfDasa)]
	[PGDisplayName("Show Events")]
	public bool DasaShowEvents
	{
		get => _bDasaShowEvents;
		set => _bDasaShowEvents = value;
	}

	[PropertyOrder(3)]
	[Category(CatLfDasa)]
	[PGDisplayName("Show Events Level")]
	public int DasaEventsLevel
	{
		get => _miDasaShowEventsLevel;
		set => _miDasaShowEventsLevel = value;
	}

	[PropertyOrder(4)]
	[Category(CatLfDasa)]
	[PGDisplayName("Period foreground color")]
	public Color DasaPeriodColor
	{
		get => _mcDasaPeriodColor;
		set => _mcDasaPeriodColor = value;
	}

	[PropertyOrder(5)]
	[Category(CatLfDasa)]
	[PGDisplayName("Date foreground color")]
	public Color DasaDateColor
	{
		get => _mcDasaDateColor;
		set => _mcDasaDateColor = value;
	}

	[PropertyOrder(6)]
	[Category(CatLfDasa)]
	[PGDisplayName("Background colour")]
	public Color DasaBackgroundColor
	{
		get => _mcDasaBackColor;
		set => _mcDasaBackColor = value;
	}

	[PropertyOrder(7)]
	[Category(CatLfDasa)]
	[PGDisplayName("Item highlight color")]
	public Color DasaHighlightColor
	{
		get => _mcDasaHighlightColor;
		set => _mcDasaHighlightColor = value;
	}

	[PropertyOrder(1)]
	[Category(CatLfDiv)]
	[PGDisplayName("Display style")]
	public DivisionalChart.UserOptions.EChartStyle VargaStyle
	{
		get => _mChartStyle;
		set => _mChartStyle = value;
	}

	[PropertyOrder(2)]
	[Category(CatLfDiv)]
	[PGDisplayName("Maintain square proportions")]
	public bool VargaChartIsSquare
	{
		get => _bVargaSquare;
		set => _bVargaSquare = value;
	}

	[PropertyOrder(3)]
	[Category(CatLfDiv)]
	[PGDisplayName("Show time of birth")]
	public bool VargaShowDob
	{
		get => _bVargaShowDob;
		set => _bVargaShowDob = value;
	}

	[PropertyOrder(4)]
	[Category(CatLfDiv)]
	[PGDisplayName("Show rasi's SAV bindus")]
	public bool VargaShowSavRasi
	{
		get => _bVargaShowSavRasi;
		set => _bVargaShowSavRasi = value;
	}

	[PropertyOrder(5)]
	[Category(CatLfDiv)]
	[PGDisplayName("Show varga's SAV bindus")]
	public bool VargaShowSavVarga
	{
		get => _bVargaShowSavVarga;
		set => _bVargaShowSavVarga = value;
	}

	[PropertyOrder(6)]
	[Category(CatLfDiv)]
	[PGDisplayName("Background colour")]
	public Color VargaBackgroundColor
	{
		get => _mcVargaBackground;
		set => _mcVargaBackground = value;
	}

	[Category(CatLfDiv)]
	[PropertyOrder(7)]
	[PGDisplayName("Graha foreground colour")]
	public Color VargaGrahaColor
	{
		get => _mcVargaGraha;
		set => _mcVargaGraha = value;
	}

	[Category(CatLfDiv)]
	[PropertyOrder(8)]
	[PGDisplayName("Secondary foreground colour")]
	public Color VargaSecondaryColor
	{
		get => _mcVargaSecondary;
		set => _mcVargaSecondary = value;
	}

	[Category(CatLfDiv)]
	[PropertyOrder(9)]
	[PGDisplayName("Lagna foreground colour")]
	public Color VargaLagnaColor
	{
		get => _mcVargaLagna;
		set => _mcVargaLagna = value;
	}

	[Category(CatLfDiv)]
	[PropertyOrder(10)]
	[PGDisplayName("Special lagna foreground colour")]
	public Color VargaSpecialLagnaColor
	{
		get => _mcVargaSpecialLagna;
		set => _mcVargaSpecialLagna = value;
	}

	[Category(CatLfDiv)]
	[PropertyOrder(11)]
	[PGDisplayName("SAV foreground colour")]
	public Color VargaSavColor
	{
		get => _mcVargaSav;
		set => _mcVargaSav = value;
	}

	[Category(CatLfDiv)]
	[PropertyOrder(12)]
	[PGDisplayName("Font")]
	public Font VargaFont
	{
		get => _mfVarga;
		set => _mfVarga = value;
	}

	[Category(CatLfBindus)]
	[PropertyOrder(1)]
	[PGDisplayName("Lagna")]
	public Color BindusLagnaColor
	{
		get => _mcBodyLagna;
		set => _mcBodyLagna = value;
	}

	[Category(CatLfBindus)]
	[PropertyOrder(2)]
	[PGDisplayName("Sun")]
	public Color BindusSunColor
	{
		get => _mcBodySun;
		set => _mcBodySun = value;
	}

	[Category(CatLfBindus)]
	[PropertyOrder(3)]
	[PGDisplayName("Moon")]
	public Color BindusMoonColor
	{
		get => _mcBodyMoon;
		set => _mcBodyMoon = value;
	}

	[Category(CatLfBindus)]
	[PropertyOrder(4)]
	[PGDisplayName("Mars")]
	public Color BindusMarsColor
	{
		get => _mcBodyMars;
		set => _mcBodyMars = value;
	}

	[Category(CatLfBindus)]
	[PropertyOrder(5)]
	[PGDisplayName("Mercury")]
	public Color BindusMercuryColor
	{
		get => _mcBodyMercury;
		set => _mcBodyMercury = value;
	}

	[Category(CatLfBindus)]
	[PropertyOrder(6)]
	[PGDisplayName("Jupiter")]
	public Color BindusJupiterColor
	{
		get => _mcBodyJupiter;
		set => _mcBodyJupiter = value;
	}

	[Category(CatLfBindus)]
	[PropertyOrder(7)]
	[PGDisplayName("Venus")]
	public Color BindusVenusColor
	{
		get => _mcBodyVenus;
		set => _mcBodyVenus = value;
	}

	[Category(CatLfBindus)]
	[PropertyOrder(8)]
	[PGDisplayName("Saturn")]
	public Color BindusSaturnColor
	{
		get => _mcBodySaturn;
		set => _mcBodySaturn = value;
	}

	[Category(CatLfBindus)]
	[PropertyOrder(9)]
	[PGDisplayName("Rahu")]
	public Color BindusRahuColor
	{
		get => _mcBodyRahu;
		set => _mcBodyRahu = value;
	}

	[Category(CatLfBindus)]
	[PropertyOrder(10)]
	[PGDisplayName("Ketu")]
	public Color BindusKetuColor
	{
		get => _mcBodyKetu;
		set => _mcBodyKetu = value;
	}

	[Category(CatLfBindus)]
	[PropertyOrder(11)]
	[PGDisplayName("Other")]
	public Color BindusOtherColor
	{
		get => _mcBodyOther;
		set => _mcBodyOther = value;
	}

	[Category(CatLfTable)]
	[PropertyOrder(1)]
	[PGDisplayName("Background colour")]
	public Color TableBackgroundColor
	{
		get => _mcTableBackground;
		set => _mcTableBackground = value;
	}

	[Category(CatLfTable)]
	[PropertyOrder(2)]
	[PGDisplayName("Foreground colour")]
	public Color TableForegroundColor
	{
		get => _mcTableForeground;
		set => _mcTableForeground = value;
	}

	[Category(CatLfTable)]
	[PropertyOrder(3)]
	[PGDisplayName("Interleave colour (odd)")]
	public Color TableOddRowColor
	{
		get => _mcTableInterleaveFirst;
		set => _mcTableInterleaveFirst = value;
	}

	[Category(CatLfTable)]
	[PropertyOrder(4)]
	[PGDisplayName("Interleave colour (even)")]
	public Color TableEvenRowColor
	{
		get => _mcTableInterleaveSecond;
		set => _mcTableInterleaveSecond = value;
	}

	[Category(CatLfChakra)]
	[PGDisplayName("Background colour")]
	public Color ChakraBackgroundColor
	{
		get => _mcChakraBackground;
		set => _mcChakraBackground = value;
	}

	public static event EvtChanged DisplayPrefsChanged;
	public static event EvtChanged CalculationPrefsChanged;


	public static void NotifyDisplayChange()
	{
		DisplayPrefsChanged(Instance);
	}

	public static void NotifyCalculationChange()
	{
		CalculationPrefsChanged(Instance.HOptions);
	}

	private Font AddToFontSizesHelper(Font f, int i)
	{
		return new Font(f.FontFamily, f.SizeInPoints + i);
	}

	private void AddToFontSizes(int i)
	{
		_mfFixedWidth = AddToFontSizesHelper(_mfFixedWidth, i);
		_mfGeneral    = AddToFontSizesHelper(_mfGeneral, i);
		_mfVarga      = AddToFontSizesHelper(_mfVarga, i);
	}

	public void IncreaseFontSize()
	{
		AddToFontSizes(1);
	}

	public void DecreaseFontSize()
	{
		AddToFontSizes(-1);
	}

	public Color GetBinduColor(Body.BodyType b)
	{
		switch (b)
		{
			case Body.BodyType.Lagna:   return _mcBodyLagna;
			case Body.BodyType.Sun:     return _mcBodySun;
			case Body.BodyType.Moon:    return _mcBodyMoon;
			case Body.BodyType.Mars:    return _mcBodyMars;
			case Body.BodyType.Mercury: return _mcBodyMercury;
			case Body.BodyType.Jupiter: return _mcBodyJupiter;
			case Body.BodyType.Venus:   return _mcBodyVenus;
			case Body.BodyType.Saturn:  return _mcBodySaturn;
			case Body.BodyType.Rahu:    return _mcBodyRahu;
			case Body.BodyType.Ketu:    return _mcBodyKetu;
			default:                return _mcBodyOther;
		}
	}


	public static MhoraGlobalOptions ReadFromFile()
	{
		var filename = GetOptsFilename();
		if (File.Exists(filename))
		{
			try
			{
				using (StreamReader sr = new StreamReader(filename))
				{
					var serializer = new JsonSerializer();
					try
					{
						var options = (MhoraGlobalOptions) serializer.Deserialize(sr, typeof(MhoraGlobalOptions));
						Instance = options;
					}
					catch (Exception e)
					{
						Console.WriteLine(e);
					}
				}
			}
			catch
			{
				Application.Log.Debug("MHora: Unable to read user preferences", "GlobalOptions");
			}
		}

		return Instance ??=new MhoraGlobalOptions() ;
	}

	public void SaveToFile()
	{
		var filename = GetOptsFilename();
		Application.Log.Debug("Saving Preferences to {0}", filename);
		try
		{
			JsonSerializer serializer = new JsonSerializer
			{
				NullValueHandling = NullValueHandling.Ignore,
			};

			using (StreamWriter sw = new StreamWriter(filename))
			{
				using (JsonWriter writer = new JsonTextWriter(sw))
				{
					writer.Formatting = Formatting.Indented;
					serializer.Serialize(sw, this);
				}
			}
			
		}
		catch (Exception e)
		{
			Application.Log.Exception(e);
		}
	}
}