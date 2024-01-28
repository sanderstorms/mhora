using System.Collections.Generic;
using System.Linq;
using Mhora.Definitions;
using Mhora.Util;

namespace Mhora.Elements.Yoga
{
	public static class Yoga
	{
		public static List<Graha> Grahas (DivisionType varga)
		{
			var grahas = Grahas(varga);
			grahas.RemoveAll(graha => graha.Body == Body.Lagna);
			return grahas;
		}

		//The Seven grahas occupy the seven contiguous houses starting from houses other than the kendras
		//The native is a commander of an army, honored by the ruler, good in looks, brave and wealthy.
		public static bool AakritiArdhachandra(this DivisionType varga)
		{
			var grahas = Graha.Planets(varga);
			var first   = grahas.First();
			if (first.Bhava.IsKendra())
			{
				return (false);
			}

			foreach (var graha in grahas)
			{
				if (graha.Conjunct.Count > 0)
				{
					return (false);
				}

				if (graha.Bhava.HousesFrom(first.Bhava) > 7)
				{
					return (false);
				}
			}

			return (true);
		}

		//All Planets in six alternate houses starting from the lagna.
		//The native is of lovely looks, illustrious, a king or his equivalent.
		public static bool AakritiChakra(this DivisionType varga)
		{
			var bhava   = Bhava.LagnaBhava;
			var grahas = Grahas(varga);

			foreach (var graha in grahas)
			{
				if (graha.Bhava != bhava)
				{
					return (false);
				}

				bhava += 2;
			}

			return (true);
		}

		//All grahas located in houses 7 to 1.
		//A native with this yoga is scholarly, wise, kindhearted, high status, looking after his dependents,
		//long lived, comfortable in the early and concluding portions of his life.
		public static bool AakritiChhatra(this DivisionType varga)
		{
			var grahas = Grahas(varga);

			foreach (var graha in grahas)
			{
				if (graha.Bhava.Index() > 7)
				{
					return (false);
				}
			}

			return (true);
		}

		//All grahas located in the four houses from the tenth to Lagna.
		//Such a native is indigent, servile, rejected, bereft of comforts, devoid of near and dear ones and cruel hearted.
		public static bool AakritiDanda(this DivisionType varga)
		{
			var grahas = Grahas(varga);

			foreach (var graha in grahas)
			{
				if ((graha.Bhava.Index() < 10) && (graha.Bhava != Bhava.LagnaBhava))
				{
					return (false);
				}
			}

			return (true);
		}

		//All grahas located in houses 10 to 4.
		//The native is brave, a jailer, proud, skilled in archery, a thief, a wanderer, happy in the middle portion of his life.
		public static bool AakritiDhanush(this DivisionType varga)
		{
			var grahas = Grahas(varga);

			foreach (var graha in grahas)
			{
				if ((graha.Bhava.Index() > 4) && (graha.Bhava.Index() < 10))
				{
					return (false);
				}
			}

			return (true);
		}

		//All grahas in two adjacent kendras.
		//The native with this yoga is wealthy, learned, versed in mantras, tantras and music,
		//fearsome, envious of others and ever engaged in earning money.
		public static bool AakritiGada(this DivisionType varga)
		{
			var grahas = Grahas(varga);

			var first = grahas[0];
			if (first.Bhava.IsKendra() == false)
			{
				return (false);
			}

			foreach (var graha in grahas)
			{
				var placement = graha.Bhava.HousesFrom(first.Bhava);
				if ((placement != 1) && (placement != 7))
				{
					return (false);
				}
			}

			return (true);
		}

		//All grahas in houses 4, 8, 12.
		//One born in this yoga is gluttonous, servile, blessed with friends, liked by good people, living by agriculture.
		public static bool AakritiHalaMoksha(this DivisionType varga)
		{
			var grahas = Grahas(varga);
			foreach (var graha in grahas)
			{
				if (graha.Bhava.IsMoksha() == false)
				{
					return (false);
				}
			}
			return (true);
		}

		//All grahas in houses 3, 7, 11.
		//One born in this yoga is gluttonous, servile, blessed with friends, liked by good people, living by agriculture.
		public static bool AakritiHalaKama(this DivisionType varga)
		{
			var grahas = Grahas(varga);
			foreach (var graha in grahas)
			{
				if (graha.Bhava.IsKama() == false)
				{
					return (false);
				}
			}
			return (true);
		}

