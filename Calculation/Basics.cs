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
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using Mhora.Body;
using Mhora.Settings;
using Mhora.SwissEph;
using Mhora.Util;
using Mhora.Varga;

namespace Mhora.Calculation
{
    /// <summary>
    ///     Simple functions that don't belong anywhere else
    /// </summary>
    public class Basics
    {
        /// <summary>
        ///     Enumeration of the various division types. Where a varga has multiple
        ///     definitions, each of these should be specified separately below
        /// </summary>
        [TypeConverter(typeof(EnumDescConverter))]
        public enum DivisionType
        {
            [Description("D-1: Rasi")]
            Rasi = 0,

            [Description("D-9: Navamsa")]
            Navamsa,

            [Description("D-2: Hora (Parashara)")]
            HoraParasara,

            [Description("D-2: Hora (Jagannatha)")]
            HoraJagannath,

            [Description("D-2: Hora (Parivritti)")]
            HoraParivrittiDwaya,

            [Description("D-2: Hora (Kashinatha)")]
            HoraKashinath,

            [Description("D-3: Dreshkana (Parashara)")]
            DrekkanaParasara,

            [Description("D-3: Dreshkana (Jagannatha)")]
            DrekkanaJagannath,

            [Description("D-3: Dreshkana (Somnatha)")]
            DrekkanaSomnath,

            [Description("D-3: Dreshkana (Parivritti)")]
            DrekkanaParivrittitraya,

            [Description("D-4: Chaturthamsa")]
            Chaturthamsa,

            [Description("D-5: Panchamsa")]
            Panchamsa,

            [Description("D-6: Shashtamsa")]
            Shashthamsa,

            [Description("D-7: Saptamsa")]
            Saptamsa,

            [Description("D-8: Ashtamsa")]
            Ashtamsa,

            [Description("D-8: Ashtamsa (Raman)")]
            AshtamsaRaman,

            [Description("D-10: Dasamsa")]
            Dasamsa,

            [Description("D-11: Rudramsa (Rath)")]
            Rudramsa,

            [Description("D-11: Rudramsa (Raman)")]
            RudramsaRaman,

            [Description("D-12: Dwadasamsa")]
            Dwadasamsa,

            [Description("D-16: Shodasamsa")]
            Shodasamsa,

            [Description("D-20: Vimsamsa")]
            Vimsamsa,

            [Description("D-24: Chaturvimsamsa")]
            Chaturvimsamsa,

            [Description("D-27: Nakshatramsa")]
            Nakshatramsa,

            [Description("D-30: Trimsamsa (Parashara)")]
            Trimsamsa,

            [Description("D-30: Trimsamsa (Parivritti)")]
            TrimsamsaParivritti,

            [Description("D-30: Trimsamsa (Simple)")]
            TrimsamsaSimple,

            [Description("D-40: Khavedamsa")]
            Khavedamsa,

            [Description("D-45: Akshavedamsa")]
            Akshavedamsa,

            [Description("D-60: Shashtyamsa")]
            Shashtyamsa,

            [Description("D-108: Ashtottaramsa (Regular)")]
            Ashtottaramsa,

            [Description("D-150: Nadiamsa (Equal Division)")]
            Nadiamsa,

            [Description("D-150: Nadiamsa (Chandra Kala Nadi)")]
            NadiamsaCKN,

            [Description("D-9-12: Navamsa-Dwadasamsa (Composite)")]
            NavamsaDwadasamsa,

            [Description("D-12-12: Dwadasamsa-Dwadasamsa (Composite)")]
            DwadasamsaDwadasamsa,

            [Description("D-1: Bhava (9 Padas)")]
            BhavaPada,

            [Description("D-1: Bhava (Equal Length)")]
            BhavaEqual,

            [Description("D-1: Bhava (Alcabitus)")]
            BhavaAlcabitus,

            [Description("D-1: Bhava (Axial)")]
            BhavaAxial,

            [Description("D-1: Bhava (Campanus)")]
            BhavaCampanus,

