using System.Collections.Generic;
using Mhora.Definitions;
using Mhora.Elements;
using Mhora.Elements.Extensions;
using Mhora.Util;

namespace Mhora.Yoga
{
	public static class Raja
	{
		//Exchange between lords of the 4th and 10th houses, and they associates with the 5th or the 9th lords.
		//This Yoga confers the status of the individual in terms of success recognition and status.
		public static bool Raja1(this Grahas grahaList)
		{
			var lord4  = grahaList.Rashis.Find(Bhava.SukhaBhava).Lord;
			var lord10 = grahaList.Rashis.Find(Bhava.KarmaBhava).Lord;

			if (lord4.Exchange != lord10)
			{
				return (false);
			}
			var lord5 = grahaList.Rashis.Find(Bhava.PutraBhava).Lord;
			var lord9 = grahaList.Rashis.Find(Bhava.DharmaBhava).Lord;

			if ((lord4.IsAssociatedWith(lord5) == false) && 
			    (lord4.IsAssociatedWith(lord9) == false))
			{
				return (false);
			}

			if ((lord10.IsAssociatedWith(lord5) == false) && 
			    (lord10.IsAssociatedWith(lord9) == false))
			{
				return (false);
			}

			return (true);
		}

		//The 9th lord & the Atmakarka located in Lagna, 5th house or the 7th house & aspected by benefices.
		//Brings fourth king.
		public static bool Raja2(this Grahas grahaList)
		{
			var atmakaraka = grahaList.Find(Karaka8.Atma);
			var lord9      = grahaList.Rashis.Find(Bhava.DharmaBhava).Lord;

			switch (atmakaraka.Bhava)
			{
				case Bhava.LagnaBhava:
				case Bhava.PutraBhava:
				case Bhava.JayaBhava:
					break;
				default:
					return (false);
			}

			switch (lord9.Bhava)
			{
				case Bhava.LagnaBhava:
				case Bhava.PutraBhava:
				case Bhava.JayaBhava:
					break;
				default:
					return (false);
			}

			if (atmakaraka.IsAspectedBy(Nature.Benefic, true) == false)
			{
				return (false);
			}

			if (lord9.IsAspectedBy(Nature.Benefic, true) == false)
			{
				return (false);
			}

			return (false);
		}

		//Debilitated planets occupying 3rd, 6th, 8th or 11th house in the presence of strong Lagna lord,
		//which is aspecting the Lagna.
		//This Yoga confers the status of the individual in terms of success recognition and status.
		public static bool Raja3(this Grahas grahaList)
		{
			var lord1 = grahaList.Rashis.Find(Bhava.LagnaBhava).Lord;
			if (lord1.IsAspecting(Body.Lagna) == false)
			{
				return (false);
			}

			foreach (var graha in grahaList.Planets)
			{
				if (graha.IsDebilitated)
				{
					switch (graha.Bhava)
					{
						case Bhava.SahajaBhava:
						case Bhava.ShatruBhava:
						case Bhava.MrtyuBhava:
						case Bhava.LabhaBhava: 
							break;
						default:
							return (false);
					}
				}
			}

			return (true);
		}

		//The lord of the 9th house is associtating with the lord of the 10th house.
		//This Yoga confers the status of the individual in terms of success recognition and status.
		public static bool Raja4(this Grahas grahaList)
		{
			var lord9  = grahaList.Rashis.Find(Bhava.DharmaBhava).Lord;
			var lord10 = grahaList.Rashis.Find(Bhava.KarmaBhava).Lord;
			if (lord9.IsAssociatedWith(lord10))
			{
				return (true);
			}

			return (false);
		}

		//A conjunction or mutual aspect between the lord of the 5th house and the lord of the 9th house.
		//The 9th Lord is a minister, the 5th is a chief minister. By kingship is meant a high govt. status in the modern context.
		public static bool Raja5(this Grahas grahaList)
		{
			var lord5 = grahaList.Rashis.Find(Bhava.PutraBhava).Lord;
			var lord9  = grahaList.Rashis.Find(Bhava.DharmaBhava).Lord;

			if (lord5.IsConjuctWith(lord9))
			{
				return (true);
			}

			return lord5.HasMutualAspectWith(lord9);
		}