		//All grahas in houses 2, 6, 10.
		//One born in this yoga is gluttonous, servile, blessed with friends, liked by good people, living by agriculture.
		public static bool AakritiHalaArtha(this DivisionType varga)
		{
			var grahas = Grahas(varga);
			foreach (var graha in grahas)
			{
				if (graha.Bhava.IsArtha() == false)
				{
					return (false);
				}
			}
			return (true);
		}

		//Planets located in houses 4 to 10.
		//The result is an untruthful native, a jailer, cruel, a resident of forts or hills or forests, a wrestler,
		//ignorant of what is right and wrong.
		public static bool AakritiKoota(this DivisionType varga)
		{
			var grahas = Grahas(varga);

			foreach (var graha in grahas)
			{
				if ((graha.Bhava.Index() < 4) && (graha.Bhava.Index() > 10))
				{
					return (false);
				}
			}

			return (true);
		}

		//All grahas located in houses 1 to 7
		//This makes the native famous, miserly, greedy, ambitious, of fickle nature, earning through water related pursuits.
		public static bool AakritiNauka(this DivisionType varga)
		{
			var grahas = Grahas(varga);

			foreach (var graha in grahas)
			{
				if (graha.Bhava.Index() > 7)
				{
					return (false);
				}
			}

			return (true);
		}


		//All grahas in houses 4 and 10
		//The native is a wanderer, servile, quarrelsome, a message bearer,and an ambassador
		public static bool AakritiPakshi(this DivisionType varga)
		{
			var grahas = Grahas(varga);

			foreach (var graha in grahas)
			{
				if ((graha.Bhava != Bhava.SukhaBhava) && (graha.Bhava != Bhava.KarmaBhava))
				{
					return (false);
				}
			}

			return (true);
		}

		//Planets in six alternate houses starting from the Second houses.
		//The native is wealthy, renowned, blessed with physical pleasures, likeable, stable of mind.
		public static bool AakritiSamudra(this DivisionType varga)
		{
			var grahas = Grahas(varga);
			var bhava   = Bhava.DhanaBhava;

			foreach (var graha in grahas)
			{
				if (graha.Bhava != bhava)
				{
					return (false);
				}

				bhava += 2;
			}
			return (true);
		}

		//All grahas in houses 1 and 7
		//One born in this yoga suffers penury and privations, ill health, lean body, discord with a wicked wife
		//and earning only through hard labor.
		public static bool AakritiShakata(this DivisionType varga)
		{
			var grahas = Grahas(varga);

			foreach (var graha in grahas)
			{
				if ((graha.Bhava != Bhava.LagnaBhava) && (graha.Bhava != Bhava.JayaBhava))
				{
					return (false);
				}
			}

			return (true);
		}

		//All grahas located in houses 7 to 10.
		//The native is poor, lazy, long lived, combative, argumentative, good to look at, stable and tormented by failures.
		public static bool AakritiShakti(this DivisionType varga)
		{
			var grahas = Grahas(varga);

			foreach (var graha in grahas)
			{
				if ((graha.Bhava < Bhava.JayaBhava) && (graha.Bhava > Bhava.KarmaBhava))
				{
					return (false);
				}
			}

			return (true);
		}

		//All grahas located in houses 4 to 7
		//One born in this yoga is a native who manufactures arrows, is cruel and wicked, a hunter, a jailer, fond of animal food.
		public static bool AakritiShara(this DivisionType varga)
		{
			var grahas = Grahas(varga);

			foreach (var graha in grahas)
			{
				if ((graha.Bhava != Bhava.SukhaBhava) && (graha.Bhava != Bhava.JayaBhava))
				{
					return (false);
				}
			}

			return (true);
		}

		//All grahas in trikonas 1, 5, 9.
		//The native is blessed with comforts fond of combat, courageous, very wise, wealthy,
		//devoted to his first wife, and indifferent to the second one.
		public static bool AakritiShringataka(this DivisionType varga)
		{
			var grahas = Grahas(varga);

			foreach (var graha in grahas)
			{
				if (graha.Bhava.IsTrikona() == false)
				{
					return (false);
				}
			}

			return (true);
		}