            [Description("D-1: Bhava (Koch)")]
            BhavaKoch,

            [Description("D-1: Bhava (Placidus)")]
            BhavaPlacidus,

            [Description("D-1: Bhava (Regiomontanus)")]
            BhavaRegiomontanus,

            [Description("D-1: Bhava (Sripati)")]
            BhavaSripati,

            [Description("Regular: Parivritti")]
            GenericParivritti,

            [Description("Regular: Shashtamsa (d-6)")]
            GenericShashthamsa,

            [Description("Regular: Saptamsa (d-7)")]
            GenericSaptamsa,

            [Description("Regular: Dasamsa (d-10)")]
            GenericDasamsa,

            [Description("Regular: Dwadasamsa (d-12)")]
            GenericDwadasamsa,

            [Description("Regular: Chaturvimsamsa (d-24)")]
            GenericChaturvimsamsa,

            [Description("Trikona: Drekkana (d-3)")]
            GenericDrekkana,

            [Description("Trikona: Shodasamsa (d-16)")]
            GenericShodasamsa,

            [Description("Trikona: Vimsamsa (d-20)")]
            GenericVimsamsa,

            [Description("Kendra: Chaturthamsa (d-4)")]
            GenericChaturthamsa,

            [Description("Kendra: Nakshatramsa (d-27)")]
            GenericNakshatramsa
        }

        public enum Muhurta
        {
            Rudra = 1,
            Ahi,
            Mitra,
            Pitri,
            Vasu,
            Ambu,
            Visvadeva,
            Abhijit,
            Vidhata,
            Puruhuta,
            Indragni,
            Nirriti,
            Varuna,
            Aryaman,
            Bhaga,
            Girisa,
            Ajapada,
            Ahirbudhnya,
            Pushan,
            Asvi,
            Yama,
            Agni,
            Vidhaatri,
            Chanda,
            Aditi,
            Jiiva,
            Vishnu,
            Arka,
            Tvashtri,
            Maruta
        }

        // This matches the sweph definitions for easy conversion
        public enum Weekday
        {
            Monday    = 0,
            Tuesday   = 1,
            Wednesday = 2,
            Thursday  = 3,
            Friday    = 4,
            Saturday  = 5,
            Sunday    = 6
        }

        /// <summary>
        ///     Normalize a number between bounds
        /// </summary>
        /// <param name="lower">The lower bound of normalization</param>
        /// <param name="upper">The upper bound of normalization</param>
        /// <param name="x">The value to be normalized</param>
        /// <returns>
        ///     The normalized value of x, where lower <= x <= upper </returns>
        public static int normalize_inc(int lower, int upper, int x)
        {
            var size = upper - lower + 1;
            while (x > upper)
            {
                x -= size;
            }

            while (x < lower)
            {
                x += size;
            }

            Trace.Assert(x >= lower && x <= upper, "Basics.normalize failed");
            return x;
        }

        /// <summary>
        ///     Normalize a number between bounds
        /// </summary>
        /// <param name="lower">The lower bound of normalization</param>
        /// <param name="upper">The upper bound of normalization</param>
        /// <param name="x">The value to be normalized</param>
        /// <returns>
        ///     The normalized value of x, where lower = x <= upper </returns>
        public static double normalize_exc(double lower, double upper, double x)
        {
            var size = upper - lower;
            while (x > upper)
            {
                x -= size;
            }

            while (x <= lower)
            {
                x += size;
            }

            Trace.Assert(x >= lower && x <= upper, "Basics.normalize failed");
            return x;
        }

        public static double normalize_exc_lower(double lower, double upper, double x)
        {
            var size = upper - lower;
            while (x >= upper)
            {
                x -= size;
            }

            while (x < lower)
            {
                x += size;
            }

            Trace.Assert(x >= lower && x <= upper, "Basics.normalize failed");
            return x;
        }

