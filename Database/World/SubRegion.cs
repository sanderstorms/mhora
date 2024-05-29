using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SqlNado;
using SqlNado.Query;
using SqlNado.Utilities;

namespace Mhora.Database.World;

[SQLiteTable(Name = "subregions")]
public class SubRegion : SQLiteBaseObject, IComparable
{
	private Region _region;

	private Dictionary<string, string> _translations;

	public SubRegion(SQLiteDatabase database) : base(database)
	{
	}


	[SQLiteColumn(Name = "id", IsPrimaryKey = true, AutoIncrements = true)]
	public int Id { get; set; }

	[SQLiteColumn(Name = "name")]
	public string Name { get; set; }

	[SQLiteColumn(Name = "translations")]
	public string TranslationsJson { get; set; }

	[SQLiteColumn(Name = "region_id")]
	public int RegionId { get; set; }

	[SQLiteColumn(Name = "created_at")]
	public DateTime CreatedAt { get; set; }

	[SQLiteColumn(Name = "updated_at")]
	public DateTime UpdatedAt { get; set; }

	[SQLiteColumn(Name = "flag")]
	public int Flag { get; set; }

	[SQLiteColumn(Name = "wikiDataId")]
	public string WikiDataId { get; set; }

	[SQLiteColumn(Ignore = true)]
	public Region Region
	{
		get
		{
			if (_region == null)
			{
				var query   = Query.From<Region>().Where(region => region.Id == RegionId).SelectAll();
				var regions = Database?.Load<Region>(query.ToString()).ToList();
				if (regions?.Count > 0)
				{
					_region = regions[0];
				}
			}

			return _region;
		}
	}

	[SQLiteColumn(Ignore = true)]
	public Dictionary<string, string> Translations
	{
		get
		{
			try
			{
				_translations ??= JsonConvert.DeserializeObject<Dictionary<string, string>>(TranslationsJson);
			}
			catch (Exception e)
			{
				_translations = new Dictionary<string, string>();
			}

			return _translations;
		}
	}

	public int CompareTo(object obj)
	{
		if (obj is string str)
		{
			return string.Compare(ToString(), str, StringComparison.Ordinal);
		}

		if (obj is SubRegion subRegion)
		{
			return CompareTo(obj.ToString());
		}

		return 0;
	}

	public override string ToString() => Name;
}
//CREATE TABLE `subregions` (
//    `id` integer NOT NULL PRIMARY KEY AUTOINCREMENT
//    ,  `name` varchar(100) NOT NULL
//    ,  `translations` text COLLATE BINARY
//    ,  `region_id` integer NOT NULL
//    ,  `created_at` timestamp NULL DEFAULT NULL
//    ,  `updated_at` timestamp NOT NULL DEFAULT current_timestamp 
//    ,  `flag` integer NOT NULL DEFAULT '1'
//    ,  `wikiDataId` varchar(255) DEFAULT NULL
//    , CONSTRAINT `subregion_continent_final` FOREIGN KEY(`region_id`) REFERENCES `regions` (`id`)
//    );