using UnityEngine;

namespace Wondeluxe
{
	/// <summary>
	/// Override a field's default label in the Inspector.
	/// </summary>

	public class LabelAttribute : PropertyAttribute
	{
		/// <summary>
		/// The label to use for the field.
		/// </summary>

		public string Label { get; private set; }

		/// <summary>
		/// Override a field's default label.
		/// </summary>
		/// <param name="label">The label to use for the field.</param>

		public LabelAttribute(string label)
		{
			Label = label;
		}
	}
}