		//When placement of Rahu/Ketu in kendras in association with trikona lords.
		//This Yoga confers the status of the individual in terms of success recognition and status.
		public static bool Raja6(this Grahas grahaList)
		{
			var rahu = grahaList.Find(Body.Rahu);
			var ketu = grahaList.Find(Body.Ketu);
			if (rahu.Bhava.IsKendra() == false)
			{
				return (false);
			}

			bool yoga = false;
			foreach (var graha in rahu.Association)
			{
				if (graha.Bhava.IsTrikona())
				{
					yoga = true;
					break;
				}
			}

			if (yoga == false)
			{
				return (false);
			}
			
			yoga = false;
			foreach (var graha in ketu.Association)
			{
				if (graha.Bhava.IsTrikona())
				{
					yoga = true;
					break;
				}
			}

			return (yoga);
		}

		//when benefices associate with houses 2, 4, and 5 as reckoned from Lagna or the Atmakarka,
		//and melefics occupy the houses 3 and 6 from them
		//The native attains kingship
		public static bool Raja7(this Grahas grahaList)
		{
			byte yoga       = 0x00;
			byte yoga2      = 0x00;
			var  atmakaraka = grahaList.Find(Karaka8.Atma);

			foreach (var graha in grahaList.Planets)
			{
				if (graha.IsNaturalBenefic)
				{
					foreach (var ass in graha.Association)
					{
						switch (ass.Bhava)
						{
							case Bhava.DhanaBhava:
							{
								yoga |= 0x01;
							}
							break;

							case Bhava.SukhaBhava:
							{
								yoga |= 0x02;
							}
							break;

							case Bhava.PutraBhava:
							{
								yoga |= 0x04;
							}
							break;
						}

						switch (ass.HouseFrom(atmakaraka))
						{
							case Bhava.DhanaBhava:
							{
								yoga2 |= 0x01;
							}
							break;

							case Bhava.SukhaBhava:
							{
								yoga2 |= 0x02;
							}
							break;

							case Bhava.PutraBhava:
							{
								yoga2 |= 0x04;
							}
							break;
						}
					}

					if ((yoga != 0x07) && (yoga2 != 0x07))
					{
						return (false);
					}
				}
				else if (graha.IsNaturalMalefic)
				{
					switch (graha.Bhava)
					{
						case Bhava.DhanaBhava:
						{
							yoga |= 0x10;
						}
						break;

						case Bhava.ShatruBhava:
						{
							yoga |= 0x20;
						}
						break;
					}

					switch (graha.HouseFrom(atmakaraka))
					{
						case Bhava.DhanaBhava:
						{
							yoga2 |= 0x10;
						}
						break;

						case Bhava.ShatruBhava:
						{
							yoga2 |= 0X20;
						}
						break;
					}
				}
			}

			if (yoga == 0X27)
			{
				return (true);
			}

			if (yoga2 == 0x27)
			{
				return (true);
			}
			return (true);
		}

		//Jupiter is in conjunction with Venus in the 9th house or associated with the 5th lord.
		//This Yoga confers the status of the individual in terms of success recognition and status.
		public static bool Raja8(this Grahas grahaList)
		{
			var jupiter = grahaList.Find(Body.Jupiter);

			if (jupiter.Bhava != Bhava.DharmaBhava)
			{
				return (false);
			}

			var lord9   = grahaList.Rashis.Find(Bhava.DharmaBhava).Lord;
			if (jupiter.IsConjuctWith(lord9))
			{
				return (true);
			}

			var lord5 = grahaList.Rashis.Find(Bhava.PutraBhava).Lord;
			if (jupiter.IsAssociatedWith(lord5))
			{
				return (true);
			}
			return (false);
		}

		//Exchange between lords of the 4th and 10th houses, and they are being aspected by the 5th or the 9th lords.
		//This Yoga confers the status of the individual in terms of success recognition and status.
		public static bool Raja9(this Grahas grahaList)
		{
			var lord4 = grahaList.Rashis.Find(Bhava.SukhaBhava).Lord;
			var lord10 = grahaList.Rashis.Find(Bhava.KarmaBhava).Lord;

			if (lord4.Exchange != lord10)
			{
				return (false);
			}
			var lord5 = grahaList.Rashis.Find(Bhava.PutraBhava).Lord;
			var lord9 = grahaList.Rashis.Find(Bhava.DharmaBhava).Lord;

			if (lord4.IsAspectedBy(lord5) || lord4.IsAspectedBy(lord9))
			{
				return (true);
			}

			return (false);
		}

