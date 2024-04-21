using System;
using Mhora.Calculation;
using Mhora.Definitions;
using Mhora.Util;

namespace Mhora.Elements.Extensions
{
	public static class Bhavas
	{
		[Flags]
		public enum Class : ushort
		{
			None          = 0x0000,
			Dharma        = 0x0001,
			Artha         = 0x0002,
			Kama          = 0x0004,
			Moksha        = 0x0008,
			Kendra        = 0x0010,
			Trikona       = 0x0020,
			Upachay       = 0x0040,
			Apachay       = 0x0080,
			Maraka        = 0x0010,
			MarmaShtana   = 0x0020,
			SukhaSthana   = 0x0080,
			Dushtana      = 0x0100,
			KutumbaSthana = 0x0200,
		}

		public enum PurusharthaType
		{
			Dharma = 0,
			Artha  = 1,
			Karma  = 2,
			Moksha = 3,
		}

		public static Class Classification (this Bhava bhava)
		{
			var classification = Class.None;

			switch (bhava.Purushartha())
			{
				case PurusharthaType.Artha:
				{
					classification |= Class.Artha;
				}
				break;

				case PurusharthaType.Dharma:
				{
					classification |= Class.Dharma;
				}
				break;

				case PurusharthaType.Karma:
				{
					classification |= Class.Kama;
				}
				break;

				case PurusharthaType.Moksha:
				{
					classification |= Class.Moksha;
				}
				break;
			}

			if (bhava.IsTrikona())
			{
				classification |= Class.Trikona;
			}

			if (bhava.IsKendra())
			{
				classification |= Class.Kendra;
			}

			if (bhava.IsMaraka())
			{
				classification |= Class.Maraka;
			}

			if (bhava.IsMarmaShtana())
			{
				classification |= Class.MarmaShtana;
			}

			if (bhava.IsApachay())
			{
				classification |= Class.Apachay;
			}

			if (bhava.IsUpachay())
			{
				classification |= Class.Upachay;
			}

			if (bhava.IsSukhaSthana())
			{
				classification |= Class.SukhaSthana;
			}

			if (bhava.IsDushtana())
			{
				classification |= Class.Dushtana;
			}

			if (bhava.IsKutumbaSthana())
			{
				classification |= Class.KutumbaSthana;
			}

			return classification;
		}

		public static PurusharthaType Purushartha(this Bhava bhava)
		{
			if (bhava.IsDharma())
			{
				return PurusharthaType.Dharma;
			}

			if (bhava.IsArtha())
			{
				return PurusharthaType.Artha;
			}

			if (bhava.IsKama())
			{
				return PurusharthaType.Karma;
			}

			return PurusharthaType.Moksha;
		}

		public static bool IsDhana(this Bhava bhava)
		{
			switch (bhava)
			{
				case Bhava.LagnaBhava:
				case Bhava.DhanaBhava:
				case Bhava.PutraBhava:
				case Bhava.DharmaBhava:
				case Bhava.LabhaBhava:
					return true;
			}
			return false;
		}

		public static bool IsDharma(this Bhava bhava)
		{
			switch (bhava)
			{
				case Bhava.LagnaBhava:
				case Bhava.PutraBhava:		
				case Bhava.DharmaBhava:
					return true;
			}
			return false;
		}

		public static bool IsArtha(this Bhava bhava)
		{
			switch (bhava)
			{
				case Bhava.DhanaBhava:
				case Bhava.ShatruBhava:
				case Bhava.KarmaBhava:
					return true;
			}

			return false;
		}

		public static bool IsKama(this Bhava bhava)
		{
			switch (bhava)
			{
				case Bhava.SahajaBhava:
				case Bhava.JayaBhava:
				case Bhava.LabhaBhava:
					return true;
			}
			return false;
		}

		public static bool IsMoksha(this Bhava bhava)
		{
			switch (bhava)
			{
				case Bhava.SukhaBhava:
				case Bhava.MrtyuBhava:
				case Bhava.VyayaBhava:
					return true;
			}

			return false;
		}

		public static bool IsTrikona (this Bhava bhava)
		{
			switch (bhava)
			{
				case Bhava.LagnaBhava:
				case Bhava.PutraBhava:		
				case Bhava.DharmaBhava:
					return true;
			}
			return false;
		}

