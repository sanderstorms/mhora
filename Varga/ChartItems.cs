using Mhora.Calculation;
using Mhora.Varga;
using System.Collections.Generic;

namespace mhora.Varga
{
	public class ChartItems
	{
		private readonly Dictionary<ZodiacHouse.Name, List<DivisionPosition>> _items;

		public ChartItems()
		{
			_items = new Dictionary<ZodiacHouse.Name, List<DivisionPosition>>();
		}

		public void Clear()
		{
			_items.Clear();
		}

		public List<DivisionPosition> this[ZodiacHouse.Name sign]
		{
			get
			{
				if (_items.TryGetValue(sign, out var dpList) == false)
				{
					dpList = new List<DivisionPosition>();
					_items.Add (sign, dpList);
				}
				return dpList;
			}
		}

		public void Add(DivisionPosition dp)
		{
			if (_items.TryGetValue(dp.zodiac_house.value, out var dpList))
			{
				dpList.Add(dp);
			}
			else
			{
				dpList = new List<DivisionPosition>
				{
					dp
				};
				_items.Add(dp.zodiac_house.value, dpList);
			}
		}
	}
}
