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
	}
}