		public static bool IsKendra(this Bhava bhava)
		{
			switch (bhava)
			{
				case Bhava.LagnaBhava:
				case Bhava.SukhaBhava:
				case Bhava.JayaBhava:
				case Bhava.KarmaBhava:
					return true;

			}
			return false;
		}

		public static bool IsMaraka(this Bhava bhava)
		{
			switch (bhava)
			{
				case Bhava.DhanaBhava:
				case Bhava.JayaBhava: 
					return true;
			}
			return false;
		}

		public static bool IsMarmaShtana(this Bhava bhava)
		{
			switch (bhava)
			{
				case Bhava.SukhaBhava:
				case Bhava.MrtyuBhava:
				case Bhava.VyayaBhava:
					return true;
			}
			return false;
		}

		public static bool IsApachay(this Bhava bhava)
		{
			switch (bhava)
			{
				case Bhava.LagnaBhava:
				case Bhava.DhanaBhava:
				case Bhava.SukhaBhava:
				case Bhava.JayaBhava:
				case Bhava.MrtyuBhava:
					return true;
			}
			return false;
		}

		public static bool IsUpachay(this Bhava bhava)
		{
			switch (bhava)
			{
				case Bhava.SahajaBhava:
				case Bhava.ShatruBhava:
				case Bhava.KarmaBhava:
				case Bhava.LabhaBhava:
					return true;

			}
			return false;
		}

		public static bool IsSukhaSthana(this Bhava bhava)
		{
			switch (bhava)
			{
				case Bhava.ShatruBhava:
				case Bhava.JayaBhava:
					return true;
			}
			return false;
		}

		public static bool IsDushtana(this Bhava bhava)
		{
			switch (bhava)
			{
				case Bhava.ShatruBhava:
				case Bhava.MrtyuBhava:
				case Bhava.VyayaBhava:
					return true;
			}
			return false;
		}

		public static bool IsTrishadaya(this Bhava bhava)
		{
			switch (bhava)
			{
				case Bhava.SahajaBhava:
				case Bhava.ShatruBhava:
				case Bhava.LabhaBhava:
					return true;
			}
			return false;
		}

		public static bool IsPanaphara(this Bhava bhava)
		{
			switch (bhava)
			{
				case Bhava.DhanaBhava:
				case Bhava.PutraBhava:
				case Bhava.MrtyuBhava:
				case Bhava.LabhaBhava:
					return true;
			}
			return false;
		}

		public static bool IsApoklima(this Bhava bhava)
		{
			switch (bhava)
			{
				case Bhava.SahajaBhava:
				case Bhava.ShatruBhava:
				case Bhava.DharmaBhava:
				case Bhava.VyayaBhava:
					return (true);
			}

			return false;
		}

		public static bool IsKutumbaSthana(this Bhava bhava)
		{
			switch (bhava)
			{
				case Bhava.DhanaBhava: 
					return true;
			}

			return false;
		}

		public static bool IsKaraka(this Bhava bhava, Body graha)
		{
			switch (bhava)
			{
				case Bhava.LagnaBhava:
					switch (graha)
					{
						case Body.Sun:
							return true;
					}
					break;
				case Bhava.DhanaBhava:
					switch (graha)
					{
						case Body.Jupiter:
							return true;
					}
					break;
				case Bhava.SahajaBhava:
					switch (graha)
					{
						case Body.Mars:
							return true;
					}
					break;
				case Bhava.SukhaBhava:
					switch (graha)
					{
						case Body.Moon:
						case Body.Mercury:
							return true;
					}
					break;
				case Bhava.PutraBhava:
					switch (graha)
					{
						case Body.Jupiter:
							return true;
					}
					break;
				case Bhava.ShatruBhava:
					switch (graha)
					{
						case Body.Mars:
						case Body.Saturn:
						case Body.Ketu:
							return true;
					}
					break;
				case Bhava.JayaBhava:
					switch (graha)
					{
						case Body.Venus:
							return true;
					}
					break;
				case Bhava.MrtyuBhava:
					switch (graha)
					{
						case Body.Saturn:
							return true;
					}
					break;
				case Bhava.DharmaBhava:
					switch (graha)
					{
						case Body.Jupiter:
							return true;
					}
					break;
				case Bhava.KarmaBhava:
					switch (graha)
					{
						case Body.Jupiter:
						case Body.Sun:
						case Body.Mercury:
						case Body.Saturn:
							return true;
					}
					break;
				case Bhava.LabhaBhava:
					switch (graha)
					{
						case Body.Jupiter:
							return true;
					}
					break;
				case Bhava.VyayaBhava:
					switch (graha)
					{
						case Body.Saturn:
						case Body.Rahu:
							return true;
					}
					break;
				}
			
			return false;
		}

