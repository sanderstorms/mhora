using System;
using System.Collections;
using System.Collections.Generic;
using Mhora.Calculation;
using Mhora.Definitions;
using Mhora.Elements.Extensions;
using Mhora.Util;
using Mhora.Yoga;

namespace Mhora.Elements
{
	public class Grahas : IReadOnlyList<Graha>
	{
		private readonly Horoscope              _horoscope;
		private readonly DivisionType           _varga;
		private readonly List<Graha>            _grahas;
		private readonly Rashis                 _rashis;
		private readonly List<DivisionPosition> _dpList;

		public Grahas(Horoscope h, DivisionType varga)
		{
			_horoscope = h;
			_varga     = varga;
			_rashis    = new(this);
			_grahas    = [];

			_dpList = h.PositionList.CalculateDivisionPositions(varga);

			foreach (DivisionPosition dp in _dpList)
			{
				if ((dp.BodyType == BodyType.Graha) || (dp.BodyType == BodyType.Lagna))
				{
					var position = h.GetPosition(dp.Body);
					var graha    = new Graha(position, dp, _rashis.Find(dp.ZodiacHouse));
					_grahas.Add(graha);
				}
			}

			_grahas.Sort((x, y) => x.Position.Longitude.CompareTo(y.Position.Longitude));
		}


		public static implicit operator Horoscope(Grahas grahas) => grahas._horoscope;
		public static implicit operator Rashis(Grahas    grahas) => grahas.Rashis;

		public Graha this [Body body] => Find(body);

		public Horoscope Horoscope => _horoscope;

		public DivisionType Varga => _varga;

		public Graha Find(Karaka8 karaka) => Karaka8[karaka.Index()];

		public Graha Find(Karaka7 karaka) => Karaka7[karaka.Index()];

		public Graha Find(Body body)
		{
			var graha = _grahas.Find(graha => graha == body);
			if (graha == null)
			{
				var position = Horoscope.GetPosition(body);
				var dp       = position.ToDivisionPosition(Varga);
				graha        = new Graha(position, dp, _rashis.Find(dp.ZodiacHouse));
				_grahas.Add(graha);
			}

			return (graha);
		}

		public List<Graha> NavaGrahas => _grahas.FindAll(graha => graha.BodyType == BodyType.Graha);

		public List<Graha> Karaka8
		{
			get
			{
				var karaka8 = NavaGrahas;
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

		public FindYoga.Yogas Yogas
		{
			get;
			private set;
		}

		internal bool Examine()
		{
			foreach (var graha in _grahas)
			{
				graha.Connect(this);
			}
			_rashis.Examine();
			foreach (var graha in _grahas)
			{
				graha.Examine();
			}

			Yogas = this.FindYogas();

			return (true);
		}

		public IEnumerator<Graha> GetEnumerator() => (_grahas.GetEnumerator());

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public int Count => _grahas.Count;

		public Graha this[int index] => _grahas[index];

		public Rashis      Rashis => _rashis;

		public List<DivisionPosition> DivisionPositions => _dpList;

		public List<Graha> FindAll(Predicate<Graha> func) => (_grahas.FindAll(func));

		public int Compare(Body b1, Body b2, bool simpleLord, ArrayList rules, out int winner)
		{
			var strength = new List<GrahaStrength>();

			foreach (GrahaStrength rule in rules)
			{
				strength.Add(rule);
			}

			return (Compare(b1, b2, simpleLord, strength, out winner));
		}

		public int Compare(Body b1, Body b2, bool simpleLord, List<GrahaStrength> rules, out int winner) => this[b1].CompareTo(this[b2], simpleLord, rules, out winner);

		public Graha Stronger(Body b1, Body b2, bool simpleLord, ArrayList rules, out int winner)
		{
			var strength = new List<GrahaStrength>();

			foreach (GrahaStrength rule in rules)
			{
				strength.Add(rule);
			}

			return (Stronger(b1, b2, simpleLord, strength, out winner));
		}

		public Graha Stronger(Body b1, Body b2, bool simpleLord, List<GrahaStrength> rules, out int winner)
		{
			if (this[b1].CompareTo(this[b2], simpleLord, rules, out winner) > 0)
			{
				return (this [b1]);
			}

			return (this[b2]);
		}


		public Graha Weaker(Body b1, Body b2, bool simpleLord, ArrayList rules, out int winner)
		{
			var strength = new List<GrahaStrength>();

			foreach (GrahaStrength rule in rules)
			{
				strength.Add(rule);
			}

			return (Weaker(b1, b2, simpleLord, strength, out winner));
		}

		public Graha Weaker(Body b1, Body b2, bool simpleLord, List<GrahaStrength> rules, out int winner)
		{
			if (this[b1].CompareTo(this[b2], simpleLord, rules, out winner) < 0)
			{
				return (this [b1]);
			}

			return (this[b2]);
		}

		public Longitude Calc(JulianDate ut, Body body, Body other, bool sub)
		{
			var bp1 = Horoscope.CalculateSingleBodyPosition(ut, body.SwephBody(), body, BodyType.Graha);
			var bp2 = Horoscope.CalculateSingleBodyPosition(ut, other.SwephBody(), other, BodyType.Graha);

			if (sub)
			{
				return bp1.Longitude.Sub(bp2.Longitude);
			}
			return bp1.Longitude.Add(bp2.Longitude);
		}

		public Func<JulianDate, Ref<bool>, Longitude> Calc(Body body, Body other, bool sub)
		{
			Longitude Fnc(JulianDate ut, Ref<bool> ret)
			{
				ret.Value = false;
				return Calc(ut, body, other, sub);
			}

			return (Fnc);
		}
	}
}