        public static ZodiacHouse getMoolaTrikonaRasi(Body.Body.Name b)
        {
            var z = ZodiacHouse.Name.Ari;
            switch (b)
            {
                case Body.Body.Name.Sun:
                    z = ZodiacHouse.Name.Leo;
                    break;
                case Body.Body.Name.Moon:
                    z = ZodiacHouse.Name.Tau;
                    break;
                case Body.Body.Name.Mars:
                    z = ZodiacHouse.Name.Ari;
                    break;
                case Body.Body.Name.Mercury:
                    z = ZodiacHouse.Name.Vir;
                    break;
                case Body.Body.Name.Jupiter:
                    z = ZodiacHouse.Name.Sag;
                    break;
                case Body.Body.Name.Venus:
                    z = ZodiacHouse.Name.Lib;
                    break;
                case Body.Body.Name.Saturn:
                    z = ZodiacHouse.Name.Aqu;
                    break;
                case Body.Body.Name.Rahu:
                    z = ZodiacHouse.Name.Vir;
                    break;
                case Body.Body.Name.Ketu:
                    z = ZodiacHouse.Name.Pis;
                    break;
            }

            return new ZodiacHouse(z);
        }

        public static Weekday bodyToWeekday(Body.Body.Name b)
        {
            switch (b)
            {
                case Body.Body.Name.Sun:     return Weekday.Sunday;
                case Body.Body.Name.Moon:    return Weekday.Monday;
                case Body.Body.Name.Mars:    return Weekday.Tuesday;
                case Body.Body.Name.Mercury: return Weekday.Wednesday;
                case Body.Body.Name.Jupiter: return Weekday.Thursday;
                case Body.Body.Name.Venus:   return Weekday.Friday;
                case Body.Body.Name.Saturn:  return Weekday.Saturday;
            }

            Debug.Assert(false, string.Format("bodyToWeekday({0})", b));
            throw new Exception();
        }

        public static Body.Body.Name weekdayRuler(Weekday w)
        {
            switch (w)
            {
                case Weekday.Sunday:    return Body.Body.Name.Sun;
                case Weekday.Monday:    return Body.Body.Name.Moon;
                case Weekday.Tuesday:   return Body.Body.Name.Mars;
                case Weekday.Wednesday: return Body.Body.Name.Mercury;
                case Weekday.Thursday:  return Body.Body.Name.Jupiter;
                case Weekday.Friday:    return Body.Body.Name.Venus;
                case Weekday.Saturday:  return Body.Body.Name.Saturn;
                default:
                    Debug.Assert(false, "Basics::weekdayRuler");
                    return Body.Body.Name.Sun;
            }
        }

        public static string weekdayToShortString(Weekday w)
        {
            switch (w)
            {
                case Weekday.Sunday:    return "Su";
                case Weekday.Monday:    return "Mo";
                case Weekday.Tuesday:   return "Tu";
                case Weekday.Wednesday: return "We";
                case Weekday.Thursday:  return "Th";
                case Weekday.Friday:    return "Fr";
                case Weekday.Saturday:  return "Sa";
            }

            return string.Empty;
        }

