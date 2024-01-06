﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SqlNado;
using SqlNado.Utilities;

namespace Mhora.Database.World;

[SQLiteTable(Name = "regions")]
public class Region : SQLiteBaseObject, IComparable
{
	private Dictionary<string, string> _translations;

	public Region(SQLiteDatabase database) : base(database)
	{
	}

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

		if (obj is Region region)
		{
			return CompareTo(obj.ToString());
		}

		return 0;
	}

	public override string ToString()
	{
		return Name;
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