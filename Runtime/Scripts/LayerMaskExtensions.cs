using UnityEngine;

namespace Wondeluxe
{
	/// <summary>
	/// Extension and utility methods for LayerMask.
	/// </summary>

	public static class LayerMaskExtensions
	{
		/// <summary>
		/// Checks if the LayerMask contains a layer.
		/// </summary>
		/// <param name="layerMask">The LayerMask to check.</param>
		/// <param name="layer">The layer to check for.</param>
		/// <returns><c>true</c> if the LayerMask contains <c>layer</c>, otherwise <c>false</c>.</returns>

		public static bool Contains(this LayerMask layerMask, int layer)
		{
			LayerMask mask = 1 << layer;

			return ((layerMask & mask) == mask);
		}

		/// <summary>
		/// Checks if the LayerMask contains another layer mask.
		/// </summary>
		/// <param name="layerMask">The LayerMask to check.</param>
		/// <param name="mask">The other layer mask to check for.</param>
		/// <returns><c>true</c> if the LayerMask contains <c>mask</c>, otherwise <c>false</c>.</returns>

		public static bool Contains(this LayerMask layerMask, LayerMask mask)
		{
			return ((layerMask & mask) == mask);
		}

		/// <summary>
		/// Combines two layer masks.
		/// </summary>
		/// <param name="maskA">The first LayerMask to combine.</param>
		/// <param name="maskB">The second LayerMask to combine.</param>
		/// <returns>A new LayerMask made up of <c>maskA</c> and <c>maskB</c>.</returns>

		public static LayerMask Add(LayerMask maskA, LayerMask maskB)
		{
			return maskA | maskB;
		}

		/// <summary>
		/// Removes a LayerMask from another.
		/// </summary>
		/// <param name="lhs">Layer mask for the left hand side of the operation. The layer mask to remove from.</param>
		/// <param name="rhs">Layer mask for the right hand side of the operation. The layer mask to remove.</param>
		/// <returns>A resulting layer mask with of <c>rhs</c> removed from <c>lhs</c>.</returns>

		public static LayerMask Remove(LayerMask lhs, LayerMask rhs)
		{
			return lhs & ~rhs;
		}
	}
}