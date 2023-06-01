using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using Wondeluxe;

namespace WondeluxeEditor
{
	[CustomPropertyDrawer(typeof(LabeledArrayAttribute))]
	public class LabeledArrayAttributeDrawer : WondeluxePropertyDrawer
	{
		private LabeledArrayAttribute LabeledArrayAttribute => (attribute as LabeledArrayAttribute);

		public override bool HasCustomLabel => true;

		public override GUIContent GetCustomLabel(SerializedProperty property, GUIContent label)
		{
			Regex regex = new Regex("(?<=\\[)(\\d+)(?=\\]$)");
			Match match = regex.Match(property.propertyPath);

			if (match != null && !string.IsNullOrWhiteSpace(match.Value))
			{
				label.text = LabeledArrayAttribute.Labels[int.Parse(match.Value)];
			}

			return label;
		}
	}
}