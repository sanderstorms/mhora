using System.Collections.Generic;
using Mhora.Elements.Calculation;
using Mhora.Tables;
using Mhora.Util;

namespace Mhora.Elements.Yoga
{
	public class Graha
	{
		private static readonly Dictionary<Vargas.DivisionType, List <Graha>> _grahas = new();

		private readonly Body.BodyType       _body;
		private readonly Vargas.DivisionType _varga;

		private Bhava               _bhava;
		private Rasi                _rasi;
		private DivisionPosition    _dp;
		private Position            _position;

		protected Graha(Body.BodyType body, Vargas.DivisionType varga)
		{
			_body  = body;
			_varga = varga;

			AspectFrom = new List<Graha>();
			AspectTo   = new List<Graha>();
			Conjunct   = new List<Graha>();
		}

		public static Graha Find(Body.BodyType body, Vargas.DivisionType varga)
		{
			List<Graha> grahas = null;

			if (_grahas.TryGetValue(varga, out grahas) == false)
			{
				grahas = new List<Graha>();
				_grahas.Add(varga, grahas);
			}

			var graha = grahas.Find(graha => graha._body == body);
			if (graha == null)
			{
				graha = new Graha(body, varga);
				grahas.Add(graha);
			}

			return graha;
		}

		public static Dictionary<Vargas.DivisionType, List <Graha>> Grahas => _grahas;

		public Body.BodyType       BodyType => _body;
		public Rasi                Rasi     => _rasi;
		public Vargas.DivisionType Varga    => _varga;

		public Conditions Conditions { get; private set; }

		public Graha HouseLord { get; private set; }
		public Graha Exchange  { get; private set; }


		public List <Graha> AspectFrom      { get; set; }
		public List<Graha>  AspectTo        { get; set; }
		public List<Graha>  Conjunct        { get; set; }

		public int HousesTo(Graha graha)
		{
			var bhava = graha._bhava.Index() - _bhava.Index() + 1;

			if (bhava <= 0)
			{
				bhava = 12 + bhava;
			}

			return bhava;
		}

		public int HousesFrom(Graha graha)
		{
			var bhava = _bhava.Index() - graha._bhava.Index()  + 1;

			if (bhava <= 0)
			{
				bhava = 12 + bhava;
			}

			return bhava;
		}

		//Planets placed in 2nd, 3rd, 4th, 10th, 11th & 12th from a planet act as its Temporary Friend
		public bool IsTemporalFriend(Graha graha)
		{
			switch (HousesTo(graha))
			{
				case 2:
				case 3:
				case 4:
				case 10:
				case 11:
				case 12:
					return true;
			}
			return false;
		}

		//Planets placed in 1st, 5th, 6th, 7th, 8th, 9th from a planet act as a Temporary Enemy
		public bool IsTemporalEnemy(Graha graha)
		{
			switch (HousesTo(graha))
			{
				case 1:
				case 5:
				case 6:
				case 7:
				case 8:
				case 9:
					return true;
			}
			return false;
		}

		public Body.Relationship Relationship(Graha graha)
		{
			if (_body.IsFriend(graha._body))
			{
				if (IsTemporalEnemy(graha))
				{
					return Body.Relationship.Neutral;
				}

				if (IsTemporalFriend(graha))
				{
					return Body.Relationship.BestFriend;
				}

				return Body.Relationship.Friend;
			}

			if (_body.IsEnemy(graha._body))
			{
				if (IsTemporalEnemy(graha))
				{
					return Body.Relationship.BitterEnemy;
				}

				if (IsTemporalFriend(graha))
				{
					return Body.Relationship.Neutral;
				}

				return Body.Relationship.Enemy;
			}

			if (IsTemporalEnemy(graha))
			{
				return Body.Relationship.Enemy;
			}

			if (IsTemporalFriend(graha))
			{
				return Body.Relationship.Friend;
			}

			return Body.Relationship.Neutral;
		}

		public void Examine(Horoscope h, Division varga)
		{
			_position = h.GetPosition(_body);
			_dp       = _position.ToDivisionPosition(varga);

			_rasi = Rasi.Find(_dp.ZodiacHouse);

			if (_body == Body.BodyType.Lagna)
			{
				_bhava = Bhava.LagnaBhava;
			}
			else
			{
				var lagna = Find(Body.BodyType.Lagna, _varga);
				var bhava = _dp.ZodiacHouse.Index() - ((ZodiacHouse) lagna.Rasi).Index() + 1;

				if (bhava <= 0)
				{
					bhava = 12 + bhava;
				}

				_bhava = (Bhava) bhava;
			}

			if (_dp.IsDebilitatedPhalita())
			{
				Conditions |= Conditions.Debilitated;
			}

			if (_dp.IsExaltedPhalita())
			{
				Conditions |= Conditions.Exalted;
			}

			if (_dp.IsInMoolaTrikona())
			{
				Conditions |= Conditions.Moolatrikona;
			}

			if (_dp.IsInOwnHouse())
			{
				Conditions |= Conditions.OwnHouse;
			}

			if (_bhava.IsKaraka(_body))
			{
				Conditions |= Conditions.KarakaPlanet;
			}

			var lord = _dp.ZodiacHouse.SimpleLordOfZodiacHouse();
			HouseLord = Find(lord, _varga);

			foreach (var graha in _grahas [_varga])
			{
				if (_dp.GrahaDristi(graha._dp.ZodiacHouse))
				{
					AspectTo.Add(graha);
				}

				if (graha._dp.GrahaDristi(_dp.ZodiacHouse))
				{
					AspectFrom.Add(graha);
				}

				if (HouseLord._body == graha._body && _body == graha.HouseLord._body)
				{
					Exchange = graha;
				}

				if (_dp.ZodiacHouse == graha._dp.ZodiacHouse)
				{
					Conjunct.Add(graha);
				}

			}
		}
	}
}
