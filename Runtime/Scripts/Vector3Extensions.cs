using System.Text.RegularExpressions;
using UnityEngine;

namespace Wondeluxe
{
	/// <summary>
	/// Extension and utility methods for Vector3.
	/// </summary>

	public static class Vector3Extensions
	{
		/// <summary>
		/// Convert a string representation of a Vector3 to a Vector3.
		/// </summary>
		/// <param name="value">A string representation of a Vector3.</param>
		/// <returns>The Vector3 represented by <c>value</c>.</returns>

		public static Vector3 Parse(string value)
		{
			Regex regex = new Regex(@"[-]?\d+([\.,](?=\d)\d+)?(e?[+-]\d+)?");
			MatchCollection matches = regex.Matches(value);

			float x = float.Parse(matches[0].Value);
			float y = float.Parse(matches[1].Value);
			float z = float.Parse(matches[2].Value);

			return new Vector3(x, y, z);
		}
	}
}