		public static int HousesTo(this Bhava bhava, Bhava other)
		{
			var houses = other.Index() - bhava.Index() + 1;

			if (houses <= 0)
			{
				houses = 12 + houses;
			}

			return houses;
		}

		public static int HousesFrom(this Bhava bhava, Bhava other)
		{
			return (other.HousesTo(bhava));
		}

		public static Bhava Add(this Bhava bhava, int nr)
		{
			var result = (bhava.Index() + nr).NormalizeInc(1, 12);

			return (Bhava) result;
		}

		public static Body NaisargikaKaraka (this Bhava bhava)
		{
			switch (bhava)
			{
				case Bhava.LagnaBhava:  return Body.Sun;
				case Bhava.DhanaBhava:  return Body.Jupiter;
				case Bhava.SahajaBhava: return Body.Mars;
				case Bhava.SukhaBhava:  return Body.Moon;
				case Bhava.PutraBhava:  return Body.Jupiter;
				case Bhava.ShatruBhava: return Body.Mars;
				case Bhava.JayaBhava:   return Body.Venus;
				case Bhava.MrtyuBhava:  return Body.Saturn;
				case Bhava.DharmaBhava: return Body.Jupiter;
				case Bhava.KarmaBhava:  return Body.Mercury;
				case Bhava.LabhaBhava:  return Body.Jupiter;
				case Bhava.VyayaBhava:  return Body.Saturn;
			}

			throw new Exception("Invalid bhava");
		}

		// D1 - Rasi chart
		// 1H – Body, fame, place of birth, self, honour, self respect, peace of mind, present life,
		//		complexion,	health, longevity, head, brain, hair and stamina.
		// 2H – Wealth, family, speech, eating habits, taste, status or self esteem, death, face, eyes,
		//		nose, nails, teeth, tongue.
		// 3H – Courage and valour, hobbies, younger coborns, mode of death, communication, neighbours,
		//		short journey, ear, neck, throat, shoulders, upper limbs, physical growth.
		// 4H – Mother, happiness or unhappiness, lands, houses, vehicles, education, own family, chest,
		//		lungs, heart, breast.
		// 5H – Progeny, intelligence, poorvapunya, mantra Siddhi, speculation, learning, fascinations,
		//		writing, heart, upper abdomen, liver, gal bladder, mental illness or soundness.
		// 6H – Enemy, debt, disease, accident, injury, competition, maternal relations, servants, lower abdomen,
		//		intestine, surgical operations.
		// 7H – Spouse, partners, marriage, passion, status, urinary tract, semen.
		// 8H – Longevity, death, obstacles, suddenness, maglaya for females, unexpected gains or losses, inheritance,
		//		hidden talents, incurable disease, external genital, mental anguish.
		// 9H – Religious inclinations, father, guru, pilgrimages, long journey, charity, fame and fortune, temples,
		//		hips, thigh.
		// 10H – Profession, karmas, source of livelihood, business, power, sacred and religious deeds and duties,
		//		knee joints and knee cap.
		// 11H – Gains of all material things, income, fulfilment of desires, reward and punishment,
		//		elder coborn, legs, ear, recovery from disease.
		// 12H – Expenditure, separation, loss, bedroom, sleep, confinement, hospitalisation, foreign,
		//		renunciation, mental balance, feet, eye.

		// D2 - Labh Mandook Hora or Kerala Hora
		// 1H – General strength and nature to acquire wealth and means used to accumulate the wealth.
		//		Self perception of wealth.
		// 2H – Wealth accumulation. Available resources at ones disposal.
		// 3H – Effort put in to earn wealth. Relation with younger siblings in connection to your wealth.
		// 4H – Happiness/unhappiness derived from wealth and your speech. The source or direction of happiness
		//		in earning or spending money. Happiness derived from chartiy or otherwise.
		// 5H – Various means employed to earn money. Wealth of the spouse. Monetary relations with progeny.
		//		Popularity earned by money.
		// 6H – Obstacles faced in earning money. Loss of money of the partners.
		// 7H – Attractions which makes one earn money. Wealth kept with others.
		// 8H – Inheritance of property or wealth. Unaccounted money or the money received from others which
		//		you do not remember. Ups and down and changes in status of wealth.
		// 9H – The religion of money i.e. the protection from wealth. Luck in earning. Travels undertaken
		//		to earn wealth.
		// 10H – Work done for earning through rightful means. Money related activities in society.
		// 11H – Easy monetory gains. Desires and skills of gaining all types of wealth.
		// 12H – Loss of wealth. Expenditure of all types.

