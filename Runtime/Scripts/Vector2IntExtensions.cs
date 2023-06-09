using System.Text.RegularExpressions;
using UnityEngine;

namespace Wondeluxe
{
	/// <summary>
	/// Extension and utility methods for Vector2Int.
	/// </summary>

	public static class Vector2IntExtensions
	{
		/// <summary>
		/// Calculate the Dot Product of two vectors.
		/// </summary>
		/// <param name="lhs">Left hand side vector.</param>
		/// <param name="rhs">Right hand side vector.</param>
		/// <returns><c>lhs</c> . <c>rhs</c>.</returns>

		public static int Dot(Vector2Int lhs, Vector2Int rhs)
		{
			return lhs.x * rhs.x + lhs.y * rhs.y;
		}

		/// <summary>
		/// Convert a string representation of a Vector2Int to a Vector2Int.
		/// </summary>
		/// <param name="value">A string representation of a Vector2Int.</param>
		/// <returns>The Vector2Int represented by <c>value</c>.</returns>

		public static Vector2Int Parse(string value)
		{
			Regex regex = new Regex(@"[-]?\d+");
			MatchCollection matches = regex.Matches(value);

			int x = int.Parse(matches[0].Value);
			int y = int.Parse(matches[1].Value);

			return new Vector2Int(x, y);
		}
	}
}