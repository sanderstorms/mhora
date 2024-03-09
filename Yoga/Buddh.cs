using Mhora.Definitions;
using Mhora.Elements;

namespace Mhora.Yoga
{
	public static class Buddh
	{
		//Conjunction between Mercury and Jupiter.
		//Eloquent, learned, amiable, handsome, wealthy, well-versed in dance, song and music, very virtuous and fond of perfumes.
		public static bool BuddhGuru(this Grahas grahaList)
		{
			var mercury = grahaList.Find(Body.Mercury);
			return mercury.IsConjuctWith(Body.Jupiter);
		}

		//Mercury and Jupiter are conjunct in 10th House.
		//A minister or a king, distinguished, amiable, a scholar.
		public static bool BuddhGuru10(this Grahas grahaList)
		{
			var jupiter = grahaList.Find(Body.Jupiter);
			if (jupiter.Bhava != Bhava.KarmaBhava)
			{
				return (false);
			}

			return BuddhGuru(grahaList);
		}

		//Mercury and Jupiter are Conjunct in Lagna.
		//Very handsome, learned, honored by the ruler, blessed with pleasures and vehicles.
		public static bool BuddhGuru1(this Grahas grahaList)
		{
			var jupiter = grahaList.Find(Body.Jupiter);
			if (jupiter.Bhava != Bhava.LagnaBhava)
			{
				return (false);
			}

			return BuddhGuru(grahaList);

		}

		//Mercury and Jupiter are Conjunct in 7th House.
		//Excels his father, very strong, dominates his opponents, blessed with wife, wealth and friends.
		public static bool BuddhGuru7(this Grahas grahaList)
		{
			var jupiter = grahaList.Find(Body.Jupiter);
			if (jupiter.Bhava != Bhava.JayaBhava)
			{
				return (false);
			}

			return BuddhGuru(grahaList);

		}

		//Mercury and Jupiter are Conjunct in 9th House.
		//Versed in scriptures, learned, wealthy, sweet-tongued, adept in fine arts
		public static bool BuddhGuru9(this Grahas grahaList)
		{
			var jupiter = grahaList.Find(Body.Jupiter);
			if (jupiter.Bhava != Bhava.DharmaBhava)
			{
				return (false);
			}

			return BuddhGuru(grahaList);
		}

		//Mercury and Jupiter are Conjunct in 4th House.
		//Blessed with friends, comforts, wife and wealth, honored by the ruler.
		public static bool BuddhGuru4(this Grahas grahaList)
		{
			var jupiter = grahaList.Find(Body.Jupiter);
			if (jupiter.Bhava != Bhava.SukhaBhava)
			{
				return (false);
			}

			return BuddhGuru(grahaList);
		}

		//Conjunction between Mercury and Mars.
		//Adept in making medicines, eloquent, not very rich, looks after a widow or a base woman,
		//versed in metal-craft and fine arts, a wrestler or boxer.
		public static bool BuddhMangal(this Grahas grahaList)
		{
			var mercury = grahaList.Find(Body.Mercury);
			return mercury.IsConjuctWith(Body.Mars);
		}

		//Mercury and Mars are Conjunct in Lagna.
		//Given to violence or killing, adept in fire-related pursuits, an ambassador or a confidant.
		public static bool BuddhMangal1(this Grahas grahaList)
		{
			var mercury = grahaList.Find(Body.Mercury);
			if (mercury.Bhava != Bhava.LagnaBhava)
			{
				return (false);
			}

			return BuddhMangal(grahaList);
		}

		//Mercury and Mars are Conjunct in 9th house
		//Commander of an army, brave, strong, honored by the king.
		public static bool BuddhMangal9(this Grahas grahaList)
		{
			var mercury = grahaList.Find(Body.Mercury);
			if (mercury.Bhava != Bhava.DharmaBhava)
			{
				return (false);
			}

			return BuddhMangal(grahaList);
		}

		//Mercury and Mars are Conjunct in 10th house
		//Given to travelling far and wide, quarrelsome, good looking, loses his first wife.
		public static bool BuddhMangal10(this Grahas grahaList)
		{
			var mercury = grahaList.Find(Body.Mercury);
			if (mercury.Bhava != Bhava.KarmaBhava)
			{
				return (false);
			}

			return BuddhMangal(grahaList);
		}

		//Mercury and Mars are Conjunct in 7th house
		//Bereft of relatives, blessed with friends, wealth, conveyances and pleasures, suffers humiliation at the hands of his own people.
		public static bool BuddhMangal7(this Grahas grahaList)
		{
			var mercury = grahaList.Find(Body.Mercury);
			if (mercury.Bhava != Bhava.JayaBhava)
			{
				return (false);
			}

			return BuddhMangal(grahaList);
		}

		//Mercury and Mars are Conjunct in 4th house
		//Blessed with his friends, children, comforts, fame and fortune.
		public static bool BuddhMangal4(this Grahas grahaList)
		{
			var mercury = grahaList.Find(Body.Mercury);
			if (mercury.Bhava != Bhava.SukhaBhava)
			{
				return (false);
			}

			return BuddhMangal(grahaList);
		}

