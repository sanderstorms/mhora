using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using mhora.Database;
using SqlNado;
using SujaySarma.Data.Files.TokenLimitedFiles;
using SujaySarma.Data.Files.TokenLimitedFiles.Attributes;
using static Mhora.Database.TimeZoneDb;

namespace Mhora.Database
{
    public class GeoNamesDb
    {
        private static GeoNamesDb _instance;
        private const  string   CsvFile = "geonames-all-cities-with-a-population-1000.csv";

        [SQLiteTable(Name = "Places")]
        public class GeoName
        {
            [FileField("Geoname ID")]
            private int id;
            [FileField("Name")]
            private string name;
            [FileField("ASCII Name")]
            private string asciiName;
            [FileField("Alternate Names")]
            private string alternateName;
            [FileField("Feature Class")]
            private string featureClass;
            [FileField("Feature Code")]
            private string featureCode;
            [FileField("Country Code")]
            private string countryCode;
            [FileField("Country name EN")]
            private string countryNameEN;
            [FileField("Country Code 2")]
            private string countryCode2;
            [FileField("Admin1 Code")]
            private string admin1Code;
            [FileField("Admin2 Code")]
            private string admin2Code;
            [FileField("Admin3 Code")]
            private string admin3Code;
            [FileField("Admin4 Code")]
            private string admin4Code;
            [FileField("Population")]
            private int population;
            [FileField("Elevation")]
            private int elevation;
            [FileField("DIgital Elevation Model")]
            private int digitalElevationModel;
            [FileField("Timezone")]
            private string timeZone;
            [FileField("Modification date")]
            private DateTime modificationDate;
            [FileField("LABEL EN")]
            private string LabelEN;
            [FileField("Coordinates")]
            private PointF coordinates;
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
                var csvFile = Path.Combine(mhora.WorkingDir, "Database", CsvFile);
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
