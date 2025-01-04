using System;
using SqlNado;
using SqlNado.Utilities;

namespace Mhora.Database
{
	//CREATE TABLE "geonames-all-cities-with-a-population-1000" (
	// 	"Geoname ID"				INTEGER,
	// 	"Name"						TEXT,
	// 	"ASCII Name"				TEXT,
	// 	"Alternate Names"			TEXT,
	// 	"Feature Class"				TEXT,
	// 	"Feature Code"				TEXT,
	// 	"Country Code"				TEXT,
	// 	"Country name EN"			TEXT,
	// 	"Country Code 2"			TEXT,
	// 	"Admin1 Code"				INTEGER,
	// 	"Admin2 Code"				TEXT,
	// 	"Admin3 Code"				TEXT,
	// 	"Admin4 Code"				TEXT,
	// 	"Population"				INTEGER,
	// 	"Elevation"					TEXT,
	// 	"DIgital Elevation Model"	INTEGER,
	// 	"Timezone"					TEXT,
	// 	"Modification date"			TEXT,
	// 	"LABEL EN"					TEXT,
	// 	"Coordinates"				TEXT
	// );
	[SQLiteTable(Name = "eonames-all-cities-with-a-population-1000")]
	public class GeoName : SQLiteBaseObject, IComparable
	{
		public GeoName(SQLiteDatabase database) : base(database)
		{
		}


		[SQLiteColumn(Name = "Geoname ID", IsPrimaryKey = true, AutoIncrements = true)]
		public int Id { get; set; }

		[SQLiteColumn(Name = "Name", IsPrimaryKey = true, AutoIncrements = true)]
		public string Name                  { get; set; }

		[SQLiteColumn(Name = "ASCII Name", IsPrimaryKey = true, AutoIncrements = true)]
		public string ASCIIName             { get; set; }

		[SQLiteColumn(Name = "Alternate Names", IsPrimaryKey = true, AutoIncrements = true)]
		public string AlternateNames        { get; set; }

		[SQLiteColumn(Name = "Feature Class", IsPrimaryKey = true, AutoIncrements = true)]
		public string FeatureClass          { get; set; }

		[SQLiteColumn(Name = "Feature Code", IsPrimaryKey = true, AutoIncrements = true)]
		public string FeatureCode           { get; set; }

		[SQLiteColumn(Name = "Country Code", IsPrimaryKey = true, AutoIncrements = true)]
		public string CountryCode           { get; set; }

		[SQLiteColumn(Name = "Country name EN", IsPrimaryKey = true, AutoIncrements = true)]
		public string CountryNameEN         { get; set; }

		[SQLiteColumn(Name = "Country Code 2", IsPrimaryKey = true, AutoIncrements = true)]
		public string CountryCode2          { get; set; }

		[SQLiteColumn(Name = "Admin1 Code", IsPrimaryKey = true, AutoIncrements = true)]
		public int    Admin1Code            { get; set; }

		[SQLiteColumn(Name = "Admin1]2 Code", IsPrimaryKey = true, AutoIncrements = true)]
		public string Admin2Code            { get; set; }

		[SQLiteColumn(Name = "Admin1]3 Code", IsPrimaryKey = true, AutoIncrements = true)]
		public string Admin3Code            { get; set; }

		[SQLiteColumn(Name = "Admin4 Code", IsPrimaryKey = true, AutoIncrements = true)]
		public string Admin4Code            { get; set; }

		[SQLiteColumn(Name = "Population", IsPrimaryKey = true, AutoIncrements = true)]
		public int    Population            { get; set; }

		[SQLiteColumn(Name = "Elevation", IsPrimaryKey = true, AutoIncrements = true)]
		public string Elevation             { get; set; }

		[SQLiteColumn(Name = "DIgital Elevation Model", IsPrimaryKey = true, AutoIncrements = true)]
		public int    DIgitalElevationModel { get; set; }

		[SQLiteColumn(Name = "Timezone", IsPrimaryKey = true, AutoIncrements = true)]
		public string Timezone              { get; set; }

		[SQLiteColumn(Name = "Modification Date", IsPrimaryKey = true, AutoIncrements = true)]
		public string ModificationDate      { get; set; }

		[SQLiteColumn(Name = "LABEL EN", IsPrimaryKey = true, AutoIncrements = true)]
		public string LabelEN               { get; set; }

		[SQLiteColumn(Name = "Coordinates", IsPrimaryKey = true, AutoIncrements = true)]
		public string Coordinates           { get; set; }


		public int CompareTo(object obj)
		{
			return obj switch
			{
				string str => string.Compare(ToString(), str, StringComparison.Ordinal),
				GeoName    => CompareTo(obj.ToString()),
				_          => 0
			};
		}

		public override string ToString() => $"{ASCIIName}";

	}

}