        public static Nakshatra28.Name NakLordOfMuhurta(Muhurta m)
        {
            switch (m)
            {
                case Muhurta.Rudra:       return Nakshatra28.Name.Aridra;
                case Muhurta.Ahi:         return Nakshatra28.Name.Aslesha;
                case Muhurta.Mitra:       return Nakshatra28.Name.Anuradha;
                case Muhurta.Pitri:       return Nakshatra28.Name.Makha;
                case Muhurta.Vasu:        return Nakshatra28.Name.Dhanishta;
                case Muhurta.Ambu:        return Nakshatra28.Name.PoorvaShada;
                case Muhurta.Visvadeva:   return Nakshatra28.Name.UttaraShada;
                case Muhurta.Abhijit:     return Nakshatra28.Name.Abhijit;
                case Muhurta.Vidhata:     return Nakshatra28.Name.Rohini;
                case Muhurta.Puruhuta:    return Nakshatra28.Name.Jyestha;
                case Muhurta.Indragni:    return Nakshatra28.Name.Vishaka;
                case Muhurta.Nirriti:     return Nakshatra28.Name.Moola;
                case Muhurta.Varuna:      return Nakshatra28.Name.Satabisha;
                case Muhurta.Aryaman:     return Nakshatra28.Name.UttaraPhalguni;
                case Muhurta.Bhaga:       return Nakshatra28.Name.PoorvaPhalguni;
                case Muhurta.Girisa:      return Nakshatra28.Name.Aridra;
                case Muhurta.Ajapada:     return Nakshatra28.Name.PoorvaBhadra;
                case Muhurta.Ahirbudhnya: return Nakshatra28.Name.UttaraBhadra;
                case Muhurta.Pushan:      return Nakshatra28.Name.Revati;
                case Muhurta.Asvi:        return Nakshatra28.Name.Aswini;
                case Muhurta.Yama:        return Nakshatra28.Name.Bharani;
                case Muhurta.Agni:        return Nakshatra28.Name.Krittika;
                case Muhurta.Vidhaatri:   return Nakshatra28.Name.Rohini;
                case Muhurta.Chanda:      return Nakshatra28.Name.Mrigarirsa;
                case Muhurta.Aditi:       return Nakshatra28.Name.Punarvasu;
                case Muhurta.Jiiva:       return Nakshatra28.Name.Pushya;
                case Muhurta.Vishnu:      return Nakshatra28.Name.Sravana;
                case Muhurta.Arka:        return Nakshatra28.Name.Hasta;
                case Muhurta.Tvashtri:    return Nakshatra28.Name.Chittra;
                case Muhurta.Maruta:      return Nakshatra28.Name.Swati;
            }

            Debug.Assert(false, string.Format("Basics::NakLordOfMuhurta Unknown Muhurta {0}", m));
            return Nakshatra28.Name.Aswini;
        }

