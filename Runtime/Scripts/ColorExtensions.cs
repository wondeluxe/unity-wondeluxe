using System.Text.RegularExpressions;
using UnityEngine;

namespace Wondeluxe
{
	/// <summary>
	/// Extension and utility methods for Color.
	/// </summary>

	public static class ColorExtensions
	{
		/// <summary>
		/// Convert a string representation of a Color to a Color.
		/// </summary>
		/// <param name="value">A string representation of a Color.</param>
		/// <returns>The Color represented by <c>value</c>.</returns>

		public static Color Parse(string value)
		{
			Regex regex = new Regex(@"[-]?\d+([\.,](?=\d)\d+)?(e?[+-]\d+)?");
			MatchCollection matches = regex.Matches(value);

			float r = float.Parse(matches[0].Value);
			float g = float.Parse(matches[1].Value);
			float b = float.Parse(matches[2].Value);
			float a = float.Parse(matches[3].Value);

			return new Color(r, g, b, a);
		}
	}
}