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
    public class LagnaKendradiRasiDasa : Dasa, IDasa
    {
        private readonly Horoscope           h;
        private readonly Division            m_dtype = new Division(Basics.DivisionType.Rasi);
        private readonly RasiDasaUserOptions options;

        public LagnaKendradiRasiDasa(Horoscope _h)
        {
            var fs_rasi = new FindStronger(h, m_dtype, FindStronger.RulesNarayanaDasaRasi(h));
            h       = _h;
            options = new RasiDasaUserOptions(h, FindStronger.RulesNarayanaDasaRasi(h));
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
            var al = new ArrayList(24);
            int[] order =
            {
                1,
                4,
                7,
                10,
                2,
                5,
                8,
                11,
                3,
                6,
                9,
                12
            };
            double dasa_length_sum = 0;

            var zh_start = options.getSeed();
            zh_start.value = options.findStrongerRasi(options.SeventhStrengths, zh_start.value, zh_start.add(7).value);

            var bIsZodiacal = isZodiacal();
            for (var i = 0; i < 12; i++)
            {
                var zh = zh_start.add(1);
                if (bIsZodiacal)
                {
                    zh = zh.add(order[i]);
                }
                else
                {
                    zh = zh.addReverse(order[i]);
                }

                var    lord        = h.LordOfZodiacHouse(zh, m_dtype);
                var    dp_lord     = h.getPosition(lord).toDivisionPosition(m_dtype);
                double dasa_length = NarayanaDasaLength(zh, dp_lord);
                var    de          = new DasaEntry(zh.value, dasa_length_sum, dasa_length, 1, zh.value.ToString());
                al.Add(de);
                dasa_length_sum += dasa_length;
            }

            for (var i = 0; i < 12; i++)
            {
                var de_first    = (DasaEntry)al[i];
                var dasa_length = 12.0 - de_first.dasaLength;
                var de          = new DasaEntry(de_first.zodiacHouse, dasa_length_sum, dasa_length, 1, de_first.shortDesc);
                dasa_length_sum += dasa_length;
                al.Add(de);
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
            return "Lagna Kendradi Rasi Dasa seeded from" + " seeded from " + options.SeedRasi;
        }

        public object GetOptions()
        {
            return options.Clone();
        }

        public object SetOptions(object a)
        {
            options.CopyFrom(a);
            RecalculateEvent();
            return options.Clone();
        }

        public new void DivisionChanged(Division div)
        {
            var newOpts = (RasiDasaUserOptions)options.Clone();
            newOpts.Division = (Division)div.Clone();
            SetOptions(newOpts);
        }

        private bool isZodiacal()
        {
            var zh_start = options.getSeed();
            zh_start.value = options.findStrongerRasi(options.SeventhStrengths, zh_start.value, zh_start.add(7).value);

            var forward = zh_start.isOdd();
            if (options.saturnExceptionApplies(zh_start.value))
            {
                return forward;
            }

            if (options.ketuExceptionApplies(zh_start.value))
            {
                forward = !forward;
            }

            return forward;
        }
    }
}