﻿using System.Collections.Generic;
using System.Linq;
using Mhora.Definitions;
using Mhora.Util;

namespace Mhora.Elements.Yoga
{
	public static class Yoga
	{
		//The Seven planets occupy the seven contiguous houses starting from houses other than the kendras
		//The native is a commander of an army, honored by the ruler, good in looks, brave and wealthy.
		public static bool AakritiArdhachandra(this DivisionType varga)
		{
			var planets = Graha.Planets(varga);
			var first   = planets.First();
			if (first.Bhava.IsKendra())
			{
				return (false);
			}

			foreach (var graha in planets)
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
			var planets = Graha.Planets(varga);
			var bhava   = Bhava.LagnaBhava;

			foreach (var graha in planets)
			{
				if (graha.Bhava != bhava)
				{
					return (false);
				}

				bhava += 2;
			}

			return (true);
		}

		//All planets located in houses 7 to 1.
		//A native with this yoga is scholarly, wise, kindhearted, high status, looking after his dependents,
		//long lived, comfortable in the early and concluding portions of his life.
		public static bool AakritiChhatra(this DivisionType varga)
		{
			var planets = Graha.Planets(varga);

			foreach (var graha in planets)
			{
				if (graha.Bhava.Index() > 7)
				{
					return (false);
				}
			}

			return (true);
		}

		//All planets located in the four houses from the tenth to Lagna.
		//Such a native is indigent, servile, rejected, bereft of comforts, devoid of near and dear ones and cruel hearted.
		public static bool AakritiDanda(this DivisionType varga)
		{
			var planets = Graha.Planets(varga);

			foreach (var graha in planets)
			{
				if ((graha.Bhava.Index() < 10) && (graha.Bhava != Bhava.LagnaBhava))
				{
					return (false);
				}
			}

			return (true);
		}

		//All planets located in houses 10 to 4.
		//The native is brave, a jailer, proud, skilled in archery, a thief, a wanderer, happy in the middle portion of his life.
		public static bool AakritiDhanush(this DivisionType varga)
		{
			var planets = Graha.Planets(varga);

			foreach (var graha in planets)
			{
				if ((graha.Bhava.Index() > 4) && (graha.Bhava.Index() < 10))
				{
					return (false);
				}
			}

			return (true);
		}

		//All planets in two adjacent kendras.
		//The native with this yoga is wealthy, learned, versed in mantras, tantras and music,
		//fearsome, envious of others and ever engaged in earning money.
		public static bool AakritiGada(this DivisionType varga)
		{
			var planets = Graha.Planets(varga);

			var first = planets[0];
			if (first.Bhava.IsKendra() == false)
			{
				return (false);
			}

			foreach (var graha in planets)
			{
				var placement = graha.Bhava.HousesFrom(first.Bhava);
				if ((placement != 1) && (placement != 7))
				{
					return (false);
				}
			}

			return (true);
		}

		//All planets in houses 4, 8, 12.
		//One born in this yoga is gluttonous, servile, blessed with friends, liked by good people, living by agriculture.
		public static bool AakritiHalaMoksha(this DivisionType varga)
		{
			var planets = Graha.Planets(varga);
			foreach (var graha in planets)
			{
				if (graha.Bhava.IsMoksha() == false)
				{
					return (false);
				}
			}
			return (true);
		}

		//All planets in houses 3, 7, 11.
		//One born in this yoga is gluttonous, servile, blessed with friends, liked by good people, living by agriculture.
		public static bool AakritiHalaKama(this DivisionType varga)
		{
			var planets = Graha.Planets(varga);
			foreach (var graha in planets)
			{
				if (graha.Bhava.IsKama() == false)
				{
					return (false);
				}
			}
			return (true);
		}

		//All planets in houses 2, 6, 10.
		//One born in this yoga is gluttonous, servile, blessed with friends, liked by good people, living by agriculture.
		public static bool AakritiHalaArtha(this DivisionType varga)
		{
			var planets = Graha.Planets(varga);
			foreach (var graha in planets)
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
			var planets = Graha.Planets(varga);

			foreach (var graha in planets)
			{
				if ((graha.Bhava.Index() < 4) && (graha.Bhava.Index() > 10))
				{
					return (false);
				}
			}

			return (true);
		}

		//All planets located in houses 1 to 7
		//This makes the native famous, miserly, greedy, ambitious, of fickle nature, earning through water related pursuits.
		public static bool AakritiNauka(this DivisionType varga)
		{
			var planets = Graha.Planets(varga);

			foreach (var graha in planets)
			{
				if (graha.Bhava.Index() > 7)
				{
					return (false);
				}
			}

			return (true);
		}


		//All planets in houses 4 and 10
		//The native is a wanderer, servile, quarrelsome, a message bearer,and an ambassador
		public static bool AakritiPakshi(this DivisionType varga)
		{
			var planets = Graha.Planets(varga);

			foreach (var graha in planets)
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
			var planets = Graha.Planets(varga);
			var bhava   = Bhava.DhanaBhava;

			foreach (var graha in planets)
			{
				if (graha.Bhava != bhava)
				{
					return (false);
				}

				bhava += 2;
			}
			return (true);
		}

		//All planets in houses 1 and 7
		//One born in this yoga suffers penury and privations, ill health, lean body, discord with a wicked wife
		//and earning only through hard labor.
		public static bool AakritiShakata(this DivisionType varga)
		{
			var planets = Graha.Planets(varga);

			foreach (var graha in planets)
			{
				if ((graha.Bhava != Bhava.LagnaBhava) && (graha.Bhava != Bhava.JayaBhava))
				{
					return (false);
				}
			}

			return (true);
		}

		//All planets located in houses 7 to 10.
		//The native is poor, lazy, long lived, combative, argumentative, good to look at, stable and tormented by failures.
		public static bool AakritiShakti(this DivisionType varga)
		{
			var planets = Graha.Planets(varga);

			foreach (var graha in planets)
			{
				if ((graha.Bhava < Bhava.JayaBhava) && (graha.Bhava > Bhava.KarmaBhava))
				{
					return (false);
				}
			}

			return (true);
		}

		//All planets located in houses 4 to 7
		//One born in this yoga is a native who manufactures arrows, is cruel and wicked, a hunter, a jailer, fond of animal food.
		public static bool AakritiShara(this DivisionType varga)
		{
			var planets = Graha.Planets(varga);

			foreach (var graha in planets)
			{
				if ((graha.Bhava != Bhava.SukhaBhava) && (graha.Bhava != Bhava.JayaBhava))
				{
					return (false);
				}
			}

			return (true);
		}

		//All planets in trikonas 1, 5, 9.
		//The native is blessed with comforts fond of combat, courageous, very wise, wealthy,
		//devoted to his first wife, and indifferent to the second one.
		public static bool AakritiShringataka(this DivisionType varga)
		{
			var planets = Graha.Planets(varga);

			foreach (var graha in planets)
			{
				if (graha.Bhava.IsTrikona() == false)
				{
					return (false);
				}
			}

			return (true);
		}

		//All planets falling in houses other than the four kendras.
		//The native this yoga in his horoscope is ever engaged in accumulating wealth, has small but lasting comforts,
		//long life, sweet- tongued and tends to hoard his wealth and possessions.
		public static bool AakritiVaapi(this DivisionType varga)
		{
			var planets = Graha.Planets(varga);

			foreach (var graha in planets)
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
			var planets = Graha.Planets(varga);

			foreach (var graha in planets)
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
	}
}