		// D3 - Drekkana
		// 1H – General well being of the co-born and the relations of native with co-born.
		//		Promise of having co-born or not. The general initiative and drive. Inclinations towards life.
		// 2H – Loss of courage and initiative. Expenses of younger coborn. Gain of wealth from co-born.
		// 3H – Focus of action in society. Success or failure of actions. Immediate younger co-born.
		// 4H – Happiness of and from co-born.
		// 5H – Planning of actions. Thinking towards co-born. Second younger co-born.
		// 6H – Hindrances in getting the fruits of actions (Karma).
		// 7H – Sexual inclinations and desires.
		// 8H – Longevity, Khara, obstacles in fulfilment of desires.
		// 9H – Luck factor in accomplishing the goals.
		// 10H – Action undertaken to achieve desires.
		// 11H – Fulfilment, getting the desired object, Lust.
		// 12H – Loss of action, losses and separation from co-born.

		// D4 - Chaturthamsha
		// 1H – The use of wealth for enjoyment/sufferings and fulfilment of desires through wealth accumulation
		//		for happiness.
		// 2H – Capacity to accumulate happiness from possessions. Habits acquired from the wealth.
		// 3H – Risks taken for earning happiness. Courage to retain the happiness. Change in residence.
		// 4H – Fixed and liquid assets. The sense of happiness and contentment.
		// 5H – Poorva Punya in regard to happiness and fate. Long lasting assets.
		// 6H – Losses due to enemy. Division of wealth.
		// 7H – Acceptance of our wealth and happiness by public i.e. the outward image in regard to your wealth.
		// 8H – Losses of permanent nature. Mercury is considered good in 8th house.
		// 9H – Religion with the help of wealth and for happiness i.e. donations, charity done for the religion.
		// 10H – Actions or karma related to wealth. Afflicted tenth house gives losses through own actions.
		// 11H – Gains of wealth by goodwill. Easy assets.
		// 12H – Wasteful expenditure, end of happiness, expenditure done to satisfy lust. Twelfth lord in 4th house
		//		then lives in someone else house.

		// D6 - saptvimshamsha 
		// 1H – General proneness to disease and trouble by enemy. Self acquired problems.
		// 2H – Eating habits and the effect therefrom on the health. A benefic is welcome here.
		// 3H – Capacity to fight disease and enemy. Benefic aspect is required to ensure health and freedom from enemy.
		// 4H – Support of family in maintaining health. The planet associated with fourth house is the support giver.
		// 5H – Problems or comfort due to poorvapunya. Mental attitude towards illness or enemies. Inborn diseases.
		// 6H – Curable disease, debt, enemy. Outward illness.
		// 7H – Helping hands or trouble makers.
		// 8H – Chronic or incurable disease. Permanent enemies.
		// 9H – Luck factor in dealing with diseases and enemy.
		// 10H – Problems created by own action or inaction.
		// 11H – Sixth from sixth house and the strongest upchaya house. Expansion of disease and problems.
		// 12H – confinement, release from disease or enemy.

		// D7 - saptamsha
		// 1H – The promise of having children and their well being. It also shows the luck factor which is carried
		//		from past life.
		// 2H – The actions (Karmas) which are influenced by past life i.e. the inbuilt samskar.
		//		General wealth of the children.
		// 3H – General acts of courage by children. The fruits of your past karmas.
		// 4H – Happiness from children and of children.
		// 5H – Poorva punya house. First child. A strong fifth house is a must to make a person enjoy
		//		this life since it governs your mental inclination and intelligence.
		// 6H – Problems from children. Diseases of the children. Suffering of partner on account of child.
		// 7H – Third from fifth house. Relationship with opposite sex. Conceivement of child.
		// 8H – Longevity of the relationship with opposite sex. Longevity of children. Obstacles in the
		//		life of children. Miscarriage.
		// 9H – Grand children, luck of children.
		// 10H – Diseases of children and the care they get from parents. Actions of children in general.
		// 11H – Gains from children, desires the parents cherish from progeny.
		// 12H – Longevity of progeny. Expenses incurred for nurturing children.

