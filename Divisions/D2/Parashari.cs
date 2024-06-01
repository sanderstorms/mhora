using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mhora.Definitions;
using Mhora.Elements;
using Mhora.Elements.Extensions;

namespace Mhora.Divisions.D2
{
	// D2-Parasharior Cancer/Leo or Uma Shambhu Hora
	// Sign			Ari	Tau	Gem	Can	Leo	Vir	Lib	Sco	Sag	Cap	Aua	Pis
	// 0 - 15⁰		Leo	Can	Leo	Can	Leo	Can	Leo	Can	Leo	Can	Leo	Can
	// 15⁰ - 30⁰	Can	Leo	Can	Leo	Can	Leo	Can	Leo	Can	Leo	Can	Leo
	public class Parashari
	{
		public DivisionPosition ToDivisionPosition(Position position)
		{
			ZodiacHouse zh;
			var         rulerIndex = 0;
			if (position.Longitude.HoraSunOddEven())
			{
				zh         = ZodiacHouse.Leo;
				rulerIndex = 1;
			}
			else
			{
				zh         = ZodiacHouse.Can;
				rulerIndex = 2;
			}

			var cusp = new Cusp(position.Longitude, 2);
			var dp = new DivisionPosition(position.Name, position.BodyType, zh, cusp)
			{
				Longitude  = zh.DivisionalLongitude(position.Longitude, 2),
				RulerIndex = rulerIndex
			};
			return dp;
		}
	}
}