		//All grahas falling in houses other than the four kendras.
		//The native this yoga in his horoscope is ever engaged in accumulating wealth, has small but lasting comforts,
		//long life, sweet- tongued and tends to hoard his wealth and possessions.
		public static bool AakritiVaapi(this DivisionType varga)
		{
			var grahas = Grahas(varga);

			foreach (var graha in grahas)
			{
				if (graha.Bhava.IsKendra())
				{
					return (false);
				}
			}

			return (true);
		}

		//Benefices in houses 1 and 7, Malefic in 4 and 10.
		//The native is good in look, brave, wickedly disposed and happy during early life and old age.
		public static bool AakritiVajra(this DivisionType varga)
		{
			var grahas = Grahas(varga);

			foreach (var graha in grahas)
			{
				if (graha.IsBenefic)
				{
					if ((graha.Bhava != Bhava.LagnaBhava) && (graha.Bhava != Bhava.JayaBhava))
					{
						return (false);
					}
				}
				else
				{
					if ((graha.Bhava != Bhava.SukhaBhava) && (graha.Bhava != Bhava.KarmaBhava))
					{
						return (false);
					}
				}
			}

			return (true);
		}

		//Malefic in houses 1 and 7, Benefices in houses 4 and 10.
		//Given to restraint and auspicious pursuits, such a native is consistent in nature,
		//wealthy, charitable and happy in the middle portion of his life.
		public static bool AakritiYava(this DivisionType varga)
		{
			var grahas = Grahas(varga);

			foreach (var graha in grahas)
			{
				if (graha.IsBenefic)
				{
					if ((graha.Bhava != Bhava.SukhaBhava) && (graha.Bhava != Bhava.KarmaBhava))
					{
						return (false);
					}
				}
				else
				{
					if ((graha.Bhava != Bhava.LagnaBhava) && (graha.Bhava != Bhava.JayaBhava))
					{
						return (false);
					}
				}
			}

			return (true);
		}

		//All grahas located in houses 1 to 4
		//The native is contented, very learned, wealthy, valorous, blessed with home comforts and pursues his wordy duties.
		public static bool AakritiYoopa(this DivisionType varga)
		{
			var grahas = Grahas(varga);

			foreach (var graha in grahas)
			{
				if (graha.Bhava.Index() > 4)
				{
					return (false);
				}
			}

			return (true);
		}

		//All grahas in Sthira Sign and several other grahas are also in Sthira sign.
		//One born in this yoga is proud, learned, wealthy, liked by the ruler, famous, of a stable nature and blessed with several sons.
		public static bool AashrayaMusala(this DivisionType varga)
		{
			var grahas = Grahas(varga);

			foreach (var graha in grahas)
			{
				if (graha.Rashi.ZodiacHouse.IsFixedSign() == false)
				{
					return (false);
				}
			}
			return (true);
		}

		//All grahas in Dwiswabhava Sign and several other grahas are also in Dwiswabhava sign.
		//One born in this yoga is defective of a limb, resolute, very clever, of fluctuating wealth,
		//good to look at and fond of his near and dear ones.
		public static bool AashrayaNala(this DivisionType varga)
		{
			var grahas = Grahas(varga);

			foreach (var graha in grahas)
			{
				if (graha.Rashi.ZodiacHouse.IsDualSign() == false)
				{
					return (false);
				}
			}
			return (true);
		}

		//All grahas in Chara Sign and several other grahas are also in Chara sign.
		//One born in this yoga is fond of travel, of good looks, ambitious, cruel and delights to frequent alien lands in pursuit of wealth.
		public static bool AashrayaRajju(this DivisionType varga)
		{
			var grahas = Grahas(varga);

			foreach (var graha in grahas)
			{
				if (graha.Rashi.ZodiacHouse.IsMoveableSign() == false)
				{
					return (false);
				}
			}
			return (true);
		}

