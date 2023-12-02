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

using System.ComponentModel;
using System.Diagnostics;
using Mhora.Util;

namespace Mhora.Calculation
{
    public class Tithi
    {
        [TypeConverter(typeof(EnumDescConverter))]
        public enum Name
        {
            [Description("Shukla Pratipada")]
            ShuklaPratipada = 1,

            [Description("Shukla Dvitiya")]
            ShuklaDvitiya,

            [Description("Shukla Tritiya")]
            ShuklaTritiya,

            [Description("Shukla Chaturti")]
            ShuklaChaturti,

            [Description("Shukla Panchami")]
            ShuklaPanchami,

            [Description("Shukla Shashti")]
            ShuklaShashti,

            [Description("Shukla Saptami")]
            ShuklaSaptami,

            [Description("Shukla Ashtami")]
            ShuklaAshtami,

            [Description("Shukla Navami")]
            ShuklaNavami,

            [Description("Shukla Dashami")]
            ShuklaDasami,

            [Description("Shukla Ekadasi")]
            ShuklaEkadasi,

            [Description("Shukla Dwadasi")]
            ShuklaDvadasi,

            [Description("Shukla Trayodasi")]
            ShuklaTrayodasi,

            [Description("Shukla Chaturdasi")]
            ShuklaChaturdasi,

            [Description("Paurnami")]
            Paurnami,

            [Description("Krishna Pratipada")]
            KrishnaPratipada,

            [Description("Krishna Dvitiya")]
            KrishnaDvitiya,

            [Description("Krishna Tritiya")]
            KrishnaTritiya,

            [Description("Krishna Chaturti")]
            KrishnaChaturti,

            [Description("Krishna Panchami")]
            KrishnaPanchami,

            [Description("Krishna Shashti")]
            KrishnaShashti,

            [Description("Krishna Saptami")]
            KrishnaSaptami,

            [Description("Krishna Ashtami")]
            KrishnaAshtami,

            [Description("Krishna Navami")]
            KrishnaNavami,

            [Description("Krishna Dashami")]
            KrishnaDasami,

            [Description("Krishna Ekadasi")]
            KrishnaEkadasi,

            [Description("Krishna Dwadasi")]
            KrishnaDvadasi,

            [Description("Krishna Trayodasi")]
            KrishnaTrayodasi,

            [Description("Krishna Chaturdasi")]
            KrishnaChaturdasi,

            [Description("Amavasya")]
            Amavasya
        }

        public enum NandaType
        {
            Nanda,
            Bhadra,
            Jaya,
            Rikta,
            Purna
        }


        public Tithi(Name _mValue)
        {
            value = (Name)Basics.normalize_inc(1, 30, (int)_mValue);
        }

        public Name value
        {
            get;
            set;
        }

        public string ToUnqualifiedString()
        {
            switch (value)
            {
                case Name.KrishnaPratipada:
                case Name.ShuklaPratipada: return "Prathama";
                case Name.KrishnaDvitiya:
                case Name.ShuklaDvitiya: return "Dvitiya";
                case Name.KrishnaTritiya:
                case Name.ShuklaTritiya: return "Tritiya";
                case Name.KrishnaChaturti:
                case Name.ShuklaChaturti: return "Chaturthi";
                case Name.KrishnaPanchami:
                case Name.ShuklaPanchami: return "Panchami";
                case Name.KrishnaShashti:
                case Name.ShuklaShashti: return "Shashti";
                case Name.KrishnaSaptami:
                case Name.ShuklaSaptami: return "Saptami";
                case Name.KrishnaAshtami:
                case Name.ShuklaAshtami: return "Ashtami";
                case Name.KrishnaNavami:
                case Name.ShuklaNavami: return "Navami";
                case Name.KrishnaDasami:
                case Name.ShuklaDasami: return "Dashami";
                case Name.KrishnaEkadasi:
                case Name.ShuklaEkadasi: return "Ekadashi";
                case Name.KrishnaDvadasi:
                case Name.ShuklaDvadasi: return "Dwadashi";
                case Name.KrishnaTrayodasi:
                case Name.ShuklaTrayodasi: return "Trayodashi";
                case Name.KrishnaChaturdasi:
                case Name.ShuklaChaturdasi: return "Chaturdashi";
                case Name.Paurnami: return "Poornima";
                case Name.Amavasya: return "Amavasya";
            }

            return string.Empty;
        }

        public override string ToString()
        {
            return EnumDescConverter.GetEnumDescription(value);
        }

        public Tithi add(int i)
        {
            var tnum = Basics.normalize_inc(1, 30, (int)value + i - 1);
            return new Tithi((Name)tnum);
        }

        public Tithi addReverse(int i)
        {
            var tnum = Basics.normalize_inc(1, 30, (int)value - i + 1);
            return new Tithi((Name)tnum);
        }

        public Body.Body.Name getLord()
        {
            // 1 based index starting with prathama
            var t = (int)value;

            //mhora.Log.Debug ("Looking for lord of tithi {0}", t);
            // check for new moon and full moon 
            if (t == 30)
            {
                return Body.Body.Name.Rahu;
            }

            if (t == 15)
            {
                return Body.Body.Name.Saturn;
            }

            // coalesce pakshas
            if (t >= 16)
            {
                t -= 15;
            }

            switch (t)
            {
                case 1:
                case 9: return Body.Body.Name.Sun;
                case 2:
                case 10: return Body.Body.Name.Moon;
                case 3:
                case 11: return Body.Body.Name.Mars;
                case 4:
                case 12: return Body.Body.Name.Mercury;
                case 5:
                case 13: return Body.Body.Name.Jupiter;
                case 6:
                case 14: return Body.Body.Name.Venus;
                case 7: return Body.Body.Name.Saturn;
                case 8: return Body.Body.Name.Rahu;
            }

            Debug.Assert(false, "Tithi::getLord");
            return Body.Body.Name.Sun;
        }

        public NandaType toNandaType()
        {
            // 1 based index starting with prathama
            var t = (int)value;

            // check for new moon and full moon 

            if (t == 30 || t == 15)
            {
                return NandaType.Purna;
            }

            // coalesce pakshas
            if (t >= 16)
            {
                t -= 15;
            }

            switch (t)
            {
                case 1:
                case 6:
                case 11: return NandaType.Nanda;
                case 2:
                case 7:
                case 12: return NandaType.Bhadra;
                case 3:
                case 8:
                case 13: return NandaType.Jaya;
                case 4:
                case 9:
                case 14: return NandaType.Rikta;
                case 5:
                case 10: return NandaType.Purna;
            }

            Debug.Assert(false, "Tithi::toNandaType");
            return NandaType.Nanda;
        }
    }
}