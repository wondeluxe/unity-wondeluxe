using UnityEditor;
using UnityEngine;
using Wondeluxe;

namespace WondeluxeEditor
{
	[CustomPropertyDrawer(typeof(ImplicitBoolAttribute))]
	public class ImplicitBoolAttributeDrawer : WondeluxePropertyDrawer
	{
		private ImplicitBoolAttribute Attribute => (attribute as ImplicitBoolAttribute);

		public override bool HasCustomLayout => true;

		public override float GetCustomPropertyHeight(SerializedProperty property, GUIContent label)
		{
			if (property.GetImplicitBoolValue())
			{
				return EditorGUI.GetPropertyHeight(property, label, true);
			}

			return EditorGUI.GetPropertyHeight(property, label, false);
		}

		public override void OnCustomGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			// BeginProperty/EndProperty calls not required.
			// Called in WondeluxePropertyDrawer before/after OnCustomGUI.

			if (property.GetImplicitBoolValue())
			{
				EditorGUI.PropertyField(position, property, label, true);
			}
			else
			{
				EditorGUI.LabelField(position, label, new GUIContent(Attribute.FalseLabel));
			}
		}
	}
}