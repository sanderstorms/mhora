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
using System.Globalization;

namespace Mhora.Calculation
{
    internal class OrderedZodiacHousesConverter : ExpandableObjectConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type t)
        {
            if (t == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, t);
        }

        public override object ConvertFrom(
            ITypeDescriptorContext context,
            CultureInfo            info,
            object                 value)
        {
            Trace.Assert(value is string, "OrderedZodiacHousesConverter::ConvertFrom 1");
            var s = (string)value;

            var oz  = new OrderedZodiacHouses();
            var al  = new ArrayList();
            var arr = s.Split('.', ' ', ':', ',');
            foreach (var szh_mixed in arr)
            {
                var szh = szh_mixed.ToLower();
                switch (szh)
                {
                    case "ari":
                        al.Add(ZodiacHouse.Name.Ari);
                        break;
                    case "tau":
                        al.Add(ZodiacHouse.Name.Tau);
                        break;
                    case "gem":
                        al.Add(ZodiacHouse.Name.Gem);
                        break;
                    case "can":
                        al.Add(ZodiacHouse.Name.Can);
                        break;
                    case "leo":
                        al.Add(ZodiacHouse.Name.Leo);
                        break;
                    case "vir":
                        al.Add(ZodiacHouse.Name.Vir);
                        break;
                    case "lib":
                        al.Add(ZodiacHouse.Name.Lib);
                        break;
                    case "sco":
                        al.Add(ZodiacHouse.Name.Sco);
                        break;
                    case "sag":
                        al.Add(ZodiacHouse.Name.Sag);
                        break;
                    case "cap":
                        al.Add(ZodiacHouse.Name.Cap);
                        break;
                    case "aqu":
                        al.Add(ZodiacHouse.Name.Aqu);
                        break;
                    case "pis":
                        al.Add(ZodiacHouse.Name.Pis);
                        break;
                }
            }

            oz.houses = (ArrayList)al.Clone();
            return oz;
        }

        public override object ConvertTo(
            ITypeDescriptorContext context,
            CultureInfo            culture,
            object                 value,
            Type                   destType)
        {
            Trace.Assert(destType == typeof(string) && value is OrderedZodiacHouses, "HMSInfo::ConvertTo 1");
            var oz = (OrderedZodiacHouses)value;
            return oz.ToString();
        }
    }
}