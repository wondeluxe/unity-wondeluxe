using UnityEngine;

namespace Wondeluxe
{
	/// <summary>
	/// Mark a string or int property as an Animator State. Converts the Inspector to a dropdown list populated with the Animator State Names in an attached Animator.
	/// String properties will be treated as Animator State Names, int values will be treated as Animator State Name Hashes.
	/// </summary>

	public class AnimatorStateAttribute : PropertyAttribute
	{
		/// <summary>
		/// The layer index to list Animator States for. If layer is less than 0, Animator States on all layers will be listed.
		/// </summary>

		public int Layer { get; private set; }

		/// <summary>
		/// Mark a string or int property as an Animator State. Converts the Inspector to a dropdown list populated with the Animator State Names in an attached Animator.
		/// </summary>
		/// <param name="layer">The layer index to list Animator States for. If layer is less than 0, Animator States on all layers will be listed.</param>

		public AnimatorStateAttribute(int layer = 0)
		{
			Layer = layer;
		}
	}
}