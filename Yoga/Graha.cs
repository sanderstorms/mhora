using System.Collections.Generic;
using System.Linq;
using Mhora.Calculation;
using Mhora.Definitions;
using Mhora.Elements;
using Mhora.SwissEph;
using Mhora.Util;

namespace Mhora.Yoga
{
	public class Graha
	{
		private readonly Position         _position;
		private readonly DivisionPosition _dp;
		private readonly Rashi            _rashi;
		private readonly Angle            _houseOffset;
		private          Angle            _angle;
		private          Grahas           _grahas;

		internal Graha(Position position, DivisionPosition dp, Rashi rashi)
		{
			_position = position;
			_dp       = dp;
			_rashi    = rashi;

			_isRetrograde = (_position.SpeedLongitude < 0.0);
			_houseOffset  = _dp.Longitude.ToZodiacHouseOffset();
			if (IsChayaGraha)
			{
				_houseOffset = 30.0 - _houseOffset;
			}

			if ((dp.Body != Body.Lagna) && (IsChayaGraha == false))
			{
				_digBala = _position.H.DigBala(dp.Body);
			}

			AspectFrom   = new List<Graha>();
			AspectTo     = new List<Graha>();
			MutualAspect = new List<Graha>();
			Conjunct     = new List<Graha>();
			RashiDrishti = new List<Graha>();
			Ownership    = new List<Rashi>();
			Association  = new List<Graha>();
			OwnHouses    = new List<Rashi>();
		}

		public static implicit operator Body(Graha      graha) => graha.Body;
		public static implicit operator Grahas(Graha    graha) => graha._grahas;

		public string Name => Body.Name();

		public override string ToString()
		{
			return _dp.ToString();
		}

		public bool Owns(Bhava bhava)
		{
			var rashi = _grahas.Rashis.Find(bhava);
			return (Owns(rashi));
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

		//The five major associations are:
		// 
		// By mutual exchange of sign, e.g. the Sun in Aries and Mars in Leo,
		// By mutual aspect of planets, e.g. Saturn in Aries and Mars in Capricorn,
		// By occupying a particular sign other than its own, e.g. Jupiter in taurus sign owned by Venus,
		// By occupying mutual square positions, e.g. Jupiter in Aries and Mars in Cancer or Libra or Capricorn, and
		// By occupying mutual trinal positions, e.g. Jupiter in Aries and Mars in either Leo or in Sagittarius.[4]
		public bool IsAssociatedWith (Graha graha)
		{
			if (Conjunct.Contains(graha))
			{
				return (true);
			}

			if (MutualAspect.Contains(graha))
			{
				return (true);
			}

			if (Bhava.IsKendra() && graha.Bhava.IsKendra())
			{
				return (true);
			}

			if (Bhava.IsTrikona() && graha.Bhava.IsTrikona())
			{
				return (true);
			}

			if (Rashi.Lord == graha)
			{
				return (true);
			}


			return (false);
		}

		public bool IsAssociatedWith(Bhava bhava)
		{
			foreach (var graha in Association)
			{
				if (graha.Bhava == bhava)
				{
					return (true);
				}
			}
			return (false);
		}


		public bool IsMutualAssosiating(Graha graha) => IsAssociatedBy(graha) && IsAssociatedWith(graha);

		public bool IsAssociatedBy(Body body)
		{
			var graha = _grahas.Find(body);
			return (IsAssociatedBy(graha));
		}

		public bool IsAssociatedBy(Graha graha)
		{
			return graha.IsAssociatedWith(this);
		}


		public bool IsAssociatedWith (Nature nature, bool noChayaGraha)
		{
			foreach (var graha in Association)
			{
				if (graha.Nature == nature)
				{
					if ((noChayaGraha == false) || (graha.IsChayaGraha == false))
					{
						return (true);
					}
				}
			}

			return (false);
		}

		public bool IsAspectedBy (Nature nature, bool noChayaGraha)
		{
			foreach (var graha in AspectFrom)
			{
				if (graha.Nature == nature)
				{
					if ((noChayaGraha == false) || (graha.IsChayaGraha == false))
					{
						return (true);
					}
				}
			}

			return (false);
		}

		public bool HasDrishtiOn(ZodiacHouse zh)
		{
			return _dp.GrahaDristi(zh);
		}


		public bool IsAspecting(Body body)
		{
			var graha = _grahas.Find(body);
			return IsAspecting(graha);
		}

		public bool IsAspecting(Graha graha)
		{
			foreach (var aspect in AspectTo)
			{
				if (aspect == graha)
				{
					return (true);
				}
			}

			return (false);

		}

		public bool IsAspectedBy(Body body)
		{
			var graha = _grahas.Find(body);
			return IsAspectedBy(graha);
		}

		public bool IsAspectedBy(Graha graha)
		{
			foreach (var aspect in AspectFrom)
			{
				if (aspect == graha)
				{
					return (true);
				}
			}
			return (false);
		}

		public bool IsConjuctWith(Body body)
		{
			var graha = _grahas.Find(body);
			return (IsConjuctWith(graha));
		}

		public bool IsConjuctWith(Graha graha)
		{
			foreach (var conjunct in Conjunct)
			{
				if (conjunct == graha)
				{
					return (true);
				}
			}
			return (false);
		}

		public bool IsConjunctWithPlanet
		{
			get
			{
				foreach (var conjunct in Conjunct)
				{
					if (conjunct.Type != GrahaType.Chaya)
					{
						return (true);
					}
				}
				return (false);

			}
		}

		public bool IsUnderInfluenceOf(Body body)
		{
			var graha = _grahas.Find(body);
			return IsUnderInfluenceOf(graha);
		}

		public bool IsUnderInfluenceOf(Graha graha)
		{
			if (IsAspectedBy(graha))
			{
				return (true);
			}

			if (IsConjuctWith(graha))
			{
				return (true);
			}

			if (IsAssociatedWith(graha))
			{
				return (true);
			}
			return (false);
		}

		public bool IsAssociatedWith(Body body)
		{
			var graha = _grahas.Find(body);
			return IsAssociatedWith(graha);
		}


		public bool IsNaturalBenefic
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
					var sun = _grahas.Find(Body.Sun);
					var tithi = _position.Longitude.Sub(sun._position.Longitude).ToTithi();
					if (tithi >= Tithi.KrishnaPratipada)
					{
						return (false);
					}
					return (true);
				}

