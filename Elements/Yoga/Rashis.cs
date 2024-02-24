using System;
using System.Collections;
using System.Collections.Generic;
using Mhora.Definitions;

namespace Mhora.Elements.Yoga
{
	public class Rashis : IReadOnlyCollection<Rashi>
	{
		private readonly DivisionType _varga;
		private readonly List<Rashi>  _rashis;

		internal Rashis (DivisionType varga)
		{
			_varga = varga;
			_rashis = new List<Rashi>();
			foreach (ZodiacHouse zh in Enum.GetValues(typeof(ZodiacHouse)))
			{
				_rashis.Add(new Rashi(zh));
			}
		}

		public DivisionType Varga => _varga;

		public Rashis (List<Rashi> rashis)
		{
			_rashis = rashis;
		}

		internal void Examine(Grahas grahaList)
		{
			foreach (var rashi in _rashis)
			{
				rashi.Examine(grahaList);
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

		public int    Count => _rashis.Count;

	}
}
