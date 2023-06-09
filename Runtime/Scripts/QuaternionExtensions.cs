using System.Text.RegularExpressions;
using UnityEngine;

namespace Wondeluxe
{
	/// <summary>
	/// Extension and utility methods for Quaternion.
	/// </summary>

	public static class QuaternionExtensions
	{
		/// <summary>
		/// Convert a string representation of a Quaternion to a Quaternion.
		/// </summary>
		/// <param name="value">A string representation of a Quaternion.</param>
		/// <returns>The Quaternion represented by <c>value</c>.</returns>

		public static Quaternion Parse(string value)
		{
			Regex regex = new Regex(@"[-]?\d+([\.,](?=\d)\d+)?(e?[+-]\d+)?");
			MatchCollection matches = regex.Matches(value);

			float x = float.Parse(matches[0].Value);
			float y = float.Parse(matches[1].Value);
			float z = float.Parse(matches[2].Value);
			float w = float.Parse(matches[3].Value);

			return new Quaternion(x, y, z, w);
		}
	}
}