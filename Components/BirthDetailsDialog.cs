using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Mhora.Database.World;
using SqlNado;
using SqlNado.Query;

namespace Mhora.Components
{
    public partial class BirthDetailsDialog : Form
    {
        private SQLiteDatabase _db;
        private List<Country> _countries;
        private List<State>   _states;
        private List<City> _cities;
        private City _city;
        private Country _country;
        private State _state;
        private Database.TimeZone _timeZone;

        public BirthDetailsDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
                _db?.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (File.Exists("world.db"))
            {
                try
                {
                    _db = new SQLiteDatabase("world.db");

                    var countries = Query.From<Country>().SelectAll().ToString();
                    _countries = _db.Load<Country>(countries).ToList();;
                    comboBoxCountry.DataSource = _countries;
                    comboBoxCountry.DisplayMember = "name";
                }
                catch (Exception ex)
                {
                    mhora.Log.Exception(ex);
                }

            }
        }

        public Country Country => _country;
        public State State => _state;
        public City City => _city;

        private void OnCountrySelected(object sender, EventArgs e)
        {
            _country = (Country)comboBoxCountry.SelectedItem;
            var states = Query.From<State>().Where(state => state.CountryId == Country.Id).SelectAll().ToString();

            _states = _db.Load<State>(states).ToList();

            comboBoxState.DataSource = _states;
            comboBoxState.DisplayMember = "name";

        }

        private void OnStateSelected(object sender, EventArgs e)
        {
            _state = (State)comboBoxState.SelectedItem;
            var cities = Query.From<City>().Where(city => city.StateId == State.Id).SelectAll().ToString();

            _cities = _db.Load<City>(cities).ToList();
            _timeZone = mhora.TimeZones.FindId(_country.Timezones[0].zoneName);

            comboBoxCity.DataSource = _cities;
            comboBoxCity.DisplayMember = "name";
        }

        private void OnCitySelected(object sender, EventArgs e)
        {
            _city = (City)comboBoxCity.SelectedItem;

            txtLongitude.Text = _city.Longitude.ToString("0.0000");
            txtLatitude.Text = _city.Latitude.ToString("0.0000");
            txtTimezone.Text = _timeZone.offsets[0];

            if (_timeZone.offsets.Count > 1)
            {
                txtDst.Text = _timeZone.offsets[1];
            }
        }
    }
}
