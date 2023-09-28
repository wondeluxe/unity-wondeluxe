using UnityEngine;

namespace Wondeluxe
{
	/// <summary>
	/// Extension and utility methods for ContactFilter2D.
	/// </summary>

	public static class ContactFilter2DExtensions
	{
		/// <summary>
		/// Epsilon angle used when filtering contact angles.
		/// </summary>

		public const float NormalAngleEpsilon = 0.01f;

		/// <summary>
		/// Shortcut method for creating a ContactFilter2D that filters for a specified LayerMask.
		/// </summary>
		/// <param name="layerMask">Layer mask to filter for.</param>
		/// <returns>A ContactFilter2D.</returns>

		public static ContactFilter2D CreateFilter(LayerMask layerMask)
		{
			return new ContactFilter2D { layerMask = layerMask, useLayerMask = true };
		}

		/// <summary>
		/// Shortcut method for creating a ContactFilter2D that filters only for contacts that collide <b>into</b> the ray/shapecast.
		/// i.e. the filter ignores contacts that occur behind the ray/shape and contacts on surfaces perpendicular to the ray.
		/// The dot product between the ray direction and a contact's normal returned using this filter should be < 0.
		/// </summary>
		/// <param name="ray">Ray to create the filter for.</param>
		/// <param name="layerMask">Layer mask to filter for.</param>
		/// <returns>A ContactFilter2D.</returns>
		/// <remarks>
		/// This method sets the filter's <c>minNormalAngle</c> and <c>maxNormalAngle</c> based on the ray's angle.
		/// </remarks>

		public static ContactFilter2D CreateFilter(Vector2 ray, LayerMask layerMask)
		{
			float angle = Mathf.Atan2(ray.y, ray.x) * Mathf.Rad2Deg;

			ContactFilter2D filter = new ContactFilter2D { layerMask = layerMask, useLayerMask = true };

			filter.minNormalAngle = angle + 90f + NormalAngleEpsilon;
			filter.maxNormalAngle = angle + 270f - NormalAngleEpsilon;

			if (filter.minNormalAngle < 0f)
			{
				filter.minNormalAngle += 360f;
			}

			if (filter.maxNormalAngle >= 360f)
			{
				filter.maxNormalAngle -= 360f;
			}

			filter.useNormalAngle = true;
			filter.useOutsideNormalAngle = (angle < -90f || angle > 90f);

			return filter;
		}
	}
}