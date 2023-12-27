using System.Data;
using System.Drawing;
using System.IO;
using SqlNado;
using SujaySarma.Data.Files.TokenLimitedFiles;
using SujaySarma.Data.Files.TokenLimitedFiles.Attributes;

namespace Mhora.Database.Countries
{
    public class GeoNamesDb
    {
        private static GeoNamesDb _instance;
        public const  string   CsvFile = "geonames-all-cities-with-a-population-1000.csv";

        [SQLiteTable(Name = "Places")]
        public class GeoName
        {
            [SQLiteColumn(IsPrimaryKey = true)]
            [FileField("Geoname ID")]
            public int Id
            {
                get;
                set;
            }
            [FileField("Name")]
            public string Name
            {
                get;
                set;
            }
            [FileField("ASCII Name")]
            public string AsciiName
            {
                get;
                set;
            }
            [FileField("Alternate Names")]
            public string AlternateName
            {
                get;
                set;
            }
            [FileField("Country Code")]
            public string CountryCode
            {
                get;
                set;
            }
            [FileField("Country name EN")]
            public string CountryNameEn
            {
                get;
                set;
            }
            [FileField("Country Code 2")]
            public string CountryCode2
            {
                get;
                set;
            }
            [FileField("Elevation")]
            public int Elevation
            {
                get;
                set;
            }
            [FileField("Timezone")]
            public string TimeZone
            {
                get;
                set;
            }
           [FileField("LABEL EN")]
            public string LabelEn
            {
                get;
                set;
            }
            [FileField("Coordinates")]
            public PointF Coordinates
            {
                get;
                set;
            }
        }
        protected GeoNamesDb ()
        {
            
        }

        public static GeoNamesDb Instance
        {
            get
            {
                return (_instance ??= new GeoNamesDb());
            }
        }

        DataTable _table;
        public DataTable Table
        {
            get
            {
                var csvFile = Path.Combine(Mhora.mhora.WorkingDir, "Database", CsvFile);
                if (File.Exists(csvFile))
                {
                    _table ??= TokenLimitedFileReader.GetTable(csvFile, ';');
                }
                return (_table);
            }
        }

        public GeoName this[int index]
        {
            get
            {
                var entry = new GeoName();
                var geoName = Table.Rows[index];
                for (int col = 0; col < Table.Columns.Count; col++)
                {
                    var name = Table.Columns[col].ColumnName;
                    var value = geoName[col].ToString();
                    entry.SetValue(name, value);
                }

                return (entry);
            }
        }
   }
}
