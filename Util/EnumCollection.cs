using System;
using System.Collections;
using System.Collections.Generic;

namespace Mhora.Util
{
	public class EnumCollection : ICollection<EnumValue>
	{
		private List<EnumValue>        _values;
		public  IEnumerator<EnumValue> GetEnumerator() => throw new NotImplementedException();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public void Add(EnumValue item)
		{
			_values.Add(item);
		}

		public void Clear()
		{
			_values.Clear();
		}

		public bool Contains(EnumValue item) => _values.Contains(item);

		public void CopyTo(EnumValue[] array, int arrayIndex)
		{
			_values.CopyTo(array, arrayIndex);
		}

		public bool Remove(EnumValue item) => _values.Remove(item);

		public int Count => _values.Count;

		public bool IsReadOnly => false;
	}
}