        public static string variationNameOfDivision(Division d)
        {
            if (d.MultipleDivisions.Length > 1)
            {
                return "Composite";
            }

            switch (d.MultipleDivisions[0].Varga)
            {
                case DivisionType.Rasi:                    return "Rasi";
                case DivisionType.Navamsa:                 return "Navamsa";
                case DivisionType.HoraParasara:            return "Parasara";
                case DivisionType.HoraJagannath:           return "Jagannath";
                case DivisionType.HoraParivrittiDwaya:     return "Parivritti";
                case DivisionType.HoraKashinath:           return "Kashinath";
                case DivisionType.DrekkanaParasara:        return "Parasara";
                case DivisionType.DrekkanaJagannath:       return "Jagannath";
                case DivisionType.DrekkanaParivrittitraya: return "Parivritti";
                case DivisionType.DrekkanaSomnath:         return "Somnath";
                case DivisionType.Chaturthamsa:            return string.Empty;
                case DivisionType.Panchamsa:               return string.Empty;
                case DivisionType.Shashthamsa:             return string.Empty;
                case DivisionType.Saptamsa:                return string.Empty;
                case DivisionType.Ashtamsa:                return "Rath";
                case DivisionType.AshtamsaRaman:           return "Raman";
                case DivisionType.Dasamsa:                 return string.Empty;
                case DivisionType.Rudramsa:                return "Rath";
                case DivisionType.RudramsaRaman:           return "Raman";
                case DivisionType.Dwadasamsa:              return string.Empty;
                case DivisionType.Shodasamsa:              return string.Empty;
                case DivisionType.Vimsamsa:                return string.Empty;
                case DivisionType.Chaturvimsamsa:          return string.Empty;
                case DivisionType.Nakshatramsa:            return string.Empty;
                case DivisionType.Trimsamsa:               return string.Empty;
                case DivisionType.TrimsamsaParivritti:     return "Parivritti";
                case DivisionType.TrimsamsaSimple:         return "Simple";
                case DivisionType.Khavedamsa:              return string.Empty;
                case DivisionType.Akshavedamsa:            return string.Empty;
                case DivisionType.Shashtyamsa:             return string.Empty;
                case DivisionType.Ashtottaramsa:           return string.Empty;
                case DivisionType.Nadiamsa:                return "Equal Size";
                case DivisionType.NadiamsaCKN:             return "Chandra Kala";
                case DivisionType.NavamsaDwadasamsa:       return "Composite";
                case DivisionType.DwadasamsaDwadasamsa:    return "Composite";
                case DivisionType.BhavaPada:               return "9 Padas";
                case DivisionType.BhavaEqual:              return "Equal Houses";
                case DivisionType.BhavaAlcabitus:          return "Alcabitus";
                case DivisionType.BhavaAxial:              return "Axial";
                case DivisionType.BhavaCampanus:           return "Campanus";
                case DivisionType.BhavaKoch:               return "Koch";
                case DivisionType.BhavaPlacidus:           return "Placidus";
                case DivisionType.BhavaRegiomontanus:      return "Regiomontanus";
                case DivisionType.BhavaSripati:            return "Sripati";
                case DivisionType.GenericParivritti:       return "Parivritti";
                case DivisionType.GenericShashthamsa:      return "Regular: Shashtamsa";
                case DivisionType.GenericSaptamsa:         return "Regular: Saptamsa";
                case DivisionType.GenericDasamsa:          return "Regular: Dasamsa";
                case DivisionType.GenericDwadasamsa:       return "Regular: Dwadasamsa";
                case DivisionType.GenericChaturvimsamsa:   return "Regular: Chaturvimsamsa";
                case DivisionType.GenericChaturthamsa:     return "Kendra: Chaturtamsa";
                case DivisionType.GenericNakshatramsa:     return "Kendra: Nakshatramsa";
                case DivisionType.GenericDrekkana:         return "Trikona: Drekkana";
                case DivisionType.GenericShodasamsa:       return "Trikona: Shodasamsa";
                case DivisionType.GenericVimsamsa:         return "Trikona: Vimsamsa";
            }

            Debug.Assert(false, string.Format("Basics::numPartsInBasics.DivisionType. Unknown Division {0}", d.MultipleDivisions[0].Varga));
            return string.Empty;
        }

        public static string numPartsInDivisionString(Division div)
        {
            var sRet = "D";
            foreach (var dSingle in div.MultipleDivisions)
            {
                sRet = string.Format("{0}-{1}", sRet, numPartsInDivision(dSingle));
            }

            return sRet;
        }

        public static int numPartsInDivision(Division div)
        {
            var parts = 1;
            foreach (var dSingle in div.MultipleDivisions)
            {
                parts *= numPartsInDivision(dSingle);
            }

            return parts;
        }

        public static int numPartsInDivision(Division.SingleDivision dSingle)
        {
            switch (dSingle.Varga)
            {
                case DivisionType.Rasi:    return 1;
                case DivisionType.Navamsa: return 9;
                case DivisionType.HoraParasara:
                case DivisionType.HoraJagannath:
                case DivisionType.HoraParivrittiDwaya:
                case DivisionType.HoraKashinath: return 2;
                case DivisionType.DrekkanaParasara:
                case DivisionType.DrekkanaJagannath:
                case DivisionType.DrekkanaParivrittitraya:
                case DivisionType.DrekkanaSomnath: return 3;
                case DivisionType.Chaturthamsa: return 4;
                case DivisionType.Panchamsa:    return 5;
                case DivisionType.Shashthamsa:  return 6;
                case DivisionType.Saptamsa:     return 7;
                case DivisionType.Ashtamsa:
                case DivisionType.AshtamsaRaman: return 8;
                case DivisionType.Dasamsa: return 10;
                case DivisionType.Rudramsa:
                case DivisionType.RudramsaRaman: return 11;
                case DivisionType.Dwadasamsa:     return 12;
                case DivisionType.Shodasamsa:     return 16;
                case DivisionType.Vimsamsa:       return 20;
                case DivisionType.Chaturvimsamsa: return 24;
                case DivisionType.Nakshatramsa:   return 27;
                case DivisionType.Trimsamsa:
                case DivisionType.TrimsamsaParivritti:
                case DivisionType.TrimsamsaSimple: return 30;
                case DivisionType.Khavedamsa:    return 40;
                case DivisionType.Akshavedamsa:  return 45;
                case DivisionType.Shashtyamsa:   return 60;
                case DivisionType.Ashtottaramsa: return 108;
                case DivisionType.Nadiamsa:
                case DivisionType.NadiamsaCKN: return 150;
                case DivisionType.NavamsaDwadasamsa:    return 108;
                case DivisionType.DwadasamsaDwadasamsa: return 144;
                case DivisionType.BhavaPada:
                case DivisionType.BhavaEqual:
                case DivisionType.BhavaAlcabitus:
                case DivisionType.BhavaAxial:
                case DivisionType.BhavaCampanus:
                case DivisionType.BhavaKoch:
                case DivisionType.BhavaPlacidus:
                case DivisionType.BhavaRegiomontanus:
                case DivisionType.BhavaSripati: return 1;
                default: return dSingle.NumParts;
            }
        }

