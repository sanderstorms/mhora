using System.Collections.Generic;
using Mhora.Definitions;

namespace Mhora.Elements.Yoga
{
	public class Rasi
	{
		private static List <Rasi>  _rasis = new List <Rasi> ();

		private readonly ZodiacHouse _zh;

		protected Rasi(ZodiacHouse zh)
		{
			_zh    = zh;
			Grahas = new List<Graha>();
		}

		public static implicit operator ZodiacHouse(Rasi rasi)
		{
			return (rasi._zh);
		}

		public static Rasi Find(ZodiacHouse zh)
		{
			var rasi = _rasis.Find (rasi => rasi._zh == zh);
			if (rasi == null)
			{
				rasi = new Rasi (zh);
				_rasis.Add (rasi);
			}
			return rasi;
		}

		public static List<Rasi> Rasis => _rasis;

		public List<Graha> Grahas { get; set;}

		public List<Rasi> Dristi { get; set; }

		public void Examine(Horoscope h, Division varga)
		{
		}
	}
}
