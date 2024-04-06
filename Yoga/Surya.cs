using Mhora.Definitions;
using Mhora.Elements;

namespace Mhora.Yoga
{
	public static class Surya
	{
		//Famous worldwide, blessed with vehicles and material comforts.
		//These results occur if the Sun or Mercury are not in their debilitation.
		public static bool SuryaBuddh(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.Bhava != Bhava.KarmaBhava)
			{
				return (false);
			}
			return sun.IsConjuctWith(Body.Mercury);
		}

		//The result is Valorous, haughty, skilled in work on stones, machines and tools,
		//very wealthy, harsh, cruel and easily submitting to women.
		public static bool SuryaChandra(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			return sun.IsConjuctWith(Body.Moon);
		}

		//The result is Burdened with the miseries of his parents, bereft of name and fame, poor.
		public static bool SuryaChandra1(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.Bhava != Bhava.LagnaBhava)
			{
				return (false);
			}
			return sun.IsConjuctWith(Body.Moon);
		}

		//The native is Stupid, bereft of near and dear ones, without comfort or riches.
		public static bool SuryaChandra4(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.Bhava != Bhava.SukhaBhava)
			{
				return (false);
			}
			return sun.IsConjuctWith(Body.Moon);
		}

		//Results is constantly troubled by women, bereft of friends.
		public static bool SuryaChandra7(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.Bhava != Bhava.JayaBhava)
			{
				return (false);
			}
			return sun.IsConjuctWith(Body.Moon);
		}

		//Results is wealthy, good in looks, fond of quarrels, short-lived and suffers from eye disease.
		public static bool SuryaChandra9(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.Bhava != Bhava.DharmaBhava)
			{
				return (false);
			}
			return sun.IsConjuctWith(Body.Moon);
		}

		//Good in looks, royal in bearing, strong, cruel hearted, tormentor of foes.
		public static bool SuryaChandra10(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.Bhava != Bhava.KarmaBhava)
			{
				return (false);
			}
			return sun.IsConjuctWith(Body.Moon);
		}

		//A teacher or preceptor, servile, engaged in religious pursuits,
		//honored by the ruler, blessed with friends and wealth, widely renowned.
		public static bool SuryaGuru(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			return (sun.IsConjuctWith(Body.Jupiter));
		}

		//Virtuous, scholarly, wealthy, famous, engaged in physical pleasures, a minister.
		public static bool SuryaGuru1(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.Bhava != Bhava.LagnaBhava)
			{
				return (false);
			}
			return (sun.IsConjuctWith(Body.Jupiter));
		}

		//Versed in scriptures, good in looks, sweet tongued, engaged in secret pursuits
		public static bool SuryaGuru4(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.Bhava != Bhava.SukhaBhava)
			{
				return (false);
			}
			return (sun.IsConjuctWith(Body.Jupiter));
		}

		//Dominated by women because of excessive sexual urge, hostile to father, good in looks, and own precious stones etc.
		public static bool SuryaGuru7(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.Bhava != Bhava.JayaBhava)
			{
				return (false);
			}
			return (sun.IsConjuctWith(Body.Jupiter));
		}

		//Wealthy parents, wealthy himself, brave and long-lived.
		public static bool SuryaGuru9(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.Bhava != Bhava.DharmaBhava)
			{
				return (false);
			}
			return (sun.IsConjuctWith(Body.Jupiter));
		}

		//Obtains fame, comforts and status even if born in an ordinary family.
		public static bool SuryaGuru10(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.Bhava != Bhava.KarmaBhava)
			{
				return (false);
			}
			return (sun.IsConjuctWith(Body.Jupiter));
		}

		//Strong, energetic, illustrious, wicked, sinful, aggressive and cruel.
		public static bool SuryaMangal(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			return (sun.IsConjuctWith(Body.Mars));
		}

		//The result is of a bilious nature, aggressive, cruel, brave in combat, with injured body.
		public static bool SuryaMangal1(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.Bhava != Bhava.LagnaBhava)
			{
				return (false);
			}
			return (sun.IsConjuctWith(Body.Mars));
		}

		//Bereft of friends, wealth and home comforts, miserable, loyal to none.
		public static bool SuryaMangal4(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.Bhava != Bhava.SukhaBhava)
			{
				return (false);
			}
			return (sun.IsConjuctWith(Body.Mars));
		}

		//Suffers separation from wife, living in a foreign or distant land
		public static bool SuryaMangal7(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.Bhava != Bhava.JayaBhava)
			{
				return (false);
			}
			return (sun.IsConjuctWith(Body.Mars));
		}

		//The native is quarrelsome aggressive, clever, liked by the ruler
		public static bool SuryaMangal9(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.Bhava != Bhava.DharmaBhava)
			{
				return (false);
			}
			return (sun.IsConjuctWith(Body.Mars));
		}

		//Result is servant of king, ever anxious, earning a bad name in his undertakings.
		public static bool SuryaMangal10(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.Bhava != Bhava.KarmaBhava)
			{
				return (false);
			}
			return (sun.IsConjuctWith(Body.Mars));
		}

		//Skilled in metallurgy, engaged in religious pursuits, bereft of wife and children,
		//learned, dominated by enemies, follows traditions
		public static bool SuryaShani(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			return (sun.IsConjuctWith(Body.Saturn));
		}

		//Base earnings, blemished learning, a sinner.
		public static bool SuryaShani1(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.Bhava != Bhava.LagnaBhava)
			{
				return (false);
			}
			return (sun.IsConjuctWith(Body.Saturn));
		}

		//Very poor, troubled by his near and dear ones, a sinner.
		public static bool SuryaShani4(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.Bhava != Bhava.SukhaBhava)
			{
				return (false);
			}
			return (sun.IsConjuctWith(Body.Saturn));
		}

		//Slow-witted, lazy, devoid of wealth from wife, subject to misfortunes, a fool.
		public static bool SuryaShani7(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.Bhava != Bhava.JayaBhava)
			{
				return (false);
			}
			return (sun.IsConjuctWith(Body.Saturn));
		}

		//Wealthy, quarrelsome, adverse of parents, short-lived, with eye disease.
		public static bool SuryaShani9(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.Bhava != Bhava.DharmaBhava)
			{
				return (false);
			}
			return (sun.IsConjuctWith(Body.Saturn));
		}

		//Servile, a wanderer, loses his earnings to thieves and dacoits.
		public static bool SuryaShani10(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.Bhava != Bhava.KarmaBhava)
			{
				return (false);
			}
			return (sun.IsConjuctWith(Body.Saturn));
		}

		//Intelligent, skilled in wielding weapons, given to easy morals, earn through women
		//not his own, of poor vision in old age, gains from such pursuits as dance, drama, acting and music.
		public static bool SuryaShukra(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			return (sun.IsConjuctWith(Body.Venus));
		}

		//The result is quarrelsome, bereft of virtue and wealth, harsh, wicked.
		public static bool SuryaShukra1(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.Bhava != Bhava.LagnaBhava)
			{
				return (false);
			}
			return (sun.IsConjuctWith(Body.Venus));
		}

		//Servile, miserable, poor.
		public static bool SuryaShukra4(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.Bhava != Bhava.SukhaBhava)
			{
				return (false);
			}
			return (sun.IsConjuctWith(Body.Venus));
		}

		//Tormented by women, poor, a wanderer
		public static bool SuryaShukra7(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.Bhava != Bhava.JayaBhava)
			{
				return (false);
			}
			return (sun.IsConjuctWith(Body.Venus));
		}

		//Ailing, a lover, blessed with fine clothes, perfumes and ornaments
		public static bool SuryaShukra9(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.Bhava != Bhava.DharmaBhava)
			{
				return (false);
			}
			return (sun.IsConjuctWith(Body.Venus));
		}

		//Clever in dealings, minister to a king, learned in scriptures, famous and wealthy.
		public static bool SuryaShukra10(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.Bhava != Bhava.KarmaBhava)
			{
				return (false);
			}
			return (sun.IsConjuctWith(Body.Venus));
		}

		//Sharp-witted, engaged in the study of scriptures, a writer, a sculptor, valorous, suffering from eye disease.
		public static bool SuryaBuddhGuru(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Mercury) == false)
			{
				return (false);
			}

			return sun.IsConjuctWith(Body.Jupiter);
		}

		//Quarrelsome, conceited, lacking enthusiasm, behaving like a enunch.
		public static bool SuryaBuddhGuruShani(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Saturn) == false)
			{
				return (false);
			}

			return grahaList.SuryaBuddhGuru();
		}

		//Famous, wealthy, a leader, fulfills his desired objectives.
		public static bool SuryaBuddhGuruShukra(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Venus) == false)
			{
				return (false);
			}

			return grahaList.SuryaBuddhGuru();
		}

		//Versed in sacred scriptures, virtuous, devoted to preceptors, kindhearted.
		public static bool SuryaBuddhGuruShukraShani(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Saturn) == false)
			{
				return (false);
			}

			return grahaList.SuryaBuddhGuruShukra();
		}

		//Famous, valorous, cruel, a wrestler, shameless, wealth and progeny.
		public static bool SuryaBuddhMangal(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Mercury) == false)
			{
				return (false);
			}

			return sun.IsConjuctWith(Body.Mars);
		}

		//Rejected by friends and relatives, wicked, jealous, behaving like eunuchs, suffers humiliation.
		public static bool SuryaBuddhShani(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Mercury) == false)
			{
				return (false);
			}

			return sun.IsConjuctWith(Body.Saturn);
		}

		//Very talkative, a wanderer, of a slender constitution, learned, humiliated by parents and preceptors,
		//suffers torment because of his wife.
		public static bool SuryaBuddhShukra(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Mercury) == false)
			{
				return (false);
			}

			return sun.IsConjuctWith(Body.Venus);

		}

		//Pious, beautiful, learned, comfortable, truthful, good speaker, helpful to friends.
		public static bool SuryaBuddhShukraShani(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Saturn) == false)
			{
				return (false);
			}

			return (grahaList.SuryaBuddhShukra());
		}

		//Confident of a king, illustrious, versed in Shastras or scriptures, very learned, blessed with wealth and beauty,
		//sweet-tempered, fond of poetry and Puranic tales.
		public static bool SuryaChandraBuddh(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Moon) == false)
			{
				return (false);
			}

			return sun.IsConjuctWith(Body.Mercury);
		}

		//Healthy, wealthy, strong, virtuous, good in looks, with lovely eyes, a sculptor.
		public static bool SuryaChandraBuddhGuru(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Jupiter) == false)
			{
				return (false);
			}

			return grahaList.SuryaChandraBuddh();
		}

		//Sustained or fed by others(dependent), cruel, cowardly, a cheat.
		public static bool SuryaChandraBuddhGuruShani(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Saturn) == false)
			{
				return (false);
			}

			return grahaList.SuryaChandraBuddhGuru();
		}

		//Very wealthy, a minister, a King’s counselor, having authority to punish( a judge), renowned, powerful.
		public static bool SuryaChandraBuddhGuruShukra(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Venus) == false)
			{
				return (false);
			}

			return grahaList.SuryaChandraBuddhGuru();
		}

		//Bereft of wife and wealth, suffers mental anguish, kindhearted, a King’s counselor, handsome.
		public static bool SuryaChandraBuddhGuruShukraShani(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Saturn) == false)
			{
				return (false);
			}

			return grahaList.SuryaChandraBuddhGuruShukra();
		}

		//Living on alms, a pauper, ungrateful, with eye disease.
		public static bool SuryaChandraBuddhShani(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Saturn) == false)
			{
				return (false);
			}

			return grahaList.SuryaChandraBuddh();
		}

		//Eloquent speaker, handsome, dwarf, liked by the ruler, of defective vision.
		public static bool SuryaChandraBuddhShukra(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Venus) == false)
			{
				return (false);
			}

			return grahaList.SuryaChandraBuddh();
		}

		//Sickly, of tall stature, bereft of wealth, friends and comforts, of a hairy body.
		public static bool SuryaChandraBuddhShukraShani(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Saturn) == false)
			{
				return (false);
			}

			return grahaList.SuryaChandraBuddhShukra();
		}

		//Highly virtuous, a scholar, a minister, of stable wisdom, easily angered, living in a foreign land,
		//capable of spreading his influence around, deceitful.
		public static bool SuryaChandraGuru(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Moon) == false)
			{
				return (false);
			}

			return sun.IsConjuctWith(Body.Jupiter);
		}

		//Honorable, blessed with wealth and progeny, weak-bodied, of a balanced outlook, liked by worthy women, easily angered.
		public static bool SuryaChandraGuruShani(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Saturn) == false)
			{
				return (false);
			}

			return grahaList.SuryaChandraGuru();
		}

		//Honored by the ruler, owns watery and forest Region, enjoying several comforts.
		public static bool SuryaChandraGuruShukra(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Venus) == false)
			{
				return (false);
			}

			return grahaList.SuryaChandraGuru();
		}

		//Learned, fearless, fond of his wife, eloquent, clever in juggling, fickle- minded.
		public static bool SuryaChandraGuruShukraShani(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Saturn) == false)
			{
				return (false);
			}

			return grahaList.SuryaChandraGuruShukra();
		}

		//Valorous, merciless, very capable, wealthy, a sculptor, versed in mantras and yantras,
		//eliminator of his enemies, and suffers from the diseases of the blood.
		public static bool SuryaChandraMangal(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Moon) == false)
			{
				return (false);
			}

			return sun.IsConjuctWith(Body.Mars);
		}

		//Sickly, a writer, skilled in creating illusions, clever, eloquent, thievish.
		public static bool SuryaChandraMangalBuddh(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Mercury) == false)
			{
				return (false);
			}

			return grahaList.SuryaChandraMangal ();
		}

		//Bereft of comforts, wife and wealth, suffers incarceration, short-lived.
		public static bool SuryaChandraMangalBuddhGuru(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Jupiter) == false)
			{
				return (false);
			}

			return grahaList.SuryaChandraMangalBuddh();
		}

		//Given to charities, pious, fickle- minded, living in deserted places, adulterous, helpful to others.
		public static bool SuryaChandraMangalBuddhGuruShani(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Saturn) == false)
			{
				return (false);
			}

			return grahaList.SuryaChandraMangalBuddhGuru();
		}


		//Blessed with learning, wife, wealth and virtue, goes on pilgrimages, famous, dwells in hills and forests, lean, talkative.
		public static bool SuryaChandraMangalBuddhGuruShukra(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Venus) == false)
			{
				return (false);
			}

			return grahaList.SuryaChandraMangalBuddhGuru();
		}

		//Illustrious like the Sun, revered by the ruler, devoted to Lord Shiva, wealthy and of a charitable disposition.
		public static bool SuryaChandraMangalBuddhGuruShukraShani(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Saturn) == false)
			{
				return (false);
			}

			return grahaList.SuryaChandraMangalBuddhGuruShukra();
		}

		//Bereft of comforts, wife and wealth, suffers incarceration, short-lived.
		public static bool SuryaChandraMangalBuddhShani(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Saturn) == false)
			{
				return (false);
			}

			return grahaList.SuryaChandraMangalBuddh();
		}

		//Working for others, bereft of friends and relatives, untruthful, befriends eunuchs.
		public static bool SuryaChandraMangalBuddhShukra(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Venus) == false)
			{
				return (false);
			}

			return grahaList.SuryaChandraMangalBuddh();
		}

		//Suspicious, famous, destroyer of foes, fond of quarrels, adulterous, displaced from place of residence.
		public static bool SuryaChandraMangalBuddhShukraShani(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Saturn) == false)
			{
				return (false);
			}

			return grahaList.SuryaChandraMangalBuddhShukra();
		}

		//Famous, intelligent, wealthy, talented, devoted to the ruler, bereft of sorrow and sickness.
		public static bool SuryaChandraMangalGuru(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Jupiter) == false)
			{
				return (false);
			}

			return grahaList.SuryaChandraMangal();
		}

		//Good in combat, efficient, acquires the wealth of others, wicked, separated from his ladylove.
		public static bool SuryaChandraMangalGuruShani(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Saturn) == false)
			{
				return (false);
			}

			return grahaList.SuryaChandraMangalGuru();
		}

		//Rejected by parents, very miserable, a terrorist or an assassin, sightless.
		public static bool SuryaChandraMangalGuruShukra(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Venus) == false)
			{
				return (false);
			}

			return grahaList.SuryaChandraMangalGuru();
		}


		//Wicked, engaged in serving others, easily angered, suffers from tuberculosis and other chest diseases,
		//bereft of comforts but contented, devoted to his wife.
		public static bool SuryaChandraMangalGuruShukraShani(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Saturn) == false)
			{
				return (false);
			}

			return grahaList.SuryaChandraMangalGuruShukra();
		}

		//Stupid, dwarf, poor, a wanderer, having restless eyes and an unfaithful wife, living on alms.
		public static bool SuryaChandraMangalShani(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Saturn) == false)
			{
				return (false);
			}

			return grahaList.SuryaChandraMangal();
		}

		//Learned, comfortable, renowned, blessed with wife, progeny, wealth and virtue.
		public static bool SuryaChandraMangalShukra(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Venus) == false)
			{
				return (false);
			}

			return grahaList.SuryaChandraMangal();
		}

		//Deprived, poor, addicted to wives of others.
		public static bool SuryaChandraMangalShukraShani(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Saturn) == false)
			{
				return (false);
			}

			return grahaList.SuryaChandraMangalShukra();
		}

		//Servile, bereft of riches and grace, wickedly inclined, deceitful,
		//engaged in work pertaining to metals, undertakes futile labor.
		public static bool SuryaChandraShani(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Moon) == false)
			{
				return (false);
			}

			return sun.IsConjuctWith(Body.Saturn);
		}


		//Disinclined towards virtue, keen to acquire wealth not his own, addicted to
		//other people’s wives, good in looks, scholarly, rich, in dread of his enemies and suffers from dental disease.
		public static bool SuryaChandraShukra(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Moon) == false)
			{
				return (false);
			}

			return sun.IsConjuctWith(Body.Venus);
		}

		//Clever in talking, favored by the ruler, fearless, of pure thoughts, blessed with wife,
		//friends and progeny, of asymmetrical body, wastes money on women.
		public static bool SuryaGuruShani(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Jupiter) == false)
			{
				return (false);
			}

			return sun.IsConjuctWith(Body.Saturn);
		}

		//Covetous, a leader, daring, proficient in arts, favored by the ruler.
		public static bool SuryaGuruShukraShani(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Venus) == false)
			{
				return (false);
			}

			return grahaList.SuryaGuruShani ();
		}

		//Valorous, vagrant, adulterous, wealthy, a leader, but afflicted with misfortunes and eye disease.
		public static bool SuryaMangalBuddhGuru(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Mars) == false)
			{
				return (false);
			}

			return grahaList.SuryaBuddhGuru();

		}

		//Ever mentally perturbed, dressed in dirty and tattered clothes, lives on begged food.
		public static bool SuryaMangalBuddhGuruShani(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Saturn) == false)
			{
				return (false);
			}

			return grahaList.SuryaMangalBuddhGuru();

		}

		//Famous, commander of an army, bereft of sorrow, fortunate, handsome, liked by the king, pursues women not his own.
		public static bool SuryaMangalBuddhGuruShukra(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Venus) == false)
			{
				return (false);
			}

			return grahaList.SuryaMangalBuddhGuru();

		}

		//Wandering in hills and forests, goes on pilgrimage, bereft of wife, wealth and progeny.
		public static bool SuryaMangalBuddhGuruShukraShani(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Saturn) == false)
			{
				return (false);
			}

			return grahaList.SuryaMangalBuddhGuruShukra();

		}

		//Learned, a fighter, a commander, a counselor to the ruler, indulges in base acts.
		public static bool SuryaMangalBuddhShani(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Mars) == false)
			{
				return (false);
			}

			return grahaList.SuryaBuddhShani();

		}

		//Adulterous, shameless, wicked, of odd looks and dress.
		public static bool SuryaMangalBuddhShukra(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Mars) == false)
			{
				return (false);
			}

			return grahaList.SuryaBuddhShukra();

		}

		//Wandering in hills and forests, goes on pilgrimage, bereft of wife, wealth and progeny.
		public static bool SuryaMangalBuddhShukraShani(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Saturn) == false)
			{
				return (false);
			}

			return grahaList.SuryaMangalBuddhShukra();

		}

		//Very wealthy, eloquent speaker, a minister or a commander, generous, truthful, aggressive and dominating,
		//fond of entertainment, inclined to render justice.
		public static bool SuryaMangalGuru(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Mars) == false)
			{
				return (false);
			}

			return sun.IsConjuctWith(Body.Jupiter);
		}

		//Honored fulfils his undertaking, blessed with friends, relatives and royal favors, mentally unbalanced.
		public static bool SuryaMangalGuruShani(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Saturn) == false)
			{
				return (false);
			}

			return grahaList.SuryaMangalGuru();
		}

		//Renowned, wealthy, fortunate, held in esteem by the ruler.
		public static bool SuryaMangalGuruShukra(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Venus) == false)
			{
				return (false);
			}

			return grahaList.SuryaMangalGuru();
		}

		//Learned, thoughtful, adept in metal work and in dealing with chemicals.
		public static bool SuryaMangalGuruShukraShani(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Saturn) == false)
			{
				return (false);
			}

			return grahaList.SuryaMangalGuruShukra();
		}

		//Ignorant, poor, bereft of father and friends, ailing, defective of a limb, quarrelsome, with a hairy body.
		public static bool SuryaMangalShani(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Mars) == false)
			{
				return (false);
			}

			return sun.IsConjuctWith(Body.Saturn);

		}

		//Lacking in intelligence, looks and virtues, suffers mental anguish, dominated by all.
		public static bool SuryaMangalShukraShani(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Venus) == false)
			{
				return (false);
			}

			return grahaList.SuryaMangalShani();

		}

		//Learned, poor, in the refuge of a king, a servant, talkative, cruel, with poor vision.
		public static bool SuryaShukraGuru(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Venus) == false)
			{
				return (false);
			}

			return sun.IsConjuctWith(Body.Jupiter);
		}

		//Fortunate, very wise, wealthy, amiable, harsh, good in looks, a man of few words,
		//suffering from eye disease, given to pleasure of the flash.
		public static bool SuryaShukraMangal(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Venus) == false)
			{
				return (false);
			}

			return sun.IsConjuctWith(Body.Mars);
		}

		//Wicked, haughty, in dread of his enemies, bereft of learning, grace and fame, unskilled,
		//inclined towards immoral pursuits, suffers from skin disease.
		public static bool SuryaShukraShani(this Grahas grahaList)
		{
			var sun = grahaList.Find(Body.Sun);
			if (sun.IsConjuctWith(Body.Venus) == false)
			{
				return (false);
			}

			return sun.IsConjuctWith(Body.Saturn);
		}
	}
}
