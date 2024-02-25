using System;
using System.Collections;
using System.Collections.Generic;
using Mhora.Definitions;
using Mhora.Util;

namespace Mhora.Elements.Yoga
{
	public class Grahas : IReadOnlyList<Graha>
	{
		private readonly Horoscope    _horoscope;
		private readonly DivisionType _varga;
		private readonly List<Graha>  _grahas;
		private readonly Rashis       _rashis;
		private readonly DpList       _dpList;

		public Grahas (Horoscope horoscope, List<Graha> grahas, Rashis rashis, DpList dplist, DivisionType varga)
		{
			_horoscope = horoscope;
			_dpList    = dplist;
			_varga     = varga;
			_grahas    = grahas;
			_rashis    = rashis;

		}

		public Horoscope Horoscope => _horoscope;

		public DivisionType Varga => _varga;

		public DpList DpList => _dpList;

		public Graha Find (Karaka8 karaka)
		{
			var karaka8 = Karaka8;
			return (karaka8[karaka.Index()]);
		}

		public Graha Find (Karaka7 karaka)
		{
			var karaka7 = Karaka7;
			return (karaka7[karaka.Index()]);
		}

		public Graha Find(Body body)
		{
			return _grahas.Find(graha => graha == body);
		}

		public List<Graha> Karaka8
		{
			get
			{
				var karaka8 = _grahas.FindAll(graha => graha.BodyType == BodyType.Graha);
				karaka8.RemoveAll(graha => graha.Body == Body.Ketu);
				karaka8.Sort((x, y) => y.HouseOffset.CompareTo(x.HouseOffset));
				return (karaka8);
			}
		}

		public List<Graha> Karaka7
		{
			get
			{
				var karaka7 = Karaka8;
				karaka7.RemoveAll(graha => graha.Body == Body.Rahu);
				karaka7.Sort((x, y) => y.HouseOffset.CompareTo(x.HouseOffset));
				return (karaka7);
			}
		}

		public List<Graha> Planets
		{
			get
			{
				var planets = Karaka7;
				planets.Sort((x, y) => x.Angle.CompareTo(y.Angle));
				return (planets);
			}
		}

		internal bool Examine()
		{
			foreach (var graha in _grahas)
			{
				graha.Connect(this);
			}
			_rashis.Examine(this);
			foreach (var graha in _grahas)
			{
				graha.Examine();
			}
			/*
			AkritiYoga      = this.FindAakritiYoga();
			ChandraYoga     = this.FindChandraYoga();
			DhanaYoga       = this.FindDhanaYoga();
			GenericYoga     = this.FindGenericYoga();
			MahaParivartana = this.FindMahaParivartanaYoga();
			Mahapurusha     = this.FindMahapurushaYoga();
			MalikaYoga      = this.FindMalikaYoga();
			RajaYoga        = this.FindRajaYoga();
			*/
			return (true);
		}

		public uint MahaParivartana
		{
			get; private set;
		}

		public uint Mahapurusha
		{
			get; private set;
		}

		public uint MalikaYoga
		{
			get; private set;
		}

		public uint RajaYoga
		{
			get; private set;
		}

		public uint AkritiYoga
		{
			get; private set;
		}

		public uint ChandraYoga
		{
			get; private set;
		}

		public uint DhanaYoga
		{
			get; private set;
		}

		public ulong GenericYoga
		{
			get; private set;
		}

		public IEnumerator<Graha> GetEnumerator()
		{
			return (_grahas.GetEnumerator());
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public int Count => _grahas.Count;

		public Graha this[int index] => _grahas[index];

		public Rashis      Rashis => _rashis;

		public List<Graha> FindAll(Predicate<Graha> func)
		{
			return (_grahas.FindAll(func));
		}
	}
}
