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
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using mhora.Components.Property;
using mhora.Hora;
using mhora.Varga;

namespace mhora.Settings
{
    /// <summary>
    ///     Summary description for GlobalOptions.
    /// </summary>
    [XmlRoot("MhoraOptions")]
    [Serializable]
    public class MhoraGlobalOptions : MhoraSerializableOptions, ISerializable
    {
        protected const string CAT_GENERAL   = "1: General Settings";
        protected const string CAT_LOCATION  = "2: Default Location";
        protected const string CAT_LF_GEN    = "3: Look and Feel";
        protected const string CAT_LF_DASA   = "3: Look and Feel: Dasa";
        protected const string CAT_LF_DIV    = "4: Look and Feel: Vargas";
        protected const string CAT_LF_TABLE  = "5: Look and Feel: Tabular Charts";
        protected const string CAT_LF_CHAKRA = "6: Look and Feel: Chakras";

        protected const string CAT_LF_BINDUS = "7: Look and Feel: Bindus";

        //[NonSerialized]	public static object Reference = null;
        [NonSerialized]
        public static object mainControl;

        public static MhoraGlobalOptions Instance;

        // Dasa Control
        private bool bDasaHoverSelect;
        private bool bDasaMoveSelect;
        private bool bDasaShowEvents;
        private bool bVargaShowDob;
        private bool bVargaShowSAVRasi;
        private bool bVargaShowSAVVarga;

        // Varga charts
        private bool bVargaSquare;

        // Form Widths
        public Size GrahaStrengthsFormSize = new Size(0, 0);

        public  HoroscopeOptions HOptions;
        private bool             mbSavePrefsOnExit;

        // General
        private bool  mbShowSplashScreeen;
        private Color mcBodyJupiter;
        private Color mcBodyKetu;

        // Body Colors
        private Color mcBodyLagna;
        private Color mcBodyMars;
        private Color mcBodyMercury;
        private Color mcBodyMoon;
        private Color mcBodyOther;
        private Color mcBodyRahu;
        private Color mcBodySaturn;
        private Color mcBodySun;
        private Color mcBodyVenus;

        // Chakra Displays
        private Color                                   mcChakraBackground;
        private Color                                   mcDasaBackColor;
        private Color                                   mcDasaDateColor;
        private Color                                   mcDasaHighlightColor;
        private Color                                   mcDasaPeriodColor;
        private DivisionalChart.UserOptions.EChartStyle mChartStyle;

        // Tabular Displays
        private Color mcTableBackground;
        private Color mcTableForeground;
        private Color mcTableInterleaveFirst;
        private Color mcTableInterleaveSecond;
        private Color mcVargaBackground;
        private Color mcVargaGraha;
        private Color mcVargaLagna;
        private Color mcVargaSAV;
        private Color mcVargaSecondary;
        private Color mcVargaSpecialLagna;
        private Font  mfFixedWidth;

        // General Font families
        private Font mfGeneral;
        private Font mfVarga;
        private int  miDasaShowEventsLevel;

        private HMSInfo         mLat;
        private HMSInfo         mLon;
        private string          msNotesExtension;
        private HMSInfo         mTz;
        public  Size            RasiStrengthsFormSize = new Size(0, 0);
        public  StrengthOptions SOptions;
        public  Size            VargaRectificationFormSize = new Size(0, 0);

