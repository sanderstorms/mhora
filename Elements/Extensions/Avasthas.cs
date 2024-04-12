using System;
using Mhora.Definitions;
using Mhora.Util;

namespace Mhora.Elements.Extensions
{
	public static class Avasthas
	{
		//Formula for calculating the Sayanadi Avastha of a planet
		// ‘C’ - Nakshatra number occupied by the planet (1 for aswini, 2 for bharani etc.
		// ‘P’ - Planet whose avastha we have to calculate (1 for Sun, 2 for Moon, 3 for Mars, 4 for
		// Mercury, 5 for Jupiter, 6 for Venus and 7 for Saturn).
		// ‘A’ - Index of the Navamsa occupied by the planet in rasi.
		// ‘M’ - Constellation occupied by Moon.
		// ‘G’ - Ghati running at birth.
		// ‘L’ - Rasi occupied by Lagna (1 for Aries, 2 for Taurus) and so on).
		// Compute (C x P x A) + M + G + L and then divide it by 12 and take the remainder.
		// Using the resulting number as the avastha index and referring to table given above, we
		// find out the avastha of the planet.
		// Example:
		// C – Planet Sun is in Hasta nakshatra. It is the 13th constellation
		// P – Sun is taken – Hence number = 1.
		// A – Sun is in 18° Vi 36’ which has the index number as 6.
		// M – Moon’s nakshatra is Purvabhadrapada which is the 25th constellation.
		// G – Native was born at 13:47. Sun rise on that date was 6:04. 7 hours and 43 minutes
		// have passed since sun rise which is equal to 463 minutes. A ghati contains 24
		// minnutes, 463/24 = 19.29 ghatis. So the 20th ghati was running at birth.
		// L – Lagna is Capricorn which is the 10th in the natural zodiac.

		public static SayanadiAvastha SayanadiAvastha(this Horoscope h, Body b)
		{
			var grahas    = h.FindGrahas(DivisionType.Rasi);

			var graha     = grahas[b];
			var moon      = grahas[Body.Moon];
			var lagna     = grahas [Body.Lagna];

			var nakshatra   = graha.Position.Longitude.ToNakshatra();
			var houseOffset = (double) graha.HouseOffset;
			var navamsha = (int) (houseOffset / (30.0 / 9)).Ceil();
			var ghati    = h.Vara.HoursAfterSunrise.Ghati;

			var avastha = nakshatra.Index() * (b.Index() + 1) * navamsha;
			avastha += moon.Position.Longitude.ToNakshatra().Index();
			avastha += (int) (ghati).Ceil();
			avastha += lagna.Rashi.ZodiacHouse.Index();

			return (SayanadiAvastha) (avastha % 12);
		}

		public static BaaladiAvastha BaaladiAwastha(this Horoscope h, Body b)
		{
			var grahas = h.FindGrahas(DivisionType.Rasi);

			var graha       = grahas[b];
			var houseOffset = (double) graha.HouseOffset;

			if (graha.Rashi.ZodiacHouse.IsOdd() == false)
			{
				houseOffset = 30 - houseOffset;
			}
			var avastha = (int) (houseOffset % 6).Floor() + 1;
			return ((BaaladiAvastha) houseOffset);
		}

		// Deepta : In the condition when planet is placed in exalted sign ( rashi) when the avastha is Deepta it means that the results wil be Bright.
		// Swastha : In the condition, when the planet is placed in its own sign (rashi)  when the avastha is Swastha it means Contentment.
		// Mudita: In the condition, when the planet is placed in its great friendly sign (rashi)  when the avastha is Mudita it means Delighted.
		// Shanta : In the condition, when the planet is placed in its friendly sign (rashi)  when the avastha is Shanta it means Peaceful.
		// Dina : In the condition, when the planet is placed in its neutral sign (rashi)  when the avastha is Din it means Depression.
		// Dukhita : In the condition, when the planet is placed in its enemy sign (rashi)  when the avastha is Dukhita it means Distressed.
		// Vikala : In the condition, when the planet is placed in it is in yuti with the malefic sign (rashi)  when the avastha is Vikala it means Crippled.
		// Khala : In the condition, when the planet is placed in its great enemy sign (rashi)  when the avastha is Khala it means Mischievous.
		// Kopa :In the condition, when the planet is being eclipsed by Sun  when the avastha is Kopa  it means anger, furious.
		public static DeeptadiAvasthas DeeptadiAvasthas(this Horoscope h, Body b)
		{
			var grahas = h.FindGrahas(DivisionType.Rasi);
			var graha  = grahas[b];

			if (graha.IsExalted)
			{
				return Definitions.DeeptadiAvasthas.Dipta;
			}

			if (graha.IsInOwnHouse)
			{
				return Definitions.DeeptadiAvasthas.Svastha;
			}

			if (graha.Rashi.Lord.IsNaturalMalefic)
			{
				return Definitions.DeeptadiAvasthas.Vikala;
			}

			if (graha.IsEcliped)
			{
				return Definitions.DeeptadiAvasthas.Kopa;
			}

			return graha.Rashi.Lord.Relationship(graha) switch
			       {
				       Relation.BestFriend  => Definitions.DeeptadiAvasthas.Mudita,
				       Relation.Friend      => Definitions.DeeptadiAvasthas.Santa,
				       Relation.Neutral     => Definitions.DeeptadiAvasthas.Dina,
				       Relation.Enemy       => Definitions.DeeptadiAvasthas.Dukhita,
				       Relation.BitterEnemy => Definitions.DeeptadiAvasthas.Khala,
				       _                    => Definitions.DeeptadiAvasthas.None
			       };
		}

