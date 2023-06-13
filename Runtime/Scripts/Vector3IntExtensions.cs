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
		/// Linearly interpolates between two vectors.
		/// </summary>
		/// <remarks>
		/// Interpolates between the vectors <c>a</c> and <c>b</c> by the interpolant <c>t</c>.
		/// </remarks>
		/// <param name="a">From value, returned when <c>t = 0</c>.</param>
		/// <param name="b">To value, returned when <c>t = 1</c>.</param>
		/// <param name="t">Value used to interpolate between <c>a</c> and <c>b</c>.</param>
		/// <returns>The interpolated vector between <c>a</c> and <c>b</c>.</returns>

		public static Vector3Int Lerp(Vector3Int a, Vector3Int b, float t)
		{
			Vector3Int delta = b - a;
			return new Vector3Int(a.x + (int)(delta.x * t), a.y + (int)(delta.y * t), a.z + (int)(delta.z * t));
		}

		/// <summary>
		/// Convert a string representation of a Vector3Int to a Vector3Int.
		/// </summary>
		/// <param name="value">A string representation of a Vector3Int.</param>
		/// <returns>The Vector3Int represented by <c>value</c>.</returns>

		public static Vector3Int Parse(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return default;
			}

			Regex regex = new Regex(@"[-]?\d+");
			MatchCollection matches = regex.Matches(value);

			int x = int.Parse(matches[0].Value);
			int y = int.Parse(matches[1].Value);
			int z = int.Parse(matches[2].Value);

			return new Vector3Int(x, y, z);
		}
	}
}