		//Conjunction between Mercury and Saturn
		//Of a sickly constitution, learned, wealthy, provides sustenance to many, quarrelsome,
		//fickle-minded, adept in several arts, disobedient to his elders, a cheat.
		public static bool BuddhShani(this Grahas grahaList)
		{
			var mercury = grahaList.Find(Body.Mercury);
			return mercury.IsConjuctWith(Body.Saturn);
		}

		//Conjunction between Mercury and Saturn in Lagna
		//Ugly, sinful, devoid of learning, wealth, and vehicles, short-lived.
		public static bool BuddhShani1(this Grahas grahaList)
		{
			var mercury = grahaList.Find(Body.Mercury);
			if (mercury.Bhava != Bhava.LagnaBhava)
			{
				return (false);
			}

			return BuddhShani(grahaList);
		}

		//Conjunction between Mercury and Saturn in 4th house
		//Without relatives, food and drink, humiliated by his own people.
		public static bool BuddhShani4(this Grahas grahaList)
		{
			var mercury = grahaList.Find(Body.Mercury);
			if (mercury.Bhava != Bhava.SukhaBhava)
			{
				return (false);
			}

			return BuddhShani(grahaList);
		}

		//Conjunction between Mercury and Saturn in 7th house
		//Devoted to god, doing good to others, resorts to falsehood.
		public static bool BuddhShani7(this Grahas grahaList)
		{
			var mercury = grahaList.Find(Body.Mercury);
			if (mercury.Bhava != Bhava.JayaBhava)
			{
				return (false);
			}

			return BuddhShani(grahaList);
		}

		//Conjunction between Mercury and Saturn in 10th house
		//Destroys his opponents, famous for his worldly possessions, devoted to gods, guru and Brahmins.
		public static bool BuddhShani10(this Grahas grahaList)
		{
			var mercury = grahaList.Find(Body.Mercury);
			if (mercury.Bhava != Bhava.KarmaBhava)
			{
				return (false);
			}

			return BuddhShani(grahaList);
		}


		//Conjunction between Mercury and Saturn in 9th house
		//Ailing, wealthy, very talkative, thinks ill of others.
		public static bool BuddhShani9(this Grahas grahaList)
		{
			var mercury = grahaList.Find(Body.Mercury);
			if (mercury.Bhava != Bhava.DharmaBhava)
			{
				return (false);
			}

			return BuddhShani(grahaList);
		}

		//Conjunction between Mercury and Venus.
		//Eloquent, virtuous, well versed in scriptural learning, extremely wealthy, a fine sculptor, adept in music,
		//well dressed, owner of lands,
		public static bool BuddhShukra(this Grahas grahaList)
		{
			var mercury = grahaList.Find(Body.Mercury);
			return mercury.IsConjuctWith(Body.Venus);
		}

		//Mercury and Venus are Conjunct in Lagna.
		//Good in looks, learned, honored by the ruler, distinguished, devoted to gods and Brahmins.
		public static bool BuddhShukra1(this Grahas grahaList)
		{
			var mercury = grahaList.Find(Body.Mercury);
			if (mercury.Bhava != Bhava.LagnaBhava)
			{
				return (false);
			}
			return BuddhShukra(grahaList);

		}
		//Mercury and Venus are Conjunct in 4th house.
		//Blessed with friends and progeny, indulges in good deeds, a minister.
		public static bool BuddhShukra4(this Grahas grahaList)
		{
			var mercury = grahaList.Find(Body.Mercury);
			if (mercury.Bhava != Bhava.LagnaBhava)
			{
				return (false);
			}
			return BuddhShukra(grahaList);

		}

		//Mercury and Venus are Conjunct in 7th house.
		//Attached to several women, plenty of physical and material pleasures
		public static bool BuddhShukra7(this Grahas grahaList)
		{
			var mercury = grahaList.Find(Body.Mercury);
			if (mercury.Bhava != Bhava.LagnaBhava)
			{
				return (false);
			}
			return BuddhShukra(grahaList);

		}

		//Mercury and Venus are Conjunct in 9th house.
		//Famous, learned, stable, fortunate, keeps his promise.
		public static bool BuddhShukra9(this Grahas grahaList)
		{
			var mercury = grahaList.Find(Body.Mercury);
			if (mercury.Bhava != Bhava.LagnaBhava)
			{
				return (false);
			}
			return BuddhShukra(grahaList);

		}

		//Mercury and Venus are Conjunct in 10tgh house.
		//Versed in philosophical knowledge, famous in high status, not very rich, fulfils his undertakings.
		public static bool BuddhShukra10(this Grahas grahaList)
		{
			var mercury = grahaList.Find(Body.Mercury);
			if (mercury.Bhava != Bhava.LagnaBhava)
			{
				return (false);
			}
			return BuddhShukra(grahaList);

		}

