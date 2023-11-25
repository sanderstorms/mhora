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

using System.Diagnostics;

namespace mhora.Kuta
{
    public class KutaNakshatraYoni
    {
        public enum ESex
        {
            IMale,
            IFemale
        }

        public enum EType
        {
            IHorse,
            IElephant,
            ISheep,
            ISerpent,
            IDog,
            ICat,
            IRat,
            ICow,
            IBuffalo,
            ITiger,
            IHare,
            IMonkey,
            ILion,
            IMongoose
        }

        public static ESex getSex(Nakshatra n)
        {
            switch (n.value)
            {
                case Nakshatra.Name.Aswini:
                case Nakshatra.Name.Bharani:
                case Nakshatra.Name.Pushya:
                case Nakshatra.Name.Rohini:
                case Nakshatra.Name.Moola:
                case Nakshatra.Name.Aslesha:
                case Nakshatra.Name.Makha:
                case Nakshatra.Name.UttaraPhalguni:
                case Nakshatra.Name.Swati:
                case Nakshatra.Name.Vishaka:
                case Nakshatra.Name.Jyestha:
                case Nakshatra.Name.PoorvaShada:
                case Nakshatra.Name.PoorvaBhadra:
                case Nakshatra.Name.UttaraShada: return ESex.IMale;
            }

            return ESex.IFemale;
        }

        public static EType getType(Nakshatra n)
        {
            switch (n.value)
            {
                case Nakshatra.Name.Aswini:
                case Nakshatra.Name.Satabisha: return EType.IHorse;
                case Nakshatra.Name.Bharani:
                case Nakshatra.Name.Revati: return EType.IElephant;
                case Nakshatra.Name.Pushya:
                case Nakshatra.Name.Krittika: return EType.ISheep;
                case Nakshatra.Name.Rohini:
                case Nakshatra.Name.Mrigarirsa: return EType.ISerpent;
                case Nakshatra.Name.Moola:
                case Nakshatra.Name.Aridra: return EType.IDog;
                case Nakshatra.Name.Aslesha:
                case Nakshatra.Name.Punarvasu: return EType.ICat;
                case Nakshatra.Name.Makha:
                case Nakshatra.Name.PoorvaPhalguni: return EType.IRat;
                case Nakshatra.Name.UttaraPhalguni:
                case Nakshatra.Name.UttaraBhadra: return EType.ICow;
                case Nakshatra.Name.Swati:
                case Nakshatra.Name.Hasta: return EType.IBuffalo;
                case Nakshatra.Name.Vishaka:
                case Nakshatra.Name.Chittra: return EType.ITiger;
                case Nakshatra.Name.Jyestha:
                case Nakshatra.Name.Anuradha: return EType.IHare;
                case Nakshatra.Name.PoorvaShada:
                case Nakshatra.Name.Sravana: return EType.IMonkey;
                case Nakshatra.Name.PoorvaBhadra:
                case Nakshatra.Name.Dhanishta: return EType.ILion;
                case Nakshatra.Name.UttaraShada: return EType.IMongoose;
            }


            Debug.Assert(false, "KutaNakshatraYoni::getType");
            return EType.IHorse;
        }
    }
}