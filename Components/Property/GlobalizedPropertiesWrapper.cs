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
using System.Collections.Generic;
using System.ComponentModel;
using Mhora.Components.Delegates;

namespace Mhora.Components.Property;

public class GlobalizedPropertiesWrapper : ICustomTypeDescriptor
{
	private readonly object obj;

	public GlobalizedPropertiesWrapper(object _obj)
	{
		obj = _obj;
	}

	public string GetClassName() => TypeDescriptor.GetClassName(obj, true);

	public AttributeCollection GetAttributes() => TypeDescriptor.GetAttributes(obj, true);

	public string GetComponentName() => TypeDescriptor.GetComponentName(obj, true);

	public TypeConverter GetConverter() => TypeDescriptor.GetConverter(obj, true);

	public EventDescriptor GetDefaultEvent() => TypeDescriptor.GetDefaultEvent(obj, true);

	public PropertyDescriptor GetDefaultProperty() => TypeDescriptor.GetDefaultProperty(obj, true);

	public object GetEditor(Type editorBaseType) => TypeDescriptor.GetEditor(obj, editorBaseType, true);

	public EventDescriptorCollection GetEvents(Attribute[] attributes) => TypeDescriptor.GetEvents(obj, attributes, true);

	public EventDescriptorCollection GetEvents() => TypeDescriptor.GetEvents(obj, true);

	public object GetPropertyOwner(PropertyDescriptor pd) => obj;

	public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
	{
		var orderedProperties = new List<PropertyOrderPair>();
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
				var poa = (PropertyOrderAttribute) attOrder;
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

			//Mhora.Log.Debug ("Enumerating property {0}", oProp.DisplayName);
			//PGDisplayName invisible = (PGDisplayName)oProp.Attributes[typeof(PGNotVisible)];
			//if (invisible == null)
			//else
			//	Mhora.Log.Debug ("Property {0} is invisible", oProp.DisplayName);
		}

		orderedProperties.Sort();
		foreach (PropertyOrderPair pop in orderedProperties)
		{
			Application.Log.Debug("Adding sorted {0}", pop.Name);
			retProps.Add(new GlobalizedPropertyDescriptor(pop.Property));
		}

		return retProps;
	}

	public PropertyDescriptorCollection GetProperties()
	{
		var baseProps = TypeDescriptor.GetProperties(obj, true);
		return baseProps;
	}

	public object GetWrappedObject() => obj;

	public bool IsPropertyVisible(PropertyDescriptor prop)
	{
		if (null != prop.Attributes[typeof(PGNotVisible)])
		{
			return false;
		}

		return true;
		//	Mhora.Log.Debug ("Property {0} is invisible", prop.DisplayName);
		//return true;
	}
}