		//Jupiter is in Lagna, Moon in a Kendra from Jupiter, Rahu in the 2nd from Moon, Sun and Mars in 3rd from Lagna.
		//The person has matchless strength is conversant with scriptures very talented and renowned.
		public static bool Buddha(this Grahas grahaList)
		{
			var jupiter = grahaList.Find(Body.Jupiter);
			if (jupiter.Bhava != Bhava.LagnaBhava)
			{
				return (false);
			}

			var moon = grahaList.Find(Body.Moon);
			var bhava = (Bhava) (jupiter.Bhava.HousesFrom(moon.Bhava));
			if (bhava.IsKendra() == false)
			{
				return false;
			}

			var rahu = grahaList.Find(Body.Rahu);
			if (rahu.Bhava.HousesFrom(moon.Bhava) != 2)
			{
				return false;
			}

			var sun = grahaList.Find(Body.Sun);
			if (sun.Bhava != Bhava.SahajaBhava)
			{
				return (false);
			}
			if (sun.Conjunct.Count > 0)
			{
				foreach (var graha in sun.Conjunct)
				{
					if (graha.Body == Body.Mars)
					{
						return (true);
					}
				}
			}

			return (false);
		}

		//Conjunction between Mercury, Jupiter, and Saturn.
		//Prosperous, blessed with physical comforts, learned, fortunate, devoted to his wife.
		public static bool BuddhGuruShani(this Grahas grahaList) => (BuddhShani(grahaList) && BuddhGuru(grahaList));

		//Conjunction between Mercury, Jupiter, Venus, and Saturn.
		//Very learned, of remarkable memory, truthful, amiable, sensuous.
		public static bool BuddhGuruShaniShukra(this Grahas grahaList) => (BuddhGuruShani(grahaList) && BuddhShukra(grahaList));

		//Conjunction between Mercury, Mars, and Jupiter.
		//Honored in his family, given to poetry, music and drama, attached to young women, husband of a fine woman, engaged in doing good to others.
		public static bool BuddhMangalGuru(this Grahas grahaList) => BuddhMangal(grahaList) && BuddhGuru(grahaList);

		//Conjunction between Mercury, Mars, and Saturn.
		//Living in fear, lean-bodied, disease in the eyes and mouth, witty, a menial worker, a vagrant.
		public static bool BuddhMangalShani(this Grahas grahaList) => BuddhMangal(grahaList) && BuddhShani(grahaList);

		//Conjunction between Mercury, Venus, and Jupiter.
		//Good in looks, bereft of enemies, truthful, of lasting fame.
		public static bool BuddhShukraGuru(this Grahas grahaList) => BuddhShukra(grahaList) && BuddhGuru(grahaList);

		//Conjunction between Mercury, Venus, and Mars.
		//Very talkative, fickle-minded, defective of a limb, slim of body, base-born, wicked, enthusiastic, rich.
		public static bool BuddhShukraMangal(this Grahas grahaList) => BuddhShukra(grahaList) && BuddhMangal(grahaList);

		//Conjunction between Mercury, Venus, and Saturn.
		//The person is untruthful, vicious, addicted to women not his own, a wanderer.
		public static bool BuddhShukraShani(this Grahas grahaList) => BuddhShukra(grahaList) && BuddhShani(grahaList);

		//Conjunction between Sun and Mercury.
		//The result is learned, sweet- tongued, clever, earns wealth by serving others, scholarly, good in looks and fickle.
		public static bool BudhaAditya(this Grahas grahaList)
		{
			var mercury = grahaList.Find(Body.Mercury);
			return mercury.IsConjuctWith(Body.Sun);
		}

		//Sun and Mercury are Conjunct in Lagna.
		//Learned, talkative, strong, wise, long-lived.
		public static bool BudhaAditya1 (this Grahas grahaList)
		{
			var mercury = grahaList.Find(Body.Mercury);
			if (mercury.Bhava != Bhava.LagnaBhava)
			{
				return (false);
			}
			return grahaList.BudhaAditya();
		}

		//Sun and Mercury are Conjunct in 4th House.
		//The native is very wealthy(even like kubera), of heavy build and defective nose.
		public static bool BudhaAditya4(this Grahas grahaList)
		{
			var mercury = grahaList.Find(Body.Mercury);
			if (mercury.Bhava != Bhava.SukhaBhava)
			{
				return (false);
			}
			return grahaList.BudhaAditya();
		}

		//Sun and Mercury are Conjunct in 7th House.
		//The native is Cruel-hearted, a killer, without greed, bereft of comforts from his wife.
		public static bool BudhaAditya7(this Grahas grahaList)
		{
			var mercury = grahaList.Find(Body.Mercury);
			if (mercury.Bhava != Bhava.JayaBhava)
			{
				return (false);
			}
			return grahaList.BudhaAditya();

		}

		//Sun and Mercury are Conjunct in 9th House.
		//Clever, devoid of comforts, many foes and ailments.
		public static bool BudhaAditya9(this Grahas grahaList)
		{
			var mercury = grahaList.Find(Body.Mercury);
			if (mercury.Bhava != Bhava.DhanaBhava)
			{
				return (false);
			}
			return grahaList.BudhaAditya();

		}
	}
}
