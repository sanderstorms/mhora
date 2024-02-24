using System;
using System.Collections.Generic;
using Mhora.Definitions;
using Mhora.Util;

namespace Mhora.Elements.Yoga
{
	public class Rashi
	{
		private static readonly Dictionary<DivisionType, Rashis> _rashis = new();

		private readonly ZodiacHouse _zh;
		private          Bhava       _bhava;

		internal Rashi(ZodiacHouse zh)
		{
			_zh    = zh;
			Grahas = new List<Graha>();
		}

		public static implicit operator ZodiacHouse(Rashi rashi) => rashi.ZodiacHouse;

		public override string ToString()
		{
			return _zh.Name ();
		}

		public Grahas GrahaList
		{
			get;
			private set;
		}

		internal static void Clear()
		{
			_rashis.Clear();
		}

		public override bool Equals(object obj)
		{
			if (obj is Rashi rashi)
			{
				return (rashi._zh == _zh);
			}

			if (obj is ZodiacHouse zh)
			{
				return (_zh == zh);
			}

			return (false);
		}

		public ZodiacHouse ZodiacHouse => _zh;

		public Bhava Bhava
		{
			get
			{
				if (_bhava == Bhava.None)
				{
					var lagna  = GrahaList.Find(Body.Lagna).Rashi.ZodiacHouse;
					_bhava = (Bhava) lagna.NumHousesBetween(ZodiacHouse);
				}

				return _bhava;
			}
		}

		public Graha Lord => GrahaList.Find(_zh.LordOfSign());

		public List<Graha> Grahas { get; set;}

		internal void Examine(Grahas grahaList)
		{
			GrahaList = grahaList;
			Grahas = GrahaList.FindAll(graha => graha.Rashi == this);

		}

		public static Rashis Find(DivisionType varga)
		{
			if (_rashis.TryGetValue(varga, out var rashis) == false)
			{
				rashis = new Rashis(varga);
				_rashis.Add(varga, rashis);
			}

			return (rashis);
		}

		public static Rashi Find(Bhava bhava, DivisionType varga)
		{
			if (_rashis.TryGetValue(varga, out var rashis))
			{
				return rashis.Find(bhava);
			}

			return null;
		}

		public static Rashi Find(ZodiacHouse zh, DivisionType varga)
		{
			if (_rashis.TryGetValue(varga, out var rashis))
			{
				return rashis.Find(zh);
			}

			return null;
		}

	}
}
