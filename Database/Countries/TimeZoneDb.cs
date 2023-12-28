using System.Collections.Generic;
using System.Data;
using System.IO;
using SqlNado;
using SujaySarma.Data.Files.TokenLimitedFiles;
using SujaySarma.Data.Files.TokenLimitedFiles.Attributes;

namespace Mhora.Database.Countries;

public class TimeZoneDb
{
    public const   string     CsvFile = "timezones.csv";
    private static TimeZoneDb _instance;

    private DataTable _table;

    public List<Timezone> _timeZones;

    protected TimeZoneDb()
    {
    }

    public static TimeZoneDb Instance => _instance ??= new TimeZoneDb();

    public DataTable Table
    {
        get
        {
            var csvFile = Path.Combine(mhora.WorkingDir, "Database", CsvFile);
            if (File.Exists(csvFile))
            {
                _table ??= TokenLimitedFileReader.GetTable(csvFile, ';');
            }

            return _table;
        }
    }

    public Timezone this[int index]
    {
        get
        {
            var entry    = new Timezone();
            var timeZone = Table.Rows[index];
            for (var col = 0; col < Table.Columns.Count; col++)
            {
                var name  = Table.Columns[col].ColumnName;
                var value = timeZone[col].ToString();
                entry.SetValue(name, value);
            }

            return entry;
        }
    }

    public List<Timezone> TimeZones
    {
        get
        {
            if (_timeZones == null)
            {
                _timeZones = new List<Timezone>();
                foreach (DataRow timeZone in Table.Rows)
                {
                    var entry = new Timezone();

                    for (var col = 0; col < Table.Columns.Count; col++)
                    {
                        var name  = Table.Columns[col].ColumnName;
                        var value = timeZone[col].ToString();
                        entry.SetValue(name, value);
                    }

                    _timeZones.Add(entry);
                }
            }

            return _timeZones;
        }
    }

    [SQLiteTable(Name = "TimeZones")]
    public class Timezone
    {
        [SQLiteColumn(IsPrimaryKey = true)]
        [FileField("Country code(s)")]
        public string CountryCode
        {
            get;
            set;
        }

        [FileField("TZ identifier")]
        public string Identifier
        {
            get;
            set;
        }

        [FileField("Embedded comments")]
        public string Comments
        {
            get;
            set;
        }

        [FileField("UTC offset")]
        public string UtcOffset
        {
            get;
            set;
        }

        [FileField("Dst")]
        public string Dst
        {
            get;
            set;
        }

        [FileField("Time zone")]
        public string TimeZone
        {
            get;
            set;
        }
    }
}