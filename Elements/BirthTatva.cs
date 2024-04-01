using Mhora.Definitions;

namespace Mhora.Elements
{
	public class BirthTatva
	{
		public Yama   Yama        { get; }
		public Tatva Tatva       { get; }
		public Tatva AntaraTatva { get; }

		public BirthTatva (Yama yama, Tatva tatva, Tatva antaraTatva)
		{
			Yama        = yama;
			Tatva       = tatva;
			AntaraTatva = antaraTatva;
		}
	}
}
