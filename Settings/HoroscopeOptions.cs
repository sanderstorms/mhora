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
using Mhora.Calculation;
using Mhora.Components.Property;
using Mhora.Hora;
using Mhora.Util;

namespace Mhora.Settings
{
    [Serializable]
    public class HoroscopeOptions : MhoraSerializableOptions, ICloneable, ISerializable
    {
        public enum AyanamsaType
        {
            Fagan        = 0,
            Lahiri       = 1,
            Raman        = 3,
            Ushashashi   = 4,
            Krishnamurti = 5
        }

        [TypeConverter(typeof(EnumDescConverter))]
        public enum EBhavaType
        {
            [Description("Lagna is at the beginning of the bhava")]
            Start,

            [Description("Lagna is in the middle of the bhava")]
            Middle
        }

        public enum EGrahaPositionType
        {
            Apparent,
            True
        }

        [TypeConverter(typeof(EnumDescConverter))]
        public enum EHoraType
        {
            [Description("Day split into equal parts")]
            Sunriset,

            [Description("Daytime and Nighttime split into equal parts")]
            SunrisetEqual,

            [Description("Day (Local Mean Time) split into equal parts")]
            Lmt
        }

        [TypeConverter(typeof(EnumDescConverter))]
        public enum EMaandiType
        {
            [Description("Rises at the beginning of Saturn's portion")]
            SaturnBegin,

            [Description("Rises in the middle of Saturn's portion")]
            SaturnMid,

            [Description("Rises at the end of Saturn's portion")]
            SaturnEnd,

            [Description("Rises at the beginning of the lordless portion")]
            LordlessBegin,

            [Description("Rises in the middle of the lordless portion")]
            LordlessMid,

            [Description("Rises at the end of the lordless portion")]
            LordlessEnd
        }

        [TypeConverter(typeof(EnumDescConverter))]
        public enum ENodeType
        {
            [Description("Mean Positions of nodes")]
            Mean,

            [Description("True Positions of nodes")]
            True
        }

        [TypeConverter(typeof(EnumDescConverter))]
        public enum EUpagrahaType
        {
            [Description("Rises at the beginning of the grahas portion")]
            Begin,

            [Description("Rises in the middle of the grahas portion")]
            Mid,

            [Description("Rises at the end of the grahas portion")]
            End
        }

        [TypeConverter(typeof(EnumDescConverter))]
        public enum SunrisePositionType
        {
            [Description("Sun's center rises")]
            TrueDiscCenter,

            [Description("Sun's edge rises")]
            TrueDiscEdge,

            [Description("Sun's center appears to rise")]
            ApparentDiscCenter,

            [Description("Sun's edge apprears to rise")]
            ApparentDiscEdge,

            [Description("6am Local Mean Time")]
            Lmt
        }

        protected const string              CAT_GENERAL  = "1: General Settings";
        protected const string              CAT_GRAHA    = "2: Graha Settings";
        protected const string              CAT_SUNRISE  = "3: Sunrise Settings";
        protected const string              CAT_UPAGRAHA = "4: Upagraha Settings";
        public          EGrahaPositionType  grahaPositionType;
        private         AyanamsaType        mAyanamsa;
        private         HMSInfo             mAyanamsaOffset;
        private         EBhavaType          mBhavaType;
        private         string              mEphemPath;
        private         EMaandiType         mGulikaType;
        private         EHoraType           mHoraType;
        private         EHoraType           mKalaType;
        private         EMaandiType         mMaandiType;
        private         SunrisePositionType mSunrisePosition;
        private         EUpagrahaType       mUpagrahaType;
        private         Longitude           mUserLongitude;
        public          ENodeType           nodeType;

        public HoroscopeOptions()
        {
            sunrisePosition   = SunrisePositionType.TrueDiscEdge;
            mHoraType         = EHoraType.Lmt;
            mKalaType         = EHoraType.Sunriset;
            mBhavaType        = EBhavaType.Start;
            grahaPositionType = EGrahaPositionType.True;
            nodeType          = ENodeType.Mean;
            Ayanamsa          = AyanamsaType.Lahiri;
            AyanamsaOffset    = new HMSInfo(0, 0, 0, HMSInfo.dir_type.EW);
            mUserLongitude    = new Longitude(0);
            MaandiType        = EMaandiType.SaturnBegin;
            GulikaType        = EMaandiType.SaturnMid;
            UpagrahaType      = EUpagrahaType.Mid;
            mEphemPath        = getExeDir() + "\\eph";
        }

        protected HoroscopeOptions(SerializationInfo info, StreamingContext context)
            :
            this()
        {
            Constructor(GetType(), info, context);
        }