				if (Body == Body.Mercury)
				{
					var jupiter = _grahas.Find(Body.Jupiter);
					if (jupiter.Strength >= 2)
					{
						return (true);
					}
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
								if (graha.IsNaturalBenefic == false)
								{
									return (false);
								}
							}
						}

						if ((Before.IsNaturalBenefic == false) && (After.IsNaturalBenefic == false))
						{
							return (false);
						}

						return (true);

					}
				}

				return (true);
			}
		}

		public bool IsNaturalMalefic
		{
			get
			{
				switch (Body)
				{
					case Body.Mars:
					case Body.Rahu:
					case Body.Ketu:
					case Body.Saturn:
						return (true);

					case Body.Jupiter:
					case Body.Venus:
						return (false);

					case Body.Mercury: 
						return (IsNaturalBenefic == false);
				}
				return (false);
			}
		}

		public Nature Nature => IsNaturalMalefic ? Nature.Malefic : Nature.Benefic;

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
						var lagna = _grahas.Find(Body.Lagna);
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

		private readonly bool _isRetrograde;
        public           bool IsRetrograde => _isRetrograde;

		public BodyType  BodyType    => _position.BodyType;
		public Body      Body        => _position.Name;
		public Bhava     Bhava       => _rashi.Bhava;
		public Rashi     Rashi       => _rashi;
		public Angle     HouseOffset => _houseOffset;
		public Angle     Angle       => _angle;
		public Horoscope Horoscope   => _grahas.Horoscope;

		public Conditions Conditions { get; private set; }

		private Graha _houseLord;
		public Graha HouseLord
		{
			get
			{
				if (_houseLord == null)
				{
					var lord = _dp.ZodiacHouse.SimpleLordOfZodiacHouse(); 
					_houseLord = _grahas.Find(lord);
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
					for (int index = 0; index < _grahas.Count; index++)
					{
						if (_grahas[index].Body == Body)
						{
							if (index == 0)
							{
								_before = _grahas.Last();
							}
							else
							{
								_before = _grahas[index - 1];
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
					for (int index = 0; index < _grahas.Count; index++)
					{
						if (_grahas[index].Body == Body)
						{
							if (index == _grahas.Count - 1)
							{
								_after = _grahas.First();
							}
							else
							{
								_after = _grahas[index + 1];
							}
							break;
						}
					}
				}
				return _after;
			}
		}


		private Graha    _exchange;
		public  Graha    Exchange => _exchange;
		public  Position Position => _position;

		public  List<Rashi>  Ownership    { get; }
		public  List<Graha>  RashiDrishti { get; }
		public  List <Graha> AspectFrom   { get; }
		public  List <Graha> AspectTo     { get; }
		public  List<Graha>  MutualAspect { get; }
		public  List<Graha>  Conjunct     { get; }
		public  List<Graha>  Association  { get; }
		public  List<Rashi>  OwnHouses    { get; }
		private double       _digBala;
        public  double       DigBala => _digBala;

        public bool HasMutualAspectWith(Graha graha) => IsAspectedBy(graha) && graha.IsAspecting(this);

		//Any planet which owns both a kendra and a trikona is called a Yoga-Karaka, this gives rise to highly benefic Raja Yoga
		//This Yoga confers the status of the individual in terms of success recognition and status..
        public bool YogaKaraka
        {
	        get
	        {
		        if (BodyType != BodyType.Graha)
		        {
			        return (false);
		        }
		        if (OwnHouses.Count == 1)
		        {
			        return false;
		        }

		        if (OwnHouses[0].Bhava.IsKendra() && OwnHouses[1].Bhava.IsTrikona())
		        {
			        return (true);
		        }
		        if (OwnHouses[0].Bhava.IsTrikona() && OwnHouses[1].Bhava.IsKendra())
		        {
			        return (true);
		        }

		        return (false);
	        }
        }

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
					if (graha.IsNaturalBenefic)
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
					if (graha.IsNaturalBenefic)
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
				if (Before.IsNaturalBenefic == false)
				{
					if (Bhava.HousesFrom(Before.Bhava) > 1)
					{
						return false;
					}
					if (After.IsNaturalBenefic == false)
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
				if (Before.IsNaturalBenefic)
				{
					if (Bhava.HousesFrom(Before.Bhava) > 1)
					{
						return false;
					}
					if (After.IsNaturalBenefic)
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

		public bool NeutralSign
		{
			get
			{
				if (FriendlySign)
				{
					return (false);
				}

				if (EnemySign)
				{
					return (false);
				}
				return (true);
			}
		}

		public bool IsTaraGraha  => Type == GrahaType.Tara;
		public bool IsLuminary   => Type == GrahaType.Luminary;
		public bool IsChayaGraha => Type == GrahaType.Chaya;

		public GrahaType Type
		{
			get
			{
				switch (Body)
				{
					case Body.Moon:
					case Body.Sun:
						return GrahaType.Luminary;
					case Body.Rahu:
					case Body.Ketu:
						return GrahaType.Chaya;
				}

				return GrahaType.Tara;

			}
		}

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
							if (DistanceTo(graha) < 1.0)
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
							return (_position.Latitude > graha._position.Latitude);
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
				if (BodyType != BodyType.Graha)
				{
					return (false);
				}
				foreach (var graha in Conjunct)
				{
					if (graha.Body == Body.Sun)
					{
						switch (Body)
						{
							case Body.Moon:    return (DistanceTo(graha) <= 12);
							case Body.Saturn:  return (DistanceTo(graha) <= 16);
							case Body.Jupiter: return (DistanceTo(graha) <= 11);
							case Body.Mars:
								if (IsRetrograde)
								{
									return (DistanceTo(graha) <= 17);
								}
								return (DistanceTo(graha) <= 8);
							case Body.Mercury:
								if (IsRetrograde)
								{
									return (DistanceTo(graha) <= 14);
								}
								return (DistanceTo(graha) <= 12);
							case Body.Venus:
								if (IsRetrograde)
								{
									return (DistanceTo(graha) <= 10);
								}
								return (DistanceTo(graha) <= 8);
						}
					}
				}
				return (false);
			}
		}

		public double DistanceTo(Graha graha) => (graha._position.Longitude - _position.Longitude);

		public double DistanceFrom(Graha graha) => (_position.Longitude - graha._position.Longitude);


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
			if (Body.IsFriend(graha.Body))
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

			if (Body.IsEnemy(graha.Body))
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

		public bool IsDebilitated
		{
			get
			{
				if (BodyType != BodyType.Graha)
				{
					return (false);
				}
				return (Rashi == Body.DebilitationSign());
			}
		}
		public bool IsExalted
		{
			get
			{
				if (BodyType != BodyType.Graha)
				{
					return (false);
				}
				return Rashi == Body.ExaltationSign();
			}
		}

		public bool IsMoolTrikona
		{
			get
			{
				if (BodyType != BodyType.Graha)
				{
					return (false);
				}
				return Rashi == Body.MooltrikonaSign();
			}
		}

		public bool IsInOwnHouse
		{
			get
			{
				if (BodyType != BodyType.Graha)
				{
					return (true);
				}
				return Rashi.Lord == this;
			}
		}

		public bool IsEcliped
		{
			get
			{
				return IsCombust; //Todo: proper definition
			}
		}

		//When a Neecha - Bhanga Yoga is present,the debilitation gets cancelled and is said to produce benefic results.
		public bool NeechaBhanga
		{
			get
			{
				if (IsDebilitated == false)
				{
					return (false);
				}

				//The debilitated planet is associated with or aspected by its exaltation sign's lord.
				var lord = _grahas.Rashis.Find(Body.ExaltationSign()).Lord;
				if (IsAspectedBy(lord))
				{
					return (true);
				}

				//The lord of the house where the planet is Exalted (I.e. the Exaltation lord of the planet)
				//is in kendra from the lagna or the Moon.
				if (lord.Bhava.IsKendra())
				{
					return (true);
				}

				if (Body != Body.Moon)
				{
					var moon  = _grahas.Find(Body.Moon);
					var bhava = (Bhava) lord.Bhava.HousesFrom(moon.Bhava);
					if (bhava.IsKendra())
					{
						return (true);
					}
				}

				//The debilitated planet is associated with or aspected by its debilitation sign's lord.
				foreach (var graha in MutualAspect)
				{
					if (graha.IsDebilitated)
					{
						return (true);
					}
				}

				//The lord of the house where the planet is debilitated (I.e. the debilitation lord of the planet)
				//is in kendra from the lagna or the Moon
				if (Rashi.Lord.Bhava.IsKendra())
				{
					return (true);
				}

				if (Body != Body.Moon)
				{
					var moon  = _grahas.Find(Body.Moon);
					var bhava = (Bhava) lord.Bhava.HousesFrom(moon.Bhava);
					if (bhava.IsKendra())
					{
						return (true);
					}
				}

				//The debilitated planet exchanges houses with its debilitation sign's lord.
				if (Exchange == Rashi.Lord)
				{
					return (true);
				}
				return (false);
			}
			
		}

		public Bhava HouseFrom(Body body)
		{
			var graha = _grahas.Find(body);
			return HouseFrom(graha);
		}

		public Bhava HouseFrom(Graha graha) => (Bhava) Bhava.HousesFrom(graha.Bhava);

		public bool IsBefore(Body body)
		{
			var graha = _grahas.Find(body);
			return (IsBefore(graha));
		}

		public bool IsBefore(Graha graha) => _position.Longitude < graha._position.Longitude;

		public bool IsAfter(Body body)
		{
			var graha = _grahas.Find(body);
			return (IsAfter(graha));
		}

		public bool IsAfter(Graha graha) => _position.Longitude > graha._position.Longitude;

		internal void Connect(Grahas grahas)
		{
			_grahas = grahas;
		}

		internal void Examine()
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

			_angle       =  (Bhava.Index() - 1) * 30.0;
			_angle       += _houseOffset;

			if (Bhava.IsKaraka(Body))
			{
				Conditions |= Conditions.KarakaPlanet;
			}

			foreach (var rashi in _grahas.Rashis)
			{
				if (Owns(rashi))
				{
					Ownership.Add(rashi);
				}
			}

			foreach (var graha in _grahas)
			{
				if (graha.Body == Body)
				{
					continue;
				}
				if (_dp.GrahaDristi(graha.Rashi.ZodiacHouse))
				{
					AspectTo.Add(graha);
				}

				if (graha._dp.GrahaDristi(Rashi.ZodiacHouse))
				{
					AspectFrom.Add(graha);
				}

				if (graha.Rashi.ZodiacHouse.RasiDristi(_rashi.ZodiacHouse))
				{
					RashiDrishti.Add(graha);
				}

				if (HouseLord.Body == graha.Body && Body == graha.HouseLord.Body)
				{
					_exchange = graha;
				}

				if (Rashi.ZodiacHouse == graha.Rashi.ZodiacHouse)
				{
					if (graha.Body != Body.Lagna)
					{
						Conjunct.Add(graha);
					}
				}

				if (IsAssociatedWith(graha))
				{
					Association.Add(graha);
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

		public int CompareTo(Graha graha, bool bSimpleLord, List<GrahaStrength> rules, out int winner)
		{
			winner = 0;
			foreach (GrahaStrength s in rules)
			{
				var result = _grahas.GetStronger(this, graha, bSimpleLord, s);
				if (result == 0)
				{
					winner++;
				}
				else
				{
					return result;
				}
			}
			return 0;
		}


		public Longitude CalculateLongitude(double ut, Ref <bool> isRetro)
		{
			if (this == Body.Lagna)
			{
				return new Longitude(Horoscope.Lagna(ut));
			}

			var bp = Horoscope.CalculateSingleBodyPosition(ut, Body.SwephBody(), Body, BodyType.Other);
			if (IsTaraGraha)
			{
				if (isRetro != null)
				{
					if (bp.SpeedLongitude >= 0)
					{
						isRetro.Value = false;
					}
					else
					{
						isRetro.Value = true;
					}
				}
			}
			else
			{
				isRetro.Value = false;
			}

			return bp.Longitude;
		}
	}
}
