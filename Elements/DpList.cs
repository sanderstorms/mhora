using System.Collections.Generic;
using Mhora.Calculation;
using Mhora.Definitions;

namespace Mhora.Elements
{
	public class DpList
	{
		public List <DivisionPosition> Positions   {get;}
		public List <DivisionPosition> Arudha      { get;}
		public List <DivisionPosition> Varnada     { get;}
		public List <DivisionPosition> GrahaArudha { get;}

		public DpList(Horoscope h, DivisionType division)
		{
			Positions   = h.PositionList.CalculateDivisionPositions(division);
			Arudha      = h.CalculateArudhaDivisionPositions(division);
			Varnada     = h.CalculateVarnadaDivisionPositions(division);
			GrahaArudha = h.CalculateGrahaArudhaDivisionPositions(division);
		}
	}

}