		// D8 - Ashtamsha 
		// 1H – Obstacles to enemies. Relief from debt and illness.
		// 2H – Problems to partners and spouse. Longevity of spouse.
		// 3H – Sharing of inheritance and partition of property. Longevity of the person.
		// 4H – Gain of inheritance. Happiness or suffering due to legacy. Death of father.
		// 5H – Sudden developments in the career. Scheming or making plans.
		// 6H – Problems to elder brother and friends. Receiving gifts from seniors.
		// 7H – Imprisonment or punishment and also release from confinement and hospitalisation.
		// 8H – Scandals. Unearned money, accidents, insurance claims, death, longevity, chronic problems.
		// 9H – Sudden loss of money and reputation.
		// 10H – Obstacles or death to brothers and sisters, legacy gained by them, trend of events.
		// 11H – Inheritance from mother, family disputes, vehicular accidents.
		// 12H – Problems to children, perverted thinking, sudden gains from speculation, and losses in profession.

		// D9 - Navamsha
		// 1H - self and this will thus represent one’s commitment to the norms
		//		of dharma towards self to protect all the creations of this world.
		// 2H - family, income, speech, emotions, etc. It is equally a trine (5th) to the 10th house indicating
		//		‘punya karma’ that will be inherited by a person as the 10th house represents the karma stahana.
		//		This house being the 8th house from the 7th house, so the second bhava of the navamsha represents
		//		the secret affairs of a person other than marriage. Since Moon exalts in the house as per kaal -
		//		pursha kundali, it depicts the mind of a person as Moon rules the mind and emotions.
		// 3H - parakrama, viz, actions and urge to achieve. It shows what a person will like to perform and achieve.
		//		It equally shows skills, crafts, hobbies and the artistic pursuits of a person. It represents younger
		//		co - borns as well. It is one of the important ‘up - chhaya house’, the house of growth.
		// 4H - mother, motherland, society, one’s personal comforts/ discomforts etc.
		//		It will thus represent one’s commitment to dharma in all such spheres of one’s activities.
		// 6H - indicated hurdles, bottlenecks, hardships, diseases, court cases etc, as per the main birth chart 
		//		and applies equally in the navamsha chart as well. This house being the 12th to the 7th house,
		//		it indicates the hurdles etc, to the marriage and the matrimonial relationship as well.
		//		It is not always a bad bhava as it 9th and the 10th houses from the 10th house and the 9th house
		//		of a birth or a navmasha hart as well. Thus known as ‘ardhi - trikona. So it is equally a very important
		//		house for ones growth and karma. It signifies competitions and will to fight back.
		//		It is one of the important ‘up - chhaya’ houses, the house of growth.
		// 7H - spouse and the other family members. Here the family does not mean the immediate family only,
		//		but it includes the whole family where commitment to dharma (good deeds) needs to be performed
		//		as the head of the whole family, which includes one’s parents and one’s immediate relatives as well.
		// 8H - It has all the significations as that of a birth chart for the navamsha chart as well. It being the
		//		2nd house from the 7th house, thus its marka sthana as well. It is the ‘mangalya sthana’ of a female viz,
		//		the house of longevity of marriage
		// 10H - the karma sathana, which is universal dharma where commitment to dharma (good deeds) needs
		//		to be performed to protect all the creation of the supreme creator. It will include compassion and
		//		humility to the whole humanity as also to the other lives other than humans (Sarwadharma).
		//		This bhava (house) creates a bigger karmic influence and responsibility to be performed as a better
		//		dharma (better deeds).
		// 11H - being the 2nd house from the 10th house thus its growth and income. It is the house of growth and is
		//		an important ‘up - chahaya house’, the house of growth. It is an important house of the navamsha chart
		//		from the growth and the material prosperity point of view. It is the 3rd house from the 9th house,
		//		thus represents the parakrama house, the house of actions for ones destiny, dharma and Guru and ones father.
		// 12H - This house has again all the significations of that of a birth chart. It is the house of loss and denial.
		//		This house signifies the bed comforts as well. It is the house of moksha as well.
		//		
		//		The 1st, 5th and the 9th houses from the navamsha lagna are called as navamsha trikonas.
		//		The trines or the trikonas are otherwise very important houses (bhavas) of the lagna chart and in the
		//		case of the navamsha chart, these houses are equally very important houses and depict the capabilities
		//		and the nature of a person. As already mentioned above that the navamsha shows the earned fruits of
		//		one’s dharma acquired from the previous births, its trines depict such dharmic or adarmic (religious or
		//		irreligious) traits of a person in this birth, acquired from the previous births. Benefic planets in the
		//		trines of a navamsha chart show the results as follows:
		//		Jupiter: Blessings of Lord                     Shiva, Moon: Blessings of Saraswati,
		//		Mercury: Blessings of Vishnu, Venus: Blessings of Laxmi