		public static JagradadiAvasthas JagradadiAvasthas(this Horoscope h, Body b)
		{
			var grahas = h.FindGrahas(DivisionType.Rasi);
			var graha  = grahas[b];

			if (graha.IsExalted || graha.IsInOwnHouse)
			{
				return Definitions.JagradadiAvasthas.Jagrata;
			}

			if (graha.EnemySign == false)
			{
				return Definitions.JagradadiAvasthas.Swapna;
			}

			return Definitions.JagradadiAvasthas.Susupta;
		}

		public static LajjitadiAvasthas LajjitadiAvasthas(this Horoscope h, Body b)
		{
			var grahas = h.FindGrahas(DivisionType.Rasi);
			var graha  = grahas[b];

			if (graha.IsChayaGraha == false)
			{
				if (graha.Bhava == Bhava.PutraBhava)
				{
					if (graha.IsConjuctWith(Body.Sun))
					{
						return (Definitions.LajjitadiAvasthas.Lajjita);
					}
					if (graha.IsConjuctWith(Body.Saturn))
					{
						return (Definitions.LajjitadiAvasthas.Lajjita);
					}

					if (graha.IsConjuctWith(Body.Mars))
					{
						return (Definitions.LajjitadiAvasthas.Lajjita);
					}
				}
			}
			else if (graha.Conjunct.Count >= 2)
			{
				if (graha.IsConjuctWith(Body.Sun))
				{
					return (Definitions.LajjitadiAvasthas.Lajjita);
				}
				if (graha.IsConjuctWith(Body.Saturn))
				{
					return (Definitions.LajjitadiAvasthas.Lajjita);
				}

				if (graha.IsConjuctWith(Body.Mars))
				{
					return (Definitions.LajjitadiAvasthas.Lajjita);
				}
			}

			if (graha.IsExalted || graha.IsMoolTrikona)
			{
				return Definitions.LajjitadiAvasthas.Garvita;
			}

			if (graha.EnemySign)
			{
				return Definitions.LajjitadiAvasthas.Ksudhita;
			}

			foreach (var conj in graha.Conjunct)
			{
				if (graha.Relationship(conj) <= Relation.Enemy)
				{
					return Definitions.LajjitadiAvasthas.Ksudhita;
				}

				if (conj == Body.Saturn)
				{
					return Definitions.LajjitadiAvasthas.Ksudhita;
				}

				if (graha.Relationship(conj) >= Relation.Friend)
				{
					return (Definitions.LajjitadiAvasthas.Mudita);
				}
			}

			foreach (var aspect in graha.AspectFrom)
			{
				if (graha.Relationship(aspect) <= Relation.Enemy)
				{
					return Definitions.LajjitadiAvasthas.Ksudhita;
				}

				if (graha.Relationship(aspect) >= Relation.Friend)
				{
					return Definitions.LajjitadiAvasthas.Mudita;
				}
			}

			foreach (var ass in graha.Association)
			{
				if (graha.Relationship(ass) >= Relation.Friend)
				{
					return (Definitions.LajjitadiAvasthas.Mudita);
				}
			}

			if (graha.Rashi.ZodiacHouse.Element() == Element.Water)
			{
				if (graha.IsAspectedBy (Nature.Benefic, true) == false)
				{
					foreach (var aspect in graha.AspectTo)
					{
						if (aspect.IsNaturalMalefic)
						{
							return (Definitions.LajjitadiAvasthas.Krsita);
						}
					}
				}
			}

			if (graha.IsConjuctWith(Body.Sun))
			{
				foreach (var aspect in graha.AspectFrom)
				{
					if (aspect.IsNaturalMalefic)
					{
						if (graha.Relationship(aspect) <= Relation.Enemy)
						{
							return (Definitions.LajjitadiAvasthas.Kshobhita);
						}
					}
				}
			}

			return Definitions.LajjitadiAvasthas.None;
		}
	}
}
