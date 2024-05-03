using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Mhora.Database.Settings;
using Mhora.Database.World;
using Mhora.Elements;
using Mhora.Util;
using SQLinq;
using SqlNado.Query;
using TimeZone = Mhora.Database.World.TimeZone;

namespace Mhora.Components;

public partial class BirthDetailsDialog : Form
{
	private HoraInfo       _info;
	private List<City>     _cities;

	private bool           _manualEnter;
	private List<State>    _states;
	private TimeZone       _timeZone;

	private static List<Country>  _countries;

	public BirthDetailsDialog()
	{
		InitializeComponent();

		if (_countries == null)
		{
			var countries = Query.From<Country>().SelectAll().ToString();
			_countries = Application.WorldDb.Load<Country>(countries).ToList();
			_countries.Sort();
		}

		comboBoxCountry.DataSource = _countries;
	}

	public BirthDetailsDialog(HoraInfo info) : this()
	{
		Info = info;
	}


	public string ChartName
	{
		get
		{
			return txtName.Text;
		}
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

	public HoraInfo Info
	{
		get
		{
			MhoraGlobalOptions.Instance.City      = City.Name;
			MhoraGlobalOptions.Instance.Latitude  = new DmsPoint(City.Latitude, false);
			MhoraGlobalOptions.Instance.Longitude = City.Longitude;

			var date   = dateTimePicker.Value;
			var time   = timePicker.Value;
			var moment = new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);

			var info   = new HoraInfo (City)
			{
				DateOfBirth = moment,
				Latitude    = MhoraGlobalOptions.Instance.Latitude,
				Longitude   = MhoraGlobalOptions.Instance.Longitude,
				UseDst      = chkUseDst.Checked
			};
			return info;
		}
		set
		{
			_info                = value;
			dateTimePicker.Value = value.DateOfBirth;
			timePicker.Value     = value.DateOfBirth;
			chkUseDst.Checked    = value.UseDst;
			if (value.City != null)
			{
				var query  = Query.From<City>().Where(city => city.Id == value.City.Id).SelectAll();
				var cities = Application.WorldDb.Load<City>(query.ToString()).ToArray();
				if (cities.Length > 0)
				{
					comboBoxCountry.SelectedItem = _countries.Find(country => country.Id == cities[0].Country.Id);
					comboBoxState.SelectedItem   = _states.Find(state => state.Id        == cities[0].StateId);
					comboBoxCity.SelectedItem    = _cities.Find(city => city.Id          == cities[0].Id);
				}
			}
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
			Application.WorldDb?.Dispose();
		}

		base.Dispose(disposing);
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);

		if (_info != null)
		{
			return;
		}
		timePicker.Value = DateTime.Now;
		dateTimePicker.Value = DateTime.Now;

        if (System.IO.File.Exists("world.db"))
        {
            try
            {
                ThreadPool.QueueUserWorkItem (state =>
                {
                    var query  = Query.From<City>().Where(city => city.Name == "Maastricht").SelectAll();
                    var cities = Application.WorldDb.Load<City>(query.ToString()).ToArray();
                    if (cities.Length > 0)
                    {
                        Country = _countries.Find(country => country.Id == cities[0].Country.Id);
                        Invoke(() => comboBoxCountry.SelectedItem = Country);
                        State = _states.Find(state => state.Id == cities[0].StateId);
                        Invoke(() => comboBoxState.SelectedItem = State);
                        City = _cities.Find(city => city.Id == cities[0].Id);
                        Invoke(() => comboBoxCity.SelectedItem = City);
                    }
                });
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

		_states = Application.WorldDb.Load<State>(states).ToList();
		_states.Sort();

		comboBoxState.DataSource = _states;
	}

	private void OnStateSelected(object sender, EventArgs e)
	{
		State = (State) comboBoxState.SelectedItem;
		var cities = Query.From<City>().Where(city => city.StateId == State.Id).SelectAll().ToString();

		_cities = Application.WorldDb.Load<City>(cities).ToList();
		_cities.Sort();
		_timeZone = Country.TimeZone;

		comboBoxCity.DataSource = _cities;
	}

	private void OnCitySelected(object sender, EventArgs e)
	{
		City = (City) comboBoxCity.SelectedItem;

		txtLongitude.Text = City.Longitude.ToString("0.0000");
		txtLatitude.Text  = City.Latitude.ToString("0.0000");
		var location = new DmsLocation(City.Longitude, City.Latitude);
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
		var cities    = Application.WorldDb.Load<City>(sql).ToList();

		_manualEnter             = true;
		comboBoxCity.DataSource  = cities;
		comboBoxCity.DroppedDown = true;
		Cursor.Current           = Cursors.Default;
	}

	private void OnCityDropDown(object sender, EventArgs e)
	{
		if (_manualEnter)
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