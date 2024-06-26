﻿using System.Collections.Generic;
using Mhora.Definitions;
using Mhora.Elements;

namespace Mhora.Components.VargaControl;

public class ChartItems
{
	private readonly Dictionary<ZodiacHouse, List<DivisionPosition>> _items;

	public ChartItems()
	{
		_items = new Dictionary<ZodiacHouse, List<DivisionPosition>>();
	}

	public List<DivisionPosition> this[ZodiacHouse sign]
	{
		get
		{
			if (_items.TryGetValue(sign, out var dpList) == false)
			{
				dpList = [];
				_items.Add(sign, dpList);
			}

			return dpList;
		}
	}

	public void Clear()
	{
		_items.Clear();
	}

	public void Add(DivisionPosition dp)
	{
		if (_items.TryGetValue(dp.ZodiacHouse, out var dpList))
		{
			dpList.Add(dp);
		}
		else
		{
			dpList = [dp];
			_items.Add(dp.ZodiacHouse, dpList);
		}
	}
}