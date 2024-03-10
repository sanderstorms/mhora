using System;
using System.Collections;
using System.Collections.Generic;
using Mhora.Definitions;

namespace Mhora.Elements
{
	public class Rashis : IReadOnlyList<Rashi>
	{
		private readonly Grahas      _grahas;
		private readonly List<Rashi> _rashis;

		internal Rashis (Grahas grahas)
		{
			_grahas = grahas;
			_rashis = new List<Rashi>();
			foreach (ZodiacHouse zh in Enum.GetValues(typeof(ZodiacHouse)))
			{
				_rashis.Add(new Rashi(zh, this));
			}
		}

		public DivisionType Varga  => _grahas.Varga;
		public Grahas       Base => _grahas;

		public Rashi this [ZodiacHouse zh] => Find(zh);
		public Rashi this[int index] => _rashis [index];


		internal void Examine()
		{
			foreach (var rashi in _rashis)
			{
				rashi.Examine();
			}
		}


		public Rashi Find(Bhava bhava)
		{
			return (_rashis.Find(rashi => rashi.Bhava == bhava));
		}


		public Rashi Find(ZodiacHouse zh)
		{
			return _rashis.Find (rashi => rashi == zh);
		}


		public  IEnumerator<Rashi> GetEnumerator()
		{
			return (_rashis.GetEnumerator());
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public int Count => _rashis.Count;

		public int Compare(ZodiacHouse zha, ZodiacHouse zhb, bool simpleLord, ArrayList rules, out int winner)
		{
			var strength = new List<RashiStrength>();

			foreach (RashiStrength rule in rules)
			{
				strength.Add(rule);
			}

			return Compare(zha, zhb, simpleLord, strength, out winner);
		}


		public int Compare(ZodiacHouse zha, ZodiacHouse zhb, bool simpleLord, List<RashiStrength> rules, out int winner)
		{
			return this[zha].CompareTo(this[zhb], simpleLord, rules, out winner);
		}

		public Rashi Stronger(ZodiacHouse zha, ZodiacHouse zhb, bool simpleLord, ArrayList rules, out int winner)
		{
			var strength = new List<RashiStrength>();

			foreach (RashiStrength rule in rules)
			{
				strength.Add(rule);
			}

			return Stronger(zha, zhb, simpleLord, strength, out winner);
		}


		public Rashi Stronger(ZodiacHouse zha, ZodiacHouse zhb, bool simpleLord, List<RashiStrength> rules, out int winner)
		{
			if (this[zha].CompareTo(this[zhb], simpleLord, rules, out winner) > 0)
			{
				return (this [zha]);
			}

			return (this[zhb]);
		}

		public Rashi Weaker(ZodiacHouse zha, ZodiacHouse zhb, bool simpleLord, List<RashiStrength> rules, out int winner)
		{
			if (this[zha].CompareTo(this[zhb], simpleLord, rules, out winner) < 0)
			{
				return (this [zha]);
			}

			return (this[zhb]);
		}

		public Rashi Weaker (ZodiacHouse zha, ZodiacHouse zhb, bool simpleLord, ArrayList rules, out int winner)
		{
			var strength = new List<RashiStrength>();

			foreach (RashiStrength rule in rules)
			{
				strength.Add(rule);
			}

			return Weaker(zha, zhb, simpleLord, strength, out winner);
		}
	}
}
