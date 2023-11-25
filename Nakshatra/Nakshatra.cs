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

using mhora.Calculation;

namespace mhora
{
    public class Nakshatra
    {
        // int values should not be changed. 
        // used in kalachakra dasa, and various other places.
        public enum Name
        {
            Aswini         = 1,
            Bharani        = 2,
            Krittika       = 3,
            Rohini         = 4,
            Mrigarirsa     = 5,
            Aridra         = 6,
            Punarvasu      = 7,
            Pushya         = 8,
            Aslesha        = 9,
            Makha          = 10,
            PoorvaPhalguni = 11,
            UttaraPhalguni = 12,
            Hasta          = 13,
            Chittra        = 14,
            Swati          = 15,
            Vishaka        = 16,
            Anuradha       = 17,
            Jyestha        = 18,
            Moola          = 19,
            PoorvaShada    = 20,
            UttaraShada    = 21,
            Sravana        = 22,
            Dhanishta      = 23,
            Satabisha      = 24,
            PoorvaBhadra   = 25,
            UttaraBhadra   = 26,
            Revati         = 27
        }

        public Nakshatra(Name nak)
        {
            value = (Name)Basics.normalize_inc(1, 27, (int)nak);
        }

        public Name value
        {
            get;
            set;
        }

        public override string ToString()
        {
            switch (value)
            {
                case Name.Aswini:         return "Aswini";
                case Name.Bharani:        return "Bharani";
                case Name.Krittika:       return "Krittika";
                case Name.Rohini:         return "Rohini";
                case Name.Mrigarirsa:     return "Mrigasira";
                case Name.Aridra:         return "Ardra";
                case Name.Punarvasu:      return "Punarvasu";
                case Name.Pushya:         return "Pushyami";
                case Name.Aslesha:        return "Aslesha";
                case Name.Makha:          return "Makha";
                case Name.PoorvaPhalguni: return "P.Phalguni";
                case Name.UttaraPhalguni: return "U.Phalguni";
                case Name.Hasta:          return "Hasta";
                case Name.Chittra:        return "Chitta";
                case Name.Swati:          return "Swati";
                case Name.Vishaka:        return "Visakha";
                case Name.Anuradha:       return "Anuradha";
                case Name.Jyestha:        return "Jyeshtha";
                case Name.Moola:          return "Moola";
                case Name.PoorvaShada:    return "P.Ashada";
                case Name.UttaraShada:    return "U.Ashada";
                case Name.Sravana:        return "Sravana";
                case Name.Dhanishta:      return "Dhanishta";
                case Name.Satabisha:      return "Shatabisha";
                case Name.PoorvaBhadra:   return "P.Bhadra";
                case Name.UttaraBhadra:   return "U.Bhadra";
                case Name.Revati:         return "Revati";
                default:                  return "---";
            }
        }

        public string toShortString()
        {
            switch (value)
            {
                case Name.Aswini:         return "Asw";
                case Name.Bharani:        return "Bha";
                case Name.Krittika:       return "Kri";
                case Name.Rohini:         return "Roh";
                case Name.Mrigarirsa:     return "Mri";
                case Name.Aridra:         return "Ari";
                case Name.Punarvasu:      return "Pun";
                case Name.Pushya:         return "Pus";
                case Name.Aslesha:        return "Asl";
                case Name.Makha:          return "Mak";
                case Name.PoorvaPhalguni: return "P.Ph";
                case Name.UttaraPhalguni: return "U.Ph";
                case Name.Hasta:          return "Has";
                case Name.Chittra:        return "Chi";
                case Name.Swati:          return "Swa";
                case Name.Vishaka:        return "Vis";
                case Name.Anuradha:       return "Anu";
                case Name.Jyestha:        return "Jye";
                case Name.Moola:          return "Moo";
                case Name.PoorvaShada:    return "P.Ash";
                case Name.UttaraShada:    return "U.Ash";
                case Name.Sravana:        return "Sra";
                case Name.Dhanishta:      return "Dha";
                case Name.Satabisha:      return "Sat";
                case Name.PoorvaBhadra:   return "P.Bh";
                case Name.UttaraBhadra:   return "U.Bh";
                case Name.Revati:         return "Rev";
                default:                  return "---";
            }
        }

        public int normalize()
        {
            return Basics.normalize_inc(1, 27, (int)value);
        }

        public Nakshatra add(int i)
        {
            var snum = Basics.normalize_inc(1, 27, (int)value + i - 1);
            return new Nakshatra((Name)snum);
        }

        public Nakshatra addReverse(int i)
        {
            var snum = Basics.normalize_inc(1, 27, (int)value - i + 1);
            return new Nakshatra((Name)snum);
        }
    }
}