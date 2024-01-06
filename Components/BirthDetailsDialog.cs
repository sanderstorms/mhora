using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Mhora.Database.Settings;
using Mhora.Database.World;
using Mhora.Elements.Calculation;
using Mhora.Elements.Hora;
using mhora.Util;
using SQLinq;
using SqlNado;
using SqlNado.Query;
using TimeZone = mhora.Database.World.TimeZone;

namespace Mhora.Components;

public partial class BirthDetailsDialog : Form
{
	private List<City>     _cities;
	private List<Country>  _countries;
	private SQLiteDatabase _db;
	private bool           _manualEnter;
	private List<State>    _states;
	private TimeZone       _timeZone;

	public BirthDetailsDialog()
	{
		InitializeComponent();
	}

	public Country Country
	{
		get;
		private set;
	}

	public State State
	{
		get;
		private set;
	}

	public City City
	{
		get;
		private set;
	}

	public Horoscope Horoscope
	{
		get
		{
			MhoraGlobalOptions.Instance.Latitude  = new HMSInfo(City.Latitude);
			MhoraGlobalOptions.Instance.Longitude = new HMSInfo(City.Longitude);
			MhoraGlobalOptions.Instance.TimeZone  = new HMSInfo(Country.Timezones[0].gmtOffset / 3600.0);

			var dateTime = dateTimePicker.Value;
			var moment   = new Moment(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
			var info     = new HoraInfo(moment, MhoraGlobalOptions.Instance.Latitude, MhoraGlobalOptions.Instance.Longitude, MhoraGlobalOptions.Instance.TimeZone);
			return new Horoscope(info, new HoroscopeOptions());
		}
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		if (ActiveControl == comboBoxCity && comboBoxCity.DroppedDown == false)
		{
			switch (keyData)
			{
				case Keys.Enter:
				{
					BeginInvoke(LookupCity);
				}
					return true;

				case Keys.Back:
				{
					if (string.IsNullOrEmpty(comboBoxCity.SelectedText) == false)
					{
						BeginInvoke(() =>
						{
							comboBoxCity.Text       = null;
							_manualEnter            = false;
							comboBoxCity.DataSource = _cities;
							return true;
						});
					}
				}
					break;
			}
		}

		return base.ProcessCmdKey(ref msg, keyData);
	}

	/// <summary>
	///     Clean up any resources being used.
	/// </summary>
	/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
			_db?.Dispose();
		}

		base.Dispose(disposing);
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		timePicker.Value = DateTime.Now;

		if (File.Exists("world.db"))
		{
			try
			{
				_db = new SQLiteDatabase("world.db");

				var countries = Query.From<Country>().SelectAll().ToString();
				_countries = _db.Load<Country>(countries).ToList();
				_countries.Sort();

				comboBoxCountry.DataSource = _countries;

				var query  = Query.From<City>().Where(city => city.Name == "Maastricht").SelectAll();
				var cities = _db.Load<City>(query.ToString()).ToArray();
				if (cities.Length > 0)
				{
					comboBoxCountry.SelectedItem = _countries.Find(country => country.Id == cities[0].Country.Id);
					comboBoxState.SelectedItem   = _states.Find(state => state.Id        == cities[0].StateId);
					comboBoxCity.SelectedItem    = _cities.Find(city => city.Id          == cities[0].Id);
				}
			}
			catch (Exception ex)
			{
				Application.Log.Exception(ex);
			}
		}
	}

	private void OnCountrySelected(object sender, EventArgs e)
	{
		Country = (Country) comboBoxCountry.SelectedItem;
		var states = Query.From<State>().Where(state => state.CountryId == Country.Id).SelectAll().ToString();

		_states = _db.Load<State>(states).ToList();
		_states.Sort();

		comboBoxState.DataSource = _states;
	}

	private void OnStateSelected(object sender, EventArgs e)
	{
		State = (State) comboBoxState.SelectedItem;
		var cities = Query.From<City>().Where(city => city.StateId == State.Id).SelectAll().ToString();

		_cities = _db.Load<City>(cities).ToList();
		_cities.Sort();
		_timeZone = TimeZone.TimeZones.FindId(Country.Timezones[0].zoneName);

		comboBoxCity.DataSource = _cities;
	}

	private void OnCitySelected(object sender, EventArgs e)
	{
		City = (City) comboBoxCity.SelectedItem;

		txtLongitude.Text = City.Longitude.ToString("0.0000");
		txtLatitude.Text  = City.Latitude.ToString("0.0000");
		var location = new LocationConverter.DmsLocation(City.Longitude, City.Latitude);
		var str      = location.ToString().Split(',');

		txtLongitude2.Text = str[0];
		txtLatitude2.Text  = str[1];
		txtTimezone.Text   = _timeZone.offsets[0];

		if (_timeZone.offsets.Count > 1)
		{
			txtDst.Text = _timeZone.offsets[1];
		}

		if (_manualEnter)
		{
			State              = City.State;
			comboBoxState.Text = State.ToString();
		}
	}

	private void LookupCity()
	{
		var query = from city in new SQLinq<City>() where city.Name.StartsWith(comboBoxCity.Text) && city.CountryId == Country.Id select city;

		var sql = query.ToSQL().ToQuery();

		var cityQuery = Query.From<City>().Where(city => city.Name.StartsWith(comboBoxCity.Text) && city.CountryId == Country.Id).SelectAll();
		var cities    = _db.Load<City>(sql).ToList();

		_manualEnter             = true;
		comboBoxCity.DataSource  = cities;
		comboBoxCity.DroppedDown = true;
		Cursor.Current           = Cursors.Default;
	}

	private void OnCityDropDown(object sender, EventArgs e)
	{
		if (_manualEnter == false)
		{
			comboBoxCity.AutoCompleteSource = AutoCompleteSource.ListItems;
			comboBoxCity.AutoCompleteMode   = AutoCompleteMode.Suggest;
		}
	}

	private void OnCityDropDownClose(object sender, EventArgs e)
	{
		if (_manualEnter == false)
		{
			comboBoxCity.AutoCompleteSource = AutoCompleteSource.None;
			comboBoxCity.AutoCompleteMode   = AutoCompleteMode.None;
		}
	}
}