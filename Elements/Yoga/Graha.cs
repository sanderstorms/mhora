using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mhora.Definitions;
using Mhora.Elements.Calculation;
using Mhora.SwissEph;
using Mhora.SwissEph.Helpers;

namespace Mhora.Elements.Yoga
{
	public class Graha
	{
		private static readonly Dictionary<DivisionType, List <Graha>> _grahas = new();

		private readonly DivisionType     _varga;
		private readonly DivisionPosition _dp;
		private          Position         _position;
		private readonly Rashi            _rashi;
		private          BodyPosition     _bodyPosition;

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
			Ownership    = new List<Rashi>();
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

		public static List<Graha> Planets(DivisionType varga)
		{
			var planets = Grahas(varga).FindAll(graha => graha.BodyType == BodyType.Graha);
			planets.RemoveAll(graha => graha.Body == Body.Ketu || graha.Body == Body.Rahu);
			planets.Sort((x, y) => x.Bhava.CompareTo(y.Bhava));
			return (planets);
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

		public bool Owns(Rashi rashi)
		{
			if (rashi.ZodiacHouse.LordOfSign() == Body)
			{
				return (true);
			}

			if (Body == Body.Rahu)
				return (rashi.ZodiacHouse == ZodiacHouse.Aqu);

			if (Body == Body.Ketu)
				return (rashi.ZodiacHouse == ZodiacHouse.Sco);

			return (false);
		}

		public bool IsBenefic
		{
			get
			{
				switch (Body)
				{
					case Body.Sun:
					case Body.Mars:
					case Body.Rahu:
					case Body.Ketu:
					case Body.Saturn:
						return false;

					case Body.Jupiter:
					case Body.Venus:
						return (true);
				}

				if (Body == Body.Moon)
				{
					var tithi = _dp.Longitude.ToTithi();
					if (tithi >= Tithi.KrishnaPratipada)
					{
						return (false);
					}
					return (true);
				}

				if (Body == Body.Mercury)
				{
					if (Bhava.IsDushtana())
					{
						return (false);
					}


					if (Conjunct.Count > 0)
					{
						foreach (var graha in Conjunct)
						{
							if (graha.Body != Body.Sun)
							{
								if (graha.IsBenefic == false)
								{
									return (false);
								}
							}
						}

						if ((Before.IsBenefic == false) && (After.IsBenefic == false))
						{
							return (false);
						}

						return (true);

					}
				}

				return (true);
			}
		}


		public bool IsFunctionalBenefic
		{
			get
			{
				if (IsFunctionalMalefic)
				{
					return (false);
				}

				foreach (var rashi in Ownership)
				{
					if (rashi.Bhava.IsTrikona())
					{
						return (true);
					}

					if (rashi.Bhava.IsKendra())
					{
						var lagna = Find(Body.Lagna, _varga);
						if (lagna.Body.IsFriend(Body))
						{
							return (true);
						}
					}
				}

				return (false);
			}
		}

		public bool IsFunctionalMalefic
		{
			get
			{
				foreach (var rashi in Ownership)
				{
					if (rashi.Bhava.IsDushtana())
					{
						return (true);
					}
				}

				return (false);
			}
		}

        public bool IsRetrograde => (_position.SpeedLongitude < 0.0);

		public BodyType     BodyType     => _dp.BodyType;
		public Body         Body         => _dp.Body;
		public Bhava        Bhava        => Rashi.Bhava;
		public Rashi        Rashi        => _rashi;
		public DivisionType Varga        => _varga;
		public BodyPosition BodyPosition => _bodyPosition;

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

		private Graha _before;
		public Graha Before
		{
			get
			{
				if (_before == null)
				{
					var grahas = _grahas[_varga];

					for (int index = 0; index < grahas.Count; index++)
					{
						if (grahas[index].Body == Body)
						{
							if (index == 0)
							{
								_before = grahas.Last();
							}
							else
							{
								_before = grahas [index - 1];
							}
							break;
						}
					}
				}
				return _before;
			}
		}

		private Graha _after;
		public Graha After
		{
			get
			{
				if (_after == null)
				{
					var grahas = _grahas[_varga];

					for (int index = 0; index < grahas.Count; index++)
					{
						if (grahas[index].Body == Body)
						{
							if (index == grahas.Count - 1)
							{
								_after = grahas.First();
							}
							else
							{
								_after = grahas [index + 1];
							}
							break;
						}
					}
				}
				return _after;
			}
		}


		private Graha _exchange;
		public  Graha Exchange => _exchange;


		public List<Rashi>  Ownership    { get; }
		public List<Graha>  RashiDrishti { get; }
		public List <Graha> AspectFrom   { get; }
		public List<Graha>  AspectTo     { get; }
		public List<Graha>  MutualAspect { get; }
		public List<Graha>  Conjunct     { get; }

		private double _digBala;
        public double DigBala => _digBala;

		public bool IsStrong
		{
			get
			{
				if (IsDebilitated)
				{
					if (Strength < 2)
					{
						return (false);
					}
				}

				if (IsExalted || IsMoolTrikona || IsInOwnHouse)
				{
					if (Strength >= 2)
					{
						return (true);
					}
				}

				if (Strength >= 3)
				{
					return (true);
				}

				return (false);
			}
		}

		//Area of the Dig bala = length of planets-deducted from the weak point of the planets
		//(if the distance between them is more than 180 degree then it is deducted from 360 degrees)
		public bool IsDigBala
		{
			get
			{
				switch (Body)
				{
					case Body.Sun:     return (Bhava == Bhava.KarmaBhava);
					case Body.Moon:    return (Bhava == Bhava.SukhaBhava);
					case Body.Mars:    return (Bhava == Bhava.KarmaBhava);
					case Body.Mercury: return (Bhava == Bhava.LagnaBhava);
					case Body.Jupiter: return (Bhava == Bhava.LagnaBhava);
					case Body.Venus:   return (Bhava == Bhava.SukhaBhava);
					case Body.Saturn:  return (Bhava == Bhava.JayaBhava );
				}

				return (false);
			}
		}

		// F.T.S. - FIRST TIER STRENGTH
		// S.T.S. = SECOND TIER STRENGHT
		// F.T.W. = FIRST TIER WEAKNESS
		// S.T.W. = SECOND TIER WEAKNESS
		//
		// 1	
		// Retrograde	
		// Assoc/Asp of NB	
		// Combustion	
		// Assoc/Asp of NM
		// 
		// 2	
		// Exhalted	
		// Friendly Sign	
		// Debilitation	
		// Enemy Sign
		// 
		// 3	
		// Sva Rasi	
		// Flanked by NB’s	
		// “New’ish” Moon	
		// Flanked by NB’s
		// 
		// 4	
		// Dig Bala	
		// Winner of War	
		// Solar or Lunar Eclipse	
		// Rasi Sandhi
		// 
		// 5	
		// “Full’ish” Moon		
		// Loses War	
		// 
		// 6	
		// RA/KE Eclipse			
		public int Strength
		{
			get
			{
				int strength = 0;
				if (IsRetrograde)
                {
                    strength += 2;
                }
				if (IsExalted)
                {
                    strength += 2;
                }
				else if (IsInOwnHouse)
                {
                    strength += 2;
                }
				if (DigBala > 30)
                {
                    strength += 2;
                }

				int st = 0;
				foreach (var graha in Conjunct)
				{
					if (graha.IsBenefic)
					{
						st++;
					}
					else
					{
						st--;
					}
				}

				foreach (var graha in AspectFrom)
				{
					if (graha.IsBenefic)
					{
						st++;
					}
					else
					{
						st--;
						if (graha.Body == Body.Sun)
						{
							strength--;
						}
					}
				}

				if (st > 0)
				{
					strength++;
				}
				else if (st < 0)
				{
					strength--;
				}

				if (IsDebilitated)
				{
					strength -= 2;
				}

				if (GrahaYuda)
				{
					if (WinnerOfWar)
					{
						strength += 1;
					}
					else
					{
						strength -= 1;
					}
				}

				return (strength);
			}
		}


		public bool Paapkartari 
		{
			get
			{
				if (Before.IsBenefic == false)
				{
					if (Bhava.HousesFrom(Before.Bhava) > 1)
					{
						return false;
					}
					if (After.IsBenefic == false)
					{
						if (Bhava.HousesTo(Before.Bhava) > 1)
						{
							return false;
						}

						return (true);
					}
				}
				return false;
			}
		}

		public bool ShubhKartari
		{
			get
			{
				if (IsCombust || IsRetrograde)
				{
					return (false);
				}
				if (Before.IsBenefic)
				{
					if (Bhava.HousesFrom(Before.Bhava) > 1)
					{
						return false;
					}
					if (After.IsBenefic)
					{
						if (Bhava.HousesTo(Before.Bhava) > 1)
						{
							return false;
						}

						return (true);
					}
				}
				return false;
			}
		}

		public bool FriendlySign 
		{
			get
			{
				if ((IsExalted) || (IsMoolTrikona) || (IsInOwnHouse))
				{
					return (true);
				}

				if (HouseLord.Body.IsFriend(Body))
				{
					return (true);
				}
				return (false);
			}
		}

		public bool EnemySign
		{
			get
			{
				if (IsDebilitated)
				{
					return (true);
				}

				if (HouseLord.Body.IsEnemy(Body))
				{
					return (true);
				}

				return (false);
			}
		}

		public bool IsTaraGraha
		{
			get
			{
				switch (Body)
				{
					case Body.Mars:
					case Body.Mercury:
					case Body.Jupiter:
					case Body.Venus:
					case Body.Saturn:
						return true;
				}
				return false;
			}
		}

		public bool IsLuminary    => (Body == Body.Moon) || (Body == Body.Sun);
		public bool IsChayaGraha => (Body == Body.Rahu) || (Body == Body.Ketu);

		//Planetary war
		public bool GrahaYuda
		{
			get
			{
				if (IsTaraGraha)
				{
					foreach (var graha in Conjunct)
					{
						if (graha.IsTaraGraha)
						{
							if (Distance(graha) < 1.0)
							{
								return (true);
							}
						}
					}
				}
				return (false);
			}
		}

		//Planet with highest latitude wins
		public bool WinnerOfWar
		{
			get
			{
				if (IsTaraGraha)
				{
					foreach (var graha in Conjunct)
					{
						if (graha.IsTaraGraha)
						{
							return (_bodyPosition.Latitude > graha._bodyPosition.Latitude);
						}
					}
				}
				return (false);
			}
		}

		//                  normal      vakri
		// Moon (Chandra)	0° to 12°	--
		// Mars (Mangal)	0° to 17°	0° to 8°
		// Mercury (Budh)	0° to 14°	0° to 12°
		// Jupiter (Guru)	0° to 11°	0° to 11°
		// Venus (Shukra)	0° to 10°	0° to 8°
		// Saturn (Shani)	0° to 16°	0° to 16°
		public bool IsCombust
		{
			get
			{
				foreach (var graha in Conjunct)
				{
					if (graha.Body == Body.Sun)
					{
						switch (Body)
						{
							case Body.Moon:    return (Distance(graha) <= 12);
							case Body.Saturn:  return (Distance(graha) <= 16);
							case Body.Jupiter: return (Distance(graha) <= 11);
							case Body.Mars:
								if (IsRetrograde)
								{
									return (Distance(graha) <= 17);
								}
								return (Distance(graha) <= 8);
							case Body.Mercury:
								if (IsRetrograde)
								{
									return (Distance(graha) <= 14);
								}
								return (Distance(graha) <= 12);
							case Body.Venus:
								if (IsRetrograde)
								{
									return (Distance(graha) <= 10);
								}
								return (Distance(graha) <= 8);
						}
					}
				}
				return (false);
			}
		}

		public double Distance(Graha graha) => (graha._position.Longitude - _position.Longitude);


		//Planets placed in 2nd, 3rd, 4th, 10th, 11th & 12th from a planet act as its Temporary Friend
		public bool IsTemporalFriend(Graha graha)
		{
			switch (Bhava.HousesTo(graha.Bhava))
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
			switch (Bhava.HousesTo(graha.Bhava))
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

		public bool IsDebilitated => _dp.IsDebilitatedPhalita();
		public bool IsExalted     => _dp.IsExaltedPhalita();
		public bool IsMoolTrikona => _dp.IsInMoolaTrikona();
		public bool IsInOwnHouse => _dp.IsInOwnHouse();

		private void Examine()
		{
			if (IsDebilitated)
			{
				Conditions |= Conditions.Debilitated;
			}

			if (IsExalted)
			{
				Conditions |= Conditions.Exalted;
			}

			if (IsMoolTrikona)
			{
				Conditions |= Conditions.Moolatrikona;
			}

			if (IsInOwnHouse)
			{
				Conditions |= Conditions.OwnHouse;
			}

			if (Bhava.IsKaraka(_dp.Body))
			{
				Conditions |= Conditions.KarakaPlanet;
			}

			foreach (var rashi in Rashi.Rashis(_varga))
			{
				if (Owns(rashi))
				{
					Ownership.Add(rashi);
				}
			}

			foreach (var graha in _grahas [_varga])
			{
				if (graha.Body == Body)
				{
					continue;
				}
				if (_dp.GrahaDristi(graha._dp.ZodiacHouse))
				{
					AspectTo.Add(graha);
				}

				if (graha._dp.GrahaDristi(_dp.ZodiacHouse))
				{
					AspectFrom.Add(graha);
				}

				if (graha._dp.ZodiacHouse.RasiDristi(_rashi.ZodiacHouse))
				{
					RashiDrishti.Add(graha);
				}

				if (HouseLord._dp.Body == graha._dp.Body && _dp.Body == graha.HouseLord._dp.Body)
				{
					_exchange = graha;
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

		public static BodyPosition GetBodyPosition(Horoscope h, Body body)
		{
			var sterr    = new StringBuilder();
			var position = new double[6];

			var result = h.CalcUT(h.Info.Jd, body.SwephBody(), 0, position);

			if (result == sweph.ERR)
			{
				throw new SwedllException(sterr.ToString());
			}

			return new BodyPosition
			{
				Longitude      = position[0],
				Latitude       = position[1],
				Distance       = position[2],
				LongitudeSpeed = position[3],
				LatitudeSpeed  = position[4],
				DistanceSpeed  = position[5]
			};
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
				if ((dp.BodyType == BodyType.Graha) || (dp.BodyType == BodyType.Lagna))
				{
					var graha = new Graha(dp, varga)
                    {
                        _position = h.GetPosition(dp.Body),
                    };
					if ((dp.Body != Body.Lagna) && (graha.IsChayaGraha == false))
					{
						graha._digBala      = h.DigBala(dp.Body);
						graha._bodyPosition = GetBodyPosition(h, dp.Body);
					}
                    grahas.Add(graha);
				}
			}

			grahas.Sort((x, y) => x._dp.Longitude.CompareTo(y._dp.Longitude));

			foreach (var graha in grahas)
			{
				graha.Examine();
			}

			Rashi.Examine(varga);
		}
	}
}
