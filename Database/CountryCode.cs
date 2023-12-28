using System.Data;
using System.IO;
using SqlNado;
using SujaySarma.Data.Files.TokenLimitedFiles;
using SujaySarma.Data.Files.TokenLimitedFiles.Attributes;

namespace Mhora.Database;

public class CountryCode
{
    public const   string      CsvFile = "Country codes.csv";
    private static CountryCode _instance;


    private DataTable _table;

    public static CountryCode Instance => _instance ??= new CountryCode();

    public DataTable Table
    {
        get
        {
            var csvFile = Path.Combine(mhora.WorkingDir, "Database", CsvFile);
            if (File.Exists(csvFile))
            {
                _table ??= TokenLimitedFileReader.GetTable(csvFile);
            }

            return _table;
        }
    }

    public CountryCode this[int index]
    {
        get
        {
            var entry   = new CountryCode();
            var geoName = Table.Rows[index];
            for (var col = 0; col < Table.Columns.Count; col++)
            {
                var name  = Table.Columns[col].ColumnName;
                var value = geoName[col].ToString();
                entry.SetValue(name, value);
            }

            return entry;
        }
    }

    [SQLiteTable(Name = "Countries")]
    public class Country
    {
        [FileField("name")]
        public string Name
        {
            get;
            set;
        }

        [SQLiteColumn(IsPrimaryKey = true)]
        [FileField("alpha-2")]
        public string Code
        {
            get;
            set;
        }

        [FileField("alpha-3")]
        public string Code2
        {
            get;
            set;
        }

        [FileField("region")]
        public string Region
        {
            get;
            set;
        }

        [FileField("sub-region")]
        public string SubRegion
        {
            get;
            set;
        }

        [FileField("intermediate-region")]
        public string IntermediateRegion
        {
            get;
            set;
        }
    }
}