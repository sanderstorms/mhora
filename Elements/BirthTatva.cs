using Mhora.Definitions;

namespace Mhora.Elements
{
	public class BirthTatva
	{
		public Yama   Yama        { get; }
		public Tatva Tatva       { get; }
		public Tatva AntaraTatva { get; }

		// 1. Prithivi /for males ; materialistic/ earthly disposition, enjoying mood, comforts.
		// 2. Jala /for females ; emotional, sensitive, passive, intuitive, detached.
		// 3. Tejas/ for males ; ksatriya type, enthusiastic, imposive, impressive, in power (rising or losing), commanding.
		// 4. Vayu/ for females; active, communicative, enjoying, intellectual.
		// 5. Akasa/ for males; deep thinking, research, analysis, philosophy, science, immaterial, detached
		public BirthTatva (Yama yama, Tatva tatva, Tatva antaraTatva)
		{
			Yama        = yama;
			Tatva       = tatva;
			AntaraTatva = antaraTatva;
		}
	}
}