        public static Division[] Shadvargas()
        {
            return new[]
            {
                new Division(DivisionType.Rasi),
                new Division(DivisionType.HoraParasara),
                new Division(DivisionType.DrekkanaParasara),
                new Division(DivisionType.Navamsa),
                new Division(DivisionType.Dwadasamsa),
                new Division(DivisionType.Trimsamsa)
            };
        }

        public static Division[] Saptavargas()
        {
            return new[]
            {
                new Division(DivisionType.Rasi),
                new Division(DivisionType.HoraParasara),
                new Division(DivisionType.DrekkanaParasara),
                new Division(DivisionType.Saptamsa),
                new Division(DivisionType.Navamsa),
                new Division(DivisionType.Dwadasamsa),
                new Division(DivisionType.Trimsamsa)
            };
        }

        public static Division[] Dasavargas()
        {
            return new[]
            {
                new Division(DivisionType.Rasi),
                new Division(DivisionType.HoraParasara),
                new Division(DivisionType.DrekkanaParasara),
                new Division(DivisionType.Saptamsa),
                new Division(DivisionType.Navamsa),
                new Division(DivisionType.Dasamsa),
                new Division(DivisionType.Dwadasamsa),
                new Division(DivisionType.Shodasamsa),
                new Division(DivisionType.Trimsamsa),
                new Division(DivisionType.Shashtyamsa)
            };
        }

        public static Division[] Shodasavargas()
        {
            return new[]
            {
                new Division(DivisionType.Rasi),
                new Division(DivisionType.HoraParasara),
                new Division(DivisionType.DrekkanaParasara),
                new Division(DivisionType.Chaturthamsa),
                new Division(DivisionType.Saptamsa),
                new Division(DivisionType.Navamsa),
                new Division(DivisionType.Dasamsa),
                new Division(DivisionType.Dwadasamsa),
                new Division(DivisionType.Shodasamsa),
                new Division(DivisionType.Vimsamsa),
                new Division(DivisionType.Chaturvimsamsa),
                new Division(DivisionType.Nakshatramsa),
                new Division(DivisionType.Trimsamsa),
                new Division(DivisionType.Khavedamsa),
                new Division(DivisionType.Akshavedamsa),
                new Division(DivisionType.Shashtyamsa)
            };
        }