        [Category(CAT_GENERAL)]
        [PropertyOrder(1)]
        [PGDisplayName("Full Ephemeris Path")]
        public string EphemerisPath
        {
            get =>
                mEphemPath;
            set =>
                mEphemPath = value;
        }

        [PropertyOrder(2)]
        [Category(CAT_GENERAL)]
        public AyanamsaType Ayanamsa
        {
            get =>
                mAyanamsa;
            set =>
                mAyanamsa = value;
        }

        [PropertyOrder(4)]
        [Category(CAT_GENERAL)]
        [PGDisplayName("Custom Longitude")]
        public Longitude CustomBodyLongitude
        {
            get =>
                mUserLongitude;
            set =>
                mUserLongitude = value;
        }

        [Category(CAT_GENERAL)]
        [PropertyOrder(3)]
        [PGDisplayName("Ayanamsa Offset")]
        public HMSInfo AyanamsaOffset
        {
            get =>
                mAyanamsaOffset;
            set =>
                mAyanamsaOffset = value;
        }

        [Category(CAT_UPAGRAHA)]
        [PropertyOrder(1)]
        [PGDisplayName("Upagraha")]
        public EUpagrahaType UpagrahaType
        {
            get =>
                mUpagrahaType;
            set =>
                mUpagrahaType = mUpagrahaType;
        }

        [Category(CAT_UPAGRAHA)]
        [PropertyOrder(2)]
        [PGDisplayName("Maandi")]
        public EMaandiType MaandiType
        {
            get =>
                mMaandiType;
            set =>
                mMaandiType = value;
        }

        [Category(CAT_UPAGRAHA)]
        [PropertyOrder(3)]
        [PGDisplayName("Gulika")]
        public EMaandiType GulikaType
        {
            get =>
                mGulikaType;
            set =>
                mGulikaType = value;
        }

        [Category(CAT_SUNRISE)]
        [PropertyOrder(1)]
        [PGDisplayName("Sunrise")]
        public SunrisePositionType sunrisePosition
        {
            get =>
                mSunrisePosition;
            set =>
                mSunrisePosition = value;
        }

        [Category(CAT_SUNRISE)]
        [PropertyOrder(2)]
        [PGDisplayName("Hora")]
        public EHoraType HoraType
        {
            get =>
                mHoraType;
            set =>
                mHoraType = value;
        }

        [Category(CAT_SUNRISE)]
        [PropertyOrder(3)]
        [PGDisplayName("Kala")]
        public EHoraType KalaType
        {
            get =>
                mKalaType;
            set =>
                mKalaType = value;
        }

        //public EGrahaPositionType GrahaPositionType
        //{
        //	get { return grahaPositionType; }
        //	set { grahaPositionType = value; }
        //}
        [Category(CAT_GRAHA)]
        [PropertyOrder(1)]
        [PGDisplayName("Rahu / Ketu")]
        public ENodeType NodeType
        {
            get =>
                nodeType;
            set =>
                nodeType = value;
        }

        [Category(CAT_GRAHA)]
        [PropertyOrder(2)]
        [PGDisplayName("Bhava")]
        public EBhavaType BhavaType
        {
            get =>
                mBhavaType;
            set =>
                mBhavaType = value;
        }

        public object Clone()
        {
            var o = new HoroscopeOptions();
            o.sunrisePosition   = sunrisePosition;
            o.grahaPositionType = grahaPositionType;
            o.nodeType          = nodeType;
            o.Ayanamsa          = Ayanamsa;
            o.AyanamsaOffset    = AyanamsaOffset;
            o.HoraType          = HoraType;
            o.KalaType          = KalaType;
            o.BhavaType         = BhavaType;
            o.mUserLongitude    = mUserLongitude.add(0);
            o.MaandiType        = MaandiType;
            o.GulikaType        = GulikaType;
            o.UpagrahaType      = UpagrahaType;
            o.EphemerisPath     = EphemerisPath;
            return o;
        }

        void ISerializable.GetObjectData(
            SerializationInfo info,
            StreamingContext  context)
        {
            GetObjectData(GetType(), info, context);
        }

        public void Copy(HoroscopeOptions o)
        {
            sunrisePosition   = o.sunrisePosition;
            grahaPositionType = o.grahaPositionType;
            nodeType          = o.nodeType;
            Ayanamsa          = o.Ayanamsa;
            AyanamsaOffset    = o.AyanamsaOffset;
            HoraType          = o.HoraType;
            KalaType          = o.KalaType;
            BhavaType         = o.BhavaType;
            mUserLongitude    = o.mUserLongitude.add(0);
            MaandiType        = o.MaandiType;
            GulikaType        = o.GulikaType;
            UpagrahaType      = o.UpagrahaType;
            EphemerisPath     = o.EphemerisPath;
        }
    }
}