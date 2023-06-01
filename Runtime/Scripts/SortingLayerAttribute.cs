using UnityEngine;

namespace Wondeluxe
{
	/// <summary>
	/// Mark a string or int field as a Sorting Layer. Converts the field to a dropdown list in the Inspector,
	/// populated with the Sorting Layers in the project. String properties will be treated as Sorting Layer Names,
	/// int values will be treated as Sorting Layer IDs.
	/// </summary>

	public class SortingLayerAttribute : PropertyAttribute
	{
	}
}