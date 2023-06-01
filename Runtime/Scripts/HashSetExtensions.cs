using System.Collections.Generic;

namespace Wondeluxe
{
	/// <summary>
	/// Extension and utility methods for HashSet.
	/// </summary>

	public static class HashSetExtensions
	{
		/// <summary>
		/// Copies the elements of the HashSet<T> to a new array.
		/// </summary>
		/// <typeparam name="T">The type of elements in the hash set.</typeparam>
		/// <param name="set">The HashSet<T> to create an array from.</param>
		/// <returns>An array containing copies of the elements of the HashSet<T>.</returns>

		public static T[] ToArray<T>(this HashSet<T> set)
		{
			T[] array = new T[set.Count];
			set.CopyTo(array);
			return array;
		}
	}
}