		// D10 - Dashamsha
		// 1H – The beginning of the career. Perception of career. The strength of your karmas i.e.
		//		all actions. A strong Lagna gives a solid foundation of your career.
		// 2H – The resources of wealth, status and speech available for your career and activities.
		// 3H – Creativity and imagination towards profession. It is the communication skill in your career.
		// 4H – General sense of comfort and happiness you derive from your profession. It denotes the inward
		//		emotional level of your work.
		// 5H – Your standing in the professional field. The well wishers of your career and those who benefit
		//		from you. Shrewdness in profession.
		// 6H – The rivalries in the career. The obstacles which you face in service.
		// 7H – Associates in your profession. Business partners.
		// 8H – Hidden activities, underhand dealings for gains in profession. New ways to achieve your goals.
		//		Problems in career and end of career.
		// 9H – The preceptor and the guidance received from him. The rightful path of the profession.
		// 10H – Overall activities and the sum total of all actions. Status achieved in profession. Immediate boss.
		// 11H – Gains in the career.
		// 12H – End of career. Charities made in profession. Secret activities against your career.

		// D11 - Ekadashamsha
		// 1H – Personal gains/losses of all types.
		// 2H – Gains through the family and your status in such gains. Can give gains through speech.
		// 3H – Gains from coborns, neighbours and by communications and writing. Expenditure on comfort.
		// 4H – Gains from property both movable or immovable, the happiness or sorrow derived from them.
		// 5H – Intellectual gains. Speculation, lottery or gains through progeny. Expenditure on progeny.
		// 6H – Enemy house and earning from them. This house gives earning through commission, hospitals
		//		or by court cases of others.
		// 7H – Gains from spouse or the public at large. Gains after marriage.
		// 8H – By new inventions, inheritance, gains of a psychological doctor, donations for an ashram.
		// 9H – Gains from father, religion or Guru.
		// 10H – From the king which include both reward and punishment.
		// 11H – Gains of all types and from any source.
		// 12H – Gains from abroad, it may be that he receives money etc. from some source abroad. By luxury items.

		// D12 - Dwadashamsha
		// 1H – General relations with the parents/grand parents. The general condition of the parental
		//		family and the influence a person gets from his family.
		// 2H – The sanskar of the family. The wealth and social status of the family and parents.
		//		The support native gets from parents.
		// 3H – It is maraka place for father. Efforts of parents.
		// 4H – Mother, It is eighth from 9th and 9th from 8th house and shows the longevity of father.
		//		Happiness of family.
		// 5H – Past karmas of parents. It is 9th from ninth so it denotes the grand parents.
		//		It is also fifth from Lagna and is the child of the parents as such it represents the person
		//		whose chart is seen. Thus the relation of fifth house with Sun, Moon, 9th house and 4th house
		//		will indicate the relation of native with parents. It also shows the future generation.
		// 6H – Debts of the parents which may be inherited by the native. Profession of father.
		//		The diseases and capacity to fight.Diseases of the parents which are passed on to the native.
		// 7H – The partners to promote or to continue the lineage of the family. The spouse of the native is seen from it.
		// 8H – Inheritance from the parents. This can be the help taken from them.
		// 9H – The house for father and the dharma of the family. The religious training received from them.
		// 10H – Maraka house for both mother and father. It is the karma house of the parents.
		//		The karmas performed by parents towards their parents.
		// 11H – The gains from the parents. This may be monetary help taken from the parents by the native
		//		and inter-alia shows the gains of the parents which they can share with you.
		// 12H – This house indicate your lineage, Gotra etc. This house is 4th from 9th i.e. your paternal
		//		grand mother. It is also 9th from 4th i.e. your maternal grand father. The gotra of the family
		//		is decided by the planets in 12th or the lord of the 12th house along with modifying influence on them.

