using System;
using System.Collections.Generic;
using Mhora.Definitions;
using Mhora.Util;

namespace Mhora.Elements.Yoga
{
	public class Rashi
	{
		private static readonly Dictionary<DivisionType, List <Rashi>> _rashis = new();

		private readonly DivisionType _varga;
		private readonly ZodiacHouse  _zh;
		private          Bhava        _bhava;

		protected Rashi(ZodiacHouse zh, DivisionType varga)
		{
			_varga = varga;
			_zh    = zh;
			Grahas = new List<Graha>();
		}

		public override string ToString()
		{
			return _zh.Name ();
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
					var grahas = Graha.Grahas(_varga);
					var lagna  = grahas.Find(graha => graha.Body == Body.Lagna).Rashi.ZodiacHouse;
					_bhava = (Bhava) lagna.NumHousesBetween(ZodiacHouse);
				}

				return _bhava;
			}
		}

		public static Rashi FindOrAdd(ZodiacHouse zh, DivisionType varga)
		{
			var rashis = Rashis(varga);
			var rashi = rashis.Find (rashi => rashi._zh == zh);
			if (rashi == null)
			{
				rashi = new Rashi (zh, varga);
				rashis.Add (rashi);
			}
			return rashi;
		}

		public static List <Rashi> Rashis (DivisionType varga)
		{
			if (_rashis.TryGetValue(varga, out var rashis) == false)
			{
				rashis = new List<Rashi>();
				_rashis.Add(varga, rashis);
				foreach (ZodiacHouse zh in Enum.GetValues(typeof(ZodiacHouse)))
				{
					rashis.Add(new Rashi(zh,varga));
				}
			}

			return (rashis);
		}

		public List<Graha> Grahas { get; set;}

		public static bool Examine(DivisionType varga)
		{

			var rashis = Rashis(varga);
			var grahas = Graha.Grahas(varga);

			var lagna = grahas.Find(graha => graha.Body == Body.Lagna).Rashi.ZodiacHouse;
		

			foreach (var rashi in rashis)
			{
				rashi._bhava = (Bhava) lagna.NumHousesBetween(rashi.ZodiacHouse);
				rashi.Grahas = grahas.FindAll(graha => graha.Rashi == rashi);
			}
			return (true);

		}

		public static void Create(DivisionType varga)
		{
			if (_rashis.TryGetValue(varga, out var rashis) == false)
			{
				rashis = Rashis(varga);
			}
		}

		public static Rashi Find(Bhava bhava, DivisionType varga)
		{
			var rashis = Rashis(varga);
			return (rashis.Find(rashi => rashi.Bhava == bhava));
		}

		public static Rashi Find(ZodiacHouse zh, DivisionType varga)
		{
			var rashis = Rashis(varga);
			return (rashis.Find(rashi => rashi.ZodiacHouse == zh));
		}
	}
}