        public MhoraGlobalOptions()
        {
            HOptions = new HoroscopeOptions();
            SOptions = new StrengthOptions();
            mLat     = new HMSInfo(47, 40, 27, HMSInfo.dir_type.NS);
            mLon     = new HMSInfo(-122, 7, 13, HMSInfo.dir_type.EW);
            mTz      = new HMSInfo(-7, 0, 0, HMSInfo.dir_type.EW);

            mfFixedWidth = new Font("Courier New", 10);
            mfGeneral    = new Font("Microsoft Sans Serif", 10);

            bDasaHoverSelect      = false;
            bDasaMoveSelect       = true;
            bDasaShowEvents       = true;
            miDasaShowEventsLevel = 2;
            mcDasaBackColor       = Color.Lavender;
            mcDasaDateColor       = Color.DarkRed;
            mcDasaPeriodColor     = Color.DarkBlue;
            mcDasaHighlightColor  = Color.White;

            mbShowSplashScreeen = true;
            mbSavePrefsOnExit   = true;
            msNotesExtension    = "txt";

            mcBodyLagna   = Color.BlanchedAlmond;
            mcBodySun     = Color.Orange;
            mcBodyMoon    = Color.LightSkyBlue;
            mcBodyMars    = Color.Red;
            mcBodyMercury = Color.Green;
            mcBodyJupiter = Color.Yellow;
            mcBodyVenus   = Color.Violet;
            mcBodySaturn  = Color.DarkBlue;
            mcBodyRahu    = Color.LightBlue;
            mcBodyKetu    = Color.LightPink;
            mcBodyOther   = Color.Black;

            mcVargaBackground   = Color.AliceBlue;
            mcVargaSecondary    = Color.CadetBlue;
            mcVargaGraha        = Color.DarkRed;
            mcVargaLagna        = Color.DarkViolet;
            mcVargaSAV          = Color.Gainsboro;
            mcVargaSpecialLagna = Color.Gray;
            mChartStyle         = DivisionalChart.UserOptions.EChartStyle.SouthIndian;
            mfVarga             = new Font("Times New Roman", 7);
            bVargaSquare        = true;
            bVargaShowDob       = true;
            bVargaShowSAVVarga  = true;
            bVargaShowSAVRasi   = false;

            mcTableBackground       = Color.Lavender;
            mcTableForeground       = Color.Black;
            mcTableInterleaveFirst  = Color.AliceBlue;
            mcTableInterleaveSecond = Color.Lavender;

            mcChakraBackground = Color.AliceBlue;
        }

        protected MhoraGlobalOptions(SerializationInfo info, StreamingContext context)
            :
            this()
        {
            Constructor(GetType(), info, context);
        }


        [Category(CAT_GENERAL)]
        [PropertyOrder(1)]
        [PGDisplayName("Show splash screen")]
        public bool ShowSplashScreen
        {
            get =>
                mbShowSplashScreeen;
            set =>
                mbShowSplashScreeen = value;
        }

        [Category(CAT_GENERAL)]
        [PropertyOrder(2)]
        [PGDisplayName("Save Preferences on Exit")]
        public bool SavePrefsOnExit
        {
            get =>
                mbSavePrefsOnExit;
            set =>
                mbSavePrefsOnExit = value;
        }

        [Category(CAT_GENERAL)]
        [PropertyOrder(3)]
        [PGDisplayName("Notes file type")]
        public string ChartNotesFileExtension
        {
            get =>
                msNotesExtension;
            set =>
                msNotesExtension = value;
        }

        [Category(CAT_GENERAL)]
        [PropertyOrder(4)]
        [PGDisplayName("Yogas file name")]
        public string YogasFileName => getExeDir() + "\\" + "yogas.mhr";

        [PropertyOrder(1)]
        [Category(CAT_LOCATION)]
        public HMSInfo Latitude
        {
            get =>
                mLat;
            set =>
                mLat = value;
        }

        [PropertyOrder(2)]
        [Category(CAT_LOCATION)]
        public HMSInfo Longitude
        {
            get =>
                mLon;
            set =>
                mLon = value;
        }

        [PropertyOrder(3)]
        [Category(CAT_LOCATION)]
        [PGDisplayName("Time zone")]
        public HMSInfo TimeZone
        {
            get =>
                mTz;
            set =>
                mTz = value;
        }


        [Category(CAT_LF_GEN)]
        [PGDisplayName("Font")]
        public Font GeneralFont
        {
            get =>
                mfGeneral;
            set =>
                mfGeneral = value;
        }

        [Category(CAT_LF_GEN)]
        [PGDisplayName("Fixed width font")]
        public Font FixedWidthFont
        {
            get =>
                mfFixedWidth;
            set =>
                mfFixedWidth = value;
        }

