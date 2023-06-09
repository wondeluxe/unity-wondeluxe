using System.Text.RegularExpressions;
using UnityEngine;

namespace Wondeluxe
{
	/// <summary>
	/// Extension and utility methods for Vector3Int.
	/// </summary>

	public static class Vector3IntExtensions
	{
		/// <summary>
		/// Convert a string representation of a Vector3Int to a Vector3Int.
		/// </summary>
		/// <param name="value">A string representation of a Vector3Int.</param>
		/// <returns>The Vector3Int represented by <c>value</c>.</returns>

		public static Vector3Int Parse(string value)
		{
			Regex regex = new Regex(@"[-]?\d+");
			MatchCollection matches = regex.Matches(value);

			int x = int.Parse(matches[0].Value);
			int y = int.Parse(matches[1].Value);
			int z = int.Parse(matches[2].Value);

			return new Vector3Int(x, y, z);
		}
	}
}