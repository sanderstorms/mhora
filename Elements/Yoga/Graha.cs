using System.Collections.Generic;
using Mhora.Definitions;
using Mhora.Util;

namespace Mhora.Elements.Yoga
{
	public class Graha
	{
		private static readonly Dictionary<DivisionType, List <Graha>> _grahas = new();

		private readonly DivisionType     _varga;
		private readonly DivisionPosition _dp;
		private readonly Rashi            _rashi;

		private Bhava _bhava;

		protected Graha(DivisionPosition dp, DivisionType varga)
		{
			_dp    = dp;
			_varga = varga;
			_rashi  = Rashi.FindOrAdd(_dp.ZodiacHouse, _varga);

			AspectFrom   = new List<Graha>();
			AspectTo     = new List<Graha>();
			MutualAspect = new List<Graha>();
			Conjunct     = new List<Graha>();
			RashiDrishti = new List<Graha>();
		}


		public override string ToString()
		{
			return _dp.ToString();
		}

		public static List<Graha> Grahas(DivisionType varga)
		{
			if (_grahas.TryGetValue(varga, out var grahas) == false)
			{
				grahas = new List<Graha>();
				_grahas.Add(varga, grahas);
			}

			return (grahas);
		}

		public static Graha FindOrAdd(DivisionPosition dp, DivisionType varga)
		{
			var grahas = Grahas(varga);
			var graha  = grahas.Find(graha => graha._dp.Body == dp.Body);
			if (graha == null)
			{
				graha = new Graha(dp, varga);
				grahas.Add(graha);
			}

			return graha;
		}

		public static Graha Find(Body body, DivisionType varga)
		{
			var grahas = Grahas(varga);
			return grahas.Find(graha => graha._dp.Body == body);
		}

		public Rashi Rasi => _rashi;
		public DivisionType Varga => _varga;

		public Conditions Conditions { get; private set; }

		private Graha _houseLord;
		public Graha HouseLord
		{
			get
			{
				if (_houseLord == null)
				{
					var lord = _dp.ZodiacHouse.SimpleLordOfZodiacHouse(); 
					_houseLord = Find(lord, _varga);
				}

				return (_houseLord);
			}
		}

		public Graha Exchange  { get; private set; }


		public List<Graha>  RashiDrishti { get; }
		public List <Graha> AspectFrom   { get; }
		public List<Graha>  AspectTo     { get; }
		public List<Graha>  MutualAspect { get; }
		public List<Graha>  Conjunct     { get; }

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

		public Relation Relationship(Graha graha)
		{
			if (_dp.Body.IsFriend(graha._dp.Body))
			{
				if (IsTemporalEnemy(graha))
				{
					return Relation.Neutral;
				}

				if (IsTemporalFriend(graha))
				{
					return Relation.BestFriend;
				}

				return Relation.Friend;
			}

			if (_dp.Body.IsEnemy(graha._dp.Body))
			{
				if (IsTemporalEnemy(graha))
				{
					return Relation.BitterEnemy;
				}

				if (IsTemporalFriend(graha))
				{
					return Relation.Neutral;
				}

				return Relation.Enemy;
			}

			if (IsTemporalEnemy(graha))
			{
				return Relation.Enemy;
			}

			if (IsTemporalFriend(graha))
			{
				return Relation.Friend;
			}

			return Relation.Neutral;
		}

		private void Examine()
		{
			if (_dp.Body == Body.Lagna)
			{
				_bhava = Bhava.LagnaBhava;
			}
			else
			{
				var lagna = Find(Body.Lagna, _varga);
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

			if (_bhava.IsKaraka(_dp.Body))
			{
				Conditions |= Conditions.KarakaPlanet;
			}

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

				if (graha._dp.ZodiacHouse.RasiDristi(_rashi))
				{
					RashiDrishti.Add(graha);
				}

				if (HouseLord._dp.Body == graha._dp.Body && _dp.Body == graha.HouseLord._dp.Body)
				{
					Exchange = graha;
				}

				if (_dp.ZodiacHouse == graha._dp.ZodiacHouse)
				{
					Conjunct.Add(graha);
				}
			}

			foreach (var graha in AspectFrom)
			{
				if (AspectTo.Contains(graha))
				{
					MutualAspect.Add(graha);
				}
			}

		}

		private static bool Examine(DivisionType varga)
		{
			var grahas = Grahas(varga);

			if (grahas.Count < 9)
			{
				return (false);
			}

			foreach (var graha in grahas)
			{
				graha.Examine();
			}
			return (true);
		}

		public static void Create(Horoscope h, DivisionType varga)
		{
			var division = new Division(varga);
			var positions = h.CalculateDivisionPositions(division);

			var grahas = Grahas(varga);
			grahas.Clear();

			Rashi.Create(varga);

			foreach (DivisionPosition dp in positions)
			{
				if (dp.BodyType == BodyType.Graha)
				{
					grahas.Add(new Graha(dp, varga));
				}
				else if (dp.BodyType == BodyType.Lagna)
				{
					grahas.Add(new Graha(dp, varga));
				}
			}

			foreach (var graha in grahas)
			{
				graha.Examine();
			}

			Rashi.Examine(varga);
		}
	}
}
