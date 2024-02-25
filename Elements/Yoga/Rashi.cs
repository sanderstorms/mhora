using System.Collections.Generic;
using Mhora.Definitions;
using Mhora.Util;

namespace Mhora.Elements.Yoga
{
	public class Rashi
	{
		private readonly ZodiacHouse _zh;
		private          Bhava       _bhava;

		internal Rashi(ZodiacHouse zh)
		{
			_zh    = zh;
			Grahas = new List<Graha>();
		}

		public static implicit operator ZodiacHouse(Rashi rashi) => rashi.ZodiacHouse;

		public override string ToString()
		{
			return _zh.Name ();
		}

		public Grahas GrahaList
		{
			get;
			private set;
		}

		public override bool Equals(object obj)
		{
			if (obj is Rashi rashi)
			{
				return (rashi._zh == _zh);
			}

			if (obj is ZodiacHouse zh)
			{
				return (_zh == zh);
			}

			return (false);
		}

		public ZodiacHouse ZodiacHouse => _zh;

		public Bhava Bhava
		{
			get
			{
				if (_bhava == Bhava.None)
				{
					var lagna  = GrahaList.Find(Body.Lagna).Rashi.ZodiacHouse;
					_bhava = (Bhava) lagna.NumHousesBetween(ZodiacHouse);
				}

				return _bhava;
			}
		}

		public Graha Lord => GrahaList.Find(_zh.LordOfSign());

		public List<Graha> Grahas { get; set;}

		internal void Examine(Grahas grahaList)
		{
			GrahaList = grahaList;
			Grahas = GrahaList.FindAll(graha => graha.Rashi == this);

		}
	}
}
