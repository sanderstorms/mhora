using Mhora.Definitions;
using Mhora.Elements;
using Mhora.Elements.Extensions;

namespace Mhora.Divisions
{
	public static class Hora
	{
		public static bool HoraSunDayNight(this Longitude longitude)
		{
			var part = longitude.PartOfZodiacHouse(2);
			if (longitude.ToZodiacHouse().IsDaySign())
			{
				return part switch
				       {
					       1 => true,
					       _ => false
				       };
			}

			return part switch
			       {
				       1 => false,
				       _ => true
			       };
		}

		public static bool HoraSunOddEven(this Longitude longitude)
		{
			var sign = (int) longitude.ToZodiacHouse();
			var part = longitude.PartOfZodiacHouse(2);
			var mod  = sign % 2;

			return mod switch
			       {
				       1 => part == 1,
				       _ => part != 1
			       };
		}

	}
}
