using System;
using System.Linq;
using SqlNado;
using SqlNado.Query;
using SqlNado.Utilities;

namespace mhora.Database.World
{
    [SQLiteTable(Name = "cities")]
    public class City : SQLiteBaseObject
    {
        [SQLiteColumn(Name = "id", IsPrimaryKey = true, AutoIncrements = true)]
        public int Id { get; set; }
        [SQLiteColumn(Name = "name")]
        public string Name { get; set; }
        [SQLiteColumn(Name = "state_id")]   //Foreign key: States.id
        public int StateId { get; set; }
        [SQLiteColumn(Name = "state_code")]
        public string StateCode { get; set; }
        [SQLiteColumn(Name = "country_id")] //Foreign key: Countries.id
        public int CountryId { get; set; }
        [SQLiteColumn(Name = "country_code")]
        public string CountryCode { get; set; }
        [SQLiteColumn(Name = "latitude")]
        public double Latitude { get; set; }
        [SQLiteColumn(Name = "longitude")]
        public double Longitude { get; set; }
        [SQLiteColumn(Name = "created_at")]
        public DateTime CreatedAt { get; set; }
        [SQLiteColumn(Name = "updated_at")]
        public DateTime UpdatedAt { get; set;}
        [SQLiteColumn(Name = "flag")]
        public int Flag { get; set; }
        [SQLiteColumn(Name = "wikiDataId")]
        public string WikiDataId { get; set; }

        public City(SQLiteDatabase database) : base(database)
        {
        }

        [SQLiteColumn(Ignore = true)]
        public State State
        {
            get
            {
                var query = Query.From<State>().Where(state => state.Id == StateId).SelectAll();
                var states = Database?.Load<State>(query.ToString()).ToList();
                if (states?.Count > 0)
                {
                    return (states[0]);
                }

                return (null);
            }
        }

        [SQLiteColumn(Ignore = true)]
        public Country Country
        {
            get
            {
                var query = Query.From<Country>().Where(country => country.Id == CountryId).SelectAll();
                var countries = Database?.Load<Country>(query.ToString()).ToList();
                if (countries?.Count > 0)
                {
                    return (countries[0]);
                }

                return (null);
            }
        }

    }
    //CREATE TABLE `cities` (
    //    `id` integer NOT NULL PRIMARY KEY AUTOINCREMENT
    //    ,  `name` varchar(255) NOT NULL
    //    ,  `state_id` integer NOT NULL
    //    ,  `state_code` varchar(255) NOT NULL
    //    ,  `country_id` integer NOT NULL
    //    ,  `country_code` char (2) NOT NULL
    //    ,  `latitude` decimal (10,8) NOT NULL
    //    ,  `longitude` decimal (11,8) NOT NULL
    //    ,  `created_at` timestamp NOT NULL DEFAULT '2014-01-01 06:31:01'
    //    ,  `updated_at` timestamp NOT NULL DEFAULT current_timestamp 
    //    ,  `flag` integer NOT NULL DEFAULT '1'
    //    ,  `wikiDataId` varchar(255) DEFAULT NULL
    //    , CONSTRAINT `cities_ibfk_1` FOREIGN KEY(`state_id`) REFERENCES `states` (`id`)
    //    ,  CONSTRAINT `cities_ibfk_2` FOREIGN KEY(`country_id`) REFERENCES `countries` (`id`)
    //    );

}
