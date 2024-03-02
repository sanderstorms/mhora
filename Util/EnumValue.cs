using System;


namespace Mhora.Util
{
	public class EnumValue
	{
		private Type _type;
		private int  _nr;

		public EnumValue (Enum value)
		{
			_type = value.GetType();
			_nr   = value.Index();
		}

		public override string ToString()
		{
			return Description;
		}

		public static implicit operator Enum(EnumValue enumValue) => enumValue.Value;
		public static implicit operator EnumValue (Enum value) => new EnumValue (value);

		private string Description => Value.GetEnumDescription();
		private string Name        => Enum.GetName(_type, _nr);
		public  int    Index       => _nr;

		private Enum Value
		{
			get
			{
				var value = Enum.ToObject(_type, _nr);
				return ((Enum)value);
			}
			set
			{
				_type = value.GetType();
				var name = Enum.GetName(_type, value);
				_nr = (int)Enum.Parse(_type, name);
			}
		}
	}
}