        /// <summary>
        ///     Specify the Lord of a ZodiacHouse. The owernership of the nodes is not considered
        /// </summary>
        /// <param name="zh">The House whose lord should be returned</param>
        /// <returns>The lord of zh</returns>
        public static Body.Body.Name SimpleLordOfZodiacHouse(ZodiacHouse.Name zh)
        {
            switch (zh)
            {
                case ZodiacHouse.Name.Ari: return Body.Body.Name.Mars;
                case ZodiacHouse.Name.Tau: return Body.Body.Name.Venus;
                case ZodiacHouse.Name.Gem: return Body.Body.Name.Mercury;
                case ZodiacHouse.Name.Can: return Body.Body.Name.Moon;
                case ZodiacHouse.Name.Leo: return Body.Body.Name.Sun;
                case ZodiacHouse.Name.Vir: return Body.Body.Name.Mercury;
                case ZodiacHouse.Name.Lib: return Body.Body.Name.Venus;
                case ZodiacHouse.Name.Sco: return Body.Body.Name.Mars;
                case ZodiacHouse.Name.Sag: return Body.Body.Name.Jupiter;
                case ZodiacHouse.Name.Cap: return Body.Body.Name.Saturn;
                case ZodiacHouse.Name.Aqu: return Body.Body.Name.Saturn;
                case ZodiacHouse.Name.Pis: return Body.Body.Name.Jupiter;
            }

            Trace.Assert(false,
                         string.Format("Basics.SimpleLordOfZodiacHouse for {0} failed", (int)zh));
            return Body.Body.Name.Other;
        }


        public static Longitude CalculateBodyLongitude(double ut, int ipl)
        {
            var xx = new double[6]
            {
                0,
                0,
                0,
                0,
                0,
                0
            };
            try
            {
                sweph.CalcUT(ut, ipl, 0, xx);
                return new Longitude(xx[0]);
            }
            catch (SwephException exc)
            {
                mhora.Log.Debug("Sweph: {0}\n", exc.status);
                throw new Exception(string.Empty);
            }
        }

        /// <summary>
        ///     Calculated a BodyPosition for a given time and place using the swiss ephemeris
        /// </summary>
        /// <param name="ut">The time for which the calculations should be performed</param>
        /// <param name="ipl">The Swiss Ephemeris body Name</param>
        /// <param name="body">The local application body name</param>
        /// <param name="type">The local application body type</param>
        /// <returns>A BodyPosition class</returns>
        public static Position CalculateSingleBodyPosition(double ut, int ipl, Body.Body.Name body, BodyType.Name type, Horoscope h)
        {
            if (body == Body.Body.Name.Lagna)
            {
                var b = new Position(h, body, type, new Longitude(sweph.Lagna(ut)), 0, 0, 0, 0, 0);
                return b;
            }

            var xx = new double[6]
            {
                0,
                0,
                0,
                0,
                0,
                0
            };
            try
            {
                sweph.CalcUT(ut, ipl, 0, xx);

                var b = new Position(h,
                                         body,
                                         type,
                                         new Longitude(xx[0]),
                                         xx[1],
                                         xx[2],
                                         xx[3],
                                         xx[4],
                                         xx[5]);
                return b;
            }
            catch (SwephException exc)
            {
                mhora.Log.Debug("Sweph: {0}\n", exc.status);
                throw new Exception(string.Empty);
            }
        }