		//All benefices in kendra and all malefics located in houses 3, 6, 11.
		//This Raja Yoga brings fourth king.
		public static bool Raja10(this Grahas grahaList)
		{
			foreach (var graha in grahaList.Planets)
			{
				if (graha.IsFunctionalBenefic)
				{
					if (graha.Bhava.IsKendra() == false)
					{
						return (false);
					}
				}

				if (graha.IsFunctionalMalefic)
				{
					switch (graha.Bhava)
					{
						case Bhava.SahajaBhava:
						case Bhava.ShatruBhava:
						case Bhava.LabhaBhava:
							break;
						default:
							return (false);
					}
				}
			}

			return (true);
		}

		//Lords of the 4th and the 10th are in conjunction with the lords of the 5th or the 9th house.
		//This Yoga confers the status of the individual in terms of success recognition and status.
		public static bool Raja11(this Grahas grahaList)
		{
			var lord4  = grahaList.Rashis.Find(Bhava.SukhaBhava).Lord;
			var lord10 = grahaList.Rashis.Find(Bhava.KarmaBhava).Lord;

			var lord5 = grahaList.Rashis.Find(Bhava.PutraBhava).Lord;
			var lord9 = grahaList.Rashis.Find(Bhava.DharmaBhava).Lord;

			if ((lord4.IsConjuctWith(lord5) == false) && (lord4.IsConjuctWith(lord9) == false))
			{
				return (false);
			}

			if ((lord10.IsConjuctWith(lord5) == false) && (lord10.IsConjuctWith(lord9) == false))
			{
				return (false);
			}

			return (true);
		}

		//Lord of the 5th house associated with the Lagna lord or the 9th lord, and is located in Lagna or 4th or 10th house.
		//Brings fourth king.
		public static bool Raja12(this Grahas grahaList)
		{
			var lord1 = grahaList.Rashis.Find(Bhava.LagnaBhava).Lord;
			var lord5 = grahaList.Rashis.Find(Bhava.PutraBhava).Lord;
			var lord9 = grahaList.Rashis.Find(Bhava.DharmaBhava).Lord;

			if ((lord5.IsAssociatedWith(lord1) == false) && (lord5.IsAssociatedWith(lord9) == false))
			{
				return (false);
			}

			switch (lord5.Bhava)
			{
				case Bhava.LagnaBhava:
				case Bhava.SukhaBhava:
				case Bhava.KarmaBhava:
					return true;
			}

			return (false);
		}

		//The Lagna lord is associtating with the lord of the 4th or 5th or 7th or 9th or 10th house.
		//This Yoga confers the status of the individual in terms of success recognition and status.
		public static bool Raja13(this Grahas grahaList)
		{
			var lord1 = grahaList.Rashis.Find(Bhava.LagnaBhava).Lord;
			foreach (var graha in lord1.Association)
			{
				if (graha.Owns(Bhava.ShatruBhava))
				{
					return (true);
				}
				if (graha.Owns(Bhava.PutraBhava))
				{
					return (true);
				}
				if (graha.Owns(Bhava.JayaBhava))
				{
					return (true);
				}
				if (graha.Owns(Bhava.DharmaBhava))
				{
					return (true);
				}
				if (graha.Owns(Bhava.KarmaBhava))
				{
					return (true);
				}
			}
			return (false);
		}

		//10th lord exalted or in its own house aspecting the Lagna at the same time.
		//This Yoga confers the status of the individual in terms of success recognition and status.
		public static bool Raja14(this Grahas grahaList)
		{
			var lord10 = grahaList.Rashis.Find(Bhava.KarmaBhava).Lord;
			if ((lord10.IsExalted == false) && (lord10.IsInOwnHouse == false))
			{
				return (false);
			}

			return lord10.IsAspecting(Body.Lagna);
		}

