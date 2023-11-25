/******
Copyright (C) 2005 Ajit Krishnan (http://www.mudgala.com)

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
******/

using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using mhora.Calculation;
using mhora.Components.Property;
using mhora.Settings;
using mhora.Util;

namespace mhora.Hora
{
    /// <summary>
    ///     A class containing all required input from the user for a given chart
    ///     (e.g.) all the information contained in a .jhd file
    /// </summary>
    [Serializable]
    public class HoraInfo : MhoraSerializableOptions, ICloneable, ISerializable
    {
        public enum EFileType
        {
            JagannathaHora,
            MudgalaHora
        }

        public enum Name
        {
            Birth,
            Progression,
            TithiPravesh,
            Transit,
            Dasa
        }

        private const string CAT_TOB = "1: Birth Info";

        private const string CAT_EVT = "2: Events";

        //public double lon, lat, alt, tz;
        public double          alt;
        public double          defaultYearCompression;
        public double          defaultYearLength;
        public ToDate.DateType defaultYearType = ToDate.DateType.FixedYear;

        private UserEvent[] events;
        public  EFileType   FileType;

        public HMSInfo lon,
                       lat,
                       tz;

        public string name;
        public Moment tob;
        public Name   type;

        protected HoraInfo(SerializationInfo info, StreamingContext context)
            :
            this()
        {
            Constructor(GetType(), info, context);
        }

        public HoraInfo(Moment atob, HMSInfo alat, HMSInfo alon, HMSInfo atz)
        {
            tob      = atob;
            lon      = alon;
            lat      = alat;
            tz       = atz;
            alt      = 0.0;
            type     = Name.Birth;
            FileType = EFileType.MudgalaHora;
            events   = new UserEvent[0];
        }

        public HoraInfo()
        {
            var t = DateTime.Now;
            tob      = new Moment(t.Year, t.Month, t.Day, t.Hour, t.Minute, t.Second);
            lon      = (HMSInfo)MhoraGlobalOptions.Instance.Longitude.Clone();
            lat      = (HMSInfo)MhoraGlobalOptions.Instance.Latitude.Clone();
            tz       = (HMSInfo)MhoraGlobalOptions.Instance.TimeZone.Clone();
            alt      = 0.0;
            type     = Name.Birth;
            FileType = EFileType.MudgalaHora;
            events   = new UserEvent[0];
        }

        [Category(CAT_TOB)]
        [PropertyOrder(1)]
        [PGDisplayName("Time of Birth")]
        [Description("Date of Birth. Format is 'dd Mmm yyyy hh:mm:ss'\n Example 23 Mar 1979 23:11:00")]
        public Moment DateOfBirth
        {
            get =>
                tob;
            set =>
                tob = value;
        }

        [Category(CAT_TOB)]
        [PropertyOrder(2)]
        [Description("Latitude. Format is 'hh D mm:ss mm:ss'\n Example 23 N 24:00")]
        public HMSInfo Latitude
        {
            get =>
                lat;
            set =>
                lat = value;
        }

        [Category(CAT_TOB)]
        [PropertyOrder(3)]
        [Description("Longitude. Format is 'hh D mm:ss mm:ss'\n Example 23 E 24:00")]
        public HMSInfo Longitude
        {
            get =>
                lon;
            set =>
                lon = value;
        }

        [Category(CAT_TOB)]
        [PropertyOrder(4)]
        [PGDisplayName("Time zone")]
        [Description("Time Zone. Format is 'hh D mm:ss mm:ss'\n Example 3 E 00:00")]
        public HMSInfo TimeZone
        {
            get =>
                tz;
            set =>
                tz = value;
        }

        [Category(CAT_TOB)]
        [PropertyOrder(5)]
        public double Altitude
        {
            get =>
                alt;
            set =>
                alt = value;
        }

        [Category(CAT_EVT)]
        [PropertyOrder(1)]
        [Description("Events")]
        public UserEvent[] Events
        {
            get =>
                events;
            set =>
                events = value;
        }

        public object Clone()
        {
            var hi = new HoraInfo((Moment)tob.Clone(), (HMSInfo)lat.Clone(), (HMSInfo)lon.Clone(), (HMSInfo)tz.Clone());
            hi.events                 = events;
            hi.name                   = name;
            hi.defaultYearCompression = defaultYearCompression;
            hi.defaultYearLength      = defaultYearLength;
            hi.defaultYearType        = defaultYearType;
            return hi;
        }

        void ISerializable.GetObjectData(
            SerializationInfo info,
            StreamingContext  context)
        {
            GetObjectData(GetType(), info, context);
        }
    }
}