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
using Mhora.Settings;

namespace Mhora.Hora
{
    [Serializable]
    [TypeConverter(typeof(HMSInfoConverter))]
    public class HMSInfo : MhoraSerializableOptions, ICloneable, ISerializable
    {
        public enum dir_type
        {
            NS = 1,
            EW = 2
        }

        public dir_type direction;

        private int m_hour,
                    m_minute,
                    m_second;

        protected HMSInfo(SerializationInfo info, StreamingContext context)
            :
            this()
        {
            Constructor(GetType(), info, context);
        }

        public HMSInfo()
        {
            m_hour    = m_minute = m_second = 0;
            direction = dir_type.NS;
        }

        public HMSInfo(int hour, int min, int sec, dir_type dt)
        {
            m_hour    = hour;
            m_minute  = min;
            m_second  = sec;
            direction = dt;
        }

        public HMSInfo(double hms)
        {
            var hour = Math.Floor(hms);
            hms = (hms - hour) * 60.0;
            var min = Math.Floor(hms);
            hms = (hms - min) * 60.0;
            var sec = Math.Floor(hms);
            m_hour    = (int)hour;
            m_minute  = (int)min;
            m_second  = (int)sec;
            direction = dir_type.NS;
        }

        public dir_type dir
        {
            get =>
                direction;
            set =>
                direction = value;
        }

        public int degree
        {
            get =>
                m_hour;
            set =>
                m_hour = value;
        }

        public int minute
        {
            get =>
                m_minute;
            set =>
                m_minute = value;
        }

        public int second
        {
            get =>
                m_second;
            set =>
                m_second = value;
        }

        public object Clone()
        {
            return new HMSInfo(m_hour, m_minute, m_second, direction);
        }

        void ISerializable.GetObjectData(
            SerializationInfo info,
            StreamingContext  context)
        {
            GetObjectData(GetType(), info, context);
        }

        public double toDouble()
        {
            if (m_hour >= 0)
            {
                return m_hour + m_minute / 60.0 + m_second / 3600.0;
            }

            return m_hour - m_minute / 60.0 - m_second / 3600.0;
        }

        public override string ToString()
        {
            string dirs;
            if (direction == dir_type.EW && m_hour < 0)
            {
                dirs = "W";
            }
            else if (direction == dir_type.EW)
            {
                dirs = "E";
            }
            else if (direction == dir_type.NS && m_hour < 0)
            {
                dirs = "S";
            }
            else
            {
                dirs = "N";
            }


            var m_htemp = m_hour >= 0 ? m_hour : m_hour * -1;
            return (m_htemp < 10 ? "0" : string.Empty) + m_htemp + " " + dirs + " " + (m_minute < 10 ? "0" : string.Empty) + m_minute + ":" + (m_second < 10 ? "0" : string.Empty) + m_second;
        }
    }
}