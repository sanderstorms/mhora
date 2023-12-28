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

namespace Mhora.Components.Property;

public class PropertyOrderPair : IComparable
{
    private readonly int _order;

    public PropertyOrderPair(PropertyDescriptor pdesc, string name, int order)
    {
        Property = pdesc;
        _order   = order;
        Name     = name;
    }

    public string Name
    {
        get;
    }

    public PropertyDescriptor Property
    {
        get;
    }

    public int CompareTo(object obj)
    {
        //
        // Sort the pair objects by ordering by order value
        // Equal values get the same rank
        //
        var otherOrder = ((PropertyOrderPair) obj)._order;
        if (otherOrder == _order)
        {
            //
            // If order not specified, sort by name
            //
            var otherName = ((PropertyOrderPair) obj).Name;
            return string.Compare(Name, otherName);
        }

        if (otherOrder > _order)
        {
            return -1;
        }

        return 1;
    }
}