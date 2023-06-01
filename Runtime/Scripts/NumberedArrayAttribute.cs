using UnityEngine;

namespace Wondeluxe
{
	/// <summary>
	/// Modifies the displayed number for each element of an array or list in the Inspector.
	/// </summary>

	public class NumberedArrayAttribute : PropertyAttribute
	{
		/// <summary>
		/// Text prefix to display before the number. Can be null to only show a number.
		/// </summary>

		public string LabelPrefix { get; private set; }

		/// <summary>
		/// The number to display for the first element of the array or list. Most commonly this will be 1.
		/// </summary>

		public int StartNumber { get; private set; }

		/// <summary>
		/// Modifies the displayed number for each element of an array or list in the Inspector.
		/// </summary>
		/// <param name="labelPrefix">Text prefix to display before the number. Can be null to only show a number.</param>
		/// <param name="startNumber">The number to display for the first element of the array or list. Most commonly this will be 1.</param>

		public NumberedArrayAttribute(string labelPrefix, int startNumber)
		{
			LabelPrefix = labelPrefix;
			StartNumber = startNumber;
		}

		/// <summary>
		/// Returns the text label for an array or list element.
		/// </summary>
		/// <param name="index">The index of the array or list element to return the label for.</param>
		/// <returns>A text label.</returns>

		public string LabelForIndex(int index)
		{
			return string.IsNullOrWhiteSpace(LabelPrefix) ? $"{StartNumber + index}" : $"{LabelPrefix} {(StartNumber + index)}";
		}
	}
}