using SujaySarma.Data.Files.TokenLimitedFiles;
using SujaySarma.Data.Files.TokenLimitedFiles.Attributes;
using System.Data;
using System.IO;

namespace Mhora.Database
{
    public class TimeZoneDb
    {
        private static TimeZoneDb _instance;
        private const string CsvFile = "timezones.csv";

        [FileField("Country code(s)")]
        private string countryCode;
        [FileField("TZ identifier")]
        private string identifier;
        [FileField("Country code(s)")]
        private string indentifier;
        [FileField("Embedded comments")]
        private string comments;
        [FileField("Type")]
        private string type;
        [FileField("UTC offset")]
        private string utcOffset;
        [FileField("Time zone")]
        private string timeZone;
        [FileField("Source")]
        private string source;

        protected TimeZoneDb()
        {

        }

        public static TimeZoneDb Instance
        {
            get
            {
                return (_instance ??= new TimeZoneDb());
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
    }
}
