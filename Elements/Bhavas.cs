using System;

namespace Mhora.Elements
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

			if (bhava.Trikona())
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

		public static bool Trikona (this Bhava bhava)
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
				case Bhava.JayaBhava: return true;
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

		public static bool IsKutumbaSthana(this Bhava bhava)
		{
			switch (bhava)
			{
				case Bhava.DhanaBhava: 
					return true;
			}

			return false;
		}

		public static bool IsKaraka(this Bhava bhava, Body.BodyType graha)
		{
			switch (bhava)
			{
				case Bhava.LagnaBhava:
					switch (graha)
					{
						case Body.BodyType.Sun:
							return true;
					}
					break;
				case Bhava.DhanaBhava:
					switch (graha)
					{
						case Body.BodyType.Jupiter:
							return true;
					}
					break;
				case Bhava.SahajaBhava:
					switch (graha)
					{
						case Body.BodyType.Mars:
							return true;
					}
					break;
				case Bhava.SukhaBhava:
					switch (graha)
					{
						case Body.BodyType.Moon:
						case Body.BodyType.Mercury:
							return true;
					}
					break;
				case Bhava.PutraBhava:
					switch (graha)
					{
						case Body.BodyType.Jupiter:
							return true;
					}
					break;
				case Bhava.ShatruBhava:
					switch (graha)
					{
						case Body.BodyType.Mars:
						case Body.BodyType.Saturn:
						case Body.BodyType.Ketu:
							return true;
					}
					break;
				case Bhava.JayaBhava:
					switch (graha)
					{
						case Body.BodyType.Venus:
							return true;
					}
					break;
				case Bhava.MrtyuBhava:
					switch (graha)
					{
						case Body.BodyType.Saturn:
							return true;
					}
					break;
				case Bhava.DharmaBhava:
					switch (graha)
					{
						case Body.BodyType.Jupiter:
							return true;
					}
					break;
				case Bhava.KarmaBhava:
					switch (graha)
					{
						case Body.BodyType.Jupiter:
						case Body.BodyType.Sun:
						case Body.BodyType.Mercury:
						case Body.BodyType.Saturn:
							return true;
					}
					break;
				case Bhava.LabhaBhava:
					switch (graha)
					{
						case Body.BodyType.Jupiter:
							return true;
					}
					break;
				case Bhava.VyayaBhava:
					switch (graha)
					{
						case Body.BodyType.Saturn:
						case Body.BodyType.Rahu:
							return true;
					}
					break;
				}
			
			return false;
		}
	}
}
