using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SqlNado;

namespace mhora.Database.World
{
    [SQLiteTable(Name = "regions")]
    public class Region
    {
        [SQLiteColumn(Name = "id", IsPrimaryKey = true, AutoIncrements = true)]
        public int Id { get; set; }
        [SQLiteColumn(Name = "name")]
        public string Name { get; set; }
        [SQLiteColumn(Name = "translations")]
        public string TranslationsJson { get; set; }
        [SQLiteColumn(Name = "created_at")]
        public DateTime CreatedAt { get; set; }
        [SQLiteColumn(Name = "updated_at")]
        public DateTime UpdatedAt { get; set; }
        [SQLiteColumn(Name = "flag")]
        public int Flag { get; set; }
        [SQLiteColumn(Name = "wikiDataId")]
        public string WikiDataId { get; set; }

        private Dictionary<string, string> _translations;
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

                return (_translations);
            }
        }
    }

    //CREATE TABLE `regions` (
    //    `id` integer NOT NULL PRIMARY KEY AUTOINCREMENT
    //    ,  `name` varchar(100) NOT NULL
    //    ,  `translations` text COLLATE BINARY
    //    ,  `created_at` timestamp NULL DEFAULT NULL
    //    ,  `updated_at` timestamp NOT NULL DEFAULT current_timestamp 
    //    ,  `flag` integer NOT NULL DEFAULT '1'
    //    ,  `wikiDataId` varchar(255) DEFAULT NULL
    //    );
}