		//This yoga is caused by the presence of a natural benefic in the 10 houses from lagna or Moon.
		//A native with this yoga is revered by the ruler, blessed with physical pleasures, likeable and helpful and enjoys a lasting fame.
		public static bool AmalaKirti(this DivisionType varga)
		{
			var grahas = Grahas(varga);
			var moon    = grahas.Find(graha => graha.Body == Body.Moon);

			foreach (var graha in grahas)
			{
				if (graha.IsBenefic)
				{
					if (graha.Bhava == Bhava.KarmaBhava)
					{
						return (true);
					}

					if (graha.Bhava.HousesFrom(moon.Bhava) == 10)
					{
						return (true);
					}
				}
			}

			return (false);
		}

		//All Benefic in kendras
		//The person will achieve the ownership of vast lands. The person will gain abundant wealth.
		public static bool AmaraYogaBenefics(this DivisionType varga)
		{
			var grahas = Grahas(varga);

			bool positiveYoga = true;

			foreach (var graha in grahas)
			{
				if (graha.IsBenefic)
				{
					positiveYoga &= graha.Bhava.IsKendra();
				}
			}

			return (positiveYoga);

		}

		//All malefic in kendras.
		//The person will achieve the ownership of vast lands. The person will gain abundant wealth.
		public static bool AmaraYogaMalefics (this DivisionType varga)
		{
			var grahas = Grahas(varga);

			bool negativeYoga = true;

			foreach (var graha in grahas)
			{
				if (graha.IsBenefic == false)
				{
					negativeYoga &= graha.Bhava.IsKendra();
				}
			}

			return (negativeYoga);
		}

		//Rahu is in 1st house and Ketu is in 7th house and all the grahas fall on one side of Rahu/Ketu axis
		public static bool AnantKalsarpa(this DivisionType varga)
		{
			var grahas = Grahas(varga);
			bool left = true;
			bool right = true;

			var rahu = Graha.Find(Body.Rahu, varga);
			if (rahu.Bhava != Bhava.LagnaBhava)
			{
				return (false);
			}

			foreach (var graha in Graha.Planets(varga))
			{
				left  |= (graha.Bhava <= Bhava.JayaBhava);
				right |= (graha.Bhava  > Bhava.JayaBhava);
			}

			return (left |right);
		}


		//All seven planets occupying one house each from the lagna to seventh house starting with the 9th house.
		//Powerful well to do, saintly, a devout performer of sacrifices.
		public static bool BhagyaMalika(this DivisionType varga)
		{
			var   grahas     = Graha.Planets(varga);
			Bhava firstBhave = grahas.First().Bhava;

			if (firstBhave == Bhava.MrtyuBhava)
			{
				return (false);
			}

			for (int index = 1; index < grahas.Count; index++)
			{
				if (grahas[index].Bhava == Bhava.MrtyuBhava)
				{
					return (false);
				}
				if (firstBhave.HousesTo(grahas[index].Bhava) != index)
				{
					return (false);
				}
			}
			return (true);
		}

		//The lord of the 9 being strong and the lagna lord as also Jupiter and Venus occupy kendras.
		//One born in this yoga is of a royal bearing, of noble birth, well- behaved, well with wife, sons and fame, bereft of disease.
		public static bool Bheri(this DivisionType varga)
		{
			var lagnaRashi = Rashi.Find(Bhava.LagnaBhava, varga);
			var lagnaLord  = lagnaRashi.ZodiacHouse.LordOfSign();
			var graha      = Graha.Find(lagnaLord, varga);
			if (graha.Rashi.Bhava.IsKendra() == false)
			{
				return (false);
			}

			graha = Graha.Find(Body.Jupiter, varga);
			if (graha.Rashi.Bhava.IsKendra() == false)
			{
				return (false);
			}

			graha = Graha.Find(Body.Venus, varga);
			if (graha.Rashi.Bhava.IsKendra() == false)
			{
				return (false);
			}

			var ninthRashi = Rashi.Find(Bhava.DharmaBhava, varga);
			var ninthLord = ninthRashi.ZodiacHouse.LordOfSign();
			graha = Graha.Find(ninthLord, varga);
			if (graha.IsDebilitated)
			{
				return (false);
			}

			if (graha.IsExalted || graha.IsMoolTrikona || graha.IsInOwnHouse)
			{
				return (true);
			}

			return (false);
		}
	}
}