		//The lord of the 4th house is associtating with the lord of the 5th or 9th house.
		//This Yoga confers status of the individual in terms of success recognition and status.
		public static bool Raja15(this Grahas grahaList)
		{
			var lord4 = grahaList.Rashis.Find(Bhava.SukhaBhava).Lord;
			var lord5 = grahaList.Rashis.Find(Bhava.PutraBhava).Lord;
			var lord9 = grahaList.Rashis.Find(Bhava.DharmaBhava).Lord;

			if (lord4.IsAssociatedWith(lord5))
			{
				return (true);
			}

			if (lord4.IsAssociatedWith(lord9))
			{
				return (true);
			}

			return (false);
		}

		//when benefices associate with houses 2, 4, and 5 as reckoned from Lagna or the Atmakarka,
		//and melefics occupy the houses 3 and 6 from them
		//The native attains kingship.
		public static bool Raja16(this Grahas grahaList)
		{
			var atmakaraka = grahaList.Find(Karaka8.Atma).Bhava.Index();

			foreach (var graha in grahaList.Planets)
			{
				if (graha.IsNaturalBenefic)
				{
					bool yoga = true;
					if (graha.IsAssociatedWith(Bhava.DhanaBhava) == false)
					{
						yoga = false;
					}
					else if (graha.IsAssociatedWith(Bhava.SukhaBhava) == false)
					{
						yoga = false;
					}
					else if (graha.IsAssociatedWith(Bhava.PutraBhava) == false)
					{
						yoga = false;
					}

					if (yoga == false)
					{
						if (graha.IsAssociatedWith(Bhava.DhanaBhava.Add(atmakaraka)) == false)
						{
							return false;
						}
						if (graha.IsAssociatedWith(Bhava.SukhaBhava.Add(atmakaraka)) == false)
						{
							return false;
						}
						if (graha.IsAssociatedWith(Bhava.PutraBhava.Add(atmakaraka)) == false)
						{
							return false;
						}
					}

					yoga  = false;
					var  rashi = grahaList.Rashis.Find(graha.Bhava.Add(3));
					foreach (var gr in rashi.Grahas)
					{
						if (gr.IsNaturalBenefic)
						{
							return false;
						}

						if (gr.IsNaturalMalefic)
						{
							yoga = true;
						}
					}

					if (yoga)
					{
						yoga  = false;
						rashi = grahaList.Rashis.Find(graha.Bhava.Add(6));
						foreach (var gr in rashi.Grahas)
						{
							if (gr.IsNaturalBenefic)
							{
								return false;
							}

							if (gr.IsNaturalMalefic)
							{
								yoga = true;
							}
						}
					}

					if (yoga == false)
					{
						return (false);
					}
				}
			}

			return (true);
		}

		//When placement of Rahu/Ketu in trikonas in association with kendra lords.
		//This Yoga confers the status of the individual in terms of success recognition and status.
		public static bool Raja17(this Grahas grahaList)
		{
			var rahu = grahaList.Find(Body.Rahu);
			var ketu = grahaList.Find(Body.Ketu);
			if (rahu.Bhava.IsTrikona() == false)
			{
				return (false);
			}

			byte yoga = 0x00;

			foreach (var rashi in grahaList.Rashis)
			{
				if (rashi.Bhava.IsKendra())
				{
					if (rahu.IsAssociatedWith(rashi.Lord))
					{
						yoga |= 0x01;
					}
					if (ketu.IsAssociatedWith(rashi.Lord))
					{
						yoga |= 0x02;
					}
				}
			}

			return (yoga == 0x03);
		}

		//The lord of the 5th house is associtating with the lord of 7th or the 10th house.
		//This Yoga confers the status of the individual in terms of success recognition and status.
		public static bool Raja18(this Grahas grahaList)
		{
			var lord5  = grahaList.Rashis.Find(Bhava.PutraBhava).Lord;
			var lord7  = grahaList.Rashis.Find(Bhava.JayaBhava).Lord;
			var lord10 = grahaList.Rashis.Find(Bhava.KarmaBhava).Lord;

			if (lord5.IsAssociatedWith(lord7))
			{
				return (true);
			}
			if (lord5.IsAssociatedWith(lord10))
			{
				return (true);
			}
			return (false);
		}