        [PropertyOrder(1)]
        [Category(CAT_LF_DASA)]
        [PGDisplayName("Select by Mouse Hover")]
        public bool DasaHoverSelect
        {
            get =>
                bDasaHoverSelect;
            set =>
                bDasaHoverSelect = value;
        }

        [PropertyOrder(1)]
        [Category(CAT_LF_DASA)]
        [PGDisplayName("Select by Mouse Move")]
        public bool DasaMoveSelect
        {
            get =>
                bDasaMoveSelect;
            set =>
                bDasaMoveSelect = value;
        }

        [PropertyOrder(2)]
        [Category(CAT_LF_DASA)]
        [PGDisplayName("Show Events")]
        public bool DasaShowEvents
        {
            get =>
                bDasaShowEvents;
            set =>
                bDasaShowEvents = value;
        }

        [PropertyOrder(3)]
        [Category(CAT_LF_DASA)]
        [PGDisplayName("Show Events Level")]
        public int DasaEventsLevel
        {
            get =>
                miDasaShowEventsLevel;
            set =>
                miDasaShowEventsLevel = value;
        }

        [PropertyOrder(4)]
        [Category(CAT_LF_DASA)]
        [PGDisplayName("Period foreground color")]
        public Color DasaPeriodColor
        {
            get =>
                mcDasaPeriodColor;
            set =>
                mcDasaPeriodColor = value;
        }

        [PropertyOrder(5)]
        [Category(CAT_LF_DASA)]
        [PGDisplayName("Date foreground color")]
        public Color DasaDateColor
        {
            get =>
                mcDasaDateColor;
            set =>
                mcDasaDateColor = value;
        }

        [PropertyOrder(6)]
        [Category(CAT_LF_DASA)]
        [PGDisplayName("Background colour")]
        public Color DasaBackgroundColor
        {
            get =>
                mcDasaBackColor;
            set =>
                mcDasaBackColor = value;
        }

        [PropertyOrder(7)]
        [Category(CAT_LF_DASA)]
        [PGDisplayName("Item highlight color")]
        public Color DasaHighlightColor
        {
            get =>
                mcDasaHighlightColor;
            set =>
                mcDasaHighlightColor = value;
        }

        [PropertyOrder(1)]
        [Category(CAT_LF_DIV)]
        [PGDisplayName("Display style")]
        public DivisionalChart.UserOptions.EChartStyle VargaStyle
        {
            get =>
                mChartStyle;
            set =>
                mChartStyle = value;
        }

        [PropertyOrder(2)]
        [Category(CAT_LF_DIV)]
        [PGDisplayName("Maintain square proportions")]
        public bool VargaChartIsSquare
        {
            get =>
                bVargaSquare;
            set =>
                bVargaSquare = value;
        }

        [PropertyOrder(3)]
        [Category(CAT_LF_DIV)]
        [PGDisplayName("Show time of birth")]
        public bool VargaShowDob
        {
            get =>
                bVargaShowDob;
            set =>
                bVargaShowDob = value;
        }

        [PropertyOrder(4)]
        [Category(CAT_LF_DIV)]
        [PGDisplayName("Show rasi's SAV bindus")]
        public bool VargaShowSAVRasi
        {
            get =>
                bVargaShowSAVRasi;
            set =>
                bVargaShowSAVRasi = value;
        }

        [PropertyOrder(5)]
        [Category(CAT_LF_DIV)]
        [PGDisplayName("Show varga's SAV bindus")]
        public bool VargaShowSAVVarga
        {
            get =>
                bVargaShowSAVVarga;
            set =>
                bVargaShowSAVVarga = value;
        }

        [PropertyOrder(6)]
        [Category(CAT_LF_DIV)]
        [PGDisplayName("Background colour")]
        public Color VargaBackgroundColor
        {
            get =>
                mcVargaBackground;
            set =>
                mcVargaBackground = value;
        }

        [Category(CAT_LF_DIV)]
        [PropertyOrder(7)]
        [PGDisplayName("Graha foreground colour")]
        public Color VargaGrahaColor
        {
            get =>
                mcVargaGraha;
            set =>
                mcVargaGraha = value;
        }

