using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using Wondeluxe;

namespace WondeluxeEditor
{
	[CustomPropertyDrawer(typeof(NumberedArrayAttribute))]
	public class NumberedArrayAttributeDrawer : WondeluxePropertyDrawer
	{
		private NumberedArrayAttribute Attribute => (attribute as NumberedArrayAttribute);

		public override bool HasCustomLabel => true;

		public override GUIContent GetCustomLabel(SerializedProperty property, GUIContent label)
		{
			Regex regex = new Regex("(?<=\\[)(\\d+)(?=\\]$)");
			Match match = regex.Match(property.propertyPath);

			int index = int.Parse(match.Value);

			label.text = Attribute.LabelForIndex(index);

			return label;
		}
	}
}