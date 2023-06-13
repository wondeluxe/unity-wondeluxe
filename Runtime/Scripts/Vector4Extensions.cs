using System.Text.RegularExpressions;
using UnityEngine;

namespace Wondeluxe
{
	/// <summary>
	/// Extension and utility methods for Vector4.
	/// </summary>

	public static class Vector4Extensions
	{
		/// <summary>
		/// Convert a string representation of a Vector4 to a Vector4.
		/// </summary>
		/// <param name="value">A string representation of a Vector4.</param>
		/// <returns>The Vector4 represented by <c>value</c>.</returns>

		public static Vector4 Parse(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return default;
			}

			Regex regex = new Regex(@"[-]?\d+([\.,](?=\d)\d+)?(e?[+-]\d+)?");
			MatchCollection matches = regex.Matches(value);

			float x = float.Parse(matches[0].Value);
			float y = float.Parse(matches[1].Value);
			float z = float.Parse(matches[2].Value);
			float w = float.Parse(matches[3].Value);

			return new Vector4(x, y, z, w);
		}
	}
}