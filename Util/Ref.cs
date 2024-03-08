namespace Mhora.Util
{
	public class Ref<T>
	{
		public T Value { get; set; }

		public static implicit operator T (Ref<T> r) => r.Value;

		public Ref(T value)
		{
			Value = value;
		}
	}
}
