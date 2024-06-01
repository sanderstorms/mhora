using System;
using System.Linq;
using SqlNado;
using SqlNado.Query;
using SqlNado.Utilities;

namespace Mhora.Database.World;

[SQLiteTable(Name = "states")]
public class State : SQLiteBaseObject, IComparable
{
	public State(SQLiteDatabase database) : base(database)
	{
	}

	[SQLiteColumn(Name = "id", IsPrimaryKey = true, AutoIncrements = true)]
	public int Id { get; set; }

	[SQLiteColumn(Name = "name")]
	public string Name { get; set; }

	[SQLiteColumn(Name = "country_id")]
	public int CountryId { get; set; }

	[SQLiteColumn(Name = "country_code")]
	public string CountryCode { get; set; }

	[SQLiteColumn(Name = "fips_code")]
	public string FipsCode { get; set; }

	[SQLiteColumn(Name = "iso2")]
	public string Iso2 { get; set; }

	[SQLiteColumn(Name = "type")]
	public string Type { get; set; }

	[SQLiteColumn(Name = "latitude")]
	public double Latitude { get; set; }

	[SQLiteColumn(Name = "longitude")]
	public double Longitude { get; set; }

	[SQLiteColumn(Name = "created_at")]
	public DateTime CreatedAt { get; set; }

	[SQLiteColumn(Name = "updated_at")]
	public DateTime UpdatedAt { get; set; }

	[SQLiteColumn(Name = "flag")]
	public int Flag { get; set; }

	[SQLiteColumn(Name = "wikiDataId")]
	public string WikiDataId { get; set; }

	[SQLiteColumn(Ignore = true)]
	public Country Country
	{
		get
		{
			var query     = Query.From<Country>().Where(country => country.Id == CountryId).SelectAll();
			var countries = Database?.Load<Country>(query.ToString()).ToList();
			if (countries?.Count > 0)
			{
				return countries[0];
			}

			return null;
		}
	}

	public int CompareTo(object obj)
	{
		if (obj is string str)
		{
			return string.Compare(ToString(), str, StringComparison.Ordinal);
		}

		if (obj is State state)
		{
			return CompareTo(obj.ToString());
		}

		return 0;
	}

	public override string ToString() => Name;
}
//CREATE TABLE `states` (
//    `id` integer NOT NULL PRIMARY KEY AUTOINCREMENT
//    ,  `name` varchar(255) NOT NULL
//    ,  `country_id` integer NOT NULL
//    ,  `country_code` char (2) NOT NULL
//    ,  `fips_code` varchar(255) DEFAULT NULL
//    ,  `iso2` varchar(255) DEFAULT NULL
//    ,  `type` varchar(191) DEFAULT NULL
//    ,  `latitude` decimal (10,8) DEFAULT NULL
//    ,  `longitude` decimal (11,8) DEFAULT NULL
//    ,  `created_at` timestamp NULL DEFAULT NULL
//    ,  `updated_at` timestamp NOT NULL DEFAULT current_timestamp 
//    ,  `flag` integer NOT NULL DEFAULT '1'
//    ,  `wikiDataId` varchar(255) DEFAULT NULL
//    , CONSTRAINT `country_region_final` FOREIGN KEY(`country_id`) REFERENCES `countries` (`id`)
//    );