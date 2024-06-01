using Mhora.Calculation;
using Mhora.Definitions;
using Mhora.Divisions;
using Mhora.Elements.Extensions;
using Mhora.Util;

namespace Mhora.Elements
{
	public class Cusp
	{
		public         int         Parts       { get; protected set; }
		public         Longitude   Offset      { get; }
		public         ZodiacHouse ZodiacHouse { get; set; }
		public         int         Part        { get; protected set; }
		public         double      Length      => 30.0 / Parts;
		public virtual Longitude   Lower       => (double) Base + ((Part - 1) * Length);
		public virtual Longitude   Upper       => Lower.Add(Length);

		private Longitude Base => (ZodiacHouse.Index() - 1) * 30.0;

		protected Cusp()
		{

		}

		public Cusp(Longitude longitude, int parts)
		{
			Parts       = parts;
			ZodiacHouse = longitude.ToZodiacHouse();
			Part        = longitude.PartOfZodiacHouse(parts);
			Offset      = longitude.Sub(Lower);
		}
	}

	public class Cusps2 : Cusp
	{
		public override Longitude Lower { get; }
		public override Longitude Upper { get; }

		public Cusps2(Longitude lower, Longitude upper, int part, int parts)
		{
			Lower       = lower;
			Upper       = upper;
			Part        = part;
			Parts       = parts;
			ZodiacHouse = lower.ToZodiacHouse();
		}
	}
}