        [Category(CAT_LF_DIV)]
        [PropertyOrder(8)]
        [PGDisplayName("Secondary foreground colour")]
        public Color VargaSecondaryColor
        {
            get =>
                mcVargaSecondary;
            set =>
                mcVargaSecondary = value;
        }

        [Category(CAT_LF_DIV)]
        [PropertyOrder(9)]
        [PGDisplayName("Lagna foreground colour")]
        public Color VargaLagnaColor
        {
            get =>
                mcVargaLagna;
            set =>
                mcVargaLagna = value;
        }

        [Category(CAT_LF_DIV)]
        [PropertyOrder(10)]
        [PGDisplayName("Special lagna foreground colour")]
        public Color VargaSpecialLagnaColor
        {
            get =>
                mcVargaSpecialLagna;
            set =>
                mcVargaSpecialLagna = value;
        }

        [Category(CAT_LF_DIV)]
        [PropertyOrder(11)]
        [PGDisplayName("SAV foreground colour")]
        public Color VargaSAVColor
        {
            get =>
                mcVargaSAV;
            set =>
                mcVargaSAV = value;
        }

        [Category(CAT_LF_DIV)]
        [PropertyOrder(12)]
        [PGDisplayName("Font")]
        public Font VargaFont
        {
            get =>
                mfVarga;
            set =>
                mfVarga = value;
        }

        [Category(CAT_LF_BINDUS)]
        [PropertyOrder(1)]
        [PGDisplayName("Lagna")]
        public Color BindusLagnaColor
        {
            get =>
                mcBodyLagna;
            set =>
                mcBodyLagna = value;
        }

        [Category(CAT_LF_BINDUS)]
        [PropertyOrder(2)]
        [PGDisplayName("Sun")]
        public Color BindusSunColor
        {
            get =>
                mcBodySun;
            set =>
                mcBodySun = value;
        }

        [Category(CAT_LF_BINDUS)]
        [PropertyOrder(3)]
        [PGDisplayName("Moon")]
        public Color BindusMoonColor
        {
            get =>
                mcBodyMoon;
            set =>
                mcBodyMoon = value;
        }

        [Category(CAT_LF_BINDUS)]
        [PropertyOrder(4)]
        [PGDisplayName("Mars")]
        public Color BindusMarsColor
        {
            get =>
                mcBodyMars;
            set =>
                mcBodyMars = value;
        }

        [Category(CAT_LF_BINDUS)]
        [PropertyOrder(5)]
        [PGDisplayName("Mercury")]
        public Color BindusMercuryColor
        {
            get =>
                mcBodyMercury;
            set =>
                mcBodyMercury = value;
        }

        [Category(CAT_LF_BINDUS)]
        [PropertyOrder(6)]
        [PGDisplayName("Jupiter")]
        public Color BindusJupiterColor
        {
            get =>
                mcBodyJupiter;
            set =>
                mcBodyJupiter = value;
        }

        [Category(CAT_LF_BINDUS)]
        [PropertyOrder(7)]
        [PGDisplayName("Venus")]
        public Color BindusVenusColor
        {
            get =>
                mcBodyVenus;
            set =>
                mcBodyVenus = value;
        }

        [Category(CAT_LF_BINDUS)]
        [PropertyOrder(8)]
        [PGDisplayName("Saturn")]
        public Color BindusSaturnColor
        {
            get =>
                mcBodySaturn;
            set =>
                mcBodySaturn = value;
        }

        [Category(CAT_LF_BINDUS)]
        [PropertyOrder(9)]
        [PGDisplayName("Rahu")]
        public Color BindusRahuColor
        {
            get =>
                mcBodyRahu;
            set =>
                mcBodyRahu = value;
        }

        [Category(CAT_LF_BINDUS)]
        [PropertyOrder(10)]
        [PGDisplayName("Ketu")]
        public Color BindusKetuColor
        {
            get =>
                mcBodyKetu;
            set =>
                mcBodyKetu = value;
        }

