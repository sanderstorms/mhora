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
using mhora.Delegates;

namespace mhora.Components.Property
{
    public class GlobalizedPropertiesWrapper : ICustomTypeDescriptor
    {
        private readonly object obj;

        public GlobalizedPropertiesWrapper(object _obj)
        {
            obj = _obj;
        }

        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(obj, true);
        }

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(obj, true);
        }

        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(obj, true);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(obj, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(obj, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(obj, true);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(obj, editorBaseType, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(obj, attributes, true);
        }

        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(obj, true);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return obj;
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            var orderedProperties = new ArrayList();
            var retProps          = new PropertyDescriptorCollection(null);
            var baseProps         = TypeDescriptor.GetProperties(obj, attributes, true);
            foreach (PropertyDescriptor oProp in baseProps)
            {
                var attOrder = oProp.Attributes[typeof(PropertyOrderAttribute)];
                if (false == IsPropertyVisible(oProp))
                {
                    continue;
                }

                if (attOrder != null)
                {
                    //
                    // If the attribute is found, then create an pair object to hold it
                    //
                    var poa = (PropertyOrderAttribute)attOrder;
                    orderedProperties.Add(new PropertyOrderPair(oProp, oProp.Name, poa.Order));
                }
                else
                {
                    //
                    // If no order attribute is specifed then given it an order of 0
                    //
                    orderedProperties.Add(new PropertyOrderPair(oProp, oProp.Name, 0));
                }
                //retProps.Add (new GlobalizedPropertyDescriptor(oProp));

                //Console.WriteLine ("Enumerating property {0}", oProp.DisplayName);
                //PGDisplayName invisible = (PGDisplayName)oProp.Attributes[typeof(PGNotVisible)];
                //if (invisible == null)
                //else
                //	Console.WriteLine ("Property {0} is invisible", oProp.DisplayName);
            }

            orderedProperties.Sort();
            foreach (PropertyOrderPair pop in orderedProperties)
            {
                Console.WriteLine("Adding sorted {0}", pop.Name);
                retProps.Add(new GlobalizedPropertyDescriptor(pop.Property));
            }

            return retProps;
        }

        public PropertyDescriptorCollection GetProperties()
        {
            var baseProps = TypeDescriptor.GetProperties(obj, true);
            return baseProps;
        }

        public object GetWrappedObject()
        {
            return obj;
        }

        public bool IsPropertyVisible(PropertyDescriptor prop)
        {
            if (null != prop.Attributes[typeof(PGNotVisible)])
            {
                return false;
            }

            return true;
            //	Console.WriteLine ("Property {0} is invisible", prop.DisplayName);
            //return true;
        }
    }
}