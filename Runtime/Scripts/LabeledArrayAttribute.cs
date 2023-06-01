using System;
using UnityEngine;

namespace Wondeluxe
{
	/// <summary>
	/// Supplies a fixed set of text labels to be used for array or list elements in the Inspector.
	/// If the attribute doesn't supply a label for an element (such as when the size of the array exceeds the number of labels), the default "Element {index}" pattern will be used.
	/// </summary>

	public class LabeledArrayAttribute : PropertyAttribute
	{
		/// <summary>
		/// The set of text labels to use.
		/// </summary>

		public string[] Labels { get; private set; }

		/// <summary>
		/// Supplies a fixed set of text labels to be used for array or list elements in the Inspector.
		/// </summary>
		/// <param name="labels">The text labels to use.</param>

		public LabeledArrayAttribute(params string[] labels)
		{
			Labels = labels;
		}

		/// <summary>
		/// Supplies a fixed set of text labels to be used for array or list elements in the Inspector, using the values of an enum.
		/// The name of each value in the enum is used for its integer value.
		/// </summary>
		/// <remarks>
		/// This provides a convenient way mimic dictionary behaviour with an array, using the integer value of an enum value to access an array element.
		/// Labels will default to "Element {index}" where an enum doesn't define an integer/index.
		/// </remarks>
		/// <param name="enumType">The enum Type whose values names to use.</param>

		public LabeledArrayAttribute(Type enumType)
		{
			Array enumValues = Enum.GetValues(enumType);
			Type intType = typeof(int);

			int maxValue = 0;

			for (int i = 0; i < enumValues.Length; i++)
			{
				object enumValue = enumValues.GetValue(i);
				int intValue = (int)Convert.ChangeType(enumValue, intType);

				maxValue = Mathf.Max(intValue, maxValue);
			}

			Labels = new string[maxValue + 1];

			for (int i = 0; i < Labels.Length; i++)
			{
				Labels[i] = $"Element {i}";
			}

			for (int i = 0; i < enumValues.Length; i++)
			{
				object enumValue = enumValues.GetValue(i);
				int intValue = (int)Convert.ChangeType(enumValue, intType);

				Labels[intValue] = enumValue.ToString();
			}
		}

		/// <summary>
		/// Returns the text label for an array element.
		/// </summary>
		/// <param name="index">The index of the array element to return the label for.</param>
		/// <returns>A text label.</returns>

		public string LabelForIndex(int index)
		{
			if (index < Labels.Length)
			{
				return Labels[index];
			}

			return $"Element {index}";
		}
	}
}