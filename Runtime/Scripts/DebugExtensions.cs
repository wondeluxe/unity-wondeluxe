using System.Collections.Generic;
using UnityEngine;

namespace Wondeluxe
{
	/// <summary>
	/// Extension and utility methods for Debug.
	/// </summary>

	public static class DebugExtensions
	{
		/// <summary>
		/// Returns the names of a collection of Unity objects.
		/// </summary>
		/// <param name="objects">The objects whose names to return.</param>
		/// <returns>A string containing the names of the objects.</returns>

		public static string ObjectNames<T>(T[] objects) where T : Object
		{
			List<string> names = new List<string>();

			foreach (Object o in objects)
			{
				names.Add(o.name);
			}

			return $"{{{string.Join(", ", names)}}}";
		}

		/// <summary>
		/// Returns the names of a collection of Unity objects.
		/// </summary>
		/// <param name="objects">The objects whose names to return.</param>
		/// <returns>A string containing the names of the objects.</returns>

		public static string ObjectNames<T>(IList<T> objects) where T : Object
		{
			List<string> names = new List<string>();

			foreach (Object o in objects)
			{
				names.Add(o.name);
			}

			return $"{{{string.Join(", ", names)}}}";
		}
	}
}