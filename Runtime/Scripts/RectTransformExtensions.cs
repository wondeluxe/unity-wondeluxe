using UnityEngine;

namespace Wondeluxe
{
	/// <summary>
	/// Extension and utility methods for RectTransform.
	/// </summary>

	public static class RectTransformExtensions
	{
		/// <summary>
		/// Returns a point on the edge (of the rectangle) of a given RectTransform, in a specified
		/// direction.
		/// </summary>
		/// <param name="rectTransform">The RectTransform to find the point on the edge of.</param>
		/// <param name="direction">The direction (in world space) in which to find the point.</param>
		/// <returns>A point in world space on the edge of <c>rectTransform</c>.</returns>

		public static Vector3 GetPointOnEdge(this RectTransform rectTransform, Vector2 direction)
		{
			// direction becomes localDirection.

			direction = Quaternion.Inverse(rectTransform.rotation) * direction;

			if (direction != Vector2.zero)
			{
				direction /= Mathf.Max(Mathf.Abs(direction.x), Mathf.Abs(direction.y));
			}

			// direction becomes localPosition.

			direction = rectTransform.rect.center + Vector2.Scale(rectTransform.rect.size, direction * 0.5f);

			return rectTransform.TransformPoint(direction);
		}
	}
}