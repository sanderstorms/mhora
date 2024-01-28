using System;
using System.Collections.Generic;
using Mhora.Definitions;
using Mhora.Util;

namespace Mhora.Elements.Yoga
{
	public class Rashi
	{
		private static readonly Dictionary<DivisionType, List <Rashi>> _rashis = new();

		private readonly ZodiacHouse  _zh;

		protected Rashi(ZodiacHouse zh)
		{
			_zh = zh;
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

		public static Rashi FindOrAdd(ZodiacHouse zh, DivisionType varga)
		{
			if (_rashis.TryGetValue(varga, out var rashis) == false)
			{
				rashis = new List<Rashi>();
				_rashis.Add(varga, rashis);
			}

			var rashi = rashis.Find (rashi => rashi._zh == zh);
			if (rashi == null)
			{
				rashi = new Rashi (zh);
				rashis.Add (rashi);
			}
			return rashi;
		}

		public static Rashi Find(ZodiacHouse zh, DivisionType varga)
		{
			if (_rashis.TryGetValue(varga, out var rashis) == false)
			{
				rashis = new List<Rashi>();
				_rashis.Add(varga, rashis);
			}

			return rashis.Find (rashi => rashi._zh == zh);
		}

		public List<Graha> Grahas { get; set;}

		public static bool Examine(DivisionType varga)
		{
			if (_rashis.TryGetValue(varga, out var rashis) == false)
			{
				rashis = new List<Rashi>();
				_rashis.Add(varga, rashis);
				return (false);
			}

			if (rashis.Count < 12)
			{
				return (false);
			}

			var grahas = Graha.Grahas(varga);

			foreach (var rashi in rashis)
			{
				rashi.Grahas = grahas.FindAll(graha => graha.Rasi == rashi);
			}
			return (true);

		}

		public static void Create(DivisionType varga)
		{
			if (_rashis.TryGetValue(varga, out var rashis) == false)
			{
				rashis = new List<Rashi>();
				_rashis.Add(varga, rashis);
			}
			else
			{
				rashis.Clear();
			}

			foreach (ZodiacHouse zh in Enum.GetValues(typeof(ZodiacHouse)))
			{
				rashis.Add(new Rashi(zh));
			}
		}
	}
}
