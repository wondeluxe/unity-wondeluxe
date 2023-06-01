using UnityEngine;

namespace Wondeluxe
{
	/// <summary>
	/// Displays a text label for a field whose type overrides the implicit bool operator, and whose value evaluates to false.
	/// </summary>

	public class ImplicitBoolAttribute : PropertyAttribute
	{
		/// <summary>
		/// Text label to display when the field evaluates to false. Defaults to "False".
		/// </summary>

		public string FalseLabel { get; private set; } = "False";

		/// <summary>
		/// Displays a text label for a field whose type overrides the implicit bool operator, and whose value evaluates to false.
		/// </summary>
		/// <param name="falseLabel">Text label to display when the field evaluates to false.</param>

		public ImplicitBoolAttribute(string falseLabel)
		{
			FalseLabel = string.IsNullOrWhiteSpace(falseLabel) ? "" : falseLabel;
		}
	}
}