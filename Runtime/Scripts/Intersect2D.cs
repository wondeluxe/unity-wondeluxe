using UnityEngine;

namespace Wondeluxe
{
	public static class Intersect2D
	{
		/// <summary>
		/// Find the intersection point between two lines.
		/// </summary>
		/// <param name="a1">The first point of the first line.</param>
		/// <param name="a2">The second point of the first line.</param>
		/// <param name="b1">The first point of the second line.</param>
		/// <param name="b2">The second point of the second line.</param>
		/// <param name="finite">If <c>true</c>, the test will only consider intersections successful if they are contained within the segments of the two lines; otherwise any intersection along the lines axes will be valid.</param>
		/// <param name="point">The point that the two lines intersect will be assigned to this Vector2.</param>
		/// <returns><c>true</c> if the two lines intersect; otherwise <c>false</c>.</returns>

		public static bool Intersect(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2, bool finite, out Vector2 point)
		{
			if ((a1.x == a2.x && a1.y == a2.y) || (b1.x == b2.x && b1.y == b2.y))
			{
				// One of the lines has a length of 0.

				point = Vector2.zero;
				return false;
			}

			float denominator = (b2.y - b1.y) * (a2.x - a1.x) - (b2.x - b1.x) * (a2.y - a1.y);

			if (denominator == 0f)
			{
				// Lines are parellel.

				point = Vector2.zero;
				return false;
			}

			float ua = ((b2.x - b1.x) * (a1.y - b1.y) - (b2.y - b1.y) * (a1.x - b1.x)) / denominator;
			float ub = ((a2.x - a1.x) * (a1.y - b1.y) - (a2.y - a1.y) * (a1.x - b1.x)) / denominator;

			if (finite && (ua < 0 || ua > 1 || ub < 0 || ub > 1))
			{
				point = Vector2.zero;
				return false;
			}

			float x = a1.x + ua * (a2.x - a1.x);
			float y = a1.y + ua * (a2.y - a1.y);

			point = new Vector2(x, y);
			return true;
		}
	}
}