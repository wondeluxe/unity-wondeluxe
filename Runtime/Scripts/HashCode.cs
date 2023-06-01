namespace Wondeluxe
{
	/// <summary>
	/// Provides utility methods for combining the hash codes for multiple values into a single hash code.
	/// </summary>
	/// <remarks>
	/// This class is modelled off the <a href="https://docs.microsoft.com/en-us/dotnet/api/system.hashcode?view=net-5.0">HashCode</a> struct available from .NET 4.6.1.
	/// </remarks>

	public static class HashCode
	{
		// The hashing algorithm used below is taken from Joshua Bloch's Effective Java.
		// https://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-overriding-gethashcode

		/// <summary>
		/// Combines multiple hash codes into a single hash code.
		/// </summary>
		/// <param name="hashCodes">The hash codes to combine.</param>
		/// <returns>A combined hash code.</returns>

		public static int Combine(params int[] hashCodes)
		{
			unchecked
			{
				int hashCode = 17;

				for (int i = 0; i < hashCodes.Length; i++)
				{
					hashCode = hashCode * 23 + hashCodes[i];
				}

				return hashCode;
			}
		}

		/// <summary>
		/// Combines the hash code for multiple objects into a single hash code.
		/// </summary>
		/// <param name="values">The objects whose hash codes to combine.</param>
		/// <returns>A combined hash code.</returns>

		public static int Combine(params object[] values)
		{
			unchecked
			{
				int hashCode = 17;

				for (int i = 0; i < values.Length; i++)
				{
					if (values[i] != null)
					{
						hashCode = hashCode * 23 + values[i].GetHashCode();
					}
				}

				return hashCode;
			}
		}
	}
}