using Mhora.Definitions;
using Mhora.Elements;

namespace Mhora.Yoga
{
	public static class Guru
	{
		//Conjunction between Jupiter and Saturn.
		//A minister, healthy, affable, blessed with friends and relatives.
		public static bool GuruShani(this Grahas grahaList)
		{
			var jupiter = grahaList.Find(Body.Jupiter);
			return (jupiter.IsConjuctWith(Body.Saturn));
		}

		//Jupiter and Saturn are conjunct in lagna
		//Loses money through hostility towards women, brave, indulges in base acts, foolish, covets his father’s money.
		public static bool GuruShani1(this Grahas grahaList)
		{
			var jupiter = grahaList.Find(Body.Jupiter);
			if (jupiter.Bhava == Bhava.LagnaBhava)
			{
				return (jupiter.IsConjuctWith(Body.Saturn));
			}

			return (false);
		}

		//Jupiter and Saturn are conjunct in 4th House.
		//Strong, famous, leader of a group or an army, an artist, a barber, a potter,
		//a cook, wealthy, suspicious of his wife, a wanderer, tends cattle.
		public static bool GuruShani4(this Grahas grahaList)
		{
			var jupiter = grahaList.Find(Body.Jupiter);
			if (jupiter.Bhava == Bhava.SukhaBhava)
			{
				return (jupiter.IsConjuctWith(Body.Saturn));
			}

			return (false);
		}

		//Jupiter and Saturn are conjunct in 7th House.
		//Learned, harsh, fond of wealth, wicked, a drunkard.
		public static bool GuruShani7(this Grahas grahaList)
		{
			var jupiter = grahaList.Find(Body.Jupiter);
			if (jupiter.Bhava == Bhava.JayaBhava)
			{
				return (jupiter.IsConjuctWith(Body.Saturn));
			}

			return (false);
		}

		//Jupiter and Saturn are conjunct in 9th House.
		//Enjoys wealth, sickly, bereft of near and dear ones.
		public static bool GuruShani9(this Grahas grahaList)
		{
			var jupiter = grahaList.Find(Body.Jupiter);
			if (jupiter.Bhava == Bhava.DhanaBhava)
			{
				return (jupiter.IsConjuctWith(Body.Saturn));
			}

			return (false);
		}

		//Jupiter and Saturn are conjunct in 10th House.
		//Equal to king, few sons, stable, blessed with wealth and vehicles.
		public static bool GuruShani10(this Grahas grahaList)
		{
			var jupiter = grahaList.Find(Body.Jupiter);
			if (jupiter.Bhava == Bhava.KarmaBhava)
			{
				return (jupiter.IsConjuctWith(Body.Saturn));
			}

			return (false);
		}

		//Conjunction between Venus and Jupiter.
		//Very learned, blessed with virtuous wife, wealthy, religious, earn his living through the use of his learning.
		public static bool GuruShukra(this Grahas grahaList)
		{
			var jupiter = grahaList.Find(Body.Jupiter);
			return (jupiter.IsConjuctWith(Body.Venus));
		}

		//Jupiter and Venus are conjunct in Lagna.
		public static bool GuruShukra1(this Grahas grahaList)
		{
			var jupiter = grahaList.Find(Body.Jupiter);
			if (jupiter.Bhava != Bhava.LagnaBhava)
			{
				return (false);
			}
			return (jupiter.IsConjuctWith(Body.Venus));

		}

		//Jupiter and Venus are conjunct in 4th house
		//Destroys his opponents, blessed with physical comforts, devoted to God and guru.
		public static bool GuruShukra4(this Grahas grahaList)
		{
			var jupiter = grahaList.Find(Body.Jupiter);
			if (jupiter.Bhava != Bhava.SukhaBhava)
			{
				return (false);
			}
			return (jupiter.IsConjuctWith(Body.Venus));

		}

		//Jupiter and Venus are conjunct in 7th house
		//Blessed with a beautiful wife, father of daughters, famous.
		public static bool GuruShukra7(this Grahas grahaList)
		{
			var jupiter = grahaList.Find(Body.Jupiter);
			if (jupiter.Bhava != Bhava.JayaBhava)
			{
				return (false);
			}
			return (jupiter.IsConjuctWith(Body.Venus));

		}

		//Jupiter and Venus are conjunct in 9th house
		//Sweet-tongued, a king, blessed with many comforts, long lived.
		public static bool GuruShukra9(this Grahas grahaList)
		{
			var jupiter = grahaList.Find(Body.Jupiter);
			if (jupiter.Bhava != Bhava.DharmaBhava)
			{
				return (false);
			}
			return (jupiter.IsConjuctWith(Body.Venus));

		}

		//Jupiter and Venus are conjunct in 10th house
		//Renowned, a king, blessed with many comforts, long-lived.
		public static bool GuruShukra10(this Grahas grahaList)
		{
			var jupiter = grahaList.Find(Body.Jupiter);
			if (jupiter.Bhava != Bhava.KarmaBhava)
			{
				return (false);
			}
			return (jupiter.IsConjuctWith(Body.Venus));

		}

	}
}