        /// <summary>
        ///     Given a HoraInfo object (all required user inputs), calculate a list of
        ///     all bodypositions we will ever require
        /// </summary>
        /// <param name="h">The HoraInfo object</param>
        /// <returns></returns>
        public static ArrayList CalculateBodyPositions(Horoscope h, double sunrise)
        {
            var hi = h.info;
            var o  = h.options;

            var serr      = new StringBuilder(256);
            var ephe_path = MhoraGlobalOptions.Instance.HOptions.EphemerisPath;

            // The order of the array must reflect the order define in Basics.GrahaIndex
            var std_grahas = new ArrayList(20);

            sweph.SetPath(ephe_path);
            var julday_ut = sweph.JulDay(hi.tob.year,
                                             hi.tob.month,
                                             hi.tob.day,
                                             hi.tob.time - hi.tz.toDouble());
            //	h.tob.hour + (((double)h.tob.minute) / 60.0) + (((double)h.tob.second) / 3600.0));
            //	(h.tob.time / 24.0) + (h.tz.toDouble()/24.0));
            //(h.tob.hour/24.0) + (((double)h.tob.minute) / 60.0) + (((double)h.tob.second) / 3600.0));
            //julday_ut = julday_ut - (h.tz.toDouble() / 24.0);

            var swephRahuBody = sweph.SE_MEAN_NODE;
            if (o.nodeType == HoroscopeOptions.ENodeType.True)
            {
                swephRahuBody = sweph.SE_TRUE_NODE;
            }

            var addFlags = 0;
            if (o.grahaPositionType == HoroscopeOptions.EGrahaPositionType.True)
            {
                addFlags = sweph.SEFLG_TRUEPOS;
            }

            std_grahas.Add(CalculateSingleBodyPosition(julday_ut, sweph.SE_SUN, Body.Body.Name.Sun, BodyType.Name.Graha, h));
            std_grahas.Add(CalculateSingleBodyPosition(julday_ut, sweph.SE_MOON, Body.Body.Name.Moon, BodyType.Name.Graha, h));
            std_grahas.Add(CalculateSingleBodyPosition(julday_ut, sweph.SE_MARS, Body.Body.Name.Mars, BodyType.Name.Graha, h));
            std_grahas.Add(CalculateSingleBodyPosition(julday_ut, sweph.SE_MERCURY, Body.Body.Name.Mercury, BodyType.Name.Graha, h));
            std_grahas.Add(CalculateSingleBodyPosition(julday_ut, sweph.SE_JUPITER, Body.Body.Name.Jupiter, BodyType.Name.Graha, h));
            std_grahas.Add(CalculateSingleBodyPosition(julday_ut, sweph.SE_VENUS, Body.Body.Name.Venus, BodyType.Name.Graha, h));
            std_grahas.Add(CalculateSingleBodyPosition(julday_ut, sweph.SE_SATURN, Body.Body.Name.Saturn, BodyType.Name.Graha, h));
            var rahu = CalculateSingleBodyPosition(julday_ut, swephRahuBody, Body.Body.Name.Rahu, BodyType.Name.Graha, h);

            var ketu = CalculateSingleBodyPosition(julday_ut, swephRahuBody, Body.Body.Name.Ketu, BodyType.Name.Graha, h);
            ketu.longitude = rahu.longitude.add(new Longitude(180.0));
            std_grahas.Add(rahu);
            std_grahas.Add(ketu);

            var asc = sweph.Lagna(julday_ut);
            std_grahas.Add(new Position(h, Body.Body.Name.Lagna, BodyType.Name.Lagna, new Longitude(asc), 0, 0, 0, 0, 0));

            var ista_ghati = normalize_exc(0.0, 24.0, hi.tob.time - sunrise) * 2.5;
            var gl_lon     = ((Position)std_grahas[0]).longitude.add(new Longitude(ista_ghati        * 30.0));
            var hl_lon     = ((Position)std_grahas[0]).longitude.add(new Longitude(ista_ghati * 30.0 / 2.5));
            var bl_lon     = ((Position)std_grahas[0]).longitude.add(new Longitude(ista_ghati * 30.0 / 5.0));

            var vl = ista_ghati * 5.0;
            while (ista_ghati > 12.0)
            {
                ista_ghati -= 12.0;
            }

            var vl_lon = ((Position)std_grahas[0]).longitude.add(new Longitude(vl * 30.0));

            std_grahas.Add(new Position(h, Body.Body.Name.BhavaLagna, BodyType.Name.SpecialLagna, bl_lon, 0, 0, 0, 0, 0));
            std_grahas.Add(new Position(h, Body.Body.Name.HoraLagna, BodyType.Name.SpecialLagna, hl_lon, 0, 0, 0, 0, 0));
            std_grahas.Add(new Position(h, Body.Body.Name.GhatiLagna, BodyType.Name.SpecialLagna, gl_lon, 0, 0, 0, 0, 0));
            std_grahas.Add(new Position(h, Body.Body.Name.VighatiLagna, BodyType.Name.SpecialLagna, vl_lon, 0, 0, 0, 0, 0));


            return std_grahas;
        }
    }
}