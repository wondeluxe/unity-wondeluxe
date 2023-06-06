using System;

namespace Wondeluxe.Samples
{
	[Serializable]
	public class DemoData
	{
		public string Language;
		public string Hello;
		public string Goodbye;
		public string One;
		public string Two;
		public string Three;

		public static implicit operator bool(DemoData data)
		{
			if (!string.IsNullOrWhiteSpace(data.Language))
				return false;

			if (!string.IsNullOrWhiteSpace(data.Hello))
				return false;

			if (!string.IsNullOrWhiteSpace(data.Goodbye))
				return false;

			if (!string.IsNullOrWhiteSpace(data.One))
				return false;

			if (!string.IsNullOrWhiteSpace(data.Two))
				return false;

			if (!string.IsNullOrWhiteSpace(data.Three))
				return false;

			return true;
		}
	}
}