        [Category(CAT_LF_BINDUS)]
        [PropertyOrder(11)]
        [PGDisplayName("Other")]
        public Color BindusOtherColor
        {
            get =>
                mcBodyOther;
            set =>
                mcBodyOther = value;
        }

        [Category(CAT_LF_TABLE)]
        [PropertyOrder(1)]
        [PGDisplayName("Background colour")]
        public Color TableBackgroundColor
        {
            get =>
                mcTableBackground;
            set =>
                mcTableBackground = value;
        }

        [Category(CAT_LF_TABLE)]
        [PropertyOrder(2)]
        [PGDisplayName("Foreground colour")]
        public Color TableForegroundColor
        {
            get =>
                mcTableForeground;
            set =>
                mcTableForeground = value;
        }

        [Category(CAT_LF_TABLE)]
        [PropertyOrder(3)]
        [PGDisplayName("Interleave colour (odd)")]
        public Color TableOddRowColor
        {
            get =>
                mcTableInterleaveFirst;
            set =>
                mcTableInterleaveFirst = value;
        }

        [Category(CAT_LF_TABLE)]
        [PropertyOrder(4)]
        [PGDisplayName("Interleave colour (even)")]
        public Color TableEvenRowColor
        {
            get =>
                mcTableInterleaveSecond;
            set =>
                mcTableInterleaveSecond = value;
        }

        [Category(CAT_LF_CHAKRA)]
        [PGDisplayName("Background colour")]
        public Color ChakraBackgroundColor
        {
            get =>
                mcChakraBackground;
            set =>
                mcChakraBackground = value;
        }

        void ISerializable.GetObjectData(
            SerializationInfo info,
            StreamingContext  context)
        {
            GetObjectData(GetType(), info, context);
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

        private Font addToFontSizesHelper(Font f, int i)
        {
            return new Font(f.FontFamily, f.SizeInPoints + i);
        }

        private void addToFontSizes(int i)
        {
            mfFixedWidth = addToFontSizesHelper(mfFixedWidth, i);
            mfGeneral    = addToFontSizesHelper(mfGeneral, i);
            mfVarga      = addToFontSizesHelper(mfVarga, i);
        }

        public void increaseFontSize()
        {
            addToFontSizes(1);
        }

        public void decreaseFontSize()
        {
            addToFontSizes(-1);
        }

        public Color getBinduColor(Body.Body.Name b)
        {
            switch (b)
            {
                case Body.Body.Name.Lagna:   return mcBodyLagna;
                case Body.Body.Name.Sun:     return mcBodySun;
                case Body.Body.Name.Moon:    return mcBodyMoon;
                case Body.Body.Name.Mars:    return mcBodyMars;
                case Body.Body.Name.Mercury: return mcBodyMercury;
                case Body.Body.Name.Jupiter: return mcBodyJupiter;
                case Body.Body.Name.Venus:   return mcBodyVenus;
                case Body.Body.Name.Saturn:  return mcBodySaturn;
                case Body.Body.Name.Rahu:    return mcBodyRahu;
                case Body.Body.Name.Ketu:    return mcBodyKetu;
                default:                return mcBodyOther;
            }
        }


        public static MhoraGlobalOptions readFromFile()
        {
            var gOpts = new MhoraGlobalOptions();
            try
            {
                FileStream sOut;
                sOut = new FileStream(getOptsFilename(), FileMode.Open, FileAccess.Read);
                var formatter = new BinaryFormatter();
                formatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
                gOpts                    = (MhoraGlobalOptions)formatter.Deserialize(sOut);
                sOut.Close();
            }
            catch
            {
                Console.WriteLine("MHora: Unable to read user preferences", "GlobalOptions");
            }

            Instance = gOpts;
            return gOpts;
        }

        public void saveToFile()
        {
            Console.WriteLine("Saving Preferences to {0}", getOptsFilename());
            var sOut      = new FileStream(getOptsFilename(), FileMode.OpenOrCreate, FileAccess.Write);
            var formatter = new BinaryFormatter();
            formatter.Serialize(sOut, this);
            sOut.Close();
        }
    }
}