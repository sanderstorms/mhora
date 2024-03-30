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
			return (12 - other.HousesTo(bhava) + 1);
		}

		public static Bhava Add(this Bhava bhava, int nr)
		{
			var result = (bhava.Index() + nr).NormalizeInc(1, 12);

			return (Bhava) result;
		}
	}
}
