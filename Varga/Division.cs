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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using Mhora.Calculation;
using Mhora.Components.Property;

namespace Mhora.Varga;

[Serializable]
[TypeConverter(typeof(DivisionConverter))]
public class Division : ICloneable
{
    private SingleDivision[] mMultipleDivisions;

    public Division(Basics.DivisionType _dtype)
    {
        mMultipleDivisions = new[]
        {
            new SingleDivision(_dtype)
        };
    }

    public Division(SingleDivision single)
    {
        mMultipleDivisions = new[]
        {
            single
        };
    }

    public Division()
    {
        mMultipleDivisions = new[]
        {
            new SingleDivision(Basics.DivisionType.Rasi)
        };
    }

    [PGDisplayName("Composite Division")]
    public SingleDivision[] MultipleDivisions
    {
        get =>
            mMultipleDivisions;
        set =>
            mMultipleDivisions = value;
    }

    public object Clone()
    {
        var dRet = new Division();
        var al   = new ArrayList();
        foreach (var dSingle in MultipleDivisions)
        {
            al.Add(dSingle.Clone());
        }

        dRet.MultipleDivisions = (SingleDivision[]) al.ToArray(typeof(SingleDivision));
        return dRet;
    }

    public override string ToString()
    {
        return Basics.numPartsInDivisionString(this);
    }

    public override bool Equals(object obj)
    {
        if (obj is Division)
        {
            return this == (Division) obj;
        }

        return base.Equals(obj);
    }

    public static bool operator !=(Division d1, Division d2)
    {
        return !(d1 == d2);
    }

    public static bool operator ==(Division d1, Division d2)
    {
        if ((object) d1 == null && (object) d2 == null)
        {
            return true;
        }

        if ((object) d1 == null || (object) d2 == null)
        {
            return false;
        }

        if (d1.MultipleDivisions.Length != d2.MultipleDivisions.Length)
        {
            return false;
        }

        for (var i = 0; i < d1.MultipleDivisions.Length; i++)
        {
            if (d1.MultipleDivisions[i].Varga != d2.MultipleDivisions[i].Varga || d1.MultipleDivisions[i].NumParts != d2.MultipleDivisions[i].NumParts)
            {
                return false;
            }
        }

        return true;
    }

    public static void CopyToClipboard(Division div)
    {
        var mStr      = new MemoryStream();
        var bStr      = new BinaryWriter(mStr);
        var formatter = new BinaryFormatter();
        formatter.Serialize(mStr, div);
        Clipboard.SetDataObject(mStr, false);
    }

    public static Division CopyFromClipboard()
    {
        try
        {
            var mStr      = (MemoryStream) Clipboard.GetDataObject().GetData(typeof(MemoryStream));
            var bStr      = new BinaryReader(mStr);
            var formatter = new BinaryFormatter();
            var div       = (Division) formatter.Deserialize(bStr.BaseStream);
            return div;
        }
        catch
        {
            return null;
        }
    }

    [Serializable]
    [TypeConverter(typeof(SingleDivisionConverter))]
    public class SingleDivision : ICloneable
    {
        private Basics.DivisionType mDtype;
        private int                 mNumParts;

        public SingleDivision(Basics.DivisionType _dtype, int _numParts)
        {
            mDtype    = _dtype;
            mNumParts = _numParts;
        }

        public SingleDivision(Basics.DivisionType _dtype)
        {
            mDtype = _dtype;
        }

        public SingleDivision()
        {
            mDtype    = Basics.DivisionType.Rasi;
            mNumParts = 1;
        }

        public Basics.DivisionType Varga
        {
            get =>
                mDtype;
            set =>
                mDtype = value;
        }

        public int NumParts
        {
            get =>
                mNumParts;
            set =>
                mNumParts = value;
        }

        public object Clone()
        {
            return new SingleDivision(Varga, NumParts);
        }

        public override string ToString()
        {
            return mDtype + " " + mNumParts;
        }
    }
}