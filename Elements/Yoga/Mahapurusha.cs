using Mhora.Definitions;

namespace Mhora.Elements.Yoga
{
	public static class Mahapurusha
	{
		//When Mercury, exalted (in Kanya) or in its own sign (Mithuna or Kanya), is located in a kendra.
		//One born in Bhadra yoga is physically strong and healthy with long arms and fine dense beard.
		//Commanding stature, broad chest and shoulders, beautiful forehead & dark curly hair.
		public static bool MahapurushaBhadraBuddh(this Grahas grahaList)
		{
			var mercury = grahaList.Find(Body.Mercury);
			if ((mercury.IsInOwnHouse == false) && (mercury.IsExalted == false))
			{
				return (false);
			}

			if (mercury.Bhava.IsKendra())
			{
				return (true);
			}

			return (false);
		}

		//When Jupiter, exalted ( in Karka) or in its own sign (Dhanu or Meena), is located in a kendra.
		//One born in Hams yoga has a fair complexion, prominent cheeks, beautiful face and a golden luster in his skin.
		//His nails and palms are red, his feet are lovely and his eyes have the pallor of honey. His voice is sweet like that of a swan.
		public static bool MahapurushaHamsa (this Grahas grahaList)
		{
			var jupiter = grahaList.Find(Body.Jupiter);
			if ((jupiter.IsInOwnHouse == false) && (jupiter.IsExalted == false))
			{
				return (false);
			}

			if (jupiter.Bhava.IsKendra())
			{
				return (true);
			}

			return (false);
		}

		//When Venus, exalted (in Meena), or in its own sign (Vrisha or Tula), is located in kendra. Possible for all lagnas.
		//Graceful appearance, fine and beautiful lips, slim waist, handsome body, deep voice, sparkling teeth and bright complexion like full moon.
		public static bool MahapurushaMalavya (this Grahas grahaList)
		{
			var venus = grahaList.Find(Body.Venus);
			if ((venus.IsInOwnHouse == false) && (venus.IsExalted == false))
			{
				return (false);
			}

			if (venus.Bhava.IsKendra())
			{
				return (true);
			}

			return (false);
		}

		//This Yoga forms when Mars, exalted ( in Makara) or in its own sign (Mesha or Vrischika), is located in a kendra from the lagna.
		//One born in Ruchaka yoga is good looking and handsome. Prominent face, beautiful eyebrows, dark hair and clear complexion.
		//He is Commander of an army or leader of a gang of thieves.
		public static bool MahapurushaRuchaka (this Grahas grahaList)
		{
			var mars = grahaList.Find(Body.Mars);
			if ((mars.IsInOwnHouse == false) && (mars.IsExalted == false))
			{
				return (false);
			}

			if (mars.Bhava.IsKendra())
			{
				return (true);
			}

			return (false);
		}

		//When Saturn, exalted (in Tula) or in its own sign(Makara or Kumbha), is located in kendra. Can exist only for Chara and lagnas.
		//Brave and cruel, medium height, slim waist and high-set teeth. He has beautiful leg and has a fast but regular gait.
		public static bool MahapurushaSasha (this Grahas grahaList)
		{
			var saturn = grahaList.Find(Body.Mars);
			if ((saturn.IsInOwnHouse == false) && (saturn.IsExalted == false))
			{
				return (false);
			}

			if (saturn.Bhava.IsKendra())
			{
				return (true);
			}

			return (false);
		}
	}
}
