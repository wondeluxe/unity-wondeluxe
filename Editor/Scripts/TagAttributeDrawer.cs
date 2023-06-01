using System;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using Wondeluxe;

namespace WondeluxeEditor
{
	[CustomPropertyDrawer(typeof(TagAttribute))]
	public class TagAttributeDrawer : WondeluxePropertyDrawer
	{
		public override bool HasCustomLayout => true;

		public override void OnCustomGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			// BeginProperty/EndProperty calls not required.
			// Called in WondeluxePropertyDrawer before/after OnCustomGUI.

			if (property.propertyType != SerializedPropertyType.String)
			{
				throw new Exception($"Tag attribute used on unsupported property type ({property.propertyType}). Tag attribute may only be used on string properties.");
			}

			string layerName = property.stringValue;

			string[] valueLabels = InternalEditorUtility.tags;
			int valueIndex = Mathf.Clamp(Array.IndexOf(valueLabels, layerName), 0, valueLabels.Length - 1);

			valueIndex = EditorGUIExtensions.DrawOptionsField(label.text, valueIndex, valueLabels, ref position);

			layerName = valueLabels[valueIndex];

			property.stringValue = layerName;
		}
	}
}