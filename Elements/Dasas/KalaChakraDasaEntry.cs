
using Mhora.Definitions;
using Mhora.Util;

namespace Mhora.Elements.Dasas
{
	public class KalaChakraDasaEntry : DasaEntry
	{
		private bool _direct;
		public KalaChakraDasaEntry(ZodiacHouse zh, TimeOffset startUt, TimeOffset dasaLength, int level, bool direct, string name) : base (zh, startUt, dasaLength, level, name)
		{
			_direct = direct;
		}

		public bool Direct => _direct;
	}
}
