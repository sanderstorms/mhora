using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SqlNado;
using SqlNado.Query;
using SqlNado.Utilities;

namespace Mhora.Database.World;

[SQLiteTable(Name = "countries")]
public class Country : SQLiteBaseObject, IComparable
{
    private List<TimeZone> _timeZone;

    private Dictionary<string, string> _translations;

    public Country(SQLiteDatabase database) : base(database)
    {
    }

    public override string ToString()
    {
        return $"{Name} ({Region?.Name})";
    }

    public int CompareTo(object obj)
    {
        if (obj is string str)
        {
            return (string.Compare(ToString(), str, StringComparison.Ordinal));
        }

        if (obj is Country country)
        {
            return (CompareTo(obj.ToString()));
        }

        return (0);
    }


    [SQLiteColumn(Name = "id", IsPrimaryKey = true, AutoIncrements = true)]
    public int Id { get; set; }

    [SQLiteColumn(Name = "name")]
    public string Name { get; set; }

    [SQLiteColumn(Name = "iso3")]
    public string Iso3 { get; set; }

    [SQLiteColumn(Name = "numeric_code")]
    public string NumericCode { get; set; }

    [SQLiteColumn(Name = "iso2")]
    public string Iso2 { get; set; }

    [SQLiteColumn(Name = "phonecode")]
    public string PhoneCode { get; set; }

    [SQLiteColumn(Name = "capital")]
    public string Capital { get; set; }

    [SQLiteColumn(Name = "currency")]
    public string Currency { get; set; }

    [SQLiteColumn(Name = "currency_name")]
    public string CurrencyName { get; set; }

    [SQLiteColumn(Name = "currency_symbol")]
    public string CurrencySymbol { get; set; }

    [SQLiteColumn(Name = "tld")]
    public string Tld { get; set; }

    [SQLiteColumn(Name = "native")]
    public string Native { get; set; }

    [SQLiteColumn(Name = "region")]
    public string RegionName { get; set; }

    [SQLiteColumn(Name = "region_id")] //Foreign key Region.id
    public int RegionId { get; set; }

    [SQLiteColumn(Name = "subregion")]
    public string SubRegionName { get; set; }

    [SQLiteColumn(Name = "subregion_id")] //foreign key SubRegions.id
    public int SubRegionId { get; set; }

    [SQLiteColumn(Name = "nationality")]
    public string Nationality { get; set; }

    [SQLiteColumn(Name = "timezones")]
    public string TimeZonesJson { get; set; }

    [SQLiteColumn(Name = "translations")]
    public string TranslationsJson { get; set; }

    [SQLiteColumn(Name = "latitude")]
    public double Latitude { get; set; }

    [SQLiteColumn(Name = "longitude")]
    public double Longitude { get; set; }

    [SQLiteColumn(Name = "emoji")]
    public string Emoji { get; set; }

    [SQLiteColumn(Name = "emojiU")]
    public string EmojiU { get; set; }

    [SQLiteColumn(Name = "created_at")]
    public DateTime CreatedAt { get; set; }

    [SQLiteColumn(Name = "updated_at")]
    public DateTime UpdatedAt { get; set; }

    [SQLiteColumn(Name = "flag")]
    public int Flag { get; set; }

    [SQLiteColumn(Name = "wikiDataId")]
    public string WikiDataId { get; set; }

    public Region Region
    {
        get
        {
            var query   = Query.From<Region>().Where(region => region.Id == RegionId).SelectAll();
            var regions = Database?.Load<Region>(query.ToString()).ToList();
            if (regions?.Count > 0)
            {
                return regions[0];
            }

            return null;
        }
    }

    public SubRegion SubRegion
    {
        get
        {
            var query     = Query.From<SubRegion>().Where(subRegion => subRegion.Id == SubRegionId).SelectAll();
            var subRegion = Database?.Load<SubRegion>(query.ToString()).ToList();
            if (subRegion?.Count > 0)
            {
                return subRegion[0];
            }

            return null;
        }
    }

    [SQLiteColumn(Ignore = true)]
    public List<TimeZone> Timezones
    {
        get
        {
            try
            {
                _timeZone ??= JsonConvert.DeserializeObject<List<TimeZone>>(TimeZonesJson);
            }
            catch (Exception e)
            {
                _timeZone ??= new List<TimeZone>();
            }

            return _timeZone;
        }
    }

    [SQLiteColumn(Ignore = true)]
    public TimeZoneInfo TimeZoneInfo
    {
        get
        {
            try
            {
                return (TimeZoneInfo.FindSystemTimeZoneById(Timezones[0].zoneName));

            }
            catch (Exception e)
            {
                return (null);
            }
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

    public class TimeZone
    {
        public string zoneName      { get; set; }
        public int    gmtOffset     { get; set; }
        public string gmtOffsetName { get; set; }
        public string abbreviation  { get; set; }
        public string tzName        { get; set; }
    }
}

//CREATE TABLE `countries` (
//    `id` integer  NOT NULL PRIMARY KEY AUTOINCREMENT
//    ,  `name` varchar(100) NOT NULL
//    ,  `iso3` char(3) DEFAULT NULL
//    ,  `numeric_code` char(3) DEFAULT NULL
//    ,  `iso2` char(2) DEFAULT NULL
//    ,  `phonecode` varchar(255) DEFAULT NULL
//    ,  `capital` varchar(255) DEFAULT NULL
//    ,  `currency` varchar(255) DEFAULT NULL
//    ,  `currency_name` varchar(255) DEFAULT NULL
//    ,  `currency_symbol` varchar(255) DEFAULT NULL
//    ,  `tld` varchar(255) DEFAULT NULL
//    ,  `native` varchar(255) DEFAULT NULL
//    ,  `region` varchar(255) DEFAULT NULL
//    ,  `region_id` integer  DEFAULT NULL
//    ,  `subregion` varchar(255) DEFAULT NULL
//    ,  `subregion_id` integer  DEFAULT NULL
//    ,  `nationality` varchar(255) DEFAULT NULL
//    ,  `timezones` text COLLATE BINARY
//    ,  `translations` text COLLATE BINARY
//    ,  `latitude` decimal(10,8) DEFAULT NULL
//    ,  `longitude` decimal(11,8) DEFAULT NULL
//    ,  `emoji` varchar(191) DEFAULT NULL
//    ,  `emojiU` varchar(191) DEFAULT NULL
//    ,  `created_at` timestamp NULL DEFAULT NULL
//    ,  `updated_at` timestamp NOT NULL DEFAULT current_timestamp 
//    ,  `flag` integer NOT NULL DEFAULT '1'
//    ,  `wikiDataId` varchar(255) DEFAULT NULL
//    , CONSTRAINT `country_continent_final` FOREIGN KEY (`region_id`) REFERENCES `regions` (`id`)
//    , CONSTRAINT `country_subregion_final` FOREIGN KEY (`subregion_id`) REFERENCES `subregions` (`id`)
//    );