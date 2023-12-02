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

namespace Mhora.Components.Property
{
    public class GlobalizedPropertyDescriptor : PropertyDescriptor
    {
        private readonly PropertyDescriptor basePropertyDescriptor;

        public GlobalizedPropertyDescriptor(PropertyDescriptor basePropertyDescriptor)
            : base(basePropertyDescriptor)
        {
            this.basePropertyDescriptor = basePropertyDescriptor;
        }

        public override Type ComponentType => basePropertyDescriptor.ComponentType;

        public override string DisplayName
        {
            get
            {
                var dn = (PGDisplayName)basePropertyDescriptor.Attributes[typeof(PGDisplayName)];
                if (dn != null)
                {
                    return dn.DisplayName;
                }

                return basePropertyDescriptor.DisplayName;
            }
        }

        public override string Description => basePropertyDescriptor.Description;

        public override bool IsReadOnly => basePropertyDescriptor.IsReadOnly;

        public override string Name => basePropertyDescriptor.Name;

        public override Type PropertyType => basePropertyDescriptor.PropertyType;

        public override bool CanResetValue(object component)
        {
            return basePropertyDescriptor.CanResetValue(component);
        }

        public override object GetValue(object component)
        {
            return basePropertyDescriptor.GetValue(component);
        }

        public override void ResetValue(object component)
        {
            basePropertyDescriptor.ResetValue(component);
        }

        public override bool ShouldSerializeValue(object component)
        {
            return basePropertyDescriptor.ShouldSerializeValue(component);
        }

        public override void SetValue(object component, object value)
        {
            basePropertyDescriptor.SetValue(component, value);
        }
    }
}