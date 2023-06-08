using UnityEngine;
using UnityEditor;
using Wondeluxe;

namespace WondeluxeEditor
{
	[CustomPropertyDrawer(typeof(LabelAttribute))]
	public class LabelAttributeDrawer : WondeluxePropertyDrawer
	{
		private LabelAttribute LabelAttribute => (attribute as LabelAttribute);

		public override bool HasCustomLabel => true;

		public override GUIContent GetCustomLabel(SerializedProperty property, GUIContent label)
		{
			label.text = LabelAttribute.Label;

			return label;
		}
	}
}