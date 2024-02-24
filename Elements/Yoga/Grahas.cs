using System;
using System.Collections;
using System.Collections.Generic;
using Mhora.Definitions;
using Mhora.Elements.Calculation;
using Mhora.SwissEph.Helpers;
using Mhora.Util;

namespace Mhora.Elements.Yoga
{
	public class Grahas : IReadOnlyList<Graha>
	{
		private static readonly Dictionary<DivisionType, Grahas> _foundGrahas = new ();

		private readonly DivisionType _varga;
		private readonly List<Graha>  _grahas;
		private readonly Rashis       _rashis;

		public Grahas (List<Graha> grahas, DivisionType varga)
		{
			_varga = varga;
			_grahas = grahas;
			_rashis = Rashi.Find(varga);

			foreach (var graha in _grahas)
			{
				graha.List = this;
			}
		}

		public DivisionType Varga => _varga;

		public Graha Find (Karaka8 karaka)
		{
			var karaka8 = Karaka8;
			return (karaka8[karaka.Index()]);
		}

		public Graha Find (Karaka7 karaka)
		{
			var karaka7 = Karaka7;
			return (karaka7[karaka.Index()]);
		}

		public Graha Find(Body body)
		{
			return _grahas.Find(graha => graha == body);
		}

		public List<Graha> Karaka8
		{
			get
			{
				var karaka8 = _grahas.FindAll(graha => graha.BodyType == BodyType.Graha);
				karaka8.RemoveAll(graha => graha.Body == Body.Ketu);
				karaka8.Sort((x, y) => y.HouseOffset.CompareTo(x.HouseOffset));
				return (karaka8);
			}
		}

		public List<Graha> Karaka7
		{
			get
			{
				var karaka7 = Karaka8;
				karaka7.RemoveAll(graha => graha.Body == Body.Rahu);
				karaka7.Sort((x, y) => y.HouseOffset.CompareTo(x.HouseOffset));
				return (karaka7);
			}
		}

		public List<Graha> Planets
		{
			get
			{
				var planets = Karaka7;
				planets.Sort((x, y) => x.Angle.CompareTo(y.Angle));
				return (planets);
			}
		}

		internal bool Examine()
		{
			_rashis.Examine(this);
			foreach (var graha in _grahas)
			{
				graha.Examine();
			}
			return (true);
		}


		public IEnumerator<Graha> GetEnumerator()
		{
			return (_grahas.GetEnumerator());
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public int Count => _grahas.Count;

		public Graha this[int index] => _grahas[index];

		public Rashis      Rashis => _rashis;

		public List<Graha> FindAll(Predicate<Graha> func)
		{
			return (_grahas.FindAll(func));
		}

		public static void Clear()
		{
			_foundGrahas.Clear();
		}

		public static Grahas Find(Horoscope h, DivisionType varga)
		{
			if (_foundGrahas.TryGetValue(varga, out var grahaList))
			{
				return (grahaList);
			}
			var grahas = new List<Graha> ();
			try
			{
				var dpList = new DpList(h, new Division(varga));
				var rashis = Rashi.Find(varga);

				foreach (DivisionPosition dp in dpList.Positions)
				{
					if ((dp.BodyType == BodyType.Graha) || (dp.BodyType == BodyType.Lagna))
					{
						var position = h.GetPosition(dp.Body);
						var graha    = new Graha(position, dp, varga, rashis.Find(dp.ZodiacHouse));
						grahas.Add(graha);
					}
				}

				grahas.Sort((x, y) => x.BodyPosition.Longitude.CompareTo(y.BodyPosition.Longitude));

				grahaList = new Grahas(grahas, varga);
				grahaList.Examine();
				return (grahaList);
			}
			catch (Exception e)
			{
				Application.Log.Exception(e);
			}

			return null;
		}

	}
}
