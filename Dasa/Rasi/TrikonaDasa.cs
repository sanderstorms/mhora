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

using System.Collections;
using Mhora.Calculation;
using Mhora.Settings;
using Mhora.Varga;

namespace Mhora
{
    public class TrikonaDasa : Dasa, IDasa
    {
        private readonly Horoscope   h;
        private readonly UserOptions options;

        private readonly int[] order =
        {
            1,
            5,
            9,
            2,
            6,
            10,
            3,
            7,
            11,
            4,
            8,
            12
        };

        public TrikonaDasa(Horoscope _h)
        {
            h       = _h;
            options = new UserOptions(h, FindStronger.RulesNavamsaDasaRasi(h));
        }

        public void recalculateOptions()
        {
            options.recalculate();
        }

        public double paramAyus()
        {
            return 144;
        }

        public ArrayList Dasa(int cycle)
        {
            var al      = new ArrayList(12);
            var zh_seed = options.getSeed();
            if (options.TrikonaStrengths.houses.Count >= 1)
            {
                zh_seed.value = (ZodiacHouse.Name)options.TrikonaStrengths.houses[0];
            }

            zh_seed.value = options.findStrongerRasi(options.SeventhStrengths, zh_seed.value, zh_seed.add(7).value);

            var bIsZodiacal = zh_seed.isOdd();

            var dasa_length_sum = 0.0;
            for (var i = 0; i < 12; i++)
            {
                ZodiacHouse zh_dasa = null;
                if (bIsZodiacal)
                {
                    zh_dasa = zh_seed.add(order[i]);
                }
                else
                {
                    zh_dasa = zh_seed.addReverse(order[i]);
                }

                double dasa_length = NarayanaDasaLength(zh_dasa, getLordsPosition(zh_dasa));


                var di = new DasaEntry(zh_dasa.value, dasa_length_sum, dasa_length, 1, zh_dasa.value.ToString());
                al.Add(di);
                dasa_length_sum += dasa_length;
            }

            for (var i = 0; i < 12; i++)
            {
                var df          = (DasaEntry)al[i];
                var dasa_length = 12.0 - df.dasaLength;
                var di          = new DasaEntry(df.zodiacHouse, dasa_length_sum, dasa_length, 1, df.shortDesc);
                al.Add(di);
                dasa_length_sum += dasa_length;
            }


            var cycle_length = cycle * paramAyus();
            foreach (DasaEntry di in al)
            {
                di.startUT += cycle_length;
            }

            return al;
        }

        public ArrayList AntarDasa(DasaEntry pdi)
        {
            var nd = new NarayanaDasa(h);
            nd.options = options;
            return nd.AntarDasa(pdi);
        }

        public string Description()
        {
            return "Trikona Dasa seeded from " + options.SeedRasi;
        }

        public object GetOptions()
        {
            return options.Clone();
        }

        public object SetOptions(object a)
        {
            var uo = (UserOptions)a;
            options.CopyFrom(uo);
            RecalculateEvent();
            return options.Clone();
        }

        public new void DivisionChanged(Division div)
        {
            var newOpts = (UserOptions)options.Clone();
            newOpts.Division = (Division)div.Clone();
            SetOptions(newOpts);
        }

        public DivisionPosition getLordsPosition(ZodiacHouse zh)
        {
            Body.Body.Name b;
            if (zh.value == ZodiacHouse.Name.Sco)
            {
                b = options.ColordSco;
            }
            else if (zh.value == ZodiacHouse.Name.Aqu)
            {
                b = options.ColordAqu;
            }
            else
            {
                b = Basics.SimpleLordOfZodiacHouse(zh.value);
            }

            return h.getPosition(b).toDivisionPosition(options.Division);
        }

        private class UserOptions : RasiDasaUserOptions
        {
            protected OrderedZodiacHouses mTrikonaStrengths;

            public UserOptions(Horoscope _h, ArrayList _rules)
                :
                base(_h, _rules)
            {
                calculateTrikonaStrengths();
            }

            public OrderedZodiacHouses TrikonaStrengths
            {
                get =>
                    mTrikonaStrengths;
                set =>
                    mTrikonaStrengths = value;
            }

            private void calculateTrikonaStrengths()
            {
                var zh = getSeed();
                var zh_t = new ZodiacHouse.Name[3]
                {
                    zh.add(1).value,
                    zh.add(5).value,
                    zh.add(9).value
                };
                var fs = new FindStronger(h, Division, mRules);
                mTrikonaStrengths = fs.getOrderedHouses(zh_t);
            }

            public override object Clone()
            {
                var uo = new UserOptions(h, mRules);
                CopyFromNoClone(this);
                uo.mTrikonaStrengths = (OrderedZodiacHouses)mTrikonaStrengths.Clone();
                return uo;
            }

            public override object CopyFrom(object _uo)
            {
                var uo = (UserOptions)_uo;
                if (Division  != uo.Division  ||
                    ColordAqu != uo.ColordAqu ||
                    ColordSco != uo.ColordSco)
                {
                    calculateTrikonaStrengths();
                    calculateSeed();
                    calculateExceptions();
                    calculateSeventhStrengths();
                    calculateCoLords();
                }

                base.CopyFromNoClone(_uo);
                return Clone();
            }

            public new void recalculate()
            {
                calculateTrikonaStrengths();
                calculateSeed();
                calculateExceptions();
                calculateSeventhStrengths();
                calculateCoLords();
            }
        }
    }
}