		// D16 - Shodashamsha
		// 1H – General level and sources of luxury. The quantum of various luxuries is seen from
		//		the strength of Lagna.
		// 2H – The upbringing which moulds your attitude towards the happiness.
		//		The status of your happiness. Accumulation of happiness.
		// 3H – The courage exhibited or the acts of courage to get the luxury. It can give loss of
		//		vehicles and unhappiness by own doing. Any relation of third house/lord with Lagna/Lagna lord
		//		or fourth house/lord promise such losses.
		// 4H – Happiness or unhappiness from the luxuries of life. This house is the sukh sthana.
		//		It allows us to taste the nectar or poison of luxuries. The attitude toward happiness.
		//		Any relation with sixth, eighth or twelfth house indicate vehicular or other accidents which causes
		//		loss of happiness.
		// 5H – It is an important house as it stands for your karmas of past life in regard to your luxuries of this life.
		// 6H – Sixth house is the enemy of your luxuries. This house shows the loans you raise to acquire
		//		any luxury. The vehicular accidents are also ruled by this house. It shows litigations or disputes
		//		due to conveyance.
		// 7H – Partners who enjoy or suffer the luxuries with you or contribute to your happiness.
		//		The lord of 4th house of natal chart and related to 7th house of shodashamsha gives conveyance
		//		provided by others and not your own.
		// 8H – Long term problems due to the luxuries or vehicles. Fatal accidents due to conveyance of any type.
		//		Afflicted planets in 8th house need to be watched during their dasha.
		// 9H – The luck factor of luxuries. It is the house for long travels also. A strong 9th house makes
		//		one possess good luxuries and he enjoys it.
		// 10H – The use of conveyance. It can be for good purpose or bad purpose. Actions for comforts in life.
		// 11H – Gain or loss of the luxuries. The level of greed towards the amenities of life.
		//		Relation of Lagna and eleventh and with malefic connection indicate a materialistic person
		//		whose greed can never be satisfied.
		// 12H – Loss of the amenities. The losses can be due to theft or accident or by litigation (sixth house).

		// D20 - Vimshamsha 
		// 1H – The self, the Yogi, good deeds for self, self nature to renounce. A strong Lagna is a must to be
		//		interested in the path of spirituality and have achievement of blessings in worship. T
		//		his is the first house of Dharma trine. Desire for name and fame.
		// 2H – The sanskars for worship, speech used in religious activity. Holy deeds and the sacred text to promote
		//		the activity. It is the nectar of wisdom if well associated. Lord of second house is the Kula Devta.
		// 3H – Control of mind, pilgrimages undertaken for religious purpose, daily personal religious duties,
		//		temples, the advice given by guru (updesha), spiritual practice through writing or speaking.
		//		Present day Dharma gurus have a strong 3rd house.
		// 4H – Character you observe, the faith and trust in your own pursuits and the amount of happiness derived.
		//		The seat of God, your good deeds towards society, forgiveness or the depth of tolerance.
		//		This is first house of Moksha trine. This is the house of action of religious activities like kirtan
		//		or meditation. Lord of fourth house is the Gram or Village Devta.
		// 5H – The Mantra Siddhi or to achieve the desired goals, the devotion and emotion in your prayers,
		//		the wisdom, the thoughts, enlightenment, Poorva punya, the deity to be worshipped.
		//		The nature of fifth lord indicates the path of progress i.e. Sun is donation.
		// 6H – Obstacles in the path of worship or Mantra Siddhi, It is maraka house for fifth house and is
		//		killer of Mantra Sadhna. It shows the agony and worries of religious pursuits. A good sixth house
		//		gives strong determination to progress or fight the obstacles. Service to the deity.
		// 7H – It is the house of Dhyan (è;ku). It shows peace and the result of religious efforts granted by God.
		//		This house indicate the community involvement in worship i.e. singing Bhajan or Kirtans.
		//		It is the house for charities and celebrations. It also gives growth of religious activity.
		// 8H – Samadhi. The act of renunciation or taking Sanyasa. The Karmas caused by self are killed in
		//		eighth house. It also shows the intuitive power and the secrets of worship. Deep mental anguish
		//		is also seen from eighth house. This is the second house of Moksha trine and is an important house.
		// 9H – Kindness (n;k). Good conduct, pilgrimage, guru, charity, internal purity of mind, relation with guru,
		//		temples, receitation of the mantra (japa). Ninth house is the last house of Dharma trine.
		//		Depth of devotion. The spiritual path of Guru and the lineage of Guru. Lord of ninth house is the
		//		Ishta Devata or Dharma Devta.
		// 10H – Karma (deZ). Right path, dignity, sacred spells, acquisition of power of Mantra.
		//		 Feel of the God. Tenth house indicate your patience in worship or the strength of your determination.
		// 11H – Niyam, fulfillment or realization, desire to worship God, desire to have gains, Sattwik nature.
		// 12H – Salvation or final emancipation, ashram of guru (4th from 9th), meditation, confinement,
		//		 sleeping condition for worship. Ability to attain Samadhi.


