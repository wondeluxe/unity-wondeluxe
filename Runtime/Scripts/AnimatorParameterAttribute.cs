using UnityEngine;

namespace Wondeluxe
{
	/// <summary>
	/// Mark a string or int property as an Animator Parameter. Converts the Inspector to a dropdown list populated with the Animator Parameters in an attached Animator.
	/// String properties will be treated as Animator Parameter Names, int values will be treated as Animator State IDs.
	/// </summary>

	public class AnimatorParameterAttribute : PropertyAttribute
	{
	}
}