		//Debilitated lords of the 6th, the 8th or the 12th house when aspecting the lagna produce Raja Yoga.
		//This Yoga confers the status of the individual in terms of success recognition and status.
		public static bool Raja19(this Grahas grahaList)
		{
			var lords = new List<Graha>()
			{
				grahaList.Rashis.Find(Bhava.ShatruBhava).Lord,
				grahaList.Rashis.Find(Bhava.MrtyuBhava).Lord,
				grahaList.Rashis.Find(Bhava.VyayaBhava).Lord
			};

			foreach (var lord in lords)
			{
				if (lord.IsDebilitated && lord.IsAspecting(Body.Lagna))
				{
					return (true);
				}
			}

			return (false);
		}

		//All benefices in Kendras and all malefics occupy the Trishadaya houses 3, 6 and 11.
		//This Raja Yoga brings fourth king.
		public static bool Raja20(this Grahas grahaList)
		{
			foreach (var graha in grahaList.Planets)
			{
				if (graha.IsNaturalBenefic)
				{
					if (graha.Bhava.IsKendra() == false)
					{
						return (false);
					}
				}

				if (graha.IsNaturalMalefic)
				{
					if (graha.Bhava.IsTrishadaya() == false)
					{
						return (false);
					}
				}
			}

			return (true);
		}
		
		//The lord of the 7th house is associtating with the lord of the 9th house.
		//This Yoga confers the status of the individual in terms of success recognition and status.
		public static bool Raja21(this Grahas grahaList)
		{
			var lord7 = grahaList.Rashis.Find(Bhava.JayaBhava).Lord;
			var lord9 = grahaList.Rashis.Find(Bhava.DharmaBhava).Bhava;

			return (lord7.IsAssociatedWith(lord9));
		}

		//When placement of Rahu/Ketu in the signs of natural benefic planets.
		//This Yoga confers the status of the individual in terms of success recognition and status.
		public static bool Raja22(this Grahas grahaList)
		{
			var rahu = grahaList.Find(Body.Rahu);
			var ketu = grahaList.Find(Body.Ketu);

			if (rahu.Rashi.Lord.IsNaturalBenefic == false)
			{
				return (false);
			}
			if (ketu.Rashi.Lord.IsNaturalBenefic == false)
			{
				return (false);
			}

			return (true);
		}

		//Venus occupying the Lagna and aspected by or associated with the Moon or Jupiter.
		//This Yoga confers the status of the individual in terms of success recognition and status.
		public static bool Raja23(this Grahas grahaList)
		{
			var venus = grahaList.Find(Body.Venus);
			if (venus.Bhava != Bhava.LagnaBhava)
			{
				return (false);
			}

			if (venus.IsAspectedBy(Body.Moon) || venus.IsAspectedBy(Body.Jupiter))
			{
				return (true);
			}

			if (venus.IsAssociatedWith(Body.Moon) || venus.IsAssociatedWith(Body.Jupiter))
			{
				return (true);
			}

			return (false);
		}

		//The 6th lord is in the 6th, 8th or 12th house.
		//This is supposed to confer happiness health and fame. The person will conquer his/her enemies
		//and will hesitate in indulging in sinful deeds. Friends will be illustrious and with class.
		public static bool ViparitaHarshaRaja(this Grahas grahaList)
		{
			var lord6 = grahaList.Rashis.Find(Bhava.ShatruBhava).Lord;
			return lord6.Bhava.IsDushtana();
		}

		//The 8th lord is in the 6th, 8th or 12th house.
		//This confers learning longevity and prosperity. The person will be successful
		//in all ventures conqueror of foes and a great celebrity.
		public static bool ViparitaSaralaRaja(this Grahas grahaList)
		{
			var lord8 = grahaList.Rashis.Find(Bhava.MrtyuBhava).Lord;
			return lord8.Bhava.IsDushtana();
		}

		//The 12th lord is in the 6th, 8th or 12th house.
		//This makes the person virtuous and contented. The person will be equipped with good behavior
		//towards others will enjoy happiness will be independent following a respectable profession or
		//conduct and will be known for good qualities.
		public static bool ViparitaVimalaRaja(this Grahas grahaList)
		{
			var lord12 = grahaList.Rashis.Find(Bhava.VyayaBhava).Lord;
			return lord12.Bhava.IsDushtana();
		}
	}
}
