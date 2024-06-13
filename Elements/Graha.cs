using System.Collections.Generic;
using System.Linq;
using Mhora.Calculation;
using Mhora.Definitions;
using Mhora.Elements.Extensions;
using Mhora.SwissEph;
using Mhora.Util;

namespace Mhora.Elements
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

			AspectFrom   = [];
			AspectTo     = [];
			MutualAspect = [];
			Conjunct     = [];
			RashiDrishti = [];
			Ownership    = [];
			Association  = [];
		}

		public static implicit operator Body  (Graha graha) => graha.Body;
		public static implicit operator Grahas(Graha graha) => graha._grahas;

		public string Name => Body.Name();

		public override string ToString() => _dp.ToString();

		public bool Owns(Bhava bhava)
		{
			var rashi = _grahas.Rashis[bhava];
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

		#region Associated
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

		public bool IsAssociatedWith(Body body)
		{
			var graha = _grahas[body];
			return IsAssociatedWith(graha);
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

		public bool IsMutualAssosiating(Graha graha) => IsAssociatedBy(graha) && IsAssociatedWith(graha);

		public bool IsAssociatedBy(Body body)
		{
			var graha = _grahas[body];
			return (IsAssociatedBy(graha));
		}

		public bool IsAssociatedBy(Graha graha) => graha.IsAssociatedWith(this);

#endregion

		#region drishti
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

		public bool HasDrishtiOn(ZodiacHouse zh) => _dp.GrahaDristi(zh);

		public bool IsAspecting(Body body)
		{
			var graha = _grahas[body];
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
			var graha = _grahas[body];
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
		#endregion

		#region conjunct
		public bool IsConjuctWith(Body body)
		{
			var graha = _grahas[body];
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

		public bool IsConjuctWith(Nature nature, bool noChayaGraha)
		{
			foreach (var conjunct in Conjunct)
			{
				if (conjunct.Nature == nature)
				{
					if ((noChayaGraha == false) || (conjunct.IsChayaGraha == false))
					{
						return (true);
					}
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
		#endregion

		public bool IsUnderInfluenceOf(Body body)
		{
			var graha = _grahas[body];
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
					var sun = _grahas[Body.Sun];
					var tithi = _position.Longitude.Sub(sun._position.Longitude).ToTithi();
					if (tithi >= Tithi.KrishnaPratipada)
					{
						return (false);
					}
					return (true);
				}

				if (Body == Body.Mercury)
				{
					var jupiter = _grahas[Body.Jupiter];
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
						var lagna = _grahas[Body.Lagna];
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
					_houseLord = _grahas[lord];
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
					for (var index = 0; index < _grahas.Count; index++)
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
					for (var index = 0; index < _grahas.Count; index++)
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
		        if (Ownership.Count == 1)
		        {
			        return false;
		        }

		        if (Ownership[0].Bhava.IsKendra() && Ownership[1].Bhava.IsTrikona())
		        {
			        return (true);
		        }
		        if (Ownership[0].Bhava.IsTrikona() && Ownership[1].Bhava.IsKendra())
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
				return Body switch
				       {
					       Body.Sun     => (Bhava == Bhava.KarmaBhava),
					       Body.Moon    => (Bhava == Bhava.SukhaBhava),
					       Body.Mars    => (Bhava == Bhava.KarmaBhava),
					       Body.Mercury => (Bhava == Bhava.LagnaBhava),
					       Body.Jupiter => (Bhava == Bhava.LagnaBhava),
					       Body.Venus   => (Bhava == Bhava.SukhaBhava),
					       Body.Saturn  => (Bhava == Bhava.JayaBhava),
					       _            => (false)
				       };
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
		private int _strength = int.MinValue;
		public int Strength
		{
			get
			{
				if (_strength != int.MinValue)
				{
					return (_strength);
				}
				_strength = 0;
				if (IsExalted)
                {
                    _strength += 2;
                }
				else if (IsInOwnHouse)
                {
                    _strength += 2;
                }

				if ((BodyType == BodyType.Graha) && (IsChayaGraha == false))
				{
					if (IsRetrograde)
					{
						_strength += 2;
					}
					if (Horoscope.DigBala (Body) > 30)
					{
						_strength += 2;
					}
				}

				var st = 0;
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
							_strength--;
						}
					}
				}

				if (st > 0)
				{
					_strength++;
				}
				else if (st < 0)
				{
					_strength--;
				}

				if (IsDebilitated)
				{
					_strength -= 2;
				}

				if (GrahaYuda)
				{
					if (WinnerOfWar)
					{
						_strength += 1;
					}
					else
					{
						_strength -= 1;
					}
				}

				return (_strength);
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

		public bool IsEcliped      => IsCombust; //Todo: proper definition
		public bool PushkarNavamsa => Position.PushkarNavamsa();
		public bool PushkaraBhaga  => Position.PushkaraBhaga();

		//When a Neecha - Bhanga Yoga is present,the debilitation gets cancelled and is said to produce benefic results.
		public bool NeechaBhanga
		{
			get
			{
				if (BodyType != BodyType.Graha)
				{
					return (false);
				}

				if (IsDebilitated == false)
				{
					return (false);
				}

				//The debilitated planet is associated with or aspected by its exaltation sign's lord.
				var lord = _grahas.Rashis[Body.ExaltationSign()].Lord;
				if (IsAspectedBy(lord))
				{
					return (true);
				}

				if (IsAssociatedWith(lord))
				{
					return (true);
				}

				//The lord of the house where the planet is Exalted (I.e. the Exaltation lord of the planet)
				//is in kendra from the lagna or the Moon.
				if (lord.Bhava.IsKendra())
				{
					return (true);
				}

				if (lord.HouseFrom(Body.Moon).IsKendra())
				{
					return (true);
				}

				//The debilitated planet is associated with or aspected by its debilitation sign's lord.
				lord = _grahas.Rashis[Body.DebilitationSign()].Lord;
				if (IsAssociatedWith(lord))
				{
					return (true);
				}

				if (IsAspectedBy(lord))
				{
					return (true);
				}

				//The lord of the house where the planet is debilitated (I.e. the debilitation lord of the planet)
				//is in kendra from the lagna or the Moon
				if (lord.Bhava.IsKendra())
				{
					return (true);
				}
				if (lord.HouseFrom(Body.Moon).IsKendra())
				{
					return (true);
				}

				//The debilitated planet exchanges houses with its debilitation sign's lord.
				if (Exchange == lord)
				{
					return (true);
				}
				return (false);
			}
			
		}

		public Bhava HouseFrom(Body body)
		{
			var graha = _grahas[body];
			return HouseFrom(graha);
		}

		public Bhava HouseFrom(Graha graha) => (Bhava) Bhava.HousesFrom(graha.Bhava);

		public bool IsBefore(Body body)
		{
			var graha = _grahas[body];
			return (IsBefore(graha));
		}

		public bool IsBefore(Graha graha) => _position.Longitude < graha._position.Longitude;

		public bool IsAfter(Body body)
		{
			var graha = _grahas[body];
			return (IsAfter(graha));
		}

		public bool IsAfter(Graha graha) => _position.Longitude > graha._position.Longitude;

		internal void Connect(Grahas grahas)
		{
			_grahas = grahas;
		}

		internal void Examine()
		{
			foreach (var rashi in _grahas.Rashis)
			{
				if (Owns(rashi))
				{
					Ownership.Add(rashi);
				}
			}

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

			if (FriendlySign)
			{
				Conditions |= Conditions.FriendlySign;
			}
			else if (EnemySign)
			{
				Conditions |= Conditions.EnemySign;
			}

			if (IsDigBala)
			{
				Conditions |= Conditions.DigBala;
			}

			if (IsCombust)
			{
				Conditions |= Conditions.Combust;
			}

			if (_grahas.Varga == DivisionType.Rasi)
			{
				if (PushkarNavamsa)
				{
					Conditions |= Conditions.PushkarNavamsa;
				}

				if (PushkaraBhaga)
				{
					Conditions |= Conditions.PushkaraBhaga;
				}
			}

			_angle       =  (Bhava.Index() - 1) * 30.0;
			_angle       += _houseOffset;

			if (Bhava.IsKaraka(Body))
			{
				Conditions |= Conditions.KarakaPlanet;
			}

			if (IsFunctionalBenefic)
			{
				Conditions |= Conditions.FunctionalBenefic;
			}

			if (IsFunctionalMalefic)
			{
				Conditions |= Conditions.FunctionalMalefic;
			}

			if (YogaKaraka)
			{
				Conditions |= Conditions.YogaKaraka;
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
			foreach (var s in rules)
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

		public Longitude CalculateLongitude(JulianDate ut, Ref <bool> isRetro)
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

		// Nakshtra			Visha Ghati	Longitude of Moon (from) 	Moon Zodiac Ends
		// Ashwini			51 to 54	Aries		11° 06′ 40″		12° 00′ 00″
		// Bharani			25 to 28	Aries		18° 40′ 00″		19° 33′ 20″
		// Krittika			31 to 34	Taurus		03° 20′ 00″		04° 13′ 20″
		// Rohini			41 to 44	Taurus		18° 53′ 20″		19° 46′ 40″
		// Mrigshira		15 to 18	Taurus		26° 26′ 40″		27° 20′ 00″
		// Ardra			22 to 25	Gemini		11° 20′ 00″		12° 13′ 20″
		// Punarvasu		31 to 34	Gemini		26° 40′ 00″		27° 33′ 20″
		// Pushya			21 to 24	Cancer		07° 46′ 40″		08° 40′ 00″
		// Ashlesha			33 to 36	Cancer		23° 46′ 40″		24° 40′ 00″
		// Magha			31 to 34	Leo			06° 40′ 00″		07° 33′ 20″
		// P. Phalguni		21 to 24	Leo			17° 46′ 40″		18° 40′ 00″
		// U. Phalguni		19 to 22	Virgo		00° 40′ 00″		01° 33′ 20″
		// Hasta			22 to 25	Virgo		14° 40′ 00″		15° 33′ 20″
		// Chitra			21 to 24	Virgo		27° 46′ 40″		28° 40′ 00″
		// Swati			15 to 18	Libra		09° 46′ 40″		10° 40′ 00″
		// Vishakha			15 to 18	Libra		23° 06′ 40″		24° 00′ 00″
		// Anuradha			11 to 14	Scorpio		05° 33′ 20″		06° 26′ 40″
		// Jyeshtha			15 to 18	Scorpio		19° 46′ 40″		20° 40′ 00″
		// Moola			57 to 60	Sagittarius	12° 26′ 40″		13° 20′ 00″
		// P. Ashada		25 to 28	Sagittarius	18° 40′ 00″		19° 33′ 20″
		// U. Ashada		21 to 24	Capricorn	01° 06′ 40″		02° 00′ 00″
		// Sravana			11 to 14	Capricorn	12° 13′ 20″		13° 06′ 40″
		// Dhanishta		11 to 14	Capricorn	25° 33′ 20″		26° 26′ 40″
		// Shatbhisha		19 to 22	Aquarius	10° 40′ 00″		11° 33′ 20″
		// P. Bhadrapada	17 to 20	Aquarius	23° 33′ 20″		24° 26′ 40″
		// U. Bhadrapada	25 to 28	Pisces		08° 40′ 00″		09° 33′ 40″
		// Revati			31 to 34	Pisces		23° 20′ 00″		24° 13′ 20″

		//Vishanadi is inauspicious for 4 Nadis. The 1st Nadi is said to bring ruin; the 2nd to adversely affect
		//the life of the individual; the 3rd to ruin everything and everyone; and the 4th is said to hurt the
		//prosperity of the family.
		// Exception: Rohini, Mrigshira, Ardra, Swati, Anuradha, Uttara Ashadha and Sravana have no significant Vishanadi.
		// Exception: The Moon in the 9th or 10th aspected by Jupiter overcomes Vishanadi.
		// Exception: The waxing Moon in exaltation, in own Navamsa, in the Lagna, or in Simhasanamsa removes the ill effects of Vishanadi.
		public bool VishaGhati
		{
			get
			{
				var l = Position.Longitude.ToZodiacHouseOffset();
				return Position.Longitude.ToNakshatra() switch
				{
					Nakshatra.Aswini         => l > new DmsPoint(11,  6, 40) && (l < new DmsPoint(12,  0,  0)),
					Nakshatra.Bharani        => l > new DmsPoint(18, 40,  0) && (l < new DmsPoint(19, 33, 20)),
					Nakshatra.Krittika       => l > new DmsPoint( 3, 20,  0) && (l < new DmsPoint( 4, 13, 20)),
					Nakshatra.Rohini         => l > new DmsPoint(18, 53, 20) && (l < new DmsPoint(19, 46, 40)),
					Nakshatra.Mrigarirsa     => l > new DmsPoint(26, 26, 40) && (l < new DmsPoint(27, 20,  0)),
					Nakshatra.Aridra         => l > new DmsPoint(11, 20,  0) && (l < new DmsPoint(12, 13, 20)),
					Nakshatra.Punarvasu      => l > new DmsPoint(26, 40,  0) && (l < new DmsPoint(27, 33, 20)),
					Nakshatra.Pushya         => l > new DmsPoint( 7, 46, 40) && (l < new DmsPoint( 8, 40,  0)),
					Nakshatra.Aslesha        => l > new DmsPoint(23, 46, 40) && (l < new DmsPoint(24, 40,  0)),
					Nakshatra.Makha          => l > new DmsPoint( 6, 40,  0) && (l < new DmsPoint( 7, 33, 20)),
					Nakshatra.PoorvaPhalguni => l > new DmsPoint(17, 46, 40) && (l < new DmsPoint(18, 40,  0)),
					Nakshatra.UttaraPhalguni => l > new DmsPoint( 0, 40,  0) && (l < new DmsPoint( 1, 33, 20)),
					Nakshatra.Hasta          => l > new DmsPoint(14, 40,  0) && (l < new DmsPoint(15, 33, 20)),
					Nakshatra.Chittra        => l > new DmsPoint(27, 46, 40) && (l < new DmsPoint(28, 40,  0)),
					Nakshatra.Swati          => l > new DmsPoint( 9, 46, 40) && (l < new DmsPoint(10, 40,  0)),
					Nakshatra.Vishaka        => l > new DmsPoint(23,  6, 40) && (l < new DmsPoint(44,  0,  0)),
					Nakshatra.Anuradha       => l > new DmsPoint( 5, 33, 40) && (l < new DmsPoint( 6, 26, 40)),
					Nakshatra.Jyestha        => l > new DmsPoint( 9, 46, 40) && (l < new DmsPoint(20, 40,  0)),
					Nakshatra.Moola          => l > new DmsPoint(12, 26, 40) && (l < new DmsPoint(13, 20,  0)),
					Nakshatra.PoorvaShada    => l > new DmsPoint(18, 40,  0) && (l < new DmsPoint(19, 33, 20)),
					Nakshatra.UttaraShada    => l > new DmsPoint( 1,  6, 40) && (l < new DmsPoint( 2,  0,  0)),
					Nakshatra.Sravana        => l > new DmsPoint(12, 13, 20) && (l < new DmsPoint(13,  6, 40)),
					Nakshatra.Dhanishta      => l > new DmsPoint(25, 33, 20) && (l < new DmsPoint(26, 26, 40)),
					Nakshatra.Satabisha      => l > new DmsPoint(10, 40,  0) && (l < new DmsPoint(11, 33, 20)),
					Nakshatra.PoorvaBhadra   => l > new DmsPoint(23, 33, 20) && (l < new DmsPoint(24, 26, 40)),
					Nakshatra.UttaraBhadra   => l > new DmsPoint( 8, 40,  0) && (l < new DmsPoint( 9, 33, 40)),
					Nakshatra.Revati         => l > new DmsPoint(23, 20,  0) && (l < new DmsPoint(24, 13, 20)),
					_                        => false
				};
			}
		}

		// Fatal Planetary Positions
		// Sign/Planet	La	Su	Mo  Ma	Me	Ju	Ve	Sa	Ra	Ke	Mandi
		// Aries		1	20	26	19	15	19	28	10	14	8	23
		// Taurus		9	9	12	28	14	29	15	4	13	18	24
		// Gemini		22	12	13	25	13	12	11	7	12	20	11
		// Cancer		22	6	25	23	12	27	17	9	11	10	12
		// Leo			25	8	24	28	9	6	10	12	24	21	13
		// Virgo		2	24	11	28	18	4	13	16	23	22	14
		// Libra		4	16	26	14	20	13	4	3	22	23	8
		// Scorpio		23	17	14	21	10	10	6	18	21	24	18
		// Sagittarius	18	22	13	2	21	17	27	28	10	11	20
		// Capricorn	20	2	25	15	22	11	12	14	20	12	10
		// Aquarius		10	3	5	11	7	15	29	13	18	13	21
		// Pisces		10	23	12	6	5	28	19	15	8	14	22
		public bool FatalDegree
		{
			get
			{
				return (false);
			}
		}
	}
}
