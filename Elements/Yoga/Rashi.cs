using System.Collections.Generic;
using Mhora.Definitions;
using Mhora.Elements.Calculation;
using Mhora.Util;

namespace Mhora.Elements.Yoga
{
	public class Rashi
	{
		private readonly Rashis      _rashis;
		private readonly ZodiacHouse _zh;
		private          Bhava       _bhava;

		internal Rashi(ZodiacHouse zh, Rashis rashis)
		{
			_rashis = rashis;
			_zh     = zh;
			Grahas  = new List<Graha>();
		}

		public static implicit operator ZodiacHouse(Rashi rashi) => rashi.ZodiacHouse;

		public override string ToString()
		{
			return _zh.Name ();
		}

		public Grahas Base => _rashis.Base;

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
					var lagna  = Base.Find(Body.Lagna).Rashi.ZodiacHouse;
					_bhava = (Bhava) lagna.NumHousesBetween(ZodiacHouse);
				}

				return _bhava;
			}
		}

		public bool HasDirshtiOn(ZodiacHouse zh)
		{
			return ZodiacHouse.RasiDristi(zh);
		}

		public Graha Lord => Base.Find(_zh.LordOfSign());

		public List<Graha> Grahas { get; set;}

		internal void Examine()
		{
			Grahas = Base.NavaGrahas.FindAll(graha => graha.Rashi == this);
		}

		public int CompareTo(Rashi rashi, bool simpleLord, List<RashiStrength> rules, out int winner)
		{
			winner = 0;
			foreach (RashiStrength s in rules)
			{
				var result = Base.GetStronger(this, rashi, simpleLord, s);
				if (result == 0)
				{
					winner++;
				}
				else
				{
					return (result);
				}
			}
			return 0;
		}

	}
}