		// D24 - Chaturvimshamsha
		// 1H – The general level of education. Interest in learning. An afflicted Lagna is not good for educational
		//		achievement. A strong Lagna is a must to have higher education. Type of education a person is interested
		//		in or is of his liking.
		// 2H – The family background and the support one gets from family. The final retention of the education one has
		//		taken. The financial position of family for your education. Basic studies. Memory.
		// 3H – Efforts done to get the education. The attitude towards education. Colleagues of school or Guru Bhai
		//		and relation with them. Short travel for studies. Attention one pays toward studies.
		//		Junior students in school.
		// 4H – The happiness of education or the difficulties faced. The formal education which is pursued from home.
		//		In the modern context it represent education upto graduation.
		// 5H – Intelligence of a person. Field of education . A strong fifth house gives a sharp brain.
		//		Higher education. This is the house to be seen for the subjects which a person study.
		// 6H – Competitors in education or learning. This does not indicate all the students but only the ones
		//		with whom you compete. The obstacles in educational life. A strong sixth house or a malefic
		//		in 6th gives a sense of competition and the person gets high ranks in class.
		// 7H – Activities in the student life or the educational life. The cooperation or otherwise of the fellow students.
		// 8H – Hidden aspects of education. The activities which are secretly undertaken without the knowledge of
		//		other students. One student who studies in night and bully others in day time comes under the preview
		//		of this house. Using unfair means. Research related education. Slow learning. Break in education.
		// 9H – The teacher who imparts you education and knowledge. The sincerity and religiousness in your
		//		studies and the respect you give to your teacher. Long travel for education.
		// 10H – The status in the educational field. Popularity in the school. How the teachers rate you in your
		//		 overall educational activity. How a person will teach his students. The actions in the educational field.
		// 11H – Gains derived from education like awards, scholarships and medals. Friends and seniors in school
		//		or college and the relationship one have with them.
		// 12H – Education obtained at a place other than birth place. Foreign studies i.e. foreign language or in
		//		foreign land. Expenditure incurred in education.

		// D27 - Saptavimshamsha

		// D30 - Trimshamsha
		// 1H – Weakness of self and the miseries i.e. the problems or happiness generated by own doings.
		//		Problems acquired in this birth.
		// 2H – Happiness or miseries due to the family one is born, whether you get happiness or sorrows from family.
		// 3H – Courage to face the miseries of life. Problems arising from neighbours and coborn.
		// 4H – The character and the problems due to your behaviour at home. It is the house of aggrevation or cure
		//		of the problems. Problems due to property.
		// 5H – Good or bad intelligence which cause happiness or trouble. Perverted thoughts. Trouble from progeny.
		// 6H – It is house of enemy and disease. The diseases of the body with known causes like accident.
		// 7H – Outside world and people at large. The problems arising generally through the society, village etc.
		//		These can be through spouse. It shows your public behaviour and etiquette.
		// 8H – The congenital miseries which you are born with. The weaknesses which are carried from past karmas.
		//		These are permanent and cannot be remedied. The diseases which cannot be cured and become chronic.
		// 9H – Miseries arising out of disrespect to religion or guru or Gods. One should seek their blessings
		//		instead of curses.
		// 10H – Problems due to wilful wrong action. The mental attitude which give happiness or otherwise.
		//		The karmas which cause it.
		// 11H – Greed and the miseries due to greed. The best solution is to avoid greed consciously and get
		//		happiness instead of miseries.
		// 12H – Problems which one faces in foreign country. The place of hospital and sleep.

		// D40 - Khavedamsha
	}
}
